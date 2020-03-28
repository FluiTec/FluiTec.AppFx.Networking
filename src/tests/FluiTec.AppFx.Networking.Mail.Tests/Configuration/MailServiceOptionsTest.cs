using FluiTec.AppFx.Networking.Mail.Configuration;
using MailKit.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluiTec.AppFx.Networking.Mail.Tests.Configuration
{
    [TestClass]
    public class MailServiceOptionsTest
    {
        [TestMethod]
        public void DefaultsToNoAuthentication()
        {
            var options = new MailServiceOptions();
            Assert.IsFalse(options.Authenticate);
        }

        [TestMethod]
        public void DefaultsToNoSsl()
        {
            var options = new MailServiceOptions();
            Assert.IsFalse(options.EnableSsl);
        }

        [TestMethod]
        public void DefaultsToPort25()
        {
            var options = new MailServiceOptions();
            Assert.AreEqual(25, options.SmtpPort);
        }

        [TestMethod]
        public void DefaultsToSecureSocketNone()
        {
            var options = new MailServiceOptions();
            Assert.AreEqual(SecureSocketOptions.None, options.SocketOptions);
        }

        [TestMethod]
        public void DefaultsTo10SecondTimeout()
        {
            var options = new MailServiceOptions();
            Assert.AreEqual(10, options.TimeOut);
        }
    }
}