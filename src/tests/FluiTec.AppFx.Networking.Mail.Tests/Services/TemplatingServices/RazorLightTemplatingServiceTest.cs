using System;
using FluiTec.AppFx.Networking.Mail.Configuration;
using FluiTec.AppFx.Networking.Mail.Services;
using FluiTec.AppFx.Networking.Mail.Tests.Helpers;
using FluiTec.AppFx.Networking.Mail.Tests.Services.TemplatingServices.Models;
using FluiTec.AppFx.Options.Exceptions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RazorLight;

namespace FluiTec.AppFx.Networking.Mail.Tests.Services.TemplatingServices
{
    [TestClass]
    public class RazorLightTemplatingServiceTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowsOnMissingOptions()
        {
            var unused = new RazorLightTemplatingService(new Mock<IRazorLightEngine>().Object, null, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowOnMissingEngine()
        {
            var unused = new RazorLightTemplatingService(null, new MailTemplateOptions(), null);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void ThrowsOnInvalidOptions()
        {
            var unused = new RazorLightTemplatingService(null, new MailTemplateOptions {BaseDirectory = string.Empty}, null);
        }

        [TestMethod]
        public void DoesNotThrowOnEmptyLogger()
        {
            var unused = new RazorLightTemplatingService(new Mock<IRazorLightEngine>().Object, new MailTemplateOptions(), null);
        }

        [TestMethod]
        public void LogsViewName()
        {
            var loggerMock = new Mock<ILogger<RazorLightTemplatingService>>();
            var service = new RazorLightTemplatingService(new Mock<IRazorLightEngine>().Object, new MailTemplateOptions(), loggerMock.Object);
            var parsedTemplate = service.Parse(new Test());
            loggerMock.VerifyLog(LogLevel.Debug, $"ViewName of '{typeof(Test).Name}' is '{nameof(Test)}.cshtml'.");
        }

        [TestMethod]
        public void LogsParsingByClass()
        {
            var loggerMock = new Mock<ILogger<RazorLightTemplatingService>>();
            var service = new RazorLightTemplatingService(new Mock<IRazorLightEngine>().Object, new MailTemplateOptions(), loggerMock.Object);
            var parsedTemplate = service.Parse(new Test());
            loggerMock.VerifyLog(LogLevel.Debug, $"Parsing model '{typeof(Test).Name}'.");
        }

        [TestMethod]
        public void LogsParsingByView()
        {
            var loggerMock = new Mock<ILogger<RazorLightTemplatingService>>();
            var service = new RazorLightTemplatingService(new Mock<IRazorLightEngine>().Object, new MailTemplateOptions(), loggerMock.Object);
            var parsedTemplate = service.Parse(new Test());
            loggerMock.VerifyLog(LogLevel.Information, $"Parsing view '{nameof(Test)}.cshtml' for model '{typeof(Test).Name}'.");
        }
    }
}
