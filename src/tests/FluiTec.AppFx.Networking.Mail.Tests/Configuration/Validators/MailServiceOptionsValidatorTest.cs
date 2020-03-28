using System.Linq;
using FluiTec.AppFx.Networking.Mail.Configuration;
using FluiTec.AppFx.Networking.Mail.Configuration.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluiTec.AppFx.Networking.Mail.Tests.Configuration.Validators
{
    [TestClass]
    public class MailServiceOptionsValidatorTest
    {
        private const string NotEmptyValidator = "NotEmptyValidator";

        private const string GreaterThanValidator = "GreaterThanValidator";

        [TestMethod]
        public void ValidatesEmptySmtpServer()
        {
            var options = new MailServiceOptions();
            var validator = new MailServiceOptionsValidator();
            var result = validator.Validate(options);
            var validated = result.Errors.Any(e =>
                e.PropertyName == nameof(MailServiceOptions.SmtpServer) && e.ResourceName == NotEmptyValidator);

            Assert.IsTrue(validated);
        }

        [TestMethod]
        public void ValidatesSmtpPort()
        {
            var options = new MailServiceOptions {SmtpPort = 0};
            var validator = new MailServiceOptionsValidator();
            var result = validator.Validate(options);
            var validated = result.Errors.Any(e =>
                e.PropertyName == nameof(MailServiceOptions.SmtpPort) && e.ResourceName == GreaterThanValidator);

            Assert.IsTrue(validated);
        }

        [TestMethod]
        public void ValidatesEmptyFromMail()
        {
            var options = new MailServiceOptions();
            var validator = new MailServiceOptionsValidator();
            var result = validator.Validate(options);
            var validated = result.Errors.Any(e =>
                e.PropertyName == nameof(MailServiceOptions.FromMail) && e.ResourceName == NotEmptyValidator);

            Assert.IsTrue(validated);
        }

        [TestMethod]
        public void ValidatesEmptyFromName()
        {
            var options = new MailServiceOptions();
            var validator = new MailServiceOptionsValidator();
            var result = validator.Validate(options);
            var validated = result.Errors.Any(e =>
                e.PropertyName == nameof(MailServiceOptions.FromName) && e.ResourceName == NotEmptyValidator);

            Assert.IsTrue(validated);
        }

        [TestMethod]
        public void ValidatesEmptyUsernameOnAuthenticate()
        {
            var options = new MailServiceOptions {Authenticate = true};
            var validator = new MailServiceOptionsValidator();
            var result = validator.Validate(options);
            var validated = result.Errors.Any(e =>
                e.PropertyName == nameof(MailServiceOptions.Username) && e.ResourceName == NotEmptyValidator);

            Assert.IsTrue(validated);
        }

        [TestMethod]
        public void ValidatesEmptyPasswordOnAuthenticate()
        {
            var options = new MailServiceOptions {Authenticate = true};
            var validator = new MailServiceOptionsValidator();
            var result = validator.Validate(options);
            var validated = result.Errors.Any(e =>
                e.PropertyName == nameof(MailServiceOptions.Password) && e.ResourceName == NotEmptyValidator);

            Assert.IsTrue(validated);
        }
    }
}