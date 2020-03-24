using System;
using FluiTec.AppFx.Networking.Mail.Tests.Services.MailServices.TestServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluiTec.AppFx.Networking.Mail.Tests.Services.MailServices
{
    [TestClass]
    public class CertificateValidatingMailServiceTest : LoggingMailServiceTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowsOnMissingCertificateOptions()
        {
            var unused = new TestCertificateValidatingMailService(GetTestMailServiceOptions(25), null, null);
        }
    }
}
