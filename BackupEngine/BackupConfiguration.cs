using BackupEngine.Shared;
using Newtonsoft.Json;

namespace BackupEngine
{
    /// <summary>
    /// Represents a backup configuration.
    /// </summary>
    public class BackupConfiguration(string name, Path sourcePath, Path destinationPath, BackupType backupType) : IJsonSerializable
    {
        public string Name { get; set; } = name;
        public Path SourcePath { get; set; } = sourcePath;
        public Path DestinationPath { get; set; } = destinationPath;
        public BackupType BackupType { get; set; } = backupType;

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