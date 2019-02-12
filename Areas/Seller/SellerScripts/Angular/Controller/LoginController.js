


angular.module('MyAppSeller') // extending from previously created angular module in the First Part
.controller('LoginController', function ($scope, LoginService) {
    $scope.IsLogedIn = false;
    $scope.Message = '';
    $scope.Submitted = false;
    $scope.IsFormValid = false;

    $scope.LoginData = {
        Email: '',
        Password: ''
    };

    //Check is Form Valid or Not // Here f1 is our form Name
    $scope.$watch('f1.$valid', function (newVal) {
        $scope.IsFormValid = newVal;
    });

    $scope.Login = function () {
        $scope.Submitted = true;
        if ($scope.IsFormValid) {
            LoginService.GetUser($scope.LoginData).then(function (d) {
                if (d.data.Email != null) {
                    $scope.IsLogedIn = true;
                    window.location.href = 'Seller/Seller/Index';
                    // $scope.Message = "Successfully login done. Welcome " + d.data.username;
                }
                else {
                    alert('Invalid Credential!');
                }
            });
        }
    };

})
.factory('LoginService', function ($http) {
    var fac = {};
    fac.GetUser = function (d) {
        return $http({
            url: 'Seller/SellerVendor/UserLogin',
            method: 'POST',
            data: JSON.stringify(d),
            headers: { 'content-type': 'application/json' }
        });
    };
    return fac;
});