using System.Collections.Generic;
using System.Linq;
using LaborSafety.Persistencia.Interfaces;

namespace LaborSafety.Persistencia.Servicos
{
    public class FuncionalidadePersistencia : IFuncionalidadesPersistencia
    {
        private readonly IDbLaborSafetyEntities databaseEntities;
        public FuncionalidadePersistencia(IDbLaborSafetyEntities databaseEntities)
        {
            this.databaseEntities = databaseEntities;
        }

        public IEnumerable<FUNCIONALIDADE> ListarTodasFuncionalidades()
        {
            using (var entities = new DB_LaborSafetyEntities())
            {
                IEnumerable<FUNCIONALIDADE> funcionalidades = entities.FUNCIONALIDADE.ToList();
                return funcionalidades;
            }
        }

        public FUNCIONALIDADE ListarFuncionalidadePorId(long id)
        {
            using (var entities = databaseEntities.GetDB_LaborSafetyEntities())
            {
                FUNCIONALIDADE funcionalidade = entities.FUNCIONALIDADE.Where(func => func.CodFuncionalidade == id).First();
                return funcionalidade;
            }
        }

        public List<PERFIL_FUNCIONALIDADE> ListarFuncionalidadesPor8ID(string c8id)
        {
            using (var entities = databaseEntities.GetDB_LaborSafetyEntities())
            {
                // Gera array de funcionalidades para determinado usuário a partir de seu 8ID
                var funcionalidadesPor8ID = entities.USUARIO
                    .Where(user => user.C8ID == c8id)
                    .Join(entities.USUARIO_PERFIL, usuario => usuario.CodUsuario, up => up.CodUsuario, (usuario, up) => new { usuario, up })
                    .Join(entities.PERFIL_FUNCIONALIDADE, up => up.up.CodPerfil, perfi_func => perfi_func.CodPerfil, (up, perfi_func) => new { up, perfi_func })
                    .Select(x => x.perfi_func)
                    .ToList();
                return funcionalidadesPor8ID;
            }
        }
    }
}
