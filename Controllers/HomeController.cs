﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SellerVendor.Models;
using Newtonsoft.Json;
using System.Data.Entity.Infrastructure;
using System.Text;
using System.Data;
using System.Data.Entity.Validation;
using System.Diagnostics;
using SellerVendor.Common;
using System.IO;
using Microsoft.Vbe.Interop;
using SellerVendor.Areas.Seller.Models.DBContext;
using SellerVendor.Areas.Seller.Models;

namespace SellerVendor.Controllers
{
    public class HomeController : Controller
    {

        private SellerAdminContext db = new SellerAdminContext();
        public SellerContext dba = new SellerContext();
        common_function cf = new common_function();
        MailMessageFormat mmf = new MailMessageFormat();
        public ActionResult Index()
        {
            //var getdetails = db.m_color.ToList();
            //ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        /// <summary>
        /// This is for Login 
        /// </summary>
        /// <returns></returns>
        public ActionResult Login()
        {
            if (Request.Cookies["SellerCookie"] != null)
            {
                SellerVendor.Areas.Seller.Controllers.comman_function commmn = new Areas.Seller.Controllers.comman_function();
                if (commmn.session_check())
                {
                    return RedirectToAction("Index", "Seller/Seller");
                }

                //string[] cookies = Request.Cookies.AllKeys;
                //foreach (string cookie in cookies)
                //{
                //    Response.Cookies[cookie].Expires = DateTime.Now.AddDays(-1);
                //}
            }

            List<tbl_user_login> list = new List<tbl_user_login>();
            try
            {
                // list = db.tbl_user_login.ToList();
                //return View(list);
                return View();
            }
            catch (Exception ex)
            {
                string appPath = Server.MapPath("~/Upload_Log/log.txt");
                using (StreamWriter writer = new StreamWriter(appPath, true))
                {
                    writer.WriteLine("Important data line 2" + ex.InnerException.ToString());
                    writer.WriteLine("Important data line 1" + ex.Message.ToString());
                    writer.Dispose();
                }
            }
            return View();
        }

        public ActionResult UserLogin(LoginData obj)
        {
            try
            {
                if (Request.Cookies["SellerCookie"] != null)
                {
                    Request.Cookies.Remove("SellerCookie");
                }
                var user_details = db.tbl_user_login.Where(a => a.Email == obj.Email && a.password == obj.Password && a.isactive == 1).FirstOrDefault();
                //var user_details = db.tbl_user_login.Where(a => a.Email == "vineetgahlot1989@gmail.com" && a.password == "12345" && a.isactive == 1).FirstOrDefault();
                var seller_details = db.tbl_sellers.Where(a => a.id == user_details.tbl_sellers_id).ToList();
                if (seller_details != null)
                {
                    HttpCookie userInfo = new HttpCookie("SellerCookie");
                    Session["Email"] = userInfo["SellerEmail"] = seller_details[0].email;
                    Session["UserName"] = userInfo["SellerUserName"] = seller_details[0].business_name;
                    Session["SellerID"] = userInfo["SellerSellerID"] = seller_details[0].id.ToString();
                    Session["UserWelcomeName"] = userInfo["SellerUserWelcomeName"] = "Welcome" + "  " + seller_details[0].business_name;
                    Session["SellerType"] = userInfo["SellerSellerType"] = seller_details[0].tbl_type_id.ToString();
                    Session["TypeSeller"] = userInfo["SellerTypeSeller"] = seller_details[0].t_seller_typeid.ToString();
                    Session["WalletBalance"] = userInfo["SellerWalletBalance"] = user_details.wallet_balance.ToString();
                    Session["TotalOrders"] = userInfo["SellerTotalOrders"] = user_details.total_orders.ToString();
                    Session["State"] = userInfo["SellerState"] = seller_details[0].state.ToString();

                    if (user_details.wallet_balance != null && user_details.wallet_balance !=0)
                    {
                        if (user_details.wallet_balance < 0)
                        {
                            Session["Warning"] = 1; //"Your Account Balance is Low Please Add Some value....";
                        }
                        else
                        {
                            Session["Warning"] = 0;
                        }
                    }
                    if (user_details != null)
                    {
                        Session["LastLogin"] = userInfo["SellerLastLogin"] = Convert.ToDateTime(user_details.last_login).ToString("dd MMM hh:mm tt");
                        user_details.last_login = DateTime.Now;
                        db.Entry(user_details).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    userInfo.Expires = DateTime.Now.AddMonths(1); //.AddMonths(1); // AddYears(1);
                    Response.Cookies.Set(userInfo);
                    return new JsonResult { Data = user_details, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }
                else
                {
                    ViewBag.Message = "Invalid user name or password !";
                    return RedirectToAction("Home", new { Sessions = "2" });
                }
            }
            catch (Exception ex)
            {

                // return new JsonResult { Data = ex.Message.ToString() + " Data Invalid " + ex.InnerException.Message.ToString(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                return new JsonResult { Data = "Invalid user name or password !", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                //  throw;
            }
            //return new JsonResult { Data = user_details, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        /// <summary>
        /// This is for Seller
        /// </summary>
        /// <returns></returns>
        public ActionResult Seller()
        {
            return View();
        }

        public ActionResult TestResult()
        {
            List<tbl_user_login> list = new List<tbl_user_login>();
            try
            {
                list = db.tbl_user_login.ToList();
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message.ToString();
                throw;
            }
            return View(list);
        }


        public JsonResult FillSourceJoiningForSeller()
        {
            var GetSourceJoiningSeller = db.m_source_of_joining.ToList();
            return new JsonResult { Data = GetSourceJoiningSeller, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult FillPlanTypeForSeller()
        {
            var GetTypeSeller = db.tbl_m_plan.Where(a => a.isactive == 1).ToList();
            return new JsonResult { Data = GetTypeSeller, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult FillDatabseNameforSeller()
        {
            var GetDatabseDetails = db.m_database.Where(a => a.max_sellers != a.current_sellers).Take(1).ToList();
            return new JsonResult { Data = GetDatabseDetails, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult FillSellerTypeForSeller()
        {
            var GetSellerType = db.tbl_seller_type.Where(a => a.isactive == 1).ToList();
            return new JsonResult { Data = GetSellerType, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpPost]
        public JsonResult SaveSellerDetails(partial_tbl_sellers objSeller)
        {
            tbl_user_login objuserlogin = new tbl_user_login();
            string Message;
            bool flag = false;
            var get_details = db.tbl_sellers.Where(a => a.email == objSeller.email).FirstOrDefault();
            if (get_details == null)
            {
                try
                {

                    tbl_sellers obj_seller = new tbl_sellers();
                    obj_seller.business_name = objSeller.business_name;
                    obj_seller.address = objSeller.address;
                    obj_seller.city = objSeller.city;
                    obj_seller.contact_person = objSeller.contact_person;
                    obj_seller.country = objSeller.country;
                    obj_seller.created_by = objSeller.created_by;
                    obj_seller.date_created = DateTime.Now;
                    obj_seller.date_of_signup = DateTime.Now;
                    obj_seller.db_name = objSeller.db_name;
                    obj_seller.db_pwd = cf.AutoGeneratePassword();
                    obj_seller.email = objSeller.email;
                    obj_seller.gstin = objSeller.gstin;
                    obj_seller.isactive = 1;
                    obj_seller.mobile = objSeller.mobile;
                    obj_seller.m_source_of_joining_id = objSeller.m_source_of_joining_id;
                    obj_seller.pan = objSeller.pan;
                    obj_seller.pincode = objSeller.pincode;
                    obj_seller.state = objSeller.state;
                    obj_seller.tbl_type_id = objSeller.tbl_type_id;
                    obj_seller.t_seller_typeid = objSeller.t_seller_typeid;
                    obj_seller.referred_by = objSeller.referred_by;                 
                    db.tbl_sellers.Add(obj_seller);
                    db.SaveChanges();
                    flag = true;

                    var get_db_details = db.m_database.Where(a => a.id == obj_seller.db_name).FirstOrDefault();
                    if (get_db_details != null)
                    {
                        get_db_details.current_sellers += 1;
                        db.Entry(get_db_details).State = EntityState.Modified;
                        db.SaveChanges();
                    }

                    var getsellerdetails = db.tbl_sellers.Where(a => a.id == obj_seller.id).ToList();
                    if (getsellerdetails != null)
                    {
                        objuserlogin.Email = getsellerdetails[0].email;
                        objuserlogin.password = getsellerdetails[0].db_pwd;
                        objuserlogin.tbl_sellers_id = getsellerdetails[0].id;
                        objuserlogin.date_created = DateTime.Now;
                        objuserlogin.isactive = 1;
                        objuserlogin.wallet_balance = objSeller.wallet_balance;
                        objuserlogin.applied_plan_rate = objSeller.applied_plan_rate;
                        db.tbl_user_login.Add(objuserlogin);
                        db.SaveChanges();
                    }
                    //update                        
                    mmf.SendMailToNewSeller(obj_seller);
                    Message = "Seller is created successfully And Password is sent to your given Mail-Id";
                   
                    tbl_user_payment obj = new tbl_user_payment();
                    obj.user_id = objuserlogin.tbl_sellers_id;
                    obj.date_of_creation = DateTime.Now;
                    obj.amount_added = objuserlogin.wallet_balance;
                    obj.type = 1;
                    obj.description = "Reward";
                    db.tbl_user_payment.Add(obj);
                    db.SaveChanges();


                    tbl_seller_setting obj_seller_setting = new tbl_seller_setting();
                    obj_seller_setting.created_on = DateTime.Now;
                    obj_seller_setting.t_running_no = 1;
                    obj_seller_setting.tbl_seller_id = objuserlogin.tbl_sellers_id;
                    obj_seller_setting.is_active = 1;
                    dba.tbl_seller_setting.Add(obj_seller_setting);
                    dba.SaveChanges();
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                    throw;
                }
                return new JsonResult { Data = new { Message = Message, Status = flag } };
            }
            else
            {
                return new JsonResult { Data = new { Message = "Email Already Exist", Status = flag } };
            }

        }
        //public JsonResult SaveSellerDetails(tbl_sellers objSeller)
        //{
        //    tbl_user_login objuserlogin = new tbl_user_login();
        //    string Message;
        //    bool flag = false;
        //    try
        //    {
        //        objSeller.date_created = DateTime.Now;
        //        objSeller.date_of_signup = DateTime.Now;
        //        objSeller.isactive = 1;
        //        // ob.last_login = DateTime.Now;// DateTime.Parse("1/1/0001 12:00:00 AM");
        //        objSeller.db_pwd = cf.AutoGeneratePassword();
        //        db.tbl_sellers.Add(objSeller);
        //        db.SaveChanges();
        //        flag = true;

        //        var get_db_details = db.m_database.Where(a => a.id == objSeller.db_name).FirstOrDefault();
        //        if (get_db_details != null)
        //        {
        //            get_db_details.current_sellers += 1;
        //            db.Entry(get_db_details).State = EntityState.Modified;
        //            db.SaveChanges();
        //        }

        //        var getsellerdetails = db.tbl_sellers.Where(a => a.id == objSeller.id).ToList();
        //        if (getsellerdetails != null)
        //        {
        //            objuserlogin.Email = getsellerdetails[0].email;
        //            objuserlogin.password = getsellerdetails[0].db_pwd;
        //            objuserlogin.tbl_sellers_id = getsellerdetails[0].id;
        //            objuserlogin.date_created = DateTime.Now;
        //            objuserlogin.isactive = 1;
        //            db.tbl_user_login.Add(objuserlogin);
        //            db.SaveChanges();
        //        }
        //        //update                        
        //        mmf.SendMailToNewSeller(objSeller);
        //        Message = "Seller is created successfully";
        //    }
        //    catch (Exception e)
        //    {
        //        Debug.WriteLine(e.Message);
        //        throw;
        //    }
        //    return new JsonResult { Data = new { Message = Message, Status = flag } };
        //}


        [HttpPost]
        public JsonResult UpdateSellerDetails(tbl_sellers objtblSeller)
        {
            string Message;
            bool flag = false;
            try
            {
                var get_details = db.tbl_sellers.Where(a => a.id == objtblSeller.id).FirstOrDefault();
                if (get_details != null)
                {
                    get_details.address = objtblSeller.address;
                    get_details.business_name = objtblSeller.business_name;
                    get_details.email = objtblSeller.email;
                    get_details.db_pwd = objtblSeller.db_pwd;
                    get_details.mobile = objtblSeller.mobile;
                    get_details.contact_person = objtblSeller.contact_person;
                    db.Entry(get_details).State = EntityState.Modified;
                    db.SaveChanges();
                    flag = true;
                    Message = "Seller is updated successfully";
                }
                else
                {
                    flag = false;
                    Message = "some error occurred !!";
                }
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Debug.WriteLine(@"Entity of type has the following validation errors:", eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Debug.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
            catch (DbUpdateException dbu)
            {
                var exception = HandleDbUpdateException(dbu);
                throw exception;               
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw;
            }

            return new JsonResult { Data = new { Message = Message, Status = flag } };
        }

        public JsonResult DeleteSeller(string id)
        {
            try
            {
                int no = Convert.ToInt32(id);
                tbl_sellers objtblsellers = db.tbl_sellers.Where(a => a.id == no).FirstOrDefault();
                objtblsellers.isactive = 0;
                db.Entry(objtblsellers).State = EntityState.Modified;
                db.SaveChanges();

                var get_db_details = db.m_database.Where(a => a.id == objtblsellers.db_name).FirstOrDefault();
                if (get_db_details != null)
                {
                    get_db_details.current_sellers -= 1;
                    db.Entry(get_db_details).State = EntityState.Modified;
                    db.SaveChanges();

                }
                var getUserlogin = db.tbl_user_login.Where(a => a.tbl_sellers_id == objtblsellers.id).FirstOrDefault();
                if (getUserlogin != null)
                {
                    getUserlogin.isactive = 0;
                    db.Entry(getUserlogin).State = EntityState.Modified;
                    db.SaveChanges();
                }




                string Message = "Entry is deleted Successfully";
                return Json(Message, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exp)
            {
                return Json("Error in getting record !", JsonRequestBehavior.AllowGet);
            }


        }
        public JsonResult GetSeller()
        {
            var get_details = (from ob_tbl_sellers in db.tbl_sellers
                               join ob_m_source_of_joining in db.m_source_of_joining on ob_tbl_sellers.m_source_of_joining_id
                               equals ob_m_source_of_joining.id
                               join ob_tbl_country in db.tbl_country on
                                   ob_tbl_sellers.country equals ob_tbl_country.id
                               select new EntitiesWrapper
                               {
                                   ob_tbl_sellers = ob_tbl_sellers,
                                   ob_tbl_country = ob_tbl_country,
                                   ob_m_source_of_joining = ob_m_source_of_joining
                               }).OrderByDescending(a => a.ob_tbl_sellers.id).ToList();
            return new JsonResult { Data = get_details, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        /// <summary>
        /// Get County List
        /// </summary>
        /// <returns></returns>

        public JsonResult GetCountry()
        {
            var CountryList = db.tbl_country.Where(a => a.status == 1 && a.countrylevel == 0).OrderBy(a => a.countryname).ToList();
            return this.Json(CountryList, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get State List Counrtry Wise
        /// </summary>
        /// <param name="countrystateid"></param>
        /// <returns></returns>
        public JsonResult GetStateDetails(int countrystateid)
        {
            var statedetails = db.tbl_country.Where(m => m.parentid == countrystateid && m.countrylevel == 1 && m.status == 1).ToList();
            return this.Json(statedetails, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// This is for Source of joining
        /// </summary>
        /// <returns></returns>
        public ActionResult SourceOfJoining()
        {
            return View();
        }

        public JsonResult GetSourceJoiningList()
        {
            var getlist = db.m_source_of_joining.ToList();
            var JsonResult = Json(getlist, JsonRequestBehavior.AllowGet);
            return JsonResult;
        }

        public string SaveSourceJoining(string source)
        {
            string Message = "";
            try
            {
                m_source_of_joining ob_souceJoining = new m_source_of_joining();
                ob_souceJoining.source = source;
                db.m_source_of_joining.Add(ob_souceJoining);
                db.SaveChanges();
                Message = "Source Of Joining saved successfully !";
            }
            catch (Exception ex)
            {
                Message = "some error occurred";
            }
            return Message;
        }


        public string UpdateSourceJoining(m_source_of_joining obsouceJoining)
        {
            string Message = "";
            try
            {
                var getSourceJoiningDetails = db.m_source_of_joining.Where(a => a.id == obsouceJoining.id).FirstOrDefault();
                if (getSourceJoiningDetails != null)
                {
                    getSourceJoiningDetails.source = obsouceJoining.source;
                    db.Entry(getSourceJoiningDetails).State = EntityState.Modified;
                    db.SaveChanges();
                    Message = "Source Of Joining updated successfully !";
                }
            }
            catch (Exception ex)
            {
                Message = "some error occurred";
            }
            return Message;
        }

        public string DeleteSourceJoining(string id)
        {
            string Message = "";
            try
            {
                int number = Convert.ToInt32(id);
                var getSourceJoiningDetails = db.m_source_of_joining.Where(a => a.id == number).FirstOrDefault();
                if (getSourceJoiningDetails != null)
                {
                    db.m_source_of_joining.Remove(getSourceJoiningDetails);
                    db.SaveChanges();
                    Message = "Source Of Joining deleted successfully !";
                }
            }
            catch (Exception ex)
            {
                Message = "some error occurred";
            }
            return Message;
        }
        /// <summary>
        /// This is for Market Place
        /// </summary>
        /// <param name="dbu"></param>
        /// <returns></returns>
        /// 
        public ActionResult MarketPlace()
        {
            return View();
        }

        public JsonResult GetAllMarketPlaceList()
        {
            var marketList = db.m_marketplace.Where(a => a.isactive == 1).ToList();
            var JsonResult = Json(marketList, JsonRequestBehavior.AllowGet);
            return JsonResult;
        }

        [HttpPost]
        public JsonResult AddMarketPlace(m_marketplace Eve)
        {
            string Message, fileName, actualFileName;
            Message = fileName = actualFileName = string.Empty;
            bool flag = false;
            if (Eve != null)
            {
                if (Request.Files != null)
                {
                    if (Request.Files.Count > 0)
                    {
                        var file = Request.Files[0];
                        actualFileName = file.FileName;
                        string[] getfileName = actualFileName.Split('.');
                        actualFileName = getfileName[0];
                        fileName = actualFileName + DateTime.Now.ToString("yyyyMMddHHmmssfff") + Path.GetExtension(file.FileName);
                        var path = "";
                        path = Path.Combine(Server.MapPath("~/Upload/MarketPlaceLogo"), fileName);
                        string getpath = Url.Content(Path.Combine("~/Upload/MarketPlaceLogo", fileName));
                        file.SaveAs(path);
                        Eve.logo_path = getpath;
                    }
                    try
                    {
                        Eve.isactive = 1;
                        Eve.date_created = DateTime.Now;
                        db.m_marketplace.Add(Eve);
                        db.SaveChanges();
                        flag = true;
                        Message = "Addition of Market Place sucessfull !";
                    }

                    catch (DbUpdateException dbu)
                    {
                        var exception = HandleDbUpdateException(dbu);
                        Message = "Addition of Event Unsucessfull !";
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e.Message);
                        Message = "Addition of Event Unsucessfull !";
                    }
                }
            }
            else
            {
                Message = "Addition of Event unsucessfull !";
            }

            return new JsonResult { Data = new { Message = Message, Status = flag } };
        }


        public JsonResult GetMarketPlacebyId(string id)
        {
            try
            {
                int no = Convert.ToInt32(id);
                var mMarketPlaceList = db.m_marketplace.Find(no);
                return Json(mMarketPlaceList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exp)
            {
                return Json("Error in getting record !", JsonRequestBehavior.AllowGet);
            }
        }
        public string UpdateMarketPlace(m_marketplace Eve)
        {
            string Message, fileName, actualFileName;
            Message = fileName = actualFileName = string.Empty;
            bool flag = false;
            if (Eve != null)
            {
                var getMarketPlaceDetails = db.m_marketplace.Where(a => a.id == Eve.id).FirstOrDefault();
                if (getMarketPlaceDetails != null)
                {
                    if (Request.Files != null)
                    {
                        if (Request.Files.Count > 0)
                        {
                            var file = Request.Files[0];
                            actualFileName = file.FileName;
                            string[] getfileName = actualFileName.Split('.');
                            actualFileName = getfileName[0];
                            fileName = actualFileName + DateTime.Now.ToString("yyyyMMddHHmmssfff") + Path.GetExtension(file.FileName);
                            var path = "";
                            path = Path.Combine(Server.MapPath("~/Upload/MarketPlaceLogo"), fileName);
                            string getpath = Url.Content(Path.Combine("~/Upload/MarketPlaceLogo", fileName));
                            file.SaveAs(path);
                            getMarketPlaceDetails.logo_path = getpath;
                        }
                        try
                        {
                            getMarketPlaceDetails.name = Eve.name;
                            getMarketPlaceDetails.api_url = Eve.api_url;
                            getMarketPlaceDetails.date_created = DateTime.Now;
                            db.Entry(getMarketPlaceDetails).State = EntityState.Modified;
                            db.SaveChanges();
                            flag = true;
                            Message = "Update of Market Place sucessfull !";
                        }
                        catch (DbUpdateException dbu)
                        {
                            var exception = HandleDbUpdateException(dbu);
                            Message = "Update of Market Place Unsucessfull !";
                        }
                        catch (Exception e)
                        {
                            Debug.WriteLine(e.Message);
                            Message = "Update of Market Place Unsucessfull !";
                        }
                    }
                }
                Message = "Update of Market Place sucessfull !";
            }
            else
            {
                Message = "Update of Market Place unsucessfull !";
            }
            return Message;
        }


        public JsonResult DeleteMarketPlaceDetail(string id)
        {
            try
            {
                string Message = "";
                int no = Convert.ToInt32(id);
                m_marketplace marketplacedetails = db.m_marketplace.Where(a => a.id == no).FirstOrDefault();
                if (marketplacedetails != null)
                {
                    marketplacedetails.isactive = 0;
                    db.Entry(marketplacedetails).State = EntityState.Modified;
                    db.SaveChanges();
                    Message = "Entry is deleted Successfully";
                }
                else
                {
                    Message = "Entry is not deleted Successfully";
                }

                return Json(Message, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exp)
            {
                return Json("Error in getting record !", JsonRequestBehavior.AllowGet);
            }
        }

        private Exception HandleDbUpdateException(DbUpdateException dbu)
        {
            var builder = new StringBuilder("A DbUpdateException was caught while saving changes. ");

            try
            {
                foreach (var result in dbu.Entries)
                {
                    builder.AppendFormat("Type: {0} was part of the problem. ", result.Entity.GetType().Name);
                }
            }
            catch (Exception e)
            {
                builder.Append("Error parsing DbUpdateException: " + e.ToString());
            }

            string message = builder.ToString();
            return new Exception(message, dbu);
        }

        public ActionResult Logout()
        {
            string[] cookies = Request.Cookies.AllKeys;
            foreach (string cookie in cookies)
            {
                if (cookie == "SellerCookie")
                    Response.Cookies[cookie].Expires = DateTime.Now.AddDays(-1);
            }
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Login", "Home");
        }
    }
}
