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
    
    public partial class LOG_RISCO_INVENTARIO_AMBIENTE
    {
        public long CodLogRiscoInventarioAmbiente { get; set; }
        public long CodLogInventarioAmbiente { get; set; }
        public long CodRisco { get; set; }
        public Nullable<long> CodSeveridade { get; set; }
        public Nullable<long> CodProbabilidade { get; set; }
        public string FonteGeradora { get; set; }
        public string ProcedimentosAplicaveis { get; set; }
        public string ContraMedidas { get; set; }
        public string CodigosEPIs { get; set; }
    
        public virtual LOG_INVENTARIO_AMBIENTE LOG_INVENTARIO_AMBIENTE { get; set; }
    }
}
