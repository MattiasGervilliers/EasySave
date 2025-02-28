using System;
using System.Text.Json.Serialization;

namespace LogLib
{
    /// <summary>
    /// Base class to represent a log entry
    /// </summary>
    public class Log
    {
        /// <summary>
        /// Severity level of the log (e.g., INFO, ERROR, DEBUG, etc.)
        /// </summary>
        public LogLevel Level { get; set; }

        /// <summary>
        /// Descriptive message of the log
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Date and time when the log was created
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Constructor for the Log class
        /// </summary>
        /// <param name="level"></param>
        /// <param name="message"></param>
        public Log(LogLevel level, string message)
        {
            Level = level; // Sets the log level
            Message = message; // Sets the log message
            Timestamp = DateTime.Now; // Captures the time when the log is created
        }
    }

    /// <summary>
    /// Enumeration defining the different log levels
    /// </summary>
    public enum LogLevel
    {
        TRACE,  // Very detailed information for advanced debugging
        DEBUG,  // Information for development
        INFO,   // General information about the execution
        WARN,   // Warnings about potential anomalies
        ERROR,  // Error that doesn't interrupt the application
        FATAL   // Critical error requiring immediate intervention
    }

    public enum LogType
    {
        Json,
        Xml
    }
}
