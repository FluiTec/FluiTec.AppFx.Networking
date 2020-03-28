using System;
using System.IO;
using FLuiTec.AppFx.Networking.Mail.ConsoleSample.Configuration;
using FLuiTec.AppFx.Networking.Mail.ConsoleSample.Helpers;
using FLuiTec.AppFx.Networking.Mail.ConsoleSample.MailModels;
using FluiTec.AppFx.Networking.Mail.Services;
using FluiTec.AppFx.Options.Exceptions;
using FluiTec.AppFx.Options.Managers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FLuiTec.AppFx.Networking.Mail.ConsoleSample
{
    internal class Program
    {
        private static void Main()
        {
            var serviceProvider = ConfigureServices(Configure(ApplicationHelper.GetApplicationPath()));
            var service = serviceProvider.GetRequiredService<ITemplatingMailService>();
            var appSettings = serviceProvider.GetRequiredService<ApplicationSettings>();
            service.SendMail(new Test(appSettings.SampleSubject), appSettings.RecipientMail, appSettings.RecipientName);
        }

        private static IConfigurationRoot Configure(string path)
        {
            try
            {
                Console.WriteLine($"BasePath: {path}");
                return new ConfigurationBuilder()
                    .SetBasePath(path)
                    .AddJsonFile("appsettings.json", false, true)
                    .AddJsonFile("appsettings.secret.json", false, true)
                    .Build();
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine(
                    "######################################################################################################################");
                Console.WriteLine(
                    "In order to run this sample you need to add a custom appsettings.secret.json-File containing additional configuration.");
                Console.WriteLine("Mine contains settings for the following values for example:");
                Console.WriteLine("- MailServiceOptions:SmtpServer");
                Console.WriteLine("- MailServiceOptions:FromMail");
                Console.WriteLine("- MailServiceOptions:Username");
                Console.WriteLine("- MailServiceOptions:Password");
                Console.WriteLine("- AppSettings:RecipientMail");
                Console.WriteLine("- AppSettings:RecipientName");
                Console.WriteLine(
                    "######################################################################################################################");
                throw;
            }
        }

        private static IServiceProvider ConfigureServices(IConfigurationRoot config)
        {
            try
            {
                var environment = new TestHostingEnvironment
                    {ContentRootPath = ApplicationHelper.GetApplicationCodebase()};
                var manager = new ConsoleReportingConfigurationManager(config);
                var services = new ServiceCollection();
                services.ConfigureMailServiceTemplated(environment, manager);
                services.Configure<ApplicationSettings>(manager, true);
                services.AddLogging(builder => builder
                    .AddFilter("Microsoft", LogLevel.Warning)
                    .AddFilter("System", LogLevel.Warning)
                    .AddConsole());
                return services.BuildServiceProvider();
            }
            catch (MissingSettingException e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}