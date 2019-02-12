angular.module('MyAppSeller').controller('ReturnGoodsController', function ($scope, ReturnGoodsServices) {

    //$scope.Message = "";
    //$scope.courier_company_name = "";
    $scope.IsFormSubmitted = false;
    $scope.IsFormValid = false;
    //$scope.id = null;
    $scope.currentPage = 1;
    $scope.numberlength = "";

    GetAllReturnGoodsList();

    $scope.pageChangeHandler = function (num) {
        console.log('meals page changed to ' + num);
    };



    function GetAllReturnGoodsList() {
        var getData = ReturnGoodsServices.GetReturnNameList();
        getData.then(function (emp) {
            $scope.ReturnGoodsList = emp.data;
            angular.forEach($scope.ReturnGoodsList, function (obj) {
                obj["showEdit"] = true;
                obj["showDelete"] = true;
            })
        }, function (emp) {
            alert("Records gathering failed!");
        });
    }

    //$scope.chkphysicallyClick = function () {
    //    if ($("#chkYes").is(":checked")) {
    //        alert("checked");           
    //        $scope.physicallygoods = 1;
    //        alert($scope.physicallygoods);
    //    } else {
    //        alert("unchecked");           
    //        $scope.physicallygoods = 2;
    //        alert($scope.physicallygoods);
    //    }
    //}
    $scope.chkClick = function () {
        if ($("#chkYes").is(":checked")) {
            //alert("checked");
           
            $scope.n_paymentMode = 1;
        } else {
            //alert("unchecked");
           
            $scope.n_paymentMode = 2;
        }
    }
    //function ClearPOFields() {
       
    //    $scope.physicallygoods = "";
        
    //}

    $scope.toggleCancelEdit = function (emp) {
        emp.showEdit = true;
        eve.showDelete = true;
    }

   
    $scope.$watch("f1.$valid", function (isValid) {
        $scope.IsFormValid = isValid;
    });


}).factory('ReturnGoodsServices', function ($http, $q) {
    var fac = {};
   
    fac.GetReturnNameList = function () {
        return $http.get('/Seller/Sales/GetReturnGoodsDetails');
    }

    return fac;

});