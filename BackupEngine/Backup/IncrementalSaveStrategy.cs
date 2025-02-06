using System;
using System.IO;

namespace BackupEngine.Backup
{
    public class IncrementalSaveStrategy : SaveStrategy
    {
        public override void Save(string sourcePath, string destinationPath)
        {
            if (!Directory.Exists(sourcePath))
            {
                throw new DirectoryNotFoundException($"Le dossier source '{sourcePath}' n'existe pas.");
            }

            Directory.CreateDirectory(destinationPath);

            foreach (string file in Directory.GetFiles(sourcePath, "*", SearchOption.AllDirectories))
            {
                string relativePath = file.Substring(sourcePath.Length + 1);
                string destFile = System.IO.Path.Combine(destinationPath, relativePath);
                Directory.CreateDirectory(System.IO.Path.GetDirectoryName(destFile));

                if (!File.Exists(destFile) || File.GetLastWriteTimeUtc(file) > File.GetLastWriteTimeUtc(destFile))
                {
                    File.Copy(file, destFile, true);
                    Console.WriteLine($"Fichier mis à jour : {relativePath}");
                }
            }

            Console.WriteLine("Sauvegarde incrémentale effectuée.");
        }
    }
}
