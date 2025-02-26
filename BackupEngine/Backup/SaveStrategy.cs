using BackupEngine.Log;
using BackupEngine.State;

namespace BackupEngine.Backup
{
    /// <summary>
    /// Classe abstraite SaveStrategy qui définit la base pour les stratégies de sauvegarde (complète, différentielle, etc.).
    /// Elle inclut des événements pour signaler l'état de la sauvegarde et les transferts de fichiers.
    /// </summary>
    public abstract class SaveStrategy
    {
        /// <summary>
        /// Événements pour notifier les informations de transfert et d'état pendant la sauvegarde.
        /// </summary>
        public event EventHandler<TransferEvent> Transfer;
        public event EventHandler<StateEvent> StateUpdated;

        /// <summary>
        /// Configuration de la sauvegarde, inclut des informations comme le chemin source et destination, les options de cryptage, etc.
        /// </summary>
        protected readonly BackupConfiguration Configuration;

        /// <summary>
        /// Stratégie de transfert utilisée pour effectuer le transfert des fichiers.
        /// </summary>
        public ITransferStrategy TransferStrategy;

        /// <summary>
        /// Méthode abstraite pour la sauvegarde. Chaque stratégie de sauvegarde doit implémenter cette méthode.
        /// </summary>
        public abstract void Save(string uniqueDestinationPath);

        /// <summary>
        /// Méthode protégée pour notifier un événement de transfert.
        /// </summary>
        protected void onTransfer(TransferEvent e)
        {
            Transfer?.Invoke(this, e);
        }

        /// <summary>
        /// Méthode protégée pour notifier un événement de mise à jour d'état.
        /// </summary>
        protected void onStateUpdated(StateEvent state)
        {
            StateUpdated?.Invoke(this, state);
        }
    }
}
