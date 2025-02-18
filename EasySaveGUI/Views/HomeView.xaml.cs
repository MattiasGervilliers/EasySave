using BackupEngine;
using EasySaveGUI.ViewModels;
using System.Windows.Controls;

namespace EasySaveGUI.Views
{
    /// <summary>
    /// Logique d'interaction pour HomeView.xaml
    /// </summary>
    public partial class HomeView : UserControl
    {
        public HomeView()
        {
            InitializeComponent();
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataContext is HomeViewModel viewModel && sender is ListBox listBox)
            {
                // Sync selected items with the ViewModel
                viewModel.SelectedConfigurations.Clear();
                foreach (var item in listBox.SelectedItems.Cast<BackupConfiguration>())
                {
                    viewModel.SelectedConfigurations.Add(item);
                }
            }
        }
    }
}
