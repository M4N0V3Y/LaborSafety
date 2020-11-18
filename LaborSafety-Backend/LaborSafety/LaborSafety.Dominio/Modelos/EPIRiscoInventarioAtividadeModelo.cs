using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaborSafety.Dominio.Modelos
{
    public class EPIRiscoInventarioAtividadeModelo
    {
        public long CodEpiRiscoInventarioAtividade { get; set; }
        public long CodEPI { get; set; }
        public long CodRiscoInventarioAtividade { get; set; }
        public List<string> NomeEPI { get; set; }
    }
}
