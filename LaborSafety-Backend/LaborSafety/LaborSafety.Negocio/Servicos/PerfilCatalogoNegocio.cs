using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaborSafety.Dominio.Modelos;
using LaborSafety.Negocio.Interfaces;
using LaborSafety.Negocio.Validadores.Interface;
using LaborSafety.Persistencia;
using LaborSafety.Persistencia.Interfaces;

namespace LaborSafety.Negocio.Servicos
{
    public class PerfilCatalogoNegocio : IPerfilCatalogoNegocio
    {
        private readonly IPerfilCatalogoPersistencia perfilCatalogoPersistencia;
        public PerfilCatalogoNegocio(IPerfilCatalogoPersistencia perfilCatalogoPersistencia)
        {
            this.perfilCatalogoPersistencia = perfilCatalogoPersistencia;
        }

        public PerfilCatalogoModelo MapeamentoPerfilCatalogo(PERFIL_CATALOGO perfilCat)
        {
            PerfilCatalogoModelo perfilCatalogo = new PerfilCatalogoModelo()
            {
                CodPerfilCatalogo = perfilCat.CodPerfilCatalogo,
                Codigo = perfilCat.Codigo,
                Nome = perfilCat.Nome,
                Idioma = perfilCat.Idioma,
                Mdt = perfilCat.Mdt
            };

            return perfilCatalogo;
        }

        public PerfilCatalogoModelo ListarPerfilCatalogoPorId(long id)
        {
            PERFIL_CATALOGO sis = this.perfilCatalogoPersistencia.ListarPerfilCatalogoPorId(id);
            if (sis == null)
            {
                throw new KeyNotFoundException("Perfil de catálogo não encontrado.");
            }
            return MapeamentoPerfilCatalogo(sis);
        }

        public PerfilCatalogoModelo ListarPerfilCatalogoPorNome(string nome)
        {
            PERFIL_CATALOGO sis = this.perfilCatalogoPersistencia.ListarPerfilCatalogoPorNome(nome);
            if (sis == null)
            {
                throw new KeyNotFoundException("Perfil de catálogo não encontrado.");
            }
            return MapeamentoPerfilCatalogo(sis);
        }

        public PerfilCatalogoModelo ListarPerfilCatalogoPorCodigo(string codigo)
        {
            PERFIL_CATALOGO sis = this.perfilCatalogoPersistencia.ListarPerfilCatalogoPorCodigo(codigo);
            if (sis == null || codigo == null)
            {
                throw new KeyNotFoundException("Perfil de catálogo não encontrado.");
            }
            return MapeamentoPerfilCatalogo(sis);
        }

        public IEnumerable<PerfilCatalogoModelo> ListarTodosPCs()
        {
            List<PerfilCatalogoModelo> perfilCatalogoModelo = new List<PerfilCatalogoModelo>();

            IEnumerable<PERFIL_CATALOGO> pcs = this.perfilCatalogoPersistencia.ListarTodosPCs();

            if (pcs == null)
            {
                throw new KeyNotFoundException("Perfil de catálogo não encontrado.");
            }

            foreach (PERFIL_CATALOGO pc in pcs)
            {
                perfilCatalogoModelo.Add(MapeamentoPerfilCatalogo(pc));
            }

            return perfilCatalogoModelo;
        }
    }
}
