using System.Collections.Generic;
using LaborSafety.Dominio.Modelos;

namespace LaborSafety.Negocio.Interfaces
{
    public interface IRiscoNegocio
    {
        RiscoModelo ListarRiscoPorId(long id);
        RiscoModelo ListarRiscoPorNome(string nome);
        IEnumerable<RiscoModelo> ListarTodosRiscos();
        IEnumerable<RiscoModelo> ListarPorTipoRisco(long codTipo);
        List<TipoRiscoModelo> ListarTiposRisco();
    }
}
