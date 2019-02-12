using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SellerVendor.Areas.Seller.Models
{
    public class PaytmOrder
    {
        public string Itemid { get; set; }
        public string OrderId { get; set; }
        public string ProductId { get; set; }
        public string MerchantId { get; set; }
        public string MerchantSku { get; set; }
        public string ProductName { get; set; }
        public string QtyOrdered { get; set; }
        public string FulfillmentId { get; set; }
        public string PromoCode { get; set; }
        public string PromoDescription { get; set; }

        public double Mrp { get; set; }
        public double Discount { get; set; }
        public string SellingPrice { get; set; }
        public string ShippingAmount { get; set; }
        public string ShipByDate { get; set; }
        public string Attributes { get; set; }
        public string OrderDate { get; set; }
        public string ItemStatus { get; set; }
        public string PaymentStatus { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerFirstname { get; set; }
        public string CustomerLastname { get; set; }
        public string State { get; set; }
        public string Pincode { get; set; }
        public string Address { get; set; }

        public string City { get; set; }
        public string Phone { get; set; }

        public string ShipmentCreatedDate { get; set; }
        public string AWB { get; set; }
        public string TrackingUrl { get; set; }
        public string LogisticsPartner { get; set; }
        public string IMEI { get; set; }
        public string ShippedAt { get; set; }
        public string DeliveredAt { get; set; }
        public string ReturnedAt { get; set; }
        public string ShipperId { get; set; }
        public string ManifestId { get; set; }
        public string SLAextended { get; set; }
        public string WarehouseId { get; set; }
        public string MerchantOverdueDate { get; set; }
        public string ParentOrderId { get; set; }
        public string ParentItemId { get; set; }
        public string ChildOrderId { get; set; }
        public string ChildItemId { get; set; }
        public string MaxRefund { get; set; }
        public string MrAt { get; set; }
        public string DeliveryType { get; set; }
        public string CustomerGst { get; set; }
        public string MetaData { get; set; }
        public string FulfillmentReq { get; set; }
    }

    public class tbl_PaytmMaster//used main table and name same as database table
    {
        public int Id { get; set; }
        public string merchant_id { get; set; }
        public string merchant_name { get; set; }
        public string fulfillment_service_name { get; set; }
        public string service_type { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
        public string order_id { get; set; }
        public string order_item_id { get; set; }
        public string product_id { get; set; }
        public string sku_name { get; set; }
        public double price { get; set; }
        public int qty_ordered { get; set; }
        public double total_price { get; set; }
        public string item_status_text { get; set; }
        public double discount { get; set; }
        public double wallet_paid { get; set; }
        public double pg_paid { get; set; }
        public double cod_paid { get; set; }
        public double marketplace_cashback { get; set; }
        public string vertical_name { get; set; }
        public string tracking_number { get; set; }
        public double shipping_amount { get; set; }
        public string shipped_at { get; set; }
        public string delivered_at { get; set; }
        public string returned_at { get; set; }
        public string isCOD { get; set; }
        public string forward_settled_at { get; set; }
        public string reverse_settled_at { get; set; }
        public string merchant_address { get; set; }
        public string merchant_city { get; set; }
        public string merchant_state { get; set; }
        public string merchant_pincode { get; set; }
        public string customer_id { get; set; }
        public string customer_firstname { get; set; }
        public string customer_lastname { get; set; }
        public string customer_address { get; set; }
        public string customer_city { get; set; }
        public string customer_pincode { get; set; }
        public string invoice_id { get; set; }
        public string invoice_date { get; set; }
        public string origin_pin { get; set; }
        public string destination_pin { get; set; }
        public string origin_city { get; set; }
        public string destination_city { get; set; }
        public string failure_reason { get; set; }
        public string payout_status { get; set; }
        public string conv_fee { get; set; }
        public string parent_item_id { get; set; }
        public string parent_order_id { get; set; }
        public string qrid { get; set; }
        public string ecc { get; set; }
        public string wt { get; set; }
        public string dsv { get; set; }
        public string payout_from { get; set; }
        public string payout_status_forward { get; set; }
        public string payout_status_reverse { get; set; }
        public string c_sid { get; set; }
        public string isLMD { get; set; }
        public string payment_bank { get; set; }
        public string payment_method { get; set; }
        public string promo_code { get; set; }
        public int? Uploadid { get; set; }
        public int? Marketplaceid { get; set; }
        public int? Status { get; set; }
        public int? tbl_SellerId { get; set; }
        public double? GSTAmount { get; set; }

        public string Merchant_sku { get; set; }
        public string fulfillment_id { get; set; }
        public string promo_description { get; set; }
        public double? mrp { get; set; }
        public string ship_by_date { get; set; }
        public string attributes { get; set; }

        public string customer_email { get; set; }
        public string customer_gst { get; set; }
        public string state { get; set; }
        public string phone { get; set; }
        public string Shipment_created_date { get; set; }
        public string awb { get; set; }
        public string tracking_url { get; set; }
        public string shipper { get; set; }
        public string IMEI { get; set; }

        public string shipper_id { get; set; }
        public string manifest_id { get; set; }
        public string SLAextended { get; set; }
        public string warehouse_id { get; set; }
        public string merchant_overdue_date { get; set; }

        public string child_order_id { get; set; }
        public string child_item_id { get; set; }
        public string max_refund { get; set; }
        public string mr_at { get; set; }
        public string delivery_type { get; set; }
        public string meta_data { get; set; }
        public string fulfillment_req { get; set; }
        public string delivery_mode { get; set; }

    }


    public class tbl_paytmsettmaster
    {
        public int Id { get; set; }
        public string OrderID { get; set; }
        public string OrderItemID { get; set; }
        public string OrderCreationDate { get; set; }
        public string ReturnDate { get; set; }
        public string ProductID { get; set; }
        public string ProductName { get; set; }
        public string MerchantSKU { get; set; }
        public string OrderItemStatus { get; set; }
        public string SettlementDate { get; set; }
        public string PaymentType { get; set; }
        public string PaymentStatus { get; set; }
        public string AdjustmentReason { get; set; }
        public string TotalPrice { get; set; }
        public string MarketplaceCommission { get; set; }
        public string LogisticsCharges { get; set; }
        public string PGCommission { get; set; }
        public string Penalty { get; set; }
        public string AdjustmentAmount { get; set; }
        public string AdjustmentTaxes { get; set; }
        public string NetAdjustments { get; set; }
        public string ServiceTax { get; set; }
        public string PayableAmount { get; set; }
        public string PayoutWallet { get; set; }
        public string PayoutPG { get; set; }
        public string PayoutCOD { get; set; }
        public string WalletUTR { get; set; }
        public string PGUTR { get; set; }
        public string CODUTR { get; set; }
        public string OperatorReferenceNumber { get; set; }
        public string mp_commission_cgst { get; set; }
        public string mp_commission_igst { get; set; }
        public string mp_commission_sgst { get; set; }
        public string pg_commission_cgst { get; set; }
        public string pg_commission_igst { get; set; }
        public string pg_commission_sgst { get; set; }
        public string logistics_cgst { get; set; }
        public string logistics_igst { get; set; }
        public string logistics_sgst { get; set; }
        public string Customercompanyname { get; set; }
        public string Customerbillingaddress { get; set; }
        public string CustomerGSTIN { get; set; }
        public string igst { get; set; }
        public string cgst { get; set; }
        public string sgst { get; set; }
        public string gst_source_state { get; set; }
        public string gst_destination_state { get; set; }
        public string gst_source_pincode { get; set; }
        public string gst_destination_pincode { get; set; }
        public string InvoiceGenerationDate { get; set; }
        public string InvoiceNumber { get; set; }
        public int? Uploadid { get; set; }
        public int? Marketplaceid { get; set; }
        public int? Status { get; set; }
        public int? tbl_SellerId { get; set; }
    }

    public class tbl_paytmgstmaster
    {
        public int Id { get; set; }
        public string OrderID { get; set; }
        public string OrderItemID { get; set; }
        public string OrderCreationDate { get; set; }
        public string ReturnDate { get; set; }
        public string ProductID { get; set; }
        public string ProductName { get; set; }
        public string MerchantSKU { get; set; }
        public string OrderItemStatus { get; set; }
        public string payment_methods { get; set; }
        public string item_source_state { get; set; }

        public string item_source_pincode { get; set; }
        public string item_destination_state { get; set; }
        public string item_destination_pincode { get; set; }
        public string item_invoice_id { get; set; }
        public string item_invoice_date { get; set; }
        public string TotalPrice { get; set; }
        public string ProductPrice { get; set; }
        public string ShippingChargesTotal { get; set; }
        public string ProductQty { get; set; }
        public string item_taxable_value { get; set; }
        public string item_hsn { get; set; }
        public string item_sgst { get; set; }
        public string item_cgst { get; set; }
        public string item_igst { get; set; }
        public string item_cess { get; set; }
        public string item_sgst_amount { get; set; }
        public string item_cgst_amount { get; set; }
        public string item_igst_amount { get; set; }
        public string item_cess_amount { get; set; }
        public string item_shipping_taxable_value { get; set; }
        public string item_shipping_sgst { get; set; }
        public string item_shipping_cgst { get; set; }
        public string item_shipping_igst { get; set; }
        public string item_shipping_cess { get; set; }
        public string item_shipping_sgst_amount { get; set; }
        public string item_shipping_cgst_amount { get; set; }
        public string item_shipping_igst_amount { get; set; }
        public string item_shipping_cess_amount { get; set; }
        public string item_total_gst { get; set; }
        public string item_credit_note { get; set; }
        public string item_credit_note_date { get; set; }
        public string customer_company_name { get; set; }
        public string customer_billing_address { get; set; }
        public string info_gstin { get; set; }
        public string gst_item_direction { get; set; }

        public int? Uploadid { get; set; }
        public int? Marketplaceid { get; set; }
        public int? Status { get; set; }
        public int? tbl_SellerId { get; set; }

    }

}