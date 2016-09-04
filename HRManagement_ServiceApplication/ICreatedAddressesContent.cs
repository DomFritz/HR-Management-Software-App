using HRManagement_ServiceApplication_HRItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
