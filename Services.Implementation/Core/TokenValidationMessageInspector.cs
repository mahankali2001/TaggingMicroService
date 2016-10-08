using System;
using System.IdentityModel.Tokens;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using Demandtec.DealManagement.Core.Contracts;

namespace Demandtec.DealManagement.Services.Core
{
    internal class TokenValidationMessageInspector : IDispatchMessageInspector
    {
        #region IDispatchMessageInspector Members

        public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
        {
            //Header d = GenericContext<Header>.Current.Value;
            //if (!String.IsNullOrEmpty(d.Token))
            //{
            //    var myGuid = new Guid(d.Token);
            //    bool v = Validator.Instance().Validate(myGuid);

            //    if (v) Console.Write("User Validated .. ");
            //    else
            //    {
            //        Console.Write("User not Validated .. ");
            //        throw new SecurityTokenException(string.Format("Invalid Token - {0}", d.Token));
            //    }
            //}
            //Console.WriteLine("IDispatchMessageInspector.AfterReceiveRequest called.");
            return null;
        }

        public void BeforeSendReply(ref Message reply, object correlationState)
        {
            Console.WriteLine("IDispatchMessageInspector.BeforeSendReply called.");
        }

        #endregion
    }
}