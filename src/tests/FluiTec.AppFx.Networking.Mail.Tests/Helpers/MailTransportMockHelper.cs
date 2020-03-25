using System.Linq;
using MailKit;
using MailKit.Security;
using MimeKit;
using Moq;

namespace FluiTec.AppFx.Networking.Mail.Tests.Helpers
{
    public static class MailTransportMockHelper
    {
        public static void VerifySendMail(this Mock<IMailTransport> mailTransportMock)
        {
            VerifySendMail(mailTransportMock, GlobalTestSettings.SmtpServer, GlobalTestSettings.SmtpPort,
                GlobalTestSettings.SmtpSenderName, GlobalTestSettings.SmtpSenderMail,
                GlobalTestSettings.SmtpRecipientName, GlobalTestSettings.SmtpRecipientMail,
                GlobalTestSettings.MailSubject, GlobalTestSettings.MailContent);
        }

        public static void VerifySendMailAsync(this Mock<IMailTransport> mailTransportMock)
        {
            VerifySendMailAsync(mailTransportMock, GlobalTestSettings.SmtpServer, GlobalTestSettings.SmtpPort,
                GlobalTestSettings.SmtpSenderName, GlobalTestSettings.SmtpSenderMail,
                GlobalTestSettings.SmtpRecipientName, GlobalTestSettings.SmtpRecipientMail,
                GlobalTestSettings.MailSubject, GlobalTestSettings.MailContent);
        }

        public static void VerifySendMail(this Mock<IMailTransport> mailTransportMock, string server, int port, string senderName, string senderMail, string recipientName, string recipientMail, string subject, string expectedContent)
        {
            // verify connection
            mailTransportMock.Verify(mock =>
                mock.Connect
                (
                    server, 
                    port,
                    It.IsAny<SecureSocketOptions>(),
                    default
                )
            );

            // verify sending
            mailTransportMock.Verify(mock =>
                mock.Send
                (
                    It.Is<MimeMessage>(message =>
                        message.From.Single().ToString() == $"\"{senderName}\" <{senderMail}>" &&
                        message.To.Single().ToString() == $"\"{recipientName}\" <{recipientMail}>" &&
                        message.Subject == subject &&
                        ((TextPart)message.Body).Text == expectedContent),
                    default,
                    null
                )
            );
        }

        public static void VerifySendMailAsync(this Mock<IMailTransport> mailTransportMock, string server, int port, string senderName, string senderMail, string recipientName, string recipientMail, string subject, string expectedContent)
        {
            // verify connection
            mailTransportMock.Verify(mock =>
                mock.ConnectAsync
                (
                    server,
                    port,
                    It.IsAny<SecureSocketOptions>(),
                    default
                )
            );

            // verify sending
            mailTransportMock.Verify(mock =>
                mock.SendAsync
                (
                    It.Is<MimeMessage>(message =>
                        message.From.Single().ToString() == $"\"{senderName}\" <{senderMail}>" &&
                        message.To.Single().ToString() == $"\"{recipientName}\" <{recipientMail}>" &&
                        message.Subject == subject &&
                        ((TextPart)message.Body).Text == expectedContent),
                    default,
                    null
                )
            );
        }
    }
}
