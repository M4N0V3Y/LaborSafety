using System;
using System.Collections.Generic;
using System.Linq;
using LaborSafety.Dominio.Modelos;
using LaborSafety.Persistencia.Interfaces;
using System.Data.Entity;
using LaborSafety.Utils.Constantes;
using LaborSafety.Dominio.Modelos.Exportacao;

namespace LaborSafety.Persistencia.Servicos
{
    public class InventarioAmbientePersistencia : IInventariosAmbientePersistencia
    {
        private readonly IDbLaborSafetyEntities databaseEntities;
        public InventarioAmbientePersistencia(IDbLaborSafetyEntities databaseEntities)
        {
            this.databaseEntities = databaseEntities;
        }

        // pesquisa por Id
        public INVENTARIO_AMBIENTE ListarInventarioAmbientePorId(long id, DB_LaborSafetyEntities entities = null, bool validarAtivo = true)
        {
            if (entities == null)
                entities = new DB_LaborSafetyEntities();

            if (validarAtivo)
            {
                INVENTARIO_AMBIENTE inventarioAmbiente = entities.INVENTARIO_AMBIENTE
                    .Include(x => x.NR_INVENTARIO_AMBIENTE)
                    .Include(x => x.RISCO_INVENTARIO_AMBIENTE)
                    .Include(x => x.LOCAL_INSTALACAO)
                    .Include(x => x.RISCO_INVENTARIO_AMBIENTE.Select(sev => sev.SEVERIDADE))
                    .Include(x => x.RISCO_INVENTARIO_AMBIENTE.Select(prob => prob.PROBABILIDADE))
                    .Include(x => x.RISCO_INVENTARIO_AMBIENTE.Select(risco => risco.RISCO))
                    .Include(x => x.RISCO_INVENTARIO_AMBIENTE.Select(risco => risco.RISCO).Select(tpr => tpr.TIPO_RISCO))
                    .Include(x => x.RISCO_INVENTARIO_AMBIENTE.Select(risco => risco.EPI_RISCO_INVENTARIO_AMBIENTE))
                    .Where(invAmb => invAmb.CodInventarioAmbiente == id && invAmb.Ativo).FirstOrDefault();
                return inventarioAmbiente;
            }
            else
            {
                INVENTARIO_AMBIENTE inventarioAmbiente = entities.INVENTARIO_AMBIENTE
                .Include(x => x.NR_INVENTARIO_AMBIENTE)
                .Include(x => x.RISCO_INVENTARIO_AMBIENTE)
                .Include(x => x.LOCAL_INSTALACAO)
                .Include(x => x.RISCO_INVENTARIO_AMBIENTE.Select(sev => sev.SEVERIDADE))
                .Include(x => x.RISCO_INVENTARIO_AMBIENTE.Select(prob => prob.PROBABILIDADE))
                .Include(x => x.RISCO_INVENTARIO_AMBIENTE.Select(risco => risco.RISCO))
                .Include(x => x.RISCO_INVENTARIO_AMBIENTE.Select(risco => risco.RISCO).Select(tpr => tpr.TIPO_RISCO))
                .Include(x => x.RISCO_INVENTARIO_AMBIENTE.Select(risco => risco.EPI_RISCO_INVENTARIO_AMBIENTE))
                .Where(invAmb => invAmb.CodInventarioAmbiente == id).FirstOrDefault();
                return inventarioAmbiente;
            }



        }

        public List<INVENTARIO_AMBIENTE> ListarTodosInventarios()
        {
            using (var entities = new DB_LaborSafetyEntities())
            {
                var resultado = entities.INVENTARIO_AMBIENTE
                    .Include(x => x.NR_INVENTARIO_AMBIENTE)
                    .Include(x => x.RISCO_INVENTARIO_AMBIENTE)
                    .Include(x => x.LOCAL_INSTALACAO)
                    .Include(x => x.RISCO_INVENTARIO_AMBIENTE.Select(sev => sev.SEVERIDADE))
                    .Include(x => x.RISCO_INVENTARIO_AMBIENTE.Select(prob => prob.PROBABILIDADE))
                    .Include(x => x.RISCO_INVENTARIO_AMBIENTE.Select(risco => risco.RISCO))
                    .Include(x => x.RISCO_INVENTARIO_AMBIENTE.Select(risco => risco.RISCO).Select(tpr => tpr.TIPO_RISCO))
                    .Include(x => x.RISCO_INVENTARIO_AMBIENTE.Select(risco => risco.EPI_RISCO_INVENTARIO_AMBIENTE))
                    .Where(invAmb => invAmb.Ativo && invAmb.CodInventarioAmbiente != (long)Constantes.InventarioAmbiente.SEM_INVENTARIO).ToList();

                return resultado;
            }
        }

        public INVENTARIO_AMBIENTE ListarInventarioAmbientePorCodigo(string codigo, DB_LaborSafetyEntities entities = null)
        {
            if (entities == null)
                entities = new DB_LaborSafetyEntities();
            var inventarioAmbiente = entities.INVENTARIO_AMBIENTE.Where(invAmb => invAmb.Codigo == codigo && invAmb.Ativo).FirstOrDefault();
            if (inventarioAmbiente == null)
            {
                return null;
            }

            return ListarInventarioAmbientePorId(inventarioAmbiente.CodInventarioAmbiente,entities);
        }

        public INVENTARIO_AMBIENTE ListarInventarioAmbienteAtivadoEDesativadoPorId(long id, DB_LaborSafetyEntities entities = null)
        {
            if (entities == null)
                entities = new DB_LaborSafetyEntities();


            INVENTARIO_AMBIENTE inventarioAmbiente = entities.INVENTARIO_AMBIENTE
                .Include(x => x.NR_INVENTARIO_AMBIENTE)
                .Include(x => x.RISCO_INVENTARIO_AMBIENTE)
                .Include(x => x.LOCAL_INSTALACAO)
                .Include(x => x.RISCO_INVENTARIO_AMBIENTE.Select(sev => sev.SEVERIDADE))
                .Include(x => x.RISCO_INVENTARIO_AMBIENTE.Select(prob => prob.PROBABILIDADE))
                .Include(x => x.RISCO_INVENTARIO_AMBIENTE.Select(risco => risco.RISCO))
                .Include(x => x.RISCO_INVENTARIO_AMBIENTE.Select(risco => risco.RISCO).Select(tpr => tpr.TIPO_RISCO))
                .Include(x => x.RISCO_INVENTARIO_AMBIENTE.Select(risco => risco.EPI_RISCO_INVENTARIO_AMBIENTE))
                .Where(invAmb => invAmb.CodInventarioAmbiente == id).FirstOrDefault();
            return inventarioAmbiente;

        }

        public INVENTARIO_AMBIENTE ListarInventarioAmbientePorIdAmbiente(long idAmbiente)
        {
            using (var entities = new DB_LaborSafetyEntities())
            {
                INVENTARIO_AMBIENTE inventarioAmbiente = entities.INVENTARIO_AMBIENTE
                    .Where(invAmb => invAmb.CodAmbiente == idAmbiente && invAmb.Ativo).FirstOrDefault();
                return inventarioAmbiente;
            }
        }

        public IEnumerable<INVENTARIO_AMBIENTE> ListarInventarioAmbiente(FiltroInventarioAmbienteModelo filtroInventarioAmbienteModelo)
        {

            using (var entities = new DB_LaborSafetyEntities())
            {

                var resultado = entities.INVENTARIO_AMBIENTE

                    .Include(x => x.NR_INVENTARIO_AMBIENTE.Select(nr => nr.NR))
                    .Include(x => x.RISCO_INVENTARIO_AMBIENTE.Select(sev => sev.SEVERIDADE))
                    .Include(x => x.RISCO_INVENTARIO_AMBIENTE.Select(prob => prob.PROBABILIDADE))
                    .Include(x => x.RISCO_INVENTARIO_AMBIENTE.Select(risco => risco.RISCO))
                    .Include(x => x.RISCO_INVENTARIO_AMBIENTE.Select(risco => risco.RISCO).Select(tpr => tpr.TIPO_RISCO))
                    .Include(x => x.RISCO_INVENTARIO_AMBIENTE.Select(risco => risco.EPI_RISCO_INVENTARIO_AMBIENTE))
                    .Include(x => x.LOCAL_INSTALACAO.Select(peso => peso.PESO))
                    .Include(x => x.LOCAL_INSTALACAO.Select(peso => peso.PERFIL_CATALOGO))
                    .Include(x => x.AMBIENTE)
                    .Where(x => x.Ativo && x.CodInventarioAmbiente != (long)Constantes.InventarioAmbiente.SEM_INVENTARIO);

                if (filtroInventarioAmbienteModelo.CodSeveridade != 0 && filtroInventarioAmbienteModelo.CodSeveridade != null)
                    resultado = resultado.Where(x => x.RISCO_INVENTARIO_AMBIENTE.Any(y => y.CodSeveridade == filtroInventarioAmbienteModelo.CodSeveridade));

                if (filtroInventarioAmbienteModelo.CodProbabilidade != 0 && filtroInventarioAmbienteModelo.CodProbabilidade != null)
                    resultado = resultado.Where(x => x.RISCO_INVENTARIO_AMBIENTE.Any(y => y.CodProbabilidade == filtroInventarioAmbienteModelo.CodProbabilidade));

                if (filtroInventarioAmbienteModelo.CodAmbiente != 0 && filtroInventarioAmbienteModelo.CodAmbiente != null)
                    resultado = resultado.Where(a => a.CodAmbiente == filtroInventarioAmbienteModelo.CodAmbiente);

                if (filtroInventarioAmbienteModelo.Riscos != null)
                {
                    foreach (var risco in filtroInventarioAmbienteModelo.Riscos)
                        resultado = resultado.Where(a => a.RISCO_INVENTARIO_AMBIENTE.Any(x => x.RISCO.CodRisco == risco));
                }

                var resultadoQuery = resultado.ToList();

                if (filtroInventarioAmbienteModelo.CodLocaisInstalacao.Count > 0)
                {
                    List<LOCAL_INSTALACAO> listaLocaisComInventarios = new List<LOCAL_INSTALACAO>();

                    foreach (var local in filtroInventarioAmbienteModelo.CodLocaisInstalacao)
                    {
                        var localEnviado = entities.LOCAL_INSTALACAO
                            .Where(loc => loc.CodLocalInstalacao == local && loc.CodLocalInstalacao != (long)Constantes.LocalInstalacao.SEM_ASSOCIACAO).FirstOrDefault();

                        //Filtra somente os locais do pai
                        List<LOCAL_INSTALACAO> locaisFilhos = this.BuscaLocaisEFilhos(localEnviado, entities);

                        listaLocaisComInventarios.AddRange(locaisFilhos);
                    }
                       
                    resultadoQuery = resultadoQuery.Where(local => local.LOCAL_INSTALACAO.Any(x => listaLocaisComInventarios.Contains(x))).ToList();
                }

                return resultadoQuery;
            }
        }

        private List<LOCAL_INSTALACAO> BuscaLocaisEFilhos(LOCAL_INSTALACAO localEnviado, DB_LaborSafetyEntities entities)
        {
            List<LOCAL_INSTALACAO> locaisFiltrados = new List<LOCAL_INSTALACAO>();

            if (String.IsNullOrEmpty(localEnviado.N2))
                locaisFiltrados = entities.LOCAL_INSTALACAO.Where(x => x.N1 == localEnviado.N1 &&
                x.CodLocalInstalacao != (long)Constantes.LocalInstalacao.SEM_ASSOCIACAO).ToList();
            else if (String.IsNullOrEmpty(localEnviado.N3))
                locaisFiltrados = entities.LOCAL_INSTALACAO.Where(x => x.N1 == localEnviado.N1 && x.N2 == localEnviado.N2
                  && x.CodLocalInstalacao != (long)Constantes.LocalInstalacao.SEM_ASSOCIACAO).ToList();
            else if (String.IsNullOrEmpty(localEnviado.N4))
                locaisFiltrados = entities.LOCAL_INSTALACAO.Where(x => x.N1 == localEnviado.N1 && x.N2 == localEnviado.N2 &&
                x.N3 == localEnviado.N3 && x.CodLocalInstalacao != (long)Constantes.LocalInstalacao.SEM_ASSOCIACAO).ToList();
            else if (String.IsNullOrEmpty(localEnviado.N5))
                locaisFiltrados = entities.LOCAL_INSTALACAO.Where(x => x.N1 == localEnviado.N1 && x.N2 == localEnviado.N2 &&
                x.N3 == localEnviado.N3 && x.N4 == localEnviado.N4 && x.CodLocalInstalacao != (long)Constantes.LocalInstalacao.SEM_ASSOCIACAO).ToList();
            else if (String.IsNullOrEmpty(localEnviado.N6))
                locaisFiltrados = entities.LOCAL_INSTALACAO.Where(x => x.N1 == localEnviado.N1 && x.N2 == localEnviado.N2
                && x.N3 == localEnviado.N3 && x.N4 == localEnviado.N4 && x.N5 == localEnviado.N5
                && x.CodLocalInstalacao != (long)Constantes.LocalInstalacao.SEM_ASSOCIACAO).ToList();
            else
                locaisFiltrados = entities.LOCAL_INSTALACAO.Where(x => x.N1 == localEnviado.N1 && x.N2 == localEnviado.N2
                && x.N3 == localEnviado.N3 && x.N4 == localEnviado.N4 && x.N5 == localEnviado.N5 && x.N6 == localEnviado.N6
                && x.CodLocalInstalacao != (long)Constantes.LocalInstalacao.SEM_ASSOCIACAO).ToList();

            return locaisFiltrados;
        }

        public List<INVENTARIO_AMBIENTE> ListarInventarioAmbienteExportacao(DadosExportacaoAmbienteModelo dados)
        {

            using (var entities = new DB_LaborSafetyEntities())
            {
                List<INVENTARIO_AMBIENTE> resultadoSistemaOperacional = new List<INVENTARIO_AMBIENTE>();
                List<INVENTARIO_AMBIENTE> resultadoNR = new List<INVENTARIO_AMBIENTE>();
                List<INVENTARIO_AMBIENTE> resultadoRisco = new List<INVENTARIO_AMBIENTE>();
                List<INVENTARIO_AMBIENTE> resultadoProbabilidade = new List<INVENTARIO_AMBIENTE>();
                List<INVENTARIO_AMBIENTE> resultadoSeveridade = new List<INVENTARIO_AMBIENTE>();
                List<INVENTARIO_AMBIENTE> resultadoEPI = new List<INVENTARIO_AMBIENTE>();
                List<INVENTARIO_AMBIENTE> resultadoLocalInstalacao = new List<INVENTARIO_AMBIENTE>();
                List<INVENTARIO_AMBIENTE> resultadoFinal = new List<INVENTARIO_AMBIENTE>();
                int cont = 0;

                var resultado = entities.INVENTARIO_AMBIENTE

                    .Include(x => x.NR_INVENTARIO_AMBIENTE.Select(nr => nr.NR))
                    .Include(x => x.RISCO_INVENTARIO_AMBIENTE.Select(sev => sev.SEVERIDADE))
                    .Include(x => x.RISCO_INVENTARIO_AMBIENTE.Select(prob => prob.PROBABILIDADE))
                    .Include(x => x.RISCO_INVENTARIO_AMBIENTE.Select(risco => risco.RISCO))
                    .Include(x => x.RISCO_INVENTARIO_AMBIENTE.Select(risco => risco.RISCO).Select(tpr => tpr.TIPO_RISCO))
                    .Include(x => x.RISCO_INVENTARIO_AMBIENTE.Select(risco => risco.EPI_RISCO_INVENTARIO_AMBIENTE))
                    .Include(x => x.AMBIENTE)
                    .Where(b => b.LOCAL_INSTALACAO.All(y => y.CodLocalInstalacao != (int)Constantes.LocalInstalacao.SEM_ASSOCIACAO)
                    && b.CodInventarioAmbiente != (long)Constantes.InventarioAmbiente.SEM_INVENTARIO && b.Ativo).ToList();

                if (dados.AMBIENTE.Count > 0 && dados.AMBIENTE != null)
                {
                    resultadoSistemaOperacional = resultado.Where(b => dados.AMBIENTE.Contains(b.CodAmbiente)).ToList();
                    cont++;
                }
                    

                if (dados.NR.Count > 0 && dados.NR != null)
                {
                    resultadoNR = resultado.Where(teste => teste.NR_INVENTARIO_AMBIENTE.Any(a => dados.NR.Contains(a.CodNR))).ToList();
                    cont++;
                }
                    

                if (dados.RISCO.Count > 0 && dados.RISCO != null)
                {
                    resultadoRisco = resultado.Where(teste => teste.RISCO_INVENTARIO_AMBIENTE.Any(a => dados.RISCO.Contains(a.CodRiscoAmbiente))).ToList();
                    cont++;
                }
                    

                if (dados.PROBABILIDADE.Count > 0 && dados.PROBABILIDADE != null)
                {
                    resultadoProbabilidade = resultado.Where(teste => teste.RISCO_INVENTARIO_AMBIENTE.Any(a => dados.PROBABILIDADE.Contains(a.CodProbabilidade))).ToList();
                    cont++;
                }
                    

                if (dados.SEVERIDADE.Count > 0 && dados.SEVERIDADE != null)
                {
                    resultadoSeveridade = resultado.Where(teste => teste.RISCO_INVENTARIO_AMBIENTE.Any(a => dados.SEVERIDADE.Contains(a.CodSeveridade))).ToList();
                    cont++;
                }
                    

                if (dados.EPI.Count > 0 && dados.EPI != null)
                {
                    resultadoEPI = resultado.Where(teste => teste.RISCO_INVENTARIO_AMBIENTE.All(i => i.EPI_RISCO_INVENTARIO_AMBIENTE.Any(a => dados.EPI.Contains(a.CodEPI)))).ToList();
                    cont++;
                }

                if (dados.LOCAL_INSTALACAO.Count > 0 && dados.LOCAL_INSTALACAO != null)
                {
                    List<long> listaLocais = new List<long>();
                    foreach (var item in dados.LOCAL_INSTALACAO)
                    {
                        var localInstalacao = entities.LOCAL_INSTALACAO.Where(nomeLocal => nomeLocal.CodLocalInstalacao == item).FirstOrDefault();
                        var codigoLocal = localInstalacao.CodLocalInstalacao;

                        listaLocais.Add(codigoLocal);
                    }
                    resultadoLocalInstalacao = resultado.Where(b => b.LOCAL_INSTALACAO.Any(a => listaLocais.Contains(a.CodLocalInstalacao))).ToList();
                    cont++;
                }

                foreach (var itemSO in resultadoSistemaOperacional)
                {
                    resultadoFinal.Add(itemSO);
                }

                foreach (var itemNR in resultadoNR)
                {
                    resultadoFinal.Add(itemNR);
                }

                foreach (var itemRisco in resultadoRisco)
                {
                    resultadoFinal.Add(itemRisco);
                }
                   
                foreach (var itemProbabilidade in resultadoProbabilidade)
                {
                    resultadoFinal.Add(itemProbabilidade);
                }
                    
                foreach (var itemSeveridade in resultadoSeveridade)
                {
                    resultadoFinal.Add(itemSeveridade);
                }
                    
                foreach (var itemEPI in resultadoEPI)
                {
                    resultadoFinal.Add(itemEPI);
                }
                    
                foreach (var itemLocalInstalacao in resultadoLocalInstalacao)
                {
                    resultadoFinal.Add(itemLocalInstalacao);
                }

                if (resultado.Count > 0 && cont != 0)
                    resultadoFinal = resultadoFinal.Distinct().ToList();

                else if(resultado.Count > 0 && cont == 0)
                    resultadoFinal = resultado.Distinct().ToList();

                else
                    throw new Exception("Não foram encontrados inventários para exportação.");

                return resultadoFinal;
            }
        }

        public INVENTARIO_AMBIENTE ListarInventarioAmbientePorLI(long li, DB_LaborSafetyEntities entities = null)
        {
            if (entities == null)
                entities = new DB_LaborSafetyEntities();

            INVENTARIO_AMBIENTE inventarioAmbiente = entities.INVENTARIO_AMBIENTE
                    .Include(x => x.LOCAL_INSTALACAO)
                    .Include(x => x.RISCO_INVENTARIO_AMBIENTE)
                    .Include(x => x.NR_INVENTARIO_AMBIENTE)
                    .Where(x => x.LOCAL_INSTALACAO.Any(y => y.CodLocalInstalacao == li) &&
                    x.CodInventarioAmbiente != (long)Constantes.InventarioAmbiente.SEM_INVENTARIO && x.Ativo).FirstOrDefault();

                return inventarioAmbiente;
            
        }

        public INVENTARIO_AMBIENTE ListarInventarioAmbienteAtivadoEDesativadoPorLI(long li, DB_LaborSafetyEntities entities = null)
        {
            if (entities == null)
                entities = new DB_LaborSafetyEntities();

            INVENTARIO_AMBIENTE inventarioAmbiente = entities.INVENTARIO_AMBIENTE
                    .Include(x => x.LOCAL_INSTALACAO)
                    .Include(x => x.RISCO_INVENTARIO_AMBIENTE)
                    .Include(x => x.NR_INVENTARIO_AMBIENTE)
                    .Where(x => x.LOCAL_INSTALACAO.Any(y => y.CodLocalInstalacao == li) &&
                    x.CodInventarioAmbiente != (long)Constantes.InventarioAmbiente.SEM_INVENTARIO).FirstOrDefault();

            return inventarioAmbiente;

        }

        // insert inventario ambiente
        public INVENTARIO_AMBIENTE Inserir(InventarioAmbienteModelo inventarioAmbienteModelo, DB_LaborSafetyEntities entities)
        {
            List<NrInventarioAmbienteModelo> nrs = inventarioAmbienteModelo.NR_INVENTARIO_AMBIENTE;
            List<RiscoInventarioAmbienteModelo> riscos = inventarioAmbienteModelo.RISCO_INVENTARIO_AMBIENTE;

            entities.Configuration.AutoDetectChangesEnabled = false;

            try
            {
                if (entities == null)
                    entities = new DB_LaborSafetyEntities();


                var inventario = new INVENTARIO_AMBIENTE()
                {
                    Codigo = $"INV_AMB - {inventarioAmbienteModelo.CodInventarioAmbiente}",
                    CodAmbiente = inventarioAmbienteModelo.CodAmbiente,
                    Descricao = inventarioAmbienteModelo.Descricao,
                    ObservacaoGeral = inventarioAmbienteModelo.ObservacaoGeral,
                    RiscoGeral = inventarioAmbienteModelo.RiscoGeral,
                    DataAtualizacao = DateTime.Now,
                    Ativo = true
                };

                entities.INVENTARIO_AMBIENTE.Add(inventario);
                entities.SaveChanges();

                long idInv = inventario.CodInventarioAmbiente;

                inventario.Codigo = $"INV_AMB - {idInv}";
                entities.SaveChanges();

                if (nrs != null)
                {
                    foreach (var nr in nrs)
                    {
                        entities.NR_INVENTARIO_AMBIENTE.Add(new NR_INVENTARIO_AMBIENTE()
                        {
                            CodInventarioAmbiente = idInv,
                            CodNR = nr.CodNR,
                            Ativo = true
                        });
                    }

                    entities.SaveChanges();
                }

                if (riscos != null)
                {
                    foreach (var risco in riscos)
                    {
                        var novoRisco = new RISCO_INVENTARIO_AMBIENTE()
                        {
                            CodInventarioAmbiente = idInv,
                            CodRiscoAmbiente = risco.CodRiscoAmbiente,
                            CodSeveridade = risco.CodSeveridade,
                            CodProbabilidade = risco.CodProbabilidade,
                            FonteGeradora = risco.FonteGeradora,
                            ProcedimentosAplicaveis = risco.ProcedimentosAplicaveis,
                            ContraMedidas = risco.ContraMedidas,
                            Ativo = true
                        };
                        entities.RISCO_INVENTARIO_AMBIENTE.Add(novoRisco);
                        entities.SaveChanges();

                        if (risco.EPIRiscoInventarioAmbienteModelo.Count >= 0)
                        {
                            foreach (var epi in risco.EPIRiscoInventarioAmbienteModelo)
                            {
                                entities.EPI_RISCO_INVENTARIO_AMBIENTE.Add(new EPI_RISCO_INVENTARIO_AMBIENTE()
                                {
                                    CodRiscoInventarioAmbiente = novoRisco.CodRiscoInventarioAmbiente,
                                    CodEPI = epi.CodEPI
                                });
                            }
                        }
                        entities.SaveChanges();
                    }

                }

                if (inventarioAmbienteModelo.LOCAL_INSTALACAO_MODELO != null)
                {
                    foreach (var li in inventarioAmbienteModelo.LOCAL_INSTALACAO_MODELO)
                    {
                        var localEnviado = entities.LOCAL_INSTALACAO.Where(x => x.CodLocalInstalacao == li.CodLocalInstalacao).FirstOrDefault();
                        localEnviado.CodInventarioAmbiente = idInv;
                    }

                }

                entities.ChangeTracker.DetectChanges();
                entities.SaveChanges();

                return inventario;
            }

            catch (Exception)
            {
                throw;
            }
            finally
            {
                entities.Configuration.AutoDetectChangesEnabled = true;
            }
        }

        public INVENTARIO_AMBIENTE InserirPorEdicao(InventarioAmbienteModelo inventarioAmbienteModelo, DB_LaborSafetyEntities entities)
        {
            List<NrInventarioAmbienteModelo> nrs = inventarioAmbienteModelo.NR_INVENTARIO_AMBIENTE;
            List<RiscoInventarioAmbienteModelo> riscos = inventarioAmbienteModelo.RISCO_INVENTARIO_AMBIENTE;

            try
            {
                if (entities == null)
                    entities = new DB_LaborSafetyEntities();

                entities.Configuration.AutoDetectChangesEnabled = false;

                DesativarInventarioPorCodigo(inventarioAmbienteModelo.Codigo,entities);

                var inventario = new INVENTARIO_AMBIENTE()
                {
                    Codigo = inventarioAmbienteModelo.Codigo,
                    CodAmbiente = inventarioAmbienteModelo.CodAmbiente,
                    Descricao = inventarioAmbienteModelo.Descricao,
                    ObservacaoGeral = inventarioAmbienteModelo.ObservacaoGeral,
                    RiscoGeral = inventarioAmbienteModelo.RiscoGeral,
                    DataAtualizacao = DateTime.Now,
                    Ativo = true
                };

                entities.INVENTARIO_AMBIENTE.Add(inventario);
                entities.SaveChanges();

                long idInv = inventario.CodInventarioAmbiente;

                if (nrs != null)
                {
                    foreach (var nr in nrs)
                    {
                        entities.NR_INVENTARIO_AMBIENTE.Add(new NR_INVENTARIO_AMBIENTE()
                        {
                            CodInventarioAmbiente = idInv,
                            CodNR = nr.CodNR,
                            Ativo = true
                        });
                    }

                    entities.SaveChanges();
                }

                if (riscos != null)
                {
                    foreach (var risco in riscos)
                    {
                        var novoRisco = new RISCO_INVENTARIO_AMBIENTE()
                        {
                            CodInventarioAmbiente = idInv,
                            CodRiscoAmbiente = risco.CodRiscoAmbiente,
                            CodSeveridade = risco.CodSeveridade,
                            CodProbabilidade = risco.CodProbabilidade,
                            FonteGeradora = risco.FonteGeradora,
                            ProcedimentosAplicaveis = risco.ProcedimentosAplicaveis,
                            ContraMedidas = risco.ContraMedidas,
                            Ativo = true
                        };
                        entities.RISCO_INVENTARIO_AMBIENTE.Add(novoRisco);
                        entities.SaveChanges();

                        if (risco.EPIRiscoInventarioAmbienteModelo.Count >= 0)
                        {
                            foreach (var epi in risco.EPIRiscoInventarioAmbienteModelo)
                            {
                                entities.EPI_RISCO_INVENTARIO_AMBIENTE.Add(new EPI_RISCO_INVENTARIO_AMBIENTE()
                                {
                                    CodRiscoInventarioAmbiente = novoRisco.CodRiscoInventarioAmbiente,
                                    CodEPI = epi.CodEPI
                                });
                            }
                        }
                        entities.SaveChanges();
                    }

                }

                if (inventarioAmbienteModelo.LOCAL_INSTALACAO_MODELO != null)
                {
                    foreach (var li in inventarioAmbienteModelo.LOCAL_INSTALACAO_MODELO)
                    {
                        var localEnviado = entities.LOCAL_INSTALACAO.Where(x => x.CodLocalInstalacao == li.CodLocalInstalacao).FirstOrDefault();
                        localEnviado.CodInventarioAmbiente = idInv;
                    }
                    entities.ChangeTracker.DetectChanges();
                    entities.SaveChanges();
                }

                return inventario;
            }

            catch (Exception exception)
            {
                throw exception;
            }
            finally
            {
                entities.Configuration.AutoDetectChangesEnabled = true;
            }
        }

        public List<LOCAL_INSTALACAO> BuscaFilhosPorNivelExcetoInventario(long codLocal, long idInventario, 
            DB_LaborSafetyEntities entities = null)
        {
            if (entities == null)
                entities = new DB_LaborSafetyEntities();

            entities.Database.CommandTimeout = 9999;

            var localEnviado = entities.LOCAL_INSTALACAO.Where(x => x.CodLocalInstalacao == codLocal)
                .FirstOrDefault();

            List<LOCAL_INSTALACAO> locaisFiltrados = new List<LOCAL_INSTALACAO>();

            if (String.IsNullOrEmpty(localEnviado.N2))
                locaisFiltrados = entities.LOCAL_INSTALACAO.Where(x => x.N1 == localEnviado.N1 && x.CodInventarioAmbiente != idInventario).ToList();
            else if (String.IsNullOrEmpty(localEnviado.N3))
                locaisFiltrados = entities.LOCAL_INSTALACAO.Where(x => x.N1 == localEnviado.N1 && x.N2 == localEnviado.N2
                && x.CodInventarioAmbiente != idInventario).ToList();
            else if (String.IsNullOrEmpty(localEnviado.N4))
                locaisFiltrados = entities.LOCAL_INSTALACAO.Where(x => x.N1 == localEnviado.N1 && x.N2 == localEnviado.N2 &&
                x.N3 == localEnviado.N3 && x.CodInventarioAmbiente != idInventario).ToList();
            else if (String.IsNullOrEmpty(localEnviado.N5))
                locaisFiltrados = entities.LOCAL_INSTALACAO.Where(x => x.N1 == localEnviado.N1 && x.N2 == localEnviado.N2 &&
                x.N3 == localEnviado.N3 && x.N4 == localEnviado.N4 && x.CodInventarioAmbiente != idInventario).ToList();
            else if (String.IsNullOrEmpty(localEnviado.N6))
                locaisFiltrados = entities.LOCAL_INSTALACAO.Where(x => x.N1 == localEnviado.N1 && x.N2 == localEnviado.N2
                && x.N3 == localEnviado.N3 && x.N4 == localEnviado.N4 && x.N5 == localEnviado.N5 && x.CodInventarioAmbiente != idInventario).ToList();
            else
                locaisFiltrados = entities.LOCAL_INSTALACAO.Where(x => x.N1 == localEnviado.N1 && x.N2 == localEnviado.N2
                && x.N3 == localEnviado.N3 && x.N4 == localEnviado.N4 && x.N5 == localEnviado.N5 && x.N6 == localEnviado.N6
                && x.CodInventarioAmbiente != idInventario).ToList();

            return locaisFiltrados;
        }
       
        public long ListarCodAprPorInventario(long codInventario, DB_LaborSafetyEntities entities = null)
        {
            if (entities == null)
               entities = new DB_LaborSafetyEntities();

                var resultado = (
                from inv in entities.INVENTARIO_AMBIENTE
                join li in entities.LOCAL_INSTALACAO on inv.CodInventarioAmbiente equals li.CodInventarioAmbiente
                join op in entities.OPERACAO_APR on li.CodLocalInstalacao equals op.CodLI
                join apr in entities.APR on op.CodAPR equals apr.CodAPR
                where inv.CodInventarioAmbiente == codInventario
                select apr.CodAPR).FirstOrDefault();

            return resultado;
        }

        public long ListarCodAprPorInventarioTela(long codInventario, DB_LaborSafetyEntities entities = null)
        {
            if (entities == null)
                entities = new DB_LaborSafetyEntities();

            var resultado = (
            from inv in entities.INVENTARIO_AMBIENTE
            join li in entities.LOCAL_INSTALACAO on inv.CodInventarioAmbiente equals li.CodInventarioAmbiente
            join op in entities.OPERACAO_APR on li.CodLocalInstalacao equals op.CodLI
            join apr in entities.APR on op.CodAPR equals apr.CodAPR
            where inv.CodInventarioAmbiente == codInventario && apr.Ativo
            select apr.CodAPR).FirstOrDefault();

            return resultado;
        }

        public INVENTARIO_AMBIENTE EditarInventarioAmbiente(InventarioAmbienteModelo inventarioAmbienteModelo, DB_LaborSafetyEntities entities)
        {
            INVENTARIO_AMBIENTE inventarioAmbienteExistente;

            inventarioAmbienteExistente = entities.INVENTARIO_AMBIENTE.Where(invAtv => invAtv.CodInventarioAmbiente == 
            inventarioAmbienteModelo.CodInventarioAmbiente).FirstOrDefault();

            if (inventarioAmbienteExistente == null)
                throw new KeyNotFoundException();

            if (inventarioAmbienteModelo.Ativo)
            {
                inventarioAmbienteExistente.Descricao = inventarioAmbienteModelo.Descricao;
                inventarioAmbienteExistente.ObservacaoGeral = inventarioAmbienteModelo.ObservacaoGeral;

                entities.SaveChanges();
            }

            else
            {
                    try
                    {
                        DesativarInventario(inventarioAmbienteExistente.CodInventarioAmbiente, entities);

                        inventarioAmbienteExistente = Inserir(inventarioAmbienteModelo, entities);
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
            }

            return inventarioAmbienteExistente;
        }

        public void EditarNrInventarioAmbiente(long idInventario, long idNr)
        {
            using (DB_LaborSafetyEntities entities = new DB_LaborSafetyEntities())
            {
                NR_INVENTARIO_AMBIENTE inventarioAmbienteExistente = entities.NR_INVENTARIO_AMBIENTE.Where(invAmb => invAmb.CodInventarioAmbiente == idInventario).FirstOrDefault();
                NR nrInventarioAmbienteExistente = entities.NR.FirstOrDefault(invAmb => invAmb.CodNR == idNr);

                if (inventarioAmbienteExistente == null)
                {
                    throw new KeyNotFoundException();
                }
                else if (nrInventarioAmbienteExistente == null)
                {
                    throw new KeyNotFoundException();
                }
                else
                {
                    inventarioAmbienteExistente.CodNR = idNr;
                }
                entities.SaveChanges();
            }
        }

        public void EditarRiscoInventarioAmbiente(long idInventario, long idRisco)
        {
            using (DB_LaborSafetyEntities entities = new DB_LaborSafetyEntities())
            {
                RISCO_INVENTARIO_AMBIENTE inventarioAmbienteExistente = entities.RISCO_INVENTARIO_AMBIENTE.Where(invAmb => invAmb.CodInventarioAmbiente == idInventario).FirstOrDefault();
                RISCO riscoInventarioAmbienteExistente = entities.RISCO.FirstOrDefault(invAmb => invAmb.CodRisco == idRisco);

                if (inventarioAmbienteExistente == null)
                {
                    throw new KeyNotFoundException();
                }
                else if (riscoInventarioAmbienteExistente == null)
                {
                    throw new KeyNotFoundException();
                }
                else
                {
                    inventarioAmbienteExistente.CodRiscoAmbiente = idRisco;
                }

                entities.SaveChanges();
            }
        }

        public void DesativarInventarioPorCodigo(string codigo, DB_LaborSafetyEntities entities)
        {
            if (entities == null)
            {
                entities = new DB_LaborSafetyEntities();
            }
            List<INVENTARIO_AMBIENTE> listaInventarioAmbiente = entities.INVENTARIO_AMBIENTE.Where(invAmb => invAmb.Codigo == codigo && invAmb.Ativo == true).ToList();
            if (listaInventarioAmbiente.Any() == false)
            {
                throw new KeyNotFoundException();
            }
            foreach( var inventarioAmbiente in listaInventarioAmbiente)
            {
                DesativarInventario(inventarioAmbiente.CodInventarioAmbiente, entities);
            }
        }

        public void DesativarInventario(long codInventarioExistente, DB_LaborSafetyEntities entities)
        {
            INVENTARIO_AMBIENTE inventarioAmbienteExistente = 
                entities.INVENTARIO_AMBIENTE.Where(invAmb => invAmb.CodInventarioAmbiente == codInventarioExistente && invAmb.Ativo).FirstOrDefault();
            List<RISCO_INVENTARIO_AMBIENTE> riscoExistente = 
                entities.RISCO_INVENTARIO_AMBIENTE.Where(risco => risco.CodInventarioAmbiente == codInventarioExistente && risco.Ativo).ToList();
            List<NR_INVENTARIO_AMBIENTE> nrExistente = 
                entities.NR_INVENTARIO_AMBIENTE.Where(nr => nr.CodInventarioAmbiente == codInventarioExistente && nr.Ativo).ToList();
            List<LOCAL_INSTALACAO> localExistente = 
                entities.LOCAL_INSTALACAO.Where(local => local.CodInventarioAmbiente == codInventarioExistente).ToList();

            try
            {
                if (entities == null)
                    entities = new DB_LaborSafetyEntities();

                if (inventarioAmbienteExistente == null)
                    throw new Exception("Ocorreu um erro ao listar inventário de ambiente");

                var inventarioPorApr = ListarCodAprPorInventario(codInventarioExistente, entities);

                if(inventarioPorApr > 0)
                    InserirLocalInstalacaoHistorico(localExistente, inventarioPorApr, entities);

                inventarioAmbienteExistente.Ativo = false;

                foreach (var item in riscoExistente)
                    item.Ativo = false;

                foreach (var item in nrExistente)
                    item.Ativo = false;

                foreach (var item in localExistente)
                    item.CodInventarioAmbiente = (long)Constantes.LocalInstalacao.SEM_ASSOCIACAO;

                entities.SaveChanges();

            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        private void InserirLocalInstalacaoHistorico(List<LOCAL_INSTALACAO> locais, long codAPR, DB_LaborSafetyEntities entities)
        {
            try
            {
                if (entities == null)
                    entities = new DB_LaborSafetyEntities();

                foreach (var item in locais)
                {
                    var localComOperacao = entities.OPERACAO_APR.Where(x => x.CodLI == item.CodLocalInstalacao).FirstOrDefault();

                    if(localComOperacao != null)
                    {
                        var localInstalacao = new LOCAL_INSTALACAO_INVENTARIO_AMBIENTE_HISTORICO_APR()
                        {
                            CodInventarioAmbiente = item.CodInventarioAmbiente,
                            CodLocalInstalacao = item.CodLocalInstalacao,
                            CodApr = codAPR,
                            Ativo = true
                        };
                        entities.LOCAL_INSTALACAO_INVENTARIO_AMBIENTE_HISTORICO_APR.Add(localInstalacao);
                        entities.SaveChanges();
                    }
                }

                APR apr = entities.APR.Where(x => x.CodAPR == codAPR).FirstOrDefault();
                apr.Ativo = false;
                entities.SaveChanges();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public void ExcluirInventarioAmbiente(long id)
        {
            using (DB_LaborSafetyEntities entities = new DB_LaborSafetyEntities())
            {
                entities.NR_INVENTARIO_AMBIENTE.RemoveRange(
                    entities.NR_INVENTARIO_AMBIENTE
                        .Where(nr => nr.CodInventarioAmbiente == id).ToList());

                entities.RISCO_INVENTARIO_AMBIENTE.RemoveRange(
                    entities.RISCO_INVENTARIO_AMBIENTE
                        .Where(risco => risco.CodInventarioAmbiente == id).ToList());

                var delInv = entities.INVENTARIO_AMBIENTE.Where(invAmb => invAmb.CodInventarioAmbiente == id).FirstOrDefault();

                entities.INVENTARIO_AMBIENTE.Remove(delInv);

                entities.SaveChanges();
            }
        }
    }
}