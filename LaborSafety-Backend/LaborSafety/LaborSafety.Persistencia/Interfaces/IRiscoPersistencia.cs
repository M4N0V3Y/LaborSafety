using System.Collections.Generic;
using LaborSafety.Dominio.Modelos;

namespace LaborSafety.Persistencia.Interfaces
{
    public interface IRiscoPersistencia
    {
        RISCO ListarRiscoPorId(long id);
        RISCO ListarRiscoPorNome(string nome);
        IEnumerable<RISCO> ListarTodosRiscos();
        IEnumerable<RISCO> ListarPorTipoRisco(long codTipo);
        List<TIPO_RISCO> ListarTiposRisco();
        RISCO ListarRiscoPorNomeETipo(string nome, long? tipo);
    }
}
