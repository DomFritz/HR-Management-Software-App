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
    public partial class CreateEmployee : Window
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
            }
            else
            {
                mButtonSaveClose.Content = "Mitarbeiter aktualisieren";
                Title = "Mitarbeiter bearbeiten";

                mTextBoxID.Text = employeeToUpdate.Id.ToString();
                mTextBoxAge.Text = employeeToUpdate.Age.ToString();
                mTextBoxFirstName.Text = employeeToUpdate.FirstName;
                mTextBoxLastName.Text = employeeToUpdate.LastName;
                mTextBoxCity.Text = employeeToUpdate.Addresses.City;
                mTextBoxState.Text = employeeToUpdate.Addresses.State;
                mTextBoxZip.Text = employeeToUpdate.Addresses.Zip.ToString();
                mTextBoxStreet.Text = employeeToUpdate.Addresses.Street;
            }
        }

        private void mButtonSaveAndClose_Click(object sender, RoutedEventArgs e)
        {
            if(string.IsNullOrEmpty(mTextBoxAge.Text) || string.IsNullOrEmpty(mTextBoxLastName.Text) || string.IsNullOrEmpty(mTextBoxState.Text))
            {
                MessageBox.Show("Um einen Datensatz zu speichern, müssen mindestens die Felder Nachname, Alter und Staat ausgefüllt sein.");
                return;
            }

            int zip = 0;
            int age = 0;
            if(!Int32.TryParse(mTextBoxZip.Text, out zip))
            {
                MessageBox.Show("Das Feld PLZ hat keinen gültigen Wert.");
                return;
            }

            if (!Int32.TryParse(mTextBoxAge.Text, out age))
            {
                MessageBox.Show("Das Feld Alter hat keinen gültigen Wert.");
                return;
            }

            WcfEmployeeDatabaseServiceClient serviceClient = new WcfEmployeeDatabaseServiceClient();

            if (mMode == DialogMode.Create)
            {
                Address address = new Address()
                {
                    Id = Guid.NewGuid(),
                    State = mTextBoxState.Text,
                    Street = mTextBoxStreet.Text,
                    Zip = zip,
                    City = mTextBoxCity.Text
                };

                Employee newCreatedEmployee = new Employee()
                {
                    Id = Guid.NewGuid(),
                    FirstName = mTextBoxFirstName.Text,
                    LastName = mTextBoxLastName.Text,
                    Age = age,
                    Addresses = address
                };

                int retVal = serviceClient.InsertEmployeeAndAddressIfNotAvailable(newCreatedEmployee);
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
                    MessageBox.Show("Es konnte keine Datenbankverbindung aufgebaut werden.");
                }
            }
            else
            {
                mEmployeeToUpdate.Age = age;
                mEmployeeToUpdate.FirstName = mTextBoxFirstName.Text;
                mEmployeeToUpdate.LastName = mTextBoxLastName.Text;
                mEmployeeToUpdate.Addresses.City = mTextBoxCity.Text;
                mEmployeeToUpdate.Addresses.State = mTextBoxState.Text;
                mEmployeeToUpdate.Addresses.Zip = zip;
                mEmployeeToUpdate.Addresses.Street = mTextBoxStreet.Text;

                bool retVal = serviceClient.UpdateEmployeeAndAddress(mEmployeeToUpdate);
                if (retVal)
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
    }
}
