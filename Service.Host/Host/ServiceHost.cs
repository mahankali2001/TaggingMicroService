using System;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace Service.Host
{

    public class ServiceHost<T> : System.ServiceModel.ServiceHost
    {
        #region ServiceHost Constructor

        public ServiceHost() : base(typeof(T)) { }
        public ServiceHost(params string[] baseAddresses) : base(typeof(T), baseAddresses.Select((address) => new Uri(address)).ToArray()) { }
        public ServiceHost(params Uri[] baseAddresses) : base(typeof(T), baseAddresses) { }
        public ServiceHost(T singleton, params string[] baseAddresses) : base(singleton, baseAddresses.Select((address) => new Uri(address)).ToArray()) { }
        public ServiceHost(T singleton) : base(singleton) { }
        public ServiceHost(T singleton, params Uri[] baseAddresses) : base(singleton, baseAddresses) { }

        #endregion


        public void EnableMetadataExchange()
        {
            EnableMetadataExchange(true);
        }

        public void EnableMetadataExchange(bool enableHttpGet)
        {
            if (State == CommunicationState.Opened)
            {
                throw new InvalidOperationException("Host is already opened");
            }

            ServiceMetadataBehavior metadataBehavior;
            metadataBehavior = Description.Behaviors.Find<ServiceMetadataBehavior>();

            if (metadataBehavior == null)
            {
                metadataBehavior = new ServiceMetadataBehavior();
                Description.Behaviors.Add(metadataBehavior);

                if (BaseAddresses.Any((uri) => uri.Scheme == "http"))
                {
                    metadataBehavior.HttpGetEnabled = enableHttpGet;
                }

                if (BaseAddresses.Any((uri) => uri.Scheme == "https"))
                {
                    metadataBehavior.HttpsGetEnabled = enableHttpGet;
                }
            }
            AddAllMexEndPoints();
        }

        public void AddAllMexEndPoints()
        {
            Debug.Assert(HasMexEndpoint == false);

            foreach (Uri baseAddress in BaseAddresses)
            {
                Binding binding = null;
                switch (baseAddress.Scheme)
                {
                    case "net.tcp":
                        {
                            binding = MetadataExchangeBindings.CreateMexTcpBinding();
                            break;
                        }
                    case "net.pipe":
                        {
                            binding = MetadataExchangeBindings.CreateMexNamedPipeBinding();
                            break;
                        }
                    case "http":
                        {
                            binding = MetadataExchangeBindings.CreateMexHttpBinding();
                            break;
                        }
                    case "https":
                        {
                            binding = MetadataExchangeBindings.CreateMexHttpsBinding();
                            break;
                        }
                }
                if (binding != null)
                {
                    AddServiceEndpoint(typeof(IMetadataExchange), binding, "mex");
                }
            }
        }

        public bool HasMexEndpoint
        {
            get
            {
                return Description.Endpoints.Any(endpoint => endpoint.Contract.ContractType == typeof(IMetadataExchange));
            }
        }
        public ServiceThrottle Throttle
        {
            get
            {
                if (State != CommunicationState.Opened)
                {
                    throw new InvalidOperationException("Host is not opened");
                }

                ChannelDispatcher dispatcher = OperationContext.Current.Host.ChannelDispatchers[0] as ChannelDispatcher;
                return dispatcher.ServiceThrottle;
            }
        }

        public bool SecurityAuditEnabled
        {
            get
            {
                ServiceSecurityAuditBehavior securityAudit = Description.Behaviors.Find<ServiceSecurityAuditBehavior>();
                if (securityAudit != null)
                {
                    return securityAudit.MessageAuthenticationAuditLevel == AuditLevel.SuccessOrFailure &&
                           securityAudit.ServiceAuthorizationAuditLevel == AuditLevel.SuccessOrFailure;
                }
                else
                {
                    return false;
                }
            }
            set
            {
                if (State == CommunicationState.Opened)
                {
                    throw new InvalidOperationException("Host is already opened");
                }
                ServiceSecurityAuditBehavior securityAudit = Description.Behaviors.Find<ServiceSecurityAuditBehavior>();
                if (securityAudit == null && value == true)
                {
                    securityAudit = new ServiceSecurityAuditBehavior();
                    securityAudit.MessageAuthenticationAuditLevel = AuditLevel.SuccessOrFailure;
                    securityAudit.ServiceAuthorizationAuditLevel = AuditLevel.SuccessOrFailure;
                    Description.Behaviors.Add(securityAudit);
                }
            }
        }

        public virtual T Singleton
        {
            get
            {
                if (SingletonInstance == null)
                {
                    return default(T);
                }
                Debug.Assert(SingletonInstance is T);
                return (T)SingletonInstance;
            }
        }
    }


}