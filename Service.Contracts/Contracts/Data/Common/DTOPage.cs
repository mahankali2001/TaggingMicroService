using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Service.Contracts.Data.Common
{
    [DataContract(Namespace = ServiceConstants.DataContractNameSpace, Name = "DTOPageOf{0}")]
    public class DTOPage<T> where T : class
    {
        [DataMember(Order = 1)]
        public int CurrentPage { get; set; }

        [DataMember(Order = 2)]
        public int PageSize { get; set; }

        [DataMember(Order = 3)]
        public int PageCount { get; set; }

        [DataMember(Order = 4)]
        public int RowCount { get; set; }

        [DataMember(Order = 5)]
        public List<T> Results { get; set; }
    }
}
