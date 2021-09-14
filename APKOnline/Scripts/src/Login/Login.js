angular.module('ApkApp').controller('LoginController', ['$scope', '$location', '$stateParams', '$http', '$rootScope',
    function ($scope, $location, $stateParams, $http, $rootScope) {
    $rootScope.CheckUser = 0;

    $scope.submitLogin = function () {
        $http.post("../api/Staffs/login", { StaffLogin: $scope.username, StaffPassword: $scope.password })
            .then(function (data) {
                
                if (data.data.StatusCode === 2) {
                    $scope.username = "";
                    $scope.password = "";
                    console.log(data.data.Messages);
                    var Msg = data.data.Messages;
                    swal({
                        title: 'Information',
                        text: data.data.Messages,
                        type: "info",
                        showCancelButton: false,
                        confirmButtonColor: "#6EAA6F",
                        confirmButtonText: 'OK'
                    })
                    //swal({
                    //    title: 'Information',
                    //    text: Msg,
                        
                    //});
                    
                }
                else {
                    if (data.data.Results.StaffLogin.length > 0) {
                        $scope.login = data.data.Results.StaffLogin[0];
                        localStorage.setItem('StaffID', $scope.login.StaffID);
                        localStorage.setItem('StaffCode', $scope.login.StaffCode);
                        localStorage.setItem('StaffImage', $scope.login.StaffImage);
                        localStorage.setItem('StaffFirstName', $scope.login.StaffFirstName);
                        localStorage.setItem('StaffLastName', $scope.login.StaffLastName);
                        localStorage.setItem('StaffDepartmentID', $scope.login.StaffDepartmentID);
                        localStorage.setItem('StaffDepartmentName', $scope.login.DepartmentName);
                        localStorage.setItem('StaffLevelID', $scope.login.StaffLevelID);
                        localStorage.setItem('StaffLevel', $scope.login.StaffLevel);
                        localStorage.setItem('isPreview', $scope.login.isPreview);
                        window.location = '../../#/';
                    }
                    else {
                        $rootScope.CheckUser = 1;
                    }
                }
            });
    };

    console.clear();
}])

