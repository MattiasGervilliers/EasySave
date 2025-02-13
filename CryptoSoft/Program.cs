using System;
using System.Diagnostics;
using System.Text;

namespace CryptoSoft
{
    class Program
    {

        static int Main(string[] args)
        {
            int returnValue = 0;
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            try
            {
                if (args[2] == "0")
                {
                    Encoder.Encrypt(args[0], args[1]);
                }
                else if (args[2] == "1")
                {
                    Encoder.Decrypt(args[0]);
                }

            }
            catch (Exception e)
            {
                return -1;
            }


            stopWatch.Stop();
            int elapsedTime = (int)stopWatch.ElapsedMilliseconds;
            Console.WriteLine(elapsedTime);
            return returnValue;
        }
    }
}
