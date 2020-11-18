using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaborSafety.Dominio.Modelos
{
    public class EPIRiscoInventarioAmbienteModelo
    {
        public long CodEpiRiscoInventarioAmbiente { get; set; }
        public long CodEPI { get; set; }
        public long CodRiscoInventarioAmbiente { get; set; }

        public List<string> NomeEPI { get; set; }
    }
}
