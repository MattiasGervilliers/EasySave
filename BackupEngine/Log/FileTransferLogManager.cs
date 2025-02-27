using LogLib;
using BackupEngine.Settings;

namespace BackupEngine.Log
{
    /// <summary>
    /// The FileTransferLogManager class is responsible for managing logs related to file transfers.
    /// It uses a LogWriter to record transfer events in log files.
    /// </summary>
    internal class FileTransferLogManager
    {
        /// <summary>
        /// Instance of LogWriter used to write logs into the specified log file.
        /// </summary>
        private readonly LogWriter _logWriter;

        /// <summary>
        /// Constructor of the FileTransferLogManager class.
        /// Initializes LogWriter with the specified log directory path and log type.
        /// </summary>
        /// <param name="logDirectoryPath">Path to the directory where logs will be stored.</param>
        /// <param name="logType">Log type (e.g., text file or other format).</param>
        public FileTransferLogManager(string logDirectoryPath, LogType logType)
        {
            /// <summary>
            /// Creates an instance of LogWriter by passing the log directory and log type.
            /// </summary>
            _logWriter = new LogWriter(logDirectoryPath, logType);
        }

        /// <summary>
        /// Method called when a file is transferred to log the details of the transfer.
        /// </summary>
        /// <param name="sender">The sender of the event (typically the object triggering the transfer).</param>
        /// <param name="transferEvent">The event containing information related to the transfer (e.g., paths, size, duration, etc.).</param>
        public void OnTransfer(object sender, TransferEvent transferEvent)
        {
            /// <summary>
            /// Retrieves the full path of the source file being transferred.
            /// </summary>
            string sourcePath = transferEvent.File.FullName;

            /// <summary>
            /// Retrieves the full path of the destination file of the transfer.
            /// </summary>
            string destinationPath = transferEvent.NewFile.FullName;

            /// <summary>
            /// Retrieves the size of the transferred file in bytes.
            /// </summary>
            long size = transferEvent.File.Length;

            /// <summary>
            /// Retrieves the transfer duration in milliseconds.
            /// </summary>
            int duration = transferEvent.TransferDuration.Milliseconds;

            /// <summary>
            /// Retrieves the name of the backup configuration associated with the transfer.
            /// </summary>
            string backupName = transferEvent.Configuration.Name;

            /// <summary>
            /// Creates a FileTransferLog object with the transfer details and writes the log to the file using LogWriter.
            /// </summary>
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
