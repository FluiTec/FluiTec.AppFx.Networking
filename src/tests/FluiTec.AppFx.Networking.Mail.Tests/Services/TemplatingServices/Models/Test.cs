using FluiTec.AppFx.Networking.Mail.Services;

namespace FluiTec.AppFx.Networking.Mail.Tests.Services.TemplatingServices.Models
{
    public class Test : IMailModel
    {
        public string Subject => GlobalTestSettings.MailSubject;
    }
}
