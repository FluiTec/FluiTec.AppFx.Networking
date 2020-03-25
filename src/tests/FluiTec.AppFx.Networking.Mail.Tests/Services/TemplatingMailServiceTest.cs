using System;
using System.Threading;
using FluiTec.AppFx.Networking.Mail.Configuration;
using FluiTec.AppFx.Networking.Mail.Services;
using FluiTec.AppFx.Networking.Mail.Tests.Helpers;
using FluiTec.AppFx.Networking.Mail.Tests.Services.MailServices;
using FluiTec.AppFx.Networking.Mail.Tests.Services.MailServices.TestServices;
using FluiTec.AppFx.Networking.Mail.Tests.Services.TemplatingServices.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RazorLight;

namespace FluiTec.AppFx.Networking.Mail.Tests.Services
{
    [TestClass]
    public class TemplatingMailServiceTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowsOnMissingMailService()
        {
            var unused = new TemplatingMailService(null, new Mock<ITemplatingService>().Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowsOnMissingTemplatingService()
        {
            var unused = new TemplatingMailService(new Mock<IMailService>().Object, null);
        }

        [TestMethod]
        public void CanSendMailByModel()
        {
            var smtpMock = MailKitSmtpMailServiceTest.GetSmtpMock();
            var mailService = new TestMailKitSmtpMailService(MailKitSmtpMailServiceTest.GetTestMailServiceOptions(smtpMock.Port));
            var templateService = new RazorLightTemplatingService(new Mock<IRazorLightEngine>().Object, new MailTemplateOptions(), null);
            var service = new TemplatingMailService(mailService, templateService);

            try
            {
                smtpMock.Started = (sender, listener) =>
                {
                    var model = new Test();
                    service.SendMail(model, GlobalTestSettings.SmtpMail, GlobalTestSettings.SmtpName);
                    MailAssertHelper.VerifySuccessfulMail(smtpMock);
                };
                smtpMock.Start();
                Thread.SpinWait(10000);
            }
            finally
            {
                smtpMock.Stop();
            }
        }
    }
}
