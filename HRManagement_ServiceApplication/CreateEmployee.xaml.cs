using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using HRManagement_ServiceApplication.WcfEmployeeDatabaseService;
using System.Collections.ObjectModel;

namespace HRManagement_ServiceApplication
{
    public enum DialogMode
    {
        Create = 0,
        Update = 1
    }

    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class CreateEmployee : Window, ICreatedAddressesContent
    {
        private Guid mNewGuid;
        private IUpdateableEmployeeContent mUpdateableContent;
        private DialogMode mMode;
        private Employee mEmployeeToUpdate = null;

        /// <summary>
        /// This dialog has two modes. It is used to create an employee and to edit an employee
        /// </summary>
        /// <param name="mode">The mode if we create or update an employee.</param>
        /// <param name="mainWindow">The main window, necessary to update the list view.</param>
        /// <param name="employeeToUpdate">The employee to update if we have update-mode, null if creation mode.</param>
        public CreateEmployee(DialogMode mode, IUpdateableEmployeeContent mainWindow, Employee employeeToUpdate = null)
        {
            InitializeComponent();
            mUpdateableContent = mainWindow;
            mMode = mode;
            mEmployeeToUpdate = employeeToUpdate;

            if (mode == DialogMode.Create)
            {
                mNewGuid = Guid.NewGuid();
                mTextBoxID.Text = mNewGuid.ToString();
                mTextBoxFirstName.Focus();
            }
            else
            {
                mButtonSaveClose.Content = "Änderungen übernehmen";
                Title = "Mitarbeiter bearbeiten";

                // read the addresses for the employee
                WcfEmployeeDatabaseServiceClient serviceClient = new WcfEmployeeDatabaseServiceClient();
                var employeeAddresses = serviceClient.ReadAllAddresses(employeeToUpdate);
                this.mAddressesView.DataContext = employeeAddresses;
                foreach (var address in serviceClient.ReadAllAddresses(employeeToUpdate))
                {
                    mAddressesView.Items.Add(address);
                }

                mTextBoxID.Text = employeeToUpdate.Id.ToString();
                mTextBoxAge.Text = employeeToUpdate.Age.ToString();
                mTextBoxFirstName.Text = employeeToUpdate.FirstName;
                mTextBoxLastName.Text = employeeToUpdate.LastName;
            }
        }

        private void mButtonSaveAndClose_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(mTextBoxAge.Text) || string.IsNullOrEmpty(mTextBoxLastName.Text))
            {
                MessageBox.Show("Um einen Datensatz zu speichern, müssen mindestens die Felder Nachname und Alter ausgefüllt sein.");
                return;
            }

            int age = 0;
            if (!Int32.TryParse(mTextBoxAge.Text, out age))
            {
                MessageBox.Show("Das Feld Alter hat keinen gültigen Wert.");
                return;
            }

            WcfEmployeeDatabaseServiceClient serviceClient = new WcfEmployeeDatabaseServiceClient();

            if (mMode == DialogMode.Create)
            {
                List<Address> createdAddresses = new List<Address>();
                foreach (Address address in mAddressesView.Items)
                {
                    createdAddresses.Add(address);
                }

                Employee newCreatedEmployee = new Employee()
                {
                    Id = Guid.NewGuid(),
                    FirstName = mTextBoxFirstName.Text,
                    LastName = mTextBoxLastName.Text,
                    Age = age,
                    Addresses = createdAddresses.ToArray()
                };

                int retVal = serviceClient.InsertEmployeeAndAddresses(newCreatedEmployee);
                if (retVal == 1)
                {
                    MessageBox.Show("Der Mitarbeiter wurde erfolgreich hinzugefügt.");
                    mUpdateableContent.UpdateEmployeeList();
                    this.Close();
                }
                else if (retVal == 0)
                {
                    MessageBox.Show("Ein Mitarbeiter mit diesen Daten existiert bereits.");
                }
                else if (retVal == -1)
                {
                    MessageBox.Show("Es besteht ein Problem mit der Datenbank. Bitte prüfen Sie diese und die Verbindung");
                }
            }
            else
            {
                List<Address> createdAddresses = new List<Address>();
                foreach (Address address in mAddressesView.Items)
                {
                    createdAddresses.Add(address);
                }

                mEmployeeToUpdate.Age = age;
                mEmployeeToUpdate.FirstName = mTextBoxFirstName.Text;
                mEmployeeToUpdate.LastName = mTextBoxLastName.Text;
                mEmployeeToUpdate.Addresses = createdAddresses.ToArray();

                serviceClient.DeleteAddressesOfEmployee(mEmployeeToUpdate.Id); // delete all addresses before adding all created ones.
                int retVal = serviceClient.UpdateEmployeeAndAddresses(mEmployeeToUpdate);
                if (retVal > 0)
                {
                    MessageBox.Show("Der Datensatz wurde erfolgreich aktualisiert.");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Es gab ein Problem beim Aktualisieren der Daten. Bitte prüfen Sie die eingegebenen Daten auf ihre Korrektheit.");
                }
            }
        }

        private void mButtonAddAddress_Click(object sender, RoutedEventArgs e)
        {
            new CreateAddress(DialogMode.Create, Guid.Parse(mTextBoxID.Text), this).Show();
        }

        public void UpdateAddressesListView(Address address)
        {
            if (!mAddressesView.Items.Contains(address))
            {
                mAddressesView.Items.Add(address);
            }
            else
            {
                mAddressesView.Items.Remove(address);
                mAddressesView.Items.Add(address);
            }
        }

        private void mAddressesView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender != null)
            {
                DataGridRow dgr = sender as DataGridRow;
                var address = dgr.Item as Address;

                if (address != null)
                {
                    new CreateAddress(DialogMode.Update, Guid.Parse(mTextBoxID.Text), this, mAddressesView.SelectedItem != null ? mAddressesView.SelectedItem as Address : address).Show();
                }
            }
        }

        private void mAddressesView_KeyDownEvent(object sender, KeyEventArgs e) // delete the selected entry when ENTF-Key is pressed.
        {
            if (mAddressesView.SelectedItem == null)
            {
                return;
            }

            if (e.Key != Key.Delete)
            {
                return;
            }
            else
            {
                mButtonDeleteEmployee_Click(this, null);
            }
        }

        private void mButtonDeleteEmployee_Click(object sender, RoutedEventArgs e)
        {
            if (mAddressesView.SelectedItem == null)
            {
                MessageBox.Show("Es wurde keine Addresse zum Löschen selektiert.");
                return;
            }

            var result = MessageBox.Show("Wollen Sie den gewählten Datensatz wirklich löschen?", "Datensatz löschen", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.No)
            {
                return;
            }
            else
            {
                var address = mAddressesView.SelectedItem as Address;
                mAddressesView.Items.Remove(address);
            }
        }
    }
}
