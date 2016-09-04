using HRManagement_ServiceApplication_HRItems;
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
using System.Windows.Threading;

namespace HRManagement_ServiceApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class EmployeeOverviewWindow : Window
    {
        /// <summary>
        /// The main window of the application.
        /// </summary>
        public EmployeeOverviewWindow()
        {
            InitializeComponent();
            SetButtonPictures();

            Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose; // shut down the application when the main window is closed.
            this.DataContext = new EmployeeOverviewViewModel();
        }

        private void SetButtonPictures() // set the pictures of the buttons - the buttons will be committed under an image-folder
        {
            var directoryName = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            mButtonNewEmployee.Content = ButtonImages.GetButtonContent(new Uri(System.IO.Path.Combine(directoryName, "Images\\Add.png")));
            mButtonDeleteEmployee.Content = ButtonImages.GetButtonContent(new Uri(System.IO.Path.Combine(directoryName, "Images\\Remove.png")));
            mButtonEditEmployee.Content = ButtonImages.GetButtonContent(new Uri(System.IO.Path.Combine(directoryName, "Images\\Edit.png")));
            mRefreshButton.Content = ButtonImages.GetButtonContent(new Uri(System.IO.Path.Combine(directoryName, "Images\\RefreshButton.png")));
            mCreateXMLEmployees.Content = ButtonImages.GetButtonContent(new Uri(System.IO.Path.Combine(directoryName, "Images\\Xml.png")));
        }

        private void mEmployeeView_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
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
