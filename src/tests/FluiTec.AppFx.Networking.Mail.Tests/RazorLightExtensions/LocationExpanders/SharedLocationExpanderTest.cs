using FluiTec.AppFx.Networking.Mail.RazorLightExtensions.LocationExpanders;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluiTec.AppFx.Networking.Mail.Tests.RazorLightExtensions.LocationExpanders
{
    [TestClass]
    public class SharedLocationExpanderTest : LocationExpanderTest
    {
        protected override ILocationExpander GetExpander() => new SharedLocationExpander();

        [TestMethod]
        public void CanExpand() => TestExpanding("Test", "Shared/Test");
    }
}