using System;
using System.Collections.Generic;
using System.Linq;
using LaborSafety.Dominio.Modelos;
using System.Data.Entity;
using LaborSafety.Persistencia.Interfaces;
using LaborSafety.Utils.Constantes;

namespace LaborSafety.Persistencia.Servicos
{
    public class RascunhoInventarioAtividadePersistencia : IRascunhoInventarioAtividadePersistencia
    {
        private readonly IDbLaborSafetyEntities databaseEntities;
        public RascunhoInventarioAtividadePersistencia(IDbLaborSafetyEntities databaseEntities)
        {
            this.databaseEntities = databaseEntities;
        }

        public List<LOCAL_INSTALACAO> BuscaFilhosPorNivel(long codigoLocal, DB_LaborSafetyEntities entities = null)
        {
            if (entities == null)
                entities = new DB_LaborSafetyEntities();

            entities.Database.CommandTimeout = 9999;

            var localEnviado = entities.LOCAL_INSTALACAO.Where(x => x.CodLocalInstalacao == codigoLocal).FirstOrDefault();

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

        private List<LOCAL_INSTALACAO> BuscaLocaisEFilhos(List<LOCAL_INSTALACAO> locais, LOCAL_INSTALACAO localEnviado)
        {
            List<LOCAL_INSTALACAO> locaisFiltrados = new List<LOCAL_INSTALACAO>();

            if (String.IsNullOrEmpty(localEnviado.N2))
                locaisFiltrados = locais.Where(x => x.N1 == localEnviado.N1).ToList();
            else if (String.IsNullOrEmpty(localEnviado.N3))
                locaisFiltrados = locais.Where(x => x.N1 == localEnviado.N1 && x.N2 == localEnviado.N2).ToList();
            else if (String.IsNullOrEmpty(localEnviado.N4))
                locaisFiltrados = locais.Where(x => x.N1 == localEnviado.N1 && x.N2 == localEnviado.N2 &&
                x.N3 == localEnviado.N3).ToList();
            else if (String.IsNullOrEmpty(localEnviado.N5))
                locaisFiltrados = locais.Where(x => x.N1 == localEnviado.N1 && x.N2 == localEnviado.N2 &&
                x.N3 == localEnviado.N3 && x.N4 == localEnviado.N4).ToList();
            else if (String.IsNullOrEmpty(localEnviado.N6))
                locaisFiltrados = locais.Where(x => x.N1 == localEnviado.N1 && x.N2 == localEnviado.N2
                && x.N3 == localEnviado.N3 && x.N4 == localEnviado.N4 && x.N5 == localEnviado.N5).ToList();
            else
                locaisFiltrados = locais.Where(x => x.N1 == localEnviado.N1 && x.N2 == localEnviado.N2
                && x.N3 == localEnviado.N3 && x.N4 == localEnviado.N4 && x.N5 == localEnviado.N5 && x.N6 == localEnviado.N6).ToList();

            return locaisFiltrados;
        }

        public void Inserir(RascunhoInventarioAtividadeModelo rascunhoInventarioAtividadeModelo, DB_LaborSafetyEntities entities, List<LOCAL_INSTALACAO> locaisInsercao = null)
        {
            List<RiscoRascunhoInventarioAtividadeModelo> riscos = rascunhoInventarioAtividadeModelo.RISCO_RASCUNHO_INVENTARIO_ATIVIDADE;

            if (entities == null)
                entities = new DB_LaborSafetyEntities();

            try
            {
                var inventario = new RASCUNHO_INVENTARIO_ATIVIDADE()
                {
                    Codigo = $"R_INV_ATV - {rascunhoInventarioAtividadeModelo.CodRascunhoInventarioAtividade} - {rascunhoInventarioAtividadeModelo.RiscoGeral}",
                    CodPeso = rascunhoInventarioAtividadeModelo.CodPeso,
                    CodPerfilCatalogo = rascunhoInventarioAtividadeModelo.CodPerfilCatalogo,
                    CodDuracao = rascunhoInventarioAtividadeModelo.CodDuracao,
                    CodAtividade = rascunhoInventarioAtividadeModelo.CodAtividade,
                    CodDisciplina = rascunhoInventarioAtividadeModelo.CodDisciplina,
                    Descricao = rascunhoInventarioAtividadeModelo.Descricao,
                    RiscoGeral = rascunhoInventarioAtividadeModelo.RiscoGeral,
                    ObservacaoGeral = rascunhoInventarioAtividadeModelo.ObservacaoGeral
                };

                entities.RASCUNHO_INVENTARIO_ATIVIDADE.Add(inventario);
                entities.SaveChanges();

                long idInv = inventario.CodRascunhoInventarioAtividade;

                if (locaisInsercao != null)
                {
                    foreach (var local in locaisInsercao)
                    {
                        entities.LOCAL_INSTALACAO_RASCUNHO_INVENTARIO_ATIVIDADE.Add(new LOCAL_INSTALACAO_RASCUNHO_INVENTARIO_ATIVIDADE()
                        {
                            CodRascunhoInventarioAtividade = idInv,
                            CodLocalInstalacao = local.CodLocalInstalacao
                        });
                    }
                    entities.SaveChanges();
                }

                if (riscos != null)
                {
                    foreach (var risco in riscos)
                    {
                        var novoRisco = new RISCO_RASCUNHO_INVENTARIO_ATIVIDADE()
                        {
                            CodRascunhoInventarioAtividade = idInv,
                            CodRisco = risco.CodRisco,
                            CodSeveridade = risco.CodSeveridade,
                            FonteGeradora = risco.FonteGeradora,
                            ProcedimentoAplicavel = risco.ProcedimentoAplicavel,
                            ContraMedidas = risco.ContraMedidas
                        };
                        entities.RISCO_RASCUNHO_INVENTARIO_ATIVIDADE.Add(novoRisco);
                        entities.SaveChanges();
                        foreach (var epi in risco.EPIRiscoRascunhoInventarioAtividadeModelo)
                        {
                            entities.EPI_RISCO_RASCUNHO_INVENTARIO_ATIVIDADE.Add(new EPI_RISCO_RASCUNHO_INVENTARIO_ATIVIDADE()
                            {
                                CodRiscoRascunhoInventarioAtividade = novoRisco.CodRiscoRascunhoInventarioAtividade,
                                CodEPI = epi.CodEPI
                            });
                        }
                    }
                    entities.SaveChanges();
                }

            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
        public void InserirRascunhoInventarioAtividade(RascunhoInventarioAtividadeModelo rascunhoInventarioAtividadeModelo, List<LOCAL_INSTALACAO> locais = null)
        {
            using (var entities = databaseEntities.GetDB_LaborSafetyEntities())
            {
                using (var transaction = entities.Database.BeginTransaction())
                {
                    try
                    {
                        this.Inserir(rascunhoInventarioAtividadeModelo, entities);
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public void EditarRascunhoInventarioAtividade(RascunhoInventarioAtividadeModelo rascunhoInventarioAtividadeModelo, DB_LaborSafetyEntities entities, List<LOCAL_INSTALACAO> locaisInstalacao, DbContextTransaction transaction)
        {
                RASCUNHO_INVENTARIO_ATIVIDADE rascunhoInventarioAtividadeExistente = entities.RASCUNHO_INVENTARIO_ATIVIDADE.Where(invAtv => invAtv.CodRascunhoInventarioAtividade == rascunhoInventarioAtividadeModelo.CodRascunhoInventarioAtividade).FirstOrDefault();

                if (rascunhoInventarioAtividadeExistente == null)
                {
                    throw new KeyNotFoundException();
                }

                else
                {
                        try
                        {
                            if(!rascunhoInventarioAtividadeModelo.novoInventario)
                            {
                                ExcluirRascunhoInventarioAtividade(rascunhoInventarioAtividadeExistente.CodRascunhoInventarioAtividade, entities);

                                Inserir(rascunhoInventarioAtividadeModelo, entities, locaisInstalacao);

                                //transaction.Commit();
                            }

                            else
                            {
                                ExcluirRascunhoInventarioAtividade(rascunhoInventarioAtividadeExistente.CodRascunhoInventarioAtividade, entities);
                            }
                        }
                        catch (Exception)
                        {
                            transaction.Rollback();
                            throw;
                        }
                }
        }

        public List<RASCUNHO_INVENTARIO_ATIVIDADE> ListarRascunhoInventarioAtividadePorLI(List<long> li)
        {
            using (var entities = new DB_LaborSafetyEntities())
            {
                List<RASCUNHO_INVENTARIO_ATIVIDADE> rascunhoInventarioAtividade = entities.RASCUNHO_INVENTARIO_ATIVIDADE
                    .Include(x => x.LOCAL_INSTALACAO_RASCUNHO_INVENTARIO_ATIVIDADE)
                    .Include(x => x.RISCO_RASCUNHO_INVENTARIO_ATIVIDADE)
                    .Where(x => x.LOCAL_INSTALACAO_RASCUNHO_INVENTARIO_ATIVIDADE.Any(y => li.Contains(y.CodLocalInstalacao))).ToList();

                return rascunhoInventarioAtividade;
            }
        }

        public RASCUNHO_INVENTARIO_ATIVIDADE ListarRascunhoInventarioAtividadePorId(long id)
        {
            using (var entities = new DB_LaborSafetyEntities())
            {
                RASCUNHO_INVENTARIO_ATIVIDADE inventarioAtividade = entities.RASCUNHO_INVENTARIO_ATIVIDADE
                    .Include(x => x.LOCAL_INSTALACAO_RASCUNHO_INVENTARIO_ATIVIDADE)
                    .Include(x => x.LOCAL_INSTALACAO_RASCUNHO_INVENTARIO_ATIVIDADE.Select(a => a.LOCAL_INSTALACAO))
                    .Include(x => x.RISCO_RASCUNHO_INVENTARIO_ATIVIDADE)
                    .Include(x => x.RISCO_RASCUNHO_INVENTARIO_ATIVIDADE.Select(risco => risco.RISCO).Select(tpr => tpr.TIPO_RISCO))
                    .Include(x => x.RISCO_RASCUNHO_INVENTARIO_ATIVIDADE.Select(risco => risco.EPI_RISCO_RASCUNHO_INVENTARIO_ATIVIDADE))
                    .Where(invAtv => invAtv.CodRascunhoInventarioAtividade == id).FirstOrDefault();
                return inventarioAtividade;
            }
        }
        public IEnumerable<RASCUNHO_INVENTARIO_ATIVIDADE> ListarRascunhoInventarioAtividade(FiltroInventarioAtividadeModelo filtroInventarioAtividadeModelo)
        {
            using (var entities = new DB_LaborSafetyEntities())
            {
                var resultado = entities.RASCUNHO_INVENTARIO_ATIVIDADE
                    .Include(x => x.RISCO_RASCUNHO_INVENTARIO_ATIVIDADE.Select(risco => risco.RISCO))
                    .Include(x => x.RISCO_RASCUNHO_INVENTARIO_ATIVIDADE.Select(risco => risco.RISCO).Select(tpr => tpr.TIPO_RISCO))
                    .Include(x => x.RISCO_RASCUNHO_INVENTARIO_ATIVIDADE.Select(risco => risco.EPI_RISCO_RASCUNHO_INVENTARIO_ATIVIDADE))
                    .Include(x => x.LOCAL_INSTALACAO_RASCUNHO_INVENTARIO_ATIVIDADE.Select(local => local.LOCAL_INSTALACAO))
                    .ToList();

                if (filtroInventarioAtividadeModelo.CodAtividade != 0)
                    resultado = resultado.Where(o => o.CodAtividade == filtroInventarioAtividadeModelo.CodAtividade)
                        .ToList();

                if (filtroInventarioAtividadeModelo.CodDisciplina != 0)
                    resultado = resultado.Where(o => o.CodDisciplina == filtroInventarioAtividadeModelo.CodDisciplina)
                        .ToList();

                if (filtroInventarioAtividadeModelo.CodPeso != 0)
                    resultado = resultado.Where(o => o.CodPeso == filtroInventarioAtividadeModelo.CodPeso)
                        .ToList();

                if (filtroInventarioAtividadeModelo.CodPerfilCatalogo != 0)
                    resultado = resultado.Where(o => o.CodPerfilCatalogo == filtroInventarioAtividadeModelo.CodPerfilCatalogo)
                        .ToList();

                if (filtroInventarioAtividadeModelo.Locais.Count > 0)
                {
                    List<LOCAL_INSTALACAO> locaisOrigem = new List<LOCAL_INSTALACAO>();
                    List<LOCAL_INSTALACAO> locaisAPesquisar = new List<LOCAL_INSTALACAO>();
                    //Busca todos os locais
                    List<LOCAL_INSTALACAO> locais = entities.LOCAL_INSTALACAO
                         .Where(x => x.CodLocalInstalacao != (long)Constantes.LocalInstalacao.SEM_ASSOCIACAO)
                         .ToList();

                    foreach (var nlocal in filtroInventarioAtividadeModelo.Locais)
                    {
                        var local = entities.LOCAL_INSTALACAO.Where(x => x.CodLocalInstalacao == nlocal).FirstOrDefault();

                        if (local == null)
                            throw new Exception($"O local {local.Nome} não consta na base de dados.");

                        locaisOrigem.Add(local);
                    }

                    //Busca filhos dos locais
                    foreach (var local in locaisOrigem)
                    {
                        List<LOCAL_INSTALACAO> locaisFilhos = this.BuscaLocaisEFilhos(locais, local);
                        locaisAPesquisar.AddRange(locaisFilhos);
                    }

                    resultado = resultado.Where(b => b.LOCAL_INSTALACAO_RASCUNHO_INVENTARIO_ATIVIDADE.Any
                    (x => locaisAPesquisar.Contains(x.LOCAL_INSTALACAO))).ToList();
                }

                if (filtroInventarioAtividadeModelo.CodSeveridade != 0)
                    resultado = resultado.Where(x => x.RISCO_RASCUNHO_INVENTARIO_ATIVIDADE.Any(y => y.CodSeveridade == filtroInventarioAtividadeModelo.CodSeveridade))
                        .ToList();

                if (filtroInventarioAtividadeModelo.Riscos.Count != 0 || filtroInventarioAtividadeModelo.Riscos == null)
                    foreach (var risco in filtroInventarioAtividadeModelo.Riscos)
                        resultado = resultado.Where(a => a.RISCO_RASCUNHO_INVENTARIO_ATIVIDADE.Any(x => x.RISCO.CodRisco == risco))
                            .ToList();

                return resultado;
            }
        }

        public void ExcluirRascunhoInventarioAtividade(long id, DB_LaborSafetyEntities entities)
        {
            if (entities == null)
            {
                using (entities = new DB_LaborSafetyEntities())
                {
                    entities.LOCAL_INSTALACAO_RASCUNHO_INVENTARIO_ATIVIDADE.RemoveRange(
    entities.LOCAL_INSTALACAO_RASCUNHO_INVENTARIO_ATIVIDADE
        .Where(local => local.CodRascunhoInventarioAtividade == id).ToList());

                    entities.RISCO_RASCUNHO_INVENTARIO_ATIVIDADE.RemoveRange(
                        entities.RISCO_RASCUNHO_INVENTARIO_ATIVIDADE
                            .Where(risc => risc.CodRascunhoInventarioAtividade == id).ToList());

                    entities.EPI_RISCO_RASCUNHO_INVENTARIO_ATIVIDADE.RemoveRange(
                        entities.EPI_RISCO_RASCUNHO_INVENTARIO_ATIVIDADE
                            .Where(risc => risc.CodRiscoRascunhoInventarioAtividade == risc.RISCO_RASCUNHO_INVENTARIO_ATIVIDADE.CodRiscoRascunhoInventarioAtividade && risc.RISCO_RASCUNHO_INVENTARIO_ATIVIDADE.CodRascunhoInventarioAtividade == id).ToList());

                    var delInventario = entities.RASCUNHO_INVENTARIO_ATIVIDADE.Where(invAtv => invAtv.CodRascunhoInventarioAtividade == id).FirstOrDefault();

                    entities.RASCUNHO_INVENTARIO_ATIVIDADE.Remove(delInventario);

                    entities.SaveChanges();
                }
            }
            else
            {
                entities.LOCAL_INSTALACAO_RASCUNHO_INVENTARIO_ATIVIDADE.RemoveRange(
                    entities.LOCAL_INSTALACAO_RASCUNHO_INVENTARIO_ATIVIDADE
                        .Where(local => local.CodRascunhoInventarioAtividade == id).ToList());

                entities.RISCO_RASCUNHO_INVENTARIO_ATIVIDADE.RemoveRange(
                    entities.RISCO_RASCUNHO_INVENTARIO_ATIVIDADE
                        .Where(risc => risc.CodRascunhoInventarioAtividade == id).ToList());

                entities.EPI_RISCO_RASCUNHO_INVENTARIO_ATIVIDADE.RemoveRange(
                    entities.EPI_RISCO_RASCUNHO_INVENTARIO_ATIVIDADE
                        .Where(risc => risc.CodRiscoRascunhoInventarioAtividade == risc.RISCO_RASCUNHO_INVENTARIO_ATIVIDADE.CodRiscoRascunhoInventarioAtividade && risc.RISCO_RASCUNHO_INVENTARIO_ATIVIDADE.CodRascunhoInventarioAtividade == id).ToList());

                var delInv = entities.RASCUNHO_INVENTARIO_ATIVIDADE.Where(invAtv => invAtv.CodRascunhoInventarioAtividade == id).FirstOrDefault();

                entities.RASCUNHO_INVENTARIO_ATIVIDADE.Remove(delInv);

                entities.SaveChanges();
            }
        }
    }
    }