using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using ReveationLabs.CartAbandonmentReminder.Core.Services;
using VirtoCommerce.CartModule.Core.Model;
using VirtoCommerce.NotificationsModule.Core.Extensions;
using VirtoCommerce.NotificationsModule.Core.Model;
using VirtoCommerce.NotificationsModule.Core.Services;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.GenericCrud;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.StoreModule.Core.Model;

namespace ReveationLabs.CartAbandonmentReminder.Data.Notifications
{
    public class SendCartReminderEmailNotification : ISendCartReminderEmailNotification
    {
        private readonly INotificationSearchService _notificationSearchService;
        private readonly INotificationSender _notificationSender;
        private readonly ICrudService<Store> _crudService;
        private readonly ISettingsManager _settingsManager;
        private readonly UserManager<ApplicationUser> _userManager;
        public SendCartReminderEmailNotification(
            INotificationSender notificationSender,
            ICrudService<Store> crudService,
            ISettingsManager settingsManager,
            UserManager<ApplicationUser> userManager,
            INotificationSearchService notificationSearchService)
        {
            _notificationSender = notificationSender;
            _crudService = crudService;
            _settingsManager = settingsManager;
            _notificationSearchService = notificationSearchService;
            _userManager = userManager;
        }

        public async Task TryToSendCartReminderAsync(List<ShoppingCart> shoppingCarts,Store store,bool isAnonymousUserAllowed,bool isLoginUserAllowed)
        {
            foreach (var shoppingCart in shoppingCarts)
            {
                var notifications = new List<EmailNotification>();
                var notification = await _notificationSearchService.GetNotificationAsync<CartReminderEmailNotification>(new TenantIdentity(shoppingCart.StoreId, nameof(Store)));
                if (notification != null)
                {
                    if (shoppingCart.Shipments.Count > 0 && shoppingCart.Shipments.First().DeliveryAddress is not null && isAnonymousUserAllowed)
                    {
                        var anonymousUserEmail = shoppingCart.Shipments.First().DeliveryAddress.Email;
                        notification.LanguageCode = shoppingCart.LanguageCode;
                        notification.ShoppingCart = shoppingCart;
                        notifications.Add(notification);
                        notification.SetFromToMembers(store.Email,anonymousUserEmail);
                        await _notificationSender.SendNotificationAsync(notification);
                    }

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
