using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace FluiTec.AppFx.Networking.Mail.Tests.Mocking
{
    /// <summary>
    /// Simple SMTP Mock server
    /// </summary>
    public class SmtpMock
    {
        private TcpListener _smtpListener;
        private Thread _smtpServerThread;

        /// <summary>
        /// start server
        /// </summary>
        public void Start()
        {
            _smtpServerThread = new Thread(Run);
            _smtpServerThread.Start();
        }

        public void Wait()
        {
            _smtpServerThread.Join();
        }

        /// <summary>
        /// stop server
        /// </summary>
        public void Stop()
        {
            _smtpListener?.Stop();
        }

        public delegate void ConnectionHandler(SmtpMock sender, Socket sock);
        public ConnectionHandler Connect = null;

        public delegate void ExceptionHandler(SmtpMock sender, Exception ex);
        public ExceptionHandler Error = null;

        public delegate void StartHandler(SmtpMock sender, TcpListener listener);
        public StartHandler Started = null;

        public event SmtpSession.DataHandler Received;
        public event SmtpSession.DataHandler Sent;
        public event SmtpSession.EndHandler End;

        public SmtpSession Session { get; private set; }

        /// <summary>
        /// run server
        /// </summary>
        private void Run()
        {
            try
            {
                _smtpListener = new TcpListener(IPAddress.Any, 25); // open listener for port 
                _smtpListener.Start();

                Started?.Invoke(this, _smtpListener);

                int count = 1;

                try
                {
                    while (true)
                    {
                        var clientSocket = _smtpListener.AcceptSocket();
                        Connect?.Invoke(this, clientSocket);
                        Session = new SmtpSession(clientSocket, count);
                        Session.Error += session_Error;
                        Session.Sent += session_Sent;
                        Session.Received += session_Received;
                        Session.End += session_End;
                        var sessionThread = new Thread(Session.Process);
                        sessionThread.Start();
                        count++;
                    }
                }
                catch (InvalidOperationException)
                {
                    // server stopped
                }
                finally
                {
                    _smtpListener.Stop();
                }
            }
            catch (Exception ex)
            {
                Error?.Invoke(this, ex);

                _smtpListener.Stop();
            }
        }

        private void session_End(SmtpSession sender)
        {
            End?.Invoke(sender);
        }

        private void session_Received(SmtpSession sender, string line)
        {
            Received?.Invoke(sender, line);
        }

        private void session_Sent(SmtpSession sender, string line)
        {
            Sent?.Invoke(sender, line);
        }

        private void session_Error(SmtpSession sender, Exception ex)
        {
            Error?.Invoke(this, ex);
        }
    }
}
