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
    
    public partial class LOG_INVENTARIO_ATIVIDADE
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public LOG_INVENTARIO_ATIVIDADE()
        {
            this.LOG_RISCO_INVENTARIO_ATIVIDADE = new HashSet<LOG_RISCO_INVENTARIO_ATIVIDADE>();
        }
    
        public long CodLogInventarioAtividade { get; set; }
        public long CodLogTipoOperacao { get; set; }
        public string CodInventariosAntigos { get; set; }
        public long CodInventarioAtividade { get; set; }
        public string CodUsuarioModificador { get; set; }
        public Nullable<long> CodPesoAntigo { get; set; }
        public Nullable<long> CodPesoNovo { get; set; }
        public Nullable<long> CodPerfilCatalogoAntigo { get; set; }
        public Nullable<long> CodPerfilCatalogoNovo { get; set; }
        public Nullable<long> CodDuracaoAntiga { get; set; }
        public Nullable<long> CodDuracaoNovo { get; set; }
        public Nullable<long> CodAtividadeAntiga { get; set; }
        public Nullable<long> CodAtividadeNova { get; set; }
        public Nullable<long> CodDisciplinaAntiga { get; set; }
        public Nullable<long> CodDisciplinaNova { get; set; }
        public string CodLIsAntigos { get; set; }
        public string CodLIsNovos { get; set; }
        public string DescricaoAntiga { get; set; }
        public string DescricaoNova { get; set; }
        public Nullable<int> RiscoGeralAntigo { get; set; }
        public Nullable<int> RiscoGeralNovo { get; set; }
        public string ObsGeralAntiga { get; set; }
        public string ObsGeralNova { get; set; }
        public System.DateTime DataAlteracao { get; set; }
    
        public virtual LOG_TIPO_OPERACAO LOG_TIPO_OPERACAO { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LOG_RISCO_INVENTARIO_ATIVIDADE> LOG_RISCO_INVENTARIO_ATIVIDADE { get; set; }
    }
}