using BackupEngine.Shared;
using Newtonsoft.Json;

namespace BackupEngine
{
    /// <summary>
    /// Represents a backup configuration.
    /// </summary>
    public class BackupConfiguration : IJsonSerializable
    {
        /// <summary>
        /// The name of the backup configuration.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The source directory path from which files will be backed up.
        /// </summary>
        public CustomPath SourcePath { get; set; }

        /// <summary>
        /// The destination directory path where the backup files will be stored.
        /// </summary>
        public CustomPath DestinationPath { get; set; }

        /// <summary>
        /// The type of backup to be performed (e.g., full or differential).
        /// </summary>
        public BackupType BackupType { get; set; }

        /// <summary>
        /// The set of file extensions to be specifically included in the backup.
        /// </summary>
        public HashSet<string>? ExtensionsToSave { get; set; }
        /// <summary>
        /// Loads the backup configuration from a JSON string.
        /// </summary>
        /// <param name="json">The JSON string representing the backup configuration.</param>
        public void FromJson(string json)
        {
            BackupConfiguration jsonConfiguration = JsonConvert.DeserializeObject<BackupConfiguration>(json);
            if (jsonConfiguration != null)
            {
                Name = jsonConfiguration.Name;
                SourcePath = jsonConfiguration.SourcePath;
                DestinationPath = jsonConfiguration.DestinationPath;
                BackupType = jsonConfiguration.BackupType;
                ExtensionsToSave = jsonConfiguration.ExtensionsToSave;
            }
        }
        /// <summary>
        /// Serializes the current backup configuration to a JSON string.
        /// </summary>
        /// <returns>A JSON string representing the backup configuration.</returns>
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}