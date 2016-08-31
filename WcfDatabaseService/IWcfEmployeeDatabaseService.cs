using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WcfDatabaseService
{
    [ServiceContract]
    // This interface is necessary to communicate with the GUI Application which has the service reference. All necessary methods which are outside of this service needed, are here.
    public interface IWcfEmployeeDatabaseService
    {
        [OperationContract]
        int InsertEmployeeAndAddressIfNotAvailable(Employee e);

        [OperationContract]
        int InsertAddress(Address a);

        [OperationContract]
        ObservableCollection<Employee> ReadAllEmployees();

        [OperationContract]
        ObservableCollection<Address> ReadAllAddresses();

        [OperationContract]
        bool UpdateEmployeeAndAddress(Employee e);

        [OperationContract]
        int DeleteEmployee(Employee e);

        [OperationContract]
        void DeleteAddressWhenNotUsedByOtherEmployee(Address a);
    }
}
