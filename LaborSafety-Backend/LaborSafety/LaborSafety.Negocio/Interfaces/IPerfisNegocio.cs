using System;
using System.Collections.Generic;
using LaborSafety.Dominio.Modelos;

namespace LaborSafety.Negocio.Interfaces
{
    public interface IPerfisNegocio
    {
        IEnumerable<PerfilModelo> ListarTodosOsTiposPerfis();
        IEnumerable<PerfilModelo> ObterPerfisPor8ID(String id);
        PerfilModelo ListarPerfilPorId(long id);
        List<TelaModelo> ListarListaTelasEFuncionalidadesPorPerfil(long codPerfil);
        TelaModelo ListarTelaEFuncionalidadesPorPerfilETela(long codPerfil, long codTela);
        bool Insercao(PerfilFuncionalidadeModelo perfilFuncionalidadeModelo);
    }
}
