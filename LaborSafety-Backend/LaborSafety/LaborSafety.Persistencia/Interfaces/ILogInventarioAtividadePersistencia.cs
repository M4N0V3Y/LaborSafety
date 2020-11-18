using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaborSafety.Dominio.Modelos;

namespace LaborSafety.Persistencia.Interfaces
{
    public interface ILogInventarioAtividadePersistencia
    {
        LOG_INVENTARIO_ATIVIDADE Editar(InventarioAtividadeModelo novoInventario,
     List<LOCAL_INSTALACAO> locaisInstalacao, DB_APRPTEntities entities = null);

        LOG_INVENTARIO_ATIVIDADE Inserir(InventarioAtividadeModelo novoInventario, long codInventarioInserido,
    DB_APRPTEntities entities = null);
        List<LOG_INVENTARIO_ATIVIDADE> BuscarTodos(DB_APRPTEntities entities = null);
        List<LOG_INVENTARIO_ATIVIDADE> BuscarPorInventario(long codInventario, DB_APRPTEntities entities = null);
        LOG_INVENTARIO_ATIVIDADE Excluir(InventarioAtividadeDelecaoComLogModelo inventarioAtividadeDelecaoComLogModelo, DB_APRPTEntities entities = null);
        List<LOG_INVENTARIO_ATIVIDADE> ListarLogInventario(long codInventarioAtividade, DB_APRPTEntities entities = null);
        List<LOG_RISCO_INVENTARIO_ATIVIDADE> ListarLogRiscosInventario(long codInventarioAtividade, DB_APRPTEntities entities = null);
        void AtualizarCodInventarioLog(long ?codInventarioAntigo, long codInventarioAtual,
           DB_APRPTEntities entities = null);
    }
}
