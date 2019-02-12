
angular.module('MyAppSeller').controller('banktransfer', function ($scope, banktransferServices) {

    $scope.Message = "";
    //$scope.courier_company_name = "";
    $scope.IsFormSubmitted = false;
    $scope.IsFormValid = false;
    $scope.id = null;
    $scope.currentPage = 1;
    $scope.numberlength = "";
    $scope.MarketPlaceList = [];
    SellerMarketplaceList();
    GetBankList();

    $scope.pageChangeHandler = function (num) {
        console.log('meals page changed to ' + num);
    };

    function SellerMarketplaceList() {
        var getData = banktransferServices.GetAllMarketplacelist();
        getData.then(function (eve) {
            $scope.id = eve.data.id;
            $scope.MarketplaceList = eve.data;
        }, function (eve) {
            alert("Records gathering failed!");
        });
    }

    function GetBankList() {
        var getData = banktransferServices.GetBankList();
        getData.then(function (emp) {
            $scope.BankList = emp.data;            
            angular.forEach($scope.BankList, function (obj) {
                obj["showEdit"] = true;
                obj["showDelete"] = true;
            })
        }, function (emp) {
            //alert("Records gathering failed!");
        });
    }
     
    $scope.cleardata = function () {
        $('#txt_to').val('');
        $('#txt_from').val('');
        $('#ddl_marketplaceAPI').val(0);
        $scope.ddl_marketplaceAPI = '';
        GetBankList();
    }
    $scope.SearchData = function () {
        //alert("dd");
        //alert("ss"+$scope.ddl_marketplaceAPI)
        $scope.Productfrom = $('#txt_from').val();
        $scope.Productto = $('#txt_to').val();
        
        var getData = banktransferServices.GetsearchList($scope.ddl_marketplaceAPI, $scope.Productfrom, $scope.Productto);
        getData.then(function (emp) {
            $scope.BankList = emp.data;
            angular.forEach($scope.BankList, function (obj) {
                obj["showEdit"] = true;
                obj["showDelete"] = true;
            })
        }, function (emp) {
            //alert("Records gathering failed!");
        });
    }


    $scope.toggleEdit = function (workSeller) {
        workSeller.showEdit = workSeller.showEdit ? false : true;
        if (workSeller.showEdit) {
            // alert("chk" + workSeller.id)
            var Details = {};
            Details = {
                id: workSeller.id,
                remarks: workSeller.remarks,
                verifystatus: workSeller.verifystatus,
            };
            var getData = banktransferServices.UpdateBankList(Details);
            getData.then(function (msg) {

                $("#alertModal").modal('show');
                $scope.msg = msg.data;
                GetBankList();

            }, function (msg) {
                $("#alertModal").modal('show');
                $scope.msg = msg.data;
            });


        }
        else {
            // emp.id = emp.ob_m_partner_type.id;

        }

    }



    $scope.toggleCancelEdit = function (emp) {
        emp.showEdit = true;
        eve.showDelete = true;
    }




    $scope.$watch("f1.$valid", function (isValid) {
        $scope.IsFormValid = isValid;
    });


    


}).factory('banktransferServices', function ($http, $q) {
    var fac = {};


    fac.GetBankList = function () {
        return $http.get('/Seller/SaleReport/GetBankTransfer');
    }
    fac.UpdateBankList = function (Details) {

        var response = $http({
            method: "post",
            url: "/Seller/SaleReport/UpdateBankDetails",
            data: JSON.stringify(Details),
            dataType: "json"
        });
        return response;
    }

    fac.GetsearchList = function (ddl_marketplaceAPI, Productfrom, Productto) {
       
        return $http.get("/Seller/SaleReport/GetSearchTransfer?ddl_marketplaceProductAPI=" + ddl_marketplaceAPI + '&Productfrom=' + Productfrom + '&Productto=' + Productto);

        //alert(ddl_marketplaceAPI);
        //alert(Productfrom);
        //alert(Productto);
        //var data = new FormData();
        //data.append("ddl_marketplaceProductAPI", ddl_marketplaceAPI);
        //data.append("Productfrom", Productfrom);
        //data.append("Productto", Productto);
        ////var response = $http({
        ////    method: "post",
        ////    url: "/Seller/SaleReport/GetSearchTransfer",
        ////    data: JSON.stringify(data),
        ////    dataType: "json"
        ////});
        //var objXhr = new XMLHttpRequest();
        
        //objXhr.open("GET", "/Seller/SaleReport/GetSearchTransfer/");
        //objXhr.send(data);
        ////return response;
    }
    fac.GetAllMarketplacelist = function () {
        return $http.get('/Seller/UploadDataAPI/FillMarketplace');
    }

    return fac;

});