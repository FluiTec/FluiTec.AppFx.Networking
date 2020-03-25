using System;
using FluiTec.AppFx.Networking.Mail.Configuration;
using FluiTec.AppFx.Networking.Mail.Tests.Helpers;
using FluiTec.AppFx.Networking.Mail.Tests.Services.MailServices.TestServices;
using FluiTec.AppFx.Options.Exceptions;
using MailKit;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MimeKit.Text;
using Moq;

namespace FluiTec.AppFx.Networking.Mail.Tests.Services.MailServices
{
    [TestClass]
    public class MailKitSmtpMailServiceTest
    {
        #region Fields

        private static int _lastPort = 49999;

        #endregion

        #region Methods

        internal static int GetFreePort()
        {
            _lastPort++;
            return _lastPort;
        }

        internal static MailServiceOptions GetTestMailServiceOptions()
        {
            return new MailServiceOptions
            {
                FromMail = GlobalTestSettings.SmtpMail,
                FromName = GlobalTestSettings.SmtpName,
                SmtpServer = GlobalTestSettings.SmtpServer,
                SmtpPort = GlobalTestSettings.SmtpPort
            };
        }

        #endregion
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowsOnMissingOptions()
        {
            var unused = new TestMailKitSmtpMailService(null, new Mock<IMailTransport>().Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void ThrowsOnInvalidOptions()
        {
            var unused = new TestMailKitSmtpMailService(new MailServiceOptions {SmtpServer = "127.0.0.1"}, new Mock<IMailTransport>().Object);
        }

        [TestMethod]
        public void CanSendMail()
        {
            var mailTransportMock = new Mock<IMailTransport>();
            var service = new TestMailKitSmtpMailService(GetTestMailServiceOptions(), mailTransportMock.Object);
            service.SendEmail(GlobalTestSettings.SmtpMail, GlobalTestSettings.MailSubject,
                GlobalTestSettings.MailContent, TextFormat.Plain, GlobalTestSettings.SmtpName);
            mailTransportMock.VerifySendMail(GlobalTestSettings.MailContent);
        }

        [TestMethod]
        public void CanSendMailAsync()
        {
            var mailTransportMock = new Mock<IMailTransport>();
            var service = new TestMailKitSmtpMailService(GetTestMailServiceOptions(), mailTransportMock.Object);
            service.SendEmailAsync(GlobalTestSettings.SmtpMail, GlobalTestSettings.MailSubject,
                GlobalTestSettings.MailContent, TextFormat.Plain, GlobalTestSettings.SmtpName).Wait();
            mailTransportMock.VerifySendMailAsync(GlobalTestSettings.MailContent);
        }
    }
}
