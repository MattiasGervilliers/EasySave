using BackupEngine.Shared;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace BackupEngine.Cache
{
    /// <summary>
    /// Classe représentant un cache de sauvegardes différentielles.
    /// Permet de sérialiser et désérialiser les configurations de sauvegarde depuis/vers un format JSON.
    /// </summary>
    internal class DifferentialBackupCache : IJsonSerializable
    {
        /// <summary>
        /// Liste des configurations de sauvegarde mises en cache.
        /// </summary>
        public List<CachedConfiguration> _configurations { get; set; }

        /// <summary>
        /// Constructeur par défaut initialisant la liste de configurations.
        /// </summary>
        public DifferentialBackupCache()
        {
            _configurations = new List<CachedConfiguration>();
        }

        /// <summary>
        /// Méthode pour désérialiser les données JSON et peupler la liste des configurations.
        /// </summary>
        public void FromJson(string json)
        {
            _configurations = JsonConvert.DeserializeObject<List<CachedConfiguration>>(json) ?? new List<CachedConfiguration>();
        }

        /// <summary>
        /// Méthode pour sérialiser la liste des configurations en format JSON.
        /// </summary>
        public string ToJson()
        {
            return JsonConvert.SerializeObject(_configurations);
        }
    }

    /// <summary>
    /// Classe représentant une configuration de sauvegarde mise en cache.
    /// </summary>
    internal class CachedConfiguration
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("backups")]
        public List<Backup> Backups { get; set; }
    }

    /// <summary>
    /// Classe représentant une sauvegarde spécifique avec la date et le nom du répertoire.
    /// </summary>
    public class Backup
    {
        [JsonPropertyName("date")]
        public DateTime Date { get; set; }

        [JsonPropertyName("directoryName")]
        public string DirectoryName { get; set; }
    }
}
