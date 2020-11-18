using System.Collections.Generic;
using LaborSafety.Dominio.Modelos;

namespace LaborSafety.Negocio.Interfaces
{
    public interface IFuncionalidadesNegocio
    {
        IEnumerable<FuncionalidadeModelo> ListarTodasFuncionalidades();
        FuncionalidadeModelo ListarFuncionalidadePorId(long id);
        FuncionalidadesPor8IdModelo ListarFuncionalidadesPor8ID(string c8id);
    }
}
