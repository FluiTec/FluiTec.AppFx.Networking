using System.Net.Security;
using FluiTec.AppFx.Networking.Mail.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluiTec.AppFx.Networking.Mail.Tests.Configuration
{
    [TestClass]
    public class MailServerCertificateValidationOptionsTest
    {
        [TestMethod]
        public void DefaultsToEnabledValidation()
        {
            var options = new MailServerCertificateValidationOptions();
            Assert.IsTrue(options.Validate);
        }

        [TestMethod]
        public void DefaultsToNoSslPolicyErrors()
        {
            const SslPolicyErrors acceptableErrors = SslPolicyErrors.None;
            var options = new MailServerCertificateValidationOptions();
            Assert.AreEqual(acceptableErrors, options.AcceptablePolicyErrors);
        }

        [TestMethod]
        public void DefaultsToNoCertificateValidation()
        {
            var options = new MailServerCertificateValidationOptions();
            Assert.IsFalse(options.CertificateValidation.Validate);
        }
    }
}