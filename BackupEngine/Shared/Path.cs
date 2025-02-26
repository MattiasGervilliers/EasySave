using Newtonsoft.Json;

namespace BackupEngine.Shared
{
    /// <summary>
    /// La classe CustomPath encapsule un chemin de fichier et fournit des méthodes pour valider, obtenir des informations sur ce chemin,
    /// et effectuer des opérations liées au chemin de manière sûre et contrôlée.
    /// </summary>
    [JsonConverter(typeof(CheminConverter))]
    public class CustomPath
    {
        /// <summary>
        /// Le chemin du fichier ou du répertoire.
        /// </summary>
        private string _path { get; set; }

        /// <summary>
        /// Constructeur de la classe CustomPath.
        /// Le chemin fourni est validé. Si le chemin est invalide, une exception est levée.
        /// </summary>
        /// <param name="path">Le chemin du fichier ou du répertoire à encapsuler.</param>
        /// <exception cref="ArgumentException">Si le chemin n'est pas valide.</exception>
        public CustomPath(string path)
        {
            if (!CheckPathValidity(path))
            {
                throw new ArgumentException("The path is not valid.");
            }

            _path = path;
        }

        /// <summary>
        /// Vérifie si le répertoire existe à l'emplacement spécifié par le chemin.
        /// </summary>
        /// <returns>Vrai si le répertoire existe, sinon faux.</returns>
        public bool PathExists()
        {
            return Directory.Exists(_path);
        }

        /// <summary>
        /// Vérifie la validité du chemin. Cette méthode peut être étendue pour effectuer des vérifications plus approfondies si nécessaire.
        /// </summary>
        /// <param name="TestedPath">Le chemin à tester.</param>
        /// <returns>Retourne toujours vrai pour l'instant, mais peut être étendu pour effectuer des vérifications de validité du chemin.</returns>
        private bool CheckPathValidity(string TestedPath)
        {
            // TODO : Check if the path is valid
            return true;
        }

        /// <summary>
        /// Retourne le chemin absolu correspondant au chemin relatif stocké dans _path.
        /// </summary>
        /// <returns>Le chemin absolu sous forme de chaîne de caractères.</returns>
        public string GetAbsolutePath()
        {
            return Path.GetFullPath(_path);
        }

        /// <summary>
        /// Redéfinition de la méthode ToString pour retourner le chemin absolu.
        /// </summary>
        /// <returns>Le chemin absolu du répertoire ou fichier.</returns>
        public override string ToString()
        {
            return GetAbsolutePath();
        }
    }

    /// <summary>
    /// Classe de conversion JSON pour la classe CustomPath.
    /// Cette classe permet de sérialiser et de désérialiser des objets CustomPath en utilisant leur chemin absolu sous forme de chaîne.
    /// </summary>
    public class PathConverter : JsonConverter<CustomPath>
    {
        /// <summary>
        /// Sérialise un objet CustomPath en une chaîne représentant son chemin absolu.
        /// </summary>
        /// <param name="writer">Le JsonWriter utilisé pour écrire le JSON.</param>
        /// <param name="value">L'objet CustomPath à sérialiser.</param>
        /// <param name="serializer">Le sérialiseur JSON.</param>
        public override void WriteJson(JsonWriter writer, CustomPath value, JsonSerializer serializer)
        {
            // Sérialise l'objet CustomPath en tant que chaîne de caractères représentant son chemin absolu.
            writer.WriteValue(value.GetAbsolutePath());
        }

        /// <summary>
        /// Désérialise un chemin sous forme de chaîne de caractères et le convertit en un objet CustomPath.
        /// </summary>
        /// <param name="reader">Le JsonReader utilisé pour lire le JSON.</param>
        /// <param name="objectType">Le type de l'objet à désérialiser.</param>
        /// <param name="existingValue">La valeur existante de l'objet CustomPath, s'il y en a une.</param>
        /// <param name="hasExistingValue">Indique si une valeur existante est présente.</param>
        /// <param name="serializer">Le sérialiseur JSON.</param>
        /// <returns>Un objet CustomPath créé à partir du chemin fourni dans le JSON.</returns>
        public override CustomPath ReadJson(JsonReader reader, Type objectType, CustomPath existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            // Crée et retourne un objet CustomPath en utilisant le chemin contenu dans la valeur JSON.
            return new CustomPath(reader.Value.ToString());
        }
    }
}
