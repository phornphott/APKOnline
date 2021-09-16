angular.module('ApkApp').controller('ApprovePRController', ['$scope', '$stateParams', '$http', '$rootScope', '$filter',
    function ($scope, $stateParams, $http, $rootScope, $filter) {
        console.log($stateParams);
        $scope.SaveText = "อนุมัติ";
        $scope.RejectText = 'ไม่อนุมัติ'
        $scope.showColumnLines = true;
        $scope.showRowLines = true;
        $scope.showBorders = true;
        $scope.rowAlternationEnabled = true;
        $scope.Document_Dep = 0;
        $scope.Document_Depid = 0;
        $scope.Document_Job = 0;
        $scope.Document_Category = 0;
        $scope.Document_Group = 0;
        $scope.Objective = 0;
        $scope.Document_ID = $stateParams.id 
        $scope.Document_Vnos = "";
        $scope.showpreview = 0;
        $scope.ispreview = false;
        var d = new Date()
        $scope.DocDate = d.getDate() + '-' + (d.getMonth() + 1) + '-' + d.getFullYear();
        $http.get("api/PR/ViewPRData/" + $scope.Document_ID + "?staffid=" + localStorage.getItem('StaffID') ).then(function (data) {
            console.log(data);
            $scope.Header = data.data.Results.Header[0];
            var FileUpload = data.data.Results.FileUpload; 
            $scope.SaveText = data.data.Results.Header[0].SaveText;
            if ($scope.SaveText == "อนุมัติ")
                $scope.showpreview = data.data.Results.Header[0].isPreview;
            else {
                $scope.showpreview = false;
            }
            $scope.Document_Depid = data.data.Results.Header[0].Document_Depid;
            console.log($scope.Document_Depid);
            var Detail = data.data.Results.Detail;
            $rootScope.Account = data.data.Results.Account;
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
                //editing: {
                //    mode: "row",
                //    allowUpdating: true,
                //    allowAdding: true,
                //    texts: {
                //        editRow: "แก้ไข",
                //        saveRowChanges: "บันทึก",
                //        cancelRowChanges: "ยกเลิก"
                //    }
                //},
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
                //onContentReady: function (e) {

                //    //e.component.selectRowsByIndexes(rowSelectIndex);
                //},
                onRowInserted: function (e) {
                    var detail = {
                        "Document_Detail_Hid": $scope.Document_ID,
                        "Document_Detail_Id": 0,
                        
                        "Document_Detail_Vnos":'',
                        "Document_Detail_Acc": e.key.Document_Detail_Acc,
                        "Document_Detail_Acc_Desc": e.key.Document_Detail_Acc_Desc,
                        "Document_Detail_Quan": e.key.Document_Detail_Quan,
                        "Document_Detail_UnitPrice": e.key.Document_Detail_UnitPrice,
                        "Document_Detail_CreateUser": localStorage.getItem('StaffID'),
                       
                    };
                    console.log(detail);
                    $http.post("api/PR/AddPRDetail?", detail).then(function successCallback(response) {
                       
                        if (response.data.StatusCode > 1) {
                            swal({
                                title: 'Information',
                                text: data.Messages,
                                type: "info",
                                showCancelButton: false,
                                confirmButtonColor: "#6EAA6F",
                                confirmButtonText: 'OK'
                            })
                           
                        }
                        $http.get("api/PR/PRDetailData/" + $scope.Document_ID + "?type=0").then(function (data) {
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

                            $("#gridContainer").dxDataGrid({ dataSource: data.data.Results.Detail });
                            $("#gridContainer").dxDataGrid("instance").refresh();
                        });

                    });             
                },
                onEditorPrepared: function (e) {
              
                    if (e.dataField == "Document_Detail_Quan") {
                        $(e.editorElement).dxTextBox("instance").on("valueChanged", function (args) {
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
                        $(e.editorElement).dxTextBox("instance").on("valueChanged", function (args) {
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
                //},
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
        });
        $scope.ListFilePR = [{}];
        $scope.checkpreview = {
            value: $scope.ispreview,
            text: "Preview",
            onValueChanged: function (e) {
                $scope.ispreview = e.value;
            }
        };
        $scope.addFilePR = function () {
            $scope.ListFilePR.push({});
        };
        $scope.SaveDocuments = function () {

            swal({
                title: "ยืนยันการอนุมัติเอกสาร",
                text: "ต้องการอนุมัติรายการขอซื้อ!",
                icon: "info",
                buttons: true,
                dangerMode: false,
            })
                .then((willSave) => {
                    console.log(willSave)
                    if (willSave) {
                        var Header = {
                            "Document_Id": $scope.Document_ID,
                            "Document_Depid": $scope.Document_Depid,
                            "isPreview": $scope.ispreview,
                            "Document_CreateUser": localStorage.getItem('StaffID'),
                        };
                        console.log(Header);
                        $http.post("api/PR/ApprovePRData?", Header).then(function successCallback(response) {
                            console.log(response);
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
                            else { window.location = '#/PurchaseRequest/ListPRApprove'; }
                            //
                        });
                    }
                });
            

        };
        $scope.CancelDocuments = function () {

           
                    window.location = '#/PurchaseRequest/ListPRApprove';
               
        };
        $scope.RejectDocuments = function () {
            swal({
                title: "ยืนยันไม่อนุมัติเอกสาร",
                text: "ไม่อนุมัติรายการขอซื้อ!",
                icon: "info",
                buttons: true,
                dangerMode: false,
            })
                .then((willSave) => {
                    console.log(willSave)
                    if (willSave) {
                        var Header = {
                            "Document_Id": $scope.Document_ID,
                            "Document_Depid": $scope.Document_Depid,
                            "isPreview": $scope.ispreview,
                            "Document_CreateUser": localStorage.getItem('StaffID'),
                        };
                        console.log(Header);
                        $http.post("api/PR/RejectPRData?", Header).then(function successCallback(response) {
                            console.log(response);
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
                            else { window.location = '#/PurchaseRequest/ListPRApprove'; }
                            //
                        });
                    }
                });
        };
        $scope.removeFilePR = function (index) {
            $scope.ListFilePR.splice(index, 1);
        };
        var formatNumber = function (num) {
            return num.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, '$1,')
        }
        var onCellClickViewFile = function (e) {
            window.open("/Upload/" + e.data.path, "popup", "width=800,height=600,left=300,top=200");
        };

    }
])