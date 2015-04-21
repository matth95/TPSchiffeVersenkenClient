using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SV_Client.Classes
{
    public class RelayCommand : ICommand
    {
        // VARIABLES

        private Action<object> pr_execute;
        private Predicate<object> pr_canExecute;

        public bool CanExecute(object parameter)
        {
            return this.pr_canExecute != null && this.pr_canExecute(parameter);
        }

        public event EventHandler CanExecuteChanged; // needed for ICommand structure

        public void Execute(object parameter)
        {
            this.pr_execute(parameter);
        }

        // CONSTRUCTORS

        public RelayCommand(Action<object> execute)
            : this(execute, DefaultCanExecute)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="execute"></param>
        /// <param name="canExecute"></param>
        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            if (execute == null)
            {
                throw new ArgumentNullException("execute");
            }
            if (canExecute == null)
            {

                throw new ArgumentNullException("canExecute");
            }
            this.pr_execute = execute;
            this.pr_canExecute = canExecute;
        }

        // FUNCTIONS

        private static bool DefaultCanExecute(object parameter)
        {
            return true;
        }
    }
}
