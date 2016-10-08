using System;
using System.Collections.ObjectModel;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using Core.Logger;

namespace Services.Implementation.Core
{
    public class WCFMessageLoggerBehavior : Attribute, IDispatchMessageInspector, IServiceBehavior
    {
        #region IDispatchMessageInspector Members

        public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
        {
            ErrorHandlerHelper.WriteToLogFile("WCF:[Recieve]" + request, LogType.Info);
            return null;
        }

        public void BeforeSendReply(ref Message reply, object correlationState)
        {
            ErrorHandlerHelper.WriteToLogFile("WCF:[Send]" + reply, LogType.Info);
        }

        #endregion

        #region IServiceBehavior Members

        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
        }

        public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase,
                                         Collection<ServiceEndpoint> endpoints,
                                         BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            foreach (ChannelDispatcher dispatcher in serviceHostBase.ChannelDispatchers)
            {
                foreach (EndpointDispatcher endpoint in dispatcher.Endpoints)
                {
                    endpoint.DispatchRuntime.MessageInspectors.Add(this);
                }
            }
        }

        #endregion
    }
}