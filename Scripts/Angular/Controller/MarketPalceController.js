

angular.module('MyApp2').controller('MarketPalceController', function ($scope, FileUploadService) {
    // Variables

    $scope.Message = "";
    $scope.FileInvalidMessage = "";
    $scope.SelectedFileForUpload = null;
    $scope.name = "";
    $scope.api_url = "";
    $scope.IsFormSubmitted = false;
    $scope.IsFileValid = false;
    $scope.IsFormValid = false;
    //$scope.id = null;
  
    $scope.MarketPlace = null;
    $scope.currentPage = 1;
    GetAllMarketPlaceList();
    $scope.pageChangeHandler = function (num) {
        console.log('meals page changed to ' + num);
    };

 

    function GetAllMarketPlaceList() {

        var getData = FileUploadService.GetMarketPlaceList();
        getData.then(function (emp) {
            $scope.MarketPlace = emp.data;

            angular.forEach($scope.MarketPlace, function (obj) {
                obj["showEdit"] = true;
                obj["showDelete"] = true;
            })

        }, function (emp) {
            alert("Records gathering failed!");
        });
    }


    

    
    //cancel or delete
    $scope.toggleCancelEdit = function (emp) {
        emp.showEdit = true;
    }


    $scope.toggleDeleteEdit = function (partner) {
        var getData = FileUploadService.DeleteMarketPlace(partner.id);
        getData.then(function (eve) {
            GetAllMarketPlaceList();
        },
        function (msg) {
            //$("#alertModal").modal('show');
            alert(msg.data);
        });
    }

    $scope.toggleEdit = function (emp) {
        emp.showEdit = emp.showEdit ? false : true;
        if (emp.showEdit) {
            var MarketPlaceDetails = {};
         
            MarketPlaceDetails = {
                id: emp.id,
               
                name: emp.name,
                api_url: emp.api_url,
            };
           
            $scope.ChechFileValid($scope.SelectedFileForUpload);
            if ($scope.IsFileValid) {
                MarketPlaceDetails.file = $scope.SelectedFileForUpload
            }
            else {
            }
            var getData = FileUploadService.UpdateMarketDetails(MarketPlaceDetails);
            getData.then(function (msg) {
                GetAllMarketPlaceList();
              
                $scope.msg = msg.Message;
                ClearForm();
            }, function (msg) {
             
                $scope.msg = msg.Message;
            });

        }
        else {
            // emp.id = emp.ob_m_partner_type.id;

        }

    }

    $scope.$watch("f1.$valid", function (isValid) {
        $scope.IsFormValid = isValid;
    });
   
    try {

        $scope.ChechFileValid = function (file) {
            var isValid = false;
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
                $scope.FileInvalidMessage = "Image required!";
            }
            $scope.IsFileValid = isValid;
        };
    }
    catch (e) {
        alert("error block called !!");
    }


    //File Select event 
    $scope.selectFileforUpload = function (file) {
        $scope.SelectedFileForUpload = file[0];
    }
    //----------------------------------------------------------------------------------------

    //Save File
    //$scope.SaveMarketPlaceDetails = function () {
    //    alert("Add");
    //    $scope.IsFormSubmitted = true;
    //    $scope.Message = "";
    //    $scope.ChechFileValid($scope.SelectedFileForUpload);
    //    if ($scope.IsFormValid && $scope.IsFileValid) {
    //        FileUploadService.UploadFile($scope.SelectedFileForUpload, $scope.name, $scope.api_url).then(function (d) {
    //            ClearForm();
    //            alert(d.Message);

    //        }, function (e) {
    //            alert(e);
    //        });
    //    }
    //    else {
    //        $scope.Message = "All the fields are required.";
    //    }
    //};

    $scope.SaveMarketPlaceDetails = function () {

        $scope.errorflag = true;
        $scope.IsFormSubmitted = true;
        var MarketPlaceDetails = {};        
        $scope.ChechFileValid($scope.SelectedFileForUpload);
       
        if ($scope.IsFileValid && $scope.IsFormValid) {
            MarketPlaceDetails = {
                name: $scope.name,
                api_url: $scope.api_url,           
                file: $scope.SelectedFileForUpload,            
            };

            var getData = FileUploadService.UploadFile(MarketPlaceDetails);
            getData.then(function (msg) {
                GetAllMarketPlaceList();              
                $scope.msg = msg.Message;
                ClearForm();

            }, function (msg) {            
                $scope.msg = msg.Message;
            });

        }
        else {
            $scope.Message = "All the fields are required.";
        }

    }








    //Clear form 
    function ClearForm() {
        $scope.name = "";
        $scope.api_url = "";
        $scope.id = 0;
        angular.forEach(angular.element("input[type='file']"), function (inputElem) {
            angular.element(inputElem).val(null);
        });     
        FileUploadService.GetMarketPlaceList().then(function (d) {
            $scope.MarketPlace = d.data; //Success callback
            angular.forEach($scope.MarketPlace, function (obj) {
                obj["showEdit"] = true;
                obj["showDelete"] = true;
            })
        }, function (error) {
            alert('Error!' + error); // Failed Callback
        });

        // $scope.f1.$setPristine();
        $scope.IsFormSubmitted = false;
        $scope.f1.$setPristine();
    }

})
.factory('FileUploadService', function ($http, $q) {
    var fac = {};
    //fac.UploadFile = function (file, name, api_url) {
    fac.UploadFile = function (MarketPlaceDetails) {
      alert("Add2")
        var formData = new FormData();    
        formData.append("file", MarketPlaceDetails.file);
        formData.append("name", MarketPlaceDetails.name);
        formData.append("api_url", MarketPlaceDetails.api_url);
        var defer = $q.defer();
        $http.post("/Home/AddMarketPlace", formData,
            {
                withCredentials: true,
                headers: { 'Content-Type': undefined },
                transformRequest: angular.identity
            })
        .success(function (d) {
            defer.resolve(d);
        })
        .error(function () {
            defer.reject("File Upload Failed!");
        });

        return defer.promise;

    }

    fac.UpdateMarketDetails = function (MarketPlaceDetails) {
        var formData = new FormData();
        formData.append("id", MarketPlaceDetails.id);
        formData.append("file", MarketPlaceDetails.file);
        formData.append("name", MarketPlaceDetails.name);
        formData.append("api_url", MarketPlaceDetails.api_url);
       
        var defer = $q.defer();
        $http.post("/Home/UpdateMarketPlace", formData,
            {
                withCredentials: true,
                headers: { 'Content-Type': undefined },
                transformRequest: angular.identity
            })
        .success(function (d) {
            defer.resolve(d);
        })
        .error(function () {
            defer.reject("File Upload Failed!");
        });
        return defer.promise;
    }

    fac.DeleteMarketPlace = function (MarketPlaceId) {
        var response = $http({
            method: "post",
            url: "/Home/DeleteMarketPlaceDetail",
            params: {
                id: JSON.stringify(MarketPlaceId)
            }
        });

        return response;
    }

    fac.GetMarketPlaceList = function () {
        return $http.get('/Home/GetAllMarketPlaceList');
    }
    return fac;

});

