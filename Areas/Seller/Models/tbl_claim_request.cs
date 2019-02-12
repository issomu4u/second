using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SellerVendor.Areas.Seller.Models
{
    public class tbl_claim_request
    {
        public int? id { get; set; }
        public int? tbl_history_id { get; set; }
        public DateTime? claim_request_date { get; set; }
        public DateTime? claim_created_on { get; set; }
        public short? claim_request_type { get; set; }
    }
}