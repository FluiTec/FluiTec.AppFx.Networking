using System.Collections.Generic;
using FluiTec.AppFx.Networking.Mail.Configuration;
using FluiTec.AppFx.Options.Managers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluiTec.AppFx.Networking.Mail.Tests
{
    [TestClass]
    public class MailServiceExtensionTest
    {
        [TestMethod]
        public void AddsOptions()
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
        public void AddsValidators()
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
    }
}
