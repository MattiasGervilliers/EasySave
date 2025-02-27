using Newtonsoft.Json;

namespace BackupEngine.Cache
{/// <summary>
/// Repository responsible for managing the differential backup cache.
/// </summary>
    internal class DifferentialBackupCacheRepository
    {
        /// <summary>
        /// The file path where the differential backup cache is stored.
        /// </summary>
        private const string CACHE_PATH = ".differentiel_cache.json";
        /// <summary>
        /// The in-memory cache of differential backup configurations.
        /// </summary>
        private readonly DifferentialBackupCache _cache;
        /// <summary>
        /// Initializes a new instance of the DifferentialBackupCacheRepository class and loads the cache.
        /// </summary>
        public DifferentialBackupCacheRepository()
        {
            _cache = Load();
        }
        /// <summary>
        /// Loads the differential backup cache from the file system.
        /// </summary>
        /// <returns>A DifferentialBackupCache instance containing cached backup data.</returns>
        private DifferentialBackupCache Load()
        {
            if (File.Exists(CACHE_PATH))
            {
                DifferentialBackupCache cache = new DifferentialBackupCache();

                using (FileStream fs = File.Open(CACHE_PATH, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    using (StreamReader sr = new StreamReader(fs))
                    {
                        cache.FromJson(sr.ReadToEnd());
                    }
                }
                return cache;
            }
            else
            {
                CreateFile();
                return new DifferentialBackupCache();
            }
        }
        /// <summary>
        /// Creates a new cache file if it does not exist.
        /// </summary>
        private void CreateFile()
        {
            FileStream fs = File.Create(CACHE_PATH);
            fs.Close();
        }
        /// <summary>
        /// Saves the current state of the differential backup cache to the file system.
        /// </summary>
        private void Save()
        {
            string json = _cache.ToJson();
            using (FileStream fs = File.Open(CACHE_PATH, FileMode.Open, FileAccess.Write, FileShare.ReadWrite))
            using (StreamWriter sw = new StreamWriter(fs))
            {
                // indent the json for better readability
                sw.Write(JsonConvert.SerializeObject(JsonConvert.DeserializeObject(json), Formatting.Indented));
            }
        }
        /// <summary>
        /// Retrieves the cached configuration for a given backup configuration.
        /// </summary>
        /// <param name="configuration">The backup configuration to look for.</param>
        /// <returns>The cached configuration if found, otherwise null.</returns>
        public CachedConfiguration? GetCachedConfiguration(BackupConfiguration configuration)
        {
            return _cache._configurations.Find(c => c.Name == configuration.Name);
        }
        /// <summary>
        /// Adds a new backup entry to the cache for a given backup configuration.
        /// </summary>
        /// <param name="configuration">The backup configuration associated with the backup.</param>
        /// <param name="date">The date and time of the backup.</param>
        /// <param name="directoryName">The name of the directory where the backup is stored.</param>
        public void AddBackup(BackupConfiguration configuration, DateTime date, string directoryName)
        {
            CachedConfiguration? cachedConfiguration = GetCachedConfiguration(configuration);
            if (cachedConfiguration == null)
            {
                cachedConfiguration = new CachedConfiguration
                {
                    Name = configuration.Name,
                    Backups = new List<Backup>()
                };
                _cache._configurations.Add(cachedConfiguration);
            }
            cachedConfiguration.Backups.Add(new Backup(date, directoryName));
            Save();
        }
    }
}
