using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaborSafety.Dominio.Modelos
{
    public class LocalInstalacaoRascunhoInventarioAtividadeModelo
    {
        public long CodLocalInstalacaoRascunhoInventarioAtividade { get; set; }
        public long CodRascunhoInventarioAtividade { get; set; }
        public long CodLocalInstalacao { get; set; }
        public bool Ativo { get; set; }
        public LocalInstalacaoModelo LocalInstalacao { get; set; }
    }
}
