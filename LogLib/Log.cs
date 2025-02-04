using System;
using System.Text.Json.Serialization;

namespace LogLib
{
    // Classe de base pour représenter un journal de log
    public class Log
    {
        // Niveau de sévérité du log (ex: INFO, ERROR, DEBUG, etc.)
        public LogLevel Level { get; set; }

        // Message descriptif du log
        public string Message { get; set; }

        // Date et heure de création du log
        public DateTime Timestamp { get; set; }

        // Constructeur de la classe Log
        public Log(LogLevel level, string message)
        {
            Level = level; // Définit le niveau du log
            Message = message; // Définit le message du log
            Timestamp = DateTime.Now; // Capture l'instant où le log est créé
        }
    }

    // Enumération définissant les différents niveaux de logs
    public enum LogLevel
    {
        TRACE,  // Détails très fins pour le debugging avancé
        DEBUG,  // Infos pour le développement
        INFO,   // Informations générales sur l'exécution
        WARN,   // Avertissements sur des anomalies possibles
        ERROR,  // Erreur qui n'interrompt pas l'application
        FATAL   // Erreur critique nécessitant une intervention immédiate
    }
}
