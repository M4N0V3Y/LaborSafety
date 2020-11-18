using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaborSafety.Dominio.Modelos;

namespace LaborSafety.Negocio.Interfaces
{
    public interface IOperacaoAprNegocio
    {
        IEnumerable<OperacaoAprModelo> ListarPorCodigos(List<long> codigos);
    }
}
