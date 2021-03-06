﻿using FluiTec.AppFx.Networking.Mail.Configuration;
using FluiTec.AppFx.Networking.Mail.Factories;
using FluiTec.AppFx.Networking.Mail.Services;
using Microsoft.Extensions.Logging;

namespace FluiTec.AppFx.Networking.Mail.Tests.Services.MailServices.TestServices
{
    public class TestCertificateValidatingMailService : CertificateValidatingMailService
    {
        public TestCertificateValidatingMailService(MailServiceOptions options,
            ILogger<CertificateValidatingMailService> logger,
            MailServerCertificateValidationOptions certificateOptions,
            IMailTransportFactory mailClientFactory)
            : base(options, logger, certificateOptions, mailClientFactory)
        {
        }
    }
}