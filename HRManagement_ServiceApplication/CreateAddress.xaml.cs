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
    /// <summary>
    /// Interaction logic for CreateAddress.xaml
    /// </summary>
    public partial class CreateAddress : Window
    {
        private Guid mEmployeeId = Guid.Empty;
        private DialogMode mMode;
        private ICreatedAddressesContent mCreatedAddressView;
        private Address mAddressToUpdate;

        public CreateAddress(DialogMode mode, Guid employeeId, ICreatedAddressesContent createAddressView, Address addressToUpdate = null)
        {
            InitializeComponent();
            mEmployeeId = employeeId;
            mMode = mode;
            mCreatedAddressView = createAddressView;
            mAddressToUpdate = addressToUpdate;

            if (mode == DialogMode.Update)
            {
                Title = "Addresse bearbeiten";
                mButtonAddAddress.Content = "Änderungen übernehmen";

                mTextBoxCity.Text = addressToUpdate.City;
                mTextBoxState.Text = addressToUpdate.State;
                mTextBoxStreet.Text = addressToUpdate.Street;
                mTextBoxZip.Text = addressToUpdate.Zip.ToString();
            }
        }

        public List<Address> GetCreatedAddresses()
        {
            throw new NotImplementedException();
        }

        private void mButtonAddAddress_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(mTextBoxState.Text) || string.IsNullOrEmpty(mTextBoxZip.Text))
            {
                MessageBox.Show("Um einen Datensatz zu speichern, müssen mindestens die Felder Staat und PLZ ausgefüllt sein.");
                return;
            }

            int zip = 0;
            if (!Int32.TryParse(mTextBoxZip.Text, out zip))
            {
                MessageBox.Show("Der Wert im Feld PLZ ist ungültig.");
                return;
            }

            WcfEmployeeDatabaseServiceClient serviceClient = new WcfEmployeeDatabaseServiceClient();

            if (mMode == DialogMode.Create)
            {
                Address newAddress = new Address()
                {
                    Id = Guid.NewGuid(),
                    Street = mTextBoxStreet.Text,
                    City = mTextBoxCity.Text,
                    State = mTextBoxState.Text,
                    Zip = zip
                };

                mCreatedAddressView.UpdateAddressesListView(newAddress);
                this.Close();
            }
            else
            {
                mAddressToUpdate.City = mTextBoxCity.Text;
                mAddressToUpdate.State = mTextBoxState.Text;
                mAddressToUpdate.Street = mTextBoxStreet.Text;
                mAddressToUpdate.Zip = zip;
                mAddressToUpdate.EmployeeId = mEmployeeId;

                int retVal = serviceClient.UpdateAddress(mAddressToUpdate, mEmployeeId);
                if (retVal == 0)
                {
                    MessageBox.Show("Die Änderungen konnten nicht übernommen werden. Bitte überprüfen Sie die angegebenen Daten.");
                }
                else if(retVal == -1)
                {
                    MessageBox.Show("Es besteht ein Problem mit der Datenbank. Bitte prüfen Sie diese und die Verbindung");
                }

                mCreatedAddressView.UpdateAddressesListView(mAddressToUpdate);
                this.Close();
            }
        }
    }
}
