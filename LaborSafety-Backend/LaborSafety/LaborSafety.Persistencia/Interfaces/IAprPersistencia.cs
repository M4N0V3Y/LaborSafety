using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaborSafety.Dominio.Modelos;
using LaborSafety.Dominio.Modelos.Exportacao;

namespace LaborSafety.Persistencia.Interfaces
{
    public interface IAprPersistencia
    {
        APR InserirApr(AprModelo aprModelo, List<LOCAL_INSTALACAO> locaisInstalacao, DB_APRPTEntities entities);
        APR EditarApr(AprModelo aprModelo, DB_APRPTEntities entities, List<LOCAL_INSTALACAO> locaisInstalacao);
        APR Inserir(AprModelo aprModelo, DB_APRPTEntities entities, List<LOCAL_INSTALACAO> locaisInstalacao);
        APR Editar(AprModelo aprModelo, DB_APRPTEntities entities, List<LOCAL_INSTALACAO> locaisInstalacao);
        void InserirCabecalho(AprModelo aprModelo);
        void InserirPessoa(PessoaModelo pessoas);
        void InserirListaPessoa(List<PessoaModelo> pessoas);
        void InserirRisco(List<RiscoAprModelo> riscoApr);
        void InserirRiscosAtividadeAPR(long idApr, long idLocal, long idDisciplina, long idAtividade, DB_APRPTEntities entities);

        void InserirAtividadeOperacao(List<OperacaoAprModelo> atividadeOperacao);
        void InserirPessoaGeral(DB_APRPTEntities entities, PessoaModelo pessoas);
        void InserirAprovadores(List<AprovadorAprModelo> aprovadores);
        void InserirExecutores(List<ExecutanteAprModelo> executores);
        void InsereEditaPessoa(DB_APRPTEntities entities, List<PessoaModelo> pessoas, bool eAprovador, long id, long idPessoa);
        APR InserirSomenteComNumeroSerie(bool origemTela = true, DB_APRPTEntities entities = null);
        APR InserirSomenteComNumeroSeriaViaSAP(DB_APRPTEntities entities = null);
        void EditarRiscos(List<RiscoAprModelo> riscosAPR);
        void EditarAtividades(List<OperacaoAprModelo> atividadeOperacao);
        void EditarExecutores(List<ExecutanteAprModelo> executantes);
        void EditarAprovadores(List<AprovadorAprModelo> aprovadores);
        void EditarStatusApr(string codOrdemManutencao, long novoCodStatusApr, DB_APRPTEntities entities = null);
        APR PesquisarPorNumeroSerie(string numeroSerie);
        APR PesquisarPorOrdemManutencao(string ordemManutencao, DB_APRPTEntities entities = null);
        bool ValidarExistenciaOrdemManutencao(string ordemManutencao, long codApr);
        APR PesquisarPorOrdemManutencaoExistentesEInexistentes(string ordemManutencao, DB_APRPTEntities entities = null);
        APR PesquisarPorId(long idApr);
        LOCAL_INSTALACAO_INVENTARIO_AMBIENTE_HISTORICO_APR PesquisarAprInventarioAmbiente(long codLI, long codApr);
        List<LOCAL_INSTALACAO_INVENTARIO_ATIVIDADE_HISTORICO_APR> PesquisarAprInventarioAtividade(long codLI, long codApr);
        List<APR> PesquisarPorRisco(long codRisco);
        List<APR> PesquisarPorLocalInstalacao(long codLocalInstalacao);
        List<APR> PesquisarPorStatusOk();
        List<APR> ListarApr(FiltroAprModelo filtroAprModelo);
        List<APR> ListarTodasAPRs();
        List<APR> ListarAprExportacao(DadosExportacaoAprModelo dados);
        void DesativarApr(long codAprExistente);
        List<LOCAL_INSTALACAO> BuscaFilhosPorNivel(long codLocal, DB_APRPTEntities entities = null);
        long BuscarPessoaPorAprovador(long codAprovador, long codApr);
        long BuscarPessoaPorExecutante(long codExecutante, long codApr);
        APR PesquisarPorIdAnteriorAEdicao(long idApr);

        void InserirRiscosAmbienteAPR(long idApr, long idLocal, DB_APRPTEntities entities);
        List<RISCO> BuscarTodosOsRiscos(long codAPR, bool riscosAtivos, DB_APRPTEntities entities = null);
    }
}
