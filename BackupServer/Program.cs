using BackupEngine.Job;
using BackupServer;
namespace BackupSrv
{
    class Program
    {
        private static Server _server = new Server(5000, HandleRemoteCommand);
        private static bool _isPaused = false;
        private static bool _isStopped = false;
        private static readonly object _pauseLock = new object();
        public static void StartServer()
        {
            _server.Start();
        }
        private JobManager JobManager = new JobManager();
        private static void HandleRemoteCommand(string command)
        {
            if (command == "pause")
            {
                //JobManager.PauseJob();
                /*
                _isPaused = true;
                Console.WriteLine("[Remote] Pause de la sauvegarde demandée.");
                lock (_pauseLock)
                {
                    _isPaused = true;
                }
                 * */
            }
            else if (command == "resume")
            {
                _isPaused = false;

                Console.WriteLine("[Remote] Reprise de la sauvegarde demandée.");
                lock (_pauseLock)
                {
                    _isPaused = false;
                    Monitor.PulseAll(_pauseLock); // Réveille les threads en pause
                }
            }
            else if (command == "stop")
            {
                Console.WriteLine("[Remote] Arrêt de la sauvegarde demandée.");
                lock (_pauseLock)
                {
                    _isStopped = true;
                    Monitor.PulseAll(_pauseLock);
                }
            }
        }
        static void Main(string[] args)
        { 
            
        }
    }
}