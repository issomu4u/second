using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SellerVendor.Areas.Seller.Models
{
    public class tbl_return_upload
    {
        public int id { get; set; }
        public int? tbl_seller_id { get; set; }
        public int? tbl_marketplace_id { get; set; }
        public DateTime? uploaded_on { get; set; }
        public int? uploaded_by { get; set; }
        public DateTime? processing_time { get; set; }
        public DateTime? complition_time { get; set; }
        public short? source { get; set; }
        public string file_status { get; set; }
        public string file_name { get; set; }
        public int? return_count { get; set; }
        public int? voucher_running_no { get; set; }
    }
}