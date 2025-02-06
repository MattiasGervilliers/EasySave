using BackupEngine.Settings;
using BackupEngine;

namespace EasySaveConsole.View
{
    internal class ViewLister
    {
        private readonly Language _language;

        public ViewLister(Language language)
        {
            _language = language;
        }

        public void AfficheConfigurations()
        {
            if (_language == Language.French)
            {
                Console.WriteLine("Liste des configurations de sauvegarde : ");
            }
            else
            {
                Console.WriteLine("List of backup configurations: ");
            }
        }

        public void AfficheConfiguration(BackupConfiguration configuration)
        {
            if (_language == Language.French)
            {
                Console.WriteLine($"Nom: ${configuration.Name} --- Dossier source: ${configuration.SourcePath.GetAbsolutePath()}" +
                    $" --- Dossier de destination: ${configuration.DestinationPath.GetAbsolutePath()} " +
                    $"--- Sauvegarde ${configuration.BackupType}");
            }
            else
            {
                Console.WriteLine($"Name: ${configuration.Name} --- Source folder: ${configuration.SourcePath.GetAbsolutePath()}" +
                    $" --- Destination folder: ${configuration.DestinationPath.GetAbsolutePath()} " +
                    $"--- Backup ${configuration.BackupType}");
            }
        }
    }
}
