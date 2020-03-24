using System.Linq;
using FluiTec.AppFx.Networking.Mail.Tests.Mocking;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using nDumbsterCore.smtp;

namespace FluiTec.AppFx.Networking.Mail.Tests.Helpers
{
    public class MailAssertHelper
    {
        public static void VerifySuccessfulMail(SmtpMock smtpMock)
        {
            Assert.IsTrue(smtpMock.Session.History.Contains($"From: {GlobalTestSettings.SmtpName} <{GlobalTestSettings.SmtpMail}>"));
            Assert.IsTrue(smtpMock.Session.History.Contains($"To: \"{GlobalTestSettings.SmtpMail}\" <{GlobalTestSettings.SmtpName}>"));
            Assert.IsTrue(smtpMock.Session.History.Contains($"RCPT TO:<{GlobalTestSettings.SmtpName}>"));
            Assert.IsTrue(smtpMock.Session.History.Contains($"Subject: {GlobalTestSettings.MailSubject}"));
            Assert.IsTrue(smtpMock.Session.History.LastIndexOf("250 OK") >
                          smtpMock.Session.History.FindLastIndex(s => s.Contains($"Subject: {GlobalTestSettings.MailSubject}")));
        }

        public static void VerifySuccessfulMail(SimpleSmtpServer server)
        {
            var email = server.ReceivedEmail.Single();
            Assert.AreEqual(GlobalTestSettings.SmtpMail, email.To.Single().Address);
            Assert.AreEqual(GlobalTestSettings.SmtpName, email.To.Single().DisplayName);

            Assert.AreEqual(GlobalTestSettings.SmtpMail, email.From.Address);
            Assert.AreEqual(GlobalTestSettings.SmtpName, email.From.DisplayName);

            Assert.AreEqual(GlobalTestSettings.MailSubject, email.Subject);
            Assert.AreEqual(GlobalTestSettings.MailContent, email.Body);
        }
    }
}
