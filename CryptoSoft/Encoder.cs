using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace CryptoSoft
{
    /// <summary>
    /// The Encoder class is responsible for encoding and decoding files using an XOR method with a defined key.
    /// It allows for encryption and decryption of files through data streams.
    /// </summary>
    internal class Encoder
    {
        private static SecureString _key = GenerateKey();

        private static SecureString GenerateKey()
        {
            SecureString secureKey = new SecureString();
            Random random = new Random();
            for (int i = 0; i < 32; i++) 
            {
                secureKey.AppendChar((char)random.Next(33, 126)); 
            }
            secureKey.MakeReadOnly();
            return secureKey;
        }
        
        /// <summary>
        /// Constructor of the Encoder class. It initializes the class without any specific effect.
        /// </summary>
        public Encoder()
        {
            SecureString secureKey = new SecureString();
            Random random = new Random();
            for (int i = 0; i < 32; i++) 
            {
                secureKey.AppendChar((char)random.Next(33, 126)); 
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

        /// <summary>
        /// Static method to encrypt a file using the XOR method with a given key.
        /// The source file is read, the data is encrypted, and written to a destination file.
        /// </summary>
        /// <param name="source">The path of the source file to encrypt.</param>
        /// <param name="destination">The path of the destination file where the encrypted data will be written.</param>
        public static void Encrypt(string source, string destination)
        {
            byte[] keyBytes = GetKeyBytes();
            int keyLength = keyBytes.Length;
            try
            {
                using (FileStream sourceStream = new FileStream(source, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (FileStream destStream = new FileStream(destination, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    byte[] buffer = new byte[4096]; // lit les données par bloc 
                    int bytesRead; // bytes lis a chaque itération 
                    int keyIndex = 0;

                    while ((bytesRead = sourceStream.Read(buffer, 0, buffer.Length)) > 0) //lit les donnes par bloc de 4096 octet
                    {
                        for (int i = 0; i < bytesRead; i++)// pour chaque octet lu
                        {
                            buffer[i] ^= keyBytes[keyIndex % keyLength];// XOR avec la clé de chiffrement
                            keyIndex++;
                        }

                        destStream.Write(buffer, 0, bytesRead); // ecriture du bloc chiffré dans le fichier de destinantion
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
