using FluiTec.AppFx.Networking.Mail.Configuration;
using FluiTec.AppFx.Networking.Mail.Tests.Services.MailServices.TestServices;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FluiTec.AppFx.Networking.Mail.Tests.Services.MailServices
{
    [TestClass]
    public class LoggingMailServiceTest : MailKitSmtpMailServiceTest
    {
        [TestMethod]
        public void DoesNotThrowOnEmptyLogger()
        {
            var service = new TestLoggingMailService(new MailServiceOptions
            {
                SmtpServer = SmtpServer,
                SmtpPort = SmtpPort,
                FromName = SmtpName, FromMail = SmtpMail
            }, null);
        }

        [TestMethod]
        public void TestLogging()
        {
            var mock = new Mock<ILogger<TestLoggingMailService>>();
            var logger = mock.Object;

            var service = new TestLoggingMailService(new MailServiceOptions
            {
                SmtpServer = SmtpServer,
                SmtpPort = SmtpPort,
                FromName = SmtpName, FromMail = SmtpMail
            }, logger);
        }
    }
}
