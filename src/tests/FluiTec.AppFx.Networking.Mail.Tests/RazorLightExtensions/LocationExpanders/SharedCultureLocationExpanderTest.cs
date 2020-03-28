using System.Globalization;
using FluiTec.AppFx.Networking.Mail.RazorLightExtensions.LocationExpanders;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluiTec.AppFx.Networking.Mail.Tests.RazorLightExtensions.LocationExpanders
{
    [TestClass]
    public class SharedCultureLocationExpanderTest : LocationExpanderTest
    {
        protected override IFileLocationExpander GetExpander()
        {
            return new SharedCultureLocationExpander();
        }

        protected override IResourceExpander GetResourceExpander()
        {
            return new SharedCultureLocationExpander();
        }

        [TestMethod]
        public void CanExpand()
        {
            TestExpanding("Test", $"Shared/{CultureInfo.CurrentUICulture.TwoLetterISOLanguageName}/Test");
        }

        [TestMethod]
        public void CanExpandResource()
        {
            TestResourceExpanding("Test", $"Shared.{CultureInfo.CurrentUICulture.TwoLetterISOLanguageName}.Test");
        }
    }
}