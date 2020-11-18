using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaborSafety.Dominio.Modelos;
using LaborSafety.Persistencia.Interfaces;

namespace LaborSafety.Persistencia.Servicos
{
    public class RascunhoAprPersistencia : IRascunhoAprPersistencia
    {
        private readonly IDbLaborSafetyEntities databaseEntities;
        public RascunhoAprPersistencia(IDbLaborSafetyEntities databaseEntities)
        {
            this.databaseEntities = databaseEntities;
        }

        public void InserirRascunhoApr(RascunhoAprModelo rascunhoAprModelo)
        {
            using (var entities = databaseEntities.GetDB_LaborSafetyEntities())
            {
                using (var transaction = entities.Database.BeginTransaction())
                {
                    try
                    {
                        this.Inserir(rascunhoAprModelo, entities);
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

        public void Inserir(RascunhoAprModelo rascunhoAprModelo, DB_LaborSafetyEntities entities)
        {
            List<OperacaoAprModelo> operacaoApr = rascunhoAprModelo.OPERACAO_APR;
            List<AprovadorAprModelo> aprovadores = rascunhoAprModelo.APROVADOR_APR;
            List<ExecutanteAprModelo> executores = rascunhoAprModelo.EXECUTANTE_APR;
            List<PessoaModelo> pessoas = rascunhoAprModelo.PESSOA;

            try
            {
                var apr = new APR()
                {
                    CodStatusAPR = rascunhoAprModelo.CodStatusAPR,
                    NumeroSerie = rascunhoAprModelo.OrdemManutencao,
                    OrdemManutencao = rascunhoAprModelo.OrdemManutencao,
                    Descricao = rascunhoAprModelo.Descricao,
                    RiscoGeral = rascunhoAprModelo.RiscoGeral,
                    DataAprovacao = DateTime.Now,
                    DataInicio = DateTime.Now,
                    DataEncerramento = DateTime.Now,
                    Ativo = true
                };

                entities.APR.Add(apr);
                entities.SaveChanges();

                long id = apr.CodAPR;

                if (operacaoApr != null)
                {
                    foreach (var atividade in operacaoApr)
                    {
                        var codLocalInstalacao = entities.LOCAL_INSTALACAO.Where(lc => lc.Nome == atividade.NomeLI).FirstOrDefault();

                        entities.OPERACAO_APR.Add(new OPERACAO_APR()
                        {
                            CodAPR = id,
                            CodStatusAPR = atividade.CodStatusAPR,
                            Codigo = atividade.Codigo,
                            Descricao = atividade.Descricao,
                            CodLI = codLocalInstalacao.CodLocalInstalacao,
                            CodDisciplina = atividade.CodDisciplina,
                            CodAtvPadrao = atividade.CodAtvPadrao
                        });
                        //InserirRiscoApr(id, codLocalInstalacao.CodLocalInstalacao, entities);
                    }
                }

                foreach (var aprovador in aprovadores)
                {
                    //InsereEditaPessoa(entities, pessoas, true, id, aprovador.CodPessoa);
                }

                foreach (var executor in executores)
                {
                    //InsereEditaPessoa(entities, pessoas, false, id, executor.CodPessoa);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
