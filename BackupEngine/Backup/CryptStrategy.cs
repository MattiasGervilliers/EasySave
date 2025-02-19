using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;

namespace BackupEngine.Backup
{
    internal class CryptStrategy : ITransferStrategy
    {
        private SecureString _key;
        private string _extensions;
        public CryptStrategy(string key, HashSet<string> extensions)
        {
            _key = new NetworkCredential("", key).SecurePassword;
            if (extensions == null)
            {
                _extensions =  "";
            }
            _extensions = string.Join(", ", extensions);
        }
        public void TransferFile(string source, string destination)
        {
            bool encrypt = true;
            LaunchCryptoSoft(source, destination,encrypt, GetKeyToString(),_extensions);

        }
        public void UpdateKey(string key)
        {
            _key = new NetworkCredential("", key).SecurePassword;
        }
        public void UpdateExtensions(string extensions)
        {
            this._extensions = extensions;
        }
        internal string GetKeyToString()
        {
            string theString = new NetworkCredential("", _key).Password;
            return theString;

        }
        public void LaunchCryptoSoft(string source, string destination, bool encrypt, String encryptionkey, string extensions)
        {
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = "\"C:\\Users\\Nino\\source\\repos\\EasySave\\CryptoSoft\\bin\\Debug\\net8.0\\CryptoSoft.exe\"",
                    Arguments = $"\"{source}\" \"{destination}\" {encrypt} \" {encryptionkey} \" {extensions}",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };
                Console.WriteLine(psi.Arguments);
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

    