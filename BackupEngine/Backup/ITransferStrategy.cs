using BackupEngine.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackupEngine.Backup
{
    public interface ITransferStrategy
    {
        public void TransferFile(string cheminSource, string cheminDestination);
    }
}
