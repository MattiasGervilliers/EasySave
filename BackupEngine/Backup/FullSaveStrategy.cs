using System;
using System.IO;
using BackupEngine.State;
using System.Linq;
using BackupEngine.Log;

namespace BackupEngine.Backup
{
    public class FullSaveStrategy : SaveStrategy
    {
        public FullSaveStrategy(BackupConfiguration configuration) : base(configuration) { }

        public override void Save(string uniqueDestinationPath)
        {
            string sourcePath = Configuration.SourcePath.GetAbsolutePath();

            if (!Directory.Exists(sourcePath))
            {
                throw new DirectoryNotFoundException($"Le dossier source '{sourcePath}' n'existe pas.");
            }

            // Obtenir les fichiers à sauvegarder
            string[] files = Directory.GetFiles(sourcePath, "*", SearchOption.AllDirectories);
            int totalFiles = files.Length;
            long totalSize = files.Sum(file => new FileInfo(file).Length);
            int remainingFiles = totalFiles;
            long remainingSize = totalSize;

            // Mettre à jour l'état au début de la sauvegarde
            OnStateUpdated(new StateEvent(
                "Full Backup",
                "Actif",
                totalFiles,
                totalSize,
                remainingFiles,
                remainingSize,
                "",
                ""
            ));

            Console.WriteLine($"Sauvegarde {Configuration.Name} complète en cours...");

            // Parcourir chaque fichier et copier
            foreach (string file in files)
            {
                string relativePath = file.Substring(sourcePath.Length + 1);
                string destFile = Path.Combine(uniqueDestinationPath, relativePath);
                Directory.CreateDirectory(Path.GetDirectoryName(destFile));

                try
                {
                    DateTime start = DateTime.Now;

                    // Copy file using filestream to avoid file locking
                    using (FileStream sourceStream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        using (FileStream destStream = new FileStream(destFile, FileMode.Create, FileAccess.Write, FileShare.None))
                        {
                            sourceStream.CopyTo(destStream);
                        }
                    }

                    DateTime end = DateTime.Now;
                    TimeSpan duration = end - start;

                    // Mise à jour de l'état avec le fichier en cours
                    remainingFiles--;
                    remainingSize -= new FileInfo(file).Length;

                    // On envoie l'événement d'état
                    OnStateUpdated(new StateEvent(
                        "Full Backup",
                        "Actif",
                        totalFiles,
                        totalSize,
                        remainingFiles,
                        remainingSize,
                        file,
                        destFile
                    ));

                    // Créer l'événement de transfert (logique existante)
                    TransferEvent transferEvent = new TransferEvent(Configuration, duration, new FileInfo(file));
                    OnTransfer(transferEvent);
                }
                catch (Exception e)
                {
                    // En cas d'erreur lors de la copie
                    Console.WriteLine($"Erreur lors de la copie du fichier {file} : {e.Message}");
                    OnTransfer(new TransferEvent(Configuration, new TimeSpan(-1), new FileInfo(file)));
                }
            }

            // Mise à jour de l'état à la fin de la sauvegarde
            OnStateUpdated(new StateEvent(
                "Full Backup",
                "Terminé",
                totalFiles,
                totalSize,
                0,
                0,
                "",
                ""
            ));

            Console.WriteLine($"Sauvegarde complète effectuée dans : {uniqueDestinationPath}");
        }
    }
}
