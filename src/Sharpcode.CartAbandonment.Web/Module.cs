using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Settings;
using Sharpcode.CartAbandonment.Core;
using VirtoCommerce.StoreModule.Core.Model;
using VirtoCommerce.NotificationsModule.Core.Services;
using VirtoCommerce.Platform.Core.Bus;
using VirtoCommerce.Platform.Core.Settings.Events;
using Sharpcode.CartAbandonment.Data.BackgroundJobs;
using Sharpcode.CartAbandonment.Core.Services;
using Sharpcode.CartAbandonment.Data.Repositories;
using Sharpcode.CartAbandonment.Data.Notifications;
using Hangfire;
using Sharpcode.CartAbandonment.Data.Handlers;

namespace Sharpcode.CartAbandonment.Web
{
    public class Module : IModule, IHasConfiguration
    {
        public ManifestModuleInfo ModuleInfo { get; set; }
        public IConfiguration Configuration { get; set; }

        public void Initialize(IServiceCollection serviceCollection)
        {
            // Initialize database
            var connectionString = Configuration.GetConnectionString(ModuleInfo.Id) ??
                                   Configuration.GetConnectionString("VirtoCommerce");

            

            // Override models
            //AbstractTypeFactory<OriginalModel>.OverrideType<OriginalModel, ExtendedModel>().MapToType<ExtendedEntity>();
            //AbstractTypeFactory<OriginalEntity>.OverrideType<OriginalEntity, ExtendedEntity>();

            // Register services
            //serviceCollection.AddTransient<IMyService, MyService>();
            serviceCollection.AddTransient<ObjectSettingEntryChangedEventHandler>();
            serviceCollection.AddTransient<BackgroundCartJob>();
            serviceCollection.AddTransient<IExtendShoppingCartSearchService, ExtendShoppingCartSearchService>();
            serviceCollection.AddTransient<ISendCartReminderEmailNotification,SendCartReminderEmailNotification>();
            serviceCollection.AddHangfire(sp => { sp.UseSqlServerStorage(connectionString); });
            serviceCollection.AddHangfireServer();
        }

        public async void PostInitialize(IApplicationBuilder appBuilder)
        {
            appBuilder.UseHangfireDashboard();
            var serviceProvider = appBuilder.ApplicationServices;
            // Register settings
            var settingsRegistrar = serviceProvider.GetRequiredService<ISettingsRegistrar>();
            settingsRegistrar.RegisterSettings(ModuleConstants.Settings.AllSettings, ModuleInfo.Id);
            settingsRegistrar.RegisterSettings(ModuleConstants.Settings.StoreLevelSettings, ModuleInfo.Id);
            settingsRegistrar.RegisterSettingsForType(ModuleConstants.Settings.StoreLevelSettings, typeof(Store).Name);
            
            var notificationregistrar = appBuilder.ApplicationServices.GetService<INotificationRegistrar>();
            notificationregistrar.RegisterNotification<CartReminderEmailNotification>();

            // Register permissions
            var permissionsRegistrar = serviceProvider.GetRequiredService<IPermissionsRegistrar>();
            permissionsRegistrar.RegisterPermissions(ModuleConstants.Security.Permissions.AllPermissions
                .Select(x => new Permission { ModuleId = ModuleInfo.Id, GroupName = "CartAbandonment", Name = x })
                .ToArray());

            //Schedule periodic subscription processing job
            var jobsRunner = appBuilder.ApplicationServices.GetService<BackgroundCartJob>();
            await jobsRunner.ConfigureProcessCartReminderJob();

            var inProcessBus = appBuilder.ApplicationServices.GetService<IHandlerRegistrar>();
            inProcessBus.RegisterHandler<ObjectSettingChangedEvent>(async (message, token) => await appBuilder.ApplicationServices.GetService<ObjectSettingEntryChangedEventHandler>().Handle(message));

            // Apply migrations
                        
        }

        public void Uninstall()
        {
        }
    }
}
