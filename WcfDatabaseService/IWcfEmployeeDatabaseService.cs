using HRManagement_ServiceApplication_HRItems;
using System;
using System.Collections.ObjectModel;
using System.ServiceModel;

namespace WcfDatabaseService
{
    [ServiceContract]
    // This interface is necessary to communicate with the GUI Application which has the service reference. All necessary methods which are outside of this service needed, are here.
    public interface IWcfEmployeeDatabaseService
    {
        [OperationContract]
        int InsertEmployeeAndAddresses(Employee e);

        [OperationContract]
        int InsertAddress(Address a, Guid employeeId);

        [OperationContract]
        ObservableCollection<Employee> ReadAllEmployees();

        [OperationContract]
        ObservableCollection<Address> ReadAllAddresses(Employee e = null);

        [OperationContract]
        int UpdateEmployeeAndAddresses(Employee e);

        [OperationContract]
        int UpdateAddress(Address a, Guid employeeId);

        [OperationContract]
        int DeleteEmployee(Employee e);

        [OperationContract]
        int DeleteAddress(Address a);

        [OperationContract]
        int DeleteAddressesOfEmployee(Guid employeeId);

        [OperationContract]
        string GetApplicationName();

        [OperationContract]
        bool CheckDatabaseAvailability();
    }
}
