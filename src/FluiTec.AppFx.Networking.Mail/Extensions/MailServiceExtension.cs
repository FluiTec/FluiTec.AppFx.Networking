using FluiTec.AppFx.Networking.Mail.Configuration;
using FluiTec.AppFx.Networking.Mail.Configuration.Validators;
using FluiTec.AppFx.Networking.Mail.RazorLightExtensions.LocationExpanders;
using FluiTec.AppFx.Networking.Mail.RazorLightExtensions.Projects;
using FluiTec.AppFx.Networking.Mail.Services;
using FluiTec.AppFx.Options.Managers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using RazorLight;
using RazorLight.Razor;

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
            services.Configure<MailServerCertificateValidationOptions>(manager);
            services.AddScoped<IMailService, ConfigurationValidatingMailService>();
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
            services.AddScoped<ITemplatingService, RazorLightTemplatingService>();
            return services;
        }

        /// <summary>Configures the razor light.</summary>
        /// <param name="services">The services.</param>
        /// <param name="environment">The environment.</param>
        /// <returns></returns>
        public static IServiceCollection ConfigureRazorLight(this IServiceCollection services,
            IWebHostEnvironment environment)
        {
            services.AddSingleton<ILocationExpander, DefaultCultureLocationExpander>();
            services.AddSingleton<ILocationExpander, SharedCultureLocationExpander>();
            services.AddSingleton<ILocationExpander, SharedLocationExpander>();
            services.AddSingleton<ILocationExpander, DefaultLocationExpander>();
            services.AddSingleton<RazorLightProject>(provider =>
            {
                var expanders = provider.GetServices<ILocationExpander>();
                var templateOptions = provider.GetRequiredService<MailTemplateOptions>();
                var absoluteRoot = System.IO.Path.GetFullPath(System.IO.Path.Combine(environment.ContentRootPath, templateOptions.BaseDirectory));
                return new LocationExpandingRazorProject(expanders, provider.GetService<ILogger<LocationExpandingRazorProject>>(), absoluteRoot);
            });
            services.AddSingleton<IRazorLightEngine>(provider =>
            {
                var project = provider.GetRequiredService<RazorLightProject>();
                return new RazorLightEngineBuilder()
                    .UseProject(project)
                    .UseMemoryCachingProvider()
                    .Build();
            });
            return services;
        }

        /// <summary>Configures the mail service and templates.</summary>
        /// <param name="services">The services.</param>
        /// <param name="environment">The HostingEnvironment.</param>
        /// <param name="manager">The manager.</param>
        /// <returns></returns>
        // ReSharper disable once UnusedMember.Global
        public static IServiceCollection ConfigureMailServiceTemplated(this IServiceCollection services, 
            IWebHostEnvironment environment, ValidatingConfigurationManager manager)
        {
            return services
                .ConfigureMailService(manager)
                .ConfigureMailTemplateService(manager)
                .ConfigureRazorLight(environment)
                .AddScoped<ITemplatingMailService, TemplatingMailService>();
        }
    }
}
