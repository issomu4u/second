angular.module('MyAppseller2').controller('MyInventoryController', function ($scope, MyInventoryServices) {

    $scope.Message = "";
    $scope.brand = "";
  
    $scope.item_code = "";
    $scope.item_description = "";
    $scope.item_name = "";
    $scope.item_dimension = "";
   
    $scope.packed_dimension = "";
    $scope.sku = "";
    $scope.item_count = "";
    $scope.mrp = "";
    $scope.packed_weight = "";
    $scope.selling_price = "";
    $scope.m_item_color_id = "";
    $scope.tbl_item_category_id = "";
    $scope.tbl_item_subcategory_id = "";
    $scope.item_weight = "";
    $scope.transfer_price = "";
    $scope.FileInvalidMessage = "";
    $scope.FileInvalidMessage1 = "";
    $scope.FileInvalidMessage2 = "";
    $scope.SelectedFileForUpload = null;
    $scope.SelectedFileForUpload1 = null;
    $scope.SelectedFileForUpload2 = null;
    $scope.IsFormSubmitted = false;
    $scope.IsFormValid = false;
    $scope.id = null;
    $scope.currentPage = 1;
    $scope.SellerList = null;

    $scope.ColorList = null;
    $scope.CategoryList = null;
    $scope.subcategorylist = null;

    $scope.CategoryList = [];
    $scope.GetSubCategory = [];
    $scope.ColorList = [];
    $scope.tbl_item_subcategory_id = 0;
    $scope.showImages = false;
    $scope.buttonTxt = "Add";
    $scope.hdn_AddUpdate_id = "0";
    GetColorList();
    $scope.InventoryDetailList = "";
    GetAllInventoryList();
    getFullInventoryList();
    

    MyInventoryServices.GetCategoryForInventory().then(function (d) {
        $scope.id = d.data.id;
        $scope.CategoryList = d.data;

    }, function (error) {
        alert('Error!');
    });
    //$scope.CategoryChange = function (id) {
    //    alert(id);
    //    $scope.GetSubCategory(id);
    //    //getInventoryList(id);
        
    //}


    function getInventoryList(id) {
        var getData = MyInventoryServices.GetInventoryList(id, 0);
        getData.then(function (emp) {
            $scope.InventoryList = emp.data;
            angular.forEach($scope.InventoryList, function (obj) {
                obj["showEdit"] = true;
                obj["showDelete"] = true;
            })

        }, function (emp) {
            alert("Records gathering failed!");
        });
    }

    $scope.GetSubCategory = function (categoryid) {
        //alert("sub category" + categoryid);
        $scope.tbl_item_subcategory_id = categoryid;
        var getData = MyInventoryServices.getsubcategoryList(categoryid);
        getData.then(function (eve) {
            $scope.subcategorylist = eve.data;
            //console.log(eve.data);
        }, function (eve) {
            alert("Records gathering failed!");
        });
    }

    
    //$scope.SubCategoryChange = function (tbl_item_subcategory_id) {
    //    var getData = MyInventoryServices.GetInventoryList(0, tbl_item_subcategory_id);
    //    getData.then(function (emp) {
    //        $scope.InventoryList = emp.data;
    //        angular.forEach($scope.InventoryList, function (obj) {
    //            obj["showEdit"] = true;
    //            obj["showDelete"] = true;
    //        })

    //    }, function (emp) {
    //        alert("Records gathering failed!");
    //    });
    //}
   
    $scope.pageChangeHandler = function (num) {
        console.log('meals page changed to ' + num);
    };

    function GetColorList() {
        var getData = MyInventoryServices.GetColorForInventory();
        getData.then(function (eve) {
            $scope.id = eve.data.id;
            $scope.ColorList = eve.data;
        }, function (eve) {
            alert("Records gathering failed!");
        });
    }


    $scope.SearchRecords1 = function () {
        alert("Search Work");
        getInventoryList($scope.tbl_item_category_id);

    }

    function GetAllInventoryList() {
        var getData = MyInventoryServices.GetInventoryList(0,0);
        getData.then(function (emp) {
            $scope.InventoryList = emp.data;
            angular.forEach($scope.InventoryList, function (obj) {
                obj["showEdit"] = true;
                obj["showDelete"] = true;
            })

        }, function (emp) {
            alert("Records gathering failed!");
        });
    }

    $scope.toggleCancelEdit = function (emp) {
        emp.showEdit = true;
        eve.showDelete = true;
    }

   
    //$scope.editEvent = function (EditEvent) {
    //    $scope.IsVisible = true;
    //    var getData = EventService.getEvent(EditEvent.id);
    //    getData.then(function (eve) {
    //        $scope.Event = eve.data;
    //        $scope.id = $scope.Event.id,
    //        $scope.event_name = $scope.Event.event_name,
    //        $scope.event_from_date = $filter('date')(parseInt($scope.Event.event_from_date.substr(6)), "dd-MM-yyyy");
    //        $scope.event_to_date = $filter('date')(parseInt($scope.Event.event_to_date.substr(6)), "dd-MM-yyyy");
    //        $scope.event_location = $scope.Event.event_location,
    //        $scope.event_text = $scope.Event.event_text,
    //        $scope.event_description = $scope.Event.event_description,
    //        $scope.event_banner_url = $scope.Event.event_banner_url
    //        $scope.Action = "Edit";
    //        $("#myModal").modal('show');
    //    },
    //    function (msg) {
    //        $("#alertModal").modal('show');
    //        $scope.msg = msg.data;
    //    });
    //}

    $scope.ViewInventoryDetailList = function (colorPack) {
        $scope.IsVisible = true;
        alert("PopUp Open");
        var getData = MyInventoryServices.InventoryDetailsByID(colorPack.ob_tbl_inventory.id);
        
        //$('#ItemModal').modal('toggle');
        getData.then(function (eve) {
            alert("POPUP" + eve.data);
            //$("#ViewInventoryDetailsList").html(eve.data)
            window.location.href = '/InventoryDetailList';
         
        });
        
        
    }





    $scope.toggleEdit = function (emp) {

        //find the value to upadate the table details in database
        emp.showEdit = emp.showEdit ? false : true;
        if (emp.showEdit) {
            MyInventoryServices.UpdateInventory(emp.ob_tbl_inventory.id, emp.ob_tbl_inventory.selling_price, emp.ob_tbl_inventory.item_count).then(function (d) {
                alert(d.Message);
                //  $scope.Message = d.Message;
                ClearForm();

            }, function (e) {
                alert(e);
            });
        }
        else {
            //emp.id = emp.ob_m_partner_type.id;
        }
    }


    $scope.$watch("f1.$valid", function (isValid) {
        $scope.IsFormValid = isValid;
    });
   

    function ClearForm() {

        $scope.id = 0;
        $scope.brand = "";
        // $scope.hsn_code = "";
        $scope.item_code = "";
        $scope.item_description = "";
        $scope.item_dimension = "";
        $scope.item_name = "";
        //$scope.remarks = "";
        $scope.packed_dimension = "";
        $scope.sku = "";
        $scope.item_count = "";
        $scope.mrp = "";
        $scope.item_photo1_path = "";
        $scope.item_photo2_path = "";
        $scope.item_photo3_path = "";
        $scope.packed_weight = "";
        $scope.selling_price = "";
        $scope.m_item_color_id = "";
        $scope.tbl_item_category_id = "";
        $scope.tbl_item_subcategory_id = "";
        $scope.item_weight = "";
        $scope.transfer_price = "";
        $scope.showImages = false;

        angular.forEach(angular.element("input[type='file']"), function (inputElem) {
            angular.element(inputElem).val(null);
        });
        angular.forEach(angular.element("input[type='file1']"), function (inputElem) {
            angular.element(inputElem).val(null);
        });
        angular.forEach(angular.element("input[type='file2']"), function (inputElem) {
            angular.element(inputElem).val(null);
        });
        $scope.IsFormSubmitted = false;
        $scope.f1.$setPristine();

        MyInventoryServices.GetInventoryList().then(function (d) {
            //console.log(d);
            $scope.InventoryList = d.data;
            angular.forEach($scope.InventoryList, function (obj) {
                obj["showEdit"] = true;
                obj["showDelete"] = true;
            })
        }, function (error) {
            alert('Error!' + error);
        });


    }

    function getFullInventoryList(id) {
        var getData = MyInventoryServices.InventoryDetailList(id);
        getData.then(function (emp) {
            $scope.InventoryDetailList = emp.data;
            angular.forEach($scope.InventoryDetailList, function (obj) {
                obj["showEdit"] = true;
                obj["showDelete"] = true;
            })

        }, function (emp) {
            alert("Records gathering failed!");
        });
    }
   

}).factory('MyInventoryServices', function ($http, $q) {
    var fac = {};
 
    fac.GetInventoryList = function (categoryID,subcategoryID) {
        return $http.get('/Seller/Seller/GetMyInventory?categoryID=' + categoryID + '&subcategoryID=' + subcategoryID);
    }

    fac.GetColorForInventory = function () {
        return $http.get('/Seller/Seller/FillColor');
    }

    fac.InventoryDetailList = function (ID) {
        return $http.get('/Seller/Seller/InventoryDetailList?ID=' + ID);
    }


    //fac.GetCategoryForInventory = function () {
    //    return $http.get('/Seller/Seller/FillCategory');
    //}

    //fac.getsubcategoryList = function (categoryid) {
    //    alert("services" + categoryid)
    //    return $http.get("/Seller/Seller/FillSubCategory?categoryid=" + categoryid);
    //};

    fac.GetCategoryForInventory = function () {
        return $http.get('/Seller/Seller/FillCategory');
    }

    fac.getsubcategoryList = function (categoryid) {
        alert("services" + categoryid)
        return $http.get("/Seller/Seller/FillSubCategory?categoryid=" + categoryid);
    };

    fac.UpdateInventory = function (id, selling_price, item_count) {
        alert("260")

        var formData = new FormData();
        formData.append("id", id);
        formData.append("selling_price", selling_price);
        formData.append("item_count", item_count);      
        var defer = $q.defer();
        $http.post("/Seller/Seller/UpdateMyInventoryDetails", formData,
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


    fac.InventoryDetailsByID = function (colorId) {
        var response = $http({
            method: "get",
            url: "/Seller/Seller/ViewInventoryDetailsList",
            params: {
                id: JSON.stringify(colorId)
            }
        });

        return response;
    }
    

  
    return fac;

});