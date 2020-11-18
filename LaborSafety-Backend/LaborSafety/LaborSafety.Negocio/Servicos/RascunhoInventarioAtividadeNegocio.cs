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

namespace LaborSafety.Negocio.Servicos
{
    public class RascunhoInventarioAtividadeNegocio : IRascunhoInventarioAtividadeNegocio
    {
        private readonly IRascunhoInventarioAtividadePersistencia rascunhoInventarioAtividadePersistencia;
        private readonly ILocalInstalacaoPersistencia localInstalacaoPersistencia;
        private readonly Validador<RascunhoInventarioAtividadeModelo> validadorRascunhoInventarioAtividade;
        private readonly IInventariosAtividadePersistencia inventarioAtividadePersistencia;
        private readonly IEPIPersistencia epiPersistencia;
        private readonly IAtividadePadraoPersistencia atividadePersistencia;
        private readonly IDisciplinaPersistencia disciplinaPersistencia;
        private readonly ILogInventarioAtividadePersistencia logInventarioAtividadePersistencia;

        public RascunhoInventarioAtividadeNegocio(IRascunhoInventarioAtividadePersistencia rascunhoInventarioAtividadePersistencia,
            ILocalInstalacaoPersistencia localInstalacaoPersistencia, Validador<RascunhoInventarioAtividadeModelo> validadorRascunhoInventarioAtividade,
            IInventariosAtividadePersistencia inventarioAtividadePersistencia, IEPIPersistencia epiPersistencia, IAtividadePadraoPersistencia atividadePersistencia,
            IDisciplinaPersistencia disciplinaPersistencia, ILogInventarioAtividadePersistencia logInventarioAtividadePersistencia)
        {
            this.rascunhoInventarioAtividadePersistencia = rascunhoInventarioAtividadePersistencia;
            this.localInstalacaoPersistencia = localInstalacaoPersistencia;
            this.validadorRascunhoInventarioAtividade = validadorRascunhoInventarioAtividade;
            this.inventarioAtividadePersistencia = inventarioAtividadePersistencia;
            this.epiPersistencia = epiPersistencia;
            this.atividadePersistencia = atividadePersistencia;
            this.disciplinaPersistencia = disciplinaPersistencia;
            this.logInventarioAtividadePersistencia = logInventarioAtividadePersistencia;
        }

        public RascunhoInventarioAtividadeModelo MapeamentoRascunhoInventarioAtividade(RASCUNHO_INVENTARIO_ATIVIDADE inventario)
        {
            RascunhoInventarioAtividadeModelo inventarioAtv = new RascunhoInventarioAtividadeModelo()
            {
                CodRascunhoInventarioAtividade = inventario.CodRascunhoInventarioAtividade,
                Codigo = inventario.Codigo,
                CodPeso = inventario.CodPeso,
                CodPerfilCatalogo = inventario.CodPerfilCatalogo,
                CodDuracao = inventario.CodDuracao,
                CodAtividade = inventario.CodAtividade,
                CodDisciplina = inventario.CodDisciplina,
                Descricao = inventario.Descricao,
                RiscoGeral = inventario.RiscoGeral,
                ObservacaoGeral = inventario.ObservacaoGeral
            };

            inventarioAtv.RISCO_RASCUNHO_INVENTARIO_ATIVIDADE = new List<RiscoRascunhoInventarioAtividadeModelo>();
            inventarioAtv.LOCAL_INSTALACAO_RASCUNHO_INVENTARIO_ATIVIDADE = new List<LocalInstalacaoRascunhoInventarioAtividadeModelo>();

            Mapper.Map(inventario.RISCO_RASCUNHO_INVENTARIO_ATIVIDADE, inventarioAtv.RISCO_RASCUNHO_INVENTARIO_ATIVIDADE);

            for (int i = 0; i < inventario.LOCAL_INSTALACAO_RASCUNHO_INVENTARIO_ATIVIDADE.Count; i++)
            {
                var liInventario = inventario.LOCAL_INSTALACAO_RASCUNHO_INVENTARIO_ATIVIDADE.ElementAt(i);
                var localInstalacao = localInstalacaoPersistencia.ListarLocalInstalacaoPorId(liInventario.CodLocalInstalacao);
                liInventario.LOCAL_INSTALACAO = localInstalacao;

                LocalInstalacaoRascunhoInventarioAtividadeModelo modeloLIInv = new LocalInstalacaoRascunhoInventarioAtividadeModelo();
                modeloLIInv.CodRascunhoInventarioAtividade = liInventario.CodRascunhoInventarioAtividade;
                modeloLIInv.CodLocalInstalacao = liInventario.CodLocalInstalacao;
                modeloLIInv.CodLocalInstalacaoRascunhoInventarioAtividade = liInventario.CodLocalInstalacaoRascunhoInventarioAtividade;

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

                var local = localInstalacaoPersistencia.ListarLocalInstalacaoPorId(modeloLI.CodLocalInstalacao);

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

                if (n2.Contains("000_BASE") || n3.Contains("000_BASE") || n4.Contains("000_BASE") || n5.Contains("000_BASE") || n6.Contains("000_BASE"))
                {
                    var nomeSeparado = local.Nome.Split(' ');
                    var nomeCompleto = local.Nome.Replace(nomeSeparado[0], nomeSeparado[0] + "-000_BASE");
                    modeloLI.Nome = nomeCompleto;
                }

                modeloLIInv.LocalInstalacao = modeloLI;

                inventarioAtv.LOCAL_INSTALACAO_RASCUNHO_INVENTARIO_ATIVIDADE.Add(modeloLIInv);
            }

            List<RiscoRascunhoInventarioAtividadeModelo> listaRisco = new List<RiscoRascunhoInventarioAtividadeModelo>();
            foreach (var itemRisco in inventario.RISCO_RASCUNHO_INVENTARIO_ATIVIDADE)
            {
                RiscoRascunhoInventarioAtividadeModelo risco = new RiscoRascunhoInventarioAtividadeModelo();
                risco.Ativo = true;
                risco.CodRascunhoInventarioAtividade = itemRisco.CodRascunhoInventarioAtividade;
                risco.CodRiscoRascunhoInventarioAtividade = itemRisco.CodRiscoRascunhoInventarioAtividade;
                risco.CodRisco = itemRisco.CodRisco;
                risco.CodSeveridade = itemRisco.CodSeveridade;
                risco.FonteGeradora = itemRisco.FonteGeradora;
                risco.ProcedimentoAplicavel = itemRisco.ProcedimentoAplicavel;
                risco.ContraMedidas = itemRisco.ContraMedidas;

                risco.EPIRiscoRascunhoInventarioAtividadeModelo = new List<EPIRiscoRascunhoInventarioAtividadeModelo>();

                var listaEPI = itemRisco.EPI_RISCO_RASCUNHO_INVENTARIO_ATIVIDADE
                    .Where(a => a.CodRiscoRascunhoInventarioAtividade == itemRisco.CodRiscoRascunhoInventarioAtividade).ToList();

                Mapper.Map(listaEPI, risco.EPIRiscoRascunhoInventarioAtividadeModelo);

                listaRisco.Add(risco);
            }

            inventarioAtv.RISCO_RASCUNHO_INVENTARIO_ATIVIDADE = listaRisco;

            return inventarioAtv;
        }

        public RascunhoInventarioAtividadeModelo ListarInventarioAtividadePorId(long id)
        {
            RASCUNHO_INVENTARIO_ATIVIDADE inv = this.rascunhoInventarioAtividadePersistencia.ListarRascunhoInventarioAtividadePorId(id);

            if (inv == null)
                throw new KeyNotFoundException("Rascunho de inventário de atividade não encontrado.");

            return MapeamentoRascunhoInventarioAtividade(inv);
        }

        public List<RascunhoInventarioAtividadeModelo> ListarInventarioAtividadePorLI(List<string> nomesLi)
        {
            List<long> listaCodLi = new List<long>();
            List<RascunhoInventarioAtividadeModelo> inventarioAtividadeModelo = new List<RascunhoInventarioAtividadeModelo>();

            foreach (var itemNomeLi in nomesLi)
            {
                var localInstalacao = localInstalacaoPersistencia.ListarLocalInstalacaoPorNome(itemNomeLi);
                if (localInstalacao == null)
                    throw new Exception($"Não foi encontrado local de instalação de nome {localInstalacao.Nome}.");

                listaCodLi.Add(localInstalacao.CodLocalInstalacao);
            }

            var inv = this.rascunhoInventarioAtividadePersistencia.ListarRascunhoInventarioAtividadePorLI(listaCodLi);
            if (inv == null)
                throw new KeyNotFoundException("Local de instalação do rascunho inventário de atividade não encontrado.");

            foreach (RASCUNHO_INVENTARIO_ATIVIDADE inventario in inv)
                inventarioAtividadeModelo.Add(MapeamentoRascunhoInventarioAtividade(inventario));

            return inventarioAtividadeModelo;
        }

        public List<RascunhoInventarioAtividadeModelo> ListarInventarioAtividade(FiltroInventarioAtividadeModelo filtroInventarioAtividadeModelo)
        {
            List<RascunhoInventarioAtividadeModelo> inventarioAtividadeModelo = new List<RascunhoInventarioAtividadeModelo>();

            //ValidarFiltro(filtroInventarioAtividadeModelo);

            IEnumerable<RASCUNHO_INVENTARIO_ATIVIDADE> inv = this.rascunhoInventarioAtividadePersistencia.ListarRascunhoInventarioAtividade(filtroInventarioAtividadeModelo);

            if (inv == null)
                throw new KeyNotFoundException("Rascunho de inventário de atividade não encontrado.");

            foreach (RASCUNHO_INVENTARIO_ATIVIDADE inventario in inv)
            {
                inventarioAtividadeModelo.Add(MapeamentoRascunhoInventarioAtividade(inventario));
            }

            return inventarioAtividadeModelo;
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

        public void InserirRascunhoInventarioAtividade(RascunhoInventarioAtividadeModelo rascunhoInventarioAtividadeModelo)
        {
            validadorRascunhoInventarioAtividade.ValidaInsercao(rascunhoInventarioAtividadeModelo);

            List<LOCAL_INSTALACAO> locaisInstalacao = new List<LOCAL_INSTALACAO>();

            using (var entities = new DB_APRPTEntities())
            {
                using (var transaction = entities.Database.BeginTransaction(System.Data.IsolationLevel.Serializable))
                {
                    try
                    {
                        //List<LOCAL_INSTALACAO> locais = localInstalacaoPersistencia.ListarTodosLIs(entities);

                        for (int i = 0; i < rascunhoInventarioAtividadeModelo.LOCAL_INSTALACAO_RASCUNHO_INVENTARIO_ATIVIDADE.Count; i++)
                        {
                            var codigoLocal = rascunhoInventarioAtividadeModelo.LOCAL_INSTALACAO_RASCUNHO_INVENTARIO_ATIVIDADE[i].LocalInstalacao.CodLocalInstalacao;

                            var localEnviado = localInstalacaoPersistencia.ListarLocalInstalacaoPorId(codigoLocal, entities);

                            //Filtra somente os locais do pai
                            List<LOCAL_INSTALACAO> locaisEFilhos = this.BuscaLocaisEFilhos(entities, localEnviado);

                            //List<LOCAL_INSTALACAO> locaisFilhos =
                                //rascunhoInventarioAtividadePersistencia.BuscaFilhosPorNivel(codigoLocal, entities);

                            locaisInstalacao.AddRange(locaisEFilhos);
                        }

                        this.rascunhoInventarioAtividadePersistencia.Inserir(rascunhoInventarioAtividadeModelo, entities, locaisInstalacao);

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

        public void EditarInventarioAtividade(RascunhoInventarioAtividadeModelo rascunhoInventarioAtividadeModelo)
        {
            validadorRascunhoInventarioAtividade.ValidaEdicao(rascunhoInventarioAtividadeModelo);

            List<LOCAL_INSTALACAO> locaisInstalacao = new List<LOCAL_INSTALACAO>();

            using (var entities = new DB_APRPTEntities())
            {
                using (var transaction = entities.Database.BeginTransaction(System.Data.IsolationLevel.Serializable))
                {
                    try
                    {
                        //List<LOCAL_INSTALACAO> locais = localInstalacaoPersistencia.ListarTodosLIs(entities);

                        for (int i = 0; i < rascunhoInventarioAtividadeModelo.LOCAL_INSTALACAO_RASCUNHO_INVENTARIO_ATIVIDADE.Count; i++)
                        {
                            var codigoLocal = rascunhoInventarioAtividadeModelo.LOCAL_INSTALACAO_RASCUNHO_INVENTARIO_ATIVIDADE[i].LocalInstalacao.CodLocalInstalacao;

                            var localEnviado = localInstalacaoPersistencia.ListarLocalInstalacaoPorId(codigoLocal, entities);

                            //Filtra somente os locais do pai
                            List<LOCAL_INSTALACAO> locaisEFilhos = this.BuscaLocaisEFilhos(entities, localEnviado);

                            //List<LOCAL_INSTALACAO> locaisFilhos =
                            //    rascunhoInventarioAtividadePersistencia.BuscaFilhosPorNivel(codigoLocal, entities);

                            locaisInstalacao.AddRange(locaisEFilhos);
                        }

                        this.rascunhoInventarioAtividadePersistencia.EditarRascunhoInventarioAtividade(rascunhoInventarioAtividadeModelo, entities, locaisInstalacao, transaction);

                        if (rascunhoInventarioAtividadeModelo.novoInventario)
                        {
                            // inserir inventario de verdade
                            InventarioAtividadeModelo inventarioAtividadeModelo = new InventarioAtividadeModelo();

                            inventarioAtividadeModelo.CodInventarioAtividade = rascunhoInventarioAtividadeModelo.CodRascunhoInventarioAtividade;
                            inventarioAtividadeModelo.Codigo = rascunhoInventarioAtividadeModelo.Codigo;
                            inventarioAtividadeModelo.CodPeso = (long)rascunhoInventarioAtividadeModelo.CodPeso;
                            inventarioAtividadeModelo.CodPerfilCatalogo = (long)rascunhoInventarioAtividadeModelo.CodPerfilCatalogo;
                            inventarioAtividadeModelo.CodDuracao = (long)rascunhoInventarioAtividadeModelo.CodDuracao;
                            inventarioAtividadeModelo.CodAtividade = (long)rascunhoInventarioAtividadeModelo.CodAtividade;
                            inventarioAtividadeModelo.CodDisciplina = (long)rascunhoInventarioAtividadeModelo.CodDisciplina;
                            inventarioAtividadeModelo.Descricao = rascunhoInventarioAtividadeModelo.Descricao;
                            inventarioAtividadeModelo.RiscoGeral = (int)rascunhoInventarioAtividadeModelo.RiscoGeral;
                            inventarioAtividadeModelo.ObservacaoGeral = rascunhoInventarioAtividadeModelo.ObservacaoGeral;
                            inventarioAtividadeModelo.DataAtualizacao = DateTime.Now;
                            inventarioAtividadeModelo.Ativo = true;

                            List<RiscoInventarioAtividadeModelo> listaRisco = new List<RiscoInventarioAtividadeModelo>();
                            foreach (var itemRiscoRascunho in rascunhoInventarioAtividadeModelo.RISCO_RASCUNHO_INVENTARIO_ATIVIDADE)
                            {
                                RiscoInventarioAtividadeModelo risco = new RiscoInventarioAtividadeModelo();

                                risco.CodInventarioAtividade = itemRiscoRascunho.CodRascunhoInventarioAtividade;
                                risco.Ativo = true;
                                risco.CodRiscoInventarioAtividade = itemRiscoRascunho.CodRiscoRascunhoInventarioAtividade;
                                risco.CodRisco = itemRiscoRascunho.CodRisco;
                                risco.CodSeveridade = itemRiscoRascunho.CodSeveridade;
                                risco.FonteGeradora = itemRiscoRascunho.FonteGeradora;
                                risco.ProcedimentoAplicavel = itemRiscoRascunho.ProcedimentoAplicavel;
                                risco.ContraMedidas = itemRiscoRascunho.ContraMedidas;

                                risco.EPIRiscoInventarioAtividadeModelo = new List<EPIRiscoInventarioAtividadeModelo>();

                                var listaEPI = itemRiscoRascunho.EPIRiscoRascunhoInventarioAtividadeModelo
                                    .Where(a => a.CodRiscoRascunhoInventarioAtividade == itemRiscoRascunho.CodRisco).ToList();

                                foreach (var itemListaEpi in listaEPI)
                                {
                                    EPIRiscoInventarioAtividadeModelo epi = new EPIRiscoInventarioAtividadeModelo();

                                    epi.CodEPI = itemListaEpi.CodEPI;
                                    epi.CodEpiRiscoInventarioAtividade = itemListaEpi.CodEpiRiscoRascunhoInventarioAtividade;
                                    epi.CodRiscoInventarioAtividade = itemListaEpi.CodRiscoRascunhoInventarioAtividade;
                                    risco.EPIRiscoInventarioAtividadeModelo.Add(epi);
                                }

                                listaRisco.Add(risco);
                            }
                            inventarioAtividadeModelo.RISCO_INVENTARIO_ATIVIDADE = listaRisco;

                            List<LOCAL_INSTALACAO> novosLocais = new List<LOCAL_INSTALACAO>();

                            foreach (var itemNovosLocais in rascunhoInventarioAtividadeModelo.LOCAL_INSTALACAO_RASCUNHO_INVENTARIO_ATIVIDADE)
                            {
                                var localInst = entities.LOCAL_INSTALACAO.Where(local => local.CodLocalInstalacao == itemNovosLocais.LocalInstalacao.CodLocalInstalacao).FirstOrDefault();
                                novosLocais.Add(localInst);
                            }

                            foreach (var itemLi in novosLocais)
                            {
                                var unicoInventario = inventarioAtividadePersistencia.ListarInventarioAtividadePorAtividadeDisciplinaLI(inventarioAtividadeModelo.CodAtividade, inventarioAtividadeModelo.CodDisciplina, itemLi.CodLocalInstalacao, entities);

                                if (unicoInventario != null)
                                {
                                    var atvPadrao = atividadePersistencia.ListarAtividadePorId(inventarioAtividadeModelo.CodAtividade);
                                    var disciplina = disciplinaPersistencia.ListarDisciplinaPorId(inventarioAtividadeModelo.CodDisciplina);
                                    throw new Exception($"Já existe um inventário de atividade com atividade padrão {atvPadrao.Nome}, disciplina {disciplina.Nome} e local de instalação {itemLi.Nome}");
                                }
                            }

                            var resultadoInsercao = inventarioAtividadePersistencia.Inserir(inventarioAtividadeModelo, entities, novosLocais);
                            inventarioAtividadeModelo.EightIDUsuarioModificador = rascunhoInventarioAtividadeModelo.EightIDUsuarioModificador;
                            logInventarioAtividadePersistencia.Inserir(inventarioAtividadeModelo, resultadoInsercao.CodInventarioAtividade, entities);
                        }

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

        public void ExcluirRascunhoInventarioAtividade(long id)
        {
            this.rascunhoInventarioAtividadePersistencia.ExcluirRascunhoInventarioAtividade(id, null);
        }
    }
}
