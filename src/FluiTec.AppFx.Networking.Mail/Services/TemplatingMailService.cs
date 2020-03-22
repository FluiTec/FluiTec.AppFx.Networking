using System;

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

        /// <summary>Initializes a new instance of the <see cref="TemplatingMailService"/> class.</summary>
        /// <param name="mailService">The mail service.</param>
        /// <param name="templatingService">The templating service.</param>
        public TemplatingMailService(IMailService mailService, ITemplatingService templatingService)
        {
            MailService = mailService ?? throw new ArgumentNullException(nameof(mailService));
            TemplatingService = templatingService ?? throw new ArgumentNullException(nameof(templatingService));
        }
    }
}