using SellerVendor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SellerVendor.Areas.Seller.Models
{
    public class SellerUtility
    {
        public tbl_seller_vendors ob_tbl_seller_vendors { get; set; }
        public tbl_seller_warehouses ob_tbl_seller_warehouses { get; set; }
        public tbl_purchase ob_tbl_purchase { get; set; }
        public tbl_purchase_details ob_tbl_purchase_details { get; set; }
        public m_color ob_m_color { get; set; }
        public tbl_inventory ob_tbl_inventory { get; set; }
        public m_item_category ob_m_item_category { get; set; }
        public m_item_subcategory ob_m_item_subcategory { get; set; }
        public tbl_tax ob_tbl_tax { get; set; }
        public tbl_inventory_details ob_tbl_inventory_details { get; set; }
        public tbl_item_category ob_tbl_item_category { get; set; }
        public tbl_item_subcategory ob_tbl_item_subcategory { get; set; }
        public tbl_vendor_payment ob_tbl_vendor_payment { get; set; }
        public m_item_status ob_m_item_status { get; set; }
       
        public m_marketplace ob_m_marketplace { get; set; }
        public tbl_sales_order ob_tbl_sales_order { get; set; }
        public tbl_sales_order_details ob_tbl_sales_order_details { get; set; }
        public tbl_sellermarketplace ob_tbl_sellermarketplace { get; set; }
        public tbl_sales_order_status ob_tbl_sales_order_status { get; set; }
        public tbl_customer_details ob_tbl_customer_details { get; set; }
        public tbl_courier_comapny ob_tbl_courier_comapny { get; set; }
        public tbl_order_delivery ob_tbl_order_delivery { get; set; }
        public m_fullfilled ob_m_fullfilled { get; set; }
        public tbl_order_history ob_tbl_order_history { get; set; }
        public m_tbl_expense ob_m_tbl_expense { get; set; }
        public tbl_settlement_upload ob_tbl_settlement_upload { get; set; }
        public tbl_settlement_suspense_entries ob_tbl_settlement_suspense_entries { get; set; }
        
       
    }

    
}