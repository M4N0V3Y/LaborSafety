

using System.Collections.Generic;

namespace LaborSafety.Persistencia.Interfaces
{
    public interface ISeveridadePersistencia
    {
        SEVERIDADE ListarSeveridadePorId(long id);
        SEVERIDADE ListarSeveridadePorNome(string nome);
        IEnumerable<SEVERIDADE> ListarTodasSeveridades();
    }
}
