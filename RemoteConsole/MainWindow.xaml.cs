using System.Windows;
using System.Windows.Controls.Primitives;

namespace RemoteConsole
{
    public partial class MainWindow : Window
    {
        private RemoteClient _client;

        public MainWindow()
        {
            InitializeComponent();
            _client = new RemoteClient();
            _client.OnMessageReceived += UpdateStatus;
            _client.Connect("127.0.0.1", 5000);
        }

        private void UpdateStatus(string message)
        {
            Dispatcher.Invoke(() => StatusBox.Text += message + "\n");
        }

        private void Pause_Click(object sender, RoutedEventArgs e)
        {
            _client.SendCommand("pause");
        }

        private void Resume_Click(object sender, RoutedEventArgs e)
        {
            _client.SendCommand("resume");
        }
    }
}