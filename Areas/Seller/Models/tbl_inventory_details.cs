using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SellerVendor.Areas.Seller.Models
{
    public class tbl_inventory_details
    {
        public Nullable<double> bought_price { get; set; }
        public Nullable<DateTime> created_on { get; set; }
        public int id { get; set; }
        public string item_uid { get; set; }
        public Nullable<int> m_item_status_id { get; set; }
        public Nullable<double> sold_price { get; set; }
        public Nullable<double> tax_charged { get; set; }
        public Nullable<int> tax_paid { get; set; }
        public Nullable<int> tbl_inventory_id { get; set; }
        public Nullable<int> tbl_inventory_warehouse_transfers_id { get; set; }
        public Nullable<int> tbl_purchase_details_id { get; set; }
        public Nullable<DateTime> updated_on { get; set; }
        public Nullable<int> isactive { get; set; }
        public Nullable<int> tbl_sellers_id { get; set; }
        public int? n_virtualStatus { get; set; }
        public string item_serial_No { get; set; }
        public int? item_Quantity { get; set; }
        public string item_serialNo { get; set; }
        public string batch_no { get; set; }
        public int? m_marketplace_id { get; set; }
    }
}