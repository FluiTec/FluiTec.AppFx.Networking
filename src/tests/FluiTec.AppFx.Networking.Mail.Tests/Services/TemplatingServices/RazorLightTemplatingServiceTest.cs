using System;
using FluiTec.AppFx.Networking.Mail.Configuration;
using FluiTec.AppFx.Networking.Mail.Services;
using FluiTec.AppFx.Networking.Mail.Tests.Services.MailServices.TestServices;
using FluiTec.AppFx.Options.Exceptions;
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
            var unused = new RazorLightTemplatingService(new Mock<IRazorLightEngine>().Object, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowOnMissingEngine()
        {
            var unused = new RazorLightTemplatingService(null, new MailTemplateOptions());
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void ThrowsOnInvalidOptions()
        {
            var unused = new RazorLightTemplatingService(null, new MailTemplateOptions {BaseDirectory = string.Empty});
        }
    }
}
