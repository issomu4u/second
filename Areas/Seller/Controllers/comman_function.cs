using iTextSharp.text;
using iTextSharp.text.pdf;
using SellerVendor.Areas.Seller.Models;
using SellerVendor.Areas.Seller.Models.DBContext;
using SellerVendor.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Mail;
using System.Globalization;
using System.Data.Entity;
using System.Xml;
using System.Xml.Linq;

namespace SellerVendor.Areas.Seller.Controllers
{
    public class comman_function : Controller
    {
        public SellerContext dba = new SellerContext();
        public SellerAdminContext db = new SellerAdminContext();
        SelectListItem SelectItems = null;
        SelectListItem SelectAllItems = null;

        public comman_function()
        {
            SelectItems = new SelectListItem();
            SelectItems.Text = "--Select--";
            SelectItems.Value = "";
            SelectAllItems = new SelectListItem();
            SelectAllItems.Text = " --Select--  ";
            SelectAllItems.Value = "0";
        }
        public bool session_check()
        {
            string Email = System.Web.HttpContext.Current.Session["Email"] + "";
            bool is_available = false;
            if (Email != "")
            {
                is_available = true;
            }
            if (is_available == false)
            {
                try
                {
                    HttpCookie cookie = System.Web.HttpContext.Current.Request.Cookies["SellerCookie"];
                    if (cookie != null)
                    {
                        System.Web.HttpContext.Current.Session["Email"] = cookie["SellerEmail"].ToString();
                        System.Web.HttpContext.Current.Session["UserName"] = cookie["SellerUserName"].ToString();
                        System.Web.HttpContext.Current.Session["SellerID"] = cookie["SellerSellerID"].ToString();
                        System.Web.HttpContext.Current.Session["UserWelcomeName"] = cookie["SellerUserWelcomeName"].ToString();
                        System.Web.HttpContext.Current.Session["SellerType"] = cookie["SellerSellerType"].ToString();
                        System.Web.HttpContext.Current.Session["TypeSeller"] = cookie["SellerTypeSeller"].ToString();
                        System.Web.HttpContext.Current.Session["WalletBalance"] = cookie["SellerWalletBalance"].ToString();
                        System.Web.HttpContext.Current.Session["TotalOrders"] = cookie["SellerTotalOrders"].ToString();
                        System.Web.HttpContext.Current.Session["State"] = cookie["SellerState"].ToString();
                        // System.Web.HttpContext.Current.Session["LastLogin"] = cookie["SellerLastLogin"].ToString();
                        Email = cookie["SellerEmail"].ToString();
                        var user_details = db.tbl_user_login.Where(a => a.Email == Email).FirstOrDefault();
                        if (user_details != null)
                        {
                            System.Web.HttpContext.Current.Session["LastLogin"] = Convert.ToDateTime(user_details.last_login).ToString("dd MMM hh:mm tt");
                            user_details.last_login = DateTime.Now;
                            db.Entry(user_details).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                        is_available = true;
                    }
                }
                catch { }
            }
            return is_available;
        }
        public List<SelectListItem> GetSrchMarketPalceList(int? sellers_id)
        {
            //int sellers_id = Convert.ToInt32(Session["SellerID"]);
            List<SelectListItem> lst_Marketplace = new List<SelectListItem>();
            lst_Marketplace.Add(SelectAllItems);
            var getMarketPlace = db.m_marketplace.Where(a => a.isactive == 1).ToList();
            var get_sellermarketplace = dba.tbl_sellermarketplace.Where(a => a.isactive == 1 && a.tbl_seller_id == sellers_id).ToList();
            foreach (var items in getMarketPlace)
            {
                lst_Marketplace.Add(new SelectListItem { Value = items.id.ToString(), Text = items.name });
            }
            return lst_Marketplace;
        }
        public List<SelectListItem> GetMarket_Place(int? sellers_id)
        {
            List<SelectListItem> lst = new List<SelectListItem>();
            //lst.Add(SelectAllItems);

            var getdata = dba.tbl_sellermarketplace.Where(a => a.tbl_seller_id == sellers_id && a.isactive == 1).ToList();
            foreach (var items in getdata)
            {
                var getmarketplace = db.m_marketplace.Where(a => a.id == items.m_marketplace_id && a.isactive == 1).FirstOrDefault();
                if (getmarketplace != null)
                {
                    lst.Add(new SelectListItem { Value = getmarketplace.id.ToString(), Text = getmarketplace.name });
                }
            }
            return lst;
        }
        public List<SelectListItem> GetMarketPlace(int? sellers_id)
        {
            List<SelectListItem> lst = new List<SelectListItem>();
            lst.Add(SelectAllItems);

            var getdata = dba.tbl_sellermarketplace.Where(a => a.tbl_seller_id == sellers_id && a.isactive == 1).ToList();
            foreach (var items in getdata)
            {
                var getmarketplace = db.m_marketplace.Where(a => a.id == items.m_marketplace_id && a.isactive == 1).FirstOrDefault();
                if (getmarketplace != null)
                {
                    lst.Add(new SelectListItem { Value = getmarketplace.id.ToString(), Text = getmarketplace.name });
                }
            }
            return lst;
        }
        public List<SelectListItem> GetExpenseList()
        {
            List<SelectListItem> lst_Expense = new List<SelectListItem>();
            lst_Expense.Add(new SelectListItem { Text = "-Select-", Value = "" });
            lst_Expense.Add(new SelectListItem { Text = "FBA Weight Handling Fee", Value = "1" });
            lst_Expense.Add(new SelectListItem { Text = "Technology Fee", Value = "2" });
            lst_Expense.Add(new SelectListItem { Text = "Commission", Value = "3" });
            lst_Expense.Add(new SelectListItem { Text = "Fixed closing fee", Value = "4" });
            lst_Expense.Add(new SelectListItem { Text = "Shipping Chargeback", Value = "5" });
            lst_Expense.Add(new SelectListItem { Text = "Shipping discount", Value = "6" });
            lst_Expense.Add(new SelectListItem { Text = "Refund commission", Value = "7" });
            return lst_Expense;
        }

        public List<SelectListItem> GetExportList()
        {
            List<SelectListItem> lst_export = new List<SelectListItem>();
            lst_export.Add(new SelectListItem { Text = "-Select-", Value = "" });
            lst_export.Add(new SelectListItem { Text = "Tally", Value = "1" });
            lst_export.Add(new SelectListItem { Text = "Busy", Value = "2" });
            lst_export.Add(new SelectListItem { Text = "XML", Value = "3" });
            return lst_export;
        }

        public List<SelectListItem> GetExcelExportList()
        {
            List<SelectListItem> lst_export = new List<SelectListItem>();
            lst_export.Add(new SelectListItem { Text = "-Select-", Value = "" });
            lst_export.Add(new SelectListItem { Text = "Excel", Value = "1" });
            lst_export.Add(new SelectListItem { Text = "PDF", Value = "2" });
            return lst_export;
        }
        public List<SelectListItem> GetFullilmentList()
        {
            List<SelectListItem> lst_export = new List<SelectListItem>();
            lst_export.Add(new SelectListItem { Text = "-Select-", Value = "" });
            lst_export.Add(new SelectListItem { Text = "MFN", Value = "1" });
            lst_export.Add(new SelectListItem { Text = "AFN", Value = "2" });
            lst_export.Add(new SelectListItem { Text = "Both", Value = "3" });
            return lst_export;
        }

        public List<SelectListItem> GetPercentageList()
        {
            List<SelectListItem> lst_export = new List<SelectListItem>();
            lst_export.Add(new SelectListItem { Text = "-Select-", Value = "0" });
            lst_export.Add(new SelectListItem { Text = "0% to -100%", Value = "1" });           
            lst_export.Add(new SelectListItem { Text = "0% to 25%", Value = "2" });
            lst_export.Add(new SelectListItem { Text = "25.1% to 50%", Value = "3" });
            lst_export.Add(new SelectListItem { Text = "50.1% to 60%", Value = "4" });
            lst_export.Add(new SelectListItem { Text = "60.1% to 70%", Value = "5" });
            lst_export.Add(new SelectListItem { Text = "70.1% to 80%", Value = "6" });
            lst_export.Add(new SelectListItem { Text = "80.1% to 100%", Value = "7" });
            //ViewData["Checkboxlist"] = lst_export;    
            return lst_export;
        }
        public List<SelectListItem> GetEFTCODList()
        {
            List<SelectListItem> lst_export = new List<SelectListItem>();
            lst_export.Add(new SelectListItem { Text = "-Select-", Value = "" });
            lst_export.Add(new SelectListItem { Text = "EFT", Value = "1" });
            lst_export.Add(new SelectListItem { Text = "COD", Value = "2" });
            return lst_export;
        }


        public string GetMonth(int iMonth)
        {
            if (iMonth <= 0)
            {
                iMonth = 12 + iMonth;
            }
            if (iMonth == 1)
            {
                return "01";
            }
            else if(iMonth ==2)
            {           
                return "02";
            }
            else if (iMonth == 3)
            {
                return "03";
            }
            else if (iMonth == 4)
            {
                return "04";
            }
            else if (iMonth == 5)
            {
                return "05";
            }
            else if (iMonth == 6)
            {
                return "06";
            }
            else if (iMonth == 7)
            {
                return "07";
            }
            else if (iMonth == 8)
            {
                return "08";
            }
            else if (iMonth == 9)
            {
                return "09";
            }
            else if (iMonth == 10)
            {
                return "10";
            }
            else if (iMonth == 11)
            {
                return "11";
            }
            else if(iMonth == 12)
            {
                return "12";
            }

            return "";
        }

        public string GetYear(int Month)
        {
            if (Month <= 0)
            {
                return (DateTime.Now.Year - 1).ToString();
            }
            else
            {
                return (DateTime.Now.Year).ToString();
            }
           
        }
        public string GetEndDate(string Month)
        {
            if (Month == "01")
            {
                return "31";
            }
            else if (Month == "02")
            {
                return "28";
            }
            else if (Month == "03")
            {
                return "31";
            }
            else if (Month == "04")
            {
                return "30";
            }
            else if (Month == "05")
            {
                return "31";
            }
            else if (Month == "06")
            {
                return "30";
            }
            else if (Month == "07")
            {
                return "31";
            }
            else if (Month == "08")
            {
                return "31";
            }
            else if (Month == "09")
            {
                return "30";
            }
            else if (Month == "10")
            {
                return "31";
            }
            else if (Month == "11")
            {
                return "30";
            }
            else if (Month == "12")
            {
                return "31";
            }
            return "";
        }

        public DateTime MyDateTimeConverter(string dateString)
        {
            DateTime dateTime;
            if (DateTime.TryParseExact(dateString, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime))
            {
                Console.WriteLine(dateTime);
            }
            else
            {
                DateTimeOffset dto = DateTimeOffset.Parse(dateString);

                //Get the date object from the string. 
                dateTime = dto.DateTime;
                //String dfc = dtObject.ToString("dd-MM-yyyy HH:mm:ss");
            }

            return dateTime;
        }
        public List<SelectListItem> GetSrchReferenceList(int? PartID, int? sellers_id, int? marketplaceID)
        {
            List<SelectListItem> lst_cat = new List<SelectListItem>();
            lst_cat.Add(SelectAllItems);
            //if (PartID != 0 && PartID != null)
            //{
            if (marketplaceID != 0 && marketplaceID != null)
            {
                var getDepartment = dba.tbl_settlement_upload.Where(a => a.tbl_seller_id == sellers_id && a.status == 0).ToList();
                if (PartID != null && PartID != 0 && getDepartment != null)
                    getDepartment = getDepartment.Where(a => a.settlement_type == PartID).ToList();
                if (marketplaceID != null && marketplaceID != 0 && getDepartment != null)
                    getDepartment = getDepartment.Where(a => a.market_place_id == marketplaceID).ToList();
                foreach (var items in getDepartment)
                {
                    string depositeDate = "";
                    DateTime date = Convert.ToDateTime(items.deposit_date);
                    lst_cat.Add(new SelectListItem { Value = items.Id.ToString(), Text = items.settlement_refernece_no + " (" + date.ToString("dd-MMM-yyy") + ")" });
                }
            }
           //}
            return lst_cat;
        }
        public List<SelectListItem> GetSearchEFTCODList()
        {
            List<SelectListItem> lst_export = new List<SelectListItem>();
            lst_export.Add(new SelectListItem { Text = "-All-", Value = "0" });
            lst_export.Add(new SelectListItem { Text = "EFT", Value = "1" });
            lst_export.Add(new SelectListItem { Text = "COD", Value = "2" });
            return lst_export;
        }

        public DataTable CreateNetrealizationReportWithTax(List<SaleReport> lst_seminar)
        {
            DataTable export_dt = new DataTable();
            export_dt.Columns.Add("Sr. No.", typeof(string));
            export_dt.Columns.Add("MarketPlace Name", typeof(string));
            export_dt.Columns.Add("OrderID", typeof(string));
            export_dt.Columns.Add("Sale-Order Date", typeof(string));
            export_dt.Columns.Add("Product Name", typeof(string));
            export_dt.Columns.Add("SKU-No", typeof(string));
            export_dt.Columns.Add("Item Price", typeof(string));
            export_dt.Columns.Add("Sale Value", typeof(string));
            export_dt.Columns.Add("Expenses Deducted by Portal", typeof(string));
            export_dt.Columns.Add("Net Realization", typeof(string));
            export_dt.Columns.Add("Profit/Loss Per Order", typeof(string));

            int s = 1;
            foreach (var Data in lst_seminar)
            {
                DataRow row = export_dt.NewRow();
                if (Data == null)
                    continue;
                row["Sr. No."] = s + ".";
                row[0] = Data.MarketPlaceName;
                row[1] = Data.OrderID;
                row[2] = Data.OrderDate;
                row[3] = Data.ProductName;
                row[4] = Data.skuNo;
                row[5] = Data.ProductValue;
                row[6] = Data.SumOrder;
                row[7] = Data.FullExpenseTotal;
                row[8] = Data.NetTotal;
                row[9] = Data.Profit_lossAmount;

                export_dt.Rows.Add(row);
                s++;
            }
            return export_dt;

        }




        public DataTable CreateTallyVoucherDatatable(List<SaleReport> lst_seminar)
        {
            DataTable export_dt = new DataTable();
            //export_dt.Columns.Add("Sr. No.", typeof(string));
            export_dt.Columns.Add("Voucher Date", typeof(string));
            export_dt.Columns.Add("Voucher Number", typeof(string));
            export_dt.Columns.Add("Ledger Name", typeof(string));
            export_dt.Columns.Add("Reference Number", typeof(string));
            export_dt.Columns.Add("Dr Amount", typeof(string));
            export_dt.Columns.Add("Cr Amount", typeof(string));
            export_dt.Columns.Add("Narration", typeof(string));

            int s = 1;
            foreach (var Data in lst_seminar)
            {
                DataRow row = export_dt.NewRow();
                if (Data == null)
                    continue;
                //row["Sr. No."] = s + ".";
                row[0] = Data.Sett_orderDate;
                row[1] = Data.VoucherNumber;
                row[2] = Data.ExpenseName;
                row[3] = Data.OrderID;
                row[4] = Data.refund_SumOrder;
                row[5] = Data.SumOrder;
                row[6] = Data.Narration;

                export_dt.Rows.Add(row);
                s++;
            }
            return export_dt;

        }

        //public DataTable CreateNetRealizationDatatable(List<SaleReport> lst_seminar)
        //{
        //    DataTable export_dt = new DataTable();
        //    export_dt.Columns.Add("Sr. No.", typeof(string));
        //    export_dt.Columns.Add("OrderID", typeof(string));
        //    export_dt.Columns.Add("Product Name", typeof(string));
        //    export_dt.Columns.Add("SKU N0", typeof(string));
        //    export_dt.Columns.Add("Product Value", typeof(string));
        //    export_dt.Columns.Add("IGST", typeof(string));
        //    export_dt.Columns.Add("SGST", typeof(string));
        //    export_dt.Columns.Add("CGST", typeof(string));
        //    export_dt.Columns.Add("Total", typeof(string));

        //    export_dt.Columns.Add("Reference-ID", typeof(string));
        //    export_dt.Columns.Add("Order-Product Value", typeof(string));
        //    export_dt.Columns.Add("Order-Commission", typeof(string));
        //    export_dt.Columns.Add("Order-FBA Weight Handling Fee", typeof(string));
        //    export_dt.Columns.Add("Order-Fixed closing fee", typeof(string));
        //    export_dt.Columns.Add("Order-Shipping Chargeback", typeof(string));
        //    export_dt.Columns.Add("Order-Technology Fee", typeof(string));
        //    export_dt.Columns.Add("Order-Shipping Discount", typeof(string));
        //    export_dt.Columns.Add("Order-Shipping Commission", typeof(string));
        //    export_dt.Columns.Add("Easy Ship weight handling fees", typeof(string));
        //    export_dt.Columns.Add("FBA Pick & Pack Fee", typeof(string));
        //    export_dt.Columns.Add("Gift Wrap Chargeback", typeof(string));
        //    export_dt.Columns.Add("Amazon Easy Ship Charges", typeof(string));
        //    export_dt.Columns.Add("Order-IGST", typeof(string));
        //    export_dt.Columns.Add("Order-CGST", typeof(string));
        //    export_dt.Columns.Add("Order-SGST", typeof(string));
        //    export_dt.Columns.Add("Order-Realization", typeof(string));

        //    export_dt.Columns.Add("Refund_Ref_No", typeof(string));
        //    export_dt.Columns.Add("Refund_Product Value", typeof(string));
        //    export_dt.Columns.Add("Refund_Commission", typeof(string));
        //    export_dt.Columns.Add("Refund_FBA Weight Handling Fee", typeof(string));
        //    export_dt.Columns.Add("Refund_Fixed closing fee", typeof(string));
        //    export_dt.Columns.Add("Refund_Shipping Chargeback", typeof(string));
        //    export_dt.Columns.Add("Refund_Technology Fee", typeof(string));
        //    export_dt.Columns.Add("Refund_Shipping Discount", typeof(string));
        //    export_dt.Columns.Add("Refund_Refund Commission", typeof(string));
        //    export_dt.Columns.Add("Refund_Shipping Commission", typeof(string));
        //    export_dt.Columns.Add("Refund Easy Ship weight handling fees", typeof(string));
        //    export_dt.Columns.Add("Refund FBA Pick & Pack Fee", typeof(string));
        //    export_dt.Columns.Add("Refund Gift Wrap Chargeback", typeof(string));
        //    export_dt.Columns.Add("Refund Amazon Easy Ship Charges", typeof(string)); 
        //    export_dt.Columns.Add("Refund_IGST", typeof(string));
        //    export_dt.Columns.Add("Refund_CGST", typeof(string));
        //    export_dt.Columns.Add("Refund_SGST", typeof(string));
        //    export_dt.Columns.Add("Amount Paid", typeof(string));

        //    export_dt.Columns.Add("Net Realization", typeof(string));
        //    export_dt.Columns.Add("Net Realization %", typeof(string));
        //    export_dt.Columns.Add("Order Expenses", typeof(string));
        //    export_dt.Columns.Add("Refund Expenses", typeof(string));


        //    int s = 1;
        //    foreach (var Data in lst_seminar)
        //    {
        //        DataRow row = export_dt.NewRow();
        //        if (Data == null)
        //            continue;
        //        row["Sr. No."] = s + ".";
        //        row[1] = Data.OrderID;
        //        row[2] = Data.ProductName;
        //        row[3] = Data.skuNo;
        //        row[4] = Data.Principal;
        //        row[5] = Data.orderigst;
        //        row[6] = Data.ordersgst;
        //        row[7] = Data.ordercgst;
        //        row[8] = Data.ordertotal;

        //        row[9] = Data.ReferenceID;
        //        row[10] = Data.orderTotal;
        //        row[11] = Data.CommissionFee;
        //        row[12] = Data.FBAFEE;
        //        row[13] = Data.FixedClosingFee;
        //        row[14] = Data.ShippingChargebackFee;
        //        row[15] = Data.TechnologyFee;
        //        row[16] = Data.ShippingDiscountFee;
        //        row[17] = Data.ShippingCommision;
        //        row[18] = Data.EasyShipweighthandlingfees;
        //        row[19] = Data.FBAPickPackFee;
        //        row[20] = Data.GiftWrapChargeback;
        //        row[21] = Data.AmazonEasyShipCharges; 
        //        row[22] = Data.SUMIGST;
        //        row[23] = Data.SUMCGST;
        //        row[24] = Data.SUMSGST;
        //        row[25] = Data.SumOrder;


        //        row[26] = Data.refundReferenceID;
        //        row[27] = Data.refundTotal;
        //        row[28] = Data.RefundCommissionFee;
        //        row[29] = Data.RefundFBAFEE;
        //        row[30] = Data.RefundFixedClosingFee;
        //        row[31] = Data.RefundShippingChargebackFee;
        //        row[32] = Data.RefundTechnologyFee;
        //        row[33] = Data.RefundShippingDiscountFee;
        //        row[34] = Data.Refund_Commision;
        //        row[35] = Data.Refund_ShippingCommision;
        //        row[36] = Data.Refund_EasyShipweighthandlingfees;
        //        row[37] = Data.Refund_FBAPick_PackFee;
        //        row[38] = Data.Refund_GiftWrapChargeback;
        //        row[39] = Data.Refund_AmazonEasyShipCharges;
        //        row[40] = Data.Refund_SUMIGST;
        //        row[41] = Data.Refund_SUMCGST;
        //        row[42] = Data.Refund_SUMSGST;
        //        row[43] = Data.refund_SumOrder;

        //        row[44] = Data.NetTotal;
        //        row[45] = Data.PercentageAmount;
        //        row[46] = Data.SumFee;
        //        row[47] = Data.refund_SumFee;

        //        export_dt.Rows.Add(row);
        //        s++;
        //    }
        //    return export_dt;

        //}

        public DataTable CreateNetRealizationDatatable(List<SaleReport> lst_seminar)
        {
            DataTable export_dt = new DataTable();
            export_dt.Columns.Add("Sr. No.", typeof(string));
            export_dt.Columns.Add("OrderID", typeof(string));
            export_dt.Columns.Add("Product Name", typeof(string));
            export_dt.Columns.Add("SKU N0", typeof(string));
            export_dt.Columns.Add("Product Value", typeof(string));
            export_dt.Columns.Add("IGST", typeof(string));
            export_dt.Columns.Add("SGST", typeof(string));
            export_dt.Columns.Add("CGST", typeof(string));
            export_dt.Columns.Add("Total", typeof(string));

            export_dt.Columns.Add("Order Reference-ID", typeof(string));
            export_dt.Columns.Add("Refund Ref_No", typeof(string));
            export_dt.Columns.Add("Order Product Value", typeof(string));
            export_dt.Columns.Add("Commission", typeof(string));
            export_dt.Columns.Add("FBA Weight Handling Fee", typeof(string));
            export_dt.Columns.Add("Fixed closing fee", typeof(string));
            export_dt.Columns.Add("Shipping Chargeback", typeof(string));
            export_dt.Columns.Add("Technology Fee", typeof(string));
            export_dt.Columns.Add("Shipping Discount", typeof(string));
            export_dt.Columns.Add("Shipping Commission", typeof(string));
            export_dt.Columns.Add("Easy Ship weight handling fees", typeof(string));
            export_dt.Columns.Add("FBA Pick & Pack Fee", typeof(string));
            export_dt.Columns.Add("Gift Wrap Chargeback", typeof(string));
            export_dt.Columns.Add("Amazon Easy Ship Charges", typeof(string));
            export_dt.Columns.Add("Amazon Refund Commission", typeof(string));
            export_dt.Columns.Add("Total IGST", typeof(string));
            export_dt.Columns.Add("Total CGST", typeof(string));
            export_dt.Columns.Add("Total SGST", typeof(string));
                       
            export_dt.Columns.Add("Net Realization", typeof(string));
            export_dt.Columns.Add("Net Realization %", typeof(string));
            export_dt.Columns.Add("Order Expenses", typeof(string));
            export_dt.Columns.Add("Refund Expenses", typeof(string));


            int s = 1;
            foreach (var Data in lst_seminar)
            {
                DataRow row = export_dt.NewRow();
                if (Data == null)
                    continue;
                row["Sr. No."] = s + ".";
                row[1] = Data.OrderID;
                row[2] = Data.ProductName;
                row[3] = Data.skuNo;
                row[4] = Data.Principal;
                row[5] = Data.orderigst;
                row[6] = Data.ordersgst;
                row[7] = Data.ordercgst;
                row[8] = Data.ordertotal;

                row[9] = Data.ReferenceID;
                row[10] = Data.refundReferenceID;
                row[11] = Data.ActualOrderTotal;
                row[12] = Data.ActualCommission;
                row[13] = Data.ActualFBAFee;
                row[14] = Data.ActualFixedClosingFee;
                row[15] = Data.ActualShippingChargebackFee;
                row[16] = Data.ActualTechnologyFee;
                row[17] = Data.ActualShippingDiscountFee;
                row[18] = Data.ActualShippingCommision;
                row[19] = Data.ActualEasyShipWeightFee;
                row[20] = Data.ActualFBAPickPackFee;
                row[21] = Data.ActualGiftWrapChargeback;
                row[22] = Data.ActualAmazonEasyShipCharges;
                row[23] = Data.ActualRefundCommission;
                row[24] = Data.ActualIGST;
                row[25] = Data.ActualCGST;
                row[26] = Data.ActualSGST;
                row[27] = Data.ActualNetTotal;                        
                row[28] = Data.PercentageAmount;
                row[29] = Data.SumFee;
                row[30] = Data.refund_SumFee;

                export_dt.Rows.Add(row);
                s++;
            }
            return export_dt;

        }


        public DataTable CreateNetRealizationWithOutTaxDatatable(List<SaleReport> lst_seminar)
        {
            DataTable export_dt = new DataTable();
            export_dt.Columns.Add("Sr. No.", typeof(string));
            export_dt.Columns.Add("OrderID", typeof(string));
            export_dt.Columns.Add("SKU No", typeof(string));
            export_dt.Columns.Add("Item Name", typeof(string));
            export_dt.Columns.Add("Item Price", typeof(string));
            export_dt.Columns.Add("Total", typeof(string));

            export_dt.Columns.Add("Order Reference-ID", typeof(string));
            export_dt.Columns.Add("Refund_Ref_No", typeof(string));
            export_dt.Columns.Add("Product Value", typeof(string));
            export_dt.Columns.Add("Commission", typeof(string));
            export_dt.Columns.Add("FBA Weight Handling Fee", typeof(string));
            export_dt.Columns.Add("Fixed closing fee", typeof(string));
            export_dt.Columns.Add("Shipping Chargeback", typeof(string));
            export_dt.Columns.Add("Technology Fee", typeof(string));
            export_dt.Columns.Add("Shipping Discount", typeof(string));
            export_dt.Columns.Add("Shipping Commission", typeof(string));
            export_dt.Columns.Add("Easy Ship weight handling fees", typeof(string));
            export_dt.Columns.Add("FBA Pick & Pack Fee", typeof(string));
            export_dt.Columns.Add("Gift Wrap Chargeback", typeof(string));
            export_dt.Columns.Add("Amazon Easy Ship Charges", typeof(string));        
            export_dt.Columns.Add("Net Realization", typeof(string));
            export_dt.Columns.Add("Net Realization %", typeof(string));
            export_dt.Columns.Add("Profit/Loss Per Order", typeof(string));
            export_dt.Columns.Add("Order Expenses", typeof(string));
            export_dt.Columns.Add("Refund Expenses", typeof(string));


            int s = 1;
            foreach (var Data in lst_seminar)
            {
                DataRow row = export_dt.NewRow();
                if (Data == null)
                    continue;
                row["Sr. No."] = s + ".";
                row[1] = Data.OrderID;
                row[2] = Data.skuNo;
                row[3] = Data.ProductName;
                row[4] = Data.ProductValue;
                row[5] = Data.ordertotal;

                row[6] = Data.ReferenceID;
                row[7] = Data.refundReferenceID;
                row[8] = Data.ActualOrderTotal;
                row[9] = Data.ActualCommission;
                row[10] = Data.ActualFBAFee;
                row[11] = Data.ActualFixedClosingFee;
                row[12] = Data.ActualShippingChargebackFee;
                row[13] = Data.ActualTechnologyFee;
                row[14] = Data.ActualShippingDiscountFee;
                row[15] = Data.ActualShippingCommision;
                row[16] = Data.ActualEasyShipWeightFee;
                row[17] = Data.ActualFBAPickPackFee;
                row[18] = Data.ActualGiftWrapChargeback;
                row[19] = Data.ActualAmazonEasyShipCharges;
           
                row[20] = Data.ActualNetTotal;
                row[21] = Data.PercentageAmount;
                row[22] = Data.Profit_lossAmount;
                row[23] = Data.SumFee;
                row[24] = Data.refund_SumFee;

                export_dt.Rows.Add(row);
                s++;
            }
            return export_dt;

        }

        public DataTable CreateNetRealizationWithOutTaxDatatable1(List<SaleReport> lst_seminar)
        {
            DataTable export_dt = new DataTable();
            export_dt.Columns.Add("Sr. No.", typeof(string));
            export_dt.Columns.Add("OrderID", typeof(string));

            export_dt.Columns.Add("SKU No", typeof(string));
            export_dt.Columns.Add("Item Name", typeof(string));
            export_dt.Columns.Add("Item Price", typeof(string));

            export_dt.Columns.Add("Total", typeof(string));

            export_dt.Columns.Add("Reference-ID", typeof(string));
            export_dt.Columns.Add("Order-Product Value", typeof(string));
            export_dt.Columns.Add("Order-Commission", typeof(string));
            export_dt.Columns.Add("Order-FBA Weight Handling Fee", typeof(string));
            export_dt.Columns.Add("Order-Fixed closing fee", typeof(string));
            export_dt.Columns.Add("Order-Shipping Chargeback", typeof(string));
            export_dt.Columns.Add("Order-Technology Fee", typeof(string));
            export_dt.Columns.Add("Order-Shipping Discount", typeof(string));
            export_dt.Columns.Add("Order-Shipping Commission", typeof(string));
            export_dt.Columns.Add("Easy Ship weight handling fees", typeof(string));
            export_dt.Columns.Add("FBA Pick & Pack Fee", typeof(string));
            export_dt.Columns.Add("Gift Wrap Chargeback", typeof(string));
            export_dt.Columns.Add("Amazon Easy Ship Charges", typeof(string));
            export_dt.Columns.Add("Order-Realization", typeof(string));

            export_dt.Columns.Add("Refund_Ref_No", typeof(string));
            export_dt.Columns.Add("Refund_Product Value", typeof(string));
            export_dt.Columns.Add("Refund_Commission", typeof(string));
            export_dt.Columns.Add("Refund_FBA Weight Handling Fee", typeof(string));
            export_dt.Columns.Add("Refund_Fixed closing fee", typeof(string));
            export_dt.Columns.Add("Refund_Shipping Chargeback", typeof(string));
            export_dt.Columns.Add("Refund_Technology Fee", typeof(string));
            export_dt.Columns.Add("Refund_Shipping Discount", typeof(string));
            export_dt.Columns.Add("Refund_Refund Commission", typeof(string));
            export_dt.Columns.Add("Refund_Shipping Commission", typeof(string));
            export_dt.Columns.Add("Refund Easy Ship weight handling fees", typeof(string));
            export_dt.Columns.Add("Refund FBA Pick & Pack Fee", typeof(string));
            export_dt.Columns.Add("Refund Gift Wrap Chargeback", typeof(string));
            export_dt.Columns.Add("Refund Amazon Easy Ship Charges", typeof(string)); 
            export_dt.Columns.Add("Amount Paid", typeof(string));

            export_dt.Columns.Add("Net Realization", typeof(string));
            export_dt.Columns.Add("Net Realization %", typeof(string));
            export_dt.Columns.Add("Profit/Loss Per Order", typeof(string)); 
            export_dt.Columns.Add("Order Expenses", typeof(string));
            export_dt.Columns.Add("Refund Expenses", typeof(string));


            int s = 1;
            foreach (var Data in lst_seminar)
            {
                DataRow row = export_dt.NewRow();
                if (Data == null)
                    continue;
                row["Sr. No."] = s + ".";
                row[1] = Data.OrderID;

                row[2] = Data.skuNo;
                row[3] = Data.ProductName;
                row[4] = Data.ProductValue;

                row[5] = Data.ordertotal;

                row[6] = Data.ReferenceID;
                row[7] = Data.orderTotal;
                row[8] = Data.CommissionFee;
                row[9] = Data.FBAFEE;
                row[10] = Data.FixedClosingFee;
                row[11] = Data.ShippingChargebackFee;
                row[12] = Data.TechnologyFee;
                row[13] = Data.ShippingDiscountFee;
                row[14] = Data.ShippingCommision;
                row[15] = Data.EasyShipweighthandlingfees;
                row[16] = Data.FBAPickPackFee;
                row[17] = Data.GiftWrapChargeback;
                row[18] = Data.AmazonEasyShipCharges; 
                row[19] = Data.SumOrder;

                row[20] = Data.refundReferenceID;
                row[21] = Data.refundTotal;
                row[22] = Data.RefundCommissionFee;
                row[23] = Data.RefundFBAFEE;
                row[24] = Data.RefundFixedClosingFee;
                row[25] = Data.RefundShippingChargebackFee;
                row[26] = Data.RefundTechnologyFee;
                row[27] = Data.RefundShippingDiscountFee;
                row[28] = Data.Refund_Commision;
                row[29] = Data.Refund_ShippingCommision;
                row[30] = Data.Refund_EasyShipweighthandlingfees;
                row[31] = Data.Refund_FBAPick_PackFee;
                row[32] = Data.Refund_GiftWrapChargeback;
                row[33] = Data.Refund_AmazonEasyShipCharges;
                 
                row[34] = Data.refund_SumOrder;

                row[35] = Data.NetTotal;
                row[36] = Data.PercentageAmount;
                row[37] = Data.Profit_lossAmount;   
                row[38] = Data.SumFee;
                row[39] = Data.refund_SumFee;

                export_dt.Rows.Add(row);
                s++;
            }
            return export_dt;

        }

        public DataTable CreateNetRealizationWithOutTaxFlipkart(List<SaleReport> lst_seminar)
        {
            DataTable export_dt = new DataTable();
            export_dt.Columns.Add("Sr. No.", typeof(string));
            export_dt.Columns.Add("OrderID", typeof(string));
            export_dt.Columns.Add("SKU No", typeof(string));
            export_dt.Columns.Add("Item Name", typeof(string));
            export_dt.Columns.Add("Item Price", typeof(string));
            export_dt.Columns.Add("Total", typeof(string));

            export_dt.Columns.Add("Order Reference Payment-ID", typeof(string));
            export_dt.Columns.Add("Refund Reference Payment-ID", typeof(string));
            export_dt.Columns.Add("Order-Product Value", typeof(string));
            export_dt.Columns.Add("Commission", typeof(string));
            export_dt.Columns.Add("Shipping Fee", typeof(string));
            export_dt.Columns.Add("Collection Fee", typeof(string));
            export_dt.Columns.Add("Reverse Shipping Fee", typeof(string));
            export_dt.Columns.Add("Fixed Fee", typeof(string));

            export_dt.Columns.Add("Total Expenses", typeof(string));          
            export_dt.Columns.Add("Net Realization", typeof(string));
            export_dt.Columns.Add("Net Realization %", typeof(string));
            


            int s = 1;
            foreach (var Data in lst_seminar)
            {
                DataRow row = export_dt.NewRow();
                if (Data == null)
                    continue;
                row["Sr. No."] = s + ".";
                row[1] = Data.OrderID;
                row[2] = Data.skuNo;
                row[3] = Data.ProductName;
                row[4] = Data.ProductValue;
                row[5] = Data.ordertotal;

                row[6] = Data.ReferenceID;
                row[7] = Data.refundReferenceID;
                row[8] = Data.Flip_Totalordervalue;
                row[9] = Data.Flip_Totalcommission;
                row[10] = Data.Flip_Totalshippingfee;
                row[11] = Data.Flip_Totalcollectionfee;
                row[12] = Data.Flip_Totalreverseshippingfee;
                row[13] = Data.Flip_Totalfixedfee;

                row[14] = Data.FullExpenseTotal;            
                row[15] = Data.NetTotal;
                row[16] = Data.PercentageAmount;              
                export_dt.Rows.Add(row);
                s++;
            }
            return export_dt;

        }

        public DataTable CreateNetRealizationWithOutTaxPaytm(List<SaleReport> lst_seminar)
        {
            DataTable export_dt = new DataTable();
            export_dt.Columns.Add("Sr. No.", typeof(string));
            export_dt.Columns.Add("OrderID", typeof(string));
            export_dt.Columns.Add("SKU No", typeof(string));
            export_dt.Columns.Add("Item Name", typeof(string));
            export_dt.Columns.Add("Item Price", typeof(string));
            export_dt.Columns.Add("Total", typeof(string));

            export_dt.Columns.Add("Order Reference Payment-ID", typeof(string));
            export_dt.Columns.Add("Refund Reference Payment-ID", typeof(string));
            export_dt.Columns.Add("Order-Product Value", typeof(string));

            export_dt.Columns.Add("Marketplace Commission", typeof(string));
            export_dt.Columns.Add("Logistics Charges", typeof(string));
            export_dt.Columns.Add("PG Commission", typeof(string));
            export_dt.Columns.Add("Penalty", typeof(string));
            export_dt.Columns.Add("Net Adjustments", typeof(string));

            export_dt.Columns.Add("Total Expenses", typeof(string));
            export_dt.Columns.Add("Net Realization", typeof(string));
            export_dt.Columns.Add("Net Realization %", typeof(string));



            int s = 1;
            foreach (var Data in lst_seminar)
            {
                DataRow row = export_dt.NewRow();
                if (Data == null)
                    continue;
                row["Sr. No."] = s + ".";
                row[1] = Data.OrderID;
                row[2] = Data.skuNo;
                row[3] = Data.ProductName;
                row[4] = Data.ProductValue;
                row[5] = Data.ordertotal;

                row[6] = Data.ReferenceID;
                row[7] = Data.refundReferenceID;
                row[8] = Data.Flip_Totalordervalue;

                row[9] = Data.ActualFBAFee;
                row[10] = Data.ActualTechnologyFee;
                row[11] = Data.ActualCommission;
                row[12] = Data.ActualFixedClosingFee;
                row[13] = Data.ActualShippingChargebackFee;

                row[14] = Data.FullExpenseTotal;
                row[15] = Data.ActualNetTotal;
                row[16] = Data.PercentageAmount;
                export_dt.Rows.Add(row);
                s++;
            }
            return export_dt;

        }

        public DataTable CreateNetRealizationDatatableFlipkart(List<SaleReport> lst_seminar)
        {
            DataTable export_dt = new DataTable();
            export_dt.Columns.Add("Sr. No.", typeof(string));
            export_dt.Columns.Add("OrderID", typeof(string));
            export_dt.Columns.Add("Product Name", typeof(string));
            export_dt.Columns.Add("SKU N0", typeof(string));
            export_dt.Columns.Add("Product Value", typeof(string));
            export_dt.Columns.Add("IGST", typeof(string));
            export_dt.Columns.Add("SGST", typeof(string));
            export_dt.Columns.Add("CGST", typeof(string));
            export_dt.Columns.Add("Total", typeof(string));

            export_dt.Columns.Add("Reference-ID", typeof(string));
            export_dt.Columns.Add("Order-Product Value", typeof(string));
            export_dt.Columns.Add("Order-Commission", typeof(string));
            export_dt.Columns.Add("Order-Shipping Fee", typeof(string));
            export_dt.Columns.Add("Order-Collection Fee", typeof(string));
            export_dt.Columns.Add("Order-Reverse Shipping Fee", typeof(string));
            export_dt.Columns.Add("Order-Fixed Fee", typeof(string));           
            export_dt.Columns.Add("Order-IGST", typeof(string));
            export_dt.Columns.Add("Order-CGST", typeof(string));
            export_dt.Columns.Add("Order-SGST", typeof(string));
            export_dt.Columns.Add("Order-Realization", typeof(string));

            export_dt.Columns.Add("Refund_Ref_No", typeof(string));
            export_dt.Columns.Add("Refund_Product Value", typeof(string));
            export_dt.Columns.Add("Refund_Commission", typeof(string));
            export_dt.Columns.Add("Refund_Shipping Fee", typeof(string));
            export_dt.Columns.Add("Refund_Collection Fee", typeof(string));
            export_dt.Columns.Add("Refund_Reverse Shipping Fee", typeof(string));
            export_dt.Columns.Add("Refund_Fixed Fee", typeof(string));           
            export_dt.Columns.Add("Refund_IGST", typeof(string));
            export_dt.Columns.Add("Refund_CGST", typeof(string));
            export_dt.Columns.Add("Refund_SGST", typeof(string));
            export_dt.Columns.Add("Amount Paid", typeof(string));

            export_dt.Columns.Add("Net Realization", typeof(string));
            export_dt.Columns.Add("Net Realization %", typeof(string));
            export_dt.Columns.Add("Order Expenses", typeof(string));
            export_dt.Columns.Add("Refund Expenses", typeof(string));

            int s = 1;
            foreach (var Data in lst_seminar)
            {
                DataRow row = export_dt.NewRow();
                if (Data == null)
                    continue;
                row["Sr. No."] = s + ".";
                row[1] = Data.OrderID;
                row[2] = Data.ProductName;
                row[3] = Data.skuNo;
                row[4] = Data.Principal;
                row[5] = Data.orderigst;
                row[6] = Data.ordersgst;
                row[7] = Data.ordercgst;
                row[8] = Data.ordertotal;

                row[9] = Data.ReferenceID;
                row[10] = Data.orderTotal;
                row[11] = Data.CommissionFee;
                row[12] = Data.flipShipping;
                row[13] = Data.flipCollection;
                row[14] = Data.flipReverseShipping;
                row[15] = Data.flipFixedFee;                
                row[16] = Data.SUMIGST;
                row[17] = Data.SUMCGST;
                row[18] = Data.SUMSGST;
                row[19] = Data.SumOrder;

                row[20] = Data.refundReferenceID;
                row[21] = Data.refundTotal;
                row[22] = Data.RefundCommissionFee;
                row[23] = Data.refund_flipShipping;
                row[24] = Data.refund_flipCollection;
                row[25] = Data.refund_flipReverseShipping;
                row[26] = Data.refund_flipFixedFee;               
                row[27] = Data.Refund_SUMIGST;
                row[28] = Data.Refund_SUMCGST;
                row[29] = Data.Refund_SUMSGST;
                row[30] = Data.refund_SumOrder;

                row[31] = Data.NetTotal;
                row[32] = Data.PercentageAmount;
                row[33] = Data.SumFee;
                row[34] = Data.refund_SumFee;

                export_dt.Rows.Add(row);
                s++;
            }
            return export_dt;

        }

        public DataTable CreateNetRealizationDatatablePaytm(List<SaleReport> lst_seminar)
        {
            DataTable export_dt = new DataTable();
            export_dt.Columns.Add("Sr. No.", typeof(string));
            export_dt.Columns.Add("OrderID", typeof(string));
            export_dt.Columns.Add("Order Date", typeof(string));
            export_dt.Columns.Add("Dispatch Date", typeof(string));
            export_dt.Columns.Add("Product Name", typeof(string));
            export_dt.Columns.Add("SKU N0", typeof(string));
            export_dt.Columns.Add("Product Value", typeof(string));
            export_dt.Columns.Add("IGST", typeof(string));
            export_dt.Columns.Add("SGST", typeof(string));
            export_dt.Columns.Add("CGST", typeof(string));
            export_dt.Columns.Add("Total", typeof(string));

            export_dt.Columns.Add("Order Reference-ID", typeof(string));
            export_dt.Columns.Add("Refund Ref_No", typeof(string));
            export_dt.Columns.Add("Order Product Value", typeof(string));

            export_dt.Columns.Add("Marketplace Commission", typeof(string));
            export_dt.Columns.Add("Logistics Charges", typeof(string));
            export_dt.Columns.Add("PG Commission", typeof(string));
            export_dt.Columns.Add("Penalty", typeof(string));
            export_dt.Columns.Add("Net Adjustments", typeof(string));
           
            export_dt.Columns.Add("Total IGST", typeof(string));
            export_dt.Columns.Add("Total CGST", typeof(string));
            export_dt.Columns.Add("Total SGST", typeof(string));

            export_dt.Columns.Add("Net Realization", typeof(string));
            export_dt.Columns.Add("Net Realization %", typeof(string));
            //export_dt.Columns.Add("Order Expenses", typeof(string));
            //export_dt.Columns.Add("Refund Expenses", typeof(string));


            int s = 1;
            foreach (var Data in lst_seminar)
            {
                DataRow row = export_dt.NewRow();
                if (Data == null)
                    continue;
                row["Sr. No."] = s + ".";
                row[1] = Data.OrderID;
                row[2] = Data.OrderDate;
                row[3] = Data.DispatchDate;
                row[4] = Data.ProductName;
                row[5] = Data.skuNo;
                row[6] = Data.Principal;
                row[7] = Data.orderigst;
                row[8] = Data.ordersgst;
                row[9] = Data.ordercgst;
                row[10] = Data.ordertotal;

                row[11] = Data.ReferenceID;
                row[12] = Data.refundReferenceID;
                row[13] = Data.ActualOrderTotal;

                row[14] = Data.ActualFBAFee;
                row[15] = Data.ActualTechnologyFee;
                row[16] = Data.ActualCommission;
                row[17] = Data.ActualFixedClosingFee;
                row[18] = Data.ActualShippingChargebackFee;
              

                row[19] = Data.ActualIGST;
                row[20] = Data.ActualCGST;
                row[21] = Data.ActualSGST;
                row[22] = Data.ActualNetTotal;
                row[23] = Data.PercentageAmount;
                //row[24] = Data.SumFee;
                //row[25] = Data.refund_SumFee;

                export_dt.Rows.Add(row);
                s++;
            }
            return export_dt;

        }
        public DataTable CreateSummaryRealizationDatatable(List<SaleReport> lst_seminar)
        {
            DataTable export_dt = new DataTable();
            export_dt.Columns.Add("Sr. No.", typeof(string));
            export_dt.Columns.Add("OrderID", typeof(string));
            //export_dt.Columns.Add("Purchase Cost", typeof(string));
            export_dt.Columns.Add("SKU No", typeof(string));
            export_dt.Columns.Add("Item Name", typeof(string));
            export_dt.Columns.Add("Item Price", typeof(string));

            export_dt.Columns.Add("Sale Value", typeof(string));
            export_dt.Columns.Add("Order-Total-Product-Value", typeof(string));
            export_dt.Columns.Add("Order-Total-Expenses-Deducted", typeof(string));
            export_dt.Columns.Add("Order-Realization-on-Settlement", typeof(string));
            export_dt.Columns.Add("Return-Product-Value", typeof(string));
            export_dt.Columns.Add("TotalExpenses-Reimbursed", typeof(string));
            export_dt.Columns.Add("Realization on Return", typeof(string));
            export_dt.Columns.Add("Net Realization", typeof(string));
            export_dt.Columns.Add("Net Realization %", typeof(string));
            export_dt.Columns.Add("Profit/Loss Per Order", typeof(string)); 

            int s = 1;
            foreach (var Data in lst_seminar)
            {
                DataRow row = export_dt.NewRow();
                if (Data == null)
                    continue;
                row["Sr. No."] = s + ".";
                row[1] = Data.OrderID;
                //row[2] = "";
                row[2] = Data.skuNo;
                row[3] = Data.ProductName;
                row[4] = Data.ProductValue;

                row[5] = Data.ordertotal;
                row[6] = Data.orderTotal;
                row[7] = Data.SumFee;
                row[8] = Data.SumOrder;
                row[9] = Data.refundTotal;
                row[10] = Data.refund_SumFee;
                row[11] = Data.refund_SumOrder;
                row[12] = Data.NetTotal;
                row[13] = Data.PercentageAmount;
                row[14] = Data.Profit_lossAmount; 
                export_dt.Rows.Add(row);
                s++;
            }
            return export_dt;

        }

        #region ExpenseFor Amazon
        public DataTable CreateExpenseDatatable(List<SaleReport> lst_seminar)
        {
            DataTable export_dt = new DataTable();
            export_dt.Columns.Add("Sr. No.", typeof(string));
            export_dt.Columns.Add("Order ID", typeof(string));
            export_dt.Columns.Add("Order Date", typeof(string));
            export_dt.Columns.Add("Reference ID", typeof(string));
            export_dt.Columns.Add("Settlement Date", typeof(string));
            export_dt.Columns.Add("Commission Fee", typeof(string));
            export_dt.Columns.Add("Refund Commission Fee", typeof(string));
            export_dt.Columns.Add("Fixed Closing Fee", typeof(string));
            export_dt.Columns.Add("Refund Fixed Closing Fee", typeof(string));
            export_dt.Columns.Add("FBA FEE", typeof(string));
            export_dt.Columns.Add("Refund FBA FEE", typeof(string));
            export_dt.Columns.Add("Technology Fee", typeof(string));
            export_dt.Columns.Add("Refund Technology Fee", typeof(string));
            export_dt.Columns.Add("Refund Commision", typeof(string));
            export_dt.Columns.Add("Refund_Commision", typeof(string));
            export_dt.Columns.Add("Shipping Chargeback Fee", typeof(string));
            export_dt.Columns.Add("Refund Shipping Chargeback Fee", typeof(string));          

            int s = 1;
            foreach (var Data in lst_seminar)
            {
                DataRow row = export_dt.NewRow();
                if (Data == null)
                    continue;
                row["Sr. No."] = s + ".";
                row[1] = Data.OrderID;
                row[2] = Data.OrderDate;
                row[3] = Data.ReferenceID;
                row[4] = Data.SettlementDate;
                row[5] = Data.CommissionFee;
                row[6] = Data.RefundCommissionFee;
                row[7] = Data.FixedClosingFee;
                row[8] = Data.RefundFixedClosingFee;
                row[9] = Data.FBAFEE;
                row[10] = Data.RefundFBAFEE;
                row[11] = Data.TechnologyFee;
                row[12] = Data.RefundTechnologyFee;
                row[13] = Data.RefundCommision;
                row[14] = Data.Refund_Commision;
                row[15] = Data.ShippingChargebackFee;
                row[16] = Data.RefundShippingChargebackFee;               
                export_dt.Rows.Add(row);
                s++;
            }
            return export_dt;

        }
        #endregion

        #region Expense For Paytm
        public DataTable CreateExpenseDatatablePaytm(List<SaleReport> lst_seminar)
        {
            DataTable export_dt = new DataTable();
            export_dt.Columns.Add("Sr. No.", typeof(string));
            export_dt.Columns.Add("Order ID", typeof(string));
            export_dt.Columns.Add("Order Date", typeof(string));
            export_dt.Columns.Add("Reference ID", typeof(string));
            export_dt.Columns.Add("Settlement Date", typeof(string));
           
            export_dt.Columns.Add("Marketplace Commission Fee", typeof(string));
            export_dt.Columns.Add("Logistics Charges Fee", typeof(string));
            export_dt.Columns.Add("PG Commission Fee", typeof(string));
            export_dt.Columns.Add("Penalty Fee", typeof(string));
            export_dt.Columns.Add("Net Adjustments Fee", typeof(string));
            export_dt.Columns.Add("Refund Marketplace Commission Fee", typeof(string));
            export_dt.Columns.Add("Refund Logistics Charges Fee", typeof(string));
            export_dt.Columns.Add("Refund PG Commission Fee", typeof(string));
            export_dt.Columns.Add("Refund Penalty Fee", typeof(string));
            export_dt.Columns.Add("Refund Net Adjustments Fee", typeof(string));

            int s = 1;
            foreach (var Data in lst_seminar)
            {
                DataRow row = export_dt.NewRow();
                if (Data == null)
                    continue;
                row["Sr. No."] = s + ".";
                row[1] = Data.OrderID;
                row[2] = Data.OrderDate;
                row[3] = Data.ReferenceID;
                row[4] = Data.SettlementDate;           
                row[5] = Data.Marketplacecommission;
                row[6] = Data.LogisticsCharges;
                row[7] = Data.PGCommission;
                row[8] = Data.Penaty;
                row[9] = Data.NetAdjustments;
                row[10] = Data.RefundMarketplacecommission;
                row[11] = Data.RefundLogisticsCharges;
                row[12] = Data.RefundPGCommission;
                row[13] = Data.RefundPenaty;
                row[14] = Data.RefundNetAdjustments;
                export_dt.Rows.Add(row);
                s++;
            }
            return export_dt;

        }
        #endregion

        public DataTable CreateDebtorLedgerDatatable(List<SaleReport> lst_seminar)
        {
            DataTable export_dt = new DataTable();
            export_dt.Columns.Add("Sr. No.", typeof(string));
            export_dt.Columns.Add("MarketPlace Name", typeof(string));
            export_dt.Columns.Add("Order ID", typeof(string));
            export_dt.Columns.Add("Order Date", typeof(string));
            export_dt.Columns.Add("Status", typeof(string));
            export_dt.Columns.Add("Sum Fee", typeof(string));
            export_dt.Columns.Add("Cancelled Order Date", typeof(string));
            export_dt.Columns.Add("SumCancel Fee", typeof(string));
            export_dt.Columns.Add("Sett_order Date", typeof(string));
            export_dt.Columns.Add("Reference ID", typeof(string));
            export_dt.Columns.Add("Order Total", typeof(string));
            export_dt.Columns.Add("Refund Date", typeof(string));
            export_dt.Columns.Add("Refund Reference ID", typeof(string));
            export_dt.Columns.Add("Refund Total", typeof(string));
            export_dt.Columns.Add("Physically Return Date", typeof(string));
            export_dt.Columns.Add("Physically Return Amount", typeof(string));
            export_dt.Columns.Add("Net Total", typeof(string));
            int s = 1;
            foreach (var Data in lst_seminar)
            {
                DataRow row = export_dt.NewRow();
                if (Data == null)
                    continue;
                row["Sr. No."] = s + ".";
                row[1] = Data.MarketPlaceName;
                row[2] = Data.OrderID;
                row[3] = Data.OrderDate;
                row[4] = Data.Status;
                row[5] = Data.SumFee;
                row[6] = Data.CancelledOrderDate;
                row[7] = Data.SumCancelFee;
                row[8] = Data.Sett_orderDate;
                row[9] = Data.ReferenceID;
                row[10] = Data.orderTotal;
                row[11] = Data.refundDate;
                row[12] = Data.refundReferenceID;
                row[13] = Data.refundtotal;
                row[14] = Data.PhysicallyDate;
                row[15] = Data.PhysicallyAmount;
                row[16] = Data.NetTotal;
                export_dt.Rows.Add(row);
                s++;
            }
            return export_dt;

        }

        public DataTable CreateSaleLedgerDatatable(List<SaleReport> lst_seminar)
        {
            DataTable export_dt = new DataTable();
            export_dt.Columns.Add("Sr. No.", typeof(string));
            export_dt.Columns.Add("MarketPlace Name", typeof(string));
            export_dt.Columns.Add("Order ID", typeof(string));
            export_dt.Columns.Add("Order Date", typeof(string));
            export_dt.Columns.Add("Order Amount", typeof(string));
            export_dt.Columns.Add("Cancelled Order Date", typeof(string));
            export_dt.Columns.Add("Cancelled Order Amount", typeof(string));
            export_dt.Columns.Add("Settlement Date", typeof(string));
            export_dt.Columns.Add("Principal", typeof(string));
            export_dt.Columns.Add("NetTotal", typeof(string));
            int s = 1;
            foreach (var Data in lst_seminar)
            {
                DataRow row = export_dt.NewRow();
                if (Data == null)
                    continue;
                row["Sr. No."] = s + ".";
                row[1] = Data.MarketPlaceName;
                row[2] = Data.OrderID;
                row[3] = Data.OrderDate;
                row[4] = Data.OrderAmount;
                row[5] = Data.CancelledOrderDate;
                row[6] = Data.CancelledorderAmount;
                row[7] = Data.SettlementDate;
                row[8] = Data.Principal;
                row[9] = Data.NetTotal;
                export_dt.Rows.Add(row);
                s++;
            }
            return export_dt;

        }


        public DataTable CreateRevenueLedgerDatatable(List<SaleReport> lst_seminar)
        {
            DataTable export_dt = new DataTable();
            export_dt.Columns.Add("Sr. No.", typeof(string));
            export_dt.Columns.Add("MarketPlace Name", typeof(string));
            export_dt.Columns.Add("Order ID", typeof(string));
            export_dt.Columns.Add("Order Date", typeof(string));
            export_dt.Columns.Add("Sum Order", typeof(string));
            export_dt.Columns.Add("Cancelled Order Date", typeof(string));
            export_dt.Columns.Add("SumCancel Fee", typeof(string));
            export_dt.Columns.Add("Settlement Date", typeof(string));
            export_dt.Columns.Add("Refund total", typeof(string));
            export_dt.Columns.Add("NetTotal", typeof(string));
            int s = 1;
            foreach (var Data in lst_seminar)
            {
                DataRow row = export_dt.NewRow();
                if (Data == null)
                    continue;
                row["Sr. No."] = s + ".";
                row[1] = Data.MarketPlaceName;
                row[2] = Data.OrderID;
                row[3] = Data.OrderDate;
                row[4] = Data.SumOrder;
                row[5] = Data.CancelledOrderDate;
                row[6] = Data.SumCancelFee;
                row[7] = Data.SettlementDate;
                row[8] = Data.refundtotal;
                row[9] = Data.NetTotal;
                export_dt.Rows.Add(row);
                s++;
            }
            return export_dt;

        }

        public DataTable CreateSaleVoucherDatatable1(List<SaleReport> lst_seminar)
        {
            DataTable export_dt = new DataTable();
            //export_dt.Columns.Add("Sr. No.", typeof(string));
            export_dt.Columns.Add("Voucher Date", typeof(string));
            export_dt.Columns.Add("Voucher Number", typeof(string));
            export_dt.Columns.Add("Ledger Name", typeof(string));
            export_dt.Columns.Add("Reference Number", typeof(string));
            export_dt.Columns.Add("Dr Amount", typeof(string));
            export_dt.Columns.Add("Cr Amount", typeof(string));
            export_dt.Columns.Add("Narration", typeof(string));
            int s = 1;
            foreach (var Data in lst_seminar)
            {
                DataRow row = export_dt.NewRow();
                if (Data == null)
                    continue;
               // row["Sr. No."] = s + ".";
                row[0] = Data.Sett_orderDate;
                row[1] = Data.VoucherNumber;
                row[2] = Data.ExpenseName;
                row[3] = Data.OrderID;
                row[4] = Data.refund_SumOrder;
                row[5] = Data.SumOrder;
                row[6] = Data.Narration;
                export_dt.Rows.Add(row);
                s++;
            }
            return export_dt;

        }

        public DataTable CreateSaleVoucherDatatable(List<SaleReport> lst_seminar)
        {
            DataTable export_dt = new DataTable();
            //export_dt.Columns.Add("Sr. No.", typeof(string));
            export_dt.Columns.Add("Date", typeof(string));
            export_dt.Columns.Add("Sale Order Number", typeof(string));
            export_dt.Columns.Add("Invoice number", typeof(string));           
            export_dt.Columns.Add("Channel entry", typeof(string));
            export_dt.Columns.Add("Channel Ledger", typeof(string));
            export_dt.Columns.Add("Product Name", typeof(string));
            export_dt.Columns.Add("Product SKU Code", typeof(string));
            export_dt.Columns.Add("Qty", typeof(string));
            export_dt.Columns.Add("Unit Price", typeof(string));

            export_dt.Columns.Add("Currency", typeof(string));
            export_dt.Columns.Add("conversion rate", typeof(string));
            export_dt.Columns.Add("Total", typeof(string));
            export_dt.Columns.Add("Customer Name", typeof(string));
            export_dt.Columns.Add("Shipping Address Name", typeof(string));
            export_dt.Columns.Add("Shipping Address Line 1", typeof(string));
            export_dt.Columns.Add("Shipping Address Line 2", typeof(string));
            export_dt.Columns.Add("Shipping Address City", typeof(string));
            export_dt.Columns.Add("Shipping Address State", typeof(string));
            export_dt.Columns.Add("Shipping Address Country", typeof(string));
            export_dt.Columns.Add("Shipping Address Pincode", typeof(string));
            export_dt.Columns.Add("Shipping Address Phone", typeof(string));
            export_dt.Columns.Add("Shipping Provider", typeof(string));
            export_dt.Columns.Add("AWB num", typeof(string));
            export_dt.Columns.Add("Sales", typeof(string));
            export_dt.Columns.Add("Sales Ledger", typeof(string));
            export_dt.Columns.Add("CGST", typeof(string));
            export_dt.Columns.Add("CGST Rate", typeof(string));
            export_dt.Columns.Add("SGST", typeof(string));
            export_dt.Columns.Add("SGST Rate", typeof(string));
            export_dt.Columns.Add("IGST", typeof(string));
            export_dt.Columns.Add("IGST Rate", typeof(string));
            export_dt.Columns.Add("UTGST", typeof(string));
            export_dt.Columns.Add("UTGST Rate", typeof(string));
            export_dt.Columns.Add("CESS", typeof(string));
            export_dt.Columns.Add("CESS Rate", typeof(string));
            export_dt.Columns.Add("Other charges", typeof(string));
            export_dt.Columns.Add("Other charges Ledger", typeof(string));
            export_dt.Columns.Add("Other charges1", typeof(string));
            export_dt.Columns.Add("Other charges Ledger1", typeof(string));
            export_dt.Columns.Add("Service tax", typeof(string));
            export_dt.Columns.Add("ST Ledger", typeof(string));
            export_dt.Columns.Add("IMEI", typeof(string));
            export_dt.Columns.Add("Godown", typeof(string));
            export_dt.Columns.Add("Dispatch Date/Cancellation Date", typeof(string));
            export_dt.Columns.Add("Narration", typeof(string));
            export_dt.Columns.Add("Entity", typeof(string));
            export_dt.Columns.Add("Voucher Type Name", typeof(string));
            export_dt.Columns.Add("TIN NO", typeof(string));
            export_dt.Columns.Add("Original Invoice Date", typeof(string));
            export_dt.Columns.Add("Original Sale No", typeof(string));
            export_dt.Columns.Add("Channel Invoice Created", typeof(string));
            export_dt.Columns.Add("Tax Verification", typeof(string));           
            int s = 1;
            foreach (var Data in lst_seminar)
            {
                DataRow row = export_dt.NewRow();
                if (Data == null)
                    continue;
                // row["Sr. No."] = s + ".";
                row[0] = Data.OrderDate;
                row[1] = Data.OrderID;
                row[2] = Data.InvoiceNo;
                row[3] = Data.Channelentry;
                row[4] = Data.Channelledger;
                row[5] = Data.ProductName;
                row[6] = Data.skuNo;
                row[7] = Data.Quantity;
                row[8] = Data.ProductValue;

                row[9] = Data.Currency;
                row[10] = Data.Quantity;
                row[11] = Data.OrderAddTotal;
                row[12] = Data.CustomerName;
                row[13] = Data.shipaddressname;
                row[14] = Data.shipaddressname1;
                row[15] = Data.shipaddressname2;
                row[16] = Data.shipcity;
                row[17] = Data.shipstate;
                row[18] = Data.shipcountry;
                row[19] = Data.shippincode;
                row[20] = Data.shipphoneno;
                row[21] = Data.shipprovider;
                row[22] = Data.AWBNo;
                row[23] = Data.itemamountwithout_tax;
                row[24] = Data.SalesLedger;
                row[25] = Data.SUMCGST;
                row[26] = Data.CGST_rate;
                row[27] = Data.SUMSGST;
                row[28] = Data.SGST_rate;
                row[29] = Data.SUMIGST;
                row[30] = Data.IGST_rate;
                row[31] = Data.UTGST;
                row[32] = Data.UTGSTRate;
                row[33] = Data.CESS;
                row[34] = Data.CESSRate;
                row[35] = Data.Shipping;
                row[36] = Data.Shipping_rate;
                row[37] = Data.GiftwrapAmount;
                row[38] = Data.Giftwrap_rate;
                row[39] = Data.Servicetax;
                row[40] = Data.StLedger;
                row[41] = Data.IMEI;
                row[42] = Data.Godown;
                row[43] = Data.Dispatch_Cancellationdate;
                row[44] = Data.Narration;
                row[45] = Data.Entity;
                row[46] = Data.Entity;
                row[47] = Data.TinNo;
                row[48] = Data.OrderDate;
                row[49] = Data.OrderID;
                row[50] = Data.Channelinvoicecreated;
                row[51] = Data.TaxVerification;                             
                export_dt.Rows.Add(row);
                s++;
            }
            return export_dt;

        }

        public DataTable CreateGSTDatatable(List<SaleReport> lst_seminar)
        {
            DataTable export_dt = new DataTable();
            export_dt.Columns.Add("Sr. No.", typeof(string));
            export_dt.Columns.Add("Type", typeof(string));
            export_dt.Columns.Add("Place Of Supply", typeof(string));
            export_dt.Columns.Add("Applicable % of Tax Rate", typeof(string));
            export_dt.Columns.Add("Rate", typeof(string));
            export_dt.Columns.Add("Taxable Value", typeof(string));
            export_dt.Columns.Add("Cess Amount", typeof(string));
            export_dt.Columns.Add("E-Commerce GSTIN", typeof(string));
           
            int s = 1;
            foreach (var Data in lst_seminar)
            {
                DataRow row = export_dt.NewRow();
                if (Data == null)
                    continue;
                row["Sr. No."] = s + ".";
                row[1] = Data.GSTType;
                row[2] = Data.PlaceSupply;
                row[3] = Data.ApplicableTaxrate;
                row[4] = Data.Rate;
                row[5] = Data.ToatlAmountGST;
                row[6] = "";
                row[7] = Data.ECommerceGSTIN;                
                export_dt.Rows.Add(row);
                s++;
            }
            return export_dt;

        }

        public DataTable CreateSettlementDetailedDatatable(List<SaleReport> lst_seminar)
        {
            DataTable export_dt = new DataTable();
            export_dt.Columns.Add("Sr. No.", typeof(string));
            export_dt.Columns.Add("Settlement Reference No", typeof(string));
            export_dt.Columns.Add("Deposit Date", typeof(string));
            export_dt.Columns.Add("Bank Amount", typeof(string));           
            export_dt.Columns.Add("Prevoius Reserve Amount", typeof(string));
            export_dt.Columns.Add("Current Reserve Amount", typeof(string));
            export_dt.Columns.Add("Order Amount", typeof(string));
            export_dt.Columns.Add("Refund Amount", typeof(string));
            export_dt.Columns.Add("Commission", typeof(string));
            export_dt.Columns.Add("Fixed closing fee", typeof(string));
            export_dt.Columns.Add("Refund commission", typeof(string));
            export_dt.Columns.Add("Shipping commission", typeof(string));
            export_dt.Columns.Add("Easy Ship weight handling fees", typeof(string));
            

            int s = 1;
            foreach (var Data in lst_seminar)
            {
                DataRow row = export_dt.NewRow();
                if (Data == null)
                    continue;
                row["Sr. No."] = s + ".";
                row[1] = Data.ReferenceID;
                row[2] = Data.SettlementDate;
                row[3] = Data.BankAmount;
                row[4] = Data.PreviousReserveAmount;
                row[5] = Data.CurrentReserveAmount;
                row[6] = Data.OrderAmount;
                row[7] = Data.refundTotal;
                row[8] = Data.ActualCommission;
                row[9] = Data.ActualFixedClosingFee;
                row[10] = Data.ActualRefundCommission;
                row[11] = Data.ActualShippingCommision;
                row[12] = Data.ActualAmazonEasyShipCharges;
               
                export_dt.Rows.Add(row);
                s++;
            }
            return export_dt;

        }

        public DataTable CreateReturnDatatable(List<partial_tbl_order_history> lst_seminar)
        {
            DataTable export_dt = new DataTable();
            export_dt.Columns.Add("Sr. No.", typeof(string));
            export_dt.Columns.Add("MarketPlace Name", typeof(string));
            export_dt.Columns.Add("Order ID", typeof(string));
            export_dt.Columns.Add("Order Date", typeof(string));
            export_dt.Columns.Add("SKU No", typeof(string));
            export_dt.Columns.Add("Product Value", typeof(string));
            export_dt.Columns.Add("Physically Type", typeof(string));
            export_dt.Columns.Add("Condition Type", typeof(string));
            export_dt.Columns.Add("Physically Receive Date", typeof(string));         
            export_dt.Columns.Add("Fullfillment Type", typeof(string));           
            int s = 1;
            foreach (var Data in lst_seminar)
            {
                DataRow row = export_dt.NewRow();
                if (Data == null)
                    continue;
                row["Sr. No."] = s + ".";
                row[1] = Data.MarketPlaceName;
                row[2] = Data.order_id;
                row[3] = Data.OrderDate;
                row[4] = Data.sku;
                row[5] = Data.TotalValue;
                row[6] = Data.ReturnType;
                row[7] = Data.ConditionType;
                row[8] = Data.PhysicallyDate;
                row[9] = Data.fullfillment_id;               
                export_dt.Rows.Add(row);
                s++;
            }
            return export_dt;

        }

        public DataTable MonthReportDatatable(List<MonthReport> lst_seminar, SowMonthColumn showcol)
        {
            DataTable export_dt = new DataTable();
            export_dt.Columns.Add("Particulars", typeof(string));

            if (showcol.ShowJan)
                export_dt.Columns.Add("January", typeof(string));
            if (showcol.ShowFeb)
                export_dt.Columns.Add("February", typeof(string));
            if (showcol.ShowMarch)
                export_dt.Columns.Add("March", typeof(string));
            if (showcol.ShowApril)
                export_dt.Columns.Add("April", typeof(string));
            if (showcol.ShowMay)
                export_dt.Columns.Add("May", typeof(string));
            if (showcol.ShowJune)
                export_dt.Columns.Add("June", typeof(string));
            if (showcol.ShowJuly)
                export_dt.Columns.Add("July", typeof(string));
            if (showcol.ShowAug)
                export_dt.Columns.Add("August", typeof(string));
            if (showcol.ShowSept)
                export_dt.Columns.Add("September", typeof(string));
            if (showcol.ShowOct)
                export_dt.Columns.Add("October", typeof(string));
            if (showcol.ShowNov)
                export_dt.Columns.Add("November", typeof(string));
            if (showcol.ShowDec)
                export_dt.Columns.Add("December", typeof(string));

            export_dt.Columns.Add("Total", typeof(string));
            

            int s = 1;
            foreach (var Data in lst_seminar)
            {
                DataRow row = export_dt.NewRow();
                if (Data == null)
                    continue;
                //row["Sr. No."] = s + ".";
                row["Particulars"] = Data.particulars;
                if (showcol.ShowJan)
                {
                    if (Data.Jan != null && Data.Jan != 0)
                        row["January"] = Data.Jan;
                }
                if (showcol.ShowFeb)
                {
                    if (Data.Feb != null && Data.Feb != 0)
                        row["February"] = Data.Feb;
                }
                if (showcol.ShowMarch)
                {
                    if (Data.March != null && Data.March != 0)
                        row["March"] = Data.March;
                }
                if (showcol.ShowApril)
                {
                    if (Data.April != null && Data.April != 0)
                        row["April"] = Data.April;
                }
                if (showcol.ShowMay)
                {
                    if (Data.May != null && Data.May != 0)
                        row["May"] = Data.May;
                }
                if (showcol.ShowJune)
                {
                    if (Data.June != null && Data.June != 0)
                        row["June"] = Data.June;
                }
                if (showcol.ShowJuly)
                {
                    if (Data.July != null && Data.July != 0)
                        row["July"] = Data.July;
                }
                if (showcol.ShowAug)
                {
                    if (Data.Aug != null && Data.Aug != 0)
                        row["August"] = Data.Aug;
                }
                if (showcol.ShowSept)
                {
                    if (Data.Sept != null && Data.Sept != 0)
                        row["September"] = Data.Sept;
                }
                if (showcol.ShowOct)
                {
                    if (Data.Oct != null && Data.Oct != 0)
                        row["October"] = Data.Oct;
                }
                if (showcol.ShowNov)
                {
                    if (Data.Nov != null && Data.Nov != 0)
                        row["November"] = Data.Nov;
                }
                if (showcol.ShowDec)
                {
                    if (Data.Dec != null && Data.Dec != 0)
                        row["December"] = Data.Dec;
                }
                if (Data.Total_mounthcount != null && Data.Total_mounthcount != 0)
                    row["Total"] = Data.Total_mounthcount;                                        
                export_dt.Rows.Add(row);
                s++;
            }
            return export_dt;

        }

        public int Export(string header, DataTable dt_header, DataTable dt, HttpResponseBase Response, int type, bool SendMail, string EmailID)
        {
            int message = 0;
            string HeaderRow_ForColor = "Black";
            string HeaderRow_BackColor = "White";
            string RowStyle_BackColor = "White";
            string BorderColor = "Black";           
            int head_col = dt.Columns.Count;
            var grid = new GridView();
            grid.DataSource = dt;
            grid.DataBind();
            grid.ShowHeader = false;
            grid.CssClass = "textmode";
            grid.HeaderRow.CssClass = "HeaderRow_ForColor";
            //grid.HeaderRow.BackColor = Color.DarkCyan;
            grid.AlternatingRowStyle.CssClass = "AlternatingRowStyle";        
            if (type == 1)
            {
                #region Excel
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);

                string style = @"<style> .textmode{border : 1px Solid " + BorderColor + ";  } .textmode td,th {height:25px;}.filtermode{border : 1px Solid #fff;} .filtermode td,th{border:0px;font-weight: bold;} .logo{Color:" + HeaderRow_ForColor + ";text-align:center;background:" + HeaderRow_BackColor + ";font-size:16pt;font-weight: bold;} .HeaderRow_ForColor{color:" + HeaderRow_ForColor + ";background:" + HeaderRow_BackColor + "} .AlternatingRowStyle{background:" + RowStyle_BackColor + "}</style>";

                sw.Write(style);
                //sw.Write("<table><tr><td></td><td  class='logo'>" + header + "</td></tr><tr><td></td><td style='border : 1px Solid " + BorderColor + ";'>");
                //grid2.RenderControl(htw);
                // sw.Write("</td></tr><tr><td></td><td style='border : 1px Solid " + BorderColor + ";'>");
                grid.RenderControl(htw);
                //sw.Write("</td></tr></table>");
                if (SendMail)
                {
                    System.IO.MemoryStream s = new MemoryStream();
                    System.Text.Encoding Enc = System.Text.Encoding.Default;
                    byte[] mBArray = Enc.GetBytes(sw.ToString());
                    string[] send_to = new string[] { EmailID };
                    string[] bcc_to = new string[] { "" };
                    string subject = "EMS - " + header;
                    SendMail maill = new SendMail();
                    bool i = maill.send_mail("Hello, <br/>Please find the report attached.<br/><br/>Thanks", send_to, bcc_to, subject, mBArray, header + ".xls");
                    if (i) message = 3;
                    else message = 2;

                }
                else
                {
                    Response.Clear();
                    Response.Buffer = true;
                    Response.AddHeader("content-disposition", "attachment;filename=" + header + ".xls");
                    Response.Charset = "";
                    //Response.ContentType = "application/vnd.ms-excel";
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Write(sw.ToString());
                    Response.End();
                    message = 1;
                }
                #endregion
            }
            else if (type == 2)
            {
                #region PDF
                Response.Clear();
                var doc = new Document();
                if (!SendMail)
                    PdfWriter.GetInstance(doc, Response.OutputStream);

                System.IO.MemoryStream s = new MemoryStream();
                PdfWriter writer = PdfWriter.GetInstance(doc, s);
                doc.SetPageSize(PageSize.A4.Rotate());
                doc.Open();

                var BHeaderRow_ForColor = System.Drawing.ColorTranslator.FromHtml(HeaderRow_ForColor);
                var BHeaderRow_BackColor = System.Drawing.ColorTranslator.FromHtml(HeaderRow_BackColor);
                var BRowStyle_BackColor = System.Drawing.ColorTranslator.FromHtml(RowStyle_BackColor);
                var BBorderColor = System.Drawing.ColorTranslator.FromHtml(BorderColor);

                var arial = FontFactory.GetFont("Arial", 8, BaseColor.BLACK);
                var arialBold = FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.BOLD, new BaseColor(BHeaderRow_ForColor));
                var FilterBold = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
                BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
                Font times = new Font(bfTimes, 15, Font.ITALIC, new BaseColor(BHeaderRow_ForColor));
                PdfPTable tbl_header = new PdfPTable(2);
                tbl_header.SetWidths(new int[] { 0, 50 });
                tbl_header.TotalWidth = 700f;
                tbl_header.LockedWidth = true;
                PdfPCell cell = new PdfPCell(new Paragraph(header, times));
                cell.Colspan = 4;
                cell.PaddingTop = 10;
                cell.PaddingBottom = 10;
                cell.BorderColor = new BaseColor(BBorderColor);
                cell.BackgroundColor = new BaseColor(BHeaderRow_BackColor);
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                tbl_header.AddCell(cell);
                doc.Add(tbl_header);
                int headerColumn = dt_header.Columns.Count;
                if (headerColumn > 6) headerColumn = 6;
                var tblheader = new PdfPTable(headerColumn)
                {
                    TotalWidth = 700f,
                    LockedWidth = true,
                };

                for (int k = 0; k < dt_header.Rows.Count; k++)
                {
                    for (int j = 0; j < headerColumn; j++)
                    {
                        PdfPCell NoBoaderCell = new PdfPCell(new Phrase(dt_header.Rows[k][j].ToString(), FilterBold));

                        NoBoaderCell.BorderColor = new BaseColor(BBorderColor);
                        if (j == 0)
                        {
                            NoBoaderCell.BorderWidthLeft = 1;
                            NoBoaderCell.BorderWidthTop = 0;
                            NoBoaderCell.BorderWidthBottom = 0;
                            NoBoaderCell.BorderWidthRight = 0;
                        }
                        else if (j == headerColumn - 1)
                        {
                            NoBoaderCell.BorderWidthLeft = 0;
                            NoBoaderCell.BorderWidthTop = 0;
                            NoBoaderCell.BorderWidthBottom = 0;
                            NoBoaderCell.BorderWidthRight = 1;
                        }
                        else
                            NoBoaderCell.BorderWidth = 0;
                        tblheader.AddCell(NoBoaderCell);
                    }
                }
                doc.Add(tblheader);
                var table = new PdfPTable(dt.Columns.Count)
                {
                    TotalWidth = 700f,
                    LockedWidth = true,
                };

                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    PdfPCell hcell = new PdfPCell(new Paragraph(dt.Columns[i].ColumnName, arialBold));
                    hcell.BackgroundColor = new BaseColor(BHeaderRow_BackColor);
                    hcell.HorizontalAlignment = Element.ALIGN_CENTER;
                    hcell.BorderColor = new BaseColor(BBorderColor);
                    table.AddCell(hcell);
                }
                for (int k = 0; k < dt.Rows.Count; k++)
                {
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        PdfPCell dcell = new PdfPCell(new Phrase(dt.Rows[k][j].ToString(), arial));
                        dcell.HorizontalAlignment = Element.ALIGN_CENTER;
                        dcell.BorderColor = new BaseColor(BBorderColor);
                        table.AddCell(dcell);
                    }
                }
                doc.Add(table);
                if (SendMail)
                {
                    doc.Close();
                    byte[] mBArray = s.ToArray();
                    s.Close();
                    string[] send_to = new string[] { EmailID };
                    string[] bcc_to = new string[] { "" };
                    string subject = "ATS - " + header;
                    SendMail maill = new SendMail();
                    bool i = maill.send_mail("Hello, <br/>Please find the report attached.<br/><br/>Thanks", send_to, bcc_to, subject, mBArray, header + ".pdf");
                    if (i) message = 3;
                }
                else
                {
                    doc.Close();
                    Response.Buffer = true;
                    Response.AddHeader("Content-disposition", "attachment; filename=" + header + ".pdf");
                    Response.ContentType = "application/octet-stream";
                    Response.Write(doc);
                    Response.End();
                    message = 1;
                }
                #endregion
            }            
            return message;
        }
        
    }
}
