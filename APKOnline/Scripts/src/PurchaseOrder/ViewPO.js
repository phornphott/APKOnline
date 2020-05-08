angular.module('ApkApp').controller('ViewPOController', ['$scope', '$stateParams', '$http', '$rootScope', '$filter',
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
                     format: '#,###'
                }, {
                        dataField: "Document_Detail_UnitPrice",
                        caption: "ราคา/หน่วย",
                        format:'#,###.##'
                }, {
                        dataField: "Document_Detail_Cog",
                        caption: "จำนวนเงิน",
                        format: '#,###.##',
                        editorOptions: {
                            disabled: true
                        }
                }],
            };
        });

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
                window.location = '#/PurchaseOrder/ListPurchaseOrder';
        };

    }
])