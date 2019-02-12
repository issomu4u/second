angular.module('MyAppSeller').controller('SellerController', function ($scope, SellerServices, $http) {


    $scope.ActiveDashboard = true;
    $scope.Warning = false;
    $scope.WarningClose = function () {     
        $scope.Warning = true;
    }

    $scope.loading = true;
    $scope.itemSelected = "3";
    $scope.ChartHeading = "Last 3 days Orders";
    //Get dashboard data
    $http({
        method: 'POST',
        url: '/Seller/Seller/GetDashboardData',
        data: {}
    }).success(function (result) {
        $scope.DashboardData = result;
    });
    //Get dashboard top seller
    $http({
        method: 'POST',
        url: '/Seller/Seller/PartialTopSeller',
        dataType: 'html',
        data: {}
    }).success(function (result) {
        $("#DivTopSeller").html(result);
    });
    //Get dashboard top customer
    $http({
        method: 'POST',
        url: '/Seller/Seller/PartialTopCustomer',
        dataType: 'html',
        data: {}
    }).success(function (result) {
        $("#DivTopCustomer").html(result);
    });
    //Get dashboard top customer
    $http({
        method: 'POST',
        url: '/Seller/Seller/PartialTopReturn',
        dataType: 'html',
        data: {}
    }).success(function (result) {
        $("#DivTopReturn").html(result);
    });


    $scope.Bardata = [];// [60, 90, 120, 60, 90, 120, 60]
    $scope.Barlabels = [];//  ['2006', '2007', '2008', '2009', '2010', '2011', '2012'];//city array
    $scope.Barseries = ['Total Order'];
    $scope.GetLastOrders = function (days) {
        $scope.loading = true;
        $http({
            method: 'POST',
            url: '/Seller/Seller/GetLastOrder',
            data: { days: days }
        }).success(function (result) {
            if (result.length > 0) {
                for (var i = 0; i < result.length; i++) {
                    $scope.Barlabels.push(result[i].Date);
                    $scope.Bardata.push(result[i].TotalOrders);
                }
            }
            $scope.loading = false;
        });
    }
    $scope.GetLastOrders(3);
    $scope.change = function () {
        $scope.loading = true;
        if ($scope.itemSelected != "") {
            $scope.ChartHeading = "Last " + $scope.itemSelected + " days Orders";
            $scope.GetLastOrders($scope.itemSelected);
        }
        $scope.loading = false;


    };
    //Get Gross Chart
    var aDatasets1 = [];
    var aDatasets2 = [];
    $scope.GrossBarlabels = [];
    $scope.GrossBarseries = ['Sales', 'Returns'];
    $scope.loading = true;
    $http({
        method: 'POST',
        url: '/Seller/Seller/NewChart',
        data: {}
    }).success(function (chData) {
        var aData = chData;
        //alert("data");
        console.log("data1");
        console.log(chData);
        var aLabels = [];
        var aDatasets1 = [];
        var aDatasets2 = [];
        for (i = 0; i < chData.length; i++) {
            $scope.GrossBarlabels.push(chData[i].Date);
            aDatasets1.push(chData[i].TotalOrders);
            aDatasets2.push(chData[i].TotalReturn);
        }
        $scope.GrossBardata = [aDatasets1, aDatasets2];
        console.log("chart");
        console.log($scope.GrossBardata);
        $scope.loading = false;
    });

    //Net Realization Chart
    $scope.RealizationBardata = [];
    $scope.RealizationBarlabels = [];
    $scope.RealizationBarseries = ["Gross Sale Value", "Expenses", "Net Realization"];
    $scope.loading = true;
    $http({
        method: 'POST',
        url: '/Seller/Seller/GetNetRealization',
        data: {}
    }).success(function (chData) {
        var aDatasets1 = [];
        var aDatasets2 = [];
        var aDatasets3 = [];
        for (i = 0; i < chData.length; i++) {
            $scope.RealizationBarlabels.push(chData[i].Date);
            aDatasets1.push(chData[i].TotalAmount);
            aDatasets2.push(chData[i].TotalExpences);
            aDatasets3.push(chData[i].NetRealization);
        }
        $scope.RealizationBardata = [aDatasets1, aDatasets2, aDatasets3];
        $scope.loading = false;
    });

}).factory('SellerServices', function ($http, $q) {
    var fac = {};
    

    return fac;

});
