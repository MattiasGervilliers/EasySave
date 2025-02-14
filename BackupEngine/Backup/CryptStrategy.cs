using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackupEngine.Backup
{
    internal class CryptStrategy : ITransferStrategy
    {

        public void TransferFile(string source, string destination)
        {
            bool encrypt = true;
            LaunchCryptoSoft(source, destination,encrypt);
            
        }
        public static void LaunchCryptoSoft(string source, string destination, bool encrypt)
        {
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = "C:\\Users\\Nino\\source\\repos\\EasySave\\CryptoSoft\\bin\\Debug\\net8.0\\CryptoSoft.exe",
                    Arguments = $"\"{source}\" \"{destination}\" {encrypt}",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (Process process = new Process { StartInfo = psi })
                {
                    Console.WriteLine(psi.Arguments);
                    process.Start();

                    // Lire la sortie du programme (utile pour debug)
                    string output = process.StandardOutput.ReadToEnd();
                    string error = process.StandardError.ReadToEnd();

                    process.WaitForExit(); // Attendre la fin du processus

                    Console.WriteLine($"CryptoSoft terminé avec code {process.ExitCode}");
                    Console.WriteLine($"Sortie : {output}");

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

    