using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SellerVendor.Common;
using SellerVendor.Models;
using System.Data.Entity;
using SellerVendor.Areas.Seller.Models.DBContext;

namespace SellerVendor.Controllers
{
    public class CategoryController : Controller
    {
        
        SellerAdminContext db = new SellerAdminContext();
        common_function cf = new common_function();
        MailMessageFormat mmf = new MailMessageFormat();

        /// <summary>
        /// Category Page
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }


        public JsonResult AddCategory(m_item_category AddCategory)
        {
          
            string Msg = "";
            try
            {
                cf = new common_function();
                using (var transaction = cf.CreateTransactionScope())
                {

                    if (AddCategory.id == 0)
                    {
                        m_item_category m_Category_Details = new m_item_category();

                        m_Category_Details.category_name = AddCategory.category_name;
                        m_Category_Details.tax_rate = AddCategory.tax_rate;

                        m_Category_Details.updated_on = DateTime.Now;
                        m_Category_Details.isactive = 1;
                        db.m_item_category.Add(m_Category_Details);
                        db.SaveChanges();              
                        Msg = "Data has been saved successfully.";

                        //msg.id = 1;
                        //msg.Message = Message.DataSaved;
                    }
                    else
                    {
                        var category_details = db.m_item_category.Where(a => a.id == AddCategory.id).FirstOrDefault();

                        if (category_details != null)
                        {
                            category_details.category_name = AddCategory.category_name;
                            category_details.tax_rate = AddCategory.tax_rate;
                           
                            db.Entry(category_details).State = EntityState.Modified;
                            db.SaveChanges();                                                                
                            Msg = "Data has been updated successfully.";
                        }
                    }
                    transaction.Complete();

                }

            }
            catch (Exception ex)
            {

                Msg = "Data has not been Saved .";

               
            }
            return Json(Msg, JsonRequestBehavior.AllowGet);               
        }

        public PartialViewResult CategoryDetails()
        {
            var get_details = db.m_item_category.Where(a => a.isactive == 1).OrderByDescending(a => a.id).ToList();
            return PartialView(get_details);
        }

        public JsonResult fill_data(int id)
        {

            // var get_details = db.m_item_category.Where(a =>a.id == id).OrderByDescending(a => a.id).ToList();
            var get_details = (from ob_category in db.m_item_category.Where(a => a.id == id)
                               select new EntitiesWrapper
                               {
                                   ob_m_item_category = ob_category

                               }).FirstOrDefault();
            return Json(get_details);
        }

        public JsonResult Delete_Category(int id)
        {
            
            string Message = "";
            try
            {
                cf = new common_function();
                using (var transaction = cf.CreateTransactionScope())
                {
                    m_item_category category_details = db.m_item_category.Where(a => a.id == id).FirstOrDefault();
                    category_details.isactive = 0;
                    db.Entry(category_details).State = EntityState.Modified;
                    db.SaveChanges();
                    Message = "Delete Sucessfully";
                   
                    transaction.Complete();
                }
                
            }
            catch (Exception ex)
            {

            }

            return Json(Message, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Category Tree View
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public PartialViewResult CategoryTreeView()
        {
            ViewBag.CategoryId = 1;
            categoryTreeView tv = new categoryTreeView();
            tv.list_parent_category = new List<parentCategory>();


            var categorydetailslist = db.m_item_category.Where(a =>a.isactive==1).ToList();

            foreach (var data in categorydetailslist)
            {
                parentCategory objCategoryDatail = new parentCategory();
                int created_by_id = data.id;
                objCategoryDatail.id = created_by_id;
                objCategoryDatail.category_name = data.category_name;


                objCategoryDatail.list_sub_category = new List<parentSubCategory>();
                var subcategory_all = db.m_item_subcategory.Where(a => a.m_item_category_id == created_by_id && a.isactive == 1).ToList();
                foreach (var data1 in subcategory_all)
                {
                    parentSubCategory subcategory = new parentSubCategory();
                    subcategory.subcategory_name = data1.subcategory_name;
                    subcategory.id = data1.id;
                    subcategory.hsn_code = data1.hsn_code;
                    subcategory.tax_rate = Convert.ToDouble(data1.tax_rate);
                    objCategoryDatail.list_sub_category.Add(subcategory);
                }
                tv.list_parent_category.Add(objCategoryDatail);

            }
            return PartialView(tv);
        }


      


        /// <summary>
        /// Sub Category  Details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult AddSubCategory()
        {            
                categoryTreeView tv = new categoryTreeView();
                //tv = CategoryTreeView(id);
                return View(tv);          
        }

        public JsonResult FillCategoryForm(int id)
        {
            var get_details = db.m_item_category.Where(a => a.id == id).FirstOrDefault();
            return Json(get_details);
        }

        public JsonResult AddSubCategoryDeatils(m_item_subcategory AddSubCategory)
        {

            string Msg = "";
            try
            {
                cf = new common_function();
                using (var transaction = cf.CreateTransactionScope())
                {

                    if (AddSubCategory.id == 0)
                    {
                        m_item_subcategory m_subCategory_Details = new m_item_subcategory();

                        m_subCategory_Details.m_item_category_id = AddSubCategory.m_item_category_id;
                        m_subCategory_Details.subcategory_name = AddSubCategory.subcategory_name;
                        m_subCategory_Details.hsn_code = AddSubCategory.hsn_code;
                        m_subCategory_Details.tax_rate = AddSubCategory.tax_rate;

                        m_subCategory_Details.updated_on = DateTime.Now;
                        m_subCategory_Details.isactive = 1;
                        db.m_item_subcategory.Add(m_subCategory_Details);
                        db.SaveChanges();

                        Msg = "Data has been saved successfully.";
                        
                    }
                    else
                    {
                        var subcategory_details = db.m_item_subcategory.Where(a => a.id == AddSubCategory.id).FirstOrDefault();
                        if (subcategory_details != null)
                        {
                            subcategory_details.m_item_category_id = AddSubCategory.m_item_category_id;
                            subcategory_details.subcategory_name = AddSubCategory.subcategory_name;
                            subcategory_details.hsn_code = AddSubCategory.hsn_code;
                            subcategory_details.tax_rate = AddSubCategory.tax_rate;
                            subcategory_details.updated_on = DateTime.Now;

                            db.Entry(subcategory_details).State = EntityState.Modified;
                            db.SaveChanges();
                            Msg = "Data has been Updated successfully.";
                        }
                    }
                    transaction.Complete();
                }
            }
            catch (Exception ex)
            {
                Msg = "Data has not been Saved .";
            }
            return Json(Msg, JsonRequestBehavior.AllowGet);
        }


     


        public JsonResult FillNodeForm(int id)
        {
            var get_details = (from ob_m_item_subcategory in db.m_item_subcategory.Where(a => a.id == id)
                               join ob_m_item_category in db.m_item_category on
                               ob_m_item_subcategory.m_item_category_id equals ob_m_item_category.id
                               select new
                               {
                                   ob_m_item_subcategory = ob_m_item_subcategory,
                                   ob_m_item_category = ob_m_item_category,

                               }).FirstOrDefault();
            return Json(get_details);
        }

        public JsonResult Delete_SubCategory(int id)
        {
            string Message = "";
            try
            {
                cf = new common_function();
                using (var transaction = cf.CreateTransactionScope())
                {
                    m_item_subcategory subcategory_details = db.m_item_subcategory.Where(a => a.id == id).FirstOrDefault();
                    subcategory_details.isactive = 0;
                    db.Entry(subcategory_details).State = EntityState.Modified;
                    db.SaveChanges();
                    Message = "Delete Sucessfully";
                    transaction.Complete();
                }
            }
            catch (Exception ex)
            {

            }
            return Json(Message, JsonRequestBehavior.AllowGet);
        }
    }
}
