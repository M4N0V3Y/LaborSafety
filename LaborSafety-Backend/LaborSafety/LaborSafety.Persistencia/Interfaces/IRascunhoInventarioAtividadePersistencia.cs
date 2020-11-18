using System.Collections.Generic;
using System.Data.Entity;
using LaborSafety.Dominio.Modelos;

namespace LaborSafety.Persistencia.Interfaces
{
    public interface IRascunhoInventarioAtividadePersistencia
    {
        void Inserir(RascunhoInventarioAtividadeModelo rascunhoInventarioAtividadeModelo, DB_APRPTEntities entities, List<LOCAL_INSTALACAO> locaisInsercao = null);
        void InserirRascunhoInventarioAtividade(RascunhoInventarioAtividadeModelo rascunhoInventarioAtividadeModelo, List<LOCAL_INSTALACAO> locais = null);
        List<RASCUNHO_INVENTARIO_ATIVIDADE> ListarRascunhoInventarioAtividadePorLI(List<long> li);
        RASCUNHO_INVENTARIO_ATIVIDADE ListarRascunhoInventarioAtividadePorId(long id);
        IEnumerable<RASCUNHO_INVENTARIO_ATIVIDADE> ListarRascunhoInventarioAtividade(FiltroInventarioAtividadeModelo filtroInventarioAtividadeModelo);
        List<LOCAL_INSTALACAO> BuscaFilhosPorNivel(long codigoLocal, DB_APRPTEntities entities = null);
        void EditarRascunhoInventarioAtividade(RascunhoInventarioAtividadeModelo rascunhoInventarioAtividadeModelo, DB_APRPTEntities entities, List<LOCAL_INSTALACAO> locaisInstalacao, DbContextTransaction transaction);
        void ExcluirRascunhoInventarioAtividade(long id, DB_APRPTEntities entities);
    }
}
