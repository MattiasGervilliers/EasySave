using BackupEngine.Settings;
using BackupEngine;

namespace EasySaveConsole.View
{
    internal class LaunchBackupView (Language language)
    {
        public void AskBackupConfigurationName()
        {
            if (language == Language.French)
            {
                Console.WriteLine("Rentrez le nom de la configuration de sauvegarde à lancer");
            }
            else
            {
                Console.WriteLine("Enter the name of the backup configuration to launch");
            }
        }

        public void BackupLaunched()
        {
            if (language == Language.French)
            {
                Console.WriteLine("La sauvegarde a été lancée");
            }
            else
            {
                Console.WriteLine("The backup has been launched");
            }
        }

        public void ConfigNotFound()
        {
            if (language == Language.French)
            {
                Console.WriteLine("La configuration de sauvegarde n'a pas été trouvée");
            }
            else
            {
                Console.WriteLine("The backup configuration was not found");
            }
        }
    }
}
