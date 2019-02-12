using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SellerVendor.Areas.Seller.Models
{
    public class tbl_inventory_sku_mapping
    {
        public int id { get; set; }
        public int m_marketplace_id { get; set; }
        public int tbl_inventory_id { get; set; }
        public int tbl_sellers_id { get; set; }
        public string unique_id { get; set; }
    }
}