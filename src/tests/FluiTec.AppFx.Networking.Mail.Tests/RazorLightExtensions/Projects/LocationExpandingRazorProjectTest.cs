﻿using System;
using System.IO;
using FluiTec.AppFx.Networking.Mail.RazorLightExtensions.LocationExpanders;
using FluiTec.AppFx.Networking.Mail.RazorLightExtensions.Projects;
using FluiTec.AppFx.Networking.Mail.Tests.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluiTec.AppFx.Networking.Mail.Tests.RazorLightExtensions.Projects
{
    [TestClass]
    public class LocationExpandingRazorProjectTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowsOnMissingExpanders()
        {
            var unused = new LocationExpandingRazorProject(null, null, ApplicationHelper.GetMailViewPath());
        }

        [TestMethod]
        [ExpectedException(typeof(DirectoryNotFoundException))]
        public void ThrowsOnMissingDirectory()
        {
            var unused = new LocationExpandingRazorProject(new [] {new DefaultLocationExpander() }, null, string.Empty);
        }

        [TestMethod]
        public void DoesntThrowOnMissingFile()
        {
            var root = ApplicationHelper.GetApplicationPath();
            var project = new LocationExpandingRazorProject(new [] {new DefaultLocationExpander() }, null, ApplicationHelper.GetMailViewPath());
            Assert.IsFalse(project.GetItemAsync("Missing").Result.Exists);
        }

        [TestMethod]
        public void CanFindFile()
        {
            try
            {
                var project = new LocationExpandingRazorProject(new [] {new DefaultLocationExpander() }, null, ApplicationHelper.GetMailViewPath());
                Assert.IsTrue(project.GetItemAsync("Test").Result.Exists);
            }
            catch (Exception e)
            {
                throw new Exception(ApplicationHelper.GetMailViewPath());
            }
        }
    }
}
