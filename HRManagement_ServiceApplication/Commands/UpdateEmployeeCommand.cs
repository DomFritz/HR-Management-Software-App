using HRManagement_ServiceApplication.Commands;
using System;
using System.Linq;
using System.Windows.Input;

namespace HRManagement_ServiceApplication
{
    internal interface IUpdateEmployeeContent : ICloseable
    {
        void UpdateEmployee();
    }

    /// <summary>
    /// Update the given employee in the Update-Mode of the "Create Employee"-Window.
    /// </summary>
    internal class UpdateEmployeeCommand : ICommand
    {
        private IUpdateEmployeeContent mCreateCommandViewModel;

        public UpdateEmployeeCommand(IUpdateEmployeeContent model)
        {
            mCreateCommandViewModel = model;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            var values = parameter as object[];
            if(values != null && values.Count() > 2 && values[1] is string && values[2] is int)
            {
                if (string.IsNullOrEmpty(values[1] as string) || (int)values[2] == 0)
                {
                    return false; // if invalid values, not possible to add
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return true; // at first start..
            }
        }

        public void Execute(object parameter)
        {
            // update the employee and close the dialog
            mCreateCommandViewModel.UpdateEmployee();
            mCreateCommandViewModel.CloseAction();
        }

        public void FireCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
            {
                CanExecuteChanged(this, EventArgs.Empty);
            }
        }
    }
}
