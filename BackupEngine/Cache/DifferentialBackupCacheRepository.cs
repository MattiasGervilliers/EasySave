using Newtonsoft.Json;

namespace BackupEngine.Cache
{
    internal class DifferentialBackupCacheRepository
    {
        private const string CACHE_PATH = ".differentiel_cache.json";

        private readonly DifferentialBackupCache _cache;

        public DifferentialBackupCacheRepository()
        {
            _cache = Load();
        }

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

        private void CreateFile()
        {
            FileStream fs = File.Create(CACHE_PATH);
            fs.Close();
        }

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

        public CachedConfiguration? GetCachedConfiguration(BackupConfiguration configuration)
        {
            return _cache._configurations.Find(c => c.Name == configuration.Name);
        }

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
