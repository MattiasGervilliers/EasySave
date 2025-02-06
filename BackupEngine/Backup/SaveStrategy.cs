using BackupEngine.Log;

namespace BackupEngine.Backup
{
    public abstract class SaveStrategy(BackupConfiguration configuration)
    {
        public event EventHandler<TransferEvent> Transfer;
        protected readonly BackupConfiguration Configuration = configuration;

        public abstract void Save(string uniqueDestinationPath);

        protected void OnTransfer(TransferEvent e)
        {
            Transfer?.Invoke(this, e);
        }
    }
}