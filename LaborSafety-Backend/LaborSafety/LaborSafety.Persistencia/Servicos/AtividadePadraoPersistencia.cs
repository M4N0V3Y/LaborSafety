using System;
using System.Collections.Generic;
using System.Linq;
using LaborSafety.Dominio.Modelos;
using LaborSafety.Persistencia.Interfaces;
using System.Data.Entity;

namespace LaborSafety.Persistencia.Servicos
{
    public class AtividadePadraoPersistencia : IAtividadePadraoPersistencia
    {
        private readonly IDbLaborSafetyEntities databaseEntities;
        public AtividadePadraoPersistencia(IDbLaborSafetyEntities databaseEntities)
        {
            this.databaseEntities = databaseEntities;
        }

        public void Inserir(AtividadePadraoModelo modelo)
        {
            using (var entities = new DB_LaborSafetyEntities())
            {
                var atividadePadrao = new ATIVIDADE_PADRAO()
                {
                    Nome = modelo.Nome,
                    Descricao = modelo.Descricao,
                    Ativo = true
                };

                entities.ATIVIDADE_PADRAO.Add(atividadePadrao);
                entities.SaveChanges();
            }
        }

        public void Editar(AtividadePadraoModelo modelo)
        {
            using (var entities = new DB_LaborSafetyEntities())
            {
                //Verifica se a atividade existe
                var atividadeExistente = this.ListarAtividadePorNome(modelo.Nome);

                if (atividadeExistente == null)
                    throw new Exception("A atividade informada não existe!");

                atividadeExistente.Nome = modelo.Nome;
                atividadeExistente.Descricao = modelo.Descricao;


                entities.SaveChanges();
            }
        }

        public void ExcluirAtividade(AtividadePadraoModelo modelo)
        {
            using (var entities = new DB_LaborSafetyEntities())
            {
                //Verifica se a disciplina existe
                var atividadeExistente = this.ListarAtividadePorNome(modelo.Nome);

                if (atividadeExistente == null)
                    throw new Exception("A atividade informada não existe!");

                atividadeExistente.Ativo = false;

                entities.SaveChanges();
            }
        }

        public ATIVIDADE_PADRAO ListarAtividadePorId(long id, DB_LaborSafetyEntities entities = null)
        {
            if (entities == null)
                entities = new DB_LaborSafetyEntities();

            ATIVIDADE_PADRAO atividade = entities.ATIVIDADE_PADRAO
                    .Where(sist => sist.CodAtividadePadrao == id).FirstOrDefault();

            return atividade;

        }

        public ATIVIDADE_PADRAO ListarAtividadePorNome(string nome, DB_LaborSafetyEntities entities = null)
        {
            if (entities == null)
                entities = new DB_LaborSafetyEntities();

            ATIVIDADE_PADRAO atividade = entities.ATIVIDADE_PADRAO
                .Where(sist => sist.Nome.ToUpper() == nome.ToUpper()).FirstOrDefault();
            return atividade;

        }

        public IEnumerable<ATIVIDADE_PADRAO> ListarTodasAtividades()
        {
            using (var entities = new DB_LaborSafetyEntities())
            {
                var resultado = entities.ATIVIDADE_PADRAO
                    .ToList();

                return resultado;
            }
        }

        public List<ATIVIDADE_PADRAO> ListarTodasAtividadesExportacao(List<long> atividades)
        {
            using (var entities = new DB_LaborSafetyEntities())
            {
                var resultado = entities.ATIVIDADE_PADRAO.Where(p => atividades.Contains(p.CodAtividadePadrao)).ToList();

                return resultado;
            }
        }
    }
}
