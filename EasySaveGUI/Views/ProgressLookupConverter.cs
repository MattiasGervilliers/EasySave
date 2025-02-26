using BackupEngine;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Data;


namespace EasySaveGUI.Views;

public class ProgressLookupConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values.Length < 2 || values[0] is not ObservableCollection<KeyValuePair<BackupConfiguration, double>> progressCollection || values[1] is not BackupConfiguration configuration)
        {
            return 0.0; // Default progress if binding fails
        }

        // Find progress
        var progress = progressCollection.FirstOrDefault(kvp => kvp.Key == configuration).Value;
        return progress;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
