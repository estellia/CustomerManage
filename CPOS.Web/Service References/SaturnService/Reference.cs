﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.18444
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace JIT.CPOS.Web.SaturnService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="SaturnService.PlatformSystemSoap")]
    public interface PlatformSystemSoap {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/SaturnGetProductDetails", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        string SaturnGetProductDetails(string TraceCode, string ClientCode, string DeviceNum, string BillID, string SecretCode);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface PlatformSystemSoapChannel : JIT.CPOS.Web.SaturnService.PlatformSystemSoap, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class PlatformSystemSoapClient : System.ServiceModel.ClientBase<JIT.CPOS.Web.SaturnService.PlatformSystemSoap>, JIT.CPOS.Web.SaturnService.PlatformSystemSoap {
        
        public PlatformSystemSoapClient() {
        }
        
        public PlatformSystemSoapClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public PlatformSystemSoapClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public PlatformSystemSoapClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public PlatformSystemSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public string SaturnGetProductDetails(string TraceCode, string ClientCode, string DeviceNum, string BillID, string SecretCode) {
            return base.Channel.SaturnGetProductDetails(TraceCode, ClientCode, DeviceNum, BillID, SecretCode);
        }
    }
}
