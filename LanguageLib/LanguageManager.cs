using LanguageLib;
using System.Text.Json;

public class LanguageManager
{
    // Dictionnaire contenant les traductions de la langue actuelle.
    private Dictionary<string, string>? _translations;

    // Dossier contenant les fichiers de langue (au format JSON).
    private readonly string _languageFolder = "Languages"; // Folder containing JSON files

    // La langue actuelle sélectionnée.
    private Language _currentLanguage;

    /// <summary>
    /// Constructeur pour initialiser le gestionnaire de langue avec la langue sélectionnée.
    /// Charge également le fichier de langue correspondant.
    /// </summary>
    /// <param name="language">Langue sélectionnée pour les traductions.</param>
    public LanguageManager(Language language)
    {
        _currentLanguage = language;
        LoadLanguageFile(); // Charge le fichier de langue correspondant.
    }

    /// <summary>
    /// Charge le fichier JSON correspondant à la langue actuelle et deserialise les traductions.
    /// </summary>
    private void LoadLanguageFile()
    {
        // Génère le nom du fichier basé sur la langue actuelle (par exemple : "English.json").
        string fileName = _currentLanguage.ToString() + ".json";

        // Crée le chemin complet vers le fichier de langue.
        string filePath = Path.Combine(_languageFolder, fileName);

        // Vérifie si le fichier de langue existe à cet emplacement.
        if (File.Exists(filePath))
        {
            // Lit le contenu du fichier JSON.
            string json = File.ReadAllText(filePath);

            // Désérialise le contenu JSON dans un dictionnaire de traductions (clé = texte original, valeur = traduction).
            _translations = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
        }
        else
        {
            // Si le fichier n'est pas trouvé, lève une exception pour indiquer l'erreur.
            throw new FileNotFoundException($"Language file '{filePath}' not found.");
        }
    }

    /// <summary>
    /// Récupère la traduction d'une clé spécifiée, avec la possibilité d'inclure des valeurs de remplacement.
    /// </summary>
    /// <param name="key">Clé pour laquelle la traduction est demandée.</param>
    /// <param name="placeholders">Dictionnaire optionnel contenant des paires de clés/valeurs à remplacer dans la traduction.</param>
    /// <returns>La traduction correspondant à la clé, avec les remplacements effectués, ou la clé elle-même si la traduction est introuvable.</returns>
    public string GetTranslation(string key, Dictionary<string, string>? placeholders = null)
    {
        // Tente de récupérer la valeur de traduction pour la clé donnée.
        if (_translations.TryGetValue(key, out string value))
        {
            // Si des espaces réservés sont fournis, les remplace dans la traduction.
            if (placeholders != null)
            {
                foreach (var placeholder in placeholders)
                {
                    value = value.Replace($"{{{placeholder.Key}}}", placeholder.Value);
                }
            }

            // Retourne la traduction avec les remplacements effectués (si applicable).
            return value;
        }

        // Si la clé n'est pas trouvée, retourne la clé elle-même comme valeur par défaut.
        return key;
    }
}
