using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SellerVendor.Areas.Seller.Models
{
    public class InventoryDetailsView
    {
        public Nullable<double> bought_price { get; set; }
       
     
        public Nullable<int> item_uid { get; set; }
        public Nullable<int> m_item_status_id { get; set; }
        public Nullable<double> sold_price { get; set; }
        public Nullable<double> tax_charged { get; set; }
        public Nullable<int> tax_paid { get; set; }
        public Nullable<int> tbl_inventory_id { get; set; }
        public Nullable<int> tbl_inventory_warehouse_transfers_id { get; set; }
        public Nullable<int> tbl_purchase_details_id { get; set; }
      
      
        public Nullable<int> tbl_sellers_id { get; set; }
        public string SellerName { get; set; }
    }
}