using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SellerVendor.Areas.Seller.Models
{
    public class tbl_sales_order
    {    
        /// <summary>
        /// this is for new Xml Record
        /// </summary>
        /// 
        public int id { get; set; }
        public int is_active { get; set; }
        public DateTime Latest_ShipDate { get; set; }
        public string order_type { get; set; }
        public DateTime purchase_date { get; set; }
        public string amazon_order_id { get; set; }
        public string buyer_emil { get; set; }
        public string is_Replacement_order { get; set; }
        public DateTime? last_updated_date { get; set; }
        public string m_ship_service_level_id { get; set; }
        public int no_of_itemshipped { get; set; }
        public string order_status { get; set; }
        public string sales_channel { get; set; }
        public string is_business_order { get; set; }
        public int no_of_item_unshippes { get; set; }
        public string payment_method_detail { get; set; }
        public string buyer_name { get; set; }
        public string currency_code { get; set; }
        public double bill_amount { get; set; }
        public string is_premium_order { get; set; }
        public DateTime earliest_ship_date { get; set; }
        public string marketplace_id { get; set; }
        public string fullfillment_channel { get; set; }
        public string payment_method { get; set; }
        public string is_prime { get; set; }
        public string ship_service_category { get; set; }
        public string seller_order_id { get; set; }
        public DateTime created_on { get; set; }
        public int tbl_Marketplace_Id { get; set; }
        public int tbl_sellers_id { get; set; }
        public int? order_invoice_status { get; set; }
        public int? n_fullfilled_id { get; set; }
        public int? tbl_Customer_Id { get; set; }
        public string order_item_id { get; set; }
        public DateTime? dispatch_afterdate { get; set; }
        public string t_Hold { get; set; }
        public int? n_item_orderstatus { get; set; }
        public int? flag_id { get; set; }
        public int? tbl_order_upload_id { get; set; }
        public string ship_city { get; set; }
        public string ship_state { get; set; }
        public string ship_postal_code { get; set; }
        public string ship_country { get; set; }
        public DateTime? LastUpdatedDateUTC { get; set; }
        public string merchant_id { get; set; }
        public DateTime? returned_at { get; set; }
        /// <summary>
        /// not use in save data in db
        /// </summary>
        //public string customer_name { get; set; }
        //public string fulfilled_by { get; set; }      
        //public string merchant_order_id { get; set; }
        //public int m_mrketplace_id { get; set; }
        //public int m_saleorder_entry_src_id { get; set; }           
        //public string ship_address { get; set; }
        //public string ship_city { get; set; }
        //public string ship_country { get; set; }
        //public string ship_postal_code { get; set; }
        //public string ship_state { get; set; }

    }

    public partial class viewSalesOrder
    {
        public int id { get; set; }
        public int marketplaceid { get; set; }
        public string ImagePath { get; set; }
        public string amazon_order_id { get; set; }
        public DateTime purchase_date { get; set; }
        public string BuyerEmail { get; set; }
        public string BuyerName { get; set; }
        public string PaymentMethod { get; set; }
        public double BillAmount { get; set; }
        public string OrderType { get; set; }
        public string FullfillmentChannel { get; set; }
        public string PaymentMethodDetail { get; set; }
        public string SalesOrderStatus { get; set; }
        public int Statusid { get; set; }
        public int order_invoice_status { get; set; }
        public string FullfilledName { get; set; }
        public string ContactNO { get; set; }
        public DateTime DispatchAfterDate { get; set; }
        public DateTime DispatchByDate { get; set; }
        public string Hold { get; set; }
        public int itemcount { get; set; }
        public int n_fullfilled_id { get; set; }
        public string OrderStatus { get; set; }
    }

    public partial class OrderInvoiceprint
    {
        public List<PrintSaleOrder> PrintSaleOrder { get; set; }
    }
    public partial class PrintSaleOrder
    {
        public int id { get; set; }
        public string address { get; set; }
        public string business_name { get; set; }
        public string city { get; set; }
        public string gstin { get; set; }
        public string pan { get; set; }
        public string pincode { get; set; }
        public string statename { get; set; }
        public string countryname { get; set; }

        public string Address_1 { get; set; }
        public string Address_2 { get; set; }
        public string shipping_Buyer_Name { get; set; }
        public string City { get; set; }
        public string Country_Code { get; set; }
        public string Postal_Code { get; set; }
        public string State_Region { get; set; }

        public string purchase_date { get; set; }
        public string amazon_order_id { get; set; }
        public string AmountWords { get; set; }
        public double Unitprice { get; set; }
        public double taxprice { get; set; }
        public double price { get; set; }

        public List<tbl_sales_order_details> ddlsaleorderdetailList { get; set; }



        
    }

    public partial class Tab
    {
        public string amazon_order_id { get; set; }
        public string orderDate { get; set; }
        public string shipdate { get; set; }
        public string Time { get; set; }
        public string orderstatus { get; set; }
        public string ordertype { get; set; }
        public string Paymentmethoddetails { get; set; }
        public string billamount { get; set; }
        public string buyeremail { get; set; }
        public string FullfilledBy { get; set; }
        public int? Order_status { get; set; }
        public int? Orderid { get; set; }
        
    }

    public partial class OrderTax
    {
        public string  OrderSettlementID { get; set; }
        public double OrderPrice { get; set; }
        public DateTime SettlementDate { get; set; }
        public string  ReferenceNo { get; set; }
        public double OrderIGST { get; set; }
        public double OrderCGST { get; set; }
        public double OrderSGST { get; set; }
        public string SettlementType { get; set; }
        public int settlement_id { get; set; }
    }
    public partial class Ordertext2
    {
        public string ReferenceID { get; set; }
        public string OrderID { get; set; }
        public double Principal { get; set; }
        public double Product_Tax { get; set; }
        public double Shipping { get; set; }
        public double Shipping_Tax { get; set; }
        public double GiftwarpPrice { get; set; }
        public double GiftwarpTax { get; set; }
        public double SumOrder { get; set; }

        public double FBAFEE { get; set; }
        public double TechnologyFee { get; set; }
        public double CommissionFee { get; set; }
        public double FixedClosingFee { get; set; }
        public double ShippingChargebackFee { get; set; }
        public double ShippingDiscountFee { get; set; }

        public double RefundCommision { get; set; }
        public double SumFee { get; set; }

        public double FBACGST { get; set; }
        public double FBASGST { get; set; }
        public double TechnologyIGST { get; set; }
        public double TechnologyCGST { get; set; }
        public double TechnologySGST { get; set; }
        public double CommissionIGST { get; set; }
        public double CommissionCGST { get; set; }
        public double CommissionSGST { get; set; }
        public double FixedclosingIGST { get; set; }
        public double FixedclosingCGST { get; set; }
        public double FixedclosingSGST { get; set; }
        public double shippingchargeCGST { get; set; }
        public double shippingchargeSGST { get; set; }
        public double Shippingtaxdiscount { get; set; }
        public double RefundCommisionIGST { get; set; }
        public double SumTaxFee { get; set; }
        public double NetTotal { get; set; }

        
        
    }
}