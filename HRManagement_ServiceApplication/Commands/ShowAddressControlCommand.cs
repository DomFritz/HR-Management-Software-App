using HRManagement_ServiceApplication.WcfEmployeeDatabaseService;
using System;
using System.Windows.Input;

namespace HRManagement_ServiceApplication
{
    internal interface IShowAddressContent
    {
        void StartAddAddressControl(DialogMode mode = DialogMode.Create, Address a = null);
    }

    /// <summary>
    /// The command to show the "Create Address"-Window in Create-Mode.
    /// </summary>
    internal class ShowAddressControlCommand : ICommand
    {
        private IShowAddressContent mModel;

        public ShowAddressControlCommand(IShowAddressContent viewModel)
        {
            mModel = viewModel;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            mModel.StartAddAddressControl();
        }

        public void FireCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
            {
                CanExecuteChanged(this, EventArgs.Empty);
            }
        }
    }
}
