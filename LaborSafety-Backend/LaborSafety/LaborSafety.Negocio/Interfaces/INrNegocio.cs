using System.Collections.Generic;
using LaborSafety.Dominio.Modelos;

namespace LaborSafety.Negocio.Interfaces
{
    public interface INrNegocio
    {
        NrModelo ListarNrPorId(long id);
        NrModelo ListarNRPorCodigo(string codigo);
        IEnumerable<NrModelo> ListarTodasNRs();
    }
}
