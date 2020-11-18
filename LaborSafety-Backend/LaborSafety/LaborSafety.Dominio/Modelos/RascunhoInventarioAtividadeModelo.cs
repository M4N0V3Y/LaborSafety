using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaborSafety.Dominio.Modelos
{
    public class RascunhoInventarioAtividadeModelo
    {
        public long CodRascunhoInventarioAtividade { get; set; }
        public string Codigo { get; set; }
        public long? CodPeso { get; set; }
        public long? CodPerfilCatalogo { get; set; }
        public long? CodDuracao { get; set; }
        public long? CodAtividade { get; set; }
        public long? CodDisciplina { get; set; }
        public string Descricao { get; set; }
        public int? RiscoGeral { get; set; }
        public string ObservacaoGeral { get; set; }
        public bool novoInventario { get; set; }
        public string EightIDUsuarioModificador { get; set; }
        public List<LocalInstalacaoRascunhoInventarioAtividadeModelo> LOCAL_INSTALACAO_RASCUNHO_INVENTARIO_ATIVIDADE { get; set; }
        public List<RiscoRascunhoInventarioAtividadeModelo> RISCO_RASCUNHO_INVENTARIO_ATIVIDADE { get; set; }
    }
}
