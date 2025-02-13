using LogLib;
using BackupEngine.Settings;

namespace BackupEngine.Log
{
    internal class FileTransferLogManager
    {
        private readonly LogWriter _logWriter;

        public FileTransferLogManager(string logDirectoryPath, LogType logType)
        {
            _logWriter = new LogWriter(logDirectoryPath, logType);
        }

        public void OnTransfer(object sender, TransferEvent transferEvent)
        {
            string sourcePath = transferEvent.File.FullName;
            string destinationPath = transferEvent.Configuration.DestinationPath.GetAbsolutePath() + "\\" + transferEvent.File.Name;
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
