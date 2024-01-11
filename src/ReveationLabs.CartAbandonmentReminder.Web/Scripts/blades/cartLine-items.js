angular.module('CartAbandonmentReminder')
    .controller('CartAbandonmentReminde.LineItemsController', ['$scope', 'platformWebApp.bladeNavigationService', 'virtoCommerce.orderModule.catalogItems', 'virtoCommerce.orderModule.prices', '$translate', 'platformWebApp.authService', function ($scope, bladeNavigationService, items, prices, $translate, authService) {
    var blade = $scope.blade;
    blade.isVisiblePrices = authService.checkPermission('order:read_prices');

    var selectedProducts = [];
    $scope.cart = $scope.blade.currentEntities;

    blade.refresh = function () {
        blade.isLoading = false;
        blade.selectedAll = false;
    };


    $scope.openItemDynamicProperties = function (item) {
        var blade = {
            id: "dynamicPropertiesList",
            currentEntity: item,
            controller: 'platformWebApp.propertyValueListController',
            template: '$(Platform)/Scripts/app/dynamicProperties/blades/propertyValue-list.tpl.html'
        };
        bladeNavigationService.showBlade(blade, $scope.blade);
    };

    $scope.openItemDetail = function (item) {
        var newBlade = {
            id: "listItemDetail",
            itemId: item.productId,
            productType: item.productType,
            title: item.name,
            controller: 'virtoCommerce.catalogModule.itemDetailController',
            template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/item-detail.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, $scope.blade);
    };

    function openAddEntityWizard() {
        var options = {
            checkItemFn: function (listItem, isSelected) {
                if (isSelected) {
                    if (_.all(selectedProducts, function (x) { return x.id !== listItem.id; })) {
                        selectedProducts.push(listItem);
                    }
                }
                else {
                    selectedProducts = _.reject(selectedProducts, function (x) { return x.id === listItem.id; });
                }
            }
        };
        var newBlade = {
            id: "CatalogItemsSelect",
            currentEntities: blade.currentEntity,
            title: "orders.blades.catalog-items-select.title",
            controller: 'virtoCommerce.catalogModule.catalogItemSelectController',
            template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/common/catalog-items-select.tpl.html',
            options: options,
            breadcrumbs: [],
            toolbarCommands: [
              {
                  name: "orders.commands.add-selected", icon: 'fas fa-plus',
                  executeMethod: function (blade) {
                      addProductsToOrder(selectedProducts);
                      selectedProducts.length = 0;
                      bladeNavigationService.closeBlade(blade);
                  },
                  canExecuteMethod: function () {
                      return selectedProducts.length > 0;
                  }
              }]
        };
        bladeNavigationService.showBlade(newBlade, $scope.blade);
    }


    $scope.checkAll = function (selected) {
        angular.forEach(blade.currentEntity.items, function (item) {
            item.selected = selected;
        });
    };


    blade.refresh();
}]);
