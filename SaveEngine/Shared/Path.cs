using System;

namespace BackupEngine
{
    internal class Path
    {
        private string _path { get; set; }

        public Path(string path)
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
            return System.IO.Path.IsPathRooted(TestedPath);
        }

        public string GetAbsolutePath()
        {
            return System.IO.Path.GetFullPath(_path);
        }
    }
}
