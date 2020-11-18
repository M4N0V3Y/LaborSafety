using System;
using System.Collections.Generic;
using System.Linq;
using LaborSafety.Dominio.Modelos;
using System.Data.Entity;
using LaborSafety.Utils.Constantes;
using LaborSafety.Persistencia.Interfaces;

namespace LaborSafety.Persistencia.Servicos
{
    public class RascunhoInventarioAmbientePersistencia : IRascunhoInventarioAmbientePersistencia
    {
        private readonly IDbLaborSafetyEntities databaseEntities;
        public RascunhoInventarioAmbientePersistencia(IDbLaborSafetyEntities databaseEntities)
        {
            this.databaseEntities = databaseEntities;
        }

        public void InserirRascunhoInventarioAmbiente(RascunhoInventarioAmbienteModelo rascunhoInventarioAmbienteModelo)
        {
            using (var entities = databaseEntities.GetDB_LaborSafetyEntities())
            {
                using (var transaction = entities.Database.BeginTransaction())
                {
                    try
                    {
                        this.Inserir(rascunhoInventarioAmbienteModelo, entities);
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

        // insert inventario ambiente
        public RASCUNHO_INVENTARIO_AMBIENTE Inserir(RascunhoInventarioAmbienteModelo rascunhoInventarioAmbienteModelo, DB_LaborSafetyEntities entities)
        {
            List<NrRascunhoInventarioAmbienteModelo> nrs = rascunhoInventarioAmbienteModelo.NR_RASCUNHO_INVENTARIO_AMBIENTE;
            List<RiscoRascunhoInventarioAmbienteModelo> riscos = rascunhoInventarioAmbienteModelo.RISCO_RASCUNHO_INVENTARIO_AMBIENTE;
            List<LocalInstalacaoModelo> locais = rascunhoInventarioAmbienteModelo.LOCAL_INSTALACAO_MODELO;

            entities.Configuration.AutoDetectChangesEnabled = false;

            try
            {
                var inventario = new RASCUNHO_INVENTARIO_AMBIENTE()
                {
                    Codigo = $"R_INV_AMB - {rascunhoInventarioAmbienteModelo.CodRascunhoInventarioAmbiente} - {rascunhoInventarioAmbienteModelo.RiscoGeral}",
                    CodAmbiente = rascunhoInventarioAmbienteModelo.CodAmbiente,
                    Descricao = rascunhoInventarioAmbienteModelo.Descricao,
                    ObservacaoGeral = rascunhoInventarioAmbienteModelo.ObservacaoGeral,
                    RiscoGeral = rascunhoInventarioAmbienteModelo.RiscoGeral,
                    CodLocalInstalacao = rascunhoInventarioAmbienteModelo.CodLocalInstalacao
                };

                entities.RASCUNHO_INVENTARIO_AMBIENTE.Add(inventario);
                entities.SaveChanges();

                long idInv = inventario.CodRascunhoInventarioAmbiente;

                if (nrs != null)
                {
                    foreach (var nr in nrs)
                    {
                        entities.NR_RASCUNHO_INVENTARIO_AMBIENTE.Add(new NR_RASCUNHO_INVENTARIO_AMBIENTE()
                        {
                            CodRascunhoInventarioAmbiente = idInv,
                            CodNR = nr.CodNR
                        });
                    }

                    entities.SaveChanges();
                }

                if (riscos != null)
                {
                    foreach (var risco in riscos)
                    {
                            var novoRisco = new RISCO_RASCUNHO_INVENTARIO_AMBIENTE()
                            {
                                CodRascunhoInventarioAmbiente = idInv,
                                CodRisco = risco.CodRiscoAmbiente,
                                CodSeveridade = risco.CodSeveridade,
                                CodProbabilidade = risco.CodProbabilidade,
                                FonteGeradora = risco.FonteGeradora,
                                ProcedimentosAplicaveis = risco.ProcedimentosAplicaveis,
                                ContraMedidas = risco.ContraMedidas
                            };
                        entities.RISCO_RASCUNHO_INVENTARIO_AMBIENTE.Add(novoRisco);
                        entities.SaveChanges();
                        if (risco.EPIRiscoRascunhoInventarioAmbiente.Count >= 0)
                        {
                            foreach (var epi in risco.EPIRiscoRascunhoInventarioAmbiente)
                            {
                                entities.EPI_RISCO_RASCUNHO_INVENTARIO_AMBIENTE.Add(new EPI_RISCO_RASCUNHO_INVENTARIO_AMBIENTE()
                                {
                                    CodRiscoRascunhoInventarioAmbiente = novoRisco.CodRiscoRascunhoInventarioAmbiente,
                                    CodEPI = epi.CodEPI
                                });
                            }
                        }
                    }

                    entities.ChangeTracker.DetectChanges();
                    entities.SaveChanges();

                    entities.Configuration.AutoDetectChangesEnabled = true;
                }
                return inventario;
            }

            catch (Exception exception)
            {
                throw exception;
            }
        }

        public List<LOCAL_INSTALACAO> BuscaFilhosPorNivelExcetoInventario(long codLocal,
           DB_LaborSafetyEntities entities = null)
        {
            if (entities == null)
                entities = new DB_LaborSafetyEntities();

            entities.Database.CommandTimeout = 9999;

            var localEnviado = entities.LOCAL_INSTALACAO.Where(x => x.CodLocalInstalacao == codLocal)
                .FirstOrDefault();

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

        public List<LOCAL_INSTALACAO> BuscaFilhosPorNivelDoInventario(long codLocal, long idInventario,
    DB_LaborSafetyEntities entities = null)
        {
            if (entities == null)
                entities = new DB_LaborSafetyEntities();

            entities.Database.CommandTimeout = 9999;

            var localEnviado = entities.LOCAL_INSTALACAO.Where(x => x.CodLocalInstalacao == codLocal)
                .FirstOrDefault();

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
                && x.N3 == localEnviado.N3 && x.N4 == localEnviado.N4 &&
                x.N5 == localEnviado.N5).ToList();
            else
                locaisFiltrados = entities.LOCAL_INSTALACAO.Where(x => x.N1 == localEnviado.N1 && x.N2 == localEnviado.N2
                && x.N3 == localEnviado.N3 && x.N4 == localEnviado.N4 && x.
                N5 == localEnviado.N5 && x.N6 == localEnviado.N6).ToList();

            return locaisFiltrados;
        }

        public void EditarRascunhoInventarioAmbiente(RascunhoInventarioAmbienteModelo rascunhoInventarioAmbienteModelo, DB_LaborSafetyEntities entities, DbContextTransaction transaction)
        {
                RASCUNHO_INVENTARIO_AMBIENTE rascunhoInventarioAmbienteExistente = entities.RASCUNHO_INVENTARIO_AMBIENTE.Where(invAtv => invAtv.CodRascunhoInventarioAmbiente == rascunhoInventarioAmbienteModelo.CodRascunhoInventarioAmbiente).FirstOrDefault();

                if (rascunhoInventarioAmbienteExistente == null)
                {
                    throw new KeyNotFoundException();
                }

                else
                {
                        try
                        {
                            if (!rascunhoInventarioAmbienteModelo.novoInventario)
                            {
                                ExcluirRascunhoInventarioAmbiente(rascunhoInventarioAmbienteExistente.CodRascunhoInventarioAmbiente, entities);

                                Inserir(rascunhoInventarioAmbienteModelo, entities);

                                //transaction.Commit();
                            }

                            else
                            {
                                ExcluirRascunhoInventarioAmbiente(rascunhoInventarioAmbienteExistente.CodRascunhoInventarioAmbiente, entities);
                            }
                        }
                        catch (Exception)
                        {
                            transaction.Rollback();
                            throw;
                        }
                }
        }

        public RASCUNHO_INVENTARIO_AMBIENTE ListarRascunhoInventarioAmbientePorLI(long li)
        {
            using (var entities = new DB_LaborSafetyEntities())
            {
                RASCUNHO_INVENTARIO_AMBIENTE rascunhoInventarioAmbiente = entities.RASCUNHO_INVENTARIO_AMBIENTE
                    .Include(x => x.RISCO_RASCUNHO_INVENTARIO_AMBIENTE)
                    .Include(x => x.NR_RASCUNHO_INVENTARIO_AMBIENTE).FirstOrDefault();
                //.Where(x => x.CodLocalInstalacao == li.ToString()).FirstOrDefault();

                return rascunhoInventarioAmbiente;
            }
        }

        public RASCUNHO_INVENTARIO_AMBIENTE ListarRascunhoInventarioAmbientePorId(long id)
        {
            using (var entities = new DB_LaborSafetyEntities())
            {
                RASCUNHO_INVENTARIO_AMBIENTE inventarioAmbiente = entities.RASCUNHO_INVENTARIO_AMBIENTE
                    .Include(x => x.NR_RASCUNHO_INVENTARIO_AMBIENTE)
                    .Include(x => x.RISCO_RASCUNHO_INVENTARIO_AMBIENTE)
                    .Include(x => x.RISCO_RASCUNHO_INVENTARIO_AMBIENTE.Select(risco => risco.RISCO).Select(tpr => tpr.TIPO_RISCO))
                    .Include(x => x.RISCO_RASCUNHO_INVENTARIO_AMBIENTE.Select(risco => risco.EPI_RISCO_RASCUNHO_INVENTARIO_AMBIENTE))
                    .Where(invAmb => invAmb.CodRascunhoInventarioAmbiente == id).FirstOrDefault();
                return inventarioAmbiente;
            }
        }

        public IEnumerable<RASCUNHO_INVENTARIO_AMBIENTE> ListarRascunhoInventarioAmbiente(FiltroInventarioAmbienteModelo filtroInventarioAmbienteModelo)
        {

            using (var entities = new DB_LaborSafetyEntities())
            {
                List<RASCUNHO_INVENTARIO_AMBIENTE> rascunhosDistinct = new List<RASCUNHO_INVENTARIO_AMBIENTE>();
                List<RASCUNHO_INVENTARIO_AMBIENTE> rascunhos = new List<RASCUNHO_INVENTARIO_AMBIENTE>();
                var resultado = entities.RASCUNHO_INVENTARIO_AMBIENTE

                    .Include(x => x.NR_RASCUNHO_INVENTARIO_AMBIENTE.Select(nr => nr.NR))
                    .Include(x => x.RISCO_RASCUNHO_INVENTARIO_AMBIENTE.Select(risco => risco.RISCO))
                    .Include(x => x.RISCO_RASCUNHO_INVENTARIO_AMBIENTE.Select(risco => risco.RISCO).Select(tpr => tpr.TIPO_RISCO))
                    .Include(x => x.RISCO_RASCUNHO_INVENTARIO_AMBIENTE.Select(risco => risco.EPI_RISCO_RASCUNHO_INVENTARIO_AMBIENTE));

                if (filtroInventarioAmbienteModelo.CodSeveridade != 0 && filtroInventarioAmbienteModelo.CodSeveridade != null)
                    resultado = resultado.Where(x => x.RISCO_RASCUNHO_INVENTARIO_AMBIENTE.Any(y => y.CodSeveridade == filtroInventarioAmbienteModelo.CodSeveridade));

                if (filtroInventarioAmbienteModelo.CodProbabilidade != 0 && filtroInventarioAmbienteModelo.CodProbabilidade != null)
                    resultado = resultado.Where(x => x.RISCO_RASCUNHO_INVENTARIO_AMBIENTE.Any(y => y.CodProbabilidade == filtroInventarioAmbienteModelo.CodProbabilidade));

                if (filtroInventarioAmbienteModelo.CodAmbiente != 0 && filtroInventarioAmbienteModelo.CodAmbiente != null)
                    resultado = resultado.Where(a => a.CodAmbiente == filtroInventarioAmbienteModelo.CodAmbiente);

                if (filtroInventarioAmbienteModelo.Riscos != null && filtroInventarioAmbienteModelo.Riscos.Count > 0)
                {
                    foreach (var risco in filtroInventarioAmbienteModelo.Riscos)
                        resultado = resultado.Where(a => a.RISCO_RASCUNHO_INVENTARIO_AMBIENTE.Any(x => x.RISCO.CodRisco == risco));
                }

                var resultadoQuery = resultado.ToList();
                

                if (filtroInventarioAmbienteModelo.CodLocaisInstalacao != null && filtroInventarioAmbienteModelo.CodLocaisInstalacao.Count > 0)
                {
                    List<string> CodLocais = new List<string>();
                    List<LOCAL_INSTALACAO> listaLocaisComInventarios = new List<LOCAL_INSTALACAO>();
                    //Busca todos os locais
                    List<LOCAL_INSTALACAO> locais = entities.LOCAL_INSTALACAO
                         .Where(x => x.CodLocalInstalacao != (long)Constantes.LocalInstalacao.SEM_ASSOCIACAO)
                         .ToList();

                    foreach (var item in filtroInventarioAmbienteModelo.CodLocaisInstalacao)
                    {
                        var local = entities.LOCAL_INSTALACAO.Where(lc => lc.CodLocalInstalacao == item).FirstOrDefault();
                        if (local != null)
                            CodLocais.Add(local.CodLocalInstalacao.ToString());
                        else
                            throw new Exception($"O local {item} não foi encontrado na base de dados!");

                        //List<LOCAL_INSTALACAO> locaisFilhos = BuscaFilhosPorNivelExcetoInventario(local.CodLocalInstalacao, entities);

                        //Filtra somente os locais do pai
                        List<LOCAL_INSTALACAO> locaisFilhos = this.BuscaLocaisEFilhos(locais, local);

                        listaLocaisComInventarios.AddRange(locaisFilhos);

                        foreach (var itemLocalComInventario in listaLocaisComInventarios)
                            CodLocais.Add(itemLocalComInventario.CodLocalInstalacao.ToString());
                    }

                    List<int> codLocaisInt = new List<int>();
                    
                    foreach (var itemInteiro in CodLocais)
                        codLocaisInt.Add(Convert.ToInt32(itemInteiro));

                    foreach (var itemResultado in resultadoQuery)
                    {
                        if (!string.IsNullOrEmpty(itemResultado.CodLocalInstalacao))
                        {
                            var separarLocais = itemResultado.CodLocalInstalacao.Split(',');
                            foreach (var item in separarLocais)
                            {
                                if (codLocaisInt.Contains(Convert.ToInt32(item)))
                                    rascunhos.Add(itemResultado);
                            }
                        }
                    }
                }

                

                if (rascunhos.Count <= 0)
                    rascunhosDistinct = resultadoQuery;

                else
                    rascunhosDistinct = rascunhos.Distinct().ToList();

                return rascunhosDistinct;
            }
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

        public void ExcluirRascunhoInventarioAmbiente(long id, DB_LaborSafetyEntities entities)
        {
            if (entities == null)
            {
                using (entities = new DB_LaborSafetyEntities())
                {
                    entities.NR_RASCUNHO_INVENTARIO_AMBIENTE.RemoveRange(
                        entities.NR_RASCUNHO_INVENTARIO_AMBIENTE
                            .Where(nr => nr.CodRascunhoInventarioAmbiente == id).ToList());

                    entities.RISCO_RASCUNHO_INVENTARIO_AMBIENTE.RemoveRange(
                        entities.RISCO_RASCUNHO_INVENTARIO_AMBIENTE
                            .Where(risco => risco.CodRascunhoInventarioAmbiente == id).ToList());

                    entities.EPI_RISCO_RASCUNHO_INVENTARIO_AMBIENTE.RemoveRange(
entities.EPI_RISCO_RASCUNHO_INVENTARIO_AMBIENTE
.Where(risc => risc.CodRiscoRascunhoInventarioAmbiente == risc.RISCO_RASCUNHO_INVENTARIO_AMBIENTE.CodRiscoRascunhoInventarioAmbiente && risc.RISCO_RASCUNHO_INVENTARIO_AMBIENTE.CodRascunhoInventarioAmbiente == id).ToList());

                    var delInv = entities.RASCUNHO_INVENTARIO_AMBIENTE.Where(invAmb => invAmb.CodRascunhoInventarioAmbiente == id).FirstOrDefault();

                    entities.RASCUNHO_INVENTARIO_AMBIENTE.Remove(delInv);

                    entities.SaveChanges();
                }
            }

            else
            {
                    entities.NR_RASCUNHO_INVENTARIO_AMBIENTE.RemoveRange(
                        entities.NR_RASCUNHO_INVENTARIO_AMBIENTE
                            .Where(nr => nr.CodRascunhoInventarioAmbiente == id).ToList());

                    entities.RISCO_RASCUNHO_INVENTARIO_AMBIENTE.RemoveRange(
                        entities.RISCO_RASCUNHO_INVENTARIO_AMBIENTE
                            .Where(risco => risco.CodRascunhoInventarioAmbiente == id).ToList());

                entities.EPI_RISCO_RASCUNHO_INVENTARIO_AMBIENTE.RemoveRange(
                    entities.EPI_RISCO_RASCUNHO_INVENTARIO_AMBIENTE
                        .Where(risc => risc.CodRiscoRascunhoInventarioAmbiente == risc.RISCO_RASCUNHO_INVENTARIO_AMBIENTE.CodRiscoRascunhoInventarioAmbiente && risc.RISCO_RASCUNHO_INVENTARIO_AMBIENTE.CodRascunhoInventarioAmbiente == id).ToList());


                var delInv = entities.RASCUNHO_INVENTARIO_AMBIENTE.Where(invAmb => invAmb.CodRascunhoInventarioAmbiente == id).FirstOrDefault();

                    entities.RASCUNHO_INVENTARIO_AMBIENTE.Remove(delInv);

                    entities.SaveChanges();
            }
        }
    }
}
