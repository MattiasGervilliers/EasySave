using BackupEngine.Shared;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace BackupEngine.Cache
{
    /// <summary>
    /// Stores cached configurations for differential backup operations.
    /// </summary>
    internal class DifferentialBackupCache : IJsonSerializable
    {
        /// <summary>
        /// List of cached backup configurations.
        /// </summary>
        public List<CachedConfiguration> _configurations { get; set; }
        /// <summary>
        /// Initializes a new instance of the DifferentialBackupCache class.
        /// </summary>
        public DifferentialBackupCache()
        {
            _configurations = new List<CachedConfiguration>();
        }
        /// <summary>
        /// Deserializes the backup cache from a JSON string.
        /// </summary>
        /// <param name="json">The JSON string containing cached configurations.</param>
        public void FromJson(string json)
        {
            _configurations = JsonConvert.DeserializeObject<List<CachedConfiguration>>(json) ?? new List<CachedConfiguration>();
        }
        /// <summary>
        /// Serializes the backup cache to a JSON string.
        /// </summary>
        /// <returns>A JSON string representing the cached configurations.</returns>
        public string ToJson()
        {
            return JsonConvert.SerializeObject(_configurations);
        }
    }
    /// <summary>
    /// Represents a cached backup configuration.
    /// </summary>
    internal class CachedConfiguration
    {
        /// <summary>
        /// The name of the backup configuration.
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }
        /// <summary>
        /// The list of backups associated with this configuration.
        /// </summary>

        [JsonPropertyName("backups")]
        public List<Backup> Backups { get; set; }
    }

    /// <summary>
    /// Represents a backup entry with a timestamp and directory name.
    /// </summary>
    /// <param name="date">The date and time when the backup was created.</param>
    /// <param name="directoryName">The name of the directory where the backup is stored.</param>
    public class Backup (DateTime date, string directoryName)
    {
        /// <summary>
        /// The date and time when the backup was created.
        /// </summary>
        [JsonPropertyName("date")]
        public DateTime Date { get; set; } = date;
        /// <summary>
        /// The name of the directory where the backup is stored.
        /// </summary>
        [JsonPropertyName("directoryName")]
        public string DirectoryName { get; set; } = directoryName;
    }
}
