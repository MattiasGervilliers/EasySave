using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaveEngine.Backup
{
    public class FileManager
    {
        private ISaveStrategy saveStrategy;

        public FileManager(ISaveStrategy saveStrategy)
        {
            this.saveStrategy = saveStrategy;
        }

        public void SetSaveStrategy(ISaveStrategy newStrategy)
        {
            saveStrategy = newStrategy;
        }

        public void Save(string sourcePath, string destinationPath)
        {
            saveStrategy.Save(sourcePath, destinationPath);
        }
    }


}
