using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaborSafety.Dominio.Modelos
{
    public class FiltroInventarioAmbienteModelo
    {
        public long? CodAmbiente { get; set; }
        public List<long> CodLocaisInstalacao { get; set; }
        public List<long> Riscos { get; set; }
        public long? CodSeveridade { get; set; }
        public long? CodProbabilidade { get; set; }

    }
}
