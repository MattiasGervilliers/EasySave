using LogLib;

namespace BackupEngine.Log
{
    internal class FileTransferLogManager
    {
        private LogWriter _logWriter;

        public FileTransferLogManager(string logDirectoryPath)
        {
            _logWriter = new LogWriter(logDirectoryPath);
        }

        public void OnTransfer(object sender, TransferEvent transferEvent)
        {

        }
    }
}
