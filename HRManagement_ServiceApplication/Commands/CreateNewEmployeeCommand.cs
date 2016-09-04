using System;
using System.Windows.Input;

namespace HRManagement_ServiceApplication.Commands
{
    internal interface ICreateNewEmployeeContent
    {
        void CreateNewEmployee();
    }

    /// <summary>
    /// Command to create a new employee. The "Create Employee"-Window will be called with mode Create.
    /// </summary>
    internal class CreateNewEmployeeCommand : ICommand
    {
        private ICreateNewEmployeeContent mEmployeeOverviewViewModel;

        public CreateNewEmployeeCommand(ICreateNewEmployeeContent employeeModel)
        {
            mEmployeeOverviewViewModel = employeeModel;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true; // this can be done everytime
        }

        public void Execute(object parameter)
        {
            mEmployeeOverviewViewModel.CreateNewEmployee();
        }
    }
}
