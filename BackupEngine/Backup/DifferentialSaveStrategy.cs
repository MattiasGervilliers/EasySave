using BackupEngine.Cache;
using BackupEngine.Log;
using BackupEngine.State;

namespace BackupEngine.Backup
{
    public class DifferentialSaveStrategy (BackupConfiguration configuration) : SaveStrategy (configuration)
    {
        private DifferentialBackupCacheRepository _cacheRepository = new DifferentialBackupCacheRepository();

        public override void Save(string uniqueDestinationPath)
        {

            if (PreviousSaveExists() && !NeedToPerformFullSave())
            {
                DifferentialSave(uniqueDestinationPath, PreviousSavePath());
            }
            else
            {
                PerformFullSave(uniqueDestinationPath);
                UpdateCache(uniqueDestinationPath);
            }
        }

        private void PerformFullSave(string uniqueDestinationPath)
        {
            FullSaveStrategy fullSaveStrategy = new FullSaveStrategy(Configuration);
            fullSaveStrategy.Transfer += (sender, e) => OnTransfer(e);
            fullSaveStrategy.StateUpdated += (sender, e) => OnStateUpdated(e);
            fullSaveStrategy.Save(uniqueDestinationPath);
        }

        private void DifferentialSave(string uniqueDestinationPath, string previousSavePath)
        {
            if (Configuration.Encrypt)
            {
                TransferStrategy = new CryptStrategy();
            }
            else
            {
                TransferStrategy = new CopyStrategy();
            }
            string sourcePath = Configuration.SourcePath.GetAbsolutePath();

            if (!Directory.Exists(sourcePath))
            {
                throw new DirectoryNotFoundException($"Le dossier source '{sourcePath}' n'existe pas.");
            }

            if (!Directory.Exists(previousSavePath))
            {
                throw new DirectoryNotFoundException($"Le dossier de la dernière sauvegarde complète '{previousSavePath}' n'existe pas.");
            }

            Directory.CreateDirectory(uniqueDestinationPath);

            string[] files = Directory.GetFiles(sourcePath, "*", SearchOption.AllDirectories);
            int totalFiles = files.Length;
            long totalSize = files.Sum(file => new FileInfo(file).Length);
            int remainingFiles = totalFiles;
            long remainingSize = totalSize;


            // Mettre à jour l'état au début de la sauvegarde
            OnStateUpdated(new StateEvent(
                "Differential Backup",
                "Active",
                totalFiles,
                totalSize,
                remainingFiles,
                remainingSize,
                "",
                ""
            ));

            foreach (string file in files)
            {
                string relativePath = file.Substring(sourcePath.Length + 1);
                string prevFile = Path.Combine(previousSavePath, relativePath);
                string destFile = Path.Combine(uniqueDestinationPath, relativePath);

                Directory.CreateDirectory(Path.GetDirectoryName(destFile));

                bool fileDoesNotExistInPrevious = !File.Exists(prevFile);
                bool fileHasChanged = fileDoesNotExistInPrevious || File.GetLastWriteTimeUtc(file) > File.GetLastWriteTimeUtc(prevFile);

                if (fileHasChanged)
                {
                    DateTime start = DateTime.Now;
                    // transfer the file
                    TransferStrategy.TransferFile(file, destFile);
                    DateTime end = DateTime.Now;
                    TimeSpan duration = end - start;

                    // Mise à jour de l'état avec le fichier en cours
                    remainingFiles--;
                    remainingSize -= new FileInfo(file).Length;

                    // On envoie l'événement d'état
                    OnStateUpdated(new StateEvent(
                        "Differential Backup",
                        "Active",
                        totalFiles,
                        totalSize,
                        remainingFiles,
                        remainingSize,
                        file,
                        destFile
                    ));

                    TransferEvent transferEvent = new TransferEvent(Configuration, duration, new FileInfo(file), new FileInfo(destFile));
                    OnTransfer(transferEvent);
                }
            }

            // Mise à jour de l'état à la fin de la sauvegarde
            OnStateUpdated(new StateEvent(
                "Differential Backup",
                "Completed",
                totalFiles,
                totalSize,
                0,
                0,
                "",
                ""
            ));

            Console.WriteLine($"Sauvegarde différentielle effectuée dans : {uniqueDestinationPath}");
            //UpdateCache(uniqueDestinationPath);
        }


        private bool PreviousSaveExists()
        {
            CachedConfiguration? cached = _cacheRepository.GetCachedConfiguration(Configuration);
            return cached != null;
        }

        private bool NeedToPerformFullSave()
        {
            CachedConfiguration? cached = _cacheRepository.GetCachedConfiguration(Configuration);
            if (cached == null)
            {
                return true;
            }
            return cached.Backups.Count % 10 == 0;
        }

        private string PreviousSavePath()
        {
            CachedConfiguration? cached = _cacheRepository.GetCachedConfiguration(Configuration);
            return cached!.Backups.OrderByDescending(b => b.Date).First().DirectoryName;
        }

        private void UpdateCache(string uniqueDestinationPath)
        {
            _cacheRepository.AddBackup(Configuration, DateTime.Now, uniqueDestinationPath);
        }
    }
}
