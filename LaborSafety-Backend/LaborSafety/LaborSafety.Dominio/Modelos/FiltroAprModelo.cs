using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaborSafety.Dominio.Modelos
{
    public class FiltroAprModelo
    {
        public List<long> CodLocalInstalacao { get; set; }
        public string NumeroSerie { get; set; }
        public string OrdemManutencao { get; set; }
    }
}
