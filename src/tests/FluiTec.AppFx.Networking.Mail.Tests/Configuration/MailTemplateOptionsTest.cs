using FluiTec.AppFx.Networking.Mail.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluiTec.AppFx.Networking.Mail.Tests.Configuration
{
    [TestClass]
    public class MailTemplateOptionsTest
    {
        [TestMethod]
        public void DefaultsToMailViewsDirectory()
        {
            var options = new MailTemplateOptions();
            Assert.IsTrue(options.BaseDirectory == "MailViews");
        }

        [TestMethod]
        public void DefaultsToMailViewsNamespace()
        {
            var options = new MailTemplateOptions();
            Assert.IsTrue(options.DefaultNamespace == "MailViews");
        }

        [TestMethod]
        public void DefaultsToCshtmlExtension()
        {
            var options = new MailTemplateOptions();
            Assert.IsTrue(options.Extension == ".cshtml");
        }
    }
}