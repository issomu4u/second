using SellerVendor.Areas.Seller.Models.DBContext;
using SellerVendor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SellerVendor.Areas.Seller.Models
{
    public class MasterLedgerVoucherXML
    {
        public SellerContext dba = new SellerContext();
        public SellerAdminContext db = new SellerAdminContext();


        string VoucherHeader =
"<ENVELOPE>\n" +
"  <HEADER>\n" +
"    <TALLYREQUEST>Import Data</TALLYREQUEST>\n" +
"  </HEADER>\n" +
"  <BODY>\n" +
"   <IMPORTDATA>\n" +
"    <REQUESTDESC>\n" +
"     <REPORTNAME>All Masters</REPORTNAME>\n" +
"     <STATICVARIABLES>\n" +
"      <SVCURRENTCOMPANY>Test Amazon</SVCURRENTCOMPANY>\n" +
"     </STATICVARIABLES>\n" +
"    </REQUESTDESC>\n" +
"    <REQUESTDATA>\n";

        string voucheReserverd =
"   <TALLYMESSAGE xmlns:UDF=\"TallyUDF\">" + "\n" +
"    <CURRENCY NAME=\"₹\" RESERVEDNAME=\"\">" + "\n" +
"      <GUID>df13ea43-535b-411e-a3ae-09a30f52223d-0000001d</GUID>" + "\n" +
"      <MAILINGNAME>INR</MAILINGNAME>" + "\n" +
"      <ORIGINALNAME>₹</ORIGINALNAME>" + "\n" +
"      <EXPANDEDSYMBOL>INR</EXPANDEDSYMBOL>" + "\n" +
"      <DECIMALSYMBOL>paise</DECIMALSYMBOL>" + "\n" +
"      <ISUPDATINGTARGETID>No</ISUPDATINGTARGETID>" + "\n" +
"      <ASORIGINAL>Yes</ASORIGINAL>" + "\n" +
"      <ISSUFFIX>No</ISSUFFIX>" + "\n" +
"      <HASSPACE>Yes</HASSPACE>" + "\n" +
"      <INMILLIONS>No</INMILLIONS>" + "\n" +
"      <ALTERID> 359</ALTERID>" + "\n" +
"      <DECIMALPLACES> 2</DECIMALPLACES>" + "\n" +
"      <DECIMALPLACESFORPRINTING> 2</DECIMALPLACESFORPRINTING>" + "\n" +
"      <DAILYSTDRATES.LIST>      </DAILYSTDRATES.LIST>" + "\n" +
"      <DAILYBUYINGRATES.LIST>      </DAILYBUYINGRATES.LIST>" + "\n" +
"      <DAILYSELLINGRATES.LIST>      </DAILYSELLINGRATES.LIST>" + "\n" +
"     </CURRENCY>" + "\n" +
"    </TALLYMESSAGE>\n";

          string voucheReserverd1 =
"    <TALLYMESSAGE xmlns:UDF=\"TallyUDF\">" + "\n" +
"     <CURRENCY NAME=\"₹\" RESERVEDNAME=\"\">" + "\n" +
"      <GUID>df13ea43-535b-411e-a3ae-09a30f52223d-0000001d</GUID>" + "\n" +
"      <MAILINGNAME>INR</MAILINGNAME>" + "\n" +
"      <ORIGINALNAME>₹</ORIGINALNAME>" + "\n" +
"      <EXPANDEDSYMBOL>INR</EXPANDEDSYMBOL>" + "\n" +
"      <DECIMALSYMBOL>paise</DECIMALSYMBOL>" + "\n" +
"      <ISUPDATINGTARGETID>No</ISUPDATINGTARGETID>" + "\n" +
"      <ASORIGINAL>Yes</ASORIGINAL>" + "\n" +
"      <ISSUFFIX>No</ISSUFFIX>" + "\n" +
"      <HASSPACE>Yes</HASSPACE>" + "\n" +
"      <INMILLIONS>No</INMILLIONS>" + "\n" +
"      <ALTERID> 359</ALTERID>" + "\n" +
"      <DECIMALPLACES> 2</DECIMALPLACES>" + "\n" +
"      <DECIMALPLACESFORPRINTING> 2</DECIMALPLACESFORPRINTING>" + "\n" +
"      <DAILYSTDRATES.LIST>      </DAILYSTDRATES.LIST>" + "\n" +
"      <DAILYBUYINGRATES.LIST>      </DAILYBUYINGRATES.LIST>" + "\n" +
"      <DAILYSELLINGRATES.LIST>      </DAILYSELLINGRATES.LIST>" + "\n" +
"     </CURRENCY>" + "\n" +
"    </TALLYMESSAGE>\n";

        string voucherBranch =
"    <TALLYMESSAGE xmlns:UDF=\"TallyUDF\">"+ "\n" +
"     <GROUP NAME=\"Branch / Divisions\" RESERVEDNAME=\"Branch / Divisions\">"+ "\n" +
"      <GUID>df13ea43-535b-411e-a3ae-09a30f52223d-00000007</GUID>"+ "\n" +
"      <PARENT/>"+ "\n" +
"      <GRPDEBITPARENT/>"+ "\n" +
"      <GRPCREDITPARENT/>"+ "\n" +
"      <ISBILLWISEON>No</ISBILLWISEON>"+ "\n" +
"      <ISCOSTCENTRESON>No</ISCOSTCENTRESON>"+ "\n" +
"      <ISADDABLE>No</ISADDABLE>"+ "\n" +
"      <ISUPDATINGTARGETID>No</ISUPDATINGTARGETID>"+ "\n" +
"      <ASORIGINAL>Yes</ASORIGINAL>"+ "\n" +
"      <ISSUBLEDGER>No</ISSUBLEDGER>"+ "\n" +
"      <ISREVENUE>No</ISREVENUE>"+ "\n" +
"      <AFFECTSGROSSPROFIT>No</AFFECTSGROSSPROFIT>"+ "\n" +
"      <ISDEEMEDPOSITIVE>No</ISDEEMEDPOSITIVE>"+ "\n" +
"      <TRACKNEGATIVEBALANCES>No</TRACKNEGATIVEBALANCES>"+ "\n" +
"      <ISCONDENSED>No</ISCONDENSED>"+ "\n" +
"      <AFFECTSSTOCK>No</AFFECTSSTOCK>"+ "\n" +
"      <ISGROUPFORLOANRCPT>No</ISGROUPFORLOANRCPT>"+ "\n" +
"      <ISGROUPFORLOANPYMNT>No</ISGROUPFORLOANPYMNT>"+ "\n" +
"      <ISRATEINCLUSIVEVAT>No</ISRATEINCLUSIVEVAT>"+ "\n" +
"      <ISINVDETAILSENABLE>No</ISINVDETAILSENABLE>"+ "\n" +
"      <SORTPOSITION> 70</SORTPOSITION>"+ "\n" +
"      <ALTERID> 8</ALTERID>"+ "\n" +
"      <SERVICETAXDETAILS.LIST>      </SERVICETAXDETAILS.LIST>"+ "\n" +
"      <VATDETAILS.LIST>      </VATDETAILS.LIST>"+ "\n" +
"      <SALESTAXCESSDETAILS.LIST>      </SALESTAXCESSDETAILS.LIST>"+ "\n" +
"      <GSTDETAILS.LIST>      </GSTDETAILS.LIST>"+ "\n" +
"      <LANGUAGENAME.LIST>"+ "\n" +
"       <NAME.LIST TYPE=\"String\">"+ "\n" +
"        <NAME>Branch / Divisions</NAME>"+ "\n" +
"       </NAME.LIST>"+ "\n" +
"       <LANGUAGEID> 1033</LANGUAGEID>"+ "\n" +
"      </LANGUAGENAME.LIST>"+ "\n" +
"      <XBRLDETAIL.LIST>      </XBRLDETAIL.LIST>"+ "\n" +
"      <AUDITDETAILS.LIST>      </AUDITDETAILS.LIST>"+ "\n" +
"      <SCHVIDETAILS.LIST>      </SCHVIDETAILS.LIST>"+ "\n" +
"      <EXCISETARIFFDETAILS.LIST>      </EXCISETARIFFDETAILS.LIST>"+ "\n" +
"      <TCSCATEGORYDETAILS.LIST>      </TCSCATEGORYDETAILS.LIST>"+ "\n" +
"      <TDSCATEGORYDETAILS.LIST>      </TDSCATEGORYDETAILS.LIST>"+ "\n" +
"      <GSTCLASSFNIGSTRATES.LIST>      </GSTCLASSFNIGSTRATES.LIST>"+ "\n" +
"      <EXTARIFFDUTYHEADDETAILS.LIST>      </EXTARIFFDUTYHEADDETAILS.LIST>"+ "\n" +
"     </GROUP>"+ "\n" +
"    </TALLYMESSAGE>\n";

        string VoucherCapital=
"    <TALLYMESSAGE xmlns:UDF=\"TallyUDF\">"+ "\n" +
"     <GROUP NAME=\"Capital Account\" RESERVEDNAME=\"Capital Account\">"+ "\n" +
"      <GUID>df13ea43-535b-411e-a3ae-09a30f52223d-00000001</GUID>"+ "\n" +
"      <PARENT/>"+ "\n" +
"      <GRPDEBITPARENT/>"+ "\n" +
"      <GRPCREDITPARENT/>"+ "\n" +
"      <ISBILLWISEON>No</ISBILLWISEON>"+ "\n" +
"      <ISCOSTCENTRESON>No</ISCOSTCENTRESON>"+ "\n" +
"      <ISADDABLE>No</ISADDABLE>"+ "\n" +
"      <ISUPDATINGTARGETID>No</ISUPDATINGTARGETID>"+ "\n" +
"      <ASORIGINAL>Yes</ASORIGINAL>"+ "\n" +
"      <ISSUBLEDGER>No</ISSUBLEDGER>"+ "\n" +
"      <ISREVENUE>No</ISREVENUE>"+ "\n" +
"      <AFFECTSGROSSPROFIT>No</AFFECTSGROSSPROFIT>"+ "\n" +
"      <ISDEEMEDPOSITIVE>No</ISDEEMEDPOSITIVE>"+ "\n" +
"      <TRACKNEGATIVEBALANCES>No</TRACKNEGATIVEBALANCES>"+ "\n" +
"      <ISCONDENSED>No</ISCONDENSED>"+ "\n" +
"      <AFFECTSSTOCK>No</AFFECTSSTOCK>"+ "\n" +
"      <ISGROUPFORLOANRCPT>No</ISGROUPFORLOANRCPT>"+ "\n" +
"      <ISGROUPFORLOANPYMNT>No</ISGROUPFORLOANPYMNT>"+ "\n" +
"      <ISRATEINCLUSIVEVAT>No</ISRATEINCLUSIVEVAT>"+ "\n" +
"      <ISINVDETAILSENABLE>No</ISINVDETAILSENABLE>"+ "\n" +
"      <SORTPOSITION> 10</SORTPOSITION>"+ "\n" +
"      <ALTERID> 2</ALTERID>"+ "\n" +
"      <SERVICETAXDETAILS.LIST>      </SERVICETAXDETAILS.LIST>"+ "\n" +
"      <VATDETAILS.LIST>      </VATDETAILS.LIST>"+ "\n" +
"      <SALESTAXCESSDETAILS.LIST>      </SALESTAXCESSDETAILS.LIST>"+ "\n" +
"      <GSTDETAILS.LIST>      </GSTDETAILS.LIST>"+ "\n" +
"      <LANGUAGENAME.LIST>"+ "\n" +
"       <NAME.LIST TYPE=\"String\">"+ "\n" +
"        <NAME>Capital Account</NAME>"+ "\n" +
"       </NAME.LIST>"+ "\n" +
"       <LANGUAGEID> 1033</LANGUAGEID>"+ "\n" +
"      </LANGUAGENAME.LIST>"+ "\n" +
"      <XBRLDETAIL.LIST>      </XBRLDETAIL.LIST>"+ "\n" +
"      <AUDITDETAILS.LIST>      </AUDITDETAILS.LIST>"+ "\n" +
"      <SCHVIDETAILS.LIST>      </SCHVIDETAILS.LIST>"+ "\n" +
"      <EXCISETARIFFDETAILS.LIST>      </EXCISETARIFFDETAILS.LIST>"+ "\n" +
"      <TCSCATEGORYDETAILS.LIST>      </TCSCATEGORYDETAILS.LIST>"+ "\n" +
"      <TDSCATEGORYDETAILS.LIST>      </TDSCATEGORYDETAILS.LIST>"+ "\n" +
"      <GSTCLASSFNIGSTRATES.LIST>      </GSTCLASSFNIGSTRATES.LIST>"+ "\n" +
"      <EXTARIFFDUTYHEADDETAILS.LIST>      </EXTARIFFDUTYHEADDETAILS.LIST>"+ "\n" +
"     </GROUP>"+ "\n" +
"    </TALLYMESSAGE>\n";

        string VoucherCurrentAsset =
"    <TALLYMESSAGE xmlns:UDF=\"TallyUDF\">"+ "\n" +
"     <GROUP NAME=\"Current Assets\" RESERVEDNAME=\"Current Assets\">"+ "\n" +
"      <GUID>df13ea43-535b-411e-a3ae-09a30f52223d-00000006</GUID>"+ "\n" +
"      <PARENT/>"+ "\n" +
"      <GRPDEBITPARENT/>"+ "\n" +
"      <GRPCREDITPARENT/>"+ "\n" +
"      <ISBILLWISEON>No</ISBILLWISEON>"+ "\n" +
"      <ISCOSTCENTRESON>No</ISCOSTCENTRESON>"+ "\n" +
"      <ISADDABLE>No</ISADDABLE>"+ "\n" +
"      <ISUPDATINGTARGETID>No</ISUPDATINGTARGETID>"+ "\n" +
"      <ASORIGINAL>Yes</ASORIGINAL>"+ "\n" +
"      <ISSUBLEDGER>No</ISSUBLEDGER>"+ "\n" +
"      <ISREVENUE>No</ISREVENUE>"+ "\n" +
"      <AFFECTSGROSSPROFIT>No</AFFECTSGROSSPROFIT>"+ "\n" +
"      <ISDEEMEDPOSITIVE>Yes</ISDEEMEDPOSITIVE>"+ "\n" +
"      <TRACKNEGATIVEBALANCES>No</TRACKNEGATIVEBALANCES>"+ "\n" +
"      <ISCONDENSED>No</ISCONDENSED>"+ "\n" +
"      <AFFECTSSTOCK>No</AFFECTSSTOCK>"+ "\n" +
"      <ISGROUPFORLOANRCPT>No</ISGROUPFORLOANRCPT>"+ "\n" +
"      <ISGROUPFORLOANPYMNT>No</ISGROUPFORLOANPYMNT>"+ "\n" +
"      <ISRATEINCLUSIVEVAT>No</ISRATEINCLUSIVEVAT>"+ "\n" +
"      <ISINVDETAILSENABLE>No</ISINVDETAILSENABLE>"+ "\n" +
"      <SORTPOSITION> 60</SORTPOSITION>"+ "\n" +
"      <ALTERID> 7</ALTERID>"+ "\n" +
"      <SERVICETAXDETAILS.LIST>      </SERVICETAXDETAILS.LIST>"+ "\n" +
"      <VATDETAILS.LIST>      </VATDETAILS.LIST>"+ "\n" +
"      <SALESTAXCESSDETAILS.LIST>      </SALESTAXCESSDETAILS.LIST>"+ "\n" +
"      <GSTDETAILS.LIST>      </GSTDETAILS.LIST>"+ "\n" +
"      <LANGUAGENAME.LIST>"+ "\n" +
"       <NAME.LIST TYPE=\"String\">"+ "\n" +
"        <NAME>Current Assets</NAME>"+ "\n" +
"       </NAME.LIST>"+ "\n" +
"       <LANGUAGEID> 1033</LANGUAGEID>"+ "\n" +
"      </LANGUAGENAME.LIST>"+ "\n" +
"      <XBRLDETAIL.LIST>      </XBRLDETAIL.LIST>"+ "\n" +
"      <AUDITDETAILS.LIST>      </AUDITDETAILS.LIST>"+ "\n" +
"      <SCHVIDETAILS.LIST>      </SCHVIDETAILS.LIST>"+ "\n" +
"      <EXCISETARIFFDETAILS.LIST>      </EXCISETARIFFDETAILS.LIST>"+ "\n" +
"      <TCSCATEGORYDETAILS.LIST>      </TCSCATEGORYDETAILS.LIST>"+ "\n" +
"      <TDSCATEGORYDETAILS.LIST>      </TDSCATEGORYDETAILS.LIST>"+ "\n" +
"      <GSTCLASSFNIGSTRATES.LIST>      </GSTCLASSFNIGSTRATES.LIST>"+ "\n" +
"      <EXTARIFFDUTYHEADDETAILS.LIST>      </EXTARIFFDUTYHEADDETAILS.LIST>"+ "\n" +
"     </GROUP>"+ "\n" +
"    </TALLYMESSAGE>\n";

        string VoucherCurrentLiabilities =
"   <TALLYMESSAGE xmlns:UDF=\"TallyUDF\">"+ "\n" +
"     <GROUP NAME=\"Current Liabilities\" RESERVEDNAME=\"Current Liabilities\">"+ "\n" +
"      <GUID>df13ea43-535b-411e-a3ae-09a30f52223d-00000003</GUID>"+ "\n" +
"      <PARENT/>"+ "\n" +
"      <GRPDEBITPARENT/>"+ "\n" +
"      <GRPCREDITPARENT/>"+ "\n" +
"      <ISBILLWISEON>No</ISBILLWISEON>"+ "\n" +
"      <ISCOSTCENTRESON>No</ISCOSTCENTRESON>"+ "\n" +
"      <ISADDABLE>No</ISADDABLE>"+ "\n" +
"      <ISUPDATINGTARGETID>No</ISUPDATINGTARGETID>"+ "\n" +
"      <ASORIGINAL>Yes</ASORIGINAL>"+ "\n" +
"      <ISSUBLEDGER>No</ISSUBLEDGER>"+ "\n" +
"      <ISREVENUE>No</ISREVENUE>"+ "\n" +
"      <AFFECTSGROSSPROFIT>No</AFFECTSGROSSPROFIT>"+ "\n" +
"      <ISDEEMEDPOSITIVE>No</ISDEEMEDPOSITIVE>"+ "\n" +
"      <TRACKNEGATIVEBALANCES>No</TRACKNEGATIVEBALANCES>"+ "\n" +
"      <ISCONDENSED>No</ISCONDENSED>"+ "\n" +
"      <AFFECTSSTOCK>No</AFFECTSSTOCK>"+ "\n" +
"      <ISGROUPFORLOANRCPT>No</ISGROUPFORLOANRCPT>"+ "\n" +
"      <ISGROUPFORLOANPYMNT>No</ISGROUPFORLOANPYMNT>"+ "\n" +
"      <ISRATEINCLUSIVEVAT>No</ISRATEINCLUSIVEVAT>"+ "\n" +
"      <ISINVDETAILSENABLE>No</ISINVDETAILSENABLE>"+ "\n" +
"      <SORTPOSITION> 30</SORTPOSITION>"+ "\n" +
"      <ALTERID> 4</ALTERID>"+ "\n" +
"      <SERVICETAXDETAILS.LIST>      </SERVICETAXDETAILS.LIST>"+ "\n" +
"      <VATDETAILS.LIST>      </VATDETAILS.LIST>"+ "\n" +
"      <SALESTAXCESSDETAILS.LIST>      </SALESTAXCESSDETAILS.LIST>"+ "\n" +
"      <GSTDETAILS.LIST>      </GSTDETAILS.LIST>"+ "\n" +
"      <LANGUAGENAME.LIST>"+ "\n" +
"       <NAME.LIST TYPE=\"String\">"+ "\n" +
"        <NAME>Current Liabilities</NAME>"+ "\n" +
"       </NAME.LIST>"+ "\n" +
"       <LANGUAGEID> 1033</LANGUAGEID>"+ "\n" +
"      </LANGUAGENAME.LIST>"+ "\n" +
"      <XBRLDETAIL.LIST>      </XBRLDETAIL.LIST>"+ "\n" +
"      <AUDITDETAILS.LIST>      </AUDITDETAILS.LIST>"+ "\n" +
"      <SCHVIDETAILS.LIST>      </SCHVIDETAILS.LIST>"+ "\n" +
"      <EXCISETARIFFDETAILS.LIST>      </EXCISETARIFFDETAILS.LIST>"+ "\n" +
"      <TCSCATEGORYDETAILS.LIST>      </TCSCATEGORYDETAILS.LIST>"+ "\n" +
"      <TDSCATEGORYDETAILS.LIST>      </TDSCATEGORYDETAILS.LIST>"+ "\n" +
"      <GSTCLASSFNIGSTRATES.LIST>      </GSTCLASSFNIGSTRATES.LIST>"+ "\n" +
"      <EXTARIFFDUTYHEADDETAILS.LIST>      </EXTARIFFDUTYHEADDETAILS.LIST>"+ "\n" +
"     </GROUP>"+ "\n" +
"    </TALLYMESSAGE>\n";

        string voucherDirectExpenses=
"    <TALLYMESSAGE xmlns:UDF=\"TallyUDF\">"+ "\n" +
"     <GROUP NAME=\"Direct Expenses\" RESERVEDNAME=\"Direct Expenses\">"+ "\n" +
"      <GUID>df13ea43-535b-411e-a3ae-09a30f52223d-0000001a</GUID>"+ "\n" +
"      <PARENT/>"+ "\n" +
"      <GRPDEBITPARENT/>"+ "\n" +
"      <GRPCREDITPARENT/>"+ "\n" +
"      <ISBILLWISEON>Yes</ISBILLWISEON>"+ "\n" +
"      <ISCOSTCENTRESON>Yes</ISCOSTCENTRESON>"+ "\n" +
"      <ISADDABLE>No</ISADDABLE>"+ "\n" +
"      <ISUPDATINGTARGETID>No</ISUPDATINGTARGETID>"+ "\n" +
"      <ASORIGINAL>Yes</ASORIGINAL>"+ "\n" +
"      <ISSUBLEDGER>No</ISSUBLEDGER>"+ "\n" +
"      <ISREVENUE>Yes</ISREVENUE>"+ "\n" +
"      <AFFECTSGROSSPROFIT>Yes</AFFECTSGROSSPROFIT>"+ "\n" +
"      <ISDEEMEDPOSITIVE>Yes</ISDEEMEDPOSITIVE>"+ "\n" +
"      <TRACKNEGATIVEBALANCES>No</TRACKNEGATIVEBALANCES>"+ "\n" +
"      <ISCONDENSED>No</ISCONDENSED>"+ "\n" +
"      <AFFECTSSTOCK>No</AFFECTSSTOCK>"+ "\n" +
"      <ISGROUPFORLOANRCPT>No</ISGROUPFORLOANRCPT>"+ "\n" +
"      <ISGROUPFORLOANPYMNT>No</ISGROUPFORLOANPYMNT>"+ "\n" +
"      <ISRATEINCLUSIVEVAT>No</ISRATEINCLUSIVEVAT>"+ "\n" +
"      <ISINVDETAILSENABLE>No</ISINVDETAILSENABLE>"+ "\n" +
"      <SORTPOSITION> 260</SORTPOSITION>"+ "\n" +
"      <ALTERID> 27</ALTERID>"+ "\n" +
"      <SERVICETAXDETAILS.LIST>      </SERVICETAXDETAILS.LIST>"+ "\n" +
"      <VATDETAILS.LIST>      </VATDETAILS.LIST>"+ "\n" +
"      <SALESTAXCESSDETAILS.LIST>      </SALESTAXCESSDETAILS.LIST>"+ "\n" +
"      <GSTDETAILS.LIST>      </GSTDETAILS.LIST>"+ "\n" +
"      <LANGUAGENAME.LIST>"+ "\n" +
"       <NAME.LIST TYPE=\"String\">"+ "\n" +
"        <NAME>Direct Expenses</NAME>"+ "\n" +
"        <NAME>Expenses (Direct)</NAME>"+ "\n" +
"       </NAME.LIST>"+ "\n" +
"       <LANGUAGEID> 1033</LANGUAGEID>"+ "\n" +
"      </LANGUAGENAME.LIST>"+ "\n" +
"      <XBRLDETAIL.LIST>      </XBRLDETAIL.LIST>"+ "\n" +
"      <AUDITDETAILS.LIST>      </AUDITDETAILS.LIST>"+ "\n" +
"      <SCHVIDETAILS.LIST>      </SCHVIDETAILS.LIST>"+ "\n" +
"      <EXCISETARIFFDETAILS.LIST>      </EXCISETARIFFDETAILS.LIST>"+ "\n" +
"      <TCSCATEGORYDETAILS.LIST>      </TCSCATEGORYDETAILS.LIST>"+ "\n" +
"      <TDSCATEGORYDETAILS.LIST>      </TDSCATEGORYDETAILS.LIST>"+ "\n" +
"      <GSTCLASSFNIGSTRATES.LIST>      </GSTCLASSFNIGSTRATES.LIST>"+ "\n" +
"      <EXTARIFFDUTYHEADDETAILS.LIST>      </EXTARIFFDUTYHEADDETAILS.LIST>"+ "\n" +
"     </GROUP>"+ "\n" +
"    </TALLYMESSAGE>\n";

        string DirectIncome =
"   <TALLYMESSAGE xmlns:UDF=\"TallyUDF\">"+ "\n" +
"     <GROUP NAME=\"Direct Incomes\" RESERVEDNAME=\"Direct Incomes\">"+ "\n" +
"      <GUID>df13ea43-535b-411e-a3ae-09a30f52223d-00000019</GUID>"+ "\n" +
"      <PARENT/>"+ "\n" +
"      <GRPDEBITPARENT/>"+ "\n" +
"      <GRPCREDITPARENT/>"+ "\n" +
"      <ISBILLWISEON>Yes</ISBILLWISEON>"+ "\n" +
"      <ISCOSTCENTRESON>Yes</ISCOSTCENTRESON>"+ "\n" +
"      <ISADDABLE>No</ISADDABLE>"+ "\n" +
"      <ISUPDATINGTARGETID>No</ISUPDATINGTARGETID>"+ "\n" +
"      <ASORIGINAL>Yes</ASORIGINAL>"+ "\n" +
"      <ISSUBLEDGER>No</ISSUBLEDGER>"+ "\n" +
"      <ISREVENUE>Yes</ISREVENUE>"+ "\n" +
"      <AFFECTSGROSSPROFIT>Yes</AFFECTSGROSSPROFIT>"+ "\n" +
"      <ISDEEMEDPOSITIVE>No</ISDEEMEDPOSITIVE>"+ "\n" +
"      <TRACKNEGATIVEBALANCES>No</TRACKNEGATIVEBALANCES>"+ "\n" +
"      <ISCONDENSED>No</ISCONDENSED>"+ "\n" +
"      <AFFECTSSTOCK>No</AFFECTSSTOCK>"+ "\n" +
"      <ISGROUPFORLOANRCPT>No</ISGROUPFORLOANRCPT>"+ "\n" +
"      <ISGROUPFORLOANPYMNT>No</ISGROUPFORLOANPYMNT>"+ "\n" +
"      <ISRATEINCLUSIVEVAT>No</ISRATEINCLUSIVEVAT>"+ "\n" +
"      <ISINVDETAILSENABLE>No</ISINVDETAILSENABLE>"+ "\n" +
"      <SORTPOSITION> 250</SORTPOSITION>"+ "\n" +
"      <ALTERID> 26</ALTERID>"+ "\n" +
"      <SERVICETAXDETAILS.LIST>      </SERVICETAXDETAILS.LIST>"+ "\n" +
"      <VATDETAILS.LIST>      </VATDETAILS.LIST>"+ "\n" +
"      <SALESTAXCESSDETAILS.LIST>      </SALESTAXCESSDETAILS.LIST>"+ "\n" +
"      <GSTDETAILS.LIST>      </GSTDETAILS.LIST>"+ "\n" +
"      <LANGUAGENAME.LIST>"+ "\n" +
"       <NAME.LIST TYPE=\"String\">"+ "\n" +
"        <NAME>Direct Incomes</NAME>"+ "\n" +
"        <NAME>Income (Direct)</NAME>"+ "\n" +
"       </NAME.LIST>"+ "\n" +
"       <LANGUAGEID> 1033</LANGUAGEID>"+ "\n" +
"      </LANGUAGENAME.LIST>"+ "\n" +
"      <XBRLDETAIL.LIST>      </XBRLDETAIL.LIST>"+ "\n" +
"      <AUDITDETAILS.LIST>      </AUDITDETAILS.LIST>"+ "\n" +
"      <SCHVIDETAILS.LIST>      </SCHVIDETAILS.LIST>"+ "\n" +
"      <EXCISETARIFFDETAILS.LIST>      </EXCISETARIFFDETAILS.LIST>"+ "\n" +
"      <TCSCATEGORYDETAILS.LIST>      </TCSCATEGORYDETAILS.LIST>"+ "\n" +
"      <TDSCATEGORYDETAILS.LIST>      </TDSCATEGORYDETAILS.LIST>"+ "\n" +
"      <GSTCLASSFNIGSTRATES.LIST>      </GSTCLASSFNIGSTRATES.LIST>"+ "\n" +
"      <EXTARIFFDUTYHEADDETAILS.LIST>      </EXTARIFFDUTYHEADDETAILS.LIST>"+ "\n" +
"     </GROUP>"+ "\n" +
"    </TALLYMESSAGE>\n";

        string FixedAssets =
"    <TALLYMESSAGE xmlns:UDF=\"TallyUDF\">"+ "\n" +
"     <GROUP NAME=\"Fixed Assets\" RESERVEDNAME=\"Fixed Assets\">"+ "\n" +
"      <GUID>df13ea43-535b-411e-a3ae-09a30f52223d-00000004</GUID>"+ "\n" +
"      <PARENT/>"+ "\n" +
"      <GRPDEBITPARENT/>"+ "\n" +
"      <GRPCREDITPARENT/>"+ "\n" +
"      <ISBILLWISEON>No</ISBILLWISEON>"+ "\n" +
"      <ISCOSTCENTRESON>No</ISCOSTCENTRESON>"+ "\n" +
"      <ISADDABLE>No</ISADDABLE>"+ "\n" +
"      <ISUPDATINGTARGETID>No</ISUPDATINGTARGETID>"+ "\n" +
"      <ASORIGINAL>Yes</ASORIGINAL>"+ "\n" +
"      <ISSUBLEDGER>No</ISSUBLEDGER>"+ "\n" +
"      <ISREVENUE>No</ISREVENUE>"+ "\n" +
"      <AFFECTSGROSSPROFIT>No</AFFECTSGROSSPROFIT>"+ "\n" +
"      <ISDEEMEDPOSITIVE>Yes</ISDEEMEDPOSITIVE>"+ "\n" +
"      <TRACKNEGATIVEBALANCES>No</TRACKNEGATIVEBALANCES>"+ "\n" +
"      <ISCONDENSED>No</ISCONDENSED>"+ "\n" +
"      <AFFECTSSTOCK>No</AFFECTSSTOCK>"+ "\n" +
"      <ISGROUPFORLOANRCPT>No</ISGROUPFORLOANRCPT>"+ "\n" +
"      <ISGROUPFORLOANPYMNT>No</ISGROUPFORLOANPYMNT>"+ "\n" +
"      <ISRATEINCLUSIVEVAT>No</ISRATEINCLUSIVEVAT>"+ "\n" +
"      <ISINVDETAILSENABLE>No</ISINVDETAILSENABLE>"+ "\n" +
"      <SORTPOSITION> 40</SORTPOSITION>"+ "\n" +
"      <ALTERID> 5</ALTERID>"+ "\n" +
"      <SERVICETAXDETAILS.LIST>      </SERVICETAXDETAILS.LIST>"+ "\n" +
"      <VATDETAILS.LIST>      </VATDETAILS.LIST>"+ "\n" +
"      <SALESTAXCESSDETAILS.LIST>      </SALESTAXCESSDETAILS.LIST>"+ "\n" +
"      <GSTDETAILS.LIST>      </GSTDETAILS.LIST>"+ "\n" +
"      <LANGUAGENAME.LIST>"+ "\n" +
"       <NAME.LIST TYPE=\"String\">"+ "\n" +
"        <NAME>Fixed Assets</NAME>"+ "\n" +
"       </NAME.LIST>"+ "\n" +
"       <LANGUAGEID> 1033</LANGUAGEID>"+ "\n" +
"      </LANGUAGENAME.LIST>"+ "\n" +
"      <XBRLDETAIL.LIST>      </XBRLDETAIL.LIST>"+ "\n" +
"      <AUDITDETAILS.LIST>      </AUDITDETAILS.LIST>"+ "\n" +
"      <SCHVIDETAILS.LIST>      </SCHVIDETAILS.LIST>"+ "\n" +
"      <EXCISETARIFFDETAILS.LIST>      </EXCISETARIFFDETAILS.LIST>"+ "\n" +
"      <TCSCATEGORYDETAILS.LIST>      </TCSCATEGORYDETAILS.LIST>"+ "\n" +
"      <TDSCATEGORYDETAILS.LIST>      </TDSCATEGORYDETAILS.LIST>"+ "\n" +
"      <GSTCLASSFNIGSTRATES.LIST>      </GSTCLASSFNIGSTRATES.LIST>"+ "\n" +
"      <EXTARIFFDUTYHEADDETAILS.LIST>      </EXTARIFFDUTYHEADDETAILS.LIST>"+ "\n" +
"     </GROUP>"+ "\n" +
"    </TALLYMESSAGE>\n";

        string VoucherIndirectExpenses =
"    <TALLYMESSAGE xmlns:UDF=\"TallyUDF\">"+ "\n" +
"     <GROUP NAME=\"Indirect Expenses\" RESERVEDNAME=\"Indirect Expenses\">"+ "\n" +
"      <GUID>df13ea43-535b-411e-a3ae-09a30f52223d-0000001c</GUID>"+ "\n" +
"      <PARENT/>"+ "\n" +
"      <GRPDEBITPARENT/>"+ "\n" +
"      <GRPCREDITPARENT/>"+ "\n" +
"      <ISBILLWISEON>No</ISBILLWISEON>"+ "\n" +
"      <ISCOSTCENTRESON>Yes</ISCOSTCENTRESON>"+ "\n" +
"      <ISADDABLE>No</ISADDABLE>"+ "\n" +
"      <ISUPDATINGTARGETID>No</ISUPDATINGTARGETID>"+ "\n" +
"      <ASORIGINAL>Yes</ASORIGINAL>"+ "\n" +
"      <ISSUBLEDGER>No</ISSUBLEDGER>"+ "\n" +
"      <ISREVENUE>Yes</ISREVENUE>"+ "\n" +
"      <AFFECTSGROSSPROFIT>No</AFFECTSGROSSPROFIT>"+ "\n" +
"      <ISDEEMEDPOSITIVE>Yes</ISDEEMEDPOSITIVE>"+ "\n" +
"      <TRACKNEGATIVEBALANCES>No</TRACKNEGATIVEBALANCES>"+ "\n" +
"      <ISCONDENSED>No</ISCONDENSED>"+ "\n" +
"      <AFFECTSSTOCK>No</AFFECTSSTOCK>"+ "\n" +
"      <ISGROUPFORLOANRCPT>No</ISGROUPFORLOANRCPT>"+ "\n" +
"      <ISGROUPFORLOANPYMNT>No</ISGROUPFORLOANPYMNT>"+ "\n" +
"      <ISRATEINCLUSIVEVAT>No</ISRATEINCLUSIVEVAT>"+ "\n" +
"      <ISINVDETAILSENABLE>No</ISINVDETAILSENABLE>"+ "\n" +
"      <SORTPOSITION> 280</SORTPOSITION>"+ "\n" +
"      <ALTERID> 29</ALTERID>"+ "\n" +
"      <SERVICETAXDETAILS.LIST>      </SERVICETAXDETAILS.LIST>"+ "\n" +
"      <VATDETAILS.LIST>      </VATDETAILS.LIST>"+ "\n" +
"      <SALESTAXCESSDETAILS.LIST>      </SALESTAXCESSDETAILS.LIST>"+ "\n" +
"      <GSTDETAILS.LIST>      </GSTDETAILS.LIST>"+ "\n" +
"      <LANGUAGENAME.LIST>"+ "\n" +
"       <NAME.LIST TYPE=\"String\">"+ "\n" +
"        <NAME>Indirect Expenses</NAME>"+ "\n" +
"        <NAME>Expenses (Indirect)</NAME>"+ "\n" +
"       </NAME.LIST>"+ "\n" +
"       <LANGUAGEID> 1033</LANGUAGEID>"+ "\n" +
"      </LANGUAGENAME.LIST>"+ "\n" +
"      <XBRLDETAIL.LIST>      </XBRLDETAIL.LIST>"+ "\n" +
"      <AUDITDETAILS.LIST>      </AUDITDETAILS.LIST>"+ "\n" +
"      <SCHVIDETAILS.LIST>      </SCHVIDETAILS.LIST>"+ "\n" +
"      <EXCISETARIFFDETAILS.LIST>      </EXCISETARIFFDETAILS.LIST>"+ "\n" +
"      <TCSCATEGORYDETAILS.LIST>      </TCSCATEGORYDETAILS.LIST>"+ "\n" +
"      <TDSCATEGORYDETAILS.LIST>      </TDSCATEGORYDETAILS.LIST>"+ "\n" +
"      <GSTCLASSFNIGSTRATES.LIST>      </GSTCLASSFNIGSTRATES.LIST>"+ "\n" +
"      <EXTARIFFDUTYHEADDETAILS.LIST>      </EXTARIFFDUTYHEADDETAILS.LIST>"+ "\n" +
"     </GROUP>"+ "\n" +
"    </TALLYMESSAGE>\n";

        string voucherIndirectIncome =
"    <TALLYMESSAGE xmlns:UDF=\"TallyUDF\">"+ "\n" +
"     <GROUP NAME=\"Indirect Incomes\" RESERVEDNAME=\"Indirect Incomes\">"+ "\n" +
"      <GUID>df13ea43-535b-411e-a3ae-09a30f52223d-0000001b</GUID>"+ "\n" +
"      <PARENT/>"+ "\n" +
"      <GRPDEBITPARENT/>"+ "\n" +
"      <GRPCREDITPARENT/>"+ "\n" +
"      <ISBILLWISEON>No</ISBILLWISEON>"+ "\n" +
"      <ISCOSTCENTRESON>Yes</ISCOSTCENTRESON>"+ "\n" +
"      <ISADDABLE>No</ISADDABLE>"+ "\n" +
"      <ISUPDATINGTARGETID>No</ISUPDATINGTARGETID>"+ "\n" +
"      <ASORIGINAL>Yes</ASORIGINAL>"+ "\n" +
"      <ISSUBLEDGER>No</ISSUBLEDGER>"+ "\n" +
"      <ISREVENUE>Yes</ISREVENUE>"+ "\n" +
"      <AFFECTSGROSSPROFIT>No</AFFECTSGROSSPROFIT>"+ "\n" +
"      <ISDEEMEDPOSITIVE>No</ISDEEMEDPOSITIVE>"+ "\n" +
"      <TRACKNEGATIVEBALANCES>No</TRACKNEGATIVEBALANCES>"+ "\n" +
"      <ISCONDENSED>No</ISCONDENSED>"+ "\n" +
"      <AFFECTSSTOCK>No</AFFECTSSTOCK>"+ "\n" +
"      <ISGROUPFORLOANRCPT>No</ISGROUPFORLOANRCPT>"+ "\n" +
"      <ISGROUPFORLOANPYMNT>No</ISGROUPFORLOANPYMNT>"+ "\n" +
"      <ISRATEINCLUSIVEVAT>No</ISRATEINCLUSIVEVAT>"+ "\n" +
"      <ISINVDETAILSENABLE>No</ISINVDETAILSENABLE>"+ "\n" +
"      <SORTPOSITION> 270</SORTPOSITION>"+ "\n" +
"      <ALTERID> 28</ALTERID>"+ "\n" +
"      <SERVICETAXDETAILS.LIST>      </SERVICETAXDETAILS.LIST>"+ "\n" +
"      <VATDETAILS.LIST>      </VATDETAILS.LIST>"+ "\n" +
"      <SALESTAXCESSDETAILS.LIST>      </SALESTAXCESSDETAILS.LIST>"+ "\n" +
"      <GSTDETAILS.LIST>      </GSTDETAILS.LIST>"+ "\n" +
"      <LANGUAGENAME.LIST>"+ "\n" +
"       <NAME.LIST TYPE=\"String\">"+ "\n" +
"        <NAME>Indirect Incomes</NAME>"+ "\n" +
"        <NAME>Income (Indirect)</NAME>"+ "\n" +
"       </NAME.LIST>"+ "\n" +
"       <LANGUAGEID> 1033</LANGUAGEID>"+ "\n" +
"      </LANGUAGENAME.LIST>"+ "\n" +
"      <XBRLDETAIL.LIST>      </XBRLDETAIL.LIST>"+ "\n" +
"      <AUDITDETAILS.LIST>      </AUDITDETAILS.LIST>"+ "\n" +
"      <SCHVIDETAILS.LIST>      </SCHVIDETAILS.LIST>"+ "\n" +
"      <EXCISETARIFFDETAILS.LIST>      </EXCISETARIFFDETAILS.LIST>"+ "\n" +
"      <TCSCATEGORYDETAILS.LIST>      </TCSCATEGORYDETAILS.LIST>"+ "\n" +
"      <TDSCATEGORYDETAILS.LIST>      </TDSCATEGORYDETAILS.LIST>"+ "\n" +
"      <GSTCLASSFNIGSTRATES.LIST>      </GSTCLASSFNIGSTRATES.LIST>"+ "\n" +
"      <EXTARIFFDUTYHEADDETAILS.LIST>      </EXTARIFFDUTYHEADDETAILS.LIST>"+ "\n" +
"     </GROUP>"+ "\n" +
"    </TALLYMESSAGE>\n";

        string VoucherInvestment =
"    <TALLYMESSAGE xmlns:UDF=\"TallyUDF\">"+ "\n" +
"     <GROUP NAME=\"Investments\" RESERVEDNAME=\"Investments\">"+ "\n" +
"      <GUID>df13ea43-535b-411e-a3ae-09a30f52223d-00000005</GUID>"+ "\n" +
"      <PARENT/>"+ "\n" +
"      <GRPDEBITPARENT/>"+ "\n" +
"      <GRPCREDITPARENT/>"+ "\n" +
"      <ISBILLWISEON>No</ISBILLWISEON>"+ "\n" +
"      <ISCOSTCENTRESON>No</ISCOSTCENTRESON>"+ "\n" +
"      <ISADDABLE>No</ISADDABLE>"+ "\n" +
"      <ISUPDATINGTARGETID>No</ISUPDATINGTARGETID>"+ "\n" +
"      <ASORIGINAL>Yes</ASORIGINAL>"+ "\n" +
"      <ISSUBLEDGER>No</ISSUBLEDGER>"+ "\n" +
"      <ISREVENUE>No</ISREVENUE>"+ "\n" +
"      <AFFECTSGROSSPROFIT>No</AFFECTSGROSSPROFIT>"+ "\n" +
"      <ISDEEMEDPOSITIVE>Yes</ISDEEMEDPOSITIVE>"+ "\n" +
"      <TRACKNEGATIVEBALANCES>No</TRACKNEGATIVEBALANCES>"+ "\n" +
"      <ISCONDENSED>No</ISCONDENSED>"+ "\n" +
"      <AFFECTSSTOCK>No</AFFECTSSTOCK>"+ "\n" +
"      <ISGROUPFORLOANRCPT>No</ISGROUPFORLOANRCPT>"+ "\n" +
"      <ISGROUPFORLOANPYMNT>No</ISGROUPFORLOANPYMNT>"+ "\n" +
"      <ISRATEINCLUSIVEVAT>No</ISRATEINCLUSIVEVAT>"+ "\n" +
"      <ISINVDETAILSENABLE>No</ISINVDETAILSENABLE>"+ "\n" +
"      <SORTPOSITION> 50</SORTPOSITION>"+ "\n" +
"      <ALTERID> 6</ALTERID>"+ "\n" +
"      <SERVICETAXDETAILS.LIST>      </SERVICETAXDETAILS.LIST>"+ "\n" +
"      <VATDETAILS.LIST>      </VATDETAILS.LIST>"+ "\n" +
"      <SALESTAXCESSDETAILS.LIST>      </SALESTAXCESSDETAILS.LIST>"+ "\n" +
"      <GSTDETAILS.LIST>      </GSTDETAILS.LIST>"+ "\n" +
"      <LANGUAGENAME.LIST>"+ "\n" +
"       <NAME.LIST TYPE=\"String\">"+ "\n" +
"        <NAME>Investments</NAME>"+ "\n" +
"       </NAME.LIST>"+ "\n" +
"       <LANGUAGEID> 1033</LANGUAGEID>"+ "\n" +
"      </LANGUAGENAME.LIST>"+ "\n" +
"      <XBRLDETAIL.LIST>      </XBRLDETAIL.LIST>"+ "\n" +
"      <AUDITDETAILS.LIST>      </AUDITDETAILS.LIST>"+ "\n" +
"      <SCHVIDETAILS.LIST>      </SCHVIDETAILS.LIST>"+ "\n" +
"      <EXCISETARIFFDETAILS.LIST>      </EXCISETARIFFDETAILS.LIST>"+ "\n" +
"      <TCSCATEGORYDETAILS.LIST>      </TCSCATEGORYDETAILS.LIST>"+ "\n" +
"      <TDSCATEGORYDETAILS.LIST>      </TDSCATEGORYDETAILS.LIST>"+ "\n" +
"      <GSTCLASSFNIGSTRATES.LIST>      </GSTCLASSFNIGSTRATES.LIST>"+ "\n" +
"      <EXTARIFFDUTYHEADDETAILS.LIST>      </EXTARIFFDUTYHEADDETAILS.LIST>"+ "\n" +
"     </GROUP>"+ "\n" +
"    </TALLYMESSAGE>\n";

        string VoucherLoans =
"    <TALLYMESSAGE xmlns:UDF=\"TallyUDF\">"+ "\n" +
"     <GROUP NAME=\"Loans (Liability)\" RESERVEDNAME=\"Loans (Liability)\">"+ "\n" +
"      <GUID>df13ea43-535b-411e-a3ae-09a30f52223d-00000002</GUID>"+ "\n" +
"      <PARENT/>"+ "\n" +
"      <GRPDEBITPARENT/>"+ "\n" +
"      <GRPCREDITPARENT/>"+ "\n" +
"      <ISBILLWISEON>No</ISBILLWISEON>"+ "\n" +
"      <ISCOSTCENTRESON>No</ISCOSTCENTRESON>"+ "\n" +
"      <ISADDABLE>No</ISADDABLE>"+ "\n" +
"      <ISUPDATINGTARGETID>No</ISUPDATINGTARGETID>"+ "\n" +
"      <ASORIGINAL>Yes</ASORIGINAL>"+ "\n" +
"      <ISSUBLEDGER>No</ISSUBLEDGER>"+ "\n" +
"      <ISREVENUE>No</ISREVENUE>"+ "\n" +
"      <AFFECTSGROSSPROFIT>No</AFFECTSGROSSPROFIT>"+ "\n" +
"      <ISDEEMEDPOSITIVE>No</ISDEEMEDPOSITIVE>"+ "\n" +
"      <TRACKNEGATIVEBALANCES>No</TRACKNEGATIVEBALANCES>"+ "\n" +
"      <ISCONDENSED>No</ISCONDENSED>"+ "\n" +
"      <AFFECTSSTOCK>No</AFFECTSSTOCK>"+ "\n" +
"      <ISGROUPFORLOANRCPT>No</ISGROUPFORLOANRCPT>"+ "\n" +
"      <ISGROUPFORLOANPYMNT>No</ISGROUPFORLOANPYMNT>"+ "\n" +
"      <ISRATEINCLUSIVEVAT>No</ISRATEINCLUSIVEVAT>"+ "\n" +
"      <ISINVDETAILSENABLE>No</ISINVDETAILSENABLE>"+ "\n" +
"      <SORTPOSITION> 20</SORTPOSITION>"+ "\n" +
"      <ALTERID> 3</ALTERID>"+ "\n" +
"      <SERVICETAXDETAILS.LIST>      </SERVICETAXDETAILS.LIST>"+ "\n" +
"      <VATDETAILS.LIST>      </VATDETAILS.LIST>"+ "\n" +
"      <SALESTAXCESSDETAILS.LIST>      </SALESTAXCESSDETAILS.LIST>"+ "\n" +
"      <GSTDETAILS.LIST>      </GSTDETAILS.LIST>"+ "\n" +
"      <LANGUAGENAME.LIST>"+ "\n" +
"       <NAME.LIST TYPE=\"String\">"+ "\n" +
"        <NAME>Loans (Liability)</NAME>"+ "\n" +
"       </NAME.LIST>"+ "\n" +
"       <LANGUAGEID> 1033</LANGUAGEID>"+ "\n" +
"      </LANGUAGENAME.LIST>"+ "\n" +
"      <XBRLDETAIL.LIST>      </XBRLDETAIL.LIST>"+ "\n" +
"      <AUDITDETAILS.LIST>      </AUDITDETAILS.LIST>"+ "\n" +
"      <SCHVIDETAILS.LIST>      </SCHVIDETAILS.LIST>"+ "\n" +
"      <EXCISETARIFFDETAILS.LIST>      </EXCISETARIFFDETAILS.LIST>"+ "\n" +
"      <TCSCATEGORYDETAILS.LIST>      </TCSCATEGORYDETAILS.LIST>"+ "\n" +
"      <TDSCATEGORYDETAILS.LIST>      </TDSCATEGORYDETAILS.LIST>"+ "\n" +
"      <GSTCLASSFNIGSTRATES.LIST>      </GSTCLASSFNIGSTRATES.LIST>"+ "\n" +
"      <EXTARIFFDUTYHEADDETAILS.LIST>      </EXTARIFFDUTYHEADDETAILS.LIST>"+ "\n" +
"     </GROUP>"+ "\n" +
"    </TALLYMESSAGE>\n";

        string VoucherMiscExpenses=
"    <TALLYMESSAGE xmlns:UDF=\"TallyUDF\">"+ "\n" +
"     <GROUP NAME=\"Misc. Expenses (ASSET)\" RESERVEDNAME=\"Misc. Expenses (ASSET)\">"+ "\n" +
"      <GUID>df13ea43-535b-411e-a3ae-09a30f52223d-00000008</GUID>"+ "\n" +
"      <PARENT/>"+ "\n" +
"      <GRPDEBITPARENT/>"+ "\n" +
"      <GRPCREDITPARENT/>"+ "\n" +
"      <ISBILLWISEON>No</ISBILLWISEON>"+ "\n" +
"      <ISCOSTCENTRESON>No</ISCOSTCENTRESON>"+ "\n" +
"      <ISADDABLE>No</ISADDABLE>"+ "\n" +
"      <ISUPDATINGTARGETID>No</ISUPDATINGTARGETID>"+ "\n" +
"      <ASORIGINAL>Yes</ASORIGINAL>"+ "\n" +
"      <ISSUBLEDGER>No</ISSUBLEDGER>"+ "\n" +
"      <ISREVENUE>No</ISREVENUE>"+ "\n" +
"      <AFFECTSGROSSPROFIT>No</AFFECTSGROSSPROFIT>"+ "\n" +
"      <ISDEEMEDPOSITIVE>Yes</ISDEEMEDPOSITIVE>"+ "\n" +
"      <TRACKNEGATIVEBALANCES>No</TRACKNEGATIVEBALANCES>"+ "\n" +
"      <ISCONDENSED>No</ISCONDENSED>"+ "\n" +
"      <AFFECTSSTOCK>No</AFFECTSSTOCK>"+ "\n" +
"      <ISGROUPFORLOANRCPT>No</ISGROUPFORLOANRCPT>"+ "\n" +
"      <ISGROUPFORLOANPYMNT>No</ISGROUPFORLOANPYMNT>"+ "\n" +
"      <ISRATEINCLUSIVEVAT>No</ISRATEINCLUSIVEVAT>"+ "\n" +
"      <ISINVDETAILSENABLE>No</ISINVDETAILSENABLE>"+ "\n" +
"      <SORTPOSITION> 80</SORTPOSITION>"+ "\n" +
"      <ALTERID> 9</ALTERID>"+ "\n" +
"      <SERVICETAXDETAILS.LIST>      </SERVICETAXDETAILS.LIST>"+ "\n" +
"      <VATDETAILS.LIST>      </VATDETAILS.LIST>"+ "\n" +
"      <SALESTAXCESSDETAILS.LIST>      </SALESTAXCESSDETAILS.LIST>"+ "\n" +
"      <GSTDETAILS.LIST>      </GSTDETAILS.LIST>"+ "\n" +
"      <LANGUAGENAME.LIST>"+ "\n" +
"       <NAME.LIST TYPE=\"String\">"+ "\n" +
"        <NAME>Misc. Expenses (ASSET)</NAME>"+ "\n" +
"       </NAME.LIST>"+ "\n" +
"       <LANGUAGEID> 1033</LANGUAGEID>"+ "\n" +
"      </LANGUAGENAME.LIST>"+ "\n" +
"      <XBRLDETAIL.LIST>      </XBRLDETAIL.LIST>"+ "\n" +
"      <AUDITDETAILS.LIST>      </AUDITDETAILS.LIST>"+ "\n" +
"      <SCHVIDETAILS.LIST>      </SCHVIDETAILS.LIST>"+ "\n" +
"      <EXCISETARIFFDETAILS.LIST>      </EXCISETARIFFDETAILS.LIST>"+ "\n" +
"      <TCSCATEGORYDETAILS.LIST>      </TCSCATEGORYDETAILS.LIST>"+ "\n" +
"      <TDSCATEGORYDETAILS.LIST>      </TDSCATEGORYDETAILS.LIST>"+ "\n" +
"      <GSTCLASSFNIGSTRATES.LIST>      </GSTCLASSFNIGSTRATES.LIST>"+ "\n" +
"      <EXTARIFFDUTYHEADDETAILS.LIST>      </EXTARIFFDUTYHEADDETAILS.LIST>"+ "\n" +
"     </GROUP>"+ "\n" +
"    </TALLYMESSAGE>\n";

        string VoucherPurchaseAccount =
"    <TALLYMESSAGE xmlns:UDF=\"TallyUDF\">"+ "\n" +
"     <GROUP NAME=\"Purchase Accounts\" RESERVEDNAME=\"Purchase Accounts\">"+ "\n" +
"      <GUID>df13ea43-535b-411e-a3ae-09a30f52223d-00000018</GUID>"+ "\n" +
"      <PARENT/>"+ "\n" +
"      <GRPDEBITPARENT/>"+ "\n" +
"      <GRPCREDITPARENT/>"+ "\n" +
"      <ISBILLWISEON>No</ISBILLWISEON>"+ "\n" +
"      <ISCOSTCENTRESON>Yes</ISCOSTCENTRESON>"+ "\n" +
"      <ISADDABLE>No</ISADDABLE>"+ "\n" +
"      <ISUPDATINGTARGETID>No</ISUPDATINGTARGETID>"+ "\n" +
"      <ASORIGINAL>Yes</ASORIGINAL>"+ "\n" +
"      <ISSUBLEDGER>No</ISSUBLEDGER>"+ "\n" +
"      <ISREVENUE>Yes</ISREVENUE>"+ "\n" +
"      <AFFECTSGROSSPROFIT>Yes</AFFECTSGROSSPROFIT>"+ "\n" +
"      <ISDEEMEDPOSITIVE>Yes</ISDEEMEDPOSITIVE>"+ "\n" +
"      <TRACKNEGATIVEBALANCES>No</TRACKNEGATIVEBALANCES>"+ "\n" +
"      <ISCONDENSED>No</ISCONDENSED>"+ "\n" +
"      <AFFECTSSTOCK>Yes</AFFECTSSTOCK>"+ "\n" +
"      <ISGROUPFORLOANRCPT>No</ISGROUPFORLOANRCPT>"+ "\n" +
"      <ISGROUPFORLOANPYMNT>No</ISGROUPFORLOANPYMNT>"+ "\n" +
"      <ISRATEINCLUSIVEVAT>No</ISRATEINCLUSIVEVAT>"+ "\n" +
"      <ISINVDETAILSENABLE>No</ISINVDETAILSENABLE>"+ "\n" +
"      <SORTPOSITION> 240</SORTPOSITION>"+ "\n" +
"      <ALTERID> 25</ALTERID>"+ "\n" +
"      <SERVICETAXDETAILS.LIST>      </SERVICETAXDETAILS.LIST>"+ "\n" +
"      <VATDETAILS.LIST>      </VATDETAILS.LIST>"+ "\n" +
"      <SALESTAXCESSDETAILS.LIST>      </SALESTAXCESSDETAILS.LIST>"+ "\n" +
"      <GSTDETAILS.LIST>      </GSTDETAILS.LIST>"+ "\n" +
"      <LANGUAGENAME.LIST>"+ "\n" +
"       <NAME.LIST TYPE=\"String\">"+ "\n" +
"        <NAME>Purchase Accounts</NAME>"+ "\n" +
"       </NAME.LIST>"+ "\n" +
"       <LANGUAGEID> 1033</LANGUAGEID>"+ "\n" +
"      </LANGUAGENAME.LIST>"+ "\n" +
"      <XBRLDETAIL.LIST>      </XBRLDETAIL.LIST>"+ "\n" +
"      <AUDITDETAILS.LIST>      </AUDITDETAILS.LIST>"+ "\n" +
"      <SCHVIDETAILS.LIST>      </SCHVIDETAILS.LIST>"+ "\n" +
"      <EXCISETARIFFDETAILS.LIST>      </EXCISETARIFFDETAILS.LIST>"+ "\n" +
"      <TCSCATEGORYDETAILS.LIST>      </TCSCATEGORYDETAILS.LIST>"+ "\n" +
"      <TDSCATEGORYDETAILS.LIST>      </TDSCATEGORYDETAILS.LIST>"+ "\n" +
"      <GSTCLASSFNIGSTRATES.LIST>      </GSTCLASSFNIGSTRATES.LIST>"+ "\n" +
"      <EXTARIFFDUTYHEADDETAILS.LIST>      </EXTARIFFDUTYHEADDETAILS.LIST>"+ "\n" +
"     </GROUP>"+ "\n" +
"    </TALLYMESSAGE>\n";

        string VoucherSalesAccounts=
"    <TALLYMESSAGE xmlns:UDF=\"TallyUDF\">"+ "\n" +
"     <GROUP NAME=\"Sales Accounts\" RESERVEDNAME=\"Sales Accounts\">"+ "\n" +
"      <GUID>df13ea43-535b-411e-a3ae-09a30f52223d-00000017</GUID>"+ "\n" +
"      <PARENT/>"+ "\n" +
"      <GRPDEBITPARENT/>"+ "\n" +
"      <GRPCREDITPARENT/>"+ "\n" +
"      <ISBILLWISEON>No</ISBILLWISEON>"+ "\n" +
"      <ISCOSTCENTRESON>Yes</ISCOSTCENTRESON>"+ "\n" +
"      <ISADDABLE>No</ISADDABLE>"+ "\n" +
"      <ISUPDATINGTARGETID>No</ISUPDATINGTARGETID>"+ "\n" +
"      <ASORIGINAL>Yes</ASORIGINAL>"+ "\n" +
"      <ISSUBLEDGER>No</ISSUBLEDGER>"+ "\n" +
"      <ISREVENUE>Yes</ISREVENUE>"+ "\n" +
"      <AFFECTSGROSSPROFIT>Yes</AFFECTSGROSSPROFIT>"+ "\n" +
"      <ISDEEMEDPOSITIVE>No</ISDEEMEDPOSITIVE>"+ "\n" +
"      <TRACKNEGATIVEBALANCES>No</TRACKNEGATIVEBALANCES>"+ "\n" +
"      <ISCONDENSED>No</ISCONDENSED>"+ "\n" +
"      <AFFECTSSTOCK>Yes</AFFECTSSTOCK>"+ "\n" +
"      <ISGROUPFORLOANRCPT>No</ISGROUPFORLOANRCPT>"+ "\n" +
"      <ISGROUPFORLOANPYMNT>No</ISGROUPFORLOANPYMNT>"+ "\n" +
"      <ISRATEINCLUSIVEVAT>No</ISRATEINCLUSIVEVAT>"+ "\n" +
"      <ISINVDETAILSENABLE>No</ISINVDETAILSENABLE>"+ "\n" +
"      <SORTPOSITION> 230</SORTPOSITION>"+ "\n" +
"      <ALTERID> 24</ALTERID>"+ "\n" +
"      <SERVICETAXDETAILS.LIST>      </SERVICETAXDETAILS.LIST>"+ "\n" +
"      <VATDETAILS.LIST>      </VATDETAILS.LIST>"+ "\n" +
"      <SALESTAXCESSDETAILS.LIST>      </SALESTAXCESSDETAILS.LIST>"+ "\n" +
"      <GSTDETAILS.LIST>      </GSTDETAILS.LIST>"+ "\n" +
"      <LANGUAGENAME.LIST>"+ "\n" +
"       <NAME.LIST TYPE=\"String\">"+ "\n" +
"        <NAME>Sales Accounts</NAME>"+ "\n" +
"       </NAME.LIST>"+ "\n" +
"       <LANGUAGEID> 1033</LANGUAGEID>"+ "\n" +
"      </LANGUAGENAME.LIST>"+ "\n" +
"      <XBRLDETAIL.LIST>      </XBRLDETAIL.LIST>"+ "\n" +
"      <AUDITDETAILS.LIST>      </AUDITDETAILS.LIST>"+ "\n" +
"      <SCHVIDETAILS.LIST>      </SCHVIDETAILS.LIST>"+ "\n" +
"      <EXCISETARIFFDETAILS.LIST>      </EXCISETARIFFDETAILS.LIST>"+ "\n" +
"      <TCSCATEGORYDETAILS.LIST>      </TCSCATEGORYDETAILS.LIST>"+ "\n" +
"      <TDSCATEGORYDETAILS.LIST>      </TDSCATEGORYDETAILS.LIST>"+ "\n" +
"      <GSTCLASSFNIGSTRATES.LIST>      </GSTCLASSFNIGSTRATES.LIST>"+ "\n" +
"      <EXTARIFFDUTYHEADDETAILS.LIST>      </EXTARIFFDUTYHEADDETAILS.LIST>"+ "\n" +
"     </GROUP>"+ "\n" +
"    </TALLYMESSAGE>\n";

        string VoucherSuspenseAccount =
"    <TALLYMESSAGE xmlns:UDF=\"TallyUDF\">"+ "\n" +
"     <GROUP NAME=\"Suspense A/c\" RESERVEDNAME=\"Suspense A/c\">"+ "\n" +
"      <GUID>df13ea43-535b-411e-a3ae-09a30f52223d-00000009</GUID>"+ "\n" +
"      <PARENT/>"+ "\n" +
"      <GRPDEBITPARENT/>"+ "\n" +
"      <GRPCREDITPARENT/>"+ "\n" +
"      <ISBILLWISEON>No</ISBILLWISEON>"+ "\n" +
"      <ISCOSTCENTRESON>No</ISCOSTCENTRESON>"+ "\n" +
"      <ISADDABLE>No</ISADDABLE>"+ "\n" +
"      <ISUPDATINGTARGETID>No</ISUPDATINGTARGETID>"+ "\n" +
"      <ASORIGINAL>Yes</ASORIGINAL>"+ "\n" +
"      <ISSUBLEDGER>No</ISSUBLEDGER>"+ "\n" +
"      <ISREVENUE>No</ISREVENUE>"+ "\n" +
"      <AFFECTSGROSSPROFIT>No</AFFECTSGROSSPROFIT>"+ "\n" +
"      <ISDEEMEDPOSITIVE>No</ISDEEMEDPOSITIVE>"+ "\n" +
"      <TRACKNEGATIVEBALANCES>No</TRACKNEGATIVEBALANCES>"+ "\n" +
"      <ISCONDENSED>No</ISCONDENSED>"+ "\n" +
"      <AFFECTSSTOCK>No</AFFECTSSTOCK>"+ "\n" +
"      <ISGROUPFORLOANRCPT>No</ISGROUPFORLOANRCPT>"+ "\n" +
"      <ISGROUPFORLOANPYMNT>No</ISGROUPFORLOANPYMNT>"+ "\n" +
"      <ISRATEINCLUSIVEVAT>No</ISRATEINCLUSIVEVAT>"+ "\n" +
"      <ISINVDETAILSENABLE>No</ISINVDETAILSENABLE>"+ "\n" +
"      <SORTPOSITION> 90</SORTPOSITION>"+ "\n" +
"      <ALTERID> 10</ALTERID>"+ "\n" +
"      <SERVICETAXDETAILS.LIST>      </SERVICETAXDETAILS.LIST>"+ "\n" +
"      <VATDETAILS.LIST>      </VATDETAILS.LIST>"+ "\n" +
"      <SALESTAXCESSDETAILS.LIST>      </SALESTAXCESSDETAILS.LIST>"+ "\n" +
"      <GSTDETAILS.LIST>      </GSTDETAILS.LIST>"+ "\n" +
"      <LANGUAGENAME.LIST>"+ "\n" +
"       <NAME.LIST TYPE=\"String\">"+ "\n" +
"        <NAME>Suspense A/c</NAME>"+ "\n" +
"       </NAME.LIST>"+ "\n" +
"       <LANGUAGEID> 1033</LANGUAGEID>"+ "\n" +
"      </LANGUAGENAME.LIST>"+ "\n" +
"      <XBRLDETAIL.LIST>      </XBRLDETAIL.LIST>"+ "\n" +
"      <AUDITDETAILS.LIST>      </AUDITDETAILS.LIST>"+ "\n" +
"      <SCHVIDETAILS.LIST>      </SCHVIDETAILS.LIST>"+ "\n" +
"      <EXCISETARIFFDETAILS.LIST>      </EXCISETARIFFDETAILS.LIST>"+ "\n" +
"      <TCSCATEGORYDETAILS.LIST>      </TCSCATEGORYDETAILS.LIST>"+ "\n" +
"      <TDSCATEGORYDETAILS.LIST>      </TDSCATEGORYDETAILS.LIST>"+ "\n" +
"      <GSTCLASSFNIGSTRATES.LIST>      </GSTCLASSFNIGSTRATES.LIST>"+ "\n" +
"      <EXTARIFFDUTYHEADDETAILS.LIST>      </EXTARIFFDUTYHEADDETAILS.LIST>"+ "\n" +
"     </GROUP>"+ "\n" +
"    </TALLYMESSAGE>\n";

        string VoucherBankAccount=
"    <TALLYMESSAGE xmlns:UDF=\"TallyUDF\">"+ "\n" +
"     <GROUP NAME=\"Bank Accounts\" RESERVEDNAME=\"Bank Accounts\">"+ "\n" +
"      <GUID>df13ea43-535b-411e-a3ae-09a30f52223d-00000016</GUID>"+ "\n" +
"      <PARENT>Current Assets</PARENT>"+ "\n" +
"      <GRPDEBITPARENT/>"+ "\n" +
"      <GRPCREDITPARENT/>"+ "\n" +
"      <ISBILLWISEON>No</ISBILLWISEON>"+ "\n" +
"      <ISCOSTCENTRESON>No</ISCOSTCENTRESON>"+ "\n" +
"      <ISADDABLE>No</ISADDABLE>"+ "\n" +
"      <ISUPDATINGTARGETID>No</ISUPDATINGTARGETID>"+ "\n" +
"      <ASORIGINAL>Yes</ASORIGINAL>"+ "\n" +
"      <ISSUBLEDGER>No</ISSUBLEDGER>"+ "\n" +
"      <ISREVENUE>No</ISREVENUE>"+ "\n" +
"      <AFFECTSGROSSPROFIT>No</AFFECTSGROSSPROFIT>"+ "\n" +
"      <ISDEEMEDPOSITIVE>Yes</ISDEEMEDPOSITIVE>"+ "\n" +
"      <TRACKNEGATIVEBALANCES>Yes</TRACKNEGATIVEBALANCES>"+ "\n" +
"      <ISCONDENSED>No</ISCONDENSED>"+ "\n" +
"      <AFFECTSSTOCK>No</AFFECTSSTOCK>"+ "\n" +
"      <ISGROUPFORLOANRCPT>No</ISGROUPFORLOANRCPT>"+ "\n" +
"      <ISGROUPFORLOANPYMNT>No</ISGROUPFORLOANPYMNT>"+ "\n" +
"      <ISRATEINCLUSIVEVAT>No</ISRATEINCLUSIVEVAT>"+ "\n" +
"      <ISINVDETAILSENABLE>No</ISINVDETAILSENABLE>"+ "\n" +
"      <SORTPOSITION> 220</SORTPOSITION>"+ "\n" +
"      <ALTERID> 23</ALTERID>"+ "\n" +
"      <SERVICETAXDETAILS.LIST>      </SERVICETAXDETAILS.LIST>"+ "\n" +
"      <VATDETAILS.LIST>      </VATDETAILS.LIST>"+ "\n" +
"      <SALESTAXCESSDETAILS.LIST>      </SALESTAXCESSDETAILS.LIST>"+ "\n" +
"      <GSTDETAILS.LIST>      </GSTDETAILS.LIST>"+ "\n" +
"      <LANGUAGENAME.LIST>"+ "\n" +
"       <NAME.LIST TYPE=\"String\">"+ "\n" +
"        <NAME>Bank Accounts</NAME>"+ "\n" +
"       </NAME.LIST>"+ "\n" +
"       <LANGUAGEID> 1033</LANGUAGEID>"+ "\n" +
"      </LANGUAGENAME.LIST>"+ "\n" +
"      <XBRLDETAIL.LIST>      </XBRLDETAIL.LIST>"+ "\n" +
"      <AUDITDETAILS.LIST>      </AUDITDETAILS.LIST>"+ "\n" +
"      <SCHVIDETAILS.LIST>      </SCHVIDETAILS.LIST>"+ "\n" +
"      <EXCISETARIFFDETAILS.LIST>      </EXCISETARIFFDETAILS.LIST>"+ "\n" +
"      <TCSCATEGORYDETAILS.LIST>      </TCSCATEGORYDETAILS.LIST>"+ "\n" +
"      <TDSCATEGORYDETAILS.LIST>      </TDSCATEGORYDETAILS.LIST>"+ "\n" +
"      <GSTCLASSFNIGSTRATES.LIST>      </GSTCLASSFNIGSTRATES.LIST>"+ "\n" +
"      <EXTARIFFDUTYHEADDETAILS.LIST>      </EXTARIFFDUTYHEADDETAILS.LIST>"+ "\n" +
"     </GROUP>"+ "\n" +
"    </TALLYMESSAGE>\n";

        string VoucherBankODAccounts =
"    <TALLYMESSAGE xmlns:UDF=\"TallyUDF\">"+ "\n" +
"     <GROUP NAME=\"Bank OD A/c\" RESERVEDNAME=\"Bank OD A/c\">"+ "\n" +
"      <GUID>df13ea43-535b-411e-a3ae-09a30f52223d-0000000b</GUID>"+ "\n" +
"      <PARENT>Loans (Liability)</PARENT>"+ "\n" +
"      <GRPDEBITPARENT/>"+ "\n" +
"      <GRPCREDITPARENT/>"+ "\n" +
"      <ISBILLWISEON>No</ISBILLWISEON>"+ "\n" +
"      <ISCOSTCENTRESON>No</ISCOSTCENTRESON>"+ "\n" +
"      <ISADDABLE>No</ISADDABLE>"+ "\n" +
"      <ISUPDATINGTARGETID>No</ISUPDATINGTARGETID>"+ "\n" +
"      <ASORIGINAL>Yes</ASORIGINAL>"+ "\n" +
"      <ISSUBLEDGER>No</ISSUBLEDGER>"+ "\n" +
"      <ISREVENUE>No</ISREVENUE>"+ "\n" +
"      <AFFECTSGROSSPROFIT>No</AFFECTSGROSSPROFIT>"+ "\n" +
"      <ISDEEMEDPOSITIVE>No</ISDEEMEDPOSITIVE>"+ "\n" +
"      <TRACKNEGATIVEBALANCES>No</TRACKNEGATIVEBALANCES>"+ "\n" +
"      <ISCONDENSED>No</ISCONDENSED>"+ "\n" +
"      <AFFECTSSTOCK>No</AFFECTSSTOCK>"+ "\n" +
"      <ISGROUPFORLOANRCPT>No</ISGROUPFORLOANRCPT>"+ "\n" +
"      <ISGROUPFORLOANPYMNT>No</ISGROUPFORLOANPYMNT>"+ "\n" +
"      <ISRATEINCLUSIVEVAT>No</ISRATEINCLUSIVEVAT>"+ "\n" +
"      <ISINVDETAILSENABLE>No</ISINVDETAILSENABLE>"+ "\n" +
"      <SORTPOSITION> 110</SORTPOSITION>"+ "\n" +
"      <ALTERID> 12</ALTERID>"+ "\n" +
"      <SERVICETAXDETAILS.LIST>      </SERVICETAXDETAILS.LIST>"+ "\n" +
"      <VATDETAILS.LIST>      </VATDETAILS.LIST>"+ "\n" +
"      <SALESTAXCESSDETAILS.LIST>      </SALESTAXCESSDETAILS.LIST>"+ "\n" +
"      <GSTDETAILS.LIST>      </GSTDETAILS.LIST>"+ "\n" +
"      <LANGUAGENAME.LIST>"+ "\n" +
"       <NAME.LIST TYPE=\"String\">"+ "\n" +
"        <NAME>Bank OD A/c</NAME>"+ "\n" +
"        <NAME>Bank OCC A/c</NAME>"+ "\n" +
"       </NAME.LIST>"+ "\n" +
"       <LANGUAGEID> 1033</LANGUAGEID>"+ "\n" +
"      </LANGUAGENAME.LIST>"+ "\n" +
"      <XBRLDETAIL.LIST>      </XBRLDETAIL.LIST>"+ "\n" +
"      <AUDITDETAILS.LIST>      </AUDITDETAILS.LIST>"+ "\n" +
"      <SCHVIDETAILS.LIST>      </SCHVIDETAILS.LIST>"+ "\n" +
"      <EXCISETARIFFDETAILS.LIST>      </EXCISETARIFFDETAILS.LIST>"+ "\n" +
"      <TCSCATEGORYDETAILS.LIST>      </TCSCATEGORYDETAILS.LIST>"+ "\n" +
"      <TDSCATEGORYDETAILS.LIST>      </TDSCATEGORYDETAILS.LIST>"+ "\n" +
"      <GSTCLASSFNIGSTRATES.LIST>      </GSTCLASSFNIGSTRATES.LIST>"+ "\n" +
"      <EXTARIFFDUTYHEADDETAILS.LIST>      </EXTARIFFDUTYHEADDETAILS.LIST>"+ "\n" +
"     </GROUP>"+ "\n" +
"    </TALLYMESSAGE>\n";

        string VoucherCashinHand =
"    <TALLYMESSAGE xmlns:UDF=\"TallyUDF\">"+ "\n" +
"     <GROUP NAME=\"Cash-in-hand\" RESERVEDNAME=\"Cash-in-hand\">"+ "\n" +
"      <GUID>df13ea43-535b-411e-a3ae-09a30f52223d-00000015</GUID>"+ "\n" +
"      <PARENT>Current Assets</PARENT>"+ "\n" +
"      <GRPDEBITPARENT/>"+ "\n" +
"      <GRPCREDITPARENT/>"+ "\n" +
"      <ISBILLWISEON>No</ISBILLWISEON>"+ "\n" +
"      <ISCOSTCENTRESON>No</ISCOSTCENTRESON>"+ "\n" +
"      <ISADDABLE>No</ISADDABLE>"+ "\n" +
"      <ISUPDATINGTARGETID>No</ISUPDATINGTARGETID>"+ "\n" +
"      <ASORIGINAL>Yes</ASORIGINAL>"+ "\n" +
"      <ISSUBLEDGER>No</ISSUBLEDGER>"+ "\n" +
"      <ISREVENUE>No</ISREVENUE>"+ "\n" +
"      <AFFECTSGROSSPROFIT>No</AFFECTSGROSSPROFIT>"+ "\n" +
"      <ISDEEMEDPOSITIVE>Yes</ISDEEMEDPOSITIVE>"+ "\n" +
"      <TRACKNEGATIVEBALANCES>Yes</TRACKNEGATIVEBALANCES>"+ "\n" +
"      <ISCONDENSED>No</ISCONDENSED>"+ "\n" +
"      <AFFECTSSTOCK>No</AFFECTSSTOCK>"+ "\n" +
"      <ISGROUPFORLOANRCPT>No</ISGROUPFORLOANRCPT>"+ "\n" +
"      <ISGROUPFORLOANPYMNT>No</ISGROUPFORLOANPYMNT>"+ "\n" +
"      <ISRATEINCLUSIVEVAT>No</ISRATEINCLUSIVEVAT>"+ "\n" +
"      <ISINVDETAILSENABLE>No</ISINVDETAILSENABLE>"+ "\n" +
"      <SORTPOSITION> 210</SORTPOSITION>"+ "\n" +
"      <ALTERID> 22</ALTERID>"+ "\n" +
"      <SERVICETAXDETAILS.LIST>      </SERVICETAXDETAILS.LIST>"+ "\n" +
"      <VATDETAILS.LIST>      </VATDETAILS.LIST>"+ "\n" +
"      <SALESTAXCESSDETAILS.LIST>      </SALESTAXCESSDETAILS.LIST>"+ "\n" +
"      <GSTDETAILS.LIST>      </GSTDETAILS.LIST>"+ "\n" +
"      <LANGUAGENAME.LIST>"+ "\n" +
"       <NAME.LIST TYPE=\"String\">"+ "\n" +
"        <NAME>Cash-in-hand</NAME>"+ "\n" +
"       </NAME.LIST>"+ "\n" +
"       <LANGUAGEID> 1033</LANGUAGEID>"+ "\n" +
"      </LANGUAGENAME.LIST>"+ "\n" +
"      <XBRLDETAIL.LIST>      </XBRLDETAIL.LIST>"+ "\n" +
"      <AUDITDETAILS.LIST>      </AUDITDETAILS.LIST>"+ "\n" +
"      <SCHVIDETAILS.LIST>      </SCHVIDETAILS.LIST>"+ "\n" +
"      <EXCISETARIFFDETAILS.LIST>      </EXCISETARIFFDETAILS.LIST>"+ "\n" +
"      <TCSCATEGORYDETAILS.LIST>      </TCSCATEGORYDETAILS.LIST>"+ "\n" +
"      <TDSCATEGORYDETAILS.LIST>      </TDSCATEGORYDETAILS.LIST>"+ "\n" +
"      <GSTCLASSFNIGSTRATES.LIST>      </GSTCLASSFNIGSTRATES.LIST>"+ "\n" +
"      <EXTARIFFDUTYHEADDETAILS.LIST>      </EXTARIFFDUTYHEADDETAILS.LIST>"+ "\n" +
"     </GROUP>"+ "\n" +
"    </TALLYMESSAGE>\n";

        string VoucherDeposit =
"    <TALLYMESSAGE xmlns:UDF=\"TallyUDF\">"+ "\n" +
"     <GROUP NAME=\"Deposits (Asset)\" RESERVEDNAME=\"Deposits (Asset)\">"+ "\n" +
"      <GUID>df13ea43-535b-411e-a3ae-09a30f52223d-00000012</GUID>"+ "\n" +
"      <PARENT>Current Assets</PARENT>"+ "\n" +
"      <GRPDEBITPARENT/>"+ "\n" +
"      <GRPCREDITPARENT/>"+ "\n" +
"      <ISBILLWISEON>No</ISBILLWISEON>"+ "\n" +
"      <ISCOSTCENTRESON>No</ISCOSTCENTRESON>"+ "\n" +
"      <ISADDABLE>No</ISADDABLE>"+ "\n" +
"      <ISUPDATINGTARGETID>No</ISUPDATINGTARGETID>"+ "\n" +
"      <ASORIGINAL>Yes</ASORIGINAL>"+ "\n" +
"      <ISSUBLEDGER>No</ISSUBLEDGER>"+ "\n" +
"      <ISREVENUE>No</ISREVENUE>"+ "\n" +
"      <AFFECTSGROSSPROFIT>No</AFFECTSGROSSPROFIT>"+ "\n" +
"      <ISDEEMEDPOSITIVE>Yes</ISDEEMEDPOSITIVE>"+ "\n" +
"      <TRACKNEGATIVEBALANCES>No</TRACKNEGATIVEBALANCES>"+ "\n" +
"      <ISCONDENSED>No</ISCONDENSED>"+ "\n" +
"      <AFFECTSSTOCK>No</AFFECTSSTOCK>"+ "\n" +
"      <ISGROUPFORLOANRCPT>No</ISGROUPFORLOANRCPT>"+ "\n" +
"      <ISGROUPFORLOANPYMNT>No</ISGROUPFORLOANPYMNT>"+ "\n" +
"      <ISRATEINCLUSIVEVAT>No</ISRATEINCLUSIVEVAT>"+ "\n" +
"      <ISINVDETAILSENABLE>No</ISINVDETAILSENABLE>"+ "\n" +
"      <SORTPOSITION> 180</SORTPOSITION>"+ "\n" +
"      <ALTERID> 19</ALTERID>"+ "\n" +
"      <SERVICETAXDETAILS.LIST>      </SERVICETAXDETAILS.LIST>"+ "\n" +
"      <VATDETAILS.LIST>      </VATDETAILS.LIST>"+ "\n" +
"      <SALESTAXCESSDETAILS.LIST>      </SALESTAXCESSDETAILS.LIST>"+ "\n" +
"      <GSTDETAILS.LIST>      </GSTDETAILS.LIST>"+ "\n" +
"      <LANGUAGENAME.LIST>"+ "\n" +
"       <NAME.LIST TYPE=\"String\">"+ "\n" +
"        <NAME>Deposits (Asset)</NAME>"+ "\n" +
"       </NAME.LIST>"+ "\n" +
"       <LANGUAGEID> 1033</LANGUAGEID>"+ "\n" +
"      </LANGUAGENAME.LIST>"+ "\n" +
"      <XBRLDETAIL.LIST>      </XBRLDETAIL.LIST>"+ "\n" +
"      <AUDITDETAILS.LIST>      </AUDITDETAILS.LIST>"+ "\n" +
"      <SCHVIDETAILS.LIST>      </SCHVIDETAILS.LIST>"+ "\n" +
"      <EXCISETARIFFDETAILS.LIST>      </EXCISETARIFFDETAILS.LIST>"+ "\n" +
"      <TCSCATEGORYDETAILS.LIST>      </TCSCATEGORYDETAILS.LIST>"+ "\n" +
"      <TDSCATEGORYDETAILS.LIST>      </TDSCATEGORYDETAILS.LIST>"+ "\n" +
"      <GSTCLASSFNIGSTRATES.LIST>      </GSTCLASSFNIGSTRATES.LIST>"+ "\n" +
"      <EXTARIFFDUTYHEADDETAILS.LIST>      </EXTARIFFDUTYHEADDETAILS.LIST>"+ "\n" +
"     </GROUP>"+ "\n" +
"    </TALLYMESSAGE>\n";

        string VoucherDutiesandTaxes =
"    <TALLYMESSAGE xmlns:UDF=\"TallyUDF\">"+ "\n" +
"     <GROUP NAME=\"Duties &amp; Taxes\" RESERVEDNAME=\"Duties &amp; Taxes\">"+ "\n" +
"      <GUID>df13ea43-535b-411e-a3ae-09a30f52223d-0000000e</GUID>"+ "\n" +
"      <PARENT>Current Liabilities</PARENT>"+ "\n" +
"      <GRPDEBITPARENT/>"+ "\n" +
"      <GRPCREDITPARENT/>"+ "\n" +
"      <ISBILLWISEON>No</ISBILLWISEON>"+ "\n" +
"      <ISCOSTCENTRESON>No</ISCOSTCENTRESON>"+ "\n" +
"      <ISADDABLE>No</ISADDABLE>"+ "\n" +
"      <ISUPDATINGTARGETID>No</ISUPDATINGTARGETID>"+ "\n" +
"      <ASORIGINAL>Yes</ASORIGINAL>"+ "\n" +
"      <ISSUBLEDGER>No</ISSUBLEDGER>"+ "\n" +
"      <ISREVENUE>No</ISREVENUE>"+ "\n" +
"      <AFFECTSGROSSPROFIT>No</AFFECTSGROSSPROFIT>"+ "\n" +
"      <ISDEEMEDPOSITIVE>No</ISDEEMEDPOSITIVE>"+ "\n" +
"      <TRACKNEGATIVEBALANCES>No</TRACKNEGATIVEBALANCES>"+ "\n" +
"      <ISCONDENSED>Yes</ISCONDENSED>"+ "\n" +
"      <AFFECTSSTOCK>No</AFFECTSSTOCK>"+ "\n" +
"      <ISGROUPFORLOANRCPT>No</ISGROUPFORLOANRCPT>"+ "\n" +
"      <ISGROUPFORLOANPYMNT>No</ISGROUPFORLOANPYMNT>"+ "\n" +
"      <ISRATEINCLUSIVEVAT>No</ISRATEINCLUSIVEVAT>"+ "\n" +
"      <ISINVDETAILSENABLE>No</ISINVDETAILSENABLE>"+ "\n" +
"      <SORTPOSITION> 140</SORTPOSITION>"+ "\n" +
"      <ALTERID> 15</ALTERID>"+ "\n" +
"      <SERVICETAXDETAILS.LIST>      </SERVICETAXDETAILS.LIST>"+ "\n" +
"      <VATDETAILS.LIST>      </VATDETAILS.LIST>"+ "\n" +
"      <SALESTAXCESSDETAILS.LIST>      </SALESTAXCESSDETAILS.LIST>"+ "\n" +
"      <GSTDETAILS.LIST>      </GSTDETAILS.LIST>"+ "\n" +
"      <LANGUAGENAME.LIST>"+ "\n" +
"       <NAME.LIST TYPE=\"String\">"+ "\n" +
"        <NAME>Duties &amp; Taxes</NAME>"+ "\n" +
"       </NAME.LIST>"+ "\n" +
"       <LANGUAGEID> 1033</LANGUAGEID>"+ "\n" +
"      </LANGUAGENAME.LIST>"+ "\n" +
"      <XBRLDETAIL.LIST>      </XBRLDETAIL.LIST>"+ "\n" +
"      <AUDITDETAILS.LIST>      </AUDITDETAILS.LIST>"+ "\n" +
"      <SCHVIDETAILS.LIST>      </SCHVIDETAILS.LIST>"+ "\n" +
"      <EXCISETARIFFDETAILS.LIST>      </EXCISETARIFFDETAILS.LIST>"+ "\n" +
"      <TCSCATEGORYDETAILS.LIST>      </TCSCATEGORYDETAILS.LIST>"+ "\n" +
"      <TDSCATEGORYDETAILS.LIST>      </TDSCATEGORYDETAILS.LIST>"+ "\n" +
"      <GSTCLASSFNIGSTRATES.LIST>      </GSTCLASSFNIGSTRATES.LIST>"+ "\n" +
"      <EXTARIFFDUTYHEADDETAILS.LIST>      </EXTARIFFDUTYHEADDETAILS.LIST>"+ "\n" +
"     </GROUP>"+ "\n" +
"    </TALLYMESSAGE>\n";

        string VoucherLoansandAdvances =
"    <TALLYMESSAGE xmlns:UDF=\"TallyUDF\">"+ "\n" +
"     <GROUP NAME=\"Loans &amp; Advances (Asset)\" RESERVEDNAME=\"Loans &amp; Advances (Asset)\">"+ "\n" +
"      <GUID>df13ea43-535b-411e-a3ae-09a30f52223d-00000013</GUID>"+ "\n" +
"      <PARENT>Current Assets</PARENT>"+ "\n" +
"      <GRPDEBITPARENT/>"+ "\n" +
"      <GRPCREDITPARENT/>"+ "\n" +
"      <ISBILLWISEON>No</ISBILLWISEON>"+ "\n" +
"      <ISCOSTCENTRESON>No</ISCOSTCENTRESON>"+ "\n" +
"      <ISADDABLE>No</ISADDABLE>"+ "\n" +
"      <ISUPDATINGTARGETID>No</ISUPDATINGTARGETID>"+ "\n" +
"      <ASORIGINAL>Yes</ASORIGINAL>"+ "\n" +
"      <ISSUBLEDGER>No</ISSUBLEDGER>"+ "\n" +
"      <ISREVENUE>No</ISREVENUE>"+ "\n" +
"      <AFFECTSGROSSPROFIT>No</AFFECTSGROSSPROFIT>"+ "\n" +
"      <ISDEEMEDPOSITIVE>Yes</ISDEEMEDPOSITIVE>"+ "\n" +
"      <TRACKNEGATIVEBALANCES>No</TRACKNEGATIVEBALANCES>"+ "\n" +
"      <ISCONDENSED>No</ISCONDENSED>"+ "\n" +
"      <AFFECTSSTOCK>No</AFFECTSSTOCK>"+ "\n" +
"      <ISGROUPFORLOANRCPT>No</ISGROUPFORLOANRCPT>"+ "\n" +
"      <ISGROUPFORLOANPYMNT>No</ISGROUPFORLOANPYMNT>"+ "\n" +
"      <ISRATEINCLUSIVEVAT>No</ISRATEINCLUSIVEVAT>"+ "\n" +
"      <ISINVDETAILSENABLE>No</ISINVDETAILSENABLE>"+ "\n" +
"      <SORTPOSITION> 190</SORTPOSITION>"+ "\n" +
"      <ALTERID> 20</ALTERID>"+ "\n" +
"      <SERVICETAXDETAILS.LIST>      </SERVICETAXDETAILS.LIST>"+ "\n" +
"      <VATDETAILS.LIST>      </VATDETAILS.LIST>"+ "\n" +
"      <SALESTAXCESSDETAILS.LIST>      </SALESTAXCESSDETAILS.LIST>"+ "\n" +
"      <GSTDETAILS.LIST>      </GSTDETAILS.LIST>"+ "\n" +
"      <LANGUAGENAME.LIST>"+ "\n" +
"       <NAME.LIST TYPE=\"String\">"+ "\n" +
"        <NAME>Loans &amp; Advances (Asset)</NAME>"+ "\n" +
"       </NAME.LIST>"+ "\n" +
"       <LANGUAGEID> 1033</LANGUAGEID>"+ "\n" +
"      </LANGUAGENAME.LIST>"+ "\n" +
"      <XBRLDETAIL.LIST>      </XBRLDETAIL.LIST>"+ "\n" +
"      <AUDITDETAILS.LIST>      </AUDITDETAILS.LIST>"+ "\n" +
"      <SCHVIDETAILS.LIST>      </SCHVIDETAILS.LIST>"+ "\n" +
"      <EXCISETARIFFDETAILS.LIST>      </EXCISETARIFFDETAILS.LIST>"+ "\n" +
"      <TCSCATEGORYDETAILS.LIST>      </TCSCATEGORYDETAILS.LIST>"+ "\n" +
"      <TDSCATEGORYDETAILS.LIST>      </TDSCATEGORYDETAILS.LIST>"+ "\n" +
"      <GSTCLASSFNIGSTRATES.LIST>      </GSTCLASSFNIGSTRATES.LIST>"+ "\n" +
"      <EXTARIFFDUTYHEADDETAILS.LIST>      </EXTARIFFDUTYHEADDETAILS.LIST>"+ "\n" +
"     </GROUP>"+ "\n" +
"    </TALLYMESSAGE>\n";

        string VoucherProvision =
"    <TALLYMESSAGE xmlns:UDF=\"TallyUDF\">"+ "\n" +
"     <GROUP NAME=\"Provisions\" RESERVEDNAME=\"Provisions\">"+ "\n" +
"      <GUID>df13ea43-535b-411e-a3ae-09a30f52223d-0000000f</GUID>"+ "\n" +
"      <PARENT>Current Liabilities</PARENT>"+ "\n" +
"      <GRPDEBITPARENT/>"+ "\n" +
"      <GRPCREDITPARENT/>"+ "\n" +
"      <ISBILLWISEON>No</ISBILLWISEON>"+ "\n" +
"      <ISCOSTCENTRESON>No</ISCOSTCENTRESON>"+ "\n" +
"      <ISADDABLE>No</ISADDABLE>"+ "\n" +
"      <ISUPDATINGTARGETID>No</ISUPDATINGTARGETID>"+ "\n" +
"      <ASORIGINAL>Yes</ASORIGINAL>"+ "\n" +
"      <ISSUBLEDGER>No</ISSUBLEDGER>"+ "\n" +
"      <ISREVENUE>No</ISREVENUE>"+ "\n" +
"      <AFFECTSGROSSPROFIT>No</AFFECTSGROSSPROFIT>"+ "\n" +
"      <ISDEEMEDPOSITIVE>No</ISDEEMEDPOSITIVE>"+ "\n" +
"      <TRACKNEGATIVEBALANCES>No</TRACKNEGATIVEBALANCES>"+ "\n" +
"      <ISCONDENSED>No</ISCONDENSED>"+ "\n" +
"      <AFFECTSSTOCK>No</AFFECTSSTOCK>"+ "\n" +
"      <ISGROUPFORLOANRCPT>No</ISGROUPFORLOANRCPT>"+ "\n" +
"      <ISGROUPFORLOANPYMNT>No</ISGROUPFORLOANPYMNT>"+ "\n" +
"      <ISRATEINCLUSIVEVAT>No</ISRATEINCLUSIVEVAT>"+ "\n" +
"      <ISINVDETAILSENABLE>No</ISINVDETAILSENABLE>"+ "\n" +
"      <SORTPOSITION> 150</SORTPOSITION>"+ "\n" +
"      <ALTERID> 16</ALTERID>"+ "\n" +
"      <SERVICETAXDETAILS.LIST>      </SERVICETAXDETAILS.LIST>"+ "\n" +
"      <VATDETAILS.LIST>      </VATDETAILS.LIST>"+ "\n" +
"      <SALESTAXCESSDETAILS.LIST>      </SALESTAXCESSDETAILS.LIST>"+ "\n" +
"      <GSTDETAILS.LIST>      </GSTDETAILS.LIST>"+ "\n" +
"      <LANGUAGENAME.LIST>"+ "\n" +
"       <NAME.LIST TYPE=\"String\">"+ "\n" +
"        <NAME>Provisions</NAME>"+ "\n" +
"       </NAME.LIST>"+ "\n" +
"       <LANGUAGEID> 1033</LANGUAGEID>"+ "\n" +
"      </LANGUAGENAME.LIST>"+ "\n" +
"      <XBRLDETAIL.LIST>      </XBRLDETAIL.LIST>"+ "\n" +
"      <AUDITDETAILS.LIST>      </AUDITDETAILS.LIST>"+ "\n" +
"      <SCHVIDETAILS.LIST>      </SCHVIDETAILS.LIST>"+ "\n" +
"      <EXCISETARIFFDETAILS.LIST>      </EXCISETARIFFDETAILS.LIST>"+ "\n" +
"      <TCSCATEGORYDETAILS.LIST>      </TCSCATEGORYDETAILS.LIST>"+ "\n" +
"      <TDSCATEGORYDETAILS.LIST>      </TDSCATEGORYDETAILS.LIST>"+ "\n" +
"      <GSTCLASSFNIGSTRATES.LIST>      </GSTCLASSFNIGSTRATES.LIST>"+ "\n" +
"      <EXTARIFFDUTYHEADDETAILS.LIST>      </EXTARIFFDUTYHEADDETAILS.LIST>"+ "\n" +
"     </GROUP>"+ "\n" +
"    </TALLYMESSAGE>\n";

        string VoucherReserveandSurplus=
"    <TALLYMESSAGE xmlns:UDF=\"TallyUDF\">"+ "\n" +
"     <GROUP NAME=\"Reserves &amp; Surplus\" RESERVEDNAME=\"Reserves &amp; Surplus\">"+ "\n" +
"      <GUID>df13ea43-535b-411e-a3ae-09a30f52223d-0000000a</GUID>"+ "\n" +
"      <PARENT>Capital Account</PARENT>"+ "\n" +
"      <GRPDEBITPARENT/>"+ "\n" +
"      <GRPCREDITPARENT/>"+ "\n" +
"      <ISBILLWISEON>No</ISBILLWISEON>"+ "\n" +
"      <ISCOSTCENTRESON>No</ISCOSTCENTRESON>"+ "\n" +
"      <ISADDABLE>No</ISADDABLE>"+ "\n" +
"      <ISUPDATINGTARGETID>No</ISUPDATINGTARGETID>"+ "\n" +
"      <ASORIGINAL>Yes</ASORIGINAL>"+ "\n" +
"      <ISSUBLEDGER>No</ISSUBLEDGER>"+ "\n" +
"      <ISREVENUE>No</ISREVENUE>"+ "\n" +
"      <AFFECTSGROSSPROFIT>No</AFFECTSGROSSPROFIT>"+ "\n" +
"      <ISDEEMEDPOSITIVE>No</ISDEEMEDPOSITIVE>"+ "\n" +
"      <TRACKNEGATIVEBALANCES>No</TRACKNEGATIVEBALANCES>"+ "\n" +
"      <ISCONDENSED>No</ISCONDENSED>"+ "\n" +
"      <AFFECTSSTOCK>No</AFFECTSSTOCK>"+ "\n" +
"      <ISGROUPFORLOANRCPT>No</ISGROUPFORLOANRCPT>"+ "\n" +
"      <ISGROUPFORLOANPYMNT>No</ISGROUPFORLOANPYMNT>"+ "\n" +
"      <ISRATEINCLUSIVEVAT>No</ISRATEINCLUSIVEVAT>"+ "\n" +
"      <ISINVDETAILSENABLE>No</ISINVDETAILSENABLE>"+ "\n" +
"      <SORTPOSITION> 100</SORTPOSITION>"+ "\n" +
"      <ALTERID> 11</ALTERID>"+ "\n" +
"      <SERVICETAXDETAILS.LIST>      </SERVICETAXDETAILS.LIST>"+ "\n" +
"      <VATDETAILS.LIST>      </VATDETAILS.LIST>"+ "\n" +
"      <SALESTAXCESSDETAILS.LIST>      </SALESTAXCESSDETAILS.LIST>"+ "\n" +
"      <GSTDETAILS.LIST>      </GSTDETAILS.LIST>"+ "\n" +
"      <LANGUAGENAME.LIST>"+ "\n" +
"       <NAME.LIST TYPE=\"String\">"+ "\n" +
"        <NAME>Reserves &amp; Surplus</NAME>"+ "\n" +
"        <NAME>Retained Earnings</NAME>"+ "\n" +
"       </NAME.LIST>"+ "\n" +
"       <LANGUAGEID> 1033</LANGUAGEID>"+ "\n" +
"      </LANGUAGENAME.LIST>"+ "\n" +
"      <XBRLDETAIL.LIST>      </XBRLDETAIL.LIST>"+ "\n" +
"      <AUDITDETAILS.LIST>      </AUDITDETAILS.LIST>"+ "\n" +
"      <SCHVIDETAILS.LIST>      </SCHVIDETAILS.LIST>"+ "\n" +
"      <EXCISETARIFFDETAILS.LIST>      </EXCISETARIFFDETAILS.LIST>"+ "\n" +
"      <TCSCATEGORYDETAILS.LIST>      </TCSCATEGORYDETAILS.LIST>"+ "\n" +
"      <TDSCATEGORYDETAILS.LIST>      </TDSCATEGORYDETAILS.LIST>"+ "\n" +
"      <GSTCLASSFNIGSTRATES.LIST>      </GSTCLASSFNIGSTRATES.LIST>"+ "\n" +
"      <EXTARIFFDUTYHEADDETAILS.LIST>      </EXTARIFFDUTYHEADDETAILS.LIST>"+ "\n" +
"     </GROUP>"+ "\n" +
"    </TALLYMESSAGE>\n";
        
        string VoucherSecruedLoans = 
"   <TALLYMESSAGE xmlns:UDF=\"TallyUDF\">"+ "\n" +
"     <GROUP NAME=\"Secured Loans\" RESERVEDNAME=\"Secured Loans\">"+ "\n" +
"      <GUID>df13ea43-535b-411e-a3ae-09a30f52223d-0000000c</GUID>"+ "\n" +
"      <PARENT>Loans (Liability)</PARENT>"+ "\n" +
"      <GRPDEBITPARENT/>"+ "\n" +
"      <GRPCREDITPARENT/>"+ "\n" +
"      <ISBILLWISEON>No</ISBILLWISEON>"+ "\n" +
"      <ISCOSTCENTRESON>No</ISCOSTCENTRESON>"+ "\n" +
"      <ISADDABLE>No</ISADDABLE>"+ "\n" +
"      <ISUPDATINGTARGETID>No</ISUPDATINGTARGETID>"+ "\n" +
"      <ASORIGINAL>Yes</ASORIGINAL>"+ "\n" +
"      <ISSUBLEDGER>No</ISSUBLEDGER>"+ "\n" +
"      <ISREVENUE>No</ISREVENUE>"+ "\n" +
"      <AFFECTSGROSSPROFIT>No</AFFECTSGROSSPROFIT>"+ "\n" +
"      <ISDEEMEDPOSITIVE>No</ISDEEMEDPOSITIVE>"+ "\n" +
"      <TRACKNEGATIVEBALANCES>No</TRACKNEGATIVEBALANCES>"+ "\n" +
"      <ISCONDENSED>No</ISCONDENSED>"+ "\n" +
"      <AFFECTSSTOCK>No</AFFECTSSTOCK>"+ "\n" +
"      <ISGROUPFORLOANRCPT>No</ISGROUPFORLOANRCPT>"+ "\n" +
"      <ISGROUPFORLOANPYMNT>No</ISGROUPFORLOANPYMNT>"+ "\n" +
"      <ISRATEINCLUSIVEVAT>No</ISRATEINCLUSIVEVAT>"+ "\n" +
"      <ISINVDETAILSENABLE>No</ISINVDETAILSENABLE>"+ "\n" +
"      <SORTPOSITION> 120</SORTPOSITION>"+ "\n" +
"      <ALTERID> 13</ALTERID>"+ "\n" +
"      <SERVICETAXDETAILS.LIST>      </SERVICETAXDETAILS.LIST>"+ "\n" +
"      <VATDETAILS.LIST>      </VATDETAILS.LIST>"+ "\n" +
"      <SALESTAXCESSDETAILS.LIST>      </SALESTAXCESSDETAILS.LIST>"+ "\n" +
"      <GSTDETAILS.LIST>      </GSTDETAILS.LIST>"+ "\n" +
"      <LANGUAGENAME.LIST>"+ "\n" +
"       <NAME.LIST TYPE=\"String\">"+ "\n" +
"        <NAME>Secured Loans</NAME>"+ "\n" +
"       </NAME.LIST>"+ "\n" +
"       <LANGUAGEID> 1033</LANGUAGEID>"+ "\n" +
"      </LANGUAGENAME.LIST>"+ "\n" +
"      <XBRLDETAIL.LIST>      </XBRLDETAIL.LIST>"+ "\n" +
"      <AUDITDETAILS.LIST>      </AUDITDETAILS.LIST>"+ "\n" +
"      <SCHVIDETAILS.LIST>      </SCHVIDETAILS.LIST>"+ "\n" +
"      <EXCISETARIFFDETAILS.LIST>      </EXCISETARIFFDETAILS.LIST>"+ "\n" +
"      <TCSCATEGORYDETAILS.LIST>      </TCSCATEGORYDETAILS.LIST>"+ "\n" +
"      <TDSCATEGORYDETAILS.LIST>      </TDSCATEGORYDETAILS.LIST>"+ "\n" +
"      <GSTCLASSFNIGSTRATES.LIST>      </GSTCLASSFNIGSTRATES.LIST>"+ "\n" +
"      <EXTARIFFDUTYHEADDETAILS.LIST>      </EXTARIFFDUTYHEADDETAILS.LIST>"+ "\n" +
"     </GROUP>"+ "\n" +
"    </TALLYMESSAGE>\n";

        string VoucherStockinHand =
"    <TALLYMESSAGE xmlns:UDF=\"TallyUDF\">"+ "\n" +
"     <GROUP NAME=\"Stock-in-hand\" RESERVEDNAME=\"Stock-in-hand\">"+ "\n" +
"      <GUID>df13ea43-535b-411e-a3ae-09a30f52223d-00000011</GUID>"+ "\n" +
"      <PARENT>Current Assets</PARENT>"+ "\n" +
"      <GRPDEBITPARENT/>"+ "\n" +
"      <GRPCREDITPARENT/>"+ "\n" +
"      <ISBILLWISEON>No</ISBILLWISEON>"+ "\n" +
"      <ISCOSTCENTRESON>No</ISCOSTCENTRESON>"+ "\n" +
"      <ISADDABLE>No</ISADDABLE>"+ "\n" +
"      <ISUPDATINGTARGETID>No</ISUPDATINGTARGETID>"+ "\n" +
"      <ASORIGINAL>Yes</ASORIGINAL>"+ "\n" +
"      <ISSUBLEDGER>No</ISSUBLEDGER>"+ "\n" +
"      <ISREVENUE>No</ISREVENUE>"+ "\n" +
"      <AFFECTSGROSSPROFIT>No</AFFECTSGROSSPROFIT>"+ "\n" +
"      <ISDEEMEDPOSITIVE>Yes</ISDEEMEDPOSITIVE>"+ "\n" +
"      <TRACKNEGATIVEBALANCES>No</TRACKNEGATIVEBALANCES>"+ "\n" +
"      <ISCONDENSED>No</ISCONDENSED>"+ "\n" +
"      <AFFECTSSTOCK>No</AFFECTSSTOCK>"+ "\n" +
"      <ISGROUPFORLOANRCPT>No</ISGROUPFORLOANRCPT>"+ "\n" +
"      <ISGROUPFORLOANPYMNT>No</ISGROUPFORLOANPYMNT>"+ "\n" +
"      <ISRATEINCLUSIVEVAT>No</ISRATEINCLUSIVEVAT>"+ "\n" +
"      <ISINVDETAILSENABLE>No</ISINVDETAILSENABLE>"+ "\n" +
"      <SORTPOSITION> 170</SORTPOSITION>"+ "\n" +
"      <ALTERID> 18</ALTERID>"+ "\n" +
"      <SERVICETAXDETAILS.LIST>      </SERVICETAXDETAILS.LIST>"+ "\n" +
"      <VATDETAILS.LIST>      </VATDETAILS.LIST>"+ "\n" +
"      <SALESTAXCESSDETAILS.LIST>      </SALESTAXCESSDETAILS.LIST>"+ "\n" +
"      <GSTDETAILS.LIST>      </GSTDETAILS.LIST>"+ "\n" +
"      <LANGUAGENAME.LIST>"+ "\n" +
"       <NAME.LIST TYPE=\"String\">"+ "\n" +
"        <NAME>Stock-in-hand</NAME>"+ "\n" +
"       </NAME.LIST>"+ "\n" +
"       <LANGUAGEID> 1033</LANGUAGEID>"+ "\n" +
"      </LANGUAGENAME.LIST>"+ "\n" +
"      <XBRLDETAIL.LIST>      </XBRLDETAIL.LIST>"+ "\n" +
"      <AUDITDETAILS.LIST>      </AUDITDETAILS.LIST>"+ "\n" +
"      <SCHVIDETAILS.LIST>      </SCHVIDETAILS.LIST>"+ "\n" +
"      <EXCISETARIFFDETAILS.LIST>      </EXCISETARIFFDETAILS.LIST>"+ "\n" +
"      <TCSCATEGORYDETAILS.LIST>      </TCSCATEGORYDETAILS.LIST>"+ "\n" +
"      <TDSCATEGORYDETAILS.LIST>      </TDSCATEGORYDETAILS.LIST>"+ "\n" +
"      <GSTCLASSFNIGSTRATES.LIST>      </GSTCLASSFNIGSTRATES.LIST>"+ "\n" +
"      <EXTARIFFDUTYHEADDETAILS.LIST>      </EXTARIFFDUTYHEADDETAILS.LIST>"+ "\n" +
"     </GROUP>"+ "\n" +
"    </TALLYMESSAGE>\n";

        string VoucherSundryCreditors =
"    <TALLYMESSAGE xmlns:UDF=\"TallyUDF\">"+ "\n" +
"     <GROUP NAME=\"Sundry Creditors\" RESERVEDNAME=\"Sundry Creditors\">"+ "\n" +
"      <GUID>df13ea43-535b-411e-a3ae-09a30f52223d-00000010</GUID>"+ "\n" +
"      <PARENT>Current Liabilities</PARENT>"+ "\n" +
"      <GRPDEBITPARENT/>"+ "\n" +
"      <GRPCREDITPARENT/>"+ "\n" +
"      <ISBILLWISEON>Yes</ISBILLWISEON>"+ "\n" +
"      <ISCOSTCENTRESON>No</ISCOSTCENTRESON>"+ "\n" +
"      <ISADDABLE>No</ISADDABLE>"+ "\n" +
"      <ISUPDATINGTARGETID>No</ISUPDATINGTARGETID>"+ "\n" +
"      <ASORIGINAL>Yes</ASORIGINAL>"+ "\n" +
"      <ISSUBLEDGER>Yes</ISSUBLEDGER>"+ "\n" +
"      <ISREVENUE>No</ISREVENUE>"+ "\n" +
"      <AFFECTSGROSSPROFIT>No</AFFECTSGROSSPROFIT>"+ "\n" +
"      <ISDEEMEDPOSITIVE>No</ISDEEMEDPOSITIVE>"+ "\n" +
"      <TRACKNEGATIVEBALANCES>No</TRACKNEGATIVEBALANCES>"+ "\n" +
"      <ISCONDENSED>No</ISCONDENSED>"+ "\n" +
"      <AFFECTSSTOCK>No</AFFECTSSTOCK>"+ "\n" +
"      <ISGROUPFORLOANRCPT>No</ISGROUPFORLOANRCPT>"+ "\n" +
"      <ISGROUPFORLOANPYMNT>No</ISGROUPFORLOANPYMNT>"+ "\n" +
"      <ISRATEINCLUSIVEVAT>No</ISRATEINCLUSIVEVAT>"+ "\n" +
"      <ISINVDETAILSENABLE>No</ISINVDETAILSENABLE>"+ "\n" +
"      <SORTPOSITION> 160</SORTPOSITION>"+ "\n" +
"      <ALTERID> 17</ALTERID>"+ "\n" +
"      <SERVICETAXDETAILS.LIST>      </SERVICETAXDETAILS.LIST>"+ "\n" +
"      <VATDETAILS.LIST>      </VATDETAILS.LIST>"+ "\n" +
"      <SALESTAXCESSDETAILS.LIST>      </SALESTAXCESSDETAILS.LIST>"+ "\n" +
"      <GSTDETAILS.LIST>      </GSTDETAILS.LIST>"+ "\n" +
"      <LANGUAGENAME.LIST>"+ "\n" +
"       <NAME.LIST TYPE=\"String\">"+ "\n" +
"        <NAME>Sundry Creditors</NAME>"+ "\n" +
"       </NAME.LIST>"+ "\n" +
"       <LANGUAGEID> 1033</LANGUAGEID>"+ "\n" +
"      </LANGUAGENAME.LIST>"+ "\n" +
"      <XBRLDETAIL.LIST>      </XBRLDETAIL.LIST>"+ "\n" +
"      <AUDITDETAILS.LIST>      </AUDITDETAILS.LIST>"+ "\n" +
"      <SCHVIDETAILS.LIST>      </SCHVIDETAILS.LIST>"+ "\n" +
"      <EXCISETARIFFDETAILS.LIST>      </EXCISETARIFFDETAILS.LIST>"+ "\n" +
"      <TCSCATEGORYDETAILS.LIST>      </TCSCATEGORYDETAILS.LIST>"+ "\n" +
"      <TDSCATEGORYDETAILS.LIST>      </TDSCATEGORYDETAILS.LIST>"+ "\n" +
"      <GSTCLASSFNIGSTRATES.LIST>      </GSTCLASSFNIGSTRATES.LIST>"+ "\n" +
"      <EXTARIFFDUTYHEADDETAILS.LIST>      </EXTARIFFDUTYHEADDETAILS.LIST>"+ "\n" +
"     </GROUP>"+ "\n" +
"    </TALLYMESSAGE>\n";

        string VoucherSundryDebtors =
"    <TALLYMESSAGE xmlns:UDF=\"TallyUDF\">"+ "\n" +
"     <GROUP NAME=\"Sundry Debtors\" RESERVEDNAME=\"Sundry Debtors\">"+ "\n" +
"      <GUID>df13ea43-535b-411e-a3ae-09a30f52223d-00000014</GUID>"+ "\n" +
"      <PARENT>Current Assets</PARENT>"+ "\n" +
"      <GRPDEBITPARENT/>"+ "\n" +
"      <GRPCREDITPARENT/>"+ "\n" +
"      <ISBILLWISEON>Yes</ISBILLWISEON>"+ "\n" +
"      <ISCOSTCENTRESON>No</ISCOSTCENTRESON>"+ "\n" +
"      <ISADDABLE>No</ISADDABLE>"+ "\n" +
"      <ISUPDATINGTARGETID>No</ISUPDATINGTARGETID>"+ "\n" +
"      <ASORIGINAL>Yes</ASORIGINAL>"+ "\n" +
"      <ISSUBLEDGER>Yes</ISSUBLEDGER>"+ "\n" +
"      <ISREVENUE>No</ISREVENUE>"+ "\n" +
"      <AFFECTSGROSSPROFIT>No</AFFECTSGROSSPROFIT>"+ "\n" +
"      <ISDEEMEDPOSITIVE>Yes</ISDEEMEDPOSITIVE>"+ "\n" +
"      <TRACKNEGATIVEBALANCES>No</TRACKNEGATIVEBALANCES>"+ "\n" +
"      <ISCONDENSED>No</ISCONDENSED>"+ "\n" +
"      <AFFECTSSTOCK>No</AFFECTSSTOCK>"+ "\n" +
"      <ISGROUPFORLOANRCPT>No</ISGROUPFORLOANRCPT>"+ "\n" +
"      <ISGROUPFORLOANPYMNT>No</ISGROUPFORLOANPYMNT>"+ "\n" +
"      <ISRATEINCLUSIVEVAT>No</ISRATEINCLUSIVEVAT>"+ "\n" +
"      <ISINVDETAILSENABLE>No</ISINVDETAILSENABLE>"+ "\n" +
"      <SORTPOSITION> 200</SORTPOSITION>"+ "\n" +
"      <ALTERID> 21</ALTERID>"+ "\n" +
"      <SERVICETAXDETAILS.LIST>      </SERVICETAXDETAILS.LIST>"+ "\n" +
"      <VATDETAILS.LIST>      </VATDETAILS.LIST>"+ "\n" +
"      <SALESTAXCESSDETAILS.LIST>      </SALESTAXCESSDETAILS.LIST>"+ "\n" +
"      <GSTDETAILS.LIST>      </GSTDETAILS.LIST>"+ "\n" +
"      <LANGUAGENAME.LIST>"+ "\n" +
"       <NAME.LIST TYPE=\"String\">"+ "\n" +
"        <NAME>Sundry Debtors</NAME>"+ "\n" +
"       </NAME.LIST>"+ "\n" +
"       <LANGUAGEID> 1033</LANGUAGEID>"+ "\n" +
"      </LANGUAGENAME.LIST>"+ "\n" +
"      <XBRLDETAIL.LIST>      </XBRLDETAIL.LIST>"+ "\n" +
"      <AUDITDETAILS.LIST>      </AUDITDETAILS.LIST>"+ "\n" +
"      <SCHVIDETAILS.LIST>      </SCHVIDETAILS.LIST>"+ "\n" +
"      <EXCISETARIFFDETAILS.LIST>      </EXCISETARIFFDETAILS.LIST>"+ "\n" +
"      <TCSCATEGORYDETAILS.LIST>      </TCSCATEGORYDETAILS.LIST>"+ "\n" +
"      <TDSCATEGORYDETAILS.LIST>      </TDSCATEGORYDETAILS.LIST>"+ "\n" +
"      <GSTCLASSFNIGSTRATES.LIST>      </GSTCLASSFNIGSTRATES.LIST>"+ "\n" +
"      <EXTARIFFDUTYHEADDETAILS.LIST>      </EXTARIFFDUTYHEADDETAILS.LIST>"+ "\n" +
"     </GROUP>"+ "\n" +
"    </TALLYMESSAGE>\n";

        string VoucherUnSecuredLoans =
"    <TALLYMESSAGE xmlns:UDF=\"TallyUDF\">"+ "\n" +
"     <GROUP NAME=\"Unsecured Loans\" RESERVEDNAME=\"Unsecured Loans\">"+ "\n" +
"      <GUID>df13ea43-535b-411e-a3ae-09a30f52223d-0000000d</GUID>"+ "\n" +
"      <PARENT>Loans (Liability)</PARENT>"+ "\n" +
"      <GRPDEBITPARENT/>"+ "\n" +
"      <GRPCREDITPARENT/>"+ "\n" +
"      <ISBILLWISEON>No</ISBILLWISEON>"+ "\n" +
"      <ISCOSTCENTRESON>No</ISCOSTCENTRESON>"+ "\n" +
"      <ISADDABLE>No</ISADDABLE>"+ "\n" +
"      <ISUPDATINGTARGETID>No</ISUPDATINGTARGETID>"+ "\n" +
"      <ASORIGINAL>Yes</ASORIGINAL>"+ "\n" +
"      <ISSUBLEDGER>No</ISSUBLEDGER>"+ "\n" +
"      <ISREVENUE>No</ISREVENUE>"+ "\n" +
"      <AFFECTSGROSSPROFIT>No</AFFECTSGROSSPROFIT>"+ "\n" +
"      <ISDEEMEDPOSITIVE>No</ISDEEMEDPOSITIVE>"+ "\n" +
"      <TRACKNEGATIVEBALANCES>No</TRACKNEGATIVEBALANCES>"+ "\n" +
"      <ISCONDENSED>No</ISCONDENSED>"+ "\n" +
"      <AFFECTSSTOCK>No</AFFECTSSTOCK>"+ "\n" +
"      <ISGROUPFORLOANRCPT>No</ISGROUPFORLOANRCPT>"+ "\n" +
"      <ISGROUPFORLOANPYMNT>No</ISGROUPFORLOANPYMNT>"+ "\n" +
"      <ISRATEINCLUSIVEVAT>No</ISRATEINCLUSIVEVAT>"+ "\n" +
"      <ISINVDETAILSENABLE>No</ISINVDETAILSENABLE>"+ "\n" +
"      <SORTPOSITION> 130</SORTPOSITION>"+ "\n" +
"      <ALTERID> 14</ALTERID>"+ "\n" +
"      <SERVICETAXDETAILS.LIST>      </SERVICETAXDETAILS.LIST>"+ "\n" +
"      <VATDETAILS.LIST>      </VATDETAILS.LIST>"+ "\n" +
"      <SALESTAXCESSDETAILS.LIST>      </SALESTAXCESSDETAILS.LIST>"+ "\n" +
"      <GSTDETAILS.LIST>      </GSTDETAILS.LIST>"+ "\n" +
"      <LANGUAGENAME.LIST>"+ "\n" +
"       <NAME.LIST TYPE=\"String\">"+ "\n" +
"        <NAME>Unsecured Loans</NAME>"+ "\n" +
"       </NAME.LIST>"+ "\n" +
"       <LANGUAGEID> 1033</LANGUAGEID>"+ "\n" +
"      </LANGUAGENAME.LIST>"+ "\n" +
"      <XBRLDETAIL.LIST>      </XBRLDETAIL.LIST>"+ "\n" +
"      <AUDITDETAILS.LIST>      </AUDITDETAILS.LIST>"+ "\n" +
"      <SCHVIDETAILS.LIST>      </SCHVIDETAILS.LIST>"+ "\n" +
"      <EXCISETARIFFDETAILS.LIST>      </EXCISETARIFFDETAILS.LIST>"+ "\n" +
"      <TCSCATEGORYDETAILS.LIST>      </TCSCATEGORYDETAILS.LIST>"+ "\n" +
"      <TDSCATEGORYDETAILS.LIST>      </TDSCATEGORYDETAILS.LIST>"+ "\n" +
"      <GSTCLASSFNIGSTRATES.LIST>      </GSTCLASSFNIGSTRATES.LIST>"+ "\n" +
"      <EXTARIFFDUTYHEADDETAILS.LIST>      </EXTARIFFDUTYHEADDETAILS.LIST>"+ "\n" +
"     </GROUP>"+ "\n" +
"    </TALLYMESSAGE>\n";

        string VoucherProfitandLoss =
"    <TALLYMESSAGE xmlns:UDF=\"TallyUDF\">"+ "\n" +
"     <LEDGER NAME=\"Profit &amp; Loss A/c\" RESERVEDNAME=\"Profit &amp; Loss A/c\">"+ "\n" +
"      <GUID>df13ea43-535b-411e-a3ae-09a30f52223d-0000001e</GUID>"+ "\n" +
"      <PARENT/>"+ "\n" +
"      <TAXCLASSIFICATIONNAME/>"+ "\n" +
"      <TAXTYPE>Others</TAXTYPE>"+ "\n" +
"      <GSTTYPE/>"+ "\n" +
"      <APPROPRIATEFOR/>"+ "\n" +
"      <SERVICECATEGORY>&#4; Not Applicable</SERVICECATEGORY>"+ "\n" +
"      <EXCISELEDGERCLASSIFICATION/>"+ "\n" +
"      <EXCISEDUTYTYPE/>"+ "\n" +
"      <EXCISENATUREOFPURCHASE/>"+ "\n" +
"      <LEDGERFBTCATEGORY/>"+ "\n" +
"      <ISBILLWISEON>No</ISBILLWISEON>"+ "\n" +
"      <ISCOSTCENTRESON>No</ISCOSTCENTRESON>"+ "\n" +
"      <ISINTERESTON>No</ISINTERESTON>"+ "\n" +
"      <ALLOWINMOBILE>No</ALLOWINMOBILE>"+ "\n" +
"      <ISCOSTTRACKINGON>No</ISCOSTTRACKINGON>"+ "\n" +
"      <ISBENEFICIARYCODEON>No</ISBENEFICIARYCODEON>"+ "\n" +
"      <ISUPDATINGTARGETID>No</ISUPDATINGTARGETID>"+ "\n" +
"      <ASORIGINAL>Yes</ASORIGINAL>"+ "\n" +
"      <ISCONDENSED>No</ISCONDENSED>"+ "\n" +
"      <AFFECTSSTOCK>No</AFFECTSSTOCK>"+ "\n" +
"      <ISRATEINCLUSIVEVAT>No</ISRATEINCLUSIVEVAT>"+ "\n" +
"      <FORPAYROLL>No</FORPAYROLL>"+ "\n" +
"      <ISABCENABLED>No</ISABCENABLED>"+ "\n" +
"      <ISCREDITDAYSCHKON>No</ISCREDITDAYSCHKON>"+ "\n" +
"      <INTERESTONBILLWISE>No</INTERESTONBILLWISE>"+ "\n" +
"      <OVERRIDEINTEREST>No</OVERRIDEINTEREST>"+ "\n" +
"      <OVERRIDEADVINTEREST>No</OVERRIDEADVINTEREST>"+ "\n" +
"      <USEFORVAT>No</USEFORVAT>"+ "\n" +
"      <IGNORETDSEXEMPT>No</IGNORETDSEXEMPT>"+ "\n" +
"      <ISTCSAPPLICABLE>No</ISTCSAPPLICABLE>"+ "\n" +
"      <ISTDSAPPLICABLE>No</ISTDSAPPLICABLE>"+ "\n" +
"      <ISFBTAPPLICABLE>No</ISFBTAPPLICABLE>"+ "\n" +
"      <ISGSTAPPLICABLE>No</ISGSTAPPLICABLE>"+ "\n" +
"      <ISEXCISEAPPLICABLE>No</ISEXCISEAPPLICABLE>"+ "\n" +
"      <ISTDSEXPENSE>No</ISTDSEXPENSE>"+ "\n" +
"      <ISEDLIAPPLICABLE>No</ISEDLIAPPLICABLE>"+ "\n" +
"      <ISRELATEDPARTY>No</ISRELATEDPARTY>"+ "\n" +
"      <USEFORESIELIGIBILITY>No</USEFORESIELIGIBILITY>"+ "\n" +
"      <ISINTERESTINCLLASTDAY>No</ISINTERESTINCLLASTDAY>"+ "\n" +
"      <APPROPRIATETAXVALUE>No</APPROPRIATETAXVALUE>"+ "\n" +
"      <ISBEHAVEASDUTY>No</ISBEHAVEASDUTY>"+ "\n" +
"      <INTERESTINCLDAYOFADDITION>No</INTERESTINCLDAYOFADDITION>"+ "\n" +
"      <INTERESTINCLDAYOFDEDUCTION>No</INTERESTINCLDAYOFDEDUCTION>"+ "\n" +
"      <OVERRIDECREDITLIMIT>No</OVERRIDECREDITLIMIT>"+ "\n" +
"      <ISAGAINSTFORMC>No</ISAGAINSTFORMC>"+ "\n" +
"      <ISCHEQUEPRINTINGENABLED>No</ISCHEQUEPRINTINGENABLED>"+ "\n" +
"      <ISPAYUPLOAD>No</ISPAYUPLOAD>"+ "\n" +
"      <ISPAYBATCHONLYSAL>No</ISPAYBATCHONLYSAL>"+ "\n" +
"      <ISBNFCODESUPPORTED>No</ISBNFCODESUPPORTED>"+ "\n" +
"      <ALLOWEXPORTWITHERRORS>No</ALLOWEXPORTWITHERRORS>"+ "\n" +
"      <USEFORNOTIONALITC>No</USEFORNOTIONALITC>"+ "\n" +
"      <ISECOMMOPERATOR>No</ISECOMMOPERATOR>"+ "\n" +
"      <SHOWINPAYSLIP>No</SHOWINPAYSLIP>"+ "\n" +
"      <USEFORGRATUITY>No</USEFORGRATUITY>"+ "\n" +
"      <ISTDSPROJECTED>No</ISTDSPROJECTED>"+ "\n" +
"      <FORSERVICETAX>No</FORSERVICETAX>"+ "\n" +
"      <ISINPUTCREDIT>No</ISINPUTCREDIT>"+ "\n" +
"      <ISEXEMPTED>No</ISEXEMPTED>"+ "\n" +
"      <ISABATEMENTAPPLICABLE>No</ISABATEMENTAPPLICABLE>"+ "\n" +
"      <ISSTXPARTY>No</ISSTXPARTY>"+ "\n" +
"      <ISSTXNONREALIZEDTYPE>No</ISSTXNONREALIZEDTYPE>"+ "\n" +
"      <ISUSEDFORCVD>No</ISUSEDFORCVD>"+ "\n" +
"      <LEDBELONGSTONONTAXABLE>No</LEDBELONGSTONONTAXABLE>"+ "\n" +
"      <ISEXCISEMERCHANTEXPORTER>No</ISEXCISEMERCHANTEXPORTER>"+ "\n" +
"      <ISPARTYEXEMPTED>No</ISPARTYEXEMPTED>"+ "\n" +
"      <ISSEZPARTY>No</ISSEZPARTY>"+ "\n" +
"      <TDSDEDUCTEEISSPECIALRATE>No</TDSDEDUCTEEISSPECIALRATE>"+ "\n" +
"      <ISECHEQUESUPPORTED>No</ISECHEQUESUPPORTED>"+ "\n" +
"      <ISEDDSUPPORTED>No</ISEDDSUPPORTED>"+ "\n" +
"      <HASECHEQUEDELIVERYMODE>No</HASECHEQUEDELIVERYMODE>"+ "\n" +
"      <HASECHEQUEDELIVERYTO>No</HASECHEQUEDELIVERYTO>"+ "\n" +
"      <HASECHEQUEPRINTLOCATION>No</HASECHEQUEPRINTLOCATION>"+ "\n" +
"      <HASECHEQUEPAYABLELOCATION>No</HASECHEQUEPAYABLELOCATION>"+ "\n" +
"      <HASECHEQUEBANKLOCATION>No</HASECHEQUEBANKLOCATION>"+ "\n" +
"      <HASEDDDELIVERYMODE>No</HASEDDDELIVERYMODE>"+ "\n" +
"      <HASEDDDELIVERYTO>No</HASEDDDELIVERYTO>"+ "\n" +
"      <HASEDDPRINTLOCATION>No</HASEDDPRINTLOCATION>"+ "\n" +
"      <HASEDDPAYABLELOCATION>No</HASEDDPAYABLELOCATION>"+ "\n" +
"      <HASEDDBANKLOCATION>No</HASEDDBANKLOCATION>"+ "\n" +
"      <ISEBANKINGENABLED>No</ISEBANKINGENABLED>"+ "\n" +
"      <ISEXPORTFILEENCRYPTED>No</ISEXPORTFILEENCRYPTED>"+ "\n" +
"      <ISBATCHENABLED>No</ISBATCHENABLED>"+ "\n" +
"      <ISPRODUCTCODEBASED>No</ISPRODUCTCODEBASED>"+ "\n" +
"      <HASEDDCITY>No</HASEDDCITY>"+ "\n" +
"      <HASECHEQUECITY>No</HASECHEQUECITY>"+ "\n" +
"      <ISFILENAMEFORMATSUPPORTED>No</ISFILENAMEFORMATSUPPORTED>"+ "\n" +
"      <HASCLIENTCODE>No</HASCLIENTCODE>"+ "\n" +
"      <PAYINSISBATCHAPPLICABLE>No</PAYINSISBATCHAPPLICABLE>"+ "\n" +
"      <PAYINSISFILENUMAPP>No</PAYINSISFILENUMAPP>"+ "\n" +
"      <ISSALARYTRANSGROUPEDFORBRS>No</ISSALARYTRANSGROUPEDFORBRS>"+ "\n" +
"      <ISEBANKINGSUPPORTED>No</ISEBANKINGSUPPORTED>"+ "\n" +
"      <ISSCBUAE>No</ISSCBUAE>"+ "\n" +
"      <ISBANKSTATUSAPP>No</ISBANKSTATUSAPP>"+ "\n" +
"      <ISSALARYGROUPED>No</ISSALARYGROUPED>"+ "\n" +
"      <USEFORPURCHASETAX>No</USEFORPURCHASETAX>"+ "\n" +
"      <AUDITED>No</AUDITED>"+ "\n" +
"      <SORTPOSITION> 1000</SORTPOSITION>"+ "\n" +
"      <ALTERID> 31</ALTERID>"+ "\n" +
"      <SERVICETAXDETAILS.LIST>      </SERVICETAXDETAILS.LIST>"+ "\n" +
"      <LBTREGNDETAILS.LIST>      </LBTREGNDETAILS.LIST>"+ "\n" +
"      <VATDETAILS.LIST>      </VATDETAILS.LIST>"+ "\n" +
"      <SALESTAXCESSDETAILS.LIST>      </SALESTAXCESSDETAILS.LIST>"+ "\n" +
"      <GSTDETAILS.LIST>      </GSTDETAILS.LIST>"+ "\n" +
"      <LANGUAGENAME.LIST>"+ "\n" +
"       <NAME.LIST TYPE=\"String\">"+ "\n" +
"        <NAME>Profit &amp; Loss A/c</NAME>"+ "\n" +
"       </NAME.LIST>"+ "\n" +
"       <LANGUAGEID> 1033</LANGUAGEID>"+ "\n" +
"      </LANGUAGENAME.LIST>"+ "\n" +
"      <XBRLDETAIL.LIST>      </XBRLDETAIL.LIST>"+ "\n" +
"      <AUDITDETAILS.LIST>      </AUDITDETAILS.LIST>"+ "\n" +
"      <SCHVIDETAILS.LIST>      </SCHVIDETAILS.LIST>"+ "\n" +
"      <EXCISETARIFFDETAILS.LIST>      </EXCISETARIFFDETAILS.LIST>"+ "\n" +
"      <TCSCATEGORYDETAILS.LIST>      </TCSCATEGORYDETAILS.LIST>"+ "\n" +
"      <TDSCATEGORYDETAILS.LIST>      </TDSCATEGORYDETAILS.LIST>"+ "\n" +
"      <SLABPERIOD.LIST>      </SLABPERIOD.LIST>"+ "\n" +
"      <GRATUITYPERIOD.LIST>      </GRATUITYPERIOD.LIST>"+ "\n" +
"      <ADDITIONALCOMPUTATIONS.LIST>      </ADDITIONALCOMPUTATIONS.LIST>"+ "\n" +
"      <EXCISEJURISDICTIONDETAILS.LIST>      </EXCISEJURISDICTIONDETAILS.LIST>"+ "\n" +
"      <EXCLUDEDTAXATIONS.LIST>      </EXCLUDEDTAXATIONS.LIST>"+ "\n" +
"      <BANKALLOCATIONS.LIST>      </BANKALLOCATIONS.LIST>"+ "\n" +
"      <PAYMENTDETAILS.LIST>      </PAYMENTDETAILS.LIST>"+ "\n" +
"      <BANKEXPORTFORMATS.LIST>      </BANKEXPORTFORMATS.LIST>"+ "\n" +
"      <BILLALLOCATIONS.LIST>      </BILLALLOCATIONS.LIST>"+ "\n" +
"      <INTERESTCOLLECTION.LIST>      </INTERESTCOLLECTION.LIST>"+ "\n" +
"      <LEDGERCLOSINGVALUES.LIST>      </LEDGERCLOSINGVALUES.LIST>"+ "\n" +
"      <LEDGERAUDITCLASS.LIST>      </LEDGERAUDITCLASS.LIST>"+ "\n" +
"      <OLDAUDITENTRIES.LIST>      </OLDAUDITENTRIES.LIST>"+ "\n" +
"      <TDSEXEMPTIONRULES.LIST>      </TDSEXEMPTIONRULES.LIST>"+ "\n" +
"      <DEDUCTINSAMEVCHRULES.LIST>      </DEDUCTINSAMEVCHRULES.LIST>"+ "\n" +
"      <LOWERDEDUCTION.LIST>      </LOWERDEDUCTION.LIST>"+ "\n" +
"      <STXABATEMENTDETAILS.LIST>      </STXABATEMENTDETAILS.LIST>"+ "\n" +
"      <LEDMULTIADDRESSLIST.LIST>      </LEDMULTIADDRESSLIST.LIST>"+ "\n" +
"      <STXTAXDETAILS.LIST>      </STXTAXDETAILS.LIST>"+ "\n" +
"      <CHEQUERANGE.LIST>      </CHEQUERANGE.LIST>"+ "\n" +
"      <DEFAULTVCHCHEQUEDETAILS.LIST>      </DEFAULTVCHCHEQUEDETAILS.LIST>"+ "\n" +
"      <ACCOUNTAUDITENTRIES.LIST>      </ACCOUNTAUDITENTRIES.LIST>"+ "\n" +
"      <AUDITENTRIES.LIST>      </AUDITENTRIES.LIST>"+ "\n" +
"      <BRSIMPORTEDINFO.LIST>      </BRSIMPORTEDINFO.LIST>"+ "\n" +
"      <AUTOBRSCONFIGS.LIST>      </AUTOBRSCONFIGS.LIST>"+ "\n" +
"      <BANKURENTRIES.LIST>      </BANKURENTRIES.LIST>"+ "\n" +
"      <DEFAULTCHEQUEDETAILS.LIST>      </DEFAULTCHEQUEDETAILS.LIST>"+ "\n" +
"      <DEFAULTOPENINGCHEQUEDETAILS.LIST>      </DEFAULTOPENINGCHEQUEDETAILS.LIST>"+ "\n" +
"      <CANCELLEDPAYALLOCATIONS.LIST>      </CANCELLEDPAYALLOCATIONS.LIST>"+ "\n" +
"      <ECHEQUEPRINTLOCATION.LIST>      </ECHEQUEPRINTLOCATION.LIST>"+ "\n" +
"      <ECHEQUEPAYABLELOCATION.LIST>      </ECHEQUEPAYABLELOCATION.LIST>"+ "\n" +
"      <EDDPRINTLOCATION.LIST>      </EDDPRINTLOCATION.LIST>"+ "\n" +
"      <EDDPAYABLELOCATION.LIST>      </EDDPAYABLELOCATION.LIST>"+ "\n" +
"      <AVAILABLETRANSACTIONTYPES.LIST>      </AVAILABLETRANSACTIONTYPES.LIST>"+ "\n" +
"      <LEDPAYINSCONFIGS.LIST>      </LEDPAYINSCONFIGS.LIST>"+ "\n" +
"      <TYPECODEDETAILS.LIST>      </TYPECODEDETAILS.LIST>"+ "\n" +
"      <FIELDVALIDATIONDETAILS.LIST>      </FIELDVALIDATIONDETAILS.LIST>"+ "\n" +
"      <INPUTCRALLOCS.LIST>      </INPUTCRALLOCS.LIST>"+ "\n" +
"      <GSTCLASSFNIGSTRATES.LIST>      </GSTCLASSFNIGSTRATES.LIST>"+ "\n" +
"      <EXTARIFFDUTYHEADDETAILS.LIST>      </EXTARIFFDUTYHEADDETAILS.LIST>"+ "\n" +
"      <VOUCHERTYPEPRODUCTCODES.LIST>      </VOUCHERTYPEPRODUCTCODES.LIST>"+ "\n" +
"     </LEDGER>"+ "\n" +
"    </TALLYMESSAGE>\n";

        string VoucherAmazonReceipt =
"    <TALLYMESSAGE xmlns:UDF=\"TallyUDF\">"+ "\n" +
"     <LEDGER NAME=\"Amazon - Receipt - ET\" RESERVEDNAME=\"\">"+ "\n" +
"      <MAILINGNAME.LIST TYPE=\"String\">"+ "\n" +
"       <MAILINGNAME>Amazon - Receipt - ET</MAILINGNAME>"+ "\n" +
"      </MAILINGNAME.LIST>"+ "\n" +
"      <OLDAUDITENTRYIDS.LIST TYPE=\"Number\">"+ "\n" +
"       <OLDAUDITENTRYIDS>-1</OLDAUDITENTRYIDS>"+ "\n" +
"      </OLDAUDITENTRYIDS.LIST>"+ "\n" +
"      <GUID>df13ea43-535b-411e-a3ae-09a30f52223d-0000009f</GUID>"+ "\n" +
"      <CURRENCYNAME>\"₹\"</CURRENCYNAME>"+ "\n" +
"      <VATDEALERTYPE>Regular</VATDEALERTYPE>"+ "\n" +
"      <PARENT>Sundry Debtors</PARENT>"+ "\n" +
"      <TAXCLASSIFICATIONNAME/>"+ "\n" +
"      <TAXTYPE>Others</TAXTYPE>"+ "\n" +
"      <GSTTYPE/>"+ "\n" +
"      <APPROPRIATEFOR/>"+ "\n" +
"      <SERVICECATEGORY>&#4; Not Applicable</SERVICECATEGORY>"+ "\n" +
"      <EXCISELEDGERCLASSIFICATION/>"+ "\n" +
"      <EXCISEDUTYTYPE/>"+ "\n" +
"      <EXCISENATUREOFPURCHASE/>"+ "\n" +
"      <LEDGERFBTCATEGORY/>"+ "\n" +
"      <ISBILLWISEON>Yes</ISBILLWISEON>"+ "\n" +
"      <ISCOSTCENTRESON>No</ISCOSTCENTRESON>"+ "\n" +
"      <ISINTERESTON>No</ISINTERESTON>"+ "\n" +
"      <ALLOWINMOBILE>No</ALLOWINMOBILE>"+ "\n" +
"      <ISCOSTTRACKINGON>No</ISCOSTTRACKINGON>"+ "\n" +
"      <ISBENEFICIARYCODEON>No</ISBENEFICIARYCODEON>"+ "\n" +
"      <ISUPDATINGTARGETID>No</ISUPDATINGTARGETID>"+ "\n" +
"      <ASORIGINAL>Yes</ASORIGINAL>"+ "\n" +
"      <ISCONDENSED>No</ISCONDENSED>"+ "\n" +
"      <AFFECTSSTOCK>No</AFFECTSSTOCK>"+ "\n" +
"      <ISRATEINCLUSIVEVAT>No</ISRATEINCLUSIVEVAT>"+ "\n" +
"      <FORPAYROLL>No</FORPAYROLL>"+ "\n" +
"      <ISABCENABLED>No</ISABCENABLED>"+ "\n" +
"      <ISCREDITDAYSCHKON>No</ISCREDITDAYSCHKON>"+ "\n" +
"      <INTERESTONBILLWISE>No</INTERESTONBILLWISE>"+ "\n" +
"      <OVERRIDEINTEREST>No</OVERRIDEINTEREST>"+ "\n" +
"      <OVERRIDEADVINTEREST>No</OVERRIDEADVINTEREST>"+ "\n" +
"      <USEFORVAT>No</USEFORVAT>"+ "\n" +
"      <IGNORETDSEXEMPT>No</IGNORETDSEXEMPT>"+ "\n" +
"      <ISTCSAPPLICABLE>No</ISTCSAPPLICABLE>"+ "\n" +
"      <ISTDSAPPLICABLE>No</ISTDSAPPLICABLE>"+ "\n" +
"      <ISFBTAPPLICABLE>No</ISFBTAPPLICABLE>"+ "\n" +
"      <ISGSTAPPLICABLE>No</ISGSTAPPLICABLE>"+ "\n" +
"      <ISEXCISEAPPLICABLE>No</ISEXCISEAPPLICABLE>"+ "\n" +
"      <ISTDSEXPENSE>No</ISTDSEXPENSE>"+ "\n" +
"      <ISEDLIAPPLICABLE>No</ISEDLIAPPLICABLE>"+ "\n" +
"      <ISRELATEDPARTY>No</ISRELATEDPARTY>"+ "\n" +
"      <USEFORESIELIGIBILITY>No</USEFORESIELIGIBILITY>"+ "\n" +
"      <ISINTERESTINCLLASTDAY>No</ISINTERESTINCLLASTDAY>"+ "\n" +
"      <APPROPRIATETAXVALUE>No</APPROPRIATETAXVALUE>"+ "\n" +
"      <ISBEHAVEASDUTY>No</ISBEHAVEASDUTY>"+ "\n" +
"      <INTERESTINCLDAYOFADDITION>No</INTERESTINCLDAYOFADDITION>"+ "\n" +
"      <INTERESTINCLDAYOFDEDUCTION>No</INTERESTINCLDAYOFDEDUCTION>"+ "\n" +
"      <OVERRIDECREDITLIMIT>No</OVERRIDECREDITLIMIT>"+ "\n" +
"      <ISAGAINSTFORMC>No</ISAGAINSTFORMC>"+ "\n" +
"      <ISCHEQUEPRINTINGENABLED>Yes</ISCHEQUEPRINTINGENABLED>"+ "\n" +
"      <ISPAYUPLOAD>No</ISPAYUPLOAD>"+ "\n" +
"      <ISPAYBATCHONLYSAL>No</ISPAYBATCHONLYSAL>"+ "\n" +
"      <ISBNFCODESUPPORTED>No</ISBNFCODESUPPORTED>"+ "\n" +
"      <ALLOWEXPORTWITHERRORS>No</ALLOWEXPORTWITHERRORS>"+ "\n" +
"      <USEFORNOTIONALITC>No</USEFORNOTIONALITC>"+ "\n" +
"      <ISECOMMOPERATOR>No</ISECOMMOPERATOR>"+ "\n" +
"      <SHOWINPAYSLIP>No</SHOWINPAYSLIP>"+ "\n" +
"      <USEFORGRATUITY>No</USEFORGRATUITY>"+ "\n" +
"      <ISTDSPROJECTED>No</ISTDSPROJECTED>"+ "\n" +
"      <FORSERVICETAX>No</FORSERVICETAX>"+ "\n" +
"      <ISINPUTCREDIT>No</ISINPUTCREDIT>"+ "\n" +
"      <ISEXEMPTED>No</ISEXEMPTED>"+ "\n" +
"      <ISABATEMENTAPPLICABLE>No</ISABATEMENTAPPLICABLE>"+ "\n" +
"      <ISSTXPARTY>No</ISSTXPARTY>"+ "\n" +
"      <ISSTXNONREALIZEDTYPE>No</ISSTXNONREALIZEDTYPE>"+ "\n" +
"      <ISUSEDFORCVD>No</ISUSEDFORCVD>"+ "\n" +
"      <LEDBELONGSTONONTAXABLE>No</LEDBELONGSTONONTAXABLE>"+ "\n" +
"      <ISEXCISEMERCHANTEXPORTER>No</ISEXCISEMERCHANTEXPORTER>"+ "\n" +
"      <ISPARTYEXEMPTED>No</ISPARTYEXEMPTED>"+ "\n" +
"      <ISSEZPARTY>No</ISSEZPARTY>"+ "\n" +
"      <TDSDEDUCTEEISSPECIALRATE>No</TDSDEDUCTEEISSPECIALRATE>"+ "\n" +
"      <ISECHEQUESUPPORTED>No</ISECHEQUESUPPORTED>"+ "\n" +
"      <ISEDDSUPPORTED>No</ISEDDSUPPORTED>"+ "\n" +
"      <HASECHEQUEDELIVERYMODE>No</HASECHEQUEDELIVERYMODE>"+ "\n" +
"      <HASECHEQUEDELIVERYTO>No</HASECHEQUEDELIVERYTO>"+ "\n" +
"      <HASECHEQUEPRINTLOCATION>No</HASECHEQUEPRINTLOCATION>"+ "\n" +
"      <HASECHEQUEPAYABLELOCATION>No</HASECHEQUEPAYABLELOCATION>"+ "\n" +
"      <HASECHEQUEBANKLOCATION>No</HASECHEQUEBANKLOCATION>"+ "\n" +
"      <HASEDDDELIVERYMODE>No</HASEDDDELIVERYMODE>"+ "\n" +
"      <HASEDDDELIVERYTO>No</HASEDDDELIVERYTO>"+ "\n" +
"      <HASEDDPRINTLOCATION>No</HASEDDPRINTLOCATION>"+ "\n" +
"      <HASEDDPAYABLELOCATION>No</HASEDDPAYABLELOCATION>"+ "\n" +
"      <HASEDDBANKLOCATION>No</HASEDDBANKLOCATION>"+ "\n" +
"      <ISEBANKINGENABLED>No</ISEBANKINGENABLED>"+ "\n" +
"      <ISEXPORTFILEENCRYPTED>No</ISEXPORTFILEENCRYPTED>"+ "\n" +
"      <ISBATCHENABLED>No</ISBATCHENABLED>"+ "\n" +
"      <ISPRODUCTCODEBASED>No</ISPRODUCTCODEBASED>"+ "\n" +
"      <HASEDDCITY>No</HASEDDCITY>"+ "\n" +
"      <HASECHEQUECITY>No</HASECHEQUECITY>"+ "\n" +
"      <ISFILENAMEFORMATSUPPORTED>No</ISFILENAMEFORMATSUPPORTED>"+ "\n" +
"      <HASCLIENTCODE>No</HASCLIENTCODE>"+ "\n" +
"      <PAYINSISBATCHAPPLICABLE>No</PAYINSISBATCHAPPLICABLE>"+ "\n" +
"      <PAYINSISFILENUMAPP>No</PAYINSISFILENUMAPP>"+ "\n" +
"      <ISSALARYTRANSGROUPEDFORBRS>No</ISSALARYTRANSGROUPEDFORBRS>"+ "\n" +
"      <ISEBANKINGSUPPORTED>No</ISEBANKINGSUPPORTED>"+ "\n" +
"      <ISSCBUAE>No</ISSCBUAE>"+ "\n" +
"      <ISBANKSTATUSAPP>No</ISBANKSTATUSAPP>"+ "\n" +
"      <ISSALARYGROUPED>No</ISSALARYGROUPED>"+ "\n" +
"      <USEFORPURCHASETAX>No</USEFORPURCHASETAX>"+ "\n" +
"      <AUDITED>No</AUDITED>"+ "\n" +
"      <SORTPOSITION> 1000</SORTPOSITION>"+ "\n" +
"      <ALTERID> 238</ALTERID>"+ "\n" +
"      <SERVICETAXDETAILS.LIST>      </SERVICETAXDETAILS.LIST>"+ "\n" +
"      <LBTREGNDETAILS.LIST>      </LBTREGNDETAILS.LIST>"+ "\n" +
"      <VATDETAILS.LIST>      </VATDETAILS.LIST>"+ "\n" +
"      <SALESTAXCESSDETAILS.LIST>      </SALESTAXCESSDETAILS.LIST>"+ "\n" +
"      <GSTDETAILS.LIST>      </GSTDETAILS.LIST>"+ "\n" +
"      <LANGUAGENAME.LIST>"+ "\n" +
"       <NAME.LIST TYPE=\"String\">"+ "\n" +
"        <NAME>Amazon - Receipt - ET</NAME>"+ "\n" +
"       </NAME.LIST>"+ "\n" +
"       <LANGUAGEID> 1033</LANGUAGEID>"+ "\n" +
"      </LANGUAGENAME.LIST>"+ "\n" +
"      <XBRLDETAIL.LIST>      </XBRLDETAIL.LIST>"+ "\n" +
"      <AUDITDETAILS.LIST>      </AUDITDETAILS.LIST>"+ "\n" +
"      <SCHVIDETAILS.LIST>      </SCHVIDETAILS.LIST>"+ "\n" +
"      <EXCISETARIFFDETAILS.LIST>      </EXCISETARIFFDETAILS.LIST>"+ "\n" +
"      <TCSCATEGORYDETAILS.LIST>      </TCSCATEGORYDETAILS.LIST>"+ "\n" +
"      <TDSCATEGORYDETAILS.LIST>      </TDSCATEGORYDETAILS.LIST>"+ "\n" +
"      <SLABPERIOD.LIST>      </SLABPERIOD.LIST>"+ "\n" +
"      <GRATUITYPERIOD.LIST>      </GRATUITYPERIOD.LIST>"+ "\n" +
"      <ADDITIONALCOMPUTATIONS.LIST>      </ADDITIONALCOMPUTATIONS.LIST>"+ "\n" +
"      <EXCISEJURISDICTIONDETAILS.LIST>      </EXCISEJURISDICTIONDETAILS.LIST>"+ "\n" +
"      <EXCLUDEDTAXATIONS.LIST>      </EXCLUDEDTAXATIONS.LIST>"+ "\n" +
"      <BANKALLOCATIONS.LIST>      </BANKALLOCATIONS.LIST>"+ "\n" +
"      <PAYMENTDETAILS.LIST>      </PAYMENTDETAILS.LIST>"+ "\n" +
"      <BANKEXPORTFORMATS.LIST>      </BANKEXPORTFORMATS.LIST>"+ "\n" +
"      <BILLALLOCATIONS.LIST>      </BILLALLOCATIONS.LIST>"+ "\n" +
"      <INTERESTCOLLECTION.LIST>      </INTERESTCOLLECTION.LIST>"+ "\n" +
"      <LEDGERCLOSINGVALUES.LIST>      </LEDGERCLOSINGVALUES.LIST>"+ "\n" +
"      <LEDGERAUDITCLASS.LIST>      </LEDGERAUDITCLASS.LIST>"+ "\n" +
"      <OLDAUDITENTRIES.LIST>      </OLDAUDITENTRIES.LIST>"+ "\n" +
"      <TDSEXEMPTIONRULES.LIST>      </TDSEXEMPTIONRULES.LIST>"+ "\n" +
"      <DEDUCTINSAMEVCHRULES.LIST>      </DEDUCTINSAMEVCHRULES.LIST>"+ "\n" +
"      <LOWERDEDUCTION.LIST>      </LOWERDEDUCTION.LIST>"+ "\n" +
"      <STXABATEMENTDETAILS.LIST>      </STXABATEMENTDETAILS.LIST>"+ "\n" +
"      <LEDMULTIADDRESSLIST.LIST>      </LEDMULTIADDRESSLIST.LIST>"+ "\n" +
"      <STXTAXDETAILS.LIST>      </STXTAXDETAILS.LIST>"+ "\n" +
"      <CHEQUERANGE.LIST>      </CHEQUERANGE.LIST>"+ "\n" +
"      <DEFAULTVCHCHEQUEDETAILS.LIST>      </DEFAULTVCHCHEQUEDETAILS.LIST>"+ "\n" +
"      <ACCOUNTAUDITENTRIES.LIST>      </ACCOUNTAUDITENTRIES.LIST>"+ "\n" +
"      <AUDITENTRIES.LIST>      </AUDITENTRIES.LIST>"+ "\n" +
"      <BRSIMPORTEDINFO.LIST>      </BRSIMPORTEDINFO.LIST>"+ "\n" +
"      <AUTOBRSCONFIGS.LIST>      </AUTOBRSCONFIGS.LIST>"+ "\n" +
"      <BANKURENTRIES.LIST>      </BANKURENTRIES.LIST>"+ "\n" +
"      <DEFAULTCHEQUEDETAILS.LIST>      </DEFAULTCHEQUEDETAILS.LIST>"+ "\n" +
"      <DEFAULTOPENINGCHEQUEDETAILS.LIST>      </DEFAULTOPENINGCHEQUEDETAILS.LIST>"+ "\n" +
"      <CANCELLEDPAYALLOCATIONS.LIST>      </CANCELLEDPAYALLOCATIONS.LIST>"+ "\n" +
"      <ECHEQUEPRINTLOCATION.LIST>      </ECHEQUEPRINTLOCATION.LIST>"+ "\n" +
"      <ECHEQUEPAYABLELOCATION.LIST>      </ECHEQUEPAYABLELOCATION.LIST>"+ "\n" +
"      <EDDPRINTLOCATION.LIST>      </EDDPRINTLOCATION.LIST>"+ "\n" +
"      <EDDPAYABLELOCATION.LIST>      </EDDPAYABLELOCATION.LIST>"+ "\n" +
"      <AVAILABLETRANSACTIONTYPES.LIST>      </AVAILABLETRANSACTIONTYPES.LIST>"+ "\n" +
"      <LEDPAYINSCONFIGS.LIST>      </LEDPAYINSCONFIGS.LIST>"+ "\n" +
"      <TYPECODEDETAILS.LIST>      </TYPECODEDETAILS.LIST>"+ "\n" +
"      <FIELDVALIDATIONDETAILS.LIST>      </FIELDVALIDATIONDETAILS.LIST>"+ "\n" +
"      <INPUTCRALLOCS.LIST>      </INPUTCRALLOCS.LIST>"+ "\n" +
"      <GSTCLASSFNIGSTRATES.LIST>      </GSTCLASSFNIGSTRATES.LIST>"+ "\n" +
"      <EXTARIFFDUTYHEADDETAILS.LIST>      </EXTARIFFDUTYHEADDETAILS.LIST>"+ "\n" +
"      <VOUCHERTYPEPRODUCTCODES.LIST>      </VOUCHERTYPEPRODUCTCODES.LIST>"+ "\n" +
"     </LEDGER>"+ "\n" +
"    </TALLYMESSAGE>\n";

        string VoucherCash =
"   <TALLYMESSAGE xmlns:UDF=\"TallyUDF\">"+ "\n" +
"     <LEDGER NAME=\"Cash\" RESERVEDNAME=\"\">"+ "\n" +
"      <GUID>df13ea43-535b-411e-a3ae-09a30f52223d-0000001f</GUID>"+ "\n" +
"      <PARENT>Cash-in-hand</PARENT>"+ "\n" +
"      <TAXCLASSIFICATIONNAME/>"+ "\n" +
"      <TAXTYPE>Others</TAXTYPE>"+ "\n" +
"      <GSTTYPE/>"+ "\n" +
"      <APPROPRIATEFOR/>"+ "\n" +
"      <SERVICECATEGORY>&#4; Not Applicable</SERVICECATEGORY>"+ "\n" +
"      <EXCISELEDGERCLASSIFICATION/>"+ "\n" +
"      <EXCISEDUTYTYPE/>"+ "\n" +
"      <EXCISENATUREOFPURCHASE/>"+ "\n" +
"      <LEDGERFBTCATEGORY/>"+ "\n" +
"      <ISBILLWISEON>No</ISBILLWISEON>"+ "\n" +
"      <ISCOSTCENTRESON>No</ISCOSTCENTRESON>"+ "\n" +
"      <ISINTERESTON>No</ISINTERESTON>"+ "\n" +
"      <ALLOWINMOBILE>No</ALLOWINMOBILE>"+ "\n" +
"      <ISCOSTTRACKINGON>No</ISCOSTTRACKINGON>"+ "\n" +
"      <ISBENEFICIARYCODEON>No</ISBENEFICIARYCODEON>"+ "\n" +
"      <ISUPDATINGTARGETID>No</ISUPDATINGTARGETID>"+ "\n" +
"      <ASORIGINAL>Yes</ASORIGINAL>"+ "\n" +
"      <ISCONDENSED>No</ISCONDENSED>"+ "\n" +
"      <AFFECTSSTOCK>No</AFFECTSSTOCK>"+ "\n" +
"      <ISRATEINCLUSIVEVAT>No</ISRATEINCLUSIVEVAT>"+ "\n" +
"      <FORPAYROLL>No</FORPAYROLL>"+ "\n" +
"      <ISABCENABLED>No</ISABCENABLED>"+ "\n" +
"      <ISCREDITDAYSCHKON>No</ISCREDITDAYSCHKON>"+ "\n" +
"      <INTERESTONBILLWISE>No</INTERESTONBILLWISE>"+ "\n" +
"      <OVERRIDEINTEREST>No</OVERRIDEINTEREST>"+ "\n" +
"      <OVERRIDEADVINTEREST>No</OVERRIDEADVINTEREST>"+ "\n" +
"      <USEFORVAT>No</USEFORVAT>"+ "\n" +
"      <IGNORETDSEXEMPT>No</IGNORETDSEXEMPT>"+ "\n" +
"      <ISTCSAPPLICABLE>No</ISTCSAPPLICABLE>"+ "\n" +
"      <ISTDSAPPLICABLE>No</ISTDSAPPLICABLE>"+ "\n" +
"      <ISFBTAPPLICABLE>No</ISFBTAPPLICABLE>"+ "\n" +
"      <ISGSTAPPLICABLE>No</ISGSTAPPLICABLE>"+ "\n" +
"      <ISEXCISEAPPLICABLE>No</ISEXCISEAPPLICABLE>"+ "\n" +
"      <ISTDSEXPENSE>No</ISTDSEXPENSE>"+ "\n" +
"      <ISEDLIAPPLICABLE>No</ISEDLIAPPLICABLE>"+ "\n" +
"      <ISRELATEDPARTY>No</ISRELATEDPARTY>"+ "\n" +
"      <USEFORESIELIGIBILITY>No</USEFORESIELIGIBILITY>"+ "\n" +
"      <ISINTERESTINCLLASTDAY>No</ISINTERESTINCLLASTDAY>"+ "\n" +
"      <APPROPRIATETAXVALUE>No</APPROPRIATETAXVALUE>"+ "\n" +
"      <ISBEHAVEASDUTY>No</ISBEHAVEASDUTY>"+ "\n" +
"      <INTERESTINCLDAYOFADDITION>No</INTERESTINCLDAYOFADDITION>"+ "\n" +
"      <INTERESTINCLDAYOFDEDUCTION>No</INTERESTINCLDAYOFDEDUCTION>"+ "\n" +
"      <OVERRIDECREDITLIMIT>No</OVERRIDECREDITLIMIT>"+ "\n" +
"      <ISAGAINSTFORMC>No</ISAGAINSTFORMC>"+ "\n" +
"      <ISCHEQUEPRINTINGENABLED>No</ISCHEQUEPRINTINGENABLED>"+ "\n" +
"      <ISPAYUPLOAD>No</ISPAYUPLOAD>"+ "\n" +
"      <ISPAYBATCHONLYSAL>No</ISPAYBATCHONLYSAL>"+ "\n" +
"      <ISBNFCODESUPPORTED>No</ISBNFCODESUPPORTED>"+ "\n" +
"      <ALLOWEXPORTWITHERRORS>No</ALLOWEXPORTWITHERRORS>"+ "\n" +
"      <USEFORNOTIONALITC>No</USEFORNOTIONALITC>"+ "\n" +
"      <ISECOMMOPERATOR>No</ISECOMMOPERATOR>"+ "\n" +
"      <SHOWINPAYSLIP>No</SHOWINPAYSLIP>"+ "\n" +
"      <USEFORGRATUITY>No</USEFORGRATUITY>"+ "\n" +
"      <ISTDSPROJECTED>No</ISTDSPROJECTED>"+ "\n" +
"      <FORSERVICETAX>No</FORSERVICETAX>"+ "\n" +
"      <ISINPUTCREDIT>No</ISINPUTCREDIT>"+ "\n" +
"      <ISEXEMPTED>No</ISEXEMPTED>"+ "\n" +
"      <ISABATEMENTAPPLICABLE>No</ISABATEMENTAPPLICABLE>"+ "\n" +
"      <ISSTXPARTY>No</ISSTXPARTY>"+ "\n" +
"      <ISSTXNONREALIZEDTYPE>No</ISSTXNONREALIZEDTYPE>"+ "\n" +
"      <ISUSEDFORCVD>No</ISUSEDFORCVD>"+ "\n" +
"      <LEDBELONGSTONONTAXABLE>No</LEDBELONGSTONONTAXABLE>"+ "\n" +
"      <ISEXCISEMERCHANTEXPORTER>No</ISEXCISEMERCHANTEXPORTER>"+ "\n" +
"      <ISPARTYEXEMPTED>No</ISPARTYEXEMPTED>"+ "\n" +
"      <ISSEZPARTY>No</ISSEZPARTY>"+ "\n" +
"      <TDSDEDUCTEEISSPECIALRATE>No</TDSDEDUCTEEISSPECIALRATE>"+ "\n" +
"      <ISECHEQUESUPPORTED>No</ISECHEQUESUPPORTED>"+ "\n" +
"      <ISEDDSUPPORTED>No</ISEDDSUPPORTED>"+ "\n" +
"      <HASECHEQUEDELIVERYMODE>No</HASECHEQUEDELIVERYMODE>"+ "\n" +
"      <HASECHEQUEDELIVERYTO>No</HASECHEQUEDELIVERYTO>"+ "\n" +
"      <HASECHEQUEPRINTLOCATION>No</HASECHEQUEPRINTLOCATION>"+ "\n" +
"      <HASECHEQUEPAYABLELOCATION>No</HASECHEQUEPAYABLELOCATION>"+ "\n" +
"      <HASECHEQUEBANKLOCATION>No</HASECHEQUEBANKLOCATION>"+ "\n" +
"      <HASEDDDELIVERYMODE>No</HASEDDDELIVERYMODE>"+ "\n" +
"      <HASEDDDELIVERYTO>No</HASEDDDELIVERYTO>"+ "\n" +
"      <HASEDDPRINTLOCATION>No</HASEDDPRINTLOCATION>"+ "\n" +
"      <HASEDDPAYABLELOCATION>No</HASEDDPAYABLELOCATION>"+ "\n" +
"      <HASEDDBANKLOCATION>No</HASEDDBANKLOCATION>"+ "\n" +
"      <ISEBANKINGENABLED>No</ISEBANKINGENABLED>"+ "\n" +
"      <ISEXPORTFILEENCRYPTED>No</ISEXPORTFILEENCRYPTED>"+ "\n" +
"      <ISBATCHENABLED>No</ISBATCHENABLED>"+ "\n" +
"      <ISPRODUCTCODEBASED>No</ISPRODUCTCODEBASED>"+ "\n" +
"      <HASEDDCITY>No</HASEDDCITY>"+ "\n" +
"      <HASECHEQUECITY>No</HASECHEQUECITY>"+ "\n" +
"      <ISFILENAMEFORMATSUPPORTED>No</ISFILENAMEFORMATSUPPORTED>"+ "\n" +
"      <HASCLIENTCODE>No</HASCLIENTCODE>"+ "\n" +
"      <PAYINSISBATCHAPPLICABLE>No</PAYINSISBATCHAPPLICABLE>"+ "\n" +
"      <PAYINSISFILENUMAPP>No</PAYINSISFILENUMAPP>"+ "\n" +
"      <ISSALARYTRANSGROUPEDFORBRS>No</ISSALARYTRANSGROUPEDFORBRS>"+ "\n" +
"      <ISEBANKINGSUPPORTED>No</ISEBANKINGSUPPORTED>"+ "\n" +
"      <ISSCBUAE>No</ISSCBUAE>"+ "\n" +
"      <ISBANKSTATUSAPP>No</ISBANKSTATUSAPP>"+ "\n" +
"      <ISSALARYGROUPED>No</ISSALARYGROUPED>"+ "\n" +
"      <USEFORPURCHASETAX>No</USEFORPURCHASETAX>"+ "\n" +
"      <AUDITED>No</AUDITED>"+ "\n" +
"      <SORTPOSITION> 1000</SORTPOSITION>"+ "\n" +
"      <ALTERID> 32</ALTERID>"+ "\n" +
"      <SERVICETAXDETAILS.LIST>      </SERVICETAXDETAILS.LIST>"+ "\n" +
"      <LBTREGNDETAILS.LIST>      </LBTREGNDETAILS.LIST>"+ "\n" +
"      <VATDETAILS.LIST>      </VATDETAILS.LIST>"+ "\n" +
"      <SALESTAXCESSDETAILS.LIST>      </SALESTAXCESSDETAILS.LIST>"+ "\n" +
"      <GSTDETAILS.LIST>      </GSTDETAILS.LIST>"+ "\n" +
"      <LANGUAGENAME.LIST>"+ "\n" +
"       <NAME.LIST TYPE=\"String\">"+ "\n" +
"        <NAME>Cash</NAME>"+ "\n" +
"       </NAME.LIST>"+ "\n" +
"       <LANGUAGEID> 1033</LANGUAGEID>"+ "\n" +
"      </LANGUAGENAME.LIST>"+ "\n" +
"      <XBRLDETAIL.LIST>      </XBRLDETAIL.LIST>"+ "\n" +
"      <AUDITDETAILS.LIST>      </AUDITDETAILS.LIST>"+ "\n" +
"      <SCHVIDETAILS.LIST>      </SCHVIDETAILS.LIST>"+ "\n" +
"      <EXCISETARIFFDETAILS.LIST>      </EXCISETARIFFDETAILS.LIST>"+ "\n" +
"      <TCSCATEGORYDETAILS.LIST>      </TCSCATEGORYDETAILS.LIST>"+ "\n" +
"      <TDSCATEGORYDETAILS.LIST>      </TDSCATEGORYDETAILS.LIST>"+ "\n" +
"      <SLABPERIOD.LIST>      </SLABPERIOD.LIST>"+ "\n" +
"      <GRATUITYPERIOD.LIST>      </GRATUITYPERIOD.LIST>"+ "\n" +
"      <ADDITIONALCOMPUTATIONS.LIST>      </ADDITIONALCOMPUTATIONS.LIST>"+ "\n" +
"      <EXCISEJURISDICTIONDETAILS.LIST>      </EXCISEJURISDICTIONDETAILS.LIST>"+ "\n" +
"      <EXCLUDEDTAXATIONS.LIST>      </EXCLUDEDTAXATIONS.LIST>"+ "\n" +
"      <BANKALLOCATIONS.LIST>      </BANKALLOCATIONS.LIST>"+ "\n" +
"      <PAYMENTDETAILS.LIST>      </PAYMENTDETAILS.LIST>"+ "\n" +
"      <BANKEXPORTFORMATS.LIST>      </BANKEXPORTFORMATS.LIST>"+ "\n" +
"      <BILLALLOCATIONS.LIST>      </BILLALLOCATIONS.LIST>"+ "\n" +
"      <INTERESTCOLLECTION.LIST>      </INTERESTCOLLECTION.LIST>"+ "\n" +
"      <LEDGERCLOSINGVALUES.LIST>      </LEDGERCLOSINGVALUES.LIST>"+ "\n" +
"      <LEDGERAUDITCLASS.LIST>      </LEDGERAUDITCLASS.LIST>"+ "\n" +
"      <OLDAUDITENTRIES.LIST>      </OLDAUDITENTRIES.LIST>"+ "\n" +
"      <TDSEXEMPTIONRULES.LIST>      </TDSEXEMPTIONRULES.LIST>"+ "\n" +
"      <DEDUCTINSAMEVCHRULES.LIST>      </DEDUCTINSAMEVCHRULES.LIST>"+ "\n" +
"      <LOWERDEDUCTION.LIST>      </LOWERDEDUCTION.LIST>"+ "\n" +
"      <STXABATEMENTDETAILS.LIST>      </STXABATEMENTDETAILS.LIST>"+ "\n" +
"      <LEDMULTIADDRESSLIST.LIST>      </LEDMULTIADDRESSLIST.LIST>"+ "\n" +
"      <STXTAXDETAILS.LIST>      </STXTAXDETAILS.LIST>"+ "\n" +
"      <CHEQUERANGE.LIST>      </CHEQUERANGE.LIST>"+ "\n" +
"      <DEFAULTVCHCHEQUEDETAILS.LIST>      </DEFAULTVCHCHEQUEDETAILS.LIST>"+ "\n" +
"      <ACCOUNTAUDITENTRIES.LIST>      </ACCOUNTAUDITENTRIES.LIST>"+ "\n" +
"      <AUDITENTRIES.LIST>      </AUDITENTRIES.LIST>"+ "\n" +
"      <BRSIMPORTEDINFO.LIST>      </BRSIMPORTEDINFO.LIST>"+ "\n" +
"      <AUTOBRSCONFIGS.LIST>      </AUTOBRSCONFIGS.LIST>"+ "\n" +
"      <BANKURENTRIES.LIST>      </BANKURENTRIES.LIST>"+ "\n" +
"      <DEFAULTCHEQUEDETAILS.LIST>      </DEFAULTCHEQUEDETAILS.LIST>"+ "\n" +
"      <DEFAULTOPENINGCHEQUEDETAILS.LIST>      </DEFAULTOPENINGCHEQUEDETAILS.LIST>"+ "\n" +
"      <CANCELLEDPAYALLOCATIONS.LIST>      </CANCELLEDPAYALLOCATIONS.LIST>"+ "\n" +
"      <ECHEQUEPRINTLOCATION.LIST>      </ECHEQUEPRINTLOCATION.LIST>"+ "\n" +
"      <ECHEQUEPAYABLELOCATION.LIST>      </ECHEQUEPAYABLELOCATION.LIST>"+ "\n" +
"      <EDDPRINTLOCATION.LIST>      </EDDPRINTLOCATION.LIST>"+ "\n" +
"      <EDDPAYABLELOCATION.LIST>      </EDDPAYABLELOCATION.LIST>"+ "\n" +
"      <AVAILABLETRANSACTIONTYPES.LIST>      </AVAILABLETRANSACTIONTYPES.LIST>"+ "\n" +
"      <LEDPAYINSCONFIGS.LIST>      </LEDPAYINSCONFIGS.LIST>"+ "\n" +
"      <TYPECODEDETAILS.LIST>      </TYPECODEDETAILS.LIST>"+ "\n" +
"      <FIELDVALIDATIONDETAILS.LIST>      </FIELDVALIDATIONDETAILS.LIST>"+ "\n" +
"      <INPUTCRALLOCS.LIST>      </INPUTCRALLOCS.LIST>"+ "\n" +
"      <GSTCLASSFNIGSTRATES.LIST>      </GSTCLASSFNIGSTRATES.LIST>"+ "\n" +
"      <EXTARIFFDUTYHEADDETAILS.LIST>      </EXTARIFFDUTYHEADDETAILS.LIST>"+ "\n" +
"      <VOUCHERTYPEPRODUCTCODES.LIST>      </VOUCHERTYPEPRODUCTCODES.LIST>"+ "\n" +
"     </LEDGER>"+ "\n" +
"    </TALLYMESSAGE>\n";

        string VoucherTaxType =
"    <TALLYMESSAGE xmlns:UDF=\"TallyUDF\">"+ "\n" +
"     <LEDGER NAME=\"#TaxName#\" RESERVEDNAME=\"\">" + "\n" +
"      <MAILINGNAME.LIST TYPE=\"String\">"+ "\n" +
"       <MAILINGNAME>#TaxName#</MAILINGNAME>" + "\n" +
"      </MAILINGNAME.LIST>"+ "\n" +
"      <OLDAUDITENTRYIDS.LIST TYPE=\"Number\">"+ "\n" +
"       <OLDAUDITENTRYIDS>-1</OLDAUDITENTRYIDS>"+ "\n" +
"      </OLDAUDITENTRYIDS.LIST>"+ "\n" +
"      <GUID>#Guiduniqueno#</GUID>" + "\n" +
"      <CURRENCYNAME>₹</CURRENCYNAME>"+ "\n" +
"      <COUNTRYNAME>India</COUNTRYNAME>"+ "\n" +
"      <PARENT>Current Assets</PARENT>"+ "\n" +
"      <TAXCLASSIFICATIONNAME/>"+ "\n" +
"      <TAXTYPE>Others</TAXTYPE>"+ "\n" +
"      <COUNTRYOFRESIDENCE>India</COUNTRYOFRESIDENCE>"+ "\n" +
"      <LEDADDLALLOCTYPE/>"+ "\n" +
"      <GSTTYPE/>"+ "\n" +
"      <APPROPRIATEFOR/>"+ "\n" +
"      <SERVICECATEGORY>&#4; Not Applicable</SERVICECATEGORY>"+ "\n" +
"      <EXCISELEDGERCLASSIFICATION/>"+ "\n" +
"      <EXCISEDUTYTYPE/>"+ "\n" +
"      <EXCISENATUREOFPURCHASE/>"+ "\n" +
"      <LEDGERFBTCATEGORY/>"+ "\n" +
"      <LEDSTATENAME/>"+ "\n" +
"      <ISBILLWISEON>Yes</ISBILLWISEON>"+ "\n" +
"      <ISCOSTCENTRESON>No</ISCOSTCENTRESON>"+ "\n" +
"      <ISINTERESTON>No</ISINTERESTON>"+ "\n" +
"      <ALLOWINMOBILE>No</ALLOWINMOBILE>"+ "\n" +
"      <ISCOSTTRACKINGON>No</ISCOSTTRACKINGON>"+ "\n" +
"      <ISBENEFICIARYCODEON>No</ISBENEFICIARYCODEON>"+ "\n" +
"      <ISUPDATINGTARGETID>No</ISUPDATINGTARGETID>"+ "\n" +
"      <ASORIGINAL>Yes</ASORIGINAL>"+ "\n" +
"      <ISCONDENSED>No</ISCONDENSED>"+ "\n" +
"      <AFFECTSSTOCK>No</AFFECTSSTOCK>"+ "\n" +
"      <ISRATEINCLUSIVEVAT>No</ISRATEINCLUSIVEVAT>"+ "\n" +
"      <FORPAYROLL>No</FORPAYROLL>"+ "\n" +
"      <ISABCENABLED>No</ISABCENABLED>"+ "\n" +
"      <ISCREDITDAYSCHKON>No</ISCREDITDAYSCHKON>"+ "\n" +
"      <INTERESTONBILLWISE>No</INTERESTONBILLWISE>"+ "\n" +
"      <OVERRIDEINTEREST>No</OVERRIDEINTEREST>"+ "\n" +
"      <OVERRIDEADVINTEREST>No</OVERRIDEADVINTEREST>"+ "\n" +
"      <USEFORVAT>No</USEFORVAT>"+ "\n" +
"      <IGNORETDSEXEMPT>No</IGNORETDSEXEMPT>"+ "\n" +
"      <ISTCSAPPLICABLE>No</ISTCSAPPLICABLE>"+ "\n" +
"      <ISTDSAPPLICABLE>No</ISTDSAPPLICABLE>"+ "\n" +
"      <ISFBTAPPLICABLE>No</ISFBTAPPLICABLE>"+ "\n" +
"      <ISGSTAPPLICABLE>No</ISGSTAPPLICABLE>"+ "\n" +
"      <ISEXCISEAPPLICABLE>No</ISEXCISEAPPLICABLE>"+ "\n" +
"      <ISTDSEXPENSE>No</ISTDSEXPENSE>"+ "\n" +
"      <ISEDLIAPPLICABLE>No</ISEDLIAPPLICABLE>"+ "\n" +
"      <ISRELATEDPARTY>No</ISRELATEDPARTY>"+ "\n" +
"      <USEFORESIELIGIBILITY>No</USEFORESIELIGIBILITY>"+ "\n" +
"      <ISINTERESTINCLLASTDAY>No</ISINTERESTINCLLASTDAY>"+ "\n" +
"      <APPROPRIATETAXVALUE>No</APPROPRIATETAXVALUE>"+ "\n" +
"      <ISBEHAVEASDUTY>No</ISBEHAVEASDUTY>"+ "\n" +
"      <INTERESTINCLDAYOFADDITION>No</INTERESTINCLDAYOFADDITION>"+ "\n" +
"      <INTERESTINCLDAYOFDEDUCTION>No</INTERESTINCLDAYOFDEDUCTION>"+ "\n" +
"      <OVERRIDECREDITLIMIT>No</OVERRIDECREDITLIMIT>"+ "\n" +
"      <ISAGAINSTFORMC>No</ISAGAINSTFORMC>"+ "\n" +
"      <ISCHEQUEPRINTINGENABLED>Yes</ISCHEQUEPRINTINGENABLED>"+ "\n" +
"      <ISPAYUPLOAD>No</ISPAYUPLOAD>"+ "\n" +
"      <ISPAYBATCHONLYSAL>No</ISPAYBATCHONLYSAL>"+ "\n" +
"      <ISBNFCODESUPPORTED>No</ISBNFCODESUPPORTED>"+ "\n" +
"      <ALLOWEXPORTWITHERRORS>No</ALLOWEXPORTWITHERRORS>"+ "\n" +
"      <USEFORNOTIONALITC>No</USEFORNOTIONALITC>"+ "\n" +
"      <ISECOMMOPERATOR>No</ISECOMMOPERATOR>"+ "\n" +
"      <SHOWINPAYSLIP>No</SHOWINPAYSLIP>"+ "\n" +
"      <USEFORGRATUITY>No</USEFORGRATUITY>"+ "\n" +
"      <ISTDSPROJECTED>No</ISTDSPROJECTED>"+ "\n" +
"      <FORSERVICETAX>No</FORSERVICETAX>"+ "\n" +
"      <ISINPUTCREDIT>No</ISINPUTCREDIT>"+ "\n" +
"      <ISEXEMPTED>No</ISEXEMPTED>"+ "\n" +
"      <ISABATEMENTAPPLICABLE>No</ISABATEMENTAPPLICABLE>"+ "\n" +
"      <ISSTXPARTY>No</ISSTXPARTY>"+ "\n" +
"      <ISSTXNONREALIZEDTYPE>No</ISSTXNONREALIZEDTYPE>"+ "\n" +
"      <ISUSEDFORCVD>No</ISUSEDFORCVD>"+ "\n" +
"      <LEDBELONGSTONONTAXABLE>No</LEDBELONGSTONONTAXABLE>"+ "\n" +
"      <ISEXCISEMERCHANTEXPORTER>No</ISEXCISEMERCHANTEXPORTER>"+ "\n" +
"      <ISPARTYEXEMPTED>No</ISPARTYEXEMPTED>"+ "\n" +
"      <ISSEZPARTY>No</ISSEZPARTY>"+ "\n" +
"      <TDSDEDUCTEEISSPECIALRATE>No</TDSDEDUCTEEISSPECIALRATE>"+ "\n" +
"      <ISECHEQUESUPPORTED>No</ISECHEQUESUPPORTED>"+ "\n" +
"      <ISEDDSUPPORTED>No</ISEDDSUPPORTED>"+ "\n" +
"      <HASECHEQUEDELIVERYMODE>No</HASECHEQUEDELIVERYMODE>"+ "\n" +
"      <HASECHEQUEDELIVERYTO>No</HASECHEQUEDELIVERYTO>"+ "\n" +
"      <HASECHEQUEPRINTLOCATION>No</HASECHEQUEPRINTLOCATION>"+ "\n" +
"      <HASECHEQUEPAYABLELOCATION>No</HASECHEQUEPAYABLELOCATION>"+ "\n" +
"      <HASECHEQUEBANKLOCATION>No</HASECHEQUEBANKLOCATION>"+ "\n" +
"      <HASEDDDELIVERYMODE>No</HASEDDDELIVERYMODE>"+ "\n" +
"      <HASEDDDELIVERYTO>No</HASEDDDELIVERYTO>"+ "\n" +
"      <HASEDDPRINTLOCATION>No</HASEDDPRINTLOCATION>"+ "\n" +
"      <HASEDDPAYABLELOCATION>No</HASEDDPAYABLELOCATION>"+ "\n" +
"      <HASEDDBANKLOCATION>No</HASEDDBANKLOCATION>"+ "\n" +
"      <ISEBANKINGENABLED>No</ISEBANKINGENABLED>"+ "\n" +
"      <ISEXPORTFILEENCRYPTED>No</ISEXPORTFILEENCRYPTED>"+ "\n" +
"      <ISBATCHENABLED>No</ISBATCHENABLED>"+ "\n" +
"      <ISPRODUCTCODEBASED>No</ISPRODUCTCODEBASED>"+ "\n" +
"      <HASEDDCITY>No</HASEDDCITY>"+ "\n" +
"      <HASECHEQUECITY>No</HASECHEQUECITY>"+ "\n" +
"      <ISFILENAMEFORMATSUPPORTED>No</ISFILENAMEFORMATSUPPORTED>"+ "\n" +
"      <HASCLIENTCODE>No</HASCLIENTCODE>"+ "\n" +
"      <PAYINSISBATCHAPPLICABLE>No</PAYINSISBATCHAPPLICABLE>"+ "\n" +
"      <PAYINSISFILENUMAPP>No</PAYINSISFILENUMAPP>"+ "\n" +
"      <ISSALARYTRANSGROUPEDFORBRS>No</ISSALARYTRANSGROUPEDFORBRS>"+ "\n" +
"      <ISEBANKINGSUPPORTED>No</ISEBANKINGSUPPORTED>"+ "\n" +
"      <ISSCBUAE>No</ISSCBUAE>"+ "\n" +
"      <ISBANKSTATUSAPP>No</ISBANKSTATUSAPP>"+ "\n" +
"      <ISSALARYGROUPED>No</ISSALARYGROUPED>"+ "\n" +
"      <USEFORPURCHASETAX>No</USEFORPURCHASETAX>"+ "\n" +
"      <AUDITED>No</AUDITED>"+ "\n" +
"      <SORTPOSITION> 1000</SORTPOSITION>"+ "\n" +
"      <ALTERID> 251</ALTERID>"+ "\n" +
"      <SERVICETAXDETAILS.LIST>      </SERVICETAXDETAILS.LIST>"+ "\n" +
"      <LBTREGNDETAILS.LIST>      </LBTREGNDETAILS.LIST>"+ "\n" +
"      <VATDETAILS.LIST>      </VATDETAILS.LIST>"+ "\n" +
"      <SALESTAXCESSDETAILS.LIST>      </SALESTAXCESSDETAILS.LIST>"+ "\n" +
"      <GSTDETAILS.LIST>      </GSTDETAILS.LIST>"+ "\n" +
"      <LANGUAGENAME.LIST>"+ "\n" +
"       <NAME.LIST TYPE=\"String\">"+ "\n" +
"        <NAME>#TaxName#</NAME>" + "\n" +
"       </NAME.LIST>"+ "\n" +
"       <LANGUAGEID> 1033</LANGUAGEID>"+ "\n" +
"      </LANGUAGENAME.LIST>"+ "\n" +
"      <XBRLDETAIL.LIST>      </XBRLDETAIL.LIST>"+ "\n" +
"      <AUDITDETAILS.LIST>      </AUDITDETAILS.LIST>"+ "\n" +
"      <SCHVIDETAILS.LIST>      </SCHVIDETAILS.LIST>"+ "\n" +
"      <EXCISETARIFFDETAILS.LIST>      </EXCISETARIFFDETAILS.LIST>"+ "\n" +
"      <TCSCATEGORYDETAILS.LIST>      </TCSCATEGORYDETAILS.LIST>"+ "\n" +
"      <TDSCATEGORYDETAILS.LIST>      </TDSCATEGORYDETAILS.LIST>"+ "\n" +
"      <SLABPERIOD.LIST>      </SLABPERIOD.LIST>"+ "\n" +
"      <GRATUITYPERIOD.LIST>      </GRATUITYPERIOD.LIST>"+ "\n" +
"      <ADDITIONALCOMPUTATIONS.LIST>      </ADDITIONALCOMPUTATIONS.LIST>"+ "\n" +
"      <EXCISEJURISDICTIONDETAILS.LIST>      </EXCISEJURISDICTIONDETAILS.LIST>"+ "\n" +
"      <EXCLUDEDTAXATIONS.LIST>      </EXCLUDEDTAXATIONS.LIST>"+ "\n" +
"      <BANKALLOCATIONS.LIST>      </BANKALLOCATIONS.LIST>"+ "\n" +
"      <PAYMENTDETAILS.LIST>      </PAYMENTDETAILS.LIST>"+ "\n" +
"      <BANKEXPORTFORMATS.LIST>      </BANKEXPORTFORMATS.LIST>"+ "\n" +
"      <BILLALLOCATIONS.LIST>      </BILLALLOCATIONS.LIST>"+ "\n" +
"      <INTERESTCOLLECTION.LIST>      </INTERESTCOLLECTION.LIST>"+ "\n" +
"      <LEDGERCLOSINGVALUES.LIST>      </LEDGERCLOSINGVALUES.LIST>"+ "\n" +
"      <LEDGERAUDITCLASS.LIST>      </LEDGERAUDITCLASS.LIST>"+ "\n" +
"      <OLDAUDITENTRIES.LIST>      </OLDAUDITENTRIES.LIST>"+ "\n" +
"      <TDSEXEMPTIONRULES.LIST>      </TDSEXEMPTIONRULES.LIST>"+ "\n" +
"      <DEDUCTINSAMEVCHRULES.LIST>      </DEDUCTINSAMEVCHRULES.LIST>"+ "\n" +
"      <LOWERDEDUCTION.LIST>      </LOWERDEDUCTION.LIST>"+ "\n" +
"      <STXABATEMENTDETAILS.LIST>      </STXABATEMENTDETAILS.LIST>"+ "\n" +
"      <LEDMULTIADDRESSLIST.LIST>      </LEDMULTIADDRESSLIST.LIST>"+ "\n" +
"      <STXTAXDETAILS.LIST>      </STXTAXDETAILS.LIST>"+ "\n" +
"      <CHEQUERANGE.LIST>      </CHEQUERANGE.LIST>"+ "\n" +
"      <DEFAULTVCHCHEQUEDETAILS.LIST>      </DEFAULTVCHCHEQUEDETAILS.LIST>"+ "\n" +
"      <ACCOUNTAUDITENTRIES.LIST>      </ACCOUNTAUDITENTRIES.LIST>"+ "\n" +
"      <AUDITENTRIES.LIST>      </AUDITENTRIES.LIST>"+ "\n" +
"      <BRSIMPORTEDINFO.LIST>      </BRSIMPORTEDINFO.LIST>"+ "\n" +
"      <AUTOBRSCONFIGS.LIST>      </AUTOBRSCONFIGS.LIST>"+ "\n" +
"      <BANKURENTRIES.LIST>      </BANKURENTRIES.LIST>"+ "\n" +
"      <DEFAULTCHEQUEDETAILS.LIST>      </DEFAULTCHEQUEDETAILS.LIST>"+ "\n" +
"      <DEFAULTOPENINGCHEQUEDETAILS.LIST>      </DEFAULTOPENINGCHEQUEDETAILS.LIST>"+ "\n" +
"      <CANCELLEDPAYALLOCATIONS.LIST>      </CANCELLEDPAYALLOCATIONS.LIST>"+ "\n" +
"      <ECHEQUEPRINTLOCATION.LIST>      </ECHEQUEPRINTLOCATION.LIST>"+ "\n" +
"      <ECHEQUEPAYABLELOCATION.LIST>      </ECHEQUEPAYABLELOCATION.LIST>"+ "\n" +
"      <EDDPRINTLOCATION.LIST>      </EDDPRINTLOCATION.LIST>"+ "\n" +
"      <EDDPAYABLELOCATION.LIST>      </EDDPAYABLELOCATION.LIST>"+ "\n" +
"      <AVAILABLETRANSACTIONTYPES.LIST>      </AVAILABLETRANSACTIONTYPES.LIST>"+ "\n" +
"      <LEDPAYINSCONFIGS.LIST>      </LEDPAYINSCONFIGS.LIST>"+ "\n" +
"      <TYPECODEDETAILS.LIST>      </TYPECODEDETAILS.LIST>"+ "\n" +
"      <FIELDVALIDATIONDETAILS.LIST>      </FIELDVALIDATIONDETAILS.LIST>"+ "\n" +
"      <INPUTCRALLOCS.LIST>      </INPUTCRALLOCS.LIST>"+ "\n" +
"      <GSTCLASSFNIGSTRATES.LIST>      </GSTCLASSFNIGSTRATES.LIST>"+ "\n" +
"      <EXTARIFFDUTYHEADDETAILS.LIST>      </EXTARIFFDUTYHEADDETAILS.LIST>"+ "\n" +
"      <VOUCHERTYPEPRODUCTCODES.LIST>      </VOUCHERTYPEPRODUCTCODES.LIST>"+ "\n" +
"     </LEDGER>"+ "\n" +
"    </TALLYMESSAGE>\n";


         string VoucherExpenseType =
"    <TALLYMESSAGE xmlns:UDF=\"TallyUDF\">"+ "\n" +
"     <LEDGER NAME=\"#ExpenseName#\" RESERVEDNAME=\"\">" + "\n" +
"      <MAILINGNAME.LIST TYPE=\"String\">"+ "\n" +
"       <MAILINGNAME>#ExpenseName#</MAILINGNAME>" + "\n" +
"      </MAILINGNAME.LIST>"+ "\n" +
"      <OLDAUDITENTRYIDS.LIST TYPE=\"Number\">"+ "\n" +
"       <OLDAUDITENTRYIDS>-1</OLDAUDITENTRYIDS>"+ "\n" +
"      </OLDAUDITENTRYIDS.LIST>"+ "\n" +
"      <GUID>#guiduniqueno#</GUID>" + "\n" +
"      <CURRENCYNAME>₹</CURRENCYNAME>"+ "\n" +
"      <PARENT>Direct Expenses</PARENT>"+ "\n" +
"      <GSTAPPLICABLE>&#4; Not Applicable</GSTAPPLICABLE>"+ "\n" +
"      <TAXCLASSIFICATIONNAME/>"+ "\n" +
"      <TAXTYPE>Others</TAXTYPE>"+ "\n" +
"      <LEDADDLALLOCTYPE/>"+ "\n" +
"      <GSTTYPE/>"+ "\n" +
"      <APPROPRIATEFOR/>"+ "\n" +
"      <SERVICECATEGORY>&#4; Not Applicable</SERVICECATEGORY>"+ "\n" +
"      <EXCISELEDGERCLASSIFICATION/>"+ "\n" +
"      <EXCISEDUTYTYPE/>"+ "\n" +
"      <EXCISENATUREOFPURCHASE/>"+ "\n" +
"      <LEDGERFBTCATEGORY/>"+ "\n" +
"      <VATAPPLICABLE>&#4; Not Applicable</VATAPPLICABLE>"+ "\n" +
"      <ISBILLWISEON>Yes</ISBILLWISEON>"+ "\n" +
"      <ISCOSTCENTRESON>Yes</ISCOSTCENTRESON>"+ "\n" +
"      <ISINTERESTON>No</ISINTERESTON>"+ "\n" +
"      <ALLOWINMOBILE>No</ALLOWINMOBILE>"+ "\n" +
"      <ISCOSTTRACKINGON>No</ISCOSTTRACKINGON>"+ "\n" +
"      <ISBENEFICIARYCODEON>No</ISBENEFICIARYCODEON>"+ "\n" +
"      <ISUPDATINGTARGETID>No</ISUPDATINGTARGETID>"+ "\n" +
"      <ASORIGINAL>Yes</ASORIGINAL>"+ "\n" +
"      <ISCONDENSED>No</ISCONDENSED>"+ "\n" +
"      <AFFECTSSTOCK>No</AFFECTSSTOCK>"+ "\n" +
"      <ISRATEINCLUSIVEVAT>No</ISRATEINCLUSIVEVAT>"+ "\n" +
"      <FORPAYROLL>No</FORPAYROLL>"+ "\n" +
"      <ISABCENABLED>No</ISABCENABLED>"+ "\n" +
"      <ISCREDITDAYSCHKON>No</ISCREDITDAYSCHKON>"+ "\n" +
"      <INTERESTONBILLWISE>No</INTERESTONBILLWISE>"+ "\n" +
"      <OVERRIDEINTEREST>No</OVERRIDEINTEREST>"+ "\n" +
"      <OVERRIDEADVINTEREST>No</OVERRIDEADVINTEREST>"+ "\n" +
"      <USEFORVAT>No</USEFORVAT>"+ "\n" +
"      <IGNORETDSEXEMPT>No</IGNORETDSEXEMPT>"+ "\n" +
"      <ISTCSAPPLICABLE>No</ISTCSAPPLICABLE>"+ "\n" +
"      <ISTDSAPPLICABLE>No</ISTDSAPPLICABLE>"+ "\n" +
"      <ISFBTAPPLICABLE>No</ISFBTAPPLICABLE>"+ "\n" +
"      <ISGSTAPPLICABLE>No</ISGSTAPPLICABLE>"+ "\n" +
"      <ISEXCISEAPPLICABLE>No</ISEXCISEAPPLICABLE>"+ "\n" +
"      <ISTDSEXPENSE>No</ISTDSEXPENSE>"+ "\n" +
"      <ISEDLIAPPLICABLE>No</ISEDLIAPPLICABLE>"+ "\n" +
"      <ISRELATEDPARTY>No</ISRELATEDPARTY>"+ "\n" +
"      <USEFORESIELIGIBILITY>No</USEFORESIELIGIBILITY>"+ "\n" +
"      <ISINTERESTINCLLASTDAY>No</ISINTERESTINCLLASTDAY>"+ "\n" +
"      <APPROPRIATETAXVALUE>No</APPROPRIATETAXVALUE>"+ "\n" +
"      <ISBEHAVEASDUTY>No</ISBEHAVEASDUTY>"+ "\n" +
"      <INTERESTINCLDAYOFADDITION>No</INTERESTINCLDAYOFADDITION>"+ "\n" +
"      <INTERESTINCLDAYOFDEDUCTION>No</INTERESTINCLDAYOFDEDUCTION>"+ "\n" +
"      <OVERRIDECREDITLIMIT>No</OVERRIDECREDITLIMIT>"+ "\n" +
"      <ISAGAINSTFORMC>No</ISAGAINSTFORMC>"+ "\n" +
"      <ISCHEQUEPRINTINGENABLED>Yes</ISCHEQUEPRINTINGENABLED>"+ "\n" +
"      <ISPAYUPLOAD>No</ISPAYUPLOAD>"+ "\n" +
"      <ISPAYBATCHONLYSAL>No</ISPAYBATCHONLYSAL>"+ "\n" +
"      <ISBNFCODESUPPORTED>No</ISBNFCODESUPPORTED>"+ "\n" +
"      <ALLOWEXPORTWITHERRORS>No</ALLOWEXPORTWITHERRORS>"+ "\n" +
"      <USEFORNOTIONALITC>No</USEFORNOTIONALITC>"+ "\n" +
"      <ISECOMMOPERATOR>No</ISECOMMOPERATOR>"+ "\n" +
"      <SHOWINPAYSLIP>No</SHOWINPAYSLIP>"+ "\n" +
"      <USEFORGRATUITY>No</USEFORGRATUITY>"+ "\n" +
"      <ISTDSPROJECTED>No</ISTDSPROJECTED>"+ "\n" +
"      <FORSERVICETAX>No</FORSERVICETAX>"+ "\n" +
"      <ISINPUTCREDIT>No</ISINPUTCREDIT>"+ "\n" +
"      <ISEXEMPTED>No</ISEXEMPTED>"+ "\n" +
"      <ISABATEMENTAPPLICABLE>No</ISABATEMENTAPPLICABLE>"+ "\n" +
"      <ISSTXPARTY>No</ISSTXPARTY>"+ "\n" +
"      <ISSTXNONREALIZEDTYPE>No</ISSTXNONREALIZEDTYPE>"+ "\n" +
"      <ISUSEDFORCVD>No</ISUSEDFORCVD>"+ "\n" +
"      <LEDBELONGSTONONTAXABLE>No</LEDBELONGSTONONTAXABLE>"+ "\n" +
"      <ISEXCISEMERCHANTEXPORTER>No</ISEXCISEMERCHANTEXPORTER>"+ "\n" +
"      <ISPARTYEXEMPTED>No</ISPARTYEXEMPTED>"+ "\n" +
"      <ISSEZPARTY>No</ISSEZPARTY>"+ "\n" +
"      <TDSDEDUCTEEISSPECIALRATE>No</TDSDEDUCTEEISSPECIALRATE>"+ "\n" +
"      <ISECHEQUESUPPORTED>No</ISECHEQUESUPPORTED>"+ "\n" +
"      <ISEDDSUPPORTED>No</ISEDDSUPPORTED>"+ "\n" +
"      <HASECHEQUEDELIVERYMODE>No</HASECHEQUEDELIVERYMODE>"+ "\n" +
"      <HASECHEQUEDELIVERYTO>No</HASECHEQUEDELIVERYTO>"+ "\n" +
"      <HASECHEQUEPRINTLOCATION>No</HASECHEQUEPRINTLOCATION>"+ "\n" +
"      <HASECHEQUEPAYABLELOCATION>No</HASECHEQUEPAYABLELOCATION>"+ "\n" +
"      <HASECHEQUEBANKLOCATION>No</HASECHEQUEBANKLOCATION>"+ "\n" +
"      <HASEDDDELIVERYMODE>No</HASEDDDELIVERYMODE>"+ "\n" +
"      <HASEDDDELIVERYTO>No</HASEDDDELIVERYTO>"+ "\n" +
"      <HASEDDPRINTLOCATION>No</HASEDDPRINTLOCATION>"+ "\n" +
"      <HASEDDPAYABLELOCATION>No</HASEDDPAYABLELOCATION>"+ "\n" +
"      <HASEDDBANKLOCATION>No</HASEDDBANKLOCATION>"+ "\n" +
"      <ISEBANKINGENABLED>No</ISEBANKINGENABLED>"+ "\n" +
"      <ISEXPORTFILEENCRYPTED>No</ISEXPORTFILEENCRYPTED>"+ "\n" +
"      <ISBATCHENABLED>No</ISBATCHENABLED>"+ "\n" +
"      <ISPRODUCTCODEBASED>No</ISPRODUCTCODEBASED>"+ "\n" +
"      <HASEDDCITY>No</HASEDDCITY>"+ "\n" +
"      <HASECHEQUECITY>No</HASECHEQUECITY>"+ "\n" +
"      <ISFILENAMEFORMATSUPPORTED>No</ISFILENAMEFORMATSUPPORTED>"+ "\n" +
"      <HASCLIENTCODE>No</HASCLIENTCODE>"+ "\n" +
"      <PAYINSISBATCHAPPLICABLE>No</PAYINSISBATCHAPPLICABLE>"+ "\n" +
"      <PAYINSISFILENUMAPP>No</PAYINSISFILENUMAPP>"+ "\n" +
"      <ISSALARYTRANSGROUPEDFORBRS>No</ISSALARYTRANSGROUPEDFORBRS>"+ "\n" +
"      <ISEBANKINGSUPPORTED>No</ISEBANKINGSUPPORTED>"+ "\n" +
"      <ISSCBUAE>No</ISSCBUAE>"+ "\n" +
"      <ISBANKSTATUSAPP>No</ISBANKSTATUSAPP>"+ "\n" +
"      <ISSALARYGROUPED>No</ISSALARYGROUPED>"+ "\n" +
"      <USEFORPURCHASETAX>No</USEFORPURCHASETAX>"+ "\n" +
"      <AUDITED>No</AUDITED>"+ "\n" +
"      <SORTPOSITION> 1000</SORTPOSITION>"+ "\n" +
"      <ALTERID> 210</ALTERID>"+ "\n" +
"      <SERVICETAXDETAILS.LIST>      </SERVICETAXDETAILS.LIST>"+ "\n" +
"      <LBTREGNDETAILS.LIST>      </LBTREGNDETAILS.LIST>"+ "\n" +
"      <VATDETAILS.LIST>      </VATDETAILS.LIST>"+ "\n" +
"      <SALESTAXCESSDETAILS.LIST>      </SALESTAXCESSDETAILS.LIST>"+ "\n" +
"      <GSTDETAILS.LIST>      </GSTDETAILS.LIST>"+ "\n" +
"      <LANGUAGENAME.LIST>"+ "\n" +
"       <NAME.LIST TYPE=\"String\">"+ "\n" +
"        <NAME>#ExpenseName#</NAME>" + "\n" +
"       </NAME.LIST>"+ "\n" +
"       <LANGUAGEID> 1033</LANGUAGEID>"+ "\n" +
"      </LANGUAGENAME.LIST>"+ "\n" +
"      <XBRLDETAIL.LIST>      </XBRLDETAIL.LIST>"+ "\n" +
"      <AUDITDETAILS.LIST>      </AUDITDETAILS.LIST>"+ "\n" +
"      <SCHVIDETAILS.LIST>      </SCHVIDETAILS.LIST>"+ "\n" +
"      <EXCISETARIFFDETAILS.LIST>      </EXCISETARIFFDETAILS.LIST>"+ "\n" +
"      <TCSCATEGORYDETAILS.LIST>      </TCSCATEGORYDETAILS.LIST>"+ "\n" +
"      <TDSCATEGORYDETAILS.LIST>      </TDSCATEGORYDETAILS.LIST>"+ "\n" +
"      <SLABPERIOD.LIST>      </SLABPERIOD.LIST>"+ "\n" +
"      <GRATUITYPERIOD.LIST>      </GRATUITYPERIOD.LIST>"+ "\n" +
"      <ADDITIONALCOMPUTATIONS.LIST>      </ADDITIONALCOMPUTATIONS.LIST>"+ "\n" +
"      <EXCISEJURISDICTIONDETAILS.LIST>      </EXCISEJURISDICTIONDETAILS.LIST>"+ "\n" +
"      <EXCLUDEDTAXATIONS.LIST>      </EXCLUDEDTAXATIONS.LIST>"+ "\n" +
"      <BANKALLOCATIONS.LIST>      </BANKALLOCATIONS.LIST>"+ "\n" +
"      <PAYMENTDETAILS.LIST>      </PAYMENTDETAILS.LIST>"+ "\n" +
"      <BANKEXPORTFORMATS.LIST>      </BANKEXPORTFORMATS.LIST>"+ "\n" +
"      <BILLALLOCATIONS.LIST>      </BILLALLOCATIONS.LIST>"+ "\n" +
"      <INTERESTCOLLECTION.LIST>      </INTERESTCOLLECTION.LIST>"+ "\n" +
"      <LEDGERCLOSINGVALUES.LIST>      </LEDGERCLOSINGVALUES.LIST>"+ "\n" +
"      <LEDGERAUDITCLASS.LIST>      </LEDGERAUDITCLASS.LIST>"+ "\n" +
"      <OLDAUDITENTRIES.LIST>      </OLDAUDITENTRIES.LIST>"+ "\n" +
"      <TDSEXEMPTIONRULES.LIST>      </TDSEXEMPTIONRULES.LIST>"+ "\n" +
"      <DEDUCTINSAMEVCHRULES.LIST>      </DEDUCTINSAMEVCHRULES.LIST>"+ "\n" +
"      <LOWERDEDUCTION.LIST>      </LOWERDEDUCTION.LIST>"+ "\n" +
"      <STXABATEMENTDETAILS.LIST>      </STXABATEMENTDETAILS.LIST>"+ "\n" +
"      <LEDMULTIADDRESSLIST.LIST>      </LEDMULTIADDRESSLIST.LIST>"+ "\n" +
"      <STXTAXDETAILS.LIST>      </STXTAXDETAILS.LIST>"+ "\n" +
"      <CHEQUERANGE.LIST>      </CHEQUERANGE.LIST>"+ "\n" +
"      <DEFAULTVCHCHEQUEDETAILS.LIST>      </DEFAULTVCHCHEQUEDETAILS.LIST>"+ "\n" +
"      <ACCOUNTAUDITENTRIES.LIST>      </ACCOUNTAUDITENTRIES.LIST>"+ "\n" +
"      <AUDITENTRIES.LIST>      </AUDITENTRIES.LIST>"+ "\n" +
"      <BRSIMPORTEDINFO.LIST>      </BRSIMPORTEDINFO.LIST>"+ "\n" +
"      <AUTOBRSCONFIGS.LIST>      </AUTOBRSCONFIGS.LIST>"+ "\n" +
"      <BANKURENTRIES.LIST>      </BANKURENTRIES.LIST>"+ "\n" +
"      <DEFAULTCHEQUEDETAILS.LIST>      </DEFAULTCHEQUEDETAILS.LIST>"+ "\n" +
"      <DEFAULTOPENINGCHEQUEDETAILS.LIST>      </DEFAULTOPENINGCHEQUEDETAILS.LIST>"+ "\n" +
"      <CANCELLEDPAYALLOCATIONS.LIST>      </CANCELLEDPAYALLOCATIONS.LIST>"+ "\n" +
"      <ECHEQUEPRINTLOCATION.LIST>      </ECHEQUEPRINTLOCATION.LIST>"+ "\n" +
"      <ECHEQUEPAYABLELOCATION.LIST>      </ECHEQUEPAYABLELOCATION.LIST>"+ "\n" +
"      <EDDPRINTLOCATION.LIST>      </EDDPRINTLOCATION.LIST>"+ "\n" +
"      <EDDPAYABLELOCATION.LIST>      </EDDPAYABLELOCATION.LIST>"+ "\n" +
"      <AVAILABLETRANSACTIONTYPES.LIST>      </AVAILABLETRANSACTIONTYPES.LIST>"+ "\n" +
"      <LEDPAYINSCONFIGS.LIST>      </LEDPAYINSCONFIGS.LIST>"+ "\n" +
"      <TYPECODEDETAILS.LIST>      </TYPECODEDETAILS.LIST>"+ "\n" +
"      <FIELDVALIDATIONDETAILS.LIST>      </FIELDVALIDATIONDETAILS.LIST>"+ "\n" +
"      <INPUTCRALLOCS.LIST>      </INPUTCRALLOCS.LIST>"+ "\n" +
"      <GSTCLASSFNIGSTRATES.LIST>      </GSTCLASSFNIGSTRATES.LIST>"+ "\n" +
"      <EXTARIFFDUTYHEADDETAILS.LIST>      </EXTARIFFDUTYHEADDETAILS.LIST>"+ "\n" +
"      <VOUCHERTYPEPRODUCTCODES.LIST>      </VOUCHERTYPEPRODUCTCODES.LIST>"+ "\n" +
"     </LEDGER>" + "\n" +
"    </TALLYMESSAGE>\n";

        string VoucherCurrentReserveAmount = 
"    <TALLYMESSAGE xmlns:UDF=\"TallyUDF\">" + "\n" +
"     <LEDGER NAME=\"Current Reserve Amount\" RESERVEDNAME=\"\">" + "\n" +
"      <MAILINGNAME.LIST TYPE=\"String\">" + "\n" +
"       <MAILINGNAME>Current Reserve Amount</MAILINGNAME>" + "\n" +
"      </MAILINGNAME.LIST>" + "\n" +
"      <OLDAUDITENTRYIDS.LIST TYPE=\"Number\">" + "\n" +
"       <OLDAUDITENTRYIDS>-1</OLDAUDITENTRYIDS>" + "\n" +
"      </OLDAUDITENTRYIDS.LIST>" + "\n" +
"      <GUID>df13ea43-535b-411e-a3ae-09a30f52223d-000000ab</GUID>" + "\n" +
"      <CURRENCYNAME>₹</CURRENCYNAME>" + "\n" +
"      <COUNTRYNAME>India</COUNTRYNAME>" + "\n" +
"      <PARENT>Sundry Debtors</PARENT>" + "\n" +
"      <GSTAPPLICABLE>&#4; Applicable</GSTAPPLICABLE>" + "\n" +
"      <TAXCLASSIFICATIONNAME/>" + "\n" +
"      <TAXTYPE>Others</TAXTYPE>" + "\n" +
"      <COUNTRYOFRESIDENCE>India</COUNTRYOFRESIDENCE>" + "\n" +
"      <LEDADDLALLOCTYPE/>" + "\n" +
"      <GSTTYPE/>" + "\n" +
"      <APPROPRIATEFOR/>" + "\n" +
"      <GSTTYPEOFSUPPLY>Services</GSTTYPEOFSUPPLY>" + "\n" +
"      <SERVICECATEGORY>&#4; Not Applicable</SERVICECATEGORY>" + "\n" +
"      <EXCISELEDGERCLASSIFICATION/>" + "\n" +
"      <EXCISEDUTYTYPE/>" + "\n" +
"      <EXCISENATUREOFPURCHASE/>" + "\n" +
"      <LEDGERFBTCATEGORY/>" + "\n" +
"      <LEDSTATENAME/>" + "\n" +
"      <ISBILLWISEON>Yes</ISBILLWISEON>" + "\n" +
"      <ISCOSTCENTRESON>No</ISCOSTCENTRESON>" + "\n" +
"      <ISINTERESTON>No</ISINTERESTON>" + "\n" +
"      <ALLOWINMOBILE>No</ALLOWINMOBILE>" + "\n" +
"      <ISCOSTTRACKINGON>No</ISCOSTTRACKINGON>" + "\n" +
"      <ISBENEFICIARYCODEON>No</ISBENEFICIARYCODEON>" + "\n" +
"      <ISUPDATINGTARGETID>No</ISUPDATINGTARGETID>" + "\n" +
"      <ASORIGINAL>Yes</ASORIGINAL>" + "\n" +
"      <ISCONDENSED>No</ISCONDENSED>" + "\n" +
"      <AFFECTSSTOCK>No</AFFECTSSTOCK>" + "\n" +
"      <ISRATEINCLUSIVEVAT>No</ISRATEINCLUSIVEVAT>" + "\n" +
"      <FORPAYROLL>No</FORPAYROLL>" + "\n" +
"      <ISABCENABLED>No</ISABCENABLED>" + "\n" +
"      <ISCREDITDAYSCHKON>No</ISCREDITDAYSCHKON>" + "\n" +
"      <INTERESTONBILLWISE>No</INTERESTONBILLWISE>" + "\n" +
"      <OVERRIDEINTEREST>No</OVERRIDEINTEREST>" + "\n" +
"      <OVERRIDEADVINTEREST>No</OVERRIDEADVINTEREST>" + "\n" +
"      <USEFORVAT>No</USEFORVAT>" + "\n" +
"      <IGNORETDSEXEMPT>No</IGNORETDSEXEMPT>" + "\n" +
"      <ISTCSAPPLICABLE>No</ISTCSAPPLICABLE>" + "\n" +
"      <ISTDSAPPLICABLE>No</ISTDSAPPLICABLE>" + "\n" +
"      <ISFBTAPPLICABLE>No</ISFBTAPPLICABLE>" + "\n" +
"      <ISGSTAPPLICABLE>No</ISGSTAPPLICABLE>" + "\n" +
"      <ISEXCISEAPPLICABLE>No</ISEXCISEAPPLICABLE>" + "\n" +
"      <ISTDSEXPENSE>No</ISTDSEXPENSE>" + "\n" +
"      <ISEDLIAPPLICABLE>No</ISEDLIAPPLICABLE>" + "\n" +
"      <ISRELATEDPARTY>No</ISRELATEDPARTY>" + "\n" +
"      <USEFORESIELIGIBILITY>No</USEFORESIELIGIBILITY>" + "\n" +
"      <ISINTERESTINCLLASTDAY>No</ISINTERESTINCLLASTDAY>" + "\n" +
"      <APPROPRIATETAXVALUE>No</APPROPRIATETAXVALUE>" + "\n" +
"      <ISBEHAVEASDUTY>No</ISBEHAVEASDUTY>" + "\n" +
"      <INTERESTINCLDAYOFADDITION>No</INTERESTINCLDAYOFADDITION>" + "\n" +
"      <INTERESTINCLDAYOFDEDUCTION>No</INTERESTINCLDAYOFDEDUCTION>" + "\n" +
"      <OVERRIDECREDITLIMIT>No</OVERRIDECREDITLIMIT>" + "\n" +
"      <ISAGAINSTFORMC>No</ISAGAINSTFORMC>" + "\n" +
"      <ISCHEQUEPRINTINGENABLED>Yes</ISCHEQUEPRINTINGENABLED>" + "\n" +
"      <ISPAYUPLOAD>No</ISPAYUPLOAD>" + "\n" +
"      <ISPAYBATCHONLYSAL>No</ISPAYBATCHONLYSAL>" + "\n" +
"      <ISBNFCODESUPPORTED>No</ISBNFCODESUPPORTED>" + "\n" +
"      <ALLOWEXPORTWITHERRORS>No</ALLOWEXPORTWITHERRORS>" + "\n" +
"      <USEFORNOTIONALITC>No</USEFORNOTIONALITC>" + "\n" +
"      <ISECOMMOPERATOR>No</ISECOMMOPERATOR>" + "\n" +
"      <SHOWINPAYSLIP>No</SHOWINPAYSLIP>" + "\n" +
"      <USEFORGRATUITY>No</USEFORGRATUITY>" + "\n" +
"      <ISTDSPROJECTED>No</ISTDSPROJECTED>" + "\n" +
"      <FORSERVICETAX>No</FORSERVICETAX>" + "\n" +
"      <ISINPUTCREDIT>No</ISINPUTCREDIT>" + "\n" +
"      <ISEXEMPTED>No</ISEXEMPTED>" + "\n" +
"      <ISABATEMENTAPPLICABLE>No</ISABATEMENTAPPLICABLE>" + "\n" +
"      <ISSTXPARTY>No</ISSTXPARTY>" + "\n" +
"      <ISSTXNONREALIZEDTYPE>No</ISSTXNONREALIZEDTYPE>" + "\n" +
"      <ISUSEDFORCVD>No</ISUSEDFORCVD>" + "\n" +
"      <LEDBELONGSTONONTAXABLE>No</LEDBELONGSTONONTAXABLE>" + "\n" +
"      <ISEXCISEMERCHANTEXPORTER>No</ISEXCISEMERCHANTEXPORTER>" + "\n" +
"      <ISPARTYEXEMPTED>No</ISPARTYEXEMPTED>" + "\n" +
"      <ISSEZPARTY>No</ISSEZPARTY>" + "\n" +
"      <TDSDEDUCTEEISSPECIALRATE>No</TDSDEDUCTEEISSPECIALRATE>" + "\n" +
"      <ISECHEQUESUPPORTED>No</ISECHEQUESUPPORTED>" + "\n" +
"      <ISEDDSUPPORTED>No</ISEDDSUPPORTED>" + "\n" +
"      <HASECHEQUEDELIVERYMODE>No</HASECHEQUEDELIVERYMODE>" + "\n" +
"      <HASECHEQUEDELIVERYTO>No</HASECHEQUEDELIVERYTO>" + "\n" +
"      <HASECHEQUEPRINTLOCATION>No</HASECHEQUEPRINTLOCATION>" + "\n" +
"      <HASECHEQUEPAYABLELOCATION>No</HASECHEQUEPAYABLELOCATION>" + "\n" +
"      <HASECHEQUEBANKLOCATION>No</HASECHEQUEBANKLOCATION>" + "\n" +
"      <HASEDDDELIVERYMODE>No</HASEDDDELIVERYMODE>" + "\n" +
"      <HASEDDDELIVERYTO>No</HASEDDDELIVERYTO>" + "\n" +
"      <HASEDDPRINTLOCATION>No</HASEDDPRINTLOCATION>" + "\n" +
"      <HASEDDPAYABLELOCATION>No</HASEDDPAYABLELOCATION>" + "\n" +
"      <HASEDDBANKLOCATION>No</HASEDDBANKLOCATION>" + "\n" +
"      <ISEBANKINGENABLED>No</ISEBANKINGENABLED>" + "\n" +
"      <ISEXPORTFILEENCRYPTED>No</ISEXPORTFILEENCRYPTED>" + "\n" +
"      <ISBATCHENABLED>No</ISBATCHENABLED>" + "\n" +
"      <ISPRODUCTCODEBASED>No</ISPRODUCTCODEBASED>" + "\n" +
"      <HASEDDCITY>No</HASEDDCITY>" + "\n" +
"      <HASECHEQUECITY>No</HASECHEQUECITY>" + "\n" +
"      <ISFILENAMEFORMATSUPPORTED>No</ISFILENAMEFORMATSUPPORTED>" + "\n" +
"      <HASCLIENTCODE>No</HASCLIENTCODE>" + "\n" +
"      <PAYINSISBATCHAPPLICABLE>No</PAYINSISBATCHAPPLICABLE>" + "\n" +
"      <PAYINSISFILENUMAPP>No</PAYINSISFILENUMAPP>" + "\n" +
"      <ISSALARYTRANSGROUPEDFORBRS>No</ISSALARYTRANSGROUPEDFORBRS>" + "\n" +
"      <ISEBANKINGSUPPORTED>No</ISEBANKINGSUPPORTED>" + "\n" +
"      <ISSCBUAE>No</ISSCBUAE>" + "\n" +
"      <ISBANKSTATUSAPP>No</ISBANKSTATUSAPP>" + "\n" +
"      <ISSALARYGROUPED>No</ISSALARYGROUPED>" + "\n" +
"      <USEFORPURCHASETAX>No</USEFORPURCHASETAX>" + "\n" +
"      <AUDITED>No</AUDITED>" + "\n" +
"      <SORTPOSITION> 1000</SORTPOSITION>" + "\n" +
"      <ALTERID> 273</ALTERID>" + "\n" +
"      <SERVICETAXDETAILS.LIST>      </SERVICETAXDETAILS.LIST>" + "\n" +
"      <LBTREGNDETAILS.LIST>      </LBTREGNDETAILS.LIST>" + "\n" +
"      <VATDETAILS.LIST>      </VATDETAILS.LIST>" + "\n" +
"      <SALESTAXCESSDETAILS.LIST>      </SALESTAXCESSDETAILS.LIST>" + "\n" +
"      <GSTDETAILS.LIST>      </GSTDETAILS.LIST>" + "\n" +
"      <LANGUAGENAME.LIST>" + "\n" +
"       <NAME.LIST TYPE=\"String\">" + "\n" +
"        <NAME>Current Reserve Amount</NAME>" + "\n" +
"       </NAME.LIST>" + "\n" +
"       <LANGUAGEID> 1033</LANGUAGEID>" + "\n" +
"      </LANGUAGENAME.LIST>" + "\n" +
"      <XBRLDETAIL.LIST>      </XBRLDETAIL.LIST>" + "\n" +
"      <AUDITDETAILS.LIST>      </AUDITDETAILS.LIST>" + "\n" +
"      <SCHVIDETAILS.LIST>      </SCHVIDETAILS.LIST>" + "\n" +
"      <EXCISETARIFFDETAILS.LIST>      </EXCISETARIFFDETAILS.LIST>" + "\n" +
"      <TCSCATEGORYDETAILS.LIST>      </TCSCATEGORYDETAILS.LIST>" + "\n" +
"      <TDSCATEGORYDETAILS.LIST>      </TDSCATEGORYDETAILS.LIST>" + "\n" +
"      <SLABPERIOD.LIST>      </SLABPERIOD.LIST>" + "\n" +
"      <GRATUITYPERIOD.LIST>      </GRATUITYPERIOD.LIST>" + "\n" +
"      <ADDITIONALCOMPUTATIONS.LIST>      </ADDITIONALCOMPUTATIONS.LIST>" + "\n" +
"      <EXCISEJURISDICTIONDETAILS.LIST>      </EXCISEJURISDICTIONDETAILS.LIST>" + "\n" +
"      <EXCLUDEDTAXATIONS.LIST>      </EXCLUDEDTAXATIONS.LIST>" + "\n" +
"      <BANKALLOCATIONS.LIST>      </BANKALLOCATIONS.LIST>" + "\n" +
"      <PAYMENTDETAILS.LIST>      </PAYMENTDETAILS.LIST>" + "\n" +
"      <BANKEXPORTFORMATS.LIST>      </BANKEXPORTFORMATS.LIST>" + "\n" +
"      <BILLALLOCATIONS.LIST>      </BILLALLOCATIONS.LIST>" + "\n" +
"      <INTERESTCOLLECTION.LIST>      </INTERESTCOLLECTION.LIST>" + "\n" +
"      <LEDGERCLOSINGVALUES.LIST>      </LEDGERCLOSINGVALUES.LIST>" + "\n" +
"      <LEDGERAUDITCLASS.LIST>      </LEDGERAUDITCLASS.LIST>" + "\n" +
"      <OLDAUDITENTRIES.LIST>      </OLDAUDITENTRIES.LIST>" + "\n" +
"      <TDSEXEMPTIONRULES.LIST>      </TDSEXEMPTIONRULES.LIST>" + "\n" +
"      <DEDUCTINSAMEVCHRULES.LIST>      </DEDUCTINSAMEVCHRULES.LIST>" + "\n" +
"      <LOWERDEDUCTION.LIST>      </LOWERDEDUCTION.LIST>" + "\n" +
"      <STXABATEMENTDETAILS.LIST>      </STXABATEMENTDETAILS.LIST>" + "\n" +
"      <LEDMULTIADDRESSLIST.LIST>      </LEDMULTIADDRESSLIST.LIST>" + "\n" +
"      <STXTAXDETAILS.LIST>      </STXTAXDETAILS.LIST>" + "\n" +
"      <CHEQUERANGE.LIST>      </CHEQUERANGE.LIST>" + "\n" +
"      <DEFAULTVCHCHEQUEDETAILS.LIST>      </DEFAULTVCHCHEQUEDETAILS.LIST>" + "\n" +
"      <ACCOUNTAUDITENTRIES.LIST>      </ACCOUNTAUDITENTRIES.LIST>" + "\n" +
"      <AUDITENTRIES.LIST>      </AUDITENTRIES.LIST>" + "\n" +
"      <BRSIMPORTEDINFO.LIST>      </BRSIMPORTEDINFO.LIST>" + "\n" +
"      <AUTOBRSCONFIGS.LIST>      </AUTOBRSCONFIGS.LIST>" + "\n" +
"      <BANKURENTRIES.LIST>      </BANKURENTRIES.LIST>" + "\n" +
"      <DEFAULTCHEQUEDETAILS.LIST>      </DEFAULTCHEQUEDETAILS.LIST>" + "\n" +
"      <DEFAULTOPENINGCHEQUEDETAILS.LIST>      </DEFAULTOPENINGCHEQUEDETAILS.LIST>" + "\n" +
"      <CANCELLEDPAYALLOCATIONS.LIST>      </CANCELLEDPAYALLOCATIONS.LIST>" + "\n" +
"      <ECHEQUEPRINTLOCATION.LIST>      </ECHEQUEPRINTLOCATION.LIST>" + "\n" +
"      <ECHEQUEPAYABLELOCATION.LIST>      </ECHEQUEPAYABLELOCATION.LIST>" + "\n" +
"      <EDDPRINTLOCATION.LIST>      </EDDPRINTLOCATION.LIST>" + "\n" +
"      <EDDPAYABLELOCATION.LIST>      </EDDPAYABLELOCATION.LIST>" + "\n" +
"      <AVAILABLETRANSACTIONTYPES.LIST>      </AVAILABLETRANSACTIONTYPES.LIST>" + "\n" +
"      <LEDPAYINSCONFIGS.LIST>      </LEDPAYINSCONFIGS.LIST>" + "\n" +
"      <TYPECODEDETAILS.LIST>      </TYPECODEDETAILS.LIST>" + "\n" +
"      <FIELDVALIDATIONDETAILS.LIST>      </FIELDVALIDATIONDETAILS.LIST>" + "\n" +
"      <INPUTCRALLOCS.LIST>      </INPUTCRALLOCS.LIST>" + "\n" +
"      <GSTCLASSFNIGSTRATES.LIST>      </GSTCLASSFNIGSTRATES.LIST>" + "\n" +
"      <EXTARIFFDUTYHEADDETAILS.LIST>      </EXTARIFFDUTYHEADDETAILS.LIST>" + "\n" +
"      <VOUCHERTYPEPRODUCTCODES.LIST>      </VOUCHERTYPEPRODUCTCODES.LIST>" + "\n" +
"     </LEDGER>" + "\n" +
"    </TALLYMESSAGE>\n";

        string VoucherNonSubscription =
"    <TALLYMESSAGE xmlns:UDF=\"TallyUDF\">" + "\n" +
"     <LEDGER NAME=\"NonSubscriptionFeeAdj\" RESERVEDNAME=\"\">" + "\n" +
"      <MAILINGNAME.LIST TYPE=\"String\">" + "\n" +
"       <MAILINGNAME>NonSubscriptionFeeAdj</MAILINGNAME>" + "\n" +
"      </MAILINGNAME.LIST>" + "\n" +
"      <OLDAUDITENTRYIDS.LIST TYPE=\"Number\">" + "\n" +
"       <OLDAUDITENTRYIDS>-1</OLDAUDITENTRYIDS>" + "\n" +
"      </OLDAUDITENTRYIDS.LIST>" + "\n" +
"      <GUID>df13ea43-535b-411e-a3ae-09a30f52223d-000000ad</GUID>" + "\n" +
"      <CURRENCYNAME>₹</CURRENCYNAME>" + "\n" +
"      <PARENT>Direct Expenses</PARENT>" + "\n" +
"      <GSTAPPLICABLE>&#4; Not Applicable</GSTAPPLICABLE>" + "\n" +
"      <TAXCLASSIFICATIONNAME/>" + "\n" +
"      <TAXTYPE>Others</TAXTYPE>" + "\n" +
"      <LEDADDLALLOCTYPE/>" + "\n" +
"      <GSTTYPE/>" + "\n" +
"      <APPROPRIATEFOR/>" + "\n" +
"      <SERVICECATEGORY>&#4; Not Applicable</SERVICECATEGORY>" + "\n" +
"      <EXCISELEDGERCLASSIFICATION/>" + "\n" +
"      <EXCISEDUTYTYPE/>" + "\n" +
"      <EXCISENATUREOFPURCHASE/>" + "\n" +
"      <LEDGERFBTCATEGORY/>" + "\n" +
"      <VATAPPLICABLE>&#4; Not Applicable</VATAPPLICABLE>" + "\n" +
"      <ISBILLWISEON>Yes</ISBILLWISEON>" + "\n" +
"      <ISCOSTCENTRESON>Yes</ISCOSTCENTRESON>" + "\n" +
"      <ISINTERESTON>No</ISINTERESTON>" + "\n" +
"      <ALLOWINMOBILE>No</ALLOWINMOBILE>" + "\n" +
"      <ISCOSTTRACKINGON>No</ISCOSTTRACKINGON>" + "\n" +
"      <ISBENEFICIARYCODEON>No</ISBENEFICIARYCODEON>" + "\n" +
"      <ISUPDATINGTARGETID>No</ISUPDATINGTARGETID>" + "\n" +
"      <ASORIGINAL>Yes</ASORIGINAL>" + "\n" +
"      <ISCONDENSED>No</ISCONDENSED>" + "\n" +
"      <AFFECTSSTOCK>No</AFFECTSSTOCK>" + "\n" +
"      <ISRATEINCLUSIVEVAT>No</ISRATEINCLUSIVEVAT>" + "\n" +
"      <FORPAYROLL>No</FORPAYROLL>" + "\n" +
"      <ISABCENABLED>No</ISABCENABLED>" + "\n" +
"      <ISCREDITDAYSCHKON>No</ISCREDITDAYSCHKON>" + "\n" +
"      <INTERESTONBILLWISE>No</INTERESTONBILLWISE>" + "\n" +
"      <OVERRIDEINTEREST>No</OVERRIDEINTEREST>" + "\n" +
"      <OVERRIDEADVINTEREST>No</OVERRIDEADVINTEREST>" + "\n" +
"      <USEFORVAT>No</USEFORVAT>" + "\n" +
"      <IGNORETDSEXEMPT>No</IGNORETDSEXEMPT>" + "\n" +
"      <ISTCSAPPLICABLE>No</ISTCSAPPLICABLE>" + "\n" +
"      <ISTDSAPPLICABLE>No</ISTDSAPPLICABLE>" + "\n" +
"      <ISFBTAPPLICABLE>No</ISFBTAPPLICABLE>" + "\n" +
"      <ISGSTAPPLICABLE>No</ISGSTAPPLICABLE>" + "\n" +
"      <ISEXCISEAPPLICABLE>No</ISEXCISEAPPLICABLE>" + "\n" +
"      <ISTDSEXPENSE>No</ISTDSEXPENSE>" + "\n" +
"      <ISEDLIAPPLICABLE>No</ISEDLIAPPLICABLE>" + "\n" +
"      <ISRELATEDPARTY>No</ISRELATEDPARTY>" + "\n" +
"      <USEFORESIELIGIBILITY>No</USEFORESIELIGIBILITY>" + "\n" +
"      <ISINTERESTINCLLASTDAY>No</ISINTERESTINCLLASTDAY>" + "\n" +
"      <APPROPRIATETAXVALUE>No</APPROPRIATETAXVALUE>" + "\n" +
"      <ISBEHAVEASDUTY>No</ISBEHAVEASDUTY>" + "\n" +
"      <INTERESTINCLDAYOFADDITION>No</INTERESTINCLDAYOFADDITION>" + "\n" +
"      <INTERESTINCLDAYOFDEDUCTION>No</INTERESTINCLDAYOFDEDUCTION>" + "\n" +
"      <OVERRIDECREDITLIMIT>No</OVERRIDECREDITLIMIT>" + "\n" +
"      <ISAGAINSTFORMC>No</ISAGAINSTFORMC>" + "\n" +
"      <ISCHEQUEPRINTINGENABLED>Yes</ISCHEQUEPRINTINGENABLED>" + "\n" +
"      <ISPAYUPLOAD>No</ISPAYUPLOAD>" + "\n" +
"      <ISPAYBATCHONLYSAL>No</ISPAYBATCHONLYSAL>" + "\n" +
"      <ISBNFCODESUPPORTED>No</ISBNFCODESUPPORTED>" + "\n" +
"      <ALLOWEXPORTWITHERRORS>No</ALLOWEXPORTWITHERRORS>" + "\n" +
"      <USEFORNOTIONALITC>No</USEFORNOTIONALITC>" + "\n" +
"      <ISECOMMOPERATOR>No</ISECOMMOPERATOR>" + "\n" +
"      <SHOWINPAYSLIP>No</SHOWINPAYSLIP>" + "\n" +
"      <USEFORGRATUITY>No</USEFORGRATUITY>" + "\n" +
"      <ISTDSPROJECTED>No</ISTDSPROJECTED>" + "\n" +
"      <FORSERVICETAX>No</FORSERVICETAX>" + "\n" +
"      <ISINPUTCREDIT>No</ISINPUTCREDIT>" + "\n" +
"      <ISEXEMPTED>No</ISEXEMPTED>" + "\n" +
"      <ISABATEMENTAPPLICABLE>No</ISABATEMENTAPPLICABLE>" + "\n" +
"      <ISSTXPARTY>No</ISSTXPARTY>" + "\n" +
"      <ISSTXNONREALIZEDTYPE>No</ISSTXNONREALIZEDTYPE>" + "\n" +
"      <ISUSEDFORCVD>No</ISUSEDFORCVD>" + "\n" +
"      <LEDBELONGSTONONTAXABLE>No</LEDBELONGSTONONTAXABLE>" + "\n" +
"      <ISEXCISEMERCHANTEXPORTER>No</ISEXCISEMERCHANTEXPORTER>" + "\n" +
"      <ISPARTYEXEMPTED>No</ISPARTYEXEMPTED>" + "\n" +
"      <ISSEZPARTY>No</ISSEZPARTY>" + "\n" +
"      <TDSDEDUCTEEISSPECIALRATE>No</TDSDEDUCTEEISSPECIALRATE>" + "\n" +
"      <ISECHEQUESUPPORTED>No</ISECHEQUESUPPORTED>" + "\n" +
"      <ISEDDSUPPORTED>No</ISEDDSUPPORTED>" + "\n" +
"      <HASECHEQUEDELIVERYMODE>No</HASECHEQUEDELIVERYMODE>" + "\n" +
"      <HASECHEQUEDELIVERYTO>No</HASECHEQUEDELIVERYTO>" + "\n" +
"      <HASECHEQUEPRINTLOCATION>No</HASECHEQUEPRINTLOCATION>" + "\n" +
"      <HASECHEQUEPAYABLELOCATION>No</HASECHEQUEPAYABLELOCATION>" + "\n" +
"      <HASECHEQUEBANKLOCATION>No</HASECHEQUEBANKLOCATION>" + "\n" +
"      <HASEDDDELIVERYMODE>No</HASEDDDELIVERYMODE>" + "\n" +
"      <HASEDDDELIVERYTO>No</HASEDDDELIVERYTO>" + "\n" +
"      <HASEDDPRINTLOCATION>No</HASEDDPRINTLOCATION>" + "\n" +
"      <HASEDDPAYABLELOCATION>No</HASEDDPAYABLELOCATION>" + "\n" +
"      <HASEDDBANKLOCATION>No</HASEDDBANKLOCATION>" + "\n" +
"      <ISEBANKINGENABLED>No</ISEBANKINGENABLED>" + "\n" +
"      <ISEXPORTFILEENCRYPTED>No</ISEXPORTFILEENCRYPTED>" + "\n" +
"      <ISBATCHENABLED>No</ISBATCHENABLED>" + "\n" +
"      <ISPRODUCTCODEBASED>No</ISPRODUCTCODEBASED>" + "\n" +
"      <HASEDDCITY>No</HASEDDCITY>" + "\n" +
"      <HASECHEQUECITY>No</HASECHEQUECITY>" + "\n" +
"      <ISFILENAMEFORMATSUPPORTED>No</ISFILENAMEFORMATSUPPORTED>" + "\n" +
"      <HASCLIENTCODE>No</HASCLIENTCODE>" + "\n" +
"      <PAYINSISBATCHAPPLICABLE>No</PAYINSISBATCHAPPLICABLE>" + "\n" +
"      <PAYINSISFILENUMAPP>No</PAYINSISFILENUMAPP>" + "\n" +
"      <ISSALARYTRANSGROUPEDFORBRS>No</ISSALARYTRANSGROUPEDFORBRS>" + "\n" +
"      <ISEBANKINGSUPPORTED>No</ISEBANKINGSUPPORTED>" + "\n" +
"      <ISSCBUAE>No</ISSCBUAE>" + "\n" +
"      <ISBANKSTATUSAPP>No</ISBANKSTATUSAPP>" + "\n" +
"      <ISSALARYGROUPED>No</ISSALARYGROUPED>" + "\n" +
"      <USEFORPURCHASETAX>No</USEFORPURCHASETAX>" + "\n" +
"      <AUDITED>No</AUDITED>" + "\n" +
"      <SORTPOSITION> 1000</SORTPOSITION>" + "\n" +
"      <ALTERID> 241</ALTERID>" + "\n" +
"      <SERVICETAXDETAILS.LIST>      </SERVICETAXDETAILS.LIST>" + "\n" +
"      <LBTREGNDETAILS.LIST>      </LBTREGNDETAILS.LIST>" + "\n" +
"      <VATDETAILS.LIST>      </VATDETAILS.LIST>" + "\n" +
"      <SALESTAXCESSDETAILS.LIST>      </SALESTAXCESSDETAILS.LIST>" + "\n" +
"      <GSTDETAILS.LIST>      </GSTDETAILS.LIST>" + "\n" +
"      <LANGUAGENAME.LIST>" + "\n" +
"       <NAME.LIST TYPE=\"String\">" + "\n" +
"        <NAME>NonSubscriptionFeeAdj</NAME>" + "\n" +
"       </NAME.LIST>" + "\n" +
"       <LANGUAGEID> 1033</LANGUAGEID>" + "\n" +
"      </LANGUAGENAME.LIST>" + "\n" +
"      <XBRLDETAIL.LIST>      </XBRLDETAIL.LIST>" + "\n" +
"      <AUDITDETAILS.LIST>      </AUDITDETAILS.LIST>" + "\n" +
"      <SCHVIDETAILS.LIST>      </SCHVIDETAILS.LIST>" + "\n" +
"      <EXCISETARIFFDETAILS.LIST>      </EXCISETARIFFDETAILS.LIST>" + "\n" +
"      <TCSCATEGORYDETAILS.LIST>      </TCSCATEGORYDETAILS.LIST>" + "\n" +
"      <TDSCATEGORYDETAILS.LIST>      </TDSCATEGORYDETAILS.LIST>" + "\n" +
"      <SLABPERIOD.LIST>      </SLABPERIOD.LIST>" + "\n" +
"      <GRATUITYPERIOD.LIST>      </GRATUITYPERIOD.LIST>" + "\n" +
"      <ADDITIONALCOMPUTATIONS.LIST>      </ADDITIONALCOMPUTATIONS.LIST>" + "\n" +
"      <EXCISEJURISDICTIONDETAILS.LIST>      </EXCISEJURISDICTIONDETAILS.LIST>" + "\n" +
"      <EXCLUDEDTAXATIONS.LIST>      </EXCLUDEDTAXATIONS.LIST>" + "\n" +
"      <BANKALLOCATIONS.LIST>      </BANKALLOCATIONS.LIST>" + "\n" +
"      <PAYMENTDETAILS.LIST>      </PAYMENTDETAILS.LIST>" + "\n" +
"      <BANKEXPORTFORMATS.LIST>      </BANKEXPORTFORMATS.LIST>" + "\n" +
"      <BILLALLOCATIONS.LIST>      </BILLALLOCATIONS.LIST>" + "\n" +
"      <INTERESTCOLLECTION.LIST>      </INTERESTCOLLECTION.LIST>" + "\n" +
"      <LEDGERCLOSINGVALUES.LIST>      </LEDGERCLOSINGVALUES.LIST>" + "\n" +
"      <LEDGERAUDITCLASS.LIST>      </LEDGERAUDITCLASS.LIST>" + "\n" +
"      <OLDAUDITENTRIES.LIST>      </OLDAUDITENTRIES.LIST>" + "\n" +
"      <TDSEXEMPTIONRULES.LIST>      </TDSEXEMPTIONRULES.LIST>" + "\n" +
"      <DEDUCTINSAMEVCHRULES.LIST>      </DEDUCTINSAMEVCHRULES.LIST>" + "\n" +
"      <LOWERDEDUCTION.LIST>      </LOWERDEDUCTION.LIST>" + "\n" +
"      <STXABATEMENTDETAILS.LIST>      </STXABATEMENTDETAILS.LIST>" + "\n" +
"      <LEDMULTIADDRESSLIST.LIST>      </LEDMULTIADDRESSLIST.LIST>" + "\n" +
"      <STXTAXDETAILS.LIST>      </STXTAXDETAILS.LIST>" + "\n" +
"      <CHEQUERANGE.LIST>      </CHEQUERANGE.LIST>" + "\n" +
"      <DEFAULTVCHCHEQUEDETAILS.LIST>      </DEFAULTVCHCHEQUEDETAILS.LIST>" + "\n" +
"      <ACCOUNTAUDITENTRIES.LIST>      </ACCOUNTAUDITENTRIES.LIST>" + "\n" +
"      <AUDITENTRIES.LIST>      </AUDITENTRIES.LIST>" + "\n" +
"      <BRSIMPORTEDINFO.LIST>      </BRSIMPORTEDINFO.LIST>" + "\n" +
"      <AUTOBRSCONFIGS.LIST>      </AUTOBRSCONFIGS.LIST>" + "\n" +
"      <BANKURENTRIES.LIST>      </BANKURENTRIES.LIST>" + "\n" +
"      <DEFAULTCHEQUEDETAILS.LIST>      </DEFAULTCHEQUEDETAILS.LIST>" + "\n" +
"      <DEFAULTOPENINGCHEQUEDETAILS.LIST>      </DEFAULTOPENINGCHEQUEDETAILS.LIST>" + "\n" +
"      <CANCELLEDPAYALLOCATIONS.LIST>      </CANCELLEDPAYALLOCATIONS.LIST>" + "\n" +
"      <ECHEQUEPRINTLOCATION.LIST>      </ECHEQUEPRINTLOCATION.LIST>" + "\n" +
"      <ECHEQUEPAYABLELOCATION.LIST>      </ECHEQUEPAYABLELOCATION.LIST>" + "\n" +
"      <EDDPRINTLOCATION.LIST>      </EDDPRINTLOCATION.LIST>" + "\n" +
"      <EDDPAYABLELOCATION.LIST>      </EDDPAYABLELOCATION.LIST>" + "\n" +
"      <AVAILABLETRANSACTIONTYPES.LIST>      </AVAILABLETRANSACTIONTYPES.LIST>" + "\n" +
"      <LEDPAYINSCONFIGS.LIST>      </LEDPAYINSCONFIGS.LIST>" + "\n" +
"      <TYPECODEDETAILS.LIST>      </TYPECODEDETAILS.LIST>" + "\n" +
"      <FIELDVALIDATIONDETAILS.LIST>      </FIELDVALIDATIONDETAILS.LIST>" + "\n" +
"      <INPUTCRALLOCS.LIST>      </INPUTCRALLOCS.LIST>" + "\n" +
"      <GSTCLASSFNIGSTRATES.LIST>      </GSTCLASSFNIGSTRATES.LIST>" + "\n" +
"      <EXTARIFFDUTYHEADDETAILS.LIST>      </EXTARIFFDUTYHEADDETAILS.LIST>" + "\n" +
"      <VOUCHERTYPEPRODUCTCODES.LIST>      </VOUCHERTYPEPRODUCTCODES.LIST>" + "\n" +
"     </LEDGER>" + "\n" +
"    </TALLYMESSAGE>\n";

        string OrderCancellation =
"    <TALLYMESSAGE xmlns:UDF=\"TallyUDF\">" + "\n" +
"     <LEDGER NAME=\"Order Cancellation Charge\" RESERVEDNAME=\"\">" + "\n" +
"      <OLDAUDITENTRYIDS.LIST TYPE=\"Number\">" + "\n" +
"       <OLDAUDITENTRYIDS>-1</OLDAUDITENTRYIDS>" + "\n" +
"      </OLDAUDITENTRYIDS.LIST>" + "\n" +
"      <GUID>df13ea43-535b-411e-a3ae-09a30f52223d-000000b1</GUID>" + "\n" +
"      <CURRENCYNAME>₹</CURRENCYNAME>" + "\n" +
"      <PARENT>Direct Expenses</PARENT>" + "\n" +
"      <GSTAPPLICABLE>&#4; Applicable</GSTAPPLICABLE>" + "\n" +
"      <TAXCLASSIFICATIONNAME/>" + "\n" +
"      <TAXTYPE>Others</TAXTYPE>" + "\n" +
"      <LEDADDLALLOCTYPE/>" + "\n" +
"      <GSTTYPE/>" + "\n" +
"      <APPROPRIATEFOR/>" + "\n" +
"      <GSTTYPEOFSUPPLY>Services</GSTTYPEOFSUPPLY>" + "\n" +
"      <SERVICECATEGORY>&#4; Not Applicable</SERVICECATEGORY>" + "\n" +
"      <EXCISELEDGERCLASSIFICATION/>" + "\n" +
"      <EXCISEDUTYTYPE/>" + "\n" +
"      <EXCISENATUREOFPURCHASE/>" + "\n" +
"      <LEDGERFBTCATEGORY/>" + "\n" +
"      <VATAPPLICABLE>&#4; Not Applicable</VATAPPLICABLE>" + "\n" +
"      <ISBILLWISEON>Yes</ISBILLWISEON>" + "\n" +
"      <ISCOSTCENTRESON>Yes</ISCOSTCENTRESON>" + "\n" +
"      <ISINTERESTON>No</ISINTERESTON>" + "\n" +
"      <ALLOWINMOBILE>No</ALLOWINMOBILE>" + "\n" +
"      <ISCOSTTRACKINGON>No</ISCOSTTRACKINGON>" + "\n" +
"      <ISBENEFICIARYCODEON>No</ISBENEFICIARYCODEON>" + "\n" +
"      <ISUPDATINGTARGETID>No</ISUPDATINGTARGETID>" + "\n" +
"      <ASORIGINAL>Yes</ASORIGINAL>" + "\n" +
"      <ISCONDENSED>No</ISCONDENSED>" + "\n" +
"      <AFFECTSSTOCK>No</AFFECTSSTOCK>" + "\n" +
"      <ISRATEINCLUSIVEVAT>No</ISRATEINCLUSIVEVAT>" + "\n" +
"      <FORPAYROLL>No</FORPAYROLL>" + "\n" +
"      <ISABCENABLED>No</ISABCENABLED>" + "\n" +
"      <ISCREDITDAYSCHKON>No</ISCREDITDAYSCHKON>" + "\n" +
"      <INTERESTONBILLWISE>No</INTERESTONBILLWISE>" + "\n" +
"      <OVERRIDEINTEREST>No</OVERRIDEINTEREST>" + "\n" +
"      <OVERRIDEADVINTEREST>No</OVERRIDEADVINTEREST>" + "\n" +
"      <USEFORVAT>No</USEFORVAT>" + "\n" +
"      <IGNORETDSEXEMPT>No</IGNORETDSEXEMPT>" + "\n" +
"      <ISTCSAPPLICABLE>No</ISTCSAPPLICABLE>" + "\n" +
"      <ISTDSAPPLICABLE>No</ISTDSAPPLICABLE>" + "\n" +
"      <ISFBTAPPLICABLE>No</ISFBTAPPLICABLE>" + "\n" +
"      <ISGSTAPPLICABLE>No</ISGSTAPPLICABLE>" + "\n" +
"      <ISEXCISEAPPLICABLE>No</ISEXCISEAPPLICABLE>" + "\n" +
"      <ISTDSEXPENSE>No</ISTDSEXPENSE>" + "\n" +
"      <ISEDLIAPPLICABLE>No</ISEDLIAPPLICABLE>" + "\n" +
"      <ISRELATEDPARTY>No</ISRELATEDPARTY>" + "\n" +
"      <USEFORESIELIGIBILITY>No</USEFORESIELIGIBILITY>" + "\n" +
"      <ISINTERESTINCLLASTDAY>No</ISINTERESTINCLLASTDAY>" + "\n" +
"      <APPROPRIATETAXVALUE>No</APPROPRIATETAXVALUE>" + "\n" +
"      <ISBEHAVEASDUTY>No</ISBEHAVEASDUTY>" + "\n" +
"      <INTERESTINCLDAYOFADDITION>No</INTERESTINCLDAYOFADDITION>" + "\n" +
"      <INTERESTINCLDAYOFDEDUCTION>No</INTERESTINCLDAYOFDEDUCTION>" + "\n" +
"      <OVERRIDECREDITLIMIT>No</OVERRIDECREDITLIMIT>" + "\n" +
"      <ISAGAINSTFORMC>No</ISAGAINSTFORMC>" + "\n" +
"      <ISCHEQUEPRINTINGENABLED>Yes</ISCHEQUEPRINTINGENABLED>" + "\n" +
"      <ISPAYUPLOAD>No</ISPAYUPLOAD>" + "\n" +
"      <ISPAYBATCHONLYSAL>No</ISPAYBATCHONLYSAL>" + "\n" +
"      <ISBNFCODESUPPORTED>No</ISBNFCODESUPPORTED>" + "\n" +
"      <ALLOWEXPORTWITHERRORS>No</ALLOWEXPORTWITHERRORS>" + "\n" +
"      <USEFORNOTIONALITC>No</USEFORNOTIONALITC>" + "\n" +
"      <ISECOMMOPERATOR>No</ISECOMMOPERATOR>" + "\n" +
"      <SHOWINPAYSLIP>No</SHOWINPAYSLIP>" + "\n" +
"      <USEFORGRATUITY>No</USEFORGRATUITY>" + "\n" +
"      <ISTDSPROJECTED>No</ISTDSPROJECTED>" + "\n" +
"      <FORSERVICETAX>No</FORSERVICETAX>" + "\n" +
"      <ISINPUTCREDIT>No</ISINPUTCREDIT>" + "\n" +
"      <ISEXEMPTED>No</ISEXEMPTED>" + "\n" +
"      <ISABATEMENTAPPLICABLE>No</ISABATEMENTAPPLICABLE>" + "\n" +
"      <ISSTXPARTY>No</ISSTXPARTY>" + "\n" +
"      <ISSTXNONREALIZEDTYPE>No</ISSTXNONREALIZEDTYPE>" + "\n" +
"      <ISUSEDFORCVD>No</ISUSEDFORCVD>" + "\n" +
"      <LEDBELONGSTONONTAXABLE>No</LEDBELONGSTONONTAXABLE>" + "\n" +
"      <ISEXCISEMERCHANTEXPORTER>No</ISEXCISEMERCHANTEXPORTER>" + "\n" +
"      <ISPARTYEXEMPTED>No</ISPARTYEXEMPTED>" + "\n" +
"      <ISSEZPARTY>No</ISSEZPARTY>" + "\n" +
"      <TDSDEDUCTEEISSPECIALRATE>No</TDSDEDUCTEEISSPECIALRATE>" + "\n" +
"      <ISECHEQUESUPPORTED>No</ISECHEQUESUPPORTED>" + "\n" +
"      <ISEDDSUPPORTED>No</ISEDDSUPPORTED>" + "\n" +
"      <HASECHEQUEDELIVERYMODE>No</HASECHEQUEDELIVERYMODE>" + "\n" +
"      <HASECHEQUEDELIVERYTO>No</HASECHEQUEDELIVERYTO>" + "\n" +
"      <HASECHEQUEPRINTLOCATION>No</HASECHEQUEPRINTLOCATION>" + "\n" +
"      <HASECHEQUEPAYABLELOCATION>No</HASECHEQUEPAYABLELOCATION>" + "\n" +
"      <HASECHEQUEBANKLOCATION>No</HASECHEQUEBANKLOCATION>" + "\n" +
"      <HASEDDDELIVERYMODE>No</HASEDDDELIVERYMODE>" + "\n" +
"      <HASEDDDELIVERYTO>No</HASEDDDELIVERYTO>" + "\n" +
"      <HASEDDPRINTLOCATION>No</HASEDDPRINTLOCATION>" + "\n" +
"      <HASEDDPAYABLELOCATION>No</HASEDDPAYABLELOCATION>" + "\n" +
"      <HASEDDBANKLOCATION>No</HASEDDBANKLOCATION>" + "\n" +
"      <ISEBANKINGENABLED>No</ISEBANKINGENABLED>" + "\n" +
"      <ISEXPORTFILEENCRYPTED>No</ISEXPORTFILEENCRYPTED>" + "\n" +
"      <ISBATCHENABLED>No</ISBATCHENABLED>" + "\n" +
"      <ISPRODUCTCODEBASED>No</ISPRODUCTCODEBASED>" + "\n" +
"      <HASEDDCITY>No</HASEDDCITY>" + "\n" +
"      <HASECHEQUECITY>No</HASECHEQUECITY>" + "\n" +
"      <ISFILENAMEFORMATSUPPORTED>No</ISFILENAMEFORMATSUPPORTED>" + "\n" +
"      <HASCLIENTCODE>No</HASCLIENTCODE>" + "\n" +
"      <PAYINSISBATCHAPPLICABLE>No</PAYINSISBATCHAPPLICABLE>" + "\n" +
"      <PAYINSISFILENUMAPP>No</PAYINSISFILENUMAPP>" + "\n" +
"      <ISSALARYTRANSGROUPEDFORBRS>No</ISSALARYTRANSGROUPEDFORBRS>" + "\n" +
"      <ISEBANKINGSUPPORTED>No</ISEBANKINGSUPPORTED>" + "\n" +
"      <ISSCBUAE>No</ISSCBUAE>" + "\n" +
"      <ISBANKSTATUSAPP>No</ISBANKSTATUSAPP>" + "\n" +
"      <ISSALARYGROUPED>No</ISSALARYGROUPED>" + "\n" +
"      <USEFORPURCHASETAX>No</USEFORPURCHASETAX>" + "\n" +
"      <AUDITED>No</AUDITED>" + "\n" +
"      <SORTPOSITION> 1000</SORTPOSITION>" + "\n" +
"      <ALTERID> 309</ALTERID>" + "\n" +
"      <SERVICETAXDETAILS.LIST>      </SERVICETAXDETAILS.LIST>" + "\n" +
"      <LBTREGNDETAILS.LIST>      </LBTREGNDETAILS.LIST>" + "\n" +
"      <VATDETAILS.LIST>      </VATDETAILS.LIST>" + "\n" +
"      <SALESTAXCESSDETAILS.LIST>      </SALESTAXCESSDETAILS.LIST>" + "\n" +
"      <GSTDETAILS.LIST>      </GSTDETAILS.LIST>" + "\n" +
"      <LANGUAGENAME.LIST>" + "\n" +
"       <NAME.LIST TYPE=\"String\">" + "\n" +
"        <NAME>Order Cancellation Charge</NAME>" + "\n" +
"       </NAME.LIST>" + "\n" +
"       <LANGUAGEID> 1033</LANGUAGEID>" + "\n" +
"      </LANGUAGENAME.LIST>" + "\n" +
"      <XBRLDETAIL.LIST>      </XBRLDETAIL.LIST>" + "\n" +
"      <AUDITDETAILS.LIST>      </AUDITDETAILS.LIST>" + "\n" +
"      <SCHVIDETAILS.LIST>      </SCHVIDETAILS.LIST>" + "\n" +
"      <EXCISETARIFFDETAILS.LIST>      </EXCISETARIFFDETAILS.LIST>" + "\n" +
"      <TCSCATEGORYDETAILS.LIST>      </TCSCATEGORYDETAILS.LIST>" + "\n" +
"      <TDSCATEGORYDETAILS.LIST>      </TDSCATEGORYDETAILS.LIST>" + "\n" +
"      <SLABPERIOD.LIST>      </SLABPERIOD.LIST>" + "\n" +
"      <GRATUITYPERIOD.LIST>      </GRATUITYPERIOD.LIST>" + "\n" +
"      <ADDITIONALCOMPUTATIONS.LIST>      </ADDITIONALCOMPUTATIONS.LIST>" + "\n" +
"      <EXCISEJURISDICTIONDETAILS.LIST>      </EXCISEJURISDICTIONDETAILS.LIST>" + "\n" +
"      <EXCLUDEDTAXATIONS.LIST>      </EXCLUDEDTAXATIONS.LIST>" + "\n" +
"      <BANKALLOCATIONS.LIST>      </BANKALLOCATIONS.LIST>" + "\n" +
"      <PAYMENTDETAILS.LIST>      </PAYMENTDETAILS.LIST>" + "\n" +
"      <BANKEXPORTFORMATS.LIST>      </BANKEXPORTFORMATS.LIST>" + "\n" +
"      <BILLALLOCATIONS.LIST>      </BILLALLOCATIONS.LIST>" + "\n" +
"      <INTERESTCOLLECTION.LIST>      </INTERESTCOLLECTION.LIST>" + "\n" +
"      <LEDGERCLOSINGVALUES.LIST>      </LEDGERCLOSINGVALUES.LIST>" + "\n" +
"      <LEDGERAUDITCLASS.LIST>      </LEDGERAUDITCLASS.LIST>" + "\n" +
"      <OLDAUDITENTRIES.LIST>      </OLDAUDITENTRIES.LIST>" + "\n" +
"      <TDSEXEMPTIONRULES.LIST>      </TDSEXEMPTIONRULES.LIST>" + "\n" +
"      <DEDUCTINSAMEVCHRULES.LIST>      </DEDUCTINSAMEVCHRULES.LIST>" + "\n" +
"      <LOWERDEDUCTION.LIST>      </LOWERDEDUCTION.LIST>" + "\n" +
"      <STXABATEMENTDETAILS.LIST>      </STXABATEMENTDETAILS.LIST>" + "\n" +
"      <LEDMULTIADDRESSLIST.LIST>      </LEDMULTIADDRESSLIST.LIST>" + "\n" +
"      <STXTAXDETAILS.LIST>      </STXTAXDETAILS.LIST>" + "\n" +
"      <CHEQUERANGE.LIST>      </CHEQUERANGE.LIST>" + "\n" +
"      <DEFAULTVCHCHEQUEDETAILS.LIST>      </DEFAULTVCHCHEQUEDETAILS.LIST>" + "\n" +
"      <ACCOUNTAUDITENTRIES.LIST>      </ACCOUNTAUDITENTRIES.LIST>" + "\n" +
"      <AUDITENTRIES.LIST>      </AUDITENTRIES.LIST>" + "\n" +
"      <BRSIMPORTEDINFO.LIST>      </BRSIMPORTEDINFO.LIST>" + "\n" +
"      <AUTOBRSCONFIGS.LIST>      </AUTOBRSCONFIGS.LIST>" + "\n" +
"      <BANKURENTRIES.LIST>      </BANKURENTRIES.LIST>" + "\n" +
"      <DEFAULTCHEQUEDETAILS.LIST>      </DEFAULTCHEQUEDETAILS.LIST>" + "\n" +
"      <DEFAULTOPENINGCHEQUEDETAILS.LIST>      </DEFAULTOPENINGCHEQUEDETAILS.LIST>" + "\n" +
"      <CANCELLEDPAYALLOCATIONS.LIST>      </CANCELLEDPAYALLOCATIONS.LIST>" + "\n" +
"      <ECHEQUEPRINTLOCATION.LIST>      </ECHEQUEPRINTLOCATION.LIST>" + "\n" +
"      <ECHEQUEPAYABLELOCATION.LIST>      </ECHEQUEPAYABLELOCATION.LIST>" + "\n" +
"      <EDDPRINTLOCATION.LIST>      </EDDPRINTLOCATION.LIST>" + "\n" +
"      <EDDPAYABLELOCATION.LIST>      </EDDPAYABLELOCATION.LIST>" + "\n" +
"      <AVAILABLETRANSACTIONTYPES.LIST>      </AVAILABLETRANSACTIONTYPES.LIST>" + "\n" +
"      <LEDPAYINSCONFIGS.LIST>      </LEDPAYINSCONFIGS.LIST>" + "\n" +
"      <TYPECODEDETAILS.LIST>      </TYPECODEDETAILS.LIST>" + "\n" +
"      <FIELDVALIDATIONDETAILS.LIST>      </FIELDVALIDATIONDETAILS.LIST>" + "\n" +
"      <INPUTCRALLOCS.LIST>      </INPUTCRALLOCS.LIST>" + "\n" +
"      <GSTCLASSFNIGSTRATES.LIST>      </GSTCLASSFNIGSTRATES.LIST>" + "\n" +
"      <EXTARIFFDUTYHEADDETAILS.LIST>      </EXTARIFFDUTYHEADDETAILS.LIST>" + "\n" +
"      <VOUCHERTYPEPRODUCTCODES.LIST>      </VOUCHERTYPEPRODUCTCODES.LIST>" + "\n" +
"     </LEDGER>" + "\n" +
"    </TALLYMESSAGE>\n";

        string VoucherPreviousAmount=
"    <TALLYMESSAGE xmlns:UDF=\"TallyUDF\">" + "\n" +
"     <LEDGER NAME=\"Previous Reserve Amount Balance\" RESERVEDNAME=\"\">" + "\n" +
"      <MAILINGNAME.LIST TYPE=\"String\">" + "\n" +
"       <MAILINGNAME>Previous Reserve Amount Balance</MAILINGNAME>" + "\n" +
"      </MAILINGNAME.LIST>" + "\n" +
"      <OLDAUDITENTRYIDS.LIST TYPE=\"Number\">" + "\n" +
"       <OLDAUDITENTRYIDS>-1</OLDAUDITENTRYIDS>" + "\n" +
"      </OLDAUDITENTRYIDS.LIST>" + "\n" +
"      <GUID>df13ea43-535b-411e-a3ae-09a30f52223d-000000ac</GUID>" + "\n" +
"      <CURRENCYNAME>₹</CURRENCYNAME>" + "\n" +
"      <COUNTRYNAME>India</COUNTRYNAME>" + "\n" +
"      <PARENT>Sundry Debtors</PARENT>" + "\n" +
"      <TAXCLASSIFICATIONNAME/>" + "\n" +
"      <TAXTYPE>Others</TAXTYPE>" + "\n" +
"      <COUNTRYOFRESIDENCE>India</COUNTRYOFRESIDENCE>" + "\n" +
"      <LEDADDLALLOCTYPE/>" + "\n" +
"      <GSTTYPE/>" + "\n" +
"      <APPROPRIATEFOR/>" + "\n" +
"      <SERVICECATEGORY>&#4; Not Applicable</SERVICECATEGORY>" + "\n" +
"      <EXCISELEDGERCLASSIFICATION/>" + "\n" +
"      <EXCISEDUTYTYPE/>" + "\n" +
"      <EXCISENATUREOFPURCHASE/>" + "\n" +
"      <LEDGERFBTCATEGORY/>" + "\n" +
"      <LEDSTATENAME/>" + "\n" +
"      <ISBILLWISEON>Yes</ISBILLWISEON>" + "\n" +
"      <ISCOSTCENTRESON>No</ISCOSTCENTRESON>" + "\n" +
"      <ISINTERESTON>No</ISINTERESTON>" + "\n" +
"      <ALLOWINMOBILE>No</ALLOWINMOBILE>" + "\n" +
"      <ISCOSTTRACKINGON>No</ISCOSTTRACKINGON>" + "\n" +
"      <ISBENEFICIARYCODEON>No</ISBENEFICIARYCODEON>" + "\n" +
"      <ISUPDATINGTARGETID>No</ISUPDATINGTARGETID>" + "\n" +
"      <ASORIGINAL>Yes</ASORIGINAL>" + "\n" +
"      <ISCONDENSED>No</ISCONDENSED>" + "\n" +
"      <AFFECTSSTOCK>No</AFFECTSSTOCK>" + "\n" +
"      <ISRATEINCLUSIVEVAT>No</ISRATEINCLUSIVEVAT>" + "\n" +
"      <FORPAYROLL>No</FORPAYROLL>" + "\n" +
"      <ISABCENABLED>No</ISABCENABLED>" + "\n" +
"      <ISCREDITDAYSCHKON>No</ISCREDITDAYSCHKON>" + "\n" +
"      <INTERESTONBILLWISE>No</INTERESTONBILLWISE>" + "\n" +
"      <OVERRIDEINTEREST>No</OVERRIDEINTEREST>" + "\n" +
"      <OVERRIDEADVINTEREST>No</OVERRIDEADVINTEREST>" + "\n" +
"      <USEFORVAT>No</USEFORVAT>" + "\n" +
"      <IGNORETDSEXEMPT>No</IGNORETDSEXEMPT>" + "\n" +
"      <ISTCSAPPLICABLE>No</ISTCSAPPLICABLE>" + "\n" +
"      <ISTDSAPPLICABLE>No</ISTDSAPPLICABLE>" + "\n" +
"      <ISFBTAPPLICABLE>No</ISFBTAPPLICABLE>" + "\n" +
"      <ISGSTAPPLICABLE>No</ISGSTAPPLICABLE>" + "\n" +
"      <ISEXCISEAPPLICABLE>No</ISEXCISEAPPLICABLE>" + "\n" +
"      <ISTDSEXPENSE>No</ISTDSEXPENSE>" + "\n" +
"      <ISEDLIAPPLICABLE>No</ISEDLIAPPLICABLE>" + "\n" +
"      <ISRELATEDPARTY>No</ISRELATEDPARTY>" + "\n" +
"      <USEFORESIELIGIBILITY>No</USEFORESIELIGIBILITY>" + "\n" +
"      <ISINTERESTINCLLASTDAY>No</ISINTERESTINCLLASTDAY>" + "\n" +
"      <APPROPRIATETAXVALUE>No</APPROPRIATETAXVALUE>" + "\n" +
"      <ISBEHAVEASDUTY>No</ISBEHAVEASDUTY>" + "\n" +
"      <INTERESTINCLDAYOFADDITION>No</INTERESTINCLDAYOFADDITION>" + "\n" +
"      <INTERESTINCLDAYOFDEDUCTION>No</INTERESTINCLDAYOFDEDUCTION>" + "\n" +
"      <OVERRIDECREDITLIMIT>No</OVERRIDECREDITLIMIT>" + "\n" +
"      <ISAGAINSTFORMC>No</ISAGAINSTFORMC>" + "\n" +
"      <ISCHEQUEPRINTINGENABLED>Yes</ISCHEQUEPRINTINGENABLED>" + "\n" +
"      <ISPAYUPLOAD>No</ISPAYUPLOAD>" + "\n" +
"      <ISPAYBATCHONLYSAL>No</ISPAYBATCHONLYSAL>" + "\n" +
"      <ISBNFCODESUPPORTED>No</ISBNFCODESUPPORTED>" + "\n" +
"      <ALLOWEXPORTWITHERRORS>No</ALLOWEXPORTWITHERRORS>" + "\n" +
"      <USEFORNOTIONALITC>No</USEFORNOTIONALITC>" + "\n" +
"      <ISECOMMOPERATOR>No</ISECOMMOPERATOR>" + "\n" +
"      <SHOWINPAYSLIP>No</SHOWINPAYSLIP>" + "\n" +
"      <USEFORGRATUITY>No</USEFORGRATUITY>" + "\n" +
"      <ISTDSPROJECTED>No</ISTDSPROJECTED>" + "\n" +
"      <FORSERVICETAX>No</FORSERVICETAX>" + "\n" +
"      <ISINPUTCREDIT>No</ISINPUTCREDIT>" + "\n" +
"      <ISEXEMPTED>No</ISEXEMPTED>" + "\n" +
"      <ISABATEMENTAPPLICABLE>No</ISABATEMENTAPPLICABLE>" + "\n" +
"      <ISSTXPARTY>No</ISSTXPARTY>" + "\n" +
"      <ISSTXNONREALIZEDTYPE>No</ISSTXNONREALIZEDTYPE>" + "\n" +
"      <ISUSEDFORCVD>No</ISUSEDFORCVD>" + "\n" +
"      <LEDBELONGSTONONTAXABLE>No</LEDBELONGSTONONTAXABLE>" + "\n" +
"      <ISEXCISEMERCHANTEXPORTER>No</ISEXCISEMERCHANTEXPORTER>" + "\n" +
"      <ISPARTYEXEMPTED>No</ISPARTYEXEMPTED>" + "\n" +
"      <ISSEZPARTY>No</ISSEZPARTY>" + "\n" +
"      <TDSDEDUCTEEISSPECIALRATE>No</TDSDEDUCTEEISSPECIALRATE>" + "\n" +
"      <ISECHEQUESUPPORTED>No</ISECHEQUESUPPORTED>" + "\n" +
"      <ISEDDSUPPORTED>No</ISEDDSUPPORTED>" + "\n" +
"      <HASECHEQUEDELIVERYMODE>No</HASECHEQUEDELIVERYMODE>" + "\n" +
"      <HASECHEQUEDELIVERYTO>No</HASECHEQUEDELIVERYTO>" + "\n" +
"      <HASECHEQUEPRINTLOCATION>No</HASECHEQUEPRINTLOCATION>" + "\n" +
"      <HASECHEQUEPAYABLELOCATION>No</HASECHEQUEPAYABLELOCATION>" + "\n" +
"      <HASECHEQUEBANKLOCATION>No</HASECHEQUEBANKLOCATION>" + "\n" +
"      <HASEDDDELIVERYMODE>No</HASEDDDELIVERYMODE>" + "\n" +
"      <HASEDDDELIVERYTO>No</HASEDDDELIVERYTO>" + "\n" +
"      <HASEDDPRINTLOCATION>No</HASEDDPRINTLOCATION>" + "\n" +
"      <HASEDDPAYABLELOCATION>No</HASEDDPAYABLELOCATION>" + "\n" +
"      <HASEDDBANKLOCATION>No</HASEDDBANKLOCATION>" + "\n" +
"      <ISEBANKINGENABLED>No</ISEBANKINGENABLED>" + "\n" +
"      <ISEXPORTFILEENCRYPTED>No</ISEXPORTFILEENCRYPTED>" + "\n" +
"      <ISBATCHENABLED>No</ISBATCHENABLED>" + "\n" +
"      <ISPRODUCTCODEBASED>No</ISPRODUCTCODEBASED>" + "\n" +
"      <HASEDDCITY>No</HASEDDCITY>" + "\n" +
"      <HASECHEQUECITY>No</HASECHEQUECITY>" + "\n" +
"      <ISFILENAMEFORMATSUPPORTED>No</ISFILENAMEFORMATSUPPORTED>" + "\n" +
"      <HASCLIENTCODE>No</HASCLIENTCODE>" + "\n" +
"      <PAYINSISBATCHAPPLICABLE>No</PAYINSISBATCHAPPLICABLE>" + "\n" +
"      <PAYINSISFILENUMAPP>No</PAYINSISFILENUMAPP>" + "\n" +
"      <ISSALARYTRANSGROUPEDFORBRS>No</ISSALARYTRANSGROUPEDFORBRS>" + "\n" +
"      <ISEBANKINGSUPPORTED>No</ISEBANKINGSUPPORTED>" + "\n" +
"      <ISSCBUAE>No</ISSCBUAE>" + "\n" +
"      <ISBANKSTATUSAPP>No</ISBANKSTATUSAPP>" + "\n" +
"      <ISSALARYGROUPED>No</ISSALARYGROUPED>" + "\n" +
"      <USEFORPURCHASETAX>No</USEFORPURCHASETAX>" + "\n" +
"      <AUDITED>No</AUDITED>" + "\n" +
"      <SORTPOSITION> 1000</SORTPOSITION>" + "\n" +
"      <ALTERID> 272</ALTERID>" + "\n" +
"      <SERVICETAXDETAILS.LIST>      </SERVICETAXDETAILS.LIST>" + "\n" +
"      <LBTREGNDETAILS.LIST>      </LBTREGNDETAILS.LIST>" + "\n" +
"      <VATDETAILS.LIST>      </VATDETAILS.LIST>" + "\n" +
"      <SALESTAXCESSDETAILS.LIST>      </SALESTAXCESSDETAILS.LIST>" + "\n" +
"      <GSTDETAILS.LIST>      </GSTDETAILS.LIST>" + "\n" +
"      <LANGUAGENAME.LIST>" + "\n" +
"       <NAME.LIST TYPE=\"String\">" + "\n" +
"        <NAME>Previous Reserve Amount Balance</NAME>" + "\n" +
"       </NAME.LIST>" + "\n" +
"       <LANGUAGEID> 1033</LANGUAGEID>" + "\n" +
"      </LANGUAGENAME.LIST>" + "\n" +
"      <XBRLDETAIL.LIST>      </XBRLDETAIL.LIST>" + "\n" +
"      <AUDITDETAILS.LIST>      </AUDITDETAILS.LIST>" + "\n" +
"      <SCHVIDETAILS.LIST>      </SCHVIDETAILS.LIST>" + "\n" +
"      <EXCISETARIFFDETAILS.LIST>      </EXCISETARIFFDETAILS.LIST>" + "\n" +
"      <TCSCATEGORYDETAILS.LIST>      </TCSCATEGORYDETAILS.LIST>" + "\n" +
"      <TDSCATEGORYDETAILS.LIST>      </TDSCATEGORYDETAILS.LIST>" + "\n" +
"      <SLABPERIOD.LIST>      </SLABPERIOD.LIST>" + "\n" +
"      <GRATUITYPERIOD.LIST>      </GRATUITYPERIOD.LIST>" + "\n" +
"      <ADDITIONALCOMPUTATIONS.LIST>      </ADDITIONALCOMPUTATIONS.LIST>" + "\n" +
"      <EXCISEJURISDICTIONDETAILS.LIST>      </EXCISEJURISDICTIONDETAILS.LIST>" + "\n" +
"      <EXCLUDEDTAXATIONS.LIST>      </EXCLUDEDTAXATIONS.LIST>" + "\n" +
"      <BANKALLOCATIONS.LIST>      </BANKALLOCATIONS.LIST>" + "\n" +
"      <PAYMENTDETAILS.LIST>      </PAYMENTDETAILS.LIST>" + "\n" +
"      <BANKEXPORTFORMATS.LIST>      </BANKEXPORTFORMATS.LIST>" + "\n" +
"      <BILLALLOCATIONS.LIST>      </BILLALLOCATIONS.LIST>" + "\n" +
"      <INTERESTCOLLECTION.LIST>      </INTERESTCOLLECTION.LIST>" + "\n" +
"      <LEDGERCLOSINGVALUES.LIST>      </LEDGERCLOSINGVALUES.LIST>" + "\n" +
"      <LEDGERAUDITCLASS.LIST>      </LEDGERAUDITCLASS.LIST>" + "\n" +
"      <OLDAUDITENTRIES.LIST>      </OLDAUDITENTRIES.LIST>" + "\n" +
"      <TDSEXEMPTIONRULES.LIST>      </TDSEXEMPTIONRULES.LIST>" + "\n" +
"      <DEDUCTINSAMEVCHRULES.LIST>      </DEDUCTINSAMEVCHRULES.LIST>" + "\n" +
"      <LOWERDEDUCTION.LIST>      </LOWERDEDUCTION.LIST>" + "\n" +
"      <STXABATEMENTDETAILS.LIST>      </STXABATEMENTDETAILS.LIST>" + "\n" +
"      <LEDMULTIADDRESSLIST.LIST>      </LEDMULTIADDRESSLIST.LIST>" + "\n" +
"      <STXTAXDETAILS.LIST>      </STXTAXDETAILS.LIST>" + "\n" +
"      <CHEQUERANGE.LIST>      </CHEQUERANGE.LIST>" + "\n" +
"      <DEFAULTVCHCHEQUEDETAILS.LIST>      </DEFAULTVCHCHEQUEDETAILS.LIST>" + "\n" +
"      <ACCOUNTAUDITENTRIES.LIST>      </ACCOUNTAUDITENTRIES.LIST>" + "\n" +
"      <AUDITENTRIES.LIST>      </AUDITENTRIES.LIST>" + "\n" +
"      <BRSIMPORTEDINFO.LIST>      </BRSIMPORTEDINFO.LIST>" + "\n" +
"      <AUTOBRSCONFIGS.LIST>      </AUTOBRSCONFIGS.LIST>" + "\n" +
"      <BANKURENTRIES.LIST>      </BANKURENTRIES.LIST>" + "\n" +
"      <DEFAULTCHEQUEDETAILS.LIST>      </DEFAULTCHEQUEDETAILS.LIST>" + "\n" +
"      <DEFAULTOPENINGCHEQUEDETAILS.LIST>      </DEFAULTOPENINGCHEQUEDETAILS.LIST>" + "\n" +
"      <CANCELLEDPAYALLOCATIONS.LIST>      </CANCELLEDPAYALLOCATIONS.LIST>" + "\n" +
"      <ECHEQUEPRINTLOCATION.LIST>      </ECHEQUEPRINTLOCATION.LIST>" + "\n" +
"      <ECHEQUEPAYABLELOCATION.LIST>      </ECHEQUEPAYABLELOCATION.LIST>" + "\n" +
"      <EDDPRINTLOCATION.LIST>      </EDDPRINTLOCATION.LIST>" + "\n" +
"      <EDDPAYABLELOCATION.LIST>      </EDDPAYABLELOCATION.LIST>" + "\n" +
"      <AVAILABLETRANSACTIONTYPES.LIST>      </AVAILABLETRANSACTIONTYPES.LIST>" + "\n" +
"      <LEDPAYINSCONFIGS.LIST>      </LEDPAYINSCONFIGS.LIST>" + "\n" +
"      <TYPECODEDETAILS.LIST>      </TYPECODEDETAILS.LIST>" + "\n" +
"      <FIELDVALIDATIONDETAILS.LIST>      </FIELDVALIDATIONDETAILS.LIST>" + "\n" +
"      <INPUTCRALLOCS.LIST>      </INPUTCRALLOCS.LIST>" + "\n" +
"      <GSTCLASSFNIGSTRATES.LIST>      </GSTCLASSFNIGSTRATES.LIST>" + "\n" +
"      <EXTARIFFDUTYHEADDETAILS.LIST>      </EXTARIFFDUTYHEADDETAILS.LIST>" + "\n" +
"      <VOUCHERTYPEPRODUCTCODES.LIST>      </VOUCHERTYPEPRODUCTCODES.LIST>" + "\n" +
"     </LEDGER>" + "\n" +
"    </TALLYMESSAGE>\n";

        string VoucherSalesAccountsub =
"    <TALLYMESSAGE xmlns:UDF=\"TallyUDF\">" + "\n" +
"     <LEDGER NAME=\"Sales A/c\" RESERVEDNAME=\"\">" + "\n" +
"      <OLDAUDITENTRYIDS.LIST TYPE=\"Number\">" + "\n" +
"       <OLDAUDITENTRYIDS>-1</OLDAUDITENTRYIDS>" + "\n" +
"      </OLDAUDITENTRYIDS.LIST>" + "\n" +
"      <GUID>df13ea43-535b-411e-a3ae-09a30f52223d-000000b4</GUID>" + "\n" +
"      <CURRENCYNAME>₹</CURRENCYNAME>" + "\n" +
"      <PARENT>Sales Accounts</PARENT>" + "\n" +
"      <GSTAPPLICABLE>&#4; Applicable</GSTAPPLICABLE>" + "\n" +
"      <TAXCLASSIFICATIONNAME/>" + "\n" +
"      <TAXTYPE>Others</TAXTYPE>" + "\n" +
"      <GSTTYPE/>" + "\n" +
"      <APPROPRIATEFOR/>" + "\n" +
"      <GSTTYPEOFSUPPLY>Goods</GSTTYPEOFSUPPLY>" + "\n" +
"      <SERVICECATEGORY>&#4; Not Applicable</SERVICECATEGORY>" + "\n" +
"      <EXCISELEDGERCLASSIFICATION/>" + "\n" +
"      <EXCISEDUTYTYPE/>" + "\n" +
"      <EXCISENATUREOFPURCHASE/>" + "\n" +
"      <LEDGERFBTCATEGORY/>" + "\n" +
"      <VATAPPLICABLE>&#4; Applicable</VATAPPLICABLE>" + "\n" +
"      <ISBILLWISEON>No</ISBILLWISEON>" + "\n" +
"      <ISCOSTCENTRESON>Yes</ISCOSTCENTRESON>" + "\n" +
"      <ISINTERESTON>No</ISINTERESTON>" + "\n" +
"      <ALLOWINMOBILE>No</ALLOWINMOBILE>" + "\n" +
"      <ISCOSTTRACKINGON>No</ISCOSTTRACKINGON>" + "\n" +
"      <ISBENEFICIARYCODEON>No</ISBENEFICIARYCODEON>" + "\n" +
"      <ISUPDATINGTARGETID>No</ISUPDATINGTARGETID>" + "\n" +
"      <ASORIGINAL>Yes</ASORIGINAL>" + "\n" +
"      <ISCONDENSED>No</ISCONDENSED>" + "\n" +
"      <AFFECTSSTOCK>Yes</AFFECTSSTOCK>" + "\n" +
"      <ISRATEINCLUSIVEVAT>No</ISRATEINCLUSIVEVAT>" + "\n" +
"      <FORPAYROLL>No</FORPAYROLL>" + "\n" +
"      <ISABCENABLED>No</ISABCENABLED>" + "\n" +
"      <ISCREDITDAYSCHKON>No</ISCREDITDAYSCHKON>" + "\n" +
"      <INTERESTONBILLWISE>No</INTERESTONBILLWISE>" + "\n" +
"      <OVERRIDEINTEREST>No</OVERRIDEINTEREST>" + "\n" +
"      <OVERRIDEADVINTEREST>No</OVERRIDEADVINTEREST>" + "\n" +
"      <USEFORVAT>No</USEFORVAT>" + "\n" +
"      <IGNORETDSEXEMPT>No</IGNORETDSEXEMPT>" + "\n" +
"      <ISTCSAPPLICABLE>No</ISTCSAPPLICABLE>" + "\n" +
"      <ISTDSAPPLICABLE>No</ISTDSAPPLICABLE>" + "\n" +
"      <ISFBTAPPLICABLE>No</ISFBTAPPLICABLE>" + "\n" +
"      <ISGSTAPPLICABLE>No</ISGSTAPPLICABLE>" + "\n" +
"      <ISEXCISEAPPLICABLE>No</ISEXCISEAPPLICABLE>" + "\n" +
"      <ISTDSEXPENSE>No</ISTDSEXPENSE>" + "\n" +
"      <ISEDLIAPPLICABLE>No</ISEDLIAPPLICABLE>" + "\n" +
"      <ISRELATEDPARTY>No</ISRELATEDPARTY>" + "\n" +
"      <USEFORESIELIGIBILITY>No</USEFORESIELIGIBILITY>" + "\n" +
"      <ISINTERESTINCLLASTDAY>No</ISINTERESTINCLLASTDAY>" + "\n" +
"      <APPROPRIATETAXVALUE>No</APPROPRIATETAXVALUE>" + "\n" +
"      <ISBEHAVEASDUTY>No</ISBEHAVEASDUTY>" + "\n" +
"      <INTERESTINCLDAYOFADDITION>No</INTERESTINCLDAYOFADDITION>" + "\n" +
"      <INTERESTINCLDAYOFDEDUCTION>No</INTERESTINCLDAYOFDEDUCTION>" + "\n" +
"      <OVERRIDECREDITLIMIT>No</OVERRIDECREDITLIMIT>" + "\n" +
"      <ISAGAINSTFORMC>No</ISAGAINSTFORMC>" + "\n" +
"      <ISCHEQUEPRINTINGENABLED>Yes</ISCHEQUEPRINTINGENABLED>" + "\n" +
"      <ISPAYUPLOAD>No</ISPAYUPLOAD>" + "\n" +
"      <ISPAYBATCHONLYSAL>No</ISPAYBATCHONLYSAL>" + "\n" +
"      <ISBNFCODESUPPORTED>No</ISBNFCODESUPPORTED>" + "\n" +
"      <ALLOWEXPORTWITHERRORS>No</ALLOWEXPORTWITHERRORS>" + "\n" +
"      <USEFORNOTIONALITC>No</USEFORNOTIONALITC>" + "\n" +
"      <ISECOMMOPERATOR>No</ISECOMMOPERATOR>" + "\n" +
"      <SHOWINPAYSLIP>No</SHOWINPAYSLIP>" + "\n" +
"      <USEFORGRATUITY>No</USEFORGRATUITY>" + "\n" +
"      <ISTDSPROJECTED>No</ISTDSPROJECTED>" + "\n" +
"      <FORSERVICETAX>No</FORSERVICETAX>" + "\n" +
"      <ISINPUTCREDIT>No</ISINPUTCREDIT>" + "\n" +
"      <ISEXEMPTED>No</ISEXEMPTED>" + "\n" +
"      <ISABATEMENTAPPLICABLE>No</ISABATEMENTAPPLICABLE>" + "\n" +
"      <ISSTXPARTY>No</ISSTXPARTY>" + "\n" +
"      <ISSTXNONREALIZEDTYPE>No</ISSTXNONREALIZEDTYPE>" + "\n" +
"      <ISUSEDFORCVD>No</ISUSEDFORCVD>" + "\n" +
"      <LEDBELONGSTONONTAXABLE>No</LEDBELONGSTONONTAXABLE>" + "\n" +
"      <ISEXCISEMERCHANTEXPORTER>No</ISEXCISEMERCHANTEXPORTER>" + "\n" +
"      <ISPARTYEXEMPTED>No</ISPARTYEXEMPTED>" + "\n" +
"      <ISSEZPARTY>No</ISSEZPARTY>" + "\n" +
"      <TDSDEDUCTEEISSPECIALRATE>No</TDSDEDUCTEEISSPECIALRATE>" + "\n" +
"      <ISECHEQUESUPPORTED>No</ISECHEQUESUPPORTED>" + "\n" +
"      <ISEDDSUPPORTED>No</ISEDDSUPPORTED>" + "\n" +
"      <HASECHEQUEDELIVERYMODE>No</HASECHEQUEDELIVERYMODE>" + "\n" +
"      <HASECHEQUEDELIVERYTO>No</HASECHEQUEDELIVERYTO>" + "\n" +
"      <HASECHEQUEPRINTLOCATION>No</HASECHEQUEPRINTLOCATION>" + "\n" +
"      <HASECHEQUEPAYABLELOCATION>No</HASECHEQUEPAYABLELOCATION>" + "\n" +
"      <HASECHEQUEBANKLOCATION>No</HASECHEQUEBANKLOCATION>" + "\n" +
"      <HASEDDDELIVERYMODE>No</HASEDDDELIVERYMODE>" + "\n" +
"      <HASEDDDELIVERYTO>No</HASEDDDELIVERYTO>" + "\n" +
"      <HASEDDPRINTLOCATION>No</HASEDDPRINTLOCATION>" + "\n" +
"      <HASEDDPAYABLELOCATION>No</HASEDDPAYABLELOCATION>" + "\n" +
"      <HASEDDBANKLOCATION>No</HASEDDBANKLOCATION>" + "\n" +
"      <ISEBANKINGENABLED>No</ISEBANKINGENABLED>" + "\n" +
"      <ISEXPORTFILEENCRYPTED>No</ISEXPORTFILEENCRYPTED>" + "\n" +
"      <ISBATCHENABLED>No</ISBATCHENABLED>" + "\n" +
"      <ISPRODUCTCODEBASED>No</ISPRODUCTCODEBASED>" + "\n" +
"      <HASEDDCITY>No</HASEDDCITY>" + "\n" +
"      <HASECHEQUECITY>No</HASECHEQUECITY>" + "\n" +
"      <ISFILENAMEFORMATSUPPORTED>No</ISFILENAMEFORMATSUPPORTED>" + "\n" +
"      <HASCLIENTCODE>No</HASCLIENTCODE>" + "\n" +
"      <PAYINSISBATCHAPPLICABLE>No</PAYINSISBATCHAPPLICABLE>" + "\n" +
"      <PAYINSISFILENUMAPP>No</PAYINSISFILENUMAPP>" + "\n" +
"      <ISSALARYTRANSGROUPEDFORBRS>No</ISSALARYTRANSGROUPEDFORBRS>" + "\n" +
"      <ISEBANKINGSUPPORTED>No</ISEBANKINGSUPPORTED>" + "\n" +
"      <ISSCBUAE>No</ISSCBUAE>" + "\n" +
"      <ISBANKSTATUSAPP>No</ISBANKSTATUSAPP>" + "\n" +
"      <ISSALARYGROUPED>No</ISSALARYGROUPED>" + "\n" +
"      <USEFORPURCHASETAX>No</USEFORPURCHASETAX>" + "\n" +
"      <AUDITED>No</AUDITED>" + "\n" +
"      <SORTPOSITION> 1000</SORTPOSITION>" + "\n" +
"      <ALTERID> 343</ALTERID>" + "\n" +
"      <SERVICETAXDETAILS.LIST>      </SERVICETAXDETAILS.LIST>" + "\n" +
"      <LBTREGNDETAILS.LIST>      </LBTREGNDETAILS.LIST>" + "\n" +
"      <VATDETAILS.LIST>      </VATDETAILS.LIST>" + "\n" +
"      <SALESTAXCESSDETAILS.LIST>      </SALESTAXCESSDETAILS.LIST>" + "\n" +
"      <GSTDETAILS.LIST>" + "\n" +
"       <APPLICABLEFROM>20170701</APPLICABLEFROM>" + "\n" +
"       <HSNMASTERNAME/>" + "\n" +
"       <TAXABILITY>Taxable</TAXABILITY>" + "\n" +
"       <GSTNATUREOFTRANSACTION>Interstate Sales Taxable</GSTNATUREOFTRANSACTION>" + "\n" +
"       <ISREVERSECHARGEAPPLICABLE>No</ISREVERSECHARGEAPPLICABLE>" + "\n" +
"       <ISNONGSTGOODS>No</ISNONGSTGOODS>" + "\n" +
"       <GSTINELIGIBLEITC>No</GSTINELIGIBLEITC>" + "\n" +
"       <INCLUDEEXPFORSLABCALC>No</INCLUDEEXPFORSLABCALC>" + "\n" +
"       <STATEWISEDETAILS.LIST>" + "\n" +
"        <STATENAME>&#4; Any</STATENAME>" + "\n" +
"        <RATEDETAILS.LIST>" + "\n" +
"         <GSTRATEDUTYHEAD>Central Tax</GSTRATEDUTYHEAD>" + "\n" +
"         <GSTRATEVALUATIONTYPE>Based on Value</GSTRATEVALUATIONTYPE>" + "\n" +
"         <GSTRATE> 6</GSTRATE>" + "\n" +
"        </RATEDETAILS.LIST>" + "\n" +
"        <RATEDETAILS.LIST>" + "\n" +
"         <GSTRATEDUTYHEAD>State Tax</GSTRATEDUTYHEAD>" + "\n" +
"         <GSTRATEVALUATIONTYPE>Based on Value</GSTRATEVALUATIONTYPE>" + "\n" +
"         <GSTRATE> 6</GSTRATE>" + "\n" +
"        </RATEDETAILS.LIST>" + "\n" +
"        <RATEDETAILS.LIST>" + "\n" +
"         <GSTRATEDUTYHEAD>Integrated Tax</GSTRATEDUTYHEAD>" + "\n" +
"         <GSTRATEVALUATIONTYPE>Based on Value</GSTRATEVALUATIONTYPE>" + "\n" +
"         <GSTRATE> 12</GSTRATE>" + "\n" +
"        </RATEDETAILS.LIST>" + "\n" +
"        <RATEDETAILS.LIST>" + "\n" +
"         <GSTRATEDUTYHEAD>Cess</GSTRATEDUTYHEAD>" + "\n" +
"         <GSTRATEVALUATIONTYPE>Based on Value</GSTRATEVALUATIONTYPE>" + "\n" +
"        </RATEDETAILS.LIST>" + "\n" +
"        <RATEDETAILS.LIST>" + "\n" +
"         <GSTRATEDUTYHEAD>Cess on Qty</GSTRATEDUTYHEAD>" + "\n" +
"         <GSTRATEVALUATIONTYPE>Based on Quantity</GSTRATEVALUATIONTYPE>" + "\n" +
"        </RATEDETAILS.LIST>" + "\n" +
"        <GSTSLABRATES.LIST>        </GSTSLABRATES.LIST>" + "\n" +
"       </STATEWISEDETAILS.LIST>" + "\n" +
"       <TEMPGSTDETAILSLABRATES.LIST>       </TEMPGSTDETAILSLABRATES.LIST>" + "\n" +
"      </GSTDETAILS.LIST>" + "\n" +
"      <GSTDETAILS.LIST>" + "\n" +
"       <APPLICABLEFROM>20171130</APPLICABLEFROM>" + "\n" +
"       <HSNMASTERNAME/>" + "\n" +
"       <TAXABILITY/>" + "\n" +
"       <ISREVERSECHARGEAPPLICABLE>No</ISREVERSECHARGEAPPLICABLE>" + "\n" +
"       <ISNONGSTGOODS>No</ISNONGSTGOODS>" + "\n" +
"       <GSTINELIGIBLEITC>No</GSTINELIGIBLEITC>" + "\n" +
"       <INCLUDEEXPFORSLABCALC>No</INCLUDEEXPFORSLABCALC>" + "\n" +
"       <STATEWISEDETAILS.LIST>" + "\n" +
"        <STATENAME>&#4; Any</STATENAME>" + "\n" +
"        <RATEDETAILS.LIST>" + "\n" +
"         <GSTRATEDUTYHEAD>Central Tax</GSTRATEDUTYHEAD>" + "\n" +
"         <GSTRATEVALUATIONTYPE>Based on Value</GSTRATEVALUATIONTYPE>" + "\n" +
"        </RATEDETAILS.LIST>" + "\n" +
"        <RATEDETAILS.LIST>" + "\n" +
"         <GSTRATEDUTYHEAD>State Tax</GSTRATEDUTYHEAD>" + "\n" +
"         <GSTRATEVALUATIONTYPE>Based on Value</GSTRATEVALUATIONTYPE>" + "\n" +
"        </RATEDETAILS.LIST>" + "\n" +
"        <RATEDETAILS.LIST>" + "\n" +
"         <GSTRATEDUTYHEAD>Integrated Tax</GSTRATEDUTYHEAD>" + "\n" +
"         <GSTRATEVALUATIONTYPE>Based on Value</GSTRATEVALUATIONTYPE>" + "\n" +
"        </RATEDETAILS.LIST>" + "\n" +
"        <RATEDETAILS.LIST>" + "\n" +
"         <GSTRATEDUTYHEAD>Cess</GSTRATEDUTYHEAD>" + "\n" +
"         <GSTRATEVALUATIONTYPE>Based on Value</GSTRATEVALUATIONTYPE>" + "\n" +
"        </RATEDETAILS.LIST>" + "\n" +
"        <RATEDETAILS.LIST>" + "\n" +
"         <GSTRATEDUTYHEAD>Cess on Qty</GSTRATEDUTYHEAD>" + "\n" +
"         <GSTRATEVALUATIONTYPE>Based on Quantity</GSTRATEVALUATIONTYPE>" + "\n" +
"        </RATEDETAILS.LIST>" + "\n" +
"        <GSTSLABRATES.LIST>        </GSTSLABRATES.LIST>" + "\n" +
"       </STATEWISEDETAILS.LIST>" + "\n" +
"       <TEMPGSTDETAILSLABRATES.LIST>       </TEMPGSTDETAILSLABRATES.LIST>" + "\n" +
"      </GSTDETAILS.LIST>" + "\n" +
"      <LANGUAGENAME.LIST>" + "\n" +
"       <NAME.LIST TYPE=\"String\">" + "\n" +
"        <NAME>Sales A/c</NAME>" + "\n" +
"       </NAME.LIST>" + "\n" +
"       <LANGUAGEID> 1033</LANGUAGEID>" + "\n" +
"      </LANGUAGENAME.LIST>" + "\n" +
"      <XBRLDETAIL.LIST>      </XBRLDETAIL.LIST>" + "\n" +
"      <AUDITDETAILS.LIST>      </AUDITDETAILS.LIST>" + "\n" +
"      <SCHVIDETAILS.LIST>      </SCHVIDETAILS.LIST>" + "\n" +
"      <EXCISETARIFFDETAILS.LIST>      </EXCISETARIFFDETAILS.LIST>" + "\n" +
"      <TCSCATEGORYDETAILS.LIST>      </TCSCATEGORYDETAILS.LIST>" + "\n" +
"      <TDSCATEGORYDETAILS.LIST>      </TDSCATEGORYDETAILS.LIST>" + "\n" +
"      <SLABPERIOD.LIST>      </SLABPERIOD.LIST>" + "\n" +
"      <GRATUITYPERIOD.LIST>      </GRATUITYPERIOD.LIST>" + "\n" +
"      <ADDITIONALCOMPUTATIONS.LIST>      </ADDITIONALCOMPUTATIONS.LIST>" + "\n" +
"      <EXCISEJURISDICTIONDETAILS.LIST>      </EXCISEJURISDICTIONDETAILS.LIST>" + "\n" +
"      <EXCLUDEDTAXATIONS.LIST>      </EXCLUDEDTAXATIONS.LIST>" + "\n" +
"      <BANKALLOCATIONS.LIST>      </BANKALLOCATIONS.LIST>" + "\n" +
"      <PAYMENTDETAILS.LIST>      </PAYMENTDETAILS.LIST>" + "\n" +
"      <BANKEXPORTFORMATS.LIST>      </BANKEXPORTFORMATS.LIST>" + "\n" +
"      <BILLALLOCATIONS.LIST>      </BILLALLOCATIONS.LIST>" + "\n" +
"      <INTERESTCOLLECTION.LIST>      </INTERESTCOLLECTION.LIST>" + "\n" +
"      <LEDGERCLOSINGVALUES.LIST>      </LEDGERCLOSINGVALUES.LIST>" + "\n" +
"      <LEDGERAUDITCLASS.LIST>      </LEDGERAUDITCLASS.LIST>" + "\n" +
"      <OLDAUDITENTRIES.LIST>      </OLDAUDITENTRIES.LIST>" + "\n" +
"      <TDSEXEMPTIONRULES.LIST>      </TDSEXEMPTIONRULES.LIST>" + "\n" +
"      <DEDUCTINSAMEVCHRULES.LIST>      </DEDUCTINSAMEVCHRULES.LIST>" + "\n" +
"      <LOWERDEDUCTION.LIST>      </LOWERDEDUCTION.LIST>" + "\n" +
"      <STXABATEMENTDETAILS.LIST>      </STXABATEMENTDETAILS.LIST>" + "\n" +
"      <LEDMULTIADDRESSLIST.LIST>      </LEDMULTIADDRESSLIST.LIST>" + "\n" +
"      <STXTAXDETAILS.LIST>      </STXTAXDETAILS.LIST>" + "\n" +
"      <CHEQUERANGE.LIST>      </CHEQUERANGE.LIST>" + "\n" +
"      <DEFAULTVCHCHEQUEDETAILS.LIST>      </DEFAULTVCHCHEQUEDETAILS.LIST>" + "\n" +
"      <ACCOUNTAUDITENTRIES.LIST>      </ACCOUNTAUDITENTRIES.LIST>" + "\n" +
"      <AUDITENTRIES.LIST>      </AUDITENTRIES.LIST>" + "\n" +
"      <BRSIMPORTEDINFO.LIST>      </BRSIMPORTEDINFO.LIST>" + "\n" +
"      <AUTOBRSCONFIGS.LIST>      </AUTOBRSCONFIGS.LIST>" + "\n" +
"      <BANKURENTRIES.LIST>      </BANKURENTRIES.LIST>" + "\n" +
"      <DEFAULTCHEQUEDETAILS.LIST>      </DEFAULTCHEQUEDETAILS.LIST>" + "\n" +
"      <DEFAULTOPENINGCHEQUEDETAILS.LIST>      </DEFAULTOPENINGCHEQUEDETAILS.LIST>" + "\n" +
"      <CANCELLEDPAYALLOCATIONS.LIST>      </CANCELLEDPAYALLOCATIONS.LIST>" + "\n" +
"      <ECHEQUEPRINTLOCATION.LIST>      </ECHEQUEPRINTLOCATION.LIST>" + "\n" +
"      <ECHEQUEPAYABLELOCATION.LIST>      </ECHEQUEPAYABLELOCATION.LIST>" + "\n" +
"      <EDDPRINTLOCATION.LIST>      </EDDPRINTLOCATION.LIST>" + "\n" +
"      <EDDPAYABLELOCATION.LIST>      </EDDPAYABLELOCATION.LIST>" + "\n" +
"      <AVAILABLETRANSACTIONTYPES.LIST>      </AVAILABLETRANSACTIONTYPES.LIST>" + "\n" +
"      <LEDPAYINSCONFIGS.LIST>      </LEDPAYINSCONFIGS.LIST>" + "\n" +
"      <TYPECODEDETAILS.LIST>      </TYPECODEDETAILS.LIST>" + "\n" +
"      <FIELDVALIDATIONDETAILS.LIST>      </FIELDVALIDATIONDETAILS.LIST>" + "\n" +
"      <INPUTCRALLOCS.LIST>      </INPUTCRALLOCS.LIST>" + "\n" +
"      <GSTCLASSFNIGSTRATES.LIST>      </GSTCLASSFNIGSTRATES.LIST>" + "\n" +
"      <EXTARIFFDUTYHEADDETAILS.LIST>      </EXTARIFFDUTYHEADDETAILS.LIST>" + "\n" +
"      <VOUCHERTYPEPRODUCTCODES.LIST>      </VOUCHERTYPEPRODUCTCODES.LIST>" + "\n" +
"     </LEDGER>" + "\n" +
"    </TALLYMESSAGE>\n";

         string VoucherFooter =
 "     </REQUESTDATA>" + "\n" +
 "   </IMPORTDATA>" + "\n" +
 "  </BODY>" + "\n" +
 "</ENVELOPE>";


         public string MasterledgervoucherXML(int? ddl_marketplace, int? sellers_id, string CompanyName)
         {
             string xmloutput = "";
             try
             {
                 var getAllExpense = dba.m_settlement_fee.Where(a => a.return_fee != "Goodwill" && a.return_fee != "Principal" && a.return_fee != "Product Tax" && a.return_fee != "Shipping" && a.return_fee != "Shipping tax" && a.remarks == "Amazon").ToList();

                 var getAllTax = dba.tbl_Salesledger_tax.ToList();
                 if (getAllExpense != null)
                 {
                    

                     string myvoucherHeader = VoucherHeader;
                     xmloutput += myvoucherHeader;

                     string myvoucherReserved = voucheReserverd;
                     xmloutput += myvoucherReserved;

                     string myvoucherReserved1 = voucheReserverd1;
                     xmloutput += myvoucherReserved1;

                     string myvoucherBranch = voucherBranch;
                     xmloutput += myvoucherBranch;

                     string myvoucherCapital = VoucherCapital;
                     xmloutput += myvoucherCapital;

                     string myvouchercurrentasset = VoucherCurrentAsset;
                     xmloutput += myvouchercurrentasset;

                     string myvouchercurrentliabilities = VoucherCurrentLiabilities;
                     xmloutput += myvouchercurrentliabilities;

                     string myvoucherdirectexpenses = voucherDirectExpenses;
                     xmloutput += myvoucherdirectexpenses;

                     string myvoucherdirectincome = DirectIncome;
                     xmloutput += myvoucherdirectincome;

                     string myvoucherfixedassets = FixedAssets;
                     xmloutput += myvoucherfixedassets;

                     string myvoucherindirectexpenses = VoucherIndirectExpenses;
                     xmloutput += myvoucherindirectexpenses;

                     string myvoucherindirectincome = voucherIndirectIncome;
                     xmloutput += myvoucherindirectincome;

                     string myvoucherinvestment = VoucherInvestment;
                     xmloutput += myvoucherinvestment;

                     string myvoucherloans = VoucherLoans;
                     xmloutput += myvoucherloans;

                     string myvouchermiscexpenses = VoucherMiscExpenses;
                     xmloutput += myvouchermiscexpenses;

                     string myvoucherpurchaseaccount = VoucherPurchaseAccount;
                     xmloutput += myvoucherpurchaseaccount;

                     string myvouchersaleaccount = VoucherSalesAccounts;
                     xmloutput += myvouchersaleaccount;

                     string myvouchersuspenseaccount = VoucherSuspenseAccount;
                     xmloutput += myvouchersuspenseaccount;

                     string myvoucherbankaccount = VoucherBankAccount;
                     xmloutput += myvoucherbankaccount;

                     string myvoucherbankodaccount = VoucherBankODAccounts;
                     xmloutput += myvoucherbankodaccount;

                     string myvouchercashinhand = VoucherCashinHand;
                     xmloutput += myvouchercashinhand;

                     string myvoucherdeposit = VoucherDeposit;
                     xmloutput += myvoucherdeposit;

                     string myvoucherdutiesandtaxes = VoucherDutiesandTaxes;
                     xmloutput += myvoucherdutiesandtaxes;

                     string myvoucherloansadvances = VoucherLoansandAdvances;
                     xmloutput += myvoucherloansadvances;

                     string myvoucherprovision = VoucherProvision;
                     xmloutput += myvoucherprovision;

                     string myvoucherreservesurplus = VoucherReserveandSurplus;
                     xmloutput += myvoucherreservesurplus;

                     string myvouchersecruedloans = VoucherSecruedLoans;
                     xmloutput += myvouchersecruedloans;

                     string myvoucherstockinhand = VoucherStockinHand;
                     xmloutput += myvoucherstockinhand;

                     string myvouchersundrycreditors = VoucherSundryCreditors;
                     xmloutput += myvouchersundrycreditors;

                     string myvouchersundrydebtors = VoucherSundryDebtors;
                     xmloutput += myvouchersundrydebtors;

                     string myvoucherunsecruedloans = VoucherUnSecuredLoans;
                     xmloutput += myvoucherunsecruedloans;

                     string myvoucherprofitandloss = VoucherProfitandLoss;
                     xmloutput += myvoucherprofitandloss;

                     string myvoucheramazonreceipt = VoucherAmazonReceipt;
                     xmloutput += myvoucheramazonreceipt;

                     string myvouchercash = VoucherCash;
                     xmloutput += myvouchercash;

                     foreach (var item in getAllExpense)
                     {
                         Guid g;
                         g = Guid.NewGuid();
                         string Name = item.return_fee;

                         string myvoucherexpensetype = VoucherExpenseType;
                         myvoucherexpensetype = myvoucherexpensetype.Replace("#guiduniqueno#", g.ToString());
                         myvoucherexpensetype = myvoucherexpensetype.Replace("#ExpenseName#", Name);

                         xmloutput += myvoucherexpensetype;
                     }// end if foreach loop

                     if (getAllTax != null)
                     {
                         foreach (var taxtype in getAllTax)
                         {
                             Guid g1;
                             g1 = Guid.NewGuid();
                             string TaxName = taxtype.tax_name;
                             string myvouchertaxtype = VoucherTaxType;
                             myvouchertaxtype = myvouchertaxtype.Replace("#Guiduniqueno#", g1.ToString());
                             myvouchertaxtype = myvouchertaxtype.Replace("#TaxName#", TaxName);

                             xmloutput += myvouchertaxtype;
                         }
                     }

                     string myvouchercurrentReserveamount = VoucherCurrentReserveAmount;
                     xmloutput += myvouchercurrentReserveamount;

                     string myvouchernonsubscription = VoucherNonSubscription;
                     xmloutput += myvouchernonsubscription;

                     string myvoucherordercancellation = OrderCancellation;
                     xmloutput += myvoucherordercancellation;

                     string myvoucherpreviousamount = VoucherPreviousAmount;
                     xmloutput += myvoucherpreviousamount;

                     string myvouchersalesacoountsub = VoucherSalesAccountsub;
                     xmloutput += myvouchersalesacoountsub;

                     string myvoucherfooter = VoucherFooter;
                     xmloutput += myvoucherfooter;

                 }// end of if(getAllExpense)
             }
             catch(Exception ex)
             {
             }
             return (xmloutput);
         }

    }
}