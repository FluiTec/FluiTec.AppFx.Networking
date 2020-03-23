using System;
using System.Net.Security;
using FluiTec.AppFx.Networking.Mail.Configuration;
using FluiTec.AppFx.Networking.Mail.Services;
using FluiTec.AppFx.Networking.Mail.Tests.Mocking;
using FluiTec.AppFx.Options.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MimeKit.Text;

namespace FluiTec.AppFx.Networking.Mail.Tests.Services.MailServices
{
    [TestClass]
    public class MailKitSmtpMailServiceTest
    {
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
            var service = new TestMailKitSmtpMailService(new MailServiceOptions {SmtpServer = "localhost"});
        }

        [TestMethod]
        public void CanSendMail()
        {
            var mail = "test@example.com";
            var name = "Test";
            var subject = "Test";

            var mock = new SmtpMock();
            mock.Start();
            try
            {
                var service = new TestMailKitSmtpMailService(new MailServiceOptions
                {
                    SmtpServer = "localhost",
                    FromName = "Test", FromMail = mail
                });
                service.SendEmail(mail, subject, "Test", TextFormat.Text, name);
                Assert.IsTrue(mock.Session.ClientHistory.Contains($"From: {name} <{mail}>"));
                Assert.IsTrue(mock.Session.ClientHistory.Contains($"To: \"{mail}\" <{name}>"));
                Assert.IsTrue(mock.Session.ClientHistory.Contains($"RCPT TO:<{name}>"));
                Assert.IsTrue(mock.Session.ClientHistory.Contains($"Subject: {subject}"));
                Assert.IsTrue(mock.Session.ClientHistory.Contains($"Subject: {subject}"));
            }
            finally
            {
                mock.Stop();
            }
        }

        [TestMethod]
        public void CanSendMailAsync()
        {
            var mail = "test@example.com";
            var name = "Test";
            var subject = "Test";

            var mock = new SmtpMock();
            mock.Start();
            try
            {
                var service = new TestMailKitSmtpMailService(new MailServiceOptions
                {
                    SmtpServer = "localhost",
                    FromName = "Test", FromMail = mail
                });
                service.SendEmailAsync(mail, subject, "Test", TextFormat.Text, name).Wait();
                Assert.IsTrue(mock.Session.ClientHistory.Contains($"From: {name} <{mail}>"));
                Assert.IsTrue(mock.Session.ClientHistory.Contains($"To: \"{mail}\" <{name}>"));
                Assert.IsTrue(mock.Session.ClientHistory.Contains($"RCPT TO:<{name}>"));
                Assert.IsTrue(mock.Session.ClientHistory.Contains($"Subject: {subject}"));
                Assert.IsTrue(mock.Session.ClientHistory.Contains($"Subject: {subject}"));
            }
            finally
            {
                mock.Stop();
            }
        }

        public class TestMailKitSmtpMailService : MailKitSmtpMailService
        {
            public TestMailKitSmtpMailService(MailServiceOptions options) : base(options)
            {
            }

            public override RemoteCertificateValidationCallback CertificateValidationCallback => (sender, certificate, chain, errors) => true;
        }
    }
}
