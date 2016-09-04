using HRManagement_ServiceApplication.Commands;
using HRManagement_ServiceApplication.WcfEmployeeDatabaseService;
using HRManagement_ServiceApplication_HRItems;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace HRManagement_ServiceApplication
{
    public enum DialogMode
    {
        Create = 0,
        Update = 1
    }

    /// <summary>
    /// The view model for the "Create Employee"-Window.
    /// </summary>
    public class CreateEmployeeViewModel : INotifyPropertyChanged, IEditAddressContent, IRemoveAddressContent, ISaveEmployeeContent, IShowAddressContent, IUpdateEmployeeContent, IAddOrUpdateAddressContent
    {
        private Guid mId;
        private string mFirstName = string.Empty;
        private string mLastName = string.Empty;
        private int mAge;
        private ObservableCollection<Address> mAddresses;

        private DialogMode mMode;
        private Employee mEmployee;
        private Address mSelectedAddress;

        private IUpdateableEmployeeContent mEmployeeViewModel;

        public CreateEmployeeViewModel(IUpdateableEmployeeContent employeeViewModel, Employee e, DialogMode mode)
        {
            mMode = mode;
            mEmployee = e;
            mEmployeeViewModel = employeeViewModel;

            if (mode == DialogMode.Create)
            {
                SaveCommand = new SaveEmployeeCommand(this);
            }
            else
            {
                SaveCommand = new UpdateEmployeeCommand(this);
            }

            AddAddressCommand = new ShowAddressControlCommand(this);
            RemoveAddressCommand = new RemoveAddressCommand(this);
            EditAddressCommand = new EditAddressCommand(this);

            mId = e.Id;
            FirstName = e.FirstName;
            LastName = e.LastName;
            Age = e.Age;
            Addresses = new ObservableCollection<Address>();
            foreach (var item in e.Addresses)
            {
                Addresses.Add(item);
            }
        }

        #region Commands

        private ICommand mSaveCommand;
        private ShowAddressControlCommand mAddAddressCommand;
        private RemoveAddressCommand mRemoveAddressCommand;
        private EditAddressCommand mEditAddressCommand;

        public ICommand AddAddressCommand
        {
            get
            {
                return mAddAddressCommand;
            }
            set
            {
                mAddAddressCommand = value as ShowAddressControlCommand;
                OnPropertyChanged();
            }
        }

        public ICommand SaveCommand
        {
            get
            {
                return mSaveCommand;
            }
            set
            {
                mSaveCommand = value;
                OnPropertyChanged();
            }
        }

        public ICommand EditAddressCommand
        {
            get
            {
                return mEditAddressCommand;
            }
            set
            {
                mEditAddressCommand = value as EditAddressCommand;
                OnPropertyChanged();
            }
        }

        public ICommand RemoveAddressCommand
        {
            get
            {
                return mRemoveAddressCommand;
            }
            set
            {
                mRemoveAddressCommand = value as RemoveAddressCommand;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Data Binding-Propertes

        public Guid Id
        {
            get { return mId; }
        }

        public string FirstName
        {
            get
            {
                return mFirstName;
            }
            set
            {
                mFirstName = value;
                OnPropertyChanged();
            }
        }

        public Action CloseAction
        {
            get;
            set;
        }

        public string LastName
        {
            get
            {
                return mLastName;
            }
            set
            {
                mLastName = value;
                OnPropertyChanged();
                if (mMode == DialogMode.Create)
                {
                    ((SaveEmployeeCommand)mSaveCommand).FireCanExecuteChanged();
                }
                else
                {
                    ((UpdateEmployeeCommand)mSaveCommand).FireCanExecuteChanged();
                }
            }
        }

        public int Age
        {
            get
            {
                return mAge;
            }
            set
            {
                mAge = value;
                OnPropertyChanged();
                if (mMode == DialogMode.Create)
                {
                    ((SaveEmployeeCommand)mSaveCommand).FireCanExecuteChanged();
                }
                else
                {
                    ((UpdateEmployeeCommand)mSaveCommand).FireCanExecuteChanged();
                }
            }
        }

        public ObservableCollection<Address> Addresses
        {
            get
            {
                return mAddresses;
            }
            set
            {
                mAddresses = value;
                OnPropertyChanged();
                if (mMode == DialogMode.Create)
                {
                    ((SaveEmployeeCommand)mSaveCommand).FireCanExecuteChanged();
                }
                else
                {
                    ((UpdateEmployeeCommand)mSaveCommand).FireCanExecuteChanged();
                }
            }
        }

        public Address SelectedAddress
        {
            get
            {
                return mSelectedAddress;
            }
            set
            {
                mSelectedAddress = value;
                OnPropertyChanged();
                mEditAddressCommand.FireCanExecuteChanged();
                mRemoveAddressCommand.FireCanExecuteChanged();
            }
        }

        #endregion

        #region ISaveEmployeeContent

        /// <summary>
        /// This method saves the new created employee with the addresses into the database. 
        /// </summary>
        /// <param name="e">The employee to insert.</param>
        public void SaveEmployee(Employee e)
        {
            WcfEmployeeDatabaseServiceClient serviceClient = new WcfEmployeeDatabaseServiceClient();

            List<Address> createdAddresses = new List<Address>();
            foreach (Address address in mAddresses)
            {
                createdAddresses.Add(address);
            }

            Employee newCreatedEmployee = new Employee()
            {
                Id = Guid.NewGuid(),
                FirstName = mFirstName,
                LastName = mLastName,
                Age = mAge,
                Addresses = createdAddresses
            };

            int retVal = serviceClient.InsertEmployeeAndAddresses(newCreatedEmployee);
            if (retVal == 1)
            {
                MessageBox.Show("Der Mitarbeiter wurde erfolgreich hinzugefügt.", "Mitarbeiter hinzugefügt", MessageBoxButton.OK, MessageBoxImage.Information);
                mEmployeeViewModel.UpdateEmployeeList();
            }
            else if (retVal == 0)
            {
                MessageBox.Show("Der Mitarbeiter konnte nicht hinzugefügt werden", "Mitarbeiter nicht hinzugefügt", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (retVal == -1)
            {
                MessageBox.Show("Es besteht ein Problem mit der Datenbank. Bitte prüfen Sie diese und die Verbindung", "Mitarbeiter nicht hinzugefügt", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion

        #region ShowAddressContent

        /// <summary>
        /// This method starts the "Adresse hinzufügen"-Window mit dem gewünschten Modus (Update oder Create).
        /// </summary>
        /// <param name="mode">The mode (1 or 0)</param>
        /// <param name="a">The address.</param>
        public void StartAddAddressControl(DialogMode mode = DialogMode.Create, Address a = null)
        {
            new CreateAddress(this, mode, Guid.Parse(mId.ToString()), a).Show();
        }

        #endregion

        #region IAddOrUpdateAddressContent

        /// <summary>
        /// This method adds the address to the data grid if not available or updates the available value.
        /// </summary>
        /// <param name="a">The Address to add or update.</param>
        public void AddOrUpdateAddress(Address a)
        {
            if (mAddresses.Contains(a))
            {
                var address = mAddresses.FirstOrDefault(x => x.Id.Equals(a.Id));
                int index = mAddresses.IndexOf(address);
                address.Street = a.Street;
                address.Zip = a.Zip;
                address.State = a.State;
                address.City = a.City;

                mAddresses.Remove(address);
                mAddresses.Insert(index, address);
            }
            else
            {
                mAddresses.Add(a);
            }
            OnPropertyChanged();
            mRemoveAddressCommand.FireCanExecuteChanged();
        }

        #endregion

        #region IUpdateEmployeeContent

        /// <summary>
        /// This method calls the service client to update the changed employee.
        /// </summary>
        public void UpdateEmployee()
        {
            List<Address> createdAddresses = new List<Address>();
            foreach (Address address in mAddresses)
            {
                createdAddresses.Add(address);
            }

            mEmployee.Age = Age;
            mEmployee.FirstName = mFirstName;
            mEmployee.LastName = mLastName;
            mEmployee.Addresses = createdAddresses;

            WcfEmployeeDatabaseServiceClient serviceClient = new WcfEmployeeDatabaseServiceClient();
            serviceClient.DeleteAddressesOfEmployee(mEmployee.Id); // delete all addresses before adding all created ones.
            int retVal = serviceClient.UpdateEmployeeAndAddresses(mEmployee);
            if (retVal > 0)
            {
                MessageBox.Show("Der Datensatz wurde erfolgreich aktualisiert.");
                mEmployeeViewModel.UpdateEmployeeList();
            }
            else
            {
                MessageBox.Show("Es gab ein Problem beim Aktualisieren der Daten. Bitte prüfen Sie die eingegebenen Daten auf ihre Korrektheit.");
            }
        }

        #endregion

        #region IRemoveAddressContent

        /// <summary>
        /// This method removes the selected address of the data grid.
        /// </summary>
        public void RemoveAddress()
        {
            if(MessageBox.Show("Wollen Sie die gewählte Addresse wirklich löschen?", "Addresse löschen", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
            {
                return;
            }

            mAddresses.Remove(SelectedAddress);
            mRemoveAddressCommand.FireCanExecuteChanged();
        }

        #endregion

        #region IEditAddressContent

        /// <summary>
        /// This method starts the "Adresse erstellen"-Window in the Update-Mode.
        /// </summary>
        public void EditAddress()
        {
            new CreateAddress(this, DialogMode.Update, mId, SelectedAddress).Show();
        }

        #endregion

        #region Event-Handling and INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void FireOnCheckAgeField()
        {
            if (mMode == DialogMode.Create)
            {
                ((SaveEmployeeCommand)mSaveCommand).FireCanExecuteChanged();
            }
            else
            {
                ((UpdateEmployeeCommand)mSaveCommand).FireCanExecuteChanged();
            }
        }

        #endregion
    }
}
