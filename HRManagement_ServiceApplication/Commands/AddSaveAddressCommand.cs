using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using HRManagement_ServiceApplication.WcfEmployeeDatabaseService;

namespace HRManagement_ServiceApplication.Commands
{
    public interface ICloseable
    {
        Action CloseAction { get; }
    }

    public interface IAddSaveAddressContent : ICloseable
    {
        void UpdateOrAddAddress(Address address);
        Address Address { get; }
    }

    /// <summary>
    /// The command to add and save an new created address.
    /// </summary>
    internal class AddSaveAddressCommand : ICommand
    {
        private IAddSaveAddressContent mCreateAddressViewModel;
        private Address mAddress;

        public AddSaveAddressCommand(IAddSaveAddressContent createAddressViewModel)
        {
            this.mCreateAddressViewModel = createAddressViewModel;
            this.mAddress = createAddressViewModel.Address;
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
            // only if plz and state are valid
            var values = (object[])parameter;

            if (!(values[2] is int) || !(values[3] is string))
            {
                return false;
            }

            int plz = (int)values[2];
            string state = (string)values[3];

            if (plz <= 0 || string.IsNullOrEmpty(state))
            {
                return false;
            }

            return true;
        }

        public void Execute(object parameter)
        {
            var values = (object[])parameter;
            string street = values[0] as string;
            string city = values[1] as string;
            int zip = (int)values[2];
            string state = values[3] as string;

            mAddress.Street = street;
            mAddress.City = city;
            mAddress.Zip = zip;
            mAddress.State = state;

            mCreateAddressViewModel.UpdateOrAddAddress(mAddress);
            mCreateAddressViewModel.CloseAction();
        }
    }
}
