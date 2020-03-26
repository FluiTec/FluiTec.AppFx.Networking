using System;
using FluiTec.AppFx.Networking.Mail.RazorLightExtensions.LocationExpanders;
using FluiTec.AppFx.Networking.Mail.RazorLightExtensions.Projects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluiTec.AppFx.Networking.Mail.Tests.RazorLightExtensions.Projects
{
    [TestClass]
    public class LocationExpandingEmbeddedRazorProjectTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowsOnMissingExpanders()
        {
            var unused = new LocationExpandingEmbeddedRazorProject(typeof(GlobalTestSettings), null, null, "MailViewsEmbedded");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ThrowsOnMissingNamespace()
        {
            var unused = new LocationExpandingEmbeddedRazorProject(typeof(GlobalTestSettings), new IResourceExpander[] {}, null, "");
        }

        [TestMethod]
        public void DoesntThrowOnMissingResource()
        {
            var project = new LocationExpandingEmbeddedRazorProject(typeof(GlobalTestSettings), new IResourceExpander[] {}, null, "MailViewsEmbedded");
            Assert.IsFalse(project.GetItemAsync("Missing").Result.Exists);
        }

        [TestMethod]
        public void CanFindTemplate()
        {
            var project = new LocationExpandingEmbeddedRazorProject(typeof(GlobalTestSettings), new IResourceExpander[] {}, null, "MailViewsEmbedded");
            Assert.IsTrue(project.GetItemAsync("Test").Result.Exists);
        }
    }
}
