using BackupEngine.Log;
using BackupEngine.State;

namespace BackupEngine.Backup
{
    public abstract class SaveStrategy(BackupConfiguration configuration)
    {
        public event EventHandler<TransferEvent> Transfer;
        public event EventHandler<StateEvent> StateUpdated;
        protected readonly BackupConfiguration _configuration = configuration;
        public ITransferStrategy TransferStrategy;

        public abstract void Save(string uniqueDestinationPath);

        protected void OnTransfer(TransferEvent e)
        {
            Transfer?.Invoke(this, e);
        }

        protected void OnStateUpdated(StateEvent state)
        {
            StateUpdated?.Invoke(this, state);
        }
    }
}