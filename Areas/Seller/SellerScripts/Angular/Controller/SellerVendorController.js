angular.module('MyAppSeller').controller('SellerVendorController', function ($scope, SellerVendorServices) {

    $scope.Message = "";
    $scope.vendor_name = "";
    $scope.mobile = "";
    $scope.telephone = "";
    $scope.email = "";
    $scope.address = "";
    $scope.contact_person = "";
    $scope.country = "";
    $scope.state = "";
    $scope.city = "";
    $scope.pincode = "";
    $scope.gstin = "";
    $scope.pan = "";

    $scope.IsFormSubmitted = false;
    $scope.IsFormValid = false;
    $scope.id = null;
    $scope.currentPage = 1;
    $scope.SellerList = null;

    $scope.numberlength = "";
    $scope.CountryList = [];
    $scope.stateList = [];
    $scope.state = 0;


    GetAllSellerVendorList();


    SellerVendorServices.getCountryList().then(function (d) {
        // console.log(d);
        $scope.id = d.data.id;
        $scope.CountryList = d.data;

    }, function (error) {
        //alert('Error!');
    });


    $scope.GetState = function (countrystateid) {

        $scope.state = "";
        var getData = SellerVendorServices.getstateList(countrystateid);
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



    function GetAllSellerVendorList() {

        var getData = SellerVendorServices.GetSellerVendorList();
        getData.then(function (emp) {
            $scope.SellerVendorList = emp.data;

            angular.forEach($scope.SellerVendorList, function (obj) {
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
            //console.log(workSeller);
            //console.log("pata karna ha");
            //alert("1")
            var SellersVendorDetails = {};
            SellersVendorDetails = {
                id: workSeller.ob_tbl_seller_vendors.id,
                address: workSeller.ob_tbl_seller_vendors.address,
                vendor_name: workSeller.ob_tbl_seller_vendors.vendor_name,
                email: workSeller.ob_tbl_seller_vendors.email,
                mobile: workSeller.ob_tbl_seller_vendors.mobile,
                contact_person: workSeller.ob_tbl_seller_vendors.contact_person,
            };

            var getData = SellerVendorServices.UpdateSellerVendor(SellersVendorDetails);
            getData.then(function (msg) {

                $("#alertModal").modal('show');
                $scope.msg = msg.data;
                ClearForm();

            }, function (msg) {
                $("#alertModal").modal('show');
                $scope.msg = msg.data;
            });

            GetAllSellerVendorList();
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

        var getData = SellerVendorServices.DeleteSellerVendorId(sellerPack.ob_tbl_seller_vendors.id);
        getData.then(function (eve) {
            GetAllSellerVendorList();
        },
        function (msg) {
            $("#alertModal").modal('show');
            alert(msg.data);
        });
    }




    $scope.$watch("f1.$valid", function (isValid) {
        $scope.IsFormValid = isValid;
    });

    $scope.SaveSellerVendorDetails = function () {
        $scope.IsFormSubmitted = true;
        var value = parseInt(('' + $scope.mobile).length);
        if (value > 13) {
            $scope.numberlength = "number should be 10 character !!";
        }

        $scope.Message = "";
        if ($scope.IsFormValid) {
            //alert($scope.address);
            SellerVendorServices.SaveSelleVendor($scope.vendor_name, $scope.mobile, $scope.telephone, $scope.email,
                $scope.address, $scope.contact_person, $scope.country, $scope.state, $scope.city, $scope.pincode,
                $scope.pan, $scope.gstin).then(function (d) {
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
        $scope.vendor_name = "";
        $scope.mobile = "";
        $scope.telephone = "";
        $scope.email = "";
        $scope.address = "";
        $scope.contact_person = "";
        $scope.country = "";
        $scope.state = "";
        $scope.city = "";
        $scope.pincode = "";
        $scope.gstin = "";
        $scope.pan = "";
        $scope.IsFormSubmitted = false;
        $scope.f1.$setPristine();

        SellerVendorServices.GetSellerVendorList().then(function (d) {           
            $scope.SellerVendorList = d.data;
            angular.forEach($scope.SellerVendorList, function (obj) {
                obj["showEdit"] = true;
                obj["showDelete"] = true;
            })
        }, function (error) {
            alert('Error!' + error);
        });
    }



    $scope.keyupFunction = function ValidateEmail(mail) {      
        if (/^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/.test(mail)) {
            jAlert("Email is valid");                  
            return (true)
        }
        jAlert("Email is not valid");
        $scope.email = "";
        //alert("You have entered an invalid email address!")
        return (false)
    }


}).factory('SellerVendorServices', function ($http, $q) {
    var fac = {};

   // alert("Call");
    fac.SaveSelleVendor = function (vendor_name, mobile, telephone, email, address, contact_person, country,
        state, city, pincode, pan, gstin) {

        var formData = new FormData();
        formData.append("vendor_name", vendor_name);
        formData.append("mobile", mobile);
        formData.append("telephone", telephone);
        formData.append("email", email);
        formData.append("address", address);
        formData.append("contact_person", contact_person);
        formData.append("country", country);
        formData.append("state", state);
        formData.append("city", city);
        formData.append("pincode", pincode);
        formData.append("pan", pan);
        formData.append("gstin", gstin);


        var defer = $q.defer();
        $http.post("/Seller/Seller/SaveSellerVendorDetails", formData,
        //$http.post("/Seller/SellerVendor/SaveSellerVendorDetails", formData,
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


    fac.GetSellerVendorList = function () {
        return $http.get('/Seller/Seller/GetSellerVendor');
    }


    fac.UpdateSellerVendor = function (SellersVendorDetails) {
        //alert("2")
        var response = $http({
            method: "post",
            url: "/Seller/Seller/UpdateSellerVendorDetails",
            data: JSON.stringify(SellersVendorDetails),
            dataType: "json"
        });
        return response;
    }

    fac.DeleteSellerVendorId = function (sellerId) {
        var response = $http({
            method: "post",
            url: "/Seller/Seller/DeleteSellerVendor",
            params: {
                id: JSON.stringify(sellerId)
            }
        });

        return response;
    }
    return fac;

});