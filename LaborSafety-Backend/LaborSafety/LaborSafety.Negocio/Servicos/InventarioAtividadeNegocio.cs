using LaborSafety.Dominio.Modelos;
using LaborSafety.Negocio.Interfaces;
using LaborSafety.Negocio.Validadores.Interface;
using LaborSafety.Persistencia.Interfaces;
using AutoMapper;
using System.Collections.Generic;
using LaborSafety.Persistencia;
using System;
using ClosedXML.Excel;
using System.Threading.Tasks;
using System.Linq;
using LaborSafety.Utils.Constantes;
using System.Configuration;
using System.IO;
using System.Diagnostics;

namespace LaborSafety.Negocio.Servicos
{
    public class InventarioAtividadeNegocio : IInventariosAtividadeNegocio
    {
        private readonly IInventariosAtividadePersistencia inventarioAtividadePersistencia;
        private readonly Validador<InventarioAtividadeModelo> validadorInventarioAtividade;

        private readonly ILocalInstalacaoPersistencia localInstalacaoPersistencia;
        private readonly IEPIPersistencia epiPersistencia;
        private readonly IRiscoPersistencia riscoPersistencia;
        private readonly ISeveridadePersistencia severidadePersistencia;
        private readonly IProbabilidadePersistencia probabilidadePersistencia;
        private readonly IAtividadePadraoPersistencia atividadePersistencia;
        private readonly IPerfilCatalogoPersistencia perfilCatalogoPersistencia;
        private readonly IPesoPersistencia pesoPersistencia;
        private readonly IDuracaoPersistencia duracaoPersistencia;
        private readonly IDisciplinaPersistencia disciplinaPersistencia;
        private readonly ILogInventarioAtividadePersistencia logInventarioAtividadePersistencia;

        public InventarioAtividadeNegocio(IInventariosAtividadePersistencia inventarioAtividadePersistencia, Validador<InventarioAtividadeModelo> validadorInventarioAtividade,
            ILocalInstalacaoPersistencia localInstalacaoPersistencia, IEPIPersistencia epiPersistencia,
            IRiscoPersistencia riscoPersistencia, ISeveridadePersistencia severidadePersistencia, IProbabilidadePersistencia probabilidadePersistencia,
            IAtividadePadraoPersistencia atividadePersistencia, IPerfilCatalogoPersistencia perfilCatalogoPersistencia, IPesoPersistencia pesoPersistencia,
            IDuracaoPersistencia duracaoPersistencia, IDisciplinaPersistencia disciplinaPersistencia, ILogInventarioAtividadePersistencia logInventarioAtividadePersistencia = null)
        {
            this.inventarioAtividadePersistencia = inventarioAtividadePersistencia;
            this.validadorInventarioAtividade = validadorInventarioAtividade;

            this.localInstalacaoPersistencia = localInstalacaoPersistencia;
            this.epiPersistencia = epiPersistencia;
            this.riscoPersistencia = riscoPersistencia;
            this.severidadePersistencia = severidadePersistencia;
            this.probabilidadePersistencia = probabilidadePersistencia;
            this.atividadePersistencia = atividadePersistencia;
            this.perfilCatalogoPersistencia = perfilCatalogoPersistencia;
            this.pesoPersistencia = pesoPersistencia;
            this.duracaoPersistencia = duracaoPersistencia;
            this.disciplinaPersistencia = disciplinaPersistencia;
            this.logInventarioAtividadePersistencia = logInventarioAtividadePersistencia;
        }

        public int CalcularRiscoAtividade(decimal numSeveridade, int numArt, int numDa, int numPa)
        {
            // Maior valor arredondado para cima de (SEVERIDADE x ART x DA x PA)

            decimal resultadoRisco;
            resultadoRisco = numSeveridade * numArt * numDa * numPa;

            if (resultadoRisco >= 150)
                resultadoRisco = 4;

            else if (resultadoRisco >= 75 && resultadoRisco < 150)
                resultadoRisco = 3;

            else if (resultadoRisco >= Convert.ToDecimal(7.5) && resultadoRisco < 75)
                resultadoRisco = 2;

            else
                resultadoRisco = 1;

            return Convert.ToInt32(Math.Ceiling(resultadoRisco));
        }

        public InventarioAtividadeModelo ListarInventarioAtividadePorId(long id)
        {
            INVENTARIO_ATIVIDADE inv = this.inventarioAtividadePersistencia.ListarInventarioAtividadePorId(id);

            if (inv == null)
                throw new KeyNotFoundException("Inventário de atividade não encontrado.");

            var mapeamento = MapeamentoInventarioAtividade(inv, true);

            return mapeamento;
        }

        public long ListarCodAprPorInventario(long codInventario)
        {
            var inventario = inventarioAtividadePersistencia.ListarInventarioAtividadePorId(codInventario);
            long inventarioComApr=0;

            foreach (var item in inventario.LOCAL_INSTALACAO_INVENTARIO_ATIVIDADE)
            {
                long inv = inventarioAtividadePersistencia.ListarCodAprPorInventario(codInventario, inventario.CodAtividade, inventario.CodDisciplina, item.CodLocalInstalacao, null);

                if (inv > 0)
                    inventarioComApr = inv;
            }

            return inventarioComApr;
        }

        public long ListarCodAprPorInventarioTela(long codInventario)
        {
            var inventario = inventarioAtividadePersistencia.ListarInventarioAtividadePorId(codInventario);
            long inventarioComApr = 0;

            foreach (var item in inventario.LOCAL_INSTALACAO_INVENTARIO_ATIVIDADE)
            {
                long inv = inventarioAtividadePersistencia.ListarCodAprPorInventarioTela(codInventario, inventario.CodAtividade, inventario.CodDisciplina, item.CodLocalInstalacao, null);

                if (inv > 0)
                    inventarioComApr = inv;
            }

            return inventarioComApr;
        }

        public InventarioAtividadeModelo MapeamentoInventarioAtividade(INVENTARIO_ATIVIDADE inventario, bool eFiltroPorID = false)
        {
            InventarioAtividadeModelo inventarioAtv = new InventarioAtividadeModelo()
            {
                CodInventarioAtividade = inventario.CodInventarioAtividade,
                Codigo = inventario.Codigo,
                CodPeso = inventario.CodPeso,
                CodPerfilCatalogo = inventario.CodPerfilCatalogo,
                CodDuracao = inventario.CodDuracao,
                CodAtividade = inventario.CodAtividade,
                CodDisciplina = inventario.CodDisciplina,
                Descricao = inventario.Descricao,
                RiscoGeral = inventario.RiscoGeral,
                ObservacaoGeral = inventario.ObservacaoGeral,
                DataAtualizacao = inventario.DataAtualizacao,
                Ativo = inventario.Ativo
            };

            inventarioAtv.RISCO_INVENTARIO_ATIVIDADE = new List<RiscoInventarioAtividadeModelo>();
            inventarioAtv.LOCAL_INSTALACAO_INVENTARIO_ATIVIDADE = new List<LocalInstalacaoInventarioAtividadeModelo>();

            if (eFiltroPorID)
            {
                //List<LOCAL_INSTALACAO> todosLocais = localInstalacaoPersistencia.ListarTodosLIs();

                DB_APRPTEntities entities = new DB_APRPTEntities();

                for (int i = 0; i < inventario.LOCAL_INSTALACAO_INVENTARIO_ATIVIDADE.Count; i++) 
                {
                    var liInventario = inventario.LOCAL_INSTALACAO_INVENTARIO_ATIVIDADE.ElementAt(i);

                    //var localInstalacao = todosLocais.Where(x => x.CodLocalInstalacao == liInventario.CodLocalInstalacao).FirstOrDefault();
                    var localInstalacao = entities.LOCAL_INSTALACAO.Where(x => x.CodLocalInstalacao == liInventario.CodLocalInstalacao).FirstOrDefault();

                    liInventario.LOCAL_INSTALACAO = localInstalacao;

                    LocalInstalacaoInventarioAtividadeModelo modeloLIInv = new LocalInstalacaoInventarioAtividadeModelo();
                    modeloLIInv.Ativo = liInventario.Ativo;
                    modeloLIInv.CodInventarioAtividade = liInventario.CodInventarioAtividade;
                    modeloLIInv.CodLocalInstalacao = liInventario.CodLocalInstalacao;
                    modeloLIInv.CodLocalInstalacaoInventarioAtividade = liInventario.CodLocalInstalacaoInventarioAtividade;

                    LocalInstalacaoModelo modeloLI = new LocalInstalacaoModelo();
                    modeloLI.CodInventarioAmbiente = liInventario.LOCAL_INSTALACAO.CodInventarioAmbiente;
                    modeloLI.CodLocalInstalacao = liInventario.LOCAL_INSTALACAO.CodLocalInstalacao;
                    modeloLI.CodPerfilCatalogo = liInventario.LOCAL_INSTALACAO.CodPerfilCatalogo;
                    modeloLI.CodPeso = liInventario.LOCAL_INSTALACAO.CodPeso;
                    modeloLI.Descricao = liInventario.LOCAL_INSTALACAO.Descricao;
                    modeloLI.N1 = liInventario.LOCAL_INSTALACAO.N1;
                    modeloLI.N2 = liInventario.LOCAL_INSTALACAO.N2;
                    modeloLI.N3 = liInventario.LOCAL_INSTALACAO.N3;
                    modeloLI.N4 = liInventario.LOCAL_INSTALACAO.N4;
                    modeloLI.N5 = liInventario.LOCAL_INSTALACAO.N5;
                    modeloLI.N6 = liInventario.LOCAL_INSTALACAO.N6;
                    modeloLI.Nome = liInventario.LOCAL_INSTALACAO.Nome;

                    //var local = todosLocais.Where(x => x.CodLocalInstalacao == modeloLI.CodLocalInstalacao).FirstOrDefault();
                    var local = entities.LOCAL_INSTALACAO.Where(x => x.CodLocalInstalacao == modeloLI.CodLocalInstalacao).FirstOrDefault();

                    string n2 = "", n3 = "", n4 = "", n5 = "", n6 = "";

                    if (!string.IsNullOrEmpty(local.N2))
                        n2 = local.N2;

                    if (!string.IsNullOrEmpty(local.N3))
                        n3 = local.N3;

                    if (!string.IsNullOrEmpty(local.N4))
                        n4 = local.N4;

                    if (!string.IsNullOrEmpty(local.N5))
                        n5 = local.N5;

                    if (!string.IsNullOrEmpty(local.N6))
                        n6 = local.N6;

                    if (n2.Contains("000_BASE") || n3.Contains("000_BASE") || n4.Contains("000_BASE") || n5.Contains("000_BASE") || n6.Contains("000_BASE")) {
                        var nomeSeparado = local.Descricao.Split(' ');
                        var nomeCompleto = local.Descricao.Replace(nomeSeparado[0], nomeSeparado[0] + "-000_BASE");
                        modeloLI.Nome = nomeCompleto;
                    }

                    modeloLIInv.LocalInstalacao = modeloLI;

                    inventarioAtv.LOCAL_INSTALACAO_INVENTARIO_ATIVIDADE.Add(modeloLIInv);
                }


                List<RiscoInventarioAtividadeModelo> listaRisco = new List<RiscoInventarioAtividadeModelo>();
                foreach (var itemRisco in inventario.RISCO_INVENTARIO_ATIVIDADE)
                {
                    RiscoInventarioAtividadeModelo risco = new RiscoInventarioAtividadeModelo();
                    risco.Ativo = true;
                    risco.CodInventarioAtividade = itemRisco.CodInventarioAtividade;
                    risco.CodRiscoInventarioAtividade = itemRisco.CodRiscoInventarioAtividade;
                    risco.CodRisco = itemRisco.CodRisco;
                    risco.CodSeveridade = itemRisco.CodSeveridade;
                    risco.FonteGeradora = itemRisco.FonteGeradora;
                    risco.ProcedimentoAplicavel = itemRisco.ProcedimentoAplicavel;
                    risco.ContraMedidas = itemRisco.ContraMedidas;

                    risco.EPIRiscoInventarioAtividadeModelo = new List<EPIRiscoInventarioAtividadeModelo>();

                    var listaEPI = itemRisco.EPI_RISCO_INVENTARIO_ATIVIDADE
                        .Where(a => a.CodRiscoInventarioAtividade == itemRisco.CodRiscoInventarioAtividade).ToList();

                    Mapper.Map(listaEPI, risco.EPIRiscoInventarioAtividadeModelo);

                    listaRisco.Add(risco);
                }

                inventarioAtv.RISCO_INVENTARIO_ATIVIDADE = listaRisco;
            }

            return inventarioAtv;
        }

        public List<InventarioAtividadeModelo> ListarInventarioAtividade(FiltroInventarioAtividadeModelo filtroInventarioAtividadeModelo)
        {
            List<InventarioAtividadeModelo> inventarioAtividadeModelo = new List<InventarioAtividadeModelo>();

            IEnumerable<INVENTARIO_ATIVIDADE> inv = this.inventarioAtividadePersistencia.ListarInventarioAtividade(filtroInventarioAtividadeModelo);

            if (inv == null)
                throw new KeyNotFoundException("Inventário de atividade não encontrado.");

            foreach (INVENTARIO_ATIVIDADE inventario in inv)
                inventarioAtividadeModelo.Add(MapeamentoInventarioAtividade(inventario));

            return inventarioAtividadeModelo;
        }

        
        private List<LOCAL_INSTALACAO> BuscaLocaisEFilhos(DB_APRPTEntities entities, LOCAL_INSTALACAO localEnviado, long idInventario)
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
        

        private List<LOCAL_INSTALACAO> BuscaLocaisEFilhos(LOCAL_INSTALACAO localEnviado, DB_APRPTEntities entities)
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

        public void InserirInventarioAtividade(InventarioAtividadeModelo inventarioAtividadeModelo)
        {
            validadorInventarioAtividade.ValidaInsercao(inventarioAtividadeModelo);

            List<LOCAL_INSTALACAO> locaisInstalacao = new List<LOCAL_INSTALACAO>();

            using (var entities = new DB_APRPTEntities())
            {
                entities.Database.CommandTimeout = 9999;
                using (var transaction = entities.Database.BeginTransaction(System.Data.IsolationLevel.Serializable))
                {
                    try
                    {
                        //List<LOCAL_INSTALACAO> locais = localInstalacaoPersistencia.ListarTodosLIs(entities);

                        for (int i = 0; i < inventarioAtividadeModelo.LOCAL_INSTALACAO_INVENTARIO_ATIVIDADE.Count; i++)
                        {
                            var codLocal = inventarioAtividadeModelo.LOCAL_INSTALACAO_INVENTARIO_ATIVIDADE[i].LocalInstalacao.CodLocalInstalacao;

                            //var localEnviado = localInstalacaoPersistencia.ListarLocalInstalacaoPorId(codLocal, entities);
                            var localEnviado = entities.LOCAL_INSTALACAO.Where(x => x.CodLocalInstalacao == codLocal).FirstOrDefault();

                            //Filtra somente os locais do pai
                            List<LOCAL_INSTALACAO> locaisEFilhos = this.BuscaLocaisEFilhos(localEnviado, entities);

                            //List<LOCAL_INSTALACAO> locaisFilhos =
                            //    inventarioAtividadePersistencia.BuscaFilhosPorNivel(codLocal, entities);

                            locaisInstalacao.AddRange(locaisEFilhos);
                        }

                        List<INVENTARIO_ATIVIDADE> inventarios = inventarioAtividadePersistencia.ListarTodos(entities);

                        foreach (var itemLi in locaisInstalacao)
                        {
                            var inventarioExistente = entities.INVENTARIO_ATIVIDADE.Where(x => x.LOCAL_INSTALACAO_INVENTARIO_ATIVIDADE.Any(y => y.CodLocalInstalacao ==
                            itemLi.CodLocalInstalacao && x.Ativo) && x.CodAtividade == inventarioAtividadeModelo.CodAtividade
                            && x.CodDisciplina == inventarioAtividadeModelo.CodDisciplina && x.Ativo).FirstOrDefault();

                            //var unicoInventario = inventarioAtividadePersistencia.ListarInventarioAtividadePorAtividadeDisciplinaLI(inventarioAtividadeModelo.CodAtividade, inventarioAtividadeModelo.CodDisciplina, itemLi.CodLocalInstalacao, entities);

                            if (inventarioExistente != null)
                            {
                                var atvPadrao = atividadePersistencia.ListarAtividadePorId(inventarioAtividadeModelo.CodAtividade, entities);
                                var disciplina = disciplinaPersistencia.ListarDisciplinaPorId(inventarioAtividadeModelo.CodDisciplina, entities);
                                throw new Exception($"Já existe um inventário de atividade com atividade padrão {atvPadrao.Nome}, disciplina {disciplina.Nome} e local de instalação {itemLi.Nome}");
                            }
                                
                        }

                        var inventario = inventarioAtividadePersistencia.Inserir(inventarioAtividadeModelo, entities, locaisInstalacao);

                        logInventarioAtividadePersistencia.Inserir(inventarioAtividadeModelo, inventario.CodInventarioAtividade, entities);

                        entities.SaveChanges();
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }

            
        }

        private class ProcessamentoThread
        {
            public bool sucesso;
            public List<LOCAL_INSTALACAO> locaisAAssociar = new List<LOCAL_INSTALACAO>();
        }


        public void EditarInventarioAtividade(InventarioAtividadeModelo inventarioAtividadeModelo)
        {
            validadorInventarioAtividade.ValidaEdicao(inventarioAtividadeModelo);

            List<LOCAL_INSTALACAO> locaisInstalacao = new List<LOCAL_INSTALACAO>();

            using (var entities = new DB_APRPTEntities())
            {
                using (var transaction = entities.Database.BeginTransaction(System.Data.IsolationLevel.Serializable))
                {
                    try
                    {
                        List<EPIRiscoInventarioAtividadeModelo> listaEPIs = new List<EPIRiscoInventarioAtividadeModelo>();

                        /*
                        foreach (var itemRisco in inventarioAtividadeModelo.RISCO_INVENTARIO_ATIVIDADE)
                        {
                            if (itemRisco.EPIRiscoInventarioAtividadeModelo.Count > 0)
                            {
                                listaEPIs.AddRange(itemRisco.EPIRiscoInventarioAtividadeModelo);
                                var teste = listaEPIs.Select(x => x.CodEPI).Distinct();

                                List<EPIRiscoInventarioAtividadeModelo> novaLista = new List<EPIRiscoInventarioAtividadeModelo>();

                                foreach (var itemEPI in teste)
                                {
                                    var item = itemRisco.EPIRiscoInventarioAtividadeModelo.Where(x => x.CodEPI == itemEPI).First();
                                    novaLista.Add(item);
                                }
                                itemRisco.EPIRiscoInventarioAtividadeModelo = novaLista;
                            }
                        }*/

                        //List<LOCAL_INSTALACAO> locais = localInstalacaoPersistencia.ListarTodosLIs(entities);

                        for (int i = 0; i < inventarioAtividadeModelo.LOCAL_INSTALACAO_INVENTARIO_ATIVIDADE.Count; i++)
                        {
                            var codLocal = inventarioAtividadeModelo.LOCAL_INSTALACAO_INVENTARIO_ATIVIDADE[i].LocalInstalacao.CodLocalInstalacao;

                            var localEnviado = localInstalacaoPersistencia.ListarLocalInstalacaoPorId(codLocal, entities);

                            //Filtra somente os locais do pai
                            List<LOCAL_INSTALACAO> locaisEFilhos = this.BuscaLocaisEFilhos(entities, localEnviado, codLocal);

                            //List<LOCAL_INSTALACAO> locaisFilhos =
                            //    inventarioAtividadePersistencia.BuscaFilhosPorNivel(codLocal, entities);

                            locaisInstalacao.AddRange(locaisEFilhos);
                        }

                        foreach (var itemLi in locaisInstalacao)
                        {
                            var unicoInventario = inventarioAtividadePersistencia.ListarInventarioAtividadePorAtividadeDisciplinaLIInv(inventarioAtividadeModelo.CodAtividade,
                                inventarioAtividadeModelo.CodDisciplina, itemLi.CodLocalInstalacao, inventarioAtividadeModelo.CodInventarioAtividade, entities);

                            if (unicoInventario != null)
                            {
                                var atvPadrao = atividadePersistencia.ListarAtividadePorId(inventarioAtividadeModelo.CodAtividade);
                                var disciplina = disciplinaPersistencia.ListarDisciplinaPorId(inventarioAtividadeModelo.CodDisciplina);
                                throw new Exception($"Já existe um inventário de atividade com atividade padrão {atvPadrao.Nome}, disciplina {disciplina.Nome} e local de instalação {itemLi.Nome}");
                            }

                        }

                        logInventarioAtividadePersistencia.Editar(inventarioAtividadeModelo, locaisInstalacao, entities);
                        var edicao = inventarioAtividadePersistencia.EditarInventarioAtividade(inventarioAtividadeModelo, entities, locaisInstalacao);
                        
                        logInventarioAtividadePersistencia.AtualizarCodInventarioLog(inventarioAtividadeModelo.CodInventarioAtividade, edicao.CodInventarioAtividade, entities);

                        entities.SaveChanges();
                        transaction.Commit();

                        return;

                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }
        }

        public void EditarLocalInstalacaoInventarioAtividade(long idInventario, long idlI)
        {
            this.inventarioAtividadePersistencia.EditarLocalInstalacaoInventarioAtividade(idInventario, idlI);
        }

        public void EditarRiscoInventarioAtividade(long idInventario, long idRisco)
        {
            this.inventarioAtividadePersistencia.EditarRiscoInventarioAtividade(idInventario, idRisco);
        }

        public void EditarResponsavelInventarioAtividade(long idInventario, long idResponsavel)
        {
            this.inventarioAtividadePersistencia.EditarResponsavelInventarioAtividade(idInventario, idResponsavel);
        }
        public void DesativarInventario(InventarioAtividadeDelecaoComLogModelo inventarioAtividadeDelecao, DB_APRPTEntities entities)
        {

            using (entities = new DB_APRPTEntities())
            {
                using (var transaction = entities.Database.BeginTransaction())
                {
                    try
                    {
                        this.logInventarioAtividadePersistencia.Excluir(inventarioAtividadeDelecao,entities);
                        this.inventarioAtividadePersistencia.DesativarInventario(inventarioAtividadeDelecao.CodInventarioAtividade, entities);
                        transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw e;
                    }

                }

            }
        }

        public void ExcluirInventarioAtividade(long id)
        {
            this.inventarioAtividadePersistencia.ExcluirInventarioAtividade(id);
        }

        #region PlanilhaInventarioAtividade

        public ResultadoImportacao ImportarPlanilha(string path, string eightId)
        {
            ResultadoImportacao resultadoImportacao = new ResultadoImportacao();
            List<InventarioAtividadeModelo> listaInvAEditar = new List<InventarioAtividadeModelo>();
            List<InventarioAtividadeModelo> listaInvAImportar = new List<InventarioAtividadeModelo>();
            List<ErroImportacao> listaErros = new List<ErroImportacao>();

            try
            {
                XLWorkbook wb = new XLWorkbook(path);

                var planilha = wb.Worksheets.Worksheet("InventarioAtividade");

                if(!planilha.Cell("B1").Value.ToString().Contains("Atividade"))
                    throw new Exception("É necessário realizar a importação de um inventário de atividade que respeite o modelo!");

                var linhasPreenchidas = planilha.RowsUsed();

                List<IXLRow> linhas = new List<IXLRow>();

                foreach (var item in linhasPreenchidas)
                {
                    linhas.Add(item);
                }

                List<Task> tasks = new List<Task>();
                List<ResultadoProcessamentoPlanilha> resultados = new List<ResultadoProcessamentoPlanilha>();

                //Monoprocessamento
                if (linhas.Count <= 250)
                {
                    ResultadoProcessamentoPlanilha resultado = processaPlanilha(wb, planilha, 2, linhas.Count - 1);
                    resultados.Add(resultado);
                }
                //Thread
                else
                {
                    int numeroLinhasPorThread = linhas.Count / 3;

                    Task primeiraTask = Task.Factory.StartNew(() =>
                    {
                        ResultadoProcessamentoPlanilha resultado = processaPlanilha(wb, planilha, 2, numeroLinhasPorThread - 1);
                        resultados.Add(resultado);
                    });

                    Task segundaTask = Task.Factory.StartNew(() =>
                    {
                        int inicial = numeroLinhasPorThread;
                        int final = (inicial + numeroLinhasPorThread) - 1;

                        ResultadoProcessamentoPlanilha resultado = processaPlanilha(wb, planilha, inicial, final);
                        resultados.Add(resultado);
                    });

                    Task terceiraTask = Task.Factory.StartNew(() =>
                    {
                        int inicio = numeroLinhasPorThread * 2;
                        int final = linhas.Count - 1;

                        ResultadoProcessamentoPlanilha resultado = processaPlanilha(wb, planilha, inicio, final);
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

                if (listaErros.Any() == false && listaInvAImportar.Any() == true)
                    this.InserirInventarioAtividadeImportacaoLog(listaInvAImportar, eightId);


                if (listaErros.Any() == false && listaInvAEditar.Any() == true)
                    this.EditarInventarioAtividadeImportacaoLog(listaInvAEditar, eightId);

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

        public void InserirInventarioAtividadeImportacaoLog(List<InventarioAtividadeModelo> inventarios, string eightId)
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
                            foreach (var itemInventario in inventarios)
                            {
                                var inventarioInserido = inventarioAtividadePersistencia.InserirItemListaInventarioAtividade(itemInventario, entities);
                                itemInventario.EightIDUsuarioModificador = eightId;
                                logInventarioAtividadePersistencia.Inserir(itemInventario, inventarioInserido.CodInventarioAtividade, entities);
                            }
                            

                            transaction.Commit();
                        }
                        catch (Exception e)
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

        public void EditarInventarioAtividadeImportacaoLog(List<InventarioAtividadeModelo> inventarios, string eightId)
        {
            using (var entities = new DB_APRPTEntities())
            {
                //IMPORTANTE: É NECESSÁRIO DESABILITAR ESSA OPÇÃO PARA OTIMIZAR OS INSERTS
                //entities.Configuration.AutoDetectChangesEnabled = false;
                try
                {
                    using (var transaction = entities.Database.BeginTransaction())
                    {
                        try
                        {
                            foreach (var inventarioAtividade in inventarios)
                            {
                                var ultimoInventario = entities.INVENTARIO_ATIVIDADE.Where(x => x.Codigo == inventarioAtividade.Codigo).OrderByDescending(x => x.CodInventarioAtividade).FirstOrDefault();

                                List<LOCAL_INSTALACAO> locaisInstalacao = new List<LOCAL_INSTALACAO>();

                                inventarioAtividade.EightIDUsuarioModificador = eightId;

                                foreach (var itemLocal in inventarioAtividade.LOCAL_INSTALACAO_INVENTARIO_ATIVIDADE)
                                {
                                    var local = entities.LOCAL_INSTALACAO.Where(x => x.CodLocalInstalacao == itemLocal.CodLocalInstalacao).FirstOrDefault();
                                    locaisInstalacao.Add(local);
                                }

                                inventarioAtividade.CodInventarioAtividade = ultimoInventario.CodInventarioAtividade;
                                var ultimoLog = logInventarioAtividadePersistencia.Editar(inventarioAtividade, locaisInstalacao, entities);
                                
                                var itemInventario = this.inventarioAtividadePersistencia.EditarInventarioAtividadePorImportacao(inventarioAtividade, entities);

                                var splitCod = ultimoLog.CodInventariosAntigos.Split(',');
                                var codInvAntigo = splitCod.LastOrDefault();

                                if (!string.IsNullOrEmpty(codInvAntigo))
                                    logInventarioAtividadePersistencia.AtualizarCodInventarioLog(ultimoInventario.CodInventarioAtividade, itemInventario.CodInventarioAtividade, entities);

                                else
                                    logInventarioAtividadePersistencia.AtualizarCodInventarioLog(0, itemInventario.CodInventarioAtividade, entities);
                            }

                            transaction.Commit();
                        }
                        catch (Exception e)
                        {
                            transaction.Rollback();
                            throw;
                        }
                        finally
                        {
                            //entities.Configuration.AutoDetectChangesEnabled = true;
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

        public class ResultadoProcessamentoPlanilha
        {
            public ResultadoImportacao resultadoImportacao = new ResultadoImportacao();
            public List<long> riscosImportados = new List<long>();
            public List<RiscoInventarioAtividadeModelo> listaRiscosImportados = new List<RiscoInventarioAtividadeModelo>();
            public List<InventarioAtividadeModelo> listaInvAImportar = new List<InventarioAtividadeModelo>();
            public List<InventarioAtividadeModelo> listaInvAEditar = new List<InventarioAtividadeModelo>();
            public long tempoProcessamento;
        }

        class Validacao
        {
            public long codAtividade;
            public long codDisciplina;
            public List<LocalInstalacaoInventarioAtividadeModelo> listaLocais = new List<LocalInstalacaoInventarioAtividadeModelo>();
        }

        private ResultadoProcessamentoPlanilha processaPlanilha(XLWorkbook wb, IXLWorksheet planilha, int numeroLinhasInicio, int numeroLinhasFim)
        {
            List<Validacao> dadosValidados = new List<Validacao>();

            ResultadoProcessamentoPlanilha resultado = new ResultadoProcessamentoPlanilha();

            var watch = System.Diagnostics.Stopwatch.StartNew();

            var linhasPreenchidas = planilha.RowsUsed();

            List<IXLRow> linhas = new List<IXLRow>();

            int valorRisco, valorRiscoAtual = 0;

            foreach (var item in linhasPreenchidas)
            {
                linhas.Add(item);
            }

            for (int i = numeroLinhasInicio; i <= numeroLinhasFim; i++)
            {
                var linha = linhas[i];
                bool ehEdicao = false;
                InventarioAtividadeModelo inventarioAtividadeModelo = new InventarioAtividadeModelo();
                LocalInstalacaoInventarioAtividadeModelo liInventarioAtividadeModelo = new LocalInstalacaoInventarioAtividadeModelo();
                List<RiscoInventarioAtividadeModelo> listaRiscoModelo = new List<RiscoInventarioAtividadeModelo>();
                RiscoInventarioAtividadeModelo riscoModelo = new RiscoInventarioAtividadeModelo();
                EPIRiscoInventarioAtividadeModelo epiRiscoModelo = new EPIRiscoInventarioAtividadeModelo();

                inventarioAtividadeModelo.LOCAL_INSTALACAO_INVENTARIO_ATIVIDADE = new List<LocalInstalacaoInventarioAtividadeModelo>();
                inventarioAtividadeModelo.RISCO_INVENTARIO_ATIVIDADE = new List<RiscoInventarioAtividadeModelo>();
                riscoModelo.EPIRiscoInventarioAtividadeModelo = new List<EPIRiscoInventarioAtividadeModelo>();

                resultado.riscosImportados.Clear();

                var celulasPreenchidas = linha.CellsUsed();

                foreach (var celula in celulasPreenchidas)
                {
                    string nomeCelulaAtual = celula.Address.ColumnLetter + celula.Address.RowNumber;
                    ATIVIDADE_PADRAO atividadePad = new ATIVIDADE_PADRAO();
                    DISCIPLINA disciplinaAtv = new DISCIPLINA();

                    if (celula.Address.ColumnLetter == "B")
                    {
                        if(string.IsNullOrEmpty(celula.Value.ToString()))
                        {
                            inventarioAtividadeModelo.Codigo = $"INV_ATV_IMPORTACAO";
                        }
                        else
                        {
                            inventarioAtividadeModelo.Codigo = celula.Value.ToString();
                        }
                    }
                        

                    else if (celula.Address.ColumnLetter == "C")
                    {
                        disciplinaAtv = this.disciplinaPersistencia.ListarDisciplinaPorNome(celula.Value.ToString());
                        if (disciplinaAtv is null) adicionaErroLista(resultado.resultadoImportacao.erros, 1, celula.Address.ColumnLetter + celula.Address.RowNumber, "Disciplina inválida!");
                        else inventarioAtividadeModelo.CodDisciplina = disciplinaAtv.CodDisciplina;
                    }
                    else if (celula.Address.ColumnLetter == "D")
                    {
                        atividadePad = this.atividadePersistencia.ListarAtividadePorNome(celula.Value.ToString());
                        if (atividadePad is null) adicionaErroLista(resultado.resultadoImportacao.erros, 2, celula.Address.ColumnLetter + celula.Address.RowNumber, "Atividade inválida!");
                        else inventarioAtividadeModelo.CodAtividade = atividadePad.CodAtividadePadrao;
                    }

                    else if (celula.Address.ColumnLetter == "E")
                    {
                        var perfilCat = this.perfilCatalogoPersistencia.ListarPerfilCatalogoPorCodigo(celula.Value.ToString());
                        if (perfilCat is null) adicionaErroLista(resultado.resultadoImportacao.erros, 3, celula.Address.ColumnLetter + celula.Address.RowNumber, "Perfil de catálogo inválido!");
                        else inventarioAtividadeModelo.CodPerfilCatalogo = perfilCat.CodPerfilCatalogo;
                    }

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
                                   celula.Address.ColumnLetter + celula.Address.RowNumber, "Existem locais de instalação repetidos para o mesmo inventário de atividade.");
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
                                {
                                    adicionaErroLista(resultado.resultadoImportacao.erros, 2,
                                        celula.Address.ColumnLetter + celula.Address.RowNumber, "Local de instalação '" + item + "' inválido!");
                                }
                                else
                                {
                                    var unicoInventario = inventarioAtividadePersistencia.ListarInventarioAtividadePorAtividadeDisciplinaLI(inventarioAtividadeModelo.CodAtividade, inventarioAtividadeModelo.CodDisciplina, localInstalacao.CodLocalInstalacao, null);

                                    if (unicoInventario != null && inventarioAtividadeModelo.Codigo != unicoInventario.Codigo)
                                        adicionaErroLista(resultado.resultadoImportacao.erros, 2,
                                            celula.Address.ColumnLetter + celula.Address.RowNumber, $"Já existe um inventário de atividade com os dados local de instalação {item} , atividade {atividadePad.Nome} , disciplina {disciplinaAtv.Nome}!");

                                    else
                                    {
                                        var inventarioAtividade = inventarioAtividadePersistencia.ListarInventarioDeAtividadePorCodigo(inventarioAtividadeModelo.Codigo,null);
                                        if (inventarioAtividade != null && inventarioAtividadeModelo.Codigo == inventarioAtividade.Codigo)
                                        {
                                            ehEdicao = true;
                                        }
                                        liInventarioAtividadeModelo = new LocalInstalacaoInventarioAtividadeModelo();
                                        liInventarioAtividadeModelo.CodLocalInstalacao = localInstalacao.CodLocalInstalacao;
                                        inventarioAtividadeModelo.LOCAL_INSTALACAO_INVENTARIO_ATIVIDADE.Add(liInventarioAtividadeModelo);
                                    }
                                }
                            }
                        }
                    }

                    else if (celula.Address.ColumnLetter == "H")
                    {
                        var peso = this.pesoPersistencia.ListarPesoPorNome(celula.Value.ToString());
                        if (peso is null) adicionaErroLista(resultado.resultadoImportacao.erros, 4, celula.Address.ColumnLetter + celula.Address.RowNumber, "Peso inválido!");
                        else inventarioAtividadeModelo.CodPeso = peso.CodPeso;
                    }
                    else if (celula.Address.ColumnLetter == "I")
                    {
                        var duracao = this.duracaoPersistencia.ListarDuracaoPorNome(celula.Value.ToString());
                        if (duracao is null) adicionaErroLista(resultado.resultadoImportacao.erros, 5, celula.Address.ColumnLetter + celula.Address.RowNumber, "Duração inválida!");
                        else inventarioAtividadeModelo.CodDuracao = duracao.CodDuracao;
                    }

                    else if (celula.Address.ColumnLetter == "J")
                    {
                        if (celula.Value.ToString().Length > 500) adicionaErroLista(resultado.resultadoImportacao.erros, 9, nomeCelulaAtual, "A descrição excedeu o limite de 500 caracteres!");
                        else inventarioAtividadeModelo.Descricao = celula.Value.ToString();
                    }

                    else if (celula.Address.ColumnLetter == "K")
                    {
                        if (celula.Value.ToString().Length > 500) adicionaErroLista(resultado.resultadoImportacao.erros, 9, nomeCelulaAtual, "A observação geral excedeu o limite de 500 caracteres!");
                        else inventarioAtividadeModelo.ObservacaoGeral = celula.Value.ToString();
                    }

                    else
                    {
                        string nomeColuna = planilha.Cell(celula.Address.ColumnLetter + "3").Value.ToString().ToUpper();
                        string nomeCelula = celula.Address.ColumnLetter + celula.Address.RowNumber;

                        //MAPEAMENTO DE RISCO
                        if (nomeColuna.Contains("RISCO_"))
                        {
                            riscoModelo = new RiscoInventarioAtividadeModelo();
                            riscoModelo.EPIRiscoInventarioAtividadeModelo = new List<EPIRiscoInventarioAtividadeModelo>();
                            var risco = this.riscoPersistencia.ListarRiscoPorNomeETipo(celula.Value.ToString(), (long)Constantes.TipoRisco.INV_ATV);
                            if (risco is null) adicionaErroLista(resultado.resultadoImportacao.erros, 3, nomeCelula, "Risco ou tipo risco inválido!");
                            else
                                riscoModelo.CodRisco = risco.CodRisco;

                            if (resultado.riscosImportados.Contains(riscoModelo.CodRisco))
                                adicionaErroLista(resultado.resultadoImportacao.erros, 3, nomeCelula, $"O risco {celula.Value.ToString()} ja existe no inventário {nomeCelula}");

                            resultado.riscosImportados.Add(riscoModelo.CodRisco);
                        }
                        else if (nomeColuna.Contains("SEVERIDADE"))
                        {
                            var severidade = this.severidadePersistencia.ListarSeveridadePorNome(celula.Value.ToString());
                            if (severidade is null) adicionaErroLista(resultado.resultadoImportacao.erros, 4, nomeCelula, "Severidade inválida!");
                            else riscoModelo.CodSeveridade = severidade.CodSeveridade;
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
                                else
                                {
                                    riscoModelo.ProcedimentoAplicavel = procAplicaveis;
                                }
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
                                        long codRisco = riscoModelo.CodRisco;
                                        if (codRisco != 0)
                                        {
                                            epiRiscoModelo = new EPIRiscoInventarioAtividadeModelo();
                                            epiRiscoModelo.CodEPI = epi.CodEPI;
                                            epiRiscoModelo.CodRiscoInventarioAtividade = riscoModelo.CodRiscoInventarioAtividade;
                                            riscoModelo.EPIRiscoInventarioAtividadeModelo.Add(epiRiscoModelo);
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
                                if (cm.Length > 500) adicionaErroLista(resultado.resultadoImportacao.erros, 7, nomeCelula, "Contramedida excedeu o limite de 500 caracteres!");
                                else riscoModelo.ContraMedidas = cm;
                            }
                        }

                        listaRiscoModelo.Add(riscoModelo);
                        resultado.listaRiscosImportados.Add(riscoModelo);
                    }

                }

                listaRiscoModelo = listaRiscoModelo.Distinct().ToList();
                ValidarPreenchimentoCampoObrigatorios(resultado.resultadoImportacao.erros, listaRiscoModelo, linha.RowNumber());
                ValidarPerfilCatalogoLocalInstalacao(resultado.resultadoImportacao.erros,inventarioAtividadeModelo,linha.RowNumber());

                if (resultado.resultadoImportacao.erros.Count == 0)
                {
                    //Adiciona o risco
                    inventarioAtividadeModelo.RISCO_INVENTARIO_ATIVIDADE.AddRange(listaRiscoModelo);

                    if (riscoModelo.CodRisco == 0)
                        throw new Exception("O preenchimento de riscos é obrigatório!");

                    //Calcula o risco Geral
                    valorRisco = CalcularRiscoTotalLista((int)riscoModelo.CodSeveridade, (int)riscoModelo.CodRisco, (int)inventarioAtividadeModelo.CodAtividade,
                        inventarioAtividadeModelo.CodDisciplina, (int)inventarioAtividadeModelo.CodDuracao, (int)inventarioAtividadeModelo.CodPeso);

                    if (valorRisco > valorRiscoAtual)
                        valorRiscoAtual = valorRisco;

                    //Adiciona o risco geral
                    inventarioAtividadeModelo.RiscoGeral = valorRiscoAtual;

                    //Adiciona o local de instalação
                    //inventarioAtividadeModelo.LOCAL_INSTALACAO_INVENTARIO_ATIVIDADE.Add(liInventarioAtividadeModelo);
                    if (ehEdicao == true)
                    {
                        resultado.listaInvAEditar.Add(inventarioAtividadeModelo);
                        ehEdicao = false;
                    }
                    else
                    {
                        Validacao itemAValidar = new Validacao();
                        itemAValidar.codAtividade = inventarioAtividadeModelo.CodAtividade;
                        itemAValidar.codDisciplina = inventarioAtividadeModelo.CodDisciplina;
                        itemAValidar.listaLocais.AddRange(inventarioAtividadeModelo.LOCAL_INSTALACAO_INVENTARIO_ATIVIDADE);
                        if (dadosValidados.Count > 0)
                        {
                            var inventariosIguais = dadosValidados.Where(x => x.codAtividade == itemAValidar.codAtividade && x.codDisciplina 
                            == itemAValidar.codDisciplina).ToList();

                            if(inventariosIguais.Count > 0)
                            {
                                bool existeErroLocais = false;

                                foreach (var inventario in inventariosIguais)
                                {
                                    foreach (var localInstalacao in inventario.listaLocais)
                                    {
                                        if (itemAValidar.listaLocais.Where(x => x.CodLocalInstalacao == localInstalacao.CodLocalInstalacao).FirstOrDefault() != null)
                                        {
                                            var nomeDaAtividade = atividadePersistencia.ListarAtividadePorId(itemAValidar.codAtividade);
                                            var nomeDaDisciplina = disciplinaPersistencia.ListarDisciplinaPorId(itemAValidar.codDisciplina);

                                            adicionaErroLista(resultado.resultadoImportacao.erros, 7, "G", $"Existem mais de um inventário de atividade na planilha para a atividade" +
                                                $" {nomeDaAtividade.Nome}, disciplina {nomeDaDisciplina.Nome} e seus respectivos locais!");
                                            existeErroLocais = true;
                                            break;
                                        }
                                    }

                                    if (existeErroLocais == false)
                                    {
                                        if(!dadosValidados.Contains(itemAValidar))
                                            dadosValidados.Add(itemAValidar);
                                    }
                                    else
                                        break;
                                }

                                //foreach (var itemResult in inventariosIguais)
                                //{
                                //    foreach (var itemAValidarLocais in itemResult.listaLocais)
                                //    {
                                //        if (itemAValidar.listaLocais.Contains(itemAValidarLocais))
                                //        {
                                //            adicionaErroLista(resultado.resultadoImportacao.erros, 7, "G", $"Já existe inventário de atividade na planilha para a atividade" +
                                //                $"{itemAValidar.codAtividade}, disciplina {itemAValidar.codDisciplina} e locais {itemAValidar.listaLocais}!");
                                //            existeErroLocais = true;
                                //            break;
                                //        }
                                //    }

                                //    if (existeErroLocais == false)
                                //    {
                                //        if(!dadosValidados.Contains(itemAValidar))
                                //        {
                                //            dadosValidados.Add(itemAValidar);
                                //        }
                                //    }
                                //    else
                                //    {
                                //        break;
                                //    }
                                //}

                            }
                            else
                            {
                                dadosValidados.Add(itemAValidar);
                            }
                        }
                        else
                        {
                            dadosValidados.Add(itemAValidar);
                        }
                        resultado.listaInvAImportar.Add(inventarioAtividadeModelo);
                    }
                }

            }

            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            elapsedMs = (elapsedMs / 1000) / 60;

            resultado.tempoProcessamento = elapsedMs;

            return resultado;
        }

        private void ValidarPerfilCatalogoLocalInstalacao(List<ErroImportacao> listaErros, InventarioAtividadeModelo inventarioAtividadeModelo,int linha)
        {
            foreach (var localInstalacao in inventarioAtividadeModelo.LOCAL_INSTALACAO_INVENTARIO_ATIVIDADE)
            {
                var li = localInstalacaoPersistencia.ListarLocalInstalacaoPorId(localInstalacao.CodLocalInstalacao);
                if (li.CodPerfilCatalogo != inventarioAtividadeModelo.CodPerfilCatalogo)
                {
                  adicionaErroLista(listaErros, 11, $"E{linha}", $"O local de instalação {li.Nome}, possui um perfil de catálogo diferente do que se está tentando associar");
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

        public int CalcularRiscoTotalLista(int codSeveridade, int codRisco, int codAtividade, long codDisciplina, int codDuracao, int codPeso)
        {

            int maiorRisco = 0;

            decimal valorSeveridade;
            int valorArt, valorDA, valorPA;

            var severidade = severidadePersistencia.ListarSeveridadePorId(codSeveridade);
            var risco = riscoPersistencia.ListarRiscoPorId(codRisco);
            var duracao = duracaoPersistencia.ListarDuracaoPorId(codDuracao);
            var peso = pesoPersistencia.ListarPesoPorId(codPeso);
            var disciplina = disciplinaPersistencia.ListarDisciplinaPorId(codDisciplina);
            var atividade = atividadePersistencia.ListarAtividadePorId(codAtividade);
            valorSeveridade = severidade.Indice;

            int codRiscoAtividade = IdentificaRisco((Int32)risco.CodRisco);

            valorArt = CalcularART((int)atividade.CodAtividadePadrao, codRiscoAtividade, (int)duracao.CodDuracao);
            valorDA = CalcularDA((int)disciplina.CodDisciplina, (int)atividade.CodAtividadePadrao);
            valorPA = CalcularPA((int)peso.Indice,(int)atividade.CodAtividadePadrao);

            int riscoAtual = CalcularRiscoAtividade(valorSeveridade, valorArt, valorDA, valorPA);

            if (riscoAtual > maiorRisco)
                maiorRisco = riscoAtual;

            return maiorRisco;
        }

        public int CalcularRiscoTotalTela(RiscoTotalAtividadeModelo riscoTotalAtividadeModelo)
        {
            decimal valorSeveridade;
            int valorArt, valorDA, valorPA;

            var severidade = severidadePersistencia.ListarSeveridadePorId(riscoTotalAtividadeModelo.CodSeveridade);
            var risco = riscoPersistencia.ListarRiscoPorId(riscoTotalAtividadeModelo.CodRisco);
            var duracao = duracaoPersistencia.ListarDuracaoPorId(riscoTotalAtividadeModelo.CodDuracao);
            var peso = pesoPersistencia.ListarPesoPorId(riscoTotalAtividadeModelo.CodPeso);
            var disciplina = disciplinaPersistencia.ListarDisciplinaPorId(riscoTotalAtividadeModelo.CodDisciplina);
            var atividade = atividadePersistencia.ListarAtividadePorId(riscoTotalAtividadeModelo.CodAtividade);
            valorSeveridade = severidade.Indice;

            int codRiscoAtividade = IdentificaRisco((Int32)risco.CodRisco);

            valorArt = CalcularART((int)atividade.CodAtividadePadrao, codRiscoAtividade, (int)duracao.CodDuracao);
            valorDA = CalcularDA((int)disciplina.CodDisciplina, (int)atividade.CodAtividadePadrao);
            valorPA = CalcularPA((int)peso.Indice, (int)atividade.CodAtividadePadrao);

            int valorRiscoTotal = CalcularRiscoAtividade(valorSeveridade, valorArt, valorDA, valorPA);

            return valorRiscoTotal;
        }

        public int CalcularDA(int disciplina, int atividade)
        {
            int[,] matriz = new int[5, 5];

            if (disciplina != 0 && atividade != 0)
            {
                matriz = new int[5, 5] { { 0, 0, 0, 0, 0 }, { 0, 1, 2, 3, 2 }, { 0, 2, 1, 3, 3 }, { 0, 1, 1, 2, 1 }, { 0, 1, 1, 2, 1 } };

                return matriz[(int)disciplina, (int)atividade];
            }
            else
                return 0;
        }

        public int CalcularART(int atividade, int risco, int tempo)
        {
            int[,] matriz = new int[9, 9];

            if (risco != 0 && tempo != 0)
            {
                switch (atividade)
                {
                    case 1:
                        {
                            matriz = new int[9, 9] { { 0,0,0,0,0,0,0,0,0 }, { 0,1,1,1,1,1,1,1,1 }, { 0,2,2,3,3,3,4,4,5 }, { 0,1,1,1,1,1,1,1,1 }, { 0,1,1,1,1,1,1,1,1 }
                        , { 0,1,1,1,1,1,1,1,1 }, { 0,1,1,1,1,1,1,1,1 }, { 0,1,1,1,1,1,1,1,1 }, { 0,1,1,1,1,1,1,1,2 } };

                            return (int)matriz[(int)risco, (int)tempo];
                        }

                    case 2:
                        {
                            matriz = new int[9, 9] { { 0,0,0,0,0,0,0,0,0 }, { 0,2,2,2,3,3,3,3,3 }, { 0,2,2,3,3,4,4,5,5 }, { 0,1,1,1,2,2,2,3,3 }, { 0,1,1,2,2,2,2,2,2 }
                        , { 0,1,1,1,2,2,2,2,2 }, { 0,1,1,2,2,3,3,3,3 }, { 0,1,1,1,1,1,1,1,1 }, { 0,1,1,2,2,2,2,3,3 } };

                            return matriz[(int)risco, (int)tempo];
                        }

                    case 3:
                        {
                            matriz = new int[9, 9] { { 0,0,0,0,0,0,0,0,0 }, { 0,3,3,3,4,4,5,5,5 }, { 0,2,2,3,3,3,3,3,3 }, { 0,1,1,2,2,3,3,4,5 }, { 0,1,2,1,1,1,1,1,1 }
                        , { 0,1,1,2,2,2,2,2,2 }, { 0,1,3,1,1,1,1,1,1 }, { 0,1,1,1,1,1,1,1,1 }, { 0,1,2,2,2,2,2,2,3 } };

                            return matriz[(int)risco, (int)tempo];
                        }

                    case 4:
                        {
                            matriz = new int[9, 9] { { 0,0,0,0,0,0,0,0,0 }, { 0,1,2,3,3,3,3,3,2 }, { 0,2,2,2,2,2,2,2,2 }, { 0,1,2,2,2,2,2,2,1 }, { 0,1,2,1,1,1,1,1,1 }
                        , { 0,1,1,2,2,2,2,2,1 }, { 0,1,3,1,1,1,1,1,1 }, { 0,2,2,2,2,2,2,2,2 }, { 0,1,2,2,2,2,2,2,1 } };

                            return matriz[(int)risco, (int)tempo];
                        }

                    default:
                        break;
                }

                return matriz[(int)risco, (int)tempo];
            }
            else
                return 0;
        }

        public int CalcularPA(int peso, int atividade)
        {
            int[,] matriz = new int[6, 5];

            if (peso != 0 && atividade != 0)
            {
                matriz = new int[6, 5] { { 0, 0, 0, 0, 0}, { 0, 1, 1, 1, 1}, { 0, 1, 2, 2, 1}, { 0, 1, 2, 2, 2}, { 0, 1, 2, 3, 3}, { 0, 1, 2, 4, 3} };

                return matriz[(int)peso, (int)atividade];
            }
            else
                return 0;
        }

        public int IdentificaRisco(int risco)
        {
            try
            {
                if (risco == (Int32)Constantes.RiscoAtividade.PRENSAMENTOS)
                    return 1;

                else if (risco == (Int32)Constantes.RiscoAtividade.QUEDAS)
                    return 2;

                else if (risco == (Int32)Constantes.RiscoAtividade.CORTES)
                    return 3;

                else if (risco == (Int32)Constantes.RiscoAtividade.AMPUTACOES)
                    return 4;

                else if (risco == (Int32)Constantes.RiscoAtividade.PERFURACOES)
                    return 5;

                else if (risco == (Int32)Constantes.RiscoAtividade.QUEIMADURAS)
                    return 6;

                else if (risco == (Int32)Constantes.RiscoAtividade.CHOQUE_ELETRICO)
                    return 7;

                else if (risco == (Int32)Constantes.RiscoAtividade.OUTROS)
                    return 8;

                else return 0;
            }
            catch (Exception)
            {
                throw new Exception("Gentileza informar um risco de atividade válido!");
            }
        }

        public class ArquivoLog
        {
            public string resultadoString { get; set; }
        }

        public List<LOG_INVENTARIO_ATIVIDADE> ListarLogInventario(List<long> codInventarioAmbiente)
        {
            List<INVENTARIO_ATIVIDADE> listaInventarios = new List<INVENTARIO_ATIVIDADE>();
            List<LOG_INVENTARIO_ATIVIDADE> logInventarios = new List<LOG_INVENTARIO_ATIVIDADE>();
            List<LOG_INVENTARIO_ATIVIDADE> listaLogInventarios = new List<LOG_INVENTARIO_ATIVIDADE>();

            try
            {
                foreach (var item in codInventarioAmbiente)
                {
                    var inventario = inventarioAtividadePersistencia.ListarInventarioAtividadeAtivadoEDesativadoPorId(item);

                    listaInventarios.Add(inventario);
                }

                foreach (var itemInventario in listaInventarios)
                {
                    logInventarios = this.logInventarioAtividadePersistencia.ListarLogInventario(itemInventario.CodInventarioAtividade);

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

        public ArquivoLog EscreverLogTodosInventarios()
        {
            try
            {
                List<long> codInventarios = new List<long>();

                var todosInventarios = inventarioAtividadePersistencia.ListarTodosInventarios();

                if (todosInventarios.Count <= 0)
                    throw new Exception("Não existem inventários cadastrados na base de dados.");

                foreach (var itemInventario in todosInventarios)
                {
                    codInventarios.Add(itemInventario.CodInventarioAtividade);
                }

                var resultado = EscreverLogEmTxt(codInventarios);

                return resultado;
            }

            catch (Exception)
            {
                throw;
            }
        }

        public ArquivoLog EscreverLogEmTxt(List<long> codInventariosAtividade)
        {
            try
            {
                if (codInventariosAtividade.Count <= 0)
                    throw new Exception($"Gentileza informar no mínimo um código de inventário para a geração dos logs.");

                string caminhoLogInventario = ConfigurationManager.AppSettings["caminhoLogInventario"];
                string nomeArquivo = "log_" + "_" + DateTime.Now.ToString() + ".txt";
                nomeArquivo = nomeArquivo.Replace("/", "_").Replace(" ", "_").Replace(":", "_");
                string caminhoCompleto = caminhoLogInventario + nomeArquivo;

                string todosLogs = "";

                if (!File.Exists(caminhoLogInventario))
                    File.Create(caminhoCompleto).Close();

                var logInventarioAtividade = ListarLogInventario(codInventariosAtividade);

                if (logInventarioAtividade.Count <= 0)
                    throw new Exception($"Não foram encontrados logs referentes ao inventário informado.");

                var listaDeLogsAgrupadosPorInventario = logInventarioAtividade.GroupBy(x => x.CodInventarioAtividade).Select(u => u.ToList()).ToList();

                int i = 0;
                foreach (var itemLista1 in listaDeLogsAgrupadosPorInventario)
                {
                    foreach (var itemLista2 in listaDeLogsAgrupadosPorInventario[i])
                    {
                        var inventario = inventarioAtividadePersistencia.ListarInventarioAtividadePorId(itemLista2.CodInventarioAtividade,false);

                        var entities = new DB_APRPTEntities();
                            var riscosLog = entities.LOG_RISCO_INVENTARIO_ATIVIDADE.Where(x => x.CodLogInventarioAtividade == itemLista2.CodLogInventarioAtividade).ToList();

                        entities.Dispose();

                        var listaRiscosDoInventario = inventario.RISCO_INVENTARIO_ATIVIDADE.Where(x => x.CodInventarioAtividade == inventario.CodInventarioAtividade).ToList();

                        todosLogs = geraLogInventario(itemLista2, inventario, todosLogs, riscosLog);

                        if (listaRiscosDoInventario.Count <= 0)
                            throw new Exception($"Não foram encontrados riscos para o inventário de código {inventario.Codigo}");

                        if (itemLista2.CodLogTipoOperacao == 2)
                        {
                            foreach (var itemRisco in listaRiscosDoInventario)
                            {
                                var epi = itemRisco.EPI_RISCO_INVENTARIO_ATIVIDADE.Where(y => y.CodRiscoInventarioAtividade == itemRisco.CodRiscoInventarioAtividade).ToList();

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


        private string geraLogInventario(LOG_INVENTARIO_ATIVIDADE logs, INVENTARIO_ATIVIDADE inventario, string conteudo, List<LOG_RISCO_INVENTARIO_ATIVIDADE> riscos)
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
                    #region LIs
                    string liAntigo = "", liNovo = "";
                    var listaLIsAntigos = logs.CodLIsAntigos.Split(',');
                    var listaLIsNovos = logs.CodLIsNovos.Split(',');
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

                            Descrição do inventário antigo: {logs.DescricaoAntiga}
                            Descrição do novo inventário: {logs.DescricaoNova}

                            Observações antigas: {logs.ObsGeralAntiga}
                            Novas observações: {logs.ObsGeralNova}

                            Locais de instalação antigos: {liAntigo}
                            Novos locais de instalação: {liNovo}

                            Risco geral antigo: {logs.RiscoGeralAntigo}
                            Novo risco geral: {logs.RiscoGeralNovo}";

                    foreach (var itemRisco in riscos)
                    {
                        var nomeRisco = riscoPersistencia.ListarRiscoPorId(itemRisco.CodRiscoAtividade);
                        var nomeSeveridade = severidadePersistencia.ListarSeveridadePorId((long)itemRisco.CodSeveridade);

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

                        if(string.IsNullOrEmpty(epi))
                        {
                            conteudo += $@" {Environment.NewLine}  
                            Dados do risco antigo:         
                            Risco: {nomeRisco.Nome}
                            Severidade: {nomeSeveridade.Nome}
                            Fonte geradora: {itemRisco.FonteGeradora}
                            Procedimentos aplicáveis: {itemRisco.ProcedimentoAplicavel}
                            Contramedidas: {itemRisco.ContraMedidas}";
                        }
                        else
                        {
                            conteudo += $@" {Environment.NewLine}  
                            Dados do risco antigo:         
                            Risco: {nomeRisco.Nome}
                            Severidade: {nomeSeveridade.Nome}
                            Fonte geradora: {itemRisco.FonteGeradora}
                            Procedimentos aplicáveis: {itemRisco.ProcedimentoAplicavel}
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
            catch (Exception e)
            {
                throw;
            }
        }



        private string geraLogTodosRiscosInventario(RISCO_INVENTARIO_ATIVIDADE itemRisco, List<EPI_RISCO_INVENTARIO_ATIVIDADE> listaEPI, string conteudo, LOG_INVENTARIO_ATIVIDADE logs)
        {
            try
            {
                var risco = riscoPersistencia.ListarRiscoPorId(itemRisco.CodRisco);
                var severidade = severidadePersistencia.ListarSeveridadePorId((long)itemRisco.CodSeveridade);

                if (string.IsNullOrEmpty(itemRisco.ProcedimentoAplicavel) && string.IsNullOrEmpty(itemRisco.ContraMedidas))
                {
                    conteudo += $@" {Environment.NewLine} 
                            Dados do risco atual:       
                            Risco: {risco.Nome}
                            Severidade: {severidade.Nome}
                            Fonte geradora: {itemRisco.FonteGeradora}";
                }
                else if (string.IsNullOrEmpty(itemRisco.ProcedimentoAplicavel) && !string.IsNullOrEmpty(itemRisco.ContraMedidas))
                {
                    conteudo += $@" {Environment.NewLine} 
                            Dados do risco atual:       
                            Risco: {risco.Nome}
                            Severidade: {severidade.Nome}
                            Fonte geradora: {itemRisco.FonteGeradora}
                            Contramedidas: {itemRisco.ContraMedidas}";
                }
                else if (!string.IsNullOrEmpty(itemRisco.ProcedimentoAplicavel) && string.IsNullOrEmpty(itemRisco.ContraMedidas))
                {
                    conteudo += $@" {Environment.NewLine} 
                            Dados do risco atual:       
                            Risco: {risco.Nome}
                            Severidade: {severidade.Nome}
                            Fonte geradora: {itemRisco.FonteGeradora}
                            Procedimentos aplicáveis: {itemRisco.ProcedimentoAplicavel}";
                }

                else if (!string.IsNullOrEmpty(itemRisco.ProcedimentoAplicavel) && !string.IsNullOrEmpty(itemRisco.ContraMedidas))
                {
                    conteudo += $@" {Environment.NewLine} 
                            Dados do risco atual:       
                            Risco: {risco.Nome}
                            Severidade: {severidade.Nome}
                            Fonte geradora: {itemRisco.FonteGeradora}
                            Contramedidas: {itemRisco.ContraMedidas}
                            Procedimentos aplicáveis: { itemRisco.ProcedimentoAplicavel} ";
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

                return conteudo;
            }
            catch (Exception e)
            {
                throw;
            }
        }





        private string geraLogRiscosInventario(RISCO_INVENTARIO_ATIVIDADE itemRisco, List<EPI_RISCO_INVENTARIO_ATIVIDADE> listaEPI, string conteudo)
        {
            try
            {
                var risco = riscoPersistencia.ListarRiscoPorId(itemRisco.CodRisco);
                var severidade = severidadePersistencia.ListarSeveridadePorId((long)itemRisco.CodSeveridade);

                if (string.IsNullOrEmpty(itemRisco.ProcedimentoAplicavel) && string.IsNullOrEmpty(itemRisco.ContraMedidas))
                {
                    conteudo += $@" {Environment.NewLine} 
                            Dados do risco atual:       
                            Risco: {risco.Nome}
                            Severidade: {severidade.Nome}
                            Fonte geradora: {itemRisco.FonteGeradora}";
                }
                else if (string.IsNullOrEmpty(itemRisco.ProcedimentoAplicavel) && !string.IsNullOrEmpty(itemRisco.ContraMedidas))
                {
                    conteudo += $@" {Environment.NewLine} 
                            Dados do risco atual:       
                            Risco: {risco.Nome}
                            Severidade: {severidade.Nome}
                            Fonte geradora: {itemRisco.FonteGeradora}
                            Contramedidas: {itemRisco.ContraMedidas}";
                }
                else if (!string.IsNullOrEmpty(itemRisco.ProcedimentoAplicavel) && string.IsNullOrEmpty(itemRisco.ContraMedidas))
                {
                    conteudo += $@" {Environment.NewLine} 
                            Dados do risco atual:       
                            Risco: {risco.Nome}
                            Severidade: {severidade.Nome}
                            Fonte geradora: {itemRisco.FonteGeradora}
                            Procedimentos aplicáveis: {itemRisco.ProcedimentoAplicavel}";
                }

                else if (!string.IsNullOrEmpty(itemRisco.ProcedimentoAplicavel) && !string.IsNullOrEmpty(itemRisco.ContraMedidas))
                {
                    conteudo += $@" {Environment.NewLine} 
                            Dados do risco atual:       
                            Risco: {risco.Nome}
                            Severidade: {severidade.Nome}
                            Fonte geradora: {itemRisco.FonteGeradora}
                            Contramedidas: {itemRisco.ContraMedidas}
                            Procedimentos aplicáveis: { itemRisco.ProcedimentoAplicavel} ";
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
            catch (Exception e)
            {
                throw;
            }
        }

        private void ValidarPreenchimentoCampoObrigatorios(List<ErroImportacao> listaDeErros, List<RiscoInventarioAtividadeModelo> listaRiscoModelo, int linha)
        {
            foreach (var riscoModelo in listaRiscoModelo)
            {
                if (riscoModelo.EPIRiscoInventarioAtividadeModelo.Any() == false && string.IsNullOrEmpty(riscoModelo.ProcedimentoAplicavel) && string.IsNullOrEmpty(riscoModelo.ContraMedidas))
                {
                    adicionaErroLista(listaDeErros, 10, "", $"Os campos contramedida, epi e procedimento aplicável estão nulos na linha {linha}. Gentileza preencher no mínimo um desses campos!");
                }
            }
        }

    }
    #endregion
}
