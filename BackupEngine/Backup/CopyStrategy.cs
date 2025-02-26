namespace BackupEngine.Backup
{
    public class CopyStrategy : ITransferStrategy
    {
        public CopyStrategy() 
        {
        }
        public void TransferFile(string file, string destFile)
        {

            // Copy file using filestream to avoid file locking
            using (FileStream sourceStream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (FileStream destStream = new FileStream(destFile, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    sourceStream.CopyTo(destStream);
                }
            }
        }

    }
}
