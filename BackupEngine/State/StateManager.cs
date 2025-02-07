using System.IO;
using System.Text.Json;

namespace BackupEngine.State
{
    public class StateManager
    {
        private readonly string _logPath;

        public StateManager(string logPath)
        {
            _logPath = logPath;
        }

        public void OnStateUpdated(object sender, StateEvent e)
        {
            // Logge l'état mis à jour
            Console.WriteLine($"État mis à jour : {e.JobName} - {e.JobState} - {e.RemainingFiles} fichiers restants");

            // Enregistre l'état dans un fichier JSON en le réécrivant
            LogState(e);
        }

        private void LogState(StateEvent stateEvent)
        {
            // Chemin du fichier JSON
            string jsonFilePath = Path.Combine(_logPath, "state_log.json");

            // Sérialiser l'objet StateEvent en JSON
            string jsonString = JsonSerializer.Serialize(stateEvent, new JsonSerializerOptions { WriteIndented = true });

            // Réécrire le fichier avec le contenu actuel
            File.WriteAllText(jsonFilePath, jsonString);
        }
    }
}
