﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Ternium.LaborSafety.Negocio.SVC_SAP {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://ternium.com.br/INT_FromERP_toAPR_PT", ConfigurationName="SVC_SAP.SI_Arquivo_Out")]
    public interface SI_Arquivo_Out {
        
        // CODEGEN: Generating message contract since the operation SI_Arquivo_Out is neither RPC nor document wrapped.
        [System.ServiceModel.OperationContractAttribute(Action="http://sap.com/xi/WebService/soap1.1", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        Ternium.LaborSafety.Negocio.SVC_SAP.SI_Arquivo_OutResponse SI_Arquivo_Out(Ternium.LaborSafety.Negocio.SVC_SAP.SI_Arquivo_OutRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://sap.com/xi/WebService/soap1.1", ReplyAction="*")]
        System.Threading.Tasks.Task<Ternium.LaborSafety.Negocio.SVC_SAP.SI_Arquivo_OutResponse> SI_Arquivo_OutAsync(Ternium.LaborSafety.Negocio.SVC_SAP.SI_Arquivo_OutRequest request);
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.3130.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://ternium.com.br/INT_FromERP_toAPR_PT")]
    public partial class DT_Arquivo : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string num_OrdemField;
        
        private string nome_ArquivoField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public string Num_Ordem {
            get {
                return this.num_OrdemField;
            }
            set {
                this.num_OrdemField = value;
                this.RaisePropertyChanged("Num_Ordem");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=1)]
        public string Nome_Arquivo {
            get {
                return this.nome_ArquivoField;
            }
            set {
                this.nome_ArquivoField = value;
                this.RaisePropertyChanged("Nome_Arquivo");
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
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.3130.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://ternium.com.br/INT_FromERP_toAPR_PT")]
    public partial class DT_Arquivo_Response : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string statusField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public string Status {
            get {
                return this.statusField;
            }
            set {
                this.statusField = value;
                this.RaisePropertyChanged("Status");
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
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class SI_Arquivo_OutRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ternium.com.br/INT_FromERP_toAPR_PT", Order=0)]
        public Ternium.LaborSafety.Negocio.SVC_SAP.DT_Arquivo MT_Arquivo;
        
        public SI_Arquivo_OutRequest() {
        }
        
        public SI_Arquivo_OutRequest(Ternium.LaborSafety.Negocio.SVC_SAP.DT_Arquivo MT_Arquivo) {
            this.MT_Arquivo = MT_Arquivo;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class SI_Arquivo_OutResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ternium.com.br/INT_FromERP_toAPR_PT", Order=0)]
        public Ternium.LaborSafety.Negocio.SVC_SAP.DT_Arquivo_Response MT_Arquivo_Response;
        
        public SI_Arquivo_OutResponse() {
        }
        
        public SI_Arquivo_OutResponse(Ternium.LaborSafety.Negocio.SVC_SAP.DT_Arquivo_Response MT_Arquivo_Response) {
            this.MT_Arquivo_Response = MT_Arquivo_Response;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface SI_Arquivo_OutChannel : Ternium.LaborSafety.Negocio.SVC_SAP.SI_Arquivo_Out, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class SI_Arquivo_OutClient : System.ServiceModel.ClientBase<Ternium.LaborSafety.Negocio.SVC_SAP.SI_Arquivo_Out>, Ternium.LaborSafety.Negocio.SVC_SAP.SI_Arquivo_Out {
        
        public SI_Arquivo_OutClient() {
        }
        
        public SI_Arquivo_OutClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public SI_Arquivo_OutClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public SI_Arquivo_OutClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public SI_Arquivo_OutClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        Ternium.LaborSafety.Negocio.SVC_SAP.SI_Arquivo_OutResponse Ternium.LaborSafety.Negocio.SVC_SAP.SI_Arquivo_Out.SI_Arquivo_Out(Ternium.LaborSafety.Negocio.SVC_SAP.SI_Arquivo_OutRequest request) {
            return base.Channel.SI_Arquivo_Out(request);
        }
        
        public Ternium.LaborSafety.Negocio.SVC_SAP.DT_Arquivo_Response SI_Arquivo_Out(Ternium.LaborSafety.Negocio.SVC_SAP.DT_Arquivo MT_Arquivo) {
            Ternium.LaborSafety.Negocio.SVC_SAP.SI_Arquivo_OutRequest inValue = new Ternium.LaborSafety.Negocio.SVC_SAP.SI_Arquivo_OutRequest();
            inValue.MT_Arquivo = MT_Arquivo;
            Ternium.LaborSafety.Negocio.SVC_SAP.SI_Arquivo_OutResponse retVal = ((Ternium.LaborSafety.Negocio.SVC_SAP.SI_Arquivo_Out)(this)).SI_Arquivo_Out(inValue);
            return retVal.MT_Arquivo_Response;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<Ternium.LaborSafety.Negocio.SVC_SAP.SI_Arquivo_OutResponse> Ternium.LaborSafety.Negocio.SVC_SAP.SI_Arquivo_Out.SI_Arquivo_OutAsync(Ternium.LaborSafety.Negocio.SVC_SAP.SI_Arquivo_OutRequest request) {
            return base.Channel.SI_Arquivo_OutAsync(request);
        }
        
        public System.Threading.Tasks.Task<Ternium.LaborSafety.Negocio.SVC_SAP.SI_Arquivo_OutResponse> SI_Arquivo_OutAsync(Ternium.LaborSafety.Negocio.SVC_SAP.DT_Arquivo MT_Arquivo) {
            Ternium.LaborSafety.Negocio.SVC_SAP.SI_Arquivo_OutRequest inValue = new Ternium.LaborSafety.Negocio.SVC_SAP.SI_Arquivo_OutRequest();
            inValue.MT_Arquivo = MT_Arquivo;
            return ((Ternium.LaborSafety.Negocio.SVC_SAP.SI_Arquivo_Out)(this)).SI_Arquivo_OutAsync(inValue);
        }
    }
}