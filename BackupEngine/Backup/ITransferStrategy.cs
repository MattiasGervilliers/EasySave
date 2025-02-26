using BackupEngine.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackupEngine.Cache
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
