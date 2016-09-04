﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace HRManagement_ServiceApplication_HRItems
{
    [DataContract]
    // The DataContract Object Classes with the DataMembers for the Employees.
    public class Employee
    {
        [DataMember]
        public Guid Id { get; set; }

        [DataMember]
        public string FirstName { get; set; }

        [DataMember]
        public string LastName { get; set; }

        [DataMember]
        public int Age { get; set; }

        [DataMember]
        public List<Address> Addresses { get; set; }
    }
}