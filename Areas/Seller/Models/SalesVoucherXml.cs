using SellerVendor.Areas.Seller.Models.DBContext;
using SellerVendor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SellerVendor.Areas.Seller.Models
{
    public class SalesVoucherXml
    {
        public SellerContext dba = new SellerContext();
        public SellerAdminContext db = new SellerAdminContext();

        string salesheader = 
"<?xml version =\"1.0\"?>\n" +
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
        //VCHKEY=\"e44e5438-bbf7-4b26-94b2-672e136f7320-0000a83b:00000500\"
        string messagedetails =
"       <TALLYMESSAGE xmlns:UDF=\"TallyUDF\">" + "\n" +
"          <VOUCHER REMOTEID=\"#guiduniqueno#\" VCHKEY=\"e44e5438-bbf7-4b26-94b2-672e136f7320-0000a83b:00000500\" VCHTYPE=\"Raintree Sales(S)\" ACTION=\"Create\" OBJVIEW=\"Invoice Voucher View\">" + "\n" +
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
"            <GUID>#guiduniqueno#</GUID>" + "\n" +
"            <STATENAME>#statename#</STATENAME>" + "\n" +
"            <VATDEALERTYPE>Unregistered</VATDEALERTYPE>" + "\n" +
"            <NARRATION>#itemName#</NARRATION>" + "\n" +
"            <PARTYNAME>#salecustomermarketplace#</PARTYNAME>" + "\n" +
"            <VOUCHERTYPENAME>Raintree Sale(S)</VOUCHERTYPENAME>" + "\n" +
"            <REFERENCE>#orderid#</REFERENCE>" + "\n" +
"            <VOUCHERNUMBER>#voucherno#</VOUCHERNUMBER>" + "\n" +
"            <PARTYLEDGERNAME>#salecustomermarketplace#</PARTYLEDGERNAME>" + "\n" +
"            <CSTFORMISSUETYPE/>" + "\n" +
"           <CSTFORMRECVTYPE/>" + "\n" +
"            <FBTPAYMENTTYPE>Default</FBTPAYMENTTYPE>" + "\n" +
"           <PERSISTEDVIEW>Invoice Voucher View</PERSISTEDVIEW>" + "\n" +
"            <BASICSHIPPEDBY>  </BASICSHIPPEDBY>" + "\n" +
"           <BASICBUYERNAME> </BASICBUYERNAME>" + "\n" +
"            <BASICSHIPDOCUMENTNO> </BASICSHIPDOCUMENTNO>" + "\n" +
"            <VCHGSTCLASS/>" + "\n" +
"            <ENTEREDBY> </ENTEREDBY>" + "\n" +
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
"              <BASICSHIPDELIVERYNOTE> </BASICSHIPDELIVERYNOTE>" + "\n" +
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
"              <LEDGERNAME>#salecustomermarketplace#</LEDGERNAME>" + "\n" +
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
"            </LEDGERENTRIES.LIST>" ;

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
"             <REMOTECMPNAME>#companyname#(F.Y. 2016-17)</REMOTECMPNAME>" + "\n" +
"             <REMOTECMPSTATE>Haryana</REMOTECMPSTATE>" + "\n" +
"           </REMOTECMPINFO.LIST>" + "\n" +
"           <REMOTECMPINFO.LIST MERGE=\"Yes\">" + "\n" +
"             <NAME>94cd897a-2b55-4e78-a9f3-a5c1cd6b553b</NAME>" + "\n" +
"             <REMOTECMPNAME>#companyname# (2015-2016)</REMOTECMPNAME>" + "\n" +
"             <REMOTECMPSTATE>Haryana</REMOTECMPSTATE>" + "\n" +
"           </REMOTECMPINFO.LIST>" + "\n" +
"           <REMOTECMPINFO.LIST MERGE=\"Yes\">" + "\n" +
"             <NAME>667b8d34-edaf-4feb-8411-99466ae97cfd</NAME>" + "\n" +
"             <REMOTECMPNAME>#companyname#</REMOTECMPNAME>" + "\n" +
"             <REMOTECMPSTATE>Haryana</REMOTECMPSTATE>" + "\n" +
"           </REMOTECMPINFO.LIST>" + "\n" +
"           <REMOTECMPINFO.LIST MERGE=\"Yes\">" + "\n" +
"             <NAME>db2ac668-c997-44d9-a9a7-5901d78c0b5a</NAME>" + "\n" +
"            <REMOTECMPNAME>#companyname#</REMOTECMPNAME>" + "\n" +
"             <REMOTECMPSTATE>Haryana</REMOTECMPSTATE>" + "\n" +
"           </REMOTECMPINFO.LIST>" + "\n" +
"           <REMOTECMPINFO.LIST MERGE=\"Yes\">" + "\n" +
"             <NAME>c2ff9cf4-62e1-4e6e-ad55-7c1972f9e6e0</NAME>" + "\n" +
"             <REMOTECMPNAME>#companyname#</REMOTECMPNAME>" + "\n" +
"             <REMOTECMPSTATE>Haryana</REMOTECMPSTATE>" + "\n" +
"          </REMOTECMPINFO.LIST>" + "\n" +
"        </COMPANY>" + "\n" +
"      </TALLYMESSAGE>\n";

        string messagecompany1=
"       <TALLYMESSAGE xmlns:UDF=\"TallyUDF\">" + "\n" +
"          <COMPANY>" + "\n" +
"            <REMOTECMPINFO.LIST MERGE=\"Yes\">" + "\n" +
"              <NAME>e44e5438-bbf7-4b26-94b2-672e136f7320</NAME>" + "\n" +
"             <REMOTECMPNAME>#companyname#</REMOTECMPNAME>" + "\n" +
"             <REMOTECMPSTATE>Haryana</REMOTECMPSTATE>" + "\n" +
"           </REMOTECMPINFO.LIST>" + "\n" +
"           <REMOTECMPINFO.LIST MERGE=\"Yes\">" + "\n" +
"             <NAME>94cd897a-2b55-4e78-a9f3-a5c1cd6b553b</NAME>" + "\n" +
"             <REMOTECMPNAME>#companyname#</REMOTECMPNAME>" + "\n" +
"             <REMOTECMPSTATE>Haryana</REMOTECMPSTATE>" + "\n" +
"           </REMOTECMPINFO.LIST>" + "\n" +
"           <REMOTECMPINFO.LIST MERGE=\"Yes\">" + "\n" +
"             <NAME>667b8d34-edaf-4feb-8411-99466ae97cfd</NAME>" + "\n" +
"             <REMOTECMPNAME>#companyname#</REMOTECMPNAME>" + "\n" +
"             <REMOTECMPSTATE>Haryana</REMOTECMPSTATE>" + "\n" +
"           </REMOTECMPINFO.LIST>" + "\n" +
"           <REMOTECMPINFO.LIST MERGE=\"Yes\">" + "\n" +
"             <NAME>db2ac668-c997-44d9-a9a7-5901d78c0b5a</NAME>" + "\n" +
"            <REMOTECMPNAME>#companyname#</REMOTECMPNAME>" + "\n" +
"             <REMOTECMPSTATE>Haryana</REMOTECMPSTATE>" + "\n" +
"           </REMOTECMPINFO.LIST>" + "\n" +
"           <REMOTECMPINFO.LIST MERGE=\"Yes\">" + "\n" +
"             <NAME>c2ff9cf4-62e1-4e6e-ad55-7c1972f9e6e0</NAME>" + "\n" +
"             <REMOTECMPNAME>#companyname#</REMOTECMPNAME>" + "\n" +
"             <REMOTECMPSTATE>Haryana</REMOTECMPSTATE>" + "\n" +
"          </REMOTECMPINFO.LIST>" + "\n" +
"        </COMPANY>" + "\n" +
"      </TALLYMESSAGE>\n";

        
        string salesfooter =
"     </REQUESTDATA>" + "\n" +
"   </IMPORTDATA>" + "\n" +
"  </BODY>" + "\n" +
"</ENVELOPE\n";
        //public string SalesVoucherXML(List<SaleReport> lstOrdertext2, string CompanyName) <GUID>e44e5438-bbf7-4b26-94b2-672e136f7320-00015bc1</GUID>
        public string SalesVoucherXML(DateTime? txt_from, DateTime? txt_to, int? sellers_id, string CompanyName, string MarketPlace, int? ddl_market_place)
        {
            string xmloutput = "";
            try
            {
                if (txt_from != null && txt_to != null)
                {
                    var GetSaleOrderDetail = (from ob_tbl_sales_order in dba.tbl_sales_order
                                              select new
                                              {
                                                  ob_tbl_sales_order = ob_tbl_sales_order
                                              }).Where(a => a.ob_tbl_sales_order.tbl_sellers_id == sellers_id && a.ob_tbl_sales_order.tbl_Marketplace_Id == ddl_market_place && a.ob_tbl_sales_order.order_status != "Cancelled").ToList();

                    if (GetSaleOrderDetail != null)
                    {
                        if (txt_from != null && txt_to != null)
                        {
                            GetSaleOrderDetail = GetSaleOrderDetail.Where(a => a.ob_tbl_sales_order.purchase_date.Date >= txt_from && a.ob_tbl_sales_order.purchase_date.Date <= txt_to).ToList();
                        }
                    }
                    string Guid1 = "";
                    if (GetSaleOrderDetail != null)
                    {
                        
                        string myvoucherHeader = salesheader;
                        myvoucherHeader = myvoucherHeader.Replace("#companyname#", CompanyName);

                        xmloutput += myvoucherHeader;
                        int counter = 0;
                        foreach (var item in GetSaleOrderDetail)
                        {
                            //counter++;
                            //if (counter > 5)
                            //{
                            //    break;
                            //} 
                            Guid g;
                            // Create and display the value of two GUIDs.
                            g = Guid.NewGuid();
                            DateTime dt = DateTime.Now;
                            string g1 = g + "-" + dt.ToString("ddMMyyyy-HHMMssff");

                            //if (item.ob_tbl_sales_order.amazon_order_id == "404-9746292-0961142")
                            //{
                            //}
                            var get_guid_no = dba.tbl_seller_accounting_pkg_details.Where(a => a.seller_id == sellers_id).FirstOrDefault();
                                if (get_guid_no != null)
                                {
                                    DateTime dt1 = DateTime.Now;
                                    string value = dt1.ToString("ddMMyyyy-HHMMssff");
                                    Guid1 = get_guid_no.guid + "-" + value;
                                }
                            double promotiondiscount = 0;
                            double product_amount = 0;
                            string voucher_no ="",productname="";
                           // double Shippingamount = 0;
                            var get_saleorder_details = dba.tbl_sales_order_details.Where(a => a.tbl_sales_order_id == item.ob_tbl_sales_order.id && a.tbl_seller_id == sellers_id).ToList();
                            foreach (var itemDetail1 in get_saleorder_details)
                            {
                                
                                promotiondiscount = promotiondiscount + itemDetail1.promotion_amount;
                                voucher_no = itemDetail1.tax_invoiceno;
                                product_amount = itemDetail1.item_price_amount;
                                productname = itemDetail1.product_name;
                               // if()
                                 //var get_inventory = dba.tbl_inventory.Where(a =>a.sku.ToLower()== itemDetail1.sku_no.ToLower())
                            }
                            string country = "", state = "", postalcode = "", city = "", phoneno = "", city_state = "", country_pincode = "";
                            var get_customer_details = dba.tbl_customer_details.Where(a => a.id == item.ob_tbl_sales_order.tbl_Customer_Id && a.tbl_seller_id == sellers_id).FirstOrDefault();
                            if (get_customer_details != null)
                            {
                                country = get_customer_details.Country_Code;
                                state = get_customer_details.State_Region;
                                postalcode = get_customer_details.Postal_Code;
                                city = get_customer_details.City;
                                city_state = city + "-" + state;
                                country_pincode = country + "-" + postalcode;
                                phoneno = "";
                            }

                            string purchase_date = Convert.ToDateTime(item.ob_tbl_sales_order.purchase_date).ToString("yyyy-MM-dd").Replace("-", "");
                            //purchase_date = "20171201";
                            string Order_id = item.ob_tbl_sales_order.amazon_order_id;
                           
                            double amount = (item.ob_tbl_sales_order.bill_amount - promotiondiscount)*(-1);
                            
                            string salemarketplace = "Sale-Customer-" + MarketPlace;
                            

                            string mysalesbody = messagedetails;

                            mysalesbody = mysalesbody.Replace("#guiduniqueno#", g1.ToString());
                            mysalesbody = mysalesbody.Replace("#buyeraddress1#", city);
                            mysalesbody = mysalesbody.Replace("#buyeraddress2#", city);
                            mysalesbody = mysalesbody.Replace("#buyeraddress3#", city_state);
                            mysalesbody = mysalesbody.Replace("#buyeraddress4#", country_pincode);
                            mysalesbody = mysalesbody.Replace("#statename#", state);
                            mysalesbody = mysalesbody.Replace("#buyeraddress5#", phoneno);
                            mysalesbody = mysalesbody.Replace("#purchasedate#", purchase_date);
                            mysalesbody = mysalesbody.Replace("#orderid#", Order_id);
                            mysalesbody = mysalesbody.Replace("#bill_amount#", Convert.ToString(amount));
                            mysalesbody = mysalesbody.Replace("#voucherno#", voucher_no);//itemName
                            mysalesbody = mysalesbody.Replace("#itemName#", productname);
                            mysalesbody = mysalesbody.Replace("#salecustomermarketplace#", salemarketplace);

                            xmloutput += mysalesbody;

                            
                            if (get_saleorder_details != null)
                            {
                                double igst_amount = 0;
                                double cgst_amount = 0;
                                double sgst_amount = 0;
                                string igst_taxrate = "";
                                string cgst_taxrate = "";
                                string sgst_taxrate = "";
                                foreach (var itemDetail in get_saleorder_details)
                                {
                                    //promotiondiscount = promotiondiscount + itemDetail.promotion_amount;
                                    int deatil_id = itemDetail.id;
                                    string sku = itemDetail.sku_no;
                                    string voucher_no1 = itemDetail.tax_invoiceno;
                                    string shippingamount = "";
                                    string shippingtaxamount = "";
                                    string shipamt = Convert.ToString(itemDetail.shipping_price_Amount);
                                    string shipamt_tax =Convert.ToString(itemDetail.shipping_tax_Amount);

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
                                                //if (Convert.ToInt16(decimal_places) < 10)
                                                //{
                                                //    igst_taxrate = decimal_places;
                                                //}
                                                igst_taxrate =Convert.ToString(float.Parse( decimal_places));
                                            }
                                           
                                            //if (shipamt == "0")
                                            //{
                                            //    var abc = (itemDetail.shipping_price_Amount * 100) ;
                                            //    double igst =Convert.ToDouble(igst_taxrate);
                                            //    var aaa = (100 + igst);
                                               
                                            //    var zzz = Convert.ToDecimal( abc / aaa);
                                            //   decimal result = decimal.Round(zzz, 2, MidpointRounding.AwayFromZero);
                                            //   decimal sss =Convert.ToDecimal(itemDetail.shipping_price_Amount - Convert.ToDouble(result));
                                            //   shippingamount =Convert.ToString(result);
                                            //   shippingtaxamount = Convert.ToString(sss);
                                            //}

                                            if (shipamt == "0")
                                            {
                                                var abc = (itemDetail.shipping_price_Amount * 100);
                                                double igst = Convert.ToDouble(igst_taxrate);
                                                var aaa = (100 + igst);

                                                var zzz = Convert.ToDecimal(abc / aaa);
                                                decimal result = decimal.Round(zzz, 2, MidpointRounding.AwayFromZero);
                                                decimal sss = Convert.ToDecimal(itemDetail.shipping_price_Amount - Convert.ToDouble(result));
                                                shippingamount = Convert.ToString(result);
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
                                       // double amout = igst_amount; //+ Convert.ToDouble(shippingtaxamount);
                                        //if (shippingtaxamount != "" && shippingtaxamount != null)
                                        //{
                                             //amout1 = Convert.ToDouble(shippingtaxamount);
                                        //}
                                        //double amout = igst_amount + amout1;
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

                                        double cgst_taxrate1 = 0;
                                        double sgst_taxrate1 = 0;
                                        string taxname2 = "SGST@" + sgst_taxrate + "%";
                                        if (cgst_taxrate != "" && cgst_taxrate != null)
                                        {
                                             cgst_taxrate1 = Convert.ToDouble(cgst_taxrate);
                                        }
                                        
                                        if (sgst_taxrate != "" && sgst_taxrate != null)
                                        {
                                            sgst_taxrate1 = Convert.ToDouble(sgst_taxrate);
                                        }
                                        
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
                                        double ship_amt = 0;
                                        if (shippingamount != "" && shippingamount != null)
                                        {
                                             ship_amt = Convert.ToDouble(shippingamount);
                                        }
                                        product_amount = product_amount - ship_amt;
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


                            }// if sale order details
                            
                            xmloutput += messagefooter;

                        }// end of forloop main

                        //foreach (var item in GetSaleOrderDetail)
                        //{
                        //    string mycompany = messagecompany;
                        //    xmloutput += mycompany;
                        //}
                        string mycompany = messagecompany;
                        mycompany = mycompany.Replace("#companyname#", CompanyName);
                        xmloutput += mycompany;
                       

                        string mycompany1 = messagecompany1;
                        mycompany1 = mycompany1.Replace("#companyname#", CompanyName);
                        xmloutput += mycompany1;

                        xmloutput += salesfooter;

                    }
                }// end of if
             
            }// end of try
            catch(Exception ex)
            {
            }
            return (xmloutput);
        }
    }
}