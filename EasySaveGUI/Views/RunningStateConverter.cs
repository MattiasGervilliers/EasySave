using System.Globalization;
using System.Windows.Data;
using BackupEngine;
using EasySaveGUI.ViewModels;

namespace EasySaveGUI.Views;

public class RunningStateConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values.Length < 2 || values[0] is not HomeViewModel viewModel || values[1] is not BackupConfiguration config)
            return "Play default"; // Valeur par défaut

        if (viewModel.IsLaunched(config) && viewModel.IsPaused(config))
            return "Resume";
        if (viewModel.IsLaunched(config))
            return "Pause";
        return "Play";
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
