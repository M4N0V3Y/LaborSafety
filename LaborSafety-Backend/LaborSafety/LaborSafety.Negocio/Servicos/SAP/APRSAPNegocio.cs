using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Xml.Serialization;
using LaborSafety.Dominio.Modelos;
using LaborSafety.Dominio.Modelos.SAP;
using LaborSafety.Negocio.Interfaces;
using LaborSafety.Negocio.Interfaces.SAP;
using LaborSafety.Persistencia;
using LaborSafety.Persistencia.Interfaces;
using LaborSafety.Utils;
using LaborSafety.Utils.Constantes;

namespace LaborSafety.Negocio.Servicos.SAP
{
    public class APRSAPNegocio : IAPRSapNegocio
    {
        readonly ILocalInstalacaoPersistencia localInstalacaoPersistencia;
        readonly IDisciplinaPersistencia disciplinaPersistencia;
        readonly IAtividadePadraoPersistencia atividadePadraoPersistencia;
        readonly IAprPersistencia aprPersistencia;
        readonly IOperacaoAprPersistencia operacaoAprPersistencia;
        readonly IAprNegocio aprNegocio;
        readonly IInventariosAmbientePersistencia inventarioAmbientePersistencia;
        readonly IInventariosAtividadePersistencia inventarioAtividadePersistencia;
        readonly INrPersistencia nrPersistencia;
        readonly IEPIPersistencia epiPersistencia;
        readonly IProbabilidadePersistencia probabilidadePersistencia;
        readonly ISeveridadePersistencia severidadePersistencia;
        readonly IPesoPersistencia pesoPersistencia;
        readonly IDuracaoPersistencia duracaoPersistencia;
        readonly IRiscoPersistencia riscoPersistencia;
        readonly IBloqueioPersistencia bloqueioPersistencia;
        readonly IEPIRiscoInventarioAmbientePersistencia epiRiscoInventarioAmbientePersistencia;
        readonly IEPIRiscoInventarioAtividadePersistencia epiRiscoInventarioAtividadePersistencia;
        readonly ILogAprPersistencia logAprPersistencia;
        readonly IPessoaPersistencia pessoaPersistencia;

        public APRSAPNegocio(ILocalInstalacaoPersistencia localInstalacaoPersistencia, IDisciplinaPersistencia disciplinaPersistencia,
            IAtividadePadraoPersistencia atividadePadraoPersistencia, IAprPersistencia aprPersistencia, IOperacaoAprPersistencia operacaoAprPersistencia,
            IAprNegocio aprNegocio, IInventariosAmbientePersistencia inventarioAmbientePersistencia, IInventariosAtividadePersistencia inventarioAtividadePersistencia,
            INrPersistencia nrPersistencia, IEPIPersistencia epiPersistencia, IProbabilidadePersistencia probabilidadePersistencia,
            ISeveridadePersistencia severidadePersistencia, IPesoPersistencia pesoPersistencia, IDuracaoPersistencia duracaoPersistencia, IRiscoPersistencia riscoPersistencia,
            IBloqueioPersistencia bloqueioPersistencia, IEPIRiscoInventarioAmbientePersistencia epiRiscoInventarioAmbientePersistencia ,
            IEPIRiscoInventarioAtividadePersistencia epiRiscoInventarioAtividadePersistencia, ILogAprPersistencia logAprPersistencia, IPessoaPersistencia pessoaPersistencia)
        {
            this.localInstalacaoPersistencia = localInstalacaoPersistencia;
            this.disciplinaPersistencia = disciplinaPersistencia;
            this.atividadePadraoPersistencia = atividadePadraoPersistencia;
            this.aprPersistencia = aprPersistencia;
            this.operacaoAprPersistencia = operacaoAprPersistencia;
            this.aprNegocio = aprNegocio;
            this.inventarioAmbientePersistencia = inventarioAmbientePersistencia;
            this.inventarioAtividadePersistencia = inventarioAtividadePersistencia;
            this.nrPersistencia = nrPersistencia;
            this.epiPersistencia = epiPersistencia;
            this.probabilidadePersistencia = probabilidadePersistencia;
            this.severidadePersistencia = severidadePersistencia;
            this.pesoPersistencia = pesoPersistencia;
            this.duracaoPersistencia = duracaoPersistencia;
            this.riscoPersistencia = riscoPersistencia;
            this.bloqueioPersistencia = bloqueioPersistencia;
            this.epiRiscoInventarioAmbientePersistencia = epiRiscoInventarioAmbientePersistencia;
            this.epiRiscoInventarioAtividadePersistencia = epiRiscoInventarioAtividadePersistencia;
            this.logAprPersistencia = logAprPersistencia;
            this.pessoaPersistencia = pessoaPersistencia;
        }

        struct DadosOperacao
        {
            public long CodLI { get; set; }
            public long CodDisciplina { get; set; }
            public long CodAtividadePadrao { get; set; }

        }


        public APRSAPModelo LoadFromXMLString(string XmlInput)
        {
            using (var stringReader = new System.IO.StringReader(XmlInput))
            {
                var serializer = new XmlSerializer(typeof(APRSAPModelo));
                return serializer.Deserialize(stringReader) as APRSAPModelo;
            }
        }

        public APRSAPResponse ProcessaOrdem(APRSAPModelo modelo)
        {
            APRSAPResponse response = new APRSAPResponse();
            response.Itens = new List<APRItemSAPResponse>();

            this.ValidaModelo(modelo);
            var statusApr = modelo.Itens.First().Status_Ordem;

            using (var entities = new DB_APRPTEntities())
            {
                using (var transaction = entities.Database.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
                {
                    APRItemSAPResponse itemResponse = new APRItemSAPResponse();
                    itemResponse.Endereco_Rede = "";
                    APR apr = new APR();
                    try
                    {

                        if (statusApr == Enum.GetName(typeof(Constantes.StatusOrdem), Constantes.StatusOrdem.IMPR))
                        {
                            AtualizaStatusApr(modelo.Itens.First().Numero_Ordem, (long)Constantes.StatusOrdem.IMPR , entities);
                        }
                        else
                        {
                            if (statusApr == Enum.GetName(typeof(Constantes.StatusOrdem), Constantes.StatusOrdem.ASAP))
                            {
                                var aprBase = aprPersistencia.PesquisarPorOrdemManutencao(modelo.Itens.First().Numero_Ordem, entities);
                                apr = aprBase;
                                var aprBase64 = GerarAprSerializada(aprBase.NumeroSerie);
                                itemResponse.Endereco_Rede = aprBase64;
                                //EnviarDadosSAP(aprBase64, apr.OrdemManutencao);
                            }
                            else
                            {
                                //Verifica se já existe APR para ordem
                                var aprExistente = aprPersistencia.PesquisarPorOrdemManutencaoExistentesEInexistentes(modelo.Itens.First().Numero_Ordem, entities);
                                if (aprExistente == null)
                                {
                                    apr = aprPersistencia.InserirSomenteComNumeroSeriaViaSAP(entities);
                                    apr.OrdemManutencao = modelo.Itens.First().Numero_Ordem;
                                    apr.Descricao = modelo.Itens.First().Descricao_Ordem;
                                    apr.LocalInstalacao = modelo.Itens.First().Local_Instalacao;
                                }
                                else
                                {
                                    apr = aprExistente;
                                    apr.CodStatusAPR = (long)Constantes.StatusAPR.Criado;
                                    apr.LocalInstalacao = modelo.Itens.First().Local_Instalacao;
                                    apr.Descricao = modelo.Itens.First().Descricao_Ordem;
                                    this.DesativarOperacoesApr(apr,entities);
                                }
                                var maiorRiscoGeral = int.MinValue;
                                foreach (var item in modelo.Itens)
                                {
                                    OPERACAO_APR operacao = new OPERACAO_APR();
                                    //Realiza as validações dos valores dos itens
                                    DadosOperacao dadosOperacao = this.VerificaValoresNaBase(item, entities);
                                    //Adiciona a operação na APR
                                    this.AdicionaOperacaoAPR(apr, item, dadosOperacao, entities);
                                    //Calcula o valor do risco geral da APR

                                    // Lógica para obtenção do maior risco das operações calculadas
                                    var riscoCalculado = this.CalculaRiscoGeral(dadosOperacao, entities);
                                    if (maiorRiscoGeral < riscoCalculado)
                                    {
                                        maiorRiscoGeral = riscoCalculado;
                                    }
                                }
                                itemResponse.VRG = maiorRiscoGeral.ToString();
                                apr.RiscoGeral = maiorRiscoGeral;
                                apr.DataInicio = DateTime.Now;
                            }
                        }
                        entities.SaveChanges();
                        transaction.Commit();

                        //if (statusApr == Enum.GetName(typeof(Constantes.StatusOrdem), Constantes.StatusOrdem.ASAP_CALC))
                        if (statusApr.ToUpper() == Constantes.StatusAPRIntegracaoOrdem.EnviarRecalcular.ToUpper())
                        {
                            var aprBase64 = GerarAprSerializada(apr.NumeroSerie);
                            itemResponse.Endereco_Rede = aprBase64;
                            //EnviarDadosSAP(aprBase64, apr.OrdemManutencao);
                        }
                        itemResponse.Status = Constantes.StatusResponseIntegracao.S.ToString();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        itemResponse.Status = Constantes.StatusResponseIntegracao.E.ToString();
                        itemResponse.Descricao = ex.Message;

                        throw ex;
                    }
                    finally
                    {
                        itemResponse.Numero_Serie = $"{apr.NumeroSerie}";
                        response.Itens.Add(itemResponse);
                    }
                }
            }

            return response;
        }

        private void AtualizaStatusApr(string Numero_Ordem,long codStatusApr, DB_APRPTEntities entities)
        {
            aprPersistencia.EditarStatusApr(Numero_Ordem, codStatusApr, entities);
        } 

        private void ValidaModelo(APRSAPModelo modelo)
        {
            try
            {
                if (modelo == null)
                    throw new Exception("O modelo de APR não foi informado!");

                if (modelo.Itens == null)
                    throw new Exception("A(s) ordem(ordens) não foi(ram) informada(s)!");

                if (modelo.Itens.Count == 0)
                    throw new Exception("A(s) ordem(ordens) não foi(ram) informada(s)!");
                else
                {
                    foreach (var item in modelo.Itens)
                    {
                        if (String.IsNullOrEmpty(item.Numero_Ordem))
                            throw new Exception($"O número da ordem não foi informado!");

                        if (String.IsNullOrEmpty(item.Descricao_Ordem))
                            throw new Exception($"A descrição da ordem {item.Numero_Ordem} não foi informada!");

                        if (String.IsNullOrEmpty(item.Local_Instalacao))
                            throw new Exception($"O local de instalação da ordem {item.Numero_Ordem} não foi informado!");

                        if (String.IsNullOrEmpty(item.Operacao))
                            throw new Exception($"A operação da ordem {item.Numero_Ordem} não foi informada!");

                        if (String.IsNullOrEmpty(item.Descricao_Operacao))
                            throw new Exception($"A descrição da operação da ordem {item.Numero_Ordem} não foi informada!");

                        if (String.IsNullOrEmpty(item.Centro_Trabalho))
                            throw new Exception($"O centro de trabalho da ordem {item.Numero_Ordem} não foi informado!");

                        if (String.IsNullOrEmpty(item.Valor_Centro_Trabalho))
                            throw new Exception($"A característica do centro de trabalho da ordem {item.Numero_Ordem} não foi informada!");

                        if (String.IsNullOrEmpty(item.Chave_Modelo_Operacao))
                            throw new Exception($"A chave de modelo da ordem {item.Numero_Ordem} não foi informada!");

                        if (String.IsNullOrEmpty(item.Local_Instalacao_Operacao))
                            throw new Exception($"O local de instalação da operação da ordem {item.Numero_Ordem} não foi informado!");

                        if (String.IsNullOrEmpty(item.Status_Ordem))
                            throw new Exception($"O status da ordem {item.Numero_Ordem} não foi informado!");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private DadosOperacao VerificaValoresNaBase(APRItemSAPModelo item, DB_APRPTEntities entities)
        {
            DadosOperacao result = new DadosOperacao();

            //Verifica se o local de instalação existe na base
            var localExistente = this.localInstalacaoPersistencia.ListarLocalInstalacaoPorNome(item.Local_Instalacao_Operacao, entities);
            if (localExistente == null)
                throw new Exception($"O local informado para a ordem-operação {item.Numero_Ordem}-{item.Operacao} não existe na base de dados");
            else
                result.CodLI = localExistente.CodLocalInstalacao;

            //Verifica se a disciplina(Caracteristica da classe) existe na base
            var disciplinaExistente = this.disciplinaPersistencia.ListarDisciplinaPorNome(item.Valor_Centro_Trabalho, entities);
            if (disciplinaExistente == null)
                throw new Exception($"A característica da classe informada para a ordem-operação {item.Numero_Ordem}-{item.Operacao} não existe na base de dados");
            else
                result.CodDisciplina = disciplinaExistente.CodDisciplina;

            //Verifica se a atividade padrão (Chave modelo) existe na base
            var atvPadraoExistente = this.atividadePadraoPersistencia.ListarAtividadePorNome(item.Chave_Modelo_Operacao, entities);
            if (atvPadraoExistente == null)
                throw new Exception($"A atividade informada para a ordem-operação {item.Numero_Ordem}-{item.Operacao} não existe na base de dados");
            else
                result.CodAtividadePadrao = atvPadraoExistente.CodAtividadePadrao;

            return result;


        }
        private void DesativarOperacoesApr(APR apr, DB_APRPTEntities entities)
        {
            operacaoAprPersistencia.DesativarOperacoesDeApr(apr.CodAPR,entities);
        }

        private void AdicionaOperacaoAPR(APR apr, APRItemSAPModelo item, DadosOperacao dadosOperacao, DB_APRPTEntities entities)
        {
            //TODO: É NECESSÁRIO APAGAR TODAS AS OPERACOES E INSERIR DE NOVO

            OPERACAO_APR operacao = new OPERACAO_APR();

            operacao.CodAPR = apr.CodAPR;
            operacao.CodStatusAPR = (long)Constantes.StatusAPR.Criado;
            operacao.Codigo = item.Operacao;
            operacao.CodLI = dadosOperacao.CodLI;
            operacao.CodDisciplina = dadosOperacao.CodDisciplina;
            operacao.CodAtvPadrao = dadosOperacao.CodAtividadePadrao;
            operacao.Ativo = true;

            apr.OPERACAO_APR.Add(operacao);

            //Salva a operação no APR
            operacaoAprPersistencia.InsereOuEditaOperacaoAPR(apr.CodAPR, operacao, entities);

            //Salva os riscos

            //Riscos de Ambiente
            aprPersistencia.InserirRiscosAmbienteAPR(apr.CodAPR, dadosOperacao.CodLI, entities);
            aprPersistencia.InserirRiscosAtividadeAPR(apr.CodAPR, dadosOperacao.CodLI, dadosOperacao.CodDisciplina, dadosOperacao.CodAtividadePadrao, entities);

        }

        private int CalculaRiscoGeral(DadosOperacao dadosOperacao, DB_APRPTEntities entities)
        {
            FiltroUnicoInventarioAtividadeModelo filtroVRG = new FiltroUnicoInventarioAtividadeModelo();
            filtroVRG.AprAtiva = true;
            filtroVRG.CodAtividade = dadosOperacao.CodAtividadePadrao;
            filtroVRG.CodDisciplina = dadosOperacao.CodDisciplina;
            filtroVRG.CodLi = dadosOperacao.CodLI;

            int riscoGeral = aprNegocio.CalcularRiscoAprPorAtividadeDisciplinaLI(filtroVRG);
            return riscoGeral;
        }


        private string GerarAprSerializada(string numeroSerie)
        {
            string aprSerializada = "";
            try
            {
                var apr = aprPersistencia.PesquisarPorNumeroSerie(numeroSerie);
                var diretorioAprs = MontarCaminhoAprs(numeroSerie);
                GerarAPR(apr);
                GerarMapaDeBloqueio(apr.OrdemManutencao);
                var caminhoPdfsCriados = SelecionarArquivosParaAgrupar(diretorioAprs);
                var caminhoPdf = ConversaoPdfUtils.AgruparPdfs(caminhoPdfsCriados, diretorioAprs);
                ConversaoPdfUtils.DeletarPdfsTemporarios(caminhoPdfsCriados);
                aprSerializada = Convert.ToBase64String(File.ReadAllBytes(caminhoPdf));
                return aprSerializada;
            }

            catch
            {
                throw;
            }
        }

        private void GerarMapaDeBloqueio(string ordemManutencao)
        {
            List<string> ordensManutencao = new List<string>();
            ordensManutencao.Add(ordemManutencao);
            BloqueioNegocio bloqueioNegocio = new BloqueioNegocio(bloqueioPersistencia, aprPersistencia);
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
            AprNegocio aprNegocio = new AprNegocio(aprPersistencia, inventarioAmbientePersistencia, inventarioAtividadePersistencia, localInstalacaoPersistencia,
                nrPersistencia, epiPersistencia, probabilidadePersistencia, severidadePersistencia,
                atividadePadraoPersistencia, pesoPersistencia, duracaoPersistencia, disciplinaPersistencia,
                riscoPersistencia, bloqueioPersistencia, epiRiscoInventarioAmbientePersistencia, epiRiscoInventarioAtividadePersistencia, logAprPersistencia, pessoaPersistencia);

            aprNegocio.GerarApr(dadosAprModelo, apr);
        }
        private void EnviarDadosSAP(string pdfSerializado,string numeroOrdem)
        {
            try
            {
                //Instancia a classe que define o arquivo
                SVC_SAP.DT_Arquivo arquivoSerializado = new SVC_SAP.DT_Arquivo();
                arquivoSerializado.Nome_Arquivo = pdfSerializado;
                arquivoSerializado.Num_Ordem = numeroOrdem;

                //Cria as configurações do HTTP para a requisição
                BasicHttpBinding binding = new BasicHttpBinding();
                binding.Security.Mode = BasicHttpSecurityMode.TransportCredentialOnly;
                binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;

                //Define o endpoint e cria o client
                EndpointAddress endpoint = new EndpointAddress("http://tkcsac2.tkcsa.com.br:50600/XISOAPAdapter/MessageServlet?senderParty=&senderService=BS_APR_PT&receiverParty=&receiverService=&interface=SI_Arquivo_Out&interfaceNamespace=http://ternium.com.br/INT_FromERP_toAPR_PT");
                var wsclient = new SVC_SAP.SI_Arquivo_OutClient(binding, endpoint);

                //Informa as credenciais de acesso
                wsclient.ClientCredentials.UserName.UserName = ConfigurationManager.AppSettings["usuarioIntegracaoSAP"];
                wsclient.ClientCredentials.UserName.Password = ConfigurationManager.AppSettings["senhaUsuarioIntegracaoSAP"];

                //Realiza a comunicação
                wsclient.SI_Arquivo_Out(arquivoSerializado);

            }

            catch(Exception ex)
            {
                //throw ex;
                return;
            }
        }

        private List<string> SelecionarArquivosParaAgrupar(string diretorioApr)
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

        private string MontarCaminhoAprs(string numeroSerie)
        {
            var caminhoAprs = ArquivoDiretorioUtils.ObterDiretorioApr();
            var dataAtual = DateTime.Now.Date.ToString("dd/MM/yyy").Replace('/', '_');
            caminhoAprs = $"{caminhoAprs}/{dataAtual}";
            var caminhoAprDoDia = ArquivoDiretorioUtils.ConstruirDiretorio(caminhoAprs);
            caminhoAprDoDia = $"{caminhoAprDoDia}/{numeroSerie}/";
            if (Directory.Exists(caminhoAprDoDia))
            {
                Directory.Delete(caminhoAprDoDia, true);
            }
            caminhoAprDoDia = ArquivoDiretorioUtils.ConstruirDiretorio(caminhoAprDoDia);
            return caminhoAprDoDia;
        }
    }
}
