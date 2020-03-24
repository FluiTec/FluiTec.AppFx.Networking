using System;
using FluiTec.AppFx.Networking.Mail.Configuration;
using FluiTec.AppFx.Networking.Mail.Tests.Helpers;
using FluiTec.AppFx.Networking.Mail.Tests.Mocking;
using FluiTec.AppFx.Networking.Mail.Tests.Services.MailServices.TestServices;
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
            int port = GetSmtpPort();
            
            var service = new TestLoggingMailService(new MailServiceOptions
            {
                SmtpServer = SmtpServer,
                SmtpPort = port,
                FromName = SmtpName, FromMail = SmtpMail
            }, null);
        }

        [TestMethod]
        public void TestLogging()
        {
            int port = GetSmtpPort();

            var logger = new Mock<ILogger<TestLoggingMailService>>();

            var smtpMock = new SmtpMock(port);
            smtpMock.Start();

            try
            {
                var service = new TestLoggingMailService(new MailServiceOptions
                {
                    SmtpServer = SmtpServer,
                    SmtpPort = port,
                    FromName = SmtpName, FromMail = SmtpMail
                }, logger.Object);
                service.SendEmail(SmtpMail, SmtpSubject, "Test", TextFormat.Text, SmtpName);

                logger.VerifyLog(LogLevel.Information, "Successfully sent mail.");
            }
            finally
            {
                smtpMock.Stop();
            }
        }
    }
}
