using System;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using ReveationLabs.CartAbandonmentReminder.Core;
using ReveationLabs.CartAbandonmentReminder.Core.Services;
using VirtoCommerce.CartModule.Core.Model;
using VirtoCommerce.CartModule.Core.Model.Search;
using VirtoCommerce.Platform.Core.GenericCrud;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.Platform.Data.Settings;
using VirtoCommerce.StoreModule.Core.Model;
using VirtoCommerce.StoreModule.Core.Model.Search;

namespace ReveationLabs.CartAbandonmentReminder.Data.BackgroundJobs
{
    public class ProcessCartRemider
    {
        private readonly ISearchService<ShoppingCartSearchCriteria, ShoppingCartSearchResult, ShoppingCart> _cartSearchService;
        private readonly ISearchService<StoreSearchCriteria, StoreSearchResult, Store> _storeSearchService;
        private readonly ISendCartReminderEmailNotification _sendCartReminderEmailNotification;
        private readonly ISettingsManager _settingsManager;
        public ProcessCartRemider(
            ISendCartReminderEmailNotification sendCartReminderEmailNotification,
            ISearchService<ShoppingCartSearchCriteria, ShoppingCartSearchResult, ShoppingCart> cartSearchService,
            ISearchService<StoreSearchCriteria, StoreSearchResult, Store> storeSearchService,
            ISettingsManager settingsManager)
        {
            _sendCartReminderEmailNotification = sendCartReminderEmailNotification;
            _cartSearchService = cartSearchService;
            _storeSearchService = storeSearchService;
            _settingsManager = settingsManager;
        }

        [DisableConcurrentExecution(10)]
        // "DisableConcurrentExecutionAttribute" prevents to start simultaneous job payloads.
        // Should have short timeout, because this attribute implemented by following manner: newly started job falls into "processing" state immediately.
        // Then it tries to receive job lock during timeout. If the lock received, the job starts payload.
        // When the job is awaiting desired timeout for lock release, it stucks in "processing" anyway. (Therefore, you should not to set long timeouts (like 24*60*60), this will cause a lot of stucked jobs and performance degradation.)
        // Then, if timeout is over and the lock NOT acquired, the job falls into "scheduled" state (this is default fail-retry scenario).
        // Failed job goes to "Failed" state (by default) after retries exhausted.
        public async Task Process()
        {
            var response = CartResponseGroup.Full;
            var dateTime = DateTime.UtcNow;
            var storeSearch = await _storeSearchService.SearchAsync(new StoreSearchCriteria { Skip = 0,Take = 1000 });
            var stores = storeSearch.Results.ToList();
            foreach (var store in stores)
            {
                // store level settings
                var cronTime = _settingsManager.GetValue(ModuleConstants.Settings.General.CronTime.Name, 24);
                var isStoreAllowed = store.Settings.GetSettingValue(ModuleConstants.Settings.CartAbandonmentStoreSettings.EnableCartReminder.Name, false);
                if(isStoreAllowed)
                {
                    var endDateTime = dateTime.AddHours(-cronTime);
                    var startDateTime = endDateTime.AddHours(-cronTime);
                    var shoppingCartSearchCritera = new ShoppingCartSearchCriteria
                    {
                        ModifiedStartDate = startDateTime,
                        ModifiedEndDate = endDateTime,
                        Skip = 0,
                        ResponseGroup = response.ToString(),
                        StoreId = store.Id,
                        Take = 1000
                    };
                    // get shopping carts based on search criteria
                    var shoppingCarts = (await _cartSearchService.SearchAsync(shoppingCartSearchCritera)).Results.ToList();
                    if(shoppingCarts.Count > 0)
                    {
                        await _sendCartReminderEmailNotification.TryToSendCartReminderAsync(shoppingCarts,store);
                    }
                }
            }
        }
    }
}
