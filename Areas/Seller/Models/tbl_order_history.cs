using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SellerVendor.Areas.Seller.Models
{
    public class tbl_order_history
    {
        public int Id { get; set; }
        public int? tbl_seller_id { get; set; }
        public int? tbl_orders_id { get; set; }     
        public int? t_order_status { get; set; }      
        public int? updated_by { get; set; }
        public string t_remarks { get; set; }        
        public DateTime? created_on { get; set; }
        public int? tbl_orderDetails_Id { get; set; }
        public int? tbl_marketplace_id { get; set; }
        public DateTime? ShipmentDate { get; set; }
        public string SKU { get; set; }
        public string ASIN { get; set; }
        public int? Quantity { get; set; }
        public string ReplacementReasonCode { get; set; }
        public string OrderID { get; set; }
        public string unique_order_id { get; set; }
        public string OrigialOrderID { get; set; }
        public DateTime? approval_date { get; set; }
        public string reimbursement_id { get; set; }
        public string case_id { get; set; }
        public double? amount_per_unit { get; set; }
        public short? settlement_type_id { get; set; }
        public string settlement_id { get; set; }
        public double? Giftwrap_price { get; set; }
        public double? shipping_price { get; set; }
        public double? shipping_discount { get; set; }
        public double? shipping_tax_discount { get; set; }
        public double? product_tax { get; set; }
        public double? gift_wrap_tax { get; set; }
        public double? shipping_tax { get; set; }
        public double? SAFE_T_Reimbursement { get; set; }
        public double? Protection_fund_flipkart { get; set; }//vineet
        public string fulfillment_id { get; set; }
        public int? physically_type { get; set; }
        public int? condition_type { get; set; }
        public DateTime? physically_updated_date { get; set; }
        public DateTime? physically_selected_date { get; set; }
        public DateTime? return_date { get; set; }
        public int? tbl_return_upload_id { get; set; }
        public DateTime? LastUpdatedDateUTC { get; set; }
        public string ItemCreditNote { get; set; }
        


    }
}