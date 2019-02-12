using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SellerVendor.Areas.Seller.Models
{
    public class tbl_purchase
    {
        public int id { get; set; }

        public Nullable<int> tbl_sellers_id { get; set; }
        public Nullable<int> tbl_seller_vendors_id { get; set; }
        public Nullable<int> tbl_seller_warehouses_id { get; set; }

        public Nullable<double> invoice_amount { get; set; }
        public string invoice_no { get; set; }
        public string invoice_photo_path { get; set; }
        public Nullable<DateTime> date_invoice { get; set; }
        public Nullable<DateTime> po_date { get; set; }
        public string po_number { get; set; }
        public Nullable<double> tax_amount { get; set; }
        public string remarks_po { get; set; }

        public Nullable<int> created_by { get; set; }
        public Nullable<DateTime> date_created { get; set; }

        public Nullable<int> n_postatus_type { get; set; }
        public Nullable<int> isactive { get; set; }
        public Nullable<int> t_Po_status { get; set; }
        
    }
    public partial class model_tbl_purchase
    {
        public int id { get; set; }

        public Nullable<int> tbl_sellers_id { get; set; }
        public Nullable<int> tbl_seller_vendors_id { get; set; }
        public Nullable<int> tbl_seller_warehouses_id { get; set; }

        public Nullable<double> invoice_amount { get; set; }
        public string invoice_no { get; set; }
        public string invoice_photo_path { get; set; }
        public Nullable<DateTime> date_invoice { get; set; }
        public Nullable<DateTime> po_date { get; set; }
        public string po_number { get; set; }
        public Nullable<double> tax_amount { get; set; }
        public string remarks_po { get; set; }
        public Nullable<int> n_postatus_type { get; set; }
        public Nullable<int> t_Po_status { get; set; }
        public Nullable<int> created_by { get; set; }
        public Nullable<DateTime> date_created { get; set; }

        public Nullable<int> isactive { get; set; }
        public string XmlPurchaseDetails { get; set; }
        public SelectList ddlVendorList { get; set; }
        public SelectList ddlWarehouseList { get; set; }
        public SelectList ddlInventoryList { get; set; }
        public List<tbl_purchase_details> ddlPurchaseDetailsList { get; set; }
        public List<tbl_tax> ddlTaxDetailsList { get; set; }
        public List<PurchaseDetailsviewmodel> ddlPurchaseDetailsviewmodel { get; set; }
        public string t_JsonData { get; set; }
        public string t_Mode { get; set; }
        public int tbl_inventory_id { get; set; }
        public string VendorName { get; set; }
        public string WarehosueName { get; set; }
        public int? item_Quantity { get; set; }
        public string item_serialNo { get; set; }
        public string batch_no { get; set; }
        public List<model_item_details> ddlmodelitemdetails { get; set; }
        public int? SaveItemId { get; set; }
        public string XmlItemquantity { get; set; }
        
    }

    public partial class model_item_details
    {
        public int? item_Quantity { get; set; }
        public string batch_no { get; set; }
    }
    public partial class model_settlement_Bank_Transfer
    {
        public int? id { get; set; }
        public decimal? amount { get; set; }
        public string verifystatus { get; set; }
        public string remarks { get; set; }
        public int? market_place_id { get; set; }
        public DateTime? deposit_date { get; set; }
        public string MarketPlaceName { get; set; }
    }
}