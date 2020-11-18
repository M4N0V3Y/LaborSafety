using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaborSafety.Dominio.Modelos;
using LaborSafety.Persistencia.Interfaces;
using LaborSafety.Utils.Constantes;
using System.Data.Entity;

namespace LaborSafety.Persistencia.Servicos
{
    public class PessoaPersistencia : IPessoaPersistencia
    {

        private readonly IDbLaborSafetyEntities databaseEntities;
        public PessoaPersistencia(IDbLaborSafetyEntities databaseEntities)
        {
            this.databaseEntities = databaseEntities;
        }

        public bool Inserir(PessoaModelo pessoa)
        {
            using (var entities = databaseEntities.GetDB_LaborSafetyEntities())
            {
                using (var transaction = entities.Database.BeginTransaction())
                {
                    try
                    {
                        entities.PESSOA.Add(new PESSOA()
                        {
                            Ativo = true,
                            CodPessoa = pessoa.CodPessoa,
                            CPF = pessoa.CPF,
                            Email = pessoa.Email,
                            Empresa = pessoa.Empresa,
                            Matricula = pessoa.Matricula,
                            Nome = pessoa.Nome,
                            Telefone = pessoa.Telefone
                        });

                        entities.SaveChanges();
                        transaction.Commit();
                    }
                    catch (Exception exception)
                    {
                        transaction.Rollback();
                        throw exception;
                    }
                }
            }

            return true;
        }

        public void Editar(PessoaModelo pessoaModelo)
        {
            using (DB_LaborSafetyEntities entities = new DB_LaborSafetyEntities())
            {
                PESSOA pessoaExistente = entities.PESSOA.Where(p => p.CodPessoa == pessoaModelo.CodPessoa && p.Ativo).FirstOrDefault();

                if (pessoaExistente == null)
                    throw new KeyNotFoundException();

                pessoaExistente.CodPessoa = pessoaModelo.CodPessoa;
                pessoaExistente.Matricula = pessoaModelo.Matricula;
                pessoaExistente.Nome = pessoaModelo.Nome;
                pessoaExistente.CPF = pessoaModelo.CPF;
                pessoaExistente.Telefone = pessoaModelo.Telefone;
                pessoaExistente.Email = pessoaModelo.Email;
                pessoaExistente.Empresa = pessoaModelo.Empresa;

                entities.SaveChanges();
            }
        }

        public void Excluir(long codPessoa)
        {
            using (DB_LaborSafetyEntities entities = new DB_LaborSafetyEntities())
            {
                var pessoaAExcluir = entities.PESSOA.Where(pessoa => pessoa.CodPessoa == codPessoa).FirstOrDefault();

                if(pessoaAExcluir == null)
                {
                    throw new KeyNotFoundException();
                }

                pessoaAExcluir.Ativo = Convert.ToBoolean(Constantes.Status.INATIVO);

                entities.SaveChanges();
            }
        }

        public List<PESSOA> Listar()
        {
            using (var entities = new DB_LaborSafetyEntities())
            {
                var resultado = entities.PESSOA
                    .Where(x => x.Ativo)
                    .ToList();

                return resultado;
            }
        }

        public PESSOA ListarPorCodigo(long codigo)
        {
            using (var entities = new DB_LaborSafetyEntities())
            {
                PESSOA pessoa = entities.PESSOA.Where(p => p.CodPessoa == codigo && p.Ativo).FirstOrDefault();

                if (pessoa == null)
                    throw new KeyNotFoundException();

                return pessoa;
            }
        }

        public List<PESSOA> ListarPorCodigos(List<long> codigo)
        {
            using (var entities = new DB_LaborSafetyEntities())
            {
                var resultado = entities.PESSOA.Where(p => codigo.Contains(p.CodPessoa) && p.Ativo).ToList();

                return resultado;
            }
        }

        public long ValidarPessoaComAprAssociada(long codPessoa)
        {
            using (var entities = new DB_LaborSafetyEntities())
            {
                var aprovador = (from pessoa in entities.PESSOA
                           join aprov in entities.APROVADOR_APR on pessoa.CodPessoa equals aprov.CodPessoa
                           join aop in entities.OPERACAO_APR on aprov.CodAPR equals aop.CodAPR
                           join apr in entities.APR on aop.CodAPR equals apr.CodAPR
                           where aprov.CodPessoa == codPessoa
                           select apr.NumeroSerie).FirstOrDefault();

                var executante = (from pessoa in entities.PESSOA
                           join exec in entities.EXECUTANTE_APR on pessoa.CodPessoa equals exec.CodPessoa
                           join aop in entities.OPERACAO_APR on exec.CodAPR equals aop.CodAPR
                           join apr in entities.APR on aop.CodAPR equals apr.CodAPR
                                  where exec.CodPessoa == codPessoa
                                  select apr.NumeroSerie).FirstOrDefault();

                if (!string.IsNullOrEmpty(aprovador) || !string.IsNullOrEmpty(executante))
                    throw new Exception("Essa pessoa possui uma ou mais APRs associada!");

                return 0;
            }
        }

        public PESSOA ListarPorCpf(string cpf)
        {
            using (var entities = new DB_LaborSafetyEntities())
            {
                PESSOA pessoa = entities.PESSOA
                    .Where(x => x.CPF == cpf && x.Ativo)
                    .FirstOrDefault();

                if (pessoa == null)
                {
                    throw new KeyNotFoundException();
                }

                return pessoa;
            }
        }
    }

}
