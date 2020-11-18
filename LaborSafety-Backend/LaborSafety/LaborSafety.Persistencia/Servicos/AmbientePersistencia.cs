using System;
using System.Collections.Generic;
using System.Linq;
using LaborSafety.Dominio.Modelos;
using LaborSafety.Persistencia.Interfaces;
using System.Data.Entity;
using LaborSafety.Utils.Constantes;

namespace LaborSafety.Persistencia.Servicos
{
    public class AmbientePersistencia : IAmbientePersistencia
    {
        private readonly IDbLaborSafetyEntities databaseEntities;
        public AmbientePersistencia(IDbLaborSafetyEntities databaseEntities)
        {
            this.databaseEntities = databaseEntities;
        }

        public AMBIENTE Inserir(AmbienteModelo ambienteModelo)
        {
            try
            {
                using (var entities = new DB_LaborSafetyEntities())
                {
                    var ambiente = new AMBIENTE()
                    {
                        Nome = ambienteModelo.Nome,
                        Descricao = ambienteModelo.Descricao,
                        Ativo = true
                };

                    entities.AMBIENTE.Add(ambiente);
                    entities.SaveChanges();

                    return ambiente;
                }
            }

            catch (Exception exception)
            {
                throw exception;
            }
        }

        public AMBIENTE Editar(AmbienteModelo ambienteModelo)
        {
            try
            {
                using (var entities = new DB_LaborSafetyEntities())
                {
                    var ambienteExistente = entities.AMBIENTE.Where(x => x.CodAmbiente == ambienteModelo.CodAmbiente).FirstOrDefault();

                    ambienteExistente.Nome = ambienteModelo.Nome;
                    ambienteExistente.Descricao = ambienteModelo.Descricao;
                    ambienteExistente.Ativo = true;

                    entities.SaveChanges();

                    return ambienteExistente;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public AMBIENTE ListarAmbienteSemInventarioAmbienteVinculado(long id)
        {
            using (var entities = new DB_LaborSafetyEntities())
            {
                AMBIENTE sistemaOperacional = entities.AMBIENTE
                    .Include(x => x.INVENTARIO_AMBIENTE)
                    .Where(sist => sist.CodAmbiente == id && sist.CodAmbiente != (long)Constantes.Ambiente.SEM_AMBIENTE && sist.INVENTARIO_AMBIENTE.All(y => y.CodAmbiente != id)).FirstOrDefault();

                return sistemaOperacional;
            }
        }

        public AMBIENTE ListarSistemaOperacionalPorId(long id)
        {
            using (var entities = new DB_LaborSafetyEntities())
            {
                AMBIENTE sistemaOperacional = entities.AMBIENTE
                    .Where(sist => sist.CodAmbiente == id && sist.CodAmbiente != (long)Constantes.Ambiente.SEM_AMBIENTE && sist.Ativo).FirstOrDefault();
                return sistemaOperacional;
            }
        }

        public AMBIENTE ListarSistemaOperacionalPorNome(string nome)
        {
            using (var entities = new DB_LaborSafetyEntities())
            {
                AMBIENTE sistemaOperacional = entities.AMBIENTE
                    .Where(sist => sist.Nome == nome && sist.CodAmbiente != (long)Constantes.Ambiente.SEM_AMBIENTE && sist.Ativo).FirstOrDefault();
                return sistemaOperacional;
            }
        }

        public IEnumerable<AMBIENTE> ListarTodosSOs()
        {
            using (var entities = new DB_LaborSafetyEntities())
            {
                var resultado = entities.AMBIENTE
                    .Where(sist => sist.CodAmbiente != (long)Constantes.Ambiente.SEM_AMBIENTE && sist.Ativo)
                    .ToList();

                return resultado;
            }
        }

        public void DesativarAmbiente(long codAmbienteExistente)
        {
            using (DB_LaborSafetyEntities entities = new DB_LaborSafetyEntities())
            {
                AMBIENTE ambienteExistente = entities.AMBIENTE.Where(amb => amb.CodAmbiente == codAmbienteExistente).FirstOrDefault();

                if (ambienteExistente == null)
                    throw new Exception("O ambiente informado não foi encontrado na base de dados.");

                ambienteExistente.Ativo = false;

                entities.SaveChanges();
            }
        }
    }
}
