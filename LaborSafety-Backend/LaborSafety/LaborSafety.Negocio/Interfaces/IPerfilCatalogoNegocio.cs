using System.Collections.Generic;
using LaborSafety.Dominio.Modelos;

namespace LaborSafety.Negocio.Interfaces
{
    public interface IPerfilCatalogoNegocio
    {
        PerfilCatalogoModelo ListarPerfilCatalogoPorId(long id);
        PerfilCatalogoModelo ListarPerfilCatalogoPorNome(string nome);
        PerfilCatalogoModelo ListarPerfilCatalogoPorCodigo(string codigo);
        IEnumerable<PerfilCatalogoModelo> ListarTodosPCs();
    }
}
