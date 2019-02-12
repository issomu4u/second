angular.module('MyAppSeller').controller('CourierNameController', function ($scope, CourierCompanyServices) {

    $scope.Message = "";
    $scope.courier_company_name = "";   
    $scope.IsFormSubmitted = false;
    $scope.IsFormValid = false;
    $scope.id = null;
    $scope.currentPage = 1;
    $scope.numberlength = "";

    GetAllCourierCompanyList();

    $scope.pageChangeHandler = function (num) {
        console.log('meals page changed to ' + num);
    };



    function GetAllCourierCompanyList() {
        var getData = CourierCompanyServices.GetCourierNameList();
        getData.then(function (emp) {
            $scope.CourierCompanyList = emp.data;
            angular.forEach($scope.CourierCompanyList, function (obj) {
                obj["showEdit"] = true;
                obj["showDelete"] = true;
            })
        }, function (emp) {
            alert("Records gathering failed!");
        });
    }


    $scope.toggleEdit = function (workSeller) {
        workSeller.showEdit = workSeller.showEdit ? false : true;
        if (workSeller.showEdit) {
            // alert("chk" + workSeller.id)
            var Details = {};
            Details = {
                id: workSeller.ob_tbl_courier_comapny.id,
                courier_company_name: workSeller.ob_tbl_courier_comapny.courier_company_name,
            };
            var getData = CourierCompanyServices.UpdateCourierName(Details);
            getData.then(function (msg) {

                $("#alertModal").modal('show');
                $scope.msg = msg.data;
                ClearForm();
            }, function (msg) {
                $("#alertModal").modal('show');
                $scope.msg = msg.data;
            });

            GetAllCourierCompanyList();
        }
        else {
            // emp.id = emp.ob_m_partner_type.id;

        }

    }


    $scope.toggleCancelEdit = function (emp) {
        emp.showEdit = true;
        eve.showDelete = true;
    }

    $scope.toggleDeleteEdit = function (sellerPack) {

        var getData = CourierCompanyServices.DeleteByID(sellerPack.ob_tbl_courier_comapny.id);
        getData.then(function (eve) {
            GetAllCourierCompanyList();
        },
        function (msg) {
            $("#alertModal").modal('show');
            alert(msg.data);
        });
    }




    $scope.$watch("f1.$valid", function (isValid) {
        $scope.IsFormValid = isValid;
    });

    $scope.SaveCourier = function () {
        $scope.IsFormSubmitted = true;

        $scope.Message = "";
        if ($scope.IsFormValid) {
            CourierCompanyServices.SaveDetails($scope.courier_company_name).then(function (d) {
                    alert(d.Message);
                    ClearForm();
                }, function (e) {
                    alert(e);
                });
        }
        else {
            $scope.Message = "All the fields are required.";
        }
    };

    function ClearForm() {

        $scope.id = 0;
        $scope.courier_company_name = "";       
        $scope.IsFormSubmitted = false;
        $scope.f1.$setPristine();

        CourierCompanyServices.GetCourierNameList().then(function (d) {

            $scope.CourierCompanyList = d.data;
            angular.forEach($scope.CourierCompanyList, function (obj) {
                obj["showEdit"] = true;
                obj["showDelete"] = true;
            })
        }, function (error) {
            alert('Error!' + error);
        });


    }




}).factory('CourierCompanyServices', function ($http, $q) {
    var fac = {};
    fac.SaveDetails = function (courier_company_name) {

        var formData = new FormData();
        formData.append("courier_company_name", courier_company_name);       
        var defer = $q.defer();
        $http.post("/Seller/Seller/SaveCourierDetails", formData,
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


    fac.GetCourierNameList = function () {
        return $http.get('/Seller/Seller/GetCourierDetails');
    }


    fac.UpdateCourierName = function (Details) {

        var response = $http({
            method: "post",
            url: "/Seller/Seller/UpdateCourierDetails",
            data: JSON.stringify(Details),
            dataType: "json"
        });
        return response;
    }

    fac.DeleteByID = function (Id) {
        var response = $http({
            method: "post",
            url: "/Seller/Seller/DeleteCourier",
            params: {
                id: JSON.stringify(Id)
            }
        });

        return response;
    }
    return fac;

});