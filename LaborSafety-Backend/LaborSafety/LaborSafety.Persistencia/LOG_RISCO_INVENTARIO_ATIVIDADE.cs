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
    
    public partial class LOG_RISCO_INVENTARIO_ATIVIDADE
    {
        public long CodLogRiscoInventarioAtividade { get; set; }
        public long CodLogInventarioAtividade { get; set; }
        public long CodRiscoAtividade { get; set; }
        public Nullable<long> CodSeveridade { get; set; }
        public string FonteGeradora { get; set; }
        public string ProcedimentoAplicavel { get; set; }
        public string ContraMedidas { get; set; }
        public string CodigosEPIs { get; set; }
    
        public virtual LOG_INVENTARIO_ATIVIDADE LOG_INVENTARIO_ATIVIDADE { get; set; }
    }
}
