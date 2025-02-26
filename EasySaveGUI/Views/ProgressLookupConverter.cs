using BackupEngine;
using System.Globalization;
using System.Windows.Data;

namespace EasySaveGUI.Views;

public class ProgressLookupConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values.Length < 2) return 0;

        if (values[0] is Dictionary<BackupConfiguration, double> progressDict && values[1] is BackupConfiguration config)
        {
            return progressDict.TryGetValue(config, out double progress) ? progress : 0;
        }

        return 0;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
