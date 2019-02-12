using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SellerVendor.Areas.Seller.Models
{
    public class tbl_seller_accounting_pkg_details
    {
        public int id { get; set; }
        public int? accounting_sft_id { get; set; }
        public string guid { get; set; }
        public DateTime? date_created { get; set; }
        public int seller_id { get; set; }
    }
}