using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using LaborSafety.Dominio.Modelos;
using LaborSafety.Negocio.Interfaces;
using LaborSafety.Persistencia;
using LaborSafety.Persistencia.Interfaces;
using LaborSafety.Utils;
using LaborSafety.Utils.Constantes;

namespace LaborSafety.Negocio.Servicos
{
    public class BloqueioNegocio : IBloqueioNegocio
    {
        private readonly IBloqueioPersistencia bloqueioPersistencia;
        private readonly IAprPersistencia aprPersistencia;
        public BloqueioNegocio(IBloqueioPersistencia bloqueioPersistencia, IAprPersistencia aprPersistencia)
        {
            this.bloqueioPersistencia = bloqueioPersistencia;
            this.aprPersistencia = aprPersistencia;
        }
       
        private string GerarMapaBloqueio(string diretorioFinal, string area)
        {
            string caminhoModelo = ArquivoDiretorioUtils.ObterDiretorioModelo();
            string caminhoCompletoBloqueioModelo = $"{caminhoModelo}LayoutMapaBloqueio.xlsx";
            string caminhoCompletoBloqueioFinal = $"{diretorioFinal}//MapaBloqueioGerado{area}.xlsx";
            ArquivoDiretorioUtils.CopiarArquivo(caminhoCompletoBloqueioModelo,caminhoCompletoBloqueioFinal);
            return caminhoCompletoBloqueioFinal;
        }

        private string GerarEtiquetaBloqueio(string diretorioFinal,long codEtiqueta)
        {
            string caminhoModelo = ArquivoDiretorioUtils.ObterDiretorioModelo();
            string caminhoCompletoEtiquetaBloqueioModelo = $"{caminhoModelo}LayoutEtiquetaBloqueio.xlsx";
            string caminhoCompletoEtiquetaBloqueioFinal = $"{diretorioFinal}//EtiquetaBloqueioGerado_{codEtiqueta}.xlsx";
            ArquivoDiretorioUtils.CopiarArquivo(caminhoCompletoEtiquetaBloqueioModelo, caminhoCompletoEtiquetaBloqueioFinal);
            return caminhoCompletoEtiquetaBloqueioFinal;
        }


        private void ValidarMapaBloqueio(APR apr, TIPO_ENERGIA_BLOQUEIO tipoEnergiaBloqueio, TAG_KKS_BLOQUEIO kks,
            DISPOSITIVO_BLOQUEIO dispositivo, LOCAL_A_BLOQUEAR localABloquear, string ordemManutencao)
        {
            if (apr == null)
            {
                throw new Exception("APR não encontrada!");
            }
            if (tipoEnergiaBloqueio == null)
            {
                throw new Exception("Tipo de energia não encontrada!");
            }
            if (kks == null)
            {
                throw new Exception("KKS não encontrada!");
            }

            if (localABloquear == null)
            {
                throw new Exception("Local a bloquear não encontrado!");
            }

            if (dispositivo == null)
            {
                throw new Exception("Dispositivo não encontrado!");
            }

            if (string.IsNullOrEmpty(ordemManutencao))
                throw new Exception("Ordem de manutenção está nula!");
        }

        private void ValidarEtiquetaBloqueio(APR apr, TAG_KKS_BLOQUEIO kks, string ordemManutencao)
        {
            if (apr == null)
                throw new Exception("APR não encontrada!");

            if (kks == null)
                throw new Exception("KKS não encontrada!");

            if (string.IsNullOrEmpty(ordemManutencao))
                throw new Exception("Ordem de manutenção está nula!");
        }

        private void PreencherLinhasMapa(IXLWorksheet planilha, int linha, DateTime aprDataInicio, string ordemManutencao,
            string tipoEnergiaNome, string kksNome, string localBloquearNome, string dispositivoNome, string nomeArea)
        {
            var numeroItem = linha - 8;
            if (linha != Constantes.LINHA_INICIAL_PLANILHA_BLOQUEIO_NEGOCIO)
            {
                var celulaBase = planilha.Range("B9:CB9");
                planilha.Cell(linha, 2).Value = celulaBase;
            }
            planilha.Cell($"B{linha}").Value = numeroItem;
            planilha.Cell($"C{linha}").Value = aprDataInicio;
            planilha.Cell($"F{linha}").Value = ordemManutencao;
            planilha.Cell($"J{linha}").Value = tipoEnergiaNome;
            planilha.Cell($"M{linha}").Value = kksNome;
            planilha.Cell($"R{linha}").Value = localBloquearNome;
            planilha.Cell($"W{linha}").Value = dispositivoNome;
            planilha.Cell($"F4").Value = nomeArea;

            //planilha.Rows($"9,{linha}").AdjustToContents();
        }

        private void PreencherLinhasEtiqueta(IXLWorksheet planilha, APR apr, TAG_KKS_BLOQUEIO kks)
        {
            planilha.Cell($"B14").Value = apr.OrdemManutencao;
            planilha.Cell($"E14").Value = apr.NumeroSerie;
            planilha.Cell($"B16").Value = kks.Codigo;
        }

        //public List<IGrouping<long, BloqueioModelo>> ListarBloqueioPorListaLIPorArea(DadosMapaBloqueioAprModelo dados)
        //{
        //    List<IGrouping<long, BloqueioModelo>> bloqueioModelo = new List<IGrouping<long, BloqueioModelo>>();
        //    List<BLOQUEIO> lista = this.bloqueioPersistencia.ListarBloqueioPorListaLIPorArea(dados.LocaisIntalacao);
        //    if (lista.Count == 0 || lista == null)
        //        throw new KeyNotFoundException("Não existem bloqueios para o filtro informado.");

        //    var apr = aprPersistencia.PesquisarPorOrdemManutencao(dados.OrdemManutencao);
        //    if (apr == null)
        //        throw new Exception("APR não encontrada!");

        //    var diretorioEtiquetaMapaBloqueio = ArquivoDiretorioUtils.ObterDiretorioApr();
        //    diretorioEtiquetaMapaBloqueio = ArquivoDiretorioUtils.ConstruirObterDiretorioData(diretorioEtiquetaMapaBloqueio);
        //    diretorioEtiquetaMapaBloqueio = $"{diretorioEtiquetaMapaBloqueio}/{apr.NumeroSerie}";
        //    diretorioEtiquetaMapaBloqueio = ArquivoDiretorioUtils.ConstruirDiretorio(diretorioEtiquetaMapaBloqueio);

        //    var caminhoMapaBloqueioFinal = GerarMapaBloqueio(diretorioEtiquetaMapaBloqueio);
        //    int linhas = Constantes.LINHA_INICIAL_PLANILHA_BLOQUEIO_NEGOCIO;

        //    using (var workbook = new XLWorkbook(caminhoMapaBloqueioFinal))
        //    {
        //        foreach (var itemBloqueio in lista)
        //        {
        //            var kks = bloqueioPersistencia.ListarTagKKS(itemBloqueio.CodTagKKSBloqueio, null);
        //            var etiquetaBloqueio = GerarEtiquetaBloqueio(diretorioEtiquetaMapaBloqueio, kks.Codigo);
        //            using (var workbookEtiqueta = new XLWorkbook(etiquetaBloqueio))
        //            {
        //                var planilhaEtiqueta = workbookEtiqueta.Worksheets.Worksheet("Etiqueta de Bloqueio");
        //                ValidarEtiquetaBloqueio(apr, kks, dados.OrdemManutencao);
        //                PreencherLinhasEtiqueta(planilhaEtiqueta, apr, kks);
        //                workbookEtiqueta.Save();
        //                workbookEtiqueta.Dispose();
        //            }
        //        }

        //        var planilha = workbook.Worksheets.Worksheet("Mapa de Bloqueio");
        //        var listaBloqueio = lista.GroupBy(x => x.CodArea);
        //        foreach (var area in listaBloqueio)
        //        {
        //            foreach (var bloqueio in area)
        //            {
        //                var tipoEnergia = bloqueioPersistencia.ListarTipoEnergia(bloqueio.CodTipoEnergiaBloqueio, null);
        //                var kks = bloqueioPersistencia.ListarTagKKS(bloqueio.CodTagKKSBloqueio, null);
        //                var dispositivo = bloqueioPersistencia.ListarDispositivo(bloqueio.CodDispositivoBloqueio, null);
        //                var localABloquear = bloqueioPersistencia.ListarLocalABloquear(bloqueio.CodLocalABloquear, null);
        //                ValidarMapaBloqueio(apr, tipoEnergia, kks, dispositivo, localABloquear, dados.OrdemManutencao);
        //                PreencherLinhasMapa(planilha, linhas, apr.DataInicio, dados.OrdemManutencao, tipoEnergia.Nome, kks.Nome
        //                    , localABloquear.Nome, dispositivo.Nome);
        //                linhas++;
        //            }
        //        }
        //        workbook.Save();
        //        workbook.Dispose();
        //    }
        //    return bloqueioModelo;
        //}

        public List<string> ListarBloqueioPorListaLIPorArea(DadosMapaBloqueioAprModelo dados)
        {
            List<IGrouping<long, BloqueioModelo>> bloqueioModelo = new List<IGrouping<long, BloqueioModelo>>();
            List<string> listaCaminhosDeBloqueios = new List<string>();
            foreach (var item in dados.OrdemManutencao)
            {
                var aprExistente = aprPersistencia.PesquisarPorOrdemManutencao(item);
                if (aprExistente == null)
                    throw new Exception("APR não encontrada!");

                List<long> CodLocaisOperacaoApr = new List<long>();
                foreach (var itemLi in aprExistente.OPERACAO_APR)
                {
                    if (itemLi.Ativo)
                    {
                        CodLocaisOperacaoApr.Add((long)itemLi.CodLI);
                    }
                }
                List<BLOQUEIO> lista = this.bloqueioPersistencia.ListarBloqueioPorListaLIPorAreaLong(CodLocaisOperacaoApr);

                if (lista == null || !lista.Any())
                    return listaCaminhosDeBloqueios;

                var diretorioEtiquetaMapaBloqueio = ArquivoDiretorioUtils.ObterDiretorioApr();
                diretorioEtiquetaMapaBloqueio = ArquivoDiretorioUtils.ConstruirObterDiretorioData(diretorioEtiquetaMapaBloqueio);
                diretorioEtiquetaMapaBloqueio = $"{diretorioEtiquetaMapaBloqueio}/{aprExistente.NumeroSerie}";
                diretorioEtiquetaMapaBloqueio = ArquivoDiretorioUtils.ConstruirDiretorio(diretorioEtiquetaMapaBloqueio);

                foreach (var itemBloqueio in lista)
                {
                    var kks = bloqueioPersistencia.ListarTagKKS(itemBloqueio.CodTagKKSBloqueio, null);
                    var etiquetaBloqueio = GerarEtiquetaBloqueio(diretorioEtiquetaMapaBloqueio, itemBloqueio.CodBloqueio);
                    listaCaminhosDeBloqueios.Add(etiquetaBloqueio);
                    using (var workbookEtiqueta = new XLWorkbook(etiquetaBloqueio))
                    {
                        var planilhaEtiqueta = workbookEtiqueta.Worksheets.Worksheet("Etiqueta de Bloqueio");
                        ValidarEtiquetaBloqueio(aprExistente, kks, item);
                        PreencherLinhasEtiqueta(planilhaEtiqueta, aprExistente, kks);
                        workbookEtiqueta.Save();
                        workbookEtiqueta.Dispose();
                    }
                }
                var listaBloqueio = lista.GroupBy(x => x.CodArea);
                foreach (var area in listaBloqueio)
                {
                    var caminhoMapaBloqueioFinal = GerarMapaBloqueio(diretorioEtiquetaMapaBloqueio,area.Key.ToString());
                    listaCaminhosDeBloqueios.Add(caminhoMapaBloqueioFinal);
                    var workbook = new XLWorkbook(caminhoMapaBloqueioFinal);
                    var planilha = workbook.Worksheets.Worksheet("Mapa de Bloqueio");
                    int linhas = Constantes.LINHA_INICIAL_PLANILHA_BLOQUEIO_NEGOCIO;
                    foreach (var bloqueio in area)
                    {
                        var tipoEnergia = bloqueioPersistencia.ListarTipoEnergia(bloqueio.CodTipoEnergiaBloqueio, null);
                        var kks = bloqueioPersistencia.ListarTagKKS(bloqueio.CodTagKKSBloqueio, null);
                        var dispositivo = bloqueioPersistencia.ListarDispositivo(bloqueio.CodDispositivoBloqueio, null);
                        var localABloquear = bloqueioPersistencia.ListarLocalABloquear(bloqueio.CodLocalABloquear, null);
                        ValidarMapaBloqueio(aprExistente, tipoEnergia, kks, dispositivo, localABloquear, item);
                        PreencherLinhasMapa(planilha, linhas, aprExistente.DataInicio.Value, item, tipoEnergia.Nome, kks.Nome
                            , localABloquear.Nome, dispositivo.Nome, bloqueio.AREA.Nome);
                        linhas++;
                    }
                    workbook.Save();
                    workbook.Dispose();

                }
            }

            return listaCaminhosDeBloqueios;
        }
    }
}