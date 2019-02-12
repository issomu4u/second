using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SellerVendor.Areas.Seller.Models
{
    public class tbl_warehouse_transfer
    {
        public int id { get; set; }
        public DateTime created_on { get; set; }
        public int from_warehouse_id { get; set; }
        public int to_warehouse_id { get; set; }       
        public int item_total_count { get; set; }
        public int Item_Transfer_Count { get; set; }
        public int tbl_inventory_Id { get; set; }
        public int is_active { get; set; }
        public int tbl_seller_id { get; set; }
        public string t_ItemName { get; set; }
        public int? Marketplace_id { get; set; }
    }
}