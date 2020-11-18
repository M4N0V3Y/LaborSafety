//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LaborSafety.Persistencia
{
    using System;
    using System.Collections.Generic;
    
    public partial class APR
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public APR()
        {
            this.APROVADOR_APR = new HashSet<APROVADOR_APR>();
            this.EXECUTANTE_APR = new HashSet<EXECUTANTE_APR>();
            this.LOG_APR = new HashSet<LOG_APR>();
            this.OPERACAO_APR = new HashSet<OPERACAO_APR>();
            this.RISCO_APR = new HashSet<RISCO_APR>();
        }
    
        public long CodAPR { get; set; }
        public long CodStatusAPR { get; set; }
        public string NumeroSerie { get; set; }
        public string OrdemManutencao { get; set; }
        public string Descricao { get; set; }
        public Nullable<int> RiscoGeral { get; set; }
        public Nullable<System.DateTime> DataAprovacao { get; set; }
        public Nullable<System.DateTime> DataInicio { get; set; }
        public Nullable<System.DateTime> DataEncerramento { get; set; }
        public bool Ativo { get; set; }
        public string LocalInstalacao { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<APROVADOR_APR> APROVADOR_APR { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EXECUTANTE_APR> EXECUTANTE_APR { get; set; }
        public virtual STATUS_APR STATUS_APR { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LOG_APR> LOG_APR { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OPERACAO_APR> OPERACAO_APR { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RISCO_APR> RISCO_APR { get; set; }
    }
}
