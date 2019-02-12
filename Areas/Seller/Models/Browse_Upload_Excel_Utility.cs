using Newtonsoft.Json;
using SellerVendor.Areas.Seller.Models;
using SellerVendor.Areas.Seller.Models.DBContext;
using SellerVendor.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Xml;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;
using System.Data;
using System.Globalization;
using System.Text.RegularExpressions;
using LinqToExcel;
using System.Web.UI.WebControls;
using System.Data.OleDb;
using System.Configuration;
using MySql.Data.MySqlClient;
using SellerVendor.Areas.Seller.Controllers;

namespace SellerVendor.Areas.Seller.Models
{
    public class Browse_Upload_Excel_Utility
    {
        public SellerContext dba = new SellerContext();
        public SellerAdminContext db = new SellerAdminContext();
        List<AmazonreconciliationOrder> objjson1 = null;
        //public List<AmazonreconciliationOrder> ReconciliationOrder_Browse(string strFilePath)
        //{
        //    List<AmazonreconciliationOrder> objjson1 = new List<AmazonreconciliationOrder>();
        //    List<reconciliationorder> objreconciliationorder = new List<reconciliationorder>();
        //    try
        //    {
        //        string text;
        //        string text1;
        //        Excel.Application xlApp;
        //        Excel.Workbook xlWorkBook;
        //        Excel.Worksheet xlWorkSheet;
        //        Excel.Range range;
        //        DataTable dt = new DataTable();
        //        string str;
        //        int rCnt;
        //        int cCnt;
        //        int rw = 0;
        //        int cl = 0;

        //        xlApp = new Excel.Application();
        //        //xlWorkBook = xlApp.Workbooks.Open(@"c:\\7723918584017438.xls", 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
        //        xlWorkBook = xlApp.Workbooks.Open(strFilePath, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
        //        xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

        //        range = xlWorkSheet.UsedRange;
        //        rw = range.Rows.Count;
        //        cl = range.Columns.Count;
        //        for (int i = 1; i <= rw; i++)
        //        {
        //            DataRow dtrow = dt.NewRow();
        //            for (int j = 1; j <= cl; j++)
        //            {
        //                int cell = j - 1;
        //                var stsr = (range.Cells[i, j] as Excel.Range).Value2;
        //                if (i == 1)
        //                    dt.Columns.Add(stsr);
        //                else
        //                    dtrow[cell] = stsr;
        //            }
        //            if (i != 1)
        //                dt.Rows.Add(dtrow);
        //        }
        //        if (dt.Rows.Count > 0)
        //        {
        //            for (int k = 0; k < dt.Rows.Count; k++)
        //            {
        //                reconciliationorder obj_item = new reconciliationorder();
        //                obj_item.settlement_id = dt.Rows[k][0].ToString();
        //                obj_item.settlement_start_date = dt.Rows[k][1].ToString();
        //                obj_item.settlement_end_date = dt.Rows[k][2].ToString();
        //                obj_item.deposit_date = dt.Rows[k][3].ToString();
        //                obj_item.total_amount = dt.Rows[k][4].ToString();
        //                obj_item.currency = dt.Rows[k][5].ToString();
        //                obj_item.transaction_type = dt.Rows[k][6].ToString();
        //                obj_item.order_id = dt.Rows[k][7].ToString();
        //                obj_item.merchant_order_id = dt.Rows[k][8].ToString();
        //                obj_item.adjustment_id = dt.Rows[k][9].ToString();
        //                obj_item.shipment_id = dt.Rows[k][10].ToString();
        //                obj_item.marketplace_name = dt.Rows[k][11].ToString();
        //                obj_item.amount_type = dt.Rows[k][12].ToString();
        //                obj_item.amount_description = dt.Rows[k][13].ToString();
        //                obj_item.amount = dt.Rows[k][14].ToString();
        //                obj_item.fulfillment_id = dt.Rows[k][15].ToString();
        //                obj_item.posted_date = dt.Rows[k][16].ToString();
        //                obj_item.posted_date_time = dt.Rows[k][17].ToString();
        //                obj_item.order_item_code = dt.Rows[k][18].ToString();
        //                obj_item.merchant_order_item_id = dt.Rows[k][19].ToString();
        //                obj_item.merchant_adjustment_item_id = dt.Rows[k][20].ToString();
        //                obj_item.sku = dt.Rows[k][21].ToString();
        //                obj_item.quantity_purchased = dt.Rows[k][22].ToString();
        //                obj_item.promotion_id = dt.Rows[k][23].ToString();
        //                objreconciliationorder.Add(obj_item);
        //            }
        //        }
        //        objjson1.Add(new AmazonreconciliationOrder
        //        {
        //            reconciliationorder = objreconciliationorder,
        //        });

        //        //Savesettlementdata(objjson1);// for save settlement data in expense table

        //        GC.Collect();
        //        GC.WaitForPendingFinalizers();
        //        Marshal.ReleaseComObject(range);
        //        Marshal.ReleaseComObject(xlWorkSheet);
        //        //close and release
        //        xlWorkBook.Close();
        //        Marshal.ReleaseComObject(xlWorkBook);
        //        //quit and release
        //        xlApp.Quit();
        //        Marshal.ReleaseComObject(xlApp);
        //    }
        //    catch (Exception ex)
        //    {
        //    }

        //    return objjson1;
        //}


        public List<AmazonreconciliationOrder> ReadandBreakSettlement_flipkart(string strFilePath, int id, int marketplaceid, int seller_id)
        {
            List<AmazonreconciliationOrder> objjson1 = new List<AmazonreconciliationOrder>();
            List<reconciliationorder> objreconciliationorder = new List<reconciliationorder>();

            try
            {
                var excel = new ExcelQueryFactory(strFilePath)
                {
                    DatabaseEngine = LinqToExcel.Domain.DatabaseEngine.Ace,
                    TrimSpaces = LinqToExcel.Query.TrimSpacesType.Both,
                    UsePersistentConnection = true,
                    ReadOnly = true
                };

                var worksheetNames = excel.GetWorksheetNames();
                string sheetName = worksheetNames.First();

                sheetName = "Orders";

                var artistAlbums = from a in excel.Worksheet(sheetName) select a;
                Dictionary<string, reconciliationorder> dictionary = new Dictionary<string, reconciliationorder>();
                int i = 0;
                double sett_amount = 0;
                int total = artistAlbums.Count();
                LinqToExcel.Row headerrow = null;
                List<LinqToExcel.Row> lst_Add = new List<LinqToExcel.Row>();
                List<LinqToExcel.Row> lst_prepaid = new List<LinqToExcel.Row>();
                List<LinqToExcel.Row> lst_postpaid = new List<LinqToExcel.Row>();

                if (total >= 0)
                {
                    string lastdate = "";
                    foreach (var item_1 in artistAlbums)
                    {
                        i++;
                        if (i == 1)
                        {
                            headerrow = item_1;
                            continue;
                        }
                        string name = item_1[1].ToString();
                        string date = item_1[2].ToString();

                        if (lastdate == date)
                        {
                            if (name == "Prepaid")
                            {
                                int cou = lst_prepaid.Count;
                                lastdate = date;
                                if (lst_prepaid.Count == 0)
                                {
                                    lst_prepaid.Add(headerrow);
                                }
                                lst_prepaid.Add(item_1);
                            }
                            else if (name == "Postpaid")
                            {
                                int abc = lst_postpaid.Count;
                                lastdate = date;
                                if (lst_postpaid.Count == 0)
                                {
                                    lst_postpaid.Add(headerrow);
                                }
                                lst_postpaid.Add(item_1);
                            }
                        }
                        else
                        {
                            if (name == "Prepaid")
                            {
                                if (lst_prepaid.Count > 0)
                                {
                                    ReadSettlementFile_Flipkart(strFilePath, id, marketplaceid, seller_id, lst_prepaid);
                                    lst_prepaid = new List<LinqToExcel.Row>();
                                }
                                if (lst_postpaid.Count > 0)
                                {
                                    ReadSettlementFile_Flipkart(strFilePath, id, marketplaceid, seller_id, lst_postpaid);
                                    lst_postpaid = new List<LinqToExcel.Row>();
                                }

                                lastdate = date;
                                lst_prepaid.Add(headerrow);
                                lst_prepaid.Add(item_1);
                            }
                            else if (name == "Postpaid")
                            {
                                if (lst_prepaid.Count > 0)
                                {
                                    ReadSettlementFile_Flipkart(strFilePath, id, marketplaceid, seller_id, lst_prepaid);
                                    lst_prepaid = new List<LinqToExcel.Row>();
                                }
                                if (lst_postpaid.Count > 0)
                                {
                                    ReadSettlementFile_Flipkart(strFilePath, id, marketplaceid, seller_id, lst_postpaid);
                                    lst_postpaid = new List<LinqToExcel.Row>();
                                }
                                lastdate = date;
                                lst_postpaid.Add(headerrow);
                                lst_postpaid.Add(item_1);
                            }
                        }
                    }

                    if (lst_prepaid.Count > 1)
                    {
                        ReadSettlementFile_Flipkart(strFilePath, id, marketplaceid, seller_id, lst_prepaid);
                    }
                    if (lst_postpaid.Count > 1)
                    {
                        ReadSettlementFile_Flipkart(strFilePath, id, marketplaceid, seller_id, lst_postpaid);
                    }
                }
            }
            catch (Exception ex)
            {
            }

            return objjson1;
        }

        #region for flipkart new code
        public List<AmazonreconciliationOrder> ReadSettlementFile_Flipkart(string strFilePath, int id, int marketplaceid, int seller_id, List<LinqToExcel.Row> lst_prepaid)
        {
            List<AmazonreconciliationOrder> objjson1 = new List<AmazonreconciliationOrder>();
            List<reconciliationorder> objreconciliationorder = new List<reconciliationorder>();
            try
            {
                var excel = new ExcelQueryFactory(strFilePath)
                {
                    DatabaseEngine = LinqToExcel.Domain.DatabaseEngine.Ace,
                    TrimSpaces = LinqToExcel.Query.TrimSpacesType.Both,
                    UsePersistentConnection = true,
                    ReadOnly = true
                };

                var worksheetNames = excel.GetWorksheetNames();
                string sheetName = worksheetNames.First();

                sheetName = "Orders";

                //var artistAlbums = from a in excel.Worksheet(sheetName) select a;
                Dictionary<string, reconciliationorder> dictionary = new Dictionary<string, reconciliationorder>();
                int i = 0;
                double sett_amount = 0;


                LinqToExcel.Row firstrow = null;// = artistAlbums.ElementAtOrDefault(1); // select 1;
                #region Orders
                foreach (var row in lst_prepaid)
                {
                    i++;
                    if (i == 1)
                    {
                        firstrow = row;
                        continue;
                    }
                    //string artistInfo = "Artist Name: {0}; Album: {1}";
                    if (i == 2)
                    {
                        reconciliationorder obj_item1 = new reconciliationorder();
                        obj_item1.settlement_id = row[0].ToString();
                        obj_item1.deposit_date = row[2].ToString();
                        obj_item1.settlement_start_date = row[16].ToString();
                        if (row[17].ToString() != null && row[17].ToString() != "")
                        {
                            obj_item1.settlement_end_date = row[17].ToString();
                        }
                        else
                        {
                            obj_item1.settlement_end_date = row[16].ToString();
                        }
                        obj_item1.total_amount = "0";
                        objreconciliationorder.Add(obj_item1);
                    }
                    reconciliationorder obj_item = null;
                    string orderidd = row[5].ToString();
                    if (dictionary.ContainsKey(orderidd))
                    {
                        obj_item = dictionary[orderidd];

                        settlement_amt_type subobj = new settlement_amt_type();
                        //if (row[3].ToString() != "")
                        //    subobj.settlement_amount = Math.Round(Convert.ToDouble(row[3].ToString()), 2);
                        if (row[7].ToString() != "")
                            subobj.amount = Math.Round(Convert.ToDouble(row[7].ToString()), 2);
                        if (row[20].ToString() != "")
                            subobj.qty = Convert.ToInt32(row[20].ToString());

                        subobj.posteddatetime = row[16].ToString();
                        subobj.type = row[23].ToString();
                        subobj.description = "TotalSaleAmount";
                        if (row[23].ToString() == "NA")
                        {
                            string sku = row[19].ToString();
                            string order_item_id = row[6].ToString();
                            if (orderidd == "OD111102582989434000")
                            {

                            }

                            if (obj_item.order_amount_typesDict == null)
                                obj_item.order_amount_typesDict = new Dictionary<string, List<settlement_amt_type>>();


                            //------------------------add protection fund ----------------------//
                            if (row[13].ToString() != "" && row[13].ToString() != null && row[13].ToString() != "0")
                            {
                                settlement_amt_type subobj_3 = new settlement_amt_type();
                                if (row[13].ToString() != "")
                                    subobj_3.amount = Math.Round(Convert.ToDouble(row[13].ToString()), 2);
                                else
                                    subobj_3.amount = 0;
                                subobj_3.posteddatetime = row[16].ToString();
                                if (row[20].ToString() != "")
                                    subobj_3.qty = Convert.ToInt32(row[20].ToString());

                                subobj_3.description = "ProtectionFund";
                                obj_item.order_amount_typesDict = new Dictionary<string, List<settlement_amt_type>>();
                                List<settlement_amt_type> li4 = new List<settlement_amt_type>();
                                li4.Add(subobj);
                            }

                            //--------------------------------END--------------------------------//


                            List<settlement_amt_type> li = null;
                            if (obj_item.order_amount_typesDict.ContainsKey(order_item_id))// if (obj_item.order_amount_typesDict.ContainsKey(sku))
                            {
                                li = obj_item.order_amount_typesDict[row[6].ToString()];//li = obj_item.order_amount_typesDict[row[19].ToString()];
                                if (li.Count > 0)
                                {
                                    //if (row[7].ToString() != "" && row[7].ToString() != "0")//new
                                    //{
                                    li.Add(subobj);
                                    //}
                                }
                                else
                                    li.Add(subobj);
                            }
                            else
                            {
                                li = new List<settlement_amt_type>();
                                //if (row[7].ToString() != "" && row[7].ToString() != "0")//new
                                //{
                                li.Add(subobj);
                                obj_item.order_amount_typesDict.Add(order_item_id, li);
                                //}//obj_item.order_amount_typesDict.Add(sku, li);
                            }

                            ///////////////
                            if (row[32].ToString() != null && row[32].ToString() != "0")//Commission
                            {
                                settlement_amt_type subobj1 = new settlement_amt_type();
                                subobj1.amount = Math.Round(Convert.ToDouble(row[32].ToString()), 2);
                                subobj1.description = firstrow[32].ToString().Replace("(Rs.)", "").Trim();
                                li.Add(subobj1);
                            }

                            if (row[33].ToString() != null && row[33].ToString() != "0")//Collection_Fee
                            {
                                settlement_amt_type subobj2 = new settlement_amt_type();
                                subobj2.amount = Math.Round(Convert.ToDouble(row[33].ToString()), 2);
                                subobj2.description = firstrow[33].ToString().Replace("(Rs.)", "").Trim();
                                li.Add(subobj2);
                            }

                            if (row[34].ToString() != null && row[34].ToString() != "0")//Fixed Fee
                            {
                                settlement_amt_type subobj3 = new settlement_amt_type();
                                subobj3.amount = Math.Round(Convert.ToDouble(row[34].ToString()), 2);
                                subobj3.description = firstrow[34].ToString().Replace("(Rs.)", "").Trim();
                                li.Add(subobj3);
                            }

                            if (row[35].ToString() != null && row[35].ToString() != "0")//No Cost Emi Fee Reimbursement
                            {
                                settlement_amt_type subobj4 = new settlement_amt_type();
                                subobj4.amount = Math.Round(Convert.ToDouble(row[35].ToString()), 2);
                                subobj4.description = firstrow[35].ToString().Replace("(Rs.)", "").Trim();
                                li.Add(subobj4);
                            }

                            if (row[36].ToString() != null && row[36].ToString() != "0")//Installation Fee 
                            {
                                settlement_amt_type subobj5 = new settlement_amt_type();
                                subobj5.amount = Math.Round(Convert.ToDouble(row[36].ToString()), 2);
                                subobj5.description = firstrow[36].ToString().Replace("(Rs.)", "").Trim();
                                li.Add(subobj5);
                            }

                            if (row[37].ToString() != null && row[37].ToString() != "0")//Uninstallation Fee
                            {
                                settlement_amt_type subobj6 = new settlement_amt_type();
                                subobj6.amount = Math.Round(Convert.ToDouble(row[37].ToString()), 2);
                                subobj6.description = firstrow[37].ToString().Replace("(Rs.)", "").Trim();
                                li.Add(subobj6);
                            }

                            if (row[38].ToString() != null && row[38].ToString() != "0")//Tech Visit Fee
                            {
                                settlement_amt_type subobj7 = new settlement_amt_type();
                                subobj7.amount = Math.Round(Convert.ToDouble(row[38].ToString()), 2);
                                subobj7.description = firstrow[38].ToString().Replace("(Rs.)", "").Trim();
                                li.Add(subobj7);
                            }

                            if (row[39].ToString() != null && row[39].ToString() != "0")//Uninstallation & Packaging Fee
                            {
                                settlement_amt_type subobj8 = new settlement_amt_type();
                                subobj8.amount = Math.Round(Convert.ToDouble(row[39].ToString()), 2);
                                subobj8.description = firstrow[39].ToString().Replace("(Rs.)", "").Trim();
                                li.Add(subobj8);
                            }

                            if (row[40].ToString() != null && row[40].ToString() != "0")//Pick And Pack Fee
                            {
                                settlement_amt_type subobj9 = new settlement_amt_type();
                                subobj9.amount = Math.Round(Convert.ToDouble(row[40].ToString()), 2);
                                subobj9.description = firstrow[40].ToString().Replace("(Rs.)", "").Trim();
                                li.Add(subobj9);
                            }

                            if (row[41].ToString() != null && row[41].ToString() != "0" && row[41].ToString() != "")//Customer Shipping Fee Type
                            {
                                settlement_amt_type subobj10 = new settlement_amt_type();
                                if (row[41].ToString() != null && row[41].ToString() != "")
                                {
                                    subobj10.amount = Math.Round(Convert.ToDouble(row[41].ToString()), 2);
                                }
                                else
                                {
                                    subobj10.amount = 0;
                                }
                                subobj10.description = firstrow[41].ToString().Replace("(Rs.)", "").Trim();
                                li.Add(subobj10);
                            }
                            if (row[42].ToString() != null && row[42].ToString() != "0")//Customer Shipping Fee
                            {
                                settlement_amt_type subobj11 = new settlement_amt_type();
                                subobj11.amount = Math.Round(Convert.ToDouble(row[42].ToString()), 2);
                                subobj11.description = firstrow[42].ToString().Replace("(Rs.)", "").Trim();
                                li.Add(subobj11);
                            }
                            if (row[43].ToString() != null && row[43].ToString() != "0")//Shipping Fee
                            {
                                settlement_amt_type subobj12 = new settlement_amt_type();
                                subobj12.amount = Math.Round(Convert.ToDouble(row[43].ToString()), 2);
                                subobj12.description = firstrow[43].ToString().Replace("(Rs.)", "").Trim();
                                li.Add(subobj12);
                            }

                            if (row[44].ToString() != null && row[44].ToString() != "0")//Reverse Shipping Fee 
                            {
                                settlement_amt_type subobj13 = new settlement_amt_type();
                                subobj13.amount = Math.Round(Convert.ToDouble(row[44].ToString()), 2);
                                subobj13.description = firstrow[44].ToString().Replace("(Rs.)", "").Trim();
                                li.Add(subobj13);
                            }

                            if (row[45].ToString() != null && row[45].ToString() != "0")//Franchise Fee 
                            {
                                settlement_amt_type subobj14 = new settlement_amt_type();
                                subobj14.amount = Math.Round(Convert.ToDouble(row[45].ToString()), 2);
                                subobj14.description = firstrow[45].ToString().Replace("(Rs.)", "").Trim();
                                li.Add(subobj14);
                            }

                            if (row[46].ToString() != null && row[46].ToString() != "0")//Product Cancellation Fee
                            {
                                settlement_amt_type subobj15 = new settlement_amt_type();
                                subobj15.amount = Math.Round(Convert.ToDouble(row[46].ToString()), 2);
                                subobj15.description = firstrow[46].ToString().Replace("(Rs.)", "").Trim();
                                li.Add(subobj15);
                            }

                            if (row[47].ToString() != null && row[47].ToString() != "0")//Service Cancellation Fee
                            {
                                settlement_amt_type subobj16 = new settlement_amt_type();
                                subobj16.amount = Math.Round(Convert.ToDouble(row[47].ToString()), 2);
                                subobj16.description = firstrow[47].ToString().Replace("(Rs.)", "").Trim();
                                li.Add(subobj16);
                            }

                            if (row[48].ToString() != null && row[48].ToString() != "0")//Fee Discount
                            {
                                settlement_amt_type subobj17 = new settlement_amt_type();
                                subobj17.amount = Math.Round(Convert.ToDouble(row[48].ToString()), 2);
                                subobj17.description = firstrow[48].ToString().Replace("(Rs.)", "").Trim();
                                li.Add(subobj17);
                            }

                            //////
                        }
                        else if (row[23].ToString() == "Courier Return" || row[23].ToString() == "Customer Return")
                        {
                            if (row[5].ToString() == "OD111102582989434000")
                            {
                            }
                            string sku = row[19].ToString();
                            string order_item_id = row[6].ToString();
                            List<settlement_amt_type> li = null;
                            if (obj_item.refund_amount_typesDict == null)
                                obj_item.refund_amount_typesDict = new Dictionary<string, List<settlement_amt_type>>();

                            //------------------------add data in order dictionary------------------//
                            if (row[14].ToString() != "" && row[14].ToString() != null)
                            {
                                List<settlement_amt_type> li1 = null;
                                settlement_amt_type subobj_1 = new settlement_amt_type();
                                if (row[7].ToString() != "")
                                    subobj_1.amount = Math.Round(Convert.ToDouble(row[7].ToString()), 2);
                                else
                                    subobj_1.amount = 0;
                                subobj_1.posteddatetime = row[16].ToString();
                                if (row[20].ToString() != "")
                                    subobj_1.qty = Convert.ToInt32(row[20].ToString());

                                subobj_1.description = "TotalSaleAmount";
                                if (row[14].ToString() != "0")
                                {

                                    if (obj_item.order_amount_typesDict == null)
                                    {
                                        obj_item.order_amount_typesDict = new Dictionary<string, List<settlement_amt_type>>();
                                    }

                                    if (obj_item.order_amount_typesDict.ContainsKey(row[6].ToString()))
                                        li1 = obj_item.order_amount_typesDict[row[6].ToString()];
                                    else
                                    {
                                        li1 = new List<settlement_amt_type>();
                                        obj_item.order_amount_typesDict.Add(row[6].ToString(), li1);
                                    }

                                }
                                else if (row[14].ToString() == "0")
                                {
                                    if (obj_item.order_amount_typesDict == null)
                                    {
                                        obj_item.order_amount_typesDict = new Dictionary<string, List<settlement_amt_type>>();
                                    }

                                    if (obj_item.order_amount_typesDict.ContainsKey(row[6].ToString()))
                                        li1 = obj_item.order_amount_typesDict[row[6].ToString()];
                                    else
                                    {
                                        li1 = new List<settlement_amt_type>();
                                        obj_item.order_amount_typesDict.Add(row[6].ToString(), li1);
                                    }
                                }
                                if (row[7].ToString() != "" && row[7].ToString() != "0")// new
                                {
                                    li1.Add(subobj_1);
                                }

                                if (row[14].ToString() != "" && row[14].ToString() != "0")
                                    subobj.amount = Math.Round(Convert.ToDouble(row[14].ToString()), 2);
                                else
                                    subobj.amount = 0;
                            }
                            //------------------------------End------------------------------------//
                            //--------------------------add protection fund ----------------------//
                            if (row[13].ToString() != "" && row[13].ToString() != null && row[13].ToString() != "0")
                            {
                                settlement_amt_type subobj_2 = new settlement_amt_type();
                                if (row[13].ToString() != "")
                                    subobj_2.amount = Math.Round(Convert.ToDouble(row[13].ToString()), 2);
                                else
                                    subobj_2.amount = 0;
                                subobj_2.posteddatetime = row[16].ToString();
                                if (row[20].ToString() != "")
                                    subobj_2.qty = Convert.ToInt32(row[20].ToString());

                                subobj_2.description = "ProtectionFund";

                                if (obj_item.refund_amount_typesDict == null)
                                {
                                    obj_item.refund_amount_typesDict = new Dictionary<string, List<settlement_amt_type>>();
                                }

                                List<settlement_amt_type> li2 = null;
                                if (obj_item.refund_amount_typesDict.ContainsKey(row[6].ToString()))
                                    li2 = obj_item.refund_amount_typesDict[row[6].ToString()];
                                else
                                {
                                    li2 = new List<settlement_amt_type>();
                                    obj_item.refund_amount_typesDict.Add(row[6].ToString(), li2);
                                }

                                li2.Add(subobj_2);
                            }

                            //--------------------------------END--------------------------------//


                            if (obj_item.refund_amount_typesDict.ContainsKey(order_item_id))// if (obj_item.refund_amount_typesDict.ContainsKey(sku))
                            {
                                li = obj_item.refund_amount_typesDict[row[6].ToString()];// li = obj_item.refund_amount_typesDict[row[19].ToString()];
                                if (li.Count > 0)
                                {
                                    //if (row[7].ToString() != "" && row[7].ToString() != "0")//new
                                    //{
                                    li.Add(subobj);
                                    //}
                                }
                                //else
                                //    li.Add(subobj);
                            }
                            else
                            {
                                li = new List<settlement_amt_type>();
                                //if (row[7].ToString() != "" && row[7].ToString() != "0")//new
                                //{
                                li.Add(subobj);
                                obj_item.refund_amount_typesDict.Add(order_item_id, li);
                                //}//obj_item.refund_amount_typesDict.Add(sku, li)

                            }
                            if (row[32].ToString() != null && row[32].ToString() != "0")//Commission
                            {
                                settlement_amt_type subobj_1 = new settlement_amt_type();
                                subobj_1.amount = Math.Round(Convert.ToDouble(row[32].ToString()), 2);
                                subobj_1.description = firstrow[32].ToString().Replace("(Rs.)", "").Trim();
                                li.Add(subobj_1);
                            }
                            if (row[33].ToString() != null && row[33].ToString() != "0")//Collection_Fee
                            {
                                settlement_amt_type subobj_2 = new settlement_amt_type();
                                subobj_2.amount = Math.Round(Convert.ToDouble(row[33].ToString()), 2);
                                subobj_2.description = firstrow[33].ToString().Replace("(Rs.)", "").Trim();
                                li.Add(subobj_2);
                            }

                            if (row[34].ToString() != null && row[34].ToString() != "0")//Fixed Fee
                            {
                                settlement_amt_type subobj_3 = new settlement_amt_type();
                                subobj_3.amount = Math.Round(Convert.ToDouble(row[34].ToString()), 2);
                                subobj_3.description = firstrow[34].ToString().Replace("(Rs.)", "").Trim();
                                li.Add(subobj_3);
                            }

                            if (row[35].ToString() != null && row[35].ToString() != "0")//No Cost Emi Fee Reimbursement
                            {
                                settlement_amt_type subobj_4 = new settlement_amt_type();
                                subobj_4.amount = Math.Round(Convert.ToDouble(row[35].ToString()), 2);
                                subobj_4.description = firstrow[35].ToString().Replace("(Rs.)", "").Trim();
                                li.Add(subobj_4);
                            }

                            if (row[36].ToString() != null && row[36].ToString() != "0")//Installation Fee
                            {
                                settlement_amt_type subobj_5 = new settlement_amt_type();
                                subobj_5.amount = Math.Round(Convert.ToDouble(row[36].ToString()), 2);
                                subobj_5.description = firstrow[36].ToString().Replace("(Rs.)", "").Trim();
                                li.Add(subobj_5);
                            }

                            if (row[37].ToString() != null && row[37].ToString() != "0")//Uninstallation Fee
                            {
                                settlement_amt_type subobj_6 = new settlement_amt_type();
                                subobj_6.amount = Math.Round(Convert.ToDouble(row[37].ToString()), 2);
                                subobj_6.description = firstrow[37].ToString().Replace("(Rs.)", "").Trim();
                                li.Add(subobj_6);
                            }

                            if (row[38].ToString() != null && row[38].ToString() != "0")//Tech Visit Fee
                            {
                                settlement_amt_type subobj_7 = new settlement_amt_type();
                                subobj_7.amount = Math.Round(Convert.ToDouble(row[38].ToString()), 2);
                                subobj_7.description = firstrow[38].ToString().Replace("(Rs.)", "").Trim();
                                li.Add(subobj_7);
                            }

                            if (row[39].ToString() != null && row[39].ToString() != "0")//Uninstallation & Packaging Fee
                            {
                                settlement_amt_type subobj_8 = new settlement_amt_type();
                                subobj_8.amount = Math.Round(Convert.ToDouble(row[39].ToString()), 2);
                                subobj_8.description = firstrow[39].ToString().Replace("(Rs.)", "").Trim();
                                li.Add(subobj_8);
                            }

                            if (row[40].ToString() != null && row[40].ToString() != "0")//Pick And Pack Fee
                            {
                                settlement_amt_type subobj_9 = new settlement_amt_type();
                                subobj_9.amount = Math.Round(Convert.ToDouble(row[40].ToString()), 2);
                                subobj_9.description = firstrow[40].ToString().Replace("(Rs.)", "").Trim();
                                li.Add(subobj_9);
                            }

                            if (row[41].ToString() != null && row[41].ToString() != "0" && row[41].ToString() != "")//Customer Shipping Fee Type
                            {
                                settlement_amt_type subobj_10 = new settlement_amt_type();
                                string testing = row[41].ToString();
                                if (row[41].ToString() != null && row[41].ToString() != "")
                                {
                                    subobj_10.amount = Math.Round(Convert.ToDouble(row[41].ToString()), 2);
                                }
                                else
                                {
                                    subobj_10.amount = 0;
                                }
                                subobj_10.description = firstrow[41].ToString().Replace("(Rs.)", "").Trim();
                                li.Add(subobj_10);
                            }

                            if (row[42].ToString() != null && row[42].ToString() != "0")//Customer Shipping Fee
                            {
                                settlement_amt_type subobj_11 = new settlement_amt_type();
                                subobj_11.amount = Math.Round(Convert.ToDouble(row[42].ToString()), 2);
                                subobj_11.description = firstrow[42].ToString().Replace("(Rs.)", "").Trim();
                                li.Add(subobj_11);
                            }

                            if (row[43].ToString() != null && row[43].ToString() != "0")//Shipping Fee
                            {
                                settlement_amt_type subobj_12 = new settlement_amt_type();
                                subobj_12.amount = Math.Round(Convert.ToDouble(row[43].ToString()), 2);
                                subobj_12.description = firstrow[43].ToString().Replace("(Rs.)", "").Trim();
                                li.Add(subobj_12);
                            }

                            if (row[44].ToString() != null && row[44].ToString() != "0")//Reverse Shipping Fee
                            {
                                settlement_amt_type subobj_13 = new settlement_amt_type();
                                subobj_13.amount = Math.Round(Convert.ToDouble(row[44].ToString()), 2);
                                subobj_13.description = firstrow[44].ToString().Replace("(Rs.)", "").Trim();
                                li.Add(subobj_13);
                            }

                            if (row[45].ToString() != null && row[45].ToString() != "0")//Franchise Fee
                            {
                                settlement_amt_type subobj_14 = new settlement_amt_type();
                                subobj_14.amount = Math.Round(Convert.ToDouble(row[45].ToString()), 2);
                                subobj_14.description = firstrow[45].ToString().Replace("(Rs.)", "").Trim();
                                li.Add(subobj_14);
                            }

                            if (row[46].ToString() != null && row[46].ToString() != "0")//Product Cancellation Fee
                            {
                                settlement_amt_type subobj_15 = new settlement_amt_type();
                                subobj_15.amount = Math.Round(Convert.ToDouble(row[46].ToString()), 2);
                                subobj_15.description = firstrow[46].ToString().Replace("(Rs.)", "").Trim();
                                li.Add(subobj_15);
                            }

                            if (row[47].ToString() != null && row[47].ToString() != "0")//Service Cancellation Fee
                            {
                                settlement_amt_type subobj_16 = new settlement_amt_type();
                                subobj_16.amount = Math.Round(Convert.ToDouble(row[47].ToString()), 2);
                                subobj_16.description = firstrow[47].ToString().Replace("(Rs.)", "").Trim();
                                li.Add(subobj_16);
                            }

                            if (row[48].ToString() != null && row[48].ToString() != "0")//Fee Discount
                            {
                                settlement_amt_type subobj_17 = new settlement_amt_type();
                                subobj_17.amount = Math.Round(Convert.ToDouble(row[48].ToString()), 2);
                                subobj_17.description = firstrow[48].ToString().Replace("(Rs.)", "").Trim();
                                li.Add(subobj_17);
                            }
                        }
                    }
                    else
                    {
                        obj_item = new reconciliationorder();

                        sett_amount += Convert.ToDouble(row[3]);

                        obj_item.settlement_id = row[0].ToString();
                        obj_item.settlement_start_date = row[2].ToString();
                        obj_item.settlement_amount = row[3].ToString();
                        obj_item.order_id = row[5].ToString();
                        obj_item.order_itemId = row[6].ToString();
                        obj_item.amount = row[7].ToString();
                        obj_item.Total_Offer_Amount = row[8].ToString();
                        obj_item.MY_Share_Amount = row[9].ToString();
                        obj_item.Customershipping_Amount = row[10].ToString();
                        obj_item.marketplace_fee = row[11].ToString();
                        obj_item.taxes = row[12].ToString();
                        obj_item.Protection_fund = row[13].ToString();
                        obj_item.Refund_Amount = row[14].ToString();
                        obj_item.sku = row[19].ToString();
                        obj_item.quantity_purchased = row[20].ToString();
                        obj_item.posted_date = row[16].ToString();
                        obj_item.posted_date_time = row[17].ToString();
                        obj_item.transaction_type = row[23].ToString();
                        settlement_amt_type subobj = new settlement_amt_type();
                        if (row[7].ToString() != "")
                            subobj.amount = Math.Round(Convert.ToDouble(row[7].ToString()), 2);
                        else
                            subobj.amount = 0;
                        subobj.posteddatetime = row[16].ToString();
                        if (row[20].ToString() != "")
                            subobj.qty = Convert.ToInt32(row[20].ToString());

                        subobj.description = "TotalSaleAmount";

                        if (row[23].ToString() == "NA")
                        {
                            if (row[5].ToString() == "OD111102582989434000")
                            {

                            }
                            //------------------------add data in order dictionary------------------//
                            if (row[14].ToString() != "" && row[14].ToString() != null && row[14].ToString() != "0")
                            {
                                List<settlement_amt_type> li1 = null;
                                settlement_amt_type subobj_1 = new settlement_amt_type();
                                if (row[14].ToString() != "")
                                    subobj_1.amount = Math.Round(Convert.ToDouble(row[14].ToString()), 2);
                                else
                                    subobj_1.amount = 0;
                                subobj_1.posteddatetime = row[16].ToString();
                                if (row[20].ToString() != "")
                                    subobj_1.qty = Convert.ToInt32(row[20].ToString());

                                subobj_1.description = "TotalSaleAmount";
                                if (row[14].ToString() != "0")
                                {
                                    if (obj_item.refund_amount_typesDict == null)
                                    {
                                        obj_item.refund_amount_typesDict = new Dictionary<string, List<settlement_amt_type>>();
                                    }

                                    if (obj_item.refund_amount_typesDict.ContainsKey(row[6].ToString()))
                                        li1 = obj_item.refund_amount_typesDict[row[6].ToString()];
                                    else
                                    {
                                        li1 = new List<settlement_amt_type>();
                                        obj_item.refund_amount_typesDict.Add(row[6].ToString(), li1);
                                    }
                                }

                                li1.Add(subobj_1);

                                //if (row[14].ToString() != "" && row[14].ToString() != "0")
                                //    subobj.amount = Math.Round(Convert.ToDouble(row[14].ToString()), 2);
                                //else
                                //    subobj.amount = 0;
                            }
                            //------------------------------End------------------------------------//

                            //------------------------add protection fund ----------------------//
                            if (row[13].ToString() != "" && row[13].ToString() != null && row[13].ToString() != "0")
                            {
                                settlement_amt_type subobj_3 = new settlement_amt_type();
                                if (row[13].ToString() != "")
                                    subobj_3.amount = Math.Round(Convert.ToDouble(row[13].ToString()), 2);
                                else
                                    subobj_3.amount = 0;
                                subobj_3.posteddatetime = row[16].ToString();
                                if (row[20].ToString() != "")
                                    subobj_3.qty = Convert.ToInt32(row[20].ToString());

                                subobj_3.description = "ProtectionFund";
                                obj_item.order_amount_typesDict = new Dictionary<string, List<settlement_amt_type>>();
                                List<settlement_amt_type> li4 = new List<settlement_amt_type>();
                                li4.Add(subobj);
                            }

                            //--------------------------------END--------------------------------//
                            List<settlement_amt_type> li;
                            if (obj_item.order_amount_typesDict == null)
                            {
                                obj_item.order_amount_typesDict = new Dictionary<string, List<settlement_amt_type>>();
                            }
                            if (obj_item.order_amount_typesDict.ContainsKey(row[6].ToString()))
                                li = obj_item.order_amount_typesDict[row[6].ToString()];
                            else
                            {
                                li = new List<settlement_amt_type>();
                                obj_item.order_amount_typesDict.Add(row[6].ToString(), li);
                            }
                            li.Add(subobj);

                            // obj_item.order_amount_typesDict = new Dictionary<string, List<settlement_amt_type>>();
                            // List<settlement_amt_type> li = new List<settlement_amt_type>();
                            // li.Add(subobj);


                            if (row[32].ToString() != null && row[32].ToString() != "0")//commission
                            {
                                settlement_amt_type subobj1 = new settlement_amt_type();
                                subobj1.amount = Math.Round(Convert.ToDouble(row[32].ToString()), 2);
                                subobj1.description = firstrow[32].ToString().Replace("(Rs.)", "").Trim();
                                li.Add(subobj1);
                            }

                            if (row[33].ToString() != null && row[33].ToString() != "0")//collection fee
                            {
                                settlement_amt_type subobj2 = new settlement_amt_type();
                                subobj2.amount = Math.Round(Convert.ToDouble(row[33].ToString()), 2);
                                subobj2.description = firstrow[33].ToString().Replace("(Rs.)", "").Trim();
                                li.Add(subobj2);
                            }

                            if (row[34].ToString() != null && row[34].ToString() != "0")//fixed fee
                            {
                                settlement_amt_type subobj3 = new settlement_amt_type();
                                subobj3.amount = Math.Round(Convert.ToDouble(row[34].ToString()), 2);
                                subobj3.description = firstrow[34].ToString().Replace("(Rs.)", "").Trim();
                                li.Add(subobj3);
                            }

                            if (row[35].ToString() != null && row[35].ToString() != "0")//no cost emi reimbursement
                            {
                                settlement_amt_type subobj4 = new settlement_amt_type();
                                subobj4.amount = Math.Round(Convert.ToDouble(row[35].ToString()), 2);
                                subobj4.description = firstrow[35].ToString().Replace("(Rs.)", "").Trim();
                                li.Add(subobj4);
                            }

                            if (row[36].ToString() != null && row[36].ToString() != "0")// installation fee
                            {
                                settlement_amt_type subobj5 = new settlement_amt_type();
                                subobj5.amount = Math.Round(Convert.ToDouble(row[36].ToString()), 2);
                                subobj5.description = firstrow[36].ToString().Replace("(Rs.)", "").Trim();
                                li.Add(subobj5);
                            }

                            if (row[37].ToString() != null && row[37].ToString() != "0")//uninstallation fee
                            {
                                settlement_amt_type subobj6 = new settlement_amt_type();
                                subobj6.amount = Math.Round(Convert.ToDouble(row[37].ToString()), 2);
                                subobj6.description = firstrow[37].ToString().Replace("(Rs.)", "").Trim();
                                li.Add(subobj6);
                            }

                            if (row[38].ToString() != null && row[38].ToString() != "0")//tech visit fee
                            {
                                settlement_amt_type subobj7 = new settlement_amt_type();
                                subobj7.amount = Math.Round(Convert.ToDouble(row[38].ToString()), 2);
                                subobj7.description = firstrow[38].ToString().Replace("(Rs.)", "").Trim();
                                li.Add(subobj7);
                            }

                            if (row[39].ToString() != null && row[39].ToString() != "0")//uninstallation package fee
                            {
                                settlement_amt_type subobj8 = new settlement_amt_type();
                                subobj8.amount = Math.Round(Convert.ToDouble(row[39].ToString()), 2);
                                subobj8.description = firstrow[39].ToString().Replace("(Rs.)", "").Trim();
                                li.Add(subobj8);
                            }

                            if (row[40].ToString() != null && row[40].ToString() != "0")//pick and pack fee
                            {
                                settlement_amt_type subobj9 = new settlement_amt_type();
                                subobj9.amount = Math.Round(Convert.ToDouble(row[40].ToString()), 2);
                                subobj9.description = firstrow[40].ToString().Replace("(Rs.)", "").Trim();
                                li.Add(subobj9);
                            }

                            if (row[41].ToString() != null && row[41].ToString() != "0" && row[41].ToString() != "")//customer shipping fee type
                            {
                                settlement_amt_type subobj10 = new settlement_amt_type();
                                string testing = row[41].ToString();
                                if (row[41].ToString() != null && row[41].ToString() != "")
                                {
                                    subobj10.amount = Math.Round(Convert.ToDouble(row[41].ToString()), 2);
                                }
                                else
                                {
                                    subobj10.amount = 0;
                                }
                                subobj10.description = firstrow[41].ToString().Replace("(Rs.)", "").Trim();
                                li.Add(subobj10);
                            }

                            if (row[42].ToString() != null && row[42].ToString() != "0")//customer shipping fee
                            {
                                settlement_amt_type subobj11 = new settlement_amt_type();
                                subobj11.amount = Math.Round(Convert.ToDouble(row[42].ToString()), 2);
                                subobj11.description = firstrow[42].ToString().Replace("(Rs.)", "").Trim();
                                li.Add(subobj11);
                            }

                            if (row[43].ToString() != null && row[43].ToString() != "0")//shipping fee
                            {
                                settlement_amt_type subobj12 = new settlement_amt_type();
                                subobj12.amount = Math.Round(Convert.ToDouble(row[43].ToString()), 2);
                                subobj12.description = firstrow[43].ToString().Replace("(Rs.)", "").Trim();
                                li.Add(subobj12);
                            }

                            if (row[44].ToString() != null && row[44].ToString() != "0")//reverse shipping fee
                            {
                                settlement_amt_type subobj13 = new settlement_amt_type();
                                subobj13.amount = Math.Round(Convert.ToDouble(row[44].ToString()), 2);
                                subobj13.description = firstrow[44].ToString().Replace("(Rs.)", "").Trim();
                                li.Add(subobj13);
                            }

                            if (row[45].ToString() != null && row[45].ToString() != "0")// franchise fee
                            {
                                settlement_amt_type subobj14 = new settlement_amt_type();
                                subobj14.amount = Math.Round(Convert.ToDouble(row[45].ToString()), 2);
                                subobj14.description = firstrow[45].ToString().Replace("(Rs.)", "").Trim();
                                li.Add(subobj14);
                            }

                            if (row[46].ToString() != null && row[46].ToString() != "0")//product cancellation fee
                            {
                                settlement_amt_type subobj15 = new settlement_amt_type();
                                subobj15.amount = Math.Round(Convert.ToDouble(row[46].ToString()), 2);
                                subobj15.description = firstrow[46].ToString().Replace("(Rs.)", "").Trim();
                                li.Add(subobj15);
                            }

                            if (row[47].ToString() != null && row[47].ToString() != "0")//service cancellation fee
                            {
                                settlement_amt_type subobj16 = new settlement_amt_type();
                                subobj16.amount = Math.Round(Convert.ToDouble(row[47].ToString()), 2);
                                subobj16.description = firstrow[47].ToString().Replace("(Rs.)", "").Trim();
                                li.Add(subobj16);
                            }

                            if (row[48].ToString() != null && row[48].ToString() != "0")// fee discount
                            {
                                settlement_amt_type subobj17 = new settlement_amt_type();
                                subobj17.amount = Math.Round(Convert.ToDouble(row[48].ToString()), 2);
                                subobj17.description = firstrow[48].ToString().Replace("(Rs.)", "").Trim();
                                li.Add(subobj17);
                            }

                            //obj_item.order_amount_typesDict.Add(row[19].ToString(), li);//for sku
                            //obj_item.order_amount_typesDict.Add(row[6].ToString(), li);
                        }
                        else if (row[23].ToString() == "Courier Return" || row[23].ToString() == "Customer Return")
                        {
                            if (row[5].ToString() == "OD111102582989434000")
                            {

                            }
                            //if (row[13].ToString() != "" && row[13].ToString() != null && row[13].ToString() != "0")


                            //------------------------add data in order dictionary------------------//
                            if (row[14].ToString() != "" && row[14].ToString() != null)
                            {
                                List<settlement_amt_type> li1 = null;
                                settlement_amt_type subobj_1 = new settlement_amt_type();
                                if (row[7].ToString() != "")
                                    subobj_1.amount = Math.Round(Convert.ToDouble(row[7].ToString()), 2);
                                else
                                    subobj_1.amount = 0;
                                subobj_1.posteddatetime = row[16].ToString();
                                if (row[20].ToString() != "")
                                    subobj_1.qty = Convert.ToInt32(row[20].ToString());

                                subobj_1.description = "TotalSaleAmount";
                                if (row[14].ToString() != "0")
                                {

                                    if (obj_item.order_amount_typesDict == null)
                                    {
                                        obj_item.order_amount_typesDict = new Dictionary<string, List<settlement_amt_type>>();
                                    }

                                    if (obj_item.order_amount_typesDict.ContainsKey(row[6].ToString()))
                                        li1 = obj_item.order_amount_typesDict[row[6].ToString()];
                                    else
                                    {
                                        li1 = new List<settlement_amt_type>();
                                        obj_item.order_amount_typesDict.Add(row[6].ToString(), li1);
                                    }

                                }
                                else if (row[14].ToString() == "0")
                                {
                                    if (obj_item.order_amount_typesDict == null)
                                    {
                                        obj_item.order_amount_typesDict = new Dictionary<string, List<settlement_amt_type>>();
                                    }

                                    if (obj_item.order_amount_typesDict.ContainsKey(row[6].ToString()))
                                        li1 = obj_item.order_amount_typesDict[row[6].ToString()];
                                    else
                                    {
                                        li1 = new List<settlement_amt_type>();
                                        obj_item.order_amount_typesDict.Add(row[6].ToString(), li1);
                                    }

                                }
                                //if (row[7].ToString() != "" && row[7].ToString() != "0")//new
                                //{
                                li1.Add(subobj_1);
                                //}

                                if (row[14].ToString() != "" && row[14].ToString() != "0")
                                    subobj.amount = Math.Round(Convert.ToDouble(row[14].ToString()), 2);
                                else
                                    subobj.amount = 0;
                            }
                            //------------------------------End------------------------------------//

                            //--------------------------add protection fund ----------------------//
                            if (row[13].ToString() != "" && row[13].ToString() != null && row[13].ToString() != "0")
                            {
                                settlement_amt_type subobj_2 = new settlement_amt_type();
                                if (row[13].ToString() != "")
                                    subobj_2.amount = Math.Round(Convert.ToDouble(row[13].ToString()), 2);
                                else
                                    subobj_2.amount = 0;
                                subobj_2.posteddatetime = row[16].ToString();
                                if (row[20].ToString() != "")
                                    subobj_2.qty = Convert.ToInt32(row[20].ToString());

                                subobj_2.description = "ProtectionFund";

                                if (obj_item.refund_amount_typesDict == null)
                                {
                                    obj_item.refund_amount_typesDict = new Dictionary<string, List<settlement_amt_type>>();
                                }

                                List<settlement_amt_type> li2 = null;
                                if (obj_item.refund_amount_typesDict.ContainsKey(row[6].ToString()))
                                    li2 = obj_item.refund_amount_typesDict[row[6].ToString()];
                                else
                                {
                                    li2 = new List<settlement_amt_type>();
                                    obj_item.refund_amount_typesDict.Add(row[6].ToString(), li2);
                                }

                                li2.Add(subobj_2);
                            }

                            //--------------------------------END--------------------------------//
                            List<settlement_amt_type> li;
                            //if (row[23].ToString() == "Courier Return")
                            //{
                            //    if (obj_item.order_amount_typesDict == null)
                            //    {
                            //        obj_item.order_amount_typesDict = new Dictionary<string, List<settlement_amt_type>>();
                            //    }

                            //    if (obj_item.order_amount_typesDict.ContainsKey(row[5].ToString()))
                            //        li = obj_item.order_amount_typesDict[row[5].ToString()];
                            //    else
                            //    {
                            //        li = new List<settlement_amt_type>();
                            //        obj_item.order_amount_typesDict.Add(row[5].ToString(), li);
                            //    }
                            //    li.Add(subobj);
                            //}
                            //else
                            //{
                            if (obj_item.refund_amount_typesDict == null)
                            {
                                obj_item.refund_amount_typesDict = new Dictionary<string, List<settlement_amt_type>>();
                            }
                            if (obj_item.refund_amount_typesDict.ContainsKey(row[6].ToString()))
                                li = obj_item.refund_amount_typesDict[row[6].ToString()];
                            else
                            {
                                li = new List<settlement_amt_type>();
                                obj_item.refund_amount_typesDict.Add(row[6].ToString(), li);
                            }
                            li.Add(subobj);
                            //}

                            if (row[32].ToString() != null && row[32].ToString() != "0")//commission
                            {
                                settlement_amt_type subobj_1 = new settlement_amt_type();
                                subobj_1.amount = Math.Round(Convert.ToDouble(row[32].ToString()), 2);
                                subobj_1.description = firstrow[32].ToString().Replace("(Rs.)", "").Trim();
                                li.Add(subobj_1);
                            }

                            if (row[33].ToString() != null && row[33].ToString() != "0")//collection fee
                            {
                                settlement_amt_type subobj_2 = new settlement_amt_type();
                                subobj_2.amount = Math.Round(Convert.ToDouble(row[33].ToString()), 2);
                                subobj_2.description = firstrow[33].ToString().Replace("(Rs.)", "").Trim();
                                li.Add(subobj_2);
                            }

                            if (row[34].ToString() != null && row[34].ToString() != "0")//fixed fee
                            {
                                settlement_amt_type subobj_3 = new settlement_amt_type();
                                subobj_3.amount = Math.Round(Convert.ToDouble(row[34].ToString()), 2);
                                subobj_3.description = firstrow[34].ToString().Replace("(Rs.)", "").Trim();
                                li.Add(subobj_3);
                            }

                            if (row[35].ToString() != null && row[35].ToString() != "0")//no cost emi reimbursement
                            {
                                settlement_amt_type subobj_4 = new settlement_amt_type();
                                subobj_4.amount = Math.Round(Convert.ToDouble(row[35].ToString()), 2);
                                subobj_4.description = firstrow[35].ToString().Replace("(Rs.)", "").Trim();
                                li.Add(subobj_4);
                            }

                            if (row[36].ToString() != null && row[36].ToString() != "0")// installation fee
                            {
                                settlement_amt_type subobj_5 = new settlement_amt_type();
                                subobj_5.amount = Math.Round(Convert.ToDouble(row[36].ToString()), 2);
                                subobj_5.description = firstrow[36].ToString().Replace("(Rs.)", "").Trim();
                                li.Add(subobj_5);
                            }

                            if (row[37].ToString() != null && row[37].ToString() != "0")//uninstallation fee
                            {
                                settlement_amt_type subobj_6 = new settlement_amt_type();
                                subobj_6.amount = Math.Round(Convert.ToDouble(row[37].ToString()), 2);
                                subobj_6.description = firstrow[37].ToString().Replace("(Rs.)", "").Trim();
                                li.Add(subobj_6);
                            }

                            if (row[38].ToString() != null && row[38].ToString() != "0")//tech visit fee
                            {
                                settlement_amt_type subobj_7 = new settlement_amt_type();
                                subobj_7.amount = Math.Round(Convert.ToDouble(row[38].ToString()), 2);
                                subobj_7.description = firstrow[38].ToString().Replace("(Rs.)", "").Trim();
                                li.Add(subobj_7);
                            }

                            if (row[39].ToString() != null && row[39].ToString() != "0")//uninstallation package fee
                            {
                                settlement_amt_type subobj_8 = new settlement_amt_type();
                                subobj_8.amount = Math.Round(Convert.ToDouble(row[39].ToString()), 2);
                                subobj_8.description = firstrow[39].ToString().Replace("(Rs.)", "").Trim();
                                li.Add(subobj_8);
                            }

                            if (row[40].ToString() != null && row[40].ToString() != "0")//pick and pack fee
                            {
                                settlement_amt_type subobj_9 = new settlement_amt_type();
                                subobj_9.amount = Math.Round(Convert.ToDouble(row[40].ToString()), 2);
                                subobj_9.description = firstrow[40].ToString().Replace("(Rs.)", "").Trim();
                                li.Add(subobj_9);
                            }

                            if (row[41].ToString() != null && row[41].ToString() != "0" && row[41].ToString() != "")//customer shipping fee type
                            {
                                settlement_amt_type subobj_10 = new settlement_amt_type();
                                subobj_10.description = firstrow[41].ToString().Replace("(Rs.)", "").Trim();
                                string testing = row[41].ToString();
                                if (row[41].ToString() != null && row[41].ToString() != "")
                                {
                                    subobj_10.amount = Math.Round(Convert.ToDouble(row[41].ToString()), 2);
                                }
                                else
                                {
                                    subobj_10.amount = 0;
                                }

                                li.Add(subobj_10);
                            }

                            if (row[42].ToString() != null && row[42].ToString() != "0")//customer shipping fee
                            {
                                settlement_amt_type subobj_11 = new settlement_amt_type();
                                subobj_11.amount = Math.Round(Convert.ToDouble(row[42].ToString()), 2);
                                subobj_11.description = firstrow[42].ToString().Replace("(Rs.)", "").Trim();
                                li.Add(subobj_11);
                            }

                            if (row[43].ToString() != null && row[43].ToString() != "0")//shipping fee
                            {
                                settlement_amt_type subobj_12 = new settlement_amt_type();
                                subobj_12.amount = Math.Round(Convert.ToDouble(row[43].ToString()), 2);
                                subobj_12.description = firstrow[43].ToString().Replace("(Rs.)", "").Trim();
                                li.Add(subobj_12);
                            }

                            if (row[44].ToString() != null && row[44].ToString() != "0")//reverse shipping fee
                            {
                                settlement_amt_type subobj_13 = new settlement_amt_type();
                                subobj_13.amount = Math.Round(Convert.ToDouble(row[44].ToString()), 2);
                                subobj_13.description = firstrow[44].ToString().Replace("(Rs.)", "").Trim();
                                li.Add(subobj_13);
                            }

                            if (row[45].ToString() != null && row[45].ToString() != "0")// franchise fee
                            {
                                settlement_amt_type subobj_14 = new settlement_amt_type();
                                subobj_14.amount = Math.Round(Convert.ToDouble(row[45].ToString()), 2);
                                subobj_14.description = firstrow[45].ToString().Replace("(Rs.)", "").Trim();
                                li.Add(subobj_14);
                            }

                            if (row[46].ToString() != null && row[46].ToString() != "0")//product cancellation fee
                            {
                                settlement_amt_type subobj_15 = new settlement_amt_type();
                                subobj_15.amount = Math.Round(Convert.ToDouble(row[46].ToString()), 2);
                                subobj_15.description = firstrow[46].ToString().Replace("(Rs.)", "").Trim();
                                li.Add(subobj_15);
                            }

                            if (row[47].ToString() != null && row[47].ToString() != "0")//service cancellation fee
                            {
                                settlement_amt_type subobj_16 = new settlement_amt_type();
                                subobj_16.amount = Math.Round(Convert.ToDouble(row[47].ToString()), 2);
                                subobj_16.description = firstrow[47].ToString().Replace("(Rs.)", "").Trim();
                                li.Add(subobj_16);
                            }

                            if (row[48].ToString() != null && row[48].ToString() != "0")// fee discount
                            {
                                settlement_amt_type subobj_17 = new settlement_amt_type();
                                subobj_17.amount = Math.Round(Convert.ToDouble(row[48].ToString()), 2);
                                subobj_17.description = firstrow[48].ToString().Replace("(Rs.)", "").Trim();
                                li.Add(subobj_17);
                            }
                            // obj_item.refund_amount_typesDict.Add(row[5].ToString(), li);
                            //obj_item.refund_amount_typesDict.Add(row[19].ToString(), li);//for sku
                        }
                        objreconciliationorder.Add(obj_item);

                        //dictionary.Add(obj_item.sku, obj_item);
                        dictionary.Add(obj_item.order_id, obj_item);
                    }
                }// end of foreach
                #endregion

                sheetName = "Tax Details";

                System.Linq.IQueryable<LinqToExcel.Row> taxrows = from a in excel.Worksheet(sheetName) select a;
                ReadTax_Details(taxrows, ref dictionary);

                objjson1.Add(new AmazonreconciliationOrder
                {
                    reconciliationorder = objreconciliationorder,
                });

                if (objjson1 != null)
                {
                    foreach (var sitem in objjson1[0].reconciliationorder)
                    {
                        if (sitem.deposit_date != null)
                        {
                            sitem.total_amount = sett_amount.ToString();

                            break;
                        }
                    }
                }
                CronJobController objCron = new CronJobController();
                objCron.SaveBulksettlementdata(objjson1, id, marketplaceid, seller_id, null);
            }
            catch (Exception ex)
            {
            }
            return objjson1;
        }

        public Dictionary<string, reconciliationorder> ReadTax_Details(System.Linq.IQueryable<LinqToExcel.Row> rows, ref Dictionary<string, reconciliationorder> dictionary)// used for read tax details from excel sheet
        {
            string success = "";
            try
            {
                int i = 0;
                LinqToExcel.Row firstrow = null;
                foreach (var row in rows)
                {
                    i++;
                    if (i == 1)
                    {
                        firstrow = row;
                        continue;
                    }
                    reconciliationorder obj_item = null;
                    reconciliationorder found_obj_item = null;
                    obj_item = new reconciliationorder();
                    List<reconciliationorder> li = new List<reconciliationorder>();

                    string order_detai_idd = row[2].ToString();
                    string settlement_name = row[5].ToString();
                    string neft_id = row[1].ToString();
                    if (row[2].ToString() == "11099501708486300")
                    {
                    }
                    bool found = false;
                    foreach (KeyValuePair<string, reconciliationorder> pair in dictionary)
                    {
                        found_obj_item = pair.Value;

                        if (found_obj_item.settlement_id == neft_id)
                        {
                            int sname = 0;
                            try
                            {
                                if (found_obj_item.order_amount_typesDict.Values.ToList().ElementAt(0).Count > 0)
                                    for (int j = 0; j <= found_obj_item.order_amount_typesDict.Values.ToList().ElementAt(0).Count; j++)
                                    {
                                        if (settlement_name == found_obj_item.order_amount_typesDict.Values.ToList().ElementAt(0).ElementAt(j).description)
                                        {
                                            sname = j;
                                            break;
                                        }// End of main if
                                    }
                            }
                            catch (Exception ex)
                            {
                            }


                            if (found_obj_item.order_amount_typesDict != null && found_obj_item.order_amount_typesDict.ContainsKey(order_detai_idd))
                            {
                                if (found_obj_item.order_amount_typesDict[order_detai_idd] != null)
                                {
                                    if (found_obj_item.order_amount_typesDict.Values.ToList().ElementAt(0).ElementAt(sname).description.Contains(settlement_name))
                                    {
                                        List<settlement_amt_type> li1 = found_obj_item.order_amount_typesDict[order_detai_idd];
                                        settlement_amt_type subobj = new settlement_amt_type();
                                        if (row[13].ToString() != "" && row[13].ToString() != "0")
                                        {
                                            subobj.description = row[5].ToString() + " " + "IGST";
                                            subobj.amount = Convert.ToDouble(row[13].ToString());
                                            li1.Add(subobj);
                                        }
                                        else if (row[11].ToString() != "" && row[11].ToString() != "0")
                                        {
                                            subobj.description = row[5].ToString() + " " + "CGST";
                                            subobj.amount = Convert.ToDouble(row[11].ToString());
                                            li1.Add(subobj);

                                            settlement_amt_type subobj1 = new settlement_amt_type();
                                            subobj1.description = row[5].ToString() + " " + "SGST";
                                            subobj1.amount = Convert.ToDouble(row[12].ToString());
                                            li1.Add(subobj1);
                                        }
                                        break;
                                    }
                                }
                                //}
                            }
                        }// end of if compare neft_id

                        if (found_obj_item.settlement_id == neft_id)
                        {
                            int name = 0;
                            try
                            {
                                if (found_obj_item.refund_amount_typesDict.Values.ToList().ElementAt(0).Count > 0)
                                    for (int j = 0; j <= found_obj_item.refund_amount_typesDict.Values.ToList().ElementAt(0).Count; j++)
                                    {
                                        if (settlement_name == found_obj_item.refund_amount_typesDict.Values.ToList().ElementAt(0).ElementAt(j).description)
                                        {
                                            name = j;
                                            break;
                                        }// End of main if
                                    }
                            }
                            catch (Exception ex)
                            {
                            }

                            if (found_obj_item.refund_amount_typesDict != null && found_obj_item.refund_amount_typesDict.ContainsKey(order_detai_idd))
                            {
                                if (found_obj_item.refund_amount_typesDict.Values.ToList().ElementAt(0).ElementAt(name).description.Contains(settlement_name))
                                {
                                    List<settlement_amt_type> li2 = found_obj_item.refund_amount_typesDict[order_detai_idd];
                                    settlement_amt_type subobj = new settlement_amt_type();
                                    if (row[13].ToString() != "" && row[13].ToString() != "0")
                                    {
                                        subobj.description = row[5].ToString() + " " + "IGST";
                                        subobj.amount = Convert.ToDouble(row[13].ToString());
                                        li2.Add(subobj);
                                    }
                                    else if (row[11].ToString() != "" && row[11].ToString() != "0")
                                    {
                                        subobj.description = row[5].ToString() + " " + "CGST";
                                        subobj.amount = Convert.ToDouble(row[11].ToString());
                                        li2.Add(subobj);

                                        settlement_amt_type subobj1 = new settlement_amt_type();
                                        subobj1.description = row[5].ToString() + " " + "SGST";
                                        subobj1.amount = Convert.ToDouble(row[12].ToString());
                                        li2.Add(subobj1);
                                    }
                                    break;
                                }
                            }
                        }// end of if compare neft_id
                        //}
                    }//end for

                    if (found)
                    {
                        if (found_obj_item.order_amount_typesDict != null)
                        {

                        }//end if
                        else if (found_obj_item.refund_amount_typesDict != null)
                        {

                        }
                    }
                    else
                    {
                        //not found - its a problem
                        int a = 0;
                        a++;
                    }
                }// end of foreach
            }
            catch (Exception ex)
            {
                success = "Ex";
            }
            return dictionary;
        }

        #endregion

        public List<AmazonreconciliationOrder> ReadSettlementFile_Flipkart1(string strFilePath, int seller_id)
        {
            List<AmazonreconciliationOrder> objjson1 = new List<AmazonreconciliationOrder>();
            List<reconciliationorder> objreconciliationorder = new List<reconciliationorder>();
            try
            {
                var excel = new ExcelQueryFactory(strFilePath)
                {
                    DatabaseEngine = LinqToExcel.Domain.DatabaseEngine.Ace,
                    TrimSpaces = LinqToExcel.Query.TrimSpacesType.Both,
                    UsePersistentConnection = true,
                    ReadOnly = true
                };

                var worksheetNames = excel.GetWorksheetNames();
                string sheetName = worksheetNames.First();

                sheetName = "Orders";

                var artistAlbums = from a in excel.Worksheet(sheetName) select a;
                Dictionary<string, reconciliationorder> dictionary = new Dictionary<string, reconciliationorder>();
                int i = 0;
                double sett_amount = 0;
                int total = artistAlbums.Count();
                LinqToExcel.Row headerrow = null;
                List<LinqToExcel.Row> lst_Add = new List<LinqToExcel.Row>();
                List<LinqToExcel.Row> lst_prepaid = new List<LinqToExcel.Row>();
                List<LinqToExcel.Row> lst_postpaid = new List<LinqToExcel.Row>();
                if (total >= 0)
                {
                    string lastdate = "";
                    foreach (var item_1 in artistAlbums)
                    {
                        i++;
                        if (i == 1)
                        {
                            headerrow = item_1;
                            continue;
                        }
                        string name = item_1[1].ToString();
                        string date = item_1[2].ToString();

                        if (lastdate == date)
                        {
                            if (name == "Prepaid")
                            {
                                lastdate = date;
                                lst_prepaid.Add(item_1);
                            }
                            else
                            {
                                lastdate = date;
                                lst_postpaid.Add(item_1);
                            }
                        }
                        else
                        {

                            if (name == "Prepaid")
                            {
                                lastdate = date;
                                lst_prepaid.Add(headerrow);
                                lst_prepaid.Add(item_1);
                            }
                            else
                            {
                                lastdate = date;
                                lst_postpaid.Add(headerrow);
                                lst_postpaid.Add(item_1);
                            }
                        }
                        lst_Add.AddRange(lst_postpaid);
                        lst_Add.AddRange(lst_prepaid);
                    }
                }

                LinqToExcel.Row firstrow = null;// = artistAlbums.ElementAtOrDefault(1); // select 1;
                #region Orders
                foreach (var row in artistAlbums)
                {
                    i++;
                    if (i == 1)
                    {
                        firstrow = row;
                        continue;
                    }
                    //string artistInfo = "Artist Name: {0}; Album: {1}";
                    if (i == 2)
                    {
                        reconciliationorder obj_item1 = new reconciliationorder();
                        obj_item1.settlement_id = row[0].ToString();
                        obj_item1.deposit_date = row[2].ToString();
                        obj_item1.settlement_start_date = row[16].ToString();
                        obj_item1.settlement_end_date = row[17].ToString();
                        obj_item1.total_amount = "0";
                        objreconciliationorder.Add(obj_item1);
                    }



                    reconciliationorder obj_item = null;
                    reconciliationorder obj1_item = null;
                    reconciliationorder obj2_item = null;
                    string orderidd = row[5].ToString();
                    if (dictionary.ContainsKey(orderidd))
                    {
                        obj_item = dictionary[orderidd];

                        settlement_amt_type subobj = new settlement_amt_type();
                        //if (row[3].ToString() != "")
                        //    subobj.settlement_amount = Math.Round(Convert.ToDouble(row[3].ToString()), 2);
                        if (row[7].ToString() != "")
                            subobj.amount = Math.Round(Convert.ToDouble(row[7].ToString()), 2);
                        if (row[20].ToString() != "")
                            subobj.qty = Convert.ToInt32(row[20].ToString());

                        subobj.posteddatetime = row[16].ToString();
                        subobj.type = row[23].ToString();
                        subobj.description = "TotalSaleAmount";
                        if (row[23].ToString() == "NA")
                        {
                            string sku = row[19].ToString();
                            string order_item_id = row[6].ToString();
                            if (orderidd == "OD110995017084863000")
                            {

                            }

                            if (obj_item.order_amount_typesDict == null)
                                obj_item.order_amount_typesDict = new Dictionary<string, List<settlement_amt_type>>();


                            //------------------------add protection fund ----------------------//
                            if (row[13].ToString() != "" && row[13].ToString() != null && row[13].ToString() != "0")
                            {
                                settlement_amt_type subobj_3 = new settlement_amt_type();
                                if (row[13].ToString() != "")
                                    subobj_3.amount = Math.Round(Convert.ToDouble(row[13].ToString()), 2);
                                else
                                    subobj_3.amount = 0;
                                subobj_3.posteddatetime = row[16].ToString();
                                if (row[20].ToString() != "")
                                    subobj_3.qty = Convert.ToInt32(row[20].ToString());

                                subobj_3.description = "ProtectionFund";
                                obj_item.order_amount_typesDict = new Dictionary<string, List<settlement_amt_type>>();
                                List<settlement_amt_type> li4 = new List<settlement_amt_type>();
                                li4.Add(subobj);
                            }

                            //--------------------------------END--------------------------------//


                            List<settlement_amt_type> li = null;
                            if (obj_item.order_amount_typesDict.ContainsKey(order_item_id))// if (obj_item.order_amount_typesDict.ContainsKey(sku))
                            {
                                li = obj_item.order_amount_typesDict[row[6].ToString()];//li = obj_item.order_amount_typesDict[row[19].ToString()];
                                if (li.Count > 0)
                                {
                                    if (row[7].ToString() != "" && row[7].ToString() != "0")//new
                                    {
                                        li.Add(subobj);
                                    }
                                }
                                else
                                    li.Add(subobj);
                            }
                            else
                            {
                                li = new List<settlement_amt_type>();
                                if (row[7].ToString() != "" && row[7].ToString() != "0")//new
                                {
                                    li.Add(subobj);
                                    obj_item.order_amount_typesDict.Add(order_item_id, li);
                                }//obj_item.order_amount_typesDict.Add(sku, li);
                            }

                            ///////////////
                            if (row[32].ToString() != null && row[32].ToString() != "0")//Commission
                            {
                                settlement_amt_type subobj1 = new settlement_amt_type();
                                subobj1.amount = Math.Round(Convert.ToDouble(row[32].ToString()), 2);
                                subobj1.description = firstrow[32].ToString().Replace("(Rs.)", "").Trim();
                                li.Add(subobj1);
                            }

                            if (row[33].ToString() != null && row[33].ToString() != "0")//Collection_Fee
                            {
                                settlement_amt_type subobj2 = new settlement_amt_type();
                                subobj2.amount = Math.Round(Convert.ToDouble(row[33].ToString()), 2);
                                subobj2.description = firstrow[33].ToString().Replace("(Rs.)", "").Trim();
                                li.Add(subobj2);
                            }

                            if (row[34].ToString() != null && row[34].ToString() != "0")//Fixed Fee
                            {
                                settlement_amt_type subobj3 = new settlement_amt_type();
                                subobj3.amount = Math.Round(Convert.ToDouble(row[34].ToString()), 2);
                                subobj3.description = firstrow[34].ToString().Replace("(Rs.)", "").Trim();
                                li.Add(subobj3);
                            }

                            if (row[35].ToString() != null && row[35].ToString() != "0")//No Cost Emi Fee Reimbursement
                            {
                                settlement_amt_type subobj4 = new settlement_amt_type();
                                subobj4.amount = Math.Round(Convert.ToDouble(row[35].ToString()), 2);
                                subobj4.description = firstrow[35].ToString().Replace("(Rs.)", "").Trim();
                                li.Add(subobj4);
                            }

                            if (row[36].ToString() != null && row[36].ToString() != "0")//Installation Fee 
                            {
                                settlement_amt_type subobj5 = new settlement_amt_type();
                                subobj5.amount = Math.Round(Convert.ToDouble(row[36].ToString()), 2);
                                subobj5.description = firstrow[36].ToString().Replace("(Rs.)", "").Trim();
                                li.Add(subobj5);
                            }

                            if (row[37].ToString() != null && row[37].ToString() != "0")//Uninstallation Fee
                            {
                                settlement_amt_type subobj6 = new settlement_amt_type();
                                subobj6.amount = Math.Round(Convert.ToDouble(row[37].ToString()), 2);
                                subobj6.description = firstrow[37].ToString().Replace("(Rs.)", "").Trim();
                                li.Add(subobj6);
                            }

                            if (row[38].ToString() != null && row[38].ToString() != "0")//Tech Visit Fee
                            {
                                settlement_amt_type subobj7 = new settlement_amt_type();
                                subobj7.amount = Math.Round(Convert.ToDouble(row[38].ToString()), 2);
                                subobj7.description = firstrow[38].ToString().Replace("(Rs.)", "").Trim();
                                li.Add(subobj7);
                            }

                            if (row[39].ToString() != null && row[39].ToString() != "0")//Uninstallation & Packaging Fee
                            {
                                settlement_amt_type subobj8 = new settlement_amt_type();
                                subobj8.amount = Math.Round(Convert.ToDouble(row[39].ToString()), 2);
                                subobj8.description = firstrow[39].ToString().Replace("(Rs.)", "").Trim();
                                li.Add(subobj8);
                            }

                            if (row[40].ToString() != null && row[40].ToString() != "0")//Pick And Pack Fee
                            {
                                settlement_amt_type subobj9 = new settlement_amt_type();
                                subobj9.amount = Math.Round(Convert.ToDouble(row[40].ToString()), 2);
                                subobj9.description = firstrow[40].ToString().Replace("(Rs.)", "").Trim();
                                li.Add(subobj9);
                            }

                            if (row[41].ToString() != null && row[41].ToString() != "0" && row[41].ToString() != "")//Customer Shipping Fee Type
                            {
                                settlement_amt_type subobj10 = new settlement_amt_type();
                                if (row[41].ToString() != null && row[41].ToString() != "")
                                {
                                    subobj10.amount = Math.Round(Convert.ToDouble(row[41].ToString()), 2);
                                }
                                else
                                {
                                    subobj10.amount = 0;
                                }
                                subobj10.description = firstrow[41].ToString().Replace("(Rs.)", "").Trim();
                                li.Add(subobj10);
                            }
                            if (row[42].ToString() != null && row[42].ToString() != "0")//Customer Shipping Fee
                            {
                                settlement_amt_type subobj11 = new settlement_amt_type();
                                subobj11.amount = Math.Round(Convert.ToDouble(row[42].ToString()), 2);
                                subobj11.description = firstrow[42].ToString().Replace("(Rs.)", "").Trim();
                                li.Add(subobj11);
                            }
                            if (row[43].ToString() != null && row[43].ToString() != "0")//Shipping Fee
                            {
                                settlement_amt_type subobj12 = new settlement_amt_type();
                                subobj12.amount = Math.Round(Convert.ToDouble(row[43].ToString()), 2);
                                subobj12.description = firstrow[43].ToString().Replace("(Rs.)", "").Trim();
                                li.Add(subobj12);
                            }

                            if (row[44].ToString() != null && row[44].ToString() != "0")//Reverse Shipping Fee 
                            {
                                settlement_amt_type subobj13 = new settlement_amt_type();
                                subobj13.amount = Math.Round(Convert.ToDouble(row[44].ToString()), 2);
                                subobj13.description = firstrow[44].ToString().Replace("(Rs.)", "").Trim();
                                li.Add(subobj13);
                            }

                            if (row[45].ToString() != null && row[45].ToString() != "0")//Franchise Fee 
                            {
                                settlement_amt_type subobj14 = new settlement_amt_type();
                                subobj14.amount = Math.Round(Convert.ToDouble(row[45].ToString()), 2);
                                subobj14.description = firstrow[45].ToString().Replace("(Rs.)", "").Trim();
                                li.Add(subobj14);
                            }

                            if (row[46].ToString() != null && row[46].ToString() != "0")//Product Cancellation Fee
                            {
                                settlement_amt_type subobj15 = new settlement_amt_type();
                                subobj15.amount = Math.Round(Convert.ToDouble(row[46].ToString()), 2);
                                subobj15.description = firstrow[46].ToString().Replace("(Rs.)", "").Trim();
                                li.Add(subobj15);
                            }

                            if (row[47].ToString() != null && row[47].ToString() != "0")//Service Cancellation Fee
                            {
                                settlement_amt_type subobj16 = new settlement_amt_type();
                                subobj16.amount = Math.Round(Convert.ToDouble(row[47].ToString()), 2);
                                subobj16.description = firstrow[47].ToString().Replace("(Rs.)", "").Trim();
                                li.Add(subobj16);
                            }

                            if (row[48].ToString() != null && row[48].ToString() != "0")//Fee Discount
                            {
                                settlement_amt_type subobj17 = new settlement_amt_type();
                                subobj17.amount = Math.Round(Convert.ToDouble(row[48].ToString()), 2);
                                subobj17.description = firstrow[48].ToString().Replace("(Rs.)", "").Trim();
                                li.Add(subobj17);
                            }

                            //////
                        }
                        else if (row[23].ToString() == "Courier Return" || row[23].ToString() == "Customer Return")
                        {
                            if (row[5].ToString() == "OD110995017084863000")
                            {
                            }
                            string sku = row[19].ToString();
                            string order_item_id = row[6].ToString();
                            List<settlement_amt_type> li = null;
                            if (obj_item.refund_amount_typesDict == null)
                                obj_item.refund_amount_typesDict = new Dictionary<string, List<settlement_amt_type>>();

                            //------------------------add data in order dictionary------------------//
                            if (row[14].ToString() != "" && row[14].ToString() != null)
                            {
                                List<settlement_amt_type> li1 = null;
                                settlement_amt_type subobj_1 = new settlement_amt_type();
                                if (row[7].ToString() != "")
                                    subobj_1.amount = Math.Round(Convert.ToDouble(row[7].ToString()), 2);
                                else
                                    subobj_1.amount = 0;
                                subobj_1.posteddatetime = row[16].ToString();
                                if (row[20].ToString() != "")
                                    subobj_1.qty = Convert.ToInt32(row[20].ToString());

                                subobj_1.description = "TotalSaleAmount";
                                if (row[14].ToString() != "0")
                                {

                                    if (obj_item.order_amount_typesDict == null)
                                    {
                                        obj_item.order_amount_typesDict = new Dictionary<string, List<settlement_amt_type>>();
                                    }

                                    if (obj_item.order_amount_typesDict.ContainsKey(row[6].ToString()))
                                        li1 = obj_item.order_amount_typesDict[row[6].ToString()];
                                    else
                                    {
                                        li1 = new List<settlement_amt_type>();
                                        obj_item.order_amount_typesDict.Add(row[6].ToString(), li1);
                                    }

                                }
                                else if (row[14].ToString() == "0")
                                {
                                    if (obj_item.order_amount_typesDict == null)
                                    {
                                        obj_item.order_amount_typesDict = new Dictionary<string, List<settlement_amt_type>>();
                                    }

                                    if (obj_item.order_amount_typesDict.ContainsKey(row[6].ToString()))
                                        li1 = obj_item.order_amount_typesDict[row[6].ToString()];
                                    else
                                    {
                                        li1 = new List<settlement_amt_type>();
                                        obj_item.order_amount_typesDict.Add(row[6].ToString(), li1);
                                    }

                                }
                                if (row[7].ToString() != "" && row[7].ToString() != "0")// new
                                {
                                    li1.Add(subobj_1);
                                }

                                if (row[14].ToString() != "" && row[14].ToString() != "0")
                                    subobj.amount = Math.Round(Convert.ToDouble(row[14].ToString()), 2);
                                else
                                    subobj.amount = 0;
                            }
                            //------------------------------End------------------------------------//
                            //--------------------------add protection fund ----------------------//
                            if (row[13].ToString() != "" && row[13].ToString() != null && row[13].ToString() != "0")
                            {
                                settlement_amt_type subobj_2 = new settlement_amt_type();
                                if (row[13].ToString() != "")
                                    subobj_2.amount = Math.Round(Convert.ToDouble(row[13].ToString()), 2);
                                else
                                    subobj_2.amount = 0;
                                subobj_2.posteddatetime = row[16].ToString();
                                if (row[20].ToString() != "")
                                    subobj_2.qty = Convert.ToInt32(row[20].ToString());

                                subobj_2.description = "ProtectionFund";

                                if (obj_item.refund_amount_typesDict == null)
                                {
                                    obj_item.refund_amount_typesDict = new Dictionary<string, List<settlement_amt_type>>();
                                }

                                List<settlement_amt_type> li2 = null;
                                if (obj_item.refund_amount_typesDict.ContainsKey(row[6].ToString()))
                                    li2 = obj_item.refund_amount_typesDict[row[6].ToString()];
                                else
                                {
                                    li2 = new List<settlement_amt_type>();
                                    obj_item.refund_amount_typesDict.Add(row[6].ToString(), li2);
                                }

                                li2.Add(subobj_2);
                            }

                            //--------------------------------END--------------------------------//


                            if (obj_item.refund_amount_typesDict.ContainsKey(order_item_id))// if (obj_item.refund_amount_typesDict.ContainsKey(sku))
                            {
                                li = obj_item.refund_amount_typesDict[row[6].ToString()];// li = obj_item.refund_amount_typesDict[row[19].ToString()];
                                if (li.Count > 0)
                                {
                                    if (row[7].ToString() != "" && row[7].ToString() != "0")//new
                                    {
                                        li.Add(subobj);
                                    }
                                }
                                //else
                                //    li.Add(subobj);
                            }
                            else
                            {
                                li = new List<settlement_amt_type>();
                                if (row[7].ToString() != "" && row[7].ToString() != "0")//new
                                {
                                    li.Add(subobj);
                                    obj_item.refund_amount_typesDict.Add(order_item_id, li);
                                }//obj_item.refund_amount_typesDict.Add(sku, li)

                            }
                            if (row[32].ToString() != null && row[32].ToString() != "0")//Commission
                            {
                                settlement_amt_type subobj_1 = new settlement_amt_type();
                                subobj_1.amount = Math.Round(Convert.ToDouble(row[32].ToString()), 2);
                                subobj_1.description = firstrow[32].ToString().Replace("(Rs.)", "").Trim();
                                li.Add(subobj_1);
                            }
                            if (row[33].ToString() != null && row[33].ToString() != "0")//Collection_Fee
                            {
                                settlement_amt_type subobj_2 = new settlement_amt_type();
                                subobj_2.amount = Math.Round(Convert.ToDouble(row[33].ToString()), 2);
                                subobj_2.description = firstrow[33].ToString().Replace("(Rs.)", "").Trim();
                                li.Add(subobj_2);
                            }

                            if (row[34].ToString() != null && row[34].ToString() != "0")//Fixed Fee
                            {
                                settlement_amt_type subobj_3 = new settlement_amt_type();
                                subobj_3.amount = Math.Round(Convert.ToDouble(row[34].ToString()), 2);
                                subobj_3.description = firstrow[34].ToString().Replace("(Rs.)", "").Trim();
                                li.Add(subobj_3);
                            }

                            if (row[35].ToString() != null && row[35].ToString() != "0")//No Cost Emi Fee Reimbursement
                            {
                                settlement_amt_type subobj_4 = new settlement_amt_type();
                                subobj_4.amount = Math.Round(Convert.ToDouble(row[35].ToString()), 2);
                                subobj_4.description = firstrow[35].ToString().Replace("(Rs.)", "").Trim();
                                li.Add(subobj_4);
                            }

                            if (row[36].ToString() != null && row[36].ToString() != "0")//Installation Fee
                            {
                                settlement_amt_type subobj_5 = new settlement_amt_type();
                                subobj_5.amount = Math.Round(Convert.ToDouble(row[36].ToString()), 2);
                                subobj_5.description = firstrow[36].ToString().Replace("(Rs.)", "").Trim();
                                li.Add(subobj_5);
                            }

                            if (row[37].ToString() != null && row[37].ToString() != "0")//Uninstallation Fee
                            {
                                settlement_amt_type subobj_6 = new settlement_amt_type();
                                subobj_6.amount = Math.Round(Convert.ToDouble(row[37].ToString()), 2);
                                subobj_6.description = firstrow[37].ToString().Replace("(Rs.)", "").Trim();
                                li.Add(subobj_6);
                            }

                            if (row[38].ToString() != null && row[38].ToString() != "0")//Tech Visit Fee
                            {
                                settlement_amt_type subobj_7 = new settlement_amt_type();
                                subobj_7.amount = Math.Round(Convert.ToDouble(row[38].ToString()), 2);
                                subobj_7.description = firstrow[38].ToString().Replace("(Rs.)", "").Trim();
                                li.Add(subobj_7);
                            }

                            if (row[39].ToString() != null && row[39].ToString() != "0")//Uninstallation & Packaging Fee
                            {
                                settlement_amt_type subobj_8 = new settlement_amt_type();
                                subobj_8.amount = Math.Round(Convert.ToDouble(row[39].ToString()), 2);
                                subobj_8.description = firstrow[39].ToString().Replace("(Rs.)", "").Trim();
                                li.Add(subobj_8);
                            }

                            if (row[40].ToString() != null && row[40].ToString() != "0")//Pick And Pack Fee
                            {
                                settlement_amt_type subobj_9 = new settlement_amt_type();
                                subobj_9.amount = Math.Round(Convert.ToDouble(row[40].ToString()), 2);
                                subobj_9.description = firstrow[40].ToString().Replace("(Rs.)", "").Trim();
                                li.Add(subobj_9);
                            }

                            if (row[41].ToString() != null && row[41].ToString() != "0" && row[41].ToString() != "")//Customer Shipping Fee Type
                            {
                                settlement_amt_type subobj_10 = new settlement_amt_type();
                                string testing = row[41].ToString();
                                if (row[41].ToString() != null && row[41].ToString() != "")
                                {
                                    subobj_10.amount = Math.Round(Convert.ToDouble(row[41].ToString()), 2);
                                }
                                else
                                {
                                    subobj_10.amount = 0;
                                }
                                subobj_10.description = firstrow[41].ToString().Replace("(Rs.)", "").Trim();
                                li.Add(subobj_10);
                            }

                            if (row[42].ToString() != null && row[42].ToString() != "0")//Customer Shipping Fee
                            {
                                settlement_amt_type subobj_11 = new settlement_amt_type();
                                subobj_11.amount = Math.Round(Convert.ToDouble(row[42].ToString()), 2);
                                subobj_11.description = firstrow[42].ToString().Replace("(Rs.)", "").Trim();
                                li.Add(subobj_11);
                            }

                            if (row[43].ToString() != null && row[43].ToString() != "0")//Shipping Fee
                            {
                                settlement_amt_type subobj_12 = new settlement_amt_type();
                                subobj_12.amount = Math.Round(Convert.ToDouble(row[43].ToString()), 2);
                                subobj_12.description = firstrow[43].ToString().Replace("(Rs.)", "").Trim();
                                li.Add(subobj_12);
                            }

                            if (row[44].ToString() != null && row[44].ToString() != "0")//Reverse Shipping Fee
                            {
                                settlement_amt_type subobj_13 = new settlement_amt_type();
                                subobj_13.amount = Math.Round(Convert.ToDouble(row[44].ToString()), 2);
                                subobj_13.description = firstrow[44].ToString().Replace("(Rs.)", "").Trim();
                                li.Add(subobj_13);
                            }

                            if (row[45].ToString() != null && row[45].ToString() != "0")//Franchise Fee
                            {
                                settlement_amt_type subobj_14 = new settlement_amt_type();
                                subobj_14.amount = Math.Round(Convert.ToDouble(row[45].ToString()), 2);
                                subobj_14.description = firstrow[45].ToString().Replace("(Rs.)", "").Trim();
                                li.Add(subobj_14);
                            }

                            if (row[46].ToString() != null && row[46].ToString() != "0")//Product Cancellation Fee
                            {
                                settlement_amt_type subobj_15 = new settlement_amt_type();
                                subobj_15.amount = Math.Round(Convert.ToDouble(row[46].ToString()), 2);
                                subobj_15.description = firstrow[46].ToString().Replace("(Rs.)", "").Trim();
                                li.Add(subobj_15);
                            }

                            if (row[47].ToString() != null && row[47].ToString() != "0")//Service Cancellation Fee
                            {
                                settlement_amt_type subobj_16 = new settlement_amt_type();
                                subobj_16.amount = Math.Round(Convert.ToDouble(row[47].ToString()), 2);
                                subobj_16.description = firstrow[47].ToString().Replace("(Rs.)", "").Trim();
                                li.Add(subobj_16);
                            }

                            if (row[48].ToString() != null && row[48].ToString() != "0")//Fee Discount
                            {
                                settlement_amt_type subobj_17 = new settlement_amt_type();
                                subobj_17.amount = Math.Round(Convert.ToDouble(row[48].ToString()), 2);
                                subobj_17.description = firstrow[48].ToString().Replace("(Rs.)", "").Trim();
                                li.Add(subobj_17);
                            }
                        }
                    }
                    else
                    {
                        obj_item = new reconciliationorder();

                        sett_amount += Convert.ToDouble(row[3]);

                        obj_item.settlement_id = row[0].ToString();
                        obj_item.settlement_start_date = row[2].ToString();
                        obj_item.settlement_amount = row[3].ToString();
                        obj_item.order_id = row[5].ToString();
                        obj_item.order_itemId = row[6].ToString();
                        obj_item.amount = row[7].ToString();
                        obj_item.Total_Offer_Amount = row[8].ToString();
                        obj_item.MY_Share_Amount = row[9].ToString();
                        obj_item.Customershipping_Amount = row[10].ToString();
                        obj_item.marketplace_fee = row[11].ToString();
                        obj_item.taxes = row[12].ToString();
                        obj_item.Protection_fund = row[13].ToString();
                        obj_item.Refund_Amount = row[14].ToString();
                        obj_item.sku = row[19].ToString();
                        obj_item.quantity_purchased = row[20].ToString();
                        obj_item.posted_date = row[16].ToString();
                        obj_item.posted_date_time = row[17].ToString();
                        obj_item.transaction_type = row[23].ToString();
                        settlement_amt_type subobj = new settlement_amt_type();
                        if (row[7].ToString() != "")
                            subobj.amount = Math.Round(Convert.ToDouble(row[7].ToString()), 2);
                        else
                            subobj.amount = 0;
                        subobj.posteddatetime = row[16].ToString();
                        if (row[20].ToString() != "")
                            subobj.qty = Convert.ToInt32(row[20].ToString());

                        subobj.description = "TotalSaleAmount";

                        if (row[23].ToString() == "NA")
                        {
                            if (row[5].ToString() == "OD110995017084863000")
                            {

                            }
                            //---------------------------add---------------------------//
                            //if (row[6].ToString() != "" && row[6].ToString() != null && row[6].ToString() != "0" && row[13].ToString() == "0")//vineet
                            //{
                            //    settlement_amt_type subobj_3 = new settlement_amt_type();
                            //    if (row[6].ToString() != "")
                            //        subobj_3.amount = Math.Round(Convert.ToDouble(row[6].ToString()), 2);
                            //    else
                            //        subobj_3.amount = 0;
                            //    subobj_3.posteddatetime = row[15].ToString();
                            //    if (row[20].ToString() != "")
                            //        subobj_3.qty = Convert.ToInt32(row[20].ToString());

                            //    subobj_3.description = "TotalSaleAmount";
                            //    if (obj_item.order_amount_typesDict == null)
                            //    {
                            //        obj_item.order_amount_typesDict = new Dictionary<string, List<settlement_amt_type>>();
                            //    }
                            //    List<settlement_amt_type> li3 = null;
                            //    if (obj_item.order_amount_typesDict.ContainsKey(row[5].ToString()))
                            //        li3 = obj_item.order_amount_typesDict[row[5].ToString()];
                            //    else
                            //    {
                            //        li3 = new List<settlement_amt_type>();
                            //        obj_item.order_amount_typesDict.Add(row[5].ToString(), li1);
                            //    }

                            //    li3.Add(subobj_3);
                            //}

                            //----------------------------end--------------------------//

                            //------------------------add protection fund ----------------------//
                            if (row[13].ToString() != "" && row[13].ToString() != null && row[13].ToString() != "0")
                            {
                                settlement_amt_type subobj_3 = new settlement_amt_type();
                                if (row[13].ToString() != "")
                                    subobj_3.amount = Math.Round(Convert.ToDouble(row[13].ToString()), 2);
                                else
                                    subobj_3.amount = 0;
                                subobj_3.posteddatetime = row[16].ToString();
                                if (row[20].ToString() != "")
                                    subobj_3.qty = Convert.ToInt32(row[20].ToString());

                                subobj_3.description = "ProtectionFund";
                                obj_item.order_amount_typesDict = new Dictionary<string, List<settlement_amt_type>>();
                                List<settlement_amt_type> li4 = new List<settlement_amt_type>();
                                li4.Add(subobj);
                            }

                            //--------------------------------END--------------------------------//


                            obj_item.order_amount_typesDict = new Dictionary<string, List<settlement_amt_type>>();
                            List<settlement_amt_type> li = new List<settlement_amt_type>();
                            li.Add(subobj);


                            if (row[32].ToString() != null && row[32].ToString() != "0")//commission
                            {
                                settlement_amt_type subobj1 = new settlement_amt_type();
                                subobj1.amount = Math.Round(Convert.ToDouble(row[32].ToString()), 2);
                                subobj1.description = firstrow[32].ToString().Replace("(Rs.)", "").Trim();
                                li.Add(subobj1);
                            }

                            if (row[33].ToString() != null && row[33].ToString() != "0")//collection fee
                            {
                                settlement_amt_type subobj2 = new settlement_amt_type();
                                subobj2.amount = Math.Round(Convert.ToDouble(row[33].ToString()), 2);
                                subobj2.description = firstrow[33].ToString().Replace("(Rs.)", "").Trim();
                                li.Add(subobj2);
                            }

                            if (row[34].ToString() != null && row[34].ToString() != "0")//fixed fee
                            {
                                settlement_amt_type subobj3 = new settlement_amt_type();
                                subobj3.amount = Math.Round(Convert.ToDouble(row[34].ToString()), 2);
                                subobj3.description = firstrow[34].ToString().Replace("(Rs.)", "").Trim();
                                li.Add(subobj3);
                            }

                            if (row[35].ToString() != null && row[35].ToString() != "0")//no cost emi reimbursement
                            {
                                settlement_amt_type subobj4 = new settlement_amt_type();
                                subobj4.amount = Math.Round(Convert.ToDouble(row[35].ToString()), 2);
                                subobj4.description = firstrow[35].ToString().Replace("(Rs.)", "").Trim();
                                li.Add(subobj4);
                            }

                            if (row[36].ToString() != null && row[36].ToString() != "0")// installation fee
                            {
                                settlement_amt_type subobj5 = new settlement_amt_type();
                                subobj5.amount = Math.Round(Convert.ToDouble(row[36].ToString()), 2);
                                subobj5.description = firstrow[36].ToString().Replace("(Rs.)", "").Trim();
                                li.Add(subobj5);
                            }

                            if (row[37].ToString() != null && row[37].ToString() != "0")//uninstallation fee
                            {
                                settlement_amt_type subobj6 = new settlement_amt_type();
                                subobj6.amount = Math.Round(Convert.ToDouble(row[37].ToString()), 2);
                                subobj6.description = firstrow[37].ToString().Replace("(Rs.)", "").Trim();
                                li.Add(subobj6);
                            }

                            if (row[38].ToString() != null && row[38].ToString() != "0")//tech visit fee
                            {
                                settlement_amt_type subobj7 = new settlement_amt_type();
                                subobj7.amount = Math.Round(Convert.ToDouble(row[38].ToString()), 2);
                                subobj7.description = firstrow[38].ToString().Replace("(Rs.)", "").Trim();
                                li.Add(subobj7);
                            }

                            if (row[39].ToString() != null && row[39].ToString() != "0")//uninstallation package fee
                            {
                                settlement_amt_type subobj8 = new settlement_amt_type();
                                subobj8.amount = Math.Round(Convert.ToDouble(row[39].ToString()), 2);
                                subobj8.description = firstrow[39].ToString().Replace("(Rs.)", "").Trim();
                                li.Add(subobj8);
                            }

                            if (row[40].ToString() != null && row[40].ToString() != "0")//pick and pack fee
                            {
                                settlement_amt_type subobj9 = new settlement_amt_type();
                                subobj9.amount = Math.Round(Convert.ToDouble(row[40].ToString()), 2);
                                subobj9.description = firstrow[40].ToString().Replace("(Rs.)", "").Trim();
                                li.Add(subobj9);
                            }

                            if (row[41].ToString() != null && row[41].ToString() != "0" && row[41].ToString() != "")//customer shipping fee type
                            {
                                settlement_amt_type subobj10 = new settlement_amt_type();
                                string testing = row[41].ToString();
                                if (row[41].ToString() != null && row[41].ToString() != "")
                                {
                                    subobj10.amount = Math.Round(Convert.ToDouble(row[41].ToString()), 2);
                                }
                                else
                                {
                                    subobj10.amount = 0;
                                }
                                subobj10.description = firstrow[41].ToString().Replace("(Rs.)", "").Trim();
                                li.Add(subobj10);
                            }

                            if (row[42].ToString() != null && row[42].ToString() != "0")//customer shipping fee
                            {
                                settlement_amt_type subobj11 = new settlement_amt_type();
                                subobj11.amount = Math.Round(Convert.ToDouble(row[42].ToString()), 2);
                                subobj11.description = firstrow[42].ToString().Replace("(Rs.)", "").Trim();
                                li.Add(subobj11);
                            }

                            if (row[43].ToString() != null && row[43].ToString() != "0")//shipping fee
                            {
                                settlement_amt_type subobj12 = new settlement_amt_type();
                                subobj12.amount = Math.Round(Convert.ToDouble(row[43].ToString()), 2);
                                subobj12.description = firstrow[43].ToString().Replace("(Rs.)", "").Trim();
                                li.Add(subobj12);
                            }

                            if (row[44].ToString() != null && row[44].ToString() != "0")//reverse shipping fee
                            {
                                settlement_amt_type subobj13 = new settlement_amt_type();
                                subobj13.amount = Math.Round(Convert.ToDouble(row[44].ToString()), 2);
                                subobj13.description = firstrow[44].ToString().Replace("(Rs.)", "").Trim();
                                li.Add(subobj13);
                            }

                            if (row[45].ToString() != null && row[45].ToString() != "0")// franchise fee
                            {
                                settlement_amt_type subobj14 = new settlement_amt_type();
                                subobj14.amount = Math.Round(Convert.ToDouble(row[45].ToString()), 2);
                                subobj14.description = firstrow[45].ToString().Replace("(Rs.)", "").Trim();
                                li.Add(subobj14);
                            }

                            if (row[46].ToString() != null && row[46].ToString() != "0")//product cancellation fee
                            {
                                settlement_amt_type subobj15 = new settlement_amt_type();
                                subobj15.amount = Math.Round(Convert.ToDouble(row[46].ToString()), 2);
                                subobj15.description = firstrow[46].ToString().Replace("(Rs.)", "").Trim();
                                li.Add(subobj15);
                            }

                            if (row[47].ToString() != null && row[47].ToString() != "0")//service cancellation fee
                            {
                                settlement_amt_type subobj16 = new settlement_amt_type();
                                subobj16.amount = Math.Round(Convert.ToDouble(row[47].ToString()), 2);
                                subobj16.description = firstrow[47].ToString().Replace("(Rs.)", "").Trim();
                                li.Add(subobj16);
                            }

                            if (row[48].ToString() != null && row[48].ToString() != "0")// fee discount
                            {
                                settlement_amt_type subobj17 = new settlement_amt_type();
                                subobj17.amount = Math.Round(Convert.ToDouble(row[48].ToString()), 2);
                                subobj17.description = firstrow[48].ToString().Replace("(Rs.)", "").Trim();
                                li.Add(subobj17);
                            }

                            //obj_item.order_amount_typesDict.Add(row[19].ToString(), li);//for sku
                            obj_item.order_amount_typesDict.Add(row[5].ToString(), li);
                        }
                        else if (row[23].ToString() == "Courier Return" || row[23].ToString() == "Customer Return")
                        {
                            if (row[5].ToString() == "OD110995017084863000")
                            {

                            }
                            //if (row[13].ToString() != "" && row[13].ToString() != null && row[13].ToString() != "0")


                            //------------------------add data in order dictionary------------------//
                            if (row[14].ToString() != "" && row[14].ToString() != null)
                            {
                                List<settlement_amt_type> li1 = null;
                                settlement_amt_type subobj_1 = new settlement_amt_type();
                                if (row[7].ToString() != "")
                                    subobj_1.amount = Math.Round(Convert.ToDouble(row[7].ToString()), 2);
                                else
                                    subobj_1.amount = 0;
                                subobj_1.posteddatetime = row[16].ToString();
                                if (row[20].ToString() != "")
                                    subobj_1.qty = Convert.ToInt32(row[20].ToString());

                                subobj_1.description = "TotalSaleAmount";
                                if (row[14].ToString() != "0")
                                {

                                    if (obj_item.order_amount_typesDict == null)
                                    {
                                        obj_item.order_amount_typesDict = new Dictionary<string, List<settlement_amt_type>>();
                                    }

                                    if (obj_item.order_amount_typesDict.ContainsKey(row[6].ToString()))
                                        li1 = obj_item.order_amount_typesDict[row[6].ToString()];
                                    else
                                    {
                                        li1 = new List<settlement_amt_type>();
                                        obj_item.order_amount_typesDict.Add(row[6].ToString(), li1);
                                    }

                                }
                                else if (row[14].ToString() == "0")
                                {
                                    if (obj_item.order_amount_typesDict == null)
                                    {
                                        obj_item.order_amount_typesDict = new Dictionary<string, List<settlement_amt_type>>();
                                    }

                                    if (obj_item.order_amount_typesDict.ContainsKey(row[6].ToString()))
                                        li1 = obj_item.order_amount_typesDict[row[6].ToString()];
                                    else
                                    {
                                        li1 = new List<settlement_amt_type>();
                                        obj_item.order_amount_typesDict.Add(row[6].ToString(), li1);
                                    }

                                }
                                if (row[7].ToString() != "" && row[7].ToString() != "0")//new
                                {
                                    li1.Add(subobj_1);
                                }

                                if (row[14].ToString() != "" && row[14].ToString() != "0")
                                    subobj.amount = Math.Round(Convert.ToDouble(row[14].ToString()), 2);
                                else
                                    subobj.amount = 0;
                            }
                            //------------------------------End------------------------------------//

                            //--------------------------add protection fund ----------------------//
                            if (row[13].ToString() != "" && row[13].ToString() != null && row[13].ToString() != "0")
                            {
                                settlement_amt_type subobj_2 = new settlement_amt_type();
                                if (row[13].ToString() != "")
                                    subobj_2.amount = Math.Round(Convert.ToDouble(row[13].ToString()), 2);
                                else
                                    subobj_2.amount = 0;
                                subobj_2.posteddatetime = row[16].ToString();
                                if (row[20].ToString() != "")
                                    subobj_2.qty = Convert.ToInt32(row[20].ToString());

                                subobj_2.description = "ProtectionFund";

                                if (obj_item.refund_amount_typesDict == null)
                                {
                                    obj_item.refund_amount_typesDict = new Dictionary<string, List<settlement_amt_type>>();
                                }

                                List<settlement_amt_type> li2 = null;
                                if (obj_item.refund_amount_typesDict.ContainsKey(row[6].ToString()))
                                    li2 = obj_item.refund_amount_typesDict[row[6].ToString()];
                                else
                                {
                                    li2 = new List<settlement_amt_type>();
                                    obj_item.refund_amount_typesDict.Add(row[6].ToString(), li2);
                                }

                                li2.Add(subobj_2);
                            }

                            //--------------------------------END--------------------------------//
                            List<settlement_amt_type> li;
                            //if (row[23].ToString() == "Courier Return")
                            //{
                            //    if (obj_item.order_amount_typesDict == null)
                            //    {
                            //        obj_item.order_amount_typesDict = new Dictionary<string, List<settlement_amt_type>>();
                            //    }

                            //    if (obj_item.order_amount_typesDict.ContainsKey(row[5].ToString()))
                            //        li = obj_item.order_amount_typesDict[row[5].ToString()];
                            //    else
                            //    {
                            //        li = new List<settlement_amt_type>();
                            //        obj_item.order_amount_typesDict.Add(row[5].ToString(), li);
                            //    }
                            //    li.Add(subobj);
                            //}
                            //else
                            //{
                            if (obj_item.refund_amount_typesDict == null)
                            {
                                obj_item.refund_amount_typesDict = new Dictionary<string, List<settlement_amt_type>>();
                            }
                            if (obj_item.refund_amount_typesDict.ContainsKey(row[6].ToString()))
                                li = obj_item.refund_amount_typesDict[row[6].ToString()];
                            else
                            {
                                li = new List<settlement_amt_type>();
                                obj_item.refund_amount_typesDict.Add(row[6].ToString(), li);
                            }
                            li.Add(subobj);
                            //}

                            if (row[32].ToString() != null && row[32].ToString() != "0")//commission
                            {
                                settlement_amt_type subobj_1 = new settlement_amt_type();
                                subobj_1.amount = Math.Round(Convert.ToDouble(row[32].ToString()), 2);
                                subobj_1.description = firstrow[32].ToString().Replace("(Rs.)", "").Trim();
                                li.Add(subobj_1);
                            }

                            if (row[33].ToString() != null && row[33].ToString() != "0")//collection fee
                            {
                                settlement_amt_type subobj_2 = new settlement_amt_type();
                                subobj_2.amount = Math.Round(Convert.ToDouble(row[33].ToString()), 2);
                                subobj_2.description = firstrow[33].ToString().Replace("(Rs.)", "").Trim();
                                li.Add(subobj_2);
                            }

                            if (row[34].ToString() != null && row[34].ToString() != "0")//fixed fee
                            {
                                settlement_amt_type subobj_3 = new settlement_amt_type();
                                subobj_3.amount = Math.Round(Convert.ToDouble(row[34].ToString()), 2);
                                subobj_3.description = firstrow[34].ToString().Replace("(Rs.)", "").Trim();
                                li.Add(subobj_3);
                            }

                            if (row[35].ToString() != null && row[35].ToString() != "0")//no cost emi reimbursement
                            {
                                settlement_amt_type subobj_4 = new settlement_amt_type();
                                subobj_4.amount = Math.Round(Convert.ToDouble(row[35].ToString()), 2);
                                subobj_4.description = firstrow[35].ToString().Replace("(Rs.)", "").Trim();
                                li.Add(subobj_4);
                            }

                            if (row[36].ToString() != null && row[36].ToString() != "0")// installation fee
                            {
                                settlement_amt_type subobj_5 = new settlement_amt_type();
                                subobj_5.amount = Math.Round(Convert.ToDouble(row[36].ToString()), 2);
                                subobj_5.description = firstrow[36].ToString().Replace("(Rs.)", "").Trim();
                                li.Add(subobj_5);
                            }

                            if (row[37].ToString() != null && row[37].ToString() != "0")//uninstallation fee
                            {
                                settlement_amt_type subobj_6 = new settlement_amt_type();
                                subobj_6.amount = Math.Round(Convert.ToDouble(row[37].ToString()), 2);
                                subobj_6.description = firstrow[37].ToString().Replace("(Rs.)", "").Trim();
                                li.Add(subobj_6);
                            }

                            if (row[38].ToString() != null && row[38].ToString() != "0")//tech visit fee
                            {
                                settlement_amt_type subobj_7 = new settlement_amt_type();
                                subobj_7.amount = Math.Round(Convert.ToDouble(row[38].ToString()), 2);
                                subobj_7.description = firstrow[38].ToString().Replace("(Rs.)", "").Trim();
                                li.Add(subobj_7);
                            }

                            if (row[39].ToString() != null && row[39].ToString() != "0")//uninstallation package fee
                            {
                                settlement_amt_type subobj_8 = new settlement_amt_type();
                                subobj_8.amount = Math.Round(Convert.ToDouble(row[39].ToString()), 2);
                                subobj_8.description = firstrow[39].ToString().Replace("(Rs.)", "").Trim();
                                li.Add(subobj_8);
                            }

                            if (row[40].ToString() != null && row[40].ToString() != "0")//pick and pack fee
                            {
                                settlement_amt_type subobj_9 = new settlement_amt_type();
                                subobj_9.amount = Math.Round(Convert.ToDouble(row[40].ToString()), 2);
                                subobj_9.description = firstrow[40].ToString().Replace("(Rs.)", "").Trim();
                                li.Add(subobj_9);
                            }

                            if (row[41].ToString() != null && row[41].ToString() != "0" && row[41].ToString() != "")//customer shipping fee type
                            {
                                settlement_amt_type subobj_10 = new settlement_amt_type();
                                subobj_10.description = firstrow[41].ToString().Replace("(Rs.)", "").Trim();
                                string testing = row[41].ToString();
                                if (row[41].ToString() != null && row[41].ToString() != "")
                                {
                                    subobj_10.amount = Math.Round(Convert.ToDouble(row[41].ToString()), 2);
                                }
                                else
                                {
                                    subobj_10.amount = 0;
                                }

                                li.Add(subobj_10);
                            }

                            if (row[42].ToString() != null && row[42].ToString() != "0")//customer shipping fee
                            {
                                settlement_amt_type subobj_11 = new settlement_amt_type();
                                subobj_11.amount = Math.Round(Convert.ToDouble(row[42].ToString()), 2);
                                subobj_11.description = firstrow[42].ToString().Replace("(Rs.)", "").Trim();
                                li.Add(subobj_11);
                            }

                            if (row[43].ToString() != null && row[43].ToString() != "0")//shipping fee
                            {
                                settlement_amt_type subobj_12 = new settlement_amt_type();
                                subobj_12.amount = Math.Round(Convert.ToDouble(row[43].ToString()), 2);
                                subobj_12.description = firstrow[43].ToString().Replace("(Rs.)", "").Trim();
                                li.Add(subobj_12);
                            }

                            if (row[44].ToString() != null && row[44].ToString() != "0")//reverse shipping fee
                            {
                                settlement_amt_type subobj_13 = new settlement_amt_type();
                                subobj_13.amount = Math.Round(Convert.ToDouble(row[44].ToString()), 2);
                                subobj_13.description = firstrow[44].ToString().Replace("(Rs.)", "").Trim();
                                li.Add(subobj_13);
                            }

                            if (row[45].ToString() != null && row[45].ToString() != "0")// franchise fee
                            {
                                settlement_amt_type subobj_14 = new settlement_amt_type();
                                subobj_14.amount = Math.Round(Convert.ToDouble(row[45].ToString()), 2);
                                subobj_14.description = firstrow[45].ToString().Replace("(Rs.)", "").Trim();
                                li.Add(subobj_14);
                            }

                            if (row[46].ToString() != null && row[46].ToString() != "0")//product cancellation fee
                            {
                                settlement_amt_type subobj_15 = new settlement_amt_type();
                                subobj_15.amount = Math.Round(Convert.ToDouble(row[46].ToString()), 2);
                                subobj_15.description = firstrow[46].ToString().Replace("(Rs.)", "").Trim();
                                li.Add(subobj_15);
                            }

                            if (row[47].ToString() != null && row[47].ToString() != "0")//service cancellation fee
                            {
                                settlement_amt_type subobj_16 = new settlement_amt_type();
                                subobj_16.amount = Math.Round(Convert.ToDouble(row[47].ToString()), 2);
                                subobj_16.description = firstrow[47].ToString().Replace("(Rs.)", "").Trim();
                                li.Add(subobj_16);
                            }

                            if (row[48].ToString() != null && row[48].ToString() != "0")// fee discount
                            {
                                settlement_amt_type subobj_17 = new settlement_amt_type();
                                subobj_17.amount = Math.Round(Convert.ToDouble(row[48].ToString()), 2);
                                subobj_17.description = firstrow[48].ToString().Replace("(Rs.)", "").Trim();
                                li.Add(subobj_17);
                            }
                            // obj_item.refund_amount_typesDict.Add(row[5].ToString(), li);
                            //obj_item.refund_amount_typesDict.Add(row[19].ToString(), li);//for sku
                        }
                        objreconciliationorder.Add(obj_item);

                        //dictionary.Add(obj_item.sku, obj_item);
                        dictionary.Add(obj_item.order_id, obj_item);
                    }
                }// end of foreach
                #endregion

                sheetName = "Tax Details";

                System.Linq.IQueryable<LinqToExcel.Row> taxrows = from a in excel.Worksheet(sheetName) select a;
                ReadTax_Details(taxrows, ref dictionary);

                objjson1.Add(new AmazonreconciliationOrder
                {
                    reconciliationorder = objreconciliationorder,
                });

                if (objjson1 != null)
                {
                    foreach (var sitem in objjson1[0].reconciliationorder)
                    {
                        if (sitem.deposit_date != null)
                        {
                            sitem.total_amount = sett_amount.ToString();

                            break;
                        }
                    }
                }
                CronJobController objCron = new CronJobController();
                //objCron.SaveBulksettlementdata(objjson1, 1, 1, 19);
            }
            catch (Exception ex)
            {
            }
            return objjson1;
        }

        public Dictionary<string, reconciliationorder> ReadTax_Details1(System.Linq.IQueryable<LinqToExcel.Row> rows, ref Dictionary<string, reconciliationorder> dictionary)// used for read tax details from excel sheet
        {
            string success = "";
            try
            {
                int i = 0;
                LinqToExcel.Row firstrow = null;
                foreach (var row in rows)
                {
                    i++;
                    if (i == 1)
                    {
                        firstrow = row;
                        continue;
                    }
                    reconciliationorder obj_item = null;
                    reconciliationorder found_obj_item = null;
                    obj_item = new reconciliationorder();
                    List<reconciliationorder> li = new List<reconciliationorder>();

                    string order_detai_idd = row[2].ToString();
                    string settlement_name = row[5].ToString();
                    if (row[2].ToString() == "11099501708486300")
                    {
                    }
                    bool found = false;
                    foreach (KeyValuePair<string, reconciliationorder> pair in dictionary)
                    {
                        found_obj_item = pair.Value;

                        int sname = 0;
                        try
                        {
                            if (found_obj_item.order_amount_typesDict.Values.ToList().ElementAt(0).Count > 0)
                                for (int j = 0; j <= found_obj_item.order_amount_typesDict.Values.ToList().ElementAt(0).Count; j++)
                                {
                                    if (settlement_name == found_obj_item.order_amount_typesDict.Values.ToList().ElementAt(0).ElementAt(j).description)
                                    {
                                        sname = j;
                                        break;
                                    }// End of main if
                                }
                        }
                        catch (Exception ex)
                        {
                        }


                        if (found_obj_item.order_amount_typesDict != null && found_obj_item.order_amount_typesDict.ContainsKey(order_detai_idd))
                        {
                            if (found_obj_item.order_amount_typesDict[order_detai_idd] != null)
                            {
                                if (found_obj_item.order_amount_typesDict.Values.ToList().ElementAt(0).ElementAt(sname).description.Contains(settlement_name))
                                {
                                    List<settlement_amt_type> li1 = found_obj_item.order_amount_typesDict[order_detai_idd];
                                    settlement_amt_type subobj = new settlement_amt_type();
                                    if (row[13].ToString() != "" && row[13].ToString() != "0")
                                    {
                                        subobj.description = row[5].ToString() + " " + "IGST";
                                        subobj.amount = Convert.ToDouble(row[13].ToString());
                                        li1.Add(subobj);
                                    }
                                    else if (row[11].ToString() != "" && row[11].ToString() != "0")
                                    {
                                        subobj.description = row[5].ToString() + " " + "CGST";
                                        subobj.amount = Convert.ToDouble(row[11].ToString());
                                        li1.Add(subobj);

                                        settlement_amt_type subobj1 = new settlement_amt_type();
                                        subobj1.description = row[5].ToString() + " " + "SGST";
                                        subobj1.amount = Convert.ToDouble(row[12].ToString());
                                        li1.Add(subobj1);
                                    }
                                    break;
                                }

                            }

                            //}
                        }
                        int name = 0;
                        try
                        {
                            if (found_obj_item.refund_amount_typesDict.Values.ToList().ElementAt(0).Count > 0)
                                for (int j = 0; j <= found_obj_item.refund_amount_typesDict.Values.ToList().ElementAt(0).Count; j++)
                                {
                                    if (settlement_name == found_obj_item.refund_amount_typesDict.Values.ToList().ElementAt(0).ElementAt(j).description)
                                    {
                                        name = j;
                                        //if (found_obj_item.refund_amount_typesDict != null && found_obj_item.refund_amount_typesDict.ContainsKey(order_detai_idd))
                                        //{
                                        //    if (found_obj_item.refund_amount_typesDict.Values.ToList().ElementAt(0).ElementAt(j).description.Contains(settlement_name))
                                        //    {
                                        //        List<settlement_amt_type> li2 = found_obj_item.refund_amount_typesDict[order_detai_idd];
                                        //        settlement_amt_type subobj = new settlement_amt_type();
                                        //        if (row[12].ToString() != "" && row[12].ToString() != "0")
                                        //        {
                                        //            subobj.description = row[4].ToString() + " " + "IGST";
                                        //            subobj.amount = Convert.ToDouble(row[12].ToString());
                                        //            li2.Add(subobj);
                                        //        }
                                        //        else if (row[10].ToString() != "" && row[10].ToString() != "0")
                                        //        {
                                        //            subobj.description = row[4].ToString() + " " + "CGST";
                                        //            subobj.amount = Convert.ToDouble(row[10].ToString());
                                        //            li2.Add(subobj);
                                        //            settlement_amt_type subobj1 = new settlement_amt_type();
                                        //            subobj1.description = row[4].ToString() + " " + "SGST";
                                        //            subobj1.amount = Convert.ToDouble(row[11].ToString());
                                        //            li2.Add(subobj1);
                                        //        }
                                        //        break;
                                        //    }
                                        //}
                                        break;
                                    }// End of main if
                                }
                        }
                        catch (Exception ex)
                        {
                        }

                        if (found_obj_item.refund_amount_typesDict != null && found_obj_item.refund_amount_typesDict.ContainsKey(order_detai_idd))
                        {
                            if (found_obj_item.refund_amount_typesDict.Values.ToList().ElementAt(0).ElementAt(name).description.Contains(settlement_name))
                            {
                                List<settlement_amt_type> li2 = found_obj_item.refund_amount_typesDict[order_detai_idd];
                                settlement_amt_type subobj = new settlement_amt_type();
                                if (row[13].ToString() != "" && row[13].ToString() != "0")
                                {
                                    subobj.description = row[5].ToString() + " " + "IGST";
                                    subobj.amount = Convert.ToDouble(row[13].ToString());
                                    li2.Add(subobj);
                                }
                                else if (row[11].ToString() != "" && row[11].ToString() != "0")
                                {
                                    subobj.description = row[5].ToString() + " " + "CGST";
                                    subobj.amount = Convert.ToDouble(row[11].ToString());
                                    li2.Add(subobj);

                                    settlement_amt_type subobj1 = new settlement_amt_type();
                                    subobj1.description = row[5].ToString() + " " + "SGST";
                                    subobj1.amount = Convert.ToDouble(row[12].ToString());
                                    li2.Add(subobj1);
                                }
                                break;
                            }
                        }
                        //}
                    }//end for

                    if (found)
                    {
                        if (found_obj_item.order_amount_typesDict != null)
                        {

                        }//end if
                        else if (found_obj_item.refund_amount_typesDict != null)
                        {

                        }
                    }
                    else
                    {
                        //not found - its a problem
                        int a = 0;
                        a++;
                    }
                }// end of foreach
            }
            catch (Exception ex)
            {
                success = "Ex";
            }
            return dictionary;
        }


        public List<AmazonreconciliationOrder> ReadSettlementFile_Amazon_v2(string strFilePath, int seller_id)
        {
            List<AmazonreconciliationOrder> objjson1 = new List<AmazonreconciliationOrder>();
            List<reconciliationorder> objreconciliationorder = new List<reconciliationorder>();
            try
            {
                /*var excel = new ExcelQueryFactory(strFilePath)
                {
                    DatabaseEngine = LinqToExcel.Domain.DatabaseEngine.Ace,
                    TrimSpaces = LinqToExcel.Query.TrimSpacesType.Both,
                    UsePersistentConnection = true,
                    ReadOnly = true
                };

                var worksheetNames = excel.GetWorksheetNames();
                string sheetName = worksheetNames.First();

                //string sheetName = "Sheet1";

                var artistAlbums = from a in excel.Worksheet(sheetName) select a;
                 * */

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

                if (colCnt == 24)
                {
                    //flat file v2
                    // return null;
                }
                while (streamreader.Peek() > 0)
                {
                    DataRow datarow = datatable.NewRow();
                    datarow.ItemArray = streamreader.ReadLine().Split(delimiter);
                    datatable.Rows.Add(datarow);
                }

                Dictionary<string, reconciliationorder> dictionary = new Dictionary<string, reconciliationorder>();

                string qty = "0";

                foreach (DataRow row in datatable.Rows)
                {
                    //foreach (var row in artistAlbums)
                    //{
                    reconciliationorder obj_item = null;
                    string orderidd = row[7].ToString();

                    if (orderidd == "171-3392903-7191538")
                    {

                    }

                    if (orderidd == "404-3660718-7893903") //403-9291094-5853145")
                    {

                    }

                    if (row[13].ToString() == "Amazon Easy Ship Charges Tax")
                    {

                    }

                    if (dictionary.ContainsKey(orderidd))
                    {
                        obj_item = dictionary[orderidd];

                        settlement_amt_type subobj = new settlement_amt_type();
                        if (row[14].ToString() != "")
                        {
                            subobj.amount = 0;
                            try
                            {
                                subobj.amount = Math.Round(Convert.ToDouble(row[14].ToString()), 2);
                            }
                            catch (Exception ex)
                            {
                                int aaaa = 0;
                                aaaa++;
                                if (objreconciliationorder[0].suspenseAccountList == null)
                                    objreconciliationorder[0].suspenseAccountList = new List<settlement_amt_type>();

                                string r = String.Join(",", row.ItemArray);
                                subobj.type = r;
                                objreconciliationorder[0].suspenseAccountList.Add(subobj);
                            }
                        }

                        subobj.description = row[13].ToString();
                        subobj.type = row[12].ToString();
                        subobj.posteddatetime = row[17].ToString();

                        if (row[22].ToString() != "")
                            subobj.qty = Convert.ToInt32(row[22].ToString());

                        if (row[6].ToString() == "Cancellation")
                        {

                        }

                        if (row[6].ToString() == "Order" || (row[6].ToString() == "other-transaction" && (row[7].ToString() != null && row[7].ToString() != "") && (obj_item.refund_amount_typesDict == null && obj_item.order_amount_typesDict != null)) || row[6].ToString() == "Cancellation")
                        {
                            string sku = row[21].ToString();
                            if (obj_item.order_amount_typesDict.ContainsKey(sku))
                            {
                                List<settlement_amt_type> li = obj_item.order_amount_typesDict[row[21].ToString()];

                                if (subobj.description == "Principal" && li.Count > 0)
                                {
                                    if (li[li.Count - 1].description == "Product Tax")
                                        li.Insert(li.Count - 1, subobj);
                                    else
                                        li.Add(subobj);
                                }
                                else
                                    li.Add(subobj);

                            }
                            else
                            {
                                if (sku != null && sku != "")
                                {
                                    List<settlement_amt_type> li = new List<settlement_amt_type>();
                                    li.Add(subobj);
                                    obj_item.order_amount_typesDict.Add(sku, li);
                                }
                                else
                                {
                                    ////////////
                                    //sharad101 - added Amazon Easy Ship Charges Tax for supporting yellow files containg tax before GST time
                                    if (row[13].ToString() == "Amazon Easy Ship Charges" || row[13].ToString() == "MFNPostagePurchaseCompleteIGST" ||
                                        row[13].ToString() == "Amazon Easy Ship Charges Tax" || row[13].ToString() == "MFNPostagePurchaseCompleteCGST" || row[13].ToString() == "MFNPostagePurchaseCompleteSGST")
                                    {
                                        string shipment_id = row[10].ToString();
                                        subobj.amount = Math.Round(Convert.ToDouble(row[14].ToString()), 2);

                                        if (obj_item.easyship_amount_typesDict == null)
                                            obj_item.easyship_amount_typesDict = new Dictionary<string, List<settlement_amt_type>>();

                                        if (obj_item.easyship_amount_typesDict.ContainsKey(shipment_id))
                                        {
                                            List<settlement_amt_type> li = obj_item.easyship_amount_typesDict[shipment_id];
                                            li.Add(subobj);
                                        }
                                        else
                                        {
                                            List<settlement_amt_type> li = new List<settlement_amt_type>();
                                            li.Add(subobj);
                                            obj_item.easyship_amount_typesDict.Add(shipment_id, li);
                                        }
                                    }
                                    else
                                    {
                                        //suspense
                                        int aaaa = 0;
                                        aaaa++;
                                        if (objreconciliationorder[0].suspenseAccountList == null)
                                            objreconciliationorder[0].suspenseAccountList = new List<settlement_amt_type>();

                                        string r = String.Join(",", row.ItemArray);
                                        subobj.type = r;
                                        objreconciliationorder[0].suspenseAccountList.Add(subobj);
                                    }

                                    /////////////

                                }
                            }
                        }
                        else if (row[6].ToString() == "Refund" || obj_item.refund_amount_typesDict != null)
                        {
                            string sku = row[21].ToString();
                            if (obj_item.refund_amount_typesDict == null)
                                obj_item.refund_amount_typesDict = new Dictionary<string, List<settlement_amt_type>>();

                            if (obj_item.refund_amount_typesDict.ContainsKey(sku))
                            {
                                List<settlement_amt_type> li = obj_item.refund_amount_typesDict[row[21].ToString()];

                                if (subobj.description == "Principal" && li.Count > 0)
                                {
                                    if (li[li.Count - 1].description == "Product Tax")
                                        li.Insert(li.Count - 1, subobj);
                                    else
                                        li.Add(subobj);
                                }
                                else
                                    li.Add(subobj);
                            }
                            else
                            {
                                if (sku != null && sku != "")
                                {
                                    List<settlement_amt_type> li = new List<settlement_amt_type>();
                                    li.Add(subobj);
                                    obj_item.refund_amount_typesDict.Add(sku, li);
                                }
                                else
                                {
                                    if (row[13].ToString() == "Amazon Easy Ship Charges" || row[13].ToString() == "MFNPostagePurchaseCompleteIGST" ||
                                        row[13].ToString() == "Amazon Easy Ship Charges Tax" || row[13].ToString() == "MFNPostagePurchaseCompleteCGST" || row[13].ToString() == "MFNPostagePurchaseCompleteSGST")
                                    {
                                        string shipment_id = row[10].ToString();
                                        subobj.amount = Math.Round(Convert.ToDouble(row[14].ToString()), 2);

                                        if (obj_item.easyship_amount_typesDict == null)
                                            obj_item.easyship_amount_typesDict = new Dictionary<string, List<settlement_amt_type>>();

                                        if (obj_item.easyship_amount_typesDict.ContainsKey(shipment_id))
                                        {
                                            List<settlement_amt_type> li = obj_item.easyship_amount_typesDict[shipment_id];
                                            li.Add(subobj);
                                        }
                                        else
                                        {
                                            List<settlement_amt_type> li = new List<settlement_amt_type>();
                                            li.Add(subobj);
                                            obj_item.easyship_amount_typesDict.Add(shipment_id, li);
                                        }
                                    }
                                    else
                                    {
                                        //suspense
                                        int aaaa = 0;
                                        aaaa++;
                                        if (objreconciliationorder[0].suspenseAccountList == null)
                                            objreconciliationorder[0].suspenseAccountList = new List<settlement_amt_type>();

                                        string r = String.Join(",", row.ItemArray);
                                        subobj.type = r;
                                        objreconciliationorder[0].suspenseAccountList.Add(subobj);
                                    }

                                }
                                //List<settlement_amt_type> li = new List<settlement_amt_type>();
                                //li.Add(subobj);
                                //obj_item.refund_amount_typesDict.Add(sku, li);
                            }
                        }
                        //#region refund

                        //else if (row[6].ToString() == "Refund" || obj_item.refund_amount_typesDict != null || row[6].ToString() == "other-transaction")
                        //{
                        //    string sku = row[21].ToString();
                        //    if (obj_item.refund_amount_typesDict.ContainsKey(sku))
                        //    {
                        //        List<settlement_amt_type> li = obj_item.refund_amount_typesDict[row[21].ToString()];

                        //        if (subobj.description == "Principal" && li.Count > 0)
                        //        {
                        //            if (li[li.Count - 1].description == "Product Tax")
                        //                li.Insert(li.Count - 1, subobj);
                        //            else
                        //                li.Add(subobj);
                        //        }
                        //        else
                        //            li.Add(subobj);

                        //    }
                        //    else
                        //    {
                        //        if (sku != null && sku != "")
                        //        {
                        //            List<settlement_amt_type> li = new List<settlement_amt_type>();
                        //            li.Add(subobj);
                        //            obj_item.refund_amount_typesDict.Add(sku, li);
                        //        }
                        //        else
                        //        {
                        //            if (row[13].ToString() == "Amazon Easy Ship Charges" || row[13].ToString() == "MFNPostagePurchaseCompleteIGST" ||
                        //                row[13].ToString() == "Amazon Easy Ship Charges Tax" || row[13].ToString() == "MFNPostagePurchaseCompleteCGST" || row[13].ToString() == "MFNPostagePurchaseCompleteSGST")
                        //            {
                        //                string shipment_id = row[10].ToString();
                        //                subobj.amount = Math.Round(Convert.ToDouble(row[14].ToString()), 2);

                        //                if (obj_item.easyship_amount_typesDict == null)
                        //                    obj_item.easyship_amount_typesDict = new Dictionary<string, List<settlement_amt_type>>();

                        //                if (obj_item.easyship_amount_typesDict.ContainsKey(shipment_id))
                        //                {
                        //                    List<settlement_amt_type> li = obj_item.easyship_amount_typesDict[shipment_id];
                        //                    li.Add(subobj);
                        //                }
                        //                else
                        //                {
                        //                    List<settlement_amt_type> li = new List<settlement_amt_type>();
                        //                    li.Add(subobj);
                        //                    obj_item.easyship_amount_typesDict.Add(shipment_id, li);
                        //                }
                        //            }
                        //            else
                        //            {
                        //                //suspense
                        //                int aaaa = 0;
                        //                aaaa++;
                        //                if (objreconciliationorder[0].suspenseAccountList == null)
                        //                    objreconciliationorder[0].suspenseAccountList = new List<settlement_amt_type>();

                        //                string r = String.Join(",", row.ItemArray);
                        //                subobj.type = r;
                        //                objreconciliationorder[0].suspenseAccountList.Add(subobj);
                        //            }

                        //            /////////////

                        //        }
                        //    }
                        //}
                        //#endregion


                        else if (row[6].ToString() == "other-transaction" || row[6].ToString() == "ServiceFee")
                        {
                            if (obj_item.otherTransatanctionList == null)
                                obj_item.otherTransatanctionList = new List<settlement_amt_type>();

                            obj_item.otherTransatanctionList.Add(subobj);
                        }
                        else if (row[13].ToString() == "FBAInboundTransportationFee" || row[13].ToString() == "CGST" || row[13].ToString() == "SGST")
                        {
                            if (obj_item.otherTransatanctionList == null)
                                obj_item.otherTransatanctionList = new List<settlement_amt_type>();

                            obj_item.otherTransatanctionList.Add(subobj);
                        }
                        else
                        {
                            int aaaa = 0;

                            aaaa++;

                            if (objreconciliationorder[0].suspenseAccountList == null)
                                objreconciliationorder[0].suspenseAccountList = new List<settlement_amt_type>();

                            string r = String.Join(",", row.ItemArray);
                            subobj.type = r;
                            objreconciliationorder[0].suspenseAccountList.Add(subobj);
                            //sharad101
                            //problem - log it. Record lost i.e. not even picked up
                            //strFilePath
                            //sellerid
                            //row

                        }

                    }
                    else
                    {
                        obj_item = new reconciliationorder();
                        obj_item.settlement_id = row[0].ToString();
                        obj_item.settlement_start_date = row[1].ToString();
                        obj_item.settlement_end_date = row[2].ToString();
                        obj_item.deposit_date = row[3].ToString();
                        obj_item.total_amount = row[4].ToString();
                        obj_item.currency = row[5].ToString();
                        obj_item.transaction_type = row[6].ToString();
                        obj_item.order_id = row[7].ToString();
                        obj_item.merchant_order_id = row[8].ToString();
                        obj_item.adjustment_id = row[9].ToString();
                        obj_item.shipment_id = row[10].ToString();
                        obj_item.marketplace_name = row[11].ToString();
                        obj_item.amount_type = row[12].ToString();
                        obj_item.amount_description = row[13].ToString();
                        obj_item.amount = row[14].ToString();
                        obj_item.fulfillment_id = row[15].ToString();
                        obj_item.posted_date = row[16].ToString();
                        obj_item.posted_date_time = row[17].ToString();
                        obj_item.order_item_code = row[18].ToString();
                        obj_item.merchant_order_item_id = row[19].ToString();
                        obj_item.merchant_adjustment_item_id = row[20].ToString();
                        obj_item.sku = row[21].ToString();
                        obj_item.quantity_purchased = row[22].ToString();

                        //sharad 231
                        try
                        {
                            if (row[23] != null)
                                obj_item.promotion_id = row[23].ToString();
                        }
                        catch (Exception e1)
                        {
                            Writelog log = new Writelog();
                            if (strFilePath.Length > 100)
                                strFilePath = strFilePath.Substring(strFilePath.Length - 100, 98);
                            log.write_exception_log(strFilePath, "Browse_Upload_Excel_Utility", "ReadSettlementFile_Amazon_v2 row[23]", DateTime.Now, e1);
                        }

                        settlement_amt_type subobj = new settlement_amt_type();
                        if (row[14].ToString() != "")
                            subobj.amount = Math.Round(Convert.ToDouble(row[14].ToString()), 2);
                        else
                            subobj.amount = 0;
                        subobj.description = row[13].ToString();
                        subobj.type = row[12].ToString();
                        subobj.posteddatetime = row[17].ToString();
                        if (row[22].ToString() != "")
                            subobj.qty = Convert.ToInt32(row[22].ToString());

                        if (row[6].ToString() == "Cancellation")
                        {

                        }

                        if (row[6].ToString() == "Order" || (row[6].ToString() == "other-transaction" && (row[7].ToString() != null || row[7].ToString() != "")) || row[6].ToString() == "Cancellation")
                        {
                            obj_item.order_amount_typesDict = new Dictionary<string, List<settlement_amt_type>>();
                            List<settlement_amt_type> li = new List<settlement_amt_type>();
                            li.Add(subobj);
                            obj_item.order_amount_typesDict.Add(row[21].ToString(), li); //21 is sku
                        }
                        else if (row[6].ToString() == "Refund")
                        {
                            obj_item.refund_amount_typesDict = new Dictionary<string, List<settlement_amt_type>>();
                            List<settlement_amt_type> li = new List<settlement_amt_type>();
                            li.Add(subobj);
                            obj_item.refund_amount_typesDict.Add(row[21].ToString(), li); //sku
                        }
                        else if (row[6].ToString() == "SAFE-T Reimbursement" && (row[7].ToString() != null && row[7].ToString() != ""))
                        {
                            //sharad 122
                            obj_item.order_amount_typesDict = new Dictionary<string, List<settlement_amt_type>>();
                            List<settlement_amt_type> li = new List<settlement_amt_type>();
                            li.Add(subobj);
                            obj_item.order_amount_typesDict.Add(row[21].ToString(), li); //21 is sku
                        }
                        else
                        {
                            //suspense table write 

                        }

                        objreconciliationorder.Add(obj_item);

                        dictionary.Add(obj_item.order_id, obj_item);
                    }
                }

                objjson1.Add(new AmazonreconciliationOrder
                {
                    reconciliationorder = objreconciliationorder,
                });
            }
            catch (Exception ex)
            {
                Writelog log = new Writelog();
                log.write_exception_log("0", "Browse_Upload_Excel_Utility", "ReadSettlementFile_Amazon_v2", DateTime.Now, ex);
                return null;
            }
            return objjson1;
        }

        public List<AmazonreconciliationOrder> ReadSettlementFile_Amazon_flatfile(string strFilePath, int seller_id)
        {
            List<AmazonreconciliationOrder> objjson1 = new List<AmazonreconciliationOrder>();
            List<reconciliationorder> objreconciliationorder = new List<reconciliationorder>();
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
                if (colCnt == 33)
                {
                    //flat file
                }
                else if (colCnt == 24)
                {
                    //flat file v2
                    return null;
                }
                while (streamreader.Peek() > 0)
                {
                    DataRow datarow = datatable.NewRow();
                    datarow.ItemArray = streamreader.ReadLine().Split(delimiter);
                    datatable.Rows.Add(datarow);
                }

                Dictionary<string, reconciliationorder> dictionary = new Dictionary<string, reconciliationorder>();

                string qty = "0";

                foreach (DataRow row in datatable.Rows)
                {
                    if (row[6].ToString() == "Previous Reserve Amount Balance")
                    {

                    }
                    reconciliationorder obj_item = null;
                    string orderidd = row[7].ToString();
                    if (orderidd != "" && dictionary.ContainsKey(orderidd))
                    {
                        obj_item = dictionary[orderidd];

                        if (orderidd == "407-4119461-3523549")
                        {

                        }
                        settlement_amt_type subobj = new settlement_amt_type();

                        if (row[22].ToString() != "") //qty
                        {
                            qty = row[22].ToString();
                            if (row[6].ToString() == "Order")
                                continue;
                        }
                        if (row[23].ToString() != "") //price-type
                        {
                            if (row[24].ToString() != "")
                            {
                                subobj.amount = Math.Round(Convert.ToDouble(row[24].ToString()), 2);
                            }
                            subobj.description = row[23].ToString();
                            subobj.type = "price-type";
                        }
                        else if (row[25].ToString() != "") //item-related-fee-type
                        {
                            if (row[26].ToString() != "")
                            {
                                subobj.amount = Math.Round(Convert.ToDouble(row[26].ToString()), 2);
                            }
                            subobj.description = row[25].ToString();
                            subobj.type = "item-related-fee-type"; // row[12].ToString();
                        }


                        subobj.posteddatetime = row[17].ToString();

                        subobj.qty = Convert.ToInt32(qty);

                        if (row[6].ToString() == "Order")
                        {
                            string sku = row[21].ToString();
                            if (obj_item.order_amount_typesDict.ContainsKey(sku))
                            {
                                List<settlement_amt_type> li = obj_item.order_amount_typesDict[row[21].ToString()];
                                if (subobj.description == "Principal" && li.Count > 0)
                                {
                                    if (li[li.Count - 1].description == "Product Tax")
                                        li.Insert(li.Count - 1, subobj);
                                    else
                                        li.Add(subobj);
                                }
                                else
                                    li.Add(subobj);

                            }
                            else
                            {
                                List<settlement_amt_type> li = new List<settlement_amt_type>();
                                li.Add(subobj);
                                obj_item.order_amount_typesDict.Add(sku, li);
                            }
                        }
                        else if (row[6].ToString() == "Refund")
                        {
                            string sku = row[21].ToString();
                            if (obj_item.refund_amount_typesDict == null)
                                obj_item.refund_amount_typesDict = new Dictionary<string, List<settlement_amt_type>>();

                            if (obj_item.refund_amount_typesDict.ContainsKey(sku))
                            {
                                List<settlement_amt_type> li = obj_item.refund_amount_typesDict[row[21].ToString()];

                                if (subobj.description == "Principal" && li.Count > 0)
                                {
                                    if (li[li.Count - 1].description == "Product Tax")
                                        li.Insert(li.Count - 1, subobj);
                                    else
                                        li.Add(subobj);
                                }
                                else
                                    li.Add(subobj);
                            }
                            else
                            {
                                List<settlement_amt_type> li = new List<settlement_amt_type>();
                                li.Add(subobj);
                                obj_item.refund_amount_typesDict.Add(sku, li);
                            }
                        }
                        else if (row[6].ToString() == "SAFE-T Reimbursement")
                        {
                            if (obj_item.refund_amount_typesDict != null)
                            {
                                string sku = row[21].ToString();
                                if (obj_item.refund_amount_typesDict.ContainsKey(sku))
                                {
                                    List<settlement_amt_type> li = obj_item.refund_amount_typesDict[row[21].ToString()];
                                    subobj.description = "SAFE-T Reimbursement";
                                    if (row[32].ToString() != null && row[32].ToString() != "")
                                    {
                                        subobj.amount = Math.Round(Convert.ToDouble(row[32].ToString()), 2);
                                    }
                                    else
                                    {
                                        subobj.amount = Math.Round(Convert.ToDouble(row[35].ToString()), 2);
                                    }
                                    //subobj.amount = Math.Round(Convert.ToDouble(row[32].ToString()), 2);
                                    li.Add(subobj);
                                }
                            }
                            else if (obj_item.order_amount_typesDict != null)
                            {

                            }
                        }
                        else if (row[6].ToString() == "Amazon Easy Ship Charges")// changes for row 36
                        {
                            string shipment_id = row[10].ToString();
                            if (row[32].ToString() != null && row[32].ToString() != "")
                            {
                                subobj.amount = Math.Round(Convert.ToDouble(row[32].ToString()), 2);
                            }
                            else
                            {
                                subobj.amount = Math.Round(Convert.ToDouble(row[35].ToString()), 2);
                            }
                            //subobj.amount = Math.Round(Convert.ToDouble(row[32].ToString()), 2);
                            if (obj_item.easyship_amount_typesDict == null)
                                obj_item.easyship_amount_typesDict = new Dictionary<string, List<settlement_amt_type>>();

                            if (obj_item.easyship_amount_typesDict.ContainsKey(shipment_id))
                            {
                                List<settlement_amt_type> li = obj_item.easyship_amount_typesDict[shipment_id];
                                li.Add(subobj);
                            }
                            else
                            {
                                List<settlement_amt_type> li = new List<settlement_amt_type>();
                                li.Add(subobj);
                                obj_item.easyship_amount_typesDict.Add(shipment_id, li);
                            }
                        }
                        else if (row[6].ToString() == "Cancellation")
                        {
                            string sku = row[17].ToString(); //posted date
                            subobj.description = row[29].ToString();
                            subobj.amount = Math.Round(Convert.ToDouble(row[28].ToString()), 2);
                            if (obj_item.order_amount_typesDict.ContainsKey(sku))
                            {
                                List<settlement_amt_type> li = obj_item.order_amount_typesDict[row[17].ToString()];
                                li.Add(subobj);
                            }
                            else
                            {
                                List<settlement_amt_type> li = new List<settlement_amt_type>();
                                li.Add(subobj);
                                obj_item.order_amount_typesDict.Add(sku, li);
                            }
                        }
                        else if (row[6].ToString() == "other-transaction")
                        {
                            if (obj_item.otherTransatanctionList == null)
                                obj_item.otherTransatanctionList = new List<settlement_amt_type>();

                            obj_item.otherTransatanctionList.Add(subobj);
                        }

                    }
                    else
                    {
                        obj_item = new reconciliationorder();
                        obj_item.settlement_id = row[0].ToString();
                        obj_item.settlement_start_date = row[1].ToString();
                        obj_item.settlement_end_date = row[2].ToString();
                        obj_item.deposit_date = row[3].ToString();
                        obj_item.total_amount = row[4].ToString();
                        obj_item.currency = row[5].ToString();
                        obj_item.transaction_type = row[6].ToString();
                        obj_item.order_id = row[7].ToString();
                        obj_item.merchant_order_id = row[8].ToString();
                        obj_item.adjustment_id = row[9].ToString();
                        obj_item.shipment_id = row[10].ToString();
                        obj_item.marketplace_name = row[11].ToString();

                        //obj_item.amount_type = row[12].ToString();
                        //obj_item.amount_description = row[13].ToString();
                        //obj_item.amount = row[14].ToString();
                        obj_item.fulfillment_id = row[16].ToString();
                        //obj_item.posted_date = row[16].ToString();
                        //obj_item.posted_date_time = row[17].ToString();
                        //obj_item.order_item_code = row[18].ToString();
                        //obj_item.merchant_order_item_id = row[19].ToString();
                        //obj_item.merchant_adjustment_item_id = row[20].ToString();
                        //obj_item.sku = row[21].ToString();
                        //obj_item.quantity_purchased = row[22].ToString();
                        //obj_item.promotion_id = row[23].ToString();

                        settlement_amt_type subobj = new settlement_amt_type();

                        if (row[23].ToString() != "") //price-type
                        {
                            if (row[24].ToString() != "")
                            {
                                subobj.amount = Math.Round(Convert.ToDouble(row[24].ToString()), 2);
                            }
                            subobj.description = row[23].ToString();
                            subobj.type = "price-type";
                        }
                        else if (row[25].ToString() != "") //item-related-fee-type
                        {
                            if (row[26].ToString() != "")
                            {
                                subobj.amount = Math.Round(Convert.ToDouble(row[26].ToString()), 2);
                                subobj.type = "item-related-fee-type"; // row[12].ToString();
                            }
                            else
                            {
                                if (row[32].ToString() != null && row[32].ToString() != "")
                                {
                                    subobj.amount = Math.Round(Convert.ToDouble(row[32].ToString()), 2);
                                }
                                else
                                {
                                    subobj.amount = Math.Round(Convert.ToDouble(row[35].ToString()), 2);
                                }
                                //subobj.amount = Math.Round(Convert.ToDouble(row[32].ToString()), 2);
                                subobj.type = row[25].ToString();
                            }
                            subobj.description = row[25].ToString();

                        }

                        if (row[22].ToString() != "") //qty
                        {
                            qty = row[22].ToString();
                            subobj.qty = Convert.ToInt32(qty);
                        }

                        subobj.posteddatetime = row[17].ToString();
                        //if (row[22].ToString() != "")
                        //    subobj.qty = Convert.ToInt32(row[22].ToString());

                        if (row[6].ToString() == "Order" || row[6].ToString() == "SAFE-T Reimbursement" || row[6].ToString() == "Cancellation")
                        {
                            obj_item.order_amount_typesDict = new Dictionary<string, List<settlement_amt_type>>();
                            List<settlement_amt_type> li = new List<settlement_amt_type>();
                            li.Add(subobj);

                            if (row[6].ToString() == "SAFE-T Reimbursement")
                            {
                                subobj.description = "SAFE-T Reimbursement";
                                if (row[32].ToString() != null && row[32].ToString() != "")
                                {
                                    subobj.amount = Math.Round(Convert.ToDouble(row[32].ToString()), 2);
                                }
                                else
                                {
                                    subobj.amount = Math.Round(Convert.ToDouble(row[35].ToString()), 2);
                                }
                                //subobj.amount = Math.Round(Convert.ToDouble(row[32].ToString()), 2);
                                obj_item.order_amount_typesDict.Add(row[21].ToString(), li); //sku is the key
                            }
                            else if (row[6].ToString() == "Cancellation")
                            {
                                string posted = row[17].ToString(); //posted date
                                subobj.description = row[29].ToString(); // "Order Cancellation Charge";
                                subobj.amount = Math.Round(Convert.ToDouble(row[28].ToString()), 2);
                                obj_item.order_amount_typesDict.Add(posted, li); //posted date is the key
                            }
                            else
                            {
                                obj_item.order_amount_typesDict.Add(row[21].ToString(), li); //sku is the key
                            }
                        }
                        else if (row[6].ToString() == "Refund")
                        {
                            obj_item.refund_amount_typesDict = new Dictionary<string, List<settlement_amt_type>>();
                            List<settlement_amt_type> li = new List<settlement_amt_type>();
                            li.Add(subobj);
                            obj_item.refund_amount_typesDict.Add(row[21].ToString(), li);
                        }
                        else if (row[6].ToString() == "Amazon Easy Ship Charges")
                        {
                            string shipment_id = row[10].ToString();
                            obj_item.easyship_amount_typesDict = new Dictionary<string, List<settlement_amt_type>>();
                            List<settlement_amt_type> li = new List<settlement_amt_type>();
                            li.Add(subobj);
                            obj_item.easyship_amount_typesDict.Add(shipment_id, li); //shipment-id
                        }
                        else if (row[6].ToString() == "Previous Reserve Amount Balance")
                        {
                            if (obj_item.otherTransatanctionList == null)
                                obj_item.otherTransatanctionList = new List<settlement_amt_type>();

                            subobj.description = "Previous Reserve Amount Balance";//changes for row 36
                            if (row[32].ToString() != null && row[32].ToString() != "")
                            {
                                subobj.amount = Math.Round(Convert.ToDouble(row[32].ToString()), 2);
                            }
                            else
                            {
                                subobj.amount = Math.Round(Convert.ToDouble(row[35].ToString()), 2);
                            }

                            obj_item.otherTransatanctionList.Add(subobj);
                        }
                        else if (row[6].ToString() == "Current Reserve Amount")
                        {
                            if (obj_item.otherTransatanctionList == null)
                                obj_item.otherTransatanctionList = new List<settlement_amt_type>();

                            subobj.description = "Current Reserve Amount";
                            if (row[32].ToString() != null && row[32].ToString() != "")
                            {
                                subobj.amount = Math.Round(Convert.ToDouble(row[32].ToString()), 2);
                            }
                            else
                            {
                                subobj.amount = Math.Round(Convert.ToDouble(row[35].ToString()), 2);
                            }
                            //subobj.amount = Math.Round(Convert.ToDouble(row[32].ToString()), 2);
                            obj_item.otherTransatanctionList.Add(subobj);
                        }
                        else if (row[6].ToString() == "NonSubscriptionFeeAdj")
                        {
                            if (obj_item.otherTransatanctionList == null)
                                obj_item.otherTransatanctionList = new List<settlement_amt_type>();

                            subobj.description = "NonSubscriptionFeeAdj";
                            if (row[32].ToString() != null && row[32].ToString() != "")
                            {
                                subobj.amount = Math.Round(Convert.ToDouble(row[32].ToString()), 2);
                            }
                            else
                            {
                                subobj.amount = Math.Round(Convert.ToDouble(row[35].ToString()), 2);
                            }
                            //subobj.amount = Math.Round(Convert.ToDouble(row[32].ToString()), 2);
                            obj_item.otherTransatanctionList.Add(subobj);
                        }
                        else if (row[6].ToString() == "SAFE-T Reimbursement")
                        {

                        }

                        objreconciliationorder.Add(obj_item);

                        if (obj_item.order_id != "")
                            dictionary.Add(obj_item.order_id, obj_item);
                    }


                }//end foreach rows

                objjson1.Add(new AmazonreconciliationOrder
                {
                    reconciliationorder = objreconciliationorder,
                });


            }
            catch (Exception ex)
            {

                return null;
            }

            return objjson1;
        }




        public List<AmazonreconciliationOrder> ReadSettlementFile_Flipkart2(string strFilePath)
        {
            List<AmazonreconciliationOrder> objjson1 = new List<AmazonreconciliationOrder>();
            List<reconciliationorder> objreconciliationorder = new List<reconciliationorder>();
            try
            {
                var excel = new ExcelQueryFactory(strFilePath)
                {
                    DatabaseEngine = LinqToExcel.Domain.DatabaseEngine.Ace,
                    TrimSpaces = LinqToExcel.Query.TrimSpacesType.Both,
                    UsePersistentConnection = true,
                    ReadOnly = true
                };

                string sheetName = "Orders";

                var excelSheet = from a in excel.Worksheet(sheetName) select a;
                Dictionary<string, reconciliationorder> dictionary = new Dictionary<string, reconciliationorder>();

                foreach (var row in excelSheet)
                {
                    reconciliationorder obj_item = null;
                    string orderidd = row[4].ToString();
                    if (dictionary.ContainsKey(orderidd))
                    {
                        obj_item = dictionary[orderidd];

                        settlement_amt_type subobj = new settlement_amt_type();
                        if (row[6].ToString() != "")
                            subobj.amount = Math.Round(Convert.ToDouble(row[6].ToString()), 2);

                        subobj.description = "Product value"; //row[13].ToString();
                        subobj.type = "Item Price"; //row[12].ToString();
                        subobj.posteddatetime = row[1].ToString();

                        if (row[20].ToString() != "")
                            subobj.qty = Convert.ToInt32(row[20].ToString());

                        int refundcase = Convert.ToInt16(row[13].ToString());
                        if (row[6].ToString() == "0" && refundcase > 0)
                        {
                            string sku = row[19].ToString();
                            if (obj_item.order_amount_typesDict.ContainsKey(sku))
                            {
                                List<settlement_amt_type> li = obj_item.order_amount_typesDict[row[21].ToString()];

                                if (subobj.description == "Principal" && li.Count > 0)
                                {
                                    if (li[li.Count - 1].description == "Product Tax")
                                        li.Insert(li.Count - 1, subobj);
                                    else
                                        li.Add(subobj);
                                }
                                else
                                    li.Add(subobj);

                            }
                            else
                            {
                                List<settlement_amt_type> li = new List<settlement_amt_type>();
                                li.Add(subobj);
                                obj_item.order_amount_typesDict.Add(sku, li);
                            }
                        }
                        if (row[6].ToString() == "Refund")
                        {
                            string sku = row[21].ToString();
                            if (obj_item.refund_amount_typesDict == null)
                                obj_item.refund_amount_typesDict = new Dictionary<string, List<settlement_amt_type>>();

                            if (obj_item.refund_amount_typesDict.ContainsKey(sku))
                            {
                                List<settlement_amt_type> li = obj_item.refund_amount_typesDict[row[21].ToString()];

                                if (subobj.description == "Principal" && li.Count > 0)
                                {
                                    if (li[li.Count - 1].description == "Product Tax")
                                        li.Insert(li.Count - 1, subobj);
                                    else
                                        li.Add(subobj);
                                }
                                else
                                    li.Add(subobj);

                            }
                            else
                            {
                                List<settlement_amt_type> li = new List<settlement_amt_type>();
                                li.Add(subobj);
                                obj_item.refund_amount_typesDict.Add(sku, li);
                            }
                        }
                        else if (row[6].ToString() == "other-transaction")
                        {
                            if (obj_item.otherTransatanctionList == null)
                                obj_item.otherTransatanctionList = new List<settlement_amt_type>();

                            obj_item.otherTransatanctionList.Add(subobj);
                        }

                    }
                    else
                    {
                        obj_item = new reconciliationorder();
                        obj_item.settlement_id = row[0].ToString();
                        obj_item.settlement_start_date = row[1].ToString();
                        obj_item.settlement_end_date = row[2].ToString();
                        obj_item.deposit_date = row[3].ToString();
                        obj_item.total_amount = row[4].ToString();
                        obj_item.currency = row[5].ToString();
                        obj_item.transaction_type = row[6].ToString();
                        obj_item.order_id = row[7].ToString();
                        obj_item.merchant_order_id = row[8].ToString();
                        obj_item.adjustment_id = row[9].ToString();
                        obj_item.shipment_id = row[10].ToString();
                        obj_item.marketplace_name = row[11].ToString();
                        obj_item.amount_type = row[12].ToString();
                        obj_item.amount_description = row[13].ToString();
                        obj_item.amount = row[14].ToString();
                        obj_item.fulfillment_id = row[15].ToString();
                        obj_item.posted_date = row[16].ToString();
                        obj_item.posted_date_time = row[17].ToString();
                        obj_item.order_item_code = row[18].ToString();
                        obj_item.merchant_order_item_id = row[19].ToString();
                        obj_item.merchant_adjustment_item_id = row[20].ToString();
                        obj_item.sku = row[21].ToString();
                        obj_item.quantity_purchased = row[22].ToString();
                        obj_item.promotion_id = row[23].ToString();

                        settlement_amt_type subobj = new settlement_amt_type();
                        if (row[14].ToString() != "")
                            subobj.amount = Math.Round(Convert.ToDouble(row[14].ToString()), 2);
                        else
                            subobj.amount = 0;
                        subobj.description = row[13].ToString();
                        subobj.type = row[12].ToString();
                        subobj.posteddatetime = row[17].ToString();
                        if (row[22].ToString() != "")
                            subobj.qty = Convert.ToInt32(row[22].ToString());

                        if (row[6].ToString() == "Order")
                        {
                            obj_item.order_amount_typesDict = new Dictionary<string, List<settlement_amt_type>>();
                            List<settlement_amt_type> li = new List<settlement_amt_type>();
                            li.Add(subobj);
                            obj_item.order_amount_typesDict.Add(row[21].ToString(), li);
                        }
                        else if (row[6].ToString() == "Refund")
                        {
                            obj_item.refund_amount_typesDict = new Dictionary<string, List<settlement_amt_type>>();
                            List<settlement_amt_type> li = new List<settlement_amt_type>();
                            li.Add(subobj);
                            obj_item.refund_amount_typesDict.Add(row[21].ToString(), li);
                        }
                        objreconciliationorder.Add(obj_item);

                        dictionary.Add(obj_item.order_id, obj_item);
                    }


                }

                objjson1.Add(new AmazonreconciliationOrder
                {
                    reconciliationorder = objreconciliationorder,
                });


            }
            catch (Exception ex)
            {

                return null;
            }

            return objjson1;
        }


        public List<AmazonreconciliationOrder> ReconciliationOrder_Browse1(string strFilePath)
        {
            List<AmazonreconciliationOrder> objjson1 = new List<AmazonreconciliationOrder>();
            List<reconciliationorder> objreconciliationorder = new List<reconciliationorder>();
            try
            {
                var excel = new ExcelQueryFactory(strFilePath)
                {
                    DatabaseEngine = LinqToExcel.Domain.DatabaseEngine.Ace,
                    TrimSpaces = LinqToExcel.Query.TrimSpacesType.Both,
                    UsePersistentConnection = true,
                    ReadOnly = true
                };
                // var planets = from p in excel.Worksheet<Planet>("Planets")
                //select p;
                string sheetName = "Sheet1";
                //var excelFile = new ExcelQueryFactory(strFolderPath1);
                var artistAlbums = from a in excel.Worksheet(sheetName) select a;
                ////var worksheetsList = excelFile.GetWorksheetNames();

                //string text;
                //string text1;
                //Excel.Application xlApp;
                //Excel.Workbook xlWorkBook;
                //Excel.Worksheet xlWorkSheet;
                //Excel.Range range;
                //DataTable dt = new DataTable();
                //string str;
                //int rCnt;
                //int cCnt;
                //int rw = 0;
                //int cl = 0;

                //xlApp = new Excel.Application();
                ////xlWorkBook = xlApp.Workbooks.Open(@"c:\\7723918584017438.xls", 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
                //xlWorkBook = xlApp.Workbooks.Open(strFilePath, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
                //xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

                //range = xlWorkSheet.UsedRange;
                //rw = range.Rows.Count;
                //cl = range.Columns.Count;
                //for (int i = 1; i <= rw; i++)
                //{
                //    DataRow dtrow = dt.NewRow();
                //    for (int j = 1; j <= cl; j++)
                //    {
                //        int cell = j - 1;
                //        var stsr = (range.Cells[i, j] as Excel.Range).Value2;
                //        if (i == 1)
                //            dt.Columns.Add(stsr);
                //        else
                //            dtrow[cell] = stsr;
                //    }
                //    if (i != 1)
                //        dt.Rows.Add(dtrow);
                //}
                //if (dt.Rows.Count > 0)
                //{
                foreach (var row in artistAlbums)
                {
                    string artistInfo = "Artist Name: {0}; Album: {1}";


                    //}
                    //    for (int k = 0; k < dt.Rows.Count; k++)
                    //    {
                    reconciliationorder obj_item = new reconciliationorder();
                    obj_item.settlement_id = row[0].ToString();
                    obj_item.settlement_start_date = row[1].ToString();
                    obj_item.settlement_end_date = row[2].ToString();
                    obj_item.deposit_date = row[3].ToString();
                    obj_item.total_amount = row[4].ToString();
                    obj_item.currency = row[5].ToString();
                    obj_item.transaction_type = row[6].ToString();
                    obj_item.order_id = row[7].ToString();
                    obj_item.merchant_order_id = row[8].ToString();
                    obj_item.adjustment_id = row[9].ToString();
                    obj_item.shipment_id = row[10].ToString();
                    obj_item.marketplace_name = row[11].ToString();
                    obj_item.amount_type = row[12].ToString();
                    obj_item.amount_description = row[13].ToString();
                    obj_item.amount = row[14].ToString();
                    obj_item.fulfillment_id = row[15].ToString();
                    obj_item.posted_date = row[16].ToString();
                    obj_item.posted_date_time = row[17].ToString();
                    obj_item.order_item_code = row[18].ToString();
                    obj_item.merchant_order_item_id = row[19].ToString();
                    obj_item.merchant_adjustment_item_id = row[20].ToString();
                    obj_item.sku = row[21].ToString();
                    obj_item.quantity_purchased = row[22].ToString();
                    obj_item.promotion_id = row[23].ToString();
                    objreconciliationorder.Add(obj_item);
                }

                objjson1.Add(new AmazonreconciliationOrder
                {
                    reconciliationorder = objreconciliationorder,
                });

                ////Savesettlementdata(objjson1);// for save settlement data in expense table


            }
            catch (Exception ex)
            {

                return null;
            }

            return objjson1;
        }



        public string csv(string strFilePath, int? strType, int? SellerId, int? marketplaceID, int? addorder, string strFileName)
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
                                        obj.order_date = DateTime.Parse(str);
                                        obj.shipment_id = values[2].ToString();
                                        str = values[3].ToString().Replace("\"", "");
                                        obj.shipment_date = DateTime.Parse(str);
                                        str = values[4].ToString().Replace("\"", "").Substring(0, 16);
                                        obj.tax_calculated_date = DateTime.Parse(str);
                                        str = values[5].ToString().Replace("\"", "");
                                        obj.posted_date = DateTime.Parse(str);
                                        obj.marketplace = values[6].ToString().Replace("\"", "");
                                        obj.tax_invoice_number = values[9].ToString().Replace("\"", "");
                                        obj.fulfillment = values[11].ToString().Replace("\"", "");
                                        obj.asin = values[12].ToString().Replace("\"", "");
                                        obj.sku = values[13].ToString().Replace("\"", "");
                                        obj.transaction_type = values[14].ToString();
                                        obj.product_tax_code = values[15].ToString().Replace("\"", "");
                                        obj.quantity = string.IsNullOrEmpty(values[16].ToString().Replace("\"", "")) ? 0 : int.Parse(values[16].ToString().Replace("\"", ""));
                                        obj.currency = values[17].ToString().Replace("\"", "");
                                        obj.display_price = string.IsNullOrEmpty(values[18].ToString().Replace("\"", "")) ? 0 : double.Parse(values[18].ToString().Replace("\"", ""));

                                        obj.is_display_price_taxinclusive = values[19].ToString().Replace("\"", "");
                                        obj.final_taxinclusive_selling_price = string.IsNullOrEmpty(values[20].ToString().Replace("\"", "")) ? 0 : double.Parse(values[20].ToString().Replace("\"", ""));
                                        obj.taxexclusive_selling_price = string.IsNullOrEmpty(values[21].ToString().Replace("\"", "")) ? 0 : double.Parse(values[21].ToString().Replace("\"", ""));
                                        obj.total_tax = string.IsNullOrEmpty(values[22].ToString().Replace("\"", "")) ? 0 : double.Parse(values[22].ToString().Replace("\"", ""));

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
                        success = Insert_Order_If_Not_Exist(lstTaxFile, strType, SellerId, marketplaceID, strFileName);
                    }
                    //////////////// Update Database File ///////////////////
                    success = Update_Sales_Detail(lstTaxFile, strType, SellerId, marketplaceID, addorder, strFileName);
                }
                else
                {
                    success = "Em";
                }
            }
            catch (Exception ex)
            {
                success = "Ex";
            }

            return success;
        }

        public string Insert_Order_If_Not_Exist(List<tbl_seller_tax_file> lstObj, int? strType, int? SellerId, int? marketplaceID, string strFileName)
        {
            string success = "S";
            int iEx = 0;
            int uniqueupload = 0;
            var get_order_uploaddetails = dba.tbl_order_upload.Where(a => a.tbl_seller_id == SellerId && a.filename == strFileName).FirstOrDefault();
            if (get_order_uploaddetails == null)
            {
                try
                {
                    short sourcetype = 1;
                    int current_running_no = 0;
                    var get_seller_setting = dba.tbl_seller_setting.Where(a => a.tbl_seller_id == SellerId && a.type == 3).FirstOrDefault();
                    if (get_seller_setting != null)
                    {
                        current_running_no = Convert.ToInt16(get_seller_setting.current_running_no);
                    }
                    tbl_order_upload obj_upload = new tbl_order_upload();
                    obj_upload.date_uploaded = DateTime.Now;
                    obj_upload.voucher_running_no = current_running_no;
                    obj_upload.filename = strFileName;
                    obj_upload.type = 2;//its for tax file
                    obj_upload.tbl_seller_id = SellerId;
                    obj_upload.tbl_Marketplace_id = marketplaceID;
                    obj_upload.source = sourcetype;
                    dba.tbl_order_upload.Add(obj_upload);
                    dba.SaveChanges();
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
                            objsale_order.tbl_Marketplace_Id = 3;
                            objsale_order.tbl_order_upload_id = obj_upload.id;

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
                                objInventory.tbl_item_category_id = 19;
                                objInventory.tbl_item_subcategory_id = 14;
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
                            if (get_sale_order_id.tbl_order_upload_id == obj_upload.id)
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

                    var get_balance_details = db.tbl_user_login.Where(a => a.tbl_sellers_id == SellerId).FirstOrDefault();
                    if (get_balance_details != null)
                    {
                        var last_order = get_balance_details.total_orders;
                        var diff_order = uniqueupload - last_order;
                        var plan_rate = get_balance_details.applied_plan_rate * diff_order;
                        get_balance_details.wallet_balance = get_balance_details.wallet_balance - Convert.ToDouble(plan_rate);
                        int totalOrder = Convert.ToInt16(diff_order);
                        if (get_balance_details.total_orders != null)
                            totalOrder = totalOrder + Convert.ToInt16(get_balance_details.total_orders);
                        get_balance_details.total_orders = totalOrder;
                        //var plan_rate = get_balance_details.applied_plan_rate * uniqueupload;
                        //get_balance_details.wallet_balance = get_balance_details.wallet_balance - Convert.ToDouble(plan_rate);
                        //int totalOrder = Convert.ToInt16(uniqueupload);
                        //if (get_balance_details.total_orders != null)
                        //    totalOrder = totalOrder + Convert.ToInt16(get_balance_details.total_orders);
                        //get_balance_details.total_orders = totalOrder;
                        db.Entry(get_balance_details).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    var get_upload_settlement_details = dba.tbl_order_upload.Where(a => a.tbl_seller_id == SellerId && a.id == obj_upload.id).FirstOrDefault();
                    if (get_upload_settlement_details != null)
                    {
                        get_upload_settlement_details.new_order_uploaded = uniqueupload;
                        dba.Entry(get_upload_settlement_details).State = EntityState.Modified;
                        dba.SaveChanges();
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

                }
                catch (Exception ex)
                {
                    success = "Error " + iEx.ToString() + " : " + ex.Message.ToString();
                }
            }
            else
            {
                success = "File Already Exist!";
            }
            return success;
        }

        public string Update_Sales_Detail(List<tbl_seller_tax_file> lstObj, int? strType, int? SellerId, int? marketplaceID, int? addorder, string strFileName)
        {
            string success = "S";

            int current_running_no = 0;
            short sourcetype = 1;
            int upload_id = 0;
            var get_seller_setting = dba.tbl_seller_setting.Where(a => a.tbl_seller_id == SellerId && a.type == 3).FirstOrDefault();
            if (get_seller_setting != null)
            {
                current_running_no = Convert.ToInt16(get_seller_setting.current_running_no);
            }
            var get_details = dba.tbl_order_upload.Where(a => a.tbl_seller_id == SellerId && a.filename == strFileName).FirstOrDefault();
            if (get_details == null)
            {
                tbl_order_upload obj_upload = new tbl_order_upload();
                obj_upload.date_uploaded = DateTime.Now;
                obj_upload.voucher_running_no = current_running_no;
                obj_upload.filename = strFileName;
                obj_upload.tbl_seller_id = SellerId;
                obj_upload.type = 2;//its for tax file
                obj_upload.tbl_Marketplace_id = marketplaceID;
                obj_upload.source = sourcetype;
                dba.tbl_order_upload.Add(obj_upload);
                dba.SaveChanges();
                upload_id = obj_upload.id;
            }


            // var distinctNames = (from d in lstObj select d.order_id, d.asin ).Distinct();
            var distinct_order_asin_sku = (from m in lstObj group m by new { m.order_id, m.asin, m.sku } into mygroup select mygroup.FirstOrDefault()).Distinct().ToList();
            //var Countries = query.ToList().Select(m => new tbl_seller_tax_file { order_id = m.order_id, asin = m.asin, sku = m.sku }).ToList();
            foreach (var items in distinct_order_asin_sku)
            {

                if (items.order_id == "407-4738909-9816329")
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
                            double Taxed_Jurisdiction_Tax_Rate = listitem.taxed_jurisdiction_tax_rate;
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
                                    get_sale_order_detail.shipping_price_Amount = TaxExclusive_Selling_Price;
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
                                    // end //
                                    dba.Entry(get_sale_order_detail).State = EntityState.Modified;
                                    dba.SaveChanges();
                                }
                                else
                                {
                                    double promo_total_tax = Tax_Amount + Tax_Amount_BlankRow;
                                    get_sale_order_detail.shipping_price_Amount = get_sale_order_detail.shipping_price_Amount + TaxExclusive_Selling_Price;//TaxExclusive_Selling_Price;//get_sale_order_detail.shipping_price_Amount + TaxExclusive_Selling_Price;
                                    get_sale_order_detail.shipping_tax_Amount = get_sale_order_detail.shipping_tax_Amount + promo_total_tax;//promo_total_tax;//get_sale_order_detail.shipping_tax_Amount + promo_total_tax; // Tax_Amount;
                                    get_sale_order_detail.shipping_discount_amt = get_sale_order_detail.shipping_discount_amt + Promo_TaxExclusive_Amount;
                                    get_sale_order_detail.shipping_discount_tax_amount = get_sale_order_detail.shipping_discount_tax_amount + Promo_Amount_Tax;
                                    get_sale_order_detail.tax_flag += 1;
                                    get_sale_order_detail.Ship_from_city = listitem.ship_from_city;
                                    get_sale_order_detail.ship_from_state = listitem.ship_from_state;
                                    get_sale_order_detail.ship_from_postalcode = listitem.ship_from_postal_code;
                                    get_sale_order_detail.ship_to_city = listitem.ship_to_city;
                                    get_sale_order_detail.ship_to_state = listitem.ship_to_state;
                                    get_sale_order_detail.ship_to_postalcode = listitem.ship_to_postal_code;
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
                                    get_sale_order_detail.item_price_amount = TaxExclusive_Selling_Price;
                                    get_sale_order_detail.item_tax_amount = Total_Tax_Amount; // Tax_Amount;
                                    get_sale_order_detail.tax_flag += 1;
                                    get_sale_order_detail.tax_invoiceno = listitem.tax_invoice_number;
                                    get_sale_order_detail.Ship_from_city = listitem.ship_from_city;
                                    get_sale_order_detail.ship_from_state = listitem.ship_from_state;
                                    get_sale_order_detail.ship_from_postalcode = listitem.ship_from_postal_code;
                                    get_sale_order_detail.ship_to_city = listitem.ship_to_city;
                                    get_sale_order_detail.ship_to_state = listitem.ship_to_state;
                                    get_sale_order_detail.ship_to_postalcode = listitem.ship_to_postal_code;
                                    dba.Entry(get_sale_order_detail).State = EntityState.Modified;
                                    dba.SaveChanges();

                                    //////////////////// UPDATE TAX TABLE ////////////////
                                    Update_Tax_Table(SalesOrderDetailId, Taxed_Jurisdiction_Tax_Rate, TaxType_BlankRow, Tax_Amount_BlankRow, Taxed_Jurisdiction_Tax_Rate_BlankRow, TaxType, Tax_Amount, iMultipleItem_N, seller_id);
                                }
                                else
                                {
                                    get_sale_order_detail.item_price_amount = get_sale_order_detail.item_price_amount + TaxExclusive_Selling_Price;
                                    get_sale_order_detail.item_tax_amount = get_sale_order_detail.item_tax_amount + Total_Tax_Amount; // Tax_Amount;
                                    get_sale_order_detail.tax_invoiceno = listitem.tax_invoice_number;
                                    get_sale_order_detail.Ship_from_city = listitem.ship_from_city;
                                    get_sale_order_detail.ship_from_state = listitem.ship_from_state;
                                    get_sale_order_detail.ship_from_postalcode = listitem.ship_from_postal_code;
                                    get_sale_order_detail.ship_to_city = listitem.ship_to_city;
                                    get_sale_order_detail.ship_to_state = listitem.ship_to_state;
                                    get_sale_order_detail.ship_to_postalcode = listitem.ship_to_postal_code;
                                    get_sale_order_detail.tax_flag += 1;
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
            return success;
        }

        public void Update_Tax_Table(int tbl_referneced_id, double Taxed_Jurisdiction_Tax_Rate, string TaxType_BlankRow, double Tax_Amount_BlankRow, double Taxed_Jurisdiction_Tax_Rate_BlankRow, string TaxType, double Tax_Amount, int iRepeat, int SellerId)
        {//////////////////////// TAX TABLE UPDATE /////////////////////////////
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
            }
        }


        #region Read And Save Paytm Settlement File 
        public DataTable readpaytmsettlement(string strFilePath, int marketplaceID, int id, int SellerId)
        {
            string success = "S";
            string filename = "";
            DataTable dtCsv = new DataTable();
            try
            {
                dtCsv = new DataTable();
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
                    string paymentType = columns.Contains("Payment Type") ? dtCsv.Rows[i]["Payment Type"].ToString().Replace("\"", "") : "";
                    string payoutPG = columns.Contains("Payout - PG") ? dtCsv.Rows[i]["Payout - PG"].ToString().Replace("\"", "") : "";

                    var get_settdetails = dba.tbl_paytmsettmaster
                            .FirstOrDefault(a => a.tbl_SellerId == SellerId
                            && a.OrderID == ordrid
                            && a.OrderItemID == orderitemid
                            && a.Marketplaceid == marketplaceID
                            && a.PaymentType == paymentType
                            & a.PayoutPG == payoutPG);

                    if (get_settdetails != null)
                    {
                        continue;
                    }

                    tbl_paytmsettmaster objsett = new tbl_paytmsettmaster();
                    objsett.OrderID = columns.Contains("Order ID") ? dtCsv.Rows[i]["Order ID"].ToString() : "";

                    objsett.OrderItemID = columns.Contains("Order Item ID") ? dtCsv.Rows[i]["Order Item ID"].ToString().Replace("\"", "") : "";
                    objsett.OrderCreationDate = columns.Contains("Order Creation Date") ? dtCsv.Rows[i]["Order Creation Date"].ToString().Replace("\"", "") : "";
                    objsett.ReturnDate = columns.Contains("Return Date") ? dtCsv.Rows[i]["Return Date"].ToString().Replace("\"", "") : "";
                    objsett.ProductID = columns.Contains("Product ID") ? dtCsv.Rows[i]["Product ID"].ToString().Replace("\"", "") : "";
                    objsett.ProductName = columns.Contains("Product Name") ? dtCsv.Rows[i]["Product Name"].ToString().Replace("\"", "") : "";
                    objsett.MerchantSKU = columns.Contains("Merchant SKU") ? dtCsv.Rows[i]["Merchant SKU"].ToString().Replace("\"", "") : "";
                    objsett.OrderItemStatus = columns.Contains("Order Item Status") ? dtCsv.Rows[i]["Order Item Status"].ToString().Replace("\"", "") : "";
                    objsett.SettlementDate = columns.Contains("Settlement Date") ? dtCsv.Rows[i]["Settlement Date"].ToString().Replace("\"", "") : "";

                    objsett.PaymentType = columns.Contains("Payment Type") ? dtCsv.Rows[i]["Payment Type"].ToString().Replace("\"", "") : "";
                    objsett.PaymentStatus = columns.Contains("Payment Status") ? dtCsv.Rows[i]["Payment Status"].ToString().Replace("\"", "") : "";
                    objsett.AdjustmentReason = columns.Contains("Adjustment Reason") ? dtCsv.Rows[i]["Adjustment Reason"].ToString().Replace("\"", "") : "";
                    objsett.TotalPrice = columns.Contains("Total Price") ? dtCsv.Rows[i]["Total Price"].ToString().Replace("\"", "") : "";
                    objsett.MarketplaceCommission = columns.Contains("Marketplace Commission") ? dtCsv.Rows[i]["Marketplace Commission"].ToString().Replace("\"", "") : "";
                    objsett.LogisticsCharges = columns.Contains("Logistics Charges") ? dtCsv.Rows[i]["Logistics Charges"].ToString().Replace("\"", "") : "";
                    objsett.PGCommission = columns.Contains("PG Commission") ? dtCsv.Rows[i]["PG Commission"].ToString().Replace("\"", "") : "";
                    objsett.Penalty = columns.Contains("Penalty") ? dtCsv.Rows[i]["Penalty"].ToString().Replace("\"", "") : "";
                    objsett.AdjustmentAmount = columns.Contains("Adjustment Amount") ? dtCsv.Rows[i]["Adjustment Amount"].ToString().Replace("\"", "") : "";
                    objsett.AdjustmentTaxes = columns.Contains("Adjustment Taxes") ? dtCsv.Rows[i]["Adjustment Taxes"].ToString().Replace("\"", "") : "";
                    objsett.NetAdjustments = columns.Contains("Net Adjustments") ? dtCsv.Rows[i]["Net Adjustments"].ToString().Replace("\"", "") : "";
                    objsett.ServiceTax = columns.Contains("Service Tax") ? dtCsv.Rows[i]["Service Tax"].ToString().Replace("\"", "") : "";
                    objsett.PayableAmount = columns.Contains("Payable Amount") ? dtCsv.Rows[i]["Payable Amount"].ToString().Replace("\"", "") : "";
                    objsett.PayoutWallet = columns.Contains("Payout - Wallet") ? dtCsv.Rows[i]["Payout - Wallet"].ToString().Replace("\"", "") : "";
                    objsett.PayoutPG = columns.Contains("Payout - PG") ? dtCsv.Rows[i]["Payout - PG"].ToString().Replace("\"", "") : "";
                    objsett.PayoutCOD = columns.Contains("Payout - COD") ? dtCsv.Rows[i]["Payout - COD"].ToString().Replace("\"", "") : "";
                    objsett.WalletUTR = columns.Contains("Wallet UT") ? dtCsv.Rows[i]["Wallet UTR"].ToString().Replace("\"", "") : "";
                    objsett.CODUTR = columns.Contains("COD UTR") ? dtCsv.Rows[i]["COD UTR"].ToString().Replace("\"", "") : "";

                    objsett.PGUTR = columns.Contains("PG UTR") ?
                        string.IsNullOrEmpty(dtCsv.Rows[i]["PG UTR"].ToString().Replace("\"", ""))
                        ? string.IsNullOrEmpty(objsett.CODUTR) ? "NA" : objsett.CODUTR
                        : dtCsv.Rows[i]["PG UTR"].ToString().Replace("\"", "")
                        : "NA";

                    objsett.OperatorReferenceNumber = columns.Contains("Operator Reference Number") ? dtCsv.Rows[i]["Operator Reference Number"].ToString().Replace("\"", "") : "";

                    objsett.mp_commission_cgst = columns.Contains("mp_commission_cgst") ? dtCsv.Rows[i]["mp_commission_cgst"].ToString().Replace("\"", "") : "";
                    objsett.mp_commission_igst = columns.Contains("mp_commission_igst") ? dtCsv.Rows[i]["mp_commission_igst"].ToString().Replace("\"", "") : "";
                    objsett.mp_commission_sgst = columns.Contains("mp_commission_sgst") ? dtCsv.Rows[i]["mp_commission_sgst"].ToString().Replace("\"", "") : "";
                    objsett.pg_commission_cgst = columns.Contains("pg_commission_cgst") ? dtCsv.Rows[i]["pg_commission_cgst"].ToString().Replace("\"", "") : "";
                    objsett.pg_commission_igst = columns.Contains("pg_commission_igst") ? dtCsv.Rows[i]["pg_commission_igst"].ToString().Replace("\"", "") : "";
                    objsett.pg_commission_sgst = columns.Contains("pg_commission_sgst") ? dtCsv.Rows[i]["pg_commission_sgst"].ToString().Replace("\"", "") : "";
                    objsett.logistics_cgst = columns.Contains("logistics_cgst") ? dtCsv.Rows[i]["logistics_cgst"].ToString().Replace("\"", "") : "";
                    objsett.logistics_igst = columns.Contains("logistics_igst") ? dtCsv.Rows[i]["logistics_igst"].ToString().Replace("\"", "") : "";
                    objsett.logistics_sgst = columns.Contains("logistics_sgst") ? dtCsv.Rows[i]["logistics_sgst"].ToString().Replace("\"", "") : "";
                    objsett.Customercompanyname = columns.Contains("Customer company name") ? dtCsv.Rows[i]["Customer company name"].ToString().Replace("\"", "") : "";
                    objsett.Customerbillingaddress = columns.Contains("Customer billing address") ? dtCsv.Rows[i]["Customer billing address"].ToString().Replace("\"", "") : "";
                    objsett.CustomerGSTIN = columns.Contains("Customer GSTIN") ? dtCsv.Rows[i]["Customer GSTIN"].ToString().Replace("\"", "") : "";
                    objsett.igst = columns.Contains("igst") ? dtCsv.Rows[i]["igst"].ToString().Replace("\"", "") : "";
                    objsett.cgst = columns.Contains("cgst") ? dtCsv.Rows[i]["cgst"].ToString().Replace("\"", "") : "";
                    objsett.sgst = columns.Contains("sgst") ? dtCsv.Rows[i]["sgst"].ToString().Replace("\"", "") : "";
                    objsett.gst_source_state = columns.Contains("gst_source_state") ? dtCsv.Rows[i]["gst_source_state"].ToString().Replace("\"", "") : "";
                    objsett.gst_destination_state = columns.Contains("gst_destination_state") ? dtCsv.Rows[i]["gst_destination_state"].ToString().Replace("\"", "") : "";
                    objsett.gst_source_pincode = columns.Contains("gst_source_pincode") ? dtCsv.Rows[i]["gst_source_pincode"].ToString().Replace("\"", "") : "";
                    objsett.gst_destination_pincode = columns.Contains("gst_destination_pincode") ? dtCsv.Rows[i]["gst_destination_pincode"].ToString().Replace("\"", "") : "";
                    objsett.InvoiceGenerationDate = columns.Contains("Invoice Generation Date") ? dtCsv.Rows[i]["Invoice Generation Date"].ToString().Replace("\"", "") : "";
                    objsett.InvoiceNumber = columns.Contains("Invoice Number") ? dtCsv.Rows[i]["Invoice Number"].ToString().Replace("\"", "") : "";

                    objsett.Uploadid = id;
                    objsett.Marketplaceid = marketplaceID;
                    objsett.Status = 0;
                    objsett.tbl_SellerId = SellerId;
                    dba.tbl_paytmsettmaster.Add(objsett);
                    dba.SaveChanges();

                }
                
                uploadPayTmSettlements(marketplaceID, id, SellerId);
            }
            catch (Exception ex)
            {
            }
            return dtCsv;
            //return success;
        }

        public void uploadPayTmSettlements1(int marketplaceID, int uploadId, int SellerId)
        {

            var temp_allSettlements = dba.tbl_paytmsettmaster.Where(a => a.Marketplaceid == marketplaceID &&
                a.tbl_SellerId == SellerId && a.Uploadid == uploadId && a.Status == 0).OrderBy(x => x.PGUTR).ToList();

            try
            {
                if (temp_allSettlements == null)
                {
                    return;
                }
                var distinctSettlementsByRefNumber = temp_allSettlements.GroupBy(x => x.PGUTR).Select(x => x.FirstOrDefault()).ToList();

                var settlementFees = dba.m_settlement_fee.Where(x => x.return_fee == "Marketplace Commission" || x.return_fee == "Logistics Charges" ||
                x.return_fee == "PG Commission" || x.return_fee == "Penalty" || x.return_fee == "Net Adjustments").ToList();

                var get_seller_setting = dba.tbl_seller_setting.Where(a => a.tbl_seller_id == SellerId && a.type == 2).FirstOrDefault();


                var currentRunningNumber = 0;

                var get_upload = dba.tbl_settlement_upload.Where(a => a.tbl_seller_id == SellerId && a.Id == uploadId &&
                       a.settlement_refernece_no == null &&
                       a.market_place_id == marketplaceID).FirstOrDefault();

                currentRunningNumber = (int)get_upload?.voucher_running_no;

                foreach (var distinctSettlement in distinctSettlementsByRefNumber)
                {


                    var updatedSettlementUploadId = uploadId;

                    var allSettlementsByRefId = temp_allSettlements.Where(x => x.PGUTR == distinctSettlement.PGUTR).ToList();



                    #region INSERT/UPDATE INTO settlement_upload TABLE
                    //if (get_upload != null)
                    //{
                    //    // update
                    //    get_upload.settlement_refernece_no = distinctSettlement.PGUTR;

                    //    get_upload.settlement_from = Convert.ToDateTime(allSettlementsByRefId.Min(x => x.OrderCreationDate));
                    //    get_upload.settlement_to = Convert.ToDateTime(allSettlementsByRefId.Max(x => x.OrderCreationDate));
                    //    get_upload.deposit_date = Convert.ToDateTime(allSettlementsByRefId.Max(x => x.SettlementDate));
                    //    get_upload.processing_datetime = DateTime.Now;
                    //    get_upload.Source = 2;
                    //    get_upload.settlement_type = 0;
                    //    get_upload.status = 0;
                    //    dba.Entry(get_upload).State = EntityState.Modified;
                    //    dba.SaveChanges();

                    //    currentRunningNumber = (int)get_upload.voucher_running_no;
                    //}
                    //else
                    //{
                    //new record entry
                    var orderSettlementUpload = new tbl_settlement_upload
                    {
                        tbl_seller_id = distinctSettlement.tbl_SellerId,
                        market_place_id = distinctSettlement.Marketplaceid,
                        settlement_refernece_no = distinctSettlement.PGUTR,
                        file_name = get_upload.file_name,
                        uploaded_on = DateTime.Now,
                        settlement_type = 0,
                        Source = 2,
                        settlement_from = Convert.ToDateTime(allSettlementsByRefId.Min(x => x.OrderCreationDate)),
                        settlement_to = Convert.ToDateTime(allSettlementsByRefId.Max(x => x.OrderCreationDate)),
                        uploaded_by = distinctSettlement.tbl_SellerId,
                        deposit_date = Convert.ToDateTime(allSettlementsByRefId.Max(x => x.SettlementDate)),
                        voucher_running_no = currentRunningNumber,
                        file_status = "Processing",
                        processing_datetime = DateTime.Now,
                        status = 0
                    };

                    dba.tbl_settlement_upload.Add(orderSettlementUpload);
                    dba.SaveChanges();

                    updatedSettlementUploadId = orderSettlementUpload.Id;
                    //}
                    #endregion

                    #region INSERT INTO BANK TRANSFER

                    var bankTransfer = new tbl_imp_bank_transfers
                    {
                        tbl_settlement_upload_id = updatedSettlementUploadId,
                        amount = allSettlementsByRefId.Sum(x => Convert.ToDecimal(x.PayableAmount))
                    };

                    dba.tbl_imp_bank_transfers.Add(bankTransfer);
                    dba.SaveChanges();

                    #endregion



                    foreach (var settlementItem in allSettlementsByRefId)
                    {

                        #region INSERT INTO SETTLEMENT ORDERS

                        if (settlementItem.PaymentType.ToLower().Trim() == "credit")
                        {
                            #region SAVE SETTLEMENT ORDER
                            var settlementOrder = new tbl_settlement_order
                            {
                                Order_Id = settlementItem.OrderID,
                                unique_order_id = settlementItem.OrderID + "_" + settlementItem.OrderItemID,
                                settlement_id = settlementItem.PGUTR,
                                principal_price = Convert.ToDouble(settlementItem.TotalPrice),
                                posted_date = Convert.ToDateTime(settlementItem.OrderCreationDate),
                                //TODO: This will update with original sku number. right now we =don't have this.
                                Sku_no = settlementItem.ProductID,

                                quantity = 1,
                                tbl_seller_id = settlementItem.tbl_SellerId,
                                created_on = DateTime.Now,
                                LastUpdatedDateUTC = DateTime.Now,
                            };

                            dba.tbl_settlement_order.Add(settlementOrder);
                            dba.SaveChanges();
                            #endregion

                            #region INSERT INTO EXPENSE TABLE AND TAX

                            foreach (var fee in settlementFees)
                            {
                                var orderExpences = new m_tbl_expense
                                {
                                    date_created = DateTime.Now,

                                    reference_number = settlementItem.PGUTR,
                                    tbl_seller_id = settlementItem.tbl_SellerId,
                                    settlement_datetime = Convert.ToDateTime(settlementItem.SettlementDate),
                                    Original_order_id = settlementItem.OrderID,
                                    settlement_order_id = settlementItem.OrderID + "_" + settlementItem.OrderItemID,
                                    sku_no = settlementItem.ProductID,
                                    quantity_purchased = 1,
                                    t_transactionType_id = 1,// 1 for order, 2 for refund
                                    tbl_order_historyid = null
                                };

                                var orderTax = new tbl_tax
                                {
                                    isactive = 1,
                                    tbl_seller_id = (int)settlementItem.tbl_SellerId,
                                    reference_type = 2// 2 for order 7 for refund
                                };

                                if (fee.return_fee == "Marketplace Commission" && !string.IsNullOrEmpty(settlementItem.MarketplaceCommission)
                                    && Convert.ToDouble(settlementItem.MarketplaceCommission) != 0)
                                {
                                    orderExpences.expense_type_id = fee.id;
                                    orderExpences.expense_amount = -Convert.ToDouble(settlementItem.MarketplaceCommission);

                                    orderTax.CGST_amount = -Convert.ToDouble(settlementItem.mp_commission_cgst);
                                    orderTax.Igst_amount = -Convert.ToDouble(settlementItem.mp_commission_igst);
                                    orderTax.sgst_amount = -Convert.ToDouble(settlementItem.mp_commission_sgst);

                                    dba.m_tbl_expense.Add(orderExpences);
                                    dba.SaveChanges();


                                    orderTax.tbl_referneced_id = orderExpences.id;
                                    dba.tbl_tax.Add(orderTax);
                                    dba.SaveChanges();
                                }
                                else if (fee.return_fee == "Logistics Charges" && !string.IsNullOrEmpty(settlementItem.LogisticsCharges)
                                    && Convert.ToDouble(settlementItem.LogisticsCharges) != 0)
                                {
                                    orderExpences.expense_type_id = fee.id;
                                    orderExpences.expense_amount = -Convert.ToDouble(settlementItem.LogisticsCharges);

                                    orderTax.CGST_amount = -Convert.ToDouble(settlementItem.logistics_cgst);
                                    orderTax.Igst_amount = -Convert.ToDouble(settlementItem.logistics_igst);
                                    orderTax.sgst_amount = -Convert.ToDouble(settlementItem.logistics_sgst);

                                    dba.m_tbl_expense.Add(orderExpences);
                                    dba.SaveChanges();


                                    orderTax.tbl_referneced_id = orderExpences.id;
                                    dba.tbl_tax.Add(orderTax);
                                    dba.SaveChanges();
                                }
                                else if (fee.return_fee == "PG Commission" && !string.IsNullOrEmpty(settlementItem.PGCommission)
                                    && Convert.ToDouble(settlementItem.PGCommission) != 0)
                                {
                                    orderExpences.expense_type_id = fee.id;
                                    orderExpences.expense_amount = -Convert.ToDouble(settlementItem.PGCommission);

                                    orderTax.CGST_amount = -Convert.ToDouble(settlementItem.pg_commission_cgst);
                                    orderTax.Igst_amount = -Convert.ToDouble(settlementItem.pg_commission_igst);
                                    orderTax.sgst_amount = -Convert.ToDouble(settlementItem.pg_commission_sgst);

                                    dba.m_tbl_expense.Add(orderExpences);
                                    dba.SaveChanges();


                                    orderTax.tbl_referneced_id = orderExpences.id;
                                    dba.tbl_tax.Add(orderTax);
                                    dba.SaveChanges();
                                }
                                else if (fee.return_fee == "Penalty" && !string.IsNullOrEmpty(settlementItem.Penalty)
                                    && Convert.ToDouble(settlementItem.Penalty) != 0)
                                {
                                    orderExpences.expense_type_id = fee.id;
                                    orderExpences.expense_amount = Convert.ToDouble(settlementItem.Penalty);

                                    orderTax.CGST_amount = 0;
                                    orderTax.Igst_amount = Math.Abs((Convert.ToDouble(settlementItem.Penalty) * 18) / 100);

                                    orderTax.sgst_amount = 0;

                                    dba.m_tbl_expense.Add(orderExpences);
                                    dba.SaveChanges();


                                    orderTax.tbl_referneced_id = orderExpences.id;
                                    dba.tbl_tax.Add(orderTax);
                                    dba.SaveChanges();
                                }
                                else if (fee.return_fee == "Net Adjustments" && !string.IsNullOrEmpty(settlementItem.NetAdjustments)
                                    && Convert.ToDouble(settlementItem.NetAdjustments) != 0)
                                {
                                    orderExpences.expense_type_id = fee.id;
                                    orderExpences.expense_amount = -Convert.ToDouble(settlementItem.NetAdjustments);

                                    //For Net Adjustents all tax is 0
                                    orderTax.CGST_amount = 0;
                                    orderTax.Igst_amount = 0;
                                    orderTax.sgst_amount = 0;

                                    dba.m_tbl_expense.Add(orderExpences);
                                    dba.SaveChanges();


                                    orderTax.tbl_referneced_id = orderExpences.id;
                                    dba.tbl_tax.Add(orderTax);
                                    dba.SaveChanges();
                                }

                            }

                            //SaveOrderExpencesAndTaxes(settlementFees, orderExpences, orderTax, settlementItem, true);
                            #endregion
                        }
                        else // DEBIT
                        {
                            #region SAVE REFUND
                            var settlementRefund = new tbl_order_history
                            {
                                tbl_seller_id = settlementItem.tbl_SellerId,
                                updated_by = settlementItem.tbl_SellerId,
                                created_on = DateTime.Now,
                                tbl_marketplace_id = marketplaceID,
                                SKU = settlementItem.ProductID,
                                Quantity = 1,
                                OrderID = settlementItem.OrderID,
                                unique_order_id = settlementItem.OrderID + "_" + settlementItem.OrderItemID,
                                t_order_status = 9,
                                ShipmentDate = Convert.ToDateTime(settlementItem.OrderCreationDate),
                                amount_per_unit = -Convert.ToDouble(settlementItem.TotalPrice),
                                settlement_id = settlementItem.PGUTR,
                                LastUpdatedDateUTC = DateTime.Now
                            };

                            dba.tbl_order_history.Add(settlementRefund);
                            dba.SaveChanges();
                            #endregion

                            #region INSERT INTO EXPENSE TABLE AND TAX

                            foreach (var fee in settlementFees)
                            {

                                var orderExpences = new m_tbl_expense
                                {
                                    date_created = DateTime.Now,

                                    reference_number = settlementItem.PGUTR,
                                    tbl_seller_id = settlementItem.tbl_SellerId,
                                    settlement_datetime = Convert.ToDateTime(settlementItem.SettlementDate),
                                    Original_order_id = settlementItem.OrderID,
                                    settlement_order_id = settlementItem.OrderID + "_" + settlementItem.OrderItemID,
                                    sku_no = settlementItem.ProductID,
                                    quantity_purchased = 1,
                                    t_transactionType_id = 2,// 1 for order, 2 for refund
                                    tbl_order_historyid = settlementRefund.Id
                                };

                                var orderTax = new tbl_tax
                                {
                                    isactive = 1,
                                    tbl_seller_id = (int)settlementItem.tbl_SellerId,
                                    reference_type = 7// 2 for order 7 for refund
                                };

                                if (fee.return_fee == "Marketplace Commission" && !string.IsNullOrEmpty(settlementItem.MarketplaceCommission)
                                    && Convert.ToDouble(settlementItem.MarketplaceCommission) != 0)
                                {
                                    orderExpences.expense_type_id = fee.id;
                                    orderExpences.expense_amount = -Convert.ToDouble(settlementItem.MarketplaceCommission);

                                    orderTax.CGST_amount = -Convert.ToDouble(settlementItem.mp_commission_cgst);
                                    orderTax.Igst_amount = -Convert.ToDouble(settlementItem.mp_commission_igst);
                                    orderTax.sgst_amount = -Convert.ToDouble(settlementItem.mp_commission_sgst);

                                    dba.m_tbl_expense.Add(orderExpences);
                                    dba.SaveChanges();


                                    orderTax.tbl_referneced_id = orderExpences.id;
                                    dba.tbl_tax.Add(orderTax);
                                    dba.SaveChanges();
                                }
                                else if (fee.return_fee == "Logistics Charges" && !string.IsNullOrEmpty(settlementItem.LogisticsCharges)
                                    && Convert.ToDouble(settlementItem.LogisticsCharges) != 0)
                                {
                                    orderExpences.expense_type_id = fee.id;
                                    orderExpences.expense_amount = -Convert.ToDouble(settlementItem.LogisticsCharges);

                                    orderTax.CGST_amount = -Convert.ToDouble(settlementItem.logistics_cgst);
                                    orderTax.Igst_amount = -Convert.ToDouble(settlementItem.logistics_igst);
                                    orderTax.sgst_amount = -Convert.ToDouble(settlementItem.logistics_sgst);

                                    dba.m_tbl_expense.Add(orderExpences);
                                    dba.SaveChanges();


                                    orderTax.tbl_referneced_id = orderExpences.id;
                                    dba.tbl_tax.Add(orderTax);
                                    dba.SaveChanges();
                                }
                                else if (fee.return_fee == "PG Commission" && !string.IsNullOrEmpty(settlementItem.PGCommission)
                                    && Convert.ToDouble(settlementItem.PGCommission) != 0)
                                {
                                    orderExpences.expense_type_id = fee.id;
                                    orderExpences.expense_amount = -Convert.ToDouble(settlementItem.PGCommission);

                                    orderTax.CGST_amount = -Convert.ToDouble(settlementItem.pg_commission_cgst);
                                    orderTax.Igst_amount = -Convert.ToDouble(settlementItem.pg_commission_igst);
                                    orderTax.sgst_amount = -Convert.ToDouble(settlementItem.pg_commission_sgst);

                                    dba.m_tbl_expense.Add(orderExpences);
                                    dba.SaveChanges();


                                    orderTax.tbl_referneced_id = orderExpences.id;
                                    dba.tbl_tax.Add(orderTax);
                                    dba.SaveChanges();
                                }
                                else if (fee.return_fee == "Penalty" && !string.IsNullOrEmpty(settlementItem.Penalty)
                                    && Convert.ToDouble(settlementItem.Penalty) != 0)
                                {
                                    orderExpences.expense_type_id = fee.id;
                                    orderExpences.expense_amount = Convert.ToDouble(settlementItem.Penalty);


                                    orderTax.CGST_amount = 0;
                                    orderTax.Igst_amount = Convert.ToDouble(settlementItem.Penalty) * 18 / 100;
                                    orderTax.sgst_amount = 0;

                                    dba.m_tbl_expense.Add(orderExpences);
                                    dba.SaveChanges();


                                    orderTax.tbl_referneced_id = orderExpences.id;
                                    dba.tbl_tax.Add(orderTax);
                                    dba.SaveChanges();
                                }
                                else if (fee.return_fee == "Net Adjustments" && !string.IsNullOrEmpty(settlementItem.NetAdjustments)
                                    && Convert.ToDouble(settlementItem.NetAdjustments) != 0)
                                {
                                    orderExpences.expense_type_id = fee.id;
                                    orderExpences.expense_amount = -Convert.ToDouble(settlementItem.NetAdjustments);


                                    //For Net Adjustents all tax is 0
                                    orderTax.CGST_amount = 0;
                                    orderTax.Igst_amount = 0;
                                    orderTax.sgst_amount = 0;

                                    dba.m_tbl_expense.Add(orderExpences);
                                    dba.SaveChanges();

                                    orderTax.tbl_referneced_id = orderExpences.id;
                                    dba.tbl_tax.Add(orderTax);
                                    dba.SaveChanges();
                                }

                            }

                            //SaveOrderExpencesAndTaxes(settlementFees, orderExpences, orderTax, settlementItem, false);
                            #endregion
                        }

                        #endregion
                    }


                    var settlement_upload = dba.tbl_settlement_upload.Where(a => a.tbl_seller_id == SellerId &&
                    a.settlement_refernece_no == distinctSettlement.PGUTR &&
                    a.market_place_id == marketplaceID).FirstOrDefault();
                    if (settlement_upload != null)
                    {
                        settlement_upload.completed_datetime = DateTime.Now;
                        settlement_upload.file_status = "Complete";
                        settlement_upload.new_order_uploaded = allSettlementsByRefId.Count();
                        settlement_upload.LastUpdatedDateUTC = DateTime.Now;

                        dba.Entry(settlement_upload).State = EntityState.Modified;
                        dba.SaveChanges();
                    }

                    currentRunningNumber = currentRunningNumber + 1;

                }
                dba.Entry(get_upload).State = EntityState.Deleted;
                if (temp_allSettlements != null)
                {
                    foreach (var dd in temp_allSettlements)
                    {
                        dd.Status = 1;
                        dba.Entry(dd).State = EntityState.Modified;

                    }
                }
                if(get_seller_setting != null)
                {
                    if (currentRunningNumber != 0)
                    {
                        get_seller_setting.current_running_no = currentRunningNumber;
                        dba.Entry(get_seller_setting).State = EntityState.Modified;
                        dba.SaveChanges();
                    }
                }
                dba.SaveChanges();


            }
            catch
            {
            }

        }
        public void uploadPayTmSettlements(int marketplaceID, int uploadId, int SellerId)
        {

            var temp_allSettlements = dba.tbl_paytmsettmaster.Where(a => a.Marketplaceid == marketplaceID &&
                a.tbl_SellerId == SellerId && a.Uploadid == uploadId && a.Status == 0).OrderBy(x => x.PGUTR).ToList();

            try
            {
                if (temp_allSettlements == null)
                {
                    return;
                }
                var distinctSettlementsByRefNumber = temp_allSettlements.GroupBy(x => x.PGUTR).Select(x => x.FirstOrDefault()).ToList();

                var settlementFees = dba.m_settlement_fee.Where(x => x.return_fee == "Marketplace Commission" || x.return_fee == "Logistics Charges" ||
                x.return_fee == "PG Commission" || x.return_fee == "Penalty" || x.return_fee == "Net Adjustments").ToList();

                var get_seller_setting = dba.tbl_seller_setting.Where(a => a.tbl_seller_id == SellerId && a.type == 2).FirstOrDefault();


                var currentRunningNumber = 0;

                var get_upload = dba.tbl_settlement_upload.Where(a => a.tbl_seller_id == SellerId && a.Id == uploadId &&
                       a.settlement_refernece_no == null &&
                       a.market_place_id == marketplaceID).FirstOrDefault();

                currentRunningNumber = (int)get_upload?.voucher_running_no;

                foreach (var distinctSettlement in distinctSettlementsByRefNumber)
                {


                    var updatedSettlementUploadId = uploadId;

                    var allSettlementsByRefId = temp_allSettlements.Where(x => x.PGUTR == distinctSettlement.PGUTR).ToList();



                    #region INSERT/UPDATE INTO settlement_upload TABLE

                    //new record entry
                    var orderSettlementUpload = new tbl_settlement_upload
                    {
                        tbl_seller_id = distinctSettlement.tbl_SellerId,
                        market_place_id = distinctSettlement.Marketplaceid,
                        settlement_refernece_no = distinctSettlement.PGUTR,
                        file_name = get_upload.file_name,
                        uploaded_on = DateTime.Now,
                        settlement_type = 0,
                        Source = 2,
                        uploaded_by = distinctSettlement.tbl_SellerId,
                        voucher_running_no = currentRunningNumber,
                        file_status = "Processing",
                        processing_datetime = DateTime.Now,
                        status = 0
                    };

                    var settlement_from = allSettlementsByRefId.Min(x => x.OrderCreationDate);
                    if (settlement_from.Contains('-') && settlement_from.Contains(':'))
                    {
                        orderSettlementUpload.settlement_from = DateTime.ParseExact(settlement_from, "dd-MM-yyyy HH:mm", CultureInfo.InvariantCulture);
                    }
                    else if (settlement_from.Contains('-'))
                    {
                        orderSettlementUpload.settlement_from = DateTime.ParseExact(settlement_from, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        DateTime outDate;
                        DateTime.TryParse(settlement_from, out outDate);
                        orderSettlementUpload.settlement_from = outDate;
                    }

                    var settlement_to = allSettlementsByRefId.Max(x => x.OrderCreationDate);
                    if (settlement_to.Contains('-') && settlement_to.Contains(':'))
                    {
                        orderSettlementUpload.settlement_to = DateTime.ParseExact(settlement_to, "dd-MM-yyyy HH:mm", CultureInfo.InvariantCulture);
                    }
                    else if (settlement_to.Contains('-'))
                    {
                        orderSettlementUpload.settlement_to = DateTime.ParseExact(settlement_to, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        DateTime outDate;
                        DateTime.TryParse(settlement_to, out outDate);
                        orderSettlementUpload.settlement_to = outDate;
                    }


                    var settlementDate = allSettlementsByRefId.Max(x => x.SettlementDate);
                    if (settlementDate.Contains('-') && settlementDate.Contains(':'))
                    {
                        orderSettlementUpload.deposit_date = DateTime.ParseExact(settlementDate, "dd-MM-yyyy HH:mm", CultureInfo.InvariantCulture);
                    }
                    else if (settlementDate.Contains('-'))
                    {
                        orderSettlementUpload.deposit_date = DateTime.ParseExact(settlementDate, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        DateTime outDate;
                        DateTime.TryParse(settlementDate, out outDate);
                        orderSettlementUpload.deposit_date = outDate;
                    }


                    dba.tbl_settlement_upload.Add(orderSettlementUpload);
                    dba.SaveChanges();

                    updatedSettlementUploadId = orderSettlementUpload.Id;
                    //}
                    #endregion

                    #region INSERT INTO BANK TRANSFER

                    var bankTransfer = new tbl_imp_bank_transfers
                    {
                        tbl_settlement_upload_id = updatedSettlementUploadId,
                        amount = allSettlementsByRefId.Sum(x => Convert.ToDecimal(x.PayableAmount))
                    };

                    dba.tbl_imp_bank_transfers.Add(bankTransfer);
                    dba.SaveChanges();

                    #endregion



                    foreach (var settlementItem in allSettlementsByRefId)
                    {

                        #region INSERT INTO SETTLEMENT ORDERS

                        if (settlementItem.PaymentType.ToLower().Trim() == "credit")
                        {
                            #region SAVE SETTLEMENT ORDER
                            var settlementOrder = new tbl_settlement_order
                            {
                                Order_Id = settlementItem.OrderID,
                                unique_order_id = settlementItem.OrderID + "_" + settlementItem.OrderItemID,
                                settlement_id = settlementItem.PGUTR,
                                principal_price = Convert.ToDouble(settlementItem.TotalPrice),
                                //TODO: This will update with original sku number. right now we =don't have this.
                                Sku_no = settlementItem.MerchantSKU,//settlementItem.ProductID,

                                quantity = 1,
                                tbl_seller_id = settlementItem.tbl_SellerId,
                                created_on = DateTime.Now,
                                LastUpdatedDateUTC = DateTime.Now,
                            };

                            if (settlementItem.OrderCreationDate.Contains('-') && settlementItem.OrderCreationDate.Contains(':'))
                            {
                                settlementOrder.posted_date = DateTime.ParseExact(settlementItem.OrderCreationDate, "dd-MM-yyyy HH:mm", CultureInfo.InvariantCulture);
                            }
                            else if (settlementItem.OrderCreationDate.Contains('-'))
                            {
                                settlementOrder.posted_date = DateTime.ParseExact(settlementItem.OrderCreationDate, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                            }
                            else
                            {
                                DateTime outDate;
                                DateTime.TryParse(settlementItem.OrderCreationDate, out outDate);
                                settlementOrder.posted_date = outDate;
                            }

                            dba.tbl_settlement_order.Add(settlementOrder);
                            dba.SaveChanges();
                            #endregion

                            #region INSERT INTO EXPENSE TABLE AND TAX

                            foreach (var fee in settlementFees)
                            {
                                var orderExpences = new m_tbl_expense
                                {
                                    date_created = DateTime.Now,

                                    reference_number = settlementItem.PGUTR,
                                    tbl_seller_id = settlementItem.tbl_SellerId,
                                    Original_order_id = settlementItem.OrderID,
                                    settlement_order_id = settlementItem.OrderID + "_" + settlementItem.OrderItemID,
                                    sku_no = settlementItem.MerchantSKU, //settlementItem.ProductID,
                                    quantity_purchased = 1,
                                    t_transactionType_id = 1,// 1 for order, 2 for refund
                                    tbl_order_historyid = null
                                };

                                if (settlementItem.SettlementDate.Contains('-') && settlementItem.SettlementDate.Contains(':'))
                                {
                                    orderExpences.settlement_datetime = DateTime.ParseExact(settlementItem.SettlementDate, "dd-MM-yyyy HH:mm", CultureInfo.InvariantCulture);
                                }
                                else if (settlementItem.SettlementDate.Contains('-'))
                                {
                                    orderExpences.settlement_datetime = DateTime.ParseExact(settlementItem.SettlementDate, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                                }
                                else
                                {
                                    DateTime outDate;
                                    DateTime.TryParse(settlementItem.SettlementDate, out outDate);
                                    orderExpences.settlement_datetime = outDate;
                                }

                                var orderTax = new tbl_tax
                                {
                                    isactive = 1,
                                    tbl_seller_id = (int)settlementItem.tbl_SellerId,
                                    reference_type = 2// 2 for order 7 for refund
                                };

                                if (fee.return_fee == "Marketplace Commission" && !string.IsNullOrEmpty(settlementItem.MarketplaceCommission)
                                    && Convert.ToDouble(settlementItem.MarketplaceCommission) != 0)
                                {
                                    orderExpences.expense_type_id = fee.id;
                                    orderExpences.expense_amount = -Convert.ToDouble(settlementItem.MarketplaceCommission);

                                    orderTax.CGST_amount = -Convert.ToDouble(settlementItem.mp_commission_cgst);
                                    orderTax.Igst_amount = -Convert.ToDouble(settlementItem.mp_commission_igst);
                                    orderTax.sgst_amount = -Convert.ToDouble(settlementItem.mp_commission_sgst);

                                    dba.m_tbl_expense.Add(orderExpences);
                                    dba.SaveChanges();


                                    orderTax.tbl_referneced_id = orderExpences.id;
                                    dba.tbl_tax.Add(orderTax);
                                    dba.SaveChanges();
                                }
                                else if (fee.return_fee == "Logistics Charges" && !string.IsNullOrEmpty(settlementItem.LogisticsCharges)
                                    && Convert.ToDouble(settlementItem.LogisticsCharges) != 0)
                                {
                                    orderExpences.expense_type_id = fee.id;
                                    orderExpences.expense_amount = -Convert.ToDouble(settlementItem.LogisticsCharges);

                                    orderTax.CGST_amount = -Convert.ToDouble(settlementItem.logistics_cgst);
                                    orderTax.Igst_amount = -Convert.ToDouble(settlementItem.logistics_igst);
                                    orderTax.sgst_amount = -Convert.ToDouble(settlementItem.logistics_sgst);

                                    dba.m_tbl_expense.Add(orderExpences);
                                    dba.SaveChanges();


                                    orderTax.tbl_referneced_id = orderExpences.id;
                                    dba.tbl_tax.Add(orderTax);
                                    dba.SaveChanges();
                                }
                                else if (fee.return_fee == "PG Commission" && !string.IsNullOrEmpty(settlementItem.PGCommission)
                                    && Convert.ToDouble(settlementItem.PGCommission) != 0)
                                {
                                    orderExpences.expense_type_id = fee.id;
                                    orderExpences.expense_amount = -Convert.ToDouble(settlementItem.PGCommission);

                                    orderTax.CGST_amount = -Convert.ToDouble(settlementItem.pg_commission_cgst);
                                    orderTax.Igst_amount = -Convert.ToDouble(settlementItem.pg_commission_igst);
                                    orderTax.sgst_amount = -Convert.ToDouble(settlementItem.pg_commission_sgst);

                                    dba.m_tbl_expense.Add(orderExpences);
                                    dba.SaveChanges();


                                    orderTax.tbl_referneced_id = orderExpences.id;
                                    dba.tbl_tax.Add(orderTax);
                                    dba.SaveChanges();
                                }
                                else if (fee.return_fee == "Penalty" && !string.IsNullOrEmpty(settlementItem.Penalty)
                                    && Convert.ToDouble(settlementItem.Penalty) != 0)
                                {
                                    orderExpences.expense_type_id = fee.id;
                                    orderExpences.expense_amount = Convert.ToDouble(settlementItem.Penalty);

                                    orderTax.CGST_amount = 0;
                                    orderTax.Igst_amount = Math.Abs((Convert.ToDouble(settlementItem.Penalty) * 18) / 100);

                                    orderTax.sgst_amount = 0;

                                    dba.m_tbl_expense.Add(orderExpences);
                                    dba.SaveChanges();


                                    orderTax.tbl_referneced_id = orderExpences.id;
                                    dba.tbl_tax.Add(orderTax);
                                    dba.SaveChanges();
                                }
                                else if (fee.return_fee == "Net Adjustments" && !string.IsNullOrEmpty(settlementItem.NetAdjustments)
                                    && Convert.ToDouble(settlementItem.NetAdjustments) != 0)
                                {
                                    orderExpences.expense_type_id = fee.id;
                                    orderExpences.expense_amount = -Convert.ToDouble(settlementItem.NetAdjustments);

                                    //For Net Adjustents all tax is 0
                                    orderTax.CGST_amount = 0;
                                    orderTax.Igst_amount = 0;
                                    orderTax.sgst_amount = 0;

                                    dba.m_tbl_expense.Add(orderExpences);
                                    dba.SaveChanges();


                                    orderTax.tbl_referneced_id = orderExpences.id;
                                    dba.tbl_tax.Add(orderTax);
                                    dba.SaveChanges();
                                }

                            }

                            //SaveOrderExpencesAndTaxes(settlementFees, orderExpences, orderTax, settlementItem, true);
                            #endregion
                        }
                        else // DEBIT
                        {
                            #region SAVE REFUND
                            //int history_id = 0;
                            var get_historydetails = dba.tbl_order_history.Where(a => a.OrderID == settlementItem.OrderID && a.SKU == settlementItem.MerchantSKU && a.tbl_seller_id == settlementItem.tbl_SellerId && a.t_order_status==9).FirstOrDefault();
                            //if (get_historydetails == null)
                            //{
                                var settlementRefund = new tbl_order_history
                                {
                                    tbl_seller_id = settlementItem.tbl_SellerId,
                                    updated_by = settlementItem.tbl_SellerId,
                                    created_on = DateTime.Now,
                                    tbl_marketplace_id = marketplaceID,
                                    SKU = settlementItem.MerchantSKU,// settlementItem.ProductID,
                                    Quantity = 1,
                                    OrderID = settlementItem.OrderID,
                                    unique_order_id = settlementItem.OrderID + "_" + settlementItem.OrderItemID,
                                    t_order_status = 9,
                                    amount_per_unit = -Convert.ToDouble(settlementItem.TotalPrice),
                                    settlement_id = settlementItem.PGUTR,
                                    LastUpdatedDateUTC = DateTime.Now,
                                    //physically_type = 1,
                                    //physically_selected_date = DateTime.Now,

                                };

                                if (settlementItem.OrderCreationDate.Contains('-') && settlementItem.OrderCreationDate.Contains(':'))
                                {
                                    settlementRefund.ShipmentDate = DateTime.ParseExact(settlementItem.OrderCreationDate, "dd-MM-yyyy HH:mm", CultureInfo.InvariantCulture);
                                }
                                else if (settlementItem.OrderCreationDate.Contains('-'))
                                {
                                    settlementRefund.ShipmentDate = DateTime.ParseExact(settlementItem.OrderCreationDate, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                                }
                                else
                                {
                                    DateTime outDate;
                                    DateTime.TryParse(settlementItem.OrderCreationDate, out outDate);
                                    settlementRefund.ShipmentDate = outDate;
                                }

                                dba.tbl_order_history.Add(settlementRefund);
                                dba.SaveChanges();
                                //history_id = settlementRefund.Id;
                             //}
                            //else
                            //{
                            //    get_historydetails.settlement_id = settlementItem.PGUTR;
                            //    dba.Entry(get_historydetails).State = EntityState.Modified;
                            //    dba.SaveChanges();
                            //    history_id = get_historydetails.Id;
                            //}
                                #endregion

                                #region INSERT INTO EXPENSE TABLE AND TAX

                                foreach (var fee in settlementFees)
                                {

                                var orderExpences = new m_tbl_expense
                                {
                                    date_created = DateTime.Now,

                                    reference_number = settlementItem.PGUTR,
                                    tbl_seller_id = settlementItem.tbl_SellerId,
                                    Original_order_id = settlementItem.OrderID,
                                    settlement_order_id = settlementItem.OrderID + "_" + settlementItem.OrderItemID,
                                    sku_no = settlementItem.MerchantSKU, //settlementItem.ProductID,
                                    quantity_purchased = 1,
                                    t_transactionType_id = 2,// 1 for order, 2 for refund
                                    tbl_order_historyid = settlementRefund.Id,// history_id,//settlementRefund.Id
                                    };

                                    if (settlementItem.SettlementDate.Contains('-') && settlementItem.SettlementDate.Contains(':'))
                                    {
                                        orderExpences.settlement_datetime = DateTime.ParseExact(settlementItem.SettlementDate, "dd-MM-yyyy HH:mm", CultureInfo.InvariantCulture);
                                    }
                                    else if (settlementItem.SettlementDate.Contains('-'))
                                    {
                                        orderExpences.settlement_datetime = DateTime.ParseExact(settlementItem.SettlementDate, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                                    }
                                    else
                                    {
                                        DateTime outDate;
                                        DateTime.TryParse(settlementItem.SettlementDate, out outDate);
                                        orderExpences.settlement_datetime = outDate;
                                    }

                                    var orderTax = new tbl_tax
                                    {
                                        isactive = 1,
                                        tbl_seller_id = (int)settlementItem.tbl_SellerId,
                                        reference_type = 7// 2 for order 7 for refund
                                    };

                                    if (fee.return_fee == "Marketplace Commission" && !string.IsNullOrEmpty(settlementItem.MarketplaceCommission)
                                        && Convert.ToDouble(settlementItem.MarketplaceCommission) != 0)
                                    {
                                        orderExpences.expense_type_id = fee.id;
                                        orderExpences.expense_amount = -Convert.ToDouble(settlementItem.MarketplaceCommission);

                                        orderTax.CGST_amount = -Convert.ToDouble(settlementItem.mp_commission_cgst);
                                        orderTax.Igst_amount = -Convert.ToDouble(settlementItem.mp_commission_igst);
                                        orderTax.sgst_amount = -Convert.ToDouble(settlementItem.mp_commission_sgst);

                                        dba.m_tbl_expense.Add(orderExpences);
                                        dba.SaveChanges();


                                        orderTax.tbl_referneced_id = orderExpences.id;
                                        dba.tbl_tax.Add(orderTax);
                                        dba.SaveChanges();
                                    }
                                    else if (fee.return_fee == "Logistics Charges" && !string.IsNullOrEmpty(settlementItem.LogisticsCharges)
                                        && Convert.ToDouble(settlementItem.LogisticsCharges) != 0)
                                    {
                                        orderExpences.expense_type_id = fee.id;
                                        orderExpences.expense_amount = -Convert.ToDouble(settlementItem.LogisticsCharges);

                                        orderTax.CGST_amount = -Convert.ToDouble(settlementItem.logistics_cgst);
                                        orderTax.Igst_amount = -Convert.ToDouble(settlementItem.logistics_igst);
                                        orderTax.sgst_amount = -Convert.ToDouble(settlementItem.logistics_sgst);

                                        dba.m_tbl_expense.Add(orderExpences);
                                        dba.SaveChanges();


                                        orderTax.tbl_referneced_id = orderExpences.id;
                                        dba.tbl_tax.Add(orderTax);
                                        dba.SaveChanges();
                                    }
                                    else if (fee.return_fee == "PG Commission" && !string.IsNullOrEmpty(settlementItem.PGCommission)
                                        && Convert.ToDouble(settlementItem.PGCommission) != 0)
                                    {
                                        orderExpences.expense_type_id = fee.id;
                                        orderExpences.expense_amount = -Convert.ToDouble(settlementItem.PGCommission);

                                        orderTax.CGST_amount = -Convert.ToDouble(settlementItem.pg_commission_cgst);
                                        orderTax.Igst_amount = -Convert.ToDouble(settlementItem.pg_commission_igst);
                                        orderTax.sgst_amount = -Convert.ToDouble(settlementItem.pg_commission_sgst);

                                        dba.m_tbl_expense.Add(orderExpences);
                                        dba.SaveChanges();


                                        orderTax.tbl_referneced_id = orderExpences.id;
                                        dba.tbl_tax.Add(orderTax);
                                        dba.SaveChanges();
                                    }
                                    else if (fee.return_fee == "Penalty" && !string.IsNullOrEmpty(settlementItem.Penalty)
                                        && Convert.ToDouble(settlementItem.Penalty) != 0)
                                    {
                                        orderExpences.expense_type_id = fee.id;
                                        orderExpences.expense_amount = Convert.ToDouble(settlementItem.Penalty);


                                        orderTax.CGST_amount = 0;
                                        orderTax.Igst_amount = Convert.ToDouble(settlementItem.Penalty) * 18 / 100;
                                        orderTax.sgst_amount = 0;

                                        dba.m_tbl_expense.Add(orderExpences);
                                        dba.SaveChanges();


                                        orderTax.tbl_referneced_id = orderExpences.id;
                                        dba.tbl_tax.Add(orderTax);
                                        dba.SaveChanges();
                                    }
                                    else if (fee.return_fee == "Net Adjustments" && !string.IsNullOrEmpty(settlementItem.NetAdjustments)
                                        && Convert.ToDouble(settlementItem.NetAdjustments) != 0)
                                    {
                                        orderExpences.expense_type_id = fee.id;
                                        orderExpences.expense_amount = -Convert.ToDouble(settlementItem.NetAdjustments);


                                        //For Net Adjustents all tax is 0
                                        orderTax.CGST_amount = 0;
                                        orderTax.Igst_amount = 0;
                                        orderTax.sgst_amount = 0;

                                        dba.m_tbl_expense.Add(orderExpences);
                                        dba.SaveChanges();

                                        orderTax.tbl_referneced_id = orderExpences.id;
                                        dba.tbl_tax.Add(orderTax);
                                        dba.SaveChanges();
                                    }

                                }

                                //SaveOrderExpencesAndTaxes(settlementFees, orderExpences, orderTax, settlementItem, false);
                                #endregion
                            //}
                        }

                        #endregion
                    }


                    var settlement_upload = dba.tbl_settlement_upload.Where(a => a.tbl_seller_id == SellerId &&
                    a.settlement_refernece_no == distinctSettlement.PGUTR &&
                    a.market_place_id == marketplaceID).FirstOrDefault();
                    if (settlement_upload != null)
                    {
                        settlement_upload.completed_datetime = DateTime.Now;
                        settlement_upload.file_status = "Complete";
                        settlement_upload.new_order_uploaded = allSettlementsByRefId.Count();
                        settlement_upload.LastUpdatedDateUTC = DateTime.Now;

                        dba.Entry(settlement_upload).State = EntityState.Modified;
                        dba.SaveChanges();
                    }

                    currentRunningNumber = currentRunningNumber + 1;

                }
                dba.Entry(get_upload).State = EntityState.Deleted;
                if (temp_allSettlements != null)
                {
                    foreach (var dd in temp_allSettlements)
                    {
                        dd.Status = 1;
                        dba.Entry(dd).State = EntityState.Modified;

                    }
                }
                if (get_seller_setting != null)
                {
                    if (currentRunningNumber != 0)
                    {
                        get_seller_setting.current_running_no = currentRunningNumber;
                        dba.Entry(get_seller_setting).State = EntityState.Modified;
                        dba.SaveChanges();
                    }
                }
                dba.SaveChanges();


            }
            catch (Exception ex)
            {
            }

        }
        public void SaveOrderExpencesAndTaxes(List<m_settlement_fee> settlementFees, m_tbl_expense orderExpences, tbl_tax orderTax, tbl_paytmsettmaster settlementItem, bool isCredit)
        {
            foreach (var fee in settlementFees)
            {

                if (fee.return_fee == "Marketplace Commission" && !string.IsNullOrEmpty(settlementItem.MarketplaceCommission) && Convert.ToDouble(settlementItem.MarketplaceCommission) > 0)
                {
                    orderExpences.expense_type_id = fee.id;
                    if (isCredit)
                    {
                        orderExpences.expense_amount = Convert.ToDouble(settlementItem.MarketplaceCommission) * -1;
                    }
                    else
                    {
                        orderExpences.expense_amount = Convert.ToDouble(settlementItem.MarketplaceCommission);
                    }

                    orderTax.CGST_amount = Convert.ToDouble(settlementItem.mp_commission_cgst);
                    orderTax.Igst_amount = Convert.ToDouble(settlementItem.mp_commission_igst);
                    orderTax.sgst_amount = Convert.ToDouble(settlementItem.mp_commission_sgst);

                    dba.m_tbl_expense.Add(orderExpences);
                    dba.SaveChanges();


                    orderTax.tbl_referneced_id = orderExpences.id;
                    dba.tbl_tax.Add(orderTax);
                    dba.SaveChanges();
                }
                else if (fee.return_fee == "Logistics Charges" && !string.IsNullOrEmpty(settlementItem.LogisticsCharges) && Convert.ToDouble(settlementItem.LogisticsCharges) > 0)
                {
                    orderExpences.expense_type_id = fee.id;
                    if (isCredit)
                    {
                        orderExpences.expense_amount = Convert.ToDouble(settlementItem.LogisticsCharges) * -1;
                    }
                    else
                    {
                        orderExpences.expense_amount = Convert.ToDouble(settlementItem.LogisticsCharges);
                    }

                    orderTax.CGST_amount = Convert.ToDouble(settlementItem.logistics_cgst);
                    orderTax.Igst_amount = Convert.ToDouble(settlementItem.logistics_igst);
                    orderTax.sgst_amount = Convert.ToDouble(settlementItem.logistics_sgst);

                    dba.m_tbl_expense.Add(orderExpences);
                    dba.SaveChanges();


                    orderTax.tbl_referneced_id = orderExpences.id;
                    dba.tbl_tax.Add(orderTax);
                    dba.SaveChanges();
                }
                else if (fee.return_fee == "PG Commission" && !string.IsNullOrEmpty(settlementItem.PGCommission) && Convert.ToDouble(settlementItem.PGCommission) > 0)
                {
                    orderExpences.expense_type_id = fee.id;
                    if (isCredit)
                    {
                        orderExpences.expense_amount = Convert.ToDouble(settlementItem.PGCommission) * -1;
                    }
                    else
                    {
                        orderExpences.expense_amount = Convert.ToDouble(settlementItem.PGCommission);
                    }

                    orderTax.CGST_amount = Convert.ToDouble(settlementItem.pg_commission_cgst);
                    orderTax.Igst_amount = Convert.ToDouble(settlementItem.pg_commission_igst);
                    orderTax.sgst_amount = Convert.ToDouble(settlementItem.pg_commission_sgst);

                    dba.m_tbl_expense.Add(orderExpences);
                    dba.SaveChanges();


                    orderTax.tbl_referneced_id = orderExpences.id;
                    dba.tbl_tax.Add(orderTax);
                    dba.SaveChanges();
                }
                else if (fee.return_fee == "Penalty" && !string.IsNullOrEmpty(settlementItem.Penalty) && Convert.ToDouble(settlementItem.Penalty) > 0)
                {
                    orderExpences.expense_type_id = fee.id;
                    if (isCredit)
                    {
                        orderExpences.expense_amount = Convert.ToDouble(settlementItem.Penalty) * -1;
                    }
                    else
                    {
                        orderExpences.expense_amount = Convert.ToDouble(settlementItem.Penalty);
                    }

                    orderTax.CGST_amount = 0;
                    orderTax.Igst_amount = (Convert.ToDouble(settlementItem.Penalty) * 18) / 100;
                    orderTax.sgst_amount = 0;

                    dba.m_tbl_expense.Add(orderExpences);
                    dba.SaveChanges();


                    orderTax.tbl_referneced_id = orderExpences.id;
                    dba.tbl_tax.Add(orderTax);
                    dba.SaveChanges();
                }
                else if (fee.return_fee == "Net Adjustments" && !string.IsNullOrEmpty(settlementItem.NetAdjustments) && Convert.ToDouble(settlementItem.NetAdjustments) > 0)
                {
                    orderExpences.expense_type_id = fee.id;
                    if (isCredit)
                    {
                        orderExpences.expense_amount = Convert.ToDouble(settlementItem.NetAdjustments) * -1;
                    }
                    else
                    {
                        orderExpences.expense_amount = Convert.ToDouble(settlementItem.NetAdjustments);
                    }

                    //For Net Adjustents all tax is 0
                    orderTax.CGST_amount = 0;
                    orderTax.Igst_amount = 0;
                    orderTax.sgst_amount = 0;

                    dba.m_tbl_expense.Add(orderExpences);
                    dba.SaveChanges();


                    orderTax.tbl_referneced_id = orderExpences.id;
                    dba.tbl_tax.Add(orderTax);
                    dba.SaveChanges();
                }

            }
        }



      

        #endregion







    }
}