﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.1022
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace JIT.CPOS.BS.Web.UnitServiceSoap {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="UnitServiceSoap.UnitServiceSoap")]
    public interface UnitServiceSoap {
        
        // CODEGEN: 命名空间 http://tempuri.org/ 的元素名称 HelloWorldResult 以后生成的消息协定未标记为 nillable
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/HelloWorld", ReplyAction="*")]
        JIT.CPOS.BS.Web.UnitServiceSoap.HelloWorldResponse HelloWorld(JIT.CPOS.BS.Web.UnitServiceSoap.HelloWorldRequest request);
        
        // CODEGEN: 命名空间 http://tempuri.org/ 的元素名称 cityId 以后生成的消息协定未标记为 nillable
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/GetUnitIdList", ReplyAction="*")]
        JIT.CPOS.BS.Web.UnitServiceSoap.GetUnitIdListResponse GetUnitIdList(JIT.CPOS.BS.Web.UnitServiceSoap.GetUnitIdListRequest request);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class HelloWorldRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="HelloWorld", Namespace="http://tempuri.org/", Order=0)]
        public JIT.CPOS.BS.Web.UnitServiceSoap.HelloWorldRequestBody Body;
        
        public HelloWorldRequest() {
        }
        
        public HelloWorldRequest(JIT.CPOS.BS.Web.UnitServiceSoap.HelloWorldRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute()]
    public partial class HelloWorldRequestBody {
        
        public HelloWorldRequestBody() {
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class HelloWorldResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="HelloWorldResponse", Namespace="http://tempuri.org/", Order=0)]
        public JIT.CPOS.BS.Web.UnitServiceSoap.HelloWorldResponseBody Body;
        
        public HelloWorldResponse() {
        }
        
        public HelloWorldResponse(JIT.CPOS.BS.Web.UnitServiceSoap.HelloWorldResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class HelloWorldResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string HelloWorldResult;
        
        public HelloWorldResponseBody() {
        }
        
        public HelloWorldResponseBody(string HelloWorldResult) {
            this.HelloWorldResult = HelloWorldResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class GetUnitIdListRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="GetUnitIdList", Namespace="http://tempuri.org/", Order=0)]
        public JIT.CPOS.BS.Web.UnitServiceSoap.GetUnitIdListRequestBody Body;
        
        public GetUnitIdListRequest() {
        }
        
        public GetUnitIdListRequest(JIT.CPOS.BS.Web.UnitServiceSoap.GetUnitIdListRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class GetUnitIdListRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string cityId;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string timestamp;
        
        public GetUnitIdListRequestBody() {
        }
        
        public GetUnitIdListRequestBody(string cityId, string timestamp) {
            this.cityId = cityId;
            this.timestamp = timestamp;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class GetUnitIdListResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="GetUnitIdListResponse", Namespace="http://tempuri.org/", Order=0)]
        public JIT.CPOS.BS.Web.UnitServiceSoap.GetUnitIdListResponseBody Body;
        
        public GetUnitIdListResponse() {
        }
        
        public GetUnitIdListResponse(JIT.CPOS.BS.Web.UnitServiceSoap.GetUnitIdListResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class GetUnitIdListResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string GetUnitIdListResult;
        
        public GetUnitIdListResponseBody() {
        }
        
        public GetUnitIdListResponseBody(string GetUnitIdListResult) {
            this.GetUnitIdListResult = GetUnitIdListResult;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface UnitServiceSoapChannel : JIT.CPOS.BS.Web.UnitServiceSoap.UnitServiceSoap, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class UnitServiceSoapClient : System.ServiceModel.ClientBase<JIT.CPOS.BS.Web.UnitServiceSoap.UnitServiceSoap>, JIT.CPOS.BS.Web.UnitServiceSoap.UnitServiceSoap {
        
        public UnitServiceSoapClient() {
        }
        
        public UnitServiceSoapClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public UnitServiceSoapClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public UnitServiceSoapClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public UnitServiceSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        JIT.CPOS.BS.Web.UnitServiceSoap.HelloWorldResponse JIT.CPOS.BS.Web.UnitServiceSoap.UnitServiceSoap.HelloWorld(JIT.CPOS.BS.Web.UnitServiceSoap.HelloWorldRequest request) {
            return base.Channel.HelloWorld(request);
        }
        
        public string HelloWorld() {
            JIT.CPOS.BS.Web.UnitServiceSoap.HelloWorldRequest inValue = new JIT.CPOS.BS.Web.UnitServiceSoap.HelloWorldRequest();
            inValue.Body = new JIT.CPOS.BS.Web.UnitServiceSoap.HelloWorldRequestBody();
            JIT.CPOS.BS.Web.UnitServiceSoap.HelloWorldResponse retVal = ((JIT.CPOS.BS.Web.UnitServiceSoap.UnitServiceSoap)(this)).HelloWorld(inValue);
            return retVal.Body.HelloWorldResult;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        JIT.CPOS.BS.Web.UnitServiceSoap.GetUnitIdListResponse JIT.CPOS.BS.Web.UnitServiceSoap.UnitServiceSoap.GetUnitIdList(JIT.CPOS.BS.Web.UnitServiceSoap.GetUnitIdListRequest request) {
            return base.Channel.GetUnitIdList(request);
        }
        
        public string GetUnitIdList(string cityId, string timestamp) {
            JIT.CPOS.BS.Web.UnitServiceSoap.GetUnitIdListRequest inValue = new JIT.CPOS.BS.Web.UnitServiceSoap.GetUnitIdListRequest();
            inValue.Body = new JIT.CPOS.BS.Web.UnitServiceSoap.GetUnitIdListRequestBody();
            inValue.Body.cityId = cityId;
            inValue.Body.timestamp = timestamp;
            JIT.CPOS.BS.Web.UnitServiceSoap.GetUnitIdListResponse retVal = ((JIT.CPOS.BS.Web.UnitServiceSoap.UnitServiceSoap)(this)).GetUnitIdList(inValue);
            return retVal.Body.GetUnitIdListResult;
        }
    }
}
