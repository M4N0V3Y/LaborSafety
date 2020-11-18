using System;
using System.Collections.Generic;
using System.Linq;
using LaborSafety.Persistencia.Interfaces;
using System.Data.Entity;
using LaborSafety.Dominio.Modelos;
using LaborSafety.Dominio.Modelos.Exportacao;
using LaborSafety.Utils.Constantes;
using static LaborSafety.Utils.Constantes.Constantes;
using System.Diagnostics;

namespace LaborSafety.Persistencia.Servicos
{
    public class AprPersistencia : IAprPersistencia
    {
        private readonly IDbLaborSafetyEntities databaseEntities;
        public AprPersistencia(IDbLaborSafetyEntities databaseEntities)
        {
            this.databaseEntities = databaseEntities;
        }

        public APR InserirApr(AprModelo aprModelo, List<LOCAL_INSTALACAO> locaisInstalacao, DB_LaborSafetyEntities entities)
        {
            try
            {
                var apr = this.Inserir(aprModelo, entities, locaisInstalacao);

                return apr;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public APR EditarApr(AprModelo aprModelo, DB_LaborSafetyEntities entities, List<LOCAL_INSTALACAO> locaisInstalacao)
        {
            try
            {
                var apr = this.Editar(aprModelo, entities, locaisInstalacao);
                return apr;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public APR Inserir(AprModelo aprModelo, DB_LaborSafetyEntities entities, List<LOCAL_INSTALACAO> locaisInstalacao)
        {
            //List<FolhaAnexoAprModelo> folhasAnexo = aprModelo.FOLHA_ANEXO_APR;
            List<OperacaoAprModelo> operacaoApr = aprModelo.OPERACAO_APR;
            List<AprovadorAprModelo> aprovadores = aprModelo.APROVADOR_APR;
            List<ExecutanteAprModelo> executores = aprModelo.EXECUTANTE_APR;
            List<PessoaModelo> pessoas = aprModelo.PESSOA;

            if (entities == null)
                entities = new DB_LaborSafetyEntities();

            try
            {
                var apr = new APR()
                {
                    CodStatusAPR = (long)Constantes.TipoCodStatusApr.IMPR,
                    NumeroSerie = "numeroSerie",
                    OrdemManutencao = aprModelo.OrdemManutencao,
                    Descricao = aprModelo.Descricao,
                    RiscoGeral = aprModelo.RiscoGeral,
                    DataAprovacao = DateTime.Now,
                    DataInicio = DateTime.Now,
                    DataEncerramento = DateTime.Now,
                    Ativo = true
                };

                entities.APR.Add(apr);
                entities.SaveChanges();

                long id = apr.CodAPR;

                var numeroSerie = Constantes.GerarNumeroSerie(id, true);

                apr.NumeroSerie = numeroSerie;
                entities.SaveChanges();

                if (operacaoApr != null)
                {
                    int cont = 1;

                    //Insere os riscos de inv de ambiente uma unica vez 
                    InserirRiscosAmbienteAPR(apr.CodAPR, operacaoApr.First().CodLI.Value, entities);

                    foreach (var atividade in operacaoApr)
                    {
                        entities.OPERACAO_APR.Add(new OPERACAO_APR()
                        {
                            CodAPR = id,
                            CodStatusAPR = atividade.CodStatusAPR,
                            Codigo = $"Operação {cont}",
                            Descricao = atividade.Descricao,
                            CodLI = atividade.CodLI,
                            CodDisciplina = atividade.CodDisciplina,
                            CodAtvPadrao = atividade.CodAtvPadrao,
                            Ativo = true
                        });
                        InserirRiscosAtividadeAPR(id, atividade.CodLI.Value, atividade.CodDisciplina.Value, atividade.CodAtvPadrao.Value, entities);
                        cont++;
                    }
                }

                foreach (var aprovador in aprovadores)
                    InsereEditaPessoa(entities, pessoas, true, id, aprovador.CodPessoa);

                foreach (var executor in executores)
                    InsereEditaPessoa(entities, pessoas, false, id, executor.CodPessoa);

                return apr;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public APR InserirSomenteComNumeroSeriaViaSAP(DB_LaborSafetyEntities entities = null)
        {
            try
            {
                if (entities == null)
                    entities = new DB_LaborSafetyEntities();

                var apr = new APR()
                {
                    CodStatusAPR = (long)Constantes.TipoCodStatusApr.CRI,
                    NumeroSerie = "numeroSerie",
                    OrdemManutencao = "",
                    Descricao = "",
                    RiscoGeral = 0,
                    DataAprovacao = DateTime.Now,
                    DataInicio = DateTime.Now,
                    DataEncerramento = DateTime.Now,
                    Ativo = true
                };

                entities.APR.Add(apr);
                entities.SaveChanges();

                long id = apr.CodAPR;

                var numeroSerie = Constantes.GerarNumeroSerie(id, false);

                apr.NumeroSerie = numeroSerie;
                entities.SaveChanges();

                return apr;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public APR InserirSomenteComNumeroSerie(bool origemTela = true, DB_LaborSafetyEntities entities = null)
        {
            try
            {
                if (entities == null)
                    entities = new DB_LaborSafetyEntities();

                var apr = new APR()
                {
                    CodStatusAPR = (long)Constantes.TipoCodStatusApr.LIB,
                    NumeroSerie = "numeroSerie",
                    OrdemManutencao = "",
                    Descricao = "",
                    RiscoGeral = 0,
                    DataAprovacao = DateTime.Now,
                    DataInicio = DateTime.Now,
                    DataEncerramento = DateTime.Now,
                    Ativo = true
                };

                entities.APR.Add(apr);
                entities.SaveChanges();

                long id = apr.CodAPR;

                var numeroSerie = Constantes.GerarNumeroSerie(id, origemTela);

                apr.NumeroSerie = numeroSerie;
                entities.SaveChanges();

                return apr;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public APR Editar(AprModelo aprModelo, DB_LaborSafetyEntities entities, List<LOCAL_INSTALACAO> locaisInstalacao)
        {
            List<OperacaoAprModelo> operacaoApr = aprModelo.OPERACAO_APR;
            List<AprovadorAprModelo> aprovadores = aprModelo.APROVADOR_APR;
            List<ExecutanteAprModelo> executores = aprModelo.EXECUTANTE_APR;

            try
            {
                var aprExistente = entities.APR.Where(x => x.CodAPR == aprModelo.CodAPR).FirstOrDefault();

                var historicoAprInventarioAmbiente = entities.LOCAL_INSTALACAO_INVENTARIO_AMBIENTE_HISTORICO_APR
                    .Where(apr => apr.CodApr == aprModelo.CodAPR).FirstOrDefault();

                var historicoAprInventarioAtividade = entities.LOCAL_INSTALACAO_INVENTARIO_ATIVIDADE_HISTORICO_APR
                    .Where(apr => apr.CodApr == aprModelo.CodAPR).FirstOrDefault();

                if (historicoAprInventarioAmbiente != null)
                {
                    historicoAprInventarioAmbiente.Ativo = false;
                    entities.SaveChanges();
                }

                if (historicoAprInventarioAtividade != null)
                {
                    historicoAprInventarioAtividade.Ativo = false;
                    entities.SaveChanges();
                }

                if(aprExistente.NumeroSerie.Contains("M"))
                {
                    aprExistente.OrdemManutencao = aprModelo.OrdemManutencao;
                }
                aprExistente.Descricao = aprModelo.Descricao;
                aprExistente.RiscoGeral = aprModelo.RiscoGeral;
                aprExistente.Ativo = true;

                entities.SaveChanges();

                long id = aprExistente.CodAPR;

                if (operacaoApr.Count > 0)
                {
                    var operacaoExistente = entities.OPERACAO_APR.Where(x => x.CodAPR == aprExistente.CodAPR).ToList();
                    var riscoExistente = entities.RISCO_APR.Where(x => x.CodAPR == aprExistente.CodAPR).ToList();

                    foreach (var itemOperacaoExistente in operacaoExistente)
                        itemOperacaoExistente.Ativo = false;

                    foreach (var itemRiscoExistente in riscoExistente)
                        itemRiscoExistente.Ativo = false;

                    //Insere os riscos de inv de ambiente uma unica vez 
                    InserirRiscosAmbienteAPR(id, operacaoApr.First().CodLI.Value, entities);

                    foreach (var atividade in operacaoApr)
                    {
                        OPERACAO_APR operacaoAPR = new OPERACAO_APR();
                        operacaoAPR.CodAPR = id;
                        operacaoAPR.CodStatusAPR = atividade.CodStatusAPR;
                        operacaoAPR.Descricao = atividade.Descricao;
                        operacaoAPR.Codigo = atividade.Codigo;
                        operacaoAPR.CodLI = atividade.CodLI;
                        operacaoAPR.CodDisciplina = atividade.CodDisciplina;
                        operacaoAPR.CodAtvPadrao = atividade.CodAtvPadrao;
                        operacaoAPR.Ativo = true;

                        entities.OPERACAO_APR.Add(operacaoAPR);

                        entities.SaveChanges();
                        InserirRiscosAtividadeAPR(id, (long)operacaoAPR.CodLI, (long)operacaoAPR.CodDisciplina, (long)operacaoAPR.CodAtvPadrao, entities);
                    }
                }

                if (aprovadores.Count > 0)
                {
                    var aprovadorExistente = entities.APROVADOR_APR.Where(x => x.CodAPR == aprExistente.CodAPR).ToList();

                    foreach (var itemAprovadorExistente in aprovadorExistente)
                    {
                        itemAprovadorExistente.Ativo = false;
                    }

                    foreach (var aprovador in aprovadores)
                    {
                        APROVADOR_APR aprovadorAPR = new APROVADOR_APR();
                        aprovadorAPR.CodAPR = id;
                        aprovadorAPR.CodPessoa = aprovador.CodPessoa;
                        aprovadorAPR.Ativo = true;

                        entities.APROVADOR_APR.Add(aprovadorAPR);
                    }
                    entities.SaveChanges();
                }

                if (executores.Count > 0)
                {
                    var executanteExistente = entities.EXECUTANTE_APR.Where(x => x.CodAPR == aprExistente.CodAPR).ToList();

                    foreach (var itemExecutanteExistente in executanteExistente)
                    {
                        itemExecutanteExistente.Ativo = false;
                    }

                    foreach (var executante in executores)
                    {
                        EXECUTANTE_APR executanteAPR = new EXECUTANTE_APR();
                        executanteAPR.CodAPR = id;
                        executanteAPR.CodPessoa = executante.CodPessoa;
                        executanteAPR.Ativo = true;

                        entities.EXECUTANTE_APR.Add(executanteAPR);
                    }

                    entities.SaveChanges();
                }

                return aprExistente;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void EditarStatusApr(string codOrdemManutencao,long novoCodStatusApr , DB_LaborSafetyEntities entities = null)
        {
            if (entities == null)
                entities = new DB_LaborSafetyEntities();

            var apr = entities.APR.Where(x => x.OrdemManutencao == codOrdemManutencao && x.Ativo).FirstOrDefault();
            if (apr == null)
            {
                throw new KeyNotFoundException();
            }
            apr.CodStatusAPR = novoCodStatusApr;
            entities.SaveChanges();

        }

        public void InsereEditaPessoa(DB_LaborSafetyEntities entities, List<PessoaModelo> pessoas, bool eAprovador, long id, long idPessoa)
        {
            ExecutanteAprModelo executanteAprModelo = new ExecutanteAprModelo();
            AprovadorAprModelo aprovadorAprModelo = new AprovadorAprModelo();

                    if (eAprovador)
                    {
                        entities.APROVADOR_APR.Add(new APROVADOR_APR()
                        {
                            CodAPR = id,
                            CodAprovadorAPR = aprovadorAprModelo.CodAprovadorApr,
                            CodPessoa = idPessoa,
                            Ativo = true
                        });
                        entities.SaveChanges();
                    }
                    else
                    {
                        entities.EXECUTANTE_APR.Add(new EXECUTANTE_APR()
                        {
                            CodAPR = id,
                            CodExecutanteAPR = executanteAprModelo.CodExecutanteApr,
                            CodPessoa = idPessoa,
                            Ativo = true
                        });
                        entities.SaveChanges();
                    }
        }

        public void InserirCabecalho(AprModelo aprModelo)
        {
            using (var entities = new DB_LaborSafetyEntities())
            {
                try
                {
                    var apr = new APR()
                    {
                        NumeroSerie = aprModelo.NumeroSerie,
                        OrdemManutencao = aprModelo.OrdemManutencao,
                        DataAprovacao = aprModelo.DataAprovacao,
                        DataInicio = aprModelo.DataInicio,
                        DataEncerramento = aprModelo.DataEncerramento,
                        Ativo = true
                    };

                    entities.APR.Add(apr);
                    entities.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public void InserirPessoa(PessoaModelo pessoas)
        {
            using (var entities = new DB_LaborSafetyEntities())
            {
                    entities.PESSOA.Add(new PESSOA()
                    {
                        Matricula = pessoas.Matricula,
                        Nome = pessoas.Nome,
                        CPF = pessoas.CPF,
                        Telefone = pessoas.Telefone,
                        Email = pessoas.Email,
                        Empresa = pessoas.Empresa,
                        Ativo = true
                    });
                entities.SaveChanges();
            }
        }

        public void InserirPessoaGeral(DB_LaborSafetyEntities entities, PessoaModelo pessoas)
        {
            using (entities = new DB_LaborSafetyEntities())
            {
                entities.PESSOA.Add(new PESSOA()
                {
                    Matricula = pessoas.Matricula,
                    Nome = pessoas.Nome,
                    CPF = pessoas.CPF,
                    Telefone = pessoas.Telefone,
                    Email = pessoas.Email,
                    Empresa = pessoas.Empresa,
                    Ativo = true
                });
                entities.SaveChanges();
            }
        }

        public void InserirListaPessoa(List<PessoaModelo> pessoas)
        {
            using (var entities = new DB_LaborSafetyEntities())
            {
                foreach (var pessoa in pessoas)
                {
                    entities.PESSOA.Add(new PESSOA()
                    {
                        Matricula = pessoa.Matricula,
                        Nome = pessoa.Nome,
                        CPF = pessoa.CPF,
                        Telefone = pessoa.Telefone,
                        Email = pessoa.Email,
                        Empresa = pessoa.Empresa,
                        Ativo = true
                    });
                }
                entities.SaveChanges();
            }
        }

        public void InserirAprovadores(List<AprovadorAprModelo> aprovadores)
        {
            using (var entities = new DB_LaborSafetyEntities())
            {
                APR apr = entities.APR.Where(x => x.CodAPR == aprovadores[0].CodApr).FirstOrDefault();

                if (apr == null)
                    throw new KeyNotFoundException();

                long codAPR = apr.CodAPR;

                foreach (var aprovador in aprovadores)
                {
                    entities.APROVADOR_APR.Add(new APROVADOR_APR()
                    {
                        CodAPR = codAPR,
                        CodAprovadorAPR = aprovador.CodAprovadorApr,
                        Ativo = true
                    });
                }
                entities.SaveChanges();
            }
        }

        public void InserirExecutores(List<ExecutanteAprModelo> executores)
        {
            using (var entities = new DB_LaborSafetyEntities())
            {
                APR apr = entities.APR.Where(x => x.CodAPR == executores[0].CodApr).FirstOrDefault();

                if (apr == null)
                    throw new KeyNotFoundException();

                long codAPR = apr.CodAPR;

                foreach (var executor in executores)
                {
                    entities.EXECUTANTE_APR.Add(new EXECUTANTE_APR()
                    {
                        CodAPR = codAPR,
                        CodExecutanteAPR = executor.CodExecutanteApr,
                        Ativo = true
                    });
                }
                entities.SaveChanges();
            }
        }


        public void InserirRisco(List<RiscoAprModelo> riscoApr)
        {
            using (var entities = new DB_LaborSafetyEntities())
            {
                APR apr = entities.APR.Where(x => x.CodAPR == riscoApr[0].CodApr).FirstOrDefault();

                long codAPR = apr.CodAPR;

                foreach (var risco in riscoApr)
                {
                    entities.RISCO_APR.Add(new RISCO_APR()
                    {
                        CodAPR = codAPR,
                        CodRisco = risco.CodRisco,
                        Ativo = true
                    });
                }
                entities.SaveChanges();
            }
        }

        public void InserirRiscosAmbienteAPR(long idApr, long idLocal, DB_LaborSafetyEntities entities)
        {
            List<long> riscosApr = new List<long>();

            var riscosAmbiente = (from risco in entities.RISCO
                                  join rinv in entities.RISCO_INVENTARIO_AMBIENTE on risco.CodRisco equals rinv.CodRiscoAmbiente
                                  join inv in entities.INVENTARIO_AMBIENTE on rinv.CodInventarioAmbiente equals inv.CodInventarioAmbiente
                                  join li in entities.LOCAL_INSTALACAO on inv.CodInventarioAmbiente equals li.CodInventarioAmbiente
                                  where li.CodLocalInstalacao == idLocal
                                  select risco).ToList();

            foreach (var risco in riscosAmbiente)
                riscosApr.Add(risco.CodRisco);

            foreach (var item in riscosApr)
            {
                entities.RISCO_APR.Add(new RISCO_APR()
                {
                    CodAPR = idApr,
                    CodRisco = item,
                    Ativo = true
                });
            }
            entities.SaveChanges();
        }

        public void InserirRiscosAtividadeAPR(long idApr, long idLocal, long idDisciplina, long idAtividade, DB_LaborSafetyEntities entities)
        {
            List<long> riscosApr = new List<long>(); 

            var riscosAtividade = (from risco in entities.RISCO
                                   join ratv in entities.RISCO_INVENTARIO_ATIVIDADE on risco.CodRisco equals ratv.CodRisco
                                   join invatv in entities.INVENTARIO_ATIVIDADE on ratv.CodInventarioAtividade equals invatv.CodInventarioAtividade
                                   join liInvAtv in entities.LOCAL_INSTALACAO_INVENTARIO_ATIVIDADE on invatv.CodInventarioAtividade equals liInvAtv.CodInventarioAtividade
                                   join li in entities.LOCAL_INSTALACAO on liInvAtv.CodLocalInstalacao equals li.CodLocalInstalacao
                                   where li.CodLocalInstalacao == idLocal && invatv.CodDisciplina == idDisciplina && invatv.CodAtividade == idAtividade
                                   select risco).ToList();

            foreach (var risco in riscosAtividade)
                riscosApr.Add(risco.CodRisco);

            foreach (var item in riscosApr)
            {
                entities.RISCO_APR.Add(new RISCO_APR()
                {
                    CodAPR = idApr,
                    CodRisco = item,
                    Ativo = true
                });
            }
            entities.SaveChanges();
        }

        public void InserirAtividadeOperacao(List<OperacaoAprModelo> atividadeOperacao)
        {
            using (var entities = new DB_LaborSafetyEntities())
            {
                APR apr = entities.APR.Where(x => x.CodAPR == atividadeOperacao[0].CodAPR).FirstOrDefault();

                if (apr == null)
                    throw new KeyNotFoundException();

                long codAPR = apr.CodAPR;

                foreach (var atividade in atividadeOperacao)
                {
                    entities.OPERACAO_APR.Add(new OPERACAO_APR()
                    {
                        CodAPR = codAPR,
                        CodStatusAPR = atividade.CodStatusAPR,
                        Codigo = atividade.Codigo,
                        Descricao = atividade.Descricao,
                        CodLI = atividade.CodLI,
                        CodDisciplina = atividade.CodDisciplina,
                        CodAtvPadrao = atividade.CodAtvPadrao,
                        Ativo = true
                    });
                }
                entities.SaveChanges();
            }
        }

        public void EditarRiscos(List<RiscoAprModelo> riscosAPR)
        {
            using (DB_LaborSafetyEntities entities = new DB_LaborSafetyEntities())
            {
                long codAPR = riscosAPR[0].CodApr;

                entities.RISCO_APR.RemoveRange(entities.RISCO_APR.Where(apr => apr.CodAPR == codAPR).ToList());

                foreach (var risco in riscosAPR)
                {
                    RISCO_APR riscoAPR = new RISCO_APR();
                    riscoAPR.CodAPR = risco.CodApr;
                    riscoAPR.CodRisco = risco.CodRisco;
                    riscoAPR.CodRiscoAPR = risco.CodApr;
                    riscoAPR.Ativo = true;

                    entities.RISCO_APR.Add(riscoAPR);
                }

                entities.SaveChanges();
            }
        }

        public void EditarAtividades(List<OperacaoAprModelo> atividadeOperacao)
        {
            using (DB_LaborSafetyEntities entities = new DB_LaborSafetyEntities())
            {
                long codAPR = atividadeOperacao[0].CodAPR;

                entities.OPERACAO_APR.RemoveRange(entities.OPERACAO_APR.Where(apr => apr.CodAPR == codAPR).ToList());

                foreach (var atividade in atividadeOperacao)
                {
                    OPERACAO_APR operacaoAPR = new OPERACAO_APR();
                    operacaoAPR.CodAPR = atividade.CodAPR;
                    operacaoAPR.CodStatusAPR = atividade.CodStatusAPR;
                    operacaoAPR.Codigo = atividade.Codigo;
                    operacaoAPR.Descricao = atividade.Descricao;
                    operacaoAPR.CodLI = atividade.CodLI;
                    operacaoAPR.CodDisciplina = atividade.CodDisciplina;
                    operacaoAPR.CodAtvPadrao = atividade.CodAtvPadrao;
                    operacaoAPR.Ativo = true;

                    entities.OPERACAO_APR.Add(operacaoAPR);
                }

                entities.SaveChanges();
            }
        }

        //public void EditarFolhasAnexo(List<FolhaAnexoAprModelo> folhaAnexo)
        //{
        //    using (DB_LaborSafetyEntities entities = new DB_LaborSafetyEntities())
        //    {
        //        long codAPR = folhaAnexo[0].CodApr;

        //        entities.FOLHA_ANEXO_APR.RemoveRange(entities.FOLHA_ANEXO_APR.Where(apr => apr.CodAPR == codAPR).ToList());

        //        foreach (var itemFolha in folhaAnexo)
        //        {
        //            FOLHA_ANEXO_APR folhaApr = new FOLHA_ANEXO_APR();
        //            folhaApr.CodAPR = itemFolha.CodApr;
        //            folhaApr.CodAnexo = itemFolha.CodAnexo;
        //            folhaApr.Ativo = true;

        //            entities.FOLHA_ANEXO_APR.Add(folhaApr);
        //        }

        //        entities.SaveChanges();
        //    }
        //}

        public void EditarExecutores (List<ExecutanteAprModelo> executantes)
        {
            using (DB_LaborSafetyEntities entities = new DB_LaborSafetyEntities())
            {
                long codAPR = executantes[0].CodApr;

                entities.EXECUTANTE_APR.RemoveRange(entities.EXECUTANTE_APR.Where(apr => apr.CodAPR == codAPR).ToList());

                foreach (var executante in executantes)
                {
                    EXECUTANTE_APR executanteAPR = new EXECUTANTE_APR();
                    executanteAPR.CodAPR = executante.CodApr;
                    executanteAPR.CodPessoa = executante.CodPessoa;
                    executanteAPR.Ativo = true;

                    entities.EXECUTANTE_APR.Add(executanteAPR);
                }

                entities.SaveChanges();
            }
        }

        public void EditarAprovadores(List<AprovadorAprModelo> aprovadores)
        {
            using (DB_LaborSafetyEntities entities = new DB_LaborSafetyEntities())
            {
                long codAPR = aprovadores[0].CodApr;

                entities.APROVADOR_APR.RemoveRange(entities.APROVADOR_APR.Where(apr => apr.CodAPR == codAPR).ToList());

                foreach (var aprovador in aprovadores)
                {
                    APROVADOR_APR aprovadorAPR = new APROVADOR_APR();
                    aprovadorAPR.CodAPR = aprovador.CodApr;
                    aprovadorAPR.CodPessoa = aprovador.CodPessoa;
                    aprovadorAPR.Ativo = true;

                    entities.APROVADOR_APR.Add(aprovadorAPR);
                }

                entities.SaveChanges();
            }
        }

        public APR PesquisarPorNumeroSerie(string numeroSerie)
        {
            var entities = new DB_LaborSafetyEntities();
                APR apr = entities.APR
                    .Where(x => x.NumeroSerie == numeroSerie)
                    .FirstOrDefault();

            if (apr == null)
                throw new Exception($"APR de número de série {numeroSerie} não possui status liberado ou não existe na base de dados.");

                return apr;
        }

        public APR PesquisarPorOrdemManutencao(string ordemManutencao, DB_LaborSafetyEntities entities = null)
        {
            if (entities == null)
                entities = new DB_LaborSafetyEntities();

            APR apr = entities.APR
                .Where(x => x.OrdemManutencao == ordemManutencao)
                .Include(x => x.RISCO_APR)
                .Include(x => x.OPERACAO_APR)
                .Include(x => x.APROVADOR_APR)
                .Include(x => x.EXECUTANTE_APR)
                .FirstOrDefault();

            if (apr == null)
            {
                throw new Exception("Não existem APRs com status liberado!");
            }
            return apr;
        }

        public bool ValidarExistenciaOrdemManutencao(string ordemManutencao, long codApr)
        {
            bool ordemExistente = false;
            using (var entities = new DB_LaborSafetyEntities())
            {
                APR apr;

                if (codApr == 0)
                {
                    apr = entities.APR
                        .Where(x => x.OrdemManutencao == ordemManutencao)
                        .FirstOrDefault();

                    if (apr != null)
                    {
                        ordemExistente = true;
                    }
                }
                else
                {
                    apr = entities.APR
                            .Where(x => x.OrdemManutencao == ordemManutencao && x.CodAPR != codApr)
                            .FirstOrDefault();

                    if (apr != null)
                    {
                        ordemExistente = true;
                    }
                }

                return ordemExistente;
            }
        }

        public APR PesquisarPorOrdemManutencaoExistentesEInexistentes(string ordemManutencao, DB_LaborSafetyEntities entities = null)
        {
            if (entities == null)
                entities = new DB_LaborSafetyEntities();

            APR apr = entities.APR
                .Where(x => x.OrdemManutencao == ordemManutencao && x.CodStatusAPR != (long)TipoCodStatusApr.LIB && x.Ativo)
                .Include(x => x.RISCO_APR)
                .Include(x => x.OPERACAO_APR)
                .Include(x => x.APROVADOR_APR)
                .Include(x => x.EXECUTANTE_APR)
                .FirstOrDefault();
            return apr;
        }

        public APR PesquisarPorId(long idApr)
        {
            using (var entities = new DB_LaborSafetyEntities())
            {
                var apr = entities.APR
                    .Include(x => x.RISCO_APR)
                    .Include(x => x.OPERACAO_APR)
                    .Include(x => x.APROVADOR_APR)
                    .Include(x => x.EXECUTANTE_APR)
                    .Include(x => x.STATUS_APR)
                 .Where(x => x.CodAPR == idApr)
                 .FirstOrDefault();

                return apr;
            }
        }

        public APR PesquisarPorIdAnteriorAEdicao(long idApr)
        {
            using (var entities = new DB_LaborSafetyEntities())
            {
                var apr = entities.APR
                    .Include(x => x.RISCO_APR)
                    .Include(x => x.OPERACAO_APR)
                    .Include(x => x.APROVADOR_APR)
                    .Include(x => x.EXECUTANTE_APR)
                    .Include(x => x.STATUS_APR)
                 .Where(x => x.CodAPR == idApr && x.Ativo)
                 .FirstOrDefault();

                return apr;
            }
        }

        public LOCAL_INSTALACAO_INVENTARIO_AMBIENTE_HISTORICO_APR PesquisarAprInventarioAmbiente(long codLI, long codApr)
        {
            using (var entities = new DB_LaborSafetyEntities())
            {
                var apr = entities.LOCAL_INSTALACAO_INVENTARIO_AMBIENTE_HISTORICO_APR
                 .Where(x => x.CodLocalInstalacao == codLI && x.CodApr == codApr && x.Ativo)
                 .FirstOrDefault();

                return apr;
            }
        }

        public List<LOCAL_INSTALACAO_INVENTARIO_ATIVIDADE_HISTORICO_APR> PesquisarAprInventarioAtividade(long codLI, long codApr)
        {
            using (var entities = new DB_LaborSafetyEntities())
            {
                var apr = entities.LOCAL_INSTALACAO_INVENTARIO_ATIVIDADE_HISTORICO_APR
                 .Where(x => x.CodLocalInstalacao == codLI && x.CodApr == codApr && x.Ativo)
                 .ToList();

                return apr;
            }
        }

        public List<APR> PesquisarPorRisco(long codRisco)
        {
            using (var entities = new DB_LaborSafetyEntities())
            {
                var apr = entities.APR
                    .Include(x => x.RISCO_APR)
                    .Include(x => x.OPERACAO_APR)
                    .Include(x => x.APROVADOR_APR)
                    .Include(x => x.EXECUTANTE_APR)
                 .Where(x => x.RISCO_APR.Any(i => i.CodRisco == codRisco) && x.Ativo)
                 .ToList();

                return apr;
            }
        }

        public List<APR> ListarTodasAPRs()
        {
            using (var entities = new DB_LaborSafetyEntities())
            {
                var apr = entities.APR
                    .Include(x => x.RISCO_APR)
                    .Include(x => x.OPERACAO_APR)
                    .Include(x => x.APROVADOR_APR)
                    .Include(x => x.EXECUTANTE_APR)
                 .ToList();

                return apr;
            }
        }

        public List<APR> PesquisarPorLocalInstalacao(long codLocalInstalacao)
        {
            using (var entities = new DB_LaborSafetyEntities())
            {
                var res2 = (from apr in entities.APR
                            join rapr in entities.RISCO_APR on apr.CodAPR equals rapr.CodAPR
                            join aop in entities.OPERACAO_APR on apr.CodAPR equals aop.CodAPR
                            join aprov in entities.APROVADOR_APR on apr.CodAPR equals aprov.CodAPR
                            join exe in entities.EXECUTANTE_APR on apr.CodAPR equals exe.CodAPR
                            join risco in entities.RISCO on rapr.CodRisco equals risco.CodRisco
                            join rinv in entities.RISCO_INVENTARIO_AMBIENTE on risco.CodRisco equals rinv.CodRiscoAmbiente
                            join inv in entities.INVENTARIO_AMBIENTE on rinv.CodInventarioAmbiente equals inv.CodInventarioAmbiente
                            join li in entities.LOCAL_INSTALACAO on inv.CodInventarioAmbiente equals li.CodInventarioAmbiente
                            where li.CodLocalInstalacao == codLocalInstalacao && apr.Ativo select apr).ToList();

                return res2;
            }
        }

        public List<APR> PesquisarPorStatusOk()
        {
            using (var entities = new DB_LaborSafetyEntities())
            {
                var apr = entities.APR
                .Include(x => x.OPERACAO_APR)
                 .Where(x => x.CodStatusAPR == 1 && x.Ativo)
                 .ToList();

                return apr;
            }
        }

        public List<LOCAL_INSTALACAO> BuscaFilhosPorNivel(long codLocal, DB_LaborSafetyEntities entities = null)
        {
            if (entities == null)
                entities = new DB_LaborSafetyEntities();

            entities.Database.CommandTimeout = 9999;

            var localEnviado = entities.LOCAL_INSTALACAO.Where(x => x.CodLocalInstalacao == codLocal).FirstOrDefault();

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

        public List<APR> ListarApr(FiltroAprModelo filtroAprModelo)
        {
            using (var entities = new DB_LaborSafetyEntities())
            {
                var sw = new Stopwatch();

                sw.Start();

                var resultado = entities.APR
                .Include(x => x.RISCO_APR)
                .Include(x => x.OPERACAO_APR)
                .Include(x => x.APROVADOR_APR)
                .Include(x => x.EXECUTANTE_APR)
                .Include(x => x.RISCO_APR.Select(a => a.RISCO))
                .Include(x => x.RISCO_APR.Select(a => a.RISCO).Select(a => a.RISCO_INVENTARIO_AMBIENTE))
                .Include(x => x.RISCO_APR.Select(a => a.RISCO).Select(a => a.RISCO_INVENTARIO_ATIVIDADE))
                .Include(x => x.RISCO_APR.Select(a => a.RISCO).Select(a => a.RISCO_INVENTARIO_AMBIENTE.Select(b => b.INVENTARIO_AMBIENTE)))
                .Include(x => x.RISCO_APR.Select(a => a.RISCO).Select(a => a.RISCO_INVENTARIO_ATIVIDADE.Select(b => b.INVENTARIO_ATIVIDADE)))
                .Include(x => x.RISCO_APR.Select(a => a.RISCO).Select(a => a.RISCO_INVENTARIO_AMBIENTE.Select(b => b.INVENTARIO_AMBIENTE.LOCAL_INSTALACAO)))
                .Include(x => x.RISCO_APR.Select(a => a.RISCO).Select(a => a.RISCO_INVENTARIO_ATIVIDADE.Select
                                                                          (b => b.INVENTARIO_ATIVIDADE.LOCAL_INSTALACAO_INVENTARIO_ATIVIDADE)));

                if (!string.IsNullOrEmpty(filtroAprModelo.NumeroSerie))
                    resultado = resultado.Where(x => x.NumeroSerie == filtroAprModelo.NumeroSerie);

                if (!string.IsNullOrEmpty(filtroAprModelo.OrdemManutencao))
                    resultado = resultado.Where(x => x.OrdemManutencao == filtroAprModelo.OrdemManutencao);

                var resultadoFinal = resultado.ToList();

                sw.Stop();
                var tempo = sw.ElapsedMilliseconds / 1000;

                if (filtroAprModelo.CodLocalInstalacao != null && filtroAprModelo.CodLocalInstalacao.Count > 0)
                {
                    List<LOCAL_INSTALACAO> locaisAPesquisar = new List<LOCAL_INSTALACAO>();
                    LOCAL_INSTALACAO_INVENTARIO_AMBIENTE_HISTORICO_APR localHistorico = new LOCAL_INSTALACAO_INVENTARIO_AMBIENTE_HISTORICO_APR();
                    LOCAL_INSTALACAO_INVENTARIO_ATIVIDADE_HISTORICO_APR localHistoricoAtividade = new LOCAL_INSTALACAO_INVENTARIO_ATIVIDADE_HISTORICO_APR();
                    LOCAL_INSTALACAO local = new LOCAL_INSTALACAO();
                    List<long> CodLocais = new List<long>();
                    foreach (var item in filtroAprModelo.CodLocalInstalacao)
                    {
                        localHistoricoAtividade = entities.LOCAL_INSTALACAO_INVENTARIO_ATIVIDADE_HISTORICO_APR.
                            Where(lc => lc.CodLocalInstalacao == item && lc.Ativo).FirstOrDefault();

                        if (localHistoricoAtividade != null)
                            CodLocais.Add(localHistoricoAtividade.CodLocalInstalacao);

                        else
                        {
                            local = entities.LOCAL_INSTALACAO.Where(lc => lc.CodLocalInstalacao == item).FirstOrDefault();

                            if (local != null)
                                CodLocais.Add(local.CodLocalInstalacao);

                            else
                                throw new Exception($"O local {item} não foi encontrado na base de dados!");
                        }
                    }

                    //Busca filhos dos locais
                    foreach (var novoLocal in CodLocais)
                    {
                        List<LOCAL_INSTALACAO> locaisEFilhos = BuscaFilhosPorNivel(novoLocal, entities);
                        locaisAPesquisar.AddRange(locaisEFilhos);
                    }

                    List<long> buscarLocais = new List<long>();
                    foreach (var pesquisaLocal in locaisAPesquisar)
                        buscarLocais.Add(pesquisaLocal.CodLocalInstalacao);

                    resultadoFinal = resultadoFinal.Where(x => x.RISCO_APR.Select(a => a.RISCO).
                    Any(a => a.RISCO_INVENTARIO_ATIVIDADE.Any(b => b.INVENTARIO_ATIVIDADE.LOCAL_INSTALACAO_INVENTARIO_ATIVIDADE.
                    Any(u => buscarLocais.Contains(u.CodLocalInstalacao))))).ToList();

                    //List<string> NomeLocais = new List<string>();
                    //Busca todos os locais
                    //List<LOCAL_INSTALACAO> locais = entities.LOCAL_INSTALACAO
                    //     .Where(x => x.CodLocalInstalacao != (long)Constantes.LocalInstalacao.SEM_ASSOCIACAO)
                    //     .ToList();

                    foreach (var item in filtroAprModelo.CodLocalInstalacao)
                    {
                        localHistorico = entities.LOCAL_INSTALACAO_INVENTARIO_AMBIENTE_HISTORICO_APR.Where(lc => lc.CodLocalInstalacao == item && lc.Ativo).FirstOrDefault();

                        if (localHistorico != null)
                        {
                            CodLocais.Add(localHistorico.CodLocalInstalacao);
                            //var nomeLocalBD = entities.LOCAL_INSTALACAO.Where(x => x.CodLocalInstalacao == localHistorico.CodLocalInstalacao).FirstOrDefault();
                            //NomeLocais.Add(nomeLocalBD.Nome);
                        }

                        else
                        {
                            local = entities.LOCAL_INSTALACAO.Where(lc => lc.CodLocalInstalacao == item).FirstOrDefault();

                            if (local != null)
                                CodLocais.Add(local.CodLocalInstalacao);

                            else
                                throw new Exception($"O local {item} não foi encontrado na base de dados!");
                        }
                    }

                    //Busca filhos dos locais
                    foreach (var novoLocal in CodLocais)
                    {
                        var li = entities.LOCAL_INSTALACAO.Where(x => x.CodLocalInstalacao == novoLocal).FirstOrDefault();

                        //Filtra somente os locais do pai
                        List<LOCAL_INSTALACAO> locaisFilhos = this.BuscaLocaisEFilhos(entities, li);

                        locaisAPesquisar.AddRange(locaisFilhos);
                    }

                    List<long> buscarLocaisAmbiente = new List<long>();

                    foreach (var pesquisaLocal in locaisAPesquisar)
                        buscarLocaisAmbiente.Add(pesquisaLocal.CodLocalInstalacao);

                    resultadoFinal = resultadoFinal.Where(x => x.RISCO_APR.Select(a => a.RISCO).
                    Any(a => a.RISCO_INVENTARIO_AMBIENTE.Any(y => y.INVENTARIO_AMBIENTE.LOCAL_INSTALACAO.
                    Any(j => buscarLocaisAmbiente.Contains(j.CodLocalInstalacao))))).ToList();
                }

                return resultadoFinal;
            }
        }

        private List<LOCAL_INSTALACAO> BuscaLocaisEFilhos(DB_LaborSafetyEntities entities, LOCAL_INSTALACAO localEnviado)
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
        public List<APR> ListarAprExportacao(DadosExportacaoAprModelo dados)
        {
            using (var entities = new DB_LaborSafetyEntities())
            {
                List<APR> resultadoLocal = new List<APR>();
                List<APR> resultadoRiscoGeral = new List<APR>();
                List<APR> resultadoRiscoApr = new List<APR>();
                List<APR> resultadoFinal = new List<APR>();

                var resultado = entities.APR
                    .Include(x => x.RISCO_APR)
                    .Include(x => x.OPERACAO_APR)
                    //.Include(x => x.APROVADOR_APR)
                    //.Include(x => x.EXECUTANTE_APR)
                    .Include(x => x.RISCO_APR.Select(a => a.RISCO))
                    .Include(x => x.RISCO_APR.Select(a => a.RISCO).Select(a => a.RISCO_INVENTARIO_AMBIENTE))
                    .Include(x => x.RISCO_APR.Select(a => a.RISCO).Select(a => a.RISCO_INVENTARIO_ATIVIDADE))
                    .Include(x => x.RISCO_APR.Select(a => a.RISCO).Select(a => a.RISCO_INVENTARIO_AMBIENTE.Select(b => b.INVENTARIO_AMBIENTE)))
                    .Include(x => x.RISCO_APR.Select(a => a.RISCO).Select(a => a.RISCO_INVENTARIO_ATIVIDADE.Select(b => b.INVENTARIO_ATIVIDADE)))
                    .Include(x => x.RISCO_APR.Select(a => a.RISCO).Select(a => a.RISCO_INVENTARIO_AMBIENTE.Select(b => b.INVENTARIO_AMBIENTE.LOCAL_INSTALACAO)))
                    .Include(x => x.RISCO_APR.Select(a => a.RISCO).Select(a => a.RISCO_INVENTARIO_ATIVIDADE.Select(b => b.INVENTARIO_ATIVIDADE.LOCAL_INSTALACAO_INVENTARIO_ATIVIDADE)))
                    .Where(x => x.Ativo).ToList();

                if (dados.LOCAL_INSTALACAO != null && dados.LOCAL_INSTALACAO.Count > 0)
                {
                    foreach (var itemLocal in dados.LOCAL_INSTALACAO)
                    {
                        var localRecuperado = entities.LOCAL_INSTALACAO.Where(a => a.CodLocalInstalacao == itemLocal).FirstOrDefault();
                        var resultadoAprLocal = PesquisarPorLocalInstalacao(localRecuperado.CodLocalInstalacao);
                        resultadoLocal.AddRange(resultadoAprLocal);
                    }
                }

                if (dados.RISCO_GERAL != null && dados.RISCO_GERAL.Count > 0)
                {
                    resultadoRiscoGeral = resultado.Where(apr => dados.RISCO_GERAL.Contains(apr.RiscoGeral.Value)).ToList();
                }

                if (dados.RISCO_APR != null && dados.RISCO_APR.Count > 0)
                {
                    resultadoRiscoApr = resultado.Where(apr => apr.RISCO_APR.Any(a => dados.RISCO_APR.Contains(a.CodRisco))).ToList();
                }

                foreach (var itemSeveridade in resultadoLocal)
                    resultadoFinal.Add(itemSeveridade);

                foreach (var itemRiscoGeral in resultadoRiscoGeral)
                    resultadoFinal.Add(itemRiscoGeral);

                foreach (var itemRisco in resultadoRiscoApr)
                    resultadoFinal.Add(itemRisco);

                return resultado;
            }
        }

        public void DesativarApr(long codAprExistente)
        {
            try
            {
                using (DB_LaborSafetyEntities entities = new DB_LaborSafetyEntities())
                {
                    APR aprExistente = entities.APR.Where(apr => apr.CodAPR == codAprExistente).FirstOrDefault();

                    aprExistente.Ativo = false;

                    entities.SaveChanges();
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public long BuscarPessoaPorAprovador(long codAprovador, long codApr)
        {
            using (var entities = new DB_LaborSafetyEntities())
            {
                var resultado = entities.APROVADOR_APR
                 .Where(x => x.CodAprovadorAPR == codAprovador && x.CodAPR == codApr)
                 .FirstOrDefault();

                var aprovador = resultado.CodPessoa;

                return aprovador;
            }
        }

        public long BuscarPessoaPorExecutante(long codExecutante, long codApr)
        {
            using (var entities = new DB_LaborSafetyEntities())
            {
                var resultado = entities.EXECUTANTE_APR
                 .Where(x => x.CodExecutanteAPR == codExecutante && x.CodAPR == codApr)
                 .FirstOrDefault();

                var executante = resultado.CodPessoa;

                return executante;
            }
        }

        public List<RISCO> BuscarTodosOsRiscos(long codAPR, bool riscosAtivos, DB_LaborSafetyEntities entities = null)
        {
            if (entities == null)
                entities = new DB_LaborSafetyEntities();

            var riscosAPR = entities.RISCO_APR.Where(x => x.CodAPR == codAPR && x.Ativo == riscosAtivos).ToList();

            List<RISCO> result = new List<RISCO>();

            foreach (var riscoAPR in riscosAPR)
            {
                var risco = entities.RISCO.Where(x => x.CodRisco == riscoAPR.CodRisco).FirstOrDefault();
                result.Add(risco);
            }

            return result;
        }
    }
}