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
        private static SecureString _key = GenerateKey();

        private static SecureString GenerateKey()
        {
            SecureString secureKey = new SecureString();
            Random random = new Random();
            for (int i = 0; i < 32; i++) // Clé de 32 caractères
            {
                secureKey.AppendChar((char)random.Next(33, 126)); // Caractères imprimables ASCII
            }
            secureKey.MakeReadOnly();
            return secureKey;
        }

        private static byte[] GetKeyBytes()
        {
            IntPtr keyPtr = Marshal.SecureStringToGlobalAllocUnicode(_key);
            try
            {
                string keyString = Marshal.PtrToStringUni(keyPtr);
                return Encoding.UTF8.GetBytes(keyString);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(keyPtr);
            }
        }

        public static void Encrypt(string source, string destination)
        {
            byte[] keyBytes = GetKeyBytes();
            int keyLength = keyBytes.Length;
            try
            {
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

            byte[] keyBytes = GetKeyBytes();
            int keyLength = keyBytes.Length;

            try
            {
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

                Console.WriteLine($"Fichier déchiffré avec succès : {destination}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Erreur lors du déchiffrement : {e.Message}");
            }
        }
    }
}
