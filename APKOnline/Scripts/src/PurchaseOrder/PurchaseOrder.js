angular.module('ApkApp').controller('PurchaseOrderController', ['$scope', '$stateParams', '$http', '$rootScope', '$filter',
    function ($scope, $stateParams, $http, $rootScope, $filter) {
        $scope.showColumnLines = true;
        $scope.showRowLines = true;
        $scope.showBorders = true;
        $scope.rowAlternationEnabled = true;
        $http.get("api/PO/ListPO/" + localStorage.getItem("StaffID")+"?").then(function (data) {
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
                onCellClick: onCellClickViewPO,
                columnAutoWidth: true,
                columns: [{
                   
                    dataField: "Document_Vnos",
                    caption: "เลขที่ใบสำคัญ",
                    cellTemplate: function (container, item) {
                        var data = item.data,
                            markup = "<a >" + data.Document_Vnos + "</a>";
                        container.append(markup);
                    },
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
                    caption: "ยอดเงิน",
                    cellTemplate: function (container, item) {
                        var data = item.data,
                            markup = formatNumber(parseFloat(data.Document_Cog).toFixed(2));
                        container.append(markup);
                    },
                }, {
                    dataField: "Document_VatSUM",
                    alignment: "right",
                    caption: "VAT",
                    cellTemplate: function (container, item) {
                        var data = item.data,
                            markup = formatNumber(parseFloat(data.Document_VatSUM).toFixed(2));
                        container.append(markup);
                    },
                }, {
                    dataField: "Document_NetSUM",
                    alignment: "right",
                    caption: "ยอดเงินรวม VAT",
                    cellTemplate: function (container, item) {
                        var data = item.data,
                            markup = formatNumber(parseFloat(data.Document_NetSUM).toFixed(2));
                        container.append(markup);
                    },
                }, {
                        dataField: "Dep",
                    caption: "แผนก"
                }, {
                    dataField: "Staff",
                    caption: "พนักงาน"
                }, {
                        dataField: "Document_Status",
                        caption: "พิมพ์",
                        cellTemplate: function (container, item) {
                            var data = item.data,
                                markup = "<a > พิมพ์เอกสาร </a>";
                            if (data.Document_Status < 2) {
                                markup = "<a > </a>";
                            }
                            container.append(markup);
                        },
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
                    caption: "เลขที่" ,
                    cellTemplate: function (container, item) {
                        var data = item.data,
                            markup = "<a >" + data.Document_Vnos + "</a>";
                        container.append(markup);
                    },
                }, {
                        dataField: "DocDate",
                    caption: "วันที่"
                }, {
                        dataField: "Document_Desc",
                    caption: "รายละเอียด"
                }, {
                        dataField: "Qty",
                        caption: "จำนวน",
                        cellTemplate: function (container, item) {
                            var data = item.data,
                                markup = formatNumber(parseFloat(data.Qty).toFixed(2));
                            container.append(markup);
                        },

                }, {
                        dataField: "Document_NetSUM",
                        caption: "ราคา",
                        cellTemplate: function (container, item) {
                            var data = item.data,
                                markup = formatNumber(parseFloat(data.Document_NetSUM).toFixed(2));
                            container.append(markup);
                        },
                    }, {
                        dataField: "Staff",
                        caption: "พนักงานที่ขอ"
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
        var onCellClickViewPO = function (e) {
            console.log(e);
            if (e.column.dataField === "Document_Vnos") {
                localStorage.setItem('polinkid', '1');
                setTimeout(function () {
                    window.location = '#/PurchaseRequest/ViewPO/' + e.data.Document_Id;
                }, 700);

            }
            if (e.column.dataField === "Document_Status") {
                setTimeout(function () {
                    window.open('/Reports/PrintPreview.aspx?Parameter=' + e.data.Document_Id, '_blank');
                }, 700);
            }
        };

        var onCellClickViewPR = function (e) {
            console.log(e);
           
            if (e.column.dataField === "POCreate") {
                $('#ImportPRModal').modal('hide');
                setTimeout(function () {
                    window.location = "#/PurchaseRequest/ViewPurchaseOrder/" + e.data.Document_Id;
                }, 700);
            };
            if (e.column.dataField === "Document_Vnos") {
                $('#ImportPRModal').modal('hide');
                localStorage.setItem('prlinkid', '2');
                setTimeout(function () {
                    window.location = '#/PurchaseRequest/ViewPurchaseRequest/' + e.data.Document_Id;
                }, 700);
                
            }
        };
        $scope.CancelDocuments = function () {
            window.location = '#/PurchaseOrder/ListPurchaseOrder';

        };
        var formatNumber = function (num) {
            return num.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, '$1,')
        }

    }
])