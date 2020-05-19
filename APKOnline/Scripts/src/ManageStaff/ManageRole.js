angular.module('ApkApp').controller('ManageRoleController', ['$scope', '$stateParams', '$http', '$rootScope', '$filter',
    function ($scope, $stateParams, $http, $rootScope, $filter) {
        $scope.data = {};
        $("#showAdd").hide();

        $scope.showColumnLines = true;
        $scope.showRowLines = true;
        $scope.showBorders = true;
        $scope.rowAlternationEnabled = true;

        var api = "api/Staffs/StaffRoleData"
        $http.get(api).then(function (data) {
            console.log(data);
            $scope.dataGridOptions = {
                dataSource: data.data.Results.PositionData,
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
                    dataField: "Positionid",
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
                    dataField: "PositionCode",
                    caption: "ตำแหน่ง",

                }, {
                    dataField: "PositionName",
                    caption: "ชื่อตำแหน่ง",

                }, {
                    dataField: "PositionLevel",
                    caption: "ระดับ",
                    editorOptions: {
                        disabled: true
                    },

                }, {
                    dataField: "PositionLimit",
                    caption: "ยอดอนุมัติ",
                    dataType: "number",
                    format: "currency",
                    alignment: "right",
                    editorOptions: {
                        disabled: true
                    },
                    //caption: "กำหนดสิทธิ์การใช้งาน",
                    //cellTemplate: function (container, item) {
                    //    //    console.log(item)
                    //    var data = item.data,
                    //        markup = "<a >กำหนดสิทธิ์การใช้งาน</a>";
                    //    container.append(markup);
                    //},
                    //alignment: "center",
                //}, {
                //    caption: "กำหนดตำแหน่ง",
                //    cellTemplate: function (container, item) {
                //        //    console.log(item)
                //        var data = item.data,
                //            markup = "<a >กำหนดตำแหน่ง</a>";
                //        container.append(markup);
                //    },
                //    alignment: "center",

                }, {
                    dataField: "Positionid",
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
                                $("#btnEditPOS").show();
                                $("#gridContainer").hide();

                                var api = "api/Staffs/StaffRoleDataByID?POSid=" + options.key.Positionid;

                                $http.get(api)
                                    .then(function (data) {

                                        if (data.data.StatusCode > 1) {
                                            DevExpress.ui.notify(data.data.Messages);
                                            $("#loadIndicator").dxLoadIndicator({
                                                visible: false
                                            });
                                        } else {

                                            //  LoadGrid(data.data);
                                            console.log(data.data.Results.PositionData);
                                            $scope.LoadForm(data.data.Results.PositionData[0]);
                                        }

                                    });
                            }
                        }).appendTo(container);
                    }

                }
                ],

            };
        })

        $scope.LoadForm = function (data) {
            $("#form-container").dxForm({
                colCount: 1,
                width: 500,
                formData: data,
                showColonAfterLabel: true,
                showValidationSummary: true,
                items: [{
                    dataField: "PositionCode",
                    label: {
                        text: "ตำแหน่ง",
                    },
                    editorOptions: {
                        disabled: false,
                        attr: { 'style': "text-transform: uppercase" },
                        Maxleght: 15,
                    },
                    validationRules: [{
                        type: "required",
                        message: "โปรดระบุ ตำแหน่ง"
                    }]
                },
                {
                    dataField: "PositionName",
                    label: {
                        text: "ชื่อตำแหน่ง"
                    },
                    editorOptions: {
                        disabled: false
                    }
                },
                {
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
                },


                ]
            });

        }

        $scope.Refresh = function () {
            $("#btnEditPOS").hide();
            $("#showAdd").hide();
            $("#gridContainer").show();

            $scope.LoadForm({});
        }

        $scope.SubmitEditPosition = function () {

            if ($("#form-container").dxForm("instance").validate().isValid) {
                var obj = $("#form-container").dxForm("instance").option('formData');
                var Pos = {
                    "Positionid": obj.Positionid,
                    "PositionCode": obj.PositionCode,
                    "PositionName": obj.PositionName,
                    "PositionLimit": obj.PositionLimit,
                };
                $.post("api/Staffs/SetPositionRoleData?", Pos

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
                            $("#btnEditPOS").hide();
                            $("#showAdd").hide();
                            $("#gridContainer").show();
                            var api = "api/Staffs/StaffRoleData"
                            $http.get(api).then(function (data) {
                                var Datasource = data.data.Results.PositionData;
                                $("#gridContainer").dxDataGrid("instance").option("dataSource", Datasource);
                                $("#gridContainer").dxDataGrid("instance").refresh();
                            });
                        }

                    });
            }


        }

    }
])