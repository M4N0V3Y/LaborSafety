using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaborSafety.Dominio.Modelos.Exportacao
{
    public class DadosExportacaoAtividadeModelo
    {
        public List<long> ATIVIDADE_PADRAO { get; set; }
        public List<long> DISCIPLINA { get; set; }
        public List<long> PESO { get; set; }
        public List<long> PERFIL_CATALOGO { get; set; }
        public List<long> SEVERIDADE { get; set; }
        public List<long> RISCO { get; set; }
        public List<long> LOCAL_INSTALACAO { get; set; }
        public List<long> EPI { get; set; }
    }
}
