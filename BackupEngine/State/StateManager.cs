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
            _statePath = settingsRepository.GetStatePath();
        }

        public void OnStateUpdated(object sender, StateEvent e)
        {
            // Enregistre l'état dans un fichier JSON en le réécrivant
            LogState(e);
        }

        private void LogState(StateEvent stateEvent)
        {
            // Sérialiser l'objet StateEvent en JSON
            string jsonString = JsonSerializer.Serialize(stateEvent, new JsonSerializerOptions { WriteIndented = true });

            // Réécrire le fichier avec le contenu actuel
            lock (_fileLock) // Ensure thread safety
            {
                using (StreamWriter sw = new StreamWriter(_statePath))
                {
                    sw.WriteLine(jsonString);
                }
            }
        }
    }
}
