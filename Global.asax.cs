using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace SellerVendor
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //var razorEngine = ViewEngines.Engines.OfType<RazorViewEngine>().First();
            //razorEngine.ViewLocationFormats = razorEngine.ViewLocationFormats.Concat(new string[] 
            //{
            //    "~/Areas/Seller/Views/{1}/{0}.cshtml"                
            //   }).ToArray();
            //razorEngine.PartialViewLocationFormats = razorEngine.PartialViewLocationFormats.Concat(new string[]
            //{
            //    "~/Areas/Seller/Views/{1}/{0}.cshtml"              
            //    }).ToArray();





            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();
        }
    }
}