using System;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace CryptoSoft
{
    class Program
    {
        private static Mutex _mutex = new Mutex(true, "Global\\CryptoSoftMutex");

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
