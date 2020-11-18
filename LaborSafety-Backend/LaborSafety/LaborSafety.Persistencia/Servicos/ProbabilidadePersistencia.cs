using System.Collections.Generic;
using System.Linq;
using LaborSafety.Persistencia.Interfaces;

namespace LaborSafety.Persistencia.Servicos
{

    //probabilidade = frequencia
    public class ProbabilidadePersistencia : IProbabilidadePersistencia
    {
        private readonly IDbLaborSafetyEntities databaseEntities;
        public ProbabilidadePersistencia(IDbLaborSafetyEntities databaseEntities)
        {
            this.databaseEntities = databaseEntities;
        }

        public PROBABILIDADE ListarProbabilidadePorId(long id)
        {
            using (var entities = new DB_LaborSafetyEntities())
            {
                PROBABILIDADE probabilidade = entities.PROBABILIDADE
                    .Where(prob => prob.CodProbabilidade == id).FirstOrDefault();
                return probabilidade;
            }
        }

        public PROBABILIDADE ListarProbabilidadePorNome(string nome)
        {
            using (var entities = new DB_LaborSafetyEntities())
            {
                PROBABILIDADE probabilidade = entities.PROBABILIDADE
                    .Where(prob => prob.Nome == nome).FirstOrDefault();
                return probabilidade;
            }
        }

        public IEnumerable<PROBABILIDADE> ListarTodasProbabilidades()
        {
            using (var entities = new DB_LaborSafetyEntities())
            {
                var resultado = entities.PROBABILIDADE
                    .ToList();

                return resultado;
            }
        }

        public List<PROBABILIDADE> ListarTodasProbabilidadesExportacao(List<long> probabilidades)
        {
            using (var entities = new DB_LaborSafetyEntities())
            {
                var resultado = entities.PROBABILIDADE.Where(p => probabilidades.Contains(p.CodProbabilidade)).ToList();

                return resultado;
            }
        }
    }
}
