using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaborSafety.Dominio.Modelos;

namespace LaborSafety.Persistencia.Interfaces
{
    public interface ILogInventarioAmbientePersistencia
    {
        LOG_INVENTARIO_AMBIENTE Editar(List<LOCAL_INSTALACAO> novosLIs, InventarioAmbienteModelo novoInventario,
            DB_APRPTEntities entities = null);

        LOG_INVENTARIO_AMBIENTE Inserir(InventarioAmbienteModelo novoInventario, long codInventarioInserido,
    DB_APRPTEntities entities = null);
        List<LOG_INVENTARIO_AMBIENTE> BuscarTodos(DB_APRPTEntities entities = null);
        List<LOG_INVENTARIO_AMBIENTE> BuscarPorInventario(long codInventario, DB_APRPTEntities entities = null);
        LOG_INVENTARIO_AMBIENTE Excluir(InventarioAmbienteDelecaoComLogModelo inventarioAmbienteDelecaoComLogModelo, DB_APRPTEntities entities = null);
        List<LOG_INVENTARIO_AMBIENTE> ListarLogInventario(long codInventarioAmbiente, DB_APRPTEntities entities = null);
        List<LOG_RISCO_INVENTARIO_AMBIENTE> ListarLogRiscosInventario(long codInventarioAmbiente, DB_APRPTEntities entities = null);
        void AtualizarCodInventarioLog(long ?codInventarioAntigo, long codInventarioAtual,
           DB_APRPTEntities entities = null);
    }
}
