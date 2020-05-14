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
                height:100+'%',
                editing: {
                    mode: "popup",
                    allowUpdating: true,
                    allowDeleting: true,
                    allowAdding: true,
                    texts: {
                        editRow: "แก้ไข",
                        deleteRow: "ลบ",
                        saveRowChanges: "บันทึก",
                        confirmDeleteTitle: "คุณแน่ใจหรือไม่?",
                        confirmDeleteMessage: "คุณไม่สามารถกู้ข้อมูลกลับมาได้!",
                        cancelRowChanges: "ยกเลิก"
                    },
                    popup: {
                        title: "Department Info",
                        showTitle: true,
                        width: 700,
                        height: 300,
                        position: {
                            my: "top",
                            at: "top",
                            of: window
                        }
                    },
                    form: {
                        items: [{
                            itemType: "group",
                            colCount: 2,
                            colSpan: 2,
                            items: ["DEPcode", "DEPdescT"]
                        }]
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
                    }, {
                    dataField: "DEPcode",
                    caption: "รหัสแผนก",

                    }, {
                        dataField: "DEPdescT",
                        caption: "ชื่อแผนก",
                }],
                onRowInserted: function (e) {

                    var Dept = {
                        "DEPid": 0,
                        "DEPcode": e.key.DEPcode,
                        "DEPdescT": e.key.DEPdescT,
                        "DEPdescE": '',
                    };
           
                    $http.post("api/Staffs/SetDepartmentData?", Dept).then(function successCallback(response) {

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
                    });
                },
                onRowUpdated: function (e) {
                    var Dept = {
                        "DEPid": e.key.DEPid,
                        "DEPcode": e.key.DEPcode,
                        "DEPdescT": e.key.DEPdescT,
                        "DEPdescE": '',
                    };
        
                    $http.post("api/Staffs/SetDepartmentData?", Dept).then(function successCallback(response) {

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
                    });
                },
                onRowRemoved: function (e) {
                    $http.post("api/Staffs/DeleteDepartment/" + e.key.DEPid).then(function successCallback(response) {

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
                    });
                }
            };
        })
    }
])