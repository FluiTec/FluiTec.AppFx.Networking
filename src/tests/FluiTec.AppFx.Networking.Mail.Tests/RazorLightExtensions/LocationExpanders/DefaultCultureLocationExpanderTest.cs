using System.Globalization;
using FluiTec.AppFx.Networking.Mail.RazorLightExtensions.LocationExpanders;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluiTec.AppFx.Networking.Mail.Tests.RazorLightExtensions.LocationExpanders
{
    [TestClass]
    public class DefaultCultureLocationExpanderTest : LocationExpanderTest
    {
        protected override IFileLocationExpander GetExpander()
        {
            return new DefaultCultureLocationExpander();
        }

        protected override IResourceExpander GetResourceExpander()
        {
            return new DefaultCultureLocationExpander();
        }

        [TestMethod]
        public void CanExpand()
        {
            TestExpanding("Test", $"{CultureInfo.CurrentUICulture.TwoLetterISOLanguageName}/Test");
        }

        [TestMethod]
        public void CanExpandResource()
        {
            TestResourceExpanding("Test", $"{CultureInfo.CurrentUICulture.TwoLetterISOLanguageName}.Test");
        }
    }
}