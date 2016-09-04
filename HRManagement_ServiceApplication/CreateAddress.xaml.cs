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
        public CreateAddress(IAddOrUpdateAddressContent createAddressView, DialogMode mode, Guid employeeId, Address addressToUpdate = null)
        {
            InitializeComponent();

            if(mode == DialogMode.Create)
            {
                addressToUpdate = new Address() { Id = Guid.NewGuid(), EmployeeId = employeeId };
                mTextBoxStreet.Focus();
            }
            else
            {
                Title = "Addresse bearbeiten";
                mButtonAddAddress.Content = "Änderungen übernehmen";
            }

            var viewModel = new CreateAddressViewModel(createAddressView, addressToUpdate, mode, employeeId);
            if(viewModel.CloseAction == null)
            {
                viewModel.CloseAction = new Action(this.Close);
            }
            this.DataContext = viewModel;
        }

        private void mTextBoxZip_KeyUp(object sender, KeyEventArgs e)
        {
            var viewModel = this.DataContext as CreateAddressViewModel;
            if (viewModel != null)
            {
                if (string.IsNullOrEmpty(mTextBoxZip.Text))
                {
                    viewModel.Zip = 0;
                    mTextBoxZip.SelectAll();
                }
                else
                {
                    int age = 0;
                    if (Int32.TryParse(mTextBoxZip.Text, out age))
                    {
                        viewModel.Zip = age;
                    }
                    else
                    {
                        viewModel.Zip = 0;
                        mTextBoxZip.SelectAll();
                    }
                }

                viewModel.FireOnCheckPlzField();
            }
        }

        private void mTextBoxZip_GotFocus(object sender, RoutedEventArgs e)
        {
            if (mTextBoxZip.Text == "0") // if invalid age mark all in the textbox to overwrite it when typing in
            {
                mTextBoxZip.SelectAll();
            }
        }
    }
}
