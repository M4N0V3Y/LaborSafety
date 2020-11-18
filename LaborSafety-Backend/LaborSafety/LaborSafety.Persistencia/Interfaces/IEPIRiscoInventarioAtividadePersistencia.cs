
using System.Collections.Generic;
using LaborSafety.Dominio.Modelos;

namespace LaborSafety.Persistencia.Interfaces
{
    public interface IEPIRiscoInventarioAtividadePersistencia
    {
        List<EPI_RISCO_INVENTARIO_ATIVIDADE> ListarEPIPorRisco(long codInventario);
    }
}
