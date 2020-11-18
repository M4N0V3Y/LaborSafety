using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaborSafety.Dominio.Modelos
{
    public class FiltroInventarioAtividadeModelo
    {   
        public List<long> Riscos { get; set; }
        public long? CodPeso { get; set; }
        public long? CodPerfilCatalogo { get; set; }
        public long? CodAtividade { get; set; }
        public long? CodDisciplina { get; set; }
        public long? CodSeveridade { get; set; }
        public List<long> Locais { get; set; }
    }
}
