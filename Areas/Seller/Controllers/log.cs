using SellerVendor.Areas.Seller.Models;
using SellerVendor.Areas.Seller.Models.DBContext;
using SellerVendor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SellerVendor.Areas.Seller.Controllers
{
    public class Writelog
    {
        public SellerContext db = new SellerContext();
       // public SellerAdminContext db = new SellerAdminContext();       
        public int write_exception_log(string UserID, string source_file, string func_name, DateTime dateoferror, Exception ex)
        {
            exception_history history = new exception_history();
            int retVal = 0;
            try
            {
                string Error = ex.Message.ToString();
                if (ex.InnerException != null)
                {
                    Error = ex.InnerException.Message;
                    if (ex.InnerException.InnerException != null)
                    {
                        Error = ex.InnerException.InnerException.Message;
                        if (ex.InnerException.InnerException.InnerException != null)
                        {
                            Error = ex.InnerException.InnerException.InnerException.Message;
                            if (ex.InnerException.InnerException.InnerException.InnerException != null)
                                Error = ex.InnerException.InnerException.InnerException.InnerException.Message;
                        }
                    }
                }
                
               
                int line_no = GetLineNumber(ex);
                Error += " at line no " + line_no;
                history.tbl_seller_id = UserID;
                history.source_file = source_file;
                history.page = func_name;
                history.exception_date = dateoferror;
                history.exception = Error;
                db.exception_history.Add(history);
                db.SaveChanges();
                retVal = 1;
                return retVal;
            }
            catch (Exception ff)
            {
                int line_no = GetLineNumber(ex);
                string Error = ex.Message;
                Error += " at line no " + line_no;
                history.tbl_seller_id = UserID;
                history.source_file = source_file;
                history.page = func_name;
                history.exception_date = dateoferror;
                history.exception = Error;
                db.exception_history.Add(history);
                db.SaveChanges();
                return retVal; 
            }
        }
        public int GetLineNumber(Exception ex)
        {
            var lineNumber = 0;
            const string lineSearch = ":line ";
            var index = ex.StackTrace.LastIndexOf(lineSearch);
            if (index != -1)
            {
                var lineNumberText = ex.StackTrace.Substring(index + lineSearch.Length);
                if (int.TryParse(lineNumberText, out lineNumber))
                {
                }
            }
            return lineNumber;
        }
    }
}