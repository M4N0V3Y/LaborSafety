using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaborSafety.Dominio.Modelos;
using static LaborSafety.Negocio.Servicos.RascunhoInventarioAmbienteNegocio;

namespace LaborSafety.Negocio.Interfaces
{
    public interface IRascunhoInventarioAmbienteNegocio
    {
        RascunhoInventarioAmbienteModelo ListarRascunhoInventarioAmbientePorId(long id);
        RascunhoInventarioAmbienteModelo ListarRascunhoInventarioAmbientePorLI(long li);
        IEnumerable<RascunhoInventarioAmbienteModelo> ListarRascunhoInventarioAmbiente(FiltroInventarioAmbienteModelo filtroInventarioAmbienteModelo);
        RetornoInsercao InserirRascunhoInventarioAmbiente(RascunhoInventarioAmbienteModelo rascunhoInventarioAmbienteModelo);
        Retorno EditarRascunhoInventarioAmbiente(RascunhoInventarioAmbienteModelo rascunhoInventarioAmbienteModelo);
        void ExcluirRascunhoInventarioAmbiente(long id);
    }
}
