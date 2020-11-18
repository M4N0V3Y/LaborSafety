using System.Collections.Generic;

namespace LaborSafety.Persistencia.Interfaces
{
    public interface INrPersistencia
    {
        NR ListarNrPorId(long id);
        NR ListarNrPorIdString(string id);
        NR ListarNRPorCodigo(string codigo);
        IEnumerable<NR> ListarTodasNRs();
    }
}
