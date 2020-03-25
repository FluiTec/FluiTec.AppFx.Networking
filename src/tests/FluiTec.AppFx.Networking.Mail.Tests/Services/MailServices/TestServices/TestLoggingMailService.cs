using System.Net.Security;
using FluiTec.AppFx.Networking.Mail.Configuration;
using FluiTec.AppFx.Networking.Mail.Services;
using MailKit;
using Microsoft.Extensions.Logging;

namespace FluiTec.AppFx.Networking.Mail.Tests.Services.MailServices.TestServices
{
    public class TestLoggingMailService : LoggingMailService
    {
        private readonly IMailTransport _mailClient;

        public TestLoggingMailService(MailServiceOptions options, ILogger<LoggingMailService> logger, IMailTransport mailClient) : base(options, logger)
        {
            _mailClient = mailClient;
        }

        public override RemoteCertificateValidationCallback CertificateValidationCallback => (sender, certificate, chain, errors) => true;

        protected override IMailTransport GetMailClient()
        {
            return _mailClient;
        }
    }
}