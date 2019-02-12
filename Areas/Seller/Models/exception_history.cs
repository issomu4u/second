using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SellerVendor.Areas.Seller.Models
{
    public class exception_history
    {
        public int id { get; set; }
        public string tbl_seller_id { get; set; }
        public string source_file { get; set; }
        public string page { get; set; }
        public string exception { get; set; }
        public DateTime exception_date { get; set; }
    }
}