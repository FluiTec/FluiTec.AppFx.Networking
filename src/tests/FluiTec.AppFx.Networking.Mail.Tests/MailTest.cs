using System.Linq;
using FluiTec.AppFx.Networking.Mail.Configuration;
using FluiTec.AppFx.Networking.Mail.Tests.Helpers;
using FluiTec.AppFx.Networking.Mail.Tests.Services.MailServices.TestServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MimeKit.Text;
using nDumbsterCore.smtp;

namespace FluiTec.AppFx.Networking.Mail.Tests
{
    [TestClass]
    public class MailTest
    {
        [TestMethod]
        public void DoMailTest()
        {
            var port = 25;
            var server = SimpleSmtpServer.Start(port);
            var service = new TestMailKitSmtpMailService(new MailServiceOptions
            {
                FromMail = GlobalTestSettings.SmtpMail,
                FromName = GlobalTestSettings.SmtpName,
                SmtpPort = port,
                SmtpServer = "127.0.0.1"
            });
            service.SendEmail(GlobalTestSettings.SmtpMail, GlobalTestSettings.MailSubject,
                GlobalTestSettings.MailContent, TextFormat.Plain, GlobalTestSettings.SmtpName);

            var email = server.ReceivedEmail.Single();
            MailAssertHelper.VerifySuccessfulMail(server);
            //Assert.AreEqual(GlobalTestSettings.SmtpMail, email.To.Single().Address);
            //Assert.AreEqual(GlobalTestSettings.SmtpName, email.To.Single().DisplayName);

            //Assert.AreEqual(GlobalTestSettings.SmtpMail, email.From.Address);
            //Assert.AreEqual(GlobalTestSettings.SmtpName, email.From.DisplayName);

            //Assert.AreEqual(GlobalTestSettings.MailSubject, email.Subject);
            //Assert.AreEqual(GlobalTestSettings.MailContent, email.Body);

            server.Stop();
        }

        [TestMethod]
        public void DoMailTest2()
        {
            var port = 26;
            var server = SimpleSmtpServer.Start(port);
            var service = new TestMailKitSmtpMailService(new MailServiceOptions
            {
                FromMail = GlobalTestSettings.SmtpMail,
                FromName = GlobalTestSettings.SmtpName,
                SmtpPort = port,
                SmtpServer = "127.0.0.1"
            });
            service.SendEmail(GlobalTestSettings.SmtpMail, GlobalTestSettings.MailSubject,
                GlobalTestSettings.MailContent, TextFormat.Plain, GlobalTestSettings.SmtpName);

            var email = server.ReceivedEmail.Single();
            Assert.AreEqual(GlobalTestSettings.SmtpMail, email.To.Single().Address);
            Assert.AreEqual(GlobalTestSettings.SmtpName, email.To.Single().DisplayName);

            Assert.AreEqual(GlobalTestSettings.SmtpMail, email.From.Address);
            Assert.AreEqual(GlobalTestSettings.SmtpName, email.From.DisplayName);

            Assert.AreEqual(GlobalTestSettings.MailSubject, email.Subject);
            Assert.AreEqual(GlobalTestSettings.MailContent, email.Body);

            server.Stop();
        }
    }
}
