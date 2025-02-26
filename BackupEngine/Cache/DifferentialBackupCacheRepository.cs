using Newtonsoft.Json;

namespace BackupEngine.Cache
{
    /// <summary>
    /// La classe DifferentialBackupCacheRepository est responsable de la gestion du cache pour les sauvegardes différentielles.
    /// Elle permet de charger, enregistrer, et mettre à jour le cache des sauvegardes différentielles.
    /// </summary>
    internal class DifferentialBackupCacheRepository
    {
        /// <summary>
        /// Chemin du fichier cache où les informations de sauvegarde sont stockées.
        /// </summary>
        private const string CACHE_PATH = ".differentiel_cache.json";

        /// <summary>
        /// Instance de DifferentialBackupCache qui contient les configurations des sauvegardes différentielles.
        /// </summary>
        private readonly DifferentialBackupCache _cache;

        /// <summary>
        /// Constructeur de la classe DifferentialBackupCacheRepository.
        /// Il charge les informations de cache à partir du fichier de cache ou crée un nouveau cache si le fichier n'existe pas.
        /// </summary>
        public DifferentialBackupCacheRepository()
        {
            /// <summary>
            /// Chargement du cache depuis le fichier ou création d'un nouveau cache vide.
            /// </summary>
            _cache = Load();
        }

        /// <summary>
        /// Méthode pour charger les données du cache à partir du fichier.
        /// Si le fichier de cache n'existe pas, elle crée un fichier vide et retourne un cache vide.
        /// </summary>
        /// <returns>Retourne une instance de DifferentialBackupCache contenant les données du cache.</returns>
        private DifferentialBackupCache Load()
        {
            /// <summary>
            /// Vérifie si le fichier de cache existe.
            /// </summary>
            if (File.Exists(CACHE_PATH))
            {
                DifferentialBackupCache cache = new DifferentialBackupCache();

                /// <summary>
                /// Si le fichier de cache existe, ouvre le fichier en lecture et charge les données JSON dans l'objet cache.
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
                /// Si le fichier de cache n'existe pas, crée un nouveau fichier de cache et retourne un cache vide.
                /// </summary>
                CreateFile();
                return new DifferentialBackupCache();
            }
        }

        /// <summary>
        /// Méthode pour créer un fichier vide de cache si celui-ci n'existe pas.
        /// </summary>
        private void CreateFile()
        {
            /// <summary>
            /// Crée le fichier de cache vide.
            /// </summary>
            FileStream fs = File.Create(CACHE_PATH);
            fs.Close();
        }

        /// <summary>
        /// Méthode pour sauvegarder le cache actuel dans le fichier de cache.
        /// Elle sérialise l'objet cache en JSON et l'écrit dans le fichier avec une indentation pour la lisibilité.
        /// </summary>
        private void Save()
        {
            /// <summary>
            /// Sérialise le cache en format JSON.
            /// </summary>
            string json = _cache.ToJson();

            /// <summary>
            /// Ouvre le fichier de cache en mode écriture et sauvegarde le JSON formaté.
            /// </summary>
            using (FileStream fs = File.Open(CACHE_PATH, FileMode.Open, FileAccess.Write, FileShare.None))
            using (StreamWriter sw = new StreamWriter(fs))
            {
                // Indente le JSON pour une meilleure lisibilité
                sw.Write(JsonConvert.SerializeObject(JsonConvert.DeserializeObject(json), Formatting.Indented));
            }
        }

        /// <summary>
        /// Recherche et retourne la configuration de sauvegarde mise en cache pour une configuration donnée.
        /// </summary>
        /// <param name="configuration">La configuration de sauvegarde pour laquelle nous cherchons une entrée dans le cache.</param>
        /// <returns>Retourne une instance de CachedConfiguration correspondant à la configuration donnée, ou null si elle n'existe pas.</returns>
        public CachedConfiguration? GetCachedConfiguration(BackupConfiguration configuration)
        {
            /// <summary>
            /// Recherche dans le cache la configuration correspondant au nom de la configuration donnée.
            /// </summary>
            return _cache._configurations.Find(c => c.Name == configuration.Name);
        }

        /// <summary>
        /// Ajoute une nouvelle sauvegarde dans le cache pour une configuration donnée.
        /// Si la configuration n'existe pas dans le cache, elle est ajoutée.
        /// </summary>
        /// <param name="configuration">La configuration de sauvegarde associée à la nouvelle sauvegarde.</param>
        /// <param name="date">La date et l'heure de la sauvegarde.</param>
        /// <param name="directoryName">Le nom du répertoire où la sauvegarde a été effectuée.</param>
        public void AddBackup(BackupConfiguration configuration, DateTime date, string directoryName)
        {
            /// <summary>
            /// Cherche la configuration dans le cache.
            /// </summary>
            CachedConfiguration? cachedConfiguration = GetCachedConfiguration(configuration);

            /// <summary>
            /// Si la configuration n'existe pas dans le cache, une nouvelle entrée est créée.
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
            /// Ajoute la nouvelle sauvegarde à la configuration existante.
            /// </summary>
            cachedConfiguration.Backups.Add(new Backup(date, directoryName));

            /// <summary>
            /// Sauvegarde le cache après l'ajout de la nouvelle sauvegarde.
            /// </summary>
            Save();
        }
    }
}
