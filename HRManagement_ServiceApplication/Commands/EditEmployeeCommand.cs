using HRManagement_ServiceApplication.WcfEmployeeDatabaseService;
using System;
using System.Windows.Input;

namespace HRManagement_ServiceApplication.Commands
{
    internal interface IEditEmployeeContent
    {
        void EditEmployee();
    }

    /// <summary>
    /// Command to edit an employee
    /// </summary>
    internal class EditEmployeeCommand : ICommand
    {
        private IEditEmployeeContent mEmployeeOverviewViewModel;

        public EditEmployeeCommand(IEditEmployeeContent model)
        {
            mEmployeeOverviewViewModel = model;
        }

        public void FireCanExecuteChanged()
        {
            if(CanExecuteChanged != null)
            {
                CanExecuteChanged(this, EventArgs.Empty);
            }
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            // if an employee is selected, the command can be started, otherwise not
            if(parameter is Employee)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Execute(object parameter)
        {
            mEmployeeOverviewViewModel.EditEmployee();
        }
    }
}
