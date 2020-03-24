using System;
using FluiTec.AppFx.Networking.Mail.Configuration;
using FluiTec.AppFx.Networking.Mail.Tests.Helpers;
using FluiTec.AppFx.Networking.Mail.Tests.Mocking;
using FluiTec.AppFx.Networking.Mail.Tests.Services.MailServices.TestServices;
using FluiTec.AppFx.Options.Exceptions;
using MailKit.Net.Smtp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MimeKit;
using MimeKit.Text;
using nDumbsterCore.smtp;

namespace FluiTec.AppFx.Networking.Mail.Tests.Services.MailServices
{
    [TestClass]
    public class MailKitSmtpMailServiceTest
    {
        #region Fields

        private static int _lastPort = 49999;

        #endregion

        #region Methods

        private static int GetFreePort()
        {
            _lastPort++;
            return _lastPort;
        }

        internal static SmtpMock GetSmtpMock()
        {
            return new SmtpMock(GetFreePort(), GlobalTestSettings.SmtpServerName);
        }

        internal static MailServiceOptions GetTestMailServiceOptions(int port)
        {
            return new MailServiceOptions
            {
                FromMail = GlobalTestSettings.SmtpMail,
                FromName = GlobalTestSettings.SmtpName,
                SmtpServer = GlobalTestSettings.SmtpServer,
                SmtpPort = port
            };
        }

        #endregion
        
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
        public void MailTest()
        {
            var server = SimpleSmtpServer.Start(25);

            using (var client = new SmtpClient())
            {
                client.ServerCertificateValidationCallback = (sender, certificate, chain, errors) => true; // accepts all certs
                client.Connect("127.0.0.1", 25, false);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                client.Send(new MimeMessage
                {
                    From = { new MailboxAddress("Test", "test@example.com") },
                    To = { new MailboxAddress("Test", "test@example.com") },
                    Subject = "Subject",
                    Body = new TextPart(TextFormat.Plain)
                });
            }

            server.Stop();
        }

        //[TestMethod]
        //public void CanSendMail2()
        //{
        //    var port = GetFreePort();
        //    var server = SimpleSmtpServer.Start(port);
        //    var service = new TestMailKitSmtpMailService(GetTestMailServiceOptions(port));
        //    service.SendEmail(GlobalTestSettings.SmtpMail, GlobalTestSettings.MailSubject,
        //        GlobalTestSettings.MailContent, TextFormat.Plain, GlobalTestSettings.SmtpName);
        //    MailAssertHelper.VerifySuccessfulMail(server);
        //    server.Stop();
        //}

        //[TestMethod]
        //public void CanSendMail()
        //{
        //    var smtpMock = GetSmtpMock();
        //    smtpMock.Started = (sender, listener) =>
        //    {
        //        var service = new TestMailKitSmtpMailService(GetTestMailServiceOptions(smtpMock.Port));
        //        service.SendEmail(GlobalTestSettings.SmtpMail, GlobalTestSettings.MailSubject,
        //            GlobalTestSettings.MailContent, TextFormat.Plain, GlobalTestSettings.SmtpName);

        //        smtpMock.Stop();
        //    };
        //    smtpMock.Start();
        //    MailAssertHelper.VerifySuccessfulMail(smtpMock);
        //}

        [TestMethod]
        public void CanSendMailAsync()
        {
            var smtpMock = GetSmtpMock();
            try
            {
                smtpMock.Started = (sender, listener) =>
                {
                    var service = new TestMailKitSmtpMailService(GetTestMailServiceOptions(smtpMock.Port));
                    service.SendEmailAsync(GlobalTestSettings.SmtpMail, GlobalTestSettings.MailSubject, GlobalTestSettings.MailContent, TextFormat.Plain, GlobalTestSettings.SmtpName).Wait();
                    MailAssertHelper.VerifySuccessfulMail(smtpMock);
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
