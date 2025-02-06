using System;
using System.Text.RegularExpressions;

namespace BackupEngine.Shared
{
    public class Chemin
    {
        private string _path { get; set; }

        public Chemin(string path)
        {
            if (!CheckPathValidity(path))
            {
                throw new ArgumentException("The path is not valid.");
            }

            _path = path;
        }

        private bool PathExists(string TestedPath)
        {
            return System.IO.Directory.Exists(TestedPath);
        }

        private bool CheckPathValidity(string TestedPath)
        {
            // TODO : Check if the path is valid
            return true;
        }

        public string GetAbsolutePath()
        {
            return System.IO.Path.GetFullPath(_path);
        }
    }
}
