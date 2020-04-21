﻿angular.module('ApkApp').controller('PurchaseOrderController', ['$scope', '$stateParams', '$http', '$rootScope', '$filter',
    function ($scope, $stateParams, $http, $rootScope, $filter) {
        $scope.showColumnLines = true;
        $scope.showRowLines = true;
        $scope.showBorders = true;
        $scope.rowAlternationEnabled = true;
        $http.get("api/PO/ListPO").then(function (data) {
            var ListPRData = data.data.Results.ListPRData;
            $scope.dataGridOptions = {
                dataSource: ListPRData,
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
                    mode: "row",
                },
                columnAutoWidth: true,
                columns: [{
                   
                    dataField: "Document_Vnos",
                    caption: "เลขที่ใบสำคัญ"
                }, {
                        dataField: "DocDate",
                    caption: "วันที่"
                }, {
                        dataField: "Group",
                    caption: "ลักษณะค่าใช้จ่าย"
                }, {
                        dataField: "Category",
                    caption: "ประเภทการจัดซื้อ"
                }, {
                    dataField: "Document_Desc",
                    caption: "คำอธิบาย"
                }, {
                        dataField: "Document_NetSUM",
                    caption: "ยอดรวม"
                }, {
                        dataField: "Dep",
                    caption: "แผนก"
                }, {
                    dataField: "Staff",
                    caption: "พนักงาน"
                }]
            };
        });
        $http.get("api/PO/ListPRForCreatePO/0").then(function (data) {
            var ListPRData = data.data.Results.ListPRData;
            $scope.dataPurchaseRequestOptions = {
                dataSource: ListPRData, 
                loadPanel: {
                    enabled: false
                },
                scrolling: {
                    mode: "infinite"
                },
                sorting: {
                    mode: "multiple"
                },
                //selection: {
                //    mode: "multiple"
                //},
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
                    caption: "เลขที่"
                }, {
                        dataField: "DocDate",
                    caption: "วันที่"
                }, {
                        dataField: "Document_Desc",
                    caption: "รายละเอียด"
                }, {
                        dataField: "Qty",
                    caption: "จำนวน"
                }, {
                        dataField: "Document_NetSUM",
                        caption: "ราคา"
                }, {
                    dataField: "POCreate",
                        caption: "สร้างเอกสารสั่งซื้อ",
                        cellTemplate: function (container, item) {
                            var data = item.data,
                                markup = "<a>สร้างเอกสารสั่งซื้อ</a>";
                            container.append(markup);
                        }
                }]
            };
        });
        $scope.goToViewPurchaseOrder = function () {
            
        };
        var onCellClickViewPR = function (e) {
            console.log(e);
           
            if (e.column.dataField === "POCreate") {
                $('#ImportPRModal').modal('hide');
                setTimeout(function () {
                    window.location = "#/PurchaseRequest/ViewPurchaseOrder/" + e.data.Document_Id;
                }, 700);
            };
        };
    }
])