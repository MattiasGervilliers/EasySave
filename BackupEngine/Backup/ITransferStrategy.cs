using BackupEngine.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackupEngine.Cache
{
    /// <summary>
    /// Interface qui définit la méthode pour transférer un fichier d'un chemin source vers un chemin de destination.
    /// </summary>
    public interface ITransferStrategy
    {
        /// <summary>
        /// Méthode pour transférer un fichier de la source vers la destination.
        /// </summary>
        public void TransferFile(string cheminSource, string cheminDestination);
    }
}
