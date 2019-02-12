using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SellerVendor.Areas.Seller.Models
{
    public class tbl_settlement_order
    {     
        public int Id { get; set; }
        public string Order_Id { get; set; }
        public string unique_order_id { get; set; }
        public string settlement_id { get; set; }
        public double? principal_price { get; set; }
        public double? product_tax { get; set; }
        public double? shipping_price { get; set; }
        public double? shipping_tax { get; set; }
        public double? giftwrap_price { get; set; }
        public double? giftwarp_tax { get; set; }
        public double? shipping_discount { get; set; }
        public double? shipping_tax_discount { get; set; }
        public double? SAFE_T_Reimbursement { get; set; }
        public double? Protection_fund_flipkart { get; set; }//vineet
        public double? INCORRECT_FEES_ITEMS { get; set; }
        public DateTime? posted_date { get; set; }
        public string Sku_no { get; set; }
        public int? quantity { get; set; }
        public int? tbl_seller_id { get; set; }
        public DateTime? created_on { get; set; }
        public DateTime? LastUpdatedDateUTC { get; set; }
    }
}