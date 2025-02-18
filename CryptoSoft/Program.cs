using System;
using System.Diagnostics;
using System.Text;

namespace CryptoSoft
{
    class Program
    {

        static int Main(string[] args)
        {
            Console.WriteLine("EEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEE");

            int returnValue = 0;
            try
            {
                if (args[2] == "True")
                {

                    Encoder.Encrypt(args[0], args[1], args[3]);
                }
                else 
                {
                    //Encoder.Decrypt(args[0]);
                }

            }
            catch (Exception e)
            {
                return -1;
            }
            return returnValue;
        }
    }
}
