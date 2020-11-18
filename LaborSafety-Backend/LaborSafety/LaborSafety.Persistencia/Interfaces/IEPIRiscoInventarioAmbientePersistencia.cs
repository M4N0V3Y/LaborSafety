
using System.Collections.Generic;
using LaborSafety.Dominio.Modelos;

namespace LaborSafety.Persistencia.Interfaces
{
    public interface IEPIRiscoInventarioAmbientePersistencia
    {
        List<EPI_RISCO_INVENTARIO_AMBIENTE> ListarEPIPorRisco(long codRisco);
    }
}
