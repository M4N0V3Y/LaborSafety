using System.Collections.Generic;

namespace LaborSafety.Persistencia.Interfaces
{
    public interface IFuncionalidadesPersistencia
    {
        IEnumerable<FUNCIONALIDADE> ListarTodasFuncionalidades();
        FUNCIONALIDADE ListarFuncionalidadePorId(long id);
        List<PERFIL_FUNCIONALIDADE> ListarFuncionalidadesPor8ID(string c8id);
    }
}
