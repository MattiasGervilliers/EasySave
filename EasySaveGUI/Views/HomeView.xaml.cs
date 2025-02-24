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
                foreach (var item in e.RemovedItems.Cast<BackupConfiguration>())
                {
                    viewModel.SelectedConfigurations.Remove(item);
                }

                foreach (var item in e.AddedItems.Cast<BackupConfiguration>())
                {
                    if (viewModel.SelectedConfigurations.Contains(item))
                    {
                        // Deselect if already selected (toggle effect)
                        listBox.SelectedItems.Remove(item);
                    }
                    else
                    {
                        viewModel.SelectedConfigurations.Add(item);
                    }
                }
            }
        }

    }
}
