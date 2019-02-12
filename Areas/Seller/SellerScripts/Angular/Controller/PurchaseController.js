angular.module('MyAppseller2').controller('PurchaseController', function ($scope, PurchaseServices) {

    $scope.Message = "";
    $scope.invoice_amount = "";
    $scope.invoice_no = "";
    $scope.po_number = "";
    $scope.tax_amount = "";
    $scope.remarks_po = "";
    $scope.FileInvalidMessage = "";
    $scope.SelectedFileForUpload = null;
    $scope.IsFormSubmitted = false;
    $scope.IsFormValid = false;
    $scope.id = null;
    $scope.currentPage = 1;
    $scope.SellerList = null;

    $scope.numberlength = "";
    GetVendorNameList();
    GetWarehouseNameList();
    GetAllPurchaseList();

    function GetVendorNameList() {
        var getData = PurchaseServices.GetVendorNameforPurchase();
        getData.then(function (eve) {
            $scope.id = eve.data.id;
            $scope.VendorName = eve.data;
        }, function (eve) {
            alert("Records gathering failed!");
        });
    }
    
    function GetWarehouseNameList() {
        var getData = PurchaseServices.GetWarehouseNameforPurchase();
        getData.then(function (eve) {
            $scope.id = eve.data.id;
            $scope.WareHouse = eve.data;
        }, function (eve) {
            alert("Records gathering failed!");
        });
    }


    $scope.pageChangeHandler = function (num) {
        console.log('meals page changed to ' + num);
    };



    function GetAllPurchaseList() {

        var getData = PurchaseServices.GetPurchaseList();
        getData.then(function (emp) {
            $scope.PurchaseList = emp.data;

            angular.forEach($scope.PurchaseList, function (obj) {
                obj["showEdit"] = true;
                obj["showDelete"] = true;
            })

        }, function (emp) {
            alert("Records gathering failed!");
        });
    }


    function getSplitDate(selectedDate) {
        var datearr = selectedDate.split('-');
        var DateValue = datearr[0];
        var MonthValue = datearr[1];
        var YearValue = datearr[2];
        var _date = MonthValue + "-" + DateValue + "-" + YearValue + " 12:00:00 AM";
        return _date;
    };

    $scope.toggleCancelEdit = function (emp) {
        emp.showEdit = true;
        eve.showDelete = true;
    }



    $scope.toggleEdit = function (emp) {
        emp.showEdit = emp.showEdit ? false : true;
        if (emp.showEdit) {
            var PurchaseDetails = {};

            PurchaseDetails = {
                id: emp.ob_tbl_purchase.id,

                invoice_amount: emp.ob_tbl_purchase.invoice_amount,
              
                invoice_no: emp.ob_tbl_purchase.invoice_no,
            };
            alert(PurchaseDetails.invoice_amount);
            $scope.ChechFileValid($scope.SelectedFileForUpload);
            if ($scope.IsFileValid) {
                PurchaseDetails.file = $scope.SelectedFileForUpload
            }
            else {
            }
            var getData = PurchaseServices.UpdatePurchase(PurchaseDetails);
            getData.then(function (msg) {
                GetAllPurchaseList();

                $scope.msg = msg.Message;
                ClearForm();
            }, function (msg) {

                $scope.msg = msg.Message;
            });

        }
        else {
            // emp.id = emp.ob_m_partner_type.id;

        }

    }


    $scope.toggleDeleteEdit = function (sellerPack) {

        var getData = PurchaseServices.DeletePurchaseByID(sellerPack.ob_tbl_purchase.id);
        getData.then(function (eve) {
            GetAllPurchaseList();
        },
        function (msg) {
            $("#alertModal").modal('show');
            alert(msg.data);
        });
    }

    $scope.$watch("f1.$valid", function (isValid) {
        $scope.IsFormValid = isValid;
    });
    try {

        $scope.ChechFileValid = function (file) {
            var isValid = false;
            if ($scope.SelectedFileForUpload != null) {
                if ((file.type == 'image/png' || file.type == 'image/jpeg' || file.type == 'image/gif')) {
                    $scope.FileInvalidMessage = "";
                    isValid = true;
                }
                else {
                    alert("file is not valid");
                    $scope.FileInvalidMessage = "Selected file is Invalid. (only file type png, jpeg and gif)";
                }
            }
            else {
                $scope.FileInvalidMessage = "Image required!";
            }
            $scope.IsFileValid = isValid;
        };
    }
    catch (e) {
        alert("error block called !!");
    }
    //File Select event 
    $scope.selectFileforUpload = function (file) {
        $scope.SelectedFileForUpload = file[0];
    }



    $scope.SavePurchaseDetails = function () {
        $scope.errorflag = true;
        $scope.IsFormSubmitted = true;
        
        var PurchaseDetails = {};
        $scope.ChechFileValid($scope.SelectedFileForUpload);
        $scope.po_date = getSplitDate($scope.po_date);
        $scope.date_invoice = getSplitDate($scope.date_invoice);

        if ($scope.IsFileValid && $scope.IsFormValid) {
            PurchaseDetails = {
                tbl_seller_warehouses_id: $scope.tbl_seller_warehouses_id,
                tbl_seller_vendors_id: $scope.tbl_seller_vendors_id,
                invoice_amount: $scope.invoice_amount,
                invoice_no: $scope.invoice_no,
                file: $scope.SelectedFileForUpload,
                po_date: $scope.po_date,
                date_invoice: $scope.date_invoice,
                remarks_po: $scope.remarks_po,
                tax_amount: $scope.tax_amount,
                po_number: $scope.po_number,
            };

            var getData = PurchaseServices.SavePurchaseDetails(PurchaseDetails);
            getData.then(function (msg) {
                GetAllPurchaseList();
                $scope.msg = msg.Message;
                ClearForm();

            }, function (msg) {
                $scope.msg = msg.Message;
            });

        }
        else {
            $scope.Message = "All the fields are required.";
        }

    }


    function ClearForm() {

        $scope.id = 0;
        $scope.tbl_seller_vendors_id = "";
        $scope.tbl_seller_warehouses_id = "";
        $scope.invoice_amount = "";
        $scope.invoice_no = "";
        $scope.date_invoice = "";
        $scope.po_date = "";
        $scope.po_number = "";
        $scope.tax_amount = "";
        $scope.remarks_po = "";
        angular.forEach(angular.element("input[type='file']"), function (inputElem) {
            angular.element(inputElem).val(null);
        });
        $scope.IsFormSubmitted = false;
        $scope.f1.$setPristine();

        PurchaseServices.GetPurchaseList().then(function (d) {
            //console.log(d);
            $scope.PurchaseList = d.data;
            angular.forEach($scope.PurchaseList, function (obj) {
                obj["showEdit"] = true;
                obj["showDelete"] = true;
            })
        }, function (error) {
            alert('Error!' + error);
        });


    }




}).factory('PurchaseServices', function ($http, $q) {
    var fac = {};
    fac.SavePurchaseDetails = function (PurchaseDetails) {
        //alert("Add2")
        var formData = new FormData();
        formData.append("tbl_seller_vendors_id", PurchaseDetails.tbl_seller_vendors_id);
        formData.append("tbl_seller_warehouses_id", PurchaseDetails.tbl_seller_warehouses_id);
        formData.append("invoice_amount", PurchaseDetails.invoice_amount);
        formData.append("invoice_no", PurchaseDetails.invoice_no);
        formData.append("file", PurchaseDetails.file);
        formData.append("date_invoice", PurchaseDetails.date_invoice);
        formData.append("po_date", PurchaseDetails.po_date);
        formData.append("po_number", PurchaseDetails.po_number);
        formData.append("tax_amount", PurchaseDetails.tax_amount);
        formData.append("remarks_po", PurchaseDetails.remarks_po);


        var defer = $q.defer();
        $http.post("/Seller/Seller/SavePurchaseDetails", formData,
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


    fac.GetVendorNameforPurchase = function () {
        return $http.get('/Seller/Seller/FillVendorForPurchase');
    }
   
    fac.GetWarehouseNameforPurchase = function () {
        return $http.get('/Seller/Seller/FillWareHouseForPurchase');
    }

    fac.GetPurchaseList = function () {
        return $http.get('/Seller/Seller/GetPurchase');
    }


    fac.UpdatePurchase = function (PurchaseDetails) {
        //alert("255")
        var response = $http({
            method: "post",
            url: "/Seller/Seller/UpdatePurchase",
            data: JSON.stringify(PurchaseDetails),
            dataType: "json"
        });
        return response;
    }

    fac.DeletePurchaseByID = function (purchaseId) {
        var response = $http({
            method: "post",
            url: "/Seller/Seller/DeletePurchase",
            params: {
                id: JSON.stringify(purchaseId)
            }
        });

        return response;
    }
    return fac;

});