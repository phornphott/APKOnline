angular.module('ApkApp').controller('EditPurchaseRequestController', ['$scope', '$stateParams', '$http', '$rootScope', '$filter',
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
        $scope.detailorg = [];
        $scope.listupdate = [];
        $scope.listfiledelete = [];
        $scope.isapprove = false;
        $scope.tmpfolder = Math.floor(Math.random() * 1000000);
        var d = new Date()
        $scope.DocDate = d.getDate() + '-' + (d.getMonth() + 1) + '-' + d.getFullYear();
        $http.get("api/PR/ViewPRData/" + $scope.Document_ID + "?staffid=" + localStorage.getItem('StaffID')).then(function (data) {
            console.log(data);
            $scope.Header = data.data.Results.Header[0];
            var FileUpload = data.data.Results.FileUpload; 
            var Detail = data.data.Results.Detail;
            $scope.detailorg = data.data.Results.Detail;
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
                columnAutoWidth: true,
                editing: {
                    mode: "popup",
                    allowUpdating: true,
                    allowDeleting: true,
                    allowAdding: false,
                    texts: {
                        //editRow: "แก้ไข",
                        //saveRowChanges: "บันทึก",
                        //cancelRowChanges: "ยกเลิก"
                    },
                    popup: {
                        title: "รายการขออนุมัติ",
                        showTitle: true,
                        width: 700,
                        height: 525,
                        position: {
                            my: "top",
                            at: "top",
                            of: window
                        }
                    },
                    form: {

                        items: ["Document_Detail_Acc", "Document_Detail_Acc_Desc", "Document_Detail_Quan", "Document_Detail_UnitPrice"]
                    }
                },
                columns: [{
                    dataField: "Document_Detail_Acc",
                    caption: "รหัสบัญชี",
                    //editorOptions: {
                    //    disabled: true
                    //},
                    lookup: {
                        dataSource: $rootScope.Account,
                        displayExpr: "Name",
                        valueExpr: "Code" ,
                    }
                }, {
                    dataField: "Document_Detail_Acc_Desc",
                    caption: "รายละเอียดสินค้า" ,
                    // editorOptions: {
                    //    disabled: true
                    //}
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
                onRowUpdated: function (e) {
                    console.log(e);

                    var detail = {
                        "Document_Detail_Hid": $scope.Document_ID,
                        "Document_Detail_Id": e.key.Document_Detail_Id,
                        "Document_Detail_Acc": e.key.Document_Detail_Acc,
                        "Document_Detail_Acc_Desc": e.key.Document_Detail_Acc_Desc,
                        "Document_Detail_Quan": e.key.Document_Detail_Quan,
                        "Document_Detail_UnitPrice": e.key.Document_Detail_UnitPrice,
                        "Document_Detail_CreateUser": localStorage.getItem('StaffID'),

                    };
                    $scope.listupdate.push(detail);

                    //var grid = $("#gridContainer").dxDataGrid("instance");
                    //var index = e.row.rowIndex;
                    var qty = e.key.Document_Detail_Quan;
                    var unitprice = e.key.Document_Detail_UnitPrice;

                    result = qty * unitprice;
                    result = parseFloat(result).toFixed(2);

                    e.key.Document_Detail_Cog = result;


                },
                onRowRemoved: function (e) {
                    $http.post("api/PR/DeletePRDetail/" + e.key.Document_Detail_Id).then(function successCallback(response) {

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
                        $scope.detailCount = $scope.detailCount + 1;
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


                    //if (e.parentType == 'dataRow' && e.dataField == 'Document_Detail_Quan') {
                    //    $(e.editorElement).dxTextBox("instance").on("keyPress", function (args) {


                    //        var event = args.jQueryEvent;
                    //        console.log(event);
                    //        if (event.which != 8 && event.which != 46 && event.which != 0 && (event.which < 48 || event.which > 57)) {
                    //            event.stopPropagation();
                    //            event.preventDefault();

                    //        }
                    //    });


                    //    $(e.editorElement).dxTextBox("instance").on("valueChanged", function (args) {
                    //        var grid = $("#gridContainer").dxDataGrid("instance");
                    //        var index = e.row.rowIndex;
                    //        var data = grid.cellValue(index, "Document_Detail_UnitPrice");
                    //        var result = 0;
                    //        if (data == undefined) {
                    //            result = 0;

                    //        }
                    //        else {
                    //            result = args.value * data;
                    //            result = parseFloat(result).toFixed(2);
                    //        }

                    //        var amount = args.value;
                    //        grid.cellValue(index, "Document_Detail_Quan", amount);
                    //        grid.cellValue(index, "Document_Detail_Cog", result);

                    //    });
                    //}
                    //else if (e.parentType == 'dataRow' && e.dataField == "Document_Detail_UnitPrice") {
                    //    $(e.editorElement).dxTextBox("instance").on("keyPress", function (args) {

                    //        var event = args.jQueryEvent;
                    //        console.log(event);
                    //        if (event.which != 8 && event.which != 46 && event.which != 0 && (event.which < 48 || event.which > 57)) {
                    //            event.stopPropagation();
                    //            event.preventDefault();

                    //        }
                    //    });
                    //    $(e.editorElement).dxTextBox("instance").on("valueChanged", function (args) {
                    //        var grid = $("#gridContainer").dxDataGrid("instance");
                    //        var index = e.row.rowIndex;
                    //        var data = grid.cellValue(index, "Document_Detail_Quan");
                    //        var result = 0;
                    //        if (data == undefined) {
                    //            result = 0;

                    //        }
                    //        else {
                    //            result = args.value * data;
                    //            result = parseFloat(result).toFixed(2);
                    //        }

                    //        var amount = parseFloat(args.value).toFixed(2);
                    //        grid.cellValue(index, "Document_Detail_UnitPrice", amount);
                    //        grid.cellValue(index, "Document_Detail_Cog", result);

                    //    });
                    //}
                },
            };
            $scope.datafileGridOptions = {
                dataSource: FileUpload,
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
                }, {
                        dataField: "filename",
                        caption: "ลบไฟล์",
                        alignment: 'center',
                        allowFiltering: false,
                        width: 100,                                                                                    
                        cellTemplate: function (container, options) {
                            //if (options.key.Document_Status == 0) {

                                $("<div />").dxButton({
                                    icon: 'fa fa-trash',
                                    type: 'danger',
                                    disabled: false,
                                    onClick: function (e) {
                                        var r = confirm("ต้องการลบไฟล์ใช่หรือไม่ !!!");
                                        if (r === true) {
                                            console.log(FileUpload);
                                            $scope.listfiledelete.push($scope.Document_ID + '/' + options.key.filename);
                                            for (var i = FileUpload.length; i--;) {
                                                if (FileUpload[i].filename === options.key.filename) FileUpload.splice(i, 1);
                                            }

                                            console.log($scope.listfiledelete);
                                            $("#gridfileContainer").dxDataGrid({ dataSource: FileUpload});
                                            //$("#gridfileContainer").dxDataGrid("instance").dataSource = FileUpload;
                                            $("#gridfileContainer").dxDataGrid("instance").refresh();

                                        }
                                    }
                                }).appendTo(container);
                            //}
                        }
                }],

            };

        });
        var formatNumber = function (num) {
            return num.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, '$1,')
        }
        var onCellClickViewFile = function (e) {
            if (e.columnIndex == 0) {                                                     ลลลลลล 
                window.open("/Upload/" + e.data.path, "popup", "width=800,height=600,left=300,top=200");
            }
        };
        $scope.ListFilePR = [{}];

        $scope.addFilePR = function () {
            $scope.ListFilePR.push({});
        };
        $scope.CancelDocuments = function () {

            if ($scope.listfiledelete.length > 0 || $scope.listupdate.length > 0) {
                swal({
                    title: 'Information',
                    text: "มีการแก้ไขข้อมูลที่ยังไม่ได้บันทึก ต้องการยกเลิกการแก้ไขข้อมูล",
                    icon: "info",
                    buttons: true,
                    dangerMode: false,
                })
                    .then((willSave) => {
                        console.log(willSave)
                        if (willSave) {
                            window.location = '#/PurchaseRequest/ListPurchaseRequest';
                        }
                    });
            }
            else {
                window.location = '#/PurchaseRequest/ListPurchaseRequest';
            }
        };
        $scope.SaveDocuments = function () {
            //if ($scope.listupdate.length > 0 || $scope.listfiledelete > 0) {

                swal({
                    title: "ยืนยันการบันทึกข้อมูล?",
                    text: "ต้องการบันทึกข้อมูลรายการขอซื้อ!",
                    icon: "info",
                    buttons: true,
                    dangerMode: false,
                })
                    .then((willSave) => {
                        console.log(willSave)
                        if (willSave) {
                            $http.post("api/PR/updateFileEdit?docid=" + $scope.Document_ID + "&tmpupload=" + $scope.tmpfolder, $scope.listfiledelete).then(function successCallback(response) {

                                $http.get("api/PR/GetFileUpload/" + $scope.Document_ID + "?").then(function (data) {
                                    $scope.files = [];
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

                                    $("#gridfileContainer").dxDataGrid({
                                        dataSource: data.data.Results.FileUpload
                                    });
                                    $("#gridfileContainer").dxDataGrid("instance").refresh();
                                });
                            });
                            if ($scope.listupdate.length > 0) {
                                $http.post("api/PR/UpdateEditPRDetail?", $scope.listupdate).then(function successCallback(response) {
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
                                    } else {


                                        swal("บันทึกข้อมูลสำเร็จ", {
                                            icon: "success",
                                        });
                                        $scope.listupdate = [];
                                        $http.get("api/PR/PRDetailData/" + $scope.Document_ID + "?type=1").then(function (data) {
                                            console.log(data.data.Results.Detail);
                                            console.log(data);
                                            if (data.data.StatusCode > 1) {
                                                swal({
                                                    title: 'Information',
                                                    text: response.data.Messages,
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

                                    }

                                });
                            }
                        }
                    });

              
            //}
            //else {
            //    swal("ไม่มีการแก้ไขรายการขออนุมัติ");
            //}



        };
        $scope.removeFilePR = function (index) {
            $scope.ListFilePR.splice(index, 1);
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
                console.log(data);
                swal({
                    title: 'info',
                    text: data.data.Messages,
                    type: "info",
                    showCancelButton: false,
                    confirmButtonColor: "#6EAA6F",
                    confirmButtonText: 'OK'
                })
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
                if (response.data.StatusCode == 1) {
                    $scope.files = tmpfile;
                }
                swal({
                    title: 'Information',
                    text: data.Messages,
                    type: "info",
                    showCancelButton: false,
                    confirmButtonColor: "#6EAA6F",
                    confirmButtonText: 'OK'
                })
                // this callback will be called asynchronously
                // when the response is available
            }, function errorCallback(response) {
            });
        }
    }
])