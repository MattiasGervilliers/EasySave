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
        private Path SourcePath { get; set; }
        private Path DestinationPath { get; set; }
        private BackupType BackupType { get; set; }

        public void Update(string name, Path NewSourcePath,Path NewCiblePath,BackupType backupType)
        {
            this.Name = name;
            this.SourcePath = NewSourcePath ;
            this.DestinationPath = NewCiblePath ;
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