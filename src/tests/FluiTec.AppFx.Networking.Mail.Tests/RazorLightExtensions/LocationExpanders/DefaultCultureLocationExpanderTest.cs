using System.Globalization;
using FluiTec.AppFx.Networking.Mail.RazorLightExtensions.LocationExpanders;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluiTec.AppFx.Networking.Mail.Tests.RazorLightExtensions.LocationExpanders
{
    [TestClass]
    public class DefaultCultureLocationExpanderTest : LocationExpanderTest
    {
        protected override ILocationExpander GetExpander() => new DefaultCultureLocationExpander();

        [TestMethod]
        public void CanExpand() => TestExpanding("Test", $"{CultureInfo.CurrentUICulture.TwoLetterISOLanguageName}/Test");
    }
}