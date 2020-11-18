using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaborSafety.Dominio.Modelos;

namespace LaborSafety.Persistencia.Interfaces
{
    public interface IMapeamentoLocalInventarioAmbientePersistencia
    {
        List<INVENTARIO_AMBIENTE> InserirInventarioAmbienteComLocalInstalacao(List<InventarioAmbienteModelo> inventarios, DB_APRPTEntities entities);
    }
}
