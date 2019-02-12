using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using SellerVendor.Areas.Seller.Models;
using SellerVendor.Areas.Seller.Models.DBContext;
using SellerVendor.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace SellerVendor.Areas.Seller.Controllers
{
    public class CronJobController : Controller
    {
        public SellerContext dba = new SellerContext();
        public SellerAdminContext db = new SellerAdminContext();
        Browse_Upload_Excel_Utility obj_tax = new Browse_Upload_Excel_Utility();
        comman_function cf = null;

        static bool bCron_running = false;

        public ActionResult Index()
        {
            try
            {
                if (bCron_running)
                {
                    exception_history objexception = new exception_history();
                    objexception.exception_date = DateTime.Now;
                    objexception.source_file = "Cron Job";
                    objexception.exception = "Returning. Already Running";
                    objexception.page = "1";
                    objexception.tbl_seller_id = "";
                    dba.exception_history.Add(objexception);
                    dba.SaveChanges();
                }
                else
                {
                    try
                    {
                        bCron_running = true;
                        dba = new SellerContext();
                        dba.Configuration.AutoDetectChangesEnabled = false;
                        SalesOrderCrown();
                        SalesOrderAPICrown();

                        dba = new SellerContext();
                        dba.Configuration.AutoDetectChangesEnabled = false;
                        TaxCrown();

                        dba = new SellerContext();
                        dba.Configuration.AutoDetectChangesEnabled = false;
                        SettlementCrown();
                        dba = new SellerContext();
                        dba.Configuration.AutoDetectChangesEnabled = false;
                        SettlementAPICrown();

                        dba = new SellerContext();
                        dba.Configuration.AutoDetectChangesEnabled = false;
                        ReturnDataCall();
                        ProductListAPI();
                        //CallPaytmData();

                    }
                    catch (Exception e)
                    {

                    }
                    finally
                    {
                        bCron_running = false;
                        exception_history objexception = new exception_history();
                        objexception.exception_date = DateTime.Now;
                        objexception.source_file = "Cron Job";
                        objexception.exception = "success";
                        objexception.page = "1";
                        objexception.tbl_seller_id = "";
                        dba.exception_history.Add(objexception);
                        dba.SaveChanges();

                    }
                }
            }
            catch (Exception ex)
            {
                if (bCron_running == true)
                    bCron_running = false;
            }

            return View();
        }

        public void CallPaytmData()
        {
            Browse_Upload_Excel_Utility obj = new Browse_Upload_Excel_Utility();

            obj.readpaytmsettlement("Abc", 5, 480, 19);//string strFilePath, int marketplaceID, int id, int SellerId
        }
        public JsonResult GetImageDetails()
        {

            return new JsonResult { Data = "", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }



        #region BulkSettlementData
        /// <summary>
        /// used for save settlement data in a table
        /// </summary>
        /// <returns></returns>
        public void SettlementCrown()
        {
            int seller_id = 0;
            try
            {
                var get_settlement_details = dba.tbl_settlement_upload.Where(a => a.file_status == "Queued" && a.Source == 1).OrderBy(a => a.Id).ToList();
                if (get_settlement_details != null)
                {
                    foreach (var item in get_settlement_details)
                    {
                        //sharad231
                        dba = new SellerContext();

                        seller_id = Convert.ToInt16(item.tbl_seller_id);
                        string filename = item.file_name;
                        int settlementuploadtype = Convert.ToInt16(item.settlement_type);
                        short sourcetype = Convert.ToInt16(item.Source);
                        int marketplaceid = Convert.ToInt16(item.market_place_id);
                        int id = item.Id;
                        string path = Server.MapPath("~/UploadExcel/" + seller_id + "/settlement/" + filename);
                        //string path = "http://demo.raintree.online/UploadExcel/" + seller_id + "/settlement/" + filename;// System.IO.Path.Combine(Server.MapPath("~/UploadExcel/" + seller_id + "/settlement/" + filename));

                        var getdetailss = dba.tbl_settlement_upload.Where(a => a.Id == id).FirstOrDefault();
                        if (getdetailss != null)
                        {
                            getdetailss.processing_datetime = DateTime.Now;
                            //sharad
                            getdetailss.file_status = "Processing";
                            dba.Entry(getdetailss).State = EntityState.Modified;

                            dba.SaveChanges();
                        }
                        Browse_Upload_Excel_Utility obj = new Browse_Upload_Excel_Utility();
                        List<AmazonreconciliationOrder> objjson1 = null;
                        string success = "";
                        if (marketplaceid == 3)
                        {
                            //Amazon
                            try
                            {
                                objjson1 = obj.ReadSettlementFile_Amazon_flatfile(path, seller_id);
                            }
                            catch (Exception e)
                            {
                            }
                            try
                            {
                                if (objjson1 == null)
                                {
                                    objjson1 = obj.ReadSettlementFile_Amazon_v2(path, seller_id);
                                }
                            }
                            catch (Exception e)
                            {
                                Writelog log = new Writelog();
                                log.write_exception_log(id.ToString(), "CronJobController", "while ReadSettlementFile_Amazon_v2", DateTime.Now, e);
                            }
                        }
                        else if (marketplaceid == 1)
                        {
                            //Flipkart
                            try
                            {
                                obj.ReadandBreakSettlement_flipkart(path, id, marketplaceid, seller_id);
                                //objjson1 = obj.ReadSettlementFile_Flipkart(path, seller_id);
                            }
                            catch (Exception e)
                            {

                            }
                        }
                        //else if (marketplaceid == 5)
                        //{
                        //    //Paytm
                        //    try
                        //    {
                        //        obj.readpaytmsettlement(path, marketplaceid, id, seller_id);
                        //    }
                        //    catch (Exception e)
                        //    {

                        //    }
                        //}
                        try
                        {
                            if (objjson1 != null)
                            {
                                success = SaveBulksettlementdata(objjson1, id, marketplaceid, seller_id, null);// for save 
                            }
                        }
                        catch (Exception e)
                        {
                            Writelog log = new Writelog();
                            log.write_exception_log(id.ToString(), "CronJobController", "SettlementCrown", DateTime.Now, e);
                        }
                    }//foreach
                }
            }
            catch (Exception ex)
            {
                Writelog log = new Writelog();
                log.write_exception_log(seller_id.ToString(), "CronJobController", "SettlementCrown", DateTime.Now, ex);
            }


            // return View("Index");
        }

        public void handleOrder(string file_settlement_id, reconciliationorder obj, int SellerId, int upload_tbl_id)
        {
            //int SellerId = Convert.ToInt32(Session["SellerID"]);
            try
            {
                string sku = "";
                if (obj.order_id == "171-7616497-6675534" || obj.order_id == "406-1128348-5744302" || obj.order_id == "405-8281746-7323528")
                {

                }
                if (obj.order_amount_typesDict != null)
                {
                    int orderSKUcounter = 0;
                    foreach (KeyValuePair<string, List<settlement_amt_type>> pair in obj.order_amount_typesDict)
                    {
                        //orderSKUcounter++;
                        sku = pair.Key;
                        List<settlement_amt_type> order_amount_typesDict = pair.Value;

                        List<List<settlement_amt_type>> superlist = new List<List<settlement_amt_type>>();

                        //////////////////////
                        Dictionary<string, string> dictionary_deduplicate = new Dictionary<string, string>();
                        for (int k = 0; k < order_amount_typesDict.Count; k++)
                        {
                            settlement_amt_type item = order_amount_typesDict[k];
                            if (item.description != "" && item.description != null)//changes by vineet
                            {
                                if (dictionary_deduplicate.ContainsKey(item.description))
                                {
                                    dictionary_deduplicate = new Dictionary<string, string>();
                                    //dictionary_deduplicate.Add(item.description, "");
                                    List<settlement_amt_type> a = new List<settlement_amt_type>();
                                    superlist.Add(a);
                                    //a.Add(item);
                                    do
                                    {
                                        dictionary_deduplicate.Add(item.description, "");
                                        a.Add(item);
                                        k++;

                                        if (k == order_amount_typesDict.Count)
                                            break;
                                        else
                                            item = order_amount_typesDict[k];
                                    } while (!dictionary_deduplicate.ContainsKey(item.description));
                                    if (k < order_amount_typesDict.Count) k--;
                                }
                                else
                                {
                                    List<settlement_amt_type> a = new List<settlement_amt_type>();
                                    superlist.Add(a);
                                    //a.Add(item);
                                    do
                                    {
                                        dictionary_deduplicate.Add(item.description, "");
                                        a.Add(item);
                                        k++;

                                        if (k == order_amount_typesDict.Count)
                                            break;
                                        else
                                            item = order_amount_typesDict[k];
                                    } while (!dictionary_deduplicate.ContainsKey(item.description));

                                    if (k < order_amount_typesDict.Count) k--;
                                }
                            }
                        }

                        ////////////////////////////////////////////////////////////////////////////////////////////
                        var Tbl_SettlementFees = settlementDB_Context.m_settlement_fee.ToList();
                        var get_settlement_transactiontype = settlementDB_Context.m_settlement_transaction_type.ToList();

                        //loop thru all the items for a SKU
                        foreach (List<settlement_amt_type> myskulist in superlist)
                        {
                            tbl_settlement_order obj_settlement = null;
                            string unique_order_id = obj.order_id + "-" + orderSKUcounter;
                            orderSKUcounter++;
                            Dictionary<String, expense_tax_class> expenseId_Dict = new Dictionary<String, expense_tax_class>();

                            foreach (settlement_amt_type item in myskulist)
                            {
                                if (item.description == null || item.description == "")
                                    continue;

                                var Getsettlementorder = settlementDB_Context.tbl_settlement_order.Where(a => a.settlement_id == file_settlement_id && a.tbl_seller_id == SellerId && a.Sku_no.ToLower() == sku.ToLower() && a.unique_order_id == unique_order_id).FirstOrDefault();

                                if (Getsettlementorder == null)
                                {
                                    if (obj_settlement == null)
                                    {
                                        obj_settlement = new tbl_settlement_order();
                                        obj_settlement.Order_Id = obj.order_id;
                                        obj_settlement.unique_order_id = unique_order_id;
                                        obj_settlement.tbl_seller_id = SellerId;
                                        obj_settlement.created_on = DateTime.Now;
                                        obj_settlement.Sku_no = sku;
                                        obj_settlement.settlement_id = file_settlement_id;
                                        if (item.posteddatetime != null && item.posteddatetime != "")
                                        {
                                            string datetime = item.posteddatetime.Replace("UTC", "");
                                            string datetime2 = datetime.Replace(".", "-").TrimEnd();
                                            DateTime ddd = cf.MyDateTimeConverter(datetime2);
                                            //DateTime ddd = DateTime.ParseExact(datetime2, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                                            obj_settlement.posted_date = ddd;
                                        }

                                        obj_settlement.LastUpdatedDateUTC = DateTime.UtcNow;
                                        if (item.qty != null)
                                            obj_settlement.quantity = Convert.ToInt16(item.qty);
                                        settlementDB_Context.tbl_settlement_order.Add(obj_settlement);
                                        settlementDB_Context.SaveChanges();
                                    }
                                }
                                // #region Order check

                                if (item.description != null)
                                {
                                    if (item.description == "Principal" || item.description == "TotalSaleAmount")
                                    {
                                        obj_settlement.principal_price = item.amount;
                                        handleDebitCredit((Decimal)item.amount);
                                    }
                                    else if (item.description == "Shipping")
                                    {
                                        obj_settlement.shipping_price = item.amount;
                                        handleDebitCredit((Decimal)item.amount);
                                    }
                                    else if (item.description == "Gift wrap")
                                    {
                                        obj_settlement.giftwrap_price = item.amount;
                                        handleDebitCredit((Decimal)item.amount);
                                    }
                                    else if (item.description == "Gift Wrap Tax")
                                    {
                                        obj_settlement.giftwarp_tax = item.amount;
                                        handleDebitCredit((Decimal)item.amount);
                                    }
                                    else if (item.description == "Product Tax")
                                    {
                                        obj_settlement.product_tax = item.amount;
                                        handleDebitCredit((Decimal)item.amount);
                                    }
                                    else if (item.description == "Shipping tax")
                                    {
                                        obj_settlement.shipping_tax = item.amount;
                                        handleDebitCredit((Decimal)item.amount);
                                    }
                                    else if (item.description == "Shipping discount")
                                    {
                                        obj_settlement.shipping_discount = item.amount;
                                        handleDebitCredit((Decimal)item.amount);
                                    }
                                    else if (item.description == "Shipping tax discount")
                                    {
                                        obj_settlement.shipping_tax_discount = item.amount;
                                        handleDebitCredit((Decimal)item.amount);
                                    }
                                    else if (item.description == "SAFE-T Reimbursement")
                                    {
                                        obj_settlement.SAFE_T_Reimbursement = item.amount;
                                        handleDebitCredit((Decimal)item.amount);
                                    }
                                    else if (item.description == "INCORRECT_FEES_ITEMS")
                                    {
                                        obj_settlement.INCORRECT_FEES_ITEMS = item.amount;
                                        handleDebitCredit((Decimal)item.amount);
                                    }
                                    else if (item.description == "ProtectionFund")
                                    {
                                        obj_settlement.Protection_fund_flipkart = item.amount;
                                        handleDebitCredit((Decimal)item.amount);
                                    }
                                    else
                                    {

                                        var get_expensedata = Tbl_SettlementFees.Where(a => a.return_fee.ToLower() == item.description.ToLower()).FirstOrDefault();
                                        if (get_expensedata != null)
                                        {
                                            //sharad101
                                            if (item.amount < 0)
                                            {
                                                handleDebitCredit((Decimal)Convert.ToDouble(item.amount));
                                            }
                                            else
                                            {
                                                //sharad 122
                                                handleDebitCredit((Decimal)Convert.ToDouble(item.amount));
                                            }
                                            //if (Convert.ToDouble(item.amount) < 0)
                                            //    handleDebit((Decimal)Convert.ToDouble(item.amount) * (-1));
                                            //else
                                            //{
                                            //    handleCredit((Decimal)Convert.ToDouble(item.amount));
                                            //}

                                            var id = get_expensedata.id;
                                            m_tbl_expense objexpense = new m_tbl_expense();
                                            objexpense.reference_number = file_settlement_id;
                                            objexpense.tbl_seller_id = SellerId;
                                            // objexpense.tbl_order_historyid = get_orderhistory.Id;
                                            objexpense.expense_type_id = id;
                                            objexpense.expense_amount = Convert.ToDouble(item.amount);
                                            objexpense.date_created = DateTime.Now;
                                            objexpense.settlement_order_id = unique_order_id;
                                            objexpense.sku_no = sku;
                                            objexpense.Original_order_id = obj.order_id;
                                            if (item.posteddatetime != null && item.posteddatetime != "")
                                            {
                                                string datetime = item.posteddatetime.Replace("UTC", "");
                                                string datetime2 = datetime.Replace(".", "-").TrimEnd();
                                                DateTime ddd = cf.MyDateTimeConverter(datetime2);
                                                //DateTime ddd = DateTime.ParseExact(datetime2, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                                                objexpense.settlement_datetime = ddd;
                                            }
                                            //objexpense.promotion_id = item.promotion_id;
                                            if (item.qty != null)
                                                objexpense.quantity_purchased = Convert.ToInt16(item.qty);
                                            objexpense.t_transactionType_id = 1; //Order

                                            settlementDB_Context.m_tbl_expense.Add(objexpense);
                                            settlementDB_Context.SaveChanges();

                                            expense_tax_class taxobj = new expense_tax_class();
                                            taxobj.expense_db_id = objexpense.id;
                                            expenseId_Dict.Add(item.description, taxobj);
                                        }
                                        else
                                        {
                                            //TBD - Taxes
                                            if (item.description.Contains("CGST") || item.description.Contains("SGST") || item.description.Contains("IGST"))
                                            {
                                                //if (item.amount == 0)
                                                //    continue;

                                                string ss = item.description.Replace("CGST", "");
                                                ss = ss.Replace("SGST", "");
                                                ss = ss.Replace("IGST", "").Trim();
                                                if (expenseId_Dict.ContainsKey(ss))
                                                {
                                                    if (item.amount < 0)
                                                        handleDebitCredit((Decimal)item.amount);
                                                    else
                                                        handleDebitCredit((Decimal)item.amount);

                                                    expense_tax_class taxobj = expenseId_Dict[ss];
                                                    if (item.description.Contains("CGST"))
                                                    {
                                                        taxobj.cgst = item.amount;
                                                    }
                                                    else if (item.description.Contains("SGST"))
                                                    {
                                                        taxobj.sgst = item.amount;
                                                    }
                                                    else if (item.description.Contains("IGST"))
                                                    {
                                                        taxobj.igst = item.amount;
                                                    }
                                                }//end if
                                                else
                                                {
                                                    if (expenseId_Dict.Count > 0)
                                                    {
                                                        bool found = false;
                                                        foreach (KeyValuePair<string, expense_tax_class> pair1 in expenseId_Dict)
                                                        {
                                                            string k = pair1.Key;
                                                            k = Regex.Replace(k, @"\s+", "");
                                                            //k = k.Replace(' ', '');
                                                            //sharad 112
                                                            if (k == ss || (k == "AmazonEasyShipCharges" && ss == "MFNPostagePurchaseComplete"))
                                                            {
                                                                found = true;
                                                                //total_debitAmt += (Decimal)item.amount;
                                                                if (item.amount < 0)
                                                                    handleDebitCredit((Decimal)item.amount);
                                                                else
                                                                    handleDebitCredit((Decimal)item.amount);

                                                                expense_tax_class taxobj = pair1.Value;
                                                                if (item.description.Contains("CGST"))
                                                                {
                                                                    taxobj.cgst = item.amount;
                                                                }
                                                                else if (item.description.Contains("SGST"))
                                                                {
                                                                    taxobj.sgst = item.amount;
                                                                }
                                                                else if (item.description.Contains("IGST"))
                                                                {
                                                                    taxobj.igst = item.amount;
                                                                }

                                                                break;
                                                            }


                                                        }//end for loop
                                                        if (found == false)
                                                        {
                                                            if (item.amount != 0 && item.amount != null)
                                                            {
                                                                //handleDebitCredit((Decimal)item.amount);
                                                                tbl_settlement_suspense_entries obj_suspense = new tbl_settlement_suspense_entries();
                                                                obj_suspense.tbl_settlement_upload_id = upload_tbl_id;
                                                                obj_suspense.suspense_details = item.description;
                                                                obj_suspense.amount = item.amount;
                                                                obj_suspense.order_id_if_present = obj.order_id;
                                                                settlementDB_Context.tbl_settlement_suspense_entries.Add(obj_suspense);
                                                                settlementDB_Context.SaveChanges();
                                                            }
                                                            //suspense?
                                                            //goes in suspense account
                                                        }
                                                    }//end if
                                                    else
                                                    {
                                                        //key not found - sharad 231
                                                        int aaaa = 0;
                                                        if (item.amount != 0 && item.amount != null)
                                                        {
                                                            //handleDebitCredit((Decimal)item.amount);
                                                            tbl_settlement_suspense_entries obj_suspense1 = new tbl_settlement_suspense_entries();
                                                            obj_suspense1.tbl_settlement_upload_id = upload_tbl_id;
                                                            obj_suspense1.suspense_details = item.description;
                                                            obj_suspense1.amount = item.amount;
                                                            obj_suspense1.order_id_if_present = obj.order_id;
                                                            settlementDB_Context.tbl_settlement_suspense_entries.Add(obj_suspense1);
                                                            settlementDB_Context.SaveChanges();
                                                        }
                                                    }
                                                }//end else
                                            }
                                            else
                                            {
                                                int aaaa = 0;
                                                aaaa++;
                                                if (item.amount != 0 && item.amount != null)
                                                {
                                                    //handleDebitCredit((Decimal)item.amount);
                                                    tbl_settlement_suspense_entries obj_suspense = new tbl_settlement_suspense_entries();
                                                    obj_suspense.tbl_settlement_upload_id = upload_tbl_id;
                                                    obj_suspense.suspense_details = item.description;
                                                    obj_suspense.amount = item.amount;
                                                    obj_suspense.order_id_if_present = obj.order_id;
                                                    settlementDB_Context.tbl_settlement_suspense_entries.Add(obj_suspense);
                                                    settlementDB_Context.SaveChanges();
                                                }
                                                //suspense?
                                                //head not found -goes in suspense
                                            }

                                        }//end else taxes
                                    }//end else
                                }
                                //}
                            }//end inner foreach - looping for one record
                            if (obj_settlement != null)
                            {
                                settlementDB_Context.Entry(obj_settlement).State = EntityState.Modified;
                                settlementDB_Context.SaveChanges();

                                foreach (KeyValuePair<string, expense_tax_class> pair1 in expenseId_Dict)
                                {
                                    expense_tax_class taxobj = pair1.Value;
                                    if (taxobj.sgst != 0 || taxobj.igst != 0 || taxobj.cgst != 0)
                                    {
                                        tbl_tax obj1 = new tbl_tax();
                                        obj1.reference_type = 2; //settlement_order
                                        obj1.tbl_referneced_id = taxobj.expense_db_id;
                                        obj1.isactive = 1;
                                        obj1.tbl_seller_id = SellerId;
                                        obj1.sgst_amount = Convert.ToDouble(taxobj.sgst);
                                        obj1.Igst_amount = Convert.ToDouble(taxobj.igst);
                                        obj1.CGST_amount = Convert.ToDouble(taxobj.cgst);
                                        //obj1.giftwarp_tax = Convert.ToDouble(giftraptax);
                                        //objtax.tbl_history_id = get_orderhistory.Id;
                                        settlementDB_Context.tbl_tax.Add(obj1);
                                        settlementDB_Context.SaveChanges();
                                    }
                                }
                            }
                        }//end foreach
                    }
                }
                obj.order_amount_typesDict = null;
                if (obj.refund_amount_typesDict != null)
                {
                    handleRefund(file_settlement_id, obj, SellerId, upload_tbl_id);
                }
                if (obj.easyship_amount_typesDict != null)
                {
                    int cnt = 0;
                    foreach (KeyValuePair<string, List<settlement_amt_type>> pair in obj.easyship_amount_typesDict)
                    {
                        string shipment_id = pair.Key;
                        List<settlement_amt_type> easyship_amount_typesDict = pair.Value;
                        handleEasyShip(sku, cnt, obj.order_id, file_settlement_id, easyship_amount_typesDict, SellerId, upload_tbl_id);
                        cnt++;

                    }//end outer for loop
                }//end easy ship handling
            }
            catch (Exception ex)
            {
                Writelog log = new Writelog();
                log.write_exception_log(upload_tbl_id.ToString(), "CronJobController", "handleOrder", DateTime.Now, ex);
                int a = 0;
            }
        }//end function

        public void handleEasyShip(string sku, int index, string orderid, string file_settlement_id, List<settlement_amt_type> easyship_amount_typesDict, int SellerId, int upload_tbl_id)
        {
            try
            {
                //int SellerId = Convert.ToInt32(Session["SellerID"]);
                var totalcount = dba.tbl_settlement_order.Where(a => a.Order_Id == orderid && a.tbl_seller_id == SellerId && a.settlement_id == file_settlement_id).Count();
                if (totalcount > 1)
                {
                    index = 1;
                }
                string unique_order_id = orderid + "-" + index;
                expense_tax_class taxobj = null;
                foreach (settlement_amt_type item in easyship_amount_typesDict)
                {
                    var Tbl_SettlementFees = settlementDB_Context.m_settlement_fee.ToList();
                    var get_settlement_transactiontype = settlementDB_Context.m_settlement_transaction_type.ToList();
                    var get_expensedata = Tbl_SettlementFees.Where(a => a.return_fee.ToLower() == item.description.ToLower()).FirstOrDefault();
                    if (get_expensedata != null)
                    {
                        var id = get_expensedata.id;
                        m_tbl_expense objexpense = new m_tbl_expense();
                        objexpense.reference_number = file_settlement_id;
                        objexpense.tbl_seller_id = SellerId;
                        // objexpense.tbl_order_historyid = get_orderhistory.Id;
                        objexpense.expense_type_id = id;
                        objexpense.expense_amount = Convert.ToDouble(item.amount);

                        if (item.amount < 0)
                            handleDebitCredit((Decimal)item.amount);
                        else
                            handleDebitCredit((Decimal)item.amount);

                        objexpense.date_created = DateTime.Now;
                        objexpense.settlement_order_id = unique_order_id;
                        objexpense.sku_no = sku;
                        objexpense.Original_order_id = orderid;
                        if (item.posteddatetime != null && item.posteddatetime != "")
                        {
                            string datetime = item.posteddatetime.Replace("UTC", "");
                            string datetime2 = datetime.Replace(".", "-").TrimEnd();
                            DateTime ddd = cf.MyDateTimeConverter(datetime2);
                            objexpense.settlement_datetime = ddd;
                        }
                        //objexpense.promotion_id = item.promotion_id;

                        if (item.qty != null)
                            objexpense.quantity_purchased = Convert.ToInt16(item.qty);
                        objexpense.t_transactionType_id = 1; //Order

                        settlementDB_Context.m_tbl_expense.Add(objexpense);
                        settlementDB_Context.SaveChanges();

                        taxobj = new expense_tax_class();
                        taxobj.expense_db_id = objexpense.id;
                    }
                    else
                    {
                        //TBD - Taxes
                        if (item.description.Contains("CGST") || item.description.Contains("SGST") || item.description.Contains("IGST"))
                        {
                            if (item.description.Contains("CGST"))
                            {
                                taxobj.cgst = item.amount;

                            }
                            else if (item.description.Contains("SGST"))
                            {
                                taxobj.sgst = item.amount;
                            }
                            else if (item.description.Contains("IGST"))
                            {
                                taxobj.igst = item.amount;
                            }

                            if (item.amount < 0)
                                handleDebitCredit((Decimal)item.amount);
                            else
                                handleDebitCredit((Decimal)item.amount);
                        }
                        else
                        {
                            if (item.amount != 0 && item.amount != null)
                            {
                                //handleDebitCredit((Decimal)item.amount);
                                tbl_settlement_suspense_entries obj_suspense = new tbl_settlement_suspense_entries();
                                obj_suspense.tbl_settlement_upload_id = upload_tbl_id;
                                obj_suspense.suspense_details = item.description;
                                obj_suspense.amount = item.amount;
                                obj_suspense.order_id_if_present = orderid;
                                settlementDB_Context.tbl_settlement_suspense_entries.Add(obj_suspense);
                                settlementDB_Context.SaveChanges();
                            }
                            //suspense?
                            int aaaa = 0;
                            aaaa++;

                        }

                    }//end else taxes

                }//end for loop

                if (taxobj != null && (taxobj.sgst != 0 || taxobj.igst != 0 || taxobj.cgst != 0))
                {
                    tbl_tax obj1 = new tbl_tax();
                    obj1.reference_type = 2; //settlement_order
                    obj1.tbl_referneced_id = taxobj.expense_db_id;
                    obj1.isactive = 1;
                    obj1.tbl_seller_id = SellerId;
                    obj1.sgst_amount = Convert.ToDouble(taxobj.sgst);
                    obj1.Igst_amount = Convert.ToDouble(taxobj.igst);
                    obj1.CGST_amount = Convert.ToDouble(taxobj.cgst);
                    settlementDB_Context.tbl_tax.Add(obj1);
                    settlementDB_Context.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                Writelog log = new Writelog();
                log.write_exception_log(upload_tbl_id.ToString(), "CronJobController", "handleEasyShip", DateTime.Now, ex);
            }

        }


        public void handleRefund(string file_settlement_id, reconciliationorder obj, int SellerId, int upload_tbl_id)
        {
            //int SellerId = Convert.ToInt32(Session["SellerID"]);
            try
            {
                int orderSKUcounter = 0;
                if (obj.order_id == "171-7616497-6675534" || obj.order_id == "406-1128348-5744302" || obj.order_id == "405-8281746-7323528")
                {

                }
                foreach (KeyValuePair<string, List<settlement_amt_type>> pair in obj.refund_amount_typesDict)
                {
                    //orderSKUcounter++;
                    string sku = pair.Key;



                    List<settlement_amt_type> refund_amount_typesDict = pair.Value;

                    List<List<settlement_amt_type>> superlist = new List<List<settlement_amt_type>>();

                    //foreach (settlement_amt_type item in order_amount_typesDict)
                    //assuming that if theer are less than 19 values then there is only one order with one oreder item

                    //////////////////////
                    Dictionary<string, string> dictionary_deduplicate = new Dictionary<string, string>();
                    for (int k = 0; k < refund_amount_typesDict.Count; k++)
                    {
                        settlement_amt_type item = refund_amount_typesDict[k];
                        if (dictionary_deduplicate.ContainsKey(item.description))
                        {
                            dictionary_deduplicate = new Dictionary<string, string>();
                            //dictionary_deduplicate.Add(item.description, "");
                            List<settlement_amt_type> a = new List<settlement_amt_type>();
                            superlist.Add(a);
                            //a.Add(item);
                            do
                            {
                                dictionary_deduplicate.Add(item.description, "");
                                a.Add(item);
                                k++;

                                if (k == refund_amount_typesDict.Count)
                                    break;
                                else
                                    item = refund_amount_typesDict[k];
                            } while (!dictionary_deduplicate.ContainsKey(item.description));
                            if (k < refund_amount_typesDict.Count) k--;
                        }
                        else
                        {
                            List<settlement_amt_type> a = new List<settlement_amt_type>();
                            superlist.Add(a);
                            //a.Add(item);
                            do
                            {
                                dictionary_deduplicate.Add(item.description, "");
                                a.Add(item);
                                k++;

                                if (k == refund_amount_typesDict.Count)
                                    break;
                                else
                                    item = refund_amount_typesDict[k];
                            } while (!dictionary_deduplicate.ContainsKey(item.description));

                            if (k < refund_amount_typesDict.Count) k--;
                        }

                        ////////////////////////////////////////////////////////////////////////////////////////////
                    }

                    var Tbl_SettlementFees = settlementDB_Context.m_settlement_fee.ToList();
                    var get_settlement_transactiontype = settlementDB_Context.m_settlement_transaction_type.ToList();

                    //loop thru all the items for a SKU
                    foreach (List<settlement_amt_type> myskulist in superlist)
                    {
                        tbl_order_history obj_Refund = null;
                        string unique_order_id = obj.order_id + "-" + orderSKUcounter;
                        orderSKUcounter++;
                        Dictionary<String, expense_tax_class> expenseId_Dict = new Dictionary<String, expense_tax_class>();
                        if (obj.order_id == "171-7616497-6675534")
                        {

                        }

                        foreach (settlement_amt_type item in myskulist)
                        {
                            var GetRefundOrderRecord = settlementDB_Context.tbl_order_history.Where(a => a.settlement_id == file_settlement_id && a.tbl_seller_id == SellerId && a.unique_order_id == unique_order_id).FirstOrDefault();

                            if (GetRefundOrderRecord == null)
                            {
                                if (obj_Refund == null)
                                {
                                    obj_Refund = new tbl_order_history();
                                    obj_Refund.tbl_marketplace_id = m_marketplaceID;
                                    obj_Refund.OrderID = obj.order_id;
                                    obj_Refund.unique_order_id = unique_order_id;
                                    obj_Refund.tbl_seller_id = SellerId;
                                    obj_Refund.created_on = DateTime.Now;
                                    obj_Refund.SKU = sku;
                                    obj_Refund.t_order_status = 9; //refund
                                    obj_Refund.settlement_id = file_settlement_id;
                                    obj_Refund.fulfillment_id = obj.fulfillment_id;
                                    if (item.posteddatetime != null && item.posteddatetime != "")
                                    {
                                        string datetime = item.posteddatetime.Replace("UTC", "");
                                        string datetime2 = datetime.Replace(".", "-").TrimEnd();
                                        //DateTime ddd = DateTime.ParseExact(datetime2, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                                        DateTime ddd = cf.MyDateTimeConverter(datetime2);
                                        obj_Refund.ShipmentDate = ddd;
                                    }
                                    obj_Refund.LastUpdatedDateUTC = DateTime.UtcNow;
                                    if (item.qty != null)
                                        obj_Refund.Quantity = Convert.ToInt16(item.qty);
                                    settlementDB_Context.tbl_order_history.Add(obj_Refund);
                                    settlementDB_Context.SaveChanges();
                                }
                                else
                                {

                                }
                            }

                            // #region Order check

                            if (item.description != null)
                            {
                                // obj_Refund = new tbl_order_history();//vineet
                                if (item.description == "Principal" || item.description == "TotalSaleAmount")
                                {
                                    obj_Refund.amount_per_unit = item.amount;
                                    handleDebitCredit((Decimal)item.amount);
                                    //if (item.amount < 0)
                                    //    total_debitAmt += (Decimal)item.amount * (-1);
                                    //else
                                    //    total_debitAmt += (Decimal)item.amount;
                                }
                                else if (item.description == "Shipping")
                                {
                                    obj_Refund.shipping_price = item.amount;
                                    handleDebitCredit((Decimal)item.amount);
                                }
                                else if (item.description == "Gift wrap")
                                {
                                    obj_Refund.Giftwrap_price = item.amount;
                                    handleDebitCredit((Decimal)item.amount);
                                }
                                else if (item.description == "Gift Wrap Tax")
                                {
                                    obj_Refund.gift_wrap_tax = item.amount;
                                    handleDebitCredit((Decimal)item.amount);
                                }
                                else if (item.description == "Product Tax")
                                {
                                    obj_Refund.product_tax = item.amount;
                                    handleDebitCredit((Decimal)item.amount);
                                }
                                else if (item.description == "Shipping tax")
                                {
                                    obj_Refund.shipping_tax = item.amount;
                                    handleDebitCredit((Decimal)item.amount);
                                }
                                else if (item.description == "Shipping discount")
                                {
                                    obj_Refund.shipping_discount = item.amount;
                                    handleDebitCredit((Decimal)item.amount);
                                }
                                else if (item.description == "Shipping tax discount")
                                {
                                    obj_Refund.shipping_tax_discount = item.amount;
                                    handleDebitCredit((Decimal)item.amount);
                                }
                                else if (item.description == "ProtectionFund")//vineet
                                {
                                    if (item.amount != null && item.amount != 0)
                                    {
                                        obj_Refund.Protection_fund_flipkart = item.amount;
                                        handleDebitCredit((Decimal)item.amount);
                                    }
                                    else
                                    {
                                        obj_Refund.Protection_fund_flipkart = 0;
                                        handleDebitCredit((Decimal)item.amount);
                                    }
                                }
                                else
                                {

                                    var get_expensedata = Tbl_SettlementFees.Where(a => a.return_fee.ToLower() == item.description.ToLower()).FirstOrDefault();
                                    if (get_expensedata != null)
                                    {
                                        var id = get_expensedata.id;
                                        m_tbl_expense objexpense = new m_tbl_expense();
                                        objexpense.reference_number = file_settlement_id;
                                        objexpense.tbl_seller_id = SellerId;
                                        // objexpense.tbl_order_historyid = get_orderhistory.Id;
                                        objexpense.expense_type_id = id;
                                        objexpense.expense_amount = Convert.ToDouble(item.amount);


                                        handleDebitCredit((Decimal)item.amount);



                                        objexpense.date_created = DateTime.Now;
                                        objexpense.settlement_order_id = unique_order_id;
                                        objexpense.tbl_order_historyid = obj_Refund.Id;
                                        objexpense.sku_no = sku;
                                        objexpense.Original_order_id = obj.order_id;
                                        if (item.posteddatetime != null && item.posteddatetime != "")
                                        {
                                            string datetime = item.posteddatetime.Replace("UTC", "");
                                            string datetime2 = datetime.Replace(".", "-").TrimEnd();
                                            DateTime ddd = cf.MyDateTimeConverter(datetime2);
                                            objexpense.settlement_datetime = ddd;
                                        }
                                        //objexpense.promotion_id = item.promotion_id;

                                        if (item.qty != null)
                                            objexpense.quantity_purchased = Convert.ToInt16(item.qty);
                                        objexpense.t_transactionType_id = 2; //Refund

                                        settlementDB_Context.m_tbl_expense.Add(objexpense);
                                        settlementDB_Context.SaveChanges();

                                        expense_tax_class taxobj = new expense_tax_class();
                                        taxobj.expense_db_id = objexpense.id;
                                        expenseId_Dict.Add(item.description, taxobj);
                                    }
                                    else
                                    {
                                        //TBD - Taxes
                                        if (item.description.Contains("CGST") || item.description.Contains("SGST") || item.description.Contains("IGST"))
                                        {
                                            string ss = item.description.Replace("CGST", "");
                                            ss = ss.Replace("SGST", "");
                                            ss = ss.Replace("IGST", "").Trim();
                                            if (expenseId_Dict.ContainsKey(ss))
                                            {
                                                expense_tax_class taxobj = expenseId_Dict[ss];
                                                if (item.description.Contains("CGST"))
                                                {
                                                    taxobj.cgst = item.amount;
                                                    handleDebitCredit((Decimal)item.amount);
                                                }
                                                else if (item.description.Contains("SGST"))
                                                {
                                                    taxobj.sgst = item.amount;
                                                    handleDebitCredit((Decimal)item.amount);
                                                }
                                                else if (item.description.Contains("IGST"))
                                                {
                                                    taxobj.igst = item.amount;
                                                    handleDebitCredit((Decimal)item.amount);
                                                }
                                            }//end if
                                            else
                                            {
                                                if (expenseId_Dict.Count > 0)
                                                {
                                                    bool found = false;
                                                    foreach (KeyValuePair<string, expense_tax_class> pair1 in expenseId_Dict)
                                                    {
                                                        string k = pair1.Key;
                                                        k = Regex.Replace(k, @"\s+", "");
                                                        //k = k.Replace(' ', '');
                                                        //sharad 112
                                                        if (k == ss || (k == "AmazonEasyShipCharges" && ss == "MFNPostagePurchaseComplete"))
                                                        {
                                                            found = true;
                                                            //total_debitAmt += (Decimamal)item.amount;
                                                            handleDebitCredit((Decimal)item.amount);
                                                            //if (item.amount < 0)
                                                            //    handleDebit((Decimal)item.amount * (-1));
                                                            //else
                                                            //    handleDebit((Decimal)item.amount);

                                                            expense_tax_class taxobj = pair1.Value;
                                                            if (item.description.Contains("CGST"))
                                                            {
                                                                taxobj.cgst = item.amount;
                                                            }
                                                            else if (item.description.Contains("SGST"))
                                                            {
                                                                taxobj.sgst = item.amount;
                                                            }
                                                            else if (item.description.Contains("IGST"))
                                                            {
                                                                taxobj.igst = item.amount;
                                                            }

                                                            break;
                                                        }


                                                    }//end for loop
                                                    if (found == false)
                                                    {
                                                        if (item.amount != 0 && item.amount != null)
                                                        {
                                                            //handleDebitCredit((Decimal)item.amount);
                                                            tbl_settlement_suspense_entries obj_suspense = new tbl_settlement_suspense_entries();
                                                            obj_suspense.tbl_settlement_upload_id = upload_tbl_id;
                                                            obj_suspense.suspense_details = item.description;
                                                            obj_suspense.amount = item.amount;
                                                            obj_suspense.order_id_if_present = obj.order_id;
                                                            settlementDB_Context.tbl_settlement_suspense_entries.Add(obj_suspense);
                                                            //dba.SaveChanges();
                                                        }
                                                        //suspense?
                                                        //goes in suspense account
                                                    }
                                                }//end if
                                                else
                                                {
                                                    //key not found - sharad 231
                                                    int aaaa = 0;
                                                    if (item.amount != 0 && item.amount != null)
                                                    {
                                                        //handleDebitCredit((Decimal)item.amount);
                                                        tbl_settlement_suspense_entries obj_suspense1 = new tbl_settlement_suspense_entries();
                                                        obj_suspense1.tbl_settlement_upload_id = upload_tbl_id;
                                                        obj_suspense1.suspense_details = item.description;
                                                        obj_suspense1.amount = item.amount;
                                                        obj_suspense1.order_id_if_present = obj.order_id;
                                                        settlementDB_Context.tbl_settlement_suspense_entries.Add(obj_suspense1);
                                                        //dba.SaveChanges();
                                                    }
                                                }

                                            }//end else
                                        }
                                        else
                                        {
                                            int aaaa = 0;
                                            aaaa++;
                                            if (item.amount != 0 && item.amount != null)
                                            {
                                                //handleDebitCredit((Decimal)item.amount);
                                                tbl_settlement_suspense_entries obj_suspense = new tbl_settlement_suspense_entries();
                                                obj_suspense.tbl_settlement_upload_id = upload_tbl_id;
                                                obj_suspense.suspense_details = item.description;
                                                obj_suspense.amount = item.amount;
                                                obj_suspense.order_id_if_present = obj.order_id;
                                                settlementDB_Context.tbl_settlement_suspense_entries.Add(obj_suspense);
                                                //dba.SaveChanges();
                                            }
                                            //suspense?
                                            //head not found -goes in suspense
                                        }

                                    }//end else taxes
                                }//end else
                            }
                            else
                            {
                                //item description null
                                int a = 0;
                                a++;
                                if (item.amount != 0 && item.amount != null)
                                {
                                    //handleDebitCredit((Decimal)item.amount);
                                    tbl_settlement_suspense_entries obj_suspense = new tbl_settlement_suspense_entries();
                                    obj_suspense.tbl_settlement_upload_id = upload_tbl_id;
                                    obj_suspense.suspense_details = item.description;
                                    obj_suspense.amount = item.amount;
                                    obj_suspense.order_id_if_present = obj.order_id;
                                    settlementDB_Context.tbl_settlement_suspense_entries.Add(obj_suspense);
                                    settlementDB_Context.SaveChanges();
                                }
                            }
                            //}
                        }//end inner foreach - looping for one record
                        if (obj_Refund != null)
                        {
                            settlementDB_Context.Entry(obj_Refund).State = EntityState.Modified;
                            settlementDB_Context.SaveChanges();

                            foreach (KeyValuePair<string, expense_tax_class> pair1 in expenseId_Dict)
                            {
                                expense_tax_class taxobj = pair1.Value;
                                if (taxobj.sgst != 0 || taxobj.igst != 0 || taxobj.cgst != 0)
                                {

                                    tbl_tax obj1 = new tbl_tax();
                                    obj1.reference_type = 7; //Order_history
                                    obj1.tbl_referneced_id = taxobj.expense_db_id;
                                    obj1.isactive = 1;
                                    obj1.tbl_seller_id = SellerId;
                                    obj1.sgst_amount = Convert.ToDouble(taxobj.sgst);
                                    obj1.Igst_amount = Convert.ToDouble(taxobj.igst);
                                    obj1.CGST_amount = Convert.ToDouble(taxobj.cgst);
                                    //obj1.giftwarp_tax = Convert.ToDouble(giftraptax);
                                    //objtax.tbl_history_id = get_orderhistory.Id;
                                    settlementDB_Context.tbl_tax.Add(obj1);
                                    settlementDB_Context.SaveChanges();
                                }
                            }
                        }
                    }


                }//end foreach

                if (obj.order_amount_typesDict != null)
                {
                    obj.refund_amount_typesDict = null;
                    handleOrder(file_settlement_id, obj, SellerId, upload_tbl_id);
                }
            }
            catch (Exception ex)
            {
                Writelog log = new Writelog();
                log.write_exception_log(upload_tbl_id.ToString(), "CronJobController", "handleRefund", DateTime.Now, ex);
                int a = 0;
            }
        }

        int m_marketplaceID;
        int SellerId;
        Decimal total_debitAmt = 0;
        Decimal total_creditAmt = 0;
        Decimal suspense_Amount = 0;
        public string suspense_data = "";

        List<Decimal> list_debit, list_credit;

        /*private void handleDebit(Decimal amt)
        {
            //if (list_debit == null)
            //    list_debit = new List<Decimal>();

            //list_debit.Add(amt);
            total_debitAmt += amt;
        }

        private void handleCredit(Decimal amt)
        {
            //if (list_credit == null)
            //    list_credit = new List<Decimal>();

           // list_credit.Add(amt);
            total_creditAmt += amt;
        }*/


        private void handleDebitCredit(Decimal amt)
        {
            //if (list_credit == null)
            //    list_credit = new List<Decimal>();

            // list_credit.Add(amt);

            if (amt < 0)
                total_debitAmt += (amt * (-1));
            else
                total_creditAmt += amt;
        }

        SellerContext settlementDB_Context;

        public string SaveBulksettlementdata(List<AmazonreconciliationOrder> objjson1, int upload_tbl_id, int marketplaceID, int SellerId, string filename)
        {
            string Reference_No = "";
            string datasaved = "S";
            int count = 0;
            m_marketplaceID = marketplaceID;


            using (settlementDB_Context = new SellerContext())
            {
                settlementDB_Context.Configuration.AutoDetectChangesEnabled = false;
                if (objjson1 != null)
                {
                    foreach (var ref_no in objjson1[0].reconciliationorder)
                    {
                        Reference_No = ref_no.settlement_id;
                        break;
                    }
                    var get_upload = settlementDB_Context.tbl_settlement_upload.Where(a => a.tbl_seller_id == SellerId && a.settlement_refernece_no == Reference_No && a.market_place_id == marketplaceID).FirstOrDefault();

                    if (get_upload == null)
                    {
                        count = objjson1[0].reconciliationorder.Count();
                    }
                    else
                    {
                        if (upload_tbl_id == get_upload.Id)
                        {
                            //the record found is same as the current one. May be two cron jobs are doing it together
                            //return "";
                        }
                        else
                        {
                            var getdetails = settlementDB_Context.tbl_settlement_upload.Where(a => a.Id == upload_tbl_id && a.tbl_seller_id == SellerId).FirstOrDefault();
                            if (getdetails != null)
                            {
                                getdetails.file_status = "Discarded (Ref No. Already exist)";
                                getdetails.settlement_refernece_no = Reference_No;
                                if (filename != null)
                                {
                                    getdetails.file_name = filename;
                                }
                                getdetails.LastUpdatedDateUTC = DateTime.UtcNow;
                                settlementDB_Context.Entry(getdetails).State = EntityState.Modified;
                                settlementDB_Context.SaveChanges();
                                //count = objjson1[0].reconciliationorder.Count();
                            }
                        }
                        //already exist - 
                        return datasaved = "File with same Settlement Reference number already Exists. ";
                    }
                }
                //sharad - commented
                cf = new comman_function();
                //bool ss = cf.session_check();

                tbl_tax storageFee_tax = null;
                try
                {
                    var get_upload_settlement_details = settlementDB_Context.tbl_settlement_upload.Where(a => a.tbl_seller_id == SellerId && a.Id == upload_tbl_id).FirstOrDefault();
                    if (get_upload_settlement_details != null)
                    {
                        //get_upload_settlement_details.file_status = "Processing";
                        //get_upload_settlement_details.processing_datetime = DateTime.Now;
                        settlementDB_Context.Entry(get_upload_settlement_details).State = EntityState.Modified;

                        if (get_upload_settlement_details.settlement_refernece_no != null)
                        {
                            tbl_settlement_upload objuplaod = null;
                            //insert a new upload_table_entry
                            objuplaod = new tbl_settlement_upload();
                            objuplaod.market_place_id = marketplaceID;
                            objuplaod.tbl_seller_id = SellerId;
                            objuplaod.uploaded_on = DateTime.Now;
                            objuplaod.uploaded_by = SellerId;
                            objuplaod.request_fromdate = get_upload_settlement_details.request_fromdate;
                            objuplaod.request_date_to = get_upload_settlement_details.request_date_to;
                            objuplaod.Source = 2; //api
                            objuplaod.settlement_type = 0;
                            if (filename != null)
                            {
                                objuplaod.file_name = filename;
                            }
                            objuplaod.settlement_refernece_no = Reference_No;
                            objuplaod.processing_datetime = DateTime.Now;
                            objuplaod.LastUpdatedDateUTC = DateTime.UtcNow;
                            settlementDB_Context.tbl_settlement_upload.Add(objuplaod);
                            settlementDB_Context.SaveChanges();
                            upload_tbl_id = objuplaod.Id;
                        }
                        else
                        {
                            //update
                            get_upload_settlement_details.settlement_refernece_no = Reference_No;
                            if (filename != null)
                            {
                                get_upload_settlement_details.file_name = filename;
                                get_upload_settlement_details.LastUpdatedDateUTC = DateTime.UtcNow;
                            }
                        }

                        settlementDB_Context.SaveChanges();
                    }
                    int current_running_no = 0;

                    if (objjson1 != null)
                    {
                        // var get_upload = dba.tbl_settlement_upload.Where(a => a.tbl_seller_id == SellerId && a.settlement_refernece_no == Reference_No).FirstOrDefault();
                        // var get_uploadid = dba.tbl_settlement_upload.Where(a => a.tbl_seller_id == SellerId && a.Id == id).FirstOrDefault();

                        // if (get_upload == null)
                        //{

                        int totCounter = 0;
                        //sharad221
                        total_creditAmt = 0;
                        total_debitAmt = 0;
                        suspense_Amount = 0;

                        int uniqueupload = 0;
                        var get_seller_setting = settlementDB_Context.tbl_seller_setting.Where(a => a.tbl_seller_id == SellerId && a.type == 2).FirstOrDefault();
                        var get_uploadid = settlementDB_Context.tbl_settlement_upload.Where(a => a.tbl_seller_id == SellerId && a.Id == upload_tbl_id).FirstOrDefault();

                        if (get_seller_setting != null)
                        {
                            current_running_no = Convert.ToInt16(get_seller_setting.current_running_no);
                        }
                        {
                            string type1 = "";
                            string last_readtype;
                            int settelmentupload_id = 0;
                            string file_settlement_id = "";


                            foreach (var item in objjson1[0].reconciliationorder)
                            {
                                totCounter++;
                                last_readtype = type1;
                                type1 = "";
                                if (item.transaction_type != null)
                                    type1 = item.transaction_type.ToLower();

                                //if (item.order_id == "OD110787505195404000")
                                //{

                                //}
                                if (item.transaction_type == "SAFE-T Reimbursement" || item.transaction_type == "Cancellation" || (item.transaction_type == "other-transaction" && item.order_id != null))
                                {
                                    handleOrder(file_settlement_id, item, SellerId, upload_tbl_id);
                                }
                                else if (item.transaction_type == "Order" || item.transaction_type == "NA")//changes vineet
                                {

                                    handleOrder(file_settlement_id, item, SellerId, upload_tbl_id);
                                }
                                else if (item.transaction_type == "Refund" || item.transaction_type == "Courier Return" || item.transaction_type == "Customer Return")//changes vineet
                                {
                                    handleRefund(file_settlement_id, item, SellerId, upload_tbl_id);
                                }
                                else if (item.deposit_date != "" && item.settlement_start_date != "" && item.settlement_end_date != "")
                                {
                                    //if (get_uploadid != null)
                                    {
                                        DateTime ddd2;
                                        if (item.settlement_start_date != "" && item.settlement_start_date != null)
                                        {
                                            string strT = item.settlement_start_date;
                                            string datetime3 = item.settlement_start_date.Replace("UTC", "");
                                            string datetime4 = datetime3.Replace(".", "-").TrimEnd();
                                            ddd2 = cf.MyDateTimeConverter(datetime4);
                                            get_uploadid.settlement_from = ddd2;
                                        }

                                        DateTime ddd1;
                                        if (item.settlement_end_date != "" && item.settlement_end_date != null)
                                        {
                                            string datetime5 = item.settlement_end_date.Replace("UTC", "");
                                            string datetime6 = datetime5.Replace(".", "-").TrimEnd();
                                            ddd1 = cf.MyDateTimeConverter(datetime6);
                                            get_uploadid.settlement_to = ddd1;
                                        }

                                        DateTime ddd3;
                                        if (item.deposit_date != "" && item.deposit_date != null)
                                        {
                                            string datetime7 = item.deposit_date.Replace("UTC", "");
                                            string datetime8 = datetime7.Replace(".", "-").TrimEnd();
                                            ddd3 = cf.MyDateTimeConverter(datetime8);
                                            get_uploadid.deposit_date = ddd3;
                                        }
                                        file_settlement_id = item.settlement_id;
                                        get_uploadid.settlement_refernece_no = item.settlement_id;

                                        // objuplaod.settlement_type = Convert.ToInt16(settlementuploadtype);
                                        get_uploadid.status = 0;

                                        get_uploadid.voucher_running_no = current_running_no;// add by vineet 
                                        if (item.otherTransatanctionList != null)
                                        {
                                            foreach (var otherT in item.otherTransatanctionList)
                                            {
                                                if (otherT.description == "Previous Reserve Amount Balance")
                                                {

                                                    get_uploadid.previous_reserve_amount = Convert.ToDecimal(otherT.amount);
                                                    handleDebitCredit((Decimal)get_uploadid.previous_reserve_amount);
                                                }
                                                else if (otherT.description == "NonSubscriptionFeeAdj")
                                                {
                                                    if (get_uploadid.Nonsubscription_feeadj == null)
                                                        get_uploadid.Nonsubscription_feeadj = 0;

                                                    get_uploadid.Nonsubscription_feeadj += Convert.ToDecimal(otherT.amount);
                                                    if (Convert.ToDecimal(otherT.amount) < 0)
                                                    {
                                                        handleDebitCredit(Convert.ToDecimal(otherT.amount));
                                                    }
                                                    else
                                                    {
                                                        handleDebitCredit(Convert.ToDecimal(otherT.amount));
                                                    }
                                                }
                                                else if (otherT.description == "Current Reserve Amount")
                                                {
                                                    get_uploadid.current_reserve_amount = Convert.ToDecimal(otherT.amount);
                                                    if (get_uploadid.current_reserve_amount < 0)
                                                    {
                                                        handleDebitCredit((Decimal)get_uploadid.current_reserve_amount);
                                                    }
                                                    else
                                                    {
                                                        handleDebitCredit((Decimal)get_uploadid.current_reserve_amount);
                                                    }
                                                } //sharad
                                                else if (otherT.description == "INCORRECT_FEES_ITEMS")
                                                {
                                                    if (get_uploadid.INCORRECT_FEES_ITEMS == null)
                                                        get_uploadid.INCORRECT_FEES_ITEMS = 0;

                                                    get_uploadid.INCORRECT_FEES_ITEMS += Convert.ToDecimal(otherT.amount);

                                                    handleDebitCredit(Convert.ToDecimal(otherT.amount));
                                                }
                                                else if (otherT.description == "TransactionTotalAmount")
                                                {
                                                    if (get_uploadid.Cost_of_Advertising == null)
                                                        get_uploadid.Cost_of_Advertising = 0;

                                                    get_uploadid.Cost_of_Advertising += Convert.ToDecimal(otherT.amount);

                                                    //handleDebit(Convert.ToDecimal(otherT.amount));   
                                                    if (otherT.amount < 0)
                                                    {
                                                        handleDebitCredit((Decimal)otherT.amount);
                                                    }
                                                    else
                                                    {
                                                        handleDebitCredit((Decimal)otherT.amount);
                                                    }
                                                }
                                                else if (otherT.description == "Storage Fee") //sharad21
                                                {
                                                    if (get_uploadid.Storage_Fee == null)
                                                        get_uploadid.Storage_Fee = 0;

                                                    get_uploadid.Storage_Fee += Convert.ToDecimal(otherT.amount);

                                                    if (otherT.amount >= 0)
                                                        handleDebitCredit((Decimal)Convert.ToDouble(otherT.amount));
                                                    else
                                                        handleDebitCredit((Decimal)Convert.ToDouble(otherT.amount));
                                                }
                                                else if (otherT.description == "StorageBillingCGST")
                                                {
                                                    if (storageFee_tax == null)
                                                        storageFee_tax = new tbl_tax();
                                                    storageFee_tax.reference_type = 10; //settlement_upload
                                                    storageFee_tax.isactive = 1;
                                                    storageFee_tax.tbl_seller_id = SellerId;

                                                    storageFee_tax.CGST_amount = Convert.ToDouble(otherT.amount);
                                                    handleDebitCredit(Convert.ToDecimal(otherT.amount));
                                                }
                                                else if (otherT.description == "StorageBillingSGST")
                                                {
                                                    if (storageFee_tax == null)
                                                        storageFee_tax = new tbl_tax();

                                                    storageFee_tax.reference_type = 10; //settlement_upload

                                                    storageFee_tax.isactive = 1;
                                                    storageFee_tax.tbl_seller_id = SellerId;
                                                    storageFee_tax.sgst_amount = Convert.ToDouble(otherT.amount);
                                                    handleDebitCredit(Convert.ToDecimal(otherT.amount));
                                                }
                                                else if (otherT.description == "BalanceAdjustment") //sharad21
                                                {
                                                    if (get_uploadid.BalanceAdjustment == null)
                                                        get_uploadid.BalanceAdjustment = 0;

                                                    get_uploadid.BalanceAdjustment += Convert.ToDecimal(otherT.amount);
                                                    handleDebitCredit(Convert.ToDecimal(otherT.amount));
                                                }
                                                else if (otherT.description == "Payable to Amazon") //sharad91
                                                {
                                                    if (get_uploadid.Payable_to_Amazon == null)
                                                        get_uploadid.Payable_to_Amazon = 0;

                                                    get_uploadid.Payable_to_Amazon += Convert.ToDecimal(otherT.amount);

                                                    if (otherT.amount > 0)
                                                        handleDebitCredit(Convert.ToDecimal(otherT.amount));
                                                    else
                                                        handleDebitCredit(Convert.ToDecimal(otherT.amount));


                                                }
                                                else if (otherT.description == "FBAInboundTransportationFee") //vineet new 
                                                {
                                                    if (get_uploadid.FBAInboundTransportationFee == null)
                                                        get_uploadid.FBAInboundTransportationFee = 0;

                                                    get_uploadid.FBAInboundTransportationFee += Convert.ToDecimal(otherT.amount);
                                                    handleDebitCredit(Convert.ToDecimal(otherT.amount));
                                                }
                                                else if (otherT.description == "CGST")
                                                {
                                                    if (storageFee_tax == null)
                                                        storageFee_tax = new tbl_tax();
                                                    storageFee_tax.reference_type = 10; //settlement_upload
                                                    storageFee_tax.isactive = 1;
                                                    storageFee_tax.tbl_seller_id = SellerId;
                                                    handleDebitCredit((Decimal)Convert.ToDouble(otherT.amount));
                                                    storageFee_tax.CGST_amount = Convert.ToDouble(otherT.amount);
                                                    //?
                                                }
                                                else if (otherT.description == "SGST")
                                                {
                                                    if (storageFee_tax == null)
                                                        storageFee_tax = new tbl_tax();

                                                    storageFee_tax.reference_type = 10; //settlement_upload
                                                    storageFee_tax.isactive = 1;
                                                    storageFee_tax.tbl_seller_id = SellerId;
                                                    storageFee_tax.sgst_amount = Convert.ToDouble(otherT.amount);
                                                    handleDebitCredit((Decimal)Convert.ToDouble(otherT.amount));
                                                    //?
                                                }
                                                else if (otherT.description == "Storage Fee Tax")
                                                {
                                                    if (storageFee_tax == null)
                                                        storageFee_tax = new tbl_tax();

                                                    storageFee_tax.reference_type = 10; //settlement_upload
                                                    storageFee_tax.isactive = 1;
                                                    storageFee_tax.tbl_seller_id = SellerId;
                                                    storageFee_tax.sgst_amount = Convert.ToDouble(otherT.amount);

                                                    if (otherT.amount >= 0)
                                                        handleDebitCredit((Decimal)Convert.ToDouble(otherT.amount));
                                                    else
                                                        handleDebitCredit((Decimal)Convert.ToDouble(otherT.amount));
                                                    //?
                                                }
                                                else
                                                {
                                                    if (item.amount != "" && item.amount != null && item.amount != "0")
                                                    {
                                                        tbl_settlement_suspense_entries obj_suspense = new tbl_settlement_suspense_entries();
                                                        obj_suspense.tbl_settlement_upload_id = upload_tbl_id;
                                                        obj_suspense.suspense_details = otherT.description;
                                                        obj_suspense.amount = otherT.amount;
                                                        obj_suspense.order_id_if_present = item.order_id;
                                                        settlementDB_Context.tbl_settlement_suspense_entries.Add(obj_suspense);
                                                        settlementDB_Context.SaveChanges();
                                                    }

                                                    //suspense?
                                                }
                                            }
                                        }

                                        settlementDB_Context.Entry(get_uploadid).State = EntityState.Modified;
                                        settlementDB_Context.SaveChanges();
                                        settelmentupload_id = upload_tbl_id;

                                        if (storageFee_tax != null)
                                        {
                                            storageFee_tax.tbl_referneced_id = settelmentupload_id;
                                            settlementDB_Context.tbl_tax.Add(storageFee_tax);
                                        }

                                        tbl_imp_bank_transfers objbank = new tbl_imp_bank_transfers();
                                        objbank.tbl_settlement_upload_id = upload_tbl_id;
                                        if (item.total_amount != null && item.total_amount != "")
                                        {
                                            objbank.amount = Convert.ToDecimal(item.total_amount);
                                            //handleDebit((Decimal)objbank.amount);
                                            //sharad - 221
                                            handleDebitCredit((Decimal)objbank.amount * (-1));
                                            /* if (objbank.amount > 0)
                                                 handleDebitCredit((Decimal)objbank.amount);
                                             else
                                                 handleDebitCredit((Decimal)objbank.amount);*/
                                        }
                                        else
                                        {
                                            objbank.amount = 0;
                                        }
                                        settlementDB_Context.tbl_imp_bank_transfers.Add(objbank);
                                        settlementDB_Context.SaveChanges();
                                    }
                                }// end of else if

                                else if (item.otherTransatanctionList != null)
                                {

                                    foreach (var otherT in item.otherTransatanctionList)
                                    {
                                        if (otherT.description == "Previous Reserve Amount Balance")
                                        {
                                            get_uploadid.previous_reserve_amount = Convert.ToDecimal(otherT.amount);
                                            handleDebitCredit((Decimal)get_uploadid.previous_reserve_amount);
                                        }
                                        else if (otherT.description == "NonSubscriptionFeeAdj")
                                        {
                                            if (get_uploadid.Nonsubscription_feeadj == null)
                                                get_uploadid.Nonsubscription_feeadj = 0;

                                            get_uploadid.Nonsubscription_feeadj += Convert.ToDecimal(otherT.amount);

                                            if (Convert.ToDecimal(otherT.amount) < 0)
                                            {
                                                handleDebitCredit(Convert.ToDecimal(otherT.amount));
                                            }
                                            else
                                            {
                                                handleDebitCredit(Convert.ToDecimal(otherT.amount));
                                            }
                                        }
                                        else if (otherT.description == "Current Reserve Amount")
                                        {
                                            get_uploadid.current_reserve_amount = Convert.ToDecimal(otherT.amount);
                                            if (get_uploadid.current_reserve_amount < 0)
                                            {
                                                handleDebitCredit((Decimal)get_uploadid.current_reserve_amount);
                                            }
                                            else
                                            {
                                                handleDebitCredit((Decimal)get_uploadid.current_reserve_amount);
                                            }
                                        } //sharad
                                        else if (otherT.description == "INCORRECT_FEES_ITEMS")
                                        {
                                            get_uploadid.INCORRECT_FEES_ITEMS += Convert.ToDecimal(otherT.amount);
                                            handleDebitCredit(Convert.ToDecimal(otherT.amount));
                                        }
                                        else if (otherT.description == "TransactionTotalAmount")
                                        {
                                            get_uploadid.Cost_of_Advertising += Convert.ToDecimal(otherT.amount);

                                            if (Convert.ToDecimal(otherT.amount) < 0)
                                            {
                                                handleDebitCredit(Convert.ToDecimal(otherT.amount));
                                            }
                                            else
                                            {
                                                handleDebitCredit(Convert.ToDecimal(otherT.amount));
                                            }
                                            //handleDebit(Convert.ToDecimal(otherT.amount));
                                        }
                                        else if (otherT.description == "Storage Fee") //sharad21
                                        {
                                            if (get_uploadid.Storage_Fee == null)
                                                get_uploadid.Storage_Fee = 0;

                                            get_uploadid.Storage_Fee += Convert.ToDecimal(otherT.amount);
                                            if (otherT.amount >= 0)
                                                handleDebitCredit((Decimal)Convert.ToDouble(otherT.amount));
                                            else
                                                handleDebitCredit((Decimal)Convert.ToDouble(otherT.amount));
                                        }
                                        else if (otherT.description == "StorageBillingCGST")
                                        {
                                            if (storageFee_tax == null)
                                                storageFee_tax = new tbl_tax();
                                            storageFee_tax.reference_type = 10; //settlement_upload
                                            storageFee_tax.isactive = 1;
                                            storageFee_tax.tbl_seller_id = SellerId;

                                            storageFee_tax.CGST_amount = Convert.ToDouble(otherT.amount);
                                            handleDebitCredit(Convert.ToDecimal(otherT.amount));
                                        }
                                        else if (otherT.description == "StorageBillingSGST")
                                        {
                                            if (storageFee_tax == null)
                                                storageFee_tax = new tbl_tax();

                                            storageFee_tax.reference_type = 10; //settlement_upload

                                            storageFee_tax.isactive = 1;
                                            storageFee_tax.tbl_seller_id = SellerId;
                                            storageFee_tax.sgst_amount = Convert.ToDouble(otherT.amount);
                                            handleDebitCredit(Convert.ToDecimal(otherT.amount));
                                        }
                                        else if (otherT.description == "BalanceAdjustment") //sharad21
                                        {
                                            if (get_uploadid.BalanceAdjustment == null)
                                                get_uploadid.BalanceAdjustment = 0;

                                            get_uploadid.BalanceAdjustment += Convert.ToDecimal(otherT.amount);
                                            handleDebitCredit(Convert.ToDecimal(otherT.amount));
                                        }
                                        else if (otherT.description == "Payable to Amazon") //sharad91
                                        {
                                            if (get_uploadid.Payable_to_Amazon == null)
                                                get_uploadid.Payable_to_Amazon = 0;

                                            get_uploadid.Payable_to_Amazon += Convert.ToDecimal(otherT.amount);

                                            if (otherT.amount > 0)
                                                handleDebitCredit(Convert.ToDecimal(otherT.amount));
                                            else
                                                handleDebitCredit(Convert.ToDecimal(otherT.amount));
                                        }

                                        else if (otherT.description == "FBAInboundTransportationFee") //vineet new 
                                        {
                                            if (get_uploadid.FBAInboundTransportationFee == null)
                                                get_uploadid.FBAInboundTransportationFee = 0;

                                            get_uploadid.FBAInboundTransportationFee += Convert.ToDecimal(otherT.amount);
                                            handleDebitCredit(Convert.ToDecimal(otherT.amount));
                                        }
                                        else if (otherT.description == "CGST")
                                        {
                                            if (storageFee_tax == null)
                                                storageFee_tax = new tbl_tax();
                                            storageFee_tax.reference_type = 10; //settlement_upload
                                            storageFee_tax.isactive = 1;
                                            storageFee_tax.tbl_seller_id = SellerId;
                                            handleDebitCredit((Decimal)Convert.ToDouble(otherT.amount));
                                            storageFee_tax.CGST_amount = Convert.ToDouble(otherT.amount);
                                            //?
                                        }
                                        else if (otherT.description == "SGST")
                                        {
                                            if (storageFee_tax == null)
                                                storageFee_tax = new tbl_tax();

                                            storageFee_tax.reference_type = 10; //settlement_upload
                                            storageFee_tax.isactive = 1;
                                            storageFee_tax.tbl_seller_id = SellerId;
                                            storageFee_tax.sgst_amount = Convert.ToDouble(otherT.amount);
                                            handleDebitCredit((Decimal)Convert.ToDouble(otherT.amount));
                                            //?
                                        }
                                        else if (otherT.description == "Storage Fee Tax")
                                        {
                                            if (storageFee_tax == null)
                                                storageFee_tax = new tbl_tax();

                                            storageFee_tax.reference_type = 10; //settlement_upload
                                            storageFee_tax.isactive = 1;
                                            storageFee_tax.tbl_seller_id = SellerId;
                                            storageFee_tax.sgst_amount = Convert.ToDouble(otherT.amount);

                                            if (otherT.amount >= 0)
                                                handleDebitCredit((Decimal)Convert.ToDouble(otherT.amount));
                                            else
                                                handleDebitCredit((Decimal)Convert.ToDouble(otherT.amount));
                                            //?
                                        }
                                        else
                                        {
                                            if (item.amount != "" && item.amount != null && item.amount != "0")
                                            {
                                                tbl_settlement_suspense_entries obj_suspense = new tbl_settlement_suspense_entries();
                                                obj_suspense.tbl_settlement_upload_id = upload_tbl_id;
                                                obj_suspense.suspense_details = otherT.description;
                                                obj_suspense.amount = otherT.amount;
                                                obj_suspense.order_id_if_present = item.order_id;
                                                settlementDB_Context.tbl_settlement_suspense_entries.Add(obj_suspense);
                                                settlementDB_Context.SaveChanges();
                                            }
                                            //suspense?
                                        }
                                    }
                                    settlementDB_Context.Entry(get_uploadid).State = EntityState.Modified;
                                    settlementDB_Context.SaveChanges();
                                }
                            }//end for
                        }


                        string connectionstring = ConfigurationManager.ConnectionStrings["SellerConnection"].ConnectionString;
                        MySqlConnection con = new MySqlConnection(connectionstring);
                        var get_unique_count_details = "SELECT count(DISTINCT aa.id) FROM(SELECT DISTINCT amazon_order_id AS id FROM `tbl_sales_order` where tbl_sellers_id = " + SellerId + " UNION ALL SELECT DISTINCT OrderID AS id FROM `tbl_order_history` where tbl_seller_id =" + SellerId + " UNION ALL SELECT DISTINCT Order_Id AS id FROM `tbl_settlement_order` where tbl_seller_id =" + SellerId + " ) AS aa";
                        MySqlCommand cmd = new MySqlCommand(get_unique_count_details, con);
                        con.Open();
                        MySqlDataReader dr = cmd.ExecuteReader();
                        if (dr.HasRows)
                        {
                            dr.Read();
                            uniqueupload = Convert.ToInt32(dr[0]);
                        }
                        cmd.Dispose();
                        con.Close();
                        var diff_order = 0;
                        int totalOrder = 0;
                        var get_balance_details = db.tbl_user_login.Where(a => a.tbl_sellers_id == SellerId).FirstOrDefault();
                        if (get_balance_details != null)
                        {
                            var last_order = get_balance_details.total_orders;
                            diff_order = Convert.ToInt16(uniqueupload - last_order);
                            //diff_order = Convert.ToInt16(uniqueupload);
                            var plan_rate = get_balance_details.applied_plan_rate * diff_order;
                            get_balance_details.wallet_balance = get_balance_details.wallet_balance - Convert.ToDouble(plan_rate);
                            totalOrder = Convert.ToInt16(diff_order);
                            if (get_balance_details.total_orders != null)
                                totalOrder = totalOrder + Convert.ToInt16(get_balance_details.total_orders);
                            get_balance_details.total_orders = totalOrder;
                            db.Entry(get_balance_details).State = EntityState.Modified;
                            db.SaveChanges();

                            var get_upload_settlement = settlementDB_Context.tbl_settlement_upload.Where(a => a.tbl_seller_id == SellerId && a.Id == upload_tbl_id).FirstOrDefault();
                            if (get_upload_settlement != null)
                            {
                                //suspense account
                                if (total_creditAmt != total_debitAmt)
                                {
                                    suspense_Amount = total_debitAmt - total_creditAmt;
                                    get_upload_settlement.suspense_amt = suspense_Amount;
                                }
                                get_upload_settlement.new_order_uploaded = count - 1;//diff_order;
                                //get_upload_settlement_details.new_order_uploaded = totalOrder;
                                get_upload_settlement.file_status = "Completed";
                                get_upload_settlement.completed_datetime = DateTime.Now;
                                settlementDB_Context.Entry(get_upload_settlement).State = EntityState.Modified;
                                settlementDB_Context.SaveChanges();
                            }
                        }
                        if (get_seller_setting != null)
                        {
                            if (current_running_no != 0)
                            {
                                get_seller_setting.current_running_no += 1;
                                dba.Entry(get_seller_setting).State = EntityState.Modified;
                                dba.SaveChanges();
                            }
                        }
                        //}// end of get_upload
                    }// end of if(Condition null)
                }// end of try
                catch (Exception ex)
                {
                    var get_upload_settlement = settlementDB_Context.tbl_settlement_upload.Where(a => a.tbl_seller_id == SellerId && a.Id == upload_tbl_id).FirstOrDefault();
                    if (get_upload_settlement != null)
                    {
                        get_upload_settlement.file_status = "Error";
                        get_upload_settlement.settlement_refernece_no = null;
                        settlementDB_Context.Entry(get_upload_settlement).State = EntityState.Modified;
                        settlementDB_Context.SaveChanges();
                    }
                    Writelog log = new Writelog();
                    log.write_exception_log(upload_tbl_id.ToString(), "CronJobController", "SaveBulksettlementdata inside", DateTime.Now, ex);
                    datasaved = "F" + ex.Message.ToString(); ;
                }// end of catch()
            }
            return datasaved;
        }

        #endregion


        #region Upload Sales Order
        /// <summary>
        /// this is for sales order
        /// </summary>
        /// <returns></returns>
        public void SalesOrderCrown()
        {
            int seller_id = 0;
            try
            {
                var get_details = dba.tbl_order_upload.Where(a => a.type == 1 && a.source == 1 && a.status == "Queued").OrderBy(a => a.id).ToList();
                if (get_details != null)
                {
                    foreach (var item in get_details)
                    {
                        seller_id = Convert.ToInt16(item.tbl_seller_id);
                        string filename = item.filename;
                        int marketplaceID = Convert.ToInt32(item.tbl_Marketplace_id);
                        int id = item.id;

                        string path = System.IO.Path.Combine(Server.MapPath("~/UploadExcel/" + seller_id + "/OrderSales/" + filename));
                        if (marketplaceID == 3)
                        {
                            bulkxml_Parse(path, marketplaceID, id, seller_id);//for amazon 
                        }
                        else if (marketplaceID == 1)
                        {
                            readflipkartorder(path, marketplaceID, id, seller_id);//for flipkart
                        }
                        else if (marketplaceID == 5)
                        {
                            readpaytmorder(path, marketplaceID, id, seller_id);//for flipkart
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Writelog log = new Writelog();
                log.write_exception_log(seller_id.ToString(), "CronJobController", "SalesOrderCrown", DateTime.Now, ex);
            }

            //return View("Index");
        }
        public List<tbl_order_history> historyObj_list = null;
        public List<tbl_sales_order_details> SalesObj_list = null;
        public int bulkxml_Parse(string strFilePath, int marketplaceID, int id, int SellerId)
        {

            int current_running_no = 0;
            int count = 0;
            string startdate = "", enddate = "";
            XmlDocument contentxml = new XmlDocument();

            try
            {
                var get_upload_order_details = dba.tbl_order_upload.Where(aa => aa.tbl_seller_id == SellerId && aa.id == id).FirstOrDefault();


                //if (contentxml.InnerXml != null && contentxml.InnerXml != "")
                //{
                contentxml.Load(strFilePath);
                if (get_upload_order_details != null)
                {
                    get_upload_order_details.status = "Processing";
                    get_upload_order_details.processing_datetime = DateTime.Now;
                    dba.Entry(get_upload_order_details).State = EntityState.Modified;
                    dba.SaveChanges();
                }
                //}
                StringWriter sw = new StringWriter();
                XmlTextWriter xw = new XmlTextWriter(sw);
                contentxml.WriteTo(xw);
                string str = sw.ToString();
                //Load the XML file in XmlDocument.
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(str);
                string a = doc.NamespaceURI;
                //Loop through the selected Nodes.
                XmlNodeList xnList = doc.SelectNodes("/AmazonEnvelope/Message/Order");
                decimal orderbillamount;

                dba.Configuration.AutoDetectChangesEnabled = false;

                historyObj_list = new List<tbl_order_history>();
                SalesObj_list = new List<tbl_sales_order_details>();
                var getdetails = dba.tbl_sales_order_status.Where(aa => aa.is_active == 0).ToList();
                Dictionary<string, int> orderstatus_dict = new Dictionary<string, int>();
                foreach (var item1 in getdetails)
                {
                    string name = item1.sales_order_status.ToLower();
                    orderstatus_dict.Add(name, item1.id);
                }

                int kk = 0;
                //var get_upload_order_details = dba.tbl_order_upload.Where(aa => aa.tbl_seller_id == SellerId && aa.id == id).FirstOrDefault();
                //if (get_upload_order_details != null)
                //{
                //    get_upload_order_details.status = "Processing";
                //    get_upload_order_details.processing_datetime = DateTime.Now;
                //    dba.Entry(get_upload_order_details).State = EntityState.Modified;
                //    dba.SaveChanges();
                //}
                count = xnList.Count;
                if (count > 0)
                {
                    foreach (XmlNode xn in xnList)
                    {



                        if (kk == 0)
                        {
                            startdate = Convert.ToString(xn.SelectSingleNode("PurchaseDate").InnerText == null ? "" : xn["PurchaseDate"].InnerText);
                        }
                        if (kk == xnList.Count - 1)
                        {
                            enddate = Convert.ToString(xn.SelectSingleNode("PurchaseDate").InnerText == null ? "" : xn["PurchaseDate"].InnerText);
                        }


                        kk++;

                        XmlNode anode = xn;
                        if (anode != null)
                        {
                            if (anode["AmazonOrderID"].InnerText == "406-9251467-9993938")//408-1721149-1594725 	
                            {
                            }
                            XmlNode addre = anode.SelectSingleNode("FulfillmentData");
                            if (addre != null)
                            {
                                XmlNode addres = addre.SelectSingleNode("Address");
                                if (addres != null)
                                {
                                    XmlNodeList order = anode.SelectNodes("OrderItem");//get order details 
                                    if (order != null)
                                    {
                                        List<OrderItem> obj1 = new List<OrderItem>();
                                        string abc = "";
                                        string xyz = "";
                                        string itempromotion = "";
                                        string asin = "";
                                        string sku = "";
                                        string itemstatus = "";
                                        string productname = "";
                                        string quantity = "";
                                        orderbillamount = 0; //sharad
                                        foreach (XmlNode aa in order)
                                        {
                                            if (aa != null)
                                            {
                                                if (aa.SelectSingleNode("ASIN") != null)
                                                {
                                                    asin = aa.SelectSingleNode("ASIN").InnerText == null ? "" : aa["ASIN"].InnerText;
                                                }
                                                if (aa.SelectSingleNode("SKU") != null)
                                                {
                                                    sku = aa.SelectSingleNode("SKU").InnerText == null ? "" : aa["SKU"].InnerText;
                                                }
                                                if (aa.SelectSingleNode("ItemStatus") != null)
                                                {
                                                    itemstatus = aa.SelectSingleNode("ItemStatus").InnerText == null ? "" : aa["ItemStatus"].InnerText;
                                                }
                                                if (aa.SelectSingleNode("ProductName") != null)
                                                {
                                                    productname = aa.SelectSingleNode("ProductName").InnerText == null ? "" : aa["ProductName"].InnerText;
                                                }
                                                if (aa.SelectSingleNode("Quantity") != null)
                                                {
                                                    quantity = aa.SelectSingleNode("Quantity").InnerText == null ? "" : aa["Quantity"].InnerText;
                                                }
                                                XmlNode prom = aa.SelectSingleNode("Promotion");//get promotion 
                                                if (prom != null)
                                                {
                                                    if (prom.SelectSingleNode("PromotionIDs") != null)
                                                    {
                                                        abc = prom.SelectSingleNode("PromotionIDs").InnerText == null ? "" : prom["PromotionIDs"].InnerText;
                                                    }
                                                    if (prom.SelectSingleNode("ItemPromotionDiscount") != null)
                                                    {

                                                        itempromotion = prom.SelectSingleNode("ItemPromotionDiscount").InnerText == null ? "" : prom["ItemPromotionDiscount"].InnerText;
                                                    }
                                                    if (prom.SelectSingleNode("ShipPromotionDiscount") != null)
                                                    {

                                                        xyz = prom.SelectSingleNode("ShipPromotionDiscount").InnerText == null ? "" : prom["ShipPromotionDiscount"].InnerText;
                                                    }

                                                }
                                                //////////// ////changes in item price////////////////////////////////////////////
                                                XmlNodeList itempriceslist = aa.SelectNodes("ItemPrice/Component");
                                                List<ItemPrice> objitemprice = new List<ItemPrice>();
                                                if (itempriceslist != null)
                                                {
                                                    string type = "";
                                                    string amount = "";
                                                    string shipamount = "";
                                                    string giftwrap = "";
                                                    string taxamount = "";
                                                    string shippingtax = "";
                                                    foreach (XmlNode pricelist in itempriceslist)
                                                    {
                                                        if (pricelist != null)
                                                        {
                                                            if (pricelist.SelectSingleNode("Type") != null)
                                                            {
                                                                type = pricelist.SelectSingleNode("Type").InnerText == null ? "" : pricelist["Type"].InnerText;
                                                                if (type == "Principal")
                                                                {
                                                                    if (pricelist.SelectSingleNode("Amount") != null)
                                                                    {
                                                                        amount = pricelist.SelectSingleNode("Amount").InnerText == null ? "" : pricelist["Amount"].InnerText;
                                                                    }
                                                                }
                                                                else
                                                                    if (type == "Shipping")
                                                                {
                                                                    if (pricelist.SelectSingleNode("Amount") != null)
                                                                    {
                                                                        shipamount = pricelist.SelectSingleNode("Amount").InnerText == null ? "" : pricelist["Amount"].InnerText;
                                                                    }
                                                                }
                                                                else
                                                                        if (type == "GiftWrap")
                                                                {
                                                                    if (pricelist.SelectSingleNode("Amount") != null)
                                                                    {
                                                                        giftwrap = pricelist.SelectSingleNode("Amount").InnerText == null ? "" : pricelist["Amount"].InnerText;
                                                                    }
                                                                }
                                                                else
                                                                            if (type == "Tax")
                                                                {
                                                                    if (pricelist.SelectSingleNode("Amount") != null)
                                                                    {
                                                                        taxamount = pricelist.SelectSingleNode("Amount").InnerText == null ? "" : pricelist["Amount"].InnerText;
                                                                    }
                                                                }
                                                                else
                                                                                if (type == "ShippingTax")
                                                                {
                                                                    if (pricelist.SelectSingleNode("Amount") != null)
                                                                    {
                                                                        shippingtax = pricelist.SelectSingleNode("Amount").InnerText == null ? "" : pricelist["Amount"].InnerText;
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }//End of For each loop pricelist 
                                                    if (shipamount == "")
                                                        shipamount = "0";

                                                    if (amount == "")
                                                        amount = "0";

                                                    if (giftwrap == "")
                                                        giftwrap = "0";

                                                    if (taxamount == "")
                                                        taxamount = "0";

                                                    if (shippingtax == "")
                                                        shippingtax = "0";
                                                    //if (taxamount == "")
                                                    //taxamount = "0";
                                                    objitemprice.Add(new ItemPrice
                                                    {
                                                        //Type = type,
                                                        pAmonu = Convert.ToDecimal(amount),
                                                        shipAmonu = Convert.ToDecimal(shipamount),
                                                        giftwrapAmonu = Convert.ToDecimal(giftwrap),
                                                        ptaxamount = Convert.ToDecimal(taxamount),
                                                        pshippingtaxamount = Convert.ToDecimal(shippingtax)


                                                    });
                                                    orderbillamount += Convert.ToDecimal(amount) + Convert.ToDecimal(shipamount) + Convert.ToDecimal(giftwrap);
                                                }
                                                /////////////////////////End changes in item price///////////////////////////
                                                obj1.Add(new OrderItem
                                                {
                                                    ASIN = asin,
                                                    SKU = sku,
                                                    ItemStatus = itemstatus,
                                                    ProductName = productname,
                                                    Quantity = quantity,

                                                    PromotionIDs = abc,
                                                    ShipPromotionDiscount = xyz,
                                                    ItemPromotionDiscount = itempromotion,
                                                    itemprice = objitemprice
                                                });
                                            }//End
                                        }
                                        tbl_customer_details obj_custo = new tbl_customer_details();
                                        obj_custo.City = addres.SelectSingleNode("City").InnerText == null ? "" : addres["City"].InnerText;
                                        if (addres.SelectSingleNode("State") != null)
                                        {
                                            obj_custo.State_Region = addres.SelectSingleNode("State").InnerText == null ? "" : addres["State"].InnerText;
                                        }
                                        obj_custo.Country_Code = addres.SelectSingleNode("Country").InnerText == null ? "" : addres["Country"].InnerText;
                                        obj_custo.Postal_Code = addres.SelectSingleNode("PostalCode").InnerText == null ? "" : addres["PostalCode"].InnerText;
                                        obj_custo.tbl_seller_id = SellerId;
                                        dba.tbl_customer_details.Add(obj_custo);
                                        dba.SaveChanges();


                                        tbl_sales_order obj = new tbl_sales_order();
                                        obj.amazon_order_id = (anode.SelectSingleNode("AmazonOrderID") == null) ? "" : anode["AmazonOrderID"].InnerText;
                                        obj.purchase_date = Convert.ToDateTime(anode.SelectSingleNode("PurchaseDate").InnerText == null ? "" : anode["PurchaseDate"].InnerText);
                                        obj.last_updated_date = Convert.ToDateTime(anode.SelectSingleNode("LastUpdatedDate").InnerText == null ? "" : anode["LastUpdatedDate"].InnerText);
                                        obj.order_status = anode.SelectSingleNode("OrderStatus").InnerText == null ? "" : anode["OrderStatus"].InnerText;
                                        obj.sales_channel = anode.SelectSingleNode("SalesChannel").InnerText == null ? "" : anode["SalesChannel"].InnerText;

                                        if (anode.SelectSingleNode("IsBusinessOrder") != null)
                                        {
                                            obj.is_business_order = anode.SelectSingleNode("IsBusinessOrder").InnerText == null ? "" : anode["IsBusinessOrder"].InnerText;
                                        }
                                        obj.fullfillment_channel = addre.SelectSingleNode("FulfillmentChannel").InnerText == null ? "" : addre["FulfillmentChannel"].InnerText;
                                        obj.m_ship_service_level_id = addre.SelectSingleNode("ShipServiceLevel").InnerText == null ? "" : addre["ShipServiceLevel"].InnerText;

                                        obj.ship_city = addres.SelectSingleNode("City").InnerText == null ? "" : addres["City"].InnerText;
                                        if (addres.SelectSingleNode("State") != null)
                                        {
                                            obj.ship_state = addres.SelectSingleNode("State").InnerText == null ? "" : addres["State"].InnerText;
                                        }
                                        obj.ship_postal_code = addres.SelectSingleNode("PostalCode").InnerText == null ? "" : addres["PostalCode"].InnerText;
                                        obj.ship_country = addres.SelectSingleNode("Country").InnerText == null ? "" : addres["Country"].InnerText;

                                        obj.bill_amount = Convert.ToDouble(orderbillamount);
                                        obj.created_on = DateTime.Now;
                                        obj.tbl_sellers_id = SellerId;
                                        obj.is_active = 1;
                                        obj.tbl_Marketplace_Id = marketplaceID;
                                        obj.tbl_order_upload_id = id;
                                        obj.tbl_Customer_Id = obj_custo.id;
                                        obj.LastUpdatedDateUTC = DateTime.UtcNow;
                                        if (obj.fullfillment_channel == "Amazon")
                                        {
                                            obj.n_fullfilled_id = 1;
                                        }
                                        else
                                        {
                                            obj.n_fullfilled_id = 2;
                                        }
                                        string name1 = obj.order_status.ToLower();
                                        if (orderstatus_dict.ContainsKey(name1))
                                        {
                                            obj.n_item_orderstatus = orderstatus_dict[name1];
                                        }

                                        var get_saleorder = dba.tbl_sales_order.Where(aa => aa.tbl_sellers_id == SellerId && aa.amazon_order_id == obj.amazon_order_id && aa.n_item_orderstatus == obj.n_item_orderstatus).FirstOrDefault();
                                        if (get_saleorder == null)
                                        {
                                            dba.tbl_sales_order.Add(obj);
                                            dba.SaveChanges();

                                            if (obj1 != null)
                                            {
                                                //double OrderBillAmount = 0;

                                                foreach (var details in obj1)
                                                {
                                                    var chkSku = dba.tbl_inventory.Where(aa => aa.sku == details.SKU).FirstOrDefault();
                                                    if (chkSku == null)
                                                    {
                                                        tbl_inventory objInventory = new tbl_inventory();
                                                        objInventory.sku = details.SKU;
                                                        objInventory.tbl_sellers_id = SellerId;
                                                        objInventory.asin_no = details.ASIN;
                                                        objInventory.tbl_marketplace_id = marketplaceID;
                                                        objInventory.item_name = details.ProductName;
                                                        objInventory.isactive = 1;
                                                        if (details.itemprice != null)
                                                        {
                                                            foreach (var itemprice in details.itemprice)
                                                            {
                                                                objInventory.selling_price = Convert.ToInt16(itemprice.pAmonu);
                                                            }
                                                        }
                                                        dba.tbl_inventory.Add(objInventory);
                                                        dba.SaveChanges();
                                                    }

                                                    tbl_sales_order_details objsaledetails = new tbl_sales_order_details();
                                                    objsaledetails.asin = details.ASIN;
                                                    objsaledetails.product_name = details.ProductName;
                                                    objsaledetails.sku_no = details.SKU;
                                                    if (details.Quantity != null && details.Quantity != "")
                                                    {
                                                        objsaledetails.quantity_ordered = Convert.ToInt16(details.Quantity);
                                                    }
                                                    else
                                                    {
                                                        objsaledetails.quantity_ordered = 0;
                                                    }

                                                    objsaledetails.promotion_ids = details.PromotionIDs;
                                                    if (details.ShipPromotionDiscount != null && details.ShipPromotionDiscount != "")
                                                    {
                                                        objsaledetails.promotion_amount = Convert.ToDouble(details.ShipPromotionDiscount);
                                                    }
                                                    else
                                                    {
                                                        objsaledetails.promotion_amount = 0;
                                                    }
                                                    if (details.ItemPromotionDiscount != null && details.ItemPromotionDiscount != "")
                                                    {
                                                        objsaledetails.item_promotionAmount = Convert.ToDouble(details.ItemPromotionDiscount);
                                                    }
                                                    else
                                                    {
                                                        objsaledetails.item_promotionAmount = 0;
                                                    }
                                                    if (details.itemprice != null)
                                                    {
                                                        foreach (var itemprice in details.itemprice)
                                                        {
                                                            //if (itemprice.ptaxamount != 0 && itemprice.ptaxamount != null)
                                                            //{
                                                            //    var get =itemprice.pAmonu-itemprice.ptaxamount;
                                                            //    objsaledetails.item_price_amount = Convert.ToDouble(get);
                                                            //}
                                                            //else
                                                            //{
                                                            objsaledetails.item_price_amount = Convert.ToDouble(itemprice.pAmonu);
                                                            //}
                                                            //if (itemprice.pshippingtaxamount != 0 && itemprice.pshippingtaxamount != null)
                                                            //{
                                                            //    var get1 = itemprice.shipAmonu - itemprice.pshippingtaxamount;
                                                            //    objsaledetails.shipping_price_Amount = Convert.ToDouble(get1);
                                                            //}
                                                            objsaledetails.shipping_price_Amount = Convert.ToDouble(itemprice.shipAmonu);
                                                            objsaledetails.giftwrapprice_amount = Convert.ToDouble(itemprice.giftwrapAmonu);

                                                            objsaledetails.item_tax_amount = Convert.ToDouble(itemprice.ptaxamount);
                                                            objsaledetails.shipping_tax_Amount = Convert.ToDouble(itemprice.pshippingtaxamount);
                                                            objsaledetails.item_price_curr_code = "INR";
                                                        }
                                                    }// end of if(details.itemprice)
                                                    objsaledetails.is_active = 1;
                                                    objsaledetails.tbl_seller_id = SellerId;
                                                    objsaledetails.tbl_sales_order_id = obj.id;
                                                    objsaledetails.status_updated_by = SellerId;
                                                    objsaledetails.status_updated_on = DateTime.Now;
                                                    objsaledetails.n_order_status_id = Convert.ToInt16(obj.n_item_orderstatus);
                                                    objsaledetails.amazon_order_id = obj.amazon_order_id;
                                                    objsaledetails.dispatch_bydate = obj.purchase_date;
                                                    objsaledetails.dispatchAfter_date = obj.purchase_date;
                                                    objsaledetails.is_tax_calculated = 0;
                                                    objsaledetails.tax_updatedby_taxfile = 0;
                                                    SalesObj_list.Add(objsaledetails);
                                                    dba.tbl_sales_order_details.Add(objsaledetails);
                                                    dba.SaveChanges();


                                                    #region taxcalculated
                                                    //-------------------------------calculate tax from state wise -----------------------

                                                    double itemtax = 0, cgst_tax = 0, sgst_tax = 0, igst_tax = 0;
                                                    double item_price = 0, shipping_price = 0, shipp_pricewithouttax = 0, itempricewithout_tax = 0, cgst_amount = 0, sgst_amount = 0, igst_amount = 0, shipping_price_tax = 0, item_price_tax = 0;
                                                    string customerstate = "";

                                                    var get_saleorderdetails = dba.tbl_sales_order_details.Where(t => t.tbl_seller_id == SellerId && t.id == objsaledetails.id).FirstOrDefault();
                                                    if (get_saleorderdetails != null)
                                                    {
                                                        var get_sales_orders = dba.tbl_sales_order.Where(t => t.id == get_saleorderdetails.tbl_sales_order_id && t.tbl_sellers_id == SellerId).FirstOrDefault();
                                                        if (get_sales_orders != null)
                                                        {
                                                            if (get_sales_orders.ship_state != null)
                                                            {
                                                                customerstate = get_sales_orders.ship_state.ToLower();
                                                            }
                                                            shipp_pricewithouttax = get_saleorderdetails.shipping_price_Amount - get_saleorderdetails.shipping_tax_Amount;
                                                            itempricewithout_tax = get_saleorderdetails.item_price_amount - get_saleorderdetails.item_tax_amount;
                                                            var seller_details = db.tbl_sellers.Where(t => t.id == SellerId).FirstOrDefault();

                                                            double amount = Convert.ToDouble(get_saleorderdetails.item_price_amount + get_saleorderdetails.shipping_price_Amount + get_saleorderdetails.giftwrapprice_amount);
                                                            double promotionamount = 0;
                                                            if (get_saleorderdetails.promotion_amount != null)
                                                                promotionamount = get_saleorderdetails.promotion_amount;
                                                            if (get_saleorderdetails.item_promotionAmount != null)
                                                                promotionamount += Convert.ToDouble(get_saleorderdetails.item_promotionAmount);

                                                            var totalamount = amount - promotionamount;

                                                            double totaltax = 0;
                                                            if (get_saleorderdetails.item_tax_amount != null)
                                                                totaltax = get_saleorderdetails.item_tax_amount;
                                                            if (get_saleorderdetails.shipping_tax_Amount != null)
                                                                totaltax += get_saleorderdetails.shipping_tax_Amount;

                                                            if (get_saleorderdetails.giftwraptax_amount != null)
                                                                totaltax += Convert.ToDouble(get_saleorderdetails.giftwraptax_amount);

                                                            if (totaltax == null)
                                                                totaltax = 0;

                                                            var finalamt = totalamount - totaltax;

                                                            if (seller_details != null)
                                                            {
                                                                var getcountrydetails = db.tbl_country.Where(t => t.status == 1 && t.countrylevel == 0 && t.id == seller_details.country).FirstOrDefault();// to get country name from country table in admin db.
                                                                var getstatedetails = db.tbl_country.Where(m => m.id == seller_details.state && m.countrylevel == 1 && m.status == 1).FirstOrDefault();//to get state name from country table in admin db.
                                                                string sellerstate = getstatedetails.countryname.ToLower();
                                                                if (sellerstate == customerstate)
                                                                {
                                                                    decimal result = 0;
                                                                    double tax_percent = 0;
                                                                    string value = Convert.ToString((totaltax * 100) / finalamt);
                                                                    if (value != "NaN" && value != "" && value != null)
                                                                    {
                                                                        decimal abcd = Convert.ToDecimal(value);
                                                                        result = decimal.Round(abcd, 2, MidpointRounding.AwayFromZero);
                                                                        tax_percent = Convert.ToDouble(result);
                                                                        item_price_tax = (finalamt * tax_percent) / 100;
                                                                    }

                                                                    item_price = get_saleorderdetails.item_price_amount - item_price_tax;
                                                                    shipping_price_tax = (get_saleorderdetails.shipping_price_Amount * tax_percent) / 100;
                                                                    shipping_price = get_saleorderdetails.shipping_price_Amount - shipping_price_tax;
                                                                    cgst_tax = Convert.ToDouble(result) / 2;
                                                                    sgst_tax = Convert.ToDouble(result) - cgst_tax;

                                                                    //string calculatedted_tax1 = Convert.ToString((finalamt * tax_percent) / 100);
                                                                    string calculatedted_tax1 = Convert.ToString(item_price_tax);
                                                                    decimal abcd_2 = Convert.ToDecimal(calculatedted_tax1);
                                                                    decimal result_2 = decimal.Round(abcd_2, 2, MidpointRounding.AwayFromZero);
                                                                    double cal_tax = Convert.ToDouble(result_2);
                                                                    cgst_amount = totaltax / 2;//(cal_tax) / 2; 
                                                                    sgst_amount = totaltax - cgst_amount; //(cal_tax - cgst_amount);     


                                                                    //---------------save data in salesldeger tax table------------//
                                                                    string cgst_taxrate = "";
                                                                    string sgst_taxrate = "";
                                                                    string input_decimal_number = Convert.ToString(cgst_tax);
                                                                    var regex = new System.Text.RegularExpressions.Regex("(?<=[\\.])[0-9]+");
                                                                    if (regex.IsMatch(input_decimal_number))
                                                                    {
                                                                        string decimal_places = regex.Match(input_decimal_number).Value;
                                                                        cgst_taxrate = decimal_places;
                                                                    }
                                                                    else
                                                                    {
                                                                        cgst_taxrate = input_decimal_number;
                                                                    }
                                                                    string input_decimal_number1 = Convert.ToString(sgst_tax);
                                                                    var regex1 = new System.Text.RegularExpressions.Regex("(?<=[\\.])[0-9]+");
                                                                    if (regex1.IsMatch(input_decimal_number1))
                                                                    {
                                                                        string decimal_places1 = regex1.Match(input_decimal_number1).Value;
                                                                        sgst_taxrate = decimal_places1;
                                                                    }
                                                                    else
                                                                    {
                                                                        sgst_taxrate = input_decimal_number1;
                                                                    }

                                                                    string Taxname = "CGST@" + cgst_taxrate + "%";
                                                                    string taxname = "SGST@" + sgst_taxrate + "%";
                                                                    if (cgst_taxrate != "" && cgst_taxrate != null && cgst_taxrate != "0")
                                                                    {
                                                                        var gettaxdetails = dba.tbl_Salesledger_tax.Where(t => t.tax_name == Taxname).FirstOrDefault();
                                                                        if (gettaxdetails == null)
                                                                        {
                                                                            try
                                                                            {
                                                                                tbl_Salesledger_tax obj_salestax = new tbl_Salesledger_tax();
                                                                                obj_salestax.tax_name = Taxname;
                                                                                obj_salestax.tax_percentage = Convert.ToDouble(cgst_taxrate);
                                                                                dba.tbl_Salesledger_tax.Add(obj_salestax);
                                                                                dba.SaveChanges();
                                                                            }
                                                                            catch (Exception ex)
                                                                            {
                                                                            }
                                                                        }
                                                                    }
                                                                    if (sgst_taxrate != "" && sgst_taxrate != null && sgst_taxrate != "0")
                                                                    {
                                                                        var gettaxdetail = dba.tbl_Salesledger_tax.Where(t => t.tax_name == taxname).FirstOrDefault();
                                                                        if (gettaxdetail == null)
                                                                        {
                                                                            try
                                                                            {
                                                                                tbl_Salesledger_tax obj_salestax = new tbl_Salesledger_tax();
                                                                                obj_salestax.tax_name = taxname;
                                                                                obj_salestax.tax_percentage = Convert.ToDouble(sgst_taxrate);
                                                                                dba.tbl_Salesledger_tax.Add(obj_salestax);
                                                                                dba.SaveChanges();
                                                                            }
                                                                            catch (Exception ex)
                                                                            {
                                                                            }
                                                                        }
                                                                    }
                                                                    //-------------------------------end---------------------------/
                                                                }
                                                                else
                                                                {
                                                                    string value1 = Convert.ToString((totaltax * 100) / finalamt);
                                                                    if (value1 != "NaN" && value1 != "" && value1 != null)
                                                                    {
                                                                        decimal abcd1 = Convert.ToDecimal(value1);
                                                                        decimal result1 = decimal.Round(abcd1, 2, MidpointRounding.AwayFromZero);
                                                                        igst_tax = Convert.ToDouble(result1);
                                                                    }

                                                                    //item_price_tax =Convert.ToDouble((finalamt * igst_tax) / 100);
                                                                    decimal price_tax = Convert.ToDecimal((finalamt * igst_tax) / 100);
                                                                    decimal result3 = decimal.Round(price_tax, 2, MidpointRounding.AwayFromZero);
                                                                    item_price_tax = Convert.ToDouble(result3);
                                                                    item_price = get_saleorderdetails.item_price_amount - item_price_tax;
                                                                    shipping_price_tax = (get_saleorderdetails.shipping_price_Amount * igst_tax) / 100;
                                                                    shipping_price = get_saleorderdetails.shipping_price_Amount - shipping_price_tax;
                                                                    //string calculatedted_tax = Convert.ToString((item_price * igst_tax) / 100);
                                                                    //decimal abcd2 = Convert.ToDecimal(calculatedted_tax);
                                                                    //decimal result2 = decimal.Round(abcd2, 2, MidpointRounding.AwayFromZero);
                                                                    igst_amount = totaltax;// item_price_tax;

                                                                    string igst_taxrate = "";
                                                                    string input_decimal_number4 = Convert.ToString(igst_tax);
                                                                    var regex4 = new System.Text.RegularExpressions.Regex("(?<=[\\.])[0-9]+");
                                                                    if (regex4.IsMatch(input_decimal_number4))
                                                                    {
                                                                        string decimal_places4 = regex4.Match(input_decimal_number4).Value;
                                                                        igst_taxrate = decimal_places4;
                                                                    }
                                                                    else
                                                                    {
                                                                        igst_taxrate = input_decimal_number4;
                                                                    }
                                                                    if (igst_taxrate != "" && igst_taxrate != null && igst_taxrate != "0")
                                                                    {
                                                                        string igstTaxname = "IGST@" + igst_taxrate + "%";
                                                                        var gettaxdetails = dba.tbl_Salesledger_tax.Where(t => t.tax_name == igstTaxname).FirstOrDefault();
                                                                        if (gettaxdetails == null)
                                                                        {
                                                                            try
                                                                            {
                                                                                tbl_Salesledger_tax obj_salestax = new tbl_Salesledger_tax();
                                                                                obj_salestax.tax_name = igstTaxname;
                                                                                obj_salestax.tax_percentage = Convert.ToDouble(igst_taxrate);
                                                                                dba.tbl_Salesledger_tax.Add(obj_salestax);
                                                                                dba.SaveChanges();
                                                                            }
                                                                            catch (Exception ex)
                                                                            {
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                                var get_taxdetails = dba.tbl_tax.Where(t => t.tbl_referneced_id == objsaledetails.id && t.reference_type == 3).FirstOrDefault();
                                                                if (get_taxdetails == null)
                                                                {
                                                                    if (igst_tax != 0 || cgst_tax != 0.0 || sgst_tax != 0.0)
                                                                    {
                                                                        tbl_tax objtax = new tbl_tax();
                                                                        objtax.tbl_seller_id = SellerId;
                                                                        objtax.tbl_referneced_id = objsaledetails.id;
                                                                        objtax.reference_type = 3;
                                                                        objtax.isactive = 1;
                                                                        if (cgst_tax != null && cgst_tax != 0.0 && sgst_tax != null && sgst_tax != 0.0)
                                                                        {
                                                                            objtax.cgst_tax = cgst_tax;
                                                                            objtax.sgst_tax = sgst_tax;
                                                                            objtax.CGST_amount = cgst_amount;
                                                                            objtax.sgst_amount = sgst_amount;
                                                                        }
                                                                        else
                                                                        {
                                                                            objtax.igst_tax = igst_tax;
                                                                            objtax.Igst_amount = igst_amount;
                                                                        }
                                                                        dba.tbl_tax.Add(objtax);
                                                                        dba.SaveChanges();
                                                                    }

                                                                }// end of if(get_taxdetails)

                                                                //else
                                                                //{
                                                                //    if (cgst_tax != null && cgst_tax != 0.0 && sgst_tax != null && sgst_tax != 0.0)
                                                                //    {
                                                                //        get_taxdetails.cgst_tax = cgst_tax;
                                                                //        get_taxdetails.sgst_tax = sgst_tax;
                                                                //        get_taxdetails.CGST_amount = cgst_amount;
                                                                //        get_taxdetails.sgst_amount = sgst_amount;
                                                                //    }
                                                                //    else
                                                                //    {
                                                                //        get_taxdetails.igst_tax = igst_tax;
                                                                //        get_taxdetails.Igst_amount = igst_amount;
                                                                //    }
                                                                //    dba.Entry(get_taxdetails).State = EntityState.Modified;
                                                                //    dba.SaveChanges();
                                                                //}
                                                            }// end of if(seller_details)

                                                        }// end of get_sales_orders

                                                        get_saleorderdetails.item_price_amount = itempricewithout_tax;
                                                        get_saleorderdetails.shipping_price_Amount = shipp_pricewithouttax;
                                                        dba.Entry(get_saleorderdetails).State = EntityState.Modified;
                                                        dba.SaveChanges();
                                                    }// end of if get_saleorderdetails

                                                    //-----------------------------------------End-----------------------------------------
                                                    #endregion
                                                }// end of foreach loop(details)
                                            }// end of if(item.OrderDetails)
                                        }// end of if(get_saleorder)                               
                                    }
                                }
                            }
                        }// end of if(anode!=null)
                        else //if (anode != null)
                        {
                            int jjj = 0;
                            jjj++;
                        }
                        // saveXMLData(customers);
                    }// end of foreach loop xnList

                    //int i = 0;
                    //foreach (var detailObj in SalesObj_list)
                    //{
                    //    i++;
                    //    dba.tbl_sales_order_details.Add(detailObj);
                    //    if (i == 100)
                    //    {
                    //        i = 0;
                    //        dba.SaveChanges();
                    //    }
                    //}



                }// end of count


                string connectionstring = ConfigurationManager.ConnectionStrings["SellerConnection"].ConnectionString;
                MySqlConnection con = new MySqlConnection(connectionstring);

                int uniqueupload = 0;
                int total_order = 0;
                int diff_order = 0;
                var get_unique_count_details = "SELECT count(DISTINCT aa.id) FROM(SELECT DISTINCT Order_Id AS id FROM `tbl_settlement_order` where tbl_seller_id = " + SellerId + " UNION ALL SELECT DISTINCT OrderID AS id FROM `tbl_order_history` where tbl_seller_id =" + SellerId + " UNION ALL SELECT DISTINCT amazon_order_id AS id FROM `tbl_sales_order` where tbl_sellers_id =" + SellerId + " ) AS aa";
                //var get_unique_count_details = "SELECT count(DISTINCT aa.id) FROM(SELECT DISTINCT Order_Id AS id FROM `tbl_settlement_order` where tbl_seller_id = " + SellerId + " UNION ALL SELECT DISTINCT amazon_order_id AS id FROM `tbl_sales_order` where tbl_sellers_id =" + SellerId + " ) AS aa";
                MySqlCommand cmd = new MySqlCommand(get_unique_count_details, con);
                con.Open();
                MySqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    uniqueupload = Convert.ToInt32(dr[0]);
                }
                cmd.Dispose();
                con.Close();

                MySqlConnection con1 = new MySqlConnection(connectionstring);
                var get_unique_orderby_id = "SELECT count( aa.id )FROM (SELECT tbl_order_upload_id AS id FROM `tbl_sales_order` where tbl_sellers_id =" + SellerId + " && tbl_order_upload_id = " + id + ") AS aa";
                MySqlCommand cmd2 = new MySqlCommand(get_unique_orderby_id, con1);
                con1.Open();
                MySqlDataReader dr1 = cmd2.ExecuteReader();
                if (dr1.HasRows)
                {
                    dr1.Read();
                    total_order = Convert.ToInt32(dr1[0]);
                }
                cmd2.Dispose();
                con1.Close();

                var get_balance_details = db.tbl_user_login.Where(aa => aa.tbl_sellers_id == SellerId).FirstOrDefault();
                if (get_balance_details != null)
                {
                    var last_order = get_balance_details.total_orders;
                    diff_order = Convert.ToInt16(uniqueupload - last_order);
                    //diff_order = Convert.ToInt16(uniqueupload);
                    var plan_rate = get_balance_details.applied_plan_rate * diff_order;
                    get_balance_details.wallet_balance = get_balance_details.wallet_balance - Convert.ToDouble(plan_rate);
                    int totalOrder = Convert.ToInt16(diff_order);
                    if (get_balance_details.total_orders != null)
                        totalOrder = totalOrder + Convert.ToInt16(get_balance_details.total_orders);
                    get_balance_details.total_orders = totalOrder;
                    db.Entry(get_balance_details).State = EntityState.Modified;
                    db.SaveChangesAsync();
                    // Session["WalletBalance"] = get_balance_details.wallet_balance.ToString();
                    //Session["TotalOrders"] = totalOrder.ToString();
                }
                //var get_upload_settlement_details = dba.tbl_order_upload.Where(aa => aa.tbl_seller_id == SellerId && aa.id == id).FirstOrDefault();
                if (get_upload_order_details != null)
                {
                    get_upload_order_details.new_order_uploaded = total_order;
                    if (enddate != null && enddate != "")
                    {
                        get_upload_order_details.from_date = Convert.ToDateTime(enddate);
                    }
                    if (startdate != null && startdate != "")
                    {
                        get_upload_order_details.to_date = Convert.ToDateTime(startdate);
                    }
                    if (count > 0)
                    {
                        get_upload_order_details.status = "Completed";
                    }
                    else
                    {
                        get_upload_order_details.status = "There is no Order";
                    }
                    get_upload_order_details.completed_datetime = DateTime.Now;
                    dba.Entry(get_upload_order_details).State = EntityState.Modified;

                }
                dba.SaveChangesAsync();
                //}// end of content XML
                //else
                //{
                //get_upload_order_details.status = "File Format is not Correct";
                //get_upload_order_details.completed_datetime = DateTime.Now;
                //dba.Entry(get_upload_order_details).State = EntityState.Modified;
                //dba.SaveChangesAsync();
                //}
            }// end of try
            catch (Exception ex)
            {
                var get_upload_orderdetails = dba.tbl_order_upload.Where(aa => aa.tbl_seller_id == SellerId && aa.id == id).FirstOrDefault();
                get_upload_orderdetails.status = "File Format is not Correct or Some Error Occurred";
                get_upload_orderdetails.completed_datetime = DateTime.Now;
                dba.Entry(get_upload_orderdetails).State = EntityState.Modified;
                dba.SaveChangesAsync();
                Writelog log = new Writelog();
                log.write_exception_log(SellerId.ToString(), "CronJobController", "bulkxml_Parse", DateTime.Now, ex);
                throw ex;
            }

            return current_running_no;
        }


        #region For Flipkart

        public string readflipkartorder(string strFilePath, int marketplaceID, int id, int SellerId)
        {
            string success = "S";
            try
            {
                int i = 0;
                List<Flipkartcsv> lstFile = new List<Flipkartcsv>();
                Flipkartcsv obj = new Flipkartcsv();

                using (var reader = new StreamReader(strFilePath))
                {
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        if (!string.IsNullOrEmpty(line))
                        {
                            if (i > 0)
                            {
                                i++;
                                var values = line;

                                /*int ind = line.IndexOf("\"", 2);
                                if (ind == -1)
                                {
                                    var line1 = reader.ReadLine();
                                    line = line + line1;
                                }*/
                                //List<string> result = new List<string>();

                                //var splitted = values.Split('"').ToList<string>();
                                //splitted.RemoveAll(x => x == ",");
                                //foreach (var it in splitted)
                                //{
                                //    if (it.StartsWith(",") || it.EndsWith(","))
                                //    {
                                //        var tmp = it.TrimEnd(',').TrimStart(',');
                                //        result.AddRange(tmp.Split(','));
                                //    }
                                //    else
                                //    {
                                //        if (!string.IsNullOrEmpty(it)) result.Add(it);
                                //    }
                                //}
                                //Results:                           

                                MatchCollection matches = new Regex("((?<=\")[^\"]*(?=\"(,|$)+)|(?<=,|^)[^,\"]*(?=,|$))").Matches(values);

                                if (matches.Count < 36)
                                {
                                    int cnt = matches.Count;
                                    do
                                    {
                                        var line1 = reader.ReadLine();
                                        line = line + line1;
                                        matches = new Regex("((?<=\")[^\"]*(?=\"(,|$)+)|(?<=,|^)[^,\"]*(?=,|$))").Matches(line);
                                        cnt = matches.Count;
                                    } while (cnt < 36);

                                }
                                // var matches = result;

                                obj = new Flipkartcsv();

                                //char Split_Char = '-';
                                //string q = matches[0].ToString().Replace(",","");
                                //DateTime dd = Convert.ToDateTime(q);

                                //string[] strDateTime = matches[0].ToString().Split(Split_Char);
                                //string a1 = strDateTime[0];
                                //string b1 = strDateTime[1];
                                //string c1 = strDateTime[2];
                                //string newdate = c1 + "-" + b1 + "-" + a1;

                                obj.OrderedOn = matches[0].ToString();//newdate;
                                obj.ShipmentID = matches[1].ToString();
                                obj.ORDERITEMID = matches[2].ToString().Replace("'", "");
                                obj.OrderId = matches[3].ToString();
                                obj.HSNCODE = matches[4].ToString();
                                obj.OrderState = matches[5].ToString();
                                obj.OrderType = matches[6].ToString();
                                obj.FSN = matches[7].ToString();
                                obj.SKU = matches[8].ToString();
                                obj.Product = matches[9].ToString();
                                obj.InvoiceNo = matches[10].ToString();
                                obj.CGST = matches[11].ToString();
                                obj.IGST = matches[12].ToString();
                                obj.SGST = matches[13].ToString();
                                string str = matches[14].ToString();
                                obj.InvoiceDate = str;
                                obj.InvoiceAmount = matches[15].ToString();

                                //----------------- calculate tax ----------------//

                                if (obj.IGST != null && obj.IGST != "" && obj.IGST != "NA")
                                {
                                    double taxrate = 0, itemprice = 0, actualitemprice = 0, item_taxprice = 0, shippingprice = 0, actualshippingprice = 0, shippingtaxprice = 0, totaltax = 0, shippingtax = 0;
                                    decimal result = 0, result1 = 0;

                                    double taxrate1 = Convert.ToDouble(obj.IGST);
                                    taxrate = 100 + taxrate1;

                                    itemprice = Convert.ToDouble(matches[16].ToString());
                                    shippingprice = Convert.ToDouble(matches[17].ToString());

                                    string valueitem = Convert.ToString((itemprice * 100) / taxrate);
                                    string valueshipping = Convert.ToString((shippingprice * 100) / taxrate);

                                    if (valueitem != "NaN" && valueitem != "" && valueitem != null)
                                    {
                                        decimal abcd = Convert.ToDecimal(valueitem);
                                        result = decimal.Round(abcd, 2, MidpointRounding.AwayFromZero);
                                        actualitemprice = Convert.ToDouble(result);
                                        obj.item_price = actualitemprice;

                                        double itemtax = itemprice - obj.item_price;
                                        decimal ab = Convert.ToDecimal(itemtax);
                                        decimal res = decimal.Round(ab, 2, MidpointRounding.AwayFromZero);
                                        item_taxprice = Convert.ToDouble(res);

                                        obj.itemtax_price = item_taxprice;
                                    }

                                    if (valueshipping != "NaN" && valueshipping != "" && valueshipping != null)
                                    {
                                        decimal abcd1 = Convert.ToDecimal(valueshipping);
                                        result1 = decimal.Round(abcd1, 2, MidpointRounding.AwayFromZero);
                                        actualshippingprice = Convert.ToDouble(result1);

                                        obj.shipping_price = actualshippingprice;

                                        double shipping_tax = shippingprice - actualshippingprice;
                                        decimal abc = Convert.ToDecimal(shipping_tax);
                                        decimal resq = decimal.Round(abc, 2, MidpointRounding.AwayFromZero);
                                        shippingtaxprice = Convert.ToDouble(resq);

                                        obj.shipping_taxprice = shippingtaxprice;
                                    }
                                    double stax = item_taxprice + shippingtaxprice;
                                    decimal acc = Convert.ToDecimal(stax);
                                    decimal rsq = decimal.Round(acc, 2, MidpointRounding.AwayFromZero);
                                    totaltax = Convert.ToDouble(rsq);

                                    obj.Igsttaxprice = totaltax;
                                }
                                else
                                {
                                    double tax_rate = 0, itemprices = 0, actualitemprices = 0, item_taxprices = 0, shippingprices = 0, actualshippingprices = 0, shippingtaxprices = 0, totaltaxs = 0, shippingtaxs = 0;
                                    decimal results = 0, result1s = 0;
                                    double cgstrate = Convert.ToDouble(obj.CGST);
                                    double sgstrate = Convert.ToDouble(obj.SGST);

                                    tax_rate = 100 + (cgstrate + sgstrate);

                                    itemprices = Convert.ToDouble(matches[16].ToString());
                                    shippingprices = Convert.ToDouble(matches[17].ToString());

                                    string valueitem = Convert.ToString((itemprices * 100) / tax_rate);
                                    string valueshipping = Convert.ToString((shippingprices * 100) / tax_rate);

                                    if (valueitem != "NaN" && valueitem != "" && valueitem != null)
                                    {
                                        decimal abcd = Convert.ToDecimal(valueitem);
                                        results = decimal.Round(abcd, 2, MidpointRounding.AwayFromZero);
                                        actualitemprices = Convert.ToDouble(results);
                                        obj.item_price = actualitemprices;

                                        double itemtax = itemprices - obj.item_price;
                                        decimal ab = Convert.ToDecimal(itemtax);
                                        decimal res = decimal.Round(ab, 2, MidpointRounding.AwayFromZero);
                                        item_taxprices = Convert.ToDouble(res);

                                        obj.itemtax_price = item_taxprices;
                                    }

                                    if (valueshipping != "NaN" && valueshipping != "" && valueshipping != null)
                                    {
                                        decimal abcd1 = Convert.ToDecimal(valueshipping);
                                        result1s = decimal.Round(abcd1, 2, MidpointRounding.AwayFromZero);
                                        actualshippingprices = Convert.ToDouble(result1s);

                                        obj.shipping_price = actualshippingprices;

                                        double shipping_tax = shippingprices - actualshippingprices;
                                        decimal abc = Convert.ToDecimal(shipping_tax);
                                        decimal resq = decimal.Round(abc, 2, MidpointRounding.AwayFromZero);
                                        shippingtaxprices = Convert.ToDouble(resq);

                                        obj.shipping_taxprice = shippingtaxprices;
                                    }
                                    double stax = item_taxprices + shippingtaxprices;
                                    decimal acc = Convert.ToDecimal(stax);
                                    decimal rsq = decimal.Round(acc, 2, MidpointRounding.AwayFromZero);
                                    totaltaxs = Convert.ToDouble(rsq);

                                    double cgst = totaltaxs / 2;
                                    obj.cgsttaxprice = cgst;
                                    obj.sgsttaxprice = totaltaxs - cgst;
                                }
                                //---------------------END-----------------------//
                                obj.SellingPricePerItem = matches[16].ToString();
                                obj.ShippingChargeperitem = matches[17].ToString();
                                obj.Quantity = matches[18].ToString();
                                obj.Price_inc = matches[19].ToString();
                                obj.Buyername = matches[20].ToString();
                                obj.Shiptoname = matches[21].ToString();

                                if (matches[22].ToString() != null && matches[22].ToString() != "")
                                {
                                    obj.AddressLine1 = matches[22].ToString();
                                }
                                if (matches[23].ToString() != "" && matches[23].ToString() != null)
                                {
                                    obj.AddressLine2 = matches[23].ToString();
                                }
                                if (matches[24].ToString() != null && matches[24].ToString() != "")
                                {
                                    obj.City = matches[24].ToString();
                                }
                                if (matches[25].ToString() != null && matches[25].ToString() != "")
                                {
                                    obj.State = matches[25].ToString();
                                }
                                if (matches[26].ToString() != null && matches[26].ToString() != "")
                                {
                                    obj.PINCode = matches[26].ToString();
                                }
                                if (matches[27].ToString() != null && matches[27].ToString() != "")
                                {
                                    obj.ProcSLA = matches[27].ToString();
                                }
                                if (matches[28].ToString() != null && matches[28].ToString() != "")
                                {
                                    str = matches[28].ToString();
                                    obj.DispatchAfterdate = str;
                                }
                                if (matches[29].ToString() != null && matches[29].ToString() != "")
                                {
                                    str = matches[29].ToString();
                                    obj.Dispatchbydate = str;
                                }
                                if (matches[31].ToString() != null && matches[31].ToString() != "")
                                {
                                    obj.TrackingID = matches[31].ToString();
                                }
                                if (matches[32].ToString() != null && matches[32].ToString() != "")
                                {
                                    obj.PackageLength = matches[32].ToString();
                                }
                                if (matches[33].ToString() != null && matches[33].ToString() != "")
                                {
                                    obj.PackageBreadth = matches[33].ToString();
                                }
                                if (matches[34].ToString() != null && matches[34].ToString() != "")
                                {
                                    obj.PackageHeight = matches[34].ToString();
                                }
                                if (matches[35].ToString() != null && matches[35].ToString() != "")
                                {
                                    obj.PackageWeight = matches[35].ToString();
                                }
                                lstFile.Add(obj);
                            }
                            i++;
                        }

                    }
                }
                if (lstFile.Count > 0)
                {
                    success = SaveFlipkartOrder(lstFile, marketplaceID, id, SellerId);
                }
                else
                {
                    success = "Em";
                }
            }
            catch (Exception ex)
            {
            }
            return success;
        }

        public string SaveFlipkartOrder(List<Flipkartcsv> lstFile, int marketplaceID, int id, int SellerId)
        {
            string success = "S";
            try
            {
                int count = 0;
                int kk = 0;
                string startdate = "", enddate = "";
                if (lstFile != null)
                {

                    var get_upload_order_details = dba.tbl_order_upload.Where(aa => aa.tbl_seller_id == SellerId && aa.id == id && aa.tbl_Marketplace_id == marketplaceID).FirstOrDefault();
                    if (get_upload_order_details != null)
                    {
                        get_upload_order_details.status = "Processing";
                        get_upload_order_details.processing_datetime = DateTime.Now;
                        dba.Entry(get_upload_order_details).State = EntityState.Modified;
                        dba.SaveChanges();
                    }
                    foreach (var item in lstFile)
                    {

                        if (kk == 0)
                        {
                            startdate = item.OrderedOn;
                        }
                        if (kk == lstFile.Count - 1)
                        {
                            enddate = item.OrderedOn;
                        }


                        kk++;

                        var get_saleorder = dba.tbl_sales_order.Where(aa => aa.tbl_sellers_id == SellerId && aa.amazon_order_id == item.OrderId && aa.order_status == item.OrderState).FirstOrDefault();
                        if (get_saleorder == null)
                        {
                            double item_shipping_price = 0;
                            double itemprice = Convert.ToDouble(item.SellingPricePerItem);
                            if (item.ShippingChargeperitem != null && item.ShippingChargeperitem != "")
                            {
                                item_shipping_price = Convert.ToDouble(item.ShippingChargeperitem);
                            }

                            tbl_customer_details objcustumer = new tbl_customer_details();
                            objcustumer.State_Region = item.State;
                            objcustumer.shipping_Buyer_Name = item.Shiptoname;
                            objcustumer.City = item.City;
                            objcustumer.Country_Code = "IN";
                            objcustumer.Postal_Code = item.PINCode;
                            objcustumer.Address_1 = item.AddressLine1;
                            objcustumer.Address_2 = item.AddressLine2;
                            objcustumer.tbl_seller_id = SellerId;
                            dba.tbl_customer_details.Add(objcustumer);
                            dba.SaveChanges();

                            tbl_sales_order objsales = new tbl_sales_order();
                            objsales.amazon_order_id = item.OrderId;
                            objsales.buyer_name = item.Buyername;
                            objsales.tbl_Customer_Id = objcustumer.id;




                            //var date = DateTime.Parse(item.OrderedOn, new CultureInfo("en-US", true));

                            objsales.purchase_date = Convert.ToDateTime(item.OrderedOn);
                            objsales.order_status = item.OrderState;
                            objsales.bill_amount = (itemprice + item_shipping_price);//Convert.ToDouble(item.SellingPricePerItem);
                            objsales.tbl_Marketplace_Id = marketplaceID;
                            objsales.order_item_id = item.ORDERITEMID;
                            objsales.created_on = DateTime.Now;
                            objsales.is_active = 1;
                            objsales.tbl_sellers_id = SellerId;
                            objsales.tbl_order_upload_id = id;
                            objsales.last_updated_date = Convert.ToDateTime(item.Dispatchbydate);
                            objsales.earliest_ship_date = Convert.ToDateTime(item.DispatchAfterdate);
                            objsales.fullfillment_channel = "Flipkart";
                            dba.tbl_sales_order.Add(objsales);
                            dba.SaveChanges();

                            var chkSku = dba.tbl_inventory.Where(a => a.sku == item.SKU).FirstOrDefault();
                            if (chkSku == null)
                            {
                                tbl_inventory objInventory = new tbl_inventory();
                                objInventory.sku = item.SKU;
                                objInventory.tbl_sellers_id = SellerId;
                                objInventory.item_name = item.Product;
                                objInventory.selling_price = Convert.ToInt16(item.SellingPricePerItem);
                                objInventory.isactive = 1;
                                objInventory.hsn_code = item.HSNCODE;
                                objInventory.tbl_marketplace_id = marketplaceID;
                                dba.tbl_inventory.Add(objInventory);
                                dba.SaveChanges();
                            }

                            tbl_sales_order_details objsaledetails = new tbl_sales_order_details();

                            objsaledetails.quantity_ordered = Convert.ToInt32(item.Quantity);
                            objsaledetails.product_name = item.Product;
                            objsaledetails.hsn = item.HSNCODE;
                            objsaledetails.sku_no = item.SKU;
                            objsaledetails.order_item_id = item.ORDERITEMID;
                            objsaledetails.sla_flipkart = Convert.ToInt16(item.ProcSLA);
                            objsaledetails.tax_invoiceno = item.InvoiceNo;
                            objsaledetails.shipment_Id = item.ShipmentID;

                            objsaledetails.item_price_amount = item.item_price;
                            objsaledetails.item_tax_amount = item.itemtax_price;
                            objsaledetails.shipping_price_Amount = item.shipping_price;
                            objsaledetails.shipping_tax_Amount = item.shipping_taxprice;
                            objsaledetails.is_active = 1;
                            objsaledetails.tbl_seller_id = SellerId;
                            objsaledetails.tbl_sales_order_id = objsales.id;
                            objsaledetails.status_updated_by = SellerId;
                            objsaledetails.status_updated_on = DateTime.Now;
                            objsaledetails.n_order_status_id = 1;
                            objsaledetails.amazon_order_id = objsales.amazon_order_id;
                            objsaledetails.dispatch_bydate = Convert.ToDateTime(item.Dispatchbydate);
                            objsaledetails.dispatchAfter_date = Convert.ToDateTime(item.DispatchAfterdate);
                            objsaledetails.is_tax_calculated = 0;
                            objsaledetails.tax_updatedby_taxfile = 0;
                            dba.tbl_sales_order_details.Add(objsaledetails);
                            dba.SaveChanges();


                            tbl_tax objtax = new tbl_tax();
                            objtax.tbl_seller_id = SellerId;
                            objtax.tbl_referneced_id = objsaledetails.id;
                            objtax.reference_type = 3;
                            objtax.isactive = 1;

                            if (item.SGST != "NA" && item.CGST != "NA" && item.SGST != "" && item.SGST != null)
                            {
                                objtax.cgst_tax = Convert.ToDouble(item.CGST);
                                objtax.sgst_tax = Convert.ToDouble(item.SGST);
                                objtax.CGST_amount = item.cgsttaxprice;
                                objtax.sgst_amount = item.sgsttaxprice;
                            }
                            else
                            {
                                objtax.igst_tax = Convert.ToDouble(item.IGST);
                                objtax.Igst_amount = item.Igsttaxprice;
                            }
                            dba.tbl_tax.Add(objtax);
                            dba.SaveChanges();
                        }
                    }// end of for eachloop main

                    string connectionstring = ConfigurationManager.ConnectionStrings["SellerConnection"].ConnectionString;
                    MySqlConnection con = new MySqlConnection(connectionstring);

                    int uniqueupload = 0;
                    int diff_order = 0;
                    int total_order = 0;
                    var get_unique_count_details = "SELECT count(DISTINCT aa.id) FROM(SELECT DISTINCT Order_Id AS id FROM `tbl_settlement_order` where tbl_seller_id = " + SellerId + " UNION ALL SELECT DISTINCT amazon_order_id AS id FROM `tbl_sales_order` where tbl_sellers_id =" + SellerId + " ) AS aa";
                    MySqlCommand cmd = new MySqlCommand(get_unique_count_details, con);
                    con.Open();
                    MySqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        dr.Read();
                        uniqueupload = Convert.ToInt32(dr[0]);
                    }
                    cmd.Dispose();
                    con.Close();

                    MySqlConnection con1 = new MySqlConnection(connectionstring);
                    var get_unique_orderby_id = "SELECT count( aa.id )FROM (SELECT tbl_order_upload_id AS id FROM `tbl_sales_order` where tbl_sellers_id =" + SellerId + " && tbl_order_upload_id = " + id + ") AS aa";
                    MySqlCommand cmd2 = new MySqlCommand(get_unique_orderby_id, con1);
                    con1.Open();
                    MySqlDataReader dr1 = cmd2.ExecuteReader();
                    if (dr1.HasRows)
                    {
                        dr1.Read();
                        total_order = Convert.ToInt32(dr1[0]);
                    }
                    cmd2.Dispose();
                    con1.Close();

                    var get_balance_details = db.tbl_user_login.Where(aa => aa.tbl_sellers_id == SellerId).FirstOrDefault();
                    if (get_balance_details != null)
                    {
                        var last_order = get_balance_details.total_orders;
                        diff_order = Convert.ToInt16(uniqueupload - last_order);
                        var plan_rate = get_balance_details.applied_plan_rate * diff_order;
                        get_balance_details.wallet_balance = get_balance_details.wallet_balance - Convert.ToDouble(plan_rate);
                        int totalOrder = Convert.ToInt16(diff_order);
                        if (get_balance_details.total_orders != null)
                            totalOrder = totalOrder + Convert.ToInt16(get_balance_details.total_orders);
                        get_balance_details.total_orders = totalOrder;
                        db.Entry(get_balance_details).State = EntityState.Modified;
                        db.SaveChangesAsync();
                    }
                    if (get_upload_order_details != null)
                    {
                        get_upload_order_details.new_order_uploaded = total_order;
                        get_upload_order_details.from_date = Convert.ToDateTime(startdate);
                        get_upload_order_details.to_date = Convert.ToDateTime(enddate);
                        get_upload_order_details.status = "Completed";
                        get_upload_order_details.completed_datetime = DateTime.Now;
                        dba.Entry(get_upload_order_details).State = EntityState.Modified;
                    }
                    dba.SaveChangesAsync();
                }// end of if lstfile
            }
            catch (Exception ex)
            {
                success = "F";
                var get_upload_orderdetails = dba.tbl_order_upload.Where(aa => aa.tbl_seller_id == SellerId && aa.id == id).FirstOrDefault();
                get_upload_orderdetails.status = "File Format is not Correct or Some Error Occurred";
                get_upload_orderdetails.completed_datetime = DateTime.Now;
                dba.Entry(get_upload_orderdetails).State = EntityState.Modified;
                dba.SaveChangesAsync();
                Writelog log = new Writelog();
                log.write_exception_log(SellerId.ToString(), "CronJobController", "Flipkart_csvfile", DateTime.Now, ex);
                throw ex;
            }
            return success;
        }

        #endregion


        #region For Paytm
        private const char Delimiter = ',';
        private const int HeaderIndex = 0, HeaderRowCount = HeaderIndex + 1;

        public DataTable readpaytmorder(string strFilePath, int marketplaceID, int id, int SellerId)
        {
            string success = "S";
            DataTable dtCsv = new DataTable();
            try
            {
                dtCsv = new DataTable();
                string Fulltext;
                //if (FileUpload1.HasFile)
                //{
                // string FileSaveWithPath = strFilePath; //Server.MapPath(strFilePath);
                //FileUpload1.SaveAs(FileSaveWithPath);
                //strFilePath = "D:/sales_report_1527503970191.csv";
                FileStream fileStream = new FileStream(strFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                // fileStream ="C:/Users/Admin/Desktop/Paytm/sales_report_1527503970191.csv";
                using (StreamReader sr = new StreamReader(fileStream))
                {
                    while (!sr.EndOfStream)
                    {
                        Fulltext = sr.ReadToEnd().ToString(); //read full file text  
                        string[] rows = Fulltext.Split('\n'); //split full file text into rows  
                        for (int i = 0; i < rows.Count() - 1; i++)
                        {
                            string[] rowValues = rows[i].Split(','); //split each row with comma to get individual values  
                            {
                                if (i == 0)
                                {
                                    for (int j = 0; j < rowValues.Count(); j++)
                                    {
                                        if (string.IsNullOrWhiteSpace(rowValues[j]))
                                            continue;

                                        dtCsv.Columns.Add(rowValues[j].Replace("\r", "").Replace("\n", "").Trim()); //add headers  
                                    }
                                }
                                else
                                {
                                    DataRow dr = dtCsv.NewRow();
                                    for (int k = 0; k < dtCsv.Columns.Count; k++)
                                    {
                                        dr[k] = rowValues[k].ToString();
                                    }
                                    dtCsv.Rows.Add(dr); //add other rows  
                                }
                            }
                        }
                    }
                    fileStream.Close();

                }


                var columns = dtCsv.Columns;

                for (int i = 0; i < dtCsv.Rows.Count; i++)
                {

                    string order_id = columns.Contains("order_id") ? dtCsv.Rows[i]["order_id"].ToString() : "";
                    string orderitem = columns.Contains("order_item_id") ? dtCsv.Rows[i]["order_item_id"].ToString().Replace("\"", "")
                                                    : columns.Contains("item_id") ? dtCsv.Rows[i]["item_id"].ToString().Replace("\"", "") : "";

                    if (string.IsNullOrWhiteSpace(order_id) || string.IsNullOrWhiteSpace(orderitem))
                        continue;

                    var getdetails = dba.tbl_PaytmMaster.Where(a => a.tbl_SellerId == SellerId && a.Marketplaceid == marketplaceID && a.order_id == order_id && a.order_item_id == orderitem).FirstOrDefault();
                    if (getdetails == null)
                    {
                        tbl_PaytmMaster objmaster = new tbl_PaytmMaster();
                        objmaster.merchant_id = columns.Contains("merchant_id") ? dtCsv.Rows[i]["merchant_id"].ToString().Replace("\"", "") : "";

                        objmaster.merchant_name = columns.Contains("merchant_name") ? dtCsv.Rows[i]["merchant_name"].ToString().Replace("\"", "") : "";
                        objmaster.fulfillment_service_name = columns.Contains("fulfillment_service_name") ? dtCsv.Rows[i]["fulfillment_service_name"].ToString().Replace("\"", "") : "";
                        objmaster.service_type = columns.Contains("service_type") ? dtCsv.Rows[i]["service_type"].ToString().Replace("\"", "") : "";

                        objmaster.created_at = columns.Contains("created_at") ? dtCsv.Rows[i]["created_at"].ToString().Replace("\"", "") 
                                               : columns.Contains("Order_date") ? dtCsv.Rows[i]["Order_date"].ToString().Replace("\"", ""):
                                                columns.Contains("creation date") ? dtCsv.Rows[i]["creation date"].ToString().Replace("\"", "") : "";

                        objmaster.updated_at = columns.Contains("updated_at") ? dtCsv.Rows[i]["updated_at"].ToString().Replace("\"", "") :
                                               columns.Contains("updation date") ? dtCsv.Rows[i]["updation date"].ToString().Replace("\"", "") : "";
                        objmaster.order_id = columns.Contains("order_id") ? dtCsv.Rows[i]["order_id"].ToString().Replace("\"", "") : "";

                        objmaster.order_item_id = columns.Contains("order_item_id") ? dtCsv.Rows[i]["order_item_id"].ToString().Replace("\"", "")
                                                    : columns.Contains("item_id") ? dtCsv.Rows[i]["item_id"].ToString().Replace("\"", "") : "";

                        objmaster.Merchant_sku = columns.Contains("Merchant_sku") ? dtCsv.Rows[i]["Merchant_sku"].ToString().Replace("\"", "")
                                                 : columns.Contains("item.sku") ? dtCsv.Rows[i]["item.sku"].ToString().Replace("\"", "") : ""; 
                        objmaster.product_id = columns.Contains("product_id") ? dtCsv.Rows[i]["product_id"].ToString().Replace("\"", "")
                            : columns.Contains("item.product_id") ? dtCsv.Rows[i]["item.product_id"].ToString().Replace("\"", "") :"";

                        objmaster.sku_name = columns.Contains("sku_name") ? dtCsv.Rows[i]["sku_name"].ToString().Replace("\"", "") 
                            : columns.Contains("Product_name") ? dtCsv.Rows[i]["Product_name"].ToString().Replace("\"", "")
                            : columns.Contains("item_name") ? dtCsv.Rows[i]["item_name"].ToString().Replace("\"", "") : "";

                        objmaster.fulfillment_id = columns.Contains("fulfillment_id") ? dtCsv.Rows[i]["fulfillment_id"].ToString().Replace("\"", "") : "";
                        objmaster.promo_description = columns.Contains("promo_description") ? dtCsv.Rows[i]["promo_description"].ToString().Replace("\"", "") : "";
                        objmaster.mrp = columns.Contains("mrp") ? Convert.ToDouble(dtCsv.Rows[i]["mrp"].ToString().Replace("\"", "")) :
                                        columns.Contains("item_mrp") ? Convert.ToDouble(dtCsv.Rows[i]["item_mrp"].ToString().Replace("\"", "")) : 0;

                        objmaster.price = columns.Contains("price") ? Convert.ToDouble(dtCsv.Rows[i]["price"].ToString()) 
                            : objmaster.price = columns.Contains("selling_price") ? Convert.ToDouble(dtCsv.Rows[i]["selling_price"].ToString()): 0;

                        objmaster.ship_by_date = columns.Contains("ship_by_date") ? dtCsv.Rows[i]["ship_by_date"].ToString().Replace("\"", "") :
                                                columns.Contains("estimated shipping date") ? dtCsv.Rows[i]["estimated shipping date"].ToString().Replace("\"", "") : "";
                        objmaster.attributes = columns.Contains("attributes") ? dtCsv.Rows[i]["attributes"].ToString().Replace("\"", "") : "";

                        objmaster.qty_ordered = columns.Contains("qty_ordered") ? Convert.ToInt32(dtCsv.Rows[i]["qty_ordered"].ToString()) :
                                                columns.Contains("qty") ? Convert.ToInt32(dtCsv.Rows[i]["qty"].ToString()) : 0;
                        objmaster.total_price = columns.Contains("total_price") ? Convert.ToDouble(dtCsv.Rows[i]["total_price"].ToString())
                                                :columns.Contains("item_price") ? Convert.ToDouble(dtCsv.Rows[i]["item_price"].ToString()) : 0;

                        objmaster.item_status_text = columns.Contains("item_status_text") ? dtCsv.Rows[i]["item_status_text"].ToString().Replace("\"", "") 
                            : columns.Contains("item_status") ? dtCsv.Rows[i]["item_status"].ToString().Replace("\"", "")
                            : columns.Contains("item status") ? dtCsv.Rows[i]["item status"].ToString().Replace("\"", "") : "";

                        objmaster.discount = columns.Contains("discount") ? Convert.ToDouble(dtCsv.Rows[i]["discount"].ToString()) : 0;
                        objmaster.wallet_paid = columns.Contains("wallet_paid") ? Convert.ToDouble(dtCsv.Rows[i]["wallet_paid"].ToString()) : 0;
                        objmaster.pg_paid = columns.Contains("pg_paid") ? Convert.ToDouble(dtCsv.Rows[i]["pg_paid"].ToString()) : 0;
                        objmaster.cod_paid = columns.Contains("cod_paid") ? Convert.ToDouble(dtCsv.Rows[i]["cod_paid"].ToString()) : 0;
                        objmaster.marketplace_cashback = columns.Contains("marketplace_cashback") ? Convert.ToDouble(dtCsv.Rows[i]["marketplace_cashback"].ToString()) : 0;
                        objmaster.vertical_name = columns.Contains("vertical_name") ? dtCsv.Rows[i]["vertical_name"].ToString().Replace("\"", "") : "";
                        objmaster.tracking_number = columns.Contains("tracking_number") ? dtCsv.Rows[i]["tracking_number"].ToString().Replace("\"", "") : "";
                        objmaster.shipping_amount = columns.Contains("shipping_amount") ? Convert.ToDouble(dtCsv.Rows[i]["shipping_amount"].ToString()) : 0;
                        objmaster.shipped_at = columns.Contains("shipped_at") ? dtCsv.Rows[i]["shipped_at"].ToString().Replace("\"", "") : "";
                        objmaster.delivered_at = columns.Contains("delivered_at") ? dtCsv.Rows[i]["delivered_at"].ToString().Replace("\"", "") : "";
                        objmaster.returned_at = columns.Contains("returned_at") ? dtCsv.Rows[i]["returned_at"].ToString().Replace("\"", "") : "";
                        objmaster.isCOD = columns.Contains("isCOD") ? dtCsv.Rows[i]["isCOD"].ToString() : "";
                        objmaster.forward_settled_at = columns.Contains("forward_settled_at") ? dtCsv.Rows[i]["forward_settled_at"].ToString().Replace("\"", "") : "";
                        objmaster.reverse_settled_at = columns.Contains("reverse_settled_at") ? dtCsv.Rows[i]["reverse_settled_at"].ToString().Replace("\"", "") : "";
                        objmaster.merchant_address = columns.Contains("merchant_address") ? dtCsv.Rows[i]["merchant_address"].ToString().Replace("\"", "") : "";
                        objmaster.merchant_city = columns.Contains("merchant_city") ? dtCsv.Rows[i]["merchant_city"].ToString().Replace("\"", "") : "";
                        objmaster.merchant_state = columns.Contains("merchant_state") ? dtCsv.Rows[i]["merchant_state"].ToString().Replace("\"", "") : "";
                        objmaster.merchant_pincode = columns.Contains("merchant_pincode") ? dtCsv.Rows[i]["merchant_pincode"].ToString().Replace("\"", "") : "";
                        objmaster.customer_id = columns.Contains("customer_id") ? dtCsv.Rows[i]["customer_id"].ToString() : "";
                        objmaster.customer_firstname = columns.Contains("customer_firstname") ? dtCsv.Rows[i]["customer_firstname"].ToString().Replace("\"", "") :"";
                        objmaster.customer_lastname = columns.Contains("customer_lastname") ? dtCsv.Rows[i]["customer_lastname"].ToString().Replace("\"", "") : "";

                        objmaster.customer_address = columns.Contains("customer_address") ? dtCsv.Rows[i]["customer_address"].ToString().Replace("\"", "") 
                            : columns.Contains("address") ? dtCsv.Rows[i]["address"].ToString().Replace("\"", ""): "";

                        objmaster.customer_city = columns.Contains("customer_city") ? dtCsv.Rows[i]["customer_city"].ToString().Replace("\"", "") 
                            : columns.Contains("city") ? dtCsv.Rows[i]["city"].ToString().Replace("\"", "") : "";

                        objmaster.customer_pincode = columns.Contains("customer_pincode") ? dtCsv.Rows[i]["customer_pincode"].ToString().Replace("\"", "")
                            : columns.Contains("pincode") ? dtCsv.Rows[i]["pincode"].ToString().Replace("\"", "") : "";

                        objmaster.customer_email = columns.Contains("customer_email") ? dtCsv.Rows[i]["customer_email"].ToString().Replace("\"", "") : "";

                        objmaster.phone = columns.Contains("phone") ? dtCsv.Rows[i]["phone"].ToString().Replace("\"", "") : "";
                        objmaster.Shipment_created_date = columns.Contains("Shipment_created_date") ? dtCsv.Rows[i]["Shipment_created_date"].ToString().Replace("\"", "") : "";
                        objmaster.awb = columns.Contains("awb") ? dtCsv.Rows[i]["awb"].ToString().Replace("\"", "") : "";
                        objmaster.tracking_url = columns.Contains("tracking_url") ? dtCsv.Rows[i]["tracking_url"].ToString().Replace("\"", "") : "";
                        objmaster.shipper = columns.Contains("shipper") ? dtCsv.Rows[i]["shipper"].ToString().Replace("\"", "") : "";
                        objmaster.IMEI = columns.Contains("IMEI") ? dtCsv.Rows[i]["IMEI"].ToString().Replace("\"", "") : "";

                        objmaster.shipper_id = columns.Contains("shipper_id") ? dtCsv.Rows[i]["shipper_id"].ToString().Replace("\"", "") : "";
                        objmaster.manifest_id = columns.Contains("manifest_id") ? dtCsv.Rows[i]["manifest_id"].ToString().Replace("\"", "") : "";
                        objmaster.SLAextended = columns.Contains("SLAextended") ? dtCsv.Rows[i]["SLAextended"].ToString().Replace("\"", "") : "";
                        objmaster.warehouse_id = columns.Contains("warehouse_id") ? dtCsv.Rows[i]["warehouse_id"].ToString().Replace("\"", "") : "";
                        objmaster.merchant_overdue_date = columns.Contains("merchant_overdue_date") ? dtCsv.Rows[i]["merchant_overdue_date"].ToString().Replace("\"", "") : "";
                        objmaster.child_order_id = columns.Contains("child_order_id") ? dtCsv.Rows[i]["child_order_id"].ToString().Replace("\"", "") : "";
                        objmaster.child_item_id = columns.Contains("child_item_id") ? dtCsv.Rows[i]["child_item_id"].ToString().Replace("\"", "") : "";
                        objmaster.max_refund = columns.Contains("max_refund") ? dtCsv.Rows[i]["max_refund"].ToString().Replace("\"", "") : "";
                        objmaster.mr_at = columns.Contains("mr_at") ? dtCsv.Rows[i]["mr_at"].ToString().Replace("\"", "") : "";
                        objmaster.delivery_type = columns.Contains("delivery_type") ? dtCsv.Rows[i]["delivery_type"].ToString().Replace("\"", "") : "";
                        objmaster.meta_data = columns.Contains("meta_data") ? dtCsv.Rows[i]["meta_data"].ToString().Replace("\"", "") : "";
                        objmaster.fulfillment_req = columns.Contains("fulfillment_req") ? dtCsv.Rows[i]["fulfillment_req"].ToString().Replace("\"", "") : "";
                        objmaster.delivery_mode = columns.Contains("delivery_mode") ? dtCsv.Rows[i]["delivery_mode"].ToString().Replace("\"", "") : "";

                        objmaster.customer_gst = columns.Contains("customer_gst") ? dtCsv.Rows[i]["customer_gst"].ToString().Replace("\"", "") : "";
                        objmaster.state = columns.Contains("state") ? dtCsv.Rows[i]["state"].ToString().Replace("\"", "") : "";

                        objmaster.invoice_id = columns.Contains("invoice_id") ? dtCsv.Rows[i]["invoice_id"].ToString().Replace("\"", "") : "";
                        objmaster.invoice_date = columns.Contains("invoice_date") ? dtCsv.Rows[i]["invoice_date"].ToString().Replace("\"", "") : "";
                        objmaster.origin_pin = columns.Contains("origin_pin") ? dtCsv.Rows[i]["origin_pin"].ToString().Replace("\"", "") : "";
                        objmaster.destination_pin = columns.Contains("destination_pin") ? dtCsv.Rows[i]["destination_pin"].ToString().Replace("\"", "") : "";
                        objmaster.origin_city = columns.Contains("origin_city") ? dtCsv.Rows[i]["origin_city"].ToString().Replace("\"", "") : "";
                        objmaster.destination_city = columns.Contains("destination_city") ? dtCsv.Rows[i]["destination_city"].ToString().Replace("\"", "") : "";
                        objmaster.failure_reason = columns.Contains("failure_reason") ? dtCsv.Rows[i]["failure_reason"].ToString().Replace("\"", "") : "";

                        objmaster.payout_status = columns.Contains("payout_status") ? dtCsv.Rows[i]["payout_status"].ToString().Replace("\"", "") :
                            columns.Contains("payment_status") ? dtCsv.Rows[i]["payment_status"].ToString().Replace("\"", "") : "";

                        objmaster.conv_fee = columns.Contains("conv_fee") ? dtCsv.Rows[i]["conv_fee"].ToString().Replace("\"", "") : "";
                        objmaster.parent_item_id = columns.Contains("parent_item_id") ? dtCsv.Rows[i]["parent_item_id"].ToString().Replace("\"", "") : "";
                        objmaster.parent_order_id = columns.Contains("parent_order_id") ? dtCsv.Rows[i]["parent_order_id"].ToString().Replace("\"", "") : "";
                        objmaster.qrid = columns.Contains("qrid") ? dtCsv.Rows[i]["qrid"].ToString().Replace("\"", "") : "";
                        objmaster.ecc = columns.Contains("ecc") ? dtCsv.Rows[i]["ecc"].ToString().Replace("\"", "") : "";
                        objmaster.wt = columns.Contains("wt") ? dtCsv.Rows[i]["wt"].ToString().Replace("\"", "") : "";

                        objmaster.dsv = columns.Contains("dsv") ? dtCsv.Rows[i]["dsv"].ToString().Replace("\"", "") : "";
                        objmaster.payout_from = columns.Contains("payout_from") ? dtCsv.Rows[i]["payout_from"].ToString().Replace("\"", "") : "";
                        objmaster.payout_status_forward = columns.Contains("payout_status_forward") ? dtCsv.Rows[i]["payout_status_forward"].ToString().Replace("\"", "") : "";
                        objmaster.payout_status_reverse = columns.Contains("payout_status_reverse") ? dtCsv.Rows[i]["payout_status_reverse"].ToString().Replace("\"", "") : "";
                        objmaster.c_sid = columns.Contains("c_sid") ? dtCsv.Rows[i]["c_sid"].ToString().Replace("\"", "") : "";
                        objmaster.isLMD = columns.Contains("isLMD") ? dtCsv.Rows[i]["isLMD"].ToString().Replace("\"", "") : "";
                        objmaster.payment_bank = columns.Contains("payment_bank") ? dtCsv.Rows[i]["payment_bank"].ToString().Replace("\"", "") : "";
                        objmaster.payment_method = columns.Contains("payment_method") ? dtCsv.Rows[i]["payment_method"].ToString().Replace("\"", "") : "";
                        objmaster.promo_code = columns.Contains("promo_code") ? dtCsv.Rows[i]["promo_code"].ToString().Replace("\"", "") : "";
                        double aa = columns.Contains("GstAmount") ? Convert.ToDouble(dtCsv.Rows[i]["GstAmount"].ToString()) : 0;
                        objmaster.GSTAmount = columns.Contains("GstAmount") ? Convert.ToDouble(dtCsv.Rows[i]["GstAmount"].ToString().Replace("\"", "")) : 0;

                        objmaster.Uploadid = id;
                        objmaster.Marketplaceid = marketplaceID;
                        objmaster.Status = 0;
                        objmaster.tbl_SellerId = SellerId;
                        dba.tbl_PaytmMaster.Add(objmaster);
                        dba.SaveChanges();
                    }
                }
                SavePaytmOrder(marketplaceID, id, SellerId);
            }
            catch (Exception ex)
            {
            }
            return dtCsv;
            //return success;
        }


        public string SavePaytmOrder1(int marketplaceID, int id, int SellerId)
        {
            string success = "S";
            try
            {
                int count = 0;
                int kk = 0;
                string startdate = "", enddate = "";
                var getdetails = dba.tbl_sales_order_status.Where(aa => aa.is_active == 0).ToList();
                Dictionary<string, int> orderstatus_dict = new Dictionary<string, int>();
                foreach (var item1 in getdetails)
                {
                    string name = item1.sales_order_status.ToLower();
                    orderstatus_dict.Add(name, item1.id);
                }
                var getpaytmdetails = dba.tbl_PaytmMaster.Where(a => a.tbl_SellerId == SellerId && a.Marketplaceid == marketplaceID && a.Uploadid == id && a.Status == 0).ToList();
                if (getpaytmdetails != null)
                {
                    var get_upload_order_details = dba.tbl_order_upload.Where(aa => aa.tbl_seller_id == SellerId && aa.id == id && aa.tbl_Marketplace_id == marketplaceID).FirstOrDefault();
                    if (get_upload_order_details != null)
                    {
                        get_upload_order_details.status = "Processing";
                        get_upload_order_details.processing_datetime = DateTime.Now;
                        dba.Entry(get_upload_order_details).State = EntityState.Modified;
                        dba.SaveChanges();
                    }
                    foreach (var item in getpaytmdetails)
                    {
                        if (kk == 0)
                        {
                            startdate = item.created_at.ToString();
                        }
                        if (kk == getpaytmdetails.Count - 1)
                        {
                            enddate = item.created_at.ToString();
                        }
                        kk++;
                        //var get_saleorder = dba.tbl_sales_order.Where(aa => aa.tbl_sellers_id == SellerId && aa.amazon_order_id == item.OrderId && aa.order_status == item.OrderState).FirstOrDefault();
                        //if (get_saleorder == null)
                        //{

                        tbl_customer_details objcustumer = new tbl_customer_details();
                        objcustumer.State_Region = item.state;
                        objcustumer.shipping_Buyer_Name = item.customer_firstname;
                        objcustumer.last_name = item.customer_lastname;
                        objcustumer.City = item.customer_city;
                        objcustumer.Country_Code = "IN";
                        objcustumer.Postal_Code = item.customer_pincode;
                        objcustumer.Address_1 = item.customer_address;
                        objcustumer.customer_email = item.customer_email;
                        objcustumer.contact_no = item.phone;


                        objcustumer.tbl_seller_id = SellerId;
                        dba.tbl_customer_details.Add(objcustumer);
                        dba.SaveChanges();

                        tbl_sales_order objsales = new tbl_sales_order();
                        objsales.amazon_order_id = item.order_id;
                        objsales.buyer_name = item.customer_firstname;

                        objsales.tbl_Customer_Id = objcustumer.id;
                        objsales.purchase_date = Convert.ToDateTime(item.created_at);
                        objsales.order_status = item.item_status_text;
                        objsales.no_of_itemshipped = Convert.ToInt32(item.qty_ordered);
                        objsales.currency_code = "INR";
                        objsales.bill_amount = Convert.ToDouble(item.total_price);//Convert.ToDouble(item.SellingPricePerItem);
                        objsales.tbl_Marketplace_Id = marketplaceID;
                        objsales.order_item_id = item.order_item_id;
                        objsales.created_on = DateTime.Now;
                        objsales.is_active = 1;
                        objsales.tbl_sellers_id = SellerId;
                        objsales.tbl_order_upload_id = id;
                        objsales.sales_channel = "Sale Customer PayTm";//"Paytm.in";

                        objsales.last_updated_date = string.IsNullOrEmpty(item.updated_at)? (DateTime?)null: Convert.ToDateTime(item.updated_at);

                        if (item.delivered_at != "0000-00-00 00:00:00" && !string.IsNullOrEmpty(item.delivered_at))
                        {
                            objsales.Latest_ShipDate = Convert.ToDateTime(item.delivered_at);
                        }
                        if (item.shipped_at != "0000-00-00 00:00:00" && !string.IsNullOrEmpty(item.shipped_at))
                        {
                            objsales.earliest_ship_date = Convert.ToDateTime(item.shipped_at);
                        }
                        //if (item.returned_at != "0000-00-00 00:00:00" && item.returned_at != "")
                        //{
                        //    objsales.returned_at = Convert.ToDateTime(item.returned_at);
                        //}
                        objsales.fullfillment_channel = "PayTm";
                        objsales.LastUpdatedDateUTC = DateTime.UtcNow;
                        objsales.merchant_id = item.merchant_id;
                        objsales.ship_service_category = item.service_type;
                        if (!string.IsNullOrEmpty(item.shipped_at) && item.shipped_at != "0000-00-00 00:00:00")
                        {
                            objsales.dispatch_afterdate = Convert.ToDateTime(item.shipped_at);
                        }

                        string name1 = item.item_status_text.ToLower();
                        if (orderstatus_dict.ContainsKey(name1))
                        {
                            objsales.n_item_orderstatus = orderstatus_dict[name1];
                        }

                        dba.tbl_sales_order.Add(objsales);
                        dba.SaveChanges();

                        tbl_sales_order_details objsaledetails = new tbl_sales_order_details();

                        objsaledetails.quantity_ordered = Convert.ToInt32(item.qty_ordered);
                        objsaledetails.quantity_shipped = Convert.ToInt32(item.qty_ordered);
                        objsaledetails.product_name = item.sku_name;

                        //objsaledetails.sku_no = item.MerchantSku;
                        objsaledetails.order_item_id = item.order_item_id;
                        objsaledetails.shipping_price_curr_code = "INR";
                        objsaledetails.item_price_curr_code = "INR";

                        objsaledetails.item_tax_amount = Convert.ToDouble(item.GSTAmount);


                        objsaledetails.item_price_amount = Convert.ToDouble(item.total_price);
                        //objsaledetails.shipping_price_Amount = Convert.ToDouble(item.ShippingAmount);
                        objsaledetails.is_active = 1;
                        objsaledetails.tbl_seller_id = SellerId;
                        objsaledetails.tbl_sales_order_id = objsales.id;
                        objsaledetails.status_updated_by = SellerId;
                        objsaledetails.status_updated_on = DateTime.Now;
                        objsaledetails.sku_no = item.product_id;

                        objsaledetails.n_order_status_id = Convert.ToInt16(objsales.n_item_orderstatus);
                        objsaledetails.amazon_order_id = objsales.amazon_order_id;
                        if (item.shipped_at != "0000-00-00 00:00:00" && !string.IsNullOrEmpty(item.shipped_at))
                        {
                            objsaledetails.dispatchAfter_date = Convert.ToDateTime(item.shipped_at);
                        }
                        if (item.delivered_at != "0000-00-00 00:00:00" && !string.IsNullOrEmpty(item.delivered_at))
                        {
                            objsaledetails.dispatch_bydate = Convert.ToDateTime(item.delivered_at);
                        }
                        objsaledetails.is_tax_calculated = 0;
                        objsaledetails.tax_updatedby_taxfile = 0;
                        objsaledetails.product_id = item.product_id;
                        objsaledetails.promo_code = item.promo_code;
                        dba.tbl_sales_order_details.Add(objsaledetails);
                        dba.SaveChanges();



                        #region taxcalculated
                        //-------------------------------calculate tax from state wise -----------------------

                        double itemtax = 0, cgst_tax = 0, sgst_tax = 0, igst_tax = 0;
                        double item_price = 0, shipping_price = 0, shipp_pricewithouttax = 0, itempricewithout_tax = 0, cgst_amount = 0, sgst_amount = 0, igst_amount = 0, shipping_price_tax = 0, item_price_tax = 0;
                        string customerstate = "";

                        var get_saleorderdetails = dba.tbl_sales_order_details.Where(t => t.tbl_seller_id == SellerId && t.id == objsaledetails.id).FirstOrDefault();
                        if (get_saleorderdetails != null)
                        {
                            var get_customerDetails = dba.tbl_customer_details.Where(t => t.id == objcustumer.id && t.tbl_seller_id == SellerId).FirstOrDefault();
                            if (get_customerDetails != null)
                            {
                                if (get_customerDetails.State_Region != null)
                                {
                                    customerstate = get_customerDetails.State_Region.ToLower();
                                }
                                shipp_pricewithouttax = get_saleorderdetails.shipping_price_Amount - get_saleorderdetails.shipping_tax_Amount;
                                itempricewithout_tax = get_saleorderdetails.item_price_amount - get_saleorderdetails.item_tax_amount;
                                var seller_details = db.tbl_sellers.Where(t => t.id == SellerId).FirstOrDefault();
                                if (get_saleorderdetails.giftwrapprice_amount == null)
                                {
                                    get_saleorderdetails.giftwrapprice_amount = 0;
                                }
                                double amount = Convert.ToDouble(get_saleorderdetails.item_price_amount + get_saleorderdetails.shipping_price_Amount + get_saleorderdetails.giftwrapprice_amount);
                                double promotionamount = 0;
                                if (get_saleorderdetails.promotion_amount != null)
                                    promotionamount = get_saleorderdetails.promotion_amount;
                                if (get_saleorderdetails.item_promotionAmount != null)
                                    promotionamount += Convert.ToDouble(get_saleorderdetails.item_promotionAmount);

                                var totalamount = amount - promotionamount;

                                double totaltax = 0;
                                if (get_saleorderdetails.item_tax_amount != null)
                                    totaltax = get_saleorderdetails.item_tax_amount;
                                if (get_saleorderdetails.shipping_tax_Amount != null)
                                    totaltax += get_saleorderdetails.shipping_tax_Amount;

                                if (get_saleorderdetails.giftwraptax_amount != null)
                                    totaltax += Convert.ToDouble(get_saleorderdetails.giftwraptax_amount);

                                if (totaltax == null)
                                    totaltax = 0;


                                if (seller_details != null)
                                {
                                    var getcountrydetails = db.tbl_country.Where(t => t.status == 1 && t.countrylevel == 0 && t.id == seller_details.country).FirstOrDefault();// to get country name from country table in admin db.
                                    var getstatedetails = db.tbl_country.Where(m => m.id == seller_details.state && m.countrylevel == 1 && m.status == 1).FirstOrDefault();//to get state name from country table in admin db.
                                    string sellerstate = getstatedetails.countryname.ToLower();
                                    if (sellerstate == customerstate)
                                    {
                                        decimal result = 0;
                                        double tax_percent = 0;
                                        string value = Convert.ToString((totaltax * 100) / totalamount);
                                        if (value != "NaN" && value != "" && value != null)
                                        {
                                            decimal abcd = Convert.ToDecimal(value);
                                            result = decimal.Round(abcd, 2, MidpointRounding.AwayFromZero);
                                            tax_percent = Convert.ToDouble(result);
                                            item_price_tax = (totalamount * tax_percent) / 100;
                                        }

                                        item_price = get_saleorderdetails.item_price_amount - item_price_tax;
                                        shipping_price_tax = (get_saleorderdetails.shipping_price_Amount * tax_percent) / 100;
                                        shipping_price = get_saleorderdetails.shipping_price_Amount - shipping_price_tax;
                                        cgst_tax = Convert.ToDouble(result) / 2;
                                        sgst_tax = Convert.ToDouble(result) - cgst_tax;

                                        //string calculatedted_tax1 = Convert.ToString((finalamt * tax_percent) / 100);
                                        string calculatedted_tax1 = Convert.ToString(item_price_tax);
                                        decimal abcd_2 = Convert.ToDecimal(calculatedted_tax1);
                                        decimal result_2 = decimal.Round(abcd_2, 2, MidpointRounding.AwayFromZero);
                                        double cal_tax = Convert.ToDouble(result_2);
                                        cgst_amount = totaltax / 2;//(cal_tax) / 2; 
                                        sgst_amount = totaltax - cgst_amount; //(cal_tax - cgst_amount);     


                                        //---------------save data in salesldeger tax table------------//
                                        string cgst_taxrate = "";
                                        string sgst_taxrate = "";
                                        string input_decimal_number = Convert.ToString(cgst_tax);
                                        var regex = new System.Text.RegularExpressions.Regex("(?<=[\\.])[0-9]+");
                                        if (regex.IsMatch(input_decimal_number))
                                        {
                                            string decimal_places = regex.Match(input_decimal_number).Value;
                                            cgst_taxrate = decimal_places;
                                        }
                                        else
                                        {
                                            cgst_taxrate = input_decimal_number;
                                        }
                                        string input_decimal_number1 = Convert.ToString(sgst_tax);
                                        var regex1 = new System.Text.RegularExpressions.Regex("(?<=[\\.])[0-9]+");
                                        if (regex1.IsMatch(input_decimal_number1))
                                        {
                                            string decimal_places1 = regex1.Match(input_decimal_number1).Value;
                                            sgst_taxrate = decimal_places1;
                                        }
                                        else
                                        {
                                            sgst_taxrate = input_decimal_number1;
                                        }

                                        string Taxname = "CGST@" + cgst_taxrate + "%";
                                        string taxname = "SGST@" + sgst_taxrate + "%";
                                        if (cgst_taxrate != "" && cgst_taxrate != null && cgst_taxrate != "0")
                                        {
                                            var gettaxdetails = dba.tbl_Salesledger_tax.Where(t => t.tax_name == Taxname).FirstOrDefault();
                                            if (gettaxdetails == null)
                                            {
                                                try
                                                {
                                                    tbl_Salesledger_tax obj_salestax = new tbl_Salesledger_tax();
                                                    obj_salestax.tax_name = Taxname;
                                                    obj_salestax.tax_percentage = Convert.ToDouble(cgst_taxrate);
                                                    dba.tbl_Salesledger_tax.Add(obj_salestax);
                                                    dba.SaveChanges();
                                                }
                                                catch (Exception ex)
                                                {
                                                }
                                            }
                                        }
                                        if (sgst_taxrate != "" && sgst_taxrate != null && sgst_taxrate != "0")
                                        {
                                            var gettaxdetail = dba.tbl_Salesledger_tax.Where(t => t.tax_name == taxname).FirstOrDefault();
                                            if (gettaxdetail == null)
                                            {
                                                try
                                                {
                                                    tbl_Salesledger_tax obj_salestax = new tbl_Salesledger_tax();
                                                    obj_salestax.tax_name = taxname;
                                                    obj_salestax.tax_percentage = Convert.ToDouble(sgst_taxrate);
                                                    dba.tbl_Salesledger_tax.Add(obj_salestax);
                                                    dba.SaveChanges();
                                                }
                                                catch (Exception ex)
                                                {
                                                }
                                            }
                                        }
                                        //-------------------------------end---------------------------/
                                    }
                                    else
                                    {
                                        string value1 = Convert.ToString((totaltax * 100) / totalamount);
                                        if (value1 != "NaN" && value1 != "" && value1 != null)
                                        {
                                            decimal abcd1 = Convert.ToDecimal(value1);
                                            decimal result1 = decimal.Round(abcd1, 2, MidpointRounding.AwayFromZero);
                                            igst_tax = Convert.ToDouble(result1);
                                        }

                                        //item_price_tax =Convert.ToDouble((finalamt * igst_tax) / 100);
                                        decimal price_tax = Convert.ToDecimal((totalamount * igst_tax) / 100);
                                        decimal result3 = decimal.Round(price_tax, 2, MidpointRounding.AwayFromZero);
                                        item_price_tax = Convert.ToDouble(result3);
                                        item_price = get_saleorderdetails.item_price_amount - item_price_tax;
                                        shipping_price_tax = (get_saleorderdetails.shipping_price_Amount * igst_tax) / 100;
                                        shipping_price = get_saleorderdetails.shipping_price_Amount - shipping_price_tax;
                                        //string calculatedted_tax = Convert.ToString((item_price * igst_tax) / 100);
                                        //decimal abcd2 = Convert.ToDecimal(calculatedted_tax);
                                        //decimal result2 = decimal.Round(abcd2, 2, MidpointRounding.AwayFromZero);
                                        igst_amount = totaltax;// item_price_tax;

                                        string igst_taxrate = "";
                                        string input_decimal_number4 = Convert.ToString(igst_tax);
                                        var regex4 = new System.Text.RegularExpressions.Regex("(?<=[\\.])[0-9]+");
                                        if (regex4.IsMatch(input_decimal_number4))
                                        {
                                            string decimal_places4 = regex4.Match(input_decimal_number4).Value;
                                            igst_taxrate = decimal_places4;
                                        }
                                        else
                                        {
                                            igst_taxrate = input_decimal_number4;
                                        }
                                        if (igst_taxrate != "" && igst_taxrate != null && igst_taxrate != "0")
                                        {
                                            string igstTaxname = "IGST@" + igst_taxrate + "%";
                                            var gettaxdetails = dba.tbl_Salesledger_tax.Where(t => t.tax_name == igstTaxname).FirstOrDefault();
                                            if (gettaxdetails == null)
                                            {
                                                try
                                                {
                                                    tbl_Salesledger_tax obj_salestax = new tbl_Salesledger_tax();
                                                    obj_salestax.tax_name = igstTaxname;
                                                    obj_salestax.tax_percentage = Convert.ToDouble(igst_taxrate);
                                                    dba.tbl_Salesledger_tax.Add(obj_salestax);
                                                    dba.SaveChanges();
                                                }
                                                catch (Exception ex)
                                                {
                                                }
                                            }
                                        }
                                    }
                                    var get_taxdetails = dba.tbl_tax.Where(t => t.tbl_referneced_id == objsaledetails.id && t.reference_type == 3).FirstOrDefault();
                                    if (get_taxdetails == null)
                                    {
                                        if (igst_tax != 0 || cgst_tax != 0.0 || sgst_tax != 0.0)
                                        {
                                            tbl_tax objtax = new tbl_tax();
                                            objtax.tbl_seller_id = SellerId;
                                            objtax.tbl_referneced_id = objsaledetails.id;
                                            objtax.reference_type = 3;
                                            objtax.isactive = 1;
                                            if (cgst_tax != null && cgst_tax != 0.0 && sgst_tax != null && sgst_tax != 0.0)
                                            {
                                                objtax.cgst_tax = cgst_tax;
                                                objtax.sgst_tax = sgst_tax;
                                                objtax.CGST_amount = cgst_amount;
                                                objtax.sgst_amount = sgst_amount;
                                            }
                                            else
                                            {
                                                objtax.igst_tax = igst_tax;
                                                objtax.Igst_amount = igst_amount;
                                            }
                                            dba.tbl_tax.Add(objtax);
                                            dba.SaveChanges();
                                        }
                                        else
                                        {
                                            tbl_tax objtax = new tbl_tax();
                                            objtax.tbl_seller_id = SellerId;
                                            objtax.tbl_referneced_id = objsaledetails.id;
                                            objtax.reference_type = 3;
                                            objtax.isactive = 1;
                                            objtax.igst_tax = igst_tax;
                                            objtax.Igst_amount = igst_amount;
                                            dba.tbl_tax.Add(objtax);
                                            dba.SaveChanges();
                                        }

                                    }// end of if(get_taxdetails)

                                    //else
                                    //{
                                    //    if (cgst_tax != null && cgst_tax != 0.0 && sgst_tax != null && sgst_tax != 0.0)
                                    //    {
                                    //        get_taxdetails.cgst_tax = cgst_tax;
                                    //        get_taxdetails.sgst_tax = sgst_tax;
                                    //        get_taxdetails.CGST_amount = cgst_amount;
                                    //        get_taxdetails.sgst_amount = sgst_amount;
                                    //    }
                                    //    else
                                    //    {
                                    //        get_taxdetails.igst_tax = igst_tax;
                                    //        get_taxdetails.Igst_amount = igst_amount;
                                    //    }
                                    //    dba.Entry(get_taxdetails).State = EntityState.Modified;
                                    //    dba.SaveChanges();
                                    //}
                                }// end of if(seller_details)

                            }// end of get_sales_orders

                            get_saleorderdetails.item_price_amount = itempricewithout_tax;
                            //get_saleorderdetails.shipping_price_Amount = shipp_pricewithouttax;
                            dba.Entry(get_saleorderdetails).State = EntityState.Modified;
                            dba.SaveChanges();
                        }// end of if get_saleorderdetails

                        //-----------------------------------------End-----------------------------------------
                        #endregion

                       

                        //}
                    }// end of foreach

                    string connectionstring = ConfigurationManager.ConnectionStrings["SellerConnection"].ConnectionString;
                    MySqlConnection con = new MySqlConnection(connectionstring);

                    int uniqueupload = 0;
                    int diff_order = 0;
                    int total_order = 0;
                    var get_unique_count_details = "SELECT count(DISTINCT aa.id) FROM(SELECT DISTINCT Order_Id AS id FROM `tbl_settlement_order` where tbl_seller_id = " + SellerId + " UNION ALL SELECT DISTINCT amazon_order_id AS id FROM `tbl_sales_order` where tbl_sellers_id =" + SellerId + " ) AS aa";
                    MySqlCommand cmd = new MySqlCommand(get_unique_count_details, con);
                    con.Open();
                    MySqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        dr.Read();
                        uniqueupload = Convert.ToInt32(dr[0]);
                    }
                    cmd.Dispose();
                    con.Close();

                    MySqlConnection con1 = new MySqlConnection(connectionstring);
                    var get_unique_orderby_id = "SELECT count( aa.id )FROM (SELECT tbl_order_upload_id AS id FROM `tbl_sales_order` where tbl_sellers_id =" + SellerId + " && tbl_order_upload_id = " + id + ") AS aa";
                    MySqlCommand cmd2 = new MySqlCommand(get_unique_orderby_id, con1);
                    con1.Open();
                    MySqlDataReader dr1 = cmd2.ExecuteReader();
                    if (dr1.HasRows)
                    {
                        dr1.Read();
                        total_order = Convert.ToInt32(dr1[0]);
                    }
                    cmd2.Dispose();
                    con1.Close();

                    var get_balance_details = db.tbl_user_login.Where(aa => aa.tbl_sellers_id == SellerId).FirstOrDefault();
                    if (get_balance_details != null)
                    {
                        var last_order = get_balance_details.total_orders;
                        diff_order = Convert.ToInt16(uniqueupload - last_order);
                        var plan_rate = get_balance_details.applied_plan_rate * diff_order;
                        get_balance_details.wallet_balance = get_balance_details.wallet_balance - Convert.ToDouble(plan_rate);
                        int totalOrder = Convert.ToInt16(diff_order);
                        if (get_balance_details.total_orders != null)
                            totalOrder = totalOrder + Convert.ToInt16(get_balance_details.total_orders);
                        get_balance_details.total_orders = totalOrder;
                        db.Entry(get_balance_details).State = EntityState.Modified;
                        db.SaveChangesAsync();
                    }
                    if (get_upload_order_details != null)
                    {
                        get_upload_order_details.new_order_uploaded = total_order;
                        get_upload_order_details.from_date = string.IsNullOrEmpty(startdate)? (DateTime?)null: Convert.ToDateTime(startdate);
                        get_upload_order_details.to_date = string.IsNullOrEmpty(enddate) ? (DateTime?)null : Convert.ToDateTime(enddate);
                        get_upload_order_details.status = "Completed";
                        get_upload_order_details.completed_datetime = DateTime.Now;
                        dba.Entry(get_upload_order_details).State = EntityState.Modified;
                    }
                    dba.SaveChangesAsync();

                    if (getpaytmdetails != null)
                    {
                        foreach (var dd in getpaytmdetails)
                        {
                            dd.Status = 1;
                            dba.Entry(dd).State = EntityState.Modified;
                            dba.SaveChanges();

                        }
                    }

                }
            }
            catch (Exception ex)
            {
            }
            return success;
        }

        public string SavePaytmOrder(int marketplaceID, int id, int SellerId)
        {
            string success = "S";
            try
            {
                int count = 0;
                int kk = 0;
                string startdate = "", enddate = "";
                var getdetails = dba.tbl_sales_order_status.Where(aa => aa.is_active == 0).ToList();
                Dictionary<string, int> orderstatus_dict = new Dictionary<string, int>();
                foreach (var item1 in getdetails)
                {
                    string name = item1.sales_order_status.ToLower();
                    orderstatus_dict.Add(name, item1.id);
                }
                var getpaytmdetails = dba.tbl_PaytmMaster.Where(a => a.tbl_SellerId == SellerId && a.Marketplaceid == marketplaceID && a.Uploadid == id && a.Status == 0).ToList();
                if (getpaytmdetails != null)
                {
                    var get_upload_order_details = dba.tbl_order_upload.Where(aa => aa.tbl_seller_id == SellerId && aa.id == id && aa.tbl_Marketplace_id == marketplaceID).FirstOrDefault();
                    if (get_upload_order_details != null)
                    {
                        get_upload_order_details.status = "Processing";
                        get_upload_order_details.processing_datetime = DateTime.Now;
                        dba.Entry(get_upload_order_details).State = EntityState.Modified;
                        dba.SaveChanges();
                    }
                    foreach (var item in getpaytmdetails)
                    {
                        if (kk == 0)
                        {
                            startdate = item.created_at.ToString();
                        }
                        if (kk == getpaytmdetails.Count - 1)
                        {
                            enddate = item.created_at.ToString();
                        }
                        kk++;

                        var chkSku = dba.tbl_inventory.Where(aa => aa.sku == item.Merchant_sku && aa.tbl_sellers_id == SellerId).FirstOrDefault();
                        if (chkSku == null)
                        {
                            tbl_inventory objInventory = new tbl_inventory();
                            objInventory.sku = item.Merchant_sku;
                            objInventory.tbl_sellers_id = SellerId;
                            
                            objInventory.tbl_marketplace_id = marketplaceID;
                            objInventory.item_name = item.sku_name;
                            objInventory.isactive = 1;
                            if (item.total_price != null)
                            {                               
                                    objInventory.selling_price = Convert.ToInt16(item.total_price);                               
                            }
                            dba.tbl_inventory.Add(objInventory);
                            dba.SaveChanges();
                        }

                        tbl_customer_details objcustumer = new tbl_customer_details();
                        objcustumer.State_Region = item.state;
                        objcustumer.shipping_Buyer_Name = item.customer_firstname;
                        objcustumer.last_name = item.customer_lastname;
                        objcustumer.City = item.customer_city;
                        objcustumer.Country_Code = "IN";
                        objcustumer.Postal_Code = item.customer_pincode;
                        objcustumer.Address_1 = item.customer_address;
                        objcustumer.customer_email = item.customer_email;
                        objcustumer.contact_no = item.phone;


                        objcustumer.tbl_seller_id = SellerId;
                        dba.tbl_customer_details.Add(objcustumer);
                        dba.SaveChanges();

                        tbl_sales_order objsales = new tbl_sales_order();
                        objsales.amazon_order_id = item.order_id;
                        objsales.buyer_name = item.customer_firstname;

                        objsales.tbl_Customer_Id = objcustumer.id;

                        //if (item.created_at.Contains('-'))
                        //{
                        //    objsales.purchase_date = DateTime.ParseExact(item.created_at, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                        //}
                        //else
                        //{
                        //    DateTime createdAtOut;
                        //    DateTime.TryParse(item.created_at, out createdAtOut);
                        //    objsales.purchase_date = createdAtOut;
                        //}

                        if (item.created_at.Contains('-') && item.created_at.Contains(':'))
                        {
                            var dateFormat = item.created_at.Split('-')[0].Length == 4 ? "yyyy-MM-dd" : "dd-MM-yyyy";
                            var timeFormat = item.created_at.Split(':').Length == 1 ? "HH:mm" : "HH:mm:ss";

                            objsales.purchase_date = DateTime.ParseExact(item.created_at, dateFormat + " " + timeFormat, CultureInfo.InvariantCulture);
                        }
                        else if (item.created_at.Contains('-'))
                        {
                            var dateFormat = item.created_at.Split('-')[0].Length == 4 ? "yyyy-MM-dd" : "dd-MM-yyyy";
                            objsales.purchase_date = DateTime.ParseExact(item.created_at, dateFormat, CultureInfo.InvariantCulture);
                        }
                        else
                        {
                            DateTime outDate;
                            DateTime.TryParse(item.created_at, out outDate);
                            objsales.purchase_date = outDate;
                        }

                        objsales.order_status = item.item_status_text;
                        objsales.no_of_itemshipped = Convert.ToInt32(item.qty_ordered);
                        objsales.currency_code = "INR";
                        objsales.bill_amount = Convert.ToDouble(item.total_price);//Convert.ToDouble(item.SellingPricePerItem);
                        objsales.tbl_Marketplace_Id = marketplaceID;
                        objsales.order_item_id = item.order_item_id;
                        objsales.created_on = DateTime.Now;
                        objsales.is_active = 1;
                        objsales.tbl_sellers_id = SellerId;
                        objsales.tbl_order_upload_id = id;
                        objsales.sales_channel = "Sale Customer PayTm";//"Paytm.in";

                        if (item.ship_by_date != "0000-00-00 00:00:00" && !string.IsNullOrEmpty(item.ship_by_date))
                        {
                            //if (item.ship_by_date.Contains('-'))
                            //{
                            //    objsales.last_updated_date = DateTime.ParseExact(item.ship_by_date, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                            //}
                            //else
                            //{
                            //    DateTime updated_at;
                            //    DateTime.TryParse(item.ship_by_date, out updated_at);
                            //    objsales.last_updated_date = updated_at;
                            //}

                            if (item.ship_by_date.Contains('-') && item.ship_by_date.Contains(':'))
                            {
                                var dateFormat = item.ship_by_date.Split('-')[0].Length == 4 ? "yyyy-MM-dd" : "dd-MM-yyyy";
                                var timeFormat = item.ship_by_date.Split(':').Length == 1 ? "HH:mm" : "HH:mm:ss";

                                objsales.last_updated_date = DateTime.ParseExact(item.ship_by_date, dateFormat + " " + timeFormat, CultureInfo.InvariantCulture);
                            }
                            else if (item.ship_by_date.Contains('-'))
                            {
                                var dateFormat = item.ship_by_date.Split('-')[0].Length == 4 ? "yyyy-MM-dd" : "dd-MM-yyyy";
                                objsales.last_updated_date = DateTime.ParseExact(item.ship_by_date, dateFormat, CultureInfo.InvariantCulture);
                            }
                            else
                            {
                                DateTime outDate;
                                DateTime.TryParse(item.ship_by_date, out outDate);
                                objsales.last_updated_date = outDate;
                            }
                        }

                        if (item.delivered_at != "0000-00-00 00:00:00" && !string.IsNullOrEmpty(item.delivered_at))
                        {
                            //if (item.delivered_at.Contains('-'))
                            //{
                            //    objsales.Latest_ShipDate = DateTime.ParseExact(item.delivered_at, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                            //}
                            //else
                            //{
                            //    DateTime updated_at;
                            //    DateTime.TryParse(item.delivered_at, out updated_at);
                            //    objsales.Latest_ShipDate = updated_at;
                            //}


                            if (item.delivered_at.Contains('-') && item.delivered_at.Contains(':'))
                            {
                                var dateFormat = item.delivered_at.Split('-')[0].Length == 4 ? "yyyy-MM-dd" : "dd-MM-yyyy";
                                var timeFormat = item.delivered_at.Split(':').Length == 1 ? "HH:mm" : "HH:mm:ss";

                                objsales.Latest_ShipDate = DateTime.ParseExact(item.delivered_at, dateFormat + " " + timeFormat, CultureInfo.InvariantCulture);
                            }
                            else if (item.delivered_at.Contains('-'))
                            {
                                var dateFormat = item.delivered_at.Split('-')[0].Length == 4 ? "yyyy-MM-dd" : "dd-MM-yyyy";
                                objsales.Latest_ShipDate = DateTime.ParseExact(item.delivered_at, dateFormat, CultureInfo.InvariantCulture);
                            }
                            else
                            {
                                DateTime outDate;
                                DateTime.TryParse(item.delivered_at, out outDate);
                                objsales.Latest_ShipDate = outDate;
                            }
                        }
                        if (item.shipped_at != "0000-00-00 00:00:00" && !string.IsNullOrEmpty(item.shipped_at))
                        {

                            //if (item.shipped_at.Contains('-'))
                            //{
                            //    objsales.earliest_ship_date = DateTime.ParseExact(item.shipped_at, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                            //}
                            //else
                            //{
                            //    DateTime updated_at;
                            //    DateTime.TryParse(item.shipped_at, out updated_at);
                            //    objsales.earliest_ship_date = updated_at;
                            //}

                            if (item.shipped_at.Contains('-') && item.shipped_at.Contains(':'))
                            {
                                var dateFormat = item.shipped_at.Split('-')[0].Length == 4 ? "yyyy-MM-dd" : "dd-MM-yyyy";
                                var timeFormat = item.shipped_at.Split(':').Length == 1 ? "HH:mm" : "HH:mm:ss";

                                objsales.earliest_ship_date = DateTime.ParseExact(item.shipped_at, dateFormat + " " + timeFormat, CultureInfo.InvariantCulture);
                            }
                            else if (item.shipped_at.Contains('-'))
                            {
                                var dateFormat = item.shipped_at.Split('-')[0].Length == 4 ? "yyyy-MM-dd" : "dd-MM-yyyy";
                                objsales.earliest_ship_date = DateTime.ParseExact(item.shipped_at, dateFormat, CultureInfo.InvariantCulture);
                            }
                            else
                            {
                                DateTime outDate;
                                DateTime.TryParse(item.shipped_at, out outDate);
                                objsales.earliest_ship_date = outDate;
                            }

                        }
                       
                        objsales.fullfillment_channel = "PayTm";
                        objsales.LastUpdatedDateUTC = DateTime.UtcNow;
                        objsales.merchant_id = item.merchant_id;
                        objsales.ship_service_category = item.service_type;

                        if (!string.IsNullOrEmpty(item.shipped_at) && item.shipped_at != "0000-00-00 00:00:00")
                        {
                            //if (item.shipped_at.Contains('-'))
                            //{
                            //    objsales.dispatch_afterdate = DateTime.ParseExact(item.shipped_at, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                            //}
                            //else
                            //{
                            //    DateTime updated_at;
                            //    DateTime.TryParse(item.shipped_at, out updated_at);
                            //    objsales.dispatch_afterdate = updated_at;
                            //}

                            if (item.shipped_at.Contains('-') && item.shipped_at.Contains(':'))
                            {
                                var dateFormat = item.shipped_at.Split('-')[0].Length == 4 ? "yyyy-MM-dd" : "dd-MM-yyyy";
                                var timeFormat = item.shipped_at.Split(':').Length == 1 ? "HH:mm" : "HH:mm:ss";

                                objsales.dispatch_afterdate = DateTime.ParseExact(item.shipped_at, dateFormat + " " + timeFormat, CultureInfo.InvariantCulture);
                            }
                            else if (item.shipped_at.Contains('-'))
                            {
                                var dateFormat = item.shipped_at.Split('-')[0].Length == 4 ? "yyyy-MM-dd" : "dd-MM-yyyy";
                                objsales.dispatch_afterdate = DateTime.ParseExact(item.shipped_at, dateFormat, CultureInfo.InvariantCulture);
                            }
                            else
                            {
                                DateTime outDate;
                                DateTime.TryParse(item.shipped_at, out outDate);
                                objsales.dispatch_afterdate = outDate;
                            }
                        }
                        string name1 = item.item_status_text.ToLower();
                        if (orderstatus_dict.ContainsKey(name1))
                        {
                            objsales.n_item_orderstatus = orderstatus_dict[name1];
                        }

                        dba.tbl_sales_order.Add(objsales);
                        dba.SaveChanges();

                        tbl_sales_order_details objsaledetails = new tbl_sales_order_details();

                        double productprice = item.total_price - item.shipping_amount;

                        objsaledetails.quantity_ordered = Convert.ToInt32(item.qty_ordered);
                        objsaledetails.quantity_shipped = Convert.ToInt32(item.qty_ordered);
                        objsaledetails.product_name = item.sku_name;                       
                        objsaledetails.order_item_id = item.order_item_id;
                        objsaledetails.shipping_price_curr_code = "INR";
                        objsaledetails.item_price_curr_code = "INR";
                        objsaledetails.item_tax_amount = Convert.ToDouble(item.GSTAmount);
                        //objsaledetails.item_price_amount = Convert.ToDouble(item.total_price);
                        objsaledetails.item_price_amount = productprice;//Convert.ToDouble(item.total_price);
                        objsaledetails.shipping_price_Amount = item.shipping_amount;

                        objsaledetails.is_active = 1;
                        objsaledetails.tbl_seller_id = SellerId;
                        objsaledetails.tbl_sales_order_id = objsales.id;
                        objsaledetails.status_updated_by = SellerId;
                        objsaledetails.status_updated_on = DateTime.Now;
                        objsaledetails.sku_no = item.Merchant_sku; //item.product_id;

                        objsaledetails.n_order_status_id = Convert.ToInt16(objsales.n_item_orderstatus);
                        objsaledetails.amazon_order_id = objsales.amazon_order_id;
                        if (!string.IsNullOrEmpty(item.shipped_at) && item.shipped_at != "0000-00-00 00:00:00")
                        {
                            //if (item.shipped_at.Contains('-'))
                            //{
                            //    objsaledetails.dispatchAfter_date = DateTime.ParseExact(item.shipped_at, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                            //}
                            //else
                            //{
                            //    DateTime updated_at;
                            //    DateTime.TryParse(item.shipped_at, out updated_at);
                            //    objsaledetails.dispatchAfter_date = updated_at;
                            //}

                            if (item.shipped_at.Contains('-') && item.shipped_at.Contains(':'))
                            {
                                var dateFormat = item.shipped_at.Split('-')[0].Length == 4 ? "yyyy-MM-dd" : "dd-MM-yyyy";
                                var timeFormat = item.shipped_at.Split(':').Length == 1 ? "HH:mm" : "HH:mm:ss";

                                objsaledetails.dispatchAfter_date = DateTime.ParseExact(item.shipped_at, dateFormat + " " + timeFormat, CultureInfo.InvariantCulture);
                            }
                            else if (item.shipped_at.Contains('-'))
                            {
                                var dateFormat = item.shipped_at.Split('-')[0].Length == 4 ? "yyyy-MM-dd" : "dd-MM-yyyy";
                                objsaledetails.dispatchAfter_date = DateTime.ParseExact(item.shipped_at, dateFormat, CultureInfo.InvariantCulture);
                            }
                            else
                            {
                                DateTime outDate;
                                DateTime.TryParse(item.shipped_at, out outDate);
                                objsaledetails.dispatchAfter_date = outDate;
                            }
                        }

                        if (!string.IsNullOrEmpty(item.delivered_at) && item.delivered_at != "0000-00-00 00:00:00")
                        {

                            //if (item.delivered_at.Contains('-'))
                            //{
                            //    objsaledetails.dispatch_bydate = DateTime.ParseExact(item.delivered_at, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                            //}
                            //else
                            //{
                            //    DateTime updated_at;
                            //    DateTime.TryParse(item.delivered_at, out updated_at);
                            //    objsaledetails.dispatch_bydate = updated_at;
                            //}

                            if (item.delivered_at.Contains('-') && item.delivered_at.Contains(':'))
                            {
                                var dateFormat = item.delivered_at.Split('-')[0].Length == 4 ? "yyyy-MM-dd" : "dd-MM-yyyy";
                                var timeFormat = item.delivered_at.Split(':').Length == 1 ? "HH:mm" : "HH:mm:ss";

                                objsaledetails.dispatch_bydate = DateTime.ParseExact(item.delivered_at, dateFormat + " " + timeFormat, CultureInfo.InvariantCulture);
                            }
                            else if (item.delivered_at.Contains('-'))
                            {
                                var dateFormat = item.delivered_at.Split('-')[0].Length == 4 ? "yyyy-MM-dd" : "dd-MM-yyyy";
                                objsaledetails.dispatch_bydate = DateTime.ParseExact(item.delivered_at, dateFormat, CultureInfo.InvariantCulture);
                            }
                            else
                            {
                                DateTime outDate;
                                DateTime.TryParse(item.delivered_at, out outDate);
                                objsaledetails.dispatch_bydate = outDate;
                            }
                        }                       
                        objsaledetails.is_tax_calculated = 0;
                        objsaledetails.tax_updatedby_taxfile = 0;
                        objsaledetails.product_id = item.product_id;
                        objsaledetails.promo_code = item.promo_code;
                        dba.tbl_sales_order_details.Add(objsaledetails);
                        dba.SaveChanges();

                        #region tax calculated

                        tbl_tax objtax = new tbl_tax();
                        objtax.tbl_seller_id = SellerId;
                        objtax.tbl_referneced_id = objsaledetails.id;
                        objtax.reference_type = 3;
                        objtax.isactive = 1;                       
                        dba.tbl_tax.Add(objtax);
                        dba.SaveChanges();

                        #endregion





                        //}
                    }// end of foreach

                    string connectionstring = ConfigurationManager.ConnectionStrings["SellerConnection"].ConnectionString;
                    MySqlConnection con = new MySqlConnection(connectionstring);

                    int uniqueupload = 0;
                    int diff_order = 0;
                    int total_order = 0;
                    var get_unique_count_details = "SELECT count(DISTINCT aa.id) FROM(SELECT DISTINCT Order_Id AS id FROM `tbl_settlement_order` where tbl_seller_id = " + SellerId + " UNION ALL SELECT DISTINCT amazon_order_id AS id FROM `tbl_sales_order` where tbl_sellers_id =" + SellerId + " ) AS aa";
                    MySqlCommand cmd = new MySqlCommand(get_unique_count_details, con);
                    con.Open();
                    MySqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        dr.Read();
                        uniqueupload = Convert.ToInt32(dr[0]);
                    }
                    cmd.Dispose();
                    con.Close();

                    MySqlConnection con1 = new MySqlConnection(connectionstring);
                    var get_unique_orderby_id = "SELECT count( aa.id )FROM (SELECT tbl_order_upload_id AS id FROM `tbl_sales_order` where tbl_sellers_id =" + SellerId + " && tbl_order_upload_id = " + id + ") AS aa";
                    MySqlCommand cmd2 = new MySqlCommand(get_unique_orderby_id, con1);
                    con1.Open();
                    MySqlDataReader dr1 = cmd2.ExecuteReader();
                    if (dr1.HasRows)
                    {
                        dr1.Read();
                        total_order = Convert.ToInt32(dr1[0]);
                    }
                    cmd2.Dispose();
                    con1.Close();

                    var get_balance_details = db.tbl_user_login.Where(aa => aa.tbl_sellers_id == SellerId).FirstOrDefault();
                    if (get_balance_details != null)
                    {
                        var last_order = get_balance_details.total_orders;
                        diff_order = Convert.ToInt16(uniqueupload - last_order);
                        var plan_rate = get_balance_details.applied_plan_rate * diff_order;
                        get_balance_details.wallet_balance = get_balance_details.wallet_balance - Convert.ToDouble(plan_rate);
                        int totalOrder = Convert.ToInt16(diff_order);
                        if (get_balance_details.total_orders != null)
                            totalOrder = totalOrder + Convert.ToInt16(get_balance_details.total_orders);
                        get_balance_details.total_orders = totalOrder;
                        db.Entry(get_balance_details).State = EntityState.Modified;
                        db.SaveChangesAsync();
                    }
                    if (get_upload_order_details != null)
                    {
                        get_upload_order_details.new_order_uploaded = total_order;
                        if (!string.IsNullOrEmpty(startdate))
                        {
                            //if (startdate.Contains('-'))
                            //{
                            //    get_upload_order_details.from_date = DateTime.ParseExact(startdate, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                            //}
                            //else
                            //{
                            //    DateTime updated_at;
                            //    DateTime.TryParse(startdate, out updated_at);
                            //    get_upload_order_details.from_date = updated_at;
                            //}

                            if (startdate.Contains('-') && startdate.Contains(':'))
                            {
                                var dateFormat = startdate.Split('-')[0].Length == 4 ? "yyyy-MM-dd" : "dd-MM-yyyy";
                                var timeFormat = startdate.Split(':').Length == 1 ? "HH:mm" : "HH:mm:ss";

                                get_upload_order_details.from_date = DateTime.ParseExact(startdate, dateFormat + " " + timeFormat, CultureInfo.InvariantCulture);
                            }
                            else if (startdate.Contains('-'))
                            {
                                var dateFormat = startdate.Split('-')[0].Length == 4 ? "yyyy-MM-dd" : "dd-MM-yyyy";
                                get_upload_order_details.from_date = DateTime.ParseExact(startdate, dateFormat, CultureInfo.InvariantCulture);
                            }
                            else
                            {
                                DateTime outDate;
                                DateTime.TryParse(startdate, out outDate);
                                get_upload_order_details.from_date = outDate;
                            }
                        }

                        if (!string.IsNullOrEmpty(enddate))
                        {
                            //if (enddate.Contains('-'))
                            //{
                            //    get_upload_order_details.to_date = DateTime.ParseExact(enddate, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                            //}
                            //else
                            //{
                            //    DateTime updated_at;
                            //    DateTime.TryParse(enddate, out updated_at);
                            //    get_upload_order_details.to_date = updated_at;
                            //}

                            if (enddate.Contains('-') && enddate.Contains(':'))
                            {
                                var dateFormat = enddate.Split('-')[0].Length == 4 ? "yyyy-MM-dd" : "dd-MM-yyyy";
                                var timeFormat = enddate.Split(':').Length == 1 ? "HH:mm" : "HH:mm:ss";

                                get_upload_order_details.to_date = DateTime.ParseExact(enddate, dateFormat + " " + timeFormat, CultureInfo.InvariantCulture);
                            }
                            else if (enddate.Contains('-'))
                            {
                                var dateFormat = enddate.Split('-')[0].Length == 4 ? "yyyy-MM-dd" : "dd-MM-yyyy";
                                get_upload_order_details.to_date = DateTime.ParseExact(enddate, dateFormat, CultureInfo.InvariantCulture);
                            }
                            else
                            {
                                DateTime outDate;
                                DateTime.TryParse(enddate, out outDate);
                                get_upload_order_details.to_date = outDate;
                            }
                        }

                        get_upload_order_details.status = "Completed";
                        get_upload_order_details.completed_datetime = DateTime.Now;
                        dba.Entry(get_upload_order_details).State = EntityState.Modified;
                    }
                    dba.SaveChangesAsync();

                    if (getpaytmdetails != null)
                    {
                        foreach (var dd in getpaytmdetails)
                        {
                            dd.Status = 1;
                            dba.Entry(dd).State = EntityState.Modified;
                            dba.SaveChanges();

                        }
                    }

                }
            }
            catch (Exception ex)
            {
            }
            return success;
        }

        public string readpaytmorder1(string strFilePath, int marketplaceID, int id, int SellerId)
        {
            string success = "S";
            try
            {
                int i = 0;
                List<PaytmOrder> lstFile = new List<PaytmOrder>();
                PaytmOrder objfile = new PaytmOrder();
                using (var reader = new StreamReader(strFilePath))
                {
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        if (!string.IsNullOrEmpty(line))
                        {
                            if (i > 0)
                            {
                                i++;
                                var values = line;

                                MatchCollection matches = new Regex("((?<=\")[^\"]*(?=\"(,|$)+)|(?<=,|^)[^,\"]*(?=,|$))").Matches(values);


                                objfile = new PaytmOrder();

                                string abc = matches[0].ToString();
                                objfile.Itemid = matches[0].ToString();

                                string iii = matches[1].ToString();
                                objfile.OrderId = matches[1].ToString();
                                objfile.ProductId = matches[2].ToString();
                                objfile.MerchantId = matches[3].ToString();
                                objfile.MerchantSku = matches[4].ToString();
                                objfile.ProductName = matches[5].ToString();
                                objfile.QtyOrdered = matches[6].ToString();
                                objfile.FulfillmentId = matches[7].ToString();
                                objfile.PromoCode = matches[8].ToString();
                                objfile.PromoDescription = matches[9].ToString();
                                objfile.Mrp = Convert.ToDouble(matches[10].ToString());
                                objfile.Discount = Convert.ToDouble(matches[11].ToString());

                                objfile.SellingPrice = matches[12].ToString();
                                objfile.ShippingAmount = matches[13].ToString();
                                objfile.ShipByDate = matches[14].ToString();

                                objfile.Attributes = matches[15].ToString();

                                objfile.OrderDate = matches[16].ToString();

                                objfile.ItemStatus = matches[17].ToString();
                                objfile.PaymentStatus = matches[18].ToString();
                                objfile.CustomerEmail = matches[19].ToString();
                                objfile.CustomerFirstname = matches[20].ToString();
                                objfile.CustomerLastname = matches[21].ToString();
                                objfile.State = matches[22].ToString();
                                objfile.Pincode = matches[23].ToString();
                                objfile.Address = matches[24].ToString();
                                objfile.City = matches[25].ToString();
                                objfile.Phone = matches[26].ToString();

                                objfile.ShipmentCreatedDate = matches[27].ToString();
                                objfile.AWB = matches[28].ToString();
                                objfile.TrackingUrl = matches[29].ToString();
                                objfile.LogisticsPartner = matches[30].ToString();
                                objfile.IMEI = matches[31].ToString();
                                objfile.ShippedAt = matches[32].ToString();
                                objfile.DeliveredAt = matches[33].ToString();
                                objfile.ReturnedAt = matches[34].ToString();
                                objfile.ShipperId = matches[35].ToString();
                                objfile.ManifestId = matches[36].ToString();
                                objfile.SLAextended = matches[37].ToString();
                                objfile.WarehouseId = matches[38].ToString();
                                objfile.MerchantOverdueDate = matches[39].ToString();
                                objfile.ParentOrderId = matches[40].ToString();
                                objfile.ParentItemId = matches[41].ToString();
                                objfile.ChildOrderId = matches[42].ToString();
                                objfile.ChildItemId = matches[43].ToString();
                                objfile.MaxRefund = matches[44].ToString();
                                objfile.MrAt = matches[45].ToString();
                                objfile.DeliveryType = matches[46].ToString();
                                objfile.CustomerGst = matches[47].ToString();
                                objfile.MetaData = matches[48].ToString();
                                objfile.FulfillmentReq = matches[49].ToString();


                                lstFile.Add(objfile);
                            }
                            i++;
                        }

                    }
                }
                if (lstFile.Count > 0)
                {
                    //success = SavePaytmOrder(lstFile, marketplaceID, id, SellerId);
                }
                else
                {
                    success = "Em";
                }
            }// end of try block
            catch (Exception Ex)
            {
            }// end of catch block
            return success;
        }

        public string SavePaytmOrder1(List<PaytmOrder> lstFile, int marketplaceID, int id, int SellerId)
        {
            string success = "S";
            try
            {
                int count = 0;
                int kk = 0;
                string startdate = "", enddate = "";
                var getdetails = dba.tbl_sales_order_status.Where(aa => aa.is_active == 0).ToList();
                Dictionary<string, int> orderstatus_dict = new Dictionary<string, int>();
                foreach (var item1 in getdetails)
                {
                    string name = item1.sales_order_status.ToLower();
                    orderstatus_dict.Add(name, item1.id);
                }
                if (lstFile != null)
                {
                    var get_upload_order_details = dba.tbl_order_upload.Where(aa => aa.tbl_seller_id == SellerId && aa.id == id && aa.tbl_Marketplace_id == marketplaceID).FirstOrDefault();
                    if (get_upload_order_details != null)
                    {
                        get_upload_order_details.status = "Processing";
                        get_upload_order_details.processing_datetime = DateTime.Now;
                        dba.Entry(get_upload_order_details).State = EntityState.Modified;
                        dba.SaveChanges();
                    }
                    foreach (var item in lstFile)
                    {
                        if (kk == 0)
                        {
                            startdate = item.OrderDate;
                        }
                        if (kk == lstFile.Count - 1)
                        {
                            enddate = item.OrderDate;
                        }
                        kk++;
                        //var get_saleorder = dba.tbl_sales_order.Where(aa => aa.tbl_sellers_id == SellerId && aa.amazon_order_id == item.OrderId && aa.order_status == item.OrderState).FirstOrDefault();
                        //if (get_saleorder == null)
                        //{

                        tbl_customer_details objcustumer = new tbl_customer_details();
                        objcustumer.State_Region = item.State;
                        objcustumer.shipping_Buyer_Name = item.CustomerFirstname;
                        objcustumer.last_name = item.CustomerLastname;
                        objcustumer.City = item.City;
                        objcustumer.Country_Code = "IN";
                        objcustumer.Postal_Code = item.Pincode;
                        objcustumer.Address_1 = item.Address;
                        objcustumer.contact_no = item.Phone;
                        objcustumer.tbl_seller_id = SellerId;
                        dba.tbl_customer_details.Add(objcustumer);
                        dba.SaveChanges();

                        tbl_sales_order objsales = new tbl_sales_order();
                        objsales.amazon_order_id = item.OrderId;
                        objsales.buyer_name = item.CustomerFirstname;
                        objsales.buyer_emil = item.CustomerEmail;
                        objsales.tbl_Customer_Id = objcustumer.id;
                        objsales.purchase_date = Convert.ToDateTime(item.OrderDate);
                        objsales.order_status = item.ItemStatus;
                        objsales.no_of_itemshipped = Convert.ToInt32(item.QtyOrdered);
                        objsales.currency_code = "INR";
                        objsales.bill_amount = Convert.ToDouble(item.SellingPrice);//Convert.ToDouble(item.SellingPricePerItem);
                        objsales.tbl_Marketplace_Id = marketplaceID;
                        objsales.order_item_id = item.Itemid;
                        objsales.created_on = DateTime.Now;
                        objsales.is_active = 1;
                        objsales.tbl_sellers_id = SellerId;
                        objsales.tbl_order_upload_id = id;
                        objsales.sales_channel = "Paytm.in";
                        objsales.last_updated_date = Convert.ToDateTime(item.ShipByDate);
                        string Deliverd = item.DeliveredAt;
                        if (item.DeliveredAt != "0000-00-00 00:00:00" && item.DeliveredAt != "")
                        {
                            objsales.Latest_ShipDate = Convert.ToDateTime(item.DeliveredAt);
                        }
                        if (item.ShippedAt != "0000-00-00 00:00:00" && item.ShippedAt != "")
                        {
                            objsales.earliest_ship_date = Convert.ToDateTime(item.ShippedAt);
                        }
                        if (item.ReturnedAt != "0000-00-00 00:00:00" && item.ReturnedAt != "")
                        {
                            objsales.returned_at = Convert.ToDateTime(item.ReturnedAt);
                        }
                        objsales.fullfillment_channel = "PayTm";
                        objsales.LastUpdatedDateUTC = DateTime.UtcNow;
                        objsales.merchant_id = item.MerchantId;
                        objsales.ship_service_category = item.DeliveryType;
                        if (item.ShipmentCreatedDate != "")
                        {
                            objsales.dispatch_afterdate = Convert.ToDateTime(item.ShipmentCreatedDate);
                        }

                        string name1 = item.ItemStatus.ToLower();
                        if (orderstatus_dict.ContainsKey(name1))
                        {
                            objsales.n_item_orderstatus = orderstatus_dict[name1];
                        }

                        dba.tbl_sales_order.Add(objsales);
                        dba.SaveChanges();

                        tbl_sales_order_details objsaledetails = new tbl_sales_order_details();

                        objsaledetails.quantity_ordered = Convert.ToInt32(item.QtyOrdered);
                        objsaledetails.quantity_shipped = Convert.ToInt32(item.QtyOrdered);
                        objsaledetails.product_name = item.ProductName;

                        objsaledetails.sku_no = item.MerchantSku;
                        objsaledetails.order_item_id = item.Itemid;
                        objsaledetails.shipping_price_curr_code = "INR";
                        objsaledetails.item_price_curr_code = "INR";
                        objsaledetails.item_price_amount = Convert.ToDouble(item.SellingPrice);
                        objsaledetails.shipping_price_Amount = Convert.ToDouble(item.ShippingAmount);
                        objsaledetails.is_active = 1;
                        objsaledetails.tbl_seller_id = SellerId;
                        objsaledetails.tbl_sales_order_id = objsales.id;
                        objsaledetails.status_updated_by = SellerId;
                        objsaledetails.status_updated_on = DateTime.Now;

                        objsaledetails.n_order_status_id = Convert.ToInt16(objsales.n_item_orderstatus);
                        objsaledetails.amazon_order_id = objsales.amazon_order_id;
                        if (item.ShippedAt != "0000-00-00 00:00:00" && item.ShippedAt != "")
                        {
                            objsaledetails.dispatchAfter_date = Convert.ToDateTime(item.ShippedAt);
                        }
                        if (item.DeliveredAt != "0000-00-00 00:00:00" && item.DeliveredAt != "")
                        {
                            objsaledetails.dispatch_bydate = Convert.ToDateTime(item.DeliveredAt);
                        }
                        objsaledetails.is_tax_calculated = 0;
                        objsaledetails.tax_updatedby_taxfile = 0;
                        objsaledetails.product_id = item.ProductId;
                        objsaledetails.fulfillment_id = item.FulfillmentId;
                        objsaledetails.promo_code = item.PromoCode;
                        objsaledetails.promo_description = item.PromoDescription;
                        objsaledetails.AWB = item.AWB;
                        objsaledetails.tracking_url = item.TrackingUrl;
                        objsaledetails.Logistics_partner = item.LogisticsPartner;
                        objsaledetails.warehouse_id = item.WarehouseId;
                        objsaledetails.manifest_id = item.ManifestId;
                        if (item.ShipperId != "")
                        {
                            objsaledetails.shipper_id = Convert.ToInt16(item.ShipperId);
                        }


                        dba.tbl_sales_order_details.Add(objsaledetails);
                        dba.SaveChanges();

                        //}
                    }// end of foreach

                    string connectionstring = ConfigurationManager.ConnectionStrings["SellerConnection"].ConnectionString;
                    MySqlConnection con = new MySqlConnection(connectionstring);

                    int uniqueupload = 0;
                    int diff_order = 0;
                    int total_order = 0;
                    var get_unique_count_details = "SELECT count(DISTINCT aa.id) FROM(SELECT DISTINCT Order_Id AS id FROM `tbl_settlement_order` where tbl_seller_id = " + SellerId + " UNION ALL SELECT DISTINCT amazon_order_id AS id FROM `tbl_sales_order` where tbl_sellers_id =" + SellerId + " ) AS aa";
                    MySqlCommand cmd = new MySqlCommand(get_unique_count_details, con);
                    con.Open();
                    MySqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        dr.Read();
                        uniqueupload = Convert.ToInt32(dr[0]);
                    }
                    cmd.Dispose();
                    con.Close();

                    MySqlConnection con1 = new MySqlConnection(connectionstring);
                    var get_unique_orderby_id = "SELECT count( aa.id )FROM (SELECT tbl_order_upload_id AS id FROM `tbl_sales_order` where tbl_sellers_id =" + SellerId + " && tbl_order_upload_id = " + id + ") AS aa";
                    MySqlCommand cmd2 = new MySqlCommand(get_unique_orderby_id, con1);
                    con1.Open();
                    MySqlDataReader dr1 = cmd2.ExecuteReader();
                    if (dr1.HasRows)
                    {
                        dr1.Read();
                        total_order = Convert.ToInt32(dr1[0]);
                    }
                    cmd2.Dispose();
                    con1.Close();

                    var get_balance_details = db.tbl_user_login.Where(aa => aa.tbl_sellers_id == SellerId).FirstOrDefault();
                    if (get_balance_details != null)
                    {
                        var last_order = get_balance_details.total_orders;
                        diff_order = Convert.ToInt16(uniqueupload - last_order);
                        var plan_rate = get_balance_details.applied_plan_rate * diff_order;
                        get_balance_details.wallet_balance = get_balance_details.wallet_balance - Convert.ToDouble(plan_rate);
                        int totalOrder = Convert.ToInt16(diff_order);
                        if (get_balance_details.total_orders != null)
                            totalOrder = totalOrder + Convert.ToInt16(get_balance_details.total_orders);
                        get_balance_details.total_orders = totalOrder;
                        db.Entry(get_balance_details).State = EntityState.Modified;
                        db.SaveChangesAsync();
                    }
                    if (get_upload_order_details != null)
                    {
                        get_upload_order_details.new_order_uploaded = total_order;
                        get_upload_order_details.from_date = Convert.ToDateTime(startdate);
                        get_upload_order_details.to_date = Convert.ToDateTime(enddate);
                        get_upload_order_details.status = "Completed";
                        get_upload_order_details.completed_datetime = DateTime.Now;
                        dba.Entry(get_upload_order_details).State = EntityState.Modified;
                    }
                    dba.SaveChangesAsync();

                }
            }
            catch (Exception ex)
            {
            }
            return success;
        }

        #endregion


        #endregion


        #region Upload Tax File
        /// <summary>
        /// this is for Tax File
        /// </summary>
        /// <returns></returns>
        public void TaxCrown()
        {
            int seller_id = 0;
            try
            {
                var get_details = dba.tbl_order_upload.Where(a => a.type == 2 && a.source == 1 && a.status == "Queued").ToList();
                if (get_details != null)
                {
                    foreach (var item in get_details)
                    {
                        seller_id = Convert.ToInt16(item.tbl_seller_id);
                        string filename = item.filename;
                        int marketplaceID = Convert.ToInt32(item.tbl_Marketplace_id);
                        int id = item.id;
                        int addorder = Convert.ToInt16(item.checkstatus);

                        string path = System.IO.Path.Combine(Server.MapPath("~/UploadExcel/" + seller_id + "/Tax/" + filename));
                        if (marketplaceID == 3)
                        {
                            Bulkcsv(path, 1, seller_id, marketplaceID, addorder, filename, id);
                        }
                        else if(marketplaceID == 5)
                        {
                            BulkcsvForPayTm(path, filename, marketplaceID, id,seller_id);
                        }

                        string success = "S";

                        ViewBag.Message = success == "S" ? "File uploaded successfully." : "Unable to upload. " + success;
                    }
                }
            }
            catch (Exception ex)
            {
                Writelog log = new Writelog();
                log.write_exception_log(seller_id.ToString(), "CronJobController", "TaxCrown", DateTime.Now, ex);
            }
            // return View("Index");
        }

        #region For Amazon
        public string Bulkcsv(string strFilePath, int? strType, int? SellerId, int? marketplaceID, int? addorder, string strFileName, int? upload_id)
        {
            string success = "S";
            string stringSeparators = "\",\"";
            List<tbl_seller_tax_file> lstTaxFile = new List<tbl_seller_tax_file>();
            tbl_seller_tax_file obj = new tbl_seller_tax_file();
            string strAmt = "";
            int i = 0;
            int intAddIntoList = 0;
            int totalcolumn = 65;
            int indexStart = 34;
            try
            {
                using (var reader = new StreamReader(strFilePath))
                {
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        if (!string.IsNullOrEmpty(line))
                        {
                            if (i > 0)
                            {
                                i++;
                                var values = line.Split(new string[] { stringSeparators }, StringSplitOptions.None); //line.Split((',\"', );

                                if (values.Length == 1)
                                {
                                    stringSeparators = ",";
                                    values = line.Split(new string[] { stringSeparators }, StringSplitOptions.None);
                                    totalcolumn = 66;
                                    indexStart = 0;
                                }
                                if (values.Length > totalcolumn) //(values.Length < 65 || values.Length > 65)
                                {
                                    continue;
                                }
                                else //if (!string.IsNullOrEmpty(values[0].ToString().Replace("\"", "")))
                                {
                                    if (values.Length >= totalcolumn && intAddIntoList == 0 && !string.IsNullOrEmpty(values[0].ToString().Replace("\"", "")))
                                    {
                                        obj = new tbl_seller_tax_file();
                                        strAmt = values[37].ToString().Replace("\"", "");
                                        obj.order_id = values[0].ToString().Replace("\"", "");
                                        string str = values[1].ToString().Replace("\"", "");
                                        if (str != "Order_Date")
                                        {
                                            obj.order_date = DateTime.Parse(str);
                                        }
                                        obj.shipment_id = values[2].ToString();
                                        str = values[3].ToString().Replace("\"", "");

                                        if (str != "Shipment_Date")
                                        {
                                            obj.shipment_date = DateTime.Parse(str);
                                        }
                                        str = values[4].ToString().Replace("\"", "").Substring(0, 16);

                                        if (str != "Tax_Calculated_D")
                                        {
                                            obj.tax_calculated_date = DateTime.Parse(str);
                                        }
                                        str = values[5].ToString().Replace("\"", "");
                                        if (str != "Posted_Date")
                                        {
                                            obj.posted_date = DateTime.Parse(str);
                                        }

                                        obj.marketplace = values[6].ToString().Replace("\"", "");
                                        obj.tax_invoice_number = values[9].ToString().Replace("\"", "");
                                        obj.fulfillment = values[11].ToString().Replace("\"", "");
                                        obj.asin = values[12].ToString().Replace("\"", "");
                                        obj.sku = values[13].ToString().Replace("\"", "");
                                        obj.transaction_type = values[14].ToString();
                                        obj.product_tax_code = values[15].ToString().Replace("\"", "");

                                        string aa = values[16].ToString().Replace("\"", "");
                                        if (aa != "Quantity")
                                        {
                                            obj.quantity = string.IsNullOrEmpty(values[16].ToString().Replace("\"", "")) ? 0 : int.Parse(values[16].ToString().Replace("\"", ""));
                                        }

                                        obj.currency = values[17].ToString().Replace("\"", "");

                                        string display = values[18].ToString().Replace("\"", "");
                                        if (display != "Display_Price")
                                        {
                                            obj.display_price = string.IsNullOrEmpty(values[18].ToString().Replace("\"", "")) ? 0 : double.Parse(values[18].ToString().Replace("\"", ""));
                                        }
                                        string displayprice = values[19].ToString().Replace("\"", "");
                                        if (displayprice != "Is_Display_Price_TaxInclusive")
                                        {
                                            obj.is_display_price_taxinclusive = values[19].ToString().Replace("\"", "");
                                        }

                                        string finalprice = values[20].ToString().Replace("\"", "");
                                        if (finalprice != "Final_TaxInclusive_Selling_Price")
                                        {
                                            obj.final_taxinclusive_selling_price = string.IsNullOrEmpty(values[20].ToString().Replace("\"", "")) ? 0 : double.Parse(values[20].ToString().Replace("\"", ""));
                                        }
                                        string TaxSelling = values[21].ToString().Replace("\"", "");
                                        if (TaxSelling != "TaxExclusive_Selling_Price")
                                        {

                                            obj.taxexclusive_selling_price = string.IsNullOrEmpty(values[21].ToString().Replace("\"", "")) ? 0 : double.Parse(values[21].ToString().Replace("\"", ""));
                                        }
                                        //Total_Tax
                                        string totaltax = values[22].ToString().Replace("\"", "");
                                        if (totaltax != null)
                                        {
                                            obj.total_tax = string.IsNullOrEmpty(values[22].ToString().Replace("\"", "")) ? 0 : double.Parse(values[22].ToString().Replace("\"", ""));
                                        }
                                        //--------------------Add address--from-city,from-state,from-pincode,to-city,to-state,to-pincode-----------//

                                        obj.ship_from_city = values[23].ToString().Replace("\"", "");
                                        obj.ship_from_state = values[24].ToString().Replace("\"", "");
                                        obj.ship_from_postal_code = values[26].ToString().Replace("\"", "");
                                        obj.ship_to_city = values[28].ToString().Replace("\"", "");
                                        obj.ship_to_state = values[29].ToString().Replace("\"", "");
                                        obj.ship_to_postal_code = values[31].ToString().Replace("\"", "");

                                        //--------------------------------------End-----------------------------------//
                                        obj.tax_address_role = values[34].ToString().Replace("\"", "");
                                        obj.jurisdiction_level = values[35].ToString().Replace("\"", "");
                                        obj.jurisdiction_name = values[36].ToString().Replace("\"", "");
                                        strAmt = values[37].ToString().Replace("\"", "");
                                        obj.tax_amount = string.IsNullOrEmpty(values[37].ToString().Replace("\"", "")) ? 0 : double.Parse(values[37].ToString().Replace("\"", ""));
                                        obj.taxed_jurisdiction_tax_rate = string.IsNullOrEmpty(values[38].ToString().Replace("\"", "")) ? 0 : double.Parse(values[38].ToString().Replace("\"", ""));
                                        obj.tax_type = values[39].ToString().Replace("\"", "");
                                        obj.tax_calculation_reason_code = values[40].ToString().Replace("\"", "");
                                        obj.nontaxable_amount = string.IsNullOrEmpty(values[41].ToString().Replace("\"", "")) ? 0 : double.Parse(values[41].ToString().Replace("\"", ""));
                                        obj.taxable_amount = string.IsNullOrEmpty(values[42].ToString().Replace("\"", "")) ? 0 : double.Parse(values[42].ToString().Replace("\"", ""));
                                        obj.promo_taxinclusive_amount = string.IsNullOrEmpty(values[43].ToString().Replace("\"", "")) ? 0 : double.Parse(values[43].ToString().Replace("\"", ""));
                                        obj.is_display_promo_tax_inclusive = values[44].ToString().Replace("\"", "");
                                        obj.promo_type = values[45].ToString().Replace("\"", "");
                                        obj.promo_taxexclusive_amount = string.IsNullOrEmpty(values[46].ToString().Replace("\"", "")) ? 0 : double.Parse(values[46].ToString().Replace("\"", ""));
                                        obj.promo_amount_tax = string.IsNullOrEmpty(values[47].ToString().Replace("\"", "")) ? 0 : double.Parse(values[47].ToString().Replace("\"", ""));
                                        intAddIntoList = 1;
                                    }
                                    else if (values.Length <= totalcolumn && intAddIntoList == 1)
                                    {

                                        obj.tax_address_role_blank_row = values[34 - indexStart].ToString().Replace("\"", "");
                                        obj.jurisdiction_level_blank_row = values[35 - indexStart].ToString().Replace("\"", "");
                                        obj.jurisdiction_name_blank_row = values[36 - indexStart].ToString().Replace("\"", "");
                                        strAmt = values[37 - indexStart].ToString().Replace("\"", "");
                                        obj.tax_amount_blank_row = string.IsNullOrEmpty(values[37 - indexStart].ToString().Replace("\"", "")) ? 0 : double.Parse(values[37 - indexStart].ToString().Replace("\"", ""));
                                        obj.taxed_jurisdiction_tax_rate_blank_row = string.IsNullOrEmpty(values[38 - indexStart].ToString().Replace("\"", "")) ? 0 : double.Parse(values[38 - indexStart].ToString().Replace("\"", ""));
                                        obj.tax_type_blank_row = values[39 - indexStart].ToString().Replace("\"", "");
                                        obj.tax_calculation_reason_code_blank_row = values[40 - indexStart].ToString().Replace("\"", "");
                                        obj.nontaxable_amount_blank_row = string.IsNullOrEmpty(values[41 - indexStart].ToString().Replace("\"", "")) ? 0 : double.Parse(values[41 - indexStart].ToString().Replace("\"", ""));
                                        obj.taxable_amount_blank_row = string.IsNullOrEmpty(values[42 - indexStart].ToString().Replace("\"", "")) ? 0 : double.Parse(values[42 - indexStart].ToString().Replace("\"", ""));
                                        obj.promo_taxinclusive_amount_blank_row = string.IsNullOrEmpty(values[43 - indexStart].ToString().Replace("\"", "")) ? 0 : double.Parse(values[43 - indexStart].ToString().Replace("\"", ""));
                                        obj.is_display_promo_tax_inclusive_blank_row = values[44 - indexStart].ToString().Replace("\"", "");
                                        obj.promo_type_blank_row = values[45 - indexStart].ToString().Replace("\"", "");
                                        obj.promo_taxexclusive_amount_blank_row = string.IsNullOrEmpty(values[46 - indexStart].ToString().Replace("\"", "")) ? 0 : double.Parse(values[46 - indexStart].ToString().Replace("\"", ""));
                                        obj.promo_amount_tax_blank_row = string.IsNullOrEmpty(values[47 - indexStart].ToString().Replace("\"", "")) ? 0 : double.Parse(values[47 - indexStart].ToString().Replace("\"", ""));

                                        if (intAddIntoList == 1)
                                        {
                                            lstTaxFile.Add(obj);
                                            intAddIntoList = 0;
                                        }
                                    }
                                }
                            }
                            i++;
                        }
                    }
                }
                dba.Configuration.AutoDetectChangesEnabled = false;
                if (lstTaxFile.Count > 0)
                {
                    if (addorder > 0)
                    {
                        //////////////////// ADD INTO ORDER MASTER IN TABLE FORCEFULLY REQUET BY USER ////////////////
                        success = Insert_Order_If_Not_Exist(lstTaxFile, strType, SellerId, marketplaceID, strFileName, upload_id);
                    }
                    //////////////// Update Database File ///////////////////
                    success = Update_Sales_Detail(lstTaxFile, strType, SellerId, marketplaceID, addorder, strFileName, upload_id);
                }
                else
                {
                    success = "Em";
                }
            }
            catch (Exception ex)
            {
                Writelog log = new Writelog();
                log.write_exception_log(SellerId.ToString(), "CronJobController", "Bulkcsv", DateTime.Now, ex);
                success = "Ex";
            }

            return success;
        }

        public string Insert_Order_If_Not_Exist(List<tbl_seller_tax_file> lstObj, int? strType, int? SellerId, int? marketplaceID, string strFileName, int? upload_id)
        {
            string success = "S";
            int iEx = 0;
            int uniqueupload = 0;
            string startdate = "";
            string enddate = "";
            //var get_order_uploaddetails = dba.tbl_order_upload.Where(a => a.tbl_seller_id == SellerId && a.filename == strFileName).FirstOrDefault();
            //if (get_order_uploaddetails == null)
            //{
            try
            {

                var get_upload_order_details = dba.tbl_order_upload.Where(aa => aa.tbl_seller_id == SellerId && aa.id == upload_id).FirstOrDefault();
                if (get_upload_order_details != null)
                {
                    get_upload_order_details.status = "Processing";
                    get_upload_order_details.processing_datetime = DateTime.Now;
                    dba.Entry(get_upload_order_details).State = EntityState.Modified;
                    dba.SaveChanges();
                }
                var list_order = lstObj.Where(item => item.is_display_promo_tax_inclusive.ToLower() != "y").ToList();

                tbl_sales_order objsale_order = null;
                foreach (var item in list_order)
                {

                    var get_sale_order_id = dba.tbl_sales_order.Where(a => a.tbl_sellers_id == SellerId && a.amazon_order_id == item.order_id).FirstOrDefault();
                    if (get_sale_order_id == null)
                    {
                        iEx = 1;
                        objsale_order = new tbl_sales_order();
                        objsale_order.amazon_order_id = item.order_id;
                        objsale_order.created_on = DateTime.Now;
                        objsale_order.tbl_sellers_id = Convert.ToInt16(SellerId);
                        objsale_order.is_active = 1;
                        objsale_order.sales_channel = item.marketplace;
                        objsale_order.purchase_date = item.order_date;
                        objsale_order.last_updated_date = item.shipment_date;
                        objsale_order.tbl_Marketplace_Id = Convert.ToInt16(marketplaceID);
                        objsale_order.tbl_order_upload_id = upload_id;

                        objsale_order.bill_amount = item.display_price;
                        if (item.fulfillment == "AFN")
                        {
                            objsale_order.n_fullfilled_id = 1;
                        }
                        else
                        {
                            objsale_order.n_fullfilled_id = 2;
                        }
                        objsale_order.fullfillment_channel = item.fulfillment;
                        if (item.transaction_type == "SHIPMENT")
                        {
                            objsale_order.n_item_orderstatus = 3;
                            objsale_order.order_status = "Shipped";
                        }
                        dba.tbl_sales_order.Add(objsale_order);
                        dba.SaveChanges();


                        iEx = 2;

                        var chkSku = dba.tbl_inventory.Where(aa => aa.sku == item.sku).FirstOrDefault();
                        if (chkSku == null)
                        {
                            tbl_inventory objInventory = new tbl_inventory();
                            objInventory.sku = item.sku;
                            objInventory.tbl_sellers_id = SellerId;
                            objInventory.tbl_marketplace_id = marketplaceID;
                            //objInventory.tbl_item_category_id = 19;
                            //objInventory.tbl_item_subcategory_id = 14;
                            //objInventory.item_name = item.ProductName;
                            objInventory.isactive = 1;
                            //if (item.itemprice != null)
                            //{
                            //    foreach (var itemprice in item.itemprice)
                            //    {
                            //        objInventory.selling_price = Convert.ToInt16(itemprice.pAmonu);
                            //    }
                            //}
                            dba.tbl_inventory.Add(objInventory);
                            dba.SaveChanges();
                        }



                        tbl_sales_order_details objsaledetails = new tbl_sales_order_details();
                        objsaledetails.tbl_sales_order_id = objsale_order.id;
                        objsaledetails.status_updated_by = objsale_order.tbl_sellers_id;
                        objsaledetails.status_updated_on = DateTime.Now;
                        objsaledetails.tbl_seller_id = objsale_order.tbl_sellers_id;
                        objsaledetails.is_active = 1;
                        objsaledetails.n_order_status_id = Convert.ToInt16(objsale_order.n_item_orderstatus);
                        objsaledetails.sku_no = item.sku;
                        objsaledetails.asin = item.asin;
                        objsaledetails.amazon_order_id = item.order_id;
                        objsaledetails.tax_flag = 0;
                        //add address//
                        objsaledetails.Ship_from_city = item.ship_from_city;
                        objsaledetails.ship_from_state = item.ship_from_state;
                        objsaledetails.ship_from_postalcode = item.ship_from_postal_code;
                        objsaledetails.ship_to_city = item.ship_to_city;
                        objsaledetails.ship_to_state = item.ship_to_state;
                        objsaledetails.ship_to_postalcode = item.ship_to_postal_code;
                        objsaledetails.tax_invoiceno = item.tax_invoice_number;

                        //end//
                        dba.tbl_sales_order_details.Add(objsaledetails);
                        dba.SaveChanges();


                        iEx = 3;
                        //--------------------save data in tax table----------------
                        tbl_tax objtax = new tbl_tax();
                        objtax.tbl_seller_id = objsale_order.tbl_sellers_id;
                        objtax.tbl_referneced_id = objsaledetails.id;
                        objtax.reference_type = 3;
                        objtax.isactive = 1;
                        dba.tbl_tax.Add(objtax);
                        dba.SaveChanges();

                        //iEx = 4;
                        //--------------------------End---------------------------//
                        //--------------- save data in table order history -------------------//
                        //var getstatus = dba.tbl_sales_order_status.Where(a => a.is_active == 0).ToList();
                        //tbl_order_history objhistory = new tbl_order_history();
                        //objhistory.created_on = DateTime.Now;
                        //objhistory.tbl_orders_id = objsale_order.id;
                        //objhistory.tbl_seller_id = SellerId;
                        //objhistory.tbl_orderDetails_Id = objsaledetails.id;
                        //objhistory.ASIN = objsaledetails.asin;
                        //objhistory.SKU = objsaledetails.sku_no;
                        //objhistory.Quantity = objsaledetails.quantity_ordered;
                        //objhistory.OrigialOrderID = objsale_order.amazon_order_id;
                        //objhistory.OrderID = objsale_order.amazon_order_id;
                        //objhistory.ShipmentDate = objsale_order.purchase_date;
                        //objhistory.tbl_marketplace_id = objsale_order.tbl_Marketplace_Id;
                        //objhistory.t_order_status = objsaledetails.n_order_status_id;
                        //dba.tbl_order_history.Add(objhistory);
                        //dba.SaveChanges();

                        //iEx = 5;
                        //-----------------------------End------------------------------------//

                    }
                    else
                    {
                        bool creatednow = false;
                        if (get_sale_order_id.tbl_order_upload_id == upload_id)
                        {
                            creatednow = true;
                        }

                        string sku = item.sku;
                        string amazon_order_id = item.order_id;
                        if (item.transaction_type == "SHIPMENT")
                        {
                            //var get_sale_order = dba.tbl_sales_order.Where(a => a.amazon_order_id == amazon_order_id && a.tbl_sellers_id == SellerId).FirstOrDefault();
                            //if (get_sale_order != null)
                            //{
                            if (creatednow)
                            {
                                get_sale_order_id.bill_amount += item.display_price;
                                dba.Entry(get_sale_order_id).State = EntityState.Modified;
                                dba.SaveChanges();
                            }
                            // }
                        }
                        var get_sale_order_detail_id = dba.tbl_sales_order_details.Where(a => a.tbl_sales_order_id == get_sale_order_id.id && a.sku_no == sku).FirstOrDefault();
                        if (get_sale_order_detail_id == null)
                        {
                            ////////////// INSERT INTO SALES ORDER DETAIL TABLE /////////////////

                            iEx = 4;
                            tbl_sales_order_details objsaledetails = new tbl_sales_order_details();
                            objsaledetails.tbl_sales_order_id = get_sale_order_id.id;
                            objsaledetails.status_updated_by = Convert.ToInt16(SellerId);
                            objsaledetails.status_updated_on = DateTime.Now;
                            objsaledetails.tbl_seller_id = Convert.ToInt16(SellerId);
                            objsaledetails.is_active = 1;
                            objsaledetails.tax_flag = 0;
                            objsaledetails.sku_no = item.sku;
                            objsaledetails.asin = item.asin;
                            //add address//
                            objsaledetails.Ship_from_city = item.ship_from_city;
                            objsaledetails.ship_from_state = item.ship_from_state;
                            objsaledetails.ship_from_postalcode = item.ship_from_postal_code;
                            objsaledetails.ship_to_city = item.ship_to_city;
                            objsaledetails.ship_to_state = item.ship_to_state;
                            objsaledetails.ship_to_postalcode = item.ship_to_postal_code;
                            objsaledetails.tax_invoiceno = item.tax_invoice_number;
                            // end//

                            objsaledetails.amazon_order_id = item.order_id;
                            dba.tbl_sales_order_details.Add(objsaledetails);
                            dba.SaveChanges();


                            iEx = 5;
                            //--------------------save data in tax table----------------
                            tbl_tax objtax = new tbl_tax();
                            objtax.tbl_seller_id = Convert.ToInt16(SellerId);
                            objtax.tbl_referneced_id = objsaledetails.id;
                            objtax.reference_type = 3;
                            objtax.isactive = 1;
                            dba.tbl_tax.Add(objtax);
                            dba.SaveChanges();

                            iEx = 6;
                            //--------------------------End---------------------------//
                            //--------------- save data in table order history -------------------//
                            //var getstatus = dba.tbl_sales_order_status.Where(a => a.is_active == 0).ToList();
                            //tbl_order_history objhistory = new tbl_order_history();
                            //objhistory.created_on = DateTime.Now;
                            //objhistory.tbl_orders_id = get_sale_order_id.id;
                            //objhistory.tbl_seller_id = SellerId;
                            //objhistory.tbl_orderDetails_Id = objsaledetails.id;
                            //objhistory.ASIN = objsaledetails.asin;
                            //objhistory.SKU = objsaledetails.sku_no;
                            //objhistory.Quantity = objsaledetails.quantity_ordered;
                            //objhistory.OrigialOrderID = get_sale_order_id.amazon_order_id;
                            //objhistory.OrderID = get_sale_order_id.amazon_order_id;
                            //objhistory.ShipmentDate = get_sale_order_id.purchase_date;
                            //objhistory.tbl_marketplace_id = get_sale_order_id.tbl_Marketplace_Id;
                            //dba.tbl_order_history.Add(objhistory);
                            //dba.SaveChanges();

                            iEx = 7;
                        }
                    }
                }
                string connectionstring = ConfigurationManager.ConnectionStrings["SellerConnection"].ConnectionString;
                MySqlConnection con = new MySqlConnection(connectionstring);

                //int uniqueupload = 0;
                var get_unique_count_details = "SELECT count(DISTINCT aa.id) FROM(SELECT DISTINCT Order_Id AS id FROM `tbl_settlement_order` where tbl_seller_id = " + SellerId + " UNION ALL SELECT DISTINCT amazon_order_id AS id FROM `tbl_sales_order` where tbl_sellers_id =" + SellerId + " ) AS aa";
                MySqlCommand cmd = new MySqlCommand(get_unique_count_details, con);
                con.Open();
                MySqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    uniqueupload = Convert.ToInt32(dr[0]);
                }
                cmd.Dispose();
                con.Close();
                int diff_order = 0;
                var get_balance_details = db.tbl_user_login.Where(a => a.tbl_sellers_id == SellerId).FirstOrDefault();
                if (get_balance_details != null)
                {
                    var last_order = get_balance_details.total_orders;
                    diff_order = Convert.ToInt16(uniqueupload - last_order);
                    var plan_rate = get_balance_details.applied_plan_rate * diff_order;
                    get_balance_details.wallet_balance = get_balance_details.wallet_balance - Convert.ToDouble(plan_rate);
                    int totalOrder = Convert.ToInt16(diff_order);
                    if (get_balance_details.total_orders != null)
                        totalOrder = totalOrder + Convert.ToInt16(get_balance_details.total_orders);
                    get_balance_details.total_orders = totalOrder;
                    db.Entry(get_balance_details).State = EntityState.Modified;
                    //Session["WalletBalance"] = get_balance_details.wallet_balance.ToString();
                    //Session["TotalOrders"] = totalOrder.ToString();
                    db.SaveChanges();
                }
                //var get_upload_settlement_details = dba.tbl_order_upload.Where(a => a.tbl_seller_id == SellerId && a.id == upload_id).FirstOrDefault();
                if (get_upload_order_details != null)
                {
                    get_upload_order_details.new_order_uploaded = diff_order;
                    get_upload_order_details.status = "Completed";
                    get_upload_order_details.completed_datetime = DateTime.Now;
                    dba.Entry(get_upload_order_details).State = EntityState.Modified;
                    dba.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Writelog log = new Writelog();
                log.write_exception_log(SellerId.ToString(), "CronJobController", "Insert_Order_If_Not_Exist", DateTime.Now, ex);
                success = "Error " + iEx.ToString() + " : " + ex.Message.ToString();
            }
            //}
            //else
            //{
            success = "File Already Exist!";
            //}
            return success;
        }

        public string Update_Sales_Detail(List<tbl_seller_tax_file> lstObj, int? strType, int? SellerId, int? marketplaceID, int? addorder, string strFileName, int? upload_id)
        {
            string success = "S";
            try
            {

                var get_upload_order_details = dba.tbl_order_upload.Where(aa => aa.tbl_seller_id == SellerId && aa.id == upload_id).FirstOrDefault();
                if (get_upload_order_details != null)
                {
                    get_upload_order_details.status = "Processing";
                    get_upload_order_details.processing_datetime = DateTime.Now;
                    dba.Entry(get_upload_order_details).State = EntityState.Modified;
                    dba.SaveChanges();
                }

                // var distinctNames = (from d in lstObj select d.order_id, d.asin ).Distinct();
                var distinct_order_asin_sku = (from m in lstObj group m by new { m.order_id, m.asin, m.sku } into mygroup select mygroup.FirstOrDefault()).Distinct().ToList();
                //var Countries = query.ToList().Select(m => new tbl_seller_tax_file { order_id = m.order_id, asin = m.asin, sku = m.sku }).ToList();
                foreach (var items in distinct_order_asin_sku)
                {

                    if (items.order_id == "405-7959510-2745912")
                    {
                    }
                    ///////////// GET SALES ORDER ID /////////////////////
                    var get_sale_order_id = dba.tbl_sales_order.Where(a => a.tbl_sellers_id == SellerId && a.amazon_order_id == items.order_id).FirstOrDefault();
                    if (get_sale_order_id != null)
                    {
                        ///////////// GET SALES ORDER DETAIL DATA /////////////////////
                        //var get_sale_order_detail = dba.tbl_sales_order_details.Where(a => a.tbl_sales_order_id == get_sale_order_id.id && a.amazon_order_id == items.order_id && a.asin == items.asin && a.sku_no == items.sku).FirstOrDefault();
                        var get_sale_order_detail = dba.tbl_sales_order_details.Where(a => a.tbl_sales_order_id == get_sale_order_id.id && a.asin == items.asin && a.sku_no == items.sku).FirstOrDefault();
                        if (get_sale_order_detail != null)
                        {
                            int iMultipleItem_N = 0;
                            int iMultipleItem_Y = 0;
                            int SalesOrderId = get_sale_order_detail.tbl_sales_order_id;
                            int SalesOrderDetailId = get_sale_order_detail.id;
                            int seller_id = get_sale_order_detail.tbl_seller_id;

                            /////////////// Collect Data Information from List object ////////////////
                            var list_order_asin_sku = lstObj.Where(item => item.order_id == items.order_id.ToString() && item.asin == items.asin.ToString() && item.sku == items.sku.ToString() && item.transaction_type.ToLower() == "shipment").ToList();
                            foreach (var listitem in list_order_asin_sku)
                            {
                                string ASIN = listitem.asin;
                                string SKU = listitem.sku;
                                string Is_Display_Promo_Tax_Inclusive = listitem.is_display_promo_tax_inclusive;
                                double TaxExclusive_Selling_Price = listitem.taxexclusive_selling_price;
                                double Total_Tax_Amount = listitem.total_tax;
                                double Tax_Amount = listitem.tax_amount;
                                double Tax_Amount_BlankRow = listitem.tax_amount_blank_row;
                                double Taxed_Jurisdiction_Tax_Rate = listitem.taxed_jurisdiction_tax_rate;//taxrate
                                double Taxed_Jurisdiction_Tax_Rate_BlankRow = listitem.taxed_jurisdiction_tax_rate_blank_row;
                                double Promo_TaxExclusive_Amount = listitem.promo_taxexclusive_amount;
                                double Promo_Amount_Tax = listitem.promo_amount_tax;
                                string TaxType = listitem.tax_type;
                                string TaxType_BlankRow = listitem.tax_type_blank_row;

                                if (Is_Display_Promo_Tax_Inclusive == "Y")
                                {
                                    if (iMultipleItem_Y == 0)
                                    {
                                        double promo_total_tax = Tax_Amount + Tax_Amount_BlankRow;
                                        //get_sale_order_detail.shipping_price_Amount = TaxExclusive_Selling_Price;
                                        get_sale_order_detail.shipping_tax_Amount = promo_total_tax;// Tax_Amount;
                                        get_sale_order_detail.shipping_discount_amt = Promo_TaxExclusive_Amount;
                                        get_sale_order_detail.shipping_discount_tax_amount = Promo_Amount_Tax;
                                        get_sale_order_detail.tax_flag += 1;
                                        // add address//
                                        get_sale_order_detail.Ship_from_city = listitem.ship_from_city;
                                        get_sale_order_detail.ship_from_state = listitem.ship_from_state;
                                        get_sale_order_detail.ship_from_postalcode = listitem.ship_from_postal_code;
                                        get_sale_order_detail.ship_to_city = listitem.ship_to_city;
                                        get_sale_order_detail.ship_to_state = listitem.ship_to_state;
                                        get_sale_order_detail.ship_to_postalcode = listitem.ship_to_postal_code;
                                        get_sale_order_detail.tax_invoiceno = listitem.tax_invoice_number;
                                        get_sale_order_detail.is_tax_calculated = 0;
                                        //get_sale_order_detail.tax_updatedby_taxfile = 0;
                                        // end //
                                        dba.Entry(get_sale_order_detail).State = EntityState.Modified;
                                        dba.SaveChanges();
                                    }
                                    else
                                    {
                                        double promo_total_tax = Tax_Amount + Tax_Amount_BlankRow;
                                        //get_sale_order_detail.shipping_price_Amount = get_sale_order_detail.shipping_price_Amount + TaxExclusive_Selling_Price;                                     
                                        get_sale_order_detail.shipping_tax_Amount = get_sale_order_detail.shipping_tax_Amount + promo_total_tax; // Tax_Amount;
                                        get_sale_order_detail.shipping_discount_amt = get_sale_order_detail.shipping_discount_amt + Promo_TaxExclusive_Amount;
                                        get_sale_order_detail.shipping_discount_tax_amount = get_sale_order_detail.shipping_discount_tax_amount + Promo_Amount_Tax;
                                        get_sale_order_detail.tax_flag += 1;
                                        get_sale_order_detail.is_tax_calculated = 0;
                                        //get_sale_order_detail.tax_updatedby_taxfile = 0;
                                        get_sale_order_detail.tax_invoiceno = listitem.tax_invoice_number;
                                        dba.Entry(get_sale_order_detail).State = EntityState.Modified;
                                        dba.SaveChanges();
                                    }
                                    iMultipleItem_Y++;
                                }
                                else
                                {
                                    if (iMultipleItem_N == 0)
                                    {
                                        // get_sale_order_detail.item_price_amount = TaxExclusive_Selling_Price;
                                        get_sale_order_detail.item_tax_amount = Total_Tax_Amount; // Tax_Amount;
                                        get_sale_order_detail.tax_flag += 1;
                                        get_sale_order_detail.tax_invoiceno = listitem.tax_invoice_number;
                                        get_sale_order_detail.is_tax_calculated = 0;
                                        get_sale_order_detail.tax_updatedby_taxfile = 0;
                                        dba.Entry(get_sale_order_detail).State = EntityState.Modified;
                                        dba.SaveChanges();

                                        //////////////////// UPDATE TAX TABLE ////////////////
                                        Update_Tax_Table(SalesOrderDetailId, Taxed_Jurisdiction_Tax_Rate, TaxType_BlankRow, Tax_Amount_BlankRow, Taxed_Jurisdiction_Tax_Rate_BlankRow, TaxType, Tax_Amount, iMultipleItem_N, seller_id);
                                    }
                                    else
                                    {
                                        // get_sale_order_detail.item_price_amount = get_sale_order_detail.item_price_amount + TaxExclusive_Selling_Price;                                       
                                        get_sale_order_detail.item_tax_amount = get_sale_order_detail.item_tax_amount + Total_Tax_Amount; // Tax_Amount;
                                        get_sale_order_detail.tax_flag += 1;
                                        get_sale_order_detail.tax_invoiceno = listitem.tax_invoice_number;
                                        get_sale_order_detail.is_tax_calculated = 0;
                                        // get_sale_order_detail.tax_updatedby_taxfile = 0;
                                        dba.Entry(get_sale_order_detail).State = EntityState.Modified;
                                        dba.SaveChanges();

                                        //////////////////// UPDATE TAX TABLE ////////////////
                                        Update_Tax_Table(SalesOrderDetailId, Taxed_Jurisdiction_Tax_Rate, TaxType_BlankRow, Tax_Amount_BlankRow, Taxed_Jurisdiction_Tax_Rate_BlankRow, TaxType, Tax_Amount, iMultipleItem_N, seller_id);
                                    }
                                    iMultipleItem_N++;
                                }
                            }
                        }
                    }
                    else
                    {

                    }// end of else                             
                }
                if (get_upload_order_details != null)
                {
                    get_upload_order_details.status = "Completed";
                    get_upload_order_details.completed_datetime = DateTime.Now;
                    dba.Entry(get_upload_order_details).State = EntityState.Modified;
                    dba.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Writelog log = new Writelog();
                log.write_exception_log(SellerId.ToString(), "CronJobController", "Update_Sales_Detail", DateTime.Now, ex);
            }
            return success;
        }

        public void Update_Tax_Table(int tbl_referneced_id, double Taxed_Jurisdiction_Tax_Rate, string TaxType_BlankRow, double Tax_Amount_BlankRow, double Taxed_Jurisdiction_Tax_Rate_BlankRow, string TaxType, double Tax_Amount, int iRepeat, int SellerId)
        {//////////////////////// TAX TABLE UPDATE /////////////////////////////
            try
            {

                var get_saleorderdetails = dba.tbl_sales_order_details.Where(a => a.id == tbl_referneced_id && a.tbl_seller_id == SellerId && a.tax_updatedby_taxfile == 0).FirstOrDefault();
                if (get_saleorderdetails != null)
                {
                    if (TaxType == "IGST")
                    {
                        decimal itemamount = Convert.ToDecimal(get_saleorderdetails.item_price_amount);
                        decimal shippingamt = Convert.ToDecimal(get_saleorderdetails.shipping_price_Amount);
                        decimal taxrate = 0;
                        string input_decimal_number4 = Convert.ToString(Taxed_Jurisdiction_Tax_Rate);
                        var regex = new System.Text.RegularExpressions.Regex("(?<=[\\.])[0-9]+");
                        if (regex.IsMatch(input_decimal_number4))
                        {
                            string decimal_places4 = regex.Match(input_decimal_number4).Value;
                            taxrate = Convert.ToDecimal(decimal_places4);
                        }
                        decimal ttax = 100 + taxrate;
                        decimal total = (itemamount * 100) / ttax;
                        decimal totalshipping = (shippingamt * 100) / ttax;


                        string value = Convert.ToString(total);
                        string value2 = Convert.ToString(totalshipping);
                        if (value != "NaN" && value != "-Infinity")
                        {
                            decimal abc = Convert.ToDecimal(value);
                            decimal result = decimal.Round(abc, 2, MidpointRounding.AwayFromZero);//Math.Round(abc, 2);//decimal.Round(abc, 2, MidpointRounding.AwayFromZero);                                                        
                            get_saleorderdetails.item_price_amount = Convert.ToDouble(result);
                        }
                        if (value2 != "NaN" && value2 != "-Infinity")
                        {
                            decimal abc = Convert.ToDecimal(value2);
                            decimal result = decimal.Round(abc, 2, MidpointRounding.AwayFromZero);//Math.Round(abc, 2); //
                            get_saleorderdetails.shipping_price_Amount = Convert.ToDouble(result);
                        }
                        get_saleorderdetails.tax_updatedby_taxfile = 1;
                        dba.Entry(get_saleorderdetails).State = EntityState.Modified;
                        dba.SaveChanges();
                    }
                    else if (TaxType == "CGST")
                    {
                        decimal itemamount = Convert.ToDecimal(get_saleorderdetails.item_price_amount);
                        decimal shippingamt = Convert.ToDecimal(get_saleorderdetails.shipping_price_Amount);
                        decimal taxrate = 0;
                        double totaltax = Taxed_Jurisdiction_Tax_Rate + Taxed_Jurisdiction_Tax_Rate;
                        string input_decimal_number5 = Convert.ToString(totaltax);
                        var regex = new System.Text.RegularExpressions.Regex("(?<=[\\.])[0-9]+");
                        if (regex.IsMatch(input_decimal_number5))
                        {
                            string decimal_places5 = regex.Match(input_decimal_number5).Value;
                            taxrate = Convert.ToDecimal(decimal_places5);
                        }
                        decimal ttax = 100 + taxrate;
                        decimal total = (itemamount * 100) / ttax;
                        decimal totalshipping = (shippingamt * 100) / ttax;
                        string value = Convert.ToString(total);
                        string value2 = Convert.ToString(totalshipping);
                        if (value != "NaN" && value != "-Infinity")
                        {
                            decimal abc = Convert.ToDecimal(value);
                            decimal result = Math.Round(abc, 2); //decimal.Round(abc, 2, MidpointRounding.AwayFromZero); //Math.Round(abc, 2); 
                            get_saleorderdetails.item_price_amount = Convert.ToDouble(result);
                        }
                        if (value2 != "NaN" && value2 != "-Infinity")
                        {
                            decimal abc = Convert.ToDecimal(value2);
                            decimal result = Math.Round(abc, 2);//decimal.Round(abc, 2, MidpointRounding.AwayFromZero);// 
                            get_saleorderdetails.shipping_price_Amount = Convert.ToDouble(result);
                        }
                        get_saleorderdetails.tax_updatedby_taxfile = 1;
                        dba.Entry(get_saleorderdetails).State = EntityState.Modified;
                        dba.SaveChanges();
                    }
                }
                var get_Tax = dba.tbl_tax.Where(a => a.reference_type == 3 && a.tbl_referneced_id == tbl_referneced_id && a.isactive == 1).FirstOrDefault();
                if (get_Tax == null)
                {
                    tbl_tax objtax = new tbl_tax();
                    objtax.tbl_seller_id = SellerId;
                    objtax.tbl_referneced_id = tbl_referneced_id;
                    objtax.reference_type = 3;
                    objtax.isactive = 1;
                    dba.tbl_tax.Add(objtax);
                    dba.SaveChanges();
                }
                var getTax = dba.tbl_tax.Where(a => a.reference_type == 3 && a.tbl_referneced_id == tbl_referneced_id && a.isactive == 1).FirstOrDefault();
                if (getTax != null)
                {
                    getTax.cgst_tax = TaxType.ToLower() == "cgst" ? Taxed_Jurisdiction_Tax_Rate : TaxType_BlankRow.ToLower() == "cgst" && Tax_Amount_BlankRow > 0 ? Taxed_Jurisdiction_Tax_Rate_BlankRow : 0;
                    getTax.igst_tax = TaxType.ToLower() == "igst" ? Taxed_Jurisdiction_Tax_Rate : TaxType_BlankRow.ToLower() == "igst" && Tax_Amount_BlankRow > 0 ? Taxed_Jurisdiction_Tax_Rate_BlankRow : 0;
                    getTax.sgst_tax = TaxType.ToLower() == "sgst" ? Taxed_Jurisdiction_Tax_Rate : TaxType_BlankRow.ToLower() == "sgst" && Tax_Amount_BlankRow > 0 ? Taxed_Jurisdiction_Tax_Rate_BlankRow : 0;
                    if (iRepeat == 0)
                    {
                        getTax.CGST_amount = TaxType.ToLower() == "cgst" ? Tax_Amount : TaxType_BlankRow.ToLower() == "cgst" && Tax_Amount_BlankRow > 0 ? Tax_Amount_BlankRow : 0;
                        getTax.Igst_amount = TaxType.ToLower() == "igst" ? Tax_Amount : TaxType_BlankRow.ToLower() == "igst" && Tax_Amount_BlankRow > 0 ? Tax_Amount_BlankRow : 0;
                        getTax.sgst_amount = TaxType.ToLower() == "sgst" ? Tax_Amount : TaxType_BlankRow.ToLower() == "sgst" && Tax_Amount_BlankRow > 0 ? Tax_Amount_BlankRow : 0;
                    }
                    else
                    {
                        getTax.CGST_amount = getTax.CGST_amount + (TaxType.ToLower() == "cgst" ? Tax_Amount : TaxType_BlankRow.ToLower() == "cgst" && Tax_Amount_BlankRow > 0 ? Tax_Amount_BlankRow : 0);
                        getTax.Igst_amount = getTax.Igst_amount + (TaxType.ToLower() == "igst" ? Tax_Amount : TaxType_BlankRow.ToLower() == "igst" && Tax_Amount_BlankRow > 0 ? Tax_Amount_BlankRow : 0);
                        getTax.sgst_amount = getTax.sgst_amount + (TaxType.ToLower() == "sgst" ? Tax_Amount : TaxType_BlankRow.ToLower() == "sgst" && Tax_Amount_BlankRow > 0 ? Tax_Amount_BlankRow : 0);
                    }
                    dba.Entry(getTax).State = EntityState.Modified;
                    dba.SaveChanges();

                    if (getTax.cgst_tax != 0 && getTax.cgst_tax != null)
                    {
                        string cgst_taxrate = "";
                        string input_decimal_number = Convert.ToString(getTax.cgst_tax);
                        var regex = new System.Text.RegularExpressions.Regex("(?<=[\\.])[0-9]+");
                        if (regex.IsMatch(input_decimal_number))
                        {
                            string decimal_places = regex.Match(input_decimal_number).Value;
                            cgst_taxrate = decimal_places;
                        }

                        string Taxname = "CGST@" + cgst_taxrate + "%";
                        var gettaxdetails = dba.tbl_Salesledger_tax.Where(a => a.tax_name == Taxname).FirstOrDefault();
                        if (gettaxdetails == null)
                        {
                            tbl_Salesledger_tax obj_salestax = new tbl_Salesledger_tax();
                            obj_salestax.tax_name = Taxname;
                            obj_salestax.tax_percentage = Convert.ToDouble(cgst_taxrate);
                            dba.tbl_Salesledger_tax.Add(obj_salestax);
                            dba.SaveChanges();
                        }
                    }

                    if (getTax.sgst_tax != 0 && getTax.sgst_tax != null)
                    {
                        string sgst_taxrate = "";
                        string input_decimal_number1 = Convert.ToString(getTax.sgst_tax);
                        var regex = new System.Text.RegularExpressions.Regex("(?<=[\\.])[0-9]+");
                        if (regex.IsMatch(input_decimal_number1))
                        {
                            string decimal_places1 = regex.Match(input_decimal_number1).Value;
                            sgst_taxrate = decimal_places1;
                        }
                        string Taxname = "SGST@" + sgst_taxrate + "%";
                        var gettaxdetails = dba.tbl_Salesledger_tax.Where(a => a.tax_name == Taxname).FirstOrDefault();
                        if (gettaxdetails == null)
                        {
                            tbl_Salesledger_tax obj_salestax = new tbl_Salesledger_tax();
                            obj_salestax.tax_name = Taxname;
                            obj_salestax.tax_percentage = Convert.ToDouble(sgst_taxrate);
                            dba.tbl_Salesledger_tax.Add(obj_salestax);
                            dba.SaveChanges();
                        }
                    }
                    if (getTax.igst_tax != 0 && getTax.igst_tax != null)
                    {
                        string igst_taxrate = "";
                        string input_decimal_number2 = Convert.ToString(getTax.igst_tax);
                        var regex = new System.Text.RegularExpressions.Regex("(?<=[\\.])[0-9]+");
                        if (regex.IsMatch(input_decimal_number2))
                        {
                            string decimal_places2 = regex.Match(input_decimal_number2).Value;
                            igst_taxrate = decimal_places2;
                        }

                        string Taxname = "IGST@" + igst_taxrate + "%";
                        var gettaxdetails = dba.tbl_Salesledger_tax.Where(a => a.tax_name == Taxname).FirstOrDefault();
                        if (gettaxdetails == null)
                        {
                            tbl_Salesledger_tax obj_salestax = new tbl_Salesledger_tax();
                            obj_salestax.tax_name = Taxname;
                            obj_salestax.tax_percentage = Convert.ToDouble(igst_taxrate);
                            dba.tbl_Salesledger_tax.Add(obj_salestax);
                            dba.SaveChanges();
                        }
                    }

                }
                else
                {
                    tbl_tax objtax = new tbl_tax();
                    objtax.tbl_referneced_id = tbl_referneced_id;

                    getTax.cgst_tax = TaxType.ToLower() == "cgst" ? Taxed_Jurisdiction_Tax_Rate : TaxType_BlankRow.ToLower() == "cgst" && Tax_Amount_BlankRow > 0 ? Taxed_Jurisdiction_Tax_Rate_BlankRow : 0;
                    getTax.CGST_amount = TaxType.ToLower() == "cgst" ? Tax_Amount : TaxType_BlankRow.ToLower() == "cgst" && Tax_Amount_BlankRow > 0 ? Tax_Amount_BlankRow : 0;
                    getTax.igst_tax = TaxType.ToLower() == "igst" ? Taxed_Jurisdiction_Tax_Rate : TaxType_BlankRow.ToLower() == "igst" && Tax_Amount_BlankRow > 0 ? Taxed_Jurisdiction_Tax_Rate_BlankRow : 0;
                    getTax.Igst_amount = TaxType.ToLower() == "igst" ? Tax_Amount : TaxType_BlankRow.ToLower() == "igst" && Tax_Amount_BlankRow > 0 ? Tax_Amount_BlankRow : 0;
                    getTax.sgst_tax = TaxType.ToLower() == "sgst" ? Taxed_Jurisdiction_Tax_Rate : TaxType_BlankRow.ToLower() == "sgst" && Tax_Amount_BlankRow > 0 ? Taxed_Jurisdiction_Tax_Rate_BlankRow : 0;
                    getTax.sgst_amount = TaxType.ToLower() == "sgst" ? Tax_Amount : TaxType_BlankRow.ToLower() == "sgst" && Tax_Amount_BlankRow > 0 ? Tax_Amount_BlankRow : 0;
                    objtax.reference_type = 3;
                    objtax.isactive = 1;

                    dba.tbl_tax.Add(objtax);
                    dba.SaveChanges();

                    //tbl_Salesledger_tax obj_salestax = new tbl_Salesledger_tax();
                    //if()
                }
            }
            catch (Exception ex)
            {
                Writelog log = new Writelog();
                log.write_exception_log(SellerId.ToString(), "CronJobController", "Update_Tax_Table", DateTime.Now, ex);
            }
        }
        #endregion

        #region for Paytm
        public DataTable BulkcsvForPayTm(string strFilePath, string strFileName, int marketplaceID, int upload_id, int SellerId)
        {
            string success = "S";
            string filename = "";
            DataTable dtCsv = new DataTable();
            try
            {
                string Fulltext;
                FileStream fileStream = new FileStream(strFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                using (StreamReader sr = new StreamReader(fileStream))
                {
                    while (!sr.EndOfStream)
                    {
                        Fulltext = sr.ReadToEnd().ToString(); //read full file text  
                        string[] rows = Fulltext.Split('\n'); //split full file text into rows  
                        for (int i = 0; i < rows.Count() - 1; i++)
                        {
                            string[] rowValues = rows[i].Split(','); //split each row with comma to get individual values  
                            {
                                if (i == 0)
                                {
                                    for (int j = 0; j < rowValues.Count(); j++)
                                    {
                                        dtCsv.Columns.Add(rowValues[j].Replace("\r", "").Replace("\n", "").Trim()); //add headers  
                                    }
                                }
                                else
                                {
                                    DataRow dr = dtCsv.NewRow();
                                    for (int k = 0; k < rowValues.Count(); k++)
                                    {
                                        dr[k] = rowValues[k].ToString();
                                    }
                                    dtCsv.Rows.Add(dr); //add other rows  
                                }
                            }
                        }
                    }
                    fileStream.Close();

                }


                var columns = dtCsv.Columns;

                for (int i = 0; i < dtCsv.Rows.Count; i++)
                {
                    string ordrid = columns.Contains("Order ID") ? dtCsv.Rows[i]["Order ID"].ToString() : "";
                    string orderitemid = columns.Contains("Order Item ID") ? dtCsv.Rows[i]["Order Item ID"].ToString().Replace("\"", "") : "";

                    var get_settdetails = dba.tbl_paytmgstmaster
                            .FirstOrDefault(a => a.tbl_SellerId == SellerId
                            && a.OrderID == ordrid
                            && a.OrderItemID == orderitemid
                            && a.Marketplaceid == marketplaceID);

                    if (get_settdetails != null)
                    {
                        continue;
                    }

                    tbl_paytmgstmaster paytmGstMaster = new tbl_paytmgstmaster();
                    paytmGstMaster.OrderID = ordrid; //columns.Contains("Order ID") ? dtCsv.Rows[i]["Order ID"].ToString() : "";
                    paytmGstMaster.OrderItemID = orderitemid;// columns.Contains("Order Item ID") ? dtCsv.Rows[i]["Order Item ID"].ToString().Replace("\"", "") : "";
                    paytmGstMaster.OrderCreationDate = columns.Contains("Order Creation Date") ? dtCsv.Rows[i]["Order Creation Date"].ToString().Replace("\"", "") : "";
                    paytmGstMaster.ReturnDate = columns.Contains("Return Date") ? dtCsv.Rows[i]["Return Date"].ToString().Replace("\"", "") : "";
                    paytmGstMaster.ProductID = columns.Contains("Product ID") ? dtCsv.Rows[i]["Product ID"].ToString().Replace("\"", "") : "";
                    paytmGstMaster.ProductName = columns.Contains("Product Name") ? dtCsv.Rows[i]["Product Name"].ToString().Replace("\"", "") : "";
                    paytmGstMaster.MerchantSKU = columns.Contains("Merchant SKU") ? dtCsv.Rows[i]["Merchant SKU"].ToString().Replace("\"", "") : "";
                    paytmGstMaster.OrderItemStatus = columns.Contains("Order Item Status") ? dtCsv.Rows[i]["Order Item Status"].ToString().Replace("\"", "") : "";
                    paytmGstMaster.payment_methods = columns.Contains("payment_methods") ? dtCsv.Rows[i]["payment_methods"].ToString().Replace("\"", "") : "";

                    paytmGstMaster.item_source_state = columns.Contains("item_source_state") ? dtCsv.Rows[i]["item_source_state"].ToString().Replace("\"", "") : "";
                    paytmGstMaster.item_source_pincode = columns.Contains("item_source_pincode") ? dtCsv.Rows[i]["item_source_pincode"].ToString().Replace("\"", "") : "";
                    paytmGstMaster.item_destination_state = columns.Contains("item_destination_state") ? dtCsv.Rows[i]["item_destination_state"].ToString().Replace("\"", "") : "";
                    paytmGstMaster.item_destination_pincode = columns.Contains("item_destination_pincode") ? dtCsv.Rows[i]["item_destination_pincode"].ToString().Replace("\"", "") : "";
                    paytmGstMaster.item_invoice_id = columns.Contains("item_invoice_id") ? dtCsv.Rows[i]["item_invoice_id"].ToString().Replace("\"", "") : "";
                    paytmGstMaster.item_invoice_date = columns.Contains("item_invoice_date") ? dtCsv.Rows[i]["item_invoice_date"].ToString().Replace("\"", "") : "";
                    paytmGstMaster.TotalPrice = columns.Contains("Total Price") ? dtCsv.Rows[i]["Total Price"].ToString().Replace("\"", "") : "";
                    paytmGstMaster.ProductPrice = columns.Contains("Product Price") ? dtCsv.Rows[i]["Product Price"].ToString().Replace("\"", "") : "";
                    paytmGstMaster.ShippingChargesTotal = columns.Contains("Shipping Charges Total") ? dtCsv.Rows[i]["Shipping Charges Total"].ToString().Replace("\"", "") : "";
                    paytmGstMaster.ProductQty = columns.Contains("Product Qty") ? dtCsv.Rows[i]["Product Qty"].ToString().Replace("\"", "") : "";
                    paytmGstMaster.item_taxable_value = columns.Contains("item_taxable_value") ? dtCsv.Rows[i]["item_taxable_value"].ToString().Replace("\"", "") : "";
                    paytmGstMaster.item_hsn = columns.Contains("item_hsn") ? dtCsv.Rows[i]["item_hsn"].ToString().Replace("\"", "") : "";
                    paytmGstMaster.item_sgst = columns.Contains("item_sgst") ? dtCsv.Rows[i]["item_sgst"].ToString().Replace("\"", "") : "";
                    paytmGstMaster.item_cgst = columns.Contains("item_cgst") ? dtCsv.Rows[i]["item_cgst"].ToString().Replace("\"", "") : "";
                    paytmGstMaster.item_igst = columns.Contains("item_igst") ? dtCsv.Rows[i]["item_igst"].ToString().Replace("\"", "") : "";
                    paytmGstMaster.item_cess = columns.Contains("item_cess") ? dtCsv.Rows[i]["item_cess"].ToString().Replace("\"", "") : "";
                    paytmGstMaster.item_sgst_amount = columns.Contains("item_sgst_amount") ? dtCsv.Rows[i]["item_sgst_amount"].ToString().Replace("\"", "") : "";
                    paytmGstMaster.item_cgst_amount = columns.Contains("item_cgst_amount") ? dtCsv.Rows[i]["item_cgst_amount"].ToString().Replace("\"", "") : "";
                    paytmGstMaster.item_igst_amount = columns.Contains("item_igst_amount") ? dtCsv.Rows[i]["item_igst_amount"].ToString().Replace("\"", "") : "";
                    paytmGstMaster.item_cess_amount = columns.Contains("item_cess_amount") ? dtCsv.Rows[i]["item_cess_amount"].ToString().Replace("\"", "") : "";
                    paytmGstMaster.item_shipping_taxable_value = columns.Contains("item_shipping_taxable_value") ? dtCsv.Rows[i]["item_shipping_taxable_value"].ToString().Replace("\"", "") : "";
                    paytmGstMaster.item_shipping_sgst = columns.Contains("item_shipping_sgst") ? dtCsv.Rows[i]["item_shipping_sgst"].ToString().Replace("\"", "") : "";
                    paytmGstMaster.item_shipping_cgst = columns.Contains("item_shipping_cgst") ? dtCsv.Rows[i]["item_shipping_cgst"].ToString().Replace("\"", "") : "";
                    paytmGstMaster.item_shipping_igst = columns.Contains("item_shipping_igst") ? dtCsv.Rows[i]["item_shipping_igst"].ToString().Replace("\"", "") : "";
                    paytmGstMaster.item_shipping_cess = columns.Contains("item_shipping_cess") ? dtCsv.Rows[i]["item_shipping_cess"].ToString().Replace("\"", "") : "";
                    paytmGstMaster.item_shipping_sgst_amount = columns.Contains("item_shipping_sgst_amount") ? dtCsv.Rows[i]["item_shipping_sgst_amount"].ToString().Replace("\"", "") : "";
                    paytmGstMaster.item_shipping_cgst_amount = columns.Contains("item_shipping_cgst_amount") ? dtCsv.Rows[i]["item_shipping_cgst_amount"].ToString().Replace("\"", "") : "";
                    paytmGstMaster.item_shipping_igst_amount = columns.Contains("item_shipping_igst_amount") ? dtCsv.Rows[i]["item_shipping_igst_amount"].ToString().Replace("\"", "") : "";
                    paytmGstMaster.item_shipping_cess_amount = columns.Contains("item_shipping_cess_amount") ? dtCsv.Rows[i]["item_shipping_cess_amount"].ToString().Replace("\"", "") : "";
                    paytmGstMaster.item_total_gst = columns.Contains("item_total_gst") ? dtCsv.Rows[i]["item_total_gst"].ToString().Replace("\"", "") : "";
                    paytmGstMaster.item_credit_note = columns.Contains("item_credit_note") ? dtCsv.Rows[i]["item_credit_note"].ToString().Replace("\"", "") : "";
                    paytmGstMaster.item_credit_note_date = columns.Contains("item_credit_note_date") ? dtCsv.Rows[i]["item_credit_note_date"].ToString().Replace("\"", "") : "";
                    paytmGstMaster.customer_company_name = columns.Contains("customer_company_name") ? dtCsv.Rows[i]["customer_company_name"].ToString().Replace("\"", "") : "";
                    paytmGstMaster.customer_billing_address = columns.Contains("customer_billing_address") ? dtCsv.Rows[i]["customer_billing_address"].ToString().Replace("\"", "") : "";
                    paytmGstMaster.info_gstin = columns.Contains("info_gstin") ? dtCsv.Rows[i]["info_gstin"].ToString().Replace("\"", "").Replace("\r","") : "";
                    paytmGstMaster.gst_item_direction = columns.Contains("gst_item_direction") ? dtCsv.Rows[i]["gst_item_direction"].ToString().Replace("\"", "").Replace("\r", "") : "";
                    paytmGstMaster.Uploadid = upload_id;
                    paytmGstMaster.Marketplaceid = marketplaceID;
                    paytmGstMaster.Status = 0;
                    paytmGstMaster.tbl_SellerId = SellerId;
                    dba.tbl_paytmgstmaster.Add(paytmGstMaster);
                    
                }

                dba.SaveChanges();

                SavePaytmGST(SellerId,marketplaceID, upload_id, filename);
                //uploadPayTmSettlements(marketplaceID, id, SellerId);
            }
            catch (Exception ex)
            {
            }
            return dtCsv;
        }

        public string SavePaytmGST(int seller_id, int marketplaceID, int id, string filename)
        {
            string success = "S";
            try
            {
                int count = 0;
                int kk = 0;
                string startdate = "", enddate = "";
                var getdetails = dba.tbl_sales_order_status.Where(aa => aa.is_active == 0).ToList();
                Dictionary<string, int> orderstatus_dict = new Dictionary<string, int>();
                foreach (var item1 in getdetails)
                {
                    string name = item1.sales_order_status.ToLower();
                    orderstatus_dict.Add(name, item1.id);
                }
                var getpaytmdetails = dba.tbl_paytmgstmaster.Where(a => a.tbl_SellerId == seller_id && a.Marketplaceid == marketplaceID && a.Uploadid == id && a.Status == 0).ToList();
                if (getpaytmdetails != null)
                {
                    var get_upload_order_details = dba.tbl_order_upload.Where(aa => aa.tbl_seller_id == seller_id && aa.id == id && aa.tbl_Marketplace_id == marketplaceID).FirstOrDefault();
                    if (get_upload_order_details != null)
                    {
                        get_upload_order_details.filename = filename;
                        get_upload_order_details.status = "Processing";
                        get_upload_order_details.processing_datetime = DateTime.Now;
                        dba.Entry(get_upload_order_details).State = EntityState.Modified;
                        dba.SaveChanges();
                    }
                    foreach (var item in getpaytmdetails)
                    {
                        string orderid = item.OrderID;
                        string itemorderid = item.OrderItemID;
                        string productid = item.ProductID;

                        if (item.gst_item_direction == "1")
                        {
                            var getorder_details = dba.tbl_sales_order_details.Where(a => a.amazon_order_id == orderid && a.product_id == productid && a.order_item_id == itemorderid && a.tbl_seller_id == seller_id).FirstOrDefault();
                            #region update orderdetails and tax table when id id matched
                            if (getorder_details != null)
                            {
                                int saledetail_id = getorder_details.id;
                                double itempr = Convert.ToDouble(item.item_taxable_value);
                                double shipp = Convert.ToDouble(item.ShippingChargesTotal);
                                double differ = itempr - shipp;
                                getorder_details.hsn = item.item_hsn;
                                getorder_details.item_price_amount = differ;
                                getorder_details.shipping_price_Amount = Convert.ToDouble(item.ShippingChargesTotal);
                                getorder_details.item_tax_amount = Convert.ToDouble(item.item_total_gst);
                                getorder_details.tax_invoiceno = item.item_invoice_id;
                                dba.Entry(getorder_details).State = EntityState.Modified;
                                dba.SaveChanges();

                                var get_taxdetails = dba.tbl_tax.Where(a => a.tbl_seller_id == seller_id && a.reference_type == 3 && a.isactive == 1 && a.tbl_referneced_id == saledetail_id).FirstOrDefault();
                                if (get_taxdetails != null)
                                {
                                    if (item.item_igst != "" && item.item_igst != null)
                                    {
                                        get_taxdetails.igst_tax = Convert.ToDouble(item.item_igst);
                                        get_taxdetails.Igst_amount = Convert.ToDouble(item.item_igst_amount);
                                        dba.Entry(get_taxdetails).State = EntityState.Modified;
                                        dba.SaveChanges();
                                    }
                                    else
                                    {
                                        get_taxdetails.cgst_tax = Convert.ToDouble(item.item_cgst);
                                        get_taxdetails.sgst_tax = Convert.ToDouble(item.item_sgst);
                                        get_taxdetails.CGST_amount = Convert.ToDouble(item.item_cgst_amount);
                                        get_taxdetails.sgst_amount = Convert.ToDouble(item.item_sgst_amount);
                                        dba.Entry(get_taxdetails).State = EntityState.Modified;
                                        dba.SaveChanges();
                                    }
                                }// end of if(get_taxdetails)
                            }// end of if (getorder_details != null)
                            #endregion

                            #region add new order
                            else
                            {

                                double itempr1 = Convert.ToDouble(item.item_taxable_value);
                                double shipp1 = Convert.ToDouble(item.ShippingChargesTotal);
                                double differ1 = itempr1 - shipp1;

                                var chkSku = dba.tbl_inventory.Where(aa => aa.sku == item.MerchantSKU && aa.tbl_sellers_id == seller_id).FirstOrDefault();
                                if (chkSku == null)
                                {
                                    tbl_inventory objInventory = new tbl_inventory();
                                    objInventory.sku = item.MerchantSKU;
                                    objInventory.tbl_sellers_id = seller_id;

                                    objInventory.tbl_marketplace_id = marketplaceID;
                                    objInventory.item_name = item.ProductName;
                                    objInventory.isactive = 1;
                                    if (differ1 != null)
                                    {
                                        objInventory.selling_price = Convert.ToInt16(differ1);
                                    }
                                    dba.tbl_inventory.Add(objInventory);
                                    dba.SaveChanges();
                                }

                                tbl_customer_details objcustumer = new tbl_customer_details();
                                objcustumer.State_Region = item.item_destination_state;
                                objcustumer.Country_Code = "IN";
                                objcustumer.Postal_Code = item.item_destination_pincode;
                                objcustumer.tbl_seller_id = seller_id;
                                dba.tbl_customer_details.Add(objcustumer);
                                dba.SaveChanges();

                                tbl_sales_order objsales = new tbl_sales_order();
                                objsales.amazon_order_id = item.OrderID;
                                objsales.tbl_Customer_Id = objcustumer.id;

                                if (item.OrderCreationDate.Contains('-') && item.OrderCreationDate.Contains(':'))
                                {
                                    var dateFormat = item.OrderCreationDate.Split('-')[0].Length == 4 ? "yyyy-MM-dd" : "dd-MM-yyyy";
                                    var timeFormat = item.OrderCreationDate.Split(':').Length == 1 ? "HH:mm" : "HH:mm:ss";

                                    objsales.purchase_date = DateTime.ParseExact(item.OrderCreationDate, dateFormat +" " + timeFormat, CultureInfo.InvariantCulture);
                                }
                                else if (item.OrderCreationDate.Contains('-'))
                                {
                                    var dateFormat = item.OrderCreationDate.Split('-')[0].Length == 4 ? "yyyy-MM-dd" : "dd-MM-yyyy";
                                    objsales.purchase_date = DateTime.ParseExact(item.OrderCreationDate, dateFormat, CultureInfo.InvariantCulture);
                                }
                                else
                                {
                                    DateTime outDate;
                                    DateTime.TryParse(item.OrderCreationDate, out outDate);
                                    objsales.purchase_date = outDate;
                                }

                                //objsales.purchase_date = Convert.ToDateTime(item.OrderCreationDate);
                                objsales.order_status = item.OrderItemStatus;
                                objsales.no_of_itemshipped = Convert.ToInt32(item.ProductQty);
                                objsales.currency_code = "INR";
                                objsales.bill_amount = Convert.ToDouble(item.TotalPrice);//Convert.ToDouble(item.SellingPricePerItem);
                                objsales.tbl_Marketplace_Id = marketplaceID;
                                objsales.order_item_id = item.OrderItemID;
                                objsales.created_on = DateTime.Now;
                                objsales.is_active = 1;
                                objsales.tbl_sellers_id = seller_id;
                                objsales.tbl_order_upload_id = id;
                                objsales.sales_channel = "Sale Customer PayTm";//"Paytm.in";

                                objsales.last_updated_date = DateTime.Now;
                                objsales.fullfillment_channel = "PayTm";
                                objsales.LastUpdatedDateUTC = DateTime.UtcNow;
                                string name1 = item.OrderItemStatus.ToLower();
                                if (orderstatus_dict.ContainsKey(name1))
                                {
                                    objsales.n_item_orderstatus = orderstatus_dict[name1];
                                }
                                if (item.ReturnDate != "0000-00-00 00:00:00" && item.ReturnDate != "")
                                {

                                    if (item.ReturnDate.Contains('-') && item.ReturnDate.Contains(':'))
                                    {
                                        var dateFormat = item.ReturnDate.Split('-')[0].Length == 4 ? "yyyy-MM-dd" : "dd-MM-yyyy";
                                        var timeFormat = item.ReturnDate.Split(':').Length == 1 ? "HH:mm" : "HH:mm:ss";

                                        objsales.returned_at = DateTime.ParseExact(item.ReturnDate, dateFormat + " " + timeFormat, CultureInfo.InvariantCulture);
                                    }
                                    else if (item.ReturnDate.Contains('-'))
                                    {
                                        var dateFormat = item.ReturnDate.Split('-')[0].Length == 4 ? "yyyy-MM-dd" : "dd-MM-yyyy";
                                        objsales.returned_at = DateTime.ParseExact(item.ReturnDate, dateFormat, CultureInfo.InvariantCulture);
                                    }
                                    else
                                    {
                                        DateTime outDate;
                                        DateTime.TryParse(item.ReturnDate, out outDate);
                                        objsales.returned_at = outDate;
                                    }
                                    // objsales.returned_at = Convert.ToDateTime(item.ReturnDate);
                                }
                                dba.tbl_sales_order.Add(objsales);
                                dba.SaveChanges();

                                tbl_sales_order_details objsaledetails = new tbl_sales_order_details();

                                objsaledetails.quantity_ordered = Convert.ToInt32(item.ProductQty);
                                objsaledetails.quantity_shipped = Convert.ToInt32(item.ProductQty);
                                objsaledetails.product_name = item.ProductName;
                                objsaledetails.order_item_id = item.OrderItemID;
                                objsaledetails.shipping_price_curr_code = "INR";
                                objsaledetails.item_price_curr_code = "INR";
                                objsaledetails.item_tax_amount = Convert.ToDouble(item.item_total_gst);



                                objsaledetails.item_price_amount = Convert.ToDouble(differ1);
                                objsaledetails.shipping_price_Amount = Convert.ToDouble(item.ShippingChargesTotal);
                                objsaledetails.is_active = 1;
                                objsaledetails.tbl_seller_id = seller_id;
                                objsaledetails.tbl_sales_order_id = objsales.id;
                                objsaledetails.status_updated_by = seller_id;
                                objsaledetails.status_updated_on = DateTime.Now;
                                objsaledetails.sku_no = item.MerchantSKU; //item.ProductID;
                                objsaledetails.n_order_status_id = Convert.ToInt16(objsales.n_item_orderstatus);
                                objsaledetails.amazon_order_id = objsales.amazon_order_id;
                                objsaledetails.is_tax_calculated = 0;
                                objsaledetails.tax_updatedby_taxfile = 0;
                                objsaledetails.product_id = item.ProductID;
                                dba.tbl_sales_order_details.Add(objsaledetails);
                                dba.SaveChanges();

                                tbl_tax objtax = new tbl_tax();
                                objtax.tbl_seller_id = seller_id;
                                objtax.tbl_referneced_id = objsaledetails.id;
                                objtax.reference_type = 3;
                                objtax.isactive = 1;
                                if (item.item_igst != "" && item.item_igst != null)
                                {
                                    objtax.igst_tax = Convert.ToDouble(item.item_igst);
                                    objtax.Igst_amount = Convert.ToDouble(item.item_igst_amount);
                                }
                                else
                                {
                                    objtax.cgst_tax = Convert.ToDouble(item.item_cgst);
                                    objtax.sgst_tax = Convert.ToDouble(item.item_sgst);
                                    objtax.CGST_amount = Convert.ToDouble(item.item_cgst_amount);
                                    objtax.sgst_amount = Convert.ToDouble(item.item_sgst_amount);
                                }

                                dba.tbl_tax.Add(objtax);
                                dba.SaveChanges();
                            }
                            #endregion

                        }// end of item==1
                        else if (item.gst_item_direction == "2")
                        {

                            var get_historydata = dba.tbl_order_history.Where(a => a.tbl_seller_id == seller_id && a.t_order_status == 9 && a.OrderID == orderid && a.tbl_marketplace_id == marketplaceID && a.SKU == item.MerchantSKU).FirstOrDefault();
                            if (get_historydata != null)
                            {
                                //double itempr1 = Convert.ToDouble(item.item_taxable_value) * (-1);
                                //double shipp1 = Convert.ToDouble(item.ShippingChargesTotal);
                                //double differ1 = itempr1 - shipp1;
                                 get_historydata.ItemCreditNote = item.item_credit_note;
                                //get_historydata.amount_per_unit = -differ1;
                                //get_historydata.shipping_price = -shipp1;
                               // get_historydata.product_tax = Convert.ToDouble(item.item_total_gst);
                                get_historydata.physically_type = 1;
                                get_historydata.physically_selected_date = DateTime.Now;

                                dba.Entry(get_historydata).State = EntityState.Modified;
                                dba.SaveChanges();
                            }
                            //else
                            //{
                            //    tbl_order_history objhistory = new tbl_order_history();
                            //    double itempr1 = Convert.ToDouble(item.item_taxable_value) * (-1);
                            //    double shipp1 = Convert.ToDouble(item.ShippingChargesTotal);
                            //    double differ1 = itempr1 - shipp1;
                            //    objhistory.ItemCreditNote = item.item_credit_note;
                            //    objhistory.amount_per_unit = -differ1;
                            //    objhistory.shipping_price = -shipp1;
                            //    objhistory.product_tax = Convert.ToDouble(item.item_total_gst);
                            //    objhistory.physically_type = 1;
                            //    objhistory.physically_selected_date = DateTime.Now;


                            //    objhistory.tbl_seller_id = seller_id;
                            //    objhistory.updated_by = seller_id;
                            //    objhistory.created_on = DateTime.Now;
                            //    objhistory.tbl_marketplace_id = marketplaceID;
                            //    objhistory.SKU = item.MerchantSKU; //item.ProductID;
                            //    objhistory.Quantity = 1;
                            //    objhistory.OrderID = item.OrderID;
                            //    objhistory.unique_order_id = item.OrderID + "_" + item.OrderItemID;
                            //    objhistory.t_order_status = 9;

                            //    if (item.OrderCreationDate.Contains('-') && item.OrderCreationDate.Contains(':'))
                            //    {
                            //        var dateFormat = item.OrderCreationDate.Split('-')[0].Length == 4 ? "yyyy-MM-dd" : "dd-MM-yyyy";
                            //        var timeFormat = item.OrderCreationDate.Split(':').Length == 1 ? "HH:mm" : "HH:mm:ss";

                            //        objhistory.ShipmentDate = DateTime.ParseExact(item.OrderCreationDate, dateFormat +" "+ timeFormat, CultureInfo.InvariantCulture);
                            //    }
                            //    else if (item.OrderCreationDate.Contains('-'))
                            //    {
                            //        var dateFormat = item.OrderCreationDate.Split('-')[0].Length == 4 ? "yyyy-MM-dd" : "dd-MM-yyyy";
                            //        objhistory.ShipmentDate = DateTime.ParseExact(item.OrderCreationDate, dateFormat, CultureInfo.InvariantCulture);
                            //    }
                            //    else
                            //    {
                            //        DateTime outDate;
                            //        DateTime.TryParse(item.OrderCreationDate, out outDate);
                            //        objhistory.ShipmentDate = outDate;
                            //    }
                              
                            //    objhistory.LastUpdatedDateUTC = DateTime.Now;
                            //    dba.tbl_order_history.Add(objhistory);
                            //    dba.SaveChanges();
                            //}
                        }// end of elseif
                    }// end of foreach

                    string connectionstring = ConfigurationManager.ConnectionStrings["SellerConnection"].ConnectionString;
                    MySqlConnection con = new MySqlConnection(connectionstring);

                    int uniqueupload = 0;
                    int diff_order = 0;
                    int total_order = 0;
                    var get_unique_count_details = "SELECT count(DISTINCT aa.id) FROM(SELECT DISTINCT Order_Id AS id FROM `tbl_settlement_order` where tbl_seller_id = " + seller_id + " UNION ALL SELECT DISTINCT amazon_order_id AS id FROM `tbl_sales_order` where tbl_sellers_id =" + seller_id + " ) AS aa";
                    MySqlCommand cmd = new MySqlCommand(get_unique_count_details, con);
                    con.Open();
                    MySqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        dr.Read();
                        uniqueupload = Convert.ToInt32(dr[0]);
                    }
                    cmd.Dispose();
                    con.Close();

                    MySqlConnection con1 = new MySqlConnection(connectionstring);
                    var get_unique_orderby_id = "SELECT count( aa.id )FROM (SELECT tbl_order_upload_id AS id FROM `tbl_sales_order` where tbl_sellers_id =" + seller_id + " && tbl_order_upload_id = " + id + ") AS aa";
                    MySqlCommand cmd2 = new MySqlCommand(get_unique_orderby_id, con1);
                    con1.Open();
                    MySqlDataReader dr1 = cmd2.ExecuteReader();
                    if (dr1.HasRows)
                    {
                        dr1.Read();
                        total_order = Convert.ToInt32(dr1[0]);
                    }
                    cmd2.Dispose();
                    con1.Close();

                    var get_balance_details = db.tbl_user_login.Where(aa => aa.tbl_sellers_id == SellerId).FirstOrDefault();
                    if (get_balance_details != null)
                    {
                        var last_order = get_balance_details.total_orders;
                        diff_order = Convert.ToInt16(uniqueupload - last_order);
                        var plan_rate = get_balance_details.applied_plan_rate * diff_order;
                        get_balance_details.wallet_balance = get_balance_details.wallet_balance - Convert.ToDouble(plan_rate);
                        int totalOrder = Convert.ToInt16(diff_order);
                        if (get_balance_details.total_orders != null)
                            totalOrder = totalOrder + Convert.ToInt16(get_balance_details.total_orders);
                        get_balance_details.total_orders = totalOrder;
                        db.Entry(get_balance_details).State = EntityState.Modified;
                        db.SaveChangesAsync();
                    }
                    if (get_upload_order_details != null)
                    {
                        get_upload_order_details.new_order_uploaded = total_order;

                        //get_upload_order_details.from_date = string.IsNullOrEmpty(startdate) ? (DateTime?)null : Convert.ToDateTime(startdate);                    

                        if (string.IsNullOrEmpty(startdate))
                        {
                            get_upload_order_details.from_date = (DateTime?)null;
                        }
                        else
                        {
                            if (startdate.Contains('-') && startdate.Contains(':'))
                            {
                                var dateFormat = startdate.Split('-')[0].Length == 4 ? "yyyy-MM-dd" : "dd-MM-yyyy";
                                var timeFormat = startdate.Split(':').Length == 1 ? "HH:mm" : "HH:mm:ss";

                                get_upload_order_details.from_date = DateTime.ParseExact(startdate, dateFormat + " " + timeFormat, CultureInfo.InvariantCulture);
                            }
                            else if (startdate.Contains('-'))
                            {
                                var dateFormat = startdate.Split('-')[0].Length == 4 ? "yyyy-MM-dd" : "dd-MM-yyyy";
                                get_upload_order_details.from_date = DateTime.ParseExact(startdate, dateFormat, CultureInfo.InvariantCulture);
                            }
                            else
                            {
                                DateTime outDate;
                                DateTime.TryParse(startdate, out outDate);
                                get_upload_order_details.from_date = outDate;
                            }
                        }

                        //get_upload_order_details.to_date = string.IsNullOrEmpty(enddate) ? (DateTime?)null : Convert.ToDateTime(enddate);
                        if (string.IsNullOrEmpty(enddate))
                        {
                            get_upload_order_details.to_date = (DateTime?)null;
                        }
                        else
                        {
                            if (enddate.Contains('-') && enddate.Contains(':'))
                            {
                                var dateFormat = enddate.Split('-')[0].Length == 4 ? "yyyy-MM-dd" : "dd-MM-yyyy";
                                var timeFormat = enddate.Split(':').Length == 1 ? "HH:mm" : "HH:mm:ss";

                                get_upload_order_details.to_date = DateTime.ParseExact(enddate, dateFormat + " " + timeFormat, CultureInfo.InvariantCulture);
                            }
                            else if (startdate.Contains('-'))
                            {
                                var dateFormat = enddate.Split('-')[0].Length == 4 ? "yyyy-MM-dd" : "dd-MM-yyyy";
                                get_upload_order_details.to_date = DateTime.ParseExact(enddate, dateFormat, CultureInfo.InvariantCulture);
                            }
                            else
                            {
                                DateTime outDate;
                                DateTime.TryParse(enddate, out outDate);
                                get_upload_order_details.to_date = outDate;
                            }
                        }

                        get_upload_order_details.status = "Completed";
                        get_upload_order_details.completed_datetime = DateTime.Now;
                        dba.Entry(get_upload_order_details).State = EntityState.Modified;
                    }
                    dba.SaveChangesAsync();

                    if (getpaytmdetails != null)
                    {
                        foreach (var dd in getpaytmdetails)
                        {
                            dd.Status = 1;
                            dba.Entry(dd).State = EntityState.Modified;
                            dba.SaveChanges();

                        }
                    }

                }
            }
            catch (Exception ex)
            {
            }
            return success;
        }
        #endregion

        #endregion

        #region Call OrderAPI
        /// <summary>
        /// this is for calling SalesOrderAPI
        /// </summary>
        public void SalesOrderAPICrown()
        {
            int sellers_id = 0;
            try
            {
                var get_details = dba.tbl_order_upload.Where(a => a.type == 3 && a.source == 2 && a.status == "Queued").ToList();
                if (get_details != null)
                {
                    foreach (var item in get_details)
                    {
                        sellers_id = Convert.ToInt16(item.tbl_seller_id);
                        int marketplaceID = Convert.ToInt32(item.tbl_Marketplace_id);
                        DateTime txt_from = Convert.ToDateTime(item.from_date);
                        DateTime txt_to = Convert.ToDateTime(item.to_date);
                        int id = item.id;
                        Get_Seller_API_DATA obj = new Get_Seller_API_DATA();
                        if (marketplaceID == 3)
                        {
                            obj.Collect_Seller(marketplaceID, txt_from, txt_to, sellers_id, id);
                        }
                        else if (marketplaceID == 1)
                        {
                            GetFlipkartSalesOrderSearchData(marketplaceID, txt_from, txt_to, sellers_id, id);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Writelog log = new Writelog();
                log.write_exception_log(sellers_id.ToString(), "CronJobController", "SalesOrderAPICrown", DateTime.Now, ex);
            }
            //return View("Index");
        }
        #endregion


        #region Call SettlementAPI
        /// <summary>
        /// this is for calling SettlementAPI
        /// </summary>
        public void SettlementAPICrown()
        {
            int seller_id = 0;
            try
            {
                var get_settlement_details = dba.tbl_settlement_upload.Where(a => a.file_status == "Queued" && a.Source == 2).ToList();
                if (get_settlement_details != null)
                {
                    foreach (var item in get_settlement_details)
                    {
                        seller_id = Convert.ToInt16(item.tbl_seller_id);
                        DateTime txt_from = Convert.ToDateTime(item.request_fromdate);
                        DateTime txt_to = Convert.ToDateTime(item.request_date_to);
                        short sourcetype = Convert.ToInt16(item.Source);
                        int marketplaceid = Convert.ToInt16(item.market_place_id);
                        int id = item.Id;

                        var get_settlement = dba.tbl_settlement_upload.Where(a => a.Id == id).FirstOrDefault();
                        if (get_settlement != null)
                        {
                            get_settlement.file_status = "Processing";
                            get_settlement.processing_datetime = DateTime.Now;
                            dba.Entry(get_settlement).State = EntityState.Modified;
                            dba.SaveChanges();
                        }
                        Get_Seller_API_DATA obj = new Get_Seller_API_DATA();
                        obj.Collect_Seller_Settlement(marketplaceid, txt_from, txt_to, seller_id, id);
                    }
                }
            }
            catch (Exception ex)
            {
                Writelog log = new Writelog();
                log.write_exception_log(seller_id.ToString(), "CronJobController", "SettlementAPICrown", DateTime.Now, ex);
            }

            //return View("Index");
        }
        #endregion

        #region callReturnDataCrown
        /// <summary>
        /// this is for save Return Data from uploaded txt file
        /// </summary>
        List<ReturensDataDetail> objdata = null;
        public void ReturnDataCall()
        {
            int seller_id = 0;
            try
            {
                var get_return_details = dba.tbl_return_upload.Where(a => a.file_status == "Queued" && a.source == 1).ToList();
                if (get_return_details != null)
                {
                    foreach (var item in get_return_details)
                    {
                        seller_id = Convert.ToInt16(item.tbl_seller_id);
                        string filename = item.file_name;
                        //int settlementuploadtype = Convert.ToInt16(item.settlement_type);
                        short sourcetype = Convert.ToInt16(item.source);
                        int marketplaceid = Convert.ToInt16(item.tbl_marketplace_id);
                        int id = item.id;
                        string path = System.IO.Path.Combine(Server.MapPath("~/UploadExcel/" + seller_id + "/Return/" + filename));

                        var getdetails = dba.tbl_return_upload.Where(a => a.id == id).FirstOrDefault();
                        if (getdetails != null)
                        {
                            getdetails.processing_time = DateTime.Now;
                            dba.Entry(getdetails).State = EntityState.Modified;
                            dba.SaveChanges();
                        }
                        Browse_Upload_Excel_Utility obj = new Browse_Upload_Excel_Utility();
                        string success = "";
                        if (marketplaceid == 3)
                        {
                            //Amazon
                            try
                            {
                                objdata = ReadReturnData(path);
                            }
                            catch (Exception e)
                            {
                            }
                        }
                        if (objdata != null)
                        {
                            success = SaveBulkReturndata(objdata, id, marketplaceid, seller_id);// for save 
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Writelog log = new Writelog();
                log.write_exception_log(seller_id.ToString(), "CronJobController", "ReturnDataCall", DateTime.Now, ex);
            }

            //eturn View("Index");
        }

        public List<ReturensDataDetail> ReadReturnData(string strFilePath)
        {
            List<ReturensDataDetail> objdata = new List<ReturensDataDetail>();
            List<ReturnsData> objreconciliationorder = new List<ReturnsData>();
            try
            {
                DataTable datatable = new DataTable();
                StreamReader streamreader = new StreamReader(strFilePath);
                char[] delimiter = new char[] { '\t' };
                string[] columnheaders = streamreader.ReadLine().Split(delimiter);
                int colCnt = 0;

                foreach (string columnheader in columnheaders)
                {
                    datatable.Columns.Add(columnheader); // I've added the column headers here.
                    colCnt++;
                }
                while (streamreader.Peek() > 0)
                {
                    DataRow datarow = datatable.NewRow();
                    datarow.ItemArray = streamreader.ReadLine().Split(delimiter);
                    datatable.Rows.Add(datarow);
                }
                foreach (DataRow row in datatable.Rows)
                {
                    ReturnsData obj_item = new ReturnsData();
                    obj_item.ReturnDate = row[0].ToString();
                    obj_item.Order_id = row[1].ToString();
                    obj_item.sku = row[2].ToString();
                    obj_item.Asin = row[3].ToString();
                    obj_item.Fnsku = row[4].ToString();
                    obj_item.Product_name = row[5].ToString();
                    obj_item.quantity = row[6].ToString();
                    obj_item.fullfillment_centerid = row[7].ToString();
                    obj_item.DetailedDepostion = row[8].ToString();
                    obj_item.Reason = row[9].ToString();
                    obj_item.Licence_paltenumber = row[10].ToString();
                    obj_item.Customer_comments = row[11].ToString();
                    objreconciliationorder.Add(obj_item);
                }// end of for each loop
                objdata.Add(new ReturensDataDetail
                {
                    ReturnsDataDetails = objreconciliationorder,
                });
            }// end of try block

            catch (Exception ex)
            {
                Writelog log = new Writelog();
                log.write_exception_log("0", "CronJobController", "ReadReturnData", DateTime.Now, ex);
                return null;
            }// end of catch block
            return objdata;
        }

        public string SaveBulkReturndata(List<ReturensDataDetail> objdata, int upload_tbl_id, int marketplaceID, int SellerId)
        {
            string Reference_No = "";
            string datasaved = "S";
            try
            {
                if (objdata != null)
                {
                    int uniqueupload = 0;
                    tbl_order_history obj_history = null;


                    foreach (var item in objdata[0].ReturnsDataDetails)
                    {
                        var get_inventory_details = dba.tbl_inventory.Where(a => a.tbl_sellers_id == SellerId && a.sku == item.sku).FirstOrDefault();
                        if (get_inventory_details == null)
                        {
                            tbl_inventory obj_inventory = new tbl_inventory();
                            obj_inventory.sku = item.sku;
                            obj_inventory.item_name = item.Product_name;
                            obj_inventory.tbl_sellers_id = SellerId;
                            obj_inventory.isactive = 1;
                            //obj_inventory.tbl_item_category_id = 19;
                            //obj_inventory.tbl_item_subcategory_id = 14;
                            dba.tbl_inventory.Add(obj_inventory);
                            dba.SaveChanges();

                        }
                        obj_history = new tbl_order_history();
                        obj_history.ASIN = item.Asin;
                        obj_history.return_date = Convert.ToDateTime(item.ReturnDate);
                        obj_history.SKU = item.sku;
                        obj_history.OrderID = item.Order_id;
                        obj_history.tbl_marketplace_id = marketplaceID;
                        obj_history.tbl_return_upload_id = upload_tbl_id;
                        obj_history.t_order_status = 10;
                        obj_history.tbl_seller_id = SellerId;
                        if (item.quantity != null && item.quantity != "" && item.quantity != "0")
                        {
                            obj_history.Quantity = Convert.ToInt16(item.quantity);
                        }
                        dba.tbl_order_history.Add(obj_history);
                        dba.SaveChanges();

                        tbl_history_return_details obj_details = new tbl_history_return_details();
                        obj_details.tbl_history_id = obj_history.Id;
                        obj_details.tbl_seller_id = SellerId;
                        obj_details.customer_comments = item.Customer_comments;
                        obj_details.detailed_disposition = item.DetailedDepostion;
                        obj_details.fnsku = item.Fnsku;
                        obj_details.license_plate_no = item.Licence_paltenumber;
                        obj_details.reason = item.Reason;
                        obj_details.fullfillment_center_id = item.fullfillment_centerid;
                        dba.tbl_history_return_details.Add(obj_details);
                        dba.SaveChanges();
                    }// end of for each loop

                    string connectionstring = ConfigurationManager.ConnectionStrings["SellerConnection"].ConnectionString;
                    MySqlConnection con = new MySqlConnection(connectionstring);
                    var get_unique_count_details = "SELECT count(DISTINCT aa.id) FROM(SELECT DISTINCT amazon_order_id AS id FROM `tbl_sales_order` where tbl_sellers_id = " + SellerId + " UNION ALL SELECT DISTINCT OrderID AS id FROM `tbl_order_history` where tbl_seller_id =" + SellerId + " UNION ALL SELECT DISTINCT Order_Id AS id FROM `tbl_settlement_order` where tbl_seller_id =" + SellerId + " ) AS aa";
                    MySqlCommand cmd = new MySqlCommand(get_unique_count_details, con);
                    con.Open();
                    MySqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        dr.Read();
                        uniqueupload = Convert.ToInt32(dr[0]);
                    }
                    cmd.Dispose();
                    con.Close();
                    var diff_order = 0;
                    int totalOrder = 0;
                    var get_balance_details = db.tbl_user_login.Where(a => a.tbl_sellers_id == SellerId).FirstOrDefault();
                    if (get_balance_details != null)
                    {
                        var last_order = get_balance_details.total_orders;
                        diff_order = Convert.ToInt16(uniqueupload - last_order);
                        var plan_rate = get_balance_details.applied_plan_rate * diff_order;
                        get_balance_details.wallet_balance = get_balance_details.wallet_balance - Convert.ToDouble(plan_rate);
                        totalOrder = Convert.ToInt16(diff_order);
                        if (get_balance_details.total_orders != null)
                            totalOrder = totalOrder + Convert.ToInt16(get_balance_details.total_orders);
                        get_balance_details.total_orders = totalOrder;
                        db.Entry(get_balance_details).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    var get_upload_return = dba.tbl_return_upload.Where(a => a.tbl_seller_id == SellerId && a.id == upload_tbl_id).FirstOrDefault();
                    if (get_upload_return != null)
                    {
                        get_upload_return.return_count = diff_order;
                        get_upload_return.file_status = "Completed";
                        get_upload_return.complition_time = DateTime.Now;
                        dba.Entry(get_upload_return).State = EntityState.Modified;
                        dba.SaveChanges();
                    }
                }// end of if(objdata)
            }
            catch (Exception ex)
            {
                Writelog log = new Writelog();
                log.write_exception_log(SellerId.ToString(), "CronJobController", "SaveBulkReturndata", DateTime.Now, ex);
                datasaved = "F" + ex.Message.ToString();
            }// end of catch()
            return datasaved;
        }
        #endregion


        #region Call ProductAPI
        /// <summary>
        /// this is for calling Product API 
        /// </summary>
        public void ProductListAPI()
        {
            var get_productdetails = dba.tbl_product_upload.Where(a => a.status == "Queued" && a.source == 1).ToList();
            if (get_productdetails != null)
            {
                foreach (var item in get_productdetails)
                {
                    int seller_id = Convert.ToInt16(item.tbl_seller_id);
                    DateTime txt_from = Convert.ToDateTime(item.from_date);
                    DateTime txt_to = Convert.ToDateTime(item.to_datetime);
                    short sourcetype = Convert.ToInt16(item.source);
                    int marketplaceid = Convert.ToInt16(item.tbl_marketplace_id);
                    int id = item.Id;

                    var get_product = dba.tbl_product_upload.Where(a => a.Id == id).FirstOrDefault();
                    if (get_product != null)
                    {
                        get_product.status = "Processing";
                        get_product.processing_datetime = DateTime.Now;
                        dba.Entry(get_product).State = EntityState.Modified;
                        dba.SaveChanges();
                    }
                    Get_Seller_API_DATA obj = new Get_Seller_API_DATA();
                    obj.Collect_Product_List(marketplaceid, txt_from, txt_to, seller_id, id);
                }
            }

            //return View("Index");
        }
        #endregion


        #region FlipkartApi
        /// <summary>
        /// call flipkart api for sale order and order details also
        /// </summary>
        List<parseJson> objjson = null;
        List<shipmentDetails> objshipment = new List<shipmentDetails>();
        public string GetFlipkartSalesOrderSearchData(int? ddl_marketplaceAPI, DateTime? txt_from, DateTime? txt_to, int sellers_id, int? id)
        {
            List<parseJson> lstAPIData = new List<parseJson>();
            parseJson SearchResponseNew = new parseJson();
            shipmentDetails objresponse = new shipmentDetails();
            try
            {
                List<seller_parameter_list> lstSeller = new List<seller_parameter_list>();
                string token = "";
                var seller_List_Admin = db.tbl_sellers.Where(a => a.isactive == 1 && a.id == sellers_id).FirstOrDefault();
                if (seller_List_Admin != null)
                {
                    int strId = Convert.ToInt32(seller_List_Admin.id);
                    var seller_Market_List = dba.tbl_sellermarketplace.Where(a => a.isactive == 1 && a.tbl_seller_id == strId && a.m_marketplace_id == ddl_marketplaceAPI).FirstOrDefault();

                    if (seller_Market_List != null)
                    {
                        seller_parameter_list obj = new seller_parameter_list();
                        token = seller_Market_List.t_auth_token.ToString();
                        obj.id = seller_Market_List.id.ToString();
                        obj.my_unique_id = seller_Market_List.my_unique_id.ToString(); ///// SELLER ID 
                        obj.my_seller_id = seller_Market_List.tbl_seller_id.ToString(); ///// SELLER ID - Primary Key Value                           
                        obj.t_access_Key_id = seller_Market_List.t_access_Key_id.ToString();
                        obj.t_auth_token = seller_Market_List.t_auth_token.ToString();
                        obj.t_secret_Key = seller_Market_List.t_secret_Key.ToString();
                        obj.market_palce_id = seller_Market_List.market_palce_id.ToString();
                        obj.m_marketplace_id = seller_Market_List.m_marketplace_id.ToString();
                        lstSeller.Add(obj);
                    }
                }
                SalesOrderRequest searchrequest = new SalesOrderRequest();
                searchrequest.filter = new filter();
                searchrequest.filter.orderDate = new orderDate();
                searchrequest.filter.orderDate.fromDate = Convert.ToDateTime(txt_from).ToString("yyyy-MM-dd");//"2018-01-01";
                searchrequest.filter.orderDate.toDate = Convert.ToDateTime(txt_to).ToString("yyyy-MM-dd");//"2018-01-31";


                string response = CallFlipkartSalesOrderAPI(searchrequest, token);
                //ParseJson(response);
                SearchResponseNew = JsonConvert.DeserializeObject<parseJson>(response);
                lstAPIData.Add(SearchResponseNew);
                string itemorderid = "";
                string comma = ",";
                if (lstAPIData.Count > 0)
                {
                    foreach (var item in lstAPIData[0].orderItems)
                    {
                        if (itemorderid == "")
                            itemorderid = item.orderItemId;
                        else
                            itemorderid = itemorderid + comma + item.orderItemId;
                    }
                }
                string response1 = "";
                if (itemorderid != "" && itemorderid != null)
                {
                    response1 = CallFlipkartOrderDetailsAPI(itemorderid, token);
                    objresponse = JsonConvert.DeserializeObject<shipmentDetails>(response1);
                    //lstAPIData.Add(objresponse);
                }
                parseresult(response, response1, ddl_marketplaceAPI, sellers_id, id);
            }
            catch (Exception ex)
            {
                //return null;
            }
            return "";

        }

        public string CallFlipkartSalesOrderAPI(SalesOrderRequest objsalesorderrequest, string token)
        {
            string responseMessage = string.Empty;
            try
            {
                // string sss=@"{\"filter\": {\"orderDate\": { \"fromDate\": "2017-12-01","toDate": "2017-12-05" } }}";
                //string publicKey = "c48983b1-a228-4afa-9753-cc5f124c14ba";
                //string appid = "ba56584a49a0293089669a980aabb465b800";
                //string appsecret = "38ad1904a2ebc2eb00a24960a3cfa3956";

                string baseAddress = "https://api.flipkart.net/";
                string apiPath = "sellers/orders/search";

                var request = (HttpWebRequest)WebRequest.Create(baseAddress + apiPath);
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Accept = "application/json";

                string jsonOrder = JsonConvert.SerializeObject(objsalesorderrequest);
                var data = Encoding.UTF8.GetBytes(jsonOrder);
                request.ContentLength = data.Length;
                request.Headers["Authorization"] = token;//"Bearer 66a71783-c252-40f9-96f9-5c3520e81ae3";
                Stream streams = request.GetRequestStream();
                streams.Write(data, 0, data.Length);
                streams.Close();

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream dataStream = response.GetResponseStream();
                if (((HttpWebResponse)response).StatusDescription == "OK")
                {
                    using (var stream = response.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(stream);
                        responseMessage = reader.ReadToEnd();
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return responseMessage;
        }



        public string CallFlipkartOrderDetailsAPI(string itemorderid, string token)
        {
            string responseMessage1 = string.Empty;
            try
            {
                string baseAddress = "https://api.flipkart.net";
                string apiPath = "/sellers/v2/orders/shipments?orderItemIds=" + itemorderid;

                var request = (HttpWebRequest)WebRequest.Create(baseAddress + apiPath);
                request.Method = "GET";
                request.Accept = "application/json";
                request.Headers["Authorization"] = token;//"Bearer 66a71783-c252-40f9-96f9-5c3520e81ae3";
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream dataStream = response.GetResponseStream();
                if (((HttpWebResponse)response).StatusDescription == "OK")
                {
                    using (var stream = response.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(stream);
                        responseMessage1 = reader.ReadToEnd();
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return responseMessage1;
        }

        public string parseresult(string response, string response1, int? ddl_marketplaceAPI, int sellers_id, int? id)
        {
            objjson = new List<parseJson>();
            List<orderItems> objorderitem = null;
            priceComponents obj = new priceComponents();
            try
            {
                if (response != "" && response != null)
                {

                    var _data = JsonConvert.DeserializeObject<parseJson>(response);

                    if (_data != null)
                    {
                        int kk = 0;
                        objorderitem = new List<orderItems>();

                        foreach (var d in _data.orderItems)
                        {
                            if (d.orderId == "OD111190234835374000")
                            {

                            }
                            orderItems obj_item = new orderItems();
                            kk++;
                            obj_item.orderItemId = d.orderItemId;
                            obj_item.orderId = d.orderId;
                            obj_item.hsn = d.hsn;
                            obj_item.status = d.status;
                            obj_item.hold = d.hold;
                            obj_item.orderDate = d.orderDate;
                            obj_item.dispatchAfterDate = d.dispatchAfterDate;
                            obj_item.dispatchByDate = d.dispatchByDate;
                            obj_item.updatedAt = d.updatedAt;
                            obj_item.sla = d.sla;
                            obj_item.title = d.title;
                            obj_item.listingId = d.listingId;
                            obj_item.fsn = d.fsn;
                            obj_item.sku = d.sku;
                            obj_item.shippingPincode = d.shippingPincode;
                            obj_item.paymentType = d.paymentType;
                            obj_item.quantity = d.quantity;
                            obj_item.price = d.price;
                            obj_item.shippingFee = d.shippingFee;
                            obj_item.sellingPrice = d.priceComponents.sellingPrice;
                            obj_item.customerPrice = d.priceComponents.customerPrice;
                            obj_item.shippingCharge = d.priceComponents.shippingCharge;
                            obj_item.totalPrice = d.priceComponents.totalPrice;
                            obj_item.shipmentDetails = new List<shipments>();

                            if (response1 != null && response1 != "")
                            {
                                var _data1 = JsonConvert.DeserializeObject<shipmentDetails>(response1);
                                if (_data1 != null)
                                {

                                    foreach (var item in _data1.shipments)
                                    {
                                        if (item != null)
                                        {

                                            if (obj_item.orderId == item.orderId)
                                            {
                                                shipments obj_ship = new shipments();

                                                obj_ship.orderId = item.orderId;
                                                obj_ship.shipmentId = item.shipmentId;
                                                obj_ship.weighingRequired = item.weighingRequired;
                                                if (item.returnAddress != null)
                                                {
                                                    obj_ship.returnAddress = item.returnAddress;
                                                }
                                                obj_ship.orderItems = new List<Common.orderItems>();

                                                if (item.orderItems != null)
                                                {
                                                    foreach (var item1 in item.orderItems)
                                                    {
                                                        Common.orderItems objo = new Common.orderItems();
                                                        objo.id = item1.id;
                                                        objo.fragile = item1.fragile;
                                                        objo.dangerous = item1.dangerous;
                                                        objo.large = item1.large;

                                                        obj_ship.orderItems.Add(objo);
                                                    }// end of for each(item1)

                                                }// end of if(item.orders)
                                                obj_ship.deliveryAddress = new deliveryAddress();
                                                if (item.deliveryAddress != null)
                                                {
                                                    obj_ship.deliveryAddress.firstName = item.deliveryAddress.firstName;
                                                    if (item.deliveryAddress.lastName != null && item.deliveryAddress.lastName != "" && item.deliveryAddress.lastName != " ")
                                                    {
                                                        obj_ship.deliveryAddress.lastName = item.deliveryAddress.lastName;
                                                    }
                                                    obj_ship.deliveryAddress.addressLine1 = item.deliveryAddress.addressLine1;
                                                    obj_ship.deliveryAddress.addressLine2 = item.deliveryAddress.addressLine2;
                                                    obj_ship.deliveryAddress.city = item.deliveryAddress.city;
                                                    obj_ship.deliveryAddress.state = item.deliveryAddress.state;
                                                    obj_ship.deliveryAddress.pincode = item.deliveryAddress.pincode;
                                                    obj_ship.deliveryAddress.stateCode = item.deliveryAddress.stateCode;
                                                    obj_ship.deliveryAddress.stateName = item.deliveryAddress.stateName;
                                                    obj_ship.deliveryAddress.landmark = item.deliveryAddress.landmark;
                                                    obj_ship.deliveryAddress.contactNumber = item.deliveryAddress.contactNumber;

                                                }
                                                obj_ship.billingAddress = new billingAddress();
                                                if (item.billingAddress != null)
                                                {
                                                    obj_ship.billingAddress.firstName = item.billingAddress.firstName;
                                                    if (item.billingAddress.lastName != null && item.billingAddress.lastName != "" && item.billingAddress.lastName != " ")
                                                    {
                                                        obj_ship.billingAddress.lastName = item.billingAddress.lastName;
                                                    }
                                                    obj_ship.billingAddress.addressLine1 = item.billingAddress.addressLine1;
                                                    obj_ship.billingAddress.addressLine2 = item.billingAddress.addressLine2;
                                                    obj_ship.billingAddress.city = item.billingAddress.city;
                                                    obj_ship.billingAddress.state = item.billingAddress.state;
                                                    obj_ship.billingAddress.pincode = item.billingAddress.pincode;
                                                    obj_ship.billingAddress.stateCode = item.billingAddress.stateCode;
                                                    obj_ship.billingAddress.stateName = item.billingAddress.stateName;
                                                    obj_ship.billingAddress.landmark = item.billingAddress.landmark;
                                                    obj_ship.billingAddress.contactNumber = item.billingAddress.contactNumber;

                                                }
                                                obj_ship.buyerDetails = new buyerDetails();
                                                if (item.buyerDetails != null)
                                                {
                                                    if (item.buyerDetails.firstName != null)
                                                    {
                                                        obj_ship.buyerDetails.firstName = item.buyerDetails.firstName;
                                                    }
                                                    if (item.buyerDetails.lastName != null)
                                                    {
                                                        obj_ship.buyerDetails.lastName = item.buyerDetails.lastName;
                                                    }
                                                    if (item.buyerDetails.contactNumber != null)
                                                    {
                                                        obj_ship.buyerDetails.contactNumber = item.buyerDetails.contactNumber;
                                                    }
                                                    if (item.buyerDetails.contactNumber != null)
                                                    {
                                                        obj_ship.buyerDetails.contactNumber = item.buyerDetails.contactNumber;
                                                    }
                                                }
                                                if (item.sellerAddress != null)
                                                {
                                                }

                                                obj_item.shipmentDetails.Add(obj_ship);
                                            }// end of if (orderid)
                                        }
                                    }// end of foreach(item)
                                }// end of if(_data1)

                            }// end of if (response1)
                            //}// end of using(streamReader1)

                            objorderitem.Add(obj_item);
                        }// end of foreach(d)
                        objjson.Add(new parseJson
                        {
                            orderItems = objorderitem,
                            //  shipmentDetails = objshipment,
                        });
                    }// end of if(data null)
                    else
                    {
                        int jjj = 0;
                        jjj++;
                    }
                    //saveFlipkartData(objjson,ddl_marketplaceAPI,sellers_id,id);
                    //}

                }// end of if (response)
            }
            catch (Exception ex)
            {
            }
            return "";
        }


        public string saveFlipkartData(List<parseJson> objjson, int? ddl_marketplaceAPI, int sellers_id, int? id)
        {
            string success = "S";
            cf = new comman_function();
            bool ss = cf.session_check();
            try
            {
                tbl_sales_order objsale = new tbl_sales_order();
                tbl_customer_details objcustumer = new tbl_customer_details();
                tbl_sales_order_details objsaledetails = new tbl_sales_order_details();
                //int SellerId = Convert.ToInt32(Session["SellerID"]);
                string datasaved = "";
                string lowername = "Flipkart".ToLower();
                if (objjson != null)
                {
                    var get_upload_order_details = dba.tbl_order_upload.Where(aa => aa.tbl_seller_id == sellers_id && aa.id == id && aa.tbl_Marketplace_id == ddl_marketplaceAPI).FirstOrDefault();
                    if (get_upload_order_details != null)
                    {
                        get_upload_order_details.status = "Processing";
                        get_upload_order_details.processing_datetime = DateTime.Now;
                        dba.Entry(get_upload_order_details).State = EntityState.Modified;
                        dba.SaveChanges();
                    }
                    foreach (var item in objjson[0].orderItems)
                    {
                        string Message;
                        bool flag = false;
                        var getmarketplacedetails = db.m_marketplace.Where(a => a.isactive == 1).ToList();// get details from marketplace table in seller admin
                        var getdetails = dba.tbl_sales_order.Where(a => a.amazon_order_id == item.orderId && a.order_status == item.status).FirstOrDefault();
                        if (getdetails == null)
                        {
                            if (item.shipmentDetails != null && item.shipmentDetails.Count > 0)
                            {
                                if (item.shipmentDetails[0].billingAddress != null)
                                {
                                    objcustumer.Address_1 = item.shipmentDetails[0].billingAddress.addressLine1;
                                    objcustumer.Address_2 = item.shipmentDetails[0].billingAddress.addressLine2;
                                    objcustumer.shipping_Buyer_Name = item.shipmentDetails[0].billingAddress.firstName;
                                    objcustumer.City = item.shipmentDetails[0].billingAddress.city;
                                    objcustumer.Postal_Code = item.shipmentDetails[0].billingAddress.pincode;
                                    objcustumer.State_Region = item.shipmentDetails[0].billingAddress.stateName;
                                    objcustumer.last_name = item.shipmentDetails[0].billingAddress.lastName;
                                    objcustumer.state_code = item.shipmentDetails[0].billingAddress.stateCode;
                                    objcustumer.landmark = item.shipmentDetails[0].billingAddress.landmark;
                                    objcustumer.contact_no = item.shipmentDetails[0].billingAddress.contactNumber;
                                    objcustumer.state = item.shipmentDetails[0].billingAddress.state;
                                    objcustumer.tbl_seller_id = sellers_id;
                                    dba.tbl_customer_details.Add(objcustumer);
                                    dba.SaveChanges();
                                }// end of  if(item.billing address)
                            }// end of if(item.shipping details)

                            objsale.amazon_order_id = item.orderId;
                            objsale.order_item_id = item.orderItemId;
                            objsale.order_status = item.status;
                            objsale.purchase_date = Convert.ToDateTime(item.orderDate);
                            objsale.payment_method = item.paymentType;
                            objsale.bill_amount = Convert.ToDouble(item.price);
                            objsale.tbl_sellers_id = sellers_id;
                            objsale.created_on = DateTime.Now;
                            objsale.is_active = 1;
                            objsale.tbl_Customer_Id = objcustumer.id;
                            objsale.buyer_name = objcustumer.shipping_Buyer_Name;
                            objsale.earliest_ship_date = Convert.ToDateTime(item.dispatchByDate);
                            objsale.dispatch_afterdate = Convert.ToDateTime(item.dispatchAfterDate);
                            objsale.t_Hold = item.hold;
                            objsale.n_item_orderstatus = 1;
                            objsale.tbl_order_upload_id = id;
                            objsale.tbl_Marketplace_Id = Convert.ToInt16(ddl_marketplaceAPI);

                            //foreach (var abc in getmarketplacedetails)
                            //{
                            //    string name = abc.name.ToLower();
                            //    if (lowername == name)
                            //    {
                            //        objsale.tbl_Marketplace_Id = abc.id;
                            //        break;
                            //    }
                            //}//end of for each loop ( for get marketplace details)

                            dba.tbl_sales_order.Add(objsale);
                            dba.SaveChanges();


                            var chkSku = dba.tbl_inventory.Where(a => a.sku == item.sku).FirstOrDefault();
                            if (chkSku == null)
                            {
                                tbl_inventory objInventory = new tbl_inventory();
                                objInventory.sku = item.sku;
                                objInventory.tbl_sellers_id = SellerId;
                                objInventory.item_name = item.title;
                                objInventory.selling_price = Convert.ToInt16(item.sellingPrice);
                                objInventory.isactive = 1;
                                objInventory.hsn_code = item.hsn;
                                objInventory.tbl_marketplace_id = ddl_marketplaceAPI;
                                objInventory.asin_no = item.fsn;
                                dba.tbl_inventory.Add(objInventory);
                                dba.SaveChanges();
                            }

                            objsaledetails.asin = item.fsn;
                            objsaledetails.sku_no = item.sku;
                            objsaledetails.sla_flipkart = Convert.ToInt16(item.sla);
                            objsaledetails.listing_id = item.listingId;
                            objsaledetails.product_name = item.title;
                            objsaledetails.quantity_ordered = item.quantity;
                            objsaledetails.order_item_id = item.orderItemId;
                            objsaledetails.dispatch_bydate = Convert.ToDateTime(item.dispatchByDate);
                            objsaledetails.dispatchAfter_date = Convert.ToDateTime(item.dispatchAfterDate);
                            objsaledetails.shipping_pincode = item.shippingPincode;
                            objsaledetails.selling_price = Convert.ToDouble(item.sellingPrice);
                            objsaledetails.hsn = item.hsn;
                            objsaledetails.amazon_order_id = item.orderId;
                            if (item.priceComponents != null)
                            {
                                objsaledetails.item_price_amount = Convert.ToDouble(item.priceComponents.sellingPrice);
                                if (item.priceComponents.shippingCharge != 0 && item.priceComponents.shippingCharge != null)
                                {
                                    objsaledetails.shipping_price_Amount = Convert.ToDouble(item.priceComponents.shippingCharge);
                                }
                                else
                                {
                                    objsaledetails.shipping_price_Amount = 0;
                                }
                            }
                            if (item.shipmentDetails != null && item.shipmentDetails.Count > 0)
                            {
                                objsaledetails.shipment_Id = item.shipmentDetails[0].shipmentId;
                            }

                            objsaledetails.is_active = 1;
                            objsaledetails.tbl_seller_id = sellers_id;
                            objsaledetails.tbl_sales_order_id = objsale.id;
                            objsaledetails.status_updated_by = sellers_id;
                            objsaledetails.status_updated_on = DateTime.Now;
                            objsaledetails.n_order_status_id = 1;
                            dba.tbl_sales_order_details.Add(objsaledetails);
                            dba.SaveChanges();

                        }// end of if(getdetails)

                    }// end of foreach(item)

                    string connectionstring = ConfigurationManager.ConnectionStrings["SellerConnection"].ConnectionString;
                    MySqlConnection con = new MySqlConnection(connectionstring);

                    int uniqueupload = 0;
                    int diff_order = 0;
                    int total_order = 0;
                    var get_unique_count_details = "SELECT count(DISTINCT aa.id) FROM(SELECT DISTINCT Order_Id AS id FROM `tbl_settlement_order` where tbl_seller_id = " + sellers_id + " UNION ALL SELECT DISTINCT amazon_order_id AS id FROM `tbl_sales_order` where tbl_sellers_id =" + sellers_id + " ) AS aa";
                    MySqlCommand cmd = new MySqlCommand(get_unique_count_details, con);
                    con.Open();
                    MySqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        dr.Read();
                        uniqueupload = Convert.ToInt32(dr[0]);
                    }
                    cmd.Dispose();
                    con.Close();

                    MySqlConnection con1 = new MySqlConnection(connectionstring);
                    var get_unique_orderby_id = "SELECT count( aa.id )FROM (SELECT tbl_order_upload_id AS id FROM `tbl_sales_order` where tbl_sellers_id =" + sellers_id + " && tbl_order_upload_id = " + id + ") AS aa";
                    MySqlCommand cmd2 = new MySqlCommand(get_unique_orderby_id, con1);
                    con1.Open();
                    MySqlDataReader dr1 = cmd2.ExecuteReader();
                    if (dr1.HasRows)
                    {
                        dr1.Read();
                        total_order = Convert.ToInt32(dr1[0]);
                    }
                    cmd2.Dispose();
                    con1.Close();

                    var get_balance_details = db.tbl_user_login.Where(aa => aa.tbl_sellers_id == sellers_id).FirstOrDefault();
                    if (get_balance_details != null)
                    {
                        var last_order = get_balance_details.total_orders;
                        diff_order = Convert.ToInt16(uniqueupload - last_order);
                        var plan_rate = get_balance_details.applied_plan_rate * diff_order;
                        get_balance_details.wallet_balance = get_balance_details.wallet_balance - Convert.ToDouble(plan_rate);
                        int totalOrder = Convert.ToInt16(diff_order);
                        if (get_balance_details.total_orders != null)
                            totalOrder = totalOrder + Convert.ToInt16(get_balance_details.total_orders);
                        get_balance_details.total_orders = totalOrder;
                        db.Entry(get_balance_details).State = EntityState.Modified;
                        db.SaveChangesAsync();
                    }
                    if (get_upload_order_details != null)
                    {
                        get_upload_order_details.new_order_uploaded = total_order;
                        get_upload_order_details.status = "Completed";
                        get_upload_order_details.completed_datetime = DateTime.Now;
                        dba.Entry(get_upload_order_details).State = EntityState.Modified;
                    }
                    dba.SaveChangesAsync();

                }// end of if(objson)
                return datasaved;
            }// end of try
            catch (Exception ex)
            {
                success = "F";
                var get_upload_orderdetails = dba.tbl_order_upload.Where(aa => aa.tbl_seller_id == sellers_id && aa.id == id).FirstOrDefault();
                get_upload_orderdetails.status = "File Format is not Correct or Some Error Occurred";
                get_upload_orderdetails.completed_datetime = DateTime.Now;
                dba.Entry(get_upload_orderdetails).State = EntityState.Modified;
                dba.SaveChangesAsync();
                Writelog log = new Writelog();
                log.write_exception_log(sellers_id.ToString(), "CronJobController", "Flipkart_API_Call", DateTime.Now, ex);
                throw ex;
            }
            return success;
        }
        #endregion





    }
}
