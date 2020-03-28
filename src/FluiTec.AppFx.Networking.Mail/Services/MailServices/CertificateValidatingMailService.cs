using System;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using FluiTec.AppFx.Networking.Mail.Configuration;
using FluiTec.AppFx.Networking.Mail.Factories;
using MailKit;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FluiTec.AppFx.Networking.Mail.Services
{
    /// <summary>Configuration-based certificate-validating MailService using MailKit and Smtp.</summary>
    /// <seealso cref="FluiTec.AppFx.Networking.Mail.Services.MailKitSmtpMailService" />
    public class CertificateValidatingMailService : LoggingMailService
    {
        #region Fields

        private readonly MailServerCertificateValidationOptions _certificateOptions;

        #endregion

        #region Properties

        /// <summary>Gets the certificate options.</summary>
        /// <value>The certificate options.</value>
        public MailServerCertificateValidationOptions CertificateOptions => CertificateOptionsMonitor != null
            ? CertificateOptionsMonitor.CurrentValue
            : _certificateOptions;

        /// <summary>Gets the certificate options monitor.</summary>
        /// <value>The certificate options monitor.</value>
        public IOptionsMonitor<MailServerCertificateValidationOptions> CertificateOptionsMonitor { get; }

        /// <summary>Gets the certificate validation callback.</summary>
        /// <value>The certificate validation callback.</value>
        public override RemoteCertificateValidationCallback CertificateValidationCallback { get; }

        /// <summary>Gets the mail client factory.</summary>
        /// <value>The mail client factory.</value>
        public IMailTransportFactory MailClientFactory { get; }

        #endregion

        #region Constructors

        /// <summary>Initializes a new instance of the <see cref="CertificateValidatingMailService" /> class.</summary>
        /// <param name="options">The options.</param>
        /// <param name="logger">The logger to use.</param>
        /// <param name="certificateOptions">The certificateOptions.</param>
        /// <param name="mailClientFactory">The mailClientFactory.</param>
        /// <exception cref="ArgumentNullException">certificateOptions, options</exception>
        public CertificateValidatingMailService(MailServiceOptions options,
            ILogger<CertificateValidatingMailService> logger,
            MailServerCertificateValidationOptions certificateOptions,
            IMailTransportFactory mailClientFactory)
            : base(options, logger)
        {
            MailClientFactory = mailClientFactory ?? throw new ArgumentNullException(nameof(mailClientFactory));
            _certificateOptions = certificateOptions ?? throw new ArgumentNullException(nameof(certificateOptions));
            CertificateValidationCallback = ValidateCertificate;
        }

        /// <summary>
        ///     <para></para>
        ///     <para>Initializes a new instance of the <see cref="CertificateValidatingMailService" /> class.</para>
        /// </summary>
        /// <param name="optionsMonitor">The optionsMonitor.</param>
        /// <param name="logger">The logger to use.</param>
        /// <param name="certificateOptionsMonitor">The certificateOptionsMonitor</param>
        /// <param name="mailClientFactory">The mailClientFactory.</param>
        /// <exception cref="ArgumentNullException">certificateOptions, options</exception>
        public CertificateValidatingMailService(IOptionsMonitor<MailServiceOptions> optionsMonitor,
            ILogger<CertificateValidatingMailService> logger,
            IOptionsMonitor<MailServerCertificateValidationOptions> certificateOptionsMonitor,
            IMailTransportFactory mailClientFactory)
            : base(optionsMonitor, logger)
        {
            MailClientFactory = mailClientFactory ?? throw new ArgumentNullException(nameof(mailClientFactory));
            CertificateOptionsMonitor = certificateOptionsMonitor ??
                                        throw new ArgumentNullException(nameof(certificateOptionsMonitor));
            CertificateValidationCallback = ValidateCertificate;
        }

        #endregion

        #region Methods

        /// <summary>Gets the client.</summary>
        /// <returns>A mailClient implementing IMailTransport.</returns>
        protected override IMailTransport GetMailClient()
        {
            return MailClientFactory.CreateNew();
        }

        private bool ValidateCertificate(object sender, X509Certificate certificate, X509Chain chain,
            SslPolicyErrors sslpolicyerrors)
        {
            Logger?.LogInformation("Validating MailServer-Certificate.");

            // ignore everything in this case
            if (!CertificateOptions.Validate)
            {
                Logger?.LogInformation("-> No Validation enabled. Accepting connection.");
                return true;
            }

            // don't accept null certificates if validation was enabled
            if (certificate == null)
            {
                Logger?.LogInformation("-> Certificate was null. Denying connection.");
                return false;
            }

            // validate ssl policy errors
            if (!sslpolicyerrors.HasFlag(sslpolicyerrors))
            {
                Logger?.LogInformation($"-> Certificate has PolicyErrors. Denying connection. {sslpolicyerrors}");
                return false;
            }

            // validate the certificate itself
            if (!CertificateOptions.CertificateValidation.Validate)
            {
                Logger?.LogInformation("-> No special CertificateValidation enabled. Accepting connection.");
                return true;
            }

            // validate subject if any was set
            if (!string.IsNullOrWhiteSpace(CertificateOptions.CertificateValidation.Subject) &&
                CertificateOptions.CertificateValidation.Subject != certificate.Subject)
            {
                Logger?.LogInformation(
                    $"-> Certificate has wrong subject. Actual{certificate.Subject}, Expected: {CertificateOptions.CertificateValidation.Subject}. Denying connection.");
                return false;
            }

            // validate issuer if any was set
            if (!string.IsNullOrWhiteSpace(CertificateOptions.CertificateValidation.Issuer) &&
                CertificateOptions.CertificateValidation.Issuer != certificate.Issuer)
            {
                Logger?.LogInformation(
                    $"-> Certificate has wrong issuer. Actual{certificate.Issuer}, Expected: {CertificateOptions.CertificateValidation.Issuer}. Denying connection.");
                return false;
            }

            // validate serial if any was set
            if (!string.IsNullOrWhiteSpace(CertificateOptions.CertificateValidation.SerialNumber) &&
                CertificateOptions.CertificateValidation.SerialNumber != certificate.GetSerialNumberString())
            {
                Logger?.LogInformation(
                    $"-> Certificate has wrong serial. Actual{certificate.GetSerialNumberString()}, Expected: {CertificateOptions.CertificateValidation.SerialNumber}. Denying connection.");
                return false;
            }

            // validate hash if any was set
            if (!string.IsNullOrWhiteSpace(CertificateOptions.CertificateValidation.Hash) &&
                CertificateOptions.CertificateValidation.Hash != certificate.GetCertHashString())
            {
                Logger?.LogInformation(
                    $"-> Certificate has wrong hash. Actual{certificate.GetCertHashString()}, Expected: {CertificateOptions.CertificateValidation.Hash}. Denying connection.");
                return false;
            }

            // validate thumbprint if any was set and certificate is X509Certificate2
            if (!string.IsNullOrWhiteSpace(CertificateOptions.CertificateValidation.Thumbprint) &&
                certificate is X509Certificate2 x509Certificate2 &&
                CertificateOptions.CertificateValidation.Thumbprint != x509Certificate2.Thumbprint)
            {
                Logger?.LogInformation(
                    $"-> Certificate has wrong thumbprint. Actual{x509Certificate2.Thumbprint}, Expected: {CertificateOptions.CertificateValidation.Thumbprint}. Denying connection.");
                return false;
            }

            Logger?.LogInformation("-> Certificate fulfilled all configured requirements. Accepting connection.");
            return true;
        }

        #endregion
    }
}