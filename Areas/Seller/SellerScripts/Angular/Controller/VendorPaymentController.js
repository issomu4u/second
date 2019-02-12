angular.module('MyAppSeller').controller('VendorPaymentController', function ($scope, VendorPaymentServices) {

    $scope.POList = null;
    $scope.currentPage = 1;
    GetAllPONumber();
    GetAllPOVendorList();

    //To Get All Records  
    function GetAllPONumber() {
        var getData = VendorPaymentServices.GetPOList();
        getData.then(function (emp) {
            $scope.POList = emp.data;
            //console.log("PO list");
            //console.log($scope.POList);
        }, function (emp) {
            //alert("Records gathering failed!");
        });
    }

    function GetAllPOVendorList() {

        var getData = VendorPaymentServices.GetPOVendorList();
        getData.then(function (emp) {
            $scope.POVendorList = emp.data;
            angular.forEach($scope.POVendorList, function (obj) {
                obj["showEdit"] = true;
                obj["showDelete"] = true;
            })
        }, function (emp) {
            //alert("Records gathering failed!");
        });
    }

    $scope.toggleEdit = function (emp) {
        
        $scope.hdn_Purchase_id = emp.ob_tbl_purchase.id;
       // alert("hidden value " + $scope.hdn_Purchase_id);
        $scope.tbl_vendor_id = emp.ob_tbl_purchase.tbl_seller_vendors_id;
        //alert("hidden valuess " + $scope.tbl_vendor_id);
        $scope.po_invoiceAmount = emp.ob_tbl_purchase.invoice_amount;
        $scope.po_number = emp.ob_tbl_purchase.po_number;
        $scope.po_date = emp.ob_tbl_purchase.po_date;      
        $scope.Action = "Add";
        $scope.IsVisible = false;
        IsFormSubmitted = false;
        //ClearPOFields();
    }


    $scope.toggleView = function (emp) {
        $("input[name=chkPassPort][value=1]").prop("checked", false);
        $("input[name=chkPassPort][value=2]").prop("checked", false);
        $scope.hdn_Purchase_id = emp.ob_tbl_purchase.id;        
        $scope.tbl_vendor_id = emp.ob_tbl_purchase.tbl_seller_vendors_id;        
        $scope.po_invoiceAmount = emp.ob_tbl_purchase.invoice_amount;
        $scope.po_number = emp.ob_tbl_purchase.po_number;
        $scope.po_date = emp.ob_tbl_purchase.po_date;
        alert(emp.ob_tbl_vendor_payment.n_paymentMode);
        $scope.n_paymentMode = emp.ob_tbl_vendor_payment.n_paymentMode;
       
        if ($scope.n_paymentMode = "1") {
            $("input[name=chkPassPort][value= 1]").prop("checked", 'checked');
            // $scope.dvPassport=true;
            $scope.dvPassport=true;
        }
        else{
            $("input[name=chkPassPort][value= 2]").prop("checked", 'checked');
            //$('#chkNo').attr('checked');
            $scope.dvPassport = false;
            //$scope.dvPassport.hide();          
        }        
        $scope.t_BankName = emp.ob_tbl_vendor_payment.t_BankName;
        $scope.t_chequeno = emp.ob_tbl_vendor_payment.t_chequeno;
        $scope.t_PurchaseAmount = emp.ob_tbl_vendor_payment.t_PurchaseAmount;
        $scope.t_Remark = emp.ob_tbl_vendor_payment.t_Remark;
        $scope.d_date_of_payment = emp.ob_tbl_vendor_payment.d_date_of_payment;
        //$scope.t_PurchaseAmount = emp.ob_tbl_purchase.t_PurchaseAmount;
    }

    function ClearPOFields() {
        $scope.hdn_Purchase_id = "";
        $scope.tbl_vendor_id = "";
        $scope.po_invoiceAmount = "";
        $scope.po_number = "";
        $scope.n_paymentMode = "";
        $scope.t_BankName = "";
        $scope.t_chequeno = "";
        $scope.t_PurchaseAmount = "";
        $scope.t_Remark = "";
    }
    $scope.chkClick = function () {
        if ($("#chkYes").is(":checked")) {
            //alert("checked");           
            $scope.dvPassport=true;          
            $scope.n_paymentMode = 1;
        } else {
            //alert("unchecked");
            $scope.dvPassport = false;
            $scope.n_paymentMode = 2;         
        }
    }
    $scope.AddUpdatePO = function () {
        //alert("Hello PO");
        var PODetails = {};      
        $scope.t_BankName = $('#t_BankName').val();
        $scope.t_chequeno = $('#t_chequeno').val();
       
        PODetails = {
            
            tbl_purchase_id: $scope.hdn_Purchase_id,
            tbl_vendor_id: $scope.tbl_vendor_id,
            t_BankName: $scope.t_BankName,
            t_chequeno: $scope.t_chequeno,
            t_PurchaseAmount: $scope.t_PurchaseAmount,
            po_number: $scope.po_number,
            po_invoiceAmount: $scope.po_invoiceAmount,
            n_paymentMode: $scope.n_paymentMode,
            t_Remark: $scope.t_Remark,
        };
        var getAction = $scope.Action;
        var getData = VendorPaymentServices.AddVendorPayment(PODetails);
        getData.then(function (msg) {
            
            $scope.msg = msg.Message;
            $("#alertModal").modal('show');
            $scope.msg = msg.Message;
            ClearPOFields();
            GetAllPONumber();          
            }, function (error) {
                alert('Error!');
            });
        }, function (msg) {
            $("#alertModal").modal('show');
            $scope.msg = msg.Message;      
    }


    $scope.alertmsg = function () {
        $("#alertModal").modal('hide');
    };



}).factory('VendorPaymentServices', function ($http, $q) {
    var fac = {};
   
    fac.GetPOList = function () {
        return $http.get('/Seller/Seller/GetAllPONumber');
    }

    fac.GetPOVendorList = function () {
        return $http.get('/Seller/Seller/FillVendorForPO');
    }
    
   
    fac.AddVendorPayment = function (PODetails) {
        //alert("PO ADD")
        var formData = new FormData();
        formData.append("tbl_purchase_id", PODetails.tbl_purchase_id);
        formData.append("tbl_vendor_id", PODetails.tbl_vendor_id);
        formData.append("t_BankName", PODetails.t_BankName);
        formData.append("t_chequeno", PODetails.t_chequeno);
        formData.append("t_PurchaseAmount", PODetails.t_PurchaseAmount);
        formData.append("po_number", PODetails.po_number);
        formData.append("po_invoiceAmount", PODetails.po_invoiceAmount);
        formData.append("n_paymentMode", PODetails.n_paymentMode);
        formData.append("t_Remark", PODetails.t_Remark);
        var defer = $q.defer();
        $http.post("/Seller/Seller/AddVendorPayment", formData,
            {
                withCredentials: true,
                headers: { 'Content-Type': undefined },
                transformRequest: angular.identity
            })
        .success(function (d) {
            defer.resolve(d);
        })
        .error(function () {
            defer.reject("some error occred !!");
        });

        return defer.promise;

    }
   
   

    return fac;

});