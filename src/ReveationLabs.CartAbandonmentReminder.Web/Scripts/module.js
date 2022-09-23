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
                                id: 'blade1',
                                controller: 'CartAbandonmentReminder.helloWorldController',
                                template: 'Modules/$(ReveationLabs.CartAbandonmentReminder)/Scripts/blades/hello-world.html',
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
                icon: 'fa fa-cube',
                title: 'CartAbandonmentReminder',
                priority: 100,
                action: function () { $state.go('workspace.CartAbandonmentReminderState'); },
                permission: 'CartAbandonmentReminder:access',
            };
            mainMenuService.addMenuItem(menuItem);
        }
    ]);