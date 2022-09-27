    var CartAbandonmentReminder = angular.module('CartAbandonmentReminder');
 
    CartAbandonmentReminder.service('cartAbandonmentReminderService', ['$http', function ($http, $localStorage) {
        return {
            search: function (criteria) {
                return $http.post('api/cart-abandonment/search', criteria);
            },
        }
    }]);
    