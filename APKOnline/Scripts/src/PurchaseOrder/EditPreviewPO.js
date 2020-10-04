angular.module('ApkApp').controller('EditPreviewPOController', ['$scope', '$stateParams', '$http', '$rootScope', '$filter',
    function ($scope, $stateParams, $http, $rootScope, $filter) {
        $scope.SaveText = "บันทึก";
        $scope.showColumnLines = true;
        $scope.showRowLines = true;
        $scope.showBorders = true;
        $scope.rowAlternationEnabled = true;
        console.log($stateParams);
        $scope.Document_Dep = 0;
        $http.get("api/PO/GETApprovePO/" + $stateParams.id + "?StaffID=" + localStorage.getItem("StaffID")).then(function (data) {
            console.log(data);
            //$scope.Header = data.data.Results.Document_Vnos[0].Column1
            $scope.Header = data.data.Results.Header[0]
            $scope.SaveText = $scope.Header.SaveText; 
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
                        editorOptions: {
                            disabled: true
                        },

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
                        "Document_Detail_Hid": $scope.Document_ID,
                        "Document_Detail_Id": e.key.Document_Detail_Id,
                        "Document_Detail_Acc": e.key.Document_Detail_Acc,
                        "Document_Detail_Acc_Desc": e.key.Document_Detail_Acc_Desc,
                        "Document_Detail_Quan": e.key.Document_Detail_Quan,
                        "Document_Detail_UnitPrice": e.key.Document_Detail_UnitPrice,
                        "Document_Detail_EditUser": localStorage.getItem('StaffID'),

                    };
                    console.log(detail);
                    $http.post("api/PO/UpdatePreviewDetailData/"+ $stateParams.id , detail).then(function successCallback(response) {

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
                        $http.get("api/PO/PRDetailData/" + $scope.Document_ID + "?type=1").then(function (data) {
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
            
       
                var Header = {
                    "Document_Id": $stateParams.id,
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
                $http.post("api/PO/ApprovePOData", Header).then(function successCallback(response) {
                    console.log(response);
                    window.location = '#/PurchaseOrder/ListPOApprove';
                });
            




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