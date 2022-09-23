using System;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using VirtoCommerce.CartAbandonmentReminder.Core;
using VirtoCommerce.CartAbandonmentReminder.Core.Services;
using VirtoCommerce.CartModule.Core.Model;
using VirtoCommerce.CartModule.Core.Model.Search;
using VirtoCommerce.Platform.Core.GenericCrud;
using VirtoCommerce.Platform.Core.Settings;

namespace VirtoCommerce.CartAbandonmentReminder.Data.BackgroundJobs
{
    public class ProcessCartRemider
    {
        ISearchService<ShoppingCartSearchCriteria, ShoppingCartSearchResult, ShoppingCart> _searchService;
        private readonly ISendCartReminderEmailNotification _sendCartReminderEmailNotification;
        private readonly ISettingsManager _settingsManager;
        public ProcessCartRemider(ISendCartReminderEmailNotification sendCartReminderEmailNotification, ISearchService<ShoppingCartSearchCriteria, ShoppingCartSearchResult, ShoppingCart> searchService,ISettingsManager settingsManager)
        {
            _sendCartReminderEmailNotification = sendCartReminderEmailNotification;
            _searchService = searchService;
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
            var dateTime = DateTime.Now;
            var cartAbandonmentStartTime = _settingsManager.GetValue(ModuleConstants.Settings.CartAbandonmentTime.CartAbandonmentStartTime.Name,24);
            var cartAbandonmentEndTime = _settingsManager.GetValue(ModuleConstants.Settings.CartAbandonmentTime.CartAbandonmentEndTime.Name,24);
            var startDateTime = dateTime.AddHours(-cartAbandonmentStartTime);
            var endDateTime = dateTime.AddHours(-cartAbandonmentEndTime);
            var shoppingCartSearchCritera = new ShoppingCartSearchCriteria
            {
                CreatedStartDate = startDateTime,
                ModifiedEndDate = endDateTime,
                Skip = 0,
                ResponseGroup = response.ToString()
            };
            var shoppingCarts = await _searchService.SearchAsync(shoppingCartSearchCritera);
            var carts = shoppingCarts.Results;
            await _sendCartReminderEmailNotification.TryToSendCartReminderAsync(carts.ToList());
        }

    }
}
