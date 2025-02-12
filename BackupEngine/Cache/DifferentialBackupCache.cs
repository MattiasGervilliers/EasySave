using BackupEngine.Shared;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace BackupEngine.Cache
{
    internal class DifferentialBackupCache : IJsonSerializable
    {
        public List<CachedConfiguration> _configurations { get; set; }

        public DifferentialBackupCache()
        {
            _configurations = new List<CachedConfiguration>();
        }

        public void FromJson(string json)
        {
            _configurations = JsonConvert.DeserializeObject<List<CachedConfiguration>>(json) ?? new List<CachedConfiguration>();
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(_configurations);
        }
    }

    internal class CachedConfiguration
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("backups")]
        public List<Backup> Backups { get; set; }
    }


    public class Backup (DateTime date, string directoryName)
    {
        [JsonPropertyName("date")]
        public DateTime Date { get; set; } = date;

        [JsonPropertyName("directoryName")]
        public string DirectoryName { get; set; } = directoryName;
    }
}
