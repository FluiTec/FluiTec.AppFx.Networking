using FluiTec.AppFx.Networking.Mail.Configuration;
using FluiTec.AppFx.Networking.Mail.Configuration.Validators;
using FluiTec.AppFx.Options.Managers;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>Extension for MailServices.</summary>
    public static class MailServiceExtension
    {
        /// <summary>Configures the mail service.</summary>
        /// <param name="services">The services.</param>
        /// <param name="manager">The manager.</param>
        /// <returns></returns>
        public static IServiceCollection ConfigureMailService(this IServiceCollection services, ValidatingConfigurationManager manager)
        {
            // configure options and validators
            services.Configure<MailServiceOptions>(manager);
            manager.ConfigureValidator(new MailServiceOptionsValidator());
            return services;
        }
    }
}
