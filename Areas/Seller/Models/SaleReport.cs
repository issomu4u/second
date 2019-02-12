using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SellerVendor.Areas.Seller.Models
{
    public class MainTallySettlementVoucher
    {
        public string settlement_ref;
        public string voucher_date;
        public Dictionary<string, TallyOrder> tallyOrderDict; //key is uniqueOrderId
        public TallyDebitCredit debitType_bank_balance;
        public TallyDebitCredit creditType_Previous_Reserve_Amount_Balance;
        public TallyDebitCredit debitType_Current_Reserve_Amount;
        public TallyDebitCredit creditType_Nonsubscription_feeadj;
        public TallyDebitCredit creditType_incorrectItemFees;
        public TallyDebitCredit debitType_INCORRECT_FEES_ITEMS;
        public TallyDebitCredit debitType_StorageFee;
        public TallyDebitCredit debittype_StorageFeeCGST;
        public TallyDebitCredit debittype_StorageFeeSGST;
        public TallyDebitCredit creditType_BalanceAdjustment;//new
        public TallyDebitCredit debitType_FBAInboundTransportationFee;//new
        public TallyDebitCredit debitType_Payable_to_Amazon;//new
        public TallyDebitCredit suspenseEntry;//new
    }

    public class TallyOrder
    {
        public string orig_Order_id;
        public Dictionary<string, TallyDebitCredit> tallyExpenseDebitCreditDict; //key is the expense name
        public TallyDebitCredit igst;
        public TallyDebitCredit cgst;
        public TallyDebitCredit sgst;
        //public Dictionary<int, TallyDebitCredit> tallyTaxDebitCreditDict;  //key is the tax name
        public TallyDebitCredit voucherEntry;
    }

    public class TallyDebitCredit
    {
        public string ledger_name;
        public double debit_amt;
        public double credit_amt;
        public string voucher_number;
        public string voucher_date;
        public string reference_num;
        public string narration;
    }

    public class SaleReport
    {

        public string OrderID { get; set; }
        public string InvoiceNo { get; set; }
        public string Channelentry { get; set; }
        public string Channelledger { get; set; }
        public int Quantity { get; set; }
        public string CustomerName { get; set; }

        public string shipaddressname { get; set; }
        public string shipaddressname1 { get; set; }
        public string shipaddressname2 { get; set; }
        public string shipcity { get; set; }
        public string shipstate { get; set; }
        public string shipcountry { get; set; }
        public string shippincode { get; set; }
        public string shipphoneno { get; set; }
        public string shipprovider { get; set; }
        public string AWBNo { get; set; }
        public string UTGST { get; set; }
        public string UTGSTRate { get; set; }
        public string CESS { get; set; }
        public string CESSRate { get; set; }
        public string Servicetax { get; set; }
        public string StLedger { get; set; }
        public string Godown { get; set; }
        public string Dispatch_Cancellationdate { get; set; }
        public string Entity { get; set; }
        public string TinNo {get; set;}
        public string Channelinvoicecreated { get; set; }
        public string TaxVerification { get; set; }
        public double itemamountwithout_tax { get; set; }
        public string SalesLedger { get; set; }
        public double GiftwrapAmount { get; set; }
        public string IMEI { get; set; }


        public string Currency { get; set; }
        public double Principal { get; set; }
        public double Product_Tax { get; set; }
        public double Shipping { get; set; }
        public double Shipping_Tax { get; set; }
        public double SumOrder { get; set; }
        public double ShippingDiscount { get; set; }
        public double ShippingDiscount_tax { get; set; }
        public double PromotionAmt { get; set; }

        public double FBAFEE { get; set; }
        public double TechnologyFee { get; set; }
        public double CommissionFee { get; set; }
        public double FixedClosingFee { get; set; }
        public double ShippingChargebackFee { get; set; }
        public double ShippingDiscountFee { get; set; }
        public double RefundCommision { get; set; }
        public double ShippingCommision { get; set; }
        public double SumFee { get; set; }
        public double EasyShipweighthandlingfees { get; set; }

        public double FBAPickPackFee { get; set; }
        public double GiftWrapChargeback { get; set; }
        public double AmazonEasyShipCharges { get; set; }

        public double FBACGST { get; set; }
        public double FBASGST { get; set; }
        public double FBAIGST { get; set; }
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
        public double shippingchargeIGST { get; set; }

        public double Shippingtaxdiscount { get; set; }
        public double ShippingCommissionIGST { get; set; }
        public double EasyShipweighthandlingfeesIGST { get; set; }

        public double FBAPickPackFeeCGST { get; set; }
        public double FBAPickPackFeeSGST { get; set; }
        public double GiftWrapChargebackCGST { get; set; }
        public double GiftWrapChargebackSGST { get; set; }
        public double AmazonEasyShipChargesIGST { get; set; }

        public double RefundDiscount { get; set; }
        public double SumTaxFee { get; set; }
        public double NetTotal { get; set; }
        public double ExpenseTotal { get; set; }
        public double FullExpenseTotal { get; set; }

        public double SUMIGST { get; set; }
        public double SUMSGST { get; set; }
        public double SUMCGST { get; set; }
        public string IGST_rate { get; set; }
        public string SGST_rate { get; set; }
        public string CGST_rate { get; set; }
        public string Shipping_rate { get; set; }
        public string Giftwrap_rate { get; set; }

        public string SettlementDate { get; set; }

        public string FromDate { get; set; }
        public string ToDate { get; set; }

        public double OrderAmount { get; set; }
        public String OrderDate { get; set; }
        public string DispatchDate { get; set; }
        public double CancelledorderAmount { get; set; }
        public String CancelledOrderDate { get; set; }
        public string skuNo { get; set; }
        public string ProductName { get; set; }
        public double ProductValue { get; set; }
        public double productIgstamt { get; set; }
        public double productSgstamt { get; set; }
        public double productCgstamt { get; set; }
        public string SettlementType { get; set; }
        public double OrderAddTotal { get; set; }
        public double OrderReturnTotal { get; set; }
        public string MarketPlaceName { get; set; }
        public double GiftAmount { get; set; }
        public double GiftTax { get; set; }
        public string Status { get; set; }

        public Double CancelProductTax { get; set; }
        public Double CancelShippingTax { get; set; }
        public Double CancelWriftrapTax { get; set; }
        public Double CancelShipping { get; set; }
        public Double CancelGiftWarp { get; set; }
        public double SumCancelFee { get; set; }


        public double orderprincipal { get; set; }
        public double orderproduct_tax { get; set; }
        public double ordershipping { get; set; }
        public double ordershipping_tax { get; set; }
        public double ordergiftwrap { get; set; }
        public double ordergiftwrap_tax { get; set; }
        public double orderTotal { get; set; }
        public string ReferenceID { get; set; }
        public string Sett_orderDate { get; set; }
        public double ordershipping_discount { get; set; }
        public double ordershipping_discounttax { get; set; }
        public double orderigst { get; set; }
        public double ordersgst { get; set; }
        public double ordercgst { get; set; }
        public double ordertotal { get; set; }


        public double refundprincipal { get; set; }
        public double refundproduct_tax { get; set; }
        public double refundshipping { get; set; }
        public double refundshipping_tax { get; set; }
        public double refundgiftwrap { get; set; }
        public double refundgiftwrap_tax { get; set; }

        public double refundshipping_discount { get; set; }
        public double refundshipping_discount_tax { get; set; }
        public double refundTotal { get; set; }
        public string refundReferenceID { get; set; }
        public string refundDate { get; set; }
        public double refundtotal { get; set; }

        public double RefundFBAFEE { get; set; }
        public double RefundTechnologyFee { get; set; }
        public double RefundCommissionFee { get; set; }
        public double RefundFixedClosingFee { get; set; }
        public double RefundShippingChargebackFee { get; set; }
        public double RefundShippingDiscountFee { get; set; }
        public double Refund_Commision { get; set; }
        public double Refund_Expenses { get; set; }

        public double Refund_ShippingCommision { get; set; }
        public double Refund_EasyShipWeight { get; set; }


        public double RefundFBACGST { get; set; }
        public double RefundFBASGST { get; set; }
        public double RefundFBAIGST { get; set; }

        public double RefundTechnologyIGST { get; set; }
        public double RefundTechnologyCGST { get; set; }
        public double RefundTechnologySGST { get; set; }

        public double RefundCommissionIGST { get; set; }
        public double RefundCommissionCGST { get; set; }
        public double RefundCommissionSGST { get; set; }

        public double RefundFixedclosingIGST { get; set; }
        public double RefundFixedclosingCGST { get; set; }
        public double RefundFixedclosingSGST { get; set; }

        public double Refund_EasyShipweighthandlingfeesIGST { get; set; }
        public double RefundshippingchargeCGST { get; set; }
        public double RefundshippingchargeSGST { get; set; }
        public double RefundshippingchargeIGST { get; set; }

        public double RefundFBAPick_PackFeeCGST { get; set; }
        public double RefundFBAPick_PackFeeSGST { get; set; }
        public double RefundGiftWrapChargebackCGST { get; set; }
        public double RefundGiftWrapChargebackSGST { get; set; }
        public double Refund_AmazonEasyShipChargesIGST { get; set; }
        

        public double RefundShippingtaxdiscount { get; set; }
        public double Refund_Discount { get; set; }
        public double Refund_DiscountCGST { get; set; }
        public double Refund_DiscountSGST { get; set; }
        public double Refund_Shipping_Commission { get; set; }
        public double Refund_EasyShipweighthandlingfees { get; set; }
        public double Refund_Easy_Ship { get; set; }
        public double Refund_FBAPick_PackFee { get; set; }
        public double Refund_GiftWrapChargeback { get; set; }
        public double Refund_AmazonEasyShipCharges { get; set; }

        public double Refund_SUMIGST { get; set; }
        public double Refund_SUMSGST { get; set; }
        public double Refund_SUMCGST { get; set; }
        public double refund_SumOrder { get; set; }
        public double refund_SumFee { get; set; }
        public double prcntRealization { get; set; }

        public string ExpenseName { get; set; }

        public double Igsttotal { get; set; }
        public double Sgsttotal { get; set; }
        public double Cgsttotal { get; set; }

        public string orderadd { get; set; }
        public string ordersaleadd { get; set; }

        public string VoucherNumber { get; set; }
        public string Narration { get; set; }

        public double PhysicallyAmount { get; set; }
        public string PhysicallyDate { get; set; }


        public double ActualOrderTotal { get; set; }
        public double ActualCommission { get; set; }
        public double ActualFBAFee { get; set; }
        public double ActualFixedClosingFee { get; set; }
        public double ActualShippingChargebackFee { get; set; }
        public double ActualTechnologyFee { get; set; }
        public double ActualShippingDiscountFee { get; set; }
        public double ActualShippingCommision { get; set; }
        public double ActualEasyShipWeightFee { get; set; }
        public double ActualFBAPickPackFee { get; set; }
        public double ActualGiftWrapChargeback { get; set; }
        public double ActualAmazonEasyShipCharges { get; set; }
        public double ActualRefundCommission { get; set; }
        public double ActualIGST { get; set; }
        public double ActualCGST { get; set; }
        public double ActualSGST { get; set; }
        public double ActualNetTotal { get; set; }
 

        //--flipkart--//
        public double refund_flipShipping { get; set; }
        public double refund_flipCollection { get; set; }
        public double refund_flipReverseShipping { get; set; }
        public double refund_flipFixedFee { get; set; }

        public double Refund_flipShippingFeeIGST { get; set; }
        public double Refund_flipShippingFeeCGST { get; set; }
        public double Refund_flipShippingFeeSGST { get; set; }
        public double Refund_flipCollectionIGST { get; set; }
        public double Refund_flipCollectionCGST { get; set; }
        public double Refund_flipCollectionSGST { get; set; }
        public double Refund_flipReverseShippingIGST { get; set; }
        public double Refund_flipReverseShippingCGST { get; set; }
        public double Refund_flipReverseShippingSGST { get; set; }
        public double Refund_flipFixedFeeIGST { get; set; }
        public double Refund_flipFixedFeeCGST { get; set; }
        public double Refund_flipFixedFeeSGST { get; set; }

        public double flipShipping { get; set; }
        public double flipCollection { get; set; }
        public double flipReverseShipping { get; set; }
        public double flipFixedFee { get; set; }

        public double flipShippingFeeIGST { get; set; }
        public double flipShippingFeeCGST { get; set; }
        public double flipShippingFeeSGST { get; set; }
        public double flipCollectionIGST { get; set; }
        public double flipCollectionCGST { get; set; }
        public double flipCollectionSGST { get; set; }
        public double flipReverseShippingIGST { get; set; }
        public double flipReverseShippingCGST { get; set; }
        public double flipReverseShippingSGST { get; set; }
        public double flipFixedFeeIGST { get; set; }
        public double flipFixedFeeCGST { get; set; }
        public double flipFixedFeeSGST { get; set; }

        //sharad - added
        public double RefundCommisionIgst_Deducted { get; set; }

        public double Profit_lossAmount { get; set; }
        public string PercentageAmount { get; set; }



        public double Flip_Totalordervalue { get; set; }
        public double Flip_Totalcommission { get; set; }
        public double Flip_Totalshippingfee { get; set; }
        public double Flip_Totalcollectionfee { get; set; }
        public double Flip_Totalreverseshippingfee { get; set; }
        public double Flip_Totalfixedfee { get; set; }


        public string GSTType { get; set; }
        public string ECommerceGSTIN { get; set; }
        public string ApplicableTaxrate { get; set; }
        public string PlaceSupply { get; set; }
        public double Rate { get; set; }
        public double ToatlAmountGST { get; set; }


        public double PreviousReserveAmount { get; set; }
        public double CurrentReserveAmount { get; set; }
        public double  BankAmount { get; set; }


        public double Marketplacecommission { get; set; }
        public double LogisticsCharges { get; set; }

        public double PGCommission { get; set; }
        public double Penaty { get; set; }
        public double NetAdjustments { get; set; }
        public double RefundMarketplacecommission { get; set; }
        public double RefundLogisticsCharges { get; set; }
        public double RefundPGCommission { get; set; }
        public double RefundPenaty { get; set; }
        public double RefundNetAdjustments { get; set; }


        public double payMarketplaceCommission { get; set; }

        public double MarketplaceCommissionIGST { get; set; }
        public double MarketplaceCommissionCGST { get; set; }
        public double MarketplaceCommissionSGST { get; set; }

        public double payLogisticsCharges { get; set; }

        public double LogisticsChargesIGST { get; set; }
        public double LogisticsChargesCGST { get; set; }
        public double LogisticsChargesSGST { get; set; }

        public double payPGCommission { get; set; }

        public double PGCommissionIGST { get; set; }
        public double PGCommissionCGST { get; set; }
        public double PGCommissionSGST { get; set; }

        public double payPenalty { get; set; }

        public double PenaltyIGST { get; set; }
        public double PenaltyCGST { get; set; }
        public double PenaltySGST { get; set; }

        public double payNetAdjustments { get; set; }

        public double NetAdjustmentsIGST { get; set; }
        public double NetAdjustmentsCGST { get; set; }
        public double NetAdjustmentsSGST { get; set; }


    }

    public class partial_tbl_order_history
    {
        public int? id { get; set; }
        public string order_id { get; set; }
        public string fullfillment_id { get; set; }
        public double principalvalue { get; set; }
        public double IGST { get; set; }
        public double SGST { get; set; }
        public double CGST { get; set; }
        public double TotalValue { get; set; }
        public double NetRealization { get; set; }

        public string physicallyreturn { get; set; }
        public string condition { get; set; }
        public string Msg { get; set; }
        public int? returngoods { get; set; }
        public string sku { get; set; }
        public string OrderDate { get; set; }
        public double ClaimReceived_order { get; set; }
        public double ClaimReceived_refund { get; set; }
        public string SelectDate { get; set; }
        public DateTime? physically_selected_date { get; set; }

        public DateTime? claimselected_date { get; set; }
        public string claim_return_type { get; set; }
        public string MarketPlaceName { get; set; }
        public string PhysicallyDate { get; set; }
        public string ReturnType { get; set; }
        public string ConditionType { get; set; }
    }


    public class MonthReport
    {
        //public int Number { get; set; }
        public string particulars { get; set; }
        public double Jan { get; set; }
        public double Feb { get; set; }
        public double March { get; set; }
        public double April { get; set; }
        public double May { get; set; }
        public double June { get; set; }
        public double July { get; set; }
        public double Aug { get; set; }
        public double Sept { get; set; }
        public double Oct { get; set; }
        public double Nov { get; set; }
        public double Dec { get; set; }
        public double Total_mounthcount { get; set; }       
    }
    public class SowMonthColumn
    {
        public bool ShowJan { get; set; }
        public bool ShowFeb { get; set; }
        public bool ShowMarch { get; set; }
        public bool ShowApril { get; set; }
        public bool ShowMay { get; set; }
        public bool ShowJune { get; set; }
        public bool ShowJuly { get; set; }
        public bool ShowAug { get; set; }
        public bool ShowSept { get; set; }
        public bool ShowOct { get; set; }
        public bool ShowNov { get; set; }
        public bool ShowDec { get; set; }
        public int ColSpan { get; set; }
    }

    public class proc_Settlement_report
    {
        public int orderid { get; set; }
        public string amazon_order_id { get; set; }
        public DateTime? purchase_date { get; set; }
        public DateTime? Latest_ShipDate { get; set; }
        public string order_item_id { get; set; }
        public string product_name { get; set; }
        public string sku_no { get; set; }
        public int orderdetailsid { get; set; }
        public double? item_price_amount { get; set; }
        public double? shipping_price_Amount { get; set; }
        public double? shipping_tax_Amount { get; set; }
        public double? promotion_amount { get; set; }
        public double? item_promotionAmount { get; set; }             
        public double? giftwrapprice_amount { get; set; }
        public double? item_tax_amount { get; set; }

        public int? tbl_referneced_id { get; set; }
        public double? Igst_amount { get; set; }
        public double? CGST_amount { get; set; }
        public double? sgst_amount { get; set; }

        public string Order_Id { get; set; }
        public string settlement_id { get; set; }
        public double? principal_price { get; set; }
        public double? product_tax { get; set; }
        public double? shipping_price { get; set; }
        public double? shipping_tax { get; set; }
        public double? giftwrap_price { get; set; }
        public double? giftwarp_tax { get; set; }
        public double? shipping_discount { get; set; }
        public double? shipping_tax_discount { get; set; }
        public string Settlement_sku { get; set; }


        public string history_OrderID { get; set; }
        public string history_settlement_id { get; set; }
        public double? history_amount_per_unit { get; set; }
        public double? history_product_tax { get; set; }
        public double? history_shipping_price { get; set; }
        public double? history_shipping_tax { get; set; }
        public double? history_Giftwrap_price { get; set; }
        public double? history_gift_wrap_tax { get; set; }
        public double? history_shipping_discount { get; set; }
        public double? history_shipping_tax_discount { get; set; }
        public string history_sku { get; set; }
    }


}