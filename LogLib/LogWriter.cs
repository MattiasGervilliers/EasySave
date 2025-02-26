using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace LogLib
{
    public class LogWriter
    {
        // Directory where the log files will be saved.
        private string _logDirectoryPath;

        // Log format (JSON or XML).
        private LogType _logFormat;

        // Lock object to ensure thread-safe writing.
        private static readonly object _fileLock = new object();

        /// <summary>
        /// Constructor to initialize the LogWriter with a specified directory and log format.
        /// </summary>
        /// <param name="logDirectoryPath">The path of the directory where the logs will be saved.</param>
        /// <param name="logFormat">The log format (JSON or XML).</param>
        public LogWriter(string logDirectoryPath, LogType logFormat)
        {
            _logDirectoryPath = logDirectoryPath;
            _logFormat = logFormat;
            EnsureDirectoryExists(); // Checks if the directory exists and creates it if necessary.
        }

        /// <summary>
        /// Checks if the log directory exists and creates it if it does not.
        /// </summary>
        private void EnsureDirectoryExists()
        {
            // If the directory does not exist, create it.
            if (!Directory.Exists(_logDirectoryPath))
            {
                Directory.CreateDirectory(_logDirectoryPath);
            }
        }

        /// <summary>
        /// Method to write a log to a file. The log format (JSON or XML) is determined by _logFormat.
        /// </summary>
        /// <param name="log">The log object to be written to the file.</param>
        public void WriteLog(object log)
        {
            // Determines the file extension based on the log format.
            string extension = _logFormat == LogType.Json ? "json" : "xml";

            // Creates the log file path, with a name based on the current date.
            string logFilePath = Path.Combine(_logDirectoryPath, $"log_{DateTime.Now:yyyyMMdd}.{extension}");

            // Locks the file write to avoid conflicts in case of simultaneous writes.
            lock (_fileLock)
            {
                // Opens a StreamWriter to append lines to the log file.
                using (StreamWriter sw = new StreamWriter(logFilePath, true))
                {
                    // If the format is JSON, serialize the log object to JSON and write it to the file.
                    if (_logFormat == LogType.Json)
                    {
                        string json = JsonSerializer.Serialize(log, GetJsonOptions());
                        sw.WriteLine(json);
                    }
                    else
                    {
                        // If the format is XML, serialize the log object to XML and write it to the file.
                        XmlSerializer serializer = new XmlSerializer(log.GetType());
                        serializer.Serialize(sw, log);
                    }
                }
            }
        }

        /// <summary>
        /// Gets the JSON serialization options, including formatting and enum conversion settings.
        /// </summary>
        /// <returns>The configured JSON serialization options.</returns>
        private JsonSerializerOptions GetJsonOptions()
        {
            return new JsonSerializerOptions
            {
                WriteIndented = true, // Enables indentation to make the JSON file readable.
                Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) } // Converts enumerations to camelCase.
            };
        }
    }
}
