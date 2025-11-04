using System;
using System.Windows.Input;

namespace KukaQRCodeGenerationTools2026.Common.Wpf
{
    public class CommandBase: ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return DoCanExecute?.Invoke(parameter) == true;
        }

        public void Execute(object parameter)
        {
            // DoExecute不为空则执行
            DoExecute?.Invoke(parameter);
        }

        public Action<object> DoExecute { get; set; }

        public Func<object, bool> DoCanExecute { get; set; }


        public CommandBase(Action<object> doExecute, Func<object, bool> doCanExecute = null)
        {
            DoExecute = doExecute;
            DoCanExecute = doCanExecute ?? ((o) => true);
        }
    }
}
