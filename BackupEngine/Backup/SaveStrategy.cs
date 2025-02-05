using BackupEngine.Log;

namespace BackupEngine.Backup
{
    public abstract class SaveStrategy
    {
        public event EventHandler<TransferEvent> Transfer;
        public abstract void Save(string sourcePath, string destinationPath);

        protected void OnTransfer(TransferEvent e)
        {
            Transfer?.Invoke(this, e);
        }
    }
}