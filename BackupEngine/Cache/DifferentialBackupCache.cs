using BackupEngine.Shared;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace BackupEngine.Cache
{
    /// <summary>
    /// Class representing a differential backup cache.
    /// Allows serializing and deserializing backup configurations from/to a JSON format.
    /// </summary>
    internal class DifferentialBackupCache : IJsonSerializable
    {
        /// <summary>
        /// List of cached backup configurations.
        /// </summary>
        public List<CachedConfiguration> Configurations { get; set; }

        /// <summary>
        /// Default constructor initializing the list of configurations.
        /// </summary>
        public DifferentialBackupCache()
        {
            Configurations = new List<CachedConfiguration>();
        }

        /// <summary>
        /// Method to deserialize JSON data and populate the list of configurations.
        /// </summary>
        public void FromJson(string json)
        {
            Configurations = JsonConvert.DeserializeObject<List<CachedConfiguration>>(json) ?? new List<CachedConfiguration>();
        }

        /// <summary>
        /// Method to serialize the list of configurations into JSON format.
        /// </summary>
        public string ToJson()
        {
            return JsonConvert.SerializeObject(Configurations);
        }
    }

    /// <summary>
    /// Class representing a cached backup configuration.
    /// </summary>
    internal class CachedConfiguration
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("backups")]
        public List<Backup> Backups { get; set; }
    }

    /// <summary>
    /// Class representing a specific backup with the date and directory name.
    /// </summary>
    public class Backup (DateTime date, string directoryName)
    {
        [JsonPropertyName("date")]
        public DateTime Date { get; set; } = date;

        [JsonPropertyName("directoryName")]
        public string DirectoryName { get; set; } = directoryName;
    }
}
