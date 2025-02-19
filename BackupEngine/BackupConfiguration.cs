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
        public CustomPath SourcePath { get; set; }
        public CustomPath DestinationPath { get; set; }
        public BackupType BackupType { get; set; }
        public string EncryptionKey { get; set; }
        public HashSet<string> ExtensionsToSave { get; set; }

        public void FromJson(string json)
        {
            BackupConfiguration jsonConfiguration = JsonConvert.DeserializeObject<BackupConfiguration>(json);
            if (jsonConfiguration != null)
            {
                Name = jsonConfiguration.Name;
                SourcePath = jsonConfiguration.SourcePath;
                DestinationPath = jsonConfiguration.DestinationPath;
                BackupType = jsonConfiguration.BackupType;
                EncryptionKey = jsonConfiguration.EncryptionKey;
                ExtensionsToSave = jsonConfiguration.ExtensionsToSave;
            }
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}