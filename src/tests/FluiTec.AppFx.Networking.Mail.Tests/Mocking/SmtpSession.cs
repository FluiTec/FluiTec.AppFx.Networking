using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;

namespace FluiTec.AppFx.Networking.Mail.Tests.Mocking
{
  /// <summary>
    /// Simple session
    /// </summary>
    public class SmtpSession
    {
        #region Fields

        private readonly Socket _socket;
        private readonly string _serverName;

        #endregion

        #region Properties

        public int Id { get; }
        public List<string> History { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="id"></param>
        /// /// <param name="serverName"></param>
        public SmtpSession(Socket socket, int id, string serverName = "coolcat.de")
        {
            _socket = socket;
            Id = id;
            History = new List<string>();
            _serverName = serverName;
        }

        #endregion

        #region Handlers

        public delegate void DataHandler(SmtpSession sender, string line);
        public event DataHandler Received;
        public event DataHandler Sent;

        public delegate void EndHandler(SmtpSession sender);
        public event EndHandler End;

        public delegate void ExceptionHandler(SmtpSession sender, Exception ex);
        public event ExceptionHandler Error;

        #endregion

        #region Methods

        private void Write(TextWriter sw, string line)
        {
            History.Add(line);
            sw.WriteLine(line);
            Sent?.Invoke(this, line);
        }

        /// <summary>
        /// process session
        /// </summary>
        public void Process()
        {
            var networkStream = new NetworkStream(_socket);
            var streamWriter = new StreamWriter(networkStream);
            var streamReader = new StreamReader(networkStream);
            streamWriter.AutoFlush = true;

            try
            {
                streamWriter.WriteLine($"220 {_serverName} SMTP Mock Server Ready");
                bool datasent = false;

                var ts = DateTime.UtcNow;

                while (_socket.Connected)
                {
                    string line = streamReader.ReadLine();

                    History.Add(line);

                    if (string.IsNullOrEmpty(line))
                    {
                        if (ts.AddMinutes(1) <= DateTime.UtcNow)
                            break;

                        continue;
                    }

                    ts = DateTime.UtcNow;

                    Received?.Invoke(this, line);

                    if (line.ToUpper().StartsWith("QUIT"))
                    {
                        Write(streamWriter, $"221 {_serverName} Service closing transmission channel");
                        break;
                    }

                    if (line.ToUpper().StartsWith("DATA"))
                    {
                        datasent = true;
                        Write(streamWriter, "354 Immediate Reply");
                    }
                    else if (datasent && line.Trim() == ".")
                    {
                        datasent = false;
                        Write(streamWriter, "250 OK");
                    }
                    else if (!datasent)
                    {
                        Write(streamWriter, "250 OK");
                    }
                }

                End?.Invoke(this);
            }
            catch (Exception ex)
            {
                Error?.Invoke(this, ex);
            }
            finally
            {
                streamReader.Close();
                streamWriter.Close();
                networkStream.Close();
                _socket.Close();
            }

        }

        #endregion
    }
}
