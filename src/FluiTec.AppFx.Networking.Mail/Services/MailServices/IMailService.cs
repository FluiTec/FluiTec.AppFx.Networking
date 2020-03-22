using System.Threading.Tasks;
using MimeKit.Text;

namespace FluiTec.AppFx.Networking.Mail.Services
{
    /// <summary>Service that can send emails.</summary>
    public interface IMailService
    {
        /// <summary>Sends the email.</summary>
        /// <param name="recipient">The recipient (email).</param>
        /// <param name="subject">The subject.</param>
        /// <param name="content">The content.</param>
        /// <param name="format">The format.</param>
        /// <param name="recipientName">Name of the recipient.</param>
        /// <remarks>
        /// Empty recipientName means recipientName = recipient = email.
        /// </remarks>
        void SendEmail(string recipient, string subject, string content, TextFormat format, string recipientName = null);

        /// <summary>Sends the email asynchronous.</summary>
        /// <param name="recipient">The recipient (email).</param>
        /// <param name="subject">The subject.</param>
        /// <param name="content">The content.</param>
        /// <param name="format">The format.</param>
        /// <param name="recipientName">Name of the recipient.</param>
        /// <returns>The task used to send the email.</returns>
        /// <remarks>
        /// Empty recipientName means recipientName = recipient = email.
        /// </remarks>
        Task SendEmailAsync(string recipient, string subject, string content, TextFormat format, string recipientName = null);
    }
}