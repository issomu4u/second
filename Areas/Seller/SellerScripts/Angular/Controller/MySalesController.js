angular.module('MyAppseller2').controller('MySalesController', function ($scope, $location, MySalesServices) {
    function getUrlParameter(name) {
        name = name.replace(/[\[]/, '\\[').replace(/[\]]/, '\\]');
        var regex = new RegExp('[\\?&]' + name + '=([^&#]*)');
        var results = regex.exec(location.search);
        return results === null ? '' : decodeURIComponent(results[1].replace(/\+/g, ' '));
    };
    $scope.loading = true;
    $scope.Message = "";
    $scope.IsFormSubmitted = false;
    $scope.IsFormValid = false;
    $scope.id = null;
    $scope.currentPage = 1;
    $scope.currentPage2 = 1;
    $scope.currentPage1 = 1;
    GetAllSalesList();
    $scope.IsHidden = true;
    $scope.item_serialNo = "";
    //$filter('date')(date, format, timezone);
    GetItemStatusList();
    GetStatusList();
    GetCourierList();
    //$scope.SalesDetailList = [];
    $scope.SalesCustomerDetailList = [];
    $scope.BindOrderHistoryDetails = [];
    $scope.BindSettlementDetails = [];
    $scope.inventoryId = $location.absUrl().split('=')[1];
    $scope.StatusList = [];
    $scope.n_sale_order_status = null;
    $scope.countries = [];
    $scope.SelectedCountries = null;
    $scope.Selected = [];
    $scope.SumOrder = "";
    $scope.SumFee = "";
    $scope.SumTaxFee = "";
    $scope.NetTotal = "";
    $scope.item_serialNo = "";


    $scope.uncheckAll = function () {
        for (var i = 0; i < $scope.SalesDetailList.length; i++) {
            var item = $scope.SalesDetailList[i];
            $scope.SalesDetailList[i].Selected = false;
        }
    };
    $scope.selected = {};
    $scope.checkAll = function () {
        var message = "";
        for (var i = 0; i < $scope.SalesDetailList.length; i++) {
            //var item = $scope.SalesDetailList[i];
            $scope.SalesDetailList[i].Selected = true;            
            //message += $scope.SalesDetailList[i].ob_tbl_sales_order_details.id + ";";
        }
        //alert(message);
    };
    $scope.GetValue = function () {
        var message = "";
        for (var i = 0; i < $scope.SalesDetailList.length; i++) {
            if ($scope.SalesDetailList[i].Selected) {
                var fruitId = $scope.SalesDetailList[i].ob_tbl_sales_order_details.id;
                var fruitName = $scope.SalesDetailList[i].ob_tbl_sales_order_details.product_name;               
                if (fruitId != null) {
                    var getData = MySalesServices.getorderinvoice(fruitId);
                }
            }
        }
        //alert(message);
    }
    $scope.pageChangeHandler = function (num) {
        console.log('meals page changed to ' + num);
    };

    function GetAllSalesList() {
        $scope.loading = true;       
        var getData = MySalesServices.GetSalesList();
        getData.then(function (emp) {
            $scope.SalesList = emp.data;
            angular.forEach($scope.SalesList, function (obj) {
                obj["showEdit"] = true;
                obj["showDelete"] = true;
            })
            $scope.loading = false;
        }, function (emp) {
            //alert("Records gathering failed!");
        });
    }


    $scope.BindSalesDetails = function () {       
        var getData = MySalesServices.getsalesdetailsList($scope.inventoryId);
        getData.then(function (emp) {
            $scope.SalesDetailList = emp.data;
            angular.forEach($scope.SalesDetailList, function (obj) {
                obj["showEdit"] = true;
                obj["showDelete"] = true;
            })           
        }, function (emp) {
            //alert("Records gathering failed!");
        });
    }

    $scope.BindCustomerDetails = function () {       
        var getData = MySalesServices.getsalescustomerList($scope.inventoryId);
        getData.then(function (emp) {
            $scope.SalesCustomerDetailList = emp.data;
            angular.forEach($scope.SalesCustomerDetailList, function (obj) {
                obj["showEdit"] = true;
                obj["showDelete"] = true;
            })            
        }, function (emp) {
            //alert("Records gathering failed!");
        });
    }

    $scope.BindSettlementDetails = function () {      
        var getData = MySalesServices.getsettlementList($scope.inventoryId);
        getData.then(function (emp) {
            $scope.SettlementDetailList = emp.data;
            $scope.SumOrder = $scope.SettlementDetailList[0].SumOrder;
            $scope.SumFee = $scope.SettlementDetailList[0].SumFee;
            $scope.SumTaxFee = $scope.SettlementDetailList[0].SumTaxFee;
            $scope.NetTotal = $scope.SettlementDetailList[0].NetTotal;
            angular.forEach($scope.SettlementDetailList, function (obj) {
                obj["showEdit"] = true;
                obj["showDelete"] = true;
            })           
        }, function (emp) {
            //alert("Records gathering failed!");
        });
    }


   
    $scope.ViewExpense = function () {
        $scope.loading = false;
        $scope.IsVisible = true;
        var getdata = $scope.SettlementDetailList;

        $scope.SettlementDetailList1 = getdata;
        angular.forEach($scope.SettlementDetailList1, function (obj) {
            //alert("gg");
            //console.log("ee");
            //console.log($scope.SettlementDetailList1);
        })
        $scope.loading = false;
    }, function (emp) {
        alert("Records gathering failed!");

    }
    //$scope.BindOrderHistoryDetails = function () {     
    //    var getData = MySalesServices.getorderhistoryList($scope.inventoryId);
    //    getData.then(function (emp) {
    //        $scope.OrderHistoryList = emp.data;
    //        angular.forEach($scope.OrderHistoryList, function (obj) {
    //            obj["showEdit"] = true;
    //            obj["showDelete"] = true;
    //        })
    //    }, function (emp) {
    //        alert("Records gathering failed!");
    //    });
    //}

    $scope.BindSalesDetails($scope.inventoryId)
    $scope.BindCustomerDetails($scope.inventoryId)
    $scope.BindSettlementDetails($scope.inventoryId)
    //$scope.BindOrderHistoryDetails($scope.inventoryId)


    function GetItemStatusList() {
        var getData = MySalesServices.GetItemStatus();
        getData.then(function (eve) {
            $scope.id = eve.data.id;
            $scope.StatusList = eve.data;
        }, function (eve) {
            //alert("Records gathering failed!");
        });
    }

    //$scope.toggleCancelEdit = function (emp) {
    //       emp.showEdit = true;
    //       eve.showDelete = true;
    //   }



    //$scope.toggleEdit = function (emp) {
    //    emp.showEdit = emp.showEdit ? false : true;
    //    if (emp.showEdit) {
    //        MyInventoryServices.UpdateInventory(emp.ob_tbl_inventory.id, emp.ob_tbl_inventory.selling_price, emp.ob_tbl_inventory.item_count).then(function (d) {
    //            alert(d.Message);
    //            ClearForm();

    //        }, function (e) {
    //            alert(e);
    //        });
    //    }
    //    else {

    //    }
    //}


    $scope.toggleEdit = function (emp) {
        emp.showEdit = emp.showEdit ? false : true;       
        if (emp.showEdit) {
            MySalesServices.UpdateSalesStatus(emp.ob_tbl_sales_order_details.id, emp.ob_tbl_sales_order_details.n_order_status_id).then(function (d) {
                //alert(d.Message);             
            }, function (e) {
                alert(e);
            });
            GetAllSalesList();
        }
        else {

        }
    }

    $scope.toggleCancelEdit = function (emp) {
        emp.showEdit = true;
        eve.showDelete = true;
    }



    $scope.$watch("f1.$valid", function (isValid) {
        $scope.IsFormValid = isValid;
    });



    $scope.UpdateSalesStatus = function (EditEvent) {
        $scope.IsVisible = true;
        var getData = MySalesServices.getsalesorderList(EditEvent.id);
        var date = "";
        getData.then(function (eve) {
            $scope.Event = eve.data;
            $scope.hdn_Item_id = $scope.Event[0].ob_tbl_sales_order.id,
           date = new Date();
            $scope.dispatch_date = date.getFullYear() + '-' + ('0' + (date.getMonth() + 1)).slice(-2) + '-' + ('0' + date.getDate()).slice(-2);           
            $scope.Action = "Add";
            $scope.IsVisible = false;
            IsFormSubmitted = false;
        });
    }

    $scope.UpdateSalesStatus2 = function (EditEvent) {
        $scope.IsVisible = true;      
        var getData = MySalesServices.getsalesorderList2(EditEvent.ob_tbl_sales_order_details.id);
        var date = "";
        getData.then(function (eve) {
            $scope.Event = eve.data;
            $scope.hdn_Item_id = $scope.Event[0].ob_tbl_sales_order.id,
            $scope.hdn_orderItem_id = $scope.Event[0].ob_tbl_sales_order_details.id,
            $scope.hdn_fullfilled_id = $scope.Event[0].ob_tbl_sales_order.n_fullfilled_id,           
            $scope.batch_no = $scope.Event[0].ob_tbl_inventory_details.batch_no,
            $scope.item_quantity = $scope.Event[0].ob_tbl_inventory_details.item_Quantity,
            date = new Date();
            $scope.dispatch_date = date.getFullYear() + '-' + ('0' + (date.getMonth() + 1)).slice(-2) + '-' + ('0' + date.getDate()).slice(-2);         
            $scope.Action = "Add";
            $scope.IsVisible = false;
            IsFormSubmitted = false;
        });
    }

    $scope.CallItemSerial = function (EditEvent) {
        $scope.IsVisible = true;
        var getData = MySalesServices.getorderitemList(EditEvent.ob_tbl_sales_order_details.id);
        getData.then(function (eve) {
            $scope.id = eve.data.id;
            $scope.ItemSerailList = eve.data;
            console.log("k");
            console.log($scope.ItemSerailList);
        }, function (eve) {
            //alert("Records gathering failed!");
        });
    }

    function GetStatusList() {
        var getData = MySalesServices.GetItemStatus();
        getData.then(function (eve) {
            $scope.id = eve.data.id;
            $scope.StatusList = eve.data;
        }, function (eve) {
            //alert("Records gathering failed!");
        });
    }

   

    function GetCourierList() {
        var getData = MySalesServices.GetCourierStatus();
        getData.then(function (eve) {
            $scope.id = eve.data.id;
            $scope.CourierList = eve.data;
        }, function (eve) {
            //alert("Records gathering failed!");
        });
    }

    $scope.AddUpdateItem = function () {
        var ItemStatusDetails = {};
        $scope.dispatch_date = $('#dispatch_date').val();
        ItemStatusDetails = {
            tbl_sales_order_id: $scope.hdn_Item_id,
            n_courier_id: $scope.n_courier_id,
            n_sale_order_status: $scope.n_sale_order_status,
            t_awb_number: $scope.t_awb_number,
            t_Remarks: $scope.t_Remarks,
            dispatch_date: $scope.dispatch_date,
            tbl_sale_orderdetails_id: $scope.hdn_orderItem_id,
            t_shipping_price: $scope.t_shipping_price,
            t_shipping_tax: $scope.t_shipping_tax,
        };
        var getAction = $scope.Action;
        var getData = MySalesServices.Addsalesstatus(ItemStatusDetails);
        getData.then(function (msg) {
            $scope.msg = msg.Message;
            $("#alertModal").modal('show');
            $scope.msg = msg.Message;
            ClearFields();
            GetAllSalesList();
        }, function (error) {
            alert('Error!');
        });
    }, function (msg) {
        $("#alertModal").modal('show');
        $scope.msg = msg.Message;
    }

    $scope.AddUpdateSalesDetailsItem = function () {
        var ItemStatusDetails = {};
        //alert("hhii");
        $scope.item_serialNo = $('#item_serialNo').val();
        alert($scope.item_serialNo);
        $scope.dispatch_date = $('#dispatch_date').val();
        ItemStatusDetails = {
         
            tbl_sales_order_id: $scope.hdn_Item_id,
            n_courier_id: $scope.n_courier_id,
            n_sale_order_status: $scope.n_sale_order_status,
            t_awb_number: $scope.t_awb_number,
            t_Remarks: $scope.t_Remarks,
            dispatch_date: $scope.dispatch_date,
            tbl_sale_orderdetails_id: $scope.hdn_orderItem_id,
            t_shipping_price: $scope.t_shipping_price,
            t_shipping_tax: $scope.t_shipping_tax,          
            item_serialNo: $scope.item_serialNo,
        };
        var getAction = $scope.Action;
        var getData = MySalesServices.Addsalesdetailsstatus(ItemStatusDetails);
        getData.then(function (msg) {
            $scope.msg = msg.Message;
            $("#alertModal").modal('show');
            $scope.msg = msg.Message;
            ClearFields();
            GetAllSalesList();
        }, function (error) {
            alert('Error!');
        });
    }, function (msg) {
        $("#alertModal").modal('show');
        $scope.msg = msg.Message;
    }
    function ClearFields() {
        $scope.tbl_sales_order_id = "";
        $scope.n_courier_id = "";
        $scope.n_sale_order_status = "";
        $scope.t_awb_number = "";
        $scope.t_Remarks = "";
        $scope.dispatch_date = "";
    }

}).factory('MySalesServices', function ($http, $q) {
    var fac = {};

    fac.GetSalesList = function () {
       
        return $http.get('/Seller/Sales/GetSaleDetails');
    }

    fac.getsalesdetailsList = function (id) {
        return $http.get("/Seller/Sales/GetSaleMappingDetails?id=" + id);
    };

    fac.getsalescustomerList = function (id) {
        return $http.get("/Seller/Sales/GetSaleCustomerDetails?id=" + id);
    };
    fac.getsettlementList = function (id) {
        return $http.get("/Seller/Sales/GetOrderSettlementDetails?id=" + id);
    };
    //fac.getorderhistoryList = function (id) {    
    //    return $http.get("/Seller/Sales/GetOrderHistoryDetails?id=" + id);
    //};

    fac.getorderitemList = function (id) {    
        return $http.get("/Seller/Sales/FillItemSerailNo?orderdetailid=" + id);
    };

    fac.GetItemStatus = function () {
        return $http.get('/Seller/Sales/FillSalesStatus');
    }

    fac.GetCourierStatus = function () {
        return $http.get('/Seller/Sales/FillCourierName');
    }


    fac.UpdateSalesStatus = function (id, n_order_status_id) {
        var formData = new FormData();
        formData.append("id", id);
        formData.append("n_order_status_id", n_order_status_id);
        var defer = $q.defer();
        $http.post("/Seller/Sales/updateorderstatus", formData,
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

    fac.getsalesorderList = function (id) {
        return $http.get("/Seller/Sales/FillStatusorder?id=" + id);
    };

    fac.getsalesorderList2 = function (orderdetailid) {
        return $http.get("/Seller/Sales/FillStatusorder2?orderdetailid=" + orderdetailid);
    };
    fac.getorderinvoice = function (id) {
        //alert(id);
        return $http.get("/Seller/Sales/OrderInvoicePrint?id=" + id);
    };

    fac.Addsalesstatus = function (ItemStatusDetails) {     
        var formData = new FormData();
        formData.append("tbl_sales_order_id", ItemStatusDetails.tbl_sales_order_id);
        formData.append("n_courier_id", ItemStatusDetails.n_courier_id);
        formData.append("n_sale_order_status", ItemStatusDetails.n_sale_order_status);
        formData.append("t_awb_number", ItemStatusDetails.t_awb_number);
        formData.append("t_Remarks", ItemStatusDetails.t_Remarks);
        formData.append("dispatch_date", ItemStatusDetails.dispatch_date);
        formData.append("tbl_sale_orderdetails_id", ItemStatusDetails.tbl_sale_orderdetails_id);
        formData.append("t_shipping_price", ItemStatusDetails.t_shipping_price);
        formData.append("t_shipping_tax", ItemStatusDetails.t_shipping_tax);
      
        var defer = $q.defer();
        $http.post("/Seller/Sales/UpdateSalesOrderByStatus", formData,
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


    fac.Addsalesdetailsstatus = function (ItemStatusDetails) {      
        var formData = new FormData();
        formData.append("tbl_sales_order_id", ItemStatusDetails.tbl_sales_order_id);
        formData.append("n_courier_id", ItemStatusDetails.n_courier_id);
        formData.append("n_sale_order_status", ItemStatusDetails.n_sale_order_status);
        formData.append("t_awb_number", ItemStatusDetails.t_awb_number);
        formData.append("t_Remarks", ItemStatusDetails.t_Remarks);
        formData.append("dispatch_date", ItemStatusDetails.dispatch_date);
        formData.append("tbl_sale_orderdetails_id", ItemStatusDetails.tbl_sale_orderdetails_id);
        formData.append("t_shipping_price", ItemStatusDetails.t_shipping_price);
        formData.append("t_shipping_tax", ItemStatusDetails.t_shipping_tax);
        formData.append("item_serialNo", ItemStatusDetails.item_serialNo);
        var defer = $q.defer();
        $http.post("/Seller/Sales/UpdateSalesDetailsOrderByStatus", formData,
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