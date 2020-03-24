using System;
using FluiTec.AppFx.Networking.Mail.Services;
using FluiTec.AppFx.Networking.Mail.Tests.Services.MailServices;
using FluiTec.AppFx.Networking.Mail.Tests.Services.MailServices.TestServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FluiTec.AppFx.Networking.Mail.Tests.Services
{
    [TestClass]
    public class TemplatingMailServiceTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowsOnMissingMailService()
        {
            var unused = new TemplatingMailService(null, new Mock<ITemplatingService>().Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowsOnMissingTemplatingService()
        {
            var unused = new TemplatingMailService(new Mock<IMailService>().Object, null);
        }
    }
}
