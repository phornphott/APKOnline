angular.module('ApkApp').controller('PermissionStaffController', ['$scope', '$stateParams', '$http', '$rootScope', '$filter',
    function ($scope, $stateParams, $http, $rootScope, $filter) {
        $scope.data = {};
        $("#showAdd").hide();
        
        $scope.CallData = function () {
            $("#btnEditDEP").hide();
            $("#btnAddDEP").hide();
            $("#showAdd").hide();
            $("#btnAdd").show();
            $("#gridContainer").show();

            var api = "api/Staffs/DepartmentRoleData"
            $http.get(api).then(function (data) {
                var Datasource = data.data.Results.DepartmentData;

                $("#gridContainer").dxDataGrid("instance").refresh();

                $scope.dataGridOptions = {
                    dataSource: Datasource,
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
                    height: 100 + '%',
                    editing: {
                        mode: "row",
                        //allowUpdating: true,
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
                    }, {
                        dataField: "DEPcode",
                        caption: "รหัสแผนก",

                    }, {
                        dataField: "DEPdescT",
                        caption: "ชื่อแผนกไทย",
                    }, {
                        dataField: "DEPdescE",
                        caption: "ชื่อแผนกอังกฤษ",
                    }, {
                        dataField: "DEPid",
                        caption: "แก้ไขข้อมูล",
                        alignment: 'center',
                        allowFiltering: false,
                        width: 100,
                        cellTemplate: function (container, options) {
                            $("<div />").dxButton({
                                icon: 'fa fa-pencil',
                                type: 'default',
                                disabled: false,
                                onClick: function (e) {
                                    $("#showAdd").show();
                                    $("#btnEditDEP").show();
                                    $("#btnAddDEP").hide();
                                    $("#btnAdd").hide();
                                    $("#gridContainer").hide();

                                    var api = "api/Staffs/DepartmentRoleDataByID?DEPid=" + options.key.DEPid;

                                    $http.get(api)
                                        .then(function (data) {

                                            if (data.data.StatusCode > 1) {
                                                DevExpress.ui.notify(data.data.Messages);
                                                $("#loadIndicator").dxLoadIndicator({
                                                    visible: false
                                                });
                                            } else {

                                                //  LoadGrid(data.data);
                                                console.log(data.data.Results.DepartmentData);
                                                $scope.LoadForm(data.data.Results.DepartmentData[0]);

                                            }

                                        });
                                }
                            }).appendTo(container);
                        }

                    }
                    , {
                        dataField: "DEPid",
                        caption: "ลบข้อมูล",
                        alignment: 'center',
                        allowFiltering: false,
                        width: 100,
                        cellTemplate: function (container, options) {
                            $("<div />").dxButton({
                                icon: 'fa fa-trash',
                                type: 'default',
                                disabled: false,
                                onClick: function (e) {
                                    var r = confirm("ต้องการลบรัสแผนกนี้หรือไม่ !!!");
                                    if (r === true) {
                                        $http.post("api/Staffs/DeleteDepartment/" + e.key.DEPid).then(function successCallback(response) {

                                            if (response.data.StatusCode > 1) {
                                                $("#loadIndicator").dxLoadIndicator({
                                                    visible: false
                                                });
                                                DevExpress.ui.notify(data.Messages);

                                            } else {
                                                DevExpress.ui.notify(data.Messages);
                                                $("#loadIndicator").dxLoadIndicator({
                                                    visible: false
                                                });
                                                $scope.CallData();
                                            }

                                        });
                                    }
                                }
                            }).appendTo(container);
                        }

                    },
                    ],
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

                };
            })
        }

        $scope.LoadForm = function (data) {
            $("#form-container").dxForm({
                colCount: 1,
                width: 500,
                formData: data,
                showColonAfterLabel: true,
                showValidationSummary: true,
                items: [{
                    dataField: "DEPcode",
                    label: {
                        text: "รหัสแผนก",
                    },
                    editorOptions: {
                        disabled: false,
                        attr: { 'style': "text-transform: uppercase" },
                        Maxleght: 15,
                    },
                    validationRules: [{
                        type: "required",
                        message: "โปรดระบุ รหัสแผนก"
                    }]
                },
                {
                    dataField: "DEPdescT",
                    label: {
                        text: "ชื่อแผนกไทย"
                    },
                    editorOptions: {
                        disabled: false
                    },
                    validationRules: [{
                        type: "required",
                        message: "โปรดระบุ ชื่อแผนกไทย"
                    }]
                },
                {
                    dataField: "DEPdescE",

                    label: {
                        text: "ชื่อแผนกอังกฤษ",
                    },
                    editorOptions: {
                        disabled: false
                    }
                },


                ]
            });

        }


        //LoadForm(data);
        //CallData();

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
                height: 100 + '%',
                editing: {
                    mode: "row",
                    //allowUpdating: true,
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
                }, {
                    dataField: "DEPcode",
                    caption: "รหัสแผนก",

                }, {
                    dataField: "DEPdescT",
                    caption: "ชื่อแผนกไทย",
                }, {
                    dataField: "DEPdescE",
                    caption: "ชื่อแผนกอังกฤษ",
                }, {
                    dataField: "DEPid",
                    caption: "แก้ไขข้อมูล",
                    alignment: 'center',
                    allowFiltering: false,
                    width: 100,
                    cellTemplate: function (container, options) {
                        $("<div />").dxButton({
                            icon: 'fa fa-pencil',
                            type: 'default',
                            disabled: false,
                            onClick: function (e) {
                                $("#showAdd").show();
                                $("#btnEditDEP").show();
                                $("#btnAddDEP").hide();
                                $("#btnAdd").hide();
                                $("#gridContainer").hide();
                                
                                var api = "api/Staffs/DepartmentRoleDataByID?DEPid=" + options.key.DEPid;

                                $http.get(api)
                                    .then(function (data) {

                                        if (data.data.StatusCode > 1) {
                                            DevExpress.ui.notify(data.Messages);
                                            $("#loadIndicator").dxLoadIndicator({
                                                visible: false
                                            });
                                        } else {

                                            //  LoadGrid(data.data);
                                            console.log(data.data.Results.DepartmentData);
                                            $scope.LoadForm(data.data.Results.DepartmentData[0]);

                                        }

                                    });
                            }
                        }).appendTo(container);
                    }

                }
                , {
                    dataField: "DEPid",
                    caption: "ลบข้อมูล",
                    alignment: 'center',
                    allowFiltering: false,
                    width: 100,
                    cellTemplate: function (container, options) {
                        $("<div />").dxButton({
                            icon: 'fa fa-trash',
                            type: 'default',
                            disabled: false,
                            onClick: function (e) {
                                var r = confirm("ต้องการลบรัสแผนกนี้หรือไม่ !!!");
                                if (r === true) {
                                    $http.post("api/Staffs/DeleteDepartment/" + options.key.DEPid).then(function successCallback(response) {

                                        if (response.data.StatusCode > 1) {
                                            $("#loadIndicator").dxLoadIndicator({
                                                visible: false
                                            });
                                            DevExpress.ui.notify(response.data.Messages);

                                        } else {
                                            DevExpress.ui.notify(response.data.Messages);
                                            $("#loadIndicator").dxLoadIndicator({
                                                visible: false
                                            });
                                            //$scope.CallData();
                                            $("#btnEditDEP").hide();
                                            $("#btnAddDEP").hide();
                                            $("#showAdd").hide();
                                            $("#btnAdd").show();
                                            $("#gridContainer").show();
                                            var api = "api/Staffs/DepartmentRoleData"
                                            $http.get(api).then(function (data) {
                                                var Datasource = data.data.Results.DepartmentData;
                                                $("#gridContainer").dxDataGrid("instance").option("dataSource", Datasource);
                                                $("#gridContainer").dxDataGrid("instance").refresh();
                                            });
                                        }

                                    });
                                }
                            }
                        }).appendTo(container);
                    }

                },
                ],
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

            };
        })

        $scope.InsertDEP = function () {

            if ($("#form-container").dxForm("instance").validate().isValid) {
                var obj = $("#form-container").dxForm("instance").option('formData');
                var Dept = {
                    "DEPid": 0,
                    "DEPcode": obj.DEPcode,
                    "DEPdescT": obj.DEPdescT,
                    "DEPdescE": obj.DEPdescE,
                };
                $.post("api/Staffs/SetDepartmentData?", Dept

                )
                    .done(function (data) {

                        if (data.StatusCode > 1) {
                            $("#loadIndicator").dxLoadIndicator({
                                visible: false
                            });
                            DevExpress.ui.notify(data.Messages);
                        }
                        else{
                            //var api = "api/Staffs/DepartmentRoleData"
                            //$http.get(api).then(function (data) {
                            //    var Datasource = data.data.Results.DepartmentData;
                            //    $("#gridContainer").dxDataGrid("instance").option("dataSource", Datasource);
                            //    $("#gridContainer").dxDataGrid("instance").refresh();
                            //});
                            //$scope.CallData();
                            DevExpress.ui.notify(data.Messages);
                            $("#btnEditDEP").hide();
                            $("#btnAddDEP").hide();
                            $("#showAdd").hide();
                            $("#btnAdd").show();
                            $("#gridContainer").show();
                            var api = "api/Staffs/DepartmentRoleData"
                            $http.get(api).then(function (data) {
                                var Datasource = data.data.Results.DepartmentData;
                                $("#gridContainer").dxDataGrid("instance").option("dataSource", Datasource);
                                $("#gridContainer").dxDataGrid("instance").refresh();
                            });
                        }
                        
                    });

            }

        }

        $scope.SubmitEditDEP = function () {

            if ($("#form-container").dxForm("instance").validate().isValid) {
                var obj = $("#form-container").dxForm("instance").option('formData');
                var Dept = {
                    "DEPid": obj.DEPid,
                    "DEPcode": obj.DEPcode,
                    "DEPdescT": obj.DEPdescT,
                    "DEPdescE": obj.DEPdescE,
                };
                $.post("api/Staffs/SetDepartmentData?", Dept
      
                )
                    .done(function (data) {

                        if (data.StatusCode > 1) {
                            $("#loadIndicator").dxLoadIndicator({
                                visible: false
                            });
                            DevExpress.ui.notify(data.Messages);
                            //  Refresh();
                        } else {
                            
                            DevExpress.ui.notify(data.Messages);
                            $("#loadIndicator").dxLoadIndicator({
                                visible: false
                            });
                            //$scope.CallData();
                            $("#btnEditDEP").hide();
                            $("#btnAddDEP").hide();
                            $("#showAdd").hide();
                            $("#btnAdd").show();
                            $("#gridContainer").show();
                            var api = "api/Staffs/DepartmentRoleData"
                            $http.get(api).then(function (data) {
                                var Datasource = data.data.Results.DepartmentData;
                                $("#gridContainer").dxDataGrid("instance").option("dataSource", Datasource);
                                $("#gridContainer").dxDataGrid("instance").refresh();
                            });
                        }

                    });
            }


        }

        $scope.ShowForm = function (id) {

            $("#showAdd").show();
            $("#btnEditDEP").show();
            $("#btnAddDEP").hide();
            $("#btnAdd").hide();
            $("#gridContainer").hide();

            var url = "api/Staffs/DepartmentRoleData";

            $http.get(url).then(function (data) {
                console.log(data);
                if (data.StatusCode < 2) {
                    //  LoadGrid(data.data);

                    LoadForm(data.data.Results.DepartmentData);
                } else {
                    DevExpress.ui.notify(data.Messages);
                    $("#loadIndicator").dxLoadIndicator({
                        visible: false
                    });

                }

            });
        }

        $scope.Refresh = function () {
            $("#btnEditDEP").hide();
            $("#btnAddDEP").show();
            $("#showAdd").hide();
            $("#btnAdd").show();
            $("#gridContainer").show();

            $scope.LoadForm({});
        }

        $scope.Add = function () {
            $("#showAdd").show();
            $scope.LoadForm({})
            $("#btnAddDEP").show();
            $("#btnEditDEP").hide();
            $("#btnAdd").hide();
            $("#gridContainer").hide();
        }
    }
])







