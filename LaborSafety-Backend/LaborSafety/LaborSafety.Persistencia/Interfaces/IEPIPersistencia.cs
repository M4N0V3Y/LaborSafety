
using System.Collections.Generic;
using LaborSafety.Dominio.Modelos;

namespace LaborSafety.Persistencia.Interfaces
{
    public interface IEPIPersistencia
    {
        EPI ListarEPIPorId(long id);
        EPI ListarEPIPorNome(string nome);
        IEnumerable<EPI> ListarTodosEPIs();
        EPI ListarEPIPorNivel(string nomeCompleto);
        List<EPIModelo> ListarTodosEPIPorNivel(string nomeCompleto);
        List<EPI> ListarEPIsPorListaId(List<long> id);
        EPI ListarEPIPorIdString(string id);
    }
}
