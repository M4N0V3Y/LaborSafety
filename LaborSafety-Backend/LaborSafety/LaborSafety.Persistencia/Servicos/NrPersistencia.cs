using System.Collections.Generic;
using System.Linq;
using LaborSafety.Persistencia.Interfaces;

namespace LaborSafety.Persistencia.Servicos
{
    public class NrPersistencia : INrPersistencia
    {
        private readonly IDbLaborSafetyEntities databaseEntities;
        public NrPersistencia(IDbLaborSafetyEntities databaseEntities)
        {
            this.databaseEntities = databaseEntities;
        }

        public NR ListarNrPorId(long id)
        {
            using (var entities = new DB_LaborSafetyEntities())
            {
                NR nr = entities.NR
                    .Where(pes => pes.CodNR == id).FirstOrDefault();
                return nr;
            }
        }

        public NR ListarNrPorIdString(string id)
        {
            using (var entities = new DB_LaborSafetyEntities())
            {
                NR nr = entities.NR
                    .Where(pes => pes.CodNR.ToString() == id).FirstOrDefault();
                return nr;
            }
        }

        public NR ListarNRPorCodigo(string codigo)
        {
            using (var entities = new DB_LaborSafetyEntities())
            {
                NR nr = entities.NR
                    .Where(pes => pes.Codigo == codigo).FirstOrDefault();
                return nr;
            }
        }

        public IEnumerable<NR> ListarTodasNRs()
        {
            using (var entities = new DB_LaborSafetyEntities())
            {
                var resultado = entities.NR
                    .ToList();

                return resultado;
            }
        }
    }
}
