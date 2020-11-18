using System.Collections.Generic;
using LaborSafety.Dominio.Modelos;

namespace LaborSafety.Negocio.Interfaces
{
    public interface ILocalInstalacaoNegocio
    {
        LocalInstalacaoModelo ListarLocalInstalacaoPorId(long id);
        LocalInstalacaoModelo ListarLocalInstalacaoPorNome(string nome);
        List<LocalInstalacaoModelo> ListarLIPorNivel(LocalInstalacaoFiltroModelo filtro);
        IEnumerable<LocalInstalacaoModelo> ListarTodosLIs();
    }
}
