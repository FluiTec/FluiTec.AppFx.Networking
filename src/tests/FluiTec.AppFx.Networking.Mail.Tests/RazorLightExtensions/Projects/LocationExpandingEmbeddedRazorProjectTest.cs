using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using FluiTec.AppFx.Networking.Mail.RazorLightExtensions.LocationExpanders;
using FluiTec.AppFx.Networking.Mail.RazorLightExtensions.Projects;
using FluiTec.AppFx.Networking.Mail.Tests.Helpers;
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
            var unused = new LocationExpandingEmbeddedRazorProject(typeof(GlobalTestSettings), null, null);
        }

        [TestMethod]
        public void CanFindTemplate()
        {
            var project = new LocationExpandingEmbeddedRazorProject(typeof(GlobalTestSettings), new ILocationExpander[] {}, null);
            var template = project.GetItemAsync("Test.cshtml").Result;
            var exists = template.Exists;
        }
    }
}
