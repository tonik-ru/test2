﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18408
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WheelsScraper.SRUpdater {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="UpdateFileInfo", Namespace="http://schemas.datacontract.org/2004/07/")]
    [System.SerializableAttribute()]
    public partial class UpdateFileInfo : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string HashField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int LengthField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string NameField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string VersionField;
        
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
        public string Hash {
            get {
                return this.HashField;
            }
            set {
                if ((object.ReferenceEquals(this.HashField, value) != true)) {
                    this.HashField = value;
                    this.RaisePropertyChanged("Hash");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int Length {
            get {
                return this.LengthField;
            }
            set {
                if ((this.LengthField.Equals(value) != true)) {
                    this.LengthField = value;
                    this.RaisePropertyChanged("Length");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Name {
            get {
                return this.NameField;
            }
            set {
                if ((object.ReferenceEquals(this.NameField, value) != true)) {
                    this.NameField = value;
                    this.RaisePropertyChanged("Name");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Version {
            get {
                return this.VersionField;
            }
            set {
                if ((object.ReferenceEquals(this.VersionField, value) != true)) {
                    this.VersionField = value;
                    this.RaisePropertyChanged("Version");
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
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="SRUpdater.IUpdaterService")]
    public interface IUpdaterService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IUpdaterService/GetAppFiles", ReplyAction="http://tempuri.org/IUpdaterService/GetAppFilesResponse")]
        WheelsScraper.SRUpdater.UpdateFileInfo[] GetAppFiles(string applicationName, int clientID, ref int release);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IUpdaterService/GetFile", ReplyAction="http://tempuri.org/IUpdaterService/GetFileResponse")]
        System.IO.Stream GetFile(string applicationName, string fileName, int clientID);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IUpdaterServiceChannel : WheelsScraper.SRUpdater.IUpdaterService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class UpdaterServiceClient : System.ServiceModel.ClientBase<WheelsScraper.SRUpdater.IUpdaterService>, WheelsScraper.SRUpdater.IUpdaterService {
        
        public UpdaterServiceClient() {
        }
        
        public UpdaterServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public UpdaterServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public UpdaterServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public UpdaterServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public WheelsScraper.SRUpdater.UpdateFileInfo[] GetAppFiles(string applicationName, int clientID, ref int release) {
            return base.Channel.GetAppFiles(applicationName, clientID, ref release);
        }
        
        public System.IO.Stream GetFile(string applicationName, string fileName, int clientID) {
            return base.Channel.GetFile(applicationName, fileName, clientID);
        }
    }
}
