using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackupEngine.Backup
{

    // The CryptStrategy class implements the ITransferStrategy interface
    // It uses an external process (CryptoSoft) to perform encryption or decryption operations
    internal class CryptStrategy : ITransferStrategy
    {
        // Method that transfers a file by encrypting or decrypting it via CryptoSoft
        public void TransferFile(string source, string destination)
        {
            // The 'encrypt' variable determines whether the file should be encrypted or decrypted
            bool encrypt = true; // Here, we set 'true' to indicate encryption
            // Call the LaunchCryptoSoft method to start the encryption process
            LaunchCryptoSoft(source, destination, encrypt);
        }

        // Static method that launches the CryptoSoft process to encrypt or decrypt a file
        public static void LaunchCryptoSoft(string source, string destination, bool encrypt)
        {
            try
            {
                // Set up the information to start the external process (CryptoSoft)
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    // Specifies the path to the CryptoSoft executable
                    FileName = "C:\\Users\\Nino\\source\\repos\\EasySave\\CryptoSoft\\bin\\Debug\\net8.0\\CryptoSoft.exe",
                    // Arguments for the program: source, destination, and the mode (encryption or decryption)
                    Arguments = $"\"{source}\" \"{destination}\" {encrypt}",
                    // Redirect standard output (useful for reading results or errors)
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    // Don't use the shell to start the process
                    UseShellExecute = false,
                    // Create without a window
                    CreateNoWindow = true
                };

                // Start the external process
                using (Process process = new Process { StartInfo = psi })
                {
                    // Start the process
                    process.Start();

                    // Read the output from the program (useful for debugging or logging)
                    string output = process.StandardOutput.ReadToEnd();
                    string error = process.StandardError.ReadToEnd();

                    // Wait for the process to exit
                    process.WaitForExit();

                    // (Commented) Display the output and exit code for debugging
                    /*
                    Console.WriteLine($"CryptoSoft finished with code {process.ExitCode}");
                    Console.WriteLine($"Output: {output}");
                    */

                    // If there are any errors, display them in the console
                    if (!string.IsNullOrEmpty(error))
                    {
                        Console.WriteLine($"Errors: {error}");
                    }
                }
            }
            catch (Exception e)
            {
                // In case of an exception, display the error in the console
                Console.WriteLine($"Error while launching CryptoSoft: {e.Message}");
            }
        }
    }
}
