angular.module('MyAppSeller').controller('WarehouseController', function ($scope, WarehouseServices) {

    $scope.Message = "";
    $scope.warehouse_name = "";
    $scope.mobile = "";
   
    $scope.email = "";
    $scope.address = "";
    $scope.contact_person = "";
    $scope.country = "";
    $scope.state = "";
    $scope.city = "";

    $scope.n_default_warehouse = "0";
   

    $scope.IsFormSubmitted = false;
    $scope.IsFormValid = false;
    $scope.id = null;
    $scope.currentPage = 1;
    $scope.SellerList = null;

    $scope.numberlength = "";
    $scope.CountryList = [];
    $scope.stateList = [];
    $scope.state = 0;


    GetAllWarehouseList();

     
      
      
    $scope.CheckPassport = function () {
       
        if ($scope.n_default_warehouse) {
            //alert($scope.n_default_warehouse);
                    $window.alert("CheckBox is checked.");
                } else {
                    $window.alert("CheckBox is not checked.");
                }
            };
      
   




    WarehouseServices.getCountryList().then(function (d) {
        // console.log(d);
        $scope.id = d.data.id;
        $scope.CountryList = d.data;

    }, function (error) {
        alert('Error!');
    });


    $scope.GetState = function (countrystateid) {
        $scope.state = "";
        var getData = WarehouseServices.getstateList(countrystateid);
        getData.then(function (eve) {
            $scope.stateList = eve.data;
            console.log(eve.data);
        }, function (eve) {
            //alert("Records gathering failed!");
        });
    }

    $scope.pageChangeHandler = function (num) {
        console.log('meals page changed to ' + num);
    };



    function GetAllWarehouseList() {

        var getData = WarehouseServices.GetWarehouseList();
        getData.then(function (emp) {
            $scope.WarehouseList = emp.data;
            angular.forEach($scope.WarehouseList, function (obj) {
                obj["showEdit"] = true;
                obj["showDelete"] = true;
            })
        }, function (emp) {
            //alert("Records gathering failed!");
        });
    }


    $scope.toggleEdit = function (workSeller) {

        //find the value to upadate the table details in database
        workSeller.showEdit = workSeller.showEdit ? false : true;
        if (workSeller.showEdit) {
            //console.log("pata karna ha");
            //console.log(workSeller);
            
            //alert("1")
           // alert("Address" + workSeller.ob_tbl_seller_warehouses.address)
            var WarehouseDetails = {};
            WarehouseDetails = {
                id: workSeller.ob_tbl_seller_warehouses.id,
                address: workSeller.ob_tbl_seller_warehouses.address,
                warehouse_name: workSeller.ob_tbl_seller_warehouses.warehouse_name,
                email: workSeller.ob_tbl_seller_warehouses.email,
                mobile: workSeller.ob_tbl_seller_warehouses.mobile,
                contact_person: workSeller.ob_tbl_seller_warehouses.contact_person,
            };

            var getData = WarehouseServices.UpdateWarehouse(WarehouseDetails);
            getData.then(function (msg) {

                $("#alertModal").modal('show');
                $scope.msg = msg.data;
                ClearForm();

            }, function (msg) {
                $("#alertModal").modal('show');
                $scope.msg = msg.data;
            });

            GetAllWarehouseList();
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
        var getData = WarehouseServices.DeleteWarehouseByID(sellerPack.ob_tbl_seller_warehouses.id);
        getData.then(function (eve) {
            GetAllWarehouseList();
        },
        function (msg) {
            $("#alertModal").modal('show');
            alert(msg.data);
        });
    }




    $scope.$watch("f1.$valid", function (isValid) {
        $scope.IsFormValid = isValid;
    });

    $scope.SaveWarehouseDetails = function () {
        $scope.IsFormSubmitted = true;
        var value = parseInt(('' + $scope.mobile).length);
        if (value > 13) {
            $scope.numberlength = "number should be 13 character !!";
        }

        $scope.Message = "";
        if ($scope.IsFormValid) {
            if ($scope.default_warehouse)
                $scope.n_default_warehouse = 1;
            else
                $scope.n_default_warehouse = 0;

            //alert($scope.n_default_warehouse);
            WarehouseServices.SaveWarehouseDetails($scope.warehouse_name, $scope.mobile, $scope.email,
                $scope.address, $scope.contact_person, $scope.country, $scope.state, $scope.city, $scope.n_default_warehouse).then(function (d) {
                    alert(d.Message);
                    //  $scope.Message = d.Message;
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
        $scope.warehouse_name = "";
        $scope.mobile = "";      
        $scope.email = "";
        $scope.address = "";
        $scope.contact_person = "";
        $scope.country = "";
        $scope.state = "";
        $scope.city = "";
        $scope.n_default_warehouse = "";
        $scope.IsFormSubmitted = false;
        $scope.f1.$setPristine();

        WarehouseServices.GetWarehouseList().then(function (d) {
            //console.log(d);
            $scope.WarehouseList = d.data;
            angular.forEach($scope.WarehouseList, function (obj) {
                obj["showEdit"] = true;
                obj["showDelete"] = true;
            })
        }, function (error) {
            alert('Error!' + error);
        });
    }

    $scope.keyupFunction = function ValidateEmail(mail) {
        if (/^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/.test(mail)) {           
            return (true)
        }
        jAlert("Email is not valid");
        $scope.email = "";
        //alert("You have entered an invalid email address!")
        return (false)
    }



}).factory('WarehouseServices', function ($http, $q) {
    var fac = {};
    fac.SaveWarehouseDetails = function (warehouse_name, mobile, email, address, contact_person, country,
        state, city,n_default_warehouse) {

        var formData = new FormData();
        formData.append("warehouse_name", warehouse_name);
        formData.append("mobile", mobile);   
        formData.append("email", email);
        formData.append("address", address);
        formData.append("contact_person", contact_person);
        formData.append("country", country);
        formData.append("state", state);
        formData.append("city", city);
        formData.append("n_default_warehouse", n_default_warehouse);
       

        var defer = $q.defer();
        $http.post("/Seller/Seller/SaveWarehouseDetails", formData,
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



    fac.getCountryList = function () {
        return $http.get("/Home/GetCountry");
    };

    fac.getstateList = function (countrystateid) {
        return $http.get("/Home/GetStateDetails?countrystateid=" + countrystateid);
    };


    fac.GetWarehouseList = function () {
        return $http.get('/Seller/Seller/GetWarehouse');
    }


    fac.UpdateWarehouse = function (WarehouseDetails) {
        //alert("2")
        //alert("Add" + WarehouseDetails.address)
        var response = $http({
            method: "post",
            url: "/Seller/Seller/UpdateWarehouseDetails",
            data: JSON.stringify(WarehouseDetails),
            dataType: "json"
        });
        return response;
    }

    fac.DeleteWarehouseByID = function (warehouseId) {
        var response = $http({
            method: "post",
            url: "/Seller/Seller/DeleteWarehouse",
            params: {
                id: JSON.stringify(warehouseId)
            }
        });

        return response;
    }
    return fac;

});