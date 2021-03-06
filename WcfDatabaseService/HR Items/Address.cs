﻿using System;
using System.Runtime.Serialization;

namespace HRManagement_ServiceApplication_HRItems
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

        [DataMember]
        public Guid EmployeeId { get; set; }
    }
}