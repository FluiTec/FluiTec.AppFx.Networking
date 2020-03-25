using MailKit;
using MailKit.Net.Smtp;

namespace FluiTec.AppFx.Networking.Mail.Factories
{
    /// <summary>Factory for IMailTransport using SmtpClient of MailKit.</summary>
    /// <seealso cref="FluiTec.AppFx.Networking.Mail.Factories.IMailTransportFactory" />
    public class MailKitSmtpTransportFactory : IMailTransportFactory 
    {
        /// <summary>Creates the new IMailTransport.</summary>
        /// <returns>A new IMailTransport.</returns>
        public IMailTransport CreateNew()
        {
            return new SmtpClient();
        }
    }
}