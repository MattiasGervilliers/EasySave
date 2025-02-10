using BackupEngine;
using EasySaveConsole.Model;

namespace EasySaveConsole.Controller
{
    internal class ArgumentsController
    {
        public static void LaunchWithArguments(string[] args)
        {
            HashSet<int> backupIds = ParseBackupArguments(args[0]);

            List<BackupConfiguration> configs = new List<BackupConfiguration>();

            foreach (int i in backupIds)
            {
                BackupConfiguration? config = BackupModel.FindConfig(i-1);
                if (config != null)
                {
                    configs.Add(config);
                }
                else
                {
                    Console.WriteLine($"Configuration {i} not found");
                }
            }

            BackupModel.LaunchConfigs(configs);
        }

        static HashSet<int> ParseBackupArguments(string input)
        {
            HashSet<int> backupIds = new HashSet<int>();

            foreach (string part in input.Split(';'))
            {
                if (part.Contains('-'))
                {
                    string[] rangeParts = part.Split('-');
                    if (rangeParts.Length == 2 && int.TryParse(rangeParts[0], out int start) && int.TryParse(rangeParts[1], out int end))
                    {
                        for (int i = start; i <= end; i++)
                        {
                            backupIds.Add(i);
                        }
                    }
                }
                else if (int.TryParse(part, out int singleBackup))
                {
                    backupIds.Add(singleBackup);
                }
            }

            return backupIds;
        }
    }
}
