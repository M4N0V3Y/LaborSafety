using System.Collections.Generic;
using System.Data.Entity;
using LaborSafety.Dominio.Modelos;

namespace LaborSafety.Persistencia.Interfaces
{
    public interface IRascunhoInventarioAmbientePersistencia
    {
        void InserirRascunhoInventarioAmbiente(RascunhoInventarioAmbienteModelo rascunhoInventarioAmbienteModelo);
        RASCUNHO_INVENTARIO_AMBIENTE Inserir(RascunhoInventarioAmbienteModelo rascunhoInventarioAmbienteModelo, DB_APRPTEntities entities);
        RASCUNHO_INVENTARIO_AMBIENTE ListarRascunhoInventarioAmbientePorLI(long li);
        RASCUNHO_INVENTARIO_AMBIENTE ListarRascunhoInventarioAmbientePorId(long id);
        IEnumerable<RASCUNHO_INVENTARIO_AMBIENTE> ListarRascunhoInventarioAmbiente(FiltroInventarioAmbienteModelo filtroInventarioAmbienteModelo);
        List<LOCAL_INSTALACAO> BuscaFilhosPorNivelDoInventario(long local, long idInventario, DB_APRPTEntities entities = null);     
        void EditarRascunhoInventarioAmbiente(RascunhoInventarioAmbienteModelo rascunhoInventarioAmbienteModelo, DB_APRPTEntities entities, DbContextTransaction transaction);
        void ExcluirRascunhoInventarioAmbiente(long id, DB_APRPTEntities entities);
        List<LOCAL_INSTALACAO> BuscaFilhosPorNivelExcetoInventario(long codLocal, DB_APRPTEntities entities = null);
    }
}
