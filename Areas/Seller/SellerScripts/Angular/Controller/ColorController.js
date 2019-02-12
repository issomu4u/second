angular.module('MyAppSeller').controller('ColorController', function ($scope, ColorServices) {
    //$scope.dataTableOpt = {
    //    //custom datatable options 
    //    // or load data through ajax call also
    //    "aLengthMenu": [[10, 50, 100, -1], [10, 50, 100, 'All']],
    //};
    $scope.Message = "";
    $scope.color_code = "";
    $scope.color_name = "";

    $scope.IsFormSubmitted = false;
    $scope.IsFormValid = false;
    $scope.id = null;
    $scope.currentPage = 1;
 


    GetAllColorList();

    $scope.pageChangeHandler = function (num) {
        console.log('meals page changed to ' + num);
    };



    function GetAllColorList() {

        var getData = ColorServices.GetColorList();
        getData.then(function (emp) {
            $scope.ColorList = emp.data;

            angular.forEach($scope.ColorList, function (obj) {
                obj["showEdit"] = true;
                obj["showDelete"] = true;
            })

        }, function (emp) {
            alert("Records gathering failed!");
        });
    }


    $scope.toggleEdit = function (color) {

        //find the value to upadate the table details in database
        color.showEdit = color.showEdit ? false : true;
        if (color.showEdit) {
            //console.log(workSeller);
            //console.log("pata karna ha");
            alert("1")
            var ColorDetails = {};
            ColorDetails = {
                id: color.ob_m_color.id,
                color_name: color.ob_m_color.color_name,
                color_code: color.ob_m_color.color_code,
            };

            var getData = ColorServices.UpdateColor(ColorDetails);
            getData.then(function (msg) {

                $("#alertModal").modal('show');
                $scope.msg = msg.data;
                ClearForm();

            }, function (msg) {
                $("#alertModal").modal('show');
                $scope.msg = msg.data;
            });

            GetAllColorList();
        }
        else {
            // emp.id = emp.ob_m_partner_type.id;

        }

    }

    $scope.toggleCancelEdit = function (emp) {
        emp.showEdit = true;
        eve.showDelete = true;
    }

    $scope.toggleDeleteEdit = function (colorPack) {

        var getData = ColorServices.DeleteColorByID(colorPack.ob_m_color.id);
        getData.then(function (eve) {
            GetAllColorList();
        },
        function (msg) {
            $("#alertModal").modal('show');
            alert(msg.data);
        });
    }

    $scope.$watch("f1.$valid", function (isValid) {
        $scope.IsFormValid = isValid;
    });

    $scope.SaveColorDetails = function () {
        $scope.IsFormSubmitted = true;
       
        $scope.Message = "";
        if ($scope.IsFormValid) {
            ColorServices.SaveColorDetails($scope.color_name, $scope.color_code).then(function (d) {
                
                    ClearForm();
                    //GetAllColorList();
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
        $scope.color_code = "";
        $scope.color_name = "";
        $scope.IsFormSubmitted = false;
        $scope.f1.$setPristine();

        ColorServices.GetColorList().then(function (d) {
            //console.log(d);
            $scope.ColorList = d.data;
            angular.forEach($scope.ColorList, function (obj) {
                obj["showEdit"] = true;
                obj["showDelete"] = true;
            })
        }, function (error) {
            alert('Error!' + error);
        });


    }




}).factory('ColorServices', function ($http, $q) {
    var fac = {};
    fac.SaveColorDetails = function (color_name, color_code) {

        var formData = new FormData();
        formData.append("color_name", color_name);
        formData.append("color_code", color_code);
        var defer = $q.defer();
        $http.post("/Seller/Seller/SaveColorDetails", formData,
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


    fac.GetColorList = function () {
        return $http.get('/Seller/Seller/GetColor');
    }


    fac.UpdateColor = function (ColorDetails) {
        alert("2")
        var response = $http({
            method: "post",
            url: "/Seller/Seller/UpdateColorDetails",
            data: JSON.stringify(ColorDetails),
            dataType: "json"
        });
        return response;
    }

    fac.DeleteColorByID = function (colorId) {
        var response = $http({
            method: "post",
            url: "/Seller/Seller/DeleteColor",
            params: {
                id: JSON.stringify(colorId)
            }
        });

        return response;
    }
    return fac;

});