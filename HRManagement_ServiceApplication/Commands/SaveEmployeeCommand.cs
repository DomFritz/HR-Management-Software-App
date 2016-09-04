using HRManagement_ServiceApplication.Commands;
using HRManagement_ServiceApplication_HRItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HRManagement_ServiceApplication
{
    internal interface ISaveEmployeeContent : ICloseable
    {
        void SaveEmployee(Employee e);
    }

    /// <summary>
    /// The command to save the employee into the database. Only if valid values are given, this command can be executed.
    /// </summary>
    internal class SaveEmployeeCommand : ICommand
    {
        private ISaveEmployeeContent mViewModel;

        public SaveEmployeeCommand(ISaveEmployeeContent viewModel)
        {
            mViewModel = viewModel;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            var values = (object[])parameter;
            // check the parameters of the multi binding converter
            if(!(values[1] is string) || !(values[2] is int))
            {
                return false;
            }

            string lastName = (string)values[1];
            int age = (int)values[2];

            if(string.IsNullOrEmpty(lastName) || age <= 0)
            {
                return false;
            }

            return true;
        }

        public void Execute(object parameter)
        {
            // save the employee into the database...
            mViewModel.SaveEmployee(parameter as Employee);
            mViewModel.CloseAction();
        }

        public void FireCanExecuteChanged()
        {
            if(CanExecuteChanged != null)
            {
                CanExecuteChanged(this, EventArgs.Empty);
            }
        }
    }
}
