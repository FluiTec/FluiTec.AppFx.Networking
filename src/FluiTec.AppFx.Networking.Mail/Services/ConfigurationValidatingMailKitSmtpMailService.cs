using System;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using FluiTec.AppFx.Networking.Mail.Configuration;

namespace FluiTec.AppFx.Networking.Mail.Services
{
    /// <summary>Configuration-based certificate-validating MailService using MailKit and Smtp.</summary>
    /// <seealso cref="FluiTec.AppFx.Networking.Mail.Services.MailKitSmtpMailService" />
    public class ConfigurationValidatingMailKitSmtpMailService : MailKitSmtpMailService
    {
        #region Properties

        /// <summary>Gets the certificate options.</summary>
        /// <value>The certificate options.</value>
        public MailServerCertificateValidationOptions CertificateOptions { get; }

        #endregion


        /// <summary>Initializes a new instance of the <see cref="ConfigurationValidatingMailKitSmtpMailService"/> class.</summary>
        /// <param name="options">The options.</param>
        /// <param name="certificateOptions"></param>
        /// <exception cref="ArgumentNullException">certificateOptions, options</exception>
        public ConfigurationValidatingMailKitSmtpMailService(MailServiceOptions options, MailServerCertificateValidationOptions certificateOptions) : base(options)
        {
            CertificateOptions = certificateOptions ?? throw new ArgumentNullException(nameof(certificateOptions));
            CertificateValidationCallback = ValidateCertificate;
        }

        /// <summary>Gets the certificate validation callback.</summary>
        /// <value>The certificate validation callback.</value>
        public override RemoteCertificateValidationCallback CertificateValidationCallback { get; }

        private bool ValidateCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslpolicyerrors)
        {
            // ignore everything in this case
            if (!CertificateOptions.Validate) return true;

            // don't accept null certificates if validation was enabled
            if (certificate == null)
                return false;

            // validate ssl policy errors
            if (!sslpolicyerrors.HasFlag(sslpolicyerrors))
                return false;

            // validate the certificate itself
            if (!CertificateOptions.CertificateValidation.Validate) return true;

            // validate subject if any was set
            if (!string.IsNullOrWhiteSpace(CertificateOptions.CertificateValidation.Subject) &&
                CertificateOptions.CertificateValidation.Subject != certificate.Subject)
                return false;

            // validate issuer if any was set
            if (!string.IsNullOrWhiteSpace(CertificateOptions.CertificateValidation.Issuer) &&
                CertificateOptions.CertificateValidation.Issuer != certificate.Issuer)
                return false;

            // validate serial if any was set
            if (!string.IsNullOrWhiteSpace(CertificateOptions.CertificateValidation.SerialNumber) &&
                CertificateOptions.CertificateValidation.SerialNumber != certificate.GetSerialNumberString())
                return false;

            // validate hash if any was set
            if (!string.IsNullOrWhiteSpace(CertificateOptions.CertificateValidation.Hash) &&
                CertificateOptions.CertificateValidation.Hash != certificate.GetCertHashString())
                return false;

            // validate thumbprint if any was set and certificate is X509Certificate2
            if (!string.IsNullOrWhiteSpace(CertificateOptions.CertificateValidation.Thumbprint) &&
                certificate is X509Certificate2 x509Certificate2 && 
                CertificateOptions.CertificateValidation.Thumbprint != x509Certificate2.Thumbprint)
                return false;

            return true;
        }
    }
}