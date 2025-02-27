using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace LogLib
{
    public class LogWriter
    {
        /// <summary>
        /// The directory path where log files are stored.
        /// </summary>
        private string _logDirectoryPath;

        /// <summary>
        /// The format type used for logging (JSON or XML).
        /// </summary>
        private LogType _logFormat;

        /// <summary>
        /// Lock object to ensure thread-safe file writing.
        /// </summary>
        private static readonly object _fileLock = new object();

        /// <summary>
        /// Initializes a new instance of the LogWriter class.
        /// </summary>
        /// <param name="logDirectoryPath">The directory where logs will be stored.</param>
        /// <param name="logFormat">The format type of the log files.</param>
        public LogWriter(string logDirectoryPath, LogType logFormat)
        {
            _logDirectoryPath = logDirectoryPath;
            _logFormat = logFormat;
            EnsureDirectoryExists();
        }
        /// <summary>
        /// Ensures that the log directory exists; creates it if it does not exist.
        /// </summary>
        private void EnsureDirectoryExists()
        {
            if (!Directory.Exists(_logDirectoryPath))
            {
                Directory.CreateDirectory(_logDirectoryPath);
            }
        }
        /// <summary>
        /// Writes a log entry to a file in the specified format.
        /// </summary>
        /// <param name="log">The log object to be written.</param>
        public void WriteLog(object log)
        {
            string extension = _logFormat == LogType.Json ? "json" : "xml";
            string logFilePath = Path.Combine(_logDirectoryPath, $"log_{DateTime.Now:yyyyMMdd}.{extension}");

            lock (_fileLock)
            {
                using (StreamWriter sw = new StreamWriter(logFilePath, true))
                {
                    if (_logFormat == LogType.Json)
                    {
                        string json = JsonSerializer.Serialize(log, GetJsonOptions());
                        sw.WriteLine(json);
                    }
                    else
                    {
                        XmlSerializer serializer = new XmlSerializer(log.GetType());
                        serializer.Serialize(sw, log);
                    }
                }
            }
        }
        /// <summary>
        /// Retrieves JSON serialization options, including indentation and enum conversion.
        /// </summary>
        /// <returns>Configured JsonSerializerOptions.</returns>
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
