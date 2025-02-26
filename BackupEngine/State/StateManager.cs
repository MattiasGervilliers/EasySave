using BackupEngine.Settings;
using System.Text.Json;

namespace BackupEngine.Progress
{
    public class StateManager
    {
        private readonly string _statePath;
        private static readonly object _fileLock = new object();

        public StateManager()
        {
            SettingsRepository settingsRepository = new SettingsRepository();
            _statePath = settingsRepository.GetStatePath().GetAbsolutePath() + "/state.json";
        }

        public void OnStateUpdated(object sender, StateEvent e)
        {
            // Create file if doesn't exists
            if (!File.Exists(_statePath))
            {
                File.Create(_statePath).Close();
            }
            // Enregistre l'état dans un fichier JSON en le réécrivant
            LogState(e);
        }

        private void LogState(StateEvent stateEvent)
        {
            // Sérialiser l'objet StateEvent en JSON
            string jsonString = JsonSerializer.Serialize(stateEvent, new JsonSerializerOptions { WriteIndented = true });

            lock (_fileLock) // Ensure thread safety
            {
                using (StreamWriter sw = new StreamWriter(_statePath, false)) // 'false' to overwrite the file
                {
                    sw.WriteLine(jsonString);
                }
            }
        }
    }
}