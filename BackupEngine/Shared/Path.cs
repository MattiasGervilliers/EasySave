using Newtonsoft.Json;

namespace BackupEngine.Shared
{
    /// <summary>
    /// The CustomPath class encapsulates a file path and provides methods to validate, retrieve information about the path,
    /// and perform path-related operations in a safe and controlled manner.
    /// </summary>
    [JsonConverter(typeof(PathConverter))]
    public class CustomPath
    {
        /// <summary>
        /// The file or directory path.
        /// </summary>
        private string _path { get; set; }

        /// <summary>
        /// Constructor for the CustomPath class.
        /// The provided path is validated. If the path is invalid, an exception is thrown.
        /// </summary>
        /// <param name="path">The file or directory path to encapsulate.</param>
        /// <exception cref="ArgumentException">If the path is not valid.</exception>
        public CustomPath(string path)
        {
            if (!CheckPathValidity(path))
            {
                throw new ArgumentException("The path is not valid.");
            }

            _path = path;
        }

        /// <summary>
        /// Checks if the directory exists at the location specified by the path.
        /// </summary>
        /// <returns>True if the directory exists, otherwise false.</returns>
        public bool PathExists()
        {
            return Directory.Exists(_path);
        }

        /// <summary>
        /// Checks the validity of the path. This method can be extended to perform more thorough checks if necessary.
        /// </summary>
        /// <param name="TestedPath">The path to test.</param>
        /// <returns>Returns true for now, but can be extended to perform path validity checks.</returns>
        private bool CheckPathValidity(string testedPath)
        {
            // TODO : Check if the path is valid
            return true;
        }

        /// <summary>
        /// Returns the absolute path corresponding to the relative path stored in _path.
        /// </summary>
        /// <returns>The absolute path as a string.</returns>
        public string GetAbsolutePath()
        {
            return Path.GetFullPath(_path);
        }

        /// <summary>
        /// Overrides the ToString method to return the absolute path.
        /// </summary>
        /// <returns>The absolute path of the directory or file.</returns>
        public override string ToString()
        {
            return GetAbsolutePath();
        }
    }
  
    /// <summary>
    /// JSON conversion class for the CustomPath class.
    /// This class allows serializing and deserializing CustomPath objects using their absolute path as a string.
    /// </summary>
    public class PathConverter : JsonConverter<CustomPath>
    {
        /// <summary>
        /// Serializes a CustomPath object into a string representing its absolute path.
        /// </summary>
        /// <param name="writer">The JsonWriter used to write the JSON.</param>
        /// <param name="value">The CustomPath object to serialize.</param>
        /// <param name="serializer">The JSON serializer.</param>
        public override void WriteJson(JsonWriter writer, CustomPath value, JsonSerializer serializer)
        {
            // Serializes the CustomPath object as a string representing its absolute path.
            writer.WriteValue(value.GetAbsolutePath());
        }

        /// <summary>
        /// Deserializes a path as a string and converts it into a CustomPath object.
        /// </summary>
        /// <param name="reader">The JsonReader used to read the JSON.</param>
        /// <param name="objectType">The type of the object to deserialize.</param>
        /// <param name="existingValue">The existing value of the CustomPath object, if any.</param>
        /// <param name="hasExistingValue">Indicates whether an existing value is present.</param>
        /// <param name="serializer">The JSON serializer.</param>
        /// <returns>A CustomPath object created from the path provided in the JSON.</returns>
        public override CustomPath ReadJson(JsonReader reader, Type objectType, CustomPath existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            // Creates and returns a CustomPath object using the path contained in the JSON value.
            return new CustomPath(reader.Value.ToString());
        }
    }
}
