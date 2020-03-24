using System.Linq;
using MailKit.Net.Smtp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MimeKit;
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
                    Subject = GlobalTestSettings.MailSubject,
                    Body = new TextPart(TextFormat.Plain) { Text = GlobalTestSettings.MailContent }
                });
            }

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
