angular.module('MyAppSeller').controller('UploadDataAPIController', function ($scope, UploadDataAPIServices) {

    //$scope.Message = "ggggggggggggggg";
    $scope.my_unique_id = "";
    $scope.m_marketplace_id = "";
    $scope.t_loginName = "";
    $scope.t_password = "";
    $scope.t_access_Key_id = "";
    $scope.t_auth_token = "";
    $scope.t_secret_Key = "";
    $scope.market_palce_id = "";

    $scope.IsFormSubmitted = false;
    $scope.IsFormValid = false;
    $scope.id = null;
    $scope.currentPage = 1;
    $scope.currentPage2 = 1;
    $scope.currentPage1 = 1;
    $scope.currentPage3 = 1;
    $scope.currentPage4 = 1;
    $scope.currentPage5 = 1;
    $scope.numberlength = "";
    $scope.MarketPlaceList = [];
    $scope.SelectedFileForUpload = null;
    $scope.filter = 0;
    $scope.Id = "";
    SellerMarketplaceList();
    $scope.MarketplaceList = null;
    // GetAllOrderUploadList();
    //GetAllSettlementUploadList();
    //GetAllTaxUploadList();
    //GetAllReturnUploadList();


    function SellerMarketplaceList() {
        var getData = UploadDataAPIServices.GetAllMarketplacelist();
        getData.then(function (eve) {
            $scope.id = eve.data.id;
            $scope.MarketplaceList = eve.data;
        }, function (eve) {
            //alert("Records gathering failed!");
        });
    }

    $scope.dataget = 1;
    $scope.Getsellerapi = function (marketplaceid) {
        $scope.tbl_marketplace_id = marketplaceid;
        var getData = UploadDataAPIServices.CheckMarketplace(marketplaceid);
        getData.then(function (eve) {           
            $scope.dataget = eve.data;
            //alert($scope.dataget);
        }, function (eve) {
        });
    }

    $scope.getFileDetails = function (e) {

        $scope.files = [];
        $scope.$apply(function () {
            for (var i = 0; i < e.files.length; i++) {
                $scope.files.push(e.files[i])
            }
        });
    };
    $scope.ddl_marketplacebulk = '';
    $scope.uploadFiles = function (IsValid) {
        if (IsValid) {           
            var data = new FormData();
            var ddlmarket = $scope.ddl_marketplacebulk;
            var ddlfund = ($('#ddlFundType').val());
            for (var i in $scope.files) {
                data.append("uploadedFile" + i, $scope.files[i]);
            }
            data.append("ddlFundType", ddlfund);
            data.append("ddl_marketplacebulk", ddlmarket);
            var objXhr = new XMLHttpRequest();
            objXhr.addEventListener("progress", updateProgress, false);
            objXhr.addEventListener("load", transferComplete, false);
            objXhr.open("POST", "/UploadDataAPI/UploadBulkSettlement/");
            objXhr.send(data);
        }
    }


    $scope.uploadorderFiles = function (IsValid) {
        if (IsValid) {
            var data = new FormData();
            var ddlorder = $scope.ddl_marketplaceOrder; //($('#ddl_marketplaceOrder').val());
            for (var i in $scope.files) {
                data.append("uploadedFile" + i, $scope.files[i]);
            }
            data.append("ddl_marketplaceOrder", ddlorder);
            var objXhr = new XMLHttpRequest();
            objXhr.addEventListener("progress", updateProgress, false);
            objXhr.addEventListener("load", transferComplete, false);
            objXhr.open("POST", "/UploadDataAPI/UploadBulkOrderExcel/");
            objXhr.send(data);
            //ClearForm();
            // GetAllOrderUploadList();
        }
    }

    $scope.uploadreturnFiles = function (IsValid) {
        if (IsValid) {
            var data = new FormData();
            var ddlorder = $scope.ddl_marketplaceReturn;//($('#ddl_marketplaceReturn').val());
            for (var i in $scope.files) {
                data.append("uploadedFile" + i, $scope.files[i]);
            }
            data.append("ddl_marketplaceReturn", ddlorder);
            var objXhr = new XMLHttpRequest();
            objXhr.addEventListener("progress", updateProgress, false);
            objXhr.addEventListener("load", transferComplete, false);
            objXhr.open("POST", "/UploadDataAPI/UploadReturnFile/");
            objXhr.send(data);           
        }
    }

    $scope.uploadTaxFiles = function (IsValid) {
        if (IsValid) {
            var data = new FormData();
            var ddltax = $scope.ddl_marketplaceTax;//($('#ddl_marketplaceTax').val());
            var abc = $scope.filter;           
            for (var i in $scope.files) {
                data.append("uploadedFile" + i, $scope.files[i]);
            }
            data.append("ddl_marketplaceTax", ddltax);
            data.append("is_checked", abc);
            var objXhr = new XMLHttpRequest();
            objXhr.addEventListener("progress", updateProgress, false);
            objXhr.addEventListener("load", transferComplete, false);
            objXhr.open("POST", "/UploadDataAPI/UploadBulkTaxExcel/");
            objXhr.send(data);           
        }
    }
    $scope.uploadproductreport = function (IsValid) {      
        if (IsValid) {
            var data = new FormData();         
            $scope.Productfrom = $('#txt_Productfrom').val();
            $scope.Productto = $('#txt_Productto').val();
            var ddlaaa = $scope.ddl_marketplaceProductAPI;
            var Productfrom = $scope.Productfrom;
            var Productto = $scope.Productto;

            data.append("ddl_marketplaceProductAPI", ddlaaa);
            data.append("txt_Productfrom", Productfrom);
            data.append("txt_Productto", Productto);
            var objXhr = new XMLHttpRequest();
            objXhr.addEventListener("progress", updateProgress, false);
            objXhr.addEventListener("load", transferComplete, false);
            objXhr.open("POST", "/UploadDataAPI/ProductAPI/");
            objXhr.send(data);

        }
    }

    $scope.uploadorderapi = function (IsValid) {     
        if (IsValid) {
            var data = new FormData();
           
            $scope.from = $('#txt_from').val();
            $scope.To = $('#txt_to').val();
            var ddlmarket = $scope.ddl_marketplaceAPI;
            var from = $scope.from;
            var To = $scope.To;

            data.append("ddl_marketplaceAPI", ddlmarket);
            data.append("txt_from", from);
            data.append("txt_to", To);
            var objXhr = new XMLHttpRequest();
            objXhr.addEventListener("progress", updateProgress, false);
            objXhr.addEventListener("load", transferComplete, false);
            objXhr.open("POST", "/UploadDataAPI/OrderAPI/");
            objXhr.send(data);

        }
    }

    $scope.uploadsettlementapi = function (IsValid) {
        if (IsValid) {
            var data = new FormData();

            $scope.setfrom = $('#txt_Settlementfrom').val();
            $scope.setTo = $('#txt_settlementto').val();
            var ddlmarket = $scope.ddl_marketplaceSettAPI;
            var from = $scope.setfrom;
            var To = $scope.setTo;

            data.append("ddl_marketplaceSettAPI", ddlmarket);
            data.append("txt_Settlementfrom", from);
            data.append("txt_settlementto", To);
            var objXhr = new XMLHttpRequest();
            objXhr.addEventListener("progress", updateProgress, false);
            objXhr.addEventListener("load", transferComplete, false);
            objXhr.open("POST", "/UploadDataAPI/SettlementAPI/");
            objXhr.send(data);

        }
    }


    function updateProgress(e) {
        if (e.lengthComputable) {
        }
    }
    function transferComplete(e) {
        var RetData = JSON.parse(this.responseText);
        if (RetData.status == 1) {
            jAlert(RetData.message, 'Message');
        }
        else
            jAlert(RetData.message, 'Alert');
        ClearForm();
        GetAllSettlementUploadList();
        GetAllOrderUploadList();
        GetAllTaxUploadList();
        GetAllReturnUploadList();
        GetAllProductUploadList();
        $scope.FormBulkSettlement.$setPristine();
        $scope.FormBulkOrder.$setPristine();
        $scope.FormBulkTax.$setPristine();
        $scope.FormBulkReturnOrder.$setPristine();
        $scope.FormProductReport.$setPristine();
        $scope.FormOrderAPI.$setPristine();
        $scope.FormSettlementAPI.$setPristine();
        //alert("Files uploaded successfully.");
    }






    //setInterval(function () {        
    //    GetAllOrderUploadList();
    //    GetAllSettlementUploadList();
    //    GetAllTaxUploadList();
    //}, 6000);

    function ClearForm() {
        $('#ddl_marketplacebulk').val(0);
        $('#ddl_marketplaceOrder').val(0);
        $('#ddl_marketplaceAPI').val(0);
        $('#ddl_marketplaceSettAPI').val(0);
        $('#ddlFundType').val(0);
        $('#ddl_marketplaceTax').val(0);
        $('#ddl_marketplaceReturn').val(0);
        $('#ddl_marketplaceProductAPI').val(0);
        $('#txt_Productfrom').val('');
        $('#txt_Productto').val('');
        $('#txt_from').val('');
        $('#txt_to').val('');
        $('#txt_Settlementfrom').val('');
        $('#txt_settlementto').val('');
        $scope.filter = 0;
        $scope.files = [];
        $scope.ddl_marketplacebulk = '';
        $scope.ddl_marketplaceOrder = '';
        $scope.ddl_marketplaceTax = '';
        $scope.ddl_marketplaceReturn = '';
        $scope.ddl_marketplaceProductAPI = '';
        $scope.ddl_marketplaceAPI = '';
        $scope.ddl_marketplaceSettAPI = '';
        $('#file').val('');
        $('#file1').val('');
        $('#file2').val('');
        $('#file4').val('');
    }
    $scope.pageChangeHandler = function (num) {
        //alert('meals page changed to ' + num);
    };

    $scope.SettlementData = function () {
        GetAllSettlementUploadList();
    };

    $scope.SalesOrderData = function () {
        GetAllOrderUploadList();
    }
    $scope.TaxData = function () {
        GetAllTaxUploadList();
    }

    $scope.ReturnOrderData = function () {
        GetAllReturnUploadList();
    }

    $scope.ProductData = function () {
        GetAllProductUploadList();
    }
    function GetAllOrderUploadList() {
        var getData = UploadDataAPIServices.GetuploadorderList();
        getData.then(function (emp) {
            $scope.OrderUploadList = emp.data;          
            angular.forEach($scope.OrderUploadList, function (obj) {
                obj["showEdit"] = true;
                obj["showDelete"] = true;
            })
        }, function (emp) {
            //alert("Records gathering failed!");
        });
    }
    GetAllOrderUploadList();

    function GetAllProductUploadList() {
        //alert("after submit");
        var getData = UploadDataAPIServices.GetProductList();
        getData.then(function (emp) {
            $scope.ProductUploadList = emp.data;
            console.log("data");
            console.log($scope.ProductUploadList);
            angular.forEach($scope.ProductUploadList, function (obj) {
                obj["showEdit"] = true;
                obj["showDelete"] = true;
            })
        }, function (emp) {
            //alert("Records gathering failed!");
        });
    }
    GetAllProductUploadList();

    function GetAllTaxUploadList() {
        var getData = UploadDataAPIServices.GetuploadTaxList();
        getData.then(function (emp) {
            $scope.TaxUploadList = emp.data;
            angular.forEach($scope.TaxUploadList, function (obj) {
                obj["showEdit"] = true;
                obj["showDelete"] = true;
            })
        }, function (emp) {
            // alert("Records gathering failed!");
        });
    }
    GetAllTaxUploadList();
    function GetAllSettlementUploadList() {
        //alert("calll");
        var getData = UploadDataAPIServices.GetsettlementorderList();
        getData.then(function (emp) {
            $scope.SettlementUploadList = emp.data;
            angular.forEach($scope.SettlementUploadList, function (obj) {
                obj["showEdit"] = true;
                obj["showDelete"] = true;
            })
        }, function (emp) {
            //alert("Records gathering failed!");
        });
    }
    GetAllSettlementUploadList();

    function GetAllReturnUploadList() {
        //alert("calll");
        var getData = UploadDataAPIServices.GetreturnorderList();
        getData.then(function (emp) {
            $scope.ReturnUploadList = emp.data;
            angular.forEach($scope.ReturnUploadList, function (obj) {
                obj["showEdit"] = true;
                obj["showDelete"] = true;
            })
        }, function (emp) {
            //alert("Records gathering failed!");
        });
    }
    GetAllReturnUploadList();

    $scope.toggleCancelEdit = function (emp) {
        emp.showEdit = true;
        eve.showDelete = true;
    }

    $scope.$watch("f1.$valid", function (isValid) {
        $scope.IsFormValid = isValid;
    });

    $scope.TaxUpdateCall = function () {
        $scope.loading = true;
        var getData = UploadDataAPIServices.Getupdatetax();
        getData.then(function (eve) {
            $scope.loading = false;
        });
    }



    $scope.ViewSuspenseDetails = function (id) {       
        var getData = UploadDataAPIServices.getsuspenseList(id);
        getData.then(function (emp) {
            $scope.SuspenseDetailList = emp.data;
            angular.forEach($scope.SuspenseDetailList, function (obj) {
                obj["showEdit"] = true;
                obj["showDelete"] = true;
            })
        }, function (emp) {
            //alert("Records gathering failed!");
        });
    }




}).factory('UploadDataAPIServices', function ($http, $q) {
    var fac = {};
    fac.GetuploadorderList = function () {
        return $http.get('/Seller/UploadDataAPI/GetUploadOrderDetails');
    }
    fac.Getupdatetax = function () {
        return $http.get('/Seller/UploadDataAPI/UpdateTax');
    }

    fac.GetuploadTaxList = function () {
        return $http.get('/Seller/UploadDataAPI/GetUploadTaxDetails');
    }
    fac.GetsettlementorderList = function () {
        return $http.get('/Seller/UploadDataAPI/GetSettlementOrderDetails');
    }
    fac.GetreturnorderList = function () {
        return $http.get('/Seller/UploadDataAPI/GetReturnUploadDetails');
    }
    fac.GetProductList = function () {
        return $http.get('/Seller/UploadDataAPI/GetProductDetails');
    }
    fac.getsuspenseList = function (id) {
        return $http.get("/Seller/UploadDataAPI/GetSuspenseDetails?id=" + id);
    };

    fac.GetAllMarketplacelist = function () {
        return $http.get('/Seller/UploadDataAPI/FillMarketplace');
    }

    fac.CheckMarketplace = function (marketplaceid) {
        return $http.get("/Seller/UploadDataAPI/GetApiDetails?marketplaceid=" + marketplaceid);
    };

    return fac;

});