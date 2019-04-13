using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SVNMailer
{
    public class RelayCommand: ICommand
    {
        private Func<bool> _CanExecute;
        private Action _Execute;

        public RelayCommand(Func<bool> canExecute,Action execute)
        {
            _CanExecute = canExecute;
            _Execute = execute;
        }
        public bool CanExecute(object parameter)
        {
            return _CanExecute();
        }
        public void Execute(object parameter)
        {
            _Execute();
        }

        public event EventHandler CanExecuteChanged;
    }
}
