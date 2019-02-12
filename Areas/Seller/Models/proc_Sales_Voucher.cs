using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SellerVendor.Areas.Seller.Models
{
    public class proc_Sales_Voucher
    {
        public int? OrderID { get; set; }
        public string amazon_order_id { get; set; }
        public DateTime? purchase_date { get; set; }
        public string buyer_name { get; set; }
        public string order_status { get; set; }
        public string shipping_Buyer_Name { get; set; }
        public string State_Region { get; set; }
        public string City { get; set; }
        public string Country_Code { get; set; }
        public string Postal_Code { get; set; }
        public string Address_1 { get; set; }
        public string Address_2 { get; set; }
        public string product_name { get; set; }
        public string sku_no { get; set; }
        public int? orderdetailsID { get; set; }
        public int? quantity_ordered { get; set; }
        public double? item_price_amount { get; set; }
        public double? shipping_price_Amount { get; set; }
        public double? giftwrapprice_amount { get; set; }
        public double? item_tax_amount { get; set; }
        public double? shipping_tax_Amount { get; set; }
        public double? giftwraptax_amount { get; set; }
        //public double? cgst_tax { get; set; }
        //public double? sgst_tax { get; set; }
        //public double? igst_tax { get; set; }
        //public double? CGST_amount { get; set; }
        //public double? Igst_amount { get; set; }
        //public double? sgst_amount { get; set; }
        //public int? TaxID { get; set; }
    }
}