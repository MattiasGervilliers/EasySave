using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace CryptoSoft
{
    internal class Encoder
    {
        public static SecureString _key = new SecureString();
        public static string _binaryKey = "";
        public Encoder()
        {

        }
        public void UpdateKey(SecureString secureKey)
        {
            _key = secureKey;
        }
        public static void Encrypt(string source, string destination, string key)
        {

            //IntPtr keyPtr = Marshal.SecureStringToGlobalAllocUnicode(_key);
            //string keyString = Marshal.PtrToStringUni(keyPtr);

            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            int keyLength = keyBytes.Length;

            try
            {
                Console.WriteLine("EEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEE");
                using (FileStream sourceStream = new FileStream(source, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (FileStream destStream = new FileStream(destination, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    byte[] buffer = new byte[4096]; 
                    int bytesRead;
                    int keyIndex = 0;

                    while ((bytesRead = sourceStream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        for (int i = 0; i < bytesRead; i++)
                        {
                            buffer[i] ^= keyBytes[keyIndex % keyLength]; 
                            keyIndex++;
                        }

                        destStream.Write(buffer, 0, bytesRead);
                    }
                }

                Console.WriteLine($"Fichier chiffré avec succès : {destination}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Erreur lors du chiffrement : {e.Message}");
            }
        }

        

        public static void Decrypt(string source, string destination)
        {
            if (!File.Exists(source))
            {
                Console.WriteLine($"Erreur : le fichier source '{source}' n'existe pas.");
                return;
            }
            IntPtr keyPtr = Marshal.SecureStringToGlobalAllocUnicode(_key);
            string keyString = Marshal.PtrToStringUni(keyPtr);
            byte[] keyBytes = Encoding.UTF8.GetBytes(keyString);
            int keyLength = keyBytes.Length;

            try
            {
                using (FileStream sourceStream = new FileStream(source, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (FileStream destStream = new FileStream(destination, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    byte[] buffer = new byte[4096]; // Taille du buffer
                    int bytesRead;
                    int keyIndex = 0;

                    while ((bytesRead = sourceStream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        for (int i = 0; i < bytesRead; i++)
                        {
                            buffer[i] ^= keyBytes[keyIndex % keyLength]; 
                            keyIndex++;
                        }

                        destStream.Write(buffer, 0, bytesRead);
                    }
                }

                Console.WriteLine($"Fichier déchiffré avec succès : {destination}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Erreur lors du déchiffrement : {e.Message}");
            }
        }

    }
}

