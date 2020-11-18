﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Ternium.LaborSafety.Integracao.SAP_03_LOCAL_INSTALACAO_SERVICE {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://ternium.com.br/INT_FromERP_toAPR_PT", ConfigurationName="SAP_03_LOCAL_INSTALACAO_SERVICE.SI_LOCALINST_LaborSafety_RFC03_Out")]
    public interface SI_LOCALINST_LaborSafety_RFC03_Out {
        
        // CODEGEN: Generating message contract since the operation SI_LOCALINST_LaborSafety_RFC03_Out is neither RPC nor document wrapped.
        [System.ServiceModel.OperationContractAttribute(Action="http://sap.com/xi/WebService/soap1.1", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        Ternium.LaborSafety.Integracao.SAP_03_LOCAL_INSTALACAO_SERVICE.SI_LOCALINST_LaborSafety_RFC03_OutResponse SI_LOCALINST_LaborSafety_RFC03_Out(Ternium.LaborSafety.Integracao.SAP_03_LOCAL_INSTALACAO_SERVICE.SI_LOCALINST_LaborSafety_RFC03_OutRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://sap.com/xi/WebService/soap1.1", ReplyAction="*")]
        System.Threading.Tasks.Task<Ternium.LaborSafety.Integracao.SAP_03_LOCAL_INSTALACAO_SERVICE.SI_LOCALINST_LaborSafety_RFC03_OutResponse> SI_LOCALINST_LaborSafety_RFC03_OutAsync(Ternium.LaborSafety.Integracao.SAP_03_LOCAL_INSTALACAO_SERVICE.SI_LOCALINST_LaborSafety_RFC03_OutRequest request);
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3752.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://ternium.com.br/INT_FromERP_toAPR_PT")]
    public partial class DT_LOCALINST_LaborSafety_RFC03Request : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string local_InstalacaoField;
        
        private string desc_Loc_InstalacaoField;
        
        private string perfil_Loc_InstalacaoField;
        
        private string desc_CatalogoField;
        
        private string classe_Loc_InstalacaoField;
        
        private string desc_ClasseField;
        
        private string caracteristica_ClasseField;
        
        private string desc_CaracteristicaField;
        
        private string valor_CaracteristicaField;
        
        private string status_Loc_InstalacaoField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public string Local_Instalacao {
            get {
                return this.local_InstalacaoField;
            }
            set {
                this.local_InstalacaoField = value;
                this.RaisePropertyChanged("Local_Instalacao");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=1)]
        public string Desc_Loc_Instalacao {
            get {
                return this.desc_Loc_InstalacaoField;
            }
            set {
                this.desc_Loc_InstalacaoField = value;
                this.RaisePropertyChanged("Desc_Loc_Instalacao");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=2)]
        public string Perfil_Loc_Instalacao {
            get {
                return this.perfil_Loc_InstalacaoField;
            }
            set {
                this.perfil_Loc_InstalacaoField = value;
                this.RaisePropertyChanged("Perfil_Loc_Instalacao");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=3)]
        public string Desc_Catalogo {
            get {
                return this.desc_CatalogoField;
            }
            set {
                this.desc_CatalogoField = value;
                this.RaisePropertyChanged("Desc_Catalogo");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=4)]
        public string Classe_Loc_Instalacao {
            get {
                return this.classe_Loc_InstalacaoField;
            }
            set {
                this.classe_Loc_InstalacaoField = value;
                this.RaisePropertyChanged("Classe_Loc_Instalacao");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=5)]
        public string Desc_Classe {
            get {
                return this.desc_ClasseField;
            }
            set {
                this.desc_ClasseField = value;
                this.RaisePropertyChanged("Desc_Classe");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=6)]
        public string Caracteristica_Classe {
            get {
                return this.caracteristica_ClasseField;
            }
            set {
                this.caracteristica_ClasseField = value;
                this.RaisePropertyChanged("Caracteristica_Classe");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=7)]
        public string Desc_Caracteristica {
            get {
                return this.desc_CaracteristicaField;
            }
            set {
                this.desc_CaracteristicaField = value;
                this.RaisePropertyChanged("Desc_Caracteristica");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=8)]
        public string Valor_Caracteristica {
            get {
                return this.valor_CaracteristicaField;
            }
            set {
                this.valor_CaracteristicaField = value;
                this.RaisePropertyChanged("Valor_Caracteristica");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=9)]
        public string Status_Loc_Instalacao {
            get {
                return this.status_Loc_InstalacaoField;
            }
            set {
                this.status_Loc_InstalacaoField = value;
                this.RaisePropertyChanged("Status_Loc_Instalacao");
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3752.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://ternium.com.br/INT_FromERP_toAPR_PT")]
    public partial class DT_LOCALINST_LaborSafety_RFC03_Response : object, System.ComponentModel.INotifyPropertyChanged {
        
        private DT_LOCALINST_LaborSafety_RFC03_ResponseResponse responseField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public DT_LOCALINST_LaborSafety_RFC03_ResponseResponse Response {
            get {
                return this.responseField;
            }
            set {
                this.responseField = value;
                this.RaisePropertyChanged("Response");
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3752.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://ternium.com.br/INT_FromERP_toAPR_PT")]
    public partial class DT_LOCALINST_LaborSafety_RFC03_ResponseResponse : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string statusField;
        
        private string descricaoField;
        
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
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=1)]
        public string Descricao {
            get {
                return this.descricaoField;
            }
            set {
                this.descricaoField = value;
                this.RaisePropertyChanged("Descricao");
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
    public partial class SI_LOCALINST_LaborSafety_RFC03_OutRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ternium.com.br/INT_FromERP_toAPR_PT", Order=0)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Request", Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=false)]
        public Ternium.LaborSafety.Integracao.SAP_03_LOCAL_INSTALACAO_SERVICE.DT_LOCALINST_LaborSafety_RFC03Request[] MT_LOCALINST_LaborSafety_RFC03;
        
        public SI_LOCALINST_LaborSafety_RFC03_OutRequest() {
        }
        
        public SI_LOCALINST_LaborSafety_RFC03_OutRequest(Ternium.LaborSafety.Integracao.SAP_03_LOCAL_INSTALACAO_SERVICE.DT_LOCALINST_LaborSafety_RFC03Request[] MT_LOCALINST_LaborSafety_RFC03) {
            this.MT_LOCALINST_LaborSafety_RFC03 = MT_LOCALINST_LaborSafety_RFC03;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class SI_LOCALINST_LaborSafety_RFC03_OutResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ternium.com.br/INT_FromERP_toAPR_PT", Order=0)]
        public Ternium.LaborSafety.Integracao.SAP_03_LOCAL_INSTALACAO_SERVICE.DT_LOCALINST_LaborSafety_RFC03_Response MT_LOCALINST_LaborSafety_RFC03_Response;
        
        public SI_LOCALINST_LaborSafety_RFC03_OutResponse() {
        }
        
        public SI_LOCALINST_LaborSafety_RFC03_OutResponse(Ternium.LaborSafety.Integracao.SAP_03_LOCAL_INSTALACAO_SERVICE.DT_LOCALINST_LaborSafety_RFC03_Response MT_LOCALINST_LaborSafety_RFC03_Response) {
            this.MT_LOCALINST_LaborSafety_RFC03_Response = MT_LOCALINST_LaborSafety_RFC03_Response;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface SI_LOCALINST_LaborSafety_RFC03_OutChannel : Ternium.LaborSafety.Integracao.SAP_03_LOCAL_INSTALACAO_SERVICE.SI_LOCALINST_LaborSafety_RFC03_Out, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class SI_LOCALINST_LaborSafety_RFC03_OutClient : System.ServiceModel.ClientBase<Ternium.LaborSafety.Integracao.SAP_03_LOCAL_INSTALACAO_SERVICE.SI_LOCALINST_LaborSafety_RFC03_Out>, Ternium.LaborSafety.Integracao.SAP_03_LOCAL_INSTALACAO_SERVICE.SI_LOCALINST_LaborSafety_RFC03_Out {
        
        public SI_LOCALINST_LaborSafety_RFC03_OutClient() {
        }
        
        public SI_LOCALINST_LaborSafety_RFC03_OutClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public SI_LOCALINST_LaborSafety_RFC03_OutClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public SI_LOCALINST_LaborSafety_RFC03_OutClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public SI_LOCALINST_LaborSafety_RFC03_OutClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        Ternium.LaborSafety.Integracao.SAP_03_LOCAL_INSTALACAO_SERVICE.SI_LOCALINST_LaborSafety_RFC03_OutResponse Ternium.LaborSafety.Integracao.SAP_03_LOCAL_INSTALACAO_SERVICE.SI_LOCALINST_LaborSafety_RFC03_Out.SI_LOCALINST_LaborSafety_RFC03_Out(Ternium.LaborSafety.Integracao.SAP_03_LOCAL_INSTALACAO_SERVICE.SI_LOCALINST_LaborSafety_RFC03_OutRequest request) {
            return base.Channel.SI_LOCALINST_LaborSafety_RFC03_Out(request);
        }
        
        public Ternium.LaborSafety.Integracao.SAP_03_LOCAL_INSTALACAO_SERVICE.DT_LOCALINST_LaborSafety_RFC03_Response SI_LOCALINST_LaborSafety_RFC03_Out(Ternium.LaborSafety.Integracao.SAP_03_LOCAL_INSTALACAO_SERVICE.DT_LOCALINST_LaborSafety_RFC03Request[] MT_LOCALINST_LaborSafety_RFC03) {
            Ternium.LaborSafety.Integracao.SAP_03_LOCAL_INSTALACAO_SERVICE.SI_LOCALINST_LaborSafety_RFC03_OutRequest inValue = new Ternium.LaborSafety.Integracao.SAP_03_LOCAL_INSTALACAO_SERVICE.SI_LOCALINST_LaborSafety_RFC03_OutRequest();
            inValue.MT_LOCALINST_LaborSafety_RFC03 = MT_LOCALINST_LaborSafety_RFC03;
            Ternium.LaborSafety.Integracao.SAP_03_LOCAL_INSTALACAO_SERVICE.SI_LOCALINST_LaborSafety_RFC03_OutResponse retVal = ((Ternium.LaborSafety.Integracao.SAP_03_LOCAL_INSTALACAO_SERVICE.SI_LOCALINST_LaborSafety_RFC03_Out)(this)).SI_LOCALINST_LaborSafety_RFC03_Out(inValue);
            return retVal.MT_LOCALINST_LaborSafety_RFC03_Response;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<Ternium.LaborSafety.Integracao.SAP_03_LOCAL_INSTALACAO_SERVICE.SI_LOCALINST_LaborSafety_RFC03_OutResponse> Ternium.LaborSafety.Integracao.SAP_03_LOCAL_INSTALACAO_SERVICE.SI_LOCALINST_LaborSafety_RFC03_Out.SI_LOCALINST_LaborSafety_RFC03_OutAsync(Ternium.LaborSafety.Integracao.SAP_03_LOCAL_INSTALACAO_SERVICE.SI_LOCALINST_LaborSafety_RFC03_OutRequest request) {
            return base.Channel.SI_LOCALINST_LaborSafety_RFC03_OutAsync(request);
        }
        
        public System.Threading.Tasks.Task<Ternium.LaborSafety.Integracao.SAP_03_LOCAL_INSTALACAO_SERVICE.SI_LOCALINST_LaborSafety_RFC03_OutResponse> SI_LOCALINST_LaborSafety_RFC03_OutAsync(Ternium.LaborSafety.Integracao.SAP_03_LOCAL_INSTALACAO_SERVICE.DT_LOCALINST_LaborSafety_RFC03Request[] MT_LOCALINST_LaborSafety_RFC03) {
            Ternium.LaborSafety.Integracao.SAP_03_LOCAL_INSTALACAO_SERVICE.SI_LOCALINST_LaborSafety_RFC03_OutRequest inValue = new Ternium.LaborSafety.Integracao.SAP_03_LOCAL_INSTALACAO_SERVICE.SI_LOCALINST_LaborSafety_RFC03_OutRequest();
            inValue.MT_LOCALINST_LaborSafety_RFC03 = MT_LOCALINST_LaborSafety_RFC03;
            return ((Ternium.LaborSafety.Integracao.SAP_03_LOCAL_INSTALACAO_SERVICE.SI_LOCALINST_LaborSafety_RFC03_Out)(this)).SI_LOCALINST_LaborSafety_RFC03_OutAsync(inValue);
        }
    }
}