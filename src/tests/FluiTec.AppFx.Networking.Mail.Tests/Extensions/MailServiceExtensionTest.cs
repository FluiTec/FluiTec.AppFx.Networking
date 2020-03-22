using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using FluiTec.AppFx.Networking.Mail.Configuration;
using FluiTec.AppFx.Networking.Mail.Services;
using FluiTec.AppFx.Options.Exceptions;
using FluiTec.AppFx.Options.Managers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluiTec.AppFx.Networking.Mail.Tests.Extensions
{
    [TestClass]
    public class MailServiceExtensionTest
    {
        [TestMethod]
        public void AddsMailServiceOptions()
        {
            var services = new ServiceCollection();
            var builder = new ConfigurationBuilder()
                .AddInMemoryCollection(new[]
                {
                    new KeyValuePair<string, string>("MailServiceOptions:SmtpServer","smtp.test.com"),
                    new KeyValuePair<string, string>("MailServiceOptions:FromMail","mail@test.com"),
                    new KeyValuePair<string, string>("MailServiceOptions:FromName","Mail"),
                });
            var config = builder.Build();
            var manager = new ConsoleReportingConfigurationManager(config);
            services.ConfigureMailService(manager);

            var provider = services.BuildServiceProvider();
            var settings = provider.GetService<MailServiceOptions>();
            Assert.IsNotNull(settings);
        }

        [TestMethod]
        public void AddsMailTemplateOptions()
        {
            var services = new ServiceCollection();
            var builder = new ConfigurationBuilder()
                .AddInMemoryCollection(new[]
                {
                    new KeyValuePair<string, string>("MailTemplateOptions:BaseDirectory","MailViews"),
                    new KeyValuePair<string, string>("MailTemplateOptions:Extension",".cshtml"),
                });
            var config = builder.Build();
            var manager = new ConsoleReportingConfigurationManager(config);
            services.ConfigureMailTemplateService(manager);

            var provider = services.BuildServiceProvider();
            var settings = provider.GetService<MailTemplateOptions>();
            Assert.IsNotNull(settings);
        }

        [TestMethod]
        public void AddsMailServiceValidators()
        {
            var services = new ServiceCollection();
            var builder = new ConfigurationBuilder()
                .AddInMemoryCollection(new[]
                {
                    new KeyValuePair<string, string>("MailServiceOptions:SmtpServer","smtp.test.com"),
                    new KeyValuePair<string, string>("MailServiceOptions:FromMail","mail@test.com"),
                    new KeyValuePair<string, string>("MailServiceOptions:FromName","Mail"),
                });
            var config = builder.Build();
            var manager = new ConsoleReportingConfigurationManager(config);
            services.ConfigureMailService(manager);
            var validator = manager.Validators[typeof(MailServiceOptions)];
            Assert.IsNotNull(validator);
        }

        [TestMethod]
        public void AddsMailTemplateValidators()
        {
            var services = new ServiceCollection();
            var builder = new ConfigurationBuilder()
                .AddInMemoryCollection(new[]
                {
                    new KeyValuePair<string, string>("MailTemplateOptions:BaseDirectory","MailViews"),
                    new KeyValuePair<string, string>("MailTemplateOptions:Extension",".cshtml"),
                });
            var config = builder.Build();
            var manager = new ConsoleReportingConfigurationManager(config);
            services.ConfigureMailTemplateService(manager);
            var validator = manager.Validators[typeof(MailTemplateOptions)];
            Assert.IsNotNull(validator);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void ValidatesMailServiceOptions()
        {
            var services = new ServiceCollection();
            var builder = new ConfigurationBuilder()
                .AddInMemoryCollection(new[]
                {
                    new KeyValuePair<string, string>("MailServiceOptions:SmtpServer","smtp.test.com"),
                });
            var config = builder.Build();
            var manager = new ConsoleReportingConfigurationManager(config);

            // raises exception for not configured settings
            services.ConfigureMailService(manager);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void ValidatesMailTemplateOptions()
        {
            var services = new ServiceCollection();
            var builder = new ConfigurationBuilder()
                .AddInMemoryCollection(new[]
                {
                    new KeyValuePair<string, string>("MailTemplateOptions:Extension","cshtml"),
                });
            var config = builder.Build();
            var manager = new ConsoleReportingConfigurationManager(config);

            // raises exception for not configured settings
            services.ConfigureMailTemplateService(manager);
        }

        [TestMethod]
        public void AddsRazorLight()
        {
            var viewPath = System.IO.Path.Combine(GetApplicationRoot(), "MailViews");
            if (!System.IO.Directory.Exists(viewPath))
                System.IO.Directory.CreateDirectory(viewPath);

            var services = new ServiceCollection();
            var builder = new ConfigurationBuilder()
                .AddInMemoryCollection(new[]
                {
                    new KeyValuePair<string, string>("MailServiceOptions:SmtpServer","smtp.test.com"),
                    new KeyValuePair<string, string>("MailServiceOptions:FromMail","mail@test.com"),
                    new KeyValuePair<string, string>("MailServiceOptions:FromName","Mail"),
                    new KeyValuePair<string, string>("MailTemplateOptions:BaseDirectory","MailViews"),
                    new KeyValuePair<string, string>("MailTemplateOptions:Extension",".cshtml"),
                });
            var config = builder.Build();
            var manager = new ConsoleReportingConfigurationManager(config);
            var environment = new TestHostingEnvironment { ContentRootPath = GetApplicationRoot() };
            services.AddLogging();
            services.ConfigureMailServiceTemplated(environment, manager);
            var s = services.BuildServiceProvider().GetRequiredService<ITemplatingMailService>();
            Assert.IsNotNull(s);
        }

        private static string GetApplicationRoot()
        {
            var exePath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
            var appPathMatcher = new Regex(@"(?<!fil)[A-Za-z]:\\+[\S\s]*?(?=\\+bin)");
            var appRoot = appPathMatcher.Match(exePath).Value;
            return appRoot;
        }
    }
}
