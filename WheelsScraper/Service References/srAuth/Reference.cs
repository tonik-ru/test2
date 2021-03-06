﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18408
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WheelsScraper.srAuth {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="AuthInfo", Namespace="http://schemas.datacontract.org/2004/07/")]
    [System.SerializableAttribute()]
    public partial class AuthInfo : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private WheelsScraper.srAuth.AuthInfoResult AuthResultField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int DelayField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.TimeSpan EndTimeField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int MaxSecondsField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int MaxThreadField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int RandomField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.TimeSpan StartInField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.TimeSpan StartTimeField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.TimeSpan TimeLeftField;
        
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
        public WheelsScraper.srAuth.AuthInfoResult AuthResult {
            get {
                return this.AuthResultField;
            }
            set {
                if ((this.AuthResultField.Equals(value) != true)) {
                    this.AuthResultField = value;
                    this.RaisePropertyChanged("AuthResult");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int Delay {
            get {
                return this.DelayField;
            }
            set {
                if ((this.DelayField.Equals(value) != true)) {
                    this.DelayField = value;
                    this.RaisePropertyChanged("Delay");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.TimeSpan EndTime {
            get {
                return this.EndTimeField;
            }
            set {
                if ((this.EndTimeField.Equals(value) != true)) {
                    this.EndTimeField = value;
                    this.RaisePropertyChanged("EndTime");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int MaxSeconds {
            get {
                return this.MaxSecondsField;
            }
            set {
                if ((this.MaxSecondsField.Equals(value) != true)) {
                    this.MaxSecondsField = value;
                    this.RaisePropertyChanged("MaxSeconds");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int MaxThread {
            get {
                return this.MaxThreadField;
            }
            set {
                if ((this.MaxThreadField.Equals(value) != true)) {
                    this.MaxThreadField = value;
                    this.RaisePropertyChanged("MaxThread");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int Random {
            get {
                return this.RandomField;
            }
            set {
                if ((this.RandomField.Equals(value) != true)) {
                    this.RandomField = value;
                    this.RaisePropertyChanged("Random");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.TimeSpan StartIn {
            get {
                return this.StartInField;
            }
            set {
                if ((this.StartInField.Equals(value) != true)) {
                    this.StartInField = value;
                    this.RaisePropertyChanged("StartIn");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.TimeSpan StartTime {
            get {
                return this.StartTimeField;
            }
            set {
                if ((this.StartTimeField.Equals(value) != true)) {
                    this.StartTimeField = value;
                    this.RaisePropertyChanged("StartTime");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.TimeSpan TimeLeft {
            get {
                return this.TimeLeftField;
            }
            set {
                if ((this.TimeLeftField.Equals(value) != true)) {
                    this.TimeLeftField = value;
                    this.RaisePropertyChanged("TimeLeft");
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
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="AuthInfoResult", Namespace="http://schemas.datacontract.org/2004/07/")]
    public enum AuthInfoResult : int {
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Success = 0,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        APIKeyNotFound = 1,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        UserIsBlocked = 2,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        NotWorkingTime = 3,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        NotAllowedModule = 4,
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="srAuth.IAuthService")]
    public interface IAuthService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IAuthService/Login", ReplyAction="http://tempuri.org/IAuthService/LoginResponse")]
        WheelsScraper.srAuth.AuthInfo Login(string sceAPIKey, string moduleName);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IAuthService/ReportWorkTime", ReplyAction="http://tempuri.org/IAuthService/ReportWorkTimeResponse")]
        void ReportWorkTime(string sceAPIKey, string moduleName, System.DateTime startDate, System.TimeSpan workTime);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IAuthServiceChannel : WheelsScraper.srAuth.IAuthService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class AuthServiceClient : System.ServiceModel.ClientBase<WheelsScraper.srAuth.IAuthService>, WheelsScraper.srAuth.IAuthService {
        
        public AuthServiceClient() {
        }
        
        public AuthServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public AuthServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public AuthServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public AuthServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public WheelsScraper.srAuth.AuthInfo Login(string sceAPIKey, string moduleName) {
            return base.Channel.Login(sceAPIKey, moduleName);
        }
        
        public void ReportWorkTime(string sceAPIKey, string moduleName, System.DateTime startDate, System.TimeSpan workTime) {
            base.Channel.ReportWorkTime(sceAPIKey, moduleName, startDate, workTime);
        }
    }
}
