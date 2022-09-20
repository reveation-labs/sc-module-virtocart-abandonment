angular.module('CartAbandonmentReminder')
    .controller('CartAbandonmentReminder.helloWorldController', ['$scope', 'CartAbandonmentReminder.webApi', function ($scope, api) {
        var blade = $scope.blade;
        blade.title = 'CartAbandonmentReminder';

        blade.refresh = function () {
            api.get(function (data) {
                blade.title = 'CartAbandonmentReminder.blades.hello-world.title';
                blade.data = data.result;
                blade.isLoading = false;
            });
        };

        blade.refresh();
    }]);
