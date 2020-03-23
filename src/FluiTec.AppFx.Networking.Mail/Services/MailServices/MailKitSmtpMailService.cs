using System;
using System.Net.Security;
using System.Threading.Tasks;
using FluiTec.AppFx.Networking.Mail.Configuration;
using FluiTec.AppFx.Networking.Mail.Configuration.Validators;
using FluiTec.AppFx.Options.Exceptions;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;

namespace FluiTec.AppFx.Networking.Mail.Services
{
    /// <summary>MailService using MailKit and SMTP.</summary>
    /// <seealso cref="FluiTec.AppFx.Networking.Mail.Services.IMailService" />
    public abstract class MailKitSmtpMailService : IMailService
    {
        #region Fields

        private readonly MailServiceOptions _options;

        #endregion

        #region Properties

        /// <summary>Gets the optionsMonitor.</summary>
        /// <value>The optionsMonitor.</value>
        protected IOptionsMonitor<MailServiceOptions> OptionsMonitor { get; }

        /// <summary>Gets the options.</summary>
        /// <value>The options.</value>
        public MailServiceOptions Options => OptionsMonitor != null ? OptionsMonitor.CurrentValue : _options;

        /// <summary>Gets the certificate validation callback.</summary>
        /// <value>The certificate validation callback.</value>
        public abstract RemoteCertificateValidationCallback CertificateValidationCallback { get; }

        #endregion

        #region Constructors

        /// <summary>Initializes a new instance of the <see cref="MailKitSmtpMailService"/> class.</summary>
        /// <param name="options">The options.</param>
        protected MailKitSmtpMailService(MailServiceOptions options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
            ValidateOptions(options);
        }

        /// <summary>Initializes a new instance of the <see cref="MailKitSmtpMailService"/> class.</summary>
        /// <param name="optionsMonitor">The optionsMonitor.</param>
        protected MailKitSmtpMailService(IOptionsMonitor<MailServiceOptions> optionsMonitor)
        {
            OptionsMonitor = optionsMonitor ?? throw new ArgumentNullException(nameof(optionsMonitor));
            ValidateOptions(optionsMonitor.CurrentValue);
            OptionsMonitor.OnChange(ValidateOptions);
        }

        #endregion

        #region IMailService

        /// <summary>Sends the email.</summary>
        /// <param name="recipient">The recipient (email).</param>
        /// <param name="subject">The subject.</param>
        /// <param name="content">The content.</param>
        /// <param name="format">The format.</param>
        /// <param name="recipientName">Name of the recipient.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        /// <remarks>Empty recipientName means recipientName = recipient = email.</remarks>
        public void SendEmail(string recipient, string subject, string content, TextFormat format, string recipientName = null)
        {
            var message = new MimeMessage
            {
                From = { new MailboxAddress(Options.FromName, Options.FromMail)},
                To = { new MailboxAddress(recipient, recipientName ?? recipient)},
                Subject = subject,
                Body = new TextPart(format)
            };

            SendMail(message);
        }

        /// <summary>Sends the email asynchronous.</summary>
        /// <param name="recipient">The recipient (email).</param>
        /// <param name="subject">The subject.</param>
        /// <param name="content">The content.</param>
        /// <param name="format">The format.</param>
        /// <param name="recipientName">Name of the recipient.</param>
        /// <returns>The task used to send the email.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        /// <remarks>Empty recipientName means recipientName = recipient = email.</remarks>
        public async Task SendEmailAsync(string recipient, string subject, string content, TextFormat format, string recipientName = null)
        {
            var message = new MimeMessage
            {
                From = { new MailboxAddress(Options.FromName, Options.FromMail) },
                To = { new MailboxAddress(recipient, recipientName ?? recipient) },
                Subject = subject,
                Body = new TextPart(format)
            };

            await SendMailAsync(message);
        }

        #endregion

        #region Methods

        /// <summary>Gets the client.</summary>
        /// <returns></returns>
        protected virtual SmtpClient GetClient()
        {
            return new SmtpClient();
        }

        /// <summary>Sends the mail.</summary>
        /// <param name="message">The message.</param>
        protected virtual void SendMail(MimeMessage message)
        {
            using (var client = GetClient())
            {
                client.ServerCertificateValidationCallback = CertificateValidationCallback;

                // connect
                if (Options.EnableSsl)
                    client.Connect(Options.SmtpServer, Options.SmtpPort, Options.EnableSsl);
                else if (Options.SocketOptions != SecureSocketOptions.None)
                    client.Connect(Options.SmtpServer, Options.SmtpPort, Options.SocketOptions);
                else
                    client.Connect(Options.SmtpServer, Options.SmtpPort);

                // authenticate
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                if (Options.Authenticate)
                    client.Authenticate(Options.Username, Options.Password);

                // send
                client.Send(message);

                client.Disconnect(true);
            }
        }

        /// <summary>Sends the mail asynchronous.</summary>
        /// <param name="message">The message.</param>
        protected virtual async Task SendMailAsync(MimeMessage message)
        {
            using (var client = GetClient())
            {
                client.ServerCertificateValidationCallback = CertificateValidationCallback;

                // connect
                if (Options.EnableSsl)
                    await client.ConnectAsync(Options.SmtpServer, Options.SmtpPort, Options.EnableSsl);
                else if (Options.SocketOptions != SecureSocketOptions.None)
                    await client.ConnectAsync(Options.SmtpServer, Options.SmtpPort, Options.SocketOptions);
                else
                    await client.ConnectAsync(Options.SmtpServer, Options.SmtpPort);

                // authenticate
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                if (Options.Authenticate)
                    await client.AuthenticateAsync(Options.Username, Options.Password);

                // send
                await client.SendAsync(message);

                await client.DisconnectAsync(true);
            }
        }

        private void ValidateOptions(MailServiceOptions options)
        {
            var validationResult = new MailServiceOptionsValidator().Validate(options);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult, typeof(MailServiceOptions), "Invalid settings.");
        }

        #endregion
    }
}