using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace WcfDatabaseService
{
    [DataContract]
    // The DataContract Object Classes with the DataMembers for the Addresses.
    public class Address
    {
        [DataMember]
        public Guid Id { get; set; }

        [DataMember]
        public string Street { get; set; }

        [DataMember]
        public int Zip { get; set; }

        [DataMember]
        public string City { get; set; }

        [DataMember]
        public string State { get; set; }
    }
}