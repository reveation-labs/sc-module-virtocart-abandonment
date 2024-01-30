using System.Linq;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Core.Settings.Events;
using Sharpcode.CartAbandonmentReminder.Data.BackgroundJobs;

namespace Sharpcode.CartAbandonmentReminder.Data.Handlers
{
    public class ObjectSettingEntryChangedEventHandler : IEventHandler<ObjectSettingChangedEvent>
    {
        private readonly BackgroundCartReminder _backgroundCartReminder;

        public ObjectSettingEntryChangedEventHandler(BackgroundCartReminder backgroundCartReminder)
        {
            _backgroundCartReminder = backgroundCartReminder;
        }

        public virtual async Task Handle(ObjectSettingChangedEvent message)
        {
            if (message.ChangedEntries.Any(x => x.NewEntry.Name == Core.ModuleConstants.Settings.General.CartAbandonmentReminderEnabled.Name
                                   || x.NewEntry.Name == Core.ModuleConstants.Settings.General.CronExpression.Name))
            {
                await _backgroundCartReminder.ConfigureProcessCartReminderJob();
            }
        }
    }
}
