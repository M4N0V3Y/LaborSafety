using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaborSafety.Dominio.Modelos
{
    public class FiltroUnicoInventarioAtividadeModelo
    {
        public long CodAtividade { get; set; }
        public long CodDisciplina { get; set; }
        public long CodLi { get; set; }
        public bool AprAtiva { get; set; }
        public long? CodApr { get; set; }
    }
}
