using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace BackupEngine.Backup
{
    /// <summary>
    /// Stratégie de chiffrement utilisée pour transférer les fichiers en appliquant un chiffrement.
    /// </summary>
    internal class CryptStrategy : ITransferStrategy
    {
        private HashSet<string> _extensionsToCrypt;
        private CancellationToken _cancellationToken;

        /// <summary>
        /// Constructeur prenant en paramètres les extensions à chiffrer et le CancellationToken.
        /// </summary>
        public CryptStrategy(HashSet<string> extensions, HashSet<string> extensionPriority, CancellationToken cancellationToken)
        {
            _extensionsToCrypt = extensions ?? new HashSet<string>();
            _cancellationToken = cancellationToken;
        }

        /// <summary>
        /// Transfère un fichier du source vers la destination en appliquant le chiffrement si nécessaire.
        /// </summary>
        public void TransferFile(string source, string destination)
        {
            string extension = Path.GetExtension(source).ToLower();
            if (_extensionsToCrypt.Contains(extension))
            {
                // Vérification du token avant de lancer CryptoSoft
                if (_cancellationToken.IsCancellationRequested)
                {
                    Console.WriteLine("Opération de chiffrement annulée avant lancement.");
                    return;
                }
                LaunchCryptoSoft(source, destination);
            }
            else
            {
                Console.WriteLine("Copie de " + source + " -> " + destination);

                File.Copy(source, destination, true);
            }
        }

        /// <summary>
        /// Lance l'exécutable CryptoSoft pour chiffrer un fichier.
        /// </summary>
        private void LaunchCryptoSoft(string source, string destination)
        {
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = "CryptoSoft.exe",
                    Arguments = $"\"{source}\" \"{destination}\" True",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                Console.WriteLine($"Lancement de CryptoSoft avec : {psi.Arguments}");
                using (Process process = new Process { StartInfo = psi })
                {
                    process.Start();

                    // Attente active avec vérification du token toutes les 100 ms
                    while (!process.WaitForExit(100))
                    {
                        if (_cancellationToken.IsCancellationRequested)
                        {
                            try
                            {
                                process.Kill();
                                Console.WriteLine("CryptoSoft annulé.");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Erreur lors de l'annulation de CryptoSoft : {ex.Message}");
                            }
                            return;
                        }
                    }

                    string output = process.StandardOutput.ReadToEnd();
                    string error = process.StandardError.ReadToEnd();

                    if (!string.IsNullOrEmpty(error))
                    {
                        Console.WriteLine($"Erreur CryptoSoft : {error}");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Erreur lors du lancement de CryptoSoft : {e.Message}");
            }
        }
    }
}
