using System;
using System.Diagnostics;
using System.Text;

namespace CryptoSoft
{
    /// <summary>
    /// The Program class is responsible for the main execution of the program. It uses the Encoder class to
    /// perform encryption operations based on command-line arguments.
    /// </summary>
    class Program
    {
        /// <summary>
        /// The main method of the program. It receives command-line arguments to decide whether to
        /// encrypt or decrypt a file and displays the elapsed time for the operation.
        /// </summary>
        /// <param name="args">Array of arguments passed in the command line.</param>
        /// <returns>Returns 0 on success, -1 on error.</returns>
        static int Main(string[] args)
        {
            int returnValue = 0;  // Default return code (success)
            Stopwatch stopWatch = new Stopwatch();  // Stopwatch to measure execution time
            stopWatch.Start();  // Starts the stopwatch

            try
            {
                // Checks if the third argument (args[2]) is "True" to decide whether to encrypt or decrypt
                if (args[2] == "True")
                {
                    // Calls the Encrypt method of the Encoder class to encrypt the file
                    Encoder.Encrypt(args[0], args[1]);
                }
                else
                {
                    // If it wasn't "True", the Decrypt method could be called here, but it is commented out
                    // Encoder.Decrypt(args[0]);
                }
            }
            catch (Exception e)
            {
                // In case of an exception (for example, if the arguments are wrong or the file is not found),
                // return -1 to signal an error
                return -1;
            }

            // Stops the stopwatch after the operation has been executed
            stopWatch.Stop();
            int elapsedTime = (int)stopWatch.ElapsedMilliseconds;  // Elapsed time in milliseconds
            Console.WriteLine(elapsedTime);  // Displays the execution time in the console
            return returnValue;  // Returns 0 to indicate successful execution
        }
    }
}
