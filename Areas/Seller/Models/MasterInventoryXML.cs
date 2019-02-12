using SellerVendor.Areas.Seller.Models.DBContext;
using SellerVendor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SellerVendor.Areas.Seller.Models
{
    public class MasterInventoryXML
    {
        public SellerContext dba = new SellerContext();
        public SellerAdminContext db = new SellerAdminContext();

        string VoucherHeader =
"<ENVELOPE>\n" +
"  <HEADER>\n" +
"    <TALLYREQUEST>Import Data</TALLYREQUEST>\n" +
"  </HEADER>\n" +
"  <BODY>\n" +
"    <IMPORTDATA>\n" +
"    <REQUESTDESC>\n" +
"      <REPORTNAME>All Masters</REPORTNAME>\n" +
"      <STATICVARIABLES>\n" +
"       <SVCURRENTCOMPANY>Test Amazon</SVCURRENTCOMPANY>\n" +
"      </STATICVARIABLES>\n" +
"     </REQUESTDESC>\n" +
"     <REQUESTDATA>\n";

        string voucherNumber = 
"     <TALLYMESSAGE xmlns:UDF=\"TallyUDF\">" + "\n" +
"        <UNIT NAME=\"Nos.\" RESERVEDNAME=\"\">" + "\n" +
"        <NAME>Nos.</NAME>" + "\n" +
"        <GUID>#guiduniqueno#</GUID>" + "\n" +
"        <ORIGINALNAME>Number</ORIGINALNAME>" + "\n" +
"        <GSTREPUOM>NOS-NUMBERS</GSTREPUOM>" + "\n" +
"        <ISUPDATINGTARGETID>No</ISUPDATINGTARGETID>" + "\n" +
"        <ASORIGINAL>Yes</ASORIGINAL>" + "\n" +
"        <ISSIMPLEUNIT>Yes</ISSIMPLEUNIT>" + "\n" +
"       <ALTERID> 414</ALTERID>" + "\n" +
"       </UNIT>" + "\n" +
"     </TALLYMESSAGE>\n";

        string VoucherMoney =
"     <TALLYMESSAGE xmlns:UDF=\"TallyUDF\">" + "\n" +
"       <CURRENCY NAME=\"₹\" RESERVEDNAME=\"\">" + "\n" +
"         <GUID>#guiduniqueno#</GUID>" + "\n" +
"         <MAILINGNAME>INR</MAILINGNAME>" + "\n" +
"         <ORIGINALNAME>₹</ORIGINALNAME>" + "\n" +
"         <EXPANDEDSYMBOL>INR</EXPANDEDSYMBOL>" + "\n" +
"         <DECIMALSYMBOL>paise</DECIMALSYMBOL>" + "\n" +
"         <ISUPDATINGTARGETID>No</ISUPDATINGTARGETID>" + "\n" +
"         <ASORIGINAL>Yes</ASORIGINAL>" + "\n" +
"         <ISSUFFIX>No</ISSUFFIX>" + "\n" +
"         <HASSPACE>Yes</HASSPACE>" + "\n" +
"         <INMILLIONS>No</INMILLIONS>" + "\n" +
"         <ALTERID> 363</ALTERID>" + "\n" +
"         <DECIMALPLACES> 2</DECIMALPLACES>" + "\n" +
"         <DECIMALPLACESFORPRINTING> 2</DECIMALPLACESFORPRINTING>" + "\n" +
"         <DAILYSTDRATES.LIST>      </DAILYSTDRATES.LIST>" + "\n" +
"         <DAILYBUYINGRATES.LIST>      </DAILYBUYINGRATES.LIST>" + "\n" +
"         <DAILYSELLINGRATES.LIST>      </DAILYSELLINGRATES.LIST>" + "\n" +
"        </CURRENCY>" + "\n" +
"      </TALLYMESSAGE>\n";

        string VoucherMoney1 =
"      <TALLYMESSAGE xmlns:UDF=\"TallyUDF\">" + "\n" +
"        <CURRENCY NAME=\"₹\" RESERVEDNAME=\"\">" + "\n" +
"          <GUID>#guiduniqueno#</GUID>" + "\n" +
"          <MAILINGNAME>INR</MAILINGNAME>" + "\n" +
"          <ORIGINALNAME>₹</ORIGINALNAME>" + "\n" +
"          <EXPANDEDSYMBOL>INR</EXPANDEDSYMBOL>" + "\n" +
"          <DECIMALSYMBOL>paise</DECIMALSYMBOL>" + "\n" +
"          <ISUPDATINGTARGETID>No</ISUPDATINGTARGETID>" + "\n" +
"          <ASORIGINAL>Yes</ASORIGINAL>" + "\n" +
"          <ISSUFFIX>No</ISSUFFIX>" + "\n" +
"          <HASSPACE>Yes</HASSPACE>" + "\n" +
"          <INMILLIONS>No</INMILLIONS>" + "\n" +
"          <ALTERID> 363</ALTERID>" + "\n" +
"          <DECIMALPLACES> 2</DECIMALPLACES>" + "\n" +
"          <DECIMALPLACESFORPRINTING> 2</DECIMALPLACESFORPRINTING>" + "\n" +
"          <DAILYSTDRATES.LIST>      </DAILYSTDRATES.LIST>" + "\n" +
"          <DAILYBUYINGRATES.LIST>      </DAILYBUYINGRATES.LIST>" + "\n" +
"          <DAILYSELLINGRATES.LIST>      </DAILYSELLINGRATES.LIST>" + "\n" +
"        </CURRENCY>" + "\n" +
"      </TALLYMESSAGE>\n";

        string voucherWarehouse =
"      <TALLYMESSAGE xmlns:UDF=\"TallyUDF\">" + "\n" +
"        <GODOWN NAME=\"Main Location\" RESERVEDNAME=\"\">" + "\n" +
"         <GUID>#guiduniqueno#</GUID>" + "\n" +
"         <PARENT/>" + "\n" +
"         <JOBNAME/>" + "\n" +
"         <ARE1SERIALMASTER/>" + "\n" +
"         <ARE2SERIALMASTER/>" + "\n" +
"         <ARE3SERIALMASTER/>" + "\n" +
"         <TAXUNITNAME/>" + "\n" +
"         <ISUPDATINGTARGETID>No</ISUPDATINGTARGETID>" + "\n" +
"         <ASORIGINAL>Yes</ASORIGINAL>" + "\n" +
"         <HASNOSPACE>No</HASNOSPACE>" + "\n" +
"         <HASNOSTOCK>No</HASNOSTOCK>" + "\n" +
"         <ISEXTERNAL>No</ISEXTERNAL>" + "\n" +
"         <ISINTERNAL>No</ISINTERNAL>" + "\n" +
"         <ENABLEEXPORT>No</ENABLEEXPORT>" + "\n" +
"         <ISPRIMARYEXCISEUNIT>No</ISPRIMARYEXCISEUNIT>" + "\n" +
"         <ALLOWEXPORTREBATE>No</ALLOWEXPORTREBATE>" + "\n" +
"         <ISTRADERRGNUMBERON>No</ISTRADERRGNUMBERON>" + "\n" +
"         <ALTERID> 63</ALTERID>" + "\n" +
"         <LANGUAGENAME.LIST>" + "\n" +
"           <NAME.LIST TYPE=\"String\">" + "\n" +
"            <NAME>Main Location</NAME>" + "\n" +
"          </NAME.LIST>" + "\n" +
"          <LANGUAGEID> 1033</LANGUAGEID>" + "\n" +
"         </LANGUAGENAME.LIST>" + "\n" +
"        <SCHVIDETAILS.LIST>      </SCHVIDETAILS.LIST>" + "\n" +
"        <EXCISETARIFFDETAILS.LIST>      </EXCISETARIFFDETAILS.LIST>" + "\n" +
"        <SERIALNUMBERLIST.LIST>      </SERIALNUMBERLIST.LIST>" + "\n" +
"        <EXCISEMFGDETAILS.LIST>      </EXCISEMFGDETAILS.LIST>" + "\n" +
"        <EXCISELUTDETAILS.LIST>      </EXCISELUTDETAILS.LIST>" + "\n" +
"        <EXCISEBONDDETAILS.LIST>      </EXCISEBONDDETAILS.LIST>" + "\n" +
"        <EXCISERANGEDETAILS.LIST>      </EXCISERANGEDETAILS.LIST>" + "\n" +
"        <EXCISEDIVISIONDETAILS.LIST>      </EXCISEDIVISIONDETAILS.LIST>" + "\n" +
"        <EXCISECOMMISSIONERATEDETAILS.LIST>      </EXCISECOMMISSIONERATEDETAILS.LIST>" + "\n" +
"        <EXTARIFFDUTYHEADDETAILS.LIST>      </EXTARIFFDUTYHEADDETAILS.LIST>" + "\n" +
"       </GODOWN>" + "\n" +
"     </TALLYMESSAGE>\n";

        string voucherinventory =
"      <TALLYMESSAGE xmlns:UDF=\"TallyUDF\">" + "\n" +
"        <STOCKITEM NAME=\"#itemName#\" RESERVEDNAME=\"\">" + "\n" +
"        <OLDAUDITENTRYIDS.LIST TYPE=\"Number\">" + "\n" +
"          <OLDAUDITENTRYIDS>-1</OLDAUDITENTRYIDS>" + "\n" +
"        </OLDAUDITENTRYIDS.LIST>" + "\n" +
"        <GUID>#guiduniqueno#</GUID>" + "\n" +
"        <PARENT/>" + "\n" +
"        <CATEGORY/>" + "\n" +
"        <GSTAPPLICABLE>&#4; Applicable</GSTAPPLICABLE>" + "\n" +
"        <TAXCLASSIFICATIONNAME/>" + "\n" +
"        <GSTTYPEOFSUPPLY>Goods</GSTTYPEOFSUPPLY>" + "\n" +
"        <EXCISEAPPLICABILITY>&#4; Applicable</EXCISEAPPLICABILITY>" + "\n" +
"        <SALESTAXCESSAPPLICABLE/>" + "\n" +
"        <VATAPPLICABLE>&#4; Applicable</VATAPPLICABLE>" + "\n" +
"        <COSTINGMETHOD>Avg. Cost</COSTINGMETHOD>" + "\n" +
"        <VALUATIONMETHOD>Avg. Price</VALUATIONMETHOD>" + "\n" +
"        <BASEUNITS>Nos.</BASEUNITS>" + "\n" +
"        <ADDITIONALUNITS/>" + "\n" +
"        <EXCISEITEMCLASSIFICATION/>" + "\n" +
"        <VATBASEUNIT>Nos.</VATBASEUNIT>" + "\n" +
"        <ISCOSTCENTRESON>No</ISCOSTCENTRESON>" + "\n" +
"        <ISBATCHWISEON>No</ISBATCHWISEON>" + "\n" +
"        <ISPERISHABLEON>No</ISPERISHABLEON>" + "\n" +
"        <ISENTRYTAXAPPLICABLE>No</ISENTRYTAXAPPLICABLE>" + "\n" +
"        <ISCOSTTRACKINGON>No</ISCOSTTRACKINGON>" + "\n" +
"        <ISUPDATINGTARGETID>No</ISUPDATINGTARGETID>" + "\n" +
"        <ASORIGINAL>Yes</ASORIGINAL>" + "\n" +
"        <ISRATEINCLUSIVEVAT>No</ISRATEINCLUSIVEVAT>" + "\n" +
"        <IGNOREPHYSICALDIFFERENCE>No</IGNOREPHYSICALDIFFERENCE>" + "\n" +
"        <IGNORENEGATIVESTOCK>No</IGNORENEGATIVESTOCK>" + "\n" +
"        <TREATSALESASMANUFACTURED>No</TREATSALESASMANUFACTURED>" + "\n" +
"        <TREATPURCHASESASCONSUMED>No</TREATPURCHASESASCONSUMED>" + "\n" +
"        <TREATREJECTSASSCRAP>No</TREATREJECTSASSCRAP>" + "\n" +
"        <HASMFGDATE>No</HASMFGDATE>" + "\n" +
"        <ALLOWUSEOFEXPIREDITEMS>No</ALLOWUSEOFEXPIREDITEMS>" + "\n" +
"        <IGNOREBATCHES>No</IGNOREBATCHES>" + "\n" +
"        <IGNOREGODOWNS>No</IGNOREGODOWNS>" + "\n" +
"        <CALCONMRP>No</CALCONMRP>" + "\n" +
"        <EXCLUDEJRNLFORVALUATION>No</EXCLUDEJRNLFORVALUATION>" + "\n" +
"        <ISMRPINCLOFTAX>No</ISMRPINCLOFTAX>" + "\n" +
"        <ISADDLTAXEXEMPT>No</ISADDLTAXEXEMPT>" + "\n" +
"        <ISSUPPLEMENTRYDUTYON>No</ISSUPPLEMENTRYDUTYON>" + "\n" +
"        <REORDERASHIGHER>No</REORDERASHIGHER>" + "\n" +
"        <MINORDERASHIGHER>No</MINORDERASHIGHER>" + "\n" +
"        <ISEXCISECALCULATEONMRP>No</ISEXCISECALCULATEONMRP>" + "\n" +
"        <INCLUSIVETAX>No</INCLUSIVETAX>" + "\n" +
"        <GSTCALCSLABONMRP>No</GSTCALCSLABONMRP>" + "\n" +
"        <MODIFYMRPRATE>No</MODIFYMRPRATE>" + "\n" +
"        <ALTERID> 423</ALTERID>" + "\n" +
"        <DENOMINATOR> 1</DENOMINATOR>" + "\n" +
"        <BASICRATEOFEXCISE> #IGST#</BASICRATEOFEXCISE>" + "\n" +
"        <RATEOFVAT>0</RATEOFVAT>" + "\n" +
"        <VATBASENO> 1</VATBASENO>" + "\n" +
"        <VATTRAILNO> 1</VATTRAILNO>" + "\n" +
"        <VATACTUALRATIO> 1</VATACTUALRATIO>" + "\n" +
"        <SERVICETAXDETAILS.LIST>      </SERVICETAXDETAILS.LIST>" + "\n" +
"        <VATDETAILS.LIST>      </VATDETAILS.LIST>" + "\n" +
"        <SALESTAXCESSDETAILS.LIST>      </SALESTAXCESSDETAILS.LIST>" + "\n" +
"        <GSTDETAILS.LIST>" + "\n" +
"          <APPLICABLEFROM>20170701</APPLICABLEFROM>" + "\n" +
"          <CALCULATIONTYPE>On Value</CALCULATIONTYPE>" + "\n" +
"          <HSNCODE>6402</HSNCODE>" + "\n" +
"          <HSNMASTERNAME/>" + "\n" +
"          <HSN>#itemName#</HSN>" + "\n" +
"          <TAXABILITY>Taxable</TAXABILITY>" + "\n" +
"          <ISREVERSECHARGEAPPLICABLE>No</ISREVERSECHARGEAPPLICABLE>" + "\n" +
"          <ISNONGSTGOODS>No</ISNONGSTGOODS>" + "\n" +
"          <GSTINELIGIBLEITC>No</GSTINELIGIBLEITC>" + "\n" +
"          <INCLUDEEXPFORSLABCALC>No</INCLUDEEXPFORSLABCALC>" + "\n" +
"          <STATEWISEDETAILS.LIST>" + "\n" +
"            <STATENAME>&#4; Any</STATENAME>" + "\n" +
"            <RATEDETAILS.LIST>" + "\n" +
"               <GSTRATEDUTYHEAD>Central Tax</GSTRATEDUTYHEAD>" + "\n" +
"               <GSTRATEVALUATIONTYPE>Based on Value</GSTRATEVALUATIONTYPE>" + "\n" +
"               <GSTRATE>#SGST#</GSTRATE>" + "\n" +
"            </RATEDETAILS.LIST>" + "\n" +
"            <RATEDETAILS.LIST>" + "\n" +
"               <GSTRATEDUTYHEAD>State Tax</GSTRATEDUTYHEAD>" + "\n" +
"               <GSTRATEVALUATIONTYPE>Based on Value</GSTRATEVALUATIONTYPE>" + "\n" +
"               <GSTRATE>#CGST#</GSTRATE>" + "\n" +
"            </RATEDETAILS.LIST>" + "\n" +
"            <RATEDETAILS.LIST>" + "\n" +
"               <GSTRATEDUTYHEAD>Integrated Tax</GSTRATEDUTYHEAD>" + "\n" +
"               <GSTRATEVALUATIONTYPE>Based on Value</GSTRATEVALUATIONTYPE>" + "\n" +
"               <GSTRATE> #IGST#</GSTRATE>" + "\n" +
"            </RATEDETAILS.LIST>" + "\n" +
"            <RATEDETAILS.LIST>" + "\n" +
"               <GSTRATEDUTYHEAD>Cess</GSTRATEDUTYHEAD>" + "\n" +
"               <GSTRATEVALUATIONTYPE>Based on Value</GSTRATEVALUATIONTYPE>" + "\n" +
"            </RATEDETAILS.LIST>" + "\n" +
"            <RATEDETAILS.LIST>" + "\n" +
"               <GSTRATEDUTYHEAD>Cess on Qty</GSTRATEDUTYHEAD>" + "\n" +
"               <GSTRATEVALUATIONTYPE>Based on Quantity</GSTRATEVALUATIONTYPE>" + "\n" +
"            </RATEDETAILS.LIST>" + "\n" +
"            <GSTSLABRATES.LIST>        </GSTSLABRATES.LIST>" + "\n" +
"           </STATEWISEDETAILS.LIST>" + "\n" +
"           <TEMPGSTDETAILSLABRATES.LIST>       </TEMPGSTDETAILSLABRATES.LIST>" + "\n" +
"         </GSTDETAILS.LIST>" + "\n" +
"         <LANGUAGENAME.LIST>" + "\n" +
"           <NAME.LIST TYPE=\"String\">" + "\n" +
"             <NAME>#itemName#</NAME>" + "\n" +
"           </NAME.LIST>" + "\n" +
"           <LANGUAGEID> 1033</LANGUAGEID>" + "\n" +
"         </LANGUAGENAME.LIST>" + "\n" +
"         <SCHVIDETAILS.LIST>      </SCHVIDETAILS.LIST>" + "\n" +
"         <EXCISETARIFFDETAILS.LIST>      </EXCISETARIFFDETAILS.LIST>" + "\n" +
"         <TCSCATEGORYDETAILS.LIST>      </TCSCATEGORYDETAILS.LIST>" + "\n" +
"         <TDSCATEGORYDETAILS.LIST>      </TDSCATEGORYDETAILS.LIST>" + "\n" +
"         <EXCLUDEDTAXATIONS.LIST>      </EXCLUDEDTAXATIONS.LIST>" + "\n" +
"         <OLDAUDITENTRIES.LIST>      </OLDAUDITENTRIES.LIST>" + "\n" +
"         <ACCOUNTAUDITENTRIES.LIST>      </ACCOUNTAUDITENTRIES.LIST>" + "\n" +
"         <AUDITENTRIES.LIST>      </AUDITENTRIES.LIST>" + "\n" +
"         <MRPDETAILS.LIST>      </MRPDETAILS.LIST>" + "\n" +
"         <VATCLASSIFICATIONDETAILS.LIST>      </VATCLASSIFICATIONDETAILS.LIST>" + "\n" +
"         <COMPONENTLIST.LIST>      </COMPONENTLIST.LIST>" + "\n" +
"         <ADDITIONALLEDGERS.LIST>      </ADDITIONALLEDGERS.LIST>" + "\n" +
"         <SALESLIST.LIST>      </SALESLIST.LIST>" + "\n" +
"         <PURCHASELIST.LIST>      </PURCHASELIST.LIST>" + "\n" +
"         <FULLPRICELIST.LIST>      </FULLPRICELIST.LIST>" + "\n" +
"         <BATCHALLOCATIONS.LIST>      </BATCHALLOCATIONS.LIST>" + "\n" +
"         <TRADEREXCISEDUTIES.LIST>      </TRADEREXCISEDUTIES.LIST>" + "\n" +
"         <STANDARDCOSTLIST.LIST>      </STANDARDCOSTLIST.LIST>" + "\n" +
"         <STANDARDPRICELIST.LIST>      </STANDARDPRICELIST.LIST>" + "\n" +
"         <EXCISEITEMGODOWN.LIST>      </EXCISEITEMGODOWN.LIST>" + "\n" +
"         <MULTICOMPONENTLIST.LIST>      </MULTICOMPONENTLIST.LIST>" + "\n" +
"         <LBTDETAILS.LIST>      </LBTDETAILS.LIST>" + "\n" +
"         <PRICELEVELLIST.LIST>      </PRICELEVELLIST.LIST>" + "\n" +
"         <GSTCLASSFNIGSTRATES.LIST>      </GSTCLASSFNIGSTRATES.LIST>" + "\n" +
"         <EXTARIFFDUTYHEADDETAILS.LIST>      </EXTARIFFDUTYHEADDETAILS.LIST>" + "\n" +
"         <TEMPGSTITEMSLABRATES.LIST>      </TEMPGSTITEMSLABRATES.LIST>" + "\n" +
"        </STOCKITEM>" + "\n" +
"       </TALLYMESSAGE>\n" ;

        string voucherAttendance =
"       <TALLYMESSAGE xmlns:UDF=\"TallyUDF\">" + "\n" +
"         <VOUCHERTYPE NAME=\"Attendance\" RESERVEDNAME=\"Attendance\">" + "\n" +
"         <GUID>df13ea43-535b-411e-a3ae-09a30f52223d-00000037</GUID>" + "\n" +
"         <PARENT>Attendance</PARENT>" + "\n" +
"         <MAILINGNAME>Attd</MAILINGNAME>" + "\n" +
"         <TAXUNITNAME>&#4; Primary</TAXUNITNAME>" + "\n" +
"         <NUMBERINGMETHOD>Automatic</NUMBERINGMETHOD>" + "\n" +
"         <EXCISEUNITNAME>&#4; Primary</EXCISEUNITNAME>" + "\n" +
"         <ISUPDATINGTARGETID>No</ISUPDATINGTARGETID>" + "\n" +
"         <ASORIGINAL>Yes</ASORIGINAL>" + "\n" +
"         <ISDEEMEDPOSITIVE>Yes</ISDEEMEDPOSITIVE>" + "\n" +
"         <AFFECTSSTOCK>No</AFFECTSSTOCK>" + "\n" +
"         <PREVENTDUPLICATES>No</PREVENTDUPLICATES>" + "\n" +
"         <PREFILLZERO>No</PREFILLZERO>" + "\n" +
"         <PRINTAFTERSAVE>No</PRINTAFTERSAVE>" + "\n" +
"         <FORMALRECEIPT>No</FORMALRECEIPT>" + "\n" +
"         <ISOPTIONAL>No</ISOPTIONAL>" + "\n" +
"         <ASMFGJRNL>No</ASMFGJRNL>" + "\n" +
"         <EFFECTIVEDATE>No</EFFECTIVEDATE>" + "\n" +
"         <COMMONNARRATION>Yes</COMMONNARRATION>" + "\n" +
"         <MULTINARRATION>No</MULTINARRATION>" + "\n" +
"         <ISTAXINVOICE>No</ISTAXINVOICE>" + "\n" +
"         <USEFORPOSINVOICE>No</USEFORPOSINVOICE>" + "\n" +
"         <USEFOREXCISETRADERINVOICE>No</USEFOREXCISETRADERINVOICE>" + "\n" +
"         <USEFOREXCISE>No</USEFOREXCISE>" + "\n" +
"         <USEFORJOBWORK>No</USEFORJOBWORK>" + "\n" +
"         <ISFORJOBWORKIN>No</ISFORJOBWORKIN>" + "\n" +
"         <ALLOWCONSUMPTION>No</ALLOWCONSUMPTION>" + "\n" +
"         <USEFOREXCISEGOODS>No</USEFOREXCISEGOODS>" + "\n" +
"         <USEFOREXCISESUPPLEMENTARY>No</USEFOREXCISESUPPLEMENTARY>" + "\n" +
"         <ISDEFAULTALLOCENABLED>No</ISDEFAULTALLOCENABLED>" + "\n" +
"         <SORTPOSITION> 240</SORTPOSITION>" + "\n" +
"         <ALTERID> 56</ALTERID>" + "\n" +
"         <BEGINNINGNUMBER> 1</BEGINNINGNUMBER>" + "\n" +
"         <LANGUAGENAME.LIST>" + "\n" +
"          <NAME.LIST TYPE=\"String\">" + "\n" +
"           <NAME>Attendance</NAME>" + "\n" +
"           </NAME.LIST>" + "\n" +
"           <LANGUAGEID> 1033</LANGUAGEID>" + "\n" +
"          </LANGUAGENAME.LIST>" + "\n" +
"          <AUDITDETAILS.LIST>      </AUDITDETAILS.LIST>" + "\n" +
"          <EXCISETARIFFDETAILS.LIST>      </EXCISETARIFFDETAILS.LIST>" + "\n" +
"          <PREFIXLIST.LIST>      </PREFIXLIST.LIST>" + "\n" +
"          <SUFFIXLIST.LIST>      </SUFFIXLIST.LIST>" + "\n" +
"          <RESTARTFROMLIST.LIST>" + "\n" +
"            <DATE>20170401</DATE>" + "\n" +
"            <RESTARTFROM>Yearly</RESTARTFROM>" + "\n" +
"            <PERIODBEGINNIGNUM> 1</PERIODBEGINNIGNUM>" + "\n" +
"            <LASTNUMBERLIST.LIST>       </LASTNUMBERLIST.LIST>" + "\n" +
"          </RESTARTFROMLIST.LIST>" + "\n" +
"          <VOUCHERCLASSLIST.LIST>      </VOUCHERCLASSLIST.LIST>" + "\n" +
"          <PRODUCTCODEDETAILS.LIST>      </PRODUCTCODEDETAILS.LIST>" + "\n" +
"        </VOUCHERTYPE>" + "\n" +
"      </TALLYMESSAGE>\n";

        string voucherContra=
"     <TALLYMESSAGE xmlns:UDF=\"TallyUDF\">" + "\n" +
"       <VOUCHERTYPE NAME=\"Contra\" RESERVEDNAME=\"Contra\">" + "\n" +
"        <GUID>df13ea43-535b-411e-a3ae-09a30f52223d-00000020</GUID>" + "\n" +
"        <PARENT>Contra</PARENT>" + "\n" +
"        <MAILINGNAME>Ctra</MAILINGNAME>" + "\n" +
"        <TAXUNITNAME>&#4; Primary</TAXUNITNAME>" + "\n" +
"        <NUMBERINGMETHOD>Automatic</NUMBERINGMETHOD>" + "\n" +
"        <EXCISEUNITNAME>&#4; Primary</EXCISEUNITNAME>" + "\n" +
"        <ISUPDATINGTARGETID>No</ISUPDATINGTARGETID>" + "\n" +
"        <ASORIGINAL>Yes</ASORIGINAL>" + "\n" +
"        <ISDEEMEDPOSITIVE>No</ISDEEMEDPOSITIVE>" + "\n" +
"        <AFFECTSSTOCK>No</AFFECTSSTOCK>" + "\n" +
"        <PREVENTDUPLICATES>No</PREVENTDUPLICATES>" + "\n" +
"        <PREFILLZERO>No</PREFILLZERO>" + "\n" +
"        <PRINTAFTERSAVE>No</PRINTAFTERSAVE>" + "\n" +
"        <FORMALRECEIPT>No</FORMALRECEIPT>" + "\n" +
"        <ISOPTIONAL>No</ISOPTIONAL>" + "\n" +
"        <ASMFGJRNL>No</ASMFGJRNL>" + "\n" +
"        <EFFECTIVEDATE>No</EFFECTIVEDATE>" + "\n" +
"        <COMMONNARRATION>Yes</COMMONNARRATION>" + "\n" +
"        <MULTINARRATION>No</MULTINARRATION>" + "\n" +
"        <ISTAXINVOICE>No</ISTAXINVOICE>" + "\n" +
"        <USEFORPOSINVOICE>No</USEFORPOSINVOICE>" + "\n" +
"        <USEFOREXCISETRADERINVOICE>No</USEFOREXCISETRADERINVOICE>" + "\n" +
"        <USEFOREXCISE>No</USEFOREXCISE>" + "\n" +
"        <USEFORJOBWORK>No</USEFORJOBWORK>" + "\n" +
"        <ISFORJOBWORKIN>No</ISFORJOBWORKIN>" + "\n" +
"        <ALLOWCONSUMPTION>No</ALLOWCONSUMPTION>" + "\n" +
"        <USEFOREXCISEGOODS>No</USEFOREXCISEGOODS>" + "\n" +
"        <USEFOREXCISESUPPLEMENTARY>No</USEFOREXCISESUPPLEMENTARY>" + "\n" +
"        <ISDEFAULTALLOCENABLED>No</ISDEFAULTALLOCENABLED>" + "\n" +
"        <SORTPOSITION> 10</SORTPOSITION>" + "\n" +
"        <ALTERID> 33</ALTERID>" + "\n" +
"        <BEGINNINGNUMBER> 1</BEGINNINGNUMBER>" + "\n" +
"        <LANGUAGENAME.LIST>" + "\n" +
"         <NAME.LIST TYPE=\"String\">" + "\n" +
"          <NAME>Contra</NAME>" + "\n" +
"         </NAME.LIST>" + "\n" +
"         <LANGUAGEID> 1033</LANGUAGEID>" + "\n" +
"        </LANGUAGENAME.LIST>" + "\n" +
"        <AUDITDETAILS.LIST>      </AUDITDETAILS.LIST>" + "\n" +
"        <EXCISETARIFFDETAILS.LIST>      </EXCISETARIFFDETAILS.LIST>" + "\n" +
"        <PREFIXLIST.LIST>      </PREFIXLIST.LIST>" + "\n" +
"        <SUFFIXLIST.LIST>      </SUFFIXLIST.LIST>" + "\n" +
"        <RESTARTFROMLIST.LIST>" + "\n" +
"          <DATE>20170401</DATE>" + "\n" +
"          <RESTARTFROM>Yearly</RESTARTFROM>" + "\n" +
"          <PERIODBEGINNIGNUM> 1</PERIODBEGINNIGNUM>" + "\n" +
"          <LASTNUMBERLIST.LIST>       </LASTNUMBERLIST.LIST>" + "\n" +
"        </RESTARTFROMLIST.LIST>" + "\n" +
"        <VOUCHERCLASSLIST.LIST>      </VOUCHERCLASSLIST.LIST>" + "\n" +
"         <PRODUCTCODEDETAILS.LIST>      </PRODUCTCODEDETAILS.LIST>" + "\n" +
"        </VOUCHERTYPE>" + "\n" +
"      </TALLYMESSAGE>\n";

        string vouchercreditnode=
"      <TALLYMESSAGE xmlns:UDF=\"TallyUDF\">" + "\n" +
"        <VOUCHERTYPE NAME=\"Credit Note\" RESERVEDNAME=\"Credit Note\">" + "\n" +
"        <GUID>df13ea43-535b-411e-a3ae-09a30f52223d-00000024</GUID>" + "\n" +
"        <PARENT>Credit Note</PARENT>" + "\n" +
"        <MAILINGNAME>C/Note</MAILINGNAME>" + "\n" +
"        <TAXUNITNAME>&#4; Primary</TAXUNITNAME>" + "\n" +
"        <NUMBERINGMETHOD>Automatic</NUMBERINGMETHOD>" + "\n" +
"        <EXCISEUNITNAME>&#4; Primary</EXCISEUNITNAME>" + "\n" +
"        <CONTRACONTRA/>" + "\n" +
"        <PAYMENTCONTRA/>" + "\n" +
"        <RECEIPTCONTRA/>" + "\n" +
"        <JOURNALCONTRA/>" + "\n" +
"        <CNOTECONTRA/>" + "\n" +
"        <DNOTECONTRA/>" + "\n" +
"        <SALESCONTRA/>" + "\n" +
"        <PURCHASECONTRA/>" + "\n" +
"        <CREDITCSTCTR/>" + "\n" +
"        <DEBITCSTCTR/>" + "\n" +
"        <PREVIOUSPURCHASE/>" + "\n" +
"        <PREVIOUSSALES/>" + "\n" +
"        <PREVIOUSGODOWN/>" + "\n" +
"        <PREVNARRATION>Bieng sale made to mridul Gupta</PREVNARRATION>" + "\n" +
"        <ISUPDATINGTARGETID>No</ISUPDATINGTARGETID>" + "\n" +
"        <ASORIGINAL>Yes</ASORIGINAL>" + "\n" +
"        <ISDEEMEDPOSITIVE>No</ISDEEMEDPOSITIVE>" + "\n" +
"        <AFFECTSSTOCK>No</AFFECTSSTOCK>" + "\n" +
"        <PREVENTDUPLICATES>No</PREVENTDUPLICATES>" + "\n" +
"        <PREFILLZERO>No</PREFILLZERO>" + "\n" +
"        <PRINTAFTERSAVE>No</PRINTAFTERSAVE>" + "\n" +
"        <FORMALRECEIPT>No</FORMALRECEIPT>" + "\n" +
"        <ISOPTIONAL>No</ISOPTIONAL>" + "\n" +
"        <ASMFGJRNL>No</ASMFGJRNL>" + "\n" +
"        <EFFECTIVEDATE>No</EFFECTIVEDATE>" + "\n" +
"        <COMMONNARRATION>Yes</COMMONNARRATION>" + "\n" +
"        <MULTINARRATION>No</MULTINARRATION>" + "\n" +
"        <ISTAXINVOICE>No</ISTAXINVOICE>" + "\n" +
"        <USEFORPOSINVOICE>No</USEFORPOSINVOICE>" + "\n" +
"        <USEFOREXCISETRADERINVOICE>No</USEFOREXCISETRADERINVOICE>" + "\n" +
"        <USEFOREXCISE>No</USEFOREXCISE>" + "\n" +
"        <USEFORJOBWORK>No</USEFORJOBWORK>" + "\n" +
"        <ISFORJOBWORKIN>No</ISFORJOBWORKIN>" + "\n" +
"        <ALLOWCONSUMPTION>No</ALLOWCONSUMPTION>" + "\n" +
"        <USEFOREXCISEGOODS>No</USEFOREXCISEGOODS>" + "\n" +
"        <USEFOREXCISESUPPLEMENTARY>No</USEFOREXCISESUPPLEMENTARY>" + "\n" +
"        <ISDEFAULTALLOCENABLED>No</ISDEFAULTALLOCENABLED>" + "\n" +
"        <SORTPOSITION> 50</SORTPOSITION>" + "\n" +
"        <ALTERID> 37</ALTERID>" + "\n" +
"        <BEGINNINGNUMBER> 1</BEGINNINGNUMBER>" + "\n" +
"        <LANGUAGENAME.LIST>" + "\n" +
"          <NAME.LIST TYPE=\"String\">" + "\n" +
"           <NAME>Credit Note</NAME>" + "\n" +
"          </NAME.LIST>" + "\n" +
"          <LANGUAGEID> 1033</LANGUAGEID>" + "\n" +
"        </LANGUAGENAME.LIST>" + "\n" +
"        <AUDITDETAILS.LIST>      </AUDITDETAILS.LIST>" + "\n" +
"        <EXCISETARIFFDETAILS.LIST>      </EXCISETARIFFDETAILS.LIST>" + "\n" +
"        <PREFIXLIST.LIST>      </PREFIXLIST.LIST>" + "\n" +
"        <SUFFIXLIST.LIST>      </SUFFIXLIST.LIST>" + "\n" +
"        <RESTARTFROMLIST.LIST>" + "\n" +
"          <DATE>20170401</DATE>" + "\n" +
"          <RESTARTFROM>Yearly</RESTARTFROM>" + "\n" +
"          <PERIODBEGINNIGNUM> 1</PERIODBEGINNIGNUM>" + "\n" +
"          <LASTNUMBERLIST.LIST>       </LASTNUMBERLIST.LIST>" + "\n" +
"        </RESTARTFROMLIST.LIST>" + "\n" +
"        <VOUCHERCLASSLIST.LIST>      </VOUCHERCLASSLIST.LIST>" + "\n" +
"        <PRODUCTCODEDETAILS.LIST>      </PRODUCTCODEDETAILS.LIST>" + "\n" +
"       </VOUCHERTYPE>" + "\n" +
"      </TALLYMESSAGE>\n";

        string voucherdebitnode=
"      <TALLYMESSAGE xmlns:UDF=\"TallyUDF\">" + "\n" +
"        <VOUCHERTYPE NAME=\"Debit Note\" RESERVEDNAME=\"Debit Note\">" + "\n" +
"        <GUID>df13ea43-535b-411e-a3ae-09a30f52223d-00000025</GUID>" + "\n" +
"        <PARENT>Debit Note</PARENT>" + "\n" +
"        <MAILINGNAME>D/Note</MAILINGNAME>" + "\n" +
"        <TAXUNITNAME>&#4; Primary</TAXUNITNAME>" + "\n" +
"        <NUMBERINGMETHOD>Automatic</NUMBERINGMETHOD>" + "\n" +
"        <EXCISEUNITNAME>&#4; Primary</EXCISEUNITNAME>" + "\n" +
"        <ISUPDATINGTARGETID>No</ISUPDATINGTARGETID>" + "\n" +
"        <ASORIGINAL>Yes</ASORIGINAL>" + "\n" +
"        <ISDEEMEDPOSITIVE>Yes</ISDEEMEDPOSITIVE>" + "\n" +
"        <AFFECTSSTOCK>No</AFFECTSSTOCK>" + "\n" +
"        <PREVENTDUPLICATES>No</PREVENTDUPLICATES>" + "\n" +
"        <PREFILLZERO>No</PREFILLZERO>" + "\n" +
"        <PRINTAFTERSAVE>No</PRINTAFTERSAVE>" + "\n" +
"        <FORMALRECEIPT>No</FORMALRECEIPT>" + "\n" +
"        <ISOPTIONAL>No</ISOPTIONAL>" + "\n" +
"        <ASMFGJRNL>No</ASMFGJRNL>" + "\n" +
"        <EFFECTIVEDATE>No</EFFECTIVEDATE>" + "\n" +
"        <COMMONNARRATION>Yes</COMMONNARRATION>" + "\n" +
"        <MULTINARRATION>No</MULTINARRATION>" + "\n" +
"        <ISTAXINVOICE>No</ISTAXINVOICE>" + "\n" +
"        <USEFORPOSINVOICE>No</USEFORPOSINVOICE>" + "\n" +
"        <USEFOREXCISETRADERINVOICE>No</USEFOREXCISETRADERINVOICE>" + "\n" +
"        <USEFOREXCISE>No</USEFOREXCISE>" + "\n" +
"        <USEFORJOBWORK>No</USEFORJOBWORK>" + "\n" +
"        <ISFORJOBWORKIN>No</ISFORJOBWORKIN>" + "\n" +
"        <ALLOWCONSUMPTION>No</ALLOWCONSUMPTION>" + "\n" +
"        <USEFOREXCISEGOODS>No</USEFOREXCISEGOODS>" + "\n" +
"        <USEFOREXCISESUPPLEMENTARY>No</USEFOREXCISESUPPLEMENTARY>" + "\n" +
"        <ISDEFAULTALLOCENABLED>No</ISDEFAULTALLOCENABLED>" + "\n" +
"        <SORTPOSITION> 60</SORTPOSITION>" + "\n" +
"        <ALTERID> 38</ALTERID>" + "\n" +
"        <BEGINNINGNUMBER> 1</BEGINNINGNUMBER>" + "\n" +
"        <LANGUAGENAME.LIST>" + "\n" +
"          <NAME.LIST TYPE=\"String\">" + "\n" +
"           <NAME>Debit Note</NAME>" + "\n" +
"          </NAME.LIST>" + "\n" +
"          <LANGUAGEID> 1033</LANGUAGEID>" + "\n" +
"        </LANGUAGENAME.LIST>" + "\n" +
"        <AUDITDETAILS.LIST>      </AUDITDETAILS.LIST>" + "\n" +
"        <EXCISETARIFFDETAILS.LIST>      </EXCISETARIFFDETAILS.LIST>" + "\n" +
"        <PREFIXLIST.LIST>      </PREFIXLIST.LIST>" + "\n" +
"        <SUFFIXLIST.LIST>      </SUFFIXLIST.LIST>" + "\n" +
"        <RESTARTFROMLIST.LIST>" + "\n" +
"          <DATE>20170401</DATE>" + "\n" +
"          <RESTARTFROM>Yearly</RESTARTFROM>" + "\n" +
"          <PERIODBEGINNIGNUM> 1</PERIODBEGINNIGNUM>" + "\n" +
"          <LASTNUMBERLIST.LIST>       </LASTNUMBERLIST.LIST>" + "\n" +
"        </RESTARTFROMLIST.LIST>" + "\n" +
"        <VOUCHERCLASSLIST.LIST>      </VOUCHERCLASSLIST.LIST>" + "\n" +
"        <PRODUCTCODEDETAILS.LIST>      </PRODUCTCODEDETAILS.LIST>" + "\n" +
"      </VOUCHERTYPE>" + "\n" +
"     </TALLYMESSAGE>\n";

        string voucherdeliverynote = 
"     <TALLYMESSAGE xmlns:UDF=\"TallyUDF\">" + "\n" +
"       <VOUCHERTYPE NAME=\"Delivery Note\" RESERVEDNAME=\"Delivery Note\">" + "\n" +
"       <GUID>df13ea43-535b-411e-a3ae-09a30f52223d-00000028</GUID>" + "\n" +
"       <PARENT>Delivery Note</PARENT>" + "\n" +
"       <MAILINGNAME>Dely Note</MAILINGNAME>" + "\n" +
"       <TAXUNITNAME>&#4; Primary</TAXUNITNAME>" + "\n" +
"       <NUMBERINGMETHOD>Automatic</NUMBERINGMETHOD>" + "\n" +
"       <EXCISEUNITNAME>&#4; Primary</EXCISEUNITNAME>" + "\n" +
"       <ISUPDATINGTARGETID>No</ISUPDATINGTARGETID>" + "\n" +
"       <ASORIGINAL>Yes</ASORIGINAL>" + "\n" +
"       <ISDEEMEDPOSITIVE>No</ISDEEMEDPOSITIVE>" + "\n" +
"       <AFFECTSSTOCK>Yes</AFFECTSSTOCK>" + "\n" +
"       <PREVENTDUPLICATES>No</PREVENTDUPLICATES>" + "\n" +
"       <PREFILLZERO>No</PREFILLZERO>" + "\n" +
"       <PRINTAFTERSAVE>No</PRINTAFTERSAVE>" + "\n" +
"       <FORMALRECEIPT>No</FORMALRECEIPT>" + "\n" +
"       <ISOPTIONAL>No</ISOPTIONAL>" + "\n" +
"       <ASMFGJRNL>No</ASMFGJRNL>" + "\n" +
"       <EFFECTIVEDATE>No</EFFECTIVEDATE>" + "\n" +
"       <COMMONNARRATION>Yes</COMMONNARRATION>" + "\n" +
"       <MULTINARRATION>No</MULTINARRATION>" + "\n" +
"       <ISTAXINVOICE>No</ISTAXINVOICE>" + "\n" +
"       <USEFORPOSINVOICE>No</USEFORPOSINVOICE>" + "\n" +
"       <USEFOREXCISETRADERINVOICE>No</USEFOREXCISETRADERINVOICE>" + "\n" +
"       <USEFOREXCISE>No</USEFOREXCISE>" + "\n" +
"       <USEFORJOBWORK>No</USEFORJOBWORK>" + "\n" +
"       <ISFORJOBWORKIN>No</ISFORJOBWORKIN>" + "\n" +
"       <ALLOWCONSUMPTION>No</ALLOWCONSUMPTION>" + "\n" +
"       <USEFOREXCISEGOODS>No</USEFOREXCISEGOODS>" + "\n" +
"       <USEFOREXCISESUPPLEMENTARY>No</USEFOREXCISESUPPLEMENTARY>" + "\n" +
"       <ISDEFAULTALLOCENABLED>No</ISDEFAULTALLOCENABLED>" + "\n" +
"       <SORTPOSITION> 90</SORTPOSITION>" + "\n" +
"       <ALTERID> 41</ALTERID>" + "\n" +
"       <BEGINNINGNUMBER> 1</BEGINNINGNUMBER>" + "\n" +
"       <LANGUAGENAME.LIST>" + "\n" +
"         <NAME.LIST TYPE=\"String\">" + "\n" +
"          <NAME>Delivery Note</NAME>" + "\n" +
"         </NAME.LIST>" + "\n" +
"         <LANGUAGEID> 1033</LANGUAGEID>" + "\n" +
"       </LANGUAGENAME.LIST>" + "\n" +
"       <AUDITDETAILS.LIST>      </AUDITDETAILS.LIST>" + "\n" +
"       <EXCISETARIFFDETAILS.LIST>      </EXCISETARIFFDETAILS.LIST>" + "\n" +
"       <PREFIXLIST.LIST>      </PREFIXLIST.LIST>" + "\n" +
"       <SUFFIXLIST.LIST>      </SUFFIXLIST.LIST>" + "\n" +
"       <RESTARTFROMLIST.LIST>" + "\n" +
"        <DATE>20170401</DATE>" + "\n" +
"        <RESTARTFROM>Yearly</RESTARTFROM>" + "\n" +
"        <PERIODBEGINNIGNUM> 1</PERIODBEGINNIGNUM>" + "\n" +
"        <LASTNUMBERLIST.LIST>       </LASTNUMBERLIST.LIST>" + "\n" +
"       </RESTARTFROMLIST.LIST>" + "\n" +
"       <VOUCHERCLASSLIST.LIST>      </VOUCHERCLASSLIST.LIST>" + "\n" +
"       <PRODUCTCODEDETAILS.LIST>      </PRODUCTCODEDETAILS.LIST>" + "\n" +
"      </VOUCHERTYPE>" + "\n" +
"     </TALLYMESSAGE>\n";

        string voucherjobworkinorder=
"     <TALLYMESSAGE xmlns:UDF=\"TallyUDF\">" + "\n" +
"       <VOUCHERTYPE NAME=\"Job Work In Order\" RESERVEDNAME=\"Job Work In Order\">" + "\n" +
"       <GUID>df13ea43-535b-411e-a3ae-09a30f52223d-00000039</GUID>" + "\n" +
"       <PARENT>Job Work In Order</PARENT>" + "\n" +
"       <MAILINGNAME>Job Wrk In Ordr</MAILINGNAME>" + "\n" +
"       <TAXUNITNAME>&#4; Primary</TAXUNITNAME>" + "\n" +
"       <NUMBERINGMETHOD>Automatic</NUMBERINGMETHOD>" + "\n" +
"       <EXCISEUNITNAME>&#4; Primary</EXCISEUNITNAME>" + "\n" +
"       <ISUPDATINGTARGETID>No</ISUPDATINGTARGETID>" + "\n" +
"       <ASORIGINAL>Yes</ASORIGINAL>" + "\n" +
"       <ISDEEMEDPOSITIVE>Yes</ISDEEMEDPOSITIVE>" + "\n" +
"       <AFFECTSSTOCK>No</AFFECTSSTOCK>" + "\n" +
"       <PREVENTDUPLICATES>No</PREVENTDUPLICATES>" + "\n" +
"       <PREFILLZERO>No</PREFILLZERO>" + "\n" +
"       <PRINTAFTERSAVE>No</PRINTAFTERSAVE>" + "\n" +
"       <FORMALRECEIPT>No</FORMALRECEIPT>" + "\n" +
"       <ISOPTIONAL>No</ISOPTIONAL>" + "\n" +
"       <ASMFGJRNL>No</ASMFGJRNL>" + "\n" +
"       <EFFECTIVEDATE>No</EFFECTIVEDATE>" + "\n" +
"       <COMMONNARRATION>Yes</COMMONNARRATION>" + "\n" +
"       <MULTINARRATION>No</MULTINARRATION>" + "\n" +
"       <ISTAXINVOICE>No</ISTAXINVOICE>" + "\n" +
"       <USEFORPOSINVOICE>No</USEFORPOSINVOICE>" + "\n" +
"       <USEFOREXCISETRADERINVOICE>No</USEFOREXCISETRADERINVOICE>" + "\n" +
"       <USEFOREXCISE>No</USEFOREXCISE>" + "\n" +
"       <USEFORJOBWORK>No</USEFORJOBWORK>" + "\n" +
"       <ISFORJOBWORKIN>No</ISFORJOBWORKIN>" + "\n" +
"       <ALLOWCONSUMPTION>No</ALLOWCONSUMPTION>" + "\n" +
"       <USEFOREXCISEGOODS>No</USEFOREXCISEGOODS>" + "\n" +
"       <USEFOREXCISESUPPLEMENTARY>No</USEFOREXCISESUPPLEMENTARY>" + "\n" +
"       <ISDEFAULTALLOCENABLED>No</ISDEFAULTALLOCENABLED>" + "\n" +
"       <SORTPOSITION> 260</SORTPOSITION>" + "\n" +
"       <ALTERID> 58</ALTERID>" + "\n" +
"       <BEGINNINGNUMBER> 1</BEGINNINGNUMBER>" + "\n" +
"       <LANGUAGENAME.LIST>" + "\n" +
"         <NAME.LIST TYPE=\"String\">" + "\n" +
"          <NAME>Job Work In Order</NAME>" + "\n" +
"         </NAME.LIST>" + "\n" +
"         <LANGUAGEID> 1033</LANGUAGEID>" + "\n" +
"       </LANGUAGENAME.LIST>" + "\n" +
"       <AUDITDETAILS.LIST>      </AUDITDETAILS.LIST>" + "\n" +
"       <EXCISETARIFFDETAILS.LIST>      </EXCISETARIFFDETAILS.LIST>" + "\n" +
"       <PREFIXLIST.LIST>      </PREFIXLIST.LIST>" + "\n" +
"       <SUFFIXLIST.LIST>      </SUFFIXLIST.LIST>" + "\n" +
"       <RESTARTFROMLIST.LIST>" + "\n" +
"         <DATE>20170401</DATE>" + "\n" +
"         <RESTARTFROM>Yearly</RESTARTFROM>" + "\n" +
"         <PERIODBEGINNIGNUM> 1</PERIODBEGINNIGNUM>" + "\n" +
"         <LASTNUMBERLIST.LIST>       </LASTNUMBERLIST.LIST>" + "\n" +
"       </RESTARTFROMLIST.LIST>" + "\n" +
"       <VOUCHERCLASSLIST.LIST>      </VOUCHERCLASSLIST.LIST>" + "\n" +
"       <PRODUCTCODEDETAILS.LIST>      </PRODUCTCODEDETAILS.LIST>" + "\n" +
"      </VOUCHERTYPE>" + "\n" +
"    </TALLYMESSAGE>\n";

        string voucherjobworkoutorder =
"    <TALLYMESSAGE xmlns:UDF=\"TallyUDF\">" + "\n" +
"      <VOUCHERTYPE NAME=\"Job Work Out Order\" RESERVEDNAME=\"Job Work Out Order\">" + "\n" +
"      <GUID>df13ea43-535b-411e-a3ae-09a30f52223d-0000003a</GUID>" + "\n" +
"      <PARENT>Job Work Out Order</PARENT>" + "\n" +
"      <MAILINGNAME>Job Wrk Out Ordr</MAILINGNAME>" + "\n" +
"      <TAXUNITNAME>&#4; Primary</TAXUNITNAME>" + "\n" +
"      <NUMBERINGMETHOD>Automatic</NUMBERINGMETHOD>" + "\n" +
"      <EXCISEUNITNAME>&#4; Primary</EXCISEUNITNAME>" + "\n" +
"      <ISUPDATINGTARGETID>No</ISUPDATINGTARGETID>" + "\n" +
"      <ASORIGINAL>Yes</ASORIGINAL>" + "\n" +
"      <ISDEEMEDPOSITIVE>No</ISDEEMEDPOSITIVE>" + "\n" +
"      <AFFECTSSTOCK>No</AFFECTSSTOCK>" + "\n" +
"      <PREVENTDUPLICATES>No</PREVENTDUPLICATES>" + "\n" +
"      <PREFILLZERO>No</PREFILLZERO>" + "\n" +
"      <PRINTAFTERSAVE>No</PRINTAFTERSAVE>" + "\n" +
"      <FORMALRECEIPT>No</FORMALRECEIPT>" + "\n" +
"      <ISOPTIONAL>No</ISOPTIONAL>" + "\n" +
"      <ASMFGJRNL>No</ASMFGJRNL>" + "\n" +
"      <EFFECTIVEDATE>No</EFFECTIVEDATE>" + "\n" +
"      <COMMONNARRATION>Yes</COMMONNARRATION>" + "\n" +
"      <MULTINARRATION>No</MULTINARRATION>" + "\n" +
"      <ISTAXINVOICE>No</ISTAXINVOICE>" + "\n" +
"      <USEFORPOSINVOICE>No</USEFORPOSINVOICE>" + "\n" +
"      <USEFOREXCISETRADERINVOICE>No</USEFOREXCISETRADERINVOICE>" + "\n" +
"      <USEFOREXCISE>No</USEFOREXCISE>" + "\n" +
"      <USEFORJOBWORK>No</USEFORJOBWORK>" + "\n" +
"      <ISFORJOBWORKIN>No</ISFORJOBWORKIN>" + "\n" +
"      <ALLOWCONSUMPTION>No</ALLOWCONSUMPTION>" + "\n" +
"      <USEFOREXCISEGOODS>No</USEFOREXCISEGOODS>" + "\n" +
"      <USEFOREXCISESUPPLEMENTARY>No</USEFOREXCISESUPPLEMENTARY>" + "\n" +
"      <ISDEFAULTALLOCENABLED>No</ISDEFAULTALLOCENABLED>" + "\n" +
"      <SORTPOSITION> 270</SORTPOSITION>" + "\n" +
"      <ALTERID> 59</ALTERID>" + "\n" +
"      <BEGINNINGNUMBER> 1</BEGINNINGNUMBER>" + "\n" +
"      <LANGUAGENAME.LIST>" + "\n" +
"        <NAME.LIST TYPE=\"String\">" + "\n" +
"          <NAME>Job Work Out Order</NAME>" + "\n" +
"        </NAME.LIST>" + "\n" +
"        <LANGUAGEID> 1033</LANGUAGEID>" + "\n" +
"      </LANGUAGENAME.LIST>" + "\n" +
"      <AUDITDETAILS.LIST>      </AUDITDETAILS.LIST>" + "\n" +
"      <EXCISETARIFFDETAILS.LIST>      </EXCISETARIFFDETAILS.LIST>" + "\n" +
"      <PREFIXLIST.LIST>      </PREFIXLIST.LIST>" + "\n" +
"      <SUFFIXLIST.LIST>      </SUFFIXLIST.LIST>" + "\n" +
"      <RESTARTFROMLIST.LIST>" + "\n" +
"        <DATE>20170401</DATE>" + "\n" +
"        <RESTARTFROM>Yearly</RESTARTFROM>" + "\n" +
"        <PERIODBEGINNIGNUM> 1</PERIODBEGINNIGNUM>" + "\n" +
"        <LASTNUMBERLIST.LIST>       </LASTNUMBERLIST.LIST>" + "\n" +
"      </RESTARTFROMLIST.LIST>" + "\n" +
"      <VOUCHERCLASSLIST.LIST>      </VOUCHERCLASSLIST.LIST>" + "\n" +
"      <PRODUCTCODEDETAILS.LIST>      </PRODUCTCODEDETAILS.LIST>" + "\n" +
"     </VOUCHERTYPE>" + "\n" +
"    </TALLYMESSAGE>\n";

        string voucherJournal=
"     <TALLYMESSAGE xmlns:UDF=\"TallyUDF\">" + "\n" +
"       <VOUCHERTYPE NAME=\"Journal\" RESERVEDNAME=\"Journal\">" + "\n" +
"       <GUID>df13ea43-535b-411e-a3ae-09a30f52223d-00000023</GUID>" + "\n" +
"       <PARENT>Journal</PARENT>" + "\n" +
"       <MAILINGNAME>Jrnl</MAILINGNAME>" + "\n" +
"       <TAXUNITNAME>&#4; Primary</TAXUNITNAME>" + "\n" +
"       <NUMBERINGMETHOD>Automatic</NUMBERINGMETHOD>" + "\n" +
"       <EXCISEUNITNAME>&#4; Primary</EXCISEUNITNAME>" + "\n" +
"       <ISUPDATINGTARGETID>No</ISUPDATINGTARGETID>" + "\n" +
"       <ASORIGINAL>Yes</ASORIGINAL>" + "\n" +
"       <ISDEEMEDPOSITIVE>Yes</ISDEEMEDPOSITIVE>" + "\n" +
"       <AFFECTSSTOCK>No</AFFECTSSTOCK>" + "\n" +
"       <PREVENTDUPLICATES>No</PREVENTDUPLICATES>" + "\n" +
"       <PREFILLZERO>No</PREFILLZERO>" + "\n" +
"       <PRINTAFTERSAVE>No</PRINTAFTERSAVE>" + "\n" +
"       <FORMALRECEIPT>No</FORMALRECEIPT>" + "\n" +
"       <ISOPTIONAL>No</ISOPTIONAL>" + "\n" +
"       <ASMFGJRNL>No</ASMFGJRNL>" + "\n" +
"       <EFFECTIVEDATE>No</EFFECTIVEDATE>" + "\n" +
"       <COMMONNARRATION>Yes</COMMONNARRATION>" + "\n" +
"       <MULTINARRATION>No</MULTINARRATION>" + "\n" +
"       <ISTAXINVOICE>No</ISTAXINVOICE>" + "\n" +
"       <USEFORPOSINVOICE>No</USEFORPOSINVOICE>" + "\n" +
"       <USEFOREXCISETRADERINVOICE>No</USEFOREXCISETRADERINVOICE>" + "\n" +
"       <USEFOREXCISE>No</USEFOREXCISE>" + "\n" +
"       <USEFORJOBWORK>No</USEFORJOBWORK>" + "\n" +
"       <ISFORJOBWORKIN>No</ISFORJOBWORKIN>" + "\n" +
"       <ALLOWCONSUMPTION>No</ALLOWCONSUMPTION>" + "\n" +
"       <USEFOREXCISEGOODS>No</USEFOREXCISEGOODS>" + "\n" +
"       <USEFOREXCISESUPPLEMENTARY>No</USEFOREXCISESUPPLEMENTARY>" + "\n" +
"       <ISDEFAULTALLOCENABLED>No</ISDEFAULTALLOCENABLED>" + "\n" +
"       <SORTPOSITION> 40</SORTPOSITION>" + "\n" +
"       <ALTERID> 36</ALTERID>" + "\n" +
"       <BEGINNINGNUMBER> 1</BEGINNINGNUMBER>" + "\n" +
"       <LANGUAGENAME.LIST>" + "\n" +
"         <NAME.LIST TYPE=\"String\">" + "\n" +
"           <NAME>Journal</NAME>" + "\n" +
"         </NAME.LIST>" + "\n" +
"         <LANGUAGEID> 1033</LANGUAGEID>" + "\n" +
"       </LANGUAGENAME.LIST>" + "\n" +
"       <AUDITDETAILS.LIST>      </AUDITDETAILS.LIST>" + "\n" +
"       <EXCISETARIFFDETAILS.LIST>      </EXCISETARIFFDETAILS.LIST>" + "\n" +
"       <PREFIXLIST.LIST>      </PREFIXLIST.LIST>" + "\n" +
"       <SUFFIXLIST.LIST>      </SUFFIXLIST.LIST>" + "\n" +
"       <RESTARTFROMLIST.LIST>" + "\n" +
"         <DATE>20170401</DATE>" + "\n" +
"         <RESTARTFROM>Yearly</RESTARTFROM>" + "\n" +
"         <PERIODBEGINNIGNUM> 1</PERIODBEGINNIGNUM>" + "\n" +
"         <LASTNUMBERLIST.LIST>       </LASTNUMBERLIST.LIST>" + "\n" +
"       </RESTARTFROMLIST.LIST>" + "\n" +
"       <VOUCHERCLASSLIST.LIST>      </VOUCHERCLASSLIST.LIST>" + "\n" +
"       <PRODUCTCODEDETAILS.LIST>      </PRODUCTCODEDETAILS.LIST>" + "\n" +
"      </VOUCHERTYPE>" + "\n" +
"    </TALLYMESSAGE>\n";

        string voucherMaterialIn =
"    <TALLYMESSAGE xmlns:UDF=\"TallyUDF\">" + "\n" +
"      <VOUCHERTYPE NAME=\"Material In\" RESERVEDNAME=\"Material In\">" + "\n" +
"      <GUID>df13ea43-535b-411e-a3ae-09a30f52223d-0000003c</GUID>" + "\n" +
"      <PARENT>Material In</PARENT>" + "\n" +
"      <MAILINGNAME>Mat In</MAILINGNAME>" + "\n" +
"      <TAXUNITNAME>&#4; Primary</TAXUNITNAME>" + "\n" +
"      <NUMBERINGMETHOD>Automatic</NUMBERINGMETHOD>" + "\n" +
"      <EXCISEUNITNAME>&#4; Primary</EXCISEUNITNAME>" + "\n" +
"      <ISUPDATINGTARGETID>No</ISUPDATINGTARGETID>" + "\n" +
"      <ASORIGINAL>Yes</ASORIGINAL>" + "\n" +
"      <ISDEEMEDPOSITIVE>Yes</ISDEEMEDPOSITIVE>" + "\n" +
"      <AFFECTSSTOCK>Yes</AFFECTSSTOCK>" + "\n" +
"      <PREVENTDUPLICATES>No</PREVENTDUPLICATES>" + "\n" +
"      <PREFILLZERO>No</PREFILLZERO>" + "\n" +
"      <PRINTAFTERSAVE>No</PRINTAFTERSAVE>" + "\n" +
"      <FORMALRECEIPT>No</FORMALRECEIPT>" + "\n" +
"      <ISOPTIONAL>No</ISOPTIONAL>" + "\n" +
"      <ASMFGJRNL>No</ASMFGJRNL>" + "\n" +
"      <EFFECTIVEDATE>No</EFFECTIVEDATE>" + "\n" +
"      <COMMONNARRATION>Yes</COMMONNARRATION>" + "\n" +
"      <MULTINARRATION>No</MULTINARRATION>" + "\n" +
"      <ISTAXINVOICE>No</ISTAXINVOICE>" + "\n" +
"      <USEFORPOSINVOICE>No</USEFORPOSINVOICE>" + "\n" +
"      <USEFOREXCISETRADERINVOICE>No</USEFOREXCISETRADERINVOICE>" + "\n" +
"      <USEFOREXCISE>No</USEFOREXCISE>" + "\n" +
"      <USEFORJOBWORK>No</USEFORJOBWORK>" + "\n" +
"      <ISFORJOBWORKIN>No</ISFORJOBWORKIN>" + "\n" +
"      <ALLOWCONSUMPTION>No</ALLOWCONSUMPTION>" + "\n" +
"      <USEFOREXCISEGOODS>No</USEFOREXCISEGOODS>" + "\n" +
"      <USEFOREXCISESUPPLEMENTARY>No</USEFOREXCISESUPPLEMENTARY>" + "\n" +
"      <ISDEFAULTALLOCENABLED>No</ISDEFAULTALLOCENABLED>" + "\n" +
"      <SORTPOSITION> 290</SORTPOSITION>" + "\n" +
"      <ALTERID> 61</ALTERID>" + "\n" +
"      <BEGINNINGNUMBER> 1</BEGINNINGNUMBER>" + "\n" +
"      <LANGUAGENAME.LIST>" + "\n" +
"        <NAME.LIST TYPE=\"String\">" + "\n" +
"          <NAME>Material In</NAME>" + "\n" +
"        </NAME.LIST>" + "\n" +
"       <LANGUAGEID> 1033</LANGUAGEID>" + "\n" +
"      </LANGUAGENAME.LIST>" + "\n" +
"      <AUDITDETAILS.LIST>      </AUDITDETAILS.LIST>" + "\n" +
"      <EXCISETARIFFDETAILS.LIST>      </EXCISETARIFFDETAILS.LIST>" + "\n" +
"      <PREFIXLIST.LIST>      </PREFIXLIST.LIST>" + "\n" +
"      <SUFFIXLIST.LIST>      </SUFFIXLIST.LIST>" + "\n" +
"      <RESTARTFROMLIST.LIST>" + "\n" +
"        <DATE>20170401</DATE>" + "\n" +
"        <RESTARTFROM>Yearly</RESTARTFROM>" + "\n" +
"        <PERIODBEGINNIGNUM> 1</PERIODBEGINNIGNUM>" + "\n" +
"        <LASTNUMBERLIST.LIST>       </LASTNUMBERLIST.LIST>" + "\n" +
"      </RESTARTFROMLIST.LIST>" + "\n" +
"      <VOUCHERCLASSLIST.LIST>      </VOUCHERCLASSLIST.LIST>" + "\n" +
"      <PRODUCTCODEDETAILS.LIST>      </PRODUCTCODEDETAILS.LIST>" + "\n" +
"    </VOUCHERTYPE>" + "\n" +
"   </TALLYMESSAGE>\n";


        string vouchermaterialOut= 
"    <TALLYMESSAGE xmlns:UDF=\"TallyUDF\">" + "\n" +
"       <VOUCHERTYPE NAME=\"Material Out\" RESERVEDNAME=\"Material Out\">" + "\n" +
"       <GUID>df13ea43-535b-411e-a3ae-09a30f52223d-0000003b</GUID>" + "\n" +
"       <PARENT>Material Out</PARENT>" + "\n" +
"       <MAILINGNAME>Mat Out</MAILINGNAME>" + "\n" +
"       <TAXUNITNAME>&#4; Primary</TAXUNITNAME>" + "\n" +
"       <NUMBERINGMETHOD>Automatic</NUMBERINGMETHOD>" + "\n" +
"       <EXCISEUNITNAME>&#4; Primary</EXCISEUNITNAME>" + "\n" +
"       <ISUPDATINGTARGETID>No</ISUPDATINGTARGETID>" + "\n" +
"       <ASORIGINAL>Yes</ASORIGINAL>" + "\n" +
"       <ISDEEMEDPOSITIVE>Yes</ISDEEMEDPOSITIVE>" + "\n" +
"       <AFFECTSSTOCK>Yes</AFFECTSSTOCK>" + "\n" +
"       <PREVENTDUPLICATES>No</PREVENTDUPLICATES>" + "\n" +
"       <PREFILLZERO>No</PREFILLZERO>" + "\n" +
"       <PRINTAFTERSAVE>No</PRINTAFTERSAVE>" + "\n" +
"       <FORMALRECEIPT>No</FORMALRECEIPT>" + "\n" +
"       <ISOPTIONAL>No</ISOPTIONAL>" + "\n" +
"       <ASMFGJRNL>No</ASMFGJRNL>" + "\n" +
"       <EFFECTIVEDATE>No</EFFECTIVEDATE>" + "\n" +
"       <COMMONNARRATION>Yes</COMMONNARRATION>" + "\n" +
"       <MULTINARRATION>No</MULTINARRATION>" + "\n" +
"       <ISTAXINVOICE>No</ISTAXINVOICE>" + "\n" +
"       <USEFORPOSINVOICE>No</USEFORPOSINVOICE>" + "\n" +
"       <USEFOREXCISETRADERINVOICE>No</USEFOREXCISETRADERINVOICE>" + "\n" +
"       <USEFOREXCISE>No</USEFOREXCISE>" + "\n" +
"       <USEFORJOBWORK>No</USEFORJOBWORK>" + "\n" +
"       <ISFORJOBWORKIN>No</ISFORJOBWORKIN>" + "\n" +
"       <ALLOWCONSUMPTION>No</ALLOWCONSUMPTION>" + "\n" +
"       <USEFOREXCISEGOODS>No</USEFOREXCISEGOODS>" + "\n" +
"       <USEFOREXCISESUPPLEMENTARY>No</USEFOREXCISESUPPLEMENTARY>" + "\n" +
"       <ISDEFAULTALLOCENABLED>No</ISDEFAULTALLOCENABLED>" + "\n" +
"       <SORTPOSITION> 280</SORTPOSITION>" + "\n" +
"       <ALTERID> 60</ALTERID>" + "\n" +
"       <BEGINNINGNUMBER> 1</BEGINNINGNUMBER>" + "\n" +
"       <LANGUAGENAME.LIST>" + "\n" +
"         <NAME.LIST TYPE=\"String\">" + "\n" +
"          <NAME>Material Out</NAME>" + "\n" +
"         </NAME.LIST>" + "\n" +
"         <LANGUAGEID> 1033</LANGUAGEID>" + "\n" +
"       </LANGUAGENAME.LIST>" + "\n" +
"       <AUDITDETAILS.LIST>      </AUDITDETAILS.LIST>" + "\n" +
"       <EXCISETARIFFDETAILS.LIST>      </EXCISETARIFFDETAILS.LIST>" + "\n" +
"       <PREFIXLIST.LIST>      </PREFIXLIST.LIST>" + "\n" +
"       <SUFFIXLIST.LIST>      </SUFFIXLIST.LIST>" + "\n" +
"       <RESTARTFROMLIST.LIST>" + "\n" +
"         <DATE>20170401</DATE>" + "\n" +
"         <RESTARTFROM>Yearly</RESTARTFROM>" + "\n" +
"         <PERIODBEGINNIGNUM> 1</PERIODBEGINNIGNUM>" + "\n" +
"         <LASTNUMBERLIST.LIST>       </LASTNUMBERLIST.LIST>" + "\n" +
"       </RESTARTFROMLIST.LIST>" + "\n" +
"       <VOUCHERCLASSLIST.LIST>      </VOUCHERCLASSLIST.LIST>" + "\n" +
"       <PRODUCTCODEDETAILS.LIST>      </PRODUCTCODEDETAILS.LIST>" + "\n" +
"     </VOUCHERTYPE>" + "\n" +
"    </TALLYMESSAGE>\n";

        string vouchermemorandum= 
            "<TALLYMESSAGE xmlns:UDF=\"TallyUDF\">" + "\n" +
     "<VOUCHERTYPE NAME=\"Memorandum\" RESERVEDNAME=\"Memorandum\">" + "\n" +
      "<GUID>df13ea43-535b-411e-a3ae-09a30f52223d-0000002e</GUID>" + "\n" +
      "<PARENT>Memorandum</PARENT>" + "\n" +
      "<MAILINGNAME>Memo</MAILINGNAME>" + "\n" +
      "<TAXUNITNAME>&#4; Primary</TAXUNITNAME>" + "\n" +
      "<NUMBERINGMETHOD>Automatic</NUMBERINGMETHOD>" + "\n" +
      "<EXCISEUNITNAME>&#4; Primary</EXCISEUNITNAME>" + "\n" +
      "<ISUPDATINGTARGETID>No</ISUPDATINGTARGETID>" + "\n" +
      "<ASORIGINAL>Yes</ASORIGINAL>" + "\n" +
      "<ISDEEMEDPOSITIVE>Yes</ISDEEMEDPOSITIVE>" + "\n" +
      "<AFFECTSSTOCK>No</AFFECTSSTOCK>" + "\n" +
      "<PREVENTDUPLICATES>No</PREVENTDUPLICATES>" + "\n" +
      "<PREFILLZERO>No</PREFILLZERO>" + "\n" +
      "<PRINTAFTERSAVE>No</PRINTAFTERSAVE>" + "\n" +
      "<FORMALRECEIPT>No</FORMALRECEIPT>" + "\n" +
      "<ISOPTIONAL>No</ISOPTIONAL>" + "\n" +
      "<ASMFGJRNL>No</ASMFGJRNL>" + "\n" +
      "<EFFECTIVEDATE>No</EFFECTIVEDATE>" + "\n" +
      "<COMMONNARRATION>Yes</COMMONNARRATION>" + "\n" +
      "<MULTINARRATION>No</MULTINARRATION>" + "\n" +
      "<ISTAXINVOICE>No</ISTAXINVOICE>" + "\n" +
      "<USEFORPOSINVOICE>No</USEFORPOSINVOICE>" + "\n" +
      "<USEFOREXCISETRADERINVOICE>No</USEFOREXCISETRADERINVOICE>" + "\n" +
      "<USEFOREXCISE>No</USEFOREXCISE>" + "\n" +
      "<USEFORJOBWORK>No</USEFORJOBWORK>" + "\n" +
      "<ISFORJOBWORKIN>No</ISFORJOBWORKIN>" + "\n" +
      "<ALLOWCONSUMPTION>No</ALLOWCONSUMPTION>" + "\n" +
      "<USEFOREXCISEGOODS>No</USEFOREXCISEGOODS>" + "\n" +
      "<USEFOREXCISESUPPLEMENTARY>No</USEFOREXCISESUPPLEMENTARY>" + "\n" +
      "<ISDEFAULTALLOCENABLED>No</ISDEFAULTALLOCENABLED>" + "\n" +
      "<SORTPOSITION> 150</SORTPOSITION>" + "\n" +
      "<ALTERID> 47</ALTERID>" + "\n" +
      "<BEGINNINGNUMBER> 1</BEGINNINGNUMBER>" + "\n" +
      "<LANGUAGENAME.LIST>" + "\n" +
       "<NAME.LIST TYPE=\"String\">" + "\n" +
        "<NAME>Memorandum</NAME>" + "\n" +
       "</NAME.LIST>" + "\n" +
       "<LANGUAGEID> 1033</LANGUAGEID>" + "\n" +
      "</LANGUAGENAME.LIST>" + "\n" +
      "<AUDITDETAILS.LIST>      </AUDITDETAILS.LIST>" + "\n" +
      "<EXCISETARIFFDETAILS.LIST>      </EXCISETARIFFDETAILS.LIST>" + "\n" +
      "<PREFIXLIST.LIST>      </PREFIXLIST.LIST>" + "\n" +
      "<SUFFIXLIST.LIST>      </SUFFIXLIST.LIST>" + "\n" +
      "<RESTARTFROMLIST.LIST>" + "\n" +
       "<DATE>20170401</DATE>" + "\n" +
       "<RESTARTFROM>Yearly</RESTARTFROM>" + "\n" +
       "<PERIODBEGINNIGNUM> 1</PERIODBEGINNIGNUM>" + "\n" +
       "<LASTNUMBERLIST.LIST>       </LASTNUMBERLIST.LIST>" + "\n" +
      "</RESTARTFROMLIST.LIST>" + "\n" +
      "<VOUCHERCLASSLIST.LIST>      </VOUCHERCLASSLIST.LIST>" + "\n" +
      "<PRODUCTCODEDETAILS.LIST>      </PRODUCTCODEDETAILS.LIST>" + "\n" +
     "</VOUCHERTYPE>" + "\n" +
    "</TALLYMESSAGE>\n";

        string voucherpayment=
            "<TALLYMESSAGE xmlns:UDF=\"TallyUDF\">" + "\n" +
     "<VOUCHERTYPE NAME=\"Payment\" RESERVEDNAME=\"Payment\">" + "\n" +
      "<GUID>df13ea43-535b-411e-a3ae-09a30f52223d-00000021</GUID>" + "\n" +
      "<PARENT>Payment</PARENT>" + "\n" +
      "<MAILINGNAME>Pymt</MAILINGNAME>" + "\n" +
      "<TAXUNITNAME>&#4; Primary</TAXUNITNAME>" + "\n" +
      "<NUMBERINGMETHOD>Automatic</NUMBERINGMETHOD>" + "\n" +
      "<EXCISEUNITNAME>&#4; Primary</EXCISEUNITNAME>" + "\n" +
      "<ISUPDATINGTARGETID>No</ISUPDATINGTARGETID>" + "\n" +
      "<ASORIGINAL>Yes</ASORIGINAL>" + "\n" +
      "<ISDEEMEDPOSITIVE>Yes</ISDEEMEDPOSITIVE>" + "\n" +
      "<AFFECTSSTOCK>No</AFFECTSSTOCK>" + "\n" +
     " <PREVENTDUPLICATES>No</PREVENTDUPLICATES>" + "\n" +
      "<PREFILLZERO>No</PREFILLZERO>" + "\n" +
      "<PRINTAFTERSAVE>No</PRINTAFTERSAVE>" + "\n" +
      "<FORMALRECEIPT>No</FORMALRECEIPT>" + "\n" +
      "<ISOPTIONAL>No</ISOPTIONAL>" + "\n" +
      "<ASMFGJRNL>No</ASMFGJRNL>" + "\n" +
      "<EFFECTIVEDATE>No</EFFECTIVEDATE>" + "\n" +
      "<COMMONNARRATION>Yes</COMMONNARRATION>" + "\n" +
      "<MULTINARRATION>No</MULTINARRATION>" + "\n" +
      "<ISTAXINVOICE>No</ISTAXINVOICE>" + "\n" +
      "<USEFORPOSINVOICE>No</USEFORPOSINVOICE>" + "\n" +
      "<USEFOREXCISETRADERINVOICE>No</USEFOREXCISETRADERINVOICE>" + "\n" +
      "<USEFOREXCISE>No</USEFOREXCISE>" + "\n" +
      "<USEFORJOBWORK>No</USEFORJOBWORK>" + "\n" +
      "<ISFORJOBWORKIN>No</ISFORJOBWORKIN>" + "\n" +
      "<ALLOWCONSUMPTION>No</ALLOWCONSUMPTION>" + "\n" +
      "<USEFOREXCISEGOODS>No</USEFOREXCISEGOODS>" + "\n" +
      "<USEFOREXCISESUPPLEMENTARY>No</USEFOREXCISESUPPLEMENTARY>" + "\n" +
      "<ISDEFAULTALLOCENABLED>No</ISDEFAULTALLOCENABLED>" + "\n" +
      "<SORTPOSITION> 20</SORTPOSITION>" + "\n" +
      "<ALTERID> 34</ALTERID>" + "\n" +
      "<BEGINNINGNUMBER> 1</BEGINNINGNUMBER>" + "\n" +
      "<LANGUAGENAME.LIST>" + "\n" +
       "<NAME.LIST TYPE=\"String\">" + "\n" +
        "<NAME>Payment</NAME>" + "\n" +
       "</NAME.LIST>" + "\n" +
       "<LANGUAGEID> 1033</LANGUAGEID>" + "\n" +
      "</LANGUAGENAME.LIST>" + "\n" +
      "<AUDITDETAILS.LIST>      </AUDITDETAILS.LIST>" + "\n" +
      "<EXCISETARIFFDETAILS.LIST>      </EXCISETARIFFDETAILS.LIST>" + "\n" +
      "<PREFIXLIST.LIST>      </PREFIXLIST.LIST>" + "\n" +
      "<SUFFIXLIST.LIST>      </SUFFIXLIST.LIST>" + "\n" +
      "<RESTARTFROMLIST.LIST>" + "\n" +
      " <DATE>20170401</DATE>" + "\n" +
      " <RESTARTFROM>Yearly</RESTARTFROM>" + "\n" +
      " <PERIODBEGINNIGNUM> 1</PERIODBEGINNIGNUM>" + "\n" +
      " <LASTNUMBERLIST.LIST>       </LASTNUMBERLIST.LIST>" + "\n" +
      "</RESTARTFROMLIST.LIST>" + "\n" +
     " <VOUCHERCLASSLIST.LIST>      </VOUCHERCLASSLIST.LIST>" + "\n" +
     " <PRODUCTCODEDETAILS.LIST>      </PRODUCTCODEDETAILS.LIST>" + "\n" +
    " </VOUCHERTYPE>" + "\n" +
    "</TALLYMESSAGE>\n";

        string voucherPayroll=
            "<TALLYMESSAGE xmlns:UDF=\"TallyUDF\">" + "\n" +
    " <VOUCHERTYPE NAME=\"Payroll\" RESERVEDNAME=\"Payroll\">" + "\n" +
      "<GUID>df13ea43-535b-411e-a3ae-09a30f52223d-00000038</GUID>" + "\n" +
      "<PARENT>Payroll</PARENT>" + "\n" +
      "<MAILINGNAME>Pyrl</MAILINGNAME>" + "\n" +
      "<TAXUNITNAME>&#4; Primary</TAXUNITNAME>" + "\n" +
      "<NUMBERINGMETHOD>Automatic</NUMBERINGMETHOD>" + "\n" +
      "<EXCISEUNITNAME>&#4; Primary</EXCISEUNITNAME>" + "\n" +
      "<ISUPDATINGTARGETID>No</ISUPDATINGTARGETID>" + "\n" +
      "<ASORIGINAL>Yes</ASORIGINAL>" + "\n" +
      "<ISDEEMEDPOSITIVE>Yes</ISDEEMEDPOSITIVE>" + "\n" +
      "<AFFECTSSTOCK>No</AFFECTSSTOCK>" + "\n" +
      "<PREVENTDUPLICATES>No</PREVENTDUPLICATES>" + "\n" +
      "<PREFILLZERO>No</PREFILLZERO>" + "\n" +
      "<PRINTAFTERSAVE>No</PRINTAFTERSAVE>" + "\n" +
      "<FORMALRECEIPT>No</FORMALRECEIPT>" + "\n" +
      "<ISOPTIONAL>No</ISOPTIONAL>" + "\n" +
      "<ASMFGJRNL>No</ASMFGJRNL>" + "\n" +
      "<EFFECTIVEDATE>No</EFFECTIVEDATE>" + "\n" +
      "<COMMONNARRATION>Yes</COMMONNARRATION>" + "\n" +
      "<MULTINARRATION>No</MULTINARRATION>" + "\n" +
      "<ISTAXINVOICE>No</ISTAXINVOICE>" + "\n" +
      "<USEFORPOSINVOICE>No</USEFORPOSINVOICE>" + "\n" +
      "<USEFOREXCISETRADERINVOICE>No</USEFOREXCISETRADERINVOICE>" + "\n" +
      "<USEFOREXCISE>No</USEFOREXCISE>" + "\n" +
      "<USEFORJOBWORK>No</USEFORJOBWORK>" + "\n" +
      "<ISFORJOBWORKIN>No</ISFORJOBWORKIN>" + "\n" +
      "<ALLOWCONSUMPTION>No</ALLOWCONSUMPTION>" + "\n" +
      "<USEFOREXCISEGOODS>No</USEFOREXCISEGOODS>" + "\n" +
      "<USEFOREXCISESUPPLEMENTARY>No</USEFOREXCISESUPPLEMENTARY>" + "\n" +
      "<ISDEFAULTALLOCENABLED>No</ISDEFAULTALLOCENABLED>" + "\n" +
      "<SORTPOSITION> 250</SORTPOSITION>" + "\n" +
      "<ALTERID> 57</ALTERID>" + "\n" +
      "<BEGINNINGNUMBER> 1</BEGINNINGNUMBER>" + "\n" +
      "<LANGUAGENAME.LIST>" + "\n" +
       "<NAME.LIST TYPE=\"String\">" + "\n" +
        "<NAME>Payroll</NAME>" + "\n" +
       "</NAME.LIST>" + "\n" +
       "<LANGUAGEID> 1033</LANGUAGEID>" + "\n" +
      "</LANGUAGENAME.LIST>" + "\n" +
      "<AUDITDETAILS.LIST>      </AUDITDETAILS.LIST>" + "\n" +
      "<EXCISETARIFFDETAILS.LIST>      </EXCISETARIFFDETAILS.LIST>" + "\n" +
      "<PREFIXLIST.LIST>      </PREFIXLIST.LIST>" + "\n" +
      "<SUFFIXLIST.LIST>      </SUFFIXLIST.LIST>" + "\n" +
      "<RESTARTFROMLIST.LIST>" + "\n" +
       "<DATE>20170401</DATE>" + "\n" +
       "<RESTARTFROM>Yearly</RESTARTFROM>" + "\n" +
       "<PERIODBEGINNIGNUM> 1</PERIODBEGINNIGNUM>" + "\n" +
       "<LASTNUMBERLIST.LIST>       </LASTNUMBERLIST.LIST>" + "\n" +
      "</RESTARTFROMLIST.LIST>" + "\n" +
      "<VOUCHERCLASSLIST.LIST>      </VOUCHERCLASSLIST.LIST>" + "\n" +
      "<PRODUCTCODEDETAILS.LIST>      </PRODUCTCODEDETAILS.LIST>" + "\n" +
     "</VOUCHERTYPE>" + "\n" +
    "</TALLYMESSAGE>\n";


        string voucherPhysicalstock=
"        <TALLYMESSAGE xmlns:UDF=\"TallyUDF\">" + "\n" +
"     <VOUCHERTYPE NAME=\"Physical Stock\" RESERVEDNAME=\"Physical Stock\">" + "\n" +
      "<GUID>df13ea43-535b-411e-a3ae-09a30f52223d-0000002c</GUID>" + "\n" +
      "<PARENT>Physical Stock</PARENT>" + "\n" +
      "<MAILINGNAME>Phy Stk</MAILINGNAME>" + "\n" +
      "<TAXUNITNAME>&#4; Primary</TAXUNITNAME>" + "\n" +
      "<NUMBERINGMETHOD>Automatic</NUMBERINGMETHOD>" + "\n" +
      "<EXCISEUNITNAME>&#4; Primary</EXCISEUNITNAME>" + "\n" +
      "<ISUPDATINGTARGETID>No</ISUPDATINGTARGETID>" + "\n" +
      "<ASORIGINAL>Yes</ASORIGINAL>" + "\n" +
      "<ISDEEMEDPOSITIVE>Yes</ISDEEMEDPOSITIVE>" + "\n" +
      "<AFFECTSSTOCK>Yes</AFFECTSSTOCK>" + "\n" +
      "<PREVENTDUPLICATES>No</PREVENTDUPLICATES>" + "\n" +
      "<PREFILLZERO>No</PREFILLZERO>" + "\n" +
      "<PRINTAFTERSAVE>No</PRINTAFTERSAVE>" + "\n" +
      "<FORMALRECEIPT>No</FORMALRECEIPT>" + "\n" +
      "<ISOPTIONAL>No</ISOPTIONAL>" + "\n" +
      "<ASMFGJRNL>No</ASMFGJRNL>" + "\n" +
      "<EFFECTIVEDATE>No</EFFECTIVEDATE>" + "\n" +
      "<COMMONNARRATION>Yes</COMMONNARRATION>" + "\n" +
      "<MULTINARRATION>No</MULTINARRATION>" + "\n" +
      "<ISTAXINVOICE>No</ISTAXINVOICE>" + "\n" +
      "<USEFORPOSINVOICE>No</USEFORPOSINVOICE>" + "\n" +
      "<USEFOREXCISETRADERINVOICE>No</USEFOREXCISETRADERINVOICE>" + "\n" +
      "<USEFOREXCISE>No</USEFOREXCISE>" + "\n" +
      "<USEFORJOBWORK>No</USEFORJOBWORK>" + "\n" +
      "<ISFORJOBWORKIN>No</ISFORJOBWORKIN>" + "\n" +
      "<ALLOWCONSUMPTION>No</ALLOWCONSUMPTION>" + "\n" +
      "<USEFOREXCISEGOODS>No</USEFOREXCISEGOODS>" + "\n" +
      "<USEFOREXCISESUPPLEMENTARY>No</USEFOREXCISESUPPLEMENTARY>" + "\n" +
      "<ISDEFAULTALLOCENABLED>No</ISDEFAULTALLOCENABLED>" + "\n" +
      "<SORTPOSITION> 130</SORTPOSITION>" + "\n" +
      "<ALTERID> 45</ALTERID>" + "\n" +
      "<BEGINNINGNUMBER> 1</BEGINNINGNUMBER>" + "\n" +
      "<LANGUAGENAME.LIST>" + "\n" +
       "<NAME.LIST TYPE=\"String\">" + "\n" +
        "<NAME>Physical Stock</NAME>" + "\n" +
       "</NAME.LIST>" + "\n" +
       "<LANGUAGEID> 1033</LANGUAGEID>" + "\n" +
      "</LANGUAGENAME.LIST>" + "\n" +
      "<AUDITDETAILS.LIST>      </AUDITDETAILS.LIST>" + "\n" +
     " <EXCISETARIFFDETAILS.LIST>      </EXCISETARIFFDETAILS.LIST>" + "\n" +
     " <PREFIXLIST.LIST>      </PREFIXLIST.LIST>" + "\n" +
      "<SUFFIXLIST.LIST>      </SUFFIXLIST.LIST>" + "\n" +
      "<RESTARTFROMLIST.LIST>" + "\n" +
       "<DATE>20170401</DATE>" + "\n" +
       "<RESTARTFROM>Yearly</RESTARTFROM>" + "\n" +
       "<PERIODBEGINNIGNUM> 1</PERIODBEGINNIGNUM>" + "\n" +
       "<LASTNUMBERLIST.LIST>       </LASTNUMBERLIST.LIST>" + "\n" +
      "</RESTARTFROMLIST.LIST>" + "\n" +
      "<VOUCHERCLASSLIST.LIST>      </VOUCHERCLASSLIST.LIST>" + "\n" +
      "<PRODUCTCODEDETAILS.LIST>      </PRODUCTCODEDETAILS.LIST>" + "\n" +
     "</VOUCHERTYPE>" + "\n" +
    "</TALLYMESSAGE>\n";

            string voucherPurchase =
"        <TALLYMESSAGE xmlns:UDF=\"TallyUDF\">" + "\n" +
"          <VOUCHERTYPE NAME=\"Purchase\" RESERVEDNAME=\"Purchase\">" + "\n" +
"          <GUID>df13ea43-535b-411e-a3ae-09a30f52223d-00000027</GUID>" + "\n" +
"          <PARENT>Purchase</PARENT>" + "\n" +
"          <MAILINGNAME>Purc</MAILINGNAME>" + "\n" +
"          <TAXUNITNAME>&#4; Primary</TAXUNITNAME>" + "\n" +
"          <NUMBERINGMETHOD>Automatic</NUMBERINGMETHOD>" + "\n" +
"          <EXCISEUNITNAME>&#4; Primary</EXCISEUNITNAME>" + "\n" +
"          <ISUPDATINGTARGETID>No</ISUPDATINGTARGETID>" + "\n" +
"          <ASORIGINAL>Yes</ASORIGINAL>" + "\n" +
"          <ISDEEMEDPOSITIVE>No</ISDEEMEDPOSITIVE>" + "\n" +
"          <AFFECTSSTOCK>No</AFFECTSSTOCK>" + "\n" +
"          <PREVENTDUPLICATES>No</PREVENTDUPLICATES>" + "\n" +
"          <PREFILLZERO>No</PREFILLZERO>" + "\n" +
"          <PRINTAFTERSAVE>No</PRINTAFTERSAVE>" + "\n" +
"          <FORMALRECEIPT>No</FORMALRECEIPT>" + "\n" +
"          <ISOPTIONAL>No</ISOPTIONAL>" + "\n" +
"          <ASMFGJRNL>No</ASMFGJRNL>" + "\n" +
"          <EFFECTIVEDATE>No</EFFECTIVEDATE>" + "\n" +
"          <COMMONNARRATION>Yes</COMMONNARRATION>" + "\n" +
"          <MULTINARRATION>No</MULTINARRATION>" + "\n" +
"          <ISTAXINVOICE>No</ISTAXINVOICE>" + "\n" +
"          <USEFORPOSINVOICE>No</USEFORPOSINVOICE>" + "\n" +
"          <USEFOREXCISETRADERINVOICE>No</USEFOREXCISETRADERINVOICE>" + "\n" +
"          <USEFOREXCISE>No</USEFOREXCISE>" + "\n" +
"          <USEFORJOBWORK>No</USEFORJOBWORK>" + "\n" +
"          <ISFORJOBWORKIN>No</ISFORJOBWORKIN>" + "\n" +
"          <ALLOWCONSUMPTION>No</ALLOWCONSUMPTION>" + "\n" +
"          <USEFOREXCISEGOODS>No</USEFOREXCISEGOODS>" + "\n" +
"          <USEFOREXCISESUPPLEMENTARY>No</USEFOREXCISESUPPLEMENTARY>" + "\n" +
"          <ISDEFAULTALLOCENABLED>No</ISDEFAULTALLOCENABLED>" + "\n" +
"          <SORTPOSITION> 80</SORTPOSITION>" + "\n" +
"          <ALTERID> 40</ALTERID>" + "\n" +
"          <BEGINNINGNUMBER> 1</BEGINNINGNUMBER>" + "\n" +
"          <LANGUAGENAME.LIST>" + "\n" +
"           <NAME.LIST TYPE=\"String\">" + "\n" +
"            <NAME>Purchase</NAME>" + "\n" +
"           </NAME.LIST>" + "\n" +
"           <LANGUAGEID> 1033</LANGUAGEID>" + "\n" +
"          </LANGUAGENAME.LIST>" + "\n" +
"          <AUDITDETAILS.LIST>      </AUDITDETAILS.LIST>" + "\n" +
"          <EXCISETARIFFDETAILS.LIST>      </EXCISETARIFFDETAILS.LIST>" + "\n" +
"          <PREFIXLIST.LIST>      </PREFIXLIST.LIST>" + "\n" +
"          <SUFFIXLIST.LIST>      </SUFFIXLIST.LIST>" + "\n" +
"          <RESTARTFROMLIST.LIST>" + "\n" +
"           <DATE>20170401</DATE>" + "\n" +
"           <RESTARTFROM>Yearly</RESTARTFROM>" + "\n" +
"           <PERIODBEGINNIGNUM> 1</PERIODBEGINNIGNUM>" + "\n" +
"           <LASTNUMBERLIST.LIST>       </LASTNUMBERLIST.LIST>" + "\n" +
"          </RESTARTFROMLIST.LIST>" + "\n" +
"          <VOUCHERCLASSLIST.LIST>      </VOUCHERCLASSLIST.LIST>" + "\n" +
"          <PRODUCTCODEDETAILS.LIST>      </PRODUCTCODEDETAILS.LIST>" + "\n" +
"         </VOUCHERTYPE>" + "\n" +
"       </TALLYMESSAGE>\n";

            string voucherPurchaseOrder =
"       <TALLYMESSAGE xmlns:UDF=\"TallyUDF\">" + "\n" +
"         <VOUCHERTYPE NAME=\"Purchase Order\" RESERVEDNAME=\"Purchase Order\">" + "\n" +
"          <GUID>df13ea43-535b-411e-a3ae-09a30f52223d-00000031</GUID>" + "\n" +
"          <PARENT>Purchase Order</PARENT>" + "\n" +
"          <MAILINGNAME>Purc Order</MAILINGNAME>" + "\n" +
"          <TAXUNITNAME>&#4; Primary</TAXUNITNAME>" + "\n" +
"          <NUMBERINGMETHOD>Automatic</NUMBERINGMETHOD>" + "\n" +
"          <EXCISEUNITNAME>&#4; Primary</EXCISEUNITNAME>" + "\n" +
"          <ISUPDATINGTARGETID>No</ISUPDATINGTARGETID>" + "\n" +
"          <ASORIGINAL>Yes</ASORIGINAL>" + "\n" +
"          <ISDEEMEDPOSITIVE>No</ISDEEMEDPOSITIVE>" + "\n" +
"          <AFFECTSSTOCK>No</AFFECTSSTOCK>" + "\n" +
"          <PREVENTDUPLICATES>No</PREVENTDUPLICATES>" + "\n" +
"          <PREFILLZERO>No</PREFILLZERO>" + "\n" +
"          <PRINTAFTERSAVE>No</PRINTAFTERSAVE>" + "\n" +
"          <FORMALRECEIPT>No</FORMALRECEIPT>" + "\n" +
"          <ISOPTIONAL>No</ISOPTIONAL>" + "\n" +
"          <ASMFGJRNL>No</ASMFGJRNL>" + "\n" +
"          <EFFECTIVEDATE>No</EFFECTIVEDATE>" + "\n" +
"          <COMMONNARRATION>Yes</COMMONNARRATION>" + "\n" +
"          <MULTINARRATION>No</MULTINARRATION>" + "\n" +
"          <ISTAXINVOICE>No</ISTAXINVOICE>" + "\n" +
"          <USEFORPOSINVOICE>No</USEFORPOSINVOICE>" + "\n" +
"          <USEFOREXCISETRADERINVOICE>No</USEFOREXCISETRADERINVOICE>" + "\n" +
"          <USEFOREXCISE>No</USEFOREXCISE>" + "\n" +
"          <USEFORJOBWORK>No</USEFORJOBWORK>" + "\n" +
"          <ISFORJOBWORKIN>No</ISFORJOBWORKIN>" + "\n" +
"          <ALLOWCONSUMPTION>No</ALLOWCONSUMPTION>" + "\n" +
"          <USEFOREXCISEGOODS>No</USEFOREXCISEGOODS>" + "\n" +
"          <USEFOREXCISESUPPLEMENTARY>No</USEFOREXCISESUPPLEMENTARY>" + "\n" +
"          <ISDEFAULTALLOCENABLED>No</ISDEFAULTALLOCENABLED>" + "\n" +
"          <SORTPOSITION> 180</SORTPOSITION>" + "\n" +
"          <ALTERID> 50</ALTERID>" + "\n" +
"          <BEGINNINGNUMBER> 1</BEGINNINGNUMBER>" + "\n" +
"          <LANGUAGENAME.LIST>" + "\n" +
"           <NAME.LIST TYPE=\"String\">" + "\n" +
"            <NAME>Purchase Order</NAME>" + "\n" +
"           </NAME.LIST>" + "\n" +
"           <LANGUAGEID> 1033</LANGUAGEID>" + "\n" +
"          </LANGUAGENAME.LIST>" + "\n" +
"          <AUDITDETAILS.LIST>      </AUDITDETAILS.LIST>" + "\n" +
"          <EXCISETARIFFDETAILS.LIST>      </EXCISETARIFFDETAILS.LIST>" + "\n" +
"          <PREFIXLIST.LIST>      </PREFIXLIST.LIST>" + "\n" +
"          <SUFFIXLIST.LIST>      </SUFFIXLIST.LIST>" + "\n" +
"          <RESTARTFROMLIST.LIST>" + "\n" +
"           <DATE>20170401</DATE>" + "\n" +
"           <RESTARTFROM>Yearly</RESTARTFROM>" + "\n" +
"           <PERIODBEGINNIGNUM> 1</PERIODBEGINNIGNUM>" + "\n" +
"           <LASTNUMBERLIST.LIST>       </LASTNUMBERLIST.LIST>" + "\n" +
"          </RESTARTFROMLIST.LIST>" + "\n" +
"          <VOUCHERCLASSLIST.LIST>      </VOUCHERCLASSLIST.LIST>" + "\n" +
"          <PRODUCTCODEDETAILS.LIST>      </PRODUCTCODEDETAILS.LIST>" + "\n" +
"        </VOUCHERTYPE>" + "\n" +
"      </TALLYMESSAGE>\n";

                string voucherreceipt =
"          <TALLYMESSAGE xmlns:UDF=\"TallyUDF\">" + "\n" +
"             <VOUCHERTYPE NAME=\"Receipt\" RESERVEDNAME=\"Receipt\">" + "\n" +
"              <GUID>df13ea43-535b-411e-a3ae-09a30f52223d-00000022</GUID>" + "\n" +
"              <PARENT>Receipt</PARENT>" + "\n" +
"              <MAILINGNAME>Rcpt</MAILINGNAME>" + "\n" +
"              <TAXUNITNAME>&#4; Primary</TAXUNITNAME>" + "\n" +
"              <NUMBERINGMETHOD>Automatic</NUMBERINGMETHOD>" + "\n" +
"              <EXCISEUNITNAME>&#4; Primary</EXCISEUNITNAME>" + "\n" +
"              <CONTRACONTRA/>" + "\n" +
"              <PAYMENTCONTRA/>" + "\n" +
"              <RECEIPTCONTRA/>" + "\n" +
"              <JOURNALCONTRA/>" + "\n" +
"              <CNOTECONTRA/>" + "\n" +
"              <DNOTECONTRA/>" + "\n" +
"              <SALESCONTRA/>" + "\n" +
"              <PURCHASECONTRA/>" + "\n" +
"              <CREDITCSTCTR/>" + "\n" +
"              <DEBITCSTCTR/>" + "\n" +
"              <PREVIOUSPURCHASE/>" + "\n" +
"              <PREVIOUSSALES/>" + "\n" +
"              <PREVIOUSGODOWN/>" + "\n" +
"              <PREVNARRATION>7085145772</PREVNARRATION>" + "\n" +
"              <ISUPDATINGTARGETID>No</ISUPDATINGTARGETID>" + "\n" +
"              <ASORIGINAL>Yes</ASORIGINAL>" + "\n" +
"              <ISDEEMEDPOSITIVE>No</ISDEEMEDPOSITIVE>" + "\n" +
"              <AFFECTSSTOCK>No</AFFECTSSTOCK>" + "\n" +
"              <PREVENTDUPLICATES>No</PREVENTDUPLICATES>" + "\n" +
"              <PREFILLZERO>No</PREFILLZERO>" + "\n" +
"              <PRINTAFTERSAVE>No</PRINTAFTERSAVE>" + "\n" +
"              <FORMALRECEIPT>No</FORMALRECEIPT>" + "\n" +
"              <ASMFGJRNL>No</ASMFGJRNL>" + "\n" +
"              <EFFECTIVEDATE>No</EFFECTIVEDATE>" + "\n" +
"              <COMMONNARRATION>Yes</COMMONNARRATION>" + "\n" +
"              <MULTINARRATION>No</MULTINARRATION>" + "\n" +
"              <ISTAXINVOICE>No</ISTAXINVOICE>" + "\n" +
"              <USEFORPOSINVOICE>No</USEFORPOSINVOICE>" + "\n" +
"              <USEFOREXCISETRADERINVOICE>No</USEFOREXCISETRADERINVOICE>" + "\n" +
"              <USEFOREXCISE>No</USEFOREXCISE>" + "\n" +
"              <USEFORJOBWORK>No</USEFORJOBWORK>" + "\n" +
"              <ISFORJOBWORKIN>No</ISFORJOBWORKIN>" + "\n" +
"              <ALLOWCONSUMPTION>No</ALLOWCONSUMPTION>" + "\n" +
"              <USEFOREXCISEGOODS>No</USEFOREXCISEGOODS>" + "\n" +
"              <USEFOREXCISESUPPLEMENTARY>No</USEFOREXCISESUPPLEMENTARY>" + "\n" +
"              <ISDEFAULTALLOCENABLED>No</ISDEFAULTALLOCENABLED>" + "\n" +
"              <SORTPOSITION> 30</SORTPOSITION>" + "\n" +
"              <ALTERID> 35</ALTERID>" + "\n" +
"              <BEGINNINGNUMBER> 1</BEGINNINGNUMBER>" + "\n" +
"              <LANGUAGENAME.LIST>" + "\n" +
"               <NAME.LIST TYPE=\"String\">" + "\n" +
"                <NAME>Receipt</NAME>" + "\n" +
"               </NAME.LIST>" + "\n" +
"               <LANGUAGEID> 1033</LANGUAGEID>" + "\n" +
"              </LANGUAGENAME.LIST>" + "\n" +
"              <AUDITDETAILS.LIST>      </AUDITDETAILS.LIST>" + "\n" +
"              <EXCISETARIFFDETAILS.LIST>      </EXCISETARIFFDETAILS.LIST>" + "\n" +
"              <PREFIXLIST.LIST>      </PREFIXLIST.LIST>" + "\n" +
"              <SUFFIXLIST.LIST>      </SUFFIXLIST.LIST>" + "\n" +
"              <RESTARTFROMLIST.LIST>" + "\n" +
"               <DATE>20170401</DATE>" + "\n" +
"               <RESTARTFROM>Yearly</RESTARTFROM>" + "\n" +
"               <PERIODBEGINNIGNUM> 1</PERIODBEGINNIGNUM>" + "\n" +
"               <LASTNUMBERLIST.LIST>       </LASTNUMBERLIST.LIST>" + "\n" +
"              </RESTARTFROMLIST.LIST>" + "\n" +
"              <VOUCHERCLASSLIST.LIST>      </VOUCHERCLASSLIST.LIST>" + "\n" +
"              <PRODUCTCODEDETAILS.LIST>      </PRODUCTCODEDETAILS.LIST>" + "\n" +
"             </VOUCHERTYPE>" + "\n" +
"       </TALLYMESSAGE>\n";

                string voucherReceiptNote =
"      <TALLYMESSAGE xmlns:UDF=\"TallyUDF\">" + "\n" +
"        <VOUCHERTYPE NAME=\"Receipt Note\" RESERVEDNAME=\"Receipt Note\">" + "\n" +
"        <GUID>df13ea43-535b-411e-a3ae-09a30f52223d-00000029</GUID>" + "\n" +
"        <PARENT>Receipt Note</PARENT>" + "\n" +
"        <MAILINGNAME>Rcpt Note</MAILINGNAME>" + "\n" +
"        <TAXUNITNAME>&#4; Primary</TAXUNITNAME>" + "\n" +
"        <NUMBERINGMETHOD>Automatic</NUMBERINGMETHOD>" + "\n" +
"        <EXCISEUNITNAME>&#4; Primary</EXCISEUNITNAME>" + "\n" +
"        <ISUPDATINGTARGETID>No</ISUPDATINGTARGETID>" + "\n" +
"        <ASORIGINAL>Yes</ASORIGINAL>" + "\n" +
"        <ISDEEMEDPOSITIVE>Yes</ISDEEMEDPOSITIVE>" + "\n" +
"        <AFFECTSSTOCK>Yes</AFFECTSSTOCK>" + "\n" +
"        <PREVENTDUPLICATES>No</PREVENTDUPLICATES>" + "\n" +
"        <PREFILLZERO>No</PREFILLZERO>" + "\n" +
"        <PRINTAFTERSAVE>No</PRINTAFTERSAVE>" + "\n" +
"        <FORMALRECEIPT>No</FORMALRECEIPT>" + "\n" +
"        <ISOPTIONAL>No</ISOPTIONAL>" + "\n" +
"        <ASMFGJRNL>No</ASMFGJRNL>" + "\n" +
"        <EFFECTIVEDATE>No</EFFECTIVEDATE>" + "\n" +
"        <COMMONNARRATION>Yes</COMMONNARRATION>" + "\n" +
"        <MULTINARRATION>No</MULTINARRATION>" + "\n" +
"        <ISTAXINVOICE>No</ISTAXINVOICE>" + "\n" +
"        <USEFORPOSINVOICE>No</USEFORPOSINVOICE>" + "\n" +
"        <USEFOREXCISETRADERINVOICE>No</USEFOREXCISETRADERINVOICE>" + "\n" +
"        <USEFOREXCISE>No</USEFOREXCISE>" + "\n" +
"        <USEFORJOBWORK>No</USEFORJOBWORK>" + "\n" +
"        <ISFORJOBWORKIN>No</ISFORJOBWORKIN>" + "\n" +
"        <ALLOWCONSUMPTION>No</ALLOWCONSUMPTION>" + "\n" +
"        <USEFOREXCISEGOODS>No</USEFOREXCISEGOODS>" + "\n" +
"        <USEFOREXCISESUPPLEMENTARY>No</USEFOREXCISESUPPLEMENTARY>" + "\n" +
"        <ISDEFAULTALLOCENABLED>No</ISDEFAULTALLOCENABLED>" + "\n" +
"        <SORTPOSITION> 100</SORTPOSITION>" + "\n" +
"        <ALTERID> 42</ALTERID>" + "\n" +
"        <BEGINNINGNUMBER> 1</BEGINNINGNUMBER>" + "\n" +
"        <LANGUAGENAME.LIST>" + "\n" +
"         <NAME.LIST TYPE=\"String\">" + "\n" +
"          <NAME>Receipt Note</NAME>" + "\n" +
"         </NAME.LIST>" + "\n" +
"         <LANGUAGEID> 1033</LANGUAGEID>" + "\n" +
"        </LANGUAGENAME.LIST>" + "\n" +
"        <AUDITDETAILS.LIST>      </AUDITDETAILS.LIST>" + "\n" +
"        <EXCISETARIFFDETAILS.LIST>      </EXCISETARIFFDETAILS.LIST>" + "\n" +
"        <PREFIXLIST.LIST>      </PREFIXLIST.LIST>" + "\n" +
"        <SUFFIXLIST.LIST>      </SUFFIXLIST.LIST>" + "\n" +
"        <RESTARTFROMLIST.LIST>" + "\n" +
"          <DATE>20170401</DATE>" + "\n" +
"          <RESTARTFROM>Yearly</RESTARTFROM>" + "\n" +
"          <PERIODBEGINNIGNUM> 1</PERIODBEGINNIGNUM>" + "\n" +
"          <LASTNUMBERLIST.LIST>       </LASTNUMBERLIST.LIST>" + "\n" +
"        </RESTARTFROMLIST.LIST>" + "\n" +
"        <VOUCHERCLASSLIST.LIST>      </VOUCHERCLASSLIST.LIST>" + "\n" +
"        <PRODUCTCODEDETAILS.LIST>      </PRODUCTCODEDETAILS.LIST>" + "\n" +
"       </VOUCHERTYPE>" + "\n" +
"      </TALLYMESSAGE>\n";

        string voucherRejectionIn=
"    <TALLYMESSAGE xmlns:UDF=\"TallyUDF\">" + "\n" +
"      <VOUCHERTYPE NAME=\"Rejections In\" RESERVEDNAME=\"Rejections In\">" + "\n" +
"      <GUID>df13ea43-535b-411e-a3ae-09a30f52223d-0000002b</GUID>" + "\n" +
"      <PARENT>Rejections In</PARENT>" + "\n" +
"      <MAILINGNAME>Rej In</MAILINGNAME>" + "\n" +
"      <TAXUNITNAME>&#4; Primary</TAXUNITNAME>" + "\n" +
"      <NUMBERINGMETHOD>Automatic</NUMBERINGMETHOD>" + "\n" +
"      <EXCISEUNITNAME>&#4; Primary</EXCISEUNITNAME>" + "\n" +
"      <ISUPDATINGTARGETID>No</ISUPDATINGTARGETID>" + "\n" +
"      <ASORIGINAL>Yes</ASORIGINAL>" + "\n" +
"      <ISDEEMEDPOSITIVE>Yes</ISDEEMEDPOSITIVE>" + "\n" +
"      <AFFECTSSTOCK>Yes</AFFECTSSTOCK>" + "\n" +
"      <PREVENTDUPLICATES>No</PREVENTDUPLICATES>" + "\n" +
"      <PREFILLZERO>No</PREFILLZERO>" + "\n" +
"      <PRINTAFTERSAVE>No</PRINTAFTERSAVE>" + "\n" +
"      <FORMALRECEIPT>No</FORMALRECEIPT>" + "\n" +
"      <ISOPTIONAL>No</ISOPTIONAL>" + "\n" +
"      <ASMFGJRNL>No</ASMFGJRNL>" + "\n" +
"      <EFFECTIVEDATE>No</EFFECTIVEDATE>" + "\n" +
"      <COMMONNARRATION>Yes</COMMONNARRATION>" + "\n" +
"      <MULTINARRATION>No</MULTINARRATION>" + "\n" +
"      <ISTAXINVOICE>No</ISTAXINVOICE>" + "\n" +
"      <USEFORPOSINVOICE>No</USEFORPOSINVOICE>" + "\n" +
"      <USEFOREXCISETRADERINVOICE>No</USEFOREXCISETRADERINVOICE>" + "\n" +
"      <USEFOREXCISE>No</USEFOREXCISE>" + "\n" +
"      <USEFORJOBWORK>No</USEFORJOBWORK>" + "\n" +
"      <ISFORJOBWORKIN>No</ISFORJOBWORKIN>" + "\n" +
"      <ALLOWCONSUMPTION>No</ALLOWCONSUMPTION>" + "\n" +
"      <USEFOREXCISEGOODS>No</USEFOREXCISEGOODS>" + "\n" +
"      <USEFOREXCISESUPPLEMENTARY>No</USEFOREXCISESUPPLEMENTARY>" + "\n" +
"      <ISDEFAULTALLOCENABLED>No</ISDEFAULTALLOCENABLED>" + "\n" +
"      <SORTPOSITION> 120</SORTPOSITION>" + "\n" +
"      <ALTERID> 44</ALTERID>" + "\n" +
"      <BEGINNINGNUMBER> 1</BEGINNINGNUMBER>" + "\n" +
"      <LANGUAGENAME.LIST>" + "\n" +
"       <NAME.LIST TYPE=\"String\">" + "\n" +
"        <NAME>Rejections In</NAME>" + "\n" +
"       </NAME.LIST>" + "\n" +
"       <LANGUAGEID> 1033</LANGUAGEID>" + "\n" +
"      </LANGUAGENAME.LIST>" + "\n" +
"      <AUDITDETAILS.LIST>      </AUDITDETAILS.LIST>" + "\n" +
"      <EXCISETARIFFDETAILS.LIST>      </EXCISETARIFFDETAILS.LIST>" + "\n" +
"      <PREFIXLIST.LIST>      </PREFIXLIST.LIST>" + "\n" +
"      <SUFFIXLIST.LIST>      </SUFFIXLIST.LIST>" + "\n" +
"      <RESTARTFROMLIST.LIST>" + "\n" +
"       <DATE>20170401</DATE>" + "\n" +
"       <RESTARTFROM>Yearly</RESTARTFROM>" + "\n" +
"       <PERIODBEGINNIGNUM> 1</PERIODBEGINNIGNUM>" + "\n" +
"       <LASTNUMBERLIST.LIST>       </LASTNUMBERLIST.LIST>" + "\n" +
"      </RESTARTFROMLIST.LIST>" + "\n" +
"      <PRODUCTCODEDETAILS.LIST>      </PRODUCTCODEDETAILS.LIST>" + "\n" +
"     </VOUCHERTYPE>" + "\n" +
"    </TALLYMESSAGE>\n";

        string voucherRejectionOut=
"    <TALLYMESSAGE xmlns:UDF=\"TallyUDF\">" + "\n" +
"      <VOUCHERTYPE NAME=\"Rejections Out\" RESERVEDNAME=\"Rejections Out\">" + "\n" +
"      <GUID>df13ea43-535b-411e-a3ae-09a30f52223d-0000002a</GUID>" + "\n" +
"      <PARENT>Rejections Out</PARENT>" + "\n" +
"      <MAILINGNAME>Rej Out</MAILINGNAME>" + "\n" +
"      <TAXUNITNAME>&#4; Primary</TAXUNITNAME>" + "\n" +
"      <NUMBERINGMETHOD>Automatic</NUMBERINGMETHOD>" + "\n" +
"      <EXCISEUNITNAME>&#4; Primary</EXCISEUNITNAME>" + "\n" +
"      <ISUPDATINGTARGETID>No</ISUPDATINGTARGETID>" + "\n" +
"      <ASORIGINAL>Yes</ASORIGINAL>" + "\n" +
"      <ISDEEMEDPOSITIVE>No</ISDEEMEDPOSITIVE>" + "\n" +
"      <AFFECTSSTOCK>Yes</AFFECTSSTOCK>" + "\n" +
"      <PREVENTDUPLICATES>No</PREVENTDUPLICATES>" + "\n" +
"      <PREFILLZERO>No</PREFILLZERO>" + "\n" +
"      <PRINTAFTERSAVE>No</PRINTAFTERSAVE>" + "\n" +
"      <FORMALRECEIPT>No</FORMALRECEIPT>" + "\n" +
"      <ISOPTIONAL>No</ISOPTIONAL>" + "\n" +
"      <ASMFGJRNL>No</ASMFGJRNL>" + "\n" +
"      <EFFECTIVEDATE>No</EFFECTIVEDATE>" + "\n" +
"      <COMMONNARRATION>Yes</COMMONNARRATION>" + "\n" +
"      <MULTINARRATION>No</MULTINARRATION>" + "\n" +
"      <ISTAXINVOICE>No</ISTAXINVOICE>" + "\n" +
"      <USEFORPOSINVOICE>No</USEFORPOSINVOICE>" + "\n" +
"      <USEFOREXCISETRADERINVOICE>No</USEFOREXCISETRADERINVOICE>" + "\n" +
"      <USEFOREXCISE>No</USEFOREXCISE>" + "\n" +
"      <USEFORJOBWORK>No</USEFORJOBWORK>" + "\n" +
"      <ISFORJOBWORKIN>No</ISFORJOBWORKIN>" + "\n" +
"      <ALLOWCONSUMPTION>No</ALLOWCONSUMPTION>" + "\n" +
"      <USEFOREXCISEGOODS>No</USEFOREXCISEGOODS>" + "\n" +
"      <USEFOREXCISESUPPLEMENTARY>No</USEFOREXCISESUPPLEMENTARY>" + "\n" +
"      <ISDEFAULTALLOCENABLED>No</ISDEFAULTALLOCENABLED>" + "\n" +
"      <SORTPOSITION> 110</SORTPOSITION>" + "\n" +
"      <ALTERID> 43</ALTERID>" + "\n" +
"      <BEGINNINGNUMBER> 1</BEGINNINGNUMBER>" + "\n" +
"      <LANGUAGENAME.LIST>" + "\n" +
"       <NAME.LIST TYPE=\"String\">" + "\n" +
"        <NAME>Rejections Out</NAME>" + "\n" +
"       </NAME.LIST>" + "\n" +
"       <LANGUAGEID> 1033</LANGUAGEID>" + "\n" +
"      </LANGUAGENAME.LIST>" + "\n" +
"      <AUDITDETAILS.LIST>      </AUDITDETAILS.LIST>" + "\n" +
"      <EXCISETARIFFDETAILS.LIST>      </EXCISETARIFFDETAILS.LIST>" + "\n" +
"      <PREFIXLIST.LIST>      </PREFIXLIST.LIST>" + "\n" +
"      <SUFFIXLIST.LIST>      </SUFFIXLIST.LIST>" + "\n" +
"      <RESTARTFROMLIST.LIST>" + "\n" +
"       <DATE>20170401</DATE>" + "\n" +
"       <RESTARTFROM>Yearly</RESTARTFROM>" + "\n" +
"       <PERIODBEGINNIGNUM> 1</PERIODBEGINNIGNUM>" + "\n" +
"       <LASTNUMBERLIST.LIST>       </LASTNUMBERLIST.LIST>" + "\n" +
"      </RESTARTFROMLIST.LIST>" + "\n" +
"      <VOUCHERCLASSLIST.LIST>      </VOUCHERCLASSLIST.LIST>" + "\n" +
"      <PRODUCTCODEDETAILS.LIST>      </PRODUCTCODEDETAILS.LIST>" + "\n" +
"     </VOUCHERTYPE>" + "\n" +
"    </TALLYMESSAGE>\n";

        string voucherReversingJournal=
"    <TALLYMESSAGE xmlns:UDF=\"TallyUDF\">" + "\n" +
"      <VOUCHERTYPE NAME=\"Reversing Journal\" RESERVEDNAME=\"Reversing Journal\">" + "\n" +
"      <GUID>df13ea43-535b-411e-a3ae-09a30f52223d-0000002f</GUID>" + "\n" +
"      <PARENT>Reversing Journal</PARENT>" + "\n" +
"      <MAILINGNAME>Rev Jrnl</MAILINGNAME>" + "\n" +
"      <TAXUNITNAME>&#4; Primary</TAXUNITNAME>" + "\n" +
"      <NUMBERINGMETHOD>Automatic</NUMBERINGMETHOD>" + "\n" +
"      <EXCISEUNITNAME>&#4; Primary</EXCISEUNITNAME>" + "\n" +
"      <ISUPDATINGTARGETID>No</ISUPDATINGTARGETID>" + "\n" +
"      <ASORIGINAL>Yes</ASORIGINAL>" + "\n" +
"      <ISDEEMEDPOSITIVE>Yes</ISDEEMEDPOSITIVE>" + "\n" +
"      <AFFECTSSTOCK>No</AFFECTSSTOCK>" + "\n" +
"      <PREVENTDUPLICATES>No</PREVENTDUPLICATES>" + "\n" +
"      <PREFILLZERO>No</PREFILLZERO>" + "\n" +
"      <PRINTAFTERSAVE>No</PRINTAFTERSAVE>" + "\n" +
"      <FORMALRECEIPT>No</FORMALRECEIPT>" + "\n" +
"      <ISOPTIONAL>No</ISOPTIONAL>" + "\n" +
"      <ASMFGJRNL>No</ASMFGJRNL>" + "\n" +
"      <EFFECTIVEDATE>No</EFFECTIVEDATE>" + "\n" +
"      <COMMONNARRATION>Yes</COMMONNARRATION>" + "\n" +
"      <MULTINARRATION>No</MULTINARRATION>" + "\n" +
"      <ISTAXINVOICE>No</ISTAXINVOICE>" + "\n" +
"      <USEFORPOSINVOICE>No</USEFORPOSINVOICE>" + "\n" +
"      <USEFOREXCISETRADERINVOICE>No</USEFOREXCISETRADERINVOICE>" + "\n" +
"      <USEFOREXCISE>No</USEFOREXCISE>" + "\n" +
"      <USEFORJOBWORK>No</USEFORJOBWORK>" + "\n" +
"      <ISFORJOBWORKIN>No</ISFORJOBWORKIN>" + "\n" +
"      <ALLOWCONSUMPTION>No</ALLOWCONSUMPTION>" + "\n" +
"      <USEFOREXCISEGOODS>No</USEFOREXCISEGOODS>" + "\n" +
"      <USEFOREXCISESUPPLEMENTARY>No</USEFOREXCISESUPPLEMENTARY>" + "\n" +
"      <ISDEFAULTALLOCENABLED>No</ISDEFAULTALLOCENABLED>" + "\n" +
"      <SORTPOSITION> 160</SORTPOSITION>" + "\n" +
"      <ALTERID> 48</ALTERID>" + "\n" +
"      <BEGINNINGNUMBER> 1</BEGINNINGNUMBER>" + "\n" +
"      <LANGUAGENAME.LIST>" + "\n" +
"       <NAME.LIST TYPE=\"String\">" + "\n" +
"        <NAME>Reversing Journal</NAME>" + "\n" +
"       </NAME.LIST>" + "\n" +
"       <LANGUAGEID> 1033</LANGUAGEID>" + "\n" +
"      </LANGUAGENAME.LIST>" + "\n" +
"      <AUDITDETAILS.LIST>      </AUDITDETAILS.LIST>" + "\n" +
"      <EXCISETARIFFDETAILS.LIST>      </EXCISETARIFFDETAILS.LIST>" + "\n" +
"      <PREFIXLIST.LIST>      </PREFIXLIST.LIST>" + "\n" +
"      <SUFFIXLIST.LIST>      </SUFFIXLIST.LIST>" + "\n" +
"      <RESTARTFROMLIST.LIST>" + "\n" +
"      <DATE>20170401</DATE>" + "\n" +
"      <RESTARTFROM>Yearly</RESTARTFROM>" + "\n" +
"       <PERIODBEGINNIGNUM> 1</PERIODBEGINNIGNUM>" + "\n" +
"       <LASTNUMBERLIST.LIST>       </LASTNUMBERLIST.LIST>" + "\n" +
"      </RESTARTFROMLIST.LIST>" + "\n" +
"      <VOUCHERCLASSLIST.LIST>      </VOUCHERCLASSLIST.LIST>" + "\n" +
"      <PRODUCTCODEDETAILS.LIST>      </PRODUCTCODEDETAILS.LIST>" + "\n" +
"     </VOUCHERTYPE>" + "\n" +
"    </TALLYMESSAGE>\n";

        string voucherSales=
"    <TALLYMESSAGE xmlns:UDF=\"TallyUDF\">" + "\n" +
"      <VOUCHERTYPE NAME=\"Sales\" RESERVEDNAME=\"Sales\">" + "\n" +
"      <GUID>df13ea43-535b-411e-a3ae-09a30f52223d-00000026</GUID>" + "\n" +
"      <PARENT>Sales</PARENT>" + "\n" +
"      <MAILINGNAME>Sale</MAILINGNAME>" + "\n" +
"      <TAXUNITNAME>&#4; Primary</TAXUNITNAME>" + "\n" +
"      <NUMBERINGMETHOD>Automatic</NUMBERINGMETHOD>" + "\n" +
"      <EXCISEUNITNAME>&#4; Primary</EXCISEUNITNAME>" + "\n" +
"      <CONTRACONTRA/>" + "\n" +
"      <PAYMENTCONTRA/>" + "\n" +
"      <RECEIPTCONTRA/>" + "\n" +
"      <JOURNALCONTRA/>" + "\n" +
"      <CNOTECONTRA/>" + "\n" +
"      <DNOTECONTRA/>" + "\n" +
"      <SALESCONTRA/>" + "\n" +
"      <PURCHASECONTRA/>" + "\n" +
"      <CREDITCSTCTR/>" + "\n" +
"      <DEBITCSTCTR/>" + "\n" +
"      <PREVIOUSPURCHASE/>" + "\n" +
"      <PREVIOUSSALES/>" + "\n" +
"      <PREVIOUSGODOWN/>" + "\n" +
"      <PREVNARRATION>Bieng sale made to mridul Gupta</PREVNARRATION>" + "\n" +
"      <ISUPDATINGTARGETID>No</ISUPDATINGTARGETID>" + "\n" +
"      <ASORIGINAL>Yes</ASORIGINAL>" + "\n" +
"      <ISDEEMEDPOSITIVE>Yes</ISDEEMEDPOSITIVE>" + "\n" +
"      <AFFECTSSTOCK>No</AFFECTSSTOCK>" + "\n" +
"      <PREVENTDUPLICATES>No</PREVENTDUPLICATES>" + "\n" +
"      <PREFILLZERO>No</PREFILLZERO>" + "\n" +
"      <PRINTAFTERSAVE>No</PRINTAFTERSAVE>" + "\n" +
"      <FORMALRECEIPT>No</FORMALRECEIPT>" + "\n" +
"      <ISOPTIONAL>No</ISOPTIONAL>" + "\n" +
"      <ASMFGJRNL>No</ASMFGJRNL>" + "\n" +
"      <EFFECTIVEDATE>No</EFFECTIVEDATE>" + "\n" +
"      <COMMONNARRATION>Yes</COMMONNARRATION>" + "\n" +
"      <MULTINARRATION>No</MULTINARRATION>" + "\n" +
"      <ISTAXINVOICE>No</ISTAXINVOICE>" + "\n" +
"      <USEFORPOSINVOICE>No</USEFORPOSINVOICE>" + "\n" +
"      <USEFOREXCISETRADERINVOICE>No</USEFOREXCISETRADERINVOICE>" + "\n" +
"      <USEFOREXCISE>No</USEFOREXCISE>" + "\n" +
"      <USEFORJOBWORK>No</USEFORJOBWORK>" + "\n" +
"      <ISFORJOBWORKIN>No</ISFORJOBWORKIN>" + "\n" +
"      <ALLOWCONSUMPTION>No</ALLOWCONSUMPTION>" + "\n" +
"      <USEFOREXCISEGOODS>No</USEFOREXCISEGOODS>" + "\n" +
"      <USEFOREXCISESUPPLEMENTARY>No</USEFOREXCISESUPPLEMENTARY>" + "\n" +
"      <ISDEFAULTALLOCENABLED>No</ISDEFAULTALLOCENABLED>" + "\n" +
"      <SORTPOSITION> 70</SORTPOSITION>" + "\n" +
"      <ALTERID> 39</ALTERID>" + "\n" +
"      <BEGINNINGNUMBER> 1</BEGINNINGNUMBER>" + "\n" +
"      <LANGUAGENAME.LIST>" + "\n" +
"       <NAME.LIST TYPE=\"String\">" + "\n" +
"        <NAME>Sales</NAME>" + "\n" +
"       </NAME.LIST>" + "\n" +
"       <LANGUAGEID> 1033</LANGUAGEID>" + "\n" +
"      </LANGUAGENAME.LIST>" + "\n" +
"      <AUDITDETAILS.LIST>      </AUDITDETAILS.LIST>" + "\n" +
"      <EXCISETARIFFDETAILS.LIST>      </EXCISETARIFFDETAILS.LIST>" + "\n" +
"      <PREFIXLIST.LIST>      </PREFIXLIST.LIST>" + "\n" +
"      <SUFFIXLIST.LIST>      </SUFFIXLIST.LIST>" + "\n" +
"      <RESTARTFROMLIST.LIST>" + "\n" +
"       <DATE>20170401</DATE>" + "\n" +
"       <RESTARTFROM>Yearly</RESTARTFROM>" + "\n" +
"       <PERIODBEGINNIGNUM> 1</PERIODBEGINNIGNUM>" + "\n" +
"       <LASTNUMBERLIST.LIST>       </LASTNUMBERLIST.LIST>" + "\n" +
"      </RESTARTFROMLIST.LIST>" + "\n" +
"      <VOUCHERCLASSLIST.LIST>      </VOUCHERCLASSLIST.LIST>" + "\n" +
"      <PRODUCTCODEDETAILS.LIST>      </PRODUCTCODEDETAILS.LIST>" + "\n" +
"     </VOUCHERTYPE>" + "\n" +
"    </TALLYMESSAGE>\n";

        string voucherSalesOrder =
"    <TALLYMESSAGE xmlns:UDF=\"TallyUDF\">" + "\n" +
"      <VOUCHERTYPE NAME=\"Sales Order\" RESERVEDNAME=\"Sales Order\">" + "\n" +
"      <GUID>df13ea43-535b-411e-a3ae-09a30f52223d-00000030</GUID>" + "\n" +
"      <PARENT>Sales Order</PARENT>" + "\n" +
"      <MAILINGNAME>Sales Ordr</MAILINGNAME>" + "\n" +
"      <TAXUNITNAME>&#4; Primary</TAXUNITNAME>" + "\n" +
"      <NUMBERINGMETHOD>Automatic</NUMBERINGMETHOD>" + "\n" +
"      <EXCISEUNITNAME>&#4; Primary</EXCISEUNITNAME>" + "\n" +
"      <ISUPDATINGTARGETID>No</ISUPDATINGTARGETID>" + "\n" +
"      <ASORIGINAL>Yes</ASORIGINAL>" + "\n" +
"      <ISDEEMEDPOSITIVE>Yes</ISDEEMEDPOSITIVE>" + "\n" +
"      <AFFECTSSTOCK>No</AFFECTSSTOCK>" + "\n" +
"      <PREVENTDUPLICATES>No</PREVENTDUPLICATES>" + "\n" +
"      <PREFILLZERO>No</PREFILLZERO>" + "\n" +
"      <PRINTAFTERSAVE>No</PRINTAFTERSAVE>" + "\n" +
"      <FORMALRECEIPT>No</FORMALRECEIPT>" + "\n" +
"      <ISOPTIONAL>No</ISOPTIONAL>" + "\n" +
"      <ASMFGJRNL>No</ASMFGJRNL>" + "\n" +
"      <EFFECTIVEDATE>No</EFFECTIVEDATE>" + "\n" +
"      <COMMONNARRATION>Yes</COMMONNARRATION>" + "\n" +
"      <MULTINARRATION>No</MULTINARRATION>" + "\n" +
"      <ISTAXINVOICE>No</ISTAXINVOICE>" + "\n" +
"      <USEFORPOSINVOICE>No</USEFORPOSINVOICE>" + "\n" +
"      <USEFOREXCISETRADERINVOICE>No</USEFOREXCISETRADERINVOICE>" + "\n" +
"      <USEFOREXCISE>No</USEFOREXCISE>" + "\n" +
"      <USEFORJOBWORK>No</USEFORJOBWORK>" + "\n" +
"      <ISFORJOBWORKIN>No</ISFORJOBWORKIN>" + "\n" +
"      <ALLOWCONSUMPTION>No</ALLOWCONSUMPTION>" + "\n" +
"      <USEFOREXCISEGOODS>No</USEFOREXCISEGOODS>" + "\n" +
"      <USEFOREXCISESUPPLEMENTARY>No</USEFOREXCISESUPPLEMENTARY>" + "\n" +
"      <ISDEFAULTALLOCENABLED>No</ISDEFAULTALLOCENABLED>" + "\n" +
"      <SORTPOSITION> 170</SORTPOSITION>" + "\n" +
"      <ALTERID> 49</ALTERID>" + "\n" +
"      <BEGINNINGNUMBER> 1</BEGINNINGNUMBER>" + "\n" +
"      <LANGUAGENAME.LIST>" + "\n" +
"       <NAME.LIST TYPE=\"String\">" + "\n" +
"       <NAME>Sales Order</NAME>" + "\n" +
"       </NAME.LIST>" + "\n" +
"       <LANGUAGEID> 1033</LANGUAGEID>" + "\n" +
"      </LANGUAGENAME.LIST>" + "\n" +
"      <AUDITDETAILS.LIST>      </AUDITDETAILS.LIST>" + "\n" +
"      <EXCISETARIFFDETAILS.LIST>      </EXCISETARIFFDETAILS.LIST>" + "\n" +
"      <PREFIXLIST.LIST>      </PREFIXLIST.LIST>" + "\n" +
"      <SUFFIXLIST.LIST>      </SUFFIXLIST.LIST>" + "\n" +
"      <RESTARTFROMLIST.LIST>" + "\n" +
"       <DATE>20170401</DATE>" + "\n" +
"       <RESTARTFROM>Yearly</RESTARTFROM>" + "\n" +
"       <PERIODBEGINNIGNUM> 1</PERIODBEGINNIGNUM>" + "\n" +
"       <LASTNUMBERLIST.LIST>       </LASTNUMBERLIST.LIST>" + "\n" +
"      </RESTARTFROMLIST.LIST>" + "\n" +
"      <VOUCHERCLASSLIST.LIST>      </VOUCHERCLASSLIST.LIST>" + "\n" +
"      <PRODUCTCODEDETAILS.LIST>      </PRODUCTCODEDETAILS.LIST>" + "\n" +
"     </VOUCHERTYPE>" + "\n" +
"    </TALLYMESSAGE>\n";

        string voucherStockJournal =
"    <TALLYMESSAGE xmlns:UDF=\"TallyUDF\">" + "\n" +
"      <VOUCHERTYPE NAME=\"Stock Journal\" RESERVEDNAME=\"Stock Journal\">" + "\n" +
"      <GUID>df13ea43-535b-411e-a3ae-09a30f52223d-0000002d</GUID>" + "\n" +
"      <PARENT>Stock Journal</PARENT>" + "\n" +
"      <MAILINGNAME>Stk Jrnl</MAILINGNAME>" + "\n" +
"      <TAXUNITNAME>&#4; Primary</TAXUNITNAME>" + "\n" +
"      <NUMBERINGMETHOD>Automatic</NUMBERINGMETHOD>" + "\n" +
"      <EXCISEUNITNAME>&#4; Primary</EXCISEUNITNAME>" + "\n" +
"      <ISUPDATINGTARGETID>No</ISUPDATINGTARGETID>" + "\n" +
"      <ASORIGINAL>Yes</ASORIGINAL>" + "\n" +
"      <ISDEEMEDPOSITIVE>Yes</ISDEEMEDPOSITIVE>" + "\n" +
"      <AFFECTSSTOCK>Yes</AFFECTSSTOCK>" + "\n" +
"      <PREVENTDUPLICATES>No</PREVENTDUPLICATES>" + "\n" +
"      <PREFILLZERO>No</PREFILLZERO>" + "\n" +
"      <PRINTAFTERSAVE>No</PRINTAFTERSAVE>" + "\n" +
"      <FORMALRECEIPT>No</FORMALRECEIPT>" + "\n" +
"      <ISOPTIONAL>No</ISOPTIONAL>" + "\n" +
"      <ASMFGJRNL>No</ASMFGJRNL>" + "\n" +
"      <EFFECTIVEDATE>No</EFFECTIVEDATE>" + "\n" +
"      <COMMONNARRATION>Yes</COMMONNARRATION>" + "\n" +
"      <MULTINARRATION>No</MULTINARRATION>" + "\n" +
"      <ISTAXINVOICE>No</ISTAXINVOICE>" + "\n" +
"      <USEFORPOSINVOICE>No</USEFORPOSINVOICE>" + "\n" +
"      <USEFOREXCISETRADERINVOICE>No</USEFOREXCISETRADERINVOICE>" + "\n" +
"      <USEFOREXCISE>No</USEFOREXCISE>" + "\n" +
"      <USEFORJOBWORK>No</USEFORJOBWORK>" + "\n" +
"      <ISFORJOBWORKIN>No</ISFORJOBWORKIN>" + "\n" +
"      <ALLOWCONSUMPTION>No</ALLOWCONSUMPTION>" + "\n" +
"      <USEFOREXCISEGOODS>No</USEFOREXCISEGOODS>" + "\n" +
"      <USEFOREXCISESUPPLEMENTARY>No</USEFOREXCISESUPPLEMENTARY>" + "\n" +
"      <ISDEFAULTALLOCENABLED>No</ISDEFAULTALLOCENABLED>" + "\n" +
"      <SORTPOSITION> 140</SORTPOSITION>" + "\n" +
"      <ALTERID> 46</ALTERID>" + "\n" +
"      <BEGINNINGNUMBER> 1</BEGINNINGNUMBER>" + "\n" +
"      <LANGUAGENAME.LIST>" + "\n" +
"       <NAME.LIST TYPE=\"String\">" + "\n" +
"        <NAME>Stock Journal</NAME>" + "\n" +
"       </NAME.LIST>" + "\n" +
"       <LANGUAGEID> 1033</LANGUAGEID>" + "\n" +
"      </LANGUAGENAME.LIST>" + "\n" +
"      <AUDITDETAILS.LIST>      </AUDITDETAILS.LIST>" + "\n" +
"      <EXCISETARIFFDETAILS.LIST>      </EXCISETARIFFDETAILS.LIST>" + "\n" +
"      <PREFIXLIST.LIST>      </PREFIXLIST.LIST>" + "\n" +
"      <SUFFIXLIST.LIST>      </SUFFIXLIST.LIST>" + "\n" +
"      <RESTARTFROMLIST.LIST>" + "\n" +
"       <DATE>20170401</DATE>" + "\n" +
"       <RESTARTFROM>Yearly</RESTARTFROM>" + "\n" +
"       <PERIODBEGINNIGNUM> 1</PERIODBEGINNIGNUM>" + "\n" +
"       <LASTNUMBERLIST.LIST>       </LASTNUMBERLIST.LIST>" + "\n" +
"      </RESTARTFROMLIST.LIST>" + "\n" +
      "<VOUCHERCLASSLIST.LIST>      </VOUCHERCLASSLIST.LIST>" + "\n" +
      "<PRODUCTCODEDETAILS.LIST>      </PRODUCTCODEDETAILS.LIST>" + "\n" +
     "</VOUCHERTYPE>" + "\n" +
    "</TALLYMESSAGE>\n";

        string voucherJouranl1=
"    <TALLYMESSAGE xmlns:UDF=\"TallyUDF\">" + "\n" +
"      <VOUCHERTYPE NAME=\"Journal 1\" RESERVEDNAME=\"\">" + "\n" +
"      <GUID>df13ea43-535b-411e-a3ae-09a30f52223d-000000a1</GUID>" + "\n" +
"      <PARENT>Journal</PARENT>" + "\n" +
"      <MAILINGNAME>Jrnl</MAILINGNAME>" + "\n" +
"      <TAXUNITNAME>&#4; Primary</TAXUNITNAME>" + "\n" +
"      <NUMBERINGMETHOD>Manual</NUMBERINGMETHOD>" + "\n" +
"      <EXCISEUNITNAME>&#4; Primary</EXCISEUNITNAME>" + "\n" +
"      <CONTRACONTRA/>" + "\n" +
"      <PAYMENTCONTRA/>" + "\n" +
"      <RECEIPTCONTRA/>" + "\n" +
"      <JOURNALCONTRA/>" + "\n" +
"      <CNOTECONTRA/>" + "\n" +
"      <DNOTECONTRA/>" + "\n" +
"      <SALESCONTRA/>" + "\n" +
"      <PURCHASECONTRA/>" + "\n" +
"      <CREDITCSTCTR/>" + "\n" +
"      <DEBITCSTCTR/>" + "\n" +
"      <PREVIOUSPURCHASE/>" + "\n" +
"      <PREVIOUSSALES/>" + "\n" +
"      <PREVIOUSGODOWN/>" + "\n" +
"      <PREVNARRATION>7085145772.0</PREVNARRATION>" + "\n" +
"      <LASTNUMBER>Amazon/Settlement/03</LASTNUMBER>" + "\n" +
"      <ISUPDATINGTARGETID>No</ISUPDATINGTARGETID>" + "\n" +
"      <ASORIGINAL>Yes</ASORIGINAL>" + "\n" +
"      <ISDEEMEDPOSITIVE>Yes</ISDEEMEDPOSITIVE>" + "\n" +
"      <AFFECTSSTOCK>No</AFFECTSSTOCK>" + "\n" +
"      <PREVENTDUPLICATES>Yes</PREVENTDUPLICATES>" + "\n" +
"      <PREFILLZERO>No</PREFILLZERO>" + "\n" +
"      <PRINTAFTERSAVE>No</PRINTAFTERSAVE>" + "\n" +
"      <FORMALRECEIPT>No</FORMALRECEIPT>" + "\n" +
"      <ISOPTIONAL>No</ISOPTIONAL>" + "\n" +
"      <ASMFGJRNL>No</ASMFGJRNL>" + "\n" +
"      <EFFECTIVEDATE>No</EFFECTIVEDATE>" + "\n" +
"      <COMMONNARRATION>Yes</COMMONNARRATION>" + "\n" +
"      <MULTINARRATION>No</MULTINARRATION>" + "\n" +
"      <ISTAXINVOICE>No</ISTAXINVOICE>" + "\n" +
"      <USEFORPOSINVOICE>No</USEFORPOSINVOICE>" + "\n" +
"      <USEFOREXCISETRADERINVOICE>No</USEFOREXCISETRADERINVOICE>" + "\n" +
"      <USEFOREXCISE>No</USEFOREXCISE>" + "\n" +
"      <USEFORJOBWORK>No</USEFORJOBWORK>" + "\n" +
"      <ISFORJOBWORKIN>No</ISFORJOBWORKIN>" + "\n" +
"      <ALLOWCONSUMPTION>No</ALLOWCONSUMPTION>" + "\n" +
"      <USEFOREXCISEGOODS>No</USEFOREXCISEGOODS>" + "\n" +
"      <USEFOREXCISESUPPLEMENTARY>No</USEFOREXCISESUPPLEMENTARY>" + "\n" +
"      <ISDEFAULTALLOCENABLED>No</ISDEFAULTALLOCENABLED>" + "\n" +
"      <SORTPOSITION> 40</SORTPOSITION>" + "\n" +
"      <ALTERID> 228</ALTERID>" + "\n" +
"      <BEGINNINGNUMBER> 1</BEGINNINGNUMBER>" + "\n" +
"      <LANGUAGENAME.LIST>" + "\n" +
"       <NAME.LIST TYPE=\"String\">" + "\n" +
"         <NAME>Journal 1</NAME>" + "\n" +
"       </NAME.LIST>" + "\n" +
"       <LANGUAGEID> 1033</LANGUAGEID>" + "\n" +
"      </LANGUAGENAME.LIST>" + "\n" +
"      <AUDITDETAILS.LIST>      </AUDITDETAILS.LIST>" + "\n" +
"      <EXCISETARIFFDETAILS.LIST>      </EXCISETARIFFDETAILS.LIST>" + "\n" +
"      <PREFIXLIST.LIST>      </PREFIXLIST.LIST>" + "\n" +
"      <SUFFIXLIST.LIST>      </SUFFIXLIST.LIST>" + "\n" +
"      <RESTARTFROMLIST.LIST>" + "\n" +
"        <DATE>20170401</DATE>" + "\n" +
"        <RESTARTFROM>Yearly</RESTARTFROM>" + "\n" +
"        <PERIODBEGINNIGNUM> 1</PERIODBEGINNIGNUM>" + "\n" +
"        <LASTNUMBERLIST.LIST>" + "\n" +
"          <DATE>20170401</DATE>" + "\n" +
"        </LASTNUMBERLIST.LIST>" + "\n" +
"      </RESTARTFROMLIST.LIST>" + "\n" +
"      <VOUCHERCLASSLIST.LIST>      </VOUCHERCLASSLIST.LIST>" + "\n" +
"      <PRODUCTCODEDETAILS.LIST>      </PRODUCTCODEDETAILS.LIST>" + "\n" +
"      <UDF:ISUSEFOREXCISE.LIST DESC=\"`IsUseforExcise`\" ISLIST=\"YES\" TYPE=\"String\" INDEX=\"15124\">" + "\n" +
"       <UDF:ISUSEFOREXCISE DESC=\"`IsUseforExcise`\">No</UDF:ISUSEFOREXCISE>" + "\n" +
"      </UDF:ISUSEFOREXCISE.LIST>" + "\n" +
"     </VOUCHERTYPE>" + "\n" +
"    </TALLYMESSAGE>\n";

        string voucherEnd =
"         </REQUESTDATA>" + "\n" +
"     </IMPORTDATA>" + "\n" +
"   </BODY>" + "\n" +
" </ENVELOPE>";


        public string MasterVoucherXML(int? ddl_marketplace,int? sellers_id, string CompanyName)
        {
            string xmloutput = "";
            try
            {
                if (ddl_marketplace != null && ddl_marketplace != 0)
                {
                    var get_inventory = (from ob_tbl_inventory in dba.tbl_inventory
                                         select new
                                         {
                                             ob_tbl_inventory = ob_tbl_inventory
                                         }).Where(a => a.ob_tbl_inventory.tbl_sellers_id == sellers_id && a.ob_tbl_inventory.isactive == 1 && a.ob_tbl_inventory.tbl_marketplace_id == ddl_marketplace).ToList();

                    if (get_inventory != null)
                    {
                        string myvoucherHeader = VoucherHeader;
                        xmloutput += myvoucherHeader;

                        string myvouchernumber = voucherNumber;

                        Guid g;
                        // Create and display the value of two GUIDs.
                        g = Guid.NewGuid();
                       

                        myvouchernumber = myvouchernumber.Replace("#guiduniqueno#", g.ToString());
                        xmloutput += myvouchernumber;

                        string myvouchermoney = VoucherMoney;
                        myvouchermoney = myvouchermoney.Replace("#guiduniqueno#", g.ToString());
                        xmloutput += myvouchermoney;

                        string myvouchermoney1 = VoucherMoney;
                        myvouchermoney1 = myvouchermoney1.Replace("#guiduniqueno#", g.ToString());
                        xmloutput += myvouchermoney1;

                        string myvoucherwarehouse = voucherWarehouse;
                        myvoucherwarehouse = myvoucherwarehouse.Replace("#guiduniqueno#", g.ToString());
                        xmloutput += myvoucherwarehouse;

                        //int counter = 0;
                        foreach (var item in get_inventory)
                        {
                            //counter++;
                            //if (counter > 2)
                            //{
                            //    break;
                            //}

                            string Guid1 = "";
                            var get_guid_no = dba.tbl_seller_accounting_pkg_details.Where(a => a.seller_id == sellers_id).FirstOrDefault();
                            if (get_guid_no != null)
                            {
                                DateTime dt = DateTime.Now;
                                string value = dt.ToString("ddMMyyyy-HHMMssff");
                                Guid1 = get_guid_no.guid + "-" + value;
                            }

                            

                            string productname = item.ob_tbl_inventory.item_name;
                            double tax = 0, cgst = 0, sgst=0;
                            var categorydetails = dba.tbl_item_category.Where(a => a.id == item.ob_tbl_inventory.tbl_item_category_id && a.tbl_sellers_id == sellers_id).FirstOrDefault();
                            if (categorydetails != null)
                            {
                                var getcategoryslab = dba.tbl_category_slabs.Where(a => a.m_category_id == categorydetails.id && a.tbl_seller_id == sellers_id).OrderByDescending(a=>a.id).FirstOrDefault();
                                if (getcategoryslab != null)
                                {
                                   var tax1 =getcategoryslab.tax_rate;
                                    var cgst1 = tax / 2;
                                    var sgst1 = tax - cgst;

                                     string input_decimal_number = Convert.ToString(tax1);
                                     var regex = new System.Text.RegularExpressions.Regex("(?<=[\\.])[0-9]+");
                                     if (regex.IsMatch(input_decimal_number))
                                     {
                                         string decimal_places = regex.Match(input_decimal_number).Value;
                                         tax =Convert.ToDouble(decimal_places);
                                     }

                                     string input_decimal_number1 = Convert.ToString(cgst1);
                                     var regex1 = new System.Text.RegularExpressions.Regex("(?<=[\\.])[0-9]+");
                                     if (regex1.IsMatch(input_decimal_number1))
                                     {
                                         string decimal_places1 = regex.Match(input_decimal_number1).Value;
                                         cgst = Convert.ToDouble(decimal_places1);
                                     }

                                     string input_decimal_number2 = Convert.ToString(sgst1);
                                     var regex2 = new System.Text.RegularExpressions.Regex("(?<=[\\.])[0-9]+");
                                     if (regex2.IsMatch(input_decimal_number2))
                                     {
                                         string decimal_places2 = regex.Match(input_decimal_number2).Value;
                                         sgst = Convert.ToDouble(decimal_places2);
                                     }
                                }
                            }// end of if(categorydetails)

                            string myinventory = voucherinventory;
                            //cgst = 9;
                            //sgst = 9;
                            //tax = 18;
                            myinventory = myinventory.Replace("#guiduniqueno#", g.ToString());
                           myinventory = myinventory.Replace("#itemName#", productname);
                           myinventory = myinventory.Replace("#CGST#", Convert.ToString(cgst));
                           myinventory = myinventory.Replace("#SGST#", Convert.ToString(sgst));
                           myinventory = myinventory.Replace("#IGST#",Convert.ToString(tax));
                           xmloutput += myinventory;
                        }// end of for each loop
                        string myattendance = voucherAttendance;
                        xmloutput += myattendance;

                        string mycontra = voucherContra;
                        xmloutput += mycontra;

                        string mycredit = vouchercreditnode;
                        xmloutput += mycredit;

                        string mydebit = voucherdebitnode;
                        xmloutput += mydebit;

                        string mydeliverynote = voucherdeliverynote;
                        xmloutput += mydeliverynote;

                        string myjobworkorder = voucherjobworkinorder;
                        xmloutput += myjobworkorder;

                        string myjobworkoutorder = voucherjobworkoutorder;
                        xmloutput += myjobworkoutorder;

                        string myvoucherjournal = voucherJournal;
                        xmloutput += myvoucherjournal;

                        string mymaterialin = voucherMaterialIn;
                        xmloutput += mymaterialin;

                        string mymaterialout = vouchermaterialOut;
                        xmloutput += mymaterialout;

                        string mymemorandum = vouchermemorandum;
                        xmloutput += mymemorandum;

                        string mypayment = voucherpayment;
                        xmloutput += mypayment;

                        string mypayroll = voucherPayroll;
                        xmloutput += mypayroll;

                        string myphysicalstock = voucherPhysicalstock;
                        xmloutput += myphysicalstock;

                        string myvoucherpurchase = voucherPurchase;
                        xmloutput += myvoucherpurchase;

                        string mypurchaseorder = voucherPurchaseOrder;
                        xmloutput += mypurchaseorder;

                        string myvoucherreceipt = voucherreceipt;
                        xmloutput += myvoucherreceipt;

                        string myreceiptnote = voucherReceiptNote;
                        xmloutput += myreceiptnote;

                        string myrejectionin = voucherRejectionIn;
                        xmloutput += myrejectionin;

                        string myrejectionout = voucherRejectionOut;
                        xmloutput += myrejectionout;

                        string myreversingjournal = voucherReversingJournal;
                        xmloutput += myreversingjournal;

                        string myvouchersales = voucherSales;
                        xmloutput += myvouchersales;

                        string mysalesorder = voucherSalesOrder;
                        xmloutput += mysalesorder;

                        string mystockjournal = voucherStockJournal;
                        xmloutput += mystockjournal;

                        string myvoucherjournal1 = voucherJouranl1;
                        xmloutput += myvoucherjournal1;

                        string myvorucherend = voucherEnd;
                        xmloutput += myvorucherend;
                        
                    }
                }//end of if 
            }
            catch(Exception ex)
            {
            }
            return (xmloutput);
        }
    }
}