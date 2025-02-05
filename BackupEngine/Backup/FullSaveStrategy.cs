using System;
using System.IO;
using BackupEngine.Backup;

public class FullSaveStrategy : ISaveStrategy
{
    public void Save(string sourcePath, string destinationPath)
    {
        if (!Directory.Exists(sourcePath))
        {
            throw new DirectoryNotFoundException($"Le dossier source '{sourcePath}' n'existe pas.");
        }

        foreach (string file in Directory.GetFiles(sourcePath, "*", SearchOption.AllDirectories))
        {
            string relativePath = file.Substring(sourcePath.Length + 1);
            string destFile = Path.Combine(destinationPath, relativePath);
            Directory.CreateDirectory(Path.GetDirectoryName(destFile));
            File.Copy(file, destFile, true);
        }

        Console.WriteLine($"Sauvegarde complète effectuée dans : {destinationPath}");
    }
}
