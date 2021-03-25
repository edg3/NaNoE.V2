using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace NaNoE.V2.Droid
{
    class CommandBase : ICommand
    {
        private Action _action;

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _action.Invoke();
        }

        public CommandBase(Action act)
        {
            _action = act;
        }
    }
}
