angular.module('ApkApp').controller('PurchaseRequestReportController', ['$scope', '$stateParams', '$http', '$rootScope', '$filter',
    function ($scope, $stateParams, $http, $rootScope, $filter) {
        $scope.data = {};
        $scope.StartDate = moment(new Date()).format('YYYY-MM-DD');
        $scope.EndDate = moment(new Date()).format('YYYY-MM-DD');
        $scope.startEditAction = "click";
        $scope.selectTextOnEditStart = true;
        $scope.showColumnLines = true;
        $scope.showRowLines = true;
        $scope.showBorders = true;
        $scope.rowAlternationEnabled = true;
        $scope.Year = new Date();
        $scope.Dep = 0;
        $scope.Status = -1;
        $rootScope.DEP = {};
        $rootScope.Status = {};
        $scope.dateBox = {
            dateStart: {
                type: "date",
                displayFormat: "yyyy-MM-dd",
                value: new Date(),
                onValueChanged: function (data) {
                    $scope.StartDate = moment(data.value).format('YYYY-MM-DD');
                }
            },
            dateFinish: {
                type: "date",
                displayFormat: "yyyy-MM-dd",
                value: new Date(),
                onValueChanged: function (data) {
                    $scope.EndDate = moment(data.value).format('YYYY-MM-DD');
                }
            },
        }
        var api = "api/Report/PrepareReportPR";
        $http.get(api).then(function (data) {
            $rootScope.DEP = data.data.Results.Department;
            $rootScope.Status = data.data.Results.Status;
            console.log(data);


            $rootScope.DEP.splice(0, 0, {
                "ID": 0,
                "Name": '- เลือกทั้งหมด -',
            });
            $rootScope.Status.splice(0, 0, {
                "ID": -1,
                "Name": '- เลือกทั้งหมด -',
            });

            $("#Dept").dxLookup({
                dataSource: $rootScope.DEP ,
                displayExpr: 'Name',
                valueExpr: 'ID',
                showPopupTitle: false,
                showCancelButton: false,
                placeholder: 'เลือก',
                closeOnOutsideClick: true,
                searchEnabled: true,
                searchPlaceholder: 'ค้นหา',
                value: $scope.Dep,
                onSelectionChanged: function (data) {

                    $scope.Dep = data.selectedItem.ID;

                }
            }).dxLookup("instance");

            $("#Status").dxLookup({
                dataSource: $rootScope.Status ,
                displayExpr: 'Name',
                valueExpr: 'ID',
                showPopupTitle: false,
                showCancelButton: false,
                placeholder: 'เลือก',
                closeOnOutsideClick: true,
                searchEnabled: true,
                searchPlaceholder: 'ค้นหา',
                value: $scope.Status,
                onSelectionChanged: function (data) {

                    $scope.Status = data.selectedItem.ID;

                }
            }).dxLookup("instance");
        })

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
                //mode: "cell",
                allowUpdating: false,
                selectTextOnEditStart: false,
                startEditAction: "click"
            },
            columnAutoWidth: true,
            height: 100 + '%',
            columns: [{
                
                dataField: "Document_Vnos",
                caption: "เลขที่ใบเสนอขอซื้อภายใน",
                editorOptions: {
                    disabled: true
                },
            }, {
                    dataField: "Document_Date",
                caption: "วันที่",
                editorOptions: {
                    disabled: true
                    },
                    cellTemplate: function (container, item) {
                        var data = item.data,
                            markup = moment(data.Document_Date).format('YYYY-MM-DD');
                        container.append(markup);
                    },
            }, {
                    dataField: "DEPdescT",
                    caption: "แผนก",
                    editorOptions: {
                        disabled: true
                    },
            }, {
                    dataField: "Document_Project",
                    caption: "โครงการ",
                    editorOptions: {
                        disabled: true
                    },
            }, {
                dataField: "",
                    caption: "รหัสโครงการ",
                    editorOptions: {
                        disabled: true
                    },

            }, {
                    dataField: "Document_Cog",
                    caption: "ยอดเงิน",
                    alignment: "right",
                    cellTemplate: function (container, item) {
                        var data = item.data,
                            markup = formatNumber(parseFloat(data.Document_Cog).toFixed(2));
                        container.append(markup);
                    },

            }, {
                    dataField: "Document_VatSUM",
                    caption: "VAT",
                    alignment: "right",
                    cellTemplate: function (container, item) {
                        var data = item.data,
                            markup = formatNumber(parseFloat(data.Document_VatSUM).toFixed(2));
                        container.append(markup);
                    },
                    editorOptions: {
                        disabled: true
                    },
            }, {
                    dataField: "Document_NetSUM",
                    caption: "ยอดเงินรวมVAT",
                    alignment: "right",
                    cellTemplate: function (container, item) {
                        var data = item.data,
                            markup = formatNumber(parseFloat(data.Document_NetSUM).toFixed(2));
                        container.append(markup);
                    },
                    editorOptions: {
                        disabled: true
                    },
            }, {
                    dataField: "Staff",
                    caption: "พนักงาน",
                    editorOptions: {
                        disabled: true
                    },

            }, {
                    dataField: "DocStatus",
                    caption: "สถานะ",
                    editorOptions: {
                        disabled: true
                    },
            }],

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
        $scope.getReport = function () {

            var api = "api/Report/ReportPR?startdate=" + $scope.StartDate + "&finishdate=" + $scope.EndDate + "&dep=" + $scope.Dep + "&status=" + $scope.Status;
            $http.get(api).then(function (data) {
                console.log(data);
                $("#gridContainer").dxDataGrid({ dataSource: data.data.Results.PRReport });
                $("#gridContainer").dxDataGrid("instance").refresh();
            })

          

        };
        //$scope.getBudget();
        $scope.dateOptionsMonth = {
            formatYear: 'yyyy-MM-dd',
            startingDay: 0,
            minMode: 'date',
            showWeeks: false
        };

        $scope.popupMonth = {
            opened: false
        };

        $scope.openMonth = function () {
            $scope.popupMonth.opened = true;
        };

        $scope.ChangeStartDate = function (Date) {
            console.log(Date);
            console.log($scope.StartDate);
            $scope.StartDate = Date;
        };

        $scope.ChangeEndDate = function (Date) {
            $scope.EndDate = Date;
        };






    }
])