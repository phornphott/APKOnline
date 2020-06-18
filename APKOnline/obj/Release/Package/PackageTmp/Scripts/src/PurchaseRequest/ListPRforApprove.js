﻿angular.module('ApkApp').controller('ListPRApproveController', ['$scope', '$stateParams', '$http', '$rootScope', '$filter',
    function ($scope, $stateParams, $http, $rootScope, $filter) {

        $scope.showColumnLines = true;
        $scope.showRowLines = true;
        $scope.showBorders = true;
        $scope.rowAlternationEnabled = true;


        $http.get("api/PR/ListPRForApprove/" + localStorage.getItem('StaffID') + "?deptid=" + localStorage.getItem('StaffDepartmentID')).then(function (data) {
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
                    dataField: "Document_Vnos",
                    caption: "เลขที่ใบขอซื้อภายใน",
                    cellTemplate: function (container, item) {
                        var data = item.data,
                            markup = "<a >" + data.Document_Vnos + "</a>";
                        container.append(markup);
                    },

                }, {
                        dataField: "DocDate",
                    caption: "วันที่"
                }, {
                        dataField: "DEPdescT",
                    caption: "แผนก"
                }, {
                    dataField: "JOBdescT",
                    caption: "โครงการ"
                }, {
                        dataField: "Staff",
                    caption: "พนักงาน"
                }, {
                        dataField: "Approve",
                    caption: "อนุมัติ",
                    cellTemplate: function (container, item) {
                    
                        var data = item.data,
                            markup = "<a>อนุมัติ/รับทราบ</a>";
                        container.append(markup);
                    }
                //}, {
                //    dataField: "ใบสั่งชื้อ",
                //    caption: "ใบสั่งชื้อ"
                //}, {
                //    dataField: "BOD",
                //    caption: "BOD"
                //}, {
                //    dataField: "Excom",
                //    caption: "Excom"
                //}, {
                //    dataField: "MD",
                //    caption: "MD"
                //}, {
                //    dataField: "DMD",
                //    caption: "DMD"
                //}, {
                //    dataField: "Dir",
                //    caption: "Dir"
                //}, {
                //    dataField: "Mgr",
                //    caption: "Mgr"
                }]
            };
        });

        var onCellClickViewPR = function (e) {
            console.log(e);
            if (e.column.dataField === "Document_Vnos") {
                window.location = '#/PurchaseRequest/ViewPurchaseRequest/' + e.data.Document_Id;
            };
            if (e.column.dataField === "Approve") {
                console.log(e);
                window.location = '#/PurchaseRequest/ApprovePR/' + e.data.Document_Id;
            };
        };



    }
])