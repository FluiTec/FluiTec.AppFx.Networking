using System.Threading.Tasks;
// ReSharper disable UnusedMemberInSuper.Global

namespace FluiTec.AppFx.Networking.Mail.Services
{
    /// <summary>A templating MailService.</summary>
    public interface ITemplatingMailService
    {
        /// <summary>Sends the mail.</summary>
        /// <param name="model">The model.</param>
        /// <param name="recipient">The recipient.</param>
        /// <param name="recipientName">Name of the recipient.</param>
        void SendMail<TModel>(TModel model, string recipient, string recipientName = null) where TModel : IMailModel;

        /// <summary>Sends the mail.</summary>
        /// <param name="model">The model.</param>
        /// <param name="templateName">Name of the template.</param>
        /// <param name="recipient">The recipient.</param>
        /// <param name="recipientName">Name of the recipient.</param>
        void SendMail<TModel>(TModel model, string templateName, string recipient, string recipientName) where TModel : IMailModel;

        /// <summary>Sends the mail asynchronous.</summary>
        /// <param name="model">The model.</param>
        /// <param name="recipient">The recipient.</param>
        /// <param name="recipientName">Name of the recipient.</param>
        /// <returns></returns>
        Task SendMailAsync<TModel>(TModel model, string recipient, string recipientName = null) where TModel : IMailModel;

        /// <summary>Sends the mail asynchronous.</summary>
        /// <param name="model">The model.</param>
        /// <param name="templateName">Name of the template.</param>
        /// <param name="recipient">The recipient.</param>
        /// <param name="recipientName">Name of the recipient.</param>
        /// <returns></returns>
        Task SendMailAsync<TModel>(TModel model, string templateName, string recipient, string recipientName) where TModel : IMailModel;
    }
}