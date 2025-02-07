using System.Text.Json;
using System.Text.Json.Serialization;

namespace LogLib
{
    public class LogWriter
    {
        // Stocke le chemin du répertoire où seront sauvegardés les logs
        private string _logDirectoryPath;
        private static readonly object _fileLock = new object();

        // Constructeur qui initialise le chemin du répertoire et s'assure qu'il existe
        public LogWriter(string logDirectoryPath)
        {
            _logDirectoryPath = logDirectoryPath;
            EnsureDirectoryExists();
        }

        // Vérifie si le répertoire des logs existe, sinon il le crée
        private void EnsureDirectoryExists()
        {
            if (!Directory.Exists(_logDirectoryPath))
            {
                Directory.CreateDirectory(_logDirectoryPath);
            }
        }

        // Écrit un log dans un fichier JSON journalier
        public void WriteLog(object log)
        {
            // Génère un nom de fichier basé sur la date du jour (ex: log_20240204.json)
            string logFilePath = Path.Combine(_logDirectoryPath, $"log_{DateTime.Now:yyyyMMdd}.json");

            lock (_fileLock) // Ensure thread safety
            {
                using (StreamWriter sw = new StreamWriter(logFilePath, true)) // Append mode
                {
                    string json = JsonSerializer.Serialize(log, GetJsonOptions());
                    sw.WriteLine(json); // Write each log on a new line
                }
            }
        }

        // Configure les options de sérialisation JSON
        private JsonSerializerOptions GetJsonOptions()
        {
            return new JsonSerializerOptions
            {
                WriteIndented = true, // Indente le JSON pour le rendre lisible
                Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) } // Convertit les enums en format camelCase
            };
        }
    }
}
