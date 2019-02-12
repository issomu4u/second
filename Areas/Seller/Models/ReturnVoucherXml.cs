using SellerVendor.Areas.Seller.Models.DBContext;
using SellerVendor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SellerVendor.Areas.Seller.Models
{
    public class ReturnVoucherXml
    {
        public SellerContext dba = new SellerContext();
        public SellerAdminContext db = new SellerAdminContext();


        string salesheader =
"<ENVELOPE>\n" +
"  <HEADER>\n" +
"     <TALLYREQUEST>Import Data</TALLYREQUEST>\n" +
"  </HEADER>\n" +
"   <BODY>\n" +
"   <IMPORTDATA>\n" +
"     <REQUESTDESC>\n" +
"       <REPORTNAME>Vouchers</REPORTNAME>\n" +
"        <STATICVARIABLES>\n" +
"          <SVCURRENTCOMPANY>#companyname# (F.Y. 2016-17)</SVCURRENTCOMPANY>\n" +
"        </STATICVARIABLES>\n" +
"     </REQUESTDESC>\n" +
"    <REQUESTDATA>\n";

        string messagedetails =
"       <TALLYMESSAGE xmlns:UDF=\"TallyUDF\">" + "\n" +
"          <VOUCHER REMOTEID=\"e44e5438-bbf7-4b26-94b2-672e136f7320-00015bc1\" VCHKEY=\"e44e5438-bbf7-4b26-94b2-672e136f7320-0000a83b:00000500\" VCHTYPE=\"Raintree Sales(S)\" ACTION=\"Create\" OBJVIEW=\"Invoice Voucher View\">" + "\n" +
"            <BASICBUYERADDRESS.LIST TYPE=\"String\">" + "\n" +
"              <BASICBUYERADDRESS>#buyeraddress1#</BASICBUYERADDRESS>" + "\n" +
"              <BASICBUYERADDRESS>#buyeraddress2#</BASICBUYERADDRESS>" + "\n" +
"              <BASICBUYERADDRESS>#buyeraddress3#</BASICBUYERADDRESS>" + "\n" +
"              <BASICBUYERADDRESS>#buyeraddress4#</BASICBUYERADDRESS>" + "\n" +
"              <BASICBUYERADDRESS>#buyeraddress5#</BASICBUYERADDRESS>" + "\n" +
"            </BASICBUYERADDRESS.LIST>" + "\n" +
"            <OLDAUDITENTRYIDS.LIST TYPE=\"Number\">" + "\n" +
"              <OLDAUDITENTRYIDS>-1</OLDAUDITENTRYIDS>" + "\n" +
"            </OLDAUDITENTRYIDS.LIST>" + "\n" +
"            <DATE>#purchasedate#</DATE>" + "\n" +
"            <RETURNINVOICEDATE>#purchasedate#</RETURNINVOICEDATE>" + "\n" +
"            <REFERENCEDATE>#purchasedate#</REFERENCEDATE>" + "\n" +
"            <GUID>e44e5438-bbf7-4b26-94b2-672e136f7320-00015bc1</GUID>" + "\n" +
"            <STATENAME>#statename#</STATENAME>" + "\n" +
"            <VATDEALERTYPE>Unregistered</VATDEALERTYPE>" + "\n" +
"            <NARRATION>29-Nov-2017</NARRATION>" + "\n" +
"            <PARTYNAME>Sale-Customer-Amazon</PARTYNAME>" + "\n" +
"            <VOUCHERTYPENAME>Raintree Sale(S)</VOUCHERTYPENAME>" + "\n" +
"            <REFERENCE>#orderid#</REFERENCE>" + "\n" +
"            <VOUCHERNUMBER>#voucherno#</VOUCHERNUMBER>" + "\n" +
"            <PARTYLEDGERNAME>Sale-Customer-Amazon</PARTYLEDGERNAME>" + "\n" +
"            <CSTFORMISSUETYPE/>" + "\n" +
"           <CSTFORMRECVTYPE/>" + "\n" +
"            <FBTPAYMENTTYPE>Default</FBTPAYMENTTYPE>" + "\n" +
"           <PERSISTEDVIEW>Invoice Voucher View</PERSISTEDVIEW>" + "\n" +
"            <BASICSHIPPEDBY>ATS</BASICSHIPPEDBY>" + "\n" +
"           <BASICBUYERNAME>bhavesh patel</BASICBUYERNAME>" + "\n" +
"            <BASICSHIPDOCUMENTNO>511392752298</BASICSHIPDOCUMENTNO>" + "\n" +
"            <VCHGSTCLASS/>" + "\n" +
"            <ENTEREDBY>Vineet Gahlot</ENTEREDBY>" + "\n" +
"            <DIFFACTUALQTY>No</DIFFACTUALQTY>" + "\n" +
"            <ISMSTFROMSYNC>No</ISMSTFROMSYNC>" + "\n" +
"            <ASORIGINAL>No</ASORIGINAL>" + "\n" +
"            <AUDITED>No</AUDITED>" + "\n" +
"            <FORJOBCOSTING>No</FORJOBCOSTING>" + "\n" +
"            <ISOPTIONAL>No</ISOPTIONAL>" + "\n" +
"            <EFFECTIVEDATE>#purchasedate#</EFFECTIVEDATE>" + "\n" +
"            <USEFOREXCISE>No</USEFOREXCISE>" + "\n" +
"            <ISFORJOBWORKIN>No</ISFORJOBWORKIN>" + "\n" +
"            <ALLOWCONSUMPTION>No</ALLOWCONSUMPTION>" + "\n" +
"            <USEFORINTEREST>No</USEFORINTEREST>" + "\n" +
"            <USEFORGAINLOSS>No</USEFORGAINLOSS>" + "\n" +
"            <USEFORGODOWNTRANSFER>No</USEFORGODOWNTRANSFER>" + "\n" +
"            <USEFORCOMPOUND>No</USEFORCOMPOUND>" + "\n" +
"            <USEFORSERVICETAX>No</USEFORSERVICETAX>" + "\n" +
"            <ISEXCISEVOUCHER>No</ISEXCISEVOUCHER>\n" +
"            <EXCISETAXOVERRIDE>No</EXCISETAXOVERRIDE>" + "\n" +
"            <USEFORTAXUNITTRANSFER>No</USEFORTAXUNITTRANSFER>" + "\n" +
"            <EXCISEOPENING>No</EXCISEOPENING>" + "\n" +
"            <USEFORFINALPRODUCTION>No</USEFORFINALPRODUCTION>" + "\n" +
"            <ISTDSOVERRIDDEN>No</ISTDSOVERRIDDEN>" + "\n" +
"            <ISTCSOVERRIDDEN>No</ISTCSOVERRIDDEN>" + "\n" +
"            <ISTDSTCSCASHVCH>No</ISTDSTCSCASHVCH>" + "\n" +
"            <INCLUDEADVPYMTVCH>No</INCLUDEADVPYMTVCH>" + "\n" +
"            <ISSUBWORKSCONTRACT>No</ISSUBWORKSCONTRACT>" + "\n" +
"            <ISVATOVERRIDDEN>No</ISVATOVERRIDDEN>" + "\n" +
"            <IGNOREORIGVCHDATE>No</IGNOREORIGVCHDATE>" + "\n" +
"            <ISSERVICETAXOVERRIDDEN>No</ISSERVICETAXOVERRIDDEN>" + "\n" +
"            <ISISDVOUCHER>No</ISISDVOUCHER>" + "\n" +
"            <ISEXCISEOVERRIDDEN>No</ISEXCISEOVERRIDDEN>" + "\n" +
"            <ISEXCISESUPPLYVCH>No</ISEXCISESUPPLYVCH>" + "\n" +
"            <ISGSTOVERRIDDEN>No</ISGSTOVERRIDDEN>" + "\n" +
"            <GSTNOTEXPORTED>No</GSTNOTEXPORTED>" + "\n" +
"            <ISVATPRINCIPALACCOUNT>No</ISVATPRINCIPALACCOUNT>" + "\n" +
"            <ISBOENOTAPPLICABLE>No</ISBOENOTAPPLICABLE>" + "\n" +
"            <ISSHIPPINGWITHINSTATE>No</ISSHIPPINGWITHINSTATE>" + "\n" +
"            <ISCANCELLED>No</ISCANCELLED>" + "\n" +
"            <HASCASHFLOW>No</HASCASHFLOW>" + "\n" +
"            <ISPOSTDATED>No</ISPOSTDATED>" + "\n" +
"            <USETRACKINGNUMBER>No</USETRACKINGNUMBER>" + "\n" +
"            <ISINVOICE>Yes</ISINVOICE>" + "\n" +
"            <MFGJOURNAL>No</MFGJOURNAL>" + "\n" +
"            <HASDISCOUNTS>No</HASDISCOUNTS>" + "\n" +
"            <ASPAYSLIP>No</ASPAYSLIP>" + "\n" +
"            <ISCOSTCENTRE>No</ISCOSTCENTRE>" + "\n" +
"            <ISSTXNONREALIZEDVCH>No</ISSTXNONREALIZEDVCH>" + "\n" +
"            <ISEXCISEMANUFACTURERON>No</ISEXCISEMANUFACTURERON>" + "\n" +
"            <ISBLANKCHEQUE>No</ISBLANKCHEQUE>" + "\n" +
"            <ISVOID>No</ISVOID>" + "\n" +
"            <ISONHOLD>No</ISONHOLD>" + "\n" +
"            <ORDERLINESTATUS>No</ORDERLINESTATUS>" + "\n" +
"            <VATISAGNSTCANCSALES>No</VATISAGNSTCANCSALES>" + "\n" +
"            <VATISPURCEXEMPTED>No</VATISPURCEXEMPTED>" + "\n" +
"            <ISVATRESTAXINVOICE>No</ISVATRESTAXINVOICE>" + "\n" +
"            <VATISASSESABLECALCVCH>No</VATISASSESABLECALCVCH>" + "\n" +
"            <ISVATDUTYPAID>Yes</ISVATDUTYPAID>" + "\n" +
"            <ISDELIVERYSAMEASCONSIGNEE>No</ISDELIVERYSAMEASCONSIGNEE>" + "\n" +
"            <ISDISPATCHSAMEASCONSIGNOR>No</ISDISPATCHSAMEASCONSIGNOR>" + "\n" +
"            <ISDELETED>No</ISDELETED>" + "\n" +
"            <CHANGEVCHMODE>No</CHANGEVCHMODE>" + "\n" +
"            <ALTERID> 127409</ALTERID>" + "\n" +
"            <MASTERID> 89025</MASTERID>" + "\n" +
"            <VOUCHERKEY>184971356538112</VOUCHERKEY>" + "\n" +
"            <EXCLUDEDTAXATIONS.LIST>      </EXCLUDEDTAXATIONS.LIST>" + "\n" +
"            <OLDAUDITENTRIES.LIST>      </OLDAUDITENTRIES.LIST>" + "\n" +
"            <ACCOUNTAUDITENTRIES.LIST>      </ACCOUNTAUDITENTRIES.LIST>" + "\n" +
"            <AUDITENTRIES.LIST>      </AUDITENTRIES.LIST>" + "\n" +
"            <DUTYHEADDETAILS.LIST>      </DUTYHEADDETAILS.LIST>" + "\n" +
"            <SUPPLEMENTARYDUTYHEADDETAILS.LIST>      </SUPPLEMENTARYDUTYHEADDETAILS.LIST>" + "\n" +
"            <INVOICEDELNOTES.LIST>" + "\n" +
"              <BASICSHIPDELIVERYNOTE>511392752298</BASICSHIPDELIVERYNOTE>" + "\n" +
"            </INVOICEDELNOTES.LIST>" + "\n" +
"            <INVOICEORDERLIST.LIST>" + "\n" +
"              <BASICORDERDATE>#purchasedate#</BASICORDERDATE>" + "\n" +
"              <BASICPURCHASEORDERNO>#orderid#</BASICPURCHASEORDERNO>" + "\n" +
"            </INVOICEORDERLIST.LIST>" + "\n" +
"            <INVOICEINDENTLIST.LIST>      </INVOICEINDENTLIST.LIST>" + "\n" +
"            <ATTENDANCEENTRIES.LIST>      </ATTENDANCEENTRIES.LIST>" + "\n" +
"            <ORIGINVOICEDETAILS.LIST>      </ORIGINVOICEDETAILS.LIST>" + "\n" +
"            <INVOICEEXPORTLIST.LIST>      </INVOICEEXPORTLIST.LIST>" + "\n" +
"            <LEDGERENTRIES.LIST>" + "\n" +
"              <OLDAUDITENTRYIDS.LIST TYPE=\"Number\">" + "\n" +
"                <OLDAUDITENTRYIDS>-1</OLDAUDITENTRYIDS>" + "\n" +
"              </OLDAUDITENTRYIDS.LIST>" + "\n" +
"              <LEDGERNAME>Sale-Customer-Amazon</LEDGERNAME>" + "\n" +
"              <GSTCLASS/>" + "\n" +
"              <ISDEEMEDPOSITIVE>Yes</ISDEEMEDPOSITIVE>" + "\n" +
"              <LEDGERFROMITEM>No</LEDGERFROMITEM>" + "\n" +
"              <REMOVEZEROENTRIES>No</REMOVEZEROENTRIES>" + "\n" +
"              <ISPARTYLEDGER>Yes</ISPARTYLEDGER>" + "\n" +
"              <ISLASTDEEMEDPOSITIVE>Yes</ISLASTDEEMEDPOSITIVE>" + "\n" +
"              <AMOUNT>#bill_amount#</AMOUNT>" + "\n" +
"              <SERVICETAXDETAILS.LIST>       </SERVICETAXDETAILS.LIST>" + "\n" +
"              <BANKALLOCATIONS.LIST>       </BANKALLOCATIONS.LIST>" + "\n" +
"              <BILLALLOCATIONS.LIST>" + "\n" +
"                <NAME>#orderid#</NAME>" + "\n" +
"                <BILLTYPE>New Ref</BILLTYPE>" + "\n" +
"                <TDSDEDUCTEEISSPECIALRATE>No</TDSDEDUCTEEISSPECIALRATE>" + "\n" +
"                <AMOUNT>#bill_amount#</AMOUNT>" + "\n" +
"                <INTERESTCOLLECTION.LIST>        </INTERESTCOLLECTION.LIST>" + "\n" +
"                <STBILLCATEGORIES.LIST>        </STBILLCATEGORIES.LIST>" + "\n" +
"              </BILLALLOCATIONS.LIST>" + "\n" +
"              <INTERESTCOLLECTION.LIST>       </INTERESTCOLLECTION.LIST>" + "\n" +
"              <OLDAUDITENTRIES.LIST>       </OLDAUDITENTRIES.LIST>" + "\n" +
"              <ACCOUNTAUDITENTRIES.LIST>       </ACCOUNTAUDITENTRIES.LIST>" + "\n" +
"              <AUDITENTRIES.LIST>       </AUDITENTRIES.LIST>" + "\n" +
"              <INPUTCRALLOCS.LIST>       </INPUTCRALLOCS.LIST>" + "\n" +
"              <DUTYHEADDETAILS.LIST>       </DUTYHEADDETAILS.LIST>" + "\n" +
"              <EXCISEDUTYHEADDETAILS.LIST>       </EXCISEDUTYHEADDETAILS.LIST>" + "\n" +
"              <RATEDETAILS.LIST>       </RATEDETAILS.LIST>" + "\n" +
"              <SUMMARYALLOCS.LIST>       </SUMMARYALLOCS.LIST>" + "\n" +
"              <STPYMTDETAILS.LIST>       </STPYMTDETAILS.LIST>" + "\n" +
"              <EXCISEPAYMENTALLOCATIONS.LIST>       </EXCISEPAYMENTALLOCATIONS.LIST>" + "\n" +
"              <TAXBILLALLOCATIONS.LIST>       </TAXBILLALLOCATIONS.LIST>" + "\n" +
"              <TAXOBJECTALLOCATIONS.LIST>       </TAXOBJECTALLOCATIONS.LIST>" + "\n" +
"              <TDSEXPENSEALLOCATIONS.LIST>       </TDSEXPENSEALLOCATIONS.LIST>" + "\n" +
"              <VATSTATUTORYDETAILS.LIST>       </VATSTATUTORYDETAILS.LIST>" + "\n" +
"              <COSTTRACKALLOCATIONS.LIST>       </COSTTRACKALLOCATIONS.LIST>" + "\n" +
"              <REFVOUCHERDETAILS.LIST>       </REFVOUCHERDETAILS.LIST>" + "\n" +
"              <INVOICEWISEDETAILS.LIST>       </INVOICEWISEDETAILS.LIST>" + "\n" +
"              <VATITCDETAILS.LIST>       </VATITCDETAILS.LIST>" + "\n" +
"              <ADVANCETAXDETAILS.LIST>       </ADVANCETAXDETAILS.LIST>" + "\n" +
"            </LEDGERENTRIES.LIST>";

        string messagetax =
"            <LEDGERENTRIES.LIST>" + "\n" +
"               <OLDAUDITENTRYIDS.LIST TYPE=\"Number\">" + "\n" +
"                  <OLDAUDITENTRYIDS>-1</OLDAUDITENTRYIDS>" + "\n" +
"               </OLDAUDITENTRYIDS.LIST>" + "\n" +
"               <LEDGERNAME>#taxname#</LEDGERNAME>" + "\n" +
"               <GSTCLASS/>" + "\n" +
"               <ISDEEMEDPOSITIVE>No</ISDEEMEDPOSITIVE>" + "\n" +
"               <LEDGERFROMITEM>No</LEDGERFROMITEM>" + "\n" +
"               <REMOVEZEROENTRIES>No</REMOVEZEROENTRIES>" + "\n" +
"               <ISPARTYLEDGER>No</ISPARTYLEDGER>" + "\n" +
"               <ISLASTDEEMEDPOSITIVE>No</ISLASTDEEMEDPOSITIVE>" + "\n" +
"               <AMOUNT>#taxamount#</AMOUNT>" + "\n" +
"               <SERVICETAXDETAILS.LIST>       </SERVICETAXDETAILS.LIST>" + "\n" +
"               <BANKALLOCATIONS.LIST>       </BANKALLOCATIONS.LIST>" + "\n" +
"               <BILLALLOCATIONS.LIST>       </BILLALLOCATIONS.LIST>" + "\n" +
"               <INTERESTCOLLECTION.LIST>       </INTERESTCOLLECTION.LIST>" + "\n" +
"               <OLDAUDITENTRIES.LIST>       </OLDAUDITENTRIES.LIST>" + "\n" +
"               <ACCOUNTAUDITENTRIES.LIST>       </ACCOUNTAUDITENTRIES.LIST>" + "\n" +
"               <AUDITENTRIES.LIST>       </AUDITENTRIES.LIST>" + "\n" +
"               <INPUTCRALLOCS.LIST>       </INPUTCRALLOCS.LIST>" + "\n" +
"               <DUTYHEADDETAILS.LIST>       </DUTYHEADDETAILS.LIST>" + "\n" +
"               <EXCISEDUTYHEADDETAILS.LIST>       </EXCISEDUTYHEADDETAILS.LIST>" + "\n" +
"               <RATEDETAILS.LIST>       </RATEDETAILS.LIST>" + "\n" +
"               <SUMMARYALLOCS.LIST>       </SUMMARYALLOCS.LIST>" + "\n" +
"               <STPYMTDETAILS.LIST>       </STPYMTDETAILS.LIST>" + "\n" +
"               <EXCISEPAYMENTALLOCATIONS.LIST>       </EXCISEPAYMENTALLOCATIONS.LIST>" + "\n" +
"               <TAXBILLALLOCATIONS.LIST>       </TAXBILLALLOCATIONS.LIST>" + "\n" +
"               <TAXOBJECTALLOCATIONS.LIST>       </TAXOBJECTALLOCATIONS.LIST>" + "\n" +
"               <TDSEXPENSEALLOCATIONS.LIST>       </TDSEXPENSEALLOCATIONS.LIST>" + "\n" +
"               <VATSTATUTORYDETAILS.LIST>       </VATSTATUTORYDETAILS.LIST>" + "\n" +
"               <COSTTRACKALLOCATIONS.LIST>       </COSTTRACKALLOCATIONS.LIST>" + "\n" +
"               <REFVOUCHERDETAILS.LIST>       </REFVOUCHERDETAILS.LIST>" + "\n" +
"               <INVOICEWISEDETAILS.LIST>       </INVOICEWISEDETAILS.LIST>" + "\n" +
"               <VATITCDETAILS.LIST>       </VATITCDETAILS.LIST>" + "\n" +
"               <ADVANCETAXDETAILS.LIST>       </ADVANCETAXDETAILS.LIST>" + "\n" +
"            </LEDGERENTRIES.LIST>\n";


        string messageinventory =
"           <ALLINVENTORYENTRIES.LIST>" + "\n" +
"             <BASICUSERDESCRIPTION.LIST TYPE=\"String\">" + "\n" +
"               <BASICUSERDESCRIPTION>Main Location</BASICUSERDESCRIPTION>" + "\n" +
"             </BASICUSERDESCRIPTION.LIST>" + "\n" +
"             <STOCKITEMNAME>#productname#</STOCKITEMNAME>" + "\n" +
"             <ISDEEMEDPOSITIVE>No</ISDEEMEDPOSITIVE>" + "\n" +
"             <ISLASTDEEMEDPOSITIVE>No</ISLASTDEEMEDPOSITIVE>" + "\n" +
"             <ISAUTONEGATE>No</ISAUTONEGATE>" + "\n" +
"             <ISCUSTOMSCLEARANCE>No</ISCUSTOMSCLEARANCE>" + "\n" +
"             <ISTRACKCOMPONENT>No</ISTRACKCOMPONENT>" + "\n" +
"             <ISTRACKPRODUCTION>No</ISTRACKPRODUCTION>" + "\n" +
"             <ISPRIMARYITEM>No</ISPRIMARYITEM>" + "\n" +
"             <ISSCRAP>No</ISSCRAP>" + "\n" +
"             <RATE>#productprice#/Nos.</RATE>" + "\n" +
"             <AMOUNT>#productprice#</AMOUNT>" + "\n" +
"             <ACTUALQTY> 1.000 Nos.</ACTUALQTY>" + "\n" +
"             <BILLEDQTY> 1.000 Nos.</BILLEDQTY>" + "\n" +
"             <BATCHALLOCATIONS.LIST>" + "\n" +
"                <GODOWNNAME>Main Location</GODOWNNAME>" + "\n" +
"                <BATCHNAME>Primary Batch</BATCHNAME>" + "\n" +
"                <INDENTNO/>" + "\n" +
"                <ORDERNO/>" + "\n" +
"                <TRACKINGNUMBER/>" + "\n" +
"                <DYNAMICCSTISCLEARED>No</DYNAMICCSTISCLEARED>" + "\n" +
"                <AMOUNT>#productprice#</AMOUNT>" + "\n" +
"                <ACTUALQTY> 1.000 Nos.</ACTUALQTY>" + "\n" +
"                <BILLEDQTY> 1.000 Nos.</BILLEDQTY>" + "\n" +
"                <ADDITIONALDETAILS.LIST>        </ADDITIONALDETAILS.LIST>" + "\n" +
"                <VOUCHERCOMPONENTLIST.LIST>        </VOUCHERCOMPONENTLIST.LIST>" + "\n" +
"            </BATCHALLOCATIONS.LIST>" + "\n" +
"            <ACCOUNTINGALLOCATIONS.LIST>" + "\n" +
"              <OLDAUDITENTRYIDS.LIST TYPE=\"Number\">" + "\n" +
"                <OLDAUDITENTRYIDS>-1</OLDAUDITENTRYIDS>" + "\n" +
"             </OLDAUDITENTRYIDS.LIST>" + "\n" +
"               <LEDGERNAME>#LedgerSaleTax#</LEDGERNAME>" + "\n" +
"               <GSTCLASS/>" + "\n" +
"               <ISDEEMEDPOSITIVE>No</ISDEEMEDPOSITIVE>" + "\n" +
"               <LEDGERFROMITEM>No</LEDGERFROMITEM>" + "\n" +
"               <REMOVEZEROENTRIES>No</REMOVEZEROENTRIES>" + "\n" +
"               <ISPARTYLEDGER>No</ISPARTYLEDGER>" + "\n" +
"               <ISLASTDEEMEDPOSITIVE>No</ISLASTDEEMEDPOSITIVE>" + "\n" +
"               <AMOUNT>#productprice#</AMOUNT>" + "\n" +
"               <SERVICETAXDETAILS.LIST>        </SERVICETAXDETAILS.LIST>" + "\n" +
"               <BANKALLOCATIONS.LIST>        </BANKALLOCATIONS.LIST>" + "\n" +
"               <BILLALLOCATIONS.LIST>        </BILLALLOCATIONS.LIST>" + "\n" +
"               <INTERESTCOLLECTION.LIST>        </INTERESTCOLLECTION.LIST>" + "\n" +
"               <OLDAUDITENTRIES.LIST>        </OLDAUDITENTRIES.LIST>" + "\n" +
"               <ACCOUNTAUDITENTRIES.LIST>        </ACCOUNTAUDITENTRIES.LIST>" + "\n" +
"                <AUDITENTRIES.LIST>        </AUDITENTRIES.LIST>" + "\n" +
"                <INPUTCRALLOCS.LIST>        </INPUTCRALLOCS.LIST>" + "\n" +
"                <DUTYHEADDETAILS.LIST>        </DUTYHEADDETAILS.LIST>" + "\n" +
"                <EXCISEDUTYHEADDETAILS.LIST>        </EXCISEDUTYHEADDETAILS.LIST>" + "\n" +
"                <RATEDETAILS.LIST>        </RATEDETAILS.LIST>" + "\n" +
"                <SUMMARYALLOCS.LIST>        </SUMMARYALLOCS.LIST>" + "\n" +
"                <STPYMTDETAILS.LIST>        </STPYMTDETAILS.LIST>" + "\n" +
"                <EXCISEPAYMENTALLOCATIONS.LIST>        </EXCISEPAYMENTALLOCATIONS.LIST>" + "\n" +
"                <TAXBILLALLOCATIONS.LIST>        </TAXBILLALLOCATIONS.LIST>" + "\n" +
"                <TAXOBJECTALLOCATIONS.LIST>        </TAXOBJECTALLOCATIONS.LIST>" + "\n" +
"                <TDSEXPENSEALLOCATIONS.LIST>        </TDSEXPENSEALLOCATIONS.LIST>" + "\n" +
"                <VATSTATUTORYDETAILS.LIST>        </VATSTATUTORYDETAILS.LIST>" + "\n" +
"                <COSTTRACKALLOCATIONS.LIST>        </COSTTRACKALLOCATIONS.LIST>" + "\n" +
"                <REFVOUCHERDETAILS.LIST>        </REFVOUCHERDETAILS.LIST>" + "\n" +
"                <INVOICEWISEDETAILS.LIST>        </INVOICEWISEDETAILS.LIST>" + "\n" +
"                <VATITCDETAILS.LIST>        </VATITCDETAILS.LIST>" + "\n" +
"                <ADVANCETAXDETAILS.LIST>        </ADVANCETAXDETAILS.LIST>" + "\n" +
"              </ACCOUNTINGALLOCATIONS.LIST>" + "\n" +
"            <DUTYHEADDETAILS.LIST>       </DUTYHEADDETAILS.LIST>" + "\n" +
"            <SUPPLEMENTARYDUTYHEADDETAILS.LIST>       </SUPPLEMENTARYDUTYHEADDETAILS.LIST>" + "\n" +
"            <TAXOBJECTALLOCATIONS.LIST>       </TAXOBJECTALLOCATIONS.LIST>" + "\n" +
"            <REFVOUCHERDETAILS.LIST>       </REFVOUCHERDETAILS.LIST>" + "\n" +
"            <EXCISEALLOCATIONS.LIST>       </EXCISEALLOCATIONS.LIST>" + "\n" +
"            <EXPENSEALLOCATIONS.LIST>       </EXPENSEALLOCATIONS.LIST>" + "\n" +
"          </ALLINVENTORYENTRIES.LIST>\n";

        string messagefooter =
"            <PAYROLLMODEOFPAYMENT.LIST>      </PAYROLLMODEOFPAYMENT.LIST>" + "\n" +
"            <ATTDRECORDS.LIST>      </ATTDRECORDS.LIST>" + "\n" +
"            <TEMPGSTRATEDETAILS.LIST>      </TEMPGSTRATEDETAILS.LIST>" + "\n" +
"          </VOUCHER>" + "\n" +
"        </TALLYMESSAGE>\n";


        string messagecompany =
"       <TALLYMESSAGE xmlns:UDF=\"TallyUDF\">" + "\n" +
"          <COMPANY>" + "\n" +
"            <REMOTECMPINFO.LIST MERGE=\"Yes\">" + "\n" +
"              <NAME>e44e5438-bbf7-4b26-94b2-672e136f7320</NAME>" + "\n" +
"             <REMOTECMPNAME>Mysource-HR (F.Y. 2016-17)</REMOTECMPNAME>" + "\n" +
"             <REMOTECMPSTATE>Haryana</REMOTECMPSTATE>" + "\n" +
"           </REMOTECMPINFO.LIST>" + "\n" +
"           <REMOTECMPINFO.LIST MERGE=\"Yes\">" + "\n" +
"             <NAME>94cd897a-2b55-4e78-a9f3-a5c1cd6b553b</NAME>" + "\n" +
"             <REMOTECMPNAME>MSI  HARYANA (2015-2016)</REMOTECMPNAME>" + "\n" +
"             <REMOTECMPSTATE>Haryana</REMOTECMPSTATE>" + "\n" +
"           </REMOTECMPINFO.LIST>" + "\n" +
"           <REMOTECMPINFO.LIST MERGE=\"Yes\">" + "\n" +
"             <NAME>667b8d34-edaf-4feb-8411-99466ae97cfd</NAME>" + "\n" +
"             <REMOTECMPNAME>MYSOURCE INNOVENTURES PVT LTD(Gurgaon)</REMOTECMPNAME>" + "\n" +
"             <REMOTECMPSTATE>Haryana</REMOTECMPSTATE>" + "\n" +
"           </REMOTECMPINFO.LIST>" + "\n" +
"           <REMOTECMPINFO.LIST MERGE=\"Yes\">" + "\n" +
"             <NAME>db2ac668-c997-44d9-a9a7-5901d78c0b5a</NAME>" + "\n" +
"            <REMOTECMPNAME>Mysource Ggn</REMOTECMPNAME>" + "\n" +
"             <REMOTECMPSTATE>Haryana</REMOTECMPSTATE>" + "\n" +
"           </REMOTECMPINFO.LIST>" + "\n" +
"           <REMOTECMPINFO.LIST MERGE=\"Yes\">" + "\n" +
"             <NAME>c2ff9cf4-62e1-4e6e-ad55-7c1972f9e6e0</NAME>" + "\n" +
"             <REMOTECMPNAME>MYSOURCE INNOVENTURES PVT LTD(Gurgaon)</REMOTECMPNAME>" + "\n" +
"             <REMOTECMPSTATE>Haryana</REMOTECMPSTATE>" + "\n" +
"          </REMOTECMPINFO.LIST>" + "\n" +
"        </COMPANY>" + "\n" +
"      </TALLYMESSAGE>\n";


        string salesfooter =
"     </REQUESTDATA>" + "\n" +
"   </IMPORTDATA>" + "\n" +
"  </BODY>" + "\n" +
"</ENVELOPE\n";

        public string ReturnVoucherXML(DateTime? txt_from, DateTime? txt_to, int? sellers_id, string CompanyName)
        {
               string xmloutput = "";
               try
               {
                   if (txt_from != null && txt_to != null)
                   {
                       var GetReturnHistoryDetail = (from ob_tblhistory in dba.tbl_order_history
                                                     select new
                                                     {
                                                         ob_tblhistory = ob_tblhistory
                                                     }).Where(a => a.ob_tblhistory.tbl_seller_id == sellers_id && a.ob_tblhistory.physically_type == 1).ToList();

                       if (GetReturnHistoryDetail != null)
                       {
                           if (txt_from != null && txt_to != null)
                           {
                               GetReturnHistoryDetail = GetReturnHistoryDetail.Where(a => Convert.ToDateTime(a.ob_tblhistory.ShipmentDate).Date >= txt_from && Convert.ToDateTime(a.ob_tblhistory.ShipmentDate).Date <= txt_to).ToList();
                           }
                       }
                       if (GetReturnHistoryDetail != null)
                       {
                           string myvoucherHeader = salesheader;
                           myvoucherHeader = myvoucherHeader.Replace("#companyname#", CompanyName);

                           xmloutput += myvoucherHeader;
                           foreach (var item in GetReturnHistoryDetail)
                           {
                               string SKU_No = "";
                               double promotiondiscount = 0;
                               double product_amount = 0;
                               string voucher_no = "", purchase_date = "", country = "", state = "", postalcode = "", city = "", city_state = "", country_pincode = "", phoneno="";
                               SKU_No = item.ob_tblhistory.SKU;
                               var getsale_order_detail = dba.tbl_sales_order_details.Where(a => a.amazon_order_id == item.ob_tblhistory.OrderID && a.sku_no == SKU_No && a.tbl_seller_id == sellers_id).FirstOrDefault();
                               if(getsale_order_detail != null)
                               {
                                   promotiondiscount = promotiondiscount + getsale_order_detail.promotion_amount;
                                   voucher_no = getsale_order_detail.tax_invoiceno;
                                   product_amount = getsale_order_detail.item_price_amount;
                                   var get_sale_order = dba.tbl_sales_order.Where(a => a.id == getsale_order_detail.tbl_sales_order_id).FirstOrDefault();
                                   if (get_sale_order != null)
                                   {
                                        purchase_date = Convert.ToDateTime(get_sale_order.purchase_date).ToString("yyyy-MM-dd").Replace("-", "");                                      
                                        country = get_sale_order.ship_country;
                                        state = get_sale_order.ship_state;
                                        postalcode = get_sale_order.ship_postal_code;
                                        city = get_sale_order.ship_city;                               
                                        city_state = city + "-" + state;
                                        country_pincode = country + "-" + postalcode;
                                        phoneno = "";
                                   }
                               }
                               

                               string Order_id = item.ob_tblhistory.OrderID;
                               double refundprincipal = Convert.ToDouble(item.ob_tblhistory.amount_per_unit);
                               double refundproduct_tax = Convert.ToDouble(item.ob_tblhistory.product_tax);
                               double refundshipping = Convert.ToDouble(item.ob_tblhistory.shipping_price);
                               double refundshipping_tax = Convert.ToDouble(item.ob_tblhistory.shipping_tax);
                               double refundgiftwrap = Convert.ToDouble(item.ob_tblhistory.Giftwrap_price);
                               double refundgiftwrap_tax = Convert.ToDouble(item.ob_tblhistory.gift_wrap_tax);
                               double refundshipping_discount = Convert.ToDouble(item.ob_tblhistory.shipping_discount);
                               double refundshipping_discount_tax = Convert.ToDouble(item.ob_tblhistory.shipping_tax_discount);
                               double refundtotal = (refundprincipal + refundproduct_tax + refundshipping + refundshipping_tax + refundgiftwrap + refundgiftwrap_tax + refundshipping_discount + refundshipping_discount_tax)*(-1);


                               string mysalesbody = messagedetails;

                               mysalesbody = mysalesbody.Replace("#buyeraddress1#", city);
                               mysalesbody = mysalesbody.Replace("#buyeraddress2#", city);
                               mysalesbody = mysalesbody.Replace("#buyeraddress3#", city_state);
                               mysalesbody = mysalesbody.Replace("#buyeraddress4#", country_pincode);
                               mysalesbody = mysalesbody.Replace("#statename#", state);
                               mysalesbody = mysalesbody.Replace("#buyeraddress5#", phoneno);
                               mysalesbody = mysalesbody.Replace("#purchasedate#", purchase_date);
                               mysalesbody = mysalesbody.Replace("#orderid#", Order_id);
                               mysalesbody = mysalesbody.Replace("#bill_amount#", Convert.ToString(refundtotal));
                               mysalesbody = mysalesbody.Replace("#voucherno#", voucher_no);

                               xmloutput += mysalesbody;
                               if (getsale_order_detail != null)
                               {
                                   int deatil_id = getsale_order_detail.id;
                                   string sku = getsale_order_detail.sku_no;
                                   string voucher_no1 = getsale_order_detail.tax_invoiceno;
                                    string shippingamount = "";
                                    string shippingtaxamount = "";
                                    string shipamt = Convert.ToString(getsale_order_detail.shipping_tax_Amount);
                                    double igst_amount = 0;
                                    double cgst_amount = 0;
                                    double sgst_amount = 0;
                                    string igst_taxrate = "";
                                    string cgst_taxrate = "";
                                    string sgst_taxrate = "";

                                    var get_taxdetails = dba.tbl_tax.Where(a => a.tbl_seller_id == sellers_id && a.reference_type == 3 && a.tbl_referneced_id == deatil_id).FirstOrDefault();
                                    if (get_taxdetails != null)
                                    {
                                        if (get_taxdetails.Igst_amount < 0)
                                        {
                                            igst_amount = igst_amount + (Convert.ToDouble(get_taxdetails.Igst_amount)) * (-1);
                                            igst_taxrate = igst_taxrate + (get_taxdetails.igst_tax);
                                        }
                                        else
                                        {
                                            igst_amount = igst_amount + Convert.ToDouble(get_taxdetails.Igst_amount);
                                            

                                            igst_taxrate = igst_taxrate + (get_taxdetails.igst_tax);
                                            string input_decimal_number =Convert.ToString(igst_taxrate);
                                            var regex = new System.Text.RegularExpressions.Regex("(?<=[\\.])[0-9]+");
                                            if (regex.IsMatch(input_decimal_number))
                                            {
                                                string decimal_places = regex.Match(input_decimal_number).Value;
                                                igst_taxrate = decimal_places;
                                            }
                                           
                                            if (shipamt == "0")
                                            {
                                                var abc = (getsale_order_detail.shipping_price_Amount * 100);
                                                double igst =Convert.ToDouble(igst_taxrate);
                                                var aaa = (100 + igst);
                                               
                                                var zzz = Convert.ToDecimal( abc / aaa);
                                               decimal result = decimal.Round(zzz, 1, MidpointRounding.AwayFromZero);
                                               decimal sss = Convert.ToDecimal(getsale_order_detail.shipping_price_Amount - Convert.ToDouble(result));
                                               shippingamount =Convert.ToString(result);
                                               shippingtaxamount = Convert.ToString(sss);
                                            }
                                                                                    
                                        }

                                        if (get_taxdetails.CGST_amount < 0)
                                        {
                                            cgst_amount = cgst_amount + (Convert.ToDouble(get_taxdetails.CGST_amount)) * (-1);
                                            cgst_taxrate = cgst_taxrate + (get_taxdetails.cgst_tax);
                                        }
                                        else
                                        {
                                            cgst_amount = cgst_amount + (Convert.ToDouble(get_taxdetails.CGST_amount));
                                            cgst_taxrate = cgst_taxrate + (get_taxdetails.cgst_tax);
                                            string input_decimal_number1 = Convert.ToString(cgst_taxrate);
                                            var regex = new System.Text.RegularExpressions.Regex("(?<=[\\.])[0-9]+");
                                            if (regex.IsMatch(input_decimal_number1))
                                            {
                                                string decimal_places1 = regex.Match(input_decimal_number1).Value;
                                                cgst_taxrate = decimal_places1;
                                            }
                                           
                                        }

                                        if (get_taxdetails.sgst_amount < 0)
                                        {
                                            sgst_amount = sgst_amount + (Convert.ToDouble(get_taxdetails.sgst_amount)) * (-1);
                                            sgst_taxrate = sgst_taxrate + (get_taxdetails.sgst_tax);
                                        }
                                        else
                                        {
                                            sgst_amount = sgst_amount + (Convert.ToDouble(get_taxdetails.sgst_amount));
                                            sgst_taxrate = sgst_taxrate + (get_taxdetails.sgst_tax);
                                            string input_decimal_number2 = Convert.ToString(sgst_taxrate);
                                            var regex = new System.Text.RegularExpressions.Regex("(?<=[\\.])[0-9]+");
                                            if (regex.IsMatch(input_decimal_number2))
                                            {
                                                string decimal_places2 = regex.Match(input_decimal_number2).Value;
                                                sgst_taxrate = decimal_places2;
                                            }
                                           
                                        }
                                    }// if gettax details

                                    string mysalestaxbody = messagetax;
                                    string mysalestaxbody1 = messagetax;
                                    string mysalestaxbody2 = messagetax;

                                    string localsaleonly= "";
                                    string localsaleigst = "";
                                    if (igst_amount != 0)
                                    {
                                        
                                        string taxname = "IGST@"+ igst_taxrate +"%";
                                         localsaleigst = "Sale" + "" +taxname;
                                         double amout1 = 0;
                                        mysalestaxbody = mysalestaxbody.Replace("#taxname#", taxname);                                       
                                        mysalestaxbody = mysalestaxbody.Replace("#taxamount#", Convert.ToString(igst_amount));
                                        xmloutput += mysalestaxbody;

                                        if (shippingamount != null && shippingamount != "")
                                        {
                                            string taxname3 = "Shipping Charges@" + igst_taxrate + "%";
                                            mysalestaxbody2 = mysalestaxbody2.Replace("#taxname#", taxname3);
                                            mysalestaxbody2 = mysalestaxbody2.Replace("#taxamount#", Convert.ToString(shippingamount));
                                            xmloutput += mysalestaxbody2;
                                        }
                                    }
                                    else 
                                    {
                                        string taxname1 = "CGST@" + cgst_taxrate + "%";
                                        mysalestaxbody = mysalestaxbody.Replace("#taxname#", taxname1);
                                        mysalestaxbody = mysalestaxbody.Replace("#taxamount#", Convert.ToString(cgst_amount));
                                        xmloutput += mysalestaxbody;

                                        string taxname2 = "SGST@" + sgst_taxrate + "%";
                                        double cgst_taxrate1 = Convert.ToDouble(cgst_taxrate);
                                        double sgst_taxrate1 = Convert.ToDouble(sgst_taxrate);
                                        localsaleonly = "Local Sale@" + (cgst_taxrate1 + sgst_taxrate1) + "%";
                                        mysalestaxbody1 = mysalestaxbody1.Replace("#taxname#", taxname2);
                                        mysalestaxbody1 = mysalestaxbody1.Replace("#taxamount#", Convert.ToString(sgst_amount));
                                        xmloutput += mysalestaxbody1;
                                    }                                    
                                    var get_inventorydetais = dba.tbl_inventory.Where(a => a.sku.ToLower() == sku.ToLower() && a.tbl_sellers_id == sellers_id).FirstOrDefault();
                                    string mysalesinventory = messageinventory;
                                    if (get_inventorydetais != null)
                                    {
                                        string name = get_inventorydetais.item_name;
                                        
                                        mysalesinventory = mysalesinventory.Replace("#productname#", name);
                                        mysalesinventory = mysalesinventory.Replace("#productprice#", Convert.ToString(product_amount));
                                        if (localsaleigst != "" && localsaleigst != null)
                                        {
                                            mysalesinventory = mysalesinventory.Replace("#LedgerSaleTax#", localsaleigst);
                                        }
                                        else
                                        {
                                            mysalesinventory = mysalesinventory.Replace("#LedgerSaleTax#", localsaleonly);
                                        }
                                        xmloutput += mysalesinventory;
                                    }
                                  
                                    
                                }// foreach item details
                               xmloutput += messagefooter;

                           }// end of foreach
                           foreach (var item in GetReturnHistoryDetail)
                           {
                               string mycompany = messagecompany;
                               xmloutput += mycompany;
                           }

                           xmloutput += salesfooter;
                       }// end of if (GetReturnHistoryDetail)
                   }
               }
               catch( Exception ex)
               {
               }
               return (xmloutput);
        }
    }
}