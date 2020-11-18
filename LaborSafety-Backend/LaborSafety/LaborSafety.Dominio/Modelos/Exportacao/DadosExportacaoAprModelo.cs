using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaborSafety.Dominio.Modelos.Exportacao
{
    public class DadosExportacaoAprModelo
    {
        public List<long> LOCAL_INSTALACAO { get; set; }
        public List<long> RISCO_GERAL { get; set; }
        public List<long> RISCO_APR { get; set; }
    }
}
