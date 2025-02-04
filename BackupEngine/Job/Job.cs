using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackupEngine.Job
{
    internal class Job
    {
        private BackupConfiguration Configuration { get; set; }

        public Job(BackupConfiguration configuration) { 
            Configuration = configuration;
        }

        public void Run()
        {
            
        }
    }
}
