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
    
    public partial class LOG_TIPO_OPERACAO
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public LOG_TIPO_OPERACAO()
        {
            this.LOG_INVENTARIO_AMBIENTE = new HashSet<LOG_INVENTARIO_AMBIENTE>();
            this.LOG_INVENTARIO_ATIVIDADE = new HashSet<LOG_INVENTARIO_ATIVIDADE>();
        }
    
        public long CodLogTipoOperacao { get; set; }
        public string NomeOperacao { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LOG_INVENTARIO_AMBIENTE> LOG_INVENTARIO_AMBIENTE { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LOG_INVENTARIO_ATIVIDADE> LOG_INVENTARIO_ATIVIDADE { get; set; }
    }
}
