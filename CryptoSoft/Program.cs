using System;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace CryptoSoft
{
    /// <summary>
    /// The Program class is responsible for the main execution of the program. It uses the Encoder class to
    /// perform encryption operations based on command-line arguments.
    /// </summary>
    class Program
    {
        private static Mutex _mutex = new Mutex(true, "Global\\CryptoSoftMutex");
        
        /// <summary>
        /// The main method of the program. It receives command-line arguments to decide whether to
        /// encrypt or decrypt a file and displays the elapsed time for the operation.
        /// </summary>
        /// <param name="args">Array of arguments passed in the command line.</param>
        /// <returns>Returns 0 on success, -1 on error.</returns>
        static int Main(string[] args)
        {
            // check if there is already a Cryptosoft instance 
            if (!_mutex.WaitOne(0, false))
            {
                Console.WriteLine("CryptoSoft est déjà en cours d'exécution.");
                return -1; 
            }

            int returnValue = 0;
            try
            {
                if (args[2] == "True")
                {
                    Encoder.Encrypt(args[0], args[1]);
                }
                else
                {
                    Console.ReadLine(); 
                }
            }
            catch (Exception)
            {
                return -1;
            }
            finally
            {
                _mutex.ReleaseMutex();
            }

            return returnValue;
        }
    }
}
