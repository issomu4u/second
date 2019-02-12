function printExporter(divid, title) {
    debugger;

    var options = { mode: "iframe", popClose: true, popTitle: title, popHt: 500, popWd: 400, popX: 500, popY: 600, printBodyOptions: { styleToAdd: 'margin-left:10px;display:none' } };
    $(divid).printArea(options);


    //var headstr = "<html><head><title>" + title + "</title></head><body><h2 style='text-align:center'>" + title + "</h2>";
    //var footstr = "</body>";
    ////var newstr = document.all.item($("#Prinsu")).innerHTML;
    //var newstr = divid;
    //var oldstr = document.body.innerHTML;
    //document.body.innerHTML = headstr + newstr + footstr;

    //window.print();
    //document.body.innerHTML = oldstr;
    //return false;
}