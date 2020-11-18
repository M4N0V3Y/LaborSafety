using AutoMapper;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Office.CustomUI;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using LaborSafety.Dominio.Modelos;
using LaborSafety.Negocio.Interfaces;
using LaborSafety.Persistencia;
using LaborSafety.Persistencia.Interfaces;
using LaborSafety.Persistencia.Servicos;
using LaborSafety.Utils;
using LaborSafety.Utils.Constantes;
using static LaborSafety.Utils.Constantes.Constantes;

namespace LaborSafety.Negocio.Servicos
{
    public class AprNegocio : IAprNegocio
    {
        private readonly IAprPersistencia aprPersistencia;
        private readonly IInventariosAmbientePersistencia inventariosAmbientePersistencia;
        private readonly IInventariosAtividadePersistencia inventariosAtividadePersistencia;
        private readonly ILocalInstalacaoPersistencia localInstalacaoPersistencia;
        private readonly INrPersistencia nrPersistencia;
        private readonly IEPIPersistencia ePIPersistencia;
        private readonly IProbabilidadePersistencia probabilidadePersistencia;
        private readonly ISeveridadePersistencia severidadePersistencia;
        private readonly IAtividadePadraoPersistencia atividadePadraoPersistencia;
        private readonly IPesoPersistencia pesoPersistencia;
        private readonly IDuracaoPersistencia duracaoPersistencia;
        private readonly IDisciplinaPersistencia disciplinaPersistencia;
        private readonly IRiscoPersistencia riscoPersistencia;
        private readonly IBloqueioPersistencia bloqueioPersistencia;
        private readonly IEPIRiscoInventarioAmbientePersistencia epiRiscoInventarioAmbientePersistencia;
        private readonly IEPIRiscoInventarioAtividadePersistencia epiRiscoInventarioAtividadePersistencia;
        private readonly ILogAprPersistencia logAprPersistencia;
        private readonly IPessoaPersistencia pessoaPersistencia;

        public AprNegocio(IAprPersistencia aprPersistencia, IInventariosAmbientePersistencia inventariosAmbientePersistencia, IInventariosAtividadePersistencia inventariosAtividadePersistencia,
             ILocalInstalacaoPersistencia localInstalacaoPersistencia, INrPersistencia nrPersistencia, IEPIPersistencia ePIPersistencia,
             IProbabilidadePersistencia probabilidadePersistencia, ISeveridadePersistencia severidadePersistencia, IAtividadePadraoPersistencia atividadePadraoPersistencia,
             IPesoPersistencia pesoPersistencia, IDuracaoPersistencia duracaoPersistencia, IDisciplinaPersistencia disciplinaPersistencia, IRiscoPersistencia riscoPersistencia,
             IBloqueioPersistencia bloqueioPersistencia, IEPIRiscoInventarioAmbientePersistencia epiRiscoInventarioAmbientePersistencia,
             IEPIRiscoInventarioAtividadePersistencia epiRiscoInventarioAtividadePersistencia, ILogAprPersistencia logAprPersistencia, IPessoaPersistencia pessoaPersistencia)
        {
            this.aprPersistencia = aprPersistencia;
            this.inventariosAmbientePersistencia = inventariosAmbientePersistencia;
            this.inventariosAtividadePersistencia = inventariosAtividadePersistencia;
            this.localInstalacaoPersistencia = localInstalacaoPersistencia;
            this.nrPersistencia = nrPersistencia;
            this.ePIPersistencia = ePIPersistencia;
            this.probabilidadePersistencia = probabilidadePersistencia;
            this.severidadePersistencia = severidadePersistencia;
            this.atividadePadraoPersistencia = atividadePadraoPersistencia;
            this.pesoPersistencia = pesoPersistencia;
            this.duracaoPersistencia = duracaoPersistencia;
            this.disciplinaPersistencia = disciplinaPersistencia;
            this.riscoPersistencia = riscoPersistencia;
            this.bloqueioPersistencia = bloqueioPersistencia;
            this.logAprPersistencia = logAprPersistencia;
            this.pessoaPersistencia = pessoaPersistencia;
            //TODO: ANDRE
            this.epiRiscoInventarioAmbientePersistencia = epiRiscoInventarioAmbientePersistencia;
            this.epiRiscoInventarioAtividadePersistencia = epiRiscoInventarioAtividadePersistencia;
        }

        public int CalcularRiscoApr(int riscoAmbiente, int riscoAtividade)
        {
            int resultado = (riscoAmbiente * riscoAtividade);

                if (resultado > 16)
                    throw new Exception("Não existem valores válidos para realizar o cálculo do risco da APR.");

            return resultado;
        }

        public int CalcularRiscoAprPorAtividadeDisciplinaLI(FiltroUnicoInventarioAtividadeModelo filtro)
        {
            INVENTARIO_AMBIENTE inventarioAmbiente;
            LOCAL_INSTALACAO_INVENTARIO_AMBIENTE_HISTORICO_APR inventarioAmbienteHistorico;
            List<LOCAL_INSTALACAO_INVENTARIO_ATIVIDADE_HISTORICO_APR> inventarioAtividadeHistorico;
            int riscoAmbiente;
            int riscoAtividade=0;

            var localInstalacao = localInstalacaoPersistencia.ListarLocalInstalacaoPorId(filtro.CodLi);
            if (localInstalacao == null)
                throw new Exception($"Não foi encontrado local de instalação de nome {localInstalacao.Nome}.");

            if (!filtro.AprAtiva)
            {
                inventarioAmbienteHistorico = aprPersistencia.PesquisarAprInventarioAmbiente(filtro.CodLi, filtro.CodApr.Value);
                inventarioAtividadeHistorico = aprPersistencia.PesquisarAprInventarioAtividade(filtro.CodLi, filtro.CodApr.Value);
            }
            else
            {
                inventarioAmbienteHistorico = null;
                inventarioAtividadeHistorico = null;
            }

            if(inventarioAmbienteHistorico != null)
            {
                INVENTARIO_AMBIENTE inventarioDesativado = inventariosAmbientePersistencia.ListarInventarioAmbientePorId(inventarioAmbienteHistorico.CodInventarioAmbiente, null, false);
                riscoAmbiente = inventarioDesativado.RiscoGeral;
            }
            else
            {
                inventarioAmbiente = inventariosAmbientePersistencia.ListarInventarioAmbientePorLI(filtro.CodLi);
                if (inventarioAmbiente == null)
                    throw new Exception($"Inventário de ambiente não encontrado para o local de instalação {localInstalacao.Nome}.");

                riscoAmbiente = inventarioAmbiente.RiscoGeral;
            }

            if (inventarioAtividadeHistorico != null && inventarioAtividadeHistorico.Count > 0)
            {
                foreach (var itemInvAtvHistorico in inventarioAtividadeHistorico)
                {
                    var inventarioAtividadeDesativado = inventariosAtividadePersistencia.ListarInventarioAtividadePorId(itemInvAtvHistorico.CodInventarioAtividade, false);

                    var filtroPorAtvEDisciplina = inventariosAtividadePersistencia.ListarInventarioAtividadePorAtividadeEDisciplina(inventarioAtividadeDesativado.CodAtividade, inventarioAtividadeDesativado.CodDisciplina);

                    if (filtroPorAtvEDisciplina != null)
                    {
                        riscoAtividade = filtroPorAtvEDisciplina.RiscoGeral;
                        break;
                    }
                }
            }

            else
            { 
                var inventariosAtividade = inventariosAtividadePersistencia.ListarInventarioAtividadeAtivadoEDesativadoPorAtividadeDisciplinaLI(filtro.CodAtividade, filtro.CodDisciplina, filtro.CodLi, null);
                if (inventariosAtividade == null)
                    throw new Exception($"Inventário de atividade não encontrado para o local de instalação {localInstalacao.Nome}.");

                riscoAtividade = inventariosAtividade.RiscoGeral;
            }

            int resultado = (riscoAmbiente * riscoAtividade);

            return resultado;
        }

        public int CalcularRiscoAprTela(RiscoTotalAprModelo riscoTotalAprModelo)
        {
            int resultado = (riscoTotalAprModelo.RiscoAmbiente * riscoTotalAprModelo.RiscoAtividade);

            return resultado;
        }

        public List<RetornoStatusOrdemManutencaoModelo> listarOrdensValidas(List <string> listaOrdemManutencao)
        {
            List<RetornoStatusOrdemManutencaoModelo> statusOrdens = new List<RetornoStatusOrdemManutencaoModelo>();
            foreach (var ordemManutencao in listaOrdemManutencao)
            {
                var ordemAtual = new RetornoStatusOrdemManutencaoModelo();
                ordemAtual.CodOrdemManutencao = ordemManutencao;
                try
                {
                    var aprAtual = PesquisarPorOrdemManutencao(ordemManutencao);
                    if (aprAtual.CodStatusAPR != (long)TipoCodStatusApr.LIB)
                    {
                        ordemAtual.Status = true;
                    }
                    else
                    {
                        ordemAtual.Status = false;
                    }
                }

                catch 
                {
                    ordemAtual.Status = false;
                }
                statusOrdens.Add(ordemAtual);
            }
            return statusOrdens;
        }

        public AprModelo MapeamentoApr(APR apr)
        {
            AprModelo aprModelo = new AprModelo()
            {
                CodAPR = apr.CodAPR,
                NumeroSerie = apr.NumeroSerie,
                OrdemManutencao = apr.OrdemManutencao,
                Descricao = apr.Descricao,
                RiscoGeral = apr.RiscoGeral.Value,
                DataAprovacao = apr.DataAprovacao,
                DataInicio = apr.DataInicio.Value,
                DataEncerramento = apr.DataEncerramento,
                CodStatusAPR = apr.CodStatusAPR,
                Ativo = apr.Ativo
            };

            aprModelo.RISCO_APR = new List<RiscoAprModelo>();
            //aprModelo.FOLHA_ANEXO_APR = new List<FolhaAnexoAprModelo>();
            aprModelo.OPERACAO_APR = new List<OperacaoAprModelo>();
            aprModelo.APROVADOR_APR = new List<AprovadorAprModelo>();
            aprModelo.EXECUTANTE_APR = new List<ExecutanteAprModelo>();

            Mapper.Map(apr.RISCO_APR, aprModelo.RISCO_APR);
            //Mapper.Map(apr.FOLHA_ANEXO_APR, aprModelo.FOLHA_ANEXO_APR);
            Mapper.Map(apr.OPERACAO_APR, aprModelo.OPERACAO_APR);
            Mapper.Map(apr.APROVADOR_APR, aprModelo.APROVADOR_APR);
            Mapper.Map(apr.EXECUTANTE_APR, aprModelo.EXECUTANTE_APR);

            foreach (var item in aprModelo.OPERACAO_APR)
            {
                var local = localInstalacaoPersistencia.ListarLocalInstalacaoPorId((long)item.CodLI);

                if (local == null)
                    throw new Exception($"Local de instalação {local.Nome} não encontrado.");

                item.NomeLI = local.Nome;
            }

            return aprModelo;
        }

        #region Inserts
        public void Inserir(AprModelo aprModelo)
        {
            //if (string.IsNullOrEmpty(aprModelo.OrdemManutencao))
            //    throw new Exception("A ordem de manutenção deve ser informada!");

            if (string.IsNullOrEmpty(aprModelo.Descricao))
                throw new Exception("A descrição deve ser informada!");

            if (aprModelo.RiscoGeral <= 0)
                throw new Exception("O risco geral deve ser maior que zero!");

            if (aprModelo.DataInicio == null)
                throw new Exception("A data de início deve ser informada!");

            List<LOCAL_INSTALACAO> locaisInstalacao = new List<LOCAL_INSTALACAO>();
            using (var entities = new DB_APRPTEntities())
            {
                for (int i = 0; i < aprModelo.OPERACAO_APR.Count; i++)
                {
                    var codigoLi = aprModelo.OPERACAO_APR[i].CodLI;

                    var localBD = localInstalacaoPersistencia.ListarLocalInstalacaoPorId((long)codigoLi);

                    List<LOCAL_INSTALACAO> locaisFilhos =
                        inventariosAtividadePersistencia.BuscaFilhosPorNivel(localBD.CodLocalInstalacao, entities);

                    locaisInstalacao.AddRange(locaisFilhos);
                }

                if (locaisInstalacao.Count <= 0)
                    throw new Exception("Não foram encontrados locais de instalação.");

                var aprInserida = aprPersistencia.InserirApr(aprModelo, locaisInstalacao, entities);
                logAprPersistencia.Inserir(aprModelo, aprInserida.CodAPR, entities);


            }
        }

        public void InserirAprovadores(List<AprovadorAprModelo> aprovadores)
        {
            if (aprovadores == null)
                throw new Exception("Aprovadores não informados.");

            this.aprPersistencia.InserirAprovadores(aprovadores);
        }

        public void InserirCabecalho(AprModelo cabecalho)
        {
            if (cabecalho == null)
                throw new Exception("Dados de cabeçalho da APR não informado.");

            this.aprPersistencia.InserirCabecalho(cabecalho);
        }

        public void InserirExecutores(List<ExecutanteAprModelo> executores)
        {
            if (executores == null)
                throw new Exception("Executores não informados.");

            this.aprPersistencia.InserirExecutores(executores);
        }

        public void InserirPessoa(PessoaModelo pessoas)
        {
            if (pessoas == null)
                throw new Exception("Pessoas não informadas.");

            this.aprPersistencia.InserirPessoa(pessoas);
        }

        public void InserirListaPessoa(List<PessoaModelo> pessoas)
        {
            if (pessoas == null)
                throw new Exception("Pessoas não informadas.");

            this.aprPersistencia.InserirListaPessoa(pessoas);
        }

        public void InserirRisco(List<RiscoAprModelo> riscos)
        {
            if (riscos == null)
                throw new Exception("Riscos de APR não informados.");

            this.aprPersistencia.InserirRisco(riscos);
        }

        public void InserirAtividadeOperacao(List<OperacaoAprModelo> atividades)
        {
            if (atividades == null)
                throw new Exception("Atividades de operação de APR não informadas.");

            this.aprPersistencia.InserirAtividadeOperacao(atividades);
        }
        #endregion

        #region Updates
        public void EditarRiscos(List<RiscoAprModelo> riscosAPR)
        {
            if (riscosAPR == null)
                throw new Exception("Riscos não informados.");

            this.aprPersistencia.EditarRiscos(riscosAPR);
        }

        public void EditarAtividades(List<OperacaoAprModelo> atividades)
        {
            if (atividades == null)
                throw new Exception("Atividades não informadas.");

            this.aprPersistencia.EditarAtividades(atividades);
        }

        public void EditarExecutores(List<ExecutanteAprModelo> executantes)
        {
            if (executantes == null)
                throw new Exception("Executores não informados.");

            this.aprPersistencia.EditarExecutores(executantes);
        }

        public void EditarAprovadores(List<AprovadorAprModelo> aprovadores)
        {
            this.aprPersistencia.EditarAprovadores(aprovadores);
        }

        public void EditarApr(AprModelo aprModelo)
        {
            if (aprModelo == null)
                throw new Exception("É necessário informar uma APR para que se possa editá-la.");

            else if (!aprModelo.NumeroSerie.Contains("M"))
                throw new Exception("Só é possível editar APRs que tenham sido inseridas manualmente no sistema.");

            List<LOCAL_INSTALACAO> locaisInstalacao = new List<LOCAL_INSTALACAO>();
            using (var entities = new DB_APRPTEntities())
            {
                
                //List<LOCAL_INSTALACAO> locais = localInstalacaoPersistencia.ListarTodosLIs(entities);

                for (int i = 0; i < aprModelo.OPERACAO_APR.Count; i++)
                {
                    var codigoLi = aprModelo.OPERACAO_APR[i].CodLI;

                    var localBD = localInstalacaoPersistencia.ListarLocalInstalacaoPorId((long)codigoLi);

                    List<LOCAL_INSTALACAO> locaisFilhos =
                        inventariosAtividadePersistencia.BuscaFilhosPorNivel(localBD.CodLocalInstalacao, entities);

                    locaisInstalacao.AddRange(locaisFilhos);
                }

                if (locaisInstalacao.Count <= 0)
                    throw new Exception("Não foram encontrados locais de instalação.");

                // busco apr que ta no bd

                var aprBD = aprPersistencia.PesquisarPorIdAnteriorAEdicao(aprModelo.CodAPR);

                var logAprAnteriorAEdicao = entities.LOG_APR.Where(x => x.CodApr == aprBD.CodAPR).OrderByDescending(x => x.CodLogApr).FirstOrDefault();

                var novaAPR = aprPersistencia.EditarApr(aprModelo, entities, locaisInstalacao);

                var logAprInserida = logAprPersistencia.Editar(aprBD, aprModelo, novaAPR, entities);

                logAprPersistencia.InserirLogOperacaoApr(aprBD, logAprAnteriorAEdicao.CodLogApr, entities);

                
            }
            
        }
        #endregion

        #region Selects
        public AprModelo PesquisarPorNumeroSerie(string numeroSerie)
        {
            AprModelo APR = new AprModelo();
            Mapper.Map(this.aprPersistencia.PesquisarPorNumeroSerie(numeroSerie), APR);
            if (numeroSerie == null)
            {
                throw new KeyNotFoundException("APR não encontrada.");
            }
            return APR;
        }

        public AprModelo PesquisarPorOrdemManutencao(string ordemManutencao)
        {
            AprModelo APR = new AprModelo();
            Mapper.Map(this.aprPersistencia.PesquisarPorOrdemManutencao(ordemManutencao), APR);
            if (ordemManutencao == "")
            {
                throw new KeyNotFoundException("APR não encontrada.");
            }
            return APR;
        }

        public bool ValidarExistenciaOrdemManutencao(string ordemManutencao, long codApr)
        {
            try
            {
                bool ordemExistente = this.aprPersistencia.ValidarExistenciaOrdemManutencao(ordemManutencao, codApr);

                return ordemExistente;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public AprModelo PesquisarPorId(long idApr)
        {
            var apr = this.aprPersistencia.PesquisarPorId(idApr);

            if (idApr == 0)
            {
                throw new KeyNotFoundException("APR não encontrada.");
            }

            return MapeamentoApr(apr);
        }

        public List<AprModelo> PesquisarPorRisco(long codRisco)
        {
            List<AprModelo> listaAPR = new List<AprModelo>();
            Mapper.Map(this.aprPersistencia.PesquisarPorRisco(codRisco), listaAPR);
            if (codRisco == 0 || listaAPR.Count == 0)
            {
                throw new KeyNotFoundException("APR não encontrada.");
            }
            return listaAPR;
        }

        public List<AprModelo> PesquisarPorLocalInstalacao(long codLocalInstalacao)
        {
            List<AprModelo> listaAPR = new List<AprModelo>();
            Mapper.Map(this.aprPersistencia.PesquisarPorLocalInstalacao(codLocalInstalacao), listaAPR);
            if (codLocalInstalacao == 0 || listaAPR.Count == 0)
            {
                throw new KeyNotFoundException("APR não encontrada.");
            }
            return listaAPR;
        }

        public List<AprModelo> PesquisarPorStatusOk()
        {
            List<AprModelo> listaAPR = new List<AprModelo>();
            Mapper.Map(this.aprPersistencia.PesquisarPorStatusOk(), listaAPR);
            if (listaAPR == null || listaAPR.Count == 0)
            {
                throw new KeyNotFoundException("Não existem APRs com status OK.");
            }
            return listaAPR;
        }

        public List<AprModelo> ListarApr(FiltroAprModelo filtroAprModelo)
        {
            List<AprModelo> aprModelo = new List<AprModelo>();

            List<APR> lista = this.aprPersistencia.ListarApr(filtroAprModelo);

            if (lista.Count == 0 || lista == null)
                throw new KeyNotFoundException("Não existem APRs para os filtros informados.");

            foreach (APR Apr in lista)
            {
                aprModelo.Add(MapeamentoApr(Apr));
            }

            foreach (var item in aprModelo)
            {
                if (item.NumeroSerie.ToString().Contains("M"))
                    item.AprEditavel = true;
            }

            return aprModelo;

        }
        #endregion

        #region GeraçãoPlanilhaAPR

        public class ErroGeracao
        {
            public int codigo { get; set; }
            public string descricao { get; set; }
        }
        public class ResultadoGeracao
        {
            public bool status = true;
            public string caminhoFinal;
            public List<ErroGeracao> erros = new List<ErroGeracao>();
        }

        public class GeracaoAPRComFolha
        {
            public bool status = true;
            public List<long> folhasAnexo = new List<long>();
        }

        public class GeracaoModelo
        {
            public bool status = true;
        }

        private void adicionaErroLista(List<ErroGeracao> lista, int codigo, string descricao)
        {
            ErroGeracao erro = new ErroGeracao();
            erro.codigo = codigo;
            erro.descricao = descricao;
            lista.Add(erro);
        }

        public ResultadoGeracao GerarPrimeiraFolhaAPR(List<NR_INVENTARIO_AMBIENTE> listaNR, string localInstalacaoAPR, string numSerieAPR,
            XLWorkbook workbook, DadosAprModelo dadosAprModelo)
        {
            ResultadoGeracao resultadoGeracao = new ResultadoGeracao();
            try
            {
                var planilha = workbook.Worksheets.Worksheet("Sheet1");
                List<string> nrPreenchidas = new List<string>();
                foreach (var itemNR in listaNR)
                {
                    if (itemNR.CodNR == (long)Constantes.NR.NR10)
                        planilha.Cell("C94").Value = "x".ToString();

                    else if (itemNR.CodNR == (long)Constantes.NR.NR11)
                        planilha.Cell("C98").Value = "x".ToString();

                    else if (itemNR.CodNR == (long)Constantes.NR.NR12)
                        planilha.Cell("BI98").Value = "x".ToString();

                    else if (itemNR.CodNR == (long)Constantes.NR.NR13)
                        planilha.Cell("AF94").Value = "x".ToString();

                    else if (itemNR.CodNR == (long)Constantes.NR.NR20)
                        planilha.Cell("AF98").Value = "x".ToString();

                    else if (itemNR.CodNR == (long)Constantes.NR.NR33)
                        planilha.Cell("C102").Value = "x".ToString();

                    else if (itemNR.CodNR == (long)Constantes.NR.NR35)
                        planilha.Cell("BI94").Value = "x".ToString();

                    else
                    {
                        var outraNR = nrPersistencia.ListarNrPorId(itemNR.CodNR);

                        if (nrPreenchidas.IndexOf(outraNR.Codigo) != -1)
                        {
                            break;
                        }
                        if (outraNR.CodNR != (long)Constantes.NR.NR0)
                        {
                            if (string.IsNullOrEmpty(planilha.Cell("AF102").Value.ToString()) && string.IsNullOrEmpty(planilha.Cell("AI102").Value.ToString()))
                            {
                                planilha.Cell("AF102").Value = "x".ToString();
                                planilha.Cell("AI102").Value = $"{outraNR.Codigo} - {outraNR.Descricao}";
                                nrPreenchidas.Add(outraNR.Codigo);
                            }
                            else if (string.IsNullOrEmpty(planilha.Cell("BI102").Value.ToString()) && string.IsNullOrEmpty(planilha.Cell("BL102").Value.ToString()))
                            {
                                planilha.Cell("BI102").Value = "x".ToString();
                                planilha.Cell("BL102").Value = $"{outraNR.Codigo} - {outraNR.Descricao}";
                                nrPreenchidas.Add(outraNR.Codigo);
                            }
                            else if (string.IsNullOrEmpty(planilha.Cell("C106").Value.ToString()) && string.IsNullOrEmpty(planilha.Cell("F106").Value.ToString()))
                            {
                                planilha.Cell("C106").Value = "x".ToString();
                                planilha.Cell("F106").Value = $"{outraNR.Codigo} - {outraNR.Descricao}";
                                nrPreenchidas.Add(outraNR.Codigo);
                            }

                            else if (string.IsNullOrEmpty(planilha.Cell("AF106").Value.ToString()) && string.IsNullOrEmpty(planilha.Cell("AI106").Value.ToString()))
                            {
                                planilha.Cell("AF106").Value = "x".ToString();
                                planilha.Cell("AI106").Value = $"{outraNR.Codigo} - {outraNR.Descricao}";
                                nrPreenchidas.Add(outraNR.Codigo);
                            }
                            else if (string.IsNullOrEmpty(planilha.Cell("BI106").Value.ToString()) && string.IsNullOrEmpty(planilha.Cell("BL106").Value.ToString()))
                            {
                                planilha.Cell("BI106").Value = "x".ToString();
                                planilha.Cell("BL106").Value = $"{outraNR.Codigo} - {outraNR.Descricao}";
                                nrPreenchidas.Add(outraNR.Codigo);
                            }
                            else
                            {
                                adicionaErroLista(resultadoGeracao.erros, 5, $"A quantidade de NRs extras excedeu o limite de 5 NRs!");
                            }
                        }
                    }
                }

                if (!String.IsNullOrEmpty(localInstalacaoAPR ))
                {
                    string[] niveis = localInstalacaoAPR.Split('-');

                    if (niveis.Count() == 2)
                    {
                        var nomeArea = $@"{niveis[0]}-{niveis[1]}";
                        planilha.Cell("AA10").Value = nomeArea;

                    }
                    else if (niveis.Count() >= 3)
                    {
                        var nomeArea = $@"{niveis[0]}-{niveis[1]}";


                        var nomeLI = string.Join("-", niveis);

                        planilha.Cell("AA10").Value = nomeArea;
                        planilha.Cell("AA18").Value = nomeLI;
                    }
                }
           
                if (string.IsNullOrEmpty(numSerieAPR) == false)
                {
                    planilha.Cell("BG4").Value = $"{numSerieAPR}".ToString();
                }

                // descrição da atividade
                if (string.IsNullOrEmpty(dadosAprModelo.DescricaoAtividade) == false)
                {
                    planilha.Cell("C26").Value = dadosAprModelo.DescricaoAtividade;
                }

                // ordem manutenção
                if (string.IsNullOrEmpty(dadosAprModelo.OrdemManutencao) == false)
                {
                    planilha.Cell("BB18").Value = dadosAprModelo.OrdemManutencao;
                }
            }
            catch (Exception)
            {
                throw;
            }

            return resultadoGeracao;
        }

        private bool EscreveValoresRiscoEInformaFolhaComplementar(string celulaMarcacao, string celulaFonteGeradora, string celulaEpi, 
            string celulaProcedimento,IXLWorksheet planilha, bool ehRiscoAmbiente , Object risco, string episSeparadosPorVirgula)
        {
            bool ehFolhaComplementar = false;
            RISCO_INVENTARIO_AMBIENTE riscoAmbiente = null;
            RISCO_INVENTARIO_ATIVIDADE riscoAtividade = null;

            if (ehRiscoAmbiente)
                riscoAmbiente = (RISCO_INVENTARIO_AMBIENTE)risco;
            else
                riscoAtividade = (RISCO_INVENTARIO_ATIVIDADE)risco;

            string fonteGeradora = ehRiscoAmbiente ? riscoAmbiente.FonteGeradora : riscoAtividade.FonteGeradora;
            string procedimentos = ehRiscoAmbiente ? riscoAmbiente.ProcedimentosAplicaveis : riscoAtividade.ProcedimentoAplicavel;
            string contraMedidas = ehRiscoAmbiente ? riscoAmbiente.ContraMedidas : riscoAtividade.ContraMedidas;

            if (CamposMaioresQueTamanhoMaximo(fonteGeradora, procedimentos, episSeparadosPorVirgula))
                ehFolhaComplementar = true;

            planilha.Cell(celulaMarcacao).Value = "x".ToString();

            if (ehFolhaComplementar || !String.IsNullOrEmpty(planilha.Cell(celulaFonteGeradora).Value.ToString()))
            {
                ehFolhaComplementar = true;

                planilha.Cell(celulaFonteGeradora).Style.Font.FontColor = XLColor.Black;
                planilha.Cell(celulaEpi).Style.Font.FontColor = XLColor.Black;
                planilha.Cell(celulaProcedimento).Style.Font.FontColor = XLColor.Black;
                planilha.Cell(celulaFonteGeradora).Value = "Consultar folha complementar...";
                planilha.Cell(celulaEpi).Value = "Consultar folha complementar...";
                planilha.Cell(celulaProcedimento).Value = "Consultar folha complementar...";
            }
            else 
            {     
                planilha.Cell(celulaFonteGeradora).Style.Font.FontColor = XLColor.Black;
                planilha.Cell(celulaEpi).Style.Font.FontColor = XLColor.Black;
                planilha.Cell(celulaProcedimento).Style.Font.FontColor = XLColor.Black;
                planilha.Cell(celulaFonteGeradora).Value = fonteGeradora;
                planilha.Cell(celulaEpi).Value = episSeparadosPorVirgula;
                planilha.Cell(celulaProcedimento).Value = procedimentos + " / " + contraMedidas;
            }

            return ehFolhaComplementar;

        }

        private bool AtualizarLinha(string marcador,string celulaFonteGeradora,string celulaEpi, string celulaProcedimento, 
            IXLWorksheet planilha, RISCO_INVENTARIO_AMBIENTE itemRisco, string episSeparadosPorVirgula, bool ehFolhaComplementar)
        {
            planilha.Cell(marcador).Value = "x".ToString();
            if (string.IsNullOrEmpty(planilha.Cell(celulaFonteGeradora).Value.ToString()) && ehFolhaComplementar == false)
            {
                AtualizarCelula(planilha, celulaFonteGeradora, celulaEpi, celulaProcedimento, itemRisco, episSeparadosPorVirgula);
                return false;
            }
            else
            {
                AtualizarLinhaComoFolhaComplementar(celulaFonteGeradora, celulaEpi, celulaProcedimento, planilha);
                return true;
            }

        }

        private bool AtualizarLinhaInventariosAtividade(string marcador, string celulaFonteGeradora,string celulaEpi,string celulaProcedimento, IXLWorksheet planilha, RISCO_INVENTARIO_ATIVIDADE itemRisco, string episSeparadosPorVirgula, bool ehFolhaComplementar)
        {
            planilha.Cell(marcador).Value = "x".ToString();
            if (string.IsNullOrEmpty(planilha.Cell(celulaFonteGeradora).Value.ToString()) && ehFolhaComplementar == false)
            {
                AtualizarCelulaInventarioAtividade(planilha,celulaFonteGeradora,celulaEpi,celulaProcedimento,itemRisco, episSeparadosPorVirgula);
                return false;
            }
            else
            {
                AtualizarLinhaComoFolhaComplementar(celulaFonteGeradora, celulaEpi, celulaProcedimento, planilha);
                return true;
            }
        }


        private void AtualizarLinhaComoFolhaComplementar(string celulaFonteGeradora,string celulaEpi,string celulaProcedimento, IXLWorksheet planilha)
        {
            planilha.Cell(celulaFonteGeradora).Style.Font.FontColor = XLColor.Black;
            planilha.Cell(celulaEpi).Style.Font.FontColor = XLColor.Black;
            planilha.Cell(celulaProcedimento).Style.Font.FontColor = XLColor.Black;
            planilha.Cell(celulaFonteGeradora).Value = "Consultar folha complementar...";
            planilha.Cell(celulaEpi).Value = "Consultar folha complementar...";
            planilha.Cell(celulaProcedimento).Value = "Consultar folha complementar...";
        }

        private void SalvarFolhasComplementares(XLWorkbook folhaComplementar, string numeroSerie)
        {
            if (folhaComplementar != null && folhaComplementar.Worksheets.Any())
            {

                foreach (var worksheet in folhaComplementar.Worksheets)
                {

                    if (worksheet.Name != "Verso")
                    {
                        worksheet.Row(9).Delete();
                    }
                }
                folhaComplementar.Worksheet("Frente").Delete();
                
                if (folhaComplementar.Worksheets.Any())
                {
                    PreencherIndiceFolhasComplementaresGeradas(folhaComplementar);
                    folhaComplementar.Save();
                    folhaComplementar.Dispose();
                }

            }
        }

        private bool CamposMaioresQueTamanhoMaximo(string fonteGeradora, string procedimentosAplicaveis, string episConcatenados)
        {
            if (fonteGeradora.Length <= (long)Constantes.QuebraDadosPlanilhaApr.MAX_FONTE_GERADORA &&
                episConcatenados.Length <= (long)Constantes.QuebraDadosPlanilhaApr.MAX_EPI &&
                procedimentosAplicaveis.Length <= (long)Constantes.QuebraDadosPlanilhaApr.MAX_PA_CM)
                return false;

            return true;
        }

        private bool VerificarLimiteLinhasFolhaComplementar(int qtdItensPorFolha)
        {
            if (qtdItensPorFolha % Constantes.LIMITE_ITENS_POR_FOLHA_COMPLEMENTAR == 0 && qtdItensPorFolha != 0)
            {
                return true;
            }
            return false;
        }

        private string ObterNomesEpis(List<EPI> listaEpis)
        {
            string nomeEpis = "";
            if (listaEpis.Count() == 1)
            {
                return ObterUltimoNivelEpi(listaEpis.First());
            }
            foreach (var epi in listaEpis)
            {
                var nomeEpi = ObterUltimoNivelEpi(epi);
                nomeEpis += $"{nomeEpi},";
            }
            nomeEpis = nomeEpis.Substring(0, nomeEpis.Length - 1);
            return nomeEpis;
        }

        private bool ValidarTamanhoCamposAtividade(RISCO_INVENTARIO_ATIVIDADE itemRisco, string epi)
        {
            if (itemRisco.FonteGeradora.Length <= (long)Constantes.QuebraDadosPlanilhaApr.MAX_FONTE_GERADORA &&
                epi.Length <= (long)Constantes.QuebraDadosPlanilhaApr.MAX_EPI &&
                itemRisco.ProcedimentoAplicavel.Length <= (long)Constantes.QuebraDadosPlanilhaApr.MAX_PA_CM)
            {
                return true;
            }
            return false;
        }

        private void AtualizarCelula(IXLWorksheet planilha,string colunaCelula1,string colunaCelula2, string colunaCelula3, RISCO_INVENTARIO_AMBIENTE itemRisco, string episSeparadosPorVirgula)
        {
            planilha.Cell(colunaCelula1).Style.Font.FontColor= XLColor.Black;
            planilha.Cell(colunaCelula2).Style.Font.FontColor= XLColor.Black;
            planilha.Cell(colunaCelula3).Style.Font.FontColor= XLColor.Black;
            planilha.Cell(colunaCelula1).Value= itemRisco.FonteGeradora;
            planilha.Cell(colunaCelula2).Value= episSeparadosPorVirgula;
            planilha.Cell(colunaCelula3).Value = itemRisco.ProcedimentosAplicaveis + " / " + itemRisco.ContraMedidas;

         }


        private void AtualizarCelulaInventarioAtividade(IXLWorksheet planilha, string colunaCelula1, string colunaCelula2, string colunaCelula3, RISCO_INVENTARIO_ATIVIDADE itemRisco, string episSeparadosPorVirgula)
        {
            planilha.Cell(colunaCelula1).Style.Font.FontColor = XLColor.Black;
            planilha.Cell(colunaCelula2).Style.Font.FontColor = XLColor.Black;
            planilha.Cell(colunaCelula3).Style.Font.FontColor = XLColor.Black;
            planilha.Cell(colunaCelula1).Value = itemRisco.FonteGeradora;
            planilha.Cell(colunaCelula2).Value = episSeparadosPorVirgula;
            planilha.Cell(colunaCelula3).Value = itemRisco.ProcedimentoAplicavel + " / " + itemRisco.ContraMedidas;
        }



        private string ObterUltimoNivelEpi(EPI epi)
        {
            if (!string.IsNullOrEmpty(epi.N3))
            {
                return epi.N3;
            }
            if (!string.IsNullOrEmpty(epi.N2))
            {
                return epi.N2;
            }
            if (!string.IsNullOrEmpty(epi.N1))
            {
                return epi.N1;
            }
            return "";
        }

        private XLWorkbook GerarNovaFolhaComplementar(int qtdItensPorFolha, string caminhoGeracao)
        {
            string caminhoModeloFolha = ArquivoDiretorioUtils.ObterDiretorioModelo();
            string caminhoGeracaoFolha = caminhoGeracao;
            string caminhoCompletoFolhaModelo = $"{caminhoModeloFolha}LayoutFolhaComplementar.xlsx";
            string caminhoCompletoFolhaFinal = $"{caminhoGeracaoFolha}\\FolhaComplementarGerada.xlsx";
            if (qtdItensPorFolha > 0)
            {
              caminhoCompletoFolhaFinal = $"{caminhoGeracaoFolha}\\FolhaComplementarGerada{qtdItensPorFolha}.xlsx";
            }
            ArquivoDiretorioUtils.CopiarArquivo(caminhoCompletoFolhaModelo,caminhoCompletoFolhaFinal);
            return new XLWorkbook(caminhoCompletoFolhaFinal);
        }

        private string RetornaEPIsConcatenados(bool riscosDeAmbiente, ICollection<EPI_RISCO_INVENTARIO_AMBIENTE> lstEpiRiscosInvAmbiente,
            ICollection<EPI_RISCO_INVENTARIO_ATIVIDADE> lstEpiRiscosInvAtividade)
        {
            string episSeparadosPorVirgula = string.Empty;

            if (riscosDeAmbiente)
            {

                foreach (var itemEpiRisco in lstEpiRiscosInvAmbiente)
                {
                    var epi = ePIPersistencia.ListarEPIPorId(itemEpiRisco.CodEPI);
                    episSeparadosPorVirgula += $"{ObterUltimoNivelEpi(epi)},";
                }
            }
            else
            {
                foreach (var itemEpiRisco in lstEpiRiscosInvAtividade)
                {
                    var epi = ePIPersistencia.ListarEPIPorId(itemEpiRisco.CodEPI);
                    episSeparadosPorVirgula += $"{ObterUltimoNivelEpi(epi)},";
                }
            }

            if (episSeparadosPorVirgula.Length > 0)
            {
                episSeparadosPorVirgula = episSeparadosPorVirgula.Substring(0, episSeparadosPorVirgula.Length - 1);
            }


            return episSeparadosPorVirgula;
        }

        private void PreencheLinhaDeAcordoComRisco(XLWorkbook workbook, long codRisco, long tipoPlanilha, IXLWorksheet planilha, Object risco,
            bool ehRiscoAmbienteTerceiraFolha,string episSeparadosPorVirgula,XLWorkbook folhaComplementar, ref int qtdItensPorFolha, string numeroSerie, 
            ref string folhaAtual, ref int qtdFolhaComplementar, string caminhoGeracao)
        {
            bool ehFolhaComplementar = false;

            RISCO_INVENTARIO_AMBIENTE riscoAmbiente = null;
            RISCO_INVENTARIO_ATIVIDADE riscoAtividade = null;

            if (tipoPlanilha == (long)Constantes.Sheets_Planilha_APR.SHEET2 || ehRiscoAmbienteTerceiraFolha)
                riscoAmbiente = (RISCO_INVENTARIO_AMBIENTE)risco;
            else
                riscoAtividade = (RISCO_INVENTARIO_ATIVIDADE)risco;

            string celulaFonteMarcacao = string.Empty;
            string celulaFonteGeradora = string.Empty;
            string celulaEPI = string.Empty;
            string celulaProcedimento = string.Empty;

            //Preenche o num série no cabeçalho das folhas 2 e 3
            workbook.Worksheet("Sheet2").Cell("BG4").Value = numeroSerie;
            workbook.Worksheet("Sheet3").Cell("BG4").Value = numeroSerie;


            if (tipoPlanilha == (long)Constantes.Sheets_Planilha_APR.SHEET2)
            {
                #region [ MARCAÇÃO PÁGINA 2 ]

                switch (codRisco)
                {
                    // FISICOS
                    case (long)Constantes.RiscoAmbiente.RUIDO:
                        {
                            celulaFonteMarcacao = "D12"; celulaFonteGeradora = "R12"; celulaEPI = "AK12"; celulaProcedimento = "BC12";
                            break;
                        }

                    case (long)Constantes.RiscoAmbiente.VIBRACAO:
                        {
                            celulaFonteMarcacao = "D17"; celulaFonteGeradora = "R16"; celulaEPI = "AK16"; celulaProcedimento = "BC16";
                            break;
                        }

                    case (long)Constantes.RiscoAmbiente.ALTAS_TEMPERATURAS:
                        {
                            celulaFonteMarcacao = "D22"; celulaFonteGeradora = "R21"; celulaEPI = "AK21"; celulaProcedimento = "BC21";
                            break;
                        }
                    case (long)Constantes.RiscoAmbiente.BAIXAS_TEMPERATURAS:
                        {
                            celulaFonteMarcacao = "D27"; celulaFonteGeradora = "R26"; celulaEPI = "AK26"; celulaProcedimento = "BC26";
                            break;
                        }
                    case (long)Constantes.RiscoAmbiente.PRESSAO:
                        {
                            celulaFonteMarcacao = "D32"; celulaFonteGeradora = "R31"; celulaEPI = "AK31"; celulaProcedimento = "BC31";
                            break;
                        }
                    case (long)Constantes.RiscoAmbiente.RADIACAO_IONIZANTE:
                        {
                            celulaFonteMarcacao = "D37"; celulaFonteGeradora = "R36"; celulaEPI = "AK36"; celulaProcedimento = "BC36";
                            break;
                        }
                    case (long)Constantes.RiscoAmbiente.RADIACAO_NAO_IONIZANTE:
                        {
                            celulaFonteMarcacao = "D42"; celulaFonteGeradora = "R41"; celulaEPI = "AK41"; celulaProcedimento = "BC41";
                            break;
                        }

                    // QUIMICOS
                    case (long)Constantes.RiscoAmbiente.UMIDADE:
                        {
                            celulaFonteMarcacao = "D47"; celulaFonteGeradora = "R46"; celulaEPI = "AK46"; celulaProcedimento = "BC46";
                            break;
                        }
                    case (long)Constantes.RiscoAmbiente.POEIRAS:
                        {
                            celulaFonteMarcacao = "D57"; celulaFonteGeradora = "R57"; celulaEPI = "AK57"; celulaProcedimento = "BC57";
                            break;
                        }
                    case (long)Constantes.RiscoAmbiente.FUMOS:
                        {
                            celulaFonteMarcacao = "D62"; celulaFonteGeradora = "R61"; celulaEPI = "AK61"; celulaProcedimento = "BC61";
                            break;
                        }
                    case (long)Constantes.RiscoAmbiente.NEVOAS:
                        {
                            celulaFonteMarcacao = "D67"; celulaFonteGeradora = "R66"; celulaEPI = "AK66"; celulaProcedimento = "BC66";
                            break;
                        }
                    case (long)Constantes.RiscoAmbiente.NEBLINA:
                        {
                            celulaFonteMarcacao = "D72"; celulaFonteGeradora = "R71"; celulaEPI = "AK71"; celulaProcedimento = "BC71";
                            break;
                        }

                    // ERGONOMICOS
                    case (long)Constantes.RiscoAmbiente.GASES:
                        {
                            celulaFonteMarcacao = "D77"; celulaFonteGeradora = "R76"; celulaEPI = "AK76"; celulaProcedimento = "BC76";
                            break;
                        }
                    case (long)Constantes.RiscoAmbiente.VAPORES:
                        {
                            celulaFonteMarcacao = "D82"; celulaFonteGeradora = "R81"; celulaEPI = "AK81"; celulaProcedimento = "BC81";
                            break;
                        }
                    case (long)Constantes.RiscoAmbiente.BACTERIAS:
                        {
                            celulaFonteMarcacao = "D92"; celulaFonteGeradora = "R92"; celulaEPI = "AK92"; celulaProcedimento = "BC92";
                            break;
                        }
                    case (long)Constantes.RiscoAmbiente.FUNGOS:
                        {
                            celulaFonteMarcacao = "D97"; celulaFonteGeradora = "R96"; celulaEPI = "AK96"; celulaProcedimento = "BC96";
                            break;
                        }
                    case (long)Constantes.RiscoAmbiente.PARASITAS:
                        {
                            celulaFonteMarcacao = "D102"; celulaFonteGeradora = "R101"; celulaEPI = "AK101"; celulaProcedimento = "BC101";
                            break;
                        }
                    case (long)Constantes.RiscoAmbiente.BACILOS:
                        {
                            celulaFonteMarcacao = "D107"; celulaFonteGeradora = "R106"; celulaEPI = "AK106"; celulaProcedimento = "BC106";
                            break;
                        }
                    case (long)Constantes.RiscoAmbiente.VIRUS:
                        {
                            celulaFonteMarcacao = "D112"; celulaFonteGeradora = "R111"; celulaEPI = "AK111"; celulaProcedimento = "BC111";
                            break;
                        }
                    case (long)Constantes.RiscoAmbiente.LEVANTAMENTO_PESO:
                        {
                            celulaFonteMarcacao = "D123"; celulaFonteGeradora = "R122"; celulaEPI = "AK122"; celulaProcedimento = "BC122";
                            break;
                        }
                    case (long)Constantes.RiscoAmbiente.RITMO_EXCESSIVO:
                        {
                            celulaFonteMarcacao = "D128"; celulaFonteGeradora = "R127"; celulaEPI = "AK127"; celulaProcedimento = "BC127";
                            break;
                        }
                    case (long)Constantes.RiscoAmbiente.MONOTONIA:
                        {
                            celulaFonteMarcacao = "D133"; celulaFonteGeradora = "R132"; celulaEPI = "AK132"; celulaProcedimento = "BC132";
                            break;
                        }
                    case (long)Constantes.RiscoAmbiente.REPETITIVIDADE:
                        {
                            celulaFonteMarcacao = "D138"; celulaFonteGeradora = "R137"; celulaEPI = "AK137"; celulaProcedimento = "BC137";
                            break;
                        }
                    case (long)Constantes.RiscoAmbiente.POSICAO:
                        {
                            celulaFonteMarcacao = "D143"; celulaFonteGeradora = "R142"; celulaEPI = "AK142"; celulaProcedimento = "BC142";
                            break;
                        }
                    case (long)Constantes.RiscoAmbiente.QUEDA:
                        {
                            celulaFonteMarcacao = "D148"; celulaFonteGeradora = "R147"; celulaEPI = "AK147"; celulaProcedimento = "BC147";
                            break;
                        }
                    default:
                        break;
                }

                #endregion

                //Escreve os valores na planilha
                ehFolhaComplementar = this.EscreveValoresRiscoEInformaFolhaComplementar(celulaFonteMarcacao, celulaFonteGeradora, celulaEPI, celulaProcedimento,
                    planilha, true, riscoAmbiente, episSeparadosPorVirgula);
            }
            else
            {

                if (riscoAmbiente != null)
                {
                    #region [ MARCAÇÃO PÁGINA 3 - RISCOS AMBIENTE]
                    switch (riscoAmbiente.CodRiscoAmbiente)
                    {
                        case (long)Constantes.RiscoAmbiente.CHOQUE_ELETRICO:
                            {
                                celulaFonteMarcacao = "D13"; celulaFonteGeradora = "R12"; celulaEPI = "AK12"; celulaProcedimento = "BC12";
                                break;
                            }
                        case (long)Constantes.RiscoAmbiente.ILUMINACAO:
                            {
                                celulaFonteMarcacao = "D18"; celulaFonteGeradora = "R17"; celulaEPI = "AK17"; celulaProcedimento = "BC17";
                                break;
                            }
                        case (long)Constantes.RiscoAmbiente.CHOQUE_MECANICO:
                            {
                                celulaFonteMarcacao = "D23"; celulaFonteGeradora = "R22"; celulaEPI = "AK22"; celulaProcedimento = "BC22";
                                break;
                            }
                        case (long)Constantes.RiscoAmbiente.INCENDIO:
                            {
                                celulaFonteMarcacao = "D28"; celulaFonteGeradora = "R27"; celulaEPI = "AK27"; celulaProcedimento = "BC27";
                                break;
                            }
                        case (long)Constantes.RiscoAmbiente.EXPLOSAO:
                            {
                                celulaFonteMarcacao = "D33"; celulaFonteGeradora = "R32"; celulaEPI = "AK32"; celulaProcedimento = "BC32";
                                break;
                            }
                        case (long)Constantes.RiscoAmbiente.SOTERRAMENTO:
                            {
                                celulaFonteMarcacao = "D38"; celulaFonteGeradora = "R37"; celulaEPI = "AK37"; celulaProcedimento = "BC37";
                                break;
                            }
                        default:
                            break;
                    }
                    #endregion

                    //Escreve os valores na planilha
                    ehFolhaComplementar = this.EscreveValoresRiscoEInformaFolhaComplementar(celulaFonteMarcacao, celulaFonteGeradora, celulaEPI, celulaProcedimento,
                        planilha, true, riscoAmbiente, episSeparadosPorVirgula);
                }

                else
                {
                    #region [ MARCAÇÃO PÁGINA 3 - RISCOS ATIVIDADE]
                    switch (riscoAtividade.CodRisco)
                    {
                        case (long)Constantes.RiscoAtividade.PRENSAMENTOS:
                            {
                                celulaFonteMarcacao = "D50"; celulaFonteGeradora = "R48"; celulaEPI = "AK48"; celulaProcedimento = "BC48";
                                break;
                            }
                        case (long)Constantes.RiscoAtividade.QUEDAS:
                            {
                                celulaFonteMarcacao = "D57"; celulaFonteGeradora = "R55"; celulaEPI = "AK55"; celulaProcedimento = "BC55";
                                break;
                            }
                        case (long)Constantes.RiscoAtividade.CORTES:
                            {
                                celulaFonteMarcacao = "D64"; celulaFonteGeradora = "R62"; celulaEPI = "AK62"; celulaProcedimento = "BC62";
                                break;
                            }
                        case (long)Constantes.RiscoAtividade.AMPUTACOES:
                            {
                                celulaFonteMarcacao = "D71"; celulaFonteGeradora = "R69"; celulaEPI = "AK69"; celulaProcedimento = "BC69";
                                break;
                            }
                        case (long)Constantes.RiscoAtividade.PERFURACOES:
                            {
                                celulaFonteMarcacao = "D78"; celulaFonteGeradora = "R76"; celulaEPI = "AK76"; celulaProcedimento = "BC76";
                                break;
                            }
                        case (long)Constantes.RiscoAtividade.QUEIMADURAS:
                            {
                                celulaFonteMarcacao = "D85"; celulaFonteGeradora = "R83"; celulaEPI = "AK83"; celulaProcedimento = "BC83";
                                break;
                            }
                        case (long)Constantes.RiscoAtividade.CHOQUE_ELETRICO:
                            {
                                celulaFonteMarcacao = "D92"; celulaFonteGeradora = "R90"; celulaEPI = "AK90"; celulaProcedimento = "BC90";
                                break;
                            }
                        case (long)Constantes.RiscoAtividade.OUTROS:
                            {
                                celulaFonteMarcacao = "D99"; celulaFonteGeradora = "R97"; celulaEPI = "AK97"; celulaProcedimento = "BC97";
                                break;
                            }
                        default:
                            break;
                    }
                    #endregion

                    //Escreve os valores na planilha
                    ehFolhaComplementar = this.EscreveValoresRiscoEInformaFolhaComplementar(celulaFonteMarcacao, celulaFonteGeradora, celulaEPI, celulaProcedimento,
                        planilha, false, riscoAtividade, episSeparadosPorVirgula);
                }

            }

            //Escreve os dados na folha complementar 
            if (ehFolhaComplementar)
            {
                
                if (folhaComplementar == null)
                {
                    folhaComplementar = GerarNovaFolhaComplementar(qtdFolhaComplementar, caminhoGeracao);
                    qtdFolhaComplementar++;
                    folhaAtual = AdicionarNovaFolhaComplementar(folhaComplementar, qtdFolhaComplementar);
                    PreencherCabecalhoFolhaComplementar(folhaAtual, folhaComplementar, numeroSerie, workbook, qtdFolhaComplementar);
                }

                if (VerificarLimiteLinhasFolhaComplementar(qtdItensPorFolha))
                {
                    qtdItensPorFolha = 0;
                    qtdFolhaComplementar++;                   
                    folhaAtual = AdicionarNovaFolhaComplementar(folhaComplementar, qtdFolhaComplementar);
                    PreencherCabecalhoFolhaComplementar(folhaAtual, folhaComplementar, numeroSerie, workbook, qtdFolhaComplementar);
                }
                

                if (string.IsNullOrEmpty(folhaAtual))
                {
                    qtdFolhaComplementar++;
                    folhaAtual = AdicionarNovaFolhaComplementar(folhaComplementar, qtdFolhaComplementar);
                    PreencherCabecalhoFolhaComplementar(folhaAtual, folhaComplementar, numeroSerie, workbook, qtdFolhaComplementar);
                }


                string nomeRisco = riscoAmbiente != null ? riscoAmbiente.RISCO.Nome : riscoAtividade.RISCO.Nome;
                string fonteGeradora = riscoAmbiente != null ? riscoAmbiente.FonteGeradora : riscoAtividade.FonteGeradora;
                string procedimentos = riscoAmbiente != null ? riscoAmbiente.ProcedimentosAplicaveis : riscoAtividade.ProcedimentoAplicavel;
                string contraMedidas = riscoAmbiente != null ? riscoAmbiente.ContraMedidas : riscoAtividade.ContraMedidas;

                qtdItensPorFolha++;
                PreencherFolhaComplementar(folhaAtual, nomeRisco, fonteGeradora, episSeparadosPorVirgula, $"{procedimentos + " / " + contraMedidas}", 
                    qtdItensPorFolha, folhaComplementar);
            }           
        }

        public ResultadoGeracao GerarSegundaETerceiraFolha(XLWorkbook planilhaAPR, List<RISCO_INVENTARIO_AMBIENTE> riscosInvAmbiente,
            List<RISCO_INVENTARIO_ATIVIDADE> riscosInvAtividade, XLWorkbook folhaComplementar, ref int qtdItensPorFolha, string numeroSerie,
            ref string folhaAtual, ref int qtdFolhaComplementar, string caminhoGeracao)
        {
            ResultadoGeracao resultadoGeracao = new ResultadoGeracao();

            try
            {
                IXLWorksheet sheet;

                //Preenche os riscos de ambiente
                foreach (var riscoAmbiente in riscosInvAmbiente)
                {
                    //Busca os EPIs
                    var episSeparadosPorVirgula = this.RetornaEPIsConcatenados(true, riscoAmbiente.EPI_RISCO_INVENTARIO_AMBIENTE, null);

                    //Verifica em qual planilha irá preencher o risco de ambiente
                    long planilha = RiscosAmbienteSegundaPagina(riscoAmbiente) ? (long)Sheets_Planilha_APR.SHEET2 : (long)Sheets_Planilha_APR.SHEET3;

                    //Verifica se é risco de ambiente da terceira folha
                    bool ehRiscoAmbienteTerceiraFolha = RiscosAmbienteTerceiraPagina(riscoAmbiente);

                    if (planilha == 2)
                        sheet = planilhaAPR.Worksheets.Worksheet("Sheet2");
                    else
                        sheet = planilhaAPR.Worksheets.Worksheet("Sheet3");

                    //Preenche o valor
                    PreencheLinhaDeAcordoComRisco(planilhaAPR,riscoAmbiente.CodRiscoAmbiente, planilha, sheet, riscoAmbiente, ehRiscoAmbienteTerceiraFolha, 
                        episSeparadosPorVirgula, folhaComplementar,ref qtdItensPorFolha, numeroSerie, ref folhaAtual, ref qtdFolhaComplementar, caminhoGeracao);
                }

                IXLWorksheet folhaPlanilhaAPR = planilhaAPR.Worksheets.Worksheet("Sheet3");

                foreach (var riscoAtividade in riscosInvAtividade)
                {
   
                    //Busca os EPIs
                    var episSeparadosPorVirgula = this.RetornaEPIsConcatenados(false, null, riscoAtividade.EPI_RISCO_INVENTARIO_ATIVIDADE);

                    //Verifica em qual planilha irá preencher o risco de ambiente
                    long planilha = (long)Sheets_Planilha_APR.SHEET3;
                    sheet = planilhaAPR.Worksheets.Worksheet("Sheet3");

                    //Preenche o valor
                    PreencheLinhaDeAcordoComRisco(planilhaAPR,riscoAtividade.CodRiscoInventarioAtividade, planilha, sheet, riscoAtividade, false, episSeparadosPorVirgula, 
                        folhaComplementar, ref qtdItensPorFolha, numeroSerie, ref folhaAtual, ref qtdFolhaComplementar, caminhoGeracao);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return resultadoGeracao;
        }

        public ResultadoGeracao PreencherRiscos(XLWorkbook workbook, INVENTARIO_AMBIENTE inventarioAmbiente, INVENTARIO_ATIVIDADE inventarioAtividade,
            XLWorkbook folhaComplementar, ref int qtdItensPorFolha, string numeroSerie, ref string folhaAtual, ref int qtdFolhaComplementar, string caminhoGeracao)
        {
            ResultadoGeracao resultadoGeracao = new ResultadoGeracao();

            try
            {
                var planilha3 = workbook.Worksheets.Worksheet("Sheet3");

                ICollection<RISCO_INVENTARIO_AMBIENTE> riscosRecuperadosAmbiente = inventarioAmbiente.RISCO_INVENTARIO_AMBIENTE;
                ICollection<RISCO_INVENTARIO_ATIVIDADE> riscosRecuperados = inventarioAtividade.RISCO_INVENTARIO_ATIVIDADE;
                bool ehFolhaComplementar = false;
                var listaTodosOsRiscos = riscoPersistencia.ListarTodosRiscos();
                foreach (var itemRiscoAmbiente in riscosRecuperadosAmbiente)
                {
                    string episSeparadosPorVirgula = "";
                    if(itemRiscoAmbiente.EPI_RISCO_INVENTARIO_AMBIENTE.Count > 0)
                    {
                        foreach (var itemEpiRisco in itemRiscoAmbiente.EPI_RISCO_INVENTARIO_AMBIENTE)
                        {
                            var epi = ePIPersistencia.ListarEPIPorId(itemEpiRisco.CodEPI);
                            episSeparadosPorVirgula += $"{ObterUltimoNivelEpi(epi)},";
                        }
                        episSeparadosPorVirgula = episSeparadosPorVirgula.Substring(0, episSeparadosPorVirgula.Length - 1);
                    }

                    var nomeRisco = listaTodosOsRiscos.First(x=> x.CodRisco == itemRiscoAmbiente.CodRiscoAmbiente).Nome;

                    if (!CamposMaioresQueTamanhoMaximo(itemRiscoAmbiente.FonteGeradora, itemRiscoAmbiente.ProcedimentosAplicaveis, episSeparadosPorVirgula) 
                        && RiscosAmbienteTerceiraPagina(itemRiscoAmbiente))
                    {
                        ehFolhaComplementar = true;
                    }
                    else
                    {
                        switch (itemRiscoAmbiente.CodRiscoAmbiente)
                        {
                            case (long)Constantes.RiscoAmbiente.CHOQUE_ELETRICO:
                                ehFolhaComplementar = AtualizarLinha("D13", "R12", "AK12", "BC12", planilha3, itemRiscoAmbiente, episSeparadosPorVirgula, ehFolhaComplementar);
                                break;

                            case (long)Constantes.RiscoAmbiente.ILUMINACAO:
                                ehFolhaComplementar = AtualizarLinha("D18", "R17", "AK17", "BC17", planilha3, itemRiscoAmbiente, episSeparadosPorVirgula, ehFolhaComplementar);
                                break;

                            case (long)Constantes.RiscoAmbiente.CHOQUE_MECANICO:
                                ehFolhaComplementar = AtualizarLinha("D23", "R22", "AK22", "BC22", planilha3, itemRiscoAmbiente, episSeparadosPorVirgula, ehFolhaComplementar);
                                break;

                            case (long)Constantes.RiscoAmbiente.INCENDIO:
                                ehFolhaComplementar = AtualizarLinha("D28", "R27", "AK27", "BC27", planilha3, itemRiscoAmbiente, episSeparadosPorVirgula, ehFolhaComplementar);
                                break;

                            case (long)Constantes.RiscoAmbiente.EXPLOSAO:
                                ehFolhaComplementar = AtualizarLinha("D33", "R32", "AK32", "BC32", planilha3, itemRiscoAmbiente, episSeparadosPorVirgula, ehFolhaComplementar);
                                break;

                            case (long)Constantes.RiscoAmbiente.SOTERRAMENTO:
                                ehFolhaComplementar = AtualizarLinha("D38", "R37", "AK37", "BC37", planilha3, itemRiscoAmbiente, episSeparadosPorVirgula, ehFolhaComplementar);
                                break;

                            default:
                                break;
                        }
                    }
                    if (ehFolhaComplementar)
                    {
                        ehFolhaComplementar = false;
                        if (folhaComplementar == null)
                        {
                            folhaComplementar = GerarNovaFolhaComplementar(qtdFolhaComplementar, caminhoGeracao);
                            qtdFolhaComplementar++;
                            folhaAtual = AdicionarNovaFolhaComplementar(folhaComplementar, qtdFolhaComplementar);
                            PreencherCabecalhoFolhaComplementar(folhaAtual, folhaComplementar, numeroSerie, workbook, qtdFolhaComplementar);
                        }
                        if (VerificarLimiteLinhasFolhaComplementar(qtdItensPorFolha))
                        {
                            qtdItensPorFolha = 0;
                            qtdFolhaComplementar++;
                            PreencherCabecalhoFolhaComplementar(folhaAtual, folhaComplementar, numeroSerie, workbook, qtdFolhaComplementar);
                        }
                        if (string.IsNullOrEmpty(folhaAtual))
                        {
                            qtdFolhaComplementar++;
                            folhaAtual = AdicionarNovaFolhaComplementar(folhaComplementar, qtdFolhaComplementar);
                            PreencherCabecalhoFolhaComplementar(folhaAtual, folhaComplementar, numeroSerie, workbook, qtdFolhaComplementar);
                        }
                        qtdItensPorFolha++;
                        PreencherFolhaComplementar(folhaAtual,nomeRisco, itemRiscoAmbiente.FonteGeradora, episSeparadosPorVirgula,
                            $"{itemRiscoAmbiente.ProcedimentosAplicaveis + " / " + itemRiscoAmbiente.ContraMedidas}", qtdItensPorFolha, folhaComplementar);
                    }
                }

                // RISCOS ATIVIDADE
                foreach (var itemRisco in riscosRecuperados)
                {
                    var nomeRisco = listaTodosOsRiscos.First(x => x.CodRisco == itemRisco.CodRisco).Nome;
                    var epiRisco = epiRiscoInventarioAtividadePersistencia.ListarEPIPorRisco(itemRisco.CodRisco);
                    var epi = new EPI();
                    var listaEpis = new List<EPI>();
                    string episSeparadosPorVirgula = "";

                    if (epiRisco != null && epiRisco.Any() )
                    {
                        var listaCodEpis = epiRisco.Select(x => x.CodEPI).ToList();
                        listaEpis = ePIPersistencia.ListarEPIsPorListaId(listaCodEpis);
                        episSeparadosPorVirgula = ObterNomesEpis(listaEpis);
                    }

                    if (!ValidarTamanhoCamposAtividade(itemRisco, episSeparadosPorVirgula))
                    {
                        ehFolhaComplementar = true;
                    }

                    switch (itemRisco.CodRisco)
                    {
                        case (long)Constantes.RiscoAtividade.PRENSAMENTOS:

                          ehFolhaComplementar = AtualizarLinhaInventariosAtividade("D50", "R48", "AK48", "BC48", planilha3, itemRisco, episSeparadosPorVirgula, 
                                ehFolhaComplementar);
                          break;

                        case (long)Constantes.RiscoAtividade.QUEDAS:
                            ehFolhaComplementar = AtualizarLinhaInventariosAtividade("D57", "R55", "AK55", "BC55", planilha3, itemRisco, episSeparadosPorVirgula,
                                ehFolhaComplementar);
                            break;

                        case (long)Constantes.RiscoAtividade.CORTES:
                            ehFolhaComplementar = AtualizarLinhaInventariosAtividade("D64", "R62", "AK62", "BC62", planilha3, itemRisco, episSeparadosPorVirgula,
                                ehFolhaComplementar);
                            break;

                        case (long)Constantes.RiscoAtividade.AMPUTACOES:
                            ehFolhaComplementar = AtualizarLinhaInventariosAtividade("D71", "R69", "AK69", "BC69", planilha3, itemRisco, episSeparadosPorVirgula,
                                ehFolhaComplementar);
                            break;

                        case (long)Constantes.RiscoAtividade.PERFURACOES:
                            ehFolhaComplementar = AtualizarLinhaInventariosAtividade("D78", "R76", "AK76", "BC76", planilha3, itemRisco, episSeparadosPorVirgula,
                                ehFolhaComplementar);
                            break;

                        case (long)Constantes.RiscoAtividade.QUEIMADURAS:
                            ehFolhaComplementar = AtualizarLinhaInventariosAtividade("D85", "R83", "AK83", "BC83", planilha3, itemRisco, episSeparadosPorVirgula,
                                ehFolhaComplementar);
                            break;

                        case (long)Constantes.RiscoAtividade.CHOQUE_ELETRICO:
                            ehFolhaComplementar = AtualizarLinhaInventariosAtividade("D92", "R90", "AK90", "BC90", planilha3, itemRisco, episSeparadosPorVirgula,
                                ehFolhaComplementar);
                            break;

                        case (long)Constantes.RiscoAtividade.OUTROS:
                            ehFolhaComplementar = AtualizarLinhaInventariosAtividade("D99", "R97", "AK97", "BC97", planilha3, itemRisco, episSeparadosPorVirgula,
                                ehFolhaComplementar);
                            break;
                        default:
                            break;
                    }
                    
                    if (ehFolhaComplementar)
                    {
                        ehFolhaComplementar = false;
                        if (folhaComplementar == null)
                        {
                          folhaComplementar = GerarNovaFolhaComplementar(qtdFolhaComplementar, caminhoGeracao);
                            qtdFolhaComplementar++;
                           folhaAtual = AdicionarNovaFolhaComplementar(folhaComplementar, qtdFolhaComplementar);
                           PreencherCabecalhoFolhaComplementar(folhaAtual, folhaComplementar, numeroSerie, workbook, qtdFolhaComplementar);
                        }
                        if (VerificarLimiteLinhasFolhaComplementar(qtdItensPorFolha))
                        {
                            qtdItensPorFolha = 0;
                            qtdFolhaComplementar++;
                            folhaAtual = AdicionarNovaFolhaComplementar(folhaComplementar, qtdFolhaComplementar);
                            PreencherCabecalhoFolhaComplementar(folhaAtual, folhaComplementar, numeroSerie, workbook, qtdFolhaComplementar);
                        }
                        if (string.IsNullOrEmpty(folhaAtual))
                        {
                            qtdFolhaComplementar++;
                            folhaAtual = AdicionarNovaFolhaComplementar(folhaComplementar, qtdFolhaComplementar);
                            PreencherCabecalhoFolhaComplementar(folhaAtual, folhaComplementar, numeroSerie, workbook, qtdFolhaComplementar);
                        }
                        qtdItensPorFolha++;
                        PreencherFolhaComplementar(folhaAtual,nomeRisco, itemRisco.FonteGeradora, episSeparadosPorVirgula,
                            $"{itemRisco.ProcedimentoAplicavel + " / " + itemRisco.ContraMedidas}", qtdItensPorFolha, folhaComplementar);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return resultadoGeracao;
        }

        public bool RiscosAmbienteSegundaPagina(RISCO_INVENTARIO_AMBIENTE itemRisco)
        {
            switch (itemRisco.CodRiscoAmbiente)
            {
                // FISICOS
                case (long)Constantes.RiscoAmbiente.RUIDO:
                    return true;
                case (long)Constantes.RiscoAmbiente.VIBRACAO:
                    return true;
                case (long)Constantes.RiscoAmbiente.ALTAS_TEMPERATURAS:
                    return true;
                case (long)Constantes.RiscoAmbiente.BAIXAS_TEMPERATURAS:
                    return true;
                case (long)Constantes.RiscoAmbiente.PRESSAO:
                    return true;
                case (long)Constantes.RiscoAmbiente.RADIACAO_IONIZANTE:
                    return true;
                case (long)Constantes.RiscoAmbiente.RADIACAO_NAO_IONIZANTE:
                    return true;
                case (long)Constantes.RiscoAmbiente.UMIDADE:
                    return true;
                case (long)Constantes.RiscoAmbiente.POEIRAS:
                    return true;
                case (long)Constantes.RiscoAmbiente.FUMOS:
                    return true;
                case (long)Constantes.RiscoAmbiente.NEVOAS:
                    return true;
                case (long)Constantes.RiscoAmbiente.NEBLINA:
                    return true;
                case (long)Constantes.RiscoAmbiente.GASES:
                    return true;
                case (long)Constantes.RiscoAmbiente.VAPORES:
                    return true;
                case (long)Constantes.RiscoAmbiente.BACTERIAS:
                    return true;
                case (long)Constantes.RiscoAmbiente.FUNGOS:
                    return true;
                case (long)Constantes.RiscoAmbiente.PARASITAS:
                    return true;
                case (long)Constantes.RiscoAmbiente.BACILOS:
                    return true;
                case (long)Constantes.RiscoAmbiente.VIRUS:
                    return true;
                case (long)Constantes.RiscoAmbiente.LEVANTAMENTO_PESO:
                    return true;
                case (long)Constantes.RiscoAmbiente.RITMO_EXCESSIVO:
                    return true;
                case (long)Constantes.RiscoAmbiente.MONOTONIA:
                    return true;
                case (long)Constantes.RiscoAmbiente.REPETITIVIDADE:
                    return true;
                case (long)Constantes.RiscoAmbiente.POSICAO:
                    return true;
                case (long)Constantes.RiscoAmbiente.QUEDA:
                    return true;
                default:
                    return false;
            }
        }

        public bool RiscosAmbienteTerceiraPagina(RISCO_INVENTARIO_AMBIENTE itemRisco)
        {
            if (itemRisco.CodRiscoAmbiente == (long)Constantes.RiscoAmbiente.CHOQUE_ELETRICO)
            {
                return true;
            }
            if (itemRisco.CodRiscoAmbiente == (long)Constantes.RiscoAmbiente.ILUMINACAO)
            {
                return true;
            }
            if (itemRisco.CodRiscoAmbiente == (long)Constantes.RiscoAmbiente.CHOQUE_MECANICO)
            {
                return true;
            }
            if (itemRisco.CodRiscoAmbiente == (long)Constantes.RiscoAmbiente.INCENDIO)
            {
                return true;
            }
            if (itemRisco.CodRiscoAmbiente == (long)Constantes.RiscoAmbiente.EXPLOSAO)
            {
                return true;
            }
            if (itemRisco.CodRiscoAmbiente == (long)Constantes.RiscoAmbiente.SOTERRAMENTO)
            {
                return true;
            }
            return false;
        }

        public ResultadoGeracao GerarApr(DadosAprModelo dadosAprModelo, APR apr = null)
        {
            try
            {
                ResultadoGeracao resultadoGeracao = new ResultadoGeracao();
                var caminhoArquivoApr = GerarArquivoApr(dadosAprModelo.OrdemManutencao);
                XLWorkbook workbook = CacheFolhaAprNegocio.ObterFolhaApr();
                int qtdFolhaComplementar = 0;
                int qtdItensPorFolha = 0;
                string folhaAtual = "";
                string caminhoDiretorioApr = Path.GetDirectoryName(caminhoArquivoApr);
                XLWorkbook folhaComplementar = GerarNovaFolhaComplementar(qtdFolhaComplementar, caminhoDiretorioApr);
                PreencherRelacaoColaboradores(workbook.Worksheet(1), folhaComplementar, apr);
                IEnumerable<INVENTARIO_ATIVIDADE> result;

                workbook.Style.Font.FontName = "Arial";
                workbook.Style.Font.FontSize = 9;
                workbook.Style.Font.FontColor = XLColor.Black;
                workbook.Style.Fill.BackgroundColor = XLColor.White;

                int maiorRiscoInventarioAmbiente = 0;
                int maiorRiscoIventarioAtividade = 0;
                int maiorRiscoGeral = int.MinValue;

                //Armazena todos os riscos da APR
                List<RISCO_INVENTARIO_AMBIENTE> riscosInvAmbiente = new List<RISCO_INVENTARIO_AMBIENTE>();
                //List<RISCO> riscosInvAmbiente = new List<RISCO>();
                List<RISCO_INVENTARIO_ATIVIDADE> riscosInvAtividade = new List<RISCO_INVENTARIO_ATIVIDADE>();
                List<NR_INVENTARIO_AMBIENTE> todasNRs = new List<NR_INVENTARIO_AMBIENTE>();

                apr = aprPersistencia.PesquisarPorOrdemManutencao(dadosAprModelo.OrdemManutencao);

                foreach (var operacao in dadosAprModelo.Operacoes)
                {
                    INVENTARIO_AMBIENTE invAmbiente = null;
                    LOCAL_INSTALACAO_INVENTARIO_AMBIENTE_HISTORICO_APR invAmbienteHistorico = null;

                    INVENTARIO_ATIVIDADE invAtividade = null;
                    List<LOCAL_INSTALACAO_INVENTARIO_ATIVIDADE_HISTORICO_APR> invAtividadeHistorico = null;

                    var liOperacao = localInstalacaoPersistencia.ListarLocalInstalacaoPorId(operacao.CodLocalInstalacao);

                    if (liOperacao == null)
                        adicionaErroLista(resultadoGeracao.erros, 1, $"Não foi encontrado um local de instalação com código {operacao.CodLocalInstalacao}.");
                    else
                    {
                        if (apr.Ativo == false)
                        {
                            invAmbienteHistorico = aprPersistencia.PesquisarAprInventarioAmbiente(liOperacao.CodLocalInstalacao, apr.CodAPR);

                            if (invAmbienteHistorico != null)
                            {
                                invAmbiente = inventariosAmbientePersistencia.ListarInventarioAmbientePorId(invAmbienteHistorico.CodInventarioAmbiente, null, false);
                            }
                            else
                            {
                                invAmbiente = inventariosAmbientePersistencia.ListarInventarioAmbientePorLI(liOperacao.CodLocalInstalacao);
                            }
                        }
                        else
                        {
                            invAmbiente = inventariosAmbientePersistencia.ListarInventarioAmbientePorLI(liOperacao.CodLocalInstalacao);
                        }

                        todasNRs.AddRange(invAmbiente.NR_INVENTARIO_AMBIENTE);
                        riscosInvAmbiente.AddRange(invAmbiente.RISCO_INVENTARIO_AMBIENTE);

                        FiltroInventarioAtividadeModelo filtro = new FiltroInventarioAtividadeModelo();
                        var atividadePadrao = atividadePadraoPersistencia.ListarAtividadePorId(operacao.CodAtvPadrao);
                        var disciplina = disciplinaPersistencia.ListarDisciplinaPorId(operacao.CodDisciplina);

                        if (atividadePadrao == null)
                            adicionaErroLista(resultadoGeracao.erros, 2, $"Não foi encontrada uma atividade padrão com código {operacao.CodAtvPadrao}.");

                        else if (disciplina == null)
                            adicionaErroLista(resultadoGeracao.erros, 3, $"Não foi encontrada uma disciplina com código {operacao.CodDisciplina}.");

                        else
                        {
                            List<long> codLocalInstalacao = new List<long>();
                            codLocalInstalacao.Add(liOperacao.CodLocalInstalacao);
                            filtro.Locais = codLocalInstalacao;
                            filtro.CodAtividade = atividadePadrao.CodAtividadePadrao;
                            filtro.CodDisciplina = disciplina.CodDisciplina;
                            filtro.CodPerfilCatalogo = 0;
                            filtro.CodPeso = 0;
                            filtro.CodSeveridade = 0;
                            filtro.Riscos = new List<long>();

                            if (apr.Ativo == false)
                            {
                                invAtividadeHistorico = aprPersistencia.PesquisarAprInventarioAtividade(liOperacao.CodLocalInstalacao, apr.CodAPR);

                                if (invAtividadeHistorico != null && invAtividadeHistorico.Count > 0)
                                {
                                    foreach (var itemInvAtvHistorico in invAtividadeHistorico)
                                    {
                                        var inventarioAtividadeDesativado = inventariosAtividadePersistencia.ListarInventarioAtividadePorId(itemInvAtvHistorico.CodInventarioAtividade, false);

                                        var filtroPorAtvEDisciplina = inventariosAtividadePersistencia.ListarInventarioAtividadePorAtividadeEDisciplina(inventarioAtividadeDesativado.CodAtividade, inventarioAtividadeDesativado.CodDisciplina);

                                        if (filtroPorAtvEDisciplina != null)
                                        {
                                            invAtividade = inventariosAtividadePersistencia.ListarInventarioAtividadePorId(filtroPorAtvEDisciplina.CodInventarioAtividade, false);
                                            break;
                                        }
                                    }
                                }
                            }

                            if (invAtividade == null)
                            {
                                result = inventariosAtividadePersistencia.ListarInventarioAtividade(filtro);
                                if (result.Count() == 0 || result == null)
                                {
                                    adicionaErroLista(resultadoGeracao.erros, 4, $"Não foi encontrado nenhum inventário de atividade.");
                                }
                                else if (result.Count() > 1)
                                {
                                    adicionaErroLista(resultadoGeracao.erros, 4, $"Foram encontrados mais de um inventário de atividade para mesma atividade/disciplina/local de instalação.");
                                }
                                else
                                {
                                    invAtividade = result.First();
                                }
                            }

                            riscosInvAtividade.AddRange(invAtividade.RISCO_INVENTARIO_ATIVIDADE);
                        }

                        if (maiorRiscoGeral < invAmbiente.RiscoGeral * invAtividade.RiscoGeral)
                        {
                            maiorRiscoGeral = invAmbiente.RiscoGeral * invAtividade.RiscoGeral;
                            maiorRiscoInventarioAmbiente = invAmbiente.RiscoGeral;
                            maiorRiscoIventarioAtividade = invAtividade.RiscoGeral;
                        }
                    }

                }

                dadosAprModelo.NumeroSerie = apr.NumeroSerie;
                dadosAprModelo.LocalInstalacao = apr.LocalInstalacao;
                dadosAprModelo.OrdemManutencao = apr.OrdemManutencao;

                PreencherPaginasPlanilhaAPR(ref workbook, ref folhaComplementar, ref qtdItensPorFolha, ref qtdFolhaComplementar, ref folhaAtual,
                     riscosInvAmbiente, riscosInvAtividade, todasNRs, dadosAprModelo, caminhoDiretorioApr);

                PreencherRiscosGerais(workbook, maiorRiscoInventarioAmbiente, maiorRiscoIventarioAtividade);
                SalvarFolhasComplementares(folhaComplementar, apr.NumeroSerie);
                DeletarFolhaComplementarNaoUtilizada(folhaComplementar, caminhoDiretorioApr);
                MarcarOpcaoFolhaComplementar(workbook, folhaComplementar);
                CorrigirLarguraCelulasFolhaApr(workbook);
                workbook.SaveAs(caminhoArquivoApr);

                return resultadoGeracao;
            }
            catch (Exception ex)
            {

                throw new Exception("Ocorreu um erro ao gerar a APR : " + ex.Message);
            }
        }

        private void DeletarFolhaComplementarNaoUtilizada(XLWorkbook folhaComplementar, string caminhoDiretorioApr)
        {
            try
            {
                var caminhoFolhaComplementar = $@"{caminhoDiretorioApr}\FolhaComplementarGerada.xlsx";
                if (folhaComplementar != null && !folhaComplementar.Worksheets.Any())
                {
                    folhaComplementar.Dispose();
                    File.Delete(caminhoFolhaComplementar);
                }
            }
            catch
            {
                throw;
            }
        }

        private void PreencherRiscosGerais(XLWorkbook workbook,int maiorRiscoAmbiente,int maiorRiscoAtividade)
        {
            var planilha = workbook.Worksheet(1);
            // risco ambiente
            planilha.Cell("C114").Style.Font.FontSize = 12;
            planilha.Cell("C114").Style.Font.FontColor = ObterCorBaseadaNoRiscoInvAmbienteAtividade(maiorRiscoAmbiente);
            planilha.Cell("C114").Value = maiorRiscoAmbiente.ToString();

            // risco atividade
            planilha.Cell("C120").Style.Font.FontSize = 12;
            planilha.Cell("C120").Style.Font.FontColor = ObterCorBaseadaNoRiscoInvAmbienteAtividade(maiorRiscoAtividade);
            planilha.Cell("C120").Value = maiorRiscoAtividade.ToString();

            var riscoApr = CalcularRiscoApr(maiorRiscoAmbiente, maiorRiscoAtividade);
            // risco geral
            planilha.Cell("Q114").Style.Font.FontSize = 12;
            planilha.Cell("Q114").Style.Font.FontColor = ObterCorBaseadaNoRisco(riscoApr);
            planilha.Cell("Q114").Value = riscoApr.ToString();
        }


        public ResultadoGeracao PreencherPaginasPlanilhaAPR(ref XLWorkbook workbook, ref XLWorkbook folhaComplementar,
            ref int qtdItensPorFolha, ref int qtdFolhaComplementar, ref string folhaAtual,
            List<RISCO_INVENTARIO_AMBIENTE> riscosInvAmbiente, List<RISCO_INVENTARIO_ATIVIDADE> riscosInvAtividade,
            List<NR_INVENTARIO_AMBIENTE> listaNRs, DadosAprModelo dadosAprModelo, string caminhoDiretorioApr)
        {
            ResultadoGeracao resultadoGeracao = new ResultadoGeracao();
            try
            {
                GerarPrimeiraFolhaAPR(listaNRs, dadosAprModelo.LocalInstalacao, dadosAprModelo.NumeroSerie, workbook, dadosAprModelo);

                GerarSegundaETerceiraFolha(workbook, riscosInvAmbiente, riscosInvAtividade, folhaComplementar, ref qtdItensPorFolha,
                    dadosAprModelo.NumeroSerie, ref folhaAtual, ref qtdFolhaComplementar, caminhoDiretorioApr);
                return resultadoGeracao;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public string AdicionarNovaFolhaComplementar(XLWorkbook workbookFolha, int folhaAtual)
        {
            string sheet;
            try
            {
                sheet = $"{Constantes.NOME_FOLHA_COMPLEMENTAR_FRENTE}{folhaAtual}";
                var planilhaOriginal = workbookFolha.Worksheets.Worksheet("Frente").CopyTo(sheet);
            }
            catch (Exception)
            {
                throw;
            }

            return sheet;
        }

        public ResultadoGeracao PreencherCabecalhoFolhaComplementar(string folhaAtual, XLWorkbook workbookFolha, string numeroSerie, XLWorkbook workbook, int qtdFolhaComplementar)
        {
            ResultadoGeracao resultadoGeracao = new ResultadoGeracao();
            try
            {
                var workbookApr = workbook.Worksheets.Worksheet("Sheet1");
                var planilha = workbookFolha.Worksheets.Worksheet(folhaAtual);
                planilha.Cell("BD3").Value = numeroSerie;
                planilha.Cell("B6").Value = workbookApr.Cell("C26").Value.ToString();
            }
            catch(Exception){
                throw;
            }
            return resultadoGeracao;
        }


        public ResultadoGeracao PreencherFolhaComplementar(string folhaAtual,string risco, string fonteGeradora, string epi, string procApCM, 
            int qtdItensPorFolha, XLWorkbook workbookFolha)
        {
            ResultadoGeracao resultadoGeracao = new ResultadoGeracao();

            try
            {
                var altura = Constantes.ALTURA_CELULA_FOLHA_COMPLEMENTAR;
                var planilha = workbookFolha.Worksheets.Worksheet(folhaAtual);
                int linha = qtdItensPorFolha + 9;
                var celulaBase = planilha.Range("B9:CA9");

                planilha.Row(linha).Height = altura;
                planilha.Cell(linha, 2).Style.Font.FontSize = Constantes.TAMANHO_FONTE_FOLHA_COMPLEMENTAR;
                planilha.Cell(linha,2).Value = celulaBase;
                planilha.Cell($"B{linha}").Style.Font.FontSize = Constantes.TAMANHO_FONTE_FOLHA_COMPLEMENTAR;
                planilha.Cell($"B{linha}").Value = qtdItensPorFolha;
                planilha.Cell($"C{linha}").Style.Font.FontSize = Constantes.TAMANHO_FONTE_FOLHA_COMPLEMENTAR;
                planilha.Cell($"C{linha}").Value = risco;
                planilha.Cell($"S{linha}").Style.Font.FontSize = Constantes.TAMANHO_FONTE_FOLHA_COMPLEMENTAR;
                planilha.Cell($"S{linha}").Value = fonteGeradora;
                planilha.Cell($"AP{linha}").Style.Font.FontSize = Constantes.TAMANHO_FONTE_FOLHA_COMPLEMENTAR;
                planilha.Cell($"AP{linha}").Value = epi;
                planilha.Cell($"BB{linha}").Style.Font.FontSize = Constantes.TAMANHO_FONTE_FOLHA_COMPLEMENTAR;
                planilha.Cell($"BB{linha}").Value = procApCM;


                double maiorAltura = -1;

                double alturaEPI = 0;
                double alturaProc = 0;
                double alturaFonteGer = 0;

                //Verifica e retorna o maior número de caracteres dentre os campos do risco
                var maiorNumCaracteres = this.VerificaMaiorCampoEntreValoresRisco(epi, procApCM, fonteGeradora);

                //Verifica qual é a maior altura entre os valores EPI, Procedimento e Fonte
                alturaEPI = Constantes.CalcularNumeroLinhasPorCaracteres(altura, epi.Length, Constantes.LIMITE_CARACTERES_POR_LINHA_EPI);
                alturaProc = Constantes.CalcularNumeroLinhasPorCaracteres(altura, procApCM.Length, Constantes.LIMITE_CARACTERES_PROCEDIMENTOS_APLICAVEIS);
                alturaFonteGer = Constantes.CalcularNumeroLinhasPorCaracteres(altura, fonteGeradora.Length, Constantes.LIMITE_CARACTERES_POR_LINHA);

                //alturaCalculada = Constantes.CalcularNumeroLinhasPorCaracteres(altura, maiorNumCaracteres, Constantes.LIMITE_CARACTERES_POR_LINHA_EPI);

                if (alturaEPI > maiorAltura)
                    maiorAltura = alturaEPI;
                if (alturaProc > maiorAltura)
                    maiorAltura = alturaProc;
                if (alturaFonteGer > maiorAltura)
                    maiorAltura = alturaFonteGer;

                //planilha.Row(linha).Height = alturaCalculada;

                planilha.Row(linha).Height = maiorAltura;

                return resultadoGeracao;

            }
            catch (Exception)
            {
                throw;
            }
        }

        private int VerificaMaiorCampoEntreValoresRisco(string epi, string procAplicavel, string fonteGeradora)
        {
            int maiorValor = -1;

            if (epi.Length > maiorValor)
                maiorValor = epi.Length;
            
            if(procAplicavel.Length > maiorValor)
                maiorValor = procAplicavel.Length;

            if (fonteGeradora.Length > maiorValor)
                maiorValor = fonteGeradora.Length;

            return maiorValor;
        }

        public ResultadoGeracao GerarAPRComNumeroSerie(XLWorkbook workbook, string numeroSerie)
        {
            ResultadoGeracao resultadoGeracao = new ResultadoGeracao();

            try
            {
                var folha1 = workbook.Worksheets.Worksheet("Sheet1");
                var folha2 = workbook.Worksheets.Worksheet("Sheet2");
                var folha3 = workbook.Worksheets.Worksheet("Sheet3");
                workbook.Style.Font.FontName = "Arial";
                workbook.Style.Font.FontSize = 8;
                workbook.Style.Font.FontColor = XLColor.Black;
                workbook.Style.Fill.BackgroundColor = XLColor.White;

                folha1.Cell("BG4").Value = numeroSerie;
                folha2.Cell("BG4").Value = numeroSerie;
                folha3.Cell("BG4").Value = numeroSerie;
            }
            catch (Exception)
            {
                throw;
            }

            return resultadoGeracao;
        }

        public ResultadoGeracao GerarAPREmBrancoComNumeroSerie()
        {
            ResultadoGeracao resultadoGeracao = new ResultadoGeracao();

            using (var entities = new DB_APRPTEntities())
            {
                using (var transaction = entities.Database.BeginTransaction(System.Data.IsolationLevel.Serializable))
                {
                    try
                    {
                        //Obtém os diretórios e copia o arquivo
                        string caminhoModelo = ArquivoDiretorioUtils.ObterDiretorioModelo();
                        var caminhoAprGerada = ArquivoDiretorioUtils.ObterDiretorioApr();
                        caminhoAprGerada = ArquivoDiretorioUtils.ConstruirObterDiretorioData(caminhoAprGerada);

                        string caminhoCompletoAPRModelo = $"{caminhoModelo}LayoutApr.xlsx";
                        string caminhoCompletoAPRFinal = $"{caminhoAprGerada}\\AprGerada_{DateTime.Now.ToString("hh_mm_ss_ff")}.xlsx";
                        ArquivoDiretorioUtils.CopiarArquivo(caminhoCompletoAPRModelo, caminhoCompletoAPRFinal);

                        //Obtém o númerio de série, insere e gera a APR
                        var workbook = CacheFolhaAprNegocio.ObterFolhaApr();
                        var numeroSerie = ((APR)aprPersistencia.InserirSomenteComNumeroSerie(true, entities)).NumeroSerie;
                        GerarAPRComNumeroSerie(workbook, numeroSerie);
                        workbook.SaveAs(caminhoCompletoAPRFinal);

                        //Gera o arquivo PDF no caminho especificado
                        ConversaoPdfUtils.GerarArquivoPdf(caminhoCompletoAPRFinal);
                        var caminhoArquivoPdf = $"{caminhoCompletoAPRFinal.Split('.')[0]}.pdf";
                        resultadoGeracao.caminhoFinal = caminhoArquivoPdf;

                        transaction.Commit();

                        return resultadoGeracao;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }
        }
        public RetornoBloqueioAgrupadoModelo GerarMapaBloqueioAgrupado(List <string> listaOrdemManutencao)
        {

            List<string> ordensManutencaoInvalidas = ListarOrdensDeManutencaoInvalidas(listaOrdemManutencao);
            var ordensManutencaoValidas = listaOrdemManutencao.Except(ordensManutencaoInvalidas).ToList();
            RetornoBloqueioAgrupadoModelo retornoBloqueioAgrupado = new RetornoBloqueioAgrupadoModelo();

            if (ordensManutencaoValidas.Any()== false)
            {
                retornoBloqueioAgrupado.OrdensInvalidas = listaOrdemManutencao;
                retornoBloqueioAgrupado._b64 = "";
                return retornoBloqueioAgrupado;
            }
            BloqueioNegocio bloqueioNegocio = new BloqueioNegocio(bloqueioPersistencia, aprPersistencia);
            DadosMapaBloqueioAprModelo dadosMapaBloqueioApr = new DadosMapaBloqueioAprModelo();
            dadosMapaBloqueioApr.OrdemManutencao = ordensManutencaoValidas;
            var listaArquivosGerados = bloqueioNegocio.ListarBloqueioPorListaLIPorArea(dadosMapaBloqueioApr);
            var listaPdfParaAgrupar = ConversaoPdfUtils.GerarPdfeAgruparArquivos(listaArquivosGerados);
            var caminhoDiretorio = listaPdfParaAgrupar.First();
            caminhoDiretorio = Path.GetDirectoryName(caminhoDiretorio);
            var caminhoPdfAgrupado = ConversaoPdfUtils.AgruparPdfs(listaPdfParaAgrupar, caminhoDiretorio);
            ConversaoPdfUtils.DeletarPdfsTemporarios(listaPdfParaAgrupar);
            retornoBloqueioAgrupado.OrdensInvalidas = ordensManutencaoInvalidas;
            retornoBloqueioAgrupado._b64 = Convert.ToBase64String(File.ReadAllBytes(caminhoPdfAgrupado));
            return retornoBloqueioAgrupado;
        }

        private List<string> ListarOrdensDeManutencaoInvalidas(List<string> listaOrdemManutencao)
        {
            List<string> ordensInvalidas = new List<string>();
            foreach ( var ordemManutencao in listaOrdemManutencao)
            {
                try
                {
                    aprPersistencia.PesquisarPorOrdemManutencao(ordemManutencao);
                }
                catch
                {
                    ordensInvalidas.Add(ordemManutencao);
                }
            }
            return ordensInvalidas;
        }
        private string GerarArquivoApr(string ordemManutencao)
        {
            var apr = aprPersistencia.PesquisarPorOrdemManutencao(ordemManutencao);
            string diretorioModelo = ArquivoDiretorioUtils.ObterDiretorioModelo();
            string diretorioApr = ArquivoDiretorioUtils.ObterDiretorioApr();
            var caminhoAprGerada = ArquivoDiretorioUtils.ConstruirObterDiretorioData(diretorioApr);
            caminhoAprGerada = $"{caminhoAprGerada}{apr.NumeroSerie}/";
            caminhoAprGerada = ArquivoDiretorioUtils.ConstruirDiretorio(caminhoAprGerada);
            string caminhoCompletoAPRModelo = $"{diretorioModelo}LayoutApr.xlsx";
            string caminhoCompletoAPRFinal = $"{caminhoAprGerada}AprGerada_{DateTime.Now.ToString("hh_mm_ss_ff")}.xlsx";
            ArquivoDiretorioUtils.CopiarArquivo(caminhoCompletoAPRModelo, caminhoCompletoAPRFinal);
            return caminhoCompletoAPRFinal;
        }

        private void MarcarOpcaoFolhaComplementar(XLWorkbook workbook, XLWorkbook folhaComplementar)
        {
            var folhaApr1 = workbook.Worksheets.Worksheet("Sheet1");
            var folhaApr2 = workbook.Worksheets.Worksheet("Sheet2");
            var folhaApr3 = workbook.Worksheets.Worksheet("Sheet3");
            string opcaoX = "x";

            if (ExisteFolhaComplementarNoWorkbook(folhaComplementar) == false)
            {
                folhaApr1.Cell("CF4").Value = opcaoX;
                folhaApr2.Cell("CF4").Value = opcaoX;
                folhaApr3.Cell("CF4").Value = opcaoX;
            }
            else
            {
                folhaApr1.Cell("BZ4").Value = opcaoX;
                folhaApr2.Cell("BZ4").Value = opcaoX;
                folhaApr3.Cell("BZ4").Value = opcaoX;
            }

        }


        private bool ExisteFolhaComplementarNoWorkbook (XLWorkbook folhaComplementar)
        {
            if (folhaComplementar == null)
            {
                return false;
            }
            else
            {
                foreach (var folhasPlanilha in folhaComplementar.Worksheets)
                {
                    if (folhasPlanilha.Name.Contains("Frente"))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private void PreencherIndiceFolhasComplementaresGeradas (XLWorkbook folhaComplementar)
        {
            var folhaAtual = 1;
            if (folhaComplementar != null)
            {
                var quantidadeFolhas = folhaComplementar.Worksheets.Where(x => x.Name.Contains("Frente")).ToList().Count();
                foreach (var folha in folhaComplementar.Worksheets)
                {
                    if (folha.Name.Contains("Frente"))
                    {
                        folha.Cell("BT2").Value = $"Folha:\n{folhaAtual}/{quantidadeFolhas}";
                        folhaAtual++;
                    }
                }
            }
        }

        private XLColor ObterCorBaseadaNoRisco(long valorRisco)
        {
            if (valorRisco < (long) Constantes.ValorRisco.RISCO_BAIXO)
            {
                return XLColor.Green;
            }

            if (valorRisco >= (long) Constantes.ValorRisco.RISCO_BAIXO && valorRisco < (long)Constantes.ValorRisco.RISCO_MEDIO)
            {
                return XLColor.Yellow;
            }

            if (valorRisco >= (long) Constantes.ValorRisco.RISCO_MEDIO && valorRisco < (long) Constantes.ValorRisco.RISCO_ALTO)
            {
                return XLColor.Red ;
            }
            return XLColor.Black;
        }

        private XLColor ObterCorBaseadaNoRiscoInvAmbienteAtividade(long valorRisco)
        {
            if ( valorRisco == 1)
            {
                return XLColor.Green;
            }
            if ( valorRisco == 2)
            {
                return XLColor.Yellow;
            }
            if (valorRisco == 3)
            {
                return XLColor.Red;
            }
            return XLColor.Black;
        }

        private void PreencherRelacaoColaboradores (IXLWorksheet folhaApr,XLWorkbook folhaComplementar ,APR apr)
        {
            var planilhaFolhaComplementar = folhaComplementar;
            var numeroDeSerie = "";
            if (apr != null)
            {
                numeroDeSerie = apr.NumeroSerie;
                var listaCodigoExecutantesAtivos = apr.EXECUTANTE_APR.Where(x=> x.Ativo).Select(x=>x.CodPessoa).ToList();
                var listaCodigoAprovadoresAtivos = apr.APROVADOR_APR.Where(x => x.Ativo).Select(x => x.CodPessoa).ToList();
                var quantidadeTotalDeFuncionarios = listaCodigoExecutantesAtivos.Count() + listaCodigoAprovadoresAtivos.Count();
                if (quantidadeTotalDeFuncionarios <= 0)
                {
                    planilhaFolhaComplementar.Worksheet("Verso").Delete();
                    return;
                }

                if (quantidadeTotalDeFuncionarios <= Constantes.QUANTIDADE_FUNCIONARIOS_FOLHA_APR)
                {
                    PreencherRelacaoColaboradoresFolhaComplementar(folhaApr, listaCodigoExecutantesAtivos, listaCodigoAprovadoresAtivos);
                }
                else
                {
                    var quantidadeItensPreenchidos = 0;
                    var quantidadeCodigosExecutantes = listaCodigoExecutantesAtivos.Count();

                    if (quantidadeCodigosExecutantes <= Constantes.QUANTIDADE_FUNCIONARIOS_FOLHA_APR)
                    {
                        var listaExecutantesAtivos = listaCodigoExecutantesAtivos.GetRange(0,listaCodigoExecutantesAtivos.Count());
                        PreencherRelacaoColaboradoresFolhaComplementar(folhaApr, listaExecutantesAtivos, new List<long>());
                        quantidadeItensPreenchidos = listaCodigoExecutantesAtivos.Count();
                        listaCodigoExecutantesAtivos.RemoveRange(0, quantidadeItensPreenchidos);
                    }

                    else
                    {
                        var listaExecutantesAtivos = listaCodigoExecutantesAtivos.GetRange(0,12);
                        PreencherRelacaoColaboradoresFolhaComplementar(folhaApr, listaExecutantesAtivos, new List<long>());
                        quantidadeItensPreenchidos = Constantes.QUANTIDADE_FUNCIONARIOS_FOLHA_APR;
                        listaCodigoExecutantesAtivos.RemoveRange(0, 12);
                    }

                    if (quantidadeItensPreenchidos < Constantes.QUANTIDADE_FUNCIONARIOS_FOLHA_APR)
                    {
                        var quantidadeRestante = Constantes.QUANTIDADE_FUNCIONARIOS_FOLHA_APR - quantidadeItensPreenchidos;
                        var listaAprovadoresAtivos = listaCodigoAprovadoresAtivos.GetRange(0, quantidadeRestante);
                        PreencherRelacaoColaboradoresFolhaComplementar(folhaApr,new List<long>(),listaAprovadoresAtivos);
                        listaCodigoAprovadoresAtivos.RemoveRange(0, quantidadeRestante);
                    }

                    quantidadeTotalDeFuncionarios = listaCodigoExecutantesAtivos.Count() + listaCodigoAprovadoresAtivos.Count();
                    if (quantidadeTotalDeFuncionarios > 0)
                    {
                        int quantidadePaginas = quantidadeTotalDeFuncionarios / Constantes.QUANTIDADE_FUNCIONARIOS_FOLHA_COMPLEMENTAR;
                        quantidadePaginas = quantidadePaginas + (quantidadeTotalDeFuncionarios % Constantes.QUANTIDADE_FUNCIONARIOS_FOLHA_COMPLEMENTAR > 0 ? 1 : 0);
                        var linhaInicial = 7;
                        var paginaAtual = 1;
                        var folhaAtual = CriarFolhaComplementarDeFuncionarios(planilhaFolhaComplementar, paginaAtual, numeroDeSerie, quantidadePaginas);


                        foreach (var codigoExecutante in listaCodigoExecutantesAtivos)
                        {
                            if (linhaInicial == Constantes.POSICAO_FINAL_FUNCIONARIO_FOLHA_COMPLEMENTAR)
                            {
                                paginaAtual++;
                                linhaInicial = Constantes.POSICAO_INICIAL_FUNCIONARIO_FOLHA_COMPLEMENTAR; ;
                                folhaAtual = CriarFolhaComplementarDeFuncionarios(planilhaFolhaComplementar, paginaAtual, numeroDeSerie, quantidadePaginas);
                            }

                            var pessoa = this.pessoaPersistencia.ListarPorCodigo(codigoExecutante);
                            var matriculaEmpresa = $"{pessoa.Matricula}/{pessoa.Empresa}";
                            PreencherRelacaoColaboradoresEnvolvidosFolhaComplementar(folhaAtual, pessoa.Nome, "Aprovador", matriculaEmpresa, linhaInicial,18);
                            linhaInicial++;
                        }

                        foreach (var codigoAprovador in listaCodigoAprovadoresAtivos)
                        {
                            if (linhaInicial == Constantes.POSICAO_FINAL_FUNCIONARIO_FOLHA_COMPLEMENTAR)
                            {
                                paginaAtual++;
                                linhaInicial = Constantes.POSICAO_INICIAL_FUNCIONARIO_FOLHA_COMPLEMENTAR;
                                folhaAtual = CriarFolhaComplementarDeFuncionarios(planilhaFolhaComplementar, paginaAtual, numeroDeSerie, quantidadePaginas);
                            }

                             var pessoa = this.pessoaPersistencia.ListarPorCodigo(codigoAprovador);
                             var matriculaEmpresa = $"{pessoa.Matricula}/{pessoa.Empresa}";
                            PreencherRelacaoColaboradoresEnvolvidosFolhaComplementar(folhaAtual, pessoa.Nome, "Aprovador", matriculaEmpresa,linhaInicial,18);
                            linhaInicial++;
                        }

                    }
                }
            }
            planilhaFolhaComplementar.Worksheet("Verso").Delete();
        }

        private IXLWorksheet CriarFolhaComplementarDeFuncionarios(XLWorkbook planilha,int folhaAtual, string numeroDeSerie, int quantidadePaginas)
        {
            var sheet = $"{Constantes.NOME_FOLHA_COMPLEMENTAR_VERSO}{folhaAtual}";
            var planilhaOriginal = planilha.Worksheets.Worksheet("Verso").CopyTo(sheet);
            var folhaFuncionario = planilha.Worksheet(sheet);
            folhaFuncionario.Cell("BQ2").Value = $"Folha:\n{folhaAtual}/{quantidadePaginas}";
            folhaFuncionario.Cell("AY3").Value = numeroDeSerie;
            return folhaFuncionario;
        }
        private void PreencherRelacaoColaboradoresFolhaComplementar(IXLWorksheet folhaApr, List<long> listaCodigoExecutantes, List<long> listaCodigoAprovadores)
        {
            foreach (var codigoExecutor in listaCodigoExecutantes)
            {

             var pessoa = this.pessoaPersistencia.ListarPorCodigo(codigoExecutor);
             var matriculaEmpresa = $"{pessoa.Matricula}/{pessoa.Empresa}";
             PreencherRelacaoColaboradoresEnvolvidos(folhaApr, pessoa.Nome, "Executor", matriculaEmpresa);
            }

            foreach (var codigoAprovador in listaCodigoAprovadores)
            {

             var pessoa = pessoaPersistencia.ListarPorCodigo(codigoAprovador);
             var matriculaEmpresa = $"{pessoa.Matricula}/{pessoa.Empresa}";
             PreencherRelacaoColaboradoresEnvolvidos(folhaApr, pessoa.Nome, "Aprovador", matriculaEmpresa);
            }
        }
        
        private bool PreencherRelacaoColaboradoresEnvolvidos(IXLWorksheet folhaApr,string nome,string funcao,string matriculaEmpresa )
        {
            
            if (folhaApr.Cell("F41").IsEmpty() && folhaApr.Cell("AP41").IsEmpty() && folhaApr.Cell("BF41").IsEmpty())
            {
                PreencherCelulasColaboradoresPrimeiraFolha(folhaApr, "F41", "AP41", "BF41", nome, funcao, matriculaEmpresa);
                return true; 
            }
            if (folhaApr.Cell("F45").IsEmpty() && folhaApr.Cell("AP45").IsEmpty() && folhaApr.Cell("BF45").IsEmpty())
            {
                PreencherCelulasColaboradoresPrimeiraFolha(folhaApr, "F45", "AP45", "BF45", nome, funcao, matriculaEmpresa);
                return true;
            }

            if (folhaApr.Cell("F49").IsEmpty() && folhaApr.Cell("AP49").IsEmpty() && folhaApr.Cell("BF49").IsEmpty())
            {
                PreencherCelulasColaboradoresPrimeiraFolha(folhaApr, "F49", "AP49", "BF49", nome, funcao, matriculaEmpresa);
                return true;
            }

            if (folhaApr.Cell("F53").IsEmpty() && folhaApr.Cell("AP53").IsEmpty() && folhaApr.Cell("BF53").IsEmpty())
            {
                PreencherCelulasColaboradoresPrimeiraFolha(folhaApr, "F53", "AP53", "BF53", nome, funcao, matriculaEmpresa);
                return true;
            }
            if (folhaApr.Cell("F57").IsEmpty() && folhaApr.Cell("AP57").IsEmpty() && folhaApr.Cell("BF57").IsEmpty())
            {
                PreencherCelulasColaboradoresPrimeiraFolha(folhaApr, "F57", "AP57", "BF57", nome, funcao, matriculaEmpresa);
                return true;
            }
            if (folhaApr.Cell("F61").IsEmpty() && folhaApr.Cell("AP61").IsEmpty() && folhaApr.Cell("BF61").IsEmpty())
            {
                PreencherCelulasColaboradoresPrimeiraFolha(folhaApr, "F61", "AP61", "BF61", nome, funcao, matriculaEmpresa);
                return true;
            }
            if (folhaApr.Cell("F65").IsEmpty() && folhaApr.Cell("AP65").IsEmpty() && folhaApr.Cell("BF65").IsEmpty())
            {
                PreencherCelulasColaboradoresPrimeiraFolha(folhaApr, "F65", "AP65", "BF65", nome, funcao, matriculaEmpresa);
                return true;
            }
            if (folhaApr.Cell("F69").IsEmpty() && folhaApr.Cell("AP69").IsEmpty() && folhaApr.Cell("BF69").IsEmpty())
            {
                PreencherCelulasColaboradoresPrimeiraFolha(folhaApr, "F69", "AP69", "BF69", nome, funcao, matriculaEmpresa);
                return true;
            }
            if (folhaApr.Cell("F73").IsEmpty() && folhaApr.Cell("AP73").IsEmpty() && folhaApr.Cell("BF73").IsEmpty())
            {
                PreencherCelulasColaboradoresPrimeiraFolha(folhaApr, "F73", "AP73", "BF73", nome, funcao, matriculaEmpresa);
                return true;
            }
            if (folhaApr.Cell("F77").IsEmpty() && folhaApr.Cell("AP77").IsEmpty() && folhaApr.Cell("BF77").IsEmpty())
            {
                PreencherCelulasColaboradoresPrimeiraFolha(folhaApr, "F77", "AP77", "BF77", nome, funcao, matriculaEmpresa);
                return true;
            }
            if (folhaApr.Cell("F81").IsEmpty() && folhaApr.Cell("AP81").IsEmpty() && folhaApr.Cell("BF81").IsEmpty())
            {
                PreencherCelulasColaboradoresPrimeiraFolha(folhaApr, "F81", "AP81", "BF81", nome, funcao, matriculaEmpresa);
                return true;
            }
            if (folhaApr.Cell("F85").IsEmpty() && folhaApr.Cell("AP85").IsEmpty() && folhaApr.Cell("BF85").IsEmpty())
            {
                PreencherCelulasColaboradoresPrimeiraFolha(folhaApr,"F85","AP85","BF85",nome,funcao,matriculaEmpresa);
                return true;
            }
            return false;
        }

        private void PreencherRelacaoColaboradoresEnvolvidosFolhaComplementar (IXLWorksheet folhaComplementar, string nome, string funcao, string matriculaEmpresa, int linha, int tamanhoFonte)
        {
            var celulaNome = $"C{linha}";
            var celulaFuncao = $"AE{linha}";
            var celulaMatriculaEmpresa = $"AS{linha}";
            PreencherCelulasColaboradoresPrimeiraFolha(folhaComplementar, celulaNome,celulaFuncao,celulaMatriculaEmpresa, nome, funcao, matriculaEmpresa, tamanhoFonte);
        }
        private void PreencherCelulasColaboradoresPrimeiraFolha(IXLWorksheet primeiraFolha, string celula1, string celula2, string celula3,
            string nome, string funcao, string matriculaEmpresa, int tamanhoFonte = 8)
        {

            primeiraFolha.Cell(celula1).Style.Font.FontColor = XLColor.Black;
            primeiraFolha.Cell(celula1).Style.Font.FontSize = tamanhoFonte;
            primeiraFolha.Cell(celula2).Style.Font.FontColor = XLColor.Black;
            primeiraFolha.Cell(celula2).Style.Font.FontSize = tamanhoFonte;
            primeiraFolha.Cell(celula3).Style.Font.FontColor = XLColor.Black;
            primeiraFolha.Cell(celula3).Style.Font.FontSize = tamanhoFonte;
            primeiraFolha.Cell(celula1).Value = nome;
            primeiraFolha.Cell(celula2).Value = funcao;
            primeiraFolha.Cell(celula3).Value = matriculaEmpresa;
        }


        private void CorrigirLarguraCelulasFolhaApr(XLWorkbook workbook)
        {
            var folha2 = workbook.Worksheet("Sheet2");
            var folha3 = workbook.Worksheet("Sheet3");

            folha2.Column(4).Width = 0.17;
            folha2.Column(5).Width = 0.17;
            folha2.Column(78).Width = 0.17;
            folha2.Column(79).Width = 0.17;
            folha2.Column(84).Width = 0.17;
            folha2.Column(85).Width = 0.17;
            folha3.Column(4).Width = 0.17;
            folha3.Column(5).Width = 0.17;
            folha3.Column(78).Width = 0.17;
            folha3.Column(79).Width = 0.17;
            folha3.Column(84).Width = 0.17;
            folha3.Column(85).Width = 0.17;
        }

        #endregion

        #region Log

        public class ArquivoLog
        {
            public string resultadoString { get; set; }
        }


        public ArquivoLog EscreverLogEmTxt(List<long> codApr)
        {
            try
            {
               
                if (codApr.Any() == false)
                {
                    throw new Exception($"Gentileza informar no mínimo um código de APR para a geração dos logs.");
                }
                string caminhoLogInventario = ConfigurationManager.AppSettings["caminhoLogInventario"];
                string nomeArquivo = "log_" + "_" + DateTime.Now.ToString() + ".txt";
                nomeArquivo = nomeArquivo.Replace("/", "_").Replace(" ", "_").Replace(":", "_");
                string caminhoCompleto = caminhoLogInventario + nomeArquivo;

                string todosLogs = "";

                if (!File.Exists(caminhoLogInventario))
                    File.Create(caminhoCompleto).Close();

                var logApr = ListarLogApr(codApr);

                if (logApr.Any() == false)
                    throw new Exception($"Não foram encontrados logs referentes a APR informada.");

                var listaDeLogsAgrupadosPorCodApr = logApr.GroupBy(x => x.CodApr).Select(u => u.ToList()).ToList();

                int i = 0;
                foreach (var itemAPR in listaDeLogsAgrupadosPorCodApr)
                {
                    foreach (var itemLogAPR in listaDeLogsAgrupadosPorCodApr[i])
                    {
                        var apr = aprPersistencia.PesquisarPorId(itemLogAPR.CodApr);

                        todosLogs = GeraLogApr(itemLogAPR, apr, todosLogs);

                        var entities = new DB_APRPTEntities();

                        if (todosLogs.Contains("INSERÇÃO") && !todosLogs.Contains("EDIÇÃO"))
                        {
                            var logOpApr = entities.LOG_OPERACAO_APR.Where(x => x.CodLogApr == itemLogAPR.CodLogApr).ToList();

                            //var listaOperacoesAprInsercao = apr.OPERACAO_APR.Where(x => x.CodAPR == apr.CodAPR).OrderBy(x => x.CodOperacaoAPR).ToList();

                            if (logOpApr.Any() == false)
                            {
                                throw new Exception($"Não foram encontradas operações para a APR de código {apr.CodAPR}");
                            }
                            foreach (var operacaoApr in logOpApr)
                            {
                                todosLogs = GeraLogOperacaoAprPorLog(operacaoApr, todosLogs);
                            }
                        }

                        else
                        {
                            var logOpApr = entities.LOG_OPERACAO_APR.Where(x => x.CodLogApr == itemLogAPR.CodLogApr).ToList();

                            if (logOpApr.Any() == false)
                            {
                                var listaOperacoesApr = apr.OPERACAO_APR.Where(x => x.CodAPR == apr.CodAPR && x.Ativo).ToList();

                                if(listaOperacoesApr.Any())
                                {
                                    foreach (var operacaoApr in listaOperacoesApr)
                                    {
                                        if(todosLogs.Contains("FIM LOG EDIÇÃO"))
                                        {
                                            todosLogs = todosLogs.Replace("--- FIM LOG EDIÇÃO ---", "");
                                        }
                                        todosLogs = GeraLogOperacaoAprPorOperacao(operacaoApr, todosLogs);
                                    }
                                }
                                else
                                throw new Exception($"Não foram encontradas operações para a APR de código {apr.CodAPR}");

                            }
                            else
                            {
                                foreach (var operacaoApr in logOpApr)
                                {
                                    todosLogs = GeraLogOperacaoAprPorLog(operacaoApr, todosLogs);
                                }
                            }

                        }
                    }
                    i++;
                }

                using (StreamWriter sw = new StreamWriter(caminhoCompleto))
                {
                    sw.Write(todosLogs);
                }

                var result = File.ReadAllBytes(caminhoCompleto);

                var resultado = Convert.ToBase64String(result);

                ArquivoLog arquivoLog = new ArquivoLog();
                arquivoLog.resultadoString = resultado;
                return arquivoLog;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ArquivoLog EscreverLogTodasAPRs()
        {
            try
            {
                List<long> codApr = new List<long>();

                var todosInventarios = aprPersistencia.ListarTodasAPRs();

                if (todosInventarios.Count <= 0)
                    throw new Exception("Não existem APRs cadastrados na base de dados.");

                foreach (var itemInventario in todosInventarios)
                {
                    codApr.Add(itemInventario.CodAPR);
                }

                var resultado = EscreverLogEmTxt(codApr);

                return resultado;
            }

            catch (Exception)
            {
                throw;
            }
        }

        private List<LOG_APR> ListarLogApr(List<long> codApr)
        {
            List<APR> listaApr = new List<APR>();
            List<LOG_APR> listaLogApr = new List<LOG_APR>();

            try
            {
                foreach (var item in codApr)
                {
                    var inventario = aprPersistencia.PesquisarPorId(item);

                    listaApr.Add(inventario);
                }

                foreach (var apr in listaApr)
                {
                    var logApr = logAprPersistencia.ListarLogApr(apr.CodAPR);

                     foreach (var itemLog in logApr)
                     {
                        listaLogApr.Add(itemLog);
                     }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return listaLogApr;

        }
        private string GeraLogApr(LOG_APR logApr,APR apr, string conteudo)
        {
            try
            {
                if (conteudo.Contains($@"Numero de série da APR: {apr.NumeroSerie}") == false)
                {
                    conteudo += $@" {Environment.NewLine}
                                    Numero de série da APR: {apr.NumeroSerie}";
                }

                if (logApr.CodLogTipoOperacao == (long)Constantes.TipoOperacaoLog.INSERCAO)
                {
                    conteudo += $@" {Environment.NewLine}
                            --- INÍCIO LOG INSERÇÃO ---

                            Data da inserção: {logApr.DataAlteracao}
                            Código do usuário que realizou a inserção: {logApr.CodUsuarioModificador}";
                    return conteudo;
                }


                if (logApr.CodLogTipoOperacao == (long)Constantes.TipoOperacaoLog.EDICAO)
                {
                    var listaCodRiscoAprAntigo = logApr.CodRiscoAprAntigo.Split(',');
                    var listaCodRiscoAprNovo = logApr.CodRiscoAprNovo.Split(',');
                    var riscosNovos = new List<string>();
                    var riscosAntigos = new List<string>();
                    foreach (var codRiscoAntigo in listaCodRiscoAprAntigo)
                    {
                        var codRisco = Convert.ToInt64(codRiscoAntigo);
                        var risco = riscoPersistencia.ListarRiscoPorId(codRisco);
                        riscosNovos.Add(risco.Nome);
                    }

                    foreach (var codRiscoNovo in listaCodRiscoAprNovo)
                    {
                        var codRisco = Convert.ToInt64(codRiscoNovo);
                        var risco = riscoPersistencia.ListarRiscoPorId(codRisco);
                        riscosAntigos.Add(risco.Nome);
                    }

                    var listaCodigoAprovadorNovo = logApr.CodAprovadorAprNovo.Split(',');
                    var listaCodigoAprovadorAntigo = logApr.CodAprovadorAprAntigo.Split(',');

                    var listaAprovadoresAntigos = new List<string>();
                    var listaAprovadoresNovos = new List<string>();

                    foreach (var codigoAprovadorAntigo in listaCodigoAprovadorAntigo)
                    {
                        var codAprovador = Convert.ToInt64(codigoAprovadorAntigo);
                        var codPessoaAprovadorAntiga = aprPersistencia.BuscarPessoaPorAprovador(codAprovador, apr.CodAPR);
                        var nomeAprovador = pessoaPersistencia.ListarPorCodigo(codPessoaAprovadorAntiga);
                        listaAprovadoresAntigos.Add(nomeAprovador.Nome);
                    }

                    foreach (var codigoAprovadorNovo in listaCodigoAprovadorNovo)
                    {
                        var codAprovador = Convert.ToInt64(codigoAprovadorNovo);
                        var codPessoaAprovadorNovo = aprPersistencia.BuscarPessoaPorAprovador(codAprovador, apr.CodAPR);
                        var nomeAprovador = pessoaPersistencia.ListarPorCodigo(codPessoaAprovadorNovo);
                        listaAprovadoresNovos.Add(nomeAprovador.Nome);
                    }
                    
                    var listaCodigoExecutanteNovo = logApr.CodExecutanteAprNovo.Split(',');
                    var listaCodigoExecutanteAntigo = logApr.CodExecutanteAprAntigo.Split(',');


                    var listaExecutanteAntigo = new List<string>();
                    var listaExecutanteNovo = new List<string>();

                    foreach (var codigoExecutanteAtivo in listaCodigoExecutanteAntigo)
                    {
                        var codExecutante = Convert.ToInt64(codigoExecutanteAtivo);
                        var codPessoaExecutanteAntiga = aprPersistencia.BuscarPessoaPorExecutante(codExecutante, apr.CodAPR);
                        var nomeExecutor = pessoaPersistencia.ListarPorCodigo(codPessoaExecutanteAntiga);
                        listaExecutanteAntigo.Add(nomeExecutor.Nome);
                    }

                    foreach (var codigoExecutanteNovo in listaCodigoExecutanteNovo)
                    {
                        var codExecutante = Convert.ToInt64(codigoExecutanteNovo);
                        var codPessoaExecutanteNova = aprPersistencia.BuscarPessoaPorExecutante(codExecutante, apr.CodAPR);
                        var nomeExecutor = pessoaPersistencia.ListarPorCodigo(codPessoaExecutanteNova);
                        listaExecutanteNovo.Add(nomeExecutor.Nome);
                    }

                    string statusAprAntigo = "";
                    string statusAprNova = "";
                    if (logApr.CodStatusAprAntigo.HasValue)
                    {
                        statusAprAntigo = Enum.GetName(typeof(TipoCodStatusApr), logApr.CodStatusAprAntigo.Value);
                    }
                    if (logApr.CodStatusAprNovo.HasValue)
                    {
                        statusAprNova = Enum.GetName(typeof(TipoCodStatusApr), logApr.CodStatusAprNovo.Value);
                    }

                    string riscoGeralNovo = "";
                    string riscoGeralAntigo = "";
                    if (logApr.RiscoGeralNovo.HasValue)
                    {
                        riscoGeralNovo = logApr.RiscoGeralNovo.Value.ToString();
                    }
                    if (logApr.RiscoGeralAntigo.HasValue)
                    {
                        riscoGeralAntigo = logApr.RiscoGeralAntigo.Value.ToString();
                    }


                    conteudo += $@" {Environment.NewLine}  
                    --- INÍCIO LOG EDIÇÃO ---
                            
                    Status Apr antigo: {statusAprAntigo}
                    Status Apr nova: {statusAprNova}

                    Ordem de manutenção: {logApr.OrdemManutencao}

                    Descrição da APR antigo: {logApr.DescricaoAntiga}
                    Descrição da nova APR: {logApr.DescricaoNova}
                            
                    Nome aprovador antigo: {string.Join(",", listaAprovadoresAntigos)}
                    Nome aprovador novo: {string.Join(",", listaAprovadoresNovos)}

                    Nome executante antigo: {string.Join(",",listaExecutanteAntigo)}
                    Nome executante novo: {string.Join(",", listaExecutanteNovo)}
                            
                    Risco geral antigo: {riscoGeralAntigo}
                    Risco geral novo: {riscoGeralNovo}

                    Riscos antigos: {string.Join(",",riscosAntigos)}
                    Riscos novos: {string.Join(",", riscosNovos)}

                    Código do usuário que realizou a edição: {logApr.CodUsuarioModificador}";
                }

                if (logApr.CodLogTipoOperacao == (long)Constantes.TipoOperacaoLog.DELECAO)
                {
                    conteudo += $@" {Environment.NewLine} 
                    --- INÍCIO LOG EXCLUSÃO ---

                    Data da exclusão: {logApr.DataAlteracao}
                    Código do usuário que realizou a exclusão: {logApr.CodUsuarioModificador}

                    --- FIM LOG EXCLUSÃO --- ";
                    return conteudo;
                }

                return conteudo;
            }
            catch (Exception)
            {
                throw;
            }
        }



        private string GeraLogOperacaoAprPorLog(LOG_OPERACAO_APR operacaoApr,string conteudo)
        {
            string nomeLocalInstalacao = "";
            string nomeDisciplina = "";
            string nomeAtividadePadrao = "";

            if (operacaoApr.CodLI.HasValue)
            {
                nomeLocalInstalacao = localInstalacaoPersistencia.ListarLocalInstalacaoPorId(operacaoApr.CodLI.Value).Nome;
            }

            if (operacaoApr.CodAtvPadrao.HasValue)
            {
                nomeAtividadePadrao = atividadePadraoPersistencia.ListarAtividadePorId(operacaoApr.CodAtvPadrao.Value).Nome;
            }

            if (operacaoApr.CodDisciplina.HasValue)
            {
                nomeDisciplina = disciplinaPersistencia.ListarDisciplinaPorId(operacaoApr.CodDisciplina.Value).Nome;
            }
            try
            {
                conteudo += $@" {Environment.NewLine} 
                            Dados da operação atual:
                            Código: {operacaoApr.Codigo}
                            Descrição: {operacaoApr.Descricao}
                            Status APR: {operacaoApr.CodStatusApr}
                            Disciplina: {nomeDisciplina}
                            Atividade: {nomeAtividadePadrao}
                            Local instalação: {nomeLocalInstalacao}

                            --- FIM LOG INSERÇÃO ---
                            ";
                return conteudo;
            }
            catch(Exception)
            {
                throw;
            }
        }

        private string GeraLogOperacaoAprPorOperacao(OPERACAO_APR operacaoApr, string conteudo)
        {
            string nomeLocalInstalacao = "";
            string nomeDisciplina = "";
            string nomeAtividadePadrao = "";

            if (operacaoApr.CodLI.HasValue)
            {
                nomeLocalInstalacao = localInstalacaoPersistencia.ListarLocalInstalacaoPorId(operacaoApr.CodLI.Value).Nome;
            }

            if (operacaoApr.CodAtvPadrao.HasValue)
            {
                nomeAtividadePadrao = atividadePadraoPersistencia.ListarAtividadePorId(operacaoApr.CodAtvPadrao.Value).Nome;
            }

            if (operacaoApr.CodDisciplina.HasValue)
            {
                nomeDisciplina = disciplinaPersistencia.ListarDisciplinaPorId(operacaoApr.CodDisciplina.Value).Nome;
            }
            try
            {
                conteudo += $@" {Environment.NewLine} 
                            Dados da operação atual:
                            Código: {operacaoApr.Codigo}
                            Descrição: {operacaoApr.Descricao}
                            Status APR: {operacaoApr.CodStatusAPR}
                            Disciplina: {nomeDisciplina}
                            Atividade: {nomeAtividadePadrao}
                            Local instalação: {nomeLocalInstalacao}

                            --- FIM LOG EDIÇÃO ---
                            ";
                return conteudo;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

    }
}
