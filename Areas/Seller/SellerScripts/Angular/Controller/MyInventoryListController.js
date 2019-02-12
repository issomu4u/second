angular.module('MyAppseller2').controller('MyInventoryListController', function ($scope, MyInventoryServices) {
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
    $scope.currentPage = 1;

    $scope.CategoryList = [];
    $scope.GetSubCategory = [];
    $scope.tbl_item_subcategory_id = 0;
    $scope.showImages = false;
    $scope.buttonTxt = "Add";
    $scope.hdn_AddUpdate_id = "0";
    GetColorList();
    
    GetAllInventoryList();
    GetItemNameForInventoryList();
    GetWarehouseForInventoryList();
    $scope.IsHidden = true;
    $scope.t_virtualItemCount = "";
    $scope.MarketPlaceList = [];

    function GetColorList() {      
        var getData = MyInventoryServices.GetColorForInventory();
        getData.then(function (eve) {          
            $scope.id = eve.data.id;
            $scope.ColorList = eve.data;
        }, function (eve) {
            alert("Records gathering failed!");
        });
    }

    MyInventoryServices.getMarketPlaceList().then(function (d) {      
        $scope.id = d.data.id;
        $scope.MarketPlaceList = d.data;
    }, function (error) {
        alert('Error!');
    });

    
    function GetItemNameForInventoryList() {
        var getData = MyInventoryServices.GetItemNameForTransfer();
        getData.then(function (eve) {
            $scope.id = eve.data.id;
            $scope.ItemNameList = eve.data;
        }, function (eve) {
            alert("Records gathering failed!");
        });
    }

    function GetWarehouseForInventoryList() {
        var getData = MyInventoryServices.GetWarehouseForTransfer();
        getData.then(function (eve) {
            $scope.id = eve.data.id;
            $scope.WarehouseList = eve.data;
        }, function (eve) {
            alert("Records gathering failed!");
        });
    }

    MyInventoryServices.GetCategoryForInventory().then(function (d) {
        $scope.id = d.data.id;
        $scope.CategoryList = d.data;

    }, function (error) {
        alert('Error!');
    });


    $scope.GetSubCategory = function (categoryid) {
        $scope.tbl_item_subcategory_id = categoryid;
        var getData = MyInventoryServices.getsubcategoryList(categoryid);
        getData.then(function (eve) {
            $scope.subcategorylist = eve.data;       
        }, function (eve) {
            alert("Records gathering failed!");
        });
    }

    $scope.pageChangeHandler = function (num) { 
        console.log('meals page changed to ' + num);
    };

    $scope.SearchRecords1 = function () {       
        getInventoryList($scope.tbl_item_category_id, $scope.tbl_item_subcategory_id);    
    }

    function getInventoryList(catId, subCatId) {
        alert("fff");
        var getData = MyInventoryServices.GetInventoryList(catId, subCatId);
        getData.then(function (emp) {
            $scope.InventoryList = emp.data;
            angular.forEach($scope.InventoryList, function (obj) {
                obj["showEdit"] = true;
                obj["showDelete"] = true;
            })
            console.log("sss");
            console.log($scope.InventoryList);
        }, function (emp) {
            alert("Records gathering failed!");
        });
    }

    function GetAllInventoryList() {
        alert("dsds");
        var getData = MyInventoryServices.GetInventoryList(0, 0);
        getData.then(function (emp) {
            $scope.InventoryList = emp.data;
            console.log("sd");
            console.log($scope.InventoryList);
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



    $scope.toggleEdit = function (emp) {       
        emp.showEdit = emp.showEdit ? false : true;
        if (emp.showEdit) {
            MyInventoryServices.UpdateInventory(emp.ob_tbl_inventory.id, emp.ob_tbl_inventory.selling_price, emp.ob_tbl_inventory.item_count).then(function (d) {
                //alert(d.Message);              
                ClearForm();

            }, function (e) {
                alert(e);
            });
        }
        else {
         
        }
    }


    //$scope.toggleDeleteEdit = function (sellerPack) {

    //    var getData = InventoryServices.DeleteInventoryByID(sellerPack.ob_tbl_inventory.id);
    //    getData.then(function (eve) {
    //        GetAllInventoryList();
    //    },
    //    function (msg) {
    //        $("#alertModal").modal('show');
    //        alert(msg.data);
    //    });
    //}

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

        InventoryServices.GetInventoryList().then(function (d) {
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


   

   

    $scope.alertmsg = function () {
        $("#alertModal").modal('hide');
    };

   
    
    $scope.ViewInventoryDetailList = function (colorPack) {
        $scope.IsVisible = true;     
        var getData = MyInventoryServices.InventoryDetailsByID(colorPack.ob_tbl_inventory.id);
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

       
    $scope.ShowHide = function () {          
           $scope.IsHidden = $scope.IsHidden ? false : true;    
    };
 
   
    $scope.myFunc = function (Item_Transfer_Count) {
        var a = parseInt(Item_Transfer_Count);
        var b = parseInt($scope.item_total_count);
             if(a>b)
             {
                 $scope.Item_Transfer_Count = '';
             }
         };
   
   

    $scope.AddTransferItemintoWarehouse = function (EditEvent) {
        $scope.IsVisible = true;
        var getData = MyInventoryServices.getInventoryCountList(EditEvent.ob_tbl_inventory.id);        
        getData.then(function (eve) {
            $scope.Event = eve.data;
            $scope.hdn_Item_id = $scope.Event[0].ob_tbl_inventory.id,
            $scope.t_ItemName = $scope.Event[0].ob_tbl_inventory.item_name,
            $scope.item_total_count = $scope.Event[0].ob_tbl_inventory.item_count,
            $scope.from_warehouse_id = $scope.Event[0].ob_tbl_purchase.tbl_seller_warehouses_id,          
            $scope.Action = "Add";
            $scope.IsVisible = false;
            IsFormSubmitted = false;
        });
    }

    $scope.VirtualQuantity = function (EditEvent) {
        $scope.IsVisible = true;
        var getData = MyInventoryServices.getInventoryCountList(EditEvent.ob_tbl_inventory.id);
        getData.then(function (eve) {
            $scope.Event = eve.data;
            $scope.hdn_Item_id = $scope.Event[0].ob_tbl_inventory.id,
            $scope.t_ItemName = $scope.Event[0].ob_tbl_inventory.item_name,          
            $scope.Action = "Add";
            $scope.IsVisible = false;
            IsFormSubmitted = false;
        });
    }
   
    function ClearFields() {
        $scope.tbl_inventory_Id = "";
        $scope.item_total_count = "";
        $scope.from_warehouse_id = "";
        $scope.to_warehouse_id = "";
        $scope.Item_Transfer_Count = "";          
    }
  
    $scope.AddUpdateItem = function () {     
        var ItemTransferDetails = {};
         ItemTransferDetails = {
             tbl_inventory_Id: $scope.hdn_Item_id,
             Item_Transfer_Count: $scope.Item_Transfer_Count,
             item_total_count: $scope.item_total_count,
             to_warehouse_id: $scope.to_warehouse_id,
             from_warehouse_id: $scope.from_warehouse_id,
             t_ItemName: $scope.t_ItemName,
             Marketplace_id: $scope.Marketplace_id,        
        };
        var getAction = $scope.Action;
        var getData = MyInventoryServices.AddItemTransfer(ItemTransferDetails);
        getData.then(function (msg) {
            $scope.msg = msg.Message;
            $("#alertModal").modal('show');
            $scope.msg = msg.Message;
            ClearFields();
            //GetAllPONumber();
        }, function (error) {
            alert('Error!');
        });
    }, function (msg) {
        $("#alertModal").modal('show');
        $scope.msg = msg.Message;
    }

    $scope.AddVirtualQuantity = function () {      
        var VirtualDetails = {};     
        VirtualDetails = {
            id: $scope.hdn_Item_id,
            t_virtualItemCount: $scope.t_virtualItemCount,
        };
        var getAction = $scope.Action;
        var getData = MyInventoryServices.AddVirtualQuantity(VirtualDetails);
        getData.then(function (msg) {
            $scope.msg = msg.Message;
            $("#alertModal").modal('show');
            $scope.msg = msg.Message;
            VirtualClearFields();
            GetAllInventoryList();
        }, function (error) {
            alert('Error!');
        });
    }, function (msg) {
        $("#alertModal").modal('show');
        $scope.msg = msg.Message;
    }


    $scope.editEvent = function (EditEvent) {
        $scope.IsVisible = true;
        var getData = MyInventoryServices.getInventoryCountList(EditEvent.ob_tbl_inventory.id);
        getData.then(function (eve) {          
            $scope.Event = eve.data;
            $scope.id = $scope.Event[0].ob_tbl_inventory.id,
            $scope.t_ItemName = $scope.Event[0].ob_tbl_inventory.item_name,
            $scope.t_virtualItemCount = $scope.Event[0].ob_tbl_inventory.t_virtualItemCount                
            $scope.Action = "Edit";
            $("#myModal1").modal('show');
        },
        function (msg) {
            $("#alertModal").modal('show');
            $scope.msg = msg.data;
        });
    }

    function VirtualClearFields() {
        $scope.tbl_inventory_Id = "";
        $scope.t_virtualItemCount = "";        
    }


    ///------------------------- For sale POPUP----------------------------///////
    $scope.AddSales = function (colorPack) {
        $scope.IsVisible = true;            
        var getData = MyInventoryServices.InventorySalesDetailsByID(colorPack.ob_tbl_inventory.id);
        getData.then(function (emp) {           
            $scope.InventoryItemList = emp.data;          
            //$scope.ItemName = $scope.InventoryItemList[0].item_name;
            //$scope.Action = "Add";
            //$scope.IsVisible = false;
            //IsFormSubmitted = false;
        }, function (emp) {
            alert("Records gathering failed!");
        });
    }

    $scope.AddUpdateManageSales = function () {    
        $scope.InventoryItemList
        var ItemSKUDetails = {};   
        if ($scope.InventoryItemList.length > 0) {          
            angular.forEach($scope.InventoryItemList, function (item)
            {                      
                ItemSKUDetails = {
                   model_seller_code: item.model_seller_code,
                   m_marketplace_id: item.m_marketplace_id,
                   tbl_inventory_id: item.tbl_inventory_id,
                };            
                var getAction = $scope.Action;
                var getData = MyInventoryServices.AddSalesItemSKU($scope.InventoryItemList);
                getData.then(function (msg) {
                    $scope.msg = msg.Message;
                    $("#alertModal").modal('show');
                    $scope.msg = msg.Message;
                    //ClearFields();
                    //GetAllPONumber();
                    alert('Error!');
                });
            });
            }                   
    }, function (msg) {
        $("#alertModal").modal('show');
        $scope.msg = msg.Message;
    }
  
    /////////////////-------------------------------End_------------------------------//////
    //////////////////////////////////For Publish POPUP///////////////////////////////////// 
    $scope.AddPublish = function (colorPack) {
        $scope.IsVisible = true;       
        var getData = MyInventoryServices.InventoryPublishByID(colorPack.ob_tbl_inventory.id);
        getData.then(function (emp) {
            $scope.InventoryItemList = emp.data;
            $scope.ItemName = $scope.InventoryItemList[0].ItemName;
            $scope.total_count = $scope.InventoryItemList[0].total_count;        
            $scope.ImagePAth = $scope.InventoryItemList[0].ImagePAth;
        }, function (emp) {
            alert("Records gathering failed!");
        });
    }

    $scope.getTotal = function () {
        var total = 0;
        angular.forEach($scope.InventoryItemList, function (item) {            
            var product =parseInt(item.NewItem);
                total += product;
        });
        return total;
    }

    $scope.AddPublishItem = function () {       
        var PublishItemDetails = {};
        $scope.total_count = $('#totalvalue').text();     
        if ($scope.InventoryItemList.length > 0) {
            angular.forEach($scope.InventoryItemList, function (item) {               
                PublishItemDetails = {                  
                    CurrentItem: item.CurrentItem,
                    NewItem: item.NewItem,
                    m_marketplace_id: item.m_marketplace_id,
                    tbl_inventory_id: item.tbl_inventory_id,
                    total_count: $scope.total_count,
                    TotalItem: item.TotalItem,
                };               
                var getAction = $scope.Action;              
                var getData = MyInventoryServices.AddPublishItemDEt(PublishItemDetails);
                getData.then(function (msg) {
                    $scope.msg = msg.Message;
                    $("#alertModal").modal('show');
                    $scope.msg = msg.Message;
                    //ClearFields();
                    GetAllInventoryList();
                    alert('Error!');
                });
            });
        }
    }, function (msg) {
        $("#alertModal").modal('show');
        $scope.msg = msg.Message;
    }

    //////////////////////////////////End Publish POPUP//////////////////////////////////////

}).factory('MyInventoryServices', function ($http, $q) {
    var fac = {};   
    fac.GetColorForInventory = function () {
        return $http.get('/Seller/Seller/FillColor');
    }    
    fac.GetItemNameForTransfer = function () {
        return $http.get('/Seller/Seller/FillInventoryforItemName');
    }
    fac.GetWarehouseForTransfer = function () {
        return $http.get('/Seller/Seller/FillWareHouseForPurchase');
    }
    fac.getMarketPlaceList = function () {
        return $http.get("/Seller/Seller/FillMarketPlaceID");
    };
    fac.GetCategoryForInventory = function () {
        return $http.get('/Seller/Seller/FillCategory');
    }
    fac.getsubcategoryList = function (categoryid) {       
        return $http.get("/Seller/Seller/FillSubCategory?categoryid=" + categoryid);
    };  
    fac.GetInventoryList = function (categoryID, subcategoryID) {
        return $http.get('/Seller/Seller/GetMyInventory?categoryID=' + categoryID + '&subcategoryID=' + subcategoryID);
    }
    fac.UpdateInventory = function (id, selling_price, item_count) {      
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
    ////----------------------------------SAles SKU COde--------------
    fac.InventorySalesDetailsByID = function (ID) {
        return $http.get('/Seller/Seller/GetManageSaleAssociation?ID=' + ID);
    }

    fac.AddSalesItemSKU = function (ItemSKUDetails) {       
         var formData = new FormData();       
        //alert(ItemSKUDetails.model_seller_code);
         formData.append("Eve", ItemSKUDetails);             
        var defer = $q.defer();
        $http.post("/Seller/Seller/AddSAlesSKUCODE", formData,
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

    ////////////////////////////////////////END
    ////////////////////////////////////////////Publish Item Code/////////////////////////////////////
    fac.InventoryPublishByID = function (ID) {      
        return $http.get('/Seller/Seller/GetPublishItem?ID=' + ID);
    }
    fac.AddPublishItemDEt = function (PublishItemDetails) {       
        var formData = new FormData();                  
        formData.append("tbl_inventory_id", PublishItemDetails.tbl_inventory_id);
        formData.append("m_marketplace_id", PublishItemDetails.m_marketplace_id);
        formData.append("CurrentItem", PublishItemDetails.CurrentItem);
        formData.append("NewItem", PublishItemDetails.NewItem);
        formData.append("TotalItem", PublishItemDetails.TotalItem);
        formData.append("total_count", PublishItemDetails.total_count);
        var defer = $q.defer();
        $http.post("/Seller/Seller/AddPublishItem", formData,
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
    ///////////////////////////////////////////End Publish Code/////////////////////////////////////////
    fac.getInventoryCountList = function (id) {     
        return $http.get("/Seller/Seller/FillInventoryforItemCount?id=" + id);
    };

    fac.AddItemTransfer = function (ItemTransferDetails) {     
        var formData = new FormData();
        formData.append("tbl_inventory_Id", ItemTransferDetails.tbl_inventory_Id);
        formData.append("Item_Transfer_Count", ItemTransferDetails.Item_Transfer_Count);
        formData.append("item_total_count", ItemTransferDetails.item_total_count);
        formData.append("to_warehouse_id", ItemTransferDetails.to_warehouse_id);
        formData.append("from_warehouse_id", ItemTransferDetails.from_warehouse_id);
        formData.append("Marketplace_id", ItemTransferDetails.Marketplace_id);
        var defer = $q.defer();
        $http.post("/Seller/Seller/AddItemTransfertoWarehouse", formData,
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

    fac.AddVirtualQuantity = function (VirtualDetails) {
        var formData = new FormData();
        formData.append("id", VirtualDetails.id);
        formData.append("t_virtualItemCount", VirtualDetails.t_virtualItemCount);
        var defer = $q.defer();
        $http.post("/Seller/Seller/AddVirtualQuantity", formData,
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