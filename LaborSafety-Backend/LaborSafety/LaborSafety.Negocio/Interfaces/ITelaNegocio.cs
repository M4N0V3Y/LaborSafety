using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaborSafety.Dominio.Modelos;
using LaborSafety.Persistencia;

namespace LaborSafety.Negocio.Interfaces
{
    public interface ITelaNegocio
    {
        List<TelaModelo> Listar();
        TelaModelo ListarPorCodigo(long codigo);
    }
}
