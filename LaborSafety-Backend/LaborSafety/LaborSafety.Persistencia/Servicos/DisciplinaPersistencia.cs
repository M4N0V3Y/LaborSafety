using System;
using System.Collections.Generic;
using System.Linq;
using LaborSafety.Dominio.Modelos;
using LaborSafety.Persistencia.Interfaces;
using System.Data.Entity;
using LaborSafety.Utils.Constantes;

namespace LaborSafety.Persistencia.Servicos
{
    public class DisciplinaPersistencia : IDisciplinaPersistencia
    {
        private readonly IDbLaborSafetyEntities databaseEntities;
        public DisciplinaPersistencia(IDbLaborSafetyEntities databaseEntities)
        {
            this.databaseEntities = databaseEntities;
        }

        public void Inserir(DisciplinaModelo modelo)
        {
            using (var entities = new DB_LaborSafetyEntities())
            {
                var disciplina = new DISCIPLINA()
                {
                    Nome = modelo.Nome,
                    Descricao = modelo.Descricao,
                    Ativo = true
                };

                entities.DISCIPLINA.Add(disciplina);
                entities.SaveChanges();
            }
        }

        public void InsereOuAtualiza(DisciplinaModelo modelo)
        {
            var disciplinaExistente = this.ListarDisciplinaPorNome(modelo.Descricao);
            
            if (disciplinaExistente == null)
                this.Inserir(modelo);
            else
                this.Editar(modelo);
        }

        public void Editar(DisciplinaModelo modelo)
        {
            using (var entities = new DB_LaborSafetyEntities())
            {
                //Verifica se a disciplina existe
                var disciplinaExistente = this.ListarDisciplinaPorNome(modelo.Descricao);

                if (disciplinaExistente == null)
                    throw new Exception("A disciplina informada não existe!");

                disciplinaExistente.Nome = modelo.Descricao;
                disciplinaExistente.Descricao = modelo.Descricao;

                entities.SaveChanges();
            }
        }

        public void ExcluirDisciplina(DisciplinaModelo modelo)
        {
            using (var entities = new DB_LaborSafetyEntities())
            {
                //Verifica se a disciplina existe
                var disciplinaExistente = this.ListarDisciplinaPorNome(modelo.Nome);

                if (disciplinaExistente == null)
                    throw new Exception("A disciplina informada não existe!");

                disciplinaExistente.Ativo = false;

                entities.SaveChanges();
            }
        }

        public DISCIPLINA ListarDisciplinaPorId(long id, DB_LaborSafetyEntities entities = null)
        {
            if (entities == null)
                entities = new DB_LaborSafetyEntities();

            DISCIPLINA disciplina = entities.DISCIPLINA
                    .Where(sist => sist.CodDisciplina == id).FirstOrDefault();

            return disciplina;
        }

        public DISCIPLINA ListarDisciplinaPorNome(string nome, DB_LaborSafetyEntities entities = null)
        {
            if (entities == null)
                entities = new DB_LaborSafetyEntities();

            DISCIPLINA disciplina = entities.DISCIPLINA
                .Where(sist => sist.Nome.ToUpper() == nome.ToUpper()).FirstOrDefault();
            return disciplina;

        }

        public IEnumerable<DISCIPLINA> ListarTodasDisciplinas()
        {
            using (var entities = new DB_LaborSafetyEntities())
            {
                var resultado = entities.DISCIPLINA
                    .ToList();

                return resultado;
            }
        }
    }
}
