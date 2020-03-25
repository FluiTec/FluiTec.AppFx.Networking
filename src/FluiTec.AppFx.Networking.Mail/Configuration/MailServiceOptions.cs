using FluiTec.AppFx.Options.Attributes;
using MailKit.Security;

namespace FluiTec.AppFx.Networking.Mail.Configuration
{
    /// <summary>Options for a mail-service.</summary>
    [ConfigurationKey("MailServiceOptions")]
    public class MailServiceOptions
    {
        /// <summary>Initializes a new instance of the <see cref="MailServiceOptions"/> class.</summary>
        public MailServiceOptions()
        {
            Authenticate = false;
            SmtpPort = 25;
            EnableSsl = false;
            SocketOptions = SecureSocketOptions.None;
            TimeOut = 10;
        }

        /// <summary>	Gets or sets a value indicating whether the authenticate. </summary>
        /// <value>	True if authenticate, false if not. </value>
        public bool Authenticate { get; set; }

        /// <summary>	Gets or sets the SMTP server. </summary>
        /// <value>	The SMTP server. </value>
        public string SmtpServer { get; set; }

        /// <summary>	Gets or sets the SMTP port. </summary>
        /// <value>	The SMTP port. </value>
        public int SmtpPort { get; set; }

        /// <summary>	Gets or sets a value indicating whether the ssl is enabled. </summary>
        /// <value>	True if enable ssl, false if not. </value>
        public bool EnableSsl { get; set; }

        /// <summary>Gets or sets the socket options.</summary>
        /// <value>The socket options.</value>
        public SecureSocketOptions SocketOptions { get; set; }

        /// <summary>	Gets or sets the username. </summary>
        /// <value>	The username. </value>
        public string Username { get; set; }

        /// <summary>	Gets or sets the password. </summary>
        /// <value>	The password. </value>
        public string Password { get; set; }

        /// <summary>	Gets or sets from mail. </summary>
        /// <value>	from mail. </value>
        public string FromMail { get; set; }

        /// <summary>	Gets or sets the name of from. </summary>
        /// <value>	The name of from. </value>
        public string FromName { get; set; }

        /// <summary>Gets or sets the time out.</summary>
        /// <value>The time out.</value>
        public int TimeOut { get; set; }
    }
}
