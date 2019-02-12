using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Transactions;
using System.Web;
using SellerVendor.Models;
using System.Web.Mvc;

namespace SellerVendor.Common
{
    public class common_function :Controller
    {
        private SellerAdminContext db = new SellerAdminContext();
        private void send_mail(string message, string to, string from, string from_password, string subject)
        {
            try
            {

                SmtpClient smtpClient = new SmtpClient();
                AlternateView avHtml = AlternateView.CreateAlternateViewFromString
                    (message, null, MediaTypeNames.Text.Html);


                //create the mail message
                MailMessage mail = new MailMessage();
                //set the FROM address
                mail.From = new MailAddress(from, "Loyalty");
                //set the RECIPIENTS
                mail.To.Add(to);

                // mail.Bcc.Add("anil@softdew.co.in");
                //enter a SUBJECT
                mail.Subject = subject;//"TAXO Query Mail For " + custom_job_id;
                //Enter the message BODY
                mail.AlternateViews.Add(avHtml);


                // mail.Body = "v";// "Enter text for the e-mail here.";
                //set the mail server (default should be auth.smtp.1and1.co.uk)
                //SmtpClient smtp = new SmtpClient("auth.smtp.1and1.co.uk");
                //smtpClient.Host = "smtp.gmail.com"; // We use gmail as our smtp client
                smtpClient.Host = "relay-hosting.secureserver.net";
                smtpClient.Port = 25;

                //smtpClient.Host = "mail.daipl-mkg.com";
                //smtpClient.Port = 25;
                smtpClient.EnableSsl = false;
                smtpClient.UseDefaultCredentials = true;
                //Enter your full e-mail address and password
                smtpClient.Credentials = new NetworkCredential(from, from_password);
                //send the message 
                smtpClient.Send(mail);
            }
            catch (Exception ex)
            {
                tbl_exception_history obj = new tbl_exception_history();
                obj.Exception = ex.Message + "@@" + DateTime.Now;
                obj.Page = "Login Page";

                db.tbl_exception_history.Add(obj);
                db.SaveChanges();
            }

        }

        public bool test_send_mail(string message, string to, string from, string from_password, string subject)
        {
            bool mail_reponse = false;
            try
            {
                if (to == null)
                    to = "";

                // string[] strArr = Regex.Split(message, "@@@");




                SmtpClient smtpClient = new SmtpClient();

                AlternateView avHtml = AlternateView.CreateAlternateViewFromString
                    (message, System.Text.Encoding.UTF8, MediaTypeNames.Text.Html);



                //create the mail message
                MailMessage mail = new MailMessage();
                //set the FROM address
                mail.From = new MailAddress(from, "powersectorreforms");
                avHtml.TransferEncoding = System.Net.Mime.TransferEncoding.QuotedPrintable;
                mail.AlternateViews.Add(avHtml);
                mail.IsBodyHtml = true;
                mail.Body = message;

                //set the RECIPIENTS

                if (to != "")
                {
                    mail.To.Add(to);
                }


                // mail.Bcc.Add("bhupendra@softdew.co.in");
                //enter a SUBJECT
                mail.Subject = subject;
                //Enter the message BODY        

                smtpClient.Host = "smtp.gmail.com"; // We use gmail as our smtp client
                smtpClient.Port = 587;

                // smtpClient.Host = "hosting.secureserver.net";
                // smtpClient.Port = 80;
                mail.IsBodyHtml = true;
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = true;
                //Enter your full e-mail address and password
                //smtpClient.Credentials = new NetworkCredential(from, from_password);
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.Credentials = new NetworkCredential(from, from_password);
                //smtpClient.Timeout = 3000;
                //send the message 
                smtpClient.Send(mail);
                //smtpClient.Send(mail1);
                mail_reponse = true;
            }
            catch (Exception ex)
            {
                tbl_exception_history obj = new tbl_exception_history();
                obj.Exception = ex.Message + "@@" + DateTime.Now;
                obj.Page = "Login Page";

                db.tbl_exception_history.Add(obj);
                db.SaveChanges();
            }

            return mail_reponse;

        }



        public string AutoGeneratePassword()
        {
            string allowedChars = "";
            // allowedChars = "a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z,";
            // allowedChars += "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z,";
            allowedChars = "1,2,3,4,5,6,7,8,9,0";      //,!,@,#,$,%,&,?
            char[] sep = { ',' };
            string[] arr = allowedChars.Split(sep);
            string passwordString = "";
            string temp = "";
            Random rand = new Random();
            for (int i = 0; i < 5; i++)
            {

                temp = arr[rand.Next(0, arr.Length)];

                passwordString += temp;

            }

            return passwordString;
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

        public TransactionScope CreateTransactionScope()
        {
            var transactionOptions = new TransactionOptions
            {
                IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted,
                Timeout = TimeSpan.FromMinutes(10) //assume 10 min is the timeout time
            };
            return new TransactionScope(TransactionScopeOption.Required, transactionOptions);
        }
    }
}