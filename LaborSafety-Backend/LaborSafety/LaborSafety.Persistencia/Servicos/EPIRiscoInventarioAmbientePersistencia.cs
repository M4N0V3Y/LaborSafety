using System.Collections.Generic;
using System.Linq;
using LaborSafety.Dominio.Modelos;
using LaborSafety.Persistencia.Interfaces;

namespace LaborSafety.Persistencia.Servicos
{
    public class EPIRiscoInventarioAmbientePersistencia : IEPIRiscoInventarioAmbientePersistencia
    {
        private readonly IDbLaborSafetyEntities databaseEntities;

        public EPIRiscoInventarioAmbientePersistencia(IDbLaborSafetyEntities databaseEntities)
        {
            this.databaseEntities = databaseEntities;
        }

        public List<EPI_RISCO_INVENTARIO_AMBIENTE> ListarEPIPorRisco(long codRisco)
        {
            using (var entities = new DB_LaborSafetyEntities())
            {
                var epi = entities.EPI_RISCO_INVENTARIO_AMBIENTE
                    .Where(epiAmb => epiAmb.CodRiscoInventarioAmbiente == codRisco).ToList();
                return epi;
            }
        }
    }
}