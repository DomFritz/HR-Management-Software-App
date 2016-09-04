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
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class CreateEmployee : Window
    {
        /// <summary>
        /// This dialog has two modes. It is used to create an employee and to edit an employee
        /// </summary>
        /// <param name="mode">The mode if we create or update an employee.</param>
        /// <param name="mainWindow">The main window, necessary to update the list view.</param>
        /// <param name="employeeToUpdate">The employee to update if we have update-mode, null if creation mode.</param>
        public CreateEmployee(IUpdateableEmployeeContent updateableContent, DialogMode mode, Employee employeeToUpdate = null)
        {
            InitializeComponent();

            Employee emp = employeeToUpdate != null ? employeeToUpdate : null; // if update, choose the employee of the parameters, otherwise create a new one

            if (mode == DialogMode.Create)
            {
                var newGuid = Guid.NewGuid(); // create a new guid for the employee
                mTextBoxID.Text = newGuid.ToString();
                mTextBoxFirstName.Focus();

                emp = new Employee()
                {
                    Id = newGuid,
                    Addresses = new List<Address>().ToArray()
                };
            }
            else
            {
                mButtonSaveClose.Content = "Änderungen übernehmen"; // other title and button name when updating
                Title = "Mitarbeiter bearbeiten";
            }

            var viewModel = new CreateEmployeeViewModel(updateableContent, emp, mode);
            if (viewModel.CloseAction == null)
            {
                viewModel.CloseAction = new Action(this.Close);
            }
            this.DataContext = viewModel;

            SetPicturesOfButtons();
        }
       

        private void SetPicturesOfButtons()
        {
            var directoryName = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            mButtonAddAddress.Content = ButtonImages.GetButtonContent(new Uri(System.IO.Path.Combine(directoryName, "Images\\Add.png")));
            mButtonRemoveAddress.Content = ButtonImages.GetButtonContent(new Uri(System.IO.Path.Combine(directoryName, "Images\\Remove.png")));
            mButtonEditAddress.Content = ButtonImages.GetButtonContent(new Uri(System.IO.Path.Combine(directoryName, "Images\\Edit.png")));
        }

        private void mTextBoxAge_GotFocus(object sender, RoutedEventArgs e)
        {
            if(mTextBoxAge.Text == "0") // if invalid age mark all in the textbox to overwrite it when typing in
            {
                mTextBoxAge.SelectAll();
            }
        }

        private void mTextBoxAge_KeyUp(object sender, KeyEventArgs e) // check after every keyup if it is a valid age - otherwise the submit-button stays disabled
        {
            var viewModel = this.DataContext as CreateEmployeeViewModel;

            if (viewModel != null)
            {
                if (string.IsNullOrEmpty(mTextBoxAge.Text))
                {
                    viewModel.Age = 0;
                    mTextBoxAge.SelectAll();
                }
                else
                {
                    int age = 0;
                    if(Int32.TryParse(mTextBoxAge.Text, out age))
                    {
                        viewModel.Age = age; // set age after every VALID entry
                    }
                    else
                    {
                        viewModel.Age = 0;
                        mTextBoxAge.SelectAll();
                    }
                }

                viewModel.FireOnCheckAgeField(); // check if the age-field is valid
            }
        }

        private void mAddressesView_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender != null)
            {
                DataGrid grid = sender as DataGrid;
                if (grid != null && grid.SelectedItems != null && grid.SelectedItems.Count == 1)
                {
                    DataGridRow dgr = grid.ItemContainerGenerator.ContainerFromItem(grid.SelectedItem) as DataGridRow;
                    if (!dgr.IsMouseOver)
                    {
                        (dgr as DataGridRow).IsSelected = false; // deselect if clicking in the "white empty" space of the datagrid
                    }
                }
            }
        }
    }
}
