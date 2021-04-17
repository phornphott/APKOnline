angular.module('ApkApp').controller('ManageBudgetDepController', ['$scope', '$stateParams', '$http', '$rootScope', '$filter',
    function ($scope, $stateParams, $http, $rootScope, $filter) {
        $scope.data = {};
        $scope.MONTHDATE = new Date();
        $scope.startEditAction = "click";
        $scope.selectTextOnEditStart = true;
        $scope.showColumnLines = true;
        $scope.showRowLines = true;
        $scope.showBorders = true;
        $scope.rowAlternationEnabled = true;
        $scope.Year = new Date().getFullYear();

        $scope.dataGridOptions = {
            //dataSource: data.data.Results.Budget,
            loadPanel: {
                enabled: false
            },
            scrolling: {
                mode: "infinite"
            },
            sorting: {
                mode: "multiple"
            },
            searchPanel: {
                visible: true,
                width: 200,
                placeholder: "Search..."
            },

            bindingOptions: {
                showColumnLines: "showColumnLines",
                showRowLines: "showRowLines",
                showBorders: "showBorders",
                rowAlternationEnabled: "rowAlternationEnabled"
            },
            editing: {
                mode: "cell",
                allowUpdating: true,
                selectTextOnEditStart: false,
                startEditAction: "click"
            },
            columnAutoWidth: true,
            height: 100 + '%',
            columns: [{
                dataField: "depid",
                caption: "ลำดับ",
                width: 50,
                alignment: "center",
                editorOptions: {
                    disabled: true
                },
                cellTemplate: function (container, item) {
                    var data = item.data,
                        markup = "<p>" + (item.rowIndex + 1) + "</p>";
                    container.append(markup);
                },
            }, {
                dataField: "depcode",
                caption: "รหัสแผนก",
                editorOptions: {
                    disabled: true
                },
            }, {
                dataField: "depdesct",
                caption: "ชื่อแผนก",
                editorOptions: {
                    disabled: true
                },
            }, {
                dataField: "DEPmonth1",
                    caption: "มกราคม",
                    alignment: "right",
                    cellTemplate: function (container, item) {
                        var data = item.data,
                            markup = formatNumber(parseFloat(data.DEPmonth1).toFixed(2));
                        container.append(markup);
                    },
            }, {
                dataField: "DEPmonth2",
                    caption: "กุมภาพันธ์",
                    alignment: "right",
                    cellTemplate: function (container, item) {
                        var data = item.data,
                            markup = formatNumber(parseFloat(data.DEPmonth2).toFixed(2));
                        container.append(markup);
                    },
            }, {
                dataField: "DEPmonth3",
                    caption: "มีนาคม",
                    alignment: "right",
                    cellTemplate: function (container, item) {
                        var data = item.data,
                            markup = formatNumber(parseFloat(data.DEPmonth3).toFixed(2));
                        container.append(markup);
                    },

            }, {
                dataField: "DEPmonth4",
                    caption: "เมษายน",
                    alignment: "right",
                    cellTemplate: function (container, item) {
                        var data = item.data,
                            markup = formatNumber(parseFloat(data.DEPmonth4).toFixed(2));
                        container.append(markup);
                    },

            }, {
                dataField: "DEPmonth5",
                    caption: "พฤษภาคม",
                    alignment: "right",
                    cellTemplate: function (container, item) {
                        var data = item.data,
                            markup = formatNumber(parseFloat(data.DEPmonth5).toFixed(2));
                        container.append(markup);
                    },
            }, {
                dataField: "DEPmonth6",
                    caption: "มิถุนายน",
                    alignment: "right",
                    cellTemplate: function (container, item) {
                        var data = item.data,
                            markup = formatNumber(parseFloat(data.DEPmonth6).toFixed(2));
                        container.append(markup);
                    },
            }, {
                dataField: "DEPmonth7",
                    caption: "กรกฎาคม",
                    alignment: "right",
                    cellTemplate: function (container, item) {
                        var data = item.data,
                            markup = formatNumber(parseFloat(data.DEPmonth7).toFixed(2));
                        container.append(markup);
                    },

            }, {
                dataField: "DEPmonth8",
                    caption: "สิงหาคม",
                    alignment: "right",
                    cellTemplate: function (container, item) {
                        var data = item.data,
                            markup = formatNumber(parseFloat(data.DEPmonth8).toFixed(2));
                        container.append(markup);
                    },

            }, {
                dataField: "DEPmonth9",
                    caption: "กันยายน",
                    alignment: "right",
                    cellTemplate: function (container, item) {
                        var data = item.data,
                            markup = formatNumber(parseFloat(data.DEPmonth9).toFixed(2));
                        container.append(markup);
                    },
            }, {
                dataField: "DEPmonth10",
                    caption: "ตุลาคม",
                    alignment: "right",
                    cellTemplate: function (container, item) {
                        var data = item.data,
                            markup = formatNumber(parseFloat(data.DEPmonth10).toFixed(2));
                        container.append(markup);
                    },

            }, {
                dataField: "DEPmonth11",
                    caption: "พฤศจิกายน",
                    alignment: "right",
                    cellTemplate: function (container, item) {
                        var data = item.data,
                            markup = formatNumber(parseFloat(data.DEPmonth11).toFixed(2));
                        container.append(markup);
                    },

            }, {
                dataField: "DEPmonth12",
                    caption: "ธันวาคม",
                    alignment: "right",
                    cellTemplate: function (container, item) {
                        var data = item.data,
                            markup = formatNumber(parseFloat(data.DEPmonth12).toFixed(2));
                        container.append(markup);
                    },

            }],
            onRowUpdated: function (e) {
                console.log(e);

                var group = e.data;
                var name = Object.keys(group)[0]; // Get the first item of the list;  = key name
                console.log(name);
                var value = group[name];


                var budget = {
                    "id": e.key.id,
                    "DEPid": e.key.depid,
                    "DEPcode": e.key.depcode,
                    "DEPdescT": e.key.depdesct,
                    "ColumnName": name,
                    "Budget": value,
                    "Year": $scope.Year,
                };
                console.log(budget);
                $http.post("api/Staffs/SetBudget?", budget).then(function successCallback(response) {

                    if (response.data.StatusCode > 1) {
                        swal({
                            title: 'Information',
                            text: data.Messages,
                            type: "info",
                            showCancelButton: false,
                            confirmButtonColor: "#6EAA6F",
                            confirmButtonText: 'OK'
                        })

                    }
                    else {
                        $scope.getBudget();
                    }

                }); 
            },

        };


        var formatNumber = function (num) {
            var result = '';
     
            if (num == undefined || num == null || num == 'NaN') 
            {
                result = ''
            }
            else { result = num.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, '$1,'); }

        return result 
        }
        $scope.getBudget = function () {

            var api = "api/Staffs/GetBudgetByDep/" + $scope.Year + "?";
            $http.get(api).then(function (data) {
                console.log(data);
                $("#gridContainer").dxDataGrid({ dataSource: data.data.Results.Budget });
                $("#gridContainer").dxDataGrid("instance").refresh();
            })

          

        };
        $scope.getBudget();
        $scope.dateOptionsMonth = {
            formatYear: 'yyyy',
            startingDay: 0,
            minMode: 'year',
            showWeeks: false
        };

        $scope.popupMonth = {
            opened: false
        };

        $scope.openMonth = function () {
            $scope.popupMonth.opened = true;
        };

        $scope.ChangeMonthDate = function (Date) {
            $scope.MONTHDATE = Date;
            $scope.Year = $scope.MONTHDATE.getFullYear();
        };








    }
])