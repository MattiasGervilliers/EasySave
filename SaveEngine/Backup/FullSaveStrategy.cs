using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaveEngine.Backup
{
    public class FullSaveStrategy : ISaveStrategy
    {
        public void Save(string sourcePath, string destinationPath)
        {
            if (!Directory.Exists(sourcePath))
            {
                throw new DirectoryNotFoundException($"Le dossier source '{sourcePath}' n'existe pas.");
            }

            // Supprimer l'ancien dossier de destination s'il existe
            if (Directory.Exists(destinationPath))
            {
                Directory.Delete(destinationPath, true);
            }

            // Copier le dossier source dans le dossier destination
            Directory.CreateDirectory(destinationPath);
            foreach (string file in Directory.GetFiles(sourcePath, "*", SearchOption.AllDirectories))
            {
                string relativePath = file.Substring(sourcePath.Length + 1);
                string destFile = System.IO.Path.Combine(destinationPath, relativePath);
                Directory.CreateDirectory(System.IO.Path.GetDirectoryName(destFile));
                File.Copy(file, destFile, true);
            }

            Console.WriteLine("Sauvegarde complète effectuée.");
        }
    }
}
