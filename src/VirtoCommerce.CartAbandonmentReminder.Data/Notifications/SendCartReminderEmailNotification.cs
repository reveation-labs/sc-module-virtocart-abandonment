using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EntityFrameworkCore.Triggers;
using Humanizer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VirtoCommerce.CartAbandonmentReminder.Core.Services;
using VirtoCommerce.CartModule.Core.Model;
using VirtoCommerce.CartModule.Core.Model.Search;
using VirtoCommerce.NotificationsModule.Core.Extensions;
using VirtoCommerce.NotificationsModule.Core.Model;
using VirtoCommerce.NotificationsModule.Core.Model.Search;
using VirtoCommerce.NotificationsModule.Core.Services;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Core.Extensions;
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
        private readonly INotificationMessageSearchService _notificationMessageSearchService;
        public SendCartReminderEmailNotification(INotificationSender notificationSender, ICrudService<Store> crudService,
                                                       ISettingsManager settingsManager, UserManager<ApplicationUser> userManager,
                                                       INotificationSearchService notificationSearchService,
                                                       ISettingsSearchService settingsSearchService, INotificationMessageSearchService notificationMessageSearchService)
        {
            _notificationSender = notificationSender;
            _crudService = crudService;
            _settingsManager = settingsManager;
            _notificationSearchService = notificationSearchService;
            _userManager = userManager;
            _settingsSearchService = settingsSearchService;
            _notificationMessageSearchService = notificationMessageSearchService;
        }

        public async Task TryToSendCartReminderAsync(List<ShoppingCart> shoppingCarts)
        {
            var storeIds = shoppingCarts.Select(x => x.StoreId);
            var stores = await _crudService.GetAsync(storeIds.ToList(),StoreResponseGroup.StoreInfo.ToString());
            var searchNotification = new NotificationMessageSearchCriteria
            {
                Take = 100,
                NotificationType = "CartReminderEmailNotification"

            };
            var startDateTime = DateTime.Now.AddHours(-24);
            var oldNotifications = await _notificationMessageSearchService.SearchMessageAsync(searchNotification);
            var notificationList = new List<EmailNotificationMessage>();
            foreach(var oldNotification in oldNotifications.Results)
            {
                notificationList.Add((EmailNotificationMessage)oldNotification);
            }

            foreach (var shoppingCart in shoppingCarts)
            {
                var checkNotified = notificationList.Where(x => x.To == shoppingCart.Addresses.First().Email);

                if (checkNotified.Any(x => x.ModifiedDate > startDateTime))
                {
                    throw new Exception("User Is Already Notified");
                }
                else
                {
                    var store = stores.Where(x => x.Id == shoppingCart.StoreId).First();
                    var notifications = new List<EmailNotification>();
                    var notification = await _notificationSearchService.GetNotificationAsync<CartReminderEmailNotification>(new TenantIdentity(shoppingCart.StoreId, nameof(Store)));
                    if (notification != null)
                    {
                        notification.LanguageCode = shoppingCart.LanguageCode;
                        notification.ShoppingCart = shoppingCart;
                        notifications.Add(notification);
                        if (shoppingCart.Addresses != null)
                        {
                            notification.SetFromToMembers(store.Email, shoppingCart.Addresses.First().Email);
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
}
