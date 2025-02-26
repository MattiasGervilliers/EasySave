using LanguageLib;
using System.Text.Json;

public class LanguageManager
{
    // Dictionary containing translations for the current language.
    private Dictionary<string, string>? _translations;

    // Folder containing language files (in JSON format).
    private readonly string _languageFolder = "Languages"; // Folder containing JSON files

    // The currently selected language.
    private Language _currentLanguage;

    /// <summary>
    /// Constructor to initialize the language manager with the selected language.
    /// Also loads the corresponding language file.
    /// </summary>
    /// <param name="language">The selected language for translations.</param>
    public LanguageManager(Language language)
    {
        _currentLanguage = language;
        LoadLanguageFile(); // Loads the corresponding language file.
    }

    /// <summary>
    /// Loads the corresponding JSON language file and deserializes the translations.
    /// </summary>
    private void LoadLanguageFile()
    {
        // Generates the file name based on the current language (e.g., "English.json").
        string fileName = _currentLanguage.ToString() + ".json";

        // Creates the full path to the language file.
        string filePath = Path.Combine(_languageFolder, fileName);

        // Checks if the language file exists at this location.
        if (File.Exists(filePath))
        {
            // Reads the content of the JSON file.
            string json = File.ReadAllText(filePath);

            // Deserializes the JSON content into a dictionary of translations (key = original text, value = translation).
            _translations = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
        }
        else
        {
            // If the file is not found, throws an exception to indicate the error.
            throw new FileNotFoundException($"Language file '{filePath}' not found.");
        }
    }

    /// <summary>
    /// Retrieves the translation for a specified key, with the option to include placeholder values.
    /// </summary>
    /// <param name="key">The key for which the translation is requested.</param>
    /// <param name="placeholders">An optional dictionary containing key/value pairs to be replaced in the translation.</param>
    /// <returns>The translation corresponding to the key, with replacements made, or the key itself if the translation is not found.</returns>
    public string GetTranslation(string key, Dictionary<string, string>? placeholders = null)
    {
        // Attempts to retrieve the translation value for the given key.
        if (_translations.TryGetValue(key, out string value))
        {
            // If placeholders are provided, replace them in the translation.
            if (placeholders != null)
            {
                foreach (var placeholder in placeholders)
                {
                    value = value.Replace($"{{{placeholder.Key}}}", placeholder.Value);
                }
            }

            // Returns the translation with replacements made (if applicable).
            return value;
        }

        // If the key is not found, returns the key itself as a default value.
        return key;
    }
}
