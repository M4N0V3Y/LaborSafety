using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaborSafety.Dominio.Modelos;
using LaborSafety.Negocio.Interfaces;
using LaborSafety.Persistencia;
using LaborSafety.Persistencia.Interfaces;

namespace LaborSafety.Negocio.Servicos
{
    public class PessoaNegocio : IPessoaNegocio
    {
        private readonly IPessoaPersistencia pessoaPersistencia;

        public PessoaNegocio(IPessoaPersistencia pessoaPersistencia)
        {
            this.pessoaPersistencia = pessoaPersistencia;
        }

        public bool Inserir(PessoaModelo pessoa)
        {
            return this.pessoaPersistencia.Inserir(pessoa);
        }

        public void Editar(PessoaModelo pessoa)
        {
            this.pessoaPersistencia.Editar(pessoa);
        }

        public PessoaModelo MapeamentoPessoa(PESSOA pessoa)
        {
            PessoaModelo pessoaModelo = new PessoaModelo()
            {
                CodPessoa = pessoa.CodPessoa,
                Matricula = pessoa.Matricula,
                Nome = pessoa.Nome,
                CPF = pessoa.CPF,
                Telefone = pessoa.Telefone,
                Email = pessoa.Email,
                Empresa = pessoa.Empresa,
                Ativo = pessoa.Ativo
            };

            return pessoaModelo;
        }

        public List<PessoaModelo> Listar()
        {
            List<PessoaModelo> pessoaModelo = new List<PessoaModelo>();

            List<PESSOA> pessoas = this.pessoaPersistencia.Listar();

            if (pessoas == null)
            {
                throw new KeyNotFoundException("Pessoa não encontrada.");
            }

            foreach (PESSOA pessoa in pessoas)
            {
                pessoaModelo.Add(MapeamentoPessoa(pessoa));
            }

            return pessoaModelo;
        }

        public PessoaModelo ListarPorCodigo(long codigo)
        {
            PessoaModelo pessoaModelo = new PessoaModelo();
            AutoMapper.Mapper.Map(this.pessoaPersistencia.ListarPorCodigo(codigo), pessoaModelo);
            if (codigo == 0)
                throw new KeyNotFoundException("Pessoa não encontrada.");
            return pessoaModelo;
        }
        public PessoaModelo ListarPorCpf(string cpf)
        {
            PessoaModelo pessoaModelo = new PessoaModelo();
            AutoMapper.Mapper.Map(this.pessoaPersistencia.ListarPorCpf(cpf), pessoaModelo);
            if (cpf == null || cpf == "")
                throw new KeyNotFoundException("Pessoa não encontrada.");
            return pessoaModelo;
        }

        public IEnumerable<PessoaModelo> ListarPorCodigos(List<long> codigos)
        {
            List<PessoaModelo> pessoas = new List<PessoaModelo>();

            IEnumerable<PESSOA> pes = this.pessoaPersistencia.ListarPorCodigos(codigos);

            if (pes == null)
            {
                throw new KeyNotFoundException("Pessoa não encontrada.");
            }

            foreach (PESSOA item in pes)
            {
                pessoas.Add(MapeamentoPessoa(item));
            }

            return pessoas;
        }

        public void Excluir(long codPessoa)
        {
            this.pessoaPersistencia.ValidarPessoaComAprAssociada(codPessoa);

            this.pessoaPersistencia.Excluir(codPessoa);
        }
    }
}
