using System.Windows.Input;

namespace EasySaveGUI
{
    /// <summary>
    /// Generic command that implements the ICommand interface.
    /// It allows binding actions (methods) to commands in the user interface (for example, buttons).
    /// </summary>
    public class RelayCommand(Action<object?> execute, Func<object?, bool>? canExecute = null) : ICommand
    {
        // Action to execute when the command is invoked.
        private readonly Action<object?> _execute = execute;

        // Function that determines if the command can be executed, based on the provided parameter.
        private readonly Func<object?, bool>? _canExecute = canExecute;

        // Event that notifies when the ability to execute the command has changed.
        public event EventHandler? CanExecuteChanged;

        /// <summary>
        /// Checks if the command can be executed by calling the canExecute function if provided.
        /// </summary>
        /// <param name="parameter">Optional parameter to pass to the canExecute function.</param>
        /// <returns>Returns true if the command can be executed, false otherwise.</returns>
        public bool CanExecute(object? parameter)
            => _canExecute?.Invoke(parameter) ?? true; // If _canExecute is null, returns true (the command can be executed).

        /// <summary>
        /// Executes the action associated with the command.
        /// </summary>
        /// <param name="parameter">Optional parameter to pass to the executed action.</param>
        public void Execute(object? parameter)
            => _execute(parameter); // Executes the action, passing the parameter.
    }
}
