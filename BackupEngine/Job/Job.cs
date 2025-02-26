using BackupEngine.Backup;

namespace BackupEngine.Job
{
    /// <summary>
    /// La classe Job est responsable de la gestion de la sauvegarde.
    /// Elle configure le type de sauvegarde à utiliser (complète ou différentielle) et lance la sauvegarde via le FileManager.
    /// </summary>
    public class Job
    {
        /// <summary>
        /// Propriétés privées :
        /// Configuration : Contient la configuration de la sauvegarde, spécifiant les détails comme le type de sauvegarde.
        /// FileManager : Permet de gérer la sauvegarde en fonction de la configuration.
        /// CryptStrategy : Stratégie de cryptage utilisée pour le transfert sécurisé des fichiers. Instanciée par défaut.
        /// </summary>
        private BackupConfiguration Configuration { get; set; }
        private FileManager FileManager { get; set; }
        private CryptStrategy _cryptStrategy = new CryptStrategy();

        /// <summary>
        /// Constructeur de la classe Job. Accepte une configuration de sauvegarde.
        /// Selon le type de sauvegarde spécifié dans la configuration, il initialise le FileManager avec la stratégie appropriée.
        /// </summary>
        public Job(BackupConfiguration configuration)
        {
            Configuration = configuration;

            /// <summary>
            /// Selon le type de sauvegarde, instancie un FileManager avec la bonne stratégie de sauvegarde.
            /// </summary>
            switch (Configuration.BackupType)
            {
                case BackupType.Full:
                    FileManager = new FileManager(new FullSaveStrategy(Configuration));
                    break;
                case BackupType.Differential:
                    FileManager = new FileManager(new DifferentialSaveStrategy(Configuration));
                    break;
                default:
                    /// <summary>
                    /// Si le type de sauvegarde est invalide, on lance une exception.
                    /// </summary>
                    throw new Exception("Invalid backup type");
            }
        }

        /// <summary>
        /// Méthode publique Run() qui déclenche la sauvegarde via le FileManager.
        /// </summary>
        public void Run()
        {
            /// <summary>
            /// Appelle la méthode Save du FileManager pour effectuer la sauvegarde avec la configuration fournie.
            /// </summary>
            FileManager.Save(Configuration);
        }
    }
}
