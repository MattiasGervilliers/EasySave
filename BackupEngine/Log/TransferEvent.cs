using BackupEngine.Backup;
using System;

namespace BackupEngine.Log
{
    public class TransferEvent
    {
        private BackupConfiguration Configuration;
        private TransferState State;
        private TimeSpan TransferDuration;

        public TransferEvent(BackupConfiguration configuration, TransferState state, TimeSpan transferDuration)
        {
            Configuration = configuration;
            State = state;
            TransferDuration = transferDuration;
        }
    }
}
