using System.Windows.Input;

namespace EasySaveGUI
{
    /// <summary>
    /// Commande générique qui implémente l'interface ICommand.
    /// Elle permet de lier des actions (méthodes) à des commandes dans l'interface utilisateur (par exemple, des boutons).
    /// </summary>
    public class RelayCommand : ICommand
    {
        // Action à exécuter lorsque la commande est invoquée.
        private readonly Action<object?> _execute = execute;

        // Fonction qui détermine si la commande peut être exécutée, en fonction du paramètre fourni.
        private readonly Func<object?, bool>? _canExecute = canExecute;

        // Événement qui notifie que la possibilité d'exécuter la commande a changé.
        public event EventHandler? CanExecuteChanged;

        /// <summary>
        /// Vérifie si la commande peut être exécutée, en appelant la fonction canExecute si elle est fournie.
        /// </summary>
        /// <param name="parameter">Paramètre optionnel à passer à la fonction canExecute.</param>
        /// <returns>Retourne true si la commande peut être exécutée, false sinon.</returns>
        public bool CanExecute(object? parameter)
            => _canExecute?.Invoke(parameter) ?? true; // Si _canExecute est null, retourne true (la commande peut être exécutée).

        /// <summary>
        /// Exécute l'action associée à la commande.
        /// </summary>
        /// <param name="parameter">Paramètre optionnel à passer à l'action exécutée.</param>
        public void Execute(object? parameter)
            => _execute(parameter); // Exécute l'action en passant le paramètre.
    }
}
