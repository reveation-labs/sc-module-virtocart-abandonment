using System.Threading.Tasks;
using Hangfire;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.CartAbandonmentReminder.Core;
using VirtoCommerce.CartAbandonmentReminder.Data.BackgroundJobs;

namespace VirtoCommerce.CartAbandonmentReminder.Data.BackgroundJobs
{
    public class BackgroundCartReminder
    {
        private readonly ISettingsManager _settingsManager;

        public BackgroundCartReminder(ISettingsManager settingsManager)
        {
            _settingsManager = settingsManager;
        }

        public async Task ConfigureProcessCartReminderJob()
        {
            var processJobEnable = await _settingsManager.GetValueAsync(ModuleConstants.Settings.General.CartAbandonmentReminderEnabled.Name, true);
            if (processJobEnable)
            {
                var cronExpression = _settingsManager.GetValue(ModuleConstants.Settings.General.CronExpression.Name, "0 0 */1 * *");
                RecurringJob.AddOrUpdate<ProcessCartRemider>("ProcessCartRemiderJob", x => x.Process(), cronExpression);
            }
            else
            {
                RecurringJob.RemoveIfExists("ProcessCartRemiderJob");
            }
        }
    }
}
