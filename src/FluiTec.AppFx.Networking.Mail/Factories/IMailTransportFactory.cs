using MailKit;

namespace FluiTec.AppFx.Networking.Mail.Factories
{
    /// <summary>Factory for IMailTransport.</summary>
    public interface IMailTransportFactory
    {
        /// <summary>Creates the new IMailTransport.</summary>
        /// <returns>A new IMailTransport.</returns>
        IMailTransport CreateNew();
    }
}