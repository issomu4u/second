using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SellerVendor.Areas.Seller.Models
{
    public class tbl_Salesledger_tax
    {
        public int id { get; set; }
        public string tax_name { get; set; }
        public double? tax_percentage { get; set; }
    }
}