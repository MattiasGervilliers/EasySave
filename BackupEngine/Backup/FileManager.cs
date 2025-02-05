using System;
using System.IO;
using BackupEngine.Backup;

public class FileManager
{
    private ISaveStrategy saveStrategy;

    public FileManager(ISaveStrategy saveStrategy)
    {
        this.saveStrategy = saveStrategy;
    }

    public void SetSaveStrategy(ISaveStrategy newStrategy)
    {
        saveStrategy = newStrategy;
    }

    public void Save(string sourcePath, string destinationBasePath)
    {
        if (!Directory.Exists(destinationBasePath))
        {
            Directory.CreateDirectory(destinationBasePath);
        }

        // Générer un nom unique pour le dossier de sauvegarde
        string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
        string uniqueDestinationPath = Path.Combine(destinationBasePath, $"{timestamp}_Sauvegarde");

        // Créer le dossier de sauvegarde
        Directory.CreateDirectory(uniqueDestinationPath);

        // Lancer la sauvegarde avec le bon dossier
        saveStrategy.Save(sourcePath, uniqueDestinationPath);
    }
}
