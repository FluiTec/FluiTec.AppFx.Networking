using FluiTec.AppFx.Networking.Mail.Configuration;
using FluiTec.AppFx.Networking.Mail.Tests.Services.MailServices.TestServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluiTec.AppFx.Networking.Mail.Tests.Services.MailServices
{
    [TestClass]
    public class LoggingMailServiceTest : MailKitSmtpMailServiceTest
    {
        [TestMethod]
        public void DoesNotThrowOnEmptyLogger()
        {
            int port = GetSmtpPort();
            
            var unused = new TestLoggingMailService(new MailServiceOptions
            {
                SmtpServer = SmtpServer,
                SmtpPort = port,
                FromName = SmtpName, FromMail = SmtpMail
            }, null);
        }
    }
}
