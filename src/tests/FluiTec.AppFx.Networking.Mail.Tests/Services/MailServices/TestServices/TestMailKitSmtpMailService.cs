using System.Net.Security;
using FluiTec.AppFx.Networking.Mail.Configuration;
using FluiTec.AppFx.Networking.Mail.Services;

namespace FluiTec.AppFx.Networking.Mail.Tests.Services.MailServices.TestServices
{
    public class TestMailKitSmtpMailService : MailKitSmtpMailService
    {
        public TestMailKitSmtpMailService(MailServiceOptions options) : base(options)
        {
        }

        public override RemoteCertificateValidationCallback CertificateValidationCallback => (sender, certificate, chain, errors) => true;
    }
}
