using System;

namespace LogLib
{
    public enum LogLevel
    {
        TRACE,  // Détails très fins pour le debugging avancé
        DEBUG,  // Infos pour le développement
        INFO,   // Infos générales sur l'exécution
        WARN,   // Avertissements sur des anomalies possibles
        ERROR,  // Erreur qui n'interrompt pas l'application
        FATAL   // Erreur critique nécessitant une intervention immédiate
    }

    public class Log
    {
        public DateTime Timestamp { get; set; } // Horodatage
        public string Message { get; set; } // Message
        public LogLevel Level { get; set; } // Niveau de journalisation

        public Log(LogLevel level, string message)
        {
            Timestamp = DateTime.Now;
            Level = level;
            Message = message; // Ajout du message
        }
    }
}
