﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.18408
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace JIT.CPOS.Web.SAIFEmailValid {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://SAIF.ESB.ServiceProvider/", ConfigurationName="SAIFEmailValid.UserServiceSoap")]
    public interface UserServiceSoap {
        
        // CODEGEN: 命名空间 http://SAIF.ESB.ServiceProvider/ 的元素名称 s 以后生成的消息协定未标记为 nillable
        [System.ServiceModel.OperationContractAttribute(Action="http://SAIF.ESB.ServiceProvider/hello", ReplyAction="*")]
        JIT.CPOS.Web.SAIFEmailValid.helloResponse hello(JIT.CPOS.Web.SAIFEmailValid.helloRequest request);
        
        // CODEGEN: 命名空间 http://SAIF.ESB.ServiceProvider/ 的元素名称 username 以后生成的消息协定未标记为 nillable
        [System.ServiceModel.OperationContractAttribute(Action="http://SAIF.ESB.ServiceProvider/ValidUser", ReplyAction="*")]
        JIT.CPOS.Web.SAIFEmailValid.ValidUserResponse ValidUser(JIT.CPOS.Web.SAIFEmailValid.ValidUserRequest request);
        
        // CODEGEN: 命名空间 http://SAIF.ESB.ServiceProvider/ 的元素名称 requestXml 以后生成的消息协定未标记为 nillable
        [System.ServiceModel.OperationContractAttribute(Action="http://SAIF.ESB.ServiceProvider/CheckUsers", ReplyAction="*")]
        JIT.CPOS.Web.SAIFEmailValid.CheckUsersResponse CheckUsers(JIT.CPOS.Web.SAIFEmailValid.CheckUsersRequest request);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class helloRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="hello", Namespace="http://SAIF.ESB.ServiceProvider/", Order=0)]
        public JIT.CPOS.Web.SAIFEmailValid.helloRequestBody Body;
        
        public helloRequest() {
        }
        
        public helloRequest(JIT.CPOS.Web.SAIFEmailValid.helloRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://SAIF.ESB.ServiceProvider/")]
    public partial class helloRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string s;
        
        public helloRequestBody() {
        }
        
        public helloRequestBody(string s) {
            this.s = s;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class helloResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="helloResponse", Namespace="http://SAIF.ESB.ServiceProvider/", Order=0)]
        public JIT.CPOS.Web.SAIFEmailValid.helloResponseBody Body;
        
        public helloResponse() {
        }
        
        public helloResponse(JIT.CPOS.Web.SAIFEmailValid.helloResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://SAIF.ESB.ServiceProvider/")]
    public partial class helloResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string helloResult;
        
        public helloResponseBody() {
        }
        
        public helloResponseBody(string helloResult) {
            this.helloResult = helloResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class ValidUserRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="ValidUser", Namespace="http://SAIF.ESB.ServiceProvider/", Order=0)]
        public JIT.CPOS.Web.SAIFEmailValid.ValidUserRequestBody Body;
        
        public ValidUserRequest() {
        }
        
        public ValidUserRequest(JIT.CPOS.Web.SAIFEmailValid.ValidUserRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://SAIF.ESB.ServiceProvider/")]
    public partial class ValidUserRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string username;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string password;
        
        public ValidUserRequestBody() {
        }
        
        public ValidUserRequestBody(string username, string password) {
            this.username = username;
            this.password = password;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class ValidUserResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="ValidUserResponse", Namespace="http://SAIF.ESB.ServiceProvider/", Order=0)]
        public JIT.CPOS.Web.SAIFEmailValid.ValidUserResponseBody Body;
        
        public ValidUserResponse() {
        }
        
        public ValidUserResponse(JIT.CPOS.Web.SAIFEmailValid.ValidUserResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://SAIF.ESB.ServiceProvider/")]
    public partial class ValidUserResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string ValidUserResult;
        
        public ValidUserResponseBody() {
        }
        
        public ValidUserResponseBody(string ValidUserResult) {
            this.ValidUserResult = ValidUserResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class CheckUsersRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="CheckUsers", Namespace="http://SAIF.ESB.ServiceProvider/", Order=0)]
        public JIT.CPOS.Web.SAIFEmailValid.CheckUsersRequestBody Body;
        
        public CheckUsersRequest() {
        }
        
        public CheckUsersRequest(JIT.CPOS.Web.SAIFEmailValid.CheckUsersRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://SAIF.ESB.ServiceProvider/")]
    public partial class CheckUsersRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string requestXml;
        
        public CheckUsersRequestBody() {
        }
        
        public CheckUsersRequestBody(string requestXml) {
            this.requestXml = requestXml;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class CheckUsersResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="CheckUsersResponse", Namespace="http://SAIF.ESB.ServiceProvider/", Order=0)]
        public JIT.CPOS.Web.SAIFEmailValid.CheckUsersResponseBody Body;
        
        public CheckUsersResponse() {
        }
        
        public CheckUsersResponse(JIT.CPOS.Web.SAIFEmailValid.CheckUsersResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://SAIF.ESB.ServiceProvider/")]
    public partial class CheckUsersResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string CheckUsersResult;
        
        public CheckUsersResponseBody() {
        }
        
        public CheckUsersResponseBody(string CheckUsersResult) {
            this.CheckUsersResult = CheckUsersResult;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface UserServiceSoapChannel : JIT.CPOS.Web.SAIFEmailValid.UserServiceSoap, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class UserServiceSoapClient : System.ServiceModel.ClientBase<JIT.CPOS.Web.SAIFEmailValid.UserServiceSoap>, JIT.CPOS.Web.SAIFEmailValid.UserServiceSoap {
        
        public UserServiceSoapClient() {
        }
        
        public UserServiceSoapClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public UserServiceSoapClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public UserServiceSoapClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public UserServiceSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        JIT.CPOS.Web.SAIFEmailValid.helloResponse JIT.CPOS.Web.SAIFEmailValid.UserServiceSoap.hello(JIT.CPOS.Web.SAIFEmailValid.helloRequest request) {
            return base.Channel.hello(request);
        }
        
        public string hello(string s) {
            JIT.CPOS.Web.SAIFEmailValid.helloRequest inValue = new JIT.CPOS.Web.SAIFEmailValid.helloRequest();
            inValue.Body = new JIT.CPOS.Web.SAIFEmailValid.helloRequestBody();
            inValue.Body.s = s;
            JIT.CPOS.Web.SAIFEmailValid.helloResponse retVal = ((JIT.CPOS.Web.SAIFEmailValid.UserServiceSoap)(this)).hello(inValue);
            return retVal.Body.helloResult;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        JIT.CPOS.Web.SAIFEmailValid.ValidUserResponse JIT.CPOS.Web.SAIFEmailValid.UserServiceSoap.ValidUser(JIT.CPOS.Web.SAIFEmailValid.ValidUserRequest request) {
            return base.Channel.ValidUser(request);
        }
        
        public string ValidUser(string username, string password) {
            JIT.CPOS.Web.SAIFEmailValid.ValidUserRequest inValue = new JIT.CPOS.Web.SAIFEmailValid.ValidUserRequest();
            inValue.Body = new JIT.CPOS.Web.SAIFEmailValid.ValidUserRequestBody();
            inValue.Body.username = username;
            inValue.Body.password = password;
            JIT.CPOS.Web.SAIFEmailValid.ValidUserResponse retVal = ((JIT.CPOS.Web.SAIFEmailValid.UserServiceSoap)(this)).ValidUser(inValue);
            return retVal.Body.ValidUserResult;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        JIT.CPOS.Web.SAIFEmailValid.CheckUsersResponse JIT.CPOS.Web.SAIFEmailValid.UserServiceSoap.CheckUsers(JIT.CPOS.Web.SAIFEmailValid.CheckUsersRequest request) {
            return base.Channel.CheckUsers(request);
        }
        
        public string CheckUsers(string requestXml) {
            JIT.CPOS.Web.SAIFEmailValid.CheckUsersRequest inValue = new JIT.CPOS.Web.SAIFEmailValid.CheckUsersRequest();
            inValue.Body = new JIT.CPOS.Web.SAIFEmailValid.CheckUsersRequestBody();
            inValue.Body.requestXml = requestXml;
            JIT.CPOS.Web.SAIFEmailValid.CheckUsersResponse retVal = ((JIT.CPOS.Web.SAIFEmailValid.UserServiceSoap)(this)).CheckUsers(inValue);
            return retVal.Body.CheckUsersResult;
        }
    }
}
