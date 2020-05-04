angular.module('ApkApp').controller('ApprovePROverController', ['$scope', '$stateParams', '$http', '$rootScope', '$filter',
    function ($scope, $stateParams, $http, $rootScope, $filter) {
        console.log($stateParams);
        $scope.showColumnLines = true;
        $scope.showRowLines = true;
        $scope.showBorders = true;
        $scope.rowAlternationEnabled = true;
        $scope.Document_Dep = 0;
        $scope.Document_Job = 0;
        $scope.Document_Category = 0;
        $scope.Document_Group = 0;
        $scope.Objective = 0;
        $scope.Document_ID = $stateParams.id 
        $scope.Document_Vnos = "";
        var d = new Date()
        $scope.DocDate = d.getDate() + '-' + (d.getMonth() + 1) + '-' + d.getFullYear();
        $http.get("api/PR/ViewPRData/" + $scope.Document_ID + "?staffid=" + localStorage.getItem('StaffID')).then(function (data) {
            console.log(data);
            $scope.Header = data.data.Results.Header[0];
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
                                title: "info",
                                text: data.Messages,
                                type: "info",
                                showCancelButton: false,
                                confirmButtonColor: "#6EAA6F",
                                confirmButtonText: "OK"
                            }, function () {
                            })
                           
                        }
                        $http.get("api/PR/PRDetailData/" + $scope.Document_ID + "?type=0").then(function (data) {
                            console.log(data);
                            if (data.data.StatusCode > 1) {
                                swal({
                                    title: "info",
                                    text: data.Messages,
                                    type: "info",
                                    showCancelButton: false,
                                    confirmButtonColor: "#6EAA6F",
                                    confirmButtonText:"OK"
                                }, function () {
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
        });
        $scope.ListFilePR = [{}];

        $scope.addFilePR = function () {
            $scope.ListFilePR.push({});
        };
        $scope.SaveDocuments = function () {
          
            
                var Header = {
                    "Document_Id": $scope.Document_ID,
                    //"Document_Group": $scope.Document_Group,
                    //"Document_Category": $scope.Document_Category,
                    //"Document_Objective": $scope.Objective,
                    //"Document_Vnos": '',
                    //"Document_Means": $scope.Document_Means,
                    //"Document_Expect": $scope.Document_Expect,
                    //"Document_Cus": '',
                    //"Document_Job": $scope.Document_Job,
                    //"Document_Dep": $scope.Document_Dep,
                    //"Document_Per": '',
                    //"Document_Doc": '',
                    //"Document_Mec": '',
                    //"Document_Desc": '',
                    //"Document_Tel": $scope.Document_Tel,
                    "Document_CreateUser": localStorage.getItem('StaffID'),

                };
            $http.post("api/PR/ApprovePROverBudget?", Header).then(function successCallback(response) {
                    console.log(response);
                window.location = '#/PurchaseRequest/ListPROverBudgetApprove';
                });
            




        };
        $scope.CancelDocuments = function () {

           
            window.location = '#/PurchaseRequest/ListPROverBudgetApprove';
               
        };

        $scope.removeFilePR = function (index) {
            $scope.ListFilePR.splice(index, 1);
        };
    }
])