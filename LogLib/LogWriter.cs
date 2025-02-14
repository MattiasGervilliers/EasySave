using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace LogLib
{
    public class LogWriter
    {
        private string _logDirectoryPath;
        private LogType _logFormat;
        private static readonly object _fileLock = new object();

        public LogWriter(string logDirectoryPath, LogType logFormat)
        {
            _logDirectoryPath = logDirectoryPath;
            _logFormat = logFormat;
            EnsureDirectoryExists();
        }

        private void EnsureDirectoryExists()
        {
            if (!Directory.Exists(_logDirectoryPath))
            {
                Directory.CreateDirectory(_logDirectoryPath);
            }
        }

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
