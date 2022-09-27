angular.module('CartAbandonmentReminder')
    .controller('CartAbandonmentReminder.CartReminderController', ['$scope','platformWebApp.bladeNavigationService', 'cartAbandonmentReminderService', 
    'platformWebApp.bladeUtils', 'platformWebApp.dialogService', 'platformWebApp.authService', 'uiGridConstants', 'platformWebApp.uiGridHelper', 'platformWebApp.ui-grid.extension', 'virtoCommerce.orderModule.knownOperations', '$translate', 
    function ($scope,bladeNavigationService, cartAbandonmentReminderService,bladeUtils, dialogService, authService, uiGridConstants, uiGridHelper, gridOptionExtension, knownOperations, $translate) {
        var blade = $scope.blade;
        blade.carts = null;
        $scope.carts = null;
        blade.title = 'CartAbandonmentReminder';
        var showAllCarts = true;
        var blade = $scope.blade;
        var bladeNavigationService = bladeUtils.bladeNavigationService;
        $scope.uiGridConstants = uiGridConstants;
        $scope.pageSettings = {};
        $scope.pageSettings.currentPage = 1;
        $scope.pageSettings.itemsPerPageCount = 20;
        $scope.pageSettings.totalItems = 0;

        blade.refresh = function () {
            if (angular.isFunction(blade.refreshCallback)) {
                blade.isLoading = true;
    
                var result = blade.refreshCallback(blade);
    
                if (angular.isDefined(result.$promise)) {
                    result.$promise.then(function (data) {
                        blade.isLoading = false;
    
                        $scope.pageSettings.totalItems = data.totalCount;
                        $scope.objects = data.results;
                    });
                }
            }
            else if (blade.preloadedOrders) {
                $scope.pageSettings.totalItems = blade.preloadedOrders.length;
                $scope.objects = blade.preloadedOrders;
    
                blade.isLoading = false;
            } else {
                blade.isLoading = true;
                var criteria = {
                    skip: ($scope.pageSettings.currentPage - 1) * $scope.pageSettings.itemsPerPageCount,
                    take: $scope.pageSettings.itemsPerPageCount
                };
    
                if (blade.searchCriteria) {
                    angular.extend(criteria, blade.searchCriteria);
                }
                cartAbandonmentReminderService.search(criteria).then(function (response) {
                    blade.isLoading = false;
                    $scope.carts = response.data.results;
                    $scope.pageSettings.totalItems = response.data.totalCount;
                    $scope.objects = response.data.results;
                });
            }

        };
        // $scope.blade.toolbarCommands = [
        //     {
        //         name: "Show All Cart",
        //         icon: 'switch',
        //         class: 'switch',
        //         type: 'checkbox',
        //         executeMethod: function () {
        //             if(showAllCarts){
        //                 showAllCarts = false
        //             }else{
        //                 showAllCarts = true
        //             }
        //             blade.refresh();
        //         },
        //         canExecuteMethod: function () {
        //             return true;
        //         },
        //     }
        // ];

        $scope.openCartBlade = function (cart) {
            var newBlade = {
                id: 'cartAbandonmentReminder',
                title: 'Cart Details',
                currentEntities: cart,
                controller: 'CartAbandonmentReminde.LineItemsController',
                template: 'Modules/$(ReveationLabs.CartAbandonmentReminder)/Scripts/blades/cartLineItems.tpl.html'
            };
            bladeNavigationService.showBlade(newBlade, $scope.blade);
        };

        blade.refresh();
    }]);
