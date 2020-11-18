using System.Collections.Generic;
using LaborSafety.Dominio.Modelos;

namespace LaborSafety.Negocio.Interfaces
{
    public interface IEpiNegocio
    {
        EPIModelo ListarEPIPorID(long id);
        EPIModelo ListarEPIPorNome(string nome);
        IEnumerable<EPIModelo> ListarTodosEPIs();
        EPIModelo ListarEPIPorNivel(string nomeCompleto);
        List<EPIModelo> ListarTodosEPIPorNivel(string nomeCompleto);
        List<EPIModelo> ListarEPIsPorListaId(List<long> epis);

    }
}
