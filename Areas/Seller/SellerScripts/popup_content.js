
$(function () {
    $('.datepicker').datepicker(
        {
            changeMonth: true,
            changeYear: true,
            yearRange: "1900:+nn"
        });
});


function setFocus(input)
{

    $(input).focus();
}
function setFocusBack(input) {
    
    //alert($('div[id=dv_PaymentDetails] select:first').html());
    //$('div[id=dv_PaymentDetails] select:first').focus();
   // return false;
    $(input).attr("tabindex", "1");
    //$(input).focus();
}
function setFocusEnter(input1, input2) {

    $(this).keyup(function (e) {
       // alert(e.keyCode);
         if (e.keyCode == 13)
        {
          //  alert(e.keyCode);
            $(input1).focus();
         }
         if (e.keyCode == 9) {
             //  alert(e.keyCode);
             $(input2).focus();
         }
    });
    
}


//alert($('#t_Result').val());
function Msg() {
    $('#msg').html($('#t_Result').val());
}
    //created By Manoj Kumar on 15.11.2013
//for Create Credit group And Budget Group Etc 
function createNewCG_BG(txt_id, actionurl, ddl_id, dv_id) {
    var txt = $('#' + txt_id).val();
    $.getJSON(actionurl,
        { postedvalue: txt },
        function (data) {
            var ddlid = $('#' + ddl_id);
            ddlid.empty();
            $.each(data, function (i, item) {
               // alert(item.Value + " " + item.Text);
                ddlid.append($('<option/>', {
                    value: item.Value,
                    text: item.Text
                }));
            });

        });
    $('#' + dv_id).dialog('close');
}
//for Create New Country 
function createCountry(txt_id1, txt_id2, txt_id3, actionurl, ddl_id, dv_id) {
    var txt1 = $('#'+txt_id1).val();
    var txt2 = $('#'+txt_id2).val() + " (" + $('#'+txt_id3).val() + ")";
   // alert(txt2);
    $.getJSON(actionurl,
        { postedvalue1: txt1, postedvalue2: txt2 },
        function (data) {
            var ddlid = $('#' + ddl_id);
            ddlid.empty();
            $.each(data, function (i, item) {
               // alert(item.Value + " " + item.Text);
                ddlid.append($('<option/>', {
                    value: item.Value,
                    text: item.Text
                }));
            });

        });
    $('#' + dv_id).dialog('close');
}
//Create New State City and Area Etc
function createNewS_C_A(txt_id, txt_parrent_id, actionurl, ddl_id, dv_id) {
    var txt1 = $('#' + txt_id).val();
    var txt2 = $('#' + txt_parrent_id).val();
    $.getJSON(actionurl,
        { postedvalue1: txt1, postedvalue2: txt2 },
        function (data) {
            var ddlid = $('#' + ddl_id);
            ddlid.empty();
            $.each(data, function (i, item) {
               // alert(item.Value + " " + item.Text);
                ddlid.append($('<option/>', {
                    value: item.Value,
                    text: item.Text
                }));
            });

        });
    $('#' + dv_id).dialog('close');
}
//open Popup Window
function openPopup1(dv_id, p_height, p_width) {
    $('#' + dv_id).dialog({
        height: p_height,
        width: p_width
    });
}

//for Close Popup
function closePopup(dv_id) {
   
    $('#'+dv_id).dialog('close');
}
//created By Manoj Kumar on 16.11.2013
//for Bind Country state City Dropdown list using Json
function BindDDLJson(actionUrl,ddl_Selectedvalue_Id,bindDdl_Id) {
    $.getJSON(actionUrl, { SelectedValue: ddl_Selectedvalue_Id.value },
                            function (data) {
                                var ddl_id = $('#'+bindDdl_Id);
                                ddl_id.empty();
                                $.each(data, function (i, item) {
                                    ddl_id.append($('<option/>', {
                                        value: item.Value,
                                        text: item.Text
                                    }));
                                });
                            })
}
//for Edit Bank Temp Details and Contact Edit Details
function Edit_TempObject(objId, actionUrl, dv_Id,dv_title) {
    x = objId;
    console.log(actionUrl);
   // $("tr[id=" + x + "]").fadeOut();
    //setTimeout(function () { $("tr[id=" + x + "]").remove(); }, 1000);
    $.ajax({
        type: "GET",
        url: actionUrl,
        data: { selectedValue: objId },
        datatype: "html",
        success: function (data) {
            //$(document).html("<div id = "+dv_Id+" title="+dv_title+"></div>")
            var banks = $('#' + dv_Id);
            //banks.attr('title', dv_title);
            banks.empty();
            banks.html(data);
            openPopup(dv_Id, '600', '500');
            //banks.dialog();
        }
    });
    
}


function Cancel_TempObject(objId, actionUrl, dv_Id, dv_title) {
    x = objId;
    console.log(actionUrl);
   
    $.ajax({
        type: "GET",
        url: actionUrl,
        data: { selectedValue: objId },
        datatype: "html",
        success: function (data) {
           
            var banks = $('#' + dv_Id);
          
            banks.empty();
            banks.html(data);
            openPopup(dv_Id, '600', '500');
          
        }
    });

}

//for view Bank Temp Details and Contact Edit Details
function view_TempObject(objId, actionUrl, dv_Id, dv_title) {
    x = objId;
  //  alert(x);
    console.log(actionUrl);
  //  $("tr[id=" + x + "]").hide();
  // $('div[id=Bank]').hide();
    $.ajax({
        type: "GET",
        url: actionUrl,
        data: { selectedValue: objId },
        datatype: "html",
        success: function (data) {
          
            var banks = $('#' + dv_Id);
            $('div[id=div_city]').show();
            $('div[id=div_State]').show();

            banks.empty();
            banks.html(data);
            $('div[id=dv_Id]').show();
           // alert("hi jyoti view");
            //openPopup(dv_Id, '600', '500');
          
        }
    });
   
}


function Delete_TempObject(objId, actionUrl) {
    var x = confirm("Confirm Delete");
    if (x) {
        $.getJSON(actionUrl,
        { SelectedValue: objId },
        function (data) {
            $("tr[id=" + objId + "]").fadeOut();
            setTimeout(function () { $("tr[id=" + objId + "]").remove(); }, 1000);

            $('#Contact').fadeOut();

            $('#dv_ContactDialog').fadeIn
            $('#Bank').fadeOut();
            $('#dv_BankDialog').fadeIn();
           // alert("hi jyoti");
        });
    }

}

function GridRefresh(div_id,actionurl) {
   // alert(div_id);
    $('#'+div_id).load(
                    $.ajax(
                    {
                        type: "POST",
                        url: actionurl,
                        data: {},
                        success: function (data) {
                            var partialView = $('#' + div_id);
                            partialView.empty();
                            partialView.html(data);
                        }
                    }
                    )
                   );

}
//For Import
function GridRefreshById(div_id, data1, actionurl) {

    $('#' + div_id).load(
                    $.ajax(
                    {
                        type: "POST",
                        url: actionurl,
                        data: { searchTerm: data1 },
                        success: function (data) {
                            var partialView = $('#' + div_id);
                            partialView.empty();
                            partialView.html(data);
                        }
                    }
                    )
                   );

}
//for Cancel And Fill Data
var PiGridData;
function fillData(source_html, dest_html, btn_dv_id) {
    $('#' + btn_dv_id).fadeOut();
    var ht = $('#' + source_html);
    PiGridData = ht.html();
    ht.html($('#' + dest_html).html());
}
function CancelPi(Dv_Grid, btn_dv_id) {
    $('#' + btn_dv_id).fadeIn();
    var ht = $('#' + Dv_Grid);
    ht.html(PiGridData);
    PiGridData.empty();
}
//For Cancel
function CancelPi1(Dv_Grid) {
    
    var ht = $('#' + Dv_Grid);
    ht.html(PiGridData);
    PiGridData.empty();
}
//for Exp
var ExpData;
function fillDataExp(grid_html, btn_dv_id) {
    $('#' + btn_dv_id).fadeOut();
    var ht = $('#' + grid_html);
    ExpData = ht.html();
   // ht.html($('#' + dest_html).html());
}
function CancelExp(Dv_Grid, btn_dv_id) {
    $('#' + btn_dv_id).fadeIn();
    var ht = $('#' + Dv_Grid);
    ht.html(ExpData);
    ExpData.empty();
}

//testing
var divi;
var fid;
function openPopup(FocusInId, FocusOutID, dv_id, p_height, p_width) {
    fid = FocusOutID;
    var divid = typeof (dv_id) != 'object' ? $('#' + dv_id) : dv_id;
    divi = divid;
    divid.css('height', p_height);
    divid.css('width', p_width);
    divid.fadeIn();
    $('.overlay').fadeIn();
    $('body').width($('body').width());
    $('body').css('overflow', 'hidden');
    $('#'+FocusInId).focus();
}

function openPopupForPI(FocusInId, FocusOutID, dv_id, p_height, p_width) {

    //GetFocusID(FocusInId);
    fid = FocusOutID;
    var divid = $(dv_id);
    divi = dv_id;
    divid.css('height', p_height);
    divid.css('width', p_width);
    divid.fadeIn();
    $('.overlay').fadeIn();
    $('body').width($('body').width());
    $('body').css('overflow', 'hidden');
    $( FocusInId).focus();
}
function openPopupNew(id, input, dv_id, p_height, p_width) {
    GetFocusID(input);
    fid = input;
    var divid = $('#' + dv_id);
    divi = dv_id;
    divid.css('height', p_height);
    divid.css('width', p_width);
    divid.fadeIn();
    $('.overlay').fadeIn();
    $('body').width($('body').width());
    $('body').css('overflow', 'hidden');
    $('#' + id).focus();
}
function closePop() {
    $('.overlay').fadeOut();
   
   // alert(typeof (divi));
   divi.fadeOut();
    $('body').removeAttr('style')
    if ($(fid).length == 1) {
       $(fid).focus();
   }
   else {
       $('#' + fid).focus();
   }
}
function closePopPI() {
    $('.overlay').fadeOut();
    $(divi).fadeOut();
    $('body').removeAttr('style')
    if ($(fid).length == 1) {
        $(fid).focus();
    }
    else {
        $(fid).focus();
    }
}

//For DateTime Picker
$(function () {
    $('.datePicker').datepicker();
});

//convert mts to litter and liter to mts
function MTStoLiterANDliterToMts(id, showin, type) {
    if (type == 'liter') {
        var x = $('#' + id).val();
        var meter = parseFloat(x);
        $('#' + showin).val((meter * 1098).toFixed(3));
    }
    if (type == 'mts') {
        var x = $('#' + id).val();
        var liter = parseFloat(x);
        $('#' + showin).val((liter / 1098).toFixed(3));
    }

}


//for Grn Container
var data1;
function fillData() {
    $('#btn').fadeOut();
    data1 = $('#dv_GrnContent').html();
}
function BackToCont() {
    $('#btn').fadeIn();
    $('#dv_GrnContent').html(data1);
}
//For Contract 
var storeData;
function FillDataSearch() {
    storeData = $('#dv_SearchData').html();
}



function btncancel() {
    // alert(storeData);
    $('#dv_SearchData').html(storeData);
    enable_autocomplete("#n_ContractNo_autocomplete", "/ContractCon/GetAutoCompContNo", "#n_ContractNo")
    enable_autocomplete("#n_ProdID_autocomplete", "/ContractCon/GetAutoCompProd", "#n_ProdID")
    enable_autocomplete("#n_PartyID_autocomplete", "/ContractCon/GetAutoCompPName", "#n_PartyID")

   
}
function changeBtnText() {
    $('#btnContractSave').val("Update");
}




//For
var PidataEdit;

//function CancelPiDetails1() {
//    $('#btnid').fadeIn();
//    //  $('#dv_PiDetailsgv').empty();
//    // alert(PIData);
//    $('#dv_PiDetailsgv').html(PidataEdit);
//}
//For Date Time Picker
//$(function () {
//    $('.date').datepicker();
//}

//    )
 

//For Print Document

//function CallPrint(strid) {
   
//    var prtContent = $('#' + strid);
//    //alert(prtContent.html());
//    var WinPrint = window.open('','','left=100,top=100,width=650,height=600');
//    //alert(prtContent.html())
//   WinPrint.document.write($(prtContent).html());
//   WinPrint.document.close();
//    WinPrint.focus();
//    WinPrint.print();
//    WinPrint.close()   
//}


function CallPrint(strid) {
        var prtContent = $('#' + strid);
        var WinPrint = window.open('','','left=100,top=100,width=650,height=600');
        WinPrint.document.write($('#' + strid).html());
          WinPrint.document.close();
           WinPrint.focus();
       if (WinPrint.print()) {
           WinPrint.close();
       } else {
           WinPrint.close();
       }
  }
 


//function printDiv(divID) {

//    var divElements = $('#'+divID).html();
//    alert(divElements);
//    var oldPage = document.body.innerHTML;
//    document.body.innerHTML = "<html><head><title></title></head><body>" + divElements + "</body>";
//    window.print();
//    document.body.innerHTML = oldPage;
//}

var i=0;
var PageData = new Array();
function SetPage()
{
   //alert(i);
    PageData[i] = $('#dv_PartialView').html();
    i++;
}
function GetPage()
{
    //alert('get');
    i--;
    $('#dv_PartialView').html(PageData[i]);
    //alert(i);
}
//$('.button').bind('click', SetPage());
//$('.button1').bind('click', SetPage());
//$('.popButton').bind('click', SetPage());


function emptyErrordiv() {
    $('#Error').fadeOut();
    $('#Error').empty();
}

function handleError(data) {
    $('#Error').fadeIn().delay(5000).fadeOut();
    $('#Error').html(data + '<input type="button" id="btnError" value="X" calss="popButton" />');
    $("#btnError").bind("click", emptyErrordiv);
    $('#btnError').focus();
}

function GetForm(id, ddlid) {

    $.ajax(
    {
        type: "POST",
        url: "/Enum/GetForm/",
        data: { id: id, ddlId: $('#t_ParentID').val() },
        success: function (data) {
            $('#dv_Enum').html(data);
            $('#t_id').val(ddlid);
            $('#btnSave').attr('onclick', 'setSettingId()');
            openPopup('t_Display', ddlid, 'dv_Enum', '230', '410');
        }
    });
}
// this for Dynaic Pass Id 
function setSettingId() {
   
    var a = $('#t_ParentID').val()
   //  alert($('#' + a).val());
    if (a != null) {
        $('#n_SettingID').val($('#' + a).val());
        console.log(a, $('#' + a).val(), $('#n_SettingID').val());
    }
}

function setSingleParentId(id) {
   // alert(id)
    $('#t_ParentID').val(id);
}


function SaveEnumHost(n_EnumType, url, id, SettingID, Display) {
    $.ajax(
    {
        type: "POST",
        url: "/Enum/Index/",
        data: { n_EnumType: n_EnumType, url: url, id: id, SettingID: SettingID, Display: Display },
        success: function (data) {
            $('#dv_Enum').html(data);
            $('#n_hostid').val(5);
            $('#t_Display').focus();
        }
    });
    openPopup('t_Display', id, 'dv_Enum', '230', '410');
}

var PageDataArray = new Array();
var counter = 0;
var DDVal = new Array();
function getdata() {
    PageDataArray[counter] = $('#dv_PartialView').clone(true, true);
    var temparray = new Array();
     $('#dv_PartialView  select').each(function (i, item) {
         temparray[i] = {
            id: $(this).attr('id'),
            value: $(this).val()
        }
     });
     DDVal[counter] = temparray;
    counter++;
}

function setdata() {
    counter--;
    var tempsetArray = new Array();
    tempsetArray = DDVal[counter]; 
    $('#dv_PartialView').replaceWith(PageDataArray[counter]);
    $.each(tempsetArray, function (i, tr) {
        $('#' + tr.id).val(tr.value);
    });
}



