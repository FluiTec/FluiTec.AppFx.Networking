using System.Threading.Tasks;
using FluiTec.AppFx.Networking.Mail.Configuration;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using MimeKit;

namespace FluiTec.AppFx.Networking.Mail.Services
{
    /// <summary>Logging mail service.</summary>
    public abstract class LoggingMailService : MailKitSmtpMailService
    {
        #region Properties

        /// <summary>Gets the logger.</summary>
        /// <value>The logger.</value>
        protected ILogger<LoggingMailService> Logger { get; }

        #endregion

        #region Constructors

        /// <summary>Initializes a new instance of the <see cref="LoggingMailService"/> class.</summary>
        /// <param name="options">The options.</param>
        /// <param name="logger">The logger.</param>
        protected LoggingMailService(MailServiceOptions options, ILogger<LoggingMailService> logger) : base(options)
        {
            Logger = logger; // we accept null here
        }

        #endregion

        #region overrides

        /// <summary>Gets the client.</summary>
        /// <returns>An SmtpClient with a ProtocolLogger.</returns>
        protected override SmtpClient GetClient()
        {
            // ReSharper disable once UseObjectOrCollectionInitializer
            var client = new SmtpClient();
            client.Connected += (sender, args) => Logger?.LogDebug($"SMTP-Client connected. Host: {args.Host}");
            client.Authenticated += (sender, args) => Logger?.LogDebug("SMTP-Client authenticated.");
            client.MessageSent += (sender, args) => Logger?.LogDebug("SMTP-Client sent message.");
            client.Disconnected += (sender, args) => Logger?.LogDebug("SMTP-Client disconnected.");
            return client;
        }

        /// <summary>Sends the mail.</summary>
        /// <param name="message">The message.</param>
        protected override void SendMail(MimeMessage message)
        {
            Logger?.LogInformation($"Sending mail. Subject: {message.Subject}, Recipients:");
            foreach(var r in message.To)
                Logger?.LogInformation($"- {r}");

            base.SendMail(message);
            Logger?.LogInformation("Successfully sent mail.");
        }

        /// <summary>Sends the mail asynchronous.</summary>
        /// <param name="message">The message.</param>
        /// <returns>A task wrapping the operation.</returns>
        protected override async Task SendMailAsync(MimeMessage message)
        {
            Logger?.LogInformation($"Sending mail. Subject: {message.Subject}, Recipients:");
            foreach (var r in message.To)
                Logger?.LogInformation($"- {r}");

            await base.SendMailAsync(message);
            Logger?.LogInformation("Successfully sent mail.");
        }

        #endregion
    }
}