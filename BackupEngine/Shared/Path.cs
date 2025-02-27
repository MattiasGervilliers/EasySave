using Newtonsoft.Json;

namespace BackupEngine.Shared
{
    [JsonConverter(typeof(CheminConverter))]
    public class CustomPath
    {
        /// <summary>
        /// The stored file or directory path.
        /// </summary>
        private string _path { get; set; }
        /// <summary>
        /// Initializes a new instance of the CustomPath class.
        /// </summary>
        /// <param name="path">The file or directory path to store.</param>
        /// <exception cref="ArgumentException">Thrown if the provided path is not valid.</exception>
        public CustomPath(string path)
        {
            if (!CheckPathValidity(path))
            {
                throw new ArgumentException("The path is not valid.");
            }

            _path = path;
        }
        /// <summary>
        /// Checks if the specified path exists on the file system.
        /// </summary>
        /// <returns>True if the path exists, otherwise false.</returns>
        public bool PathExists()
        {
            return Directory.Exists(_path);
        }
        /// <summary>
        /// Validates the given path.
        /// </summary>
        /// <param name="TestedPath">The path to validate.</param>
        /// <returns>True if the path is valid, otherwise false.</returns>
        private bool CheckPathValidity(string TestedPath)
        {
            // TODO : Check if the path is valid
            return true;
        }
        /// <summary>
        /// Retrieves the absolute path representation of the stored path.
        /// </summary>
        /// <returns>The absolute path as a string.</returns>
        public string GetAbsolutePath()
        {
            return Path.GetFullPath(_path);
        }
        /// <summary>
        /// Returns the absolute path as a string.
        /// </summary>
        /// <returns>The absolute path.</returns>
        public override string ToString()
        {
            return GetAbsolutePath();
        }
    }
    /// <summary>
    /// JSON converter for serializing and deserializing CustomPath objects.
    /// </summary>
    public class CheminConverter : JsonConverter<CustomPath>
    {
        /// <summary>
        /// Serializes a CustomPath object to JSON.
        /// </summary>
        /// <param name="writer">The JSON writer.</param>
        /// <param name="value">The CustomPath instance to serialize.</param>
        /// <param name="serializer">The serializer instance.</param>
        public override void WriteJson(JsonWriter writer, CustomPath value, JsonSerializer serializer)
        {
            writer.WriteValue(value.GetAbsolutePath());  // Serialize as a string with absolute path
        }
        /// <summary>
        /// Deserializes a JSON value into a CustomPath object.
        /// </summary>
        /// <param name="reader">The JSON reader.</param>
        /// <param name="objectType">The expected type of the object.</param>
        /// <param name="existingValue">The existing CustomPath instance, if available.</param>
        /// <param name="hasExistingValue">Indicates whether an existing value was provided.</param>
        /// <param name="serializer">The serializer instance.</param>
        /// <returns>A new CustomPath instance initialized from the JSON value.</returns>
        public override CustomPath ReadJson(JsonReader reader, Type objectType, CustomPath existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            return new CustomPath(reader.Value.ToString());
        }
    }
}