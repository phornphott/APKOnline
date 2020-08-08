angular.module('ApkApp').controller('ManageStaffController', ['$scope', '$stateParams', '$http', '$rootScope', '$filter',
    function ($scope, $stateParams, $http, $rootScope, $filter) {
        $scope.data = {};
        $scope.DEP = {};
        $scope.Staff = {};
        $scope.DepartmentRole= "";
        $("#showAdd").hide();

        $scope.CallData = function () {
            $("#btnEditPER").hide();
            $("#btnAddPER").hide();
            $("#showAdd").hide();
            $("#btnAdd").show();
            $("#gridContainer").show();

            var api = "api/Staffs/StaffData"
            $http.get(api).then(function (data) {

                console.log(data);

                $scope.dataGridOptions = {
                    dataSource: data.data.Results.StaffData,
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
                        dataField: "StaffID",
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
                        dataField: "StaffFirstName",
                        caption: "ชื่อ"
                    }, {
                        dataField: "StaffLastName",
                        caption: "นามสกุล"
                    }, {
                        dataField: "StaffLogin",
                        caption: "Login",

                    }, {

                        caption: "กำหนดแผนก",
                        cellTemplate: function (container, item) {
                            //    console.log(item)
                            var data = item.data,
                                markup = "<a >กำหนดแผนก</a>";
                            container.append(markup);
                        },
                        alignment: "center",
                    }, {

                        caption: "กำหนดตำแหน่ง",
                        cellTemplate: function (container, item) {
                            //    console.log(item)
                            var data = item.data,
                                markup = "<a >กำหนดตำแหน่ง</a>";
                            container.append(markup);
                        },
                        alignment: "center",

                    }, {
                        dataField: "StaffID",
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
                                    $("#btnEditPER").show();
                                    $("#btnAddPER").hide();
                                    $("#btnAdd").hide();
                                    $("#gridContainer").hide();

                                    var api = "api/Staffs/StaffDataByID?StaffID=" + options.key.StaffID;

                                    $http.get(api)
                                        .then(function (data) {

                                            if (data.data.StatusCode > 1) {
                                                DevExpress.ui.notify(data.data.Messages);
                                                $("#loadIndicator").dxLoadIndicator({
                                                    visible: false
                                                });
                                            } else {

                                                //  LoadGrid(data.data);
                                                var api = "api/Staffs/DepartmentRoleData"
                                                $http.get(api).then(function (data) {
                                                    $scope.DEP = data.data.Results.DepartmentData;
                                                })
                                                console.log(data.data.Results.StaffData);
                                                $scope.LoadForm(data.data.Results.StaffData[0]);

                                            }

                                        });
                                }
                            }).appendTo(container);
                        }

                    }, {
                        dataField: "StaffID",
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
                                    var r = confirm("ต้องการลบรัสพนักงานนี้หรือไม่ !!!");
                                    if (r === true) {
                                        $http.post("api/Staffs/DeleteStaff/" + options.key.StaffID).then(function successCallback(response) {

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
                                                $("#btnEditPER").hide();
                                                $("#btnAddPER").hide();
                                                $("#showAdd").hide();
                                                $("#btnAdd").show();
                                                $("#gridContainer").show();
                                                var api = "api/Staffs/StaffData"
                                                $http.get(api).then(function (data) {
                                                    var Datasource = data.data.Results.StaffData;
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
            console.log($scope.DepartmentRole);
            //var listDEP = 0;
            //var api = "api/Staffs/DepartmentRoleData"
            //$http.get(api).then(function (data) {
            //    $scope.DEP = data.data.Results.DepartmentData;
            //})
            //console.log($scope.DEP);
            $("#form-container").dxForm({
                colCount: 1,
                width: 500,
                formData: data,
                showColonAfterLabel: true,
                showValidationSummary: true,
                items: [{
                    dataField: "StaffCode",
                    label: {
                        text: "รหัสพนักงาน",
                    },
                    editorOptions: {
                        disabled: false,
                        inputAttr: { 'style': "text-transform: uppercase" },
                        Maxleght: 15,
                    },
                    validationRules: [{
                        type: "required",
                        message: "โปรดระบุ รหัสพนักงาน"
                    }]
                },{
                    dataField: "StaffFirstName",
                    label: {
                        text: "ชื่อ"
                    },
                    editorOptions: {
                        disabled: false
                    },
                    validationRules: [{
                        type: "required",
                        message: "โปรดระบุ ชื่อพนักงาน"
                    }]
                },{
                    dataField: "StaffLastName",
                    label: {
                        text: "นามสกุล",
                    },
                    editorOptions: {
                        disabled: false
                    }
                }, {
                    dataField: "StaffPosition",
                    label: {
                        text: "ตำแหน่ง",
                    },
                    editorOptions: {
                        disabled: false
                    }
                },{
                    dataField: "StaffLogin",

                    label: {
                        text: "User Login",
                    },
                    editorOptions: {
                        disabled: false
                    }
                }, {
                    dataField: "StaffPassword",

                    label: {
                        text: "Password",
                    },
                    editorOptions: {
                        mode: "password",
                        disabled: false
                    },
                    validationRules: [{
                        type: "required",
                        message: "โปรดระบุ รหัสผ่าน"
                    }]
                },
                {
                    dataField: "StaffDepartmentID",
                    label: {
                        text: "แผนก",
                    },
                    editorType: "dxSelectBox",
                    editorOptions: {
                        items: $scope.DepartmentRole,
                        valueExpr: 'DEPid',
                        displayExpr: 'DEPdescT',
                        disabled: false,
                    },
                    validationRules: [{
                        type: "required",
                        message: "โปรดระบุแผนก"
                    }]
                }, 
                {
                    dataField: "StaffLevel",

                    label: {
                        text: "ระดับ ",
                    },
                    editorType: "dxSelectBox",
                    editorOptions: {
                        items: [{
                            "ID": 1,
                            "NAME": "ระดับ 1"
                        }, {
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
                }

                ]
            });
                    

        }

        $scope.showColumnLines = true;
        $scope.showRowLines = true;
        $scope.showBorders = true;
        $scope.rowAlternationEnabled = true;

        

        var api = "api/Staffs/StaffData"
        $http.get(api).then(function (data) {

            console.log(data);

            $scope.dataGridOptions = {
                dataSource: data.data.Results.StaffData,
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
                    dataField: "StaffID",
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
                    dataField: "StaffFirstName",
                    caption: "ชื่อ"
                }, {
                    dataField: "StaffLastName",
                    caption: "นามสกุล"
                }, {
                    dataField: "StaffPosition",
                    caption: "ตำแหน่ง"
                },{
                    dataField: "StaffLogin",
                    caption: "Login",
                    
                }, {
                    dataField: "StaffLevel",
                    caption: "ระดับ",
                    alignment: "center",
                },
                //{

                //    caption: "กำหนดแผนก",
                //    cellTemplate: function (container, item) {
                //        //    console.log(item)
                //        var data = item.data,
                //            markup = "<a >กำหนดแผนก</a>";
                //        container.append(markup);
                //    },
                //    alignment: "center",
                //}, {

                //    caption: "กำหนดตำแหน่ง",
                //    cellTemplate: function (container, item) {
                //        //    console.log(item)
                //        var data = item.data,
                //            markup = "<a >กำหนดตำแหน่ง</a>";
                //        container.append(markup);
                //    },
                //    alignment: "center",
                   
                //},
                {
                    dataField: "StaffID",
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
                                $("#btnEditPER").show();
                                $("#btnAddPER").hide();
                                $("#btnAdd").hide();
                                $("#gridContainer").hide();

                                var api = "api/Staffs/StaffDataByID?StaffID=" + options.key.StaffID;

                                $http.get(api)
                                    .then(function (data) {
                                        $scope.Staff = data.data.Results.StaffData[0];
                                        console.log($scope.Staff);
                                        if (data.data.StatusCode > 1) {
                                            DevExpress.ui.notify(data.data.Messages);
                                            $("#loadIndicator").dxLoadIndicator({
                                                visible: false
                                            });
                                        } else {

                                            //  LoadGrid(data.data);
                                            var api = "api/Staffs/DepartmentRoleData"
                                            $http.get(api).then(function (data) {
                                                $scope.DepartmentRole = data.data.Results.DepartmentData;
                                                console.log($scope.DepartmentRole);
                                                console.log($scope.Staff);
                                                $scope.LoadForm($scope.Staff);
                                            })
                                            //console.log($scope.DepartmentRole);
                                            //console.log(data.data.Results.StaffData);
                                            //$scope.LoadForm(data.data.Results.StaffData[0]);

                                        }

                                    });
                            }
                        }).appendTo(container);
                    }

                }, {
                    dataField: "StaffID",
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
                                var r = confirm("ต้องการลบรัสพนักงานนี้หรือไม่ !!!");
                                if (r === true) {
                                    $http.post("api/Staffs/DeleteStaff/" + options.key.StaffID).then(function successCallback(response) {

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
                                            $("#btnEditPER").hide();
                                            $("#btnAddPER").hide();
                                            $("#showAdd").hide();
                                            $("#btnAdd").show();
                                            $("#gridContainer").show();
                                            var api = "api/Staffs/StaffData"
                                            $http.get(api).then(function (data) {
                                                var Datasource = data.data.Results.StaffData;
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

        $scope.InsertPER = function () {

            if ($("#form-container").dxForm("instance").validate().isValid) {
                var obj = $("#form-container").dxForm("instance").option('formData');
                var Per = {
                    "StaffID": 0,
                    "StaffCode": obj.StaffCode,
                    "StaffFirstName": obj.StaffFirstName,
                    "StaffLastName": obj.StaffLastName,
                    "StaffPosition": obj.StaffPosition,
                    "StaffLogin": obj.StaffLogin,
                    "StaffPassword": obj.StaffPassword,
                    "StaffLevel": obj.StaffLevel,
                    "StaffDepartmentID": obj.StaffDepartmentID,
                };
                $.post("api/Staffs/SetStaffData?", Per

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
                            $("#btnEditPER").hide();
                            $("#btnAddPER").hide();
                            $("#showAdd").hide();
                            $("#btnAdd").show();
                            $("#gridContainer").show();
                            var api = "api/Staffs/StaffData"
                            $http.get(api).then(function (data) {
                                var Datasource = data.data.Results.StaffData;
                                $("#gridContainer").dxDataGrid("instance").option("dataSource", Datasource);
                                $("#gridContainer").dxDataGrid("instance").refresh();
                            });
                        }

                    });

            }

        }

        $scope.SubmitEditPER = function () {

            if ($("#form-container").dxForm("instance").validate().isValid) {
                var obj = $("#form-container").dxForm("instance").option('formData');
                var Per = {
                    "StaffID": obj.StaffID,
                    "StaffCode": obj.StaffCode,
                    "StaffFirstName": obj.StaffFirstName,
                    "StaffLastName": obj.StaffLastName,
                    "StaffPosition": obj.StaffPosition,
                    "StaffLogin": obj.StaffLogin,
                    "StaffPassword": obj.StaffPassword,
                    "StaffLevel": obj.StaffLevel,
                    "StaffDepartmentID": obj.StaffDepartmentID,
                };
                $.post("api/Staffs/SetStaffData?", Per

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
                            $("#btnEditPER").hide();
                            $("#btnAddPER").hide();
                            $("#showAdd").hide();
                            $("#btnAdd").show();
                            $("#gridContainer").show();
                            var api = "api/Staffs/StaffData"
                            $http.get(api).then(function (data) {
                                var Datasource = data.data.Results.StaffData;
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
            
            var api = "api/Staffs/DepartmentRoleData"
            $http.get(api).then(function (data) {
                console.log(data);
                $scope.DepartmentRole = data.data.Results.DepartmentData;

                $("#showAdd").show();
                $scope.LoadForm({});
                $("#btnAddPER").show();
                $("#btnEditPER").hide();
                $("#btnAdd").hide();
                $("#gridContainer").hide();
            })
            console.log($scope.DepartmentRole);


           
        }

    }
])