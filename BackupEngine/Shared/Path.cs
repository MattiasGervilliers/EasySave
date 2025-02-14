using Newtonsoft.Json;

namespace BackupEngine.Shared
{
    [JsonConverter(typeof(CheminConverter))]
    public class CustomPath
    {
        private string _path { get; set; }

        public CustomPath(string path)
        {
            if (!CheckPathValidity(path))
            {
                throw new ArgumentException("The path is not valid.");
            }

            _path = path;
        }

        private bool PathExists(string TestedPath)
        {
            return Directory.Exists(TestedPath);
        }

        private bool CheckPathValidity(string TestedPath)
        {
            // TODO : Check if the path is valid
            return true;
        }

        public string GetAbsolutePath()
        {
            return Path.GetFullPath(_path);
        }

        public override string ToString()
        {
            return GetAbsolutePath();
        }
    }

    public class CheminConverter : JsonConverter<CustomPath>
    {
        public override void WriteJson(JsonWriter writer, CustomPath value, JsonSerializer serializer)
        {
            writer.WriteValue(value.GetAbsolutePath());  // Serialize as a string with absolute path
        }

        public override CustomPath ReadJson(JsonReader reader, Type objectType, CustomPath existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            return new CustomPath(reader.Value.ToString());
        }
    }
}