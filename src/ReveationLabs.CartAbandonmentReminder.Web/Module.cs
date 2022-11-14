using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Settings;
using ReveationLabs.CartAbandonmentReminder.Core;
using ReveationLabs.CartAbandonmentReminder.Data.Repositories;
using VirtoCommerce.StoreModule.Core.Model;
using ReveationLabs.CartAbandonmentReminder.Data.BackgroundJobs;
using VirtoCommerce.NotificationsModule.Core.Services;
using ReveationLabs.CartAbandonmentReminder.Data.Notifications;
using ReveationLabs.CartAbandonmentReminder.Core.Services;
using VirtoCommerce.CartModule.Core.Model.Search;
using VirtoCommerce.CartModule.Core.Model;
using VirtoCommerce.CartModule.Data.Services;
using VirtoCommerce.Platform.Core.GenericCrud;
using ReveationLabs.CartAbandonmentReminder.Data.Handlers;
using VirtoCommerce.Platform.Core.Bus;
using VirtoCommerce.Platform.Core.Settings.Events;

namespace ReveationLabs.CartAbandonmentReminder.Web
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

            serviceCollection.AddDbContext<CartAbandonmentReminderDbContext>(options => options.UseSqlServer(connectionString));

            // Override models
            //AbstractTypeFactory<OriginalModel>.OverrideType<OriginalModel, ExtendedModel>().MapToType<ExtendedEntity>();
            //AbstractTypeFactory<OriginalEntity>.OverrideType<OriginalEntity, ExtendedEntity>();

            // Register services
            //serviceCollection.AddTransient<IMyService, MyService>();
            serviceCollection.AddTransient<ObjectSettingEntryChangedEventHandler>();
            serviceCollection.AddTransient<BackgroundCartReminder>();
            serviceCollection.AddTransient<IExtendShoppingCartSearchService, ExtendShoppingCartSearchService>();
            serviceCollection.AddTransient<ISendCartReminderEmailNotification,SendCartReminderEmailNotification>();
        }

        public void PostInitialize(IApplicationBuilder appBuilder)
        {
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
                .Select(x => new Permission { ModuleId = ModuleInfo.Id, GroupName = "CartAbandonmentReminder", Name = x })
                .ToArray());

            //Schedule periodic subscription processing job
            var jobsRunner = appBuilder.ApplicationServices.GetService<BackgroundCartReminder>();
            jobsRunner.ConfigureProcessCartReminderJob().GetAwaiter().GetResult();

            var inProcessBus = appBuilder.ApplicationServices.GetService<IHandlerRegistrar>();
            inProcessBus.RegisterHandler<ObjectSettingChangedEvent>(async (message, token) => await appBuilder.ApplicationServices.GetService<ObjectSettingEntryChangedEventHandler>().Handle(message));

            // Apply migrations
            using var serviceScope = serviceProvider.CreateScope();
            using var dbContext = serviceScope.ServiceProvider.GetRequiredService<CartAbandonmentReminderDbContext>();
            dbContext.Database.Migrate();
        }

        public void Uninstall()
        {
        }
    }
}
