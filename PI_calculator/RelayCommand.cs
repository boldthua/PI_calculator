using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PI_calculator
{
    internal class RelayCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        Action action { get; set; }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            action.Invoke();
        }

        public RelayCommand(Action action)
        {
            this.action = action;
        }
    }
}
