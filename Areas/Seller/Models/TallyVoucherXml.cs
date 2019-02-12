using SellerVendor.Areas.Seller.Models.DBContext;
using SellerVendor.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;

namespace SellerVendor.Areas.Seller.Models
{
    public class TallyVoucherXml
    {
        public SellerContext dba = new SellerContext();
        public SellerAdminContext db = new SellerAdminContext();

        //VCHKEY=\"df13ea43-535b-411e-a3ae-09a30f52223d-0000a7f7:00000008\"

        string voucherHeader = 
"<ENVELOPE>\n" +
" <HEADER>\n" +
"  <TALLYREQUEST>Import Data</TALLYREQUEST>\n" +
" </HEADER>\n" +
" <BODY>\n" +
"  <IMPORTDATA>\n" +
"   <REQUESTDESC>\n" +
"    <REPORTNAME>Vouchers</REPORTNAME>\n" +
"    <STATICVARIABLES>\n" +
"      <SVCURRENTCOMPANY>#companyname#</SVCURRENTCOMPANY>\n" +
"    </STATICVARIABLES>\n" +
"   </REQUESTDESC>\n" +
"   <REQUESTDATA>\n" +
"    <TALLYMESSAGE xmlns:UDF=\"TallyUDF\">" + "\n" +
"      <VOUCHER REMOTEID=\"#uniqueguid#\" VCHKEY=\"e44e5438-bbf7-4b26-94b2-672e136f7320-0000a83b:00000500\" VCHTYPE=\"Journal 1\" ACTION=\"Create\" OBJVIEW=\"Accounting Voucher View\">" + "\n" +
"       <OLDAUDITENTRYIDS.LIST TYPE=\"Number\">" + "\n" +
"         <OLDAUDITENTRYIDS>-1</OLDAUDITENTRYIDS>" + "\n" +
"       </OLDAUDITENTRYIDS.LIST>" + "\n" +
"       <DATE>#SettlementDate#</DATE>" + "\n" +
"       <GUID>#uniqueguid#</GUID>" + "\n" +
"       <NARRATION>#NarrartionNo#</NARRATION>" + "\n" +
"       <VOUCHERTYPENAME>Journal 1</VOUCHERTYPENAME>" + "\n" +
"       <VOUCHERNUMBER>#marketplacevoucher#</VOUCHERNUMBER>" + "\n" +
"       <PARTYLEDGERNAME>#marketplacepartyledgername#</PARTYLEDGERNAME>" + "\n" +
"       <CSTFORMISSUETYPE/>" + "\n" +
"       <CSTFORMRECVTYPE/>" + "\n" +
"       <FBTPAYMENTTYPE>Default</FBTPAYMENTTYPE>" + "\n" +
"       <PERSISTEDVIEW>Accounting Voucher View</PERSISTEDVIEW>" + "\n" +
"       <VCHGSTCLASS/>" + "\n" +
"       <DIFFACTUALQTY>No</DIFFACTUALQTY>" + "\n" +
"       <ISMSTFROMSYNC>No</ISMSTFROMSYNC>" + "\n" +
"       <ASORIGINAL>No</ASORIGINAL>" + "\n" +
"       <AUDITED>No</AUDITED>" + "\n" +
"       <FORJOBCOSTING>No</FORJOBCOSTING>" + "\n" +
"       <ISOPTIONAL>No</ISOPTIONAL>" + "\n" +
"       <EFFECTIVEDATE>#SettlementDate#</EFFECTIVEDATE>" + "\n" +
"       <USEFOREXCISE>No</USEFOREXCISE>" + "\n" +
"       <ISFORJOBWORKIN>No</ISFORJOBWORKIN>" + "\n" +
"       <ALLOWCONSUMPTION>No</ALLOWCONSUMPTION>" + "\n" +
"       <USEFORINTEREST>No</USEFORINTEREST>" + "\n" +
"       <USEFORGAINLOSS>No</USEFORGAINLOSS>" + "\n" +
"       <USEFORGODOWNTRANSFER>No</USEFORGODOWNTRANSFER>" + "\n" +
"       <USEFORCOMPOUND>No</USEFORCOMPOUND>" + "\n" +
"       <USEFORSERVICETAX>No</USEFORSERVICETAX>" + "\n" +
"       <ISEXCISEVOUCHER>No</ISEXCISEVOUCHER>" + "\n" +
"       <EXCISETAXOVERRIDE>No</EXCISETAXOVERRIDE>" + "\n" +
"       <USEFORTAXUNITTRANSFER>No</USEFORTAXUNITTRANSFER>" + "\n" +
"       <EXCISEOPENING>No</EXCISEOPENING>" + "\n" +
"       <USEFORFINALPRODUCTION>No</USEFORFINALPRODUCTION>" + "\n" +
"       <ISTDSOVERRIDDEN>No</ISTDSOVERRIDDEN>" + "\n" +
"       <ISTCSOVERRIDDEN>No</ISTCSOVERRIDDEN>" + "\n" +
"       <ISTDSTCSCASHVCH>No</ISTDSTCSCASHVCH>" + "\n" +
"       <INCLUDEADVPYMTVCH>No</INCLUDEADVPYMTVCH>" + "\n" +
"       <ISSUBWORKSCONTRACT>No</ISSUBWORKSCONTRACT>" + "\n" +
"       <ISVATOVERRIDDEN>No</ISVATOVERRIDDEN>" + "\n" +
"       <IGNOREORIGVCHDATE>No</IGNOREORIGVCHDATE>" + "\n" +
"       <ISSERVICETAXOVERRIDDEN>No</ISSERVICETAXOVERRIDDEN>" + "\n" +
"       <ISISDVOUCHER>No</ISISDVOUCHER>" + "\n" +
"       <ISEXCISEOVERRIDDEN>No</ISEXCISEOVERRIDDEN>" + "\n" +
"       <ISEXCISESUPPLYVCH>No</ISEXCISESUPPLYVCH>" + "\n" +
"       <ISGSTOVERRIDDEN>No</ISGSTOVERRIDDEN>" + "\n" +
"       <GSTNOTEXPORTED>No</GSTNOTEXPORTED>" + "\n" +
"       <ISVATPRINCIPALACCOUNT>No</ISVATPRINCIPALACCOUNT>" + "\n" +
"       <ISSHIPPINGWITHINSTATE>No</ISSHIPPINGWITHINSTATE>" + "\n" +
"       <ISCANCELLED>No</ISCANCELLED>" + "\n" +
"       <HASCASHFLOW>No</HASCASHFLOW>" + "\n" +
"       <ISPOSTDATED>No</ISPOSTDATED>" + "\n" +
"       <USETRACKINGNUMBER>No</USETRACKINGNUMBER>" + "\n" +
"       <ISINVOICE>No</ISINVOICE>" + "\n" +
"       <MFGJOURNAL>No</MFGJOURNAL>" + "\n" +
"       <HASDISCOUNTS>No</HASDISCOUNTS>" + "\n" +
"       <ASPAYSLIP>No</ASPAYSLIP>" + "\n" +
"       <ISCOSTCENTRE>No</ISCOSTCENTRE>" + "\n" +
"       <ISSTXNONREALIZEDVCH>No</ISSTXNONREALIZEDVCH>" + "\n" +
"       <ISEXCISEMANUFACTURERON>No</ISEXCISEMANUFACTURERON>" + "\n" +
"       <ISBLANKCHEQUE>No</ISBLANKCHEQUE>" + "\n" +
"       <ISVOID>No</ISVOID>" + "\n" +
"       <ISONHOLD>No</ISONHOLD>" + "\n" +
"       <ORDERLINESTATUS>No</ORDERLINESTATUS>" + "\n" +
"       <VATISAGNSTCANCSALES>No</VATISAGNSTCANCSALES>" + "\n" +
"       <VATISPURCEXEMPTED>No</VATISPURCEXEMPTED>" + "\n" +
"       <ISVATRESTAXINVOICE>No</ISVATRESTAXINVOICE>" + "\n" +
"       <VATISASSESABLECALCVCH>No</VATISASSESABLECALCVCH>" + "\n" +
"       <ISVATDUTYPAID>Yes</ISVATDUTYPAID>" + "\n" +
"       <ISDELIVERYSAMEASCONSIGNEE>No</ISDELIVERYSAMEASCONSIGNEE>" + "\n" +
"       <ISDISPATCHSAMEASCONSIGNOR>No</ISDISPATCHSAMEASCONSIGNOR>" + "\n" +
"       <ISDELETED>No</ISDELETED>" + "\n" +
"       <CHANGEVCHMODE>No</CHANGEVCHMODE>" + "\n" +
"       <ALTERID> 43</ALTERID>" + "\n" +
"       <MASTERID> 23</MASTERID>" + "\n" +
"       <VOUCHERKEY>184679298760712</VOUCHERKEY>" + "\n" +
"       <EXCLUDEDTAXATIONS.LIST>      </EXCLUDEDTAXATIONS.LIST>" + "\n" +
"       <OLDAUDITENTRIES.LIST>      </OLDAUDITENTRIES.LIST>" + "\n" +
"       <ACCOUNTAUDITENTRIES.LIST>      </ACCOUNTAUDITENTRIES.LIST>" + "\n" +
"       <AUDITENTRIES.LIST>      </AUDITENTRIES.LIST>" + "\n" +
"       <DUTYHEADDETAILS.LIST>      </DUTYHEADDETAILS.LIST>" + "\n" +
"       <SUPPLEMENTARYDUTYHEADDETAILS.LIST>      </SUPPLEMENTARYDUTYHEADDETAILS.LIST>" + "\n" +
"       <INVOICEDELNOTES.LIST>      </INVOICEDELNOTES.LIST>" + "\n" +
"       <INVOICEORDERLIST.LIST>      </INVOICEORDERLIST.LIST>" + "\n" +
"       <INVOICEINDENTLIST.LIST>      </INVOICEINDENTLIST.LIST>" + "\n" +
"       <ATTENDANCEENTRIES.LIST>      </ATTENDANCEENTRIES.LIST>" + "\n" +
"       <ORIGINVOICEDETAILS.LIST>      </ORIGINVOICEDETAILS.LIST>" + "\n" +
"       <INVOICEEXPORTLIST.LIST>      </INVOICEEXPORTLIST.LIST>";

        string voucherFooter = 
"      <PAYROLLMODEOFPAYMENT.LIST>      </PAYROLLMODEOFPAYMENT.LIST>" + "\n" +
"      <ATTDRECORDS.LIST>      </ATTDRECORDS.LIST>" + "\n" +
"      <TEMPGSTRATEDETAILS.LIST>      </TEMPGSTRATEDETAILS.LIST>" + "\n" +
"     </VOUCHER>" + "\n" +
"    </TALLYMESSAGE>" + "\n" +
"   </REQUESTDATA>" + "\n" +
"  </IMPORTDATA>" + "\n" +
" </BODY>" + "\n" +
"</ENVELOPE>";

        String ledgerheader = 
"      <ALLLEDGERENTRIES.LIST>" + "\n" +
"        <OLDAUDITENTRYIDS.LIST TYPE= \"Number\">" + "\n" +
"          <OLDAUDITENTRYIDS>-1</OLDAUDITENTRYIDS>" + "\n" +
"        </OLDAUDITENTRYIDS.LIST>" + "\n" +
"        <LEDGERNAME>#ledgername#</LEDGERNAME>" + "\n" +
"        <GSTCLASS/>" + "\n" +
"        <ISDEEMEDPOSITIVE>Yes</ISDEEMEDPOSITIVE>" + "\n" +
"        <LEDGERFROMITEM>No</LEDGERFROMITEM>" + "\n" +
"        <REMOVEZEROENTRIES>No</REMOVEZEROENTRIES>" + "\n" +
"        <ISPARTYLEDGER>No</ISPARTYLEDGER>" + "\n" +
"        <ISLASTDEEMEDPOSITIVE>Yes</ISLASTDEEMEDPOSITIVE>" + "\n" +
"        <AMOUNT>#amount#</AMOUNT>" + "\n" +
"        <SERVICETAXDETAILS.LIST>       </SERVICETAXDETAILS.LIST>" + "\n" +
"        <BANKALLOCATIONS.LIST>       </BANKALLOCATIONS.LIST>";

        String ledgerfooter = 
"        <INTERESTCOLLECTION.LIST>       </INTERESTCOLLECTION.LIST>" + "\n" +
"        <OLDAUDITENTRIES.LIST>       </OLDAUDITENTRIES.LIST>" + "\n" +
"        <ACCOUNTAUDITENTRIES.LIST>       </ACCOUNTAUDITENTRIES.LIST>" + "\n" +
"        <AUDITENTRIES.LIST>       </AUDITENTRIES.LIST>" + "\n" +
"        <INPUTCRALLOCS.LIST>       </INPUTCRALLOCS.LIST>" + "\n" +
"        <DUTYHEADDETAILS.LIST>       </DUTYHEADDETAILS.LIST>" + "\n" +
"        <EXCISEDUTYHEADDETAILS.LIST>       </EXCISEDUTYHEADDETAILS.LIST>" + "\n" +
"        <RATEDETAILS.LIST>       </RATEDETAILS.LIST>" + "\n" +
"        <SUMMARYALLOCS.LIST>       </SUMMARYALLOCS.LIST>" + "\n" +
"        <STPYMTDETAILS.LIST>       </STPYMTDETAILS.LIST>" + "\n" +
"        <EXCISEPAYMENTALLOCATIONS.LIST>       </EXCISEPAYMENTALLOCATIONS.LIST>" + "\n" +
"        <TAXBILLALLOCATIONS.LIST>       </TAXBILLALLOCATIONS.LIST>" + "\n" +
"        <TAXOBJECTALLOCATIONS.LIST>       </TAXOBJECTALLOCATIONS.LIST>" + "\n" +
"        <TDSEXPENSEALLOCATIONS.LIST>       </TDSEXPENSEALLOCATIONS.LIST>" + "\n" +
"        <VATSTATUTORYDETAILS.LIST>       </VATSTATUTORYDETAILS.LIST>" + "\n" +
"        <COSTTRACKALLOCATIONS.LIST>       </COSTTRACKALLOCATIONS.LIST>" + "\n" +
"        <REFVOUCHERDETAILS.LIST>       </REFVOUCHERDETAILS.LIST>" + "\n" +
"        <INVOICEWISEDETAILS.LIST>       </INVOICEWISEDETAILS.LIST>" + "\n" +
"        <VATITCDETAILS.LIST>       </VATITCDETAILS.LIST>" + "\n" +
"        <ADVANCETAXDETAILS.LIST>       </ADVANCETAXDETAILS.LIST>" + "\n" +
"      </ALLLEDGERENTRIES.LIST>";

        String billallocation = 
"      <BILLALLOCATIONS.LIST>" + "\n" +
"        <NAME>#orderid#</NAME>" + "\n" +
"        <BILLTYPE>New Ref</BILLTYPE>" + "\n" +
"        <TDSDEDUCTEEISSPECIALRATE>No</TDSDEDUCTEEISSPECIALRATE>" + "\n" +
"        <AMOUNT>#amount#</AMOUNT>" + "\n" +
"        <INTERESTCOLLECTION.LIST>        </INTERESTCOLLECTION.LIST>" + "\n" +
"        <STBILLCATEGORIES.LIST>        </STBILLCATEGORIES.LIST>" + "\n" +
"      </BILLALLOCATIONS.LIST>";

        public string TallySettlementXML(List<SaleReport> lstOrdertext2, string settlement_reference, string settlement_deposit_date, string CompanyName, int sellers_id, string MarketPlace)
        {
            string Guid1 = "";
            

            Dictionary<String, List<SaleReport>> xmlDict = new Dictionary<String, List<SaleReport>>();
            Dictionary<String, double> xmlDictTotal = new Dictionary<String, double>();
            String Narration = settlement_reference.Replace("-", "");
            string Date = settlement_deposit_date;
            foreach (var item in lstOrdertext2)
            {
                String ledgername = item.ExpenseName;                      
                double amt1 = 0;
                double amt = 0;
                List<SaleReport> li = null;
                if (xmlDict.ContainsKey(ledgername))
                {
                    li = xmlDict[ledgername];
                    if (li == null)
                        li = new List<SaleReport>();

                    amt1 = xmlDictTotal[ledgername];
                }
                else
                {
                    li = new List<SaleReport>();

                    xmlDict.Add(ledgername, li);

                    xmlDictTotal.Add(ledgername, amt);
                }

                if (item.SumOrder != 0)
                {
                    amt = item.SumOrder;
                    li.Add(item);
                }
                else if (item.refund_SumOrder != 0)
                {
                    
                    item.SumOrder = item.refund_SumOrder * -1;
                    amt = item.SumOrder;
                    li.Add(item);
                }
                xmlDictTotal[ledgername] = amt + amt1;
            }//end for

            Guid g;
            // Create and display the value of two GUIDs.
            g = Guid.NewGuid();
            var get_guid_no = dba.tbl_seller_accounting_pkg_details.Where(a => a.seller_id == sellers_id).FirstOrDefault();
            if (get_guid_no != null)
            {
                DateTime dt = DateTime.Now;
                string value = dt.ToString("ddMMyyyy/HHMMss");
                Guid1 = get_guid_no.guid + value;
            }
            string xmloutput = "";
            string marketplacevoucher = MarketPlace +"/ Settlement / 03";
            string marketplacepartname = MarketPlace +"- Receipt - ET";
            string myvoucherHeader = voucherHeader;
            myvoucherHeader = myvoucherHeader.Replace("#uniqueguid#", g.ToString());
            myvoucherHeader = myvoucherHeader.Replace("#SettlementDate#", Narration);
            myvoucherHeader = myvoucherHeader.Replace("#NarrartionNo#", Date);
            myvoucherHeader = myvoucherHeader.Replace("#companyname#", CompanyName);

            myvoucherHeader = myvoucherHeader.Replace("#marketplacevoucher#", marketplacevoucher);
            myvoucherHeader = myvoucherHeader.Replace("#marketplacepartyledgername#", marketplacepartname);

           
            xmloutput += myvoucherHeader;

            foreach (KeyValuePair<string, List<SaleReport>> pair in xmlDict)
            {
                string ledgername = pair.Key;
                List<SaleReport> li = pair.Value;
                double ledgeramt = xmlDictTotal[ledgername];
                string myledgerheader = ledgerheader;

                myledgerheader = myledgerheader.Replace("#ledgername#", ledgername);
                myledgerheader = myledgerheader.Replace("#amount#", Convert.ToString(ledgeramt));
                xmloutput += myledgerheader;

                if (li.Count() > 1)
                {
                    foreach (var item in li)
                    {
                        string entry = billallocation;
                        entry = entry.Replace("#orderid#", item.OrderID);
                        entry = entry.Replace("#amount#", Convert.ToString(item.SumOrder));
                        xmloutput += entry;

                    }//end for
                }
                else
                {
                    xmloutput += "<BILLALLOCATIONS.LIST>       </BILLALLOCATIONS.LIST>";
                }

                xmloutput += ledgerfooter;

            }//end for
            xmloutput += voucherFooter;

            return (xmloutput);
        }
    }
}