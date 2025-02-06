using BackupEngine.Log;

namespace BackupEngine.Backup
{
    public class IncrementalSaveStrategy (BackupConfiguration configuration) : SaveStrategy (configuration)
    {
        public override void Save(string uniqueDestinationPath)
        {
            string sourcePath = Configuration.SourcePath.GetAbsolutePath();

            if (!Directory.Exists(sourcePath))
            {
                throw new DirectoryNotFoundException($"Le dossier source '{sourcePath}' n'existe pas.");
            }

            Directory.CreateDirectory(uniqueDestinationPath);

            foreach (string file in Directory.GetFiles(sourcePath, "*", SearchOption.AllDirectories))
            {
                string relativePath = file.Substring(sourcePath.Length + 1);
                string destFile = System.IO.Path.Combine(uniqueDestinationPath, relativePath);
                Directory.CreateDirectory(System.IO.Path.GetDirectoryName(destFile));

                if (!File.Exists(destFile) || File.GetLastWriteTimeUtc(file) > File.GetLastWriteTimeUtc(destFile))
                {
                    // Time the copy took
                    DateTime start = DateTime.Now;

                    // Copy the file
                    File.Copy(file, destFile, true);

                    DateTime end = DateTime.Now;
                    TimeSpan duration = end - start;

                    TransferEvent transferEvent = new TransferEvent(Configuration, duration, new FileInfo(file));
                    OnTransfer(transferEvent);
                    Console.WriteLine($"Fichier mis à jour : {relativePath}");
                }
            }

            Console.WriteLine("Sauvegarde incrémentale effectuée.");
        }
    }
}
