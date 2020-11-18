

using System.Collections.Generic;
using LaborSafety.Dominio.Modelos;

namespace LaborSafety.Persistencia.Interfaces
{
    public interface IPerfilCatalogoPersistencia
    {
        void Inserir(PerfilCatalogoModelo modelo, DB_APRPTEntities entities = null);
        void Editar(PerfilCatalogoModelo modelo, DB_APRPTEntities entities = null);

        PERFIL_CATALOGO ListarPerfilCatalogoPorId(long id, DB_APRPTEntities entities = null);

        PERFIL_CATALOGO ListarPerfilCatalogoPorNome(string nome);
        PERFIL_CATALOGO ListarPerfilCatalogoPorCodigo(string codigo, DB_APRPTEntities entities = null);

        IEnumerable<PERFIL_CATALOGO> ListarTodosPCs();
        List<PERFIL_CATALOGO> ListarTodosPCsExportacao(List<long> pcs);
    }
}
