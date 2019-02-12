angular.module('MyApp').controller('SellerController', function ($scope, SellerServices) {

    $scope.Message = "";

    //$scope.db_pwd = "";
    $scope.address = "";
    $scope.country = "";
    $scope.state = "";

    $scope.city = "";
    $scope.pincode = "";
    $scope.business_name = "";
    $scope.mobile = "";
    $scope.email = "";
    $scope.pan = "";
    $scope.contact_person = "";
    $scope.referred_by = "";
    $scope.gstin = "";
    $scope.tbl_type_id = "";
    $scope.t_seller_typeid = "";
    $scope.wallet_balance = "";
    $scope.applied_plan_rate = "";
    $scope.IsFormSubmitted = false;
    $scope.IsFormValid = false;
    $scope.id = null;
    $scope.PartnerList = null;
    $scope.currentPage = 1;
    $scope.SellerList = null;
    $scope.db_name = "0";
    $scope.numberlength = "";
    $scope.CountryList = [];
    $scope.stateList = [];
    $scope.state = 0;

    GetDbList();
    GetSellerTypeList();
    GetAllSellerList();
    GetTypeSellerList();


    SellerServices.GetSourceJoining().then(function (d) {
        // console.log(d);
        $scope.id = d.data.id;
        $scope.SourceJoiningList = d.data;

    }, function (error) {
        alert('Error!');
    });



    function GetDbList() {
        var getData = SellerServices.GetDataBaseNameseller();
        getData.then(function (eve) {
            $scope.id = eve.data.id;
            $scope.SellerDatabseName = eve.data;
        }, function (eve) {
            alert("Records gathering failed!");
        });
    }

    function GetTypeSellerList() {
        var getData = SellerServices.GetTypeSeller();

        getData.then(function (eve) {
            $scope.id = eve.data.id;
            $scope.TypeSeller = eve.data;
        }, function (eve) {
            alert("Records gathering failed!");
        });
    }

    function GetSellerTypeList() {
        var getData = SellerServices.GetPlanTypeseller();
        getData.then(function (eve) {
            $scope.id = eve.data.id;
            $scope.SellerType = eve.data;
        }, function (eve) {
            alert("Records gathering failed!");
        });
    }

    SellerServices.getCountryList().then(function (d) {
        // console.log(d);
        $scope.id = d.data.id;
        $scope.CountryList = d.data;

    }, function (error) {
        alert('Error!');
    });


    $scope.GetState = function (countrystateid) {

        $scope.state = "";
        var getData = SellerServices.getstateList(countrystateid);
        getData.then(function (eve) {
            $scope.stateList = eve.data;
            console.log(eve.data);
        }, function (eve) {
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

    $scope.pageChangeHandler = function (num) {
        console.log('meals page changed to ' + num);
    };




    function GetAllSellerList() {

        var getData = SellerServices.GetSellerList();
        getData.then(function (emp) {
            $scope.SellerList = emp.data;

            angular.forEach($scope.SellerList, function (obj) {
                obj["showEdit"] = true;
                obj["showDelete"] = true;
            })

        }, function (emp) {
            alert("Records gathering failed!");
        });
    }


    $scope.toggleEdit = function (workSeller) {

        //find the value to upadate the table details in database
        workSeller.showEdit = workSeller.showEdit ? false : true;
        if (workSeller.showEdit) {
            //console.log(workSeller);
            //console.log("pata karna ha");

            var SellersDetails = {};
            SellersDetails = {
                id: workSeller.ob_tbl_sellers.id,
                business_name: workSeller.ob_tbl_sellers.business_name,
                email: workSeller.ob_tbl_sellers.email,
                db_pwd: workSeller.ob_tbl_sellers.db_pwd,
                mobile: workSeller.ob_tbl_sellers.mobile,
                address: workSeller.ob_tbl_sellers.address,
                contact_person: workSeller.ob_tbl_sellers.contact_person,
                wallet_balance: workSeller.ob_tbl_sellers.wallet_balance,
                applied_plan_rate: workSeller.ob_tbl_sellers.applied_plan_rate,
            };

            var getData = SellerServices.UpdateSeller(SellersDetails);
            getData.then(function (msg) {

                $("#alertModal").modal('show');
                $scope.msg = msg.data;
                ClearForm();

            }, function (msg) {
                $("#alertModal").modal('show');
                $scope.msg = msg.data;
            });

            GetAllSellerList();
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

        var getData = SellerServices.DeleteSellerId(sellerPack.ob_tbl_sellers.id);
        getData.then(function (eve) {
            GetAllSellerList();


        },
        function (msg) {
            $("#alertModal").modal('show');
            alert(msg.data);
        });
    }




    $scope.$watch("f1.$valid", function (isValid) {
        $scope.IsFormValid = isValid;
    });

    $scope.SaveSellerDetails = function () {
        $scope.IsFormSubmitted = true;
        var value = parseInt(('' + $scope.mobile).length);
        if (value > 13) {
            $scope.numberlength = "number should be 13 character !!";
        }

        $scope.Message = "";
        if ($scope.IsFormValid) {

            //alert($scope.date_of_signup);
            //alert($scope.tbl_type_id);

            SellerServices.SaveSeller($scope.db_name, $scope.t_seller_typeid, $scope.tbl_type_id, $scope.address, $scope.country, $scope.state,
                $scope.city, $scope.pincode, $scope.business_name, $scope.mobile, $scope.email, $scope.pan, $scope.contact_person,
                $scope.referred_by, $scope.gstin, $scope.wallet_balance, $scope.applied_plan_rate, $scope.m_source_of_joining_id).then(function (d) {
                    alert(d.Message);
                    //  $scope.Message = d.Message;
                    ClearForm();
                    GetDbList();
                }, function (e) {
                    alert(e);
                });
        }
        else {
            $scope.Message = "All the fields are required.";
        }
    };

    function ClearForm() {
        $scope.db_name = "";
        $scope.id = 0;
        $scope.db_pwd = "";
        $scope.address = "";
        $scope.country = "";

        $scope.state = "";
        $scope.city = "";
        $scope.pincode = "";
        $scope.business_name = "";
        $scope.mobile = "";
        $scope.email = "";
        $scope.pan = "";
        $scope.contact_person = "";
        $scope.referred_by = "";
        $scope.date_of_signup = "";
        $scope.gstin = "";
        $scope.wallet_balance = "";
        $scope.applied_plan_rate = "";
        $scope.m_source_of_joining_id = "";
        $scope.tbl_type_id = "";
        $scope.t_seller_typeid = "";

        $scope.IsFormSubmitted = false;
        $scope.f1.$setPristine();

        //call service again when i click the save button and records is saved successfully !!!
        SellerServices.GetSellerList().then(function (d) {
            //console.log(d);
            $scope.SellerList = d.data;
            angular.forEach($scope.SellerList, function (obj) {
                obj["showEdit"] = true;
                obj["showDelete"] = true;
            })
        }, function (error) {
            alert('Error!' + error);
        });


    }




}).factory('SellerServices', function ($http, $q) {
    var fac = {};
    fac.SaveSeller = function (db_name, t_seller_typeid, tbl_type_id, address, country, state, city, pincode, business_name, mobile, email, pan,
        contact_person, referred_by, gstin, wallet_balance, applied_plan_rate, m_source_of_joining_id) {
        var formData = new FormData();

        formData.append("db_name", db_name);
        //formData.append("db_pwd", db_pwd);
        formData.append("t_seller_typeid", t_seller_typeid);
        formData.append("tbl_type_id", tbl_type_id);
        formData.append("address", address);
        formData.append("country", country);

        formData.append("state", state);
        formData.append("city", city);
        formData.append("pincode", pincode);
        formData.append("business_name", business_name);
        formData.append("mobile", mobile);
        formData.append("email", email);
        formData.append("pan", pan);
        formData.append("contact_person", contact_person);
        formData.append("referred_by", referred_by);
        //formData.append("date_of_signup", date_of_signup);
        formData.append("gstin", gstin);
        formData.append("wallet_balance", wallet_balance);
        formData.append("applied_plan_rate", applied_plan_rate);
        formData.append("m_source_of_joining_id", m_source_of_joining_id);

        var defer = $q.defer();
        $http.post("/Home/SaveSellerDetails", formData,
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





    fac.GetSourceJoining = function () {
        return $http.get('/Home/FillSourceJoiningForSeller');
    }

    fac.GetDataBaseNameseller = function () {
        return $http.get('/Home/FillDatabseNameforSeller');
    }

    fac.GetPlanTypeseller = function () {
        return $http.get('/Home/FillPlanTypeForSeller');
    }

    fac.GetTypeSeller = function () {
        return $http.get('/Home/FillSellerTypeForSeller');
    }

    fac.getCountryList = function () {
        return $http.get("/Home/GetCountry");
    };

    fac.getstateList = function (countrystateid) {
        return $http.get("/Home/GetStateDetails?countrystateid=" + countrystateid);
    };


    fac.GetSellerList = function () {
        return $http.get('/Home/GetSeller');
    }


    fac.UpdateSeller = function (SellersDetails) {
        var response = $http({
            method: "post",
            url: "/Home/UpdateSellerDetails",
            data: JSON.stringify(SellersDetails),
            dataType: "json"
        });
        return response;
    }

    fac.DeleteSellerId = function (sellerId) {
        var response = $http({
            method: "post",
            url: "/Home/DeleteSeller",
            params: {
                id: JSON.stringify(sellerId)
            }
        });

        return response;
    }





    return fac;

});