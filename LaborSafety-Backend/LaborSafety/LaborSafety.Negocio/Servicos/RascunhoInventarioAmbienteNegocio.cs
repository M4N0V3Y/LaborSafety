using AutoMapper;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LaborSafety.Dominio.Modelos;
using LaborSafety.Negocio.Interfaces;
using LaborSafety.Negocio.Validadores.Interface;
using LaborSafety.Persistencia;
using LaborSafety.Persistencia.Interfaces;
using LaborSafety.Utils.Constantes;

namespace LaborSafety.Negocio.Servicos
{
    public class RascunhoInventarioAmbienteNegocio : IRascunhoInventarioAmbienteNegocio
    {
        private readonly IRascunhoInventarioAmbientePersistencia rascunhoInventarioAmbientePersistencia;
        private readonly ILocalInstalacaoPersistencia localInstalacaoPersistencia;
        private readonly Validador<RascunhoInventarioAmbienteModelo> validadorRascunhoInventarioAmbiente;
        private readonly IInventariosAmbientePersistencia inventariosAmbiente;
        private readonly IEPIPersistencia epiPersistencia;
        private readonly ILogInventarioAmbientePersistencia logInventarioAmbientePersistencia;
        public RascunhoInventarioAmbienteNegocio(IRascunhoInventarioAmbientePersistencia rascunhoInventarioAmbientePersistencia,
            ILocalInstalacaoPersistencia localInstalacaoPersistencia, Validador<RascunhoInventarioAmbienteModelo> validadorRascunhoInventarioAmbiente,
            IInventariosAmbientePersistencia inventariosAmbiente, IEPIPersistencia epiPersistencia, ILogInventarioAmbientePersistencia logInventarioAmbientePersistencia)
        {
            this.rascunhoInventarioAmbientePersistencia = rascunhoInventarioAmbientePersistencia;
            this.localInstalacaoPersistencia = localInstalacaoPersistencia;
            this.validadorRascunhoInventarioAmbiente = validadorRascunhoInventarioAmbiente;
            this.inventariosAmbiente = inventariosAmbiente;
            this.epiPersistencia = epiPersistencia;
            this.logInventarioAmbientePersistencia = logInventarioAmbientePersistencia;
        }

        public void ValidarFiltro(FiltroInventarioAmbienteModelo filtroInventarioAmbienteModelo)
        {
            if (filtroInventarioAmbienteModelo.CodLocaisInstalacao == null)
            {
                throw new Exception("Existem filtros obrigatórios sem preenchimento!");
            }
        }
        public RascunhoInventarioAmbienteModelo MapeamentoRascunhoInventarioAmbiente(RASCUNHO_INVENTARIO_AMBIENTE inventario, List<LOCAL_INSTALACAO> locais)
        {
            RascunhoInventarioAmbienteModelo inventarioAmb = new RascunhoInventarioAmbienteModelo()
            {
                CodRascunhoInventarioAmbiente = inventario.CodRascunhoInventarioAmbiente,
                Codigo = inventario.Codigo,
                CodAmbiente = inventario.CodAmbiente,
                Descricao = inventario.Descricao,
                ObservacaoGeral = inventario.ObservacaoGeral,
                RiscoGeral = inventario.RiscoGeral,
                CodLocalInstalacao = inventario.CodLocalInstalacao
            };

            inventarioAmb.NR_RASCUNHO_INVENTARIO_AMBIENTE = new List<NrRascunhoInventarioAmbienteModelo>();
            inventarioAmb.RISCO_RASCUNHO_INVENTARIO_AMBIENTE = new List<RiscoRascunhoInventarioAmbienteModelo>();
            inventarioAmb.LOCAL_INSTALACAO_MODELO = new List<LocalInstalacaoModelo>();

            Mapper.Map(inventario.NR_RASCUNHO_INVENTARIO_AMBIENTE, inventarioAmb.NR_RASCUNHO_INVENTARIO_AMBIENTE);
            Mapper.Map(inventario.RISCO_RASCUNHO_INVENTARIO_AMBIENTE, inventarioAmb.RISCO_RASCUNHO_INVENTARIO_AMBIENTE);

            List<RiscoRascunhoInventarioAmbienteModelo> listaRisco = new List<RiscoRascunhoInventarioAmbienteModelo>();
            foreach (var itemRisco in inventario.RISCO_RASCUNHO_INVENTARIO_AMBIENTE)
            {
                RiscoRascunhoInventarioAmbienteModelo risco = new RiscoRascunhoInventarioAmbienteModelo();
                risco.Ativo = true;
                risco.CodRascunhoInventarioAmbiente = itemRisco.CodRascunhoInventarioAmbiente;
                risco.CodProbabilidade = itemRisco.CodProbabilidade;
                risco.CodRiscoAmbiente = itemRisco.CodRisco;
                risco.CodRascunhoRiscoInventarioAmbiente = itemRisco.CodRiscoRascunhoInventarioAmbiente;
                risco.CodSeveridade = itemRisco.CodSeveridade;
                risco.ContraMedidas = itemRisco.ContraMedidas;
                risco.FonteGeradora = itemRisco.FonteGeradora;
                risco.ProcedimentosAplicaveis = itemRisco.ProcedimentosAplicaveis;

                risco.EPIRiscoRascunhoInventarioAmbiente = new List<EPIRiscoRascunhoInventarioAmbienteModelo>();

                var listaEPI = itemRisco.EPI_RISCO_RASCUNHO_INVENTARIO_AMBIENTE
                    .Where(a => a.CodRiscoRascunhoInventarioAmbiente == itemRisco.CodRiscoRascunhoInventarioAmbiente).ToList();

                Mapper.Map(listaEPI, risco.EPIRiscoRascunhoInventarioAmbiente);

                listaRisco.Add(risco);
            }

            inventarioAmb.RISCO_RASCUNHO_INVENTARIO_AMBIENTE = listaRisco;

            if (locais != null)
            {
                
                foreach (var itemLi in locais)
                {
                    LocalInstalacaoModelo li = new LocalInstalacaoModelo();
                    li.CodLocalInstalacao = itemLi.CodLocalInstalacao;
                    li.CodInventarioAmbiente = itemLi.CodInventarioAmbiente;
                    li.CodPeso = itemLi.CodPeso;
                    li.CodPerfilCatalogo = itemLi.CodPerfilCatalogo;
                    li.N1 = itemLi.N1;
                    li.N2 = itemLi.N2;
                    li.N3 = itemLi.N3;
                    li.N4 = itemLi.N4;
                    li.N5 = itemLi.N5;
                    li.N6 = itemLi.N6;
                    li.Nome = itemLi.Nome;
                    li.Descricao = itemLi.Descricao;
                    
                    inventarioAmb.LOCAL_INSTALACAO_MODELO.Add(li);
                }
            }

            return inventarioAmb;
        }

        public RascunhoInventarioAmbienteModelo ListarRascunhoInventarioAmbientePorId(long id)
        {
            RASCUNHO_INVENTARIO_AMBIENTE inv = this.rascunhoInventarioAmbientePersistencia.ListarRascunhoInventarioAmbientePorId(id);
            if (inv == null)
            {
                throw new KeyNotFoundException("Rascunho de inventário de ambiente não encontrado.");
            }

            var locaisPorVirgula = inv.CodLocalInstalacao.Split(',');
            List<LOCAL_INSTALACAO> listaLocais = new List<LOCAL_INSTALACAO>();

            if (!string.IsNullOrEmpty(inv.CodLocalInstalacao))
            {
                foreach (var item in locaisPorVirgula)
                {
                    var local = localInstalacaoPersistencia.ListarLocalInstalacaoPorId(Convert.ToInt64(item));
                    listaLocais.Add(local);
                }
            }

            if (listaLocais.Count > 0)
            {
                foreach (var itemLi in listaLocais)
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
            }

            return MapeamentoRascunhoInventarioAmbiente(inv, listaLocais);
        }

        public RascunhoInventarioAmbienteModelo ListarRascunhoInventarioAmbientePorLI(long li)
        {
            RASCUNHO_INVENTARIO_AMBIENTE inv = this.rascunhoInventarioAmbientePersistencia.ListarRascunhoInventarioAmbientePorLI(li);
            if (inv == null)
            {
                throw new KeyNotFoundException("Rascunho de inventário de ambiente não encontrado.");
            }

            var locaisPorVirgula = inv.CodLocalInstalacao.Split(',');
            List<LOCAL_INSTALACAO> listaLocais = new List<LOCAL_INSTALACAO>();

            foreach (var item in locaisPorVirgula)
            {
                var local = localInstalacaoPersistencia.ListarLocalInstalacaoPorId(Convert.ToInt64(item));
                listaLocais.Add(local);
            }

            return MapeamentoRascunhoInventarioAmbiente(inv, listaLocais);
        }

        public IEnumerable<RascunhoInventarioAmbienteModelo> ListarRascunhoInventarioAmbiente(FiltroInventarioAmbienteModelo filtroInventarioAmbienteModelo)
        {
            try
            {
                List<RascunhoInventarioAmbienteModelo> inventarioAmbienteModelo = new List<RascunhoInventarioAmbienteModelo>();

                ValidarFiltro(filtroInventarioAmbienteModelo);

                IEnumerable<RASCUNHO_INVENTARIO_AMBIENTE> inv = this.rascunhoInventarioAmbientePersistencia.ListarRascunhoInventarioAmbiente(filtroInventarioAmbienteModelo);

                if (inv.Count() <= 0)
                    throw new KeyNotFoundException("Rascunho de inventário de ambiente não encontrado.");

                foreach (RASCUNHO_INVENTARIO_AMBIENTE inventario in inv)
                {
                    inventarioAmbienteModelo.Add(MapeamentoRascunhoInventarioAmbiente(inventario, null));
                }

                return inventarioAmbienteModelo;
            }

            catch (Exception e)
            {
                throw e;
            }
        }

        public class RetornoInsercao
        {
            public bool status;
            public List<String> localModelo = new List<String>();

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

        public RetornoInsercao InserirRascunhoInventarioAmbiente(RascunhoInventarioAmbienteModelo rascunhoInventarioAmbienteModelo)
        {
            RetornoInsercao retornoInsercao = new RetornoInsercao();
            retornoInsercao.localModelo = new List<string>();

            validadorRascunhoInventarioAmbiente.ValidaInsercao(rascunhoInventarioAmbienteModelo);

            List<LocalInstalacaoModelo> locaisInstalacao = rascunhoInventarioAmbienteModelo.LOCAL_INSTALACAO_MODELO;

            using (var entities = new DB_APRPTEntities())
            {

                using (var transaction = entities.Database.BeginTransaction(System.Data.IsolationLevel.Serializable))
                {

                    try
                    {
                        //Limpa os locais do inventário de modelo
                        rascunhoInventarioAmbienteModelo.LOCAL_INSTALACAO_MODELO = null;

                        List<LOCAL_INSTALACAO> locaisComInventarios = new List<LOCAL_INSTALACAO>();

                        //List<LOCAL_INSTALACAO> locais = localInstalacaoPersistencia.ListarTodosLIs(entities);

                        List<string> locaisAInserir = new List<string>();
                        for (int i = 0; i < locaisInstalacao.Count; i++)
                        {
                            var codLocal = locaisInstalacao[i].CodLocalInstalacao;

                            var localEnviado = localInstalacaoPersistencia.ListarLocalInstalacaoPorId(codLocal, entities);

                            //Filtra somente os locais do pai
                            List<LOCAL_INSTALACAO> locaisEFilhos = this.BuscaLocaisEFilhos(entities, localEnviado);

                            //List<LOCAL_INSTALACAO> locaisFilhos =
                            //    rascunhoInventarioAmbientePersistencia.BuscaFilhosPorNivelExcetoInventario(codLocal, entities);

                            foreach (var local in locaisEFilhos)
                                    locaisAInserir.Add(local.CodLocalInstalacao.ToString());
                        }

                        var locaisAInserirDistinct = locaisAInserir.Distinct();

                        rascunhoInventarioAmbienteModelo.CodLocalInstalacao = string.Join(",", locaisAInserirDistinct);

                        RASCUNHO_INVENTARIO_AMBIENTE inventario = rascunhoInventarioAmbientePersistencia.Inserir(rascunhoInventarioAmbienteModelo, entities);

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

        public class Retorno
        {
            public bool status;
            public List<String> localModelo = new List<String>();

        }
        public Retorno EditarRascunhoInventarioAmbiente(RascunhoInventarioAmbienteModelo rascunhoInventarioAmbienteModelo)
        {
            Retorno retornoInsercao = new Retorno();
            retornoInsercao.localModelo = new List<string>();

            validadorRascunhoInventarioAmbiente.ValidaEdicao(rascunhoInventarioAmbienteModelo);

            List<LocalInstalacaoModelo> locaisInstalacaoOrigem = new List<LocalInstalacaoModelo>();

            locaisInstalacaoOrigem.AddRange(rascunhoInventarioAmbienteModelo.LOCAL_INSTALACAO_MODELO);

            List<LocalInstalacaoModelo> locaisInstalacaoAAssociar = new List<LocalInstalacaoModelo>();

            long codInventarioAmbiente = (long)rascunhoInventarioAmbienteModelo.CodRascunhoInventarioAmbiente;

            using (var entities = new DB_APRPTEntities())
            {
                using (var transaction = entities.Database.BeginTransaction(System.Data.IsolationLevel.Serializable))
                {
                    try
                    {
                        if (!rascunhoInventarioAmbienteModelo.novoInventario)
                        {
                            List<string> locaisAInserir = new List<string>();
                            foreach (var itemLi in rascunhoInventarioAmbienteModelo.LOCAL_INSTALACAO_MODELO)
                            {
                                locaisAInserir.Add(itemLi.CodLocalInstalacao.ToString());
                            }
                            rascunhoInventarioAmbienteModelo.CodLocalInstalacao = string.Join(",", locaisAInserir);
                        }

                        rascunhoInventarioAmbientePersistencia.EditarRascunhoInventarioAmbiente(rascunhoInventarioAmbienteModelo, entities, transaction);

                        if (rascunhoInventarioAmbienteModelo.novoInventario)
                        {
                            InventarioAmbienteModelo inventarioAmbienteModelo = new InventarioAmbienteModelo();

                            inventarioAmbienteModelo.CodInventarioAmbiente = (long)rascunhoInventarioAmbienteModelo.CodRascunhoInventarioAmbiente;
                            inventarioAmbienteModelo.Codigo = rascunhoInventarioAmbienteModelo.Codigo;
                            inventarioAmbienteModelo.CodAmbiente = (long)rascunhoInventarioAmbienteModelo.CodAmbiente;
                            inventarioAmbienteModelo.Descricao = rascunhoInventarioAmbienteModelo.Descricao;
                            inventarioAmbienteModelo.ObservacaoGeral = rascunhoInventarioAmbienteModelo.ObservacaoGeral;
                            inventarioAmbienteModelo.RiscoGeral = (int)rascunhoInventarioAmbienteModelo.RiscoGeral;
                            inventarioAmbienteModelo.DataAtualizacao = DateTime.Now;
                            inventarioAmbienteModelo.Ativo = true;

                            List<RiscoInventarioAmbienteModelo> listaRisco = new List<RiscoInventarioAmbienteModelo>();
                            foreach (var itemRiscoRascunho in rascunhoInventarioAmbienteModelo.RISCO_RASCUNHO_INVENTARIO_AMBIENTE)
                            {
                                RiscoInventarioAmbienteModelo risco = new RiscoInventarioAmbienteModelo();

                                risco.Ativo = true;
                                risco.CodInventarioAmbiente = itemRiscoRascunho.CodRascunhoInventarioAmbiente;
                                risco.CodRiscoInventarioAmbiente = itemRiscoRascunho.CodRascunhoRiscoInventarioAmbiente;
                                risco.CodRiscoAmbiente = itemRiscoRascunho.CodRiscoAmbiente;
                                risco.CodSeveridade = itemRiscoRascunho.CodSeveridade;
                                risco.CodProbabilidade = itemRiscoRascunho.CodProbabilidade;
                                risco.FonteGeradora = itemRiscoRascunho.FonteGeradora;
                                risco.ProcedimentosAplicaveis = itemRiscoRascunho.ProcedimentosAplicaveis;
                                risco.ContraMedidas = itemRiscoRascunho.ContraMedidas;

                                risco.EPIRiscoInventarioAmbienteModelo = new List<EPIRiscoInventarioAmbienteModelo>();

                                var listaEPI = itemRiscoRascunho.EPIRiscoRascunhoInventarioAmbiente
                                    .Where(a => a.CodRiscoRascunhoInventarioAmbiente == itemRiscoRascunho.CodRiscoAmbiente).ToList();

                                foreach (var itemListaEpi in listaEPI)
                                {
                                    EPIRiscoInventarioAmbienteModelo epi = new EPIRiscoInventarioAmbienteModelo();

                                    epi.CodEPI = itemListaEpi.CodEPI;
                                    epi.CodEpiRiscoInventarioAmbiente = itemListaEpi.CodEpiRiscoRascunhoInventarioAmbiente;
                                    epi.CodRiscoInventarioAmbiente = itemListaEpi.CodRiscoRascunhoInventarioAmbiente;
                                    risco.EPIRiscoInventarioAmbienteModelo.Add(epi);
                                }

                                listaRisco.Add(risco);
                            }
                            inventarioAmbienteModelo.RISCO_INVENTARIO_AMBIENTE = listaRisco;


                            List<NrInventarioAmbienteModelo> listaNR = new List<NrInventarioAmbienteModelo>();
                            foreach (var itemNrRascunho in rascunhoInventarioAmbienteModelo.NR_RASCUNHO_INVENTARIO_AMBIENTE)
                            {
                                NrInventarioAmbienteModelo nr = new NrInventarioAmbienteModelo();

                                nr.CodNRInventarioAmbiente = itemNrRascunho.CodNrRascunhoInventarioAmbiente;
                                nr.CodNR = itemNrRascunho.CodNR;
                                nr.CodInventarioAmbiente = itemNrRascunho.CodRascunhoInventarioAmbiente;
                                nr.Ativo = true;

                                listaNR.Add(nr);
                            }
                            inventarioAmbienteModelo.NR_INVENTARIO_AMBIENTE = listaNR;

                            //Limpa os locais que vieram
                            rascunhoInventarioAmbienteModelo.LOCAL_INSTALACAO_MODELO.Clear();

                            //List<LOCAL_INSTALACAO> locais = localInstalacaoPersistencia.ListarTodosLIs(entities);

                            foreach (var local in locaisInstalacaoOrigem)
                            {
                                var codLocal = local.CodLocalInstalacao;

                                var localEnviado = localInstalacaoPersistencia.ListarLocalInstalacaoPorId(codLocal, entities);

                                //Filtra somente os locais do pai
                                List<LOCAL_INSTALACAO> locaisEFilhos = this.BuscaLocaisEFilhos(entities, localEnviado);

                                //Busca todos os filhos que possuam o codInventarioAmbiente
                                //List<LOCAL_INSTALACAO> locaisFilhos = rascunhoInventarioAmbientePersistencia.BuscaFilhosPorNivelDoInventario
                                //    (local.CodLocalInstalacao, codInventarioAmbiente, entities);

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
                            inventarioAmbienteModelo.LOCAL_INSTALACAO_MODELO = new List<LocalInstalacaoModelo>();
                            inventarioAmbienteModelo.LOCAL_INSTALACAO_MODELO.AddRange(locaisInstalacaoAAssociar);

                            foreach (var item in inventarioAmbienteModelo.LOCAL_INSTALACAO_MODELO)
                            {
                                var li = localInstalacaoPersistencia.ListarLocalInstalacaoPorId(item.CodLocalInstalacao);

                                if (li.CodInventarioAmbiente != (long)Constantes.InventarioAmbiente.SEM_INVENTARIO)
                                {
                                    retornoInsercao.status = false;

                                    retornoInsercao.localModelo.Add(li.Nome);

                                    throw new Exception($"O local de instalação {li.Nome} já possui um inventário de ambiente associado.");

                                }

                            }

                            var resultadoInsercao = inventariosAmbiente.Inserir(inventarioAmbienteModelo, entities);
                            inventarioAmbienteModelo.EightIDUsuarioModificador = rascunhoInventarioAmbienteModelo.EightIDUsuarioModificador;
                            logInventarioAmbientePersistencia.Inserir(inventarioAmbienteModelo, resultadoInsercao.CodInventarioAmbiente, entities);
                        }

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

        public void ExcluirRascunhoInventarioAmbiente(long id)
        {
            this.rascunhoInventarioAmbientePersistencia.ExcluirRascunhoInventarioAmbiente(id, null);
        }
    }
}