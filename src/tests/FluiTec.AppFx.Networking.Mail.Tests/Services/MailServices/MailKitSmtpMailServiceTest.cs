using System;
using System.Runtime.CompilerServices;
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
        #region Constants

        protected const string SmtpMail = "test@example.com";
        protected const string SmtpName = "Test";
        protected const string SmtpServer = "127.0.0.1";
        protected const string SmtpServerName = "example.com"; 
        protected const string MailSubject = "TestSubject";
        protected const string MailContent = "TestContent";

        #endregion

        #region Fields

        private int _lastPort = 49999;

        #endregion

        #region Methods

        private int GetFreePort()
        {
            _lastPort++;
            return _lastPort;
        }

        protected SmtpMock GetSmtpMock()
        {
            return new SmtpMock(GetFreePort(), SmtpServerName);
        }

        protected MailServiceOptions GetTestMailServiceOptions(int port)
        {
            return new MailServiceOptions
            {
                FromMail = SmtpMail,
                FromName = SmtpName,
                SmtpServer = SmtpServer,
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
            var smtpMock = GetSmtpMock();
            try
            {
                smtpMock.Start();
                smtpMock.Started = (sender, listener) =>
                {
                    var service = new TestMailKitSmtpMailService(GetTestMailServiceOptions(smtpMock.Port));
                    service.SendEmail(SmtpMail, MailSubject, MailContent, TextFormat.Plain, SmtpName);
                };
            }
            finally
            {
                smtpMock.Stop();
            }
        }
    }
}
