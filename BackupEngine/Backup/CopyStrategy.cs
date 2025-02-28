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
    /// <summary>
    /// The CopyStrategy class implements the ITransferStrategy interface.
    /// It defines the method to transfer files by copying them without file locking, using FileStream.
    /// </summary>
    public class CopyStrategy : ITransferStrategy
    {
        /// <summary>
        /// Constructor of the CopyStrategy class.
        /// It initializes the CopyStrategy object without specific parameters.
        /// </summary>
        public CopyStrategy()
        {
        }

        /// <summary>
        /// Method that performs the transfer of a source file to a destination file.
        /// It uses a FileStream to avoid locking the source file.
        /// </summary>
        /// <param name="file">The path of the source file to transfer.</param>
        /// <param name="destFile">The path of the destination file where the file will be copied.</param>
        public void TransferFile(string file, string destFile)
        {
            /// <summary>
            /// Opens a read stream for the source file in FileAccess.Read and FileShare.ReadWrite mode
            /// to allow reading access to the source file without locking it.
            /// </summary>
            Console.WriteLine(file + " -> " + destFile);
            using (FileStream sourceStream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                /// <summary>
                /// Opens a write stream for the destination file in FileMode.Create to create a new file.
                /// FileAccess.Write is used for writing, and FileShare.None is used to prevent concurrent access to the destination file.
                /// </summary>
                using (FileStream destStream = new FileStream(destFile, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    /// <summary>
                    /// Copies the content from the source file to the destination file using the CopyTo method.
                    /// </summary>
                    sourceStream.CopyTo(destStream);
                }
            }
        }
    }
}
