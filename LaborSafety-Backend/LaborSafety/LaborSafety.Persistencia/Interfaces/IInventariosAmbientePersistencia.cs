using System.Collections.Generic;
using System.Data.Entity;
using LaborSafety.Dominio.Modelos;
using LaborSafety.Dominio.Modelos.Exportacao;

namespace LaborSafety.Persistencia.Interfaces
{
    public interface IInventariosAmbientePersistencia
    {
        INVENTARIO_AMBIENTE ListarInventarioAmbientePorId(long id, DB_APRPTEntities entities = null, bool validarAtivo = true);
        List<INVENTARIO_AMBIENTE> ListarTodosInventarios();
        INVENTARIO_AMBIENTE ListarInventarioAmbientePorIdAmbiente(long idAmbiente);

        INVENTARIO_AMBIENTE ListarInventarioAmbientePorCodigo(string codigo, DB_APRPTEntities entities = null);
        IEnumerable<INVENTARIO_AMBIENTE> ListarInventarioAmbiente(FiltroInventarioAmbienteModelo filtroInventarioAmbienteModelo);
        INVENTARIO_AMBIENTE ListarInventarioAmbientePorLI(long li, DB_APRPTEntities entities = null);
        List<INVENTARIO_AMBIENTE> ListarInventarioAmbienteExportacao(DadosExportacaoAmbienteModelo dados);
        //List<LOCAL_INSTALACAO> BuscaLocaisDosInventariosPorNivel(string nome, DB_APRPTEntities entities = null);
        List<LOCAL_INSTALACAO> BuscaFilhosPorNivelExcetoInventario(long codLocal, long idInventario, DB_APRPTEntities entities = null);
        //List<LOCAL_INSTALACAO> BuscaFilhosPorNivelDoInventario(long codLocalInstalacao, long idInventario, DB_APRPTEntities entities = null);
        INVENTARIO_AMBIENTE ListarInventarioAmbienteAtivadoEDesativadoPorLI(long li, DB_APRPTEntities entities = null);
        long ListarCodAprPorInventarioTela(long codInventario, DB_APRPTEntities entities = null);

        //List<INVENTARIO_AMBIENTE> ListarInventarioCompletoPorApr(long codAPR);
        INVENTARIO_AMBIENTE Inserir(InventarioAmbienteModelo inventarioAmbienteModelo, DB_APRPTEntities entities);
        INVENTARIO_AMBIENTE InserirPorEdicao(InventarioAmbienteModelo inventarioAmbienteModelo, DB_APRPTEntities entities);
        INVENTARIO_AMBIENTE EditarInventarioAmbiente(InventarioAmbienteModelo inventarioAmbienteModelo, DB_APRPTEntities entities);
        void EditarNrInventarioAmbiente(long idInventario, long idNr);
        void EditarRiscoInventarioAmbiente(long idInventario, long idRisco);
        void DesativarInventario(long codInventarioExistente, DB_APRPTEntities entities);

        void DesativarInventarioPorCodigo(string codigo, DB_APRPTEntities entities);

        void ExcluirInventarioAmbiente(long id);
        INVENTARIO_AMBIENTE ListarInventarioAmbienteAtivadoEDesativadoPorId(long id, DB_APRPTEntities entities = null);

        long ListarCodAprPorInventario(long codInventario, DB_APRPTEntities entities);
    }
}
