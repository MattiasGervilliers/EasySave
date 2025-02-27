using LanguageLib;
using System.Text.Json;

public class LanguageManager
{
    /// <summary>
    /// Dictionary containing translations for the current language.
    /// </summary>
    private Dictionary<string, string>? _translations;

    /// <summary>
    /// The folder where language JSON files are stored.
    /// </summary>
    private readonly string _languageFolder = "Languages"; // Folder containing JSON files

    /// <summary>
    /// The currently selected language.
    /// </summary>
    private Language _currentLanguage;

    /// <summary>
    /// Initializes a new instance of the LanguageManager class and loads the selected language file.
    /// </summary>
    /// <param name="language">The language to be loaded.</param>
    public LanguageManager(Language language)
    {
        _currentLanguage = language;
        LoadLanguageFile();
    }
    /// <summary>
    /// Loads the language file corresponding to the current language.
    /// </summary>
    /// <exception cref="FileNotFoundException">Thrown if the language file does not exist.</exception>
    private void LoadLanguageFile()
    {
        string fileName = _currentLanguage.ToString() + ".json";
        string filePath = Path.Combine(_languageFolder, fileName);

        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            _translations = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
        }
        else
        {
            throw new FileNotFoundException($"Language file '{filePath}' not found.");
        }
    }
    /// <summary>
    /// Retrieves the translation for a given key, replacing placeholders if provided.
    /// </summary>
    /// <param name="key">The key identifying the translation.</param>
    /// <param name="placeholders">Optional dictionary of placeholders to replace in the translation.</param>
    /// <returns>The translated string if found, otherwise the key itself.</returns>
    public string GetTranslation(string key, Dictionary<string, string>? placeholders = null)
    {
        if (_translations.TryGetValue(key, out string value))
        {
            if (placeholders != null)
            {
                foreach (var placeholder in placeholders)
                {
                    value = value.Replace($"{{{placeholder.Key}}}", placeholder.Value);
                }
            }
            return value;
        }

        return key; // Return the key itself if not found
    }
}
