using System;
using System.Linq.Expressions;
using System.Net.Security;
using FluiTec.AppFx.Networking.Mail.Configuration;
using FluiTec.AppFx.Networking.Mail.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Language.Flow;

namespace FluiTec.AppFx.Networking.Mail.Tests.Services.MailServices.TestServices
{
    public class TestLoggingMailService : LoggingMailService
    {
        public TestLoggingMailService(MailServiceOptions options, ILogger<LoggingMailService> logger) : base(options, logger)
        {
        }

        public TestLoggingMailService(IOptionsMonitor<MailServiceOptions> optionsMonitor, ILogger<LoggingMailService> logger) : base(optionsMonitor, logger)
        {
        }

        public override RemoteCertificateValidationCallback CertificateValidationCallback => (sender, certificate, chain, errors) => true;
    }
}