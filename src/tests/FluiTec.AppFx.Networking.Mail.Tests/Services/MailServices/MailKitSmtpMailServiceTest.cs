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
        protected const string SmtpSubject = "Test";
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
            var unused = new TestMailKitSmtpMailService(new MailServiceOptions {SmtpServer = "localhost"});
        }

        [TestMethod]
        public void CanSendMail()
        {
            const int port = 50000;

            var mock = new SmtpMock(port);
            mock.Start();
            try
            {
                var service = new TestMailKitSmtpMailService(new MailServiceOptions
                {
                    SmtpServer = SmtpServer,
                    SmtpPort = port,
                    FromName = SmtpName, FromMail = SmtpMail
                });
                service.SendEmail(SmtpMail, SmtpSubject, "Test", TextFormat.Text, SmtpName);
                Assert.IsTrue(mock.Session.History.Contains($"From: {SmtpName} <{SmtpMail}>"));
                Assert.IsTrue(mock.Session.History.Contains($"To: \"{SmtpMail}\" <{SmtpName}>"));
                Assert.IsTrue(mock.Session.History.Contains($"RCPT TO:<{SmtpName}>"));
                Assert.IsTrue(mock.Session.History.Contains($"Subject: {SmtpSubject}"));
                Assert.IsTrue(mock.Session.History.LastIndexOf("250 OK") > 
                              mock.Session.History.FindLastIndex(s => s.Contains($"Subject: {SmtpSubject}")));
            }
            finally
            {
                mock.Stop();
            }
        }

        [TestMethod]
        public void CanSendMailAsync()
        {
            const int port = 50001;

            var mock = new SmtpMock(port);
            mock.Start();
            try
            {
                var service = new TestMailKitSmtpMailService(new MailServiceOptions
                {
                    SmtpPort = port,
                    SmtpServer = SmtpServer,
                    FromName = SmtpName, FromMail = SmtpMail
                });
                service.SendEmailAsync(SmtpMail, SmtpSubject, "Test", TextFormat.Text, SmtpName).Wait();
                Assert.IsTrue(mock.Session.History.Contains($"From: {SmtpName} <{SmtpMail}>"));
                Assert.IsTrue(mock.Session.History.Contains($"To: \"{SmtpMail}\" <{SmtpName}>"));
                Assert.IsTrue(mock.Session.History.Contains($"RCPT TO:<{SmtpName}>"));
                Assert.IsTrue(mock.Session.History.Contains($"Subject: {SmtpSubject}"));
                Assert.IsTrue(mock.Session.History.LastIndexOf("250 OK") > 
                              mock.Session.History.FindLastIndex(s => s.Contains($"Subject: {SmtpSubject}")));
            }
            finally
            {
                mock.Stop();
            }
        }
    }
}
