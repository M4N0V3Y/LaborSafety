using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaborSafety.Dominio.Modelos.Exportacao;
using static LaborSafety.Negocio.Servicos.Exportacao.ExportacaoDadosNegocio;

namespace LaborSafety.Negocio.Interfaces.Exportacao
{
    public interface IExportacaoDadosNegocio
    {
        void ExportarDadosAmbiente(XLWorkbook wb, DadosExportacaoAmbienteModelo dados);
        void ExportarDadosAtividade(XLWorkbook wb, DadosExportacaoAtividadeModelo dados);
        void ExportarDadosAPR(XLWorkbook wb, DadosExportacaoAprModelo dados);
        ResultadoExportacao GerarArquivoAmbiente(DadosExportacaoAmbienteModelo dados);
        ResultadoExportacao GerarArquivoAtividade(DadosExportacaoAtividadeModelo dados);
        ResultadoExportacao GerarArquivoAPR(DadosExportacaoAprModelo dados);
        string AgruparAprPdf(string numeroSerie);
    }
}
