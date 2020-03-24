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
        public void CanSendMail2()
        {
            var server = SimpleSmtpServer.Start(SimpleSmtpServer.DEFAULT_SMTP_PORT);
            var service = new TestMailKitSmtpMailService(GetTestMailServiceOptions(SimpleSmtpServer.DEFAULT_SMTP_PORT));
            service.SendEmail(GlobalTestSettings.SmtpMail, GlobalTestSettings.MailSubject,
                GlobalTestSettings.MailContent, TextFormat.Plain, GlobalTestSettings.SmtpName);
            MailAssertHelper.VerifySuccessfulMail(server);
            server.Stop();
        }

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
