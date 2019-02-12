using SellerVendor.Areas.Seller.Models;
using SellerVendor.Areas.Seller.Models.DBContext;
using SellerVendor.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using System.Xml;
using System.Dynamic;
using System.Configuration;
using MySql.Data.MySqlClient;

namespace SellerVendor.Areas.Seller.Controllers
{
    public class SaleReportController : Controller
    {
        public SellerContext dba = new SellerContext();
        public SellerAdminContext db = new SellerAdminContext();
        comman_function cf = null;

        public ActionResult Index()
        {
            return View();
        }


        #region Sale Order report(Net Realization Report)
        /// <summary>
        /// this is for Sale Order Report
        /// </summary>
        /// <param name="form"></param>
        /// <param name="txt_from"></param>
        /// <param name="txt_to"></param>
        /// <param name="ddl_export"></param>
        /// <returns></returns>

        public ActionResult SaleOrderReport(FormCollection form, int? ddl_market_place, DateTime? txt_from, DateTime? txt_to, int? ddl_export, int? ddl_percentage)//use sp
        {
            cf = new comman_function();
            if (!cf.session_check())
                return RedirectToAction("Login", "Home");
            int sellers_id = Convert.ToInt32(Session["SellerID"]);
            ViewData["ExportList"] = cf.GetExcelExportList();
            ViewData["ExpenseList"] = cf.GetExpenseList();
            ViewData["PercentageList"] = cf.GetPercentageList();

            List<SelectListItem> lst1_loc = cf.GetMarketPlace(sellers_id);
            ViewData["MarKetPlace"] = lst1_loc;

            List<SaleReport> lstOrdertext2 = new List<SaleReport>();
            SaleReport view_salereport = null;
            try
            {
                if (txt_from != null && txt_to != null)
                {
                    SalesorderReport obj = new SalesorderReport();
                    if (ddl_market_place == 3)
                    {
                        obj.Get_SalesReport_Amazon(lstOrdertext2, view_salereport, txt_from, txt_to, sellers_id, ddl_market_place, ddl_percentage);
                    }
                    else if (ddl_market_place == 1)
                    {
                        obj.Get_SalesReport_Flipkart(lstOrdertext2, view_salereport, txt_from, txt_to, sellers_id, ddl_market_place, ddl_percentage);
                    }
                    else if (ddl_market_place == 5)
                    {
                        obj.Get_SalesReport_Paytm(lstOrdertext2, view_salereport, txt_from, txt_to, sellers_id, ddl_market_place, ddl_percentage);
                    }

                    //-------------start------------//
                    //--------------END-------------//

                    /////----------------------------------Export Excel----------------------------------

                    string sendOnMail = form["sendmail"];
                    string value1 = form["command"];
                    if (value1 == "Export" || sendOnMail == "Send On Mail")
                    {
                        if (ddl_export != null)
                        {
                            bool mail = false;
                            if (sendOnMail != null) mail = true;
                            string EmailID = form["txt_email"];

                            DataTable header = new DataTable();
                            string colum = " ";
                            for (int t = 0; t < 6; t++)
                            {
                                header.Columns.Add(colum);
                                colum = colum + " ";
                            }
                            DataRow errow = header.NewRow();
                            errow[2] = " ";
                            header.Rows.Add(errow);
                            DataRow drow = header.NewRow();
                            drow[1] = "From Date";
                            drow[2] = txt_from;
                            drow[4] = "Report Taken On";
                            drow[5] = DateTime.Now.ToString("dd/MMM/yyyy");
                            header.Rows.Add(drow);
                            DataRow brow = header.NewRow();
                            brow[1] = "To Date";
                            brow[2] = txt_to;
                            header.Rows.Add(brow);
                            DataRow erow = header.NewRow();
                            erow[2] = " ";
                            header.Rows.Add(erow);
                            DataTable export_dt = new DataTable();
                            if (ddl_market_place == 3)
                            {
                                export_dt = cf.CreateNetRealizationDatatable(lstOrdertext2);
                            }
                            else if (ddl_market_place == 1)
                            {
                                export_dt = cf.CreateNetRealizationDatatableFlipkart(lstOrdertext2);
                            }
                            else if (ddl_market_place == 5)
                            {
                                export_dt = cf.CreateNetRealizationDatatablePaytm(lstOrdertext2);
                            }
                            int type = Convert.ToInt32(ddl_export);
                            if (export_dt.Rows.Count > 0)
                            {
                                int r = cf.Export("Net_Realization_Report", header, export_dt, Response, type, mail, EmailID);
                            }
                            else ViewData["Message"] = "There are No records In Table";
                        }
                        else ViewData["Message"] = "Please Select Export Type";
                    }
                    //----------------------------------------END-----------------------------------------////
                }// end of if(txt_from)
            }// end of try block

            catch (Exception ex)
            {
            }
            if (ddl_market_place == 3)
            {
                return View("SaleOrderReportAmazon", lstOrdertext2);
            }
            else if (ddl_market_place == 1)
            {
                return View("SaleOrderReportFlipkart", lstOrdertext2);
            }
            else if (ddl_market_place == 5)
            {
                return View("SaleOrderReportPaytm", lstOrdertext2);
            }
            else
                return View(lstOrdertext2);
        }



        public ActionResult SaleOrderReport1(FormCollection form, int? ddl_market_place, DateTime? txt_from, DateTime? txt_to, int? ddl_export, int? ddl_percentage)//without sp
        {
            cf = new comman_function();
            if (!cf.session_check())
                return RedirectToAction("Login", "Home");
            int sellers_id = Convert.ToInt32(Session["SellerID"]);
            ViewData["ExportList"] = cf.GetExcelExportList();
            ViewData["ExpenseList"] = cf.GetExpenseList();
            ViewData["PercentageList"] = cf.GetPercentageList();

            List<SelectListItem> lst1_loc = cf.GetMarketPlace(sellers_id);
            ViewData["MarKetPlace"] = lst1_loc;

            List<SaleReport> lstOrdertext2 = new List<SaleReport>();
            SaleReport view_salereport = null;
            try
            {
                dba = new SellerContext();
                dba.Configuration.AutoDetectChangesEnabled = false;
                if (txt_from != null && txt_to != null)
                {
                    var GetSaleOrderDetail = (from ob_tbl_sales_order in dba.tbl_sales_order
                                              select new
                                              {
                                                  ob_tbl_sales_order = ob_tbl_sales_order

                                              }).Where(a => a.ob_tbl_sales_order.tbl_sellers_id == sellers_id && a.ob_tbl_sales_order.order_status != "Cancelled" && a.ob_tbl_sales_order.tbl_Marketplace_Id == ddl_market_place).ToList();
                    int i = 0;
                    view_salereport = new SaleReport();
                    if (GetSaleOrderDetail != null)
                    {
                        //if (txt_from != null && txt_to != null)
                        // {
                        GetSaleOrderDetail = GetSaleOrderDetail.Where(a => a.ob_tbl_sales_order.purchase_date.Date >= txt_from && a.ob_tbl_sales_order.purchase_date.Date <= txt_to).ToList();
                        // }
                    }
                    foreach (var item in GetSaleOrderDetail)
                    {

                        int iRepeatDetailData = 0;
                        if (item != null)
                        {
                            if (view_salereport.OrderID != item.ob_tbl_sales_order.amazon_order_id)
                            {
                                if (view_salereport.OrderID != null)
                                    if (PercentageFilter(ddl_percentage, view_salereport.PercentageAmount))
                                        lstOrdertext2.Add(view_salereport);
                            }
                            int fillItemCount = lstOrdertext2.Count;
                            if (fillItemCount > 0)
                            {
                                if (lstOrdertext2[fillItemCount - 1].OrderID != item.ob_tbl_sales_order.amazon_order_id && view_salereport.OrderID != item.ob_tbl_sales_order.amazon_order_id)
                                    view_salereport = new SaleReport();
                            }

                            //if (item.ob_tbl_sales_order.amazon_order_id == "171-6266491-1591505")
                            //{

                            //}
                            string amazon_order_id = item.ob_tbl_sales_order.amazon_order_id;
                            view_salereport.OrderID = item.ob_tbl_sales_order.amazon_order_id;
                            view_salereport.OrderDate = Convert.ToDateTime(item.ob_tbl_sales_order.purchase_date).ToString("yyyy-MM-dd");
                            // to get sale order details data from  sale order id 

                            double Principal = 0;
                            double Shipping = 0;
                            double orderigst = 0;
                            double ordersgst = 0;
                            double ordercgst = 0;
                            double shippingtax = 0;

                            double Giftwrap = 0, itempromotion = 0, shippingpromotion = 0;


                            var get_saleorder_details = dba.tbl_sales_order_details.Where(a => a.tbl_sales_order_id == item.ob_tbl_sales_order.id && a.tbl_seller_id == sellers_id).AsNoTracking().ToList();//to get sale_order details 
                            if (get_saleorder_details != null)
                            {
                                foreach (var order_details in get_saleorder_details)
                                {
                                    if (order_details.amazon_order_id == "404-9746292-0961142")
                                    {
                                    }
                                    view_salereport.ProductName = order_details.product_name;
                                    view_salereport.skuNo = order_details.sku_no;
                                    double ord_detail_Principal = Convert.ToDouble(order_details.item_price_amount);
                                    double ord_shipping = Convert.ToDouble(order_details.shipping_price_Amount);
                                    double ord_shippingtax = Convert.ToDouble(order_details.shipping_tax_Amount);
                                    double ord_promoshipping = Convert.ToDouble(order_details.promotion_amount);
                                    double ord_itempromotion = Convert.ToDouble(order_details.item_promotionAmount);
                                    double orde_shippingtax = Convert.ToDouble(order_details.shipping_tax_Amount);
                                    double ord_gift = Convert.ToDouble(order_details.giftwrapprice_amount);
                                    Principal = Principal + ord_detail_Principal;
                                    Shipping = Shipping + ord_shipping;
                                    Giftwrap = Giftwrap + ord_gift;
                                    shippingtax = shippingtax + ord_shippingtax;
                                    itempromotion = itempromotion + ord_itempromotion;
                                    shippingpromotion = shippingpromotion + ord_promoshipping;

                                    //shippingtax = shippingtax + ord_shippingtax;

                                    view_salereport.Shipping = Shipping;
                                    view_salereport.Principal = Principal + Shipping + Giftwrap + shippingtax - itempromotion - shippingpromotion;
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
                                }
                                iRepeatDetailData++;
                            }
                            ///////--------------End------/////////////

                            #region getSettlementdata
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

                            var get_settlementdata = dba.tbl_settlement_order.Where(a => a.Order_Id == item.ob_tbl_sales_order.amazon_order_id && a.tbl_seller_id == sellers_id).AsNoTracking().ToList();
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

                                    double FBAFEE = 0, FBACGST = 0, FBASGST = 0, TechnologyFee = 0, TechnologyIGST = 0, TechnologyCGST = 0, TechnologySGST = 0, CommissionFee = 0, CommissionIGST = 0, CommissionCGST = 0, CommissionSGST = 0, FixedClosingFee = 0, FixedclosingIGST = 0,
                                           FixedclosingCGST = 0, FixedclosingSGST = 0, ShippingChargebackFee = 0, EasyShipweighthandlingfees = 0,
                                           shippingchargeCGST = 0, shippingchargeSGST = 0, ShippingDiscountFee = 0, Shippingtaxdiscount = 0, RefundCommision = 0, RefundDiscount = 0, ShippingCommision = 0, ShippingCommissionIGST = 0, EasyShipweighthandlingfeesIGST = 0,
                                           FBAPickPackFee = 0, FBAPickPackFeeCGST = 0, FBAPickPackFeeSGST = 0, GiftWrapChargeback = 0, GiftWrapChargebackCGST = 0, GiftWrapChargebackSGST = 0, AmazonEasyShipCharges = 0, AmazonEasyShipChargesIGST = 0;

                                    var getsettlementdetails = dba.m_tbl_expense.Where(a => a.settlement_order_id.Contains(amazon_order_id) && a.tbl_seller_id == sellers_id && a.t_transactionType_id == 1).AsNoTracking().ToList();
                                    if (getsettlementdetails != null && getsettlementdetails.Count > 0)
                                    {
                                        foreach (var item1 in getsettlementdetails)
                                        {
                                            var exp_id = item1.expense_type_id;
                                            var get_expdetails = dba.m_settlement_fee.Where(a => a.id == exp_id).AsNoTracking().FirstOrDefault();
                                            var gettax_details = dba.tbl_tax.Where(a => a.tbl_referneced_id == item1.id && a.reference_type == 2).AsNoTracking().FirstOrDefault();
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
                                iRepeatSettlementData++;
                            }

                            //------------------------------------------End--------------------------------------//
                            #endregion

                            #region getHistoryData
                            //-------------------------------------To get Refund data from tbl_History and taxtable------------------------//
                            int iRepeatRefundData = 0;
                            double refundprincipal = 0, refundproduct_tax = 0, refundshipping = 0, refundshipping_tax = 0, refundgiftwrap = 0, refundgiftwrap_tax = 0, refundshipping_discount = 0, refundshipping_discounttax = 0;

                            var get_historydata = dba.tbl_order_history.Where(a => a.tbl_seller_id == sellers_id && a.OrderID == item.ob_tbl_sales_order.amazon_order_id && a.t_order_status == 9).AsNoTracking().ToList();
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


                                    var get_refundexpense = dba.m_tbl_expense.Where(a => a.settlement_order_id.Contains(amazon_order_id) && a.tbl_seller_id == sellers_id && a.t_transactionType_id == 2).AsNoTracking().ToList();// to get refund expense
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

                                }// end of foreach loop()
                                iRepeatRefundData++;
                            }// end of if(get_historydata)
                            #endregion

                            view_salereport.Refund_SUMIGST = view_salereport.RefundTechnologyIGST + view_salereport.RefundCommissionIGST + view_salereport.RefundFixedclosingIGST + view_salereport.Refund_EasyShipweighthandlingfeesIGST + view_salereport.RefundShippingtaxdiscount + view_salereport.Refund_Discount + view_salereport.Refund_Shipping_Commission + view_salereport.Refund_AmazonEasyShipChargesIGST;
                            view_salereport.Refund_SUMCGST = view_salereport.RefundshippingchargeCGST + view_salereport.RefundFBACGST + view_salereport.RefundFBAPick_PackFeeCGST + view_salereport.RefundGiftWrapChargebackCGST + view_salereport.Refund_DiscountCGST + view_salereport.RefundFixedclosingCGST + view_salereport.RefundCommissionCGST + view_salereport.RefundTechnologyCGST;
                            view_salereport.Refund_SUMSGST = view_salereport.RefundshippingchargeSGST + view_salereport.RefundFBASGST + view_salereport.RefundFBAPick_PackFeeSGST + view_salereport.RefundGiftWrapChargebackSGST + view_salereport.Refund_DiscountSGST + view_salereport.RefundFixedclosingSGST + view_salereport.RefundCommissionSGST + view_salereport.RefundTechnologySGST;

                            view_salereport.refund_SumFee = view_salereport.RefundFBAFEE + view_salereport.RefundTechnologyFee + view_salereport.RefundCommissionFee + view_salereport.RefundFixedClosingFee + view_salereport.RefundShippingChargebackFee + view_salereport.RefundShippingDiscountFee + view_salereport.Refund_EasyShipweighthandlingfees + view_salereport.Refund_Commision + view_salereport.Refund_ShippingCommision + view_salereport.Refund_FBAPick_PackFee + view_salereport.Refund_GiftWrapChargeback + view_salereport.Refund_AmazonEasyShipCharges + view_salereport.Refund_SUMIGST + view_salereport.Refund_SUMCGST + view_salereport.Refund_SUMSGST;
                            view_salereport.refund_SumOrder = view_salereport.refundTotal + view_salereport.refund_SumFee;
                            /////////-----------------------------------------End----------------------------------------------------------//

                            //sharad
                            view_salereport.SUMIGST = view_salereport.TechnologyIGST + view_salereport.CommissionIGST + view_salereport.FixedclosingIGST + view_salereport.Shippingtaxdiscount + view_salereport.RefundDiscount + view_salereport.ShippingCommissionIGST + view_salereport.EasyShipweighthandlingfeesIGST + view_salereport.AmazonEasyShipChargesIGST;

                            //view_salereport.SUMIGST = view_salereport.RefundCommisionIgst_Deducted + view_salereport.TechnologyIGST + view_salereport.CommissionIGST + view_salereport.Shippingtaxdiscount + view_salereport.RefundDiscount;
                            view_salereport.SUMCGST = view_salereport.shippingchargeCGST + view_salereport.FBACGST + view_salereport.FBAPickPackFeeCGST + view_salereport.GiftWrapChargebackCGST + view_salereport.FixedclosingCGST + view_salereport.CommissionCGST + view_salereport.TechnologyCGST;
                            view_salereport.SUMSGST = view_salereport.shippingchargeSGST + view_salereport.FBASGST + view_salereport.FBAPickPackFeeSGST + view_salereport.GiftWrapChargebackSGST + view_salereport.FixedclosingSGST + view_salereport.CommissionSGST + view_salereport.TechnologySGST;

                            view_salereport.SumFee = view_salereport.FBAFEE + view_salereport.TechnologyFee + view_salereport.CommissionFee + view_salereport.FixedClosingFee + view_salereport.ShippingChargebackFee + view_salereport.ShippingDiscountFee + view_salereport.RefundCommision + view_salereport.ShippingCommision + view_salereport.EasyShipweighthandlingfees + view_salereport.FBAPickPackFee + view_salereport.GiftWrapChargeback + view_salereport.AmazonEasyShipCharges + view_salereport.SUMIGST + view_salereport.SUMCGST + view_salereport.SUMSGST;
                            view_salereport.SumOrder = view_salereport.orderTotal + view_salereport.SumFee;

                            view_salereport.NetTotal = view_salereport.SumOrder + view_salereport.refund_SumOrder;
                            //view_salereport.PercentageAmount = view_salereport.NetTotal / view_salereport.ordertotal;

                            string value = "";
                            value = Convert.ToString((view_salereport.NetTotal / view_salereport.orderTotal) * 100);
                            //if (view_salereport.orderTotal == 0)
                            //{

                            //}
                            //else
                            //{
                            //    value = Convert.ToString((view_salereport.NetTotal / view_salereport.orderTotal) * 100);

                            //}
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

                        if (ddl_percentage != null)
                        {

                        }// chk if ddl not null
                    }// end of foreach(GetSaleOrderDetail)



                    if (view_salereport.OrderID != null)
                    {

                        if (PercentageFilter(ddl_percentage, view_salereport.PercentageAmount))
                            lstOrdertext2.Add(view_salereport);
                        // lstOrdertextCopy = lstOrdertext2.ToList();
                    }

                    /////----------------------------------Export Excel----------------------------------

                    string sendOnMail = form["sendmail"];
                    string value1 = form["command"];
                    if (value1 == "Export" || sendOnMail == "Send On Mail")
                    {
                        if (ddl_export != null)
                        {
                            bool mail = false;
                            if (sendOnMail != null) mail = true;
                            string EmailID = form["txt_email"];

                            DataTable header = new DataTable();
                            string colum = " ";
                            for (int t = 0; t < 6; t++)
                            {
                                header.Columns.Add(colum);
                                colum = colum + " ";
                            }
                            DataRow errow = header.NewRow();
                            errow[2] = " ";
                            header.Rows.Add(errow);
                            DataRow drow = header.NewRow();
                            drow[1] = "From Date";
                            drow[2] = txt_from;
                            drow[4] = "Report Taken On";
                            drow[5] = DateTime.Now.ToString("dd/MMM/yyyy");
                            header.Rows.Add(drow);
                            DataRow brow = header.NewRow();
                            brow[1] = "To Date";
                            brow[2] = txt_to;
                            header.Rows.Add(brow);
                            DataRow erow = header.NewRow();
                            erow[2] = " ";
                            header.Rows.Add(erow);
                            DataTable export_dt = new DataTable();
                            export_dt = cf.CreateNetRealizationDatatable(lstOrdertext2);
                            int type = Convert.ToInt32(ddl_export);
                            if (export_dt.Rows.Count > 0)
                            {
                                int r = cf.Export("Net_Realization_Report", header, export_dt, Response, type, mail, EmailID);
                            }
                            else ViewData["Message"] = "There are No records In Table";
                        }
                        else ViewData["Message"] = "Please Select Export Type";
                    }
                    //----------------------------------------END-----------------------------------------////
                    // end of if(GetSaleOrderDetail)
                }
            }// end of try block
            catch (Exception ex)
            {
            }

            return View(lstOrdertext2);
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
        #endregion


        #region Sale Ledger report1
        public ActionResult SaleLedgerReport1(FormCollection from, DateTime? txt_from, DateTime? txt_to)
        {
            int sellers_id = Convert.ToInt32(Session["SellerID"]);
            List<SaleReport> lstOrdertext2 = new List<SaleReport>();
            SaleReport view_salereport = null;
            try
            {
                var GetSaleOrderDetail = (from ob_tbl_sales_order in dba.tbl_sales_order
                                          select new
                                          {
                                              ob_tbl_sales_order = ob_tbl_sales_order

                                          }).Where(a => a.ob_tbl_sales_order.tbl_sellers_id == sellers_id).ToList();
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
                        if (item.ob_tbl_sales_order.order_status == "Canceled")
                        {
                            view_salereport.OrderID = item.ob_tbl_sales_order.amazon_order_id;
                            view_salereport.CancelledOrderDate = Convert.ToDateTime(item.ob_tbl_sales_order.purchase_date).ToString("yyyy-MM-dd");
                            view_salereport.CancelledorderAmount = Convert.ToDouble(item.ob_tbl_sales_order.bill_amount);
                        }
                        else
                        {
                            view_salereport.OrderID = item.ob_tbl_sales_order.amazon_order_id;
                            view_salereport.OrderDate = Convert.ToDateTime(item.ob_tbl_sales_order.purchase_date).ToString("yyyy-MM-dd");
                            view_salereport.OrderAmount = Convert.ToDouble(item.ob_tbl_sales_order.bill_amount);
                        }
                        var getsettlementdetails = dba.m_tbl_expense.Where(a => a.settlement_order_id == item.ob_tbl_sales_order.amazon_order_id && a.tbl_seller_id == sellers_id).ToList();
                        if (getsettlementdetails != null && getsettlementdetails.Count > 0)
                        {
                            foreach (var item1 in getsettlementdetails)
                            {
                                var exp_id = item1.expense_type_id;
                                var get_expdetails = dba.m_settlement_fee.Where(a => a.id == exp_id).FirstOrDefault();
                                if (get_expdetails != null)
                                {
                                    string nam = get_expdetails.return_fee;
                                    if (nam == "Principal")
                                    {
                                        if (get_expdetails.id == exp_id)
                                        {
                                            view_salereport.Principal = Convert.ToDouble(item1.expense_amount);
                                            var aaa = item1.settlement_datetime.HasValue ? item1.settlement_datetime.Value.ToString("dd-MMM-yyyy") : item1.settlement_datetime.ToString();
                                            //view_salereport.SettlementDate = Convert.ToString(item1.settlement_datetime);
                                            view_salereport.SettlementDate = aaa;
                                        }
                                        break;
                                    }
                                }// end of if(get_expdetails)

                            }// end if foreach(item1)
                        }// end of if(getsettlementdetails)                       
                        view_salereport.NetTotal = view_salereport.OrderAmount - view_salereport.Principal;
                        i++;
                    }// end of if(item !=  null)
                }// end of foreach(GetSaleOrderDetail)
                if (view_salereport.OrderID != null)
                {
                    lstOrdertext2.Add(view_salereport);
                }
                // end of if(GetSaleOrderDetail)
            }// end of try block
            catch (Exception ex)
            {
            }
            return View(lstOrdertext2);
        }
        #endregion



        #region Other Revenue Ledger report1
        public ActionResult OtherRevenueLedgerReport1(FormCollection from, DateTime? txt_from, DateTime? txt_to)
        {
            int sellers_id = Convert.ToInt32(Session["SellerID"]);
            List<SaleReport> lstOrdertext2 = new List<SaleReport>();
            SaleReport view_salereport = null;
            try
            {
                var GetSaleOrderDetail = (from ob_tbl_sales_order in dba.tbl_sales_order
                                          select new
                                          {
                                              ob_tbl_sales_order = ob_tbl_sales_order

                                          }).Where(a => a.ob_tbl_sales_order.tbl_sellers_id == sellers_id).ToList();
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
                        if (item.ob_tbl_sales_order.order_status == "Canceled")
                        {
                            view_salereport.OrderID = item.ob_tbl_sales_order.amazon_order_id;
                            view_salereport.CancelledOrderDate = Convert.ToDateTime(item.ob_tbl_sales_order.purchase_date).ToString("yyyy-MM-dd");
                            view_salereport.CancelledorderAmount = Convert.ToDouble(item.ob_tbl_sales_order.bill_amount);
                        }
                        else
                        {
                            view_salereport.OrderID = item.ob_tbl_sales_order.amazon_order_id;
                            //view_salereport.OrderDate = Convert.ToString(item.ob_tbl_sales_order.purchase_date);
                            view_salereport.OrderDate = Convert.ToDateTime(item.ob_tbl_sales_order.purchase_date).ToString("yyyy-MM-dd");
                            view_salereport.OrderAmount = Convert.ToDouble(item.ob_tbl_sales_order.bill_amount);
                        }

                        var getsettlementdetails = dba.m_tbl_expense.Where(a => a.settlement_order_id == item.ob_tbl_sales_order.amazon_order_id && a.tbl_seller_id == sellers_id).ToList();
                        if (getsettlementdetails != null && getsettlementdetails.Count > 0)
                        {
                            foreach (var item1 in getsettlementdetails)
                            {
                                var exp_id = item1.expense_type_id;
                                var get_expdetails = dba.m_settlement_fee.Where(a => a.id == exp_id).FirstOrDefault();
                                if (get_expdetails != null)
                                {
                                    string nam = get_expdetails.return_fee;
                                    if (nam == "Principal")
                                    {
                                        if (get_expdetails.id == exp_id)
                                        {
                                            view_salereport.Principal = Convert.ToDouble(item1.expense_amount);
                                            var aaa = item1.settlement_datetime.HasValue ? item1.settlement_datetime.Value.ToString("dd-MMM-yyyy") : item1.settlement_datetime.ToString();
                                            //view_salereport.SettlementDate = Convert.ToString(item1.settlement_datetime);
                                            view_salereport.SettlementDate = aaa;
                                        }
                                    }
                                    else if (nam == "Shipping")
                                    {
                                        if (get_expdetails.id == exp_id)
                                        {
                                            view_salereport.Shipping = Convert.ToDouble(item1.expense_amount);
                                        }
                                    }

                                }// end of if(get_expdetails)

                            }// end if foreach(item1)
                        }// end of if(getsettlementdetails)
                        //lstOrdertext2.Add(view_salereport);
                        //if (view_salereport.OrderAmount != null)
                        //{
                        view_salereport.SumFee = view_salereport.Principal + view_salereport.Shipping;
                        view_salereport.NetTotal = view_salereport.OrderAmount - view_salereport.SumFee;
                        //}
                        //if (view_salereport.CancelledorderAmount != null)
                        //{
                        //    view_salereport.NetTotal = view_salereport.CancelledorderAmount - view_salereport.Principal;
                        //}
                        i++;
                    }// end of if(item !=  null)
                }// end of foreach(GetSaleOrderDetail)
                if (view_salereport.OrderID != null)
                {
                    lstOrdertext2.Add(view_salereport);
                }
                // end of if(GetSaleOrderDetail)
            }// end of try block
            catch (Exception ex)
            {
            }
            return View(lstOrdertext2);
        }
        #endregion

        #region Net Realization With Tax report1
        public ActionResult NetRealizationWithTax1(FormCollection from, DateTime? txt_from, DateTime? txt_to)
        {
            int sellers_id = Convert.ToInt32(Session["SellerID"]);
            List<SaleReport> lstOrdertext2 = new List<SaleReport>();
            SaleReport view_salereport = null;
            try
            {
                var GetSaleOrderDetail = (from ob_tbl_sales_order in dba.tbl_sales_order
                                          select new
                                          {
                                              ob_tbl_sales_order = ob_tbl_sales_order
                                          }).Where(a => a.ob_tbl_sales_order.tbl_sellers_id == sellers_id).ToList();
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
                        if (item.ob_tbl_sales_order.order_status == "Canceled")
                        {
                            view_salereport.OrderID = item.ob_tbl_sales_order.amazon_order_id;
                            view_salereport.OrderDate = Convert.ToDateTime(item.ob_tbl_sales_order.purchase_date).ToString("yyyy-MM-dd");

                        }
                        else
                        {
                            view_salereport.OrderID = item.ob_tbl_sales_order.amazon_order_id;
                            view_salereport.OrderDate = Convert.ToDateTime(item.ob_tbl_sales_order.purchase_date).ToString("yyyy-MM-dd");
                        }

                        var getSaleorderitem = dba.tbl_sales_order_details.Where(a => a.tbl_sales_order_id == item.ob_tbl_sales_order.id).FirstOrDefault();
                        if (getSaleorderitem != null)
                        {
                            view_salereport.skuNo = getSaleorderitem.sku_no.ToLower();
                        }

                        var getinventorydetails = dba.tbl_inventory.Where(a => a.sku.ToLower() == view_salereport.skuNo && a.tbl_sellers_id == sellers_id).FirstOrDefault();
                        if (getinventorydetails != null)
                        {
                            view_salereport.ProductName = getinventorydetails.item_name;
                        }
                        var getsettlementdetails = dba.m_tbl_expense.Where(a => a.settlement_order_id == item.ob_tbl_sales_order.amazon_order_id && a.tbl_seller_id == sellers_id).ToList();
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
                                    if (nam == "Principal")
                                    {
                                        if (get_expdetails.id == exp_id)
                                        {
                                            view_salereport.Principal = Convert.ToDouble(item1.expense_amount);
                                            var aaa = item1.settlement_datetime.HasValue ? item1.settlement_datetime.Value.ToString("dd-MMM-yyyy") : item1.settlement_datetime.ToString();
                                            view_salereport.SettlementDate = aaa;
                                        }
                                    }
                                    else if (nam == "Product Tax")
                                    {
                                        if (get_expdetails.id == exp_id)
                                        {
                                            view_salereport.Product_Tax = Convert.ToDouble(item1.expense_amount);
                                        }
                                    }
                                    else if (nam == "Shipping")
                                    {
                                        if (get_expdetails.id == exp_id)
                                        {
                                            view_salereport.Shipping = Convert.ToDouble(item1.expense_amount);
                                        }
                                    }
                                    else if (nam == "Shipping tax")
                                    {
                                        if (get_expdetails.id == exp_id)
                                        {
                                            view_salereport.Shipping_Tax = Convert.ToDouble(item1.expense_amount);
                                        }
                                    }
                                    else if (nam == "FBA Weight Handling Fee")
                                    {
                                        if (get_expdetails.id == exp_id)
                                        {
                                            view_salereport.FBAFEE = Convert.ToDouble(item1.expense_amount);
                                            if (gettax_details != null)
                                            {
                                                view_salereport.FBACGST = Convert.ToDouble(gettax_details.CGST_amount);
                                                view_salereport.FBASGST = Convert.ToDouble(gettax_details.sgst_amount);
                                            }
                                        }
                                    }
                                    else if (nam == "Technology Fee")
                                    {
                                        if (get_expdetails.id == exp_id)
                                        {
                                            view_salereport.TechnologyFee = Convert.ToDouble(item1.expense_amount);
                                            if (gettax_details != null)
                                            {
                                                view_salereport.TechnologyIGST = Convert.ToDouble(gettax_details.Igst_amount);
                                            }
                                        }
                                    }
                                    else if (nam == "Commission")
                                    {
                                        if (get_expdetails.id == exp_id)
                                        {
                                            view_salereport.CommissionFee = Convert.ToDouble(item1.expense_amount);
                                            if (gettax_details != null)
                                            {
                                                view_salereport.CommissionIGST = Convert.ToDouble(gettax_details.Igst_amount);
                                            }
                                        }
                                    }
                                    else if (nam == "Fixed closing fee")
                                    {
                                        if (get_expdetails.id == exp_id)
                                        {
                                            view_salereport.FixedClosingFee = Convert.ToDouble(item1.expense_amount);
                                            if (gettax_details != null)
                                            {
                                                view_salereport.FixedclosingIGST = Convert.ToDouble(gettax_details.Igst_amount);
                                            }
                                        }
                                    }
                                    else if (nam == "Shipping Chargeback")
                                    {
                                        if (get_expdetails.id == exp_id)
                                        {
                                            view_salereport.ShippingChargebackFee = Convert.ToDouble(item1.expense_amount);
                                            if (gettax_details != null)
                                            {
                                                view_salereport.shippingchargeCGST = Convert.ToDouble(gettax_details.CGST_amount);
                                                view_salereport.shippingchargeSGST = Convert.ToDouble(gettax_details.sgst_amount);
                                            }
                                        }
                                    }
                                    else if (nam == "Shipping discount")
                                    {
                                        if (get_expdetails.id == exp_id)
                                        {
                                            view_salereport.ShippingDiscountFee = Convert.ToDouble(item1.expense_amount);
                                            if (gettax_details != null)
                                            {
                                                view_salereport.Shippingtaxdiscount = Convert.ToDouble(gettax_details.rateoftax_amount);
                                            }
                                        }
                                    }

                                }// end of if(get_expdetails)

                            }// end if foreach(item1)
                        }// end of if(getsettlementdetails)
                        //lstOrdertext2.Add(view_salereport);

                        view_salereport.SumOrder = view_salereport.Principal + view_salereport.Shipping + view_salereport.Product_Tax + view_salereport.Shipping_Tax;
                        view_salereport.SumFee = view_salereport.FBAFEE + view_salereport.TechnologyFee + view_salereport.CommissionFee + view_salereport.FixedClosingFee + view_salereport.ShippingChargebackFee + view_salereport.ShippingDiscountFee;
                        view_salereport.SumTaxFee = view_salereport.FBACGST + view_salereport.FBASGST + view_salereport.TechnologyIGST + view_salereport.CommissionIGST + view_salereport.FixedclosingIGST + view_salereport.shippingchargeCGST + view_salereport.shippingchargeSGST + view_salereport.Shippingtaxdiscount;
                        view_salereport.ExpenseTotal = view_salereport.SumFee + view_salereport.SumTaxFee;
                        view_salereport.NetTotal = view_salereport.SumOrder + view_salereport.ExpenseTotal;

                        i++;
                    }// end of if(item !=  null)
                }// end of foreach(GetSaleOrderDetail)
                if (view_salereport.OrderID != null)
                {
                    lstOrdertext2.Add(view_salereport);
                }
                // end of if(GetSaleOrderDetail)
            }// end of try block
            catch (Exception ex)
            {
            }
            return View(lstOrdertext2);
        }
        #endregion

        #region Net Realization WithOut Tax report1
        public ActionResult NetRealizationWithOutTax1(FormCollection from, DateTime? txt_from, DateTime? txt_to)
        {
            int sellers_id = Convert.ToInt32(Session["SellerID"]);
            List<SaleReport> lstOrdertext2 = new List<SaleReport>();
            SaleReport view_salereport = null;
            try
            {
                var GetSaleOrderDetail = (from ob_tbl_sales_order in dba.tbl_sales_order
                                          select new
                                          {
                                              ob_tbl_sales_order = ob_tbl_sales_order
                                          }).Where(a => a.ob_tbl_sales_order.tbl_sellers_id == sellers_id).ToList();
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
                        if (item.ob_tbl_sales_order.order_status == "Canceled")
                        {
                            view_salereport.OrderID = item.ob_tbl_sales_order.amazon_order_id;
                            view_salereport.OrderDate = Convert.ToDateTime(item.ob_tbl_sales_order.purchase_date).ToString("yyyy-MM-dd");

                        }
                        else
                        {
                            view_salereport.OrderID = item.ob_tbl_sales_order.amazon_order_id;
                            view_salereport.OrderDate = Convert.ToDateTime(item.ob_tbl_sales_order.purchase_date).ToString("yyyy-MM-dd");
                        }

                        var getSaleorderitem = dba.tbl_sales_order_details.Where(a => a.tbl_sales_order_id == item.ob_tbl_sales_order.id).FirstOrDefault();
                        if (getSaleorderitem != null)
                        {
                            view_salereport.skuNo = getSaleorderitem.sku_no.ToLower();
                        }

                        var getinventorydetails = dba.tbl_inventory.Where(a => a.sku.ToLower() == view_salereport.skuNo && a.tbl_sellers_id == sellers_id).FirstOrDefault();
                        if (getinventorydetails != null)
                        {
                            view_salereport.ProductName = getinventorydetails.item_name;
                        }
                        var getsettlementdetails = dba.m_tbl_expense.Where(a => a.settlement_order_id == item.ob_tbl_sales_order.amazon_order_id && a.tbl_seller_id == sellers_id).ToList();
                        if (getsettlementdetails != null && getsettlementdetails.Count > 0)
                        {
                            foreach (var item1 in getsettlementdetails)
                            {
                                var exp_id = item1.expense_type_id;
                                var get_expdetails = dba.m_settlement_fee.Where(a => a.id == exp_id).FirstOrDefault();
                                if (get_expdetails != null)
                                {
                                    string nam = get_expdetails.return_fee;
                                    if (nam == "Principal")
                                    {
                                        if (get_expdetails.id == exp_id)
                                        {
                                            view_salereport.Principal = Convert.ToDouble(item1.expense_amount);
                                            var aaa = item1.settlement_datetime.HasValue ? item1.settlement_datetime.Value.ToString("dd-MMM-yyyy") : item1.settlement_datetime.ToString();
                                            view_salereport.SettlementDate = aaa;
                                        }
                                    }
                                    else if (nam == "Shipping")
                                    {
                                        if (get_expdetails.id == exp_id)
                                        {
                                            view_salereport.Shipping = Convert.ToDouble(item1.expense_amount);
                                        }
                                    }
                                    else if (nam == "FBA Weight Handling Fee")
                                    {
                                        if (get_expdetails.id == exp_id)
                                        {
                                            view_salereport.FBAFEE = Convert.ToDouble(item1.expense_amount);
                                        }
                                    }
                                    else if (nam == "Technology Fee")
                                    {
                                        if (get_expdetails.id == exp_id)
                                        {
                                            view_salereport.TechnologyFee = Convert.ToDouble(item1.expense_amount);
                                        }
                                    }
                                    else if (nam == "Commission")
                                    {
                                        if (get_expdetails.id == exp_id)
                                        {
                                            view_salereport.CommissionFee = Convert.ToDouble(item1.expense_amount);
                                        }
                                    }
                                    else if (nam == "Fixed closing fee")
                                    {
                                        if (get_expdetails.id == exp_id)
                                        {
                                            view_salereport.FixedClosingFee = Convert.ToDouble(item1.expense_amount);
                                        }
                                    }
                                    else if (nam == "Shipping Chargeback")
                                    {
                                        if (get_expdetails.id == exp_id)
                                        {
                                            view_salereport.ShippingChargebackFee = Convert.ToDouble(item1.expense_amount);

                                        }
                                    }
                                    else if (nam == "Shipping discount")
                                    {
                                        if (get_expdetails.id == exp_id)
                                        {
                                            view_salereport.ShippingDiscountFee = Convert.ToDouble(item1.expense_amount);
                                        }
                                    }

                                }// end of if(get_expdetails)

                            }// end if foreach(item1)
                        }// end of if(getsettlementdetails)
                        //lstOrdertext2.Add(view_salereport);

                        view_salereport.SumOrder = view_salereport.Principal + view_salereport.Shipping;
                        view_salereport.SumFee = view_salereport.FBAFEE + view_salereport.TechnologyFee + view_salereport.CommissionFee + view_salereport.FixedClosingFee + view_salereport.ShippingChargebackFee + view_salereport.ShippingDiscountFee;
                        view_salereport.NetTotal = view_salereport.SumOrder + view_salereport.SumFee;

                        i++;
                    }// end of if(item !=  null)
                }// end of foreach(GetSaleOrderDetail)
                if (view_salereport.OrderID != null)
                {
                    lstOrdertext2.Add(view_salereport);
                }
                // end of if(GetSaleOrderDetail)
            }// end of try block
            catch (Exception ex)
            {
            }
            return View(lstOrdertext2);
        }
        #endregion

        #region Product Profitability Without Tax report1
        public ActionResult ProductProfitabilityWithoutTax1(FormCollection from, DateTime? txt_from, DateTime? txt_to)
        {
            int sellers_id = Convert.ToInt32(Session["SellerID"]);
            List<SaleReport> lstOrdertext2 = new List<SaleReport>();
            SaleReport view_salereport = null;
            try
            {
                var GetSaleOrderDetail = (from ob_tbl_sales_order in dba.tbl_sales_order
                                          select new
                                          {
                                              ob_tbl_sales_order = ob_tbl_sales_order
                                          }).Where(a => a.ob_tbl_sales_order.tbl_sellers_id == sellers_id).ToList();
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

                        view_salereport.OrderID = item.ob_tbl_sales_order.amazon_order_id;
                        view_salereport.OrderDate = Convert.ToDateTime(item.ob_tbl_sales_order.purchase_date).ToString("yyyy-MM-dd");

                        var getSaleorderitemdetails = dba.tbl_sales_order_details.Where(a => a.tbl_sales_order_id == item.ob_tbl_sales_order.id && a.n_order_status_id == 3).FirstOrDefault();
                        if (getSaleorderitemdetails != null)
                        {
                            view_salereport.skuNo = getSaleorderitemdetails.sku_no.ToLower();
                            var getinventorydetails1 = dba.tbl_inventory.Where(a => a.sku.ToLower() == view_salereport.skuNo && a.tbl_sellers_id == sellers_id).FirstOrDefault();
                            if (getinventorydetails1 != null)
                            {
                                var getpurchasedetails = dba.tbl_purchase_details.Where(a => a.tbl_inventory_id == getinventorydetails1.id && a.tbl_sellers_id == sellers_id).FirstOrDefault();
                                if (getpurchasedetails != null)
                                {
                                    view_salereport.ProductValue = Convert.ToDouble(getpurchasedetails.base_amount);
                                }
                            }
                        }


                        var getSaleorderitem = dba.tbl_sales_order_details.Where(a => a.tbl_sales_order_id == item.ob_tbl_sales_order.id).FirstOrDefault();
                        if (getSaleorderitem != null)
                        {
                            view_salereport.skuNo = getSaleorderitem.sku_no.ToLower();
                        }

                        var getinventorydetails = dba.tbl_inventory.Where(a => a.sku.ToLower() == view_salereport.skuNo && a.tbl_sellers_id == sellers_id).FirstOrDefault();
                        if (getinventorydetails != null)
                        {
                            view_salereport.ProductName = getinventorydetails.item_name;
                        }
                        var getsettlementdetails = dba.m_tbl_expense.Where(a => a.settlement_order_id == item.ob_tbl_sales_order.amazon_order_id && a.tbl_seller_id == sellers_id).ToList();
                        if (getsettlementdetails != null && getsettlementdetails.Count > 0)
                        {
                            foreach (var item1 in getsettlementdetails)
                            {
                                var exp_id = item1.expense_type_id;
                                var get_expdetails = dba.m_settlement_fee.Where(a => a.id == exp_id).FirstOrDefault();
                                if (get_expdetails != null)
                                {
                                    string nam = get_expdetails.return_fee;
                                    if (nam == "Principal")
                                    {
                                        if (get_expdetails.id == exp_id)
                                        {
                                            view_salereport.Principal = Convert.ToDouble(item1.expense_amount);
                                            var aaa = item1.settlement_datetime.HasValue ? item1.settlement_datetime.Value.ToString("dd-MMM-yyyy") : item1.settlement_datetime.ToString();
                                            view_salereport.SettlementDate = aaa;
                                        }
                                    }
                                    else if (nam == "Shipping")
                                    {
                                        if (get_expdetails.id == exp_id)
                                        {
                                            view_salereport.Shipping = Convert.ToDouble(item1.expense_amount);
                                        }
                                    }
                                    else if (nam == "FBA Weight Handling Fee")
                                    {
                                        if (get_expdetails.id == exp_id)
                                        {
                                            view_salereport.FBAFEE = Convert.ToDouble(item1.expense_amount);
                                        }
                                    }
                                    else if (nam == "Technology Fee")
                                    {
                                        if (get_expdetails.id == exp_id)
                                        {
                                            view_salereport.TechnologyFee = Convert.ToDouble(item1.expense_amount);
                                        }
                                    }
                                    else if (nam == "Commission")
                                    {
                                        if (get_expdetails.id == exp_id)
                                        {
                                            view_salereport.CommissionFee = Convert.ToDouble(item1.expense_amount);
                                        }
                                    }
                                    else if (nam == "Fixed closing fee")
                                    {
                                        if (get_expdetails.id == exp_id)
                                        {
                                            view_salereport.FixedClosingFee = Convert.ToDouble(item1.expense_amount);
                                        }
                                    }
                                    else if (nam == "Shipping Chargeback")
                                    {
                                        if (get_expdetails.id == exp_id)
                                        {
                                            view_salereport.ShippingChargebackFee = Convert.ToDouble(item1.expense_amount);

                                        }
                                    }
                                    else if (nam == "Shipping discount")
                                    {
                                        if (get_expdetails.id == exp_id)
                                        {
                                            view_salereport.ShippingDiscountFee = Convert.ToDouble(item1.expense_amount);
                                        }
                                    }

                                }// end of if(get_expdetails)

                            }// end if foreach(item1)
                        }// end of if(getsettlementdetails)
                        //lstOrdertext2.Add(view_salereport);

                        view_salereport.SumOrder = view_salereport.Principal + view_salereport.Shipping;
                        view_salereport.SumFee = view_salereport.FBAFEE + view_salereport.TechnologyFee + view_salereport.CommissionFee + view_salereport.FixedClosingFee + view_salereport.ShippingChargebackFee + view_salereport.ShippingDiscountFee;
                        view_salereport.NetTotal = view_salereport.SumOrder + view_salereport.SumFee;

                        i++;
                    }// end of if(item !=  null)
                }// end of foreach(GetSaleOrderDetail)
                if (view_salereport.OrderID != null)
                {
                    lstOrdertext2.Add(view_salereport);
                }
                // end of if(GetSaleOrderDetail)
            }// end of try block
            catch (Exception ex)
            {
            }
            return View(lstOrdertext2);
        }
        #endregion


        #region Product Profitability With Tax report1
        public ActionResult ProductProfitabilityWithTax1(FormCollection from, DateTime? txt_from, DateTime? txt_to)
        {
            int sellers_id = Convert.ToInt32(Session["SellerID"]);
            List<SaleReport> lstOrdertext2 = new List<SaleReport>();
            SaleReport view_salereport = null;
            try
            {
                var GetSaleOrderDetail = (from ob_tbl_sales_order in dba.tbl_sales_order
                                          select new
                                          {
                                              ob_tbl_sales_order = ob_tbl_sales_order
                                          }).Where(a => a.ob_tbl_sales_order.tbl_sellers_id == sellers_id).ToList();
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

                        view_salereport.OrderID = item.ob_tbl_sales_order.amazon_order_id;
                        view_salereport.OrderDate = Convert.ToDateTime(item.ob_tbl_sales_order.purchase_date).ToString("yyyy-MM-dd");

                        var getSaleorderitemdetails = dba.tbl_sales_order_details.Where(a => a.tbl_sales_order_id == item.ob_tbl_sales_order.id && a.n_order_status_id == 3).FirstOrDefault();
                        if (getSaleorderitemdetails != null)
                        {
                            view_salereport.skuNo = getSaleorderitemdetails.sku_no.ToLower();
                            var getinventorydetails1 = dba.tbl_inventory.Where(a => a.sku.ToLower() == view_salereport.skuNo && a.tbl_sellers_id == sellers_id).FirstOrDefault();
                            if (getinventorydetails1 != null)
                            {
                                var getpurchasedetails = dba.tbl_purchase_details.Where(a => a.tbl_inventory_id == getinventorydetails1.id && a.tbl_sellers_id == sellers_id).FirstOrDefault();
                                if (getpurchasedetails != null)
                                {
                                    view_salereport.ProductValue = Convert.ToDouble(getpurchasedetails.base_amount);
                                    var gettax_details = dba.tbl_tax.Where(a => a.tbl_referneced_id == getpurchasedetails.tbl_purchase_id && a.reference_type == 1).FirstOrDefault();
                                    if (gettax_details != null)
                                    {
                                        view_salereport.productIgstamt = Convert.ToDouble(gettax_details.Igst_amount);
                                        view_salereport.productCgstamt = Convert.ToDouble(gettax_details.CGST_amount);
                                        view_salereport.productSgstamt = Convert.ToDouble(gettax_details.sgst_amount);
                                    }
                                }
                            }
                        }


                        var getSaleorderitem = dba.tbl_sales_order_details.Where(a => a.tbl_sales_order_id == item.ob_tbl_sales_order.id).FirstOrDefault();
                        if (getSaleorderitem != null)
                        {
                            view_salereport.skuNo = getSaleorderitem.sku_no.ToLower();
                        }

                        var getinventorydetails = dba.tbl_inventory.Where(a => a.sku.ToLower() == view_salereport.skuNo && a.tbl_sellers_id == sellers_id).FirstOrDefault();
                        if (getinventorydetails != null)
                        {
                            view_salereport.ProductName = getinventorydetails.item_name;
                        }
                        var getsettlementdetails = dba.m_tbl_expense.Where(a => a.settlement_order_id == item.ob_tbl_sales_order.amazon_order_id && a.tbl_seller_id == sellers_id).ToList();
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
                                    if (nam == "Principal")
                                    {
                                        if (get_expdetails.id == exp_id)
                                        {
                                            view_salereport.Principal = Convert.ToDouble(item1.expense_amount);
                                            var aaa = item1.settlement_datetime.HasValue ? item1.settlement_datetime.Value.ToString("dd-MMM-yyyy") : item1.settlement_datetime.ToString();
                                            view_salereport.SettlementDate = aaa;
                                        }
                                    }
                                    else if (nam == "Product Tax")
                                    {
                                        if (get_expdetails.id == exp_id)
                                        {
                                            view_salereport.Product_Tax = Convert.ToDouble(item1.expense_amount);
                                        }
                                    }
                                    else if (nam == "Shipping")
                                    {
                                        if (get_expdetails.id == exp_id)
                                        {
                                            view_salereport.Shipping = Convert.ToDouble(item1.expense_amount);
                                        }
                                    }
                                    else if (nam == "Shipping tax")
                                    {
                                        if (get_expdetails.id == exp_id)
                                        {
                                            view_salereport.Shipping_Tax = Convert.ToDouble(item1.expense_amount);
                                        }
                                    }
                                    else if (nam == "FBA Weight Handling Fee")
                                    {
                                        if (get_expdetails.id == exp_id)
                                        {
                                            view_salereport.FBAFEE = Convert.ToDouble(item1.expense_amount);
                                            if (gettax_details != null)
                                            {
                                                view_salereport.FBACGST = Convert.ToDouble(gettax_details.CGST_amount);
                                                view_salereport.FBASGST = Convert.ToDouble(gettax_details.sgst_amount);
                                            }
                                        }
                                    }
                                    else if (nam == "Technology Fee")
                                    {
                                        if (get_expdetails.id == exp_id)
                                        {
                                            view_salereport.TechnologyFee = Convert.ToDouble(item1.expense_amount);
                                            if (gettax_details != null)
                                            {
                                                view_salereport.TechnologyIGST = Convert.ToDouble(gettax_details.Igst_amount);
                                            }
                                        }
                                    }
                                    else if (nam == "Commission")
                                    {
                                        if (get_expdetails.id == exp_id)
                                        {
                                            view_salereport.CommissionFee = Convert.ToDouble(item1.expense_amount);
                                            if (gettax_details != null)
                                            {
                                                view_salereport.CommissionIGST = Convert.ToDouble(gettax_details.Igst_amount);
                                            }
                                        }
                                    }
                                    else if (nam == "Fixed closing fee")
                                    {
                                        if (get_expdetails.id == exp_id)
                                        {
                                            view_salereport.FixedClosingFee = Convert.ToDouble(item1.expense_amount);
                                            if (gettax_details != null)
                                            {
                                                view_salereport.FixedclosingIGST = Convert.ToDouble(gettax_details.Igst_amount);
                                            }
                                        }
                                    }
                                    else if (nam == "Shipping Chargeback")
                                    {
                                        if (get_expdetails.id == exp_id)
                                        {
                                            view_salereport.ShippingChargebackFee = Convert.ToDouble(item1.expense_amount);
                                            if (gettax_details != null)
                                            {
                                                view_salereport.shippingchargeCGST = Convert.ToDouble(gettax_details.CGST_amount);
                                                view_salereport.shippingchargeSGST = Convert.ToDouble(gettax_details.sgst_amount);
                                            }
                                        }
                                    }
                                    else if (nam == "Shipping discount")
                                    {
                                        if (get_expdetails.id == exp_id)
                                        {
                                            view_salereport.ShippingDiscountFee = Convert.ToDouble(item1.expense_amount);
                                            if (gettax_details != null)
                                            {
                                                view_salereport.Shippingtaxdiscount = Convert.ToDouble(gettax_details.rateoftax_amount);
                                            }
                                        }
                                    }

                                }// end of if(get_expdetails)

                            }// end if foreach(item1)
                        }// end of if(getsettlementdetails)
                        //lstOrdertext2.Add(view_salereport);
                        var producttax = view_salereport.productIgstamt + view_salereport.productCgstamt + view_salereport.productSgstamt;
                        view_salereport.ProductValue = view_salereport.ProductValue + producttax;

                        view_salereport.SumOrder = view_salereport.Principal + view_salereport.Shipping + view_salereport.Product_Tax + view_salereport.Shipping_Tax;
                        view_salereport.SumFee = view_salereport.FBAFEE + view_salereport.TechnologyFee + view_salereport.CommissionFee + view_salereport.FixedClosingFee + view_salereport.ShippingChargebackFee + view_salereport.ShippingDiscountFee;
                        view_salereport.SumTaxFee = view_salereport.FBACGST + view_salereport.FBASGST + view_salereport.TechnologyIGST + view_salereport.CommissionIGST + view_salereport.FixedclosingIGST + view_salereport.shippingchargeCGST + view_salereport.shippingchargeSGST + view_salereport.Shippingtaxdiscount;
                        view_salereport.ExpenseTotal = view_salereport.SumFee + view_salereport.SumTaxFee;
                        view_salereport.NetTotal = view_salereport.SumOrder + view_salereport.ExpenseTotal;

                        i++;
                    }// end of if(item !=  null)
                }// end of foreach(GetSaleOrderDetail)
                if (view_salereport.OrderID != null)
                {
                    lstOrdertext2.Add(view_salereport);
                }
                // end of if(GetSaleOrderDetail)
            }// end of try block
            catch (Exception ex)
            {
            }
            return View(lstOrdertext2);
        }
        #endregion



        #region Expense Ledger report
        public ActionResult ExpenseOrderReport11(FormCollection form, int? ddl_expense, DateTime? txt_from, DateTime? txt_to, int? ddl_export)
        {
            int sellers_id = Convert.ToInt32(Session["SellerID"]);
            cf = new comman_function();
            ViewData["ExportList"] = cf.GetExportList();
            ViewData["ExpenseList"] = cf.GetExpenseList();
            List<SaleReport> lstOrdertext2 = new List<SaleReport>();
            SaleReport objsales = new SaleReport();
            SaleReport view_salereport = null;
            decimal CommissionTotal = 0, FBATotal = 0, TechnologyTotal = 0, FixedClosingTotal = 0, ShippingCharegeTotal = 0, RefundCommTotal = 0;
            decimal Refund_CommissionTotal = 0, Refund_FBATotal = 0, Refund_TechnologyTotal = 0, Refund_FixedClosingTotal = 0, Refund_ShippingCharegeTotal = 0, Refund_RefundCommTotal = 0;
            try
            {
                if (txt_from != null && txt_to != null)
                {
                    var GetSaleOrderDetail = (from ob_tbl_sales_order in dba.tbl_sales_order
                                              select new
                                              {
                                                  ob_tbl_sales_order = ob_tbl_sales_order

                                              }).Where(a => a.ob_tbl_sales_order.tbl_sellers_id == sellers_id).ToList();
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
                            //view_salereport.OrderID = item.ob_tbl_sales_order.amazon_order_id;  WHERE Name like @person + '%'
                            //view_salereport.OrderDate = Convert.ToDateTime(item.ob_tbl_sales_order.purchase_date).ToString("yyyy-MM-dd");

                            var getsettlementdetails = dba.m_tbl_expense.Where(a => a.settlement_order_id == item.ob_tbl_sales_order.amazon_order_id && a.tbl_seller_id == sellers_id && a.t_transactionType_id == 1).ToList();
                            if (getsettlementdetails != null && getsettlementdetails.Count > 0)
                            {
                                foreach (var item1 in getsettlementdetails)
                                {
                                    view_salereport.OrderID = item.ob_tbl_sales_order.amazon_order_id;
                                    view_salereport.OrderDate = Convert.ToDateTime(item.ob_tbl_sales_order.purchase_date).ToString("yyyy-MM-dd");
                                    var exp_id = item1.expense_type_id;
                                    view_salereport.ReferenceID = item1.reference_number;
                                    view_salereport.SettlementDate = Convert.ToDateTime(item1.settlement_datetime).ToString("yyyy-MM-dd");
                                    var get_expdetails = dba.m_settlement_fee.Where(a => a.id == exp_id).FirstOrDefault();
                                    if (get_expdetails != null)
                                    {
                                        string nam = get_expdetails.return_fee;
                                        if (nam == "FBA Weight Handling Fee")
                                        {
                                            if (get_expdetails.id == exp_id)
                                            {
                                                view_salereport.FBAFEE = Convert.ToDouble(item1.expense_amount);
                                            }
                                        }
                                        else if (nam == "Technology Fee")
                                        {
                                            if (get_expdetails.id == exp_id)
                                            {
                                                view_salereport.TechnologyFee = Convert.ToDouble(item1.expense_amount);
                                            }
                                        }
                                        else if (nam == "Commission")
                                        {
                                            if (get_expdetails.id == exp_id)
                                            {
                                                view_salereport.CommissionFee = Convert.ToDouble(item1.expense_amount);
                                            }
                                        }
                                        else if (nam == "Fixed closing fee")
                                        {
                                            if (get_expdetails.id == exp_id)
                                            {
                                                view_salereport.FixedClosingFee = Convert.ToDouble(item1.expense_amount);
                                            }
                                        }
                                        else if (nam == "Shipping Chargeback")
                                        {
                                            if (get_expdetails.id == exp_id)
                                            {
                                                view_salereport.ShippingChargebackFee = Convert.ToDouble(item1.expense_amount);
                                            }
                                        }

                                        else if (nam == "Refund commission")
                                        {
                                            if (get_expdetails.id == exp_id)
                                            {
                                                view_salereport.RefundCommision = Convert.ToDouble(item1.expense_amount);
                                            }
                                        }
                                    }// end of if(get_expdetails)
                                }// end if foreach(item1)
                            }// end of if(getsettlementdetails) 
                            //-------------------------------------To get Refund data from tbl_History and taxtable------------------------//

                            var get_historydata = dba.tbl_order_history.Where(a => a.tbl_seller_id == sellers_id && a.OrderID == item.ob_tbl_sales_order.amazon_order_id && a.t_order_status == 9).FirstOrDefault();
                            if (get_historydata != null)
                            {
                                var get_refundexpense = dba.m_tbl_expense.Where(a => a.settlement_order_id == item.ob_tbl_sales_order.amazon_order_id && a.tbl_seller_id == sellers_id && a.t_transactionType_id == 2 && a.tbl_order_historyid == get_historydata.Id).ToList();// to get refund expense
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

                                            else if (nam == "Refund commission")
                                            {
                                                if (get_details.id == exp_ID)
                                                {
                                                    view_salereport.Refund_Commision = Convert.ToDouble(refund.expense_amount);
                                                }
                                            }
                                        }// end of if(get_details)                               
                                    }// end of foreach(refund)
                                }// end of if(get_refundexpense)
                            }// end of if(get_historydata)                       
                            i++;
                        }// end of if(item !=  null)

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



                    }// end of foreach(GetSaleOrderDetail)
                    if (view_salereport.OrderID != null)
                    {
                        lstOrdertext2.Add(view_salereport);
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
                    objsales.SettlementDate = "Total";
                    lstOrdertext2.Add(objsales);
                    // end of if(GetSaleOrderDetail)
                }
                /////----------------------------------Export Excel----------------------------------
                string sendOnMail = form["sendmail"];
                string value1 = form["command"];
                if (value1 == "Export" || sendOnMail == "Send On Mail")
                {
                    if (ddl_export != null)
                    {
                        bool mail = false;
                        if (sendOnMail != null) mail = true;
                        string EmailID = form["txt_email"];

                        DataTable header = new DataTable();
                        string colum = " ";
                        for (int t = 0; t < 6; t++)
                        {
                            header.Columns.Add(colum);
                            colum = colum + " ";
                        }
                        DataRow errow = header.NewRow();
                        errow[2] = " ";
                        header.Rows.Add(errow);
                        DataRow drow = header.NewRow();
                        drow[1] = "From Date";
                        drow[2] = txt_from;
                        drow[4] = "Report Taken On";
                        drow[5] = DateTime.Now.ToString("dd/MMM/yyyy");
                        header.Rows.Add(drow);
                        DataRow brow = header.NewRow();
                        brow[1] = "To Date";
                        brow[2] = txt_to;

                        header.Rows.Add(brow);
                        DataRow erow = header.NewRow();
                        erow[2] = " ";
                        header.Rows.Add(erow);
                        DataTable export_dt = new DataTable();
                        export_dt = cf.CreateExpenseDatatable(lstOrdertext2);
                        int type = Convert.ToInt32(ddl_export);
                        if (export_dt.Rows.Count > 0)
                        {
                            int r = cf.Export("Expense Report", header, export_dt, Response, type, mail, EmailID);
                        }
                        else ViewData["Message"] = "There are No records In Table";
                    }
                    else ViewData["Message"] = "Please Select Export Type";
                }
                //----------------------------------------END-----------------------------------------////
            }// end of try block
            catch (Exception ex)
            {
            }
            return View(lstOrdertext2);
        }

        /// <summary>
        /// this is for Expense Ledger report
        /// </summary>
        /// <param name="form"></param>
        /// <param name="ddl_expense"></param>
        /// <param name="txt_from"></param>
        /// <param name="txt_to"></param>
        /// <param name="ddl_export"></param>
        /// <returns></returns>
        public ActionResult ExpenseOrderReport(FormCollection form, int? ddl_market_place, int? ddl_expense, DateTime? txt_from, DateTime? txt_to, int? ddl_export)
        {
            cf = new comman_function();
            if (!cf.session_check())
                return RedirectToAction("Login", "Home");

            int sellers_id = Convert.ToInt32(Session["SellerID"]);
            ViewData["ExportList"] = cf.GetExcelExportList();
            ViewData["ExpenseList"] = cf.GetExpenseList();

            List<SelectListItem> lst1_loc = cf.GetMarketPlace(sellers_id);
            ViewData["MarKetPlace"] = lst1_loc;
            List<SaleReport> lstOrdertext2 = new List<SaleReport>();
            SaleReport objsales = new SaleReport();
            SaleReport view_salereport = null;

            try
            {
                if (txt_from != null && txt_to != null)
                {
                    Report_Utility ru = new Report_Utility();
                    if (ddl_market_place == 3 || ddl_market_place == 1)
                    {
                        ru.Expense_Report(lstOrdertext2, view_salereport, objsales, ddl_market_place, txt_from, txt_to, sellers_id);
                    }
                    else
                    {
                        ru.Expense_ReportPaytm(lstOrdertext2, view_salereport, objsales, ddl_market_place, txt_from, txt_to, sellers_id);
                    }

                }
                /////----------------------------------Export Excel----------------------------------
                string sendOnMail = form["sendmail"];
                string value1 = form["command"];
                if (value1 == "Export" || sendOnMail == "Send On Mail")
                {
                    if (ddl_export != null)
                    {
                        bool mail = false;
                        if (sendOnMail != null) mail = true;
                        string EmailID = form["txt_email"];

                        DataTable header = new DataTable();
                        string colum = " ";
                        for (int t = 0; t < 6; t++)
                        {
                            header.Columns.Add(colum);
                            colum = colum + " ";
                        }
                        DataRow errow = header.NewRow();
                        errow[2] = " ";
                        header.Rows.Add(errow);
                        DataRow drow = header.NewRow();
                        drow[1] = "From Date";
                        drow[2] = txt_from;
                        drow[4] = "Report Taken On";
                        drow[5] = DateTime.Now.ToString("dd/MMM/yyyy");
                        header.Rows.Add(drow);
                        DataRow brow = header.NewRow();
                        brow[1] = "To Date";
                        brow[2] = txt_to;

                        header.Rows.Add(brow);
                        DataRow erow = header.NewRow();
                        erow[2] = " ";
                        header.Rows.Add(erow);
                        DataTable export_dt = new DataTable();
                        if (ddl_market_place == 3 || ddl_market_place == 1)
                        {
                            export_dt = cf.CreateExpenseDatatable(lstOrdertext2);
                        }
                        else
                        {
                            export_dt = cf.CreateExpenseDatatablePaytm(lstOrdertext2);
                        }
                        int type = Convert.ToInt32(ddl_export);
                        if (export_dt.Rows.Count > 0)
                        {
                            int r = cf.Export("Expense Report", header, export_dt, Response, type, mail, EmailID);
                        }
                        else ViewData["Message"] = "There are No records In Table";
                    }
                    else ViewData["Message"] = "Please Select Export Type";
                }
                //----------------------------------------END-----------------------------------------////
            }// end of try block
            catch (Exception ex)
            {
            }
            if (ddl_market_place == 3)
            {
                return View("ExpenseReportAmazon", lstOrdertext2);
            }
            else if (ddl_market_place == 1)
            {
                return View("ExpenseReportAmazon", lstOrdertext2);
            }
            else if (ddl_market_place == 5)
            {
                return View("ExpenseReportPaytm", lstOrdertext2);
            }
            else
                return View(lstOrdertext2);
        }
        #endregion


        #region Sale Ledger report
        /// <summary>
        /// this is for Sale Ledger report
        /// </summary>
        /// <param name="form"></param>
        /// <param name="ddl_marketplace"></param>
        /// <param name="txt_from"></param>
        /// <param name="txt_to"></param>
        /// <param name="ddl_export"></param>
        /// <returns></returns>
        public ActionResult SaleLedgerReport(FormCollection form, int? ddl_marketplace, DateTime? txt_from, DateTime? txt_to, int? ddl_export)
        {
            cf = new comman_function();
            if (!cf.session_check())
                return RedirectToAction("Login", "Home");

            int sellers_id = Convert.ToInt32(Session["SellerID"]);
            ViewData["ExportList"] = cf.GetExcelExportList();
            List<SelectListItem> lst1_loc = cf.GetMarketPlace(sellers_id);
            ViewData["MarKetPlace"] = lst1_loc;
            //List<SelectListItem> lst_loc = cf.GetSrchMarketPalceList(sellers_id);
            //ViewData["MarKetPlaceList"] = lst_loc;
            List<SaleReport> lstOrdertext2 = new List<SaleReport>();
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
                    if (ddl_marketplace != null && ddl_marketplace != 0)
                        get_marketplace = get_marketplace.Where(a => a.id == ddl_marketplace).ToList();
                    foreach (var item in GetSaleOrderDetail)
                    {
                        SaleReport view_salereport = new SaleReport();
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

                        var get_saleorder_details = dba.tbl_sales_order_details.Where(a => a.tbl_seller_id == sellers_id && a.tbl_sales_order_id == item.ob_tbl_sales_order.id).FirstOrDefault();
                        if (item.ob_tbl_sales_order.n_item_orderstatus == 2)
                        {
                            view_salereport.OrderID = item.ob_tbl_sales_order.amazon_order_id;
                            view_salereport.CancelledOrderDate = Convert.ToDateTime(item.ob_tbl_sales_order.purchase_date).ToString("yyyy-MM-dd");
                            if (get_saleorder_details != null)
                            {
                                view_salereport.CancelledorderAmount = Convert.ToDouble(get_saleorder_details.item_price_amount);
                            }// end of if(get_saleorder_details)
                        }

                        view_salereport.OrderID = item.ob_tbl_sales_order.amazon_order_id;
                        view_salereport.OrderDate = Convert.ToDateTime(item.ob_tbl_sales_order.purchase_date).ToString("yyyy-MM-dd");
                        if (get_saleorder_details != null)
                        {
                            view_salereport.OrderAmount = Convert.ToDouble(get_saleorder_details.item_price_amount);
                        }// end of if(get_saleorder_details)

                        var get_tblhistorydata = dba.tbl_order_history.Where(a => a.t_order_status == 9 && a.OrderID == item.ob_tbl_sales_order.amazon_order_id && a.tbl_seller_id == sellers_id).FirstOrDefault();
                        if (get_tblhistorydata != null)
                        {
                            view_salereport.SettlementDate = Convert.ToDateTime(get_tblhistorydata.ShipmentDate).ToString("yyyy-MM-dd");
                            view_salereport.Principal = Convert.ToDouble(get_tblhistorydata.amount_per_unit);
                        }

                        view_salereport.NetTotal = view_salereport.OrderAmount - view_salereport.CancelledorderAmount + view_salereport.Principal;
                        lstOrdertext2.Add(view_salereport);
                    }// end of foreach loop(item) 
                    /////----------------------------------Export Excel----------------------------------
                    string sendOnMail = form["sendmail"];
                    string value1 = form["command"];
                    if (value1 == "Export" || sendOnMail == "Send On Mail")
                    {
                        if (ddl_export != null)
                        {
                            bool mail = false;
                            if (sendOnMail != null) mail = true;
                            string EmailID = form["txt_email"];

                            DataTable header = new DataTable();
                            string colum = " ";
                            for (int t = 0; t < 6; t++)
                            {
                                header.Columns.Add(colum);
                                colum = colum + " ";
                            }
                            DataRow errow = header.NewRow();
                            errow[2] = " ";
                            header.Rows.Add(errow);
                            DataRow drow = header.NewRow();
                            drow[1] = "From Date";
                            drow[2] = txt_from;
                            drow[4] = "Report Taken On";
                            drow[5] = DateTime.Now.ToString("dd/MMM/yyyy");
                            header.Rows.Add(drow);
                            DataRow brow = header.NewRow();
                            brow[1] = "To Date";
                            brow[2] = txt_to;
                            header.Rows.Add(brow);
                            DataRow erow = header.NewRow();
                            erow[2] = " ";
                            header.Rows.Add(erow);
                            DataTable export_dt = new DataTable();
                            export_dt = cf.CreateSaleLedgerDatatable(lstOrdertext2);
                            int type = Convert.ToInt32(ddl_export);
                            if (export_dt.Rows.Count > 0)
                            {
                                int r = cf.Export("Sale Ledger Report", header, export_dt, Response, type, mail, EmailID);
                            }
                            else ViewData["Message"] = "There are No records In Table";
                        }
                        else ViewData["Message"] = "Please Select Export Type";
                    }
                    //----------------------------------------END-----------------------------------------////
                }
            }// end of try block
            catch (Exception ex)
            {
            }// end of catch block
            return View(lstOrdertext2);
        }
        #endregion

        #region Debtor Ledger report
        public ActionResult DebtorLedgerReport1(FormCollection form, int? ddl_marketplace, DateTime? txt_from, DateTime? txt_to, int? ddl_export)
        {
            cf = new comman_function();
            if (!cf.session_check())
                return RedirectToAction("Login", "Home");
            int sellers_id = Convert.ToInt32(Session["SellerID"]);
            cf = new comman_function();
            ViewData["ExportList"] = cf.GetExcelExportList();
            List<SelectListItem> lst1_loc = cf.GetMarketPlace(sellers_id);
            ViewData["MarKetPlace"] = lst1_loc;
            List<SaleReport> lstOrdertext2 = new List<SaleReport>();
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
                        SaleReport view_salereport = new SaleReport();
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

                        var get_saleorder_details = dba.tbl_sales_order_details.Where(a => a.tbl_seller_id == sellers_id && a.tbl_sales_order_id == item.ob_tbl_sales_order.id).FirstOrDefault();

                        if (item.ob_tbl_sales_order.order_status == "Canceled")
                        {
                            view_salereport.OrderID = item.ob_tbl_sales_order.amazon_order_id;
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

                        view_salereport.OrderID = item.ob_tbl_sales_order.amazon_order_id;
                        view_salereport.Status = item.ob_tbl_sales_order.order_status;
                        view_salereport.OrderDate = Convert.ToDateTime(item.ob_tbl_sales_order.purchase_date).ToString("yyyy-MM-dd");


                        double Principal = 0;
                        double orderigst = 0;
                        double ordersgst = 0;
                        double ordercgst = 0;

                        //var get_saleorder_details1 = dba.tbl_sales_order_details.Where(a => a.tbl_sales_order_id == item.ob_tbl_sales_order.id && a.tbl_seller_id == sellers_id).ToList();//to get sale_order details 
                        //if (get_saleorder_details1 != null)
                        //{
                        //    foreach (var order_details in get_saleorder_details1)
                        //    {
                        //        double ord_detail_Principal = order_details.item_price_amount;
                        //        Principal = Principal + ord_detail_Principal;
                        //        view_salereport.Principal = Principal;
                        //        var get_tax = dba.tbl_tax.Where(a => a.tbl_referneced_id == order_details.id && a.reference_type == 3).FirstOrDefault();
                        //        {
                        //            if (get_tax != null)
                        //            {
                        //                orderigst = orderigst + Convert.ToDouble(get_tax.Igst_amount);
                        //                ordersgst = ordersgst + Convert.ToDouble(get_tax.sgst_amount);
                        //                ordercgst = ordercgst + Convert.ToDouble(get_tax.CGST_amount);

                        //                view_salereport.orderigst = orderigst;
                        //                view_salereport.ordersgst = ordersgst;
                        //                view_salereport.ordercgst = ordercgst;
                        //            }
                        //        }
                        //        view_salereport.SumFee = view_salereport.Principal + view_salereport.orderigst + view_salereport.ordersgst + view_salereport.ordercgst;
                        //    }
                        //    iRepeatDetailData++;
                        //}
                        if (get_saleorder_details != null)
                        {
                            var get_tax_details = dba.tbl_tax.Where(a => a.tbl_referneced_id == get_saleorder_details.id && a.reference_type == 3).FirstOrDefault();
                            view_salereport.OrderAmount = Convert.ToDouble(get_saleorder_details.item_price_amount);
                            //view_salereport.Shipping = Convert.ToDouble(get_saleorder_details.shipping_price_Amount);
                            //view_salereport.GiftAmount = Convert.ToDouble(get_saleorder_details.giftwrapprice_amount);
                            //view_salereport.Product_Tax = get_saleorder_details.item_tax_amount;
                            //view_salereport.Shipping_Tax = get_saleorder_details.shipping_tax_Amount;


                            if (get_tax_details != null)
                            {
                                orderigst = orderigst + Convert.ToDouble(get_tax_details.Igst_amount);
                                ordersgst = ordersgst + Convert.ToDouble(get_tax_details.sgst_amount);
                                ordercgst = ordercgst + Convert.ToDouble(get_tax_details.CGST_amount);

                                view_salereport.orderigst = orderigst;
                                view_salereport.ordersgst = ordersgst;
                                view_salereport.ordercgst = ordercgst;
                            }

                            view_salereport.SumFee = view_salereport.OrderAmount + view_salereport.orderigst + view_salereport.ordersgst + view_salereport.ordercgst;
                            //view_salereport.SumFee = view_salereport.OrderAmount + view_salereport.Shipping + view_salereport.GiftAmount + view_salereport.Product_Tax + view_salereport.Shipping_Tax + view_salereport.GiftTax + view_salereport.ShippingDiscount + view_salereport.ShippingDiscount_tax;
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
                        // var get_tblhistory = dba.tbl_order_history.Where(a => a.tbl_seller_id == sellers_id && a.t_order_status == 9 && a.OrderID == item.ob_tbl_sales_order.amazon_order_id).FirstOrDefault();
                        var get_tblhistory = dba.tbl_order_history.Where(a => a.tbl_seller_id == sellers_id && a.OrderID == item.ob_tbl_sales_order.amazon_order_id && a.t_order_status == 9).FirstOrDefault();
                        if (get_tblhistory != null)
                        {
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
                    /////----------------------------------Export Excel----------------------------------

                    string sendOnMail = form["sendmail"];
                    string value1 = form["command"];
                    if (value1 == "Export" || sendOnMail == "Send On Mail")
                    {
                        if (ddl_export != null)
                        {
                            bool mail = false;
                            if (sendOnMail != null) mail = true;
                            string EmailID = form["txt_email"];

                            DataTable header = new DataTable();
                            string colum = " ";
                            for (int t = 0; t < 6; t++)
                            {
                                header.Columns.Add(colum);
                                colum = colum + " ";
                            }
                            DataRow errow = header.NewRow();
                            errow[2] = " ";
                            header.Rows.Add(errow);
                            DataRow drow = header.NewRow();
                            drow[1] = "From Date";
                            drow[2] = txt_from;
                            drow[4] = "Report Taken On";
                            drow[5] = DateTime.Now.ToString("dd/MMM/yyyy");
                            header.Rows.Add(drow);
                            DataRow brow = header.NewRow();
                            brow[1] = "To Date";
                            brow[2] = txt_to;
                            header.Rows.Add(brow);
                            DataRow erow = header.NewRow();
                            erow[2] = " ";
                            header.Rows.Add(erow);
                            DataTable export_dt = new DataTable();
                            export_dt = cf.CreateDebtorLedgerDatatable(lstOrdertext2);
                            int type = Convert.ToInt32(ddl_export);
                            if (export_dt.Rows.Count > 0)
                            {
                                int r = cf.Export("Debtor Ledger Report", header, export_dt, Response, type, mail, EmailID);
                            }
                            else ViewData["Message"] = "There are No records In Table";
                        }
                        else ViewData["Message"] = "Please Select Export Type";
                    }
                    //----------------------------------------END-----------------------------------------////
                }
            }// end of try block
            catch (Exception ex)
            {
            }// end of catch block

            return View(lstOrdertext2);
        }

        public ActionResult DebtorLedgerReport(FormCollection form, int? ddl_marketplace, DateTime? txt_from, DateTime? txt_to, int? ddl_export)
        {
            cf = new comman_function();
            if (!cf.session_check())
                return RedirectToAction("Login", "Home");
            int sellers_id = Convert.ToInt32(Session["SellerID"]);
            ViewData["ExportList"] = cf.GetExcelExportList();
            List<SelectListItem> lst1_loc = cf.GetMarketPlace(sellers_id);
            ViewData["MarKetPlace"] = lst1_loc;
            List<SaleReport> lstOrdertext2 = new List<SaleReport>();
            SaleReport view_salereport = null;
            try
            {
                if (txt_from != null && txt_to != null)
                {
                    SalesorderReport obj = new SalesorderReport();
                    if (ddl_marketplace == 3)
                    {
                        obj.Get_AmazonDebtorLedger(lstOrdertext2, view_salereport, ddl_marketplace, txt_from, txt_to, sellers_id);
                    }
                    else if (ddl_marketplace == 1)
                    {
                        obj.Get_FlipkartDebtorLedger(lstOrdertext2, view_salereport, ddl_marketplace, txt_from, txt_to, sellers_id);
                    }
                    else if (ddl_marketplace == 5)
                    {
                        obj.Get_PaytmDebtorLedger(lstOrdertext2, view_salereport, ddl_marketplace, txt_from, txt_to, sellers_id);
                    }
                    /////----------------------------------Export Excel----------------------------------

                    string sendOnMail = form["sendmail"];
                    string value1 = form["command"];
                    if (value1 == "Export" || sendOnMail == "Send On Mail")
                    {
                        if (ddl_export != null)
                        {
                            bool mail = false;
                            if (sendOnMail != null) mail = true;
                            string EmailID = form["txt_email"];

                            DataTable header = new DataTable();
                            string colum = " ";
                            for (int t = 0; t < 6; t++)
                            {
                                header.Columns.Add(colum);
                                colum = colum + " ";
                            }
                            DataRow errow = header.NewRow();
                            errow[2] = " ";
                            header.Rows.Add(errow);
                            DataRow drow = header.NewRow();
                            drow[1] = "From Date";
                            drow[2] = txt_from;
                            drow[4] = "Report Taken On";
                            drow[5] = DateTime.Now.ToString("dd/MMM/yyyy");
                            header.Rows.Add(drow);
                            DataRow brow = header.NewRow();
                            brow[1] = "To Date";
                            brow[2] = txt_to;
                            header.Rows.Add(brow);
                            DataRow erow = header.NewRow();
                            erow[2] = " ";
                            header.Rows.Add(erow);
                            DataTable export_dt = new DataTable();
                            export_dt = cf.CreateDebtorLedgerDatatable(lstOrdertext2);
                            int type = Convert.ToInt32(ddl_export);
                            if (export_dt.Rows.Count > 0)
                            {
                                int r = cf.Export("Debtor Ledger Report", header, export_dt, Response, type, mail, EmailID);
                            }
                            else ViewData["Message"] = "There are No records In Table";
                        }
                        else ViewData["Message"] = "Please Select Export Type";
                    }
                    //----------------------------------------END-----------------------------------------////
                }
            }// end of try block
            catch (Exception ex)
            {
            }// end of catch block

            return View(lstOrdertext2);
        }
        #endregion

        #region Other Revenue Ledger report
        /// <summary>
        /// this is for Other Revenue Ledger report
        /// </summary>
        /// <param name="form"></param>
        /// <param name="ddl_marketplace"></param>
        /// <param name="txt_from"></param>
        /// <param name="txt_to"></param>
        /// <param name="ddl_export"></param>
        /// <returns></returns>
        public ActionResult OtherRevenueLedgerReport(FormCollection form, int? ddl_marketplace, DateTime? txt_from, DateTime? txt_to, int? ddl_export)
        {
            //txt_from = txt_from != null ? txt_from : DateTime.Parse("2017-09-17");
            //txt_to = txt_to != null ? txt_to : DateTime.Parse("2017-09-25");
            cf = new comman_function();
            if (!cf.session_check())
                return RedirectToAction("Login", "Home");
            int sellers_id = Convert.ToInt32(Session["SellerID"]);
            cf = new comman_function();
            ViewData["ExportList"] = cf.GetExcelExportList();
            //List<SelectListItem> lst_loc = cf.GetSrchMarketPalceList(sellers_id);
            //ViewData["MarKetPlaceList"] = lst_loc;
            List<SelectListItem> lst1_loc = cf.GetMarketPlace(sellers_id);
            ViewData["MarKetPlace"] = lst1_loc;
            List<SaleReport> lstOrdertext2 = new List<SaleReport>();
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
                        SaleReport view_salereport = new SaleReport();
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
                        var get_saleorder_details = dba.tbl_sales_order_details.Where(a => a.tbl_seller_id == sellers_id && a.tbl_sales_order_id == item.ob_tbl_sales_order.id).FirstOrDefault();
                        if (item.ob_tbl_sales_order.order_status == "Canceled")
                        {
                            view_salereport.OrderID = item.ob_tbl_sales_order.amazon_order_id;
                            view_salereport.CancelledOrderDate = Convert.ToDateTime(item.ob_tbl_sales_order.purchase_date).ToString("yyyy-MM-dd");
                            if (get_saleorder_details != null)
                            {
                                view_salereport.CancelShipping = Convert.ToDouble(get_saleorder_details.shipping_price_Amount);
                                view_salereport.CancelGiftWarp = Convert.ToDouble(get_saleorder_details.giftwrapprice_amount);
                                view_salereport.SumCancelFee = view_salereport.CancelShipping + view_salereport.CancelGiftWarp;
                            }// end of if(get_saleorder_details)
                        }

                        view_salereport.OrderID = item.ob_tbl_sales_order.amazon_order_id;
                        view_salereport.OrderDate = Convert.ToDateTime(item.ob_tbl_sales_order.purchase_date).ToString("yyyy-MM-dd");
                        if (get_saleorder_details != null)
                        {
                            view_salereport.Shipping = Convert.ToDouble(get_saleorder_details.shipping_price_Amount);
                            view_salereport.GiftAmount = Convert.ToDouble(get_saleorder_details.giftwrapprice_amount);
                            view_salereport.SumOrder = view_salereport.Shipping + view_salereport.GiftAmount;
                        }// end of if(get_saleorder_details)

                        var get_historyData = dba.tbl_order_history.Where(a => a.tbl_seller_id == sellers_id && a.t_order_status == 9 && a.OrderID == item.ob_tbl_sales_order.amazon_order_id).FirstOrDefault();
                        if (get_historyData != null)
                        {
                            view_salereport.refundshipping = Convert.ToDouble(get_historyData.shipping_price);
                            view_salereport.refundgiftwrap = Convert.ToDouble(get_historyData.Giftwrap_price);
                            view_salereport.refundtotal = view_salereport.refundshipping + view_salereport.refundgiftwrap;
                        }// end of if(get_historyData)
                        view_salereport.NetTotal = view_salereport.SumOrder - view_salereport.SumCancelFee + view_salereport.refundtotal;
                        lstOrdertext2.Add(view_salereport);
                    }// end of foreach(item)
                    /////----------------------------------Export Excel----------------------------------

                    string sendOnMail = form["sendmail"];
                    string value1 = form["command"];
                    if (value1 == "Export" || sendOnMail == "Send On Mail")
                    {
                        if (ddl_export != null)
                        {
                            bool mail = false;
                            if (sendOnMail != null) mail = true;
                            string EmailID = form["txt_email"];

                            DataTable header = new DataTable();
                            string colum = " ";
                            for (int t = 0; t < 6; t++)
                            {
                                header.Columns.Add(colum);
                                colum = colum + " ";
                            }
                            DataRow errow = header.NewRow();
                            errow[2] = " ";
                            header.Rows.Add(errow);
                            DataRow drow = header.NewRow();
                            drow[1] = "From Date";
                            drow[2] = txt_from;
                            drow[4] = "Report Taken On";
                            drow[5] = DateTime.Now.ToString("dd/MMM/yyyy");
                            header.Rows.Add(drow);
                            DataRow brow = header.NewRow();
                            brow[1] = "To Date";
                            brow[2] = txt_to;
                            header.Rows.Add(brow);
                            DataRow erow = header.NewRow();
                            erow[2] = " ";
                            header.Rows.Add(erow);
                            DataTable export_dt = new DataTable();
                            export_dt = cf.CreateRevenueLedgerDatatable(lstOrdertext2);
                            int type = Convert.ToInt32(ddl_export);
                            if (export_dt.Rows.Count > 0)
                            {
                                int r = cf.Export("Other Revenue Ledger Report", header, export_dt, Response, type, mail, EmailID);
                            }
                            else ViewData["Message"] = "There are No records In Table";
                        }
                        else ViewData["Message"] = "Please Select Export Type";
                    }
                    //----------------------------------------END-----------------------------------------////
                }
            }
            catch (Exception ex)
            {
            }
            return View(lstOrdertext2);
        }
        #endregion

        #region Product Profitability With Tax report
        /// <summary>
        /// this is for Product Profitability With Tax report
        /// </summary>
        /// <param name="from"></param>
        /// <param name="ddl_marketplace"></param>
        /// <param name="txt_from"></param>
        /// <param name="txt_to"></param>
        /// <returns></returns>
        public ActionResult ProductProfitabilityWithTax(FormCollection from, int? ddl_marketplace, DateTime? txt_from, DateTime? txt_to)
        {
            //txt_from = txt_from != null ? txt_from : DateTime.Parse("2017-11-07");
            //txt_to = txt_to != null ? txt_to : DateTime.Parse("2017-11-25");
            cf = new comman_function();
            if (!cf.session_check())
                return RedirectToAction("Login", "Home");
            int sellers_id = Convert.ToInt32(Session["SellerID"]);

            //List<SelectListItem> lst_loc = cf.GetSrchMarketPalceList(sellers_id);
            //ViewData["MarKetPlaceList"] = lst_loc;
            List<SelectListItem> lst1_loc = cf.GetMarketPlace(sellers_id);
            ViewData["MarKetPlaceList"] = lst1_loc;
            List<SaleReport> lstOrdertext2 = new List<SaleReport>();

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
                    var get_marketplace = db.m_marketplace.Where(a => a.isactive == 1).ToList();

                    foreach (var item in GetSaleOrderDetail)
                    {
                        SaleReport view_salereport = new SaleReport();

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



                        var get_saleorder_details = dba.tbl_sales_order_details.Where(a => a.tbl_seller_id == sellers_id && a.tbl_sales_order_id == item.ob_tbl_sales_order.id).FirstOrDefault();

                        view_salereport.OrderID = item.ob_tbl_sales_order.amazon_order_id;
                        view_salereport.OrderDate = Convert.ToDateTime(item.ob_tbl_sales_order.purchase_date).ToString("yyyy-MM-dd");
                        if (get_saleorder_details != null)
                        {
                            view_salereport.Principal = Convert.ToDouble(get_saleorder_details.item_price_amount);
                            view_salereport.Shipping = Convert.ToDouble(get_saleorder_details.shipping_price_Amount);
                            view_salereport.GiftAmount = Convert.ToDouble(get_saleorder_details.giftwrapprice_amount);
                            view_salereport.Product_Tax = Convert.ToDouble(get_saleorder_details.item_tax_amount);
                            view_salereport.Shipping_Tax = Convert.ToDouble(get_saleorder_details.shipping_tax_Amount);
                            view_salereport.GiftTax = Convert.ToDouble(get_saleorder_details.giftwraptax_amount);
                            view_salereport.SumOrder = view_salereport.Principal + view_salereport.Shipping + view_salereport.GiftAmount + view_salereport.Product_Tax + view_salereport.Shipping_Tax + view_salereport.GiftTax;

                            //--------------------------- get Product name and Product Value compare by SKU code------------//

                            var get_inventory = dba.tbl_inventory.Where(a => a.tbl_sellers_id == sellers_id && a.sku.ToLower() == get_saleorder_details.sku_no.ToLower() && a.isactive == 1).FirstOrDefault();
                            if (get_inventory != null)
                            {
                                view_salereport.ProductName = get_inventory.item_name;
                                var get_purchase_details = dba.tbl_purchase_details.Where(a => a.tbl_sellers_id == sellers_id && a.tbl_inventory_id == get_inventory.id).FirstOrDefault();
                                if (get_purchase_details != null)
                                {
                                    view_salereport.ProductValue = Convert.ToDouble(get_purchase_details.base_amount);
                                    var gettbl_tax = dba.tbl_tax.Where(a => a.reference_type == 1 && a.tbl_seller_id == sellers_id && a.tbl_referneced_id == get_purchase_details.id).FirstOrDefault();
                                    if (gettbl_tax != null)
                                    {
                                        view_salereport.productIgstamt = Convert.ToDouble(gettbl_tax.Igst_amount);
                                        view_salereport.productSgstamt = Convert.ToDouble(gettbl_tax.sgst_amount);
                                        view_salereport.productCgstamt = Convert.ToDouble(gettbl_tax.CGST_amount);
                                    }
                                    view_salereport.SumFee = view_salereport.ProductValue + view_salereport.productIgstamt + view_salereport.productSgstamt + view_salereport.productCgstamt;
                                }
                            }// end of if (get_inventory)
                        }// end of if(get_saleorder_details)

                        var getsettlementdetails = dba.m_tbl_expense.Where(a => a.settlement_order_id == item.ob_tbl_sales_order.amazon_order_id && a.tbl_seller_id == sellers_id).ToList();
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
                                            view_salereport.FBAFEE = Convert.ToDouble(item1.expense_amount);
                                            if (gettax_details != null)
                                            {
                                                view_salereport.FBACGST = Convert.ToDouble(gettax_details.CGST_amount);
                                                view_salereport.FBASGST = Convert.ToDouble(gettax_details.sgst_amount);
                                            }
                                        }
                                    }
                                    else if (nam == "Technology Fee")
                                    {
                                        if (get_expdetails.id == exp_id)
                                        {
                                            view_salereport.TechnologyFee = Convert.ToDouble(item1.expense_amount);
                                            if (gettax_details != null)
                                            {
                                                view_salereport.TechnologyIGST = Convert.ToDouble(gettax_details.Igst_amount);
                                            }
                                        }
                                    }
                                    else if (nam == "Commission")
                                    {
                                        if (get_expdetails.id == exp_id)
                                        {
                                            view_salereport.CommissionFee = Convert.ToDouble(item1.expense_amount);
                                            if (gettax_details != null)
                                            {
                                                view_salereport.CommissionIGST = Convert.ToDouble(gettax_details.Igst_amount);
                                            }
                                        }
                                    }
                                    else if (nam == "Fixed closing fee")
                                    {
                                        if (get_expdetails.id == exp_id)
                                        {
                                            view_salereport.FixedClosingFee = Convert.ToDouble(item1.expense_amount);
                                            if (gettax_details != null)
                                            {
                                                view_salereport.FixedclosingIGST = Convert.ToDouble(gettax_details.Igst_amount);
                                            }
                                        }
                                    }
                                    else if (nam == "Shipping Chargeback")
                                    {
                                        if (get_expdetails.id == exp_id)
                                        {
                                            view_salereport.ShippingChargebackFee = Convert.ToDouble(item1.expense_amount);
                                            if (gettax_details != null)
                                            {
                                                view_salereport.shippingchargeCGST = Convert.ToDouble(gettax_details.CGST_amount);
                                                view_salereport.shippingchargeSGST = Convert.ToDouble(gettax_details.sgst_amount);
                                            }
                                        }
                                    }
                                    else if (nam == "Shipping discount")
                                    {
                                        if (get_expdetails.id == exp_id)
                                        {
                                            view_salereport.ShippingDiscountFee = Convert.ToDouble(item1.expense_amount);
                                            if (gettax_details != null)
                                            {
                                                view_salereport.Shippingtaxdiscount = Convert.ToDouble(gettax_details.Igst_amount);
                                            }
                                        }
                                    }

                                    else if (nam == "Refund commission")
                                    {
                                        if (get_expdetails.id == exp_id)
                                        {
                                            view_salereport.RefundCommision = Convert.ToDouble(item1.expense_amount);
                                            if (gettax_details != null)
                                            {
                                                view_salereport.RefundDiscount = Convert.ToDouble(gettax_details.Igst_amount);
                                            }
                                        }
                                    }
                                } // end of if(get_expdetails)
                                view_salereport.ExpenseTotal = view_salereport.FBAFEE + view_salereport.TechnologyFee + view_salereport.CommissionFee + view_salereport.FixedClosingFee + view_salereport.ShippingChargebackFee + view_salereport.ShippingDiscountFee + view_salereport.RefundCommision;
                                view_salereport.SumTaxFee = view_salereport.FBACGST + view_salereport.FBASGST + view_salereport.TechnologyIGST + view_salereport.CommissionIGST + view_salereport.FixedclosingIGST + view_salereport.shippingchargeCGST + view_salereport.shippingchargeSGST + view_salereport.Shippingtaxdiscount + view_salereport.RefundDiscount;
                                view_salereport.FullExpenseTotal = view_salereport.ExpenseTotal + view_salereport.SumTaxFee;
                            }// end if foreach(item1)
                        }// end of if(getsettlementdetails)

                        view_salereport.NetTotal = view_salereport.SumOrder - (view_salereport.SumFee - view_salereport.FullExpenseTotal);



                        lstOrdertext2.Add(view_salereport);
                    }// end of for each (item)
                }
            }// end of try block
            catch (Exception ex)
            {
            }//end if catch block

            return View(lstOrdertext2);
        }
        #endregion

        #region Product Profitability Without Tax report
        /// <summary>
        /// this is for Product Profitability Without Tax report
        /// </summary>
        /// <param name="from"></param>
        /// <param name="txt_from"></param>
        /// <param name="txt_to"></param>
        /// <returns></returns>
        public ActionResult ProductProfitabilityWithoutTax(FormCollection from, int? ddl_marketplace, DateTime? txt_from, DateTime? txt_to)
        {
            //txt_from = txt_from != null ? txt_from : DateTime.Parse("2017-11-07");
            //txt_to = txt_to != null ? txt_to : DateTime.Parse("2017-11-25");
            cf = new comman_function();
            if (!cf.session_check())
                return RedirectToAction("Login", "Home");
            int sellers_id = Convert.ToInt32(Session["SellerID"]);
            cf = new comman_function();
            //List<SelectListItem> lst_loc = cf.GetSrchMarketPalceList(sellers_id);
            //ViewData["MarKetPlaceList"] = lst_loc;
            List<SelectListItem> lst1_loc = cf.GetMarketPlace(sellers_id);
            ViewData["MarKetPlaceList"] = lst1_loc;
            List<SaleReport> lstOrdertext2 = new List<SaleReport>();

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
                        if (txt_from != null && txt_to != null)
                        {
                            GetSaleOrderDetail = GetSaleOrderDetail.Where(a => a.ob_tbl_sales_order.purchase_date.Date >= txt_from && a.ob_tbl_sales_order.purchase_date.Date <= txt_to).ToList();
                        }
                    }
                    var get_marketplace = db.m_marketplace.Where(a => a.isactive == 1).ToList();

                    foreach (var item in GetSaleOrderDetail)
                    {
                        SaleReport view_salereport = new SaleReport();

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



                        var get_saleorder_details = dba.tbl_sales_order_details.Where(a => a.tbl_seller_id == sellers_id && a.tbl_sales_order_id == item.ob_tbl_sales_order.id).FirstOrDefault();

                        view_salereport.OrderID = item.ob_tbl_sales_order.amazon_order_id;
                        view_salereport.OrderDate = Convert.ToDateTime(item.ob_tbl_sales_order.purchase_date).ToString("yyyy-MM-dd");
                        if (get_saleorder_details != null)
                        {
                            view_salereport.Principal = Convert.ToDouble(get_saleorder_details.item_price_amount);
                            view_salereport.Shipping = Convert.ToDouble(get_saleorder_details.shipping_price_Amount);
                            view_salereport.GiftAmount = Convert.ToDouble(get_saleorder_details.giftwrapprice_amount);
                            view_salereport.SumOrder = view_salereport.Principal + view_salereport.Shipping + view_salereport.GiftAmount;

                            //--------------------------- get Product name and Product Value compare by SKU code------------//

                            var get_inventory = dba.tbl_inventory.Where(a => a.tbl_sellers_id == sellers_id && a.sku.ToLower() == get_saleorder_details.sku_no.ToLower() && a.isactive == 1).FirstOrDefault();
                            if (get_inventory != null)
                            {
                                view_salereport.ProductName = get_inventory.item_name;
                                var get_purchase_details = dba.tbl_purchase_details.Where(a => a.tbl_sellers_id == sellers_id && a.tbl_inventory_id == get_inventory.id).FirstOrDefault();
                                if (get_purchase_details != null)
                                {
                                    view_salereport.ProductValue = Convert.ToDouble(get_purchase_details.base_amount);
                                    view_salereport.SumFee = view_salereport.ProductValue;
                                }
                            }// end of if (get_inventory)
                        }// end of if(get_saleorder_details)

                        var getsettlementdetails = dba.m_tbl_expense.Where(a => a.settlement_order_id == item.ob_tbl_sales_order.amazon_order_id && a.tbl_seller_id == sellers_id).ToList();
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
                                            view_salereport.FBAFEE = Convert.ToDouble(item1.expense_amount);
                                        }
                                    }
                                    else if (nam == "Technology Fee")
                                    {
                                        if (get_expdetails.id == exp_id)
                                        {
                                            view_salereport.TechnologyFee = Convert.ToDouble(item1.expense_amount);
                                        }
                                    }
                                    else if (nam == "Commission")
                                    {
                                        if (get_expdetails.id == exp_id)
                                        {
                                            view_salereport.CommissionFee = Convert.ToDouble(item1.expense_amount);
                                        }
                                    }
                                    else if (nam == "Fixed closing fee")
                                    {
                                        if (get_expdetails.id == exp_id)
                                        {
                                            view_salereport.FixedClosingFee = Convert.ToDouble(item1.expense_amount);
                                        }
                                    }
                                    else if (nam == "Shipping Chargeback")
                                    {
                                        if (get_expdetails.id == exp_id)
                                        {
                                            view_salereport.ShippingChargebackFee = Convert.ToDouble(item1.expense_amount);
                                        }
                                    }
                                    else if (nam == "Shipping discount")
                                    {
                                        if (get_expdetails.id == exp_id)
                                        {
                                            view_salereport.ShippingDiscountFee = Convert.ToDouble(item1.expense_amount);
                                        }
                                    }

                                    else if (nam == "Refund commission")
                                    {
                                        if (get_expdetails.id == exp_id)
                                        {
                                            view_salereport.RefundCommision = Convert.ToDouble(item1.expense_amount);
                                        }
                                    }
                                } // end of if(get_expdetails)
                                view_salereport.ExpenseTotal = view_salereport.FBAFEE + view_salereport.TechnologyFee + view_salereport.CommissionFee + view_salereport.FixedClosingFee + view_salereport.ShippingChargebackFee + view_salereport.ShippingDiscountFee + view_salereport.RefundCommision;
                            }// end if foreach(item1)
                        }// end of if(getsettlementdetails)

                        view_salereport.NetTotal = view_salereport.SumOrder - (view_salereport.SumFee - view_salereport.ExpenseTotal);



                        lstOrdertext2.Add(view_salereport);
                    }// end of for each (item)
                }
            }// end of try block
            catch (Exception ex)
            {
            }//end if catch block

            return View(lstOrdertext2);
        }
        #endregion

        #region Net Realization With Tax report
        /// <summary>
        /// this is for Net Realization  With Tax Report
        /// </summary>
        /// <param name="from"></param>
        /// <param name="ddl_marketplace"></param>
        /// <param name="txt_from"></param>
        /// <param name="txt_to"></param>
        /// <returns></returns>
        public ActionResult NetRealizationWithTax(FormCollection from, int? ddl_marketplace, DateTime? txt_from, DateTime? txt_to, int? ddl_export)
        {
            // txt_from = txt_from != null ? txt_from : DateTime.Parse("2017-11-07");
            // txt_to = txt_to != null ? txt_to : DateTime.Parse("2017-11-25");
            cf = new comman_function();
            if (!cf.session_check())
                return RedirectToAction("Login", "Home");
            int sellers_id = Convert.ToInt32(Session["SellerID"]);
            //List<SelectListItem> lst_loc = cf.GetSrchMarketPalceList(sellers_id);
            //ViewData["MarKetPlaceList"] = lst_loc;

            List<SelectListItem> lst1_loc = cf.GetMarketPlace(sellers_id);
            ViewData["MarKetPlace"] = lst1_loc;

            ViewData["ExportList"] = cf.GetExcelExportList();

            List<SaleReport> lstOrdertext2 = new List<SaleReport>();

            try
            {
                var GetSaleOrderDetail = (from ob_tbl_sales_order in dba.tbl_sales_order
                                          select new
                                          {
                                              ob_tbl_sales_order = ob_tbl_sales_order
                                          }).Where(a => a.ob_tbl_sales_order.tbl_sellers_id == sellers_id && a.ob_tbl_sales_order.tbl_Marketplace_Id == ddl_marketplace).ToList();

                if (GetSaleOrderDetail != null)
                {
                    if (txt_from != null && txt_to != null)
                    {
                        GetSaleOrderDetail = GetSaleOrderDetail.Where(a => a.ob_tbl_sales_order.purchase_date.Date >= txt_from && a.ob_tbl_sales_order.purchase_date.Date <= txt_to).ToList();
                    }
                }
                var get_marketplace = db.m_marketplace.Where(a => a.isactive == 1).ToList();

                foreach (var item in GetSaleOrderDetail)
                {
                    SaleReport view_salereport = new SaleReport();

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
                    var get_saleorder_details = dba.tbl_sales_order_details.Where(a => a.tbl_seller_id == sellers_id && a.tbl_sales_order_id == item.ob_tbl_sales_order.id).FirstOrDefault();
                    view_salereport.OrderID = item.ob_tbl_sales_order.amazon_order_id;
                    view_salereport.OrderDate = Convert.ToDateTime(item.ob_tbl_sales_order.purchase_date).ToString("yyyy-MM-dd");
                    if (get_saleorder_details != null)
                    {
                        view_salereport.Principal = Convert.ToDouble(get_saleorder_details.item_price_amount);
                        view_salereport.Shipping = Convert.ToDouble(get_saleorder_details.shipping_price_Amount);
                        view_salereport.GiftAmount = Convert.ToDouble(get_saleorder_details.giftwrapprice_amount);
                        view_salereport.Product_Tax = Convert.ToDouble(get_saleorder_details.item_tax_amount);
                        view_salereport.Shipping_Tax = Convert.ToDouble(get_saleorder_details.shipping_tax_Amount);
                        view_salereport.GiftTax = Convert.ToDouble(get_saleorder_details.giftwraptax_amount);
                        view_salereport.SumOrder = view_salereport.Principal + view_salereport.Shipping + view_salereport.GiftAmount + view_salereport.Product_Tax + view_salereport.Shipping_Tax + view_salereport.GiftTax;

                        //--------------------------- get Product name and Product Value compare by SKU code------------//
                        string itemname = "", sku = "";
                        double itemprice = 0;
                        var get_inventory = dba.tbl_inventory.Where(a => a.tbl_sellers_id == sellers_id && a.sku.ToLower() == get_saleorder_details.sku_no.ToLower()).FirstOrDefault();
                        if (get_inventory != null)
                        {
                            itemname = get_inventory.item_name;
                            sku = get_inventory.sku;

                            var get_inventoryeffective = dba.tbl_inventory_effectiveprice.Where(a => a.tbl_sellerid == sellers_id && a.tbl_inventory_id == get_inventory.id).OrderByDescending(a => a.Id).FirstOrDefault();
                            if (get_inventoryeffective != null)
                            {
                                itemprice = Convert.ToDouble(get_inventoryeffective.Gross_price);

                            }
                            view_salereport.ProductName = itemname;
                            view_salereport.ProductValue = itemprice;
                            view_salereport.skuNo = sku;
                        }
                    }// end of if(get_saleorder_details)

                    var getsettlementdetails = dba.m_tbl_expense.Where(a => a.Original_order_id == item.ob_tbl_sales_order.amazon_order_id && a.tbl_seller_id == sellers_id).ToList();
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
                                        view_salereport.FBAFEE = Convert.ToDouble(item1.expense_amount);
                                        if (gettax_details != null)
                                        {
                                            view_salereport.FBACGST = Convert.ToDouble(gettax_details.CGST_amount);
                                            view_salereport.FBASGST = Convert.ToDouble(gettax_details.sgst_amount);
                                        }
                                    }
                                }
                                else if (nam == "Technology Fee")
                                {
                                    if (get_expdetails.id == exp_id)
                                    {
                                        view_salereport.TechnologyFee = Convert.ToDouble(item1.expense_amount);
                                        if (gettax_details != null)
                                        {
                                            view_salereport.TechnologyIGST = Convert.ToDouble(gettax_details.Igst_amount);
                                            view_salereport.TechnologyCGST = Convert.ToDouble(gettax_details.CGST_amount);
                                            view_salereport.TechnologySGST = Convert.ToDouble(gettax_details.sgst_amount);
                                        }
                                    }
                                }
                                else if (nam == "Commission")
                                {
                                    if (get_expdetails.id == exp_id)
                                    {
                                        view_salereport.CommissionFee = Convert.ToDouble(item1.expense_amount);
                                        if (gettax_details != null)
                                        {
                                            view_salereport.CommissionIGST = Convert.ToDouble(gettax_details.Igst_amount);
                                            view_salereport.CommissionCGST = Convert.ToDouble(gettax_details.CGST_amount);
                                            view_salereport.CommissionSGST = Convert.ToDouble(gettax_details.sgst_amount);
                                        }
                                    }
                                }
                                else if (nam == "Fixed closing fee")
                                {
                                    if (get_expdetails.id == exp_id)
                                    {
                                        view_salereport.FixedClosingFee = Convert.ToDouble(item1.expense_amount);
                                        if (gettax_details != null)
                                        {
                                            view_salereport.FixedclosingIGST = Convert.ToDouble(gettax_details.Igst_amount);
                                            view_salereport.FixedclosingCGST = Convert.ToDouble(gettax_details.CGST_amount);
                                            view_salereport.FixedclosingSGST = Convert.ToDouble(gettax_details.sgst_amount);
                                        }
                                    }
                                }
                                else if (nam == "Shipping Chargeback")
                                {
                                    if (get_expdetails.id == exp_id)
                                    {
                                        view_salereport.ShippingChargebackFee = Convert.ToDouble(item1.expense_amount);
                                        if (gettax_details != null)
                                        {
                                            view_salereport.shippingchargeCGST = Convert.ToDouble(gettax_details.CGST_amount);
                                            view_salereport.shippingchargeSGST = Convert.ToDouble(gettax_details.sgst_amount);
                                        }
                                    }
                                }
                                else if (nam == "Shipping discount")
                                {
                                    if (get_expdetails.id == exp_id)
                                    {
                                        view_salereport.ShippingDiscountFee = Convert.ToDouble(item1.expense_amount);
                                        if (gettax_details != null)
                                        {
                                            view_salereport.Shippingtaxdiscount = Convert.ToDouble(gettax_details.Igst_amount);
                                        }
                                    }
                                }
                                else if (nam == "Refund commission")
                                {
                                    if (get_expdetails.id == exp_id)
                                    {
                                        view_salereport.RefundCommision = Convert.ToDouble(item1.expense_amount);
                                        if (gettax_details != null)
                                        {
                                            view_salereport.RefundDiscount = Convert.ToDouble(gettax_details.Igst_amount);
                                        }
                                    }
                                }
                                else if (nam == "Collection Fee")
                                {
                                    if (get_expdetails.id == exp_id)
                                    {

                                        view_salereport.flipCollection = Convert.ToDouble(item1.expense_amount);
                                        if (gettax_details != null)
                                        {
                                            view_salereport.flipCollectionIGST = Convert.ToDouble(gettax_details.Igst_amount);
                                            view_salereport.flipCollectionCGST = Convert.ToDouble(gettax_details.CGST_amount);
                                            view_salereport.flipCollectionSGST = Convert.ToDouble(gettax_details.sgst_amount);
                                        }
                                    }
                                }
                                else if (nam == "Reverse Shipping Fee")
                                {
                                    if (get_expdetails.id == exp_id)
                                    {
                                        view_salereport.flipReverseShipping = Convert.ToDouble(item1.expense_amount);
                                        if (gettax_details != null)
                                        {
                                            view_salereport.flipReverseShippingIGST = Convert.ToDouble(gettax_details.Igst_amount);
                                            view_salereport.flipReverseShippingCGST = Convert.ToDouble(gettax_details.CGST_amount);
                                            view_salereport.flipReverseShippingSGST = Convert.ToDouble(gettax_details.sgst_amount);
                                        }
                                    }
                                }
                                else if (nam == "Fixed Fee")
                                {
                                    if (get_expdetails.id == exp_id)
                                    {
                                        view_salereport.flipFixedFee = Convert.ToDouble(item1.expense_amount);
                                        if (gettax_details != null)
                                        {
                                            view_salereport.flipFixedFeeIGST = Convert.ToDouble(gettax_details.Igst_amount);
                                            view_salereport.flipFixedFeeCGST = Convert.ToDouble(gettax_details.CGST_amount);
                                            view_salereport.flipFixedFeeSGST = Convert.ToDouble(gettax_details.sgst_amount);
                                        }
                                    }
                                }

                                else if (nam == "Marketplace Commission")
                                {
                                    if (get_expdetails.id == exp_id)
                                    {
                                        view_salereport.payMarketplaceCommission = Convert.ToDouble(item1.expense_amount);
                                        if (gettax_details != null)
                                        {
                                            view_salereport.MarketplaceCommissionIGST = Convert.ToDouble(gettax_details.Igst_amount);
                                            view_salereport.MarketplaceCommissionCGST = Convert.ToDouble(gettax_details.CGST_amount);
                                            view_salereport.MarketplaceCommissionSGST = Convert.ToDouble(gettax_details.sgst_amount);
                                        }
                                    }
                                }
                                else if (nam == "Logistics Charges")
                                {
                                    if (get_expdetails.id == exp_id)
                                    {
                                        view_salereport.payLogisticsCharges = Convert.ToDouble(item1.expense_amount);
                                        if (gettax_details != null)
                                        {
                                            view_salereport.LogisticsChargesIGST = Convert.ToDouble(gettax_details.Igst_amount);
                                            view_salereport.LogisticsChargesCGST = Convert.ToDouble(gettax_details.CGST_amount);
                                            view_salereport.LogisticsChargesSGST = Convert.ToDouble(gettax_details.sgst_amount);
                                        }
                                    }
                                }
                                else if (nam == "PG Commission")
                                {
                                    if (get_expdetails.id == exp_id)
                                    {
                                        view_salereport.payPGCommission = Convert.ToDouble(item1.expense_amount);
                                        if (gettax_details != null)
                                        {
                                            view_salereport.PGCommissionIGST = Convert.ToDouble(gettax_details.Igst_amount);
                                            view_salereport.PGCommissionCGST = Convert.ToDouble(gettax_details.CGST_amount);
                                            view_salereport.PGCommissionSGST = Convert.ToDouble(gettax_details.sgst_amount);
                                        }
                                    }
                                }
                                else if (nam == "Penalty")
                                {
                                    if (get_expdetails.id == exp_id)
                                    {
                                        view_salereport.payPenalty = Convert.ToDouble(item1.expense_amount);
                                        if (gettax_details != null)
                                        {
                                            view_salereport.PenaltyIGST = Convert.ToDouble(gettax_details.Igst_amount);
                                            view_salereport.PenaltyCGST = Convert.ToDouble(gettax_details.CGST_amount);
                                            view_salereport.PenaltySGST = Convert.ToDouble(gettax_details.sgst_amount);
                                        }
                                    }
                                }
                                else if (nam == "Net Adjustments")
                                {
                                    if (get_expdetails.id == exp_id)
                                    {
                                        view_salereport.payNetAdjustments = Convert.ToDouble(item1.expense_amount);
                                        if (gettax_details != null)
                                        {
                                            view_salereport.NetAdjustmentsIGST = Convert.ToDouble(gettax_details.Igst_amount);
                                            view_salereport.NetAdjustmentsCGST = Convert.ToDouble(gettax_details.CGST_amount);
                                            view_salereport.NetAdjustmentsSGST = Convert.ToDouble(gettax_details.sgst_amount);
                                        }
                                    }
                                }

                            } // end of if(get_expdetails)
                            view_salereport.ExpenseTotal = view_salereport.FBAFEE + view_salereport.TechnologyFee + view_salereport.CommissionFee + view_salereport.FixedClosingFee + view_salereport.ShippingChargebackFee + view_salereport.ShippingDiscountFee + view_salereport.RefundCommision
                                                          + view_salereport.flipFixedFee + view_salereport.flipReverseShipping + view_salereport.flipCollection + view_salereport.payMarketplaceCommission 
                                                          +view_salereport.payLogisticsCharges + view_salereport.payPGCommission + view_salereport.payPenalty + view_salereport.payNetAdjustments;
                            view_salereport.SumTaxFee = view_salereport.FBACGST + view_salereport.FBASGST + view_salereport.TechnologyIGST + view_salereport.CommissionIGST + view_salereport.FixedclosingIGST + view_salereport.shippingchargeCGST
                                                        + view_salereport.shippingchargeSGST + view_salereport.Shippingtaxdiscount + view_salereport.RefundDiscount + view_salereport.TechnologyCGST
                                                        + view_salereport.TechnologySGST + view_salereport.CommissionCGST + view_salereport.CommissionSGST + view_salereport.FixedclosingCGST + view_salereport.FixedclosingSGST
                                                        + view_salereport.flipCollectionIGST + view_salereport.flipCollectionCGST + view_salereport.flipCollectionSGST + view_salereport.flipReverseShippingIGST + view_salereport.flipReverseShippingCGST + view_salereport.flipReverseShippingSGST
                                                        + view_salereport.flipFixedFeeIGST + view_salereport.flipFixedFeeCGST + view_salereport.flipFixedFeeSGST
                                                        + view_salereport.MarketplaceCommissionIGST + view_salereport.MarketplaceCommissionCGST + view_salereport.MarketplaceCommissionSGST + view_salereport.LogisticsChargesIGST + view_salereport.LogisticsChargesCGST + view_salereport.LogisticsChargesSGST
                                                        + view_salereport.PGCommissionIGST + view_salereport.PGCommissionCGST + view_salereport.PGCommissionSGST + view_salereport.PenaltyIGST + view_salereport.PenaltyCGST + view_salereport.PenaltySGST
                                                        + view_salereport.NetAdjustmentsIGST + view_salereport.NetAdjustmentsCGST + view_salereport.NetAdjustmentsSGST;

                            decimal avc = Convert.ToDecimal(view_salereport.ExpenseTotal + view_salereport.SumTaxFee);
                            decimal result111 = decimal.Round(avc, 2, MidpointRounding.AwayFromZero);
                            view_salereport.FullExpenseTotal =Convert.ToDouble(result111); //view_salereport.ExpenseTotal + view_salereport.SumTaxFee;
                        }// end if foreach(item1)
                    }// end of if(getsettlementdetails)
                    view_salereport.NetTotal = view_salereport.SumOrder + view_salereport.FullExpenseTotal;
                    view_salereport.Profit_lossAmount = view_salereport.NetTotal - view_salereport.ProductValue;
                    lstOrdertext2.Add(view_salereport);
                }// end of for each (item)

                /////----------------------------------Export Excel----------------------------------

                string sendOnMail = from["sendmail"];
                string value1 = from["command"];
                if (value1 == "Export" || sendOnMail == "Send On Mail")
                {
                    if (ddl_export != null)
                    {
                        bool mail = false;
                        if (sendOnMail != null) mail = true;
                        string EmailID = from["txt_email"];

                        DataTable header = new DataTable();
                        string colum = " ";
                        for (int t = 0; t < 6; t++)
                        {
                            header.Columns.Add(colum);
                            colum = colum + " ";
                        }
                        DataRow errow = header.NewRow();
                        errow[2] = " ";
                        header.Rows.Add(errow);
                        DataRow drow = header.NewRow();
                        drow[1] = "From Date";
                        drow[2] = txt_from;
                        drow[4] = "Report Taken On";
                        drow[5] = DateTime.Now.ToString("dd/MMM/yyyy");
                        header.Rows.Add(drow);
                        DataRow brow = header.NewRow();
                        brow[1] = "To Date";
                        brow[2] = txt_to;
                        header.Rows.Add(brow);
                        DataRow erow = header.NewRow();
                        erow[2] = " ";
                        header.Rows.Add(erow);
                        DataTable export_dt = new DataTable();
                        export_dt = cf.CreateNetrealizationReportWithTax(lstOrdertext2);
                        int type = Convert.ToInt32(ddl_export);
                        if (export_dt.Rows.Count > 0)
                        {
                            int r = cf.Export("Net Ralization Report Tax", header, export_dt, Response, type, mail, EmailID);
                        }
                        else ViewData["Message"] = "There are No records In Table";
                    }
                    else ViewData["Message"] = "Please Select Export Type";
                }
                //----------------------------------------END-----------------------------------------////
            }// end of try block
            catch (Exception ex)
            {
            }//end if catch block

            return View(lstOrdertext2);
        }
        #endregion

        #region Net Realization WithOut Tax report
        /// <summary>
        /// this is for Net Realization WithOut Tax Report
        /// </summary>
        /// <param name="form"></param>
        /// <param name="txt_from"></param>
        /// <param name="txt_to"></param>
        /// <param name="ddl_export"></param>
        /// <returns></returns>
        public ActionResult NetRealizationWithOutTax(FormCollection form, int? ddl_market_place, DateTime? txt_from, DateTime? txt_to, int? ddl_export)
        {
            cf = new comman_function();
            if (!cf.session_check())
                return RedirectToAction("Login", "Home");
            int sellers_id = Convert.ToInt32(Session["SellerID"]);
            List<SelectListItem> lst1_loc = cf.GetMarketPlace(sellers_id);
            ViewData["MarKetPlace"] = lst1_loc;

            ViewData["ExportList"] = cf.GetExcelExportList();
            ViewData["ExpenseList"] = cf.GetExpenseList();
            List<SaleReport> lstOrdertext2 = new List<SaleReport>();
            SaleReport view_salereport = null;
            try
            {
                //start code//
                if (txt_from != null && txt_to != null)
                {
                    SalesorderReport obj = new SalesorderReport();
                    if (ddl_market_place == 3)
                    {
                        obj.Get_AmazonNetRealizationWithOutTax(lstOrdertext2, view_salereport, ddl_market_place, txt_from, txt_to, sellers_id);
                    }
                    else if (ddl_market_place == 1)
                    {
                        obj.Get_FlipkartNetRealizationWithOutTax(lstOrdertext2, view_salereport, ddl_market_place, txt_from, txt_to, sellers_id);
                    }
                    else if (ddl_market_place == 5)
                    {
                        obj.Get_PaytmNetRealizationWithOutTax(lstOrdertext2, view_salereport, ddl_market_place, txt_from, txt_to, sellers_id);
                    }
                    /////----------------------------------Export Excel----------------------------------

                    string sendOnMail = form["sendmail"];
                    string value1 = form["command"];
                    if (value1 == "Export" || sendOnMail == "Send On Mail")
                    {
                        if (ddl_export != null)
                        {
                            bool mail = false;
                            if (sendOnMail != null) mail = true;
                            string EmailID = form["txt_email"];

                            DataTable header = new DataTable();
                            string colum = " ";
                            for (int t = 0; t < 6; t++)
                            {
                                header.Columns.Add(colum);
                                colum = colum + " ";
                            }
                            DataRow errow = header.NewRow();
                            errow[2] = " ";
                            header.Rows.Add(errow);
                            DataRow drow = header.NewRow();
                            drow[1] = "From Date";
                            drow[2] = txt_from;
                            drow[4] = "Report Taken On";
                            drow[5] = DateTime.Now.ToString("dd/MMM/yyyy");
                            header.Rows.Add(drow);
                            DataRow brow = header.NewRow();
                            brow[1] = "To Date";
                            brow[2] = txt_to;
                            header.Rows.Add(brow);
                            DataRow erow = header.NewRow();
                            erow[2] = " ";
                            header.Rows.Add(erow);
                            DataTable export_dt = new DataTable();
                            if (ddl_market_place == 3)
                            {
                                export_dt = cf.CreateNetRealizationWithOutTaxDatatable(lstOrdertext2);
                            }
                            else if (ddl_market_place == 1)
                            {
                                export_dt = cf.CreateNetRealizationWithOutTaxFlipkart(lstOrdertext2);
                            }
                            else if (ddl_market_place == 5)
                            {
                                export_dt = cf.CreateNetRealizationWithOutTaxPaytm(lstOrdertext2);
                            }

                            int type = Convert.ToInt32(ddl_export);
                            if (export_dt.Rows.Count > 0)
                            {
                                int r = cf.Export("Net Ralization Report Without Tax", header, export_dt, Response, type, mail, EmailID);
                            }
                            else ViewData["Message"] = "There are No records In Table";
                        }
                        else ViewData["Message"] = "Please Select Export Type";
                    }
                    //----------------------------------------END-----------------------------------------////
                }// end of if(txt_from)
                // end code//
            }// end of try block
            catch (Exception ex)
            {
            }
            if (ddl_market_place == 3)
            {
                return View("AmazonNetRealizationWithouttax", lstOrdertext2);
            }
            else if (ddl_market_place == 1)
            {
                return View("FlipkartNetRealizationWithouttax", lstOrdertext2);
            }
            else if (ddl_market_place == 5)
            {
                return View("PaytmNetRealizationWithouttax", lstOrdertext2);
            }
            else
                return View(lstOrdertext2);
        }
        #endregion

        #region IGST report
        public ActionResult IGST(FormCollection from, int? ddl_marketplace, DateTime? txt_from, DateTime? txt_to)
        {
            txt_from = txt_from != null ? txt_from : DateTime.Parse("2017-11-07");
            txt_to = txt_to != null ? txt_to : DateTime.Parse("2017-11-25");
            cf = new comman_function();
            if (!cf.session_check())
                return RedirectToAction("Login", "Home");
            int sellers_id = Convert.ToInt32(Session["SellerID"]);
            cf = new comman_function();
            List<SelectListItem> lst_loc = cf.GetSrchMarketPalceList(sellers_id);
            ViewData["MarKetPlaceList"] = lst_loc;

            List<SaleReport> lstOrdertext2 = new List<SaleReport>();

            try
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
                var get_marketplace = db.m_marketplace.Where(a => a.isactive == 1).ToList();

                foreach (var item in GetSaleOrderDetail)
                {
                    SaleReport view_salereport = new SaleReport();

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
                    view_salereport.OrderDate = Convert.ToDateTime(item.ob_tbl_sales_order.purchase_date).ToString("yyyy-MM-dd");


                    var getsettlementdetails = dba.m_tbl_expense.Where(a => a.settlement_order_id == item.ob_tbl_sales_order.amazon_order_id && a.tbl_seller_id == sellers_id && a.t_transactionType_id == 1).ToList();
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
                                        if (gettax_details != null)
                                        {
                                            view_salereport.FBACGST = Convert.ToDouble(gettax_details.CGST_amount);
                                            view_salereport.FBASGST = Convert.ToDouble(gettax_details.sgst_amount);
                                        }
                                    }
                                }
                                else if (nam == "Technology Fee")
                                {
                                    if (get_expdetails.id == exp_id)
                                    {
                                        if (gettax_details != null)
                                        {
                                            view_salereport.TechnologyIGST = Convert.ToDouble(gettax_details.Igst_amount);
                                        }
                                    }
                                }
                                else if (nam == "Commission")
                                {
                                    if (get_expdetails.id == exp_id)
                                    {
                                        if (gettax_details != null)
                                        {
                                            view_salereport.CommissionIGST = Convert.ToDouble(gettax_details.Igst_amount);
                                        }
                                    }
                                }
                                else if (nam == "Fixed closing fee")
                                {
                                    if (get_expdetails.id == exp_id)
                                    {
                                        if (gettax_details != null)
                                        {
                                            view_salereport.FixedclosingIGST = Convert.ToDouble(gettax_details.Igst_amount);
                                        }
                                    }
                                }
                                else if (nam == "Shipping Chargeback")
                                {
                                    if (get_expdetails.id == exp_id)
                                    {
                                        if (gettax_details != null)
                                        {
                                            view_salereport.shippingchargeCGST = Convert.ToDouble(gettax_details.CGST_amount);
                                            view_salereport.shippingchargeSGST = Convert.ToDouble(gettax_details.sgst_amount);
                                        }
                                    }
                                }
                                else if (nam == "Shipping discount")
                                {
                                    if (get_expdetails.id == exp_id)
                                    {
                                        if (gettax_details != null)
                                        {
                                            view_salereport.Shippingtaxdiscount = Convert.ToDouble(gettax_details.Igst_amount);
                                        }
                                    }
                                }
                                else if (nam == "Refund commission")
                                {
                                    if (get_expdetails.id == exp_id)
                                    {
                                        if (gettax_details != null)
                                        {
                                            view_salereport.RefundDiscount = Convert.ToDouble(gettax_details.Igst_amount);
                                        }
                                    }
                                }
                            } // end of if(get_expdetails)
                            view_salereport.ExpenseTotal = view_salereport.FBAFEE + view_salereport.TechnologyFee + view_salereport.CommissionFee + view_salereport.FixedClosingFee + view_salereport.ShippingChargebackFee + view_salereport.ShippingDiscountFee + view_salereport.RefundCommision;
                            view_salereport.SumTaxFee = view_salereport.FBACGST + view_salereport.FBASGST + view_salereport.TechnologyIGST + view_salereport.CommissionIGST + view_salereport.FixedclosingIGST + view_salereport.shippingchargeCGST + view_salereport.shippingchargeSGST + view_salereport.Shippingtaxdiscount + view_salereport.RefundDiscount;
                            view_salereport.FullExpenseTotal = view_salereport.ExpenseTotal + view_salereport.SumTaxFee;
                        }// end if foreach(item1)
                    }// end of if(getsettlementdetails)
                    view_salereport.NetTotal = view_salereport.SumOrder + view_salereport.FullExpenseTotal;
                    lstOrdertext2.Add(view_salereport);
                }// end of for each (item)

            }// end of try block
            catch (Exception ex)
            {
            }//end if catch block

            return View(lstOrdertext2);
        }
        #endregion

        #region SalesVoucher
        /// <summary>
        /// this is for Sales Voucher Report
        /// </summary>
        /// <param name="form"></param>
        /// <param name="ddl_MonthSelecter"></param>
        /// <param name="ddl_yearSelecter"></param>
        /// <param name="txt_from"></param>
        /// <param name="txt_to"></param>
        /// <param name="ddl_export"></param>
        /// <returns></returns>
        public ActionResult SalesVoucher(FormCollection form, int? ddl_market_place, int? ddl_MonthSelecter, int? ddl_yearSelecter, DateTime? txt_from, DateTime? txt_to, int? ddl_export)
        {
            cf = new comman_function();
            if (!cf.session_check())
                return RedirectToAction("Login", "Home");
            //DateTime outputDateTimeValue= DateTime.Parse("2009-05-08 14:40:52,531", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None);
            if (ddl_MonthSelecter != null && ddl_MonthSelecter != 0 && ddl_yearSelecter != null && ddl_yearSelecter != 0)
            {
                int days = DateTime.DaysInMonth(Convert.ToInt32(ddl_yearSelecter), Convert.ToInt32(ddl_MonthSelecter));
                string strStartDate = "1/" + ddl_MonthSelecter + "/" + ddl_yearSelecter + " 00:00:00";
                string strEndDate = days + "/" + ddl_MonthSelecter + "/" + ddl_yearSelecter + " 23:59:59";
                txt_from = DateTime.ParseExact(strStartDate, "d/M/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                txt_to = DateTime.ParseExact(strEndDate, "d/M/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
            }
            string MarketPlace = "";
            int sellers_id = Convert.ToInt32(Session["SellerID"]);
            int stateid = Convert.ToInt32(Session["State"]);
            string CompanyName = Convert.ToString(Session["UserName"]);
            cf = new comman_function();
            ViewData["ExportList"] = cf.GetExportList();
            List<SelectListItem> lst1_loc = cf.GetMarketPlace(sellers_id);
            ViewData["MarKetPlace"] = lst1_loc;
            if (ddl_market_place != null)
                MarketPlace = lst1_loc.Where(p => p.Value == ddl_market_place.ToString()).First().Text;

            List<SaleReport> lstOrdertext2 = new List<SaleReport>();
            try
            {
                if (txt_from != null && txt_to != null)
                {
                    SalesVoucher obj = new SalesVoucher();
                    //obj.Get_Sales_Voucher_Report_New(lstOrdertext2, txt_from, txt_to, sellers_id); used when we  want sales tally data 
                    obj.Get_Sales_Excel_Report(lstOrdertext2, txt_from, txt_to, sellers_id, stateid, ddl_market_place, MarketPlace);
                    /////----------------------------------Export Excel----------------------------------
                    string sendOnMail = form["sendmail"];
                    string value1 = form["command"];
                    if (value1 == "Export" || sendOnMail == "Send On Mail")
                    {
                        if (ddl_export != null && ddl_export == 3)
                        {
                            #region XML
                            int message = 0;
                            XmlDocument xmlDoc = new XmlDocument();
                            SalesVoucherXml objxml = new SalesVoucherXml();
                            // var xmldata = objxml.SalesVoucherXML(lstOrdertext2,CompanyName);
                            var xmldata = objxml.SalesVoucherXML(txt_from, txt_to, sellers_id, CompanyName, MarketPlace, ddl_market_place);
                            Response.Buffer = true;
                            Response.Write(xmldata);
                            Response.AddHeader("Content-disposition", "attachment; filename=" + " Sales Tally Voucher" + ".xml");
                            Response.ContentType = "application/octet-stream";
                            Response.End();
                            message = 1;
                            #endregion
                        }
                        else if (ddl_export != null && ddl_export == 1)
                        {
                            bool mail = false;
                            if (sendOnMail != null) mail = true;
                            string EmailID = form["txt_email"];

                            DataTable header = new DataTable();
                            string colum = " ";
                            for (int t = 0; t < 6; t++)
                            {
                                header.Columns.Add(colum);
                                colum = colum + " ";
                            }
                            DataRow errow = header.NewRow();
                            errow[2] = " ";
                            header.Rows.Add(errow);
                            DataRow drow = header.NewRow();
                            drow[1] = "From Date";
                            drow[2] = txt_from;
                            drow[4] = "Report Taken On";
                            drow[5] = DateTime.Now.ToString("dd/MMM/yyyy");
                            header.Rows.Add(drow);
                            DataRow brow = header.NewRow();
                            brow[1] = "To Date";
                            brow[2] = txt_to;
                            header.Rows.Add(brow);
                            DataRow erow = header.NewRow();
                            erow[2] = " ";
                            header.Rows.Add(erow);
                            DataTable export_dt = new DataTable();
                            export_dt = cf.CreateSaleVoucherDatatable(lstOrdertext2);
                            int type = Convert.ToInt32(ddl_export);
                            if (export_dt.Rows.Count > 0)
                            {
                                int r = cf.Export("Sale Voucher Report", header, export_dt, Response, type, mail, EmailID);
                            }
                            else ViewData["Message"] = "There are No records In Table";
                        }
                        else ViewData["Message"] = "Please Select Export Type";
                    }
                    //----------------------------------------END-----------------------------------------////
                }
            }// end of try block
            catch (Exception ex)
            {
            }// end of catch block
            return View(lstOrdertext2);
        }
        #endregion



        /// <summary>
        /// Tally Voucher Report
        /// </summary>
        /// <param name="from"></param>
        /// <param name="ddl_marketplace"></param>
        /// <param name="txt_from"></param>
        /// <param name="txt_to"></param>
        /// <returns></returns>
        #region TallyVoucher report
        public JsonResult GetReferenceNumber(int? CatID, int? marketplaceID)
        {

            int sellers_id = Convert.ToInt32(Session["SellerID"]);
            cf = new comman_function();
            List<SelectListItem> lst_subcat = cf.GetSrchReferenceList(CatID, sellers_id, marketplaceID);
            //var categories = dba.tbl_settlement_upload.Where(a => a.settlement_type == CatID && a.tbl_seller_id == sellers_id && a.status==0).ToList();
            return Json(lst_subcat, JsonRequestBehavior.AllowGet);
        }

        string voucher_number = "";

        public ActionResult TallyVoucher(FormCollection form, int? ddl_market_place, int? ddl_eftcod, int? ddl_reference, DateTime? txt_from, DateTime? txt_to, int? ddl_export)
        {
            cf = new comman_function();
            if (!cf.session_check())
                return RedirectToAction("Login", "Home");

            int sellers_id = Convert.ToInt32(Session["SellerID"]);
            string CompanyName = Convert.ToString(Session["UserName"]);
            cf = new comman_function();
            ViewData["ExportList"] = cf.GetExportList();
            ViewData["EFTCOD"] = cf.GetSearchEFTCODList();
            List<SelectListItem> lst1_loc = cf.GetMarketPlace(sellers_id);
            ViewData["MarKetPlace"] = lst1_loc;
            List<SelectListItem> lst_subcat = cf.GetSrchReferenceList(ddl_eftcod, sellers_id, ddl_market_place);
            ViewData["ReferenceNumber"] = lst_subcat;
            string MarketPlace = "";
            if (ddl_market_place != null)
                MarketPlace = lst1_loc.Where(p => p.Value == ddl_market_place.ToString()).First().Text;

            MainTallySettlementVoucher main_TallyObj = new MainTallySettlementVoucher();
            string settlement_deposit_date = "";
            string settlement_reference = "";
            voucher_number = MarketPlace + "/Settlement/";

            try
            {
                dba = new SellerContext();
                dba.Configuration.AutoDetectChangesEnabled = false;
                dba.Configuration.ProxyCreationEnabled = false;
                // dba.tbl_settlement_order.AsNoTracking();
                if (ddl_reference != null && ddl_reference != 0)
                {
                    var GetReferenceDetail = (from ob_tbl_sales_order in dba.tbl_settlement_upload
                                              join ob_bank in dba.tbl_imp_bank_transfers on ob_tbl_sales_order.Id
                                                 equals ob_bank.tbl_settlement_upload_id
                                              select new
                                              {
                                                  ob_tbl_sales_order = ob_tbl_sales_order,
                                                  ob_bank = ob_bank
                                              }).Where(a => a.ob_tbl_sales_order.tbl_seller_id == sellers_id && a.ob_tbl_sales_order.market_place_id == ddl_market_place).ToList();



                    if (GetReferenceDetail != null && ddl_reference != null && ddl_reference != 0)
                    {
                        GetReferenceDetail = GetReferenceDetail.Where(a => a.ob_tbl_sales_order.Id == ddl_reference).ToList();
                    }


                    //assign the value here

                    foreach (var item in GetReferenceDetail)
                    {
                        voucher_number = voucher_number + item.ob_tbl_sales_order.voucher_running_no;

                        settlement_deposit_date = Convert.ToDateTime(item.ob_tbl_sales_order.deposit_date).ToString("yyyy-MM-dd");
                        main_TallyObj.voucher_date = settlement_deposit_date;
                        settlement_reference = item.ob_tbl_sales_order.settlement_refernece_no;


                        main_TallyObj.debitType_bank_balance = new TallyDebitCredit();
                        main_TallyObj.debitType_bank_balance.debit_amt = Convert.ToDouble(item.ob_bank.amount);
                        main_TallyObj.debitType_bank_balance.voucher_date = settlement_deposit_date; // Convert.ToDateTime(item.ob_tbl_sales_order.settlement_from).ToString("yyyy-MM-dd");
                        main_TallyObj.debitType_bank_balance.voucher_number = voucher_number;//"Amazon November 2017 - V 01";
                        main_TallyObj.debitType_bank_balance.ledger_name = MarketPlace + "-Receipt";

                        main_TallyObj.creditType_Previous_Reserve_Amount_Balance = new TallyDebitCredit();
                        main_TallyObj.creditType_Previous_Reserve_Amount_Balance.credit_amt = Convert.ToDouble(item.ob_tbl_sales_order.previous_reserve_amount);
                        main_TallyObj.creditType_Previous_Reserve_Amount_Balance.voucher_date = settlement_deposit_date; // Convert.ToDateTime(item.ob_tbl_sales_order.settlement_from).ToString("yyyy-MM-dd");
                        main_TallyObj.creditType_Previous_Reserve_Amount_Balance.voucher_number = voucher_number; //"Amazon November 2017 - V 01";
                        main_TallyObj.creditType_Previous_Reserve_Amount_Balance.ledger_name = MarketPlace + "-Previous Reserve Amount Balance";

                        /////////////////
                        main_TallyObj.debitType_Current_Reserve_Amount = new TallyDebitCredit();
                        main_TallyObj.debitType_Current_Reserve_Amount.debit_amt = Convert.ToDouble(item.ob_tbl_sales_order.current_reserve_amount) * (-1);
                        main_TallyObj.debitType_Current_Reserve_Amount.voucher_date = settlement_deposit_date; // Convert.ToDateTime(item.ob_tbl_sales_order.settlement_from).ToString("yyyy-MM-dd");
                        main_TallyObj.debitType_Current_Reserve_Amount.voucher_number = voucher_number; //"Amazon November 2017 - V 01";
                        main_TallyObj.debitType_Current_Reserve_Amount.ledger_name = MarketPlace + "-Current Reserve Amount";

                        ///////////////////////
                        main_TallyObj.creditType_Nonsubscription_feeadj = new TallyDebitCredit();
                        main_TallyObj.creditType_Nonsubscription_feeadj.credit_amt = Convert.ToDouble(item.ob_tbl_sales_order.Nonsubscription_feeadj);
                        main_TallyObj.creditType_Nonsubscription_feeadj.voucher_date = settlement_deposit_date; // Convert.ToDateTime(item.ob_tbl_sales_order.settlement_from).ToString("yyyy-MM-dd");
                        main_TallyObj.creditType_Nonsubscription_feeadj.voucher_number = voucher_number; //"Amazon November 2017 - V 01";
                        main_TallyObj.creditType_Nonsubscription_feeadj.ledger_name = MarketPlace + "-Nonsubscription_feeadj";

                        main_TallyObj.creditType_incorrectItemFees = new TallyDebitCredit();
                        main_TallyObj.creditType_incorrectItemFees.credit_amt = Convert.ToDouble(item.ob_tbl_sales_order.INCORRECT_FEES_ITEMS);
                        main_TallyObj.creditType_incorrectItemFees.voucher_date = settlement_deposit_date; // Convert.ToDateTime(item.ob_tbl_sales_order.settlement_from).ToString("yyyy-MM-dd");
                        main_TallyObj.creditType_incorrectItemFees.voucher_number = voucher_number; //"Amazon November 2017 - V 01";
                        main_TallyObj.creditType_incorrectItemFees.ledger_name = MarketPlace + "-Incorrect Item Fees";


                        main_TallyObj.debitType_INCORRECT_FEES_ITEMS = new TallyDebitCredit();
                        main_TallyObj.debitType_INCORRECT_FEES_ITEMS.debit_amt = Convert.ToDouble(item.ob_tbl_sales_order.Cost_of_Advertising) * (-1);
                        main_TallyObj.debitType_INCORRECT_FEES_ITEMS.voucher_date = settlement_deposit_date; // Convert.ToDateTime(item.ob_tbl_sales_order.settlement_from).ToString("yyyy-MM-dd");
                        main_TallyObj.debitType_INCORRECT_FEES_ITEMS.voucher_number = voucher_number;// "Amazon November 2017 - V 01";
                        main_TallyObj.debitType_INCORRECT_FEES_ITEMS.ledger_name = MarketPlace + "-Cost of Advertising";

                        //add others also
                        main_TallyObj.debitType_StorageFee = new TallyDebitCredit();
                        main_TallyObj.debitType_StorageFee.debit_amt = Convert.ToDouble(item.ob_tbl_sales_order.Storage_Fee) * (-1);
                        main_TallyObj.debitType_StorageFee.voucher_date = settlement_deposit_date;
                        main_TallyObj.debitType_StorageFee.voucher_number = voucher_number;// "Amazon November 2017 - V 01";
                        main_TallyObj.debitType_StorageFee.ledger_name = MarketPlace + "-Storage Fee";

                        //add vineet
                        main_TallyObj.creditType_BalanceAdjustment = new TallyDebitCredit();
                        main_TallyObj.creditType_BalanceAdjustment.credit_amt = Convert.ToDouble(item.ob_tbl_sales_order.BalanceAdjustment);
                        main_TallyObj.creditType_BalanceAdjustment.voucher_date = settlement_deposit_date;
                        main_TallyObj.creditType_BalanceAdjustment.voucher_number = voucher_number;// "Amazon November 2017 - V 01";
                        main_TallyObj.creditType_BalanceAdjustment.ledger_name = MarketPlace + "-Balance Adjustment";

                        main_TallyObj.debitType_FBAInboundTransportationFee = new TallyDebitCredit();
                        main_TallyObj.debitType_FBAInboundTransportationFee.debit_amt = Convert.ToDouble(item.ob_tbl_sales_order.FBAInboundTransportationFee) * (-1);
                        main_TallyObj.debitType_FBAInboundTransportationFee.voucher_date = settlement_deposit_date; // Convert.ToDateTime(item.ob_tbl_sales_order.settlement_from).ToString("yyyy-MM-dd");
                        main_TallyObj.debitType_FBAInboundTransportationFee.voucher_number = voucher_number;// "Amazon November 2017 - V 01";
                        main_TallyObj.debitType_FBAInboundTransportationFee.ledger_name = MarketPlace + "-FBA Inbound Transportation Fee";

                        //sharad91
                        main_TallyObj.debitType_Payable_to_Amazon = new TallyDebitCredit();
                        main_TallyObj.debitType_Payable_to_Amazon.debit_amt = Convert.ToDouble(item.ob_tbl_sales_order.Payable_to_Amazon) * (-1);
                        main_TallyObj.debitType_Payable_to_Amazon.voucher_date = settlement_deposit_date; // Convert.ToDateTime(item.ob_tbl_sales_order.settlement_from).ToString("yyyy-MM-dd");
                        main_TallyObj.debitType_Payable_to_Amazon.voucher_number = voucher_number;// "Amazon November 2017 - V 01";
                        main_TallyObj.debitType_Payable_to_Amazon.ledger_name = MarketPlace + "-Payable to Amazon";

                        if (item.ob_tbl_sales_order.suspense_amt != 0 || item.ob_tbl_sales_order.suspense_amt != null)
                        {
                            main_TallyObj.suspenseEntry = new TallyDebitCredit();
                            if (item.ob_tbl_sales_order.suspense_amt < 0)
                                main_TallyObj.suspenseEntry.debit_amt = Convert.ToDouble(item.ob_tbl_sales_order.suspense_amt) * (-1);
                            else
                            {
                                main_TallyObj.suspenseEntry.credit_amt = Convert.ToDouble(item.ob_tbl_sales_order.suspense_amt);
                            }
                            main_TallyObj.suspenseEntry.voucher_date = settlement_deposit_date;
                            main_TallyObj.suspenseEntry.voucher_number = voucher_number;
                            main_TallyObj.suspenseEntry.ledger_name = MarketPlace + "-Suspense";
                        }


                        var get_settlementtaxdata = dba.tbl_tax.Where(a => a.tbl_seller_id == sellers_id && a.reference_type == 10 && a.tbl_referneced_id == item.ob_tbl_sales_order.Id).AsNoTracking().ToList();
                        if (get_settlementtaxdata != null)
                        {
                            int i = 0;
                            foreach (var tax1 in get_settlementtaxdata)
                            {
                                if (tax1.CGST_amount != 0)
                                {
                                    main_TallyObj.debittype_StorageFeeCGST = new TallyDebitCredit();
                                    main_TallyObj.debittype_StorageFeeCGST.debit_amt = Convert.ToDouble(tax1.CGST_amount) * (-1);
                                    main_TallyObj.debittype_StorageFeeCGST.voucher_date = settlement_deposit_date;
                                    main_TallyObj.debitType_StorageFee.voucher_number = voucher_number;//"";
                                    main_TallyObj.debittype_StorageFeeCGST.ledger_name = MarketPlace + "-Storage Billing CGST";
                                }
                                if (tax1.sgst_amount != 0)
                                {
                                    main_TallyObj.debittype_StorageFeeSGST = new TallyDebitCredit();
                                    main_TallyObj.debittype_StorageFeeSGST.debit_amt = Convert.ToDouble(tax1.sgst_amount) * (-1);
                                    main_TallyObj.debittype_StorageFeeSGST.voucher_date = settlement_deposit_date;
                                    main_TallyObj.debittype_StorageFeeSGST.voucher_number = voucher_number;// "";
                                    main_TallyObj.debittype_StorageFeeSGST.ledger_name = MarketPlace + "-Storage Billing SGST";
                                }


                            }//end for
                        }
                    }

                    main_TallyObj.tallyOrderDict = new Dictionary<string, TallyOrder>();
                    #region setlement order

                    //////////////////////////////////////////////
                    using (var connection = dba.Database.Connection)
                    {
                        connection.Open();
                        var command = connection.CreateCommand();
                        command.CommandText = "get_sett_voucher"; // "get_sett_orders";
                        command.CommandType = CommandType.StoredProcedure;

                        MySql.Data.MySqlClient.MySqlParameter param, param1;
                        param = new MySql.Data.MySqlClient.MySqlParameter("@ref", settlement_reference);
                        param.Direction = ParameterDirection.Input;
                        param.DbType = DbType.String;
                        command.Parameters.Add(param);

                        param1 = new MySql.Data.MySqlClient.MySqlParameter("@sellerid", sellers_id);
                        param1.Direction = ParameterDirection.Input;
                        param1.DbType = DbType.Int32;
                        command.Parameters.Add(param1);

                        /* System.Data.Common.DbParameter param = command.CreateParameter();
                          System.Data.Common.DbParameter param1 = command.CreateParameter();
                         param1.DbType = DbType.Int32;
                         param1.Direction = ParameterDirection.Input;
                         param1.Value = sellers_id;
                         param1.ParameterName = "@sellerid";
                         command.Parameters.Add(param1);

                         param.DbType = DbType.String;
                         param.Direction = ParameterDirection.Input;
                         param.Value = settlement_reference;
                         param1.ParameterName = "@ref";
                         command.Parameters.Add(param);*/


                        using (var reader = command.ExecuteReader())
                        {
                            var get_settlement_data =
                                ((System.Data.Entity.Infrastructure.IObjectContextAdapter)dba)
                                    .ObjectContext
                                    .Translate<tbl_Settlement_voucher>(reader)
                                    .ToList();

                            reader.NextResult();

                            var get_settlement_data1 =
                                ((System.Data.Entity.Infrastructure.IObjectContextAdapter)dba)
                                    .ObjectContext
                                    .Translate<tbl_Settlement_voucher>(reader)
                                    .ToList();

                            foreach (var sett5 in get_settlement_data1)
                            {
                                get_settlement_data.Add(sett5);
                            }

                            reader.NextResult();

                            //var get_settlement_data2 =
                            //    ((System.Data.Entity.Infrastructure.IObjectContextAdapter)dba)
                            //        .ObjectContext
                            //        .Translate<tbl_Settlement_voucher>(reader)
                            //        .ToList();

                            //foreach (var sett5 in get_settlement_data2)
                            //{
                            //    get_settlement_data.Add(sett5);
                            //}

                            //var expnseRecords =
                            //    ((IObjectContextAdapter)dba)
                            //        .ObjectContext
                            //        .Translate<m_tbl_expense>(reader)
                            //        .Where(a => a.tbl_seller_id == sellers_id && a.expense_type_id != 6 && a.expense_type_id != 8 && a.expense_type_id != 9 && a.expense_type_id != 10 && a.expense_type_id != 11  && a.t_transactionType_id == 1 && a.reference_number == settlement_reference)
                            //        .ToList();

                            //connection.Close();

                            if (get_settlement_data != null)
                            {
                                //int maincounter = 0;
                                for (int maincounter = 0; maincounter < get_settlement_data.Count; maincounter++)
                                //foreach (var sett in get_settlement_data)
                                {
                                    var sett = get_settlement_data[maincounter];
                                    if (sett.Order_Id == "4900888057")
                                    {
                                    }

                                    if (sett.Order_Id == "171-7616497-6675534")
                                    {
                                    }
                                    double orderprincipal = Convert.ToDouble(sett.principal_price);
                                    double orderproduct_tax = Convert.ToDouble(sett.product_tax);
                                    double ordershipping = Convert.ToDouble(sett.shipping_price);
                                    double ordershipping_tax = Convert.ToDouble(sett.shipping_tax);
                                    double ordergiftwrap = Convert.ToDouble(sett.giftwrap_price);
                                    double ordergiftwrap_tax = Convert.ToDouble(sett.giftwarp_tax);

                                    double shippingdiscount = Convert.ToDouble(sett.shipping_discount);
                                    double shippingtaxdiscount = Convert.ToDouble(sett.shipping_tax_discount);

                                    double orderTotal = orderprincipal + orderproduct_tax + ordershipping + ordershipping_tax + ordergiftwrap + ordergiftwrap_tax + shippingdiscount + shippingtaxdiscount;
                                    string uniqueOrderId = sett.unique_order_id;
                                    string origOrderid = sett.Order_Id;

                                    TallyOrder tallyOrderObj = null;
                                    TallyOrder tallyOrderObj1 = null;

                                    if (sett.INCORRECT_FEES_ITEMS != 0 && sett.INCORRECT_FEES_ITEMS != null)
                                    {

                                        CreateTallyOrderObject(ref tallyOrderObj1, ref main_TallyObj, sett.Order_Id, settlement_deposit_date, sett.unique_order_id, MarketPlace);
                                        tallyOrderObj1.voucherEntry.credit_amt = (double)sett.INCORRECT_FEES_ITEMS;
                                        tallyOrderObj1.voucherEntry.ledger_name = MarketPlace + "-INCORRECT_FEES_ITEMS";
                                    }

                                    if (sett.SAFE_T_Reimbursement != 0 && sett.SAFE_T_Reimbursement != null)
                                    {

                                        CreateTallyOrderObject(ref tallyOrderObj1, ref main_TallyObj, sett.Order_Id, settlement_deposit_date, sett.unique_order_id, MarketPlace);
                                        tallyOrderObj1.voucherEntry.credit_amt = (double)sett.SAFE_T_Reimbursement;
                                        tallyOrderObj1.voucherEntry.ledger_name = MarketPlace + "-SAFE_T_Reimbursement";
                                    }

                                    if (orderprincipal != 0)
                                    {
                                        CreateTallyOrderObject(ref tallyOrderObj, ref main_TallyObj, sett.Order_Id, settlement_deposit_date, sett.unique_order_id, MarketPlace);
                                        tallyOrderObj.voucherEntry.credit_amt = orderTotal;
                                    }

                                    //added for cancellation
                                    if (tallyOrderObj == null && tallyOrderObj1 == null)
                                    {
                                        CreateTallyOrderObject(ref tallyOrderObj, ref main_TallyObj, sett.Order_Id, settlement_deposit_date, sett.unique_order_id, MarketPlace);
                                    }

                                    //tallyOrderObj.voucherEntry = new TallyDebitCredit();

                                    //if (sett.Order_Id == "405-1404269-7210700")
                                    //{
                                    //}

                                    //run a for loop for expenses (only for orders )
                                    //var expnseRecords = dba.m_tbl_expense.Where(a => a.tbl_seller_id == sellers_id && a.expense_type_id != 6 && a.expense_type_id != 8 && a.expense_type_id != 9 && a.expense_type_id != 10 && a.expense_type_id != 11 && a.settlement_order_id == uniqueOrderId && a.t_transactionType_id == 1 && a.reference_number == settlement_reference).AsNoTracking().ToList();


                                    if (tallyOrderObj != null)
                                    {
                                        //HandleExpense(2, expnseRecords, ref tallyOrderObj, settlement_deposit_date, origOrderid);
                                        if (sett.expense_type_id != null)
                                            NewHandleExpense(2, ref maincounter, ref get_settlement_data, ref tallyOrderObj, settlement_deposit_date, origOrderid, MarketPlace);

                                    }
                                    else if (tallyOrderObj1 != null)
                                    {
                                        //HandleExpense(2, expnseRecords, ref tallyOrderObj1, settlement_deposit_date, origOrderid);
                                        if (sett.expense_type_id != null)
                                            NewHandleExpense(2, ref maincounter, ref get_settlement_data, ref tallyOrderObj, settlement_deposit_date, origOrderid, MarketPlace);

                                    }
                                    else
                                    {
                                        int rt = 0;
                                        //should not happen
                                    }
                                    //sharad

                                    ///////////sharad/////////////

                                }//end for loop - settlement orders
                            }
                        }//end using


                        //////////////////////////////////////////////

                        //var get_settlement_data = dba.tbl_settlement_order.Where(a => a.tbl_seller_id == sellers_id && a.settlement_id == settlement_reference).AsNoTracking().ToList();
                        //if (get_settlement_data != null)
                        //{
                        //    foreach (var sett in get_settlement_data)
                        //    {
                        //        double orderprincipal = Convert.ToDouble(sett.principal_price);
                        //        double orderproduct_tax = Convert.ToDouble(sett.product_tax);
                        //        double ordershipping = Convert.ToDouble(sett.shipping_price);
                        //        double ordershipping_tax = Convert.ToDouble(sett.shipping_tax);
                        //        double ordergiftwrap = Convert.ToDouble(sett.giftwrap_price);
                        //        double ordergiftwrap_tax = Convert.ToDouble(sett.giftwarp_tax);

                        //        double shippingdiscount = Convert.ToDouble(sett.shipping_discount);
                        //        double shippingtaxdiscount = Convert.ToDouble(sett.shipping_tax_discount);

                        //        double orderTotal = orderprincipal + orderproduct_tax + ordershipping + ordershipping_tax + ordergiftwrap + ordergiftwrap_tax + shippingdiscount + shippingtaxdiscount;
                        //        string uniqueOrderId = sett.unique_order_id;
                        //        string origOrderid = sett.Order_Id;

                        //        TallyOrder tallyOrderObj = null;
                        //        TallyOrder tallyOrderObj1 = null;

                        //        if (sett.INCORRECT_FEES_ITEMS != 0 && sett.INCORRECT_FEES_ITEMS != null)
                        //        {

                        //            CreateTallyOrderObject(ref tallyOrderObj1, ref main_TallyObj, sett.Order_Id, settlement_deposit_date, sett.unique_order_id);
                        //            tallyOrderObj1.voucherEntry.credit_amt = (double)sett.INCORRECT_FEES_ITEMS;
                        //            tallyOrderObj1.voucherEntry.ledger_name = "Amazon-INCORRECT_FEES_ITEMS";
                        //        }

                        //        if (sett.SAFE_T_Reimbursement != 0 && sett.SAFE_T_Reimbursement != null)
                        //        {

                        //            CreateTallyOrderObject(ref tallyOrderObj1, ref main_TallyObj, sett.Order_Id, settlement_deposit_date, sett.unique_order_id);
                        //            tallyOrderObj1.voucherEntry.credit_amt = (double)sett.SAFE_T_Reimbursement;
                        //            tallyOrderObj1.voucherEntry.ledger_name = "Amazon-SAFE_T_Reimbursement";
                        //        }

                        //        if (orderprincipal != 0)
                        //        {
                        //            CreateTallyOrderObject(ref tallyOrderObj, ref main_TallyObj, sett.Order_Id, settlement_deposit_date, sett.unique_order_id);
                        //            tallyOrderObj.voucherEntry.credit_amt = orderTotal;
                        //        }

                        //        //added for cancellation
                        //        if (tallyOrderObj == null && tallyOrderObj1 == null)
                        //        {
                        //            CreateTallyOrderObject(ref tallyOrderObj, ref main_TallyObj, sett.Order_Id, settlement_deposit_date, sett.unique_order_id);
                        //        }

                        //        //tallyOrderObj.voucherEntry = new TallyDebitCredit();

                        //        if (sett.Order_Id == "405-1404269-7210700")
                        //        {

                        //        }
                        //        //run a for loop for expenses (only for orders )
                        //        var expnseRecords = dba.m_tbl_expense.Where(a => a.tbl_seller_id == sellers_id && a.expense_type_id != 6 && a.expense_type_id != 8 && a.expense_type_id != 9 && a.expense_type_id != 10 && a.expense_type_id != 11 && a.settlement_order_id == uniqueOrderId && a.t_transactionType_id == 1 && a.reference_number == settlement_reference).AsNoTracking().ToList();

                        //        //sharad
                        //        if (tallyOrderObj != null)
                        //            HandleExpense(2, expnseRecords, ref tallyOrderObj, settlement_deposit_date, origOrderid);
                        //        else if (tallyOrderObj1 != null)
                        //            HandleExpense(2, expnseRecords, ref tallyOrderObj1, settlement_deposit_date, origOrderid);
                        //        else
                        //        {
                        //            int rt = 0;
                        //            //should not happen
                        //        }
                        //        ///////////sharad/////////////

                        //    }//end for loop - settlement orders
                        #endregion
                        connection.Close();
                        dba.Dispose();

                        dba = new SellerContext();
                        dba.Configuration.AutoDetectChangesEnabled = false;

                        //----------get all refund order from history table------------
                        var get_historydata = dba.tbl_order_history.Where(a => a.tbl_seller_id == sellers_id && a.settlement_id == settlement_reference && a.t_order_status == 9).AsNoTracking().ToList();
                        if (get_historydata != null)
                        {
                            foreach (var history in get_historydata)
                            {
                                string uoid = history.unique_order_id;
                                TallyOrder tobj = null;
                                if (history.OrderID == "5011095265")
                                {

                                }

                                if (main_TallyObj.tallyOrderDict.ContainsKey(uoid))
                                {
                                    tobj = main_TallyObj.tallyOrderDict[uoid];
                                }
                                else
                                {
                                    CreateTallyOrderObject(ref tobj, ref main_TallyObj, history.OrderID, settlement_deposit_date, history.unique_order_id, MarketPlace);
                                }

                                if (history.OrderID == "402-4820983-5601120")
                                {

                                }
                                double refundprincipal = Convert.ToDouble(history.amount_per_unit);
                                double refundshipping = Convert.ToDouble(history.shipping_price);
                                double refundgiftwrap = Convert.ToDouble(history.Giftwrap_price);
                                double refundshippingdiscount1 = Convert.ToDouble(history.shipping_discount);
                                double refundshippingtaxdiscount1 = Convert.ToDouble(history.shipping_tax_discount);
                                double refundproduct_tax = Convert.ToDouble(history.product_tax);
                                double refundshipping_tax = Convert.ToDouble(history.shipping_tax);
                                double refundgiftwrap_tax = Convert.ToDouble(history.gift_wrap_tax);

                                double refundtotal = refundprincipal + refundshipping + refundgiftwrap + refundshippingdiscount1 + refundshippingtaxdiscount1 + refundproduct_tax + refundshipping_tax + refundgiftwrap_tax;

                                if (refundtotal < 0)
                                    refundtotal = refundtotal * (-1);

                                tobj.voucherEntry.debit_amt = refundtotal;

                                string origOrderid = history.OrderID;


                                if (history.SAFE_T_Reimbursement != 0 && history.SAFE_T_Reimbursement != null)
                                {
                                    TallyOrder tallyOrderObj1 = null;
                                    CreateTallyOrderObject(ref tallyOrderObj1, ref main_TallyObj, history.OrderID, settlement_deposit_date, history.unique_order_id + "-0", MarketPlace);
                                    tallyOrderObj1.voucherEntry.credit_amt = (double)history.SAFE_T_Reimbursement;
                                    tallyOrderObj1.voucherEntry.ledger_name = MarketPlace + "-SAFE-T Reimbursement";
                                }

                                if (history.Protection_fund_flipkart != 0 && history.Protection_fund_flipkart != null)//vineet  add for flipkart
                                {
                                    TallyOrder tallyOrderObj1 = null;
                                    CreateTallyOrderObject(ref tallyOrderObj1, ref main_TallyObj, history.OrderID, settlement_deposit_date, history.unique_order_id + "-0", MarketPlace);
                                    tallyOrderObj1.voucherEntry.credit_amt = (double)history.Protection_fund_flipkart;
                                    tallyOrderObj1.voucherEntry.ledger_name = MarketPlace + "-Protection-Fund";
                                }
                                var returnexpnseRecords = dba.m_tbl_expense.Where(a => a.tbl_seller_id == sellers_id && a.expense_type_id != 6 && a.expense_type_id != 8 && a.expense_type_id != 9 && a.expense_type_id != 10 && a.expense_type_id != 11 && a.reference_number == settlement_reference && a.t_transactionType_id == 2 && a.tbl_order_historyid == history.Id).AsNoTracking().ToList();

                                HandleExpense(7, returnexpnseRecords, ref tobj, settlement_deposit_date, origOrderid, MarketPlace);
                            }//end for loop for all order history data
                        }

                    }
                }

            }//end try
            catch (Exception e)
            {

            }

            /////----------------------------------Export Excel----------------------------------

            List<SaleReport> lstOrdertext2 = GenerateViewFortallySettlementVoucher(settlement_deposit_date, settlement_reference, main_TallyObj, MarketPlace);

            string sendOnMail = form["sendmail"];
            string value = form["command"];
            if (value == "Export" || sendOnMail == "Send On Mail")
            {
                if (ddl_export != null && ddl_export == 3)
                {
                    #region XML
                    int message = 0;
                    XmlDocument xmlDoc = new XmlDocument();
                    TallyVoucherXml objxml = new TallyVoucherXml();
                    var xmldata = objxml.TallySettlementXML(lstOrdertext2, settlement_deposit_date, settlement_reference, CompanyName, sellers_id, MarketPlace);
                    Response.Buffer = true;
                    Response.Write(xmldata);
                    Response.AddHeader("Content-disposition", "attachment; filename=" + "Tally Voucher" + ".xml");
                    Response.ContentType = "application/octet-stream";
                    Response.End();
                    message = 1;
                    #endregion
                }
                else if (ddl_export != null && ddl_export == 1)
                {
                    bool mail = false;
                    if (sendOnMail != null) mail = true;
                    string EmailID = form["txt_email"];
                    DataTable header = new DataTable();
                    DataTable export_dt = new DataTable();
                    export_dt = cf.CreateTallyVoucherDatatable(lstOrdertext2);
                    int type = Convert.ToInt32(ddl_export);
                    if (export_dt.Rows.Count > 0)
                    {
                        //sharad 9 jan - changed file name
                        //int r = cf.Export("Tally Voucher Report", header, export_dt, Response, type, mail, EmailID);
                        string fname = MarketPlace + "_Settlement_" + settlement_reference + "_" + settlement_deposit_date;
                        int r = cf.Export(fname, header, export_dt, Response, type, mail, EmailID);
                    }
                    else ViewData["Message"] = "There are No records In Table";
                }

                else ViewData["Message"] = "Please Select Export Type";

            }



            //----------------------------------------END-----------------------------------------////

            return View(lstOrdertext2);
        }
        public ActionResult TallyVoucher1(FormCollection form, int? ddl_market_place, int? ddl_eftcod, int? ddl_reference, DateTime? txt_from, DateTime? txt_to, int? ddl_export)
        {
            cf = new comman_function();
            if (!cf.session_check())
                return RedirectToAction("Login", "Home");

            int sellers_id = Convert.ToInt32(Session["SellerID"]);
            string CompanyName = Convert.ToString(Session["UserName"]);
            cf = new comman_function();
            ViewData["ExportList"] = cf.GetExportList();
            ViewData["EFTCOD"] = cf.GetSearchEFTCODList();
            List<SelectListItem> lst1_loc = cf.GetMarketPlace(sellers_id);
            ViewData["MarKetPlace"] = lst1_loc;
            List<SelectListItem> lst_subcat = cf.GetSrchReferenceList(ddl_eftcod, sellers_id, ddl_market_place);
            ViewData["ReferenceNumber"] = lst_subcat;
            string MarketPlace = "";
            if (ddl_market_place != null)
                MarketPlace = lst1_loc.Where(p => p.Value == ddl_market_place.ToString()).First().Text;

            MainTallySettlementVoucher main_TallyObj = new MainTallySettlementVoucher();
            string settlement_deposit_date = "";
            string settlement_reference = "";
            voucher_number = MarketPlace + "/Settlement/";

            try
            {
                dba = new SellerContext();
                dba.Configuration.AutoDetectChangesEnabled = false;
                // dba.tbl_settlement_order.AsNoTracking();
                if (ddl_reference != null && ddl_reference != 0)
                {
                    var GetReferenceDetail = (from ob_tbl_sales_order in dba.tbl_settlement_upload
                                              join ob_bank in dba.tbl_imp_bank_transfers on ob_tbl_sales_order.Id
                                                 equals ob_bank.tbl_settlement_upload_id
                                              select new
                                              {
                                                  ob_tbl_sales_order = ob_tbl_sales_order,
                                                  ob_bank = ob_bank
                                              }).Where(a => a.ob_tbl_sales_order.tbl_seller_id == sellers_id).ToList();



                    if (GetReferenceDetail != null && ddl_reference != null && ddl_reference != 0)
                    {
                        GetReferenceDetail = GetReferenceDetail.Where(a => a.ob_tbl_sales_order.Id == ddl_reference).ToList();
                    }


                    //assign the value here

                    foreach (var item in GetReferenceDetail)
                    {
                        voucher_number = voucher_number + item.ob_tbl_sales_order.voucher_running_no;

                        settlement_deposit_date = Convert.ToDateTime(item.ob_tbl_sales_order.deposit_date).ToString("yyyy-MM-dd");
                        main_TallyObj.voucher_date = settlement_deposit_date;
                        settlement_reference = item.ob_tbl_sales_order.settlement_refernece_no;

                        main_TallyObj.debitType_bank_balance = new TallyDebitCredit();
                        main_TallyObj.debitType_bank_balance.debit_amt = Convert.ToDouble(item.ob_bank.amount);
                        main_TallyObj.debitType_bank_balance.voucher_date = settlement_deposit_date; // Convert.ToDateTime(item.ob_tbl_sales_order.settlement_from).ToString("yyyy-MM-dd");
                        main_TallyObj.debitType_bank_balance.voucher_number = voucher_number;//"Amazon November 2017 - V 01";
                        main_TallyObj.debitType_bank_balance.ledger_name = "Amazon-Receipt-ET";

                        main_TallyObj.creditType_Previous_Reserve_Amount_Balance = new TallyDebitCredit();
                        main_TallyObj.creditType_Previous_Reserve_Amount_Balance.credit_amt = Convert.ToDouble(item.ob_tbl_sales_order.previous_reserve_amount);
                        main_TallyObj.creditType_Previous_Reserve_Amount_Balance.voucher_date = settlement_deposit_date; // Convert.ToDateTime(item.ob_tbl_sales_order.settlement_from).ToString("yyyy-MM-dd");
                        main_TallyObj.creditType_Previous_Reserve_Amount_Balance.voucher_number = voucher_number; //"Amazon November 2017 - V 01";
                        main_TallyObj.creditType_Previous_Reserve_Amount_Balance.ledger_name = "Amazon-Previous Reserve Amount Balance";

                        /////////////////
                        main_TallyObj.debitType_Current_Reserve_Amount = new TallyDebitCredit();
                        main_TallyObj.debitType_Current_Reserve_Amount.debit_amt = Convert.ToDouble(item.ob_tbl_sales_order.current_reserve_amount) * (-1);
                        main_TallyObj.debitType_Current_Reserve_Amount.voucher_date = settlement_deposit_date; // Convert.ToDateTime(item.ob_tbl_sales_order.settlement_from).ToString("yyyy-MM-dd");
                        main_TallyObj.debitType_Current_Reserve_Amount.voucher_number = voucher_number; //"Amazon November 2017 - V 01";
                        main_TallyObj.debitType_Current_Reserve_Amount.ledger_name = "Amazon-Current Reserve Amount";

                        ///////////////////////
                        main_TallyObj.creditType_Nonsubscription_feeadj = new TallyDebitCredit();
                        main_TallyObj.creditType_Nonsubscription_feeadj.credit_amt = Convert.ToDouble(item.ob_tbl_sales_order.Nonsubscription_feeadj);
                        main_TallyObj.creditType_Nonsubscription_feeadj.voucher_date = settlement_deposit_date; // Convert.ToDateTime(item.ob_tbl_sales_order.settlement_from).ToString("yyyy-MM-dd");
                        main_TallyObj.creditType_Nonsubscription_feeadj.voucher_number = voucher_number; //"Amazon November 2017 - V 01";
                        main_TallyObj.creditType_Nonsubscription_feeadj.ledger_name = "Amazon-Nonsubscription_feeadj";

                        main_TallyObj.creditType_incorrectItemFees = new TallyDebitCredit();
                        main_TallyObj.creditType_incorrectItemFees.credit_amt = Convert.ToDouble(item.ob_tbl_sales_order.INCORRECT_FEES_ITEMS);
                        main_TallyObj.creditType_incorrectItemFees.voucher_date = settlement_deposit_date; // Convert.ToDateTime(item.ob_tbl_sales_order.settlement_from).ToString("yyyy-MM-dd");
                        main_TallyObj.creditType_incorrectItemFees.voucher_number = voucher_number; //"Amazon November 2017 - V 01";
                        main_TallyObj.creditType_incorrectItemFees.ledger_name = "Amazon-Incorrect Item Fees";


                        main_TallyObj.debitType_INCORRECT_FEES_ITEMS = new TallyDebitCredit();
                        main_TallyObj.debitType_INCORRECT_FEES_ITEMS.debit_amt = Convert.ToDouble(item.ob_tbl_sales_order.Cost_of_Advertising) * (-1);
                        main_TallyObj.debitType_INCORRECT_FEES_ITEMS.voucher_date = settlement_deposit_date; // Convert.ToDateTime(item.ob_tbl_sales_order.settlement_from).ToString("yyyy-MM-dd");
                        main_TallyObj.debitType_INCORRECT_FEES_ITEMS.voucher_number = voucher_number;// "Amazon November 2017 - V 01";
                        main_TallyObj.debitType_INCORRECT_FEES_ITEMS.ledger_name = "Amazon-Cost of Advertising";

                        //add others also
                        main_TallyObj.debitType_StorageFee = new TallyDebitCredit();
                        main_TallyObj.debitType_StorageFee.debit_amt = Convert.ToDouble(item.ob_tbl_sales_order.Storage_Fee) * (-1);
                        main_TallyObj.debitType_StorageFee.voucher_date = settlement_deposit_date;
                        main_TallyObj.debitType_StorageFee.voucher_number = voucher_number;// "Amazon November 2017 - V 01";
                        main_TallyObj.debitType_StorageFee.ledger_name = "Amazon-Storage Fee";

                        //add vineet
                        main_TallyObj.creditType_BalanceAdjustment = new TallyDebitCredit();
                        main_TallyObj.creditType_BalanceAdjustment.credit_amt = Convert.ToDouble(item.ob_tbl_sales_order.BalanceAdjustment);
                        main_TallyObj.creditType_BalanceAdjustment.voucher_date = settlement_deposit_date;
                        main_TallyObj.creditType_BalanceAdjustment.voucher_number = voucher_number;// "Amazon November 2017 - V 01";
                        main_TallyObj.creditType_BalanceAdjustment.ledger_name = "Amazon-Balance Adjustment";

                        main_TallyObj.debitType_FBAInboundTransportationFee = new TallyDebitCredit();
                        main_TallyObj.debitType_FBAInboundTransportationFee.debit_amt = Convert.ToDouble(item.ob_tbl_sales_order.FBAInboundTransportationFee) * (-1);
                        main_TallyObj.debitType_FBAInboundTransportationFee.voucher_date = settlement_deposit_date; // Convert.ToDateTime(item.ob_tbl_sales_order.settlement_from).ToString("yyyy-MM-dd");
                        main_TallyObj.debitType_FBAInboundTransportationFee.voucher_number = voucher_number;// "Amazon November 2017 - V 01";
                        main_TallyObj.debitType_FBAInboundTransportationFee.ledger_name = "Amazon-FBA Inbound Transportation Fee";

                        //sharad91
                        main_TallyObj.debitType_Payable_to_Amazon = new TallyDebitCredit();
                        main_TallyObj.debitType_Payable_to_Amazon.debit_amt = Convert.ToDouble(item.ob_tbl_sales_order.Payable_to_Amazon) * (-1);
                        main_TallyObj.debitType_Payable_to_Amazon.voucher_date = settlement_deposit_date; // Convert.ToDateTime(item.ob_tbl_sales_order.settlement_from).ToString("yyyy-MM-dd");
                        main_TallyObj.debitType_Payable_to_Amazon.voucher_number = voucher_number;// "Amazon November 2017 - V 01";
                        main_TallyObj.debitType_Payable_to_Amazon.ledger_name = "Amazon-Payable to Amazon";

                        if (item.ob_tbl_sales_order.suspense_amt != 0 || item.ob_tbl_sales_order.suspense_amt != null)
                        {
                            main_TallyObj.suspenseEntry = new TallyDebitCredit();
                            if (item.ob_tbl_sales_order.suspense_amt < 0)
                                main_TallyObj.suspenseEntry.debit_amt = Convert.ToDouble(item.ob_tbl_sales_order.suspense_amt) * (-1);
                            else
                            {
                                main_TallyObj.suspenseEntry.credit_amt = Convert.ToDouble(item.ob_tbl_sales_order.suspense_amt);
                            }
                            main_TallyObj.suspenseEntry.voucher_date = settlement_deposit_date;
                            main_TallyObj.suspenseEntry.voucher_number = voucher_number;
                            main_TallyObj.suspenseEntry.ledger_name = "Amazon-Suspense";
                        }


                        var get_settlementtaxdata = dba.tbl_tax.Where(a => a.tbl_seller_id == sellers_id && a.reference_type == 10 && a.tbl_referneced_id == item.ob_tbl_sales_order.Id).AsNoTracking().ToList();
                        if (get_settlementtaxdata != null)
                        {
                            int i = 0;
                            foreach (var tax1 in get_settlementtaxdata)
                            {
                                if (tax1.CGST_amount != 0)
                                {
                                    main_TallyObj.debittype_StorageFeeCGST = new TallyDebitCredit();
                                    main_TallyObj.debittype_StorageFeeCGST.debit_amt = Convert.ToDouble(tax1.CGST_amount) * (-1);
                                    main_TallyObj.debittype_StorageFeeCGST.voucher_date = settlement_deposit_date;
                                    main_TallyObj.debitType_StorageFee.voucher_number = voucher_number;//"";
                                    main_TallyObj.debittype_StorageFeeCGST.ledger_name = "Amazon-Storage Billing CGST";
                                }
                                if (tax1.sgst_amount != 0)
                                {
                                    main_TallyObj.debittype_StorageFeeSGST = new TallyDebitCredit();
                                    main_TallyObj.debittype_StorageFeeSGST.debit_amt = Convert.ToDouble(tax1.sgst_amount) * (-1);
                                    main_TallyObj.debittype_StorageFeeSGST.voucher_date = settlement_deposit_date;
                                    main_TallyObj.debittype_StorageFeeSGST.voucher_number = voucher_number;// "";
                                    main_TallyObj.debittype_StorageFeeSGST.ledger_name = "Amazon-Storage Billing SGST";
                                }


                            }//end for
                        }
                    }

                    main_TallyObj.tallyOrderDict = new Dictionary<string, TallyOrder>();
                    #region setlement order
                    var get_settlement_data = dba.tbl_settlement_order.Where(a => a.tbl_seller_id == sellers_id && a.settlement_id == settlement_reference).AsNoTracking().ToList();
                    if (get_settlement_data != null)
                    {
                        foreach (var sett in get_settlement_data)
                        {
                            double orderprincipal = Convert.ToDouble(sett.principal_price);
                            double orderproduct_tax = Convert.ToDouble(sett.product_tax);
                            double ordershipping = Convert.ToDouble(sett.shipping_price);
                            double ordershipping_tax = Convert.ToDouble(sett.shipping_tax);
                            double ordergiftwrap = Convert.ToDouble(sett.giftwrap_price);
                            double ordergiftwrap_tax = Convert.ToDouble(sett.giftwarp_tax);

                            double shippingdiscount = Convert.ToDouble(sett.shipping_discount);
                            double shippingtaxdiscount = Convert.ToDouble(sett.shipping_tax_discount);

                            double orderTotal = orderprincipal + orderproduct_tax + ordershipping + ordershipping_tax + ordergiftwrap + ordergiftwrap_tax + shippingdiscount + shippingtaxdiscount;
                            string uniqueOrderId = sett.unique_order_id;
                            string origOrderid = sett.Order_Id;

                            TallyOrder tallyOrderObj = null;
                            TallyOrder tallyOrderObj1 = null;

                            if (sett.INCORRECT_FEES_ITEMS != 0 && sett.INCORRECT_FEES_ITEMS != null)
                            {

                                CreateTallyOrderObject(ref tallyOrderObj1, ref main_TallyObj, sett.Order_Id, settlement_deposit_date, sett.unique_order_id, MarketPlace);
                                tallyOrderObj1.voucherEntry.credit_amt = (double)sett.INCORRECT_FEES_ITEMS;
                                tallyOrderObj1.voucherEntry.ledger_name = "Amazon-INCORRECT_FEES_ITEMS";
                            }

                            if (sett.SAFE_T_Reimbursement != 0 && sett.SAFE_T_Reimbursement != null)
                            {

                                CreateTallyOrderObject(ref tallyOrderObj1, ref main_TallyObj, sett.Order_Id, settlement_deposit_date, sett.unique_order_id, MarketPlace);
                                tallyOrderObj1.voucherEntry.credit_amt = (double)sett.SAFE_T_Reimbursement;
                                tallyOrderObj1.voucherEntry.ledger_name = "Amazon-SAFE_T_Reimbursement";
                            }

                            if (orderprincipal != 0)
                            {
                                CreateTallyOrderObject(ref tallyOrderObj, ref main_TallyObj, sett.Order_Id, settlement_deposit_date, sett.unique_order_id, MarketPlace);
                                tallyOrderObj.voucherEntry.credit_amt = orderTotal;
                            }

                            //added for cancellation
                            if (tallyOrderObj == null && tallyOrderObj1 == null)
                            {
                                CreateTallyOrderObject(ref tallyOrderObj, ref main_TallyObj, sett.Order_Id, settlement_deposit_date, sett.unique_order_id, MarketPlace);
                            }

                            //tallyOrderObj.voucherEntry = new TallyDebitCredit();

                            if (sett.Order_Id == "405-1404269-7210700")
                            {

                            }
                            //run a for loop for expenses (only for orders )
                            var expnseRecords = dba.m_tbl_expense.Where(a => a.tbl_seller_id == sellers_id && a.expense_type_id != 6 && a.expense_type_id != 8 && a.expense_type_id != 9 && a.expense_type_id != 10 && a.expense_type_id != 11 && a.settlement_order_id == uniqueOrderId && a.t_transactionType_id == 1 && a.reference_number == settlement_reference).AsNoTracking().ToList();

                            //sharad
                            if (tallyOrderObj != null)
                                HandleExpense(2, expnseRecords, ref tallyOrderObj, settlement_deposit_date, origOrderid, MarketPlace);
                            else if (tallyOrderObj1 != null)
                                HandleExpense(2, expnseRecords, ref tallyOrderObj1, settlement_deposit_date, origOrderid, MarketPlace);
                            else
                            {
                                int rt = 0;
                                //should not happen
                            }
                            ///////////sharad/////////////

                        }//end for loop - settlement orders
                        #endregion

                        //----------get all refund order from history table------------
                        var get_historydata = dba.tbl_order_history.Where(a => a.tbl_seller_id == sellers_id && a.settlement_id == settlement_reference && a.t_order_status == 9).AsNoTracking().ToList();
                        if (get_historydata != null)
                        {
                            foreach (var history in get_historydata)
                            {
                                string uoid = history.unique_order_id;
                                TallyOrder tobj = null;
                                if (history.OrderID == "171-1833488-2618741")
                                {

                                }

                                if (main_TallyObj.tallyOrderDict.ContainsKey(uoid))
                                {
                                    tobj = main_TallyObj.tallyOrderDict[uoid];
                                }
                                else
                                {
                                    CreateTallyOrderObject(ref tobj, ref main_TallyObj, history.OrderID, settlement_deposit_date, history.unique_order_id, MarketPlace);
                                }

                                if (history.OrderID == "402-4820983-5601120")
                                {

                                }
                                double refundprincipal = Convert.ToDouble(history.amount_per_unit);
                                double refundshipping = Convert.ToDouble(history.shipping_price);
                                double refundgiftwrap = Convert.ToDouble(history.Giftwrap_price);
                                double refundshippingdiscount1 = Convert.ToDouble(history.shipping_discount);
                                double refundshippingtaxdiscount1 = Convert.ToDouble(history.shipping_tax_discount);
                                double refundproduct_tax = Convert.ToDouble(history.product_tax);
                                double refundshipping_tax = Convert.ToDouble(history.shipping_tax);
                                double refundgiftwrap_tax = Convert.ToDouble(history.gift_wrap_tax);

                                double refundtotal = refundprincipal + refundshipping + refundgiftwrap + refundshippingdiscount1 + refundshippingtaxdiscount1 + refundproduct_tax + refundshipping_tax + refundgiftwrap_tax;

                                if (refundtotal < 0)
                                    refundtotal = refundtotal * (-1);

                                tobj.voucherEntry.debit_amt = refundtotal;

                                string origOrderid = history.OrderID;


                                if (history.SAFE_T_Reimbursement != 0 && history.SAFE_T_Reimbursement != null)
                                {
                                    TallyOrder tallyOrderObj1 = null;
                                    CreateTallyOrderObject(ref tallyOrderObj1, ref main_TallyObj, history.OrderID, settlement_deposit_date, history.unique_order_id + "-0", MarketPlace);
                                    tallyOrderObj1.voucherEntry.credit_amt = (double)history.SAFE_T_Reimbursement;
                                    tallyOrderObj1.voucherEntry.ledger_name = "Amazon-SAFE-T Reimbursement";
                                }

                                var returnexpnseRecords = dba.m_tbl_expense.Where(a => a.tbl_seller_id == sellers_id && a.expense_type_id != 6 && a.expense_type_id != 8 && a.expense_type_id != 9 && a.expense_type_id != 10 && a.expense_type_id != 11 && a.reference_number == settlement_reference && a.t_transactionType_id == 2 && a.tbl_order_historyid == history.Id).AsNoTracking().ToList();

                                HandleExpense(7, returnexpnseRecords, ref tobj, settlement_deposit_date, origOrderid, MarketPlace);
                            }//end for loop for all order history data
                        }

                    }
                }

            }//end try
            catch (Exception e)
            {

            }

            /////----------------------------------Export Excel----------------------------------

            List<SaleReport> lstOrdertext2 = GenerateViewFortallySettlementVoucher(settlement_deposit_date, settlement_reference, main_TallyObj, MarketPlace);

            string sendOnMail = form["sendmail"];
            string value = form["command"];
            if (value == "Export" || sendOnMail == "Send On Mail")
            {
                if (ddl_export != null && ddl_export == 3)
                {
                    #region XML
                    int message = 0;
                    XmlDocument xmlDoc = new XmlDocument();
                    TallyVoucherXml objxml = new TallyVoucherXml();
                    var xmldata = objxml.TallySettlementXML(lstOrdertext2, settlement_deposit_date, settlement_reference, CompanyName, sellers_id, MarketPlace);
                    Response.Buffer = true;
                    Response.Write(xmldata);
                    Response.AddHeader("Content-disposition", "attachment; filename=" + "Tally Voucher" + ".xml");
                    Response.ContentType = "application/octet-stream";
                    Response.End();
                    message = 1;
                    #endregion
                }
                else if (ddl_export != null && ddl_export == 1)
                {
                    bool mail = false;
                    if (sendOnMail != null) mail = true;
                    string EmailID = form["txt_email"];
                    DataTable header = new DataTable();
                    DataTable export_dt = new DataTable();
                    export_dt = cf.CreateTallyVoucherDatatable(lstOrdertext2);
                    int type = Convert.ToInt32(ddl_export);
                    if (export_dt.Rows.Count > 0)
                    {
                        //sharad 9 jan - changed file name
                        //int r = cf.Export("Tally Voucher Report", header, export_dt, Response, type, mail, EmailID);
                        string fname = MarketPlace + "_Settlement_" + settlement_reference + "_" + settlement_deposit_date;
                        int r = cf.Export(fname, header, export_dt, Response, type, mail, EmailID);
                    }
                    else ViewData["Message"] = "There are No records In Table";
                }

                else ViewData["Message"] = "Please Select Export Type";

            }



            //----------------------------------------END-----------------------------------------////

            return View(lstOrdertext2);
        }


        public void CreateTallyOrderObject(ref TallyOrder tallyOrderObj, ref MainTallySettlementVoucher main_TallyObj, string origOrder_Id, string settlement_deposit_date, string uniqueOrderId, string MarketPlace)
        {
            tallyOrderObj = new TallyOrder();
            tallyOrderObj.tallyExpenseDebitCreditDict = new Dictionary<string, TallyDebitCredit>();
            tallyOrderObj.orig_Order_id = origOrder_Id;
            tallyOrderObj.voucherEntry = new TallyDebitCredit();
            tallyOrderObj.voucherEntry.ledger_name = MarketPlace + " " + "Sale Customer" + " " + MarketPlace;
            tallyOrderObj.voucherEntry.voucher_number = MarketPlace + "November 2017 - V 01";
            tallyOrderObj.voucherEntry.voucher_date = settlement_deposit_date; // Convert.ToDateTime(sett.posted_date).ToString("yyyy-MM-dd");
            tallyOrderObj.voucherEntry.reference_num = origOrder_Id;

            tallyOrderObj.igst = new TallyDebitCredit();
            tallyOrderObj.igst.ledger_name = MarketPlace + "-IGST";
            tallyOrderObj.igst.voucher_date = settlement_deposit_date;
            tallyOrderObj.igst.reference_num = origOrder_Id;

            tallyOrderObj.cgst = new TallyDebitCredit();
            tallyOrderObj.cgst = new TallyDebitCredit();
            tallyOrderObj.cgst.ledger_name = MarketPlace + "-CGST";
            tallyOrderObj.cgst.voucher_date = settlement_deposit_date;
            tallyOrderObj.cgst.reference_num = origOrder_Id;

            tallyOrderObj.sgst = new TallyDebitCredit();
            tallyOrderObj.sgst = new TallyDebitCredit();
            tallyOrderObj.sgst.ledger_name = MarketPlace + "-SGST";
            tallyOrderObj.sgst.voucher_date = settlement_deposit_date;
            tallyOrderObj.sgst.reference_num = origOrder_Id;

            main_TallyObj.tallyOrderDict.Add(uniqueOrderId, tallyOrderObj);

            //return tallyOrderObj;
        }

        public void NewHandleExpense(int orderOrRefund, ref int maincounter, ref List<tbl_Settlement_voucher> get_settlement_data, ref TallyOrder tallyOrderObj, string settlement_deposit_date, string origOrderid, string MarketPlace)
        {
            if (origOrderid == "4900888057")
            {
            }

            int id = get_settlement_data[maincounter].Id;
            do
            {
                tbl_Settlement_voucher record = get_settlement_data[maincounter];
                if (record.expense_type_id == null)
                    break;
                TallyDebitCredit tallyObj = null;
                if (tallyOrderObj.tallyExpenseDebitCreditDict.ContainsKey(record.expense_type_id + ""))
                {
                    tallyObj = tallyOrderObj.tallyExpenseDebitCreditDict[record.expense_type_id + ""];
                }
                else
                {
                    tallyObj = new TallyDebitCredit();
                    tallyOrderObj.tallyExpenseDebitCreditDict.Add(record.expense_type_id + "", tallyObj);
                    tallyObj.reference_num = origOrderid;
                    tallyObj.voucher_number = MarketPlace + "November 2017 - V 01";
                    tallyObj.voucher_date = settlement_deposit_date;
                }
                tallyObj.ledger_name = MarketPlace + "-" + record.return_fee;
                if (record.t_transactionType_id == 1) //order
                {
                    if (record.expense_amount < 0)
                        tallyObj.debit_amt = Convert.ToDouble(record.expense_amount) * (-1);
                    else
                        tallyObj.credit_amt = Convert.ToDouble(record.expense_amount);

                    if (record.Igst_amount != null || record.CGST_amount != null || record.sgst_amount != null)
                        HandleTax(1, record.return_fee, ref tallyOrderObj, (double)record.Igst_amount, (double)record.CGST_amount, (double)record.sgst_amount, MarketPlace);
                }
                else
                {
                    if (Convert.ToDouble(record.expense_amount) < 0)
                        tallyObj.debit_amt = Convert.ToDouble(record.expense_amount) * (-1);
                    else
                        tallyObj.credit_amt = Convert.ToDouble(record.expense_amount);

                    if (record.Igst_amount != null || record.CGST_amount != null || record.sgst_amount != null)
                        HandleTax(2, record.return_fee, ref tallyOrderObj, (double)record.Igst_amount, (double)record.CGST_amount, (double)record.sgst_amount, MarketPlace);
                }


                maincounter++;
            } while (maincounter < get_settlement_data.Count && get_settlement_data[maincounter].Id == id);

            if (maincounter < get_settlement_data.Count)
                maincounter--;
        }

        public void HandleTax(int order_or_refund, string nam, ref TallyOrder tallyOrderObj, double Igst_amount, double CGST_amount, double sgst_amount, string MarketPlace)
        {
            double igst, cgst, sgst;

            if (order_or_refund == 2)
            {
                if (nam == "Refund commission")
                {
                    if ((Igst_amount != 0 && Igst_amount != null))
                    {
                        tallyOrderObj.igst.credit_amt += Convert.ToDouble(Igst_amount);
                    }
                    else
                    {
                        cgst = Convert.ToDouble(CGST_amount);
                        sgst = Convert.ToDouble(sgst_amount);
                        if (cgst < 0)
                        {
                            tallyOrderObj.cgst.debit_amt += cgst;
                            tallyOrderObj.sgst.debit_amt += sgst;
                        }
                        else
                        {
                            tallyOrderObj.cgst.credit_amt += cgst;
                            tallyOrderObj.sgst.credit_amt += sgst;
                        }
                    }
                    return;
                }
                else if (nam == "Amazon Easy Ship Charges")
                {
                    if ((Igst_amount != 0 && Igst_amount != null))
                    {
                        tallyOrderObj.igst.credit_amt += Convert.ToDouble(Igst_amount);
                    }
                    else
                    {
                        cgst = Convert.ToDouble(CGST_amount);
                        sgst = Convert.ToDouble(sgst_amount);
                        if (cgst < 0)
                        {
                            tallyOrderObj.cgst.debit_amt += cgst;
                            tallyOrderObj.sgst.debit_amt += sgst;
                        }
                        else
                        {
                            tallyOrderObj.cgst.credit_amt += cgst;
                            tallyOrderObj.sgst.credit_amt += sgst;
                        }
                    }
                    return;
                }
            }
            if ((Igst_amount != 0 && Igst_amount != null))
            {
                igst = Convert.ToDouble(Igst_amount);
                if (order_or_refund == 1)
                {
                    //order
                    tallyOrderObj.igst.debit_amt += igst;
                }
                else
                {
                    tallyOrderObj.igst.credit_amt += igst;
                }
            }
            else
            {
                cgst = Convert.ToDouble(CGST_amount);
                sgst = Convert.ToDouble(sgst_amount);
                if (order_or_refund == 1)
                {
                    tallyOrderObj.cgst.debit_amt += cgst;
                    tallyOrderObj.sgst.debit_amt += sgst;
                }
                else
                {
                    tallyOrderObj.cgst.credit_amt += cgst;
                    tallyOrderObj.sgst.credit_amt += sgst;
                }
            }
            return;

        }

        //2 means order, 7 means refund
        public void HandleExpense(int orderOrRefund, List<m_tbl_expense> expnseRecords, ref TallyOrder tallyOrderObj, string settlement_deposit_date, string origOrderid, string MarketPlace)
        {
            if (origOrderid == "5011095265") //407-2005591-3021147")
            {

            }

            if (expnseRecords != null && expnseRecords.Count > 0)
            {
                foreach (var expenseItem in expnseRecords)
                {
                    TallyDebitCredit tallyObj = null;
                    if (tallyOrderObj.tallyExpenseDebitCreditDict.ContainsKey(expenseItem.expense_type_id + ""))
                    {
                        tallyObj = tallyOrderObj.tallyExpenseDebitCreditDict[expenseItem.expense_type_id + ""];
                    }
                    else
                    {
                        tallyObj = new TallyDebitCredit();
                        tallyOrderObj.tallyExpenseDebitCreditDict.Add(expenseItem.expense_type_id + "", tallyObj);
                        tallyObj.reference_num = origOrderid;
                        tallyObj.voucher_number = MarketPlace + "November 2017 - V 01";
                        tallyObj.voucher_date = settlement_deposit_date;
                    }
                    var exp_id = expenseItem.expense_type_id;
                    var get_expdetails = dba.m_settlement_fee.Where(a => a.id == exp_id).AsNoTracking().FirstOrDefault();
                    if (get_expdetails != null)
                    {
                        tallyObj.ledger_name = MarketPlace + "-" + get_expdetails.return_fee;
                        if (expenseItem.t_transactionType_id == 1) //order
                        {
                            if (expenseItem.expense_amount < 0)
                                tallyObj.debit_amt = Convert.ToDouble(expenseItem.expense_amount) * (-1);
                            else
                                tallyObj.credit_amt = Convert.ToDouble(expenseItem.expense_amount);

                            //reference_type == 2 means settlement_order is teh source
                            var gettax_details = dba.tbl_tax.Where(a => a.tbl_referneced_id == expenseItem.id && a.reference_type == orderOrRefund).AsNoTracking().FirstOrDefault();
                            if (gettax_details != null)
                            {
                                string nam = get_expdetails.return_fee;
                                double igst, cgst, sgst;

                                if (nam == "FBA Weight Handling Fee")
                                {
                                    if ((gettax_details.Igst_amount != 0 && gettax_details.Igst_amount != null))
                                    {
                                        igst = Convert.ToDouble(gettax_details.Igst_amount);
                                        tallyOrderObj.igst.debit_amt += igst;
                                    }
                                    else
                                    {
                                        cgst = Convert.ToDouble(gettax_details.CGST_amount);
                                        sgst = Convert.ToDouble(gettax_details.sgst_amount);
                                        tallyOrderObj.cgst.debit_amt += cgst;
                                        tallyOrderObj.sgst.debit_amt += sgst;
                                    }
                                }
                                else if (nam == "Technology Fee")
                                {
                                    if ((gettax_details.Igst_amount != 0 && gettax_details.Igst_amount != null))
                                    {
                                        igst = Convert.ToDouble(gettax_details.Igst_amount);
                                        tallyOrderObj.igst.debit_amt += igst;
                                    }
                                    else
                                    {
                                        cgst = Convert.ToDouble(gettax_details.CGST_amount);
                                        sgst = Convert.ToDouble(gettax_details.sgst_amount);
                                        tallyOrderObj.cgst.debit_amt += cgst;
                                        tallyOrderObj.sgst.debit_amt += sgst;
                                    }
                                }
                                else if (nam == "Commission")
                                {
                                    if ((gettax_details.Igst_amount != 0 && gettax_details.Igst_amount != null))
                                    {
                                        igst = Convert.ToDouble(gettax_details.Igst_amount);
                                        tallyOrderObj.igst.debit_amt += igst;
                                    }
                                    else
                                    {
                                        cgst = Convert.ToDouble(gettax_details.CGST_amount);
                                        sgst = Convert.ToDouble(gettax_details.sgst_amount);
                                        tallyOrderObj.cgst.debit_amt += cgst;
                                        tallyOrderObj.sgst.debit_amt += sgst;
                                    }
                                }
                                else if (nam == "Fixed closing fee")
                                {
                                    if ((gettax_details.Igst_amount != 0 && gettax_details.Igst_amount != null))
                                    {
                                        igst = Convert.ToDouble(gettax_details.Igst_amount);
                                        tallyOrderObj.igst.debit_amt += igst;
                                    }
                                    else
                                    {
                                        cgst = Convert.ToDouble(gettax_details.CGST_amount);
                                        sgst = Convert.ToDouble(gettax_details.sgst_amount);
                                        tallyOrderObj.cgst.debit_amt += cgst;
                                        tallyOrderObj.sgst.debit_amt += sgst;
                                    }
                                }
                                else if (nam == "Shipping Chargeback")
                                {
                                    if ((gettax_details.Igst_amount != 0 && gettax_details.Igst_amount != null))
                                    {
                                        igst = Convert.ToDouble(gettax_details.Igst_amount);
                                        tallyOrderObj.igst.debit_amt += igst;
                                    }
                                    else
                                    {
                                        cgst = Convert.ToDouble(gettax_details.CGST_amount);
                                        sgst = Convert.ToDouble(gettax_details.sgst_amount);
                                        tallyOrderObj.cgst.debit_amt += cgst;
                                        tallyOrderObj.sgst.debit_amt += sgst;
                                    }
                                }
                                else if (nam == "Shipping commission")
                                {
                                    if ((gettax_details.Igst_amount != 0 && gettax_details.Igst_amount != null))
                                    {
                                        igst = Convert.ToDouble(gettax_details.Igst_amount);
                                        tallyOrderObj.igst.debit_amt += igst;
                                    }
                                    else
                                    {
                                        cgst = Convert.ToDouble(gettax_details.CGST_amount);
                                        sgst = Convert.ToDouble(gettax_details.sgst_amount);
                                        tallyOrderObj.cgst.debit_amt += cgst;
                                        tallyOrderObj.sgst.debit_amt += sgst;
                                    }
                                }

                                else if (nam == "Marketplace Commission")
                                {
                                    if ((gettax_details.Igst_amount != 0 && gettax_details.Igst_amount != null))
                                    {
                                        igst = Convert.ToDouble(gettax_details.Igst_amount);
                                        tallyOrderObj.igst.debit_amt += igst;
                                    }
                                    else
                                    {
                                        cgst = Convert.ToDouble(gettax_details.CGST_amount);
                                        sgst = Convert.ToDouble(gettax_details.sgst_amount);
                                        tallyOrderObj.cgst.debit_amt += cgst;
                                        tallyOrderObj.sgst.debit_amt += sgst;
                                    }
                                }
                                else if (nam == "Logistics Charges")
                                {
                                    if ((gettax_details.Igst_amount != 0 && gettax_details.Igst_amount != null))
                                    {
                                        igst = Convert.ToDouble(gettax_details.Igst_amount);
                                        tallyOrderObj.igst.debit_amt += igst;
                                    }
                                    else
                                    {
                                        cgst = Convert.ToDouble(gettax_details.CGST_amount);
                                        sgst = Convert.ToDouble(gettax_details.sgst_amount);
                                        tallyOrderObj.cgst.debit_amt += cgst;
                                        tallyOrderObj.sgst.debit_amt += sgst;
                                    }
                                }
                                else if (nam == "PG Commission")
                                {
                                    if ((gettax_details.Igst_amount != 0 && gettax_details.Igst_amount != null))
                                    {
                                        igst = Convert.ToDouble(gettax_details.Igst_amount);
                                        tallyOrderObj.igst.debit_amt += igst;
                                    }
                                    else
                                    {
                                        cgst = Convert.ToDouble(gettax_details.CGST_amount);
                                        sgst = Convert.ToDouble(gettax_details.sgst_amount);
                                        tallyOrderObj.cgst.debit_amt += cgst;
                                        tallyOrderObj.sgst.debit_amt += sgst;
                                    }
                                }
                                else if (nam == "Penalty")
                                {
                                    if ((gettax_details.Igst_amount != 0 && gettax_details.Igst_amount != null))
                                    {
                                        igst = Convert.ToDouble(gettax_details.Igst_amount);
                                        tallyOrderObj.igst.debit_amt += igst;
                                    }
                                    else
                                    {
                                        cgst = Convert.ToDouble(gettax_details.CGST_amount);
                                        sgst = Convert.ToDouble(gettax_details.sgst_amount);
                                        tallyOrderObj.cgst.debit_amt += cgst;
                                        tallyOrderObj.sgst.debit_amt += sgst;
                                    }
                                }
                                else if (nam == "Net Adjustments")
                                {
                                    if ((gettax_details.Igst_amount != 0 && gettax_details.Igst_amount != null))
                                    {
                                        igst = Convert.ToDouble(gettax_details.Igst_amount);
                                        tallyOrderObj.igst.debit_amt += igst;
                                    }
                                    else
                                    {
                                        cgst = Convert.ToDouble(gettax_details.CGST_amount);
                                        sgst = Convert.ToDouble(gettax_details.sgst_amount);
                                        tallyOrderObj.cgst.debit_amt += cgst;
                                        tallyOrderObj.sgst.debit_amt += sgst;
                                    }
                                }


                                else if (nam == "Easy Ship weight handling fees")
                                {
                                    if ((gettax_details.Igst_amount != 0 && gettax_details.Igst_amount != null))
                                    {
                                        igst = Convert.ToDouble(gettax_details.Igst_amount);
                                        tallyOrderObj.igst.debit_amt += igst;
                                    }
                                    else
                                    {
                                        cgst = Convert.ToDouble(gettax_details.CGST_amount);
                                        sgst = Convert.ToDouble(gettax_details.sgst_amount);
                                        tallyOrderObj.cgst.debit_amt += cgst;
                                        tallyOrderObj.sgst.debit_amt += sgst;
                                    }
                                }
                                else if (nam == "Refund commission")
                                {
                                    //view_salereport2.RefundDiscount = Convert.ToDouble(gettax_details.Igst_amount);
                                }
                                else if (nam == "Order Cancellation Charge")
                                {
                                    if ((gettax_details.Igst_amount != 0 && gettax_details.Igst_amount != null))
                                    {
                                        igst = Convert.ToDouble(gettax_details.Igst_amount);
                                        tallyOrderObj.igst.debit_amt += igst;
                                    }
                                    else
                                    {
                                        cgst = Convert.ToDouble(gettax_details.CGST_amount);
                                        sgst = Convert.ToDouble(gettax_details.sgst_amount);
                                        tallyOrderObj.cgst.debit_amt += cgst;
                                        tallyOrderObj.sgst.debit_amt += sgst;
                                    }
                                    //view_salereport2.RefundDiscount = Convert.ToDouble(gettax_details.Igst_amount);
                                }
                                else if (nam == "Amazon Easy Ship Charges")
                                {
                                    if ((gettax_details.Igst_amount != 0 && gettax_details.Igst_amount != null))
                                    {
                                        igst = Convert.ToDouble(gettax_details.Igst_amount);
                                        tallyOrderObj.igst.debit_amt += igst;
                                    }
                                    else
                                    {
                                        cgst = Convert.ToDouble(gettax_details.CGST_amount);
                                        sgst = Convert.ToDouble(gettax_details.sgst_amount);
                                        tallyOrderObj.cgst.debit_amt += cgst;
                                        tallyOrderObj.sgst.debit_amt += sgst;
                                    }
                                    //view_salereport2.RefundDiscount = Convert.ToDouble(gettax_details.Igst_amount);
                                }
                                else if (nam == "FBA Pick & Pack Fee")
                                {
                                    if ((gettax_details.Igst_amount != 0 && gettax_details.Igst_amount != null))
                                    {
                                        igst = Convert.ToDouble(gettax_details.Igst_amount);
                                        tallyOrderObj.igst.debit_amt += igst;
                                    }
                                    else
                                    {
                                        cgst = Convert.ToDouble(gettax_details.CGST_amount);
                                        sgst = Convert.ToDouble(gettax_details.sgst_amount);
                                        tallyOrderObj.cgst.debit_amt += cgst;
                                        tallyOrderObj.sgst.debit_amt += sgst;
                                    }
                                }
                                else if (nam == "Gift Wrap Chargeback")
                                {
                                    if ((gettax_details.Igst_amount != 0 && gettax_details.Igst_amount != null))
                                    {
                                        igst = Convert.ToDouble(gettax_details.Igst_amount);
                                        tallyOrderObj.igst.debit_amt += igst;
                                    }
                                    else
                                    {
                                        cgst = Convert.ToDouble(gettax_details.CGST_amount);
                                        sgst = Convert.ToDouble(gettax_details.sgst_amount);
                                        tallyOrderObj.cgst.debit_amt += cgst;
                                        tallyOrderObj.sgst.debit_amt += sgst;
                                    }
                                }

                                else if (nam == "Shipping Fee")//add vineet for flipkart
                                {
                                    if ((gettax_details.Igst_amount != 0 && gettax_details.Igst_amount != null))
                                    {
                                        igst = Convert.ToDouble(gettax_details.Igst_amount);
                                        tallyOrderObj.igst.debit_amt += igst;
                                    }
                                    else
                                    {
                                        cgst = Convert.ToDouble(gettax_details.CGST_amount);
                                        sgst = Convert.ToDouble(gettax_details.sgst_amount);
                                        tallyOrderObj.cgst.debit_amt += cgst;
                                        tallyOrderObj.sgst.debit_amt += sgst;
                                    }
                                }

                                else if (nam == "Reverse Shipping Fee")//add vineet for flipkart
                                {
                                    if ((gettax_details.Igst_amount != 0 && gettax_details.Igst_amount != null))
                                    {
                                        igst = Convert.ToDouble(gettax_details.Igst_amount);
                                        tallyOrderObj.igst.debit_amt += igst;
                                    }
                                    else
                                    {
                                        cgst = Convert.ToDouble(gettax_details.CGST_amount);
                                        sgst = Convert.ToDouble(gettax_details.sgst_amount);
                                        tallyOrderObj.cgst.debit_amt += cgst;
                                        tallyOrderObj.sgst.debit_amt += sgst;
                                    }
                                }
                                else if (nam == "Collection Fee")//add vineet for flipkart
                                {
                                    if ((gettax_details.Igst_amount != 0 && gettax_details.Igst_amount != null))
                                    {
                                        igst = Convert.ToDouble(gettax_details.Igst_amount);
                                        tallyOrderObj.igst.debit_amt += igst;
                                    }
                                    else
                                    {
                                        cgst = Convert.ToDouble(gettax_details.CGST_amount);
                                        sgst = Convert.ToDouble(gettax_details.sgst_amount);
                                        tallyOrderObj.cgst.debit_amt += cgst;
                                        tallyOrderObj.sgst.debit_amt += sgst;
                                    }
                                }
                                else if (nam == "Fixed Fee")//add vineet for flipkart
                                {
                                    if ((gettax_details.Igst_amount != 0 && gettax_details.Igst_amount != null))
                                    {
                                        igst = Convert.ToDouble(gettax_details.Igst_amount);
                                        tallyOrderObj.igst.debit_amt += igst;
                                    }
                                    else
                                    {
                                        cgst = Convert.ToDouble(gettax_details.CGST_amount);
                                        sgst = Convert.ToDouble(gettax_details.sgst_amount);
                                        tallyOrderObj.cgst.debit_amt += cgst;
                                        tallyOrderObj.sgst.debit_amt += sgst;
                                    }
                                }
                            }// end of gettax
                        }
                        else
                        {
                            double aaa = 0;
                            //refund 
                            if (Convert.ToDouble(expenseItem.expense_amount) < 0)
                                tallyObj.debit_amt += Convert.ToDouble(expenseItem.expense_amount) * (-1);
                            else
                            {
                                tallyObj.credit_amt += Convert.ToDouble(expenseItem.expense_amount);
                            }

                            //reference_type == 2 means settlement_order is teh source
                            var gettax_details = dba.tbl_tax.Where(a => a.tbl_referneced_id == expenseItem.id && a.reference_type == orderOrRefund).AsNoTracking().FirstOrDefault();
                            if (gettax_details != null)
                            {
                                string nam = get_expdetails.return_fee;
                                double igst, cgst, sgst;

                                if (nam == "FBA Weight Handling Fee")
                                {
                                    if ((gettax_details.Igst_amount != 0 && gettax_details.Igst_amount != null))
                                    {
                                        igst = Convert.ToDouble(gettax_details.Igst_amount);
                                        tallyOrderObj.igst.credit_amt += igst;
                                    }
                                    else
                                    {
                                        cgst = Convert.ToDouble(gettax_details.CGST_amount);
                                        sgst = Convert.ToDouble(gettax_details.sgst_amount);
                                        tallyOrderObj.cgst.credit_amt += cgst;
                                        tallyOrderObj.sgst.credit_amt += sgst;
                                    }
                                }
                                else if (nam == "Technology Fee")
                                {
                                    if ((gettax_details.Igst_amount != 0 && gettax_details.Igst_amount != null))
                                    {
                                        igst = Convert.ToDouble(gettax_details.Igst_amount);
                                        tallyOrderObj.igst.credit_amt += igst;
                                    }
                                    else
                                    {
                                        cgst = Convert.ToDouble(gettax_details.CGST_amount);
                                        sgst = Convert.ToDouble(gettax_details.sgst_amount);
                                        tallyOrderObj.cgst.credit_amt += cgst;
                                        tallyOrderObj.sgst.credit_amt += sgst;
                                    }
                                }
                                else if (nam == "Commission")
                                {
                                    if ((gettax_details.Igst_amount != 0 && gettax_details.Igst_amount != null))
                                    {
                                        igst = Convert.ToDouble(gettax_details.Igst_amount);
                                        tallyOrderObj.igst.credit_amt += igst;
                                    }
                                    else
                                    {
                                        cgst = Convert.ToDouble(gettax_details.CGST_amount);
                                        sgst = Convert.ToDouble(gettax_details.sgst_amount);
                                        tallyOrderObj.cgst.credit_amt += cgst;
                                        tallyOrderObj.sgst.credit_amt += sgst;
                                    }
                                }
                                else if (nam == "Fixed closing fee")
                                {
                                    if ((gettax_details.Igst_amount != 0 && gettax_details.Igst_amount != null))
                                    {
                                        igst = Convert.ToDouble(gettax_details.Igst_amount);
                                        tallyOrderObj.igst.credit_amt += igst;
                                    }
                                    else
                                    {
                                        cgst = Convert.ToDouble(gettax_details.CGST_amount);
                                        sgst = Convert.ToDouble(gettax_details.sgst_amount);
                                        tallyOrderObj.cgst.credit_amt += cgst;
                                        tallyOrderObj.sgst.credit_amt += sgst;
                                    }
                                }
                                else if (nam == "Shipping Chargeback")
                                {
                                    if ((gettax_details.Igst_amount != 0 && gettax_details.Igst_amount != null))
                                    {
                                        igst = Convert.ToDouble(gettax_details.Igst_amount);
                                        tallyOrderObj.igst.credit_amt += igst;
                                    }
                                    else
                                    {
                                        cgst = Convert.ToDouble(gettax_details.CGST_amount);
                                        sgst = Convert.ToDouble(gettax_details.sgst_amount);
                                        tallyOrderObj.cgst.credit_amt += cgst;
                                        tallyOrderObj.sgst.credit_amt += sgst;
                                    }
                                }

                                else if (nam == "Marketplace Commission")
                                {
                                    if ((gettax_details.Igst_amount != 0 && gettax_details.Igst_amount != null))
                                    {
                                        igst = Convert.ToDouble(gettax_details.Igst_amount);
                                        tallyOrderObj.igst.credit_amt += igst;
                                    }
                                    else
                                    {
                                        cgst = Convert.ToDouble(gettax_details.CGST_amount);
                                        sgst = Convert.ToDouble(gettax_details.sgst_amount);
                                        tallyOrderObj.cgst.credit_amt += cgst;
                                        tallyOrderObj.sgst.credit_amt += sgst;
                                    }
                                }
                                else if (nam == "Logistics Charges")
                                {
                                    if ((gettax_details.Igst_amount != 0 && gettax_details.Igst_amount != null))
                                    {
                                        igst = Convert.ToDouble(gettax_details.Igst_amount);
                                        tallyOrderObj.igst.credit_amt += igst;
                                    }
                                    else
                                    {
                                        cgst = Convert.ToDouble(gettax_details.CGST_amount);
                                        sgst = Convert.ToDouble(gettax_details.sgst_amount);
                                        tallyOrderObj.cgst.credit_amt += cgst;
                                        tallyOrderObj.sgst.credit_amt += sgst;
                                    }
                                }
                                else if (nam == "PG Commission")
                                {
                                    if ((gettax_details.Igst_amount != 0 && gettax_details.Igst_amount != null))
                                    {
                                        igst = Convert.ToDouble(gettax_details.Igst_amount);
                                        tallyOrderObj.igst.credit_amt += igst;
                                    }
                                    else
                                    {
                                        cgst = Convert.ToDouble(gettax_details.CGST_amount);
                                        sgst = Convert.ToDouble(gettax_details.sgst_amount);
                                        tallyOrderObj.cgst.credit_amt += cgst;
                                        tallyOrderObj.sgst.credit_amt += sgst;
                                    }
                                }
                                else if (nam == "Penalty")
                                {
                                    if ((gettax_details.Igst_amount != 0 && gettax_details.Igst_amount != null))
                                    {
                                        igst = Convert.ToDouble(gettax_details.Igst_amount);
                                        tallyOrderObj.igst.credit_amt += igst;
                                    }
                                    else
                                    {
                                        cgst = Convert.ToDouble(gettax_details.CGST_amount);
                                        sgst = Convert.ToDouble(gettax_details.sgst_amount);
                                        tallyOrderObj.cgst.credit_amt += cgst;
                                        tallyOrderObj.sgst.credit_amt += sgst;
                                    }
                                }
                                else if (nam == "Net Adjustments")
                                {
                                    if ((gettax_details.Igst_amount != 0 && gettax_details.Igst_amount != null))
                                    {
                                        igst = Convert.ToDouble(gettax_details.Igst_amount);
                                        tallyOrderObj.igst.credit_amt += igst;
                                    }
                                    else
                                    {
                                        cgst = Convert.ToDouble(gettax_details.CGST_amount);
                                        sgst = Convert.ToDouble(gettax_details.sgst_amount);
                                        tallyOrderObj.cgst.credit_amt += cgst;
                                        tallyOrderObj.sgst.credit_amt += sgst;
                                    }
                                }


                                else if (nam == "Shipping commission")
                                {
                                    if ((gettax_details.Igst_amount != 0 && gettax_details.Igst_amount != null))
                                    {
                                        igst = Convert.ToDouble(gettax_details.Igst_amount);
                                        tallyOrderObj.igst.credit_amt += igst;
                                    }
                                    else
                                    {
                                        cgst = Convert.ToDouble(gettax_details.CGST_amount);
                                        sgst = Convert.ToDouble(gettax_details.sgst_amount);
                                        tallyOrderObj.cgst.credit_amt += cgst;
                                        tallyOrderObj.sgst.credit_amt += sgst;
                                    }
                                }
                                else if (nam == "Easy Ship weight handling fees")
                                {
                                    if ((gettax_details.Igst_amount != 0 && gettax_details.Igst_amount != null))
                                    {
                                        igst = Convert.ToDouble(gettax_details.Igst_amount);
                                        tallyOrderObj.igst.credit_amt += igst;
                                    }
                                    else
                                    {
                                        cgst = Convert.ToDouble(gettax_details.CGST_amount);
                                        sgst = Convert.ToDouble(gettax_details.sgst_amount);
                                        tallyOrderObj.cgst.credit_amt += cgst;
                                        tallyOrderObj.sgst.credit_amt += sgst;
                                    }
                                }
                                else if (nam == "Refund commission")
                                {
                                    if ((gettax_details.Igst_amount != 0 && gettax_details.Igst_amount != null))
                                    {
                                        tallyOrderObj.igst.credit_amt += Convert.ToDouble(gettax_details.Igst_amount);
                                    }
                                    else
                                    {
                                        cgst = Convert.ToDouble(gettax_details.CGST_amount);
                                        sgst = Convert.ToDouble(gettax_details.sgst_amount);
                                        if (cgst < 0)
                                        {
                                            tallyOrderObj.cgst.debit_amt += cgst;
                                            tallyOrderObj.sgst.debit_amt += sgst;
                                        }
                                        else
                                        {
                                            tallyOrderObj.cgst.credit_amt += cgst;
                                            tallyOrderObj.sgst.credit_amt += sgst;
                                        }
                                    }
                                    //tallyOrderObj.igst.debit_amt = Convert.ToDouble(gettax_details.Igst_amount) * (-1);
                                    //view_salereport2.RefundDiscount = Convert.ToDouble(gettax_details.Igst_amount);
                                }
                                else if (nam == "Amazon Easy Ship Charges")
                                {
                                    if ((gettax_details.Igst_amount != 0 && gettax_details.Igst_amount != null))
                                    {
                                        tallyOrderObj.igst.credit_amt += Convert.ToDouble(gettax_details.Igst_amount);
                                    }
                                    else
                                    {
                                        cgst = Convert.ToDouble(gettax_details.CGST_amount);
                                        sgst = Convert.ToDouble(gettax_details.sgst_amount);
                                        if (cgst < 0)
                                        {
                                            tallyOrderObj.cgst.debit_amt += cgst;
                                            tallyOrderObj.sgst.debit_amt += sgst;
                                        }
                                        else
                                        {
                                            tallyOrderObj.cgst.credit_amt += cgst;
                                            tallyOrderObj.sgst.credit_amt += sgst;
                                        }
                                    }
                                }

                                else if (nam == "Shipping Fee")//add by vineet for flipkart
                                {
                                    if ((gettax_details.Igst_amount != 0 && gettax_details.Igst_amount != null))
                                    {
                                        igst = Convert.ToDouble(gettax_details.Igst_amount);
                                        tallyOrderObj.igst.credit_amt += igst;
                                    }
                                    else
                                    {
                                        cgst = Convert.ToDouble(gettax_details.CGST_amount);
                                        sgst = Convert.ToDouble(gettax_details.sgst_amount);
                                        tallyOrderObj.cgst.credit_amt += cgst;
                                        tallyOrderObj.sgst.credit_amt += sgst;
                                    }
                                }

                                else if (nam == "Reverse Shipping Fee")//add by vineet for flipkart
                                {
                                    if ((gettax_details.Igst_amount != 0 && gettax_details.Igst_amount != null))
                                    {
                                        igst = Convert.ToDouble(gettax_details.Igst_amount);
                                        tallyOrderObj.igst.credit_amt += igst;
                                    }
                                    else
                                    {
                                        cgst = Convert.ToDouble(gettax_details.CGST_amount);
                                        sgst = Convert.ToDouble(gettax_details.sgst_amount);
                                        tallyOrderObj.cgst.credit_amt += cgst;
                                        tallyOrderObj.sgst.credit_amt += sgst;
                                    }
                                }
                                else if (nam == "Collection Fee")//add by vineet for flipkart Fixed Fee
                                {
                                    if ((gettax_details.Igst_amount != 0 && gettax_details.Igst_amount != null))
                                    {
                                        igst = Convert.ToDouble(gettax_details.Igst_amount);
                                        tallyOrderObj.igst.credit_amt += igst;
                                    }
                                    else
                                    {
                                        cgst = Convert.ToDouble(gettax_details.CGST_amount);
                                        sgst = Convert.ToDouble(gettax_details.sgst_amount);
                                        tallyOrderObj.cgst.credit_amt += cgst;
                                        tallyOrderObj.sgst.credit_amt += sgst;
                                    }
                                }
                                else if (nam == "Fixed Fee")//add by vineet for flipkart Fixed Fee
                                {
                                    if ((gettax_details.Igst_amount != 0 && gettax_details.Igst_amount != null))
                                    {
                                        igst = Convert.ToDouble(gettax_details.Igst_amount);
                                        tallyOrderObj.igst.credit_amt += igst;
                                    }
                                    else
                                    {
                                        cgst = Convert.ToDouble(gettax_details.CGST_amount);
                                        sgst = Convert.ToDouble(gettax_details.sgst_amount);
                                        tallyOrderObj.cgst.credit_amt += cgst;
                                        tallyOrderObj.sgst.credit_amt += sgst;
                                    }
                                }
                            }
                        }
                    }

                }//end for loop expenseRecirds

            }
        }

        public List<SaleReport> GenerateViewFortallySettlementVoucher(String settlement_deposit_date, String settlement_ref, MainTallySettlementVoucher main_TallyObj, string MarketPlace)
        {
            List<SaleReport> lstOrdertext2 = new List<SaleReport>();

            if (main_TallyObj.debitType_bank_balance == null)
                return lstOrdertext2;

            SaleReport objb = new SaleReport();

            //sharad 221
            if (main_TallyObj.debitType_bank_balance.debit_amt >= 0)
                objb.refund_SumOrder = Convert.ToDouble(main_TallyObj.debitType_bank_balance.debit_amt);
            else
                objb.SumOrder = Convert.ToDouble(main_TallyObj.debitType_bank_balance.debit_amt) * (-1);

            objb.Sett_orderDate = main_TallyObj.voucher_date;// Convert.ToDateTime(item.ob_tbl_sales_order.settlement_from).ToString("yyyy-MM-dd");
            objb.VoucherNumber = voucher_number; //"Amazon November 2017 - V 01";
            objb.ExpenseName = MarketPlace + "-Receipt";
            objb.Narration = settlement_ref;
            lstOrdertext2.Add(objb);

            foreach (KeyValuePair<string, TallyOrder> pair in main_TallyObj.tallyOrderDict)
            {
                TallyOrder tallyorder1 = pair.Value;

                SaleReport objb1 = new SaleReport();
                objb1.Sett_orderDate = tallyorder1.voucherEntry.voucher_date;// Convert.ToDateTime(item.ob_tbl_sales_order.settlement_from).ToString("yyyy-MM-dd");
                objb1.VoucherNumber = voucher_number; //tallyorder1.voucherEntry.voucher_number;
                objb1.ExpenseName = tallyorder1.voucherEntry.ledger_name;
                objb1.refund_SumOrder = Math.Round(tallyorder1.voucherEntry.debit_amt, 2);
                objb1.SumOrder = Math.Round(tallyorder1.voucherEntry.credit_amt, 2);
                objb1.OrderID = tallyorder1.voucherEntry.reference_num;
                objb1.Narration = settlement_ref;
                if (objb1.refund_SumOrder == 0 && objb1.SumOrder == 0)
                {

                }
                else
                    lstOrdertext2.Add(objb1);

                foreach (KeyValuePair<string, TallyDebitCredit> pairr in tallyorder1.tallyExpenseDebitCreditDict)
                {
                    TallyDebitCredit tobj = pairr.Value;

                    SaleReport objb2 = new SaleReport();
                    objb2.Sett_orderDate = tobj.voucher_date;// Convert.ToDateTime(item.ob_tbl_sales_order.settlement_from).ToString("yyyy-MM-dd");
                    objb2.VoucherNumber = voucher_number; //tobj.voucher_number;
                    objb2.ExpenseName = tobj.ledger_name;
                    objb2.refund_SumOrder = Math.Round(tobj.debit_amt, 2);
                    objb2.SumOrder = Math.Round(tobj.credit_amt, 2);
                    objb2.OrderID = tobj.reference_num;
                    objb2.Sett_orderDate = tobj.voucher_date;
                    objb2.Narration = settlement_ref;
                    if (objb2.refund_SumOrder == 0 && objb2.SumOrder == 0)
                    {

                    }
                    else
                        lstOrdertext2.Add(objb2);

                }//end for loop

                bool foundAmazonEasyShipCharges = false;
                SaleReport objb3 = new SaleReport();
                objb3.Sett_orderDate = tallyorder1.igst.voucher_date;// Convert.ToDateTime(item.ob_tbl_sales_order.settlement_from).ToString("yyyy-MM-dd");
                objb3.VoucherNumber = voucher_number; //tallyorder1.igst.voucher_number;
                if (lstOrdertext2.Count > 0 && (lstOrdertext2[lstOrdertext2.Count - 1].ExpenseName == MarketPlace + "Easy Ship Charges"))
                {
                    foundAmazonEasyShipCharges = true;
                    objb3.ExpenseName = "MFNPostagePurchaseComplete " + tallyorder1.igst.ledger_name;
                }
                else
                {
                    objb3.ExpenseName = tallyorder1.igst.ledger_name;
                }

                //objb3.ExpenseName = tallyorder1.igst.ledger_name;
                objb3.refund_SumOrder = tallyorder1.igst.debit_amt < 0 ? Math.Round(tallyorder1.igst.debit_amt, 2) * -1 : Math.Round(tallyorder1.igst.debit_amt, 2);

                if (tallyorder1.igst.credit_amt < 0)
                {
                    objb3.refund_SumOrder += Math.Round(tallyorder1.igst.credit_amt, 2) * -1;
                }
                else
                {
                    objb3.SumOrder = tallyorder1.igst.credit_amt < 0 ? Math.Round(tallyorder1.igst.credit_amt, 2) * -1 : Math.Round(tallyorder1.igst.credit_amt, 2);
                }
                objb3.OrderID = tallyorder1.igst.reference_num;
                objb3.Narration = settlement_ref;
                if (objb3.refund_SumOrder == 0 && objb3.SumOrder == 0)
                {
                }
                else
                    lstOrdertext2.Add(objb3);

                objb3 = new SaleReport();
                objb3.Sett_orderDate = tallyorder1.cgst.voucher_date;// Convert.ToDateTime(item.ob_tbl_sales_order.settlement_from).ToString("yyyy-MM-dd");
                objb3.VoucherNumber = voucher_number; //tallyorder1.cgst.voucher_number;
                if (lstOrdertext2.Count > 0 && (lstOrdertext2[lstOrdertext2.Count - 1].ExpenseName == MarketPlace + "Easy Ship Charges"))
                {
                    foundAmazonEasyShipCharges = true;
                    objb3.ExpenseName = "MFNPostagePurchaseComplete " + tallyorder1.cgst.ledger_name;
                }
                else
                {

                    objb3.ExpenseName = tallyorder1.cgst.ledger_name;
                }
                //objb3.ExpenseName = tallyorder1.cgst.ledger_name;
                objb3.refund_SumOrder = tallyorder1.cgst.debit_amt < 0 ? Math.Round(tallyorder1.cgst.debit_amt, 2) * -1 : Math.Round(tallyorder1.cgst.debit_amt, 2);

                if (tallyorder1.cgst.credit_amt < 0)//add by vineet for flipkart
                {
                    objb3.refund_SumOrder += Math.Round(tallyorder1.cgst.credit_amt, 2) * -1;
                }
                else//add by vineet for flipkart
                {
                    objb3.SumOrder = tallyorder1.cgst.credit_amt < 0 ? Math.Round(tallyorder1.cgst.credit_amt, 2) * -1 : Math.Round(tallyorder1.cgst.credit_amt, 2);
                }
                objb3.OrderID = tallyorder1.cgst.reference_num;
                objb3.Narration = settlement_ref;
                if (objb3.refund_SumOrder == 0 && objb3.SumOrder == 0)
                {
                }
                else
                    lstOrdertext2.Add(objb3);

                objb3 = new SaleReport();
                objb3.Sett_orderDate = tallyorder1.sgst.voucher_date;// Convert.ToDateTime(item.ob_tbl_sales_order.settlement_from).ToString("yyyy-MM-dd");
                objb3.VoucherNumber = voucher_number; //tallyorder1.sgst.voucher_number;
                if (foundAmazonEasyShipCharges)
                {
                    objb3.ExpenseName = "MFNPostagePurchaseComplete " + tallyorder1.sgst.ledger_name;
                }
                else
                {

                    objb3.ExpenseName = tallyorder1.sgst.ledger_name;
                }

                //objb3.ExpenseName = tallyorder1.sgst.ledger_name;
                objb3.refund_SumOrder = tallyorder1.sgst.debit_amt < 0 ? Math.Round(tallyorder1.sgst.debit_amt, 2) * -1 : Math.Round(tallyorder1.sgst.debit_amt, 2);
                if (tallyorder1.sgst.credit_amt < 0)//add by vineet for flipkart
                {
                    objb3.refund_SumOrder += Math.Round(tallyorder1.sgst.credit_amt, 2) * -1;
                }
                else//add by vineet for flipkart
                {
                    objb3.SumOrder = tallyorder1.sgst.credit_amt < 0 ? Math.Round(tallyorder1.sgst.credit_amt, 2) * -1 : Math.Round(tallyorder1.sgst.credit_amt, 2); // tallyorder1.sgst.credit_amt;
                }
                objb3.OrderID = tallyorder1.sgst.reference_num;
                objb3.Narration = settlement_ref;
                if (objb3.refund_SumOrder == 0 && objb3.SumOrder == 0)
                {

                }
                else
                    lstOrdertext2.Add(objb3);

            }//end for loop


            objb = new SaleReport();
            objb.SumOrder = Math.Round(Convert.ToDouble(main_TallyObj.creditType_Previous_Reserve_Amount_Balance.credit_amt), 2);
            objb.Sett_orderDate = main_TallyObj.voucher_date;// Convert.ToDateTime(item.ob_tbl_sales_order.settlement_from).ToString("yyyy-MM-dd");
            objb.VoucherNumber = voucher_number; //"Amazon November 2017 - V 01";  Amazon - Withheld Amount
            objb.ExpenseName = MarketPlace + "-Previous Reserve Amount Balance";
            objb.Narration = settlement_ref;
            objb.OrderDate = settlement_deposit_date;
            if (objb.refund_SumOrder == 0 && objb.SumOrder == 0)
            {
            }
            else
                //lstOrdertext2.Add(objb);

                objb = new SaleReport();
            objb.refund_SumOrder = Math.Round(Convert.ToDouble(main_TallyObj.debitType_Current_Reserve_Amount.debit_amt), 2);
            objb.Sett_orderDate = main_TallyObj.voucher_date;// Convert.ToDateTime(item.ob_tbl_sales_order.settlement_from).ToString("yyyy-MM-dd");
            objb.VoucherNumber = voucher_number; //"Amazon November 2017 - V 01";  Amazon - Withheld Amount
            objb.ExpenseName = MarketPlace + "-Current Reserve Amount";
            objb.Narration = settlement_ref;
            objb.OrderDate = settlement_deposit_date;
            if (objb.refund_SumOrder == 0 && objb.SumOrder == 0)
            {
            }
            else
                // lstOrdertext2.Add(objb);


                objb = new SaleReport();//changes by vineet

            double currentamount = Math.Round(Convert.ToDouble(main_TallyObj.debitType_Current_Reserve_Amount.debit_amt), 2);
            double previousamount = Math.Round(Convert.ToDouble(main_TallyObj.creditType_Previous_Reserve_Amount_Balance.credit_amt), 2);
            double abc = currentamount - previousamount;
            if (abc < 0)
            {
                objb.SumOrder = Math.Round(Convert.ToDouble(abc), 2) * (-1); //Math.Round(Convert.ToDouble(main_TallyObj.debitType_Current_Reserve_Amount.debit_amt), 2);
                objb.Sett_orderDate = main_TallyObj.voucher_date;// Convert.ToDateTime(item.ob_tbl_sales_order.settlement_from).ToString("yyyy-MM-dd");
                objb.VoucherNumber = voucher_number; //"Amazon November 2017 - V 01";  Amazon - Withheld Amount
                objb.ExpenseName = MarketPlace + "- Withheld Amount";
                objb.Narration = settlement_ref;
                objb.OrderDate = settlement_deposit_date;
                //if (objb.refund_SumOrder == 0 && objb.SumOrder == 0)
                //{
                //}
                //else
                lstOrdertext2.Add(objb);
            }
            else
            {
                objb.refund_SumOrder = Math.Round(Convert.ToDouble(abc), 2); //Math.Round(Convert.ToDouble(main_TallyObj.debitType_Current_Reserve_Amount.debit_amt), 2);
                objb.Sett_orderDate = main_TallyObj.voucher_date;// Convert.ToDateTime(item.ob_tbl_sales_order.settlement_from).ToString("yyyy-MM-dd");
                objb.VoucherNumber = voucher_number; //"Amazon November 2017 - V 01";  Amazon - Withheld Amount
                objb.ExpenseName = MarketPlace + "- Withheld Amount";
                objb.Narration = settlement_ref;
                objb.OrderDate = settlement_deposit_date;
                //if (objb.refund_SumOrder == 0 && objb.SumOrder == 0)
                //{
                //}
                //else
                lstOrdertext2.Add(objb);
            }


            objb = new SaleReport();
            objb.SumOrder = Math.Round(Convert.ToDouble(main_TallyObj.creditType_Nonsubscription_feeadj.credit_amt), 2);
            objb.Sett_orderDate = main_TallyObj.voucher_date;// Convert.ToDateTime(item.ob_tbl_sales_order.settlement_from).ToString("yyyy-MM-dd");
            objb.VoucherNumber = voucher_number; //"Amazon November 2017 - V 01";
            objb.ExpenseName = MarketPlace + "-NonSubscriptionFeeAdj";
            objb.Narration = settlement_ref;
            objb.OrderDate = settlement_deposit_date;
            if (objb.refund_SumOrder == 0 && objb.SumOrder == 0)
            {

            }
            else
            {
                if (objb.SumOrder < 0)
                {
                    objb.refund_SumOrder = objb.SumOrder * (-1);
                    objb.SumOrder = 0;
                }
                lstOrdertext2.Add(objb);
            }
            objb = new SaleReport();

            objb.SumOrder = Math.Round(Convert.ToDouble(main_TallyObj.creditType_incorrectItemFees.credit_amt), 2);
            objb.Sett_orderDate = main_TallyObj.voucher_date;// Convert.ToDateTime(item.ob_tbl_sales_order.settlement_from).ToString("yyyy-MM-dd");
            objb.VoucherNumber = voucher_number; //"Amazon November 2017 - V 01";
            objb.ExpenseName = MarketPlace + "-FBA Inventory Reimbursement";
            objb.Narration = settlement_ref;
            objb.OrderDate = settlement_deposit_date;
            if (objb.refund_SumOrder == 0 && objb.SumOrder == 0)
            {

            }
            else
                lstOrdertext2.Add(objb);

            if (main_TallyObj.debitType_INCORRECT_FEES_ITEMS != null)
            {
                objb = new SaleReport();
                objb.refund_SumOrder = Math.Round(Convert.ToDouble(main_TallyObj.debitType_INCORRECT_FEES_ITEMS.debit_amt), 2);
                objb.Sett_orderDate = main_TallyObj.voucher_date;// Convert.ToDateTime(item.ob_tbl_sales_order.settlement_from).ToString("yyyy-MM-dd");
                objb.VoucherNumber = voucher_number; //"Amazon November 2017 - V 01";
                objb.ExpenseName = MarketPlace + "-Cost of Advertising";
                objb.Narration = settlement_ref;
                objb.OrderDate = settlement_deposit_date;
                if (objb.refund_SumOrder == 0 && objb.SumOrder == 0)
                {
                }
                else
                {
                    lstOrdertext2.Add(objb);
                }
            }
            if (main_TallyObj.debitType_StorageFee != null)
            {
                objb = new SaleReport();
                objb.refund_SumOrder = Math.Round(Convert.ToDouble(main_TallyObj.debitType_StorageFee.debit_amt), 2);
                objb.Sett_orderDate = main_TallyObj.voucher_date;
                objb.VoucherNumber = voucher_number;
                objb.ExpenseName = MarketPlace + "-Storage Fee";
                objb.Narration = settlement_ref;
                objb.OrderDate = settlement_deposit_date;
                if (objb.refund_SumOrder == 0 && objb.SumOrder == 0)
                {

                }
                else
                {
                    lstOrdertext2.Add(objb);
                }
            }
            if (main_TallyObj.debittype_StorageFeeCGST != null)
            {
                objb = new SaleReport();
                objb.refund_SumOrder = Math.Round(Convert.ToDouble(main_TallyObj.debittype_StorageFeeCGST.debit_amt), 2);
                objb.Sett_orderDate = main_TallyObj.voucher_date;
                objb.VoucherNumber = voucher_number;
                objb.ExpenseName = MarketPlace + "-CGST";
                objb.Narration = settlement_ref;
                objb.OrderDate = settlement_deposit_date;
                if (objb.refund_SumOrder == 0 && objb.SumOrder == 0)
                {

                }
                else
                {
                    lstOrdertext2.Add(objb);
                }
            }
            if (main_TallyObj.debittype_StorageFeeSGST != null)
            {
                objb = new SaleReport();
                objb.refund_SumOrder = Math.Round(Convert.ToDouble(main_TallyObj.debittype_StorageFeeSGST.debit_amt), 2);
                objb.Sett_orderDate = main_TallyObj.voucher_date;
                objb.VoucherNumber = voucher_number;
                objb.ExpenseName = MarketPlace + "-SGST";
                objb.Narration = settlement_ref;
                objb.OrderDate = settlement_deposit_date;
                if (objb.refund_SumOrder == 0 && objb.SumOrder == 0)
                {

                }
                else
                {
                    lstOrdertext2.Add(objb);
                }
            }
            if (main_TallyObj.creditType_BalanceAdjustment != null)
            {
                objb = new SaleReport();
                objb.SumOrder = Math.Round(Convert.ToDouble(main_TallyObj.creditType_BalanceAdjustment.credit_amt), 2);
                objb.Sett_orderDate = main_TallyObj.voucher_date;
                objb.VoucherNumber = voucher_number;
                objb.ExpenseName = MarketPlace + "-Balance Adjustment";
                objb.Narration = settlement_ref;
                objb.OrderDate = settlement_deposit_date;
                if (objb.refund_SumOrder == 0 && objb.SumOrder == 0)
                {

                }
                else
                {
                    lstOrdertext2.Add(objb);
                }
            }
            if (main_TallyObj.debitType_FBAInboundTransportationFee != null)
            {
                objb = new SaleReport();
                objb.refund_SumOrder = Math.Round(Convert.ToDouble(main_TallyObj.debitType_FBAInboundTransportationFee.debit_amt), 2);
                objb.Sett_orderDate = main_TallyObj.voucher_date;
                objb.VoucherNumber = voucher_number;
                objb.ExpenseName = MarketPlace + "-FBAInboundTransportationFee";
                objb.Narration = settlement_ref;
                objb.OrderDate = settlement_deposit_date;
                if (objb.refund_SumOrder == 0 && objb.SumOrder == 0)
                {

                }
                else
                {
                    lstOrdertext2.Add(objb);
                }
            }
            if (main_TallyObj.debitType_Payable_to_Amazon != null)
            {
                objb = new SaleReport();
                objb.refund_SumOrder = Math.Round(Convert.ToDouble(main_TallyObj.debitType_Payable_to_Amazon.debit_amt), 2);
                objb.Sett_orderDate = main_TallyObj.voucher_date;
                objb.VoucherNumber = voucher_number;
                objb.ExpenseName = MarketPlace + "-Payable to Amazon";
                objb.Narration = settlement_ref;
                objb.OrderDate = settlement_deposit_date;
                if (objb.refund_SumOrder == 0 && objb.SumOrder == 0)
                {

                }
                else
                {
                    lstOrdertext2.Add(objb);
                }
            }
            if (main_TallyObj.suspenseEntry != null)
            {
                objb = new SaleReport();
                if (Convert.ToDouble(main_TallyObj.suspenseEntry.debit_amt) != 0)
                    objb.refund_SumOrder = Math.Round(Convert.ToDouble(main_TallyObj.suspenseEntry.debit_amt), 2);
                else if (Convert.ToDouble(main_TallyObj.suspenseEntry.credit_amt) != 0)
                    objb.SumOrder = Math.Round(Convert.ToDouble(main_TallyObj.suspenseEntry.credit_amt), 2);

                objb.Sett_orderDate = main_TallyObj.voucher_date;
                objb.VoucherNumber = voucher_number;
                objb.ExpenseName = MarketPlace + "-Suspense";
                objb.Narration = settlement_ref;
                objb.OrderDate = settlement_deposit_date;
                if (objb.refund_SumOrder == 0 && objb.SumOrder == 0)
                {

                }
                else
                {
                    lstOrdertext2.Add(objb);
                }
            }
            return lstOrdertext2;
        }
        #endregion

        #region ReturnVoucher
        ///  This is for Return Voucher
        /// </summary>
        /// <returns></returns>
        public ActionResult ReturnVoucher(FormCollection form, DateTime? txt_from, DateTime? txt_to, int? ddl_export)
        {
            cf = new comman_function();
            if (!cf.session_check())
                return RedirectToAction("Login", "Home");
            int sellers_id = Convert.ToInt32(Session["SellerID"]);
            string CompanyName = Convert.ToString(Session["UserName"]);
            //cf = new comman_function();
            ViewData["ExportList"] = cf.GetExportList();
            List<SelectListItem> lst1_loc = cf.GetMarketPlace(sellers_id);
            ViewData["MarKetPlace"] = lst1_loc;
            List<SaleReport> lstOrdertext2 = new List<SaleReport>();
            try
            {
                if (txt_from != null && txt_to != null)
                {
                    SalesVoucher obj = new SalesVoucher();
                    obj.Get_Return_Voucher_Report(lstOrdertext2, txt_from, txt_to, sellers_id);
                    /////----------------------------------Export Excel----------------------------------
                    string sendOnMail = form["sendmail"];
                    string value1 = form["command"];
                    if (value1 == "Export" || sendOnMail == "Send On Mail")
                    {
                        if (ddl_export != null && ddl_export == 3)
                        {
                            #region XML
                            int message = 0;
                            XmlDocument xmlDoc = new XmlDocument();
                            ReturnVoucherXml objxml = new ReturnVoucherXml();
                            // var xmldata = objxml.SalesVoucherXML(lstOrdertext2,CompanyName);
                            var xmldata = objxml.ReturnVoucherXML(txt_from, txt_to, sellers_id, CompanyName);
                            Response.Buffer = true;
                            Response.Write(xmldata);
                            Response.AddHeader("Content-disposition", "attachment; filename=" + " Return Voucher" + ".xml");
                            Response.ContentType = "application/octet-stream";
                            Response.End();
                            message = 1;
                            #endregion
                        }
                        else if (ddl_export != null && ddl_export == 1)
                        {
                            bool mail = false;
                            if (sendOnMail != null) mail = true;
                            string EmailID = form["txt_email"];

                            DataTable header = new DataTable();
                            string colum = " ";
                            for (int t = 0; t < 6; t++)
                            {
                                header.Columns.Add(colum);
                                colum = colum + " ";
                            }
                            DataRow errow = header.NewRow();
                            errow[2] = " ";
                            header.Rows.Add(errow);
                            DataRow drow = header.NewRow();
                            drow[1] = "From Date";
                            drow[2] = txt_from;
                            drow[4] = "Report Taken On";
                            drow[5] = DateTime.Now.ToString("dd/MMM/yyyy");
                            header.Rows.Add(drow);
                            DataRow brow = header.NewRow();
                            brow[1] = "To Date";
                            brow[2] = txt_to;
                            header.Rows.Add(brow);
                            DataRow erow = header.NewRow();
                            erow[2] = " ";
                            header.Rows.Add(erow);
                            DataTable export_dt = new DataTable();
                            export_dt = cf.CreateSaleVoucherDatatable(lstOrdertext2);
                            int type = Convert.ToInt32(ddl_export);
                            if (export_dt.Rows.Count > 0)
                            {
                                int r = cf.Export("Sale Voucher Report", header, export_dt, Response, type, mail, EmailID);
                            }
                            else ViewData["Message"] = "There are No records In Table";
                        }
                        else ViewData["Message"] = "Please Select Export Type";
                    }
                    //----------------------------------------END-----------------------------------------////
                }// end of if(date)
            }// end of try
            catch (Exception ex)
            {
            }
            return View(lstOrdertext2);
        }

        #endregion

        #region SummaryNetRealization
        /// <summary>
        /// this is for SummaryNetRealization Report
        /// </summary>
        /// <param name="form"></param>
        /// <param name="txt_from"></param>
        /// <param name="txt_to"></param>
        /// <param name="ddl_export"></param>
        /// <returns></returns>
        public ActionResult SummaryNetRealization(FormCollection form, int? ddl_market_place, DateTime? txt_from, DateTime? txt_to, int? ddl_export)
        {
            int sellers_id = Convert.ToInt32(Session["SellerID"]);
            cf = new comman_function();
            ViewData["ExportList"] = cf.GetExcelExportList();
            ViewData["ExpenseList"] = cf.GetExpenseList();
            List<SelectListItem> lst1_loc = cf.GetMarketPlace(sellers_id);
            ViewData["MarKetPlace"] = lst1_loc;
            List<SaleReport> lstOrdertext2 = new List<SaleReport>();
            SaleReport view_salereport = null;
            try
            {
                if (txt_from != null && txt_to != null)
                {
                    SalesorderReport obj = new SalesorderReport();
                    if (ddl_market_place == 3)
                    {
                        obj.Get_AmazonSummaryRealizationReport(lstOrdertext2, view_salereport, ddl_market_place, txt_from, txt_to, sellers_id);
                    }
                    else if (ddl_market_place == 1)
                    {
                        obj.Get_FlipkartSummaryRealizationReport(lstOrdertext2, view_salereport, ddl_market_place, txt_from, txt_to, sellers_id);
                    }
                    else if (ddl_market_place == 5)
                    {
                        obj.Get_PaytmSummaryRealizationReport(lstOrdertext2, view_salereport, ddl_market_place, txt_from, txt_to, sellers_id);
                    }
                    /////----------------------------------Export Excel----------------------------------

                    string sendOnMail = form["sendmail"];
                    string value1 = form["command"];
                    if (value1 == "Export" || sendOnMail == "Send On Mail")
                    {
                        if (ddl_export != null)
                        {
                            bool mail = false;
                            if (sendOnMail != null) mail = true;
                            string EmailID = form["txt_email"];

                            DataTable header = new DataTable();
                            string colum = " ";
                            for (int t = 0; t < 6; t++)
                            {
                                header.Columns.Add(colum);
                                colum = colum + " ";
                            }
                            DataRow errow = header.NewRow();
                            errow[2] = " ";
                            header.Rows.Add(errow);
                            DataRow drow = header.NewRow();
                            drow[1] = "From Date";
                            drow[2] = txt_from;
                            drow[4] = "Report Taken On";
                            drow[5] = DateTime.Now.ToString("dd/MMM/yyyy");
                            header.Rows.Add(drow);
                            DataRow brow = header.NewRow();
                            brow[1] = "To Date";
                            brow[2] = txt_to;
                            header.Rows.Add(brow);
                            DataRow erow = header.NewRow();
                            erow[2] = " ";
                            header.Rows.Add(erow);
                            DataTable export_dt = new DataTable();
                            export_dt = cf.CreateSummaryRealizationDatatable(lstOrdertext2);
                            int type = Convert.ToInt32(ddl_export);
                            if (export_dt.Rows.Count > 0)
                            {
                                int r = cf.Export("Summary Net Ralization Report", header, export_dt, Response, type, mail, EmailID);
                            }
                            else ViewData["Message"] = "There are No records In Table";
                        }
                        else ViewData["Message"] = "Please Select Export Type";
                    }
                    //----------------------------------------END-----------------------------------------////
                    // end of if(GetSaleOrderDetail)
                }
            }// end of try block
            catch (Exception ex)
            {
            }
            return View(lstOrdertext2);
        }
        #endregion

        public ActionResult BankTransfer()
        {
            cf = new comman_function();
            if (!cf.session_check())
                return RedirectToAction("Login", "Home");
            return View();
        }

        public JsonResult GetBankTransfer()
        {
            int sellers_id = Convert.ToInt32(Session["SellerID"]);
            List<model_settlement_Bank_Transfer> obj = new List<model_settlement_Bank_Transfer>();
            var getdetails = (from ob_market_place in dba.tbl_settlement_upload.Where(a => a.tbl_seller_id == sellers_id)
                              join ob_m_settlement in dba.tbl_imp_bank_transfers on ob_market_place.Id
                              equals ob_m_settlement.tbl_settlement_upload_id
                              into JoinedEmpDept
                              from proj in JoinedEmpDept.DefaultIfEmpty()

                              select new
                              {
                                  ob_market_place = ob_market_place,
                                  ob_m_settlement = proj
                              }).ToList();
            foreach (var item in getdetails)
            {
                var ddd = db.m_marketplace.Where(m => m.isactive == 1 && m.id == item.ob_market_place.market_place_id).Select(p => new { p.id, p.name }).FirstOrDefault();
                model_settlement_Bank_Transfer objtbl = new model_settlement_Bank_Transfer();
                objtbl.amount = item.ob_m_settlement.amount;
                objtbl.deposit_date = item.ob_market_place.deposit_date;
                objtbl.MarketPlaceName = ddd.name;
                objtbl.id = item.ob_m_settlement.Id;
                objtbl.remarks = item.ob_m_settlement.remarks;
                if (item.ob_m_settlement.verifystatus == 1)
                {
                    objtbl.verifystatus = "Verified";
                }
                else
                {
                    objtbl.verifystatus = " Not Verified";
                }
                obj.Add(objtbl);
            }
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateBankDetails(tbl_imp_bank_transfers objm)
        {
            string Message;
            bool flag = false;
            try
            {
                var get_details = dba.tbl_imp_bank_transfers.Where(a => a.Id == objm.Id).FirstOrDefault();
                if (get_details != null)
                {
                    get_details.verifystatus = objm.verifystatus;
                    get_details.remarks = objm.remarks;
                    get_details.verified_on = DateTime.Now;
                    dba.Entry(get_details).State = EntityState.Modified;
                    dba.SaveChanges();
                    flag = true;
                    Message = "Status and Remarks is updated successfully";
                }
                else
                {
                    flag = false;
                    Message = "some error occurred !!";
                }
            }
            catch (Exception e)
            {
                //Debug.WriteLine(e.Message);
                throw;
            }
            return new JsonResult { Data = new { Message = Message, Status = flag } };
        }

        public JsonResult GetSearchTransfer(int? ddl_marketplaceProductAPI, DateTime? Productfrom, DateTime? Productto)
        {
            int sellers_id = Convert.ToInt32(Session["SellerID"]);
            List<model_settlement_Bank_Transfer> obj = new List<model_settlement_Bank_Transfer>();
            //var get_details = dba.tbl_settlement_upload.Where(a => a.tbl_seller_id == sellers_id).ToList();
            var get_details = (from ob_tbl_settlement_upload in dba.tbl_settlement_upload
                               select new
                               {
                                   ob_tbl_settlement_upload = ob_tbl_settlement_upload
                               }).Where(a => a.ob_tbl_settlement_upload.tbl_seller_id == sellers_id).ToList();

            //if (get_details != null)
            //{
            //    if (Productfrom != null && Productto != null)
            //    {
            //        get_details = get_details.Where(a => a.ob_tbl_settlement_upload.deposit_date >= Productfrom && a.ob_tbl_settlement_upload.deposit_date <= Productto).ToList();
            //    }
            //}
            if (get_details != null && ddl_marketplaceProductAPI != null && ddl_marketplaceProductAPI != 0)
            {
                get_details = get_details.Where(a => a.ob_tbl_settlement_upload.market_place_id == ddl_marketplaceProductAPI).ToList();
            }
            if (get_details != null)
            {
                foreach (var item in get_details)
                {
                    model_settlement_Bank_Transfer objtbl = new model_settlement_Bank_Transfer();
                    objtbl.deposit_date = item.ob_tbl_settlement_upload.deposit_date;
                    var getbankdetails = dba.tbl_imp_bank_transfers.Where(a => a.tbl_settlement_upload_id == item.ob_tbl_settlement_upload.Id).FirstOrDefault();
                    if (getbankdetails != null)
                    {
                        objtbl.amount = getbankdetails.amount;
                        objtbl.id = getbankdetails.Id;
                        objtbl.remarks = getbankdetails.remarks;
                        if (getbankdetails.verifystatus == 1)
                        {
                            objtbl.verifystatus = "Verified";
                        }
                        else
                        {
                            objtbl.verifystatus = " Not Verified";
                        }
                    }
                    var ddd = db.m_marketplace.Where(m => m.isactive == 1 && m.id == item.ob_tbl_settlement_upload.market_place_id).Select(p => new { p.id, p.name }).FirstOrDefault();
                    if (ddd != null)
                    {
                        objtbl.MarketPlaceName = ddd.name;
                    }
                    obj.Add(objtbl);
                }
            }
            //var getdetails = (from ob_market_place in dba.tbl_settlement_upload
            //                  join ob_m_settlement in dba.tbl_imp_bank_transfers on ob_market_place.Id
            //                  equals ob_m_settlement.tbl_settlement_upload_id
            //                  into JoinedEmpDept
            //                  from proj in JoinedEmpDept.DefaultIfEmpty()

            //                  select new
            //                  {
            //                      ob_market_place = ob_market_place,
            //                      ob_m_settlement = proj
            //                  }).ToList();
            //foreach (var item in getdetails)
            //{
            //    var ddd = db.m_marketplace.Where(m => m.isactive == 1 && m.id == item.ob_market_place.market_place_id).Select(p => new { p.id, p.name }).FirstOrDefault();
            //    model_settlement_Bank_Transfer objtbl = new model_settlement_Bank_Transfer();
            //    objtbl.amount = item.ob_m_settlement.amount;
            //    objtbl.deposit_date = item.ob_market_place.deposit_date;
            //    objtbl.MarketPlaceName = ddd.name;
            //    objtbl.id = item.ob_m_settlement.Id;
            //    objtbl.remarks = item.ob_m_settlement.remarks;
            //    if (item.ob_m_settlement.verifystatus == 1)
            //    {
            //        objtbl.verifystatus = "Verified";
            //    }
            //    else
            //    {
            //        objtbl.verifystatus = " Not Verified";
            //    }
            //    obj.Add(objtbl);
            //}
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        #region MonthReport
        /// <summary>
        /// This is for Month Report
        /// </summary>
        /// <param name="form"></param>
        /// <param name="ddl_MonthSelecter"></param>
        /// <param name="ddl_yearSelecter"></param>
        /// <param name="txt_from"></param>
        /// <param name="txt_to"></param>
        /// <param name="ddl_export"></param>
        /// <returns></returns>    
        public ActionResult MonthReport(FormCollection form, int? ddl_market_place, int? ddl_MonthSelecter_to, int? ddl_MonthSelecter_from, int? ddl_yearSelecter, DateTime? txt_from, DateTime? txt_to, int? ddl_export)
        {
            cf = new comman_function();
            if (!cf.session_check())
                return RedirectToAction("Login", "Home");
            int sellers_id = Convert.ToInt32(Session["SellerID"]);
            ViewData["ExportList"] = cf.GetExportList();
            List<SelectListItem> lst1_loc = cf.GetMarketPlace(sellers_id);
            ViewData["MarKetPlace"] = lst1_loc;
            bool[] ShowMonthColumn = new bool[12];
            int[] monthcount = new int[12];
            int[] monthsum = new int[12];
            int[] refundmonthcount = new int[12];
            int[] refundsum = new int[12];
            int[] refundsumtax = new int[12];
            double[] bankamount = new double[12];
            double[] adversite = new double[12];
            dynamic RetModel = new ExpandoObject();
            SowMonthColumn showcol = new SowMonthColumn();
            List<MonthReport> lstOrdertext2 = new List<MonthReport>();
            try
            {
                List<tbl_sales_order> lst_order = null;
                List<tbl_order_history> lst_history = null;
                List<tbl_settlement_upload> lst_upload = null;
                List<m_tbl_expense> lst_expense = null;
                if (ddl_MonthSelecter_to != null && ddl_MonthSelecter_from != null)
                {
                    lst_order = dba.tbl_sales_order.Where(a => a.tbl_sellers_id == sellers_id).ToList();
                    lst_history = dba.tbl_order_history.Where(a => a.tbl_seller_id == sellers_id && a.t_order_status == 9).ToList();
                    lst_upload = dba.tbl_settlement_upload.Where(a => a.tbl_seller_id == sellers_id).ToList();
                    lst_expense = dba.m_tbl_expense.Where(a => a.tbl_seller_id == sellers_id).ToList();

                    #region salesorder
                    if (ddl_MonthSelecter_to != null && ddl_MonthSelecter_from != null)
                    {
                        for (int i = Convert.ToInt32(ddl_MonthSelecter_from); i <= Convert.ToInt32(ddl_MonthSelecter_to); i++)
                        {
                            string month = i.ToString();
                            if (month.Length == 1)
                                month = "0" + month;
                            var dateformat = (ddl_yearSelecter + "-" + month + "-");
                            monthcount[i - 1] = lst_order.Where(a => a.purchase_date.Date.ToString().Contains(dateformat)).Count();
                            monthsum[i - 1] = lst_order.Where(a => a.purchase_date.Date.ToString().Contains(dateformat)).Select(a => Convert.ToInt32(a.bill_amount)).Sum();
                            refundmonthcount[i - 1] = lst_history.Where(a => Convert.ToDateTime(a.ShipmentDate).Date.ToString().Contains(dateformat)).Count();
                            refundsum[i - 1] = lst_history.Where(a => Convert.ToDateTime(a.ShipmentDate).Date.ToString().Contains(dateformat)).Select(a => Convert.ToInt32(a.amount_per_unit)).Sum();
                            refundsumtax[i - 1] = lst_history.Where(a => Convert.ToDateTime(a.ShipmentDate).Date.ToString().Contains(dateformat)).Select(a => Convert.ToInt32(a.product_tax)).Sum();

                            adversite[i - 1] = lst_upload.Where(a => Convert.ToDateTime(a.deposit_date).Date.ToString().Contains(dateformat)).Select(a => Convert.ToInt32(a.Cost_of_Advertising)).Sum();
                            var lst = lst_upload.Where(a => Convert.ToDateTime(a.deposit_date).Date.ToString().Contains(dateformat)).ToList();

                            foreach (var item in lst)
                            {
                                int id = item.Id;
                                bankamount[i] += Convert.ToDouble(dba.tbl_imp_bank_transfers.Where(a => a.tbl_settlement_upload_id == id).FirstOrDefault().amount);
                            }
                            ShowMonthColumn[i - 1] = true;
                            showcol.ColSpan++;
                        }
                        MonthReport salernode = new MonthReport();
                        //salernode.Number = 1;
                        salernode.particulars = "Sales(Nos)";
                        salernode.Jan = monthcount[0];
                        salernode.Feb = monthcount[1];
                        salernode.March = monthcount[2];
                        salernode.April = monthcount[3];
                        salernode.May = monthcount[4];
                        salernode.June = monthcount[5];
                        salernode.July = monthcount[6];
                        salernode.Aug = monthcount[7];
                        salernode.Sept = monthcount[8];
                        salernode.Oct = monthcount[9];
                        salernode.Nov = monthcount[10];
                        salernode.Dec = monthcount[11];
                        salernode.Total_mounthcount = salernode.Jan + salernode.Feb + salernode.March + salernode.April + salernode.May + salernode.June + salernode.July + salernode.Aug + salernode.Sept + salernode.Oct + salernode.Nov + salernode.Dec;
                        lstOrdertext2.Add(salernode);


                        MonthReport returnnode = new MonthReport();
                        //returnnode.Number = 2;
                        returnnode.particulars = "Returns(Nos)";
                        returnnode.Jan = refundmonthcount[0];
                        returnnode.Feb = refundmonthcount[1];
                        returnnode.March = refundmonthcount[2];
                        returnnode.April = refundmonthcount[3];
                        returnnode.May = refundmonthcount[4];
                        returnnode.June = refundmonthcount[5];
                        returnnode.July = refundmonthcount[6];
                        returnnode.Aug = refundmonthcount[7];
                        returnnode.Sept = refundmonthcount[8];
                        returnnode.Oct = refundmonthcount[9];
                        returnnode.Nov = refundmonthcount[10];
                        returnnode.Dec = refundmonthcount[11];
                        returnnode.Total_mounthcount = returnnode.Jan + returnnode.Feb + returnnode.March + returnnode.April + returnnode.May + returnnode.June + returnnode.July + returnnode.Aug + returnnode.Sept + returnnode.Oct + returnnode.Nov + returnnode.Dec;
                        lstOrdertext2.Add(returnnode);

                        MonthReport netsale_nosnode = new MonthReport();
                        //netsale_nosnode.Number = 3;
                        netsale_nosnode.particulars = "Net Sales(Nos)";
                        netsale_nosnode.Jan = salernode.Jan - returnnode.Jan;
                        netsale_nosnode.Feb = salernode.Feb - returnnode.Feb;
                        netsale_nosnode.March = salernode.March - returnnode.March;
                        netsale_nosnode.April = salernode.April - returnnode.April;
                        netsale_nosnode.May = salernode.May - returnnode.May;
                        netsale_nosnode.June = salernode.June - returnnode.June;
                        netsale_nosnode.July = salernode.July - returnnode.July;
                        netsale_nosnode.Aug = salernode.Aug - returnnode.Aug;
                        netsale_nosnode.Sept = salernode.Sept - returnnode.Sept;
                        netsale_nosnode.Oct = salernode.Oct - returnnode.Oct;
                        netsale_nosnode.Nov = salernode.Nov - returnnode.Nov;
                        netsale_nosnode.Dec = salernode.Dec - returnnode.Dec;
                        netsale_nosnode.Total_mounthcount = netsale_nosnode.Jan + netsale_nosnode.Feb + netsale_nosnode.March + netsale_nosnode.April + netsale_nosnode.May + netsale_nosnode.June + netsale_nosnode.July + netsale_nosnode.Aug + netsale_nosnode.Sept + netsale_nosnode.Oct + netsale_nosnode.Nov + netsale_nosnode.Dec;
                        lstOrdertext2.Add(netsale_nosnode);

                        MonthReport sale_rs = new MonthReport();
                        //sale_rs.Number = 4;
                        sale_rs.particulars = "Sales(Rs)";
                        sale_rs.Jan = monthsum[0];
                        sale_rs.Feb = monthsum[1];
                        sale_rs.March = monthsum[2];
                        sale_rs.April = monthsum[3];
                        sale_rs.May = monthsum[4];
                        sale_rs.June = monthsum[5];
                        sale_rs.July = monthsum[6];
                        sale_rs.Aug = monthsum[7];
                        sale_rs.Sept = monthsum[8];
                        sale_rs.Oct = monthsum[9];
                        sale_rs.Nov = monthsum[10];
                        sale_rs.Dec = monthsum[11];
                        sale_rs.Total_mounthcount = sale_rs.Jan + sale_rs.Feb + sale_rs.March + sale_rs.April + sale_rs.May + sale_rs.June + sale_rs.July + sale_rs.Aug + sale_rs.Sept + sale_rs.Oct + sale_rs.Nov + sale_rs.Dec;
                        lstOrdertext2.Add(sale_rs);

                        MonthReport return_rs = new MonthReport();
                        //return_rs.Number = 5;
                        return_rs.particulars = "Returns(Rs)";
                        return_rs.Jan = (refundsum[0] + refundsumtax[0]) * (-1);
                        return_rs.Feb = (refundsum[1] + refundsumtax[0]) * (-1);
                        return_rs.March = (refundsum[2] + refundsumtax[0]) * (-1);
                        return_rs.April = (refundsum[3] + refundsumtax[0]) * (-1);
                        return_rs.May = (refundsum[4] + refundsumtax[0]) * (-1);
                        return_rs.June = (refundsum[5] + refundsumtax[0]) * (-1);
                        return_rs.July = (refundsum[6] + refundsumtax[0]) * (-1);
                        return_rs.Aug = (refundsum[7] + refundsumtax[0]) * (-1);
                        return_rs.Sept = (refundsum[8] + refundsumtax[0]) * (-1);
                        return_rs.Oct = (refundsum[9] + refundsumtax[0]) * (-1);
                        return_rs.Nov = (refundsum[10] + refundsumtax[0]) * (-1);
                        return_rs.Dec = (refundsum[11] + refundsumtax[0]) * (-1);
                        return_rs.Total_mounthcount = return_rs.Jan + return_rs.Feb + return_rs.March + return_rs.April + return_rs.May + return_rs.June + return_rs.July + return_rs.Aug + return_rs.Sept + return_rs.Oct + return_rs.Nov + return_rs.Dec;
                        lstOrdertext2.Add(return_rs);

                        MonthReport netsale_rs = new MonthReport();
                        //netsale_rs.Number = 6;
                        netsale_rs.particulars = "Net Sales(Rs)";
                        netsale_rs.Jan = sale_rs.Jan - return_rs.Jan;
                        netsale_rs.Feb = sale_rs.Feb - return_rs.Feb;
                        netsale_rs.March = sale_rs.March - return_rs.March;
                        netsale_rs.April = sale_rs.April - return_rs.April;
                        netsale_rs.May = sale_rs.May - return_rs.May;
                        netsale_rs.June = sale_rs.June - return_rs.June;
                        netsale_rs.July = sale_rs.July - return_rs.July;
                        netsale_rs.Aug = sale_rs.Aug - return_rs.Aug;
                        netsale_rs.Sept = sale_rs.Sept - return_rs.Sept;
                        netsale_rs.Oct = sale_rs.Oct - return_rs.Oct;
                        netsale_rs.Nov = sale_rs.Nov - return_rs.Nov;
                        netsale_rs.Dec = sale_rs.Dec - return_rs.Dec;
                        netsale_rs.Total_mounthcount = netsale_rs.Jan + netsale_rs.Feb + netsale_rs.March + netsale_rs.April + netsale_rs.May + netsale_rs.June + netsale_rs.July + netsale_rs.Aug + netsale_rs.Sept + netsale_rs.Oct + netsale_rs.Nov + netsale_rs.Dec;
                        lstOrdertext2.Add(netsale_rs);

                        MonthReport avg_sale_rs = new MonthReport();
                        string value1 = "", value2 = "", value3 = "", value4 = "", value5 = "", value6 = "", value7 = "", value8 = "", value9 = "", value10 = "", value11 = "", value12 = "", value13 = "";
                        //avg_sale_rs.Number = 7;
                        avg_sale_rs.particulars = "Average Sale Value per item(Rs)";
                        value1 = Convert.ToString((netsale_rs.Jan / netsale_nosnode.Jan).ToString("0.#"));
                        if (value1 != "NaN")
                        {
                            avg_sale_rs.Jan = Convert.ToDouble(value1);
                        }
                        value2 = Convert.ToString((netsale_rs.Feb / netsale_nosnode.Feb).ToString("0.#"));
                        if (value2 != "NaN")
                        {
                            avg_sale_rs.Feb = Convert.ToDouble(value2);
                        }
                        value3 = Convert.ToString((netsale_rs.March / netsale_nosnode.March).ToString("0.#"));
                        if (value3 != "NaN")
                        {
                            avg_sale_rs.March = Convert.ToDouble(value3);
                        }
                        value4 = Convert.ToString((netsale_rs.April / netsale_nosnode.April).ToString("0.#"));
                        if (value4 != "NaN")
                        {
                            avg_sale_rs.April = Convert.ToDouble(value4);
                        }
                        value5 = Convert.ToString((netsale_rs.May / netsale_nosnode.May).ToString("0.#"));
                        if (value5 != "NaN")
                        {
                            avg_sale_rs.May = Convert.ToDouble(value5);
                        }
                        value6 = Convert.ToString((netsale_rs.June / netsale_nosnode.June).ToString("0.#"));
                        if (value6 != "NaN")
                        {
                            avg_sale_rs.June = Convert.ToDouble(value6);
                        }
                        value7 = Convert.ToString((netsale_rs.July / netsale_nosnode.July).ToString("0.#"));
                        if (value7 != "NaN")
                        {
                            avg_sale_rs.July = Convert.ToDouble(value7);
                        }
                        value8 = Convert.ToString((netsale_rs.Aug / netsale_nosnode.Aug).ToString("0.#"));
                        if (value8 != "NaN")
                        {
                            avg_sale_rs.Aug = Convert.ToDouble(value8);
                        }
                        value9 = Convert.ToString((netsale_rs.Sept / netsale_nosnode.Sept).ToString("0.#"));
                        if (value9 != "NaN")
                        {
                            avg_sale_rs.Sept = Convert.ToDouble(value9);
                        }
                        value10 = Convert.ToString((netsale_rs.Oct / netsale_nosnode.Oct).ToString("0.#"));
                        if (value10 != "NaN")
                        {
                            avg_sale_rs.Oct = Convert.ToDouble(value10);
                        }
                        value11 = Convert.ToString((netsale_rs.Nov / netsale_nosnode.Nov).ToString("0.#"));
                        if (value11 != "NaN")
                        {
                            avg_sale_rs.Nov = Convert.ToDouble(value11);
                        }
                        value12 = Convert.ToString((netsale_rs.Dec / netsale_nosnode.Dec).ToString("0.#"));
                        if (value12 != "NaN")
                        {
                            avg_sale_rs.Dec = Convert.ToDouble(value12);
                        }
                        value13 = Convert.ToString((netsale_rs.Total_mounthcount / netsale_nosnode.Total_mounthcount).ToString("0.#"));
                        if (value13 != "NaN")
                        {
                            avg_sale_rs.Total_mounthcount = Convert.ToDouble(value13);
                        }
                        lstOrdertext2.Add(avg_sale_rs);


                        MonthReport total_money = new MonthReport();
                        //total_money.Number = 8;
                        total_money.particulars = "Total Money Received(Rs)";
                        total_money.Jan = bankamount[0];
                        total_money.Feb = bankamount[1];
                        total_money.March = bankamount[2];
                        total_money.April = bankamount[3];
                        total_money.May = bankamount[4];
                        total_money.June = bankamount[5];
                        total_money.July = bankamount[6];
                        total_money.Aug = bankamount[7];
                        total_money.Sept = bankamount[8];
                        total_money.Oct = bankamount[9];
                        total_money.Nov = bankamount[10];
                        total_money.Dec = bankamount[11];
                        total_money.Total_mounthcount = total_money.Jan + total_money.Feb + total_money.March + total_money.April + total_money.May + total_money.June + total_money.July + total_money.Aug + total_money.Sept + total_money.Oct + total_money.Nov + total_money.Dec;
                        lstOrdertext2.Add(total_money);

                        MonthReport advertise_fee = new MonthReport();
                        //advertise_fee.Number = 9;
                        advertise_fee.particulars = "Sales(Rs)";
                        advertise_fee.Jan = adversite[0];
                        advertise_fee.Feb = adversite[1];
                        advertise_fee.March = adversite[2];
                        advertise_fee.April = adversite[3];
                        advertise_fee.May = adversite[4];
                        advertise_fee.June = adversite[5];
                        advertise_fee.July = adversite[6];
                        advertise_fee.Aug = adversite[7];
                        advertise_fee.Sept = adversite[8];
                        advertise_fee.Oct = adversite[9];
                        advertise_fee.Nov = adversite[10];
                        advertise_fee.Dec = adversite[11];
                        advertise_fee.Total_mounthcount = advertise_fee.Jan + advertise_fee.Feb + advertise_fee.March + advertise_fee.April + advertise_fee.May + advertise_fee.June + advertise_fee.July + advertise_fee.Aug + advertise_fee.Sept + advertise_fee.Oct + advertise_fee.Nov + advertise_fee.Dec;
                        lstOrdertext2.Add(advertise_fee);

                        var get_settlement_type = dba.m_settlement_fee.Where(a => a.remarks == "Amazon" && a.id != 7 && a.id != 8 && a.id != 9 && a.id != 10 && a.id != 11).ToList();
                        if (get_settlement_type != null)
                        {
                            //double[][] commisionamount_DblArr = new double[get_settlement_type.Count][];

                            double[] commisionamount_DblArr = new double[12];
                            foreach (var sett_item in get_settlement_type)
                            {
                                if (sett_item.return_fee == "Principal")
                                {

                                }
                                commisionamount_DblArr = new double[12];
                                for (int i = Convert.ToInt32(ddl_MonthSelecter_from); i <= Convert.ToInt32(ddl_MonthSelecter_to); i++)
                                {
                                    string month = i.ToString();
                                    if (month.Length == 1)
                                        month = "0" + month;
                                    var dateformat = (ddl_yearSelecter + "-" + month + "-");
                                    var lstcom = lst_expense.Where(a => Convert.ToDateTime(a.settlement_datetime).Date.ToString().Contains(dateformat) && a.expense_type_id == sett_item.id).ToList();
                                    //commisionamount[i] = lst.Count();
                                    foreach (var item in lstcom)
                                    {
                                        double hhh = 0;
                                        if (item.expense_amount.ToString().Contains('-'))
                                        {
                                            hhh = Convert.ToDouble(item.expense_amount) * (-1);
                                        }
                                        else
                                        {
                                            hhh = Convert.ToDouble(item.expense_amount);
                                        }

                                        double hhhjjj = 0;
                                        int id = item.id;
                                        var taxtype = dba.tbl_tax.Where(a => a.tbl_referneced_id == id && a.reference_type == 3 || a.reference_type == 2).FirstOrDefault();
                                        if (taxtype != null)
                                        {
                                            hhhjjj += Convert.ToDouble(taxtype.sgst_amount + taxtype.Igst_amount + taxtype.CGST_amount);
                                        }
                                        if (item.t_transactionType_id == 1)
                                            commisionamount_DblArr[i - 1] += (hhhjjj + hhh); //hhh;//hhhjjj + hhh;
                                        else
                                            commisionamount_DblArr[i - 1] -= (hhhjjj + hhh); //hhh;//hhhjjj + hhh;

                                    }
                                }//end month for loop
                                MonthReport expense = new MonthReport();
                                //expense.Number = 10;
                                expense.particulars = sett_item.return_fee;// "Sales(Nos)";
                                expense.Jan = commisionamount_DblArr[0];
                                expense.Feb = commisionamount_DblArr[1];
                                expense.March = commisionamount_DblArr[2];
                                expense.April = commisionamount_DblArr[3];
                                expense.May = commisionamount_DblArr[4];
                                expense.June = commisionamount_DblArr[5];
                                expense.July = commisionamount_DblArr[6];
                                expense.Aug = commisionamount_DblArr[7];
                                expense.Sept = commisionamount_DblArr[8];
                                expense.Oct = commisionamount_DblArr[9];
                                expense.Nov = commisionamount_DblArr[10];
                                expense.Dec = commisionamount_DblArr[11];
                                expense.Total_mounthcount = expense.Jan + expense.Feb + expense.March + expense.April + expense.May + expense.June + expense.July + expense.Aug + expense.Sept + expense.Oct + expense.Nov + expense.Dec;
                                if (expense.Total_mounthcount != 0)
                                    lstOrdertext2.Add(expense);
                            }//end expense type for loop

                        }
                    }
                    #endregion


                }
            }// end of try block
            catch (Exception ex)
            {
            }// end of catch block


            showcol.ShowJan = ShowMonthColumn[0];
            showcol.ShowFeb = ShowMonthColumn[1];
            showcol.ShowMarch = ShowMonthColumn[2];
            showcol.ShowApril = ShowMonthColumn[3];
            showcol.ShowMay = ShowMonthColumn[4];
            showcol.ShowJune = ShowMonthColumn[5];
            showcol.ShowJuly = ShowMonthColumn[6];
            showcol.ShowAug = ShowMonthColumn[7];
            showcol.ShowSept = ShowMonthColumn[8];
            showcol.ShowOct = ShowMonthColumn[9];
            showcol.ShowNov = ShowMonthColumn[10];
            showcol.ShowDec = ShowMonthColumn[11];
            RetModel.SowMonthColumn = showcol;
            RetModel.MonthReport = lstOrdertext2;


            #region ExportExcel
            /////----------------------------------Export Excel----------------------------------
            string sendOnMail = form["sendmail"];
            string value_1 = form["command"];
            if (value_1 == "Export" || sendOnMail == "Send On Mail")
            {
                if (ddl_export != null)
                {
                    bool mail = false;
                    if (sendOnMail != null) mail = true;
                    string EmailID = form["txt_email"];

                    DataTable header = new DataTable();
                    string colum = " ";
                    for (int t = 0; t < 6; t++)
                    {
                        header.Columns.Add(colum);
                        colum = colum + " ";
                    }
                    DataRow errow = header.NewRow();
                    errow[2] = " ";
                    header.Rows.Add(errow);
                    DataRow drow = header.NewRow();
                    drow[1] = "From Date";
                    drow[2] = txt_from;
                    drow[4] = "Report Taken On";
                    drow[5] = DateTime.Now.ToString("dd/MMM/yyyy");
                    header.Rows.Add(drow);
                    DataRow brow = header.NewRow();
                    brow[1] = "To Date";
                    brow[2] = txt_to;
                    header.Rows.Add(brow);
                    DataRow erow = header.NewRow();
                    erow[2] = " ";
                    header.Rows.Add(erow);
                    DataTable export_dt = new DataTable();
                    export_dt = cf.MonthReportDatatable(lstOrdertext2, showcol);
                    int type = Convert.ToInt32(ddl_export);
                    if (export_dt.Rows.Count > 0)
                    {
                        int r = cf.Export("Month Report", header, export_dt, Response, type, mail, EmailID);
                    }
                    else ViewData["Message"] = "There are No records In Table";
                }
                else ViewData["Message"] = "Please Select Export Type";
            }





            ////----------------------------------------End---------------------------------------//
            #endregion

            return View(RetModel);
        }

        #endregion

        #region MasterInventoryVoucher
        /// <summary>
        /// this is for getting all Inventory List
        /// </summary>
        /// <param name="from"></param>
        /// <param name="ddl_marketplace"></param>
        /// <param name="?"></param>
        /// <returns></returns>
        public ActionResult MasterInventoryReport(FormCollection from, int? ddl_marketplace, int? ddl_export)
        {

            cf = new comman_function();
            if (!cf.session_check())
                return RedirectToAction("Login", "Home");
            int sellers_id = Convert.ToInt32(Session["SellerID"]);
            string CompanyName = Convert.ToString(Session["UserName"]);
            List<SelectListItem> lst_loc = cf.GetSrchMarketPalceList(sellers_id);
            ViewData["MarKetPlaceList"] = lst_loc;
            ViewData["ExportList"] = cf.GetExportList();

            List<SaleReport> lstOrdertext2 = new List<SaleReport>();
            try
            {
                if (ddl_marketplace != null && ddl_marketplace != 0)
                {
                    SalesVoucher obj = new SalesVoucher();
                    //obj.Get_Sales_Voucher_Report_New(lstOrdertext2, txt_from, txt_to, sellers_id);
                    /////----------------------------------Export Excel----------------------------------
                    string sendOnMail = from["sendmail"];
                    string value1 = from["command"];
                    if (value1 == "Export" || sendOnMail == "Send On Mail")
                    {
                        if (ddl_export != null && ddl_export == 3)
                        {
                            #region XML
                            int message = 0;
                            XmlDocument xmlDoc = new XmlDocument();
                            MasterInventoryXML objxml = new MasterInventoryXML();
                            // var xmldata = objxml.SalesVoucherXML(lstOrdertext2,CompanyName);
                            var xmldata = objxml.MasterVoucherXML(ddl_marketplace, sellers_id, CompanyName);
                            Response.Buffer = true;
                            Response.Write(xmldata);
                            Response.AddHeader("Content-disposition", "attachment; filename=" + " Inventory Master Tally Voucher" + ".xml");
                            Response.ContentType = "application/octet-stream";
                            Response.End();
                            message = 1;
                            #endregion
                        }
                        else if (ddl_export != null && ddl_export == 1)
                        {
                            bool mail = false;
                            if (sendOnMail != null) mail = true;
                            string EmailID = from["txt_email"];

                            DataTable header = new DataTable();
                            string colum = " ";
                            for (int t = 0; t < 6; t++)
                            {
                                header.Columns.Add(colum);
                                colum = colum + " ";
                            }

                            DataTable export_dt = new DataTable();
                            export_dt = cf.CreateSaleVoucherDatatable(lstOrdertext2);
                            int type = Convert.ToInt32(ddl_export);
                            if (export_dt.Rows.Count > 0)
                            {
                                int r = cf.Export("Inventory Voucher Report", header, export_dt, Response, type, mail, EmailID);
                            }
                            else ViewData["Message"] = "There are No records In Table";
                        }
                        else ViewData["Message"] = "Please Select Export Type";
                    }
                    //----------------------------------------END-----------------------------------------////
                }
            }// end of try block
            catch (Exception ex)
            {
            }
            return View();
        }

        #endregion

        #region MasterLedgerVoucher
        /// <summary>
        /// this is for Master Ledger Vocher 
        /// </summary>
        /// <param name="from"></param>
        /// <param name="ddl_marketplace"></param>
        /// <param name="ddl_export"></param>
        /// <returns></returns>
        public ActionResult MasterLedgerVoucher(FormCollection from, int? ddl_marketplace, int? ddl_export)
        {

            cf = new comman_function();
            if (!cf.session_check())
                return RedirectToAction("Login", "Home");
            int sellers_id = Convert.ToInt32(Session["SellerID"]);
            string CompanyName = Convert.ToString(Session["UserName"]);
            List<SelectListItem> lst_loc = cf.GetSrchMarketPalceList(sellers_id);
            ViewData["MarKetPlaceList"] = lst_loc;
            ViewData["ExportList"] = cf.GetExportList();
            List<SaleReport> lstOrdertext2 = new List<SaleReport>();
            try
            {
                if (ddl_marketplace != null && ddl_marketplace != 0)
                {
                    SalesVoucher obj = new SalesVoucher();
                    //obj.Get_Sales_Voucher_Report_New(lstOrdertext2, txt_from, txt_to, sellers_id);
                    /////----------------------------------Export Excel----------------------------------
                    string sendOnMail = from["sendmail"];
                    string value1 = from["command"];
                    if (value1 == "Export" || sendOnMail == "Send On Mail")
                    {
                        if (ddl_export != null && ddl_export == 3)
                        {
                            #region XML
                            int message = 0;
                            XmlDocument xmlDoc = new XmlDocument();
                            MasterLedgerVoucherXML objxml = new MasterLedgerVoucherXML();
                            // var xmldata = objxml.SalesVoucherXML(lstOrdertext2,CompanyName);
                            var xmldata = objxml.MasterledgervoucherXML(ddl_marketplace, sellers_id, CompanyName);
                            Response.Buffer = true;
                            Response.Write(xmldata);
                            Response.AddHeader("Content-disposition", "attachment; filename=" + " Master Ledger Voucher" + ".xml");
                            Response.ContentType = "application/octet-stream";
                            Response.End();
                            message = 1;
                            #endregion
                        }
                        else if (ddl_export != null && ddl_export == 1)
                        {
                            bool mail = false;
                            if (sendOnMail != null) mail = true;
                            string EmailID = from["txt_email"];

                            DataTable header = new DataTable();
                            string colum = " ";
                            for (int t = 0; t < 6; t++)
                            {
                                header.Columns.Add(colum);
                                colum = colum + " ";
                            }

                            DataTable export_dt = new DataTable();
                            export_dt = cf.CreateSaleVoucherDatatable(lstOrdertext2);
                            int type = Convert.ToInt32(ddl_export);
                            if (export_dt.Rows.Count > 0)
                            {
                                int r = cf.Export("Master Ledger Voucher Report", header, export_dt, Response, type, mail, EmailID);
                            }
                            else ViewData["Message"] = "There are No records In Table";
                        }
                        else ViewData["Message"] = "Please Select Export Type";
                    }
                    //----------------------------------------END-----------------------------------------////
                }
            }// end of try block
            catch (Exception ex)
            {
            }
            return View();
        }

        #endregion

        #region GSTReport
        public ActionResult GstReport(FormCollection form, int? ddl_market_place, DateTime? txt_from, DateTime? txt_to, int? ddl_export)
        {
            cf = new comman_function();
            if (!cf.session_check())
                return RedirectToAction("Login", "Home");
            int sellers_id = Convert.ToInt32(Session["SellerID"]);
            ViewData["ExportList"] = cf.GetExcelExportList();

            List<SelectListItem> lst1_loc = cf.GetMarketPlace(sellers_id);
            ViewData["MarKetPlace"] = lst1_loc;

            List<SaleReport> lstOrdertext2 = new List<SaleReport>();
            SaleReport view_salereport = null;
            try
            {
                dba = new SellerContext();
                dba.Configuration.AutoDetectChangesEnabled = false;
                if (txt_from != null && txt_to != null)
                {

                    string txtfrom = Convert.ToDateTime(txt_from).ToString("yyyy-MM-dd");
                    string txtto = Convert.ToDateTime(txt_to).ToString("yyyy-MM-dd");
                    string connectionstring = ConfigurationManager.ConnectionStrings["SellerConnection"].ConnectionString;
                    MySqlConnection con = new MySqlConnection(connectionstring);


                    var get_unique_count_details = "select tbl_customer_details.State_Region as State,SUM(tbl_sales_order_details.item_price_amount)as TotalAmount,tbl_tax.sgst_tax as SGST,tbl_tax.cgst_tax as CGST ,tbl_tax.igst_tax as IGST from tbl_sales_order left join tbl_customer_details on tbl_sales_order.tbl_Customer_Id  = tbl_customer_details .id left join tbl_sales_order_details on tbl_sales_order.id = tbl_sales_order_details .tbl_sales_order_id left join tbl_tax on tbl_sales_order_details .id = tbl_tax.tbl_referneced_id and reference_type =3 where tbl_sales_order.tbl_sellers_id =" + sellers_id + " and tbl_sales_order.tbl_Marketplace_Id =" + ddl_market_place + " and tbl_sales_order.order_status != 'Canceled' GROUP BY tbl_customer_details.State_Region ,tbl_tax.igst_tax,tbl_tax.sgst_tax";
                    //var get_unique_count_details = "SELECT sku_no,product_name, id, tbl_sales_order_id, count( * ) AS total FROM tbl_sales_order_details WHERE sku_no IS NOT NULL AND tbl_seller_id =" + sellers_id + " GROUP BY sku_no ORDER BY count( * ) DESC LIMIT 5";
                    MySqlDataAdapter da1 = new MySqlDataAdapter(get_unique_count_details, con);

                    DataTable dtexpense1 = new DataTable();
                    da1.Fill(dtexpense1);
                    da1.Dispose();
                    con.Open();
                    MySqlCommand cmd1 = null;
                    for (int i = 0; i < dtexpense1.Rows.Count; i++)
                    {
                        string IGST = Convert.ToString(dtexpense1.Rows[i]["IGST"]);
                        string SGST = Convert.ToString(dtexpense1.Rows[i]["SGST"]);
                        string Gstn_No = "";
                        var details = dba.tbl_sellermarketplace.Where(a => a.m_marketplace_id == ddl_market_place && a.tbl_seller_id == sellers_id && a.isactive==1).FirstOrDefault();
                        if(details != null)
                        {
                            //Gstn_No = details.GSTN_No;
                        }
                        if (IGST != "" && SGST != "")
                        {
                            view_salereport = new SaleReport();
                            if (IGST != "0" && IGST != "")
                            {

                                view_salereport.PlaceSupply = Convert.ToString(dtexpense1.Rows[i]["State"]);// i.SKU;
                                view_salereport.Rate = Convert.ToDouble(dtexpense1.Rows[i]["IGST"]);
                                view_salereport.ToatlAmountGST = Convert.ToDouble(dtexpense1.Rows[i]["TotalAmount"]);
                                view_salereport.GSTType = "E";
                                view_salereport.ECommerceGSTIN = Gstn_No;
                                view_salereport.ApplicableTaxrate = "";
                            }
                            else
                            {
                                double sgst = Convert.ToDouble(dtexpense1.Rows[i]["SGST"]);
                                double cgst = Convert.ToDouble(dtexpense1.Rows[i]["CGST"]);
                                view_salereport.PlaceSupply = Convert.ToString(dtexpense1.Rows[i]["State"]);// i.SKU;
                                view_salereport.Rate = sgst + cgst;
                                view_salereport.ToatlAmountGST = Convert.ToDouble(dtexpense1.Rows[i]["TotalAmount"]);
                                view_salereport.GSTType = "E";
                                view_salereport.ECommerceGSTIN = Gstn_No;
                                view_salereport.ApplicableTaxrate = "";
                            }

                            lstOrdertext2.Add(view_salereport);
                        }
                    }


                    /////----------------------------------Export Excel----------------------------------

                    string sendOnMail = form["sendmail"];
                    string value1 = form["command"];
                    if (value1 == "Export" || sendOnMail == "Send On Mail")
                    {
                        if (ddl_export != null)
                        {
                            bool mail = false;
                            if (sendOnMail != null) mail = true;
                            string EmailID = form["txt_email"];

                            DataTable header = new DataTable();
                            string colum = " ";
                            for (int t = 0; t < 6; t++)
                            {
                                header.Columns.Add(colum);
                                colum = colum + " ";
                            }
                            DataRow errow = header.NewRow();
                            errow[2] = " ";
                            header.Rows.Add(errow);
                            DataRow drow = header.NewRow();
                            drow[1] = "From Date";
                            drow[2] = txt_from;
                            drow[4] = "Report Taken On";
                            drow[5] = DateTime.Now.ToString("dd/MMM/yyyy");
                            header.Rows.Add(drow);
                            DataRow brow = header.NewRow();
                            brow[1] = "To Date";
                            brow[2] = txt_to;
                            header.Rows.Add(brow);
                            DataRow erow = header.NewRow();
                            erow[2] = " ";
                            header.Rows.Add(erow);
                            DataTable export_dt = new DataTable();
                            if (ddl_market_place == 3)
                            {
                                export_dt = cf.CreateGSTDatatable(lstOrdertext2);
                            }

                            int type = Convert.ToInt32(ddl_export);
                            if (export_dt.Rows.Count > 0)
                            {
                                int r = cf.Export("GST Report", header, export_dt, Response, type, mail, EmailID);
                            }
                            else ViewData["Message"] = "There are No records In Table";
                        }
                        else ViewData["Message"] = "Please Select Export Type";
                    }
                    //----------------------------------------END-----------------------------------------////
                }
            }// end of try block
            catch (Exception ex)
            {
            }
            return View(lstOrdertext2);
        }

        #endregion

        public ActionResult SettlementReport(FormCollection form, int? ddl_market_place, DateTime? txt_from, DateTime? txt_to, int? ddl_export)
        {
            cf = new comman_function();
            if (!cf.session_check())
                return RedirectToAction("Login", "Home");
            int sellers_id = Convert.ToInt32(Session["SellerID"]);
            ViewData["ExportList"] = cf.GetExcelExportList();

            List<SelectListItem> lst1_loc = cf.GetMarketPlace(sellers_id);
            ViewData["MarKetPlace"] = lst1_loc;

            List<SaleReport> lstOrdertext2 = new List<SaleReport>();
            SaleReport view_salereport = null;

            try
            {
                if (txt_from != null && txt_to != null)
                {
                    var GetSettlementDetail = (from ob_tbl_settlement in dba.tbl_settlement_upload
                                               select new
                                               {
                                                   ob_tbl_settlement = ob_tbl_settlement
                                               }).Where(a => a.ob_tbl_settlement.tbl_seller_id == sellers_id && a.ob_tbl_settlement.market_place_id == ddl_market_place).ToList();

                    if (GetSettlementDetail != null)
                    {
                        GetSettlementDetail = GetSettlementDetail.Where(a => a.ob_tbl_settlement.deposit_date >= txt_from && a.ob_tbl_settlement.deposit_date <= txt_to).ToList();
                    }


                    foreach (var item in GetSettlementDetail)
                    {
                        view_salereport = new SaleReport();
                        string sett_reference_no = item.ob_tbl_settlement.settlement_refernece_no;
                        var getbankdetails = dba.tbl_imp_bank_transfers.Where(a => a.tbl_settlement_upload_id == item.ob_tbl_settlement.Id).FirstOrDefault();
                        if (getbankdetails != null)
                        {
                            view_salereport.BankAmount = Convert.ToDouble(getbankdetails.amount);
                        }
                        view_salereport.ReferenceID = item.ob_tbl_settlement.settlement_refernece_no;
                        view_salereport.SettlementDate = Convert.ToDateTime(item.ob_tbl_settlement.deposit_date).ToString("yyyy-MM-dd");
                        view_salereport.CurrentReserveAmount = Convert.ToDouble(item.ob_tbl_settlement.current_reserve_amount);
                        view_salereport.PreviousReserveAmount = Convert.ToDouble(item.ob_tbl_settlement.previous_reserve_amount);

                        string connectionstring1 = ConfigurationManager.ConnectionStrings["SellerConnection"].ConnectionString;
                        MySqlConnection con1 = new MySqlConnection(connectionstring1);
                        var get_settlement_price = "select sum(`principal_price`+`product_tax`+`shipping_price`+`shipping_tax`)as totalprice ,sum(giftwrap_price)as giftprice,sum(`giftwarp_tax`) as gifttax from tbl_settlement_order where tbl_seller_id =" + sellers_id + " and settlement_id =" + sett_reference_no + "";
                        MySqlDataAdapter da = new MySqlDataAdapter(get_settlement_price, con1);
                        DataTable dtexpense = new DataTable();
                        da.Fill(dtexpense);
                        da.Dispose();
                        con1.Open();
                        MySqlCommand cmd = null;
                        for (int i = 0; i < dtexpense.Rows.Count; i++)
                        {
                            double gift_price = 0, gift_tax = 0;
                            double productprice = Convert.ToInt16(dtexpense.Rows[i]["totalprice"]);
                            string giftprice = Convert.ToString(dtexpense.Rows[i]["giftprice"]);
                            if (giftprice != "" && giftprice != null)
                            {
                                gift_price = Convert.ToInt16(dtexpense.Rows[i]["giftprice"]);
                            }
                            string gifttax = Convert.ToString(dtexpense.Rows[i]["gifttax"]);
                            if (gifttax != null && gifttax != "")
                            {
                                gift_tax = Convert.ToInt16(dtexpense.Rows[i]["gifttax"]);
                            }
                            view_salereport.OrderAmount = productprice + gift_price + gift_tax;
                        }

                        //string connectionstring2 = ConfigurationManager.ConnectionStrings["SellerConnection"].ConnectionString;
                        //MySqlConnection con2 = new MySqlConnection(connectionstring2);
                        var get_refund_price = "select sum(`amount_per_unit`+`product_tax`+`shipping_price`+`shipping_tax`)as refundprice ,sum(`Giftwrap_price`)as refundgiftprice,sum(`gift_wrap_tax`) as refundgifttax from tbl_order_history where tbl_seller_id = " + sellers_id + " and settlement_id=" + sett_reference_no + "";
                        MySqlDataAdapter da2 = new MySqlDataAdapter(get_refund_price, con1);
                        DataTable dtexpense2 = new DataTable();
                        da2.Fill(dtexpense2);
                        da2.Dispose();
                        //con2.Open();
                        MySqlCommand cmd2 = null;
                        for (int i = 0; i < dtexpense2.Rows.Count; i++)
                        {
                            double refundgift_price = 0, refundgift_tax = 0;
                            double productprice = Convert.ToInt16(dtexpense2.Rows[i]["refundprice"]);
                            string refundgiftprice = Convert.ToString(dtexpense2.Rows[i]["refundgiftprice"]);
                            if (refundgiftprice != "" && refundgiftprice != null)
                            {
                                refundgift_price = Convert.ToInt16(dtexpense2.Rows[i]["refundgiftprice"]);
                            }
                            string refundgifttax = Convert.ToString(dtexpense2.Rows[i]["refundgifttax"]);
                            if (refundgifttax != null && refundgifttax != "")
                            {
                                refundgift_tax = Convert.ToInt16(dtexpense2.Rows[i]["refundgifttax"]);
                            }
                            view_salereport.refundTotal = productprice + refundgift_price + refundgift_tax;
                        }


                        //string connectionstring = ConfigurationManager.ConnectionStrings["SellerConnection"].ConnectionString;
                        //MySqlConnection con = new MySqlConnection(connectionstring);
                        var get_unique_count_details = "select expense_type_id,SUM(expense_amount)as amount from m_tbl_expense where reference_number = " + sett_reference_no + " and tbl_seller_id=" + sellers_id + " GROUP BY expense_type_id";
                        MySqlDataAdapter da1 = new MySqlDataAdapter(get_unique_count_details, con1);
                        DataTable dtexpense1 = new DataTable();
                        da1.Fill(dtexpense1);
                        da1.Dispose();
                        // con.Open();
                        MySqlCommand cmd1 = null;
                        for (int i = 0; i < dtexpense1.Rows.Count; i++)
                        {
                            int id = Convert.ToInt16(dtexpense1.Rows[i]["expense_type_id"]);
                            double amt = Convert.ToDouble(dtexpense1.Rows[i]["amount"]);
                            decimal abcd1 = Convert.ToDecimal(amt);
                            decimal result1 = decimal.Round(abcd1, 2, MidpointRounding.AwayFromZero);

                            var get_expdetails = dba.m_settlement_fee.Where(a => a.id == id).FirstOrDefault();
                            if (get_expdetails != null)
                            {
                                string nam = get_expdetails.return_fee;
                                if (nam == "Commission")
                                {
                                    view_salereport.ActualCommission = Convert.ToDouble(result1);
                                }
                                else if (nam == "Fixed closing fee")
                                {
                                    view_salereport.ActualFixedClosingFee = Convert.ToDouble(result1);
                                }
                                else if (nam == "Refund commission")
                                {
                                    view_salereport.ActualRefundCommission = Convert.ToDouble(result1);
                                }
                                else if (nam == "Shipping commission")
                                {
                                    view_salereport.ActualShippingCommision = Convert.ToDouble(result1);
                                }
                                else if (nam == "Easy Ship weight handling fees")
                                {
                                    view_salereport.ActualAmazonEasyShipCharges = Convert.ToDouble(result1);
                                }
                            }
                        }
                        con1.Close();
                        lstOrdertext2.Add(view_salereport);
                    }// end of for each loop
                    /////----------------------------------Export Excel----------------------------------

                    string sendOnMail = form["sendmail"];
                    string value1 = form["command"];
                    if (value1 == "Export" || sendOnMail == "Send On Mail")
                    {
                        if (ddl_export != null)
                        {
                            bool mail = false;
                            if (sendOnMail != null) mail = true;
                            string EmailID = form["txt_email"];

                            DataTable header = new DataTable();
                            string colum = " ";
                            for (int t = 0; t < 6; t++)
                            {
                                header.Columns.Add(colum);
                                colum = colum + " ";
                            }
                            DataRow errow = header.NewRow();
                            errow[2] = " ";
                            header.Rows.Add(errow);
                            DataRow drow = header.NewRow();
                            drow[1] = "From Date";
                            drow[2] = txt_from;
                            drow[4] = "Report Taken On";
                            drow[5] = DateTime.Now.ToString("dd/MMM/yyyy");
                            header.Rows.Add(drow);
                            DataRow brow = header.NewRow();
                            brow[1] = "To Date";
                            brow[2] = txt_to;
                            header.Rows.Add(brow);
                            DataRow erow = header.NewRow();
                            erow[2] = " ";
                            header.Rows.Add(erow);
                            DataTable export_dt = new DataTable();
                            export_dt = cf.CreateSettlementDetailedDatatable(lstOrdertext2);
                            int type = Convert.ToInt32(ddl_export);
                            if (export_dt.Rows.Count > 0)
                            {
                                int r = cf.Export("Settlement Detailed Report", header, export_dt, Response, type, mail, EmailID);
                            }
                            else ViewData["Message"] = "There are No records In Table";
                        }
                        else ViewData["Message"] = "Please Select Export Type";
                    }
                    //----------------------------------------END-----------------------------------------////
                }// end of if 
            }// end of try block
            catch (Exception ex)
            {
            }
            return View(lstOrdertext2);
        }

    }
}
