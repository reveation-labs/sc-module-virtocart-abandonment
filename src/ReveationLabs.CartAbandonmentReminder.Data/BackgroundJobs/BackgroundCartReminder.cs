using System.Threading.Tasks;
using Hangfire;
using VirtoCommerce.Platform.Core.Settings;
using ReveationLabs.CartAbandonmentReminder.Core;
using ReveationLabs.CartAbandonmentReminder.Data.BackgroundJobs;

namespace ReveationLabs.CartAbandonmentReminder.Data.BackgroundJobs
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
