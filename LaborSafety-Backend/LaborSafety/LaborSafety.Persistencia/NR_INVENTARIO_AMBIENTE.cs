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
    
    public partial class NR_INVENTARIO_AMBIENTE
    {
        public long CodNRInventarioAmbiente { get; set; }
        public long CodNR { get; set; }
        public long CodInventarioAmbiente { get; set; }
        public bool Ativo { get; set; }
    
        public virtual INVENTARIO_AMBIENTE INVENTARIO_AMBIENTE { get; set; }
        public virtual NR NR { get; set; }
    }
}
