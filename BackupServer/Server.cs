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
        /// <summary>
        /// TCP server responsible for handling remote commands and client communication.
        /// </summary>
        private TcpListener _server;

        /// <summary>
        /// Indicates whether the server is currently running.
        /// </summary>
        private bool _isRunning;

        /// <summary>
        /// Delegate invoked when a command is received from a client.
        /// </summary>
        private Action<string> _onCommandReceived;

        /// <summary>
        /// Initializes a new instance of the Server class.
        /// </summary>
        /// <param name="port">The port number on which the server will listen.</param>
        /// <param name="onCommandReceived">The action to be executed when a command is received.</param>
        public Server(int port, Action<string> onCommandReceived)
        {
            _server = new TcpListener(IPAddress.Any, port);
            _onCommandReceived = onCommandReceived;
        }
        /// <summary>
        /// Starts the server and begins listening for incoming client connections.
        /// </summary>
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
        /// <summary>
        /// Handles communication with a connected client.
        /// </summary>
        /// <param name="client">The TCP client that has connected to the server.</param>
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
        /// <summary>
        /// Sends an update message to the remote client.
        /// </summary>
        /// <param name="message">The message to send.</param>
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
        /// <summary>
        /// Stops the server and disconnects all clients.
        /// </summary>
        public void Stop()
        {
            _isRunning = false;
            _server.Stop();
        }
    }
}