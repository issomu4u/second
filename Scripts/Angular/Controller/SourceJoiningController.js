/// <reference path="../Module/MyApp.js" />


angular.module('MyApp2').controller('SourceJoiningController', function ($scope, SourceJoiningService) {
    // Variables
    $scope.SourceJoiningList = "";
    $scope.currentPage = 1;
    $scope.SourceName = "";

    $scope.pageChangeHandler = function (num) {
        console.log('meals page changed to ' + num);
    };

    GetAllSourceJoining();

    function GetAllSourceJoining() {

        var getData = SourceJoiningService.GetSourceJoiningList();
        getData.then(function (emp) {
            $scope.SourceJoiningList = emp.data;

            angular.forEach($scope.SourceJoiningList, function (obj) {
                obj["showEdit"] = true;
                obj["showDelete"] = true;
            })

        }, function (emp) {
            alert("Records gathering failed!");
        });
    }


    $scope.SaveSourceJoining = function () {
        alert();
        var getData = SourceJoiningService.SaveSourceJoining($scope.SourceName);
       //GetAllSourceJoining();
        ClearForm();
        $("#alertModal").modal('show');
        $scope.msg = "Source Of Joining saved Successfully !";
    }

    $scope.toggleEdit = function (emp) {

        //find the value to upadate the table details in database
        emp.showEdit = emp.showEdit ? false : true;
        if (emp.showEdit) {
           
            var getData = SourceJoiningService.UpdateSourceJoining(emp.id, emp.source);
            //alert(getData);
           //GetAllSourceJoining();
            $scope.msg = "Source Of Joining  updated Successfully !";
            ClearForm();
            $("#alertModal").modal('show');
            

        }
        else {
            // emp.id = emp.ob_m_partner_type.id;

        }

    }

    //cancel or delete
    $scope.toggleCancelEdit = function (eve) {
        eve.showEdit = true;
        eve.showDelete = true;
    }

    $scope.toggleDeleteEdit = function (eve) {
        var getData = SourceJoiningService.DeleteSourceJoining(eve.id);
        ClearForm();
        $("#alertModal").modal('show');
        
        $scope.msg = "Source Of Joining  deleted Successfully !";
        GetAllSourceJoining();
    }


    $scope.alertmsg = function () {
        $("#alertModal").modal('hide');
    };

    //Form Validation
    $scope.$watch("f1.$valid", function (isValid) {
        $scope.IsFormValid = isValid;
    });
    function ClearForm() {
        $scope.SourceName = "";
        $scope.IsFormSubmitted = false;
        $scope.f1.$setPristine();
    }

})
.factory('SourceJoiningService', function ($http, $q) {
    var fac = {};
    fac.SaveSourceJoining = function (source) {

        fac = $http({
            method: "post",
            url: "/Home/SaveSourceJoining",
            params: {
                source: source
            }
        });

    }

    fac.UpdateSourceJoining = function (getId, source) {
        alert(getId);
        alert(source);
        var Indata = { 'id': getId, 'source': source };
        fac = $http({
            method: "post",
            url: "/Home/UpdateSourceJoining",
            params: Indata
        });

    }

    fac.DeleteSourceJoining = function (source) {
        fac = $http({
            method: "post",
            url: "/Home/DeleteSourceJoining",
            params: {
                id: source
            }
        });

    }

    fac.GetSourceJoiningList = function () {
        return $http.get('/Home/GetSourceJoiningList');
    }
    return fac;

});

