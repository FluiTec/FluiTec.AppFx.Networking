using System.Net.Security;
using FluiTec.AppFx.Networking.Mail.Configuration;
using FluiTec.AppFx.Networking.Mail.Services;
using MailKit;

namespace FluiTec.AppFx.Networking.Mail.Tests.Services.MailServices.TestServices
{
    public class TestMailKitSmtpMailService : MailKitSmtpMailService
    {
        private readonly IMailTransport _mailClient;

        public TestMailKitSmtpMailService(MailServiceOptions options, IMailTransport mailClient) : base(options)
        {
            _mailClient = mailClient;
        }

        public override RemoteCertificateValidationCallback CertificateValidationCallback =>
            (sender, certificate, chain, errors) => true;

        protected override IMailTransport GetMailClient()
        {
            return _mailClient;
        }
    }
}