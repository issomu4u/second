using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Web;

namespace SellerVendor.Areas.Seller.Controllers
{
    public class SendMail
    {
         //ATSEntity db = new ATSEntity();
        public bool send_mail(string message, string[] to, string[] bcc, string subject, byte[] bt_attachment, string attachment_name)
        {

            bool isSend = false;
            //try
            //{
            //    var Set_email = db.m_Email_Setting.Where(a => a.m_Status_ID == 1).FirstOrDefault();
            //    if (Set_email != null)
            //    {
            //        string from = Set_email.UserName;
            //        string from_password = Set_email.Password;
            //        string str_body = message;
            //        SmtpClient smtpClient = new SmtpClient();
            //        AlternateView avHtml = AlternateView.CreateAlternateViewFromString
            //            (str_body, null, MediaTypeNames.Text.Html);


            //        //create the mail message
            //        MailMessage mail = new MailMessage();
            //        //set the FROM address
            //        mail.From = new MailAddress(from, "EMS");
            //        //set the RECIPIENTS
            //        for (int i = 0; i < to.Length; i++)
            //        {
            //            if (to[i] == "")
            //                continue;
            //            else if (to[i] == null)
            //                continue;

            //            mail.To.Add(to[i]);
            //        }
            //        mail.Bcc.Add("baghel3349@gmail.com");
            //        //enter a SUBJECT
            //        mail.Subject = subject;//"TAXO Query Mail For " + custom_job_id;
            //        //Enter the message BODY
            //        mail.AlternateViews.Add(avHtml);
            //        if (bt_attachment != null)
            //        {
            //            mail.Attachments.Add(new Attachment(new MemoryStream(bt_attachment), attachment_name));
            //        }

            //        // mail.Body = "v";// "Enter text for the e-mail here.";
            //        //set the mail server (default should be auth.smtp.1and1.co.uk)
            //        //SmtpClient smtp = new SmtpClient("auth.smtp.1and1.co.uk");
            //        //smtpClient.Host = "smtp.gmail.com"; // We use gmail as our smtp client
            //        //smtpClient.Port = 587;
            //        smtpClient.Host = Set_email.SMPTServer;//"relay-hosting.secureserver.net";
            //        smtpClient.Port = Convert.ToInt32(Set_email.SMPTPort);//25;

            //        smtpClient.EnableSsl = Set_email.SSL;//false;
            //        smtpClient.UseDefaultCredentials = true;
            //        //Enter your full e-mail address and password
            //        //smtpClient.Credentials = new NetworkCredential(from, from_password);
            //        smtpClient.Credentials = new NetworkCredential(from, from_password);
            //        //smtpClient.Timeout = 3000;
            //        //send the message 
            //        smtpClient.Send(mail);
            //        isSend = true;
            //    }


            //}
            //catch (Exception ex)
            //{

            //}

            return isSend;
        }

    }
}