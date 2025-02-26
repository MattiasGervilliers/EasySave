using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace LogLib
{
    public class LogWriter
    {
        // Répertoire où les fichiers de logs seront enregistrés.
        private string _logDirectoryPath;

        // Format du log (JSON ou XML).
        private LogType _logFormat;

        // Objet de verrouillage pour garantir une écriture sécurisée en multithreading.
        private static readonly object _fileLock = new object();

        /// <summary>
        /// Constructeur pour initialiser le LogWriter avec un répertoire et un format de log spécifiés.
        /// </summary>
        /// <param name="logDirectoryPath">Le chemin du répertoire où les logs seront sauvegardés.</param>
        /// <param name="logFormat">Le format de log (JSON ou XML).</param>
        public LogWriter(string logDirectoryPath, LogType logFormat)
        {
            _logDirectoryPath = logDirectoryPath;
            _logFormat = logFormat;
            EnsureDirectoryExists(); // Vérifie si le répertoire existe et le crée si nécessaire.
        }

        /// <summary>
        /// Vérifie si le répertoire des logs existe et le crée s'il n'existe pas.
        /// </summary>
        private void EnsureDirectoryExists()
        {
            // Si le répertoire n'existe pas, on le crée.
            if (!Directory.Exists(_logDirectoryPath))
            {
                Directory.CreateDirectory(_logDirectoryPath);
            }
        }

        /// <summary>
        /// Méthode pour écrire un log dans un fichier. Le format de log (JSON ou XML) est déterminé par _logFormat.
        /// </summary>
        /// <param name="log">L'objet log à écrire dans le fichier.</param>
        public void WriteLog(object log)
        {
            // Détermine l'extension du fichier en fonction du format de log.
            string extension = _logFormat == LogType.Json ? "json" : "xml";

            // Crée le chemin du fichier de log, avec un nom basé sur la date actuelle.
            string logFilePath = Path.Combine(_logDirectoryPath, $"log_{DateTime.Now:yyyyMMdd}.{extension}");

            // Verrouille l'écriture du fichier pour éviter les conflits en cas d'écriture simultanée.
            lock (_fileLock)
            {
                // Ouvre un StreamWriter pour ajouter des lignes dans le fichier de log.
                using (StreamWriter sw = new StreamWriter(logFilePath, true))
                {
                    // Si le format est JSON, sérialise l'objet log en JSON et l'écrit dans le fichier.
                    if (_logFormat == LogType.Json)
                    {
                        string json = JsonSerializer.Serialize(log, GetJsonOptions());
                        sw.WriteLine(json);
                    }
                    else
                    {
                        // Si le format est XML, sérialise l'objet log en XML et l'écrit dans le fichier.
                        XmlSerializer serializer = new XmlSerializer(log.GetType());
                        serializer.Serialize(sw, log);
                    }
                }
            }
        }

        /// <summary>
        /// Obtient les options de sérialisation JSON, y compris les paramètres pour la mise en forme et la conversion des énumérations.
        /// </summary>
        /// <returns>Les options de sérialisation JSON configurées.</returns>
        private JsonSerializerOptions GetJsonOptions()
        {
            return new JsonSerializerOptions
            {
                WriteIndented = true, // Active l'indentation pour rendre le fichier JSON lisible.
                Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) } // Convertit les énumérations en camelCase.
            };
        }
    }
}
