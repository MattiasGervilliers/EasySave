using System;
using System.Text.Json.Serialization;

namespace LogLib
{
    // ConsoleLog inherits from Log and adds specific information related to backup operations
    public class ConsoleLog : Log
    {
        // Name of the backup associated with the log
        public string BackupName { get; set; }

        // Source path of the backed-up file
        public string SourcePath { get; set; }

        // Destination path where the file is backed up
        public string DestinationPath { get; set; }

        // File size in bytes
        public long FileSize { get; set; }

        // Transfer time in milliseconds
        public long TransferTimeMs { get; set; }

        // Constructor that initializes the properties with provided values
        public ConsoleLog(LogLevel level, string message, string backupName, string sourcePath, string destinationPath, long fileSize, long transferTimeMs)
            : base(level, message) // Calls the constructor of the parent class Log
        {
            BackupName = backupName;
            SourcePath = sourcePath;
            DestinationPath = destinationPath;
            FileSize = fileSize;
            TransferTimeMs = transferTimeMs;
        }
    }
}
