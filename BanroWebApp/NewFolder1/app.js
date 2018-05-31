

// Declare app level module which depends on filters, and services
angular.module('ngdemo', ['ngRoute', 'ngdemo.controllers']).
    config(['$routeProvider', function ($routeProvider) {
        $routeProvider.when('/date-picker', {templateUrl: 'datepicker.html', controller: 'MixedContentController'});
        $routeProvider.otherwise({redirectTo: '/date-picker'});
    }]); 