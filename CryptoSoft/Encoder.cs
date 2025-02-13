using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CryptoSoft
{
    internal class Encoder
    {
        public static string key = "lmskdjfglmsdj";
        public static string binaryKey = "";
        public Encoder()
        {

        }

        public static void Encrypt(string source, string destination)
        {
            if (!File.Exists(source))
            {
                Console.WriteLine($"Erreur : le fichier source '{source}' n'existe pas.");
                return;
            }

            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            int keyLength = keyBytes.Length;

            try
            {
                // Ouvrir les streams en mode lecture et écriture
                using (FileStream sourceStream = new FileStream(source, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (FileStream destStream = new FileStream(destination, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    byte[] buffer = new byte[4096]; // Taille de buffer pour lecture/écriture par blocs
                    int bytesRead;
                    int keyIndex = 0;

                    while ((bytesRead = sourceStream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        // Appliquer XOR pour chiffrer le buffer
                        for (int i = 0; i < bytesRead; i++)
                        {
                            buffer[i] ^= keyBytes[keyIndex % keyLength]; // XOR avec la clé
                            keyIndex++;
                        }

                        // Écrire les données chiffrées
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

            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            int keyLength = keyBytes.Length;

            try
            {
                // Ouvrir les fichiers en mode lecture et écriture
                using (FileStream sourceStream = new FileStream(source, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (FileStream destStream = new FileStream(destination, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    byte[] buffer = new byte[4096]; // Taille du buffer
                    int bytesRead;
                    int keyIndex = 0;

                    while ((bytesRead = sourceStream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        // Appliquer XOR pour déchiffrer le buffer
                        for (int i = 0; i < bytesRead; i++)
                        {
                            buffer[i] ^= keyBytes[keyIndex % keyLength]; // XOR avec la clé
                            keyIndex++;
                        }

                        // Écrire les données déchiffrées
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

