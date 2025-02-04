using BackupEngine.Shared;
using Newtonsoft.Json;

namespace BackupEngine
{
    /// <summary>
    /// Represents a backup configuration.
    /// </summary>
    internal class BackupConfiguration : IJsonSerializable
    {
        public string Name { get; set; }
        public Path SourcePath { get; set; }
        public Path DestinationPath { get; set; }
        public BackupType BackupType { get; set; }

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