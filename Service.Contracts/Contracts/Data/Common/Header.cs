using System;
using System.Runtime.Serialization;

namespace Service.Contracts.Data
{
    [DataContract(Namespace = ServiceConstants.DataContractNameSpace)]
    public class Header
    {
        [DataMember]
        public String UserName { get; set; }

        [DataMember]
        public int AppUserId { get; set; }

        [DataMember]
        public String Token { get; set; }

        [DataMember]
        public String RetailerCode { get; set; }

        [DataMember]
        public String CultureInfo { get; set; }

        public override string ToString()
        {
            return string.Format("{0} | {1} | {2} | {3} ",  UserName, RetailerCode, CultureInfo, Token);
        }
    }
}