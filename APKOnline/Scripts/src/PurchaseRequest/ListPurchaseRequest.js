angular.module('ApkApp').controller('ListPurchaseRequestController', ['$scope', '$stateParams', '$http', '$rootScope', '$filter',
    function ($scope, $stateParams, $http, $rootScope, $filter) {

        $scope.showColumnLines = true;
        $scope.showRowLines = true;
        $scope.showBorders = true;
        $scope.rowAlternationEnabled = true;


        $http.get("api/PR/ListPRByStaff/" + localStorage.getItem('StaffID')+"?").then(function (data) {
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
                        dataField: "Document_Project",
                    caption: "โครงการ"
                }, {
                    dataField: "JOBdescT",
                    caption: "รหัสโครงการ"
                },  {
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
                    dataField: "DocStatus",
                    alignment: "center",
                    caption: "สถานะ",
                    editorOptions: {
                        disabled: true
                    }
                }, {
                        dataField: "Document_Id",
                        caption: "แก้ไขข้อมูล",
                        alignment: 'center',
                        allowFiltering: false,
                        width: 100,
                        cellTemplate: function (container, options) {
                            if (options.key.Document_Status == 0) {
                                $("<div />").dxButton({
                                    icon: 'fa fa-pencil-square',
                                    type: 'default',
                                    disabled: false,
                                    onClick: function (e) {
                                        var r = confirm("ต้องการแก้ไขใบขออนุมัตินี้ใช่หรือไม่ !!!");
                                        if (r === true) {
                                            window.location = '#/PurchaseRequest/EditPurchaseRequest/' + options.key.Document_Id;

                                        }
                                    }
                                }).appendTo(container);
                            }
                        }

                }, {
                     dataField: "Document_Id",
                     caption: "ลบข้อมูล",
                     alignment: 'center',
                     allowFiltering: false,
                     width: 100,
                     cellTemplate: function (container, options) {
                         if (options.key.Document_Status == 0) {

                             $("<div />").dxButton({
                                 icon: 'fa fa-trash',
                                 type: 'danger',
                                 disabled: false,
                                 onClick: function (e) {
                                     var r = confirm("ต้องการลบใบขออนุมัตินี้ใช่หรือไม่ !!!");
                                     if (r === true) {
                                         $http.post("api/PR/DeletePRData/" + options.key.Document_Id).then(function successCallback(response) {
                                             if (response.data.StatusCode > 1) {
                                                 $("#loadIndicator").dxLoadIndicator({
                                                     visible: false
                                                 });
                                                 DevExpress.ui.notify(response.data.Messages);

                                             } else {
                                                 DevExpress.ui.notify(response.data.Messages);
                                                 $("#loadIndicator").dxLoadIndicator({
                                                     visible: false
                                                 });
                                                 $("#gridContainer").show();
                                                 //var api = "api/Staffs/StaffData"
                                                 $http.get("api/PR/ListPRByStaff/" + localStorage.getItem('StaffID') + "?").then(function (data) {
                                                     var Datasource = data.data.Results.ListPRData;
                                                     $("#gridContainer").dxDataGrid("instance").option("dataSource", Datasource);
                                                     $("#gridContainer").dxDataGrid("instance").refresh();
                                                 });
                                             }

                                         });
                                     }
                                 }
                             }).appendTo(container);
                         }
                     }
                 }]
            };
        });
        var formatNumber = function (num) {
            return num.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, '$1,')
        }
        var onCellClickViewPR = function (e) {
            console.log(e);
            if (e.column.dataField === "Document_Vnos") {
                localStorage.setItem('prlinkid', '1');
                window.location = '#/PurchaseRequest/ViewPurchaseRequest/' + e.data.Document_Id;
            };
        };



    }
])