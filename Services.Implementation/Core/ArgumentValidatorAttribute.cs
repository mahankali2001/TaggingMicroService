using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Web;
using Service.Contracts.Services;

namespace Services.Implementation.Core
{
    public class ArgumentValidatorAttribute : Attribute,  IContractBehaviorAttribute,IContractBehavior
    {
        public Type TargetContract
        {
            get { return typeof(ITaggingExternalApi); }
        }

        public void Validate(ContractDescription contractDescription, ServiceEndpoint endpoint)
        {
            return;
        }

        public void ApplyDispatchBehavior(ContractDescription contractDescription, ServiceEndpoint endpoint, DispatchRuntime dispatchRuntime)
        {
            foreach (DispatchOperation op in dispatchRuntime.Operations)
                op.ParameterInspectors.Add(new ArgumentValidatorInspector());
        }
        public void ApplyClientBehavior(ContractDescription contractDescription, ServiceEndpoint endpoint, ClientRuntime clientRuntime){return;}

        public void AddBindingParameters(ContractDescription contractDescription, ServiceEndpoint endpoint, BindingParameterCollection bindingParameters){return;}
    }
}