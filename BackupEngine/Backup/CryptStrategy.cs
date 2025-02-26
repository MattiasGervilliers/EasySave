using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace BackupEngine.Backup
{
    /// <summary>
    /// Strategy for encrypting files before transfer.
    /// </summary>
    internal class CryptStrategy : ITransferStrategy
    {
        /// <summary>
        /// Set of file extensions that need to be encrypted.
        /// </summary>
        private HashSet<string> _extensionsToCrypt;
        /// <summary>
        /// Initializes a new instance of the CryptStrategy class with specified extensions to encrypt.
        /// </summary>
        public CryptStrategy(HashSet<string> extensions, HashSet<string> extensionPriority)
        {
            _extensionsToCrypt = extensions ?? new HashSet<string>();
        }
        /// <summary>
        /// Transfers a file from source to destination, applying encryption if required.
        /// </summary>
        public void TransferFile(string source, string destination)
        {
            string extension = Path.GetExtension(source).ToLower();
            if (_extensionsToCrypt.Contains(extension))
            {
                LaunchCryptoSoft(source, destination);
            }
            else
            {
                File.Copy(source, destination, true);
            }  
        }
        
        public void UpdateKey(string key)
        {
            _key = new NetworkCredential("", key).SecurePassword;
        }
        
        internal string GetKeyToString()
        {
            string theString = new NetworkCredential("", _key).Password;
            return theString;
        }
        
        /// <summary>
        /// Launches the CryptoSoft executable to encrypt a file.
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
                    string output = process.StandardOutput.ReadToEnd();
                    string error = process.StandardError.ReadToEnd();
                    process.WaitForExit();

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
