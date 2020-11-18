using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaborSafety.Dominio.Modelos
{
    public class LocalInstalacaoModelo
    {
        public long CodLocalInstalacao { get; set; }
        public long CodInventarioAmbiente { get; set; }
        public List<long> InventariosAtividade { get; set; }
        public long CodPeso { get; set; }
        public long CodPerfilCatalogo { get; set; }
        public string N1 { get; set; }
        public string N2 { get; set; }
        public string N3 { get; set; }
        public string N4 { get; set; }
        public string N5 { get; set; }
        public string N6 { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
    }
}
