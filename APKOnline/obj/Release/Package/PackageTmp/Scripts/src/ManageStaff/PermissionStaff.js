angular.module('ApkApp').controller('PermissionStaffController', ['$scope', '$stateParams', '$http', '$rootScope', '$filter',
    function ($scope, $stateParams, $http, $rootScope, $filter) {

        $scope.showColumnLines = true;
        $scope.showRowLines = true;
        $scope.showBorders = true;
        $scope.rowAlternationEnabled = true;

        var api = "api/Staffs/DepartmentRoleData"
        $http.get(api).then(function (data) {
            console.log(data);
            $scope.dataGridOptions = {
                dataSource: data.data.Results.DepartmentData,
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
                    //allowDeleting: true,
                    //allowAdding: true,
                    texts: {
                        editRow: "แก้ไข",
                        deleteRow: "ลบ",
                        saveRowChanges: "บันทึก",
                        confirmDeleteTitle: "คุณแน่ใจหรือไม่?",
                        confirmDeleteMessage: "คุณไม่สามารถกู้ข้อมูลกลับมาได้!",
                        cancelRowChanges: "ยกเลิก"
                    }
                },
                columnAutoWidth: true,
                columns: [{
                    dataField: "DEPid",
                    caption: "ลำดับ",
                    width: 50,
                    alignment: "center",
                    editorOptions: {
                        disabled: true
                    },
                    cellTemplate: function (container, item) {
                        var data = item.data,
                            markup = "<p>" + (item.rowIndex + 1) + "</p>";
                        container.append(markup);
                    },
                    editorOptions: {
                        disabled: true
                    },
                }, {
                    dataField: "DEPcode",
                    caption: "รหัสแผนก",

                }, {
                    dataField: "DEPdescT",
                    caption: "ชื่อแผนก",
                    editorOptions: {
                        disabled: true
                    },

                }],

            };
        })
    }
])