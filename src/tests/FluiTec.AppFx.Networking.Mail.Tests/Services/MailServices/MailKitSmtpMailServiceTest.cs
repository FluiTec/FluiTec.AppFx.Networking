using System;
using FluiTec.AppFx.Networking.Mail.Configuration;
using FluiTec.AppFx.Networking.Mail.Tests.Mocking;
using FluiTec.AppFx.Networking.Mail.Tests.Services.MailServices.TestServices;
using FluiTec.AppFx.Options.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MimeKit.Text;

namespace FluiTec.AppFx.Networking.Mail.Tests.Services.MailServices
{
    [TestClass]
    public class MailKitSmtpMailServiceTest
    {
        protected const string SmtpMail = "test@example.com";
        protected const string SmtpName = "Test";
        protected const string SmtpServer = "127.0.0.1";

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowsOnMissingOptions()
        {
            var unused = new TestMailKitSmtpMailService(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void ThrowsOnInvalidOptions()
        {
            var unused = new TestMailKitSmtpMailService(new MailServiceOptions {SmtpServer = "127.0.0.1"});
        }

        [TestMethod]
        public void CanSendMail()
        {
            var smtpMock = new SmtpMock(25, "example.com");
            try
            {
                smtpMock.Start();
                smtpMock.Started = (sender, listener) =>
                {
                    var service = new TestMailKitSmtpMailService(new MailServiceOptions
                    {
                        FromMail = "test@example.com",
                        FromName = "Test",
                        SmtpServer = SmtpServer,
                        SmtpPort = 25
                    });
                    service.SendEmail("test@example.com", "TestSubject", "TestContent", TextFormat.Plain, "Test");
                };
            }
            finally
            {
                smtpMock.Stop();
            }
        }
    }
}
