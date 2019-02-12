using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SellerVendor.Areas.Seller.Models
{
    public class tbl_api_meter
    {
        public int id { get; set; }
        public int seller_id { get; set; }
        public int m_marketplace_id { get; set; }
        public DateTime till_datetime { get; set; }
        public string api_type { get; set; }
        public string MarketId { get; set; }
    }
}