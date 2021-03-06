﻿angular.module('ApkApp').controller('EditPreviewPOController', ['$scope', '$stateParams', '$http', '$rootScope', '$filter',
    function ($scope, $stateParams, $http, $rootScope, $filter) {
        $scope.SaveText = "บันทึก";
        $scope.showColumnLines = true;
        $scope.showRowLines = true;
        $scope.showBorders = true;
        $scope.rowAlternationEnabled = true;
        console.log($stateParams);
        $scope.Document_Dep = 0;
        $scope.listupdate = [];
        $scope.btnsaveShow = false;
        $http.get("api/PO/GETApprovePO/" + $stateParams.id + "?StaffID=" + localStorage.getItem("StaffID")).then(function (data) {
            console.log(data);
            //$scope.Header = data.data.Results.Document_Vnos[0].Column1
            $scope.Header = data.data.Results.Header[0]
            $scope.btnsaveShow = $scope.Header.CheckPreview;
            //$scope.SaveText = $scope.Header.SaveText; 
            $scope.Document_Dep = $scope.Header.Document_Dep;
            var FileUpload = data.data.Results.FileUpload; 

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
                    //texts: {
                    //    editRow: "แก้ไข",
                    //    saveRowChanges: "บันทึก",
                    //    cancelRowChanges: "ยกเลิก"
                    //},

                },
                columnAutoWidth: true,
                columns: [{
                    dataField: "Document_Detail_Acc",
                    caption: "รหัสบัญชี"
                    ,
                    editorOptions: {
                        disabled: true
                    },

                }, {
                    dataField: "Document_Detail_Stk",
                        caption: "รหัสสินค้า",
                        editorOptions: {
                            disabled: true
                        },


                }, {
                        
                        dataField: "Document_Detail_Acc_Desc",
                        caption: "รายละเอียดสินค้า",
                        //editorOptions: {
                        //    disabled: true
                        //},

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
                onRowUpdated: function (e) {
                    console.log(e);

                    var detail = {
                        "Document_Detail_Hid": $stateParams.id,
                        "Document_Detail_Id": e.key.Document_Detail_Id,
                        "Document_Detail_Acc": e.key.Document_Detail_Acc,
                        "Document_Detail_Acc_Desc": e.key.Document_Detail_Acc_Desc,
                        "Document_Detail_Quan": e.key.Document_Detail_Quan,
                        "Document_Detail_UnitPrice": e.key.Document_Detail_UnitPrice,
                        "Document_Detail_EditUser": localStorage.getItem('StaffID'),
                        "Document_PreviewNote": "",
                    };
                    $scope.listupdate.push(detail);

                    var qty = e.key.Document_Detail_Quan;
                    var unitprice = e.key.Document_Detail_UnitPrice;

                    result = qty * unitprice;
                    result = parseFloat(result).toFixed(2);

                    e.key.Document_Detail_Cog = result;
                    console.log($scope.listupdate);
                    //$http.post("api/PO/UpdatePreviewDetailData/"+ $stateParams.id , detail).then(function successCallback(response) {

                    //    if (response.data.StatusCode > 1) {
                    //        swal({
                    //            title: 'Information',
                    //            text: data.Messages,
                    //            type: "info",
                    //            showCancelButton: false,
                    //            confirmButtonColor: "#6EAA6F",
                    //            confirmButtonText: 'OK'
                    //        })

                    //    }
                    //    //$http.get("api/PO/PRDetailData/" + $scope.Document_ID + "?type=1").then(function (data) {
                    //    //    console.log(data.data.Results.Detail);
                    //    //    console.log(data);
                    //    //    if (data.data.StatusCode > 1) {
                    //    //        swal({
                    //    //            title: 'Information',
                    //    //            text: data.Messages,
                    //    //            type: "info",
                    //    //            showCancelButton: false,
                    //    //            confirmButtonColor: "#6EAA6F",
                    //    //            confirmButtonText: 'OK'
                    //    //        })

                    //    //    }

                    //    //    $("#gridContainer").dxDataGrid({
                    //    //        dataSource: data.data.Results.Detail

                    //    //    });
                    //    //    $("#gridContainer").dxDataGrid("instance").refresh();
                    //    //});

                    //});


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

                }],

            };

        });
        var formatNumber = function (num) {
            return num.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, '$1,')
        }

        var onCellClickViewFile = function (e) {
            window.open("/Upload/" + e.data.path, "popup", "width=800,height=600,left=300,top=200");
        };

        $scope.SaveDocuments = function () {
            if ($scope.listupdate.length > 0) {

                swal({
                    title: "ยืนยันการบันทึกข้อมูล?",
                    text: "ต้องการบันทึกข้อมูลรายการสั่งซื้อ!",
                    icon: "info",
                    buttons: true,
                    dangerMode: false,
                })
                    .then((willSave) => {
                        console.log(willSave)
                        if (willSave) {

                            $http.post("api/PO/UpdatePreviewDetailData?", $scope.listupdate).then(function successCallback(response) {

                                if (response.data.StatusCode > 1) {
                                    swal({
                                        title: 'Information',
                                        text: data.Messages,
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
                                    $http.get("api/PO/PRDetailData/" + $stateParams.id + "?").then(function (data) {
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

                                }

                            });
                        }
                    });
            }
            else {
                swal("ไม่มีการแก้ไขรายการขออนุมัติ");
            }
        };
        $scope.CancelDocuments = function () {
            window.location = '#/PurchaseRequest/ListPreviewPurchaseRequest';
            //$http.get("api/PO/CancelPOTmpDetail/" + $stateParams.id).then(function (data) {
            //    console.log(data);
            //    window.location = '#/PurchaseOrder/ListPOApprove';
            //});
        };

    }
])