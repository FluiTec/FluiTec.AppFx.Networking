using System.Linq;
using FluiTec.AppFx.Networking.Mail.Configuration;
using FluiTec.AppFx.Networking.Mail.Configuration.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluiTec.AppFx.Networking.Mail.Tests.Configuration.Validators
{
    [TestClass]
    public class MailTemplateOptionsValidatorTest
    {
        private const string NotEmptyValidator = "NotEmptyValidator";

        private const string PredicateValidator = "PredicateValidator";

        [TestMethod]
        public void ValidatesEmptyBaseDirectory()
        {
            var options = new MailTemplateOptions
                {BaseDirectory = string.Empty, TemplateSource = MailTemplateOptions.MailTemplateSource.File};
            var validator = new MailTemplateOptionsValidator();
            var result = validator.Validate(options);
            var validated = result.Errors.Any(e =>
                e.PropertyName == nameof(MailTemplateOptions.BaseDirectory) && e.ResourceName == NotEmptyValidator);

            Assert.IsTrue(validated);
        }

        [TestMethod]
        public void NotValidatesEmptyBaseDirectory()
        {
            var options = new MailTemplateOptions
                {BaseDirectory = string.Empty, TemplateSource = MailTemplateOptions.MailTemplateSource.Embedded};
            var validator = new MailTemplateOptionsValidator();
            var result = validator.Validate(options);
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void ValidatesEmptyDefaultNamespace()
        {
            var options = new MailTemplateOptions
                {DefaultNamespace = string.Empty, TemplateSource = MailTemplateOptions.MailTemplateSource.Embedded};
            var validator = new MailTemplateOptionsValidator();
            var result = validator.Validate(options);
            var validated = result.Errors.Any(e =>
                e.PropertyName == nameof(MailTemplateOptions.DefaultNamespace) && e.ResourceName == NotEmptyValidator);

            Assert.IsTrue(validated);
        }

        [TestMethod]
        public void NotValidatesEmptyDefaultNamespace()
        {
            var options = new MailTemplateOptions
                {DefaultNamespace = string.Empty, TemplateSource = MailTemplateOptions.MailTemplateSource.File};
            var validator = new MailTemplateOptionsValidator();
            var result = validator.Validate(options);
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void ValidatesEmptyExtension()
        {
            var options = new MailTemplateOptions {Extension = string.Empty};
            var validator = new MailTemplateOptionsValidator();
            var result = validator.Validate(options);
            var validated = result.Errors.Any(e =>
                e.PropertyName == nameof(MailTemplateOptions.Extension) && e.ResourceName == NotEmptyValidator);

            Assert.IsTrue(validated);
        }

        [TestMethod]
        public void ValidatesExtensionStartsWithDot()
        {
            var options = new MailTemplateOptions {Extension = "cshtml"};
            var validator = new MailTemplateOptionsValidator();
            var result = validator.Validate(options);
            var validated = result.Errors.Any(e =>
                e.PropertyName == nameof(MailTemplateOptions.Extension) && e.ErrorCode == PredicateValidator);

            Assert.IsTrue(validated);
        }
    }
}