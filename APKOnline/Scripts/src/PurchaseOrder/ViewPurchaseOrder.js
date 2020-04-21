﻿angular.module('ApkApp').controller('ViewPurchaseOrderController', ['$scope', '$stateParams', '$http', '$rootScope', '$filter',
    function ($scope, $stateParams, $http, $rootScope, $filter) {
        $scope.showColumnLines = true;
        $scope.showRowLines = true;
        $scope.showBorders = true;
        $scope.rowAlternationEnabled = true;
        $scope.Document_ID = Math.floor(Math.random() * 10000); 
        var d = new Date()
        $scope.DocDate = d.getDate() + '-' + (d.getMonth() + 1) + '-' + d.getFullYear();
        $scope.Document_Vnos = "";
        $http.get("api/PO/CreatePOData/" + $stateParams.id + "?tmpid=" + $scope.Document_ID).then(function (data) {
            console.log(data);
            //$scope.Header = data.data.Results.Document_Vnos[0].Column1
            $scope.Header = data.data.Results.Header[0]
            $scope.Document_Vnos = data.data.Results.Document_Vnos[0].Column1
            var Detail = data.data.Results.Detail;
            $scope.dataGridOptions = {
                dataSource: Detail,
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
                    allowUpdating: true,
                    
                    texts: {
                        editRow: "แก้ไข",
                        saveRowChanges: "บันทึก",
                        cancelRowChanges: "ยกเลิก"
                    }
                },
                columnAutoWidth: true,
                columns: [{
                    dataField: "Document_Detail_Acc",
                    caption: "รหัสบัญชี"
                }, {
                    dataField: "Document_Detail_Stk",
                    caption: "รหัสสินค้า"
                }, {
                        
                        dataField: "Document_Detail_Stk_Desc",
                    caption: "รายละเอียดสินค้า"
                }, {
                        dataField: "Document_Detail_Quan",
                    caption: "จำนวน"
                }, {
                        dataField: "Document_Detail_UnitPrice",
                    caption: "ราคา/หน่วย"
                }, {
                        dataField: "Document_Detail_Cog",
                        caption: "จำนวนเงิน",
                        editorOptions: {
                            disabled: true
                        }
                }],
                onEditorPrepared: function (e) {

                    if (e.dataField == "Document_Detail_Quan") {
                        $(e.editorElement).dxNumberBox("instance").on("valueChanged", function (args) {
                            var grid = $("#gridContainer").dxDataGrid("instance");
                            var index = e.row.rowIndex;
                            var data = grid.cellValue(index, "Document_Detail_UnitPrice");
                            var result = 0;
                            if (data == undefined) {
                                result = 0;

                            }
                            else {
                                result = args.value * data;
                            }

                            var amount = args.value;
                            grid.cellValue(index, "Document_Detail_Quan", amount);
                            grid.cellValue(index, "Document_Detail_Cog", result);

                        });
                    }
                    else if (e.dataField == "Document_Detail_UnitPrice") {
                        $(e.editorElement).dxNumberBox("instance").on("valueChanged", function (args) {
                            var grid = $("#gridContainer").dxDataGrid("instance");
                            var index = e.row.rowIndex;
                            var data = grid.cellValue(index, "Document_Detail_Quan");
                            var result = 0;
                            if (data == undefined) {
                                result = 0;

                            }
                            else {
                                result = args.value * data;
                            }

                            var amount = args.value;
                            grid.cellValue(index, "Document_Detail_UnitPrice", amount);
                            grid.cellValue(index, "Document_Detail_Cog", result);

                        });
                    }
                },
                onRowUpdated: function (e) {
                    var detail = {
                        
                        "Document_Detail_Id": e.key.Document_Detail_Id,

                        "Document_Detail_Vnos": '',
                        "Document_Detail_Acc": e.key.Document_Detail_Acc,
                        "Document_Detail_Stk": e.key.Document_Detail_Stk,
                        "Document_Detail_Stk_Desc": e.key.Document_Detail_Acc_Desc,
                        "Document_Detail_Quan": e.key.Document_Detail_Quan,
                        "Document_Detail_UnitPrice": e.key.Document_Detail_UnitPrice,
                        "Document_Detail_CreateUser": localStorage.getItem('StaffID'),

                    };
                    console.log(detail);
                    $http.post("api/PO/UpdateDetailData/0", detail).then(function successCallback(response) {
                    });
                }
            };
        });

        $scope.SaveDocuments = function () {
            
       
                var Header = {
                    "Document_Id": $scope.Document_ID,
                    "Document_Group": $scope.Header.Document_Group,
                    "Document_Category": $scope.Header.Document_Category,
                    "Document_Objective": $scope.Header.Objective,
                    "Document_Vnos": '',
                    "Document_Means": $scope.Header.Document_Means,
                    "Document_Expect": $scope.Header.Document_Expect,
                    "Document_Cus": '',
                    "Document_Job": $scope.Header.Document_Job,
                    "Document_Dep": $scope.Header.Document_Dep,
                    "Document_Per": '',
                    "Document_Doc": '',
                    "Document_Mec": '',
                    "Document_Desc": '',
                    "Document_Tel": $scope.Document_Tel,
                    "Document_CreateUser": localStorage.getItem('StaffID'),

                };
                $http.post("api/PO/SavePOData/" + $stateParams.id + "?", Header).then(function successCallback(response) {
                    console.log(response);
                    window.location = '#/PurchaseOrder/ListPurchaseOrder';
                });
            




        };
        $scope.CancelDocuments = function () {

            $http.get("api/PO/CancelPOTmpDetail/" + $stateParams.id).then(function (data) {
                console.log(data);
                window.location = '#/PurchaseOrder/ListPurchaseOrder';
            });
        };

    }
])