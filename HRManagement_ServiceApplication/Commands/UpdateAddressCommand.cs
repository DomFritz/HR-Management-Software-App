using HRManagement_ServiceApplication_HRItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HRManagement_ServiceApplication.Commands
{
    internal interface IUpdateableAddressContent : ICloseable
    {
        Address Address { get; }
        int Zip { get; }
        string State { get; }

        string Street { get; }

        string City { get; }
        void UpdateOrAddAddress(Address a);
    }

    /// <summary>
    /// The command to update an existing address.
    /// </summary>
    internal class UpdateAddressCommand : ICommand
    {
        private IUpdateableAddressContent mCreateAddressViewModel;
        private Address mAddress;

        public UpdateAddressCommand(IUpdateableAddressContent viewModel)
        {
            mCreateAddressViewModel = viewModel;
            mAddress = viewModel.Address;
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
            if(mCreateAddressViewModel.Zip > 0 && !string.IsNullOrEmpty(mCreateAddressViewModel.State))
            {
                return true;
            }

            return false;
        }

        public void Execute(object parameter)
        {
            mAddress.Street = mCreateAddressViewModel.Street;
            mAddress.City = mCreateAddressViewModel.City;
            mAddress.Zip = mCreateAddressViewModel.Zip;
            mAddress.State = mCreateAddressViewModel.State;

            mCreateAddressViewModel.UpdateOrAddAddress(mAddress);
            mCreateAddressViewModel.CloseAction();
        }
    }
}
