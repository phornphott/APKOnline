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

    $scope.messages = [];
    $scope.data = [];
    $scope.NotiNumber = 0;
    $scope.Noti1 = 0;
    $scope.Noti2 = 0;
    $scope.Noti3 = 0;
    $scope.Noti4 = 0;
    $scope.Noti5 = 0;
    $scope.NotiHeaderText = '';
    $scope.Notitext_1 = '';
    $scope.Notitext_2 = '';
    $scope.Notitext_3 = '';
    $scope.Notitext_4 = '';
    $scope.Notitext_5 = '';
    $scope.Loadingdata = false;
    $scope.LoadNotification = function () {

        $scope.Loadingdata = true;

        $http.post("api/Staffs/GetNotiPR/" + localStorage.getItem("StaffID") + "?").then(function (data) {
            $scope.data = data;
            console.log($scope.data);

            $scope.Noti1 = $scope.data.data.Results;


        });
        $http.post("api/Staffs/GetNotiPROver/" + localStorage.getItem("StaffID") + "?").then(function (data) {
            $scope.data = data;
            console.log($scope.data);

            $scope.Noti2 = $scope.data.data.Results;


        });
        $http.post("api/Staffs/GetNotiPOCreate/" + localStorage.getItem("StaffDepartmentID") + "?").then(function (data) {
            $scope.data = data;
            console.log($scope.data);

            $scope.Noti3 = $scope.data.data.Results;


        });
        
        $http.post("api/Staffs/GetNotiPreview/" + localStorage.getItem("StaffID") + "?").then(function (data) {
            $scope.data = data;
            console.log($scope.data);

            $scope.Noti4 = $scope.data.data.Results;


        });
        $http.post("api/Staffs/GetNotiPO/" + localStorage.getItem("StaffID") + "?").then(function (data) {
            $scope.data = data;
            console.log($scope.data);

            $scope.Noti5 = $scope.data.data.Results;


        });

        $scope.Loadingdata = false;
    }

    $scope.FirstName = localStorage.getItem('StaffFirstName');
    $scope.LastName = localStorage.getItem('StaffLastName');
    $scope.PictureFile = localStorage.getItem('StaffImage');
    $scope.DepartmentID = localStorage.getItem('StaffDepartmentID');
    $scope.DepartmentName = localStorage.getItem('StaffDepartmentName');
    $scope.StaffLevel = localStorage.getItem('StaffLevel');

    $scope.LoadNotification();
    setInterval(function () {
        $scope.LoadNotification();
    }, 120000)

    setInterval(function () {
        console.log($scope.Loadingdata);
        if ($scope.Loadingdata == false) {
            $scope.NotiNumber = $scope.Noti1 + $scope.Noti2 + $scope.Noti3 + $scope.Noti4 + $scope.Noti5;
            console.log($scope.NotiNumber);
            $scope.NotiShow = '';
            if ($scope.NotiNumber > 0) {
                $scope.NotiShow = $scope.NotiNumber;
            }
            $scope.NotiHeaderText = 'You Have ' + $scope.NotiNumber + ' Notifications ';
            $scope.Notitext_1 = 'มี ' + $scope.Noti1 + ' รายการขออนุมัติงบประมาณรอการอนุมัติ';
            $scope.Notitext_2 = 'มี ' + $scope.Noti2 + ' รายการขออนุมัติงบประมาณเกินงบประมาณรอการอนุมัติ';
            $scope.Notitext_3 = 'มี ' + $scope.Noti3 + ' รายการขออนุมัติงบประมาณรอสร้างเอกสารขออนุมัติสั่งซื้อ';
            $scope.Notitext_4 = 'มี ' + $scope.Noti4 + ' รายการขออนุมัติสั่งซื้อรอการ Preview';
            $scope.Notitext_5 = 'มี ' + $scope.Noti5 + ' รายการขออนุมัติสั่งซื้อรอการอนุมัติ';
        }
    }, 5000)
    console.log($scope.data);

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
                title: "Sum Amount by Department [" + String($scope.MONTHDATE.getMonth() + 1) + "/" + String($scope.MONTHDATE.getFullYear()) + "]" ,
                "export": {
                    enabled: true
                },
                //onPointClick: function (e) {

                //    $scope.ShowSumdept = 0;
                //    //var point = e.target;
                //    console.log(e.target.Document_Dep);


                //    $http.get("api/Report/DashBroadByDepartment?Dep=" + e.target.argument).then(function (data) {

                //        $scope.ListReportBudgets = data.data.Results.DepAmount;
                //        $scope.ListReportPOPR = data.data.Results.PRPOData;
                //        var SeriesName = data.data.Results.Department[0].Name;
                //        $scope.chartOptions = {
                //            size: {
                //                width: 100 + '%'
                //            },

                //            palette: "soft",
                //            dataSource: $scope.ListReportBudgets,

                //            displayMode: 'stagger',
                //            series: {
                //                argumentField: "date",
                //                valueField: "Amount",
                //                name: SeriesName,
                //                type: "bar",
                //                ignoreEmptyPoints: true,

                //            },
                //            title: "Sum Amount Department by Date",
                //            "export": {
                //                enabled: true
                //            },
                //            legend: {
                //                verticalAlignment: "bottom",
                //                horizontalAlignment: "center"
                //            },
                //            onPointClick: function (e) {


                //            },

                //        };
                //        $scope.chart1Options = {
                //            size: {
                //                width: 100 + '%'
                //            },

                //            palette: "soft",
                //            dataSource: $scope.ListReportPOPR,
                //            commonSeriesSettings: {
                //                argumentField: "Document_Vnos",
                //                type: "bar",
                //                hoverMode: "allArgumentPoints",
                //                selectionMode: "allArgumentPoints",
                //                label: {
                //                    visible: true,
                //                    format: {
                //                        type: "fixedPoint",
                //                        precision: 2
                //                    }
                //                }
                //            },
                //            series: [
                //                { valueField: "POAmount", name: "PO Amount" },
                //                { valueField: "PRAmount", name: "PR Amount" },

                //            ],
                //            title: "Sum Amount Department by Date",
                //            "export": {
                //                enabled: true
                //            },
                //            legend: {
                //                verticalAlignment: "bottom",
                //                horizontalAlignment: "center"
                //            },

                //        };
                //    });

                //},
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
