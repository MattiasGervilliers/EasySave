using BackupEngine.Shared;
using Newtonsoft.Json;

namespace BackupEngine
{
    /// <summary>
    /// Represents the configuration of a backup, including the name, source and destination paths,
    /// the type of backup, and whether the backup should be encrypted or not.
    /// </summary>
    public class BackupConfiguration : IJsonSerializable
    {
        /// <summary>
        /// The name of the backup configuration.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The source path of the backup, where the files are retrieved from.
        /// </summary>
        public CustomPath SourcePath { get; set; }

        /// <summary>
        /// The destination path of the backup, where the files will be saved.
        /// </summary>
        public CustomPath DestinationPath { get; set; }

        /// <summary>
        /// The type of backup (for example, full, differential, etc.).
        /// </summary>
        public BackupType BackupType { get; set; }

        /// <summary>
        /// Indicates whether the files should be encrypted during the backup.
        /// </summary>
        public bool Encrypt { get; set; }

        /// <summary>
        /// Deserializes a JSON object into an instance of the BackupConfiguration class.
        /// This method fills the properties of the BackupConfiguration object with data from the JSON.
        /// </summary>
        /// <param name="json">The JSON to deserialize into a BackupConfiguration instance.</param>
        public void FromJson(string json)
        {
            // Deserialize the JSON into a BackupConfiguration object
            BackupConfiguration jsonConfiguration = JsonConvert.DeserializeObject<BackupConfiguration>(json);

            // If deserialization is successful, assign the values of the object's properties.
            if (jsonConfiguration != null)
            {
                Name = jsonConfiguration.Name;
                SourcePath = jsonConfiguration.SourcePath;
                DestinationPath = jsonConfiguration.DestinationPath;
                BackupType = jsonConfiguration.BackupType;
                Encrypt = jsonConfiguration.Encrypt;
            }
        }

        /// <summary>
        /// Serializes the BackupConfiguration object into a JSON string.
        /// This method converts the current object into a JSON representation.
        /// </summary>
        /// <returns>A JSON string representing the backup configuration.</returns>
        public string ToJson()
        {
            // Serialize the object into JSON using JsonConvert's ToJson method
            return JsonConvert.SerializeObject(this);
        }
    }
}
