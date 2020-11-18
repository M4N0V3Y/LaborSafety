using System.Collections.Generic;
using System.Data.Entity;
using LaborSafety.Dominio.Modelos;
using LaborSafety.Dominio.Modelos.Exportacao;

namespace LaborSafety.Persistencia.Interfaces
{
    public interface IInventariosAtividadePersistencia
    {
        // selects
        INVENTARIO_ATIVIDADE ListarInventarioAtividadePorId(long id, bool eAtivo = true);
        List<INVENTARIO_ATIVIDADE> ListarTodosInventarios();
        List<INVENTARIO_ATIVIDADE> ListarVariosInventariosAtividadePorLI(long li);
        INVENTARIO_ATIVIDADE ListarInventarioAtividadePorAtividadeEDisciplina(long codAtividade, long codDisciplina);
        IEnumerable<INVENTARIO_ATIVIDADE> ListarInventarioAtividade(FiltroInventarioAtividadeModelo filtroInventarioAtividadeModelo);
        List<INVENTARIO_ATIVIDADE> ListarInventarioAtividadeExportacao(DadosExportacaoAtividadeModelo dados);
        List<INVENTARIO_ATIVIDADE> ListarTodos(DB_APRPTEntities entities = null);
        INVENTARIO_ATIVIDADE ListarInventarioAtividadePorAtividadeDisciplinaLI(long atividadePadrao, long disciplina, long li, DB_APRPTEntities entities);
        INVENTARIO_ATIVIDADE ListarInventarioAtividadePorAtividadeDisciplinaLIInv(long atividadePadrao, long disciplina, long li, long invAtividade, DB_APRPTEntities entities);
        List<LOCAL_INSTALACAO> BuscaFilhosPorNivel(long codLocal, DB_APRPTEntities entities = null);
        INVENTARIO_ATIVIDADE ListarInventarioAtividadeAtivadoEDesativadoPorId(long id, DB_APRPTEntities entities = null);
        INVENTARIO_ATIVIDADE ListarInventarioDeAtividadePorCodigo(string codigo, DB_APRPTEntities entities);
        long ListarCodAprPorInventario(long codInventario, long codAtividade, long codDiscipllina, long codLocal, DB_APRPTEntities entities);
        long ListarCodAprPorInventarioTela(long codInventario, long codAtividade, long codDiscipllina, long codLocal, DB_APRPTEntities entities);
        INVENTARIO_ATIVIDADE ListarInventarioAtividadeAtivadoEDesativadoPorAtividadeDisciplinaLI(long atividadePadrao, long disciplina, long li, DB_APRPTEntities entities);
        // inserts
        void InserirInventarioAtividade(InventarioAtividadeModelo inventarioAtividadeModelo, List<LOCAL_INSTALACAO> locais = null);
        INVENTARIO_ATIVIDADE InserirItemListaInventarioAtividade(InventarioAtividadeModelo inventario, DB_APRPTEntities entities);
        INVENTARIO_ATIVIDADE InserirImportacao(InventarioAtividadeModelo inventarioAtividadeModelo, DB_APRPTEntities entities, bool eEdicao = false);
        INVENTARIO_ATIVIDADE Inserir(InventarioAtividadeModelo inventarioAtividadeModelo, DB_APRPTEntities entities, List<LOCAL_INSTALACAO> locaisInsercao = null);
        // updates
        INVENTARIO_ATIVIDADE EditarInventarioAtividade(InventarioAtividadeModelo inventarioAtividadeModelo, DB_APRPTEntities entities, 
            List<LOCAL_INSTALACAO> locaisInstalacao);
        void EditarLocalInstalacaoInventarioAtividade(long idInventario, long idLi);
        void EditarRiscoInventarioAtividade(long idInventario, long idRisco);
        void EditarResponsavelInventarioAtividade(long idInventario, long idResponsavel);
        void DesativarInventario(long codInventarioExistente, DB_APRPTEntities entities);

        INVENTARIO_ATIVIDADE EditarInventarioAtividadePorImportacao(InventarioAtividadeModelo inventarioAtividadeModelo, DB_APRPTEntities entities);

        // deletes
        void ExcluirInventarioAtividade(long id);
    }
}
