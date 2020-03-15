using FluiTec.AppFx.Options.Attributes;

namespace FluiTec.AppFx.Networking.Mail.Configuration
{
    /// <summary>Options for a mail-service.</summary>
    [ConfigurationKey("MailTemplateOptions")]
    public class MailTemplateOptions
    {
        /// <summary>Initializes a new instance of the <see cref="MailTemplateOptions"/> class.</summary>
        public MailTemplateOptions()
        {
            BaseDirectory = "MailViews";
            Extension = ".cshtml";
        }

        /// <summary>Gets or sets the base directory.</summary>
        /// <value>The base directory.</value>
        public string BaseDirectory { get; set; }

        /// <summary>Gets or sets the extension.</summary>
        /// <value>The extension.</value>
        public string Extension { get; set; }
    }
}
