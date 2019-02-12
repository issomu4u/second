using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SellerVendor.Areas.Seller.Models.DBContext
{
    public class SellerContext : DbContext
    {
        public SellerContext()
            : base("SellerConnection")
        {
        }
        //alter table footable drop foreign key fooconstraint     for remove foreign key in mysql
        public DbSet<tbl_seller_vendors> tbl_seller_vendors { get; set; }
        public DbSet<tbl_purchase> tbl_purchase { get; set; }
        public DbSet<tbl_purchase_details> tbl_purchase_details { get; set; }
        public DbSet<tbl_seller_warehouses> tbl_seller_warehouses { get; set; }
        public DbSet<m_color> m_color { get; set; }
        public DbSet<tbl_inventory> tbl_inventory { get; set; }
        public DbSet<tbl_item_category> tbl_item_category { get; set; }
        public DbSet<tbl_item_subcategory> tbl_item_subcategory { get; set; }
        public DbSet<tbl_tax> tbl_tax { get; set; }
        public DbSet<tbl_inventory_details> tbl_inventory_details { get; set; }
        public DbSet<tbl_vendor_payment> tbl_vendor_payment { get; set; }
        public DbSet<m_item_status> m_item_status { get; set; }
        public DbSet<tbl_warehouse_transfer> tbl_warehouse_transfer { get; set; }

        public DbSet<tbl_sellermarketplace> tbl_sellermarketplace { get; set; }
        public DbSet<m_saleorder_entry_src> m_saleorder_entry_src { get; set; }
        public DbSet<tbl_inventory_sku_mapping> tbl_inventory_sku_mapping { get; set; }
        public DbSet<tbl_sales_order_details> tbl_sales_order_details { get; set; }
        public DbSet<tbl_sales_order> tbl_sales_order { get; set; }
        public DbSet<tbl_item_sale_association> tbl_item_sale_association { get; set; }
        public DbSet<tbl_publish_item> tbl_publish_item { get; set; }
        public DbSet<tbl_sales_order_status> tbl_sales_order_status { get; set; }
        public DbSet<tbl_customer_details> tbl_customer_details { get; set; }
        public DbSet<tbl_seller_setting> tbl_seller_setting { get; set; }
        public DbSet<tbl_courier_comapny> tbl_courier_comapny { get; set; }
        public DbSet<tbl_order_delivery> tbl_order_delivery { get; set; }
        public DbSet<m_fullfilled> m_fullfilled { get; set; }
        public DbSet<tbl_details_items> tbl_details_items { get; set; }
        public DbSet<tbl_order_history> tbl_order_history { get; set; }
        public DbSet<tbl_order_payment_ledger> tbl_order_payment_ledger { get; set; }
        public DbSet<m_referenced> m_referenced { get; set; }
        public DbSet<m_settlement_fee> m_settlement_fee { get; set; }
        public DbSet<m_tbl_expense> m_tbl_expense { get; set; }
        public DbSet<m_settlement_transaction_type> m_settlement_transaction_type { get; set; }
        public DbSet<tbl_settlement_order> tbl_settlement_order { get; set; }
        public DbSet<tbl_settlement_upload> tbl_settlement_upload { get; set; }
        public DbSet<tbl_imp_bank_transfers> tbl_imp_bank_transfers { get; set; }
        public DbSet<m_report_color> m_report_color { get; set; }
        public DbSet<tbl_order_upload> tbl_order_upload { get; set; }
        public DbSet<tbl_claim_request> tbl_claim_request { get; set; }
        public DbSet<tbl_category_slabs> tbl_category_slabs { get; set; }
        public DbSet<tbl_inventory_effectiveprice> tbl_inventory_effectiveprice { get; set; }
        public DbSet<tbl_seller_api_plan> tbl_seller_api_plan { get; set; }
        public DbSet<tbl_seller_accounting_pkg_details> tbl_seller_accounting_pkg_details { get; set; }
        public DbSet<tbl_settlement_suspense_entries> tbl_settlement_suspense_entries { get; set; }
        public DbSet<tbl_history_return_details> tbl_history_return_details { get; set; }
        public DbSet<tbl_return_upload> tbl_return_upload { get; set; }
        public DbSet<exception_history> exception_history { get; set; }
        public DbSet<tbl_product_upload> tbl_product_upload { get; set; }
        public DbSet<tbl_Salesledger_tax> tbl_Salesledger_tax { get; set; }
        public DbSet<tbl_PaytmMaster> tbl_PaytmMaster { get; set; }
        public DbSet<tbl_paytmsettmaster> tbl_paytmsettmaster { get; set; }

        public DbSet<tbl_paytmgstmaster> tbl_paytmgstmaster { get; set; }

        public DbSet<ImportFileMappingEntity> ImportFileMappingEntity { get; set; }
        public DbSet<ImportFileHistoryEntity> ImportFileHistoryEntity { get; set; }




    }
}