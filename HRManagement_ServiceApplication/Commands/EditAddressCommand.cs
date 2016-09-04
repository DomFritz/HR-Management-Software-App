using HRManagement_ServiceApplication_HRItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HRManagement_ServiceApplication.Commands
{
    internal interface IEditAddressContent
    {
        void EditAddress();

        Address SelectedAddress { get; }
    }

    /// <summary>
    /// The command to edit an address.
    /// </summary>
    internal class EditAddressCommand : ICommand
    {
        private IEditAddressContent mCreateEmployeeViewModel;

        public EditAddressCommand(IEditAddressContent viewModel)
        {
            mCreateEmployeeViewModel = viewModel;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            // if an address is selected, the command can be started.
            if(mCreateEmployeeViewModel.SelectedAddress != null)
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
            mCreateEmployeeViewModel.EditAddress();
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
