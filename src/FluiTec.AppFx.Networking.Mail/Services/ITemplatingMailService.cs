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
    }
}