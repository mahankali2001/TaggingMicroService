using System.ServiceModel;

namespace Service.Contracts
{
    public class ServiceConstants
    {
        public const string ServiceNameSpace = "http://www.ibm.com/xyz/2016/03/Services";
        public const string DataContractNameSpace = "http://www.ibm.com/xyz/2016/03/Services/Data";
        public const string InternalVersionNumber = "1.0";
        public const string ExternalVersionNumber = "v1";

        public const string DateFormat = "yyyy-MM-dd";

        public const InstanceContextMode InstanceMode = InstanceContextMode.PerCall;
    }
}