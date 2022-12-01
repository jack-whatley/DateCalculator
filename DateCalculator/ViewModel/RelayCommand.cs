using System;
using System.Windows.Input;

namespace DayOfWeek.ViewModel
{
    /// <summary>
    /// Facilitates relaying commands between views and viewmodels
    /// </summary>
    public class RelayCommand : ICommand
    {
        private Predicate<object> canExecute;
        private Action<object> execute;

        /// <summary>
        /// constuctor for relay command
        /// </summary>
        /// <param name="canExecute">The function to check if the command can be run.</param>
        /// <param name="execute">The function to run.</param>
        public RelayCommand(Predicate<object> canExecute, Action<object> execute)
        {
            this.canExecute = canExecute;
            this.execute = execute;
        }

        /// <summary>
        /// constuctor for relay command
        /// </summary>
        /// <param name="execute">The function to run.</param>
        /// <remarks>Use this if the function can always be run.</remarks>
        public RelayCommand(Action<object> execute)
        {
            this.canExecute = new Predicate<object>((object _) =>true);
            this.execute = execute;
        }

        /// <summary>
        /// Ask the Canexecute value to be recalculated.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
           add { CommandManager.RequerySuggested += value; }
           remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns>True if the function can run, false otherwise.</returns>
        public bool CanExecute(object parameter)
        {
            return canExecute(parameter);
        }

        /// <summary>
        /// Runs the function.
        /// </summary>
        /// <param name="parameter">The parameter to use, null if no parameter is required.</param>
        public void Execute(object parameter)
        {
            execute(parameter);
        }
    }
}
