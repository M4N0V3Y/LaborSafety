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
    
    public partial class EPI_RISCO_RASCUNHO
    {
        public long CodEpiRiscoRascunho { get; set; }
        public long CodEpi { get; set; }
        public long CodRisco { get; set; }
    
        public virtual EPI EPI { get; set; }
        public virtual RISCO RISCO { get; set; }
    }
}
