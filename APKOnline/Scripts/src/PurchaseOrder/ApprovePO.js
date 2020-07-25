﻿angular.module('ApkApp').controller('ApprovePOController', ['$scope', '$stateParams', '$http', '$rootScope', '$filter',
    function ($scope, $stateParams, $http, $rootScope, $filter) {
        $scope.SaveText = "อนุมัติ";
        $scope.showColumnLines = true;
        $scope.showRowLines = true;
        $scope.showBorders = true;
        $scope.rowAlternationEnabled = true;
        console.log($stateParams);
        $http.get("api/PO/GETApprovePO/" + $stateParams.id + "?StaffID=" + localStorage.getItem("StaffID")).then(function (data) {
            console.log(data);
            //$scope.Header = data.data.Results.Document_Vnos[0].Column1
            $scope.Header = data.data.Results.Header[0]
            $scope.SaveText = $scope.Header.SaveText; 
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
                        caption: "จำนวน", 
                        dataType: "number",
                        format: "#,##0",
                }, {
                        dataField: "Document_Detail_UnitPrice",
                        caption: "ราคา/หน่วย",
                        format: "#,##0.00",
                }, {
                        dataField: "Document_Detail_Cog",
                        caption: "จำนวนเงิน",
                        format: "#,##0.00",
                        editorOptions: {
                            disabled: true
                        }
                }],
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

            $http.get("api/PO/CancelPOTmpDetail/" + $stateParams.id).then(function (data) {
                console.log(data);
                window.location = '#/PurchaseOrder/ListPOApprove';
            });
        };

    }
])