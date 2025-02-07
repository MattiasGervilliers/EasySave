using BackupEngine.Backup; 
class Program
{
    static void Main()
    {
        string source = "C:\\DossierSource";
        string destination = "C:\\DossierSauvegarde";

        // Sauvegarde complète
        FileManager fileManager = new FileManager(new FullSaveStrategy());
        //fileManager.Save(source, destination);

        //// Sauvegarde incrémentale
        fileManager.SetSaveStrategy(new IncrementalSaveStrategy());
        fileManager.Save(source, destination);
    }
}
