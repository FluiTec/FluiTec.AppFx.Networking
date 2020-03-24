using System;
using FluiTec.AppFx.Networking.Mail.Configuration;
using FluiTec.AppFx.Networking.Mail.Tests.Helpers;
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
            var unused = new TestLoggingMailService(new MailServiceOptions
            {
                SmtpServer = SmtpServer,
                SmtpPort = 25,
                FromName = SmtpName, FromMail = SmtpMail
            }, null);
        }

        [TestMethod]
        public void TestLogging()
        {
            var loggerMock = new Mock<ILogger<TestLoggingMailService>>();
            var smtpMock = GetSmtpMock();
            try
            {
                smtpMock.Started = (sender, listener) =>
                {
                    var service = new TestLoggingMailService(GetTestMailServiceOptions(smtpMock.Port), loggerMock.Object);
                    service.SendEmail(SmtpMail, MailSubject, MailContent, TextFormat.Text, SmtpName);
                    loggerMock.VerifyLog(LogLevel.Information, "Successfully sent mail.");
                };
                smtpMock.Start();
            }
            finally
            {
                smtpMock.Stop();
            }
        }
    }
}
