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
    
    public partial class NR
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public NR()
        {
            this.NR_INVENTARIO_AMBIENTE = new HashSet<NR_INVENTARIO_AMBIENTE>();
            this.NR_RASCUNHO_INVENTARIO_AMBIENTE = new HashSet<NR_RASCUNHO_INVENTARIO_AMBIENTE>();
        }
    
        public long CodNR { get; set; }
        public string Codigo { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NR_INVENTARIO_AMBIENTE> NR_INVENTARIO_AMBIENTE { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NR_RASCUNHO_INVENTARIO_AMBIENTE> NR_RASCUNHO_INVENTARIO_AMBIENTE { get; set; }
    }
}
