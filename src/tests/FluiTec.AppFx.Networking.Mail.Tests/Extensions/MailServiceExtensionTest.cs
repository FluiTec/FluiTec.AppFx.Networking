﻿using System.Collections.Generic;
using FluiTec.AppFx.Networking.Mail.Configuration;
using FluiTec.AppFx.Networking.Mail.Services;
using FluiTec.AppFx.Networking.Mail.Tests.Helpers;
using FluiTec.AppFx.Options.Exceptions;
using FluiTec.AppFx.Options.Managers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RazorLight.Razor;

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
                    new KeyValuePair<string, string>("MailServiceOptions:SmtpServer", "smtp.test.com"),
                    new KeyValuePair<string, string>("MailServiceOptions:FromMail", "mail@test.com"),
                    new KeyValuePair<string, string>("MailServiceOptions:FromName", "Mail"),
                    new KeyValuePair<string, string>("MailServerCertificateValidationOptions:Validate", "false")
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
                    new KeyValuePair<string, string>("MailTemplateOptions:BaseDirectory", "MailViews"),
                    new KeyValuePair<string, string>("MailTemplateOptions:Extension", ".cshtml")
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
                    new KeyValuePair<string, string>("MailServiceOptions:SmtpServer", "smtp.test.com"),
                    new KeyValuePair<string, string>("MailServiceOptions:FromMail", "mail@test.com"),
                    new KeyValuePair<string, string>("MailServiceOptions:FromName", "Mail"),
                    new KeyValuePair<string, string>("MailServerCertificateValidationOptions:Validate", "false")
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
                    new KeyValuePair<string, string>("MailTemplateOptions:BaseDirectory", "MailViews"),
                    new KeyValuePair<string, string>("MailTemplateOptions:Extension", ".cshtml")
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
                    new KeyValuePair<string, string>("MailServiceOptions:SmtpServer", "smtp.test.com")
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
                    new KeyValuePair<string, string>("MailTemplateOptions:Extension", "cshtml")
                });
            var config = builder.Build();
            var manager = new ConsoleReportingConfigurationManager(config);

            // raises exception for not configured settings
            services.ConfigureMailTemplateService(manager);
        }

        [TestMethod]
        public void AddsRazorLight()
        {
            var services = new ServiceCollection();
            var builder = new ConfigurationBuilder()
                .AddInMemoryCollection(new[]
                {
                    new KeyValuePair<string, string>("MailServiceOptions:SmtpServer", "smtp.test.com"),
                    new KeyValuePair<string, string>("MailServiceOptions:FromMail", "mail@test.com"),
                    new KeyValuePair<string, string>("MailServiceOptions:FromName", "Mail"),
                    new KeyValuePair<string, string>("MailServerCertificateValidationOptions:Validate", "false"),
                    new KeyValuePair<string, string>("MailTemplateOptions:BaseDirectory", "MailViews"),
                    new KeyValuePair<string, string>("MailTemplateOptions:Extension", ".cshtml")
                });
            var config = builder.Build();
            var manager = new ConsoleReportingConfigurationManager(config);
            var environment = new TestHostingEnvironment {ContentRootPath = ApplicationHelper.GetApplicationCodebase()};
            services.AddLogging();
            services.ConfigureMailServiceTemplated(environment, manager);
            var s = services.BuildServiceProvider().GetRequiredService<ITemplatingMailService>();
            Assert.IsNotNull(s);
        }

        [TestMethod]
        public void RespectsRazorLightFileConfig()
        {
            var services = new ServiceCollection();
            var builder = new ConfigurationBuilder()
                .AddInMemoryCollection(new[]
                {
                    new KeyValuePair<string, string>("MailServiceOptions:SmtpServer", "smtp.test.com"),
                    new KeyValuePair<string, string>("MailServiceOptions:FromMail", "mail@test.com"),
                    new KeyValuePair<string, string>("MailServiceOptions:FromName", "Mail"),
                    new KeyValuePair<string, string>("MailServerCertificateValidationOptions:Validate", "false"),
                    new KeyValuePair<string, string>("MailTemplateOptions:TemplateSource", "File"),
                    new KeyValuePair<string, string>("MailTemplateOptions:DefaultNamespace", "MailViewsEmbedded"),
                    new KeyValuePair<string, string>("MailTemplateOptions:BaseDirectory", "MailViews"),
                    new KeyValuePair<string, string>("MailTemplateOptions:Extension", ".cshtml")
                });
            var config = builder.Build();
            var manager = new ConsoleReportingConfigurationManager(config);
            var environment = new TestHostingEnvironment {ContentRootPath = ApplicationHelper.GetApplicationCodebase()};
            services.AddLogging();
            services.ConfigureMailServiceTemplated(environment, manager);

            var s = services.BuildServiceProvider().GetRequiredService<RazorLightProject>();

            Assert.IsTrue(s is FileSystemRazorProject);
            Assert.IsTrue(s.GetItemAsync("Test").Result.Exists);
        }

        [TestMethod]
        public void RespectsRazorLightEmbeddedConfig()
        {
            var services = new ServiceCollection();
            var builder = new ConfigurationBuilder()
                .AddInMemoryCollection(new[]
                {
                    new KeyValuePair<string, string>("MailServiceOptions:SmtpServer", "smtp.test.com"),
                    new KeyValuePair<string, string>("MailServiceOptions:FromMail", "mail@test.com"),
                    new KeyValuePair<string, string>("MailServiceOptions:FromName", "Mail"),
                    new KeyValuePair<string, string>("MailServerCertificateValidationOptions:Validate", "false"),
                    new KeyValuePair<string, string>("MailTemplateOptions:TemplateSource", "Embedded"),
                    new KeyValuePair<string, string>("MailTemplateOptions:DefaultNamespace", "MailViewsEmbedded"),
                    new KeyValuePair<string, string>("MailTemplateOptions:BaseDirectory", "MailViews"),
                    new KeyValuePair<string, string>("MailTemplateOptions:Extension", ".cshtml")
                });
            var config = builder.Build();
            var manager = new ConsoleReportingConfigurationManager(config);
            var environment = new TestHostingEnvironment {ContentRootPath = ApplicationHelper.GetApplicationCodebase()};
            services.AddLogging();
            services.ConfigureMailServiceTemplated(environment, manager, typeof(GlobalTestSettings));

            var s = services.BuildServiceProvider().GetRequiredService<RazorLightProject>();

            Assert.IsTrue(s is EmbeddedRazorProject);
            Assert.IsTrue(s.GetItemAsync("Test").Result.Exists);
        }
    }
}