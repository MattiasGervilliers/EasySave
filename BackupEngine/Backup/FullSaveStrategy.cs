﻿using BackupEngine.Log;

namespace BackupEngine.Backup
{
    public class FullSaveStrategy(BackupConfiguration configuration) : SaveStrategy(configuration)
    {
        public override void Save(string uniqueDestinationPath)
        {
            string sourcePath = Configuration.SourcePath.GetAbsolutePath();

            if (!Directory.Exists(sourcePath))
            {
                throw new DirectoryNotFoundException($"Le dossier source '{sourcePath}' n'existe pas.");
            }

            foreach (string file in Directory.GetFiles(sourcePath, "*", SearchOption.AllDirectories))
            {
                string relativePath = file.Substring(sourcePath.Length + 1);
                string destFile = Path.Combine(uniqueDestinationPath, relativePath);
                Directory.CreateDirectory(Path.GetDirectoryName(destFile));
                
                try
                {
                    // Time the copy took
                    DateTime start = DateTime.Now;
                    
                    // Copy the file
                    File.Copy(file, destFile, true);
                    
                    DateTime end = DateTime.Now;
                    TimeSpan duration = end - start;

                    TransferEvent transferEvent = new TransferEvent(Configuration, duration, new FileInfo(file));
                    OnTransfer(transferEvent);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Erreur lors de la copie du fichier {file} : {e.Message}");
                    OnTransfer(new TransferEvent(Configuration, new TimeSpan(-1), new FileInfo(file)));
                }

                Console.WriteLine($"Sauvegarde complète effectuée dans : {uniqueDestinationPath}");
            }
        }
    }
}