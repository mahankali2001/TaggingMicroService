using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Web;
using Core.Logger;
using Service.Contracts.Fault;

namespace Services.Implementation.Core
{

    [AttributeUsage(AttributeTargets.Class)]
    public class ErrorHandlerBehavior : Attribute, IErrorHandler, IServiceBehavior
    {
        protected Type ServiceType { get; set; }

        #region IErrorHandler Members

        bool IErrorHandler.HandleError(Exception error)
        {
            ErrorHandlerHelper.WriteToLogFile(error,LogType.Error);
            return false;
        }

        void IErrorHandler.ProvideFault(Exception error, MessageVersion version, ref Message fault)
        {
            if (error.GetType().IsGenericType && error is FaultException)
            {
                Debug.Assert(error.GetType().GetGenericTypeDefinition() == typeof (FaultException<>));
                return;
            }
            else
            {
                // Generate fault message manually
                if ((WebOperationContext.Current != null))
                {
                    HttpStatusCode status = HttpStatusCode.InternalServerError;
                    if (error is System.Data.ObjectNotFoundException)
                    {
                        status = HttpStatusCode.NotFound;
                    }
                    else if (error is ArgumentNullException)
                    {
                        status = HttpStatusCode.BadRequest;
                    }
                    else if (error is ApplicationException)
                    {
                        status = HttpStatusCode.InternalServerError;
                    }
                    else if (error is NotImplementedException)
                    {
                        status = HttpStatusCode.NotImplemented;
                    }
                    else if ((error is ArgumentException) ||  (error is System.ComponentModel.DataAnnotations.ValidationException))
                    {
                        status = HttpStatusCode.BadRequest;
                    }
                    else if (error is SerializationException)
                    {
                        status = HttpStatusCode.BadRequest;
                    }
                    else
                    {
                        // Mask the unknown error to generic message
                        // Note - The HandleError method still logs the original exception
                        error = new Exception("An error occurred while processing the request");
                    }
                    SetStatus(status);
                    CreateJsonErrorMessage(error, version, ref fault, status);
                }
                else
                {
                    MessageFault messageFault = MessageFault.CreateFault(
                        new FaultCode("Sender"),
                        new FaultReason(error.Message),
                        error,
                        new NetDataContractSerializer());
                    fault = Message.CreateMessage(version, messageFault, null);
                }
            }
        }

        private void CreateJsonErrorMessage(Exception error, MessageVersion version, ref Message fault, HttpStatusCode statusCode)
        {
            // Create message
            var jsonError = new ExposedFaultContract
                                {
                                    Message = error.Message,
                                    ErrorId = DateTime.Now.ToUnixTime()
                                };
            fault = Message.CreateMessage(version, "", jsonError,new DataContractJsonSerializer(typeof (ExposedFaultContract)));

            // Tell WCF to use JSON encoding rather than default XML
            var wbf = new WebBodyFormatMessageProperty(WebContentFormat.Json);
            fault.Properties.Add(WebBodyFormatMessageProperty.Name, wbf);

            // Modify response
            var rmp = new HttpResponseMessageProperty
                          {
                              StatusCode = statusCode,
                              StatusDescription = "Bad Request",
                          };
            rmp.Headers[HttpResponseHeader.ContentType] = "application/json";
            fault.Properties.Add(HttpResponseMessageProperty.Name, rmp);
        }

        private void SetStatus(HttpStatusCode status)
        {
            if (WebOperationContext.Current != null)
            {
                WebOperationContext.Current.OutgoingResponse.StatusCode = status;
                //WebOperationContext.Current.OutgoingResponse.bSuppressEntityBody = true;
            }
        }

        #endregion

        #region IServiceBehavior Members

        void IServiceBehavior.Validate(ServiceDescription description, ServiceHostBase host)
        {
        }

        void IServiceBehavior.AddBindingParameters(ServiceDescription description, ServiceHostBase host,
                                                   Collection<ServiceEndpoint> endpoints,
                                                   BindingParameterCollection parameters)
        {
        }

        void IServiceBehavior.ApplyDispatchBehavior(ServiceDescription description, ServiceHostBase host)
        {
            ServiceType = description.ServiceType;
            foreach (ChannelDispatcher dispatcher in host.ChannelDispatchers)
            {
                dispatcher.ErrorHandlers.Add(this);
            }
        }

        #endregion
    }
}