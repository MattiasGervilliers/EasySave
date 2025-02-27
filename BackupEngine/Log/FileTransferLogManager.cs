using LogLib;
using BackupEngine.Settings;

namespace BackupEngine.Log
{
    internal class FileTransferLogManager
    {
        /// <summary>
        /// The log writer responsible for writing transfer logs to the specified log directory.
        /// </summary>
        private readonly LogWriter _logWriter;
        /// <summary>
        /// Initializes a new instance of the FileTransferLogManager class.
        /// </summary>
        /// <param name="logDirectoryPath">The directory where logs should be stored.</param>
        /// <param name="logType">The format type of the logs.</param>
        public FileTransferLogManager(string logDirectoryPath, LogType logType)
        {
            _logWriter = new LogWriter(logDirectoryPath, logType);
        }
        /// <summary>
        /// Handles the transfer event by logging details about the transferred file.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="transferEvent">The event containing transfer details.</param>
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
