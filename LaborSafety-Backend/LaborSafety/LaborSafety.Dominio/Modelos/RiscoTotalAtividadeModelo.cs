using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaborSafety.Dominio.Modelos
{
    public class RiscoTotalAtividadeModelo
    {
        public int CodSeveridade { get; set; }
        public int CodRisco { get; set; }
        public int CodAtividade { get; set; }
        public long CodDisciplina { get; set; }
        public int CodDuracao { get; set; }
        public int CodPeso { get; set; }
    }
}
