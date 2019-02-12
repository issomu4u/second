using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SellerVendor.Areas.Seller.Models
{
    public class tbl_publish_item
    {
        public DateTime created_on { get; set; }
        public int id { get; set; }
        public int is_active { get; set; }
        public int tbl_inventory_id { get; set; }
        public int tbl_seller_id { get; set; }
        public int t_Available_qty { get; set; }
        public int t_current_item { get; set; }
        public int t_MarketPlace_id { get; set; }
        public int t_New_ItemCount { get; set; }
        public int t_Total_Item { get; set; }
        public int t_virtualItemCount { get; set; }
    }

    public partial class publishDetails
    {
        public int tbl_inventory_id { get; set; }
        public int m_marketplace_id { get; set; }
        public int CurrentItem { get; set; }
        public int NewItem { get; set; }
        public int TotalItem { get; set; }
        public string MarketPlaceName { get; set; }
        public string ItemName { get; set; }
        public int total_count { get; set; }
        public string ImagePAth { get; set; }
    }
}