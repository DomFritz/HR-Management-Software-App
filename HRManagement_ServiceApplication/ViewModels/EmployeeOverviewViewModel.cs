using HRManagement_ServiceApplication.Commands;
using HRManagement_ServiceApplication.WcfEmployeeDatabaseService;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace HRManagement_ServiceApplication
{
    public class EmployeeOverviewViewModel : INotifyPropertyChanged, IUpdateableEmployeeContent, ICreateNewEmployeeContent, IDeleteEmployeeContent, IEditEmployeeContent, IXMLExportContent
    {
        private DispatcherTimer mCheckDatabaseDispatcherTimer = new DispatcherTimer();
        private WcfEmployeeDatabaseServiceClient mCheckDatabaseConnectionService = null;

        private bool mDatabaseCheckboxState;
        private string mDatabaseCheckboxContent;
        private ObservableCollection<Employee> mAllEmployees;
        private object mSelectedItem;

        public EmployeeOverviewViewModel()
        {
            mDatabaseCheckboxState = false;
            mDatabaseCheckboxContent = string.Empty;
            mAllEmployees = new ObservableCollection<Employee>();

            RefreshButtonCommand = new RefreshButtonCommand(this);
            CreateNewEmployeeCommand = new CreateNewEmployeeCommand(this);
            EditEmployeeCommand = new EditEmployeeCommand(this);
            DeleteEmployeeCommand = new DeleteEmployeeCommand(this);
            XMLExportCommand = new XMLExportCommand(this);

            CheckDatabaseConnection();
            UpdateEmployeeList(); // read the available employees and write them into the datagrid.

            mCheckDatabaseDispatcherTimer.Tick += new EventHandler(DispatcherTimer_Tick);
            mCheckDatabaseDispatcherTimer.Interval = new TimeSpan(0, 0, 5);
            mCheckDatabaseDispatcherTimer.Start(); // check every 5 seconds if the database is available and can be connected
        }

        /// <summary>
        /// Gets the application name of the service reference.
        /// </summary>
        internal static string ApplicationName
        {
            get
            {
                return new WcfEmployeeDatabaseServiceClient().GetApplicationName();
            }
        }

        #region Data Binding-Properties

        public object SelectedItem
        {
            get
            {
                return mSelectedItem;
            }
            set
            {
                mSelectedItem = value;
                OnPropertyChanged();
                mEditEmployeeCommand.FireCanExecuteChanged();
                mDeleteEmployeeCommand.FireCanExecuteChanged();
            }
        }

        public ObservableCollection<Employee> AllEmployees
        {
            get
            {
                return mAllEmployees;
            }
            set
            {
                mAllEmployees = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Commands

        private RefreshButtonCommand mRefreshButtonCommand;
        private CreateNewEmployeeCommand mCreateNewEmployeeCommand;
        private EditEmployeeCommand mEditEmployeeCommand;
        private DeleteEmployeeCommand mDeleteEmployeeCommand;
        private XMLExportCommand mXmlExportCommand;

        public ICommand RefreshButtonCommand
        {
            get
            {
                return mRefreshButtonCommand;
            }
            set
            {
                mRefreshButtonCommand = value as RefreshButtonCommand;
                OnPropertyChanged();
            }
        }

        public ICommand CreateNewEmployeeCommand
        {
            get
            {
                return mCreateNewEmployeeCommand;
            }
            set
            {
                mCreateNewEmployeeCommand = value as CreateNewEmployeeCommand;
                OnPropertyChanged();
            }
        }

        public ICommand EditEmployeeCommand
        {
            get
            {
                return mEditEmployeeCommand;
            }
            set
            {
                mEditEmployeeCommand = value as EditEmployeeCommand;
                OnPropertyChanged();
            }
        }

        public ICommand DeleteEmployeeCommand
        {
            get
            {
                return mDeleteEmployeeCommand;
            }
            set
            {
                mDeleteEmployeeCommand = value as DeleteEmployeeCommand;
                OnPropertyChanged();
            }
        }

        public ICommand XMLExportCommand
        {
            get
            {
                return mXmlExportCommand;
            }
            set
            {
                mXmlExportCommand = value as XMLExportCommand;
                OnPropertyChanged();
            }

        }

        #endregion

        #region Database-Check

        public bool DatabaseCheckboxState
        {
            get
            {
                return mDatabaseCheckboxState;
            }
            set
            {
                mDatabaseCheckboxState = value;
                OnPropertyChanged();
            }
        }

        public string DatabaseCheckboxContent
        {
            get
            {
                return mDatabaseCheckboxContent;
            }
            set
            {
                mDatabaseCheckboxContent = value;
                OnPropertyChanged();
            }
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            CheckDatabaseConnection();
        }

        private void CheckDatabaseConnection()
        {
            if (mCheckDatabaseConnectionService == null)
            {
                mCheckDatabaseConnectionService = new WcfEmployeeDatabaseServiceClient();
            }

            if (mCheckDatabaseConnectionService.CheckDatabaseAvailability())
            {
                DatabaseCheckboxState = false;
                DatabaseCheckboxContent = "Die Verbindung zur Datenbank wurde erfolgreich aufgebaut.";
            }
            else
            {
                DatabaseCheckboxState = true;
                DatabaseCheckboxContent = "Es kann keine Verbindung zur Datenbank aufgebaut werden.";
            }
        }

        #endregion

        #region IUpdateableEmployeeContent

        /// <summary>
        /// This method reads all the employees of the database and adds them to the Observable collection, which is the data binding property of the data grid for the employees.
        /// </summary>
        public void UpdateEmployeeList()
        {
            WcfEmployeeDatabaseServiceClient client = new WcfEmployeeDatabaseServiceClient();
            var employees = client.ReadAllEmployees(); // read all employees of the database

            mAllEmployees.Clear();
            foreach (var item in employees)
            {
                AllEmployees.Add(item);
            }

            mXmlExportCommand.FireCanExecuteChanged();
        }

        #endregion

        #region ICreateNewEmployeeContent

        /// <summary>
        /// This method calls the CreateEmployee-Window in Create-Mode to create a new employee.
        /// </summary>
        public void CreateNewEmployee()
        {
            new CreateEmployee(this, DialogMode.Create).Show();
        }

        #endregion

        #region IEditEmployeeContent

        /// <summary>
        /// This method calls the CreateEmployee-Window in Update-Mode to update the employee.
        /// </summary>
        public void EditEmployee()
        {
            var selectedItem = this.SelectedItem as Employee;
            if(selectedItem != null)
            {
                new CreateEmployee(this, DialogMode.Update, selectedItem).Show();
            }
        }

        #endregion

        #region IDeleteEmployeeContent

        /// <summary>
        /// This method removes the selected employee, but before this action, all addresses for this employee will be deleted.
        /// </summary>
        /// <param name="employee"></param>
        public void DeleteEmployee(Employee employee)
        {
            var result = MessageBox.Show("Wollen Sie den gewählten Datensatz und die dazugehörige(n) Adresse(n) wirklich löschen?", "Adresse wirklich löschen", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.No)
            {
                return;
            }
            else
            {
                WcfEmployeeDatabaseServiceClient client = new WcfEmployeeDatabaseServiceClient();

                foreach (var address in employee.Addresses)
                {
                    if(client.DeleteAddress(address) == -1) // delete all addresses before deleting the employee 
                    {
                        MessageBox.Show("Es ist ein Fehler beim Löschen der zugehörigen Addressen aufgetreten.", "Addresse löschen", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                }

                if (client.DeleteEmployee(employee) != 1)
                {
                    MessageBox.Show("Der Mitarbeiter konnte nicht gelöscht werden.", "Mitarbeiter nicht gelöscht", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                this.UpdateEmployeeList();
            }
        }

        #endregion

        #region IXMLExportContent

        /// <summary>
        /// Connect to the database, read all the employees out and if there are employees available (if not, the button can't be pressed), the employees will be exported as xml.
        /// </summary>
        public void DoXMLExport()
        {
            WcfEmployeeDatabaseServiceClient client = new WcfEmployeeDatabaseServiceClient();
            var employees = client.ReadAllEmployees();

            string outputPath = EmployeeXMLWriter.WriteEmployeeXML(employees.ToList());

            MessageBox.Show(string.Format("Die Mitarbeiter wurden unter folgendem Pfad exportiert: '{0}'.", outputPath), "Alle Mitarbeiter exportiert", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        #endregion

        #region INotifyPropertyHandler

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
