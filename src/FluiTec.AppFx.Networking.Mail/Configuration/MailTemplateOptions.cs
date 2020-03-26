using FluiTec.AppFx.Options.Attributes;

namespace FluiTec.AppFx.Networking.Mail.Configuration
{
    /// <summary>Options for a mail-service.</summary>
    [ConfigurationKey("MailTemplateOptions")]
    public partial class MailTemplateOptions
    {
        /// <summary>Initializes a new instance of the <see cref="MailTemplateOptions"/> class.</summary>
        public MailTemplateOptions()
        {
            BaseDirectory = "MailViews";
            DefaultNamespace = "MailViews";
            Extension = ".cshtml";
        }

        /// <summary>Gets or sets the base directory for MailViews.</summary>
        /// <value>The base directory for MailViews.</value>
        public string BaseDirectory { get; set; }

        /// <summary>Gets or sets the default namespace for MailViews.</summary>
        /// <value>The default namespace for MailViews.</value>
        public string DefaultNamespace { get; set; }

        /// <summary>Gets or sets the extension.</summary>
        /// <value>The extension.</value>
        public string Extension { get; set; }
        
        /// <summary> Gets or sets the TemplateSource.</summary>
        /// <value>The TemplateSource.</value>
        public MailTemplateSource TemplateSource { get; set; }
    }
}
