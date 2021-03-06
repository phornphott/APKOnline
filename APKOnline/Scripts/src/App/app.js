﻿var app = angular
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
    console.log('DashboardController');
    $scope.MONTHDATE = new Date();

    let getDaysInMonth = function (month, year) {
        return new Date(year, month, 0).getDate();
    };
    $scope.ShowSumdept = 1;
    $scope.ListReportBudget = function () {
        $scope.STARTDATE = new Date($scope.MONTHDATE.getFullYear(), $scope.MONTHDATE.getMonth(), 1);
        $scope.ENDDATE = new Date($scope.MONTHDATE.getFullYear(), $scope.MONTHDATE.getMonth(), getDaysInMonth(($scope.MONTHDATE.getMonth() + 1), $scope.MONTHDATE.getFullYear()));
        $scope.MONTHS = String($scope.MONTHDATE.getMonth() + 1);

        $http.get("api/Report/DashBroad/" + localStorage.getItem("StaffID") + "?").then(function (data) {
            console.log(data);
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

angular.module('ApkApp').directive('ngFileModel', ['$parse', function ($parse) {
    return {
        restrict: 'A',
        link: function (scope, element, attrs) {
            var model = $parse(attrs.ngFileModel);
            var isMultiple = attrs.multiple;
            var modelSetter = model.assign;
            element.bind('change', function () {
                var values = [];
                angular.forEach(element[0].files, function (item) {
                    var value = {
                        // File Name 
                        name: item.name,
                        //File Size 
                        size: item.size,
                        //File URL to view 
                        url: URL.createObjectURL(item),
                        // File Input Value 
                        _file: item
                    };
                    values.push(value);
                });
                scope.$apply(function () {
                    if (isMultiple) {
                        modelSetter(scope, values);
                    } else {
                        modelSetter(scope, values[0]);
                    }
                });
            });
        }
    };
}]);
angular.module('ApkApp').directive('uploadFiles', function () {
    return {
        scope: true,        //create a new scope  
        link: function (scope, el, attrs) {
            el.bind('change', function (event) {
                var files = event.target.files;
                //iterate files since 'multiple' may be specified on the element  
                for (var i = 0; i < files.length; i++) {
                    //emit event upward  
                    scope.$emit("seletedFile", { file: files[i] });
                }
            });
        }
    };
});
angular.module('ApkApp').directive('ngFiles', ['$parse', function ($parse) {

    function fn_link(scope, element, attrs) {
        var onChange = $parse(attrs.ngFiles);
        element.on('change', function (event) {
            onChange(scope, { $files: event.target.files });
        });
    };

    return {
        link: fn_link
    }
}])
