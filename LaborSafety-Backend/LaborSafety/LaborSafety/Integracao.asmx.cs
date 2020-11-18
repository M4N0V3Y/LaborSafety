using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Web.Services;
using LaborSafety.Dominio.Modelos.SAP;
using LaborSafety.Negocio.Interfaces.SAP;
using LaborSafety.Negocio.Servicos;
using LaborSafety.Utils.Constantes;
using EntregaEColeta.API.SOAP;
using Unity;

namespace LaborSafety
{
    public enum TipoIntegracao
    {
        Disciplina = 1,
        AtividadePadrao = 2,
        LocalInstalacao = 3,
        Ordem = 4,
        PerfilCatalogo = 5,
        TSTTERNIUM = 6
    }


    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class Integracao : BaseService<Integracao>
    {
        public Integracao() : base() { }

        [Dependency]
        public IDisciplinaSAPNegocio disciplinaSAPNegocio { get; set; }

        [Dependency]
        public IAtividadePadraoSAPNegocio atividadePadraoSAPNegocio { get; set; }

        [Dependency]
        public IPerfilCatalogoSAPNegocio perfilCatalogoSAPNegocio { get; set; }

        [Dependency]
        public ILocalInstalacaoSAPNegocio localInstalacaoSAPNegocio { get; set; }

        [Dependency]
        public IAPRSapNegocio aprSapNegocio { get; set; }



        [WebMethod]
        public DisciplinaSAPResponse DT_CARACT_LaborSafety_RFC01_ProcessaDisciplina(DisciplinaSAPModelo modelo)
        {
            var disciplinaResponse = new DisciplinaSAPResponse();
            DisciplinaSAPResponse result = new DisciplinaSAPResponse();
            result.Itens = new List<DisciplinaItemSAPResponse>();

            try
            { 
                //Grava o que chegou na integração, em um arquivo texto
                this.GeraLogRecebimentoIntegracao(modelo, TipoIntegracao.Disciplina);

                result = disciplinaSAPNegocio.ProcessarDisciplina(modelo);
            }
            catch (Exception e)
            {
                DisciplinaItemSAPResponse itemResponse = new DisciplinaItemSAPResponse();
                itemResponse.Caracteristica = "";
                itemResponse.Status = Constantes.StatusResponseIntegracao.E.ToString();
                itemResponse.Descricao = e.Message;

                result.Itens.Add(itemResponse);
            }

            return result;
        }

        [WebMethod]
        public AtividadePadraoSAPResponse DT_CHAVEMOD_LaborSafety_RFC02_ProcessaAtividadePadrao(AtividadePadraoSAPModelo modelo)
        {
            var atividadePadraoResponse = new AtividadePadraoItemSAPModelo();
            AtividadePadraoSAPResponse result = new AtividadePadraoSAPResponse();
            result.Itens = new List<AtividadePadraoItemSAPResponse>();

            try
            {
                //Grava o que chegou na integração, em um arquivo texto
                this.GeraLogRecebimentoIntegracao(modelo, TipoIntegracao.AtividadePadrao);

                result = atividadePadraoSAPNegocio.ProcessarAtividadePadrao(modelo);
            }
            catch (Exception e)
            {
                AtividadePadraoItemSAPResponse itemResponse = new AtividadePadraoItemSAPResponse();
                itemResponse.Chave_Modelo = "";
                itemResponse.Status = Constantes.StatusResponseIntegracao.E.ToString();
                itemResponse.Descricao = e.Message;

                result.Itens.Add(itemResponse);
            }

            return result;
        }

        [WebMethod]
        public PerfilCatalogoSAPResponse DT_ORDEMPM_LaborSafety_RFC04_ProcessaPerfilCatalogo(PerfilCatalogoSAPModelo modelo)
        {
            PerfilCatalogoSAPResponse result = new PerfilCatalogoSAPResponse();
            result.Itens = new List<PerfilCatalogoItemSAPResponse>();

            try
            {
                //Grava o que chegou na integração, em um arquivo texto
                this.GeraLogRecebimentoIntegracao(modelo, TipoIntegracao.PerfilCatalogo);

                result = perfilCatalogoSAPNegocio.ProcessaPerfilCatalogo(modelo);
            }
            catch (Exception e)
            {
                PerfilCatalogoItemSAPResponse itemResponse = new PerfilCatalogoItemSAPResponse();
                itemResponse.Perfil_Do_Catalogo = "";
                itemResponse.Status = Constantes.StatusResponseIntegracao.E.ToString();
                itemResponse.Descricao = e.Message;

                result.Itens.Add(itemResponse);
            }

            return result;
        }

        [WebMethod]
        public LocalInstalacaoSAPResponse DT_LI_LaborSafety_IL02_ProcessaLocalInstalacao(LocalInstalacaoSAPModelo modelo)
        {
            LocalInstalacaoSAPResponse result = new LocalInstalacaoSAPResponse();
            result.Itens = new List<LocalInstalacaoItemSAPResponse>();

            try
            {
                //Grava o que chegou na integração, em um arquivo texto
                this.GeraLogRecebimentoIntegracao(modelo, TipoIntegracao.LocalInstalacao);

                result = localInstalacaoSAPNegocio.ProcessaLocalInstalacao(modelo);
            }
            catch (Exception e)
            {
                LocalInstalacaoItemSAPResponse itemResponse = new LocalInstalacaoItemSAPResponse();
                itemResponse.Status = Constantes.StatusResponseIntegracao.E.ToString();
                itemResponse.Descricao = e.Message;

                result.Itens.Add(itemResponse);
            }

            return result;
        }

        [WebMethod]
        public APRSAPResponse DT_ORDEM_LaborSafety_IW32_IW38_ProcessaOrdem(APRSAPModelo modelo)
        {
            APRSAPResponse result = new APRSAPResponse();
            result.Itens = new List<APRItemSAPResponse>();

            try
            {                
                //Grava o que chegou na integração, em um arquivo texto
                this.GeraLogRecebimentoIntegracao(modelo, TipoIntegracao.Ordem);

                result = aprSapNegocio.ProcessaOrdem(modelo);
            }
            catch (Exception e)
            {
                APRItemSAPResponse itemResponse = new APRItemSAPResponse();
                itemResponse.Status = Constantes.StatusResponseIntegracao.E.ToString();
                itemResponse.Descricao = e.Message;

                result.Itens.Add(itemResponse);
            }

            return result;
        }

        public void GeraLogRecebimentoIntegracao(object modelo, TipoIntegracao tipoIntegracao)
        {
            string caminhoLogIntegracao = ConfigurationManager.AppSettings["caminhoLogIntegracao"];
            string nomeArquivo = "log_" + tipoIntegracao.ToString() + "_" + DateTime.Now.ToString() + ".txt";
            nomeArquivo = nomeArquivo.Replace("/", "_").Replace(" ", "_").Replace(":", "_");
            string caminhoCompleto = caminhoLogIntegracao + nomeArquivo;

            if (!File.Exists(caminhoLogIntegracao)) 
                File.Create(caminhoCompleto).Close();

            using (StreamWriter sw = new StreamWriter(caminhoCompleto))
            {
                sw.WriteLine("**********LOG de recebimento de Integração**********", DateTime.Now.ToString());

                switch (tipoIntegracao)
                {
                    case TipoIntegracao.Disciplina:
                        geraLogMetodoDisciplina((DisciplinaSAPModelo)modelo, sw);
                        break;
                    case TipoIntegracao.AtividadePadrao:
                        geraLogMetodoAtividadePadrao((AtividadePadraoSAPModelo)modelo, sw);
                        break;
                    case TipoIntegracao.LocalInstalacao:
                        geraLogMetodoLI((LocalInstalacaoSAPModelo)modelo,sw);
                        break;
                    case TipoIntegracao.Ordem:
                        geraLogMetodoAPR((APRSAPModelo)modelo, sw);
                        break;
                    case TipoIntegracao.PerfilCatalogo:
                        geraLogMetodoPerfilCatalogo((PerfilCatalogoSAPModelo)modelo, sw);
                        break;
                    //case TipoIntegracao.TSTTERNIUM:
                    //    geraLogMetodoTesteTernium((AprNegocio.TSTTerniumAPR)modelo, sw);
                    //    break;
                    default:
                        break;
                }
            }

        }

        private void geraLogMetodoDisciplina(DisciplinaSAPModelo modelo, StreamWriter sw)
        {
            sw.WriteLine($@"-----MÉTODO PROCESSA DISCIPLINA-----");
            int i = 0;

            foreach (var disciplina in modelo.Itens)
            {
                sw.WriteLine("");
                sw.WriteLine($@"___ITEM {i} da lista___");
                sw.WriteLine($@"Propriedade Característica - {disciplina.Caracteristica}");
                sw.WriteLine($@"Propriedade Valor da Característica - {disciplina.ValorCaracteristica}");
            }
        }

        private void geraLogMetodoAtividadePadrao(AtividadePadraoSAPModelo modelo, StreamWriter sw)
        {
            sw.WriteLine($@"-----MÉTODO PROCESSA ATIVIDADE PADRÃO-----");
            int i = 0;

            foreach (var atv in modelo.Itens)
            {
                sw.WriteLine("");
                sw.WriteLine($@"___ITEM {i} da lista___");
                sw.WriteLine($@"Propriedade Chave Modelo - {atv.Chave_Modelo}");
                sw.WriteLine($@"Propriedade Texto Chave Modelo - {atv.Texto_Chave_Modelo}");
            }
        }

        private void geraLogMetodoLI(LocalInstalacaoSAPModelo modelo, StreamWriter sw)
        {
            sw.WriteLine($@"-----MÉTODO PROCESSA LOCAL DE INSTALAÇÃO-----");
            int i = 0;

            foreach (var LI in modelo.Itens)
            {
                sw.WriteLine("");
                sw.WriteLine($@"___ITEM {i} da lista___");
                sw.WriteLine($@"Propriedade Local de Instalação - {LI.Local_Instalacao}");
                sw.WriteLine($@"Propriedade Descrição do Local de Instalação - {LI.Descricao_Local_Instalacao}");
                sw.WriteLine($@"Propriedade Perfil de Catálogo - {LI.Perfil_Catalogo}");
                sw.WriteLine($@"Propriedade Descrição do Perfil de Catálogo - {LI.Descricao_Perfil_Catalogo}");
                sw.WriteLine($@"Propriedade Classe do Local de Instalação - {LI.Classe_Local_Instalacao}");
                sw.WriteLine($@"Propriedade Descrição da Classe do Local de Instalação - {LI.Descricao_Classe}");
                sw.WriteLine($@"Propriedade Característica da Classe - {LI.Caracteristica_Classe}");
                sw.WriteLine($@"Propriedade Descrição da Característica - {LI.Descricao_Caracteristica}");
                sw.WriteLine($@"Propriedade Valor da Característica - {LI.Valor_Caracteristica}");
                sw.WriteLine($@"Propriedade Status - {LI.Status_Local_Instalacao}");
            }
        }

        private void geraLogMetodoAPR(APRSAPModelo modelo, StreamWriter sw)
        {
            sw.WriteLine($@"-----MÉTODO PROCESSA APR-----");
            int i = 0;

            foreach (var APR in modelo.Itens)
            {
                sw.WriteLine("");
                sw.WriteLine($@"___ITEM {i} da lista___");
                sw.WriteLine($@"Propriedade Número da Ordem - {APR.Numero_Ordem}") ;
                sw.WriteLine($@"Propriedade Descrição da Ordem - {APR.Descricao_Ordem}");
                sw.WriteLine($@"Propriedade Local de Instalação - {APR.Local_Instalacao}");
                sw.WriteLine($@"Propriedade Operação- {APR.Operacao}");
                sw.WriteLine($@"Propriedade Descrição da Operação - {APR.Descricao_Operacao}");
                sw.WriteLine($@"Propriedade Centro de Trabalho - {APR.Centro_Trabalho}");
                sw.WriteLine($@"Propriedade Valor do Centro de Trabalho - {APR.Valor_Centro_Trabalho}");
                sw.WriteLine($@"Propriedade Chave do Modelo de Operação - {APR.Chave_Modelo_Operacao}");
                sw.WriteLine($@"Propriedade Local de Instalação da Operação - {APR.Local_Instalacao_Operacao}");
                sw.WriteLine($@"Propriedade Status - {APR.Status_Ordem}");
            }
        }

        private void geraLogMetodoPerfilCatalogo(PerfilCatalogoSAPModelo modelo, StreamWriter sw)
        {
            sw.WriteLine($@"-----MÉTODO PROCESSA PERFIL DE CATÁLOGO-----");
            int i = 0;

            foreach (var perfil in modelo.Itens)
            {
                sw.WriteLine("");
                sw.WriteLine($@"___ITEM {i} da lista___");
                sw.WriteLine($@"Propriedade Perfil - {perfil.PerfilCatalogo}");
                sw.WriteLine($@"Propriedade Descricao - {perfil.Descricao}");
                sw.WriteLine($@"Propriedade Status - {perfil.Status}");
            }
        }

        //private void geraLogMetodoTesteTernium(AprNegocio.TSTTerniumAPR modelo, StreamWriter sw)
        //{
        //    sw.WriteLine($@"-----RESULTADO DE TEMPOS DOS MÉTODOS-----");

        //    sw.WriteLine("");
        //    sw.WriteLine($@"Tempo para obter diretórios - {modelo.tempoObterDiretorios}");
        //    sw.WriteLine($@"Tempo para construir o diretório - {modelo.tempoConstruirDiretorios}");
        //    sw.WriteLine($@"Tempo para copiar o arquivo - {modelo.tempoCopy}");
        //    sw.WriteLine($@"Tempo do using workbook - {modelo.tempoUsingWorkbook}");
        //    sw.WriteLine($@"Tempo para inserir no banco com o número de série - {modelo.tempoInserirNumSerie}");
        //    sw.WriteLine($@"Tempo para gerar a APR com número de série - {modelo.tempoGerarComSerie}");
        //    sw.WriteLine($@"Tempo para gerar o arquivo PDF - {modelo.tempoGerarPDF}");
        //    sw.WriteLine($@"Tempo para salvar o workbook - {modelo.tempoSaveWorkbook}");
        //    sw.WriteLine($@"TEMPO GERAL - {modelo.tempoGeral }");
        //}

    }
}
