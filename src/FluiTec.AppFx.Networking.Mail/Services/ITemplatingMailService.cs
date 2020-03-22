using System.Threading.Tasks;

namespace FluiTec.AppFx.Networking.Mail.Services
{
    /// <summary>A templating MailService.</summary>
    public interface ITemplatingMailService
    {
        /// <summary>Gets the mail service.</summary>
        /// <value>The mail service.</value>
        IMailService MailService { get; }

        /// <summary>Gets the templating service.</summary>
        /// <value>The templating service.</value>
        ITemplatingService TemplatingService { get; }

        /// <summary>Sends the mail.</summary>
        /// <param name="model">The model.</param>
        /// <param name="recipient">The recipient.</param>
        /// <param name="recipientName">Name of the recipient.</param>
        void SendMail(IMailModel model, string recipient, string recipientName = null);

        /// <summary>Sends the mail.</summary>
        /// <param name="model">The model.</param>
        /// <param name="templateName">Name of the template.</param>
        /// <param name="recipient">The recipient.</param>
        /// <param name="recipientName">Name of the recipient.</param>
        void SendMail(IMailModel model, string templateName, string recipient, string recipientName);

        /// <summary>Sends the mail asynchronous.</summary>
        /// <param name="model">The model.</param>
        /// <param name="recipient">The recipient.</param>
        /// <param name="recipientName">Name of the recipient.</param>
        /// <returns></returns>
        Task SendMailAsync(IMailModel model, string recipient, string recipientName = null);

        /// <summary>Sends the mail asynchronous.</summary>
        /// <param name="model">The model.</param>
        /// <param name="templateName">Name of the template.</param>
        /// <param name="recipient">The recipient.</param>
        /// <param name="recipientName">Name of the recipient.</param>
        /// <returns></returns>
        Task SendMailAsync(IMailModel model, string templateName, string recipient, string recipientName);
    }
}