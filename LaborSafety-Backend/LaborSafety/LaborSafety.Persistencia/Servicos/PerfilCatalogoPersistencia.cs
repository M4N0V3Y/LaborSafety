using System;
using System.Collections.Generic;
using System.Linq;
using LaborSafety.Dominio.Modelos;
using LaborSafety.Persistencia.Interfaces;
using System.Data.Entity;
using LaborSafety.Utils.Constantes;

namespace LaborSafety.Persistencia.Servicos
{
    public class PerfilCatalogoPersistencia : IPerfilCatalogoPersistencia
    {
        private readonly IDbLaborSafetyEntities databaseEntities;
        public PerfilCatalogoPersistencia(IDbLaborSafetyEntities databaseEntities)
        {
            this.databaseEntities = databaseEntities;
        }

        public void Inserir(PerfilCatalogoModelo modelo, DB_LaborSafetyEntities entities = null)
        {
            if (entities == null)
                entities = new DB_LaborSafetyEntities();

            var perfilCatalogo = new PERFIL_CATALOGO()
            {
                Codigo = modelo.Codigo,
                Nome = modelo.Nome,
                Idioma = "P",
                Mdt = 200,
            };

            entities.PERFIL_CATALOGO.Add(perfilCatalogo);
            entities.SaveChanges();

        }

        public void Editar(PerfilCatalogoModelo modelo, DB_LaborSafetyEntities entities = null)
        {
            if (entities == null)
                entities = new DB_LaborSafetyEntities();

            //Verifica se perfil existe
            var perfilCatalogoExistente = this.ListarPerfilCatalogoPorCodigo(modelo.Codigo, entities);

            if (perfilCatalogoExistente == null)
                throw new Exception("O perfil de catálogo informado não existe!");

            perfilCatalogoExistente.Codigo = modelo.Codigo;
            perfilCatalogoExistente.Nome = modelo.Nome;
            perfilCatalogoExistente.Idioma = "P";
            perfilCatalogoExistente.Mdt = 200;

            entities.SaveChanges();

        }

        public void Excluir(PerfilCatalogoModelo modelo)
        {
            using (var entities = new DB_LaborSafetyEntities())
            {
                //Verifica se perfil existe
                var perfilCatalogoExistente = this.ListarPerfilCatalogoPorCodigo(modelo.Codigo, entities);

                if (perfilCatalogoExistente == null)
                    throw new Exception("O perfil de catálogo informado não existe!");


                perfilCatalogoExistente.Ativo = false;

                entities.SaveChanges();
            }
        }

        public PERFIL_CATALOGO ListarPerfilCatalogoPorId(long id, DB_LaborSafetyEntities entities = null)
        {
            if(entities == null)
                entities = new DB_LaborSafetyEntities();

            PERFIL_CATALOGO perfilCatalogo = entities.PERFIL_CATALOGO
                .Where(perf => perf.CodPerfilCatalogo == id).FirstOrDefault();
            return perfilCatalogo;

        }

        public PERFIL_CATALOGO ListarPerfilCatalogoPorCodigo(string codigo, DB_LaborSafetyEntities entities = null)
        {
            if (entities == null)
                entities = new DB_LaborSafetyEntities();


            PERFIL_CATALOGO perfilCatalogo = entities.PERFIL_CATALOGO
                .Where(perf => perf.Codigo == codigo && perf.CodPerfilCatalogo != (long)Constantes.PerfilCatalogo.SEM_PERFIL_CATALOGO).FirstOrDefault();
            return perfilCatalogo;

        }

        public PERFIL_CATALOGO ListarPerfilCatalogoPorNome(string nome)
        {
            using (var entities = new DB_LaborSafetyEntities())
            {
                PERFIL_CATALOGO perfilCatalogo = entities.PERFIL_CATALOGO
                    .Where(perf => perf.Nome == nome && perf.CodPerfilCatalogo != (long)Constantes.PerfilCatalogo.SEM_PERFIL_CATALOGO).FirstOrDefault();
                return perfilCatalogo;
            }
        }

        public IEnumerable<PERFIL_CATALOGO> ListarTodosPCs()
        {
            using (var entities = new DB_LaborSafetyEntities())
            {
                var resultado = entities.PERFIL_CATALOGO
                    .Where(perf => perf.CodPerfilCatalogo != (long)Constantes.PerfilCatalogo.SEM_PERFIL_CATALOGO).OrderBy(x => x.Codigo)
                    .ToList();

                return resultado;
            }
        }

        public List<PERFIL_CATALOGO> ListarTodosPCsExportacao(List<long> pcs)
        {
            using (var entities = new DB_LaborSafetyEntities())
            {
                var resultado = entities.PERFIL_CATALOGO.Where(p => pcs.Contains(p.CodPerfilCatalogo) && p.CodPerfilCatalogo != (long)Constantes.PerfilCatalogo.SEM_PERFIL_CATALOGO).ToList();

                return resultado;
            }
        }
    }
}
