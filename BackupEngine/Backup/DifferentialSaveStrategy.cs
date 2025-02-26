using BackupEngine.Cache;
using BackupEngine.Log;
using BackupEngine.State;

namespace BackupEngine.Backup
{
    // La classe DifferentialSaveStrategy hérite de la classe SaveStrategy et implémente une stratégie de sauvegarde différentielle
    // Elle permet de réaliser une sauvegarde uniquement des fichiers qui ont changé depuis la dernière sauvegarde
    public class DifferentialSaveStrategy : SaveStrategy
    {
        private DifferentialBackupCacheRepository _cacheRepository = new DifferentialBackupCacheRepository();

        // Méthode principale de la stratégie de sauvegarde différentielle
        public override void Save(string uniqueDestinationPath)
        {
            // Si une sauvegarde précédente existe, on effectue une sauvegarde différentielle
            if (PreviousSaveExists())
            {
                string previousSavePath = PreviousSavePath();

                // Si la sauvegarde précédente n'existe pas (dossier manquant), effectuer une sauvegarde complète
                if (!Directory.Exists(previousSavePath))
                {
                    PerformFullSave(uniqueDestinationPath);
                    UpdateCache(uniqueDestinationPath);
                }
                else
                {
                    // Si la sauvegarde précédente existe, effectuer une sauvegarde différentielle
                    DifferentialSave(uniqueDestinationPath, previousSavePath);
                }
            }
            else
            {
                // Si aucune sauvegarde précédente n'existe, effectuer une sauvegarde complète
                PerformFullSave(uniqueDestinationPath);
                UpdateCache(uniqueDestinationPath);
            }
        }

        // Effectuer une sauvegarde complète en appelant la stratégie de sauvegarde complète
        private void PerformFullSave(string uniqueDestinationPath)
        {
            FullSaveStrategy fullSaveStrategy = new FullSaveStrategy(Configuration);
            // Abonnement aux événements de transfert et de mise à jour de l'état
            fullSaveStrategy.Transfer += (sender, e) => OnTransfer(e);
            fullSaveStrategy.StateUpdated += (sender, e) => OnStateUpdated(e);
            // Exécution de la sauvegarde complète
            fullSaveStrategy.Save(uniqueDestinationPath);
        }

        // Effectuer une sauvegarde différentielle en comparant les fichiers avec la sauvegarde précédente
        private void DifferentialSave(string uniqueDestinationPath, string previousSavePath)
        {
            // Choisir la stratégie de transfert en fonction de la configuration (cryptage ou copie)
            if (Configuration.Encrypt)
            {
                TransferStrategy = new CryptStrategy();
            }
            else
            {
                TransferStrategy = new CopyStrategy();
            }

            // Obtenir le chemin absolu du dossier source
            string sourcePath = Configuration.SourcePath.GetAbsolutePath();

            // Vérifier que le dossier source existe
            if (!Directory.Exists(sourcePath))
            {
                throw new DirectoryNotFoundException($"Le dossier source '{sourcePath}' n'existe pas.");
            }

            // Créer le dossier de destination si nécessaire
            Directory.CreateDirectory(uniqueDestinationPath);

            // Récupérer la liste de tous les fichiers dans le dossier source
            string[] files = Directory.GetFiles(sourcePath, "*", SearchOption.AllDirectories);
            int totalFiles = files.Length;
            long totalSize = files.Sum(file => new FileInfo(file).Length);
            int remainingFiles = totalFiles;
            long remainingSize = totalSize;

            // Mise à jour de l'état avec les informations initiales de la sauvegarde
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

            // Traitement de chaque fichier dans le dossier source
            foreach (string file in files)
            {
                string relativePath = file.Substring(sourcePath.Length + 1);
                string prevFile = Path.Combine(previousSavePath, relativePath);
                string destFile = Path.Combine(uniqueDestinationPath, relativePath);

                // Créer les sous-dossiers nécessaires dans le dossier de destination
                Directory.CreateDirectory(Path.GetDirectoryName(destFile));

                // Vérifier si le fichier a changé depuis la dernière sauvegarde
                bool fileDoesNotExistInPrevious = !File.Exists(prevFile);
                bool fileHasChanged = fileDoesNotExistInPrevious || File.GetLastWriteTimeUtc(file) > File.GetLastWriteTimeUtc(prevFile);

                if (fileHasChanged)
                {
                    DateTime start = DateTime.Now;
                    // Transférer le fichier si nécessaire
                    TransferStrategy.TransferFile(file, destFile);
                    DateTime end = DateTime.Now;
                    TimeSpan duration = end - start;

                    // Mise à jour de l'état avec les informations du fichier en cours de transfert
                    remainingFiles--;
                    remainingSize -= new FileInfo(file).Length;

                    // Envoi de l'événement d'état pour ce fichier
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

                    // Créer et envoyer l'événement de transfert
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

            // Affichage d'un message de confirmation dans la console
            Console.WriteLine($"Sauvegarde différentielle effectuée dans : {uniqueDestinationPath}");
            // Mise à jour du cache (commenté ici)
            //UpdateCache(uniqueDestinationPath);
        }

        // Vérifier si une sauvegarde précédente existe en utilisant le cache
        private bool PreviousSaveExists()
        {
            CachedConfiguration? cached = _cacheRepository.GetCachedConfiguration(Configuration);
            return cached != null;
        }

        // Récupérer le chemin de la dernière sauvegarde effectuée à partir du cache
        private string PreviousSavePath()
        {
            CachedConfiguration? cached = _cacheRepository.GetCachedConfiguration(Configuration);
            // Récupère le dernier répertoire de sauvegarde
            return cached!.Backups.OrderByDescending(b => b.Date).First().DirectoryName;
        }

        // Mettre à jour le cache avec les informations de la nouvelle sauvegarde
        private void UpdateCache(string uniqueDestinationPath)
        {
            _cacheRepository.AddBackup(Configuration, DateTime.Now, uniqueDestinationPath);
        }
    }
}
