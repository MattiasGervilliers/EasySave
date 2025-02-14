using BackupEngine.Log;
using BackupEngine.Shared;
using BackupEngine.State;
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
            Console.WriteLine("initialisation de la copie");
        }
        public void TransferFile(string file, string destFile)
        {
            Console.WriteLine("transfert en cours ...");

            // Copy file using filestream to avoid file locking
            using (FileStream sourceStream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (FileStream destStream = new FileStream(destFile, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    sourceStream.CopyTo(destStream);
                }
            }
            Console.WriteLine("transfert fini");
        }

    }
}
