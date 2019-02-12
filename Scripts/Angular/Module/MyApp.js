(function () {
    //Create a Module 
    //var app = angular.module('MyApp', ['ngRoute']);  // Will use ['ng-Route'] when we will implement routing
    var app = angular.module('MyApp', ['angularUtils.directives.dirPagination', '720kb.datepicker', 'textAngular']);

    var app = angular.module('MyApp2', ['angularUtils.directives.dirPagination','textAngular']);
    var app = angular.module('EventApp', ['ngRoute', 'angularUtils.directives.dirPagination', 'textAngular']);

})();