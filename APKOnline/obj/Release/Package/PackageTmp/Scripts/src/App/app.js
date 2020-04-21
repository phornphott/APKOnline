var app = angular
    .module('ApkApp', [
        'ngAnimate',
        'ngResource',
        'ngSanitize',
        'dx',
        'ui.router',
        'ui.bootstrap',
        'oc.lazyLoad'
    ]);

angular.module('ApkApp').controller('IndexController', function ($scope, $rootScope, LoginService, $http) {
    $scope.logout = function () {
        localStorage.clear();
        localStorage.clear();
        window.location = 'Home/Login';
    }

    $scope.FirstName = localStorage.getItem('StaffFirstName');
    $scope.LastName = localStorage.getItem('StaffLastName');
    $scope.PictureFile = localStorage.getItem('StaffImage');
    $scope.DepartmentID = localStorage.getItem('StaffDepartmentID');

    LoginService.GlobalLogin();
});

angular.module('ApkApp').controller('DashboardController', function ($scope, $rootScope, LoginService, $http) {
    $scope.MONTHDATE = new Date();

    let getDaysInMonth = function (month, year) {
        return new Date(year, month, 0).getDate();
    };

    $scope.ListReportBudget = function () {
        $scope.STARTDATE = new Date($scope.MONTHDATE.getFullYear(), $scope.MONTHDATE.getMonth(), 1);
        $scope.ENDDATE = new Date($scope.MONTHDATE.getFullYear(), $scope.MONTHDATE.getMonth(), getDaysInMonth(($scope.MONTHDATE.getMonth() + 1), $scope.MONTHDATE.getFullYear()));
        $scope.MONTHS = String($scope.MONTHDATE.getMonth() + 1);

        $http.get("api/Report/ListReportBudget?STARTDATE=" + moment($scope.STARTDATE).format("YYYY-MM-DD") + "&ENDDATE=" + moment($scope.ENDDATE).format("YYYY-MM-DD") + "&MONTHS=" + $scope.MONTHS + "&StaffCode=" + $scope.StaffCode + "&DEPcode=" + $scope.DEPcode).then(function (data) {
            $scope.ListReportBudgets = data.data.Results.ReportBudget;
            $scope.chartOptions = {
                size: {
                    width: 500
                },
                palette: "bright",
                dataSource: $scope.ListReportBudgets,
                series: [
                    {
                        argumentField: "SumMonth",
                        valueField: "Document_NetSUM",
                        label: {
                            visible: true,
                            connector: {
                                visible: true,
                                width: 1
                            }
                        }
                    }
                ],
                onPointClick: function (e) {
                    var point = e.target;

                    toggleVisibility(point);
                },
                onLegendClick: function (e) {
                    var arg = e.target;

                    toggleVisibility(e.component.getAllSeries()[0].getPointsByArg(arg)[0]);
                }
            };
        });
    };
    
    

    function toggleVisibility(item) {
        if (item.isVisible()) {
            item.hide();
        } else {
            item.show();
        }
    }

    LoginService.GlobalLogin();
})

angular.module('ApkApp').service('LoginService', ['$http', function ($http) {
    this.GlobalLogin = function () {
        if (localStorage.getItem('StaffCode') === null || localStorage.getItem('StaffCode') === 'null') {
            localStorage.clear();
            localStorage.clear();
            window.location = 'Home/Login';
        }
    }
}]);