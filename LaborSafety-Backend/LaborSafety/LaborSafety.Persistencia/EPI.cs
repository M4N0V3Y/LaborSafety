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
    
    public partial class EPI
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public EPI()
        {
            this.EPI_RISCO = new HashSet<EPI_RISCO>();
            this.EPI_RISCO_INVENTARIO_AMBIENTE = new HashSet<EPI_RISCO_INVENTARIO_AMBIENTE>();
            this.EPI_RISCO_INVENTARIO_ATIVIDADE = new HashSet<EPI_RISCO_INVENTARIO_ATIVIDADE>();
            this.EPI_RISCO_RASCUNHO = new HashSet<EPI_RISCO_RASCUNHO>();
            this.EPI_RISCO_RASCUNHO_INVENTARIO_AMBIENTE = new HashSet<EPI_RISCO_RASCUNHO_INVENTARIO_AMBIENTE>();
            this.EPI_RISCO_RASCUNHO_INVENTARIO_ATIVIDADE = new HashSet<EPI_RISCO_RASCUNHO_INVENTARIO_ATIVIDADE>();
        }
    
        public long CodEPI { get; set; }
        public string N1 { get; set; }
        public string N2 { get; set; }
        public string N3 { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EPI_RISCO> EPI_RISCO { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EPI_RISCO_INVENTARIO_AMBIENTE> EPI_RISCO_INVENTARIO_AMBIENTE { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EPI_RISCO_INVENTARIO_ATIVIDADE> EPI_RISCO_INVENTARIO_ATIVIDADE { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EPI_RISCO_RASCUNHO> EPI_RISCO_RASCUNHO { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EPI_RISCO_RASCUNHO_INVENTARIO_AMBIENTE> EPI_RISCO_RASCUNHO_INVENTARIO_AMBIENTE { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EPI_RISCO_RASCUNHO_INVENTARIO_ATIVIDADE> EPI_RISCO_RASCUNHO_INVENTARIO_ATIVIDADE { get; set; }
    }
}
