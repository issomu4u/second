using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SellerVendor.Areas.Seller.Models
{
    public class tbl_history_return_details
    {
        public int id { get; set; }
        public int? tbl_history_id { get; set; }
        public int? tbl_seller_id { get; set; }
        public string customer_comments { get; set; }
        public string license_plate_no { get; set; }
        public string reason { get; set; }
        public string detailed_disposition { get; set; }
        public string fullfillment_center_id { get; set; }
        public string fnsku { get; set; }
    }
}