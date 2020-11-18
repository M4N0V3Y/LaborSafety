using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaborSafety.Dominio.Modelos
{
    public class RiscoAprModelo
    {
        public long CodRiscoApr { get; set; }
        public long CodRisco { get; set; }
        public long CodApr { get; set; }
        public bool Ativo { get; set; }
    }
}
