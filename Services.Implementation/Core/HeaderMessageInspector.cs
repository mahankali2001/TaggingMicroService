using System;
using System.IdentityModel.Tokens;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Web;
using System.Xml;
using Services.Implementation.Config;
using Service.Contracts.Data;

namespace Services.Implementation.Core
{
    internal class HeaderMessageInspector : IDispatchMessageInspector
    {
        #region IDispatchMessageInspector Members

        public object AfterReceiveRequest(ref System.ServiceModel.Channels.Message request, IClientChannel channel, InstanceContext instanceContext)
        {
            var h = GenericContext<Header>.Current;
            if (h == null) return null;
            DisplayHeader();
            Header d= h.Value;
            if (!String.IsNullOrEmpty(d.Token))
            {
                var myGuid = new Guid(d.Token);
                FillContext(d);
                bool v = Validator.Instance().Validate(myGuid);
                if (v) Console.Write("User Validated .. ");
                else
                {
                    Console.Write("User not Validated .. ");
                    throw new SecurityTokenException(string.Format("Invalid Token - {0}", d.Token));
                }
            }
            Console.WriteLine("IDispatchMessageInspector.AfterReceiveRequest called.");
            return null;
        }

        private void FillContext(Header header)
        {
        }

        public void BeforeSendReply(ref System.ServiceModel.Channels.Message reply, object correlationState)
        {
            Console.WriteLine("IDispatchMessageInspector.BeforeSendReply called.");
        }


        private void DisplayHeader()
        {
            var headers = OperationContext.Current.IncomingMessageHeaders;
            if (headers == null) return;
            for (int i = 0; i < headers.Count; i++)
            {
                MessageHeaderInfo h = OperationContext.Current.IncomingMessageHeaders[i];
                if (!h.Actor.Equals(String.Empty))
                    Console.WriteLine("\t" + h.Actor);
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("\t" + h.Name);
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("\t" + h.Namespace);
                Console.WriteLine("\t" + h.Relay);
                if (h.IsReferenceParameter == true)
                {
                    Console.WriteLine("IsReferenceParameter header detected: " + h.ToString());
                }
            }
        }

        #endregion
    }
}