using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaborSafety.Dominio.Modelos;

namespace LaborSafety.Persistencia.Interfaces
{
    public interface ITelaPersistencia
    {
        List<TelaModelo> Listar();
        TelaModelo ListarPorCodigo(long codigo);
    }
}
