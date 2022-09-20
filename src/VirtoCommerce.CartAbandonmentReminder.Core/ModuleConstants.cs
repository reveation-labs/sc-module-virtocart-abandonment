using System.Collections.Generic;
using VirtoCommerce.Platform.Core.Settings;

namespace VirtoCommerce.CartAbandonmentReminder.Core
{
    public static class ModuleConstants
    {
        public static class Security
        {
            public static class Permissions
            {
                public const string Access = "CartAbandonmentReminder:access";
                public const string Create = "CartAbandonmentReminder:create";
                public const string Read = "CartAbandonmentReminder:read";
                public const string Update = "CartAbandonmentReminder:update";
                public const string Delete = "CartAbandonmentReminder:delete";

                public static string[] AllPermissions { get; } =
                {
                    Access,
                    Create,
                    Read,
                    Update,
                    Delete,
                };
            }
        }

        public static class Settings
        {
            public static class General
            {
                public static SettingDescriptor CartAbandonmentReminderEnabled { get; } = new SettingDescriptor
                {
                    Name = "CartAbandonmentReminder.CartAbandonmentReminderEnabled",
                    GroupName = "CartAbandonmentReminder|General",
                    ValueType = SettingValueType.Boolean,
                    DefaultValue = false,
                };

                public static readonly SettingDescriptor CronExpression = new SettingDescriptor
                {
                    Name = "CartAbandonmentReminder.CronExpression",
                    GroupName = "CartAbandonmentReminder|General",
                    ValueType = SettingValueType.ShortText,
                    DefaultValue = "0 */1 * * *"
                };
                public static readonly SettingDescriptor CartAbandonmentTime = new SettingDescriptor
                {
                    Name = "CartAbandonmentReminder.CronExpression",
                    GroupName = "CartAbandonmentReminder|General",
                    ValueType = SettingValueType.Integer
                };
                public static IEnumerable<SettingDescriptor> StoreLevelSettings
                {
                    get
                    {
                        yield return CartAbandonmentTime;
                    }
                }
                public static IEnumerable<SettingDescriptor> AllGeneralSettings
                {
                    get
                    {
                        yield return CartAbandonmentReminderEnabled;
                        yield return CronExpression;
                    }
                }
            }
            public static IEnumerable<SettingDescriptor> AllSettings
            {
                get
                {
                    return General.AllGeneralSettings;
                }
            }
            public static IEnumerable<SettingDescriptor> StoreLevelSettings
            {
                get
                {
                    return General.StoreLevelSettings;
                }
            }
        }
    }
}
