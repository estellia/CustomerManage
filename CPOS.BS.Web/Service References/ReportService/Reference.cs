﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.1022
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace JIT.CPOS.BS.Web.ReportService {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="MstrPromptAnswerItem", Namespace="http://schemas.datacontract.org/2004/07/Utility.MSTRIntegration.WcfService")]
    [System.SerializableAttribute()]
    public partial class MstrPromptAnswerItem : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string PromptCodeField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string[] QueryConditionField;
        
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
        public string PromptCode {
            get {
                return this.PromptCodeField;
            }
            set {
                if ((object.ReferenceEquals(this.PromptCodeField, value) != true)) {
                    this.PromptCodeField = value;
                    this.RaisePropertyChanged("PromptCode");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string[] QueryCondition {
            get {
                return this.QueryConditionField;
            }
            set {
                if ((object.ReferenceEquals(this.QueryConditionField, value) != true)) {
                    this.QueryConditionField = value;
                    this.RaisePropertyChanged("QueryCondition");
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
    [System.Runtime.Serialization.DataContractAttribute(Name="MstrDataRigthPromptAnswerItem", Namespace="http://schemas.datacontract.org/2004/07/Utility.MSTRIntegration.WcfService")]
    [System.SerializableAttribute()]
    public partial class MstrDataRigthPromptAnswerItem : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int LevelField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string[] ValuesField;
        
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
        public int Level {
            get {
                return this.LevelField;
            }
            set {
                if ((this.LevelField.Equals(value) != true)) {
                    this.LevelField = value;
                    this.RaisePropertyChanged("Level");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string[] Values {
            get {
                return this.ValuesField;
            }
            set {
                if ((object.ReferenceEquals(this.ValuesField, value) != true)) {
                    this.ValuesField = value;
                    this.RaisePropertyChanged("Values");
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
    [System.Runtime.Serialization.DataContractAttribute(Name="CompositeType", Namespace="http://schemas.datacontract.org/2004/07/Utility.MSTRIntegration.WcfService")]
    [System.SerializableAttribute()]
    public partial class CompositeType : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private bool BoolValueField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string StringValueField;
        
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
        public bool BoolValue {
            get {
                return this.BoolValueField;
            }
            set {
                if ((this.BoolValueField.Equals(value) != true)) {
                    this.BoolValueField = value;
                    this.RaisePropertyChanged("BoolValue");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string StringValue {
            get {
                return this.StringValueField;
            }
            set {
                if ((object.ReferenceEquals(this.StringValueField, value) != true)) {
                    this.StringValueField = value;
                    this.RaisePropertyChanged("StringValue");
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
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ReportService.IMSTRIntegrationService")]
    public interface IMSTRIntegrationService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMSTRIntegrationService/GetData", ReplyAction="http://tempuri.org/IMSTRIntegrationService/GetDataResponse")]
        string GetData(int value);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMSTRIntegrationService/GetMstrReportUrl", ReplyAction="http://tempuri.org/IMSTRIntegrationService/GetMstrReportUrlResponse")]
        string GetMstrReportUrl(int pMstrIntegrationSessionID, string pReportGuid, string pClientId, string pUserId, JIT.CPOS.BS.Web.ReportService.MstrPromptAnswerItem[] pPromptAnswers, JIT.CPOS.BS.Web.ReportService.MstrDataRigthPromptAnswerItem[] pDataRigthPromptAnswers);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMSTRIntegrationService/Login", ReplyAction="http://tempuri.org/IMSTRIntegrationService/LoginResponse")]
        int Login(int pLanguageLCID, string pClientIP, string pClientID, string pUserID, string pWebSiteSessionId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMSTRIntegrationService/Logout", ReplyAction="http://tempuri.org/IMSTRIntegrationService/LogoutResponse")]
        void Logout(string pClientId, string pUserId, int pSessionID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMSTRIntegrationService/SwitchLanguage", ReplyAction="http://tempuri.org/IMSTRIntegrationService/SwitchLanguageResponse")]
        void SwitchLanguage(string pClientId, string pUserId, int pMstrIntegrationSessionID, int pNewLanguageLCID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMSTRIntegrationService/GetDataUsingDataContract", ReplyAction="http://tempuri.org/IMSTRIntegrationService/GetDataUsingDataContractResponse")]
        JIT.CPOS.BS.Web.ReportService.CompositeType GetDataUsingDataContract(JIT.CPOS.BS.Web.ReportService.CompositeType composite);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IMSTRIntegrationServiceChannel : JIT.CPOS.BS.Web.ReportService.IMSTRIntegrationService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class MSTRIntegrationServiceClient : System.ServiceModel.ClientBase<JIT.CPOS.BS.Web.ReportService.IMSTRIntegrationService>, JIT.CPOS.BS.Web.ReportService.IMSTRIntegrationService {
        
        public MSTRIntegrationServiceClient() {
        }
        
        public MSTRIntegrationServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public MSTRIntegrationServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public MSTRIntegrationServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public MSTRIntegrationServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public string GetData(int value) {
            return base.Channel.GetData(value);
        }
        
        public string GetMstrReportUrl(int pMstrIntegrationSessionID, string pReportGuid, string pClientId, string pUserId, JIT.CPOS.BS.Web.ReportService.MstrPromptAnswerItem[] pPromptAnswers, JIT.CPOS.BS.Web.ReportService.MstrDataRigthPromptAnswerItem[] pDataRigthPromptAnswers) {
            return base.Channel.GetMstrReportUrl(pMstrIntegrationSessionID, pReportGuid, pClientId, pUserId, pPromptAnswers, pDataRigthPromptAnswers);
        }
        
        public int Login(int pLanguageLCID, string pClientIP, string pClientID, string pUserID, string pWebSiteSessionId) {
            return base.Channel.Login(pLanguageLCID, pClientIP, pClientID, pUserID, pWebSiteSessionId);
        }
        
        public void Logout(string pClientId, string pUserId, int pSessionID) {
            base.Channel.Logout(pClientId, pUserId, pSessionID);
        }
        
        public void SwitchLanguage(string pClientId, string pUserId, int pMstrIntegrationSessionID, int pNewLanguageLCID) {
            base.Channel.SwitchLanguage(pClientId, pUserId, pMstrIntegrationSessionID, pNewLanguageLCID);
        }
        
        public JIT.CPOS.BS.Web.ReportService.CompositeType GetDataUsingDataContract(JIT.CPOS.BS.Web.ReportService.CompositeType composite) {
            return base.Channel.GetDataUsingDataContract(composite);
        }
    }
}