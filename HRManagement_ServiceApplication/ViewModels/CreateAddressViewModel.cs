using HRManagement_ServiceApplication.Commands;
using HRManagement_ServiceApplication_HRItems;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HRManagement_ServiceApplication
{
    public interface IAddOrUpdateAddressContent
    {
        void AddOrUpdateAddress(Address a);
    }

    public class CreateAddressViewModel : INotifyPropertyChanged, IAddSaveAddressContent, IUpdateableAddressContent
    {
        private Address mAddress;
        private string mStreet;
        private string mCity;
        private int mZip;
        private string mState;

        private DialogMode mMode;
        private IAddOrUpdateAddressContent mEmployeeViewModel;

        public CreateAddressViewModel(IAddOrUpdateAddressContent employeeViewModel, Address a, DialogMode mode, Guid employeeId)
        {
            mMode = mode;

            if (mode == DialogMode.Create)
            {
                this.Address = a != null ? a : new Address()
                {
                    Id = Guid.NewGuid(),
                    EmployeeId = employeeId
                };

                mAddSaveAddressCommand = new AddSaveAddressCommand(this);
            }
            else
            {
                this.Address = a;
                mAddSaveAddressCommand = new UpdateAddressCommand(this);
            }

            mEmployeeViewModel = employeeViewModel;

            this.City = mAddress.City;
            this.Street = mAddress.Street;
            this.State = mAddress.State;
            this.Zip = mAddress.Zip;
        }

        #region Data Binding-Properties

        public string Street
        {
            get
            {
                return mStreet;
            }
            set
            {
                mStreet = value;
                OnPropertyChanged();
            }
        }

        public string City
        {
            get
            {
                return mCity;
            }
            set
            {
                mCity = value;
                OnPropertyChanged();
            }
        }

        public int Zip
        {
            get
            {
                return mZip;
            }
            set
            {
                mZip = value;
                OnPropertyChanged();
                FireCanExecuteChanged();
            }
        }

        public string State
        {
            get
            {
                return mState;
            }
            set
            {
                mState = value;
                OnPropertyChanged();
                FireCanExecuteChanged();
            }
        }

        public Address Address
        {
            get
            {
                return mAddress;
            }
            set
            {
                mAddress = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Commands

        private ICommand mAddSaveAddressCommand;

        public Action CloseAction
        {
            get;
            set;
        }

        public ICommand AddSaveAddressCommand
        {
            get
            {
                return mAddSaveAddressCommand;
            }
            set
            {
                mAddSaveAddressCommand = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region IAddSaveAddressContent

        /// <summary>
        /// Update or Add the address in the database.
        /// </summary>
        /// <param name="a"></param>
        public void UpdateOrAddAddress(Address a)
        {
            mEmployeeViewModel.AddOrUpdateAddress(a);
        }

        #endregion

        #region Eventhandlers and INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void FireOnCheckPlzField()
        {
            FireCanExecuteChanged();
        }

        private void FireCanExecuteChanged()
        {
            if (mMode == DialogMode.Create)
            {
                ((AddSaveAddressCommand)mAddSaveAddressCommand).FireCanExecuteChanged();
            }
            else
            {
                ((UpdateAddressCommand)mAddSaveAddressCommand).FireCanExecuteChanged();
            }
        }

        #endregion
    }
}
