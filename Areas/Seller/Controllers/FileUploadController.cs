using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SellerVendor.Areas.Seller.Controllers
{
    public class FileUploadController : Controller
    {
        //
        // GET: /Seller/FileUpload/

        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ContentResult Upload(int ddl_marketplacebulk, int ddlFundType)
        {
            int SellerId = 22;
            string path = Server.MapPath("~/UploadExcel/" + SellerId + "/OrderSales/");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            foreach (string key in Request.Files)
            {
                HttpPostedFileBase postedFile = Request.Files[key];
                postedFile.SaveAs(path + postedFile.FileName);
            }

            return Content("Success");
        }
    }
}
