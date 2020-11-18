using AutoMapper;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LaborSafety.Dominio.Modelos;
using LaborSafety.Negocio.Interfaces;
using LaborSafety.Negocio.Validadores.Interface;
using LaborSafety.Persistencia;
using LaborSafety.Persistencia.Interfaces;
using LaborSafety.Utils.Constantes;

namespace LaborSafety.Negocio.Servicos
{
    public class InventarioAmbienteNegocio : IInventariosAmbienteNegocio
    {
        private readonly IInventariosAmbientePersistencia inventarioAmbientePersistencia;
        private readonly IMapeamentoLocalInventarioAmbientePersistencia mapeamentoLocalInventarioAmbientePersistencia;
        private readonly Validador<InventarioAmbienteModelo> validadorInventarioAmbiente;

        private readonly ILocalInstalacaoPersistencia localInstalacaoPersistencia;
        private readonly IEPIPersistencia epiPersistencia;
        private readonly INrPersistencia nrPersistencia;
        private readonly IAmbientePersistencia sistemaOperacionalPersistencia;
        private readonly IRiscoPersistencia riscoPersistencia;
        private readonly ISeveridadePersistencia severidadePersistencia;
        private readonly IProbabilidadePersistencia probabilidadePersistencia;
        private readonly ILogInventarioAmbientePersistencia logInventarioAmbientePersistencia;


        public InventarioAmbienteNegocio(IInventariosAmbientePersistencia inventarioAmbientePersistencia, IMapeamentoLocalInventarioAmbientePersistencia mapeamentoLocalInventarioAmbientePersistencia,
            Validador<InventarioAmbienteModelo> validadorInventarioAmbiente, ILocalInstalacaoPersistencia localInstalacaoPersistencia,
            IEPIPersistencia contraMedidaPersistencia, INrPersistencia nrPersistencia, INrInventarioAmbientePersistencia nrInventarioAmbientePersistencia,
            IAmbientePersistencia sistemaOperacionalPersistencia, IRiscoPersistencia riscoPersistencia, ISeveridadePersistencia severidadePersistencia,
            IProbabilidadePersistencia probabilidadePersistencia, ILogInventarioAmbientePersistencia logInventarioAmbientePersistencia = null)
        {
            this.inventarioAmbientePersistencia = inventarioAmbientePersistencia;
            this.mapeamentoLocalInventarioAmbientePersistencia = mapeamentoLocalInventarioAmbientePersistencia;
            this.validadorInventarioAmbiente = validadorInventarioAmbiente;

            this.localInstalacaoPersistencia = localInstalacaoPersistencia;
            this.epiPersistencia = contraMedidaPersistencia;
            this.nrPersistencia = nrPersistencia;
            this.sistemaOperacionalPersistencia = sistemaOperacionalPersistencia;
            this.riscoPersistencia = riscoPersistencia;
            this.severidadePersistencia = severidadePersistencia;
            this.probabilidadePersistencia = probabilidadePersistencia;
            this.logInventarioAmbientePersistencia = logInventarioAmbientePersistencia;
        }

        public void ValidarFiltro(FiltroInventarioAmbienteModelo filtroInventarioAmbienteModelo)
        {
            if (filtroInventarioAmbienteModelo.CodAmbiente == null && (filtroInventarioAmbienteModelo.CodLocaisInstalacao == null || !filtroInventarioAmbienteModelo.CodLocaisInstalacao.Any()))
            {
                throw new KeyNotFoundException("Preencha pelo menos uma das opções!");
            }
        }
        public int CalcularRiscoAmbiente(int probabilidade, decimal severidade)
        {
            int resultadoRisco;
            decimal multiplicacao = probabilidade * severidade;


            if (multiplicacao >= 150)
            {
                resultadoRisco = 4;
            }
            else if (multiplicacao >= 75 && multiplicacao <= 120)
            {
                resultadoRisco = 3;
            }
            else if (multiplicacao >= Convert.ToDecimal(7.5) && multiplicacao <= 64)
            {
                resultadoRisco = 2;
            }
            else
            {
                resultadoRisco = 1;
            }
            return resultadoRisco;
        }

        public InventarioAmbienteModelo MapeamentoInventarioAmbiente(INVENTARIO_AMBIENTE inventario)
        {
            InventarioAmbienteModelo inventarioAmb = new InventarioAmbienteModelo()
            {
                CodInventarioAmbiente = inventario.CodInventarioAmbiente,
                Codigo = inventario.Codigo,
                CodAmbiente = inventario.CodAmbiente,
                Descricao = inventario.Descricao,
                ObservacaoGeral = inventario.ObservacaoGeral,
                RiscoGeral = inventario.RiscoGeral,
                DataAtualizacao = inventario.DataAtualizacao,
                Ativo = inventario.Ativo
            };

            inventarioAmb.NR_INVENTARIO_AMBIENTE = new List<NrInventarioAmbienteModelo>();
            inventarioAmb.RISCO_INVENTARIO_AMBIENTE = new List<RiscoInventarioAmbienteModelo>();
            inventarioAmb.LOCAL_INSTALACAO_MODELO = new List<LocalInstalacaoModelo>();

            Mapper.Map(inventario.NR_INVENTARIO_AMBIENTE, inventarioAmb.NR_INVENTARIO_AMBIENTE);
            Mapper.Map(inventario.LOCAL_INSTALACAO, inventarioAmb.LOCAL_INSTALACAO_MODELO);

            List<RiscoInventarioAmbienteModelo> listaRisco = new List<RiscoInventarioAmbienteModelo>();
            foreach (var itemRisco in inventario.RISCO_INVENTARIO_AMBIENTE)
            {
                RiscoInventarioAmbienteModelo risco = new RiscoInventarioAmbienteModelo();
                risco.Ativo = true;
                risco.CodInventarioAmbiente = itemRisco.CodInventarioAmbiente;
                risco.CodProbabilidade = itemRisco.CodProbabilidade;
                risco.CodRiscoAmbiente = itemRisco.CodRiscoAmbiente;
                risco.CodRiscoInventarioAmbiente = itemRisco.CodRiscoInventarioAmbiente;
                risco.CodSeveridade = itemRisco.CodSeveridade;
                risco.ContraMedidas = itemRisco.ContraMedidas;
                risco.FonteGeradora = itemRisco.FonteGeradora;
                risco.ProcedimentosAplicaveis = itemRisco.ProcedimentosAplicaveis;

                risco.EPIRiscoInventarioAmbienteModelo = new List<EPIRiscoInventarioAmbienteModelo>();

                var listaEPI = itemRisco.EPI_RISCO_INVENTARIO_AMBIENTE
                    .Where(a => a.CodRiscoInventarioAmbiente == itemRisco.CodRiscoInventarioAmbiente).ToList();

                Mapper.Map(listaEPI, risco.EPIRiscoInventarioAmbienteModelo);

                listaRisco.Add(risco);
            }

            inventarioAmb.RISCO_INVENTARIO_AMBIENTE = listaRisco;

            return inventarioAmb;
        }

        public InventarioAmbienteModelo ListarInventarioAmbientePorId(long id)
        {
            INVENTARIO_AMBIENTE inv = this.inventarioAmbientePersistencia.ListarInventarioAmbientePorId(id);
            if (inv == null)
            {
                throw new KeyNotFoundException("Inventário de ambiente não encontrado.");
            }

            foreach (var itemLi in inv.LOCAL_INSTALACAO)
            {
                string n2 = "", n3 = "", n4 = "", n5 = "", n6 = "";

                if (!string.IsNullOrEmpty(itemLi.N2))
                    n2 = itemLi.N2;

                if (!string.IsNullOrEmpty(itemLi.N3))
                    n3 = itemLi.N3;

                if (!string.IsNullOrEmpty(itemLi.N4))
                    n4 = itemLi.N4;

                if (!string.IsNullOrEmpty(itemLi.N5))
                    n5 = itemLi.N5;

                if (!string.IsNullOrEmpty(itemLi.N6))
                    n6 = itemLi.N6;

                if (n2.Contains("000_BASE") || n3.Contains("000_BASE") || n4.Contains("000_BASE") || n5.Contains("000_BASE") || n6.Contains("000_BASE"))
                {
                    var nomeSeparado = itemLi.Nome.Split(' ');
                    var nomeCompleto = itemLi.Nome.Replace(nomeSeparado[0], nomeSeparado[0] + "-000_BASE");
                    itemLi.Nome = nomeCompleto;
                }
            }

            return MapeamentoInventarioAmbiente(inv);
        }

        public InventarioAmbienteModelo ListarInventarioAmbientePorLI(long li)
        {
            INVENTARIO_AMBIENTE inv = this.inventarioAmbientePersistencia.ListarInventarioAmbientePorLI(li);
            if (inv == null)
            {
                throw new KeyNotFoundException("Inventário de ambiente não encontrado.");
            }
            return MapeamentoInventarioAmbiente(inv);
        }

        public long ListarCodAprPorInventario(long codInventario)
        {
            long inv = inventarioAmbientePersistencia.ListarCodAprPorInventario(codInventario, null);

            return inv;
        }

        public long ListarCodAprPorInventarioTela(long codInventario)
        {
            long inv = inventarioAmbientePersistencia.ListarCodAprPorInventarioTela(codInventario, null);

            return inv;
        }

        public IEnumerable<InventarioAmbienteModelo> ListarInventarioAmbiente(FiltroInventarioAmbienteModelo filtroInventarioAmbienteModelo)
        {
            List<InventarioAmbienteModelo> inventarioAmbienteModelo = new List<InventarioAmbienteModelo>();

            ValidarFiltro(filtroInventarioAmbienteModelo);

            IEnumerable<INVENTARIO_AMBIENTE> inventariosAmbienteFiltro = null;

            inventariosAmbienteFiltro = this.inventarioAmbientePersistencia.ListarInventarioAmbiente(filtroInventarioAmbienteModelo);

            if (inventariosAmbienteFiltro != null)
            {
                foreach (INVENTARIO_AMBIENTE inventario in inventariosAmbienteFiltro)
                {
                    inventarioAmbienteModelo.Add(MapeamentoInventarioAmbiente(inventario));
                }
            }
            return inventarioAmbienteModelo;
        }

        public class RetornoInsercao
        {
            public bool status;
            public List<String> localModelo = new List<String>();
        }


        public RetornoInsercao InserirInventarioAmbiente(InventarioAmbienteModelo inventarioAmbienteModelo)
        {
            RetornoInsercao retornoInsercao = new RetornoInsercao();
            retornoInsercao.localModelo = new List<string>();

            validadorInventarioAmbiente.ValidaInsercao(inventarioAmbienteModelo);

            List<LocalInstalacaoModelo> locaisInstalacao = inventarioAmbienteModelo.LOCAL_INSTALACAO_MODELO;

            using (var entities = new DB_APRPTEntities())
            {

                using (var transaction = entities.Database.BeginTransaction(System.Data.IsolationLevel.Serializable))
                {
                    try
                    {
                        //Limpa os locais do inventário de modelo
                        inventarioAmbienteModelo.LOCAL_INSTALACAO_MODELO = null;

                        //Insere o inventário sem LI
                        INVENTARIO_AMBIENTE inventario = inventarioAmbientePersistencia.Inserir(inventarioAmbienteModelo, entities);

                        List<LOCAL_INSTALACAO> locaisComInventarios = new List<LOCAL_INSTALACAO>();

                        long codInventarioAmbiente = inventario.CodInventarioAmbiente;

                        //List<LOCAL_INSTALACAO> locais = localInstalacaoPersistencia.ListarTodosLIs(entities);

                        for (int i = 0; i < locaisInstalacao.Count; i++)
                        {
                            var codLocal = locaisInstalacao[i].CodLocalInstalacao;

                            var localEnviado = localInstalacaoPersistencia.ListarLocalInstalacaoPorId(codLocal, entities);

                            //Filtra somente os locais do pai
                            List<LOCAL_INSTALACAO> locaisEFilhos = this.BuscaLocaisEFilhos(entities, localEnviado);

                            /*
                            List<LOCAL_INSTALACAO> locaisFilhos =
                                inventarioAmbientePersistencia.BuscaFilhosPorNivelExcetoInventario(codLocal, codInventarioAmbiente, entities);
                                */

                            List<LOCAL_INSTALACAO> locaisFilhosComInventarios = locaisEFilhos.Where(x => x.CodInventarioAmbiente !=
                            (long)Constantes.InventarioAmbiente.SEM_INVENTARIO && x.CodInventarioAmbiente != codInventarioAmbiente).ToList();

                            if (locaisFilhosComInventarios.Count > 0)
                                locaisComInventarios.AddRange(locaisFilhosComInventarios);
                            else
                                foreach (var local in locaisEFilhos)
                                    local.CodInventarioAmbiente = codInventarioAmbiente;
                        }

                        if (locaisComInventarios.Count > 0)
                        {
                            retornoInsercao.status = false;

                            foreach (var local in locaisComInventarios)
                                retornoInsercao.localModelo.Add(local.Nome);

                            transaction.Rollback();

                            return retornoInsercao;
                        }

                        logInventarioAmbientePersistencia.Inserir(inventarioAmbienteModelo, inventario.CodInventarioAmbiente, entities);

                        entities.SaveChanges();
                        transaction.Commit();

                        retornoInsercao.status = true;
                        return retornoInsercao;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }
        }

        private List<LOCAL_INSTALACAO> BuscaLocaisEFilhos(DB_APRPTEntities entities, LOCAL_INSTALACAO localEnviado)
        {
            List<LOCAL_INSTALACAO> locaisFiltrados = new List<LOCAL_INSTALACAO>();

            if (String.IsNullOrEmpty(localEnviado.N2))
                locaisFiltrados = entities.LOCAL_INSTALACAO.Where(x => x.N1 == localEnviado.N1).ToList();
            else if (String.IsNullOrEmpty(localEnviado.N3))
                locaisFiltrados = entities.LOCAL_INSTALACAO.Where(x => x.N1 == localEnviado.N1 && x.N2 == localEnviado.N2).ToList();
            else if (String.IsNullOrEmpty(localEnviado.N4))
                locaisFiltrados = entities.LOCAL_INSTALACAO.Where(x => x.N1 == localEnviado.N1 && x.N2 == localEnviado.N2 &&
                x.N3 == localEnviado.N3).ToList();
            else if (String.IsNullOrEmpty(localEnviado.N5))
                locaisFiltrados = entities.LOCAL_INSTALACAO.Where(x => x.N1 == localEnviado.N1 && x.N2 == localEnviado.N2 &&
                x.N3 == localEnviado.N3 && x.N4 == localEnviado.N4).ToList();
            else if (String.IsNullOrEmpty(localEnviado.N6))
                locaisFiltrados = entities.LOCAL_INSTALACAO.Where(x => x.N1 == localEnviado.N1 && x.N2 == localEnviado.N2
                && x.N3 == localEnviado.N3 && x.N4 == localEnviado.N4 && x.N5 == localEnviado.N5).ToList();
            else
                locaisFiltrados = entities.LOCAL_INSTALACAO.Where(x => x.N1 == localEnviado.N1 && x.N2 == localEnviado.N2
                && x.N3 == localEnviado.N3 && x.N4 == localEnviado.N4 && x.N5 == localEnviado.N5 && x.N6 == localEnviado.N6).ToList();

            return locaisFiltrados;
        }

        private class ProcessamentoThread
        {
            public bool sucesso;
            public List<LOCAL_INSTALACAO> locaisComInventarios = new List<LOCAL_INSTALACAO>();
            public List<LOCAL_INSTALACAO> locaisAAssociar = new List<LOCAL_INSTALACAO>();
        }

        private ProcessamentoThread ValidaLocaisParaInsercao(long codLocalInstalacao, long codInventarioAmbiente, DB_APRPTEntities entities)
        {
            ProcessamentoThread processamentoThread = new ProcessamentoThread();

            List<LOCAL_INSTALACAO> locaisFilhos =
                inventarioAmbientePersistencia.BuscaFilhosPorNivelExcetoInventario(codLocalInstalacao, codInventarioAmbiente, entities);

            List<LOCAL_INSTALACAO> locaisComInventarios = locaisFilhos.Where(x => x.CodInventarioAmbiente !=
                                    (long)Constantes.InventarioAmbiente.SEM_INVENTARIO && x.CodInventarioAmbiente != codInventarioAmbiente).ToList();

            if (locaisComInventarios.Count > 0)
            {
                processamentoThread.sucesso = false;
                processamentoThread.locaisComInventarios = locaisComInventarios;

                return processamentoThread;
            }

            processamentoThread.sucesso = true;
            processamentoThread.locaisAAssociar = locaisFilhos;

            return processamentoThread;
        }

        public void EditarInventarioAmbiente(InventarioAmbienteModelo inventarioAmbienteModelo)
        {
            validadorInventarioAmbiente.ValidaEdicao(inventarioAmbienteModelo);

            List<LocalInstalacaoModelo> locaisInstalacaoOrigem = new List<LocalInstalacaoModelo>();

            locaisInstalacaoOrigem.AddRange(inventarioAmbienteModelo.LOCAL_INSTALACAO_MODELO);

            List<LocalInstalacaoModelo> locaisInstalacaoAAssociar = new List<LocalInstalacaoModelo>();

            using (var entities = new DB_APRPTEntities())
            {
                using (var transaction = entities.Database.BeginTransaction(System.Data.IsolationLevel.Serializable))
                {
                    try
                    {
                        //Limpa os locais que vieram
                        inventarioAmbienteModelo.LOCAL_INSTALACAO_MODELO.Clear();

                        //List<LOCAL_INSTALACAO> locais = localInstalacaoPersistencia.ListarTodosLIs(entities);

                        foreach (var local in locaisInstalacaoOrigem)
                        {
                            var codLocal = local.CodLocalInstalacao;

                            //var localEnviado = localInstalacaoPersistencia.ListarLocalInstalacaoPorId(codLocal, entities);
                            var localEnviado = entities.LOCAL_INSTALACAO.Where(x => x.CodLocalInstalacao == codLocal).FirstOrDefault();

                            var inventarioAmbienteExistente =
                                inventarioAmbientePersistencia.ListarInventarioAmbientePorLI(localEnviado.CodLocalInstalacao, entities);

                            if (localEnviado.CodInventarioAmbiente != (long)Constantes.InventarioAmbiente.SEM_INVENTARIO
                                && localEnviado.CodInventarioAmbiente != inventarioAmbienteModelo.CodInventarioAmbiente)
                                throw new Exception($"O local de código {localEnviado.Nome} já possui o inventário de ambiente " +
                                    $"associado {inventarioAmbienteExistente.Codigo} !");

                            List<LOCAL_INSTALACAO> locaisEFilhos = this.BuscaLocaisEFilhos(entities, localEnviado);

                            foreach (var localAAssociar in locaisEFilhos)
                            {
                                LocalInstalacaoModelo localModelo = new LocalInstalacaoModelo();
                                localModelo.CodInventarioAmbiente = localAAssociar.CodInventarioAmbiente;
                                localModelo.CodLocalInstalacao = localAAssociar.CodLocalInstalacao;
                                localModelo.CodPerfilCatalogo = localAAssociar.CodPerfilCatalogo;
                                localModelo.CodPeso = localAAssociar.CodPeso;
                                localModelo.Descricao = localAAssociar.Descricao;
                                localModelo.N1 = localAAssociar.N1;
                                localModelo.N2 = localAAssociar.N2;
                                localModelo.N3 = localAAssociar.N3;
                                localModelo.N4 = localAAssociar.N4;
                                localModelo.N5 = localAAssociar.N5;
                                localModelo.N6 = localAAssociar.N6;
                                localModelo.Nome = localAAssociar.Nome;

                                locaisInstalacaoAAssociar.Add(localModelo);
                            }
                        }

                        inventarioAmbienteModelo.LOCAL_INSTALACAO_MODELO.AddRange(locaisInstalacaoAAssociar);

                        List<LOCAL_INSTALACAO> listaLocaisLog = new List<LOCAL_INSTALACAO>();
                        Mapper.Map(locaisInstalacaoAAssociar, listaLocaisLog);

                        logInventarioAmbientePersistencia.Editar(listaLocaisLog, inventarioAmbienteModelo, entities);

                        var edicao = inventarioAmbientePersistencia.EditarInventarioAmbiente(inventarioAmbienteModelo, entities);

                        logInventarioAmbientePersistencia.AtualizarCodInventarioLog(inventarioAmbienteModelo.CodInventarioAmbiente, edicao.CodInventarioAmbiente, entities);

                        entities.SaveChanges();
                        transaction.Commit();

                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }
        }

        public void EditarNrInventarioAmbiente(long idInventario, long idNr)
        {
            this.inventarioAmbientePersistencia.EditarNrInventarioAmbiente(idInventario, idNr);
        }

        public void EditarRiscoInventarioAmbiente(long idInventario, long idRisco)
        {
            this.inventarioAmbientePersistencia.EditarRiscoInventarioAmbiente(idInventario, idRisco);
        }

        public void DesativarInventario(InventarioAmbienteDelecaoComLogModelo inventarioAmbienteDelecaoComLogModelo, DB_APRPTEntities entities)
        {
            using (entities = new DB_APRPTEntities())
            {
                using (var transaction = entities.Database.BeginTransaction())
                {
                    try
                    {
                        this.logInventarioAmbientePersistencia.Excluir(inventarioAmbienteDelecaoComLogModelo, entities);

                        this.inventarioAmbientePersistencia.DesativarInventario(inventarioAmbienteDelecaoComLogModelo.CodInventarioAmbiente, entities);

                        transaction.Commit();
                    }

                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }
        }

        public List<LOG_INVENTARIO_AMBIENTE> ListarLogInventario(List<long> codInventarioAmbiente)
        {
            List<INVENTARIO_AMBIENTE> listaInventarios = new List<INVENTARIO_AMBIENTE>();
            List<LOG_INVENTARIO_AMBIENTE> logInventarios = new List<LOG_INVENTARIO_AMBIENTE>();
            List<LOG_INVENTARIO_AMBIENTE> listaLogInventarios = new List<LOG_INVENTARIO_AMBIENTE>();

            try
            {
                foreach (var item in codInventarioAmbiente)
                {
                    var inventario = inventarioAmbientePersistencia.ListarInventarioAmbienteAtivadoEDesativadoPorId(item);

                    listaInventarios.Add(inventario);
                }

                foreach (var itemInventario in listaInventarios)
                {
                    logInventarios = this.logInventarioAmbientePersistencia.ListarLogInventario(itemInventario.CodInventarioAmbiente);

                    foreach (var itemLog in logInventarios)
                    {
                        listaLogInventarios.Add(itemLog);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return listaLogInventarios;
        }

        public class ArquivoLog
        {
            public string resultadoString { get; set; }
        }

        public ArquivoLog EscreverLogTodosInventarios()
        {
            try
            {
                List<long> codInventarios = new List<long>();

                var todosInventarios = inventarioAmbientePersistencia.ListarTodosInventarios();

                if (todosInventarios.Count <= 0)
                    throw new Exception("Não existem inventários cadastrados na base de dados.");

                foreach (var itemInventario in todosInventarios)
                {
                    codInventarios.Add(itemInventario.CodInventarioAmbiente);
                }

                var resultado = EscreverLogEmTxt(codInventarios);

                return resultado;
            }

            catch (Exception)
            {
                throw;
            }
        }

        public ArquivoLog EscreverLogEmTxt(List<long> codInventariosAmbiente)
        {
            try
            {
                if (codInventariosAmbiente.Count <= 0)
                    throw new Exception($"Gentileza informar no mínimo um código de inventário para a geração dos logs.");

                string caminhoLogInventario = ConfigurationManager.AppSettings["caminhoLogInventario"];
                string nomeArquivo = "log_" + "_" + DateTime.Now.ToString() + ".txt";
                nomeArquivo = nomeArquivo.Replace("/", "_").Replace(" ", "_").Replace(":", "_");
                string caminhoCompleto = caminhoLogInventario + nomeArquivo;

                string todosLogs = "";

                if (!File.Exists(caminhoLogInventario))
                    File.Create(caminhoCompleto).Close();

                var logInventarioAmbiente = ListarLogInventario(codInventariosAmbiente);

                if (logInventarioAmbiente.Count <= 0)
                    throw new Exception($"Não foram encontrados logs referentes ao inventário informado.");

                var listaDeLogsAgrupadosPorInventario = logInventarioAmbiente.GroupBy(x => x.CodInventario).Select(u => u.ToList()).ToList();

                int i = 0;
                foreach (var itemLista1 in listaDeLogsAgrupadosPorInventario)
                {
                    foreach (var itemLista2 in listaDeLogsAgrupadosPorInventario[i])
                    {
                        var inventario = inventarioAmbientePersistencia.ListarInventarioAmbientePorId(itemLista2.CodInventario, null, false);

                        var entities = new DB_APRPTEntities();
                        var riscosLog = entities.LOG_RISCO_INVENTARIO_AMBIENTE.Where(x => x.CodLogInventarioAmbiente == itemLista2.CodLogInventarioAmbiente).ToList();

                        entities.Dispose();

                        var listaRiscosDoInventario = inventario.RISCO_INVENTARIO_AMBIENTE.Where(x => x.CodInventarioAmbiente == inventario.CodInventarioAmbiente).ToList();

                        todosLogs = geraLogInventario(itemLista2, inventario, todosLogs, riscosLog);

                        if (listaRiscosDoInventario.Count <= 0)
                            throw new Exception($"Não foram encontrados riscos para o inventário de código {inventario.Codigo}");

                        if (itemLista2.CodLogTipoOperacao == 2)
                        {
                            foreach (var itemRisco in listaRiscosDoInventario)
                            {
                                var epi = itemRisco.EPI_RISCO_INVENTARIO_AMBIENTE.Where(y => y.CodRiscoInventarioAmbiente == itemRisco.CodRiscoInventarioAmbiente).ToList();

                                if (epi.Count <= 0)
                                    todosLogs = geraLogRiscosInventario(itemRisco, null, todosLogs);

                                else
                                    todosLogs = geraLogRiscosInventario(itemRisco, epi, todosLogs);
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

        private string geraLogInventario(LOG_INVENTARIO_AMBIENTE logs, INVENTARIO_AMBIENTE inventario, string conteudo, List<LOG_RISCO_INVENTARIO_AMBIENTE> riscos)
        {
            try
            {
                // IF ABAIXO COM NEGAÇÃO (!)
                if (conteudo.Contains($@"Código do inventário: {inventario.Codigo}") == false)
                    conteudo += $@" {Environment.NewLine}
                                    Código do inventário: {inventario.Codigo}";

                if (logs.CodLogTipoOperacao == (long)Constantes.TipoOperacaoLog.INSERCAO)
                {
                    conteudo += $@" {Environment.NewLine}
                            --- INÍCIO LOG INSERÇÃO ---

                            Data da inserção: {logs.DataAlteracao}
                            Código do usuário que realizou a inserção: {logs.CodUsuarioModificador}

                            --- FIM LOG INSERÇÃO --- ";
                }

                else if (logs.CodLogTipoOperacao == (long)Constantes.TipoOperacaoLog.EDICAO)
                {
                    #region buscar NRs
                    string nrAntigasNome = "", nrNovasNome = "";
                    var listaNRsAntigas = logs.CodigosNRsAntigas.Split(',');
                    List<string> nomeNRsAntigas = new List<string>();

                    // IF ABAIXO COM NEGAÇÃO
                    if (!string.IsNullOrEmpty(listaNRsAntigas[0]))
                    {
                        foreach (var itemNrAntiga in listaNRsAntigas)
                        {
                            var nr = nrPersistencia.ListarNrPorIdString(itemNrAntiga);
                            nomeNRsAntigas.Add(nr.Nome);
                        }

                        int contNrAntiga = 0;
                        foreach (var itemNomeNRAniga in nomeNRsAntigas)
                        {
                            if (contNrAntiga == 0)
                            {
                                nrAntigasNome += $"{itemNomeNRAniga}";
                            }
                            else
                            {
                                nrAntigasNome += $", {itemNomeNRAniga}";
                            }
                            contNrAntiga++;
                        }
                    }

                    var listaNRsNovas = logs.CodigosNRsNovas.Split(',');
                    List<string> nomeNRsNovas = new List<string>();

                    // IF ABAIXO COM NEGAÇÃO
                    if (!string.IsNullOrEmpty(listaNRsNovas[0]))
                    {
                        foreach (var itemNrNova in listaNRsNovas)
                        {
                            var nr = nrPersistencia.ListarNrPorIdString(itemNrNova);
                            nomeNRsNovas.Add(nr.Nome);
                        }

                        int contNrNova = 0;
                        foreach (var itemNomeNRNova in nomeNRsNovas)
                        {
                            if (contNrNova == 0)
                            {
                                nrNovasNome += $@"{itemNomeNRNova}";
                            }
                            else
                            {
                                nrNovasNome += $@", {itemNomeNRNova}";
                            }
                            contNrNova++;
                        }
                    }

                    #endregion

                    #region Ambiente
                    var ambienteAntigo = sistemaOperacionalPersistencia.ListarSistemaOperacionalPorId((long)logs.CodAmbienteAntigo);
                    var ambienteNovo = sistemaOperacionalPersistencia.ListarSistemaOperacionalPorId((long)logs.CodAmbienteNovo);
                    #endregion

                    #region LIs
                    string liAntigo = "", liNovo = "";
                    var listaLIsAntigos = logs.CodigosLIsAntigos.Split(',');
                    var listaLIsNovos = logs.CodigosLIsNovos.Split(',');
                    List<string> nomeLIsAntigos = new List<string>();
                    List<string> nomeLIsNovos = new List<string>();

                    foreach (var itemLiAntigo in listaLIsAntigos)
                    {
                        var nomeLi = localInstalacaoPersistencia.ListarLocalInstalacaoPorIdString(itemLiAntigo);
                        nomeLIsAntigos.Add(nomeLi.Nome);
                    }

                    int contLiAntigo = 0;
                    foreach (var itemNomeLIAntigo in nomeLIsAntigos)
                    {
                        if (contLiAntigo == 0)
                        {
                            liAntigo += $@"{itemNomeLIAntigo}";
                        }
                        else
                        {
                            liAntigo += $@", {itemNomeLIAntigo}";
                        }
                        contLiAntigo++;
                    }

                    foreach (var itemLiNovo in listaLIsNovos)
                    {
                        var nomeLiNovo = localInstalacaoPersistencia.ListarLocalInstalacaoPorIdString(itemLiNovo);
                        nomeLIsNovos.Add(nomeLiNovo.Nome);
                    }

                    int contLiNovo = 0;
                    foreach (var itemNomeLINovo in nomeLIsNovos)
                    {
                        if (contLiNovo == 0)
                        {
                            liNovo += $@"{itemNomeLINovo}";
                        }
                        else
                        {
                            liNovo += $@", {itemNomeLINovo}";
                        }
                        contLiNovo++;
                    }
                    #endregion


                    conteudo += $@" {Environment.NewLine}  
                           --- INÍCIO LOG EDIÇÃO ---

                            Nome do ambiente antigo: {ambienteAntigo.Nome}
                            Nome do novo ambiente: {ambienteNovo.Nome}

                            Descrição do inventário antigo: {logs.DescricaoAntiga}
                            Descrição do novo inventário: {logs.DescricaoNova}

                            Nome das NRs antigas: {nrAntigasNome}
                            Nome das novas NRs: {nrNovasNome}

                            Observações antigas: {logs.ObsGeralAntiga}
                            Novas observações: {logs.ObsGeralNova}

                            Locais de instalação antigos: {liAntigo}
                            Novos locais de instalação: {liNovo}

                            Risco geral antigo: {logs.RiscoGeralAntigo}
                            Novo risco geral: {logs.RiscoGeralNovo}";

                    foreach (var itemRisco in riscos)
                    {
                        var nomeRisco = riscoPersistencia.ListarRiscoPorId(itemRisco.CodRisco);
                        var nomeSeveridade = severidadePersistencia.ListarSeveridadePorId((long)itemRisco.CodSeveridade);
                        var nomeProbabilidade = probabilidadePersistencia.ListarProbabilidadePorId((long)itemRisco.CodProbabilidade);

                        #region EPIs
                        string epi = "";
                        if (itemRisco.CodigosEPIs != null)
                        {
                            var listaEPIs = itemRisco.CodigosEPIs.Split(',');
                            List<string> nomeEPIs = new List<string>();
                            if (listaEPIs[0] != "")
                            {
                                foreach (var itemEpi in listaEPIs)
                                {
                                    var nomeEPI = epiPersistencia.ListarEPIPorIdString(itemEpi);
                                    nomeEPIs.Add(nomeEPI.Descricao);
                                }
                                int contEPI = 0;
                                foreach (var itemNomeEPI in nomeEPIs)
                                {
                                    if (contEPI == 0)
                                    {
                                        epi += $"{itemNomeEPI}";
                                    }
                                    else
                                    {
                                        epi += $", {itemNomeEPI}";
                                    }
                                    contEPI++;
                                }
                            }
                        }

                        #endregion

                        if (string.IsNullOrEmpty(epi))
                        {
                            conteudo += $@" {Environment.NewLine}  
                            Dados do risco antigo:         
                            Risco: {nomeRisco.Nome}
                            Severidade: {nomeSeveridade.Nome}
                            Probabilidade: {nomeProbabilidade.Nome}
                            Fonte geradora: {itemRisco.FonteGeradora}
                            Procedimentos aplicáveis: {itemRisco.ProcedimentosAplicaveis}
                            Contramedidas: {itemRisco.ContraMedidas}";
                        }
                        else
                        {
                            conteudo += $@" {Environment.NewLine}  
                            Dados do risco antigo:         
                            Risco: {nomeRisco.Nome}
                            Severidade: {nomeSeveridade.Nome}
                            Probabilidade: {nomeProbabilidade.Nome}
                            Fonte geradora: {itemRisco.FonteGeradora}
                            Procedimentos aplicáveis: {itemRisco.ProcedimentosAplicaveis}
                            Contramedidas: {itemRisco.ContraMedidas}
                            Nome dos EPIs: {epi}";
                        }

                    }
                }

                else if (logs.CodLogTipoOperacao == (long)Constantes.TipoOperacaoLog.DELECAO)
                {
                    conteudo += $@" {Environment.NewLine} 
                    --- INÍCIO LOG EXCLUSÃO ---

                    Data da exclusão: {logs.DataAlteracao}
                    Código do usuário que realizou a exclusão: {logs.CodUsuarioModificador}

                    --- FIM LOG EXCLUSÃO --- ";
                }

                return conteudo;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private string geraLogRiscosInventario(RISCO_INVENTARIO_AMBIENTE itemRisco, List<EPI_RISCO_INVENTARIO_AMBIENTE> listaEPI, string conteudo)
        {
            try
            {
                var risco = riscoPersistencia.ListarRiscoPorId(itemRisco.CodRiscoAmbiente);
                var severidade = severidadePersistencia.ListarSeveridadePorId(itemRisco.CodSeveridade);
                var probabilidade = probabilidadePersistencia.ListarProbabilidadePorId(itemRisco.CodProbabilidade);

                if (string.IsNullOrEmpty(itemRisco.ProcedimentosAplicaveis) && string.IsNullOrEmpty(itemRisco.ContraMedidas))
                {
                    conteudo += $@" {Environment.NewLine} 
                            Dados do risco atual:       
                            Risco: {risco.Nome}
                            Severidade: {severidade.Nome}
                            Probabilidade: {probabilidade.Nome}
                            Fonte geradora: {itemRisco.FonteGeradora}";
                }
                else if (string.IsNullOrEmpty(itemRisco.ProcedimentosAplicaveis) && !string.IsNullOrEmpty(itemRisco.ContraMedidas))
                {
                    conteudo += $@" {Environment.NewLine} 
                            Dados do risco atual:       
                            Risco: {risco.Nome}
                            Severidade: {severidade.Nome}
                            Probabilidade: {probabilidade.Nome}
                            Fonte geradora: {itemRisco.FonteGeradora}
                            Contramedidas: {itemRisco.ContraMedidas}";
                }
                else if (!string.IsNullOrEmpty(itemRisco.ProcedimentosAplicaveis) && string.IsNullOrEmpty(itemRisco.ContraMedidas))
                {
                    conteudo += $@" {Environment.NewLine} 
                            Dados do risco atual:       
                            Risco: {risco.Nome}
                            Severidade: {severidade.Nome}
                            Probabilidade: {probabilidade.Nome}
                            Fonte geradora: {itemRisco.FonteGeradora}
                            Procedimentos aplicáveis: {itemRisco.ProcedimentosAplicaveis}";
                }
                else if (!string.IsNullOrEmpty(itemRisco.ProcedimentosAplicaveis) && !string.IsNullOrEmpty(itemRisco.ContraMedidas))
                {
                    conteudo += $@" {Environment.NewLine} 
                            Dados do risco atual:       
                            Risco: {risco.Nome}
                            Severidade: {severidade.Nome}
                            Probabilidade: {probabilidade.Nome}
                            Fonte geradora: {itemRisco.FonteGeradora}
                            Contramedidas: {itemRisco.ContraMedidas}
                            Procedimentos aplicáveis: {itemRisco.ProcedimentosAplicaveis}";
                }

                if (listaEPI != null)
                {
                    int contEPIAtual = 0;
                    foreach (var itemEPI in listaEPI)
                    {
                        var nomeEpi = epiPersistencia.ListarEPIPorId(itemEPI.CodEPI);
                        if (contEPIAtual == 0)
                        {
                            conteudo += $@"{Environment.NewLine} 
                                            Nome dos EPIs: {nomeEpi.Nome}";
                        }
                        else
                        {
                            conteudo += $@", {nomeEpi.Nome}";
                        }
                        contEPIAtual++;
                    }
                }

                if (conteudo.Contains("--- FIM LOG EDIÇÃO ---"))
                    conteudo = conteudo.Replace("--- FIM LOG EDIÇÃO ---", "");

                conteudo += $@"{Environment.NewLine} 
                          --- FIM LOG EDIÇÃO --- ";

                return conteudo;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void ExcluirInventarioAmbiente(long id)
        {
            this.inventarioAmbientePersistencia.ExcluirInventarioAmbiente(id);
        }

        #region ImportacaoPlanilha

        public class ErroImportacao
        {
            public int codigo { get; set; }
            public string descricao { get; set; }
            public string celula { get; set; }
        }

        public class ResultadoImportacao
        {
            public bool status = true;
            public List<ErroImportacao> erros = new List<ErroImportacao>();
        }

        public ResultadoImportacao ImportarPlanilha(string caminho, string eightId)
        {
            ResultadoImportacao resultadoImportacao = new ResultadoImportacao();
            List<InventarioAmbienteModelo> listaInvAImportar = new List<InventarioAmbienteModelo>();
            List<InventarioAmbienteModelo> listaInvAEditar = new List<InventarioAmbienteModelo>();
            List<ErroImportacao> listaErros = new List<ErroImportacao>();

            try
            {
                XLWorkbook wb = new XLWorkbook(caminho);
                var planilha = wb.Worksheets.Worksheet("InventarioAmbiente");

                if (!planilha.Cell("B1").Value.ToString().Contains("Ambiente"))
                    throw new Exception("É necessário realizar a importação de um inventário de ambiente que respeite o modelo!");

                var linhasPreenchidas = planilha.RowsUsed();

                List<IXLRow> linhas = new List<IXLRow>();

                foreach (var item in linhasPreenchidas)
                    linhas.Add(item);

                List<Task> tasks = new List<Task>();
                List<ResultadoProcessamentoPlanilha> resultados = new List<ResultadoProcessamentoPlanilha>();

                //Monoprocessamento
                //if (linhas.Count <= 250)
                //{
                //    ResultadoProcessamentoPlanilha resultado = processaPlanilha(wb, planilha, 2, linhas.Count - 1);
                //    resultados.Add(resultado);
                //}

                if (linhas.Count <= 30)
                {
                    ResultadoProcessamentoPlanilha resultado = processaPlanilha(planilha, 2, linhas.Count - 1);
                    resultados.Add(resultado);
                }

                //Threads
                else
                {
                    int numeroLinhasPorThread = linhas.Count / 3;

                    Task primeiraTask = Task.Factory.StartNew(() =>
                    {
                        ResultadoProcessamentoPlanilha resultado = processaPlanilha(planilha, 2, numeroLinhasPorThread - 1);
                        resultados.Add(resultado);
                    });

                    Task segundaTask = Task.Factory.StartNew(() =>
                    {
                        int inicial = numeroLinhasPorThread;
                        int final = (inicial + numeroLinhasPorThread) - 1;

                        ResultadoProcessamentoPlanilha resultado = processaPlanilha(planilha, inicial, final);
                        resultados.Add(resultado);
                    });

                    Task terceiraTask = Task.Factory.StartNew(() =>
                    {
                        int inicio = numeroLinhasPorThread * 2;
                        int final = linhas.Count - 1;

                        ResultadoProcessamentoPlanilha resultado = processaPlanilha(planilha, inicio, final);
                        resultados.Add(resultado);
                    });

                    Task.WaitAll(primeiraTask, segundaTask, terceiraTask);
                }

                foreach (var resultadoThread in resultados)
                {
                    listaInvAImportar.AddRange(resultadoThread.listaInvAImportar);
                    listaInvAEditar.AddRange(resultadoThread.listaInvAEditar);
                    listaErros.AddRange(resultadoThread.resultadoImportacao.erros);
                }

                //Insere os registros caso não existam erros
                if (listaErros.Any() == false && listaInvAImportar.Any() == true)
                {
                    this.InserirInventarioAmbienteComLocalInstalacao(listaInvAImportar, eightId);
                }

                if (listaErros.Any() == false && listaInvAEditar.Any() == true)
                {
                    EditarInventarioAmbientePlanilha(listaInvAEditar, eightId);
                }

                if (listaErros.Any() == true)
                {
                    resultadoImportacao.status = false;
                    resultadoImportacao.erros = listaErros;
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Ocorreu um erro ao importar a planilha : " + ex.Message);
            }

            return resultadoImportacao;
        }

        private class ResultadoProcessamentoPlanilha
        {
            public ResultadoImportacao resultadoImportacao = new ResultadoImportacao();
            public List<long> riscosImportados = new List<long>();
            public List<long> locaisAImportar = new List<long>();
            public List<InventarioAmbienteModelo> listaInvAImportar = new List<InventarioAmbienteModelo>();
            public List<InventarioAmbienteModelo> listaInvAEditar = new List<InventarioAmbienteModelo>();
            public float tempoProcessamento;
        }

        private ResultadoProcessamentoPlanilha processaPlanilha(IXLWorksheet planilha, int numeroLinhasInicio, int numeroLinhasFim)
        {
            ResultadoProcessamentoPlanilha resultado = new ResultadoProcessamentoPlanilha();

            var watch = System.Diagnostics.Stopwatch.StartNew();

            var linhasPreenchidas = planilha.RowsUsed();

            List<IXLRow> linhas = new List<IXLRow>();

            int valorRisco;
            int valorRiscoAtual = 0;

            foreach (var item in linhasPreenchidas)
                linhas.Add(item);

            for (int i = numeroLinhasInicio; i <= numeroLinhasFim; i++)
            {
                var linha = linhas[i];
                bool ehEdicao = false;

                InventarioAmbienteModelo inventarioAmbienteModelo = new InventarioAmbienteModelo();
                List<RiscoInventarioAmbienteModelo> listaRiscoModelo = new List<RiscoInventarioAmbienteModelo>();
                RiscoInventarioAmbienteModelo riscoModelo = new RiscoInventarioAmbienteModelo();
                EPIRiscoInventarioAmbienteModelo epiModelo = new EPIRiscoInventarioAmbienteModelo();
                NrInventarioAmbienteModelo nrInventarioAmbienteModelo = new NrInventarioAmbienteModelo();
                riscoModelo.EPIRiscoInventarioAmbienteModelo = new List<EPIRiscoInventarioAmbienteModelo>();

                inventarioAmbienteModelo.RISCO_INVENTARIO_AMBIENTE = new List<RiscoInventarioAmbienteModelo>();
                inventarioAmbienteModelo.NR_INVENTARIO_AMBIENTE = new List<NrInventarioAmbienteModelo>();
                inventarioAmbienteModelo.LOCAL_INSTALACAO_MODELO = new List<LocalInstalacaoModelo>();

                resultado.riscosImportados.Clear();

                var celulasPreenchidas = linha.CellsUsed();

                foreach (var celula in celulasPreenchidas)
                {
                    string nomeCelulaAtual = celula.Address.ColumnLetter + celula.Address.RowNumber;

                    if (celula.Address.ColumnLetter == "B")
                    {
                        if (string.IsNullOrEmpty(celula.Value.ToString()))
                        {
                            inventarioAmbienteModelo.Codigo = $"INV_AMB_IMPORTACAO";
                        }
                        else
                        {
                            inventarioAmbienteModelo.Codigo = celula.Value.ToString();
                        }
                    }

                    else if (celula.Address.ColumnLetter == "C")
                    {
                        var sistemaOP = this.sistemaOperacionalPersistencia.ListarSistemaOperacionalPorNome(celula.Value.ToString());
                        if (sistemaOP is null) adicionaErroLista(resultado.resultadoImportacao.erros, 1, celula.Address.ColumnLetter + celula.Address.RowNumber, "Ambiente inválido!");
                        else inventarioAmbienteModelo.CodAmbiente = sistemaOP.CodAmbiente;
                    }

                    else if (celula.Address.ColumnLetter == "D")
                        if (celula.Value.ToString().Length > 500) adicionaErroLista(resultado.resultadoImportacao.erros, 9, nomeCelulaAtual, "A descrição excedeu o limite de 500 caracteres!");
                        else inventarioAmbienteModelo.Descricao = celula.Value.ToString();

                    else if (celula.Address.ColumnLetter == "E")
                        if (celula.Value.ToString().Length > 500) adicionaErroLista(resultado.resultadoImportacao.erros, 9, nomeCelulaAtual, "A observação geral excedeu o limite de 500 caracteres!");
                        else inventarioAmbienteModelo.ObservacaoGeral = celula.Value.ToString();

                    else if (celula.Address.ColumnLetter == "F")
                    {
                        if (!string.IsNullOrEmpty(celula.Value.ToString())) adicionaErroLista(resultado.resultadoImportacao.erros, 9, nomeCelulaAtual, "O campo risco geral não pode ser preenchido pois o valor será calculado baseado em outras informações do inventário/risco!");
                    }

                    else if (celula.Address.ColumnLetter == "G")
                    {
                        string[] locaisInstalacao = celula.Value.ToString().Split(';');
                        var itensRepetidos = locaisInstalacao.GroupBy(x => x)
                                   .Where(g => g.Count() > 1)
                                   .Select(y => y.Key)
                                   .ToList();
                        if (itensRepetidos.Count > 0)
                        {
                            adicionaErroLista(resultado.resultadoImportacao.erros, 2,
                                   celula.Address.ColumnLetter + celula.Address.RowNumber, "Existem locais de instalação repetidos para o mesmo inventário de ambiente.");
                        }
                        else
                        {
                            foreach (string item in locaisInstalacao)
                            {
                                string[] locaisInstalacaoPorNivel = item.Split('-');

                                LOCAL_INSTALACAO localInstalacaoSplit = new LOCAL_INSTALACAO();

                                if (locaisInstalacaoPorNivel.Length == 1)
                                {
                                    localInstalacaoSplit.N1 = locaisInstalacaoPorNivel[0];
                                    localInstalacaoSplit.N2 = Constantes.LOCAL_INSTALACAO_BASE;
                                }
                                else if (locaisInstalacaoPorNivel.Length == 2)
                                {
                                    localInstalacaoSplit.N1 = locaisInstalacaoPorNivel[0];
                                    localInstalacaoSplit.N2 = locaisInstalacaoPorNivel[1];
                                    localInstalacaoSplit.N3 = Constantes.LOCAL_INSTALACAO_BASE;
                                }
                                else if (locaisInstalacaoPorNivel.Length == 3)
                                {
                                    localInstalacaoSplit.N1 = locaisInstalacaoPorNivel[0];
                                    localInstalacaoSplit.N2 = locaisInstalacaoPorNivel[1];
                                    localInstalacaoSplit.N3 = locaisInstalacaoPorNivel[2];
                                    localInstalacaoSplit.N4 = Constantes.LOCAL_INSTALACAO_BASE;
                                }
                                else if (locaisInstalacaoPorNivel.Length == 4)
                                {
                                    localInstalacaoSplit.N1 = locaisInstalacaoPorNivel[0];
                                    localInstalacaoSplit.N2 = locaisInstalacaoPorNivel[1];
                                    localInstalacaoSplit.N3 = locaisInstalacaoPorNivel[2];
                                    localInstalacaoSplit.N4 = locaisInstalacaoPorNivel[3];
                                    localInstalacaoSplit.N5 = Constantes.LOCAL_INSTALACAO_BASE;
                                }
                                else if (locaisInstalacaoPorNivel.Length == 5)
                                {
                                    localInstalacaoSplit.N1 = locaisInstalacaoPorNivel[0];
                                    localInstalacaoSplit.N2 = locaisInstalacaoPorNivel[1];
                                    localInstalacaoSplit.N3 = locaisInstalacaoPorNivel[2];
                                    localInstalacaoSplit.N4 = locaisInstalacaoPorNivel[3];
                                    localInstalacaoSplit.N5 = locaisInstalacaoPorNivel[4];
                                    localInstalacaoSplit.N6 = Constantes.LOCAL_INSTALACAO_BASE;
                                }
                                else
                                {
                                    localInstalacaoSplit.N1 = locaisInstalacaoPorNivel[0];
                                    localInstalacaoSplit.N2 = locaisInstalacaoPorNivel[1];
                                    localInstalacaoSplit.N3 = locaisInstalacaoPorNivel[2];
                                    localInstalacaoSplit.N4 = locaisInstalacaoPorNivel[3];
                                    localInstalacaoSplit.N5 = locaisInstalacaoPorNivel[4];
                                    localInstalacaoSplit.N6 = locaisInstalacaoPorNivel[5];
                                }

                                //var localInstalacao = this.localInstalacaoPersistencia.ListarLocalInstalacaoPorNome(item);
                                var localInstalacao = this.localInstalacaoPersistencia.ValidaLocalPorNivelImportacao(localInstalacaoSplit);

                                if (localInstalacao is null)
                                    adicionaErroLista(resultado.resultadoImportacao.erros, 2,
                                        celula.Address.ColumnLetter + celula.Address.RowNumber, "Local de instalação '" + item + "' inválido!");
                                else
                                {
                                    //Verifica na lista, se o local de instalação já foi atribuído para outro inventário
                                    if (!resultado.locaisAImportar.Contains(localInstalacao.CodLocalInstalacao))
                                    {
                                        var inventarioAmbiente = inventarioAmbientePersistencia.ListarInventarioAmbientePorCodigo(inventarioAmbienteModelo.Codigo);
                                        //Verifica se o local de instalação já foi atribuído para outro inventário
                                        if (localInstalacao.CodInventarioAmbiente != (int)Constantes.LocalInstalacao.SEM_ASSOCIACAO &&
                                            (inventarioAmbiente != null && inventarioAmbienteModelo.Codigo != inventarioAmbiente.Codigo))
                                        {
                                            adicionaErroLista(resultado.resultadoImportacao.erros, 2,
                                            celula.Address.ColumnLetter + celula.Address.RowNumber, $"O local de instalação { localInstalacao.Nome } " +
                                            $"já está associado ao inventário {localInstalacao.CodInventarioAmbiente}");
                                        }
                                        else
                                        {
                                            if (inventarioAmbiente != null && (inventarioAmbienteModelo.Codigo == inventarioAmbiente.Codigo))
                                            {
                                                ehEdicao = true;
                                            }
                                            LocalInstalacaoModelo localInstalacaoModelo = new LocalInstalacaoModelo();
                                            localInstalacaoModelo.CodLocalInstalacao = localInstalacao.CodLocalInstalacao;
                                            localInstalacaoModelo.CodInventarioAmbiente = localInstalacao.CodInventarioAmbiente;
                                            localInstalacaoModelo.CodPeso = localInstalacao.CodPeso;
                                            localInstalacaoModelo.CodPerfilCatalogo = localInstalacao.CodPerfilCatalogo;
                                            localInstalacaoModelo.N1 = localInstalacao.N1;
                                            localInstalacaoModelo.N2 = localInstalacao.N2;
                                            localInstalacaoModelo.N3 = localInstalacao.N3;
                                            localInstalacaoModelo.N4 = localInstalacao.N4;
                                            localInstalacaoModelo.N5 = localInstalacao.N5;
                                            localInstalacaoModelo.N6 = localInstalacao.N6;
                                            localInstalacaoModelo.Nome = localInstalacao.Nome;
                                            localInstalacaoModelo.Descricao = localInstalacao.Descricao;
                                            inventarioAmbienteModelo.LOCAL_INSTALACAO_MODELO.Add(localInstalacaoModelo);
                                            resultado.locaisAImportar.Add(localInstalacao.CodLocalInstalacao);
                                        }
                                    }
                                    else
                                    {
                                        adicionaErroLista(resultado.resultadoImportacao.erros, 2,
                                              celula.Address.ColumnLetter + celula.Address.RowNumber, $"O local de instalação { item } já foi " +
                                              $"associado à outro inventário nesta planilha!");
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        string nomeColuna = planilha.Cell(celula.Address.ColumnLetter + "3").Value.ToString().ToUpper();
                        string nomeCelula = celula.Address.ColumnLetter + celula.Address.RowNumber;

                        //MAPEAMENTO DE RISCO
                        if (nomeColuna.Contains("RISCO_"))
                        {
                            riscoModelo = new RiscoInventarioAmbienteModelo();
                            riscoModelo.EPIRiscoInventarioAmbienteModelo = new List<EPIRiscoInventarioAmbienteModelo>();

                            var risco = this.riscoPersistencia.ListarRiscoPorNomeETipo(celula.Value.ToString(), null);
                            if (risco is null) adicionaErroLista(resultado.resultadoImportacao.erros, 3, nomeCelula, "Risco ou tipo risco inválido!");
                            else
                                riscoModelo.CodRiscoAmbiente = risco.CodRisco;

                            if (resultado.riscosImportados.Contains(riscoModelo.CodRiscoAmbiente))
                                adicionaErroLista(resultado.resultadoImportacao.erros, 3, nomeCelula, $"O risco {celula.Value.ToString()} ja existe no inventário {nomeCelula}");

                            resultado.riscosImportados.Add(riscoModelo.CodRiscoAmbiente);
                        }
                        else if (nomeColuna.Contains("SEVERIDADE"))
                        {
                            var severidade = this.severidadePersistencia.ListarSeveridadePorNome(celula.Value.ToString());
                            if (severidade is null) adicionaErroLista(resultado.resultadoImportacao.erros, 4, nomeCelula, "Severidade inválida!");
                            else riscoModelo.CodSeveridade = severidade.CodSeveridade;
                        }
                        else if (nomeColuna.Contains("PROBABILIDADE"))
                        {
                            var probabilidade = this.probabilidadePersistencia.ListarProbabilidadePorNome(celula.Value.ToString());
                            if (probabilidade is null) adicionaErroLista(resultado.resultadoImportacao.erros, 5, nomeCelula, "Probabilidade inválida!");
                            else riscoModelo.CodProbabilidade = probabilidade.CodProbabilidade;
                        }
                        else if (nomeColuna.Contains("FONTE"))
                        {
                            var fonteGeradora = celula.Value.ToString();

                            if (String.IsNullOrEmpty(fonteGeradora))
                            {
                                adicionaErroLista(resultado.resultadoImportacao.erros, 6, nomeCelula, "É necessário informar a fonte geradora do risco "
                                    + celula.Value.ToString());
                            }
                            else
                            {
                                if (fonteGeradora is null) adicionaErroLista(resultado.resultadoImportacao.erros, 6, nomeCelula, "Fonte geradora inválida!");
                                else if (fonteGeradora.Length > 500) adicionaErroLista(resultado.resultadoImportacao.erros, 6, nomeCelula, "Fonte geradora excedeu o limite de 500 caracteres!");
                                else riscoModelo.FonteGeradora = fonteGeradora;
                            }
                        }
                        else if (nomeColuna.Contains("PROCEDIMENTOS"))
                        {
                            var procAplicaveis = celula.Value.ToString();

                            if (!String.IsNullOrEmpty(procAplicaveis))
                            {
                                if (procAplicaveis.Length > 500) adicionaErroLista(resultado.resultadoImportacao.erros, 7, nomeCelula, "Procedimento(s) aplicável(is) excedeu o limite de 500 caracteres!");
                                else riscoModelo.ProcedimentosAplicaveis = procAplicaveis;
                            }
                        }
                        else if (nomeColuna.Contains("EPI"))
                        {
                            if (!string.IsNullOrEmpty(celula.Value.ToString()))
                            {
                                string[] epis = celula.Value.ToString().Split(';');

                                foreach (var equip in epis)
                                {
                                    string[] niveisEPI = equip.ToString().Split('/');

                                    if (niveisEPI.Count() == 0)
                                        adicionaErroLista(resultado.resultadoImportacao.erros, 8, nomeCelula, "É necessário informar um EPI");

                                    var epi = this.epiPersistencia.ListarEPIPorNivel(equip.ToString());

                                    if (epi is null)
                                        adicionaErroLista(resultado.resultadoImportacao.erros, 8, nomeCelula, "EPI '" + equip.ToString() + "' inválido!");
                                    else
                                    {
                                        long codRisco = riscoModelo.CodRiscoAmbiente;

                                        if (codRisco != 0)
                                        {
                                            epiModelo = new EPIRiscoInventarioAmbienteModelo();
                                            epiModelo.CodEPI = epi.CodEPI;
                                            epiModelo.CodRiscoInventarioAmbiente = riscoModelo.CodRiscoAmbiente;
                                            riscoModelo.EPIRiscoInventarioAmbienteModelo.Add(epiModelo);
                                        }
                                    }

                                }
                            }
                        }
                        else if (nomeColuna.Contains("CONTRAMEDIDAS"))
                        {
                            var cm = celula.Value.ToString();

                            if (!String.IsNullOrEmpty(cm))
                            {
                                if (cm.Length > 500) adicionaErroLista(resultado.resultadoImportacao.erros, 7, nomeCelula, "Contramedidas excedeu o limite de 500 caracteres!");
                                else riscoModelo.ContraMedidas = cm;
                            }
                        }

                        //MAPEAMENTO DE NR
                        else if (nomeColuna.Contains("CÓDIGO NR"))
                        {
                            string[] nrs = celula.Value.ToString().Split(';');

                            foreach (string item in nrs)
                            {
                                var nr = this.nrPersistencia.ListarNRPorCodigo(item);
                                if (nr is null)
                                    adicionaErroLista(resultado.resultadoImportacao.erros, 9, nomeCelula, "NR '" + celula.Value.ToString() + "' inválida!");
                                else
                                {
                                    nrInventarioAmbienteModelo = new NrInventarioAmbienteModelo();
                                    nrInventarioAmbienteModelo.CodNR = nr.CodNR;
                                    inventarioAmbienteModelo.NR_INVENTARIO_AMBIENTE.Add(nrInventarioAmbienteModelo);
                                }
                            }
                        }

                        listaRiscoModelo.Add(riscoModelo);
                    }
                }
                listaRiscoModelo = listaRiscoModelo.Distinct().ToList();
                ValidarPreenchimentoCampoObrigatorios(resultado.resultadoImportacao.erros, listaRiscoModelo, linha.RowNumber());

                if (resultado.resultadoImportacao.erros.Count == 0)
                {
                    //Adiciona RISCO
                    inventarioAmbienteModelo.RISCO_INVENTARIO_AMBIENTE.AddRange(listaRiscoModelo);

                    if (riscoModelo.CodRiscoAmbiente == 0)
                        throw new Exception("O preenchimento de riscos é obrigatório!");

                    //Calcula Risco Geral
                    valorRisco = CalcularRiscoTotalLista((int)riscoModelo.CodProbabilidade, (int)riscoModelo.CodSeveridade);

                    if (valorRisco > valorRiscoAtual)
                        valorRiscoAtual = valorRisco;

                    //Atribui valor do Risco Geral
                    inventarioAmbienteModelo.RiscoGeral = valorRiscoAtual;
                    if (ehEdicao == false)
                    {
                        resultado.listaInvAImportar.Add(inventarioAmbienteModelo);
                    }
                    else
                    {
                        resultado.listaInvAEditar.Add(inventarioAmbienteModelo);
                        ehEdicao = false;
                    }
                }
            }

            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            elapsedMs = (elapsedMs / 1000);

            resultado.tempoProcessamento = elapsedMs;

            return resultado;
        }

        public void InserirInventarioAmbienteComLocalInstalacao(List<InventarioAmbienteModelo> inventarios, string eightId)
        {
            using (var entities = new DB_APRPTEntities())
            {
                //IMPORTANTE: É NECESSÁRIO DESABILITAR ESSA OPÇÃO PARA OTIMIZAR OS INSERTS
                entities.Configuration.AutoDetectChangesEnabled = false;

                try
                {
                    using (var transaction = entities.Database.BeginTransaction())
                    {
                        try
                        {
                            var result = mapeamentoLocalInventarioAmbientePersistencia.InserirInventarioAmbienteComLocalInstalacao(inventarios, entities);
                            foreach (var itemInventario in result)
                            {
                                InventarioAmbienteModelo inventarioModelo = new InventarioAmbienteModelo();
                                inventarioModelo.Ativo = itemInventario.Ativo;
                                inventarioModelo.CodAmbiente = itemInventario.CodAmbiente;
                                inventarioModelo.Codigo = itemInventario.Codigo;
                                inventarioModelo.CodInventarioAmbiente = itemInventario.CodInventarioAmbiente;
                                inventarioModelo.DataAtualizacao = itemInventario.DataAtualizacao;
                                inventarioModelo.Descricao = itemInventario.Descricao;
                                inventarioModelo.RiscoGeral = itemInventario.RiscoGeral;
                                inventarioModelo.ObservacaoGeral = itemInventario.ObservacaoGeral;
                                inventarioModelo.EightIDUsuarioModificador = eightId;

                                inventarioModelo.LOCAL_INSTALACAO_MODELO = new List<LocalInstalacaoModelo>();
                                foreach (var itemLocal in itemInventario.LOCAL_INSTALACAO)
                                {
                                    LocalInstalacaoModelo localModelo = new LocalInstalacaoModelo();
                                    localModelo.CodInventarioAmbiente = itemLocal.CodInventarioAmbiente;
                                    localModelo.CodLocalInstalacao = itemLocal.CodLocalInstalacao;
                                    localModelo.CodPerfilCatalogo = itemLocal.CodPerfilCatalogo;
                                    localModelo.CodPeso = itemLocal.CodPeso;
                                    localModelo.Descricao = itemLocal.Descricao;
                                    localModelo.N1 = itemLocal.N1;
                                    localModelo.N2 = itemLocal.N2;
                                    localModelo.N3 = itemLocal.N3;
                                    localModelo.N4 = itemLocal.N4;
                                    localModelo.N5 = itemLocal.N5;
                                    localModelo.N6 = itemLocal.N6;
                                    localModelo.Nome = itemLocal.Nome;
                                    inventarioModelo.LOCAL_INSTALACAO_MODELO.Add(localModelo);
                                }

                                inventarioModelo.RISCO_INVENTARIO_AMBIENTE = new List<RiscoInventarioAmbienteModelo>();
                                foreach (var itemRisco in itemInventario.RISCO_INVENTARIO_AMBIENTE)
                                {
                                    RiscoInventarioAmbienteModelo riscoModelo = new RiscoInventarioAmbienteModelo();
                                    riscoModelo.Ativo = itemRisco.Ativo;
                                    riscoModelo.CodInventarioAmbiente = itemRisco.CodInventarioAmbiente;
                                    riscoModelo.CodProbabilidade = itemRisco.CodInventarioAmbiente;
                                    riscoModelo.CodRiscoAmbiente = itemRisco.CodInventarioAmbiente;
                                    riscoModelo.CodRiscoInventarioAmbiente = itemRisco.CodRiscoInventarioAmbiente;
                                    riscoModelo.CodSeveridade = itemRisco.CodSeveridade;
                                    riscoModelo.ContraMedidas = itemRisco.ContraMedidas;
                                    riscoModelo.FonteGeradora = itemRisco.FonteGeradora;
                                    riscoModelo.ProcedimentosAplicaveis = itemRisco.ProcedimentosAplicaveis;

                                    riscoModelo.EPIRiscoInventarioAmbienteModelo = new List<EPIRiscoInventarioAmbienteModelo>();

                                    foreach (var itemEPI in itemRisco.EPI_RISCO_INVENTARIO_AMBIENTE)
                                    {
                                        EPIRiscoInventarioAmbienteModelo epiModelo = new EPIRiscoInventarioAmbienteModelo();

                                        epiModelo.CodEPI = itemEPI.CodEPI;
                                        epiModelo.CodEpiRiscoInventarioAmbiente = itemEPI.CodEpiRiscoInventarioAmbiente;
                                        epiModelo.CodRiscoInventarioAmbiente = itemEPI.CodRiscoInventarioAmbiente;
                                        riscoModelo.EPIRiscoInventarioAmbienteModelo.Add(epiModelo);
                                    }
                                    inventarioModelo.RISCO_INVENTARIO_AMBIENTE.Add(riscoModelo);
                                }

                                inventarioModelo.NR_INVENTARIO_AMBIENTE = new List<NrInventarioAmbienteModelo>();
                                foreach (var itemLocal in itemInventario.NR_INVENTARIO_AMBIENTE)
                                {
                                    NrInventarioAmbienteModelo nrModelo = new NrInventarioAmbienteModelo();
                                    nrModelo.Ativo = itemLocal.Ativo;
                                    nrModelo.CodInventarioAmbiente = itemLocal.CodInventarioAmbiente;
                                    nrModelo.CodNR = itemLocal.CodNR;
                                    nrModelo.CodNRInventarioAmbiente = itemLocal.CodNRInventarioAmbiente;

                                    inventarioModelo.NR_INVENTARIO_AMBIENTE.Add(nrModelo);
                                }

                                logInventarioAmbientePersistencia.Inserir(inventarioModelo, itemInventario.CodInventarioAmbiente, entities);
                            }


                            transaction.Commit();
                        }
                        catch (Exception)
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Erro ao inserir os dados da planilha no banco. \n " + ex.Message);
                }
                finally
                {
                    entities.Configuration.AutoDetectChangesEnabled = true;
                }
            }
        }

        public void EditarInventarioAmbientePlanilha(List<InventarioAmbienteModelo> inventarios, string eightId)
        {
            using (var entities = new DB_APRPTEntities())
            {
                //IMPORTANTE: É NECESSÁRIO DESABILITAR ESSA OPÇÃO PARA OTIMIZAR OS INSERTS
                entities.Configuration.AutoDetectChangesEnabled = false;

                try
                {
                    using (var transaction = entities.Database.BeginTransaction())
                    {
                        try
                        {
                            foreach (var inventarioAmbiente in inventarios)
                            {
                                var ultimoInventario = entities.INVENTARIO_AMBIENTE.Where(x => x.Codigo == inventarioAmbiente.Codigo).OrderByDescending(x => x.CodInventarioAmbiente).FirstOrDefault();

                                InventarioAmbienteModelo inventarioModelo = new InventarioAmbienteModelo();
                                inventarioModelo.Ativo = inventarioAmbiente.Ativo;
                                inventarioModelo.CodAmbiente = inventarioAmbiente.CodAmbiente;
                                inventarioModelo.Codigo = inventarioAmbiente.Codigo;
                                inventarioModelo.CodInventarioAmbiente = inventarioAmbiente.CodInventarioAmbiente;
                                inventarioModelo.DataAtualizacao = inventarioAmbiente.DataAtualizacao;
                                inventarioModelo.Descricao = inventarioAmbiente.Descricao;
                                inventarioModelo.RiscoGeral = inventarioAmbiente.RiscoGeral;
                                inventarioModelo.ObservacaoGeral = inventarioAmbiente.ObservacaoGeral;
                                inventarioModelo.EightIDUsuarioModificador = eightId;

                                inventarioModelo.LOCAL_INSTALACAO_MODELO = new List<LocalInstalacaoModelo>();
                                foreach (var itemLocal in inventarioAmbiente.LOCAL_INSTALACAO_MODELO)
                                {
                                    LocalInstalacaoModelo localModelo = new LocalInstalacaoModelo();
                                    localModelo.CodInventarioAmbiente = itemLocal.CodInventarioAmbiente;
                                    localModelo.CodLocalInstalacao = itemLocal.CodLocalInstalacao;
                                    localModelo.CodPerfilCatalogo = itemLocal.CodPerfilCatalogo;
                                    localModelo.CodPeso = itemLocal.CodPeso;
                                    localModelo.Descricao = itemLocal.Descricao;
                                    localModelo.N1 = itemLocal.N1;
                                    localModelo.N2 = itemLocal.N2;
                                    localModelo.N3 = itemLocal.N3;
                                    localModelo.N4 = itemLocal.N4;
                                    localModelo.N5 = itemLocal.N5;
                                    localModelo.N6 = itemLocal.N6;
                                    localModelo.Nome = itemLocal.Nome;
                                    inventarioModelo.LOCAL_INSTALACAO_MODELO.Add(localModelo);
                                }

                                inventarioModelo.RISCO_INVENTARIO_AMBIENTE = new List<RiscoInventarioAmbienteModelo>();
                                foreach (var itemRisco in inventarioAmbiente.RISCO_INVENTARIO_AMBIENTE)
                                {
                                    RiscoInventarioAmbienteModelo riscoModelo = new RiscoInventarioAmbienteModelo();
                                    riscoModelo.Ativo = itemRisco.Ativo;
                                    riscoModelo.CodInventarioAmbiente = itemRisco.CodInventarioAmbiente;
                                    riscoModelo.CodProbabilidade = itemRisco.CodInventarioAmbiente;
                                    riscoModelo.CodRiscoAmbiente = itemRisco.CodInventarioAmbiente;
                                    riscoModelo.CodRiscoInventarioAmbiente = itemRisco.CodRiscoInventarioAmbiente;
                                    riscoModelo.CodSeveridade = itemRisco.CodSeveridade;
                                    riscoModelo.ContraMedidas = itemRisco.ContraMedidas;
                                    riscoModelo.FonteGeradora = itemRisco.FonteGeradora;
                                    riscoModelo.ProcedimentosAplicaveis = itemRisco.ProcedimentosAplicaveis;

                                    riscoModelo.EPIRiscoInventarioAmbienteModelo = new List<EPIRiscoInventarioAmbienteModelo>();

                                    foreach (var itemEPI in itemRisco.EPIRiscoInventarioAmbienteModelo)
                                    {
                                        EPIRiscoInventarioAmbienteModelo epiModelo = new EPIRiscoInventarioAmbienteModelo();

                                        epiModelo.CodEPI = itemEPI.CodEPI;
                                        epiModelo.CodEpiRiscoInventarioAmbiente = itemEPI.CodEpiRiscoInventarioAmbiente;
                                        epiModelo.CodRiscoInventarioAmbiente = itemEPI.CodRiscoInventarioAmbiente;
                                        riscoModelo.EPIRiscoInventarioAmbienteModelo.Add(epiModelo);
                                    }
                                    inventarioModelo.RISCO_INVENTARIO_AMBIENTE.Add(riscoModelo);
                                }

                                inventarioModelo.NR_INVENTARIO_AMBIENTE = new List<NrInventarioAmbienteModelo>();
                                foreach (var itemLocal in inventarioAmbiente.NR_INVENTARIO_AMBIENTE)
                                {
                                    NrInventarioAmbienteModelo nrModelo = new NrInventarioAmbienteModelo();
                                    nrModelo.Ativo = itemLocal.Ativo;
                                    nrModelo.CodInventarioAmbiente = itemLocal.CodInventarioAmbiente;
                                    nrModelo.CodNR = itemLocal.CodNR;
                                    nrModelo.CodNRInventarioAmbiente = itemLocal.CodNRInventarioAmbiente;

                                    inventarioModelo.NR_INVENTARIO_AMBIENTE.Add(nrModelo);
                                }
                                List<LOCAL_INSTALACAO> local = new List<LOCAL_INSTALACAO>();

                                Mapper.Map(inventarioModelo.LOCAL_INSTALACAO_MODELO, local);

                                inventarioModelo.CodInventarioAmbiente = ultimoInventario.CodInventarioAmbiente;

                                var ultimoLog = logInventarioAmbientePersistencia.Editar(local, inventarioModelo, entities);
                                var itemInventario = this.inventarioAmbientePersistencia.InserirPorEdicao(inventarioAmbiente, entities);

                                var splitCod = ultimoLog.CodInventariosAntigos.Split(',');
                                var codInvAntigo = splitCod.LastOrDefault();

                                if (!string.IsNullOrEmpty(codInvAntigo))
                                {
                                    logInventarioAmbientePersistencia.AtualizarCodInventarioLog(ultimoInventario.CodInventarioAmbiente, itemInventario.CodInventarioAmbiente, entities);
                                }

                                else
                                {
                                    logInventarioAmbientePersistencia.AtualizarCodInventarioLog(0, itemInventario.CodInventarioAmbiente, entities);
                                }
                            }

                            transaction.Commit();
                        }
                        catch (Exception)
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Erro ao inserir os dados da planilha no banco. \n " + ex.Message);
                }
                finally
                {
                    entities.Configuration.AutoDetectChangesEnabled = true;
                }
            }

        }

        private void adicionaErroLista(List<ErroImportacao> lista, int codigo, string celula, string descricao)
        {
            ErroImportacao erro = new ErroImportacao();
            erro.codigo = codigo;
            erro.celula = celula;
            erro.descricao = descricao;
            lista.Add(erro);
        }

        private void ValidarPreenchimentoCampoObrigatorios(List<ErroImportacao> listaDeErros, List<RiscoInventarioAmbienteModelo> listaRiscoModelo, int linha)
        {
            foreach (var riscoModelo in listaRiscoModelo)
            {
                if (riscoModelo.EPIRiscoInventarioAmbienteModelo.Any() == false && string.IsNullOrEmpty(riscoModelo.ProcedimentosAplicaveis) && string.IsNullOrEmpty(riscoModelo.ContraMedidas))
                {
                    adicionaErroLista(listaDeErros, 10, "", $"Os campos contramedida, epi e procedimento aplicável estão nulos na linha {linha}. Gentileza preencher no mínimo um desses campos!");
                }
            }
        }

        public int CalcularRiscoTotalLista(int codProbabilidade, int codSeveridade)
        {
            int maiorRisco = 0;

            decimal valorSeveridade;
            int valorProbabilidade;

            var probabilidade = probabilidadePersistencia.ListarProbabilidadePorId(codProbabilidade);
            var severidade = severidadePersistencia.ListarSeveridadePorId(codSeveridade);

            valorProbabilidade = probabilidade.Peso;
            valorSeveridade = severidade.Indice;

            int riscoAtual = CalcularRiscoAmbiente(valorProbabilidade, valorSeveridade);

            if (riscoAtual > maiorRisco)
                maiorRisco = riscoAtual;


            return maiorRisco;
        }

        public int CalcularRiscoTotalTela(RiscoTotalAmbienteModelo riscoTotalAmbienteModelo)
        {
            decimal valorSeveridade;
            int valorProbabilidade;

            var probabilidade = probabilidadePersistencia.ListarProbabilidadePorId(riscoTotalAmbienteModelo.CodProbabilidade);
            var severidade = severidadePersistencia.ListarSeveridadePorId(riscoTotalAmbienteModelo.CodSeveridade);

            valorProbabilidade = probabilidade.Peso;
            valorSeveridade = severidade.Indice;

            int valorRiscoTotal = CalcularRiscoAmbiente(valorProbabilidade, valorSeveridade);

            return valorRiscoTotal;
        }

        #endregion

    }

}