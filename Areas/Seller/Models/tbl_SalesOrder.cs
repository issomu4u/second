using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SellerVendor.Areas.Seller.Models
{
    public class tbl_SalesOrder
    {
        public string AmazonOrderID { get; set; }
        public string MerchantOrderID { get; set; }
        public string PurchaseDate { get; set; }
        public string LastUpdatedDate { get; set; }
        public string OrderStatus { get; set; }
        public string SalesChannel { get; set; }
        public string IsBusinessOrder { get; set; }

        public string FulfillmentChannel { get; set; }
        public string ShipServiceLevel { get; set; }
       
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        /// <summary>
        /// New XMl Data
        /// </summary>
        /// 
        //////////For Customer Details///////////////
        public string Address_1 { get; set; }
        public string Address_2 { get; set; }
        
        public string CountryCode { get; set; }
        //public string PostalCode { get; set; }
        public string ShippingName { get; set; }
        public string StateRegion { get; set; }      
        public string City { get; set; }

        /// <summary>
        /// for Sales Order Master
        /// </summary>

       
        public string BuyerEmail { get; set; }       
        public string FulfilledBy { get; set; }                 
        public string LatestShipDate { get; set; }     
        public string OrderType { get; set; }   
        public decimal? BillAmount { get; set; }
        public string BuyerName { get; set; }
        public string CurrencyCode { get; set; }
        public string EarliestShipDate { get; set; }
     
        public string IsPremiumOrder { get; set; }
        public string IsPrime { get; set; }
        public string IsReplacementOrder { get; set; }
        public string Marketplaceid { get; set; }
        public int? NoOfItemshipped { get; set; }
        public int? NoOfItemUnshippes { get; set; }
        public string PaymentMethod { get; set; }
        public string PaymentMethodDetail { get; set; }
        public string SellerOrderId { get; set; }
        public string ShipServiceLevelCategory { get; set; }

        public string ShippedAmazonTFM { get; set; }
        public string LatestDeliveryDate { get; set; }
        public string EarliestDeliveryDate { get; set; }
        public string TFMShipmentStatus { get; set; }
        public string ExceutionPaymentMethod { get; set; }
        public string ExceutionCurrencyCode { get; set; }
        public decimal? ExceutionBillAmount { get; set; }
        public string ContactNo { get; set; }
        public string Addresstype { get; set; }

        public List<AmazonOrderDetails> OrderDetails { get; set; }

        public List<OrderItem> Orders { get; set; }
    }

    public partial class OrderItem
    {
        public string ASIN { get; set; }
        public string SKU { get; set; }
        public string ItemStatus { get; set; }
        public string ProductName { get; set; }
        public string Quantity { get; set; }
        public string PromotionIDs { get; set; }
        public string ShipPromotionDiscount { get; set; }
        public string ItemPromotionDiscount { get; set; }

        public List<ItemPrice> itemprice { get; set; }
    }

    public partial class ItemPrice
    {
        public string Type { get; set; }
        public decimal? pAmonu { get; set; }
        public decimal? shipAmonu { get; set; }
        public decimal? giftwrapAmonu { get; set; }
        public decimal? ptaxamount { get; set; }
        public decimal? pshippingtaxamount { get; set; }
    }

    public partial class AmazonOrderDetails
    {
        public string asin { get; set; }          
        public decimal item_price { get; set; }
        public string item_Price_name { get; set; }
        public string item_shipping { get; set; }
        public string item_status { get; set; }
        public decimal item_tax { get; set; }
        public int n_order_status_id { get; set; }
        public string order_item_id { get; set; }
        public string product_name { get; set; }
        public string promotion_ids { get; set; }
        public int quantity { get; set; }
        public int quantity_ordered { get; set; }
        public int quantity_shipped { get; set; }
        public string shipping_currency_code { get; set; }
        public decimal shipping_price { get; set; }
        public double shipping_tax { get; set; }
        public decimal ship_promotion_discount { get; set; }
        public string sku_no { get; set; }
        public int status_updated_by { get; set; }
        public DateTime status_updated_on { get; set; }
        public int tbl_inventory_id { get; set; }
        public int tbl_sales_order_id { get; set; }      
        public string amazon_order_id { get; set; }
        public decimal item_price_amount { get; set; }
        public string item_price_curr_code { get; set; }
        public decimal item_tax_amount { get; set; }
        public string item_tax_curre_code { get; set; }
        public decimal promotion_amount { get; set; }
        public string promotion_currency_code { get; set; }
        public decimal shipping_discount_amt { get; set; }
        public string shipping_discount_code { get; set; }
        public decimal shipping_price_Amount { get; set; }
        public string shipping_price_curr_code { get; set; }
        public decimal shipping_tax_Amount { get; set; }
        public string shipping_tax_curre_code { get; set; }
        public string is_gift { get; set; }
        public string giftwraptaxcode { get; set; }
        public decimal giftwraptaxamount { get; set; }
        public string giftwrappricecode { get; set; }
        public decimal giftwrappriceamount { get; set; }

        public decimal producttaxprice { get; set; }
        public decimal productprice { get; set; }
        public decimal shippingtaxprice { get; set; }
        public decimal shipping { get; set; }
        public decimal gifttax { get; set; }
        public decimal giftprice { get; set; }

    }

    public partial class ReplacementOrderDetails
    {
        public string ShipmentDate { get; set; }
        public string SKU { get; set; }
        public string ASIN { get; set; }
        public string FullfillmentcenterId { get; set; }
        public string OriginalFullfillmentcenterID { get; set; }
        public string Quantity { get; set; }
        public string ReplacementReasonCode { get; set; }
        public string ReplacementAmazonOrderID { get; set; }
        public string OrigialAmazonOrderID { get; set; }
        
    }

    public partial class AmazonReplacementOrder
    {
        public List<ReplacementOrderDetails> ReplacementOrderDetails { get; set; }
    }

    public partial class AmazonreconciliationOrder
    {
        public List<reconciliationorder> reconciliationorder { get; set; }
    }
    public partial class reconciliationorder
    {
        public string settlement_id { get; set; }
        public string settlement_start_date { get; set; }
        public string settlement_end_date { get; set; }
        public string deposit_date { get; set; }
        public string total_amount { get; set; }
        public string currency { get; set; }
        public string transaction_type { get; set; }
        public string order_id { get; set; }
        public string order_itemId { get; set; } //for flipkart
        public string merchant_order_id { get; set; }
        public string adjustment_id { get; set; }
        public string shipment_id { get; set; }
        public string marketplace_name { get; set; }
        public string amount_type { get; set; }
        public string amount_description { get; set; }
        public string amount { get; set; }
        public string settlement_amount { get; set; }//for flipkart
        public string fulfillment_id { get; set; }
        public string posted_date { get; set; }
        public string posted_date_time { get; set; }
        public string order_item_code { get; set; }
        public string merchant_order_item_id { get; set; }
        public string merchant_adjustment_item_id { get; set; }
        public string sku { get; set; }
        public string taxes { get; set; }
        public string marketplace_fee { get; set; }
        public string quantity_purchased { get; set; }
        public string promotion_id { get; set; }
        public string promotion_amount { get; set; }//for settlement Api
        public string NeftId { get; set; }// for flipkart
        public string Protection_fund { get; set; }// for flipkart
        public string Refund_Amount { get; set; }// for flipkart
        public string Total_Offer_Amount { get; set; }// for flipkart
        public string MY_Share_Amount { get; set; }// for flipkart
        public string Customershipping_Amount { get; set; }// for flipkart


        public Dictionary<string, List<settlement_amt_type>> order_amount_typesDict { get; set; } //item sku type is the key
        public Dictionary<string, List<settlement_amt_type>> refund_amount_typesDict { get; set; } //item sku type is the key
        public Dictionary<string, List<settlement_amt_type>> easyship_amount_typesDict { get; set; } //item sku type is the key

        public List<settlement_amt_type> otherTransatanctionList;
        public List<settlement_amt_type> suspenseAccountList;
    }

    public partial class settlement_amt_type
    {
        public string description { get; set; }
        public string type { get; set; }
        public double amount { get; set; }
        public string posteddatetime { get; set; }

        //public string MyProperty { get; set; }
        public int qty;
    }

    public partial class expense_tax_class
    {
        public string expense_name;
        public int expense_db_id;
        public double sgst;
        public double cgst;
        public double igst;
        public int refrence_type;

    }
    public partial class ReimburesementDetail
    {
        public List<Reimbursement> ReimbursementOrderDetails { get; set; }
    }
    public partial class Reimbursement
    {
        public string Approval_date { get; set; }
        public string Reimbursement_id { get; set; }
        public string Case_id { get; set; }
        public string Amazon_order_id { get; set; }
        public string Reason { get; set; }
        public string Sku { get; set; }
        public string FnSku { get; set; }
        public string Asin { get; set; }
        public string Produc_Name { get; set; }
        public string Condition { get; set; }
        public string Currency_Unit { get; set; }
        public string Amazon_per_unit { get; set; }
        public string Amount_Total { get; set; }
        public string Quantity_Cash { get; set; }
    }

    public partial class ReturensDataDetail
    {
        public List<ReturnsData> ReturnsDataDetails { get; set; }
    }
    public partial class ReturnsData
    {
        public string ReturnDate { get; set; }
        public string Order_id { get; set; }
        public string sku { get; set; }
        public string Asin { get; set; }
        public string Fnsku { get; set; }
        public string Product_name { get; set; }
        public string quantity { get; set; }
        public string fullfillment_centerid { get; set; }
        public string DetailedDepostion { get; set; }
        public string Reason { get; set; }
        public string Licence_paltenumber { get; set; }
        public string Customer_comments { get; set; }
    }

    public class AmazonListItem_Seller
    {
        public string orderid { get; set; } // title,
        public int quantity_ordered { get; set; } // orderquantity,
        public string product_name { get; set; } // title,
        public string asin { get; set; } // asin,
        public string sku_no { get; set; } // sellersku,
        public string order_item_id { get; set; } // orderitemid,
        public int quantity_shipped { get; set; } // quantityshippped,
        public string shipping_tax_curre_code { get; set; } // shiptaxcurrenceycode,
        public double shipping_tax_Amount { get; set; } // shiptaxamount,
        public string promotion_currency_code { get; set; } // promotioncurrecode,
        public double promotion_amount { get; set; } // promotionamount,
        public string promotion_ids { get; set; } // promotionid,
        public double shipping_discount_amt { get; set; } // shippingdisamount,
        public string shipping_discount_code { get; set; } // shippingdiscode,
        public double shipping_price_Amount { get; set; } // shippingpriceamount,
        public string shipping_price_curr_code { get; set; } // shippingpricecode,
        public double item_tax_amount { get; set; } // itemtaxamount,
        public string item_tax_curre_code { get; set; } // itemtaxcode,
        public string item_price_curr_code { get; set; } // itempricecode,
        public double item_price_amount { get; set; } // itempriceamount,
        public string is_gift { get; set; } // isgift,
        public double giftwrappriceamount { get; set; } // giftwrappriceamount,
        public string giftwrappricecode { get; set; } // giftwrappricecode,
        public double giftwraptaxamount { get; set; } // giftwraptaxamount,
        public string giftwraptaxcode { get; set; } // giftwraptaxcode,
        public double producttaxprice { get; set; } // producttaxprice,
        public double productprice { get; set; } // productprice,
        public double shippingtaxprice { get; set; } // shippingtaxprice,
        public double shipping { get; set; } // shipping,
        public double gifttax { get; set; } // gifttax,
        public double giftprice { get; set; } // giftprice,

        public string ConditionSubtypeId { get; set; } // ConditionSubtypeId,
        public string ConditionId { get; set; } // ConditionId,
        public int NumberOfItems { get; set; } // NumberOfItems,
    }

    public partial class ProductListDetails
    {
        public List<ProductList> Productlistdetails { get; set; }
    }
    public partial class ProductList
    {
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public string SKU_NO { get; set; }
        public string ASIN_NO { get; set; }
        public string ProductPrice { get; set; }
        public string ProductID { get; set; }
    }
}