angular.module('MyAppSeller').controller('SellerMarketPlaceController', function ($scope, SellerMarketPlaceServices) {

    $scope.Message = "";
    $scope.my_unique_id = "";
    $scope.m_marketplace_id = "";
    $scope.t_loginName = "";
    $scope.t_password = "";
    $scope.t_access_Key_id = "";
    $scope.t_auth_token = "";
    $scope.t_secret_Key = "";
    $scope.market_palce_id = "";
    $scope.GSTN_No = "";
   
    $scope.hdn_Userid = "";
    $scope.Business_name = "";
    $scope.Mobile = "";
    $scope.Contact_person = "";
    $scope.Pan = "";
    $scope.GSTIN = "";
    $scope.Address = "";
    $scope.Country = "";
    $scope.City = "";
    $scope.Pincode = "";
    $scope.stateid = "";

    $scope.IsFormSubmitted = false;
    $scope.IsFormValid = false;
    $scope.id = null;
    $scope.currentPage = 1;
    $scope.numberlength = "";
    $scope.MarketPlaceList = [];
    $scope.GetSubCategory = [];
    GetAllSellerMarketPlaceList();
    GetProfile();
    $scope.statelist = "";
    $scope.GetState = [];
    $scope.stateid="";
   

    SellerMarketPlaceServices.getMarketPlaceList().then(function (d) {       
        $scope.id = d.data.id;
        $scope.MarketPlaceList = d.data;
    }, function (error) {
        //alert('Error!');
    });


  
    $scope.pageChangeHandler = function (num) {
        console.log('meals page changed to ' + num);
    };



    function GetAllSellerMarketPlaceList() {
        var getData = SellerMarketPlaceServices.GetSellerMArketList();
        getData.then(function (emp) {
            $scope.SellerMarketList = emp.data;
            angular.forEach($scope.SellerMarketList, function (obj) {
                obj["showEdit"] = true;
                obj["showDelete"] = true;
            })
        }, function (emp) {
            //alert("Records gathering failed!");
        });
    }

   
    function GetProfile() {       
        var getData = SellerMarketPlaceServices.getprofile_details();
        getData.then(function (emp) {
           
            $scope.hdn_Userid = emp.data.ProfileID;
            $scope.Business_name = emp.data.business_name;
            $scope.Mobile = emp.data.mobile;
            $scope.Contact_person = emp.data.contact_person;
            $scope.Pan = emp.data.pan;
            $scope.GSTIN = emp.data.gstin;
            $scope.Address = emp.data.address;
            $scope.Country = emp.data.country;
            $scope.City = emp.data.city;
            $scope.Pincode = emp.data.pincode;
            $scope.stateid = emp.data.state;
            //alert("profile" + $scope.stateid);
            //alert("Profile1" + $scope.Country);
        }, function (emp) {
            //alert("Records gathering failed!");
        });
    }

    SellerMarketPlaceServices.GetCountry().then(function (d) {
        $scope.id = d.data.id;
        $scope.CountryList = d.data;
    }, function (error) {
        alert('Error!');
  });

    $scope.GetState = function (countryid) {
        //alert("state" + countryid);
        var getData = SellerMarketPlaceServices.getstateList(countryid);
        getData.then(function (eve) {
            console.log(eve);
            $scope.id = eve.data.id;
            $scope.statelist = eve.data;           
        }, function (eve) {         
     });
  }

  
    $scope.UpdateUserProfile = function () {       
        $scope.IsFormSubmitted = true;
        $scope.Message = "";
            
            var ProfileDetails = {};
            ProfileDetails = {
                ProfileID: $scope.hdn_Userid,
                contact_person: $scope.Contact_person,
                country: $scope.Country,
                address: $scope.Address,
                business_name: $scope.Business_name,
                city: $scope.City,
                gstin: $scope.GSTIN,
                mobile: $scope.Mobile,
                pan: $scope.Pan,
                pincode: $scope.Pincode,
                state: $scope.stateid,
            };

            var getData = SellerMarketPlaceServices.Updateprofile(ProfileDetails);
            getData.then(function (msg) {               
                jAlert(msg.data.Message, 'Message');
                $("#alertModal").modal('show');
                $scope.msg = msg.data;
                GetAllSellerMarketPlaceList();
                GetProfile();

            }, function (msg) {
                $("#alertModal").modal('show');
                $scope.msg = msg.data;
            });
            GetAllSellerMarketPlaceList();
            GetProfile();             
    };

    $scope.toggleEdit = function (workSeller) {
        workSeller.showEdit = workSeller.showEdit ? false : true;
        if (workSeller.showEdit) {
            var Details = {};       
             Details = {
                 id: workSeller.id,
                 my_unique_id: workSeller.my_unique_id,
                 m_marketplace_id: workSeller.m_marketplace_id,
                 t_loginName: workSeller.t_loginName,
                 t_password: workSeller.t_password,
                 t_access_Key_id: workSeller.t_access_Key_id,
                 t_auth_token: workSeller.t_auth_token,
                 t_secret_Key: workSeller.t_secret_Key,
                 market_palce_id: workSeller.market_palce_id,
                 GSTN_No: workSeller.GSTN_No,
            };
             var getData = SellerMarketPlaceServices.UpdateMarketPlace(Details);
             getData.then(function (msg) {
                $("#alertModal").modal('show');
                $scope.msg = msg.data;
                ClearForm();
            }, function (msg) {
                $("#alertModal").modal('show');
                $scope.msg = msg.data;
            });
            GetAllSellerMarketPlaceList();
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
        var getData = SellerMarketPlaceServices.DeleteByID(sellerPack.id);
        getData.then(function (eve) {
            GetAllSellerMarketPlaceList();
        },
        function (msg) {
            $("#alertModal").modal('show');
            alert(msg.data);
        });
    }

    $scope.chkClick = function () {    
        if ($("#chkYes").is(":checked")) {
            $scope.dvPassport = true;          
        } else {
            $scope.dvPassport = false;          
        }
    }
    $scope.IsVisible = false;
    $scope.ShowPassport = function (value) {      
        $scope.IsVisible = value == "Y";
    }


    $scope.$watch("f1.$valid", function (isValid) {
        $scope.IsFormValid = isValid;
    });

    $scope.SaveSellerMarketPlace = function () {
        $scope.IsFormSubmitted = true;     
        $scope.Message = "";
        //alert("call " + $scope.GSTN_No);
        if ($scope.IsFormValid) {
           
            SellerMarketPlaceServices.SaveDetails($scope.my_unique_id, $scope.m_marketplace_id, $scope.t_loginName,
                $scope.t_password, $scope.t_access_Key_id, $scope.t_auth_token, $scope.t_secret_Key, $scope.market_palce_id, $scope.GSTN_No).then(function (d) {
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
        $scope.my_unique_id = "";
        $scope.m_marketplace_id = "";
        $scope.t_loginName = "";
        $scope.t_password = "";
        $scope.t_access_Key_id = "";
        $scope.t_auth_token = "";
        $scope.t_secret_Key = "";
        $scope.market_palce_id = "";
        $scope.GSTN_No = "";
        $scope.IsFormSubmitted = false;
        $scope.f1.$setPristine();
        SellerMarketPlaceServices.GetSellerMArketList().then(function (d) {          
            $scope.SellerMarketList = d.data;
            angular.forEach($scope.SellerMarketList, function (obj) {
                obj["showEdit"] = true;
                obj["showDelete"] = true;
            })
        }, function (error) {
            alert('Error!' + error);
        });
    }


}).factory('SellerMarketPlaceServices', function ($http, $q) {
    var fac = {};
    
    fac.SaveDetails = function (my_unique_id, m_marketplace_id, t_loginName, t_password, t_access_Key_id, t_auth_token, t_secret_Key, market_palce_id, GSTN_No) {
        var formData = new FormData();
       
        formData.append("my_unique_id", my_unique_id);
        formData.append("m_marketplace_id", m_marketplace_id);
        formData.append("t_loginName", t_loginName);
        formData.append("t_password", t_password);
        formData.append("t_access_Key_id", t_access_Key_id);
        formData.append("t_auth_token", t_auth_token);
        formData.append("t_secret_Key", t_secret_Key);
        formData.append("market_palce_id", market_palce_id);
        formData.append("GSTN_No", GSTN_No);
        var defer = $q.defer();
        $http.post("/Seller/Seller/SaveMarketPlaceDetails", formData,
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

    fac.getMarketPlaceList = function () {
        return $http.get("/Seller/Seller/FillMarketPlaceID");
    };  

    fac.GetSellerMArketList = function () {
        return $http.get('/Seller/Seller/GetSellermarketDetails');
    }

    fac.getprofile_details = function () {       
        return $http.get("/Seller/SellerSetting/GetProfileDetails");
    };

    fac.GetCountry = function () {
        return $http.get('/Seller/SellerSetting/FillCountry');
    }
    fac.getstateList = function (countryid) {
        return $http.get("/Seller/SellerSetting/FillState?countryid=" + countryid);
    };
    
    fac.UpdateMarketPlace = function (Details) {     
        var response = $http({
            method: "post",
            url: "/Seller/Seller/UpdateDetails",
            data: JSON.stringify(Details),
            dataType: "json"
        });
        return response;
    }

    fac.Updateprofile = function (ProfileDetails) {       
        var response = $http({
            method: "post",
            url: "/Seller/SellerSetting/UpdateProfile",
            data: JSON.stringify(ProfileDetails),
            dataType: "json"
        });
        return response;
    }
    fac.DeleteByID = function (Id) {
        var response = $http({
            method: "post",
            url: "/Seller/Seller/DeleteSellerMarket",
            params: {
                id: JSON.stringify(Id)
            }
        });

        return response;
    }
    return fac;

});