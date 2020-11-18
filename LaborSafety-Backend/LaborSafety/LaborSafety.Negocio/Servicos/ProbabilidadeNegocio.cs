using System.Collections.Generic;
using LaborSafety.Dominio.Modelos;
using LaborSafety.Negocio.Interfaces;
using LaborSafety.Negocio.Validadores.Interface;
using LaborSafety.Persistencia;
using LaborSafety.Persistencia.Interfaces;

namespace LaborSafety.Negocio.Servicos
{
    public class ProbabilidadeNegocio : IProbabilidadeNegocio
    {
        private readonly IProbabilidadePersistencia probabilidadePersistencia;
        public ProbabilidadeNegocio(IProbabilidadePersistencia probabilidadePersistencia)
        {
            this.probabilidadePersistencia = probabilidadePersistencia;
        }

        public ProbabilidadeModelo MapeamentoProbabilidade(PROBABILIDADE probabilidade)
        {
            ProbabilidadeModelo probabilidadeModelo = new ProbabilidadeModelo()
            {
                CodProbabilidade = probabilidade.CodProbabilidade,
                Peso = probabilidade.Peso,
                Nome = probabilidade.Nome
            };

            return probabilidadeModelo;
        }

        public ProbabilidadeModelo ListarProbabilidadePorId(long id)
        {
            PROBABILIDADE prob = this.probabilidadePersistencia.ListarProbabilidadePorId(id);
            if (prob == null)
            {
                throw new KeyNotFoundException("Probabilidade não encontrada.");
            }
            return MapeamentoProbabilidade(prob);
        }

        public ProbabilidadeModelo ListarProbabilidadePorNome(string nome)
        {
            PROBABILIDADE prob = this.probabilidadePersistencia.ListarProbabilidadePorNome(nome);
            if (prob == null)
            {
                throw new KeyNotFoundException("Probabilidade não encontrada.");
            }
            return MapeamentoProbabilidade(prob);
        }

        public IEnumerable<ProbabilidadeModelo> ListarTodasProbabilidades()
        {
            List<ProbabilidadeModelo> probabilidadeModelo = new List<ProbabilidadeModelo>();

            IEnumerable<PROBABILIDADE> probabilidades = this.probabilidadePersistencia.ListarTodasProbabilidades();

            if (probabilidades == null)
            {
                throw new KeyNotFoundException("Probabilidade não encontrada.");
            }

            foreach (PROBABILIDADE prob in probabilidades)
            {
                probabilidadeModelo.Add(MapeamentoProbabilidade(prob));
            }

            return probabilidadeModelo;
        }
    }
}
