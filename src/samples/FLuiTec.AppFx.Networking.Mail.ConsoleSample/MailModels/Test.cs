using FluiTec.AppFx.Networking.Mail.Services;

namespace FLuiTec.AppFx.Networking.Mail.ConsoleSample.MailModels
{
    public class Test : IMailModel
    {
        public Test(string subject)
        {
            Subject = subject;
        }

        public string Subject { get; }
    }
}