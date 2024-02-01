using System.Linq;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Core.Settings.Events;
using Sharpcode.CartAbandonment.Data.BackgroundJobs;

namespace Sharpcode.CartAbandonment.Data.Handlers
{
    public class ObjectSettingEntryChangedEventHandler : IEventHandler<ObjectSettingChangedEvent>
    {
        private readonly BackgroundCartJob _backgroundCartReminder;

        public ObjectSettingEntryChangedEventHandler(BackgroundCartJob backgroundCartReminder)
        {
            _backgroundCartReminder = backgroundCartReminder;
        }

        public virtual async Task Handle(ObjectSettingChangedEvent message)
        {
            if (message.ChangedEntries.Any(x => x.NewEntry.Name == Core.ModuleConstants.Settings.General.CartAbandonmentEnabled.Name
                                   || x.NewEntry.Name == Core.ModuleConstants.Settings.General.CronExpression.Name))
            {
                await _backgroundCartReminder.ConfigureProcessCartReminderJob();
            }
        }
    }
}
