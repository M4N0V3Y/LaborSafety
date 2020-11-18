using ClosedXML.Excel;
using System.Collections.Generic;
using LaborSafety.Dominio.Modelos;
using LaborSafety.Persistencia;
using static LaborSafety.Negocio.Servicos.AprNegocio;

namespace LaborSafety.Negocio.Interfaces
{
    public interface IAprNegocio
    {
        int CalcularRiscoAprPorAtividadeDisciplinaLI(FiltroUnicoInventarioAtividadeModelo filtro);
        void Inserir(AprModelo aprModelo);
        void InserirAprovadores(List<AprovadorAprModelo> aprovadores);
        void InserirExecutores(List<ExecutanteAprModelo> executores);
        void InserirPessoa(PessoaModelo pessoas);
        void InserirListaPessoa(List<PessoaModelo> pessoas);
        void InserirCabecalho(AprModelo cabecalho);
        void InserirRisco(List<RiscoAprModelo> riscos);
        void InserirAtividadeOperacao(List<OperacaoAprModelo> atividades);
        void EditarApr(AprModelo aprModelo);
        void EditarRiscos(List<RiscoAprModelo> riscosAPR);
        void EditarAtividades(List<OperacaoAprModelo> atividades);
        void EditarExecutores(List<ExecutanteAprModelo> executantes);
        void EditarAprovadores(List<AprovadorAprModelo> aprovadores);
        AprModelo PesquisarPorNumeroSerie(string numeroSerie);
        AprModelo PesquisarPorOrdemManutencao(string ordemManutencao);
        bool ValidarExistenciaOrdemManutencao(string ordemManutencao, long codApr);
        AprModelo PesquisarPorId(long idApr);
        List<AprModelo> PesquisarPorRisco(long codRisco);
        List<AprModelo> PesquisarPorLocalInstalacao(long codLocalInstalacao);
        List<AprModelo> PesquisarPorStatusOk();
        List<AprModelo> ListarApr(FiltroAprModelo filtroAprModelo);
        List<RetornoStatusOrdemManutencaoModelo> listarOrdensValidas(List<string> listaOrdemManutencao);
        int CalcularRiscoAprTela(RiscoTotalAprModelo riscoTotalAprModelo);
        ResultadoGeracao GerarApr(DadosAprModelo dadosAprModelo, APR apr = null);
        ResultadoGeracao PreencherPaginasPlanilhaAPR(ref XLWorkbook workbook, ref XLWorkbook folhaComplementar,
                    ref int qtdItensPorFolha, ref int qtdFolhaComplementar, ref string folhaAtual,
                    List<RISCO_INVENTARIO_AMBIENTE> riscosInvAmbiente, List<RISCO_INVENTARIO_ATIVIDADE> riscosInvAtividade,
                    List<NR_INVENTARIO_AMBIENTE> listaNRs, DadosAprModelo dadosAprModelo, string caminhoDiretorioApr);
        ResultadoGeracao GerarAPREmBrancoComNumeroSerie();
        RetornoBloqueioAgrupadoModelo GerarMapaBloqueioAgrupado(List<string> listaOrdemManutencao);
        ArquivoLog EscreverLogTodasAPRs();
        ArquivoLog EscreverLogEmTxt(List<long> codApr);
    }
}
