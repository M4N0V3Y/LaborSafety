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
    
    public partial class PESO
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PESO()
        {
            this.INVENTARIO_ATIVIDADE = new HashSet<INVENTARIO_ATIVIDADE>();
            this.LOCAL_INSTALACAO = new HashSet<LOCAL_INSTALACAO>();
        }
    
        public long CodPeso { get; set; }
        public int Indice { get; set; }
        public string Nome { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<INVENTARIO_ATIVIDADE> INVENTARIO_ATIVIDADE { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LOCAL_INSTALACAO> LOCAL_INSTALACAO { get; set; }
    }
}
