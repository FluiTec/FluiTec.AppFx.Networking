﻿using FluiTec.AppFx.Networking.Mail.RazorLightExtensions.LocationExpanders;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluiTec.AppFx.Networking.Mail.Tests.RazorLightExtensions.LocationExpanders
{
    [TestClass]
    public class DefaultLocationExpanderTest : LocationExpanderTest
    {
        protected override IFileLocationExpander GetExpander()
        {
            return new DefaultLocationExpander();
        }

        protected override IResourceExpander GetResourceExpander()
        {
            return new DefaultLocationExpander();
        }

        [TestMethod]
        public void CanExpand()
        {
            TestExpanding("Test", "Test");
        }

        [TestMethod]
        public void CanExpandRersource()
        {
            TestResourceExpanding("Test", "Test");
        }
    }
}