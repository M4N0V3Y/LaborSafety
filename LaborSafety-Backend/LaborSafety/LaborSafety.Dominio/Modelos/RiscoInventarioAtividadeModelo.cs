using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaborSafety.Dominio.Modelos
{
    public class RiscoInventarioAtividadeModelo
    {
        public long CodRiscoInventarioAtividade { get; set; }
        public long CodRisco { get; set; }
        public long CodInventarioAtividade { get; set; }
        public long CodSeveridade { get; set; }
        public string FonteGeradora { get; set; }
        public string ProcedimentoAplicavel { get; set; }
        public string ContraMedidas { get; set; }
        public bool Ativo { get; set; }
        public List<EPIRiscoInventarioAtividadeModelo> EPIRiscoInventarioAtividadeModelo { get; set; }
    }
}
