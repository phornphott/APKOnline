angular.module('ApkApp').controller('ListPreviewController', ['$scope', '$stateParams', '$http', '$rootScope', '$filter',
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
                    cellTemplate: function (container, item) {
                        var data = item.data,
                            markup = "<a >" + data.PRNo + "</a>";
                        container.append(markup);
                    },
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
                        caption: "เลขที่ใบสั่งซื้อ"  ,
                        cellTemplate: function (container, item) {
                            var data = item.data,
                                markup = "<a >" + data.PONo + "</a>";
                            container.append(markup);
                        },
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
                    caption: "ยืนยัน Preview ข้อมูล",
                        alignment: 'center',
                        allowFiltering: false,
                        width: 100,
                        cellTemplate: function (container, options) {
                            if (options.key.logSatus == 0) {
                                $("<div />").dxButton({
                                    icon: 'fa fa-check-square',
                                    type: 'success',
                                    disabled: false,
                                    onClick: function (e) {
                                        var r = confirm("ต้องการยืนยันการ Preview เอกสารขอซื้อภายในและเอกสารสั่งซื้อ ใช่หรือไม่ ?");
                                        if (r === true) {
                                            $http.post("api/PR/PostLogPreview/" + options.key.logId).then(function successCallback(response) {
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
                                                    $http.get("api/PR/ListPreview/" + localStorage.getItem('StaffID') + "?deptid=" + localStorage.getItem('StaffDepartmentID')).then(function (data) {
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

            if (e.column.dataField === "PRNo")
            {
                localStorage.setItem('prlinkid', '5');
                setTimeout(function () {
                    window.location = '#/PurchaseRequest/ViewPurchaseRequest/' + e.data.PRID;
                }, 700);
            }
            else if (e.column.dataField === "PONo")
            {
                localStorage.setItem('polinkid', '2');
                setTimeout(function () {
                    window.location = '#/PurchaseRequest/PreviewEdit/' + e.data.POID;
                }, 700);
            }
            //else if (e.column.dataField === "Preview") {
            //    $http.get("api/PR/LogPreview/" + e.data.logId).then(function (data) {
            //        localStorage.setItem('polinkid', '2');
            //        window.location = "#/PurchaseRequest/ViewPO/" + e.data.POID;
            //    });

            //};

        };



    }
])