using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaborSafety.Dominio.Modelos
{
    public class EPIRiscoRascunhoModelo
    {
        public long CodEPIRiscoRascunho { get; set; }
        public long CodEPI { get; set; }
        public long CodRiscoRascunhoInventarioAtividade { get; set; }
        public bool Ativo { get; set; }
    }
}
