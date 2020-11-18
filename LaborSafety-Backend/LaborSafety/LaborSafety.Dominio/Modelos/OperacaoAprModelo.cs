using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaborSafety.Dominio.Modelos
{
    public class OperacaoAprModelo
    {
        public long CodOperacaoAPR { get; set; }
        public long CodAPR { get; set; }
        public long CodStatusAPR { get; set; }
        public string Codigo { get; set; }
        public string Descricao { get; set; }
        public long? CodLI { get; set; }
        public string NomeLI { get; set; }
        public long? CodDisciplina { get; set; }
        public string NomeDisciplina { get; set; }
        public long? CodAtvPadrao { get; set; }
        public string NomeAtvPadrao { get; set; }
        public bool Ativo { get; set; }
    }
}
