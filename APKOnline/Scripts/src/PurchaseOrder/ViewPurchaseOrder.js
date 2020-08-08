angular.module('ApkApp').controller('ViewPurchaseOrderController', ['$scope', '$stateParams', '$http', '$rootScope', '$filter',
    function ($scope, $stateParams, $http, $rootScope, $filter) {
        $scope.showColumnLines = true;
        $scope.showRowLines = true;
        $scope.showBorders = true;
        $scope.rowAlternationEnabled = true;
        $scope.Document_Cus = 0;
        $scope.Document_ID = Math.floor(Math.random() * 10000); 
        var d = new Date()
        $scope.DocDate = d.getDate() + '-' + (d.getMonth() + 1) + '-' + d.getFullYear();
        $scope.Document_Vnos = "";
        $http.get("api/PO/CreatePOData/" + $stateParams.id + "?tmpid=" + $scope.Document_ID).then(function (data) {
            console.log(data);
            $rootScope.Account = data.data.Results.Account;
            $rootScope.STK = data.data.Results.STK;
            //$scope.Header = data.data.Results.Document_Vnos[0].
            var FileUpload = data.data.Results.FileUpload;
            $scope.detailorg   =data.data.Results.Detail;
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
                    caption: "รหัสบัญชี",
                    lookup: {
                        dataSource: $rootScope.Account,
                        displayExpr: "Name",
                        valueExpr: "Code"
                    }
                }, {
                    dataField: "Document_Detail_Stk",
                    caption: "รหัสสินค้า",
                    lookup: {
                        dataSource: $rootScope.STK,
                        displayExpr: "Code",
                        valueExpr: "Code"
                    }
                }, {
                    dataField: "Document_Detail_Acc_Desc",
                    caption: "รายละเอียดสินค้า"
                }, {
                    dataField: "Document_Detail_Quan",
                    caption: "Qty",
                    alignment: "right",
                    cellTemplate: function (container, item) {
                        var data = item.data,
                            markup = formatNumber(parseFloat(data.Document_Detail_Quan).toFixed(2));
                        container.append(markup);
                    },
                }, {
                    dataField: "Document_Detail_UnitPrice",
                    caption: "ราคา/หน่วย",
                    alignment: "right",
                    cellTemplate: function (container, item) {
                        var data = item.data,
                            markup = formatNumber(parseFloat(data.Document_Detail_UnitPrice).toFixed(2));
                        container.append(markup);
                    },

                }, {
                    dataField: "Document_Detail_Cog",
                    caption: "จำนวนเงิน",
                    alignment: "right",

                    editorOptions: {
                        disabled: true
                    },
                    cellTemplate: function (container, item) {
                        var data = item.data,
                            markup = formatNumber(parseFloat(data.Document_Detail_Cog).toFixed(2));
                        container.append(markup);
                    },
                }],
                onEditorPrepared: function (e) {
                    if (e.parentType == 'dataRow' && e.dataField == 'Document_Detail_Quan') {
                        //$(e.editorElement).dxTextBox("instance").on("keyPress", function (args) {
                        //let instance = e.editorElement.dxNumberBox("instance"); 
                        //instance.option("keyPress", function (args) {
                        //    var event = args.jQueryEvent;
                        //    console.log(event);
                        //    if (event.which != 8 && event.which != 46 && event.which != 0 && (event.which < 48 || event.which > 57)) {
                        //        event.stopPropagation();
                        //        event.preventDefault();

                        //    }
                        //});

                        //instance.option("valueChanged", function (args) {

                        //    var grid = $("#gridContainer").dxDataGrid("instance");
                        //    console.log(grid);
                        //    var index = e.row.rowIndex;
                        //    var data = grid.cellValue(index, "Document_Detail_UnitPrice");
                        //    var result = 0;
                        //    if (data == undefined) {
                        //        result = 0;

                        //    }
                        //    else {
                        //        result = args.value * data;
                        //        result = parseFloat(result).toFixed(2);
                        //    }

                        //    var amount = args.value;
                        //    grid.cellValue(index, "Document_Detail_Quan", amount);
                        //    grid.cellValue(index, "Document_Detail_Cog", result);

                        //});

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
                                result = parseFloat(result).toFixed(2);
                            }

                            var amount = args.value;
                            grid.cellValue(index, "Document_Detail_Quan", amount);
                            grid.cellValue(index, "Document_Detail_Cog", result);

                        });
                    }
                    else if (e.parentType == 'dataRow' && e.dataField == "Document_Detail_UnitPrice") {
                        //let instance = e.editorElement.dxNumberBox("instance");
                        //instance.option("keyPress", function (args) {
                        //    var event = args.jQueryEvent;
                        //    console.log(event);
                        //    if (event.which != 8 && event.which != 46 && event.which != 0 && (event.which < 48 || event.which > 57)) {
                        //        event.stopPropagation();
                        //        event.preventDefault();

                        //    }
                        //});

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
                                result = parseFloat(result).toFixed(2);
                            }

                            var amount = parseFloat(args.value).toFixed(2);
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
                        "Document_Detail_Acc_Desc": e.key.Document_Detail_Acc_Desc,
                        "Document_Detail_Stk_Desc": e.key.Document_Detail_Acc_Desc,
                        "Document_Detail_Quan": e.key.Document_Detail_Quan,
                        "Document_Detail_UnitPrice": e.key.Document_Detail_UnitPrice,
                        "Document_Detail_CreateUser": localStorage.getItem('StaffID'),

                    };
                    console.log(detail);
                    $http.post("api/PO/UpdateDetailData/0", detail).then(function successCallback(response) {
                        console.log(response)
                        if (response.data.StatusCode > 1) {
                            swal({
                                title: 'Information',
                                text: response.data.Messages,
                                type: "info",
                                showCancelButton: false,
                                confirmButtonColor: "#6EAA6F",
                                confirmButtonText: 'OK'
                            })

                        }
                        $http.get("api/PO/PRDetailData/" + $stateParams.id + "?type=0").then(function (data) {
                            console.log(data.data.Results.Detail);
                            console.log(data);
                            if (data.data.StatusCode > 1) {
                                swal({
                                    title: 'Information',
                                    text: data.Messages,
                                    type: "info",
                                    showCancelButton: false,
                                    confirmButtonColor: "#6EAA6F",
                                    confirmButtonText: 'OK'
                                })

                            }
                            $("#gridContainer").dxDataGrid({
                                dataSource: data.data.Results.Detail

                            });
                            $("#gridContainer").dxDataGrid("instance").refresh();
                        });

                    });
                }
            };
            console.log(FileUpload);
            $scope.datafileGridOptions = {
                dataSource: data.data.Results.FileUpload,
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
                    visible: false,
                    width: 200,
                    placeholder: "Search..."
                },
                onCellClick: onCellClickViewFile,

                bindingOptions: {
                    showColumnLines: "showColumnLines",
                    showRowLines: "showRowLines",
                    showBorders: "showBorders",
                    rowAlternationEnabled: "rowAlternationEnabled"
                },
                columnAutoWidth: true,
                columns: [{
                    dataField: "filename",
                    caption: "ไฟล์",
                    cellTemplate: function (container, item) {
                        var data = item.data,
                            markup = "<a >" + data.filename + "</a>";
                        container.append(markup);
                    },

                }],

            };
            $scope.Header = data.data.Results.Header[0]
            $scope.Document_Vnos = data.data.Results.Document_Vnos[0].Column1
            $rootScope.Customer = data.data.Results.Customer;


            $rootScope.Customer.splice(0, 0, {
                "ID": 0,
                "Name": '- กรุณาเลือก -',
            });
            $("#Customer").dxLookup({
                dataSource: $rootScope.Customer,
                displayExpr: 'Name',
                valueExpr: 'ID',
                showPopupTitle: false,
                showCancelButton: false,
                placeholder: 'เลือก',
                closeOnOutsideClick: true,
                searchEnabled: true,
                searchPlaceholder: 'ค้นหา',
                value: $scope.Document_Cus,
                onSelectionChanged: function (data) {
                    console.log(data);
                    $scope.Document_Cus = data.selectedItem.ID;

                }
            });


        });
        var formatNumber = function (num) {
            return num.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, '$1,')
        }

        var onCellClickViewFile = function (e) {
            window.open("/Upload/" + e.data.path, "popup", "width=800,height=600,left=300,top=200");
        };
        var formatNumber = function (num) {
            return num.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, '$1,')
        }

        $scope.SaveDocuments = function () {

            if ($scope.Document_Cus == 0)
            {
                swal({
                    title: 'Information',
                    text: "กรุณาเลือกรหัสเจ้าหนี้ก่อนบันทึกข้อมูล",
                    type: "info",
                    showCancelButton: false,
                    confirmButtonColor: "#6EAA6F",
                    confirmButtonText: 'OK'
                })}
            else {

                var Header = {
                    "Document_Id": $scope.Document_ID,
                    "Document_Group": $scope.Header.Document_Group,
                    "Document_Category": $scope.Header.Document_Category,
                    "Document_Objective": $scope.Header.Document_Objective,
                    "Document_Vnos": '',
                    "Document_Means": $scope.Header.Document_Means,
                    "Document_Expect": $scope.Header.Document_Expect,
                    "Document_Cus": $scope.Document_Cus,
                    "Document_Job": $scope.Header.Document_Job,
                    "Document_Dep": $scope.Header.Document_Dep,
                    "Document_Per": '',
                    "Document_Doc": '',
                    "Document_Mec": '',
                    "Document_Desc": $scope.Header.Document_Desc,
                    "Document_Tel": $scope.Header.Document_Tel,
                    "Document_CreateUser": localStorage.getItem('StaffID'),
                    "Document_Term": $scope.Header.Document_Term,
                    "Document_Project": $scope.Header.Document_Project

                };
                $http.post("api/PO/SavePOData/" + $stateParams.id + "?", Header).then(function successCallback(response) {
                    console.log(response);
                    window.location = '#/PurchaseOrder/ListPurchaseOrder';
                });


            }


        };
        $scope.CancelDocuments = function () {

            $http.get("api/PO/CancelPOTmpDetail/" + $stateParams.id).then(function (data) {
                console.log(data);
                window.location = '#/PurchaseOrder/ListPurchaseOrder';
            });
        };

    }
])