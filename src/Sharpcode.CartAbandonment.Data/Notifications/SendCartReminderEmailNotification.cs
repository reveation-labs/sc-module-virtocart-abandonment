using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Sharpcode.CartAbandonment.Core;
using Sharpcode.CartAbandonment.Core.Services;
using VirtoCommerce.CartModule.Core.Model;
using VirtoCommerce.NotificationsModule.Core.Extensions;
using VirtoCommerce.NotificationsModule.Core.Model;
using VirtoCommerce.NotificationsModule.Core.Services;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.StoreModule.Core.Model;

namespace Sharpcode.CartAbandonment.Data.Notifications
{
    public class SendCartReminderEmailNotification : ISendCartReminderEmailNotification
    {
        private readonly INotificationSearchService _notificationSearchService;
        private readonly INotificationSender _notificationSender;        
        private readonly ISettingsManager _settingsManager;        
        private readonly UserManager<ApplicationUser> _userManager;
        public SendCartReminderEmailNotification(
            INotificationSender notificationSender,            
            ISettingsManager settingsManager,
            UserManager<ApplicationUser> userManager,
            INotificationSearchService notificationSearchService)
        {
            _notificationSender = notificationSender;            
            _settingsManager = settingsManager;
            _notificationSearchService = notificationSearchService;
            _userManager = userManager;            
        }
        // Method to trigger email notification for all shopping carts
        public async Task TryToSendCartReminderAsync(List<ShoppingCart> shoppingCarts,Store store)
        {
            var isAnonymousUserAllowed = await _settingsManager.GetValueAsync<bool>(ModuleConstants.Settings.CartAbandonmentStoreSettings.RemindUserAnonymous);
            var isLoginUserAllowed = await _settingsManager.GetValueAsync<bool>(ModuleConstants.Settings.CartAbandonmentStoreSettings.RemindUserLogin);
            foreach (var shoppingCart in shoppingCarts)
            {
                var notifications = new List<EmailNotification>();
                var notification = await _notificationSearchService.GetNotificationAsync<CartAbandonmentEmailNotification>(new TenantIdentity(shoppingCart.StoreId, nameof(Store)));
                if (notification != null)
                {
                    //if user is anonymous and we have email id available then it will use this to trigegr email
                    if (shoppingCart.Shipments.Count > 0 && shoppingCart.Shipments.First().DeliveryAddress is not null && shoppingCart.Shipments.First().DeliveryAddress.Email is not null && isAnonymousUserAllowed)
                    {
                        var anonymousUserEmail = shoppingCart.Shipments.First().DeliveryAddress.Email;
                        notification.LanguageCode = shoppingCart.LanguageCode;
                        notification.ShoppingCart = shoppingCart;
                        notifications.Add(notification);
                        notification.SetFromToMembers(store.Email,anonymousUserEmail);
                        await _notificationSender.SendNotificationAsync(notification);
                    }

                    // if login user have created cart then we will trigger this email
                    if (!shoppingCart.IsAnonymous && isLoginUserAllowed)
                    {
                        var userEmail = (await _userManager.FindByIdAsync(shoppingCart.CustomerId)).Email;
                        notification.LanguageCode = shoppingCart.LanguageCode;
                        notification.ShoppingCart = shoppingCart;
                        notifications.Add(notification);
                        notification.SetFromToMembers(store.Email, userEmail);
                        await _notificationSender.SendNotificationAsync(notification);
                    }
                }
            }
        }
    }
}
