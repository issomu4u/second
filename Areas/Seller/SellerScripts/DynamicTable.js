
//$(function () {
//    ValidateForm('PurchaseForm');
//})
//function CheckValidform() {
//    IsFormValidate('PurchaseForm')
//    {
//        ValidateForm('PurchaseForm');
//    }
//}

function btnchange() {
    $('#btnsubmit').val("Update");
}

function showpopupInventoryid(btn) {
  
    var str = btn.split("_");
    var id1 = str[1];
    var e = document.getElementsByName("ddl_inventry" + id1);
    var strUser = e[0].options[e[0].selectedIndex].value;
    var id = strUser;
    //alert("You have selected " + strUser + ".");

    $.ajax({
        url: '/Seller/Purchase/FillSalesStatus',
        datatype: "json",
        type: "post",
        contenttype: 'application/json; charset=utf-8',
        async: true,
        data: { id: id },
        
        success: function (data) {
            
            //$("#empList").html(data);
            if (data.tbl_details_item_id == 1) {
                var get_id = data.id;
                $("#Quantity").val(" ");
                $("#hdn_quantity_Id").val(get_id);
                $('#myModal2').modal({                   
                    show: 'false'
                });
            }
            if (data.tbl_details_item_id == 2) {
                var Item_id = data.id;
                $("#batchNo").val(" ");
                $("#ItemserialNo").val(" ");
                $("#hdn_ItemId").val(Item_id);
                $('#myModal4').modal({
                    show: 'false'
                });
            }
            if (data.tbl_details_item_id == 3) {
                $("#empList").html(data);
                var get_id = data.id;   
                $("#serialNo").val(" ");
                $("#hdn_serialNoId").val(get_id);
                $('#myModal3').modal({
                    show: 'false'               
                });
            }
            if (data.tbl_details_item_id == 4) {
                var hdn_Item_id = data.id;
                $('#myModal1').modal({
                    show: 'false'
                });
            }
            
        },
        error: function (xhr) {
            alert('error');
        }
    });
}
function CountrowDirector(elem) {
    debugger;
    $('#tblDirector_Name tbody tr').each(function () {
        var rowCount = $('#tblDirector_Name tbody tr').length;
        if (rowCount == 1) {
            bootbox.alert("one row is mandatory");
            DeleteRow(elem);
        }
        else {
            DeleteRow(elem);
            return false;
        }
    });
}

function AddNewrowDirectors() {

    var i = 0;

    var rowCount = 0;

    $('#tblDirector_Name tbody tr').each(function () {
        rowCount = $('#tblDirector_Name tbody tr').length;
    })
    $("#tblDirector_Name tbody").append(
        "<tr>" +
      "<td><select id='tbl_inventory_id' name='ddl_inventry" + rowCount + "' class='validate[required] text-input form-control 'style='width: 135px;'; > " + $('#tbl_inventory_id').html() + "</select><input type='hidden'></td>" +
      "<td><a  id='btn_" + rowCount + "' title='Add Details' href='#' onclick='showpopupInventoryid(this.id)'style='width: 100%'; ><i class = 'btn btn2 btn-primary'></i></a></td>" +
    
      "<td><input type='text' class = 'validate[required] text-input' style='width: 100%'; /></td>" +
    
    
      "<td><input type='text' class = 'validate[required] text-input' style='width: 100%'; /></td>" +
      "<td><input type='text' class = 'validate[required] text-input' onblur='OnSelect(this)'style='width: 100%'; /></td>" +
      "<td><input type='text' class = 'validate[required] text-input' onblur='OnSelect(this)'style='width: 100%'; /></td>" +
      "<td><input type='text' class = 'validate[required] text-input' onblur='OnSelect(this)'style='width: 100%'; /></td>" +
      "<td><input type='text' class = 'validate[required] text-input' onblur='OnSelect(this)'style='width: 100%'; /></td>" +
      "<td><input type='text' class = 'validate[required] text-input' onblur='OnSelect(this)'style='width: 100%'; /></td>" +

       "<td><input type='text' class = 'validate[required] text-input' onblur='OnSelect(this)'style='width: 100%'; /></td>" +
       "<td><input type='text' class = 'validate[required] text-input' onblur='OnSelect(this)'style='width: 100%'; /></td>" +
       "<td><input type='text' class = 'validate[required] text-input' onblur='OnSelect(this)'style='width: 100%'; /></td>" +
       
   
      "<td><a class='delete'  title='Delete' data-toggle='modal' href='#' onclick='return CountrowDirector($(this));' ><i class='glyphicon glyphicon-trash'></i></a></td>" +
     
      "</tr>");

}



function SavePurchaseDetails() {
    debugger;    
    var arr = new Array();
    $('#tblDirector_Name tbody tr').each(function (i, tr) {

        arr[i] = {
            InventoryID: ($(tr).find("td:nth-child(1) select")).find('option:selected').val(),

            ITEMCOUNT: ($(tr).find("td:nth-child(3) input[type=text]")).val(),
            BaseAmount: ($(tr).find("td:nth-child(4) input[type=text]")).val(),
         
            CGSTTAX: ($(tr).find("td:nth-child(5) input[type=text]")).val(),
            CGSTAMT: ($(tr).find("td:nth-child(6) input[type=text]")).val(),
            IGSTTAX: ($(tr).find("td:nth-child(7) input[type=text]")).val(),
            IGSTAMT: ($(tr).find("td:nth-child(8) input[type=text]")).val(),
           
            SGSTTAX: ($(tr).find("td:nth-child(9) input[type=text]")).val(),
            SGSTAMT: ($(tr).find("td:nth-child(10) input[type=text]")).val(),
            TAXPAID: ($(tr).find("td:nth-child(11) input[type=text]")).val(),
            TOTALTAXAMT: ($(tr).find("td:nth-child(12) input[type=text]")).val()
        }

    });
    console.log(arr[i]);
    $('#XmlPurchaseDetails').val(JSON.stringify(arr));
}
function DeleteRow(elem) {
    $('#sp_msg').empty();
    var par = elem.parent().parent();
    par.remove();
};



function OnSelect(input) {
    debugger;
    var tr = $(input).parent().parent();
    var BUYINGAMT = parseFloat($(tr).find('td:eq(3) input[type="text"]').val() == '' ? '0' : $(tr).find('td:eq(3) input[type="text"]').val())

    var tr = $(input).parent().parent();
    var CGSTTAX = parseFloat($(tr).find('td:eq(4) input[type="text"]').val() == '' ? '0' : $(tr).find('td:eq(4) input[type="text"]').val())
    var tr = $(input).parent().parent();

    var CGSTCALAMT = (BUYINGAMT * (CGSTTAX / 100));

    var CGSTAMOUNT = parseFloat($(tr).find('td:eq(5) input[type="text"]').val(parseFloat(CGSTCALAMT.toFixed(2))));
    var tr = $(input).parent().parent();

    var IGSTTAX = parseFloat($(tr).find('td:eq(6) input[type="text"]').val() == '' ? '0' : $(tr).find('td:eq(6) input[type="text"]').val());
    var tr = $(input).parent().parent();

    var IGSTTAXCALAMT = (BUYINGAMT * (IGSTTAX / 100));

    var IGSTAMOUNT = parseFloat($(tr).find('td:eq(7) input[type="text"]').val(parseFloat(IGSTTAXCALAMT.toFixed(2))));
    var tr = $(input).parent().parent();


 
   // var RATETAX = parseFloat($(tr).find('td:eq(7) input[type="text"]').val() == '' ? '0' : $(tr).find('td:eq(7) input[type="text"]').val());
   // var tr = $(input).parent().parent();

   // var RATETAXCALAMT = (BUYINGAMT * (RATETAX / 100));

    //var RATEAMOUNT = parseFloat($(tr).find('td:eq(8) input[type="text"]').val(parseFloat(RATETAXCALAMT.toFixed(2))));
   // var tr = $(input).parent().parent();

    var SGSTTAX = parseFloat($(tr).find('td:eq(8) input[type="text"]').val() == '' ? '0' : $(tr).find('td:eq(8) input[type="text"]').val());
    var tr = $(input).parent().parent();

    var SGSTTAXCALAMT = (BUYINGAMT * (SGSTTAX / 100));

    var SGSTAMOUNT = parseFloat($(tr).find('td:eq(9) input[type="text"]').val(parseFloat(SGSTTAXCALAMT.toFixed(2))));
    var tr = $(input).parent().parent();

    //var FinalTotal = (parseFloat(CGSTTAX) + parseFloat(IGSTTAX) + parseFloat(RATETAX) + parseFloat(SGSTTAX));
    //$(input).parent().parent().find('td:eq(11) input[type="text"]').val(parseInt(FinalTotal.toFixed(2)));

    var FinalTAXTotal = (parseFloat(CGSTCALAMT) + parseFloat(IGSTTAXCALAMT) + parseFloat(SGSTTAXCALAMT));
    $(input).parent().parent().find('td:eq(10) input[type="text"]').val(parseFloat(FinalTAXTotal.toFixed(2)));

    var TaxTotal = (parseFloat(BUYINGAMT) + parseFloat(FinalTAXTotal));
    $(input).parent().parent().find('td:eq(11) input[type="text"]').val(parseFloat(TaxTotal.toFixed(2)));


    Totals();

}


function Totals() {
    debugger;
    $("#tblDirector_Name tfoot tr").empty();

    var PItem = 0;
    var PItem1 = 0;
    $('#tblDirector_Name tbody tr').each(function (i, tr) {
        PItem += parseFloat(($(tr).find("td:nth-child(11)  input[type=text]")).val());

        PItem1 += parseFloat(($(tr).find("td:nth-child(12)  input[type=text]")).val())

    });
    $("#tblDirector_Name tfoot tr").append(
        "<td></td>" + "<td></td>" + "<td></td>" + "<td></td>" + "<td></td>" + "<td></td>" + "<td></td>" + "<td></td>" + "<td></td>" + "<td>Totals</td>" +
         "<td><input type='text' value=" + PItem + " class = 'validate[required] text-input form-control'  /></td>" +
         "<td><input type='text' value=" + PItem1 + " class = 'validate[required] text-input form-control'  /></td>"
     )

    $("#tax_amount").val(PItem);
    $("#invoice_amount").val(PItem1);


}


//$(function () {
//    $("#po_date").datepicker({
//        showOtherMonths: true,
//        selectOtherMonths: false,
//        format: "dd-MM-yyyy",
//        autoclose: true,
//        startDate: new Date(),

//    });
//    $("#date_invoice").datepicker({
//        showOtherMonths: true,
//        selectOtherMonths: false,
//        format: "dd-MM-yyyy",
//        autoclose: true,
//        startDate: new Date(),

//    });
//});

function GetPurchaseAttachment() {
    debugger;
    $('#btnsubmit').hide();
    $('#btnsubmit').show(1000);
    var n_RequestedId = $("#id").val();
    if (n_RequestedId >= 0) {
        debugger;
        var a = document.getElementById('file');
        var filename = $("input[name=HelpDeskAttachUpload]").val();
        filename = filename.replace(/C:\\fakepath\\/i, "");
        if (filename == "") {
            var filename = $("#invoice_photo_path").val();
            $("#invoice_photo_path").val(filename);
        }
        $("#invoice_photo_path").val(filename);
    }
    else {
        var fileInput = document.getElementById('file');
        for (i = 0; i < fileInput.files.length; i++) {
            var AttachedResume = fileInput.files[i].name;
            var Attached = fileInput.files[i];
            $("#invoice_photo_path").val(AttachedResume);
        }
        var fileName = $("#invoice_photo_path").val();
        $("#invoice_photo_path").val(fileName);

    }

}

function SavePurchaseAttachment() {
    debugger;
    var formdata = new FormData();
    var fileInput = document.getElementById('file');

    for (i = 0; i < fileInput.files.length; i++) {

        formdata.append(fileInput.files[i].name, fileInput.files[i]);
    }

    var xhr = new XMLHttpRequest();
    xhr.open('POST', '/Purchase/Attachment', true);
    xhr.send(formdata);
    xhr.onreadystatechange = function () {
        if (xhr.readyState == 4 && xhr.status == 200) {
            var a = xhr.responseText;
        }
    }
    return false;
}


function CountrowItem(elem) {
    debugger;
    $('#tblItem_Name tbody tr').each(function () {
        var rowCount = $('#tblItem_Name tbody tr').length;
        if (rowCount == 1) {
            bootbox.alert("one row is mandatory");
            DeleteRows(elem);
        }
        else {
            DeleteRows(elem);
            return false;
        }
    });
}
function AddNewrowItem() {
    var i = 0;
    var rowCount = 0;
    $('#tblItem_Name tbody tr').each(function () {
        rowCount = $('#tblItem_Name tbody tr').length;
    })
    $("#tblItem_Name tbody").append(
        "<tr>" +   
      "<td><input type='text' class = 'validate[required] text-input' style='width: 100%'; /></td>" +
      "<td><input type='text' class = 'validate[required] text-input' style='width: 100%'; /></td>" +
      "<td><a class='delete'  title='Delete' data-toggle='modal' href='#' onclick='return CountrowItem($(this));' ><i class='glyphicon glyphicon-trash'></i></a></td>" +
      "</tr>");
}

function SaveItemDetails() {
    debugger;
    var arr = new Array();
    $('#tblItem_Name tbody tr').each(function (i, tr) {
        arr[i] = {         
            BATCHNO: ($(tr).find("td:nth-child(1) input[type=text]")).val(),
            QUANTITY: ($(tr).find("td:nth-child(2) input[type=text]")).val()          
        }
    });
    console.log(arr[i]);
    $('#XmlItemquantity').val(JSON.stringify(arr));
}

function DeleteRows(elem) {
    $('#sp_msg').empty();
    var par = elem.parent().parent();
    par.remove();
};