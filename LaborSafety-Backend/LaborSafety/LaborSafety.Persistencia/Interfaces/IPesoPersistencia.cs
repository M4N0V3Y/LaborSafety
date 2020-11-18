using System.Collections.Generic;

namespace LaborSafety.Persistencia.Interfaces
{
    public interface IPesoPersistencia
    {
        PESO ListarPesoPorId(long id);
        PESO ListarPesoPorIndice(int indice);
        PESO ListarPesoPorNome(string nome, DB_APRPTEntities entities = null);
        IEnumerable<PESO> ListarTodosPesos();
        List<PESO> ListarTodosPesosExportacao(List<long> pesos);
    }
}
