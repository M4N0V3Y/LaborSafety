using System.Collections.Generic;
using LaborSafety.Dominio.Modelos;

namespace LaborSafety.Negocio.Interfaces
{
    public interface IPesoNegocio
    {
        PesoModelo ListarPesoPorId(long id);
        PesoModelo ListarPesoPorIndice(int indice);
        PesoModelo ListarPesoPorNome(string nome);
        IEnumerable<PesoModelo> ListarTodosPesos();
    }
}
