using FluiTec.AppFx.Options.Attributes;

namespace FLuiTec.AppFx.Networking.Mail.ConsoleSample.Configuration
{
    [ConfigurationKey("AppSettings")]
    public class ApplicationSettings
    {
        public string SampleSubject { get; set; }

        public string RecipientMail { get; set; }

        public string RecipientName { get; set; }
    }
}
