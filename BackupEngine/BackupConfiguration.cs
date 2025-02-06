using BackupEngine.Shared;
using Newtonsoft.Json;

namespace BackupEngine
{
    /// <summary>
    /// Represents a backup configuration.
    /// </summary>
    public class BackupConfiguration : IJsonSerializable
    {
        public string Name { get; set; }
        private Chemin SourcePath { get; set; }
        private Chemin DestinationPath { get; set; }
        private BackupType BackupType { get; set; }
        public BackupType GetBackupType()
        {
            return this.BackupType;
        }
        public Chemin GetSourcePath()
        {
            return this.SourcePath;
        }
        public Chemin GetDestinationPath()
        {
            return this.DestinationPath;
        }

        public void Update(string name, Chemin NewSourcePath, Chemin NewCiblePath, BackupType backupType)
        {
            this.Name = name;
            this.SourcePath = NewSourcePath;
            this.DestinationPath = NewCiblePath;
            this.BackupType = backupType;
        }

        public void FromJson(string json)
        {
            BackupConfiguration jsonConfiguration = JsonConvert.DeserializeObject<BackupConfiguration>(json);
            if (jsonConfiguration != null)
            {
                Name = jsonConfiguration.Name;
                SourcePath = jsonConfiguration.SourcePath;
                DestinationPath = jsonConfiguration.DestinationPath;
                BackupType = jsonConfiguration.BackupType;
            }
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}