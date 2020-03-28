using System;
using System.IO;
using FluiTec.AppFx.Networking.Mail.RazorLightExtensions.LocationExpanders;
using FluiTec.AppFx.Networking.Mail.RazorLightExtensions.Projects;
using FluiTec.AppFx.Networking.Mail.Tests.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluiTec.AppFx.Networking.Mail.Tests.RazorLightExtensions.Projects
{
    [TestClass]
    public class LocationExpandingFileRazorProjectTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowsOnMissingExpanders()
        {
            var unused =
                new LocationExpandingFileRazorProject(null, null, ApplicationHelper.GetMailViewPath(), ".cshtml");
        }

        [TestMethod]
        [ExpectedException(typeof(DirectoryNotFoundException))]
        public void ThrowsOnMissingDirectory()
        {
            var unused = new LocationExpandingFileRazorProject(new[] {new DefaultLocationExpander()}, null,
                string.Empty, ".cshtml");
        }

        [TestMethod]
        public void DoesntThrowOnMissingFile()
        {
            var project = new LocationExpandingFileRazorProject(new[] {new DefaultLocationExpander()}, null,
                ApplicationHelper.GetMailViewPath(), ".cshtml");
            Assert.IsFalse(project.GetItemAsync("Missing").Result.Exists);
        }

        [TestMethod]
        public void CanFindFile()
        {
            var project = new LocationExpandingFileRazorProject(new[] {new DefaultLocationExpander()}, null,
                ApplicationHelper.GetMailViewPath(), ".cshtml");
            Assert.IsTrue(project.GetItemAsync("Test").Result.Exists);
        }
    }
}