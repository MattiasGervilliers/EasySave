namespace BackupEngine.Backup
{
    /// <summary>
    /// Interface that defines the method for transferring a file from a source path to a destination path.
    /// </summary>
    public interface ITransferStrategy
    {
        /// <summary>
        /// Method to transfer a file from the source to the destination.
        /// </summary>
        public void TransferFile(string sourcePath, string destinationPath);
    }
}
