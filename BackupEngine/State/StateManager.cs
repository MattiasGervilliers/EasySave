using BackupEngine.Settings;
using System.Text.Json;

namespace BackupEngine.State
{
    public class StateManager
    {
        private readonly string _statePath;
        private static readonly object _fileLock = new object();

        public StateManager()
        {
            SettingsRepository settingsRepository = new SettingsRepository();
            _statePath = settingsRepository.GetStatePath().GetAbsolutePath();
        }

        public void OnStateUpdated(object sender, StateEvent e)
        {
            // Create file if it doesn't exist
            if (!File.Exists(_statePath))
            {
                File.Create(_statePath).Close();
            }
            // Save the state in a JSON file by overwriting it
            LogState(e);
        }

        private void LogState(StateEvent stateEvent)
        {
            // Serialize the StateEvent object into JSON
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
