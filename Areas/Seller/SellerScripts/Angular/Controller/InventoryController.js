angular.module('MyAppseller2').controller('InventoryController', function ($scope, InventoryServices) {

    $('#txt_effectivedate').datepicker();

    
    $scope.Message = "";
    $scope.brand = "";    
   // $scope.hsn_code = "";
    $scope.item_code = "";
    $scope.item_description = "";
    $scope.item_name = "";
    $scope.item_dimension = "";
   // $scope.remarks = "";
    $scope.packed_dimension = "";
    $scope.sku = "";
    //$scope.item_count = "";
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
    $scope.t_effectiveBought_price = "";
    $scope.lead_time_to_ship = "";
    $scope.n_fullfilled_id = "";
    $scope.tbl_details_item_id = "";
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
    $scope.tbl_item_subcategory_id = 0;
    $scope.showImages = false;
    $scope.buttonTxt = "Add";
    $scope.hdn_AddUpdate_id = "0";
    $scope.Customers = [];
    GetColorList();  
    GetAllInventoryList();
    GetFullfilledList();
    GetItemDetailsList();
    $scope.EffectivePrice = [];
    $scope.status = 0;
    $scope.hdn_date = null;
    $scope.Effective_price = '';
    $scope.Item_Tax = '';
    $scope.Selected = {};
    $scope.Selected = [];
   

    
    

    function GetColorList() {
        var getData = InventoryServices.GetColorForInventory();
        getData.then(function (eve) {
            $scope.id = eve.data.id;
            $scope.ColorList = eve.data;
        }, function (eve) {
           // alert("Records gathering failed!");
        });
    }
    $scope.GetValue = function (tbl_item_category_id) {

        var message = "";
        for (var i = 0; i < $scope.InventoryList.length; i++) {
            if ($scope.InventoryList[i].Selected) {
                var fruitId = $scope.InventoryList[i].ob_tbl_inventory.id;
                var fruitName = $scope.InventoryList[i].ob_tbl_inventory.product_name;
                if (fruitId != null) {

                    var getData = InventoryServices.getItemList(fruitId, tbl_item_category_id);

                    getData.then(function (eve) {
                        GetAllInventoryList();
                        $scope.selectedAll = false;
                        InventoryServices.GetCategoryForInventory().then(function (d) {
                            $scope.id = d.data.id;
                            $scope.CategoryList = d.data;
                        }, function (error) {
                            //alert('Error!');
                        });
                    })

                }
            }
        }
    }
    function GetItemDetailsList() {
        var getData = InventoryServices.GetItemDetails();
        getData.then(function (eve) {
            $scope.id = eve.data.id;
            $scope.ItemDetailsList = eve.data;
        }, function (eve) {
            //alert("Records gathering failed!");
        });
    }


    function GetFullfilledList() {
       
        var getData = InventoryServices.GetFullfileedForInventory();
        getData.then(function (eve) {
            $scope.id = eve.data.id;
            $scope.FullfilledList = eve.data;
        }, function (eve) {
            //alert("Records gathering failed!");
        });
    }
    InventoryServices.GetCategoryForInventory().then(function (d) {
        $scope.id = d.data.id;
        $scope.CategoryList = d.data;

    }, function (error) {
        alert('Error!');
    });


    $scope.GetSubCategory = function (categoryid) {
        $scope.tbl_item_subcategory_id = categoryid;
        var getData = InventoryServices.getsubcategoryList(categoryid);
        getData.then(function (eve) {
            $scope.subcategorylist = eve.data;
        }, function (eve) {
            //alert("Records gathering failed!");
        });
    }

    $scope.pageChangeHandler = function (num) {
        console.log('meals page changed to ' + num);
    };



    function GetAllInventoryList() {
        var getData = InventoryServices.GetInventoryList();
        getData.then(function (emp) {
            $scope.InventoryList = emp.data;
            angular.forEach($scope.InventoryList, function (obj) {
                obj["showEdit"] = true;
                obj["showDelete"] = true;
            })
        }, function (emp) {
            //alert("Records gathering failed!");
        });
    }


 

    $scope.toggleCancelEdit = function (emp) {
        emp.showEdit = true;
        eve.showDelete = true;
    }



    $scope.toggleEdit = function (emp) {

        $scope.showImages = true;
        $scope.hdn_AddUpdate_id = emp.ob_tbl_inventory.id;
        $scope.brand = emp.ob_tbl_inventory.brand;
        $scope.brand = emp.ob_tbl_inventory.brand;
        $scope.item_code = emp.ob_tbl_inventory.item_code;
        $scope.item_description = emp.ob_tbl_inventory.item_description;
        $scope.item_dimension = emp.ob_tbl_inventory.item_dimension;
        $scope.item_name = emp.ob_tbl_inventory.item_name;
        $scope.packed_dimension = emp.ob_tbl_inventory.packed_dimension;
        $scope.sku = emp.ob_tbl_inventory.sku;     
        $scope.mrp = emp.ob_tbl_inventory.mrp;
        $scope.packed_weight = emp.ob_tbl_inventory.packed_weight;
        $scope.t_effectiveBought_price = emp.ob_tbl_inventory.t_effectiveBought_price;       
        $scope.item_weight = emp.ob_tbl_inventory.item_weight;
        $scope.transfer_price = emp.ob_tbl_inventory.transfer_price;

        $scope.lead_time_to_ship = emp.ob_tbl_inventory.lead_time_to_ship;
        $scope.n_fullfilled_id = emp.ob_tbl_inventory.n_fullfilled_id;
        $scope.tbl_details_item_id = emp.ob_tbl_inventory.tbl_details_item_id;

        $scope.item_photo1_path = emp.ob_tbl_inventory.item_photo1_path;
        $scope.item_photo2_path = emp.ob_tbl_inventory.item_photo2_path;
        $scope.item_photo3_path = emp.ob_tbl_inventory.item_photo3_path;

        $scope.m_item_color_id = emp.ob_tbl_inventory.m_item_color_id;
        $scope.tbl_item_category_id = emp.ob_tbl_inventory.tbl_item_category_id;
        $scope.tbl_item_subcategory_id = emp.ob_tbl_inventory.tbl_item_subcategory_id;
        $scope.status = 1;
        //alert("gg" + $scope.status);
        var itemId = $scope.tbl_item_category_id;
        $scope.buttonTxt = "Update";
        $scope.GetSubCategory(itemId);

    }


    $scope.toggleDeleteEdit = function (sellerPack) {

        var getData = InventoryServices.DeleteInventoryByID(sellerPack.ob_tbl_inventory.id);
        getData.then(function (eve) {
            GetAllInventoryList();
        },
        function (msg) {
            $("#alertModal").modal('show');
            alert(msg.data);
        });
    }

    //File Select event 
    $scope.selectFileforUpload = function (file) {
        $scope.SelectedFileForUpload = file[0];
    }
    $scope.selectFileforUpload1 = function (file) {
        $scope.SelectedFileForUpload1 = file[0];
    }
    $scope.selectFileforUpload2 = function (file) {
        $scope.SelectedFileForUpload2 = file[0];
    }


    $scope.$watch("f1.$valid", function (isValid) {
        $scope.IsFormValid = isValid;
    });
    try {

        $scope.ChechFileValid = function (file) {
            var isValid = false;
            //alert("file"+$scope.SelectedFileForUpload)
            if ($scope.SelectedFileForUpload != null) {
                if ((file.type == 'image/png' || file.type == 'image/jpeg' || file.type == 'image/gif')) {
                    $scope.FileInvalidMessage = "";
                    isValid = true;
                }
                else {
                    alert("file is not valid");
                    $scope.FileInvalidMessage = "Selected file is Invalid. (only file type png, jpeg and gif)";
                }
            }
            else {
                //$scope.FileInvalidMessage = "Image required!";
            }
            $scope.IsFileValid = isValid;
        };
    }
    catch (e) {
        alert("error block called !!");
    }

    try {

        $scope.ChechFileValid1 = function (file) {
            var isValid = false;
            //console.log("Print log");
            //console.log(file);
            if ($scope.SelectFileForUpload1 != null) {
                if ((file.type == 'image/png' || file.type == 'image/jpeg' || file.type == 'image/gif')) {
                    $scope.FileInvalidMessage1 = "";
                    isValid = true;
                }
                else {
                    alert("file is not valid");
                    $scope.FileInvalidMessage1 = "Selected file is Invalid. (only file type png, jpeg and gif)";
                }
            }
            else {
               // $scope.FileInvalidMessage1 = "Image required!";
            }
            $scope.IsFileValid = isValid;
        };
    }
    catch (e) {
        alert("error block called !!");
    }

    try {

        $scope.ChechFileValid2 = function (file) {
            var isValid = false;
            if ($scope.SelectFileForUpload2 != null) {
                if ((file.type == 'image/png' || file.type == 'image/jpeg' || file.type == 'image/gif')) {
                    $scope.FileInvalidMessage2 = "";
                    isValid = true;
                }
                else {
                    alert("file is not valid");
                    $scope.FileInvalidMessage2 = "Selected file is Invalid. (only file type png, jpeg and gif)";
                }
            }
            else {
               // $scope.FileInvalidMessage2 = "Image required!";
            }
            $scope.IsFileValid = isValid;
        };
    }
    catch (e) {
        alert("error block called !!");
    }

    

    $scope.SaveInventoryDetails = function () {
        $scope.errorflag = true;
        $scope.IsFormSubmitted = true;
        $scope.IsFormValid = true;
        var InventoryDetails = {};     
        //alert($scope.SelectedFileForUpload);
        //alert($scope.SelectedFileForUpload1);
        //alert($scope.SelectedFileForUpload2);
        $scope.ChechFileValid($scope.SelectedFileForUpload);
        $scope.ChechFileValid1($scope.SelectedFileForUpload1);       
        $scope.ChechFileValid2($scope.SelectedFileForUpload2);
             
        if ($scope.IsFormValid) {          
            InventoryDetails = {             
                id:$scope.hdn_AddUpdate_id,
                brand: $scope.brand,           
                item_code: $scope.item_code,
                item_description: $scope.item_description,
                item_dimension: $scope.item_dimension,
                item_name: $scope.item_name,             
                packed_dimension: $scope.packed_dimension,
                sku: $scope.sku,
                file: $scope.SelectedFileForUpload,
                file1: $scope.SelectedFileForUpload1,
                file2: $scope.SelectedFileForUpload2,           
                mrp: $scope.mrp,
                packed_weight: $scope.packed_weight,           
                t_effectiveBought_price: $scope.t_effectiveBought_price,
                m_item_color_id: $scope.m_item_color_id,              
                item_dimension: $scope.item_dimension,
                tbl_item_category_id: $scope.tbl_item_category_id,
                tbl_item_subcategory_id: $scope.tbl_item_subcategory_id,
                item_weight: $scope.item_weight,
                transfer_price: $scope.transfer_price,
                lead_time_to_ship: $scope.lead_time_to_ship,
                n_fullfilled_id: $scope.n_fullfilled_id,
                tbl_details_item_id: $scope.tbl_details_item_id,
            };
            //Pricedetails:                   
            if ($scope.hdn_AddUpdate_id == "0") {
                var getData = InventoryServices.SaveInventoryDetails(InventoryDetails, $scope.EffectivePrice);
                getData.then(function (msg) {
                    GetAllInventoryList();
                    $scope.msg = msg.Message;
                    ClearForm();
                }, function (msg) {
                    $scope.msg = msg.Message;
                });
            }
            else {               
                var getData = InventoryServices.UpdateInventory(InventoryDetails, $scope.EffectivePrice);
                getData.then(function (msg) {
                    GetAllInventoryList();
                    $scope.msg = msg.Message;
                    ClearForm();
                }, function (msg) {
                    $scope.msg = msg.Message;
                });
            }           
        }
        else {
            $scope.Message = "All the fields are required.";            
        }
    }


    $scope.ResetForm = function () {
        $scope.buttonTxt = "Add";
        $scope.hdn_AddUpdate_id = "0";
        ClearForm();
        GetAllInventoryList();
    }

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
       // $scope.item_count = "";
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
        $scope.t_effectiveBought_price = "";
        $scope.lead_time_to_ship = "";
        $scope.n_fullfilled_id = "";
        $scope.tbl_details_item_id = "";
        $scope.showImages = false;
        $('#txt_effectivedate').datepicker();
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
            $scope.InventoryList = d.data;
            angular.forEach($scope.InventoryList, function (obj) {
                obj["showEdit"] = true;
                obj["showDelete"] = true;
            })
        }, function (error) {
            alert('Error!' + error);
        });
    }

   
    $scope.AddCategoryClick = function () {
        var CategoryDetails = {};
        CategoryDetails = {
            category_name: $scope.category_name,
            hsn_code: $scope.hsn_code,

        };     
        $.ajax({
            type: "POST",
            url: "/Seller/Seller/AddCategory",          
            data: JSON.stringify({ category_slabs: $scope.Customers, Eve: CategoryDetails }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",         
            success: function (result) {
                alert('success');
            },
            error: function (result) {
                alert('error');
            }
        });

        ClearCategoryFields();
        $scope.Action = "Add";
        $scope.IsVisible = false;
        IsFormSubmitted = false;
      
        // $scope.refresh();
    }

    $scope.Customers = [];
 
    $scope.Add = function () {       
        var customer = {};
        customer.index = $scope.Customers.length;
        customer.from_rs = $scope.txt_from;
        customer.to_rs = $scope.txt_to;
        customer.tax_rate = $scope.taxrate;
        $scope.Customers.push(customer);           
        $scope.txt_from = "";
        $scope.txt_to = "";
        $scope.taxrate = "";
    };
    $scope.CategoryUpdate = function (index) {
        //alert("cat update");
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
    $scope.UpdateCategorySlabs = function (index) {
        //alert("pop_up");
        $scope.CustomersCopy = [];
        angular.forEach($scope.lst_slabs, function (obj) {
            if (obj.index == index) {
                var customer = {};
                customer.index = index;
                customer.from_rs = $('#txt_from' + index).val();
                customer.to_rs = $('#txt_to' + index).val();
                customer.tax_rate = $('#txt_taxrate' + index).val();
                $scope.CustomersCopy.push(customer);
                //console.log(customer);
            }
            else
                $scope.CustomersCopy.push(obj);
        })
        $scope.lst_slabs = $scope.CustomersCopy;
        //console.log('final');
        //console.log($scope.lst_slabs);
    };


    
    $scope.Edit = function (index) {      
        var ID = index;
        $('#txt_from' + ID).prop('disabled', false);
        $('#txt_taxrate' + ID).prop('disabled', false);
        $('#txt_to' + ID).prop('disabled', false);      
    }
    $scope.category_name = "";
    $scope.hsn_code = "";

    $scope.ViewTax = function (tbl_item_category_id)
    {      
        $scope.IsVisible = true;
        var getData = InventoryServices.get_categoryslabs(tbl_item_category_id);
        getData.then(function (eve) {
            
            $scope.Event = eve.data;
            $scope.category_name = $scope.Event[0].category_name,
            $scope.hsn_code = $scope.Event[0].hsn_code,
            $scope.list_slabs = $scope.Event[0].obj_partial_slabs;
            console.log("list");
            console.log($scope.list_slabs);
            angular.forEach($scope.list_slabs, function (obj) {
                   })
            $scope.Action = "View";
            $scope.IsVisible = false;
            IsFormSubmitted = false;
        });
    }
 
 //   $scope.UpdatePrice = function (Update_id) {
 //       $scope.IsVisible = true;
 //       var getData = InventoryServices.get_pricelabs(Update_id);
 //       getData.then(function (eve) {
 //           $scope.EffectivePrice = eve.data;

 //           console.log("dtaatat");
 //           console.log($scope.EffectivePrice);
 //           var value = $scope.EffectivePrice[0].Effecive_date[$scope.EffectivePrice.length - 1];              
 //       angular.forEach($scope.EffectivePrice, function (obj) {
 //       })
 //      $scope.Action = "View";
 //      $scope.IsVisible = false;
 //       IsFormSubmitted = false;
 //    });
 //}
    $scope.UpdatePrice = function (Update_id) {
        $scope.IsVisible = true;
        var getData = InventoryServices.get_pricelabs(Update_id);
        getData.then(function (eve) {
            $scope.EffectivePrice = eve.data;
            var arrLength = $scope.EffectivePrice.length;
            var arrValue = $scope.EffectivePrice[arrLength - 1].Effecive_date;
            //alert(arrValue);
            // arrValue = "11/20/2019";
            var a = arrValue.split('/');
            //var s = $('#txt_effectivedate').val('');
            //alert(s);
            //var changeValue = a[1] + "/" + a[0] + "/" + a[2];
            var changeValue = a[1] + "/" + a[0] + "/" + a[2];
            $('#txt_effectivedate').datepicker({
                minDate: new Date(changeValue)
            })                     
            //console.log("list");
            //console.log($scope.EffectivePrice);
            angular.forEach($scope.EffectivePrice, function (obj) {
            })
            $scope.Action = "View";
            $scope.IsVisible = false;
            IsFormSubmitted = false;
            $('#myModal5').modal();

        });
    }

    //$scope.AddEffective = function () {     
    //    $('#txt_effectivedate').datepicker();       
    //    $('#myModal5').modal();
    //}

    $scope.Remove = function (index) {     
        var name = $scope.Customers[index].Name;     
        {        
            $scope.Customers.splice(index, 1);
        }
    }

    $scope.checkAll = function () {
        var message = "";
        if ($scope.selectedAll) {
            $scope.selectedAll = true;
            for (var i = 0; i < $scope.InventoryList.length; i++) {
                $scope.InventoryList[i].Selected = true;
            }
        }
        else {
            $scope.selectedAll = false;
            for (var i = 0; i < $scope.InventoryList.length; i++) {
                $scope.InventoryList[i].Selected = false;
            }
        }
    };
    
    $scope.Addsum = function (Effective_price, Item_Tax)
    {
        if (Effective_price == null || Effective_price == '') {
            Effective_price = 0;
        }
        if (Item_Tax == null || Item_Tax == '') {
            Item_Tax = 0;
        }       
        $scope.Gross_price = (parseInt(Effective_price) + parseInt(Item_Tax));
        return $scope.Gross_price;
    }



    $scope.EditPrice = function (index) {
        var ID = index;
        $('#Effecive_date' + ID).prop('disabled', false);
        $('#Effective_price' + ID).prop('disabled', false);
        $('#Item_Tax' + ID).prop('disabled', false);
        $('#Gross_price' + ID).prop('disabled', false);

        $(function () {
            $('#Effecive_date' + ID).datepicker();
        });
    }
    $scope.PriceUpdate = function (index) {     
        $scope.priceCopy = [];
        angular.forEach($scope.EffectivePrice, function (obj) {
            if (obj.index == index) {
                var effectiveprice = {};
                effectiveprice.index = index;
                effectiveprice.Effecive_date = $('#Effecive_date' + index).val();
                effectiveprice.Effective_price = $('#Effective_price' + index).val();
                effectiveprice.Item_Tax = $('#Item_Tax' + index).val();
                effectiveprice.Gross_price = $('#Gross_price' + index).val();
                $scope.priceCopy.push(effectiveprice);
            }
            else
                $scope.priceCopy.push(obj);
        })
        $scope.EffectivePrice = $scope.priceCopy;
    };
    $scope.AddPrice = function () {
        //alert("Add");
        var txtdate = $('#txt_effectivedate').val();
        var price = $scope.Effective_price;
        //alert("date" + price);
        if (txtdate != '' && price != '') {
            var effectiveprice = {};
            effectiveprice.index = $scope.EffectivePrice.length;

            //$scope.hdn_date
            //alert($('#txt_effectivedate').val());
            effectiveprice.Effecive_date = ($('#txt_effectivedate').val());
            effectiveprice.Effective_price = $scope.Effective_price;
            effectiveprice.Item_Tax = $scope.Item_Tax;
            effectiveprice.Gross_price = $scope.Gross_price;
            $scope.EffectivePrice.push(effectiveprice);
            //console.log("Testing");
            //console.log(effectiveprice);
            $scope.Effecive_date = "";
            $('#txt_effectivedate').val('')
            $scope.Effective_price = "";
            $scope.Item_Tax = "";
            $scope.Gross_price = "";
        }
        else {
            return
           
            //alert("test");
            $scope.msg = "Fill date";
        }
        
    };
    $scope.ClearformClick = function () {

        //$scope.EffectivePrice = "";
        //$scope.Effecive_date = "";
        $('#txt_effectivedate').val();
        $scope.Effective_price = "";
        $scope.Item_Tax = "";
        $scope.Gross_price = "";
    }
    $scope.RemovePrice = function (index) {
        var name = $scope.EffectivePrice[index].Name;
        {
            $scope.EffectivePrice.splice(index, 1);
        }
    }
    function ClearCategoryFields() {
        $scope.category_name = "";
        $scope.tax_rate = "";             
    }


    $scope.AddUpdatCategory1 = function () {
        var CategoryDetails = {};
        CategoryDetails = {
            category_name: $scope.category_name,
            hsn_code: $scope.hsn_code,

        };
        var getAction = $scope.Action;
            var getData = InventoryServices.AddCategory(CategoryDetails);
            getData.then(function (msg) {              
                $scope.msg = msg.Message;
                $("#alertModal").modal('show');
                $scope.msg = msg.Message;              
                ClearCategoryFields();
                InventoryServices.GetCategoryForInventory().then(function (d) {
                    $scope.id = d.data.id;
                    $scope.CategoryList = d.data;
                }, function (error) {
                    alert('Error!');
                });
            }, function (msg) {
                $("#alertModal").modal('show');
                $scope.msg = msg.Message;
            });            
    }


    $scope.alertmsg = function () {
        $("#alertModal").modal('hide');
    };


    function ClearSubCategoryFields() {
        $scope.tbl_item_category_id = "";
        $scope.hsn_code = "";
        $scope.tax_rate = "";
        $scope.subcategory_name = "";
    }


    $scope.AddSubCategory = function () {
        ClearSubCategoryFields();
        $scope.Action = "Add";
        $scope.IsVisible = false;
        IsFormSubmitted = false;
        // $scope.refresh();
    }

    $scope.SubCategory = function () {

        var SubCategoryDetails = {};
        SubCategoryDetails = {
            subcategory_name: $scope.subcategory_name,
            hsn_code: $scope.hsn_code,
            tax_rate: $scope.tax_rate,
            hsn_code: $scope.hsn_code,
            tbl_item_category_id: $scope.tbl_item_category_id,
        };
        var getAction = $scope.Action;
        var getData = InventoryServices.AddSubCategory(SubCategoryDetails);
        getData.then(function (msg) {
            // GetAllEvents();
            $scope.msg = msg.Message;
            $("#alertModal").modal('show');
            $scope.msg = msg.Message;
            // $("#myModal").modal('toggle');
            ClearSubCategoryFields();

        }, function (msg) {
            $("#alertModal").modal('show');
            $scope.msg = msg.Message;
        });

    }


}).factory('InventoryServices', function ($http, $q) {
    var fac = {};
    fac.SaveInventoryDetails = function (InventoryDetails, Pricedetails) {
       
       // var formData = new FormData();
       // formData.append("brand", InventoryDetails.brand);
       // formData.append("hsn_code", InventoryDetails.hsn_code);
       // formData.append("item_code", InventoryDetails.item_code);
       // formData.append("item_description", InventoryDetails.item_description);
        
       // formData.append("item_dimension", InventoryDetails.item_dimension);
       // formData.append("item_name", InventoryDetails.item_name);
       // formData.append("remarks", InventoryDetails.remarks);
       // formData.append("packed_dimension", InventoryDetails.packed_dimension);
       // formData.append("sku", InventoryDetails.sku);
       // formData.append("file", InventoryDetails.file);
       // formData.append("file1", InventoryDetails.file1);
       // formData.append("file2", InventoryDetails.file2);
       //// formData.append("item_count", InventoryDetails.item_count);
       // formData.append("mrp", InventoryDetails.mrp);

       // formData.append("packed_weight", InventoryDetails.packed_weight);
       // formData.append("t_effectiveBought_price", InventoryDetails.t_effectiveBought_price);
       // //formData.append("selling_price", InventoryDetails.selling_price);
       // formData.append("m_item_color_id", InventoryDetails.m_item_color_id);
       // formData.append("tbl_item_category_id", InventoryDetails.tbl_item_category_id);
       // formData.append("tbl_item_subcategory_id", InventoryDetails.tbl_item_subcategory_id);
       // formData.append("item_weight", InventoryDetails.item_weight);
       // formData.append("transfer_price", InventoryDetails.transfer_price);
       // formData.append("lead_time_to_ship", InventoryDetails.lead_time_to_ship);
       // formData.append("n_fullfilled_id", InventoryDetails.n_fullfilled_id);
       // formData.append("tbl_details_item_id", InventoryDetails.tbl_details_item_id);
       // formData.append("price_slabs", Pricedetails);


        var defer = $q.defer();
        $http({
                method: "post",
                url: "/Seller/Seller/SaveInventoryDetails",
                data: JSON.stringify({ Res: InventoryDetails, price_slabs: Pricedetails })
            })
        //$http.post("/Seller/Seller/SaveInventoryDetails", formData,
        //    {
        //        withCredentials: true,
        //        headers: { 'Content-Type': undefined },
        //        transformRequest: angular.identity
        //    })
        .success(function (d) {
            defer.resolve(d);
        })
        .error(function () {
            defer.reject("some error occred !!");
        });
        return defer.promise;
    }

    


    fac.AddCategory = function (CategoryDetails,categoryslabs) {       
        var formData = new FormData();
        formData.append("category_name", CategoryDetails.category_name);
        formData.append("hsn_code", CategoryDetails.hsn_code);
        formData.append("category_slabs", categoryslabs)
        //var response = $http({
        //    method: "post",
        //    url: "/Seller/Seller/AddCategory",
        //    params: {
        //        category_slabs: categoryslabs
        //    }
        //});
        //return response;
        var defer = $q.defer();
        $http.post("/Seller/Seller/AddCategory", formData,
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

    fac.AddSubCategory = function (SubCategoryDetails) {       
        var formData = new FormData();
        formData.append("subcategory_name", SubCategoryDetails.subcategory_name);
        formData.append("hsn_code", SubCategoryDetails.hsn_code);
        formData.append("tax_rate", SubCategoryDetails.tax_rate);
        formData.append("tbl_item_category_id", SubCategoryDetails.tbl_item_category_id);
        var defer = $q.defer();
        $http.post("/Seller/Seller/AddSubCategory", formData,
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

    fac.GetColorForInventory = function () {
        return $http.get('/Seller/Seller/FillColor');
    }

    fac.GetItemDetails = function () {
        return $http.get('/Seller/Seller/FillItemDetailsBy');
    }

    fac.GetFullfileedForInventory = function () {
        return $http.get('/Seller/Seller/FillFullfilled');
    }

    fac.GetCategoryForInventory = function () {
        return $http.get('/Seller/Seller/FillCategory');
    }

    fac.getsubcategoryList = function (categoryid) {      
        return $http.get("/Seller/Seller/FillSubCategory?categoryid=" + categoryid);
    };

    fac.get_categoryslabs = function (id) {      
        return $http.get("/Seller/Seller/ViewTaxDetails?id=" + id);
    };

    fac.get_pricelabs = function (id) {
       // alert("message" + id)
        return $http.get("/Seller/Seller/ViewPriceSlabDetails?id=" + id);
    };

    fac.GetInventoryList = function () {
        return $http.get('/Seller/Seller/GetInventory');
    }


    fac.UpdateInventory = function (InventoryDetails, Pricedetails) {
       
        //var formData = new FormData();
        //formData.append("id", InventoryDetails.id);
        //formData.append("brand", InventoryDetails.brand);     
        //formData.append("item_code", InventoryDetails.item_code);
        //formData.append("item_description", InventoryDetails.item_description);
        //formData.append("item_dimension", InventoryDetails.item_dimension);
        //formData.append("item_name", InventoryDetails.item_name);        
        //formData.append("packed_dimension", InventoryDetails.packed_dimension);
        //formData.append("sku", InventoryDetails.sku);
        //formData.append("file", InventoryDetails.file);
        //formData.append("file1", InventoryDetails.file1);
        //formData.append("file2", InventoryDetails.file2);
        //formData.append("mrp", InventoryDetails.mrp);
        //formData.append("packed_weight", InventoryDetails.packed_weight);
        //formData.append("t_effectiveBought_price", InventoryDetails.t_effectiveBought_price);     
        //formData.append("m_item_color_id", InventoryDetails.m_item_color_id);
        //formData.append("tbl_item_category_id", InventoryDetails.tbl_item_category_id);
        //formData.append("tbl_item_subcategory_id", InventoryDetails.tbl_item_subcategory_id);
        //formData.append("item_weight", InventoryDetails.item_weight);
        //formData.append("transfer_price", InventoryDetails.transfer_price);
        //formData.append("lead_time_to_ship", InventoryDetails.lead_time_to_ship);
        //formData.append("n_fullfilled_id", InventoryDetails.n_fullfilled_id);
        //formData.append("tbl_details_item_id", InventoryDetails.tbl_details_item_id);

        var defer = $q.defer();
        $http({
            method: "post",
            url: "/Seller/Seller/UpdateInventoryDetails",
            data: JSON.stringify({ objtbl_inventory: InventoryDetails, price_slabs: Pricedetails })
        })
        //var defer = $q.defer();
        //$http.post("/Seller/Seller/UpdateInventoryDetails", formData,
        //    {
        //        withCredentials: true,
        //        headers: { 'Content-Type': undefined },
        //        transformRequest: angular.identity
        //    })
        .success(function (d) {
            defer.resolve(d);
        })
        .error(function () {
            defer.reject("some error occred !!");
        });

        return defer.promise;
    }

    fac.DeleteInventoryByID = function (purchaseId) {
        var response = $http({
            method: "post",
            url: "/Seller/Seller/DeleteInventory",
            params: {
                id: JSON.stringify(purchaseId)
            }
        });

        return response;
    }

    fac.getItemList = function (id, tbl_item_category_id) {
        //alert(id);
        return $http.get("/Seller/Seller/SelectedItem?id=" + id + '&category_id=' + tbl_item_category_id);
    };
    return fac;

});