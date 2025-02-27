using System.Windows;
using System.Windows.Controls.Primitives;

namespace RemoteConsole
{
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Client for communicating with the remote backup server.
        /// </summary>
        private RemoteClient _client;
        /// <summary>
        /// Initializes a new instance of the MainWindow class and establishes a connection to the backup server.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            _client = new RemoteClient();
            _client.OnMessageReceived += UpdateStatus;
            _client.Connect("127.0.0.1", 5000);
        }
        /// <summary>
        /// Updates the status box with messages received from the server.
        /// </summary>
        /// <param name="message">The message received from the server.</param>
        private void UpdateStatus(string message)
        {
            Dispatcher.Invoke(() => StatusBox.Text += message + "\n");
        }
        /// <summary>
        /// Sends a pause command to the backup server.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">Event data for the button click.</param>
        private void Pause_Click(object sender, RoutedEventArgs e)
        {
            _client.SendCommand("pause");
        }
        /// <summary>
        /// Sends a resume command to the backup server.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">Event data for the button click.</param>
        private void Resume_Click(object sender, RoutedEventArgs e)
        {
            _client.SendCommand("resume");
        }
    }
}