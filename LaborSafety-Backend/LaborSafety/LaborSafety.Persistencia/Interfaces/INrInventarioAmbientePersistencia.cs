using System.Collections.Generic;
using LaborSafety.Dominio.Modelos;

namespace LaborSafety.Persistencia.Interfaces
{
    public interface INrInventarioAmbientePersistencia
    {
        void InserirNrInventarioAmbiente(NrInventarioAmbienteModelo nrInventarioAmbienteModelo);
    }
}
