using SellerVendor.Areas.Seller.Models;
using SellerVendor.Areas.Seller.Models.DBContext;
using SellerVendor.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
namespace SellerVendor.Areas.Seller.Models
{
    public class Report_Utility
    {
        public SellerContext dba = new SellerContext();
        public SellerAdminContext db = new SellerAdminContext();

        public void Expense_Report(List<SaleReport> lstOrdertext2, SaleReport view_salereport, SaleReport objsales, int? ddl_market_place, DateTime? txt_from, DateTime? txt_to, int sellers_id)
        {
            try
            {
                if (txt_from != null && txt_to != null)
                {
                    decimal CommissionTotal = 0, FBATotal = 0, TechnologyTotal = 0, FixedClosingTotal = 0, ShippingCharegeTotal = 0, RefundCommTotal = 0, MarketCommission = 0, Penanlty = 0, PgCommission = 0, AdjustmentComm = 0, LogisticCharges = 0;
                    decimal Refund_CommissionTotal = 0, Refund_FBATotal = 0, Refund_TechnologyTotal = 0, Refund_FixedClosingTotal = 0, Refund_ShippingCharegeTotal = 0, Refund_RefundCommTotal = 0, RefMarketCommission = 0, RefPenanlty = 0, RefPgCommission = 0, RefAdjustmentComm = 0, RefLogisticCharges = 0;
                    var sales_order_list = (from ob_tbl_sales_order in dba.tbl_sales_order
                                            select new
                                            {
                                                ob_tbl_sales_order = ob_tbl_sales_order
                                            }).Where(a => a.ob_tbl_sales_order.tbl_sellers_id == sellers_id && a.ob_tbl_sales_order.is_active == 1 && a.ob_tbl_sales_order.tbl_Marketplace_Id == ddl_market_place && (a.ob_tbl_sales_order.purchase_date >= txt_from && a.ob_tbl_sales_order.purchase_date <= txt_to)).ToList();
                    //.Where(a => a.ob_tbl_sales_order.tbl_sellers_id == sellers_id).ToList();from ob_tbl_sales_order in dba.tbl_sales_order

                    foreach (var item_sale_order in sales_order_list)
                    {
                        view_salereport = new SaleReport();
                        int sale_order_id = Convert.ToInt32(item_sale_order.ob_tbl_sales_order.id);
                        string amazon_order_id = item_sale_order.ob_tbl_sales_order.amazon_order_id;
                        if (amazon_order_id == "408-6328489-4373908")
                        {
                        }
                        DateTime? purchase_date = item_sale_order.ob_tbl_sales_order.purchase_date;
                        if (view_salereport.OrderID != item_sale_order.ob_tbl_sales_order.amazon_order_id)
                        {
                            if (view_salereport.OrderID != null)
                                lstOrdertext2.Add(view_salereport);
                        }
                        int fillItemCount = lstOrdertext2.Count;
                        if (fillItemCount > 0)
                        {
                            if (lstOrdertext2[fillItemCount - 1].OrderID != item_sale_order.ob_tbl_sales_order.amazon_order_id && view_salereport.OrderID != item_sale_order.ob_tbl_sales_order.amazon_order_id)
                                view_salereport = new SaleReport();
                        }
                        /////////////////////// DECLARE VARIABLES FOR EXPENSE //////////////////////////////
                        double FBAFEE = 0, TechnologyFee = 0, CommissionFee = 0, FixedClosingFee = 0, ShippingChargebackFee = 0, RefundCommision = 0, Refund_Commision = 0, RefundShippingChargebackFee = 0, RefundFixedClosingFee = 0, RefundCommissionFee = 0, RefundTechnologyFee = 0, RefundFBAFEE = 0,
                            Marketplacecommission = 0, Penaty = 0, LogisticsCharges = 0, PGCommission = 0, NetAdjustments = 0, RefundMarketplacecommission = 0, RefundPenaty = 0, RefundLogisticsCharges = 0, RefundPGCommission = 0, RefundNetAdjustments = 0;
                        ////////////////////// GET SALES ORDER DETAIL ITEM LIST ///////////////////////////
                        var sales_order_detail_list = (from ob_tbl_sales_order_details in dba.tbl_sales_order_details
                                                       select new
                                                       {
                                                           ob_tbl_sales_order_details = ob_tbl_sales_order_details
                                                       }).Where(d => d.ob_tbl_sales_order_details.tbl_sales_order_id == sale_order_id).ToList();

                        view_salereport.OrderID = amazon_order_id;
                        view_salereport.OrderDate = Convert.ToDateTime(purchase_date).ToString("yyyy-MM-dd");
                        foreach (var item_sale_order_detail in sales_order_detail_list)
                        {
                            int sale_order_detail_id = Convert.ToInt32(item_sale_order_detail.ob_tbl_sales_order_details.id);
                            string sku = item_sale_order_detail.ob_tbl_sales_order_details.sku_no;

                            //////////////////////// START COLLECT EXPENSE DATA //////////////////////////
                            var getsettlementdetails = dba.m_tbl_expense.Where(a => a.settlement_order_id.Contains(amazon_order_id) && a.tbl_seller_id == sellers_id && a.t_transactionType_id == 1).ToList();
                            foreach (var item_expense_detail in getsettlementdetails) //(var item1 in getsettlementdetails)
                            {
                                var exp_id = item_expense_detail.expense_type_id;
                                view_salereport.ReferenceID = item_expense_detail.reference_number;
                                view_salereport.SettlementDate = Convert.ToDateTime(item_expense_detail.settlement_datetime).ToString("yyyy-MM-dd");

                                var get_expdetails = dba.m_settlement_fee.Where(a => a.id == exp_id).FirstOrDefault();
                                if (get_expdetails != null)
                                {
                                    string nam = get_expdetails.return_fee;
                                    if (nam == "FBA Weight Handling Fee")
                                    {
                                        if (get_expdetails.id == exp_id)
                                        {
                                            FBAFEE += Convert.ToDouble(item_expense_detail.expense_amount);
                                            //view_salereport.FBAFEE = Convert.ToDouble(item_expense_detail.expense_amount);
                                        }
                                    }
                                    else if (nam == "Technology Fee")
                                    {
                                        if (get_expdetails.id == exp_id)
                                        {
                                            TechnologyFee += Convert.ToDouble(item_expense_detail.expense_amount);
                                            //view_salereport.TechnologyFee = Convert.ToDouble(item_expense_detail.expense_amount);
                                        }
                                    }
                                    else if (nam == "Commission")
                                    {
                                        if (get_expdetails.id == exp_id)
                                        {
                                            CommissionFee += Convert.ToDouble(item_expense_detail.expense_amount);
                                            //view_salereport.CommissionFee = Convert.ToDouble(item_expense_detail.expense_amount);
                                        }
                                    }
                                    else if (nam == "Fixed closing fee")
                                    {
                                        if (get_expdetails.id == exp_id)
                                        {
                                            FixedClosingFee += Convert.ToDouble(item_expense_detail.expense_amount);
                                            //view_salereport.FixedClosingFee = Convert.ToDouble(item_expense_detail.expense_amount);
                                        }
                                    }
                                    else if (nam == "Shipping Chargeback")
                                    {
                                        if (get_expdetails.id == exp_id)
                                        {
                                            ShippingChargebackFee += Convert.ToDouble(item_expense_detail.expense_amount);
                                            //view_salereport.ShippingChargebackFee = Convert.ToDouble(item_expense_detail.expense_amount);
                                        }
                                    }

                                    else if (nam == "Refund commission")
                                    {
                                        if (get_expdetails.id == exp_id)
                                        {
                                            RefundCommision += Convert.ToDouble(item_expense_detail.expense_amount);
                                            //view_salereport.RefundCommision = Convert.ToDouble(item_expense_detail.expense_amount);
                                        }
                                    }

                                    else if (nam == "Marketplace Commission")
                                    {
                                        if (get_expdetails.id == exp_id)
                                        {
                                            Marketplacecommission += Convert.ToDouble(item_expense_detail.expense_amount);
                                            //view_salereport.RefundCommision = Convert.ToDouble(item_expense_detail.expense_amount);
                                        }
                                    }
                                    else if (nam == "Logistics Charges")
                                    {
                                        if (get_expdetails.id == exp_id)
                                        {
                                            LogisticsCharges += Convert.ToDouble(item_expense_detail.expense_amount);
                                            //view_salereport.RefundCommision = Convert.ToDouble(item_expense_detail.expense_amount);
                                        }
                                    }
                                    else if (nam == "PG Commission")
                                    {
                                        if (get_expdetails.id == exp_id)
                                        {
                                            PGCommission += Convert.ToDouble(item_expense_detail.expense_amount);
                                            //view_salereport.RefundCommision = Convert.ToDouble(item_expense_detail.expense_amount);
                                        }
                                    }
                                    else if (nam == "Penalty")
                                    {
                                        if (get_expdetails.id == exp_id)
                                        {
                                            Penaty += Convert.ToDouble(item_expense_detail.expense_amount);
                                            //view_salereport.RefundCommision = Convert.ToDouble(item_expense_detail.expense_amount);
                                        }
                                    }
                                    else if (nam == "Net Adjustments")
                                    {
                                        if (get_expdetails.id == exp_id)
                                        {
                                            NetAdjustments += Convert.ToDouble(item_expense_detail.expense_amount);
                                            //view_salereport.RefundCommision = Convert.ToDouble(item_expense_detail.expense_amount);
                                        }
                                    }
                                }
                            }
                            //////////////////////// ENDOF COLLECT EXPENSE DATA //////////////////////////
                            //////////////////////// START COLLECT REFUND DATA //////////////////////////

                            var get_historydata = dba.tbl_order_history.Where(a => a.tbl_seller_id == sellers_id && a.OrderID == amazon_order_id && a.t_order_status == 9).FirstOrDefault();
                            if (get_historydata != null)
                            {
                                var get_refundexpense = dba.m_tbl_expense.Where(a => a.settlement_order_id.Contains(amazon_order_id) && a.tbl_seller_id == sellers_id && a.t_transactionType_id == 2 && a.tbl_order_historyid == get_historydata.Id).ToList();// to get refund expense
                                if (get_refundexpense != null && get_refundexpense.Count > 0)
                                {
                                    foreach (var refund in get_refundexpense)
                                    {
                                        var exp_ID = refund.expense_type_id;
                                        var get_details = dba.m_settlement_fee.Where(a => a.id == exp_ID).FirstOrDefault();
                                        var getExp_tax_details = dba.tbl_tax.Where(a => a.tbl_referneced_id == refund.id && a.reference_type == 7 && a.tbl_history_id == get_historydata.Id).FirstOrDefault();
                                        if (get_details != null)
                                        {
                                            string nam = get_details.return_fee;
                                            if (nam == "FBA Weight Handling Fee")
                                            {
                                                if (get_details.id == exp_ID)
                                                {
                                                    RefundFBAFEE = Convert.ToDouble(refund.expense_amount);
                                                    //view_salereport.RefundFBAFEE = Convert.ToDouble(refund.expense_amount);
                                                }
                                            }
                                            else if (nam == "Technology Fee")
                                            {
                                                if (get_details.id == exp_ID)
                                                {
                                                    RefundTechnologyFee = Convert.ToDouble(refund.expense_amount);
                                                    //view_salereport.RefundTechnologyFee = Convert.ToDouble(refund.expense_amount);
                                                }
                                            }
                                            else if (nam == "Commission")
                                            {
                                                if (get_details.id == exp_ID)
                                                {
                                                    RefundCommissionFee = Convert.ToDouble(refund.expense_amount);
                                                    //view_salereport.RefundCommissionFee = Convert.ToDouble(refund.expense_amount);
                                                }
                                            }
                                            else if (nam == "Fixed closing fee")
                                            {
                                                if (get_details.id == exp_ID)
                                                {
                                                    RefundFixedClosingFee = Convert.ToDouble(refund.expense_amount);
                                                    //view_salereport.RefundFixedClosingFee = Convert.ToDouble(refund.expense_amount);
                                                }
                                            }
                                            else if (nam == "Shipping Chargeback")
                                            {
                                                if (get_details.id == exp_ID)
                                                {
                                                    RefundShippingChargebackFee = Convert.ToDouble(refund.expense_amount);
                                                    //view_salereport.RefundShippingChargebackFee = Convert.ToDouble(refund.expense_amount);

                                                }
                                            }

                                            else if (nam == "Refund commission")
                                            {
                                                if (get_details.id == exp_ID)
                                                {
                                                    Refund_Commision = Convert.ToDouble(refund.expense_amount);
                                                    //view_salereport.Refund_Commision = Convert.ToDouble(refund.expense_amount);
                                                }
                                            }
                                            else if (nam == "Marketplace Commission")
                                            {
                                                if (get_details.id == exp_ID)
                                                {
                                                    RefundMarketplacecommission += Convert.ToDouble(refund.expense_amount);
                                                    //view_salereport.RefundCommision = Convert.ToDouble(item_expense_detail.expense_amount);
                                                }
                                            }
                                            else if (nam == "Logistics Charges")
                                            {
                                                if (get_details.id == exp_ID)
                                                {
                                                    RefundLogisticsCharges += Convert.ToDouble(refund.expense_amount);
                                                    //view_salereport.RefundCommision = Convert.ToDouble(item_expense_detail.expense_amount);
                                                }
                                            }
                                            else if (nam == "PG Commission")
                                            {
                                                if (get_details.id == exp_ID)
                                                {
                                                    RefundPGCommission += Convert.ToDouble(refund.expense_amount);
                                                    //view_salereport.RefundCommision = Convert.ToDouble(item_expense_detail.expense_amount);
                                                }
                                            }
                                            else if (nam == "Penalty")
                                            {
                                                if (get_details.id == exp_ID)
                                                {
                                                    RefundPenaty += Convert.ToDouble(refund.expense_amount);
                                                    //view_salereport.RefundCommision = Convert.ToDouble(item_expense_detail.expense_amount);
                                                }
                                            }
                                            else if (nam == "Net Adjustments")
                                            {
                                                if (get_details.id == exp_ID)
                                                {
                                                    RefundNetAdjustments += Convert.ToDouble(refund.expense_amount);
                                                    //view_salereport.RefundCommision = Convert.ToDouble(item_expense_detail.expense_amount);
                                                }
                                            }
                                            }// end of if(get_details)                               
                                        }// end of foreach(refund)
                                    }// end of if(get_refundexpense)
                                
                                //////////////////////// ENDOF COLLECT REFUND DATA //////////////////////////

                            }
                            CommissionTotal += Convert.ToDecimal(view_salereport.CommissionFee);
                            FBATotal += Convert.ToDecimal(view_salereport.FBAFEE);
                            TechnologyTotal += Convert.ToDecimal(view_salereport.TechnologyFee);
                            FixedClosingTotal += Convert.ToDecimal(view_salereport.FixedClosingFee);
                            ShippingCharegeTotal += Convert.ToDecimal(view_salereport.ShippingChargebackFee);
                            RefundCommTotal += Convert.ToDecimal(view_salereport.RefundCommision);
                            Refund_CommissionTotal += Convert.ToDecimal(view_salereport.RefundCommissionFee);
                            Refund_FBATotal += Convert.ToDecimal(view_salereport.RefundFBAFEE);
                            Refund_TechnologyTotal += Convert.ToDecimal(view_salereport.RefundTechnologyFee);
                            Refund_FixedClosingTotal += Convert.ToDecimal(view_salereport.RefundFixedClosingFee);
                            Refund_ShippingCharegeTotal += Convert.ToDecimal(view_salereport.RefundShippingChargebackFee);
                            Refund_RefundCommTotal += Convert.ToDecimal(view_salereport.Refund_Commision);


                            MarketCommission += Convert.ToDecimal(view_salereport.Marketplacecommission);
                            Penanlty += Convert.ToDecimal(view_salereport.Penaty);
                            PgCommission += Convert.ToDecimal(view_salereport.PGCommission);
                            AdjustmentComm += Convert.ToDecimal(view_salereport.NetAdjustments);
                            LogisticCharges += Convert.ToDecimal(view_salereport.LogisticsCharges);

                            RefMarketCommission += Convert.ToDecimal(view_salereport.RefundMarketplacecommission);
                            RefPenanlty += Convert.ToDecimal(view_salereport.RefundPenaty);
                            RefPgCommission += Convert.ToDecimal(view_salereport.RefundPGCommission);
                            RefAdjustmentComm += Convert.ToDecimal(view_salereport.RefundNetAdjustments);
                            RefLogisticCharges += Convert.ToDecimal(view_salereport.RefundLogisticsCharges);


                        }
                        ///////////////// ADD DATA INTO LIST //////////////////////


                        view_salereport.FBAFEE = FBAFEE; // Convert.ToDouble(item_expense_detail.expense_amount);
                        view_salereport.TechnologyFee = TechnologyFee; // Convert.ToDouble(item_expense_detail.expense_amount);
                        view_salereport.CommissionFee = CommissionFee; //Convert.ToDouble(item_expense_detail.expense_amount);
                        view_salereport.FixedClosingFee = FixedClosingFee; //Convert.ToDouble(item_expense_detail.expense_amount);
                        view_salereport.ShippingChargebackFee = ShippingChargebackFee; //Convert.ToDouble(item_expense_detail.expense_amount);
                        view_salereport.RefundCommision = RefundCommision;// Convert.ToDouble(item_expense_detail.expense_amount);

                        view_salereport.Refund_Commision = Refund_Commision; // Convert.ToDouble(refund.expense_amount);
                        view_salereport.RefundShippingChargebackFee = RefundShippingChargebackFee; //Convert.ToDouble(refund.expense_amount);
                        view_salereport.RefundFixedClosingFee = RefundFixedClosingFee; //Convert.ToDouble(refund.expense_amount);
                        view_salereport.RefundCommissionFee = RefundCommissionFee; //Convert.ToDouble(refund.expense_amount);
                        view_salereport.RefundTechnologyFee = RefundTechnologyFee; //Convert.ToDouble(refund.expense_amount);
                        view_salereport.RefundFBAFEE = RefundFBAFEE; //Convert.ToDouble(refund.expense_amount);

                        view_salereport.Marketplacecommission = Marketplacecommission;
                        view_salereport.LogisticsCharges = LogisticsCharges;
                        view_salereport.PGCommission = PGCommission;
                        view_salereport.Penaty = Penaty;
                        view_salereport.NetAdjustments = NetAdjustments;
                        view_salereport.RefundMarketplacecommission = RefundMarketplacecommission;
                        view_salereport.RefundLogisticsCharges = RefundLogisticsCharges;
                        view_salereport.RefundPGCommission = RefundPGCommission;
                        view_salereport.RefundPenaty = RefundPenaty;
                        view_salereport.RefundNetAdjustments = RefundNetAdjustments;

                        if (view_salereport.OrderID != null)
                        {
                            lstOrdertext2.Add(view_salereport);
                        }
                    }

                    objsales.CommissionFee = Convert.ToDouble(CommissionTotal);
                    objsales.FBAFEE = Convert.ToDouble(FBATotal);
                    objsales.TechnologyFee = Convert.ToDouble(TechnologyTotal);
                    objsales.FixedClosingFee = Convert.ToDouble(FixedClosingTotal);
                    objsales.ShippingChargebackFee = Convert.ToDouble(ShippingCharegeTotal);
                    objsales.RefundCommision = Convert.ToDouble(RefundCommTotal);
                    objsales.RefundCommissionFee = Convert.ToDouble(Refund_CommissionTotal);
                    objsales.RefundFBAFEE = Convert.ToDouble(Refund_FBATotal);
                    objsales.RefundTechnologyFee = Convert.ToDouble(Refund_TechnologyTotal);
                    objsales.RefundFixedClosingFee = Convert.ToDouble(Refund_FixedClosingTotal);
                    objsales.RefundShippingChargebackFee = Convert.ToDouble(Refund_ShippingCharegeTotal);
                    objsales.Refund_Commision = Convert.ToDouble(Refund_RefundCommTotal);

                    objsales.Marketplacecommission = Convert.ToDouble(MarketCommission);
                    objsales.LogisticsCharges = Convert.ToDouble(LogisticCharges);
                    objsales.PGCommission = Convert.ToDouble(PgCommission);
                    objsales.Penaty = Convert.ToDouble(Penanlty);
                    objsales.NetAdjustments = Convert.ToDouble(AdjustmentComm);
                    objsales.RefundMarketplacecommission = Convert.ToDouble(RefMarketCommission);
                    objsales.RefundLogisticsCharges = Convert.ToDouble(RefLogisticCharges);
                    objsales.RefundPGCommission = Convert.ToDouble(RefPgCommission);
                    objsales.RefundPenaty = Convert.ToDouble(RefPenanlty);
                    objsales.RefundNetAdjustments = Convert.ToDouble(RefAdjustmentComm);
                    objsales.SettlementDate = "Total";
                    lstOrdertext2.Add(objsales);
                }
            }
            catch (Exception ex)
            {
            }
        }


        #region For Paytm

        public void Expense_ReportPaytm(List<SaleReport> lstOrdertext2, SaleReport view_salereport, SaleReport objsales, int? ddl_market_place, DateTime? txt_from, DateTime? txt_to, int sellers_id)
        {
            try
            {
                if (txt_from != null && txt_to != null)
                {
                    decimal  MarketCommission = 0, Penanlty = 0, PgCommission = 0, AdjustmentComm = 0, LogisticCharges = 0;
                    decimal  RefMarketCommission = 0, RefPenanlty = 0, RefPgCommission = 0, RefAdjustmentComm = 0, RefLogisticCharges = 0;
                    var sales_order_list = (from ob_tbl_sales_order in dba.tbl_sales_order
                                            select new
                                            {
                                                ob_tbl_sales_order = ob_tbl_sales_order
                                            }).Where(a => a.ob_tbl_sales_order.tbl_sellers_id == sellers_id && a.ob_tbl_sales_order.is_active == 1 && a.ob_tbl_sales_order.tbl_Marketplace_Id == ddl_market_place && (a.ob_tbl_sales_order.purchase_date >= txt_from && a.ob_tbl_sales_order.purchase_date <= txt_to)).ToList();
                    //.Where(a => a.ob_tbl_sales_order.tbl_sellers_id == sellers_id).ToList();from ob_tbl_sales_order in dba.tbl_sales_order

                    foreach (var item_sale_order in sales_order_list)
                    {
                        view_salereport = new SaleReport();
                        int sale_order_id = Convert.ToInt32(item_sale_order.ob_tbl_sales_order.id);
                        string amazon_order_id = item_sale_order.ob_tbl_sales_order.amazon_order_id;
                        if (amazon_order_id == "408-6328489-4373908")
                        {
                        }
                        DateTime? purchase_date = item_sale_order.ob_tbl_sales_order.purchase_date;
                        if (view_salereport.OrderID != item_sale_order.ob_tbl_sales_order.amazon_order_id)
                        {
                            if (view_salereport.OrderID != null)
                                lstOrdertext2.Add(view_salereport);
                        }
                        int fillItemCount = lstOrdertext2.Count;
                        if (fillItemCount > 0)
                        {
                            if (lstOrdertext2[fillItemCount - 1].OrderID != item_sale_order.ob_tbl_sales_order.amazon_order_id && view_salereport.OrderID != item_sale_order.ob_tbl_sales_order.amazon_order_id)
                                view_salereport = new SaleReport();
                        }
                        /////////////////////// DECLARE VARIABLES FOR EXPENSE //////////////////////////////
                        double  Marketplacecommission = 0, Penaty = 0, LogisticsCharges = 0, PGCommission = 0, NetAdjustments = 0, RefundMarketplacecommission = 0, RefundPenaty = 0, RefundLogisticsCharges = 0, RefundPGCommission = 0, RefundNetAdjustments = 0;
                        ////////////////////// GET SALES ORDER DETAIL ITEM LIST ///////////////////////////
                        var sales_order_detail_list = (from ob_tbl_sales_order_details in dba.tbl_sales_order_details
                                                       select new
                                                       {
                                                           ob_tbl_sales_order_details = ob_tbl_sales_order_details
                                                       }).Where(d => d.ob_tbl_sales_order_details.tbl_sales_order_id == sale_order_id).ToList();

                        view_salereport.OrderID = amazon_order_id;
                        view_salereport.OrderDate = Convert.ToDateTime(purchase_date).ToString("yyyy-MM-dd");
                        foreach (var item_sale_order_detail in sales_order_detail_list)
                        {
                            int sale_order_detail_id = Convert.ToInt32(item_sale_order_detail.ob_tbl_sales_order_details.id);
                            string sku = item_sale_order_detail.ob_tbl_sales_order_details.sku_no;

                            //////////////////////// START COLLECT EXPENSE DATA //////////////////////////
                            var getsettlementdetails = dba.m_tbl_expense.Where(a => a.settlement_order_id.Contains(amazon_order_id) && a.tbl_seller_id == sellers_id && a.t_transactionType_id == 1).ToList();
                            foreach (var item_expense_detail in getsettlementdetails) //(var item1 in getsettlementdetails)
                            {
                                var exp_id = item_expense_detail.expense_type_id;
                                view_salereport.ReferenceID = item_expense_detail.reference_number;
                                view_salereport.SettlementDate = Convert.ToDateTime(item_expense_detail.settlement_datetime).ToString("yyyy-MM-dd");

                                var get_expdetails = dba.m_settlement_fee.Where(a => a.id == exp_id).FirstOrDefault();
                                if (get_expdetails != null)
                                {
                                    string nam = get_expdetails.return_fee;
                                                                   
                                    if (nam == "Marketplace Commission")
                                    {
                                        if (get_expdetails.id == exp_id)
                                        {
                                            Marketplacecommission += Convert.ToDouble(item_expense_detail.expense_amount);
                                        }
                                    }
                                    else if (nam == "Logistics Charges")
                                    {
                                        if (get_expdetails.id == exp_id)
                                        {
                                            LogisticsCharges += Convert.ToDouble(item_expense_detail.expense_amount);
                                        }
                                    }
                                    else if (nam == "PG Commission")
                                    {
                                        if (get_expdetails.id == exp_id)
                                        {
                                            PGCommission += Convert.ToDouble(item_expense_detail.expense_amount);
                                        }
                                    }
                                    else if (nam == "Penalty")
                                    {
                                        if (get_expdetails.id == exp_id)
                                        {
                                            Penaty += Convert.ToDouble(item_expense_detail.expense_amount);
                                        }
                                    }
                                    else if (nam == "Net Adjustments")
                                    {
                                        if (get_expdetails.id == exp_id)
                                        {
                                            NetAdjustments += Convert.ToDouble(item_expense_detail.expense_amount);
                                        }
                                    }
                                }
                            }
                            //////////////////////// ENDOF COLLECT EXPENSE DATA //////////////////////////
                            //////////////////////// START COLLECT REFUND DATA //////////////////////////

                            var get_historydata = dba.tbl_order_history.Where(a => a.tbl_seller_id == sellers_id && a.OrderID == amazon_order_id && a.t_order_status == 9).FirstOrDefault();
                            if (get_historydata != null)
                            {
                                var get_refundexpense = dba.m_tbl_expense.Where(a => a.settlement_order_id.Contains(amazon_order_id) && a.tbl_seller_id == sellers_id && a.t_transactionType_id == 2 && a.tbl_order_historyid == get_historydata.Id).ToList();// to get refund expense
                                if (get_refundexpense != null && get_refundexpense.Count > 0)
                                {
                                    foreach (var refund in get_refundexpense)
                                    {
                                        var exp_ID = refund.expense_type_id;
                                        var get_details = dba.m_settlement_fee.Where(a => a.id == exp_ID).FirstOrDefault();
                                        var getExp_tax_details = dba.tbl_tax.Where(a => a.tbl_referneced_id == refund.id && a.reference_type == 7 && a.tbl_history_id == get_historydata.Id).FirstOrDefault();
                                        if (get_details != null)
                                        {
                                            string nam = get_details.return_fee;
                                           
                                            if (nam == "Marketplace Commission")
                                            {
                                                if (get_details.id == exp_ID)
                                                {
                                                    RefundMarketplacecommission += Convert.ToDouble(refund.expense_amount);
                                                }
                                            }
                                            else if (nam == "Logistics Charges")
                                            {
                                                if (get_details.id == exp_ID)
                                                {
                                                    RefundLogisticsCharges += Convert.ToDouble(refund.expense_amount);
                                                }
                                            }
                                            else if (nam == "PG Commission")
                                            {
                                                if (get_details.id == exp_ID)
                                                {
                                                    RefundPGCommission += Convert.ToDouble(refund.expense_amount);
                                                }
                                            }
                                            else if (nam == "Penalty")
                                            {
                                                if (get_details.id == exp_ID)
                                                {
                                                    RefundPenaty += Convert.ToDouble(refund.expense_amount);
                                                }
                                            }
                                            else if (nam == "Net Adjustments")
                                            {
                                                if (get_details.id == exp_ID)
                                                {
                                                    RefundNetAdjustments += Convert.ToDouble(refund.expense_amount);
                                                }
                                            }
                                            }// end of if(get_details)                               
                                        }// end of foreach(refund)
                                    }// end of if(get_refundexpense)
                               
                                //////////////////////// ENDOF COLLECT REFUND DATA //////////////////////////

                            }                            

                            MarketCommission += Convert.ToDecimal(view_salereport.Marketplacecommission);
                            Penanlty += Convert.ToDecimal(view_salereport.Penaty);
                            PgCommission += Convert.ToDecimal(view_salereport.PGCommission);
                            AdjustmentComm += Convert.ToDecimal(view_salereport.NetAdjustments);
                            LogisticCharges += Convert.ToDecimal(view_salereport.LogisticsCharges);

                            RefMarketCommission += Convert.ToDecimal(view_salereport.RefundMarketplacecommission);
                            RefPenanlty += Convert.ToDecimal(view_salereport.RefundPenaty);
                            RefPgCommission += Convert.ToDecimal(view_salereport.RefundPGCommission);
                            RefAdjustmentComm += Convert.ToDecimal(view_salereport.RefundNetAdjustments);
                            RefLogisticCharges += Convert.ToDecimal(view_salereport.RefundLogisticsCharges);


                        }
                        ///////////////// ADD DATA INTO LIST //////////////////////                      

                        view_salereport.Marketplacecommission = Marketplacecommission;
                        view_salereport.LogisticsCharges = LogisticsCharges;
                        view_salereport.PGCommission = PGCommission;
                        view_salereport.Penaty = Penaty;
                        view_salereport.NetAdjustments = NetAdjustments;
                        view_salereport.RefundMarketplacecommission = RefundMarketplacecommission;
                        view_salereport.RefundLogisticsCharges = RefundLogisticsCharges;
                        view_salereport.RefundPGCommission = RefundPGCommission;
                        view_salereport.RefundPenaty = RefundPenaty;
                        view_salereport.RefundNetAdjustments = RefundNetAdjustments;

                        if (view_salereport.OrderID != null)
                        {
                            lstOrdertext2.Add(view_salereport);
                        }
                    }                   

                    objsales.Marketplacecommission = Convert.ToDouble(MarketCommission);
                    objsales.LogisticsCharges = Convert.ToDouble(LogisticCharges);
                    objsales.PGCommission = Convert.ToDouble(PgCommission);
                    objsales.Penaty = Convert.ToDouble(Penanlty);
                    objsales.NetAdjustments = Convert.ToDouble(AdjustmentComm);
                    objsales.RefundMarketplacecommission = Convert.ToDouble(RefMarketCommission);
                    objsales.RefundLogisticsCharges = Convert.ToDouble(RefLogisticCharges);
                    objsales.RefundPGCommission = Convert.ToDouble(RefPgCommission);
                    objsales.RefundPenaty = Convert.ToDouble(RefPenanlty);
                    objsales.RefundNetAdjustments = Convert.ToDouble(RefAdjustmentComm);
                    objsales.SettlementDate = "Total";
                    lstOrdertext2.Add(objsales);
                }
            }
            catch (Exception ex)
            {
            }
        }
        #endregion
    }
}