angular.module('ApkApp').config(function ($stateProvider, $urlRouterProvider, $ocLazyLoadProvider, $qProvider, $locationProvider) {

    $urlRouterProvider.otherwise("/");
    $qProvider.errorOnUnhandledRejections(false);
    $locationProvider.hashPrefix('');

    $stateProvider
        //General Section
        .state('/', {
            name: "Dashboard",
            url: "/",
            templateUrl: '\Home/Dashboard',
            controller: 'DashboardController'
        })
        .state('/ManageStaff/PermissionStaff', {
            url: "/ManageStaff/PermissionStaff",
            templateUrl: '\ManageStaff/PermissionStaff',
            controller: 'PermissionStaffController',
            resolve: {
                lazyLoad: ['$ocLazyLoad', function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            name: 'AceApp',
                            files: [
                                'Scripts/src/ManageStaff/PermissionStaff.js'
                            ]
                        }])
                }]
            }
        })
        .state('/PurchaseRequest/ListPurchaseRequest', {
            url: "/PurchaseRequest/ListPurchaseRequest",
            templateUrl: '\PurchaseRequest/ListPurchaseRequest',
            controller: 'ListPurchaseRequestController',
            resolve: {
                lazyLoad: ['$ocLazyLoad', function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            name: 'AceApp',
                            files: [
                                'Scripts/src/PurchaseRequest/ListPurchaseRequest.js'
                            ]
                        }])
                }]
            }
        })
        .state('/PurchaseRequest/ListPRApprove', {
            url: "/PurchaseRequest/ListPRApprove",
            templateUrl: '\PurchaseRequest/ListApprovePurchaseRequest',
            controller: 'ListPRApproveController',
            resolve: {
                lazyLoad: ['$ocLazyLoad', function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            name: 'AceApp',
                            files: [
                                'Scripts/src/PurchaseRequest/ListPRforApprove.js'
                            ]
                        }])
                }]
            }
        })

        .state('/PurchaseRequest/CreatePurchaseRequest', {
            url: "/PurchaseRequest/CreatePurchaseRequest",
            templateUrl: '\PurchaseRequest/ApprovePurchaseRequest',
            controller: 'ApprovePurchaseRequestController',
            resolve: {
                lazyLoad: ['$ocLazyLoad', function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            name: 'AceApp',
                            files: [
                                'Scripts/src/PurchaseRequest/ApprovePurchaseRequest.js'
                            ]
                        }])
                }]
            }
        })
        .state('/PurchaseRequest/ViewPurchaseRequest/:id', {
            url: "/PurchaseRequest/ViewPurchaseRequest/:id",
            templateUrl: '\PurchaseRequest/ViewPurchaseRequest',
            controller: 'ViewPurchaseRequestController',
            resolve: {
                lazyLoad: ['$ocLazyLoad', function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            name: 'AceApp',
                            files: [
                                'Scripts/src/PurchaseRequest/ViewPurchaseRequest.js'
                            ]
                        }])
                }]
            }
        })
        .state('/PurchaseRequest/EditPurchaseRequest/:id', {
            url: "/PurchaseRequest/EditPurchaseRequest/:id",
            templateUrl: '\PurchaseRequest/EditPurchaseRequest',
            controller: 'EditPurchaseRequestController',
            resolve: {
                lazyLoad: ['$ocLazyLoad', function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            name: 'AceApp',
                            files: [
                                'Scripts/src/PurchaseRequest/EditPurchaseRequest.js'
                            ]
                        }])
                }]
            }
        })
        .state('/PurchaseRequest/ApprovePR/:id', {
            url: "/PurchaseRequest/ApprovePR/:id",
            templateUrl: '\PurchaseRequest/ApprovePR',
            controller: 'ApprovePRController',
            resolve: {
                lazyLoad: ['$ocLazyLoad', function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            name: 'AceApp',
                            files: [
                                'Scripts/src/PurchaseRequest/ApprovePR.js'
                            ]
                        }])
                }]
            }
        })
        .state('/PurchaseOrder/ApprovePO/:id', {
            url: "/PurchaseOrder/ApprovePO/:id",
            templateUrl: '\PurchaseOrder/ApprovePO',
            controller: 'ApprovePOController',
            resolve: {
                lazyLoad: ['$ocLazyLoad', function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            name: 'AceApp',
                            files: [
                                'Scripts/src/PurchaseOrder/ApprovePO.js'
                            ]
                        }])
                }]
            }
        })
        .state('/PurchaseOrder/ListPurchaseOrder', {
            url: "/PurchaseOrder/ListPurchaseOrder",
            templateUrl: '\PurchaseOrder/ListPurchaseOrder',
            controller: 'PurchaseOrderController',
            resolve: {
                lazyLoad: ['$ocLazyLoad', function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            name: 'AceApp',
                            files: [
                                'Scripts/src/PurchaseOrder/PurchaseOrder.js'
                            ]
                        }])
                }]
            }
        })
        .state('/PurchaseOrder/ListPOApprove', {
            url: "/PurchaseOrder/ListPOApprove",
            templateUrl: '\PurchaseOrder/ListPurchaseOrderApprove',
            controller: 'PurchaseOrderApproveController',
            resolve: {
                lazyLoad: ['$ocLazyLoad', function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            name: 'AceApp',
                            files: [
                                'Scripts/src/PurchaseOrder/PurchaseOrderApprove.js'
                            ]
                        }])
                }]
            }
        })
        .state('/PurchaseRequest/ViewPurchaseOrder/:id', {
            url: "/PurchaseRequest/ViewPurchaseOrder/:id",
            templateUrl: '\PurchaseOrder/ViewPurchaseOrder',
            controller: 'ViewPurchaseOrderController',
            resolve: {
                lazyLoad: ['$ocLazyLoad', function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            name: 'AceApp',
                            files: [
                                'Scripts/src/PurchaseOrder/ViewPurchaseOrder.js'
                            ]
                        }])
                }]
            }
        })
        .state('/ReportBudget', {
            url: "/ReportBudget",
            templateUrl: '\Report/ReportBudget',
            controller: 'ReportBudgetController',
            resolve: {
                lazyLoad: ['$ocLazyLoad', function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            name: 'AceApp',
                            files: [
                                'Scripts/src/Report/ReportBudget.js'
                            ]
                        }])
                }]
            }
        })
        .state('/ManageStaff/ManageRole', {
            url: "/ManageStaff/ManageRole",
            templateUrl: '\ManageStaff/ManageRole',
            controller: 'ManageRoleController',
            resolve: {
                lazyLoad: ['$ocLazyLoad', function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            name: 'AceApp',
                            files: [
                                'Scripts/src/ManageStaff/ManageRole.js'
                            ]
                        }])
                }]
            }
        })
        .state('/ManageStaff/ManageStaff', {
            url: "/ManageStaff/ManageStaff",
            templateUrl: '\ManageStaff/ManageStaff',
            controller: 'ManageStaffController',
            resolve: {
                lazyLoad: ['$ocLazyLoad', function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            name: 'AceApp',
                            files: [
                                'Scripts/src/ManageStaff/ManageStaff.js'
                            ]
                        }])
                }]
            }
        })
        .state('/ManageStaff/PermissionRole', {
            url: "/ManageStaff/PermissionRole",
            templateUrl: '\ManageStaff/PermissionRole',
            controller: 'PermissionRoleController',
            resolve: {
                lazyLoad: ['$ocLazyLoad', function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            name: 'AceApp',
                            files: [
                                'Scripts/src/ManageStaff/PermissionRole.js'
                            ]
                        }])
                }]
            }
        })
        .state('/PurchaseRequest/ListPROverBudgetApprove', {
            url: "/PurchaseRequest/ListPROverBudgetApprove",
            templateUrl: '\PurchaseRequest/ListOverBGPurchaseRequest',
            controller: 'ListPROverforApproveController',
            resolve: {
                lazyLoad: ['$ocLazyLoad', function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            name: 'AceApp',
                            files: [
                                'Scripts/src/PurchaseRequest/ListPROverforApprove.js'
                            ]
                        }])
                }]
            }
        })
        .state('/PurchaseRequest/ListPreviewPurchaseRequest', {
            url: "/PurchaseRequest/ListPreviewPurchaseRequest",
            templateUrl: '\PurchaseRequest/ListPreviewPurchaseRequest',
            controller: 'ListPreviewController',
            resolve: {
                lazyLoad: ['$ocLazyLoad', function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            name: 'AceApp',
                            files: [
                                'Scripts/src/PurchaseRequest/ListPreview.js'
                            ]
                        }])
                }]
            }
        })
        .state('/PurchaseRequest/ApprovePROverBudget/:id', {
            url: "/PurchaseRequest/ApprovePROverBudget/:id",
            templateUrl: '\PurchaseRequest/ApprovePROver',
            controller: 'ApprovePROverController',
            resolve: {
                lazyLoad: ['$ocLazyLoad', function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            name: 'AceApp',
                            files: [
                                'Scripts/src/PurchaseRequest/ApprovePROverBudget.js'
                            ]
                        }])
                }]
            }
        })
        .state('/PurchaseRequest/PreviewEdit/:id', {
            url: "/PurchaseRequest/PreviewEdit/:id",
            templateUrl: '\PurchaseOrder/EditPreviewPO',
            controller: 'EditPreviewPOController',
            resolve: {
                lazyLoad: ['$ocLazyLoad', function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            name: 'AceApp',
                            files: [
                                'Scripts/src/PurchaseOrder/EditPreviewPO.js'
                            ]
                        }])
                }]
            }
        })
        .state('/PurchaseRequest/ViewPO/:id', {
            url: "/PurchaseRequest/ViewPO/:id",
            templateUrl: '\PurchaseOrder/ViewPO',
            controller: 'ViewPOController',
            resolve: {
                lazyLoad: ['$ocLazyLoad', function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            name: 'AceApp',
                            files: [
                                'Scripts/src/PurchaseOrder/ViewPO.js'
                            ]
                        }])
                }]
            }
        })
        .state('/ManageStaff/ManageBudgetDep', {
            url: "/ManageStaff/ManageBudgetDep",
            templateUrl: '\ManageStaff/ManageBudgetDep',
            controller: 'ManageBudgetDepController',
            resolve: {
                lazyLoad: ['$ocLazyLoad', function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            name: 'AceApp',
                            files: [
                                'Scripts/src/ManageStaff/ManageBudgetDep.js'
                            ]
                        }])
                }]
            }
        })
});