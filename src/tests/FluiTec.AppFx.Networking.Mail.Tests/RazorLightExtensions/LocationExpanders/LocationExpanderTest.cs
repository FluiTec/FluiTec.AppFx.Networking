using System.Linq;
using FluiTec.AppFx.Networking.Mail.RazorLightExtensions.LocationExpanders;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluiTec.AppFx.Networking.Mail.Tests.RazorLightExtensions.LocationExpanders
{
    public abstract class LocationExpanderTest
    {
        protected abstract ILocationExpander GetExpander();

        public void TestExpanding(string viewName, params string[] expexted)
        {
            var expander = GetExpander();
            var expanded = expander.Expand(viewName).ToList();
            foreach (var e in expexted)
            {
                Assert.IsTrue(expanded.Contains(e));
            }
        }
    }
}
