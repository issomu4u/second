using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace SellerVendor.Common
{
    public class SendMail
    {
        public SendMail()
        {
        }

        public SendMail(string toList, string from, string ccList, string bcc, string subject, string body, List<HttpPostedFileBase> FileList = null)
        {

            if (FileList == null || FileList.Count() == 0)
            {
                SendMailByGmail(toList, from, ccList, bcc, subject, body);
            }
            else if (FileList.Count() > 0)
            {
                SendMailByGmail(toList, from, ccList, bcc, subject, body, FileList);
            }

        }
        private string SendMailByGmail(string toList, string from, string ccList, string bcc, string subject, string body)
        {

            MailMessage message = new MailMessage();
            SmtpClient smtpClient = new SmtpClient();
            string msg = string.Empty;
            try
            {
                string contentID = "";
                MailAddress fromAddress = new MailAddress(from);
                message.From = fromAddress;
                message.To.Add(toList);
                if (ccList != null && ccList != string.Empty)
                    message.CC.Add(ccList);
                if (bcc != null && bcc != string.Empty)
                    message.Bcc.Add(bcc);
                message.Subject = subject;
                message.IsBodyHtml = true;
                message.Body = body;
                //smtpClient.Host = "mail.Mindztechnology.com";   // We use gmail as our smtp client
                smtpClient.Host = ConfigurationManager.AppSettings["Host"].ToString();   // We use gmail as our smtp client
                smtpClient.Port = Convert.ToInt32(ConfigurationManager.AppSettings["Port"]);
                smtpClient.EnableSsl = true;
                //smtpClient.EnableSsl = true;//For ClientSide AWS
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["UserName"].ToString(), ConfigurationManager.AppSettings["Pwd"].ToString());

                smtpClient.Send(message);
                msg = "Successful<BR>";
                ErrorLog.SendErrorToText("Vineet");
            }
            catch (Exception ex)
            {
                ErrorLog.SendErrorToText(ex);
                msg = ex.Message;

            }
            return msg;
        }
        private string SendMailByGmail(string toList, string from, string ccList, string bcc, string subject, string body,                  List<HttpPostedFileBase> FileList = null)
        {

            MailMessage message = new MailMessage();
            SmtpClient smtpClient = new SmtpClient();
            string msg = string.Empty;
            try
            {
                MailAddress fromAddress = new MailAddress(from);
                message.From = fromAddress;
                message.To.Add(toList);
                if (ccList != null && ccList != string.Empty)
                    message.CC.Add(ccList);
                if (bcc != null && bcc != string.Empty)
                    message.Bcc.Add(bcc);
                message.Subject = subject;
                message.IsBodyHtml = true;
                message.Body = body;
                if (FileList != null)
                {
                    foreach (var file in FileList)
                    {
                        Attachment attached = new Attachment(file.InputStream, file.FileName, file.ContentType);
                        message.Attachments.Add(attached);
                    }
                }
                //smtpClient.Host = "mail.Mindztechnology.com";   // We use gmail as our smtp client
                smtpClient.Host = ConfigurationManager.AppSettings["Host"].ToString();   // We use gmail as our smtp client
                smtpClient.Port = Convert.ToInt32(ConfigurationManager.AppSettings["Port"]);
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["UserName"].ToString(), ConfigurationManager.AppSettings["Pwd"].ToString());
                smtpClient.Send(message);
                msg = "Successful<BR>";
                ErrorLog.SendErrorToText("Prinsu");
            }
            catch (Exception ex)
            {
                ErrorLog.SendErrorToText(ex);
                msg = ex.Message;
            }
            return msg;
        }      
    }
}