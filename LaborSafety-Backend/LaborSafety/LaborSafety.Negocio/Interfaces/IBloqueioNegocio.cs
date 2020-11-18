using System.Collections.Generic;
using LaborSafety.Dominio.Modelos;

namespace LaborSafety.Negocio.Interfaces
{
    public interface IBloqueioNegocio
    {
        List<string> ListarBloqueioPorListaLIPorArea(DadosMapaBloqueioAprModelo dados);
    }
}
