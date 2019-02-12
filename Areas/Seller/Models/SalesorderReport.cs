using SellerVendor.Areas.Seller.Models.DBContext;
using SellerVendor.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace SellerVendor.Areas.Seller.Models
{
    public class SalesorderReport
    {
        public SellerContext dba = new SellerContext();
        public SellerAdminContext db = new SellerAdminContext();

        /// <summary>
        /// this is for fetching data related to amazon marketplace 
        /// </summary>
        /// <param name="lstOrdertext2"></param>
        /// <param name="txt_from"></param>
        /// <param name="txt_to"></param>
        /// <param name="sellers_id"></param>
        /// <param name="stateid"></param>
        /// <param name="ddl_market_place"></param>
        /// <param name="MarketPlace"></param>
        /// <returns></returns>
        /// 
        public string Get_SalesReport_Amazon(List<SaleReport> lstOrdertext2, SaleReport view_salereport, DateTime? txt_from, DateTime? txt_to, int sellers_id, int? ddl_market_place, int? ddl_percentage)
        {
            string Msg = "S";
            try
            {
                if (txt_from != null && txt_to != null)
                {
                    using (var connection = dba.Database.Connection)
                    {
                        connection.Open();
                        var command = connection.CreateCommand();
                        command.CommandText = "get_settlement_report"; // "get_sett_orders";
                        command.CommandType = CommandType.StoredProcedure;

                        MySql.Data.MySqlClient.MySqlParameter param, param1, param2, param3;
                        param = new MySql.Data.MySqlClient.MySqlParameter("@sellerid", sellers_id);
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.Int32;
                        command.Parameters.Add(param);

                        param1 = new MySql.Data.MySqlClient.MySqlParameter("@datefrom", txt_from);
                        param1.Direction = ParameterDirection.Input;
                        param1.DbType = DbType.DateTime;
                        command.Parameters.Add(param1);

                        param2 = new MySql.Data.MySqlClient.MySqlParameter("@dateto", txt_to);
                        param2.Direction = ParameterDirection.Input;
                        param2.DbType = DbType.DateTime;
                        command.Parameters.Add(param2);

                        param3 = new MySql.Data.MySqlClient.MySqlParameter("@marketplaceid", ddl_market_place);
                        param3.Direction = ParameterDirection.Input;
                        param3.DbType = DbType.Int32;
                        command.Parameters.Add(param3);

                        using (var reader = command.ExecuteReader())
                        {
                            var get_settlement_data =
                               ((System.Data.Entity.Infrastructure.IObjectContextAdapter)dba)
                                   .ObjectContext
                                   .Translate<proc_Settlement_report>(reader)
                                   .ToList();

                            reader.NextResult();

                            var get_settlement_data1 =
                                ((System.Data.Entity.Infrastructure.IObjectContextAdapter)dba)
                                    .ObjectContext
                                    .Translate<proc_Settlement_report>(reader)
                                    .ToList();

                            reader.NextResult();

                            var get_settlement_data2 =
                                ((System.Data.Entity.Infrastructure.IObjectContextAdapter)dba)
                                    .ObjectContext
                                    .Translate<proc_Settlement_report>(reader)
                                    .ToList();

                            reader.NextResult();

                            var get_history_data =
                                ((System.Data.Entity.Infrastructure.IObjectContextAdapter)dba)
                                    .ObjectContext
                                    .Translate<proc_Settlement_report>(reader)
                                    .ToList();

                            connection.Close();
                            dba = new SellerContext();
                            dba.Configuration.AutoDetectChangesEnabled = false;
                            if (get_settlement_data != null)
                            {
                                for (int maincounter = 0; maincounter < get_settlement_data.Count; maincounter++)
                                {

                                    view_salereport = new SaleReport();
                                    var sett = get_settlement_data[maincounter];
                                    if (sett.amazon_order_id == "408-4616397-7858764")
                                    {
                                    }
                                    string amazon_order_id = sett.amazon_order_id;
                                    string details_sku = sett.sku_no;
                                    int orderdetails_id = sett.orderdetailsid;
                                    view_salereport.OrderID = sett.amazon_order_id;
                                    view_salereport.OrderDate = Convert.ToDateTime(sett.purchase_date).ToString("yyyy-MM-dd");

                                    double Principal = 0, itemtax_amount = 0, Shipping = 0, orderigst = 0, ordersgst = 0, ordercgst = 0, shippingtax = 0, Giftwrap = 0, itempromotion = 0, shippingpromotion = 0;

                                    view_salereport.ProductName = sett.product_name;
                                    view_salereport.skuNo = sett.sku_no;

                                    Principal = Convert.ToDouble(sett.item_price_amount);
                                    Shipping = Convert.ToDouble(sett.shipping_price_Amount);
                                    Giftwrap = Convert.ToDouble(sett.giftwrapprice_amount);
                                    shippingtax = Convert.ToDouble(sett.shipping_tax_Amount);
                                    itempromotion = Convert.ToDouble(sett.item_promotionAmount);
                                    shippingpromotion = Convert.ToDouble(sett.promotion_amount);
                                    //itemtax_amount = Convert.ToDouble(sett.item_tax_amount);

                                    view_salereport.Principal = Principal + Shipping + Giftwrap - itempromotion - shippingpromotion;

                                    if (get_settlement_data1 != null)
                                    {
                                        foreach (var sett5 in get_settlement_data1)// use for tax data
                                        {
                                            //int refer_id = Convert.ToInt16(sett5.tbl_referneced_id);
                                            if (orderdetails_id == sett5.tbl_referneced_id)
                                            {
                                                orderigst = Convert.ToDouble(sett5.Igst_amount);
                                                ordersgst = Convert.ToDouble(sett5.sgst_amount);
                                                ordercgst = Convert.ToDouble(sett5.CGST_amount);

                                                view_salereport.orderigst = orderigst;
                                                view_salereport.ordersgst = ordersgst;
                                                view_salereport.ordercgst = ordercgst;
                                            }
                                        }
                                    }// end of if(get_settlement_data1)

                                    view_salereport.ordertotal = view_salereport.Principal + view_salereport.orderigst + view_salereport.ordersgst + view_salereport.ordercgst;

                                    #region get settlement details with expense and tax

                                    if (get_settlement_data2 != null)
                                    {
                                        foreach (var settle in get_settlement_data2)
                                        {
                                            string skuno = settle.Settlement_sku.ToLower();
                                            if (amazon_order_id == settle.Order_Id && details_sku.ToLower() == skuno)
                                            {
                                                view_salereport.orderprincipal = Convert.ToDouble(settle.principal_price);
                                                view_salereport.orderproduct_tax = Convert.ToDouble(settle.product_tax);
                                                view_salereport.ordershipping = Convert.ToDouble(settle.shipping_price);
                                                view_salereport.ordershipping_tax = Convert.ToDouble(settle.shipping_tax);
                                                view_salereport.ordergiftwrap = Convert.ToDouble(settle.giftwrap_price);
                                                view_salereport.ordergiftwrap_tax = Convert.ToDouble(settle.giftwarp_tax);
                                                view_salereport.ordershipping_discount = Convert.ToDouble(settle.shipping_discount);
                                                view_salereport.ordershipping_discounttax = Convert.ToDouble(settle.shipping_tax_discount);

                                                view_salereport.orderTotal = view_salereport.orderprincipal + view_salereport.orderproduct_tax + view_salereport.ordershipping + view_salereport.ordershipping_tax + view_salereport.ordergiftwrap + view_salereport.ordergiftwrap_tax + view_salereport.ordershipping_discount + view_salereport.ordershipping_discounttax;
                                                view_salereport.ReferenceID = settle.settlement_id;

                                                //-------------------------End---------------------------//
                                                //-----------------------Get  data from tbl_Expense from settlement_Id----------------//

                                                double FBAFEE = 0, FBACGST = 0, FBASGST = 0, TechnologyFee = 0, TechnologyIGST = 0, TechnologyCGST = 0, TechnologySGST = 0, CommissionFee = 0, CommissionIGST = 0, CommissionCGST = 0, CommissionSGST = 0, FixedClosingFee = 0, FixedclosingIGST = 0,
                                                       FixedclosingCGST = 0, FixedclosingSGST = 0, ShippingChargebackFee = 0, EasyShipweighthandlingfees = 0,
                                                       shippingchargeCGST = 0, shippingchargeSGST = 0, ShippingDiscountFee = 0, Shippingtaxdiscount = 0, RefundCommision = 0, RefundDiscount = 0, ShippingCommision = 0, ShippingCommissionIGST = 0, EasyShipweighthandlingfeesIGST = 0,
                                                       FBAPickPackFee = 0, FBAPickPackFeeCGST = 0, FBAPickPackFeeSGST = 0, GiftWrapChargeback = 0, GiftWrapChargebackCGST = 0, GiftWrapChargebackSGST = 0, AmazonEasyShipCharges = 0, AmazonEasyShipChargesIGST = 0;

                                                var getsettlementdetails = dba.m_tbl_expense.Where(a => a.settlement_order_id.Contains(amazon_order_id) && a.sku_no.ToLower() == skuno && a.tbl_seller_id == sellers_id && a.t_transactionType_id == 1).ToList();
                                                if (getsettlementdetails != null && getsettlementdetails.Count > 0)
                                                {
                                                    foreach (var item1 in getsettlementdetails)
                                                    {
                                                        var exp_id = item1.expense_type_id;
                                                        var get_expdetails = dba.m_settlement_fee.Where(a => a.id == exp_id).FirstOrDefault();
                                                        var gettax_details = dba.tbl_tax.Where(a => a.tbl_referneced_id == item1.id && a.reference_type == 2).FirstOrDefault();
                                                        if (get_expdetails != null)
                                                        {
                                                            string nam = get_expdetails.return_fee;
                                                            if (nam == "FBA Weight Handling Fee")
                                                            {
                                                                if (get_expdetails.id == exp_id)
                                                                {
                                                                    FBAFEE = FBAFEE + Convert.ToDouble(item1.expense_amount);
                                                                    view_salereport.FBAFEE = FBAFEE;
                                                                    if (gettax_details != null)
                                                                    {
                                                                        FBACGST = FBACGST + Convert.ToDouble(gettax_details.CGST_amount);
                                                                        FBASGST = FBASGST + Convert.ToDouble(gettax_details.sgst_amount);
                                                                        view_salereport.FBACGST = FBACGST;
                                                                        view_salereport.FBASGST = FBASGST;
                                                                    }
                                                                }
                                                            }
                                                            else if (nam == "Technology Fee")
                                                            {
                                                                if (get_expdetails.id == exp_id)
                                                                {
                                                                    TechnologyFee = TechnologyFee + Convert.ToDouble(item1.expense_amount);
                                                                    view_salereport.TechnologyFee = TechnologyFee;
                                                                    if (gettax_details != null)
                                                                    {
                                                                        TechnologyIGST = TechnologyIGST + Convert.ToDouble(gettax_details.Igst_amount);
                                                                        TechnologyCGST = TechnologyCGST + Convert.ToDouble(gettax_details.CGST_amount);
                                                                        TechnologySGST = TechnologySGST + Convert.ToDouble(gettax_details.sgst_amount);

                                                                        view_salereport.TechnologyIGST = TechnologyIGST;
                                                                        view_salereport.TechnologyCGST = TechnologyCGST;
                                                                        view_salereport.TechnologySGST = TechnologySGST;
                                                                    }
                                                                }
                                                            }
                                                            else if (nam == "Commission")
                                                            {
                                                                if (get_expdetails.id == exp_id)
                                                                {
                                                                    CommissionFee = CommissionFee + Convert.ToDouble(item1.expense_amount);
                                                                    view_salereport.CommissionFee = CommissionFee;
                                                                    if (gettax_details != null)
                                                                    {
                                                                        CommissionIGST = CommissionIGST + Convert.ToDouble(gettax_details.Igst_amount);
                                                                        CommissionCGST = CommissionCGST + Convert.ToDouble(gettax_details.CGST_amount);
                                                                        CommissionSGST = CommissionSGST + Convert.ToDouble(gettax_details.sgst_amount);
                                                                        view_salereport.CommissionIGST = CommissionIGST;
                                                                        view_salereport.CommissionCGST = CommissionCGST;
                                                                        view_salereport.CommissionSGST = CommissionSGST;
                                                                    }
                                                                }
                                                            }
                                                            else if (nam == "Fixed closing fee")
                                                            {
                                                                if (get_expdetails.id == exp_id)
                                                                {
                                                                    FixedClosingFee = FixedClosingFee + Convert.ToDouble(item1.expense_amount);
                                                                    view_salereport.FixedClosingFee = FixedClosingFee;
                                                                    if (gettax_details != null)
                                                                    {
                                                                        FixedclosingIGST = FixedclosingIGST + Convert.ToDouble(gettax_details.Igst_amount);
                                                                        FixedclosingCGST = FixedclosingCGST + Convert.ToDouble(gettax_details.CGST_amount);
                                                                        FixedclosingSGST = FixedclosingSGST + Convert.ToDouble(gettax_details.sgst_amount);

                                                                        view_salereport.FixedclosingIGST = FixedclosingIGST;
                                                                        view_salereport.FixedclosingCGST = FixedclosingCGST;
                                                                        view_salereport.FixedclosingSGST = FixedclosingSGST;
                                                                    }
                                                                }
                                                            }
                                                            else if (nam == "Shipping Chargeback")
                                                            {
                                                                if (get_expdetails.id == exp_id)
                                                                {
                                                                    ShippingChargebackFee = ShippingChargebackFee + Convert.ToDouble(item1.expense_amount);
                                                                    view_salereport.ShippingChargebackFee = ShippingChargebackFee;
                                                                    if (gettax_details != null)
                                                                    {
                                                                        shippingchargeCGST = shippingchargeCGST + Convert.ToDouble(gettax_details.CGST_amount);
                                                                        shippingchargeSGST = shippingchargeSGST + Convert.ToDouble(gettax_details.sgst_amount);
                                                                        view_salereport.shippingchargeCGST = shippingchargeCGST;
                                                                        view_salereport.shippingchargeSGST = shippingchargeSGST;
                                                                    }
                                                                }
                                                            }
                                                            else if (nam == "Shipping discount")
                                                            {
                                                                if (get_expdetails.id == exp_id)
                                                                {
                                                                    ShippingDiscountFee = ShippingDiscountFee + Convert.ToDouble(item1.expense_amount);
                                                                    view_salereport.ShippingDiscountFee = ShippingDiscountFee;
                                                                    if (gettax_details != null)
                                                                    {
                                                                        Shippingtaxdiscount = Shippingtaxdiscount + Convert.ToDouble(gettax_details.Igst_amount);
                                                                        view_salereport.Shippingtaxdiscount = Shippingtaxdiscount;
                                                                    }
                                                                }
                                                            }
                                                            else if (nam == "Refund commission")
                                                            {
                                                                if (get_expdetails.id == exp_id)
                                                                {
                                                                    RefundCommision = RefundCommision + Convert.ToDouble(item1.expense_amount);
                                                                    view_salereport.RefundCommision = RefundCommision;
                                                                    if (gettax_details != null)
                                                                    {
                                                                        RefundDiscount = RefundDiscount + Convert.ToDouble(gettax_details.Igst_amount);
                                                                        view_salereport.RefundDiscount = RefundDiscount;
                                                                    }
                                                                }
                                                            }

                                                            else if (nam == "Shipping commission")
                                                            {
                                                                if (get_expdetails.id == exp_id)
                                                                {
                                                                    ShippingCommision = ShippingCommision + Convert.ToDouble(item1.expense_amount);
                                                                    view_salereport.ShippingCommision = ShippingCommision;
                                                                    if (gettax_details != null)
                                                                    {
                                                                        ShippingCommissionIGST = ShippingCommissionIGST + Convert.ToDouble(gettax_details.Igst_amount);
                                                                        view_salereport.ShippingCommissionIGST = ShippingCommissionIGST;
                                                                    }
                                                                }
                                                            }
                                                            else if (nam == "Easy Ship weight handling fees")
                                                            {
                                                                if (get_expdetails.id == exp_id)
                                                                {
                                                                    EasyShipweighthandlingfees = EasyShipweighthandlingfees + Convert.ToDouble(item1.expense_amount);
                                                                    view_salereport.EasyShipweighthandlingfees = EasyShipweighthandlingfees;
                                                                    if (gettax_details != null)
                                                                    {
                                                                        EasyShipweighthandlingfeesIGST = EasyShipweighthandlingfeesIGST + Convert.ToDouble(gettax_details.Igst_amount);
                                                                        view_salereport.EasyShipweighthandlingfeesIGST = EasyShipweighthandlingfeesIGST;
                                                                    }
                                                                }
                                                            }

                                                            else if (nam == "FBA Pick & Pack Fee")
                                                            {
                                                                if (get_expdetails.id == exp_id)
                                                                {
                                                                    FBAPickPackFee = FBAPickPackFee + Convert.ToDouble(item1.expense_amount);
                                                                    view_salereport.FBAPickPackFee = FBAPickPackFee;
                                                                    if (gettax_details != null)
                                                                    {
                                                                        FBAPickPackFeeCGST = FBAPickPackFeeCGST + Convert.ToDouble(gettax_details.CGST_amount);
                                                                        FBAPickPackFeeSGST = FBAPickPackFeeSGST + Convert.ToDouble(gettax_details.sgst_amount);
                                                                        view_salereport.FBAPickPackFeeCGST = FBAPickPackFeeCGST;
                                                                        view_salereport.FBAPickPackFeeSGST = FBAPickPackFeeSGST;
                                                                    }
                                                                }
                                                            }

                                                            else if (nam == "Gift Wrap Chargeback")
                                                            {
                                                                if (get_expdetails.id == exp_id)
                                                                {
                                                                    GiftWrapChargeback = GiftWrapChargeback + Convert.ToDouble(item1.expense_amount);
                                                                    view_salereport.GiftWrapChargeback = GiftWrapChargeback;
                                                                    if (gettax_details != null)
                                                                    {
                                                                        GiftWrapChargebackCGST = GiftWrapChargebackCGST + Convert.ToDouble(gettax_details.CGST_amount);
                                                                        GiftWrapChargebackSGST = GiftWrapChargebackSGST + Convert.ToDouble(gettax_details.sgst_amount);
                                                                        view_salereport.GiftWrapChargebackCGST = GiftWrapChargebackCGST;
                                                                        view_salereport.GiftWrapChargebackSGST = GiftWrapChargebackSGST;
                                                                    }
                                                                }
                                                            }
                                                            else if (nam == "Amazon Easy Ship Charges")
                                                            {
                                                                if (get_expdetails.id == exp_id)
                                                                {
                                                                    AmazonEasyShipCharges = AmazonEasyShipCharges + Convert.ToDouble(item1.expense_amount);
                                                                    view_salereport.AmazonEasyShipCharges = AmazonEasyShipCharges;
                                                                    if (gettax_details != null)
                                                                    {
                                                                        AmazonEasyShipChargesIGST = AmazonEasyShipChargesIGST + Convert.ToDouble(gettax_details.Igst_amount);
                                                                        view_salereport.AmazonEasyShipChargesIGST = AmazonEasyShipChargesIGST;
                                                                    }
                                                                }
                                                            }
                                                        }// end of if(get_expdetails)
                                                    }// end if foreach(item1)
                                                }// end of if(getsettlementdetails) 
                                            }
                                        }// end of if (compare sku and orderid)
                                    }
                                    #endregion

                                    #region to get history table with expense
                                    if (get_history_data != null)
                                    {
                                        foreach (var history in get_history_data)
                                        {
                                            string sku_no = history.history_sku.ToLower();
                                            if (amazon_order_id == history.history_OrderID && details_sku.ToLower() == sku_no)
                                            {
                                                view_salereport.refundprincipal = Convert.ToDouble(history.history_amount_per_unit);
                                                view_salereport.refundproduct_tax = Convert.ToDouble(history.history_product_tax);
                                                view_salereport.refundshipping = Convert.ToDouble(history.history_shipping_price);
                                                view_salereport.refundshipping_tax = Convert.ToDouble(history.history_shipping_tax);
                                                view_salereport.refundgiftwrap = Convert.ToDouble(history.history_Giftwrap_price);
                                                view_salereport.refundgiftwrap_tax = Convert.ToDouble(history.history_gift_wrap_tax);
                                                view_salereport.refundshipping_discount = Convert.ToDouble(history.history_shipping_discount);
                                                view_salereport.refundshipping_discount_tax = Convert.ToDouble(history.history_shipping_tax_discount);

                                                view_salereport.refundTotal = view_salereport.refundprincipal + view_salereport.refundproduct_tax + view_salereport.refundshipping + view_salereport.refundshipping_tax + view_salereport.refundgiftwrap + view_salereport.refundgiftwrap_tax + view_salereport.refundshipping_discount + view_salereport.refundshipping_discount_tax;

                                                view_salereport.refundReferenceID = history.history_settlement_id;

                                                double refundFBAFEE = 0, refundFBACGST = 0, refundFBASGST = 0, refundTechnologyFee = 0, refundTechnologyIGST = 0, refundTechnologyCGST = 0, refundTechnologySGST = 0, refundCommissionFee = 0, refundCommissionIGST = 0, refundCommissionCGST = 0,
                                                       refundCommissionSGST = 0, refundFixedClosingFee = 0, refundFixedclosingIGST = 0,
                                                       refundFixedclosingCGST = 0, refundFixedclosingSGST = 0, refundShippingChargebackFee = 0, refundEasyShipweighthandlingfees = 0,
                                                       refundshippingchargeCGST = 0, refundshippingchargeSGST = 0, refundShippingDiscountFee = 0, refundShippingtaxdiscount = 0,
                                                       refund_RefundCommision = 0, refund_Refund_Discount = 0, refund_Discount_cgst = 0, refund_discount_sgst = 0, refundShippingCommision = 0, refundShippingCommissionIGST = 0, refundEasyShipweighthandlingfeesIGST = 0,
                                                       refundFBAPickPackFee = 0, refundFBAPickPackFeeCGST = 0, refundFBAPickPackFeeSGST = 0, refundGiftWrapChargeback = 0, refundGiftWrapChargebackCGST = 0,
                                                       refundGiftWrapChargebackSGST = 0, refundAmazonEasyShipCharges = 0, refundAmazonEasyShipChargesIGST = 0;


                                                var get_refundexpense = dba.m_tbl_expense.Where(a => a.settlement_order_id.Contains(amazon_order_id) && a.sku_no.ToLower() == sku_no && a.tbl_seller_id == sellers_id && a.t_transactionType_id == 2).ToList();// to get refund expense
                                                if (get_refundexpense != null && get_refundexpense.Count > 0)
                                                {
                                                    foreach (var refund in get_refundexpense)
                                                    {
                                                        var exp_ID = refund.expense_type_id;
                                                        var get_details = dba.m_settlement_fee.Where(a => a.id == exp_ID).FirstOrDefault();
                                                        var getExp_tax_details = dba.tbl_tax.Where(a => a.tbl_referneced_id == refund.id && a.reference_type == 7).FirstOrDefault();
                                                        if (get_details != null)
                                                        {
                                                            string nam = get_details.return_fee;
                                                            if (nam == "FBA Weight Handling Fee")
                                                            {
                                                                if (get_details.id == exp_ID)
                                                                {
                                                                    refundFBAFEE = refundFBAFEE + Convert.ToDouble(refund.expense_amount);
                                                                    view_salereport.RefundFBAFEE = refundFBAFEE;
                                                                    if (getExp_tax_details != null)
                                                                    {
                                                                        refundFBACGST = refundFBACGST + Convert.ToDouble(getExp_tax_details.CGST_amount);
                                                                        refundFBASGST = refundFBASGST + Convert.ToDouble(getExp_tax_details.sgst_amount);

                                                                        view_salereport.RefundFBACGST = refundFBACGST;
                                                                        view_salereport.RefundFBASGST = refundFBASGST;
                                                                    }
                                                                }
                                                            }
                                                            else if (nam == "Technology Fee")
                                                            {
                                                                if (get_details.id == exp_ID)
                                                                {
                                                                    refundTechnologyFee = refundTechnologyFee + Convert.ToDouble(refund.expense_amount);
                                                                    view_salereport.RefundTechnologyFee = refundTechnologyFee;
                                                                    if (getExp_tax_details != null)
                                                                    {
                                                                        refundTechnologyIGST = refundTechnologyIGST + Convert.ToDouble(getExp_tax_details.Igst_amount);
                                                                        refundTechnologyCGST = refundTechnologyCGST + Convert.ToDouble(getExp_tax_details.CGST_amount);
                                                                        refundTechnologySGST = refundTechnologySGST + Convert.ToDouble(getExp_tax_details.sgst_amount);

                                                                        view_salereport.RefundTechnologyIGST = refundTechnologyIGST;
                                                                        view_salereport.RefundTechnologyCGST = refundTechnologyCGST;
                                                                        view_salereport.RefundTechnologySGST = refundTechnologySGST;
                                                                    }
                                                                }
                                                            }
                                                            else if (nam == "Commission")
                                                            {
                                                                if (get_details.id == exp_ID)
                                                                {
                                                                    refundCommissionFee = refundCommissionFee + Convert.ToDouble(refund.expense_amount);
                                                                    view_salereport.RefundCommissionFee = refundCommissionFee;
                                                                    if (getExp_tax_details != null)
                                                                    {
                                                                        refundCommissionIGST = refundCommissionIGST + Convert.ToDouble(getExp_tax_details.Igst_amount);
                                                                        refundCommissionCGST = refundCommissionCGST + Convert.ToDouble(getExp_tax_details.CGST_amount);
                                                                        refundCommissionSGST = refundCommissionSGST + Convert.ToDouble(getExp_tax_details.sgst_amount);

                                                                        view_salereport.RefundCommissionIGST = refundCommissionIGST;
                                                                        view_salereport.RefundCommissionCGST = refundCommissionCGST;
                                                                        view_salereport.RefundCommissionSGST = refundCommissionSGST;
                                                                    }
                                                                }
                                                            }
                                                            else if (nam == "Fixed closing fee")
                                                            {
                                                                if (get_details.id == exp_ID)
                                                                {
                                                                    refundFixedClosingFee = refundFixedClosingFee + Convert.ToDouble(refund.expense_amount);
                                                                    view_salereport.RefundFixedClosingFee = refundFixedClosingFee;
                                                                    if (getExp_tax_details != null)
                                                                    {
                                                                        refundFixedclosingIGST = refundFixedclosingIGST + Convert.ToDouble(getExp_tax_details.Igst_amount);
                                                                        refundFixedclosingCGST = refundFixedclosingCGST + Convert.ToDouble(getExp_tax_details.CGST_amount);
                                                                        refundFixedclosingSGST = refundFixedclosingSGST + Convert.ToDouble(getExp_tax_details.sgst_amount);

                                                                        view_salereport.RefundFixedclosingIGST = refundFixedclosingIGST;
                                                                        view_salereport.RefundFixedclosingCGST = refundFixedclosingCGST;
                                                                        view_salereport.RefundFixedclosingSGST = refundFixedclosingSGST;
                                                                    }
                                                                }
                                                            }
                                                            else if (nam == "Shipping Chargeback")
                                                            {
                                                                if (get_details.id == exp_ID)
                                                                {
                                                                    refundShippingChargebackFee = refundShippingChargebackFee + Convert.ToDouble(refund.expense_amount);
                                                                    view_salereport.RefundShippingChargebackFee = refundShippingChargebackFee;
                                                                    if (getExp_tax_details != null)
                                                                    {
                                                                        refundshippingchargeCGST = refundshippingchargeCGST + Convert.ToDouble(getExp_tax_details.CGST_amount);
                                                                        refundshippingchargeSGST = refundshippingchargeSGST + Convert.ToDouble(getExp_tax_details.sgst_amount);
                                                                        view_salereport.RefundshippingchargeCGST = refundshippingchargeCGST;
                                                                        view_salereport.RefundshippingchargeSGST = refundshippingchargeSGST;
                                                                    }
                                                                }
                                                            }
                                                            else if (nam == "Shipping discount")
                                                            {
                                                                if (get_details.id == exp_ID)
                                                                {
                                                                    refundShippingDiscountFee = refundShippingDiscountFee + Convert.ToDouble(refund.expense_amount);
                                                                    view_salereport.RefundShippingDiscountFee = refundShippingDiscountFee;
                                                                    if (getExp_tax_details != null)
                                                                    {
                                                                        refundShippingtaxdiscount = refundShippingtaxdiscount + Convert.ToDouble(getExp_tax_details.Igst_amount);
                                                                        view_salereport.RefundShippingtaxdiscount = refundShippingtaxdiscount;
                                                                    }
                                                                }
                                                            }
                                                            else if (nam == "Refund commission")
                                                            {
                                                                if (get_details.id == exp_ID)
                                                                {
                                                                    refund_RefundCommision = refund_RefundCommision + Convert.ToDouble(refund.expense_amount);
                                                                    view_salereport.Refund_Commision = refund_RefundCommision;
                                                                    if (getExp_tax_details != null)
                                                                    {
                                                                        refund_Refund_Discount = refund_Refund_Discount + Convert.ToDouble(getExp_tax_details.Igst_amount);
                                                                        refund_Discount_cgst = refund_Discount_cgst + Convert.ToDouble(getExp_tax_details.CGST_amount);
                                                                        refund_discount_sgst = refund_discount_sgst + Convert.ToDouble(getExp_tax_details.sgst_amount);

                                                                        view_salereport.Refund_Discount = refund_Refund_Discount;
                                                                        view_salereport.Refund_DiscountCGST = refund_Discount_cgst;
                                                                        view_salereport.Refund_DiscountSGST = refund_discount_sgst;
                                                                    }
                                                                }
                                                            }

                                                            else if (nam == "Shipping commission")
                                                            {
                                                                if (get_details.id == exp_ID)
                                                                {
                                                                    refundShippingCommision = refundShippingCommision + Convert.ToDouble(refund.expense_amount);
                                                                    view_salereport.Refund_ShippingCommision = refundShippingCommision;
                                                                    if (getExp_tax_details != null)
                                                                    {
                                                                        refundShippingCommissionIGST = refundShippingCommissionIGST + Convert.ToDouble(getExp_tax_details.Igst_amount);
                                                                        view_salereport.Refund_Shipping_Commission = refundShippingCommissionIGST;
                                                                    }
                                                                }
                                                            }
                                                            else if (nam == "Easy Ship weight handling fees")
                                                            {
                                                                if (get_details.id == exp_ID)
                                                                {
                                                                    refundEasyShipweighthandlingfees = refundEasyShipweighthandlingfees + Convert.ToDouble(refund.expense_amount);
                                                                    view_salereport.Refund_EasyShipweighthandlingfees = refundEasyShipweighthandlingfees;
                                                                    if (getExp_tax_details != null)
                                                                    {
                                                                        refundEasyShipweighthandlingfeesIGST = refundEasyShipweighthandlingfeesIGST + Convert.ToDouble(getExp_tax_details.Igst_amount);
                                                                        view_salereport.Refund_EasyShipweighthandlingfeesIGST = refundEasyShipweighthandlingfeesIGST;
                                                                    }
                                                                }
                                                            }

                                                            else if (nam == "FBA Pick & Pack Fee")
                                                            {
                                                                if (get_details.id == exp_ID)
                                                                {
                                                                    refundFBAPickPackFee = refundFBAPickPackFee + Convert.ToDouble(refund.expense_amount);
                                                                    view_salereport.Refund_FBAPick_PackFee = refundFBAPickPackFee;
                                                                    if (getExp_tax_details != null)
                                                                    {
                                                                        refundFBAPickPackFeeCGST = refundFBAPickPackFeeCGST + Convert.ToDouble(getExp_tax_details.CGST_amount);
                                                                        refundFBAPickPackFeeSGST = refundFBAPickPackFeeSGST + Convert.ToDouble(getExp_tax_details.sgst_amount);
                                                                        view_salereport.RefundFBAPick_PackFeeCGST = refundFBAPickPackFeeCGST;
                                                                        view_salereport.RefundFBAPick_PackFeeSGST = refundFBAPickPackFeeSGST;
                                                                    }
                                                                }
                                                            }
                                                            else if (nam == "Gift Wrap Chargeback")
                                                            {
                                                                if (get_details.id == exp_ID)
                                                                {
                                                                    refundGiftWrapChargeback = refundGiftWrapChargeback + Convert.ToDouble(refund.expense_amount);
                                                                    view_salereport.Refund_GiftWrapChargeback = refundGiftWrapChargeback;
                                                                    if (getExp_tax_details != null)
                                                                    {
                                                                        refundGiftWrapChargebackCGST = refundGiftWrapChargebackCGST + Convert.ToDouble(getExp_tax_details.CGST_amount);
                                                                        refundGiftWrapChargebackSGST = refundGiftWrapChargebackSGST + Convert.ToDouble(getExp_tax_details.sgst_amount);
                                                                        view_salereport.RefundGiftWrapChargebackCGST = refundGiftWrapChargebackCGST;
                                                                        view_salereport.RefundGiftWrapChargebackSGST = refundGiftWrapChargebackSGST;
                                                                    }
                                                                }
                                                            }
                                                            else if (nam == "Amazon Easy Ship Charges")
                                                            {
                                                                if (get_details.id == exp_ID)
                                                                {
                                                                    refundAmazonEasyShipCharges = refundAmazonEasyShipCharges + Convert.ToDouble(refund.expense_amount);
                                                                    view_salereport.Refund_AmazonEasyShipCharges = refundAmazonEasyShipCharges;
                                                                    if (getExp_tax_details != null)
                                                                    {
                                                                        refundAmazonEasyShipChargesIGST = refundAmazonEasyShipChargesIGST + Convert.ToDouble(getExp_tax_details.Igst_amount);
                                                                        view_salereport.Refund_AmazonEasyShipChargesIGST = refundAmazonEasyShipChargesIGST;
                                                                    }
                                                                }
                                                            }

                                                        }// end of if(get_details)                               
                                                    }// end of foreach(refund)
                                                }// end of if(get_refundexpense)

                                            }// end of if(compare orderid and sku)                                                                                        
                                        }// end of for each loop(history)
                                    }// end of if (check not null)

                                    #endregion


                                    view_salereport.Refund_SUMIGST = view_salereport.RefundTechnologyIGST + view_salereport.RefundCommissionIGST + view_salereport.RefundFixedclosingIGST + view_salereport.Refund_EasyShipweighthandlingfeesIGST + view_salereport.RefundShippingtaxdiscount + view_salereport.Refund_Discount + view_salereport.Refund_Shipping_Commission + view_salereport.Refund_AmazonEasyShipChargesIGST;
                                    view_salereport.Refund_SUMCGST = view_salereport.RefundshippingchargeCGST + view_salereport.RefundFBACGST + view_salereport.RefundFBAPick_PackFeeCGST + view_salereport.RefundGiftWrapChargebackCGST + view_salereport.Refund_DiscountCGST + view_salereport.RefundFixedclosingCGST + view_salereport.RefundCommissionCGST + view_salereport.RefundTechnologyCGST;
                                    view_salereport.Refund_SUMSGST = view_salereport.RefundshippingchargeSGST + view_salereport.RefundFBASGST + view_salereport.RefundFBAPick_PackFeeSGST + view_salereport.RefundGiftWrapChargebackSGST + view_salereport.Refund_DiscountSGST + view_salereport.RefundFixedclosingSGST + view_salereport.RefundCommissionSGST + view_salereport.RefundTechnologySGST;
                                    view_salereport.refund_SumFee = view_salereport.RefundFBAFEE + view_salereport.RefundTechnologyFee + view_salereport.RefundCommissionFee + view_salereport.RefundFixedClosingFee + view_salereport.RefundShippingChargebackFee + view_salereport.RefundShippingDiscountFee + view_salereport.Refund_EasyShipweighthandlingfees + view_salereport.Refund_Commision + view_salereport.Refund_ShippingCommision + view_salereport.Refund_FBAPick_PackFee + view_salereport.Refund_GiftWrapChargeback + view_salereport.Refund_AmazonEasyShipCharges + view_salereport.Refund_SUMIGST + view_salereport.Refund_SUMCGST + view_salereport.Refund_SUMSGST;
                                    view_salereport.refund_SumOrder = view_salereport.refundTotal + view_salereport.refund_SumFee;

                                    view_salereport.SUMIGST = view_salereport.TechnologyIGST + view_salereport.CommissionIGST + view_salereport.FixedclosingIGST + view_salereport.Shippingtaxdiscount + view_salereport.RefundDiscount + view_salereport.ShippingCommissionIGST + view_salereport.EasyShipweighthandlingfeesIGST + view_salereport.AmazonEasyShipChargesIGST;
                                    view_salereport.SUMCGST = view_salereport.shippingchargeCGST + view_salereport.FBACGST + view_salereport.FBAPickPackFeeCGST + view_salereport.GiftWrapChargebackCGST + view_salereport.FixedclosingCGST + view_salereport.CommissionCGST + view_salereport.TechnologyCGST;
                                    view_salereport.SUMSGST = view_salereport.shippingchargeSGST + view_salereport.FBASGST + view_salereport.FBAPickPackFeeSGST + view_salereport.GiftWrapChargebackSGST + view_salereport.FixedclosingSGST + view_salereport.CommissionSGST + view_salereport.TechnologySGST;
                                    view_salereport.SumFee = view_salereport.FBAFEE + view_salereport.TechnologyFee + view_salereport.CommissionFee + view_salereport.FixedClosingFee + view_salereport.ShippingChargebackFee + view_salereport.ShippingDiscountFee + view_salereport.RefundCommision + view_salereport.ShippingCommision + view_salereport.EasyShipweighthandlingfees + view_salereport.FBAPickPackFee + view_salereport.GiftWrapChargeback + view_salereport.AmazonEasyShipCharges + view_salereport.SUMIGST + view_salereport.SUMCGST + view_salereport.SUMSGST;
                                    view_salereport.SumOrder = view_salereport.orderTotal + view_salereport.SumFee;
                                    view_salereport.NetTotal = view_salereport.SumOrder + view_salereport.refund_SumOrder;



                                    view_salereport.ActualOrderTotal = view_salereport.orderTotal + view_salereport.refundTotal;
                                    view_salereport.ActualCommission = view_salereport.CommissionFee + view_salereport.RefundCommissionFee;
                                    view_salereport.ActualFBAFee = view_salereport.FBAFEE + view_salereport.RefundFBAFEE;
                                    view_salereport.ActualFixedClosingFee = view_salereport.FixedClosingFee + view_salereport.RefundFixedClosingFee;
                                    view_salereport.ActualShippingChargebackFee = view_salereport.ShippingChargebackFee + view_salereport.RefundShippingChargebackFee;
                                    view_salereport.ActualTechnologyFee = view_salereport.TechnologyFee + view_salereport.RefundTechnologyFee;
                                    view_salereport.ActualShippingDiscountFee = view_salereport.ShippingDiscountFee + view_salereport.RefundShippingDiscountFee;
                                    view_salereport.ActualShippingCommision = view_salereport.ShippingCommision + view_salereport.Refund_ShippingCommision;
                                    view_salereport.ActualEasyShipWeightFee = view_salereport.EasyShipweighthandlingfees + view_salereport.Refund_EasyShipweighthandlingfees;
                                    view_salereport.ActualFBAPickPackFee = view_salereport.FBAPickPackFee + view_salereport.Refund_FBAPick_PackFee;
                                    view_salereport.ActualGiftWrapChargeback = view_salereport.GiftWrapChargeback + view_salereport.Refund_GiftWrapChargeback;
                                    view_salereport.ActualAmazonEasyShipCharges = view_salereport.AmazonEasyShipCharges + view_salereport.Refund_AmazonEasyShipCharges;
                                    view_salereport.ActualRefundCommission = view_salereport.RefundCommision + view_salereport.Refund_Commision;
                                    view_salereport.ActualIGST = view_salereport.SUMIGST + view_salereport.Refund_SUMIGST;
                                    view_salereport.ActualCGST = view_salereport.SUMCGST + view_salereport.Refund_SUMCGST;
                                    view_salereport.ActualSGST = view_salereport.SUMSGST + view_salereport.Refund_SUMSGST;
                                    view_salereport.ActualNetTotal = view_salereport.SumOrder + view_salereport.refund_SumOrder;


                                    string value = "";
                                    value = Convert.ToString((view_salereport.NetTotal / view_salereport.orderTotal) * 100);
                                    if (value != "NaN" && value != "-Infinity")
                                    {
                                        decimal abc = Convert.ToDecimal((view_salereport.NetTotal / view_salereport.orderTotal) * 100);
                                        decimal result = decimal.Round(abc, 2, MidpointRounding.AwayFromZero);

                                        string bb = value.ToString();
                                        string[] b = bb.Split('.');
                                        int firstValue = int.Parse(b[0]);

                                        string fvalue = result + "%";
                                        view_salereport.PercentageAmount = Convert.ToString(fvalue);
                                    }
                                    if (PercentageFilter(ddl_percentage, view_salereport.PercentageAmount))
                                        lstOrdertext2.Add(view_salereport);

                                    // lstOrdertext2.Add(view_salereport);
                                }// end of for (counter)


                            }//end of if(get_settlement_data)
                        }
                    }// end of using connection
                }// end of if (txt from )
            }// end of try block
            catch (Exception ex)
            {
            }
            return Msg;
        }


        public string Get_SalesReport_Amazon1(List<SaleReport> lstOrdertext2, SaleReport view_salereport, DateTime? txt_from, DateTime? txt_to, int sellers_id, int? ddl_market_place, int? ddl_percentage)//it is correct but not in used
        {
            string Msg = "S";
            try
            {
                if (txt_from != null && txt_to != null)
                {
                    using (var connection = dba.Database.Connection)
                    {
                        connection.Open();
                        var command = connection.CreateCommand();
                        command.CommandText = "get_settlement_report"; // "get_sett_orders";
                        command.CommandType = CommandType.StoredProcedure;

                        MySql.Data.MySqlClient.MySqlParameter param, param1, param2, param3;
                        param = new MySql.Data.MySqlClient.MySqlParameter("@sellerid", sellers_id);
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.Int32;
                        command.Parameters.Add(param);

                        param1 = new MySql.Data.MySqlClient.MySqlParameter("@datefrom", txt_from);
                        param1.Direction = ParameterDirection.Input;
                        param1.DbType = DbType.DateTime;
                        command.Parameters.Add(param1);

                        param2 = new MySql.Data.MySqlClient.MySqlParameter("@dateto", txt_to);
                        param2.Direction = ParameterDirection.Input;
                        param2.DbType = DbType.DateTime;
                        command.Parameters.Add(param2);

                        param3 = new MySql.Data.MySqlClient.MySqlParameter("@marketplaceid", ddl_market_place);
                        param3.Direction = ParameterDirection.Input;
                        param3.DbType = DbType.Int32;
                        command.Parameters.Add(param3);

                        using (var reader = command.ExecuteReader())
                        {
                            var get_settlement_data =
                               ((System.Data.Entity.Infrastructure.IObjectContextAdapter)dba)
                                   .ObjectContext
                                   .Translate<proc_Settlement_report>(reader)
                                   .ToList();

                            reader.NextResult();

                            var get_settlement_data1 =
                                ((System.Data.Entity.Infrastructure.IObjectContextAdapter)dba)
                                    .ObjectContext
                                    .Translate<proc_Settlement_report>(reader)
                                    .ToList();

                            reader.NextResult();

                            var get_settlement_data2 =
                                ((System.Data.Entity.Infrastructure.IObjectContextAdapter)dba)
                                    .ObjectContext
                                    .Translate<proc_Settlement_report>(reader)
                                    .ToList();

                            reader.NextResult();

                            var get_history_data =
                                ((System.Data.Entity.Infrastructure.IObjectContextAdapter)dba)
                                    .ObjectContext
                                    .Translate<proc_Settlement_report>(reader)
                                    .ToList();

                            connection.Close();
                            dba = new SellerContext();
                            dba.Configuration.AutoDetectChangesEnabled = false;
                            if (get_settlement_data != null)
                            {
                                for (int maincounter = 0; maincounter < get_settlement_data.Count; maincounter++)
                                {
                                    view_salereport = new SaleReport();
                                    var sett = get_settlement_data[maincounter];
                                    string amazon_order_id = sett.amazon_order_id;
                                    string details_sku = sett.sku_no;
                                    int orderdetails_id = sett.orderdetailsid;
                                    view_salereport.OrderID = sett.amazon_order_id;
                                    view_salereport.OrderDate = Convert.ToDateTime(sett.purchase_date).ToString("yyyy-MM-dd");

                                    double Principal = 0, itemtax_amount = 0, Shipping = 0, orderigst = 0, ordersgst = 0, ordercgst = 0, shippingtax = 0, Giftwrap = 0, itempromotion = 0, shippingpromotion = 0;

                                    view_salereport.ProductName = sett.product_name;
                                    view_salereport.skuNo = sett.sku_no;

                                    Principal = Convert.ToDouble(sett.item_price_amount);
                                    Shipping = Convert.ToDouble(sett.shipping_price_Amount);
                                    Giftwrap = Convert.ToDouble(sett.giftwrapprice_amount);
                                    shippingtax = Convert.ToDouble(sett.shipping_tax_Amount);
                                    itempromotion = Convert.ToDouble(sett.item_promotionAmount);
                                    shippingpromotion = Convert.ToDouble(sett.promotion_amount);
                                    //itemtax_amount = Convert.ToDouble(sett.item_tax_amount);

                                    view_salereport.Principal = Principal + Shipping + Giftwrap - itempromotion - shippingpromotion;

                                    if (get_settlement_data1 != null)
                                    {
                                        foreach (var sett5 in get_settlement_data1)// use for tax data
                                        {
                                            int refer_id = Convert.ToInt16(sett5.tbl_referneced_id);
                                            if (orderdetails_id == refer_id)
                                            {
                                                orderigst = Convert.ToDouble(sett5.Igst_amount);
                                                ordersgst = Convert.ToDouble(sett5.sgst_amount);
                                                ordercgst = Convert.ToDouble(sett5.CGST_amount);

                                                view_salereport.orderigst = orderigst;
                                                view_salereport.ordersgst = ordersgst;
                                                view_salereport.ordercgst = ordercgst;
                                            }
                                        }
                                    }// end of if(get_settlement_data1)

                                    view_salereport.ordertotal = view_salereport.Principal + view_salereport.orderigst + view_salereport.ordersgst + view_salereport.ordercgst;

                                    #region get settlement details with expense and tax

                                    if (get_settlement_data2 != null)
                                    {
                                        foreach (var settle in get_settlement_data2)
                                        {
                                            string skuno = settle.Settlement_sku.ToLower();
                                            if (amazon_order_id == settle.Order_Id && details_sku.ToLower() == skuno)
                                            {
                                                view_salereport.orderprincipal = Convert.ToDouble(settle.principal_price);
                                                view_salereport.orderproduct_tax = Convert.ToDouble(settle.product_tax);
                                                view_salereport.ordershipping = Convert.ToDouble(settle.shipping_price);
                                                view_salereport.ordershipping_tax = Convert.ToDouble(settle.shipping_tax);
                                                view_salereport.ordergiftwrap = Convert.ToDouble(settle.giftwrap_price);
                                                view_salereport.ordergiftwrap_tax = Convert.ToDouble(settle.giftwarp_tax);
                                                view_salereport.ordershipping_discount = Convert.ToDouble(settle.shipping_discount);
                                                view_salereport.ordershipping_discounttax = Convert.ToDouble(settle.shipping_tax_discount);

                                                view_salereport.orderTotal = view_salereport.orderprincipal + view_salereport.orderproduct_tax + view_salereport.ordershipping + view_salereport.ordershipping_tax + view_salereport.ordergiftwrap + view_salereport.ordergiftwrap_tax + view_salereport.ordershipping_discount + view_salereport.ordershipping_discounttax;
                                                view_salereport.ReferenceID = settle.settlement_id;

                                                //-------------------------End---------------------------//
                                                //-----------------------Get  data from tbl_Expense from settlement_Id----------------//

                                                double FBAFEE = 0, FBACGST = 0, FBASGST = 0, TechnologyFee = 0, TechnologyIGST = 0, TechnologyCGST = 0, TechnologySGST = 0, CommissionFee = 0, CommissionIGST = 0, CommissionCGST = 0, CommissionSGST = 0, FixedClosingFee = 0, FixedclosingIGST = 0,
                                                       FixedclosingCGST = 0, FixedclosingSGST = 0, ShippingChargebackFee = 0, EasyShipweighthandlingfees = 0,
                                                       shippingchargeCGST = 0, shippingchargeSGST = 0, ShippingDiscountFee = 0, Shippingtaxdiscount = 0, RefundCommision = 0, RefundDiscount = 0, ShippingCommision = 0, ShippingCommissionIGST = 0, EasyShipweighthandlingfeesIGST = 0,
                                                       FBAPickPackFee = 0, FBAPickPackFeeCGST = 0, FBAPickPackFeeSGST = 0, GiftWrapChargeback = 0, GiftWrapChargebackCGST = 0, GiftWrapChargebackSGST = 0, AmazonEasyShipCharges = 0, AmazonEasyShipChargesIGST = 0;

                                                var getsettlementdetails = dba.m_tbl_expense.Where(a => a.settlement_order_id.Contains(amazon_order_id) && a.sku_no.ToLower() == skuno && a.tbl_seller_id == sellers_id && a.t_transactionType_id == 1).ToList();
                                                if (getsettlementdetails != null && getsettlementdetails.Count > 0)
                                                {
                                                    foreach (var item1 in getsettlementdetails)
                                                    {
                                                        var exp_id = item1.expense_type_id;
                                                        var get_expdetails = dba.m_settlement_fee.Where(a => a.id == exp_id).FirstOrDefault();
                                                        var gettax_details = dba.tbl_tax.Where(a => a.tbl_referneced_id == item1.id && a.reference_type == 2).FirstOrDefault();
                                                        if (get_expdetails != null)
                                                        {
                                                            string nam = get_expdetails.return_fee;
                                                            if (nam == "FBA Weight Handling Fee")
                                                            {
                                                                if (get_expdetails.id == exp_id)
                                                                {
                                                                    FBAFEE = FBAFEE + Convert.ToDouble(item1.expense_amount);
                                                                    view_salereport.FBAFEE = FBAFEE;
                                                                    if (gettax_details != null)
                                                                    {
                                                                        FBACGST = FBACGST + Convert.ToDouble(gettax_details.CGST_amount);
                                                                        FBASGST = FBASGST + Convert.ToDouble(gettax_details.sgst_amount);
                                                                        view_salereport.FBACGST = FBACGST;
                                                                        view_salereport.FBASGST = FBASGST;
                                                                    }
                                                                }
                                                            }
                                                            else if (nam == "Technology Fee")
                                                            {
                                                                if (get_expdetails.id == exp_id)
                                                                {
                                                                    TechnologyFee = TechnologyFee + Convert.ToDouble(item1.expense_amount);
                                                                    view_salereport.TechnologyFee = TechnologyFee;
                                                                    if (gettax_details != null)
                                                                    {
                                                                        TechnologyIGST = TechnologyIGST + Convert.ToDouble(gettax_details.Igst_amount);
                                                                        TechnologyCGST = TechnologyCGST + Convert.ToDouble(gettax_details.CGST_amount);
                                                                        TechnologySGST = TechnologySGST + Convert.ToDouble(gettax_details.sgst_amount);

                                                                        view_salereport.TechnologyIGST = TechnologyIGST;
                                                                        view_salereport.TechnologyCGST = TechnologyCGST;
                                                                        view_salereport.TechnologySGST = TechnologySGST;
                                                                    }
                                                                }
                                                            }
                                                            else if (nam == "Commission")
                                                            {
                                                                if (get_expdetails.id == exp_id)
                                                                {
                                                                    CommissionFee = CommissionFee + Convert.ToDouble(item1.expense_amount);
                                                                    view_salereport.CommissionFee = CommissionFee;
                                                                    if (gettax_details != null)
                                                                    {
                                                                        CommissionIGST = CommissionIGST + Convert.ToDouble(gettax_details.Igst_amount);
                                                                        CommissionCGST = CommissionCGST + Convert.ToDouble(gettax_details.CGST_amount);
                                                                        CommissionSGST = CommissionSGST + Convert.ToDouble(gettax_details.sgst_amount);
                                                                        view_salereport.CommissionIGST = CommissionIGST;
                                                                        view_salereport.CommissionCGST = CommissionCGST;
                                                                        view_salereport.CommissionSGST = CommissionSGST;
                                                                    }
                                                                }
                                                            }
                                                            else if (nam == "Fixed closing fee")
                                                            {
                                                                if (get_expdetails.id == exp_id)
                                                                {
                                                                    FixedClosingFee = FixedClosingFee + Convert.ToDouble(item1.expense_amount);
                                                                    view_salereport.FixedClosingFee = FixedClosingFee;
                                                                    if (gettax_details != null)
                                                                    {
                                                                        FixedclosingIGST = FixedclosingIGST + Convert.ToDouble(gettax_details.Igst_amount);
                                                                        FixedclosingCGST = FixedclosingCGST + Convert.ToDouble(gettax_details.CGST_amount);
                                                                        FixedclosingSGST = FixedclosingSGST + Convert.ToDouble(gettax_details.sgst_amount);

                                                                        view_salereport.FixedclosingIGST = FixedclosingIGST;
                                                                        view_salereport.FixedclosingCGST = FixedclosingCGST;
                                                                        view_salereport.FixedclosingSGST = FixedclosingSGST;
                                                                    }
                                                                }
                                                            }
                                                            else if (nam == "Shipping Chargeback")
                                                            {
                                                                if (get_expdetails.id == exp_id)
                                                                {
                                                                    ShippingChargebackFee = ShippingChargebackFee + Convert.ToDouble(item1.expense_amount);
                                                                    view_salereport.ShippingChargebackFee = ShippingChargebackFee;
                                                                    if (gettax_details != null)
                                                                    {
                                                                        shippingchargeCGST = shippingchargeCGST + Convert.ToDouble(gettax_details.CGST_amount);
                                                                        shippingchargeSGST = shippingchargeSGST + Convert.ToDouble(gettax_details.sgst_amount);
                                                                        view_salereport.shippingchargeCGST = shippingchargeCGST;
                                                                        view_salereport.shippingchargeSGST = shippingchargeSGST;
                                                                    }
                                                                }
                                                            }
                                                            else if (nam == "Shipping discount")
                                                            {
                                                                if (get_expdetails.id == exp_id)
                                                                {
                                                                    ShippingDiscountFee = ShippingDiscountFee + Convert.ToDouble(item1.expense_amount);
                                                                    view_salereport.ShippingDiscountFee = ShippingDiscountFee;
                                                                    if (gettax_details != null)
                                                                    {
                                                                        Shippingtaxdiscount = Shippingtaxdiscount + Convert.ToDouble(gettax_details.Igst_amount);
                                                                        view_salereport.Shippingtaxdiscount = Shippingtaxdiscount;
                                                                    }
                                                                }
                                                            }
                                                            else if (nam == "Refund commission")
                                                            {
                                                                if (get_expdetails.id == exp_id)
                                                                {
                                                                    RefundCommision = RefundCommision + Convert.ToDouble(item1.expense_amount);
                                                                    view_salereport.RefundCommision = RefundCommision;
                                                                    if (gettax_details != null)
                                                                    {
                                                                        RefundDiscount = RefundDiscount + Convert.ToDouble(gettax_details.Igst_amount);
                                                                        view_salereport.RefundDiscount = RefundDiscount;
                                                                    }
                                                                }
                                                            }

                                                            else if (nam == "Shipping commission")
                                                            {
                                                                if (get_expdetails.id == exp_id)
                                                                {
                                                                    ShippingCommision = ShippingCommision + Convert.ToDouble(item1.expense_amount);
                                                                    view_salereport.ShippingCommision = ShippingCommision;
                                                                    if (gettax_details != null)
                                                                    {
                                                                        ShippingCommissionIGST = ShippingCommissionIGST + Convert.ToDouble(gettax_details.Igst_amount);
                                                                        view_salereport.ShippingCommissionIGST = ShippingCommissionIGST;
                                                                    }
                                                                }
                                                            }
                                                            else if (nam == "Easy Ship weight handling fees")
                                                            {
                                                                if (get_expdetails.id == exp_id)
                                                                {
                                                                    EasyShipweighthandlingfees = EasyShipweighthandlingfees + Convert.ToDouble(item1.expense_amount);
                                                                    view_salereport.EasyShipweighthandlingfees = EasyShipweighthandlingfees;
                                                                    if (gettax_details != null)
                                                                    {
                                                                        EasyShipweighthandlingfeesIGST = EasyShipweighthandlingfeesIGST + Convert.ToDouble(gettax_details.Igst_amount);
                                                                        view_salereport.EasyShipweighthandlingfeesIGST = EasyShipweighthandlingfeesIGST;
                                                                    }
                                                                }
                                                            }

                                                            else if (nam == "FBA Pick & Pack Fee")
                                                            {
                                                                if (get_expdetails.id == exp_id)
                                                                {
                                                                    FBAPickPackFee = FBAPickPackFee + Convert.ToDouble(item1.expense_amount);
                                                                    view_salereport.FBAPickPackFee = FBAPickPackFee;
                                                                    if (gettax_details != null)
                                                                    {
                                                                        FBAPickPackFeeCGST = FBAPickPackFeeCGST + Convert.ToDouble(gettax_details.CGST_amount);
                                                                        FBAPickPackFeeSGST = FBAPickPackFeeSGST + Convert.ToDouble(gettax_details.sgst_amount);
                                                                        view_salereport.FBAPickPackFeeCGST = FBAPickPackFeeCGST;
                                                                        view_salereport.FBAPickPackFeeSGST = FBAPickPackFeeSGST;
                                                                    }
                                                                }
                                                            }

                                                            else if (nam == "Gift Wrap Chargeback")
                                                            {
                                                                if (get_expdetails.id == exp_id)
                                                                {
                                                                    GiftWrapChargeback = GiftWrapChargeback + Convert.ToDouble(item1.expense_amount);
                                                                    view_salereport.GiftWrapChargeback = GiftWrapChargeback;
                                                                    if (gettax_details != null)
                                                                    {
                                                                        GiftWrapChargebackCGST = GiftWrapChargebackCGST + Convert.ToDouble(gettax_details.CGST_amount);
                                                                        GiftWrapChargebackSGST = GiftWrapChargebackSGST + Convert.ToDouble(gettax_details.sgst_amount);
                                                                        view_salereport.GiftWrapChargebackCGST = GiftWrapChargebackCGST;
                                                                        view_salereport.GiftWrapChargebackSGST = GiftWrapChargebackSGST;
                                                                    }
                                                                }
                                                            }
                                                            else if (nam == "Amazon Easy Ship Charges")
                                                            {
                                                                if (get_expdetails.id == exp_id)
                                                                {
                                                                    AmazonEasyShipCharges = AmazonEasyShipCharges + Convert.ToDouble(item1.expense_amount);
                                                                    view_salereport.AmazonEasyShipCharges = AmazonEasyShipCharges;
                                                                    if (gettax_details != null)
                                                                    {
                                                                        AmazonEasyShipChargesIGST = AmazonEasyShipChargesIGST + Convert.ToDouble(gettax_details.Igst_amount);
                                                                        view_salereport.AmazonEasyShipChargesIGST = AmazonEasyShipChargesIGST;
                                                                    }
                                                                }
                                                            }
                                                        }// end of if(get_expdetails)
                                                    }// end if foreach(item1)
                                                }// end of if(getsettlementdetails) 
                                            }
                                        }// end of if (compare sku and orderid)
                                    }
                                    #endregion

                                    #region to get history table with expense
                                    if (get_history_data != null)
                                    {
                                        foreach (var history in get_history_data)
                                        {
                                            string sku_no = history.history_sku.ToLower();
                                            if (amazon_order_id == history.history_OrderID && details_sku.ToLower() == sku_no)
                                            {
                                                view_salereport.refundprincipal = Convert.ToDouble(history.history_amount_per_unit);
                                                view_salereport.refundproduct_tax = Convert.ToDouble(history.history_product_tax);
                                                view_salereport.refundshipping = Convert.ToDouble(history.history_shipping_price);
                                                view_salereport.refundshipping_tax = Convert.ToDouble(history.history_shipping_tax);
                                                view_salereport.refundgiftwrap = Convert.ToDouble(history.history_Giftwrap_price);
                                                view_salereport.refundgiftwrap_tax = Convert.ToDouble(history.history_gift_wrap_tax);
                                                view_salereport.refundshipping_discount = Convert.ToDouble(history.history_shipping_discount);
                                                view_salereport.refundshipping_discount_tax = Convert.ToDouble(history.history_shipping_tax_discount);

                                                view_salereport.refundTotal = view_salereport.refundprincipal + view_salereport.refundproduct_tax + view_salereport.refundshipping + view_salereport.refundshipping_tax + view_salereport.refundgiftwrap + view_salereport.refundgiftwrap_tax + view_salereport.refundshipping_discount + view_salereport.refundshipping_discount_tax;

                                                view_salereport.refundReferenceID = history.history_settlement_id;

                                                double refundFBAFEE = 0, refundFBACGST = 0, refundFBASGST = 0, refundTechnologyFee = 0, refundTechnologyIGST = 0, refundTechnologyCGST = 0, refundTechnologySGST = 0, refundCommissionFee = 0, refundCommissionIGST = 0, refundCommissionCGST = 0,
                                                       refundCommissionSGST = 0, refundFixedClosingFee = 0, refundFixedclosingIGST = 0,
                                                       refundFixedclosingCGST = 0, refundFixedclosingSGST = 0, refundShippingChargebackFee = 0, refundEasyShipweighthandlingfees = 0,
                                                       refundshippingchargeCGST = 0, refundshippingchargeSGST = 0, refundShippingDiscountFee = 0, refundShippingtaxdiscount = 0,
                                                       refund_RefundCommision = 0, refund_Refund_Discount = 0, refund_Discount_cgst = 0, refund_discount_sgst = 0, refundShippingCommision = 0, refundShippingCommissionIGST = 0, refundEasyShipweighthandlingfeesIGST = 0,
                                                       refundFBAPickPackFee = 0, refundFBAPickPackFeeCGST = 0, refundFBAPickPackFeeSGST = 0, refundGiftWrapChargeback = 0, refundGiftWrapChargebackCGST = 0,
                                                       refundGiftWrapChargebackSGST = 0, refundAmazonEasyShipCharges = 0, refundAmazonEasyShipChargesIGST = 0;


                                                var get_refundexpense = dba.m_tbl_expense.Where(a => a.settlement_order_id.Contains(amazon_order_id) && a.sku_no.ToLower() == sku_no && a.tbl_seller_id == sellers_id && a.t_transactionType_id == 2).ToList();// to get refund expense
                                                if (get_refundexpense != null && get_refundexpense.Count > 0)
                                                {
                                                    foreach (var refund in get_refundexpense)
                                                    {
                                                        var exp_ID = refund.expense_type_id;
                                                        var get_details = dba.m_settlement_fee.Where(a => a.id == exp_ID).FirstOrDefault();
                                                        var getExp_tax_details = dba.tbl_tax.Where(a => a.tbl_referneced_id == refund.id && a.reference_type == 7).FirstOrDefault();
                                                        if (get_details != null)
                                                        {
                                                            string nam = get_details.return_fee;
                                                            if (nam == "FBA Weight Handling Fee")
                                                            {
                                                                if (get_details.id == exp_ID)
                                                                {
                                                                    refundFBAFEE = refundFBAFEE + Convert.ToDouble(refund.expense_amount);
                                                                    view_salereport.RefundFBAFEE = refundFBAFEE;
                                                                    if (getExp_tax_details != null)
                                                                    {
                                                                        refundFBACGST = refundFBACGST + Convert.ToDouble(getExp_tax_details.CGST_amount);
                                                                        refundFBASGST = refundFBASGST + Convert.ToDouble(getExp_tax_details.sgst_amount);

                                                                        view_salereport.RefundFBACGST = refundFBACGST;
                                                                        view_salereport.RefundFBASGST = refundFBASGST;
                                                                    }
                                                                }
                                                            }
                                                            else if (nam == "Technology Fee")
                                                            {
                                                                if (get_details.id == exp_ID)
                                                                {
                                                                    refundTechnologyFee = refundTechnologyFee + Convert.ToDouble(refund.expense_amount);
                                                                    view_salereport.RefundTechnologyFee = refundTechnologyFee;
                                                                    if (getExp_tax_details != null)
                                                                    {
                                                                        refundTechnologyIGST = refundTechnologyIGST + Convert.ToDouble(getExp_tax_details.Igst_amount);
                                                                        refundTechnologyCGST = refundTechnologyCGST + Convert.ToDouble(getExp_tax_details.CGST_amount);
                                                                        refundTechnologySGST = refundTechnologySGST + Convert.ToDouble(getExp_tax_details.sgst_amount);

                                                                        view_salereport.RefundTechnologyIGST = refundTechnologyIGST;
                                                                        view_salereport.RefundTechnologyCGST = refundTechnologyCGST;
                                                                        view_salereport.RefundTechnologySGST = refundTechnologySGST;
                                                                    }
                                                                }
                                                            }
                                                            else if (nam == "Commission")
                                                            {
                                                                if (get_details.id == exp_ID)
                                                                {
                                                                    refundCommissionFee = refundCommissionFee + Convert.ToDouble(refund.expense_amount);
                                                                    view_salereport.RefundCommissionFee = refundCommissionFee;
                                                                    if (getExp_tax_details != null)
                                                                    {
                                                                        refundCommissionIGST = refundCommissionIGST + Convert.ToDouble(getExp_tax_details.Igst_amount);
                                                                        refundCommissionCGST = refundCommissionCGST + Convert.ToDouble(getExp_tax_details.CGST_amount);
                                                                        refundCommissionSGST = refundCommissionSGST + Convert.ToDouble(getExp_tax_details.sgst_amount);

                                                                        view_salereport.RefundCommissionIGST = refundCommissionIGST;
                                                                        view_salereport.RefundCommissionCGST = refundCommissionCGST;
                                                                        view_salereport.RefundCommissionSGST = refundCommissionSGST;
                                                                    }
                                                                }
                                                            }
                                                            else if (nam == "Fixed closing fee")
                                                            {
                                                                if (get_details.id == exp_ID)
                                                                {
                                                                    refundFixedClosingFee = refundFixedClosingFee + Convert.ToDouble(refund.expense_amount);
                                                                    view_salereport.RefundFixedClosingFee = refundFixedClosingFee;
                                                                    if (getExp_tax_details != null)
                                                                    {
                                                                        refundFixedclosingIGST = refundFixedclosingIGST + Convert.ToDouble(getExp_tax_details.Igst_amount);
                                                                        refundFixedclosingCGST = refundFixedclosingCGST + Convert.ToDouble(getExp_tax_details.CGST_amount);
                                                                        refundFixedclosingSGST = refundFixedclosingSGST + Convert.ToDouble(getExp_tax_details.sgst_amount);

                                                                        view_salereport.RefundFixedclosingIGST = refundFixedclosingIGST;
                                                                        view_salereport.RefundFixedclosingCGST = refundFixedclosingCGST;
                                                                        view_salereport.RefundFixedclosingSGST = refundFixedclosingSGST;
                                                                    }
                                                                }
                                                            }
                                                            else if (nam == "Shipping Chargeback")
                                                            {
                                                                if (get_details.id == exp_ID)
                                                                {
                                                                    refundShippingChargebackFee = refundShippingChargebackFee + Convert.ToDouble(refund.expense_amount);
                                                                    view_salereport.RefundShippingChargebackFee = refundShippingChargebackFee;
                                                                    if (getExp_tax_details != null)
                                                                    {
                                                                        refundshippingchargeCGST = refundshippingchargeCGST + Convert.ToDouble(getExp_tax_details.CGST_amount);
                                                                        refundshippingchargeSGST = refundshippingchargeSGST + Convert.ToDouble(getExp_tax_details.sgst_amount);
                                                                        view_salereport.RefundshippingchargeCGST = refundshippingchargeCGST;
                                                                        view_salereport.RefundshippingchargeSGST = refundshippingchargeSGST;
                                                                    }
                                                                }
                                                            }
                                                            else if (nam == "Shipping discount")
                                                            {
                                                                if (get_details.id == exp_ID)
                                                                {
                                                                    refundShippingDiscountFee = refundShippingDiscountFee + Convert.ToDouble(refund.expense_amount);
                                                                    view_salereport.RefundShippingDiscountFee = refundShippingDiscountFee;
                                                                    if (getExp_tax_details != null)
                                                                    {
                                                                        refundShippingtaxdiscount = refundShippingtaxdiscount + Convert.ToDouble(getExp_tax_details.Igst_amount);
                                                                        view_salereport.RefundShippingtaxdiscount = refundShippingtaxdiscount;
                                                                    }
                                                                }
                                                            }
                                                            else if (nam == "Refund commission")
                                                            {
                                                                if (get_details.id == exp_ID)
                                                                {
                                                                    refund_RefundCommision = refund_RefundCommision + Convert.ToDouble(refund.expense_amount);
                                                                    view_salereport.Refund_Commision = refund_RefundCommision;
                                                                    if (getExp_tax_details != null)
                                                                    {
                                                                        refund_Refund_Discount = refund_Refund_Discount + Convert.ToDouble(getExp_tax_details.Igst_amount);
                                                                        refund_Discount_cgst = refund_Discount_cgst + Convert.ToDouble(getExp_tax_details.CGST_amount);
                                                                        refund_discount_sgst = refund_discount_sgst + Convert.ToDouble(getExp_tax_details.sgst_amount);

                                                                        view_salereport.Refund_Discount = refund_Refund_Discount;
                                                                        view_salereport.Refund_DiscountCGST = refund_Discount_cgst;
                                                                        view_salereport.Refund_DiscountSGST = refund_discount_sgst;
                                                                    }
                                                                }
                                                            }

                                                            else if (nam == "Shipping commission")
                                                            {
                                                                if (get_details.id == exp_ID)
                                                                {
                                                                    refundShippingCommision = refundShippingCommision + Convert.ToDouble(refund.expense_amount);
                                                                    view_salereport.Refund_ShippingCommision = refundShippingCommision;
                                                                    if (getExp_tax_details != null)
                                                                    {
                                                                        refundShippingCommissionIGST = refundShippingCommissionIGST + Convert.ToDouble(getExp_tax_details.Igst_amount);
                                                                        view_salereport.Refund_Shipping_Commission = refundShippingCommissionIGST;
                                                                    }
                                                                }
                                                            }
                                                            else if (nam == "Easy Ship weight handling fees")
                                                            {
                                                                if (get_details.id == exp_ID)
                                                                {
                                                                    refundEasyShipweighthandlingfees = refundEasyShipweighthandlingfees + Convert.ToDouble(refund.expense_amount);
                                                                    view_salereport.Refund_EasyShipweighthandlingfees = refundEasyShipweighthandlingfees;
                                                                    if (getExp_tax_details != null)
                                                                    {
                                                                        refundEasyShipweighthandlingfeesIGST = refundEasyShipweighthandlingfeesIGST + Convert.ToDouble(getExp_tax_details.Igst_amount);
                                                                        view_salereport.Refund_EasyShipweighthandlingfeesIGST = refundEasyShipweighthandlingfeesIGST;
                                                                    }
                                                                }
                                                            }

                                                            else if (nam == "FBA Pick & Pack Fee")
                                                            {
                                                                if (get_details.id == exp_ID)
                                                                {
                                                                    refundFBAPickPackFee = refundFBAPickPackFee + Convert.ToDouble(refund.expense_amount);
                                                                    view_salereport.Refund_FBAPick_PackFee = refundFBAPickPackFee;
                                                                    if (getExp_tax_details != null)
                                                                    {
                                                                        refundFBAPickPackFeeCGST = refundFBAPickPackFeeCGST + Convert.ToDouble(getExp_tax_details.CGST_amount);
                                                                        refundFBAPickPackFeeSGST = refundFBAPickPackFeeSGST + Convert.ToDouble(getExp_tax_details.sgst_amount);
                                                                        view_salereport.RefundFBAPick_PackFeeCGST = refundFBAPickPackFeeCGST;
                                                                        view_salereport.RefundFBAPick_PackFeeSGST = refundFBAPickPackFeeSGST;
                                                                    }
                                                                }
                                                            }
                                                            else if (nam == "Gift Wrap Chargeback")
                                                            {
                                                                if (get_details.id == exp_ID)
                                                                {
                                                                    refundGiftWrapChargeback = refundGiftWrapChargeback + Convert.ToDouble(refund.expense_amount);
                                                                    view_salereport.Refund_GiftWrapChargeback = refundGiftWrapChargeback;
                                                                    if (getExp_tax_details != null)
                                                                    {
                                                                        refundGiftWrapChargebackCGST = refundGiftWrapChargebackCGST + Convert.ToDouble(getExp_tax_details.CGST_amount);
                                                                        refundGiftWrapChargebackSGST = refundGiftWrapChargebackSGST + Convert.ToDouble(getExp_tax_details.sgst_amount);
                                                                        view_salereport.RefundGiftWrapChargebackCGST = refundGiftWrapChargebackCGST;
                                                                        view_salereport.RefundGiftWrapChargebackSGST = refundGiftWrapChargebackSGST;
                                                                    }
                                                                }
                                                            }
                                                            else if (nam == "Amazon Easy Ship Charges")
                                                            {
                                                                if (get_details.id == exp_ID)
                                                                {
                                                                    refundAmazonEasyShipCharges = refundAmazonEasyShipCharges + Convert.ToDouble(refund.expense_amount);
                                                                    view_salereport.Refund_AmazonEasyShipCharges = refundAmazonEasyShipCharges;
                                                                    if (getExp_tax_details != null)
                                                                    {
                                                                        refundAmazonEasyShipChargesIGST = refundAmazonEasyShipChargesIGST + Convert.ToDouble(getExp_tax_details.Igst_amount);
                                                                        view_salereport.Refund_AmazonEasyShipChargesIGST = refundAmazonEasyShipChargesIGST;
                                                                    }
                                                                }
                                                            }

                                                        }// end of if(get_details)                               
                                                    }// end of foreach(refund)
                                                }// end of if(get_refundexpense)

                                            }// end of if(compare orderid and sku)                                                                                        
                                        }// end of for each loop(history)
                                    }// end of if (check not null)

                                    #endregion

                                    view_salereport.Refund_SUMIGST = view_salereport.RefundTechnologyIGST + view_salereport.RefundCommissionIGST + view_salereport.RefundFixedclosingIGST + view_salereport.Refund_EasyShipweighthandlingfeesIGST + view_salereport.RefundShippingtaxdiscount + view_salereport.Refund_Discount + view_salereport.Refund_Shipping_Commission + view_salereport.Refund_AmazonEasyShipChargesIGST;
                                    view_salereport.Refund_SUMCGST = view_salereport.RefundshippingchargeCGST + view_salereport.RefundFBACGST + view_salereport.RefundFBAPick_PackFeeCGST + view_salereport.RefundGiftWrapChargebackCGST + view_salereport.Refund_DiscountCGST + view_salereport.RefundFixedclosingCGST + view_salereport.RefundCommissionCGST + view_salereport.RefundTechnologyCGST;
                                    view_salereport.Refund_SUMSGST = view_salereport.RefundshippingchargeSGST + view_salereport.RefundFBASGST + view_salereport.RefundFBAPick_PackFeeSGST + view_salereport.RefundGiftWrapChargebackSGST + view_salereport.Refund_DiscountSGST + view_salereport.RefundFixedclosingSGST + view_salereport.RefundCommissionSGST + view_salereport.RefundTechnologySGST;
                                    view_salereport.refund_SumFee = view_salereport.RefundFBAFEE + view_salereport.RefundTechnologyFee + view_salereport.RefundCommissionFee + view_salereport.RefundFixedClosingFee + view_salereport.RefundShippingChargebackFee + view_salereport.RefundShippingDiscountFee + view_salereport.Refund_EasyShipweighthandlingfees + view_salereport.Refund_Commision + view_salereport.Refund_ShippingCommision + view_salereport.Refund_FBAPick_PackFee + view_salereport.Refund_GiftWrapChargeback + view_salereport.Refund_AmazonEasyShipCharges + view_salereport.Refund_SUMIGST + view_salereport.Refund_SUMCGST + view_salereport.Refund_SUMSGST;
                                    view_salereport.refund_SumOrder = view_salereport.refundTotal + view_salereport.refund_SumFee;

                                    view_salereport.SUMIGST = view_salereport.TechnologyIGST + view_salereport.CommissionIGST + view_salereport.FixedclosingIGST + view_salereport.Shippingtaxdiscount + view_salereport.RefundDiscount + view_salereport.ShippingCommissionIGST + view_salereport.EasyShipweighthandlingfeesIGST + view_salereport.AmazonEasyShipChargesIGST;
                                    view_salereport.SUMCGST = view_salereport.shippingchargeCGST + view_salereport.FBACGST + view_salereport.FBAPickPackFeeCGST + view_salereport.GiftWrapChargebackCGST + view_salereport.FixedclosingCGST + view_salereport.CommissionCGST + view_salereport.TechnologyCGST;
                                    view_salereport.SUMSGST = view_salereport.shippingchargeSGST + view_salereport.FBASGST + view_salereport.FBAPickPackFeeSGST + view_salereport.GiftWrapChargebackSGST + view_salereport.FixedclosingSGST + view_salereport.CommissionSGST + view_salereport.TechnologySGST;
                                    view_salereport.SumFee = view_salereport.FBAFEE + view_salereport.TechnologyFee + view_salereport.CommissionFee + view_salereport.FixedClosingFee + view_salereport.ShippingChargebackFee + view_salereport.ShippingDiscountFee + view_salereport.RefundCommision + view_salereport.ShippingCommision + view_salereport.EasyShipweighthandlingfees + view_salereport.FBAPickPackFee + view_salereport.GiftWrapChargeback + view_salereport.AmazonEasyShipCharges + view_salereport.SUMIGST + view_salereport.SUMCGST + view_salereport.SUMSGST;
                                    view_salereport.SumOrder = view_salereport.orderTotal + view_salereport.SumFee;
                                    view_salereport.NetTotal = view_salereport.SumOrder + view_salereport.refund_SumOrder;

                                    string value = "";
                                    value = Convert.ToString((view_salereport.NetTotal / view_salereport.orderTotal) * 100);
                                    if (value != "NaN" && value != "-Infinity")
                                    {
                                        decimal abc = Convert.ToDecimal((view_salereport.NetTotal / view_salereport.orderTotal) * 100);
                                        decimal result = decimal.Round(abc, 2, MidpointRounding.AwayFromZero);

                                        string bb = value.ToString();
                                        string[] b = bb.Split('.');
                                        int firstValue = int.Parse(b[0]);

                                        string fvalue = result + "%";
                                        view_salereport.PercentageAmount = Convert.ToString(fvalue);
                                    }
                                    if (PercentageFilter(ddl_percentage, view_salereport.PercentageAmount))
                                        lstOrdertext2.Add(view_salereport);

                                    // lstOrdertext2.Add(view_salereport);
                                }// end of for (counter)


                            }//end of if(get_settlement_data)
                        }
                    }// end of using connection
                }// end of if (txt from )
            }// end of try block
            catch (Exception ex)
            {
            }
            return Msg;
        }


        /// <summary>
        /// this is for fetching records related to flipkart marketplace
        /// </summary>
        /// <param name="lstOrdertext2"></param>
        /// <param name="view_salereport"></param>
        /// <param name="txt_from"></param>
        /// <param name="txt_to"></param>
        /// <param name="sellers_id"></param>
        /// <param name="ddl_market_place"></param>
        /// <param name="ddl_percentage"></param>
        /// <returns></returns>
        public string Get_SalesReport_Flipkart(List<SaleReport> lstOrdertext2, SaleReport view_salereport, DateTime? txt_from, DateTime? txt_to, int sellers_id, int? ddl_market_place, int? ddl_percentage)
        {
            string Msg = "S";
            try
            {
                if (txt_from != null && txt_to != null)
                {
                    using (var connection = dba.Database.Connection)
                    {
                        connection.Open();
                        var command = connection.CreateCommand();
                        command.CommandText = "get_settlement_report"; // "get_sett_orders";
                        command.CommandType = CommandType.StoredProcedure;

                        MySql.Data.MySqlClient.MySqlParameter param, param1, param2, param3;
                        param = new MySql.Data.MySqlClient.MySqlParameter("@sellerid", sellers_id);
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.Int32;
                        command.Parameters.Add(param);

                        param1 = new MySql.Data.MySqlClient.MySqlParameter("@datefrom", txt_from);
                        param1.Direction = ParameterDirection.Input;
                        param1.DbType = DbType.DateTime;
                        command.Parameters.Add(param1);

                        param2 = new MySql.Data.MySqlClient.MySqlParameter("@dateto", txt_to);
                        param2.Direction = ParameterDirection.Input;
                        param2.DbType = DbType.DateTime;
                        command.Parameters.Add(param2);

                        param3 = new MySql.Data.MySqlClient.MySqlParameter("@marketplaceid", ddl_market_place);
                        param3.Direction = ParameterDirection.Input;
                        param3.DbType = DbType.Int32;
                        command.Parameters.Add(param3);


                        using (var reader = command.ExecuteReader())
                        {
                            var get_settlement_data =
                               ((System.Data.Entity.Infrastructure.IObjectContextAdapter)dba)
                                   .ObjectContext
                                   .Translate<proc_Settlement_report>(reader)
                                   .ToList();

                            reader.NextResult();

                            var get_settlement_data1 =
                                ((System.Data.Entity.Infrastructure.IObjectContextAdapter)dba)
                                    .ObjectContext
                                    .Translate<proc_Settlement_report>(reader)
                                    .ToList();
                            reader.NextResult();

                            var get_settlement_data2 =
                                ((System.Data.Entity.Infrastructure.IObjectContextAdapter)dba)
                                    .ObjectContext
                                    .Translate<proc_Settlement_report>(reader)
                                    .ToList();
                            reader.NextResult();

                            var get_history_data =
                                ((System.Data.Entity.Infrastructure.IObjectContextAdapter)dba)
                                    .ObjectContext
                                    .Translate<proc_Settlement_report>(reader)
                                    .ToList();

                            connection.Close();
                            dba = new SellerContext();
                            dba.Configuration.AutoDetectChangesEnabled = false;

                            if (get_settlement_data != null)
                            {
                                for (int maincounter = 0; maincounter < get_settlement_data.Count; maincounter++)
                                {
                                    view_salereport = new SaleReport();
                                    var sett = get_settlement_data[maincounter];
                                    string amazon_order_id = sett.amazon_order_id;
                                    string details_sku = sett.sku_no;
                                    string item_order_id = sett.order_item_id;
                                    int orderdetails_id = sett.orderdetailsid;
                                    view_salereport.OrderID = sett.amazon_order_id;
                                    view_salereport.OrderDate = Convert.ToDateTime(sett.purchase_date).ToString("yyyy-MM-dd");

                                    double Principal = 0, Shipping = 0, orderigst = 0, ordersgst = 0, ordercgst = 0, shippingtax = 0, Giftwrap = 0, itempromotion = 0, shippingpromotion = 0;

                                    view_salereport.ProductName = sett.product_name;
                                    view_salereport.skuNo = sett.sku_no;

                                    Principal = Convert.ToDouble(sett.item_price_amount);
                                    Shipping = Convert.ToDouble(sett.shipping_price_Amount);
                                    Giftwrap = Convert.ToDouble(sett.giftwrapprice_amount);
                                    shippingtax = Convert.ToDouble(sett.shipping_tax_Amount);
                                    itempromotion = Convert.ToDouble(sett.item_promotionAmount);
                                    shippingpromotion = Convert.ToDouble(sett.promotion_amount);

                                    view_salereport.Principal = Principal + Shipping + Giftwrap - itempromotion - shippingpromotion;

                                    if (get_settlement_data1 != null)
                                    {
                                        foreach (var sett5 in get_settlement_data1)// use for tax data
                                        {
                                            int refer_id = Convert.ToInt16(sett5.tbl_referneced_id);
                                            if (orderdetails_id == refer_id)
                                            {
                                                orderigst = Convert.ToDouble(sett5.Igst_amount);
                                                ordersgst = Convert.ToDouble(sett5.sgst_amount);
                                                ordercgst = Convert.ToDouble(sett5.CGST_amount);

                                                view_salereport.orderigst = orderigst;
                                                view_salereport.ordersgst = ordersgst;
                                                view_salereport.ordercgst = ordercgst;
                                            }
                                        }
                                    }// end of if(get_settlement_data1)

                                    view_salereport.ordertotal = view_salereport.Principal + view_salereport.orderigst + view_salereport.ordersgst + view_salereport.ordercgst;

                                    #region get settlement details with expense and tax

                                    if (get_settlement_data2 != null)
                                    {
                                        foreach (var settle in get_settlement_data2)
                                        {
                                            string skuno = settle.Settlement_sku.ToLower();
                                            if (amazon_order_id == settle.Order_Id && item_order_id == skuno)
                                            {
                                                view_salereport.orderprincipal = Convert.ToDouble(settle.principal_price);
                                                view_salereport.orderproduct_tax = Convert.ToDouble(settle.product_tax);
                                                view_salereport.ordershipping = Convert.ToDouble(settle.shipping_price);
                                                view_salereport.ordershipping_tax = Convert.ToDouble(settle.shipping_tax);
                                                view_salereport.ordergiftwrap = Convert.ToDouble(settle.giftwrap_price);
                                                view_salereport.ordergiftwrap_tax = Convert.ToDouble(settle.giftwarp_tax);
                                                view_salereport.ordershipping_discount = Convert.ToDouble(settle.shipping_discount);
                                                view_salereport.ordershipping_discounttax = Convert.ToDouble(settle.shipping_tax_discount);

                                                view_salereport.orderTotal = view_salereport.orderprincipal + view_salereport.orderproduct_tax + view_salereport.ordershipping + view_salereport.ordershipping_tax + view_salereport.ordergiftwrap + view_salereport.ordergiftwrap_tax + view_salereport.ordershipping_discount + view_salereport.ordershipping_discounttax;
                                                view_salereport.ReferenceID = settle.settlement_id;

                                                //-------------------------End---------------------------//
                                                //-----------------------Get  data from tbl_Expense from settlement_Id----------------//

                                                double flipShipping = 0, flipShippingFeeIGST = 0, flipShippingFeeCGST = 0, flipShippingFeeSGST = 0, flipCollection = 0, flipCollectionIGST = 0, flipCollectionCGST = 0, flipCollectionSGST = 0,
                                                       flipReverseShipping = 0, flipReverseShippingIGST = 0, flipReverseShippingCGST = 0, flipReverseShippingSGST = 0, flipFixedFee = 0, flipFixedFeeIGST = 0,
                                                       flipFixedFeeCGST = 0, flipFixedFeeSGST = 0, CommissionFee = 0, CommissionIGST = 0, CommissionCGST = 0, CommissionSGST = 0;


                                                var getsettlementdetails = dba.m_tbl_expense.Where(a => a.settlement_order_id.Contains(amazon_order_id) && a.sku_no.ToLower() == skuno && a.tbl_seller_id == sellers_id && a.t_transactionType_id == 1).ToList();
                                                if (getsettlementdetails != null && getsettlementdetails.Count > 0)
                                                {
                                                    foreach (var item1 in getsettlementdetails)
                                                    {
                                                        var exp_id = item1.expense_type_id;
                                                        var get_expdetails = dba.m_settlement_fee.Where(a => a.id == exp_id).FirstOrDefault();
                                                        var gettax_details = dba.tbl_tax.Where(a => a.tbl_referneced_id == item1.id && a.reference_type == 2).FirstOrDefault();
                                                        if (get_expdetails != null)
                                                        {
                                                            string nam = get_expdetails.return_fee;
                                                            if (nam == "Shipping Fee")
                                                            {
                                                                if (get_expdetails.id == exp_id)
                                                                {
                                                                    flipShipping = flipShipping + Convert.ToDouble(item1.expense_amount);
                                                                    view_salereport.flipShipping = flipShipping;
                                                                    if (gettax_details != null)
                                                                    {
                                                                        flipShippingFeeIGST = flipShippingFeeIGST + Convert.ToDouble(gettax_details.Igst_amount);
                                                                        flipShippingFeeCGST = flipShippingFeeCGST + Convert.ToDouble(gettax_details.CGST_amount);
                                                                        flipShippingFeeSGST = flipShippingFeeSGST + Convert.ToDouble(gettax_details.sgst_amount);

                                                                        view_salereport.flipShippingFeeIGST = flipShippingFeeIGST;
                                                                        view_salereport.flipShippingFeeCGST = flipShippingFeeCGST;
                                                                        view_salereport.flipShippingFeeSGST = flipShippingFeeSGST;
                                                                    }
                                                                }
                                                            }
                                                            else if (nam == "Collection Fee")
                                                            {
                                                                if (get_expdetails.id == exp_id)
                                                                {
                                                                    flipCollection = flipCollection + Convert.ToDouble(item1.expense_amount);
                                                                    view_salereport.flipCollection = flipCollection;
                                                                    if (gettax_details != null)
                                                                    {
                                                                        flipCollectionIGST = flipCollectionIGST + Convert.ToDouble(gettax_details.Igst_amount);
                                                                        flipCollectionCGST = flipCollectionCGST + Convert.ToDouble(gettax_details.CGST_amount);
                                                                        flipCollectionSGST = flipCollectionSGST + Convert.ToDouble(gettax_details.sgst_amount);

                                                                        view_salereport.flipCollectionIGST = flipCollectionIGST;
                                                                        view_salereport.flipCollectionCGST = flipCollectionCGST;
                                                                        view_salereport.flipCollectionSGST = flipCollectionSGST;
                                                                    }
                                                                }
                                                            }
                                                            else if (nam == "Reverse Shipping Fee")
                                                            {
                                                                if (get_expdetails.id == exp_id)
                                                                {
                                                                    flipReverseShipping = flipReverseShipping + Convert.ToDouble(item1.expense_amount);
                                                                    view_salereport.flipReverseShipping = flipReverseShipping;
                                                                    if (gettax_details != null)
                                                                    {
                                                                        flipReverseShippingIGST = flipReverseShippingIGST + Convert.ToDouble(gettax_details.Igst_amount);
                                                                        flipReverseShippingCGST = flipReverseShippingCGST + Convert.ToDouble(gettax_details.CGST_amount);
                                                                        flipReverseShippingSGST = flipReverseShippingSGST + Convert.ToDouble(gettax_details.sgst_amount);
                                                                        view_salereport.flipReverseShippingIGST = flipReverseShippingIGST;
                                                                        view_salereport.flipReverseShippingCGST = flipReverseShippingCGST;
                                                                        view_salereport.flipReverseShippingSGST = flipReverseShippingSGST;
                                                                    }
                                                                }
                                                            }
                                                            else if (nam == "Fixed Fee")
                                                            {
                                                                if (get_expdetails.id == exp_id)
                                                                {
                                                                    flipFixedFee = flipFixedFee + Convert.ToDouble(item1.expense_amount);
                                                                    view_salereport.flipFixedFee = flipFixedFee;
                                                                    if (gettax_details != null)
                                                                    {
                                                                        flipFixedFeeIGST = flipFixedFeeIGST + Convert.ToDouble(gettax_details.Igst_amount);
                                                                        flipFixedFeeCGST = flipFixedFeeCGST + Convert.ToDouble(gettax_details.CGST_amount);
                                                                        flipFixedFeeSGST = flipFixedFeeSGST + Convert.ToDouble(gettax_details.sgst_amount);

                                                                        view_salereport.flipFixedFeeIGST = flipFixedFeeIGST;
                                                                        view_salereport.flipFixedFeeCGST = flipFixedFeeCGST;
                                                                        view_salereport.flipFixedFeeSGST = flipFixedFeeSGST;
                                                                    }
                                                                }
                                                            }
                                                            else if (nam == "Commission")
                                                            {
                                                                if (get_expdetails.id == exp_id)
                                                                {
                                                                    CommissionFee = CommissionFee + Convert.ToDouble(item1.expense_amount);
                                                                    view_salereport.CommissionFee = CommissionFee;
                                                                    if (gettax_details != null)
                                                                    {
                                                                        CommissionIGST = CommissionIGST + Convert.ToDouble(gettax_details.Igst_amount);
                                                                        CommissionCGST = CommissionCGST + Convert.ToDouble(gettax_details.CGST_amount);
                                                                        CommissionSGST = CommissionSGST + Convert.ToDouble(gettax_details.sgst_amount);
                                                                        view_salereport.CommissionIGST = CommissionIGST;
                                                                        view_salereport.CommissionCGST = CommissionCGST;
                                                                        view_salereport.CommissionSGST = CommissionSGST;
                                                                    }
                                                                }
                                                            }

                                                        }// end of if(get_expdetails)
                                                    }// end if foreach(item1)
                                                }// end of if(getsettlementdetails) 
                                            }
                                        }// end of if (compare sku and orderid)
                                    }
                                    #endregion

                                    #region to get history table with expense
                                    if (get_history_data != null)
                                    {
                                        foreach (var history in get_history_data)
                                        {
                                            string sku_no = history.history_sku.ToLower();
                                            if (amazon_order_id == history.history_OrderID && item_order_id == sku_no)
                                            {
                                                view_salereport.refundprincipal = Convert.ToDouble(history.history_amount_per_unit);
                                                view_salereport.refundproduct_tax = Convert.ToDouble(history.history_product_tax);
                                                view_salereport.refundshipping = Convert.ToDouble(history.history_shipping_price);
                                                view_salereport.refundshipping_tax = Convert.ToDouble(history.history_shipping_tax);
                                                view_salereport.refundgiftwrap = Convert.ToDouble(history.history_Giftwrap_price);
                                                view_salereport.refundgiftwrap_tax = Convert.ToDouble(history.history_gift_wrap_tax);
                                                view_salereport.refundshipping_discount = Convert.ToDouble(history.history_shipping_discount);
                                                view_salereport.refundshipping_discount_tax = Convert.ToDouble(history.history_shipping_tax_discount);

                                                view_salereport.refundTotal = view_salereport.refundprincipal + view_salereport.refundproduct_tax + view_salereport.refundshipping + view_salereport.refundshipping_tax + view_salereport.refundgiftwrap + view_salereport.refundgiftwrap_tax + view_salereport.refundshipping_discount + view_salereport.refundshipping_discount_tax;

                                                view_salereport.refundReferenceID = history.history_settlement_id;

                                                double refund_flipShipping = 0, Refund_flipShippingFeeIGST = 0, Refund_flipShippingFeeCGST = 0, Refund_flipShippingFeeSGST = 0,
                                                       refund_flipCollection = 0, Refund_flipCollectionIGST = 0, Refund_flipCollectionCGST = 0,
                                                       Refund_flipCollectionSGST = 0, refundCommissionFee = 0, refundCommissionIGST = 0, refundCommissionCGST = 0,
                                                       refundCommissionSGST = 0, refund_flipFixedFee = 0, Refund_flipFixedFeeIGST = 0,
                                                       Refund_flipFixedFeeCGST = 0, Refund_flipFixedFeeSGST = 0, refund_flipReverseShipping = 0, Refund_flipReverseShippingIGST = 0, Refund_flipReverseShippingCGST = 0,
                                                       Refund_flipReverseShippingSGST = 0;


                                                var get_refundexpense = dba.m_tbl_expense.Where(a => a.settlement_order_id.Contains(amazon_order_id) && a.sku_no.ToLower() == item_order_id && a.tbl_seller_id == sellers_id && a.t_transactionType_id == 2).ToList();// to get refund expense
                                                if (get_refundexpense != null && get_refundexpense.Count > 0)
                                                {
                                                    foreach (var refund in get_refundexpense)
                                                    {
                                                        var exp_ID = refund.expense_type_id;
                                                        var get_details = dba.m_settlement_fee.Where(a => a.id == exp_ID).FirstOrDefault();
                                                        var getExp_tax_details = dba.tbl_tax.Where(a => a.tbl_referneced_id == refund.id && a.reference_type == 7).FirstOrDefault();
                                                        if (get_details != null)
                                                        {
                                                            string nam = get_details.return_fee;
                                                            if (nam == "Shipping Fee")
                                                            {
                                                                if (get_details.id == exp_ID)
                                                                {
                                                                    refund_flipShipping = refund_flipShipping + Convert.ToDouble(refund.expense_amount);
                                                                    view_salereport.refund_flipShipping = refund_flipShipping;
                                                                    if (getExp_tax_details != null)
                                                                    {
                                                                        Refund_flipShippingFeeIGST = Refund_flipShippingFeeIGST + Convert.ToDouble(getExp_tax_details.Igst_amount);
                                                                        Refund_flipShippingFeeCGST = Refund_flipShippingFeeCGST + Convert.ToDouble(getExp_tax_details.CGST_amount);
                                                                        Refund_flipShippingFeeSGST = Refund_flipShippingFeeSGST + Convert.ToDouble(getExp_tax_details.sgst_amount);

                                                                        view_salereport.Refund_flipShippingFeeIGST = Refund_flipShippingFeeIGST;
                                                                        view_salereport.Refund_flipShippingFeeCGST = Refund_flipShippingFeeCGST;
                                                                        view_salereport.Refund_flipShippingFeeSGST = Refund_flipShippingFeeSGST;
                                                                    }
                                                                }
                                                            }
                                                            else if (nam == "Collection Fee")
                                                            {
                                                                if (get_details.id == exp_ID)
                                                                {
                                                                    refund_flipCollection = refund_flipCollection + Convert.ToDouble(refund.expense_amount);
                                                                    view_salereport.refund_flipCollection = refund_flipCollection;
                                                                    if (getExp_tax_details != null)
                                                                    {
                                                                        Refund_flipCollectionIGST = Refund_flipCollectionIGST + Convert.ToDouble(getExp_tax_details.Igst_amount);
                                                                        Refund_flipCollectionCGST = Refund_flipCollectionCGST + Convert.ToDouble(getExp_tax_details.CGST_amount);
                                                                        Refund_flipCollectionSGST = Refund_flipCollectionSGST + Convert.ToDouble(getExp_tax_details.sgst_amount);

                                                                        view_salereport.Refund_flipCollectionIGST = Refund_flipCollectionIGST;
                                                                        view_salereport.Refund_flipCollectionCGST = Refund_flipCollectionCGST;
                                                                        view_salereport.Refund_flipCollectionSGST = Refund_flipCollectionSGST;
                                                                    }
                                                                }
                                                            }
                                                            else if (nam == "Reverse Shipping Fee")
                                                            {
                                                                if (get_details.id == exp_ID)
                                                                {
                                                                    refund_flipReverseShipping = refund_flipReverseShipping + Convert.ToDouble(refund.expense_amount);
                                                                    view_salereport.refund_flipReverseShipping = refund_flipReverseShipping;
                                                                    if (getExp_tax_details != null)
                                                                    {
                                                                        Refund_flipReverseShippingIGST = Refund_flipReverseShippingIGST + Convert.ToDouble(getExp_tax_details.Igst_amount);
                                                                        Refund_flipReverseShippingCGST = Refund_flipReverseShippingCGST + Convert.ToDouble(getExp_tax_details.CGST_amount);
                                                                        Refund_flipReverseShippingSGST = Refund_flipReverseShippingSGST + Convert.ToDouble(getExp_tax_details.sgst_amount);

                                                                        view_salereport.Refund_flipReverseShippingIGST = Refund_flipReverseShippingIGST;
                                                                        view_salereport.Refund_flipReverseShippingCGST = Refund_flipReverseShippingCGST;
                                                                        view_salereport.Refund_flipReverseShippingSGST = Refund_flipReverseShippingSGST;
                                                                    }
                                                                }
                                                            }
                                                            else if (nam == "Fixed Fee")
                                                            {
                                                                if (get_details.id == exp_ID)
                                                                {
                                                                    refund_flipFixedFee = refund_flipFixedFee + Convert.ToDouble(refund.expense_amount);
                                                                    view_salereport.refund_flipFixedFee = refund_flipFixedFee;
                                                                    if (getExp_tax_details != null)
                                                                    {
                                                                        Refund_flipFixedFeeIGST = Refund_flipFixedFeeIGST + Convert.ToDouble(getExp_tax_details.Igst_amount);
                                                                        Refund_flipFixedFeeCGST = Refund_flipFixedFeeCGST + Convert.ToDouble(getExp_tax_details.CGST_amount);
                                                                        Refund_flipFixedFeeSGST = Refund_flipFixedFeeSGST + Convert.ToDouble(getExp_tax_details.sgst_amount);

                                                                        view_salereport.Refund_flipFixedFeeIGST = Refund_flipFixedFeeIGST;
                                                                        view_salereport.Refund_flipFixedFeeCGST = Refund_flipFixedFeeCGST;
                                                                        view_salereport.Refund_flipFixedFeeSGST = Refund_flipFixedFeeSGST;
                                                                    }
                                                                }
                                                            }
                                                            else if (nam == "Commission")
                                                            {
                                                                if (get_details.id == exp_ID)
                                                                {
                                                                    refundCommissionFee = refundCommissionFee + Convert.ToDouble(refund.expense_amount);
                                                                    view_salereport.RefundCommissionFee = refundCommissionFee;
                                                                    if (getExp_tax_details != null)
                                                                    {
                                                                        refundCommissionIGST = refundCommissionIGST + Convert.ToDouble(getExp_tax_details.Igst_amount);
                                                                        refundCommissionCGST = refundCommissionCGST + Convert.ToDouble(getExp_tax_details.CGST_amount);
                                                                        refundCommissionSGST = refundCommissionSGST + Convert.ToDouble(getExp_tax_details.sgst_amount);

                                                                        view_salereport.RefundCommissionIGST = refundCommissionIGST;
                                                                        view_salereport.RefundCommissionCGST = refundCommissionCGST;
                                                                        view_salereport.RefundCommissionSGST = refundCommissionSGST;
                                                                    }
                                                                }
                                                            }

                                                        }// end of if(get_details)                               
                                                    }// end of foreach(refund)
                                                }// end of if(get_refundexpense)

                                            }// end of if(compare orderid and sku)                                                                                        
                                        }// end of for each loop(history)
                                    }// end of if (check not null)

                                    #endregion

                                    view_salereport.Refund_SUMIGST = view_salereport.Refund_flipShippingFeeIGST + view_salereport.Refund_flipCollectionIGST + view_salereport.Refund_flipReverseShippingIGST + view_salereport.Refund_flipFixedFeeIGST + view_salereport.RefundCommissionIGST;
                                    view_salereport.Refund_SUMCGST = view_salereport.Refund_flipShippingFeeCGST + view_salereport.Refund_flipCollectionCGST + view_salereport.Refund_flipReverseShippingCGST + view_salereport.Refund_flipFixedFeeCGST + view_salereport.RefundCommissionCGST;
                                    view_salereport.Refund_SUMSGST = view_salereport.Refund_flipShippingFeeSGST + view_salereport.Refund_flipCollectionSGST + view_salereport.Refund_flipReverseShippingSGST + view_salereport.Refund_flipFixedFeeSGST + view_salereport.RefundCommissionSGST;
                                    view_salereport.refund_SumFee = view_salereport.refund_flipShipping + view_salereport.refund_flipCollection + view_salereport.refund_flipReverseShipping + view_salereport.refund_flipFixedFee + view_salereport.RefundCommissionFee + view_salereport.Refund_SUMIGST + view_salereport.Refund_SUMCGST + view_salereport.Refund_SUMSGST;
                                    view_salereport.refund_SumOrder = view_salereport.refundTotal + view_salereport.refund_SumFee;

                                    view_salereport.SUMIGST = view_salereport.flipShippingFeeIGST + view_salereport.flipCollectionIGST + view_salereport.flipReverseShippingIGST + view_salereport.flipFixedFeeIGST + view_salereport.CommissionIGST;
                                    view_salereport.SUMCGST = view_salereport.flipShippingFeeCGST + view_salereport.flipCollectionCGST + view_salereport.flipReverseShippingCGST + view_salereport.flipFixedFeeCGST + view_salereport.CommissionCGST;
                                    view_salereport.SUMSGST = view_salereport.flipShippingFeeSGST + view_salereport.flipCollectionSGST + view_salereport.flipReverseShippingSGST + view_salereport.flipFixedFeeSGST + view_salereport.CommissionSGST;
                                    view_salereport.SumFee = view_salereport.flipShipping + view_salereport.flipCollection + view_salereport.flipReverseShipping + view_salereport.flipFixedFee + view_salereport.CommissionFee + view_salereport.SUMIGST + view_salereport.SUMCGST + view_salereport.SUMSGST;

                                    view_salereport.SumOrder = view_salereport.orderTotal + view_salereport.SumFee;
                                    view_salereport.NetTotal = view_salereport.SumOrder + view_salereport.refund_SumOrder;

                                    string value = "";
                                    value = Convert.ToString((view_salereport.NetTotal / view_salereport.orderTotal) * 100);
                                    if (value != "NaN" && value != "-Infinity")
                                    {
                                        decimal abc = Convert.ToDecimal((view_salereport.NetTotal / view_salereport.orderTotal) * 100);
                                        decimal result = decimal.Round(abc, 2, MidpointRounding.AwayFromZero);

                                        string bb = value.ToString();
                                        string[] b = bb.Split('.');
                                        int firstValue = int.Parse(b[0]);

                                        string fvalue = result + "%";
                                        view_salereport.PercentageAmount = Convert.ToString(fvalue);
                                    }
                                    if (PercentageFilter(ddl_percentage, view_salereport.PercentageAmount))
                                        lstOrdertext2.Add(view_salereport);

                                }// end of for (main counter)
                            }// end of if (get_settlement_data)

                        }// end of using reader

                    }// end of using connection
                }// end of if (txt_from)
            }//end of try
            catch (Exception ex)
            {
            }
            return Msg;
        }
        public bool PercentageFilter(int? ddl_percentage, string PercentageAmount)
        {
            bool added = true;
            if (PercentageAmount != null)
            {
                if (ddl_percentage == 2)
                {
                    if (PercentageAmount.Contains("%"))
                    {
                        float percentage = float.Parse(PercentageAmount.Replace("%", ""));
                        if (percentage >= 0 && percentage <= 25)
                            added = true;
                        else
                            added = false;
                    }
                }
                else if (ddl_percentage == 3)
                {
                    if (PercentageAmount.Contains("%"))
                    {
                        float percentage = float.Parse(PercentageAmount.Replace("%", ""));
                        if (percentage >= 25.1 && percentage <= 50)
                            added = true;
                        else
                            added = false;
                    }

                }
                else if (ddl_percentage == 4)
                {
                    if (PercentageAmount.Contains("%"))
                    {
                        float percentage = float.Parse(PercentageAmount.Replace("%", ""));
                        if (percentage >= 50.1 && percentage <= 60)
                            added = true;
                        else
                            added = false;
                    }

                }
                else if (ddl_percentage == 5)
                {
                    if (PercentageAmount.Contains("%"))
                    {
                        float percentage = float.Parse(PercentageAmount.Replace("%", ""));
                        if (percentage >= 60.1 && percentage <= 70)
                            added = true;
                        else
                            added = false;
                    }
                }
                else if (ddl_percentage == 6)
                {
                    if (PercentageAmount.Contains("%"))
                    {
                        float percentage = float.Parse(PercentageAmount.Replace("%", ""));
                        if (percentage >= 70.1 && percentage <= 80)
                            added = true;
                        else
                            added = false;
                    }
                }
                else if (ddl_percentage == 7)
                {
                    if (PercentageAmount.Contains("%"))
                    {
                        float percentage = float.Parse(PercentageAmount.Replace("%", ""));
                        if (percentage >= 80.1 && percentage <= 100)
                            added = true;
                        else
                            added = false;
                    }
                }
                else if (ddl_percentage == 1)
                {
                    if (PercentageAmount.Contains("%"))
                    {
                        float percentage = float.Parse(PercentageAmount.Replace("%", ""));
                        if (percentage >= -100 && percentage <= 0)
                            added = true;
                        else
                            added = false;
                    }
                }
            }
            else
            {
                if (ddl_percentage != 0 && ddl_percentage != null)
                    added = false;
            }
            return added;
        }


        /// <summary>
        /// this is for fetching records related to amazon marketplace
        /// </summary>
        /// <param name="lstOrdertext2"></param>
        /// <param name="view_salereport"></param>
        /// <param name="ddl_market_place"></param>
        /// <param name="txt_from"></param>
        /// <param name="txt_to"></param>
        /// <param name="sellers_id"></param>
        /// <returns></returns>
        public string Get_AmazonSummaryRealizationReport(List<SaleReport> lstOrdertext2, SaleReport view_salereport, int? ddl_market_place, DateTime? txt_from, DateTime? txt_to, int sellers_id)
        {
            string Msg = "S";
            try
            {
                if (txt_from != null && txt_to != null)
                {
                    var GetSaleOrderDetail = (from ob_tbl_sales_order in dba.tbl_sales_order
                                              select new
                                              {
                                                  ob_tbl_sales_order = ob_tbl_sales_order
                                              }).Where(a => a.ob_tbl_sales_order.tbl_sellers_id == sellers_id && a.ob_tbl_sales_order.tbl_Marketplace_Id == ddl_market_place).ToList();
                    int i = 0;
                    view_salereport = new SaleReport();
                    if (GetSaleOrderDetail != null)
                    {
                        if (txt_from != null && txt_to != null)
                        {
                            GetSaleOrderDetail = GetSaleOrderDetail.Where(a => a.ob_tbl_sales_order.purchase_date.Date >= txt_from && a.ob_tbl_sales_order.purchase_date.Date <= txt_to).ToList();
                        }
                    }
                    foreach (var item in GetSaleOrderDetail)
                    {
                        int iRepeatDetailData = 0;
                        if (item != null)
                        {
                            if (view_salereport.OrderID != item.ob_tbl_sales_order.amazon_order_id)
                            {
                                if (view_salereport.OrderID != null)
                                    lstOrdertext2.Add(view_salereport);
                            }
                            int fillItemCount = lstOrdertext2.Count;
                            if (fillItemCount > 0)
                            {
                                if (lstOrdertext2[fillItemCount - 1].OrderID != item.ob_tbl_sales_order.amazon_order_id && view_salereport.OrderID != item.ob_tbl_sales_order.amazon_order_id)
                                    view_salereport = new SaleReport();
                            }

                            //if (item.ob_tbl_sales_order.amazon_order_id == "406-7759166-7128327")
                            //{

                            //}
                            string amazon_order_id = item.ob_tbl_sales_order.amazon_order_id;
                            view_salereport.OrderID = amazon_order_id; //item.ob_tbl_sales_order.amazon_order_id;
                            view_salereport.OrderDate = Convert.ToDateTime(item.ob_tbl_sales_order.purchase_date).ToString("yyyy-MM-dd");
                            // to get sale order details data from  sale order id 

                            double Principal = 0;
                            double orderigst = 0;
                            double ordersgst = 0;
                            double ordercgst = 0, itemprice = 0;
                            string itemname = "", sku = "";
                            var get_saleorder_details = dba.tbl_sales_order_details.Where(a => a.tbl_sales_order_id == item.ob_tbl_sales_order.id && a.tbl_seller_id == sellers_id).ToList();//to get sale_order details 
                            if (get_saleorder_details != null)
                            {
                                foreach (var order_details in get_saleorder_details)
                                {
                                    double ord_detail_Principal = order_details.item_price_amount;
                                    Principal = Principal + ord_detail_Principal;
                                    view_salereport.Principal = Principal;
                                    var get_tax = dba.tbl_tax.Where(a => a.tbl_referneced_id == order_details.id && a.reference_type == 3).FirstOrDefault();
                                    {
                                        if (get_tax != null)
                                        {
                                            orderigst = orderigst + Convert.ToDouble(get_tax.Igst_amount);
                                            ordersgst = ordersgst + Convert.ToDouble(get_tax.sgst_amount);
                                            ordercgst = ordercgst + Convert.ToDouble(get_tax.CGST_amount);

                                            view_salereport.orderigst = orderigst;
                                            view_salereport.ordersgst = ordersgst;
                                            view_salereport.ordercgst = ordercgst;
                                        }
                                    }
                                    view_salereport.ordertotal = view_salereport.Principal + view_salereport.orderigst + view_salereport.ordersgst + view_salereport.ordercgst;
                                    //------------------------------------Get inventory details -------------------------------------------//
                                    var get_inventory = dba.tbl_inventory.Where(a => a.tbl_sellers_id == sellers_id && a.sku.ToLower() == order_details.sku_no.ToLower()).FirstOrDefault();
                                    if (get_inventory != null)
                                    {
                                        itemname = get_inventory.item_name;
                                        sku = get_inventory.sku;

                                        var get_inventoryeffective = dba.tbl_inventory_effectiveprice.Where(a => a.tbl_sellerid == sellers_id && a.tbl_inventory_id == get_inventory.id).OrderByDescending(a => a.Id).FirstOrDefault();
                                        if (get_inventoryeffective != null)
                                        {
                                            itemprice = Convert.ToDouble(get_inventoryeffective.Effective_price);
                                        }
                                        //itemprice = Convert.ToDouble(get_inventory.t_effectiveBought_price);
                                        view_salereport.ProductName = itemname;
                                        view_salereport.ProductValue = itemprice;
                                        view_salereport.skuNo = sku;
                                    }
                                    //------------------------------------------------End--------------------------------------//
                                }
                                iRepeatDetailData++;
                            }
                            ///////--------------End------/////////////

                            // -----------------------get data from tbl_settlement from order-ID -------------------//
                            int iRepeatSettlementData = 0;
                            double orderprincipal = 0;
                            double orderproduct_tax = 0;
                            double ordershipping = 0;
                            double ordershipping_tax = 0;
                            double ordergiftwrap = 0;
                            double ordergiftwrap_tax = 0;
                            double ordershipping_discount = 0;
                            double ordershipping_discounttax = 0;

                            var get_settlementdata = dba.tbl_settlement_order.Where(a => a.Order_Id == item.ob_tbl_sales_order.amazon_order_id && a.tbl_seller_id == sellers_id).ToList();
                            //string uniqueorderid = "";
                            if (get_settlementdata != null)
                            {
                                foreach (var settle in get_settlementdata)
                                {
                                    //string uniqueorderid = "";
                                    orderprincipal = orderprincipal + Convert.ToDouble(settle.principal_price);
                                    orderproduct_tax = orderproduct_tax + Convert.ToDouble(settle.product_tax);
                                    ordershipping = ordershipping + Convert.ToDouble(settle.shipping_price);
                                    ordershipping_tax = ordershipping_tax + Convert.ToDouble(settle.shipping_tax);
                                    ordergiftwrap = ordergiftwrap + Convert.ToDouble(settle.giftwrap_price);
                                    ordergiftwrap_tax = ordergiftwrap_tax + Convert.ToDouble(settle.giftwarp_tax);
                                    ordershipping_discount = ordershipping_discount + Convert.ToDouble(settle.shipping_discount);
                                    ordershipping_discounttax = ordershipping_discounttax + Convert.ToDouble(settle.shipping_tax_discount);

                                    view_salereport.orderprincipal = orderprincipal;
                                    view_salereport.orderproduct_tax = orderproduct_tax;
                                    view_salereport.ordershipping = ordershipping;
                                    view_salereport.ordershipping_tax = ordershipping_tax;
                                    view_salereport.ordergiftwrap = ordergiftwrap;
                                    view_salereport.ordergiftwrap_tax = ordergiftwrap_tax;
                                    view_salereport.ordershipping_discount = ordershipping_discount;
                                    view_salereport.ordershipping_discounttax = ordershipping_discounttax;

                                    view_salereport.orderTotal = view_salereport.orderprincipal + view_salereport.orderproduct_tax + view_salereport.ordershipping + view_salereport.ordershipping_tax + view_salereport.ordergiftwrap + view_salereport.ordergiftwrap_tax + view_salereport.ordershipping_discount + view_salereport.ordershipping_discounttax;
                                    view_salereport.ReferenceID = settle.settlement_id;
                                    //uniqueorderid = settle.unique_order_id;
                                    //    }
                                    //    iRepeatSettlementData++;
                                    //}
                                    //-------------------------End---------------------------//
                                    //-----------------------Get  data from tbl_Expense from settlement_Id----------------//

                                    double FBAFEE = 0, FBACGST = 0, FBASGST = 0, TechnologyFee = 0, TechnologyIGST = 0, TechnologySGST = 0, TechnologyCGST = 0, CommissionFee = 0, CommissionIGST = 0, CommissionCGST = 0, CommissionSGST = 0,
                                           FixedClosingFee = 0, FixedclosingIGST = 0, FixedclosingCGST = 0, FixedclosingSGST = 0, ShippingChargebackFee = 0,
                                           shippingchargeCGST = 0, shippingchargeSGST = 0, ShippingDiscountFee = 0, Shippingtaxdiscount = 0, RefundCommision = 0, RefundDiscount = 0,
                                           ShippingCommision = 0, ShippingCommissionIGST = 0, EasyShipweight = 0, EsayshipWeightIGST = 0;

                                    var getsettlementdetails = dba.m_tbl_expense.Where(a => a.settlement_order_id.Contains(amazon_order_id) && a.tbl_seller_id == sellers_id && a.t_transactionType_id == 1).ToList();
                                    if (getsettlementdetails != null && getsettlementdetails.Count > 0)
                                    {
                                        foreach (var item1 in getsettlementdetails)
                                        {
                                            var exp_id = item1.expense_type_id;
                                            var get_expdetails = dba.m_settlement_fee.Where(a => a.id == exp_id).FirstOrDefault();
                                            var gettax_details = dba.tbl_tax.Where(a => a.tbl_referneced_id == item1.id && a.reference_type == 2).FirstOrDefault();
                                            if (get_expdetails != null)
                                            {
                                                string nam = get_expdetails.return_fee;
                                                if (nam == "FBA Weight Handling Fee")
                                                {
                                                    if (get_expdetails.id == exp_id)
                                                    {
                                                        FBAFEE = FBAFEE + Convert.ToDouble(item1.expense_amount);
                                                        view_salereport.FBAFEE = FBAFEE;
                                                        if (gettax_details != null)
                                                        {
                                                            FBACGST = FBACGST + Convert.ToDouble(gettax_details.CGST_amount);
                                                            FBASGST = FBASGST + Convert.ToDouble(gettax_details.sgst_amount);
                                                            view_salereport.FBACGST = FBACGST;
                                                            view_salereport.FBASGST = FBASGST;
                                                        }
                                                    }
                                                }
                                                else if (nam == "Technology Fee")
                                                {
                                                    if (get_expdetails.id == exp_id)
                                                    {
                                                        TechnologyFee = TechnologyFee + Convert.ToDouble(item1.expense_amount);
                                                        view_salereport.TechnologyFee = TechnologyFee;
                                                        if (gettax_details != null)
                                                        {
                                                            TechnologyIGST = TechnologyIGST + Convert.ToDouble(gettax_details.Igst_amount);
                                                            TechnologyCGST = TechnologyCGST + Convert.ToDouble(gettax_details.CGST_amount);
                                                            TechnologySGST = TechnologySGST + Convert.ToDouble(gettax_details.sgst_amount);
                                                            view_salereport.TechnologyIGST = TechnologyIGST;
                                                            view_salereport.TechnologyCGST = TechnologyCGST;
                                                            view_salereport.TechnologySGST = TechnologySGST;
                                                        }
                                                    }
                                                }
                                                else if (nam == "Commission")
                                                {
                                                    if (get_expdetails.id == exp_id)
                                                    {
                                                        CommissionFee = CommissionFee + Convert.ToDouble(item1.expense_amount);
                                                        view_salereport.CommissionFee = CommissionFee;
                                                        if (gettax_details != null)
                                                        {
                                                            CommissionIGST = CommissionIGST + Convert.ToDouble(gettax_details.Igst_amount);
                                                            CommissionCGST = CommissionCGST + Convert.ToDouble(gettax_details.CGST_amount);
                                                            CommissionSGST = CommissionSGST + Convert.ToDouble(gettax_details.sgst_amount);

                                                            view_salereport.CommissionIGST = CommissionIGST;
                                                            view_salereport.CommissionCGST = CommissionCGST;
                                                            view_salereport.CommissionSGST = CommissionSGST;
                                                        }
                                                    }
                                                }
                                                else if (nam == "Fixed closing fee")
                                                {
                                                    if (get_expdetails.id == exp_id)
                                                    {
                                                        FixedClosingFee = FixedClosingFee + Convert.ToDouble(item1.expense_amount);
                                                        view_salereport.FixedClosingFee = FixedClosingFee;
                                                        if (gettax_details != null)
                                                        {
                                                            FixedclosingIGST = FixedclosingIGST + Convert.ToDouble(gettax_details.Igst_amount);
                                                            FixedclosingCGST = FixedclosingCGST + Convert.ToDouble(gettax_details.CGST_amount);
                                                            FixedclosingSGST = FixedclosingSGST + Convert.ToDouble(gettax_details.sgst_amount);

                                                            view_salereport.FixedclosingIGST = FixedclosingIGST;
                                                            view_salereport.FixedclosingCGST = FixedclosingCGST;
                                                            view_salereport.FixedclosingSGST = FixedclosingSGST;
                                                        }
                                                    }
                                                }
                                                else if (nam == "Shipping Chargeback")
                                                {
                                                    if (get_expdetails.id == exp_id)
                                                    {
                                                        ShippingChargebackFee = ShippingChargebackFee + Convert.ToDouble(item1.expense_amount);
                                                        view_salereport.ShippingChargebackFee = ShippingChargebackFee;
                                                        if (gettax_details != null)
                                                        {
                                                            shippingchargeCGST = shippingchargeCGST + Convert.ToDouble(gettax_details.CGST_amount);
                                                            shippingchargeSGST = shippingchargeSGST + Convert.ToDouble(gettax_details.sgst_amount);
                                                            view_salereport.shippingchargeCGST = shippingchargeCGST;
                                                            view_salereport.shippingchargeSGST = shippingchargeSGST;
                                                        }
                                                    }
                                                }
                                                else if (nam == "Shipping discount")
                                                {
                                                    if (get_expdetails.id == exp_id)
                                                    {
                                                        ShippingDiscountFee = ShippingDiscountFee + Convert.ToDouble(item1.expense_amount);
                                                        view_salereport.ShippingDiscountFee = ShippingDiscountFee;
                                                        if (gettax_details != null)
                                                        {
                                                            Shippingtaxdiscount = Shippingtaxdiscount + Convert.ToDouble(gettax_details.Igst_amount);
                                                            view_salereport.Shippingtaxdiscount = Shippingtaxdiscount;
                                                        }
                                                    }
                                                }
                                                else if (nam == "Refund commission")
                                                {
                                                    if (get_expdetails.id == exp_id)
                                                    {
                                                        RefundCommision = RefundCommision + Convert.ToDouble(item1.expense_amount);
                                                        view_salereport.RefundCommision = RefundCommision;
                                                        if (gettax_details != null)
                                                        {
                                                            RefundDiscount = RefundDiscount + Convert.ToDouble(gettax_details.Igst_amount);
                                                            view_salereport.RefundDiscount = RefundDiscount;
                                                        }
                                                    }
                                                }

                                                else if (nam == "Shipping commission")
                                                {
                                                    if (get_expdetails.id == exp_id)
                                                    {
                                                        ShippingCommision = ShippingCommision + Convert.ToDouble(item1.expense_amount);
                                                        view_salereport.ShippingCommision = ShippingCommision;
                                                        if (gettax_details != null)
                                                        {
                                                            ShippingCommissionIGST = ShippingCommissionIGST + Convert.ToDouble(gettax_details.Igst_amount);
                                                            view_salereport.ShippingCommissionIGST = ShippingCommissionIGST;
                                                        }
                                                    }
                                                }

                                                else if (nam == "Easy Ship weight handling fees")
                                                {
                                                    if (get_expdetails.id == exp_id)
                                                    {
                                                        EasyShipweight = EasyShipweight + Convert.ToDouble(item1.expense_amount);
                                                        view_salereport.EasyShipweighthandlingfees = EasyShipweight;
                                                        if (gettax_details != null)
                                                        {
                                                            EsayshipWeightIGST = EsayshipWeightIGST + Convert.ToDouble(gettax_details.Igst_amount);
                                                            view_salereport.EasyShipweighthandlingfeesIGST = EsayshipWeightIGST;
                                                        }
                                                    }
                                                }
                                            }// end of if(get_expdetails)
                                        }// end if foreach(item1)
                                    }// end of if(getsettlementdetails) 
                                }
                                iRepeatSettlementData++;
                            }

                            //------------------------------------------End--------------------------------------//
                            //-------------------------------------To get Refund data from tbl_History and taxtable------------------------//
                            int iRepeatrefundData = 0;
                            double refundprincipal = 0;
                            double refundproduct_tax = 0;
                            double refundshipping = 0;
                            double refundshipping_tax = 0;
                            double refundgiftwrap = 0;
                            double refundgiftwrap_tax = 0;
                            double refundshipping_discount = 0;
                            double refundshipping_discounttax = 0;
                            var get_historydata = dba.tbl_order_history.Where(a => a.tbl_seller_id == sellers_id && a.OrderID == item.ob_tbl_sales_order.amazon_order_id && a.t_order_status == 9).ToList();
                            if (get_historydata != null)
                            {
                                foreach (var refunditem in get_historydata)
                                {
                                    refundprincipal = refundprincipal + Convert.ToDouble(refunditem.amount_per_unit);
                                    refundproduct_tax = refundproduct_tax + Convert.ToDouble(refunditem.product_tax);
                                    refundshipping = refundshipping + Convert.ToDouble(refunditem.shipping_price);
                                    refundshipping_tax = refundshipping_tax + Convert.ToDouble(refunditem.shipping_tax);
                                    refundgiftwrap = refundgiftwrap + Convert.ToDouble(refunditem.Giftwrap_price);
                                    refundgiftwrap_tax = refundgiftwrap_tax + Convert.ToDouble(refunditem.gift_wrap_tax);
                                    refundshipping_discount = refundshipping_discount + Convert.ToDouble(refunditem.shipping_discount);
                                    refundshipping_discounttax = refundshipping_discounttax + Convert.ToDouble(refunditem.shipping_tax_discount);


                                    view_salereport.refundprincipal = refundprincipal;
                                    view_salereport.refundproduct_tax = refundproduct_tax;
                                    view_salereport.refundshipping = refundshipping;
                                    view_salereport.refundshipping_tax = refundshipping_tax;
                                    view_salereport.refundgiftwrap = refundgiftwrap;
                                    view_salereport.refundgiftwrap_tax = refundgiftwrap_tax;
                                    view_salereport.refundshipping_discount = refundshipping_discount;
                                    view_salereport.refundshipping_discount_tax = refundshipping_discounttax;
                                    view_salereport.refundTotal = view_salereport.refundprincipal + view_salereport.refundproduct_tax + view_salereport.refundshipping + view_salereport.refundshipping_tax + view_salereport.refundgiftwrap + view_salereport.refundgiftwrap_tax + view_salereport.refundshipping_discount + view_salereport.refundshipping_discount_tax;
                                    view_salereport.refundReferenceID = refunditem.settlement_id;


                                    double refundFBAFEE = 0, refundFBACGST = 0, refundFBASGST = 0, refundTechnologyFee = 0, refundTechnologyIGST = 0, refundTechnologyCGST = 0, refundTechnologySGST = 0, refundCommissionFee = 0, refundCommissionIGST = 0, refundCommissionCGST = 0,
                                          refundCommissionSGST = 0, refundFixedClosingFee = 0, refundFixedclosingIGST = 0,
                                          refundFixedclosingCGST = 0, refundFixedclosingSGST = 0, refundShippingChargebackFee = 0, refundEasyShipweighthandlingfees = 0,
                                          refundshippingchargeCGST = 0, refundshippingchargeSGST = 0, refundShippingDiscountFee = 0, refundShippingtaxdiscount = 0,
                                          refund_RefundCommision = 0, refund_Refund_Discount = 0, refund_Discount_cgst = 0, refund_discount_sgst = 0, refundShippingCommision = 0, refundShippingCommissionIGST = 0, refundEasyShipweighthandlingfeesIGST = 0,
                                          refundFBAPickPackFee = 0, refundFBAPickPackFeeCGST = 0, refundFBAPickPackFeeSGST = 0, refundGiftWrapChargeback = 0, refundGiftWrapChargebackCGST = 0,
                                          refundGiftWrapChargebackSGST = 0, refundAmazonEasyShipCharges = 0, refundAmazonEasyShipChargesIGST = 0;


                                    var get_refundexpense = dba.m_tbl_expense.Where(a => a.settlement_order_id.Contains(amazon_order_id) && a.tbl_seller_id == sellers_id && a.t_transactionType_id == 2).ToList();// to get refund expense
                                    // var get_refundexpense = dba.m_tbl_expense.Where(a => a.settlement_order_id == get_historydata.unique_order_id && a.tbl_seller_id == sellers_id && a.t_transactionType_id == 2 && a.tbl_order_historyid == get_historydata.Id).ToList();// to get refund expense
                                    if (get_refundexpense != null && get_refundexpense.Count > 0)
                                    {
                                        foreach (var refund in get_refundexpense)
                                        {
                                            var exp_ID = refund.expense_type_id;
                                            var get_details = dba.m_settlement_fee.Where(a => a.id == exp_ID).FirstOrDefault();
                                            var getExp_tax_details = dba.tbl_tax.Where(a => a.tbl_referneced_id == refund.id && a.reference_type == 7).FirstOrDefault();
                                            if (get_details != null)
                                            {
                                                string nam = get_details.return_fee;
                                                if (nam == "FBA Weight Handling Fee")
                                                {
                                                    if (get_details.id == exp_ID)
                                                    {
                                                        refundFBAFEE = refundFBAFEE + Convert.ToDouble(refund.expense_amount);
                                                        view_salereport.RefundFBAFEE = refundFBAFEE;
                                                        if (getExp_tax_details != null)
                                                        {
                                                            refundFBACGST = refundFBACGST + Convert.ToDouble(getExp_tax_details.CGST_amount);
                                                            refundFBASGST = refundFBASGST + Convert.ToDouble(getExp_tax_details.sgst_amount);

                                                            view_salereport.RefundFBACGST = refundFBACGST;
                                                            view_salereport.RefundFBASGST = refundFBASGST;
                                                        }
                                                    }
                                                }
                                                else if (nam == "Technology Fee")
                                                {
                                                    if (get_details.id == exp_ID)
                                                    {
                                                        refundTechnologyFee = refundTechnologyFee + Convert.ToDouble(refund.expense_amount);
                                                        view_salereport.RefundTechnologyFee = refundTechnologyFee;
                                                        if (getExp_tax_details != null)
                                                        {
                                                            refundTechnologyIGST = refundTechnologyIGST + Convert.ToDouble(getExp_tax_details.Igst_amount);
                                                            refundTechnologyCGST = refundTechnologyCGST + Convert.ToDouble(getExp_tax_details.CGST_amount);
                                                            refundTechnologySGST = refundTechnologySGST + Convert.ToDouble(getExp_tax_details.sgst_amount);

                                                            view_salereport.RefundTechnologyIGST = refundTechnologyIGST;
                                                            view_salereport.RefundTechnologyCGST = refundTechnologyCGST;
                                                            view_salereport.RefundTechnologySGST = refundTechnologySGST;
                                                        }
                                                    }
                                                }
                                                else if (nam == "Commission")
                                                {
                                                    if (get_details.id == exp_ID)
                                                    {
                                                        refundCommissionFee = refundCommissionFee + Convert.ToDouble(refund.expense_amount);
                                                        view_salereport.RefundCommissionFee = refundCommissionFee;
                                                        if (getExp_tax_details != null)
                                                        {
                                                            refundCommissionIGST = refundCommissionIGST + Convert.ToDouble(getExp_tax_details.Igst_amount);
                                                            refundCommissionCGST = refundCommissionCGST + Convert.ToDouble(getExp_tax_details.CGST_amount);
                                                            refundCommissionSGST = refundCommissionSGST + Convert.ToDouble(getExp_tax_details.sgst_amount);

                                                            view_salereport.RefundCommissionIGST = refundCommissionIGST;
                                                            view_salereport.RefundCommissionCGST = refundCommissionCGST;
                                                            view_salereport.RefundCommissionSGST = refundCommissionSGST;
                                                        }
                                                    }
                                                }
                                                else if (nam == "Fixed closing fee")
                                                {
                                                    if (get_details.id == exp_ID)
                                                    {
                                                        refundFixedClosingFee = refundFixedClosingFee + Convert.ToDouble(refund.expense_amount);
                                                        view_salereport.RefundFixedClosingFee = refundFixedClosingFee;
                                                        if (getExp_tax_details != null)
                                                        {
                                                            refundFixedclosingIGST = refundFixedclosingIGST + Convert.ToDouble(getExp_tax_details.Igst_amount);
                                                            refundFixedclosingCGST = refundFixedclosingCGST + Convert.ToDouble(getExp_tax_details.CGST_amount);
                                                            refundFixedclosingSGST = refundFixedclosingSGST + Convert.ToDouble(getExp_tax_details.sgst_amount);

                                                            view_salereport.RefundFixedclosingIGST = refundFixedclosingIGST;
                                                            view_salereport.RefundFixedclosingCGST = refundFixedclosingCGST;
                                                            view_salereport.RefundFixedclosingSGST = refundFixedclosingSGST;
                                                        }
                                                    }
                                                }
                                                else if (nam == "Shipping Chargeback")
                                                {
                                                    if (get_details.id == exp_ID)
                                                    {
                                                        refundShippingChargebackFee = refundShippingChargebackFee + Convert.ToDouble(refund.expense_amount);
                                                        view_salereport.RefundShippingChargebackFee = refundShippingChargebackFee;
                                                        if (getExp_tax_details != null)
                                                        {
                                                            refundshippingchargeCGST = refundshippingchargeCGST + Convert.ToDouble(getExp_tax_details.CGST_amount);
                                                            refundshippingchargeSGST = refundshippingchargeSGST + Convert.ToDouble(getExp_tax_details.sgst_amount);

                                                            view_salereport.RefundshippingchargeCGST = refundshippingchargeCGST;
                                                            view_salereport.RefundshippingchargeSGST = refundshippingchargeSGST;
                                                        }
                                                    }
                                                }
                                                else if (nam == "Shipping discount")
                                                {
                                                    if (get_details.id == exp_ID)
                                                    {
                                                        refundShippingDiscountFee = refundShippingDiscountFee + Convert.ToDouble(refund.expense_amount);
                                                        view_salereport.RefundShippingDiscountFee = refundShippingDiscountFee;
                                                        if (getExp_tax_details != null)
                                                        {
                                                            refundShippingtaxdiscount = refundShippingtaxdiscount + Convert.ToDouble(getExp_tax_details.Igst_amount);
                                                            view_salereport.RefundShippingtaxdiscount = refundShippingtaxdiscount;
                                                        }
                                                    }
                                                }
                                                else if (nam == "Refund commission")
                                                {
                                                    if (get_details.id == exp_ID)
                                                    {
                                                        refund_RefundCommision = refund_RefundCommision + Convert.ToDouble(refund.expense_amount);
                                                        view_salereport.Refund_Commision = refund_RefundCommision;
                                                        if (getExp_tax_details != null)
                                                        {
                                                            refund_Refund_Discount = refund_Refund_Discount + Convert.ToDouble(getExp_tax_details.Igst_amount);
                                                            refund_Discount_cgst = refund_Discount_cgst + Convert.ToDouble(getExp_tax_details.CGST_amount);
                                                            refund_discount_sgst = refund_discount_sgst + Convert.ToDouble(getExp_tax_details.sgst_amount);

                                                            view_salereport.Refund_Discount = refund_Refund_Discount;
                                                            view_salereport.Refund_DiscountCGST = refund_Discount_cgst;
                                                            view_salereport.Refund_DiscountSGST = refund_discount_sgst;
                                                        }
                                                    }
                                                }

                                                else if (nam == "Shipping commission")
                                                {
                                                    if (get_details.id == exp_ID)
                                                    {
                                                        refundShippingCommision = refundShippingCommision + Convert.ToDouble(refund.expense_amount);
                                                        view_salereport.Refund_ShippingCommision = refundShippingCommision;
                                                        if (getExp_tax_details != null)
                                                        {
                                                            refundShippingCommissionIGST = refundShippingCommissionIGST + Convert.ToDouble(getExp_tax_details.Igst_amount);
                                                            view_salereport.Refund_Shipping_Commission = refundShippingCommissionIGST;
                                                        }
                                                    }
                                                }
                                                else if (nam == "Easy Ship weight handling fees")
                                                {
                                                    if (get_details.id == exp_ID)
                                                    {
                                                        refundEasyShipweighthandlingfees = refundEasyShipweighthandlingfees + Convert.ToDouble(refund.expense_amount);
                                                        view_salereport.Refund_EasyShipweighthandlingfees = refundEasyShipweighthandlingfees;
                                                        if (getExp_tax_details != null)
                                                        {
                                                            refundEasyShipweighthandlingfeesIGST = refundEasyShipweighthandlingfeesIGST + Convert.ToDouble(getExp_tax_details.Igst_amount);
                                                            view_salereport.Refund_EasyShipweighthandlingfeesIGST = refundEasyShipweighthandlingfeesIGST;
                                                        }
                                                    }
                                                }
                                            }// end of if(get_details)                               
                                        }// end of foreach(refund)
                                    }// end of if(get_refundexpense)
                                }// end of froeach loop()
                                iRepeatrefundData++;
                            }// end of if(get_historydata)

                            view_salereport.Refund_SUMIGST = view_salereport.RefundTechnologyIGST + view_salereport.RefundCommissionIGST + view_salereport.RefundFixedclosingIGST + view_salereport.RefundShippingtaxdiscount + view_salereport.Refund_Discount + view_salereport.Refund_Shipping_Commission + view_salereport.Refund_EasyShipweighthandlingfeesIGST;
                            view_salereport.Refund_SUMCGST = view_salereport.RefundshippingchargeCGST + view_salereport.RefundFBACGST + view_salereport.Refund_DiscountCGST + view_salereport.RefundFixedclosingCGST + view_salereport.RefundCommissionCGST + view_salereport.RefundTechnologyCGST;
                            view_salereport.Refund_SUMSGST = view_salereport.RefundshippingchargeSGST + view_salereport.RefundFBASGST + view_salereport.Refund_DiscountSGST + view_salereport.RefundFixedclosingSGST + view_salereport.RefundCommissionSGST + view_salereport.RefundTechnologySGST;
                            view_salereport.refund_SumFee = view_salereport.RefundFBAFEE + view_salereport.RefundTechnologyFee + view_salereport.RefundCommissionFee + view_salereport.RefundFixedClosingFee + view_salereport.RefundShippingChargebackFee + view_salereport.RefundShippingDiscountFee + view_salereport.Refund_Commision + view_salereport.Refund_ShippingCommision + view_salereport.Refund_EasyShipweighthandlingfees + view_salereport.Refund_SUMIGST + view_salereport.Refund_SUMCGST + view_salereport.Refund_SUMSGST;
                            view_salereport.refund_SumOrder = view_salereport.refundTotal + view_salereport.refund_SumFee;
                            /////////-----------------------------------------End----------------------------------------------------------//

                            //sharad
                            view_salereport.SUMIGST = view_salereport.TechnologyIGST + view_salereport.CommissionIGST + view_salereport.FixedclosingIGST + view_salereport.Shippingtaxdiscount + view_salereport.RefundDiscount + view_salereport.ShippingCommissionIGST + view_salereport.EasyShipweighthandlingfeesIGST;
                            //view_salereport.SUMIGST = view_salereport.RefundCommisionIgst_Deducted + view_salereport.TechnologyIGST + view_salereport.CommissionIGST + view_salereport.Shippingtaxdiscount + view_salereport.RefundDiscount;
                            view_salereport.SUMCGST = view_salereport.shippingchargeCGST + view_salereport.FBACGST + view_salereport.FixedclosingCGST + view_salereport.CommissionCGST + view_salereport.TechnologyCGST;
                            view_salereport.SUMSGST = view_salereport.shippingchargeSGST + view_salereport.FBASGST + view_salereport.FixedclosingSGST + view_salereport.CommissionSGST + view_salereport.TechnologySGST;

                            view_salereport.SumFee = view_salereport.FBAFEE + view_salereport.TechnologyFee + view_salereport.CommissionFee + view_salereport.FixedClosingFee + view_salereport.ShippingChargebackFee + view_salereport.ShippingDiscountFee + view_salereport.RefundCommision + view_salereport.ShippingCommision + view_salereport.EasyShipweighthandlingfees + view_salereport.SUMIGST + view_salereport.SUMCGST + view_salereport.SUMSGST;
                            view_salereport.SumOrder = view_salereport.orderTotal + view_salereport.SumFee;

                            string Value = "";
                            Value = (view_salereport.SumOrder + view_salereport.refund_SumOrder).ToString("0.00");
                            view_salereport.NetTotal = Convert.ToDouble(Value);
                            view_salereport.Profit_lossAmount = view_salereport.NetTotal - view_salereport.ProductValue;

                            string value = "";
                            value = Convert.ToString((view_salereport.NetTotal / view_salereport.orderTotal) * 100);

                            if (value != "NaN" && value != "-Infinity")
                            {
                                decimal abc = Convert.ToDecimal((view_salereport.NetTotal / view_salereport.orderTotal) * 100);
                                decimal result = decimal.Round(abc, 2, MidpointRounding.AwayFromZero);

                                string bb = value.ToString();
                                string[] b = bb.Split('.');
                                int firstValue = int.Parse(b[0]);

                                string fvalue = result + "%";
                                view_salereport.PercentageAmount = Convert.ToString(fvalue);
                            }

                            i++;
                        }// end of if(item !=  null)
                    }// end of foreach(GetSaleOrderDetail)
                    if (view_salereport.OrderID != null)
                    {
                        lstOrdertext2.Add(view_salereport);
                    }
                }// end of main if
            }
            catch (Exception ex)
            {
            }
            return Msg;
        }


        /// <summary>
        /// this is used for fetching records related to flipkart marketplace
        /// </summary>
        /// <param name="lstOrdertext2"></param>
        /// <param name="view_salereport"></param>
        /// <param name="ddl_market_place"></param>
        /// <param name="txt_from"></param>
        /// <param name="txt_to"></param>
        /// <param name="sellers_id"></param>
        /// <returns></returns>
        public string Get_FlipkartSummaryRealizationReport(List<SaleReport> lstOrdertext2, SaleReport view_salereport, int? ddl_market_place, DateTime? txt_from, DateTime? txt_to, int sellers_id)
        {
            string Msg = "S";
            try
            {
                if (txt_from != null && txt_to != null)
                {
                    var GetSaleOrderDetail = (from ob_tbl_sales_order in dba.tbl_sales_order
                                              select new
                                              {
                                                  ob_tbl_sales_order = ob_tbl_sales_order
                                              }).Where(a => a.ob_tbl_sales_order.tbl_sellers_id == sellers_id && a.ob_tbl_sales_order.tbl_Marketplace_Id == ddl_market_place).ToList();
                    int i = 0;
                    view_salereport = new SaleReport();
                    if (GetSaleOrderDetail != null)
                    {
                        if (txt_from != null && txt_to != null)
                        {
                            GetSaleOrderDetail = GetSaleOrderDetail.Where(a => a.ob_tbl_sales_order.purchase_date.Date >= txt_from && a.ob_tbl_sales_order.purchase_date.Date <= txt_to).ToList();
                        }
                    }
                    foreach (var item in GetSaleOrderDetail)
                    {
                        int iRepeatDetailData = 0;
                        if (item != null)
                        {
                            if (view_salereport.OrderID != item.ob_tbl_sales_order.amazon_order_id)
                            {
                                if (view_salereport.OrderID != null)
                                    lstOrdertext2.Add(view_salereport);
                            }
                            int fillItemCount = lstOrdertext2.Count;
                            if (fillItemCount > 0)
                            {
                                if (lstOrdertext2[fillItemCount - 1].OrderID != item.ob_tbl_sales_order.amazon_order_id && view_salereport.OrderID != item.ob_tbl_sales_order.amazon_order_id)
                                    view_salereport = new SaleReport();
                            }


                            string amazon_order_id = item.ob_tbl_sales_order.amazon_order_id;
                            view_salereport.OrderID = amazon_order_id; //item.ob_tbl_sales_order.amazon_order_id;
                            view_salereport.OrderDate = Convert.ToDateTime(item.ob_tbl_sales_order.purchase_date).ToString("yyyy-MM-dd");
                            // to get sale order details data from  sale order id 

                            double Principal = 0;
                            double orderigst = 0;
                            double ordersgst = 0;
                            double ordercgst = 0, itemprice = 0;
                            string itemname = "", sku = "";
                            var get_saleorder_details = dba.tbl_sales_order_details.Where(a => a.tbl_sales_order_id == item.ob_tbl_sales_order.id && a.tbl_seller_id == sellers_id).ToList();//to get sale_order details 
                            if (get_saleorder_details != null)
                            {
                                foreach (var order_details in get_saleorder_details)
                                {
                                    double ord_detail_Principal = order_details.item_price_amount;
                                    Principal = Principal + ord_detail_Principal;
                                    view_salereport.Principal = Principal;
                                    var get_tax = dba.tbl_tax.Where(a => a.tbl_referneced_id == order_details.id && a.reference_type == 3).FirstOrDefault();
                                    {
                                        if (get_tax != null)
                                        {
                                            orderigst = orderigst + Convert.ToDouble(get_tax.Igst_amount);
                                            ordersgst = ordersgst + Convert.ToDouble(get_tax.sgst_amount);
                                            ordercgst = ordercgst + Convert.ToDouble(get_tax.CGST_amount);

                                            view_salereport.orderigst = orderigst;
                                            view_salereport.ordersgst = ordersgst;
                                            view_salereport.ordercgst = ordercgst;
                                        }
                                    }
                                    view_salereport.ordertotal = view_salereport.Principal + view_salereport.orderigst + view_salereport.ordersgst + view_salereport.ordercgst;
                                    //------------------------------------Get inventory details -------------------------------------------//
                                    var get_inventory = dba.tbl_inventory.Where(a => a.tbl_sellers_id == sellers_id && a.sku.ToLower() == order_details.sku_no.ToLower()).FirstOrDefault();
                                    if (get_inventory != null)
                                    {
                                        itemname = get_inventory.item_name;
                                        sku = get_inventory.sku;

                                        var get_inventoryeffective = dba.tbl_inventory_effectiveprice.Where(a => a.tbl_sellerid == sellers_id && a.tbl_inventory_id == get_inventory.id).OrderByDescending(a => a.Id).FirstOrDefault();
                                        if (get_inventoryeffective != null)
                                        {
                                            itemprice = Convert.ToDouble(get_inventoryeffective.Effective_price);
                                        }
                                        //itemprice = Convert.ToDouble(get_inventory.t_effectiveBought_price);
                                        view_salereport.ProductName = itemname;
                                        view_salereport.ProductValue = itemprice;
                                        view_salereport.skuNo = sku;
                                    }
                                    //------------------------------------------------End--------------------------------------//
                                }
                                iRepeatDetailData++;
                            }
                            ///////--------------End------/////////////

                            #region settlementdata
                            // -----------------------get data from tbl_settlement from order-ID -------------------//
                            int iRepeatSettlementData = 0;
                            double orderprincipal = 0;
                            double ordershipping = 0;
                            double ordergiftwrap = 0;
                            double orderproduct_tax = 0;
                            double ordergiftwrap_tax = 0;
                            double ordershipping_tax = 0;
                            double ordershipping_discount = 0;
                            double ordershipping_discounttax = 0;

                            var get_settlementdata = dba.tbl_settlement_order.Where(a => a.Order_Id == item.ob_tbl_sales_order.amazon_order_id && a.tbl_seller_id == sellers_id).ToList();
                            //string uniqueorderid = "";
                            if (get_settlementdata != null)
                            {
                                foreach (var settle in get_settlementdata)
                                {
                                    //string uniqueorderid = "";
                                    orderprincipal = orderprincipal + Convert.ToDouble(settle.principal_price);
                                    orderproduct_tax = orderproduct_tax + Convert.ToDouble(settle.product_tax);
                                    ordershipping = ordershipping + Convert.ToDouble(settle.shipping_price);
                                    ordershipping_tax = ordershipping_tax + Convert.ToDouble(settle.shipping_tax);
                                    ordergiftwrap = ordergiftwrap + Convert.ToDouble(settle.giftwrap_price);
                                    ordergiftwrap_tax = ordergiftwrap_tax + Convert.ToDouble(settle.giftwarp_tax);
                                    ordershipping_discount = ordershipping_discount + Convert.ToDouble(settle.shipping_discount);
                                    ordershipping_discounttax = ordershipping_discounttax + Convert.ToDouble(settle.shipping_tax_discount);

                                    view_salereport.orderprincipal = orderprincipal;
                                    view_salereport.orderproduct_tax = orderproduct_tax;
                                    view_salereport.ordershipping = ordershipping;
                                    view_salereport.ordershipping_tax = ordershipping_tax;
                                    view_salereport.ordergiftwrap = ordergiftwrap;
                                    view_salereport.ordergiftwrap_tax = ordergiftwrap_tax;
                                    view_salereport.ordershipping_discount = ordershipping_discount;
                                    view_salereport.ordershipping_discounttax = ordershipping_discounttax;

                                    view_salereport.orderTotal = view_salereport.orderprincipal + view_salereport.orderproduct_tax + view_salereport.ordershipping + view_salereport.ordershipping_tax + view_salereport.ordergiftwrap + view_salereport.ordergiftwrap_tax + view_salereport.ordershipping_discount + view_salereport.ordershipping_discounttax;
                                    view_salereport.ReferenceID = settle.settlement_id;
                                    //uniqueorderid = settle.unique_order_id;
                                    //    }
                                    //    iRepeatSettlementData++;
                                    //}
                                    //-------------------------End---------------------------//
                                    //-----------------------Get  data from tbl_Expense from settlement_Id----------------//

                                    double flipShipping = 0, flipShippingFeeIGST = 0, flipShippingFeeCGST = 0, flipShippingFeeSGST = 0, flipCollection = 0, flipCollectionIGST = 0, flipCollectionSGST = 0, flipCollectionCGST = 0,
                                           CommissionFee = 0, CommissionIGST = 0, CommissionCGST = 0, CommissionSGST = 0,
                                           flipReverseShipping = 0, flipReverseShippingIGST = 0, flipReverseShippingCGST = 0, flipReverseShippingSGST = 0,
                                           flipFixedFee = 0, flipFixedFeeIGST = 0, flipFixedFeeCGST = 0, flipFixedFeeSGST = 0;

                                    var getsettlementdetails = dba.m_tbl_expense.Where(a => a.settlement_order_id.Contains(amazon_order_id) && a.tbl_seller_id == sellers_id && a.t_transactionType_id == 1).ToList();
                                    if (getsettlementdetails != null && getsettlementdetails.Count > 0)
                                    {
                                        foreach (var item1 in getsettlementdetails)
                                        {
                                            var exp_id = item1.expense_type_id;
                                            var get_expdetails = dba.m_settlement_fee.Where(a => a.id == exp_id).FirstOrDefault();
                                            var gettax_details = dba.tbl_tax.Where(a => a.tbl_referneced_id == item1.id && a.reference_type == 2).FirstOrDefault();
                                            if (get_expdetails != null)
                                            {
                                                string nam = get_expdetails.return_fee;
                                                if (nam == "Shipping Fee")
                                                {
                                                    if (get_expdetails.id == exp_id)
                                                    {
                                                        flipShipping = flipShipping + Convert.ToDouble(item1.expense_amount);
                                                        view_salereport.flipShipping = flipShipping;
                                                        if (gettax_details != null)
                                                        {
                                                            flipShippingFeeIGST = flipShippingFeeIGST + Convert.ToDouble(gettax_details.Igst_amount);
                                                            flipShippingFeeCGST = flipShippingFeeCGST + Convert.ToDouble(gettax_details.CGST_amount);
                                                            flipShippingFeeSGST = flipShippingFeeSGST + Convert.ToDouble(gettax_details.sgst_amount);

                                                            view_salereport.flipShippingFeeIGST = flipShippingFeeIGST;
                                                            view_salereport.flipShippingFeeCGST = flipShippingFeeCGST;
                                                            view_salereport.flipShippingFeeSGST = flipShippingFeeSGST;
                                                        }
                                                    }
                                                }
                                                else if (nam == "Collection Fee")
                                                {
                                                    if (get_expdetails.id == exp_id)
                                                    {
                                                        flipCollection = flipCollection + Convert.ToDouble(item1.expense_amount);
                                                        view_salereport.flipCollection = flipCollection;
                                                        if (gettax_details != null)
                                                        {
                                                            flipCollectionIGST = flipCollectionIGST + Convert.ToDouble(gettax_details.Igst_amount);
                                                            flipCollectionCGST = flipCollectionCGST + Convert.ToDouble(gettax_details.CGST_amount);
                                                            flipCollectionSGST = flipCollectionSGST + Convert.ToDouble(gettax_details.sgst_amount);

                                                            view_salereport.flipCollectionIGST = flipCollectionIGST;
                                                            view_salereport.flipCollectionCGST = flipCollectionCGST;
                                                            view_salereport.flipCollectionSGST = flipCollectionSGST;
                                                        }
                                                    }
                                                }
                                                else if (nam == "Reverse Shipping Fee")
                                                {
                                                    if (get_expdetails.id == exp_id)
                                                    {
                                                        flipReverseShipping = flipReverseShipping + Convert.ToDouble(item1.expense_amount);
                                                        view_salereport.flipReverseShipping = flipReverseShipping;
                                                        if (gettax_details != null)
                                                        {
                                                            flipReverseShippingIGST = flipReverseShippingIGST + Convert.ToDouble(gettax_details.Igst_amount);
                                                            flipReverseShippingCGST = flipReverseShippingCGST + Convert.ToDouble(gettax_details.CGST_amount);
                                                            flipReverseShippingSGST = flipReverseShippingSGST + Convert.ToDouble(gettax_details.sgst_amount);
                                                            view_salereport.flipReverseShippingIGST = flipReverseShippingIGST;
                                                            view_salereport.flipReverseShippingCGST = flipReverseShippingCGST;
                                                            view_salereport.flipReverseShippingSGST = flipReverseShippingSGST;
                                                        }
                                                    }
                                                }
                                                else if (nam == "Fixed Fee")
                                                {
                                                    if (get_expdetails.id == exp_id)
                                                    {
                                                        flipFixedFee = flipFixedFee + Convert.ToDouble(item1.expense_amount);
                                                        view_salereport.flipFixedFee = flipFixedFee;
                                                        if (gettax_details != null)
                                                        {
                                                            flipFixedFeeIGST = flipFixedFeeIGST + Convert.ToDouble(gettax_details.Igst_amount);
                                                            flipFixedFeeCGST = flipFixedFeeCGST + Convert.ToDouble(gettax_details.CGST_amount);
                                                            flipFixedFeeSGST = flipFixedFeeSGST + Convert.ToDouble(gettax_details.sgst_amount);

                                                            view_salereport.flipFixedFeeIGST = flipFixedFeeIGST;
                                                            view_salereport.flipFixedFeeCGST = flipFixedFeeCGST;
                                                            view_salereport.flipFixedFeeSGST = flipFixedFeeSGST;
                                                        }
                                                    }
                                                }
                                                else if (nam == "Commission")
                                                {
                                                    if (get_expdetails.id == exp_id)
                                                    {
                                                        CommissionFee = CommissionFee + Convert.ToDouble(item1.expense_amount);
                                                        view_salereport.CommissionFee = CommissionFee;
                                                        if (gettax_details != null)
                                                        {
                                                            CommissionIGST = CommissionIGST + Convert.ToDouble(gettax_details.Igst_amount);
                                                            CommissionCGST = CommissionCGST + Convert.ToDouble(gettax_details.CGST_amount);
                                                            CommissionSGST = CommissionSGST + Convert.ToDouble(gettax_details.sgst_amount);
                                                            view_salereport.CommissionIGST = CommissionIGST;
                                                            view_salereport.CommissionCGST = CommissionCGST;
                                                            view_salereport.CommissionSGST = CommissionSGST;
                                                        }
                                                    }
                                                }

                                            }// end of if(get_expdetails)
                                        }// end if foreach(item1)
                                    }// end of if(getsettlementdetails) 
                                }
                                iRepeatSettlementData++;
                            }

                            //------------------------------------------End--------------------------------------//
                            #endregion


                            #region return data
                            //-------------------------------------To get Refund data from tbl_History and taxtable------------------------//
                            int iRepeatrefundData = 0;
                            double refundprincipal = 0;
                            double refundproduct_tax = 0;
                            double refundshipping = 0;
                            double refundshipping_tax = 0;
                            double refundgiftwrap = 0;
                            double refundgiftwrap_tax = 0;
                            double refundshipping_discount = 0;
                            double refundshipping_discounttax = 0;
                            var get_historydata = dba.tbl_order_history.Where(a => a.tbl_seller_id == sellers_id && a.OrderID == item.ob_tbl_sales_order.amazon_order_id && a.t_order_status == 9).ToList();
                            if (get_historydata != null)
                            {
                                foreach (var refunditem in get_historydata)
                                {
                                    refundprincipal = refundprincipal + Convert.ToDouble(refunditem.amount_per_unit);
                                    refundproduct_tax = refundproduct_tax + Convert.ToDouble(refunditem.product_tax);
                                    refundshipping = refundshipping + Convert.ToDouble(refunditem.shipping_price);
                                    refundshipping_tax = refundshipping_tax + Convert.ToDouble(refunditem.shipping_tax);
                                    refundgiftwrap = refundgiftwrap + Convert.ToDouble(refunditem.Giftwrap_price);
                                    refundgiftwrap_tax = refundgiftwrap_tax + Convert.ToDouble(refunditem.gift_wrap_tax);
                                    refundshipping_discount = refundshipping_discount + Convert.ToDouble(refunditem.shipping_discount);
                                    refundshipping_discounttax = refundshipping_discounttax + Convert.ToDouble(refunditem.shipping_tax_discount);


                                    view_salereport.refundprincipal = refundprincipal;
                                    view_salereport.refundproduct_tax = refundproduct_tax;
                                    view_salereport.refundshipping = refundshipping;
                                    view_salereport.refundshipping_tax = refundshipping_tax;
                                    view_salereport.refundgiftwrap = refundgiftwrap;
                                    view_salereport.refundgiftwrap_tax = refundgiftwrap_tax;
                                    view_salereport.refundshipping_discount = refundshipping_discount;
                                    view_salereport.refundshipping_discount_tax = refundshipping_discounttax;
                                    view_salereport.refundTotal = view_salereport.refundprincipal + view_salereport.refundproduct_tax + view_salereport.refundshipping + view_salereport.refundshipping_tax + view_salereport.refundgiftwrap + view_salereport.refundgiftwrap_tax + view_salereport.refundshipping_discount + view_salereport.refundshipping_discount_tax;
                                    view_salereport.refundReferenceID = refunditem.settlement_id;


                                    double refund_flipShipping = 0, Refund_flipShippingFeeIGST = 0, Refund_flipShippingFeeCGST = 0, Refund_flipShippingFeeSGST = 0,
                                           refund_flipCollection = 0, Refund_flipCollectionIGST = 0, Refund_flipCollectionCGST = 0, Refund_flipCollectionSGST = 0,
                                           refundCommissionFee = 0, refundCommissionIGST = 0, refundCommissionCGST = 0, refundCommissionSGST = 0,
                                           refund_flipReverseShipping = 0, Refund_flipReverseShippingIGST = 0, Refund_flipReverseShippingCGST = 0, Refund_flipReverseShippingSGST = 0,
                                           refund_flipFixedFee = 0, Refund_flipFixedFeeIGST = 0, Refund_flipFixedFeeCGST = 0, Refund_flipFixedFeeSGST = 0;


                                    var get_refundexpense = dba.m_tbl_expense.Where(a => a.settlement_order_id.Contains(amazon_order_id) && a.tbl_seller_id == sellers_id && a.t_transactionType_id == 2).ToList();// to get refund expense
                                    // var get_refundexpense = dba.m_tbl_expense.Where(a => a.settlement_order_id == get_historydata.unique_order_id && a.tbl_seller_id == sellers_id && a.t_transactionType_id == 2 && a.tbl_order_historyid == get_historydata.Id).ToList();// to get refund expense
                                    if (get_refundexpense != null && get_refundexpense.Count > 0)
                                    {
                                        foreach (var refund in get_refundexpense)
                                        {
                                            var exp_ID = refund.expense_type_id;
                                            var get_details = dba.m_settlement_fee.Where(a => a.id == exp_ID).FirstOrDefault();
                                            var getExp_tax_details = dba.tbl_tax.Where(a => a.tbl_referneced_id == refund.id && a.reference_type == 7).FirstOrDefault();
                                            if (get_details != null)
                                            {
                                                string nam = get_details.return_fee;
                                                if (nam == "Shipping Fee")
                                                {
                                                    if (get_details.id == exp_ID)
                                                    {
                                                        refund_flipShipping = refund_flipShipping + Convert.ToDouble(refund.expense_amount);
                                                        view_salereport.refund_flipShipping = refund_flipShipping;
                                                        if (getExp_tax_details != null)
                                                        {
                                                            Refund_flipShippingFeeIGST = Refund_flipShippingFeeIGST + Convert.ToDouble(getExp_tax_details.Igst_amount);
                                                            Refund_flipShippingFeeCGST = Refund_flipShippingFeeCGST + Convert.ToDouble(getExp_tax_details.CGST_amount);
                                                            Refund_flipShippingFeeSGST = Refund_flipShippingFeeSGST + Convert.ToDouble(getExp_tax_details.sgst_amount);

                                                            view_salereport.Refund_flipShippingFeeIGST = Refund_flipShippingFeeIGST;
                                                            view_salereport.Refund_flipShippingFeeCGST = Refund_flipShippingFeeCGST;
                                                            view_salereport.Refund_flipShippingFeeSGST = Refund_flipShippingFeeSGST;
                                                        }
                                                    }
                                                }
                                                else if (nam == "Collection Fee")
                                                {
                                                    if (get_details.id == exp_ID)
                                                    {
                                                        refund_flipCollection = refund_flipCollection + Convert.ToDouble(refund.expense_amount);
                                                        view_salereport.refund_flipCollection = refund_flipCollection;
                                                        if (getExp_tax_details != null)
                                                        {
                                                            Refund_flipCollectionIGST = Refund_flipCollectionIGST + Convert.ToDouble(getExp_tax_details.Igst_amount);
                                                            Refund_flipCollectionCGST = Refund_flipCollectionCGST + Convert.ToDouble(getExp_tax_details.CGST_amount);
                                                            Refund_flipCollectionSGST = Refund_flipCollectionSGST + Convert.ToDouble(getExp_tax_details.sgst_amount);

                                                            view_salereport.Refund_flipCollectionIGST = Refund_flipCollectionIGST;
                                                            view_salereport.Refund_flipCollectionCGST = Refund_flipCollectionCGST;
                                                            view_salereport.Refund_flipCollectionSGST = Refund_flipCollectionSGST;
                                                        }
                                                    }
                                                }
                                                else if (nam == "Reverse Shipping Fee")
                                                {
                                                    if (get_details.id == exp_ID)
                                                    {
                                                        refund_flipReverseShipping = refund_flipReverseShipping + Convert.ToDouble(refund.expense_amount);
                                                        view_salereport.refund_flipReverseShipping = refund_flipReverseShipping;
                                                        if (getExp_tax_details != null)
                                                        {
                                                            Refund_flipReverseShippingIGST = Refund_flipReverseShippingIGST + Convert.ToDouble(getExp_tax_details.Igst_amount);
                                                            Refund_flipReverseShippingCGST = Refund_flipReverseShippingCGST + Convert.ToDouble(getExp_tax_details.CGST_amount);
                                                            Refund_flipReverseShippingSGST = Refund_flipReverseShippingSGST + Convert.ToDouble(getExp_tax_details.sgst_amount);

                                                            view_salereport.Refund_flipReverseShippingIGST = Refund_flipReverseShippingIGST;
                                                            view_salereport.Refund_flipReverseShippingCGST = Refund_flipReverseShippingCGST;
                                                            view_salereport.Refund_flipReverseShippingSGST = Refund_flipReverseShippingSGST;
                                                        }
                                                    }
                                                }
                                                else if (nam == "Fixed Fee")
                                                {
                                                    if (get_details.id == exp_ID)
                                                    {
                                                        refund_flipFixedFee = refund_flipFixedFee + Convert.ToDouble(refund.expense_amount);
                                                        view_salereport.refund_flipFixedFee = refund_flipFixedFee;
                                                        if (getExp_tax_details != null)
                                                        {
                                                            Refund_flipFixedFeeIGST = Refund_flipFixedFeeIGST + Convert.ToDouble(getExp_tax_details.Igst_amount);
                                                            Refund_flipFixedFeeCGST = Refund_flipFixedFeeCGST + Convert.ToDouble(getExp_tax_details.CGST_amount);
                                                            Refund_flipFixedFeeSGST = Refund_flipFixedFeeSGST + Convert.ToDouble(getExp_tax_details.sgst_amount);

                                                            view_salereport.Refund_flipFixedFeeIGST = Refund_flipFixedFeeIGST;
                                                            view_salereport.Refund_flipFixedFeeCGST = Refund_flipFixedFeeCGST;
                                                            view_salereport.Refund_flipFixedFeeSGST = Refund_flipFixedFeeSGST;
                                                        }
                                                    }
                                                }
                                                else if (nam == "Commission")
                                                {
                                                    if (get_details.id == exp_ID)
                                                    {
                                                        refundCommissionFee = refundCommissionFee + Convert.ToDouble(refund.expense_amount);
                                                        view_salereport.RefundCommissionFee = refundCommissionFee;
                                                        if (getExp_tax_details != null)
                                                        {
                                                            refundCommissionIGST = refundCommissionIGST + Convert.ToDouble(getExp_tax_details.Igst_amount);
                                                            refundCommissionCGST = refundCommissionCGST + Convert.ToDouble(getExp_tax_details.CGST_amount);
                                                            refundCommissionSGST = refundCommissionSGST + Convert.ToDouble(getExp_tax_details.sgst_amount);

                                                            view_salereport.RefundCommissionIGST = refundCommissionIGST;
                                                            view_salereport.RefundCommissionCGST = refundCommissionCGST;
                                                            view_salereport.RefundCommissionSGST = refundCommissionSGST;
                                                        }
                                                    }
                                                }

                                            }// end of if(get_details)                               
                                        }// end of foreach(refund)
                                    }// end of if(get_refundexpense)
                                }// end of froeach loop()
                                iRepeatrefundData++;
                            }// end of if(get_historydata)

                            #endregion


                            view_salereport.Refund_SUMIGST = view_salereport.Refund_flipShippingFeeIGST + view_salereport.Refund_flipCollectionIGST + view_salereport.Refund_flipReverseShippingIGST + view_salereport.Refund_flipFixedFeeIGST + view_salereport.RefundCommissionIGST;
                            view_salereport.Refund_SUMCGST = view_salereport.Refund_flipShippingFeeCGST + view_salereport.Refund_flipCollectionCGST + view_salereport.Refund_flipReverseShippingCGST + view_salereport.Refund_flipFixedFeeCGST + view_salereport.RefundCommissionCGST;
                            view_salereport.Refund_SUMSGST = view_salereport.Refund_flipShippingFeeSGST + view_salereport.Refund_flipCollectionSGST + view_salereport.Refund_flipReverseShippingSGST + view_salereport.Refund_flipFixedFeeSGST + view_salereport.RefundCommissionSGST;
                            view_salereport.refund_SumFee = view_salereport.refund_flipShipping + view_salereport.refund_flipCollection + view_salereport.refund_flipReverseShipping + view_salereport.refund_flipFixedFee + view_salereport.RefundCommissionFee + view_salereport.Refund_SUMIGST + view_salereport.Refund_SUMCGST + view_salereport.Refund_SUMSGST;
                            view_salereport.refund_SumOrder = view_salereport.refundTotal + view_salereport.refund_SumFee;
                            /////////-----------------------------------------End----------------------------------------------------------//

                            //sharad
                            view_salereport.SUMIGST = view_salereport.flipShippingFeeIGST + view_salereport.flipCollectionIGST + view_salereport.flipReverseShippingIGST + view_salereport.flipFixedFeeIGST + view_salereport.CommissionIGST;
                            view_salereport.SUMCGST = view_salereport.flipShippingFeeCGST + view_salereport.flipCollectionCGST + view_salereport.flipReverseShippingCGST + view_salereport.flipFixedFeeCGST + view_salereport.CommissionCGST;
                            view_salereport.SUMSGST = view_salereport.flipShippingFeeSGST + view_salereport.flipCollectionSGST + view_salereport.flipReverseShippingSGST + view_salereport.flipFixedFeeSGST + view_salereport.CommissionSGST;

                            view_salereport.SumFee = view_salereport.flipShipping + view_salereport.flipCollection + view_salereport.flipReverseShipping + view_salereport.flipFixedFee + view_salereport.CommissionFee + view_salereport.SUMIGST + view_salereport.SUMCGST + view_salereport.SUMSGST;
                            view_salereport.SumOrder = view_salereport.orderTotal + view_salereport.SumFee;

                            string Value = "";
                            Value = (view_salereport.SumOrder + view_salereport.refund_SumOrder).ToString("0.00");
                            view_salereport.NetTotal = Convert.ToDouble(Value);
                            view_salereport.Profit_lossAmount = view_salereport.NetTotal - view_salereport.ProductValue;

                            string value = "";
                            value = Convert.ToString((view_salereport.NetTotal / view_salereport.orderTotal) * 100);

                            if (value != "NaN" && value != "-Infinity")
                            {
                                decimal abc = Convert.ToDecimal((view_salereport.NetTotal / view_salereport.orderTotal) * 100);
                                decimal result = decimal.Round(abc, 2, MidpointRounding.AwayFromZero);

                                string bb = value.ToString();
                                string[] b = bb.Split('.');
                                int firstValue = int.Parse(b[0]);

                                string fvalue = result + "%";
                                view_salereport.PercentageAmount = Convert.ToString(fvalue);
                            }
                            i++;
                        }// end of if(item !=  null)
                    }// end of foreach(GetSaleOrderDetail)
                    if (view_salereport.OrderID != null)
                    {
                        lstOrdertext2.Add(view_salereport);
                    }
                }// end of main if
            }
            catch (Exception ex)
            {
            }
            return Msg;
        }


        #region PaytmSummary Realization
        public string Get_PaytmSummaryRealizationReport(List<SaleReport> lstOrdertext2, SaleReport view_salereport, int? ddl_market_place, DateTime? txt_from, DateTime? txt_to, int sellers_id)
        {
            string Msg = "S";
            try
            {
                if (txt_from != null && txt_to != null)
                {
                    var GetSaleOrderDetail = (from ob_tbl_sales_order in dba.tbl_sales_order
                                              select new
                                              {
                                                  ob_tbl_sales_order = ob_tbl_sales_order
                                              }).Where(a => a.ob_tbl_sales_order.tbl_sellers_id == sellers_id && a.ob_tbl_sales_order.tbl_Marketplace_Id == ddl_market_place).ToList();
                    int i = 0;
                    view_salereport = new SaleReport();
                    if (GetSaleOrderDetail != null)
                    {
                        if (txt_from != null && txt_to != null)
                        {
                            GetSaleOrderDetail = GetSaleOrderDetail.Where(a => a.ob_tbl_sales_order.purchase_date.Date >= txt_from && a.ob_tbl_sales_order.purchase_date.Date <= txt_to).ToList();
                        }
                    }
                    foreach (var item in GetSaleOrderDetail)
                    {
                        int iRepeatDetailData = 0;
                        if (item != null)
                        {
                            if (view_salereport.OrderID != item.ob_tbl_sales_order.amazon_order_id)
                            {
                                if (view_salereport.OrderID != null)
                                    lstOrdertext2.Add(view_salereport);
                            }
                            int fillItemCount = lstOrdertext2.Count;
                            if (fillItemCount > 0)
                            {
                                if (lstOrdertext2[fillItemCount - 1].OrderID != item.ob_tbl_sales_order.amazon_order_id && view_salereport.OrderID != item.ob_tbl_sales_order.amazon_order_id)
                                    view_salereport = new SaleReport();
                            }


                            string amazon_order_id = item.ob_tbl_sales_order.amazon_order_id;
                            view_salereport.OrderID = amazon_order_id; //item.ob_tbl_sales_order.amazon_order_id;
                            view_salereport.OrderDate = Convert.ToDateTime(item.ob_tbl_sales_order.purchase_date).ToString("yyyy-MM-dd");
                            // to get sale order details data from  sale order id 

                            double Principal = 0;
                            double orderigst = 0;
                            double ordersgst = 0;
                            double ordercgst = 0, itemprice = 0;
                            string itemname = "", sku = "";
                            var get_saleorder_details = dba.tbl_sales_order_details.Where(a => a.tbl_sales_order_id == item.ob_tbl_sales_order.id && a.tbl_seller_id == sellers_id).ToList();//to get sale_order details 
                            if (get_saleorder_details != null)
                            {
                                foreach (var order_details in get_saleorder_details)
                                {
                                    double ord_detail_Principal = order_details.item_price_amount;
                                    Principal = Principal + ord_detail_Principal;
                                    view_salereport.Principal = Principal;
                                    var get_tax = dba.tbl_tax.Where(a => a.tbl_referneced_id == order_details.id && a.reference_type == 3).FirstOrDefault();
                                    {
                                        if (get_tax != null)
                                        {
                                            orderigst = orderigst + Convert.ToDouble(get_tax.Igst_amount);
                                            ordersgst = ordersgst + Convert.ToDouble(get_tax.sgst_amount);
                                            ordercgst = ordercgst + Convert.ToDouble(get_tax.CGST_amount);

                                            view_salereport.orderigst = orderigst;
                                            view_salereport.ordersgst = ordersgst;
                                            view_salereport.ordercgst = ordercgst;
                                        }
                                    }
                                    view_salereport.ordertotal = view_salereport.Principal + view_salereport.orderigst + view_salereport.ordersgst + view_salereport.ordercgst;
                                    //------------------------------------Get inventory details -------------------------------------------//
                                    var get_inventory = dba.tbl_inventory.Where(a => a.tbl_sellers_id == sellers_id && a.sku.ToLower() == order_details.sku_no.ToLower()).FirstOrDefault();
                                    if (get_inventory != null)
                                    {
                                        itemname = get_inventory.item_name;
                                        sku = get_inventory.sku;

                                        var get_inventoryeffective = dba.tbl_inventory_effectiveprice.Where(a => a.tbl_sellerid == sellers_id && a.tbl_inventory_id == get_inventory.id).OrderByDescending(a => a.Id).FirstOrDefault();
                                        if (get_inventoryeffective != null)
                                        {
                                            itemprice = Convert.ToDouble(get_inventoryeffective.Effective_price);
                                        }
                                        //itemprice = Convert.ToDouble(get_inventory.t_effectiveBought_price);
                                        view_salereport.ProductName = itemname;
                                        view_salereport.ProductValue = itemprice;
                                        view_salereport.skuNo = sku;
                                    }
                                    //------------------------------------------------End--------------------------------------//
                                }
                                iRepeatDetailData++;
                            }
                            ///////--------------End------/////////////

                            #region settlementdata
                            // -----------------------get data from tbl_settlement from order-ID -------------------//
                            int iRepeatSettlementData = 0;
                            double orderprincipal = 0;
                            double ordershipping = 0;
                            double ordergiftwrap = 0;
                            double orderproduct_tax = 0;
                            double ordergiftwrap_tax = 0;
                            double ordershipping_tax = 0;
                            double ordershipping_discount = 0;
                            double ordershipping_discounttax = 0;

                            var get_settlementdata = dba.tbl_settlement_order.Where(a => a.Order_Id == item.ob_tbl_sales_order.amazon_order_id && a.tbl_seller_id == sellers_id).ToList();
                            //string uniqueorderid = "";
                            if (get_settlementdata != null)
                            {
                                foreach (var settle in get_settlementdata)
                                {
                                    //string uniqueorderid = "";
                                    orderprincipal = orderprincipal + Convert.ToDouble(settle.principal_price);
                                    orderproduct_tax = orderproduct_tax + Convert.ToDouble(settle.product_tax);
                                    ordershipping = ordershipping + Convert.ToDouble(settle.shipping_price);
                                    ordershipping_tax = ordershipping_tax + Convert.ToDouble(settle.shipping_tax);
                                    ordergiftwrap = ordergiftwrap + Convert.ToDouble(settle.giftwrap_price);
                                    ordergiftwrap_tax = ordergiftwrap_tax + Convert.ToDouble(settle.giftwarp_tax);
                                    ordershipping_discount = ordershipping_discount + Convert.ToDouble(settle.shipping_discount);
                                    ordershipping_discounttax = ordershipping_discounttax + Convert.ToDouble(settle.shipping_tax_discount);

                                    view_salereport.orderprincipal = orderprincipal;
                                    view_salereport.orderproduct_tax = orderproduct_tax;
                                    view_salereport.ordershipping = ordershipping;
                                    view_salereport.ordershipping_tax = ordershipping_tax;
                                    view_salereport.ordergiftwrap = ordergiftwrap;
                                    view_salereport.ordergiftwrap_tax = ordergiftwrap_tax;
                                    view_salereport.ordershipping_discount = ordershipping_discount;
                                    view_salereport.ordershipping_discounttax = ordershipping_discounttax;

                                    view_salereport.orderTotal = view_salereport.orderprincipal + view_salereport.orderproduct_tax + view_salereport.ordershipping + view_salereport.ordershipping_tax + view_salereport.ordergiftwrap + view_salereport.ordergiftwrap_tax + view_salereport.ordershipping_discount + view_salereport.ordershipping_discounttax;
                                    view_salereport.ReferenceID = settle.settlement_id;
                                    //uniqueorderid = settle.unique_order_id;
                                    //    }
                                    //    iRepeatSettlementData++;
                                    //}
                                    //-------------------------End---------------------------//
                                    //-----------------------Get  data from tbl_Expense from settlement_Id----------------//

                                    double flipShipping = 0, flipShippingFeeIGST = 0, flipShippingFeeCGST = 0, flipShippingFeeSGST = 0, flipCollection = 0, flipCollectionIGST = 0, flipCollectionSGST = 0, flipCollectionCGST = 0,
                                           CommissionFee = 0, CommissionIGST = 0, CommissionCGST = 0, CommissionSGST = 0,
                                           flipReverseShipping = 0, flipReverseShippingIGST = 0, flipReverseShippingCGST = 0, flipReverseShippingSGST = 0,
                                           flipFixedFee = 0, flipFixedFeeIGST = 0, flipFixedFeeCGST = 0, flipFixedFeeSGST = 0;

                                    var getsettlementdetails = dba.m_tbl_expense.Where(a => a.settlement_order_id.Contains(amazon_order_id) && a.tbl_seller_id == sellers_id && a.t_transactionType_id == 1).ToList();
                                    if (getsettlementdetails != null && getsettlementdetails.Count > 0)
                                    {
                                        foreach (var item1 in getsettlementdetails)
                                        {
                                            var exp_id = item1.expense_type_id;
                                            var get_expdetails = dba.m_settlement_fee.Where(a => a.id == exp_id).FirstOrDefault();
                                            var gettax_details = dba.tbl_tax.Where(a => a.tbl_referneced_id == item1.id && a.reference_type == 2).FirstOrDefault();
                                            if (get_expdetails != null)
                                            {
                                                string nam = get_expdetails.return_fee;
                                                if (nam == "Marketplace Commission")
                                                {
                                                    if (get_expdetails.id == exp_id)
                                                    {
                                                        flipShipping = flipShipping + Convert.ToDouble(item1.expense_amount);
                                                        view_salereport.flipShipping = flipShipping;
                                                        if (gettax_details != null)
                                                        {
                                                            flipShippingFeeIGST = flipShippingFeeIGST + Convert.ToDouble(gettax_details.Igst_amount);
                                                            flipShippingFeeCGST = flipShippingFeeCGST + Convert.ToDouble(gettax_details.CGST_amount);
                                                            flipShippingFeeSGST = flipShippingFeeSGST + Convert.ToDouble(gettax_details.sgst_amount);

                                                            view_salereport.flipShippingFeeIGST = flipShippingFeeIGST;
                                                            view_salereport.flipShippingFeeCGST = flipShippingFeeCGST;
                                                            view_salereport.flipShippingFeeSGST = flipShippingFeeSGST;
                                                        }
                                                    }
                                                }
                                                else if (nam == "Logistics Charges")
                                                {
                                                    if (get_expdetails.id == exp_id)
                                                    {
                                                        flipCollection = flipCollection + Convert.ToDouble(item1.expense_amount);
                                                        view_salereport.flipCollection = flipCollection;
                                                        if (gettax_details != null)
                                                        {
                                                            flipCollectionIGST = flipCollectionIGST + Convert.ToDouble(gettax_details.Igst_amount);
                                                            flipCollectionCGST = flipCollectionCGST + Convert.ToDouble(gettax_details.CGST_amount);
                                                            flipCollectionSGST = flipCollectionSGST + Convert.ToDouble(gettax_details.sgst_amount);

                                                            view_salereport.flipCollectionIGST = flipCollectionIGST;
                                                            view_salereport.flipCollectionCGST = flipCollectionCGST;
                                                            view_salereport.flipCollectionSGST = flipCollectionSGST;
                                                        }
                                                    }
                                                }
                                                else if (nam == "PG Commission")
                                                {
                                                    if (get_expdetails.id == exp_id)
                                                    {
                                                        flipReverseShipping = flipReverseShipping + Convert.ToDouble(item1.expense_amount);
                                                        view_salereport.flipReverseShipping = flipReverseShipping;
                                                        if (gettax_details != null)
                                                        {
                                                            flipReverseShippingIGST = flipReverseShippingIGST + Convert.ToDouble(gettax_details.Igst_amount);
                                                            flipReverseShippingCGST = flipReverseShippingCGST + Convert.ToDouble(gettax_details.CGST_amount);
                                                            flipReverseShippingSGST = flipReverseShippingSGST + Convert.ToDouble(gettax_details.sgst_amount);
                                                            view_salereport.flipReverseShippingIGST = flipReverseShippingIGST;
                                                            view_salereport.flipReverseShippingCGST = flipReverseShippingCGST;
                                                            view_salereport.flipReverseShippingSGST = flipReverseShippingSGST;
                                                        }
                                                    }
                                                }
                                                else if (nam == "Penalty")
                                                {
                                                    if (get_expdetails.id == exp_id)
                                                    {
                                                        flipFixedFee = flipFixedFee + Convert.ToDouble(item1.expense_amount);
                                                        view_salereport.flipFixedFee = flipFixedFee;
                                                        if (gettax_details != null)
                                                        {
                                                            flipFixedFeeIGST = flipFixedFeeIGST + Convert.ToDouble(gettax_details.Igst_amount);
                                                            flipFixedFeeCGST = flipFixedFeeCGST + Convert.ToDouble(gettax_details.CGST_amount);
                                                            flipFixedFeeSGST = flipFixedFeeSGST + Convert.ToDouble(gettax_details.sgst_amount);

                                                            view_salereport.flipFixedFeeIGST = flipFixedFeeIGST;
                                                            view_salereport.flipFixedFeeCGST = flipFixedFeeCGST;
                                                            view_salereport.flipFixedFeeSGST = flipFixedFeeSGST;
                                                        }
                                                    }
                                                }
                                                else if (nam == "Net Adjustments")
                                                {
                                                    if (get_expdetails.id == exp_id)
                                                    {
                                                        CommissionFee = CommissionFee + Convert.ToDouble(item1.expense_amount);
                                                        view_salereport.CommissionFee = CommissionFee;
                                                        if (gettax_details != null)
                                                        {
                                                            CommissionIGST = CommissionIGST + Convert.ToDouble(gettax_details.Igst_amount);
                                                            CommissionCGST = CommissionCGST + Convert.ToDouble(gettax_details.CGST_amount);
                                                            CommissionSGST = CommissionSGST + Convert.ToDouble(gettax_details.sgst_amount);
                                                            view_salereport.CommissionIGST = CommissionIGST;
                                                            view_salereport.CommissionCGST = CommissionCGST;
                                                            view_salereport.CommissionSGST = CommissionSGST;
                                                        }
                                                    }
                                                }

                                            }// end of if(get_expdetails)
                                        }// end if foreach(item1)
                                    }// end of if(getsettlementdetails) 
                                }
                                iRepeatSettlementData++;
                            }

                            //------------------------------------------End--------------------------------------//
                            #endregion


                            #region return data
                            //-------------------------------------To get Refund data from tbl_History and taxtable------------------------//
                            int iRepeatrefundData = 0;
                            double refundprincipal = 0;
                            double refundproduct_tax = 0;
                            double refundshipping = 0;
                            double refundshipping_tax = 0;
                            double refundgiftwrap = 0;
                            double refundgiftwrap_tax = 0;
                            double refundshipping_discount = 0;
                            double refundshipping_discounttax = 0;
                            var get_historydata = dba.tbl_order_history.Where(a => a.tbl_seller_id == sellers_id && a.OrderID == item.ob_tbl_sales_order.amazon_order_id && a.t_order_status == 9).ToList();
                            if (get_historydata != null)
                            {
                                foreach (var refunditem in get_historydata)
                                {
                                    refundprincipal = refundprincipal + Convert.ToDouble(refunditem.amount_per_unit);
                                    refundproduct_tax = refundproduct_tax + Convert.ToDouble(refunditem.product_tax);
                                    refundshipping = refundshipping + Convert.ToDouble(refunditem.shipping_price);
                                    refundshipping_tax = refundshipping_tax + Convert.ToDouble(refunditem.shipping_tax);
                                    refundgiftwrap = refundgiftwrap + Convert.ToDouble(refunditem.Giftwrap_price);
                                    refundgiftwrap_tax = refundgiftwrap_tax + Convert.ToDouble(refunditem.gift_wrap_tax);
                                    refundshipping_discount = refundshipping_discount + Convert.ToDouble(refunditem.shipping_discount);
                                    refundshipping_discounttax = refundshipping_discounttax + Convert.ToDouble(refunditem.shipping_tax_discount);


                                    view_salereport.refundprincipal = refundprincipal;
                                    view_salereport.refundproduct_tax = refundproduct_tax;
                                    view_salereport.refundshipping = refundshipping;
                                    view_salereport.refundshipping_tax = refundshipping_tax;
                                    view_salereport.refundgiftwrap = refundgiftwrap;
                                    view_salereport.refundgiftwrap_tax = refundgiftwrap_tax;
                                    view_salereport.refundshipping_discount = refundshipping_discount;
                                    view_salereport.refundshipping_discount_tax = refundshipping_discounttax;
                                    view_salereport.refundTotal = view_salereport.refundprincipal + view_salereport.refundproduct_tax + view_salereport.refundshipping + view_salereport.refundshipping_tax + view_salereport.refundgiftwrap + view_salereport.refundgiftwrap_tax + view_salereport.refundshipping_discount + view_salereport.refundshipping_discount_tax;
                                    view_salereport.refundReferenceID = refunditem.settlement_id;


                                    double refund_flipShipping = 0, Refund_flipShippingFeeIGST = 0, Refund_flipShippingFeeCGST = 0, Refund_flipShippingFeeSGST = 0,
                                           refund_flipCollection = 0, Refund_flipCollectionIGST = 0, Refund_flipCollectionCGST = 0, Refund_flipCollectionSGST = 0,
                                           refundCommissionFee = 0, refundCommissionIGST = 0, refundCommissionCGST = 0, refundCommissionSGST = 0,
                                           refund_flipReverseShipping = 0, Refund_flipReverseShippingIGST = 0, Refund_flipReverseShippingCGST = 0, Refund_flipReverseShippingSGST = 0,
                                           refund_flipFixedFee = 0, Refund_flipFixedFeeIGST = 0, Refund_flipFixedFeeCGST = 0, Refund_flipFixedFeeSGST = 0;


                                    var get_refundexpense = dba.m_tbl_expense.Where(a => a.settlement_order_id.Contains(amazon_order_id) && a.tbl_seller_id == sellers_id && a.t_transactionType_id == 2).ToList();// to get refund expense
                                    // var get_refundexpense = dba.m_tbl_expense.Where(a => a.settlement_order_id == get_historydata.unique_order_id && a.tbl_seller_id == sellers_id && a.t_transactionType_id == 2 && a.tbl_order_historyid == get_historydata.Id).ToList();// to get refund expense
                                    if (get_refundexpense != null && get_refundexpense.Count > 0)
                                    {
                                        foreach (var refund in get_refundexpense)
                                        {
                                            var exp_ID = refund.expense_type_id;
                                            var get_details = dba.m_settlement_fee.Where(a => a.id == exp_ID).FirstOrDefault();
                                            var getExp_tax_details = dba.tbl_tax.Where(a => a.tbl_referneced_id == refund.id && a.reference_type == 7).FirstOrDefault();
                                            if (get_details != null)
                                            {
                                                string nam = get_details.return_fee;
                                                if (nam == "Marketplace Commission")
                                                {
                                                    if (get_details.id == exp_ID)
                                                    {
                                                        refund_flipShipping = refund_flipShipping + Convert.ToDouble(refund.expense_amount);
                                                        view_salereport.refund_flipShipping = refund_flipShipping;
                                                        if (getExp_tax_details != null)
                                                        {
                                                            Refund_flipShippingFeeIGST = Refund_flipShippingFeeIGST + Convert.ToDouble(getExp_tax_details.Igst_amount);
                                                            Refund_flipShippingFeeCGST = Refund_flipShippingFeeCGST + Convert.ToDouble(getExp_tax_details.CGST_amount);
                                                            Refund_flipShippingFeeSGST = Refund_flipShippingFeeSGST + Convert.ToDouble(getExp_tax_details.sgst_amount);

                                                            view_salereport.Refund_flipShippingFeeIGST = Refund_flipShippingFeeIGST;
                                                            view_salereport.Refund_flipShippingFeeCGST = Refund_flipShippingFeeCGST;
                                                            view_salereport.Refund_flipShippingFeeSGST = Refund_flipShippingFeeSGST;
                                                        }
                                                    }
                                                }
                                                else if (nam == "Logistics Charges")
                                                {
                                                    if (get_details.id == exp_ID)
                                                    {
                                                        refund_flipCollection = refund_flipCollection + Convert.ToDouble(refund.expense_amount);
                                                        view_salereport.refund_flipCollection = refund_flipCollection;
                                                        if (getExp_tax_details != null)
                                                        {
                                                            Refund_flipCollectionIGST = Refund_flipCollectionIGST + Convert.ToDouble(getExp_tax_details.Igst_amount);
                                                            Refund_flipCollectionCGST = Refund_flipCollectionCGST + Convert.ToDouble(getExp_tax_details.CGST_amount);
                                                            Refund_flipCollectionSGST = Refund_flipCollectionSGST + Convert.ToDouble(getExp_tax_details.sgst_amount);

                                                            view_salereport.Refund_flipCollectionIGST = Refund_flipCollectionIGST;
                                                            view_salereport.Refund_flipCollectionCGST = Refund_flipCollectionCGST;
                                                            view_salereport.Refund_flipCollectionSGST = Refund_flipCollectionSGST;
                                                        }
                                                    }
                                                }
                                                else if (nam == "PG Commission")
                                                {
                                                    if (get_details.id == exp_ID)
                                                    {
                                                        refund_flipReverseShipping = refund_flipReverseShipping + Convert.ToDouble(refund.expense_amount);
                                                        view_salereport.refund_flipReverseShipping = refund_flipReverseShipping;
                                                        if (getExp_tax_details != null)
                                                        {
                                                            Refund_flipReverseShippingIGST = Refund_flipReverseShippingIGST + Convert.ToDouble(getExp_tax_details.Igst_amount);
                                                            Refund_flipReverseShippingCGST = Refund_flipReverseShippingCGST + Convert.ToDouble(getExp_tax_details.CGST_amount);
                                                            Refund_flipReverseShippingSGST = Refund_flipReverseShippingSGST + Convert.ToDouble(getExp_tax_details.sgst_amount);

                                                            view_salereport.Refund_flipReverseShippingIGST = Refund_flipReverseShippingIGST;
                                                            view_salereport.Refund_flipReverseShippingCGST = Refund_flipReverseShippingCGST;
                                                            view_salereport.Refund_flipReverseShippingSGST = Refund_flipReverseShippingSGST;
                                                        }
                                                    }
                                                }
                                                else if (nam == "Penalty")
                                                {
                                                    if (get_details.id == exp_ID)
                                                    {
                                                        refund_flipFixedFee = refund_flipFixedFee + Convert.ToDouble(refund.expense_amount);
                                                        view_salereport.refund_flipFixedFee = refund_flipFixedFee;
                                                        if (getExp_tax_details != null)
                                                        {
                                                            Refund_flipFixedFeeIGST = Refund_flipFixedFeeIGST + Convert.ToDouble(getExp_tax_details.Igst_amount);
                                                            Refund_flipFixedFeeCGST = Refund_flipFixedFeeCGST + Convert.ToDouble(getExp_tax_details.CGST_amount);
                                                            Refund_flipFixedFeeSGST = Refund_flipFixedFeeSGST + Convert.ToDouble(getExp_tax_details.sgst_amount);

                                                            view_salereport.Refund_flipFixedFeeIGST = Refund_flipFixedFeeIGST;
                                                            view_salereport.Refund_flipFixedFeeCGST = Refund_flipFixedFeeCGST;
                                                            view_salereport.Refund_flipFixedFeeSGST = Refund_flipFixedFeeSGST;
                                                        }
                                                    }
                                                }
                                                else if (nam == "Net Adjustments")
                                                {
                                                    if (get_details.id == exp_ID)
                                                    {
                                                        refundCommissionFee = refundCommissionFee + Convert.ToDouble(refund.expense_amount);
                                                        view_salereport.RefundCommissionFee = refundCommissionFee;
                                                        if (getExp_tax_details != null)
                                                        {
                                                            refundCommissionIGST = refundCommissionIGST + Convert.ToDouble(getExp_tax_details.Igst_amount);
                                                            refundCommissionCGST = refundCommissionCGST + Convert.ToDouble(getExp_tax_details.CGST_amount);
                                                            refundCommissionSGST = refundCommissionSGST + Convert.ToDouble(getExp_tax_details.sgst_amount);

                                                            view_salereport.RefundCommissionIGST = refundCommissionIGST;
                                                            view_salereport.RefundCommissionCGST = refundCommissionCGST;
                                                            view_salereport.RefundCommissionSGST = refundCommissionSGST;
                                                        }
                                                    }
                                                }

                                            }// end of if(get_details)                               
                                        }// end of foreach(refund)
                                    }// end of if(get_refundexpense)
                                }// end of froeach loop()
                                iRepeatrefundData++;
                            }// end of if(get_historydata)

                            #endregion


                            view_salereport.Refund_SUMIGST = view_salereport.Refund_flipShippingFeeIGST + view_salereport.Refund_flipCollectionIGST + view_salereport.Refund_flipReverseShippingIGST + view_salereport.Refund_flipFixedFeeIGST + view_salereport.RefundCommissionIGST;
                            view_salereport.Refund_SUMCGST = view_salereport.Refund_flipShippingFeeCGST + view_salereport.Refund_flipCollectionCGST + view_salereport.Refund_flipReverseShippingCGST + view_salereport.Refund_flipFixedFeeCGST + view_salereport.RefundCommissionCGST;
                            view_salereport.Refund_SUMSGST = view_salereport.Refund_flipShippingFeeSGST + view_salereport.Refund_flipCollectionSGST + view_salereport.Refund_flipReverseShippingSGST + view_salereport.Refund_flipFixedFeeSGST + view_salereport.RefundCommissionSGST;

                            decimal aas = Convert.ToDecimal(view_salereport.refund_flipShipping + view_salereport.refund_flipCollection + view_salereport.refund_flipReverseShipping + view_salereport.refund_flipFixedFee + view_salereport.RefundCommissionFee + view_salereport.Refund_SUMIGST + view_salereport.Refund_SUMCGST + view_salereport.Refund_SUMSGST);
                            decimal result23 = decimal.Round(aas, 2, MidpointRounding.AwayFromZero);
                            view_salereport.refund_SumFee = Convert.ToDouble(result23);//view_salereport.refund_flipShipping + view_salereport.refund_flipCollection + view_salereport.refund_flipReverseShipping + view_salereport.refund_flipFixedFee + view_salereport.RefundCommissionFee + view_salereport.Refund_SUMIGST + view_salereport.Refund_SUMCGST + view_salereport.Refund_SUMSGST;
                            view_salereport.refund_SumOrder = view_salereport.refundTotal + view_salereport.refund_SumFee;
                            /////////-----------------------------------------End----------------------------------------------------------//

                            //sharad
                            view_salereport.SUMIGST = view_salereport.flipShippingFeeIGST + view_salereport.flipCollectionIGST + view_salereport.flipReverseShippingIGST + view_salereport.flipFixedFeeIGST + view_salereport.CommissionIGST;
                            view_salereport.SUMCGST = view_salereport.flipShippingFeeCGST + view_salereport.flipCollectionCGST + view_salereport.flipReverseShippingCGST + view_salereport.flipFixedFeeCGST + view_salereport.CommissionCGST;
                            view_salereport.SUMSGST = view_salereport.flipShippingFeeSGST + view_salereport.flipCollectionSGST + view_salereport.flipReverseShippingSGST + view_salereport.flipFixedFeeSGST + view_salereport.CommissionSGST;

                            view_salereport.SumFee = view_salereport.flipShipping + view_salereport.flipCollection + view_salereport.flipReverseShipping + view_salereport.flipFixedFee + view_salereport.CommissionFee + view_salereport.SUMIGST + view_salereport.SUMCGST + view_salereport.SUMSGST;
                            view_salereport.SumOrder = view_salereport.orderTotal + view_salereport.SumFee;

                            string Value = "";
                            Value = (view_salereport.SumOrder + view_salereport.refund_SumOrder).ToString("0.00");
                            view_salereport.NetTotal = Convert.ToDouble(Value);
                            view_salereport.Profit_lossAmount = view_salereport.NetTotal - view_salereport.ProductValue;

                            string value = "";
                            value = Convert.ToString((view_salereport.NetTotal / view_salereport.orderTotal) * 100);

                            if (value != "NaN" && value != "-Infinity" && value != "Infinity")
                            {
                                decimal abc = Convert.ToDecimal((view_salereport.NetTotal / view_salereport.orderTotal) * 100);
                                decimal result = decimal.Round(abc, 2, MidpointRounding.AwayFromZero);

                                string bb = value.ToString();
                                string[] b = bb.Split('.');
                                int firstValue = int.Parse(b[0]);

                                string fvalue = result + "%";
                                view_salereport.PercentageAmount = Convert.ToString(fvalue);
                            }
                            i++;
                        }// end of if(item !=  null)
                    }// end of foreach(GetSaleOrderDetail)
                    if (view_salereport.OrderID != null)
                    {
                        lstOrdertext2.Add(view_salereport);
                    }
                }// end of main if
            }
            catch (Exception ex)
            {
            }
            return Msg;
        }
        #endregion
        /// <summary>
        /// this is for fetching  records related to Amazon Net Realization without tax
        /// </summary>
        /// <param name="lstOrdertext2"></param>
        /// <param name="view_salereport"></param>
        /// <param name="ddl_market_place"></param>
        /// <param name="txt_from"></param>
        /// <param name="txt_to"></param>
        /// <param name="sellers_id"></param>
        /// <returns></returns>
        public string Get_AmazonNetRealizationWithOutTax(List<SaleReport> lstOrdertext2, SaleReport view_salereport, int? ddl_market_place, DateTime? txt_from, DateTime? txt_to, int sellers_id)
        {
            string Msg = "S";
            try
            {
                if (txt_from != null && txt_to != null)
                {
                    var GetSaleOrderDetail = (from ob_tbl_sales_order in dba.tbl_sales_order
                                              select new
                                              {
                                                  ob_tbl_sales_order = ob_tbl_sales_order

                                              }).Where(a => a.ob_tbl_sales_order.tbl_sellers_id == sellers_id && a.ob_tbl_sales_order.tbl_Marketplace_Id == ddl_market_place).ToList();
                    int i = 0;
                    view_salereport = new SaleReport();
                    if (GetSaleOrderDetail != null)
                    {
                        if (txt_from != null && txt_to != null)
                        {
                            GetSaleOrderDetail = GetSaleOrderDetail.Where(a => a.ob_tbl_sales_order.purchase_date.Date >= txt_from && a.ob_tbl_sales_order.purchase_date.Date <= txt_to).ToList();
                        }
                    }
                    foreach (var item in GetSaleOrderDetail)
                    {
                        int iRepeatDetailData = 0;
                        if (item != null)
                        {
                            if (view_salereport.OrderID != item.ob_tbl_sales_order.amazon_order_id)
                            {
                                if (view_salereport.OrderID != null)
                                    lstOrdertext2.Add(view_salereport);
                            }
                            int fillItemCount = lstOrdertext2.Count;
                            if (fillItemCount > 0)
                            {
                                if (lstOrdertext2[fillItemCount - 1].OrderID != item.ob_tbl_sales_order.amazon_order_id && view_salereport.OrderID != item.ob_tbl_sales_order.amazon_order_id)
                                    view_salereport = new SaleReport();
                            }


                            string amazon_order_id = item.ob_tbl_sales_order.amazon_order_id;
                            view_salereport.OrderID = item.ob_tbl_sales_order.amazon_order_id;
                            view_salereport.OrderDate = Convert.ToDateTime(item.ob_tbl_sales_order.purchase_date).ToString("yyyy-MM-dd");
                            // to get sale order details data from  sale order id 

                            double Principal = 0;
                            double ShippingPrice = 0;
                            double Giftwarp = 0;
                            double Shipping_Discount = 0, itemprice = 0;
                            string itemname = "", sku = "";


                            var get_saleorder_details = dba.tbl_sales_order_details.Where(a => a.tbl_sales_order_id == item.ob_tbl_sales_order.id && a.tbl_seller_id == sellers_id).ToList();//to get sale_order details 
                            if (get_saleorder_details != null)
                            {
                                foreach (var order_details in get_saleorder_details)
                                {
                                    double ord_detail_Principal = order_details.item_price_amount;
                                    Principal = Principal + ord_detail_Principal;
                                    double Shipping_Price = order_details.shipping_price_Amount;
                                    ShippingPrice = ShippingPrice + Shipping_Price;
                                    double giftwrap = Convert.ToDouble(order_details.giftwrapprice_amount);
                                    Giftwarp = Giftwarp + giftwrap;
                                    double Shipping_discount = order_details.shipping_discount_amt;
                                    Shipping_Discount = Shipping_Discount + Shipping_discount;
                                    view_salereport.Principal = Principal;
                                    view_salereport.Shipping = ShippingPrice;
                                    view_salereport.GiftAmount = Giftwarp;
                                    view_salereport.ShippingDiscount = Shipping_Discount;

                                    view_salereport.ordertotal = view_salereport.Principal + view_salereport.Shipping + view_salereport.GiftAmount + view_salereport.ShippingDiscount;

                                    var get_inventory = dba.tbl_inventory.Where(a => a.tbl_sellers_id == sellers_id && a.sku.ToLower() == order_details.sku_no.ToLower()).FirstOrDefault();
                                    if (get_inventory != null)
                                    {
                                        itemname = get_inventory.item_name;
                                        sku = get_inventory.sku;

                                        var get_inventoryeffective = dba.tbl_inventory_effectiveprice.Where(a => a.tbl_sellerid == sellers_id && a.tbl_inventory_id == get_inventory.id).OrderByDescending(a => a.Id).FirstOrDefault();
                                        if (get_inventoryeffective != null)
                                        {
                                            itemprice = Convert.ToDouble(get_inventoryeffective.Effective_price);
                                            //view_salereport.ProductName = itemname;
                                            //view_salereport.ProductValue = itemprice;
                                            //view_salereport.skuNo = sku;
                                        }
                                        //itemprice = Convert.ToDouble(get_inventory.t_effectiveBought_price);
                                        view_salereport.ProductName = itemname;
                                        view_salereport.ProductValue = itemprice;
                                        view_salereport.skuNo = sku;
                                    }

                                }
                                iRepeatDetailData++;
                            }
                            ///////--------------End------/////////////

                            // -----------------------get data from tbl_settlement from order-ID -------------------//
                            int iRepeatSettlementData = 0;
                            double orderprincipal = 0;
                            double ordershipping = 0;
                            double ordergiftwrap = 0;
                            double ordershipping_discount = 0;


                            var get_settlementdata = dba.tbl_settlement_order.Where(a => a.Order_Id == item.ob_tbl_sales_order.amazon_order_id && a.tbl_seller_id == sellers_id).ToList();
                            //string uniqueorderid = "";
                            if (get_settlementdata != null)
                            {
                                foreach (var settle in get_settlementdata)
                                {
                                    //string uniqueorderid = "";
                                    orderprincipal = orderprincipal + Convert.ToDouble(settle.principal_price);
                                    ordershipping = ordershipping + Convert.ToDouble(settle.shipping_price);
                                    ordergiftwrap = ordergiftwrap + Convert.ToDouble(settle.giftwrap_price);
                                    ordershipping_discount = ordershipping_discount + Convert.ToDouble(settle.shipping_discount);


                                    view_salereport.orderprincipal = orderprincipal;
                                    view_salereport.ordershipping = ordershipping;
                                    view_salereport.ordergiftwrap = ordergiftwrap;
                                    view_salereport.ordershipping_discount = ordershipping_discount;


                                    view_salereport.orderTotal = view_salereport.orderprincipal + view_salereport.ordershipping + view_salereport.ordergiftwrap + view_salereport.ordershipping_discount;
                                    view_salereport.ReferenceID = settle.settlement_id;

                                    //-------------------------End---------------------------//
                                    //-----------------------Get  data from tbl_Expense from settlement_Id----------------//

                                    double FBAFEE = 0, TechnologyFee = 0, CommissionFee = 0, FixedClosingFee = 0, ShippingChargebackFee = 0, EasyShipweighthandlingfees = 0,
                                            ShippingDiscountFee = 0, RefundCommision = 0, ShippingCommision = 0, FBAPickPackFee = 0, GiftWrapChargeback = 0, AmazonEasyShipCharges = 0;

                                    var getsettlementdetails = dba.m_tbl_expense.Where(a => a.settlement_order_id.Contains(amazon_order_id) && a.tbl_seller_id == sellers_id && a.t_transactionType_id == 1).ToList();
                                    if (getsettlementdetails != null && getsettlementdetails.Count > 0)
                                    {
                                        foreach (var item1 in getsettlementdetails)
                                        {
                                            var exp_id = item1.expense_type_id;
                                            var get_expdetails = dba.m_settlement_fee.Where(a => a.id == exp_id).FirstOrDefault();
                                            if (get_expdetails != null)
                                            {
                                                string nam = get_expdetails.return_fee;
                                                if (nam == "FBA Weight Handling Fee")
                                                {
                                                    if (get_expdetails.id == exp_id)
                                                    {
                                                        FBAFEE = FBAFEE + Convert.ToDouble(item1.expense_amount);
                                                        view_salereport.FBAFEE = FBAFEE;
                                                    }
                                                }
                                                else if (nam == "Technology Fee")
                                                {
                                                    if (get_expdetails.id == exp_id)
                                                    {
                                                        TechnologyFee = TechnologyFee + Convert.ToDouble(item1.expense_amount);
                                                        view_salereport.TechnologyFee = TechnologyFee;
                                                    }
                                                }
                                                else if (nam == "Commission")
                                                {
                                                    if (get_expdetails.id == exp_id)
                                                    {
                                                        CommissionFee = CommissionFee + Convert.ToDouble(item1.expense_amount);
                                                        view_salereport.CommissionFee = CommissionFee;
                                                    }
                                                }
                                                else if (nam == "Fixed closing fee")
                                                {
                                                    if (get_expdetails.id == exp_id)
                                                    {
                                                        FixedClosingFee = FixedClosingFee + Convert.ToDouble(item1.expense_amount);
                                                        view_salereport.FixedClosingFee = FixedClosingFee;
                                                    }
                                                }
                                                else if (nam == "Shipping Chargeback")
                                                {
                                                    if (get_expdetails.id == exp_id)
                                                    {
                                                        ShippingChargebackFee = ShippingChargebackFee + Convert.ToDouble(item1.expense_amount);
                                                        view_salereport.ShippingChargebackFee = ShippingChargebackFee;
                                                    }
                                                }
                                                else if (nam == "Shipping discount")
                                                {
                                                    if (get_expdetails.id == exp_id)
                                                    {
                                                        ShippingDiscountFee = ShippingDiscountFee + Convert.ToDouble(item1.expense_amount);
                                                        view_salereport.ShippingDiscountFee = ShippingDiscountFee;
                                                    }
                                                }
                                                else if (nam == "Refund commission")
                                                {
                                                    if (get_expdetails.id == exp_id)
                                                    {
                                                        RefundCommision = RefundCommision + Convert.ToDouble(item1.expense_amount);
                                                        view_salereport.RefundCommision = RefundCommision;
                                                    }
                                                }

                                                else if (nam == "Shipping commission")
                                                {
                                                    if (get_expdetails.id == exp_id)
                                                    {
                                                        ShippingCommision = ShippingCommision + Convert.ToDouble(item1.expense_amount);
                                                        view_salereport.ShippingCommision = ShippingCommision;
                                                    }
                                                }
                                                else if (nam == "Easy Ship weight handling fees")
                                                {
                                                    if (get_expdetails.id == exp_id)
                                                    {
                                                        EasyShipweighthandlingfees = EasyShipweighthandlingfees + Convert.ToDouble(item1.expense_amount);
                                                        view_salereport.EasyShipweighthandlingfees = EasyShipweighthandlingfees;
                                                    }
                                                }

                                                else if (nam == "FBA Pick & Pack Fee")
                                                {
                                                    if (get_expdetails.id == exp_id)
                                                    {
                                                        FBAPickPackFee = FBAPickPackFee + Convert.ToDouble(item1.expense_amount);
                                                        view_salereport.FBAPickPackFee = FBAPickPackFee;

                                                    }
                                                }

                                                else if (nam == "Gift Wrap Chargeback")
                                                {
                                                    if (get_expdetails.id == exp_id)
                                                    {
                                                        GiftWrapChargeback = GiftWrapChargeback + Convert.ToDouble(item1.expense_amount);
                                                        view_salereport.GiftWrapChargeback = GiftWrapChargeback;

                                                    }
                                                }
                                                else if (nam == "Amazon Easy Ship Charges")
                                                {
                                                    if (get_expdetails.id == exp_id)
                                                    {
                                                        AmazonEasyShipCharges = AmazonEasyShipCharges + Convert.ToDouble(item1.expense_amount);
                                                        view_salereport.AmazonEasyShipCharges = AmazonEasyShipCharges;

                                                    }
                                                }
                                            }// end of if(get_expdetails)
                                        }// end if foreach(item1)
                                    }// end of if(getsettlementdetails) 


                                }
                                iRepeatSettlementData++;
                            }

                            //------------------------------------------End--------------------------------------//
                            //-------------------------------------To get Refund data from tbl_History and taxtable------------------------//

                            var get_historydata = dba.tbl_order_history.Where(a => a.tbl_seller_id == sellers_id && a.OrderID == item.ob_tbl_sales_order.amazon_order_id && a.t_order_status == 9).FirstOrDefault();
                            if (get_historydata != null)
                            {
                                view_salereport.refundprincipal = Convert.ToDouble(get_historydata.amount_per_unit);
                                view_salereport.refundshipping = Convert.ToDouble(get_historydata.shipping_price);
                                view_salereport.refundgiftwrap = Convert.ToDouble(get_historydata.Giftwrap_price);
                                view_salereport.refundshipping_discount = Convert.ToDouble(get_historydata.shipping_discount);
                                view_salereport.refundTotal = view_salereport.refundprincipal + view_salereport.refundshipping + view_salereport.refundgiftwrap + view_salereport.refundshipping_discount;


                                view_salereport.refundReferenceID = get_historydata.settlement_id;

                                var get_refundexpense = dba.m_tbl_expense.Where(a => a.settlement_order_id == get_historydata.unique_order_id && a.tbl_seller_id == sellers_id && a.t_transactionType_id == 2 && a.tbl_order_historyid == get_historydata.Id).ToList();// to get refund expense
                                if (get_refundexpense != null && get_refundexpense.Count > 0)
                                {
                                    foreach (var refund in get_refundexpense)
                                    {
                                        var exp_ID = refund.expense_type_id;
                                        var get_details = dba.m_settlement_fee.Where(a => a.id == exp_ID).FirstOrDefault();
                                        if (get_details != null)
                                        {
                                            string nam = get_details.return_fee;
                                            if (nam == "FBA Weight Handling Fee")
                                            {
                                                if (get_details.id == exp_ID)
                                                {
                                                    view_salereport.RefundFBAFEE = Convert.ToDouble(refund.expense_amount);
                                                }
                                            }
                                            else if (nam == "Technology Fee")
                                            {
                                                if (get_details.id == exp_ID)
                                                {
                                                    view_salereport.RefundTechnologyFee = Convert.ToDouble(refund.expense_amount);
                                                }
                                            }
                                            else if (nam == "Commission")
                                            {
                                                if (get_details.id == exp_ID)
                                                {
                                                    view_salereport.RefundCommissionFee = Convert.ToDouble(refund.expense_amount);
                                                }
                                            }
                                            else if (nam == "Fixed closing fee")
                                            {
                                                if (get_details.id == exp_ID)
                                                {
                                                    view_salereport.RefundFixedClosingFee = Convert.ToDouble(refund.expense_amount);
                                                }
                                            }
                                            else if (nam == "Shipping Chargeback")
                                            {
                                                if (get_details.id == exp_ID)
                                                {
                                                    view_salereport.RefundShippingChargebackFee = Convert.ToDouble(refund.expense_amount);
                                                }
                                            }
                                            else if (nam == "Shipping discount")
                                            {
                                                if (get_details.id == exp_ID)
                                                {
                                                    view_salereport.RefundShippingDiscountFee = Convert.ToDouble(refund.expense_amount);
                                                }
                                            }
                                            else if (nam == "Refund commission")
                                            {
                                                if (get_details.id == exp_ID)
                                                {
                                                    view_salereport.Refund_Commision = Convert.ToDouble(refund.expense_amount);
                                                }
                                            }

                                            else if (nam == "Shipping commission")
                                            {
                                                if (get_details.id == exp_ID)
                                                {
                                                    view_salereport.Refund_ShippingCommision = Convert.ToDouble(refund.expense_amount);
                                                }
                                            }
                                            else if (nam == "Easy Ship weight handling fees")
                                            {
                                                if (get_details.id == exp_ID)
                                                {
                                                    view_salereport.Refund_EasyShipweighthandlingfees = Convert.ToDouble(refund.expense_amount);
                                                }
                                            }
                                            else if (nam == "FBA Pick & Pack Fee")
                                            {
                                                if (get_details.id == exp_ID)
                                                {
                                                    view_salereport.Refund_FBAPick_PackFee = Convert.ToDouble(refund.expense_amount);
                                                }
                                            }

                                            else if (nam == "Gift Wrap Chargeback")
                                            {
                                                if (get_details.id == exp_ID)
                                                {
                                                    view_salereport.Refund_GiftWrapChargeback = Convert.ToDouble(refund.expense_amount);
                                                }
                                            }

                                            else if (nam == "Amazon Easy Ship Charges")
                                            {
                                                if (get_details.id == exp_ID)
                                                {
                                                    view_salereport.Refund_AmazonEasyShipCharges = Convert.ToDouble(refund.expense_amount);
                                                }
                                            }

                                        }// end of if(get_details)                               
                                    }// end of foreach(refund)
                                }// end of if(get_refundexpense)
                            }// end of if(get_historydata)

                            view_salereport.refund_SumFee = view_salereport.RefundFBAFEE + view_salereport.RefundTechnologyFee + view_salereport.RefundCommissionFee + view_salereport.RefundFixedClosingFee + view_salereport.RefundShippingChargebackFee + view_salereport.RefundShippingDiscountFee + view_salereport.Refund_EasyShipweighthandlingfees + view_salereport.Refund_Commision + view_salereport.Refund_ShippingCommision + view_salereport.Refund_FBAPick_PackFee + view_salereport.Refund_GiftWrapChargeback + view_salereport.Refund_AmazonEasyShipCharges;
                            view_salereport.refund_SumOrder = view_salereport.refundTotal + view_salereport.refund_SumFee;
                            /////////-----------------------------------------End----------------------------------------------------------//
                            //sharad
                            view_salereport.SumFee = view_salereport.FBAFEE + view_salereport.TechnologyFee + view_salereport.CommissionFee + view_salereport.FixedClosingFee + view_salereport.ShippingChargebackFee + view_salereport.ShippingDiscountFee + view_salereport.RefundCommision + view_salereport.ShippingCommision + view_salereport.EasyShipweighthandlingfees + view_salereport.FBAPickPackFee + view_salereport.GiftWrapChargeback + view_salereport.AmazonEasyShipCharges;
                            view_salereport.SumOrder = view_salereport.orderTotal + view_salereport.SumFee;

                            view_salereport.NetTotal = view_salereport.SumOrder + view_salereport.refund_SumOrder;
                            view_salereport.Profit_lossAmount = view_salereport.NetTotal - view_salereport.ProductValue;


                            view_salereport.ActualOrderTotal = view_salereport.orderTotal + view_salereport.refundTotal;
                            view_salereport.ActualCommission = view_salereport.CommissionFee + view_salereport.RefundCommissionFee;
                            view_salereport.ActualFBAFee = view_salereport.FBAFEE + view_salereport.RefundFBAFEE;
                            view_salereport.ActualFixedClosingFee = view_salereport.FixedClosingFee + view_salereport.RefundFixedClosingFee;
                            view_salereport.ActualShippingChargebackFee = view_salereport.ShippingChargebackFee + view_salereport.RefundShippingChargebackFee;
                            view_salereport.ActualTechnologyFee = view_salereport.TechnologyFee + view_salereport.RefundTechnologyFee;
                            view_salereport.ActualShippingDiscountFee = view_salereport.ShippingDiscountFee + view_salereport.RefundShippingDiscountFee;
                            view_salereport.ActualShippingCommision = view_salereport.ShippingCommision + view_salereport.Refund_ShippingCommision;
                            view_salereport.ActualEasyShipWeightFee = view_salereport.EasyShipweighthandlingfees + view_salereport.Refund_EasyShipweighthandlingfees;
                            view_salereport.ActualFBAPickPackFee = view_salereport.FBAPickPackFee + view_salereport.Refund_FBAPick_PackFee;
                            view_salereport.ActualGiftWrapChargeback = view_salereport.GiftWrapChargeback + view_salereport.Refund_GiftWrapChargeback;
                            view_salereport.ActualAmazonEasyShipCharges = view_salereport.AmazonEasyShipCharges + view_salereport.Refund_AmazonEasyShipCharges;
                            view_salereport.ActualIGST = view_salereport.SUMIGST + view_salereport.Refund_SUMIGST;
                            view_salereport.ActualCGST = view_salereport.SUMCGST + view_salereport.Refund_SUMCGST;
                            view_salereport.ActualSGST = view_salereport.SUMSGST + view_salereport.Refund_SUMSGST;
                            view_salereport.ActualNetTotal = view_salereport.SumOrder + view_salereport.refund_SumOrder;




                            string value = "";
                            value = Convert.ToString((view_salereport.NetTotal / view_salereport.orderTotal) * 100);

                            if (value != "NaN" && value != "-Infinity")
                            {
                                decimal abc = Convert.ToDecimal((view_salereport.NetTotal / view_salereport.orderTotal) * 100);
                                decimal result = decimal.Round(abc, 2, MidpointRounding.AwayFromZero);

                                string bb = value.ToString();
                                string[] b = bb.Split('.');
                                int firstValue = int.Parse(b[0]);

                                string fvalue = result + "%";
                                view_salereport.PercentageAmount = Convert.ToString(fvalue);
                            }

                            i++;
                        }// end of if(item !=  null)
                    }// end of foreach(GetSaleOrderDetail)
                    if (view_salereport.OrderID != null)
                    {
                        lstOrdertext2.Add(view_salereport);
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return Msg;
        }



        /// <summary>
        /// this is fetching records related to Flipkart Net Realization without Tax
        /// </summary>
        /// <param name="lstOrdertext2"></param>
        /// <param name="view_salereport"></param>
        /// <param name="ddl_market_place"></param>
        /// <param name="txt_from"></param>
        /// <param name="txt_to"></param>
        /// <param name="sellers_id"></param>
        /// <returns></returns>
        public string Get_FlipkartNetRealizationWithOutTax1(List<SaleReport> lstOrdertext2, SaleReport view_salereport, int? ddl_market_place, DateTime? txt_from, DateTime? txt_to, int sellers_id)
        {
            string Msg = "S";
            try
            {
                if (txt_from != null && txt_to != null)
                {
                    var GetSaleOrderDetail = (from ob_tbl_sales_order in dba.tbl_sales_order
                                              select new
                                              {
                                                  ob_tbl_sales_order = ob_tbl_sales_order

                                              }).Where(a => a.ob_tbl_sales_order.tbl_sellers_id == sellers_id && a.ob_tbl_sales_order.tbl_Marketplace_Id == ddl_market_place).ToList();
                    int i = 0;
                    view_salereport = new SaleReport();
                    if (GetSaleOrderDetail != null)
                    {
                        if (txt_from != null && txt_to != null)
                        {
                            GetSaleOrderDetail = GetSaleOrderDetail.Where(a => a.ob_tbl_sales_order.purchase_date.Date >= txt_from && a.ob_tbl_sales_order.purchase_date.Date <= txt_to).ToList();
                        }
                    }
                    foreach (var item in GetSaleOrderDetail)
                    {
                        int iRepeatDetailData = 0;
                        if (item != null)
                        {
                            if (view_salereport.OrderID != item.ob_tbl_sales_order.amazon_order_id)
                            {
                                if (view_salereport.OrderID != null)
                                    lstOrdertext2.Add(view_salereport);
                            }
                            int fillItemCount = lstOrdertext2.Count;
                            if (fillItemCount > 0)
                            {
                                if (lstOrdertext2[fillItemCount - 1].OrderID != item.ob_tbl_sales_order.amazon_order_id && view_salereport.OrderID != item.ob_tbl_sales_order.amazon_order_id)
                                    view_salereport = new SaleReport();
                            }


                            string amazon_order_id = item.ob_tbl_sales_order.amazon_order_id;
                            view_salereport.OrderID = item.ob_tbl_sales_order.amazon_order_id;
                            view_salereport.OrderDate = Convert.ToDateTime(item.ob_tbl_sales_order.purchase_date).ToString("yyyy-MM-dd");
                            // to get sale order details data from  sale order id 

                            double Principal = 0;
                            double ShippingPrice = 0;
                            double Giftwarp = 0;
                            double Shipping_Discount = 0, itemprice = 0;
                            string itemname = "", sku = "";
                            string orderitemid = "";


                            var get_saleorder_details = dba.tbl_sales_order_details.Where(a => a.tbl_sales_order_id == item.ob_tbl_sales_order.id && a.tbl_seller_id == sellers_id).FirstOrDefault();//to get sale_order details 
                            if (get_saleorder_details != null)
                            {
                                //foreach (var order_details in get_saleorder_details)
                                //{
                                double ord_detail_Principal = get_saleorder_details.item_price_amount;
                                Principal = Principal + ord_detail_Principal;
                                double Shipping_Price = get_saleorder_details.shipping_price_Amount;
                                ShippingPrice = ShippingPrice + Shipping_Price;
                                double giftwrap = Convert.ToDouble(get_saleorder_details.giftwrapprice_amount);
                                Giftwarp = Giftwarp + giftwrap;
                                double Shipping_discount = get_saleorder_details.shipping_discount_amt;
                                Shipping_Discount = Shipping_Discount + Shipping_discount;
                                view_salereport.Principal = Principal;
                                view_salereport.Shipping = ShippingPrice;
                                view_salereport.GiftAmount = Giftwarp;
                                view_salereport.ShippingDiscount = Shipping_Discount;

                                orderitemid = get_saleorder_details.order_item_id;

                                view_salereport.ordertotal = view_salereport.Principal + view_salereport.Shipping + view_salereport.GiftAmount + view_salereport.ShippingDiscount;

                                var get_inventory = dba.tbl_inventory.Where(a => a.tbl_sellers_id == sellers_id && a.sku.ToLower() == get_saleorder_details.sku_no.ToLower()).FirstOrDefault();
                                if (get_inventory != null)
                                {
                                    itemname = get_inventory.item_name;
                                    sku = get_inventory.sku;

                                    var get_inventoryeffective = dba.tbl_inventory_effectiveprice.Where(a => a.tbl_sellerid == sellers_id && a.tbl_inventory_id == get_inventory.id).OrderByDescending(a => a.Id).FirstOrDefault();
                                    if (get_inventoryeffective != null)
                                    {
                                        itemprice = Convert.ToDouble(get_inventoryeffective.Effective_price);

                                    }
                                    //itemprice = Convert.ToDouble(get_inventory.t_effectiveBought_price);
                                    view_salereport.ProductName = itemname;
                                    view_salereport.ProductValue = itemprice;
                                    view_salereport.skuNo = sku;
                                }

                                //}
                                iRepeatDetailData++;
                            }
                            ///////--------------End------/////////////

                            #region get_settlement data
                            // -----------------------get data from tbl_settlement from order-ID -------------------//
                            int iRepeatSettlementData = 0;
                            double orderprincipal = 0;
                            double ordershipping = 0;
                            double ordergiftwrap = 0;
                            double ordershipping_discount = 0;


                            var get_settlementdata = dba.tbl_settlement_order.Where(a => a.Order_Id == item.ob_tbl_sales_order.amazon_order_id && a.tbl_seller_id == sellers_id && a.Sku_no == orderitemid).ToList();
                            //string uniqueorderid = "";
                            if (get_settlementdata != null)
                            {
                                foreach (var settle in get_settlementdata)
                                {
                                    //string uniqueorderid = "";
                                    orderprincipal = orderprincipal + Convert.ToDouble(settle.principal_price);
                                    ordershipping = ordershipping + Convert.ToDouble(settle.shipping_price);
                                    ordergiftwrap = ordergiftwrap + Convert.ToDouble(settle.giftwrap_price);
                                    ordershipping_discount = ordershipping_discount + Convert.ToDouble(settle.shipping_discount);


                                    view_salereport.orderprincipal = orderprincipal;
                                    view_salereport.ordershipping = ordershipping;
                                    view_salereport.ordergiftwrap = ordergiftwrap;
                                    view_salereport.ordershipping_discount = ordershipping_discount;


                                    view_salereport.orderTotal = view_salereport.orderprincipal + view_salereport.ordershipping + view_salereport.ordergiftwrap + view_salereport.ordershipping_discount;
                                    view_salereport.ReferenceID = settle.settlement_id;

                                    //-------------------------End---------------------------//
                                    //-----------------------Get  data from tbl_Expense from settlement_Id----------------//

                                    double flipShipping = 0, flipCollection = 0, CommissionFee = 0, flipReverseShipping = 0, flipFixedFee = 0;

                                    var getsettlementdetails = dba.m_tbl_expense.Where(a => a.settlement_order_id.Contains(amazon_order_id) && a.tbl_seller_id == sellers_id && a.t_transactionType_id == 1).ToList();
                                    if (getsettlementdetails != null && getsettlementdetails.Count > 0)
                                    {
                                        foreach (var item1 in getsettlementdetails)
                                        {
                                            var exp_id = item1.expense_type_id;
                                            var get_expdetails = dba.m_settlement_fee.Where(a => a.id == exp_id).FirstOrDefault();
                                            if (get_expdetails != null)
                                            {
                                                string nam = get_expdetails.return_fee;
                                                if (nam == "Shipping Fee")
                                                {
                                                    if (get_expdetails.id == exp_id)
                                                    {
                                                        flipShipping = flipShipping + Convert.ToDouble(item1.expense_amount);
                                                        view_salereport.flipShipping = flipShipping;
                                                    }
                                                }
                                                else if (nam == "Collection Fee")
                                                {
                                                    if (get_expdetails.id == exp_id)
                                                    {
                                                        flipCollection = flipCollection + Convert.ToDouble(item1.expense_amount);
                                                        view_salereport.flipCollection = flipCollection;
                                                    }
                                                }
                                                else if (nam == "Commission")
                                                {
                                                    if (get_expdetails.id == exp_id)
                                                    {
                                                        CommissionFee = CommissionFee + Convert.ToDouble(item1.expense_amount);
                                                        view_salereport.CommissionFee = CommissionFee;
                                                    }
                                                }
                                                else if (nam == "Reverse Shipping Fee")
                                                {
                                                    if (get_expdetails.id == exp_id)
                                                    {
                                                        flipReverseShipping = flipReverseShipping + Convert.ToDouble(item1.expense_amount);
                                                        view_salereport.flipReverseShipping = flipReverseShipping;
                                                    }
                                                }
                                                else if (nam == "Fixed Fee")
                                                {
                                                    if (get_expdetails.id == exp_id)
                                                    {
                                                        flipFixedFee = flipFixedFee + Convert.ToDouble(item1.expense_amount);
                                                        view_salereport.flipFixedFee = flipFixedFee;
                                                    }
                                                }

                                            }// end of if(get_expdetails)
                                        }// end if foreach(item1)
                                    }// end of if(getsettlementdetails) 


                                }
                                iRepeatSettlementData++;
                            }

                            //------------------------------------------End--------------------------------------//

                            #endregion

                            #region get_historydata
                            //-------------------------------------To get Refund data from tbl_History and taxtable------------------------//

                            var get_historydata = dba.tbl_order_history.Where(a => a.tbl_seller_id == sellers_id && a.OrderID == item.ob_tbl_sales_order.amazon_order_id && a.t_order_status == 9 && a.SKU == orderitemid).FirstOrDefault();
                            if (get_historydata != null)
                            {
                                view_salereport.refundprincipal = Convert.ToDouble(get_historydata.amount_per_unit);
                                view_salereport.refundshipping = Convert.ToDouble(get_historydata.shipping_price);
                                view_salereport.refundgiftwrap = Convert.ToDouble(get_historydata.Giftwrap_price);
                                view_salereport.refundshipping_discount = Convert.ToDouble(get_historydata.shipping_discount);
                                view_salereport.refundTotal = view_salereport.refundprincipal + view_salereport.refundshipping + view_salereport.refundgiftwrap + view_salereport.refundshipping_discount;


                                view_salereport.refundReferenceID = get_historydata.settlement_id;

                                var get_refundexpense = dba.m_tbl_expense.Where(a => a.settlement_order_id == get_historydata.unique_order_id && a.tbl_seller_id == sellers_id && a.t_transactionType_id == 2 && a.tbl_order_historyid == get_historydata.Id).ToList();// to get refund expense
                                if (get_refundexpense != null && get_refundexpense.Count > 0)
                                {
                                    foreach (var refund in get_refundexpense)
                                    {
                                        var exp_ID = refund.expense_type_id;
                                        var get_details = dba.m_settlement_fee.Where(a => a.id == exp_ID).FirstOrDefault();
                                        if (get_details != null)
                                        {
                                            string nam = get_details.return_fee;
                                            if (nam == "Shipping Fee")
                                            {
                                                if (get_details.id == exp_ID)
                                                {
                                                    view_salereport.refund_flipShipping = Convert.ToDouble(refund.expense_amount);
                                                }
                                            }
                                            else if (nam == "Collection Fee")
                                            {
                                                if (get_details.id == exp_ID)
                                                {
                                                    view_salereport.refund_flipCollection = Convert.ToDouble(refund.expense_amount);
                                                }
                                            }
                                            else if (nam == "Commission")
                                            {
                                                if (get_details.id == exp_ID)
                                                {
                                                    view_salereport.RefundCommissionFee = Convert.ToDouble(refund.expense_amount);
                                                }
                                            }
                                            else if (nam == "Reverse Shipping Fee")
                                            {
                                                if (get_details.id == exp_ID)
                                                {
                                                    view_salereport.refund_flipReverseShipping = Convert.ToDouble(refund.expense_amount);
                                                }
                                            }
                                            else if (nam == "Fixed Fee")
                                            {
                                                if (get_details.id == exp_ID)
                                                {
                                                    view_salereport.refund_flipFixedFee = Convert.ToDouble(refund.expense_amount);
                                                }
                                            }
                                        }// end of if(get_details)                               
                                    }// end of foreach(refund)
                                }// end of if(get_refundexpense)
                            }// end of if(get_historydata)

                            #endregion

                            view_salereport.refund_SumFee = view_salereport.refund_flipShipping + view_salereport.refund_flipCollection + view_salereport.RefundCommissionFee + view_salereport.refund_flipReverseShipping + view_salereport.refund_flipFixedFee;
                            view_salereport.refund_SumOrder = (view_salereport.ordertotal + view_salereport.refund_SumFee) * (-1);
                            /////////-----------------------------------------End----------------------------------------------------------//

                            //sharad
                            view_salereport.SumFee = view_salereport.flipShipping + view_salereport.flipCollection + view_salereport.CommissionFee + view_salereport.flipReverseShipping + view_salereport.flipFixedFee;
                            view_salereport.SumOrder = view_salereport.ordertotal + view_salereport.SumFee;

                            view_salereport.NetTotal = view_salereport.SumOrder + view_salereport.refund_SumOrder;
                            view_salereport.Profit_lossAmount = view_salereport.NetTotal - view_salereport.ProductValue;
                            string value = "";
                            value = Convert.ToString((view_salereport.NetTotal / view_salereport.orderTotal) * 100);

                            if (value != "NaN" && value != "-Infinity" && value != "Infinity")
                            {
                                decimal abc = Convert.ToDecimal((view_salereport.NetTotal / view_salereport.orderTotal) * 100);
                                decimal result = decimal.Round(abc, 2, MidpointRounding.AwayFromZero);

                                string bb = value.ToString();
                                string[] b = bb.Split('.');
                                int firstValue = int.Parse(b[0]);

                                string fvalue = result + "%";
                                view_salereport.PercentageAmount = Convert.ToString(fvalue);
                            }

                            i++;
                        }// end of if(item !=  null)
                    }// end of foreach(GetSaleOrderDetail)
                    if (view_salereport.OrderID != null)
                    {
                        lstOrdertext2.Add(view_salereport);
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return Msg;
        }

        public string Get_FlipkartNetRealizationWithOutTax(List<SaleReport> lstOrdertext2, SaleReport view_salereport, int? ddl_market_place, DateTime? txt_from, DateTime? txt_to, int sellers_id)
        {
            string Msg = "S";
            try
            {
                if (txt_from != null && txt_to != null)
                {
                    var GetSaleOrderDetail = (from ob_tbl_sales_order in dba.tbl_sales_order
                                              select new
                                              {
                                                  ob_tbl_sales_order = ob_tbl_sales_order

                                              }).Where(a => a.ob_tbl_sales_order.tbl_sellers_id == sellers_id && a.ob_tbl_sales_order.tbl_Marketplace_Id == ddl_market_place).ToList();
                    int i = 0;
                    view_salereport = new SaleReport();
                    if (GetSaleOrderDetail != null)
                    {
                        if (txt_from != null && txt_to != null)
                        {
                            GetSaleOrderDetail = GetSaleOrderDetail.Where(a => a.ob_tbl_sales_order.purchase_date.Date >= txt_from && a.ob_tbl_sales_order.purchase_date.Date <= txt_to).ToList();
                        }
                    }
                    foreach (var item in GetSaleOrderDetail)
                    {
                        int iRepeatDetailData = 0;

                        if (item != null)
                        {
                            if (view_salereport.OrderID != item.ob_tbl_sales_order.amazon_order_id)
                            {
                                if (view_salereport.OrderID != null)
                                    lstOrdertext2.Add(view_salereport);
                            }
                            int fillItemCount = lstOrdertext2.Count;
                            if (fillItemCount > 0)
                            {
                                if (lstOrdertext2[fillItemCount - 1].OrderID != item.ob_tbl_sales_order.amazon_order_id && view_salereport.OrderID != item.ob_tbl_sales_order.amazon_order_id)
                                    view_salereport = new SaleReport();
                            }
                            string amazon_order_id = item.ob_tbl_sales_order.amazon_order_id;
                            view_salereport.OrderID = item.ob_tbl_sales_order.amazon_order_id;
                            view_salereport.OrderDate = Convert.ToDateTime(item.ob_tbl_sales_order.purchase_date).ToString("yyyy-MM-dd");
                            // to get sale order details data from  sale order id 

                            double Principal = 0, ShippingPrice = 0, Giftwarp = 0, Shipping_Discount = 0, itemprice = 0;
                            string itemname = "", sku = "", orderitemid = "";

                            var get_saleorder_details = dba.tbl_sales_order_details.Where(a => a.tbl_sales_order_id == item.ob_tbl_sales_order.id && a.tbl_seller_id == sellers_id).FirstOrDefault();//to get sale_order details 
                            if (get_saleorder_details != null)
                            {
                                double ord_detail_Principal = get_saleorder_details.item_price_amount;
                                Principal = Principal + ord_detail_Principal;

                                double Shipping_Price = get_saleorder_details.shipping_price_Amount;
                                ShippingPrice = ShippingPrice + Shipping_Price;

                                double giftwrap = Convert.ToDouble(get_saleorder_details.giftwrapprice_amount);
                                Giftwarp = Giftwarp + giftwrap;

                                double Shipping_discount = get_saleorder_details.shipping_discount_amt;
                                Shipping_Discount = Shipping_Discount + Shipping_discount;

                                view_salereport.Principal = Principal;
                                view_salereport.Shipping = ShippingPrice;
                                view_salereport.GiftAmount = Giftwarp;
                                view_salereport.ShippingDiscount = Shipping_Discount;

                                orderitemid = get_saleorder_details.order_item_id;

                                view_salereport.ordertotal = view_salereport.Principal + view_salereport.Shipping + view_salereport.GiftAmount + view_salereport.ShippingDiscount;

                                var get_inventory = dba.tbl_inventory.Where(a => a.tbl_sellers_id == sellers_id && a.sku.ToLower() == get_saleorder_details.sku_no.ToLower()).FirstOrDefault();
                                if (get_inventory != null)
                                {
                                    itemname = get_inventory.item_name;
                                    sku = get_inventory.sku;
                                    var get_inventoryeffective = dba.tbl_inventory_effectiveprice.Where(a => a.tbl_sellerid == sellers_id && a.tbl_inventory_id == get_inventory.id).OrderByDescending(a => a.Id).FirstOrDefault();
                                    if (get_inventoryeffective != null)
                                    {
                                        itemprice = Convert.ToDouble(get_inventoryeffective.Effective_price);
                                    }
                                    view_salereport.ProductName = itemname;
                                    view_salereport.ProductValue = itemprice;
                                    view_salereport.skuNo = sku;
                                }
                                iRepeatDetailData++;
                            }
                            ///////--------------End------/////////////

                            #region get_settlement data
                            // -----------------------get data from tbl_settlement from order-ID -------------------//
                            int iRepeatSettlementData = 0;
                            double orderprincipal = 0;
                            double ordershipping = 0;
                            double ordergiftwrap = 0;
                            double ordershipping_discount = 0;

                            var get_settlementdata = dba.tbl_settlement_order.Where(a => a.Order_Id == item.ob_tbl_sales_order.amazon_order_id && a.tbl_seller_id == sellers_id && a.Sku_no == orderitemid).ToList();
                            if (get_settlementdata != null)
                            {
                                foreach (var settle in get_settlementdata)
                                {
                                    orderprincipal = orderprincipal + Convert.ToDouble(settle.principal_price);
                                    ordershipping = ordershipping + Convert.ToDouble(settle.shipping_price);
                                    ordergiftwrap = ordergiftwrap + Convert.ToDouble(settle.giftwrap_price);
                                    ordershipping_discount = ordershipping_discount + Convert.ToDouble(settle.shipping_discount);
                                    view_salereport.orderprincipal = orderprincipal;
                                    view_salereport.ordershipping = ordershipping;
                                    view_salereport.ordergiftwrap = ordergiftwrap;
                                    view_salereport.ordershipping_discount = ordershipping_discount;
                                    view_salereport.orderTotal = view_salereport.orderprincipal + view_salereport.ordershipping + view_salereport.ordergiftwrap + view_salereport.ordershipping_discount;
                                    view_salereport.ReferenceID = settle.settlement_id;

                                    //-------------------------End---------------------------//
                                    //-----------------------Get  data from tbl_Expense from settlement_Id----------------//

                                    double flipShipping = 0, flipCollection = 0, CommissionFee = 0, flipReverseShipping = 0, flipFixedFee = 0;

                                    var getsettlementdetails = dba.m_tbl_expense.Where(a => a.settlement_order_id.Contains(amazon_order_id) && a.tbl_seller_id == sellers_id && a.t_transactionType_id == 1).ToList();
                                    if (getsettlementdetails != null && getsettlementdetails.Count > 0)
                                    {
                                        foreach (var item1 in getsettlementdetails)
                                        {
                                            var exp_id = item1.expense_type_id;
                                            var get_expdetails = dba.m_settlement_fee.Where(a => a.id == exp_id).FirstOrDefault();
                                            if (get_expdetails != null)
                                            {
                                                string nam = get_expdetails.return_fee;
                                                if (nam == "Shipping Fee")
                                                {
                                                    if (get_expdetails.id == exp_id)
                                                    {
                                                        flipShipping = flipShipping + Convert.ToDouble(item1.expense_amount);
                                                        view_salereport.flipShipping = flipShipping;
                                                    }
                                                }
                                                else if (nam == "Collection Fee")
                                                {
                                                    if (get_expdetails.id == exp_id)
                                                    {
                                                        flipCollection = flipCollection + Convert.ToDouble(item1.expense_amount);
                                                        view_salereport.flipCollection = flipCollection;
                                                    }
                                                }
                                                else if (nam == "Commission")
                                                {
                                                    if (get_expdetails.id == exp_id)
                                                    {
                                                        CommissionFee = CommissionFee + Convert.ToDouble(item1.expense_amount);
                                                        view_salereport.CommissionFee = CommissionFee;
                                                    }
                                                }
                                                else if (nam == "Reverse Shipping Fee")
                                                {
                                                    if (get_expdetails.id == exp_id)
                                                    {
                                                        flipReverseShipping = flipReverseShipping + Convert.ToDouble(item1.expense_amount);
                                                        view_salereport.flipReverseShipping = flipReverseShipping;
                                                    }
                                                }
                                                else if (nam == "Fixed Fee")
                                                {
                                                    if (get_expdetails.id == exp_id)
                                                    {
                                                        flipFixedFee = flipFixedFee + Convert.ToDouble(item1.expense_amount);
                                                        view_salereport.flipFixedFee = flipFixedFee;
                                                    }
                                                }
                                            }// end of if(get_expdetails)
                                        }// end if foreach(item1)
                                    }// end of if(getsettlementdetails)
                                }
                                iRepeatSettlementData++;
                            }
                            //------------------------------------------End--------------------------------------//

                            #endregion

                            #region get_historydata
                            //-------------------------------------To get Refund data from tbl_History and taxtable------------------------//

                            var get_historydata = dba.tbl_order_history.Where(a => a.tbl_seller_id == sellers_id && a.OrderID == item.ob_tbl_sales_order.amazon_order_id && a.t_order_status == 9 && a.SKU == orderitemid).FirstOrDefault();
                            if (get_historydata != null)
                            {
                                view_salereport.refundprincipal = Convert.ToDouble(get_historydata.amount_per_unit);
                                view_salereport.refundshipping = Convert.ToDouble(get_historydata.shipping_price);
                                view_salereport.refundgiftwrap = Convert.ToDouble(get_historydata.Giftwrap_price);
                                view_salereport.refundshipping_discount = Convert.ToDouble(get_historydata.shipping_discount);
                                view_salereport.refundTotal = view_salereport.refundprincipal + view_salereport.refundshipping + view_salereport.refundgiftwrap + view_salereport.refundshipping_discount;
                                view_salereport.refundReferenceID = get_historydata.settlement_id;

                                var get_refundexpense = dba.m_tbl_expense.Where(a => a.settlement_order_id == get_historydata.unique_order_id && a.tbl_seller_id == sellers_id && a.t_transactionType_id == 2 && a.tbl_order_historyid == get_historydata.Id).ToList();// to get refund expense
                                if (get_refundexpense != null && get_refundexpense.Count > 0)
                                {
                                    foreach (var refund in get_refundexpense)
                                    {
                                        var exp_ID = refund.expense_type_id;
                                        var get_details = dba.m_settlement_fee.Where(a => a.id == exp_ID).FirstOrDefault();
                                        if (get_details != null)
                                        {
                                            string nam = get_details.return_fee;
                                            if (nam == "Shipping Fee")
                                            {
                                                if (get_details.id == exp_ID)
                                                {
                                                    view_salereport.refund_flipShipping = Convert.ToDouble(refund.expense_amount);
                                                }
                                            }
                                            else if (nam == "Collection Fee")
                                            {
                                                if (get_details.id == exp_ID)
                                                {
                                                    view_salereport.refund_flipCollection = Convert.ToDouble(refund.expense_amount);
                                                }
                                            }
                                            else if (nam == "Commission")
                                            {
                                                if (get_details.id == exp_ID)
                                                {
                                                    view_salereport.RefundCommissionFee = Convert.ToDouble(refund.expense_amount);
                                                }
                                            }
                                            else if (nam == "Reverse Shipping Fee")
                                            {
                                                if (get_details.id == exp_ID)
                                                {
                                                    view_salereport.refund_flipReverseShipping = Convert.ToDouble(refund.expense_amount);
                                                }
                                            }
                                            else if (nam == "Fixed Fee")
                                            {
                                                if (get_details.id == exp_ID)
                                                {
                                                    view_salereport.refund_flipFixedFee = Convert.ToDouble(refund.expense_amount);
                                                }
                                            }
                                        }// end of if(get_details)                               
                                    }// end of foreach(refund)
                                }// end of if(get_refundexpense)
                            }// end of if(get_historydata)

                            #endregion

                            //new changes by vineet
                            view_salereport.Flip_Totalordervalue = view_salereport.orderTotal - (view_salereport.refundTotal * (-1));
                            view_salereport.Flip_Totalcommission = view_salereport.CommissionFee - (view_salereport.RefundCommissionFee * (-1));
                            view_salereport.Flip_Totalshippingfee = view_salereport.flipShipping - (view_salereport.refund_flipShipping * (-1));
                            view_salereport.Flip_Totalcollectionfee = view_salereport.flipCollection - (view_salereport.refund_flipCollection * (-1));
                            view_salereport.Flip_Totalreverseshippingfee = view_salereport.flipReverseShipping - (view_salereport.refund_flipReverseShipping * (-1));
                            view_salereport.Flip_Totalfixedfee = view_salereport.flipFixedFee - (view_salereport.refund_flipFixedFee * (-1));
                            view_salereport.FullExpenseTotal = view_salereport.Flip_Totalcommission + view_salereport.Flip_Totalshippingfee + view_salereport.Flip_Totalcollectionfee + view_salereport.Flip_Totalreverseshippingfee + view_salereport.Flip_Totalfixedfee;

                            //end

                            view_salereport.refund_SumFee = view_salereport.refund_flipShipping + view_salereport.refund_flipCollection + view_salereport.RefundCommissionFee + view_salereport.refund_flipReverseShipping + view_salereport.refund_flipFixedFee;
                            view_salereport.refund_SumOrder = (view_salereport.ordertotal + view_salereport.refund_SumFee) * (-1);
                            /////////-----------------------------------------End----------------------------------------------------------//

                            //sharad
                            view_salereport.SumFee = view_salereport.flipShipping + view_salereport.flipCollection + view_salereport.CommissionFee + view_salereport.flipReverseShipping + view_salereport.flipFixedFee;
                            view_salereport.SumOrder = view_salereport.ordertotal + view_salereport.SumFee;

                            view_salereport.NetTotal = view_salereport.Flip_Totalordervalue - (view_salereport.FullExpenseTotal * (-1));
                            //view_salereport.NetTotal = view_salereport.SumOrder + view_salereport.refund_SumOrder;
                            view_salereport.Profit_lossAmount = view_salereport.NetTotal - view_salereport.ProductValue;
                            string value = "";
                            //value = Convert.ToString((view_salereport.NetTotal / view_salereport.orderTotal) * 100);
                            value = Convert.ToString((view_salereport.NetTotal / view_salereport.ordertotal) * 100);
                            if (value != "NaN" && value != "-Infinity" && value != "Infinity")
                            {
                                decimal abc = Convert.ToDecimal((view_salereport.NetTotal / view_salereport.ordertotal) * 100);
                                decimal result = decimal.Round(abc, 2, MidpointRounding.AwayFromZero);
                                string bb = value.ToString();
                                string[] b = bb.Split('.');
                                int firstValue = int.Parse(b[0]);
                                string fvalue = result + "%";
                                view_salereport.PercentageAmount = Convert.ToString(fvalue);
                            }
                            i++;
                        }// end of if(item !=  null)
                    }// end of foreach(GetSaleOrderDetail)
                    if (view_salereport.OrderID != null)
                    {
                        lstOrdertext2.Add(view_salereport);
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return Msg;
        }


        #region PaytmNetRealizationWithOutTAx
        public string Get_PaytmNetRealizationWithOutTax2(List<SaleReport> lstOrdertext2, SaleReport view_salereport, int? ddl_market_place, DateTime? txt_from, DateTime? txt_to, int sellers_id)
        {
            string Msg = "S";
            try
            {
                if (txt_from != null && txt_to != null)
                {
                    var GetSaleOrderDetail = (from ob_tbl_sales_order in dba.tbl_sales_order
                                              select new
                                              {
                                                  ob_tbl_sales_order = ob_tbl_sales_order

                                              }).Where(a => a.ob_tbl_sales_order.tbl_sellers_id == sellers_id && a.ob_tbl_sales_order.tbl_Marketplace_Id == ddl_market_place).ToList();
                    int i = 0;
                    view_salereport = new SaleReport();
                    if (GetSaleOrderDetail != null)
                    {
                        if (txt_from != null && txt_to != null)
                        {
                            GetSaleOrderDetail = GetSaleOrderDetail.Where(a => a.ob_tbl_sales_order.purchase_date.Date >= txt_from && a.ob_tbl_sales_order.purchase_date.Date <= txt_to).ToList();
                        }
                    }
                    foreach (var item in GetSaleOrderDetail)
                    {
                        int iRepeatDetailData = 0;

                        if (item != null)
                        {
                            if (view_salereport.OrderID != item.ob_tbl_sales_order.amazon_order_id)
                            {
                                if (view_salereport.OrderID != null)
                                    lstOrdertext2.Add(view_salereport);
                            }
                            int fillItemCount = lstOrdertext2.Count;
                            if (fillItemCount > 0)
                            {
                                if (lstOrdertext2[fillItemCount - 1].OrderID != item.ob_tbl_sales_order.amazon_order_id && view_salereport.OrderID != item.ob_tbl_sales_order.amazon_order_id)
                                    view_salereport = new SaleReport();
                            }
                            string amazon_order_id = item.ob_tbl_sales_order.amazon_order_id;
                            view_salereport.OrderID = item.ob_tbl_sales_order.amazon_order_id;
                            view_salereport.OrderDate = Convert.ToDateTime(item.ob_tbl_sales_order.purchase_date).ToString("yyyy-MM-dd");
                            // to get sale order details data from  sale order id 

                            double Principal = 0, ShippingPrice = 0, Giftwarp = 0, Shipping_Discount = 0, itemprice = 0;
                            string itemname = "", sku = "", orderitemid = "", productid = "";

                            var get_saleorder_details = dba.tbl_sales_order_details.Where(a => a.tbl_sales_order_id == item.ob_tbl_sales_order.id && a.tbl_seller_id == sellers_id).FirstOrDefault();//to get sale_order details 
                            if (get_saleorder_details != null)
                            {
                                double ord_detail_Principal = get_saleorder_details.item_price_amount;
                                Principal = Principal + ord_detail_Principal;

                                double Shipping_Price = get_saleorder_details.shipping_price_Amount;
                                ShippingPrice = ShippingPrice + Shipping_Price;

                                double giftwrap = Convert.ToDouble(get_saleorder_details.giftwrapprice_amount);
                                Giftwarp = Giftwarp + giftwrap;

                                double Shipping_discount = get_saleorder_details.shipping_discount_amt;
                                Shipping_Discount = Shipping_Discount + Shipping_discount;

                                view_salereport.Principal = Principal;
                                view_salereport.Shipping = ShippingPrice;
                                view_salereport.GiftAmount = Giftwarp;
                                view_salereport.ShippingDiscount = Shipping_Discount;

                                orderitemid = get_saleorder_details.order_item_id;
                                productid = get_saleorder_details.sku_no;
                                view_salereport.ordertotal = view_salereport.Principal + view_salereport.Shipping + view_salereport.GiftAmount + view_salereport.ShippingDiscount;

                                var get_inventory = dba.tbl_inventory.Where(a => a.tbl_sellers_id == sellers_id && a.sku == get_saleorder_details.sku_no).FirstOrDefault();
                                if (get_inventory != null)
                                {
                                    itemname = get_inventory.item_name;
                                    sku = get_inventory.sku;
                                    var get_inventoryeffective = dba.tbl_inventory_effectiveprice.Where(a => a.tbl_sellerid == sellers_id && a.tbl_inventory_id == get_inventory.id).OrderByDescending(a => a.Id).FirstOrDefault();
                                    if (get_inventoryeffective != null)
                                    {
                                        itemprice = Convert.ToDouble(get_inventoryeffective.Effective_price);
                                    }
                                    view_salereport.ProductName = itemname;
                                    view_salereport.ProductValue = itemprice;
                                    view_salereport.skuNo = sku;
                                }
                                iRepeatDetailData++;
                            }
                            ///////--------------End------/////////////

                            #region get_settlement data
                            // -----------------------get data from tbl_settlement from order-ID -------------------//
                            int iRepeatSettlementData = 0;
                            double orderprincipal = 0;
                            double ordershipping = 0;
                            double ordergiftwrap = 0;
                            double ordershipping_discount = 0;

                            var get_settlementdata = dba.tbl_settlement_order.Where(a => a.Order_Id == item.ob_tbl_sales_order.amazon_order_id && a.tbl_seller_id == sellers_id && a.Sku_no == productid).ToList();
                            if (get_settlementdata != null)
                            {
                                foreach (var settle in get_settlementdata)
                                {
                                    orderprincipal = orderprincipal + Convert.ToDouble(settle.principal_price);
                                    ordershipping = ordershipping + Convert.ToDouble(settle.shipping_price);
                                    ordergiftwrap = ordergiftwrap + Convert.ToDouble(settle.giftwrap_price);
                                    ordershipping_discount = ordershipping_discount + Convert.ToDouble(settle.shipping_discount);
                                    view_salereport.orderprincipal = orderprincipal;
                                    view_salereport.ordershipping = ordershipping;
                                    view_salereport.ordergiftwrap = ordergiftwrap;
                                    view_salereport.ordershipping_discount = ordershipping_discount;
                                    view_salereport.orderTotal = view_salereport.orderprincipal + view_salereport.ordershipping + view_salereport.ordergiftwrap + view_salereport.ordershipping_discount;
                                    view_salereport.ReferenceID = settle.settlement_id;

                                    //-------------------------End---------------------------//
                                    //-----------------------Get  data from tbl_Expense from settlement_Id----------------//

                                    double flipShipping = 0, flipCollection = 0, CommissionFee = 0, flipReverseShipping = 0, flipFixedFee = 0;

                                    var getsettlementdetails = dba.m_tbl_expense.Where(a => a.settlement_order_id.Contains(amazon_order_id) && a.tbl_seller_id == sellers_id && a.t_transactionType_id == 1).ToList();
                                    if (getsettlementdetails != null && getsettlementdetails.Count > 0)
                                    {
                                        foreach (var item1 in getsettlementdetails)
                                        {
                                            var exp_id = item1.expense_type_id;
                                            var get_expdetails = dba.m_settlement_fee.Where(a => a.id == exp_id).FirstOrDefault();
                                            if (get_expdetails != null)
                                            {
                                                string nam = get_expdetails.return_fee;
                                                if (nam == "Marketplace Commission")
                                                {
                                                    if (get_expdetails.id == exp_id)
                                                    {
                                                        flipShipping = flipShipping + Convert.ToDouble(item1.expense_amount);
                                                        view_salereport.flipShipping = flipShipping;
                                                    }
                                                }
                                                else if (nam == "Logistics Charges")
                                                {
                                                    if (get_expdetails.id == exp_id)
                                                    {
                                                        flipCollection = flipCollection + Convert.ToDouble(item1.expense_amount);
                                                        view_salereport.flipCollection = flipCollection;
                                                    }
                                                }
                                                else if (nam == "PG Commission")
                                                {
                                                    if (get_expdetails.id == exp_id)
                                                    {
                                                        CommissionFee = CommissionFee + Convert.ToDouble(item1.expense_amount);
                                                        view_salereport.CommissionFee = CommissionFee;
                                                    }
                                                }
                                                else if (nam == "Penalty")
                                                {
                                                    if (get_expdetails.id == exp_id)
                                                    {
                                                        flipReverseShipping = flipReverseShipping + Convert.ToDouble(item1.expense_amount);
                                                        view_salereport.flipReverseShipping = flipReverseShipping;
                                                    }
                                                }
                                                else if (nam == "Net Adjustments")
                                                {
                                                    if (get_expdetails.id == exp_id)
                                                    {
                                                        flipFixedFee = flipFixedFee + Convert.ToDouble(item1.expense_amount);
                                                        view_salereport.flipFixedFee = flipFixedFee;
                                                    }
                                                }
                                            }// end of if(get_expdetails)
                                        }// end if foreach(item1)
                                    }// end of if(getsettlementdetails)
                                }
                                iRepeatSettlementData++;
                            }
                            //------------------------------------------End--------------------------------------//

                            #endregion

                            #region get_historydata
                            //-------------------------------------To get Refund data from tbl_History and taxtable------------------------//

                            var get_historydata = dba.tbl_order_history.Where(a => a.tbl_seller_id == sellers_id && a.OrderID == item.ob_tbl_sales_order.amazon_order_id && a.t_order_status == 9 && a.SKU == productid).FirstOrDefault();
                            if (get_historydata != null)
                            {
                                view_salereport.refundprincipal = Convert.ToDouble(get_historydata.amount_per_unit);
                                view_salereport.refundshipping = Convert.ToDouble(get_historydata.shipping_price);
                                view_salereport.refundgiftwrap = Convert.ToDouble(get_historydata.Giftwrap_price);
                                view_salereport.refundshipping_discount = Convert.ToDouble(get_historydata.shipping_discount);
                                view_salereport.refundTotal = view_salereport.refundprincipal + view_salereport.refundshipping + view_salereport.refundgiftwrap + view_salereport.refundshipping_discount;
                                view_salereport.refundReferenceID = get_historydata.settlement_id;

                                var get_refundexpense = dba.m_tbl_expense.Where(a => a.settlement_order_id == get_historydata.unique_order_id && a.tbl_seller_id == sellers_id && a.t_transactionType_id == 2 && a.tbl_order_historyid == get_historydata.Id).ToList();// to get refund expense
                                if (get_refundexpense != null && get_refundexpense.Count > 0)
                                {
                                    foreach (var refund in get_refundexpense)
                                    {
                                        var exp_ID = refund.expense_type_id;
                                        var get_details = dba.m_settlement_fee.Where(a => a.id == exp_ID).FirstOrDefault();
                                        if (get_details != null)
                                        {
                                            string nam = get_details.return_fee;
                                            if (nam == "Marketplace Commission")
                                            {
                                                if (get_details.id == exp_ID)
                                                {
                                                    view_salereport.refund_flipShipping = Convert.ToDouble(refund.expense_amount);
                                                }
                                            }
                                            else if (nam == "Logistics Charges")
                                            {
                                                if (get_details.id == exp_ID)
                                                {
                                                    view_salereport.refund_flipCollection = Convert.ToDouble(refund.expense_amount);
                                                }
                                            }
                                            else if (nam == "PG Commission")
                                            {
                                                if (get_details.id == exp_ID)
                                                {
                                                    view_salereport.RefundCommissionFee = Convert.ToDouble(refund.expense_amount);
                                                }
                                            }
                                            else if (nam == "Penalty")
                                            {
                                                if (get_details.id == exp_ID)
                                                {
                                                    view_salereport.refund_flipReverseShipping = Convert.ToDouble(refund.expense_amount);
                                                }
                                            }
                                            else if (nam == "Net Adjustments")
                                            {
                                                if (get_details.id == exp_ID)
                                                {
                                                    view_salereport.refund_flipFixedFee = Convert.ToDouble(refund.expense_amount);
                                                }
                                            }
                                        }// end of if(get_details)                               
                                    }// end of foreach(refund)
                                }// end of if(get_refundexpense)
                            }// end of if(get_historydata)

                            #endregion

                            //new changes by vineet
                            view_salereport.Flip_Totalordervalue = view_salereport.orderTotal - (view_salereport.refundTotal * (-1));
                            view_salereport.Flip_Totalcommission = view_salereport.CommissionFee - (view_salereport.RefundCommissionFee * (-1));//pg commission
                            view_salereport.Flip_Totalshippingfee = view_salereport.flipShipping - (view_salereport.refund_flipShipping * (-1));//marketplace commission
                            view_salereport.Flip_Totalcollectionfee = view_salereport.flipCollection - (view_salereport.refund_flipCollection * (-1));//logistics charges
                            view_salereport.Flip_Totalreverseshippingfee = view_salereport.flipReverseShipping - (view_salereport.refund_flipReverseShipping * (-1));//penanilty
                            view_salereport.Flip_Totalfixedfee = view_salereport.flipFixedFee - (view_salereport.refund_flipFixedFee * (-1));//net adjustment
                            view_salereport.FullExpenseTotal = view_salereport.Flip_Totalcommission + view_salereport.Flip_Totalshippingfee + view_salereport.Flip_Totalcollectionfee + view_salereport.Flip_Totalreverseshippingfee + view_salereport.Flip_Totalfixedfee;

                            //end

                            view_salereport.refund_SumFee = view_salereport.refund_flipShipping + view_salereport.refund_flipCollection + view_salereport.RefundCommissionFee + view_salereport.refund_flipReverseShipping + view_salereport.refund_flipFixedFee;
                            view_salereport.refund_SumOrder = (view_salereport.ordertotal + view_salereport.refund_SumFee) * (-1);
                            /////////-----------------------------------------End----------------------------------------------------------//

                            //sharad
                            view_salereport.SumFee = view_salereport.flipShipping + view_salereport.flipCollection + view_salereport.CommissionFee + view_salereport.flipReverseShipping + view_salereport.flipFixedFee;
                            view_salereport.SumOrder = view_salereport.ordertotal + view_salereport.SumFee;

                            view_salereport.NetTotal = view_salereport.Flip_Totalordervalue - (view_salereport.FullExpenseTotal * (-1));
                            //view_salereport.NetTotal = view_salereport.SumOrder + view_salereport.refund_SumOrder;
                            view_salereport.Profit_lossAmount = view_salereport.NetTotal - view_salereport.ProductValue;
                            string value = "";
                            //value = Convert.ToString((view_salereport.NetTotal / view_salereport.orderTotal) * 100);
                            value = Convert.ToString((view_salereport.NetTotal / view_salereport.ordertotal) * 100);
                            if (value != "NaN" && value != "-Infinity" && value != "Infinity")
                            {
                                decimal abc = Convert.ToDecimal((view_salereport.NetTotal / view_salereport.ordertotal) * 100);
                                decimal result = decimal.Round(abc, 2, MidpointRounding.AwayFromZero);
                                string bb = value.ToString();
                                string[] b = bb.Split('.');
                                int firstValue = int.Parse(b[0]);
                                string fvalue = result + "%";
                                view_salereport.PercentageAmount = Convert.ToString(fvalue);
                            }
                            i++;
                        }// end of if(item !=  null)
                    }// end of foreach(GetSaleOrderDetail)
                    if (view_salereport.OrderID != null)
                    {
                        lstOrdertext2.Add(view_salereport);
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return Msg;
        }



        public string Get_PaytmNetRealizationWithOutTax(List<SaleReport> lstOrdertext2, SaleReport view_salereport, int? ddl_market_place, DateTime? txt_from, DateTime? txt_to, int sellers_id)
        {
            string Msg = "S";
            try
            {
                if (txt_from != null && txt_to != null)
                {
                    using (var connection = dba.Database.Connection)
                    {
                        connection.Open();
                        var command = connection.CreateCommand();
                        command.CommandText = "get_settlement_report"; // "get_sett_orders";
                        command.CommandType = CommandType.StoredProcedure;

                        MySql.Data.MySqlClient.MySqlParameter param, param1, param2, param3;
                        param = new MySql.Data.MySqlClient.MySqlParameter("@sellerid", sellers_id);
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.Int32;
                        command.Parameters.Add(param);

                        param1 = new MySql.Data.MySqlClient.MySqlParameter("@datefrom", txt_from);
                        param1.Direction = ParameterDirection.Input;
                        param1.DbType = DbType.DateTime;
                        command.Parameters.Add(param1);

                        param2 = new MySql.Data.MySqlClient.MySqlParameter("@dateto", txt_to);
                        param2.Direction = ParameterDirection.Input;
                        param2.DbType = DbType.DateTime;
                        command.Parameters.Add(param2);

                        param3 = new MySql.Data.MySqlClient.MySqlParameter("@marketplaceid", ddl_market_place);
                        param3.Direction = ParameterDirection.Input;
                        param3.DbType = DbType.Int32;
                        command.Parameters.Add(param3);

                        using (var reader = command.ExecuteReader())
                        {
                            var get_settlement_data =
                               ((System.Data.Entity.Infrastructure.IObjectContextAdapter)dba)
                                   .ObjectContext
                                   .Translate<proc_Settlement_report>(reader)
                                   .ToList();

                            reader.NextResult();

                            var get_settlement_data1 =
                                ((System.Data.Entity.Infrastructure.IObjectContextAdapter)dba)
                                    .ObjectContext
                                    .Translate<proc_Settlement_report>(reader)
                                    .ToList();

                            reader.NextResult();

                            var get_settlement_data2 =
                                ((System.Data.Entity.Infrastructure.IObjectContextAdapter)dba)
                                    .ObjectContext
                                    .Translate<proc_Settlement_report>(reader)
                                    .ToList();

                            reader.NextResult();

                            var get_history_data =
                                ((System.Data.Entity.Infrastructure.IObjectContextAdapter)dba)
                                    .ObjectContext
                                    .Translate<proc_Settlement_report>(reader)
                                    .ToList();

                            connection.Close();
                            dba = new SellerContext();
                            dba.Configuration.AutoDetectChangesEnabled = false;
                            if (get_settlement_data != null)
                            {
                                for (int maincounter = 0; maincounter < get_settlement_data.Count; maincounter++)
                                {

                                    view_salereport = new SaleReport();
                                    var sett = get_settlement_data[maincounter];
                                    if (sett.amazon_order_id == "4892789736")
                                    {
                                    }
                                    string amazon_order_id = sett.amazon_order_id;
                                    string details_sku = sett.sku_no;
                                    int orderdetails_id = sett.orderdetailsid;
                                    view_salereport.OrderID = sett.amazon_order_id;
                                    view_salereport.OrderDate = Convert.ToDateTime(sett.purchase_date).ToString("yyyy-MM-dd");
                                    view_salereport.DispatchDate = Convert.ToDateTime(sett.Latest_ShipDate).ToString("yyyy-MM-dd");
                                    double Principal = 0, itemtax_amount = 0, Shipping = 0, orderigst = 0, ordersgst = 0, ordercgst = 0, shippingtax = 0, Giftwrap = 0, itempromotion = 0, shippingpromotion = 0;

                                    view_salereport.ProductName = sett.product_name;
                                    view_salereport.skuNo = sett.sku_no;

                                    Principal = Convert.ToDouble(sett.item_price_amount);
                                    Shipping = Convert.ToDouble(sett.shipping_price_Amount);
                                    Giftwrap = Convert.ToDouble(sett.giftwrapprice_amount);
                                    shippingtax = Convert.ToDouble(sett.shipping_tax_Amount);
                                    itempromotion = Convert.ToDouble(sett.item_promotionAmount);
                                    shippingpromotion = Convert.ToDouble(sett.promotion_amount);
                                    //itemtax_amount = Convert.ToDouble(sett.item_tax_amount);

                                    view_salereport.Principal = Principal + Shipping + Giftwrap - itempromotion - shippingpromotion;

                                   
                                    //view_salereport.ordertotal = view_salereport.Principal + view_salereport.Shipping + view_salereport.GiftAmount + view_salereport.ShippingDiscount;
                                    view_salereport.ordertotal = view_salereport.Principal ;

                                    #region get settlement details with expense and tax

                                    if (get_settlement_data2 != null)
                                    {
                                        foreach (var settle in get_settlement_data2)
                                        {
                                            string skuno = settle.Settlement_sku.ToLower();
                                            if (amazon_order_id == settle.Order_Id && details_sku == skuno)
                                            {
                                                view_salereport.orderprincipal = Convert.ToDouble(settle.principal_price);
                                                view_salereport.orderproduct_tax = Convert.ToDouble(settle.product_tax);
                                                view_salereport.ordershipping = Convert.ToDouble(settle.shipping_price);
                                                view_salereport.ordershipping_tax = Convert.ToDouble(settle.shipping_tax);
                                                view_salereport.ordergiftwrap = Convert.ToDouble(settle.giftwrap_price);
                                                view_salereport.ordergiftwrap_tax = Convert.ToDouble(settle.giftwarp_tax);
                                                view_salereport.ordershipping_discount = Convert.ToDouble(settle.shipping_discount);
                                                view_salereport.ordershipping_discounttax = Convert.ToDouble(settle.shipping_tax_discount);

                                                //view_salereport.orderTotal = view_salereport.orderprincipal + view_salereport.ordershipping + view_salereport.ordergiftwrap + view_salereport.ordershipping_discount;
                                                view_salereport.orderTotal = view_salereport.orderprincipal + view_salereport.ordershipping +  view_salereport.ordergiftwrap + view_salereport.ordershipping_discount;
                                                view_salereport.ReferenceID = settle.settlement_id;

                                                //-------------------------End---------------------------//
                                                //-----------------------Get  data from tbl_Expense from settlement_Id----------------//

                                                double FBAFEE = 0, TechnologyFee = 0, CommissionFee = 0, FixedClosingFee = 0,ShippingChargebackFee = 0;


                                                var getsettlementdetails = dba.m_tbl_expense.Where(a => a.settlement_order_id.Contains(amazon_order_id) && a.sku_no == skuno && a.tbl_seller_id == sellers_id && a.t_transactionType_id == 1).ToList();
                                                if (getsettlementdetails != null && getsettlementdetails.Count > 0)
                                                {
                                                    foreach (var item1 in getsettlementdetails)
                                                    {
                                                        var exp_id = item1.expense_type_id;
                                                        var get_expdetails = dba.m_settlement_fee.Where(a => a.id == exp_id).FirstOrDefault();
                                                        //var gettax_details = dba.tbl_tax.Where(a => a.tbl_referneced_id == item1.id && a.reference_type == 2).FirstOrDefault();
                                                        if (get_expdetails != null)
                                                        {
                                                            string nam = get_expdetails.return_fee;
                                                            if (nam == "Marketplace Commission")
                                                            {
                                                                if (get_expdetails.id == exp_id)
                                                                {
                                                                    FBAFEE = FBAFEE + Convert.ToDouble(item1.expense_amount);
                                                                    view_salereport.FBAFEE = FBAFEE;
                                                                }
                                                            }
                                                            else if (nam == "Logistics Charges")
                                                            {
                                                                if (get_expdetails.id == exp_id)
                                                                {
                                                                    TechnologyFee = TechnologyFee + Convert.ToDouble(item1.expense_amount);
                                                                    view_salereport.TechnologyFee = TechnologyFee;                                                                    
                                                                }
                                                            }
                                                            else if (nam == "PG Commission")
                                                            {
                                                                if (get_expdetails.id == exp_id)
                                                                {
                                                                    CommissionFee = CommissionFee + Convert.ToDouble(item1.expense_amount);
                                                                    view_salereport.CommissionFee = CommissionFee;
                                                                }
                                                            }
                                                            else if (nam == "Penalty")
                                                            {
                                                                if (get_expdetails.id == exp_id)
                                                                {
                                                                    FixedClosingFee = FixedClosingFee + Convert.ToDouble(item1.expense_amount);
                                                                    view_salereport.FixedClosingFee = FixedClosingFee;
                                                                }
                                                            }
                                                            else if (nam == "Net Adjustments")
                                                            {
                                                                if (get_expdetails.id == exp_id)
                                                                {
                                                                    ShippingChargebackFee = ShippingChargebackFee + Convert.ToDouble(item1.expense_amount);
                                                                    view_salereport.ShippingChargebackFee = ShippingChargebackFee;
                                                                }
                                                            }


                                                        }// end of if(get_expdetails)
                                                    }// end if foreach(item1)
                                                }// end of if(getsettlementdetails) 
                                            }
                                        }// end of if (compare sku and orderid)
                                    }
                                    #endregion

                                    #region to get history table with expense
                                    if (get_history_data != null)
                                    {
                                        foreach (var history in get_history_data)
                                        {
                                            string sku_no = history.history_sku.ToLower();
                                            if (amazon_order_id == history.history_OrderID && details_sku == sku_no)
                                            {
                                                view_salereport.refundprincipal = Convert.ToDouble(history.history_amount_per_unit);
                                                view_salereport.refundproduct_tax = Convert.ToDouble(history.history_product_tax);
                                                view_salereport.refundshipping = Convert.ToDouble(history.history_shipping_price);
                                                view_salereport.refundshipping_tax = Convert.ToDouble(history.history_shipping_tax);
                                                view_salereport.refundgiftwrap = Convert.ToDouble(history.history_Giftwrap_price);
                                                view_salereport.refundgiftwrap_tax = Convert.ToDouble(history.history_gift_wrap_tax);
                                                view_salereport.refundshipping_discount = Convert.ToDouble(history.history_shipping_discount);
                                                view_salereport.refundshipping_discount_tax = Convert.ToDouble(history.history_shipping_tax_discount);

                                                //view_salereport.refundTotal = view_salereport.refundprincipal + view_salereport.refundshipping + view_salereport.refundgiftwrap + view_salereport.refundshipping_discount;
                                                view_salereport.refundTotal = view_salereport.refundprincipal + view_salereport.refundshipping  + view_salereport.refundgiftwrap + view_salereport.refundshipping_discount;

                                                view_salereport.refundReferenceID = history.history_settlement_id;

                                                double refundFBAFEE = 0,  refundTechnologyFee = 0, refundCommissionFee = 0,refundFixedClosingFee = 0,refundShippingChargebackFee = 0;



                                                var get_refundexpense = dba.m_tbl_expense.Where(a => a.settlement_order_id.Contains(amazon_order_id) && a.sku_no == details_sku && a.tbl_seller_id == sellers_id && a.t_transactionType_id == 2).ToList();// to get refund expense
                                                if (get_refundexpense != null && get_refundexpense.Count > 0)
                                                {
                                                    foreach (var refund in get_refundexpense)
                                                    {
                                                        var exp_ID = refund.expense_type_id;
                                                        var get_details = dba.m_settlement_fee.Where(a => a.id == exp_ID).FirstOrDefault();
                                                     
                                                        if (get_details != null)
                                                        {
                                                            string nam = get_details.return_fee;
                                                            if (nam == "Marketplace Commission")
                                                            {
                                                                if (get_details.id == exp_ID)
                                                                {
                                                                    refundFBAFEE = refundFBAFEE + Convert.ToDouble(refund.expense_amount);
                                                                    view_salereport.RefundFBAFEE = refundFBAFEE;
                                                                }
                                                            }
                                                            else if (nam == "Logistics Charges")
                                                            {
                                                                if (get_details.id == exp_ID)
                                                                {
                                                                    refundTechnologyFee = refundTechnologyFee + Convert.ToDouble(refund.expense_amount);
                                                                    view_salereport.RefundTechnologyFee = refundTechnologyFee;
                                                                }
                                                            }
                                                            else if (nam == "PG Commission")
                                                            {
                                                                if (get_details.id == exp_ID)
                                                                {
                                                                    refundCommissionFee = refundCommissionFee + Convert.ToDouble(refund.expense_amount);
                                                                    view_salereport.RefundCommissionFee = refundCommissionFee;
                                                                }
                                                            }
                                                            else if (nam == "Penalty")
                                                            {
                                                                if (get_details.id == exp_ID)
                                                                {
                                                                    refundFixedClosingFee = refundFixedClosingFee + Convert.ToDouble(refund.expense_amount);
                                                                    view_salereport.RefundFixedClosingFee = refundFixedClosingFee;
                                                                }
                                                            }
                                                            else if (nam == "Net Adjustments")
                                                            {
                                                                if (get_details.id == exp_ID)
                                                                {
                                                                    refundShippingChargebackFee = refundShippingChargebackFee + Convert.ToDouble(refund.expense_amount);
                                                                    view_salereport.RefundShippingChargebackFee = refundShippingChargebackFee;
                                                                }
                                                            }



                                                        }// end of if(get_details)                               
                                                    }// end of foreach(refund)
                                                }// end of if(get_refundexpense)

                                            }// end of if(compare orderid and sku)                                                                                        
                                        }// end of for each loop(history)
                                    }// end of if (check not null)

                                    #endregion



                                    //view_salereport.Flip_Totalordervalue = view_salereport.orderTotal - (view_salereport.refundTotal * (-1));
                                    //view_salereport.Flip_Totalcommission = view_salereport.CommissionFee - (view_salereport.RefundCommissionFee * (-1));//pg commission
                                    //view_salereport.Flip_Totalshippingfee = view_salereport.flipShipping - (view_salereport.refund_flipShipping * (-1));//marketplace commission
                                    //view_salereport.Flip_Totalcollectionfee = view_salereport.flipCollection - (view_salereport.refund_flipCollection * (-1));//logistics charges
                                    //view_salereport.Flip_Totalreverseshippingfee = view_salereport.flipReverseShipping - (view_salereport.refund_flipReverseShipping * (-1));//penanilty
                                    //view_salereport.Flip_Totalfixedfee = view_salereport.flipFixedFee - (view_salereport.refund_flipFixedFee * (-1));//net adjustment
                                    //view_salereport.FullExpenseTotal = view_salereport.Flip_Totalcommission + view_salereport.Flip_Totalshippingfee + view_salereport.Flip_Totalcollectionfee + view_salereport.Flip_Totalreverseshippingfee + view_salereport.Flip_Totalfixedfee;


                                    
                                    view_salereport.refund_SumFee = view_salereport.RefundFBAFEE + view_salereport.RefundTechnologyFee + view_salereport.RefundCommissionFee + view_salereport.RefundFixedClosingFee + view_salereport.RefundShippingChargebackFee;
                                    view_salereport.refund_SumOrder = view_salereport.refundTotal + view_salereport.refund_SumFee;
                                  
                                    view_salereport.SumFee = view_salereport.FBAFEE + view_salereport.TechnologyFee + view_salereport.CommissionFee + view_salereport.FixedClosingFee + view_salereport.ShippingChargebackFee;
                                    view_salereport.SumOrder = view_salereport.orderTotal + view_salereport.SumFee;
                                    view_salereport.NetTotal = view_salereport.SumOrder + view_salereport.refund_SumOrder;



                                    view_salereport.ActualOrderTotal = view_salereport.orderTotal + view_salereport.refundTotal;
                                    view_salereport.ActualCommission = view_salereport.CommissionFee + view_salereport.RefundCommissionFee;//PG Commission
                                    view_salereport.ActualFBAFee = view_salereport.FBAFEE + view_salereport.RefundFBAFEE;//Marketcommission
                                    view_salereport.ActualFixedClosingFee = view_salereport.FixedClosingFee + view_salereport.RefundFixedClosingFee;//Penalty
                                    view_salereport.ActualShippingChargebackFee = view_salereport.ShippingChargebackFee + view_salereport.RefundShippingChargebackFee;//Net Adjustments
                                    view_salereport.ActualTechnologyFee = view_salereport.TechnologyFee + view_salereport.RefundTechnologyFee;//Logistics Charges

                                   
                                    view_salereport.ActualNetTotal = view_salereport.ActualOrderTotal + view_salereport.ActualCommission + view_salereport.ActualFBAFee
                                        + view_salereport.ActualFixedClosingFee + view_salereport.ActualShippingChargebackFee + view_salereport.ActualTechnologyFee;


                                    if (view_salereport.orderTotal != 0)
                                    {
                                        view_salereport.PercentageAmount =
                                        decimal.Round((decimal)(view_salereport.ActualNetTotal / view_salereport.ordertotal) * 100, 2, MidpointRounding.AwayFromZero) + "%";
                                    }
                                    // lstOrdertext2.Add(view_salereport);
                                  
                                        lstOrdertext2.Add(view_salereport);
                                }// end of for (counter)


                            }//end of if(get_settlement_data)
                        }
                    }// end of using connection
                }// end of if (txt from )
            }// end of try block
            catch (Exception ex)
            {
            }
            return Msg;
        }
        #endregion




        /// <summary>
        /// this is fetching records related to Amazon Debtor Ledger Report
        /// </summary>
        /// <param name="lstOrdertext2"></param>
        /// <param name="view_salereport"></param>
        /// <param name="ddl_marketplace"></param>
        /// <param name="txt_from"></param>
        /// <param name="txt_to"></param>
        /// <param name="sellers_id"></param>
        /// <returns></returns>
        public string Get_AmazonDebtorLedger(List<SaleReport> lstOrdertext2, SaleReport view_salereport, int? ddl_marketplace, DateTime? txt_from, DateTime? txt_to, int sellers_id)
        {
            string Msg = "S";
            try
            {
                if (txt_from != null && txt_to != null)
                {
                    var GetSaleOrderDetail = (from ob_tbl_sales_order in dba.tbl_sales_order
                                              select new
                                              {
                                                  ob_tbl_sales_order = ob_tbl_sales_order
                                              }).Where(a => a.ob_tbl_sales_order.tbl_sellers_id == sellers_id).ToList();

                    if (GetSaleOrderDetail != null)
                    {
                        if (txt_from != null && txt_to != null)
                        {
                            GetSaleOrderDetail = GetSaleOrderDetail.Where(a => a.ob_tbl_sales_order.purchase_date.Date >= txt_from && a.ob_tbl_sales_order.purchase_date.Date <= txt_to).ToList();
                        }
                    }
                    if (GetSaleOrderDetail != null && ddl_marketplace != null && ddl_marketplace != 0)
                    {
                        GetSaleOrderDetail = GetSaleOrderDetail.Where(a => a.ob_tbl_sales_order.tbl_Marketplace_Id == ddl_marketplace).ToList();
                    }
                    var get_marketplace = db.m_marketplace.Where(a => a.isactive == 1).ToList();

                    foreach (var item in GetSaleOrderDetail)
                    {
                        int iRepeatDetailData = 0;
                        //view_salereport = new SaleReport();
                        string Marketplace_name = "";

                        if (get_marketplace != null)
                        {
                            foreach (var detail in get_marketplace)
                            {
                                if (item.ob_tbl_sales_order.tbl_Marketplace_Id == detail.id)
                                {
                                    Marketplace_name = detail.name;
                                }
                            }// end of foreach(detail)
                        }// end of if(get_marketplace)



                        //view_salereport.OrderID = item.ob_tbl_sales_order.amazon_order_id;
                        //view_salereport.Status = item.ob_tbl_sales_order.order_status;
                        //view_salereport.OrderDate = Convert.ToDateTime(item.ob_tbl_sales_order.purchase_date).ToString("yyyy-MM-dd");

                        //if (item.ob_tbl_sales_order.order_status == "Canceled")
                        //{
                        //    var get_saleorder_details = dba.tbl_sales_order_details.Where(a => a.tbl_seller_id == sellers_id && a.tbl_sales_order_id == item.ob_tbl_sales_order.id).FirstOrDefault();
                        //    view_salereport.CancelledOrderDate = Convert.ToDateTime(item.ob_tbl_sales_order.purchase_date).ToString("yyyy-MM-dd");
                        //    if (get_saleorder_details != null)
                        //    {
                        //        var get_tax_details = dba.tbl_tax.Where(a => a.tbl_referneced_id == get_saleorder_details.id && a.reference_type == 3).FirstOrDefault();
                        //        view_salereport.CancelledorderAmount = Convert.ToDouble(get_saleorder_details.item_price_amount);
                        //        view_salereport.CancelShipping = Convert.ToDouble(get_saleorder_details.shipping_price_Amount);
                        //        view_salereport.CancelGiftWarp = Convert.ToDouble(get_saleorder_details.giftwrapprice_amount);
                        //        if (get_tax_details != null)
                        //        {
                        //            view_salereport.CancelProductTax = Convert.ToDouble(get_tax_details.product_tax);
                        //            view_salereport.CancelShippingTax = Convert.ToDouble(get_tax_details.shippint_tax_amount);
                        //            view_salereport.CancelWriftrapTax = Convert.ToDouble(get_tax_details.giftwarp_tax);
                        //        }// end of if(get_tax_details)
                        //        view_salereport.SumCancelFee = view_salereport.CancelledorderAmount + view_salereport.CancelShipping + view_salereport.CancelGiftWarp + view_salereport.CancelProductTax + view_salereport.CancelShippingTax + view_salereport.CancelWriftrapTax;
                        //    }// end of if(get_saleorder_details)
                        //}

                        //double Principal = 0;
                        //double Shipping = 0;
                        //double Shippingtax = 0;
                        //double Giftwrap = 0;
                        //double Giftwraptax = 0;
                        //double Shipping_discount = 0;
                        //double Shipping_Discounttax = 0;
                        //double Producttax = 0;

                        var get_saleorderdetails = dba.tbl_sales_order_details.Where(a => a.tbl_seller_id == sellers_id && a.tbl_sales_order_id == item.ob_tbl_sales_order.id).ToList();
                        if (get_saleorderdetails != null)
                        {
                            foreach (var order_details in get_saleorderdetails)
                            {
                                view_salereport = new SaleReport();
                                double Principal = 0;
                                double Shipping = 0;
                                double Shippingtax = 0;
                                double Giftwrap = 0;
                                double Giftwraptax = 0;
                                double Shipping_discount = 0;
                                double Shipping_Discounttax = 0;
                                double Producttax = 0;
                                string Order_id = order_details.amazon_order_id;

                                view_salereport.MarketPlaceName = Marketplace_name;
                                view_salereport.OrderID = item.ob_tbl_sales_order.amazon_order_id;
                                view_salereport.Status = item.ob_tbl_sales_order.order_status;
                                view_salereport.OrderDate = Convert.ToDateTime(item.ob_tbl_sales_order.purchase_date).ToString("yyyy-MM-dd");

                                if (item.ob_tbl_sales_order.order_status == "Canceled")
                                {
                                    var get_saleorder_details = dba.tbl_sales_order_details.Where(a => a.tbl_seller_id == sellers_id && a.tbl_sales_order_id == item.ob_tbl_sales_order.id).FirstOrDefault();
                                    view_salereport.CancelledOrderDate = Convert.ToDateTime(item.ob_tbl_sales_order.purchase_date).ToString("yyyy-MM-dd");
                                    if (get_saleorder_details != null)
                                    {
                                        var get_tax_details = dba.tbl_tax.Where(a => a.tbl_referneced_id == get_saleorder_details.id && a.reference_type == 3).FirstOrDefault();
                                        view_salereport.CancelledorderAmount = Convert.ToDouble(get_saleorder_details.item_price_amount);
                                        view_salereport.CancelShipping = Convert.ToDouble(get_saleorder_details.shipping_price_Amount);
                                        view_salereport.CancelGiftWarp = Convert.ToDouble(get_saleorder_details.giftwrapprice_amount);
                                        if (get_tax_details != null)
                                        {
                                            view_salereport.CancelProductTax = Convert.ToDouble(get_tax_details.product_tax);
                                            view_salereport.CancelShippingTax = Convert.ToDouble(get_tax_details.shippint_tax_amount);
                                            view_salereport.CancelWriftrapTax = Convert.ToDouble(get_tax_details.giftwarp_tax);
                                        }// end of if(get_tax_details)
                                        view_salereport.SumCancelFee = view_salereport.CancelledorderAmount + view_salereport.CancelShipping + view_salereport.CancelGiftWarp + view_salereport.CancelProductTax + view_salereport.CancelShippingTax + view_salereport.CancelWriftrapTax;
                                    }// end of if(get_saleorder_details)
                                }


                                string Skuno = order_details.sku_no;
                                //var get_tax_details = dba.tbl_tax.Where(a => a.tbl_referneced_id == order_details.id && a.reference_type == 3).FirstOrDefault();
                                double ord_OrderAmount = order_details.item_price_amount;
                                Principal = Principal + ord_OrderAmount;
                                double ord_Shipping = order_details.shipping_price_Amount;
                                Shipping = Shipping + ord_Shipping;
                                double ord_Giftwrap = Convert.ToDouble(order_details.giftwrapprice_amount);
                                Giftwrap = Giftwrap + ord_Giftwrap;
                                double ord_shipping_dic = order_details.shipping_discount_amt;
                                Shipping_discount = Shipping_discount + ord_shipping_dic;

                                double ord_shippingtax = order_details.shipping_tax_Amount;
                                Shippingtax = Shippingtax + ord_shippingtax;

                                double ord_shipping_dic_tax = Convert.ToDouble(order_details.shipping_discount_tax_amount);
                                Shipping_Discounttax = Shipping_Discounttax + ord_shipping_dic_tax;

                                double ord_giftwrap_tax = Convert.ToDouble(order_details.giftwraptax_amount);
                                Giftwraptax = Giftwraptax + ord_giftwrap_tax;
                                double ord_producttax = Convert.ToDouble(order_details.item_tax_amount);
                                Producttax = Producttax + ord_producttax;

                                view_salereport.OrderAmount = Principal;
                                view_salereport.Shipping = Shipping;
                                view_salereport.GiftAmount = Giftwrap;
                                view_salereport.ShippingDiscount = Shipping_discount;
                                view_salereport.Product_Tax = order_details.item_tax_amount;
                                view_salereport.Shipping_Tax = Shippingtax;
                                view_salereport.ShippingDiscount_tax = Shipping_Discounttax;
                                view_salereport.GiftTax = Giftwraptax;
                                view_salereport.Product_Tax = Producttax;

                                view_salereport.SumFee = view_salereport.OrderAmount + view_salereport.Product_Tax + view_salereport.Shipping + view_salereport.Shipping_Tax;

                                var get_settlement_data = dba.tbl_settlement_order.Where(a => a.tbl_seller_id == sellers_id && a.Order_Id == Order_id && a.Sku_no == Skuno).FirstOrDefault();
                                if (get_settlement_data != null)
                                {
                                    view_salereport.orderprincipal = Convert.ToDouble(get_settlement_data.principal_price);
                                    view_salereport.orderproduct_tax = Convert.ToDouble(get_settlement_data.product_tax);
                                    view_salereport.ordershipping = Convert.ToDouble(get_settlement_data.shipping_price);
                                    view_salereport.ordershipping_tax = Convert.ToDouble(get_settlement_data.shipping_tax);
                                    view_salereport.ordergiftwrap = Convert.ToDouble(get_settlement_data.giftwrap_price);
                                    view_salereport.ordergiftwrap_tax = Convert.ToDouble(get_settlement_data.giftwarp_tax);
                                    view_salereport.ordershipping_discount = Convert.ToDouble(get_settlement_data.shipping_discount);
                                    view_salereport.ordershipping_discounttax = Convert.ToDouble(get_settlement_data.shipping_tax_discount);

                                    view_salereport.ReferenceID = get_settlement_data.settlement_id;
                                    view_salereport.Sett_orderDate = Convert.ToDateTime(get_settlement_data.posted_date).ToString("yyyy-MM-dd");

                                    view_salereport.orderTotal = view_salereport.orderprincipal + view_salereport.orderproduct_tax + view_salereport.ordershipping + view_salereport.ordershipping_tax + view_salereport.ordergiftwrap + view_salereport.ordergiftwrap_tax + view_salereport.ordershipping_discount + view_salereport.ordershipping_discounttax;
                                }// end of if(get_settlement_data)

                                // var get_tblhistory = dba.tbl_order_history.Where(a => a.tbl_seller_id == sellers_id && a.t_order_status == 9 && a.OrderID == item.ob_tbl_sales_order.amazon_order_id).FirstOrDefault();
                                var get_tblhistory = dba.tbl_order_history.Where(a => a.tbl_seller_id == sellers_id && a.OrderID == Order_id && a.t_order_status == 9 && a.SKU == Skuno).FirstOrDefault();
                                if (get_tblhistory != null)
                                {
                                    view_salereport.refundReferenceID = get_tblhistory.settlement_id;
                                    view_salereport.skuNo = get_tblhistory.SKU;
                                    view_salereport.refundDate = Convert.ToDateTime(get_tblhistory.ShipmentDate).ToString("yyyy-MM-dd");
                                    view_salereport.refundprincipal = Convert.ToDouble(get_tblhistory.amount_per_unit);
                                    view_salereport.refundproduct_tax = Convert.ToDouble(get_tblhistory.product_tax);
                                    view_salereport.refundshipping = Convert.ToDouble(get_tblhistory.shipping_price);
                                    view_salereport.refundshipping_tax = Convert.ToDouble(get_tblhistory.shipping_tax);
                                    view_salereport.refundgiftwrap = Convert.ToDouble(get_tblhistory.Giftwrap_price);
                                    view_salereport.refundgiftwrap_tax = Convert.ToDouble(get_tblhistory.gift_wrap_tax);
                                    view_salereport.refundshipping_discount = Convert.ToDouble(get_tblhistory.shipping_discount);
                                    view_salereport.refundshipping_discount_tax = Convert.ToDouble(get_tblhistory.shipping_tax_discount);
                                    view_salereport.refundtotal = view_salereport.refundprincipal + view_salereport.refundproduct_tax + view_salereport.refundshipping + view_salereport.refundshipping_tax + view_salereport.refundgiftwrap + view_salereport.refundgiftwrap_tax + view_salereport.refundshipping_discount + view_salereport.refundshipping_discount_tax;
                                }// end of if(get_tblhistory)

                                //-------------- To Get Physically return Data when its physically type =1------------------//
                                var get_tblhistory_physically = dba.tbl_order_history.Where(a => a.tbl_seller_id == sellers_id && a.t_order_status == 9 && a.OrderID == Order_id && a.physically_type == 1 && a.SKU == Skuno).FirstOrDefault();
                                if (get_tblhistory_physically != null)
                                {
                                    double principalvalue = 0, producttax = 0, shipping_discount = 0, shipping_dicount_tax = 0, shippingvalue = 0, giftwrapvalue = 0, shippingtax = 0, giftwraptax = 0;
                                    view_salereport.PhysicallyDate = Convert.ToDateTime(get_tblhistory_physically.physically_updated_date).ToString("yyyy-MM-dd");
                                    principalvalue = Convert.ToDouble(get_tblhistory.amount_per_unit);
                                    producttax = Convert.ToDouble(get_tblhistory.product_tax);
                                    shippingvalue = Convert.ToDouble(get_tblhistory.shipping_price);
                                    shippingtax = Convert.ToDouble(get_tblhistory.shipping_tax);
                                    giftwrapvalue = Convert.ToDouble(get_tblhistory.Giftwrap_price);
                                    giftwraptax = Convert.ToDouble(get_tblhistory.gift_wrap_tax);
                                    shipping_discount = Convert.ToDouble(get_tblhistory.shipping_discount);
                                    shipping_dicount_tax = Convert.ToDouble(get_tblhistory.shipping_tax_discount);
                                    view_salereport.PhysicallyAmount = principalvalue + shippingvalue + giftwrapvalue + giftwraptax + producttax + shippingtax + shipping_discount + shipping_dicount_tax;
                                }
                                view_salereport.NetTotal = view_salereport.SumFee - view_salereport.orderTotal - view_salereport.refundtotal + view_salereport.PhysicallyAmount;
                                view_salereport.NetTotal = Convert.ToDouble((view_salereport.NetTotal.ToString("0.#")));
                                lstOrdertext2.Add(view_salereport);
                            }// end of for each order_details
                        }// end of if(get_saleorder_details)

                        //----------------------------------------End----------------------------------------------//
                        //view_salereport.NetTotal = view_salereport.SumFee - view_salereport.orderTotal - view_salereport.refundtotal + view_salereport.PhysicallyAmount;
                        //view_salereport.NetTotal = Convert.ToDouble((view_salereport.NetTotal.ToString("0.#")));
                        //lstOrdertext2.Add(view_salereport);
                    }// end of foreach(item)

                }
            }
            catch (Exception ex)
            {

            }
            return Msg;
        }


        /// <summary>
        /// this is fetching records related to Flipkart Debtor Ledger Report
        /// </summary>
        /// <param name="lstOrdertext2"></param>
        /// <param name="view_salereport"></param>
        /// <param name="ddl_marketplace"></param>
        /// <param name="txt_from"></param>
        /// <param name="txt_to"></param>
        /// <param name="sellers_id"></param>
        /// <returns></returns>
        public string Get_FlipkartDebtorLedger(List<SaleReport> lstOrdertext2, SaleReport view_salereport, int? ddl_marketplace, DateTime? txt_from, DateTime? txt_to, int sellers_id)
        {
            string Msg = "S";
            try
            {
                if (txt_from != null && txt_to != null)
                {
                    var GetSaleOrderDetail = (from ob_tbl_sales_order in dba.tbl_sales_order
                                              select new
                                              {
                                                  ob_tbl_sales_order = ob_tbl_sales_order
                                              }).Where(a => a.ob_tbl_sales_order.tbl_sellers_id == sellers_id && a.ob_tbl_sales_order.tbl_Marketplace_Id == ddl_marketplace).ToList();

                    if (GetSaleOrderDetail != null)
                    {
                        GetSaleOrderDetail = GetSaleOrderDetail.Where(a => a.ob_tbl_sales_order.purchase_date.Date >= txt_from && a.ob_tbl_sales_order.purchase_date.Date <= txt_to).ToList();
                    }
                    //if (GetSaleOrderDetail != null && ddl_marketplace != null && ddl_marketplace != 0)
                    //{
                    //    GetSaleOrderDetail = GetSaleOrderDetail.Where(a => a.ob_tbl_sales_order.tbl_Marketplace_Id == ddl_marketplace).ToList();
                    //}
                    var get_marketplace = db.m_marketplace.Where(a => a.isactive == 1).ToList();

                    foreach (var item in GetSaleOrderDetail)
                    {
                        int iRepeatDetailData = 0;
                        view_salereport = new SaleReport();
                        //SaleReport view_salereport = new SaleReport();
                        if (get_marketplace != null)
                        {
                            foreach (var detail in get_marketplace)
                            {
                                if (item.ob_tbl_sales_order.tbl_Marketplace_Id == detail.id)
                                {
                                    view_salereport.MarketPlaceName = detail.name;
                                }
                            }// end of foreach(detail)
                        }// end of if(get_marketplace)



                        view_salereport.OrderID = item.ob_tbl_sales_order.amazon_order_id;
                        view_salereport.Status = item.ob_tbl_sales_order.order_status;
                        view_salereport.OrderDate = Convert.ToDateTime(item.ob_tbl_sales_order.purchase_date).ToString("yyyy-MM-dd");

                        if (item.ob_tbl_sales_order.order_status == "Canceled")
                        {
                            var get_saleorder_details = dba.tbl_sales_order_details.Where(a => a.tbl_seller_id == sellers_id && a.tbl_sales_order_id == item.ob_tbl_sales_order.id).FirstOrDefault();
                            view_salereport.CancelledOrderDate = Convert.ToDateTime(item.ob_tbl_sales_order.purchase_date).ToString("yyyy-MM-dd");
                            if (get_saleorder_details != null)
                            {
                                var get_tax_details = dba.tbl_tax.Where(a => a.tbl_referneced_id == get_saleorder_details.id && a.reference_type == 3).FirstOrDefault();
                                view_salereport.CancelledorderAmount = Convert.ToDouble(get_saleorder_details.item_price_amount);
                                view_salereport.CancelShipping = Convert.ToDouble(get_saleorder_details.shipping_price_Amount);
                                view_salereport.CancelGiftWarp = Convert.ToDouble(get_saleorder_details.giftwrapprice_amount);
                                if (get_tax_details != null)
                                {
                                    view_salereport.CancelProductTax = Convert.ToDouble(get_tax_details.product_tax);
                                    view_salereport.CancelShippingTax = Convert.ToDouble(get_tax_details.shippint_tax_amount);
                                    view_salereport.CancelWriftrapTax = Convert.ToDouble(get_tax_details.giftwarp_tax);
                                }// end of if(get_tax_details)
                                view_salereport.SumCancelFee = view_salereport.CancelledorderAmount + view_salereport.CancelShipping + view_salereport.CancelGiftWarp + view_salereport.CancelProductTax + view_salereport.CancelShippingTax + view_salereport.CancelWriftrapTax;
                            }// end of if(get_saleorder_details)
                        }

                        double Principal = 0;
                        double Shipping = 0;
                        double Shippingtax = 0;
                        double Giftwrap = 0;
                        double Giftwraptax = 0;
                        double Shipping_discount = 0;
                        double Shipping_Discounttax = 0;
                        double Producttax = 0;


                        var get_saleorderdetails = dba.tbl_sales_order_details.Where(a => a.tbl_seller_id == sellers_id && a.tbl_sales_order_id == item.ob_tbl_sales_order.id).ToList();
                        if (get_saleorderdetails != null)
                        {
                            foreach (var order_details in get_saleorderdetails)
                            {
                                var get_tax_details = dba.tbl_tax.Where(a => a.tbl_referneced_id == order_details.id && a.reference_type == 3).FirstOrDefault();

                                double ord_OrderAmount = order_details.item_price_amount;
                                Principal = Principal + ord_OrderAmount;
                                double ord_Shipping = order_details.shipping_price_Amount;
                                Shipping = Shipping + ord_Shipping;

                                double ord_shipping_dic = order_details.shipping_discount_amt;
                                Shipping_discount = Shipping_discount + ord_shipping_dic;

                                double ord_shippingtax = order_details.shipping_tax_Amount;
                                Shippingtax = Shippingtax + ord_shippingtax;

                                double ord_shipping_dic_tax = Convert.ToDouble(order_details.shipping_discount_tax_amount);
                                Shipping_Discounttax = Shipping_Discounttax + ord_shipping_dic_tax;

                                double ord_giftwrap_tax = Convert.ToDouble(order_details.giftwraptax_amount);
                                Giftwraptax = Giftwraptax + ord_giftwrap_tax;
                                double ord_producttax = Convert.ToDouble(order_details.item_tax_amount);
                                Producttax = Producttax + ord_producttax;

                                view_salereport.OrderAmount = Principal;
                                view_salereport.Shipping = Shipping;
                                view_salereport.GiftAmount = Giftwrap;
                                view_salereport.ShippingDiscount = Shipping_discount;
                                view_salereport.Product_Tax = order_details.item_tax_amount;
                                view_salereport.Shipping_Tax = Shippingtax;
                                view_salereport.ShippingDiscount_tax = Shipping_Discounttax;
                                view_salereport.GiftTax = Giftwraptax;
                                view_salereport.Product_Tax = Producttax;

                                view_salereport.SumFee = view_salereport.OrderAmount + view_salereport.Product_Tax + view_salereport.Shipping + view_salereport.Shipping_Tax;

                            }
                        }// end of if(get_saleorder_details)

                        var get_settlement_data = dba.tbl_settlement_order.Where(a => a.tbl_seller_id == sellers_id && a.Order_Id == item.ob_tbl_sales_order.amazon_order_id).FirstOrDefault();
                        if (get_settlement_data != null)
                        {
                            view_salereport.orderprincipal = Convert.ToDouble(get_settlement_data.principal_price);
                            view_salereport.orderproduct_tax = Convert.ToDouble(get_settlement_data.product_tax);
                            view_salereport.ordershipping = Convert.ToDouble(get_settlement_data.shipping_price);
                            view_salereport.ordershipping_tax = Convert.ToDouble(get_settlement_data.shipping_tax);
                            view_salereport.ordergiftwrap = Convert.ToDouble(get_settlement_data.giftwrap_price);
                            view_salereport.ordergiftwrap_tax = Convert.ToDouble(get_settlement_data.giftwarp_tax);
                            view_salereport.ordershipping_discount = Convert.ToDouble(get_settlement_data.shipping_discount);
                            view_salereport.ordershipping_discounttax = Convert.ToDouble(get_settlement_data.shipping_tax_discount);

                            view_salereport.ReferenceID = get_settlement_data.settlement_id;
                            view_salereport.Sett_orderDate = Convert.ToDateTime(get_settlement_data.posted_date).ToString("yyyy-MM-dd");

                            view_salereport.orderTotal = view_salereport.orderprincipal + view_salereport.orderproduct_tax + view_salereport.ordershipping + view_salereport.ordershipping_tax + view_salereport.ordergiftwrap + view_salereport.ordergiftwrap_tax + view_salereport.ordershipping_discount + view_salereport.ordershipping_discounttax;
                        }// end of if(get_settlement_data)

                        var get_tblhistory = dba.tbl_order_history.Where(a => a.tbl_seller_id == sellers_id && a.OrderID == item.ob_tbl_sales_order.amazon_order_id && a.t_order_status == 9).FirstOrDefault();
                        if (get_tblhistory != null)
                        {
                            view_salereport.refundReferenceID = get_tblhistory.settlement_id;
                            view_salereport.refundDate = Convert.ToDateTime(get_tblhistory.ShipmentDate).ToString("yyyy-MM-dd");
                            view_salereport.refundprincipal = Convert.ToDouble(get_tblhistory.amount_per_unit);
                            view_salereport.refundproduct_tax = Convert.ToDouble(get_tblhistory.product_tax);
                            view_salereport.refundshipping = Convert.ToDouble(get_tblhistory.shipping_price);
                            view_salereport.refundshipping_tax = Convert.ToDouble(get_tblhistory.shipping_tax);
                            view_salereport.refundgiftwrap = Convert.ToDouble(get_tblhistory.Giftwrap_price);
                            view_salereport.refundgiftwrap_tax = Convert.ToDouble(get_tblhistory.gift_wrap_tax);
                            view_salereport.refundshipping_discount = Convert.ToDouble(get_tblhistory.shipping_discount);
                            view_salereport.refundshipping_discount_tax = Convert.ToDouble(get_tblhistory.shipping_tax_discount);
                            view_salereport.refundtotal = view_salereport.refundprincipal + view_salereport.refundproduct_tax + view_salereport.refundshipping + view_salereport.refundshipping_tax + view_salereport.refundgiftwrap + view_salereport.refundgiftwrap_tax + view_salereport.refundshipping_discount + view_salereport.refundshipping_discount_tax;
                        }// end of if(get_tblhistory)

                        //-------------- To Get Physically return Data when its physically type =1------------------//
                        var get_tblhistory_physically = dba.tbl_order_history.Where(a => a.tbl_seller_id == sellers_id && a.t_order_status == 9 && a.OrderID == item.ob_tbl_sales_order.amazon_order_id && a.physically_type == 1).FirstOrDefault();
                        if (get_tblhistory_physically != null)
                        {
                            double principalvalue = 0, producttax = 0, shipping_discount = 0, shipping_dicount_tax = 0, shippingvalue = 0, giftwrapvalue = 0, shippingtax = 0, giftwraptax = 0;
                            view_salereport.PhysicallyDate = Convert.ToDateTime(get_tblhistory_physically.physically_updated_date).ToString("yyyy-MM-dd");
                            principalvalue = Convert.ToDouble(get_tblhistory.amount_per_unit);
                            producttax = Convert.ToDouble(get_tblhistory.product_tax);
                            shippingvalue = Convert.ToDouble(get_tblhistory.shipping_price);
                            shippingtax = Convert.ToDouble(get_tblhistory.shipping_tax);
                            giftwrapvalue = Convert.ToDouble(get_tblhistory.Giftwrap_price);
                            giftwraptax = Convert.ToDouble(get_tblhistory.gift_wrap_tax);
                            shipping_discount = Convert.ToDouble(get_tblhistory.shipping_discount);
                            shipping_dicount_tax = Convert.ToDouble(get_tblhistory.shipping_tax_discount);
                            view_salereport.PhysicallyAmount = principalvalue + shippingvalue + giftwrapvalue + giftwraptax + producttax + shippingtax + shipping_discount + shipping_dicount_tax;
                        }

                        //----------------------------------------End----------------------------------------------//
                        view_salereport.NetTotal = view_salereport.SumFee - view_salereport.orderTotal - view_salereport.refundtotal + view_salereport.PhysicallyAmount;
                        view_salereport.NetTotal = Convert.ToDouble((view_salereport.NetTotal.ToString("0.#")));
                        lstOrdertext2.Add(view_salereport);
                    }// end of foreach(item)

                }
            }
            catch (Exception ex)
            {
            }
            return Msg;
        }


        #region Get Payt Debtor ledger
        public string Get_PaytmDebtorLedger(List<SaleReport> lstOrdertext2, SaleReport view_salereport, int? ddl_marketplace, DateTime? txt_from, DateTime? txt_to, int sellers_id)
        {
            string Msg = "S";
            try
            {
                if (txt_from != null && txt_to != null)
                {
                    var GetSaleOrderDetail = (from ob_tbl_sales_order in dba.tbl_sales_order
                                              select new
                                              {
                                                  ob_tbl_sales_order = ob_tbl_sales_order
                                              }).Where(a => a.ob_tbl_sales_order.tbl_sellers_id == sellers_id && a.ob_tbl_sales_order.tbl_Marketplace_Id == ddl_marketplace).ToList();

                    if (GetSaleOrderDetail != null)
                    {
                        GetSaleOrderDetail = GetSaleOrderDetail.Where(a => a.ob_tbl_sales_order.purchase_date.Date >= txt_from && a.ob_tbl_sales_order.purchase_date.Date <= txt_to).ToList();
                    }

                    var get_marketplace = db.m_marketplace.Where(a => a.isactive == 1).ToList();

                    foreach (var item in GetSaleOrderDetail)
                    {
                        int iRepeatDetailData = 0;
                        view_salereport = new SaleReport();
                        //SaleReport view_salereport = new SaleReport();
                        if (get_marketplace != null)
                        {
                            foreach (var detail in get_marketplace)
                            {
                                if (item.ob_tbl_sales_order.tbl_Marketplace_Id == detail.id)
                                {
                                    view_salereport.MarketPlaceName = detail.name;
                                }
                            }// end of foreach(detail)
                        }// end of if(get_marketplace)



                        view_salereport.OrderID = item.ob_tbl_sales_order.amazon_order_id;
                        view_salereport.Status = item.ob_tbl_sales_order.order_status;
                        view_salereport.OrderDate = Convert.ToDateTime(item.ob_tbl_sales_order.purchase_date).ToString("yyyy-MM-dd");

                        if (item.ob_tbl_sales_order.order_status == "Canceled")
                        {
                            var get_saleorder_details = dba.tbl_sales_order_details.Where(a => a.tbl_seller_id == sellers_id && a.tbl_sales_order_id == item.ob_tbl_sales_order.id).FirstOrDefault();
                            view_salereport.CancelledOrderDate = Convert.ToDateTime(item.ob_tbl_sales_order.purchase_date).ToString("yyyy-MM-dd");
                            if (get_saleorder_details != null)
                            {
                                var get_tax_details = dba.tbl_tax.Where(a => a.tbl_referneced_id == get_saleorder_details.id && a.reference_type == 3).FirstOrDefault();
                                view_salereport.CancelledorderAmount = Convert.ToDouble(get_saleorder_details.item_price_amount);
                                view_salereport.CancelShipping = Convert.ToDouble(get_saleorder_details.shipping_price_Amount);
                                view_salereport.CancelGiftWarp = Convert.ToDouble(get_saleorder_details.giftwrapprice_amount);
                                if (get_tax_details != null)
                                {
                                    view_salereport.CancelProductTax = Convert.ToDouble(get_tax_details.product_tax);
                                    view_salereport.CancelShippingTax = Convert.ToDouble(get_tax_details.shippint_tax_amount);
                                    view_salereport.CancelWriftrapTax = Convert.ToDouble(get_tax_details.giftwarp_tax);
                                }// end of if(get_tax_details)
                                view_salereport.SumCancelFee = view_salereport.CancelledorderAmount + view_salereport.CancelShipping + view_salereport.CancelGiftWarp + view_salereport.CancelProductTax + view_salereport.CancelShippingTax + view_salereport.CancelWriftrapTax;
                            }// end of if(get_saleorder_details)
                        }

                        double Principal = 0;
                        double Shipping = 0;
                        double Shippingtax = 0;
                        double Giftwrap = 0;
                        double Giftwraptax = 0;
                        double Shipping_discount = 0;
                        double Shipping_Discounttax = 0;
                        double Producttax = 0;


                        var get_saleorderdetails = dba.tbl_sales_order_details.Where(a => a.tbl_seller_id == sellers_id && a.tbl_sales_order_id == item.ob_tbl_sales_order.id).ToList();
                        if (get_saleorderdetails != null)
                        {
                            foreach (var order_details in get_saleorderdetails)
                            {
                                var get_tax_details = dba.tbl_tax.Where(a => a.tbl_referneced_id == order_details.id && a.reference_type == 3).FirstOrDefault();

                                double ord_OrderAmount = order_details.item_price_amount;
                                Principal = Principal + ord_OrderAmount;
                                double ord_Shipping = order_details.shipping_price_Amount;
                                Shipping = Shipping + ord_Shipping;

                                double ord_shipping_dic = order_details.shipping_discount_amt;
                                Shipping_discount = Shipping_discount + ord_shipping_dic;

                                double ord_shippingtax = order_details.shipping_tax_Amount;
                                Shippingtax = Shippingtax + ord_shippingtax;

                                double ord_shipping_dic_tax = Convert.ToDouble(order_details.shipping_discount_tax_amount);
                                Shipping_Discounttax = Shipping_Discounttax + ord_shipping_dic_tax;

                                double ord_giftwrap_tax = Convert.ToDouble(order_details.giftwraptax_amount);
                                Giftwraptax = Giftwraptax + ord_giftwrap_tax;
                                double ord_producttax = Convert.ToDouble(order_details.item_tax_amount);
                                Producttax = Producttax + ord_producttax;

                                view_salereport.OrderAmount = Principal;
                                view_salereport.Shipping = Shipping;
                                view_salereport.GiftAmount = Giftwrap;
                                view_salereport.ShippingDiscount = Shipping_discount;
                                view_salereport.Product_Tax = order_details.item_tax_amount;
                                view_salereport.Shipping_Tax = Shippingtax;
                                view_salereport.ShippingDiscount_tax = Shipping_Discounttax;
                                view_salereport.GiftTax = Giftwraptax;
                                view_salereport.Product_Tax = Producttax;

                                view_salereport.SumFee = view_salereport.OrderAmount + view_salereport.Product_Tax + view_salereport.Shipping + view_salereport.Shipping_Tax;

                            }
                        }// end of if(get_saleorder_details)

                        var get_settlement_data = dba.tbl_settlement_order.Where(a => a.tbl_seller_id == sellers_id && a.Order_Id == item.ob_tbl_sales_order.amazon_order_id).FirstOrDefault();
                        if (get_settlement_data != null)
                        {
                            view_salereport.orderprincipal = Convert.ToDouble(get_settlement_data.principal_price);
                            view_salereport.orderproduct_tax = Convert.ToDouble(get_settlement_data.product_tax);
                            view_salereport.ordershipping = Convert.ToDouble(get_settlement_data.shipping_price);
                            view_salereport.ordershipping_tax = Convert.ToDouble(get_settlement_data.shipping_tax);
                            view_salereport.ordergiftwrap = Convert.ToDouble(get_settlement_data.giftwrap_price);
                            view_salereport.ordergiftwrap_tax = Convert.ToDouble(get_settlement_data.giftwarp_tax);
                            view_salereport.ordershipping_discount = Convert.ToDouble(get_settlement_data.shipping_discount);
                            view_salereport.ordershipping_discounttax = Convert.ToDouble(get_settlement_data.shipping_tax_discount);

                            view_salereport.ReferenceID = get_settlement_data.settlement_id;
                            view_salereport.Sett_orderDate = Convert.ToDateTime(get_settlement_data.posted_date).ToString("yyyy-MM-dd");

                            view_salereport.orderTotal = view_salereport.orderprincipal + view_salereport.orderproduct_tax + view_salereport.ordershipping + view_salereport.ordershipping_tax + view_salereport.ordergiftwrap + view_salereport.ordergiftwrap_tax + view_salereport.ordershipping_discount + view_salereport.ordershipping_discounttax;
                        }// end of if(get_settlement_data)

                        var get_tblhistory = dba.tbl_order_history.Where(a => a.tbl_seller_id == sellers_id && a.OrderID == item.ob_tbl_sales_order.amazon_order_id && a.t_order_status == 9).FirstOrDefault();
                        if (get_tblhistory != null)
                        {
                            view_salereport.refundReferenceID = get_tblhistory.settlement_id;
                            view_salereport.refundDate = Convert.ToDateTime(get_tblhistory.ShipmentDate).ToString("yyyy-MM-dd");
                            view_salereport.refundprincipal = Convert.ToDouble(get_tblhistory.amount_per_unit);
                            view_salereport.refundproduct_tax = Convert.ToDouble(get_tblhistory.product_tax);
                            view_salereport.refundshipping = Convert.ToDouble(get_tblhistory.shipping_price);
                            view_salereport.refundshipping_tax = Convert.ToDouble(get_tblhistory.shipping_tax);
                            view_salereport.refundgiftwrap = Convert.ToDouble(get_tblhistory.Giftwrap_price);
                            view_salereport.refundgiftwrap_tax = Convert.ToDouble(get_tblhistory.gift_wrap_tax);
                            view_salereport.refundshipping_discount = Convert.ToDouble(get_tblhistory.shipping_discount);
                            view_salereport.refundshipping_discount_tax = Convert.ToDouble(get_tblhistory.shipping_tax_discount);
                            view_salereport.refundtotal = view_salereport.refundprincipal + view_salereport.refundproduct_tax + view_salereport.refundshipping + view_salereport.refundshipping_tax + view_salereport.refundgiftwrap + view_salereport.refundgiftwrap_tax + view_salereport.refundshipping_discount + view_salereport.refundshipping_discount_tax;
                        }// end of if(get_tblhistory)

                        //-------------- To Get Physically return Data when its physically type =1------------------//
                        var get_tblhistory_physically = dba.tbl_order_history.Where(a => a.tbl_seller_id == sellers_id && a.t_order_status == 9 && a.OrderID == item.ob_tbl_sales_order.amazon_order_id && a.physically_type == 1).FirstOrDefault();
                        if (get_tblhistory_physically != null)
                        {
                            double principalvalue = 0, producttax = 0, shipping_discount = 0, shipping_dicount_tax = 0, shippingvalue = 0, giftwrapvalue = 0, shippingtax = 0, giftwraptax = 0;
                            view_salereport.PhysicallyDate = Convert.ToDateTime(get_tblhistory_physically.physically_updated_date).ToString("yyyy-MM-dd");
                            principalvalue = Convert.ToDouble(get_tblhistory.amount_per_unit);
                            producttax = Convert.ToDouble(get_tblhistory.product_tax);
                            shippingvalue = Convert.ToDouble(get_tblhistory.shipping_price);
                            shippingtax = Convert.ToDouble(get_tblhistory.shipping_tax);
                            giftwrapvalue = Convert.ToDouble(get_tblhistory.Giftwrap_price);
                            giftwraptax = Convert.ToDouble(get_tblhistory.gift_wrap_tax);
                            shipping_discount = Convert.ToDouble(get_tblhistory.shipping_discount);
                            shipping_dicount_tax = Convert.ToDouble(get_tblhistory.shipping_tax_discount);
                            view_salereport.PhysicallyAmount = principalvalue + shippingvalue + giftwrapvalue + giftwraptax + producttax + shippingtax + shipping_discount + shipping_dicount_tax;
                        }

                        //----------------------------------------End----------------------------------------------//
                        view_salereport.NetTotal = view_salereport.SumFee - view_salereport.orderTotal - view_salereport.refundtotal + view_salereport.PhysicallyAmount;
                        view_salereport.NetTotal = Convert.ToDouble((view_salereport.NetTotal.ToString("0.#")));
                        lstOrdertext2.Add(view_salereport);
                    }// end of foreach(item)

                }
            }
            catch (Exception ex)
            {
            }
            return Msg;
        }

        #endregion
        #region FetchPaytm NetRealization
        /// <summary>
        /// this is for fetching data related to Paytm marketplace 
        /// </summary>
        /// <param name="lstOrdertext2"></param>
        /// <param name="txt_from"></param>
        /// <param name="txt_to"></param>
        /// <param name="sellers_id"></param>
        /// <param name="stateid"></param>
        /// <param name="ddl_market_place"></param>
        /// <param name="MarketPlace"></param>
        /// <returns></returns>
        ///  
        #endregion

        #region PaytmNetRealization
        public string Get_SalesReport_Paytm1(List<SaleReport> lstOrdertext2, SaleReport view_salereport, DateTime? txt_from, DateTime? txt_to, int sellers_id, int? ddl_market_place, int? ddl_percentage)
        {
            string Success = "S";
            try
            {
                if (txt_from != null && txt_to != null)
                {
                    using (var connection = dba.Database.Connection)
                    {
                        connection.Open();
                        var command = connection.CreateCommand();
                        command.CommandText = "get_settlement_report"; // "get_sett_orders";
                        command.CommandType = CommandType.StoredProcedure;

                        MySql.Data.MySqlClient.MySqlParameter param, param1, param2, param3;
                        param = new MySql.Data.MySqlClient.MySqlParameter("@sellerid", sellers_id);
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.Int32;
                        command.Parameters.Add(param);

                        param1 = new MySql.Data.MySqlClient.MySqlParameter("@datefrom", txt_from);
                        param1.Direction = ParameterDirection.Input;
                        param1.DbType = DbType.DateTime;
                        command.Parameters.Add(param1);

                        param2 = new MySql.Data.MySqlClient.MySqlParameter("@dateto", txt_to);
                        param2.Direction = ParameterDirection.Input;
                        param2.DbType = DbType.DateTime;
                        command.Parameters.Add(param2);

                        param3 = new MySql.Data.MySqlClient.MySqlParameter("@marketplaceid", ddl_market_place);
                        param3.Direction = ParameterDirection.Input;
                        param3.DbType = DbType.Int32;
                        command.Parameters.Add(param3);


                        using (var reader = command.ExecuteReader())
                        {
                            var get_settlement_data =
                               ((System.Data.Entity.Infrastructure.IObjectContextAdapter)dba)
                                   .ObjectContext
                                   .Translate<proc_Settlement_report>(reader)
                                   .ToList();

                            reader.NextResult();

                            var get_settlement_data1 =
                                ((System.Data.Entity.Infrastructure.IObjectContextAdapter)dba)
                                    .ObjectContext
                                    .Translate<proc_Settlement_report>(reader)
                                    .ToList();
                            reader.NextResult();

                            var get_settlement_data2 =
                                ((System.Data.Entity.Infrastructure.IObjectContextAdapter)dba)
                                    .ObjectContext
                                    .Translate<proc_Settlement_report>(reader)
                                    .ToList();
                            reader.NextResult();

                            var get_history_data =
                                ((System.Data.Entity.Infrastructure.IObjectContextAdapter)dba)
                                    .ObjectContext
                                    .Translate<proc_Settlement_report>(reader)
                                    .ToList();

                            connection.Close();
                            dba = new SellerContext();
                            dba.Configuration.AutoDetectChangesEnabled = false;

                            if (get_settlement_data != null)
                            {
                                for (int maincounter = 0; maincounter < get_settlement_data.Count; maincounter++)
                                {
                                    view_salereport = new SaleReport();
                                    var sett = get_settlement_data[maincounter];
                                    string amazon_order_id = sett.amazon_order_id;
                                    string details_sku = sett.sku_no;
                                    string item_order_id = sett.order_item_id;
                                    //string productid = sett.orde
                                    int orderdetails_id = sett.orderdetailsid;
                                    view_salereport.OrderID = sett.amazon_order_id;
                                    view_salereport.OrderDate = Convert.ToDateTime(sett.purchase_date).ToString("yyyy-MM-dd");

                                    double Principal = 0, Shipping = 0, orderigst = 0, ordersgst = 0, ordercgst = 0, shippingtax = 0, Giftwrap = 0, itempromotion = 0, shippingpromotion = 0;

                                    view_salereport.ProductName = sett.product_name;
                                    view_salereport.skuNo = sett.sku_no;

                                    Principal = Convert.ToDouble(sett.item_price_amount);
                                    Shipping = Convert.ToDouble(sett.shipping_price_Amount);
                                    Giftwrap = Convert.ToDouble(sett.giftwrapprice_amount);
                                    shippingtax = Convert.ToDouble(sett.shipping_tax_Amount);
                                    itempromotion = Convert.ToDouble(sett.item_promotionAmount);
                                    shippingpromotion = Convert.ToDouble(sett.promotion_amount);

                                    view_salereport.Principal = Principal + Shipping + Giftwrap - itempromotion - shippingpromotion;

                                    if (get_settlement_data1 != null)
                                    {
                                        foreach (var sett5 in get_settlement_data1)// use for tax data
                                        {
                                            int refer_id = Convert.ToInt16(sett5.tbl_referneced_id);
                                            if (orderdetails_id == refer_id)
                                            {
                                                orderigst = Convert.ToDouble(sett5.Igst_amount);
                                                ordersgst = Convert.ToDouble(sett5.sgst_amount);
                                                ordercgst = Convert.ToDouble(sett5.CGST_amount);

                                                view_salereport.orderigst = orderigst;
                                                view_salereport.ordersgst = ordersgst;
                                                view_salereport.ordercgst = ordercgst;
                                            }
                                        }
                                    }// end of if(get_settlement_data1)

                                    view_salereport.ordertotal = view_salereport.Principal + view_salereport.orderigst + view_salereport.ordersgst + view_salereport.ordercgst;

                                    #region get settlement details with expense and tax

                                    if (get_settlement_data2 != null)
                                    {
                                        foreach (var settle in get_settlement_data2)
                                        {
                                            string skuno = settle.Settlement_sku;
                                            if (amazon_order_id == settle.Order_Id && details_sku == skuno)
                                            {
                                                view_salereport.orderprincipal = Convert.ToDouble(settle.principal_price);
                                                view_salereport.orderproduct_tax = Convert.ToDouble(settle.product_tax);
                                                view_salereport.ordershipping = Convert.ToDouble(settle.shipping_price);
                                                view_salereport.ordershipping_tax = Convert.ToDouble(settle.shipping_tax);
                                                view_salereport.ordergiftwrap = Convert.ToDouble(settle.giftwrap_price);
                                                view_salereport.ordergiftwrap_tax = Convert.ToDouble(settle.giftwarp_tax);
                                                view_salereport.ordershipping_discount = Convert.ToDouble(settle.shipping_discount);
                                                view_salereport.ordershipping_discounttax = Convert.ToDouble(settle.shipping_tax_discount);

                                                view_salereport.orderTotal = view_salereport.orderprincipal + view_salereport.orderproduct_tax + view_salereport.ordershipping + view_salereport.ordershipping_tax + view_salereport.ordergiftwrap + view_salereport.ordergiftwrap_tax + view_salereport.ordershipping_discount + view_salereport.ordershipping_discounttax;
                                                view_salereport.ReferenceID = settle.settlement_id;

                                                //-------------------------End---------------------------//
                                                //-----------------------Get  data from tbl_Expense from settlement_Id----------------//

                                                double flipShipping = 0, flipShippingFeeIGST = 0, flipShippingFeeCGST = 0, flipShippingFeeSGST = 0, flipCollection = 0, flipCollectionIGST = 0, flipCollectionCGST = 0, flipCollectionSGST = 0,
                                                       flipReverseShipping = 0, flipReverseShippingIGST = 0, flipReverseShippingCGST = 0, flipReverseShippingSGST = 0, flipFixedFee = 0, flipFixedFeeIGST = 0,
                                                       flipFixedFeeCGST = 0, flipFixedFeeSGST = 0, CommissionFee = 0, CommissionIGST = 0, CommissionCGST = 0, CommissionSGST = 0;


                                                var getsettlementdetails = dba.m_tbl_expense.Where(a => a.settlement_order_id.Contains(amazon_order_id) && a.sku_no == skuno && a.tbl_seller_id == sellers_id && a.t_transactionType_id == 1).ToList();
                                                if (getsettlementdetails != null && getsettlementdetails.Count > 0)
                                                {
                                                    foreach (var item1 in getsettlementdetails)
                                                    {
                                                        var exp_id = item1.expense_type_id;
                                                        var get_expdetails = dba.m_settlement_fee.Where(a => a.id == exp_id).FirstOrDefault();
                                                        var gettax_details = dba.tbl_tax.Where(a => a.tbl_referneced_id == item1.id && a.reference_type == 2).FirstOrDefault();
                                                        if (get_expdetails != null)
                                                        {
                                                            string nam = get_expdetails.return_fee;
                                                            if (nam == "Marketplace Commission")
                                                            {
                                                                if (get_expdetails.id == exp_id)
                                                                {
                                                                    flipShipping = flipShipping + Convert.ToDouble(item1.expense_amount);
                                                                    view_salereport.flipShipping = flipShipping;
                                                                    if (gettax_details != null)
                                                                    {
                                                                        flipShippingFeeIGST = flipShippingFeeIGST + Convert.ToDouble(gettax_details.Igst_amount);
                                                                        flipShippingFeeCGST = flipShippingFeeCGST + Convert.ToDouble(gettax_details.CGST_amount);
                                                                        flipShippingFeeSGST = flipShippingFeeSGST + Convert.ToDouble(gettax_details.sgst_amount);

                                                                        view_salereport.flipShippingFeeIGST = flipShippingFeeIGST;
                                                                        view_salereport.flipShippingFeeCGST = flipShippingFeeCGST;
                                                                        view_salereport.flipShippingFeeSGST = flipShippingFeeSGST;
                                                                    }
                                                                }
                                                            }
                                                            else if (nam == "Logistics Charges")
                                                            {
                                                                if (get_expdetails.id == exp_id)
                                                                {
                                                                    flipCollection = flipCollection + Convert.ToDouble(item1.expense_amount);
                                                                    view_salereport.flipCollection = flipCollection;
                                                                    if (gettax_details != null)
                                                                    {
                                                                        flipCollectionIGST = flipCollectionIGST + Convert.ToDouble(gettax_details.Igst_amount);
                                                                        flipCollectionCGST = flipCollectionCGST + Convert.ToDouble(gettax_details.CGST_amount);
                                                                        flipCollectionSGST = flipCollectionSGST + Convert.ToDouble(gettax_details.sgst_amount);

                                                                        view_salereport.flipCollectionIGST = flipCollectionIGST;
                                                                        view_salereport.flipCollectionCGST = flipCollectionCGST;
                                                                        view_salereport.flipCollectionSGST = flipCollectionSGST;
                                                                    }
                                                                }
                                                            }
                                                            else if (nam == "PG Commission")
                                                            {
                                                                if (get_expdetails.id == exp_id)
                                                                {
                                                                    flipReverseShipping = flipReverseShipping + Convert.ToDouble(item1.expense_amount);
                                                                    view_salereport.flipReverseShipping = flipReverseShipping;
                                                                    if (gettax_details != null)
                                                                    {
                                                                        flipReverseShippingIGST = flipReverseShippingIGST + Convert.ToDouble(gettax_details.Igst_amount);
                                                                        flipReverseShippingCGST = flipReverseShippingCGST + Convert.ToDouble(gettax_details.CGST_amount);
                                                                        flipReverseShippingSGST = flipReverseShippingSGST + Convert.ToDouble(gettax_details.sgst_amount);
                                                                        view_salereport.flipReverseShippingIGST = flipReverseShippingIGST;
                                                                        view_salereport.flipReverseShippingCGST = flipReverseShippingCGST;
                                                                        view_salereport.flipReverseShippingSGST = flipReverseShippingSGST;
                                                                    }
                                                                }
                                                            }
                                                            else if (nam == "Penalty")
                                                            {
                                                                if (get_expdetails.id == exp_id)
                                                                {
                                                                    flipFixedFee = flipFixedFee + Convert.ToDouble(item1.expense_amount);
                                                                    view_salereport.flipFixedFee = flipFixedFee;
                                                                    if (gettax_details != null)
                                                                    {
                                                                        flipFixedFeeIGST = flipFixedFeeIGST + Convert.ToDouble(gettax_details.Igst_amount);
                                                                        flipFixedFeeCGST = flipFixedFeeCGST + Convert.ToDouble(gettax_details.CGST_amount);
                                                                        flipFixedFeeSGST = flipFixedFeeSGST + Convert.ToDouble(gettax_details.sgst_amount);

                                                                        view_salereport.flipFixedFeeIGST = flipFixedFeeIGST;
                                                                        view_salereport.flipFixedFeeCGST = flipFixedFeeCGST;
                                                                        view_salereport.flipFixedFeeSGST = flipFixedFeeSGST;
                                                                    }
                                                                }
                                                            }
                                                            else if (nam == "Net Adjustments")
                                                            {
                                                                if (get_expdetails.id == exp_id)
                                                                {
                                                                    CommissionFee = CommissionFee + Convert.ToDouble(item1.expense_amount);
                                                                    view_salereport.CommissionFee = CommissionFee;
                                                                    if (gettax_details != null)
                                                                    {
                                                                        CommissionIGST = CommissionIGST + Convert.ToDouble(gettax_details.Igst_amount);
                                                                        CommissionCGST = CommissionCGST + Convert.ToDouble(gettax_details.CGST_amount);
                                                                        CommissionSGST = CommissionSGST + Convert.ToDouble(gettax_details.sgst_amount);
                                                                        view_salereport.CommissionIGST = CommissionIGST;
                                                                        view_salereport.CommissionCGST = CommissionCGST;
                                                                        view_salereport.CommissionSGST = CommissionSGST;
                                                                    }
                                                                }
                                                            }

                                                        }// end of if(get_expdetails)
                                                    }// end if foreach(item1)
                                                }// end of if(getsettlementdetails) 
                                            }
                                        }// end of if (compare sku and orderid)
                                    }
                                    #endregion

                                    #region to get history table with expense
                                    if (get_history_data != null)
                                    {
                                        foreach (var history in get_history_data)
                                        {
                                            string sku_no = history.history_sku;
                                            if (amazon_order_id == history.history_OrderID && details_sku == sku_no)
                                            {
                                                view_salereport.refundprincipal = Convert.ToDouble(history.history_amount_per_unit);
                                                view_salereport.refundproduct_tax = Convert.ToDouble(history.history_product_tax);
                                                view_salereport.refundshipping = Convert.ToDouble(history.history_shipping_price);
                                                view_salereport.refundshipping_tax = Convert.ToDouble(history.history_shipping_tax);
                                                view_salereport.refundgiftwrap = Convert.ToDouble(history.history_Giftwrap_price);
                                                view_salereport.refundgiftwrap_tax = Convert.ToDouble(history.history_gift_wrap_tax);
                                                view_salereport.refundshipping_discount = Convert.ToDouble(history.history_shipping_discount);
                                                view_salereport.refundshipping_discount_tax = Convert.ToDouble(history.history_shipping_tax_discount);

                                                view_salereport.refundTotal = view_salereport.refundprincipal + view_salereport.refundproduct_tax + view_salereport.refundshipping + view_salereport.refundshipping_tax + view_salereport.refundgiftwrap + view_salereport.refundgiftwrap_tax + view_salereport.refundshipping_discount + view_salereport.refundshipping_discount_tax;

                                                view_salereport.refundReferenceID = history.history_settlement_id;

                                                double refund_flipShipping = 0, Refund_flipShippingFeeIGST = 0, Refund_flipShippingFeeCGST = 0, Refund_flipShippingFeeSGST = 0,
                                                       refund_flipCollection = 0, Refund_flipCollectionIGST = 0, Refund_flipCollectionCGST = 0,
                                                       Refund_flipCollectionSGST = 0, refundCommissionFee = 0, refundCommissionIGST = 0, refundCommissionCGST = 0,
                                                       refundCommissionSGST = 0, refund_flipFixedFee = 0, Refund_flipFixedFeeIGST = 0,
                                                       Refund_flipFixedFeeCGST = 0, Refund_flipFixedFeeSGST = 0, refund_flipReverseShipping = 0, Refund_flipReverseShippingIGST = 0, Refund_flipReverseShippingCGST = 0,
                                                       Refund_flipReverseShippingSGST = 0;


                                                var get_refundexpense = dba.m_tbl_expense.Where(a => a.settlement_order_id.Contains(amazon_order_id) && a.sku_no == details_sku && a.tbl_seller_id == sellers_id && a.t_transactionType_id == 2).ToList();// to get refund expense
                                                if (get_refundexpense != null && get_refundexpense.Count > 0)
                                                {
                                                    foreach (var refund in get_refundexpense)
                                                    {
                                                        var exp_ID = refund.expense_type_id;
                                                        var get_details = dba.m_settlement_fee.Where(a => a.id == exp_ID).FirstOrDefault();
                                                        var getExp_tax_details = dba.tbl_tax.Where(a => a.tbl_referneced_id == refund.id && a.reference_type == 7).FirstOrDefault();
                                                        if (get_details != null)
                                                        {
                                                            string nam = get_details.return_fee;
                                                            if (nam == "Marketplace Commission")
                                                            {
                                                                if (get_details.id == exp_ID)
                                                                {
                                                                    refund_flipShipping = refund_flipShipping + Convert.ToDouble(refund.expense_amount);
                                                                    view_salereport.refund_flipShipping = refund_flipShipping;
                                                                    if (getExp_tax_details != null)
                                                                    {
                                                                        Refund_flipShippingFeeIGST = Refund_flipShippingFeeIGST + Convert.ToDouble(getExp_tax_details.Igst_amount);
                                                                        Refund_flipShippingFeeCGST = Refund_flipShippingFeeCGST + Convert.ToDouble(getExp_tax_details.CGST_amount);
                                                                        Refund_flipShippingFeeSGST = Refund_flipShippingFeeSGST + Convert.ToDouble(getExp_tax_details.sgst_amount);

                                                                        view_salereport.Refund_flipShippingFeeIGST = Refund_flipShippingFeeIGST;
                                                                        view_salereport.Refund_flipShippingFeeCGST = Refund_flipShippingFeeCGST;
                                                                        view_salereport.Refund_flipShippingFeeSGST = Refund_flipShippingFeeSGST;
                                                                    }
                                                                }
                                                            }
                                                            else if (nam == "Logistics Charges")
                                                            {
                                                                if (get_details.id == exp_ID)
                                                                {
                                                                    refund_flipCollection = refund_flipCollection + Convert.ToDouble(refund.expense_amount);
                                                                    view_salereport.refund_flipCollection = refund_flipCollection;
                                                                    if (getExp_tax_details != null)
                                                                    {
                                                                        Refund_flipCollectionIGST = Refund_flipCollectionIGST + Convert.ToDouble(getExp_tax_details.Igst_amount);
                                                                        Refund_flipCollectionCGST = Refund_flipCollectionCGST + Convert.ToDouble(getExp_tax_details.CGST_amount);
                                                                        Refund_flipCollectionSGST = Refund_flipCollectionSGST + Convert.ToDouble(getExp_tax_details.sgst_amount);

                                                                        view_salereport.Refund_flipCollectionIGST = Refund_flipCollectionIGST;
                                                                        view_salereport.Refund_flipCollectionCGST = Refund_flipCollectionCGST;
                                                                        view_salereport.Refund_flipCollectionSGST = Refund_flipCollectionSGST;
                                                                    }
                                                                }
                                                            }
                                                            else if (nam == "PG Commission")
                                                            {
                                                                if (get_details.id == exp_ID)
                                                                {
                                                                    refund_flipReverseShipping = refund_flipReverseShipping + Convert.ToDouble(refund.expense_amount);
                                                                    view_salereport.refund_flipReverseShipping = refund_flipReverseShipping;
                                                                    if (getExp_tax_details != null)
                                                                    {
                                                                        Refund_flipReverseShippingIGST = Refund_flipReverseShippingIGST + Convert.ToDouble(getExp_tax_details.Igst_amount);
                                                                        Refund_flipReverseShippingCGST = Refund_flipReverseShippingCGST + Convert.ToDouble(getExp_tax_details.CGST_amount);
                                                                        Refund_flipReverseShippingSGST = Refund_flipReverseShippingSGST + Convert.ToDouble(getExp_tax_details.sgst_amount);

                                                                        view_salereport.Refund_flipReverseShippingIGST = Refund_flipReverseShippingIGST;
                                                                        view_salereport.Refund_flipReverseShippingCGST = Refund_flipReverseShippingCGST;
                                                                        view_salereport.Refund_flipReverseShippingSGST = Refund_flipReverseShippingSGST;
                                                                    }
                                                                }
                                                            }
                                                            else if (nam == "Penalty")
                                                            {
                                                                if (get_details.id == exp_ID)
                                                                {
                                                                    refund_flipFixedFee = refund_flipFixedFee + Convert.ToDouble(refund.expense_amount);
                                                                    view_salereport.refund_flipFixedFee = refund_flipFixedFee;
                                                                    if (getExp_tax_details != null)
                                                                    {
                                                                        Refund_flipFixedFeeIGST = Refund_flipFixedFeeIGST + Convert.ToDouble(getExp_tax_details.Igst_amount);
                                                                        Refund_flipFixedFeeCGST = Refund_flipFixedFeeCGST + Convert.ToDouble(getExp_tax_details.CGST_amount);
                                                                        Refund_flipFixedFeeSGST = Refund_flipFixedFeeSGST + Convert.ToDouble(getExp_tax_details.sgst_amount);

                                                                        view_salereport.Refund_flipFixedFeeIGST = Refund_flipFixedFeeIGST;
                                                                        view_salereport.Refund_flipFixedFeeCGST = Refund_flipFixedFeeCGST;
                                                                        view_salereport.Refund_flipFixedFeeSGST = Refund_flipFixedFeeSGST;
                                                                    }
                                                                }
                                                            }
                                                            else if (nam == "Net Adjustments")
                                                            {
                                                                if (get_details.id == exp_ID)
                                                                {
                                                                    refundCommissionFee = refundCommissionFee + Convert.ToDouble(refund.expense_amount);
                                                                    view_salereport.RefundCommissionFee = refundCommissionFee;
                                                                    if (getExp_tax_details != null)
                                                                    {
                                                                        refundCommissionIGST = refundCommissionIGST + Convert.ToDouble(getExp_tax_details.Igst_amount);
                                                                        refundCommissionCGST = refundCommissionCGST + Convert.ToDouble(getExp_tax_details.CGST_amount);
                                                                        refundCommissionSGST = refundCommissionSGST + Convert.ToDouble(getExp_tax_details.sgst_amount);

                                                                        view_salereport.RefundCommissionIGST = refundCommissionIGST;
                                                                        view_salereport.RefundCommissionCGST = refundCommissionCGST;
                                                                        view_salereport.RefundCommissionSGST = refundCommissionSGST;
                                                                    }
                                                                }
                                                            }

                                                        }// end of if(get_details)                               
                                                    }// end of foreach(refund)
                                                }// end of if(get_refundexpense)

                                            }// end of if(compare orderid and sku)                                                                                        
                                        }// end of for each loop(history)
                                    }// end of if (check not null)

                                    #endregion

                                    view_salereport.Refund_SUMIGST = view_salereport.Refund_flipShippingFeeIGST + view_salereport.Refund_flipCollectionIGST + view_salereport.Refund_flipReverseShippingIGST + view_salereport.Refund_flipFixedFeeIGST + view_salereport.RefundCommissionIGST;
                                    view_salereport.Refund_SUMCGST = view_salereport.Refund_flipShippingFeeCGST + view_salereport.Refund_flipCollectionCGST + view_salereport.Refund_flipReverseShippingCGST + view_salereport.Refund_flipFixedFeeCGST + view_salereport.RefundCommissionCGST;
                                    view_salereport.Refund_SUMSGST = view_salereport.Refund_flipShippingFeeSGST + view_salereport.Refund_flipCollectionSGST + view_salereport.Refund_flipReverseShippingSGST + view_salereport.Refund_flipFixedFeeSGST + view_salereport.RefundCommissionSGST;
                                    view_salereport.refund_SumFee = view_salereport.refund_flipShipping + view_salereport.refund_flipCollection + view_salereport.refund_flipReverseShipping + view_salereport.refund_flipFixedFee + view_salereport.RefundCommissionFee + view_salereport.Refund_SUMIGST + view_salereport.Refund_SUMCGST + view_salereport.Refund_SUMSGST;
                                    view_salereport.refund_SumOrder = view_salereport.refundTotal + view_salereport.refund_SumFee;

                                    view_salereport.SUMIGST = view_salereport.flipShippingFeeIGST + view_salereport.flipCollectionIGST + view_salereport.flipReverseShippingIGST + view_salereport.flipFixedFeeIGST + view_salereport.CommissionIGST;
                                    view_salereport.SUMCGST = view_salereport.flipShippingFeeCGST + view_salereport.flipCollectionCGST + view_salereport.flipReverseShippingCGST + view_salereport.flipFixedFeeCGST + view_salereport.CommissionCGST;
                                    view_salereport.SUMSGST = view_salereport.flipShippingFeeSGST + view_salereport.flipCollectionSGST + view_salereport.flipReverseShippingSGST + view_salereport.flipFixedFeeSGST + view_salereport.CommissionSGST;
                                    view_salereport.SumFee = view_salereport.flipShipping + view_salereport.flipCollection + view_salereport.flipReverseShipping + view_salereport.flipFixedFee + view_salereport.CommissionFee + view_salereport.SUMIGST + view_salereport.SUMCGST + view_salereport.SUMSGST;

                                    view_salereport.SumOrder = view_salereport.orderTotal + view_salereport.SumFee;
                                    view_salereport.NetTotal = view_salereport.SumOrder + view_salereport.refund_SumOrder;

                                    string value = "";
                                    value = Convert.ToString((view_salereport.NetTotal / view_salereport.orderTotal) * 100);
                                    if (value != "NaN" && value != "-Infinity" && value != "Infinity")
                                    {
                                        decimal abc = Convert.ToDecimal((view_salereport.NetTotal / view_salereport.orderTotal) * 100);
                                        decimal result = decimal.Round(abc, 2, MidpointRounding.AwayFromZero);

                                        string bb = value.ToString();
                                        string[] b = bb.Split('.');
                                        int firstValue = int.Parse(b[0]);

                                        string fvalue = result + "%";
                                        view_salereport.PercentageAmount = Convert.ToString(fvalue);
                                    }
                                    if (PercentageFilter(ddl_percentage, view_salereport.PercentageAmount))
                                        lstOrdertext2.Add(view_salereport);

                                }// end of for (main counter)
                            }// end of if (get_settlement_data)

                        }// end of using reader

                    }// end of using connection
                }// end of if (txt_from)

            }// end Try block
            catch (Exception ex)
            {

            }// end catch block
            return Success;
        }

        public string Get_SalesReport_Paytm(List<SaleReport> lstOrdertext2, SaleReport view_salereport, DateTime? txt_from, DateTime? txt_to, int sellers_id, int? ddl_market_place, int? ddl_percentage)
        {
            string Msg = "S";
            try
            {
                if (txt_from != null && txt_to != null)
                {
                    using (var connection = dba.Database.Connection)
                    {
                        connection.Open();
                        var command = connection.CreateCommand();
                        command.CommandText = "get_settlement_report"; // "get_sett_orders";
                        command.CommandType = CommandType.StoredProcedure;

                        MySql.Data.MySqlClient.MySqlParameter param, param1, param2, param3;
                        param = new MySql.Data.MySqlClient.MySqlParameter("@sellerid", sellers_id);
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.Int32;
                        command.Parameters.Add(param);

                        param1 = new MySql.Data.MySqlClient.MySqlParameter("@datefrom", txt_from);
                        param1.Direction = ParameterDirection.Input;
                        param1.DbType = DbType.DateTime;
                        command.Parameters.Add(param1);

                        param2 = new MySql.Data.MySqlClient.MySqlParameter("@dateto", txt_to);
                        param2.Direction = ParameterDirection.Input;
                        param2.DbType = DbType.DateTime;
                        command.Parameters.Add(param2);

                        param3 = new MySql.Data.MySqlClient.MySqlParameter("@marketplaceid", ddl_market_place);
                        param3.Direction = ParameterDirection.Input;
                        param3.DbType = DbType.Int32;
                        command.Parameters.Add(param3);

                        using (var reader = command.ExecuteReader())
                        {
                            var get_settlement_data =
                               ((System.Data.Entity.Infrastructure.IObjectContextAdapter)dba)
                                   .ObjectContext
                                   .Translate<proc_Settlement_report>(reader)
                                   .ToList();

                            reader.NextResult();

                            var get_settlement_data1 =
                                ((System.Data.Entity.Infrastructure.IObjectContextAdapter)dba)
                                    .ObjectContext
                                    .Translate<proc_Settlement_report>(reader)
                                    .ToList();

                            reader.NextResult();

                            var get_settlement_data2 =
                                ((System.Data.Entity.Infrastructure.IObjectContextAdapter)dba)
                                    .ObjectContext
                                    .Translate<proc_Settlement_report>(reader)
                                    .ToList();

                            reader.NextResult();

                            var get_history_data =
                                ((System.Data.Entity.Infrastructure.IObjectContextAdapter)dba)
                                    .ObjectContext
                                    .Translate<proc_Settlement_report>(reader)
                                    .ToList();

                            connection.Close();
                            dba = new SellerContext();
                            dba.Configuration.AutoDetectChangesEnabled = false;
                            if (get_settlement_data != null)
                            {
                                for (int maincounter = 0; maincounter < get_settlement_data.Count; maincounter++)
                                {

                                    view_salereport = new SaleReport();
                                    var sett = get_settlement_data[maincounter];
                                    if (sett.amazon_order_id == "5255955829")
                                    {
                                    }
                                    string amazon_order_id = sett.amazon_order_id;
                                    string details_sku = sett.sku_no;
                                    int orderdetails_id = sett.orderdetailsid;
                                    view_salereport.OrderID = sett.amazon_order_id;
                                    view_salereport.OrderDate = Convert.ToDateTime(sett.purchase_date).ToString("yyyy-MM-dd");
                                    view_salereport.DispatchDate = Convert.ToDateTime(sett.Latest_ShipDate).ToString("yyyy-MM-dd");
                                    double Principal = 0, itemtax_amount = 0, Shipping = 0, orderigst = 0, ordersgst = 0, ordercgst = 0, shippingtax = 0, Giftwrap = 0, itempromotion = 0, shippingpromotion = 0;

                                    view_salereport.ProductName = sett.product_name;
                                    view_salereport.skuNo = sett.sku_no;

                                    Principal = Convert.ToDouble(sett.item_price_amount);
                                    Shipping = Convert.ToDouble(sett.shipping_price_Amount);
                                    Giftwrap = Convert.ToDouble(sett.giftwrapprice_amount);
                                    shippingtax = Convert.ToDouble(sett.shipping_tax_Amount);
                                    itempromotion = Convert.ToDouble(sett.item_promotionAmount);
                                    shippingpromotion = Convert.ToDouble(sett.promotion_amount);
                                    //itemtax_amount = Convert.ToDouble(sett.item_tax_amount);

                                    view_salereport.Principal = Principal + Shipping + Giftwrap - itempromotion - shippingpromotion;

                                    if (get_settlement_data1 != null)
                                    {
                                        foreach (var sett5 in get_settlement_data1)// use for tax data
                                        {
                                            //int refer_id = Convert.ToInt16(sett5.tbl_referneced_id);
                                            if (orderdetails_id == sett5.tbl_referneced_id)
                                            {
                                                orderigst = Convert.ToDouble(sett5.Igst_amount);
                                                ordersgst = Convert.ToDouble(sett5.sgst_amount);
                                                ordercgst = Convert.ToDouble(sett5.CGST_amount);

                                                view_salereport.orderigst = orderigst;
                                                view_salereport.ordersgst = ordersgst;
                                                view_salereport.ordercgst = ordercgst;
                                            }
                                        }
                                    }// end of if(get_settlement_data1)

                                    view_salereport.ordertotal = view_salereport.Principal + view_salereport.orderigst + view_salereport.ordersgst + view_salereport.ordercgst;

                                    #region get settlement details with expense and tax

                                    if (get_settlement_data2 != null)
                                    {
                                        foreach (var settle in get_settlement_data2)
                                        {
                                            string skuno = settle.Settlement_sku.ToLower();
                                            if (amazon_order_id == settle.Order_Id && details_sku.ToLower() == skuno)
                                            {
                                                view_salereport.orderprincipal = Convert.ToDouble(settle.principal_price);
                                                view_salereport.orderproduct_tax = Convert.ToDouble(settle.product_tax);
                                                view_salereport.ordershipping = Convert.ToDouble(settle.shipping_price);
                                                view_salereport.ordershipping_tax = Convert.ToDouble(settle.shipping_tax);
                                                view_salereport.ordergiftwrap = Convert.ToDouble(settle.giftwrap_price);
                                                view_salereport.ordergiftwrap_tax = Convert.ToDouble(settle.giftwarp_tax);
                                                view_salereport.ordershipping_discount = Convert.ToDouble(settle.shipping_discount);
                                                view_salereport.ordershipping_discounttax = Convert.ToDouble(settle.shipping_tax_discount);

                                                view_salereport.orderTotal = view_salereport.orderprincipal + view_salereport.orderproduct_tax + view_salereport.ordershipping + view_salereport.ordershipping_tax + view_salereport.ordergiftwrap + view_salereport.ordergiftwrap_tax + view_salereport.ordershipping_discount + view_salereport.ordershipping_discounttax;
                                                view_salereport.ReferenceID = settle.settlement_id;

                                                //-------------------------End---------------------------//
                                                //-----------------------Get  data from tbl_Expense from settlement_Id----------------//

                                                double FBAFEE = 0, FBAIGST = 0, FBACGST = 0, FBASGST = 0, TechnologyFee = 0, TechnologyIGST = 0, TechnologyCGST = 0, TechnologySGST = 0, CommissionFee = 0, CommissionIGST = 0, CommissionCGST = 0, CommissionSGST = 0, FixedClosingFee = 0, FixedclosingIGST = 0,
                                                       FixedclosingCGST = 0, FixedclosingSGST = 0, ShippingChargebackFee = 0,
                                                       shippingchargeCGST = 0, shippingchargeSGST = 0, shippingchargeIGST = 0;


                                                var getsettlementdetails = dba.m_tbl_expense.Where(a => a.settlement_order_id.Contains(amazon_order_id) && a.sku_no.ToLower() == skuno && a.tbl_seller_id == sellers_id && a.t_transactionType_id == 1).ToList();
                                                if (getsettlementdetails != null && getsettlementdetails.Count > 0)
                                                {
                                                    foreach (var item1 in getsettlementdetails)
                                                    {
                                                        var exp_id = item1.expense_type_id;
                                                        var get_expdetails = dba.m_settlement_fee.Where(a => a.id == exp_id).FirstOrDefault();
                                                        var gettax_details = dba.tbl_tax.Where(a => a.tbl_referneced_id == item1.id && a.reference_type == 2).FirstOrDefault();
                                                        if (get_expdetails != null)
                                                        {
                                                            string nam = get_expdetails.return_fee;
                                                            if (nam == "Marketplace Commission")
                                                            {
                                                                if (get_expdetails.id == exp_id)
                                                                {
                                                                    FBAFEE = FBAFEE + Convert.ToDouble(item1.expense_amount);
                                                                    view_salereport.FBAFEE = FBAFEE;
                                                                    if (gettax_details != null)
                                                                    {
                                                                        FBAIGST = FBAIGST + Convert.ToDouble(gettax_details.Igst_amount);//new
                                                                        FBACGST = FBACGST + Convert.ToDouble(gettax_details.CGST_amount);
                                                                        FBASGST = FBASGST + Convert.ToDouble(gettax_details.sgst_amount);
                                                                        view_salereport.FBACGST = FBACGST;
                                                                        view_salereport.FBASGST = FBASGST;
                                                                        view_salereport.FBAIGST = FBAIGST;//new
                                                                    }
                                                                }
                                                            }
                                                            else if (nam == "Logistics Charges")
                                                            {
                                                                if (get_expdetails.id == exp_id)
                                                                {
                                                                    TechnologyFee = TechnologyFee + Convert.ToDouble(item1.expense_amount);
                                                                    view_salereport.TechnologyFee = TechnologyFee;
                                                                    if (gettax_details != null)
                                                                    {
                                                                        TechnologyIGST = TechnologyIGST + Convert.ToDouble(gettax_details.Igst_amount);
                                                                        TechnologyCGST = TechnologyCGST + Convert.ToDouble(gettax_details.CGST_amount);
                                                                        TechnologySGST = TechnologySGST + Convert.ToDouble(gettax_details.sgst_amount);

                                                                        view_salereport.TechnologyIGST = TechnologyIGST;
                                                                        view_salereport.TechnologyCGST = TechnologyCGST;
                                                                        view_salereport.TechnologySGST = TechnologySGST;
                                                                    }
                                                                }
                                                            }
                                                            else if (nam == "PG Commission")
                                                            {
                                                                if (get_expdetails.id == exp_id)
                                                                {
                                                                    CommissionFee = CommissionFee + Convert.ToDouble(item1.expense_amount);
                                                                    view_salereport.CommissionFee = CommissionFee;
                                                                    if (gettax_details != null)
                                                                    {
                                                                        CommissionIGST = CommissionIGST + Convert.ToDouble(gettax_details.Igst_amount);
                                                                        CommissionCGST = CommissionCGST + Convert.ToDouble(gettax_details.CGST_amount);
                                                                        CommissionSGST = CommissionSGST + Convert.ToDouble(gettax_details.sgst_amount);
                                                                        view_salereport.CommissionIGST = CommissionIGST;
                                                                        view_salereport.CommissionCGST = CommissionCGST;
                                                                        view_salereport.CommissionSGST = CommissionSGST;
                                                                    }
                                                                }
                                                            }
                                                            else if (nam == "Penalty")
                                                            {
                                                                if (get_expdetails.id == exp_id)
                                                                {
                                                                    FixedClosingFee = FixedClosingFee + Convert.ToDouble(item1.expense_amount);
                                                                    view_salereport.FixedClosingFee = FixedClosingFee;
                                                                    if (gettax_details != null)
                                                                    {
                                                                        FixedclosingIGST = FixedclosingIGST + Convert.ToDouble(gettax_details.Igst_amount);
                                                                        FixedclosingCGST = FixedclosingCGST + Convert.ToDouble(gettax_details.CGST_amount);
                                                                        FixedclosingSGST = FixedclosingSGST + Convert.ToDouble(gettax_details.sgst_amount);

                                                                        view_salereport.FixedclosingIGST = FixedclosingIGST;
                                                                        view_salereport.FixedclosingCGST = FixedclosingCGST;
                                                                        view_salereport.FixedclosingSGST = FixedclosingSGST;
                                                                    }
                                                                }
                                                            }
                                                            else if (nam == "Net Adjustments")
                                                            {
                                                                if (get_expdetails.id == exp_id)
                                                                {
                                                                    ShippingChargebackFee = ShippingChargebackFee + Convert.ToDouble(item1.expense_amount);
                                                                    view_salereport.ShippingChargebackFee = ShippingChargebackFee;
                                                                    if (gettax_details != null)
                                                                    {
                                                                        shippingchargeIGST = shippingchargeIGST + Convert.ToDouble(gettax_details.Igst_amount);
                                                                        shippingchargeCGST = shippingchargeCGST + Convert.ToDouble(gettax_details.CGST_amount);
                                                                        shippingchargeSGST = shippingchargeSGST + Convert.ToDouble(gettax_details.sgst_amount);
                                                                        view_salereport.shippingchargeCGST = shippingchargeCGST;
                                                                        view_salereport.shippingchargeSGST = shippingchargeSGST;
                                                                        view_salereport.shippingchargeIGST = shippingchargeIGST;
                                                                    }

                                                                }
                                                            }


                                                        }// end of if(get_expdetails)
                                                    }// end if foreach(item1)
                                                }// end of if(getsettlementdetails) 
                                            }
                                        }// end of if (compare sku and orderid)
                                    }
                                    #endregion

                                    #region to get history table with expense
                                    if (get_history_data != null)
                                    {
                                        foreach (var history in get_history_data)
                                        {
                                            string sku_no = history.history_sku.ToLower();
                                            if (amazon_order_id == history.history_OrderID && details_sku.ToLower() == sku_no)
                                            {
                                                view_salereport.refundprincipal = Convert.ToDouble(history.history_amount_per_unit);
                                                view_salereport.refundproduct_tax = Convert.ToDouble(history.history_product_tax);
                                                view_salereport.refundshipping = Convert.ToDouble(history.history_shipping_price);
                                                view_salereport.refundshipping_tax = Convert.ToDouble(history.history_shipping_tax);
                                                view_salereport.refundgiftwrap = Convert.ToDouble(history.history_Giftwrap_price);
                                                view_salereport.refundgiftwrap_tax = Convert.ToDouble(history.history_gift_wrap_tax);
                                                view_salereport.refundshipping_discount = Convert.ToDouble(history.history_shipping_discount);
                                                view_salereport.refundshipping_discount_tax = Convert.ToDouble(history.history_shipping_tax_discount);

                                                view_salereport.refundTotal = view_salereport.refundprincipal + view_salereport.refundproduct_tax + view_salereport.refundshipping + view_salereport.refundshipping_tax + view_salereport.refundgiftwrap + view_salereport.refundgiftwrap_tax + view_salereport.refundshipping_discount + view_salereport.refundshipping_discount_tax;

                                                view_salereport.refundReferenceID = history.history_settlement_id;

                                                double refundFBAFEE = 0, refundFBACGST = 0, refundFBASGST = 0, refundFBAIGST = 0, refundTechnologyFee = 0, refundTechnologyIGST = 0, refundTechnologyCGST = 0, refundTechnologySGST = 0, refundCommissionFee = 0, refundCommissionIGST = 0, refundCommissionCGST = 0,
                                                       refundCommissionSGST = 0, refundFixedClosingFee = 0, refundFixedclosingIGST = 0,
                                                       refundFixedclosingCGST = 0, refundFixedclosingSGST = 0, refundShippingChargebackFee = 0,
                                                       refundshippingchargeCGST = 0, refundshippingchargeSGST = 0, refundshippingchargeIGST = 0;



                                                var get_refundexpense = dba.m_tbl_expense.Where(a => a.settlement_order_id.Contains(amazon_order_id) && a.sku_no.ToLower() == details_sku.ToLower() && a.tbl_seller_id == sellers_id && a.t_transactionType_id == 2).ToList();// to get refund expense
                                                if (get_refundexpense != null && get_refundexpense.Count > 0)
                                                {
                                                    foreach (var refund in get_refundexpense)
                                                    {
                                                        var exp_ID = refund.expense_type_id;
                                                        var get_details = dba.m_settlement_fee.Where(a => a.id == exp_ID).FirstOrDefault();
                                                        var getExp_tax_details = dba.tbl_tax.Where(a => a.tbl_referneced_id == refund.id && a.reference_type == 7).FirstOrDefault();
                                                        if (get_details != null)
                                                        {
                                                            string nam = get_details.return_fee;
                                                            if (nam == "Marketplace Commission")
                                                            {
                                                                if (get_details.id == exp_ID)
                                                                {
                                                                    refundFBAFEE = refundFBAFEE + Convert.ToDouble(refund.expense_amount);
                                                                    view_salereport.RefundFBAFEE = refundFBAFEE;
                                                                    if (getExp_tax_details != null)
                                                                    {
                                                                        refundFBAIGST = refundFBAIGST + Convert.ToDouble(getExp_tax_details.Igst_amount);
                                                                        refundFBACGST = refundFBACGST + Convert.ToDouble(getExp_tax_details.CGST_amount);
                                                                        refundFBASGST = refundFBASGST + Convert.ToDouble(getExp_tax_details.sgst_amount);

                                                                        view_salereport.RefundFBACGST = refundFBACGST;
                                                                        view_salereport.RefundFBASGST = refundFBASGST;
                                                                        view_salereport.RefundFBAIGST = refundFBAIGST;
                                                                    }
                                                                }
                                                            }
                                                            else if (nam == "Logistics Charges")
                                                            {
                                                                if (get_details.id == exp_ID)
                                                                {
                                                                    refundTechnologyFee = refundTechnologyFee + Convert.ToDouble(refund.expense_amount);
                                                                    view_salereport.RefundTechnologyFee = refundTechnologyFee;
                                                                    if (getExp_tax_details != null)
                                                                    {
                                                                        refundTechnologyIGST = refundTechnologyIGST + Convert.ToDouble(getExp_tax_details.Igst_amount);
                                                                        refundTechnologyCGST = refundTechnologyCGST + Convert.ToDouble(getExp_tax_details.CGST_amount);
                                                                        refundTechnologySGST = refundTechnologySGST + Convert.ToDouble(getExp_tax_details.sgst_amount);

                                                                        view_salereport.RefundTechnologyIGST = refundTechnologyIGST;
                                                                        view_salereport.RefundTechnologyCGST = refundTechnologyCGST;
                                                                        view_salereport.RefundTechnologySGST = refundTechnologySGST;
                                                                    }
                                                                }
                                                            }
                                                            else if (nam == "PG Commission")
                                                            {
                                                                if (get_details.id == exp_ID)
                                                                {
                                                                    refundCommissionFee = refundCommissionFee + Convert.ToDouble(refund.expense_amount);
                                                                    view_salereport.RefundCommissionFee = refundCommissionFee;
                                                                    if (getExp_tax_details != null)
                                                                    {
                                                                        refundCommissionIGST = refundCommissionIGST + Convert.ToDouble(getExp_tax_details.Igst_amount);
                                                                        refundCommissionCGST = refundCommissionCGST + Convert.ToDouble(getExp_tax_details.CGST_amount);
                                                                        refundCommissionSGST = refundCommissionSGST + Convert.ToDouble(getExp_tax_details.sgst_amount);

                                                                        view_salereport.RefundCommissionIGST = refundCommissionIGST;
                                                                        view_salereport.RefundCommissionCGST = refundCommissionCGST;
                                                                        view_salereport.RefundCommissionSGST = refundCommissionSGST;
                                                                    }
                                                                }
                                                            }
                                                            else if (nam == "Penalty")
                                                            {
                                                                if (get_details.id == exp_ID)
                                                                {
                                                                    refundFixedClosingFee = refundFixedClosingFee + Convert.ToDouble(refund.expense_amount);
                                                                    view_salereport.RefundFixedClosingFee = refundFixedClosingFee;
                                                                    if (getExp_tax_details != null)
                                                                    {
                                                                        refundFixedclosingIGST = refundFixedclosingIGST + Convert.ToDouble(getExp_tax_details.Igst_amount);
                                                                        refundFixedclosingCGST = refundFixedclosingCGST + Convert.ToDouble(getExp_tax_details.CGST_amount);
                                                                        refundFixedclosingSGST = refundFixedclosingSGST + Convert.ToDouble(getExp_tax_details.sgst_amount);

                                                                        view_salereport.RefundFixedclosingIGST = refundFixedclosingIGST;
                                                                        view_salereport.RefundFixedclosingCGST = refundFixedclosingCGST;
                                                                        view_salereport.RefundFixedclosingSGST = refundFixedclosingSGST;
                                                                    }
                                                                }
                                                            }
                                                            else if (nam == "Net Adjustments")
                                                            {
                                                                if (get_details.id == exp_ID)
                                                                {
                                                                    refundShippingChargebackFee = refundShippingChargebackFee + Convert.ToDouble(refund.expense_amount);
                                                                    view_salereport.RefundShippingChargebackFee = refundShippingChargebackFee;
                                                                    if (getExp_tax_details != null)
                                                                    {
                                                                        refundshippingchargeIGST = refundshippingchargeIGST + Convert.ToDouble(getExp_tax_details.Igst_amount);
                                                                        refundshippingchargeCGST = refundshippingchargeCGST + Convert.ToDouble(getExp_tax_details.CGST_amount);
                                                                        refundshippingchargeSGST = refundshippingchargeSGST + Convert.ToDouble(getExp_tax_details.sgst_amount);
                                                                        view_salereport.RefundshippingchargeCGST = refundshippingchargeCGST;
                                                                        view_salereport.RefundshippingchargeSGST = refundshippingchargeSGST;
                                                                        view_salereport.RefundshippingchargeIGST = refundshippingchargeIGST;
                                                                    }
                                                                }
                                                            }



                                                        }// end of if(get_details)                               
                                                    }// end of foreach(refund)
                                                }// end of if(get_refundexpense)

                                            }// end of if(compare orderid and sku)                                                                                        
                                        }// end of for each loop(history)
                                    }// end of if (check not null)

                                    #endregion


                                    view_salereport.Refund_SUMIGST = view_salereport.RefundFBAIGST + view_salereport.RefundshippingchargeIGST + view_salereport.RefundTechnologyIGST + view_salereport.RefundCommissionIGST + view_salereport.RefundFixedclosingIGST;
                                    view_salereport.Refund_SUMCGST = view_salereport.RefundshippingchargeCGST + view_salereport.RefundFBACGST + view_salereport.RefundFixedclosingCGST + view_salereport.RefundCommissionCGST + view_salereport.RefundTechnologyCGST;
                                    view_salereport.Refund_SUMSGST = view_salereport.RefundshippingchargeSGST + view_salereport.RefundFBASGST + view_salereport.RefundFixedclosingSGST + view_salereport.RefundCommissionSGST + view_salereport.RefundTechnologySGST;
                                    view_salereport.refund_SumFee = view_salereport.RefundFBAFEE + view_salereport.RefundTechnologyFee + view_salereport.RefundCommissionFee + view_salereport.RefundFixedClosingFee + view_salereport.RefundShippingChargebackFee;
                                    view_salereport.refund_SumOrder = view_salereport.refundTotal + view_salereport.refund_SumFee;

                                    view_salereport.SUMIGST = view_salereport.FBAIGST + view_salereport.TechnologyIGST + view_salereport.CommissionIGST + view_salereport.FixedclosingIGST + view_salereport.shippingchargeIGST;
                                    view_salereport.SUMCGST = view_salereport.shippingchargeCGST + view_salereport.FBACGST + view_salereport.FixedclosingCGST + view_salereport.CommissionCGST + view_salereport.TechnologyCGST;
                                    view_salereport.SUMSGST = view_salereport.shippingchargeSGST + view_salereport.FBASGST + view_salereport.FixedclosingSGST + view_salereport.CommissionSGST + view_salereport.TechnologySGST;
                                    view_salereport.SumFee = view_salereport.FBAFEE + view_salereport.TechnologyFee + view_salereport.CommissionFee + view_salereport.FixedClosingFee + view_salereport.ShippingChargebackFee;
                                    view_salereport.SumOrder = view_salereport.orderTotal + view_salereport.SumFee + view_salereport.SUMIGST;
                                    view_salereport.NetTotal = view_salereport.SumOrder + view_salereport.refund_SumOrder;



                                    view_salereport.ActualOrderTotal = view_salereport.orderTotal + view_salereport.refundTotal;
                                    view_salereport.ActualCommission = view_salereport.CommissionFee + view_salereport.RefundCommissionFee;//PG Commission
                                    view_salereport.ActualFBAFee = view_salereport.FBAFEE + view_salereport.RefundFBAFEE;//Marketcommission
                                    view_salereport.ActualFixedClosingFee = view_salereport.FixedClosingFee + view_salereport.RefundFixedClosingFee;//Penalty
                                    view_salereport.ActualShippingChargebackFee = view_salereport.ShippingChargebackFee + view_salereport.RefundShippingChargebackFee;//Net Adjustments
                                    view_salereport.ActualTechnologyFee = view_salereport.TechnologyFee + view_salereport.RefundTechnologyFee;//Logistics Charges

                                    view_salereport.ActualIGST = view_salereport.SUMIGST + view_salereport.Refund_SUMIGST;
                                    view_salereport.ActualCGST = view_salereport.SUMCGST + view_salereport.Refund_SUMCGST;
                                    view_salereport.ActualSGST = view_salereport.SUMSGST + view_salereport.Refund_SUMSGST;
                                    view_salereport.ActualNetTotal = view_salereport.ActualOrderTotal + view_salereport.ActualCommission + view_salereport.ActualFBAFee
                                        + view_salereport.ActualFixedClosingFee + view_salereport.ActualShippingChargebackFee + view_salereport.ActualTechnologyFee
                                        + view_salereport.ActualIGST + view_salereport.ActualCGST + view_salereport.ActualSGST;


                                    if (view_salereport.orderTotal != 0)
                                    {
                                        view_salereport.PercentageAmount =
                                        decimal.Round((decimal)(view_salereport.ActualNetTotal / view_salereport.orderTotal) * 100, 2, MidpointRounding.AwayFromZero) + "%";
                                    }

                                    //string value = "";
                                    //value = Convert.ToString((view_salereport.NetTotal / view_salereport.orderTotal) * 100);
                                    //if (value != "NaN" && value != "-Infinity" && value != "Infinity")
                                    //{
                                    //    decimal abc = Convert.ToDecimal((view_salereport.NetTotal / view_salereport.orderTotal) * 100);
                                    //    decimal result = decimal.Round(abc, 2, MidpointRounding.AwayFromZero);

                                    //    string bb = value.ToString();
                                    //    string[] b = bb.Split('.');
                                    //    int firstValue = int.Parse(b[0]);

                                    //    string fvalue = result + "%";
                                    //    view_salereport.PercentageAmount = Convert.ToString(fvalue);
                                    //}

                                    if (PercentageFilter(ddl_percentage, view_salereport.PercentageAmount))
                                        lstOrdertext2.Add(view_salereport);

                                    // lstOrdertext2.Add(view_salereport);
                                }// end of for (counter)


                            }//end of if(get_settlement_data)
                        }
                    }// end of using connection
                }// end of if (txt from )
            }// end of try block
            catch (Exception ex)
            {
            }
            return Msg;
        }

        #endregion
    }
}