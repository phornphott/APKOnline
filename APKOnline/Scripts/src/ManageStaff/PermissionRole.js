angular.module('ApkApp').controller('PermissionRoleController', ['$scope', '$stateParams', '$http', '$rootScope', '$filter',
    function ($scope, $stateParams, $http, $rootScope, $filter) {
        $scope.data = {};
        $scope.Authorize = {};
        $scope.USER = {};
        $scope.DEP = {};
        $scope.POS = {};
        $("#showAdd").hide();

        $scope.CallData = function () {
            $("#btnEditATH").hide();
            $("#btnAddATH").hide();
            $("#showAdd").hide();
            $("#btnAdd").show();
            $("#gridContainer").show();

            var api = "api/Staffs/PermissionRoleData"
            $http.get(api).then(function (data) {

                console.log(data);

                $scope.dataGridOptions = {
                    dataSource: data.data.Results.StaffAuthorizeData,
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
                        dataField: "ID",
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
                        dataField: "StaffCode",
                        caption: "รหัสพนักงาน",

                    }, {
                        dataField: "DEPdescT",
                        caption: "แผนก"
                    }, {
                        dataField: "PositionCode",
                        caption: "ตำแหน่ง"
                    }, {
                        dataField: "PositionLimit",
                        caption: "ยอดอนุมัติ",
                        dataType: "number",
                        format: "currency",
                        alignment: "right",
                    }, {
                        dataField: "isPreview",
                        caption: "Preview",

                    }, {
                        dataField: "Authorizeid",
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
                                    $("#btnEditATH").show();
                                    $("#btnAddATH").hide();
                                    $("#btnAdd").hide();
                                    $("#gridContainer").hide();

                                    var api = "api/Staffs/PermissionRoleDataByID?Authorizeid=" + options.key.Authorizeid;

                                    $http.get(api)
                                        .then(function (data) {
                                            $scope.Authorize = data.data.Results.StaffAuthorizeData[0];
                                            console.log($scope.Authorize);
                                            if (data.data.StatusCode > 1) {
                                                DevExpress.ui.notify(data.data.Messages);
                                                $("#loadIndicator").dxLoadIndicator({
                                                    visible: false
                                                });
                                            } else {

                                                //  LoadGrid(data.data);
                                                var api = "api/Staffs/DepartmentRoleData"       //แผนก
                                                $http.get(api).then(function (data) {
                                                    $scope.DEP = data.data.Results.DepartmentData;
                                                    console.log($scope.DEP);
                                                })
                                                api = "api/Staffs/StaffRoleData"                //ตำแหน่ง
                                                $http.get(api).then(function (data) {
                                                    $scope.POS = data.data.Results.PositionData;
                                                    console.log($scope.POS);
                                                })
                                                api = "api/Staffs/StaffData"                //พนักงาน
                                                $http.get(api).then(function (data) {
                                                    $scope.USER = data.data.Results.StaffData;
                                                    console.log($scope.USER);
                                                    console.log($scope.Authorize);
                                                    $scope.LoadForm($scope.Authorize);
                                                })
                                                //console.log($scope.DEP);
                                                //console.log(data.data.Results.StaffData);
                                                //$scope.LoadForm(data.data.Results.StaffData[0]);

                                            }

                                        });
                                }
                            }).appendTo(container);
                        }

                    }, {
                        dataField: "Authorizeid",
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
                                    var r = confirm("ต้องการลบบทบาทหน้าที่นี้หรือไม่ !!!");
                                    if (r === true) {
                                        $http.post("api/Staffs/DeleteStaffAuthorize/" + options.key.Authorizeid).then(function successCallback(response) {

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
                                                $("#btnEditATH").hide();
                                                $("#btnAddATH").hide();
                                                $("#showAdd").hide();
                                                $("#btnAdd").show();
                                                $("#gridContainer").show();
                                                var api = "api/Staffs/PermissionRoleData"
                                                $http.get(api).then(function (data) {
                                                    var Datasource = data.data.Results.PositionData;
                                                    $("#gridContainer").dxDataGrid("instance").option("dataSource", Datasource);
                                                    $("#gridContainer").dxDataGrid("instance").refresh();
                                                });
                                            }

                                        });
                                    }
                                }
                            }).appendTo(container);
                        }
                    }],

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
                    dataField: "StaffID",
                    label: {
                        text: "พนักงาน",
                    },
                    editorType: "dxSelectBox",
                    editorOptions: {
                        items: $scope.USER,
                        valueExpr: 'StaffID',
                        displayExpr: 'StaffName',
                        searchEnabled: true,
                        disabled: false,
                    },
                    validationRules: [{
                        type: "required",
                        message: "โปรดระบุรหัสพนักงาน"
                    }]
                }, {
                    dataField: "DEPid",
                    label: {
                        text: "แผนก",
                    },
                    editorType: "dxSelectBox",
                    editorOptions: {
                        items: $scope.DEP,
                        valueExpr: 'DEPid',
                        displayExpr: 'DEPdescT',
                        searchEnabled: true,
                        disabled: false,
                    },
                    validationRules: [{
                        type: "required",
                        message: "โปรดระบุแผนก"
                    }]
                }, {
                    dataField: "PositionPermissionId",
                    label: {
                        text: "ตำแหน่ง",
                    },
                    editorType: "dxSelectBox",
                    editorOptions: {
                        items: $scope.POS,
                        valueExpr: 'Positionid',
                        displayExpr: 'PositionCode',
                        searchEnabled: true,
                        disabled: false,
                    },
                    validationRules: [{
                        type: "required",
                        message: "โปรดระบุตำแหน่ง"
                    }]
                },{
                    dataField: "PositionLimit",

                    label: {
                        text: "ยอดอนุมัติ",
                    },
                    editorOptions: {
                        disabled: false
                    },
                    validationRules: [{
                        type: "required",
                        message: "โปรดระบุยอดอนุมัติ"
                    }]
                },{
                        dataField: "AuthorizeLevel",

                    label: {
                        text: "ระดับ ",
                    },
                    editorType: "dxSelectBox",
                    editorOptions: {
                        items: [{
                            "ID": 2,
                            "NAME": "ระดับ 2"
                        }, {
                            "ID": 3,
                            "NAME": "ระดับ 3"
                        }, {
                            "ID": 4,
                            "NAME": "ระดับ 4"
                        }, {
                            "ID": 5,
                            "NAME": "ระดับ 5"
                        }],
                        valueExpr: 'ID',
                        displayExpr: 'NAME',
                        disabled: false

                    },
                },
                {
                    dataField: "isPreview",
                    label: {
                        text: "Preview",
                    },
                    editorType: "dxCheckBox",
                    editorOptions: {
                        disabled: false,
                        value: false,
                    },
                },

                ]
            });


        }

        $scope.showColumnLines = true;
        $scope.showRowLines = true;
        $scope.showBorders = true;
        $scope.rowAlternationEnabled = true;
       
        var api = "api/Staffs/PermissionRoleData"
        $http.get(api).then(function (data) {

            console.log(data);

            $scope.dataGridOptions = {
                dataSource: data.data.Results.StaffAuthorizeData,
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
                    dataField: "Authorizeid",
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
                    dataField: "StaffCode",
                    caption: "รหัสพนักงาน",

                }, {
                    dataField: "StaffName",
                    caption: "ชื่อพนักงาน",
                }, {
                    dataField: "DEPdescT",
                    caption: "แผนก"
                }, {
                    dataField: "PositionCode",
                    caption: "ตำแหน่ง"
                }, {
                    dataField: "PositionLimit",
                    caption: "ยอดอนุมัติ",
                    dataType: "number",
                    format: "currency",
                    alignment: "right",
                }, {
                    dataField: "AuthorizeLevel",
                    caption: "ระดับ",
                    alignment: "center",
                }, {
                    dataField: "isPreview",
                    caption: "Preview",                   
                   
                }, {
                    dataField: "Authorizeid",
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
                                $("#btnEditATH").show();
                                $("#btnAddATH").hide();
                                $("#btnAdd").hide();
                                $("#gridContainer").hide();

                                var api = "api/Staffs/PermissionRoleDataByID?Authorizeid=" + options.key.Authorizeid;

                                $http.get(api)
                                    .then(function (data) {
                                        $scope.Authorize = data.data.Results.StaffAuthorizeData[0];
                                        console.log($scope.Authorize);
                                        if (data.data.StatusCode > 1) {
                                            DevExpress.ui.notify(data.data.Messages);
                                            $("#loadIndicator").dxLoadIndicator({
                                                visible: false
                                            });
                                        } else {

                                            //  LoadGrid(data.data);
                                            var api = "api/Staffs/DepartmentRoleData"       //แผนก
                                            $http.get(api).then(function (data) {
                                                $scope.DEP = data.data.Results.DepartmentData;
                                                console.log($scope.DEP);
                                            })
                                            api = "api/Staffs/StaffRoleData"                //ตำแหน่ง
                                            $http.get(api).then(function (data) {
                                                $scope.POS = data.data.Results.PositionData;
                                                console.log($scope.POS);
                                                api = "api/Staffs/StaffData"                //พนักงาน
                                                $http.get(api).then(function (data) {
                                                    $scope.USER = data.data.Results.StaffData;
                                                    console.log($scope.USER);
                                                    console.log($scope.Authorize);
                                                    $scope.LoadForm($scope.Authorize);
                                                })
                                            })
                                            
                                            //console.log($scope.DEP);
                                            //console.log(data.data.Results.StaffData);
                                            //$scope.LoadForm(data.data.Results.StaffData[0]);

                                        }

                                    });
                            }
                        }).appendTo(container);
                    }

                }, {
                    dataField: "Authorizeid",
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
                                var r = confirm("ต้องการลบบทบาทหน้าที่นี้หรือไม่ !!!");
                                if (r === true) {
                                    $http.post("api/Staffs/DeleteStaffAuthorize/" + options.key.Authorizeid).then(function successCallback(response) {

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
                                            $("#btnEditATH").hide();
                                            $("#btnAddATH").hide();
                                            $("#showAdd").hide();
                                            $("#btnAdd").show();
                                            $("#gridContainer").show();
                                            var api = "api/Staffs/PermissionRoleData"
                                            $http.get(api).then(function (data) {
                                                var Datasource = data.data.Results.StaffAuthorizeData;
                                                $("#gridContainer").dxDataGrid("instance").option("dataSource", Datasource);
                                                $("#gridContainer").dxDataGrid("instance").refresh();
                                            });
                                        }

                                    });
                                }
                            }
                        }).appendTo(container);
                    }
                }],
               
            };
        })

        $scope.InsertAuthorize = function () {

            if ($("#form-container").dxForm("instance").validate().isValid) {
                var obj = $("#form-container").dxForm("instance").option('formData');
                var Autho = {
                    "Authorizeid": 0,
                    "StaffID": obj.StaffID,
                    "StaffCode": "",
                    "DEPid": obj.DEPid,
                    "DEPdescT": "",
                    "PositionPermissionId": obj.PositionPermissionId,
                    "PositionCode": "",
                    "PositionLimit": obj.PositionLimit,
                    "AuthorizeLevel": obj.AuthorizeLevel,
                    "isPreview": obj.isPreview,
                };
                $.post("api/Staffs/SetStaffAuthorize?", Autho

                )
                    .done(function (data) {

                        if (data.StatusCode > 1) {
                            $("#loadIndicator").dxLoadIndicator({
                                visible: false
                            });
                            DevExpress.ui.notify(data.Messages);
                        }
                        else {
                            //var api = "api/Staffs/DepartmentRoleData"
                            //$http.get(api).then(function (data) {
                            //    var Datasource = data.data.Results.DepartmentData;
                            //    $("#gridContainer").dxDataGrid("instance").option("dataSource", Datasource);
                            //    $("#gridContainer").dxDataGrid("instance").refresh();
                            //});
                            //$scope.CallData();
                            DevExpress.ui.notify(data.Messages);
                            $("#btnEditAHT").hide();
                            $("#btnAddAHT").hide();
                            $("#showAdd").hide();
                            $("#btnAdd").show();
                            $("#gridContainer").show();
                            var api = "api/Staffs/PermissionRoleData"
                            $http.get(api).then(function (data) {
                                var Datasource = data.data.Results.StaffAuthorizeData;
                                $("#gridContainer").dxDataGrid("instance").option("dataSource", Datasource);
                                $("#gridContainer").dxDataGrid("instance").refresh();
                            });
                        }

                    });

            }

        }

        $scope.SubmitEditAuthorize = function () {

            if ($("#form-container").dxForm("instance").validate().isValid) {
                var obj = $("#form-container").dxForm("instance").option('formData');
                var Autho = {
                    "Authorizeid": obj.Authorizeid,
                    "StaffID": obj.StaffID,
                    "StaffCode": "",
                    "DEPid": obj.DEPid,
                    "DEPdescT": "",
                    "PositionPermissionId": obj.PositionPermissionId,
                    "PositionCode": "",
                    "PositionLimit": obj.PositionLimit,
                    "AuthorizeLevel": obj.AuthorizeLevel,
                    "isPreview": obj.isPreview,
                };
                $.post("api/Staffs/SetStaffAuthorize?", Autho

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
                            $("#btnEditAHT").hide();
                            $("#btnAddAHT").hide();
                            $("#showAdd").hide();
                            $("#btnAdd").show();
                            $("#gridContainer").show();
                            var api = "api/Staffs/PermissionRoleData"
                            $http.get(api).then(function (data) {
                                var Datasource = data.data.Results.StaffAuthorizeData;
                                $("#gridContainer").dxDataGrid("instance").option("dataSource", Datasource);
                                $("#gridContainer").dxDataGrid("instance").refresh();
                            });
                        }

                    });
            }


        }

        $scope.Refresh = function () {
            $("#btnEditAHT").hide();
            $("#btnAddAHT").show();
            $("#showAdd").hide();
            $("#btnAdd").show();
            $("#gridContainer").show();

            $scope.LoadForm({});
        }

        $scope.Add = function () {

            var api = "api/Staffs/DepartmentRoleData"
            $http.get(api).then(function (data) {
                console.log(data);
                $scope.DEP = data.data.Results.DepartmentData;

                api = "api/Staffs/StaffRoleData"                //ตำแหน่ง
                $http.get(api).then(function (data) {
                    $scope.POS = data.data.Results.PositionData;
                    console.log($scope.POS);
                })
                api = "api/Staffs/StaffData"                //พนักงาน
                $http.get(api).then(function (data) {
                    $scope.USER = data.data.Results.StaffData;
                    console.log($scope.USER);
                    $("#showAdd").show();
                    $scope.LoadForm({});
                    $("#btnAddATH").show();
                    $("#btnEditATH").hide();
                    $("#btnAdd").hide();
                    $("#gridContainer").hide();
                })

                
            })
            console.log($scope.DEP);
        }
    }
])