using HRManagement_ServiceApplication_HRItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HRManagement_ServiceApplication.Commands
{
    internal interface IDeleteEmployeeContent
    {
        void DeleteEmployee(Employee e);
    }

    /// <summary>
    /// Command to delete an employee.
    /// </summary>
    internal class DeleteEmployeeCommand : ICommand
    {
        private IDeleteEmployeeContent mOverviewViewModel;

        public DeleteEmployeeCommand(IDeleteEmployeeContent model)
        {
            mOverviewViewModel = model;
        }

        public void FireCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
            {
                CanExecuteChanged(this, EventArgs.Empty);
            }
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            // if an employee is selected, he can be deleted
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
            mOverviewViewModel.DeleteEmployee(parameter as Employee);
        }
    }
}
