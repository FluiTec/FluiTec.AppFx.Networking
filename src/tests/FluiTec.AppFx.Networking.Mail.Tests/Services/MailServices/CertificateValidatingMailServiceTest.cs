using System;
using FluiTec.AppFx.Networking.Mail.Factories;
using FluiTec.AppFx.Networking.Mail.Services;
using FluiTec.AppFx.Networking.Mail.Tests.Services.MailServices.TestServices;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FluiTec.AppFx.Networking.Mail.Tests.Services.MailServices
{
    [TestClass]
    public class CertificateValidatingMailServiceTest : LoggingMailServiceTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowsOnMissingCertificateOptions()
        {
            var unused = new TestCertificateValidatingMailService(GetTestMailServiceOptions(), new Mock<ILogger<CertificateValidatingMailService>>().Object, null, new Mock<IMailTransportFactory>().Object);
        }
    }
}
