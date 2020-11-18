using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaborSafety.Dominio.Modelos
{
    public class RiscoInventarioAmbienteModelo
    {
        public long CodRiscoInventarioAmbiente { get; set; }
        public long CodInventarioAmbiente { get; set; }
        public long CodRiscoAmbiente { get; set; }
        public long CodSeveridade { get; set; }
        public long CodProbabilidade { get; set; }
        public string FonteGeradora { get; set; }
        public string ProcedimentosAplicaveis { get; set; }
        public string ContraMedidas { get; set; }
        public bool Ativo { get; set; }
        public List<EPIRiscoInventarioAmbienteModelo> EPIRiscoInventarioAmbienteModelo { get; set; }
    }
}
