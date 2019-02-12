angular.module('MyAppSeller').controller('ManageCategoryController', function ($scope, ManageCategoryServices) {

    $scope.Message = "";
    $scope.brand = "";
    $scope.tbl_item_category_id = "";
    $scope.IsFormSubmitted = false;
    $scope.IsFormValid = false;
    $scope.id = null;
    $scope.currentPage = 1;
    $scope.CategoryCustomers = [];
    $scope.category_name = "";
    $scope.hsn_code = "";
    GetCategoryList();
    $scope.Customers = [];

    function GetCategoryList() {        
        var getData = ManageCategoryServices.CategoryList();
        getData.then(function (eve) {
            $scope.id = eve.data.id;
            $scope.Category = eve.data;
            angular.forEach($scope.Category, function (obj) {
                obj["showEdit"] = true;
                obj["showDelete"] = true;
            })
        }, function (eve) {
            // alert("Records gathering failed!");
        });
    }

    
    $scope.AddCategoryClick = function () {
       
        $scope.errorflag = true;
        $scope.IsFormSubmitted = true;
        $scope.IsFormValid = true;
        var CategoryDetails = {};
        if ($scope.IsFormValid) { 
            CategoryDetails = {
                category_name: $scope.category_name,
                hsn_code: $scope.hsn_code,
            };
            var txtfrom = $scope.txt_from;
            var txtto = $scope.txt_to;
            var taxrate = $scope.taxrate;
            if (txtfrom != undefined && txtfrom != '' && txtto != undefined && txtto != '' && taxrate != undefined && taxrate != '') {
                var customer = {};
                customer.index = $scope.Customers.length;
                customer.from_rs = $scope.txt_from;
                customer.to_rs = $scope.txt_to;
                customer.tax_rate = $scope.taxrate;
                $scope.Customers.push(customer);
                $scope.txt_from = "";
                $scope.txt_to = "";
                $scope.taxrate = "";
            }
            else {
                //if ($scope.Customers.length == 0) {
                    alert("Fill all these fields !")
                    $scope.msg = "Fill date";
                    return;
               // }
            }
            $.ajax({
                type: "POST",
                url: "/Seller/Seller/AddCategory",
                data: JSON.stringify({ category_slabs: $scope.Customers, Eve: CategoryDetails }),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (result) {
                    alert('success');                    
                    ClearCategoryFields();
                    GetCategoryList();
                },
                error: function (result) {
                    alert('error');                   
                    ClearCategoryFields();
                    GetCategoryList();
                }
            });
             $scope.Action = "Add";
             $scope.IsVisible = false;
             IsFormSubmitted = false;
             $("#myModal").modal('hide');
        }
        else {
            $scope.Message = "All the fields are required.";
        }
        // $scope.refresh();
    }
   

    $scope.Add = function () {
        var txtfrom = $scope.txt_from;
        var txtto = $scope.txt_to;
        var taxrate = $scope.taxrate;
        if (txtfrom != '' && txtto != '' && taxrate !='') {           
            var customer = {};
            customer.index = $scope.Customers.length;
            customer.from_rs = $scope.txt_from;
            customer.to_rs = $scope.txt_to;
            customer.tax_rate = $scope.taxrate;
            $scope.Customers.push(customer);
            $scope.txt_from = "";
            $scope.txt_to = "";
            $scope.taxrate = "";
        }
        else {
            alert("Fill all these fields !")
            $scope.msg = "Fill date";
            return
        }
    };
    function ClearCategoryFields() {
        $scope.category_name = "";
        $scope.hsn_code = "";
        $scope.Customers = [];
        $scope.txt_from = "";
        $scope.txt_to = "";
        $scope.taxrate = "";
    }

    $scope.ClearData = function () {
        $scope.category_name = "";
        $scope.hsn_code = "";
        $scope.Customers = [];       
        $scope.CustomersCopy = [];
        $scope.txt_from = "";
        $scope.txt_to = "";
        $scope.taxrate = "";
        GetCategoryList();
    }
   
    $scope.CategoryUpdate = function (index) {       
        $scope.CustomersCopy = [];
        angular.forEach($scope.Customers, function (obj) {
            if (obj.index == index) {
                var customer = {};
                customer.index = index;
                customer.from_rs = $('#txt_from' + index).val();
                customer.to_rs = $('#txt_to' + index).val();
                customer.tax_rate = $('#txt_taxrate' + index).val();
                $scope.CustomersCopy.push(customer);

            }
            else
                $scope.CustomersCopy.push(obj);
        })
        $scope.Customers = $scope.CustomersCopy;
    };

    $scope.Edit = function (index) {
        var ID = index;
        $('#txt_from' + ID).prop('disabled', false);
        $('#txt_taxrate' + ID).prop('disabled', false);
        $('#txt_to' + ID).prop('disabled', false);
    }
    $scope.Remove = function (index) {
        var name = $scope.Customers[index].Name;
        {
            $scope.Customers.splice(index, 1);
        }
    }
    
    //----------------------------------------for view Category only-----------------------------//

    //$scope.checkUserAvailable = function () {
    //    alert("username");
    //    ManageCategoryServices.IsUserNameAvailablle($scope.category_name).then(function (userstatus) {
    //        $scope.category_name.$setValidity('unique', userstatus);
    //    }, function () {
    //        alert('error while checking user from server');
    //    });
    //};

    $scope.clearviewpopup = function () {
        $scope.category_name = "";
        $scope.hsn_code = "";
       
        $scope.ViewCustomers = [];
        $scope.CustomersCopy = [];
        $scope.txt_from = "";
        $scope.txt_to = "";
        $scope.taxrate = "";
        GetCategoryList();
    }
    $scope.ViewCustomers = [];
    $scope.ViewCategory = function (tbl_item_category_id) {
        //alert(tbl_item_category_id);
        $scope.IsVisible = true;
        var getData = ManageCategoryServices.get_categoryslabs(tbl_item_category_id);
        getData.then(function (eve) {

            $scope.Event = eve.data;
            $scope.category_name = $scope.Event[0].category_name,
            $scope.hsn_code = $scope.Event[0].hsn_code,
            $scope.hdn_Item_id = $scope.Event[0].id,
            $scope.ViewCustomers = $scope.Event[0].obj_partial_slabs;            
            angular.forEach($scope.ViewCustomers, function (obj) {
            })
            $scope.Action = "View";
            $scope.IsVisible = false;
            IsFormSubmitted = false;
        });
    }

    $scope.UpdateViewCategoryClick = function () {
        
            var ViewCategoryDetails = {};
            ViewCategoryDetails = {
                category_name: $scope.category_name,
                hsn_code: $scope.hsn_code,
                id: $scope.hdn_Item_id,
            };
            var txtfrom = $scope.txt_from;
            var txtto = $scope.txt_to;
            var taxrate = $scope.taxrate;
            if (txtfrom != undefined && txtfrom != '' && txtto != undefined && txtto != '' && taxrate != undefined && taxrate != '') {
                var viewcustomer = {};
                viewcustomer.index = $scope.ViewCustomers.length;
                viewcustomer.from_rs = $scope.txt_from;
                viewcustomer.to_rs = $scope.txt_to;
                viewcustomer.tax_rate = $scope.taxrate;
                $scope.ViewCustomers.push(viewcustomer);
                $scope.txt_from = "";
                $scope.txt_to = "";
                $scope.taxrate = "";
            }
            else {
                //if ($scope.Customers.length == 0) {
                alert("Fill all these fields !")
                $scope.msg = "Fill date";
                return;
                // }
            }
            alert("save call");
            $.ajax({
                type: "POST",
                url: "/Seller/Seller/UpdateCategoryWithSlab",
                data: JSON.stringify({ category_slabs: $scope.ViewCustomers, Eve: ViewCategoryDetails }),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (result) {
                    alert('success');
                    GetCategoryList();
                    ClearCategoryFields();
                },
                error: function (result) {
                    alert('error');
                    GetCategoryList();
                    ClearCategoryFields();
                }
            });
            $scope.Action = "Add";
            $scope.IsVisible = false;
            IsFormSubmitted = false;
        
    };

    $scope.ViewAdd = function () {
        var txtfrom = $scope.txt_from;
        var txtto = $scope.txt_to;
        var taxrate = $scope.taxrate;
        if (txtfrom != '' && txtto != '' && taxrate != '') {
            var viewcustomer = {};
            viewcustomer.index = $scope.ViewCustomers.length;
            viewcustomer.from_rs = $scope.txt_from;
            viewcustomer.to_rs = $scope.txt_to;
            viewcustomer.tax_rate = $scope.taxrate;
            $scope.ViewCustomers.push(viewcustomer);
            $scope.txt_from = "";
            $scope.txt_to = "";
            $scope.taxrate = "";
        }
        else {
            alert("Fill all these fields !")
            $scope.msg = "Fill date";
            return
        }
    };
    
    $scope.ViewEdit = function () {
        var ID = index;
        $('#txt_from' + ID).prop('disabled', false);
        $('#txt_taxrate' + ID).prop('disabled', false);
        $('#txt_to' + ID).prop('disabled', false);
    };
    $scope.ViewRemove = function (index) {
        var name = $scope.ViewCustomers[index].Name;
        {
            $scope.ViewCustomers.splice(index, 1);
        }
    };

    $scope.ViewCategoryUpdate = function () {
        //alert(index);
        $scope.ViewCustomersCopy = [];
        angular.forEach($scope.ViewCustomers, function (obj) {
            if (obj.index == index) {
                var viewcustomer = {};
                viewcustomer.index = index;
                viewcustomer.from_rs = $('#txt_from' + index).val();
                viewcustomer.to_rs = $('#txt_to' + index).val();
                viewcustomer.tax_rate = $('#txt_taxrate' + index).val();
                $scope.ViewCustomersCopy.push(viewcustomer);

            }
            else
                $scope.ViewCustomersCopy.push(obj);
        })
        $scope.ViewCustomers = $scope.ViewCustomersCopy;
    };
    //-----------------------------------------------End----------------------------------------//



    $scope.$watch("f1.$valid", function (isValid) {
        $scope.IsFormValid = isValid;
    });

}).factory('ManageCategoryServices', function ($http, $q) {
    var fac = {};
    fac.CategoryList = function () {
        return $http.get('/Seller/Seller/ManageCategoryList');
    }


    fac.get_categoryslabs = function (id) {
        return $http.get("/Seller/Seller/ViewTaxDetails?id=" + id);
    };

    //fac.IsUserNameAvailablle = function (userName) {
    //    alert("username" + userName);
    //    var deferred = $q.defer();
    //    $http({
    //        method: "Get",
    //        url: "/Seller/Seller/IsUserNameAvailable?userName=" + userName,
           
    //        dataType: "json"
    //    }).success(deferred.resolve).error(deferred.reject);
    //    return deferred.promise;
    //    //var deferred = $q.defer();
    //    //$http({ method: 'GET', url: 'Seller/Seller/IsUserNameAvailable?userName=' + userName }).success(deferred.resolve).error(deferred.reject);
    //    //return deferred.promise;
    //};

    fac.UpdateCategory = function (CategoryDetails) {
       
        var response = $http({
            method: "post",
            url: "/Seller/Seller/UpdateCategoryList",
            data: JSON.stringify(CategoryDetails),
            dataType: "json"
        });
        return response;
    }
    fac.DeleteCategoryByID = function (purchaseId) {
        var response = $http({
            method: "post",
            url: "/Seller/Seller/DeleteCategory",
            params: {
                id: JSON.stringify(purchaseId)
            }
        });

        return response;
    }

    return fac;

});