using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaborSafety.Dominio.Modelos
{
    public class AprovadorAprModelo
    {
        public long CodAprovadorApr { get; set; }
        public long CodPessoa { get; set; }
        public long CodApr { get; set; }
        public bool Ativo { get; set; }
    }
}
