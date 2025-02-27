using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace RemoteConsole
{
    public class RemoteClient
    {
        private TcpClient _client;
        private StreamReader _reader;
        private StreamWriter _writer;

        public event Action<string> OnMessageReceived;
        public void RemoteConsole() { }
        public void Connect(string ip, int port)
        {
            _client = new TcpClient(ip, port);
            NetworkStream stream = _client.GetStream();
            _reader = new StreamReader(stream, Encoding.UTF8);
            _writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };

            Task.Run(() => ListenForUpdates());
        }

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

        public void SendCommand(string command)
        {
            _writer.WriteLine(command);
        }
    }
}