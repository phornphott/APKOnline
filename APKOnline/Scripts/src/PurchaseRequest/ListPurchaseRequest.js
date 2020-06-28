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
                    dataField: "JOBdescT",
                    caption: "โครงการ"
                }, , {
                    dataField: "Document_NetSUM",
                    alignment: "right",
                    format: "currency",
                    caption: "ยอดเงิน"
                }, {
                        dataField: "Staff",
                    caption: "พนักงาน"
                //}, {
                //    dataField: "สถานะ",
                //    caption: "สถานะ",
                //    editorOptions: {
                //        disabled: true
                //    },
                //    cellTemplate: function (container, item) {
                //        var data = item.data,
                //            markup = "<a>อนุมัติ</a>";
                //        container.append(markup);
                //    }
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
                 }, {
                     dataField: "DocStatus",
                     alignment: "center",
                    caption: "สถานะ",
                    editorOptions: {
                        disabled: true
                    }
                 }, {
                     dataField: "Document_Id",
                     caption: "ลบข้อมูล",
                     alignment: 'center',
                     allowFiltering: false,
                     width: 100,
                     cellTemplate: function (container, options) {
                         $("<div />").dxButton({
                             icon: 'fa fa-trash',
                             type: 'default',
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
                 }]
            };
        });

        var onCellClickViewPR = function (e) {
            console.log(e);
            if (e.column.dataField === "Document_Vnos") {
                window.location = '#/PurchaseRequest/ViewPurchaseRequest/' + e.data.Document_Id;
            };
        };



    }
])