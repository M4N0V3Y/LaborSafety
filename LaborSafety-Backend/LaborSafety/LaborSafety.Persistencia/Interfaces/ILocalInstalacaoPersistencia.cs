using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaborSafety.Dominio.Modelos;

namespace LaborSafety.Persistencia.Interfaces
{
    public interface ILocalInstalacaoPersistencia
    {
        LOCAL_INSTALACAO InserirLocalInstalacao(LOCAL_INSTALACAO modelo, DB_APRPTEntities entities = null);
         LOCAL_INSTALACAO ListarLocalInstalacaoPorId(long id, DB_APRPTEntities entities = null);

        LOCAL_INSTALACAO ListarLocalInstalacaoPorNome(string nome, DB_APRPTEntities entities = null);
        List<LocalInstalacaoModelo> ListarLIPorNivelModelo(LocalInstalacaoFiltroModelo filtro);
        List<LOCAL_INSTALACAO> ListarLIPorNivelEntidade(LocalInstalacaoFiltroModelo filtro);
        List<LOCAL_INSTALACAO> ListarLocaisInstalacaoPorCodInventarioAmbiente(long codInventarioAmbiente);

        LOCAL_INSTALACAO EditaPerfilCatalogoLocaDoLocal(long codLocalInstalacao, long codPerfilCatalogo, DB_APRPTEntities entities = null);
        LOCAL_INSTALACAO EditaLocalInstalacao(LOCAL_INSTALACAO local, DB_APRPTEntities entities = null);

        List<LOCAL_INSTALACAO_INVENTARIO_ATIVIDADE> ListarLocaisInstalacaoPorCodInventarioAtividade(long codInventarioAtividade);
        List<LOCAL_INSTALACAO> ListarTodosLIs(DB_APRPTEntities entities = null);
        void AtualizaCodigoInventarioDoLocal(long idLocal, long idInventarioAmbiente, DB_APRPTEntities entities = null,
            DbContextTransaction transaction = null);
        void AtualizaCodigoInventarioDoLocal(long idLocal, long idInventarioAmbiente, DB_APRPTEntities entities);
        void ValidaLocalPorNivel(LOCAL_INSTALACAO local, DB_APRPTEntities entities = null);
        //void ValidaEInsereLocalBasePorNivel(LOCAL_INSTALACAO local, DB_APRPT_V1_ESPEntities entities);
        LOCAL_INSTALACAO ListarLocalInstalacaoPorIdString(string id);

        LOCAL_INSTALACAO ValidaLocalPorNivelImportacao(LOCAL_INSTALACAO local, DB_APRPTEntities entities = null);
        INVENTARIO_AMBIENTE ListaInventarioAmbienteAtivoLocalInstalacao(long codLocalInstalacao, DB_APRPTEntities entities = null);

        List<INVENTARIO_ATIVIDADE> ListaInventariosAtividadeAtivosLocalInstalacao(long codLocalInstalacao, DB_APRPTEntities entities = null);
        LOCAL_INSTALACAO ValidaSeExistePaiLI(LOCAL_INSTALACAO LI, DB_APRPTEntities entities = null);

        void ValidaEInsereLocalBasePorPai(LOCAL_INSTALACAO localPai, string nomeLocalAInserir, DB_APRPTEntities entities = null);

    }
}
