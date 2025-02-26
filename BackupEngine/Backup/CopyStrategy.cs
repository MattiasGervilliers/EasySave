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
    /// <summary>
    /// La classe CopyStrategy implémente l'interface ITransferStrategy.
    /// Elle définit la méthode pour transférer des fichiers en les copiant, sans verrouillage de fichier, à l'aide de FileStream.
    /// </summary>
    public class CopyStrategy : ITransferStrategy
    {
        /// <summary>
        /// Constructeur de la classe CopyStrategy.
        /// Il initialise l'objet CopyStrategy sans paramètres spécifiques.
        /// </summary>
        public CopyStrategy()
        {
        }

        /// <summary>
        /// Méthode qui effectue le transfert d'un fichier source vers un fichier de destination.
        /// Utilise un FileStream pour éviter les blocages sur le fichier source.
        /// </summary>
        /// <param name="file">Le chemin du fichier source à transférer.</param>
        /// <param name="destFile">Le chemin du fichier de destination où le fichier sera copié.</param>
        public void TransferFile(string file, string destFile)
        {
            /// <summary>
            /// Ouvre un flux de lecture pour le fichier source en mode FileAccess.Read et FileShare.ReadWrite
            /// afin de permettre l'accès en lecture au fichier source sans le verrouiller.
            /// </summary>
            using (FileStream sourceStream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                /// <summary>
                /// Ouvre un flux d'écriture pour le fichier de destination en mode FileMode.Create pour créer un nouveau fichier.
                /// FileAccess.Write est utilisé pour l'écriture et FileShare.None pour éviter les accès concurrents au fichier de destination.
                /// </summary>
                using (FileStream destStream = new FileStream(destFile, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    /// <summary>
                    /// Copie le contenu du fichier source dans le fichier de destination à l'aide de la méthode CopyTo.
                    /// </summary>
                    sourceStream.CopyTo(destStream);
                }
            }
        }
    }
}
