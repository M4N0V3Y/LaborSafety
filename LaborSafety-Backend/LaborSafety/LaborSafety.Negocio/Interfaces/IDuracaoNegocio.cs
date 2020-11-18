using System.Collections.Generic;
using LaborSafety.Dominio.Modelos;

namespace LaborSafety.Negocio.Interfaces
{
    public interface IDuracaoNegocio
    {
        DuracaoModelo ListarDuracaoPorId(long id);
        DuracaoModelo ListarDuracaoPorIndice(int indice);
        DuracaoModelo ListarDuracaoPorNome(string nome);
        IEnumerable<DuracaoModelo> ListarTodasDuracoes();
    }
}
