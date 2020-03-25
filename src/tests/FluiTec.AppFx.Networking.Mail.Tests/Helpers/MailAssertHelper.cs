using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using nDumbsterCore.smtp;

namespace FluiTec.AppFx.Networking.Mail.Tests.Helpers
{
    public class MailAssertHelper
    {
        public static void VerifySuccessfulMail(SimpleSmtpServer server, string body = null)
        {
            var email = server.ReceivedEmail.Single();
            Assert.AreEqual(GlobalTestSettings.SmtpMail, email.To.Single().Address);
            Assert.AreEqual(GlobalTestSettings.SmtpName, email.To.Single().DisplayName);

            Assert.AreEqual(GlobalTestSettings.SmtpMail, email.From.Address);
            Assert.AreEqual(GlobalTestSettings.SmtpName, email.From.DisplayName);

            Assert.AreEqual(GlobalTestSettings.MailSubject, email.Subject);
            Assert.AreEqual(body ?? GlobalTestSettings.MailContent, email.Body);
        }
    }
}
