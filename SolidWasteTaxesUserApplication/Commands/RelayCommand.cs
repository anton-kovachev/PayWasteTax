using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SolidWasteTaxesUserApplication.Commands
{
    public class RelayCommand : ICommand
    {
        protected Action<object> _Execute;
        protected Predicate<object> _CanExecute;
        protected EventHandler _CanExecuteChanged = (o, e) => { };

        public RelayCommand(Action<object> pExecute)
            : this(pExecute, null)
        {
        }
        public RelayCommand(Action<object> pExecute, Predicate<object> pCanExecute)
        {
            if (pExecute == null)
                throw new ArgumentNullException("pExecute", "[RelayCommand] The action for a command cannot be null.");

            _Execute = pExecute;
            _CanExecute = pCanExecute;
        }


        public bool CanExecute(object pParameter)
        {
            return _CanExecute(pParameter);
        }
        public void Execute(object pParameter)
        {
            _Execute(pParameter);
        }
        public event EventHandler CanExecuteChanged
        {
            add { _CanExecuteChanged += value; }
            remove { _CanExecuteChanged -= value; }
        }
        public void SignalCanExecuteChanged()
        {
            if (_CanExecuteChanged != null)
                _CanExecuteChanged(this, new EventArgs());
        }
    }

}
