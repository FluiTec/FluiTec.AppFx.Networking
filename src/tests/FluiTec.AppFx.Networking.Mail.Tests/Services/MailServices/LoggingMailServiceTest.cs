using FluiTec.AppFx.Networking.Mail.Configuration;
using FluiTec.AppFx.Networking.Mail.Tests.Helpers;
using FluiTec.AppFx.Networking.Mail.Tests.Services.MailServices.TestServices;
using MailKit;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MimeKit.Text;
using Moq;

namespace FluiTec.AppFx.Networking.Mail.Tests.Services.MailServices
{
    [TestClass]
    public class LoggingMailServiceTest : MailKitSmtpMailServiceTest
    {
        [TestMethod]
        public void DoesNotThrowOnEmptyLogger()
        {
            var unused = new TestLoggingMailService(new MailServiceOptions
            {
                SmtpServer = GlobalTestSettings.SmtpServer,
                SmtpPort = 25,
                FromName = GlobalTestSettings.SmtpName, FromMail = GlobalTestSettings.SmtpMail
            }, null, new Mock<IMailTransport>().Object);
        }

        [TestMethod]
        public void TestLogging()
        {
            var loggerMock = new Mock<ILogger<TestLoggingMailService>>();
            var mailTransportMock = new Mock<IMailTransport>();

            var service = new TestLoggingMailService(GetTestMailServiceOptions(), loggerMock.Object, mailTransportMock.Object);
            service.SendEmailAsync(GlobalTestSettings.SmtpMail, GlobalTestSettings.MailSubject,
                GlobalTestSettings.MailContent, TextFormat.Plain, GlobalTestSettings.SmtpName).Wait();
            loggerMock.VerifyLog(LogLevel.Information, "Successfully sent mail.");
        }
    }
}
