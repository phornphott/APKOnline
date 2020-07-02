angular.module('ApkApp').controller('ApprovePurchaseRequestController', ['$scope', '$stateParams', '$http', '$rootScope', '$filter',
    function ($scope, $stateParams, $http, $rootScope, $filter) {
        $scope.ListFilePR = [{}];
        $scope.files = []; 
        $scope.RadioValue=["PR","SR","CR"]
        $scope.TextSaveButon="บันทึก";
        $scope.showColumnLines = true;
        $scope.showRowLines = true;
        $scope.showBorders = true;
        $scope.rowAlternationEnabled = true;
        $scope.Document_Dep = 0;
        $scope.Document_Job = 0;
        $scope.Document_Category = 0;
        $scope.Document_Group = 0;
        $scope.Objective = 0;
        $scope.Document_ID = Math.floor(Math.random() * 10000); 
        $scope.tmpfolder=Math.floor(Math.random() * 1000000);
        $scope.Document_Vnos = "";
        $scope.ProjectSelect = "";
        $scope.radioGroup = {
            eventRadioGroupOptions: {
                items: $scope.RadioValue,
                value: $scope.RadioValue[0],
                layout: "horizontal",
                onValueChanged: function (e) {
                    console.log(e.value);
                    $scope.ProjectSelect = e.value;
                }
            }
        };
        var d = new Date()
        $scope.DocDate = d.getDate() + '-' + (d.getMonth() + 1) + '-' + d.getFullYear();
        $http.get("api/PR/PreparePageData/0?type=0").then(function (data) {
            $rootScope.DocumentGroup = data.data.Results.DocumentGroup;
            $rootScope.Category = data.data.Results.Category;
            $rootScope.Objective = data.data.Results.Objective;
            $rootScope.JOB = data.data.Results.JOB;
            $rootScope.Account = data.data.Results.Account;
            $rootScope.DEP = data.data.Results.DEP;
            

            var Detail = data.data.Results.Detail;
            $rootScope.DEP.splice(0, 0, {
                "ID": 0,
                "Name": '- กรุณาเลือก -',
            });
            $rootScope.Category.splice(0, 0, {
                "ID": 0,
                "Name": '- กรุณาเลือก -',
            });
            $rootScope.Objective.splice(0, 0, {
                "ID": 0,
                "Name": '- กรุณาเลือก -',
                "Objective_Category_Id": 0,
            });
            $rootScope.JOB.splice(0, 0, {
                "Code": '0',
                "Name": '- กรุณาเลือก -',
            });
            $rootScope.Account.splice(0, 0, {
                "Code": '0',
                "Name": '- กรุณาเลือก -',

            });
            $("#Dept").dxLookup({
                dataSource: $rootScope.DEP,
                displayExpr: 'Name',
                valueExpr: 'ID',
                showPopupTitle: false,
                showCancelButton: false,
                placeholder: 'เลือก',
                closeOnOutsideClick: true,
                searchEnabled: true,
                searchPlaceholder: 'ค้นหา',
                value: $scope.Document_Dep,
                onSelectionChanged: function (data) {
              
                    $scope.Document_Dep = data.selectedItem.ID;

                }
            }).dxLookup("instance");
            $("#Job").dxLookup({
                dataSource: $rootScope.JOB,
                displayExpr: 'Name',
                valueExpr: 'Code',
                showPopupTitle: false,
                showCancelButton: false,
                placeholder: 'เลือก',
                closeOnOutsideClick: true,
                searchEnabled: true,
                searchPlaceholder: 'ค้นหา',
                value: $scope.Document_Job,
                onSelectionChanged: function (data) {
                  
                    $scope.Document_Job = data.selectedItem.Code;

                }
            }).dxLookup("instance");
            $("#DocumentGroup").dxLookup({
                dataSource: $rootScope.DocumentGroup,
                displayExpr: 'Name',
                valueExpr: 'ID',
                showPopupTitle: false,
                showCancelButton: false,
                placeholder: 'เลือก',
                closeOnOutsideClick: true,
                searchEnabled: true,

                searchPlaceholder: 'ค้นหา',
                value: $scope.Document_Group,
                onSelectionChanged: function (data) {

                    $scope.Document_Group = data.selectedItem.ID;
                    $http.get("api/PR/GeneratePRNo/" + data.selectedItem.ID).then(function (data) {
                        $scope.Document_Vnos = data.data.Results.Document_Vnos[0].Column1;
                        //}
                    });




                }
            }).dxLookup("instance");
            $("#Objective").dxLookup({
                dataSource: $rootScope.Objective,
                displayExpr: 'Name',
                valueExpr: 'ID',
                showPopupTitle: false,
                showCancelButton: false,
                placeholder: 'เลือก',
                closeOnOutsideClick: true,
                searchEnabled: true,
                searchPlaceholder: 'ค้นหา',
                value: $scope.Objective,
                onSelectionChanged: function (data) {
                    
                    $scope.Objective = data.selectedItem.ID;

                }
            }).dxLookup("instance");
            $("#Category").dxLookup({
                dataSource: $rootScope.Category,
                displayExpr: 'Name',
                valueExpr: 'ID',
                showPopupTitle: false,
                showCancelButton: false,
                closeOnOutsideClick: true,
                searchEnabled: true,
                disabled: $scope.Disable,
                searchPlaceholder: 'ค้นหา',
                value: $scope.Document_Category,
                onSelectionChanged: function (data) {
                
                    $scope.Document_Category = data.selectedItem.ID;

                    $http.get("api/PR/OjectiveData/" + data.selectedItem.ID).then(function (data) {

                        var SelectObjective = data.data.Results.Objective;
                        $("#Objective").dxLookup({
                            dataSource: SelectObjective,
                            displayExpr: 'Name',
                            valueExpr: 'ID',
                            showPopupTitle: false,
                            showCancelButton: false,
                            placeholder: 'เลือก',
                            closeOnOutsideClick: true,
                            searchEnabled: true,
                            searchPlaceholder: 'ค้นหา',
                            value: 0,
                            onSelectionChanged: function (data) {
                                $scope.Objective = data.selectedItem.ID;

                            }
                        }).dxLookup("instance");
                    });
                }
            }).dxLookup("instance");

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
                    allowAdding: true,
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
                                title: 'Information',
                                text: data.Messages,
                                type: "info",
                                showCancelButton: false,
                                confirmButtonColor: "#6EAA6F",
                                confirmButtonText: 'OK'
                            }, function () {
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
                //onRowUpdated: function (e) {


                //    //var MaterialDocCountDaily = [{
                //    //    "DocumentID": e.key.DocumentID,
                //    //    "KeyShopID": e.key.KeyShopID,
                //    //    "DocDetailID": e.key.DocDetailID,
                //    //    "ProductAmount": e.key.StartProductAmount,
                //    //    "CurrentStockAmount": e.key.CurrentStockAmount,
                //    //    "UnitName": e.key.UnitName,
                //    //    "UnitSmallID": e.key.UnitSmallID,
                //    //    "UnitLargeID": e.key.UnitLargeID,
                //    //    "UnitRatio": e.key.UnitRatio,
                //    //    "UnitLargeRatio": e.key.UnitLargeRatio
                //    //}];

                //    //var checkStatusAdd = 0;
                //    //$http.post("api/CountWeekly/AddMaterails", MaterialDocCountDaily).success(function (data, header, status, config) {
                //    //    $rootScope.MaterialDocAll = data.Results;
                //    //    if (data.StatusCode > 1) {
                //    //        swal({
                //    //            title: 'Information',
                //    //            text: data.Messages,
                //    //            type: "info",
                //    //            showCancelButton: false,
                //    //            confirmButtonColor: "#6EAA6F",
                //    //            confirmButtonText: 'OK'
                //    //        }, function () {
                //    //        })
                //    //        checkStatusAdd = 1;
                //    //    }
                //    //    $.get("api/CountWeekly/LoadDocument?documentTypeId=" + Number(localStorage.getItem('DocumentTypeID')) + "&documentId=" + $stateParams.DocumentID + "&keyShopId=" + $stateParams.KeyShopID + "&documentStatus=" + $stateParams.DocumentStatus).success(function (data) {

                //    //        if (data.StatusCode > 1) {
                //    //            swal({
                //    //                title: 'Information',
                //    //                text: data.Messages,
                //    //                type: "info",
                //    //                showCancelButton: false,
                //    //                confirmButtonColor: "#6EAA6F",
                //    //                confirmButtonText: 'OK'
                //    //            }, function () {
                //    //            })
                //    //            checkStatusAdd = 1;
                //    //        }
                //    //        else {
                //    //            $scope.DocumentDetails = data.Results.DocumentDetails;
                //    //            $scope.checkApprove = data.Results.checkApprove[0].check;

                //    //        }
                //    //        if (checkStatusAdd == 1) {
                //    //            $("#gridContainers").dxDataGrid({ dataSource: $scope.DocumentDetails });
                //    //            $("#gridContainers").dxDataGrid("instance").refresh();
                //    //        }


                //    //        //if ($scope.DocumentDetails.length > 0) {
                //    //        //    $scope.DocDetailsCountStock = [];
                //    //        //    for (var a = 0; a < $scope.DocumentDetails.length; a++) {
                //    //        //        var pushData = $scope.DocumentDetails[a];
                //    //        //        if ($scope.DocumentDetails[a].ProductAmount == -1 && $scope.checkcopy == false) {
                //    //        //            pushData.DiffCurrentAmount = 0 - $scope.DocumentDetails[a].CurrentStockAmount;
                //    //        //            pushData.StartProductAmount = "";
                //    //        //            pushData.ProductAmountQty = 0;
                //    //        //            $scope.checkApprove = 0;
                //    //        //        }
                //    //        //        else if ($stateParams.DocumentStatus == 1 || $stateParams.DocumentStatus == 99) {
                //    //        //            pushData.DiffCurrentAmount = $scope.DocumentDetails[a].ProductAmount - $scope.DocumentDetails[a].CurrentStockAmount;
                //    //        //            pushData.StartProductAmount = numeral($scope.DocumentDetails[a].ProductAmount).format($scope.NumericFormat);
                //    //        //            pushData.ProductAmountQty = $scope.DocumentDetails[a].ProductAmount;
                //    //        //        }
                //    //        //        else {
                //    //        //            pushData.DiffCurrentAmount = $scope.DocumentDetails[a].DiffCurrentAmount;
                //    //        //            pushData.StartProductAmount = $scope.DocumentDetails[a].StartProductAmount;
                //    //        //            pushData.ProductAmountQty = $scope.DocumentDetails[a].ProductAmountQty;
                //    //        //        }

                //    //        //        $scope.DocDetailsCountStock.push(pushData);
                //    //        //    }
                //    //        //}



                //    //        //$("#gridContainers").dxDataGrid({ dataSource: $scope.DocumentDetails});
                //    //        //$("#gridContainers").dxDataGrid("instance").refresh();

                //    //        //$("#gridContainers").dxDataGrid("instance").selectRowsByIndexes([rowSelectIndex]);


                //    //    });

                //    //});
                //},
                //onRowRemoved: function (e) {
                //    logEvent("RowRemoved");
                //},
            };
        });
      

        $scope.addFilePR = function () {
            console.log($scope.ListFilePR);
            $scope.ListFilePR.push({});
        };

        $scope.SaveDocuments = function () {
            console.log($scope.ListFilePR);
          
            var checkselectValue = 0;
            
            //if ($scope.Document_Dep == 0) {
            //    checkselectValue = 1;
            //}
            ////if ($scope.Document_Job == 0) {
            ////    checkselectValue = 1;
            ////}
            //if ($scope.Document_Category == 0) {
            //    checkselectValue = 1;
            //}
            //if ($scope.Document_Group == 0) {
            //    checkselectValue = 1;
            //}
            //if ($scope.Objective == 0) {
            //    checkselectValue = 1;
            //}
            console.log($scope.Document_Dep );
            if ($scope.Document_Dep == 0) {
                swal({
                    title: 'info',
                    text: "กรุณาเลือกแผนกก่อนบันทึก",
                    type: "info",
                    showCancelButton: false,
                    confirmButtonColor: "#6EAA6F",
                    confirmButtonText: 'OK'
                }, function () {
                })
            } else {
                var Header = {
                    "Document_Id": $scope.Document_ID,
                    "Document_Group": $scope.Document_Group,
                    "Document_Category": $scope.Document_Category,
                    "Document_Objective": $scope.Objective,
                    "Document_Vnos": '',
                    "Document_Means": $scope.Document_Means,
                    "Document_Expect": $scope.Document_Expect,
                    "Document_Cus": '',
                    "Document_Job": $scope.Document_Job,
                    "Document_Dep": $scope.Document_Dep,
                    "Document_Per": '',
                    "Document_Doc": '',
                    "Document_Mec": '',
                    "Document_Desc": $scope.Document_Desc,
                    "Document_Tel": $scope.Document_Tel,
                    "Document_CreateUser": localStorage.getItem('StaffID'),
                    "folderUpload": $scope.tmpfolder,
                    "Document_Term": $scope.Document_Term,
                    "Document_Project": $scope.ProjectSelect

                };
                $http.post("api/PR/SavePRData?", Header).then(function successCallback(response) {

                    console.log(response);
                    window.location = '#/PurchaseRequest/ListPurchaseRequest';
                });


            }


        };

        $scope.CancelDocuments = function () {
            $scope.DeleteFile();
            $http.get("api/PR/CancelPRTmpDetail/" + $scope.Document_ID).then(function (data) {

                    window.location = '#/PurchaseRequest/ListPurchaseRequest';
                });
        };

        $scope.removeFilePR = function (index) {
            $scope.ListFilePR.splice(index, 1);
        };

        $scope.OpenUploadFile = function () {
            var data = "/UploadPage/popupUploadfile.aspx?Parameter=" + $scope.tmpfolder;
            window.open(data, "popup", "width=600,height=400,left=300,top=200");
            ////window.open('/UploadPage/popupUploadfile.aspx?Parameter=' + e.data.Document_Id, '_blank');

        }
        $scope.upload = function () {
            console.log($scope.files);
            alert($scope.files.length + " files selected ... Write your Upload Code");

        };

        $scope.UploadFiles = function (files) {
            console.log(files);
            $scope.SelectedFiles = files;
            if ($scope.SelectedFiles && $scope.SelectedFiles.length) {
                Upload.upload({
                    url: "/api/PR/UploadFiles?tmppath=" + $scope.tmpfolder,
                    data: {
                        files: $scope.SelectedFiles
                    }
                }).then(function (response) {
                    $timeout(function () {
                        $scope.Result = response.data;
                    });
                }, function (response) {
                    if (response.status > 0) {
                        var errorMsg = response.status + ': ' + response.data;
                        alert(errorMsg);
                    }
                }, function (evt) {
                    var element = angular.element(document.querySelector('#dvProgress'));
                    $scope.Progress = Math.min(100, parseInt(100.0 * evt.loaded / evt.total));
                    element.html('<div style="width: ' + $scope.Progress + '%">' + $scope.Progress + '%</div>');
                });
            }
        };
        $scope.DeleteFile = function () {

            $http.post("api/PR/DeleteFiles?tmppath=" + $scope.tmpfolder).then(function (data) {
                $scope.files = [];
            });
        };
        var formdata = new FormData();
        var tmpfile = [];
        $scope.getTheFiles = function ($files) {
            tmpfile = [];
            angular.forEach($files, function (value, key) {
                console.log(key);
                console.log(value);
                tmpfile.push(value);
                formdata.append(key, value);
            });
        };
        $scope.uploadFiles = function () {
            var url = "/api/PR/FileUpload?tmppath=" + $scope.tmpfolder;
            $scope.files = [];

            $http({
                url: url,
                method: "POST",
                data: formdata,
                async: false,
                crossDomain: true,
                processData: false,
                contentType: false,
                headers: {
                    'Content-Type': undefined
                }
            }).then(function successCallback(response) {
                console.log(response);
                if (response.data.StatusCode==1) {
                    $scope.files = tmpfile;
                }
                swal({
                    title: 'Information',
                    text: data.Messages,
                    type: "info",
                    showCancelButton: false,
                    confirmButtonColor: "#6EAA6F",
                    confirmButtonText: 'OK'
                }, function () {
                })
                // this callback will be called asynchronously
                // when the response is available
            }, function errorCallback(response) {
            });
        }
    }
])