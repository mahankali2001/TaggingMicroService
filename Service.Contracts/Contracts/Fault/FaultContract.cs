using System;
using System.Runtime.Serialization;
using Service.Contracts.Core;
namespace Service.Contracts.Fault
{
    [DataContract]
    public class FaultContract : ExposedFaultContract

    {
        [DataMember]
        public string StackTrace { get; set; }
        [DataMember]
        public string Exception { get; set; }
    }

    [DataContract]
    public class ExposedFaultContract
    {
        private long  g;

        [DataMember]
        public string Message { get; set; }

        [DataMember]
        public long ErrorId
        {
            get { return g; }
            set { g = value; }
        }

    }
}