using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace BackupEngine.Backup
{
    internal class CryptStrategy : ITransferStrategy
    {
        private SecureString _key;
        public CryptStrategy(string key)
        {
            _key = new NetworkCredential("", key).SecurePassword;
        }
        public void TransferFile(string source, string destination)
        {
            bool encrypt = true;
            LaunchCryptoSoft(source, destination,encrypt, GetKeyToString());
            
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
        public void LaunchCryptoSoft(string source, string destination, bool encrypt, String encryptionkey)
        {
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = "\"C:\\Users\\Nino\\source\\repos\\EasySave\\CryptoSoft\\bin\\Debug\\net8.0\\CryptoSoft.exe\"",
                    Arguments = $"\"{source}\" \"{destination}\" {encrypt} \" {encryptionkey}",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (Process process = new Process { StartInfo = psi })
                {
                    process.Start();

                    string output = process.StandardOutput.ReadToEnd();
                    string error = process.StandardError.ReadToEnd();

                    process.WaitForExit(); 
                    if (!string.IsNullOrEmpty(error))
                    {
                        Console.WriteLine($"Erreurs : {error}");
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

    