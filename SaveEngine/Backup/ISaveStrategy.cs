using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaveEngine.Backup
{
    public interface ISaveStrategy
    {
        void Save(string sourcePath, string destinationPath);
    }
}