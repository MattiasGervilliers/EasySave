using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CryptoSoft
{
    /// <summary>
    /// The Encoder class is responsible for encoding and decoding files using an XOR method with a defined key.
    /// It allows for encryption and decryption of files through data streams.
    /// </summary>
    internal class Encoder
    {
        /// <summary>
        /// The key used for encrypting and decrypting files.
        /// </summary>
        public static string key = "azertyuiopaze";

        /// <summary>
        /// The binary key, initially empty, meant to contain the binary version of the key (not used in this code).
        /// </summary>
        public static string binaryKey = "";

        /// <summary>
        /// Constructor of the Encoder class. It initializes the class without any specific effect.
        /// </summary>
        public Encoder()
        {
        }

        /// <summary>
        /// Static method to encrypt a file using the XOR method with a given key.
        /// The source file is read, the data is encrypted, and written to a destination file.
        /// </summary>
        /// <param name="source">The path of the source file to encrypt.</param>
        /// <param name="destination">The path of the destination file where the encrypted data will be written.</param>
        public static void Encrypt(string source, string destination)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(key); // Convert the key to a byte array
            int keyLength = keyBytes.Length; // Length of the key

            try
            {
                // Open the streams in read (source) and write (destination) mode
                using (FileStream sourceStream = new FileStream(source, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (FileStream destStream = new FileStream(destination, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    byte[] buffer = new byte[4096]; // Buffer size for block-based reading/writing
                    int bytesRead;
                    int keyIndex = 0;

                    // Read the source file in blocks
                    while ((bytesRead = sourceStream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        // Apply XOR on each byte of the buffer with the key
                        for (int i = 0; i < bytesRead; i++)
                        {
                            buffer[i] ^= keyBytes[keyIndex % keyLength]; // XOR with the key
                            keyIndex++; // Increment the key index for the next bytes
                        }

                        // Write the encrypted data to the destination file
                        destStream.Write(buffer, 0, bytesRead);
                    }
                }

                Console.WriteLine($"File successfully encrypted: {destination}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error during encryption: {e.Message}");
            }
        }

        /// <summary>
        /// Static method to encrypt file data from source and destination file streams.
        /// This method works with open FileStream objects to perform real-time encryption.
        /// </summary>
        /// <param name="sourceStream">The read stream of the source file to encrypt.</param>
        /// <param name="destStream">The write stream to save the encrypted data.</param>
        public static void EEncrypt(FileStream sourceStream, FileStream destStream)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(key); // Convert the key to a byte array
            int keyLength = keyBytes.Length; // Length of the key

            try
            {
                // Open the streams in read (source) and write (destination) mode
                using (sourceStream)
                using (destStream)
                {
                    byte[] buffer = new byte[4096]; // Buffer size for block-based reading/writing
                    int bytesRead;
                    int keyIndex = 0;

                    // Read the source file in blocks
                    while ((bytesRead = sourceStream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        // Apply XOR on each byte of the buffer with the key
                        for (int i = 0; i < bytesRead; i++)
                        {
                            buffer[i] ^= keyBytes[keyIndex % keyLength]; // XOR with the key
                            keyIndex++; // Increment the key index for the next bytes
                        }

                        // Write the encrypted data to the destination stream
                        destStream.Write(buffer, 0, bytesRead);
                    }
                }

                Console.WriteLine($"File successfully encrypted");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error during encryption: {e.Message}");
            }
        }

        /// <summary>
        /// Static method to decrypt a file using the XOR method with the same key.
        /// The source file is read, the data is decrypted, and written to a destination file.
        /// </summary>
        /// <param name="source">The path of the source file to decrypt.</param>
        /// <param name="destination">The path of the destination file where the decrypted data will be written.</param>
        public static void Decrypt(string source, string destination)
        {
            if (!File.Exists(source))
            {
                Console.WriteLine($"Error: source file '{source}' does not exist.");
                return;
            }

            byte[] keyBytes = Encoding.UTF8.GetBytes(key); // Convert the key to a byte array
            int keyLength = keyBytes.Length; // Length of the key

            try
            {
                // Open the streams in read (source) and write (destination) mode
                using (FileStream sourceStream = new FileStream(source, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (FileStream destStream = new FileStream(destination, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    byte[] buffer = new byte[4096]; // Buffer size for block-based reading/writing
                    int bytesRead;
                    int keyIndex = 0;

                    // Read the source file in blocks
                    while ((bytesRead = sourceStream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        // Apply XOR on each byte of the buffer with the key to decrypt
                        for (int i = 0; i < bytesRead; i++)
                        {
                            buffer[i] ^= keyBytes[keyIndex % keyLength]; // XOR with the key
                            keyIndex++; // Increment the key index for the next bytes
                        }

                        // Write the decrypted data to the destination file
                        destStream.Write(buffer, 0, bytesRead);
                    }
                }

                Console.WriteLine($"File successfully decrypted: {destination}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error during decryption: {e.Message}");
            }
        }
    }
}
