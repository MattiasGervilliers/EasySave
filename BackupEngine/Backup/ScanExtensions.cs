using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackupEngine.Backup
{
    public class ScanExtension
    {
        private readonly string _sourcePath;

        public ScanExtension(string sourcePath)
        {
            _sourcePath = sourcePath;
        }

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

