using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaborSafety.Dominio.Modelos.Exportacao
{
    public class DadosExportacaoAmbienteModelo
    {
        public List<long> AMBIENTE { get; set; }
        public List<long> NR { get; set; }
        public List<long> RISCO { get; set; }
        public List<long> PROBABILIDADE { get; set; }
        public List<long> SEVERIDADE { get; set; }
        public List<long> LOCAL_INSTALACAO { get; set; }
        public List<long> EPI { get; set; }
    }
}
