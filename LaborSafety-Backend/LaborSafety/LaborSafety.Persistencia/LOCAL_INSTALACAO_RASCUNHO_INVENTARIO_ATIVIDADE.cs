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
    
    public partial class LOCAL_INSTALACAO_RASCUNHO_INVENTARIO_ATIVIDADE
    {
        public long CodLocalInstalacaoRascunhoInventarioAtividade { get; set; }
        public long CodLocalInstalacao { get; set; }
        public long CodRascunhoInventarioAtividade { get; set; }
    
        public virtual LOCAL_INSTALACAO LOCAL_INSTALACAO { get; set; }
        public virtual RASCUNHO_INVENTARIO_ATIVIDADE RASCUNHO_INVENTARIO_ATIVIDADE { get; set; }
    }
}
