using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace LaborSafety.Utils
{
    public static class ConversaoPdfUtils
    {
        public static void GerarArquivoPdf(string caminhoOrigem)
        {
            try
            {
                string nomeArquivo = Path.GetFileName(caminhoOrigem);
                string diretorio = Path.GetDirectoryName(caminhoOrigem);
                Process pdfProcess = new Process();
                pdfProcess.StartInfo.FileName = ConfigurationManager.AppSettings["libreOffice"];
                pdfProcess.StartInfo.Arguments =
                String.Format("--norestore --nofirststartwizard --headless --convert-to pdf  \"{0}\"", nomeArquivo);
                pdfProcess.StartInfo.WorkingDirectory = diretorio;
                pdfProcess.StartInfo.RedirectStandardOutput = true;
                pdfProcess.StartInfo.RedirectStandardError = true;
                pdfProcess.StartInfo.UseShellExecute = false;
                pdfProcess.Start();
                string saidaErro = pdfProcess.StandardError.ReadToEnd();
                if (saidaErro.Length > 0)
                {
                    throw new Exception("Erro ao gerar arquivo PDF.");
                }
            }
            catch
            {
                throw;
            }
        }

        public static List<string> SelecionarArquivosParaAgrupar(string diretorioApr)
        {
            List<string> arquivosParaAgrupar = new List<string>();
            var caminhoArquivosParaAgrupar = Directory.GetFiles(diretorioApr, "*.xlsx");
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
            return arquivosParaAgrupar;
        }

        public static List<string> GerarPdfeAgruparArquivos(List<string> listaCaminhos)
        {
            List<string> arquivosParaAgrupar = new List<string>();
            foreach (var caminhoPlanilha in listaCaminhos)
            {
                ConversaoPdfUtils.GerarArquivoPdf(caminhoPlanilha);
                var diretorioPlanilha = Path.GetDirectoryName(caminhoPlanilha);
                var arquivo = Path.GetFileName(caminhoPlanilha);
                arquivo = $"{arquivo.Split('.')[0]}.pdf";
                var caminhoArquivo = $"{diretorioPlanilha}/{arquivo}";
                arquivosParaAgrupar.Add(caminhoArquivo);
            }
            return arquivosParaAgrupar;
        }


        public static void DeletarPdfsTemporarios(List<string> listaPdfs)
        {
            foreach (var pdf in listaPdfs)
            {
                File.Delete(pdf);
            }
        }

        public static string AgruparPdfs(List<string> listaPdfs, string diretorioAprs)
        {
            using (PdfDocument pdfAgrupado = new PdfDocument())
            {
                foreach (var pdf in listaPdfs)
                {
                    PdfDocument arquivoPdf = PdfReader.Open(pdf, PdfDocumentOpenMode.Import);
                    foreach (var pagina in arquivoPdf.Pages)
                    {
                        pdfAgrupado.AddPage(pagina);
                    }
                }
                var diretorioSalvamentoPdf = $"{diretorioAprs}/pdfAgrupado.pdf";
                pdfAgrupado.Save(diretorioSalvamentoPdf);
                return diretorioSalvamentoPdf;
            }
        }

    }
}
