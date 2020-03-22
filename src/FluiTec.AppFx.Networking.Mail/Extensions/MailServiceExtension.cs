using FluiTec.AppFx.Networking.Mail.Configuration;
using FluiTec.AppFx.Networking.Mail.Configuration.Validators;
using FluiTec.AppFx.Options.Managers;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>Extension for MailServices.</summary>
    // ReSharper disable once UnusedMember.Global
    public static class MailServiceExtension
    {
        /// <summary>Configures the mail service.</summary>
        /// <param name="services">The services.</param>
        /// <param name="manager">The manager.</param>
        /// <returns></returns>
        public static IServiceCollection ConfigureMailService(this IServiceCollection services, 
            ValidatingConfigurationManager manager)
        {
            // configure options and validators
            manager.ConfigureValidator(new MailServiceOptionsValidator());
            services.Configure<MailServiceOptions>(manager);
            return services;
        }

        /// <summary>Configures the mail template service.</summary>
        /// <param name="services">The services.</param>
        /// <param name="manager">The manager.</param>
        /// <returns></returns>
        public static IServiceCollection ConfigureMailTemplateService(this IServiceCollection services,
            ValidatingConfigurationManager manager)
        {
            // configure options and validators
            manager.ConfigureValidator(new MailTemplateOptionsValidator());
            services.Configure<MailTemplateOptions>(manager);

            return services;
        }

        /// <summary>Configures the mail service and templates.</summary>
        /// <param name="services">The services.</param>
        /// <param name="manager">The manager.</param>
        /// <returns></returns>
        // ReSharper disable once UnusedMember.Global
        public static IServiceCollection ConfigureMailServiceTemplated(this IServiceCollection services,
            ValidatingConfigurationManager manager)
        {
            return services
                .ConfigureMailService(manager)
                .ConfigureMailTemplateService(manager);
        }
    }
}
