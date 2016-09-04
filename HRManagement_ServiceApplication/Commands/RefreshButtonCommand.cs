﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HRManagement_ServiceApplication.Commands
{
    /// <summary>
    /// The command for the Refresh Button to update, if there were manually added employees into the database.
    /// </summary>
    internal class RefreshButtonCommand : ICommand
    {
        private IUpdateableEmployeeContent mEmployeeOverviewViewModel;

        public RefreshButtonCommand(IUpdateableEmployeeContent employeeOverviewViewModel)
        {
            this.mEmployeeOverviewViewModel = employeeOverviewViewModel;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            mEmployeeOverviewViewModel.UpdateEmployeeList();
        }
    }
}
