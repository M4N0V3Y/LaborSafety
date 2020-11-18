using System.Collections.Generic;
using LaborSafety.Dominio.Modelos;

namespace LaborSafety.Negocio.Interfaces
{
    public interface IAtividadePadraoNegocio
    {
        AtividadePadraoModelo ListarAtividadePorId(long id);
        AtividadePadraoModelo ListarAtividadePorNome(string nome);
        IEnumerable<AtividadePadraoModelo> ListarTodasAtividades(); 
    }
}
