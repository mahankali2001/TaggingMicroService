using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MicroService.Models
{
    public class VendorsAssociatedWithUser
    {
        public string Email { get; set; }
        public object Linktype { get; set; }
        public int Retailerpartyid { get; set; }
        public string UserFullName { get; set; }
        public int UserPartyId { get; set; }
        public string UserPhoneNumber { get; set; }
        public string Userlogin { get; set; }
        public string Vendorname { get; set; }
        public int Vendorpartyid { get; set; }
    }

    public class GetVendorRepsForVendorResult
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int PageCount { get; set; }
        public int RowCount { get; set; }
        public List<VendorsAssociatedWithUser> Results { get; set; }
    }

    public class ListOfUserWrapper
    {
        public GetVendorRepsForVendorResult GetVendorRepsForVendorResult { get; set; }
    }

}
