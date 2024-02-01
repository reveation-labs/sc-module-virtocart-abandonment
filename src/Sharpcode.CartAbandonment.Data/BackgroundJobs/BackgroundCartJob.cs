using System.Threading.Tasks;
using Hangfire;
using VirtoCommerce.Platform.Core.Settings;
using Sharpcode.CartAbandonment.Core;

namespace Sharpcode.CartAbandonment.Data.BackgroundJobs
{
    public class BackgroundCartJob
    {
        private readonly ISettingsManager _settingsManager;        

        public BackgroundCartJob(ISettingsManager settingsManager)
        {
            _settingsManager = settingsManager;            
        }

        public async Task ConfigureProcessCartReminderJob()
        {
            var processJobEnable = await _settingsManager.GetValueAsync<bool>(ModuleConstants.Settings.General.CartAbandonmentEnabled);
            if (processJobEnable)
            {
                var cronExpression = await _settingsManager.GetValueAsync<string>(ModuleConstants.Settings.General.CronExpression);
                RecurringJob.AddOrUpdate<ProcessCartRemider>("ProcessCartRemiderJob", x => x.Process(), cronExpression);                
            }
            else
            {
                RecurringJob.RemoveIfExists("ProcessCartRemiderJob");
            }
        }
    }
}
