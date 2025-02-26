using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackupEngine.Backup
{

    // La classe CryptStrategy implémente l'interface ITransferStrategy
    // Elle utilise un processus externe (CryptoSoft) pour effectuer des opérations de cryptage ou de décryptage
    internal class CryptStrategy : ITransferStrategy
    {
        // Méthode qui transfère un fichier en le cryptant ou décryptant via CryptoSoft
        public void TransferFile(string source, string destination)
        {
            // La variable 'encrypt' détermine si le fichier doit être crypté ou décrypté
            bool encrypt = true; // Ici, on définit 'true' pour indiquer qu'il s'agit d'un cryptage
            // Appel à la méthode LaunchCryptoSoft pour lancer le processus de cryptage
            LaunchCryptoSoft(source, destination, encrypt);
        }

        // Méthode statique qui lance le processus CryptoSoft pour crypter ou décrypter un fichier
        public static void LaunchCryptoSoft(string source, string destination, bool encrypt)
        {
            try
            {
                // Configuration des informations pour démarrer le processus externe (CryptoSoft)
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    // Spécifie le chemin de l'exécutable CryptoSoft
                    FileName = "C:\\Users\\Nino\\source\\repos\\EasySave\\CryptoSoft\\bin\\Debug\\net8.0\\CryptoSoft.exe",
                    // Arguments pour le programme : source, destination, et le mode (cryptage ou décryptage)
                    Arguments = $"\"{source}\" \"{destination}\" {encrypt}",
                    // Rediriger les sorties standard (utile pour lire les résultats ou erreurs)
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    // Ne pas utiliser le shell pour démarrer le processus
                    UseShellExecute = false,
                    // Créer sans fenêtre
                    CreateNoWindow = true
                };

                // Lancement du processus externe
                using (Process process = new Process { StartInfo = psi })
                {
                    // Démarre le processus
                    process.Start();

                    // Lire la sortie du programme (utile pour le débogage ou la journalisation)
                    string output = process.StandardOutput.ReadToEnd();
                    string error = process.StandardError.ReadToEnd();

                    // Attendre que le processus se termine
                    process.WaitForExit();

                    // (Commenté) Affichage de la sortie et du code de sortie pour le débogage
                    /*
                    Console.WriteLine($"CryptoSoft terminé avec code {process.ExitCode}");
                    Console.WriteLine($"Sortie : {output}");
                    */

                    // Si des erreurs sont présentes, on les affiche dans la console
                    if (!string.IsNullOrEmpty(error))
                    {
                        Console.WriteLine($"Erreurs : {error}");
                    }
                }
            }
            catch (Exception e)
            {
                // En cas d'exception, afficher l'erreur dans la console
                Console.WriteLine($"Erreur lors du lancement de CryptoSoft : {e.Message}");
            }
        }
    }
}

