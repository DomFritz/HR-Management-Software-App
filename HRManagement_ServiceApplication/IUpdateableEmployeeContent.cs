using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRManagement_ServiceApplication
{
    /// <summary>
    /// The interface to update the list view in the main window for the employees.
    /// </summary>
    public interface IUpdateableEmployeeContent
    {
        void UpdateEmployeeList();
    }
}
