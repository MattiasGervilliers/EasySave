using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LogLib
{
    public class LogWriter
    {
        private string _logDirectoryPath;

        public LogWriter(string logDirectoryPath)
        {
            _logDirectoryPath = logDirectoryPath;
            EnsureDirectoryExists();
        }

        // Vérifie si le répertoire existe, sinon le crée
        private void EnsureDirectoryExists()
        {
            if (!Directory.Exists(_logDirectoryPath))
            {
                Directory.CreateDirectory(_logDirectoryPath);
            }
        }

        // Écrit un log dans un fichier JSON
        public void WriteLog(Log log)
        {
            string logFilePath = Path.Combine(_logDirectoryPath, $"log_{DateTime.Now:yyyyMMdd}.json");

            List<Log> logs = new List<Log>();

            // Lire les logs existants s'il y en a
            if (File.Exists(logFilePath))
            {
                string existingJson = File.ReadAllText(logFilePath);
                logs = JsonSerializer.Deserialize<List<Log>>(existingJson, GetJsonOptions()) ?? new List<Log>();
            }

            // Ajouter le nouveau log
            logs.Add(log);

            // Sérialiser et sauvegarder
            string json = JsonSerializer.Serialize(logs, GetJsonOptions());
            File.WriteAllText(logFilePath, json);
        }

        // Convertit les enums en texte JSON
        private JsonSerializerOptions GetJsonOptions()
        {
            return new JsonSerializerOptions
            {
                WriteIndented = true,
                Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
            };
        }
    }
}
