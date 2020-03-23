using System;
using System.IO;
using System.Net.Sockets;

namespace FluiTec.AppFx.Networking.Mail.Tests.Mocking
{
  /// <summary>
    /// Simple session
    /// </summary>
    public class SmtpSession
    {
        private readonly Socket _socket;

        public delegate void DataHandler(SmtpSession sender, string line);
        public event DataHandler Received;
        public event DataHandler Sent;

        public delegate void EndHandler(SmtpSession sender);
        public event EndHandler End;

        public delegate void ExceptionHandler(SmtpSession sender, Exception ex);
        public event ExceptionHandler Error;

        private void Write(TextWriter sw, string line)
        {
            sw.WriteLine(line);
            Sent?.Invoke(this, line);
        }

        public int Id { get; }

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
                streamWriter.WriteLine("220 coolcat.de SMTP Mock Server Ready");
                bool datasent = false;

                var ts = DateTime.UtcNow;

                while (_socket.Connected)
                {
                    string line = streamReader.ReadLine();

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
                        Write(streamWriter, "221 coolcat.de Service closing transmission channel");
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

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="socket"></param>
        /// /// <param name="id"></param>
        public SmtpSession(Socket socket, int id)
        {
            _socket = socket;
            Id = id;
        }
    }
}
