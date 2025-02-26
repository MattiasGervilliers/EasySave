using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CryptoSoft
{
    /// <summary>
    /// La classe Encoder est responsable de l'encodage et du décodage des fichiers en utilisant une méthode XOR avec une clé définie.
    /// Elle permet de chiffrer et déchiffrer des fichiers via des flux de données.
    /// </summary>
    internal class Encoder
    {
        /// <summary>
        /// La clé utilisée pour l'encryptage et le décryptage des fichiers.
        /// </summary>
        public static string key = "azertyuiopaze";

        /// <summary>
        /// La clé binaire, initialement vide, destinée à contenir la version binaire de la clé (non utilisée dans ce code).
        /// </summary>
        public static string binaryKey = "";

        /// <summary>
        /// Constructeur de la classe Encoder. Il initialise la classe sans effet spécifique.
        /// </summary>
        public Encoder()
        {
        }

        /// <summary>
        /// Méthode statique permettant de chiffrer un fichier en utilisant la méthode XOR avec une clé donnée.
        /// Le fichier source est lu, les données sont chiffrées, et écrites dans un fichier de destination.
        /// </summary>
        /// <param name="source">Le chemin du fichier source à chiffrer.</param>
        /// <param name="destination">Le chemin du fichier de destination où les données chiffrées seront écrites.</param>
        public static void Encrypt(string source, string destination)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(key); // Conversion de la clé en tableau d'octets
            int keyLength = keyBytes.Length; // Longueur de la clé

            try
            {
                // Ouvrir les streams en mode lecture (source) et écriture (destination)
                using (FileStream sourceStream = new FileStream(source, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (FileStream destStream = new FileStream(destination, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    byte[] buffer = new byte[4096]; // Taille de buffer pour lecture/écriture par blocs
                    int bytesRead;
                    int keyIndex = 0;

                    // Lecture du fichier source par blocs
                    while ((bytesRead = sourceStream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        // Appliquer XOR sur chaque byte du buffer avec la clé
                        for (int i = 0; i < bytesRead; i++)
                        {
                            buffer[i] ^= keyBytes[keyIndex % keyLength]; // XOR avec la clé
                            keyIndex++; // Incrémenter l'indice de la clé pour les prochains bytes
                        }

                        // Écrire les données chiffrées dans le fichier de destination
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

        /// <summary>
        /// Méthode statique permettant de chiffrer les données d'un fichier à partir de flux de fichiers source et destination.
        /// Cette méthode fonctionne avec des FileStream ouverts pour effectuer un chiffrement en temps réel.
        /// </summary>
        /// <param name="sourceStream">Le flux de lecture du fichier source à chiffrer.</param>
        /// <param name="destStream">Le flux d'écriture pour enregistrer les données chiffrées.</param>
        public static void EEncrypt(FileStream sourceStream, FileStream destStream)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(key); // Conversion de la clé en tableau d'octets
            int keyLength = keyBytes.Length; // Longueur de la clé

            try
            {
                // Ouvrir les streams en mode lecture (source) et écriture (destination)
                using (sourceStream)
                using (destStream)
                {
                    byte[] buffer = new byte[4096]; // Taille de buffer pour lecture/écriture par blocs
                    int bytesRead;
                    int keyIndex = 0;

                    // Lecture du fichier source par blocs
                    while ((bytesRead = sourceStream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        // Appliquer XOR sur chaque byte du buffer avec la clé
                        for (int i = 0; i < bytesRead; i++)
                        {
                            buffer[i] ^= keyBytes[keyIndex % keyLength]; // XOR avec la clé
                            keyIndex++; // Incrémenter l'indice de la clé pour les prochains bytes
                        }

                        // Écrire les données chiffrées dans le flux de destination
                        destStream.Write(buffer, 0, bytesRead);
                    }
                }

                Console.WriteLine($"Fichier chiffré avec succès ");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Erreur lors du chiffrement : {e.Message}");
            }
        }

        /// <summary>
        /// Méthode statique permettant de déchiffrer un fichier en utilisant la méthode XOR avec la même clé.
        /// Le fichier source est lu, les données sont déchiffrées, et écrites dans un fichier de destination.
        /// </summary>
        /// <param name="source">Le chemin du fichier source à déchiffrer.</param>
        /// <param name="destination">Le chemin du fichier de destination où les données déchiffrées seront écrites.</param>
        public static void Decrypt(string source, string destination)
        {
            if (!File.Exists(source))
            {
                Console.WriteLine($"Erreur : le fichier source '{source}' n'existe pas.");
                return;
            }

            byte[] keyBytes = Encoding.UTF8.GetBytes(key); // Conversion de la clé en tableau d'octets
            int keyLength = keyBytes.Length; // Longueur de la clé

            try
            {
                // Ouvrir les streams en mode lecture (source) et écriture (destination)
                using (FileStream sourceStream = new FileStream(source, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (FileStream destStream = new FileStream(destination, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    byte[] buffer = new byte[4096]; // Taille de buffer pour lecture/écriture par blocs
                    int bytesRead;
                    int keyIndex = 0;

                    // Lecture du fichier source par blocs
                    while ((bytesRead = sourceStream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        // Appliquer XOR sur chaque byte du buffer avec la clé pour déchiffrer
                        for (int i = 0; i < bytesRead; i++)
                        {
                            buffer[i] ^= keyBytes[keyIndex % keyLength]; // XOR avec la clé
                            keyIndex++; // Incrémenter l'indice de la clé pour les prochains bytes
                        }

                        // Écrire les données déchiffrées dans le fichier de destination
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
