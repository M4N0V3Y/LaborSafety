using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaborSafety.Dominio.Modelos
{
    public class LocalInstalacaoInventarioAtividadeModelo
    {
        public long CodLocalInstalacaoInventarioAtividade { get; set; }
        public long CodInventarioAtividade { get; set; }
        public long CodLocalInstalacao { get; set; }
        public bool Ativo { get; set; }
        public LocalInstalacaoModelo LocalInstalacao { get; set; }
    }
}
