using System;
using System.Text.Json.Serialization;

namespace LogLib
{
    /// <summary>
    /// Classe de base pour représenter un journal de log
    /// </summary>
    public class Log
    {
        /// <summary>
        /// Niveau de sévérité du log (ex: INFO, ERROR, DEBUG, etc.)
        /// </summary>
        public LogLevel Level { get; set; }

        /// <summary>
        /// Message descriptif du log
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Date et heure de création du log
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Constructeur de la classe Log
        /// </summary>
        /// <param name="level"></param>
        /// <param name="message"></param>
        public Log(LogLevel level, string message)
        {
            Level = level; // Définit le niveau du log
            Message = message; // Définit le message du log
            Timestamp = DateTime.Now; // Capture l'instant où le log est créé
        }
    }

    /// <summary>
    /// Enumération définissant les différents niveaux de logs
    /// </summary>
    public enum LogLevel
    {
        TRACE,  // Détails très fins pour le debugging avancé
        DEBUG,  // Infos pour le développement
        INFO,   // Informations générales sur l'exécution
        WARN,   // Avertissements sur des anomalies possibles
        ERROR,  // Erreur qui n'interrompt pas l'application
        FATAL   // Erreur critique nécessitant une intervention immédiate
    }
    public enum LogType
    {
        Json,
        Xml
    }
}
