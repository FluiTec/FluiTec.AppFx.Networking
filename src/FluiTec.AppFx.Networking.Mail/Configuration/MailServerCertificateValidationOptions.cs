using System.Net.Security;
using FluiTec.AppFx.Options.Attributes;

namespace FluiTec.AppFx.Networking.Mail.Configuration
{
    /// <summary>Options for validating a MailServerCertificate.</summary>
    [ConfigurationKey("MailServerCertificateValidationOptions")]
    public class MailServerCertificateValidationOptions
    {
        /// <summary>Initializes a new instance of the <see cref="MailServerCertificateValidationOptions"/> class.</summary>
        public MailServerCertificateValidationOptions()
        {
            Validate = true;
            AcceptablePolicyErrors = SslPolicyErrors.None;
            CertificateValidation = new X509CertificateValidationOptions();
        }

        /// <summary>Gets or sets a value indicating whether this <see cref="MailServerCertificateValidationOptions"/> is validate.</summary>
        /// <value><c>true</c> if validate; otherwise, <c>false</c>.</value>
        public bool Validate { get; set; }

        /// <summary>Gets or sets the acceptable policy errors.</summary>
        /// <value>The acceptable policy errors.</value>
        public SslPolicyErrors AcceptablePolicyErrors { get; set; }

        /// <summary>Gets or sets the certificate validation.</summary>
        /// <value>The certificate validation.</value>
        public X509CertificateValidationOptions CertificateValidation { get; set; }
    }

    /// <summary>Options for validating a X509Certificate</summary>
    public class X509CertificateValidationOptions
    {
        /// <summary>Initializes a new instance of the <see cref="X509CertificateValidationOptions"/> class.</summary>
        public X509CertificateValidationOptions()
        {
            Validate = false;
        }

        /// <summary>Gets or sets a value indicating whether this <see cref="X509CertificateValidationOptions"/> is validate.</summary>
        /// <value>
        ///   <c>true</c> if validate; otherwise, <c>false</c>.</value>
        public bool Validate { get; set; }

        /// <summary>Gets or sets the subject.</summary>
        /// <value>The subject.</value>
        public string Subject { get; set; }

        /// <summary>Gets or sets the issuer.</summary>
        /// <value>The issuer.</value>
        public string Issuer { get; set; }

        /// <summary>Gets or sets the serial number.</summary>
        /// <value>The serial number.</value>
        public string SerialNumber { get; set; }

        /// <summary>Gets or sets the hash.</summary>
        /// <value>The hash.</value>
        public string Hash { get; set; }

        /// <summary>Gets or sets the thumbprint.</summary>
        /// <value>The thumbprint.</value>
        public string Thumbprint { get; set; }
    }
}
