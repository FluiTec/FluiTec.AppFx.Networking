﻿using FluiTec.AppFx.Networking.Mail.RazorLightExtensions.LocationExpanders;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluiTec.AppFx.Networking.Mail.Tests.RazorLightExtensions.LocationExpanders
{
    [TestClass]
    public class SharedLocationExpanderTest : LocationExpanderTest
    {
        protected override IFileLocationExpander GetExpander()
        {
            return new SharedLocationExpander();
        }

        protected override IResourceExpander GetResourceExpander()
        {
            return new SharedLocationExpander();
        }

        [TestMethod]
        public void CanExpand()
        {
            TestExpanding("Test", "Shared/Test");
        }

        [TestMethod]
        public void CanExpandResource()
        {
            TestResourceExpanding("Test", "Shared.Test");
        }
    }
}