﻿//------------------------------------------------------------------------------
// <auto-generated>
//     這段程式碼是由工具產生的。
//     執行階段版本:4.0.30319.42000
//
//     對這個檔案所做的變更可能會造成錯誤的行為，而且如果重新產生程式碼，
//     變更將會遺失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace AppWebAPI.SR {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://ws.flaginfo.com.cn", ConfigurationName="SR.SmsPortType")]
    public interface SmsPortType {
        
        // CODEGEN: 參數 'out' 需要無法以參數模式來擷取的其他架構資訊。特定屬性為 'System.Xml.Serialization.XmlElementAttribute'。
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        [return: System.ServiceModel.MessageParameterAttribute(Name="out")]
        AppWebAPI.SR.SmsResponse Sms(AppWebAPI.SR.SmsRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        System.Threading.Tasks.Task<AppWebAPI.SR.SmsResponse> SmsAsync(AppWebAPI.SR.SmsRequest request);
        
        // CODEGEN: 參數 'out' 需要無法以參數模式來擷取的其他架構資訊。特定屬性為 'System.Xml.Serialization.XmlElementAttribute'。
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        [return: System.ServiceModel.MessageParameterAttribute(Name="out")]
        AppWebAPI.SR.ReportResponse Report(AppWebAPI.SR.ReportRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        System.Threading.Tasks.Task<AppWebAPI.SR.ReportResponse> ReportAsync(AppWebAPI.SR.ReportRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        AppWebAPI.SR.ReplyResponse Reply(AppWebAPI.SR.ReplyRequest request);
        
        // CODEGEN: 正在產生訊息合約，因為此作業具有多個傳回值。
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        System.Threading.Tasks.Task<AppWebAPI.SR.ReplyResponse> ReplyAsync(AppWebAPI.SR.ReplyRequest request);
        
        // CODEGEN: 訊息 ReplyConfirmRequest 的包裝函式名稱 (ReplyConfirmRequest) 與預設值 (ReplyConfirm) 不符，正在產生訊息合約
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        AppWebAPI.SR.ReplyConfirmResponse ReplyConfirm(AppWebAPI.SR.ReplyConfirmRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        System.Threading.Tasks.Task<AppWebAPI.SR.ReplyConfirmResponse> ReplyConfirmAsync(AppWebAPI.SR.ReplyConfirmRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        AppWebAPI.SR.SearchSmsNumResponse SearchSmsNum(AppWebAPI.SR.SearchSmsNumRequest request);
        
        // CODEGEN: 正在產生訊息合約，因為此作業具有多個傳回值。
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        System.Threading.Tasks.Task<AppWebAPI.SR.SearchSmsNumResponse> SearchSmsNumAsync(AppWebAPI.SR.SearchSmsNumRequest request);
        
        // CODEGEN: 訊息 AuditingRequest 的包裝函式名稱 (AuditingRequest) 與預設值 (Auditing) 不符，正在產生訊息合約
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        AppWebAPI.SR.AuditingResponse Auditing(AppWebAPI.SR.AuditingRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        System.Threading.Tasks.Task<AppWebAPI.SR.AuditingResponse> AuditingAsync(AppWebAPI.SR.AuditingRequest request);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="Sms", WrapperNamespace="http://ws.flaginfo.com.cn", IsWrapped=true)]
    public partial class SmsRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws.flaginfo.com.cn", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string in0;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws.flaginfo.com.cn", Order=1)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string in1;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws.flaginfo.com.cn", Order=2)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string in2;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws.flaginfo.com.cn", Order=3)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string in3;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws.flaginfo.com.cn", Order=4)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string in4;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws.flaginfo.com.cn", Order=5)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string in5;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws.flaginfo.com.cn", Order=6)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string in6;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws.flaginfo.com.cn", Order=7)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string in7;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws.flaginfo.com.cn", Order=8)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string in8;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws.flaginfo.com.cn", Order=9)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string in9;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws.flaginfo.com.cn", Order=10)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string in10;
        
        public SmsRequest() {
        }
        
        public SmsRequest(string in0, string in1, string in2, string in3, string in4, string in5, string in6, string in7, string in8, string in9, string in10) {
            this.in0 = in0;
            this.in1 = in1;
            this.in2 = in2;
            this.in3 = in3;
            this.in4 = in4;
            this.in5 = in5;
            this.in6 = in6;
            this.in7 = in7;
            this.in8 = in8;
            this.in9 = in9;
            this.in10 = in10;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="SmsResponse", WrapperNamespace="http://ws.flaginfo.com.cn", IsWrapped=true)]
    public partial class SmsResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws.flaginfo.com.cn", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string @out;
        
        public SmsResponse() {
        }
        
        public SmsResponse(string @out) {
            this.@out = @out;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="Report", WrapperNamespace="http://ws.flaginfo.com.cn", IsWrapped=true)]
    public partial class ReportRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws.flaginfo.com.cn", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string in0;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws.flaginfo.com.cn", Order=1)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string in1;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws.flaginfo.com.cn", Order=2)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string in2;
        
        public ReportRequest() {
        }
        
        public ReportRequest(string in0, string in1, string in2) {
            this.in0 = in0;
            this.in1 = in1;
            this.in2 = in2;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="ReportResponse", WrapperNamespace="http://ws.flaginfo.com.cn", IsWrapped=true)]
    public partial class ReportResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws.flaginfo.com.cn", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string @out;
        
        public ReportResponse() {
        }
        
        public ReportResponse(string @out) {
            this.@out = @out;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1064.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://ws.flaginfo.com.cn")]
    public partial class Reply : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string callMdnField;
        
        private string mdnField;
        
        private string contentField;
        
        private string reply_timeField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true, Order=0)]
        public string callMdn {
            get {
                return this.callMdnField;
            }
            set {
                this.callMdnField = value;
                this.RaisePropertyChanged("callMdn");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true, Order=1)]
        public string mdn {
            get {
                return this.mdnField;
            }
            set {
                this.mdnField = value;
                this.RaisePropertyChanged("mdn");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true, Order=2)]
        public string content {
            get {
                return this.contentField;
            }
            set {
                this.contentField = value;
                this.RaisePropertyChanged("content");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true, Order=3)]
        public string reply_time {
            get {
                return this.reply_timeField;
            }
            set {
                this.reply_timeField = value;
                this.RaisePropertyChanged("reply_time");
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="ReplyRequest", WrapperNamespace="http://ws.flaginfo.com.cn", IsWrapped=true)]
    public partial class ReplyRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws.flaginfo.com.cn", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string in0;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws.flaginfo.com.cn", Order=1)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string in1;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws.flaginfo.com.cn", Order=2)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string in2;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws.flaginfo.com.cn", Order=3)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string in3;
        
        public ReplyRequest() {
        }
        
        public ReplyRequest(string in0, string in1, string in2, string in3) {
            this.in0 = in0;
            this.in1 = in1;
            this.in2 = in2;
            this.in3 = in3;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="ReplyResponse", WrapperNamespace="http://ws.flaginfo.com.cn", IsWrapped=true)]
    public partial class ReplyResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws.flaginfo.com.cn", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string result;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws.flaginfo.com.cn", Order=1)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string confirm_time;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws.flaginfo.com.cn", Order=2)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string id;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws.flaginfo.com.cn", Order=3)]
        [System.Xml.Serialization.XmlElementAttribute("replys", IsNullable=true)]
        public AppWebAPI.SR.Reply[] replys;
        
        public ReplyResponse() {
        }
        
        public ReplyResponse(string result, string confirm_time, string id, AppWebAPI.SR.Reply[] replys) {
            this.result = result;
            this.confirm_time = confirm_time;
            this.id = id;
            this.replys = replys;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="ReplyConfirmRequest", WrapperNamespace="http://ws.flaginfo.com.cn", IsWrapped=true)]
    public partial class ReplyConfirmRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws.flaginfo.com.cn", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string in0;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws.flaginfo.com.cn", Order=1)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string in1;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws.flaginfo.com.cn", Order=2)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string in2;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws.flaginfo.com.cn", Order=3)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string in3;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws.flaginfo.com.cn", Order=4)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string in4;
        
        public ReplyConfirmRequest() {
        }
        
        public ReplyConfirmRequest(string in0, string in1, string in2, string in3, string in4) {
            this.in0 = in0;
            this.in1 = in1;
            this.in2 = in2;
            this.in3 = in3;
            this.in4 = in4;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="ReplyConfirmResponse", WrapperNamespace="http://ws.flaginfo.com.cn", IsWrapped=true)]
    public partial class ReplyConfirmResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws.flaginfo.com.cn", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string result;
        
        public ReplyConfirmResponse() {
        }
        
        public ReplyConfirmResponse(string result) {
            this.result = result;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="SearchSmsNumRequest", WrapperNamespace="http://ws.flaginfo.com.cn", IsWrapped=true)]
    public partial class SearchSmsNumRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws.flaginfo.com.cn", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string in0;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws.flaginfo.com.cn", Order=1)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string in1;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws.flaginfo.com.cn", Order=2)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string in2;
        
        public SearchSmsNumRequest() {
        }
        
        public SearchSmsNumRequest(string in0, string in1, string in2) {
            this.in0 = in0;
            this.in1 = in1;
            this.in2 = in2;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="SearchSmsNumResponse", WrapperNamespace="http://ws.flaginfo.com.cn", IsWrapped=true)]
    public partial class SearchSmsNumResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws.flaginfo.com.cn", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string result;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws.flaginfo.com.cn", Order=1)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string number;
        
        public SearchSmsNumResponse() {
        }
        
        public SearchSmsNumResponse(string result, string number) {
            this.result = result;
            this.number = number;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="AuditingRequest", WrapperNamespace="http://ws.flaginfo.com.cn", IsWrapped=true)]
    public partial class AuditingRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws.flaginfo.com.cn", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string in0;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws.flaginfo.com.cn", Order=1)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string in1;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws.flaginfo.com.cn", Order=2)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string in2;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws.flaginfo.com.cn", Order=3)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string in3;
        
        public AuditingRequest() {
        }
        
        public AuditingRequest(string in0, string in1, string in2, string in3) {
            this.in0 = in0;
            this.in1 = in1;
            this.in2 = in2;
            this.in3 = in3;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="AuditingResponse", WrapperNamespace="http://ws.flaginfo.com.cn", IsWrapped=true)]
    public partial class AuditingResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws.flaginfo.com.cn", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string @out;
        
        public AuditingResponse() {
        }
        
        public AuditingResponse(string @out) {
            this.@out = @out;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface SmsPortTypeChannel : AppWebAPI.SR.SmsPortType, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class SmsPortTypeClient : System.ServiceModel.ClientBase<AppWebAPI.SR.SmsPortType>, AppWebAPI.SR.SmsPortType {
        
        public SmsPortTypeClient() {
        }
        
        public SmsPortTypeClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public SmsPortTypeClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public SmsPortTypeClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public SmsPortTypeClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        AppWebAPI.SR.SmsResponse AppWebAPI.SR.SmsPortType.Sms(AppWebAPI.SR.SmsRequest request) {
            return base.Channel.Sms(request);
        }
        
        public string Sms(string in0, string in1, string in2, string in3, string in4, string in5, string in6, string in7, string in8, string in9, string in10) {
            AppWebAPI.SR.SmsRequest inValue = new AppWebAPI.SR.SmsRequest();
            inValue.in0 = in0;
            inValue.in1 = in1;
            inValue.in2 = in2;
            inValue.in3 = in3;
            inValue.in4 = in4;
            inValue.in5 = in5;
            inValue.in6 = in6;
            inValue.in7 = in7;
            inValue.in8 = in8;
            inValue.in9 = in9;
            inValue.in10 = in10;
            AppWebAPI.SR.SmsResponse retVal = ((AppWebAPI.SR.SmsPortType)(this)).Sms(inValue);
            return retVal.@out;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<AppWebAPI.SR.SmsResponse> AppWebAPI.SR.SmsPortType.SmsAsync(AppWebAPI.SR.SmsRequest request) {
            return base.Channel.SmsAsync(request);
        }
        
        public System.Threading.Tasks.Task<AppWebAPI.SR.SmsResponse> SmsAsync(string in0, string in1, string in2, string in3, string in4, string in5, string in6, string in7, string in8, string in9, string in10) {
            AppWebAPI.SR.SmsRequest inValue = new AppWebAPI.SR.SmsRequest();
            inValue.in0 = in0;
            inValue.in1 = in1;
            inValue.in2 = in2;
            inValue.in3 = in3;
            inValue.in4 = in4;
            inValue.in5 = in5;
            inValue.in6 = in6;
            inValue.in7 = in7;
            inValue.in8 = in8;
            inValue.in9 = in9;
            inValue.in10 = in10;
            return ((AppWebAPI.SR.SmsPortType)(this)).SmsAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        AppWebAPI.SR.ReportResponse AppWebAPI.SR.SmsPortType.Report(AppWebAPI.SR.ReportRequest request) {
            return base.Channel.Report(request);
        }
        
        public string Report(string in0, string in1, string in2) {
            AppWebAPI.SR.ReportRequest inValue = new AppWebAPI.SR.ReportRequest();
            inValue.in0 = in0;
            inValue.in1 = in1;
            inValue.in2 = in2;
            AppWebAPI.SR.ReportResponse retVal = ((AppWebAPI.SR.SmsPortType)(this)).Report(inValue);
            return retVal.@out;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<AppWebAPI.SR.ReportResponse> AppWebAPI.SR.SmsPortType.ReportAsync(AppWebAPI.SR.ReportRequest request) {
            return base.Channel.ReportAsync(request);
        }
        
        public System.Threading.Tasks.Task<AppWebAPI.SR.ReportResponse> ReportAsync(string in0, string in1, string in2) {
            AppWebAPI.SR.ReportRequest inValue = new AppWebAPI.SR.ReportRequest();
            inValue.in0 = in0;
            inValue.in1 = in1;
            inValue.in2 = in2;
            return ((AppWebAPI.SR.SmsPortType)(this)).ReportAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        AppWebAPI.SR.ReplyResponse AppWebAPI.SR.SmsPortType.Reply(AppWebAPI.SR.ReplyRequest request) {
            return base.Channel.Reply(request);
        }
        
        public string Reply(string in0, string in1, string in2, string in3, out string confirm_time, out string id, out AppWebAPI.SR.Reply[] replys) {
            AppWebAPI.SR.ReplyRequest inValue = new AppWebAPI.SR.ReplyRequest();
            inValue.in0 = in0;
            inValue.in1 = in1;
            inValue.in2 = in2;
            inValue.in3 = in3;
            AppWebAPI.SR.ReplyResponse retVal = ((AppWebAPI.SR.SmsPortType)(this)).Reply(inValue);
            confirm_time = retVal.confirm_time;
            id = retVal.id;
            replys = retVal.replys;
            return retVal.result;
        }
        
        public System.Threading.Tasks.Task<AppWebAPI.SR.ReplyResponse> ReplyAsync(AppWebAPI.SR.ReplyRequest request) {
            return base.Channel.ReplyAsync(request);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        AppWebAPI.SR.ReplyConfirmResponse AppWebAPI.SR.SmsPortType.ReplyConfirm(AppWebAPI.SR.ReplyConfirmRequest request) {
            return base.Channel.ReplyConfirm(request);
        }
        
        public string ReplyConfirm(string in0, string in1, string in2, string in3, string in4) {
            AppWebAPI.SR.ReplyConfirmRequest inValue = new AppWebAPI.SR.ReplyConfirmRequest();
            inValue.in0 = in0;
            inValue.in1 = in1;
            inValue.in2 = in2;
            inValue.in3 = in3;
            inValue.in4 = in4;
            AppWebAPI.SR.ReplyConfirmResponse retVal = ((AppWebAPI.SR.SmsPortType)(this)).ReplyConfirm(inValue);
            return retVal.result;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<AppWebAPI.SR.ReplyConfirmResponse> AppWebAPI.SR.SmsPortType.ReplyConfirmAsync(AppWebAPI.SR.ReplyConfirmRequest request) {
            return base.Channel.ReplyConfirmAsync(request);
        }
        
        public System.Threading.Tasks.Task<AppWebAPI.SR.ReplyConfirmResponse> ReplyConfirmAsync(string in0, string in1, string in2, string in3, string in4) {
            AppWebAPI.SR.ReplyConfirmRequest inValue = new AppWebAPI.SR.ReplyConfirmRequest();
            inValue.in0 = in0;
            inValue.in1 = in1;
            inValue.in2 = in2;
            inValue.in3 = in3;
            inValue.in4 = in4;
            return ((AppWebAPI.SR.SmsPortType)(this)).ReplyConfirmAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        AppWebAPI.SR.SearchSmsNumResponse AppWebAPI.SR.SmsPortType.SearchSmsNum(AppWebAPI.SR.SearchSmsNumRequest request) {
            return base.Channel.SearchSmsNum(request);
        }
        
        public string SearchSmsNum(string in0, string in1, string in2, out string number) {
            AppWebAPI.SR.SearchSmsNumRequest inValue = new AppWebAPI.SR.SearchSmsNumRequest();
            inValue.in0 = in0;
            inValue.in1 = in1;
            inValue.in2 = in2;
            AppWebAPI.SR.SearchSmsNumResponse retVal = ((AppWebAPI.SR.SmsPortType)(this)).SearchSmsNum(inValue);
            number = retVal.number;
            return retVal.result;
        }
        
        public System.Threading.Tasks.Task<AppWebAPI.SR.SearchSmsNumResponse> SearchSmsNumAsync(AppWebAPI.SR.SearchSmsNumRequest request) {
            return base.Channel.SearchSmsNumAsync(request);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        AppWebAPI.SR.AuditingResponse AppWebAPI.SR.SmsPortType.Auditing(AppWebAPI.SR.AuditingRequest request) {
            return base.Channel.Auditing(request);
        }
        
        public string Auditing(string in0, string in1, string in2, string in3) {
            AppWebAPI.SR.AuditingRequest inValue = new AppWebAPI.SR.AuditingRequest();
            inValue.in0 = in0;
            inValue.in1 = in1;
            inValue.in2 = in2;
            inValue.in3 = in3;
            AppWebAPI.SR.AuditingResponse retVal = ((AppWebAPI.SR.SmsPortType)(this)).Auditing(inValue);
            return retVal.@out;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<AppWebAPI.SR.AuditingResponse> AppWebAPI.SR.SmsPortType.AuditingAsync(AppWebAPI.SR.AuditingRequest request) {
            return base.Channel.AuditingAsync(request);
        }
        
        public System.Threading.Tasks.Task<AppWebAPI.SR.AuditingResponse> AuditingAsync(string in0, string in1, string in2, string in3) {
            AppWebAPI.SR.AuditingRequest inValue = new AppWebAPI.SR.AuditingRequest();
            inValue.in0 = in0;
            inValue.in1 = in1;
            inValue.in2 = in2;
            inValue.in3 = in3;
            return ((AppWebAPI.SR.SmsPortType)(this)).AuditingAsync(inValue);
        }
    }
}
