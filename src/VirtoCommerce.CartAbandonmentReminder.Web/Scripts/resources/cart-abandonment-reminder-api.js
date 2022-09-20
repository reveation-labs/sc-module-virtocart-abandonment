angular.module('CartAbandonmentReminder')
    .factory('CartAbandonmentReminder.webApi', ['$resource', function ($resource) {
        return $resource('api/cart-abandonment-reminder');
    }]);
