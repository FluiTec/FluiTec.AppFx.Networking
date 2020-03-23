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
        #region Fields

        private TcpListener _smtpListener;
        private Thread _smtpServerThread;
        private readonly int _port;
        private readonly string _serverName;

        #endregion

        #region Properties

        public SmtpSession Session { get; private set; }

        #endregion

        #region Constructors

        public SmtpMock(int port = 25, string serverName = "coolcat.de")
        {
            _port = port;
            _serverName = serverName;
        }

        #endregion

        #region Events

        public event SmtpSession.DataHandler Received;
        public event SmtpSession.DataHandler Sent;
        public event SmtpSession.EndHandler End;

        #endregion

        #region Handlers

        public delegate void ConnectionHandler(SmtpMock sender, Socket sock);
        public ConnectionHandler Connect = null;

        public delegate void ExceptionHandler(SmtpMock sender, Exception ex);
        public ExceptionHandler Error = null;

        public delegate void StartHandler(SmtpMock sender, TcpListener listener);
        public StartHandler Started = null;

        #endregion
        
        #region Methods

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
        
        /// <summary>
        /// run server
        /// </summary>
        private void Run()
        {
            try
            {
                _smtpListener = new TcpListener(IPAddress.Any, _port); // open listener for port 
                _smtpListener.Start();

                Started?.Invoke(this, _smtpListener);

                int count = 1;

                try
                {
                    while (true)
                    {
                        var clientSocket = _smtpListener.AcceptSocket();
                        Connect?.Invoke(this, clientSocket);
                        Session = new SmtpSession(clientSocket, count, _serverName);
                        Session.Error += (sender, exception) => Error?.Invoke(this, exception);
                        Session.Sent += (sender, line) => Sent?.Invoke(sender, line);
                        Session.Received += (sender, line) => Received?.Invoke(sender, line);
                        Session.End += sender => End?.Invoke(sender);
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

        #endregion
    }
}
