using System;
using System.Runtime.Serialization;
using Service.Contracts.Core;
namespace Service.Contracts.Fault
{
    [DataContract]
    public class GenericFault 
    {
        [DataMember]
        public string Code { get; set; }
    }
}