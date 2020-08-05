﻿angular.module('ApkApp').controller('ListPreviewController', ['$scope', '$stateParams', '$http', '$rootScope', '$filter',
    function ($scope, $stateParams, $http, $rootScope, $filter) {

        $scope.showColumnLines = true;
        $scope.showRowLines = true;
        $scope.showBorders = true;
        $scope.rowAlternationEnabled = true;


        $http.get("api/PR/ListPreview/" + localStorage.getItem('StaffID') + "?deptid=" + localStorage.getItem('StaffDepartmentID')).then(function (data) {
            console.log(data);
            $scope.dataGridOptions = {
                dataSource: data.data.Results.ListPRData,
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
                onCellClick: onCellClickViewPR,
                columnAutoWidth: true,
                columns: [{
                    dataField: "PRNo",
                    caption: "เลขที่ใบขอซื้อภายใน",
                }, {
                        dataField: "PRAmount",
                        alignment: "right",
                        caption: "ยอดเงินใบขอซื้อภายใน",
                        cellTemplate: function (container, item) {
                            var data = item.data,
                                markup = formatNumber(parseFloat(data.PRAmount).toFixed(2));
                            container.append(markup);
                        },
                    }, {
                        dataField: "PONo",
                        caption: "เลขที่ใบสั่งซื้อ"
                    }, {
                        dataField: "POAmount",
                        alignment: "right",
                        caption: "ยอดเงินใบสั่งซื้อ",
                        cellTemplate: function (container, item) {
                            var data = item.data,
                                markup = formatNumber(parseFloat(data.POAmount).toFixed(2));
                            container.append(markup);
                        },
              
                }, {
                    dataField: "Preview",
                    caption: "",
                    cellTemplate: function (container, item) {
                    
                        var data = item.data,
                            markup = "<a>Preview</a>";
                        container.append(markup);
                    }

                }]
            };
        });
        var formatNumber = function (num) {
            return num.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, '$1,')
        }
        var onCellClickViewPR = function (e) {
      
            if (e.column.dataField === "Preview") {
                $http.get("api/PR/LogPreview/" + e.data.logId).then(function (data) {
                    localStorage.setItem('polinkid', '2');
                    window.location = "#/PurchaseRequest/ViewPO/" + e.data.POID;
                });

            };

        };



    }
])