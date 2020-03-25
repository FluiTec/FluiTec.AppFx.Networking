using System;
using FluiTec.AppFx.Networking.Mail.Configuration;
using FluiTec.AppFx.Networking.Mail.Tests.Helpers;
using FluiTec.AppFx.Networking.Mail.Tests.Mocking;
using FluiTec.AppFx.Networking.Mail.Tests.Services.MailServices.TestServices;
using FluiTec.AppFx.Options.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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

        protected static int GetFreePort()
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
        public void CanSendMail()
        {
            var port = GetFreePort();
            var server = SimpleSmtpServer.Start(port);
            var service = new TestMailKitSmtpMailService(GetTestMailServiceOptions(port));
            service.SendEmail(GlobalTestSettings.SmtpMail, GlobalTestSettings.MailSubject,
                GlobalTestSettings.MailContent, TextFormat.Plain, GlobalTestSettings.SmtpName);
            MailAssertHelper.VerifySuccessfulMail(server);
            server.Stop();
        }

        [TestMethod]
        public void CanSendMailAsync()
        {
            var port = GetFreePort();
            var server = SimpleSmtpServer.Start(port);
            var service = new TestMailKitSmtpMailService(GetTestMailServiceOptions(port));
            service.SendEmailAsync(GlobalTestSettings.SmtpMail, GlobalTestSettings.MailSubject,
                GlobalTestSettings.MailContent, TextFormat.Plain, GlobalTestSettings.SmtpName).Wait();
            MailAssertHelper.VerifySuccessfulMail(server);
            server.Stop();
        }
    }
}
