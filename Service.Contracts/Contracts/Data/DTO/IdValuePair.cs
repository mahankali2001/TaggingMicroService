using System.Data;
using System.Runtime.Serialization;

namespace Service.Contracts.Data
{
    [DataContract]
    public class IdValuePair
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Value { get; set; }


        public static IdValuePair LoadFrom(DataRow row)
        {
            return new IdValuePair()
            {
                Id = row.Field<int>("TagId"),
                Value = row.Field<string>("Value")
            };
        }

        public static IdValuePair LoadPartyIdFullName(DataRow row)
        {
            return new IdValuePair()
            {
                Id = row.Field<int>("PartyId"),
                Value = row.Field<string>("FullName")
            };
        }
    }
}