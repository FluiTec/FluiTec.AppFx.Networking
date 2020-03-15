using System.Collections.Generic;
using FluiTec.AppFx.Networking.Mail.Configuration;
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
    }
}
