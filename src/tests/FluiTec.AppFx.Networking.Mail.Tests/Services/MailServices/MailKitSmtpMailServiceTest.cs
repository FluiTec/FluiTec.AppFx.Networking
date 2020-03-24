using System;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
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
            var unused = new TestMailKitSmtpMailService(new MailServiceOptions {SmtpServer = "localhost"});
        }

        [TestMethod]
        public void CanOpenPort()
        {
            var listener = new TcpListener(IPAddress.Any, 25);
            listener.Start();
            listener.Stop();
        }

        //[TestMethod]
        public void CanSendMail()
        {
            const string mail = "test@example.com";
            const string name = "Test";
            const string subject = "Test";
            const int smtpPort = 50000;

            var mock = new SmtpMock(smtpPort);
            mock.Start();
            try
            {
                var service = new TestMailKitSmtpMailService(new MailServiceOptions
                {
                    SmtpServer = "localhost",
                    SmtpPort = smtpPort,
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

        //[TestMethod]
        public void CanSendMailAsync()
        {
            const string mail = "test@example.com";
            const string name = "Test";
            const string subject = "Test";
            const int smtpPort = 50000;

            var mock = new SmtpMock(smtpPort);
            mock.Start();
            try
            {
                var service = new TestMailKitSmtpMailService(new MailServiceOptions
                {
                    SmtpPort = smtpPort,
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
