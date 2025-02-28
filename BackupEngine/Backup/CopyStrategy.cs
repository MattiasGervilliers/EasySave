using BackupEngine.Log;
using BackupEngine.Shared;
using BackupEngine.Progress;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BackupEngine.Backup
{
    public class CopyStrategy : ITransferStrategy
    {
        public CopyStrategy() 
        {
        }
        public void TransferFile(string file, string destFile)
        {
            Console.WriteLine("Copie de " + file + " -> " + destFile);

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
