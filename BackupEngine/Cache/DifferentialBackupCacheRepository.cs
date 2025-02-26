using Newtonsoft.Json;

namespace BackupEngine.Cache
{
    /// <summary>
    /// The DifferentialBackupCacheRepository class is responsible for managing the cache for differential backups.
    /// It allows loading, saving, and updating the cache of differential backups.
    /// </summary>
    internal class DifferentialBackupCacheRepository
    {
        /// <summary>
        /// Path to the cache file where backup information is stored.
        /// </summary>
        private const string CACHE_PATH = ".differentiel_cache.json";

        /// <summary>
        /// Instance of DifferentialBackupCache that contains the configurations for differential backups.
        /// </summary>
        private readonly DifferentialBackupCache _cache;

        /// <summary>
        /// Constructor of the DifferentialBackupCacheRepository class.
        /// It loads cache information from the cache file or creates a new cache if the file does not exist.
        /// </summary>
        public DifferentialBackupCacheRepository()
        {
            /// <summary>
            /// Loading the cache from the file or creating a new empty cache.
            /// </summary>
            _cache = Load();
        }

        /// <summary>
        /// Method to load cache data from the file.
        /// If the cache file does not exist, it creates an empty file and returns an empty cache.
        /// </summary>
        /// <returns>Returns an instance of DifferentialBackupCache containing the cache data.</returns>
        private DifferentialBackupCache Load()
        {
            /// <summary>
            /// Checks if the cache file exists.
            /// </summary>
            if (File.Exists(CACHE_PATH))
            {
                DifferentialBackupCache cache = new DifferentialBackupCache();

                /// <summary>
                /// If the cache file exists, opens the file for reading and loads the JSON data into the cache object.
                /// </summary>
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
                /// <summary>
                /// If the cache file does not exist, creates a new cache file and returns an empty cache.
                /// </summary>
                CreateFile();
                return new DifferentialBackupCache();
            }
        }

        /// <summary>
        /// Method to create an empty cache file if it does not exist.
        /// </summary>
        private void CreateFile()
        {
            /// <summary>
            /// Creates the empty cache file.
            /// </summary>
            FileStream fs = File.Create(CACHE_PATH);
            fs.Close();
        }

        /// <summary>
        /// Method to save the current cache to the cache file.
        /// It serializes the cache object to JSON and writes it to the file with indentation for readability.
        /// </summary>
        private void Save()
        {
            /// <summary>
            /// Serializes the cache to JSON format.
            /// </summary>
            string json = _cache.ToJson();

            /// <summary>
            /// Opens the cache file for writing and saves the formatted JSON.
            /// </summary>
            using (FileStream fs = File.Open(CACHE_PATH, FileMode.Open, FileAccess.Write, FileShare.None))
            using (StreamWriter sw = new StreamWriter(fs))
            {
                // Indents the JSON for better readability
                sw.Write(JsonConvert.SerializeObject(JsonConvert.DeserializeObject(json), Formatting.Indented));
            }
        }

        /// <summary>
        /// Searches and returns the cached backup configuration for a given configuration.
        /// </summary>
        /// <param name="configuration">The backup configuration for which we are looking for an entry in the cache.</param>
        /// <returns>Returns an instance of CachedConfiguration corresponding to the given configuration, or null if it does not exist.</returns>
        public CachedConfiguration? GetCachedConfiguration(BackupConfiguration configuration)
        {
            /// <summary>
            /// Searches the cache for the configuration matching the given configuration name.
            /// </summary>
            return _cache._configurations.Find(c => c.Name == configuration.Name);
        }

        /// <summary>
        /// Adds a new backup to the cache for a given configuration.
        /// If the configuration does not exist in the cache, it is added.
        /// </summary>
        /// <param name="configuration">The backup configuration associated with the new backup.</param>
        /// <param name="date">The date and time of the backup.</param>
        /// <param name="directoryName">The name of the directory where the backup was performed.</param>
        public void AddBackup(BackupConfiguration configuration, DateTime date, string directoryName)
        {
            /// <summary>
            /// Searches for the configuration in the cache.
            /// </summary>
            CachedConfiguration? cachedConfiguration = GetCachedConfiguration(configuration);

            /// <summary>
            /// If the configuration does not exist in the cache, a new entry is created.
            /// </summary>
            if (cachedConfiguration == null)
            {
                cachedConfiguration = new CachedConfiguration
                {
                    Name = configuration.Name,
                    Backups = new List<Backup>()
                };
                _cache._configurations.Add(cachedConfiguration);
            }

            /// <summary>
            /// Adds the new backup to the existing configuration.
            /// </summary>
            cachedConfiguration.Backups.Add(new Backup(date, directoryName));

            /// <summary>
            /// Saves the cache after adding the new backup.
            /// </summary>
            Save();
        }
    }
}
