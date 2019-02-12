using SellerVendor.Areas.Seller.AmazonAPI.settlement;
using SellerVendor.Areas.Seller.AmazonAPI.settlement.Model;
using SellerVendor.Areas.Seller.AmazonModel.settlementModel;
using SellerVendor.Areas.Seller.Controllers;
using SellerVendor.Areas.Seller.Models.DBContext;
using SellerVendor.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;

namespace SellerVendor.Areas.Seller.Models
{
    public class ProductListingAPI
    {
        public SellerContext dba = new SellerContext();
        public SellerAdminContext db = new SellerAdminContext();
        CronJobController objsales = new CronJobController();



        public string Call_API_Product(List<api_parameter> lstApiDetails, int strSeller_Id, int id)
        {
            string success = "";
            string responseXml = "";
            foreach (var objApi in lstApiDetails)
            {
                int my_seller_id = objApi.id; // = seller.id.ToString();
                string my_unique_id = objApi.Seller_ID; // = seller.t_access_Key_id.ToString();
                string t_secret_Key = objApi.Secret_Key; // = seller.my_unique_id.ToString();
                string t_access_Key_id = objApi.AWS_Access_Key_ID; // = seller.t_loginName.ToString();
                string t_auth_token = objApi.auth_token; // = seller.t_password.ToString();
                int m_marketplace_id = objApi.m_marketplace_id; // = seller.t_loginName.ToString();
                string market_palce_id = objApi.marketplaceid; // = seller.t_password.ToString();
                DateTime createA = objApi.createafter;
                DateTime createB = objApi.createbefore;

                String sellerid = my_unique_id;//"ALX5VZP4KM90V";
                String Secret_Key = t_secret_Key;// "tB1OE8WvQB4xZ8bOtD8aFIfW2H1uwfAIfYDMfVx8";
                String AWS_Access_Key_ID = t_access_Key_id;// "AKIAILZANI6ZKK4Z3PAQ";
                String marketplaceid = market_palce_id;//"A21TJRUUN4KGV";


                String accessKeyId = AWS_Access_Key_ID;
                String secretAccessKey = Secret_Key;

                const string applicationName = "AmazonJavascriptScratchpad";
                const string applicationVersion = "1.0.0.0";
                MarketplaceWebServiceConfig config = new MarketplaceWebServiceConfig();
                config.ServiceURL = "https://mws.amazonservices.in";



                MarketplaceWebService service = new MarketplaceWebServiceClient(
                   accessKeyId,
                   secretAccessKey,
                   applicationName,
                   applicationVersion,
                   config);

                try
                {
                    GetReportListRequest request = new GetReportListRequest();
                    DateTime DtStart;
                    DateTime DtEnd;
                    DtStart = createA;//DateTime.Parse("2017-12-01 23:59:59");
                    DtEnd = createB;//DateTime.Parse("2017-12-08 23:59:59");
                    request.Merchant = sellerid;// merchantId;               
                    request.AvailableFromDate = DtStart;
                    request.AvailableToDate = DtEnd;

                    List<string> list2 = new List<string>();
                    list2.Add("_GET_MERCHANT_LISTINGS_DATA_");
                    request.ReportTypeList = new TypeList();
                    request.ReportTypeList.Type = list2; // _GET_V2_SETTLEMENT_REPORT_DATA_XML_; // TypeList.WithType(list1);

                    List<string> InvokeGetReportList = new List<string>();

                    InvokeGetReportList = GetReportListSample.InvokeGetReportList(service, request);
                    if (InvokeGetReportList.Count > 0)
                    {
                        var item = InvokeGetReportList[InvokeGetReportList.Count - 1];
                        GetReportRequest request1 = new GetReportRequest();
                        request1.Merchant = sellerid;//merchantId;
                        request1.ReportId = item;//"8591233350017507"; // "9064510021017542";  

                        string strFolderPath = HttpContext.Current.Server.MapPath("~/UploadExcel/");
                        string filename = item + ".txt";
                        strFolderPath = strFolderPath + item + ".txt";

                        FileStream file1 = File.Open(strFolderPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                        request1.Report = file1;
                        GetReportResponse resp = service.GetReport(request1);
                        ResponseHeaderMetadata rhmd = resp.ResponseHeaderMetadata;
                        file1.Flush();
                        //byte[] bytes = new byte[file1.Length];
                        //int numBytesToRead = (int)file1.Length;
                        //int numBytesRead = 0;
                        //while (numBytesToRead > 0)
                        //{
                        //    int n = file1.Read(bytes, numBytesRead, numBytesToRead);
                        //    if (n == 0)
                        //        break;
                        //    numBytesRead += n;
                        //    numBytesToRead -= n;
                        //}
                        //string xmldata = System.Text.Encoding.UTF8.GetString(bytes);
                        file1.Close();
                        Read_ProductListData(strFolderPath, filename, strSeller_Id, id, m_marketplace_id);
                    }
                }
                catch (MarketplaceWebServiceException ex)
                {
                    success = "F";
                }
                catch (Exception ex)
                {
                    success = "F";
                }

            }//end of for each loop
            return success;
        }

        public List<ProductListDetails> Read_ProductListData(string strFolderPath, string filename, int SellerId, int id, int m_marketplace_id)
        {
            string Success = "S";
            List<ProductListDetails> objproduct = new List<ProductListDetails>();
            List<ProductList> objproductlist = new List<ProductList>();
            try
            {
                //string path = HttpContext.Current.Server.MapPath("~/UploadExcel/" + filename);
                DataTable datatable = new DataTable();
                StreamReader streamreader = new StreamReader(strFolderPath);
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
                    ProductList obj_item = new ProductList();
                    obj_item.ProductName = row[0].ToString();
                    obj_item.ProductDescription = row[1].ToString();
                    obj_item.SKU_NO = row[3].ToString();
                    obj_item.ProductPrice = row[4].ToString();
                    obj_item.ASIN_NO = row[16].ToString();
                    obj_item.ProductID = row[22].ToString();

                    objproductlist.Add(obj_item);
                }
                objproduct.Add(new ProductListDetails
                {
                    Productlistdetails = objproductlist,
                });
                if (objproduct.Count > 0)
                {
                    Success = SaveProductItem(objproduct, SellerId, id, m_marketplace_id);
                }
                else
                {
                    Success = "Em";
                }
            }// end of try 
            catch (Exception ex)
            {
                Success = "EX";
            }
            return objproduct;
        }

        public string SaveProductItem(List<ProductListDetails> objlist, int SellerId, int id, int m_marketplace_id)
        {
            string success = "S";
            try
            {
                if (objlist != null)
                {
                    tbl_inventory obj_item = null;
                    foreach (var item in objlist[0].Productlistdetails)
                    {
                        var get_inventoryDetails = dba.tbl_inventory.Where(a => a.sku.ToLower() == item.SKU_NO.ToLower() && a.tbl_sellers_id == 19).FirstOrDefault();
                        if (get_inventoryDetails == null)
                        {
                            obj_item = new tbl_inventory();
                            obj_item.sku = item.SKU_NO;
                            obj_item.item_name = item.ProductName;
                            obj_item.selling_price = Convert.ToInt16(item.ProductPrice);
                            obj_item.item_code = item.ProductID;
                            obj_item.tbl_sellers_id = SellerId;
                            obj_item.asin_no = item.ASIN_NO;
                            obj_item.item_dimension = item.ProductDescription;
                            obj_item.isactive = 1;
                            obj_item.tbl_marketplace_id = m_marketplace_id;
                            obj_item.tbl_product_upload_id = id;
                            dba.tbl_inventory.Add(obj_item);
                            dba.SaveChanges();
                        }// end of if
                    }// end of for eachloop

                    var get_productdetails = dba.tbl_product_upload.Where(a => a.Id == id).FirstOrDefault();
                    if (get_productdetails != null)
                    {
                        get_productdetails.status = "Completed";
                        get_productdetails.processing_datetime = DateTime.Now;
                        dba.Entry(get_productdetails).State = EntityState.Modified;
                        dba.SaveChanges();
                    }

                }// end of objlist
            }
            catch (Exception ex)
            {
                success = "EX";
            }
            return success;
        }
    }
}