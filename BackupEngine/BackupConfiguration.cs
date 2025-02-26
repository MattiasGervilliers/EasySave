using BackupEngine.Shared;
using Newtonsoft.Json;

namespace BackupEngine
{
    /// <summary>
    /// Représente la configuration d'une sauvegarde, incluant le nom, les chemins source et destination,
    /// le type de sauvegarde, et si la sauvegarde doit être cryptée ou non.
    /// </summary>
    public class BackupConfiguration : IJsonSerializable
    {
        /// <summary>
        /// Le nom de la configuration de sauvegarde.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Le chemin source de la sauvegarde, où les fichiers sont récupérés.
        /// </summary>
        public CustomPath SourcePath { get; set; }

        /// <summary>
        /// Le chemin de destination de la sauvegarde, où les fichiers seront sauvegardés.
        /// </summary>
        public CustomPath DestinationPath { get; set; }

        /// <summary>
        /// Le type de sauvegarde (par exemple, complète, différentielle, etc.).
        /// </summary>
        public BackupType BackupType { get; set; }

        /// <summary>
        /// Indique si les fichiers doivent être cryptés lors de la sauvegarde.
        /// </summary>
        public bool Encrypt { get; set; }

        /// <summary>
        /// Désérialise un objet JSON en une instance de la classe BackupConfiguration.
        /// Cette méthode remplit les propriétés de l'objet BackupConfiguration avec les données provenant du JSON.
        /// </summary>
        /// <param name="json">Le JSON à désérialiser en une instance de BackupConfiguration.</param>
        public void FromJson(string json)
        {
            // Désérialisation du JSON en un objet BackupConfiguration
            BackupConfiguration jsonConfiguration = JsonConvert.DeserializeObject<BackupConfiguration>(json);

            // Si la désérialisation réussit, on assigne les valeurs des propriétés de l'objet.
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
        /// Sérialise l'objet BackupConfiguration en une chaîne JSON.
        /// Cette méthode convertit l'objet actuel en une représentation JSON.
        /// </summary>
        /// <returns>Une chaîne JSON représentant la configuration de la sauvegarde.</returns>
        public string ToJson()
        {
            // Sérialisation de l'objet en JSON avec la méthode ToJson de JsonConvert
            return JsonConvert.SerializeObject(this);
        }
    }
}
