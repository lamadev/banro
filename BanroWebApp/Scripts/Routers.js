app.config(function ($routeProvider) {
    $routeProvider
                    .when('/employed', { templateUrl: '/Home/ViewEmployed', controller: 'CtrlHome' })
                    .when('/children', { templateUrl: '/Home/ViewChildren', controller: 'CtrlHome' })
                    .when('/partner', { templateUrl: '/Home/ViewPartner', controller: 'CtrlHome' })
                    .when('/visitor', { templateUrl:'/Home/ViewVisitor', controller: 'CtrlHome' })
                    .when('/employedcommand', { templateUrl: '/Home/ViewEmployedCommand', controller: 'CtrlHome' })
                    .when('/partnercommand', { templateUrl: '/Home/ViewEmployedCommand', controller: 'CtrlHome' })
                    .when('/childcommand', { templateUrl: '/Home/ViewEmployedCommand', controller: 'CtrlHome' })
                    .when('/visitorcommand', { templateUrl: '/Home/ViewEmployedCommand', controller: 'CtrlHome' })
                    .when('/printablecommand', { templateUrl: '/Home/ViewPrintableCommand', controller: 'CtrlHome' })
                    .otherwise({ redirectTo: '/' });

});
console.log("Route.js run");