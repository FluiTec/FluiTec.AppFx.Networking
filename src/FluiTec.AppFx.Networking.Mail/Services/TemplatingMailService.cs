using System;
using System.Threading.Tasks;
using MimeKit.Text;

namespace FluiTec.AppFx.Networking.Mail.Services
{
    /// <summary>A templating MailService.</summary>
    /// <seealso cref="FluiTec.AppFx.Networking.Mail.Services.ITemplatingMailService" />
    public class TemplatingMailService : ITemplatingMailService
    {
        #region Properties

        /// <summary>Gets the mail service.</summary>
        /// <value>The mail service.</value>
        public IMailService MailService { get; }

        /// <summary>Gets the templating service.</summary>
        /// <value>The templating service.</value>
        public ITemplatingService TemplatingService { get; }
        
        #endregion

        #region Constructors

        /// <summary>Initializes a new instance of the <see cref="TemplatingMailService"/> class.</summary>
        /// <param name="mailService">The mail service.</param>
        /// <param name="templatingService">The templating service.</param>
        public TemplatingMailService(IMailService mailService, ITemplatingService templatingService)
        {
            MailService = mailService ?? throw new ArgumentNullException(nameof(mailService));
            TemplatingService = templatingService ?? throw new ArgumentNullException(nameof(templatingService));
        }

        #endregion

        #region Methods

        /// <summary>Sends the mail.</summary>
        /// <param name="model">The model.</param>
        /// <param name="recipient">The recipient.</param>
        /// <param name="recipientName">Name of the recipient.</param>
        /// <exception cref="NotImplementedException"></exception>
        public void SendMail<TModel>(TModel model, string recipient, string recipientName = null) where TModel : IMailModel
        {
            var content = TemplatingService.Parse(model);
            MailService.SendEmail(recipient, model.Subject, content, TextFormat.Html, recipientName);
        }

        /// <summary>Sends the mail.</summary>
        /// <param name="model">The model.</param>
        /// <param name="templateName">Name of the template.</param>
        /// <param name="recipient">The recipient.</param>
        /// <param name="recipientName">Name of the recipient.</param>
        public void SendMail<TModel>(TModel model, string templateName, string recipient, string recipientName) where TModel : IMailModel
        {
            var content = TemplatingService.Parse(templateName, model);
            MailService.SendEmail(recipient, model.Subject, content, TextFormat.Html, recipientName);
        }

        /// <summary>Sends the mail asynchronous.</summary>
        /// <param name="model">The model.</param>
        /// <param name="recipient">The recipient.</param>
        /// <param name="recipientName">Name of the recipient.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task SendMailAsync<TModel>(TModel model, string recipient, string recipientName = null) where TModel : IMailModel
        {
            var content = TemplatingService.Parse(model);
            await MailService.SendEmailAsync(recipient, model.Subject, content, TextFormat.Html, recipientName);
        }

        /// <summary>Sends the mail asynchronous.</summary>
        /// <param name="model">The model.</param>
        /// <param name="templateName">Name of the template.</param>
        /// <param name="recipient">The recipient.</param>
        /// <param name="recipientName">Name of the recipient.</param>
        public async Task SendMailAsync<TModel>(TModel model, string templateName, string recipient, string recipientName) where TModel : IMailModel
        {
            var content = TemplatingService.Parse(templateName, model);
            await MailService.SendEmailAsync(recipient, model.Subject, content, TextFormat.Html, recipientName);
        }

        #endregion
    }
}