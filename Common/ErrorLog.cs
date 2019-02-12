using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace SellerVendor.Common
{
    public class ErrorLog
    {
        private static String ErrorlineNo, Errormsg, extype, exurl, hostIp, ErrorLocation, HostAdd;
        public static void SendErrorToText(Exception ex)
        {
            var line = Environment.NewLine + Environment.NewLine;

            ErrorlineNo = ex.StackTrace;
            Errormsg = ex.GetType().Name.ToString();
            extype = ex.GetType().ToString();
            ErrorLocation = ex.Message.ToString();

            try
            {
                //string filepath = context.Current.Server.MapPath("~/ExceptionDetailsFile/");  //Text File Path

                //if (!Directory.Exists(filepath))
                //{
                //    Directory.CreateDirectory(filepath);

                //}D:\Diet Clinic\CorprateRecuritmentApi\CorporateRecruitmentApi
                //string filepath = @"D:/NewHRMSAssets(11-04-2016)/Frankfinn HRMS/Api/CorprateRecuritmentApi/CorporateRecruitmentApi/Temp/errorlog.txt";

                //string filepath = @"E:/Frankfinn HRMS/Api/Temp/errorlog.txt";
                string filepath = @"D:/Vineet/SellerVendor/SellerVendor/Temp/errorlog.txt";
                //Text File Name
                if (!File.Exists(filepath))
                {


                    File.Create(filepath).Dispose();

                }
                using (StreamWriter sw = File.AppendText(filepath))
                {
                    string error = "Log Written Date:" + " " + DateTime.Now.ToString() + line + "Error Line No :" + " " + ErrorlineNo + line + "Error Message:" + " " + Errormsg + line + "Exception Type:" + " " + extype + line + "Error Location :" + " " + ErrorLocation + line + " Error Page Url:" + " " + exurl + line + "User Host IP:" + " " + hostIp + line;
                    sw.WriteLine("-----------Exception Details on " + " " + DateTime.Now.ToString() + "-----------------");
                    sw.WriteLine("-------------------------------------------------------------------------------------");
                    sw.WriteLine(line);
                    sw.WriteLine(error);
                    sw.WriteLine("--------------------------------*End*------------------------------------------");
                    sw.WriteLine(line);
                    sw.Flush();
                    sw.Close();

                }

            }
            catch (Exception e)
            {
                e.ToString();

            }
        }

        public static void SendErrorToText(string ex)
        {
            var line = 1;

            ErrorlineNo = "VineetGahlot";
            Errormsg = "VineetGahlot";
            extype = "VineetGahlot";
            ErrorLocation = "VineetGahlot";

            try
            {
                //string filepath = context.Current.Server.MapPath("~/ExceptionDetailsFile/");  //Text File Path

                //if (!Directory.Exists(filepath))
                //{
                //    Directory.CreateDirectory(filepath);

                //}D:\Diet Clinic\CorprateRecuritmentApi\CorporateRecruitmentApi
                //string filepath = @"E:/Frankfinn HRMS/Api/Temp/errorlog.txt";
                string filepath = @"D:/Vineet/SellerVendor/SellerVendor/Temp/errorlog.txt";
                //string filepath = @"D:/NewHRMSAssets(11-04-2016)/Frankfinn HRMS/Api/CorprateRecuritmentApi/CorporateRecruitmentApi/Temp/errorlog.txt";

                //Text File Name
                if (!File.Exists(filepath))
                {


                    File.Create(filepath).Dispose();

                }
                using (StreamWriter sw = File.AppendText(filepath))
                {
                    string error = "Log Written Date:" + " " + DateTime.Now.ToString() + line + "Error Line No :" + " " + ErrorlineNo + line + "Error Message:" + " " + Errormsg + line + "Exception Type:" + " " + extype + line + "Error Location :" + " " + ErrorLocation + line + " Error Page Url:" + " " + exurl + line + "User Host IP:" + " " + hostIp + line;
                    sw.WriteLine("-----------Exception Details on " + " " + DateTime.Now.ToString() + "-----------------");
                    sw.WriteLine("-------------------------------------------------------------------------------------");
                    sw.WriteLine(line);
                    sw.WriteLine(error);
                    sw.WriteLine("--------------------------------*End*------------------------------------------");
                    sw.WriteLine(line);
                    sw.Flush();
                    sw.Close();

                }

            }
            catch (Exception e)
            {
                e.ToString();

            }
        }
    }
}