﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HRManagement_ServiceApplication.WcfEmployeeDatabaseService {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Employee", Namespace="http://schemas.datacontract.org/2004/07/WcfDatabaseService")]
    [System.SerializableAttribute()]
    public partial class Employee : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private HRManagement_ServiceApplication.WcfEmployeeDatabaseService.Address AddressesField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int AgeField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string FirstNameField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Guid IdField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string LastNameField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public HRManagement_ServiceApplication.WcfEmployeeDatabaseService.Address Addresses {
            get {
                return this.AddressesField;
            }
            set {
                if ((object.ReferenceEquals(this.AddressesField, value) != true)) {
                    this.AddressesField = value;
                    this.RaisePropertyChanged("Addresses");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int Age {
            get {
                return this.AgeField;
            }
            set {
                if ((this.AgeField.Equals(value) != true)) {
                    this.AgeField = value;
                    this.RaisePropertyChanged("Age");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string FirstName {
            get {
                return this.FirstNameField;
            }
            set {
                if ((object.ReferenceEquals(this.FirstNameField, value) != true)) {
                    this.FirstNameField = value;
                    this.RaisePropertyChanged("FirstName");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Guid Id {
            get {
                return this.IdField;
            }
            set {
                if ((this.IdField.Equals(value) != true)) {
                    this.IdField = value;
                    this.RaisePropertyChanged("Id");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string LastName {
            get {
                return this.LastNameField;
            }
            set {
                if ((object.ReferenceEquals(this.LastNameField, value) != true)) {
                    this.LastNameField = value;
                    this.RaisePropertyChanged("LastName");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Address", Namespace="http://schemas.datacontract.org/2004/07/WcfDatabaseService")]
    [System.SerializableAttribute()]
    public partial class Address : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string CityField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Guid IdField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string StateField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string StreetField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int ZipField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string City {
            get {
                return this.CityField;
            }
            set {
                if ((object.ReferenceEquals(this.CityField, value) != true)) {
                    this.CityField = value;
                    this.RaisePropertyChanged("City");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Guid Id {
            get {
                return this.IdField;
            }
            set {
                if ((this.IdField.Equals(value) != true)) {
                    this.IdField = value;
                    this.RaisePropertyChanged("Id");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string State {
            get {
                return this.StateField;
            }
            set {
                if ((object.ReferenceEquals(this.StateField, value) != true)) {
                    this.StateField = value;
                    this.RaisePropertyChanged("State");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Street {
            get {
                return this.StreetField;
            }
            set {
                if ((object.ReferenceEquals(this.StreetField, value) != true)) {
                    this.StreetField = value;
                    this.RaisePropertyChanged("Street");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int Zip {
            get {
                return this.ZipField;
            }
            set {
                if ((this.ZipField.Equals(value) != true)) {
                    this.ZipField = value;
                    this.RaisePropertyChanged("Zip");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="WcfEmployeeDatabaseService.IWcfEmployeeDatabaseService")]
    public interface IWcfEmployeeDatabaseService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWcfEmployeeDatabaseService/InsertEmployeeAndAddressIfNotAvail" +
            "able", ReplyAction="http://tempuri.org/IWcfEmployeeDatabaseService/InsertEmployeeAndAddressIfNotAvail" +
            "ableResponse")]
        int InsertEmployeeAndAddressIfNotAvailable(HRManagement_ServiceApplication.WcfEmployeeDatabaseService.Employee e);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWcfEmployeeDatabaseService/InsertEmployeeAndAddressIfNotAvail" +
            "able", ReplyAction="http://tempuri.org/IWcfEmployeeDatabaseService/InsertEmployeeAndAddressIfNotAvail" +
            "ableResponse")]
        System.Threading.Tasks.Task<int> InsertEmployeeAndAddressIfNotAvailableAsync(HRManagement_ServiceApplication.WcfEmployeeDatabaseService.Employee e);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWcfEmployeeDatabaseService/InsertAddress", ReplyAction="http://tempuri.org/IWcfEmployeeDatabaseService/InsertAddressResponse")]
        int InsertAddress(HRManagement_ServiceApplication.WcfEmployeeDatabaseService.Address a);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWcfEmployeeDatabaseService/InsertAddress", ReplyAction="http://tempuri.org/IWcfEmployeeDatabaseService/InsertAddressResponse")]
        System.Threading.Tasks.Task<int> InsertAddressAsync(HRManagement_ServiceApplication.WcfEmployeeDatabaseService.Address a);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWcfEmployeeDatabaseService/ReadAllEmployees", ReplyAction="http://tempuri.org/IWcfEmployeeDatabaseService/ReadAllEmployeesResponse")]
        HRManagement_ServiceApplication.WcfEmployeeDatabaseService.Employee[] ReadAllEmployees();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWcfEmployeeDatabaseService/ReadAllEmployees", ReplyAction="http://tempuri.org/IWcfEmployeeDatabaseService/ReadAllEmployeesResponse")]
        System.Threading.Tasks.Task<HRManagement_ServiceApplication.WcfEmployeeDatabaseService.Employee[]> ReadAllEmployeesAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWcfEmployeeDatabaseService/ReadAllAddresses", ReplyAction="http://tempuri.org/IWcfEmployeeDatabaseService/ReadAllAddressesResponse")]
        HRManagement_ServiceApplication.WcfEmployeeDatabaseService.Address[] ReadAllAddresses();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWcfEmployeeDatabaseService/ReadAllAddresses", ReplyAction="http://tempuri.org/IWcfEmployeeDatabaseService/ReadAllAddressesResponse")]
        System.Threading.Tasks.Task<HRManagement_ServiceApplication.WcfEmployeeDatabaseService.Address[]> ReadAllAddressesAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWcfEmployeeDatabaseService/UpdateEmployeeAndAddress", ReplyAction="http://tempuri.org/IWcfEmployeeDatabaseService/UpdateEmployeeAndAddressResponse")]
        bool UpdateEmployeeAndAddress(HRManagement_ServiceApplication.WcfEmployeeDatabaseService.Employee e);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWcfEmployeeDatabaseService/UpdateEmployeeAndAddress", ReplyAction="http://tempuri.org/IWcfEmployeeDatabaseService/UpdateEmployeeAndAddressResponse")]
        System.Threading.Tasks.Task<bool> UpdateEmployeeAndAddressAsync(HRManagement_ServiceApplication.WcfEmployeeDatabaseService.Employee e);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWcfEmployeeDatabaseService/DeleteEmployee", ReplyAction="http://tempuri.org/IWcfEmployeeDatabaseService/DeleteEmployeeResponse")]
        int DeleteEmployee(HRManagement_ServiceApplication.WcfEmployeeDatabaseService.Employee e);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWcfEmployeeDatabaseService/DeleteEmployee", ReplyAction="http://tempuri.org/IWcfEmployeeDatabaseService/DeleteEmployeeResponse")]
        System.Threading.Tasks.Task<int> DeleteEmployeeAsync(HRManagement_ServiceApplication.WcfEmployeeDatabaseService.Employee e);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWcfEmployeeDatabaseService/DeleteAddressWhenNotUsedByOtherEmp" +
            "loyee", ReplyAction="http://tempuri.org/IWcfEmployeeDatabaseService/DeleteAddressWhenNotUsedByOtherEmp" +
            "loyeeResponse")]
        void DeleteAddressWhenNotUsedByOtherEmployee(HRManagement_ServiceApplication.WcfEmployeeDatabaseService.Address a);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWcfEmployeeDatabaseService/DeleteAddressWhenNotUsedByOtherEmp" +
            "loyee", ReplyAction="http://tempuri.org/IWcfEmployeeDatabaseService/DeleteAddressWhenNotUsedByOtherEmp" +
            "loyeeResponse")]
        System.Threading.Tasks.Task DeleteAddressWhenNotUsedByOtherEmployeeAsync(HRManagement_ServiceApplication.WcfEmployeeDatabaseService.Address a);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IWcfEmployeeDatabaseServiceChannel : HRManagement_ServiceApplication.WcfEmployeeDatabaseService.IWcfEmployeeDatabaseService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class WcfEmployeeDatabaseServiceClient : System.ServiceModel.ClientBase<HRManagement_ServiceApplication.WcfEmployeeDatabaseService.IWcfEmployeeDatabaseService>, HRManagement_ServiceApplication.WcfEmployeeDatabaseService.IWcfEmployeeDatabaseService {
        
        public WcfEmployeeDatabaseServiceClient() {
        }
        
        public WcfEmployeeDatabaseServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public WcfEmployeeDatabaseServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public WcfEmployeeDatabaseServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public WcfEmployeeDatabaseServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public int InsertEmployeeAndAddressIfNotAvailable(HRManagement_ServiceApplication.WcfEmployeeDatabaseService.Employee e) {
            return base.Channel.InsertEmployeeAndAddressIfNotAvailable(e);
        }
        
        public System.Threading.Tasks.Task<int> InsertEmployeeAndAddressIfNotAvailableAsync(HRManagement_ServiceApplication.WcfEmployeeDatabaseService.Employee e) {
            return base.Channel.InsertEmployeeAndAddressIfNotAvailableAsync(e);
        }
        
        public int InsertAddress(HRManagement_ServiceApplication.WcfEmployeeDatabaseService.Address a) {
            return base.Channel.InsertAddress(a);
        }
        
        public System.Threading.Tasks.Task<int> InsertAddressAsync(HRManagement_ServiceApplication.WcfEmployeeDatabaseService.Address a) {
            return base.Channel.InsertAddressAsync(a);
        }
        
        public HRManagement_ServiceApplication.WcfEmployeeDatabaseService.Employee[] ReadAllEmployees() {
            return base.Channel.ReadAllEmployees();
        }
        
        public System.Threading.Tasks.Task<HRManagement_ServiceApplication.WcfEmployeeDatabaseService.Employee[]> ReadAllEmployeesAsync() {
            return base.Channel.ReadAllEmployeesAsync();
        }
        
        public HRManagement_ServiceApplication.WcfEmployeeDatabaseService.Address[] ReadAllAddresses() {
            return base.Channel.ReadAllAddresses();
        }
        
        public System.Threading.Tasks.Task<HRManagement_ServiceApplication.WcfEmployeeDatabaseService.Address[]> ReadAllAddressesAsync() {
            return base.Channel.ReadAllAddressesAsync();
        }
        
        public bool UpdateEmployeeAndAddress(HRManagement_ServiceApplication.WcfEmployeeDatabaseService.Employee e) {
            return base.Channel.UpdateEmployeeAndAddress(e);
        }
        
        public System.Threading.Tasks.Task<bool> UpdateEmployeeAndAddressAsync(HRManagement_ServiceApplication.WcfEmployeeDatabaseService.Employee e) {
            return base.Channel.UpdateEmployeeAndAddressAsync(e);
        }
        
        public int DeleteEmployee(HRManagement_ServiceApplication.WcfEmployeeDatabaseService.Employee e) {
            return base.Channel.DeleteEmployee(e);
        }
        
        public System.Threading.Tasks.Task<int> DeleteEmployeeAsync(HRManagement_ServiceApplication.WcfEmployeeDatabaseService.Employee e) {
            return base.Channel.DeleteEmployeeAsync(e);
        }
        
        public void DeleteAddressWhenNotUsedByOtherEmployee(HRManagement_ServiceApplication.WcfEmployeeDatabaseService.Address a) {
            base.Channel.DeleteAddressWhenNotUsedByOtherEmployee(a);
        }
        
        public System.Threading.Tasks.Task DeleteAddressWhenNotUsedByOtherEmployeeAsync(HRManagement_ServiceApplication.WcfEmployeeDatabaseService.Address a) {
            return base.Channel.DeleteAddressWhenNotUsedByOtherEmployeeAsync(a);
        }
    }
}