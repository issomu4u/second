angular.module('MyAppseller2').controller('getInventoryController', function ($scope,$location, getInventoryService) {


    $scope.currentPage = 1;
    $scope.item_name = "";
    $scope.showImages = false;
    $scope.InventoryDetailList = [];  
    $scope.inventoryId = $location.absUrl().split('=')[1];
    ViewInventoryDetailList($scope.inventoryId)
    GetItemStatusList();
    $scope.PartnerList = null;
    $scope.item_serial_No = "";
  
   $scope.$watch("f1.$valid", function (isValid) {
        $scope.IsFormValid = isValid;
    });


    $scope.alertmsg = function () {
        $("#alertModal").modal('hide');
    };

   

    function ViewInventoryDetailList(colorPack) {
      
        $scope.IsVisible = true;
       
        var getData = getInventoryService.InventoryDetailsByID(colorPack);
        getData.then(function (emp) {
                     
            $scope.showImages = true;
            $scope.item_name = emp.data[0].ob_tbl_inventory.item_name;
            $scope.item_photo1_path = emp.data[0].ob_tbl_inventory.item_photo1_path;
            $scope.InventoryDetailList = emp.data;
           
           
            angular.forEach($scope.InventoryDetailList, function (obj) {
                obj["showEdit"] = true;
                obj["showDelete"] = true;
            })

        }, function (emp) {
            alert("Records gathering failed!");
        });
    }


    $scope.toggleEdit = function (emp) {
        emp.showEdit = emp.showEdit ? false : true;
        if (emp.showEdit) {
            //alert(emp.ob_tbl_inventory_details.item_serial_No);
            getInventoryService.UpdateInventory(emp.ob_tbl_inventory_details.id, emp.ob_tbl_inventory_details.m_item_status_id, emp.ob_tbl_inventory_details.item_serial_No).then(function (d) {
                alert(d.Message);
                //ClearForm();

            }, function (e) {
                alert(e);
            });
        }
        else {

        }
    }

    $scope.toggleCancelEdit = function (emp) {
        emp.showEdit = true;
        eve.showDelete = true;
    }
    function GetItemStatusList() {
        var getData = getInventoryService.GetItemStatus();
        getData.then(function (eve) {
           // alert("hiii");
            //console.log("drop");
            //console.log(eve);
            $scope.id = eve.data.id;
            $scope.StatusList = eve.data;
        }, function (eve) {
            alert("Records gathering failed!");
        });
    }

    //function GetItemStatusList() {
    //    getInventoryService.GetItemStatus.then(function (d) {
    //        $scope.PartnerList = d.data;

    //    }, function (error) {
    //        alert('Error!');
    //    });
    //}
}).factory('getInventoryService', function ($http, $q) {
    var fac = {};

    

    
    fac.InventoryDetailsByID = function (colorId) {
        return $http.get('/Seller/Seller/InventoryDetailList1?id=' + colorId);
    }

    fac.GetItemStatus = function () {
        return $http.get('/Seller/Seller/FillItemStatus');
    }
  
    fac.UpdateInventory = function (id, m_item_status_id, item_serial_No) {
       // alert("260")

        var formData = new FormData();
        formData.append("id", id);
        formData.append("m_item_status_id", m_item_status_id);
        formData.append("item_serial_No", item_serial_No);
       
        var defer = $q.defer();
        $http.post("/Seller/Seller/UpdateMyInventoryDetailsByStatus", formData,
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

    return fac;

});