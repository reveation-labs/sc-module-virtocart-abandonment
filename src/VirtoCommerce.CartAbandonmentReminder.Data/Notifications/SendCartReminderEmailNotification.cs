using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityFrameworkCore.Triggers;
using Microsoft.AspNetCore.Identity;
using VirtoCommerce.CartAbandonmentReminder.Core.Services;
using VirtoCommerce.CartModule.Core.Model;
using VirtoCommerce.CartModule.Core.Model.Search;
using VirtoCommerce.NotificationsModule.Core.Extensions;
using VirtoCommerce.NotificationsModule.Core.Model;
using VirtoCommerce.NotificationsModule.Core.Services;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Core.GenericCrud;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.StoreModule.Core.Model;

namespace VirtoCommerce.CartAbandonmentReminder.Data.Notifications
{
    public class SendCartReminderEmailNotification : ISendCartReminderEmailNotification
    {
        private readonly INotificationSearchService _notificationSearchService;
        private readonly INotificationSender _notificationSender;
        private readonly ICrudService<Store> _crudService;
        private readonly ISettingsManager _settingsManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ISettingsSearchService _settingsSearchService;
        public SendCartReminderEmailNotification(INotificationSender notificationSender, ICrudService<Store> crudService,
                                                       ISettingsManager settingsManager, UserManager<ApplicationUser> userManager, INotificationSearchService notificationSearchService, ISettingsSearchService settingsSearchService)
        {
            _notificationSender = notificationSender;
            _crudService = crudService;
            _settingsManager = settingsManager;
            _notificationSearchService = notificationSearchService;
            _userManager = userManager;
            _settingsSearchService = settingsSearchService;
        }

        public async Task TryToSendCartReminderAsync(List<ShoppingCart> shoppingCarts)
        {
            foreach(var shoppingCart in shoppingCarts)
            {
                var store = await _crudService.GetByIdAsync(shoppingCart.StoreId);
                var notifications = new List<EmailNotification>();
                var notification = await _notificationSearchService.GetNotificationAsync<CartReminderEmailNotification>(new TenantIdentity(shoppingCart.StoreId, nameof(Store)));
                if (notification != null)
                {
                    notification.LanguageCode = shoppingCart.LanguageCode;
                    notification.ShoppingCart = shoppingCart;
                    notifications.Add(notification);
                    if(shoppingCart.Addresses != null && shoppingCart.Addresses.First().Email != null)
                    {
                        notification.SetFromToMembers(store.Email,shoppingCart.Addresses.First().Email);
                        await _notificationSender.SendNotificationAsync(notification);
                    }
                    else
                    {
                        throw new Exception("Customer Email not available");
                    }
                }
            }
        }
    }
}
