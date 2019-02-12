using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SellerVendor.Areas.Seller.Models;
using SellerVendor.Areas.Seller.Models.DBContext;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SellerVendor.Areas.Seller.Controllers
{
    public class PurchaseController : Controller
    {
        public SellerContext dba = new SellerContext();
        comman_function cf = null;
        model_tbl_purchase objtblPurchase;
        public ActionResult Index(int? ID)
        {
            cf = new comman_function();
            if (!cf.session_check())
                return RedirectToAction("Login", "Home");
            int SellerId = Convert.ToInt32(Session["SellerID"]);
            objtblPurchase = new model_tbl_purchase();
            objtblPurchase.ddlPurchaseDetailsList = new List<tbl_purchase_details>();
            objtblPurchase.ddlPurchaseDetailsviewmodel = new List<PurchaseDetailsviewmodel>();
            objtblPurchase.ddlVendorList = new SelectList((dba.tbl_seller_vendors.Where(m => m.status == 1 && m.tbl_sellersid == SellerId).Select(p => new { p.id, p.vendor_name })).ToList(), "id", "vendor_name");
            
            objtblPurchase.ddlWarehouseList = new SelectList((dba.tbl_seller_warehouses.Where(m => m.isactive == 1 && m.tbl_sellers_id == SellerId && m.n_default_warehouse == 1).Select(p => new { p.id, p.warehouse_name })).ToList(), "id", "warehouse_name");

            var WareHouseName = dba.tbl_seller_warehouses.Where(m => m.isactive == 1 && m.tbl_sellers_id == SellerId && m.n_default_warehouse == 1).Select(p => new { p.id, p.warehouse_name }).ToList();
            if (WareHouseName != null && WareHouseName.Count > 0)
            {
                objtblPurchase.WarehosueName = WareHouseName[0].warehouse_name;
                objtblPurchase.tbl_seller_warehouses_id = WareHouseName[0].id;
            }
            if (ID != null && ID != 0)
            {
                var abc = dba.tbl_purchase.Where(a => a.id == ID).FirstOrDefault();
                var EditWarehousename = dba.tbl_seller_warehouses.Where(a => a.id == abc.tbl_seller_warehouses_id).FirstOrDefault();
                objtblPurchase.WarehosueName = EditWarehousename.warehouse_name;
                objtblPurchase.tbl_seller_warehouses_id = EditWarehousename.id;
            }
            objtblPurchase.ddlInventoryList = new SelectList((dba.tbl_inventory.Where(m => m.isactive == 1 && m.tbl_sellers_id == SellerId).Select(p => new { p.id, p.item_name })).ToList(), "id", "item_name");
            if (ID != null && ID != 0)
            {
                var purchase_list = dba.tbl_purchase.Where(a => a.id == ID).ToList();
                objtblPurchase.invoice_amount = purchase_list[0].invoice_amount;
                objtblPurchase.invoice_no = purchase_list[0].invoice_no;
                objtblPurchase.po_number = purchase_list[0].po_number;
                objtblPurchase.date_invoice = purchase_list[0].date_invoice;
                objtblPurchase.po_date = purchase_list[0].po_date;
                objtblPurchase.tax_amount = purchase_list[0].tax_amount;
                objtblPurchase.remarks_po = purchase_list[0].remarks_po;
                objtblPurchase.invoice_photo_path = purchase_list[0].invoice_photo_path;
                objtblPurchase.tbl_seller_vendors_id = purchase_list[0].tbl_seller_vendors_id;
                objtblPurchase.tbl_seller_warehouses_id = purchase_list[0].tbl_seller_warehouses_id;
                objtblPurchase.id = purchase_list[0].id;

               
                var purchase_details = dba.tbl_purchase_details.Where(a => a.tbl_purchase_id == ID).ToList();
                foreach (var item in purchase_details)
                {
                    PurchaseDetailsviewmodel oPurchase = new PurchaseDetailsviewmodel();
                    if (item != null)
                    {
                        //tbl_purchase_details objpurchase = new tbl_purchase_details();
                        oPurchase.base_amount = item.base_amount;
                        oPurchase.item_count = item.item_count;
                        oPurchase.tbl_inventory_id = item.tbl_inventory_id;
                        oPurchase.tbl_purchase_id = item.tbl_purchase_id;
                        oPurchase.PurchaseDetailsid = item.id;
                        objtblPurchase.ddlPurchaseDetailsviewmodel.Add(oPurchase);
                   // }           
                        var tax_details = dba.tbl_tax.Where(a => a.tbl_referneced_id == item.id && a.reference_type ==1).ToList();
                        foreach (var item2 in tax_details)
                        {
                            if (item2 != null)
                            {
                                //oPurchase.rate_of_tax = item2.rate_of_tax;
                                oPurchase.igst_tax = Convert.ToDouble(item2.igst_tax);
                                oPurchase.cgst_tax = Convert.ToDouble(item2.cgst_tax);
                                oPurchase.sgst_tax = Convert.ToDouble(item2.sgst_tax);
                                oPurchase.CGST_amount = item2.CGST_amount;
                                oPurchase.Igst_amount = item2.Igst_amount;
                                //oPurchase.rateoftax_amount = item2.rateoftax_amount;
                                oPurchase.sgst_amount = item2.sgst_amount;
                                oPurchase.t_totaltax_amount = item2.t_totaltax_amount;
                                oPurchase.tax_paid = Convert.ToInt16(item2.tax_paid);
                                objtblPurchase.ddlPurchaseDetailsviewmodel.Add(oPurchase);
                            }
                        } 
                    }                  
                }                       
            }
            return View(objtblPurchase);
        }


        public JsonResult Attachment()
        {
            string pic = string.Empty; for (int i = 0; i < Request.Files.Count; i++)
            {
                HttpPostedFileBase file = Request.Files[i];
                int filesize = file.ContentLength;
                string mimeType = file.ContentType;
                System.IO.Stream fileContent = file.InputStream;
                pic = System.IO.Path.GetFileName(file.FileName);
                string path = System.IO.Path.Combine(Server.MapPath("~/Upload/PurchaseInvoice"), pic);
                file.SaveAs(path);
            }
            return Json(pic, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveUpdatePurchase(model_tbl_purchase objtblpurchase, string Submit)
        {
            cf = new comman_function();
            if (!cf.session_check())
                return RedirectToAction("Login", "Home");
            bool ss = cf.session_check();
            int sellers_id = Convert.ToInt32(Session["SellerID"]);
            string msg = null;
            string SerialNo =  Convert.ToString(Request.Form["hdn_serialNo"]);
            //string ItemQuantity = Convert.ToString(Request.Form["hdn_orderItem"]);         
            objtblpurchase.t_Mode = Submit.ToUpper();
            objtblpurchase.tbl_sellers_id = Convert.ToInt32(Session["SellerID"]);
            objtblpurchase.created_by = Convert.ToInt32(Session["SellerID"]);
            objtblpurchase.isactive = 1;
            objtblpurchase.date_created = DateTime.Now;
            tbl_purchase tbl_purchase = new tbl_purchase();
            tbl_purchase.invoice_amount = objtblpurchase.invoice_amount;
            tbl_purchase.invoice_no = objtblpurchase.invoice_no;
            tbl_purchase.invoice_photo_path = objtblpurchase.invoice_photo_path;
            tbl_purchase.isactive = 1;
            tbl_purchase.tbl_sellers_id = Convert.ToInt32(Session["SellerID"]);
            tbl_purchase.created_by = Convert.ToInt32(Session["SellerID"]);
            tbl_purchase.date_created = DateTime.Now;
            tbl_purchase.po_date = objtblpurchase.po_date;
            tbl_purchase.remarks_po = objtblpurchase.remarks_po;
            tbl_purchase.date_invoice = objtblpurchase.date_invoice;
            tbl_purchase.po_number = objtblpurchase.po_number;
            tbl_purchase.tax_amount = objtblpurchase.tax_amount;
            tbl_purchase.tbl_seller_vendors_id = objtblpurchase.tbl_seller_vendors_id;
            tbl_purchase.tbl_seller_warehouses_id = objtblpurchase.tbl_seller_warehouses_id;
            tbl_purchase.n_postatus_type = objtblpurchase.n_postatus_type;
            tbl_purchase.t_Po_status = 0;

            //dba.tbl_purchase.Add(tbl_purchase);
            //dba.SaveChanges();


            //objtblpurchase.XmlPurchaseDetails = GetPurchaseDetailsListXML(objtblpurchase.XmlPurchaseDetails, tbl_purchase.id, SerialNo);



            if (objtblpurchase.t_Mode.ToUpper() == "SUBMIT")
            {
                //msg = "Purchase Save successfully ";
                //return msg;
                ViewBag.Message = "Purchase Save successfully";
                ViewData["Message"] = "Purchase Save successfully";
            }
            else
            {
                msg = "Update Successfully";
                //return msg;
            }
            return RedirectToAction("ManagePurchase");
            //return View("ManagePurchase");
            //Response.Redirect("");
        }

        public string GetPurchaseDetailsListXML(string Data, int purchase_id, string SerialNo)
        {
            cf = new comman_function();
            bool ss = cf.session_check();
            string t_PurchaseListXML = "<NewDataSet>";
            var CPList = JArray.Parse(Data);
            string _SerialNo = SerialNo;

         
            if (CPList.Count > 0)
            {
                //

                int sellers_id = Convert.ToInt32(Session["SellerID"]);
                int created_by = Convert.ToInt32(Session["SellerID"]);


                int i = 0;
                foreach (var item in CPList)
                {
                    dynamic CPobjc = JObject.Parse(item.ToString());

                    jsonDataconvert getdata = JsonConvert.DeserializeObject<jsonDataconvert>(item.ToString());

                    if (getdata != null)
                    {
                        //-----------------------------data save in purchase details table--------------------------
                        tbl_purchase_details Otbl_purchase_details = new tbl_purchase_details();
                        Otbl_purchase_details.tbl_sellers_id = sellers_id;
                        Otbl_purchase_details.base_amount = Convert.ToDouble(getdata.BaseAmount);
                        Otbl_purchase_details.tbl_purchase_id = purchase_id;
                        Otbl_purchase_details.item_count = Convert.ToInt32(getdata.ITEMCOUNT);
                        Otbl_purchase_details.tbl_inventory_id = Convert.ToInt32(getdata.InventoryID);
                        Otbl_purchase_details.isactive =1;
                        dba.tbl_purchase_details.Add(Otbl_purchase_details);
                        dba.SaveChanges();

                     
                        

                        var get_Inventory = dba.tbl_inventory.Where(a => a.id == Otbl_purchase_details.tbl_inventory_id && a.tbl_sellers_id == Otbl_purchase_details.tbl_sellers_id).FirstOrDefault();
                        if (get_Inventory != null)
                        {
                            int itemcount =Convert.ToInt16(get_Inventory.item_count);
                            get_Inventory.item_count = itemcount + Otbl_purchase_details.item_count;


                           double Average =Convert.ToDouble(get_Inventory.t_averagebought_price);// get first time  average price from inventory table 

                            float abc = (float)Otbl_purchase_details.base_amount / (float)Otbl_purchase_details.item_count;// divide baseamount fron item count

                            if (Average == 0 || Average == null)// check average price is null or 0
                            {
                                get_Inventory.t_averagebought_price = abc;
                            }
                            else
                            {
                                var qq = (Average + abc) / 2;// add average +abc and divide by 2 for getting average price 
                                get_Inventory.t_averagebought_price = qq;
                            }

                             dba.Entry(get_Inventory).State = EntityState.Modified;
                             dba.SaveChanges();
                        }                  

                        //-------------------------------------data save in tax table----------------------------------
                        tbl_tax objtax = new tbl_tax();
                        objtax.igst_tax = Convert.ToDouble(getdata.IGSTTAX);
                        objtax.cgst_tax = Convert.ToDouble(getdata.CGSTTAX);                    
                        objtax.sgst_tax = Convert.ToDouble(getdata.SGSTTAX);
                        objtax.CGST_amount = Convert.ToDouble(getdata.CGSTAMT);
                        objtax.Igst_amount = Convert.ToDouble(getdata.IGSTAMT);                     
                        objtax.sgst_amount = Convert.ToDouble(getdata.SGSTAMT);
                        objtax.t_totaltax_amount = Convert.ToDouble(getdata.TOTALTAXAMT);
                        objtax.tax_paid = Convert.ToDouble(getdata.TAXPAID);
                        objtax.tbl_seller_id = sellers_id;
                        objtax.tbl_referneced_id = Otbl_purchase_details.id;
                        objtax.reference_type = 1;
                        objtax.isactive = 1;
                        dba.tbl_tax.Add(objtax);
                        dba.SaveChanges();

                        // -----------------------data save in Inventory Details Table------------------------

                         var TblInventry = dba.tbl_inventory.Where(a => a.id == Otbl_purchase_details.tbl_inventory_id).FirstOrDefault();
                         string InventorySKu = TblInventry.sku;
                         int InventoryItemNo = Convert.ToInt32(TblInventry.item_No);
                         var PurchaseList = dba.tbl_purchase.Where(a => a.id == purchase_id).FirstOrDefault();
                         int warehouseid =Convert.ToInt16(PurchaseList.tbl_seller_warehouses_id); 

                        if (Otbl_purchase_details.item_count > 0)
                        {
                            for (var item1 = 0; item1 < Otbl_purchase_details.item_count; item1++)
                            {                             
                                InventoryItemNo++;
                                string getSer="";
                                string getSer1 = "";
                                try
                                {
                                    if (_SerialNo != "" && _SerialNo != null)
                                    {
                                        string a = _SerialNo;
                                        string[] serial = a.Split('#');
                                        if (serial.Length >= i)
                                        {
                                            if (serial[i] != null)
                                            {
                                                var abc = serial[i];
                                                var data1 = abc.Split(',');
                                                int ItemId = Convert.ToInt16(data1[0]);
                                                getSer = Convert.ToString(data1[1]);
                                                if (data1.Length > 2)
                                                {
                                                    getSer1 = Convert.ToString(data1[2]);
                                                }
                                            }
                                        }
                                    }
                                    tbl_inventory_details objinventorydetails = new tbl_inventory_details();
                                    objinventorydetails.bought_price = Convert.ToDouble(Otbl_purchase_details.base_amount);
                                    objinventorydetails.item_uid = InventorySKu + "-" + InventoryItemNo;

                                    objinventorydetails.m_item_status_id = 1;
                                    objinventorydetails.tbl_inventory_id = Convert.ToInt16(Otbl_purchase_details.tbl_inventory_id);
                                    objinventorydetails.tbl_purchase_details_id = Otbl_purchase_details.id;
                                    objinventorydetails.tbl_inventory_warehouse_transfers_id = warehouseid;
                                    objinventorydetails.updated_on = DateTime.Now;
                                    objinventorydetails.created_on = DateTime.Now;
                                    objinventorydetails.m_item_status_id = 1;
                                    objinventorydetails.isactive = 1;
                                    objinventorydetails.tbl_sellers_id = sellers_id;
                                    objinventorydetails.item_serialNo = getSer;
                                    objinventorydetails.batch_no = getSer1;
                                    dba.tbl_inventory_details.Add(objinventorydetails);
                                    dba.SaveChanges();
                                }
                                catch (Exception ex)
                                {
                                }
                                //for (var i = 0; i < serial.Length; i++)
                                //{
                                //    var abc = serial[i];
                                //    var data1 = abc.Split(',');
                                //    int ItemId = Convert.ToInt16(data1[0]);
                                //    string serailno = data1[1];
                                //    var get_itemdetails = dba.tbl_inventory_details.Where(d => d.tbl_inventory_id == objinventorydetails.tbl_inventory_id && d.tbl_sellers_id == objinventorydetails.tbl_sellers_id && d.id == objinventorydetails.id).FirstOrDefault();
                                //    if (ItemId == objinventorydetails.tbl_inventory_id)
                                //    {

                                //    }//end of if item compare

                                //}// end of for serail
                                //string s = ItemQuantity;
                                //string[] words = s.Split('#');
                                //for (var i = 0; i < words.Length; i++)
                                //{
                                //    var xyz = words[i];
                                //    var data2 = xyz.Split(',');
                                //    int ItemId1 = Convert.ToInt16(data2[0]);
                                //    string BatchNo = data2[1];
                                //    string Serialno = data2[2];
                                //}// end of for words
                            }
                        }
                        if (Convert.ToInt32(TblInventry.item_No) != InventoryItemNo)
                        {
                            TblInventry.item_No = InventoryItemNo;
                            dba.Entry(TblInventry).State = EntityState.Modified;
                            dba.SaveChanges();
                        }
                    }
                    i++;
                }
            }
            t_PurchaseListXML += "</NewDataSet>";
            return t_PurchaseListXML;

        }

        public class jsonDataconvert
        {
            public string InventoryID { get; set; }
            public string BaseAmount { get; set; }
            public string CGSTTAX { get; set; }
            public string CGSTAMT { get; set; }
            public string IGSTTAX { get; set; }
            public string IGSTAMT { get; set; }
            public string ITEMCOUNT { get; set; }         
            public string SGSTTAX { get; set; }
            public string SGSTAMT { get; set; }
            public string TAXPAID { get; set; }
            public string TOTALTAXAMT { get; set; }
           
        }

        /// <summary>
        /// For  Grid 
        /// </summary>
        /// <returns></returns>
        public ActionResult ManagePurchase()
        {
            List<SellerUtility> lst_purchase = new List<SellerUtility>();
            cf = new comman_function();
            if (!cf.session_check())
                return RedirectToAction("Login", "Home");
            try
            {
                int sellers_id = Convert.ToInt32(Session["SellerID"]);

                lst_purchase = (from ob_tbl_purchase in dba.tbl_purchase
                                //join ob_tbl_purchase_details in dba.tbl_purchase_details on
                                //ob_tbl_purchase.id equals ob_tbl_purchase_details.tbl_purchase_id 
                                join ob_tbl_seller_warehouses in dba.tbl_seller_warehouses on 
                                ob_tbl_purchase.tbl_seller_warehouses_id equals ob_tbl_seller_warehouses.id
                                into JoinedEmpDept
                                from proj in JoinedEmpDept.DefaultIfEmpty()

                                join ob_tbl_seller_vendors in dba.tbl_seller_vendors on
                                ob_tbl_purchase.tbl_seller_vendors_id equals ob_tbl_seller_vendors.id
                                into JoinedEmpDepts
                                from projer in JoinedEmpDepts.DefaultIfEmpty()
                                select new SellerUtility
                                {
                                    ob_tbl_seller_vendors = projer,
                                    ob_tbl_seller_warehouses = proj,
                                    ob_tbl_purchase = ob_tbl_purchase,
                                    //ob_tbl_purchase_details = ob_tbl_purchase_details,

                                }).Where(a => a.ob_tbl_purchase.tbl_sellers_id == sellers_id && a.ob_tbl_purchase.isactive == 1).ToList();

            }
            catch (Exception ex)
            {

            }

            return View(lst_purchase);
        }

        public JsonResult FindPurchaseDetail(long? Id)
        {
            cf = new comman_function();
            bool ss = cf.session_check();
            int sellers_id = Convert.ToInt32(Session["SellerID"]);
            var data = dba.tbl_purchase_details.Where(a => a.tbl_purchase_id == Id && a.isactive == 1 && a.tbl_sellers_id == sellers_id).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        


        public PartialViewResult ViewPurchaseOrderDetailsPopupPartial(int? ID)
        {
            List<PurchaseDetailsviewmodel> lst_purchase = new List<PurchaseDetailsviewmodel>();
            try
            {
                cf = new comman_function();
                bool ss = cf.session_check();
                int sellers_id = Convert.ToInt32(Session["SellerID"]);
                
                var GetPurchaseOrder = (from ob_purchase in dba.tbl_purchase_details.Where(a => a.tbl_purchase_id == ID && a.tbl_sellers_id == sellers_id && a.isactive == 1)
                                        join ob_Items in dba.tbl_tax on ob_purchase.id equals ob_Items.tbl_referneced_id
                                        into JoinedEmpDept
                                        from proj in JoinedEmpDept.Where(a => a.reference_type == 1 && a.tbl_seller_id == sellers_id).DefaultIfEmpty()
                                        join ob_Inventory in dba.tbl_inventory on ob_purchase.tbl_inventory_id equals ob_Inventory.id
                                        into JoinedEmpDept1
                                        from proj1 in JoinedEmpDept1.DefaultIfEmpty()
                                        select new
                                        {
                                            ob_tbl_purchase_details = ob_purchase,
                                            ob_tbl_tax = proj,
                                            ob_tbl_inventory = proj1
                                        }).ToList();
                foreach (var item in GetPurchaseOrder)
                {
                    PurchaseDetailsviewmodel order = new PurchaseDetailsviewmodel();
                    order.PurchaseDetailsid = item.ob_tbl_purchase_details.id;
                    
                    order.base_amount = item.ob_tbl_purchase_details.base_amount;
                    order.item_count = item.ob_tbl_purchase_details.item_count;
                    order.InventoryName = item.ob_tbl_inventory.item_name;
                    if (item.ob_tbl_tax != null)
                    {
                        order.t_totaltax_amount = item.ob_tbl_tax.t_totaltax_amount;                       
                        order.cgst_tax = Convert.ToDouble(item.ob_tbl_tax.cgst_tax);
                        order.igst_tax = Convert.ToDouble(item.ob_tbl_tax.igst_tax);
                        order.sgst_tax = Convert.ToDouble(item.ob_tbl_tax.sgst_tax);
                        order.CGST_amount = item.ob_tbl_tax.CGST_amount;
                        order.Igst_amount = item.ob_tbl_tax.Igst_amount;
                        order.sgst_amount = item.ob_tbl_tax.sgst_amount;
                        order.tax_paid = Convert.ToInt16(item.ob_tbl_tax.tax_paid);
                    }

                    lst_purchase.Add(order);
                }
            }
            catch(Exception ex)
            {
            }
            return PartialView(lst_purchase);
        }



        /// <summary>
        /// For Delete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult DeletePurchase(int id)
        {
            string Message = "";
            try
            {
                tbl_purchase purchase_details = dba.tbl_purchase.Where(a => a.id == id).FirstOrDefault();
                purchase_details.isactive = 0;
                dba.Entry(purchase_details).State = EntityState.Modified;
                dba.SaveChanges();
                var purchasedetails = dba.tbl_purchase_details.Where(a => a.tbl_purchase_id == purchase_details.id).ToList();
                if (purchasedetails != null)
                {
                    foreach (var item in purchasedetails)
                    {
                        item.isactive = 0;
                        dba.Entry(item).State = EntityState.Modified;
                        dba.SaveChanges();
                    }
                }
                Message = "Delete Sucessfully";
            }
            catch (Exception ex)
            {
            }
            return Json(Message, JsonRequestBehavior.AllowGet);
        }


        public JsonResult FillSalesStatus(int id)
        {
            cf = new comman_function();
            bool ss = cf.session_check();
            int SellerId = Convert.ToInt32(Session["SellerID"]);
            var GetStatusDetails = dba.tbl_inventory.Where(a => a.isactive == 1 && a.id == id).FirstOrDefault();
            return new JsonResult { Data = GetStatusDetails, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }
}
