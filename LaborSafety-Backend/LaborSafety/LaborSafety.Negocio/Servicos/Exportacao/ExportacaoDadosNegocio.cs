using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LaborSafety.Dominio.Modelos;
using LaborSafety.Dominio.Modelos.Exportacao;
using LaborSafety.Negocio.Interfaces.Exportacao;
using LaborSafety.Persistencia;
using LaborSafety.Persistencia.Interfaces;
using LaborSafety.Utils;
using LaborSafety.Utils.Constantes;

namespace LaborSafety.Negocio.Servicos.Exportacao
{
    public class ExportacaoDadosNegocio : IExportacaoDadosNegocio
    {
        private readonly IAtividadePadraoPersistencia atividadePersistencia;
        private readonly IPesoPersistencia pesoPersistencia;
        private readonly IDisciplinaPersistencia disciplinaPersistencia;
        private readonly IDuracaoPersistencia duracaoPersistencia;
        private readonly IPerfilCatalogoPersistencia perfilCatalogoPersistencia;
        private readonly IAmbientePersistencia sistemaOperacionalPersistencia;
        private readonly IInventariosAmbientePersistencia inventariosAmbientePersistencia;
        private readonly IInventariosAtividadePersistencia inventariosAtividadePersistencia;
        private readonly IRiscoPersistencia riscoPersistencia;
        private readonly ILocalInstalacaoPersistencia localInstalacaoPersistencia;
        private readonly IAprPersistencia aprPersistencia;
        private readonly IEPIPersistencia epiPersistencia;
        private readonly ISeveridadePersistencia severidadePersistencia;
        private readonly IProbabilidadePersistencia probabilidadePersistencia;
        private readonly IBloqueioPersistencia bloqueioPersistencia;
        private readonly INrPersistencia nrPersistencia;
        private readonly IEPIRiscoInventarioAmbientePersistencia epiRiscoInventarioAmbientePersistencia;
        private readonly IEPIRiscoInventarioAtividadePersistencia epiRiscoInventarioAtividadePersistencia;
        private readonly IPessoaPersistencia pessoaPersistencia;
        private readonly ILogAprPersistencia logAprPersistencia;

        public ExportacaoDadosNegocio(IAtividadePadraoPersistencia atividadePersistencia, IPesoPersistencia pesoPersistencia,
            IDuracaoPersistencia duracaoPersistencia, IPerfilCatalogoPersistencia perfilCatalogoPersistencia, IDisciplinaPersistencia disciplinaPersistencia,
            IAmbientePersistencia sistemaOperacionalPersistencia, IInventariosAmbientePersistencia inventariosAmbientePersistencia,
            IInventariosAtividadePersistencia inventariosAtividadePersistencia, IRiscoPersistencia riscoPersistencia, ILocalInstalacaoPersistencia localInstalacaoPersistencia,
            IAprPersistencia aprPersistencia, IEPIPersistencia epiPersistencia, ISeveridadePersistencia severidadePersistencia, IProbabilidadePersistencia probabilidadePersistencia,
            IBloqueioPersistencia bloqueioPersistencia, INrPersistencia nrPersistencia, IEPIRiscoInventarioAmbientePersistencia epiRiscoInventarioAmbientePersistencia,
            IEPIRiscoInventarioAtividadePersistencia epiRiscoInventarioAtividadePersistencia, IPessoaPersistencia pessoaPersistencia, ILogAprPersistencia logAprPersistencia)
        {
            this.atividadePersistencia = atividadePersistencia;
            this.pesoPersistencia = pesoPersistencia;
            this.disciplinaPersistencia = disciplinaPersistencia;
            this.duracaoPersistencia = duracaoPersistencia;
            this.perfilCatalogoPersistencia = perfilCatalogoPersistencia;
            this.sistemaOperacionalPersistencia = sistemaOperacionalPersistencia;
            this.inventariosAmbientePersistencia = inventariosAmbientePersistencia;
            this.inventariosAtividadePersistencia = inventariosAtividadePersistencia;
            this.riscoPersistencia = riscoPersistencia;
            this.localInstalacaoPersistencia = localInstalacaoPersistencia;
            this.aprPersistencia = aprPersistencia;
            this.epiPersistencia = epiPersistencia;
            this.severidadePersistencia = severidadePersistencia;
            this.probabilidadePersistencia = probabilidadePersistencia;
            this.bloqueioPersistencia = bloqueioPersistencia;
            this.nrPersistencia = nrPersistencia;
            this.epiRiscoInventarioAmbientePersistencia = epiRiscoInventarioAmbientePersistencia;
            this.epiRiscoInventarioAtividadePersistencia = epiRiscoInventarioAtividadePersistencia;
            this.pessoaPersistencia = pessoaPersistencia;
        }

        public class ResultadoExportacao
        {
            public string planilha;
        }

        public void ExportarDadosAmbiente(XLWorkbook wb, DadosExportacaoAmbienteModelo dados)
        {
            var folhaAtual = wb.Worksheets.Worksheet("InventarioAmbiente");
            int linha = 5;

            if (!folhaAtual.Cell("B1").Value.ToString().Contains("Ambiente"))
                throw new Exception("É necessário realizar a exportação de um inventário de ambiente com o nome da pllanilha escrita da seguinte forma: 'InventarioAmbiente'");

            var resultadoInventario = inventariosAmbientePersistencia.ListarInventarioAmbienteExportacao(dados);
            int linhaAPreencher = 5;
            string locais = "";
            if (resultadoInventario != null && resultadoInventario.Count > 0)
            {
                foreach (var item in resultadoInventario)
                {
                    string nomeNR = "";
                    var sistemaOperacional = sistemaOperacionalPersistencia.ListarSistemaOperacionalPorId(item.CodAmbiente);

                    folhaAtual.Cell($"B{linha}").Value = item.Codigo;
                    folhaAtual.Cell($"C{linha}").Value = sistemaOperacional.Nome;
                    folhaAtual.Cell($"D{linha}").Value = item.Descricao;
                    folhaAtual.Cell($"E{linha}").Value = item.ObservacaoGeral;
                    folhaAtual.Cell($"F{linha}").Value = item.RiscoGeral;

                    var itemLocal = localInstalacaoPersistencia.ListarLocaisInstalacaoPorCodInventarioAmbiente(item.CodInventarioAmbiente);

                    if (itemLocal.Count <= 0)
                        throw new Exception($"O inventário de ambiente de código {item.Codigo} não possui local de instalação.");

                    // PREENCHIMENTO LOCAL INSTALAÇÃO
                    foreach (var itemLocalAmbiente in itemLocal)
                    {
                        locais += $"{itemLocalAmbiente.Nome};";
                    }
                    var tamanhoLocal = locais.Length;
                    var nomeLocaisCorrigido = locais.Substring(0, tamanhoLocal - 1);

                    PreencherLocalInstalacao(folhaAtual, nomeLocaisCorrigido, linhaAPreencher);
                    locais = "";
                    // PREENCHIMENTO RISCOS
                    if (item.RISCO_INVENTARIO_AMBIENTE.Count <= 0)
                        throw new Exception($"O inventário de ambiente de código {item.Codigo} não possui riscos associados.");

                    IXLCell celulaAtual = folhaAtual.Cell($"I{linhaAPreencher}");

                    foreach (var itemRisco in item.RISCO_INVENTARIO_AMBIENTE)
                    {
                        celulaAtual = PreencherRiscosAmbiente(folhaAtual, itemRisco, celulaAtual);
                    }

                    // PREENCHIMENTO NR
                    foreach (var itemNR in item.NR_INVENTARIO_AMBIENTE)
                    {
                        if(itemNR.CodNR != (long)Constantes.NR.NR0)
                        {
                            var nr = nrPersistencia.ListarNrPorId(itemNR.CodNR);
                            nomeNR += $"{nr.Codigo};";
                        }
                    }

                    if(!string.IsNullOrEmpty(nomeNR))
                    {
                        var tamanhoNR = nomeNR.Length;
                        var nomeNRCorrigido = nomeNR.Substring(0, tamanhoNR - 1);

                        PreencherNR(folhaAtual, nomeNRCorrigido, linhaAPreencher);
                    }

                    linhaAPreencher++;
                    linha++;
                }

                folhaAtual.Style.Font.FontName = "Arial";
                folhaAtual.Style.Font.FontSize = 8;
                folhaAtual.Style.Font.FontColor = XLColor.Black;
            }
            else
                throw new Exception("Não há inventário(s) com os filtros informados!");
        }

        public void ExportarDadosAtividade(XLWorkbook wb, DadosExportacaoAtividadeModelo dados)
        {
            var folhaAtual = wb.Worksheets.Worksheet("InventarioAtividade");
            int linha = 5;
            int linhaAPreencher = 5;

            if (!folhaAtual.Cell("B1").Value.ToString().Contains("Atividade"))
                throw new Exception("É necessário realizar a exportação de um inventário de atividade com o nome da planilha escrita da seguinte forma: 'InventarioAtividade'");

            var resultadoInventario = inventariosAtividadePersistencia.ListarInventarioAtividadeExportacao(dados);
            
            if (resultadoInventario != null && resultadoInventario.Count > 0)
            {
                foreach (var item in resultadoInventario)
                {
                    string locais = "";
                    var peso = pesoPersistencia.ListarPesoPorId(item.CodPeso);
                    var perfil = perfilCatalogoPersistencia.ListarPerfilCatalogoPorId(item.CodPerfilCatalogo);
                    var duracao = duracaoPersistencia.ListarDuracaoPorId(item.CodDuracao);
                    var atividade = atividadePersistencia.ListarAtividadePorId(item.CodAtividade);
                    var disciplina = disciplinaPersistencia.ListarDisciplinaPorId(item.CodDisciplina);

                    folhaAtual.Cell($"B{linha}").Value = item.Codigo;
                    folhaAtual.Cell($"C{linha}").Value = disciplina.Nome;
                    folhaAtual.Cell($"D{linha}").Value = atividade.Nome;
                    folhaAtual.Cell($"E{linha}").Value = perfil.Codigo;
                    folhaAtual.Cell($"F{linha}").Value = item.RiscoGeral;
                    folhaAtual.Cell($"H{linha}").Value = peso.Nome;
                    folhaAtual.Cell($"I{linha}").Value = duracao.Nome;
                    folhaAtual.Cell($"J{linha}").Value = item.Descricao;
                    folhaAtual.Cell($"K{linha}").Value = item.ObservacaoGeral;

                    var itemLocal = localInstalacaoPersistencia.ListarLocaisInstalacaoPorCodInventarioAtividade(item.CodInventarioAtividade);
                    if (itemLocal.Count <= 0)
                        throw new Exception($"O inventário de atividade de código {item.Codigo} não possui local de instalação.");

                    // PREENCHIMENTO LOCAL INSTALAÇÃO
                    foreach (var itemLocalAtividade in itemLocal)
                    {
                        var localInstalacao = localInstalacaoPersistencia.ListarLocalInstalacaoPorId(itemLocalAtividade.CodLocalInstalacao);
                        locais += $"{localInstalacao.Nome};";
                    }
                    var tamanhoLocal = locais.Length;
                    var nomeLocaisCorrigido = locais.Substring(0, tamanhoLocal - 1);

                    PreencherLocalInstalacao(folhaAtual, nomeLocaisCorrigido, linhaAPreencher);
                    locais = "";

                    // PREENCHIMENTO RISCOS
                    if (item.RISCO_INVENTARIO_ATIVIDADE.Count <= 0)
                        throw new Exception($"O inventário de atividade de código {item.Codigo} não possui riscos associados.");

                    IXLCell celulaAtual = folhaAtual.Cell($"M{linhaAPreencher}");

                    foreach (var itemRisco in item.RISCO_INVENTARIO_ATIVIDADE)
                    {
                        celulaAtual = PreencherRiscosAtividade(folhaAtual, itemRisco, celulaAtual);
                    }

                    linhaAPreencher++;
                    linha++;
                }

                folhaAtual.Style.Font.FontName = "Arial";
                folhaAtual.Style.Font.FontSize = 8;
                folhaAtual.Style.Font.FontColor = XLColor.Black;
            }
            else
                throw new Exception("Não há inventário(s) com os filtros informados!");
        }

        public void PreencherLocalInstalacao(IXLWorksheet planilha, string local, int linhaAPreencher)
        {
            planilha.Cell($"G{linhaAPreencher}").Value = local;
        }

        public void PreencherNR(IXLWorksheet planilha, string nr, int linhaAPreencher)
        {
            planilha.Cell($"MG{linhaAPreencher}").Value = nr;
        }

        public IXLCell CalcularProximaCelula(IXLWorksheet planilha, IXLCell celula)
        {
            var proximaCelula = planilha.Cell($"{celula.Address.ColumnLetter}{celula.Address.RowNumber}").CellRight();

            return proximaCelula;
        }

        public IXLCell PreencherRiscosAmbiente(IXLWorksheet planilha, RISCO_INVENTARIO_AMBIENTE riscosAmbiente, IXLCell celulaAPreencher)
        {
            var coluna = celulaAPreencher.Address.ColumnLetter;
            var risco = riscoPersistencia.ListarRiscoPorId(riscosAmbiente.CodRiscoAmbiente);
            planilha.Cell($"{celulaAPreencher.Address.ColumnLetter}{celulaAPreencher.Address.RowNumber}").Value = risco.Nome;
            celulaAPreencher = celulaAPreencher.CellRight();

            coluna = celulaAPreencher.Address.ColumnLetter;
            var severidade = severidadePersistencia.ListarSeveridadePorId(riscosAmbiente.CodSeveridade);
            planilha.Cell($"{celulaAPreencher.Address.ColumnLetter}{celulaAPreencher.Address.RowNumber}").Value = severidade.Nome;
            celulaAPreencher = celulaAPreencher.CellRight();

            coluna = celulaAPreencher.Address.ColumnLetter;
            var probabilidade = probabilidadePersistencia.ListarProbabilidadePorId(riscosAmbiente.CodProbabilidade);
            planilha.Cell($"{celulaAPreencher.Address.ColumnLetter}{celulaAPreencher.Address.RowNumber}").Value = probabilidade.Nome;
            celulaAPreencher = celulaAPreencher.CellRight();

            coluna = celulaAPreencher.Address.ColumnLetter;
            planilha.Cell($"{celulaAPreencher.Address.ColumnLetter}{celulaAPreencher.Address.RowNumber}").Value = riscosAmbiente.FonteGeradora;
            celulaAPreencher = celulaAPreencher.CellRight();

            coluna = celulaAPreencher.Address.ColumnLetter;
            planilha.Cell($"{celulaAPreencher.Address.ColumnLetter}{celulaAPreencher.Address.RowNumber}").Value = riscosAmbiente.ProcedimentosAplicaveis;
            celulaAPreencher = celulaAPreencher.CellRight();

            // preencher EPI
            if (riscosAmbiente.EPI_RISCO_INVENTARIO_AMBIENTE.Count > 0)
            {
                string nomeEpi = "";

                coluna = celulaAPreencher.Address.ColumnLetter;
                foreach (var item in riscosAmbiente.EPI_RISCO_INVENTARIO_AMBIENTE)
                {
                    var epi = epiPersistencia.ListarEPIPorId(item.CodEPI);
                    nomeEpi += $"{epi.Nome};";
                }

                var tamanhoEpi = nomeEpi.Length;

                var nomeEpiCorrigido = nomeEpi.Substring(0, tamanhoEpi - 1);

                planilha.Cell($"{celulaAPreencher.Address.ColumnLetter}{celulaAPreencher.Address.RowNumber}").Value = nomeEpiCorrigido;
            }
            celulaAPreencher = celulaAPreencher.CellRight();

            coluna = celulaAPreencher.Address.ColumnLetter;
            planilha.Cell($"{celulaAPreencher.Address.ColumnLetter}{celulaAPreencher.Address.RowNumber}").Value = riscosAmbiente.ContraMedidas;
            celulaAPreencher = celulaAPreencher.CellRight();
            celulaAPreencher = celulaAPreencher.CellRight();
            return celulaAPreencher;
        }



        public IXLCell PreencherRiscosAtividade(IXLWorksheet planilha, RISCO_INVENTARIO_ATIVIDADE riscosAtividade, IXLCell celulaAPreencher)
        {
            var coluna = celulaAPreencher.Address.ColumnLetter;
            var risco = riscoPersistencia.ListarRiscoPorId(riscosAtividade.CodRisco);
            planilha.Cell($"{celulaAPreencher.Address.ColumnLetter}{celulaAPreencher.Address.RowNumber}").Value = risco.Nome;
            celulaAPreencher = celulaAPreencher.CellRight();

            coluna = celulaAPreencher.Address.ColumnLetter;
            var severidade = severidadePersistencia.ListarSeveridadePorId(riscosAtividade.CodSeveridade);
            planilha.Cell($"{celulaAPreencher.Address.ColumnLetter}{celulaAPreencher.Address.RowNumber}").Value = severidade.Nome;
            celulaAPreencher = celulaAPreencher.CellRight();

            coluna = celulaAPreencher.Address.ColumnLetter;
            planilha.Cell($"{celulaAPreencher.Address.ColumnLetter}{celulaAPreencher.Address.RowNumber}").Value = riscosAtividade.FonteGeradora;
            celulaAPreencher = celulaAPreencher.CellRight();

            coluna = celulaAPreencher.Address.ColumnLetter;
            planilha.Cell($"{celulaAPreencher.Address.ColumnLetter}{celulaAPreencher.Address.RowNumber}").Value = riscosAtividade.ProcedimentoAplicavel;
            celulaAPreencher = celulaAPreencher.CellRight();

            // preencher EPI
            if (riscosAtividade.EPI_RISCO_INVENTARIO_ATIVIDADE.Count > 0)
            {
                string nomeEpi = "";

                coluna = celulaAPreencher.Address.ColumnLetter;
                foreach (var item in riscosAtividade.EPI_RISCO_INVENTARIO_ATIVIDADE)
                {
                    var epi = epiPersistencia.ListarEPIPorId(item.CodEPI);
                    nomeEpi += $"{epi.Nome};";
                }

                var tamanhoEpi = nomeEpi.Length;

                var nomeEpiCorrigido = nomeEpi.Substring(0, tamanhoEpi - 1);

                planilha.Cell($"{celulaAPreencher.Address.ColumnLetter}{celulaAPreencher.Address.RowNumber}").Value = nomeEpiCorrigido;
            }
            celulaAPreencher = celulaAPreencher.CellRight();

            coluna = celulaAPreencher.Address.ColumnLetter;
            planilha.Cell($"{celulaAPreencher.Address.ColumnLetter}{celulaAPreencher.Address.RowNumber}").Value = riscosAtividade.ContraMedidas;
            celulaAPreencher = celulaAPreencher.CellRight();
            celulaAPreencher = celulaAPreencher.CellRight();
            return celulaAPreencher;
        }

       

        public void ExportarDadosAPR(XLWorkbook wb, DadosExportacaoAprModelo dados)
        {
            var planilha = wb.Worksheets.Worksheet("APR");
            int linha = 5;
            string riscos="";

            var resultado = aprPersistencia.ListarAprExportacao(dados);

            foreach (var itemApr in resultado)
            {
                planilha.Cell($"B{linha}").Value = itemApr.NumeroSerie;
                planilha.Cell($"C{linha}").Value = itemApr.OrdemManutencao;
                planilha.Cell($"D{linha}").Value = itemApr.Descricao;
                planilha.Cell($"E{linha}").Value = itemApr.RiscoGeral;

                // VALIDAÇÕES
                if (itemApr.OPERACAO_APR.Count <= 0)
                    throw new Exception($"A APR de número de série {itemApr.NumeroSerie} não possui operação vinculada.");

                if (itemApr.RISCO_APR.Count <= 0)
                    throw new Exception($"A APR de número de série {itemApr.NumeroSerie} não possui risco vinculado.");

                //RISCOS APR
                foreach (var itemRisco in itemApr.RISCO_APR)
                {
                    var nomeRisco = riscoPersistencia.ListarRiscoPorId(itemRisco.CodRisco);
                    riscos = $"{nomeRisco.Nome};";
                }

                var tamanhoRisco = riscos.Length;
                var nomeRiscoCorrigido = riscos.Substring(0, tamanhoRisco - 1);
                planilha.Cell($"G{linha}").Value = nomeRiscoCorrigido;

                //OPERAÇÃO APR
                foreach (var itemOperacao in itemApr.OPERACAO_APR)
                {
                    var localRecuperado = localInstalacaoPersistencia.ListarLocalInstalacaoPorId((long)itemOperacao.CodLI);

                    if (localRecuperado == null)
                        throw new Exception($"O local de instalação de código {itemOperacao.CodLI} não foi encontrado na base de dados.");

                    if (itemOperacao.CodAtvPadrao.Value == null || itemOperacao.CodAtvPadrao.Value == 0)
                        throw new Exception($"A atividade padrão não foi encontrado na base de dados.");

                    if (itemOperacao.CodDisciplina.Value == null || itemOperacao.CodDisciplina.Value == 0)
                        throw new Exception($"A disciplina não foi encontrado na base de dados.");

                    var atvPadrao = atividadePersistencia.ListarAtividadePorId(itemOperacao.CodAtvPadrao.Value);
                    var disciplina = disciplinaPersistencia.ListarDisciplinaPorId(itemOperacao.CodDisciplina.Value);

                    planilha.Cell($"I{linha}").Value = itemOperacao.Codigo;
                    planilha.Cell($"J{linha}").Value = itemOperacao.Descricao;
                    planilha.Cell($"K{linha}").Value = localRecuperado.Nome;
                    planilha.Cell($"L{linha}").Value = atvPadrao.Nome;
                    planilha.Cell($"M{linha}").Value = disciplina.Nome;

                    linha++;
                }
            }

            planilha.Style.Font.FontName = "Arial";
            planilha.Style.Font.FontSize = 8;
            planilha.Style.Font.FontColor = XLColor.Black;
        }

        public ResultadoExportacao GerarArquivoAmbiente(DadosExportacaoAmbienteModelo dados)
        {
            ResultadoExportacao resultadoExportacao = new ResultadoExportacao();

            try
            {
                string caminhoModelo = ArquivoDiretorioUtils.ObterDiretorioModelo();
                var caminhoGeracao = ArquivoDiretorioUtils.ObterDiretorioExportacaoAmbiente();
                caminhoGeracao = ArquivoDiretorioUtils.ConstruirObterDiretorioData(caminhoGeracao);
                var dataHora = DateTime.Now.ToString("dd_MM_yyy_HH_mm_ss");
                string caminhoCompletoExportacaoModelo = $"{caminhoModelo}LayoutExportacaoAmbiente.xlsx";
                string caminhoCompletoExportacaoFinal = $"{caminhoGeracao}/ExportacaoAmbiente_{dataHora}.xlsx";
                ArquivoDiretorioUtils.CopiarArquivo(caminhoCompletoExportacaoModelo,caminhoCompletoExportacaoFinal);

                using (var workbook = new XLWorkbook(caminhoCompletoExportacaoFinal))
                {
                    ExportarDadosAmbiente(workbook, dados);
                    workbook.Save();
                    workbook.Dispose();
                }

                string _b64 = Convert.ToBase64String(File.ReadAllBytes(caminhoCompletoExportacaoFinal));

                resultadoExportacao.planilha = _b64;

                return resultadoExportacao;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ResultadoExportacao GerarArquivoAtividade(DadosExportacaoAtividadeModelo dados)
        {
            ResultadoExportacao resultadoExportacao = new ResultadoExportacao();

            try
            {
                string caminhoModelo = ArquivoDiretorioUtils.ObterDiretorioModelo();

                var caminhoGeracao = ArquivoDiretorioUtils.ObterDiretorioExportacaoAtividade();
                caminhoGeracao = ArquivoDiretorioUtils.ConstruirObterDiretorioData(caminhoGeracao);
                string caminhoCompletoExportacaoModelo = $"{caminhoModelo}LayoutExportacaoAtividade.xlsx";
                var dataHora = DateTime.Now.ToString("dd_MM_yyy_HH_mm_ss");
                string caminhoCompletoExportacaoFinal = $"{caminhoGeracao}ExportacaoAtividadeGerada_{dataHora}.xlsx";
                ArquivoDiretorioUtils.CopiarArquivo(caminhoCompletoExportacaoModelo,caminhoCompletoExportacaoFinal);

                using (var workbook = new XLWorkbook(caminhoCompletoExportacaoFinal))
                {
                    ExportarDadosAtividade(workbook, dados);

                    workbook.Save();
                    workbook.Dispose();
                }

                string _b64 = Convert.ToBase64String(File.ReadAllBytes(caminhoCompletoExportacaoFinal));

                resultadoExportacao.planilha = _b64;

                return resultadoExportacao;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ResultadoExportacao GerarArquivoAPR(DadosExportacaoAprModelo dados)
        {
            ResultadoExportacao resultadoExportacao = new ResultadoExportacao();

            try
            {
                string caminhoModelo = ArquivoDiretorioUtils.ObterDiretorioModelo();
                var caminhoGeracaoApr = ArquivoDiretorioUtils.ObterDiretorioApr();
                caminhoGeracaoApr = ArquivoDiretorioUtils.ConstruirObterDiretorioData(caminhoGeracaoApr);
                var dataHora = DateTime.Now.ToString("dd_MM_yyy_HH_mm_ss");
                string caminhoCompletoExportacaoModelo = $"{caminhoModelo}LayoutExportacaoApr.xlsx";
                string caminhoCompletoExportacaoFinal = $"{caminhoGeracaoApr}ExportacaoApr{dataHora}.xlsx";
                ArquivoDiretorioUtils.CopiarArquivo(caminhoCompletoExportacaoModelo, caminhoCompletoExportacaoFinal);

                using (var workbook = new XLWorkbook(caminhoCompletoExportacaoFinal))
                {
                    ExportarDadosAPR(workbook, dados);

                    workbook.Save();
                    workbook.Dispose();
                }

                string _b64 = Convert.ToBase64String(File.ReadAllBytes(caminhoCompletoExportacaoFinal));

                resultadoExportacao.planilha = _b64;

                return resultadoExportacao;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string AgruparAprPdf(string numeroSerie)
        {
            var resultado = "";
            try
            {
                var apr = aprPersistencia.PesquisarPorNumeroSerie(numeroSerie);
                var diretorioAprs = MontarCaminhoAprs(numeroSerie);
                GerarAPR(apr);
                GerarMapaDeBloqueio(apr.OrdemManutencao);
                var caminhoPdfsCriados = SelecionarArquivosParaAgrupar(diretorioAprs);
                var caminhoPdf = ConversaoPdfUtils.AgruparPdfs(caminhoPdfsCriados, diretorioAprs);
                ConversaoPdfUtils.DeletarPdfsTemporarios(caminhoPdfsCriados);
                resultado = Convert.ToBase64String(File.ReadAllBytes(caminhoPdf));
                return resultado;
            }
            catch
            {
                throw;
            }
        }

        private string MontarCaminhoAprs(string numeroSerie)
        {
            var caminhoAprs = ArquivoDiretorioUtils.ObterDiretorioApr();
            var dataAtual = DateTime.Now.Date.ToString("dd/MM/yyy").Replace('/', '_');
            caminhoAprs = $"{caminhoAprs}/{dataAtual}";
            var caminhoAprDoDia = ArquivoDiretorioUtils.ConstruirDiretorio(caminhoAprs);
            caminhoAprDoDia = $"{caminhoAprDoDia}/{numeroSerie}/";
            if (Directory.Exists(caminhoAprDoDia))
            {
                Directory.Delete(caminhoAprDoDia,true);
            }
            caminhoAprDoDia = ArquivoDiretorioUtils.ConstruirDiretorio(caminhoAprDoDia);
            return caminhoAprDoDia;
        }

        private List<string> SelecionarArquivosParaAgrupar(string diretorioApr)
        {
            List<string> arquivosParaAgrupar = new List<string>();
            var caminhoArquivosParaAgrupar = Directory.GetFiles(diretorioApr,"*.xlsx");
            if (!caminhoArquivosParaAgrupar.Any())
            {
                return arquivosParaAgrupar;
            }

            foreach (var caminhoPlanilha in caminhoArquivosParaAgrupar)
            {
                ConversaoPdfUtils.GerarArquivoPdf(caminhoPlanilha);
                var diretorioPlanilha = Path.GetDirectoryName(caminhoPlanilha);
                var arquivo = Path.GetFileName(caminhoPlanilha);
                arquivo = $"{arquivo.Split('.')[0]}.pdf";
                var caminhoArquivo = $"{diretorioPlanilha}/{arquivo}";
                arquivosParaAgrupar.Add(caminhoArquivo);
            }

            //Realiza a ordenação
            arquivosParaAgrupar = OrdenaFolhasAPR(arquivosParaAgrupar);

            return arquivosParaAgrupar;
        }

        private List<string> OrdenaFolhasAPR(List<string> arquivosParaAgrupar)
        {
            string[] posicoesFolhas = new string[arquivosParaAgrupar.Count];

            int menorPosFolhaComp = 10000;
            int maiorPosFolhaComp = -1;

            int menorPosMapaBlq = 10000;
            int maiorPosMapaBlq = -1;

            int menorPosEtq = 10000;
            int maiorPosEtq = -1;

            List<string> listaOrdenada = new List<string>();

            for (int i = 0; i < arquivosParaAgrupar.Count; i++)
            {
                var caminhoArquivo = arquivosParaAgrupar[i];

                //APR na primeira posição
                if (caminhoArquivo.ToLower().Contains("aprgerada"))
                    posicoesFolhas[0] = caminhoArquivo;

                else if (caminhoArquivo.ToLower().Contains("complementar"))
                {
                    if (i < menorPosFolhaComp)
                        menorPosFolhaComp = i;

                    if (i > maiorPosFolhaComp)
                        maiorPosFolhaComp = i;
                }
                else if (caminhoArquivo.ToLower().Contains("mapa"))
                {
                    if (i < menorPosMapaBlq)
                        menorPosMapaBlq = i;

                    if (i > maiorPosMapaBlq)
                        maiorPosMapaBlq = i;
                }
                else if (caminhoArquivo.ToLower().Contains("etiqueta"))
                {
                    if (i < menorPosEtq)
                        menorPosEtq = i;

                    if (i > maiorPosEtq)
                        maiorPosEtq = i;
                }
            }

            //Adiciona a APR na primeira posição
            listaOrdenada.Add(posicoesFolhas[0]);

            //Adiciona as folhas complementares
            if (menorPosFolhaComp == maiorPosFolhaComp)
                listaOrdenada.Add(arquivosParaAgrupar[menorPosFolhaComp]);
            else
                for (int i = menorPosFolhaComp; i < maiorPosFolhaComp; i++)
                    listaOrdenada.Add(arquivosParaAgrupar[i]);

            //Adiciona os mapas de bloqueio
            if (menorPosMapaBlq == maiorPosMapaBlq)
                listaOrdenada.Add(arquivosParaAgrupar[menorPosMapaBlq]);
            else
                for (int i = menorPosMapaBlq; i < maiorPosMapaBlq; i++)
                    listaOrdenada.Add(arquivosParaAgrupar[i]);

            //Adiciona as etiquetas
            if (menorPosEtq == maiorPosEtq)
                listaOrdenada.Add(arquivosParaAgrupar[menorPosEtq]);
            else
                for (int i = menorPosEtq; i < maiorPosEtq; i++)
                    listaOrdenada.Add(arquivosParaAgrupar[i]);

            return listaOrdenada;
        }


        private void GerarMapaDeBloqueio(string ordemManutencao)
        {
            List<string> ordensManutencao = new List<string>();
            ordensManutencao.Add(ordemManutencao);
            BloqueioNegocio bloqueioNegocio = new BloqueioNegocio(bloqueioPersistencia,aprPersistencia);
            DadosMapaBloqueioAprModelo dadosMapaBloqueioAprModelo = new DadosMapaBloqueioAprModelo();
            dadosMapaBloqueioAprModelo.OrdemManutencao = ordensManutencao;
            bloqueioNegocio.ListarBloqueioPorListaLIPorArea(dadosMapaBloqueioAprModelo);
        }

        private void GerarAPR(APR apr)
        {
            DadosAprModelo dadosAprModelo = new DadosAprModelo();
            dadosAprModelo.DescricaoAtividade = apr.Descricao;
            dadosAprModelo.OrdemManutencao = apr.OrdemManutencao;
            dadosAprModelo.Operacoes = new List<AprOperacao>();
            foreach (var operacao in apr.OPERACAO_APR)
            {
                if (operacao.Ativo)
                {
                    var novaAprOperacao = new AprOperacao();
                    novaAprOperacao.CodAtvPadrao = operacao.CodAtvPadrao.Value;
                    novaAprOperacao.CodLocalInstalacao = operacao.CodLI.Value;
                    novaAprOperacao.CodDisciplina = operacao.CodDisciplina.Value;
                    dadosAprModelo.Operacoes.Add(novaAprOperacao);
                }

            }
            AprNegocio aprNegocio = new AprNegocio(aprPersistencia, inventariosAmbientePersistencia, inventariosAtividadePersistencia, localInstalacaoPersistencia,
                nrPersistencia, epiPersistencia, probabilidadePersistencia, severidadePersistencia,
                atividadePersistencia, pesoPersistencia, duracaoPersistencia, disciplinaPersistencia,
                riscoPersistencia, bloqueioPersistencia, epiRiscoInventarioAmbientePersistencia, epiRiscoInventarioAtividadePersistencia, logAprPersistencia, pessoaPersistencia);

            aprNegocio.GerarApr(dadosAprModelo, apr);
        }

    }
}
