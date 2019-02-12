using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http;
using SellerVendor.Models;

namespace SellerVendor.Common
{
    public class MailMessageFormat
    {
        private SellerAdminContext db = new SellerAdminContext();

        [HttpPost]
        public bool SendMailToNewSeller(tbl_sellers objSeller)
        {
            try
            {
                string Subject = "Hello seller Your User Name and Password";
                string str = "";


                str = "<div class='main_box' style='max-width:600px; width:100%; margin:auto; font-family:Verdana, Geneva, sans-serif; border: solid 1px #CCCCCC; padding:20px; background: #f7f7f7; text-align:justify font-size:15px;'><table width='100%' border='0'><table width = '100%' border='0'><tbody><tr><td style = 'width:100%; float:left;'> " +
    "Dear " + objSeller.business_name + "</td></tr>" + "<tr><td><br>" + "User Name: " + objSeller.email + "<br>" + "Password: " + objSeller.db_pwd + "<br>" + "You can login and check: <a href = 'http://demo.raintree.online/'> Click Here</a></td></tr>" +
        "<tr><td style='width:100%; float:left; padding:20px 0 0 0;'>Welcome to Seller Admin ! We are glad to see your interest in our product.In order to give the best service we are flexible with cloud based solution and a stand alone software as per need. </td></tr>" +
        "<tr><td style = 'width:100%; float:left; padding:20px 0;' > We are a one stop destination where you get to experience a world class solution handling end to end HR processes of an organization in a seamless manner.Our endeavor is to provide our clients a robust solution which will maximize their productivity without adding to the overhead costs.</td></tr>" +
        "<tr><td style = 'width:100%; float:left; padding:0px 0 20px 0;' > Offerings: </td></tr>" +
        "<tr><td style = 'width:100%; float:left; padding:5px 0 0 0;' >• Complete HR solution handling employee processes right from hiring to exit</td></tr>" +
        "<tr><td style = 'width:100%; float:left; padding:5px 0 0 0;' >• Employee engagement tools</td></tr>" +
        "<tr><td style = 'width:100%; float:left; padding:5px 0 0 0;' >• On the fly documentation management</td></tr>" +
        "<tr><td style = 'width:100%; float:left; padding:5px 0 0 0;' >• Biometric record integration in real time </td></tr>" +
        "<tr><td style = 'width:100%; float:left; padding:5px 0 0 0;' >• Easy report generation</td></tr>" +
        "<tr><td style = 'width:100%; float:left; padding:5px 0 0 0;' >•  Asset Management and Performance Management modules</td></tr>" +
        "<tr><td style = 'width:100%; float:left; padding:5px 0 0 0;' >• Free upgrades for life time continuity</td></tr>" +
        "<tr><td style = 'width:100%; float:left; padding:15px 0 0 0;' > To get more understanding we would like you go through our demo followed by a 30 days trial pack which will help you take an informed decision.We also help in migrating your current data to our system so as to make the entire process hassle free. </td></tr>" +
        "<tr><td style = 'width:100%; float:left; padding:15px 0 0 0;' > We guarantee the best product for you at lowest price in market</td></tr>" +
        "<tr><td style = 'width:100%; float:left; padding:15px 0 0 0;' > Kindly let know for any assistance and we will be glad to attend your request on priority.</td></tr>" +
        "<tr><td style = 'width:100%; float:left; padding:15px 0 0 0;' > Looking forward to hear from you soon.</td></tr>" +
        "<tr><td style = 'width:100%; float:left; padding:15px 0 0 0;' > Regards,</td></tr>" +
        "<tr><td style = 'width:100%; float:left;' > Seller Admin Support Team</td></tr></tbody></table></div>";



                SendMail s1 = new SendMail(objSeller.email, ConfigurationManager.AppSettings["Email"].ToString(), "support@accountscorp.com,Sandeep.kumar1234@gmail.com,ajay.vijh@gmail.com", "vineetgahlot1989@gmail.com", Subject, str);//sharad@softdew.co.in
                return true;


            }
            catch (Exception exp)
            {
                return false;
            }
        }


    }
}