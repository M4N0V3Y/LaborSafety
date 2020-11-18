using System.Collections.Generic;
using System.Linq;
using LaborSafety.Dominio.Modelos;
using LaborSafety.Persistencia.Interfaces;

namespace LaborSafety.Persistencia.Servicos
{
    public class EPIRiscoInventarioAtividadePersistencia : IEPIRiscoInventarioAtividadePersistencia
    {
        private readonly IDbLaborSafetyEntities databaseEntities;

        public EPIRiscoInventarioAtividadePersistencia(IDbLaborSafetyEntities databaseEntities)
        {
            this.databaseEntities = databaseEntities;
        }

        public List<EPI_RISCO_INVENTARIO_ATIVIDADE> ListarEPIPorRisco(long codInventario)
        {
            using (var entities = new DB_LaborSafetyEntities())
            {
                var epi = entities.EPI_RISCO_INVENTARIO_ATIVIDADE
                    .Where(epiAmb => epiAmb.CodEpiRiscoInventarioAtividade == codInventario).ToList();
                return epi;
            }
        }
    }
}