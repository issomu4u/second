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
    public class SalesVoucher
    {
        public SellerContext dba = new SellerContext();
        public SellerAdminContext db = new SellerAdminContext();


        public string Get_Sales_Voucher_Report_New(List<SaleReport> lstOrdertext2, DateTime? txt_from, DateTime? txt_to, int sellers_id)
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


                    foreach (var item in GetSaleOrderDetail)
                    {
                        if (item.ob_tbl_sales_order.amazon_order_id == "402-2104931-0457104")
                        {
                        }
                        int iRepeatDetailData = 0;
                        SaleReport view_salereport = new SaleReport();
                        int voucher_runningno = 0;
                        var get_seller_setting = dba.tbl_seller_setting.Where(a => a.tbl_seller_id == sellers_id && a.type == 3).FirstOrDefault();
                        if (get_seller_setting != null)
                        {
                            voucher_runningno = Convert.ToInt16(get_seller_setting.current_running_no);
                        }

                        view_salereport.Sett_orderDate = Convert.ToDateTime(item.ob_tbl_sales_order.purchase_date).ToString("yyyy-MM-dd");
                        view_salereport.VoucherNumber = "Amazon/Sales/" + voucher_runningno;
                        view_salereport.ExpenseName = "Sale Customer Amazon";
                        view_salereport.OrderID = item.ob_tbl_sales_order.amazon_order_id;
                        //view_salereport.refund_SumOrder = item.ob_tbl_sales_order.bill_amount;

                        //////////////////// COLLECT ORDER DETAIL DATA ///////////////
                        var get_saleorder_details = dba.tbl_sales_order_details.Where(a => a.tbl_sales_order_id == item.ob_tbl_sales_order.id && a.tbl_seller_id == sellers_id).ToList();
                        SaleReport view_salereport1 = new SaleReport();
                        view_salereport1.Sett_orderDate = Convert.ToDateTime(item.ob_tbl_sales_order.purchase_date).ToString("yyyy-MM-dd");
                        view_salereport1.VoucherNumber = "Amazon/Sales/" + voucher_runningno;
                        view_salereport1.ExpenseName = "Sales A/c";
                        view_salereport1.OrderID = item.ob_tbl_sales_order.amazon_order_id;
                        double item_price_amount = 0;
                        double igst_amount = 0;
                        double cgst_amount = 0;
                        double sgst_amount = 0;
                        double promotiondiscount = 0;

                        SaleReport view_salereport4 = new SaleReport();
                        view_salereport4.Sett_orderDate = Convert.ToDateTime(item.ob_tbl_sales_order.purchase_date).ToString("yyyy-MM-dd");
                        view_salereport4.VoucherNumber = "Amazon/Sales/" + voucher_runningno;
                        view_salereport4.ExpenseName = "IGST";
                        view_salereport4.OrderID = item.ob_tbl_sales_order.amazon_order_id;

                        
                        SaleReport view_salereport2 = new SaleReport();
                        view_salereport2.Sett_orderDate = Convert.ToDateTime(item.ob_tbl_sales_order.purchase_date).ToString("yyyy-MM-dd");
                        view_salereport2.VoucherNumber = "Amazon/Sales/" + voucher_runningno;
                        view_salereport2.ExpenseName = "CGST";
                        view_salereport2.OrderID = item.ob_tbl_sales_order.amazon_order_id;

                        SaleReport view_salereport3 = new SaleReport();
                        view_salereport3.Sett_orderDate = Convert.ToDateTime(item.ob_tbl_sales_order.purchase_date).ToString("yyyy-MM-dd");
                        view_salereport3.VoucherNumber = "Amazon/Sales/" + voucher_runningno;
                        view_salereport3.ExpenseName = "SGST";
                        view_salereport3.OrderID = item.ob_tbl_sales_order.amazon_order_id;

                        foreach (var itemDetail in get_saleorder_details)
                        {
                            int deatil_id = itemDetail.id;
                            double ord_detail_item_price_amount = itemDetail.item_price_amount;
                            double ord_detail_shipping_amount = itemDetail.shipping_price_Amount;
                            double ord_detail_shipping_discount_amount = itemDetail.shipping_discount_amt;
                            double ord_detail_shipping_tax_amount = itemDetail.shipping_tax_Amount;
                            double? ord_detail_shipping_discount_tax_amount = itemDetail.shipping_discount_tax_amount;
                            double ord_detail_giftwrap =Convert.ToDouble(itemDetail.giftwrapprice_amount);
                            double? ord_giftwraptax = itemDetail.giftwraptax_amount;
                            promotiondiscount = promotiondiscount + itemDetail.promotion_amount;
                            //view_salereport1.SumOrder = itemDetail.item_price_amount;
                            item_price_amount = item_price_amount + ord_detail_item_price_amount;// +ord_detail_shipping_amount + ord_detail_shipping_tax_amount;

                            ////////////////////// GET TAX EXPENSES //////////////////

                            var get_taxdetails = dba.tbl_tax.Where(a => a.tbl_seller_id == sellers_id && a.reference_type == 3 && a.tbl_referneced_id == deatil_id).FirstOrDefault();
                            if (get_taxdetails != null)
                            {
                                if (get_taxdetails.Igst_amount < 0)
                                {
                                    igst_amount = igst_amount + (Convert.ToDouble(get_taxdetails.Igst_amount)) * (-1);
                                    //view_salereport4.SumOrder = (Convert.ToDouble(get_taxdetails.Igst_amount)) * (-1);
                                }
                                else
                                {
                                    igst_amount = igst_amount + Convert.ToDouble(get_taxdetails.Igst_amount);
                                    //view_salereport4.SumOrder = Convert.ToDouble(get_taxdetails.Igst_amount);
                                }

                                if (get_taxdetails.CGST_amount < 0)
                                {
                                    cgst_amount = cgst_amount + (Convert.ToDouble(get_taxdetails.CGST_amount)) * (-1);
                                    //view_salereport2.SumOrder = (Convert.ToDouble(get_taxdetails.CGST_amount)) * (-1);
                                }
                                else
                                {
                                    cgst_amount = cgst_amount + (Convert.ToDouble(get_taxdetails.CGST_amount));
                                    //view_salereport2.SumOrder = Convert.ToDouble(get_taxdetails.CGST_amount);
                                }

                                if (get_taxdetails.sgst_amount < 0)
                                {
                                    sgst_amount = sgst_amount + (Convert.ToDouble(get_taxdetails.sgst_amount)) * (-1);
                                    //view_salereport3.SumOrder = (Convert.ToDouble(get_taxdetails.sgst_amount)) * (-1);
                                }
                                else
                                {
                                    sgst_amount = sgst_amount + (Convert.ToDouble(get_taxdetails.sgst_amount));
                                    //view_salereport3.SumOrder = Convert.ToDouble(get_taxdetails.sgst_amount);
                                }
                            }

                            iRepeatDetailData++;
                        }
                        view_salereport.refund_SumOrder = item.ob_tbl_sales_order.bill_amount;
                        if ( promotiondiscount != null ) 
                            view_salereport.refund_SumOrder = item.ob_tbl_sales_order.bill_amount - promotiondiscount;

                        view_salereport.Narration = Convert.ToDateTime(item.ob_tbl_sales_order.purchase_date).ToString("yyyy-MM-dd"); ;
                        lstOrdertext2.Add(view_salereport);

                        view_salereport1.SumOrder = item_price_amount;
                        view_salereport1.Narration = Convert.ToDateTime(item.ob_tbl_sales_order.purchase_date).ToString("yyyy-MM-dd"); ;
                        lstOrdertext2.Add(view_salereport1);

                        view_salereport4.SumOrder = igst_amount;
                        view_salereport4.Narration = Convert.ToDateTime(item.ob_tbl_sales_order.purchase_date).ToString("yyyy-MM-dd"); ;
                        lstOrdertext2.Add(view_salereport4);

                        view_salereport2.SumOrder = cgst_amount;
                        view_salereport2.Narration = Convert.ToDateTime(item.ob_tbl_sales_order.purchase_date).ToString("yyyy-MM-dd"); ;
                        lstOrdertext2.Add(view_salereport2);

                        view_salereport3.SumOrder = sgst_amount;
                        view_salereport3.Narration = Convert.ToDateTime(item.ob_tbl_sales_order.purchase_date).ToString("yyyy-MM-dd"); ;
                        lstOrdertext2.Add(view_salereport3);
                    }

                }
            }
            catch (Exception ex)
            {
                Msg = "EX";
            }

            return Msg;
        }

        public void Get_Sales_Voucher_Report(List<SaleReport> lstOrdertext2, DateTime? txt_from, DateTime? txt_to, int sellers_id)
        {

            //List<SaleReport> lstOrdertext2 = new List<SaleReport>();
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
                    foreach (var item in GetSaleOrderDetail)
                    {
                        SaleReport view_salereport = new SaleReport();
                        double promotiondiscount = 0;
                        var get_saleorder_details = dba.tbl_sales_order_details.Where(a => a.tbl_sales_order_id == item.ob_tbl_sales_order.id && a.tbl_seller_id == sellers_id).FirstOrDefault();//to get sale_order details 

                        view_salereport.Sett_orderDate = Convert.ToDateTime(item.ob_tbl_sales_order.purchase_date).ToString("yyyy-MM-dd");
                        view_salereport.VoucherNumber = "September 2017 - V 01";
                        view_salereport.ExpenseName = "Sale Customer Amazon";
                        view_salereport.OrderID = item.ob_tbl_sales_order.amazon_order_id;
                        if (get_saleorder_details != null)
                        {
                            promotiondiscount = get_saleorder_details.promotion_amount;
                        }
                        view_salereport.refund_SumOrder = item.ob_tbl_sales_order.bill_amount - promotiondiscount;
                        //view_salereport.refund_SumOrder = item.ob_tbl_sales_order.bill_amount;
                        view_salereport.Narration = "Sales(September 2017)";

                        lstOrdertext2.Add(view_salereport);


                        // var get_saleorder_details = dba.tbl_sales_order_details.Where(a => a.tbl_sales_order_id == item.ob_tbl_sales_order.id && a.tbl_seller_id == sellers_id).FirstOrDefault();
                        if (get_saleorder_details != null)
                        {
                            SaleReport view_salereport1 = new SaleReport();
                            int deatil_id = get_saleorder_details.id;
                            view_salereport1.Sett_orderDate = Convert.ToDateTime(item.ob_tbl_sales_order.purchase_date).ToString("yyyy-MM-dd");
                            view_salereport1.VoucherNumber = "September 2017 - V 01";
                            view_salereport1.ExpenseName = "Sales A/c";
                            view_salereport1.OrderID = get_saleorder_details.amazon_order_id;
                            if (get_saleorder_details.item_price_amount < 0)
                            {
                                view_salereport1.SumOrder = (get_saleorder_details.item_price_amount) * (-1);
                            }
                            else
                            {
                                view_salereport1.SumOrder = get_saleorder_details.item_price_amount;
                            }
                            view_salereport1.Narration = "Sales(September 2017)";
                            lstOrdertext2.Add(view_salereport1);

                            var get_taxdetails = dba.tbl_tax.Where(a => a.tbl_seller_id == sellers_id && a.reference_type == 3 && a.tbl_referneced_id == deatil_id).FirstOrDefault();
                            if (get_taxdetails != null)
                            {
                                SaleReport view_salereport4 = new SaleReport();
                                view_salereport4.Sett_orderDate = Convert.ToDateTime(item.ob_tbl_sales_order.purchase_date).ToString("yyyy-MM-dd");
                                view_salereport4.VoucherNumber = "September 2017 - V 01";
                                view_salereport4.ExpenseName = "IGST";
                                view_salereport4.OrderID = get_saleorder_details.amazon_order_id;
                                if (get_taxdetails.Igst_amount < 0)
                                {
                                    view_salereport4.SumOrder = (Convert.ToDouble(get_taxdetails.Igst_amount)) * (-1);
                                }
                                else
                                {
                                    view_salereport4.SumOrder = Convert.ToDouble(get_taxdetails.Igst_amount);
                                }
                                view_salereport4.Narration = "Sales(September 2017)";
                                lstOrdertext2.Add(view_salereport4);

                                SaleReport view_salereport2 = new SaleReport();
                                view_salereport2.Sett_orderDate = Convert.ToDateTime(item.ob_tbl_sales_order.purchase_date).ToString("yyyy-MM-dd");
                                view_salereport2.VoucherNumber = "September 2017 - V 01";
                                view_salereport2.ExpenseName = "CGST";
                                view_salereport2.OrderID = get_saleorder_details.amazon_order_id;
                                if (get_taxdetails.CGST_amount < 0)
                                {
                                    view_salereport2.SumOrder = (Convert.ToDouble(get_taxdetails.CGST_amount)) * (-1);
                                }
                                else
                                {
                                    view_salereport2.SumOrder = Convert.ToDouble(get_taxdetails.CGST_amount);
                                }
                                view_salereport2.Narration = "Sales(September 2017)";
                                lstOrdertext2.Add(view_salereport2);

                                SaleReport view_salereport3 = new SaleReport();
                                view_salereport3.Sett_orderDate = Convert.ToDateTime(item.ob_tbl_sales_order.purchase_date).ToString("yyyy-MM-dd");
                                view_salereport3.VoucherNumber = "September 2017 - V 01";
                                view_salereport3.ExpenseName = "SGST";
                                view_salereport3.OrderID = get_saleorder_details.amazon_order_id;
                                if (get_taxdetails.sgst_amount < 0)
                                {
                                    view_salereport3.SumOrder = (Convert.ToDouble(get_taxdetails.sgst_amount)) * (-1);
                                }
                                else
                                {
                                    view_salereport3.SumOrder = Convert.ToDouble(get_taxdetails.sgst_amount);
                                }
                                view_salereport3.Narration = "Sales(September 2017)";
                                lstOrdertext2.Add(view_salereport3);


                            }// end of if(get_taxdetails)
                        }// end of if(get_saleorder_details)
                    }// end of for each(item)
                    //----------------------------------------END-----------------------------------------////
                }
            }// end of try block
            catch (Exception ex)
            {
            }// end of catch block
        }


        public string Get_Return_Voucher_Report(List<SaleReport> lstOrdertext2, DateTime? txt_from, DateTime? txt_to, int sellers_id)
        {
            string Msg = "S";
            try
            {
                if (txt_from != null && txt_to != null)
                {
                    var GetReturnHistoryDetail = (from ob_tblhistory in dba.tbl_order_history
                                                  select new
                                                  {
                                                      ob_tblhistory = ob_tblhistory
                                                  }).Where(a => a.ob_tblhistory.tbl_seller_id == sellers_id && a.ob_tblhistory.physically_type == 1).ToList();

                    if (GetReturnHistoryDetail != null)
                    {
                        if (txt_from != null && txt_to != null)
                        {
                            GetReturnHistoryDetail = GetReturnHistoryDetail.Where(a => Convert.ToDateTime(a.ob_tblhistory.ShipmentDate).Date >= txt_from && Convert.ToDateTime(a.ob_tblhistory.ShipmentDate).Date <= txt_to).ToList();
                        }
                    }


                    foreach (var item in GetReturnHistoryDetail)
                    {
                        int iRepeatDetailData = 0;
                        SaleReport view_salereport = new SaleReport();

                        var getsale_order = dba.tbl_sales_order.Where(a => a.amazon_order_id == item.ob_tblhistory.OrderID).FirstOrDefault();
                        if (getsale_order != null)
                        {

                            view_salereport.Sett_orderDate = Convert.ToDateTime(getsale_order.purchase_date).ToString("yyyy-MM-dd");
                            view_salereport.VoucherNumber = "September 2017 - V 01";
                            view_salereport.ExpenseName = "Sale Customer Amazon";
                            view_salereport.OrderID = getsale_order.amazon_order_id;


                            //////////////////// COLLECT ORDER DETAIL DATA ///////////////
                            var get_saleorder_details = dba.tbl_sales_order_details.Where(a => a.tbl_sales_order_id == getsale_order.id && a.tbl_seller_id == sellers_id && a.sku_no.ToLower() == item.ob_tblhistory.SKU.ToLower()).ToList();
                            SaleReport view_salereport1 = new SaleReport();
                            view_salereport1.Sett_orderDate = Convert.ToDateTime(getsale_order.purchase_date).ToString("yyyy-MM-dd");
                            view_salereport1.VoucherNumber = "September 2017 - V 01";
                            view_salereport1.ExpenseName = "Sales A/c";
                            view_salereport1.OrderID = getsale_order.amazon_order_id;
                            double item_price_amount = 0, shippingamount = 0, total_amount = 0;
                            double igst_amount = 0;
                            double cgst_amount = 0;
                            double sgst_amount = 0;
                            double promotiondiscount = 0;

                            SaleReport view_salereport4 = new SaleReport();
                            view_salereport4.Sett_orderDate = Convert.ToDateTime(getsale_order.purchase_date).ToString("yyyy-MM-dd");
                            view_salereport4.VoucherNumber = "September 2017 - V 01";
                            view_salereport4.ExpenseName = "IGST";
                            view_salereport4.OrderID = getsale_order.amazon_order_id;


                            SaleReport view_salereport2 = new SaleReport();
                            view_salereport2.Sett_orderDate = Convert.ToDateTime(getsale_order.purchase_date).ToString("yyyy-MM-dd");
                            view_salereport2.VoucherNumber = "September 2017 - V 01";
                            view_salereport2.ExpenseName = "CGST";
                            view_salereport2.OrderID = getsale_order.amazon_order_id;

                            SaleReport view_salereport3 = new SaleReport();
                            view_salereport3.Sett_orderDate = Convert.ToDateTime(getsale_order.purchase_date).ToString("yyyy-MM-dd");
                            view_salereport3.VoucherNumber = "September 2017 - V 01";
                            view_salereport3.ExpenseName = "SGST";
                            view_salereport3.OrderID = getsale_order.amazon_order_id;

                            SaleReport view_salereport5 = new SaleReport();
                            view_salereport5.Sett_orderDate = Convert.ToDateTime(getsale_order.purchase_date).ToString("yyyy-MM-dd");
                            view_salereport5.VoucherNumber = "September 2017 - V 01";
                            view_salereport5.ExpenseName = "Shipping charges";
                            view_salereport5.OrderID = getsale_order.amazon_order_id;

                            foreach (var itemDetail in get_saleorder_details)
                            {
                                int deatil_id = itemDetail.id;
                                double ord_detail_item_price_amount = itemDetail.item_price_amount;
                                double ord_detail_shipping_amount = itemDetail.shipping_price_Amount;
                                double ord_detail_shipping_discount_amount = itemDetail.shipping_discount_amt;
                                double ord_detail_shipping_tax_amount = itemDetail.shipping_tax_Amount;
                                double? ord_detail_shipping_discount_tax_amount = itemDetail.shipping_discount_tax_amount;
                                double ord_itemtax_amount = itemDetail.item_tax_amount;
                                promotiondiscount = promotiondiscount + itemDetail.promotion_amount;
                                item_price_amount = item_price_amount + ord_detail_item_price_amount;// +ord_detail_shipping_amount + ord_detail_shipping_tax_amount;
                                shippingamount = ord_detail_shipping_amount;
                                total_amount = item_price_amount + shippingamount + ord_detail_shipping_tax_amount + ord_itemtax_amount - promotiondiscount;
                                ////////////////////// GET TAX EXPENSES //////////////////

                                var get_taxdetails = dba.tbl_tax.Where(a => a.tbl_seller_id == sellers_id && a.reference_type == 3 && a.tbl_referneced_id == deatil_id).FirstOrDefault();
                                if (get_taxdetails != null)
                                {
                                    if (get_taxdetails.Igst_amount < 0)
                                    {
                                        igst_amount = igst_amount + (Convert.ToDouble(get_taxdetails.Igst_amount)) * (-1);
                                    }
                                    else
                                    {
                                        igst_amount = igst_amount + Convert.ToDouble(get_taxdetails.Igst_amount);
                                    }

                                    if (get_taxdetails.CGST_amount < 0)
                                    {
                                        cgst_amount = cgst_amount + (Convert.ToDouble(get_taxdetails.CGST_amount)) * (-1);
                                    }
                                    else
                                    {
                                        cgst_amount = cgst_amount + (Convert.ToDouble(get_taxdetails.CGST_amount));
                                    }

                                    if (get_taxdetails.sgst_amount < 0)
                                    {
                                        sgst_amount = sgst_amount + (Convert.ToDouble(get_taxdetails.sgst_amount)) * (-1);
                                    }
                                    else
                                    {
                                        sgst_amount = sgst_amount + (Convert.ToDouble(get_taxdetails.sgst_amount));
                                    }
                                }

                                iRepeatDetailData++;
                            }
                            view_salereport.SumOrder = total_amount;//getsale_order.bill_amount - promotiondiscount;
                            view_salereport.Narration = "Sales(September 2017)";
                            lstOrdertext2.Add(view_salereport);

                            view_salereport1.refund_SumOrder = item_price_amount;
                            view_salereport1.Narration = "Sales(September 2017)";
                            lstOrdertext2.Add(view_salereport1);

                            if (shippingamount != 0 && shippingamount != null)
                            {
                                view_salereport5.refund_SumOrder = shippingamount;
                                view_salereport5.Narration = "Sales(September 2017)";
                                lstOrdertext2.Add(view_salereport5);
                            }

                            view_salereport4.refund_SumOrder = igst_amount;
                            view_salereport4.Narration = "Sales(September 2017)";
                            lstOrdertext2.Add(view_salereport4);

                            view_salereport2.refund_SumOrder = cgst_amount;
                            view_salereport2.Narration = "Sales(September 2017)";
                            lstOrdertext2.Add(view_salereport2);

                            view_salereport3.refund_SumOrder = sgst_amount;
                            view_salereport3.Narration = "Sales(September 2017)";
                            lstOrdertext2.Add(view_salereport3);
                        }// end of if(getsale_order)

                    }
                }
            }
            catch (Exception ex)
            {
                Msg = "EX";
            }

            return Msg;
        }
        public string Get_Return_Voucher_Report1(List<SaleReport> lstOrdertext2, DateTime? txt_from, DateTime? txt_to, int sellers_id)
        {
            string Msg = "S";
            try
            {
                if (txt_from != null && txt_to != null)
                {
                    var GetReturnHistoryDetail = (from ob_tblhistory in dba.tbl_order_history
                                              select new
                                              {
                                                  ob_tblhistory = ob_tblhistory
                                              }).Where(a => a.ob_tblhistory.tbl_seller_id == sellers_id && a.ob_tblhistory.physically_type==1).ToList();

                    if (GetReturnHistoryDetail != null)
                    {
                        if (txt_from != null && txt_to != null)
                        {
                            GetReturnHistoryDetail = GetReturnHistoryDetail.Where(a =>Convert.ToDateTime(a.ob_tblhistory.ShipmentDate).Date >= txt_from && Convert.ToDateTime(a.ob_tblhistory.ShipmentDate).Date <= txt_to).ToList();
                        }
                    }


                    foreach (var item in GetReturnHistoryDetail)
                    {
                        int iRepeatDetailData = 0;
                        SaleReport view_salereport = new SaleReport();

                        var getsale_order = dba.tbl_sales_order.Where(a => a.amazon_order_id == item.ob_tblhistory.OrderID).FirstOrDefault();
                        if (getsale_order != null)
                        {

                            view_salereport.Sett_orderDate = Convert.ToDateTime(getsale_order.purchase_date).ToString("yyyy-MM-dd");
                            view_salereport.VoucherNumber = "September 2017 - V 01";
                            view_salereport.ExpenseName = "Sale Customer Amazon";
                            view_salereport.OrderID = getsale_order.amazon_order_id;
                          

                            //////////////////// COLLECT ORDER DETAIL DATA ///////////////
                            var get_saleorder_details = dba.tbl_sales_order_details.Where(a => a.tbl_sales_order_id == getsale_order.id && a.tbl_seller_id == sellers_id && a.sku_no.ToLower() == item.ob_tblhistory.SKU.ToLower()).ToList();
                            SaleReport view_salereport1 = new SaleReport();
                            view_salereport1.Sett_orderDate = Convert.ToDateTime(getsale_order.purchase_date).ToString("yyyy-MM-dd");
                            view_salereport1.VoucherNumber = "September 2017 - V 01";
                            view_salereport1.ExpenseName = "Sales A/c";
                            view_salereport1.OrderID = getsale_order.amazon_order_id;
                            double item_price_amount = 0;
                            double igst_amount = 0;
                            double cgst_amount = 0;
                            double sgst_amount = 0;
                            double promotiondiscount = 0;

                            SaleReport view_salereport4 = new SaleReport();
                            view_salereport4.Sett_orderDate = Convert.ToDateTime(getsale_order.purchase_date).ToString("yyyy-MM-dd");
                            view_salereport4.VoucherNumber = "September 2017 - V 01";
                            view_salereport4.ExpenseName = "IGST";
                            view_salereport4.OrderID = getsale_order.amazon_order_id;

                            
                            SaleReport view_salereport2 = new SaleReport();
                            view_salereport2.Sett_orderDate = Convert.ToDateTime(getsale_order.purchase_date).ToString("yyyy-MM-dd");
                            view_salereport2.VoucherNumber = "September 2017 - V 01";
                            view_salereport2.ExpenseName = "CGST";
                            view_salereport2.OrderID = getsale_order.amazon_order_id;

                            SaleReport view_salereport3 = new SaleReport();
                            view_salereport3.Sett_orderDate = Convert.ToDateTime(getsale_order.purchase_date).ToString("yyyy-MM-dd");
                            view_salereport3.VoucherNumber = "September 2017 - V 01";
                            view_salereport3.ExpenseName = "SGST";
                            view_salereport3.OrderID = getsale_order.amazon_order_id;

                            foreach (var itemDetail in get_saleorder_details)
                            {
                                int deatil_id = itemDetail.id;
                                double ord_detail_item_price_amount = itemDetail.item_price_amount;
                                double ord_detail_shipping_amount = itemDetail.shipping_price_Amount;
                                double ord_detail_shipping_discount_amount = itemDetail.shipping_discount_amt;
                                double ord_detail_shipping_tax_amount = itemDetail.shipping_tax_Amount;
                                double? ord_detail_shipping_discount_tax_amount = itemDetail.shipping_discount_tax_amount;
                                promotiondiscount = promotiondiscount + itemDetail.promotion_amount;                             
                                item_price_amount = item_price_amount + ord_detail_item_price_amount;// +ord_detail_shipping_amount + ord_detail_shipping_tax_amount;

                                ////////////////////// GET TAX EXPENSES //////////////////

                                var get_taxdetails = dba.tbl_tax.Where(a => a.tbl_seller_id == sellers_id && a.reference_type == 3 && a.tbl_referneced_id == deatil_id).FirstOrDefault();
                                if (get_taxdetails != null)
                                {
                                    if (get_taxdetails.Igst_amount < 0)
                                    {
                                        igst_amount = igst_amount + (Convert.ToDouble(get_taxdetails.Igst_amount)) * (-1);                                  
                                    }
                                    else
                                    {
                                        igst_amount = igst_amount + Convert.ToDouble(get_taxdetails.Igst_amount);
                                    }

                                    if (get_taxdetails.CGST_amount < 0)
                                    {
                                        cgst_amount = cgst_amount + (Convert.ToDouble(get_taxdetails.CGST_amount)) * (-1);
                                    }
                                    else
                                    {
                                        cgst_amount = cgst_amount + (Convert.ToDouble(get_taxdetails.CGST_amount));
                                    }

                                    if (get_taxdetails.sgst_amount < 0)
                                    {
                                        sgst_amount = sgst_amount + (Convert.ToDouble(get_taxdetails.sgst_amount)) * (-1);
                                    }
                                    else
                                    {
                                        sgst_amount = sgst_amount + (Convert.ToDouble(get_taxdetails.sgst_amount));
                                    }
                                }

                                iRepeatDetailData++;
                            }
                            view_salereport.refund_SumOrder = getsale_order.bill_amount - promotiondiscount;
                            view_salereport.Narration = "Sales(September 2017)";
                            lstOrdertext2.Add(view_salereport);

                            view_salereport1.SumOrder = item_price_amount;
                            view_salereport1.Narration = "Sales(September 2017)";
                            lstOrdertext2.Add(view_salereport1);

                            view_salereport4.SumOrder = igst_amount;
                            view_salereport4.Narration = "Sales(September 2017)";
                            lstOrdertext2.Add(view_salereport4);

                            view_salereport2.SumOrder = cgst_amount;
                            view_salereport2.Narration = "Sales(September 2017)";
                            lstOrdertext2.Add(view_salereport2);

                            view_salereport3.SumOrder = sgst_amount;
                            view_salereport3.Narration = "Sales(September 2017)";
                            lstOrdertext2.Add(view_salereport3);
                        }// end of if(getsale_order)

                    }



                }
            }
            catch (Exception ex)
            {
                Msg = "EX";
            }

            return Msg;
        }

        /// <summary>
        /// this is for without using sp it is working 
        /// if use then remove 1 from function name 
        /// </summary>
        /// <param name="lstOrdertext2"></param>
        /// <param name="txt_from"></param>
        /// <param name="txt_to"></param>
        /// <param name="sellers_id"></param>
        /// <param name="stateid"></param>
        /// <returns></returns>
        public string Get_Sales_Excel_Report(List<SaleReport> lstOrdertext2, DateTime? txt_from, DateTime? txt_to, int sellers_id, int stateid, int? ddl_market_place, string MarketPlace)
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

                    if (GetSaleOrderDetail != null)
                    {
                        if (txt_from != null && txt_to != null)
                        {
                            GetSaleOrderDetail = GetSaleOrderDetail.Where(a => a.ob_tbl_sales_order.purchase_date.Date >= txt_from && a.ob_tbl_sales_order.purchase_date.Date <= txt_to).OrderByDescending(a => a.ob_tbl_sales_order.order_status == "Shipped" || a.ob_tbl_sales_order.order_status == "Shipped - Delivered to Buyer"||a.ob_tbl_sales_order.order_status=="Delivered").ToList();
                        }
                    }// end of if(GetSaleOrderDetail)
                    //int counter = 0;
                    foreach (var item in GetSaleOrderDetail)
                    {
                        //counter++;
                        //if (counter > 5)
                        //{
                        //    break;
                        //} 
                        if (item.ob_tbl_sales_order.order_status == "Shipped" || item.ob_tbl_sales_order.order_status == "Shipped - Delivered to Buyer" || item.ob_tbl_sales_order.order_status=="Delivered")
                        {
                            int iRepeatDetailData = 0;
                            var get_sale_orderdetails = dba.tbl_sales_order_details.Where(a => a.tbl_sales_order_id == item.ob_tbl_sales_order.id && a.tbl_seller_id == sellers_id && a.amazon_order_id == item.ob_tbl_sales_order.amazon_order_id).FirstOrDefault();
                            var get_customerdetails = dba.tbl_customer_details.Where(a => a.tbl_seller_id == sellers_id && a.id == item.ob_tbl_sales_order.tbl_Customer_Id).FirstOrDefault();
                            SaleReport view_salereport = new SaleReport();

                            view_salereport.OrderDate = Convert.ToDateTime(item.ob_tbl_sales_order.purchase_date).ToString("yyyy-MM-dd");
                            view_salereport.OrderID = item.ob_tbl_sales_order.amazon_order_id;
                            view_salereport.InvoiceNo = item.ob_tbl_sales_order.amazon_order_id;
                            view_salereport.Channelentry = MarketPlace + " " + "Sale Customer" + "" + MarketPlace;
                            view_salereport.Channelledger = MarketPlace + " " + "Sale Customer" + "" + MarketPlace;

                            view_salereport.shipprovider = "";
                            view_salereport.AWBNo = "";
                            view_salereport.CustomerName = item.ob_tbl_sales_order.buyer_name;
                            view_salereport.UTGST = "";
                            view_salereport.UTGSTRate = "";
                            view_salereport.CESS = "";
                            view_salereport.CESSRate = "";
                            view_salereport.Servicetax = "";
                            view_salereport.StLedger = "";
                            view_salereport.Godown = "Main Location";
                            view_salereport.Dispatch_Cancellationdate = "";
                            view_salereport.Narration = "";
                            view_salereport.Entity = "Raintree Sale(S)";
                            view_salereport.TinNo = "";
                            view_salereport.Channelinvoicecreated = "";
                            view_salereport.TaxVerification = "1";
                            view_salereport.IMEI = "";

                            if (get_customerdetails != null)
                            {
                                view_salereport.shipaddressname = get_customerdetails.shipping_Buyer_Name;
                                view_salereport.shipaddressname1 = get_customerdetails.Address_1;
                                view_salereport.shipaddressname2 = get_customerdetails.Address_2;
                                view_salereport.shipcity = get_customerdetails.City;
                                view_salereport.shipstate = get_customerdetails.State_Region;
                                view_salereport.shipcountry = get_customerdetails.Country_Code;
                                view_salereport.shippincode = get_customerdetails.Postal_Code;
                                view_salereport.shipphoneno = "";
                            }
                            if (get_sale_orderdetails != null)
                            {
                                view_salereport.ProductName = get_sale_orderdetails.product_name;
                                view_salereport.skuNo = get_sale_orderdetails.sku_no;
                                if (get_sale_orderdetails.quantity_ordered != 0 && get_sale_orderdetails.quantity_ordered != null)
                                {
                                    view_salereport.Quantity = get_sale_orderdetails.quantity_ordered;
                                }
                                else
                                {
                                    view_salereport.Quantity = 0;
                                }
                                view_salereport.ProductValue = Convert.ToDouble(get_sale_orderdetails.item_price_amount + get_sale_orderdetails.shipping_price_Amount + get_sale_orderdetails.giftwrapprice_amount);
                                view_salereport.Currency = "INR";
                                view_salereport.OrderAddTotal = Convert.ToDouble(get_sale_orderdetails.item_price_amount + get_sale_orderdetails.shipping_price_Amount + get_sale_orderdetails.giftwrapprice_amount + get_sale_orderdetails.item_tax_amount + get_sale_orderdetails.shipping_tax_Amount);
                                view_salereport.itemamountwithout_tax = get_sale_orderdetails.item_price_amount;
                                view_salereport.Shipping = get_sale_orderdetails.shipping_price_Amount;
                                view_salereport.GiftwrapAmount = Convert.ToDouble(get_sale_orderdetails.giftwrapprice_amount);
                                var get_tax = dba.tbl_tax.Where(a => a.tbl_referneced_id == get_sale_orderdetails.id && a.reference_type == 3 && a.tbl_seller_id == sellers_id).FirstOrDefault();
                                if (get_tax != null)
                                {
                                    double cgst_taxrate = 0;
                                    double sgst_taxrate = 0;
                                    double igst_taxrate = 0;
                                    string taxname2 = "";
                                    cgst_taxrate = Convert.ToDouble(get_tax.cgst_tax);
                                    sgst_taxrate = Convert.ToDouble(get_tax.sgst_tax);
                                    igst_taxrate = Convert.ToDouble(get_tax.igst_tax);
                                    if (cgst_taxrate != 0 && sgst_taxrate != 0)
                                    {
                                        var totaltax = cgst_taxrate + sgst_taxrate;
                                        taxname2 = "Local Sale@" + totaltax + "%";
                                        view_salereport.SalesLedger = taxname2;
                                        view_salereport.SUMSGST = Convert.ToDouble(get_tax.sgst_amount);
                                        view_salereport.SUMCGST = Convert.ToDouble(get_tax.CGST_amount);
                                        view_salereport.SGST_rate = "SGST@" + sgst_taxrate + "%";
                                        view_salereport.CGST_rate = "CGST@" + cgst_taxrate + "%";
                                        if (get_sale_orderdetails.shipping_tax_Amount != 0)
                                        {
                                            view_salereport.Shipping_rate = "Shipping Charges@" + totaltax + "%";
                                        }
                                        if (get_sale_orderdetails.giftwraptax_amount != 0 && get_sale_orderdetails.giftwraptax_amount != null)
                                        {
                                            view_salereport.Giftwrap_rate = "Gift Wrap Charges@" + totaltax + "%";
                                        }
                                    }
                                    else
                                    {
                                        taxname2 = "Sale IGST@" + igst_taxrate + "%";
                                        view_salereport.SalesLedger = taxname2;
                                        view_salereport.SUMIGST = Convert.ToDouble(get_tax.Igst_amount);
                                        view_salereport.IGST_rate = "IGST@" + igst_taxrate + "%";
                                        if (get_sale_orderdetails.shipping_tax_Amount != 0)
                                        {
                                            view_salereport.Shipping_rate = "Shipping Charges@" + igst_taxrate + "%";
                                        }
                                        if (get_sale_orderdetails.giftwraptax_amount != 0 && get_sale_orderdetails.giftwraptax_amount != null)
                                        {
                                            view_salereport.Giftwrap_rate = "Gift Wrap Charges@" + igst_taxrate + "%";
                                        }
                                    }

                                }// end of if(get_tax)
                                else
                                {
                                    var statename = db.tbl_country.Where(m => m.id == stateid && m.countrylevel == 1 && m.status == 1).FirstOrDefault();
                                    if (statename != null)
                                    {
                                        string taxtype = "";
                                        string sellerstate = statename.countryname.ToLower();
                                        if (sellerstate == view_salereport.shipstate.ToLower())
                                        {
                                            taxtype = "Local Sale@" + 0 + "%";
                                            view_salereport.SalesLedger = taxtype;
                                        }
                                        else
                                        {
                                            taxtype = "Sale IGST@" + 0 + "%";
                                            view_salereport.SalesLedger = taxtype;
                                        }
                                    }
                                }
                            }// end of if get_sale_orderdetails
                            lstOrdertext2.Add(view_salereport);
                        }// end of if ()
                        else if (item.ob_tbl_sales_order.order_status == "Canceled" || item.ob_tbl_sales_order.order_status == "Cancelled" || item.ob_tbl_sales_order.order_status == "Return Requested" || item.ob_tbl_sales_order.order_status == "Returned")
                        {
                            int iRepeatDetailData = 0;
                            var get_sale_orderdetails = dba.tbl_sales_order_details.Where(a => a.tbl_sales_order_id == item.ob_tbl_sales_order.id && a.tbl_seller_id == sellers_id && a.amazon_order_id == item.ob_tbl_sales_order.amazon_order_id).FirstOrDefault();
                            var get_customerdetails = dba.tbl_customer_details.Where(a => a.tbl_seller_id == sellers_id && a.id == item.ob_tbl_sales_order.tbl_Customer_Id).FirstOrDefault();
                            SaleReport view_salereport = new SaleReport();

                            view_salereport.OrderDate = Convert.ToDateTime(item.ob_tbl_sales_order.purchase_date).ToString("yyyy-MM-dd");
                            view_salereport.OrderID = item.ob_tbl_sales_order.amazon_order_id;
                            view_salereport.InvoiceNo = item.ob_tbl_sales_order.amazon_order_id;
                            view_salereport.Channelentry = MarketPlace +" "+ "Sale Customer" +" "+MarketPlace;
                            view_salereport.Channelledger = MarketPlace + " " + "Sale Customer" + " " + MarketPlace;

                            view_salereport.shipprovider = "";
                            view_salereport.AWBNo = "";
                            view_salereport.CustomerName = item.ob_tbl_sales_order.buyer_name;
                            view_salereport.UTGST = "";
                            view_salereport.UTGSTRate = "";
                            view_salereport.CESS = "";
                            view_salereport.CESSRate = "";
                            view_salereport.Servicetax = "";
                            view_salereport.StLedger = "";
                            view_salereport.Godown = "Main Location";
                            view_salereport.Dispatch_Cancellationdate = "";
                            view_salereport.Narration = "";
                            view_salereport.Entity = "Credit Note";
                            view_salereport.TinNo = "";
                            view_salereport.Channelinvoicecreated = "";
                            view_salereport.TaxVerification = "1";
                            view_salereport.IMEI = "";

                            if (get_customerdetails != null)
                            {
                                view_salereport.shipaddressname = get_customerdetails.shipping_Buyer_Name;
                                view_salereport.shipaddressname1 = get_customerdetails.Address_1;
                                view_salereport.shipaddressname2 = get_customerdetails.Address_2;
                                view_salereport.shipcity = get_customerdetails.City;
                                view_salereport.shipstate = get_customerdetails.State_Region;
                                view_salereport.shipcountry = get_customerdetails.Country_Code;
                                view_salereport.shippincode = get_customerdetails.Postal_Code;
                                view_salereport.shipphoneno = "";
                            }
                            if (get_sale_orderdetails != null)
                            {
                                view_salereport.ProductName = get_sale_orderdetails.product_name;
                                view_salereport.skuNo = get_sale_orderdetails.sku_no;
                                if (get_sale_orderdetails.quantity_ordered != 0 && get_sale_orderdetails.quantity_ordered != null)
                                {
                                    view_salereport.Quantity = get_sale_orderdetails.quantity_ordered;
                                }
                                else
                                {
                                    view_salereport.Quantity = 0;
                                }
                                view_salereport.ProductValue = Convert.ToDouble(get_sale_orderdetails.item_price_amount + get_sale_orderdetails.shipping_price_Amount + get_sale_orderdetails.giftwrapprice_amount);
                                view_salereport.Currency = "INR";
                                view_salereport.OrderAddTotal = Convert.ToDouble(get_sale_orderdetails.item_price_amount + get_sale_orderdetails.shipping_price_Amount + get_sale_orderdetails.giftwrapprice_amount + get_sale_orderdetails.item_tax_amount + get_sale_orderdetails.shipping_tax_Amount);
                                view_salereport.itemamountwithout_tax = get_sale_orderdetails.item_price_amount;
                                view_salereport.Shipping = get_sale_orderdetails.shipping_price_Amount;
                                view_salereport.GiftwrapAmount = Convert.ToDouble(get_sale_orderdetails.giftwrapprice_amount);
                                var get_tax = dba.tbl_tax.Where(a => a.tbl_referneced_id == get_sale_orderdetails.id && a.reference_type == 3 && a.tbl_seller_id == sellers_id).FirstOrDefault();
                                if (get_tax != null)
                                {
                                    double cgst_taxrate = 0;
                                    double sgst_taxrate = 0;
                                    double igst_taxrate = 0;
                                    string taxname2 = "";
                                    cgst_taxrate = Convert.ToDouble(get_tax.cgst_tax);
                                    sgst_taxrate = Convert.ToDouble(get_tax.sgst_tax);
                                    igst_taxrate = Convert.ToDouble(get_tax.igst_tax);
                                    if (cgst_taxrate != 0 && sgst_taxrate != 0)
                                    {
                                        var totaltax = cgst_taxrate + sgst_taxrate;
                                        taxname2 = "Local Sale@" + totaltax + "%";
                                        view_salereport.SalesLedger = taxname2;
                                        view_salereport.SUMSGST = Convert.ToDouble(get_tax.sgst_amount);
                                        view_salereport.SUMCGST = Convert.ToDouble(get_tax.CGST_amount);
                                        view_salereport.SGST_rate = "SGST@" + sgst_taxrate + "%";
                                        view_salereport.CGST_rate = "CGST@" + cgst_taxrate + "%";
                                        if (get_sale_orderdetails.shipping_tax_Amount != 0)
                                        {
                                            view_salereport.Shipping_rate = "Shipping Charges@" + totaltax + "%";
                                        }
                                        if (get_sale_orderdetails.giftwraptax_amount != 0 && get_sale_orderdetails.giftwraptax_amount != null)
                                        {
                                            view_salereport.Giftwrap_rate = "Gift Wrap Charges@" + totaltax + "%";
                                        }
                                    }
                                    else
                                    {
                                        taxname2 = "Sale IGST@" + igst_taxrate + "%";
                                        view_salereport.SalesLedger = taxname2;
                                        view_salereport.SUMIGST = Convert.ToDouble(get_tax.Igst_amount);
                                        view_salereport.IGST_rate = "IGST@" + igst_taxrate + "%";
                                        if (get_sale_orderdetails.shipping_tax_Amount != 0)
                                        {
                                            view_salereport.Shipping_rate = "Shipping Charges@" + igst_taxrate + "%";
                                        }
                                        if (get_sale_orderdetails.giftwraptax_amount != 0 && get_sale_orderdetails.giftwraptax_amount != null)
                                        {
                                            view_salereport.Giftwrap_rate = "Gift Wrap Charges@" + igst_taxrate + "%";
                                        }
                                    }

                                }// end of if(get_tax)
                                else
                                {
                                    var statename = db.tbl_country.Where(m => m.id == stateid && m.countrylevel == 1 && m.status == 1).FirstOrDefault();
                                    if (statename != null)
                                    {
                                        string taxtype = "";
                                        string sellerstate = statename.countryname.ToLower();
                                        if (sellerstate == view_salereport.shipstate.ToLower())
                                        {
                                            taxtype = "Local Sale@" + 0 + "%";
                                            view_salereport.SalesLedger = taxtype;
                                        }
                                        else
                                        {
                                            taxtype = "Sale IGST@" + 0 + "%";
                                            view_salereport.SalesLedger = taxtype;
                                        }
                                    }
                                }
                            }// end of if get_sale_orderdetails
                            lstOrdertext2.Add(view_salereport);

                        }// end of elseif()
                    }// end of main for each loop

                }// end of if(txt_from)
            }
            catch(Exception ex)
            {
            }
            return Msg;
        }

        /// <summary>
        /// this is for using sp it is working  
        /// </summary>
        /// <param name="lstOrdertext2"></param>
        /// <param name="txt_from"></param>
        /// <param name="txt_to"></param>
        /// <param name="sellers_id"></param>
        /// <param name="stateid"></param>
        /// <returns></returns>
        public string Get_Sales_Excel_Report1(List<SaleReport> lstOrdertext2, DateTime? txt_from, DateTime? txt_to, int sellers_id, int stateid, int? ddl_market_place)
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
                        command.CommandText = "get_sales_voucher"; // "get_sett_orders";
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
                            var get_sales_voucher_data =
                                ((System.Data.Entity.Infrastructure.IObjectContextAdapter)dba)
                                    .ObjectContext
                                    .Translate<proc_Sales_Voucher>(reader)
                                    .ToList();

                            connection.Close();
                            dba = new SellerContext();
                            dba.Configuration.AutoDetectChangesEnabled = false;

                            if (get_sales_voucher_data != null)
                            {
                                for (int maincounter = 0; maincounter < get_sales_voucher_data.Count; maincounter++)
                                {
                                    SaleReport view_salereport = new SaleReport();

                                    var sett = get_sales_voucher_data[maincounter];

                                    view_salereport.OrderDate = Convert.ToDateTime(sett.purchase_date).ToString("yyyy-MM-dd");
                                    view_salereport.OrderID = sett.amazon_order_id;
                                    view_salereport.InvoiceNo = sett.amazon_order_id;
                                    view_salereport.Channelentry = "Amazon Sale Customer Amazon";
                                    view_salereport.Channelledger = "Amazon Sale Customer Amazon";
                                    view_salereport.shipaddressname = sett.shipping_Buyer_Name;
                                    view_salereport.shipaddressname1 = sett.Address_1;
                                    view_salereport.shipaddressname2 = sett.Address_2;
                                    view_salereport.shipcity = sett.City;
                                    view_salereport.shipstate = sett.State_Region;
                                    view_salereport.shipcountry = sett.Country_Code;
                                    view_salereport.shippincode = sett.Postal_Code;
                                    view_salereport.shipphoneno = "";
                                    view_salereport.shipprovider = "";
                                    view_salereport.AWBNo = "";
                                    view_salereport.CustomerName = sett.buyer_name;
                                    view_salereport.UTGST = "";
                                    view_salereport.UTGSTRate = "";
                                    view_salereport.CESS = "";
                                    view_salereport.CESSRate = "";
                                    view_salereport.Servicetax = "";
                                    view_salereport.StLedger = "";
                                    view_salereport.Godown = "Main Location";
                                    view_salereport.Dispatch_Cancellationdate = "";
                                    view_salereport.Narration = "";
                                    view_salereport.Entity = "Raintree Sale(S)";
                                    view_salereport.TinNo = "";
                                    view_salereport.Channelinvoicecreated = "";
                                    view_salereport.TaxVerification = "1";
                                    view_salereport.IMEI = "";

                                    view_salereport.ProductName = sett.product_name;
                                    view_salereport.skuNo = sett.sku_no;
                                    if (sett.quantity_ordered != 0 && sett.quantity_ordered != null)
                                    {
                                        view_salereport.Quantity =Convert.ToInt16(sett.quantity_ordered);
                                    }
                                    else
                                    {
                                        view_salereport.Quantity = 0;
                                    }
                                    view_salereport.ProductValue = Convert.ToDouble(sett.item_price_amount + sett.shipping_price_Amount + sett.giftwrapprice_amount);
                                    view_salereport.Currency = "INR";
                                    view_salereport.OrderAddTotal = Convert.ToDouble(sett.item_price_amount + sett.shipping_price_Amount + sett.giftwrapprice_amount + sett.item_tax_amount + sett.shipping_tax_Amount);
                                    view_salereport.itemamountwithout_tax = Convert.ToDouble(sett.item_price_amount);
                                    view_salereport.Shipping = Convert.ToDouble(sett.shipping_price_Amount);
                                    view_salereport.GiftwrapAmount = Convert.ToDouble(sett.giftwrapprice_amount);

                                    var get_tax = dba.tbl_tax.Where(a => a.tbl_referneced_id == sett.orderdetailsID && a.reference_type == 3 && a.tbl_seller_id == sellers_id).FirstOrDefault();
                                    if (get_tax != null)
                                    {
                                        double cgst_taxrate = 0;
                                        double sgst_taxrate = 0;
                                        double igst_taxrate = 0;
                                        string taxname2 = "";
                                        cgst_taxrate = Convert.ToDouble(get_tax.cgst_tax);
                                        sgst_taxrate = Convert.ToDouble(get_tax.sgst_tax);
                                        igst_taxrate = Convert.ToDouble(get_tax.igst_tax);
                                        if (cgst_taxrate != 0 && sgst_taxrate != 0)
                                        {
                                            var totaltax = cgst_taxrate + sgst_taxrate;
                                            taxname2 = "Local Sale@" + totaltax + "%";
                                            view_salereport.SalesLedger = taxname2;
                                            view_salereport.SUMSGST = Convert.ToDouble(get_tax.sgst_amount);
                                            view_salereport.SUMCGST = Convert.ToDouble(get_tax.CGST_amount);
                                            view_salereport.SGST_rate = "SGST@" + sgst_taxrate + "%";
                                            view_salereport.CGST_rate = "CGST@" + cgst_taxrate + "%";
                                            if (sett.shipping_tax_Amount != 0 && sett.shipping_tax_Amount != null)
                                            {
                                                view_salereport.Shipping_rate = "Shipping Charges@" + totaltax + "%";
                                            }
                                            if (sett.giftwraptax_amount != 0 && sett.giftwraptax_amount != null)
                                            {
                                                view_salereport.Giftwrap_rate = "Gift Wrap Charges@" + totaltax + "%";
                                            }
                                        }
                                        else
                                        {
                                            taxname2 = "Sale IGST@" + igst_taxrate + "%";
                                            view_salereport.SalesLedger = taxname2;
                                            view_salereport.SUMIGST = Convert.ToDouble(get_tax.Igst_amount);
                                            view_salereport.IGST_rate = "IGST@" + igst_taxrate + "%";
                                            if (sett.shipping_tax_Amount != 0 && sett.shipping_tax_Amount != null)
                                            {
                                                view_salereport.Shipping_rate = "Shipping Charges@" + igst_taxrate + "%";
                                            }
                                            if (sett.giftwraptax_amount != 0 && sett.giftwraptax_amount != null)
                                            {
                                                view_salereport.Giftwrap_rate = "Gift Wrap Charges@" + igst_taxrate + "%";
                                            }
                                        }

                                    }// end of if(get_tax)
                                    else
                                    {
                                        var statename = db.tbl_country.Where(m => m.id == stateid && m.countrylevel == 1 && m.status == 1).FirstOrDefault();
                                        if (statename != null)
                                        {
                                            string taxtype = "";
                                            string sellerstate = statename.countryname.ToLower();
                                            if (view_salereport.shipstate != null)
                                            {
                                                if (sellerstate == view_salereport.shipstate.ToLower())
                                                {
                                                    taxtype = "Local Sale@" + 0 + "%";
                                                    view_salereport.SalesLedger = taxtype;
                                                }
                                                else
                                                {
                                                    taxtype = "Sale IGST@" + 0 + "%";
                                                    view_salereport.SalesLedger = taxtype;
                                                }
                                            }
                                        }
                                    }
                                    lstOrdertext2.Add(view_salereport);
                                }
                            }
                        }
                       // connection.Close();
                    }// end of using connection
                        //lstOrdertext2.Add(view_salereport);
                    
                    dba.Dispose();                 
                }// end of if(txt_from)
            }
            catch (Exception ex)
            {
            }
            return Msg;
        }
       
       
    }
}