using BackupEngine.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackupEngine.Backup
{
    internal interface ITransferStrategy
    {
        public void TrasferFile(Chemin cheminSource, Chemin cheminDestination)
        {

        }
    }
}
