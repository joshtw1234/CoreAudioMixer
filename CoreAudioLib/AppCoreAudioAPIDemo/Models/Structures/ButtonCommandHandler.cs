using System;
using System.Windows.Input;

namespace AppCoreAudioAPIDemo.Models.Structures
{
    public class ButtonCommandHandler : ICommand
    {
        private Action<object> _action;
        private bool _canExecute;
        private Action onButtonClick;
        private bool v;

        public ButtonCommandHandler(Action<object> action, bool canExecute)
        {
            _action = action;
            _canExecute = canExecute;
        }

        public ButtonCommandHandler(Action onButtonClick, bool v)
        {
            this.onButtonClick = onButtonClick;
            this.v = v;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            _action(parameter);
        }
    }
}
