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
        private string _extensionsToCrypt;
        private string _extensionsPriority;
        public CryptStrategy(HashSet<string> extensions, HashSet<string> ExtensionPriority)
        {
            //convert HashSet into string
            if (extensions == null)
            {
                _extensionsToCrypt =  "";
            }
            _extensionsToCrypt = string.Join(", ", extensions);
            if (ExtensionPriority == null)
            {
                _extensionsPriority = "";
            }
            _extensionsPriority = string.Join(", ", ExtensionPriority);
            //Console.WriteLine("voici la liste des priorité : " + _extensionsPriority);
        }
        public void TransferFile(string source, string destination)
        {
            //Console.WriteLine("début du transfert avec les priorités suivantes : "+ _extensionsPriority);
            bool encrypt = true;
            //Only launch cryptosoft if the source file extension is in configuration.extension hashset
            if (_extensionsToCrypt.Contains(Path.GetExtension(source).ToLower()))
            {
                LaunchCryptoSoft(source, destination, encrypt);
            }
        }
        public void UpdateExtensions(string extensions)
        {
            this._extensionsToCrypt = extensions;
        }
        public void LaunchCryptoSoft(string source, string destination, bool encrypt)
        {
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = "\"C:\\Users\\Nino\\source\\repos\\EasySave\\CryptoSoft\\bin\\Debug\\net8.0\\CryptoSoft.exe\"",
                    Arguments = $"\"{source}\" \"{destination}\" {encrypt}",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };
                Console.WriteLine("Appel de Cryptosoft avec les arguments suivants : "+psi.Arguments);
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

    