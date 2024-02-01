using System.Collections.Generic;
using VirtoCommerce.Platform.Core.Settings;

namespace Sharpcode.CartAbandonment.Core
{
    public static class ModuleConstants
    {
        public static class Security
        {
            public static class Permissions
            {
                public const string Access = "CartAbandonment:access";
                public const string Create = "CartAbandonment:create";
                public const string Read = "CartAbandonment:read";
                public const string Update = "CartAbandonment:update";
                public const string Delete = "CartAbandonment:delete";

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
                public static SettingDescriptor CartAbandonmentEnabled { get; } = new SettingDescriptor
                {
                    Name = "CartAbandonment.CartAbandonmentEnabled",
                    GroupName = "Cart Abandonment Reminder|General",
                    ValueType = SettingValueType.Boolean,
                    DefaultValue = false,
                };

                public static readonly SettingDescriptor CronExpression = new ()
                {
                    Name = "CartAbandonment.CronExpression",
                    GroupName = "Cart Abandonment Reminder|General",
                    ValueType = SettingValueType.ShortText
                };

                public static readonly SettingDescriptor CronTime = new()
                {
                    Name = "CartAbandonment.CronTime",
                    GroupName = "Cart Abandonment Reminder|General",
                    ValueType = SettingValueType.Integer
                };

                public static IEnumerable<SettingDescriptor> AllGeneralSettings
                {
                    get
                    {
                        yield return CartAbandonmentEnabled;
                        yield return CronExpression;
                        yield return CronTime;
                    }
                }
            }
            public static class CartAbandonmentStoreSettings
            {
                public static readonly SettingDescriptor EnableCartReminder = new()
                {
                    Name = "CartAbandonment.EnableCartReminder",
                    GroupName = "Cart Abandonment Reminder|Cart Abandonment Reminder",
                    ValueType = SettingValueType.Boolean,
                    DefaultValue = false
                };

                public static readonly SettingDescriptor RemindUserAnonymous = new()
                {
                    Name = "CartAbandonment.RemindUserAnonymous",
                    GroupName = "Cart Abandonment Reminder|Cart Abandonment Reminder",
                    ValueType = SettingValueType.Boolean,
                    DefaultValue = true
                };

                public static readonly SettingDescriptor RemindUserLogin = new()
                {
                    Name = "CartAbandonment.RemindUserLogin",
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
