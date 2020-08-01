angular.module('ApkApp').controller('ListPROverforApproveController', ['$scope', '$stateParams', '$http', '$rootScope', '$filter',
    function ($scope, $stateParams, $http, $rootScope, $filter) {

        $scope.showColumnLines = true;
        $scope.showRowLines = true;
        $scope.showBorders = true;
        $scope.rowAlternationEnabled = true;


        $http.get("api/PR/ListPROverForApprove/" + localStorage.getItem('StaffID') + "?depid=" + localStorage.getItem('StaffDepartmentID')).then(function (data) {
           
            if (data.data.StatusCode > 1) {
                console.log(data.data.StatusCode);
                swal({
                    title: 'information',
                    text: data.data.Messages,
                    type: "info",
                    showCancelButton: false,
                    confirmButtonColor: "#6EAA6F",
                    confirmButtonText: 'OK'
                }, function () {
                })
            }
            else {
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
                        dataField: "Staff",
                        caption: "พนักงาน"
                    }, {
                        dataField: "Approve",
                        caption: "อนุมัติ",
                        cellTemplate: function (container, item) {
                            var data = item.data,
                                markup = "<a>อนุมัติ</a>";
                            container.append(markup);
                        }

                    }]
                };
            }
        });
        var formatNumber = function (num) {
            return num.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, '$1,')
        }
        var onCellClickViewPR = function (e) {

            if (e.column.dataField === "Document_Vnos") {
                window.location = '#/PurchaseRequest/ViewPurchaseRequest/' + e.data.Document_Id;
            };
            if (e.column.dataField === "Approve") {
                console.log(e);
                window.location = '#/PurchaseRequest/ApprovePROverBudget/' + e.data.Document_Id;
            };
        };



    }
])