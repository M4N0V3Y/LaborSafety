using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaborSafety.Dominio.Modelos;

namespace LaborSafety.Persistencia.Interfaces
{
    public interface IPerfisPersistencia
    {
        IEnumerable<PERFIL> ListarTodosOsTiposPerfis();
        PERFIL ListarTipoPerfil(long id);
        IEnumerable<PERFIL> ObterPerfisPor8ID(String id);
        List<TelaModelo> ListarListaTelasEFuncionalidadesPorPerfil(long codPerfil);
        TelaModelo ListarTelaEFuncionalidadesPorPerfilETela(long codPerfil, long codTela);
        bool Insercao (PerfilFuncionalidadeModelo perfilFuncionalidadeModelo);
    }
}
