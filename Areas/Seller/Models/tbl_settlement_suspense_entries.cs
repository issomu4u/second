using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SellerVendor.Areas.Seller.Models
{
    public class tbl_settlement_suspense_entries
    {
        public int id { get; set; }
        public int? tbl_settlement_upload_id { get; set; }
        public string suspense_details { get; set; }
        public double? amount { get; set; }
        public string order_id_if_present { get; set; }
    }
}