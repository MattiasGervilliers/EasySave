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
            try
            {
                if (args[2] == "True")
                {

                    Encoder.Encrypt(args[0], args[1]);
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
