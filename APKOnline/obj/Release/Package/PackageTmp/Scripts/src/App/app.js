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
    $scope.ShowSumdept = 1;
    $scope.ListReportBudget = function () {
        $scope.STARTDATE = new Date($scope.MONTHDATE.getFullYear(), $scope.MONTHDATE.getMonth(), 1);
        $scope.ENDDATE = new Date($scope.MONTHDATE.getFullYear(), $scope.MONTHDATE.getMonth(), getDaysInMonth(($scope.MONTHDATE.getMonth() + 1), $scope.MONTHDATE.getFullYear()));
        $scope.MONTHS = String($scope.MONTHDATE.getMonth() + 1);

        $http.get("api/Report/DashBroad").then(function (data) {

            $scope.ListReportBudgets = data.data.Results.DepAmount;
            $scope.piechartOptions = {
                size: {
                    width: 100 + '%'
                },
                AutoWidth: true,
                palette: "bright",
                dataSource: $scope.ListReportBudgets,
                series: [
                    {
                        argumentField: "DEP",
                        valueField: "Amount",
                        label: {
                            visible: true,
                            connector: {
                                visible: true,
                                width: 1,
                                format: {
                                    type: 'currency'
                                }
                            }
                        }
                    }
                ],
                title: "Sum Amount by Department",
                "export": {
                    enabled: true
                },
                onPointClick: function (e) {

                    $scope.ShowSumdept = 0;
                    //var point = e.target;
                    console.log(e.target.argument);


                    $http.get("api/Report/DashBroadByDepartment?Dep=" + e.target.argument).then(function (data) {

                        $scope.ListReportBudgets = data.data.Results.DepAmount;
                        $scope.ListReportPOPR = data.data.Results.PRPOData;
                        var SeriesName = data.data.Results.Department[0].Name;
                        $scope.chartOptions = {
                            size: {
                                width: 100 + '%'
                            },

                            palette: "soft",
                            dataSource: $scope.ListReportBudgets,

                            displayMode: 'stagger',
                            series: {
                                argumentField: "date",
                                valueField: "Amount",
                                name: SeriesName,
                                type: "bar",
                                ignoreEmptyPoints: true,

                            },
                            title: "Sum Amount Department by Date",
                            "export": {
                                enabled: true
                            },
                            legend: {
                                verticalAlignment: "bottom",
                                horizontalAlignment: "center"
                            },
                            onPointClick: function (e) {


                            },

                        };
                        $scope.chart1Options = {
                            size: {
                                width: 100 + '%'
                            },

                            palette: "soft",
                            dataSource: $scope.ListReportPOPR,
                            commonSeriesSettings: {
                                argumentField: "Document_Vnos",
                                type: "bar",
                                hoverMode: "allArgumentPoints",
                                selectionMode: "allArgumentPoints",
                                label: {
                                    visible: true,
                                    format: {
                                        type: "fixedPoint",
                                        precision: 2
                                    }
                                }
                            },
                            series: [
                                { valueField: "POAmount", name: "PO Amount" },
                                { valueField: "PRAmount", name: "PR Amount" },

                            ],
                            title: "Sum Amount Department by Date",
                            "export": {
                                enabled: true
                            },
                            legend: {
                                verticalAlignment: "bottom",
                                horizontalAlignment: "center"
                            },

                        };
                    });

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