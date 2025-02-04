using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LogLib
{
    public class LogWriter
    {
        // Stocke le chemin du répertoire où seront sauvegardés les logs
        private string _logDirectoryPath;

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
        public void WriteLog(Log log)
        {
            // Génère un nom de fichier basé sur la date du jour (ex: log_20240204.json)
            string logFilePath = Path.Combine(_logDirectoryPath, $"log_{DateTime.Now:yyyyMMdd}.json");

            // Initialise une liste pour stocker les logs
            List<object> logs = new List<object>();

            // Vérifie si le fichier de log du jour existe déjà
            if (File.Exists(logFilePath))
            {
                // Lit le contenu existant du fichier
                string existingJson = File.ReadAllText(logFilePath);

                // Désérialise le fichier JSON en une liste d'objets (si possible)
                logs = JsonSerializer.Deserialize<List<object>>(existingJson, GetJsonOptions()) ?? new List<object>();
            }

            // Ajoute le nouveau log à la liste
            logs.Add(log);

            // Sérialise la liste mise à jour en JSON
            string json = JsonSerializer.Serialize(logs, GetJsonOptions());

            // Écrit le JSON dans le fichier, écrasant l'ancien contenu
            File.WriteAllText(logFilePath, json);
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
