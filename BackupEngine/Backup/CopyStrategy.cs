using BackupEngine.Log;
using BackupEngine.Shared;
using BackupEngine.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BackupEngine.Backup
{
    public class CopyStrategy : ITransferStrategy
    {
        static CopyStrategy Instance = new CopyStrategy();

        public void TransferFile(string cheminSource, string uniqueDestinationPath)
        {
            string sourcePath = cheminSource;

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

                    // Créer l'événement de transfert (logique existante)
                }
                catch (Exception e)
                {
                    // En cas d'erreur lors de la copie
                    Console.WriteLine($"Erreur lors de la copie du fichier {file} : {e.Message}");
                }
            }

            // Mise à jour de l'état à la fin de la sauvegarde
           

            Console.WriteLine($"Sauvegarde complète effectuée dans : {uniqueDestinationPath}");
        }
    
    }
}
