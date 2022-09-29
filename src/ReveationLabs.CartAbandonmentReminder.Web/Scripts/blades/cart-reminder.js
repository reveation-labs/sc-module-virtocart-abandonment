angular.module('CartAbandonmentReminder')
    .controller('CartAbandonmentReminder.CartReminderController', ['$scope','platformWebApp.bladeNavigationService', 'cartAbandonmentReminderService', 
    'platformWebApp.bladeUtils', 'platformWebApp.dialogService', 'platformWebApp.authService', 'uiGridConstants', 'platformWebApp.uiGridHelper', 'platformWebApp.ui-grid.extension', 'virtoCommerce.orderModule.knownOperations', '$translate', 
    function ($scope,bladeNavigationService, cartAbandonmentReminderService,bladeUtils, dialogService, authService, uiGridConstants, uiGridHelper, gridOptionExtension, knownOperations, $translate) {
        var blade = $scope.blade;
        blade.carts = null;
        $scope.carts = null;
        blade.title = 'Cart Abandonment Reminder';
        var bladeNavigationService = bladeUtils.bladeNavigationService;
        $scope.uiGridConstants = uiGridConstants;
        $scope.pageSettings = {};
        $scope.pageSettings.currentPage = 1;
        $scope.pageSettings.itemsPerPageCount = 20;
        $scope.pageSettings.totalItems = 0;
        
    $scope.getGridOptions = () => {
        return {
        useExternalSorting: true,
        data: 'objects',
        rowTemplate: 'order-list.row.html',
        columnDefs: [
                   { name: 'customerName', displayName: 'orders.blades.customerOrder-list.labels.customer', width: '***' },
                   { name: 'storeId', displayName: 'orders.blades.customerOrder-list.labels.store', width: '**' },
                   { name: 'currency', displayName: 'orders.blades.customerOrder-list.labels.currency', width: '*' },
                   { name: 'total', displayName: 'orders.blades.customerOrder-list.labels.total', width: '*' },
                   { name: 'createdDate', displayName: 'orders.blades.customerOrder-list.labels.created', width: '**', sort: { direction: uiGridConstants.DESC } }
       ]}
    }

        blade.refresh = function () {
            if (angular.isFunction(blade.refreshCallback)) {
                blade.isLoading = true;
    
                var result = blade.refreshCallback(blade);
    
                if (angular.isDefined(result.$promise)) {
                    result.$promise.then(function (response) {
                        blade.isLoading = false;
    
                        $scope.pageSettings.totalItems = response.data.totalCount;
                        $scope.objects = response.data.results;
                    });
                }
            }else {
                blade.isLoading = true;
                var criteria = {
                    sort: uiGridHelper.getSortExpression($scope),
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
        $scope.blade.toolbarCommands = [
            {
                name: "platform.commands.refresh", icon: 'fa fa-refresh',
                executeMethod: blade.refresh,
                canExecuteMethod: function () {
                    return true;
                }
            }
        ];

        $scope.setGridOptions = function (gridId, gridOptions) {
            // add currency filter for properties that need it

            $scope.gridOptions = gridOptions;
            gridOptionExtension.tryExtendGridOptions(gridId, gridOptions);
    
            uiGridHelper.initialize($scope, gridOptions, function (gridApi) {

                    uiGridHelper.bindRefreshOnSortChanged($scope);
            });
    
            bladeUtils.initializePagination($scope);
    
            return gridOptions;
        };
        $scope.openCartBlade = function (cart) {
            if(!cart.isAnonymous){
                cartAbandonmentReminderService.getUserById(cart.customerId).then(function(response){
                    cart.email = response.data.email;
                });
            }
            var newBlade = {
                id: 'cart-details',
                title: 'Cart Details',
                currentEntities: cart,
                controller: 'CartAbandonmentReminde.LineItemsController',
                template: 'Modules/$(ReveationLabs.CartAbandonmentReminder)/Scripts/blades/cartLineItems.tpl.html'
            };
            bladeNavigationService.showBlade(newBlade, $scope.blade);
        };

        // blade.refresh();
    }]);
