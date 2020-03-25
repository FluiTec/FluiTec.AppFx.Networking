using System.Linq;
using MailKit;
using MailKit.Security;
using MimeKit;
using Moq;

namespace FluiTec.AppFx.Networking.Mail.Tests.Helpers
{
    public static class MailTransportMockHelper
    {
        public static void VerifySendMail(this Mock<IMailTransport> mailTransportMock, string expectedContent)
        {
            // verify connection
            mailTransportMock.Verify(mock => 
                mock.Connect
                (
                    GlobalTestSettings.SmtpServer,+
                    GlobalTestSettings.SmtpPort,
                    It.IsAny<SecureSocketOptions>(),
                    default
                )    
            );

            // verify sending
            mailTransportMock.Verify(mock =>
                mock.Send
                (
                    It.Is<MimeMessage>(message => 
                        message.TextBody == expectedContent &&
                        message.From.Single().ToString() == $"\"{GlobalTestSettings.SmtpName}\" <{GlobalTestSettings.SmtpMail}>" &&
                        message.To.Single().ToString() == $"\"{GlobalTestSettings.SmtpName}\" <{GlobalTestSettings.SmtpMail}>" &&
                        message.Subject == GlobalTestSettings.MailSubject &&
                        ((TextPart)message.Body).Text == expectedContent),
                    default, 
                    null
                )
            );
        }

        public static void VerifySendMailAsync(this Mock<IMailTransport> mailTransportMock, string expectedContent)
        {
            // verify connection
            mailTransportMock.Verify(mock =>
                mock.ConnectAsync
                (
                    GlobalTestSettings.SmtpServer, +
                        GlobalTestSettings.SmtpPort,
                    It.IsAny<SecureSocketOptions>(),
                    default
                )
            );

            // verify sending
            mailTransportMock.Verify(mock =>
                mock.SendAsync
                (
                    It.Is<MimeMessage>(message =>
                        message.TextBody == expectedContent &&
                        message.From.Single().ToString() == $"\"{GlobalTestSettings.SmtpName}\" <{GlobalTestSettings.SmtpMail}>" &&
                        message.To.Single().ToString() == $"\"{GlobalTestSettings.SmtpName}\" <{GlobalTestSettings.SmtpMail}>" &&
                        message.Subject == GlobalTestSettings.MailSubject &&
                        ((TextPart)message.Body).Text == expectedContent),
                    default,
                    null
                )
            );
        }
    }
}
