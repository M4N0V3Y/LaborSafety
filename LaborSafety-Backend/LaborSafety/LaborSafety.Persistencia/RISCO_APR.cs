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
    
    public partial class RISCO_APR
    {
        public long CodRiscoAPR { get; set; }
        public long CodRisco { get; set; }
        public long CodAPR { get; set; }
        public bool Ativo { get; set; }
    
        public virtual APR APR { get; set; }
        public virtual RISCO RISCO { get; set; }
    }
}
