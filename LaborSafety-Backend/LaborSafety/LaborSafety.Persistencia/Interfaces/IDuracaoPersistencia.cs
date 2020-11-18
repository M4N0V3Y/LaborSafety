using System.Collections.Generic;

namespace LaborSafety.Persistencia.Interfaces
{
    public interface IDuracaoPersistencia
    {
        DURACAO ListarDuracaoPorId(long id);
        DURACAO ListarDuracaoPorIndice(int indice);
        DURACAO ListarDuracaoPorNome(string nome);
        IEnumerable<DURACAO> ListarTodasDuracoes();
        List<DURACAO> ListarTodasDuracoesExportacao(List<long> duracaoes);
    }
}
