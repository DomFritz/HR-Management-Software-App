using HRManagement_ServiceApplication.WcfEmployeeDatabaseService;
using System;

namespace HRManagement_ServiceApplication
{
    /// <summary>
    /// the interface to update the addresses list view in the "Mitarbeiter erstellen"-Dialog after creating, adding or editing an address
    /// </summary>
    public interface ICreatedAddressesContent
    {
        void UpdateAddressesListView(Address address);
    }
}
