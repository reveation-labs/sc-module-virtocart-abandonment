using System.Collections.Generic;
using VirtoCommerce.Platform.Core.Settings;

namespace ReveationLabs.CartAbandonmentReminder.Core
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
                    GroupName = "Cart Abandonment Reminder|General",
                    ValueType = SettingValueType.Boolean,
                    DefaultValue = false,
                };

                public static readonly SettingDescriptor CronExpression = new ()
                {
                    Name = "CartAbandonmentReminder.CronExpression",
                    GroupName = "Cart Abandonment Reminder|General",
                    ValueType = SettingValueType.ShortText
                };

                public static readonly SettingDescriptor CronTime = new()
                {
                    Name = "CartAbandonmentReminder.CronTime",
                    GroupName = "Cart Abandonment Reminder|General",
                    ValueType = SettingValueType.Integer
                };

                public static IEnumerable<SettingDescriptor> AllGeneralSettings
                {
                    get
                    {
                        yield return CartAbandonmentReminderEnabled;
                        yield return CronExpression;
                        yield return CronTime;
                    }
                }
            }
            public static class CartAbandonmentStoreSettings
            {
                public static readonly SettingDescriptor EnableCartReminder = new()
                {
                    Name = "CartAbandonmentReminder.EnableCartReminder",
                    GroupName = "Cart Abandonment Reminder|Cart Abandonment Reminder",
                    ValueType = SettingValueType.Boolean,
                    DefaultValue = false
                };

                public static readonly SettingDescriptor RemindUserAnonymous = new()
                {
                    Name = "CartAbandonmentReminder.RemindUserAnonymous",
                    GroupName = "Cart Abandonment Reminder|Cart Abandonment Reminder",
                    ValueType = SettingValueType.Boolean,
                    DefaultValue = true
                };

                public static readonly SettingDescriptor RemindUserLogin = new()
                {
                    Name = "CartAbandonmentReminder.RemindUserLogin",
                    GroupName = "Cart Abandonment Reminder|Cart Abandonment Reminder",
                    ValueType = SettingValueType.Boolean,
                    DefaultValue = true
                };
                public static IEnumerable<SettingDescriptor> StoreLevelSettings
                {
                    get
                    {
                        yield return EnableCartReminder;
                        yield return RemindUserAnonymous;
                        yield return RemindUserLogin;
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
                    return CartAbandonmentStoreSettings.StoreLevelSettings;
                }
            }
        }
    }
}
