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
                client.Timeout = 10;
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
    }
}