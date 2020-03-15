using System.Linq;
using FluiTec.AppFx.Networking.Mail.Configuration;
using FluiTec.AppFx.Networking.Mail.Configuration.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluiTec.AppFx.Networking.Mail.Tests
{
    [TestClass]
    public class MailTemplateOptionsValidatorTest
    {
        private const string NotEmptyValidator = "NotEmptyValidator";

        private const string PredicateValidator = "PredicateValidator";

        [TestMethod]
        public void ValidatesEmptyBaseDirectory()
        {
            var options = new MailTemplateOptions {BaseDirectory = string.Empty};
            var validator = new MailTemplateOptionsValidator();
            var result = validator.Validate(options);
            var validated = result.Errors.Any(e => e.PropertyName == nameof(MailTemplateOptions.BaseDirectory) && e.ResourceName == NotEmptyValidator);

            Assert.IsTrue(validated);
        }

        [TestMethod]
        public void ValidatesEmptyExtension()
        {
            var options = new MailTemplateOptions { Extension = string.Empty };
            var validator = new MailTemplateOptionsValidator();
            var result = validator.Validate(options);
            var validated = result.Errors.Any(e => e.PropertyName == nameof(MailTemplateOptions.Extension) && e.ResourceName == NotEmptyValidator);

            Assert.IsTrue(validated);
        }

        [TestMethod]
        public void ValidatesExtensionStartsWithDot()
        {
            var options = new MailTemplateOptions { Extension = "cshtml" };
            var validator = new MailTemplateOptionsValidator();
            var result = validator.Validate(options);
            var validated = result.Errors.Any(e => e.PropertyName == nameof(MailTemplateOptions.Extension) && e.ErrorCode == PredicateValidator);

            Assert.IsTrue(validated);
        }
    }
}