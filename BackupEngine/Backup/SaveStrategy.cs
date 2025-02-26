using BackupEngine.Log;
using BackupEngine.State;

namespace BackupEngine.Backup
{
    /// <summary>
    /// Abstract class SaveStrategy that defines the base for backup strategies (full, differential, etc.).
    /// It includes events to report the state of the backup and file transfers.
    /// </summary>
    public abstract class SaveStrategy
    {
        /// <summary>
        /// Events to notify transfer and state information during the backup.
        /// </summary>
        public event EventHandler<TransferEvent> Transfer;
        public event EventHandler<StateEvent> StateUpdated;

        /// <summary>
        /// Backup configuration, includes information such as source and destination paths, encryption options, etc.
        /// </summary>
        protected readonly BackupConfiguration Configuration;

        /// <summary>
        /// Transfer strategy used to perform the file transfer.
        /// </summary>
        public ITransferStrategy TransferStrategy;

        /// <summary>
        /// Abstract method for the backup. Each backup strategy must implement this method.
        /// </summary>
        public abstract void Save(string uniqueDestinationPath);

        /// <summary>
        /// Protected method to notify a transfer event.
        /// </summary>
        protected void onTransfer(TransferEvent e)
        {
            Transfer?.Invoke(this, e);
        }

        /// <summary>
        /// Protected method to notify a state update event.
        /// </summary>
        protected void onStateUpdated(StateEvent state)
        {
            StateUpdated?.Invoke(this, state);
        }
    }
}
