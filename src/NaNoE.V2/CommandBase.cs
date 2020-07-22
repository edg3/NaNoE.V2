using System;
using System.Windows.Input;

namespace NaNoE.V2
{
    /// <summary>
    /// Base command structure
    /// </summary>
    class CommandBase : ICommand
    {
        private Action _action;

        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Check if the command can execute
        ///  - assumably yes for whichever View Model is used
        /// </summary>
        /// <param name="parameter">Sender</param>
        /// <returns>True</returns>
        public bool CanExecute(object parameter)
        {
            return true;
        }

        /// <summary>
        /// Run the action
        /// </summary>
        /// <param name="parameter">Unused paramater for action</param>
        public void Execute(object parameter)
        {
            _action.Invoke();
        }

        /// <summary>
        /// Create CommandBase
        /// </summary>
        /// <param name="act">Action (i.e. code function) for it to run</param>
        public CommandBase(Action act)
        {
            _action = act;
        }
    }
}
