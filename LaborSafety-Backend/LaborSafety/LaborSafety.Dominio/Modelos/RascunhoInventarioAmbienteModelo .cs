using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaborSafety.Dominio.Modelos
{
    public class RascunhoInventarioAmbienteModelo
    {
        public long? CodRascunhoInventarioAmbiente { get; set; }
        public string Codigo { get; set; }
        public long? CodAmbiente { get; set; }
        public string CodLocalInstalacao { get; set; }
        public string NomeLocalInstalacao { get; set; }
        public string Descricao { get; set; }
        public string ObservacaoGeral { get; set; }
        public int? RiscoGeral { get; set; }
        public bool novoInventario { get; set; }
        public string EightIDUsuarioModificador { get; set; }
        public List<NrRascunhoInventarioAmbienteModelo> NR_RASCUNHO_INVENTARIO_AMBIENTE { get; set; }
        public List<RiscoRascunhoInventarioAmbienteModelo> RISCO_RASCUNHO_INVENTARIO_AMBIENTE { get; set; }
        public List<LocalInstalacaoModelo> LOCAL_INSTALACAO_MODELO { get; set; }
    }
}
