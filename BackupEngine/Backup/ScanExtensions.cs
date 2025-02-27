using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackupEngine.Backup
{
    public class ScanExtension
    {
        /// <summary>
        /// Source path where we want to scan extensions
        /// </summary>
        private readonly string _sourcePath;
        /// <summary>
        /// ScanExtension Constructor
        /// </summary>
        /// <param name="sourcePath"></param>
        public ScanExtension(string sourcePath)
        {
            _sourcePath = sourcePath;
        }
        /// <summary>
        /// Scan for extensions in the source folder
        /// </summary>
        /// <returns></returns>
        /// <exception cref="DirectoryNotFoundException"></exception>
        public HashSet<string> GetUniqueExtensions()
        {
            if (!Directory.Exists(_sourcePath))
            {
                throw new DirectoryNotFoundException($"Le dossier source '{_sourcePath}' n'existe pas.");
            }

            HashSet<string> extensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            string[] files = Directory.GetFiles(_sourcePath, "*", SearchOption.AllDirectories);

            foreach (string file in files)
            {
                string extension = Path.GetExtension(file);
                if (!string.IsNullOrEmpty(extension))
                {
                    extensions.Add(extension);
                }
            }

            return extensions;
        }
    }
}

