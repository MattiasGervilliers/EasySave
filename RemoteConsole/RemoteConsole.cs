using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace RemoteConsole
{
    public class RemoteClient
    {
        /// <summary>
        /// TCP client for remote console communication with the backup server.
        /// </summary>
        private TcpClient _client;

        /// <summary>
        /// Stream reader for receiving messages from the server.
        /// </summary>
        private StreamReader _reader;

        /// <summary>
        /// Stream writer for sending commands to the server.
        /// </summary>
        private StreamWriter _writer;

        /// <summary>
        /// Event triggered when a message is received from the server.
        /// </summary>
        public event Action<string> OnMessageReceived;

        /// <summary>
        /// Initializes a new instance of the RemoteConsole class.
        /// </summary>
        public void RemoteConsole() { }
        /// <summary>
        /// Establishes a connection to the backup server.
        /// </summary>
        /// <param name="ip">The IP address of the server.</param>
        /// <param name="port">The port number on which the server is listening.</param>
        public void Connect(string ip, int port)
        {
            _client = new TcpClient(ip, port);
            NetworkStream stream = _client.GetStream();
            _reader = new StreamReader(stream, Encoding.UTF8);
            _writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };

            Task.Run(() => ListenForUpdates());
        }
        /// <summary>
        /// Establishes a connection to the backup server.
        /// </summary>
        /// <param name="ip">The IP address of the server.</param>
        /// <param name="port">The port number on which the server is listening.</param>
        private void ListenForUpdates()
        {
            while (true)
            {
                try
                {
                    string message = _reader.ReadLine();
                    if (string.IsNullOrEmpty(message)) break;
                    OnMessageReceived?.Invoke(message);
                }
                catch
                {
                    break;
                }
            }
        }
        /// <summary>
        /// Sends a command to the backup server.
        /// </summary>
        /// <param name="command">The command to send.</param>
        public void SendCommand(string command)
        {
            _writer.WriteLine(command);
        }
    }
}