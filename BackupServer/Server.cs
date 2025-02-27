using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BackupServer
{
    public class Server
    {
        private TcpListener _server;
        private bool _isRunning;
        private Action<string> _onCommandReceived;

        public Server(int port, Action<string> onCommandReceived)
        {
            _server = new TcpListener(IPAddress.Any, port);
            _onCommandReceived = onCommandReceived;
        }

        public void Start()
        {
            _isRunning = true;
            _server.Start();
            Console.WriteLine("BackupServer démarré sur le port " + ((IPEndPoint)_server.LocalEndpoint).Port);

            Task.Run(() =>
            {
                while (_isRunning)
                {
                    try
                    {
                        TcpClient client = _server.AcceptTcpClient();
                        Task.Run(() => HandleClient(client));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Erreur Serveur: " + ex.Message);
                    }
                }
            });
        }

        private void HandleClient(TcpClient client)
        {
            using (NetworkStream stream = client.GetStream())
            {
                StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                StreamWriter writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };

                while (_isRunning)
                {
                    try
                    {
                        string command = reader.ReadLine();
                        if (string.IsNullOrEmpty(command)) break;

                        Console.WriteLine($"Commande reçue : {command}");
                        _onCommandReceived?.Invoke(command);
                    }
                    catch
                    {
                        break;
                    }
                }
            }
        }

        public void SendUpdate(string message)
        {
            try
            {
                using (TcpClient client = new TcpClient("127.0.0.1", 5000)) // Adresse du client distant
                using (NetworkStream stream = client.GetStream())
                using (StreamWriter writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true })
                {
                    writer.WriteLine(message);
                }
            }
            catch
            {
                Console.WriteLine("Impossible d'envoyer l'update au client.");
            }
        }

        public void Stop()
        {
            _isRunning = false;
            _server.Stop();
        }
    }
}