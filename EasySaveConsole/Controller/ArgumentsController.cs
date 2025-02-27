using BackupEngine;
using EasySaveConsole.Model;

namespace EasySaveConsole.Controller
{
    internal class ArgumentsController : IController
    {
        private string[] _args;

        /// <summary>
        /// Executes the backup process by launching configurations with specified arguments.
        /// </summary>
        public void Execute()
        {
            if (IsHelpRequested()) // Checks if the "-help" argument is passed
            {
                DisplayHelp(); // Displays the help
            }
            else
            {
                LaunchWithArguments(); // Otherwise, launches the program with the arguments
            }
        }

        // Method to check if the "-help" argument is passed
        private bool IsHelpRequested()
        {
            return _args.Contains("-help");
        }

        // Method to display the documentation or help
        private void DisplayHelp()
        {
            Console.WriteLine("=== EasySave ===");
            Console.WriteLine("Usage: console.exe <arguments>");
            Console.WriteLine();
            Console.WriteLine("Arguments:");
            Console.WriteLine("  -help                 Displays this documentation.");
            Console.WriteLine("  <backupIds>           List of backup IDs to launch. Use semicolon-separated numbers (e.g., 1;3) or ranges (e.g., 1-3).");
            Console.WriteLine();
            Console.WriteLine("Example:");
            Console.WriteLine("  EasySaveConsole.exe 1;3");
            Console.WriteLine("  EasySaveConsole.exe 1-3");
            Console.WriteLine("===========================================");
        }

        /// <summary>
        /// Parses backup arguments and launches the corresponding configurations.
        /// </summary>
        public void LaunchWithArguments()
        {
            HashSet<int> backupIds = ParseBackupArguments(this._args[0]);

            List<BackupConfiguration> configs = new List<BackupConfiguration>();

            foreach (int i in backupIds)
            {
                BackupConfiguration? config = BackupModel.FindConfig(i - 1);
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

        /// <summary>
        /// Updates the arguments used for launching backups.
        /// </summary>
        public void UpdateArguments(string[] args)
        {
            this._args = args;
        }

        /// <summary>
        /// Parses a string input to extract backup configuration IDs, supporting both single values and ranges.
        /// </summary>
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
