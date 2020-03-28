using System;
using FluiTec.AppFx.Networking.Mail.Configuration;
using FluiTec.AppFx.Networking.Mail.RazorLightExtensions.LocationExpanders;
using FluiTec.AppFx.Networking.Mail.RazorLightExtensions.Projects;
using FluiTec.AppFx.Networking.Mail.Services;
using FluiTec.AppFx.Networking.Mail.Tests.Helpers;
using FluiTec.AppFx.Networking.Mail.Tests.Services.MailServices;
using FluiTec.AppFx.Networking.Mail.Tests.Services.MailServices.TestServices;
using FluiTec.AppFx.Networking.Mail.Tests.Services.TemplatingServices.Models;
using MailKit;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RazorLight;
using IMailService = FluiTec.AppFx.Networking.Mail.Services.IMailService;

namespace FluiTec.AppFx.Networking.Mail.Tests.Services
{
    //[TestClass]
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
            var mailTransportMock = new Mock<IMailTransport>();
            var project = new LocationExpandingFileRazorProject(new[] {new DefaultLocationExpander()}, null,
                ApplicationHelper.GetMailViewPath(), ".cshtml");
            var engine = new RazorLightEngineBuilder()
                .UseProject(project)
                .UseMemoryCachingProvider()
                .Build();
            var mailService = new TestMailKitSmtpMailService(MailKitSmtpMailServiceTest.GetTestMailServiceOptions(),
                mailTransportMock.Object);
            var templateService = new RazorLightTemplatingService(engine, new MailTemplateOptions(), null);
            var service = new TemplatingMailService(mailService, templateService);

            var model = new Test();
            var body = templateService.Parse(model);
            service.SendMail(model, GlobalTestSettings.SmtpRecipientMail, GlobalTestSettings.SmtpRecipientName);
            mailTransportMock.VerifySendMail(GlobalTestSettings.SmtpServer, GlobalTestSettings.SmtpPort,
                GlobalTestSettings.SmtpSenderName, GlobalTestSettings.SmtpSenderMail,
                GlobalTestSettings.SmtpRecipientName, GlobalTestSettings.SmtpRecipientMail,
                GlobalTestSettings.MailSubject, body);
        }
    }
}