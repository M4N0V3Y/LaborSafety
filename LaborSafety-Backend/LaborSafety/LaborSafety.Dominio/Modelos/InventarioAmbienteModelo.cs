using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaborSafety.Dominio.Modelos
{
    public class InventarioAmbienteModelo
    {
        public long CodInventarioAmbiente { get; set; }
        public string Codigo { get; set; }
        public long CodAmbiente { get; set; }
        public string Descricao { get; set; }
        public string ObservacaoGeral { get; set; }
        public int RiscoGeral { get; set; }
        public DateTime DataAtualizacao { get; set; }
        public bool Ativo { get; set; }

        public string EightIDUsuarioModificador { get; set; }

        public List<NrInventarioAmbienteModelo> NR_INVENTARIO_AMBIENTE { get; set; }
        public List<RiscoInventarioAmbienteModelo> RISCO_INVENTARIO_AMBIENTE { get; set; }
        public List<LocalInstalacaoModelo> LOCAL_INSTALACAO_MODELO { get; set; }
    }
}
