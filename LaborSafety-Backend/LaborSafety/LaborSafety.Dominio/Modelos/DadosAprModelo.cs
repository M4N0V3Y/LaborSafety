using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaborSafety.Dominio.Modelos
{
    public class DadosAprModelo
    {
        public string OrdemManutencao { get; set; }
        public string DescricaoAtividade { get; set; }
        public string LocalInstalacao { get; set; }
        public bool OrigemTela { get; set; }
        public List<AprOperacao> Operacoes { get; set; }

        public bool? Ativo { get; set; }
        public string NumeroSerie { get; set; }

    }

    public class AprOperacao
    {
        public long CodDisciplina { get; set; }
        public long CodAtvPadrao { get; set; }
        public long CodLocalInstalacao { get; set; }
    }
}
