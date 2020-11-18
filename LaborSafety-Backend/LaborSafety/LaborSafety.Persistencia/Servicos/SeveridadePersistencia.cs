using System.Collections.Generic;
using System.Linq;
using LaborSafety.Persistencia.Interfaces;

namespace LaborSafety.Persistencia.Servicos
{
    public class SeveridadePersistencia : ISeveridadePersistencia
    {
        private readonly IDbLaborSafetyEntities databaseEntities;
        public SeveridadePersistencia(IDbLaborSafetyEntities databaseEntities)
        {
            this.databaseEntities = databaseEntities;
        }

        public SEVERIDADE ListarSeveridadePorId(long id)
        {
            using (var entities = new DB_LaborSafetyEntities())
            {
                SEVERIDADE severidade = entities.SEVERIDADE
                    .Where(sev => sev.CodSeveridade == id).FirstOrDefault();
                return severidade;
            }
        }

        public SEVERIDADE ListarSeveridadePorNome(string nome)
        {
            using (var entities = new DB_LaborSafetyEntities())
            {
                SEVERIDADE severidade = entities.SEVERIDADE
                    .Where(sev => sev.Nome == nome).FirstOrDefault();
                return severidade;
            }
        }

        public IEnumerable<SEVERIDADE> ListarTodasSeveridades()
        {
            using (var entities = new DB_LaborSafetyEntities())
            {
                var resultado = entities.SEVERIDADE
                    .ToList();

                return resultado;
            }
        }
    }
}
