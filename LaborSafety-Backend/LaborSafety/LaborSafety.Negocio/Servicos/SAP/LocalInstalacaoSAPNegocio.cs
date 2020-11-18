using DocumentFormat.OpenXml.Presentation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using LaborSafety.Dominio.Modelos;
using LaborSafety.Dominio.Modelos.SAP;
using LaborSafety.Negocio.Interfaces.SAP;
using LaborSafety.Persistencia;
using LaborSafety.Persistencia.Interfaces;
using LaborSafety.Utils.Constantes;

namespace LaborSafety.Negocio.Servicos.SAP
{
    public class LocalInstalacaoSAPNegocio : ILocalInstalacaoSAPNegocio
    {
        readonly ILocalInstalacaoPersistencia localInstalacaoPersistencia;
        readonly IPerfilCatalogoPersistencia perfilCatalogoPersistencia;
        readonly IPesoPersistencia pesoPersistencia;
        readonly IInventariosAmbientePersistencia inventariosAmbientePersistencia;
        readonly IBloqueioPersistencia bloqueioPersistencia;

        public LocalInstalacaoSAPNegocio(ILocalInstalacaoPersistencia localInstalacaoPersistencia, 
            IPerfilCatalogoPersistencia perfilCatalogoPersistencia, IPesoPersistencia pesoPersistencia,
            IInventariosAmbientePersistencia inventariosAmbientePersistencia, IBloqueioPersistencia bloqueioPersistencia)
        {
            this.localInstalacaoPersistencia = localInstalacaoPersistencia;
            this.perfilCatalogoPersistencia = perfilCatalogoPersistencia;
            this.pesoPersistencia = pesoPersistencia;
            this.inventariosAmbientePersistencia = inventariosAmbientePersistencia;
            this.bloqueioPersistencia = bloqueioPersistencia;
        }

        public LocalInstalacaoSAPModelo LoadFromXMLString(string XmlInput)
        {
            using (var stringReader = new System.IO.StringReader(XmlInput))
            {
                var serializer = new XmlSerializer(typeof(LocalInstalacaoSAPModelo));
                return serializer.Deserialize(stringReader) as LocalInstalacaoSAPModelo;
            }
        }

        private class ControleTempoProcessamento
        {
            public string nomeControle;
            public List<TempoProcessamento> temposProcessamento = new List<TempoProcessamento>();
            public long tempoTotal;
        }

        private class TempoProcessamento
        {
            public string nomeMetodo;
            public long tempoProcessamento;
        }

        private void AdicionaTempoProcessamento (string NomeMetodo, long tempoProcessamentoPreMetodo, long tempoProcessamentoPosMetodo,
            ControleTempoProcessamento controleTempo)
        {
            TempoProcessamento tmpProcessamento = new TempoProcessamento();
            tmpProcessamento.nomeMetodo = NomeMetodo;
            tmpProcessamento.tempoProcessamento = (tempoProcessamentoPosMetodo - tempoProcessamentoPreMetodo) / 1000;

            controleTempo.temposProcessamento.Add(tmpProcessamento);
        }

        private class NiveisLI
        {
            public List<string> niveisDois = new List<string>();
            public List<string> niveisTres = new List<string>();
            public List<string> niveisQuatro = new List<string>();
            public List<string> niveisCinco = new List<string>();
            public List<string> niveisSeis = new List<string>();
        }

        public LocalInstalacaoSAPResponse ProcessaLocalInstalacao(LocalInstalacaoSAPModelo modelo)
        {
            LocalInstalacaoSAPResponse response = new LocalInstalacaoSAPResponse();
            response.Itens = new List<LocalInstalacaoItemSAPResponse>();

            this.ValidaModelo(modelo);

            using (var entities = new DB_APRPTEntities())
            {
                using (var transaction = entities.Database.BeginTransaction(System.Data.IsolationLevel.Serializable))
                {
                    entities.Database.CommandTimeout = 1200;

                    string codigoBloqueioLocal;
                    NiveisLI niveisLI = new NiveisLI();
                    List<PERFIL_CATALOGO> perfisValidados = new List<PERFIL_CATALOGO>();
                    List<string> nomesPerfisValidados = new List<string>();
                    List<LOCAL_INSTALACAO> locaisValidados = new List<LOCAL_INSTALACAO>();
                    List<string> nomesLocaisValidados = new List<string>();

                    try
                    {
                        //Valida o local recebido
                        PERFIL_CATALOGO perfilCatalogoVazio =
                            perfilCatalogoPersistencia.ListarPerfilCatalogoPorId((long)Constantes.PerfilCatalogo.SEM_PERFIL_CATALOGO, entities);

                        nomesPerfisValidados.Add(perfilCatalogoVazio.Codigo);
                        perfisValidados.Add(perfilCatalogoVazio);

                        foreach (var item in modelo.Itens)
                        {
                            bool bloqueiosDeletados = false;
                            BLOQUEIO bloqueio = new BLOQUEIO();
                            PERFIL_CATALOGO perfilCatalogo = new PERFIL_CATALOGO();
                            LOCAL_INSTALACAO localExistente;
                            string nomeLIAtual = item.Local_Instalacao;
                            string descricaoLI = modelo.Itens[0].Descricao_Local_Instalacao;

                            //Se o local já foi validado, busca-o na lista
                            if (nomesLocaisValidados.Contains(nomeLIAtual))
                                localExistente = locaisValidados.Where(x => x.Nome == nomeLIAtual).First();
                            else
                            {
                                localExistente = localInstalacaoPersistencia.ListarLocalInstalacaoPorNome(nomeLIAtual, entities);

                                //Validacao de perfil de catalogo

                                if (String.IsNullOrEmpty(item.Perfil_Catalogo))
                                    perfilCatalogo = perfilCatalogoVazio;
                                else if (!nomesPerfisValidados.Contains(item.Perfil_Catalogo))
                                {
                                    perfilCatalogo = this.ValidaPerfilCatalogo(item, entities);
                                    nomesPerfisValidados.Add(perfilCatalogo.Codigo);
                                    perfisValidados.Add(perfilCatalogo);
                                }
                                else
                                    perfilCatalogo = perfisValidados.Where(x => x.Codigo == item.Perfil_Catalogo).First();

                                //Insere ou edita o local
                                if (localExistente == null)
                                    localExistente = this.ValidaEInsereLIELocalBase(item, perfilCatalogo.CodPerfilCatalogo, niveisLI, entities);
                                else
                                    localExistente = this.EditaLocalExistente(localExistente, descricaoLI, perfilCatalogo.CodPerfilCatalogo, 
                                        bloqueiosDeletados, entities);

                                nomesLocaisValidados.Add(nomeLIAtual);
                                locaisValidados.Add(localExistente);
                            }

                            if (String.IsNullOrEmpty(item.Descricao_Caracteristica) == false)
                            {
                                this.ValidaDescricaoClasse(item);

                                //Armazena a chave da tabela relacional
                                codigoBloqueioLocal = item.Classe_Local_Instalacao;

                                //Valida e insere características de bloqueio
                                if (item.Descricao_Caracteristica.ToUpper() ==
                                    Constantes.DescricaoCaracteristicaClassesLocalInstalacao.FaixaDePeso.ToUpper())
                                {
                                    var pesoExistente = this.pesoPersistencia.ListarPesoPorNome(item.Valor_Caracteristica, entities);
                                    if (pesoExistente == null)
                                        throw new Exception($"A faixa de peso {item.Valor_Caracteristica} da classse {item.Classe_Local_Instalacao}" +
                                            $" não existe na base de dados.");

                                    localExistente.CodPeso = pesoExistente.CodPeso;

                                    entities.SaveChanges();
                                }
                                else
                                {
                                    //Inserção de classes de bloqueio
                                    if (item.Descricao_Caracteristica.ToUpper()
                                        == Constantes.DescricaoCaracteristicaClassesLocalInstalacao.TagDeBloqueio.ToUpper())
                                    {
                                        TAG_KKS_BLOQUEIO tagKKS = new TAG_KKS_BLOQUEIO();
                                        tagKKS.Codigo = item.Valor_Caracteristica;
                                        tagKKS.Nome = item.Valor_Caracteristica;

                                        bloqueio.CodTagKKSBloqueio = InsereOuEditaCaracteristicaBloqueio(item, entities);

                                    }
                                    else if (item.Descricao_Caracteristica.ToUpper()
                                        == Constantes.DescricaoCaracteristicaClassesLocalInstalacao.Area.ToUpper())
                                    {
                                        AREA area = new AREA();
                                        area.Codigo = item.Valor_Caracteristica;
                                        area.Nome = item.Valor_Caracteristica;

                                        bloqueio.CodArea = InsereOuEditaCaracteristicaBloqueio(item, entities);
                                    }
                                    else if (item.Descricao_Caracteristica.ToUpper() ==
                                        Constantes.DescricaoCaracteristicaClassesLocalInstalacao.DispositivoBloqueio.ToUpper())
                                    {
                                        DISPOSITIVO_BLOQUEIO dspBloqueio = new DISPOSITIVO_BLOQUEIO();
                                        dspBloqueio.Codigo = item.Valor_Caracteristica;
                                        dspBloqueio.Nome = item.Valor_Caracteristica;

                                        bloqueio.CodDispositivoBloqueio = InsereOuEditaCaracteristicaBloqueio(item, entities);
                                    }
                                    else if (item.Descricao_Caracteristica.ToUpper() ==
                                        Constantes.DescricaoCaracteristicaClassesLocalInstalacao.TipoDeEnergia.ToUpper())
                                    {
                                        TIPO_ENERGIA_BLOQUEIO tipoEnergia = new TIPO_ENERGIA_BLOQUEIO();
                                        tipoEnergia.Codigo = item.Valor_Caracteristica;
                                        tipoEnergia.Nome = item.Valor_Caracteristica;

                                        bloqueio.CodTipoEnergiaBloqueio = InsereOuEditaCaracteristicaBloqueio(item, entities);
                                    }
                                    else if (item.Descricao_Caracteristica.ToUpper() ==
                                        Constantes.DescricaoCaracteristicaClassesLocalInstalacao.TagLocalBloqueio.ToUpper())
                                    {
                                        LOCAL_A_BLOQUEAR localABloquear = new LOCAL_A_BLOQUEAR();
                                        localABloquear.Nome = item.Valor_Caracteristica;

                                        bloqueio.CodLocalABloquear = InsereOuEditaCaracteristicaBloqueio(item, entities);
                                    }

                                    //Se já existe um conjunto associado, edita. Senão, insere
                                    BLOQUEIO_LOCAL_INSTALACAO bloqueioLocalInstalacaoExistente =
                                        this.bloqueioPersistencia.ListarBloqueioLocalInstalacaoPorNomeELocal(codigoBloqueioLocal,
                                        localExistente.CodLocalInstalacao, entities);

                                    if (bloqueioLocalInstalacaoExistente == null)
                                    {
                                        //Insere o bloqueio
                                        bloqueio = bloqueioPersistencia.InserirBloqueio(bloqueio, entities);

                                        //Insere Bloqueio_Local_Instalacao
                                        bloqueioLocalInstalacaoExistente = this.bloqueioPersistencia.InserirBloqueioLocalInstalacao(bloqueio.CodBloqueio,
                                            localExistente.CodLocalInstalacao, codigoBloqueioLocal, entities);
                                    }
                                    else
                                    {
                                        var bloqueioExistente = bloqueioPersistencia.ListarBLoqueioPorCodigo(bloqueioLocalInstalacaoExistente.CodBloqueio,
                                            entities);

                                        if (bloqueio.CodDispositivoBloqueio != 0)
                                            bloqueioExistente.CodDispositivoBloqueio = bloqueio.CodDispositivoBloqueio;

                                        if (bloqueio.CodLocalABloquear != 0)
                                            bloqueioExistente.CodLocalABloquear = bloqueio.CodLocalABloquear;

                                        if (bloqueio.CodTagKKSBloqueio != 0)
                                            bloqueioExistente.CodTagKKSBloqueio = bloqueio.CodTagKKSBloqueio;

                                        if (bloqueio.CodTipoEnergiaBloqueio != 0)
                                            bloqueioExistente.CodTipoEnergiaBloqueio = bloqueio.CodTipoEnergiaBloqueio;

                                        if (bloqueio.CodArea != 0)
                                            bloqueioExistente.CodArea = bloqueio.CodArea;

                                        bloqueio.CodBloqueio = bloqueioLocalInstalacaoExistente.CodBloqueio;

                                        this.bloqueioPersistencia.EditaBloqueio(bloqueioExistente, entities);
                                    }

                                    entities.SaveChanges();
                                }
                            }

                            entities.SaveChanges();
                        }

                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }

                    transaction.Commit();
                }

                //stopwatch.Stop();

                LocalInstalacaoItemSAPResponse itemResponse = new LocalInstalacaoItemSAPResponse();
                itemResponse.Status = Constantes.StatusResponseIntegracao.S.ToString();

                response.Itens.Add(itemResponse);

                return response;
            }
        }

        //public LocalInstalacaoSAPResponse ProcessaLocalInstalacao2(LocalInstalacaoSAPModelo modelo)
        //{
        //    LocalInstalacaoSAPResponse response = new LocalInstalacaoSAPResponse();
        //    response.Itens = new List<LocalInstalacaoItemSAPResponse>();

        //    this.ValidaModelo(modelo);

        //    long tempoPreMetodo = 0;
        //    Stopwatch stopwatchGeral = new Stopwatch();
        //    stopwatchGeral.Start();

        //    List<ControleTempoProcessamento> controlesTempoIteracoes = new List<ControleTempoProcessamento>();

        //    using (var entities = new DB_APRPT_V1_ESPEntities())
        //    {
        //        entities.Database.CommandTimeout = 9999;

        //        using (var transaction = entities.Database.BeginTransaction())
        //        {
        //            try
        //            {
        //                string nomeLIOriginal = modelo.Itens[0].Local_Instalacao;
        //                string nomeLIAtual = "";
        //                string codigoBloqueioLocal;
        //                bool bloqueiosDeletados = false;
        //                int contador = 0;

        //                foreach (var item in modelo.Itens)
        //                {
        //                    nomeLIAtual = item.Local_Instalacao;

        //                    Stopwatch stopwatch = new Stopwatch();
        //                    stopwatch.Start();

        //                    contador += 1;

        //                    ControleTempoProcessamento controleTempoProcessamento = new ControleTempoProcessamento();
        //                    controleTempoProcessamento.nomeControle = $"Iteracao do item {contador} ";

        //                    //Associa as classes de bloqueio
        //                    BLOQUEIO bloqueio = new BLOQUEIO();

        //                    tempoPreMetodo = stopwatch.ElapsedMilliseconds;

        //                    this.ValidaLocalInstalacao(item);

        //                    this.AdicionaTempoProcessamento("ValidaLocalInstalacao", tempoPreMetodo, stopwatch.ElapsedMilliseconds, controleTempoProcessamento);

        //                    nomeLIAtual = item.Local_Instalacao;

        //                    PERFIL_CATALOGO perfilCatalogo = new PERFIL_CATALOGO();

        //                    if (String.IsNullOrEmpty(item.Perfil_Catalogo) == true)
        //                    {
        //                        tempoPreMetodo = stopwatch.ElapsedMilliseconds;

        //                        perfilCatalogo = perfilCatalogoPersistencia.ListarPerfilCatalogoPorId((long)Constantes.PerfilCatalogo.SEM_PERFIL_CATALOGO, entities);

        //                        this.AdicionaTempoProcessamento("ListarPerfilCatalogoPorId", tempoPreMetodo,stopwatch.ElapsedMilliseconds, controleTempoProcessamento);
        //                    }
        //                    else
        //                    {
        //                        tempoPreMetodo = stopwatch.ElapsedMilliseconds;

        //                        perfilCatalogo = this.ValidaPerfilCatalogo(item, entities);

        //                        this.AdicionaTempoProcessamento("ValidaPerfilCatalogo", tempoPreMetodo,stopwatch.ElapsedMilliseconds, controleTempoProcessamento);
        //                    }

        //                    //Validações de local
        //                    tempoPreMetodo = stopwatch.ElapsedMilliseconds;

        //                    this.ValidaNiveisLocal(item.Local_Instalacao, entities);

        //                    this.AdicionaTempoProcessamento("ValidaNiveisLocal", tempoPreMetodo, stopwatch.ElapsedMilliseconds, controleTempoProcessamento);

        //                    tempoPreMetodo = stopwatch.ElapsedMilliseconds;

        //                    this.ValidaEInsereLocalBase(item.Local_Instalacao, entities);

        //                    this.AdicionaTempoProcessamento("ValidaEInsereLocalBase", tempoPreMetodo, stopwatch.ElapsedMilliseconds, controleTempoProcessamento);

        //                    tempoPreMetodo = stopwatch.ElapsedMilliseconds;

        //                    //Verifica se o local existe na base
        //                    var localExistente = localInstalacaoPersistencia.ListarLocalInstalacaoPorNome(nomeLIAtual, entities);

        //                    this.AdicionaTempoProcessamento("ListarLocalInstalacaoPorNome", tempoPreMetodo, stopwatch.ElapsedMilliseconds, controleTempoProcessamento);

        //                    if (localExistente == null)
        //                    {
        //                        tempoPreMetodo = stopwatch.ElapsedMilliseconds;

        //                        localExistente = this.InsereLocalInstalacao(item, perfilCatalogo.CodPerfilCatalogo, nomeLIAtual, entities);

        //                        this.AdicionaTempoProcessamento("InsereLocalInstalacao", tempoPreMetodo, stopwatch.ElapsedMilliseconds, controleTempoProcessamento);

        //                    }
        //                    else
        //                    {
        //                        localExistente.Nome = item.Local_Instalacao;
        //                        localExistente.Descricao = item.Descricao_Local_Instalacao;

        //                        tempoPreMetodo = stopwatch.ElapsedMilliseconds;

        //                        localExistente = this.EditaLocalExistente(localExistente, perfilCatalogo.CodPerfilCatalogo, bloqueiosDeletados, entities);

        //                        this.AdicionaTempoProcessamento("EditaLocalExistente", tempoPreMetodo, stopwatch.ElapsedMilliseconds, controleTempoProcessamento);

        //                        bloqueiosDeletados = true;
        //                    }

        //                    //if(String.IsNullOrEmpty(item.Classe_Local_Instalacao) == false)
        //                    if (String.IsNullOrEmpty(item.Descricao_Caracteristica) == false)
        //                    {
        //                        tempoPreMetodo = stopwatch.ElapsedMilliseconds;

        //                        //this.ValidaCaracteristicaClasse(item);
        //                        this.ValidaDescricaoClasse(item);

        //                        this.AdicionaTempoProcessamento("ValidaDescricaoClasse", tempoPreMetodo, stopwatch.ElapsedMilliseconds, controleTempoProcessamento);

        //                        //Armazena a chave da tabela relacional
        //                        codigoBloqueioLocal = item.Classe_Local_Instalacao;

        //                        //Valida e insere características de bloqueio
        //                        if (item.Descricao_Caracteristica.ToUpper() == 
        //                            Constantes.DescricaoCaracteristicaClassesLocalInstalacao.FaixaDePeso.ToUpper())
        //                        {
        //                            tempoPreMetodo = stopwatch.ElapsedMilliseconds;

        //                            var pesoExistente = this.pesoPersistencia.ListarPesoPorNome(item.Valor_Caracteristica, entities);

        //                            this.AdicionaTempoProcessamento("ListarPesoPorNome", tempoPreMetodo, stopwatch.ElapsedMilliseconds, controleTempoProcessamento);

        //                            if (pesoExistente == null)
        //                                throw new Exception($"A faixa de peso {item.Valor_Caracteristica} da classse {item.Classe_Local_Instalacao}" +
        //                                    $" não existe na base de dados.");

        //                            localExistente.CodPeso = pesoExistente.CodPeso;

        //                            //entities.ChangeTracker.DetectChanges();
        //                            entities.SaveChanges();
        //                        }
        //                        else
        //                        {
        //                            //Inserção de classes de bloqueio
        //                            if (item.Descricao_Caracteristica.ToUpper() 
        //                                == Constantes.DescricaoCaracteristicaClassesLocalInstalacao.TagDeBloqueio.ToUpper())
        //                            {
        //                                TAG_KKS_BLOQUEIO tagKKS = new TAG_KKS_BLOQUEIO();
        //                                tagKKS.Codigo = item.Valor_Caracteristica;
        //                                tagKKS.Nome = item.Valor_Caracteristica;

        //                                /*
        //                                if (String.IsNullOrEmpty(item.Valor_Caracteristica))
        //                                {
        //                                    tagKKS.Codigo = Constantes.TagKKsBloqueio.SEM_TAG_KKS_BLOQUEIO.ToString();
        //                                    tagKKS.Nome = Constantes.TagKKsBloqueio.SEM_TAG_KKS_BLOQUEIO.ToString();
        //                                }
        //                                else
        //                                {
        //                                    tagKKS.Codigo = item.Valor_Caracteristica;
        //                                    tagKKS.Nome = item.Valor_Caracteristica;
        //                                }*/

        //                                bloqueio.CodTagKKSBloqueio = InsereOuEditaCaracteristicaBloqueio(stopwatch, controleTempoProcessamento, item, entities);

        //                                /*
        //                                bloqueio.CodArea = (long)Constantes.Area.SEM_AREA;
        //                                bloqueio.CodDispositivoBloqueio = (long)Constantes.DispositivoBloqueio.SEM_DISPOSITIVO_BLOQUEIO;
        //                                bloqueio.CodTipoEnergiaBloqueio = (long)Constantes.TipoEnergiaBloqueio.SEM_TIPO_ENERGIA_BLOQUEIO;
        //                                bloqueio.CodLocalABloquear = (long)Constantes.LocalABloquear.SEM_LOCAL_A_BLOQUEAR;
        //                                */


        //                            }
        //                            else if (item.Descricao_Caracteristica.ToUpper() 
        //                                == Constantes.DescricaoCaracteristicaClassesLocalInstalacao.Area.ToUpper())
        //                            {
        //                                AREA area = new AREA();
        //                                area.Codigo = item.Valor_Caracteristica;
        //                                area.Nome = item.Valor_Caracteristica;

        //                                bloqueio.CodArea = InsereOuEditaCaracteristicaBloqueio(stopwatch, controleTempoProcessamento, item, entities);

        //                                /*
        //                                bloqueio.CodDispositivoBloqueio = (long)Constantes.DispositivoBloqueio.SEM_DISPOSITIVO_BLOQUEIO;
        //                                bloqueio.CodTipoEnergiaBloqueio = (long)Constantes.TipoEnergiaBloqueio.SEM_TIPO_ENERGIA_BLOQUEIO;
        //                                bloqueio.CodLocalABloquear = (long)Constantes.LocalABloquear.SEM_LOCAL_A_BLOQUEAR;
        //                                bloqueio.CodTagKKSBloqueio = (long)Constantes.TagKKsBloqueio.SEM_TAG_KKS_BLOQUEIO;
        //                                */
        //                            }
        //                            else if (item.Descricao_Caracteristica.ToUpper() == 
        //                                Constantes.DescricaoCaracteristicaClassesLocalInstalacao.DispositivoBloqueio.ToUpper())
        //                            {
        //                                DISPOSITIVO_BLOQUEIO dspBloqueio = new DISPOSITIVO_BLOQUEIO();
        //                                dspBloqueio.Codigo = item.Valor_Caracteristica;
        //                                dspBloqueio.Nome = item.Valor_Caracteristica;

        //                                bloqueio.CodDispositivoBloqueio = InsereOuEditaCaracteristicaBloqueio(stopwatch, controleTempoProcessamento, item, entities);

        //                                /*
        //                                bloqueio.CodArea = (long)Constantes.Area.SEM_AREA;
        //                                bloqueio.CodTipoEnergiaBloqueio = (long)Constantes.TipoEnergiaBloqueio.SEM_TIPO_ENERGIA_BLOQUEIO;
        //                                bloqueio.CodLocalABloquear = (long)Constantes.LocalABloquear.SEM_LOCAL_A_BLOQUEAR;
        //                                bloqueio.CodTagKKSBloqueio = (long)Constantes.TagKKsBloqueio.SEM_TAG_KKS_BLOQUEIO;
        //                                */
        //                            }
        //                            else if (item.Descricao_Caracteristica.ToUpper() == 
        //                                Constantes.DescricaoCaracteristicaClassesLocalInstalacao.TipoDeEnergia.ToUpper())
        //                            {
        //                                TIPO_ENERGIA_BLOQUEIO tipoEnergia = new TIPO_ENERGIA_BLOQUEIO();
        //                                tipoEnergia.Codigo = item.Valor_Caracteristica;
        //                                tipoEnergia.Nome = item.Valor_Caracteristica;

        //                                bloqueio.CodTipoEnergiaBloqueio = InsereOuEditaCaracteristicaBloqueio(stopwatch, controleTempoProcessamento, item, entities);

        //                                /*
        //                                bloqueio.CodDispositivoBloqueio = (long)Constantes.DispositivoBloqueio.SEM_DISPOSITIVO_BLOQUEIO;
        //                                bloqueio.CodArea = (long)Constantes.Area.SEM_AREA;
        //                                bloqueio.CodLocalABloquear = (long)Constantes.LocalABloquear.SEM_LOCAL_A_BLOQUEAR;
        //                                bloqueio.CodTagKKSBloqueio = (long)Constantes.TagKKsBloqueio.SEM_TAG_KKS_BLOQUEIO;
        //                                */
        //                            }
        //                            else if (item.Descricao_Caracteristica.ToUpper() == 
        //                                Constantes.DescricaoCaracteristicaClassesLocalInstalacao.TagLocalBloqueio.ToUpper())
        //                            {
        //                                LOCAL_A_BLOQUEAR localABloquear = new LOCAL_A_BLOQUEAR();
        //                                localABloquear.Nome = item.Valor_Caracteristica;

        //                                bloqueio.CodLocalABloquear = InsereOuEditaCaracteristicaBloqueio(stopwatch, controleTempoProcessamento, item, entities);

        //                                /*
        //                                bloqueio.CodDispositivoBloqueio = (long)Constantes.DispositivoBloqueio.SEM_DISPOSITIVO_BLOQUEIO;
        //                                bloqueio.CodArea = (long)Constantes.Area.SEM_AREA;
        //                                bloqueio.CodTipoEnergiaBloqueio = (long)Constantes.TipoEnergiaBloqueio.SEM_TIPO_ENERGIA_BLOQUEIO;
        //                                bloqueio.CodTagKKSBloqueio = (long)Constantes.TagKKsBloqueio.SEM_TAG_KKS_BLOQUEIO;
        //                                */
        //                            }

        //                            tempoPreMetodo = stopwatch.ElapsedMilliseconds;

        //                            //Se já existe um conjunto associado, edita. Senão, insere
        //                            BLOQUEIO_LOCAL_INSTALACAO bloqueioLocalInstalacaoExistente =
        //                                this.bloqueioPersistencia.ListarBloqueioLocalInstalacaoPorNomeELocal(codigoBloqueioLocal,
        //                                localExistente.CodLocalInstalacao, entities);

        //                            this.AdicionaTempoProcessamento("ListarBloqueioLocalInstalacaoPorNomeELocal", tempoPreMetodo, 
        //                                stopwatch.ElapsedMilliseconds, controleTempoProcessamento);


        //                            if (bloqueioLocalInstalacaoExistente == null)
        //                            {
        //                                tempoPreMetodo = stopwatch.ElapsedMilliseconds;

        //                                //Insere o bloqueio
        //                                bloqueio = bloqueioPersistencia.InserirBloqueio(bloqueio, entities);

        //                                this.AdicionaTempoProcessamento("InserirBloqueio", tempoPreMetodo,
        //                             stopwatch.ElapsedMilliseconds, controleTempoProcessamento);

        //                                tempoPreMetodo = stopwatch.ElapsedMilliseconds;

        //                                //Insere Bloqueio_Local_Instalacao
        //                                bloqueioLocalInstalacaoExistente = this.bloqueioPersistencia.InserirBloqueioLocalInstalacao(bloqueio.CodBloqueio,
        //                                    localExistente.CodLocalInstalacao, codigoBloqueioLocal, entities);

        //                                this.AdicionaTempoProcessamento("InserirBloqueioLocalInstalacao", tempoPreMetodo,
        //                             stopwatch.ElapsedMilliseconds, controleTempoProcessamento);

        //                            }
        //                            else
        //                            {
        //                                //Busca o bloqueio
        //                                /*
        //                                var bloqueioExistente = this.bloqueioPersistencia.ListarBLoqueioPorCodigo(bloqueioLocalInstalacao.CodBloqueio, 
        //                                    entities);

        //                                bloqueioExistente.CodArea = bloqueio.CodArea;
        //                                bloqueioExistente.CodDispositivoBloqueio = bloqueio.CodDispositivoBloqueio;
        //                                bloqueioExistente.CodLocalABloquear = bloqueio.CodLocalABloquear;
        //                                bloqueioExistente.CodTagKKSBloqueio = bloqueio.CodTagKKSBloqueio;
        //                                bloqueioExistente.CodTipoEnergiaBloqueio = bloqueio.CodTipoEnergiaBloqueio;
        //                                */

        //                                //Edita o bloqueio existente

        //                                tempoPreMetodo = stopwatch.ElapsedMilliseconds;

        //                                var bloqueioExistente = bloqueioPersistencia.ListarBLoqueioPorCodigo(bloqueioLocalInstalacaoExistente.CodBloqueio, entities);

        //                                this.AdicionaTempoProcessamento("ListarBLoqueioPorCodigo", tempoPreMetodo, stopwatch.ElapsedMilliseconds,
        //                                    controleTempoProcessamento);

        //                                if (bloqueio.CodDispositivoBloqueio != 0)
        //                                    bloqueioExistente.CodDispositivoBloqueio = bloqueio.CodDispositivoBloqueio;

        //                                if (bloqueio.CodLocalABloquear != 0)
        //                                    bloqueioExistente.CodLocalABloquear = bloqueio.CodLocalABloquear;

        //                                if (bloqueio.CodTagKKSBloqueio != 0)
        //                                    bloqueioExistente.CodTagKKSBloqueio = bloqueio.CodTagKKSBloqueio;

        //                                if (bloqueio.CodTipoEnergiaBloqueio != 0)
        //                                    bloqueioExistente.CodTipoEnergiaBloqueio = bloqueio.CodTipoEnergiaBloqueio;

        //                                if (bloqueio.CodArea != 0)
        //                                    bloqueioExistente.CodArea = bloqueio.CodArea;

        //                                bloqueio.CodBloqueio = bloqueioLocalInstalacaoExistente.CodBloqueio;

        //                                tempoPreMetodo = stopwatch.ElapsedMilliseconds;

        //                                this.bloqueioPersistencia.EditaBloqueio(bloqueioExistente, entities);

        //                                this.AdicionaTempoProcessamento("EditaBloqueio", tempoPreMetodo, stopwatch.ElapsedMilliseconds,
        //                                   controleTempoProcessamento);
        //                            }

        //                            tempoPreMetodo = stopwatch.ElapsedMilliseconds;

        //                            //entities.ChangeTracker.DetectChanges();
        //                            entities.SaveChanges();

        //                            this.AdicionaTempoProcessamento("SaveChanges", tempoPreMetodo, stopwatch.ElapsedMilliseconds, controleTempoProcessamento);

        //                        }
        //                    }


        //                    controleTempoProcessamento.tempoTotal += stopwatch.ElapsedMilliseconds / 1000;
        //                    stopwatch.Stop();
        //                    controlesTempoIteracoes.Add(controleTempoProcessamento);
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                transaction.Rollback();
        //                throw ex;
        //            }

        //            transaction.Commit();
        //        }
        //    }

        //    stopwatchGeral.Stop();

        //    long tempoTotalExecucao = stopwatchGeral.ElapsedMilliseconds / 10000;
        //    var teste = controlesTempoIteracoes[0].nomeControle;

        //    LocalInstalacaoItemSAPResponse itemResponse = new LocalInstalacaoItemSAPResponse();
        //    itemResponse.Status = Constantes.StatusResponseIntegracao.S.ToString();

        //    response.Itens.Add(itemResponse);

        //    return response;
        //}

        private void ValidaModelo(LocalInstalacaoSAPModelo modelo)
        {
            try
            {
                if (modelo == null)
                    throw new Exception("O modelo de local de instalacao não foi informado!");

                if (modelo.Itens == null)
                    throw new Exception("O(s) local(locais) de instalação não foi(ram) informado(s)!");

                if (modelo.Itens.Count == 0)
                    throw new Exception("O(s) local(locais) de instalação não foi(ram) informado(s)!");
                else
                {
                    foreach (var item in modelo.Itens)
                    {
                        if (String.IsNullOrEmpty(item.Local_Instalacao))
                            throw new Exception($"O local de instalação  não foi informado!");

                        if (String.IsNullOrEmpty(item.Descricao_Local_Instalacao))
                            throw new Exception($"A descrição do local de instalação {item.Local_Instalacao} não foi informada!");

                        /*
                        if (String.IsNullOrEmpty(item.Perfil_Catalogo))
                            throw new Exception($"O perfil de catálogo do local de instalação {item.Local_Instalacao} não foi informado!");

                        if (String.IsNullOrEmpty(item.Descricao_Perfil_Catalogo))
                            throw new Exception($"A descrição do perfil de catálogo  do local de instalação {item.Local_Instalacao} não foi informada!");

                        if (String.IsNullOrEmpty(item.Classe_Local_Instalacao))
                            throw new Exception($"A classe do local de instalação {item.Local_Instalacao} não foi informada!");

                        if (String.IsNullOrEmpty(item.Descricao_Classe))
                            throw new Exception($"A descrição da classe do local de instalação {item.Local_Instalacao} não foi informada!");

                        if (String.IsNullOrEmpty(item.Caracteristica_Classe))
                            throw new Exception($"A característica da classe do local de instalação {item.Local_Instalacao} não foi informada!");

                        if (String.IsNullOrEmpty(item.Descricao_Caracteristica))
                            throw new Exception($"A descrição da característica do local de instalação {item.Local_Instalacao} não foi informada!");

                        if (String.IsNullOrEmpty(item.Valor_Caracteristica))
                            throw new Exception($"O valor da característica do local de instalação {item.Local_Instalacao} não foi informado!");
                      */

                        if (!String.IsNullOrEmpty(item.Classe_Local_Instalacao) && (String.IsNullOrEmpty(item.Caracteristica_Classe) ||
                            String.IsNullOrEmpty(item.Valor_Caracteristica)))
                            throw new Exception($"A classe {item.Classe_Local_Instalacao} foi informada, mas a sua característica e/ou " +
                                $"sua descrição e/ou seu valor não foram informados.");

                        /*if (!String.IsNullOrEmpty(item.Descricao_Caracteristica) && String.IsNullOrEmpty(item.Valor_Caracteristica))
                            throw new Exception($"Foi informada uma descrição de característica '{item.Descricao_Caracteristica}' sem" +
                                $"o seu respectivo valor.");
                        */

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private LOCAL_INSTALACAO CriaEstruturaLI(string nomeLI)
        {
            string[] niveis = nomeLI.Split('-');

            LOCAL_INSTALACAO LI = new LOCAL_INSTALACAO();

            if (niveis.Count() == 1)
            {
                LI.N1 = niveis[0];
            }
            else if (niveis.Count() == 2)
            {
                LI.N1 = niveis[0];
                LI.N2 = niveis[1];
            }
            else if (niveis.Count() == 3)
            {
                LI.N1 = niveis[0];
                LI.N2 = niveis[1];
                LI.N3 = niveis[2];
            }
            else if (niveis.Count() == 4)
            {
                LI.N1 = niveis[0];
                LI.N2 = niveis[1];
                LI.N3 = niveis[2];
                LI.N4 = niveis[3];
            }
            else if (niveis.Count() == 5)
            {
                LI.N1 = niveis[0];
                LI.N2 = niveis[1];
                LI.N3 = niveis[2];
                LI.N4 = niveis[3];
                LI.N5 = niveis[4];
            }
            else if (niveis.Count() == 6)
            {
                LI.N1 = niveis[0];
                LI.N2 = niveis[1];
                LI.N3 = niveis[2];
                LI.N4 = niveis[3];
                LI.N5 = niveis[4];
                LI.N6 = niveis[5];
            }

            return LI;
        }

        private PERFIL_CATALOGO ValidaPerfilCatalogo(LocalInstalacaoItemSAPModelo item, DB_APRPTEntities entities)
        {
            if (entities == null)
                entities = new DB_APRPTEntities();

            //Verifica se o perfil de catálogo existe na base
            var perfilCatalogoExistente = perfilCatalogoPersistencia.ListarPerfilCatalogoPorCodigo(item.Perfil_Catalogo, entities);

            if (perfilCatalogoExistente == null)
                throw new Exception($"O perfil de catálogo {item.Perfil_Catalogo} do local {item.Local_Instalacao} " +
                    $" não existe na base de dados.");

            return perfilCatalogoExistente;
        }

        private LOCAL_INSTALACAO ValidaEInsereLIELocalBase(LocalInstalacaoItemSAPModelo item, long codPerfilCatalogo,
            NiveisLI niveisLI, DB_APRPTEntities entities)
        {
            string[] niveis = item.Local_Instalacao.Split('-');
            LOCAL_INSTALACAO LI = new LOCAL_INSTALACAO();
            LocalInstalacaoFiltroModelo filtroLI = new LocalInstalacaoFiltroModelo();

            if (niveis.Count() == 1)
                throw new Exception($"O Local de Instalação {item.Local_Instalacao} é inválido, pois contém somente um nível.");

            if (niveis.Count() > 6)
                throw new Exception($"O Local de Instalação {item.Local_Instalacao} é inválido, pois contém mais de 6 níveis.");

            string nomePrimeiroNivel = niveis[0];

            if (nomePrimeiroNivel != Constantes.PRIMEIRO_NIVEL_LOCAL_INSTALACAO)
                throw new Exception($"O primeiro nível {nomePrimeiroNivel} do local de instalação {item.Local_Instalacao}" +
                           $" não existe na base de dados!");

            if (niveis.Count() == 1)
                throw new Exception($"Por definição, não é permitido alterar a estrutura do primeiro nível.");  
            else if (niveis.Count() == 2)
            {
                LI.N1 = niveis[0];
                LI.N2 = niveis[1];

                if (!niveisLI.niveisDois.Contains(LI.N2))
                {
                    var localPai = localInstalacaoPersistencia.ValidaSeExistePaiLI(LI, entities);

                    if (localPai != null)
                        localInstalacaoPersistencia.ValidaEInsereLocalBasePorPai(localPai, LI.N2, entities);
                    else
                        throw new Exception($"O nível ' {LI.N1} ' não está cadastrado na base de dados!");

                    niveisLI.niveisDois.Add(LI.N2);
                }
            }
            else if (niveis.Count() == 3)
            {
                LI.N1 = niveis[0];
                LI.N2 = niveis[1];
                LI.N3 = niveis[2];

                if (!niveisLI.niveisTres.Contains(LI.N3))
                {
                    var localPai = localInstalacaoPersistencia.ValidaSeExistePaiLI(LI, entities);

                    if (localPai != null)
                        localInstalacaoPersistencia.ValidaEInsereLocalBasePorPai(localPai, LI.N3, entities);
                    else
                        throw new Exception($"O nível ' {LI.N2} ' não está cadastrado na base de dados!");

                    if (!niveisLI.niveisDois.Contains(LI.N2))
                        niveisLI.niveisDois.Add(LI.N2);

                    niveisLI.niveisTres.Add(LI.N3);
                }
            }
            else if (niveis.Count() == 4)
            {
                LI.N1 = niveis[0];
                LI.N2 = niveis[1];
                LI.N3 = niveis[2];
                LI.N4 = niveis[3];

                if (!niveisLI.niveisQuatro.Contains(LI.N4))
                {
                    var localPai = localInstalacaoPersistencia.ValidaSeExistePaiLI(LI, entities);

                    if (localPai != null)
                        localInstalacaoPersistencia.ValidaEInsereLocalBasePorPai(localPai, LI.N4, entities);
                    else
                        throw new Exception($"O nível ' {LI.N3} ' não está cadastrado na base de dados!");

                    if (!niveisLI.niveisDois.Contains(LI.N2))
                        niveisLI.niveisDois.Add(LI.N2);

                    if (!niveisLI.niveisTres.Contains(LI.N3))
                        niveisLI.niveisTres.Add(LI.N3);

                    niveisLI.niveisQuatro.Add(LI.N4);
                }
            }
            else if (niveis.Count() == 5)
            {
                LI.N1 = niveis[0];
                LI.N2 = niveis[1];
                LI.N3 = niveis[2];
                LI.N4 = niveis[3];
                LI.N5 = niveis[4];

                if (!niveisLI.niveisCinco.Contains(LI.N5))
                {
                    var localPai = localInstalacaoPersistencia.ValidaSeExistePaiLI(LI, entities);

                    if (localPai != null)
                        localInstalacaoPersistencia.ValidaEInsereLocalBasePorPai(localPai, LI.N5, entities);
                    else
                        throw new Exception($"O nível ' {LI.N4} ' não está cadastrado na base de dados!");

                    if (!niveisLI.niveisDois.Contains(LI.N2))
                        niveisLI.niveisDois.Add(LI.N2);

                    if (!niveisLI.niveisTres.Contains(LI.N3))
                        niveisLI.niveisTres.Add(LI.N3);

                    if (!niveisLI.niveisQuatro.Contains(LI.N4))
                        niveisLI.niveisQuatro.Add(LI.N4);

                    niveisLI.niveisCinco.Add(LI.N5);
                }
            }
            else if (niveis.Count() == 6)
            {
                LI.N1 = niveis[0];
                LI.N2 = niveis[1];
                LI.N3 = niveis[2];
                LI.N4 = niveis[3];
                LI.N5 = niveis[4];
                LI.N6 = niveis[5];

                if (!niveisLI.niveisSeis.Contains(LI.N6))
                {
                    var localPai = localInstalacaoPersistencia.ValidaSeExistePaiLI(LI, entities);

                    if (localPai != null)
                        localInstalacaoPersistencia.ValidaEInsereLocalBasePorPai(localPai, LI.N6, entities);
                    else
                        throw new Exception($"O nível ' {LI.N5} ' não está cadastrado na base de dados!");

                    if (!niveisLI.niveisDois.Contains(LI.N2))
                        niveisLI.niveisDois.Add(LI.N2);

                    if (!niveisLI.niveisTres.Contains(LI.N3))
                        niveisLI.niveisTres.Add(LI.N3);

                    if (!niveisLI.niveisQuatro.Contains(LI.N4))
                        niveisLI.niveisQuatro.Add(LI.N4);

                    if (!niveisLI.niveisCinco.Contains(LI.N5))
                        niveisLI.niveisCinco.Add(LI.N5);

                    niveisLI.niveisSeis.Add(LI.N6);
                }
            }

            LI.CodInventarioAmbiente = (long)Constantes.InventarioAmbiente.SEM_INVENTARIO;
            LI.CodPeso = (long)Constantes.PesoFisico.SEM_PESO;
            LI.CodPerfilCatalogo = codPerfilCatalogo;
            LI.Nome = item.Local_Instalacao;
            LI.Descricao = LI.Nome + " - " + item.Descricao_Local_Instalacao;
            LI.CodInventarioAmbiente = (long)Constantes.InventarioAmbiente.SEM_INVENTARIO;

            LI = localInstalacaoPersistencia.InserirLocalInstalacao(LI, entities);

            return LI;
        }

        private bool ENecessarioValidarLI(LOCAL_INSTALACAO LI, NiveisLI niveisLI)
        {
            if (LI.N2 != null && LI.N3 == null && LI.N4 == null && LI.N5 == null && LI.N6 == null)
            {
                if (!niveisLI.niveisDois.Contains(LI.N2))
                    return true;
            }
            else if (LI.N3 != null && LI.N4 == null && LI.N5 == null && LI.N6 == null)
            {
                if (!niveisLI.niveisTres.Contains(LI.N3))
                    return true;
            }

                return false;
        }

        private void ValidaLocalInstalacao(LocalInstalacaoItemSAPModelo item)
        {
            string[] LI = item.Local_Instalacao.Split('-');

            if(LI.Count() == 1)
                throw new Exception($"O Local de Instalação {item.Local_Instalacao} é inválido, pois contém somente um nível.");

            if (LI.Count() > 6)
                throw new Exception($"O Local de Instalação {item.Local_Instalacao} é inválido, pois contém mais de 6 níveis.");

            string nomePrimeiroNivel = LI[0];

            if (nomePrimeiroNivel != Constantes.PRIMEIRO_NIVEL_LOCAL_INSTALACAO)
                throw new Exception($"O primeiro nível {nomePrimeiroNivel} do local de instalação {item.Local_Instalacao}" +
                           $" não existe na base de dados!");
        }

        //private void ValidaDescricaoClasse(LocalInstalacaoItemSAPModelo item)
        //{
        //    if (item.Descricao_Classe.ToUpper() == Constantes.ClassesLocalInstalacao.Peso.ToUpper())
        //        return;
        //    else if (item.Descricao_Classe.ToUpper() == Constantes.ClassesLocalInstalacao.TagDeBloqueio.ToUpper())
        //        return;
        //    else
        //        throw new Exception($"A descrição {item.Descricao_Classe} da classe {item.Classe_Local_Instalacao} " +
        //           $"informada para o local de instalação {item.Local_Instalacao} não é uma descrição de classe válida.");
        //}

        private void ValidaDescricaoClasse(LocalInstalacaoItemSAPModelo item)
        {
            if (item.Descricao_Caracteristica.ToUpper() == Constantes.DescricaoCaracteristicaClassesLocalInstalacao.FaixaDePeso.ToUpper())
                return;
            else if (item.Descricao_Caracteristica.ToUpper() == Constantes.DescricaoCaracteristicaClassesLocalInstalacao.TagDeBloqueio.ToUpper())
                return;
            else if (item.Descricao_Caracteristica.ToUpper() == Constantes.DescricaoCaracteristicaClassesLocalInstalacao.TagLocalBloqueio.ToUpper())
                return;
            else if (item.Descricao_Caracteristica.ToUpper() == Constantes.DescricaoCaracteristicaClassesLocalInstalacao.DispositivoBloqueio.ToUpper())
                return;
            else if (item.Descricao_Caracteristica.ToUpper() == Constantes.DescricaoCaracteristicaClassesLocalInstalacao.TipoDeEnergia.ToUpper())
                return;
            else if (item.Descricao_Caracteristica.ToUpper() == Constantes.DescricaoCaracteristicaClassesLocalInstalacao.Area.ToUpper())
                return;
            else
                throw new Exception($"A descrição da característica {item.Descricao_Caracteristica} informada para " +
                    $"o local de instalação {item.Local_Instalacao} não é uma descrição de característica válida.");
        }

        //private void ValidaCaracteristicaClasse(LocalInstalacaoItemSAPModelo item)
        //{
        //    if (item.Caracteristica_Classe.ToUpper() == Constantes.CaracteristicaClassesLocalInstalacao.FaixaDePeso.ToUpper())
        //        return;
        //    else if (item.Caracteristica_Classe.ToUpper() == Constantes.CaracteristicaClassesLocalInstalacao.TagDeBloqueio.ToUpper())
        //        return;
        //    else if (item.Caracteristica_Classe.ToUpper() == Constantes.CaracteristicaClassesLocalInstalacao.TagLocalBloqueio.ToUpper())
        //        return;
        //    else if (item.Caracteristica_Classe.ToUpper() == Constantes.CaracteristicaClassesLocalInstalacao.DispositivoBloqueio.ToUpper())
        //        return;
        //    else if (item.Caracteristica_Classe.ToUpper() == Constantes.CaracteristicaClassesLocalInstalacao.TipoDeEnergia.ToUpper())
        //        return;
        //    else if (item.Caracteristica_Classe.ToUpper() == Constantes.CaracteristicaClassesLocalInstalacao.Area.ToUpper())
        //        return;
        //    else
        //        throw new Exception($"A caracaterística da classe {item.Caracteristica_Classe} informada para " +
        //            $"o local de instalação {item.Local_Instalacao} não é uma característica de classe válida.");
        //}

        private void ValidaNiveisLocal(string local, DB_APRPTEntities entities)
        {
            //Valida se os níveis anteriores existem na base 
            string[] niveis = local.Split('-');

            LOCAL_INSTALACAO LI = new LOCAL_INSTALACAO();

            if (niveis.Count() == 1)
            {
                LI.N1 = niveis[0];
            }
            else if (niveis.Count() == 2)
            {
                LI.N1 = niveis[0];
                LI.N2 = niveis[1];
            }
            else if (niveis.Count() == 3)
            {
                LI.N1 = niveis[0];
                LI.N2 = niveis[1];
                LI.N3 = niveis[2];
            }
            else if (niveis.Count() == 4)
            {
                LI.N1 = niveis[0];
                LI.N2 = niveis[1];
                LI.N3 = niveis[2];
                LI.N4 = niveis[3];
            }
            else if (niveis.Count() == 5)
            {
                LI.N1 = niveis[0];
                LI.N2 = niveis[1];
                LI.N3 = niveis[2];
                LI.N4 = niveis[3];
                LI.N5 = niveis[4];
            }
            else if (niveis.Count() == 6)
            {
                LI.N1 = niveis[0];
                LI.N2 = niveis[1];
                LI.N3 = niveis[2];
                LI.N4 = niveis[3];
                LI.N5 = niveis[4];
                LI.N6 = niveis[5];
            }

            try
            {
                this.localInstalacaoPersistencia.ValidaLocalPorNivel(LI, entities);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro na validação dos níveis do local {local} :  " + ex.Message);
            }
        
            return;
        }

        private void ValidaEInsereLocalBase(string local, DB_APRPTEntities entities)
        {
            //Valida se os níveis anteriores existem na base 
            string[] niveis = local.Split('-');

            LOCAL_INSTALACAO LI = new LOCAL_INSTALACAO();

            if (niveis.Count() == 1)
            {
                LI.N1 = niveis[0];
            }
            else if (niveis.Count() == 2)
            {
                LI.N1 = niveis[0];
                LI.N2 = niveis[1];
            }
            else if (niveis.Count() == 3)
            {
                LI.N1 = niveis[0];
                LI.N2 = niveis[1];
                LI.N3 = niveis[2];
            }
            else if (niveis.Count() == 4)
            {
                LI.N1 = niveis[0];
                LI.N2 = niveis[1];
                LI.N3 = niveis[2];
                LI.N4 = niveis[3];
            }
            else if (niveis.Count() == 5)
            {
                LI.N1 = niveis[0];
                LI.N2 = niveis[1];
                LI.N3 = niveis[2];
                LI.N4 = niveis[3];
                LI.N5 = niveis[4];
            }
            else if (niveis.Count() == 6)
            {
                LI.N1 = niveis[0];
                LI.N2 = niveis[1];
                LI.N3 = niveis[2];
                LI.N4 = niveis[3];
                LI.N5 = niveis[4];
                LI.N6 = niveis[5];
            }

           // this.localInstalacaoPersistencia.ValidaEInsereLocalBasePorNivel(LI,entities);
        }

        private LOCAL_INSTALACAO EditaLocalExistente(LOCAL_INSTALACAO localExistente, string descricaoLI,
            long codPerfilCatalogoNovo, bool bloqueiosDeletados, DB_APRPTEntities entities)
        {
            try
            {
                long codLocal = localExistente.CodLocalInstalacao;

                if (localExistente.CodPerfilCatalogo != (long)Constantes.PerfilCatalogo.SEM_PERFIL_CATALOGO &&
                    localExistente.CodPerfilCatalogo != codPerfilCatalogoNovo)
                {
                    //Verifica se existe(m) inventário(s) associado(s)
                    var invAtividadesExistentes = localInstalacaoPersistencia.ListaInventariosAtividadeAtivosLocalInstalacao(codLocal, entities);

                    if (invAtividadesExistentes.Count > 0)
                        throw new Exception($"Não é possível alterar o perfil de catálogo do local : {localExistente.Nome} pois existe(m) inventário(s) de atividade(s) " +
                            $" associado(s)");
                }

                if(bloqueiosDeletados == false)
                    //Remove os bloqueios 
                    bloqueioPersistencia.ApagarBloqueioLocal(localExistente.CodLocalInstalacao, entities);

                //Atualiza o LI
                localExistente.CodPerfilCatalogo = codPerfilCatalogoNovo;
                localExistente.Descricao = localExistente.Nome + " - " +  descricaoLI;
                localExistente = localInstalacaoPersistencia.EditaLocalInstalacao(localExistente, entities);

                return localExistente;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private LOCAL_INSTALACAO InsereLocalInstalacao(LocalInstalacaoItemSAPModelo item, long codPerfilCatalogo, 
            string nomeLI, DB_APRPTEntities entities)
        {
            string[] LI = item.Local_Instalacao.Split('-');

            LOCAL_INSTALACAO local = new LOCAL_INSTALACAO();
            local.CodInventarioAmbiente = (long)Constantes.InventarioAmbiente.SEM_INVENTARIO;
            local.CodPeso = (long)Constantes.PesoFisico.SEM_PESO;
            local.CodPerfilCatalogo = codPerfilCatalogo;
            local.Nome = nomeLI;
            local.Descricao = item.Descricao_Local_Instalacao;

            //Inventário de Ambiente
            local.INVENTARIO_AMBIENTE = this.inventariosAmbientePersistencia.
                ListarInventarioAmbientePorId((long)Constantes.InventarioAmbiente.SEM_INVENTARIO, entities);

            if (LI.Count() == 1)
            {
                throw new Exception("Não é possível editar o local de N1 pois seus dados não podem ser editados.");
            }
            else if (LI.Count() == 2)
            {
                local.N1 = LI[0];
                local.N2 = LI[1];
            }
            else if (LI.Count() == 3)
            {
                local.N1 = LI[0];
                local.N2 = LI[1];
                local.N3 = LI[2];
            }
            else if (LI.Count() == 4)
            {
                local.N1 = LI[0];
                local.N2 = LI[1];
                local.N3 = LI[2];
                local.N4 = LI[3];
            }
            else if (LI.Count() == 5)
            {
                local.N1 = LI[0];
                local.N2 = LI[1];
                local.N3 = LI[2];
                local.N4 = LI[3];
                local.N5 = LI[4];
            }
            else
            {
                local.N1 = LI[0];
                local.N2 = LI[1];
                local.N3 = LI[2];
                local.N4 = LI[3];
                local.N5 = LI[4];
                local.N6 = LI[5];
            }

            //Insere o local
            var localExistente = localInstalacaoPersistencia.InserirLocalInstalacao(local, entities);
            return localExistente;
        }

        private long InsereOuEditaCaracteristicaBloqueio(LocalInstalacaoItemSAPModelo item, DB_APRPTEntities entities)
        {

            //KKS-BLOQUEIO
            //if (item.Caracteristica_Classe == Constantes.CaracteristicaClassesLocalInstalacao.TagDeBloqueio)
            if (item.Descricao_Caracteristica == Constantes.DescricaoCaracteristicaClassesLocalInstalacao.TagDeBloqueio)
            {
                TAG_KKS_BLOQUEIO tagKKS = new TAG_KKS_BLOQUEIO();
                tagKKS.Codigo = item.Valor_Caracteristica;
                tagKKS.Nome = item.Valor_Caracteristica;

                var tagKKSExistente = bloqueioPersistencia.ListaTagKKSPorNome(item.Valor_Caracteristica, entities);

                if (tagKKSExistente == null)
                    tagKKS = bloqueioPersistencia.InserirTagKKSBloqueio(tagKKS, entities);
                else
                    tagKKS = bloqueioPersistencia.EditarTagKKS(tagKKSExistente, entities);

                return tagKKS.CodTagKKSBloqueio;
            }
            //AREA
            //else if (item.Caracteristica_Classe == Constantes.CaracteristicaClassesLocalInstalacao.Area)
            else if (item.Descricao_Caracteristica == Constantes.DescricaoCaracteristicaClassesLocalInstalacao.Area)

            {
                AREA area = new AREA();
                area.Codigo = item.Valor_Caracteristica;
                area.Nome = item.Valor_Caracteristica;

                var areaExistente = bloqueioPersistencia.ListaAreaPorNome(item.Valor_Caracteristica, entities);

                if (areaExistente == null)
                    area = bloqueioPersistencia.InserirAreaBloqueio(area, entities);
                else
                    area = bloqueioPersistencia.EditarAreaBloqueio(areaExistente, entities);

                return area.CodArea;
            }
            //DISPOSITIVO DE BLOQUEIO
            //else if (item.Caracteristica_Classe == Constantes.CaracteristicaClassesLocalInstalacao.DispositivoBloqueio)
            else if (item.Descricao_Caracteristica == Constantes.DescricaoCaracteristicaClassesLocalInstalacao.DispositivoBloqueio)

            {
                DISPOSITIVO_BLOQUEIO dispositivo = new DISPOSITIVO_BLOQUEIO();
                dispositivo.Codigo = item.Valor_Caracteristica;
                dispositivo.Nome = item.Valor_Caracteristica;

                var dispositivoExistente = bloqueioPersistencia.ListaDispositivoBloqueioNome(item.Valor_Caracteristica, entities);

                if (dispositivoExistente == null)
                    dispositivo = bloqueioPersistencia.InserirDispositivoBloqueio(dispositivo, entities);
                else
                    dispositivo = bloqueioPersistencia.EditarDispositivoBloqueio(dispositivoExistente, entities);

                return dispositivo.CodDispositivoBloqueio;
            }
            //TIPO ENERGIA BLOQUEIO
            //else if (item.Caracteristica_Classe == Constantes.CaracteristicaClassesLocalInstalacao.TipoDeEnergia)
            else if (item.Descricao_Caracteristica == Constantes.DescricaoCaracteristicaClassesLocalInstalacao.TipoDeEnergia)

            {
                TIPO_ENERGIA_BLOQUEIO tipoEnergia = new TIPO_ENERGIA_BLOQUEIO();
                tipoEnergia.Codigo = item.Valor_Caracteristica;
                tipoEnergia.Nome = item.Valor_Caracteristica;

                var tipoEnergiaExistente = bloqueioPersistencia.ListaTipoEnergiaBloqueioNome(item.Valor_Caracteristica, entities);

                if (tipoEnergiaExistente == null)
                    tipoEnergia = bloqueioPersistencia.InserirTipoEnergia(tipoEnergia, entities);
                else
                    tipoEnergia = bloqueioPersistencia.EditarTipoEnergia(tipoEnergiaExistente, entities);

                return tipoEnergia.CodTipoEnergiaBloqueio;
            }
            //LOCAL A BLOQUEAR
            //else if (item.Caracteristica_Classe == Constantes.CaracteristicaClassesLocalInstalacao.TagLocalBloqueio)
            else if (item.Descricao_Caracteristica == Constantes.DescricaoCaracteristicaClassesLocalInstalacao.TagLocalBloqueio)

            {
                LOCAL_A_BLOQUEAR localABloquear = new LOCAL_A_BLOQUEAR();
                localABloquear.Nome = item.Valor_Caracteristica;

                var localABloquearExistente = bloqueioPersistencia.ListaLocalABloquearPorNome(item.Valor_Caracteristica, entities);

                if (localABloquearExistente == null)
                    localABloquear = bloqueioPersistencia.InserirLocalABloquear(localABloquear, entities);
                else
                    localABloquear = bloqueioPersistencia.EditarLocalABloquear(localABloquearExistente, entities);

                return localABloquear.CodLocalABloquear;
            }
            else
                throw new Exception($"A caracaterística {item.Descricao_Caracteristica} da classe {item.Descricao_Classe} " +
                    $"informada para o local de instalação {item.Local_Instalacao} não é uma classe válida.");
        }

        private BLOQUEIO_LOCAL_INSTALACAO InsereOuEditaBloqueioLocalInstalacao (string codigoBloqueioLocal, long codigoBloqueio, long codigoLocal, 
            DB_APRPTEntities entities)
        {
            //Verifica se já existe algum relacionamento para BLOQUEIO_LOCAL_INSTALACAO
            var bloqueioExistente = this.bloqueioPersistencia.ListarBloqueioLocalInstalacaoPorNomeELocal(codigoBloqueioLocal, codigoLocal);

            if(bloqueioExistente == null)
                bloqueioExistente = this.bloqueioPersistencia.InserirBloqueioLocalInstalacao(codigoBloqueio, codigoLocal, codigoBloqueioLocal);

            return bloqueioExistente;
        }
    }
}
