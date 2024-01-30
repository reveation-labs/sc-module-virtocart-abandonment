// Call this to register your module to main application
var moduleName = 'CartAbandonmentReminder';

if (AppDependencies !== undefined) {
    AppDependencies.push(moduleName);
}

angular.module(moduleName, [])
    .config(['$stateProvider',
        function ($stateProvider) {
            $stateProvider
                .state('workspace.CartAbandonmentReminderState', {
                    url: '/CartAbandonmentReminder',
                    templateUrl: '$(Platform)/Scripts/common/templates/home.tpl.html',
                    controller: [
                        'platformWebApp.bladeNavigationService',
                        function (bladeNavigationService) {
                            var newBlade = {
                                id: 'cartAbandonmentReminder',
                                controller: 'CartAbandonmentReminder.CartReminderController',
                                isExpandable: true,
                                template: 'Modules/$(Sharpcode.CartAbandonmentReminder)/Scripts/blades/cart-reminder.html',
                                isClosingDisabled: true,
                            };
                            bladeNavigationService.showBlade(newBlade);
                        }
                    ]
                });
        }
    ])
    .run(['platformWebApp.mainMenuService', '$state',
        function (mainMenuService, $state) {
            //Register module in main menu
            var menuItem = {
                path: 'browse/CartAbandonmentReminder',
                icon: 'fa fa-shopping-cart',
                title: 'Abandoned Carts',
                priority: 100,
                action: function () { $state.go('workspace.CartAbandonmentReminderState'); },
                permission: 'CartAbandonmentReminder:access',
            };
            mainMenuService.addMenuItem(menuItem);
        }
    ]);
