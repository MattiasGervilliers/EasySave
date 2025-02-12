using LogLib;

namespace BackupEngine.Log
{
    internal class FileTransferLogManager(string logDirectoryPath)
    {
        private readonly LogWriter _logWriter = new LogWriter(logDirectoryPath);

        public void OnTransfer(object sender, TransferEvent transferEvent)
        {
            string sourcePath = transferEvent.File.FullName;
            string destinationPath = transferEvent.NewFile.FullName;
            long size = transferEvent.File.Length;
            int duration = transferEvent.TransferDuration.Milliseconds;
            string backupName = transferEvent.Configuration.Name;

            _logWriter.WriteLog(new FileTransferLog(
                    sourcePath,
                    destinationPath,
                    size,
                    duration,
                    backupName
                )
            );
        }
    }
}