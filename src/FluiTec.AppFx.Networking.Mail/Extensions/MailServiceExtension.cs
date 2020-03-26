using System;
using System.IO;
using FluiTec.AppFx.Networking.Mail.Configuration;
using FluiTec.AppFx.Networking.Mail.Configuration.Validators;
using FluiTec.AppFx.Networking.Mail.Factories;
using FluiTec.AppFx.Networking.Mail.RazorLightExtensions.LocationExpanders;
using FluiTec.AppFx.Networking.Mail.RazorLightExtensions.Projects;
using FluiTec.AppFx.Networking.Mail.Services;
using FluiTec.AppFx.Options.Managers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
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
            services.AddSingleton<IMailTransportFactory, MailKitSmtpTransportFactory>();
            services.AddScoped<IMailService, CertificateValidatingMailService>(provider => new CertificateValidatingMailService(
                provider.GetRequiredService<IOptionsMonitor<MailServiceOptions>>(), 
                provider.GetService<ILogger<CertificateValidatingMailService>>() , 
                provider.GetRequiredService<IOptionsMonitor<MailServerCertificateValidationOptions>>(),
                provider.GetRequiredService<IMailTransportFactory>()));
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
        /// <param name="assemblyRootType">A root type of the assembly.</param>
        /// <returns></returns>
        public static IServiceCollection ConfigureRazorLight(this IServiceCollection services,
            IWebHostEnvironment environment, Type assemblyRootType = null)
        {
            services.AddSingleton<IFileLocationExpander, DefaultCultureLocationExpander>();
            services.AddSingleton<IResourceExpander, DefaultCultureLocationExpander>();

            services.AddSingleton<IFileLocationExpander, SharedCultureLocationExpander>();
            services.AddSingleton<IResourceExpander, SharedCultureLocationExpander>();

            services.AddSingleton<IFileLocationExpander, SharedLocationExpander>();
            services.AddSingleton<IResourceExpander, SharedLocationExpander>();

            services.AddSingleton<IFileLocationExpander, DefaultLocationExpander>();
            services.AddSingleton<IResourceExpander, DefaultLocationExpander>();

            services.AddSingleton<RazorLightProject>(provider =>
            {
                var templateOptions = provider.GetRequiredService<MailTemplateOptions>();
                switch (templateOptions.TemplateSource)
                {
                    case MailTemplateOptions.MailTemplateSource.File:
                        var fileExpanders = provider.GetServices<IFileLocationExpander>();
                        var absoluteRoot = Path.GetFullPath(Path.Combine(environment.ContentRootPath, templateOptions.BaseDirectory));
                        return new LocationExpandingFileRazorProject(fileExpanders,
                            provider.GetService<ILogger<LocationExpandingFileRazorProject>>(), absoluteRoot,
                            templateOptions.Extension);
                    case MailTemplateOptions.MailTemplateSource.Embedded:
                        var resourceExpanders = provider.GetServices<IResourceExpander>();
                        return new LocationExpandingEmbeddedRazorProject(assemblyRootType, resourceExpanders,
                            provider.GetService<ILogger<LocationExpandingFileRazorProject>>(),
                            templateOptions.DefaultNamespace);
                    default:
                        throw new NotImplementedException();
                }
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
        /// <param name="assemblyRootType">A root type of the assembly.</param>
        /// <returns></returns>
        // ReSharper disable once UnusedMember.Global
        public static IServiceCollection ConfigureMailServiceTemplated(this IServiceCollection services, 
            IWebHostEnvironment environment, ValidatingConfigurationManager manager, Type assemblyRootType = null)
        {
            return services
                .ConfigureMailService(manager)
                .ConfigureMailTemplateService(manager)
                .ConfigureRazorLight(environment, assemblyRootType)
                .AddScoped<ITemplatingMailService, TemplatingMailService>();
        }
    }
}
