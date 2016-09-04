using HRManagement_ServiceApplication.WcfEmployeeDatabaseService;
using System;
using System.Windows.Input;

namespace HRManagement_ServiceApplication.Commands
{
    internal interface IRemoveAddressContent
    {
        Address SelectedAddress { get; }
        void RemoveAddress();
    }

    /// <summary>
    /// Remove the address of the data grid.
    /// </summary>
    internal class RemoveAddressCommand : ICommand
    {
        private IRemoveAddressContent mCreateEmployeeViewModel;

        public RemoveAddressCommand(IRemoveAddressContent model)
        {
            mCreateEmployeeViewModel = model;
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
            // command can only be executed if a address is selected in the DataGrid
            if (mCreateEmployeeViewModel.SelectedAddress != null)
            {
                return true;
            }

            return false;
        }

        public void Execute(object parameter)
        {
            // remove the address of the DataGrid
            mCreateEmployeeViewModel.RemoveAddress();
        }
    }
}
