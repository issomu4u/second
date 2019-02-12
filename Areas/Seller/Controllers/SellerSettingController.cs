using SellerVendor.Areas.Seller.Models;
using SellerVendor.Areas.Seller.Models.DBContext;
using SellerVendor.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace SellerVendor.Areas.Seller.Controllers
{
    public class SellerSettingController : Controller
    {
        public SellerContext dba = new SellerContext();
        public SellerAdminContext db = new SellerAdminContext();
        comman_function cf = null;
        model_tbl_seller_setting objtbl_sellersetting;
        public ActionResult Index(int? Id)
        {
            cf = new comman_function();
            if (!cf.session_check())
                return RedirectToAction("Login", "Home");
            objtbl_sellersetting = new model_tbl_seller_setting();
            int SellerId = Convert.ToInt32(Session["SellerID"]);
            DateTime date = DateTime.Now.Date;

            var year = date.Year;
            var Finaldate = "04/01/" + year;
            var Final = Convert.ToDateTime(Finaldate);
            if (date < Final)
            {
                var a = year - 1;
                objtbl_sellersetting.t_financial_year = Convert.ToString(a + "-" + year);
            }
            else
            {
                var b = year + 1;
                objtbl_sellersetting.t_financial_year = Convert.ToString(year + "-" + b);
            }

            var getdetails = dba.tbl_seller_setting.Where(a => a.id == Id).FirstOrDefault();
            if (getdetails != null)
            {
                objtbl_sellersetting.financialyear = getdetails.t_financial_year;
                objtbl_sellersetting.t_seller_prefix_code = getdetails.t_seller_prefix_code;
                objtbl_sellersetting.invoice_current_running_no = getdetails.t_running_no;
                objtbl_sellersetting.n_financial_status = getdetails.n_financial_status;
                objtbl_sellersetting.id = getdetails.id;

            }
            objtbl_sellersetting.tbl_seller_id = SellerId;
           // var get_details1 = db.tbl_user_payment.Where(a => a.user_id == SellerId).FirstOrDefault();
            var get_details1 = db.tbl_user_login.Where(a => a.tbl_sellers_id == SellerId && a.isactive == 1).FirstOrDefault();
            if (get_details1 != null)
            {
                var get_user_payment = db.tbl_user_payment.Where(a => a.user_id == SellerId).FirstOrDefault();
                if (get_user_payment != null)
                {
                    objtbl_sellersetting.amount_added = get_details1.wallet_balance;
                    objtbl_sellersetting.date_of_creation = get_user_payment.date_of_creation;
                    objtbl_sellersetting.description = get_user_payment.description;
                }
            }
            var get_invoicedetails = dba.tbl_seller_setting.Where(a => a.tbl_seller_id == SellerId && a.type == 1).FirstOrDefault();
            if (get_invoicedetails != null)
            {
                objtbl_sellersetting.invoice_current_running_no = Convert.ToInt16(get_invoicedetails.current_running_no);
                objtbl_sellersetting.t_seller_prefix_code = get_invoicedetails.t_seller_prefix_code;
                objtbl_sellersetting.n_financial_status = get_invoicedetails.n_financial_status;
                objtbl_sellersetting.financialyear = get_invoicedetails.t_financial_year;
            }

            var get_settdetails = dba.tbl_seller_setting.Where(a => a.tbl_seller_id == SellerId && a.type == 2).FirstOrDefault();
            if (get_settdetails != null)
            {
                objtbl_sellersetting.sett_current_running_no = Convert.ToInt16(get_settdetails.current_running_no);
                objtbl_sellersetting.sett_prefix_code = get_settdetails.t_seller_prefix_code;
            }
            var get_set_details = dba.tbl_seller_setting.Where(a => a.tbl_seller_id == SellerId && a.type == 3).FirstOrDefault();
            if (get_set_details != null)
            {
                objtbl_sellersetting.sales_current_running_no = Convert.ToInt16(get_set_details.current_running_no);
                objtbl_sellersetting.sales_prefix_code = get_set_details.t_seller_prefix_code;
            }

            var get_setdetails = dba.tbl_seller_setting.Where(a => a.tbl_seller_id == SellerId && a.type == 4).FirstOrDefault();
            if (get_setdetails != null)
            {
                objtbl_sellersetting.return_current_running_no = Convert.ToInt16(get_setdetails.current_running_no);
                objtbl_sellersetting.return_prefix_code = get_setdetails.t_seller_prefix_code;
            }

            var get_account_details = dba.tbl_seller_accounting_pkg_details.Where(a => a.seller_id == SellerId).FirstOrDefault();
            if (get_account_details != null)
            {
                objtbl_sellersetting.accounting_sft_id = Convert.ToInt16(get_account_details.accounting_sft_id);

            }
            var get_profile_details = db.tbl_sellers.Where(a => a.id == SellerId).FirstOrDefault();
            
            //objtbl_sellersetting.ddl_state = new SelectList((db.tbl_country.Where(m => m.status == 1 && m.id == SellerId).Select(p => new { p.id, p.countryname })).ToList(), "id", "countryname");
            //if (get_profile_details != null)
            //{
            //    objtbl_sellersetting.ddl_country = new SelectList((db.tbl_country.Where(m => m.status == 1 && m.countrylevel == 0).Select(p => new { p.id, p.countryname })).ToList(), "id", "countryname");

            //    objtbl_sellersetting.ddl_state = new SelectList((db.tbl_country.Where(m => m.parentid == get_profile_details.country && m.countrylevel == 1 && m.status == 1).Select(p => new { p.id, p.countryname })).ToList(), "id", "countryname");
            //    objtbl_sellersetting.contact_person = get_profile_details.contact_person;
            //    objtbl_sellersetting.business_name = get_profile_details.business_name;
            //    objtbl_sellersetting.gstin = get_profile_details.gstin;
            //    objtbl_sellersetting.pan = get_profile_details.pan;
            //    objtbl_sellersetting.state = get_profile_details.state;
            //    objtbl_sellersetting.country = get_profile_details.country;
            //    objtbl_sellersetting.city = get_profile_details.city;
            //    objtbl_sellersetting.address = get_profile_details.address;
            //    objtbl_sellersetting.pincode = get_profile_details.pincode;
            //    objtbl_sellersetting.ProfileID = get_profile_details.id;
            //    objtbl_sellersetting.mobile = get_profile_details.mobile;
            //}
            return View(objtbl_sellersetting);
        }

        public JsonResult GetProfileDetails()
        {
            int SellerId = Convert.ToInt32(Session["SellerID"]);
            model_tbl_seller_setting objprofile = new model_tbl_seller_setting();
            try
            {
                var get_profile_details = db.tbl_sellers.Where(a => a.id == SellerId).FirstOrDefault();
                if (get_profile_details != null)
                {
                    objprofile.ddl_country = new SelectList((db.tbl_country.Where(m => m.status == 1 && m.countrylevel == 0).Select(p => new { p.id, p.countryname })).ToList(), "id", "countryname");
                    objprofile.ddl_state = new SelectList((db.tbl_country.Where(m => m.parentid == get_profile_details.country && m.countrylevel == 1 && m.status == 1).Select(p => new { p.id, p.countryname })).ToList(), "id", "countryname");
                    objprofile.contact_person = get_profile_details.contact_person;
                    objprofile.business_name = get_profile_details.business_name;
                    objprofile.gstin = get_profile_details.gstin;
                    objprofile.pan = get_profile_details.pan;
                    objprofile.state = get_profile_details.state;
                    objprofile.country = get_profile_details.country;
                    objprofile.city = get_profile_details.city;
                    objprofile.address = get_profile_details.address;
                    objprofile.pincode = get_profile_details.pincode;
                    objprofile.ProfileID = get_profile_details.id;
                    objprofile.mobile = get_profile_details.mobile;
                }
            }
            catch(Exception ex)
            {
            }
            return Json(objprofile, JsonRequestBehavior.AllowGet);
        }

        public JsonResult FillCountry()
        {
            cf = new comman_function();
            bool ss = cf.session_check();
            int SellerId = Convert.ToInt32(Session["SellerID"]);
            var GetCountryDetails = db.tbl_country.Where(m => m.status == 1 && m.countrylevel == 0).ToList();
            return new JsonResult { Data = GetCountryDetails, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult FillState(int? countryid)
        {
            var GetStateDetails = db.tbl_country.Where(a => a.parentid == countryid && a.countrylevel == 1 && a.status == 1).ToList();
            return new JsonResult { Data = GetStateDetails, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult GetStateList(int? n_CountryId)
        {
            var statedetails =   db.tbl_country.Where(a => a.parentid == n_CountryId && a.countrylevel == 1 && a.status == 1).ToList();
            //db.tbl_country.Where(m => m.parentid == get_profile_details.country && m.countrylevel == 1 && m.status == 1).Select(p => new { p.id, p.countryname })).ToList(), "id", "countryname");          
            return this.Json(statedetails, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Index(FormCollection frm, model_tbl_seller_setting objsellersetting)
        {
            cf = new comman_function();
            if (!cf.session_check())
                return RedirectToAction("Login", "Home");
            string button = frm["submit"];
            string button1 = frm["submit1"];

            if (button == "Submit")
            {
                tbl_user_login objUserLogin = new tbl_user_login();
                string msg;
                var getdetails = db.tbl_user_login.Where(a => a.tbl_sellers_id == objsellersetting.tbl_seller_id && a.password == objsellersetting.t_oldPassword).FirstOrDefault();
                if (objsellersetting.t_NewPassword == objsellersetting.t_ConfirmPassword)
                {
                    if (objsellersetting.t_oldPassword == getdetails.password)
                    {
                        if (getdetails != null)
                        {
                            getdetails.password = objsellersetting.t_NewPassword;
                            dba.Entry(getdetails).State = EntityState.Modified;
                            dba.SaveChanges();
                            objsellersetting.Msg = "Your password have been changed Successfully";
                            Response.Redirect("~/Login/Home");
                        }
                    }
                    else
                    {
                        objsellersetting.Msg = "Old password not match with your Password";
                    }
                }
                else
                {
                    objsellersetting.Msg = "Password & confirm Password should not be match";
                }
            }
            return View();
        }
        public ActionResult SaveUpdateSellerSetting(model_tbl_seller_setting objsellersetting, string Submit)
        {
            cf = new comman_function();
            if (!cf.session_check())
                return RedirectToAction("Login", "Home");
            int SellerId = Convert.ToInt32(Session["SellerID"]);
            string msg = null;
            var get_details = dba.tbl_seller_setting.Where(a => a.type == 1 && a.tbl_seller_id == SellerId).FirstOrDefault();
            if (get_details == null)
            {
                tbl_seller_setting objtbl = new tbl_seller_setting();
                objtbl.created_on = DateTime.Now;
                objtbl.is_active = 0;
                objtbl.type = 1;
                objtbl.t_seller_prefix_code = objsellersetting.t_seller_prefix_code;
                objtbl.t_running_no = objsellersetting.invoice_current_running_no;
                objtbl.current_running_no = objsellersetting.invoice_current_running_no;
                objtbl.n_financial_status = objsellersetting.n_financial_status;
                objtbl.t_financial_year = objsellersetting.financialyear;
                objtbl.tbl_seller_id = Convert.ToInt32(Session["SellerID"]);
                dba.tbl_seller_setting.Add(objtbl);
                dba.SaveChanges();
                TempData["Message"] = "Settings Save successfully";

            }
            else
            {
                get_details.type = 1;
                get_details.t_seller_prefix_code = objsellersetting.t_seller_prefix_code;
                get_details.n_financial_status = objsellersetting.n_financial_status;
                get_details.t_financial_year = objsellersetting.financialyear;
                get_details.t_running_no = Convert.ToInt16(objsellersetting.invoice_current_running_no);
                get_details.current_running_no = objsellersetting.invoice_current_running_no;
                dba.Entry(get_details).State = EntityState.Modified;
                dba.SaveChanges();
            }
            return RedirectToAction("Index", "SellerSetting");
        }

        public PartialViewResult SellerSettingDetails()
        {
            cf = new comman_function();
            bool ss = cf.session_check();
            int SellerId = Convert.ToInt32(Session["SellerID"]);
            List<model_tbl_seller_setting> obj = new List<model_tbl_seller_setting>();
            var getdetails = (from ob_user in dba.tbl_seller_setting
                              select new
                              {
                                  ob_user = ob_user,
                              }).Where(a => a.ob_user.is_active == 0 && a.ob_user.tbl_seller_id == SellerId).ToList();
            foreach (var item in getdetails)
            {
                model_tbl_seller_setting order = new model_tbl_seller_setting();
                order.id = item.ob_user.id;
                order.t_financial_year = item.ob_user.t_financial_year;
                order.t_seller_prefix_code = item.ob_user.t_seller_prefix_code;
                order.t_running_no = item.ob_user.t_running_no;
                obj.Add(order);
            }

            return PartialView(obj);
        }

        public ActionResult DeleteInvoiceSetting(int? id)
        {
            var get_user_details = dba.tbl_seller_setting.Where(a => a.id == id).FirstOrDefault();
            if (get_user_details != null)
            {
                get_user_details.is_active = 1;
                dba.Entry(get_user_details).State = EntityState.Modified;
                dba.SaveChanges();
                ViewData["ShowDeleteMsg"] = "User is Deleted Successfully !!!";
            }
            return RedirectToAction("Index", "SellerSetting");
        }

        public JsonResult SaveGuidNumber(string txt_guid, int ddl_tally)
        {
            int SellerId = Convert.ToInt32(Session["SellerID"]);

            tbl_seller_accounting_pkg_details objGuidno1 = new tbl_seller_accounting_pkg_details();
            string msg;

            objGuidno1.guid = txt_guid;
            objGuidno1.date_created = DateTime.Now;
            objGuidno1.accounting_sft_id = ddl_tally;
            objGuidno1.seller_id = SellerId;
            dba.tbl_seller_accounting_pkg_details.Add(objGuidno1);
            dba.SaveChanges();
            return Json(objGuidno1, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveChangePassword(model_tbl_seller_setting objtblLogin)
        {
            tbl_user_login objUserLogin = new tbl_user_login();
            string msg;
            var getdetails = db.tbl_user_login.Where(a => a.tbl_sellers_id == objtblLogin.tbl_seller_id && a.password == objtblLogin.t_oldPassword).FirstOrDefault();
            if (objtblLogin.t_NewPassword == objtblLogin.t_ConfirmPassword)
            {
                if (objtblLogin.t_oldPassword == getdetails.password)
                {
                    if (getdetails != null)
                    {
                        getdetails.password = objtblLogin.t_NewPassword;
                        //dba.Entry(getdetails).State = EntityState.Modified;
                        //dba.SaveChanges();
                        objtblLogin.Msg = "Your password have been changed Successfully";
                        Response.Redirect("~/Login/Home");
                    }
                }
                else
                {
                    objtblLogin.Msg = "Old password not match with your Password";
                }
            }
            else
            {
                objtblLogin.Msg = "Password & confirm Password should not be match";
            }
            return Json(objtblLogin.Msg, JsonRequestBehavior.AllowGet);
        }

        //    public ActionResult Index1(int? id)  
        //    {  
        //        ViewBag.Operation = id;  
        //        ViewBag.Name = dba.tbl_seller_setting.ToList();  
        //        tbl_seller_setting objEmp = dba.tbl_seller_setting.Find(id);  
        //        return View(objEmp);  
        //    }  

        //    [HttpPost]  

        //    [ValidateAntiForgeryToken]  
        //    public ActionResult Create(model_tbl_seller_setting objEmp)   
        //    {  
        //        if (ModelState.IsValid)  
        //        {  
        //            //dba.tbl_seller_setting.Add(objEmp);  
        //            dba.SaveChanges();  
        //        }  
        //        return RedirectToAction("Index");  
        //    }  

        //    [HttpPost]  

        //    [ValidateAntiForgeryToken]  
        //    public ActionResult Update(model_tbl_seller_setting objEmp)  
        //    {  
        //        if (ModelState.IsValid)  
        //        {  
        //            dba.Entry(objEmp).State = EntityState.Modified;  
        //            dba.SaveChanges();  
        //        }  
        //        return RedirectToAction("Index", new { id = 0 });  
        //    }  

        //    public ActionResult Delete(int id)  
        //    {  
        //        //model_tbl_seller_setting objEmp = dba.tbl_seller_setting.Find(id);  
        //        //dba.tblEmps.Remove(objEmp);  
        //        //dba.SaveChanges();  
        //        return RedirectToAction("Index", new { id = 0 });  
        //    }  
        //}   

        public ActionResult SettlementVoucher(model_tbl_seller_setting obj_model)
        {
            cf = new comman_function();
            if (!cf.session_check())
                return RedirectToAction("Login", "Home");
            int SellerId = Convert.ToInt32(Session["SellerID"]);
            var get_details = dba.tbl_seller_setting.Where(a => a.type == 2 && a.tbl_seller_id == SellerId).FirstOrDefault();
            if (get_details == null)
            {
                tbl_seller_setting obj = new tbl_seller_setting();
                obj.type = 2;
                obj.t_seller_prefix_code = obj_model.sett_prefix_code;
                obj.t_running_no = Convert.ToInt16(obj_model.sett_current_running_no);
                obj.current_running_no = Convert.ToInt16(obj_model.sett_current_running_no);
                obj.tbl_seller_id = SellerId;
                obj.created_on = DateTime.Now;
                dba.tbl_seller_setting.Add(obj);
                dba.SaveChanges();
            }
            else
            {
                get_details.type = 2;
                get_details.t_seller_prefix_code = obj_model.sett_prefix_code;
                get_details.t_running_no = Convert.ToInt16(obj_model.sett_current_running_no);
                get_details.current_running_no = obj_model.sett_current_running_no;
                dba.Entry(get_details).State = EntityState.Modified;
                dba.SaveChanges();
            }
            return RedirectToAction("Index", "SellerSetting");
        }
        public ActionResult SalesVoucher(model_tbl_seller_setting obj_model)
        {
            cf = new comman_function();
            if (!cf.session_check())
                return RedirectToAction("Login", "Home");
            int SellerId = Convert.ToInt32(Session["SellerID"]);
            var get_details = dba.tbl_seller_setting.Where(a => a.type == 3 && a.tbl_seller_id == SellerId).FirstOrDefault();
            if (get_details == null)
            {
                tbl_seller_setting obj = new tbl_seller_setting();
                obj.type = 3;
                obj.t_seller_prefix_code = obj_model.sales_prefix_code;
                obj.t_running_no = Convert.ToInt16(obj_model.sales_current_running_no);
                obj.current_running_no = Convert.ToInt16(obj_model.sales_current_running_no);
                obj.created_on = DateTime.Now;
                obj.tbl_seller_id = SellerId;
                dba.tbl_seller_setting.Add(obj);
                dba.SaveChanges();
            }
            else
            {
                get_details.type = 3;
                get_details.t_seller_prefix_code = obj_model.sales_prefix_code;
                get_details.t_running_no = Convert.ToInt16(obj_model.sales_current_running_no);
                get_details.current_running_no = obj_model.sales_current_running_no;
                dba.Entry(get_details).State = EntityState.Modified;
                dba.SaveChanges();

            }
            return RedirectToAction("Index", "SellerSetting");
        }
        public ActionResult ReturnVoucher(model_tbl_seller_setting obj_model)
        {
            cf = new comman_function();
            if (!cf.session_check())
                return RedirectToAction("Login", "Home");
            int SellerId = Convert.ToInt32(Session["SellerID"]);
            var get_details = dba.tbl_seller_setting.Where(a => a.type == 4 && a.tbl_seller_id == SellerId).FirstOrDefault();
            if (get_details == null)
            {
                tbl_seller_setting obj = new tbl_seller_setting();
                obj.type = 4;
                obj.t_seller_prefix_code = obj_model.return_prefix_code;
                obj.t_running_no = Convert.ToInt16(obj_model.return_current_running_no);
                obj.current_running_no = Convert.ToInt16(obj_model.return_current_running_no);
                obj.tbl_seller_id = SellerId;
                obj.created_on = DateTime.Now;
                dba.tbl_seller_setting.Add(obj);
                dba.SaveChanges();
            }
            else
            {
                get_details.type = 4;
                get_details.t_seller_prefix_code = obj_model.return_prefix_code;
                get_details.t_running_no = Convert.ToInt16(obj_model.return_current_running_no);
                get_details.current_running_no = obj_model.return_current_running_no;
                dba.Entry(get_details).State = EntityState.Modified;
                dba.SaveChanges();
            }
            return RedirectToAction("Index", "SellerSetting");
        }
        public ActionResult UpdateProfile1(model_tbl_seller_setting objmodel)
        {
            int SellerId = Convert.ToInt32(Session["SellerID"]);
            try
            {
                var get_details = db.tbl_sellers.Where(a => a.id == SellerId).FirstOrDefault();
                if (get_details != null)
                {
                    get_details.contact_person = objmodel.contact_person;
                    get_details.country = objmodel.country;
                    get_details.date_created = DateTime.Now;
                    get_details.address = objmodel.address;
                    get_details.business_name = objmodel.business_name;
                    get_details.city = objmodel.city;
                    get_details.gstin = objmodel.gstin;
                    get_details.mobile = objmodel.mobile;
                    get_details.pan = objmodel.pan;
                    get_details.pincode = objmodel.pincode;
                    get_details.state = objmodel.state;
                    //db.Entry(get_details).State = EntityState.Modified;
                    //db.SaveChanges();
                }
            }
            catch (Exception ex)
            {

            }
            return RedirectToAction("Index", "SellerSetting");
        }


        [HttpPost]
        public JsonResult UpdateProfile(model_tbl_seller_setting objmodel)
        {
            string Message1;
            bool flag = false;
            try
            {
                var get_details = db.tbl_sellers.Where(a => a.id == objmodel.ProfileID).FirstOrDefault();
                if (get_details != null)
                {
                    get_details.contact_person = objmodel.contact_person;
                    get_details.country = objmodel.country;
                    get_details.date_created = DateTime.Now;
                    get_details.address = objmodel.address;
                    get_details.business_name = objmodel.business_name;
                    get_details.city = objmodel.city;
                    get_details.gstin = objmodel.gstin;
                    get_details.mobile = objmodel.mobile;
                    get_details.pan = objmodel.pan;
                    get_details.pincode = objmodel.pincode;
                    get_details.state = objmodel.state;
                    db.Entry(get_details).State = EntityState.Modified;
                    db.SaveChanges();
                    flag = true;
                    Message1 = "Profile is updated successfully";
                }
                else
                {
                    flag = false;
                    Message1 = "some error occurred !!";
                }
            }                  
            catch (Exception e)
            {
                Message1 = "some error occurred !!";      
            }

            return new JsonResult { Data = new { Message = Message1, Status = flag } };
        }


    }
}
