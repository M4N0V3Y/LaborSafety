

using System.Collections.Generic;

namespace LaborSafety.Persistencia.Interfaces
{
    public interface IProbabilidadePersistencia
    {
        PROBABILIDADE ListarProbabilidadePorId(long id);
        PROBABILIDADE ListarProbabilidadePorNome(string nome);
        IEnumerable<PROBABILIDADE> ListarTodasProbabilidades();
        List<PROBABILIDADE> ListarTodasProbabilidadesExportacao(List<long> probabilidades);
    }
}
