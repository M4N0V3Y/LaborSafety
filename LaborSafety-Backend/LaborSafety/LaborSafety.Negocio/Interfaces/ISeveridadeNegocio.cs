using System.Collections.Generic;
using LaborSafety.Dominio.Modelos;

namespace LaborSafety.Negocio.Interfaces
{
    public interface ISeveridadeNegocio
    {
        SeveridadeModelo ListarSeveridadePorId(long id);
        SeveridadeModelo ListarSeveridadePorNome(string nome);
        IEnumerable<SeveridadeModelo> ListarTodasSeveridades();
    }
}
