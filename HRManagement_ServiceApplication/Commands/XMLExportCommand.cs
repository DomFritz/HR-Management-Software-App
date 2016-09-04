using HRManagement_ServiceApplication_HRItems;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HRManagement_ServiceApplication.Commands
{
    public interface IXMLExportContent
    {
        void DoXMLExport();

        ObservableCollection<Employee> AllEmployees { get; }
    }

    /// <summary>
    /// The command to export all employees into a XML-File.
    /// </summary>
    public class XMLExportCommand : ICommand
    {
        private IXMLExportContent mEmployeeOverviewViewModel;

        public XMLExportCommand(IXMLExportContent model)
        {
            mEmployeeOverviewViewModel = model;
        }

        public void FireCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
            {
                CanExecuteChanged(this, EventArgs.Empty);
            }
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            // if no employees, nothing to export, so the button should be disabled
            if (mEmployeeOverviewViewModel.AllEmployees.Any())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Execute(object parameter)
        {
            // do the export
            mEmployeeOverviewViewModel.DoXMLExport();
        }
    }
}
