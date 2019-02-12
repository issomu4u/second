//function CountrowDirector(elem) {
//    debugger;
//    $('#tblDirector_Name tbody tr').each(function () {
//        var rowCount = $('#tblDirector_Name tbody tr').length;
//        if (rowCount == 1) {
//            bootbox.alert("one row is mandatory");
//            DeleteRow(elem);
//        }
//        else {
//            DeleteRow(elem);
//            return false;
//        }
//    });
//}


//function AddNewrowDirectors() {

//    var i = 0;

//    var rowCount = 0;

//    $('#tblDirector_Name tbody tr').each(function () {
//        rowCount = $('#tblDirector_Name tbody tr').length;
//    })

//    $("#tblDirector_Name tbody").append(
//        "<tr>" +
//      "<td><select id='m_marketplace_id' class='validate[required] text-input form-control 'style='width: 135px;'; > " + $('#m_marketplace_id').html() + "</select><input type='hidden'></td>" +
//      "<td><input type='text' class = 'validate[required] text-input' style='width: 100%'; /></td>" +         
//      "<td><a class='delete'  title='Delete' data-toggle='modal' href='#' onclick='return CountrowDirector($(this));' ><i class='glyphicon glyphicon-trash'></i></a></td>" +

//      "</tr>");

//}

//function DeleteRow(elem) {
//    $('#sp_msg').empty();
//    var par = elem.parent().parent();
//    par.remove();
//};


//function SavePurchaseDetails() {
//    debugger;
   
//    var arr = new Array();

//    $('#tblDirector_Name tbody tr').each(function (i, tr) {

//        arr[i] = {
//            InventoryID: ($(tr).find("td:nth-child(1) select")).find('option:selected').val(),
//            ITEMCOUNT: ($(tr).find("td:nth-child(2) input[type=text]")).val()
           
//        }

//    });
//    console.log(arr[i]);
//    $('#XmlSalesDetails').val(JSON.stringify(arr));
//}

function DeleteRow(elem) {
    $('#sp_msg').empty();
    var par = elem.parent().parent();
    par.remove();
};
function CountrowDirector(elem) {
    debugger;
    $('#tblDirector_Name tbody tr').each(function () {
        var rowCount = $('#tblDirector_Name tbody tr').length;
        if (rowCount == 1) {
            bootbox.alert("one row is mandatory");
        }
        else {
            DeleteRow(elem);
            return false;
        }
    });
}

function AddNewrowDirectors() {

    var i = 0;

    $("#tblDirector_Name tbody").append(
        "<tr>" +
     "<td><input type='text' class = 'validate[required,custom[onlyLetterSp]] text-input form-control'" + i + "/></td>" +
      "<td><input type='text'  class = 'validate[required,custom[phone],minSize[10],maxSize[15]] text-input form-control'  /></td>" +
      "<td><input type='text'   class = 'validate[required,custom[email]] form-control' /></td>" +
      "<td><a class='delete' onclick='return CountrowDirector($(this));' ></a></td>" +
      "</tr>");

}