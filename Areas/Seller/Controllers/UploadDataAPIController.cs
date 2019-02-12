using SellerVendor.Areas.Seller.Models;
using SellerVendor.Areas.Seller.Models.DBContext;
using SellerVendor.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO.Compression;
using System.Data;
using MySql.Data.MySqlClient;
using System.Data.SqlClient;
using System.Text;
using System.Globalization;
using System.Configuration;
using SellerVendor.Enums;
using SellerVendor.Utilities;

namespace SellerVendor.Areas.Seller.Controllers
{
    public class UploadDataAPIController : Controller
    {
        public SellerContext dba = new SellerContext();
        public SellerAdminContext db = new SellerAdminContext();
        comman_function cf = null;
        public ActionResult Index()
        {
            cf = new comman_function();
            if (!cf.session_check())
                return RedirectToAction("Login", "Home");
            int sellers_id = Convert.ToInt32(Session["SellerID"]);
            List<SelectListItem> lst1_loc = cf.GetMarketPlace(sellers_id);
            ViewData["MarKetPlace"] = lst1_loc;

            return View();
        }

        public FileResult Download(string ImageName)
        {
            //var FileVirtualPath = "~/App_Data/uploads/" + ImageName;
            return File(ImageName, "application/force-download", Path.GetFileName(ImageName));
        }

        public JsonResult FillMarketplace()
        {
            cf = new comman_function();
            bool ss = cf.session_check();
            int SellerId = Convert.ToInt32(Session["SellerID"]);
            var GetVendorDetails = cf.GetMarket_Place(SellerId).ToList();//dba.tbl_seller_vendors.Where(a => a.status == 1 && a.tbl_sellersid == SellerId).ToList();
            return new JsonResult { Data = GetVendorDetails, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult GetApiDetails(int marketplaceid)
        {
            int valueget = 0;
            int SellerId = Convert.ToInt32(Session["SellerID"]);
            var get_sellerapi_details = dba.tbl_sellermarketplace.Where(a => a.tbl_seller_id == SellerId && a.m_marketplace_id == marketplaceid && a.isactive == 1).FirstOrDefault();
            if (get_sellerapi_details != null)
            {
                string uniqueid = get_sellerapi_details.my_unique_id;
                string access_key = get_sellerapi_details.t_access_Key_id;
                string secret_key = get_sellerapi_details.t_secret_Key;
                string market_palce_id = get_sellerapi_details.market_palce_id;

                if (string.IsNullOrWhiteSpace(uniqueid) || string.IsNullOrWhiteSpace(access_key) || string.IsNullOrWhiteSpace(secret_key) || string.IsNullOrWhiteSpace(market_palce_id))
                {
                    valueget = 0;

                }
                else
                {
                    valueget = 1;
                }
            }
            else
            {
                valueget = 0;
            }
            return new JsonResult { Data = valueget, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        /// <summary>
        /// this is for get all order upload details
        /// </summary>
        /// <returns></returns>
        ///       
        public JsonResult GetUploadOrderDetails()
        {
            int sellers_id = Convert.ToInt32(Session["SellerID"]);

            var get_orderupload_details = dba.tbl_order_upload.Where(a => a.tbl_seller_id == sellers_id && a.type != 2).OrderByDescending(a => a.id).ToList();

            List<vieworderUpload> lst_sellermarketplace = new List<vieworderUpload>();
            foreach (var item in get_orderupload_details)
            {
                vieworderUpload uploadorder = new vieworderUpload();
                var abc = db.m_marketplace.Where(a => a.isactive == 1 && a.id == item.tbl_Marketplace_id).FirstOrDefault();
                if (abc != null)
                {
                    uploadorder.MarketplaceName = abc.name;
                    uploadorder.ImagePath = abc.logo_path;
                }
                uploadorder.FileName = item.filename;
                uploadorder.OrderCount = Convert.ToString(item.new_order_uploaded);
                uploadorder.id = item.id;
                uploadorder.Filestatus = item.status;
                uploadorder.Sellerid = Convert.ToInt16(item.tbl_seller_id);
                if (item.from_date != null)
                {
                    uploadorder.FromDate = Convert.ToDateTime(item.from_date).ToString("yyyy-MM-dd");
                }
                if (item.to_date != null)
                {
                    uploadorder.ToDate = Convert.ToDateTime(item.to_date).ToString("yyyy-MM-dd");
                }
                if (item.date_uploaded != null)
                {
                    uploadorder.UploadedDate = Convert.ToDateTime(item.date_uploaded).ToString("yyyy-MM-dd");
                }
                if (item.source == 1)
                {
                    uploadorder.SourceName = "Manual";
                }
                else if (item.source == 2)
                {
                    uploadorder.SourceName = "API";
                }

                var getsale_order = dba.tbl_sales_order.Where(a => a.tbl_sellers_id == sellers_id && a.tbl_order_upload_id == uploadorder.id).ToList();
                if (getsale_order != null)
                {
                    int count = 0;
                    foreach (var item1 in getsale_order)
                    {
                        var get_saleorder_details = dba.tbl_sales_order_details.Where(a => a.tbl_seller_id == sellers_id && a.tbl_sales_order_id == item1.id && a.is_tax_calculated == 0).FirstOrDefault();
                        if (get_saleorder_details != null)
                        {
                            count++;
                        }
                    }
                    uploadorder.BalanceTaxCount = count;
                }
                lst_sellermarketplace.Add(uploadorder);
            }
            return new JsonResult { Data = lst_sellermarketplace, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        /// <summary>
        /// this is for get all tax upload details
        /// </summary>
        /// <returns></returns>
        public JsonResult GetUploadTaxDetails()
        {

            int sellers_id = Convert.ToInt32(Session["SellerID"]);

            var get_taxupload_details = dba.tbl_order_upload.Where(a => a.tbl_seller_id == sellers_id && a.type == 2).OrderByDescending(a => a.id).ToList();

            List<vieworderUpload> lst_sellermarketplace = new List<vieworderUpload>();
            foreach (var item in get_taxupload_details)
            {
                vieworderUpload uploadorder = new vieworderUpload();
                var abc = db.m_marketplace.Where(a => a.isactive == 1 && a.id == item.tbl_Marketplace_id).FirstOrDefault();
                if (abc != null)
                {
                    uploadorder.MarketplaceName = abc.name;
                    uploadorder.ImagePath = abc.logo_path;
                }
                uploadorder.FileName = item.filename;
                uploadorder.OrderCount = Convert.ToString(item.new_order_uploaded);
                uploadorder.id = item.id;
                uploadorder.Filestatus = item.status;
                uploadorder.Sellerid = Convert.ToInt16(item.tbl_seller_id);
                if (item.from_date != null)
                {
                    uploadorder.FromDate = Convert.ToDateTime(item.from_date).ToString("yyyy-MM-dd");
                }
                if (item.to_date != null)
                {
                    uploadorder.ToDate = Convert.ToDateTime(item.to_date).ToString("yyyy-MM-dd");
                }
                if (item.date_uploaded != null)
                {
                    uploadorder.UploadedDate = Convert.ToDateTime(item.date_uploaded).ToString("yyyy-MM-dd");
                }
                if (item.source == 1)
                {
                    uploadorder.SourceName = "Manual";
                }
                else if (item.source == 2)
                {
                    uploadorder.SourceName = "Through API";
                }
                lst_sellermarketplace.Add(uploadorder);
            }



            return new JsonResult { Data = lst_sellermarketplace, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        /// <summary>
        /// this is for get all settlement  upload details 
        /// </summary>
        /// <returns></returns>
        public JsonResult GetSettlementOrderDetails()
        {
            int sellers_id = Convert.ToInt32(Session["SellerID"]);
            var get_settlementupload_details = dba.tbl_settlement_upload.Where(a => a.tbl_seller_id == sellers_id).OrderByDescending(a => a.Id).ToList();

            List<vieworderUpload> lst_sellermarketplace = new List<vieworderUpload>();
            foreach (var item in get_settlementupload_details)
            {
                vieworderUpload uploadorder = new vieworderUpload();
                var abc = db.m_marketplace.Where(a => a.isactive == 1 && a.id == item.market_place_id).FirstOrDefault();
                if (abc != null)
                {
                    uploadorder.MarketplaceName = abc.name;
                    uploadorder.ImagePath = abc.logo_path;
                }
                uploadorder.FileName = item.file_name;
                uploadorder.OrderCount = Convert.ToString(item.new_order_uploaded);
                uploadorder.Filestatus = item.file_status;
                uploadorder.suspenseamt = Convert.ToDecimal(item.suspense_amt);
                uploadorder.ReferenceNo = item.settlement_refernece_no;
                uploadorder.Sellerid = Convert.ToInt16(item.tbl_seller_id);
                if (item.Id != null)
                {
                    uploadorder.id = item.Id;
                }
                if (item.settlement_from != null)
                {
                    uploadorder.FromDate = Convert.ToDateTime(item.settlement_from).ToString("yyyy-MM-dd");
                }
                if (item.settlement_to != null)
                {
                    uploadorder.ToDate = Convert.ToDateTime(item.settlement_to).ToString("yyyy-MM-dd");
                }
                if (item.deposit_date != null)
                {
                    uploadorder.DepositDate = Convert.ToDateTime(item.deposit_date).ToString("yyyy-MM-dd");
                }
                if (item.uploaded_on != null)
                {
                    uploadorder.UploadedDate = Convert.ToDateTime(item.uploaded_on).ToString("yyyy-MM-dd");
                }
                if (item.Source == 1)
                {
                    uploadorder.SourceName = "Manual";
                }
                else if (item.Source == 2)
                {
                    uploadorder.SourceName = "API";
                }
                lst_sellermarketplace.Add(uploadorder);
            }
            return new JsonResult { Data = lst_sellermarketplace, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }



        /// <summary>
        /// this is for get all product upload details
        /// </summary>
        /// <returns></returns>
        public JsonResult GetProductDetails()
        {
            int sellers_id = Convert.ToInt32(Session["SellerID"]);

            var get_product_details = dba.tbl_product_upload.Where(a => a.tbl_seller_id == sellers_id && a.source == 1).OrderByDescending(a => a.Id).ToList();

            List<vieworderUpload> lst_sellermarketplace = new List<vieworderUpload>();
            foreach (var item in get_product_details)
            {
                vieworderUpload uploadorder = new vieworderUpload();
                var abc = db.m_marketplace.Where(a => a.isactive == 1 && a.id == item.tbl_marketplace_id).FirstOrDefault();
                if (abc != null)
                {
                    uploadorder.MarketplaceName = abc.name;
                    uploadorder.ImagePath = abc.logo_path;
                }

                uploadorder.id = item.Id;
                uploadorder.Filestatus = item.status;
                if (item.from_date != null)
                {
                    uploadorder.FromDate = Convert.ToDateTime(item.from_date).ToString("yyyy-MM-dd");
                }
                if (item.to_datetime != null)
                {
                    uploadorder.ToDate = Convert.ToDateTime(item.to_datetime).ToString("yyyy-MM-dd");
                }
                if (item.uploaded_datetime != null)
                {
                    uploadorder.UploadedDate = Convert.ToDateTime(item.uploaded_datetime).ToString("yyyy-MM-dd");
                }
                if (item.source == 2)
                {
                    uploadorder.SourceName = "Manual";
                }
                else if (item.source == 1)
                {
                    uploadorder.SourceName = "API";
                }
                lst_sellermarketplace.Add(uploadorder);
            }
            return new JsonResult { Data = lst_sellermarketplace, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        /// <summary>
        /// this is for get all return upload details
        /// </summary>
        /// <returns></returns>
        public JsonResult GetReturnUploadDetails()
        {
            int sellers_id = Convert.ToInt32(Session["SellerID"]);

            var get_return_details = dba.tbl_return_upload.Where(a => a.tbl_seller_id == sellers_id).OrderByDescending(a => a.id).ToList();

            List<vieworderUpload> lst_sellermarketplace = new List<vieworderUpload>();
            foreach (var item in get_return_details)
            {
                vieworderUpload uploadorder = new vieworderUpload();
                var abc = db.m_marketplace.Where(a => a.isactive == 1 && a.id == item.tbl_marketplace_id).FirstOrDefault();
                if (abc != null)
                {
                    uploadorder.MarketplaceName = abc.name;
                    uploadorder.ImagePath = abc.logo_path;
                }
                uploadorder.FileName = item.file_name;
                uploadorder.Sellerid = Convert.ToInt16(item.tbl_seller_id);
                uploadorder.OrderCount = Convert.ToString(item.return_count);
                uploadorder.id = item.id;
                uploadorder.Filestatus = item.file_status;

                if (item.uploaded_on != null)
                {
                    uploadorder.UploadedDate = Convert.ToDateTime(item.uploaded_on).ToString("yyyy-MM-dd");
                }
                if (item.source == 1)
                {
                    uploadorder.SourceName = "Manual";
                }
                else if (item.source == 2)
                {
                    uploadorder.SourceName = "API";
                }
                lst_sellermarketplace.Add(uploadorder);
            }
            return new JsonResult { Data = lst_sellermarketplace, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        /// <summary>
        /// get details from suspense table
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public JsonResult GetSuspenseDetails(int Id)
        {
            cf = new comman_function();
            bool ss = cf.session_check();
            int SellerId = Convert.ToInt32(Session["SellerID"]);
            var get_details = (from ob_tbl_settlement in dba.tbl_settlement_upload.Where(a => a.Id == Id)
                               join ob_tbl_suspense in dba.tbl_settlement_suspense_entries on ob_tbl_settlement.Id equals ob_tbl_suspense.tbl_settlement_upload_id
                               select new SellerUtility
                               {
                                   ob_tbl_settlement_suspense_entries = ob_tbl_suspense,
                                   ob_tbl_settlement_upload = ob_tbl_settlement,
                               }).OrderByDescending(a => a.ob_tbl_settlement_suspense_entries.id).ToList();

            //var get_details = (from ob_tbl_suspense in dba.tbl_settlement_suspense_entries
            //                   join ob_tbl_settlement in dba.tbl_settlement_upload on ob_tbl_suspense.tbl_settlement_upload_id
            //                           equals ob_tbl_settlement.Id                                      
            //                           select new SellerUtility
            //                           {
            //                               ob_tbl_settlement_suspense_entries = ob_tbl_suspense,
            //                               ob_tbl_settlement_upload = ob_tbl_settlement,
            //                           }).OrderByDescending(a => a.ob_tbl_settlement_suspense_entries.id).ToList();

            return new JsonResult { Data = get_details, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        /// <summary>
        /// /this is for  Sales Order API
        /// </summary>
        /// <returns></returns>
        public JsonResult OrderAPI(int? ddl_marketplaceAPI, DateTime? txt_from, DateTime? txt_to)
        {
            RetMessage message = new RetMessage();
            try
            {

                int SellerId = Convert.ToInt32(Session["SellerID"]);
                //cf = new comman_function();
                //List<SelectListItem> lst1_loc = cf.GetMarketPlace(SellerId);
                //ViewData["MarKetPlace"] = lst1_loc;
                int current_running_no = 0;
                short sourcetype = 2;
                var get_seller_setting = dba.tbl_seller_setting.Where(a => a.tbl_seller_id == SellerId && a.type == 3).FirstOrDefault();
                if (get_seller_setting != null)
                {
                    current_running_no = Convert.ToInt16(get_seller_setting.current_running_no);
                }
                tbl_order_upload obj_upload = new tbl_order_upload();
                obj_upload.date_uploaded = DateTime.Now;
                obj_upload.voucher_running_no = current_running_no += 1;
                obj_upload.filename = "";
                obj_upload.type = 3; //API
                obj_upload.tbl_seller_id = SellerId;
                obj_upload.tbl_Marketplace_id = ddl_marketplaceAPI;
                obj_upload.source = sourcetype;
                obj_upload.from_date = txt_from;
                obj_upload.to_date = txt_to;
                obj_upload.status = "Queued";
                dba.tbl_order_upload.Add(obj_upload);
                dba.SaveChanges();

                if (get_seller_setting != null)
                {

                    get_seller_setting.current_running_no = current_running_no;
                    dba.Entry(get_seller_setting).State = EntityState.Modified;
                    dba.SaveChanges();
                }
                message.message = "successfully saved.";
                message.status = 1;
            }
            catch (Exception ex)
            {
                message.message = "An error has occurs.";
                message.status = 0;
            }
            return Json(message, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// this is for calling Settlement API 
        /// </summary>
        /// <param name="form"></param>
        /// <param name="ddl_marketplaceAPI"></param>
        /// <param name="txt_from"></param>
        /// <param name="txt_to"></param>
        /// <returns></returns>
        public ActionResult SettlementAPI(int? ddl_marketplaceSettAPI, DateTime? txt_Settlementfrom, DateTime? txt_settlementto)
        {
            RetMessage message = new RetMessage();
            try
            {
                int SellerId = Convert.ToInt32(Session["SellerID"]);
                //cf = new comman_function();
                //List<SelectListItem> lst1_loc = cf.GetMarketPlace(SellerId);
                //ViewData["MarKetPlace"] = lst1_loc;
                int current_running_no = 0;
                int fundtransfer = 0;
                var get_seller_setting = dba.tbl_seller_setting.Where(a => a.tbl_seller_id == SellerId && a.type == 2).FirstOrDefault();
                if (get_seller_setting != null)
                {
                    current_running_no = Convert.ToInt16(get_seller_setting.current_running_no);
                }

                tbl_settlement_upload objupload = new tbl_settlement_upload();
                objupload.file_name = "";
                objupload.market_place_id = ddl_marketplaceSettAPI;
                objupload.tbl_seller_id = SellerId;
                objupload.uploaded_by = SellerId;
                objupload.uploaded_on = DateTime.Now;
                objupload.request_fromdate = txt_Settlementfrom;
                objupload.request_date_to = txt_settlementto;

                objupload.settlement_type = Convert.ToInt16(fundtransfer);
                objupload.Source = 2;
                objupload.file_status = "Queued";
                objupload.voucher_running_no = current_running_no;
                dba.tbl_settlement_upload.Add(objupload);
                dba.SaveChanges();
                if (get_seller_setting != null)
                {
                    get_seller_setting.current_running_no += 1;
                    dba.Entry(get_seller_setting).State = EntityState.Modified;
                    dba.SaveChanges();
                }
                message.message = "successfully saved.";
                message.status = 1;
            }
            catch (Exception ex)
            {
                message.message = "An error has occurs.";
                message.status = 0;
            }
            //return RedirectToAction("Index");
            // return View("Index");
            return Json(message, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// this is for bulk Settlement Upload
        /// </summary>
        /// <param name="postedFile"></param>
        /// <param name="form"></param>
        /// <returns></returns>

        [HttpPost]
        public JsonResult UploadBulkSettlement(byte ddl_marketplacebulk, string ddlFundType)
        {
            //TODO :change that to enum
            if (ddl_marketplacebulk != 5)
            {
                return UploadBulkSettlementOldVersion(ddl_marketplacebulk, ddlFundType);
            }
            else
            {
                var message = new RetMessage();
                var msgBuilder = new StringBuilder();

                var sellerId = Convert.ToInt32(Session["SellerID"]);

                msgBuilder.AppendLine("Process Started at " + DateTime.Now.ToString("HH:mm:ss", CultureInfo.InvariantCulture));
                try
                {
                    var bulkImport = new BulkImport();
                    FileType fileType;

                    Enum.TryParse(ddlFundType, out fileType);

                    //TODO: Will look into this. In this time ddlFundType is 0 
                    fileType = FileType.Settlement;
                    //TODO : this filesResult can be used to display it at screen.
                    var filesResult = bulkImport.ImportFiles(sellerId, ddl_marketplacebulk, fileType);

                    //TODO: That will be move to async process in background
                    foreach (var importedFile in filesResult.Where(x => x.IsDumpedIntoDb == true))
                    {
                        bulkImport.ProcessFile(importedFile, sellerId);
                    }

                    msgBuilder.AppendLine("Process Finished at " + DateTime.Now.ToString("HH:mm:ss", CultureInfo.InvariantCulture));

                    message.message = msgBuilder.ToString();
                    message.status = 1;

                }
                catch (Exception ex)
                {
                    message.message = "An error has occurs." + ex.Message;
                    message.status = 0;
                }
                return Json(message, JsonRequestBehavior.AllowGet);
            }


            //return View("Index");
        }

        public JsonResult UploadBulkSettlementOldVersion(byte ddl_marketplacebulk, string ddlFundType)
        {
            #region old style
            RetMessage message = new RetMessage();
            string strFolderPath = "";
            try
            {
                int SellerId = Convert.ToInt32(Session["SellerID"]);
                string strmarketplace = Convert.ToString(ddl_marketplacebulk);
                int marketplaceID = 0;
                if (strmarketplace != null && strmarketplace != "" && strmarketplace != "0")
                    marketplaceID = Convert.ToInt32(strmarketplace);

                if (SellerId == 0)
                {
                    ViewBag.Message = "Please login again";
                    message.message = "Please login again";
                    return Json(message, JsonRequestBehavior.AllowGet);
                }
                string strFundTransfer = Convert.ToString(ddlFundType);
                int fundtransfer = 0;
                if (strFundTransfer != null && strFundTransfer != "" && strFundTransfer != "0")
                    fundtransfer = Convert.ToInt32(strFundTransfer);

                strFolderPath = Server.MapPath("~/UploadExcel/" + SellerId + "/settlement/");
                if (!Directory.Exists(strFolderPath))
                {
                    Directory.CreateDirectory(strFolderPath);
                }
                int current_running_no = 0;
                int fileUploaded = 0;
                string fileNotUpload = "";
                foreach (string key in Request.Files)
                {

                    HttpPostedFileBase postedFile = Request.Files[key];
                    string fileName = Path.GetFileName(postedFile.FileName);
                    //postedFile.SaveAs(strFolderPath + fileName);
                    // ViewBag.Message += string.Format("<b>{0}</b> uploaded.<br />", fileName);


                    var get_seller_setting = dba.tbl_seller_setting.Where(a => a.tbl_seller_id == SellerId && a.type == 2).FirstOrDefault();
                    if (get_seller_setting != null)
                    {
                        current_running_no = Convert.ToInt16(get_seller_setting.current_running_no);
                    }

                    var get_uploaddetails = dba.tbl_settlement_upload.Where(a => a.tbl_seller_id == SellerId && a.file_name == fileName).FirstOrDefault();
                    if (get_uploaddetails == null)
                    {

                        postedFile.SaveAs(strFolderPath + postedFile.FileName);
                        tbl_settlement_upload objupload = new tbl_settlement_upload();
                        objupload.file_name = fileName;
                        objupload.market_place_id = marketplaceID;
                        objupload.tbl_seller_id = SellerId;
                        objupload.uploaded_by = SellerId;
                        objupload.uploaded_on = DateTime.Now;

                        objupload.settlement_type = Convert.ToInt16(fundtransfer);
                        objupload.Source = 1;
                        objupload.file_status = "Queued";
                        objupload.voucher_running_no = current_running_no;
                        dba.tbl_settlement_upload.Add(objupload);
                        dba.SaveChanges();
                        if (get_seller_setting != null)
                        {
                            get_seller_setting.current_running_no += 1;
                            dba.Entry(get_seller_setting).State = EntityState.Modified;
                            dba.SaveChanges();
                        }
                        fileUploaded++;
                    }
                    else
                    {
                        if (fileNotUpload != "")
                            fileNotUpload = fileNotUpload + ",";
                        fileNotUpload = fileNotUpload + fileName;
                    }
                }

                if (fileUploaded != Request.Files.Count)
                {
                    message.message = fileNotUpload + " files are already uploaded.";
                    message.status = 0;
                }
                else
                {
                    message.message = "files are uploaded successfully.";
                    message.status = 1;
                }
            }
            catch (Exception ex)
            {
                message.message = "An error has occurs.";
                message.status = 0;
            }
            return Json(message, JsonRequestBehavior.AllowGet);
            #endregion
        }


        [HttpPost]
        public JsonResult UploadBulkOrderExcel(int ddl_marketplaceOrder)
        {
            //TODO :change that to enum
            if (ddl_marketplaceOrder != 5)
            {
                return UploadBulkOrderExcel_Old(ddl_marketplaceOrder);
            }
            else
            {
                var message = new RetMessage();
                var msgBuilder = new StringBuilder();

                var sellerId = Convert.ToInt32(Session["SellerID"]);

                msgBuilder.AppendLine("Process Started at " + DateTime.Now.ToString("HH:mm:ss", CultureInfo.InvariantCulture));
                try
                {
                    var bulkImport = new BulkImport();
                    var fileType = FileType.SalesOrder;

                    //TODO: Will look into this. In this time ddlFundType is 0 
                    //TODO : this filesResult can be used to display it at screen.
                    var filesResult = bulkImport.ImportFiles(sellerId, (byte)ddl_marketplaceOrder, fileType);

                    //TODO: That will be move to async process in background
                    foreach (var importedFile in filesResult.Where(x => x.IsDumpedIntoDb == true))
                    {
                        bulkImport.ProcessFile(importedFile, sellerId);
                    }

                    msgBuilder.AppendLine("Process Finished at " + DateTime.Now.ToString("HH:mm:ss", CultureInfo.InvariantCulture));

                    message.message = msgBuilder.ToString();
                    message.status = 1;

                }
                catch (Exception ex)
                {
                    message.message = "An error has occurs." + ex.Message;
                    message.status = 0;
                }
                return Json(message, JsonRequestBehavior.AllowGet);
            }


            //return View("Index");
        }
        /// <summary>
        /// this is for Bulk Sales Order Upload
        /// </summary>
        /// <param name="postedFile"></param>
        /// <param name="form"></param>
        /// <returns></returns>

        public JsonResult UploadBulkOrderExcel_Old(int ddl_marketplaceOrder)
        {
            RetMessage message = new RetMessage();
            try
            {
                int SellerId = Convert.ToInt32(Session["SellerID"]);

                string strmarketplace = Convert.ToString(ddl_marketplaceOrder);
                int marketplaceID = 0;
                if (strmarketplace != null && strmarketplace != "" && strmarketplace != "0")
                    marketplaceID = Convert.ToInt32(strmarketplace);

                if (SellerId == 0)
                {
                    ViewBag.Message = "Please login again";

                }
                string strFolderPath = Server.MapPath("~/UploadExcel/" + SellerId + "/OrderSales/");
                if (!Directory.Exists(strFolderPath))
                {
                    Directory.CreateDirectory(strFolderPath);
                }
                int fileUploaded = 0;
                string fileNotUpload = "";
                foreach (string key in Request.Files)
                {
                    short sourcetype = 1;
                    HttpPostedFileBase postedFile = Request.Files[key];
                    string strFileName = "";
                    //if( Path.GetExtension(postedFile.FileName).Contains("zip"))
                    //{
                    //    ZipFile.CreateFromDirectory(postedFile.FileName, strFolderPath);
                    //    strFileName = Path.GetFileNameWithoutExtension(postedFile.FileName) + ".xml";
                    //}else 
                    //{
                    if (marketplaceID == 3)
                    {
                        strFileName = Path.GetFileNameWithoutExtension(postedFile.FileName) + ".xml";
                    }
                    else
                    {
                        strFileName = Path.GetFileName(postedFile.FileName);
                    }
                    //}


                    var get_orderupload_details = dba.tbl_order_upload.Where(a => a.tbl_seller_id == SellerId && a.filename == strFileName).FirstOrDefault();
                    if (get_orderupload_details == null)
                    {
                        postedFile.SaveAs(strFolderPath + strFileName);
                        int current_running_no = 0;

                        var get_seller_setting = dba.tbl_seller_setting.Where(a => a.tbl_seller_id == SellerId && a.type == 3).FirstOrDefault();
                        if (get_seller_setting != null)
                        {
                            current_running_no = Convert.ToInt16(get_seller_setting.current_running_no);
                        }
                        tbl_order_upload obj_upload = new tbl_order_upload();
                        obj_upload.date_uploaded = DateTime.Now;
                        obj_upload.voucher_running_no = current_running_no += 1;
                        obj_upload.filename = strFileName;
                        obj_upload.type = 1; //order xml
                        obj_upload.tbl_seller_id = SellerId;
                        obj_upload.tbl_Marketplace_id = marketplaceID;
                        obj_upload.source = sourcetype;
                        obj_upload.status = "Queued";
                        dba.tbl_order_upload.Add(obj_upload);
                        dba.SaveChanges();

                        if (get_seller_setting != null)
                        {
                            get_seller_setting.current_running_no = current_running_no;
                            dba.Entry(get_seller_setting).State = EntityState.Modified;
                            dba.SaveChanges();
                        }
                        fileUploaded++;
                        string success = "";
                    }
                    else
                    {
                        if (fileNotUpload != "")
                            fileNotUpload = fileNotUpload + ",";
                        fileNotUpload = fileNotUpload + strFileName;
                    }

                }
                if (fileUploaded != Request.Files.Count)
                {
                    message.message = fileNotUpload + " files are already uploaded.";
                    message.status = 0;
                }
                else
                {
                    message.message = "files are uploaded successfully.";
                    message.status = 1;
                }
            }
            catch (Exception ex)
            {
                message.message = "An error has occurs.";
                message.status = 0;
                //ViewBag.Message = "Unable to upload. " + ex.Message.ToString();
            }
            return Json(message, JsonRequestBehavior.AllowGet);
            //return Content("Success");

        }

        /// <summary>
        /// this is for Upload Bulk Tax file
        /// </summary>
        /// <param name="postedFile"></param>
        /// <param name="form"></param>
        /// <returns></returns>

        [HttpPost]
        public JsonResult UploadBulkTaxExcel(int ddl_marketplaceTax, string is_checked)
        {
            if (ddl_marketplaceTax != 5)
            {
                return UploadBulkTaxExcel_Old(ddl_marketplaceTax, is_checked);
            }
            else
            {
                var message = new RetMessage();
                var msgBuilder = new StringBuilder();

                var sellerId = Convert.ToInt32(Session["SellerID"]);

                msgBuilder.AppendLine("Process Started at " + DateTime.Now.ToString("HH:mm:ss", CultureInfo.InvariantCulture));
                try
                {
                    var bulkImport = new BulkImport();
                    var fileType = FileType.Tax;

                    //TODO: Will look into this. In this time ddlFundType is 0 
                    //TODO : this filesResult can be used to display it at screen.
                    var filesResult = bulkImport.ImportFiles(sellerId, (byte)ddl_marketplaceTax, fileType);

                    //TODO: That will be move to async process in background
                    foreach (var importedFile in filesResult.Where(x => x.IsDumpedIntoDb == true))
                    {
                        bulkImport.ProcessFile(importedFile, sellerId);
                    }

                    msgBuilder.AppendLine("Process Finished at " + DateTime.Now.ToString("HH:mm:ss", CultureInfo.InvariantCulture));

                    message.message = msgBuilder.ToString();
                    message.status = 1;

                }
                catch (Exception ex)
                {
                    message.message = "An error has occurs." + ex.Message;
                    message.status = 0;
                }
                return Json(message, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult UploadBulkTaxExcel_Old(int ddl_marketplaceTax, string is_checked)
        {
            RetMessage message = new RetMessage();
            try
            {
                int SellerId = Convert.ToInt32(Session["SellerID"]);
                string strmarketplace = Convert.ToString(ddl_marketplaceTax);
                int marketplaceID = 0;
                if (strmarketplace != null && strmarketplace != "" && strmarketplace != "0")
                    marketplaceID = Convert.ToInt32(strmarketplace);

                if (SellerId == 0)
                {
                    ViewBag.Message = "Please login again";

                }
                int addorder = 0;
                string orderchecked = Convert.ToString(is_checked);
                if (orderchecked != "0")
                {
                    addorder = 1;
                }
                string strFolderPath = Server.MapPath("~/UploadExcel/" + SellerId + "/Tax/");

                if (!Directory.Exists(strFolderPath))
                {
                    Directory.CreateDirectory(strFolderPath);
                }

                int fileUploaded = 0;
                string fileNotUpload = "";
                foreach (string key in Request.Files)
                {
                    short sourcetype = 1;
                    HttpPostedFileBase postedFile = Request.Files[key];
                    string strFileName = Path.GetFileName(postedFile.FileName);


                    var get_orderupload_details = dba.tbl_order_upload.Where(a => a.tbl_seller_id == SellerId && a.filename == strFileName).FirstOrDefault();
                    if (get_orderupload_details == null)
                    {

                        postedFile.SaveAs(strFolderPath + strFileName);
                        int current_running_no = 0;
                        var get_seller_setting = dba.tbl_seller_setting.Where(a => a.tbl_seller_id == SellerId && a.type == 3).FirstOrDefault();
                        if (get_seller_setting != null)
                        {
                            current_running_no = Convert.ToInt16(get_seller_setting.current_running_no);
                        }


                        tbl_order_upload obj_upload = new tbl_order_upload();
                        obj_upload.date_uploaded = DateTime.Now;
                        obj_upload.voucher_running_no = current_running_no += 1;
                        obj_upload.filename = strFileName;
                        obj_upload.type = 2; //Tax file
                        obj_upload.tbl_seller_id = SellerId;
                        obj_upload.tbl_Marketplace_id = marketplaceID;
                        obj_upload.source = sourcetype;
                        obj_upload.status = "Queued";
                        obj_upload.checkstatus = Convert.ToInt16(addorder);
                        dba.tbl_order_upload.Add(obj_upload);
                        dba.SaveChanges();
                        if (get_seller_setting != null)
                        {
                            get_seller_setting.current_running_no = current_running_no;
                            dba.Entry(get_seller_setting).State = EntityState.Modified;
                            dba.SaveChanges();
                        }
                        fileUploaded++;
                    }
                    else
                    {
                        if (fileNotUpload != "")
                            fileNotUpload = fileNotUpload + ",";
                        fileNotUpload = fileNotUpload + strFileName;
                    }
                }
                if (fileUploaded != Request.Files.Count)
                {
                    message.message = fileNotUpload + " files are already uploaded.";
                    message.status = 0;
                }
                else
                {
                    message.message = "files are uploaded successfully.";
                    message.status = 1;
                }

            }
            catch (Exception ex)
            {
                message.message = "An error has occurs.";
                message.status = 0;
            }
            return Json(message, JsonRequestBehavior.AllowGet);
            //return View("Index");
        }

        /// <summary>
        /// this is for Upload Retrun Data
        /// </summary>
        /// <param name="ddl_marketplaceTax"></param>
        /// <returns></returns>
        public JsonResult UploadReturnFile(int ddl_marketplaceReturn)
        {
            RetMessage message = new RetMessage();
            try
            {
                int SellerId = Convert.ToInt32(Session["SellerID"]);
                string strmarketplace = Convert.ToString(ddl_marketplaceReturn);
                int marketplaceID = 0;
                if (strmarketplace != null && strmarketplace != "" && strmarketplace != "0")
                    marketplaceID = Convert.ToInt32(strmarketplace);
                if (SellerId == 0)
                {
                    ViewBag.Message = "Please login again";

                }

                string strFolderPath = Server.MapPath("~/UploadExcel/" + SellerId + "/Return/");
                if (!Directory.Exists(strFolderPath))
                {
                    Directory.CreateDirectory(strFolderPath);
                }

                int fileUploaded = 0;
                string fileNotUpload = "";
                foreach (string key in Request.Files)
                {
                    short sourcetype = 1;
                    HttpPostedFileBase postedFile = Request.Files[key];
                    string fileName = Path.GetFileName(postedFile.FileName);


                    var get_returnupload_details = dba.tbl_return_upload.Where(a => a.tbl_seller_id == SellerId && a.file_name == fileName).FirstOrDefault();
                    if (get_returnupload_details == null)
                    {
                        postedFile.SaveAs(strFolderPath + fileName);
                        int current_running_no = 0;

                        var get_seller_setting = dba.tbl_seller_setting.Where(a => a.tbl_seller_id == SellerId && a.type == 4).FirstOrDefault();
                        if (get_seller_setting != null)
                        {
                            current_running_no = Convert.ToInt16(get_seller_setting.current_running_no);
                        }
                        tbl_return_upload obj_upload = new tbl_return_upload();
                        obj_upload.uploaded_on = DateTime.Now;
                        obj_upload.voucher_running_no = current_running_no += 1;
                        obj_upload.file_name = fileName;
                        obj_upload.tbl_seller_id = SellerId;
                        obj_upload.tbl_marketplace_id = marketplaceID;
                        obj_upload.uploaded_by = SellerId;
                        obj_upload.source = sourcetype;
                        obj_upload.file_status = "Queued";
                        dba.tbl_return_upload.Add(obj_upload);
                        dba.SaveChanges();
                        if (get_seller_setting != null)
                        {
                            //current_running_no++;
                            get_seller_setting.current_running_no = current_running_no;
                            dba.Entry(get_seller_setting).State = EntityState.Modified;
                            dba.SaveChanges();
                        }
                        fileUploaded++; ;
                    }
                    else
                    {
                        if (fileNotUpload != "")
                            fileNotUpload = fileNotUpload + ",";
                        fileNotUpload = fileNotUpload + fileName;
                    }
                }
                if (fileUploaded != Request.Files.Count)
                {
                    message.message = fileNotUpload + " files are already uploaded.";
                    message.status = 0;
                }
                else
                {
                    message.message = "files are uploaded successfully.";
                    message.status = 1;
                }
            }
            catch (Exception ex)
            {
                message.message = "An error has occurs.";
                message.status = 0;
            }
            return Json(message, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// this is for save product api details
        /// </summary>
        /// <param name="form"></param>
        /// <param name="ddl_marketplaceAPI"></param>
        /// <param name="txt_from"></param>
        /// <param name="txt_to"></param>
        /// <returns></returns>
        public JsonResult ProductAPI(int? ddl_marketplaceProductAPI, DateTime? txt_Productfrom, DateTime? txt_Productto)
        {
            RetMessage message = new RetMessage();
            try
            {
                int SellerId = Convert.ToInt32(Session["SellerID"]);
                cf = new comman_function();
                //List<SelectListItem> lst1_loc = cf.GetMarketPlace(SellerId);
                //ViewData["MarKetPlace"] = lst1_loc;
                //int current_running_no = 0;
                short sourcetype = 1;

                tbl_product_upload obj_upload = new tbl_product_upload();
                obj_upload.uploaded_datetime = DateTime.Now;

                obj_upload.source = 1; //API
                obj_upload.tbl_seller_id = SellerId;
                obj_upload.tbl_marketplace_id = ddl_marketplaceProductAPI;
                obj_upload.source = sourcetype;
                obj_upload.from_date = txt_Productfrom;
                obj_upload.to_datetime = txt_Productto;
                obj_upload.status = "Queued";
                dba.tbl_product_upload.Add(obj_upload);
                dba.SaveChanges();

                message.message = "successfully saved.";
                message.status = 1;
            }
            catch (Exception ex)
            {
                message.message = "An error has occurs.";
                message.status = 0;
            }
            //return RedirectToAction("Index");       
            return Json(message, JsonRequestBehavior.AllowGet);
        }


        public JsonResult UpdateTax()
        {
            string Message = "";
            int SellerId = Convert.ToInt32(Session["SellerID"]);
            try
            {
                double taxrate = 0;
                double itemtax = 0, cgst_tax = 0, sgst_tax = 0, igst_tax = 0;
                double itemprice = 0, cgst_amount = 0, sgst_amount = 0, igst_amount = 0;
                string customerstate = "";

                var get_saleorder_details = dba.tbl_sales_order_details.Where(a => a.tbl_seller_id == SellerId && a.is_tax_calculated == 0).ToList();
                if (get_saleorder_details != null)
                {
                    foreach (var item in get_saleorder_details)
                    {
                        itemtax = item.item_tax_amount;
                        itemprice = item.item_price_amount;

                        var get_inventory = dba.tbl_inventory.Where(a => a.tbl_sellers_id == SellerId && a.sku.ToLower() == item.sku_no.ToLower()).FirstOrDefault();//get inventory from sku no
                        if (get_inventory != null)
                        {
                            var categorydetails = dba.tbl_item_category.Where(a => a.id == get_inventory.tbl_item_category_id).FirstOrDefault();// get category 
                            if (categorydetails != null)
                            {

                                var category_slabs = dba.tbl_category_slabs.Where(a => a.m_category_id == categorydetails.id).ToList();
                                if (category_slabs != null)
                                {
                                    foreach (var slab in category_slabs)//if (category_slabs != null)
                                    {
                                        if (slab.from_rs < itemprice && slab.to_rs > itemprice)
                                        {
                                            taxrate = Convert.ToDouble(slab.tax_rate);
                                        }
                                        continue;
                                    }
                                }// end of if(category_slabs)

                                if (taxrate != 0 && taxrate != null)
                                {
                                    var seller_details = db.tbl_sellers.Where(a => a.id == SellerId).FirstOrDefault();
                                    if (seller_details != null)
                                    {
                                        var get_saleorder = dba.tbl_sales_order.Where(a => a.id == item.tbl_sales_order_id).FirstOrDefault();// to get sale order details for getting customer details
                                        var getcustomerdetails = dba.tbl_customer_details.Where(a => a.id == get_saleorder.tbl_Customer_Id).FirstOrDefault();// to get customer details from tbl customerdetails in seller admin db.
                                        if (getcustomerdetails != null)
                                        {
                                            customerstate = getcustomerdetails.State_Region.ToLower(); ;
                                        }
                                        var getcountrydetails = db.tbl_country.Where(a => a.status == 1 && a.countrylevel == 0 && a.id == seller_details.country).FirstOrDefault();// to get country name from country table in admin db.
                                        var getstatedetails = db.tbl_country.Where(m => m.id == seller_details.state && m.countrylevel == 1 && m.status == 1).FirstOrDefault();//to get state name from country table in admin db.
                                        string sellerstate = getstatedetails.countryname.ToLower();

                                        if (sellerstate == customerstate)
                                        {
                                            cgst_tax = Convert.ToDouble(taxrate) / 2;
                                            sgst_tax = Convert.ToDouble(taxrate) - cgst_tax;

                                            cgst_amount = (itemtax) / 2; //(objsaledetails.item_price_amount * 100) / (100 + Convert.ToDouble(cgst_tax));
                                            sgst_amount = (itemtax - cgst_amount); //(objsaledetails.item_price_amount * 100) / (100 + Convert.ToDouble(sgst_tax));                                         
                                        }
                                        else
                                        {
                                            igst_tax = Convert.ToDouble(taxrate);
                                            igst_amount = (itemtax); //(objsaledetails.item_price_amount * 100) / (100 + Convert.ToDouble(igst_tax));

                                        }
                                        var get_taxdetails = dba.tbl_tax.Where(a => a.tbl_referneced_id == item.id).FirstOrDefault();
                                        if (get_taxdetails == null)
                                        {
                                            tbl_tax objtax = new tbl_tax();
                                            objtax.tbl_seller_id = item.tbl_seller_id;
                                            objtax.tbl_referneced_id = item.id;
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

                                        }// end of if(get_taxdetails)
                                        else
                                        {
                                            if (cgst_tax != null && cgst_tax != 0.0 && sgst_tax != null && sgst_tax != 0.0)
                                            {
                                                get_taxdetails.cgst_tax = cgst_tax;
                                                get_taxdetails.sgst_tax = sgst_tax;
                                                get_taxdetails.CGST_amount = cgst_amount;
                                                get_taxdetails.sgst_amount = sgst_amount;
                                            }
                                            else
                                            {
                                                get_taxdetails.igst_tax = igst_tax;
                                                get_taxdetails.Igst_amount = igst_amount;
                                            }
                                            dba.Entry(get_taxdetails).State = EntityState.Modified;
                                            dba.SaveChanges();
                                        }
                                    }// end of if(seller_details)

                                    item.is_tax_calculated = 1;
                                    dba.Entry(item).State = EntityState.Modified;
                                    dba.SaveChanges();
                                }// end of if(taxrate)
                            }// end of if(categorydetails)

                        }// end of if (get_inventory)

                    }// end of for each loop main

                }// end of if(get_saleorder_details)

                Message = "Successfully";
                return Json(Message, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exp)
            {
                return Json("Error in getting record !", JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult Date()
        {
            return View();
        }
    }
}
