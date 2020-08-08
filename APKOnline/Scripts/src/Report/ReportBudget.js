angular.module('ApkApp').controller('ReportBudgetController', ['$scope', '$rootScope', '$stateParams', '$http', '$filter', 'LoginService',
    function ($scope, $rootScope, $stateParams, $http, $filter, LoginService) {
        $scope.MONTHDATE = new Date();
        $scope.format = "dd/MM/yyyy";
        $scope.altInputFormats = ['d!/M!/yyyy'];
        $('#myModal').modal('show');

        $scope.dateOptionsMonth = {
            formatYear: 'yyyy',
            startingDay: 0,
            minMode: 'month',
            showWeeks: false
        };

        $scope.popupMonth = {
            opened: false
        };

        $scope.openMonth = function () {
            $scope.popupMonth.opened = true;
        };
        
        $scope.OpenSearch = function () {
            $('#myModal').modal('show');
        };

        let getDaysInMonth = function (month, year) {
            return new Date(year, month, 0).getDate();
        };

        $scope.ListReportBudget = function () {
            $scope.STARTDATE = new Date($scope.MONTHDATE.getFullYear(), $scope.MONTHDATE.getMonth(), 1);
            $scope.ENDDATE = new Date($scope.MONTHDATE.getFullYear(), $scope.MONTHDATE.getMonth(), getDaysInMonth(($scope.MONTHDATE.getMonth() + 1), $scope.MONTHDATE.getFullYear()));
            $scope.MONTHS = String($scope.MONTHDATE.getMonth() + 1);

            //$http.get("api/Report/ListReportBudget?STARTDATE=" + moment($scope.STARTDATE).format("YYYY-MM-DD") + "&ENDDATE=" + moment($scope.ENDDATE).format("YYYY-MM-DD") + "&MONTHS=" + $scope.MONTHS + "&StaffCode=" + $scope.StaffCode + "&DEPcode=" + $scope.DEPcode).then(function (data) {
            //    $scope.ListReportBudgets = data.data.Results.ReportBudget;
            //    $('#myModal').modal('hide');
            //    console.log($scope.ListReportBudgets);
            //});

            $http.get("api/Report/ListReportBudget?Year=" + $scope.MONTHDATE.getFullYear() + "&month=" + $scope.MONTHDATE.getMonth() + "&StaffCode=" + localStorage.getItem('StaffID') + "&DEPcode=" + localStorage.getItem('StaffDepartmentID')).then(function (data) {
                $scope.ListReportBudgets = data.data.Results.ReportBudget;
                $('#myModal').modal('hide');
                console.log($scope.ListReportBudgets);
            });
        };

        $scope.ChangeMonthDate = function (Date) {
            $scope.MONTHDATE = Date;
            $scope.ListReportBudget();
        };
        
        $scope.ExportExcel = function () {
            let wb = XLSX.utils.table_to_book(document.getElementById('sjs-table'));
            XLSX.writeFile(wb, "ขายตามบิลขาย_" + moment($scope.STARTDATE).format("YYYYMMDD") + ".xlsx");
        };

        LoginService.GlobalLogin();
    }
]);