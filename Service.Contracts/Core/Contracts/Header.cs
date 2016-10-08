using System;
using System.Runtime.Serialization;

namespace Demandtec.DealManagement.Core.Contracts
{
    [DataContract]
    public class Header
    {
        [DataMember]
        public int ID { get; set; }

        [DataMember]
        public String Name { get; set; }

        [DataMember]
        public String Token { get; set; }

        [DataMember]
        public String RetailerCode { get; set; }

        [DataMember]
        public String CultureInfo { get; set; }

        public override string ToString()
        {
            return string.Format("{0} | {1} | {2} | {3} | {4} ", ID, Name, RetailerCode, CultureInfo, Token);
        }
    }
}