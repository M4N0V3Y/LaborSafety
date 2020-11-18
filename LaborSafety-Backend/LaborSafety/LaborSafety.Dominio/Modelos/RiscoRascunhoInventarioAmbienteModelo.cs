using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaborSafety.Dominio.Modelos
{
    public class RiscoRascunhoInventarioAmbienteModelo
    {
        public long CodRascunhoRiscoInventarioAmbiente { get; set; }
        public long CodRascunhoInventarioAmbiente { get; set; }
        public long CodRiscoAmbiente { get; set; }
        public long CodSeveridade { get; set; }
        public long CodProbabilidade { get; set; }
        public string FonteGeradora { get; set; }
        public string ProcedimentosAplicaveis { get; set; }
        public string ContraMedidas { get; set; }
        public bool Ativo { get; set; }
        public List<EPIRiscoRascunhoInventarioAmbienteModelo> EPIRiscoRascunhoInventarioAmbiente { get; set; }

        //public List<InventarioAmbienteModelo> INVENTARIO_AMBIENTE { get; set; }
        //public List<ProbabilidadeModelo> PROBABILIDADE { get; set; }
        //public List<RiscoModelo> RISCO { get; set; }
        //public List<SeveridadeModelo> SEVERIDADE { get; set; }
    }
}
