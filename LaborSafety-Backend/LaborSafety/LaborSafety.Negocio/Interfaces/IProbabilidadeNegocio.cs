using System.Collections.Generic;
using LaborSafety.Dominio.Modelos;

namespace LaborSafety.Negocio.Interfaces
{
    public interface IProbabilidadeNegocio
    {
        ProbabilidadeModelo ListarProbabilidadePorId(long id);
        ProbabilidadeModelo ListarProbabilidadePorNome(string nome);
        IEnumerable<ProbabilidadeModelo> ListarTodasProbabilidades();
    }
}
