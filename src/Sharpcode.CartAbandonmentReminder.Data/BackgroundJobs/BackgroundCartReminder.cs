using System.Threading.Tasks;
using Hangfire;
using VirtoCommerce.Platform.Core.Settings;
using Sharpcode.CartAbandonmentReminder.Core;

namespace Sharpcode.CartAbandonmentReminder.Data.BackgroundJobs
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
            var processJobEnable = await _settingsManager.GetValueAsync<bool>(ModuleConstants.Settings.General.CartAbandonmentReminderEnabled);
            if (processJobEnable)
            {
                var cronExpression = await _settingsManager.GetValueAsync<string>(ModuleConstants.Settings.General.CronExpression);
                RecurringJob.AddOrUpdate<ProcessCartRemider>("ProcessCartRemiderJob", x => x.Process(), cronExpression);
                //RecurringJob.TriggerJob("ProcessCartRemiderJob");
            }
            else
            {
                RecurringJob.RemoveIfExists("ProcessCartRemiderJob");
            }
        }
    }
}
