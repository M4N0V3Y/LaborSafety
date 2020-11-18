using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaborSafety.Persistencia.Interfaces
{
    public interface ILogPerfilFuncionalidadePersistencia
    {
        LOG_PERFIL_FUNCIONALIDADE Inserir(LOG_PERFIL_FUNCIONALIDADE modelo, DB_APRPTEntities entities = null);
        LOG_PERFIL_FUNCIONALIDADE BuscaEInsereFuncionalidades(string eightIDUsuarioModificador, long codPerfil, long codTela,
            List<FUNCIONALIDADE> novasFuncionalidades, DB_APRPTEntities entities = null);

        List<LOG_PERFIL_FUNCIONALIDADE> BuscarTodos(DB_APRPTEntities entities = null);
        List<LOG_PERFIL_FUNCIONALIDADE> BuscarPorPerfil(long codPerfil, DB_APRPTEntities entities = null);
    }
}
