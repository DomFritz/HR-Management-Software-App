using HRManagement_ServiceApplication.WcfEmployeeDatabaseService;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HRManagement_ServiceApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class EmployeeOverviewWindow : Window, IUpdateableEmployeeContent
    {
        /// <summary>
        /// The main window of the application.
        /// </summary>
        public EmployeeOverviewWindow()
        {
            InitializeComponent();
            SetPictureOfRefreshButton();

            UpdateEmployeeList();
            Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose; // shut down the application when the main window is closed.
        }

        internal static string ApplicationName
        {
            get
            {
                return new WcfEmployeeDatabaseServiceClient().GetApplicationName();
            }
        }

        private void SetPictureOfRefreshButton()
        {
            Image img = new Image();
            img.Source = new BitmapImage(new Uri(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Images\\RefreshButton.png")));
            img.Stretch = Stretch.Uniform;

            StackPanel stackPnl = new StackPanel();
            stackPnl.Orientation = Orientation.Horizontal;
            stackPnl.Children.Add(img);

            mRefreshButton.Content = stackPnl;
        }

        private void mRefreshButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateEmployeeList();
        }

        /// <summary>
        /// This method updates the employee list in the list view after a change (if deleted, or edited)
        /// </summary>
        public void UpdateEmployeeList()
        {
            WcfEmployeeDatabaseServiceClient client = new WcfEmployeeDatabaseServiceClient();
            var employees = client.ReadAllEmployees(); // read all employees of the database

            this.mEmployeeView.Items.Clear();

            this.mEmployeeView.DataContext = employees;
            foreach (var item in employees)
            {
                this.mEmployeeView.Items.Add(item);
            }
        }

        private void mEmployeeView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender != null)
            {
                DataGridRow dgr = sender as DataGridRow;
                var employee = dgr.Item as Employee;

                if (employee != null)
                {
                    new CreateEmployee(DialogMode.Update, this, mEmployeeView.SelectedItem != null ? mEmployeeView.SelectedItem as Employee : employee).Show();
                }
            }
        }

        private void mEmployeeView_KeyDownEvent(object sender, KeyEventArgs e) // delete the selected entry when ENTF-Key is pressed.
        {
            if (mEmployeeView.SelectedItem == null)
            {
                return;
            }

            if(e.Key != Key.Delete)
            {
                return;
            }
            else
            {
                mButtonDeleteEmployee_Click(this, null);
            }
        }

        private void mButtonNewEmployee_Click(object sender, RoutedEventArgs e)
        {
            new CreateEmployee(DialogMode.Create, this).Show();
        }

        private void mButtonEditEmployee_Click(object sender, RoutedEventArgs e)
        {
            if (mEmployeeView.SelectedItem == null)
            {
                MessageBox.Show("Es wurde kein Mitarbeiter zum Bearbeiten selektiert.");
                return;
            }

            new CreateEmployee(DialogMode.Update, this, mEmployeeView.SelectedItem as Employee).Show();
        }

        private void mButtonDeleteEmployee_Click(object sender, RoutedEventArgs e)
        {
            if (mEmployeeView.SelectedItem == null)
            {
                MessageBox.Show("Es wurde kein Mitarbeiter zum Löschen selektiert.");
                return;
            }

            var result = MessageBox.Show("Wollen Sie den gewählten Datensatz und die dazugehörige Adresse (falls diese bei keinem anderen Mitarbeiter eingetragen ist) wirklich löschen?", string.Empty, MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.No)
            {
                return;
            }
            else
            {
                WcfEmployeeDatabaseServiceClient client = new WcfEmployeeDatabaseServiceClient();
                var employee = mEmployeeView.SelectedItem as Employee;

                foreach (var address in employee.Addresses)
                {
                    client.DeleteAddress(address); // delete all addresses before deleting the employee
                }

                if (client.DeleteEmployee(employee) != 1)
                {
                    MessageBox.Show("Der Mitarbeiter konnte nicht gelöscht werden.");
                }

                this.UpdateEmployeeList();
            }
        }

        private void mCreateXMLEmployees_Click(object sender, RoutedEventArgs e)
        {
            WcfEmployeeDatabaseServiceClient client = new WcfEmployeeDatabaseServiceClient();
            var employees = client.ReadAllEmployees();

            if (!employees.Any())
            {
                MessageBox.Show("Es sind keine Mitarbeiter vorhanden, die in eine XML-Datei geschrieben werden können.");
                return;
            }

            string outputPath = EmployeeXMLWriter.WriteEmployeeXML(employees.ToList());

            MessageBox.Show(string.Format("Die Mitarbeiter wurden unter folgendem Pfad exportiert: {0}.", outputPath));
        }
    }
}
