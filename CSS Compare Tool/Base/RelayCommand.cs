using System;
using System.Windows.Input;

namespace CSS_Compare_Tool.Base
{
    public class RelayCommand : ICommand
    {
        public Action<object> OnExecute;
        public Func<object, bool> OnExecuteChanged;

        public RelayCommand(Action<object> onExecute, Func<object, bool> onExecuteChanged = null)
        {
            OnExecute = onExecute;
            OnExecuteChanged = onExecuteChanged;
        }

        event EventHandler ICommand.CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }

            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }

        bool ICommand.CanExecute(object parameter)
        {
            return OnExecute == null || OnExecuteChanged(parameter);
        }

        void ICommand.Execute(object parameter)
        {
            OnExecute(parameter);
        }
    }
}
