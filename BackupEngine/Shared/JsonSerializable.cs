namespace BackupEngine.Shared
{
    internal interface IJsonSerializable
    {
        /// <summary>
        /// Deserialize an object from a Json string
        /// </summary>
        /// <param name="json"></param>
        void FromJson(string json);
        
        /// <summary>
        /// Serialize an object to a Json string
        /// </summary>
        /// <returns></returns>
        string ToJson();
    }
}
