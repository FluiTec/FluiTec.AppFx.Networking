using System.Linq;
using FluiTec.AppFx.Networking.Mail.RazorLightExtensions.LocationExpanders;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluiTec.AppFx.Networking.Mail.Tests.RazorLightExtensions.LocationExpanders
{
    public abstract class LocationExpanderTest
    {
        protected abstract IFileLocationExpander GetExpander();
        protected abstract IResourceExpander GetResourceExpander();

        public void TestExpanding(string viewName, params string[] expexted)
        {
            var expander = GetExpander();
            var expanded = expander.Expand(viewName).ToList();
            foreach (var e in expexted)
            {
                Assert.IsTrue(expanded.Contains(e));
            }
        }

        public void TestResourceExpanding(string viewName, params string[] expexted)
        {
            var expander = GetResourceExpander();
            var expanded = expander.ExpandResource(viewName).ToList();
            foreach (var e in expexted)
            {
                Assert.IsTrue(expanded.Contains(e));
            }
        }
    }
}
