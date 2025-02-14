using LanguageLib;
using System.Text.Json;

public class LanguageManager
{
    private Dictionary<string, string>? _translations;
    private readonly string _languageFolder = "Languages"; // Folder containing JSON files
    private Language _currentLanguage;

    public LanguageManager(Language language)
    {
        _currentLanguage = language;
        LoadLanguageFile();
    }

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
