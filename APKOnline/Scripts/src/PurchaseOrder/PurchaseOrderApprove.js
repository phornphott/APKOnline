angular.module('ApkApp').controller('PurchaseOrderApproveController', ['$scope', '$stateParams', '$http', '$rootScope', '$filter',
    function ($scope, $stateParams, $http, $rootScope, $filter) {
        $scope.showColumnLines = true;
        $scope.showRowLines = true;
        $scope.showBorders = true;
        $scope.rowAlternationEnabled = true;
        $http.get("api/PO/ListPOApprove/" + localStorage.getItem("StaffID") + "?").then(function (data) {
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
                onCellClick: onCellClickViewPR,
                
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
                    dataField: "Document_Cog",
                    alignment: "right",
                    format: "#,##0.00",
                    caption: "ยอดเงิน"
                }, {
                    dataField: "Document_VatSUM",
                    alignment: "right",
                    format: "#,##0.00",
                    caption: "VAT"
                }, {
                    dataField: "Document_NetSUM",
                    alignment: "right",
                    format: "#,##0.00",
                    caption: "ยอดเงินรวม VAT"
                }, {
                    dataField: "Dep",
                    caption: "แผนก"
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
                }]
            };
        });

        var onCellClickViewPR = function (e) {
            console.log(e);

            if (e.column.dataField === "Approve") {
                console.log(e);
                window.location = '#/PurchaseOrder/ApprovePO/' + e.data.Document_Id;

            };
        };
    }
])