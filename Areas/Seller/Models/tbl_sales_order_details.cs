using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SellerVendor.Areas.Seller.Models
{
    public class tbl_sales_order_details
    {
        /// <summary>
        /// used  for save data in db
        /// </summary>
        public int id { get; set; }
        public int is_active { get; set; }
        public int quantity_ordered { get; set; }
        public string product_name { get; set; }
        public string asin { get; set; }
        public string sku_no { get; set; }
        public string order_item_id { get; set; }
        public int quantity_shipped { get; set; }
        public string promotion_ids { get; set; }
        public string shipping_tax_curre_code { get; set; }
        public string promotion_currency_code { get; set; }
        public string shipping_price_curr_code { get; set; }
        public string item_price_curr_code { get; set; }
        public string item_tax_curre_code { get; set; }
        public string shipping_discount_code { get; set; }
        public double  shipping_tax_Amount { get; set; }
        public double promotion_amount { get; set; }
        public double? item_promotionAmount { get; set; }
        public double shipping_price_Amount { get; set; }
        public double item_price_amount { get; set; }
        public double item_tax_amount { get; set; }
        public double shipping_discount_amt { get; set; }
        public int tbl_sales_order_id { get; set; }
        public int tbl_seller_id { get; set; }
        public int status_updated_by { get; set; }
        public DateTime? status_updated_on { get; set; }
        public int? n_order_status_id { get; set; }
        public string amazon_order_id { get; set; }
        public string is_gift { get; set; }
        public int? n_order_invoice_status { get; set; }
        public int? orderItemId { get; set; }
        public string hsn { get; set; }
        public DateTime? dispatchAfter_date { get; set; }
        public DateTime? dispatch_bydate { get; set; }
        public int? sla_flipkart { get; set; }
        public string listing_id { get; set; }
        public string shipping_pincode { get; set; }
        public double? selling_price { get; set; }
        public string shipment_Id { get; set; }
        public double? i_sgst { get; set; }
        public double? i_cgst { get; set; }
        public double? i_igst { get; set; }
        public int? tbl_inventory_id { get; set; }
        public double? giftwrapprice_amount { get; set; }
        public string giftwrapprice_code { get; set; }
        public string giftwraptaxcode { get; set; }
        public double? giftwraptax_amount { get; set; }
        public double? shipping_discount_tax_amount { get; set; }
        public int? tax_flag { get; set; }
        public string Ship_from_city { get; set; }
        public string ship_from_state { get; set; }
        public string ship_from_postalcode { get; set; }
        public string ship_to_city { get; set; }
        public string ship_to_state { get; set; }
        public string ship_to_postalcode { get; set; }
        public string tax_invoiceno { get; set; }

        public short? is_tax_calculated { get; set; }
        public short? tax_updatedby_taxfile { get; set; }
        public string product_id { get; set; }
        public string fulfillment_id { get; set; }
        public string promo_code { get; set; }
        public string promo_description { get; set; }
        public string AWB { get; set; }
        public string tracking_url { get; set; }
        public string Logistics_partner { get; set; }
        public string warehouse_id { get; set; }
        public string manifest_id { get; set; }
        public int? shipper_id { get; set; }

    }
}