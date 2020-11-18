using System;
using System.Configuration;
using System.IO;
using System.Net.NetworkInformation;

namespace LaborSafety.Utils
{
    public static class ArquivoDiretorioUtils
    {

        public static string ConstruirDiretorio(string caminhoDiretorio)
        {
            if (!Directory.Exists(caminhoDiretorio))
            {
                Directory.CreateDirectory(caminhoDiretorio);
            }
            return caminhoDiretorio;
        }
        
        public static string CopiarArquivo(string caminhoEntrada, string caminhoSaida)
        {
            if (!File.Exists(caminhoSaida))
                File.Copy(caminhoEntrada, caminhoSaida);
            return caminhoSaida;
        }

        public static string ConstruirObterDiretorioData(string diretorio)
        {
            var diretorioBase = diretorio;
            var dataAtual = DateTime.Now.ToString("dd/MM/yyy").Replace('/','_');
            diretorioBase = $"{diretorioBase}{dataAtual}/";
            ConstruirDiretorio(diretorioBase);
            return diretorioBase;
        }

        public static string ObterCaminhoDiretorio(string diretorio)
        {
            string diretorioFinal = "";
            var diretorioBase = ConfigurationManager.AppSettings["pastaRaiz"];
            diretorioFinal = $"{diretorioBase}{diretorio}/";
            ConstruirDiretorio(diretorioFinal);
            return diretorioFinal;
        }

        public static string ObterDiretorioModelo()
        {
            var nomePastaModelo = ConfigurationManager.AppSettings["caminhoModelos"];
            var diterotioModelo = ObterCaminhoDiretorio(nomePastaModelo);
            return diterotioModelo;
        }

        public static string ObterDiretorioApr()
        {
            var nomePastaApr = ConfigurationManager.AppSettings["diretorioAPR"];
            var diterotioApr = ObterCaminhoDiretorio(nomePastaApr);
            return diterotioApr;
        }

        public static string ObterDiretorioExportacaoAmbiente()
        {
            var nomePastaExportacaoAmbiente = $"{ConfigurationManager.AppSettings["diretorioExportacao"]}//InventarioAmbiente";
            var diterotioExportacaoAmbiente = ObterCaminhoDiretorio(nomePastaExportacaoAmbiente);
            return diterotioExportacaoAmbiente;
        }

        public static string ObterDiretorioExportacaoAtividade()
        {
            var nomePastaExportacaoAtividade = $"{ConfigurationManager.AppSettings["Exportacao"]}//InventarioAtividade";
            var diterotioExportacaoAtividade = ObterCaminhoDiretorio(nomePastaExportacaoAtividade);
            return diterotioExportacaoAtividade;
        }

        public static string ObterDiretorioExportacaoApr()
        {
            var nomePastaExportacaoApr = $"{ConfigurationManager.AppSettings["Exportacao"]}//Apr";
            var diterotioExportacaoApr = ObterCaminhoDiretorio(nomePastaExportacaoApr);
            return diterotioExportacaoApr;
        }

        public static string ObterDiretorioArquivosImportados()
        {
            var nomePastaArquivosImportacao = ConfigurationManager.AppSettings["diretorioArquivosImportados"];
            var diretorioArquivosImportados = ObterCaminhoDiretorio(nomePastaArquivosImportacao);
            return diretorioArquivosImportados;
        }
    }
}
